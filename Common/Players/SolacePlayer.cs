using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Net;


namespace SuperfluityTwo.Common.Players
{
    public class SolacePlayer : ModPlayer {
        public bool hasBastetBlessing = false;
        public bool hasLuckyClover = false;
        public int bastetProtectVFXTime = 0;
        public bool bastetVisible = false;
        public bool bastetAnkhVisible = false;
        public bool forceBastetVisible = false;
        public bool hasLoverLocket = false;
        public int heartPickupCooldown = 0;
        public override void ResetEffects()
        {
            hasBastetBlessing = false;
            hasLuckyClover = false;
            bastetVisible = false;
            forceBastetVisible = false;
            bastetAnkhVisible = false;
            hasLoverLocket = false;
        }
        public override void Load()
        {
            On_Player.Hurt_HurtInfo_bool += HookHurt;
        }

        private void HookHurt(On_Player.orig_Hurt_HurtInfo_bool orig, Player self, Player.HurtInfo info, bool quiet)
        {
            if (self.GetModPlayer<SolacePlayer>().hasBastetBlessing && self.statLife - info.SourceDamage <= self.statLife * 0.33) {
                int initDmg = info.Damage;
                info.Damage = (int)(info.Damage * 0.7f);
                SolacePlayer modded = self.GetModPlayer<SolacePlayer>();
                if (modded.forceBastetVisible || modded.bastetVisible)
                    for (int i = 0; i < 10; i++)
                        Dust.NewDust(self.position, self.width, self.height, DustID.GoldFlame, 0, -4);
                if (self.statLife - initDmg <= 0 && self.statLife - info.Damage > 0 && (modded.bastetAnkhVisible || modded.forceBastetVisible)) 
                    modded.bastetProtectVFXTime = 60;
            }
            orig(self, info, quiet);
        }

        public override void ModifyLuck(ref float luck)
        {
            if (hasLuckyClover)
                luck += 0.05f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.netMode == NetmodeID.SinglePlayer) {
                if (Main.rand.NextBool(20) && heartPickupCooldown <= 0) {
                    heartPickupCooldown = 6 * 60;
                    Item.NewItem(target.GetSource_Loot(), (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.Heart);
                }
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient) {
                if (Main.rand.NextBool(20) && heartPickupCooldown <= 0) {
                    heartPickupCooldown = 6 * 60;
                    int index = Item.NewItem(target.GetSource_Loot(), (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.Heart);
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, index, 1f);
                }
            }
            
        }

        public override void PostUpdate()
        {
            if (bastetProtectVFXTime > 0) bastetProtectVFXTime--;
            if (heartPickupCooldown > 0) heartPickupCooldown--;
        }

        public override void OnRespawn()
        {
            bastetProtectVFXTime = 0;
        }
    }

    internal class BastetPlayerLayer : PlayerDrawLayer
	{
        public static Asset<Texture2D> texAsset;
        public override void Load()
        {
            texAsset = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Items/Accessories/Solace/BastetVFX", AssetRequestMode.AsyncLoad);
        }

		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.IceBarrier);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            if (drawInfo.shadow != 0)
                return false;
            SolacePlayer modded = drawInfo.drawPlayer.GetModPlayer<SolacePlayer>();
            if (modded.bastetProtectVFXTime > 0)
                return true;
            return false;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			if (drawInfo.drawPlayer.dead) {
				return;
			}
            SolacePlayer modded = drawInfo.drawPlayer.GetModPlayer<SolacePlayer>();
            Texture2D texture2d = texAsset.Value;
            float offset = modded.bastetProtectVFXTime / 60.0f;
            offset = offset * offset - 1.0f;
            offset *= 60.0f;
            float a = modded.bastetProtectVFXTime / 60.0f;
            DrawData glow = new DrawData(
                texture: texture2d,
                position: new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2), (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2)
                + new Vector2(0, offset),
                sourceRect: new Rectangle(0, 0, texture2d.Width, texture2d.Height),
                color: new Color(a, a, a, a),
                rotation: 0,
                origin: new Vector2(texture2d.Width / 2, texture2d.Height / 2),
                scale: 1f,
                effect: SpriteEffects.None
            );
            glow.shader = 0;
            //glow.color.A = (byte)(int)(a * 255);
            drawInfo.DrawDataCache.Add(glow);
		}
    }
}