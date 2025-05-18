using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Mayday
{
	public class Aranearum : GlowItem
	{
        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 26;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FunGuy>().rawHasAra = true;
        }
    }
}