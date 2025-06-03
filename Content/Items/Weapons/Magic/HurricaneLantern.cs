using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Magic
{
	public class HurricaneLantern : ModItem
	{

		public override Vector2? HoldoutOrigin()
		{
			return new Vector2(0f, 0f);
		}

		// The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.SuperfluousSummoning.hjson file.
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Magic;
			Item.width = 18;
			Item.height = 32;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.knockBack = 5.5f;
			Item.value = Item.sellPrice(gold: 2); //3g, 1s
			Item.rare = ItemRarityID.LightPurple;
			Item.noMelee = true;
			Item.useTurn = false;
			Item.scale = 0.7f;
			Item.mana = 25;
			Item.shoot = ModContent.ProjectileType<Hurricane>();
			Item.shootSpeed = 4f;
			Item.UseSound = SoundID.Item8;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.NimbusRod)
				.AddIngredient(ItemID.RainCloud, 50)
				.AddIngredient(ItemID.SoulofFlight, 15)
				.AddIngredient(ItemID.Glass, 6)
				.AddIngredient(ItemID.Torch)
				.AddTile(TileID.MythrilAnvil)
				.AddCondition(Condition.NearWater)
				.Register();
		}
    }
}