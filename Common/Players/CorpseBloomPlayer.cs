using System;
using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

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

        public override void ResetEffects()
        {
            rawHasCorpseBloom = false;
            rawHasHeart = false;
        }

        public override void PostUpdateEquips()
        {
            hasCorpseBloom = rawHasCorpseBloom || rawHasHeart;
            hasHeart = rawHasHeart;
            if (!hasCorpseBloom) {
                Player.ClearBuff(ModContent.BuffType<CorpseBuff>());
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if ((target.life <= 0 || hit.InstantKill) && hasCorpseBloom) {
                Player.AddBuff(ModContent.BuffType<CorpseBuff>(), FramesPerKill);
            }
        }
    }
}
