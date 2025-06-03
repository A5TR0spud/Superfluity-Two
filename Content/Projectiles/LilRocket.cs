using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Projectiles
{
    public class LilRocket : ModProjectile
    {
		private const int DefaultWidthHeight = 14;
		private const int ExplosionWidthHeight = 150;
        ref float timer => ref Projectile.ai[0];
        public override void SetStaticDefaults() {
			ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
			ProjectileID.Sets.Explosive[Type] = true;
		}

        public override void SetDefaults()
        {
            Projectile.width = DefaultWidthHeight;
            Projectile.height = DefaultWidthHeight;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            if (Projectile.timeLeft > 3) Projectile.timeLeft = 3;
            return false;
        }

        public override void AI()
        {
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.PrepareBombToBlow();
            }
            else
            {
                Visuals();
            }

            if (timer > 20)
            {
                Physics();
            }

            timer++;
        }

        private void Physics()
        {
            Projectile.velocity *= 0.98f;
            Projectile.velocity.Y += 0.3f;
            Projectile.velocity *= 1.03f;
        }

        private void Visuals()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Vector2 offset = Vector2.UnitX.RotatedBy(Projectile.rotation);
            offset *= -Projectile.width / 2;
            int dustID = Dust.NewDust(Projectile.Center + offset, 0, 0, DustID.GoldFlame, -Projectile.velocity.X, -Projectile.velocity.Y);
            Main.dust[dustID].noLight = true;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			// Vanilla explosions do less damage to Eater of Worlds in expert mode, so we will too.
			if (Main.expertMode) {
				if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail) {
					modifiers.FinalDamage /= 5;
				}
			}
		}

        public override void PrepareBombToBlow() {
			Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the bomb explodes on slopes.
			Projectile.alpha = 255; // Set to transparent. This projectile technically lives as transparent for about 3 frames

			// Change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
			Projectile.Resize(ExplosionWidthHeight, ExplosionWidthHeight);

			//Projectile.damage = 80; // Grenade: 60, Bomb: 100, Dynamite: 250
			//Projectile.knockBack = 8f; // Grenade: 8f, Bomb: 8f, Dynamite: 10f
		}

        public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            // Smoke Dust spawn
            for (int i = 0; i < 25; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1);
                dust.velocity *= 1.2f;
			}

			// Fire Dust spawn
			for (int i = 0; i < 8; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 100, default, 1);
				dust.noGravity = true;
				dust.velocity *= 3f;
				dust.noLight = true;
				dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1);
				dust.noGravity = true;
				dust.velocity *= 3f;
				dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 100, default, 1);
				dust.velocity *= 3f;
				dust.noLight = true;
				dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1);
				dust.velocity *= 3f;
				dust.noLight = true;
			}

			// Large Smoke Gore spawn
			for (int g = 0; g < 1; g++) {
				var goreSpawnPosition = new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f);
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.velocity.X += 0.25f;
				gore.velocity.Y += 0.25f;
				gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.velocity.X -= 0.25f;
				gore.velocity.Y += 0.25f;
				gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.velocity.X += 0.25f;
				gore.velocity.Y -= 0.25f;
				gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.velocity.X -= 0.25f;
				gore.velocity.Y -= 0.25f;
			}
			// reset size to normal width and height.
			Projectile.Resize(DefaultWidthHeight, DefaultWidthHeight);
		}

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            float theta = Projectile.rotation;
            Rectangle sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            //Vector2 offset = new Vector2(-23, 0).RotatedBy(theta);
            Main.EntitySpriteDraw(
                texture: tex,
                position: Projectile.position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + drawOrigin,// + offset,
                sourceRectangle: sourceRect,
                color: Projectile.GetAlpha(lightColor),
                rotation: theta,
                origin: drawOrigin,
                scale: 1,
                effects: SpriteEffects.None,
                worthless: 0
            );
            return false;
        }
    }   
}