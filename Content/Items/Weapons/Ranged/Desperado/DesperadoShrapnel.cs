using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Desperado
{
	public class DesperadoShrapnel : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 6;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.DamageType = DamageClass.Ranged;
		}

        public override void OnSpawn(IEntitySource source)
        {
			Dust.NewDustPerfect(
				Projectile.Center,
				DustID.GoldFlame,
				Projectile.velocity * 0.1f
			);
        }
		
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.FinalDamage /= 2;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
			modifiers.FinalDamage /= 2;
        }

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			return true;
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity) {
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

			return true;
		}
    }
}