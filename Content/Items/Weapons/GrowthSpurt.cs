using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons
{
	public class GrowthSpurt : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.SuperfluousSummoning.hjson file.
        public override void SetDefaults()
		{
			Item.mana = 2;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 7f;
			Item.UseSound = SoundID.Item8;
			Item.useAnimation = 26;
			Item.useTime = 26;
			Item.noMelee = true;
			Item.crit = 0;
			Item.damage = 7;
			Item.DamageType = DamageClass.SummonMeleeSpeed;
			Item.width = 32;
			Item.height = 32;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(copper: 30);
			Item.rare = ItemRarityID.White;
			Item.useTurn = false;
			Item.shoot = ModContent.ProjectileType<Growth>();
			Item.scale = 0.75f;
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("Wood", 25)
				.AddIngredient(ItemID.Acorn, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}

        public override bool MagicPrefix()
        {
            return true;
        }
    }
}