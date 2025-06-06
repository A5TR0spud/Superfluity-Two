using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Magpie
{
    public class MagpieReticle : ModProjectile
    {
        public override string Texture => $"{nameof(SuperfluityTwo)}/Content/Items/Weapons/Ranged/Magpie/MagpieLock";
        public ref float HostMagpieIndex => ref Projectile.ai[0];
        public ref float FrameCounter => ref Projectile.ai[1];
        public static List<int> trackedNPCs = [];
        public ref float NewLockCooldown => ref Projectile.localAI[0];
        const int LOCK_COOLDOWN = 6;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            FrameCounter = 0;
            if (Main.myPlayer == Projectile.owner)
            {
                trackedNPCs = [];
                NewLockCooldown = 0;
            }
            /*else
            {
                Projectile.Opacity = 0;
            }*/
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile hostMagpie = Main.projectile[(int)HostMagpieIndex];
            if (Projectile.type != ModContent.ProjectileType<MagpieReticle>() || !hostMagpie.active || hostMagpie.type != ModContent.ProjectileType<MagpieHoldout>())
            {
                Projectile.Kill();
                return;
            }
            if (hostMagpie.TryGetOwner(out Player owner) && owner.channel)
                Projectile.timeLeft = 2;

            bool stillInUse = player.channel && !player.noItems && !player.CCed;
            if (Projectile.owner == Main.myPlayer)
            {
                UpdateAim();

                if (!stillInUse)
                {
                    Projectile.Kill();
                    return;
                }
            }
            FrameCounter++;
        }

        private void UpdateAim()
        {
            Vector2 newPos = Main.MouseWorld;
            const int maxSize = 16 * 30;
            const int timeToMax = 90;
            Player owner = Main.player[Projectile.owner];
            float x = FrameCounter / (timeToMax / owner.GetWeaponAttackSpeed(owner.HeldItem));
            x = Math.Clamp(x, 0, 1);
            x = 1 - (x - 1) * (x - 1);
            Projectile.width = 12 + (int)MathHelper.Lerp(0, maxSize, x);
            Projectile.height = Projectile.width;
            if (newPos != Projectile.Center)
            {
                Projectile.netUpdate = true;
            }
            Projectile.Center = newPos;

            for (int npcIterate = 0; npcIterate < Main.maxNPCs; npcIterate++)
            {
                NPC target = Main.npc[npcIterate];
                bool flag = new Rectangle((int)Projectile.TopLeft.X, (int)Projectile.TopLeft.Y, Projectile.width, Projectile.height).Contains((int)target.Center.X, (int)target.Center.Y);
                bool canTarget = target.CanBeChasedBy(Projectile) || (target.active && target.damage > 0 && !target.friendly && !target.dontTakeDamage);
                if (canTarget)
                // could be useful: target.aiStyle == NPCAIStyleID.Spell
                {
                    if (flag && !trackedNPCs.Contains(npcIterate) && NewLockCooldown <= 0)
                    {
                        trackedNPCs.Add(npcIterate);
                        Projectile.NewProjectile(
                            spawnSource: owner.GetSource_FromThis(),
                            position: target.position,
                            velocity: Projectile.velocity,
                            Type: ModContent.ProjectileType<MagpieLock>(),
                            Damage: Projectile.damage,
                            KnockBack: Projectile.knockBack,
                            Owner: Projectile.owner,
                            ai0: HostMagpieIndex,
                            ai1: 0,
                            ai2: npcIterate
                        );
                        NewLockCooldown = LOCK_COOLDOWN;
                    }
                }
                if (!flag || !canTarget)
                {
                    trackedNPCs.Remove(npcIterate);
                }
            }
            NewLockCooldown--;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.myPlayer != Projectile.owner) return false;

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 orig = new Vector2(texture.Width / 2, texture.Height / 2);

            Color drawColor = Projectile.GetAlpha(lightColor);

            int heightLeftToDraw = Projectile.height - 12;
            while (heightLeftToDraw > 0)
            {
                int heightToChop = Math.Min(heightLeftToDraw, 6);
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopLeft + new Vector2(0, Projectile.height - heightLeftToDraw - 6) - Main.screenPosition).Floor() + orig,
                    new Rectangle(0, 8, 6, heightToChop),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopRight + new Vector2(0, Projectile.height - heightLeftToDraw - 6) - Main.screenPosition + new Vector2(-6, 0)).Floor() + orig,
                    new Rectangle(40, 8, 6, heightToChop),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                heightLeftToDraw -= heightToChop;
            }

            int widthLeftToDraw = Projectile.width / 3 - 12;
            while (widthLeftToDraw > 0)
            {
                int widthToChop = Math.Min(widthLeftToDraw, 6);
                if (widthLeftToDraw == 7)
                {
                    widthToChop--;
                }
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopLeft + new Vector2(Projectile.width / 3 - widthLeftToDraw - 6, 0) - Main.screenPosition).Floor() + orig,
                    new Rectangle(
                        x: widthLeftToDraw <= 6 ? 16 + 6 - widthToChop : 8,
                        y: 0,
                        width: widthToChop,
                        height: 6
                    ),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopRight + new Vector2(-Projectile.width / 3 + widthLeftToDraw + 6 - widthToChop, 0) - Main.screenPosition).Floor() + orig,
                    new Rectangle(
                        x: widthLeftToDraw <= 6 ? 24 : 32,
                        y: 0,
                        width: widthToChop,
                        height: 6
                    ),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );

                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.BottomLeft + new Vector2(Projectile.width / 3 - widthLeftToDraw - 6, -6) - Main.screenPosition).Floor() + orig,
                    new Rectangle(
                        x: widthLeftToDraw <= 6 ? 16 + 6 - widthToChop : 8,
                        y: 16,
                        width: widthToChop,
                        height: 6
                    ),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.BottomRight + new Vector2(-Projectile.width / 3 + widthLeftToDraw + 6 - widthToChop, -6) - Main.screenPosition).Floor() + orig,
                    new Rectangle(
                        x: widthLeftToDraw <= 6 ? 24 : 32,
                        y: 16,
                        width: widthToChop,
                        height: 6
                    ),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );

                widthLeftToDraw -= widthToChop;
            }

            Main.EntitySpriteDraw(
                texture,
                (Projectile.TopLeft - Main.screenPosition).Floor() + orig,
                new Rectangle(0, 0, 6, 6),
                drawColor,
                Projectile.rotation,
                orig,
                1,
                SpriteEffects.None,
                0f
            );
            Main.EntitySpriteDraw(
                texture,
                (Projectile.TopRight - Main.screenPosition + new Vector2(-6, 0)).Floor() + orig,
                new Rectangle(40, 0, 6, 6),
                drawColor,
                Projectile.rotation,
                orig,
                1,
                SpriteEffects.None,
                0f
            );
            Main.EntitySpriteDraw(
                texture,
                (Projectile.BottomLeft - Main.screenPosition + new Vector2(0, -6)).Floor() + orig,
                new Rectangle(0, 16, 6, 6),
                drawColor,
                Projectile.rotation,
                orig,
                1,
                SpriteEffects.None,
                0f
            );
            Main.EntitySpriteDraw(
                texture,
                (Projectile.BottomRight - Main.screenPosition + new Vector2(-6, -6)).Floor() + orig,
                new Rectangle(40, 16, 6, 6),
                drawColor,
                Projectile.rotation,
                orig,
                1,
                SpriteEffects.None,
                0f
            );
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }
    }
}