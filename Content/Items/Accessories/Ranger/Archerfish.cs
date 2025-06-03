using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Ranger
{
	public class Archerfish : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 24;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AntlionLeg>())
                .AddIngredient(ModContent.ItemType<TrapperLash>())
                .AddIngredient(ModContent.ItemType<FloeCrystal>())
                .AddIngredient(ModContent.ItemType<LeafTail>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().HasFishProjVel = true;
            player.GetKnockback(DamageClass.Ranged) += 0.50f;
            player.GetModPlayer<RangerPlayer>().HasAntlionLeg = true;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.12f;
            player.GetModPlayer<RangerPlayer>().HasFloe = true;
            player.GetModPlayer<RangerPlayer>().HasLeafTailStealth = true;
        }
    }
}