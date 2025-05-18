using System;
using SuperfluityTwo.Content.Items.Accessories.Mage;
using SuperfluityTwo.Content.Tiles;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.World;
public class LootAdjuster : GlobalItem {
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.FrozenCrate || item.type == ItemID.FrozenCrateHard) {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FlurryScroll>(), 20));
        }
    }
}