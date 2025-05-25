using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Meridian
{
	public class Meridian : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 52;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 30f;
			Item.useAnimation = 9;
			Item.useTime = 9;
			Item.reuseDelay = 12;
			Item.noMelee = true;
			Item.crit = 29;
			Item.damage = 190;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 3.5f;
			Item.value = Item.sellPrice(gold: 20);
			Item.rare = ItemRarityID.Red;
			//Item.useTurn = false;
			Item.shoot = ModContent.ProjectileType<MeridianHoldout>();
			Item.noUseGraphic = true;
			Item.channel = true;
			ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SDMG)
				.AddIngredient(ItemID.Celeb2)
				.AddIngredient(ItemID.RocketLauncher)
				.AddIngredient(ItemID.Xenopopper)
				.AddIngredient(ItemID.StakeLauncher)
				.AddIngredient(ItemID.Tsunami)
				.AddIngredient(ItemID.VenusMagnum)
				.AddIngredient(ItemID.Megashark)
				.AddIngredient(ItemID.PhoenixBlaster)
				.AddIngredient(ItemID.TheUndertaker)
				.AddIngredient(ItemID.EndlessMusketPouch)
				.AddIngredient(ItemID.EndlessQuiver)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.SDMG)
				.AddIngredient(ItemID.Celeb2)
				.AddIngredient(ItemID.RocketLauncher)
				.AddIngredient(ItemID.Xenopopper)
				.AddIngredient(ItemID.StakeLauncher)
				.AddIngredient(ItemID.Tsunami)
				.AddIngredient(ItemID.VenusMagnum)
				.AddIngredient(ItemID.Megashark)
				.AddIngredient(ItemID.PhoenixBlaster)
				.AddIngredient(ItemID.Musket)
				.AddIngredient(ItemID.EndlessMusketPouch)
				.AddIngredient(ItemID.EndlessQuiver)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[ModContent.ProjectileType<MeridianHoldout>()] <= 0;
		}
	}
}