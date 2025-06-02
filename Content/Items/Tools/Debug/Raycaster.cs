using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Common;
using SuperfluityTwo.Content.Items.Weapons.Ranged.Meridian;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Tools.Debug
{
	public class Raycaster : ModItem
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
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				if (player.altFunctionUse == 2)
				{
					HelperMethodsSF2.RaycastReliable(player.Center, Main.MouseWorld, true);
				}
				else
				{
					Vector2 dir = Main.MouseWorld - player.Center;
					for (int i = -1; i <= 1; i++)
					{
						for (int j = -1; j <= 1; j++)
						{
							Vector2 startPos = player.Center + new Vector2(Main.rand.Next(5, 16) * i, Main.rand.Next(5, 16) * j);
							HelperMethodsSF2.Raycast(startPos, startPos + dir.RotatedByRandom(0.1f * j * i), debug: true);
						}
					}
				}
			}
			return true;
		}
    }
}