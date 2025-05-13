using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Balance
{
    [AutoloadEquip(EquipType.Neck)]
	public class SweetToothNecklace : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(gold: 3, silver: 50);
			Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StingerNecklace)
                .AddIngredient(ItemID.SliceOfCake)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BalancePlayer>().sweetToothSpeeds = true;
            player.GetArmorPenetration(DamageClass.Generic) += 5;
            player.GetModPlayer<BalancePlayer>().honeyOnHitTime += 5 * 60;
        }
    }
}