using System;
using SuperfluityTwo.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.World;
public class HerbPlanter : GlobalTile {
    public override void RandomUpdate(int i, int j, int type)
    {
        bool isGrass = type == TileID.Grass;
        bool isHallowedGrass = type == TileID.HallowedGrass;
        bool isJungleGrass = type == TileID.JungleGrass;
        bool isLivingWood = type == TileID.LivingWood;
        bool isSunflower = type == TileID.Sunflower;
        bool isGnome = type == TileID.GardenGnome;
        bool shouldTryPlantCloverAbove =
            isGrass && Main.rand.NextBool(1500)
            || isHallowedGrass && Main.rand.NextBool(1000)
            || isJungleGrass && Main.rand.NextBool(1000)
        ;
        bool shouldTryPlantCloverNearby =
            isLivingWood && Main.rand.NextBool(30)
            || isGnome && Main.rand.NextBool(70)
            || isSunflower && Main.rand.NextBool(170)
        ;
        if (shouldTryPlantCloverAbove) {
            TryGrowingCloverAbove(i, j);
        }
        if (shouldTryPlantCloverNearby) {
            TryGrowingCloverNearby(i, j);
        }
    }
    
    private static bool TryGrowingCloverNearby(int i, int j)
    {
        int x = WorldGen.genRand.Next(Math.Max(10, i - 10), Math.Min(Main.maxTilesX - 10, i + 10));
        int y = WorldGen.genRand.Next(Math.Max(10, j - 10), Math.Min(Main.maxTilesY - 10, j + 10));
        if (hasValidGroundForClover(x, y) && NoNearbyClover(x, y))
        {
            WorldGen.PlaceTile(x, y, ModContent.TileType<CloverTile>(), mute: true);
            if (Main.tile[x, y].TileType == ModContent.TileType<CloverTile>()) {
                if (Main.netMode == NetmodeID.Server && Main.tile[x, y] != null && !Main.tile[x, y].IsActuated)
                {
                    NetMessage.SendTileSquare(-1, x, y);
                }
                return true;
            }
        }
        return false;
    }

    private static bool TryGrowingCloverAbove(int i, int j)
    {
        j--;
        if (hasValidGroundForClover(i, j) && NoNearbyClover(i, j))
        {
            WorldGen.PlaceTile(i, j, ModContent.TileType<CloverTile>(), mute: true);
            if (Main.tile[i, j].TileType == ModContent.TileType<CloverTile>()) {
                if (Main.netMode == NetmodeID.Server && Main.tile[i, j] != null && !Main.tile[i, j].IsActuated)
                {
                    NetMessage.SendTileSquare(-1, i, j);
                }
                return true;
            }
        }
        return false;
    }

    private static bool hasValidGroundForClover(int x, int y) {
        if (!WorldGen.InWorld(x, y, 2))
        {
            return false;
        }
        Tile tile = Main.tile[x, y + 1];
        if (tile == null || tile.IsActuated)
        {
            return false;
        }
        ushort type = tile.TileType;
        if (type < 0)
        {
            return false;
        }
        bool allowed = false;
        foreach (int tester in CloverTile.ValidAnchorTiles) {
            if (type == tester) {
                allowed = true;
                break;
            };
        }
        if (!allowed) return false;
        return WorldGen.SolidTileAllowBottomSlope(x, y + 1);
    }

    private static bool NoNearbyClover(int i, int j)
    {
        int num = Utils.Clamp(i - 120, 10, Main.maxTilesX - 1 - 10);
        int num2 = Utils.Clamp(i + 120, 10, Main.maxTilesX - 1 - 10);
        int num3 = Utils.Clamp(j - 120, 10, Main.maxTilesY - 1 - 10);
        int num4 = Utils.Clamp(j + 120, 10, Main.maxTilesY - 1 - 10);
        for (int k = num; k <= num2; k++)
        {
            for (int l = num3; l <= num4; l++)
            {
                Tile tile = Main.tile[k, l];
                if (!tile.IsActuated && tile.TileType == ModContent.TileType<CloverTile>())
                {
                    return false;
                }
            }
        }
        return true;
    }
}