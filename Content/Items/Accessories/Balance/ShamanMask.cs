using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Balance
{
    [AutoloadEquip(EquipType.Face)]
	public class ShamanMask : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood", 50)
                .AddIngredient(ItemID.FallenStar)
                .AddTile(TileID.BewitchingTable)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.05f;
        }
    }
}