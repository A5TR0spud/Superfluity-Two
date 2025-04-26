using System;
using SuperfluityTwo.Common.Players;
using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.NPCs
{
    public class CorpseTargetNPC : GlobalNPC {
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            //base.HitEffect(npc, hit);
            //if (hit.DamageType.)
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            OnHitByPlayer(player, damageDone);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.TryGetOwner(out Player plr))
            {
                OnHitByPlayer(plr, damageDone);
            }
        }

        public void OnHitByPlayer(Player player, int damageDone) {
            if (player.GetModPlayer<CorpseBloomPlayer>().hasCorpseBloom) {
                player.GetModPlayer<CorpseBloomPlayer>().ApplyCorpseBuff((int)(damageDone * CorpseBloomPlayer.InflictMultiplier));
            }
        }
    }
}