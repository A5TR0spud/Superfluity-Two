using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged
{
	public class HandheldMissileLauncher : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.Item38;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.noMelee = true;
			Item.crit = 4;
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Green;
			Item.useTurn = false;
			Item.shoot = ModContent.ProjectileType<ATGMissile>();
			//Item.scale = 0.75f;
		}

        /*public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 15)
				.AddTile(TileID.Anvils)
				.Register();
		}*/

        public override bool? CanChooseAmmo(Item ammo, Player player)
        {
            return ammo.type == ItemID.Grenade;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextBool(2) && base.CanConsumeAmmo(ammo, player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-14, -4);
        }
    }
}