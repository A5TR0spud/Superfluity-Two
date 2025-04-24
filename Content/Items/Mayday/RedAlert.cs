using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Common.Players;

namespace SuperfluityTwo.Content.Items.Mayday
{
    //[AutoloadEquip(EquipType.Face)]
	public class RedAlert : GlowItem
	{
        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 30;
			Item.value = Item.sellPrice(gold: 1, silver: 15);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Ruby, 5)
                .AddIngredient(ItemID.DemoniteBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.Ruby, 5)
                .AddIngredient(ItemID.CrimtaneBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateVanity(Player player)
        {
            RedAlertPlayer modded = player.GetModPlayer<RedAlertPlayer>();
            modded.forceVisibleRedAlert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            RedAlertPlayer modded = player.GetModPlayer<RedAlertPlayer>();
            modded.hasRedAlert = true;
            modded.visibleRedAlert = !hideVisual;
        }

        /*public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Glow", AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale, 
                SpriteEffects.None, 
                0f
            );
        }*/
    }
}