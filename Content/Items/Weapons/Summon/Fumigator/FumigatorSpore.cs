using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Summon.Fumigator
{
	public class FumigatorSpore : ModProjectile
	{
		ref float timer => ref Projectile.ai[0];
		ref float angularVel => ref Projectile.localAI[0];
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 120;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.hide = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.rotation += Projectile.velocity.X * 0.001f;
			return false;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.Opacity = 0.0f;
			Projectile.rotation = Main.rand.NextFloatDirection();
			angularVel = (Main.rand.NextFloat() - 0.5f) * 0.1f;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindProjectiles.Add(index);
		}

		public override bool PreAI()
		{
			FadeInAndOut();
			timer++;
			Projectile.velocity *= 0.99f;
			Projectile.rotation += angularVel;

			int n = Projectile.FindTargetWithLineOfSight();
			if (n >= 0)
			{
				Projectile.velocity += Projectile.Center.DirectionTo(Main.npc[n].Center) * 0.05f;
			}

			Projectile.velocity.Y += 0.01f;
			Projectile.friendly = Projectile.timeLeft > 20 && Projectile.friendly;
			
			Lighting.AddLight(Projectile.Center, Color.DarkCyan.ToVector3() * 0.3f);
			return false;
		}

		private void FadeInAndOut()
		{
			if (timer < 20)
			{
				Projectile.Opacity = timer / 20f;
			}
			else if (Projectile.timeLeft < 40)
			{
				Projectile.Opacity = Projectile.timeLeft / 40f;
			}
			else
			{
				Projectile.Opacity = 1;
			}
		}
    }
}