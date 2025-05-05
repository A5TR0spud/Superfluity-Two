using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class Archerfish : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 24;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AntlionLeg>())
                .AddIngredient(ModContent.ItemType<TrapperLash>())
                .AddIngredient(ModContent.ItemType<FloeCrystal>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().HasTrapperLash = true;
            player.GetModPlayer<RangerPlayer>().HasAntlionLeg = true;
            player.GetModPlayer<RangerPlayer>().HasFloe = true;
        }
    }
}