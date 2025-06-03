using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Projectiles
{
	public class ShroomFume : ModProjectile
	{
		public bool isHeart = false;

        public override void SetStaticDefaults() {
			Main.projFrames[Type] = 2;
		}

		public override void SetDefaults()
        {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = ProjAIStyleID.ToxicCloud;
			Projectile.friendly = true;
			Projectile.hostile = false;
            Projectile.maxPenetrate = 2;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			isHeart = false;
			//Projectile.frame = 2;
			//Projectile.extraUpdates = 1;
            Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.ArmorPenetration = 18;
		}

        public override bool PreAI()
        {
			Projectile.frame = isHeart ? 1 : 0;
			return true;
        }
    }
}