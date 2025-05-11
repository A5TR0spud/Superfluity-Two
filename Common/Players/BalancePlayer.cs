using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SuperfluityTwo.Common.Players
{
    public class BalancePlayer : ModPlayer {
        public int bandolierCount = 0;
        public bool strataSlash = false;
        public override void ResetEffects()
        {
            bandolierCount = 0;
            strataSlash = false;
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            bool consume = true;
            for (int i = 0; i < bandolierCount; i++) {
                consume = consume && !Main.rand.NextBool(10);
            }
            return consume;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (strataSlash && Main.rand.NextBool(25) && isSummonDamage(hit.DamageType)) {
                Vector2 targetV = target.Center;
                Vector2 v = Main.rand.NextVector2CircularEdge(200f, 200f);
                if (v.Y < 0f)
                {
                    v.Y *= -1f;
                }
                v.Y += 100f;
                Vector2 vector = v.SafeNormalize(Vector2.UnitY) * 6f;
                Projectile.NewProjectile(
                    spawnSource: Player.GetSource_OnHit(target),
                    position: targetV - vector * 20f,
                    velocity: vector,
                    Type: ModContent.ProjectileType<Strataslash>(),
                    Damage: damageDone,
                    KnockBack: 0f,
                    Owner: -1,
                    ai0: 0f,
                    ai1: targetV.Y);
                //Projectile.NewProjectile(Player.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<Strataslash>(), damageDone, 0, Player.whoAmI);
            }
        }

        private bool isSummonDamage(DamageClass toTest) {
            if (toTest == DamageClass.Summon) return true;
            if (toTest == DamageClass.SummonMeleeSpeed) return true;
            if (toTest == DamageClass.MagicSummonHybrid) return true;
            return false;
        }
    }
}