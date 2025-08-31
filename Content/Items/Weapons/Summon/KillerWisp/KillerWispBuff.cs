using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Summon.KillerWisp
{
    public class KillerWispBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<KillerWispMinion>()] > 0) {
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
    }
}