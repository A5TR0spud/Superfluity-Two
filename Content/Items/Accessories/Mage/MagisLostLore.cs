using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;
using System.Linq;

namespace SuperfluityTwo.Content.Items.Accessories.Mage
{
    public class MagisLostLore : GlowItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 30;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ThaumaturgyBook>())
                .AddIngredient(ModContent.ItemType<AncientSkull>())
                .AddIngredient(ModContent.ItemType<FlurryScroll>())
                .AddIngredient(ModContent.ItemType<ThunderScroll>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetDamage(DamageClass.Magic) += 0.06f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.12f;
            player.GetModPlayer<MagePlayer>().hasFlurryScroll = true;

            player.GetModPlayer<ThaumaturgyPlayer>().hasThaumaturgy = true;
            player.GetModPlayer<ThaumaturgyPlayer>().hideThaumaturgy = hideVisual;
            player.GetModPlayer<ThaumaturgyPlayer>().thaumaturgeDefense += 4;
            player.GetModPlayer<ThaumaturgyPlayer>().thaumaturgyDamage += 20;

            player.GetModPlayer<MagePlayer>().hasZygoma = true;
            player.GetModPlayer<ThaumaturgyPlayer>().showMagi = true;
            player.GetModPlayer<ThaumaturgyPlayer>().hasMagi = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<ThaumaturgyPlayer>().forceShowThaumaturgy = true;
            player.GetModPlayer<ThaumaturgyPlayer>().showMagi = true;
        }
    }
}