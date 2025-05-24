using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Projectiles
{
    public class MeridianBullet : ModProjectile
    {
        const int MAX_TIME = 600;
        const int NO_COLLIDE_TIME = 20;
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 3;
            //Projectile.aiStyle = ProjAIStyleID.
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return Projectile.timeLeft <= MAX_TIME - NO_COLLIDE_TIME;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.95f);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }   
}