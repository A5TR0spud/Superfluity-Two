using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class LeafTail : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 22;
			Item.value = Item.sellPrice(gold: 3, silver: 50);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().HasLeafTailStealth = true;
            player.GetModPlayer<RangerPlayer>().HasLeafTailDodge = true;
        }
    }
}