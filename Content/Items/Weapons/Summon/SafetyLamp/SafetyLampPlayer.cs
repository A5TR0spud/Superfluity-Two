using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Summon.SafetyLamp;

public class SafetyLampPlayer : ModPlayer
{
    public int highestOriginalDamage = 0;
    public float highestKnockback = 0;

    public override void PostUpdateBuffs()
    {
        UpdateSafetyLampStatus();
    }
    
    private void UpdateSafetyLampStatus()
    {
        int lampID = ModContent.ProjectileType<SafetyLampMinion>();
        int counterID = ModContent.ProjectileType<SafetyLampCounter>();
        if (Player.ownedProjectileCounts[counterID] < 1)
        {
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.owner == Player.whoAmI && projectile.type == lampID)
                {
                    projectile.Kill();
                }
            }
            highestOriginalDamage = 0;
            highestKnockback = 0;
        }
        else if (Player.ownedProjectileCounts[lampID] < 1)
        {
            Projectile.NewProjectile(Player.GetSource_Misc("SF2_safetyLampSpawn"), Player.Center, Vector2.Zero, lampID, 0, 0f, Player.whoAmI);
        }
    }
}