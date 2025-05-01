using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using System;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Mayday
{
    //[AutoloadEquip(EquipType.Face)]
	public class ATGMissileLauncher : ModItem
	{
        public static readonly int missileCount = 1;
        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 34;
			Item.value = Item.sellPrice(gold: 4);
			Item.rare = ItemRarityID.Green;
            Item.damage = ATGPlayer.damage;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = ATGPlayer.knockback;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Grenade, 20)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddIngredient(ItemID.HellstoneBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool RangedPrefix()
        {
            return false;
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ATGPlayer>().MissilesPerLaunch += 1;
            //player.GetModPlayer<ATGPlayer>().MissilesBonus += 1;
        }
    }
}