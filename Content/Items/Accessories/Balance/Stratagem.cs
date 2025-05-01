using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Balance
{
	public class Stratagem : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = Item.sellPrice(silver: 25);
			Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DD2ElderCrystal)
                .AddIngredient(ItemID.Wood, 20)
                .AddTile(TileID.WarTable)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AtaraxiaPlayer>().strats = true;
        }
    }
}