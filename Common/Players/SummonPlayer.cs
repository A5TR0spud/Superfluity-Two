using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class SummonPlayer : ModPlayer {
        public bool hasShadowRite = false;
        public override void Load()
        {
            On_Player.ItemCheck_EmitUseVisuals += HookUseVisual;
        }

        public override void ResetEffects()
        {
            hasShadowRite = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasShadowRite && hit.DamageType.CountsAsClass(DamageClass.Summon)) {
                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.ShadowFlame, 360);
                }
                else if (Main.rand.NextBool(2))
                {
                    target.AddBuff(BuffID.ShadowFlame, 240);
                }
                else
                {
                    target.AddBuff(BuffID.ShadowFlame, 120);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!proj.noEnchantments && hasShadowRite && hit.DamageType.CountsAsClass(DamageClass.Summon)) {
                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.ShadowFlame, 360);
                }
                else if (Main.rand.NextBool(2))
                {
                    target.AddBuff(BuffID.ShadowFlame, 240);
                }
                else
                {
                    target.AddBuff(BuffID.ShadowFlame, 120);
                }
            }
        }

        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if (projectile.noEnchantmentVisuals || projectile.noEnchantments) return;
            if (hasShadowRite && projectile.DamageType.CountsAsClass(DamageClass.Summon) && projectile.friendly && !projectile.hostile && Main.rand.NextBool(2 * (1 + projectile.extraUpdates))) {
                int num = Dust.NewDust(
                    boxPosition,
                    boxWidth,
                    boxHeight,
                    DustID.Shadowflame,
                    projectile.velocity.X * 0.2f,// + (float)(projectile.direction * 3),
                    projectile.velocity.Y * 0.2f,
                    100,
                    default(Color),
                    2f
                );
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 0.7f;
                Main.dust[num].velocity.Y -= 0.5f;
            }
        }

        private Rectangle HookUseVisual(On_Player.orig_ItemCheck_EmitUseVisuals orig, Player self, Item sItem, Rectangle itemRectangle)
        {
            itemRectangle = orig(self, sItem, itemRectangle);
            if (sItem.DamageType.CountsAsClass(DamageClass.Summon) && !sItem.noMelee && !sItem.noUseGraphic && self.GetModPlayer<SummonPlayer>().hasShadowRite)
            {
                if (Main.rand.NextBool(3))
                {
                    int num = Dust.NewDust(
                        new Vector2(itemRectangle.X, itemRectangle.Y),
                        itemRectangle.Width,
                        itemRectangle.Height,
                        DustID.Shadowflame,
                        0,
                        0,
                        100,
                        default(Color),
                        2f
                    );
                    Main.dust[num].noGravity = true;
                    Main.dust[num].velocity *= 0.7f;
                    Main.dust[num].velocity.Y -= 0.5f;
                }
            }
            return itemRectangle;
        }
    }
}