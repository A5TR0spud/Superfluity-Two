using Microsoft.Xna.Framework;
using SuperfluityTwo.Common;
using SuperfluityTwo.Common.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Ammo.Tracer
{
	public class TracerRound : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MusketBall);
			Item.damage = 2;
			Item.shoot = ModContent.ProjectileType<TracerBullet>();
		}
		public override void AddRecipes() {
			CreateRecipe(70)
				.AddIngredient(ItemID.MusketBall, 70)
				.AddIngredient(ItemID.Lens)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
}