using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Common.Players;
using SuperfluityTwo.Content.Items.Accessories.Mayday;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Buffs
{
    public class Starsplit : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            Main.debuff[Type] = true;
            //BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<StarsplitNPC>().starStruck = true;
            if (npc.buffTime[buffIndex] % 13 == 0)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.ManaRegeneration, newColor: Colors.RarityPink);
            }
            if (npc.buffTime[buffIndex] % 7 == 0)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.ShimmerSpark);
            }
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }

    internal class StarsplitNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool starStruck = false;
        public override void ResetEffects(NPC npc)
        {
            starStruck = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (starStruck)
            {
                if (npc.lifeRegen > 0) npc.lifeRegen = 0;
                npc.lifeRegen -= 40;
                if (damage < 5) {
					damage = 5;
				}
            } 
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (starStruck) modifiers.FinalDamage *= 1.1f;
        }
    }
}