using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Balance
{
    [AutoloadEquip(EquipType.Neck)]
	public class Bandolier : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MusketBall, 999)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile(TileID.AmmoBox)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AtaraxiaPlayer>().bandolier = true;
        }
    }
}