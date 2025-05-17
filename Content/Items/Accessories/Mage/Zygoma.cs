using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Mage;
public class Zygoma : ModItem {
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 24;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 5);
        Item.rare = ItemRarityID.Yellow;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<MagePlayer>().hasZygoma = true;
    }
}