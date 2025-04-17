using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class MedkitPlayer : ModPlayer {
        public bool isMedicated = false;
        public bool isMedicatedForBuff = false;
        public override void ResetEffects()
        {
            isMedicatedForBuff = isMedicated;
            isMedicated = false;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (isMedicated)
                Player.AddBuff(ModContent.BuffType<Medicated>(), 5 * 60);
        }
    }
}