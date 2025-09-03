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

namespace SuperfluityTwo.Content.Items.Weapons.Summon.Asterism
{
	public class AsterismMinion : ModProjectile
	{
        public ref float Timer => ref Projectile.ai[0];
        public ref float Delta => ref Projectile.ai[1];
        public int Target {
			get => (int)Projectile.ai[2];
			set => Projectile.ai[2] = value;
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


			ProjectileID.Sets.TrailCacheLength[Type] = 24; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Type] = 0; // The recording mode
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.tileCollide = false; // Makes the minion go through tiles freely

			// These below are needed for a minion weapon
			Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.minion = true; // Declares this as a minion (has many effects)
			Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
			Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 59;
			Projectile.netImportant = true;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Timer = 0;
			Delta = 0;
        }

		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return true;
		}

		// The AI of this minion is split into multiple methods to avoid bloat. This method just passes values between calls actual parts of the AI.
		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Timer += 1 / 60f;

			if (!CheckActive(owner))
			{
				return;
			}

			SearchForTargets(owner, out bool foundTarget, out Vector2 targetCenter);
			Movement(owner, foundTarget, targetCenter);
			Visuals();
		}

		// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<AsterismBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<AsterismBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}

		private void SearchForTargets(Player owner, out bool foundTarget, out Vector2 targetCenter)
		{
			// Starting search distance
			float distanceFromTarget = 700f;
			targetCenter = Projectile.position;
			foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[owner.MinionAttackTargetNPC];
				Target = owner.MinionAttackTargetNPC+ 1;
				float between = Vector2.Distance(npc.Center, Projectile.Center);

				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f)
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
					lurchPos = Projectile.Center;
				}
			}

			if (Target > 0)
			{
				NPC npc = Main.npc[Target - 1];
				if (npc.active && npc.CanBeChasedBy(Projectile) && (Projectile.localNPCImmunity[npc.whoAmI] == 0 || Delta <= 1))
				{
					foundTarget = true;
					distanceFromTarget = npc.Center.Distance(Projectile.Center);
					targetCenter = npc.Center;
				}
				else
				{
					Target = 0;
					Delta = 0;
				}
			}

			if (!foundTarget)
			{
				float maxHP = 0f;
				float hp = 0f;
				bool first = true;

				foreach (var npc in Main.ActiveNPCs)
				{
					bool lineOfSightPlayer = Collision.CanHitLine(owner.Center + new Vector2(-4, -4), 8, 8, npc.position, npc.width, npc.height);
					bool lineOfSightMinion = Collision.CanHitLine(Projectile.Center + new Vector2(-4, -4), 8, 8, npc.position, npc.width, npc.height);
					if (npc.CanBeChasedBy() && (lineOfSightPlayer || lineOfSightMinion) && npc.Center.Distance(Projectile.Center) < 700f && npc.Center.Distance(owner.Center) < 700f)
					{
						float distPlayer = Vector2.Distance(npc.Center, owner.Center);
						float distMinion = Vector2.Distance(npc.Center, Projectile.Center);

						bool closePlayer = distPlayer < distanceFromTarget;
						bool closeMinion = distMinion < distanceFromTarget;
						bool highMaxHP = npc.lifeMax >= maxHP || first;
						bool highCurrentHP = npc.life >= hp || first;
						bool lowCurrentHP = npc.life <= hp || first;
						bool targetPriorityClosePlayer = Projectile.minionPos % 5 == 0;
						bool targetPriorityCloseMinion = Projectile.minionPos % 5 == 1;
						bool targetPriorityHighestMaxHP = Projectile.minionPos % 5 == 2;
						bool targetPriorityHighestCurrentHP = Projectile.minionPos % 5 == 3;
						bool targetPriorityLowestCurrentHP = Projectile.minionPos % 5 == 4;
						if (npc.lifeMax == maxHP && targetPriorityHighestMaxHP)
						{
							targetPriorityClosePlayer = true;
							targetPriorityHighestMaxHP = false;
						}
						if (npc.life == hp && targetPriorityHighestCurrentHP)
						{
							targetPriorityClosePlayer = true;
							targetPriorityHighestCurrentHP = false;
						}
						if (npc.life == hp && targetPriorityLowestCurrentHP)
						{
							targetPriorityClosePlayer = true;
							targetPriorityLowestCurrentHP = false;
						}

						if ((closePlayer && targetPriorityClosePlayer) ||
							(closeMinion && targetPriorityCloseMinion) ||
							(highMaxHP && targetPriorityHighestMaxHP) ||
							(highCurrentHP && targetPriorityHighestCurrentHP) ||
							(lowCurrentHP && targetPriorityLowestCurrentHP)
							)
						{
							targetCenter = npc.Center;
							distanceFromTarget = targetPriorityClosePlayer ? distPlayer : distMinion;
							foundTarget = true;
							Target = npc.whoAmI + 1;
							Delta = 0;
							maxHP = npc.lifeMax;
							hp = npc.life;
							lurchPos = Projectile.Center;
						}
					}
				}
				
			}

			if (!foundTarget)
			{
				Target = 0;
			}

			// friendly needs to be set to true so the minion can deal contact damage
			// friendly needs to be set to false so it doesn't damage things like target dummies while idling
			// Both things depend on if it has a target or not, so it's just one assignment here
			// You don't need this assignment if your minion is shooting things instead of dealing contact damage
			//Projectile.friendly = foundTarget;
		}

		private Vector2 GetIdlePos()
		{
			Vector2 idlePosition = Main.player[Projectile.owner].Center;
			idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)
			idlePosition.Y += Main.player[Projectile.owner].gfxOffY;

			Vector2 idleOffset = 32f * (float)Math.Sin(14.9f * Projectile.minionPos + Timer * 0.31726f) * new Vector2((float)Math.Cos(Projectile.minionPos + Timer), 0.7562f * (float)Math.Sin(Projectile.minionPos + Timer * 0.7562f));
			return idlePosition + idleOffset;
		}

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

		Vector2 lurchPos = Vector2.Zero;

		private void Movement(Player owner, bool foundTarget, Vector2 targetCenter)
		{
			if (foundTarget)
			{
				Vector2 direction = targetCenter - Projectile.Center;
				direction.Normalize();

				Projectile.Opacity = 1.0f;

				int alternator = Projectile.minionPos % 2 * 2 - 1;
				Vector2 dir = GetIdlePos().DirectionTo(targetCenter);
				Projectile.velocity = dir;

				float d = Delta * Delta;
				if (lurchPos == Vector2.Zero)
				{
					lurchPos = GetIdlePos();
				}
				Projectile.Center = lurchPos * (1.0f - d) + targetCenter * d
					+ dir.RotatedBy(Math.PI * alternator * 0.5f) * 16f * (Projectile.minionPos / 2) * (float)Math.Cos((d - 0.5f) * Math.PI);
				Delta += 1f / (59 + Projectile.minionPos);
				if (Delta > 1)
				{
					Delta = 0;
					Target = 0;
					lurchPos = Projectile.Center;
					Projectile.friendly = true;
				}
				else
				{
					Projectile.friendly = false;
				}
			}
			else
			{
				if (Projectile.Center.Distance(owner.Center) > 800f)
				{
					Projectile.position = GetIdlePos();
				}
				Projectile.position = Projectile.position.MoveTowards(GetIdlePos(), 6 + owner.velocity.Length());
				lurchPos = Projectile.Center;
			}
		}

		private void Visuals()
		{
			// So it will lean slightly towards the direction it's moving
			int alternator = Projectile.minionPos % 2 * 2 - 1;
			Projectile.rotation += (float)Math.Sin(Projectile.minionPos) * 0.01f * alternator;
			Projectile.scale = 0.8f + 0.2f * (float)Math.Sin(Timer * (1 + 0.1f * Projectile.minionPos));

			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.78f);
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

		public override bool PreDrawExtras()
		{
			// Draws an afterimage trail. See https://github.com/tModLoader/tModLoader/wiki/Basic-Projectile#afterimage-trail for more information.

			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
			{
				float delta = (float)k / Projectile.oldPos.Length;
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				color = color.MultiplyRGBA(
					Color.Lerp(new Color(0.643f, 1, 0.62f),
						Color.Lerp(new Color(0.686f, 0.643f, 0.898f),
							new Color(1, 0.027f, 0.592f),
						delta * 3 - 1),
					delta * 3)
				);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
        }
		/*public override bool PreDraw(ref Color lightColor)
		{
			
			return true;
		}*/
	}
}