using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Content.Items.Accessories.Ranger;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger.Pal
{
	public class PalProj : ModProjectile
	{
		ref float Vanity => ref Projectile.ai[0];
		bool IsVanity
		{
			get
			{
				return (int)Vanity == 1;
			}
			set
			{
				Vanity = value ? 1 : 0;
			}
		}
		ref float ShootTimer => ref Projectile.ai[1];
		public Vector2 overlappingVelocity;
		public Vector2 playerProvidedVelocity;
		public Vector2 elbowPoint;
		float elbowRot;
		float armRot;
		ref float oldgfxOffY => ref Projectile.localAI[0];

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 2;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.ContinuouslyUpdateDamageStats = true;
		}

		public override bool? CanDamage()
		{
			return false;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

		public override void AI()
		{
			int type = ModContent.ProjectileType<PalProj>();
			if (Projectile.TryGetOwner(out Player player) && Projectile.friendly && !Projectile.hostile && (player.GetModPlayer<PalPlayer>().hasPal || player.GetModPlayer<PalPlayer>().hasPalVanity) && Projectile.type == type && player.ownedProjectileCounts[type] <= 1)
			{
				Projectile.timeLeft = 2;
			}
			else
			{
				return;
			}
			Projectile.position.Y += 0.25f * (player.gfxOffY - oldgfxOffY);

			bool yesTarget = UpdateTargetting(out int facing);
			if (facing == 0)
			{
				facing = player.direction;
			}
			if (!yesTarget)
			{
				Projectile.rotation = facing == -1 ? MathHelper.Pi : 0;
			}
			Vector2 targetPosAbsolute = player.Top + new Vector2(facing * -8, -18 + player.gfxOffY);
			Vector2 targetPosRelative = targetPosAbsolute - Projectile.Center;

			overlappingVelocity = overlappingVelocity.MoveTowards(targetPosRelative, 0.8f);
			if (overlappingVelocity.Length() > 8) overlappingVelocity = 8 * overlappingVelocity.SafeNormalize(Vector2.Zero);

			playerProvidedVelocity = player.velocity * 0.9f;

			if (Projectile.Center.Distance(targetPosAbsolute) > 48)
			{
				Projectile.Center = targetPosAbsolute;
				overlappingVelocity = Vector2.Zero;
				playerProvidedVelocity = player.velocity;
				playerProvidedVelocity.Y += player.gfxOffY;
			}

			float drag = 0.9f;
			Projectile.position += overlappingVelocity + playerProvidedVelocity;
			overlappingVelocity *= drag;
			playerProvidedVelocity *= drag;

			elbowPoint = Projectile.Center;
			elbowPoint.Y = player.Center.Y + player.gfxOffY;
			elbowPoint.X += -2 * facing;
			float dist = 30;
			float threshold = 0.1f;
			Vector2 offsetCenter = player.Center;
			offsetCenter.Y += player.gfxOffY;
			for (int i = 0; i < 2; i++)
			{
				if (elbowPoint.Distance(Projectile.Center) < dist - threshold)
				{
					elbowPoint += -elbowPoint.DirectionTo(Projectile.Center) * (dist - elbowPoint.Distance(Projectile.Center));
				}
				if (elbowPoint.Distance(offsetCenter) < dist - threshold)
				{
					elbowPoint += -elbowPoint.DirectionTo(offsetCenter) * (dist - elbowPoint.Distance(offsetCenter));
				}
				if (elbowPoint.Distance(Projectile.Center) > dist + threshold)
				{
					elbowPoint = elbowPoint.MoveTowards(Projectile.Center, elbowPoint.Distance(Projectile.Center) - dist);
				}
				if (elbowPoint.Distance(offsetCenter) > dist + threshold)
				{
					elbowPoint = elbowPoint.MoveTowards(offsetCenter, elbowPoint.Distance(offsetCenter) - dist);
				}
			}
			elbowRot = elbowPoint.AngleTo(Projectile.Center);
			armRot = elbowPoint.AngleFrom(offsetCenter);

			Projectile.spriteDirection = facing;
			oldgfxOffY = player.gfxOffY;
		}

		public bool UpdateTargetting(out int facing)
		{
			Projectile.TryGetOwner(out Player player);
			IsVanity = player.GetModPlayer<PalPlayer>().hasPalVanity && !player.GetModPlayer<PalPlayer>().hasPal;
			if (IsVanity)
			{
				facing = 0;
				return false;
			}

			int targetID = Projectile.FindTargetWithLineOfSight();
			if (targetID < 0)
			{
				facing = 0;
				ShootTimer = 0;
				return false;
			}
			NPC target = Main.npc[targetID];
			facing = Math.Sign(target.Center.X - Projectile.Center.X);
			Vector2 dir = Projectile.Center.DirectionTo(target.Center);
			Projectile.rotation = dir.ToRotation();
			if (ShootTimer > 40)
			{
				if (player.PickAmmo(new Item(ItemID.FlintlockPistol), out int proj, out float speed, out int damage, out float kB, out int ammoID, true))
				{
					if (player.whoAmI == Main.myPlayer)
					{
						Projectile.NewProjectile(
							Projectile.GetSource_FromThis(),
							Projectile.Center,
							speed * dir,
							proj,
							damage,
							kB,
							player.whoAmI,
							proj == ProjectileID.ChlorophyteBullet ? targetID + 1 : 0
						);
					}
					SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);
				}
				ShootTimer = 0;
			}
			ShootTimer++;
			return true;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCs.Add(index);
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Type].Value;

			Vector2 offset = new Vector2(6, 6);
			Main.EntitySpriteDraw(
				tex,
				elbowPoint - Main.screenPosition,
				new Rectangle(0, 28, 30, 12),
				Projectile.GetAlpha(lightColor),
				armRot + MathHelper.Pi,
				offset,
				Projectile.scale,
				elbowPoint.X > Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None,
				0
			);

			offset = new Vector2(5, 5);
			Main.EntitySpriteDraw(
				tex,
				elbowPoint - Main.screenPosition,
				new Rectangle(0, 16, 30, 10),
				Projectile.GetAlpha(lightColor),
				elbowRot,
				offset,
				Projectile.scale,
				elbowPoint.X > Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None,
				0
			);

			offset = new Vector2(7, 7);
			Main.EntitySpriteDraw(
				tex,
				Projectile.position + offset - Main.screenPosition,
				new Rectangle(0, 0, 32, 14),
				Projectile.GetAlpha(lightColor),
				Projectile.rotation,
				offset,
				Projectile.scale,
				Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None,
				0
			);
			return false;
		}
    }
}