using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class TrapperLash : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = Item.sellPrice(gold: 3, silver: 50);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().HasTrapperLash = true;
            player.whipRangeMultiplier += 0.12f;
            player.GetKnockback(DamageClass.Ranged) += 0.50f;
        }
    }
}