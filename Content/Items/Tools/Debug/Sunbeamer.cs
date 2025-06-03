using Microsoft.Xna.Framework;
using SuperfluityTwo.Common;
using SuperfluityTwo.Common.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Tools.Debug
{
	public class Sunbeamer : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.useTime = 3;
			Item.useAnimation = 3;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Type] = true;
			Item.rare = ItemRarityID.Yellow;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.scale = 0.7f;
			Item.autoReuse = true;
			SF2ItemSets.isDebugItem[Type] = true;
		}

		int alt = 0;
		int alt2 = 0;
		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				Vector2 dir = Main.MouseWorld - player.Center;
				for (float theta = (alt * -MathHelper.PiOver4 / 2f) + (0.5f * alt2 * MathHelper.PiOver4 / 30f); theta <= ((1 - alt) * MathHelper.PiOver4 / 2f) - (0.5f * (1 - alt2) * MathHelper.PiOver4 / 30f); theta += MathHelper.PiOver4 / 30f)
				{
					HelperMethodsSF2.Raycast(player.Center, player.Center + dir.RotatedBy(theta), debug: true);
				}
				alt = 1 - alt;
				if (alt == 0)
				{
					alt2 = 1 - alt2;
				}
			}
			return true;
		}
    }
}