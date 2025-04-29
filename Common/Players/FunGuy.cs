using System;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Buffs;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace SuperfluityTwo.Common.Players
{
    public class FunGuy : ModPlayer {
        public bool hasAra = false;
        public bool rawHasAra = false;

        public override void ResetEffects()
        {
            rawHasAra = false;
        }

        public override void PostUpdateEquips()
        {
            hasAra = rawHasAra;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (hasAra)
                npc.AddBuff(BuffID.Confused, 3 * 60);
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (Player != Main.LocalPlayer || !hasAra)
                return;
            double dir = 0.0d;
            int projCount = 10;
            int layers = 3;
            float vel = 1.0f;
            for (int i = 0; i < projCount * layers; i++) {
                dir += Math.Tau / projCount;
                float j = (int)(i / projCount + 1);
                float ji = (float)Math.PI * j / projCount;
                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    Player.Center,
                    j * vel * new Vector2((float)Math.Cos(dir + ji), (float)Math.Sin(dir + ji)),
                    ModContent.ProjectileType<ShroomFume>(),
                    5, 0, Player.whoAmI);
            }
        }
    }
}
