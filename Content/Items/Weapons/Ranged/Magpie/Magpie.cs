using Microsoft.Xna.Framework;
using SuperfluityTwo.Common;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Magpie
{
	public class Magpie : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 46;
			Item.width = 26;
			Item.height = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 30f;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.reuseDelay = 12;
			Item.noMelee = true;
			Item.crit = 0;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 8f;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Pink;
			//Item.useTurn = false;
			Item.shoot = ModContent.ProjectileType<MagpieHoldout>();
			Item.noUseGraphic = true;
			Item.channel = true;
			ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PhoenixBlaster)
				.AddIngredient(ItemID.LightShard, 1)
				.AddIngredient(ItemID.DarkShard, 1)
				.AddIngredient(ItemID.SoulofSight, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			damage = player.HeldItem.damage;
			knockback = player.HeldItem.knockBack;
			velocity = velocity.SafeNormalize(-Vector2.UnitY) * player.HeldItem.shootSpeed;
			type = ModContent.ProjectileType<MagpieHoldout>();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[ModContent.ProjectileType<MagpieHoldout>()] <= 0;
		}
	}
}