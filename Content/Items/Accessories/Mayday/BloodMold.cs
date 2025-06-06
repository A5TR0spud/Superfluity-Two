using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Mayday
{
	public class BloodMold : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 10);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient(ItemID.ViciousMushroom, 13)
                .AddIngredient(ItemID.GlowingMushroom, 13)
                .AddTile(TileID.Bottles)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient(ItemID.VileMushroom, 13)
                .AddIngredient(ItemID.GlowingMushroom, 13)
                .AddTile(TileID.Bottles)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BloodMoldPlayer>().rawHasBloodMold = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<BloodMoldPlayer>().forceBloodMoldVisible = true;
        }
    }
}