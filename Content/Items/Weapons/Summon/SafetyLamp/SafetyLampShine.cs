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
	public class SafetyLampShine : ModProjectile
	{
		public int Strength
		{
			get => (int)Projectile.ai[0];
		}
		public int OwnerUUID
		{
			get => (int)Projectile.ai[1];
		}
		public int Range
		{
			get => 128 + 16 * Strength;
		}
		private static readonly int LIFETIME = 20;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = LIFETIME;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		float str = 0;

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.Damage();
			Projectile.damage = 0;
			str = Main.rand.NextFloat() - 0.5f;
		}

		public override void AI()
		{
			Movement();
			Visuals();
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			Player owner = Main.player[Projectile.owner];
			modifiers.HitDirectionOverride = target.Center.X > owner.Center.X ? 1 : -1;
			modifiers.ScalingArmorPenetration += 0.2f * Strength;
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			Player owner = Main.player[Projectile.owner];
			modifiers.HitDirectionOverride = target.Center.X > owner.Center.X ? 1 : -1;
			modifiers.ScalingArmorPenetration += 0.2f * Strength;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if (Projectile.damage > 0)
			{
				hitbox.Inflate(Range, Range);
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			bool lineOfSightMinion = Collision.CanHitLine(Projectile.Center, 0, 0, target.position, target.width, target.height);
			bool inRange = Projectile.Center.Distance(target.Hitbox.ClosestPointInRect(Projectile.Center)) < Range;
			if (!lineOfSightMinion || !inRange)
			{
				return false;
			}
			return null;
		}

		public override bool CanHitPlayer(Player target)
		{
			bool lineOfSightMinion = Collision.CanHitLine(Projectile.Center, 0, 0, target.position, target.width, target.height);
			bool inRange = Projectile.Center.Distance(target.Hitbox.ClosestPointInRect(Projectile.Center)) < Range;
			return lineOfSightMinion && inRange;
		}

		private Vector2 GetIdlePos()
		{
			int i = Projectile.GetByUUID(Projectile.owner, OwnerUUID);
			if (i >= 0)
			{
				return Main.projectile[i].Center;
			}
			return Vector2.Zero;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		private void Movement()
		{
			if (GetIdlePos() != Vector2.Zero)
			{
				Projectile.Center = GetIdlePos();
			}
		}

		private float LifeDelta()
		{
			return 1.0f - Projectile.timeLeft / (float)LIFETIME;
		}

		private void Visuals()
		{
			Projectile.rotation = str * LifeDelta();
			Projectile.scale = (float)Math.Cos(LifeDelta() * Math.PI * 0.5f);
			Projectile.Opacity = 1.0f - LifeDelta();
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * Projectile.Opacity;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}
	}
}