using Microsoft.Xna.Framework;
using SuperfluityTwo.Common;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.SuperSniper
{
	public class SuperSniper : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 30f;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.reuseDelay = 12;
			Item.noMelee = true;
			Item.crit = 29;
			Item.damage = 250;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 8f;
			Item.value = Item.sellPrice(gold: 15);
			Item.rare = ItemRarityID.Yellow;
			//Item.useTurn = false;
			Item.shoot = ModContent.ProjectileType<SuperSniperHoldout>();
			Item.noUseGraphic = true;
			Item.channel = true;
			ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SniperRifle)
				.AddIngredient(ItemID.IllegalGunParts)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			damage = player.HeldItem.damage;
			knockback = player.HeldItem.knockBack;
			velocity = velocity.SafeNormalize(-Vector2.UnitY) * player.HeldItem.shootSpeed;
			type = ModContent.ProjectileType<SuperSniperHoldout>();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[ModContent.ProjectileType<SuperSniperHoldout>()] <= 0;
		}
	}

	internal class SuperSniperPlayer : ModPlayer
	{
        public override void ModifyZoom(ref float zoom)
        {
			const float SuperSniperZoom = 0.6666f;
			if (Main.myPlayer == Player.whoAmI && Player.HeldItem.type == ModContent.ItemType<SuperSniper>() && Main.mouseRight)
			{
				if (zoom < 0) zoom = 0;
				zoom = 1 - (1 - zoom) * (1 - SuperSniperZoom);
			}
        }
	}
}