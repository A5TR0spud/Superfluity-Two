using System;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Projectiles
{
	public class Shockwave : ModProjectile
	{
		internal const int MAX_TIME = 10;
		public override void SetDefaults()
		{
			Projectile.width = 128;
			Projectile.height = 128;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = MAX_TIME;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.ArmorPenetration = 18;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.alpha = 255;
		}

        public override bool? CanCutTiles()
        {
            return false;
        }

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.ai[0] = 0;
			Projectile.scale = 0.25f;
		}

        public override bool ShouldUpdatePosition()
        {
			return false;
        }

		public override bool PreAI()
		{
			Projectile.ai[0] += 1;
			Projectile.scale = 0.25f + 1.25f * (Projectile.ai[0] / MAX_TIME);
			Projectile.alpha = (int)(255 * (Projectile.ai[0] / MAX_TIME));
			return false;
		}
    }
}