using System;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class RangerPlayer : ModPlayer {
        public bool HasFloe = false;
        public bool HasAntlionLeg = false;
        public bool HasTrapperLash = false;
        public bool HasLeafTailStealth = false;
        public bool HasLeafTailDodge = false;
        private bool ShouldLeafTailTimerTick = false;
        private int LeafTailTimer = 0;
        public override void ResetEffects()
        {
            HasFloe = false;
            HasAntlionLeg = false;
            HasTrapperLash = false;
            HasLeafTailStealth = false;
            HasLeafTailDodge = false;
            ShouldLeafTailTimerTick = false;
        }

        public override void PostUpdateEquips()
        {
            if (HasAntlionLeg) Player.GetAttackSpeed(DamageClass.Ranged) += 0.12f;
            if (HasTrapperLash) Player.GetKnockback(DamageClass.Ranged) += 0.50f;
            ShouldLeafTailTimerTick = HasLeafTailDodge || HasLeafTailStealth;
            if (ShouldLeafTailTimerTick) {
                if (LeafTailTimer < 60 && Player.IsStandingStillForSpecialEffects) LeafTailTimer++;
                if (HasLeafTailStealth) Player.aggro -= (int)(LeafTailTimer * 1.66667f);
                if (LeafTailTimer > 0 && !Player.IsStandingStillForSpecialEffects) LeafTailTimer--;
            }
            else LeafTailTimer = 0;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            bool tailsafe = /*info.Dodgeable && */HasLeafTailDodge && LeafTailTimer > 0 && Main.rand.NextBool(1200 / LeafTailTimer);
            if (tailsafe) Player.SetImmuneTimeForAllTypes(Player.longInvince ? 90 : 60);
            return tailsafe;
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (HasLeafTailStealth) {
                float factor = 1f - (LeafTailTimer * 0.00833333333333f);
                a *= factor;
                r *= factor;
                g *= factor;
                b *= factor;
            }
        }

        public override bool? CanAutoReuseItem(Item item)
        {
            if (item.DamageType == DamageClass.Ranged && HasAntlionLeg) return true;
            return null;
        }

        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if (projectile.noEnchantmentVisuals || projectile.noEnchantments) return;
            if (HasFloe && projectile.DamageType == DamageClass.Ranged && projectile.friendly && !projectile.hostile && Main.rand.NextBool(2 * (1 + projectile.extraUpdates))) {
                int num = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.IceTorch, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 0.7f;
                Main.dust[num].velocity.Y -= 0.5f;
            }
        }

        //shouldn't happen but you never know with mods. not even gonna bother with the enchantment visual on the item though.
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (HasFloe && hit.DamageType == DamageClass.Ranged) {
                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.Frostburn, 360);
                }
                else if (Main.rand.NextBool(2))
                {
                    target.AddBuff(BuffID.Frostburn, 240);
                }
                else
                {
                    target.AddBuff(BuffID.Frostburn, 120);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!proj.noEnchantments && HasFloe && hit.DamageType == DamageClass.Ranged) {
                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.Frostburn, 360);
                }
                else if (Main.rand.NextBool(2))
                {
                    target.AddBuff(BuffID.Frostburn, 240);
                }
                else
                {
                    target.AddBuff(BuffID.Frostburn, 120);
                }
            }
        }
    }

    public class RangerProj : GlobalProjectile {

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            //if (projectile.noEnchantments) return;
            //if (projectile.WhipPointsForCollision.Count > 0) return;
            if (projectile.DamageType != DamageClass.Ranged) return;
            if (projectile.TryGetOwner(out Player player) && player.GetModPlayer<RangerPlayer>().HasTrapperLash) {
                if (projectile.velocity.Length() > 16 * 5 && projectile.extraUpdates < 1) projectile.extraUpdates = 1;
                else projectile.velocity *= 1.33f;
            }
        }
    }
}