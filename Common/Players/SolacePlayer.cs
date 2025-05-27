using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using SuperfluityTwo.Content.Items.Accessories.Solace;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Net;


namespace SuperfluityTwo.Common.Players
{
    public class SolacePlayer : ModPlayer {
        public bool hasBastetBlessing = false;
        public bool hasLuckyClover = false;
        public bool bastetVisible = false;
        public bool bastetAnkhVisible = false;
        public bool forceBastetVisible = false;
        public bool hasLoverLocket = false;
        public bool loverLocketSolaceOverride = false;
        public int heartPickupCooldown = 0;
        public bool hasMeteorMantle = false;
        public bool hasMeteorDOTReduce = false;
        public bool hasStarCanteen = false;
        public bool hasSunflowerSpeed = false;
        public bool hasSunflowerAggro = false;
        public bool flagNeedVFX = false;
        public override void ResetEffects()
        {
            hasBastetBlessing = false;
            hasLuckyClover = false;
            bastetVisible = false;
            forceBastetVisible = false;
            bastetAnkhVisible = false;
            hasLoverLocket = false;
            hasMeteorMantle = false;
            hasMeteorDOTReduce = false;
            hasStarCanteen = false;
            hasSunflowerSpeed = false;
            hasSunflowerAggro = false;
            loverLocketSolaceOverride = false;
        }
        public override void Load()
        {
            On_Player.Hurt_HurtInfo_bool += HookHurt;
            IL_Player.UpdateLifeRegen += HookRegen;
        }

        public override void PostUpdateEquips()
        {
            if (hasStarCanteen) {
                Player.manaCost *= 0.96f;
                if (!Player.IsStandingStillForSpecialEffects && Player.grappling[0] < 0) {
                    Player.manaRegenBonus += Player.statManaMax2 / 6;
                }
            }
            if (hasSunflowerAggro) {
                Player.aggro -= 50;
            }
        }

        public override void Unload()
        {
            IL_Player.UpdateLifeRegen -= HookRegen;
        }

        private void HookRegen(ILContext il)
        {
            try {
                //Hook 1: reduce regen penalty for moving if affected by meteor mantle
                ILCursor c1 = new ILCursor(il);
                c1.GotoNext(i => i.MatchLdflda<Entity>("velocity"));
                c1.GotoNext(i => i.MatchLdfld<Player>("grappling"));
                c1.GotoNext(i => i.MatchLdcI4(0));
                c1.GotoNext(i => i.MatchLdloc(1));
                c1.GotoNext(i => i.MatchLdcR4(0.5f));
                c1.Index++;
                c1.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0); //push Player onto stack
                c1.EmitDelegate<Func<float, Player, float>>((returnValue, player) =>
                {
                    SolacePlayer modded = player.GetModPlayer<SolacePlayer>();
                    if (modded.hasMeteorMantle)
                        return Math.Min(returnValue + 0.5f, 1.25f);
                    return returnValue;
                });

                //Hook 2: reduce damage of DoT's
                ILCursor c2 = new ILCursor(il);
                c2.GotoNext(i => i.MatchLdfld<Player>("honey"));
                c2.GotoNext(i => i.MatchLdfld<Player>("nebulaLevelLife"));
                c2.Index -=4 ;
                c2.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0); //push Player onto stack
                c2.EmitDelegate<Action<Player>>((player) =>
                {
                    SolacePlayer modded = player.GetModPlayer<SolacePlayer>();
                    if (modded.hasMeteorDOTReduce && player.lifeRegen < 0)
                        player.lifeRegen = (int)(player.lifeRegen * 0.9f);
                    return;
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
        
        private void HookHurt(On_Player.orig_Hurt_HurtInfo_bool orig, Player self, Player.HurtInfo info, bool quiet)
        {
            if (self.GetModPlayer<SolacePlayer>().hasBastetBlessing && info.SourceDamage >= self.statLife * 0.66) {
                int initDmg = info.Damage;
                info.Damage = (int)(info.Damage * 0.7f);
                SolacePlayer modded = self.GetModPlayer<SolacePlayer>();
                if (modded.forceBastetVisible || modded.bastetVisible)
                {
                    for (int i = 0; i < 10; i++)
                        Dust.NewDust(self.position, self.width, self.height, DustID.GoldFlame, 0, -4);
                    if (self.statLife - initDmg <= 0 && self.statLife - info.Damage > 0)
                    {
                        modded.flagNeedVFX = true;
                    }
                }
            }
            orig(self, info, quiet);
        }

        public override void ModifyLuck(ref float luck)
        {
            if (hasLuckyClover)
                luck += 0.05f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            bool shouldWork = hasLoverLocket || loverLocketSolaceOverride;
            bool dropManaInsteadOfHealth =
                (float)Player.statMana / Player.statManaMax2 < (float)Player.statLife / Player.statLifeMax2
                && loverLocketSolaceOverride
                && Main.rand.NextBool(2);
            //if (hit.Crit) heartPickupCooldown -= (int)(10 - (10 / (0.1f * damageDone + 1)));
            if (shouldWork && (Main.rand.NextBool(20) || hit.Crit) && heartPickupCooldown <= 0) {
                heartPickupCooldown = 7 * 60;
                int index = Item.NewItem(target.GetSource_Loot(), (int)target.position.X, (int)target.position.Y, target.width, target.height, dropManaInsteadOfHealth ? ItemID.Star : ItemID.Heart);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, index, 1f);
            }
        }

        public override void PostUpdate()
        {
            if (heartPickupCooldown > 0) heartPickupCooldown--;

            if (flagNeedVFX)
            {
                flagNeedVFX = false;
                if (Main.myPlayer == Player.whoAmI)
                {
                    Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center - new Vector2(0, Player.height / 2),
                        new Vector2(Main.rand.NextFloat() - 0.5f, -3f),
                        ModContent.ProjectileType<BastetVFX>(),
                        0,
                        0,
                        Player.whoAmI
                    );
                }
            }
        }

        public override void PostUpdateRunSpeeds()
        {
            if (hasSunflowerSpeed) {
                Player.runAcceleration *= 1.2f;
                Player.accRunSpeed *= 1.1f;
                Player.maxRunSpeed *= 1.1f;
            }
        }
    }
}