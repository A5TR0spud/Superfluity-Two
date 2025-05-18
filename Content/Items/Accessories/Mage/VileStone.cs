using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Mage
{
	public class VileStone : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagePlayer>().hasVileStone = true;
        }
    }
}