using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Items
{
    public class SF2GlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (SF2ItemSets.isDebugItem[item.type])
            {
                tooltips.Add(new TooltipLine(ModContent.GetInstance<SuperfluityTwo>(), "SF2DebugTooltip", Language.GetTextValue($"Mods.{nameof(SuperfluityTwo)}.Items.Tooltips.Debug")));
            }
        }
    }
}