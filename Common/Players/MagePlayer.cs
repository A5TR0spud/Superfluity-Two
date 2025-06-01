using System;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class MagePlayer : ModPlayer
    {
        public bool hasZygoma = false;
        public bool hasVileStone = false;
        public bool hasFlurryScroll = false;
        public bool hasThunderScroll = false;
        public bool hasVenom = false;
        internal int ShockwaveTimer = 0;
        internal const int SHOCKWAVE_COOLDOWN = (int)(2.5f * 60);
        public override void ResetEffects()
        {
            hasZygoma = false;
            hasVileStone = false;
            hasFlurryScroll = false;
            hasVenom = false;
            hasThunderScroll = false;
        }

        public override void PostUpdate()
        {
            if (ShockwaveTimer > 0) ShockwaveTimer--;
        }

        public override bool? CanAutoReuseItem(Item item)
        {
            if (hasFlurryScroll && item.DamageType.CountsAsClass(DamageClass.Magic)) return true;
            return null;
        }

        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if (!HelperMethodsSF2.IsProjectileVisuallyEnchantable(projectile)) return;
            if (projectile.DamageType.CountsAsClass(DamageClass.Magic) && projectile.friendly && !projectile.hostile && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
            {
                if (hasVileStone)
                {
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
                if (hasVenom)
                {
                    int num = Dust.NewDust(
                        boxPosition,
                        boxWidth,
                        boxHeight,
                        DustID.Venom,
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
        }

        //shouldn't happen but you never know with mods. not even gonna bother with the enchantment visual on the item though.
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasVileStone && hit.DamageType.CountsAsClass(DamageClass.Magic))
            {
                if (hasVileStone) HelperMethodsSF2.OnHitInflictWithVaryingDuration(target, BuffID.Poisoned);
                if (hasVenom) HelperMethodsSF2.OnHitInflictWithVaryingDuration(target, BuffID.Venom);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!proj.noEnchantments && hit.DamageType.CountsAsClass(DamageClass.Magic))
            {
                if (hasVileStone) HelperMethodsSF2.OnHitInflictWithVaryingDuration(target, BuffID.Poisoned);
                if (hasVenom) HelperMethodsSF2.OnHitInflictWithVaryingDuration(target, BuffID.Venom);
            }
        }
    }

    internal class ZygomaProjectile : GlobalProjectile
    {
        internal const float HOMING_POWER = 50f * (MathHelper.Pi / 180f) * (1f / 60f);
        internal const float HOMING_THRESHOLD = 45f * (MathHelper.Pi / 360f);
        //internal const float HOMING_ACCELERATION = 0.1f;
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
                || !projectile.friendly
                || projectile.hostile
                || !projectile.TryGetOwner(out Player player)
                || player.heldProj == projectile.identity
                || projectile.aiStyle == ProjAIStyleID.HeldProjectile
            )
                return;

            if (projectile.CountsAsClass(DamageClass.Magic)
            && player.GetModPlayer<MagePlayer>().hasZygoma)
            {
                if (!FindZygomaTarget(projectile, out int i, out float absAngleTo)) return;
                if (i == -1) return;
                NPC target = Main.npc[i];
                //Dust.NewDust(target.position, target.width, target.height, DustID.Clentaminator_Green);

                float length = projectile.velocity.Length();
                float targetAngle = projectile.AngleTo(target.Center);
                projectile.velocity = projectile.velocity.ToRotation().AngleTowards(targetAngle, HOMING_POWER / (projectile.extraUpdates + 1f)).ToRotationVector2() * length;
                //projectile.velocity += targetAngle.ToRotationVector2() * HOMING_ACCELERATION;
            }
        }

        public static bool FindZygomaTarget(Projectile projectile, out int NPCid, out float absAngleTo, float maxRange = 1200)
        {
            int heldNPCid = -1;
            float num = HOMING_THRESHOLD;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC nPC = Main.npc[i];
                if (!nPC.CanBeChasedBy(projectile) || projectile.localNPCImmunity[i] != 0)
                    continue;
                float distance = nPC.Distance(projectile.position);
                if (distance > maxRange)
                    continue;
                //if (distance > projectile.timeLeft * projectile.velocity.Length() / 100f)
                //    continue;

                float num2 = (nPC.Center - projectile.Center).ToRotation() - projectile.velocity.ToRotation();
                if (num2 > MathHelper.Pi) num2 -= MathHelper.TwoPi;
                if (num2 < -MathHelper.Pi) num2 += MathHelper.TwoPi;
                num2 = Math.Abs(num2); //projectile.Center.Distance(nPC.Center) - nPC.Center.Distance(projectile.Center + projectile.velocity);
                if (num2 < num && (!projectile.tileCollide || Collision.CanHit(projectile.position, projectile.width, projectile.height, nPC.position, nPC.width, nPC.height)))
                {
                    num = num2;
                    heldNPCid = i;
                }
            }
            NPCid = heldNPCid;
            absAngleTo = num;
            return heldNPCid != -1;
        }
    }

    internal class ThunderProjectile : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            if (ShouldTickShockwave(projectile, out MagePlayer modded))
            {
                if (Main.rand.NextBool()) modded.ShockwaveTimer--;
                CreateShockwave(projectile);
            }
        }

        /*public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        { // screw spiky balls
            if (ShouldTickShockwave(projectile, out _) && Main.rand.NextBool(7) && projectile.velocity.LengthSquared() > 16f)
            {
                CreateShockwave(projectile, true);
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }*/

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (timeLeft <= 0 && ShouldTickShockwave(projectile, out _) && Main.rand.NextBool(2))
            {
                CreateShockwave(projectile, true);
            }
        }

        internal static bool ShouldTickShockwave(Projectile projectile, out MagePlayer moddedPlayer)
        {
            if (projectile.friendly
                && !projectile.hostile
                && projectile.damage > 0
                && !projectile.noEnchantments
                && projectile.type != ModContent.ProjectileType<Shockwave>()
                && projectile.TryGetOwner(out Player player)
                && projectile.velocity.LengthSquared() > 0.2f
                && player.GetModPlayer<MagePlayer>().hasThunderScroll
                && player.heldProj != projectile.identity
                && Main.myPlayer == player.whoAmI
                && projectile.aiStyle != ProjAIStyleID.HeldProjectile
            )
            {
                moddedPlayer = player.GetModPlayer<MagePlayer>();
                return true;
            }
            moddedPlayer = null;
            return false;
        }

        internal static bool CreateShockwave(Projectile projectile, bool ignoreTimer = false)
        {
            if (!projectile.TryGetOwner(out Player player) || !player.GetModPlayer<MagePlayer>().hasThunderScroll) return false;
            if (Main.myPlayer != player.whoAmI) return false;
            MagePlayer modded = player.GetModPlayer<MagePlayer>();
            if (!ignoreTimer)
            {
                if (modded.ShockwaveTimer > 0) return false;
                modded.ShockwaveTimer = MagePlayer.SHOCKWAVE_COOLDOWN;
            }
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                projectile.Hitbox.Center(),
                projectile.velocity,
                ModContent.ProjectileType<Shockwave>(),
                Damage: Math.Max(projectile.damage / 2, 20),
                KnockBack: 15,
                player.whoAmI
            );
            return true;
        }
    }
}