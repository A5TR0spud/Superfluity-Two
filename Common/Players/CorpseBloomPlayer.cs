using System;
using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class CorpseBloomPlayer : ModPlayer {
        public bool hasCorpseBloom = false;
        public bool rawHasCorpseBloom = false;
        static readonly public int DoubleRegenPerSecond = 11;
        static readonly public float InflictMultiplier = 2; //ticks per damage dealt
        static readonly public float SufferMultiplier = 10; //ticks per damage taken
        static readonly public int MaxCorpseFrames = 10 * 60;
        static readonly public int MinCorpseFrames = 60;
        public override void ResetEffects()
        {
            //base.ResetEffects();
            rawHasCorpseBloom = false;
        }

        public override void PostUpdateEquips()
        {
            //base.PostUpdateEquips();
            hasCorpseBloom = rawHasCorpseBloom;
        }

        public override void UpdateLifeRegen()
        {
            if (Player.HasBuff<CorpseBuff>()) {
                Player.lifeRegen += DoubleRegenPerSecond;
            }
        }

        public void ApplyCorpseBuff(int frames) {
            if (!Player.HasBuff(ModContent.BuffType<CorpseBuff>())) {
                frames = Math.Max(frames, MinCorpseFrames);
            }
            frames = Math.Min(frames, MaxCorpseFrames);
            Player.AddBuff(ModContent.BuffType<CorpseBuff>(), frames);
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (hasCorpseBloom) {
                ApplyCorpseBuff((int)(info.SourceDamage * SufferMultiplier));
            }
        }
    }
}