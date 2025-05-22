using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Microsoft.VisualBasic;
using Humanizer;
using System;

namespace SuperfluityTwo.Common.Players
{
    public class ThaumaturgyPlayer : ModPlayer
    {
        public bool hasThaumaturgy = false;
        public bool hideThaumaturgy = false;
        public bool forceShowThaumaturgy = false;
        public int thaumaturgeDefense = 0;
        internal int ThaumaturgyCycleTimer = 0;
        internal int ThaumaturgyTimer = 0;
        internal int ThaumaturgyRotation = 0;
        internal bool shouldShowThaumaturgy = false;
        private List<int> projectilesFromLastTick = [];
        internal const int ANIMATION_TIME = 20;
        internal const int ON_TIME = 7 * 60 + 2 * ANIMATION_TIME;
        internal const int OFF_TIME = 7 * 60;
        internal const int CYCLE_TIME = ON_TIME + OFF_TIME;
        internal bool reduceAlpha = false;
        internal int outOfSyncTimer = 0;
        internal bool wasProtectionUp = false;
        internal float effectScalar = 0f;
        internal float alpha = 0f;
        internal float scale = 0;
        public override void ResetEffects()
        {
            hasThaumaturgy = false;
            forceShowThaumaturgy = false;
            hideThaumaturgy = false;
            thaumaturgeDefense = 0;
        }

        public override void UpdateEquips()
        {
            if (IsProtectionUp())
            {
                TickThaumaturgy();
                Player.statDefense += thaumaturgeDefense;
                if (Main.netMode == NetmodeID.MultiplayerClient && !wasProtectionUp && Player.whoAmI == Main.myPlayer)
                {
                    outOfSyncTimer++;
                    if (outOfSyncTimer > 5) //every 5 cycles, assume other clients are out of sync
                    {
                        for (int i = 0; i < Main.maxPlayers; i++)
                        {
                            //sync only to other players within 200 tiles
                            Player target = Main.player[i];
                            if (!target.active || target.Distance(Player.Center) > 200 * 16) continue;
                            SyncPlayer(i, Player.whoAmI, false);
                        }
                        outOfSyncTimer = 0;
                    }
                }
            }
            wasProtectionUp = IsProtectionUp();
        }

        public override void PostUpdateEquips()
        {
            if (hasThaumaturgy) ThaumaturgyCycleTimer++;
            else ThaumaturgyCycleTimer = ON_TIME + 1;
            int time = ThaumaturgyCycleTimer % CYCLE_TIME;

            reduceAlpha = hideThaumaturgy;

            if (time == 0) ThaumaturgyCycleTimer = 0;

            bool cycledOn = time < ON_TIME;
            shouldShowThaumaturgy = (hasThaumaturgy /*&& !hideThaumaturgy*/ && cycledOn) || forceShowThaumaturgy;
            if (shouldShowThaumaturgy)
            {
                ThaumaturgyRotation++;
                UpdateEffectScalar();
            }
        }

        private void UpdateEffectScalar()
        {
            int time = ThaumaturgyCycleTimer % CYCLE_TIME;
            if (time < ANIMATION_TIME)
            {
                effectScalar = (float)time / ANIMATION_TIME;
            }
            else if ((ON_TIME - time) < ANIMATION_TIME)
            {
                effectScalar = Math.Max((float)(ON_TIME - time) / ANIMATION_TIME, 0f);
            }
            else
            {
                effectScalar = 1f;
            }
            effectScalar = 1f - (1f - effectScalar) * (1f - effectScalar);
            UpdateAlpha();
            UpdateScale();
        }

        private void UpdateAlpha()
        {
            float num = effectScalar * (reduceAlpha ? 0.5f : 1f);
            if (forceShowThaumaturgy)
            {
                if (hasThaumaturgy)
                {
                    float ratio = reduceAlpha ? 0.3f : 0.5f;
                    num = ratio + ((1f - ratio) * num);
                }
                else
                {
                    num = 1f;
                }
            }
            alpha = num;
        }

        private void UpdateScale()
        {
            float num = 0.25f + 0.75f * effectScalar;
            if (forceShowThaumaturgy) num = 1f;
            scale = num;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (toWho == fromWho) return;
            ModPacket myPacket = ModContent.GetInstance<SuperfluityTwo>().GetPacket();
            myPacket.Write((byte)SF2NetworkID.SyncPlayerThaumaturgyCycle_S2C);
            myPacket.Write7BitEncodedInt(fromWho);
            myPacket.Write7BitEncodedInt(Main.player[fromWho].GetModPlayer<ThaumaturgyPlayer>().ThaumaturgyCycleTimer);
            myPacket.Send(toClient: toWho, ignoreClient: fromWho);
        }

        public bool IsProtectionUp()
        {
            int time = ThaumaturgyCycleTimer % CYCLE_TIME;
            bool cycledOn = time < ON_TIME;
            bool isPlayingAnimation = time < ANIMATION_TIME / 4 || (ON_TIME - time) < ANIMATION_TIME;
            return hasThaumaturgy && cycledOn && !isPlayingAnimation;
        }

        public override void OnRespawn()
        {
            ThaumaturgyCycleTimer = ANIMATION_TIME;
        }

        internal void TickThaumaturgy()
        {
            ThaumaturgyTimer++;
            float range = 90f;
            bool shouldTick = ThaumaturgyTimer % 20 == 0;
            if (shouldTick) ThaumaturgyTimer = 0;
            int damage = 32;
            float kb = 9.5f; //only applied against npcs, not pvp players

            if (Player.whoAmI == Main.myPlayer)
            {
                for (int m = 0; m < Main.maxProjectiles; m++)
                {
                    Projectile proj = Main.projectile[m];
                    if (!proj.active || Vector2.Distance(Player.Center, proj.Center) > range)
                    {
                        projectilesFromLastTick.Remove(m);
                        continue;
                    }
                    if (projectilesFromLastTick.Contains(m))
                        continue;
                    projectilesFromLastTick.Add(m);
                    if (proj.hostile && !proj.friendly && proj.damage > 0 && proj.IsDamageDodgable())
                    {
                        if (Main.rand.NextBool(5))
                        {
                            float length = proj.velocity.Length();
                            proj.velocity = (Main.rand.NextBool() ? -proj.DirectionTo(Player.Center) * length : -proj.velocity) * 1.15f;
                            proj.friendly = true;
                            proj.hostile = false;
                            proj.timeLeft /= 2;
                            proj.alpha /= 2;
                            for (int i = 0; i < 5; i++)
                            {
                                Dust.NewDust(proj.position, proj.width, proj.height, DustID.GoldFlame);
                            }
                        }
                        else
                        {
                            proj.velocity *= 0.5f;
                        }
                    }
                    if (Player.hostile && proj.active && proj.friendly && proj.TryGetOwner(out Player pvpPlayer) && proj.damage > 0)
                    {
                        if (pvpPlayer == Player || !pvpPlayer.active || pvpPlayer.dead || !pvpPlayer.hostile || (pvpPlayer.team == Player.team && pvpPlayer.team != 0) || !(Vector2.Distance(Player.Center, pvpPlayer.Center) <= range))
                            continue;
                        proj.velocity *= 0.75f;
                    }
                }
            }


            if (Player.whoAmI == Main.myPlayer && shouldTick)
            {
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC nPC = Main.npc[k];
                    if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Player.CanNPCBeHitByPlayerOrPlayerProjectile(nPC) && Vector2.Distance(Player.Center, nPC.Center) <= range)
                    {
                        int dir = nPC.Center.X < Player.Center.X ? -1 : 1;
                        Player.ApplyDamageToNPC(nPC, damage, knockback: kb, direction: dir);
                    }
                }
                if (Player.hostile)
                {
                    for (int l = 0; l < Main.maxPlayers; l++)
                    {
                        Player pvpPlayer = Main.player[l];
                        if (pvpPlayer == Player || !pvpPlayer.active || pvpPlayer.dead || !pvpPlayer.hostile || (pvpPlayer.team == Player.team && pvpPlayer.team != 0) || !(Vector2.Distance(Player.Center, pvpPlayer.Center) <= range))
                        {
                            continue;
                        }
                        PlayerDeathReason reason = PlayerDeathReason.ByOther(16, Player.whoAmI);
                        pvpPlayer.Hurt(reason, damage, pvp: true, dodgeable: false, knockback: 0, hitDirection: 0);
                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            Player.HurtInfo hurtInfo = new()
                            {
                                Knockback = 0,
                                SourceDamage = damage,
                                DamageSource = reason,
                                PvP = true,
                                Dodgeable = false,
                                HitDirection = 0
                            };
                            NetMessage.SendPlayerHurt(l, hurtInfo);
                        }
                    }
                }
            }
        }
    }

    internal class ThaumaturgyDrawLayer : PlayerDrawLayer
    {
        internal static Asset<Texture2D> texAsset;

        public override void Load()
        {
            texAsset = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Items/Accessories/Mage/ThaumaturgyRite", AssetRequestMode.AsyncLoad);
        }

        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.BeforeFirstVanillaLayer.Layer2);
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0) return false;
            return drawInfo.drawPlayer.GetModPlayer<ThaumaturgyPlayer>().shouldShowThaumaturgy;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            if (player.dead)
            {
                return;
            }
            Texture2D texture2d = texAsset.Value;
            ThaumaturgyPlayer modded = player.GetModPlayer<ThaumaturgyPlayer>();

            //Vector2 pos = drawInfo.Center.RotatedBy(-drawInfo.rotation, drawInfo.Position + drawInfo.rotationOrigin) - Main.screenPosition;

            DrawData glow = new(
                texture: texture2d,
                position: new Vector2((int)(drawInfo.drawPlayer.Center.X - Main.screenPosition.X), (int)(drawInfo.drawPlayer.Center.Y + drawInfo.drawPlayer.gfxOffY - Main.screenPosition.Y)),
                sourceRect: new Rectangle(0, 0, texture2d.Width, texture2d.Height),
                color: new Color(0.8f, 0.8f, 0.8f, 0.6f) * modded.alpha,
                rotation: modded.ThaumaturgyRotation * 0.002f * drawInfo.drawPlayer.gravDir - drawInfo.rotation,
                origin: new Vector2(texture2d.Width / 2, texture2d.Height / 2),
                scale: modded.scale,
                effect: SpriteEffects.None
            );
            drawInfo.DrawDataCache.Add(glow);
        }
    }
}