using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories
{
	public class Harmony : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 30;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
            Item.defense = 2;
            Item.lifeRegen = 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Balance.Balance>())
                .AddIngredient(ModContent.ItemType<Solace.Solace>())
                .AddIngredient(ItemID.DarkShard)
                .AddIngredient(ItemID.LightShard)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            /*.AddIngredient(ItemID.SoulofFright) //547
            .AddIngredient(ItemID.SoulofMight) //548
            .AddIngredient(ItemID.SoulofSight) //549*/
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SolacePlayer>().hasLuckyClover = true;
            player.GetCritChance(DamageClass.Generic) += 4;
            player.GetModPlayer<SolacePlayer>().hasMeteorMantle = true;
            player.GetModPlayer<SolacePlayer>().hasBastetBlessing = true;
            player.GetModPlayer<SolacePlayer>().bastetVisible = !hideVisual;
            player.GetModPlayer<SolacePlayer>().hasStarCanteen = true;
			player.GetModPlayer<SolacePlayer>().hasSunflowerSpeed = true;
			player.GetModPlayer<SolacePlayer>().loverLocketSolaceOverride = true;
            //Balance
            player.GetDamage(DamageClass.Generic) += 0.06f;
            //Whetstone
            player.GetArmorPenetration(DamageClass.Melee) += 5;
            //Bandolier
            player.GetModPlayer<BalancePlayer>().bandolierCount += 1;
            //Focus Crystal
            player.manaCost *= 0.94f;
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