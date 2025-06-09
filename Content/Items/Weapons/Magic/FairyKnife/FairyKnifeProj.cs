using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Magic.FairyKnife
{
    public class FairyKnifeProj : ModProjectile
    {
        ref float Timer => ref Projectile.ai[0];
        ref float JumpsLeft => ref Projectile.ai[1];
        //ref float JumpTimer => ref Projectile.ai[2];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (JumpsLeft == 0)
                JumpsLeft = 1;
            else
                JumpsLeft--;


            for (int i = 0; i < 12; i++)
            {
                Vector2 dustDir = (MathHelper.TwoPi * i / 12f).ToRotationVector2();
                Dust.NewDustPerfect(Projectile.Center, DustID.ShimmerSpark, dustDir * 0.3f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, new Vector2((int)Projectile.position.X, (int)Projectile.position.Y));
            for (int num640 = 0; num640 < 10; num640++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PinkCrystalShard, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (JumpsLeft > 0 && oldVelocity.Y > float.Epsilon && Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity = oldVelocity;
                Flap();
            }
            else
            {
                //Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                Projectile.Kill();
            }

            return false;
        }

        public override void AI()
        {
            const int WEIGHTLESS_TIME = 15;
            //const int JUMP_TIME = 30;
            if (Timer > WEIGHTLESS_TIME)
            {
                Projectile.velocity *= 0.975f;
                Projectile.velocity.Y += 0.1875f;
                /*if (JumpTimer >= JUMP_TIME && JumpsLeft > 0)
                {
                    Flap();
                    JumpTimer -= 40;
                }
                JumpTimer++;*/
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;

            Projectile.frameCounter += 10 + (int)Projectile.velocity.Length();

            if (Projectile.frameCounter > 60)
            {
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
                Projectile.frameCounter -= 60;
            }

            Timer++;
        }

        private void Flap()
        {
            Projectile.frame = 1;
            Projectile.frameCounter = 120;
            if (Projectile.velocity.Y > 0)
                Projectile.velocity.Y *= -0.75f;
            else
                Projectile.velocity.Y *= 1.25f;
            Projectile.velocity.Y -= 1f;
            Projectile.velocity.X *= 2.5f;
            if (Projectile.velocity.Length() > 10f)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10;
            }
            for (int i = 0; i < 12; i++)
            {
                Vector2 dustDir = (MathHelper.TwoPi * i / 12f).ToRotationVector2();
                dustDir.X *= 1.5f;
                Dust.NewDustPerfect(Projectile.Center, DustID.ShimmerSpark, dustDir * 0.3f);
            }
            //JumpTimer = 0;
            JumpsLeft--;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int frameHeight = texture.Height / Main.projFrames[Type];
            int spriteSheetOffset = frameHeight * Projectile.frame;
            Vector2 sheetInsertPosition = (Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();

            Main.EntitySpriteDraw(
                texture,
                sheetInsertPosition,
                new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight - 2)),
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                new Vector2(texture.Width / 2f, (frameHeight - 2) / 2f),
                Projectile.scale,
                effects,
                0f
            );
            
            return false;
        }
    }   
}