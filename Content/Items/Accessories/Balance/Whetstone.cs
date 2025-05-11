using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Balance
{
	public class Whetstone : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddRecipeGroup("IronBar")
                .AddTile(TileID.SharpeningStation)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetArmorPenetration(DamageClass.Melee) += 5;
            player.GetDamage(DamageClass.Melee) += 0.04f;
        }
    }
}