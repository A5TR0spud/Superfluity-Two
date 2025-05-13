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
	public class Strataslash : ModProjectile
	{
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SuperStarSlash);
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			/*Projectile.timeLeft = 10 * Projectile.MaxUpdates;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 2;
			Projectile.tileCollide = false;
			Projectile.scale = 1f + (float)Main.rand.Next(30) * 0.01f;
            Projectile.aiStyle = ProjectileID.SuperStarSlash;//152;//ProjectileID.SuperStarSlash;*/
			//Projectile.usesIDStaticNPCImmunity = true;
			//Projectile.idStaticNPCHitCooldown = 10;
            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCHitCooldown = 10;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 200);
        }
    }
}