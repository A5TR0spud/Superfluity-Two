using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class SkipjackFin : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 3, silver: 50);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ignoreWater = true;
            //player.accFlipper = true;
            player.GetModPlayer<RangerPlayer>().HasSkipjackSpeed = true;
            player.GetModPlayer<RangerPlayer>().HasSkipjackStealth = true;
        }
    }
}