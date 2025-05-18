using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Mage
{
	public class FlurryScroll : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagePlayer>().hasScroll = true;
            player.GetAttackSpeed(DamageClass.Magic) += 0.12f;
        }
    }
}