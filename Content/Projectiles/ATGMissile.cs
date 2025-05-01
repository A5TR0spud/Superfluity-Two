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
	public class ATGMissile : ModProjectile
	{
        public override void SetDefaults()
        {
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}

        Vector2 accelerator;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[2] = 0; //timer
            accelerator = Projectile.velocity;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        float? proximity = null;
        
        public override bool PreAI()
        {
            proximity = null;
            Projectile.ai[2]++;
            
            if (Projectile.ai[2] > 20)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy()
                    && npc.Center.Distance(Projectile.Center) < 180 * 16)
                    {
                        float p = npc.Center.Distance(Projectile.Center);
                        if (p >= proximity && proximity != null) continue;
                        proximity = p;
                        Projectile.ai[0] = npc.Center.X + npc.velocity.X;
                        Projectile.ai[1] = npc.Center.Y + npc.velocity.Y;
                    }
                }
            }

            if (proximity != null)
            {
                accelerator = new Vector2(Projectile.ai[0] - Projectile.Center.X, Projectile.ai[1] - Projectile.Center.Y);
                if (accelerator.Length() > 0.55f) {
                    accelerator.Normalize();
                    accelerator *= 0.55f;
                }
                Projectile.tileCollide = false;
                if (proximity < 8 * 16 || Projectile.timeLeft < 200)
                {
                    accelerator *= Projectile.velocity.Length() * 0.6f;
                    Projectile.velocity *= 0.95f;
                }
                Projectile.velocity += accelerator;
            } else if (Projectile.ai[2] > 20){
                accelerator = new Vector2(0, 0.55f);
                Projectile.tileCollide = true;
                Projectile.velocity += accelerator;
            }
            
            Projectile.velocity *= 0.98f;
            if (Projectile.velocity.Length() > 16f) {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 16f;
            }

            Projectile.rotation = ((Projectile.velocity + accelerator)/2).ToRotation();
            Vector2 offset = Vector2.UnitX.RotatedBy(Projectile.rotation);
            offset *= -1;
            int dustID = Dust.NewDust(Projectile.Center + offset, 0, 0, DustID.GoldFlame, -accelerator.X, -accelerator.Y);
            Main.dust[dustID].noLight = true;

            accelerator = Projectile.velocity;
            return false;
        }
    }   
}