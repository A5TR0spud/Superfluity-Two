using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperfluityTwo.Common;
using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace SuperfluityTwo
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class SuperfluityTwo : Mod
	{
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			byte msgID = reader.ReadByte();
			switch (msgID)
			{
				case (byte)SF2NetworkID.SyncPlayerThaumaturgyCycle_S2C:
					int playerToCopy = reader.Read7BitEncodedInt();
					int cycleTimer = reader.Read7BitEncodedInt();
					Main.player[playerToCopy].GetModPlayer<ThaumaturgyPlayer>().ThaumaturgyCycleTimer = cycleTimer;
					break;
				default:
					Logger.WarnFormat("SuperfluityTwo: Unknown Message type: {0}", msgID);
					break;
			}
        }
	}
}
