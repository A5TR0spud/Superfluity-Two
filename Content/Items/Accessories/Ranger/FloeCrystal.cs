using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class FloeCrystal : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 30;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().HasFloe = true;
        }
    }
}