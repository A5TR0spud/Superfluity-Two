using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;
using SuperfluityTwo.Content.Buffs;

namespace SuperfluityTwo.Content.Items.Mayday
{
	public class HeartOfDecay : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 36;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Aranearum>())
                .AddIngredient(ModContent.ItemType<BloodMold>())
                .AddIngredient(ModContent.ItemType<Corpsebloom>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FunGuy>().rawHasHeart = true;
            player.GetModPlayer<BloodMoldPlayer>().rawHasHeart = true;
            player.GetModPlayer<CorpseBloomPlayer>().rawHasHeart = true;
        }
    }
}