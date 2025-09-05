using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Desperado
{
	public class DesperadoExplosion : ModProjectile
	{
		private readonly static int LIFETIME = 9;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 42;
			Projectile.height = 42;
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

		Vector2 offset = Vector2.Zero;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.FinalDamage /= 3;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
			modifiers.FinalDamage /= 3;
        }

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.position += (Projectile.width / 2 + 4) * Projectile.velocity.SafeNormalize(Vector2.Zero);
			Projectile.Opacity = 1;
			offset = Projectile.Center - Main.player[Projectile.owner].Center;
			Projectile.Damage();
			SoundEngine.PlaySound(
				SoundID.Item38,
				Projectile.Center
			);

			if (Projectile.owner == Main.myPlayer)
			{
				int type = ModContent.ProjectileType<DesperadoShrapnel>();
				Vector2 pos = Projectile.Center - 24 * Projectile.velocity.SafeNormalize(Vector2.Zero);
				Vector2 position;
				if (Collision.CanHit(Main.player[Projectile.owner].Center, 0, 0, pos, 0, 0))
				{
					position = pos;
				}
				else
				{
					position = Main.player[Projectile.owner].Center;
				}
				for (int i = 0; i < 6; i++)
				{
					float d = i / 5f;
					Vector2 newVel = Projectile.velocity.RotatedBy((d - 0.5f) * Math.PI * 0.25f).RotatedByRandom(Math.PI * 0.06125f) * (0.75f + 0.5f * Main.rand.NextFloat());

					Projectile.NewProjectile(
						source,
						position,
						newVel,
						type,
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
			Projectile.frame = (int)((1.0f - (float)Projectile.timeLeft / LIFETIME) * Main.projFrames[Type]);
			if (offset != Vector2.Zero)
			{
				Projectile.Center = Main.player[Projectile.owner].Center + new Vector2(0, Main.player[Projectile.owner].gfxOffY) + offset;
			}
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