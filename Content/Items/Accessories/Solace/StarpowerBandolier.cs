using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperfluityTwo.Content.Items.Accessories.Solace
{
    [AutoloadEquip(EquipType.Neck)]
	public class StarpowerBandolier : GlowItem
	{
        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
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
            player.manaRegenDelayBonus += 0.5f;
            player.manaRegenBonus += 10;
        }

		public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.StarInBottle] = true;
        }
    }
    
    internal class StarpowerBandolierLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.NeckAcc);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            var acc = ModContent.GetInstance<StarpowerBandolier>();
			return drawInfo.drawPlayer.neck == EquipLoader.GetEquipSlot(Mod, acc.Name, EquipType.Neck);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			if (drawInfo.drawPlayer.dead) {
				return;
			}
            var acc = ModContent.GetInstance<StarpowerBandolier>();
			DrawData? viewData = null;
			foreach (DrawData data in drawInfo.DrawDataCache) {
				if (data.texture == ModContent.Request<Texture2D>(acc.Texture + "_Neck", AssetRequestMode.ImmediateLoad).Value) {
					viewData = data;
				}
			}
			if (viewData.HasValue) {
				DrawData glow = new DrawData(
					texture: ModContent.Request<Texture2D>(acc.Texture + "_Neck_Glow", AssetRequestMode.ImmediateLoad).Value,
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