using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Buffs.MaydayVariants
{
    public class MaydayBloodMold : ModBuff
    {
        public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] % 12 == 0) {
                Dust.NewDust(player.position, player.width, player.height, DustID.RedMoss, Alpha: 128);
            }
            if (player.buffTime[buffIndex] == 1) {
                player.Heal((int)(player.GetModPlayer<MedkitPlayer>().health * 0.10f));
            }
            /*if (!player.GetModPlayer<>().isMedicatedForBuff) {
                player.DelBuff(buffIndex);
            }*/
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}