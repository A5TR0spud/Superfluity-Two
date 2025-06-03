using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class MedkitPlayer : ModPlayer {
        public bool isMedicated = false;
        public bool isMedicatedForBuff = false;
        public int health = 200;
        public override void ResetEffects()
        {
            isMedicated = false;
        }
        public override void PostUpdateEquips()
        {
            isMedicatedForBuff = isMedicated;
        }

        public override void PostUpdate()
        {
            health = Player.statLifeMax2;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (isMedicated)
                Player.AddBuff(ModContent.BuffType<Medicated>(), 8 * 60);
        }
    }
}