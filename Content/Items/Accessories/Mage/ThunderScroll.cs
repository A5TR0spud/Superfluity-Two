using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Mage
{
	public class ThunderScroll : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 32;
			Item.value = Item.sellPrice(gold: 3, silver: 50);
			Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagePlayer>().hasThunderScroll = true;
            player.GetDamage(DamageClass.Magic) += 0.06f;
        }
    }
}