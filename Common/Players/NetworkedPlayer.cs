using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class NetworkedPlayer : ModPlayer
    {
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            //if (toWho == fromWho) return;
            /*if (newPlayer && Main.netMode == NetmodeID.MultiplayerClient)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("JoiningPlayerRequestingSync Packaging"), Color.Yellow);
                ModPacket myPacket = ModContent.GetInstance<SuperfluityTwo>().GetPacket();
                myPacket.Write((byte)SF2NetworkID.JoiningPlayerRequestingSync);
                myPacket.Write7BitEncodedInt(0);
                myPacket.Send();
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("JoiningPlayerRequestingSync Sent"), Color.Yellow);
            }*/
            /*if (!newPlayer && Main.netMode == NetmodeID.MultiplayerClient)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Sending info to player: " + toWho.ToString()), Color.YellowGreen);
                SendInfoToNewPlayer(toWho);
            }*/
        }

        public override void OnEnterWorld()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket myPacket = ModContent.GetInstance<SuperfluityTwo>().GetPacket();
                myPacket.Write((byte)SF2NetworkID.JoiningPlayerRequestingSync);
                myPacket.Write7BitEncodedInt(0);
                myPacket.Send();
            }
        }

        internal void SendInfoToNewPlayer(int newPlayer)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) return;
            WriteThaumaturgyPacket(newPlayer);
        }

        internal void WriteThaumaturgyPacket(int toWho)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) return;
            ThaumaturgyPlayer modded = Player.GetModPlayer<ThaumaturgyPlayer>();
            if (!modded.hasThaumaturgy) return;
            ModPacket myPacket = ModContent.GetInstance<SuperfluityTwo>().GetPacket();
            myPacket.Write((byte)SF2NetworkID.SyncPlayerThaumaturgyCycle);
            myPacket.Write7BitEncodedInt(toWho);
            myPacket.Write7BitEncodedInt(modded.ThaumaturgyCycleTimer);
            myPacket.Send();
        }
    }
}