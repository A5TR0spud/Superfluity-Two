using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;
using Terraria.DataStructures;
using System.Linq;
using System;

namespace SuperfluityTwo.Content.Items.Accessories.Melee
{
    public class MarkOfHonor : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
            Item.defense = 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WarriorEmblem)
                .AddIngredient(ItemID.CobaltShield)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            player.GetDamage(DamageClass.Melee) += 0.30f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
            player.GetModPlayer<MarkOfHonorPlayer>().hasHonor = true;
        }
    }


    public class MarkOfHonorPlayer : ModPlayer
    {
        public bool hasHonor = false;

        public override void ResetEffects()
        {
            hasHonor = false;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasHonor &&
                proj.aiStyle != ProjAIStyleID.Spear &&
                proj.aiStyle != ProjAIStyleID.ShortSword &&
                proj.aiStyle != ProjAIStyleID.HeldProjectile
            )
            {
                modifiers.FinalDamage *= Math.Max(1f - target.Center.Distance(Player.Center) / 1500f, 0);
            }
        }

        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (hasHonor && item.DamageType.CountsAsClass(DamageClass.Melee))
            {
                scale *= 1.1f;
            }
        }
    }

    public class MarkOfHonorProj : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.friendly &&
                projectile.TryGetOwner(out Player player) &&
                projectile.active &&
                !projectile.minion &&
                !Main.projPet[projectile.type] &&
                !Main.projHook[projectile.type] &&
                !ProjectileID.Sets.IsAWhip[projectile.type] &&
                projectile.aiStyle != ProjAIStyleID.Zenith &&
                projectile.aiStyle != ProjAIStyleID.Boomerang &&
                projectile.aiStyle != ProjAIStyleID.Yoyo &&
                projectile.aiStyle != ProjAIStyleID.Spear &&
                projectile.aiStyle != ProjAIStyleID.ShortSword &&
                projectile.aiStyle != ProjAIStyleID.Flail &&
                projectile.aiStyle != ProjAIStyleID.HeldProjectile &&
                projectile.aiStyle != ProjAIStyleID.Hook &&
                projectile.damage > 0 &&
                player.GetModPlayer<MarkOfHonorPlayer>().hasHonor
            )
            {
                projectile.velocity *= 0.5f;
            }
        }

        public override void PostAI(Projectile projectile)
        {
            if (projectile.friendly &&
                projectile.TryGetOwner(out Player player) &&
                projectile.active &&
                !projectile.minion &&
                !Main.projPet[projectile.type] &&
                !Main.projHook[projectile.type] &&
                !ProjectileID.Sets.IsAWhip[projectile.type] &&
                projectile.aiStyle != ProjAIStyleID.Zenith &&
                projectile.aiStyle != ProjAIStyleID.Spear &&
                projectile.aiStyle != ProjAIStyleID.ShortSword &&
                projectile.aiStyle != ProjAIStyleID.HeldProjectile &&
                projectile.aiStyle != ProjAIStyleID.Hook &&
                player.GetModPlayer<MarkOfHonorPlayer>().hasHonor &&
                projectile.Center.Distance(player.Center) > 1500
            )
            {
                projectile.Kill();
            }
        }
    }

    public class MarkOfHonorNPC : GlobalNPC
    {
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (target.GetModPlayer<MarkOfHonorPlayer>().hasHonor)
            {
                modifiers.FinalDamage *= 0.70f;
            }
        }
    }
}