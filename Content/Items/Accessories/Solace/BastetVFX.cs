using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Solace
{
    public class BastetVFX : ModProjectile
    {
        ref float timer => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 38;
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            timer = 0;
        }

        public override void AI()
        {
            FadeInAndOut();

            Projectile.velocity *= 0.99f;

            timer++;
        }

        private void FadeInAndOut()
        {
            if (timer < 20)
            {
                Projectile.Opacity = timer / 20f;
            }
            if (Projectile.timeLeft < 20)
            {
                Projectile.Opacity = Projectile.timeLeft / 20f;
            }
        }
    }   
}