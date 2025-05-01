using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;
using SuperfluityTwo.Content.Buffs;
using SuperfluityTwo.Content.Items.Accessories.Mayday;

namespace SuperfluityTwo.Content.Items.Accessories.Mayday
{
	public class Mayday : GlowItem
	{
        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 42;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RedAlert>())
                .AddIngredient(ModContent.ItemType<HeartOfDecay>())
                .AddIngredient(ModContent.ItemType<Medkit>())
                .AddIngredient(ModContent.ItemType<ATGMissileLauncher>())
                /*.AddIngredient(ItemID.SweetheartNecklace)
                .AddIngredient(ItemID.StarVeil)
                .AddIngredient(ItemID.FrozenTurtleShell)
                .AddIngredient(ItemID.Nanites, 25)*/
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FunGuy>().rawHasMayday = true;
            player.GetModPlayer<BloodMoldPlayer>().rawHasMayday = true;
            player.GetModPlayer<CorpseBloomPlayer>().rawHasMayday = true;

            //player.GetModPlayer<MedkitPlayer>().isMedicated = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Bleeding] = true;

            player.GetModPlayer<RedAlertPlayer>().hasRedAlert = true;
            player.GetModPlayer<RedAlertPlayer>().visibleMayday = !hideVisual;

            //player.GetModPlayer<MaydayPlayer>().HasMayday = true;

            player.GetModPlayer<ATGPlayer>().MissilesPerLaunch += 1;
            player.GetModPlayer<ATGPlayer>().MissileCooldown += 30;
            //player.longInvince = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<RedAlertPlayer>().forceVisibleMayday = true;
        }
    }
}