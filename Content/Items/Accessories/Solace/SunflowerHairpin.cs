using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Solace
{
    [AutoloadEquip(EquipType.Face)]
	public class SunflowerHairpin : ModItem
	{
        public override void SetStaticDefaults()
        {
            ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[EquipLoader.GetEquipSlot(Mod, this.Name, EquipType.Face)] = true;
        }

        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 30;
			Item.value = Item.sellPrice(silver: 12);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Sunflower)
                .AddIngredient(ItemID.Vine)
                .AddTile(TileID.Tables)
                .AddTile(TileID.Chairs)
                .Register();
        }

        /*public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.Sunflower] = true;
        }*/

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.GetModPlayer<SolacePlayer>().hasSunflowerAggro = true;
			player.GetModPlayer<SolacePlayer>().hasSunflowerSpeed = true;
            if (!hideVisual)
			{
                Lighting.AddLight(player.position + player.headPosition, new Vector3(0.35f, 0.3f, 0f));
			}
        }
    }
}