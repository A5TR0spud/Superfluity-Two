using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Summon.Fumigator
{
	public class FumigatorItem : GlowItem
	{
        public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
		}

        public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 14;
			Item.mana = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.Item44;
			Item.useAnimation = 26;
			Item.useTime = 26;
			Item.noMelee = true;
			//Item.crit = 0;
			Item.damage = 7;
			Item.DamageType = DamageClass.Summon;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Blue;
			Item.useTurn = false;
			Item.buffType = ModContent.BuffType<FumigatorBuff>();
			Item.shoot = ModContent.ProjectileType<FumigatorMinion>();
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.IllegalGunParts)
				.AddRecipeGroup(RecipeGroupID.IronBar, 10)
				.AddIngredient(ItemID.GlowingMushroom, 20)
				.AddTile(TileID.Anvils)
				.Register();
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
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
			return true;
		}

    }
}