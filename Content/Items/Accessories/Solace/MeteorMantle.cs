using Microsoft.Xna.Framework;
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
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Magiluminescence)
                .AddIngredient(ItemID.MeteoriteBar, 12)
                .AddIngredient(ItemID.Silk, 20)
                .AddTile(TileID.Campfire)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SolacePlayer>().hasMeteorMantle = true;
            player.GetModPlayer<SolacePlayer>().hasMeteorDOTReduce = true;
            Lighting.AddLight(player.Center, 0.5f * new Vector3(0.9f, 0.8f, 0.5f));
            player.hasMagiluminescence = true;
        }
    }
}