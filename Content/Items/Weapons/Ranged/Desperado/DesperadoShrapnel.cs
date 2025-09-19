using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Desperado
{
	public class DesperadoShrapnel : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 12;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.DamageType = DamageClass.Ranged;

			Projectile.aiStyle = ProjAIStyleID.Arrow;
			AIType = ProjectileID.Bullet;
			Projectile.Opacity = 0;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Dust.NewDustPerfect(
				Projectile.Center,
				DustID.GoldFlame,
				Projectile.velocity * 0.1f
			);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.FinalDamage /= 2;
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			modifiers.FinalDamage /= 2;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

			return true;
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Type].Value;
			float theta = Projectile.rotation - (float)Math.PI * 0.5f;
			Vector2 drawOrigin = new Vector2(3, 1);
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