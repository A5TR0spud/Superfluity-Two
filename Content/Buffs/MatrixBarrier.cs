using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SuperfluityTwo.Content.Buffs
{
    public class MatrixBarrierSys : ModSystem {
        public static Asset<Texture2D> texAsset;
        public override void Load()
        {
            texAsset = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Buffs/MatrixBarrier_Visual", AssetRequestMode.AsyncLoad);
        }
    }

    public class MatrixBarrier : ModBuff
    {
        public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

		public override void Update(Player player, ref int buffIndex) {
            player.endurance += 0.25f;
		}
    }

    internal class MatrixBarrierLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() {
			return PlayerDrawLayers.AfterLastVanillaLayer;
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
			return drawInfo.drawPlayer.HasBuff(ModContent.BuffType<MatrixBarrier>())
            && drawInfo.shadow == 0;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			if (drawInfo.drawPlayer.dead) {
				return;
			}
            Texture2D texture2d = MatrixBarrierSys.texAsset.Value;
            DrawData glow = new DrawData(
                texture: texture2d,
                position: new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2),
                sourceRect: new Rectangle(0, 0, texture2d.Width, texture2d.Height),
                color: Color.White,
                rotation: 0,
                origin: new Vector2(texture2d.Width / 2, texture2d.Height / 2),
                scale: 1f,
                effect: SpriteEffects.None
            );
            glow.shader = 0;
            drawInfo.DrawDataCache.Add(glow);
		}
    }
}