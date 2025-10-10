using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SuperfluityTwo.Common.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using MonoMod.Cil;

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

            if ((Main.LocalPlayer == player || (Main.LocalPlayer.team == player.team && player.team != 0)) && !hideVisual && Main.rand.NextBool(8))
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
                    if (plr.team != player.team || player.team == 0) continue;
                    if (plr.Center.Distance(player.Center) < 16 * 7 && !plr.dead)
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
        public float auriDelta = 0f;
        public override void ResetEffects()
        {
            hasAuri = false;
        }
        public override void PostUpdateEquips()
        {
            if (hasAuri)
            {
                auriDelta += 0.1f;
            }
            else
            {
                auriDelta = 0;
            }
        }

        public override void Load()
        {
            IL_Main.DoDraw += DrawAuriZone;
            texAsset = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Items/Accessories/Chromagem/Auribloom/AuribloomVisual", AssetRequestMode.AsyncLoad);
        }
        public override void Unload()
        {
            IL_Main.DoDraw -= DrawAuriZone;
        }
        
        
        public static Asset<Texture2D> texAsset;

        private void DrawAuriZone(ILContext il)
        {

            try
            {
                ILCursor c2 = new ILCursor(il);
                c2.GotoNext(i => i.MatchCall<Terraria.Main>("DrawInfernoRings"));
                c2.Index++; //Move cursor to after inferno is drawn
                c2.EmitDelegate(() =>
                {
                    foreach (var player in Main.ActivePlayers)
                    {
                        if (player.outOfRange || !player.GetModPlayer<AuribloomPlayer>().hasAuri || player.dead)
                        {
                            continue;
                        }
                        if ((player.team != Main.LocalPlayer.team || player.team == 0) && player != Main.LocalPlayer) {
                            continue;
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            float d = MathHelper.Pi * i / 6f;
                            float auriDelta = player.GetModPlayer<AuribloomPlayer>().auriDelta;
                            Main.spriteBatch.Draw(
                                texture: texAsset.Value,
                                position: player.Center - Main.screenPosition,
                                sourceRectangle: null,
                                color: new Color(new Vector4(0.85f + 0.15f * (float)Math.Sin(0.1f * auriDelta))),
                                rotation: d + 0.075f * auriDelta + 0.025f * (float)Math.Sin(d * 3f + auriDelta),
                                origin: texAsset.Size() / 2,
                                scale: 1f + 0.05f * (float)Math.Sin(auriDelta * 0.1f),
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                        }
                    }
                });
            }
            catch (Exception)
            {
                // If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
                MonoModHooks.DumpIL(ModContent.GetInstance<SuperfluityTwo>(), il);

                // If the mod cannot run without the IL hook, throw an exception instead. The exception will call DumpIL internally
                // throw new ILPatchFailureException(ModContent.GetInstance<ExampleMod>(), il, e);
            }
        }
    }
}