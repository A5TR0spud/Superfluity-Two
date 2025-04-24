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
            isMedicated = false;
        }
        public override void PostUpdateEquips()
        {
            isMedicatedForBuff = isMedicated;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (isMedicated)
                Player.AddBuff(ModContent.BuffType<Medicated>(), 8 * 60);
        }
    }
}