using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperfluityTwo.Common.Players;
using SuperfluityTwo.Content.Items.Weapons.Magic.FairyKnife;
using Microsoft.Xna.Framework;
namespace SuperfluityTwo.Content.Items.Accessories.Ranger.Pal
{
    public class Pal : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FlintlockPistol)
                .AddIngredient(ItemID.HellstoneBar, 15)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PalPlayer>().hasPal = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<PalPlayer>().hasPalVanity = true;
        }
    }

    public class PalPlayer : ModPlayer
    {
        public bool hasPal = false;
        public bool hasPalVanity = false;

        public override void ResetEffects()
        {
            hasPal = false;
            hasPalVanity = false;
        }

        public override void PostUpdateEquips()
        {
            if ((hasPal || hasPalVanity) && Player.ownedProjectileCounts[ModContent.ProjectileType<PalProj>()] <= 0)
            {
                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<PalProj>(),
                    15,
                    1f,
                    Player.whoAmI,
                    hasPalVanity && !hasPal ? 1 : 0
                );
            }
        }
    }
}