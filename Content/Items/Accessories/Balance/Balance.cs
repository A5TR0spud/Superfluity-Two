using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Balance
{
    [AutoloadEquip(EquipType.Face)]
	public class Balance : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = Item.sellPrice(gold: 4, silver: 60);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Whetstone>())
                .AddIngredient(ModContent.ItemType<Bandolier>())
                .AddIngredient(ModContent.ItemType<ShamanMask>())
                .AddIngredient(ModContent.ItemType<FocusCrystal>())
                .AddIngredient(ModContent.ItemType<SweetToothNecklace>())
                .AddIngredient(ModContent.ItemType<Stratagem>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //Whestone
            player.GetModPlayer<AtaraxiaPlayer>().stoned = true;
            //Bandolier
            player.GetModPlayer<AtaraxiaPlayer>().bandolier = true;
            //Shaman Mask
            player.GetModPlayer<AtaraxiaPlayer>().shaman = true;
            //Focus Crystal
            player.GetModPlayer<AtaraxiaPlayer>().focused = true;
            //Sweet Tooth Necklace
            player.GetModPlayer<AtaraxiaPlayer>().tooth = true;
            //player.GetArmorPenetration(DamageClass.Generic) += 5;
            //player.GetModPlayer<AtaraxiaPlayer>().honeyOnHitTime += 5 * 60;
            //Stratagem
            player.GetModPlayer<AtaraxiaPlayer>().strats = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 255);
        }
    }
}