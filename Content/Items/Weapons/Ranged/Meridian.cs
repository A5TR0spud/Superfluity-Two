using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged
{
	public class Meridian : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 52;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 20f;
			Item.UseSound = SoundID.Item38;
			Item.useAnimation = 12;
			Item.useTime = 4;
			Item.noMelee = true;
			Item.crit = 29;
			Item.damage = 190;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 3.5f;
			Item.value = Item.sellPrice(gold: 20);
			Item.rare = ItemRarityID.Red;
			Item.useTurn = false;
			Item.shoot = ModContent.ProjectileType<MeridianBullet>();
			ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;
			//Item.scale = 0.75f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SDMG)
				.AddIngredient(ItemID.Celeb2)
				.AddIngredient(ItemID.RocketLauncher)
				.AddIngredient(ItemID.Xenopopper)
				.AddIngredient(ItemID.StakeLauncher)
				.AddIngredient(ItemID.Tsunami)
				.AddIngredient(ItemID.VenusMagnum)
				.AddIngredient(ItemID.SuperStarCannon)
				.AddIngredient(ItemID.TheUndertaker)
				.AddIngredient(ItemID.EndlessMusketPouch)
				.AddIngredient(ItemID.EndlessQuiver)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.SDMG)
				.AddIngredient(ItemID.Celeb2)
				.AddIngredient(ItemID.RocketLauncher)
				.AddIngredient(ItemID.Xenopopper)
				.AddIngredient(ItemID.StakeLauncher)
				.AddIngredient(ItemID.Tsunami)
				.AddIngredient(ItemID.VenusMagnum)
				.AddIngredient(ItemID.SuperStarCannon)
				.AddIngredient(ItemID.Musket)
				.AddIngredient(ItemID.EndlessMusketPouch)
				.AddIngredient(ItemID.EndlessQuiver)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-14, -4);
		}

		int counter = 0;

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			return;
			switch (counter)
			{
				case 0:
					type = ProjectileID.MoonlordBullet;
					break;
				case 1:
					type = ProjectileID.CrystalBullet;
					break;
				case 2:
					type = ProjectileID.IchorBullet;
					break;
				case 3:
					type = ProjectileID.CursedBullet;
					break;
				case 4:
					type = ProjectileID.ChlorophyteBullet;
					break;
				case 5:
					type = ProjectileID.NanoBullet;
					break;
				case 6:
					type = ProjectileID.SilverBullet;
					break;
				case 7:
					type = ProjectileID.Stake;
					break;
				case 8:
					type = ProjectileID.SuperStar;
					break;
				case 9:
					type = ProjectileID.BoneArrow;
					break;
				case 10:
					type = ProjectileID.FireArrow;
					break;
				case 11:
					type = ProjectileID.FrostArrow;
					break;
				case 12:
					type = ProjectileID.VenomArrow;
					break;
				case 13:
					type = ProjectileID.JestersArrow;
					break;
				case 14:
					type = ProjectileID.ShimmerArrow;
					break;
				case 15:
					type = ProjectileID.UnholyArrow;
					break;
				case 16:
					type = ProjectileID.Celeb2Rocket;
					break;
				case 17:
					type = ProjectileID.Celeb2RocketLarge;
					break;
				case 18:
					type = ProjectileID.MiniNukeSnowmanRocketI;
					break;
				case 19:
					type = ProjectileID.VortexBeaterRocket;
					break;
			}

			counter = (counter + 1) % 20;

			position += new Vector2(24f, 0).RotatedByRandom(MathHelper.TwoPi) + velocity.SafeNormalize(Vector2.One).RotatedByRandom(MathHelper.PiOver4) * 32f;
		}

		int shootCount = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (shootCount >= 40 && player == Main.LocalPlayer)
			{
				for (int i = 0; i < 11; i++)
				{
					Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.PiOver4 * 0.33f), Main.rand.NextBool() ? ProjectileID.MoonlordBullet : ProjectileID.ChlorophyteBullet, damage, knockback, player.whoAmI);
				}
				shootCount -= 40;
			}
			shootCount++;
			return true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}