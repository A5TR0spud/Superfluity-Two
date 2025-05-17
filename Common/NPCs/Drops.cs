using SuperfluityTwo.Common;
using SuperfluityTwo.Content.Items.Accessories.Mage;
using SuperfluityTwo.Content.Items.Accessories.Mayday;
using SuperfluityTwo.Content.Items.Accessories.Ranger;
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
            if (npc.type == NPCID.AnomuraFungus || npc.type == NPCID.MushiLadybug || npc.type == NPCID.ZombieMushroom || npc.type == NPCID.ZombieMushroomHat) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Aranearum>(), 20));
                return;
            }
            if (npc.type == NPCID.IceBat) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FloeCrystal>(), 150));
                return;
            }
            if (npc.type == NPCID.IceElemental) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FloeCrystal>(), 50));
                return;
            }
            if (npc.type == NPCID.AngryTrapper) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TrapperLash>(), 50));
                return;
            }
            if (npc.type == NPCID.WalkingAntlion || npc.type == NPCID.GiantWalkingAntlion) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AntlionLeg>(), 50));
                return;
            }
            if (npc.type == NPCID.Antlion) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AntlionLeg>(), 30));
                return;
            }
            if (npc.type == NPCID.SwampThing) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LeafTail>(), 60));
                return;
            }
            if (npc.type == NPCID.Lihzahrd || npc.type == NPCID.LihzahrdCrawler) {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<LeafTail>(), 100));
                return;
            }
            if (npc.type == NPCID.Salamander || npc.type == NPCID.Salamander2 || npc.type == NPCID.Salamander3 || npc.type == NPCID.Salamander4 || npc.type == NPCID.Salamander5 || npc.type == NPCID.Salamander6 || npc.type == NPCID.Salamander7 || npc.type == NPCID.Salamander8 || npc.type == NPCID.Salamander9) {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<LeafTail>(), 100));
                return;
            }
            if (npc.type == NPCID.RaggedCaster || npc.type == NPCID.RaggedCasterOpenCoat) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Zygoma>(), 20));
                return;
            }
            /*if (npc.type == NPCID.DiabolistRed || npc.type == NPCID.DiabolistWhite || npc.type == NPCID.Necromancer || npc.type == NPCID.NecromancerArmored) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Zygoma>(), 100));
                return;
            }*/
        }
    }
}