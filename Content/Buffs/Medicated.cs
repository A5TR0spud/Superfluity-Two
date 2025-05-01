using SuperfluityTwo.Common.Players;
using SuperfluityTwo.Content.Items.Accessories.Mayday;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Buffs
{
    public class Medicated : ModBuff
    {
        public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
        }

		public override void Update(Player player, ref int buffIndex) {
            if (player.buffTime[buffIndex] == 1) {
                player.Heal((int)(player.GetModPlayer<MedkitPlayer>().health * 0.10f));
                player.DelBuff(buffIndex);
            }
            if (!player.GetModPlayer<MedkitPlayer>().isMedicatedForBuff) {
                player.DelBuff(buffIndex);
            }
		}

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}