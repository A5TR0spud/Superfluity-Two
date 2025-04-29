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
	public class ShroomFume : ModProjectile
	{
        /*public override void SetStaticDefaults() {
			Main.projFrames[Type] = 3;
		}*/

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
			//Projectile.extraUpdates = 1;
            Projectile.usesIDStaticNPCImmunity = true;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.ArmorPenetration = 24;
		}

        //float animTimer = 0;
        /*public override bool PreAI()
        {
            if (Projectile.timeLeft < 255) Projectile.alpha = 255 - Projectile.timeLeft;
            Projectile.rotation = Projectile.velocity.ToRotation();// - MathHelper.ToRadians(90);
			//Projectile.spriteDirection = Projectile.direction;
            //animTimer+=1f/20f;
            //Projectile.frame = (int)animTimer % 3;
            return false;
        }*/
    }
}