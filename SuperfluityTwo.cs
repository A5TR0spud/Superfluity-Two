using System.IO;
using SuperfluityTwo.Common;
using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class SuperfluityTwo : Mod
	{
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			byte msgID = reader.ReadByte();
			if (Main.netMode == NetmodeID.Server)
			{
				ModPacket myPacket = ModContent.GetInstance<SuperfluityTwo>().GetPacket();
				//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("SERVER: Packet recieved"), Color.YellowGreen);
				switch (msgID)
				{
					case (byte)SF2NetworkID.JoiningPlayerRequestingSync:
						reader.Read7BitEncodedInt();
						myPacket.Write((byte)SF2NetworkID.JoiningPlayerRequestingSync);
						myPacket.Write7BitEncodedInt(whoAmI);
						myPacket.Send(ignoreClient: whoAmI);
						break;
					case (byte)SF2NetworkID.SyncPlayerThaumaturgyCycle:
						int toWho = reader.Read7BitEncodedInt();
						int cycleTimer = reader.Read7BitEncodedInt();
						myPacket.Write((byte)SF2NetworkID.SyncPlayerThaumaturgyCycle);
						myPacket.Write7BitEncodedInt(whoAmI);
						myPacket.Write7BitEncodedInt(cycleTimer);
						myPacket.Send(toClient: toWho, ignoreClient: whoAmI);
						break;
					default:
						Logger.WarnFormat("SuperfluityTwo: Unknown Message type: {0}", msgID);
						break;
				}
				return;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				int fromWho;
				switch (msgID)
				{
					case (byte)SF2NetworkID.JoiningPlayerRequestingSync:
						fromWho = reader.Read7BitEncodedInt();
						Main.LocalPlayer.GetModPlayer<NetworkedPlayer>().SendInfoToNewPlayer(fromWho);
						break;
					case (byte)SF2NetworkID.SyncPlayerThaumaturgyCycle:
						fromWho = reader.Read7BitEncodedInt();
						int cycleTimer = reader.Read7BitEncodedInt();
						Main.player[fromWho].GetModPlayer<ThaumaturgyPlayer>().ThaumaturgyCycleTimer = cycleTimer;
						break;
					default:
						Logger.WarnFormat("SuperfluityTwo: Unknown Message type: {0}", msgID);
						break;
				}
				return;
			}
			
        }
	}
}
