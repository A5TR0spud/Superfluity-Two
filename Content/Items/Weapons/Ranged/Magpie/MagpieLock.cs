using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SuperfluityTwo.Common;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Magpie
{
    public class MagpieLock : ModProjectile
    {
        public override string Texture => $"{nameof(SuperfluityTwo)}/Content/Items/Weapons/Ranged/Magpie/MagpieLock";
        public ref float HostMagpieIndex => ref Projectile.ai[0];
        public ref float FireTimer => ref Projectile.ai[1];
        public ref float TrackedNPC => ref Projectile.ai[2];
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
            FireTimer = 0;
            if (Main.myPlayer != Projectile.owner)
            {
                Projectile.Opacity = 0;
            }
        }

        /*public override void OnKill(int timeLeft)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("kil"), Color.Red);
        }*/

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile hostMagpie = Main.projectile[(int)HostMagpieIndex];
            if (Projectile.type != ModContent.ProjectileType<MagpieLock>() || !hostMagpie.active || hostMagpie.type != ModContent.ProjectileType<MagpieHoldout>())
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
            FireTimer++;
        }

        private void UpdateAim()
        {
            Player owner = Main.player[Projectile.owner];
            int targetID = (int)TrackedNPC;
            NPC target = Main.npc[targetID];
            if (Main.myPlayer == Projectile.owner)
            {
                if (!MagpieTracker.trackedNPCs.Contains(targetID))
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.width = target.width;
                Projectile.height = target.height;
                Projectile.Center = target.Center;
            }
            int time = (int)(owner.HeldItem.useTime / owner.GetWeaponAttackSpeed(owner.HeldItem));
            if (FireTimer >= time)
            {
                if (!owner.PickAmmo(owner.HeldItem, out int proj, out float speed, out int dmg, out float kB, out int ammoItemID))
                {
                    Projectile.Kill();
                    return;
                }
                if (Collision.CanHitLine(owner.Center, 2, 2, target.Center, 2, 2))
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        //float dist = target.Center.Distance(Projectile.Center);
                        //Vector2 aimCompensation = dist * target.velocity / speed;
                        //int extraUpdates = Main.projectile
                        Projectile.NewProjectile(
                            owner.GetSource_FromThis(),
                            owner.Center,
                            owner.Center.DirectionTo(target.Center) * speed,
                            proj,
                            dmg,
                            kB,
                            Projectile.owner
                        );
                    }
                    SoundEngine.PlaySound(SoundID.Item41, owner.Center);
                }
                FireTimer -= time;
            }
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

            for (int i = 0; i < Projectile.height / 6; i++)
            {
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopLeft + new Vector2(0, 6 * i) - Main.screenPosition).Floor() + orig,
                    new Rectangle(0, 8, 6, 6),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopRight + new Vector2(0, 6 * i) - Main.screenPosition + new Vector2(-6, 0)).Floor() + orig,
                    new Rectangle(40, 8, 6, 6),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
            }

            for (int i = 0; i < Projectile.width / 6; i++)
            {
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopLeft + new Vector2(6 * i, 0) - Main.screenPosition).Floor() + orig,
                    new Rectangle(8, 0, 6, 6),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.BottomLeft + new Vector2(6 * i, 0) - Main.screenPosition + new Vector2(0, -6)).Floor() + orig,
                    new Rectangle(32, 16, 6, 6),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
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

            Main.EntitySpriteDraw(
                texture,
                (Projectile.Center - Main.screenPosition + new Vector2(-3, -3)).Floor() + orig,
                new Rectangle(8, 8, 6, 6),
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