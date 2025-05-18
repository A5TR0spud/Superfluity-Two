using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SuperfluityTwo.Common;
using SuperfluityTwo.Content.Items.Weapons.Summon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Projectiles
{
	public class Growth : ModProjectile
	{
		public static Asset<Texture2D> texAsset;

        public override void Load()
        {
            texAsset = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Projectiles/Growth", AssetRequestMode.AsyncLoad);
        }

        /*public override void SetStaticDefaults() {
			Main.projFrames[Type] = 6;
		}*/

		public override void SetDefaults()
        {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = GetMaxDuration();
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.SummonMeleeSpeed;
		}

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

		public int GetMaxDuration(Player owner = null) {
			return 26;
			/*int returnValue = 26;
			if (owner == null) return returnValue;
			Item spawningItem = owner.HeldItem;
			if (spawningItem == null || spawningItem.ModItem == null || spawningItem.ModItem.GetType() != typeof(GrowthSpurt)) return returnValue;
			GrowthSpurt growthSpurt = (GrowthSpurt)spawningItem.ModItem;
			returnValue = 26 + growthSpurt.level;
			return returnValue;*/
		}

		int randomVisualOffset = 0;
		int randomVisualInterval = 0;
        public override void OnSpawn(IEntitySource source)
        {
			randomVisualOffset = Main.rand.Next(10);
			randomVisualInterval = (Main.rand.Next(3) + 1) * 2 + 1;
			Player owner = HelperMethodsSF2.TryGetOwner(Projectile);
			Projectile.timeLeft = GetMaxDuration(owner);
        }

		float animTimer = 0;
        public override bool PreAI()
        {
			Player owner = HelperMethodsSF2.TryGetOwner(Projectile);
			if (owner == null) return true;
			Projectile.position = owner.Center + new Vector2(-Projectile.Hitbox.Width / 2, -(int)(0.3f * owner.height));

			animTimer += 6f/26f * Projectile.scale;

			Projectile.gfxOffY = owner.gfxOffY; //it's that shrimple
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.spriteDirection = Projectile.velocity.X < 0 ? -1 : 1;
			
            return false;
        }

		public override bool PreDraw(ref Color lightColor) {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Color drawColor = Projectile.GetAlpha(lightColor);

			int anim = (int)animTimer;
			int stage;
			int variants;
			for (int i = 0; i <= anim; i++) {
				int j = anim - i;
				//bud
				if (anim == 0) {
					stage = 0;
					variants = 1;
				}
				//fully grown
				else if (j == anim) {
					stage = 3;
					variants = 5;
				}
				//bottom
				//a moving bottom?
				/*else if (j == 0) {
					stage = 1;
					variants = 4;
				}*/
				//stalk
				else {
					stage = 2;
					variants = 10;
				}

				int frameSize = 16;
				int padding = 2;
				int interval = frameSize + padding;

				int variant = (randomVisualOffset + randomVisualInterval * i) % variants;
				int startX = interval * variant;
				int startY = interval * stage;

				Rectangle sourceRectangle = new Rectangle(startX, startY, frameSize, frameSize);

				Vector2 origin = sourceRectangle.Size() / 2f;

				Vector2 offset = (j + animTimer % 1) * frameSize * Projectile.velocity;

				Main.EntitySpriteDraw(
					texture: texAsset.Value,
					position: Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + offset,
					sourceRectangle: sourceRectangle,
					color: drawColor,
					rotation: Projectile.rotation + MathHelper.PiOver2,
					origin: origin,
					scale: 1,
					effects: spriteEffects,
					worthless: 0
				);
			}

            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 2;
			height = 2;
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			Player owner = HelperMethodsSF2.TryGetOwner(Projectile);
			if (owner == null) return;
			hitbox.X = (int)owner.Center.X;
			hitbox.Y = (int)owner.Center.Y;

			hitbox.X -= hitbox.Width / 2;
			hitbox.Y -= (int)(0.3f * owner.height);

			hitbox.X += (int)(Projectile.velocity.X * animTimer * 16f);
			hitbox.Y += (int)(Projectile.velocity.Y * animTimer * 16f);
        }

        public override void OnKill(int timeLeft)
        {
			for (int i = 0; i <= 2 * (int)animTimer; i++) 
            	Dust.NewDust(Projectile.position + Projectile.velocity * i * 8, 16, 16, DustID.JunglePlants);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.damage = (int)(Projectile.damage * 0.66f);
        }
    }
}