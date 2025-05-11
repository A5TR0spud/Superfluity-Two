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
			Main.projFrames[Type] = 6;
		}

		public override void SetDefaults()
        {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 26;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.SummonMeleeSpeed;
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
			offset = Projectile.velocity;
			offset.Normalize();
			Projectile.ai[0] = offset.X;
			Projectile.ai[1] = offset.Y;
			Projectile.velocity *= 0.001f;
        }

		float animTimer = 0;
        public override bool PreAI()
        {
			Projectile.rotation = Projectile.ai[2];
			offset = new Vector2(Projectile.ai[0], Projectile.ai[1]);

			Player owner = Main.player[Projectile.owner];
			Projectile.position = owner.Center + offset * (offsetPower + 38) + new Vector2(2.375f * -owner.width, -0.3f * owner.height);
			Projectile.frame = (int)animTimer;
			animTimer += 6f/26f;
			offsetPower = animTimer % 1 * 16;
			
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			Player owner = Main.player[Projectile.owner];
			hitbox.X = (int)owner.Center.X;
			hitbox.Y = (int)owner.Center.Y;

			//hitbox.X += (int)(-2.375f * owner.width);
			hitbox.X -= hitbox.Width / 2;
			hitbox.Y -= (int)(0.3f * owner.height);
			//hitbox.Y -= hitbox.Height / 2;

			hitbox.X += (int)(offset.X * animTimer * 16f);
			hitbox.Y += (int)(offset.Y * animTimer * 16f);
        }

        public override void OnKill(int timeLeft)
        {
			Player owner = Main.player[Projectile.owner];
			for (int i = 0; i < 12; i++) 
            	Dust.NewDust(owner.Center + new Vector2(0, -0.3f * owner.height) + offset * i * 8, 16, 16, DustID.JunglePlants);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.damage = (int)(Projectile.damage * 0.66f);
        }
    }
}