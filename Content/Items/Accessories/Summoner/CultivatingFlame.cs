using Terraria;
using Terraria.ID;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Summoner
{
	public class CultivatingFlame : GlowItem
	{
        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 16;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SummonPlayer>().hasCultivatingFlame = true;
            player.maxMinions += 1;
        }
    }
}