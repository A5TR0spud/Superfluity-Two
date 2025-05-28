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
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.SuperSniper
{
    public class SuperSniperHoldout : ModProjectile
    {
        private static Asset<Texture2D> texAsset;

        public override string Texture => $"{nameof(SuperfluityTwo)}/Content/Items/Weapons/Ranged/SuperSniper/SuperSniper";

        ref float useTimer => ref Projectile.ai[0];
        ref float dustTimer => ref Projectile.ai[1];

        public override void Load()
        {
            texAsset = ModContent.Request<Texture2D>(Texture, AssetRequestMode.AsyncLoad);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;

            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 84;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = ProjAIStyleID.HeldProjectile;
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
            ResetUseTimer();
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, false);

            UpdatePlayerVisuals(player, rrp);

            bool stillInUse = player.channel && !player.noItems && !player.CCed;
            if (Projectile.owner == Main.myPlayer)
            {
                UpdateAim(rrp, player.HeldItem.shootSpeed);

                if (!stillInUse)
                {
                    Projectile.Kill();
                    return;
                }
            }

            if (stillInUse)
            {
                HandleFiring();
            }
            Projectile.timeLeft = 2;
        }

        private void HandleFiring()
        {
            Player player = Main.player[Projectile.owner];
            float chargeRatio = (GetDefaultUseTimer() - useTimer) / GetDefaultUseTimer();
            Vector2 dir = Projectile.velocity.SafeNormalize(-Vector2.UnitY);
            dustTimer += 30.0f * chargeRatio;
            if (useTimer <= 0 && player.whoAmI == Main.myPlayer && player.PickAmmo(player.HeldItem, out int proj, out float speed, out int damage, out float kB, out int itemID))
            {
                Projectile.NewProjectile(
                    player.GetSource_FromThis(),
                    Main.MouseWorld,
                    dir * speed,
                    ModContent.ProjectileType<SuperSniperReticle>(),
                    damage,
                    kB,
                    player.whoAmI
                );
                ResetUseTimer();
                Projectile.netUpdate = true;
            }
            while (dustTimer > 0)
            {
                Dust.NewDustPerfect(
                    Projectile.Center + dir * 30,
                    DustID.GoldFlame,
                    dir + 0.3f * new Vector2(Main.rand.Next() - 0.5f, Main.rand.Next() - 0.5f)
                ).noGravity = true;
                dustTimer--;
            }
            useTimer--;
        }

        private void ResetUseTimer()
        {
            useTimer = GetDefaultUseTimer();
        }

        private int GetDefaultUseTimer()
        {
            Player player = Main.player[Projectile.owner];
            return (int)(player.HeldItem.useTime / player.GetWeaponAttackSpeed(player.HeldItem));
        }


        private void UpdateAim(Vector2 source, float speed)
        {
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = Projectile.velocity.SafeNormalize(-Vector2.UnitY);
            }

            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            float rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction;

            Vector2 holdoutOffset = new Vector2(0, -34);
            holdoutOffset.Y += -0.5f;

            Projectile.Center = playerHandPos + new Vector2(holdoutOffset.X * Projectile.direction, holdoutOffset.Y).RotatedBy(Projectile.rotation);
            Projectile.rotation = rotation;

            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            Texture2D texture = texAsset.Value;
            int frameHeight = texture.Height;
            int spriteSheetOffset = frameHeight * Projectile.frame;
            Vector2 sheetInsertPosition = (Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();

            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight - 2)), drawColor, Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, effects, 0f);
            return false;
        }
    }
}