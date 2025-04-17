using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Other
{
    [AutoloadEquip(EquipType.Waist)]
	public class Medkit : ModItem
	{
        public int healAmount = 0;
        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(gold: 2, silver: 10);
			Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Mushroom, 15)
                .AddIngredient(ItemID.HealingPotion, 5)
                .AddIngredient(ItemID.GlowingMushroom, 15)
                .AddIngredient(ItemID.MedicatedBandage)
                .AddTile(TileID.Tables)
                .AddTile(TileID.Chairs)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MedkitPlayer>().isMedicated = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Bleeding] = true;
        }
    }
}