using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace SuperfluityTwo.Content.Items.Tools.Misc
{
    public class ReturnAddress : ModItem {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MagicMirror);
            Item.rare = ItemRarityID.Blue;
            Item.value = 50000;
            Item.width = 24;
            Item.height = 22;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PotionOfReturn, 30)
                .AddIngredient(ItemID.MagicMirror)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.RedTorch)
                .AddTile(TileID.Tables)
                .AddTile(TileID.Chairs)
                .Register();
            
            CreateRecipe()
                .AddIngredient(ItemID.PotionOfReturn, 30)
                .AddIngredient(ItemID.IceMirror)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.RedTorch)
                .AddTile(TileID.Tables)
                .AddTile(TileID.Chairs)
                .Register();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (Main.rand.NextBool()) {
                Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
			}

            if (player.itemTime == 0) {
				player.ApplyItemTime(Item);
                for (int num7 = 0; num7 < 10; num7++)
                {
                    Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
                }
			}
            else if (player.itemTime == player.itemTimeMax / 2)
            {
                SoundEngine.PlaySound(Item.UseSound, player.position);
                for (int num8 = 0; num8 < 70; num8++)
                {
                    Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
                }
                if (player.whoAmI == Main.myPlayer)
                {
                    if (player.altFunctionUse == 2)
                        player.DoPotionOfReturnReturnToOriginalUsePosition();
                    else
                        player.DoPotionOfReturnTeleportationAndSetTheComebackPoint();
                }
                for (int num9 = 0; num9 < 70; num9++)
                {
                    Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0f, 0f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
                }
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        /*public override bool? UseItem(Player player)
        {
            for (int num7 = 0; num7 < 10; num7++)
            {
                Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
            }
            return true;
        }

        public override void Load()
        {
            On_Player.ItemCheck_Inner += ReturnAddressHook;
        }

        private void ReturnAddressHook(On_Player.orig_ItemCheck_Inner orig, Player self)
        {
            orig(self);
            Item item = self.inventory[self.selectedItem];
            if (item.type != ModContent.ItemType<ReturnAddress>()) return;
            if (!self.ItemTimeIsZero && self.itemTime == self.itemTimeMax / 2)
            {
                SoundEngine.PlaySound(self.HeldItem.UseSound, self.position);
                for (int num8 = 0; num8 < 70; num8++)
                {
                    Main.dust[Dust.NewDust(self.position, self.width, self.height, DustID.MagicMirror, self.velocity.X * 0.2f, self.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
                }
                if (self.whoAmI == Main.myPlayer)
                {
                    self.DoPotionOfReturnTeleportationAndSetTheComebackPoint();
                }
                for (int num9 = 0; num9 < 70; num9++)
                {
                    Main.dust[Dust.NewDust(self.position, self.width, self.height, DustID.MagicMirror, 0f, 0f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
                }
            }
        }*/
    }
}