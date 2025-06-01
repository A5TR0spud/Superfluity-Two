using System;
using Terraria;
using Terraria.DataStructures;
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
}