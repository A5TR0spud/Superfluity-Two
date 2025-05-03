using System;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class RangerPlayer : ModPlayer {
        public bool rawHasFloe = false;
        public bool HasFloe = false;
        public override void ResetEffects()
        {
            rawHasFloe = false;
        }

        public override void PostUpdate()
        {
            HasFloe = rawHasFloe;
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
}