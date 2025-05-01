using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class MaydayPlayer : ModPlayer {
        public bool HasMayday = false;
        public override void ResetEffects()
        {
            HasMayday = false;
        }

        public override void PostUpdate()
        {
            if (HasMayday && Player.statLife < Player.statLifeMax2 * 0.5) {
                Player.AddBuff(ModContent.BuffType<MatrixBarrier>(), 5);
            }
        }
    }
}