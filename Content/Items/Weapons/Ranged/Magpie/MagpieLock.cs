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
            FireTimer = Magpie.USE_TIME;
            /*if (Main.myPlayer != Projectile.owner)
            {
                Projectile.Opacity = 0;
            }*/
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
        }

        private void UpdateAim()
        {
            Player owner = Main.player[Projectile.owner];
            int targetID = (int)TrackedNPC;
            NPC target = Main.npc[targetID];
            if (Main.myPlayer == Projectile.owner)
            {
                if (!MagpieReticle.trackedNPCs.Contains(targetID))
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.width = target.width;
                Projectile.height = target.height;
                Projectile.Center = target.Center;
            }
            if (FireTimer >= Magpie.USE_TIME)
            {
                if (!owner.PickAmmo(owner.HeldItem, out int proj, out float speed, out int dmg, out float kB, out int ammoItemID))
                {
                    Projectile.Kill();
                    return;
                }
                ///#TODO: Seriously revamp this to scan basically everything periodically and save the best scan result
                Vector2 aimpointCenter      = target.Center;
                Vector2 aimpointTop         = target.Top         + new Vector2(0, 2);
                Vector2 aimpointTopRight    = target.TopRight    + new Vector2(-2, 2);
                Vector2 aimpointRight       = target.Right       + new Vector2(-2, 0);
                Vector2 aimpointBottomRight = target.BottomRight + new Vector2(-2, -2);
                Vector2 aimpointBottom      = target.Bottom      + new Vector2(0, -2);
                Vector2 aimpointBottomLeft  = target.BottomLeft  + new Vector2(2, -2);
                Vector2 aimpointLeft        = target.Left        + new Vector2(2, 0);
                Vector2 aimpointTopLeft     = target.TopLeft     + new Vector2(2, 2);

                Vector2 randomSamplePoint1 = new Vector2(
                    MathHelper.Lerp(aimpointTopLeft.X, aimpointBottomRight.X, Main.rand.NextFloat()),
                    MathHelper.Lerp(aimpointTopLeft.Y, aimpointBottomRight.Y, Main.rand.NextFloat())
                );
                Vector2 randomSamplePoint2 = new Vector2(
                    MathHelper.Lerp(aimpointTopLeft.X, aimpointBottomRight.X, Main.rand.NextFloat()),
                    MathHelper.Lerp(aimpointTopLeft.Y, aimpointBottomRight.Y, Main.rand.NextFloat())
                );
                Vector2 randomSamplePoint3 = new Vector2(
                    MathHelper.Lerp(aimpointTopLeft.X, aimpointBottomRight.X, Main.rand.NextFloat()),
                    MathHelper.Lerp(aimpointTopLeft.Y, aimpointBottomRight.Y, Main.rand.NextFloat())
                );

                bool canHitCenter = HelperMethodsSF2.RaycastReliable(owner.Center, aimpointCenter);
                bool canHitTopLeft = HelperMethodsSF2.RaycastReliable(owner.Center, aimpointTopLeft);
                bool canHitTopRight = HelperMethodsSF2.RaycastReliable(owner.Center, aimpointTopRight);
                bool canHitBottomLeft = HelperMethodsSF2.RaycastReliable(owner.Center, aimpointBottomLeft);
                bool canHitBottomRight = HelperMethodsSF2.RaycastReliable(owner.Center, aimpointBottomRight);
                bool canHitTop = HelperMethodsSF2.RaycastReliable(owner.Center, aimpointTop);
                bool canHitLeft = HelperMethodsSF2.RaycastReliable(owner.Center, aimpointLeft);
                bool canHitRight = HelperMethodsSF2.RaycastReliable(owner.Center, aimpointRight);
                bool canHitBottom = HelperMethodsSF2.RaycastReliable(owner.Center, aimpointBottom);

                bool canHitRandom1 = HelperMethodsSF2.RaycastReliable(owner.Center, randomSamplePoint1);
                bool canHitRandom2 = HelperMethodsSF2.RaycastReliable(owner.Center, randomSamplePoint2);
                bool canHitRandom3 = HelperMethodsSF2.RaycastReliable(owner.Center, randomSamplePoint3);

                bool tryToFire =
                    canHitCenter ||
                    canHitTopLeft ||
                    canHitTopRight ||
                    canHitBottomLeft ||
                    canHitBottomRight ||
                    canHitTop ||
                    canHitLeft ||
                    canHitRight ||
                    canHitBottom ||
                    canHitRandom1 ||
                    canHitRandom2 ||
                    canHitRandom3
                ;
                if (tryToFire)
                {
                    Vector2 targetPos = aimpointCenter;
                    if (canHitRandom1 && Main.rand.NextBool())
                    {
                        targetPos = randomSamplePoint1;
                    }
                    else if (canHitCenter)
                    {
                        targetPos = aimpointCenter;
                    }
                    else if (canHitRandom2 && Main.rand.NextBool())
                    {
                        targetPos = randomSamplePoint2;
                    }
                    else if (canHitTop)
                    {
                        targetPos = aimpointTop;
                    }
                    else if (canHitRight)
                    {
                        targetPos = aimpointRight;
                    }
                    else if (canHitLeft)
                    {
                        targetPos = aimpointLeft;
                    }
                    else if (canHitBottom)
                    {
                        targetPos = aimpointBottom;
                    }
                    else if (canHitTopRight)
                    {
                        targetPos = aimpointTopRight;
                    }
                    else if (canHitBottomLeft)
                    {
                        targetPos = aimpointBottomLeft;
                    }
                    else if (canHitTopLeft)
                    {
                        targetPos = aimpointTopLeft;
                    }
                    else if (canHitBottomRight)
                    {
                        targetPos = aimpointBottomRight;
                    }
                    else if (canHitRandom1)
                    {
                        targetPos = randomSamplePoint1;
                    }
                    else if (canHitRandom2)
                    {
                        targetPos = randomSamplePoint2;
                    }
                    else if (canHitRandom3)
                    {
                        targetPos = randomSamplePoint3;
                    }
                    MagpieHoldout hostMagpie = (MagpieHoldout)Main.projectile[(int)HostMagpieIndex].ModProjectile;
                    float dist = owner.Center.Distance(targetPos);
                    Vector2 aimCompensation = target.velocity * dist / speed;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        //int extraUpdates = Main.projectile
                        Projectile.NewProjectile(
                            owner.GetSource_FromThis(),
                            owner.Center,
                            owner.Center.DirectionTo(targetPos + aimCompensation).RotatedByRandom(MathHelper.ToRadians(0.5f)) * speed,
                            proj,
                            dmg,
                            kB,
                            Projectile.owner
                        );
                    }
                    hostMagpie.overrideAim = owner.Center.DirectionTo(targetPos + aimCompensation);
                    hostMagpie.overrideAimTime = Magpie.USE_TIME;
                    SoundEngine.PlaySound(SoundID.Item41, owner.Center);
                }
                FireTimer -= Magpie.USE_TIME;
            }
            FireTimer += owner.GetWeaponAttackSpeed(owner.HeldItem) / (0.75f + 0.25f * owner.ownedProjectileCounts[Type]);
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

            int widthLeftToDraw = Projectile.width - 12;
            while (widthLeftToDraw > 0)
            {
                int widthToChop = Math.Min(widthLeftToDraw, 6);
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopLeft + new Vector2(Projectile.width - widthLeftToDraw - 6, 0) - Main.screenPosition).Floor() + orig,
                    new Rectangle(8, 0, widthToChop, 6),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.BottomLeft + new Vector2(Projectile.width - widthLeftToDraw - 6, -6) - Main.screenPosition).Floor() + orig,
                    new Rectangle(8, 16, widthToChop, 6),
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