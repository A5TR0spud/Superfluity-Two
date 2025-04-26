using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Buffs
{
    public class BloodMold : ModBuff
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
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}