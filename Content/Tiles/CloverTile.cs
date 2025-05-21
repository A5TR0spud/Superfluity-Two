using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Content.Items.Accessories.Solace;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SuperfluityTwo.Content.Tiles
{
	public class CloverTile : ModTile
	{
		public static int[] ValidAnchorTiles = [
			TileID.Grass,
			TileID.HallowedGrass,
			TileID.JungleGrass
		];

		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;
			TileID.Sets.IgnoredInHouseScore[Type] = true;
			TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileID.Sets.BreakableWhenPlacing[Type] = true;
			TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
			TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]); // Make this tile interact with golf balls in the same way other plants do

			AddMapEntry(new Color(28, 216, 94), ModContent.GetInstance<LuckyClover>().DisplayName);

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorValidTiles = ValidAnchorTiles;
			TileObjectData.addTile(Type);

			HitSound = SoundID.Grass;
			DustType = DustID.JunglePlants;
		}

        public override void DropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance)
        {
            wormChance = 177;
            grassHopperChance = 77;
            jungleGrubChance = 177;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
			offsetY = -1;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 0) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}

		public override bool CanDrop(int i, int j) {
			return Main.rand.NextBool(7);
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j) {
			if (Main.rand.NextBool(7))
			{
				yield return new Item(ModContent.ItemType<LuckyClover>(), 1, PrefixID.Lucky);
			}
			else
			{
				yield return new Item(ModContent.ItemType<LuckyClover>());
			}
		}

		public override bool IsTileSpelunkable(int i, int j) {
			return true;
		}
	}
}