using System;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using SuperfluityTwo.Content.Buffs;
using SuperfluityTwo.Content.Buffs.MaydayVariants;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class BloodMoldPlayer : ModPlayer {
        public bool hasBloodMold = false;
        public bool rawHasBloodMold = false;
        public bool hasHeart = false;
        public bool rawHasHeart = false;
        public bool rawHasMayday = false;
        public bool HasMayday = false;
        public bool forceBloodMoldVisible = false;
        public bool forceMaydayVisible = false;
        public double damageLeft = -1;
        static readonly public int TimeForHit = 8 * 60;
        static readonly public int TimeForInv = 6 * 60;
        static readonly public int InvHitPenalty = 1 * 60;

        public override void Load()
        {
            IL_Player.Hurt_HurtInfo_bool += HookHurt;
            IL_Player.UpdateLifeRegen += HookRegen;
        }

        public override void Unload()
        {
            IL_Player.Hurt_HurtInfo_bool -= HookHurt;
            IL_Player.UpdateLifeRegen -= HookRegen;
        }

        private void HookHurt(ILContext il)
        {
            try {
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

        private void HookRegen(ILContext il)
        {
            try {
                //Hook A: Change -480 life regen from red to purple
                ILCursor ca = new(il);
                // if (this.lifeRegenCount <= -480)
                ca.GotoNext(i => i.MatchLdarg(0));
                ca.GotoNext(i => i.MatchLdfld<Terraria.Player>("lifeRegenCount"));
                ca.GotoNext(i => i.MatchLdcI4(-480));
                //
                ca.GotoNext(i => i.MatchLdsfld<Terraria.CombatText>("LifeRegen"));
                ca.Index++;
                ca.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                ca.EmitDelegate<Func<Color, Player, Color>>((oldColor, player) =>
                {
                    BloodMoldPlayer moldPlayer = player.GetModPlayer<BloodMoldPlayer>();
                    if (moldPlayer.hasBloodMold) {
                        return Colors.RarityPurple;
                    }
                    return oldColor;
                });
                // else if (this.lifeRegenCount <= -360)
                ca.GotoNext(i => i.MatchLdarg(0));
                ca.GotoNext(i => i.MatchLdfld<Terraria.Player>("lifeRegenCount"));
                ca.GotoNext(i => i.MatchLdcI4(-360));
                //
                ca.GotoNext(i => i.MatchLdsfld<Terraria.CombatText>("LifeRegen"));
                ca.Index++;
                ca.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                ca.EmitDelegate<Func<Color, Player, Color>>((oldColor, player) =>
                {
                    BloodMoldPlayer moldPlayer = player.GetModPlayer<BloodMoldPlayer>();
                    if (moldPlayer.hasBloodMold) {
                        return Colors.RarityPurple;
                    }
                    return oldColor;
                });
                // else if (this.lifeRegenCount <= -240)
                ca.GotoNext(i => i.MatchLdarg(0));
                ca.GotoNext(i => i.MatchLdfld<Terraria.Player>("lifeRegenCount"));
                ca.GotoNext(i => i.MatchLdcI4(-240));
                //
                ca.GotoNext(i => i.MatchLdsfld<Terraria.CombatText>("LifeRegen"));
                ca.Index++;
                ca.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                ca.EmitDelegate<Func<Color, Player, Color>>((oldColor, player) =>
                {
                    BloodMoldPlayer moldPlayer = player.GetModPlayer<BloodMoldPlayer>();
                    if (moldPlayer.hasBloodMold) {
                        return Colors.RarityPurple;
                    }
                    return oldColor;
                });
                // else
                /*ca.GotoNext(i => i.MatchLdarg(0));
                ca.GotoNext(i => i.MatchLdfld<Terraria.Player>("lifeRegenCount"));
                ca.GotoNext(i => i.MatchLdcI4(-360));*/
                //
                ca.GotoNext(i => i.MatchLdsfld<Terraria.CombatText>("LifeRegen"));
                ca.Index++;
                ca.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                ca.EmitDelegate<Func<Color, Player, Color>>((oldColor, player) =>
                {
                    BloodMoldPlayer moldPlayer = player.GetModPlayer<BloodMoldPlayer>();
                    if (moldPlayer.hasBloodMold) {
                        return Colors.RarityPurple;
                    }
                    return oldColor;
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
            forceBloodMoldVisible = false;
            forceMaydayVisible = false;
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

            //int buffToCheck = HasMayday ? ModContent.BuffType<MaydayBloodMold>() : ModContent.BuffType<BloodMold>();
            int maydayTime = Player.HasBuff(ModContent.BuffType<MaydayBloodMold>()) ? Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<MaydayBloodMold>())] : 0;
            int bloodTime = Player.HasBuff(ModContent.BuffType<BloodMold>()) ? Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<BloodMold>())] : 0;
            int decayTime = Math.Max(maydayTime, bloodTime);

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
            if (Player.HasBuff(ModContent.BuffType<MaydayBloodSprout>()) || Player.HasBuff(ModContent.BuffType<BloodSprout>())) {
                Player.statLife = 1;
                return false;
            }

            if (hasBloodMold) {
                if ((HasMayday || forceMaydayVisible) && !forceBloodMoldVisible)
                    HelperMethodsSF2.MaydayDeathMessage(ref damageSource, Player);
                else {
                    HelperMethodsSF2.DecayDeathMessage(ref damageSource, Player);
                    if (genDust) {
                        genDust = false;
                        for (int i = 0; i < 9; i++) {
                            Dust.NewDust(
                                Player.position,
                                Player.width,
                                Player.height,
                                DustID.GreenBlood,
                                Player.velocity.X * 0.2f,
                                Player.velocity.Y * 0.2f,
                                100,
                                Color.White,
                                1.2f
                            );
                            Dust.NewDust(
                                Player.position,
                                Player.width,
                                Player.height,
                                DustID.Blood,
                                Player.velocity.X * 0.2f,
                                Player.velocity.Y * 0.2f,
                                100,
                                Color.White,
                                1.2f
                            );
                        }
                    }
                }
            }


            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
        
        public override void UpdateDead()
        {
            damageLeft = -1;
        }
        public override void OnRespawn()
        {
            damageLeft = -1;
        }
    }
}