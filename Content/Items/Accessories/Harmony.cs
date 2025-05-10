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
			Item.width = 32;
			Item.height = 32;
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

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.StarInBottle] = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 255);
        }
    }
}