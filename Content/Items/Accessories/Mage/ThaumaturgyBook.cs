using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;
using System.Linq;

namespace SuperfluityTwo.Content.Items.Accessories.Mage
{
    public class ThaumaturgyBook : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 4, silver: 50);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FlurryScroll>())
                .AddIngredient(ModContent.ItemType<ThunderScroll>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.06f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.12f;
            player.GetModPlayer<MagePlayer>().hasFlurryScroll = true;

            player.GetModPlayer<ThaumaturgyPlayer>().hasThaumaturgy = true;
            player.GetModPlayer<ThaumaturgyPlayer>().hideThaumaturgy = hideVisual;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<ThaumaturgyPlayer>().forceShowThaumaturgy = true;
        }
    }
}