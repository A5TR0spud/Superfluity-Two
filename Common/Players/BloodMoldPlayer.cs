using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using SuperfluityTwo.Content.Buffs;
using SuperfluityTwo.Content.Buffs.MaydayVariants;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace SuperfluityTwo.Common.Players
{
    public class BloodMoldPlayer : ModPlayer {
        public bool hasBloodMold = false;
        public bool rawHasBloodMold = false;
        public bool hasHeart = false;
        public bool rawHasHeart = false;
        public bool rawHasMayday = false;
        public bool HasMayday = false;
        public double damageLeft = -1;
        static readonly public int TimeForHit = 8 * 60;
        static readonly public int TimeForInv = 6 * 60;
        static readonly public int InvHitPenalty = 1 * 60;
        public PlayerDeathReason heldDamageSource;

        public override void Load()
        {
            IL_Player.Hurt_HurtInfo_bool += HookHurt;
        }
        public override void Unload()
        {
            IL_Player.Hurt_HurtInfo_bool -= HookHurt;
        }

        private void HookHurt(ILContext il)
        {
            try {
                //Hook 1: Changes damage combat text from orange/red to purple when under the effects of Blood Mold
                ILCursor c1 = new ILCursor(il);
                c1.GotoNext(i => i.MatchNewobj<Rectangle>());
                c1.GotoNext(i => i.MatchLdloc(7));
                c1.Index++;
                //^ Locating where color of combat text is loaded for combat text
                c1.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0); //push Player onto stack
                c1.EmitDelegate<Func<Color, Player, Color>>((returnValue, player) =>
                {
                    BloodMoldPlayer moldPlayer = player.GetModPlayer<BloodMoldPlayer>();
                    if (moldPlayer.hasBloodMold)
                        return /*moldPlayer.hasHeart ? Colors.RarityGreen : */Colors.RarityPurple;
                    return returnValue;
                });

                //Hook 2: Prevents player from taking direct damage when under the effects of Blood Mold
                ILCursor c2 = new ILCursor(il);
                c2.GotoNext(i => i.MatchLdfld<Terraria.Player>("statLife"));
                c2.GotoNext(i => i.MatchLdloc(4));
                c2.GotoNext(i => i.MatchConvI4());
                //^ Locating where life is decreased by num (damage). Num is casted to an int.
                c2.Index++; //Move cursor to after conversion to int
                c2.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0); //push Player onto stack
                c2.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_0); //push damageSource onto stack
                c2.EmitDelegate<Func<int, Player, PlayerDeathReason, int>>((returnValue, player, damageSource) =>
                {
                    BloodMoldPlayer moldPlayer = player.GetModPlayer<BloodMoldPlayer>();
                    if (moldPlayer.hasBloodMold) {
                        int buffToCheck = moldPlayer.HasMayday ? ModContent.BuffType<MaydayBloodSprout>() : ModContent.BuffType<BloodSprout>();
                        int debuffToCheck = moldPlayer.HasMayday ? ModContent.BuffType<MaydayBloodMold>() : ModContent.BuffType<BloodMold>();
                        if (player.HasBuff(buffToCheck))
                            player.buffTime[player.FindBuffIndex(buffToCheck)] -= InvHitPenalty;

                        if (!player.HasBuff(debuffToCheck))
                            player.AddBuff(buffToCheck, TimeForInv);
                        
                        player.AddBuff(debuffToCheck, TimeForHit);
                        moldPlayer.heldDamageSource = damageSource;
                        moldPlayer.damageLeft = Math.Max(moldPlayer.damageLeft, 0) + returnValue;
                        return 0;
                    }
                    return returnValue;
                });
            }
            catch (Exception)
            {
				// If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
				MonoModHooks.DumpIL(ModContent.GetInstance<SuperfluityTwo>(), il);

				// If the mod cannot run without the IL hook, throw an exception instead. The exception will call DumpIL internally
				// throw new ILPatchFailureException(ModContent.GetInstance<ExampleMod>(), il, e);
			}
        }

        public override void ResetEffects()
        {
            rawHasBloodMold = false;
            rawHasHeart = false;
            rawHasMayday = false;
        }

        public override void PostUpdateEquips()
        {
            hasBloodMold = rawHasBloodMold || rawHasHeart || rawHasMayday;
            hasHeart = rawHasHeart;
            HasMayday = rawHasMayday;
        }

        public override void UpdateBadLifeRegen()
        {
            if (damageLeft <= 0.01) damageLeft = -1;

            int buffToCheck = HasMayday ? ModContent.BuffType<MaydayBloodMold>() : ModContent.BuffType<BloodMold>();
            int decayTime = Player.HasBuff(buffToCheck) ? Player.buffTime[Player.FindBuffIndex(buffToCheck)] : 0;

            //base case (tick)
            if (decayTime > 0 && damageLeft > 0) {
                /*if (Player.lifeRegen > 0)
                    Player.lifeRegen /= 2;*/

                int damageToDeal = (int)(60.0f * damageLeft / decayTime);
                Player.lifeRegen -= damageToDeal * 2;
                damageLeft -= damageToDeal / 60.0f;
                //if (Player.lifeRegen > 0)
                //    Player.lifeRegen = 0;
                if (damageLeft <= 0.01) damageLeft = 0.0001f;
            }
            //case: buff removed early
            if (decayTime <= 0 && damageLeft > 0) {
                //Player.lifeRegen -= (int)(damageLeft * 120);
                damageLeft = 0;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            int buffToCheck = HasMayday ? ModContent.BuffType<MaydayBloodSprout>() : ModContent.BuffType<BloodSprout>();
            if (Player.HasBuff(buffToCheck)) {
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