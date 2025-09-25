using System;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Items.Weapons.Ranged.Desperado;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Standoff
{
	public class Standoff : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.width = 36;
			Item.height = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 17f;
			Item.UseSound = SoundID.Item41;
			Item.useAnimation = 35;
			Item.useTime = 35;
			Item.noMelee = true;
			Item.crit = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Blue;
			Item.useTurn = false;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.scale = 0.8f;
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Desperado.Desperado>())
				.AddIngredient(ItemID.TissueSample, 5)
				.AddIngredient(ItemID.Obsidian, 20)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Desperado.Desperado>())
				.AddIngredient(ItemID.ShadowScale, 5)
				.AddIngredient(ItemID.Obsidian, 20)
				.AddTile(TileID.Anvils)
				.Register();
		}
		
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = new Vector2(32, -2 * player.direction).RotatedBy(velocity.ToRotation());
			foreach (var npc in Main.ActiveNPCs)
			{
				if (!npc.friendly && player.CanNPCBeHitByPlayerOrPlayerProjectile(npc) && player.CanHit(npc) && npc.Hitbox.IntersectsConeFastInaccurate(player.Center, 16 * 6, velocity.ToRotation(), (float)Math.PI * 0.06125f))
				{
					type = ModContent.ProjectileType<StandoffExplosion>();
					position += muzzleOffset;
					break;
				}
			}
        }

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			float m = player.itemAnimationMax;
			float d = 1.0f - player.itemAnimation / m;
			if (d > 0.5f)
			{
				float d1 = 2.0f * (d - 1.0f);
				d1 = 0.5f - 0.5f * (float)Math.Cos(Math.PI * d1);
				player.itemRotation = d1 * player.direction * 2.0f * (float)Math.PI;
				player.itemLocation -= player.direction * 20f * new Vector2((float)Math.Cos(player.itemRotation), (float)Math.Sin(player.itemRotation));
				player.itemLocation.X += 8f * player.direction;
			}
        }

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(2, 4);
		}
    }
}