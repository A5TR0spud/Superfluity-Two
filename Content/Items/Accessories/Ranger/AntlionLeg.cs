using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class AntlionLeg : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 24;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().HasAntlionLeg = true;
            player.moveSpeed += 0.12f;
        }
    }
}