using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class NetworkedPlayer : ModPlayer
    {
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (toWho == fromWho) return;
            if (newPlayer && Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket myPacket = ModContent.GetInstance<SuperfluityTwo>().GetPacket();
                myPacket.Write((byte)SF2NetworkID.JoiningPlayerRequestingSync);
                myPacket.Write7BitEncodedInt(0);
                myPacket.Send();
            }
        }

        internal void PingedForReturnInfo(int newPlayer)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) return;
            WriteThaumaturgyPacket(newPlayer);
        }

        internal void WriteThaumaturgyPacket(int toWho)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) return;
            ModPacket myPacket = ModContent.GetInstance<SuperfluityTwo>().GetPacket();
            myPacket.Write((byte)SF2NetworkID.SyncPlayerThaumaturgyCycle);
            myPacket.Write7BitEncodedInt(toWho);
            myPacket.Write7BitEncodedInt(Player.GetModPlayer<ThaumaturgyPlayer>().ThaumaturgyCycleTimer);
            myPacket.Send();
        }
    }
}