using System;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class ATGPlayer : ModPlayer {
        public static readonly int damage = 30;
        public static readonly int knockback = 1;
        public int MissileCooldown = 290;
        public int MissilesPerLaunch = 0;
        public int MissilesBonus = 1;

        public override void ResetEffects()
        {
            MissilesPerLaunch = 0;
            MissilesBonus = 1;
            MissileCooldown = 290;
        }

        int timer = 0;
        public override void PostUpdateEquips()
        {
            if (MissilesPerLaunch <= 0)
                return;
            bool client = Player == Main.LocalPlayer;
            if (!client) return;
            if (timer < MissileCooldown)
            {
                timer++;
                return;
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !(npc.friendly || npc.immortal || npc.CountsAsACritter)
                && npc.Center.Distance(Player.Center) < 50 * 16)
                {
                    timer = 0;
                    int mCount = MissilesPerLaunch;
                    if (Player.IsStandingStillForSpecialEffects)
                        mCount += MissilesBonus;
                    LaunchMissiles(mCount);
                    break;
                }
            }
        }

        public void LaunchMissiles(int count, int damage = -1) {
            if (damage == -1) damage = ATGPlayer.damage;
            if (damage < 1) damage = 1;
            for (int j = 0; j < count; j++) {
                double ang = (j - (count / 2.0f) + 0.5f) * 0.2 - Math.PI * 0.5;
                Vector2 dir = 10f * new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang));
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(0, -10), dir, ModContent.ProjectileType<ATGMissile>(), damage, knockback, Player.whoAmI);
            }
        }
    }
}
