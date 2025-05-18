using SuperfluityTwo.Content.Items.Accessories.Mage;
using SuperfluityTwo.Content.Items.Accessories.Solace;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.World;
public class ChestGen : ModSystem {
    public override void PostWorldGen() {
        int maxClovers = 7;
        int cloversPlaced = 0;
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++) {
            Chest chest = Main.chest[chestIndex];
            if (chest == null) {
                continue;
            }
            Tile chestTile = Main.tile[chest.x, chest.y];
            //https://terraria.wiki.gg/wiki/Tile_IDs
            //12 is the sub id for living wood chest
            if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 12 * 36 || (WorldGen.genRand.NextBool(3) && (chestTile.WallType == WallID.LivingWood || chestTile.WallType == WallID.LivingWoodUnsafe))) {
                // living wood chest
                if (cloversPlaced >= maxClovers || !WorldGen.genRand.NextBool(9))
                    continue;
                for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++) {
                    if (chest.item[inventoryIndex].type == ItemID.None) {
                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<LuckyClover>());
                        cloversPlaced++;
                        break;
                    }
                }
            }
            //11 is frozen chest
            if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 11 * 36) {
                // frozen chest
                if (!WorldGen.genRand.NextBool(20))
                    continue;
                for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++) {
                    if (chest.item[inventoryIndex].type == ItemID.None) {
                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<FlurryScroll>());
                        break;
                    }
                }
            }
        }
    }
}