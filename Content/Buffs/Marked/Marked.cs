using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Buffs.Marked
{
    public class Marked : ModBuff
    {
        public static Asset<Texture2D> MarkSprite;
        public override void Load()
        {
            MarkSprite = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Buffs/Marked/Mark", AssetRequestMode.AsyncLoad);;
        }
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<MarkedNPC>().marked = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MarkedPlayer>().marked = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }

    internal class MarkedPlayer : ModPlayer
    {
        public bool marked = false;
        public override void ResetEffects()
        {
            marked = false;
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (marked) modifiers.FinalDamage *= 2;
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (marked) modifiers.FinalDamage *= 2;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            Player.ClearBuff(ModContent.BuffType<Marked>());
        }
    }

    internal class MarkedNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool marked = false;
        public override void ResetEffects(NPC npc)
        {
            marked = false;
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (marked) modifiers.SetCrit();
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (npc.HasBuff(ModContent.BuffType<Marked>()))
            {
                npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<Marked>()));
                Dust.NewDustPerfect(
                    npc.Center,
                    DustID.Firework_Blue,
                    Vector2.Zero,
                    0,
                    Color.White,
                    0.75f
                ).noGravity = true;
                for (int i = 0; i < 5; i++)
                {
                    float d = (i + 1) / 2.5f;
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.Firework_Blue,
                        new Vector2(d, d),
                        0,
                        Color.White,
                        0.75f
                    ).noGravity = true;
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.Firework_Blue,
                        new Vector2(d, -d),
                        0,
                        Color.White,
                        0.75f
                    ).noGravity = true;
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.Firework_Blue,
                        new Vector2(-d, d),
                        0,
                        Color.White,
                        0.75f
                    ).noGravity = true;
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.Firework_Blue,
                        new Vector2(-d, -d),
                        0,
                        Color.White,
                        0.75f
                    ).noGravity = true;
                }
            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (marked)
            {
                drawColor.B = 255;
                drawColor.G = Math.Max(drawColor.G, (byte)128);
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (marked)
            {
                spriteBatch.Draw(
                    Marked.MarkSprite.Value,
                    npc.Center - screenPos - Marked.MarkSprite.Size() / 2,
                    Color.White
                );
            }
        }
    }
}