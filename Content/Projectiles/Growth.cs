using System;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Projectiles
{
	public class Growth : ModProjectile
	{
        public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
        {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 26;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.ownerHitCheck = true;
		}

        public override bool? CanCutTiles()
        {
            return false;
        }

		Vector2 offset;
		float offsetPower = 0;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.ai[2] = Projectile.rotation;
			//Projectile.spriteDirection = Projectile.direction;
			//Projectile.knockBack *= Projectile.direction;
			offset = Projectile.velocity;
			offset.Normalize();
			Projectile.ai[0] = offset.X;
			Projectile.ai[1] = offset.Y;
			Projectile.velocity *= 0.001f;
        }

		float scaler = 0;
		float animTimer = 0;
        public override bool PreAI()
        {
			Projectile.rotation = Projectile.ai[2];
			offset = new Vector2(Projectile.ai[0], Projectile.ai[1]);

			Player owner = Main.player[Projectile.owner];
			Projectile.position = owner.Center + offset + offset * offsetPower + new Vector2(2.375f * -owner.width, -0.4f * owner.height);
			Projectile.scale = scaler;
			scaler += 1f/26f * 2f;
			offsetPower += 1.5f * 2f;
			scaler = Math.Min(scaler, 1);
			offsetPower = Math.Min(offsetPower, 39);
			Projectile.frame = (int)animTimer;
			animTimer += 4.5f/26f;
			
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			Player owner = Main.player[Projectile.owner];
			hitbox.X += (int)(2.375f * owner.width);
			hitbox.X -= hitbox.Width / 2;
			hitbox.Y += (int)(0.4f * owner.height);
			hitbox.Y -= hitbox.Height / 2;

			hitbox.X += (int)(offset.X * offsetPower * 1.25f);
			hitbox.Y += (int)(offset.Y * offsetPower * 1.25f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.damage = (int)(Projectile.damage * 0.66f);
        }
    }
}