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
			Item.width = 26;
			Item.height = 22;
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
            //Balance
            player.GetDamage(DamageClass.Generic) += 0.04f;
            //Whetstone
            player.GetArmorPenetration(DamageClass.Melee) += 5;
            //Bandolier
            player.GetModPlayer<BalancePlayer>().bandolierCount += 1;
            //Focus Crystal
            player.manaCost *= 0.96f;
            //Shaman Mask
            //player.GetDamage(DamageClass.Summon) += 0.04f;
            //Sweet Tooth Necklace
            player.GetArmorPenetration(DamageClass.Generic) += 5;
            //Stratagem
            player.GetModPlayer<BalancePlayer>().strataSlash = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 255);
        }
    }
}