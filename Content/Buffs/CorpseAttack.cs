using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SuperfluityTwo.Content.Buffs
{
    public class CorpseAttack : ModBuff
    {
        public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] % 40 == 0) {
                Dust.NewDust(player.position, player.width, player.height, DustID.JungleTorch, Alpha: 128);
            }
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
        }
    }
}