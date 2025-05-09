using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Solace
{
	public class BastetBlessing : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 20);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.defense = 2;
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SolacePlayer>().hasBastetBlessing = true;
            player.GetModPlayer<SolacePlayer>().bastetAnkhVisible = !hideVisual;
            player.GetModPlayer<SolacePlayer>().bastetVisible = !hideVisual;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<SolacePlayer>().forceBastetVisible = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Shackle)
                .AddIngredient(ItemID.Obsidian, 20)
                .AddTile(TileID.CatBast)
                .Register();
        }
    }
}