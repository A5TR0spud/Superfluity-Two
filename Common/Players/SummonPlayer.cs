using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class SummonPlayer : ModPlayer {
        public bool hasShadowRite = false;
        public bool hasCultivatingFlame = false;
        public override void Load()
        {
            On_Player.ItemCheck_EmitUseVisuals += HookUseVisual;
        }

        public override void ResetEffects()
        {
            hasShadowRite = false;
            hasCultivatingFlame = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.DamageType.CountsAsClass(DamageClass.Summon))
            {
                if (hasShadowRite) HelperMethodsSF2.OnHitInflictWithVaryingDuration(target, BuffID.ShadowFlame);
                if (hasCultivatingFlame && Main.myPlayer == Player.whoAmI && Main.rand.NextBool(20))
                {
                    Main.projectile[Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        target.Center,
                        Vector2.Zero,
                        ProjectileID.InfernoFriendlyBlast,
                        (int)(damageDone * 0.65f),
                        0,
                        Player.whoAmI
                    )].DamageType = DamageClass.Summon;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!proj.noEnchantments && hit.DamageType.CountsAsClass(DamageClass.Summon))
            {
                if (hasShadowRite) HelperMethodsSF2.OnHitInflictWithVaryingDuration(target, BuffID.ShadowFlame);
                if (hasCultivatingFlame && Main.myPlayer == Player.whoAmI && Main.rand.NextBool(20) && proj.type != ProjectileID.InfernoFriendlyBlast)
                {
                    Main.projectile[Projectile.NewProjectile(
                        proj.GetSource_FromThis(),
                        target.Center,
                        Vector2.Zero,
                        ProjectileID.InfernoFriendlyBlast,
                        (int)(damageDone * 0.65f),
                        0,
                        proj.owner
                    )].DamageType = DamageClass.Summon;
                }
            }
        }

        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if (!HelperMethodsSF2.IsProjectileVisuallyEnchantable(projectile)) return;
            if (hasShadowRite && projectile.DamageType.CountsAsClass(DamageClass.Summon) && projectile.friendly && !projectile.hostile && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
            {
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

    public class SummonPlayerNPC : GlobalNPC {
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(BuffID.ShadowFlame) && player.GetModPlayer<SummonPlayer>().hasShadowRite && item.DamageType.CountsAsClass(DamageClass.Summon))
                modifiers.ScalingBonusDamage += 0.10f;
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (!projectile.friendly || projectile.hostile) return;
            projectile.TryGetOwner(out Player owner);
            if (npc.HasBuff(BuffID.ShadowFlame) && owner.GetModPlayer<SummonPlayer>().hasShadowRite && projectile.CountsAsClass(DamageClass.Summon))
                modifiers.ScalingBonusDamage += 0.10f;
        }
    }
}