using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SuperfluityTwo.Content.Buffs
{
    public class BloodSprout : ModBuff
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
    }
}