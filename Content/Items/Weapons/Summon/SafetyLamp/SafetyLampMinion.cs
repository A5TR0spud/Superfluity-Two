using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SuperfluityTwo.Content.Items.Weapons.Summon.SafetyLamp
{
	public class SafetyLampMinion : ModProjectile
	{
		public ref float Timer => ref Projectile.ai[0];
		public int Strength
		{
			get => Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<SafetyLampCounter>()];
		}
		public int Range
		{
			get => 128 + 16 * Strength;
		}

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = false;

			Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

			ProjectileID.Sets.MinionSacrificable[Projectile.type] = false; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
			ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 34;
			Projectile.tileCollide = false; // Makes the minion go through tiles freely
			Projectile.ignoreWater = true;

			// These below are needed for a minion weapon
			Projectile.friendly = false; // Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.minion = true; // Declares this as a minion (has many effects)
			Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
			Projectile.minionSlots = 0f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.netImportant = true;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];

			if (!CheckActive(owner))
			{
				return;
			}

			UpdateDamage(owner);
			Movement(owner);
			Visuals(owner);
			if (Timer > 1f / Strength && Main.myPlayer == Projectile.owner)
			{
				Timer = Math.Min(Timer, 1f / Strength);
				if (FindTarget())
				{
					Timer -= 1f / Strength;
					Projectile.NewProjectile(
						Projectile.GetSource_FromThis(),
						Projectile.Center,
						Vector2.Zero,
						ModContent.ProjectileType<SafetyLampShine>(),
						Projectile.damage,
						Projectile.knockBack,
						Projectile.owner,
						Strength,
						Projectile.projUUID
					);
				}
				Projectile.netUpdate = true;
			}
			Timer += 1 / 60f;
		}

		private bool FindTarget()
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.active &&
					!npc.CountsAsACritter &&
					!npc.dontTakeDamage &&
					!npc.friendly &&
					!npc.immortal &&
					Main.player[Projectile.owner].CanNPCBeHitByPlayerOrPlayerProjectile(npc) &&
					Collision.CanHit(Projectile.Center, 0, 0, npc.position, npc.width, npc.height) &&
					npc.Hitbox.ClosestPointInRect(Projectile.Center).Distance(Projectile.Center) < Range)
				{
					return true;
				}
			}
			return false;
		}

		private void UpdateDamage(Player owner)
		{
			Projectile.originalDamage = owner.GetModPlayer<SafetyLampPlayer>().highestOriginalDamage;
			Projectile.knockBack = owner.GetModPlayer<SafetyLampPlayer>().highestKnockback;
		}

		// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<SafetyLampBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<SafetyLampBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}

		private Vector2 GetIdlePos()
		{
			Vector2 idlePosition = Main.player[Projectile.owner].Center;
			idlePosition.Y += -48f + Main.player[Projectile.owner].gfxOffY;
			Vector2 off = Projectile.velocity;
			if (off.Length() > 48)
			{
				off = off.SafeNormalize(Vector2.One) * 48;
			}

			return idlePosition + off;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		private void Movement(Player owner)
		{
			Projectile.velocity = owner.velocity;
			float dist = Projectile.Center.Distance(GetIdlePos());
			Projectile.Center = Projectile.Center.MoveTowards(GetIdlePos(), dist > 32 ? 5 + (dist - 32) * 0.5f : 5);
			if (dist > 50)
			{
				Projectile.Center = GetIdlePos();
			}
		}

		private void Visuals(Player owner)
		{
			Projectile.rotation = Math.Clamp(Projectile.velocity.X * 0.05f, -0.5f, 0.5f);
			Lighting.AddLight(Projectile.Center, Color.LightYellow.ToVector3() * (0.45f + 0.08f * Strength));
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindProjectiles.Add(index);
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}