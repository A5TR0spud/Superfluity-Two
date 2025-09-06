using System;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Desperado
{
	public class Desperado : GlowItem
	{
		public override void SetDefaults()
		{
			Item.damage = 21;
			Item.width = 36;
			Item.height = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 16f;
			Item.UseSound = SoundID.Item41;
			Item.useAnimation = 21;
			Item.useTime = 21;
			Item.noMelee = true;
			Item.crit = 7;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(gold: 4);
			Item.rare = ItemRarityID.Green;
			Item.useTurn = false;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.scale = 0.8f;
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FlintlockPistol)
				.AddIngredient(ItemID.Revolver)
				.AddIngredient(ItemID.AntlionMandible, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = new Vector2(32, -	2 * player.direction).RotatedBy(velocity.ToRotation());
			bool flag = false;
			foreach (var npc in Main.ActiveNPCs)
			{
				if (player.CanHit(npc) && npc.Hitbox.IntersectsConeFastInaccurate(player.Center, 16 * 6, velocity.ToRotation(), (float)Math.PI * 0.06125f))
				{
					type = ModContent.ProjectileType<DesperadoExplosion>();
					flag = true;
					break;
				}
			}
			if (flag || Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(2, 4);
		}
    }
}