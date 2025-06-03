using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Projectiles
{
    public class Hurricane : ModProjectile
    {
        ref float timer => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 85;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Opacity = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            timer = 0;
        }

        public override void AI()
        {
            DoDust();

            if (Main.myPlayer == Projectile.owner)
            {
                TryFire();
            }

            FadeInAndOut();

            timer++;
        }

        private void DoDust()
        {
            if (timer % 5 == 0)
            {
                Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Rain
                );
            }
        }

        private void TryFire()
        {
            if (timer % 7 == 6)
            {
                Player owner = Main.player[Projectile.owner];
                int count = Main.rand.Next(2, 5);
                for (int i = 0; i < count; i++)
                {
                    float speed = 5.5f + Main.rand.NextFloat();
                    Projectile.NewProjectile(
                        owner.GetSource_FromThis(),
                        Projectile.Center,
                        0.5f * Projectile.velocity + Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * speed,
                        ModContent.ProjectileType<HurriRain>(),
                        (int)(Projectile.damage * 0.6f),
                        Projectile.knockBack * 0.5f,
                        Projectile.owner
                    );
                }
            }
        }

        private void FadeInAndOut()
        {
            if (timer < 20)
            {
                Projectile.Opacity = timer / 20f;
            }
            if (Projectile.timeLeft < 20)
            {
                Projectile.Opacity = Projectile.timeLeft / 20f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            if (Projectile.timeLeft > 20) Projectile.timeLeft = 20;
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 18;
            height = 18;
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

            float theta = 0.05f * timer * Projectile.direction;

            SpriteEffects spriteEffects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture, drawPos, null, color * 0.33f, 0.33f * theta, drawOrigin, Projectile.scale * 1.33f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, color * 0.66f, 0.75f * theta, drawOrigin, Projectile.scale * 1.1f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, color * 0.88f, 1.5f * theta, drawOrigin, Projectile.scale, spriteEffects, 0);

            return false;
        }
    }   
}