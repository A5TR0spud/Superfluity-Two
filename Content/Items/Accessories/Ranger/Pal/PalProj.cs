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
				return Vanity == 1;
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
			if (Projectile.TryGetOwner(out Player player) && Projectile.friendly && !Projectile.hostile && player.GetModPlayer<PalPlayer>().hasPal && Projectile.type == type && player.ownedProjectileCounts[type] <= 1)
			{
				Projectile.timeLeft = 2;
			}
			else
			{
				Projectile.Kill();
				return;
			}

			int facing;
			bool yesTarget = UpdateTargetting(out facing);
			if (facing == 0)
			{
				facing = player.direction;
			}
			if (!yesTarget)
			{
				Projectile.rotation = facing == -1 ? MathHelper.Pi : 0;
			}
			Vector2 targetPosAbsolute = player.Top + new Vector2(facing * -8, -18);
			Vector2 targetPosRelative = targetPosAbsolute - Projectile.Center;

			overlappingVelocity = overlappingVelocity.MoveTowards(targetPosRelative, 0.2f);
			if (overlappingVelocity.LengthSquared() > 16) overlappingVelocity = 4 * overlappingVelocity.SafeNormalize(Vector2.Zero);

			playerProvidedVelocity = player.velocity * 0.9f;

			if (Projectile.Center.Distance(player.Center) > 16 * 4)
			{
				Projectile.Center = targetPosAbsolute;
				overlappingVelocity = Vector2.Zero;
				playerProvidedVelocity = player.velocity;
			}

			float drag = 0.9f;
			Projectile.position += overlappingVelocity + playerProvidedVelocity;
			Projectile.velocity *= drag;
			overlappingVelocity *= drag;
			playerProvidedVelocity *= drag;

			elbowPoint = Projectile.Center;
			elbowPoint.Y = player.Center.Y;
			elbowPoint.X += -2 * facing;
			float dist = 30;
			float threshold = 0.1f;
			for (int i = 0; i < 2; i++)
			{
				if (elbowPoint.Distance(Projectile.Center) < dist - threshold)
				{
					elbowPoint += -elbowPoint.DirectionTo(Projectile.Center) * (dist - elbowPoint.Distance(Projectile.Center));
				}
				if (elbowPoint.Distance(player.Center) < dist - threshold)
				{
					elbowPoint += -elbowPoint.DirectionTo(player.Center) * (dist - elbowPoint.Distance(player.Center));
				}
				if (elbowPoint.Distance(Projectile.Center) > dist + threshold)
				{
					elbowPoint = elbowPoint.MoveTowards(Projectile.Center, elbowPoint.Distance(Projectile.Center) - dist);
				}
				if (elbowPoint.Distance(player.Center) > dist + threshold)
				{
					elbowPoint = elbowPoint.MoveTowards(player.Center, elbowPoint.Distance(player.Center) - dist);
				}
			}
			elbowRot = elbowPoint.AngleTo(Projectile.Center);
			armRot = elbowPoint.AngleFrom(player.Center);

			Projectile.spriteDirection = facing;
		}

		public bool UpdateTargetting(out int facing)
		{
			Projectile.TryGetOwner(out Player player);
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