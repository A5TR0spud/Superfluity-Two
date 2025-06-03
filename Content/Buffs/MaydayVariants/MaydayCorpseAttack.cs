using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SuperfluityTwo.Content.Buffs.MaydayVariants
{
    public class MaydayCorpseAttack : ModBuff
    {
        public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] % 40 == 0) {
                Dust.NewDust(player.position, player.width, player.height, DustID.GreenTorch, Alpha: 128);
            }
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
            //player.moveSpeed += 1.0f;
        }
    }
}