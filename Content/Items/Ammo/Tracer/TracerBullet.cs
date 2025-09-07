using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Content.Buffs.Marked;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Ammo.Tracer
{
	public class TracerBullet : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			AIType = ProjectileID.Bullet;
			Projectile.Opacity = 0;
			Projectile.extraUpdates = 3;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Marked>(), 5 * 60);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<Marked>(), 5 * 60);
		}

		public override void OnKill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}

        public override bool PreAI()
        {
			Dust.NewDustPerfect(
				Projectile.position + Main.rand.NextFloat() * Projectile.velocity,
				DustID.Firework_Blue,
				Projectile.velocity * 0.1f,
				Projectile.alpha,
				Color.White,
				0.75f
			).noGravity = true;
            return true;
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Type].Value;
			float theta = Projectile.rotation - (float)Math.PI * 0.5f;
			Vector2 drawOrigin = new Vector2(19, 1);
			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
			{
				float d = 1.0f - i / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
				Main.EntitySpriteDraw(
					texture: tex,
					position: Projectile.oldPos[i] - Main.screenPosition,
					sourceRectangle: null,
					color: Projectile.GetAlpha(lightColor) * d,
					rotation: theta,
					origin: drawOrigin,
					scale: 1,
					effects: SpriteEffects.None,
					worthless: 0
				);
			}
			Main.EntitySpriteDraw(
				texture: tex,
				position: Projectile.position - Main.screenPosition,
				sourceRectangle: null,
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