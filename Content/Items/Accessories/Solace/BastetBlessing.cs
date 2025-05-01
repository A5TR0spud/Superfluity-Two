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
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 20);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.CatBast] = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AtaraxiaPlayer>().bastDefense = true;
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