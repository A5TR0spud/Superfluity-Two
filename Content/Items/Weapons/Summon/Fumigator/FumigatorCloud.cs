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
	public class FumigatorCloud : ModProjectile
	{
		ref float timer => ref Projectile.ai[0];
		ref float angularVel => ref Projectile.localAI[0];
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
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
			behindNPCs.Add(index);
		}

		public override bool PreAI()
		{
			FadeInAndOut();
			timer++;
			Projectile.velocity *= 0.99f;
			Projectile.rotation += angularVel;

			float overlapVelocity = 0.001f;
			foreach (var other in Main.ActiveProjectiles)
			{
				if (other.type != Projectile.type)
				{
					continue;
				}
				if (other.whoAmI != Projectile.whoAmI && other.owner == Projectile.owner && Projectile.Center.Distance(other.Center) < Projectile.width)
				{
					Vector2 dir = other.Center.DirectionTo(Projectile.Center);
					Projectile.velocity += dir * overlapVelocity;
				}
			}

			Projectile.velocity.Y += 0.01f;
			Projectile.friendly = Projectile.timeLeft > 20 && Projectile.friendly;
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
			Projectile.scale = 0.75f + timer / 240f;
		}

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

            SpriteEffects spriteEffects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture, drawPos, null, color * 0.33f, Projectile.rotation, drawOrigin, Projectile.scale * 1.33f, spriteEffects, 0);

            return false;
        }
    }
}