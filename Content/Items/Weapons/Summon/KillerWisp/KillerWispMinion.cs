using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Summon.KillerWisp
{
	public class KillerWispMinion : ModProjectile
	{
        public ref float Timer => ref Projectile.ai[0];
        public ref float Anger => ref Projectile.ai[1];
		static Asset<Texture2D> shineTex;
        public override void Load()
		{
			shineTex = ModContent.Request<Texture2D>("SuperfluityTwo/Content/Items/Weapons/Summon/KillerWisp/KillerWispShine", AssetRequestMode.AsyncLoad);
		}
        public int Target
		{
			get => (int)Projectile.ai[2];
			set => Projectile.ai[2] = value;
		}
		public int damageCooldown = 0;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.tileCollide = false; // Makes the minion go through tiles freely
			Projectile.ignoreWater = true;

			// These below are needed for a minion weapon
			Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.minion = true; // Declares this as a minion (has many effects)
			Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
			Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 48;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}
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
			damageCooldown--;
		}

		// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<KillerWispBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<KillerWispBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}

        public override bool? CanHitNPC(NPC target)
        {
            return target.Center.Distance(Projectile.Center) < 128 && target.whoAmI == Target - 1 && damageCooldown <= 0;
        }

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.Inflate(128, 128);
        }

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			target.AddBuff(BuffID.Bleeding, 60 * 5);
			if (target.life <= 200)
			{
				modifiers.SetInstantKill();
			}
			float str = Main.rand.NextFloat() * 2.0f - 1.0f;
			for (int i = 0; i < 30; i++)
			{
				float delta = i / 29f;
				Vector2 dir = Projectile.Center.DirectionTo(target.Center);
				Dust.NewDustPerfect(
					target.Center * delta + Projectile.Center * (1.0f - delta)
						+ str * 32f * dir.RotatedBy(Math.PI * 0.5f) * (float)Math.Cos((delta - 0.5f) * Math.PI),
					DustID.GoldFlame,
					Vector2.Zero
				).noGravity = true;
			}
			damageCooldown = 48;
        }

		private void SearchForTargets(Player owner, out bool foundTarget, out Vector2 targetCenter)
		{
			// Starting search distance
			float distanceFromTarget = 700f * 5;
			targetCenter = Projectile.position;
			foundTarget = false;
			bool retarget = false;
			bool targettingCursor = false;

			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[owner.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);

				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f && owner.Center.Distance(npc.Center) < 900f)
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
					Target = owner.MinionAttackTargetNPC + 1;
					targettingCursor = true;
				}
			}

			if (Target > 0)
			{
				NPC npc = Main.npc[Target - 1];
				if (npc.active && npc.CanBeChasedBy(Projectile) && npc.Center.Distance(owner.Center) < 900f)
				{
					foundTarget = true;
					//distanceFromTarget = npc.Center.Distance(Projectile.Center) * 5;
					targetCenter = npc.Center;
				}
				else
				{
					Target = 0;
					foundTarget = false;
					targettingCursor = false;
				}
				if (!targettingCursor && foundTarget)
				{
					retarget = Anger > 2.4f;
				}
			}

			if (!foundTarget)
			{
				Anger = 0;
			}

			int old = Target;

			if (retarget || !foundTarget)
			{
				List<int> blacklist = [];
				foreach (var proj in Main.ActiveProjectiles)
				{
					if (proj.type == Projectile.type && proj.owner == Projectile.owner && proj.whoAmI != Projectile.whoAmI)
					{
						blacklist.Add((int)proj.ai[2] - 1);
					}
				}
				foreach (var npc in Main.ActiveNPCs)
				{
					bool lineOfSightPlayer = Collision.CanHitLine(owner.Center + new Vector2(-4, -4), 8, 8, npc.position, npc.width, npc.height);
					bool lineOfSightMinion = Collision.CanHitLine(Projectile.Center + new Vector2(-4, -4), 8, 8, npc.position, npc.width, npc.height);
					if (npc.CanBeChasedBy() && (lineOfSightPlayer || lineOfSightMinion) && npc.Center.Distance(owner.Center) < 700f)
					{
						float distPlayer = Vector2.Distance(npc.Center, owner.Center);
						float mult = 1.0f;
						foreach (int i in blacklist)
						{
							if (npc.whoAmI == i)
							{
								mult++;
							}
						}
						distPlayer *= mult;

						bool closePlayer = distPlayer < distanceFromTarget;

						if (closePlayer)
						{
							targetCenter = npc.Center;
							distanceFromTarget = distPlayer;
							foundTarget = true;
							Target = npc.whoAmI + 1;
						}
					}
				}
			}

			if (!foundTarget)
			{
				Target = 0;
				Anger = 0;
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
			int alternator = Projectile.minionPos % 2;
			idlePosition.Y += Main.player[Projectile.owner].gfxOffY + 48 * alternator;
			idlePosition.X += -Main.player[Projectile.owner].direction * 48 * Projectile.minionPos;

			return idlePosition;
		}

		private void Movement(Player owner, bool foundTarget, Vector2 targetCenter)
		{
			if (foundTarget)
			{
				Vector2 dir = (targetCenter - Projectile.Center).SafeNormalize(Vector2.Zero); ;
				Projectile.velocity *= 0.99f;
				Projectile.velocity += 0.3f * (1 + 0.1f * Main.npc[Target - 1].velocity.Length() + 0.5f * Anger) * dir * Math.Clamp((Projectile.Center.Distance(targetCenter) - 56f) / 100f, -8f, 8f);
				if (Projectile.Center.Distance(targetCenter) > 128f)
				{
					if (Projectile.velocity.Length() > 8 + Anger)
					{
						Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.One) * (8 + Anger);
					}
				}
				Anger += 1 / 60f;
				if (Anger > 2.5f)
				{
					Anger = 2.5f;
				}
			}
			else
			{
				Vector2 dir = (GetIdlePos() - Projectile.Center).SafeNormalize(Vector2.Zero);
				Projectile.velocity *= 0.9f;
				Projectile.velocity += dir * Math.Clamp(0.003f * Projectile.Center.Distance(GetIdlePos()), 0f, 8f);
				if (Projectile.Center.Distance(owner.Center) > 1000f)
				{
					Projectile.Center = GetIdlePos();
				}
				Anger = 0;
			}
			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.1f;

			float overlapDist = Projectile.width;

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
			Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.78f);
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D shine = shineTex.Value;
			
			Vector2 drawOrigin = new Vector2(Projectile.width / 2, Projectile.height / 2);
			Vector2 drawOffset = new Vector2(0, 2f * (float)Math.Sin(Timer * 1.7f));
			Color color = GetAlpha(lightColor).GetValueOrDefault(Color.White);
			Main.EntitySpriteDraw(
				texture,
				Projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY) + drawOffset,
				null,
				color,
				Projectile.rotation + 0.1f * (float)Math.Sin(Timer * 2f),
				new Vector2(texture.Width / 2, texture.Height / 2),
				Projectile.scale,
				SpriteEffects.None,
				0
			);
			for (int k = 0; k < 6; k++)
			{
				Vector2 drawPos = Projectile.position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
				int alternator = k % 2 * 2 - 1;
				Main.EntitySpriteDraw(
					shine,
					drawPos - drawOrigin + new Vector2(shine.Width / 2, shine.Height / 2) + drawOffset,
					null,
					color,
					Projectile.rotation + 0.5f * Timer * alternator + k * (float)Math.PI / 3f,
					new Vector2(shine.Width / 2, shine.Height / 2),
					Projectile.scale + 0.1f * alternator + 0.1f * (float)Math.Sin(0.8f * Timer + k),
					SpriteEffects.None,
					0
				);
			}
			
			return false;
		}
	}
}