using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class CrankMuzzle : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GunCrank>())
                .AddIngredient(ModContent.ItemType<MuzzleExtension>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().HasMuzzle = true;
            player.GetModPlayer<RangerPlayer>().HasCrank = true;
        }
    }
}