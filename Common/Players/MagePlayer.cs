using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players;
public class MagePlayer : ModPlayer {
    public bool hasZygoma = false;
    public override void ResetEffects()
    {
        hasZygoma = false;
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