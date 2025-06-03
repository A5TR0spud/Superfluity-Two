using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria;

namespace SuperfluityTwo.Content.Items
{
    public class GlowItem : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !GetType().Name.Equals("GlowItem");
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
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
        }
    }
}