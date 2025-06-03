using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Projectiles
{
	public class HurriRain : ModProjectile
	{
		const int FADE_TIME = 10;
		ref float timer => ref Projectile.ai[0];
		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 60;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			FadeOut();
			if (timer > 25)
				Physics();
			timer++;
		}

		private void FadeOut()
		{
			if (Projectile.timeLeft <= FADE_TIME)
			{
				Projectile.Opacity = Projectile.timeLeft / (float)FADE_TIME;
			}
		}

		private void Physics()
		{
			Projectile.velocity *= 0.995f;
			Projectile.velocity.Y += 0.3f;
		}


		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 4;
			height = 4;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Vector2 dir = oldVelocity.SafeNormalize(Vector2.UnitY);
			for (int i = 0; i < 3; i++)
			{
				float theta = (-dir).ToRotation() + (i - 1) * MathHelper.PiOver4;
				Dust.NewDustPerfect(Projectile.Center + dir * 4, DustID.Rain, theta.ToRotationVector2(), Scale: 0.8f);
			}
			return true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.penetrate > 1)
				Projectile.damage /= 2;
			else if (Projectile.timeLeft > FADE_TIME)
				Projectile.timeLeft = FADE_TIME;
		}

        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D tex = TextureAssets.Projectile[Type].Value;
			float theta = Projectile.rotation;
			Rectangle sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
			Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
			Vector2 offset = new Vector2(-23, 0).RotatedBy(theta);
			Main.EntitySpriteDraw(
				texture: tex,
				position: Projectile.position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + drawOrigin + offset,
				sourceRectangle: sourceRect,
				color: Projectile.GetAlpha(lightColor),
				rotation: theta,
				origin: drawOrigin,
				scale: 1,
				effects: SpriteEffects.None,
				worthless: 0
			);
            return false;
        }
    }
}