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

namespace SuperfluityTwo.Content.Items.Weapons.Summon.SafetyLamp
{
	public class SafetyLampCounter : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = false;

			Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.tileCollide = false; // Makes the minion go through tiles freely
			Projectile.ignoreWater = true;

			// These below are needed for a minion weapon
			Projectile.friendly = false; // Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.minion = true; // Declares this as a minion (has many effects)
			Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
			Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.hide = true;
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

			Movement(owner);
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
			idlePosition.Y -= 48f;
			idlePosition.X += Projectile.minionPos;

			return idlePosition;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		private void Movement(Player owner)
		{
			Projectile.Center = GetIdlePos();
			Projectile.velocity = owner.velocity;
		}

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
	}
}