using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class AtaraxiaPlayer : ModPlayer {
        public bool sunflowerHairpinReducedSpawns = false;
        public bool sunflowerHairpinMoveSpeed = false;
        public bool bastDefense = false;
        public bool mantle = false;
        public bool bandolier = false;
        public bool focused = false;
        public bool shaman = false;
        public bool gnomed = false;
        public bool strats = false;
        public bool tooth = false;
        public bool stoned = false;
        public int honeyOnHitTime = 0;
        public override void ResetEffects()
        {
            sunflowerHairpinReducedSpawns = false;
            sunflowerHairpinMoveSpeed = false;
            mantle = false;
            bandolier = false;
            focused = false;
            bastDefense = false;
            shaman = false;
            honeyOnHitTime = 0;
            gnomed = false;
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
            if (sunflowerHairpinMoveSpeed) {
                Player.moveSpeed += 0.1f;
                Player.moveSpeed *= 1.1f;
            }
            if (bastDefense)
                Player.statDefense += 5;
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

        public override void ModifyLuck(ref float luck)
        {
            if (gnomed)
                luck += 0.2f;
        }

        public override void UpdateLifeRegen()
        {
            if (mantle)
                Player.lifeRegenTime += 2f;
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            if (mantle)
                regen *= 1.1f;
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            bool consume = true;
            if (bandolier) {
                consume = consume && !Main.rand.NextBool(5);
            }
            return consume;
        }
    }
}