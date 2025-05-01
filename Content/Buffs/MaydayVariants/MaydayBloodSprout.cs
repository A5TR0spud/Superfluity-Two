using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SuperfluityTwo.Content.Buffs.MaydayVariants
{
    public class MaydayBloodSprout : ModBuff
    {
        public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] % 20 == 0) {
                Dust.NewDust(player.position, player.width, player.height, DustID.GlowingMushroom, Alpha: 128);
            }
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}