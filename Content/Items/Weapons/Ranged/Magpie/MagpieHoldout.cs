using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Magpie
{
    public class MagpieHoldout : ModProjectile
    {
        private static Asset<Texture2D> texAsset;

        public override string Texture => $"{nameof(SuperfluityTwo)}/Content/Items/Weapons/Ranged/Magpie/Magpie";

        ref float FrameCounter => ref Projectile.ai[0];
        public Vector2 overrideAim = Vector2.Zero;
        public int overrideAimTime = 0;

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

        public override void OnSpawn(IEntitySource source)
        {
            overrideAimTime = 0;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
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
            FrameCounter++;
        }

        private void HandleFiring()
        {
            Player player = Main.player[Projectile.owner];
            if (player.whoAmI == Main.myPlayer && FrameCounter == 1)
            {
                int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI);
                Projectile.NewProjectile(
                    player.GetSource_FromThis(),
                    Main.MouseWorld,
                    Vector2.Zero,
                    ModContent.ProjectileType<MagpieReticle>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    player.whoAmI,
                    uuid
                );
                Projectile.netUpdate = true;
            }
        }

        private void UpdateAim(Vector2 source, float speed)
        {
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (overrideAimTime > 0)
            {
                aim = overrideAim;
            }
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
            overrideAimTime--;
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            float rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction;

            Vector2 holdoutOffset = new Vector2(5, -20);

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
            
            Vector2 sheetInsertPosition = (Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();
            
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(
                texture,
                sheetInsertPosition,
                new Rectangle?(new Rectangle(0, 0, texture.Width, frameHeight)),
                drawColor,
                Projectile.rotation,
                new Vector2(texture.Width / 2f, frameHeight / 2f),
                Projectile.scale * 0.85f,
                effects,
                0f
            );
            return false;
        }
    }
}