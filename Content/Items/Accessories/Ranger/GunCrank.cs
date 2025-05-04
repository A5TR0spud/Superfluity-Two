using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class GunCrank : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 26;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().HasCrank = true;
        }
    }
}