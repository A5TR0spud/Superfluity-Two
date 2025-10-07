using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories.Chromagem.Auribloom
{
    [AutoloadEquip(EquipType.Face)]
	public class AuribloomItem : ModItem
	{
        public override void SetStaticDefaults()
        {
            ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[EquipLoader.GetEquipSlot(Mod, this.Name, EquipType.Face)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.defense = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 12)
                .AddIngredient(ItemID.Amber, 5)
                .AddTile(TileID.Anvils)
                .AddDecraftCondition(Condition.CrimsonWorld)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 12)
                .AddIngredient(ItemID.Amber, 5)
                .AddTile(TileID.Anvils)
                .AddDecraftCondition(Condition.CorruptWorld)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen += 5;
            player.buffImmune[ModContent.BuffType<GoldBloodBuff>()] = true;
            if (!hideVisual && Main.rand.NextBool(4) && player.statLife < player.statLifeMax2)
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.GemAmber, Alpha: 0);
            }
        }
    }
}