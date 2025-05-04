using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class MuzzleExtension : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 26;
			Item.value = Item.sellPrice(gold: 3, silver: 50);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().HasMuzzle = true;
        }
    }
}