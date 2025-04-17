using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.NPCs
{
    public class ReducedSpawnsNPC : GlobalNPC {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<AtaraxiaPlayer>().sunflowerHairpinReducedSpawns)
            {
                spawnRate = (int)((float)spawnRate * 1.2f);
                maxSpawns = (int)((float)maxSpawns * 0.8f);
            }
        }
    }
}