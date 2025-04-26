using System;
using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SuperfluityTwo.Content.Buffs
{
    public class CorpseBuff : ModBuff
    {
        public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] % 40 == 0) {
                Dust.NewDust(player.position, player.width, player.height, DustID.JungleSpore, Alpha: 128);
            }
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            player.buffTime[buffIndex] = Math.Clamp(player.buffTime[buffIndex] + time, CorpseBloomPlayer.MinCorpseFrames, CorpseBloomPlayer.MaxCorpseFrames);
            return true;
        }
    }
}