using Microsoft.Xna.Framework;
using SuperfluityTwo.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SuperfluityTwo.Content.Items.Weapons.Magic.FairyKnife
{
	public class FairyKnife : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MagicDagger);
			Item.damage = 13;
			Item.width += 2;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.knockBack = 2;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.mana = 5;
			Item.shoot = ModContent.ProjectileType<FairyKnifeProj>();
			Item.shootSpeed = 10f;
			Item.consumable = true;
			Item.maxStack = Item.CommonMaxStack;
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			position += Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * 8f;
        }
    }
}