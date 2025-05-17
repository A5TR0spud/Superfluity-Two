using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common;
public class HelperMethodsSF2 {
    public static bool ShouldProjectileUpdatePosition(Projectile projectile) {
        if (projectile.aiStyle == 4
            || projectile.aiStyle == 38
            || projectile.aiStyle == 84
            || projectile.aiStyle == 148
            || (projectile.aiStyle == 7 && projectile.ai[0] == 2f)
            || ((projectile.type == ProjectileID.LaserMachinegunLaser || projectile.type == ProjectileID.SaucerLaser || projectile.type == ProjectileID.ScutlixLaser) && projectile.ai[1] == 1f)
            || (projectile.aiStyle == 93 && projectile.ai[0] < 0f)
            || projectile.type == ProjectileID.StardustTowerMark
            || projectile.type == ProjectileID.SharpTears
            || projectile.type == ProjectileID.WhiteTigerPounce
            || projectile.type == ProjectileID.SparkleGuitar
            || projectile.type == ProjectileID.DeerclopsIceSpike
            || projectile.type == ProjectileID.FinalFractal
            || ProjectileID.Sets.IsAGolfBall[projectile.type]
            || !ProjectileLoader.ShouldUpdatePosition(projectile)
            || (projectile.ModProjectile != null && !projectile.ModProjectile.ShouldUpdatePosition())
        )
        {
            return false;
        }
        return true;
    }
}