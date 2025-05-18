using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Mage;
public class Zygoma : GlowItem {
    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 26;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 5);
        Item.rare = ItemRarityID.Yellow;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<MagePlayer>().hasZygoma = true;
    }
}