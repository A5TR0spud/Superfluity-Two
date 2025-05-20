using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Common.Players;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria.Localization;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace SuperfluityTwo.Content.Items.Accessories.Mage
{
    [AutoloadEquip(EquipType.Face)]
	public class AncientSkull : GlowItem
	{
        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(gold: 6);
			Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Zygoma>())
                .AddIngredient(ModContent.ItemType<VileStone>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagePlayer>().hasZygoma = true;
            player.GetModPlayer<MagePlayer>().hasVenom = true;
        }
    }

    internal class AncientSkullLayer : PlayerDrawLayer
	{
		internal static Asset<Texture2D> texFace;
		internal static Asset<Texture2D> texFaceGlow;
        public override void Load()
        {
            AncientSkull acc = ModContent.GetInstance<AncientSkull>();
            texFace = ModContent.Request<Texture2D>(acc.Texture + "_Face", AssetRequestMode.AsyncLoad);
            texFaceGlow = ModContent.Request<Texture2D>(acc.Texture + "_Face_Glow", AssetRequestMode.AsyncLoad);
        }

		public override Position GetDefaultPosition() {
			return new BeforeParent(PlayerDrawLayers.FaceAcc);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            var acc = ModContent.GetInstance<AncientSkull>();
			return drawInfo.drawPlayer.face == EquipLoader.GetEquipSlot(Mod, acc.Name, EquipType.Face);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			if (drawInfo.drawPlayer.dead) {
				return;
			}
			DrawData? viewData = null;
			foreach (DrawData data in drawInfo.DrawDataCache) {
				if (data.texture == texFace.Value) {
					viewData = data;
				}
			}
			if (viewData.HasValue) {
				DrawData glow = new DrawData(
					texture: texFaceGlow.Value,
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