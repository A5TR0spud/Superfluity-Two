using SuperfluityTwo.Common;
using SuperfluityTwo.Content.Items.Accessories.Mayday;
using SuperfluityTwo.Content.Items.Accessories.Solace;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.NPCs
{
	public class Drops : GlobalNPC
	{
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Gnome) {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LuckyClover>(), 2));
                return;
			}
            /*if (npc.type == NPCID.SporeBat) {
                npcLoot.Add(ItemDropRule.ByCondition(new SuperfluousConditions.DownedEowOrBoc(), ModContent.ItemType<BloodMold>(), 1, 30, 90));
            }*/
            if (npc.type == NPCID.AnomuraFungus || npc.type == NPCID.MushiLadybug || npc.type == NPCID.ZombieMushroom || npc.type == NPCID.ZombieMushroomHat) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Aranearum>(), 30));
                return;
            }
        }
    }
}