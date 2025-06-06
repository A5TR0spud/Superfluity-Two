using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Accessories.Mayday
{
    //[AutoloadEquip(EquipType.Face)]
	public class ATGMissileLauncher : ModItem
	{
        public static readonly int missileCount = 1;
        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 30;
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