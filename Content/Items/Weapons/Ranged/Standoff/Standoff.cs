using System;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Items.Weapons.Ranged.Desperado;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Standoff
{
	public class Standoff : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 21;
			Item.width = 36;
			Item.height = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 16f;
			Item.UseSound = SoundID.Item41;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.noMelee = true;
			Item.crit = 7;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Blue;
			Item.useTurn = false;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.scale = 0.8f;
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Desperado.Desperado>())
				.AddIngredient(ItemID.TissueSample, 5)
				.AddIngredient(ItemID.Obsidian, 20)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Desperado.Desperado>())
				.AddIngredient(ItemID.ShadowScale, 5)
				.AddIngredient(ItemID.Obsidian, 20)
				.AddTile(TileID.Anvils)
				.Register();
		}
		
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int oldType = type;
			Vector2 muzzleOffset = new Vector2(32, -2 * player.direction).RotatedBy(velocity.ToRotation());
			foreach (var npc in Main.ActiveNPCs)
			{
				if (!npc.friendly && player.CanNPCBeHitByPlayerOrPlayerProjectile(npc) && player.CanHit(npc) && npc.Hitbox.IntersectsConeFastInaccurate(player.Center, 16 * 6, velocity.ToRotation(), (float)Math.PI * 0.06125f))
				{
					type = ModContent.ProjectileType<DesperadoExplosion>();
					position += muzzleOffset;
					break;
				}
			}
			Projectile.NewProjectile(
				source, position, velocity, type, damage, knockback, player.whoAmI, type == ModContent.ProjectileType<DesperadoExplosion>() ? oldType : 0
			);
            return false;
        }

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(2, 4);
		}
    }
}