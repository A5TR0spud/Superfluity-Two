using System;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Items.Weapons.Summon.Asterism;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Summon.SafetyLamp
{
	public class SafetyLampItem : GlowItem
	{
        public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
		}

        public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 30;
			Item.mana = 12;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.UseSound = SoundID.Item4;
			Item.useAnimation = 26;
			Item.useTime = 26;
			Item.noMelee = true;
			Item.scale = 0.7f;
			Item.damage = 4;
			Item.DamageType = DamageClass.Summon;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.Green;
			Item.useTurn = false;
			Item.buffType = ModContent.BuffType<SafetyLampBuff>();
			Item.shoot = ModContent.ProjectileType<SafetyLampCounter>();
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.StarinaBottle)
				.AddIngredient(ItemID.Lens)
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			/*
			// Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;

			// Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
			*/
			player.GetModPlayer<SafetyLampPlayer>().highestOriginalDamage = Math.Max(Item.damage, player.GetModPlayer<SafetyLampPlayer>().highestOriginalDamage);
			player.GetModPlayer<SafetyLampPlayer>().highestKnockback = Math.Max(Item.knockBack, player.GetModPlayer<SafetyLampPlayer>().highestKnockback);
			return true;
		}

    }
}