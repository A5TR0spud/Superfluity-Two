using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Mage
{
    public class ThaumaturgyBook : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 4, silver: 50);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpellTome)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            /*CreateRecipe()
                .AddIngredient(ModContent.ItemType<FlurryScroll>())
                .AddIngredient(ModContent.ItemType<ThunderScroll>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();*/
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            /*player.GetDamage(DamageClass.Magic) += 0.06f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.12f;
            player.GetModPlayer<MagePlayer>().hasFlurryScroll = true;*/

            player.GetModPlayer<ThaumaturgyPlayer>().hasThaumaturgy = true;
            player.GetModPlayer<ThaumaturgyPlayer>().hideThaumaturgy = hideVisual;
            player.GetModPlayer<ThaumaturgyPlayer>().thaumaturgeDefense += 3;
            player.GetModPlayer<ThaumaturgyPlayer>().thaumaturgyDamage += 10;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<ThaumaturgyPlayer>().forceShowThaumaturgy = true;
            player.GetModPlayer<ThaumaturgyPlayer>().showMagi = false;
        }
    }
}