using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Solace
{
    [AutoloadEquip(EquipType.Neck)]
	public class LoverLocket : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 36;
			Item.value = Item.sellPrice(gold: 1, silver: 50);
			Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.lifeRegen = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.HeartLamp] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HeartLantern)
                .AddIngredient(ItemID.Chain, 2)
                .AddTile(TileID.Tables)
                .AddTile(TileID.Chairs)
                .Register();
        }
    }
}