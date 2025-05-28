using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged
{
	public class HandheldMissileLauncher : GlowItem
	{
		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.width = 38;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.Item38;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.noMelee = true;
			Item.crit = 4;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Orange;
			Item.useTurn = false;
			Item.shoot = ModContent.ProjectileType<LilRocket>();
			Item.useAmmo = AmmoID.Rocket;
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Boomstick)
				.AddIngredient(ItemID.HellstoneBar, 15)
				.AddIngredient(ItemID.IllegalGunParts)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

        public override bool? CanChooseAmmo(Item ammo, Player player)
		{
			return ammo.type == ItemID.Grenade;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			type = Item.shoot;
			const int horizOffset = 54 - 12 - 8; //54 is the width of the sprite, 12 to counteract the holdoutoffset, and 4 to account for the projectile visuals
			Vector2 newPos = position + new Vector2(horizOffset, -5 * player.direction).RotatedBy(velocity.ToRotation());
			if (Collision.CanHitLine(player.Center, 0, 0, newPos, 0, 0))
			{
				position = newPos;
			}
		}

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -5);
		}
    }
}