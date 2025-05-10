using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Solace
{
    [AutoloadEquip(EquipType.Waist)]
	public class StarpowerBandolier : GlowItem
	{
        public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 15);
			Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StarinaBottle, 3)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile(TileID.Tables)
                .AddTile(TileID.Chairs)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.GetModPlayer<SolacePlayer>().hasStarCanteen = true;
        }
    }
    
    internal class StarpowerBandolierLayer : PlayerDrawLayer
	{
		private Asset<Texture2D> texWaist;
		private Asset<Texture2D> texWaistGlow;
        public override void Load()
        {
            StarpowerBandolier acc = ModContent.GetInstance<StarpowerBandolier>();
            texWaist = ModContent.Request<Texture2D>(acc.Texture + "_Waist", AssetRequestMode.AsyncLoad);
            texWaistGlow = ModContent.Request<Texture2D>(acc.Texture + "_Waist_Glow", AssetRequestMode.AsyncLoad);
        }

		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.WaistAcc);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            var acc = ModContent.GetInstance<StarpowerBandolier>();
			return drawInfo.drawPlayer.waist == EquipLoader.GetEquipSlot(Mod, acc.Name, EquipType.Waist);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			if (drawInfo.drawPlayer.dead) {
				return;
			}
            StarpowerBandolier acc = ModContent.GetInstance<StarpowerBandolier>();
			DrawData? viewData = null;
			foreach (DrawData data in drawInfo.DrawDataCache) {
				if (data.texture == texWaist.Value) {
					viewData = data;
				}
			}
			if (viewData.HasValue) {
				DrawData glow = new DrawData(
					texture: texWaistGlow.Value,
					color: Color.White * drawInfo.stealth * (1f - drawInfo.shadow),
					position: viewData.Value.position,
					sourceRect: viewData.Value.sourceRect,
					rotation: viewData.Value.rotation,
					origin: viewData.Value.origin,
					scale: viewData.Value.scale,
					effect: viewData.Value.effect,
					inactiveLayerDepth: 0
				);
				glow.shader = viewData.Value.shader;
				drawInfo.DrawDataCache.Add(glow);
			}
		}
    }
}