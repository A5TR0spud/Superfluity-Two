using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Content.Items.Weapons.Ranged.Magpie;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common;

public class HelperMethodsSF2
{
    public static bool ShouldProjectileUpdatePosition(Projectile projectile)
    {
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

#nullable enable
    public static Player? TryGetOwner(Projectile projectile)
    {
        if (projectile.friendly)
        {
            projectile.TryGetOwner(out Player? player);
            return player;
        }
        return null;
    }
#nullable restore

    public static void DecayDeathMessage(ref PlayerDeathReason deathReason, Player player)
    {
        string translation = "Mods.SuperfluityTwo.DeathMessages.Decay.";
        string suffix;
        if (player.difficulty == 2) suffix = "Hardcore";
        else suffix = "Decay9";// + Main.ServerSideCharacter.ran;
        deathReason.CustomReason = NetworkText.FromKey(translation + suffix, player.name);
    }

    public static void MaydayDeathMessage(ref PlayerDeathReason deathReason, Player player)
    {
        string translation = "Mods.SuperfluityTwo.DeathMessages.Mayday.";
        string suffix;
        if (player.difficulty == 2) suffix = "Hardcore";
        else suffix = "Mayday14";// + Main;//Main.rand.Next(25);
        deathReason.CustomReason = NetworkText.FromKey(translation + suffix, player.name);
    }

    public static void OnHitInflictWithVaryingDuration(NPC victim, int buffID)
    {
        if (Main.rand.NextBool(4))
        {
            victim.AddBuff(buffID, 360);
        }
        else if (Main.rand.NextBool(2))
        {
            victim.AddBuff(buffID, 240);
        }
        else
        {
            victim.AddBuff(buffID, 120);
        }
    }

    public static void OnHitInflictWithVaryingDuration(Player victim, int buffID)
    {
        if (Main.rand.NextBool(7))
        {
            victim.AddBuff(buffID, 360);
        }
        else if (Main.rand.NextBool(3))
        {
            victim.AddBuff(buffID, 120);
        }
        else
        {
            victim.AddBuff(buffID, 60);
        }
    }

    public static bool CanItemBeShot(Item ammo, bool checkRocket = false)
    {
        if (ammo == null ||
            ammo.notAmmo ||
            (ammo.shoot <= ProjectileID.None && !checkRocket) ||
            ammo.stack <= 0)
        {
            return false;
        }
        bool flag = ammo.ammo != AmmoID.None;
        if (flag && checkRocket)
        {
            return GetRocketShoot(ammo) > ProjectileID.None;
        }
        return flag;
    }

    public static int GetRocketShoot(Item rocketAmmo)
    {
        //int1: weapon itemID
        //int2: ammo   itemID
        //int3: ammo   projectileID
        if (AmmoID.Sets.SpecificLauncherAmmoProjectileMatches.TryGetValue(ItemID.RocketLauncher, out var value))
        {
            if (value.TryGetValue(rocketAmmo.type, out int projID))
            {
                return projID;
            }
        }
        return ProjectileID.None;
    }

    public static bool IsArrow(int ammoID) =>
        ammoID == AmmoID.Arrow ||
        ammoID == AmmoID.Stake ||
        ammoID == AmmoID.StyngerBolt ||
        ammoID == AmmoID.Dart
    ;

    public static bool IsBullet(int ammoID) =>
        ammoID == AmmoID.Bullet ||
        ammoID == AmmoID.CandyCorn ||
        ammoID == AmmoID.Coin ||
        ammoID == AmmoID.NailFriendly
    ;

    public static bool IsRocket(int ammoID) =>
        ammoID == AmmoID.Rocket
    ;

    public static bool IsOther(int ammoID)
    {
        return !IsArrow(ammoID) && !IsBullet(ammoID) && !IsRocket(ammoID) && ammoID != AmmoID.None;
    }

    public static bool IsProjectileVisuallyEnchantable(Projectile proj)
    {
        bool flag1 = proj.noEnchantments || proj.noEnchantmentVisuals;
        bool flag2 = CanProjectileDealDamage(proj);
        return !flag1 && flag2;
    }

    public static bool CanProjectileDealDamage(Projectile proj)
    {
        bool flag1 = proj.friendly && proj.damage > 0;
        bool flag2 = proj.ModProjectile == null || (proj.ModProjectile.CanDamage() ?? true);
        bool flag3 = proj.active;
        return flag3 && flag1 && flag2;
    }

    /// <summary>
    /// Performs a raycast in the world to find collisions with tiles.
    /// Assumes a "width" of zero.
    /// Also moves coords into world bounds if they are out of bounds.
    /// </summary>
    /// <param name="startPos">The start position in world coordinates (16 units = 1 tile).</param>
    /// <param name="endPos">The end position in world coordinates (16 units = 1 tile).</param>
    /// <returns>True if there is no tile in the way; <br/>
    /// False if there is a tile in the way.</returns>
    public static bool Raycast(Vector2 startPos, Vector2 endPos, int stepSize = 8, bool debug = false)
    {
        if (CoordsOutOfBounds(startPos))
        {
            Vector2 dir = startPos.DirectionFrom(endPos);
            do
            {
                startPos += dir;
            } while (CoordsOutOfBounds(startPos));
        }
        if (CoordsOutOfBounds(endPos))
        {
            Vector2 dir = startPos.DirectionTo(endPos);
            do
            {
                endPos += dir;
            } while (CoordsOutOfBounds(endPos));
        }

        int totalDistance = (int)(startPos - endPos).Length();
        int distanceLeft = totalDistance;
        int nextToChop = -1;
        while (distanceLeft > 0)
        {
            int distanceToChop = Math.Min(nextToChop <= 0 ? stepSize : nextToChop, distanceLeft);
            nextToChop = -1;
            float distanceRatio = 1 - (float)distanceLeft / totalDistance;
            distanceLeft -= distanceToChop;
            float x = MathHelper.Lerp(startPos.X, endPos.X, distanceRatio);
            float y = MathHelper.Lerp(startPos.Y, endPos.Y, distanceRatio);
            Tile toCheck = Main.tile[(int)(x / 16), (int)(y / 16)];
            if (debug)
            {
                Dust.NewDustPerfect(
                    new Vector2(x, y),
                    DustID.GoldFlame,
                    Vector2.Zero
                ).noGravity = true;
            }
            if (toCheck == null)
            {
                continue;
            }
            if (!toCheck.HasUnactuatedTile)
            {
                continue;
            }
            if (!Main.tileSolid[toCheck.TileType] || Main.tileSolidTop[toCheck.TileType])
            {
                continue;
            }
            bool solidTop = toCheck.TopSlope;
            bool solidRight = toCheck.RightSlope;
            bool solidLeft = toCheck.LeftSlope;
            bool solidBottom = toCheck.BottomSlope;
            bool halfBlock = toCheck.IsHalfBlock;
            const int SLOPE_THRESHOLD = 1;
            if (halfBlock)
            {
                if (16 - y % 16 < 8)
                {
                    if (debug)
                    {
                        Dust.NewDustPerfect(
                            endPos,
                            DustID.Clentaminator_Red,
                            Vector2.Zero
                        ).noGravity = true;
                    }
                    return false;
                }
                nextToChop = 2;
            }
            else if (solidTop && solidRight)
            {
                if ((x % 16) + (16 - y % 16) <= 16 + SLOPE_THRESHOLD)
                {
                    if (debug)
                    {
                        Dust.NewDustPerfect(
                            endPos,
                            DustID.Clentaminator_Red,
                            Vector2.Zero
                        ).noGravity = true;
                    }
                    return false;
                }
                if (distanceToChop > 2)
                    distanceLeft += distanceToChop - 2;
                nextToChop = 2;
            }
            else if (solidTop && solidLeft)
            {
                if ((x % 16) - (16 - y % 16) >= 0 - SLOPE_THRESHOLD)
                {
                    if (debug)
                    {
                        Dust.NewDustPerfect(
                            endPos,
                            DustID.Clentaminator_Red,
                            Vector2.Zero
                        ).noGravity = true;
                    }
                    return false;
                }
                if (distanceToChop > 2)
                    distanceLeft += distanceToChop - 2;
                nextToChop = 2;
            }
            else if (solidBottom && solidLeft)
            {
                if ((x % 16) + (16 - y % 16) >= 16 - SLOPE_THRESHOLD)
                {
                    if (debug)
                    {
                        Dust.NewDustPerfect(
                            endPos,
                            DustID.Clentaminator_Red,
                            Vector2.Zero
                        ).noGravity = true;
                    }
                    return false;
                }
                if (distanceToChop > 2)
                    distanceLeft += distanceToChop - 2;
                nextToChop = 2;
            }
            else if (solidBottom && solidRight)
            {
                if ((x % 16) - (16 - y % 16) <= 0 + SLOPE_THRESHOLD)
                {
                    if (debug)
                    {
                        Dust.NewDustPerfect(
                            endPos,
                            DustID.Clentaminator_Red,
                            Vector2.Zero
                        ).noGravity = true;
                    }
                    return false;
                }
                if (distanceToChop > 2)
                    distanceLeft += distanceToChop - 2;
                nextToChop = 2;
            }
            else
            {
                if (debug)
                {
                    Dust.NewDustPerfect(
                        endPos,
                        DustID.Clentaminator_Red,
                        Vector2.Zero
                    ).noGravity = true;
                }
                return false;
            }
        }
        if (debug)
        {
            Dust.NewDustPerfect(
                endPos,
                DustID.Clentaminator_Green,
                Vector2.Zero
            ).noGravity = true;
        }
        return true;
    }

    /// <summary>
    /// More reliable than Raycast, but 12x as expensive.
    /// Performs a raycast in the world to find collisions with tiles.
    /// Assumes a "width" of four.
    /// Also moves coords into world bounds if they are out of bounds.
    /// </summary>
    /// <param name="startPos">The start position in world coordinates (16 units = 1 tile).</param>
    /// <param name="endPos">The end position in world coordinates (16 units = 1 tile).</param>
    /// <returns>True if there is no tile in the way; <br/>
    /// False if there is a tile in the way.</returns>
    public static bool RaycastReliable(Vector2 startPos, Vector2 endPos, bool debug = false)
    {
        Vector2 dir = startPos.DirectionTo(endPos);
        Vector2 lOff = 2 * dir.RotatedBy(MathHelper.PiOver2);
        Vector2 rOff = 2 * dir.RotatedBy(-MathHelper.PiOver2);
        bool flag1 = Raycast(startPos + lOff, endPos + lOff, 2, debug);
        bool flag2 = Raycast(startPos, endPos, 2, debug);
        bool flag3 = Raycast(startPos + rOff, endPos + rOff, 2, debug);
        bool toRet = flag1 && flag2 && flag3;
        if (debug)
        {
            Dust.NewDustPerfect(
                endPos,
                toRet ? DustID.Confetti_Green : DustID.Clentaminator_Red,
                Vector2.Zero
            ).noGravity = true;
        }
        return toRet;
    }
    public static bool CoordsOutOfBounds(Vector2 coords) => CoordsOutOfBounds(coords.X, coords.Y);
    public static bool CoordsOutOfBounds(float x, float y)
    {
        return x < Main.leftWorld || x > Main.rightWorld || y < Main.topWorld || y > Main.bottomWorld;
    }
}