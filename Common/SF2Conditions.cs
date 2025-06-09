using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace SuperfluityTwo.Common
{
    public class SF2Conditions
    {
        public static readonly Condition EncounteredAnyFairy = new Condition("Mods.SuperfluityTwo.Conditions.EncounteredAnyFairy", () =>
        {
            bool blueFairy = Main.BestiaryDB.FindEntryByNPCID(NPCID.FairyCritterBlue).UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
            bool greenFairy = Main.BestiaryDB.FindEntryByNPCID(NPCID.FairyCritterGreen).UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
            bool pinkFairy = Main.BestiaryDB.FindEntryByNPCID(NPCID.FairyCritterPink).UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
            return blueFairy || greenFairy || pinkFairy;
        });
    }
}