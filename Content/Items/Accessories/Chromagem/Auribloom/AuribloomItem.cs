using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SuperfluityTwo.Content.Items.Accessories.Chromagem.Auribloom
{
    [AutoloadEquip(EquipType.Face)]
    public class AuribloomItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[EquipLoader.GetEquipSlot(Mod, this.Name, EquipType.Face)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.defense = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 12)
                .AddIngredient(ItemID.Amber, 5)
                .AddTile(TileID.Anvils)
                .AddDecraftCondition(Condition.CrimsonWorld)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 12)
                .AddIngredient(ItemID.Amber, 5)
                .AddTile(TileID.Anvils)
                .AddDecraftCondition(Condition.CorruptWorld)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.dead) return;
            //player.lifeRegen += 5;
            //player.buffImmune[ModContent.BuffType<GoldBloodBuff>()] = true;
            player.GetModPlayer<AuribloomPlayer>().hasAuri = true;
            if (player.statLife < player.statLifeMax2)
            {
                player.AddBuff(ModContent.BuffType<GoldBloodBuff>(), 5 * 60);
            }

            if (!hideVisual && Main.rand.NextBool(8))
            {
                Dust.NewDustDirect(player.Center + 16 * 7 * Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), 0, 0, DustID.GoldFlame, Alpha: 0).noGravity = true;
            }
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.friendly && npc.Center.Distance(player.Center) < 16 * 7)
                {
                    npc.AddBuff(ModContent.BuffType<GoldBloodBuff>(), 60 * 5);
                }
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                foreach (var plr in Main.ActivePlayers)
                {
                    if (plr == player) continue;
                    if (!plr.InOpposingTeam(player) && plr.Center.Distance(player.Center) < 16 * 7 && !plr.dead)
                    {
                        plr.AddBuff(ModContent.BuffType<GoldBloodBuff>(), 5 * 60);
                    }
                }
            }
        }
    }

    public class AuribloomPlayer : ModPlayer
    {
        public bool hasAuri = false;
        public override void ResetEffects()
        {
            hasAuri = false;
        }
    }

    
    internal class AuriLayer : PlayerDrawLayer
	{
        public static Asset<Texture2D> texAsset;

        public override void Load()
        {
            texAsset = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Items/Accessories/Chromagem/Auribloom/AuribloomVisual", AssetRequestMode.AsyncLoad);
        }

		public override Position GetDefaultPosition() {
			return PlayerDrawLayers.BeforeFirstVanillaLayer;
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            if (drawInfo.shadow != 0)
                return false;
            AuribloomPlayer modded = drawInfo.drawPlayer.GetModPlayer<AuribloomPlayer>();
            if (modded.hasAuri)
                return true;
            return false;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			if (drawInfo.drawPlayer.dead) {
				return;
			}
            Texture2D texture2d = texAsset.Value;
            DrawData glow = new DrawData(
                texture: texture2d,
                position: new Vector2((int)(drawInfo.Center.X - Main.screenPosition.X), (int)(drawInfo.Center.Y - Main.screenPosition.Y)),
                sourceRect: new Rectangle(0, 0, texture2d.Width, texture2d.Height),
                color: new Color(1f, 1f, 1f, 1f),
                rotation: 0,
                origin: new Vector2(texture2d.Width / 2, texture2d.Height / 2),
                scale: 1f,
                effect: SpriteEffects.None
            );
            drawInfo.DrawDataCache.Add(glow);
		}
    }
}