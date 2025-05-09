using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Solace
{
    [AutoloadEquip(EquipType.Back, EquipType.Front)]
	public class MeteorMantle : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 40);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.lifeRegen = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 3)
                .AddIngredient(ItemID.Silk, 10)
                .AddTile(TileID.Campfire)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.Campfire] = true;
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AtaraxiaPlayer>().mantle = true;
        }
    }
}