using Terraria;
using Terraria.DataStructures;
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
			Projectile.ArmorPenetration = 18;
			Projectile.DamageType = DamageClass.Ranged;
		}

        public override bool PreAI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation();
            return true;
        }
    }
}