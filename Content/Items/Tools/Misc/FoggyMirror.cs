using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;


namespace SuperfluityTwo.Content.Items.Tools.Misc
{
    public class FoggyMirror : ModItem {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MagicMirror);
            Item.rare = ItemRarityID.Blue;
            Item.value = 50000;
            Item.width = 22;
            Item.height = 22;
        }

        public override void AddRecipes()
        {
            /*CreateRecipe()
                .AddIngredient(ItemID.TeleportationPotion, 30)
                .AddIngredient(ItemID.MagicMirror)
                .AddIngredient(ItemID.BlackLens)
                .AddTile(TileID.Tables)
                .AddTile(TileID.Chairs)
                .AddCondition(Condition.InGraveyard)
                .Register();*/
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (Main.rand.NextBool()) {
                Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.DarkGray, 1.2f)].velocity *= 0.5f;
			}

            if (player.itemTime == 0) {
				player.ApplyItemTime(Item);
                for (int num7 = 0; num7 < 10; num7++)
                {
                    Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.DarkGray, 1.2f)].velocity *= 0.5f;
                }
			}
            else if (player.itemTime == player.itemTimeMax / 2)
            {
                SoundEngine.PlaySound(Item.UseSound, player.position);
                for (int num8 = 0; num8 < 70; num8++)
                {
                    Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.DarkGray, 1.2f)].velocity *= 0.5f;
                }
                /*if (player.altFunctionUse == 2 && player.showLastDeath) {
                    if (Main.netMode == NetmodeID.Server) {
                        player.Teleport(player.lastDeathPostion);
                        NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, player.lastDeathPostion.X, player.lastDeathPostion.Y, 2);
                    }
                    else if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        player.Teleport(player.lastDeathPostion);
                    }
                    else if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
                    {
                        player.Teleport(player.lastDeathPostion);
                        NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, player.lastDeathPostion.X, player.lastDeathPostion.Y, 2);
                    }
                }*/
                //if (player.altFunctionUse != 2) {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    player.TeleportationPotion();
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
                {
                    NetMessage.SendData(MessageID.RequestTeleportationByServer);
                }
                //}
                for (int num9 = 0; num9 < 70; num9++)
                {
                    Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0f, 0f, 150, Color.DarkGray, 1.2f)].velocity *= 0.5f;
                }
            }
        }

        /*public override bool AltFunctionUse(Player player)
        {
            return true;
        }*/
    }
}