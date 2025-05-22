using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Accessories
{
	[AutoloadEquip(EquipType.Neck)]
	public class Phylactery : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 24;
			Item.value = Item.sellPrice(gold: 6);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statLifeMax2 += 50;
			player.GetModPlayer<PhylacteryPlayer>().hasPhylactery = true;
			//player.endurance += 0.95f * (1f - player.endurance);
		}
	}

	internal class PhylacteryPlayer : ModPlayer
	{
		public bool hasPhylactery = false;
		public override void ResetEffects()
		{
			hasPhylactery = false;
		}

		public override void PostUpdateEquips()
		{
			if (hasPhylactery)
			{
				float healthRatio = (float)Player.statLife / Player.statLifeMax2;
				float protect = Math.Clamp(0.7f + 0.3f * healthRatio, 0.7f, 1f);
				Player.endurance += 1f - protect * (1f - Player.endurance);
			}
		}

        public override void UpdateLifeRegen()
        {
			if (hasPhylactery)
			{
				float healthRatio = (float)Player.statLife / Player.statLifeMax2;
				Player.lifeRegen += (int)((1.0 - healthRatio) * 10);
				if (healthRatio < 0.5f)
					Player.lifeRegenTime += 1;
				if (healthRatio < 0.2f)
					Player.lifeRegenTime += 1;
			}
        }
	}
}