using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
                Dust.NewDustDirect(player.position, player.width, player.height, DustID.GemAmber, Alpha: 63, Scale: 0.5f).velocity = Vector2.Zero;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen += 5;
            if (Main.rand.NextBool(4))
            {
                Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.GemAmber, Alpha: 63, Scale: 0.5f).velocity = Vector2.Zero;;
            }
        }
    }
}