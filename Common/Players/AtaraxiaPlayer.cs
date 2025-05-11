using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class AtaraxiaPlayer : ModPlayer {
        public bool bandolier = false;
        public bool focused = false;
        public bool shaman = false;
        public bool strats = false;
        public bool tooth = false;
        public bool stoned = false;
        public int honeyOnHitTime = 0;
        public override void ResetEffects()
        {
            bandolier = false;
            focused = false;
            shaman = false;
            honeyOnHitTime = 0;
            strats = false;
            tooth = false;
            stoned = false;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (honeyOnHitTime > 0) {
                Player.AddBuff(BuffID.Honey, honeyOnHitTime);
            }
        }

        public override void UpdateEquips()
        {
            if (focused) {
                Player.GetCritChance(DamageClass.Magic) += 2;
                Player.GetDamage(DamageClass.Magic) += 0.05f;
                Player.manaCost -= 0.02f;
                Player.statManaMax2 += 20;
            }
            if (shaman)
                Player.maxMinions += 1;
            if (strats)
                Player.maxTurrets += 1;
            if (tooth) {
                Player.pickSpeed -= 0.2f;
                Player.moveSpeed += 0.2f;
            }
            if (stoned) {
                Player.GetArmorPenetration(DamageClass.Melee) += 12;
            }
        }
    }
}