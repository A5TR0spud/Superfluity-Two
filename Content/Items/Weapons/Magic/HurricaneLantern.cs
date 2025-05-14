using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Magic
{
	public class HurricaneLantern : ModItem
	{

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(0f, 0f);
        }

        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.SuperfluousSummoning.hjson file.
        public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.width = 22;
			Item.height = 40;
			Item.useTime = 3;
			Item.useAnimation = 23;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.knockBack = 15;
			Item.value = Item.sellPrice(gold: 2); //3g, 1s
			Item.rare = ItemRarityID.Green;
			//Item.UseSound = null;
			Item.noMelee = true;
			Item.useTurn = false;
			//Item.flame = false;
			Item.scale = 0.7f;
			Item.mana = 5;
			Item.shoot = ProjectileID.Ale;//ModContent.ProjectileType<WindGust>();
			Item.shootSpeed = 12f;
		}

       /* public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.RainCloud, 50)
				.AddIngredient(ItemID.SoulofFlight, 15)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}*/

		
       /* public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int rainID = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			Projectile rain = Main.projectile[rainID];
			rain.rotation = rain.velocity.ToRotation() - MathHelper.ToRadians(90);
            return false;
        }*/

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int numberProjectiles = 3; // 4 or 5 shots
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(30)); // 30 degree spread.
				// If you want to randomize the speed to stagger the projectiles
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale * 0.2f; 
				/*if (Main.rand.NextBool(2))*/ type = ProjectileID.RainFriendly;
				int rainID = Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
				Projectile rain = Main.projectile[rainID];
				//rain.tileCollide = false;
				rain.rotation = rain.velocity.ToRotation() + MathHelper.ToRadians(90);
				rain.maxPenetrate = -1;
				rain.penetrate = -1;
				rain.height = 2;
				rain.width = 2;
				rain.spriteDirection = -1;
			}
			return false; // return false because we don't want tmodloader to shoot projectile
		}
    }
}