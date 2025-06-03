using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.ModLoader;
using SuperfluityTwo.Content.Buffs.MaydayVariants;

namespace SuperfluityTwo.Common.Players
{
    public class CorpseBloomPlayer : ModPlayer {
        public bool hasCorpseBloom = false;
        public bool rawHasCorpseBloom = false;
        public static readonly int FramesPerKill = 4 * 60;
        public static readonly int MaxCorpseFrames = 20 * 60;
        public static readonly int HealthPerKill = 20;
        static readonly public int HPT = HealthPerKill * 120 / FramesPerKill;
        public bool rawHasHeart = false;
        public bool hasHeart = false;
        public bool rawHasMayday = false;
        public bool HasMayday = false;

        public override void ResetEffects()
        {
            rawHasCorpseBloom = false;
            rawHasHeart = false;
            rawHasMayday = false;
        }

        public override void PostUpdateEquips()
        {
            hasCorpseBloom = rawHasCorpseBloom || rawHasHeart || rawHasMayday;
            hasHeart = rawHasHeart;
            HasMayday = rawHasMayday;
            if (!hasCorpseBloom) {
                int buffToCheck = HasMayday ? ModContent.BuffType<MaydayCorpseBuff>() : ModContent.BuffType<CorpseBuff>();
                Player.ClearBuff(buffToCheck);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if ((target.life <= 0 || hit.InstantKill) && hasCorpseBloom) {
                int buffToCheck = HasMayday ? ModContent.BuffType<MaydayCorpseBuff>() : ModContent.BuffType<CorpseBuff>();
                Player.AddBuff(buffToCheck, FramesPerKill);
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            int buffToCheck = HasMayday ? ModContent.BuffType<MaydayCorpseAttack>() : ModContent.BuffType<CorpseAttack>();
            if (hasCorpseBloom) {
                Player.AddBuff(buffToCheck, 5 * 60);//HasMayday ? 8 * 60 : 5 * 60);
            }
        }
    }
}
