using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Chromagem.Auribloom
{
    public class GoldBloodBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 5;
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.GemAmber, Alpha: 0);
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen += 5;
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.GemAmber, Alpha: 0);
            }
        }
    }
}