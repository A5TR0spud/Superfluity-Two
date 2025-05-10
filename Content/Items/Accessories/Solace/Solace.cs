using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Solace
{
    [AutoloadEquip(EquipType.Face)]
	public class Solace : ModItem
	{
        public override void SetStaticDefaults()
        {
            ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[EquipLoader.GetEquipSlot(Mod, this.Name, EquipType.Face)] = true;
        }
        
        public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.defense = 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BastetBlessing>())
                .AddIngredient(ModContent.ItemType<LoverLocket>())
                .AddIngredient(ModContent.ItemType<LuckyClover>())
                .AddIngredient(ModContent.ItemType<MeteorMantle>())
                .AddIngredient(ModContent.ItemType<StarpowerBandolier>())
                .AddIngredient(ModContent.ItemType<SunflowerHairpin>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SolacePlayer>().hasLuckyClover = true;
            player.GetCritChance(DamageClass.Generic) += 4;
            player.GetModPlayer<SolacePlayer>().hasMeteorMantle = true;
            player.GetModPlayer<SolacePlayer>().hasBastetBlessing = true;
            player.GetModPlayer<SolacePlayer>().bastetVisible = !hideVisual;
            player.GetModPlayer<SolacePlayer>().hasStarCanteen = true;
			player.GetModPlayer<SolacePlayer>().hasSunflowerAggro = true;
			player.GetModPlayer<SolacePlayer>().hasSunflowerSpeed = true;
			player.GetModPlayer<SolacePlayer>().loverLocketSolaceOverride = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.StarInBottle] = true;
        }
    }
}