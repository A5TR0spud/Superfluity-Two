using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SuperfluityTwo.Content.Items.Accessories.Balance
{
	public class FocusCrystal : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 80);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrystalShard, 10)
                .AddIngredient(ItemID.SoulofLight, 1)
                .AddTile(TileID.CrystalBall)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.manaCost *= 0.96f;
            player.GetCritChance(DamageClass.Magic) += 6;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 255);
        }
    }
}