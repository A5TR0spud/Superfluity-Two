using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Mayday
{
    [AutoloadEquip(EquipType.Face)]
	public class Corpsebloom : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 36;
			Item.value = Item.sellPrice(copper: 50);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.JungleRose)
                .AddIngredient(ItemID.RottenChunk, 13)
                .AddTile(TileID.DemonAltar)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.JungleRose)
                .AddIngredient(ItemID.Vertebrae, 13)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CorpseBloomPlayer>().rawHasCorpseBloom = true;
        }
    }
}