using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class MagePlayer : ModPlayer {
        public bool hasZygoma = false;
        public bool hasVileStone = false;
        public bool hasFlurryScroll = false;
        public override void ResetEffects()
        {
            hasZygoma = false;
            hasVileStone = false;
            hasFlurryScroll = false;
        }

        public override bool? CanAutoReuseItem(Item item)
        {
            if (hasFlurryScroll && item.DamageType.CountsAsClass(DamageClass.Magic)) return true;
            return null;
        }

        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if (projectile.noEnchantmentVisuals || projectile.noEnchantments) return;
            if (hasVileStone && projectile.DamageType.CountsAsClass(DamageClass.Magic) && projectile.friendly && !projectile.hostile && Main.rand.NextBool(2 * (1 + projectile.extraUpdates))) {
                int num = Dust.NewDust(
                    boxPosition,
                    boxWidth,
                    boxHeight,
                    DustID.Poisoned,
                    projectile.velocity.X * 0.2f,// + (float)(projectile.direction * 3),
                    projectile.velocity.Y * 0.2f,
                    100,
                    default(Color),
                    1f
                );
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 0.7f;
                Main.dust[num].velocity.Y -= 0.5f;
            }
        }

        //shouldn't happen but you never know with mods. not even gonna bother with the enchantment visual on the item though.
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasVileStone && hit.DamageType.CountsAsClass(DamageClass.Magic)) {
                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.Poisoned, 360);
                }
                else if (Main.rand.NextBool(2))
                {
                    target.AddBuff(BuffID.Poisoned, 240);
                }
                else
                {
                    target.AddBuff(BuffID.Poisoned, 120);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!proj.noEnchantments && hasVileStone && hit.DamageType.CountsAsClass(DamageClass.Magic)) {
                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.Poisoned, 360);
                }
                else if (Main.rand.NextBool(2))
                {
                    target.AddBuff(BuffID.Poisoned, 240);
                }
                else
                {
                    target.AddBuff(BuffID.Poisoned, 120);
                }
            }
        }
    }

    public class ZygomaProjectile : GlobalProjectile {
        public override void AI(Projectile projectile)
        {
            if (!HelperMethodsSF2.ShouldProjectileUpdatePosition(projectile)
                || projectile.type == ProjectileID.LaserMachinegun
                || projectile.type == ProjectileID.IceBlock
                || projectile.type == ProjectileID.ChargedBlasterCannon
                || projectile.type == ProjectileID.ChargedBlasterLaser
                || projectile.type == ProjectileID.LastPrism
                || projectile.type == ProjectileID.LastPrismLaser
                || projectile.type == ProjectileID.RainFriendly
                || projectile.type == ProjectileID.MedusaHead
                || projectile.type == ProjectileID.MedusaHeadRay
                || projectile.noEnchantments
                || projectile.damage <= 0
                || projectile.hide
            )
                return;
            if (projectile.CountsAsClass(DamageClass.Magic)
            && projectile.TryGetOwner(out Player player)
            && player.GetModPlayer<MagePlayer>().hasZygoma) {
                int i = projectile.FindTargetWithLineOfSight();
                if (i == -1) return;
                NPC target = Main.npc[i];
                if (projectile.Center.DistanceSQ(target.Center) + 0.001f < target.Center.DistanceSQ(projectile.Center + projectile.oldVelocity))
                    return;
                Vector2 offset = (target.Center - projectile.Center).SafeNormalize(Vector2.Zero) / (projectile.extraUpdates + 1.0f);
                float targetSpeed = projectile.velocity.Length();
                Vector2 targetDir = (projectile.velocity + offset).SafeNormalize(offset);
                projectile.velocity = targetDir * targetSpeed;
            }
        }
    }
}