using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Standoff
{
	public class StandoffExplosion : ModProjectile
	{
		private readonly static int LIFETIME = 9;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = LIFETIME;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.ownerHitCheck = true;
			Projectile.Opacity = 0;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.FinalDamage /= 3;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
			modifiers.FinalDamage /= 3;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox.Inflate(16 * 6, 16 * 6);
        }

        public override bool? CanHitNPC(NPC target)
		{
			return target.Hitbox.IntersectsConeSlowMoreAccurate(Main.player[Projectile.owner].Center, 16 * 6, Projectile.velocity.ToRotation(), (float)Math.PI * 0.06125f);
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.position += (Projectile.width / 2 + 4) * Projectile.velocity.SafeNormalize(Vector2.Zero);
			Projectile.Opacity = 1;
			Projectile.Damage();
			SoundEngine.PlaySound(
				SoundID.Item38,
				Projectile.Center
			);
			Lighting.AddLight(Projectile.Center, new Vector3(0.933f, 0.345f, 0.400f) * 1.5f);

			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 position = Main.player[Projectile.owner].Center;
				for (int i = 0; i < 6; i++)
				{
					float d = i / 5f;
					Vector2 newVel = Projectile.velocity.RotatedBy((d - 0.5f) * Math.PI * 0.25f).RotatedByRandom(Math.PI * 0.06125f) * (0.75f + 0.5f * Main.rand.NextFloat());

					Projectile.NewProjectile(
						source,
						position,
						newVel,
						ModContent.ProjectileType<Desperado.DesperadoShrapnel>(),
						Projectile.damage,
						Projectile.knockBack * 1.5f,
						Projectile.owner
					);
				}
			}

			Projectile.damage = 0;
		}

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			float d = (float)(LIFETIME - Projectile.timeLeft) / (LIFETIME + 1);
			Projectile.frame = (int)(d * (Main.projFrames[Type] + 1));
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCs.Add(index);
        }
    }
}