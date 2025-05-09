using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Solace
{
    [AutoloadEquip(EquipType.Face)]
	public class LuckyClover : ModItem
	{
        public override void SetStaticDefaults()
        {
            ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[EquipLoader.GetEquipSlot(Mod, this.Name, EquipType.Face)] = true;
        }

        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 20);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.lifeRegen = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GardenGnome)
                .AddIngredient(ItemID.Vine)
                .AddTile(TileID.Tables)
                .AddTile(TileID.Chairs)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SolacePlayer>().hasLuckyClover = true;
            player.GetCritChance(DamageClass.Generic) += 4;
        }
    }
}