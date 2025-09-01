using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Summon.KillerWisp
{
	public class KillerWispItem : GlowItem
	{
        public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
		}

        public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.mana = 16;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item44;
			Item.useAnimation = 26;
			Item.useTime = 26;
			Item.noMelee = true;
			//Item.crit = 0;
			Item.damage = 24;
			Item.DamageType = DamageClass.Summon;
			Item.knockBack = 7.5f;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.LightPurple;
			Item.useTurn = false;
			Item.buffType = ModContent.BuffType<KillerWispBuff>();
			Item.shoot = ModContent.ProjectileType<KillerWispMinion>();
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
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