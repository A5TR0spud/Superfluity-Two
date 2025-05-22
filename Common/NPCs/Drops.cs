using SuperfluityTwo.Common;
using SuperfluityTwo.Content.Items.Accessories.Mage;
using SuperfluityTwo.Content.Items.Accessories.Mayday;
using SuperfluityTwo.Content.Items.Accessories.Ranger;
using SuperfluityTwo.Content.Items.Accessories.Solace;
using SuperfluityTwo.Content.Items.Accessories.Summoner;
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
            if (npc.type == NPCID.AnomuraFungus || npc.type == NPCID.MushiLadybug || npc.type == NPCID.ZombieMushroom || npc.type == NPCID.ZombieMushroomHat)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Aranearum>(), 20));
                return;
            }
            if (npc.type == NPCID.IceBat)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FloeCrystal>(), 150));
                return;
            }
            if (npc.type == NPCID.IceElemental)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FloeCrystal>(), 50));
                return;
            }
            if (npc.type == NPCID.AngryTrapper)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TrapperLash>(), 50));
                return;
            }
            if (npc.type == NPCID.WalkingAntlion || npc.type == NPCID.GiantWalkingAntlion)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AntlionLeg>(), 50));
                return;
            }
            if (npc.type == NPCID.Antlion)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AntlionLeg>(), 30));
                return;
            }
            if (npc.type == NPCID.SwampThing)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LeafTail>(), 60));
                return;
            }
            if (npc.type == NPCID.Lihzahrd || npc.type == NPCID.LihzahrdCrawler)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<LeafTail>(), 100));
                return;
            }
            if (npc.type == NPCID.Salamander || npc.type == NPCID.Salamander2 || npc.type == NPCID.Salamander3 || npc.type == NPCID.Salamander4 || npc.type == NPCID.Salamander5 || npc.type == NPCID.Salamander6 || npc.type == NPCID.Salamander7 || npc.type == NPCID.Salamander8 || npc.type == NPCID.Salamander9)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<LeafTail>(), 100));
                return;
            }
            if (npc.type == NPCID.Crimera || npc.type == NPCID.BigCrimera || npc.type == NPCID.LittleCrimera || npc.type == NPCID.EaterofSouls || npc.type == NPCID.BigEater || npc.type == NPCID.LittleEater)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VileStone>(), 75));
                return;
            }
            if (npc.type == NPCID.RaggedCaster || npc.type == NPCID.RaggedCasterOpenCoat)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Zygoma>(), 20));
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<Zygoma>(), chanceNumerator: 39, chanceDenominator: 400));
                return;
            }
            if (npc.type == NPCID.DiabolistRed || npc.type == NPCID.DiabolistWhite)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<CultivatingFlame>(), 20));
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<CultivatingFlame>(), chanceNumerator: 39, chanceDenominator: 400));
                return;
            }
            if (npc.type == NPCID.GoblinSummoner)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<ShadowRite>(), 6, 3));
                return;
            }
            if (npc.type == NPCID.AngryNimbus)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ThunderScroll>(), 15));
                return;
            }
        }
    }
}