using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
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
            if (player.buffTime[buffIndex] == 1 && (modded.visibleRedAlert || modded.forceVisibleRedAlert || modded.visibleMayday || modded.forceVisibleMayday))
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
        public static Asset<Texture2D> texAsset;

        public override void Load()
        {
            texAsset = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Buffs/Alert_Alarm", AssetRequestMode.AsyncLoad);
        }

		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.IceBarrier);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            if (drawInfo.shadow != 0)
                return false;
            RedAlertPlayer modded = drawInfo.drawPlayer.GetModPlayer<RedAlertPlayer>();
            if (modded.forceVisibleRedAlert || modded.forceVisibleMayday)
                return true;
            if (drawInfo.drawPlayer.HasBuff(ModContent.BuffType<Alert>()) && (modded.visibleRedAlert || modded.visibleMayday))
                return true;
            return false;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			if (drawInfo.drawPlayer.dead) {
				return;
			}
            Texture2D texture2d = texAsset.Value;
            DrawData glow = new DrawData(
                texture: texture2d,
                position: new Vector2((int)(drawInfo.Center.X - Main.screenPosition.X), (int)(drawInfo.Center.Y - Main.screenPosition.Y))
                + new Vector2(0, -16 * 2 * drawInfo.drawPlayer.gravDir),
                sourceRect: new Rectangle(0, 0, texture2d.Width, texture2d.Height),
                color: new Color(0.8f, 0.8f, 0.8f, 0.6f),
                rotation: drawInfo.drawPlayer.GetModPlayer<RedAlertPlayer>().alertRotation * drawInfo.drawPlayer.gravDir,
                origin: new Vector2(texture2d.Width / 2, texture2d.Height / 2),
                scale: 1f,
                effect: SpriteEffects.None
            );
            //glow.shader = 1;
            drawInfo.DrawDataCache.Add(glow);
		}
    }
}