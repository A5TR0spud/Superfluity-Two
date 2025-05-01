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
			Item.width = 32;
			Item.height = 28;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.lifeRegen = 2;
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
            player.manaRegenDelayBonus += 0.5f;
            player.manaRegenBonus += 10;
            player.GetModPlayer<AtaraxiaPlayer>().gnomed = true;
            player.GetModPlayer<AtaraxiaPlayer>().mantle = true;
            player.GetModPlayer<AtaraxiaPlayer>().bastDefense = true;
            player.GetModPlayer<AtaraxiaPlayer>().sunflowerHairpinMoveSpeed = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.Campfire] = true;
            player.buffImmune[BuffID.HeartLamp] = true;
            player.buffImmune[BuffID.CatBast] = true;
            //player.buffImmune[BuffID.Sunflower] = true;
            player.buffImmune[BuffID.StarInBottle] = true;
        }
    }
}