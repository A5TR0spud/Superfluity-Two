using System;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class BloodMoldPlayer : ModPlayer {
        public bool hasBloodMold = false;
        public bool rawHasBloodMold = false;
        public float damageLeft = -1;
        static readonly public int TimeForHit = 8 * 60;
        static readonly public int TimeForInv = 6 * 60;
        static readonly public int InvHitPenalty = 1 * 60;
        PlayerDeathReason heldDamageSource;
        public override void ResetEffects()
        {
            rawHasBloodMold = false;
        }

        public override void PostUpdateEquips()
        {
            hasBloodMold = rawHasBloodMold;
            /*if (!hasBloodMold) {
                Player.ClearBuff(ModContent.BuffType<BloodMold>());
                Player.ClearBuff(ModContent.BuffType<BloodSprout>());
            }*/
        }

        Player.HurtModifiers m;

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            //base.ModifyHurt(ref modifiers);
            if (hasBloodMold) {
                m = modifiers;
                modifiers.SetMaxDamage(1);
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (hasBloodMold) {
                if (Player.HasBuff(ModContent.BuffType<BloodSprout>()))
                    Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<BloodSprout>())] -= InvHitPenalty;

                if (!Player.HasBuff(ModContent.BuffType<BloodMold>()))
                    Player.AddBuff(ModContent.BuffType<BloodSprout>(), TimeForInv);
                
                Player.AddBuff(ModContent.BuffType<BloodMold>(), TimeForHit);
                heldDamageSource = info.DamageSource;
                float dmg = (Player.endurance > 1 ? 0 : 1 - Player.endurance) * m.GetDamage(info.SourceDamage, Player.statDefense, Player.DefenseEffectiveness.Value);
                CombatText.NewText(new Rectangle((int)Player.Center.X - 5, (int)Player.Center.Y, 10, 10),
                Colors.RarityPurple, (int)dmg,
                dramatic: false, dot: false);
                damageLeft = Math.Max(damageLeft, 0) + dmg;
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (damageLeft <= 0.01) damageLeft = -1;

            int decayTime = Player.HasBuff(ModContent.BuffType<BloodMold>()) ? Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<BloodMold>())] : 0;

            //base case (tick)
            if (decayTime > 0 && damageLeft > 0) {
                int damageToDeal = (int)(60.0f * damageLeft / decayTime);
                Player.lifeRegen -= damageToDeal * 2;
                damageLeft -= damageToDeal / 60.0f;
                if (damageLeft <= 0.01) damageLeft = 0.0001f;
            }
            //case: buff removed early
            /*if (decayTime <= 0 && damageLeft > 0) {
                Player.lifeRegen -= (int)(damageLeft * 120);
                damageLeft = 0;
            }*/
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (Player.HasBuff(ModContent.BuffType<BloodSprout>())) {
                Player.statLife = 1;
                return false;
            }

            if (hasBloodMold && damageLeft >= 0 && heldDamageSource != null) damageSource = heldDamageSource;
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
        public override void UpdateDead()
        {
            //base.UpdateDead();
            damageLeft = -1;
        }
        public override void OnRespawn()
        {
            //base.OnRespawn();
            damageLeft = -1;
        }
    }
}