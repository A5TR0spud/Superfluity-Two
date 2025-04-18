using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Audio;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Buffs
{
    public class Alert : ModBuff
    {
        public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            RedAlertPlayer modded = player.GetModPlayer<RedAlertPlayer>();
            if (!modded.RedAlertEquipped) {
                player.DelBuff(buffIndex);
                return;
            }
            if (player.buffTime[buffIndex] == 1
                && player.GetModPlayer<RedAlertPlayer>().visibleRedAlert)
            {
                SoundStyle alarmSound = new SoundStyle($"{nameof(SuperfluityTwo)}/Assets/Sounds/RedAlertOffCooldown")
                    {
                        Volume = 1.45f,
                        PitchVariance = 0.005f
                    };
                    SoundEngine.PlaySound(alarmSound);
            }
        }
    }

    internal class AlarmLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.IceBarrier);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            if (drawInfo.shadow != 0)
                return false;
            if (drawInfo.drawPlayer.GetModPlayer<RedAlertPlayer>().forceVisibleRedAlert)
                return true;
            if (drawInfo.drawPlayer.HasBuff(ModContent.BuffType<Alert>()) && drawInfo.drawPlayer.GetModPlayer<RedAlertPlayer>().visibleRedAlert)
                return true;
            return false;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			if (drawInfo.drawPlayer.dead) {
				return;
			}
            Texture2D texture2d = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Buffs/Alert_Alarm", AssetRequestMode.ImmediateLoad).Value;
            DrawData glow = new DrawData(
                texture: texture2d,
                position: new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2), (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2)
                + new Vector2(0, -16 * 2),
                sourceRect: new Rectangle(0, 0, texture2d.Width, texture2d.Height),
                color: Color.White,
                rotation: drawInfo.drawPlayer.GetModPlayer<RedAlertPlayer>().alertRotation,
                origin: new Vector2(texture2d.Width / 2, texture2d.Height / 2),
                scale: 1f,
                effect: SpriteEffects.None
            );
            glow.shader = 1;
            drawInfo.DrawDataCache.Add(glow);
		}
    }
}