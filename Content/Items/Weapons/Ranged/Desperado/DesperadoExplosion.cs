using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
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
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		Vector2 offset = Vector2.Zero;

		public override void OnSpawn(IEntitySource source)
		{
			offset = Projectile.Center - Main.player[Projectile.owner].Center;
			Projectile.Damage();
			Projectile.damage = 0;
        }

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.frame = (int)((1.0f - (float)Projectile.timeLeft / LIFETIME) * Main.projFrames[Type]);
			if (offset != Vector2.Zero)
			{
				Projectile.Center = Main.player[Projectile.owner].Center + offset;
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