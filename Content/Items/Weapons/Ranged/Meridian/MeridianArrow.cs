using System;
using System.Threading;
using log4net.Core;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Meridian
{
    public class MeridianArrow : ModProjectile
    {
        const int ANIMATION_TIME = 60;
        const int LIFETIME = 120 + ANIMATION_TIME;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Type] = 0; // The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 14;
            Projectile.timeLeft = LIFETIME;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            //Projectile.aiStyle = ProjAIStyleID.
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1 - Projectile.alpha / 255f);
        }

        float initSpeed = 10;

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            initSpeed = Projectile.velocity.Length();
            Projectile.velocity *= 0.1f;
            for (int i = 0; i < 12; i++)
            {
                Vector2 dir = (MathHelper.TwoPi * i / 12f).ToRotationVector2();
                Dust.NewDustPerfect(Projectile.Center + dir * 6, DustID.ShimmerSpark, dir * 0.3f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // If collide with tile, reduce the penetrate.
            Projectile.penetrate--;
            if (Projectile.penetrate == 0)
            {
                Projectile.Kill();
            }
            else
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

                BounceToTarget();
                if (!BounceToTarget())
                {
                /*// If the projectile hits the left or right side of the tile, reverse the X velocity
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }

                // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }*/
                    Projectile.Kill();
                }
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.95f);
            Projectile.velocity *= 0.5f;
            BounceToTarget();
            Projectile.timeLeft = LIFETIME;
        }

        const float HOMING_ROTATION_PER_TICK = 90f * MathHelper.Pi / 180f / 60f;
        const float HOMING_RANGE = 800;
        const float ACCELERATION = 1.07f;

        public override void AI()
        {
            Projectile.rotation += 0.2f * Projectile.direction;
            if (FadeOut())
            {
                Projectile.tileCollide = false;
                Projectile.penetrate = -1;
                Projectile.damage = 0;
            }
            else
            {
                float speed = Projectile.velocity.Length();
                speed = Math.Min(speed * ACCELERATION, initSpeed);
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                Home(out bool hasLoS);
                Projectile.tileCollide = hasLoS;
            }
        }

        private bool BounceToTarget()
        {
            int victimID = Projectile.FindTargetWithLineOfSight();
            if (victimID > 0)
            {
                float speed = Projectile.velocity.Length();
                Projectile.velocity = Projectile.DirectionTo(Main.npc[victimID].Center) * speed;
                return true;
            }
            return false;
        }

        private bool FadeOut()
        {
            if (Projectile.penetrate <= 0 && Projectile.timeLeft > ANIMATION_TIME) Projectile.timeLeft = ANIMATION_TIME;
            if (Projectile.timeLeft <= ANIMATION_TIME)
            {
                Projectile.alpha = (int)(255 * (1f - Projectile.timeLeft / (float)ANIMATION_TIME));
                Projectile.velocity *= 0.93f;
                return true;
            }
            return false;
        }

        private void Home(out bool hasLineOfSight)
        {
            NPC victim;
            if (Projectile.tileCollide)
            {
                int victimID = Projectile.FindTargetWithLineOfSight(HOMING_RANGE);
                if (victimID < 0)
                {
                    hasLineOfSight = false;
                    return;
                }
                victim = Main.npc[victimID];
            }
            else
            {
                victim = Projectile.FindTargetWithinRange(HOMING_RANGE);
                if (victim == null)
                {
                    hasLineOfSight = false;
                    return;
                }
            }

            float rot = Projectile.velocity.ToRotation();
            float targetAngle = Projectile.DirectionTo(victim.Center).ToRotation();
            rot = rot.AngleTowards(targetAngle, HOMING_ROTATION_PER_TICK);
            float speed = Projectile.velocity.Length();
            Projectile.velocity = rot.ToRotationVector2() * speed;
            hasLineOfSight = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, victim.position, victim.width, victim.height);
            return;
        }
        
        public override bool PreDraw(ref Color lightColor) {
			// Draws an afterimage trail. See https://github.com/tModLoader/tModLoader/wiki/Basic-Projectile#afterimage-trail for more information.

			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = Projectile.oldPos.Length - 1; k > 0; k--) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}
    }   
}