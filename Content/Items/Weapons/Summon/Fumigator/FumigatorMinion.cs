using System;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Summon.Fumigator
{
	public class FumigatorMinion : ModProjectile
	{
        public ref float Timer => ref Projectile.ai[0];
		public int Ticker
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public override void SetStaticDefaults()
		{
			// Sets the amount of frames this minion has on its spritesheet
			//Main.projFrames[Projectile.type] = 1;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.tileCollide = false; // Makes the minion go through tiles freely

			// These below are needed for a minion weapon
			Projectile.friendly = false; // Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.minion = true; // Declares this as a minion (has many effects)
			Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
			Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.netImportant = true;
		}

		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return false;
		}
		
		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Timer += 1 / 60f;
			Ticker += 1;

			if (!CheckActive(owner))
			{
				return;
			}

			SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
			Movement(owner, foundTarget, distanceFromTarget, targetCenter);
			Visuals();
			if (Main.LocalPlayer == owner)
			{
				if (Ticker % 10 == 0)
				{
					Projectile.NewProjectileDirect(
							owner.GetSource_FromThis(),
							Projectile.Center + Main.rand.NextFloatDirection().ToRotationVector2() * Main.rand.NextFloat() * 48f,
							Vector2.Zero,
							ModContent.ProjectileType<FumigatorCloud>(),
							Projectile.damage / 3,
							0,
							owner.whoAmI
						);
				}
				if (Ticker % 45 == 0 && foundTarget)
				{
					Projectile.NewProjectileDirect(
							owner.GetSource_FromThis(),
							Projectile.Center + Main.rand.NextFloatDirection().ToRotationVector2() * Main.rand.NextFloat() * 48f,
							Vector2.Zero,
							ModContent.ProjectileType<FumigatorSpore>(),
							Projectile.damage,
							Projectile.knockBack,
							owner.whoAmI
						);
				}
			}
		}

		// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<FumigatorBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<FumigatorBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}

		private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
		{
			// Starting search distance
			distanceFromTarget = 700f;
			targetCenter = Projectile.position;
			foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[owner.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, owner.Center);

				// Reasonable distance away so it doesn't target across multiple screens
				if (between <= 700f)
				{
					distanceFromTarget = Vector2.Distance(npc.Center, Projectile.Center);
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}

			if (!foundTarget)
			{
				float temp = distanceFromTarget;
				// This code is required either way, used for finding a target
				foreach (var npc in Main.ActiveNPCs)
				{
					if (npc.CanBeChasedBy())
					{
						float between = Vector2.Distance(npc.Center, owner.Center);
						float dist = npc.Center.Distance(Projectile.Center);
						bool inRange = between < temp;
						bool closest = between < distanceFromTarget;
						bool lineOfSightPlayer = Collision.CanHitLine(owner.Center + new Vector2(-4, -4), 8, 8, npc.position, npc.width, npc.height);
						bool lineOfSightMinion = Collision.CanHitLine(Projectile.Center + new Vector2(-4, -4), 8, 8, npc.position, npc.width, npc.height);
						bool closeThroughWalls = between < 16 * 5;

						bool distTest = inRange;
						if (Projectile.minionPos % 4 == 1)
						{
							distTest = inRange;
						}
						if (Projectile.minionPos % 4 == 2)
						{
							distTest = closest;
						}
						if (Projectile.minionPos % 4 == 3)
						{
							distTest = inRange || closest;
						}
						if (Projectile.minionPos % 4 == 0)
						{
							distTest = inRange && closest;
						}
						if (distTest && (lineOfSightPlayer || lineOfSightMinion || closeThroughWalls))
						{
							temp = between;
							targetCenter = npc.Center;
							foundTarget = true;
							distanceFromTarget = dist;
						}
					}
				}
			}
		}

		private Vector2 GetIdlePos()
		{
			Vector2 idlePosition = Main.player[Projectile.owner].Center;
			idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)
			idlePosition.Y += Main.player[Projectile.owner].gfxOffY - (Projectile.minionPos % 2 == 0 ? (float)Math.Sqrt(2) * 0.5f * 32 : 0f) - 2f * (float)Math.Sin(Timer);
			idlePosition.X += -Main.player[Projectile.owner].direction * 32 * (Projectile.minionPos - 1);

			return idlePosition;
		}

		private void Movement(Player owner, bool foundTarget, float distanceFromTarget, Vector2 targetCenter)
		{
			if (foundTarget)
			{
				if (distanceFromTarget > 16 * 4f)
				{
					Vector2 direction = Projectile.Center.DirectionTo(targetCenter);
					Projectile.velocity *= 0.95f;
					Projectile.velocity += direction * 0.25f;
				}
				if (Projectile.velocity.Length() < 2f)
				{
					Projectile.velocity = 2f * Projectile.velocity.SafeNormalize(Vector2.UnitY);
				}
			}
			else
			{
				Projectile.velocity *= 0.97f;
				Projectile.velocity += (GetIdlePos() - Projectile.Center) * 0.03f;
				float d = Math.Clamp(Projectile.Center.Distance(GetIdlePos()) / 200f - 1f, 0f, 1f);
				float speed = 8f * d + 4f + (1.0f - d);
				if (Projectile.velocity.Length() > speed)
				{
					Projectile.velocity = speed * Projectile.velocity.SafeNormalize(Vector2.UnitY);
				}

				if (Projectile.Center.Distance(owner.Center) > 800)
				{
					Projectile.Center = GetIdlePos();
				}
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = foundTarget ? 0.25f : 0.1f;

			float overlapDist = foundTarget ? 128f : Projectile.width * 0.8f;

			// Fix overlap with other minions
			foreach (var other in Main.ActiveProjectiles)
			{
				if (other.type != Projectile.type)
				{
					continue;
				}
				if (other.whoAmI != Projectile.whoAmI && other.owner == Projectile.owner && Projectile.Center.Distance(other.Center) < overlapDist)
				{
					Vector2 dir = other.Center.DirectionTo(Projectile.Center);
					Projectile.velocity += dir * overlapVelocity;
					Projectile.Center += dir;
				}
			}
		}

		private void Visuals()
		{
			Projectile.rotation *= 0.99f;
			Projectile.rotation += Projectile.velocity.X * 0.01f;
			Projectile.rotation += 0.005f * (float)Math.Sin(Timer);
		}
	}
}