using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using log4net.Core;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperfluityTwo.Common;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Meridian
{
    public class MeridianRocket : ModProjectile
    {
        ref float timer => ref Projectile.ai[0];
        Vector2 TargetPos
        {
            get => new Vector2(Projectile.ai[1], Projectile.ai[2]);
            set
            {
                Projectile.ai[1] = value.X;
                Projectile.ai[2] = value.Y;
            }
        }

        float maxSpeed;

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 75;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            timer = 0;
            Projectile.velocity *= 0.3f;
            maxSpeed = Projectile.velocity.Length();
            RocketFireCounter = 0;
            RocketAmmoCycle = Main.rand.Next(100);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WritePackedVector2(Projectile.position);
            writer.WritePackedVector2(Projectile.velocity);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.position = reader.ReadPackedVector2();
            Projectile.velocity = reader.ReadPackedVector2();
        }

        Vector2 oldTargetPos = new Vector2(0, 0);

        
        float RocketFireCounter = 0;
        int RocketAmmoCycle = 0;

        public override void AI()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                TargetPos = Main.MouseWorld;
                if (oldTargetPos != TargetPos) Projectile.netUpdate = true;
                oldTargetPos = TargetPos;
                if (RocketFireCounter <= -5)
                {
                    Player owner = Main.player[Projectile.owner];
                    if (CycleRocketAmmo(out Item chosenAmmoItem))
                    {
                        Vector2 dir = Projectile.Center.DirectionTo(Main.MouseWorld);
                        Projectile.NewProjectile(
                            owner.GetSource_FromThis(),
                            Projectile.Center,
                            dir * (20 + chosenAmmoItem.shootSpeed),
                            HelperMethodsSF2.GetRocketShoot(chosenAmmoItem),
                            Projectile.damage + chosenAmmoItem.damage,
                            Projectile.knockBack + chosenAmmoItem.knockBack,
                            Projectile.owner
                        );
                    }
                    RocketFireCounter += 24;
                }
                RocketFireCounter--;
            }

            Home(TargetPos, maxSpeed);

            if (timer % 5 == 0)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Vortex
                );
                dust.noGravity = true;
            }
            timer++;

            if (Projectile.timeLeft < 20)
            {
                Projectile.alpha = (int)(255 * (1 - (Projectile.timeLeft / 20f)));
            }
        }

        const float ACCELERATION = 1.0f;
        private void Home(Vector2 targetPos, float maxVel)
        {
            Vector2 dir = Projectile.DirectionTo(targetPos);
            Vector2 newVel = Projectile.velocity + dir * ACCELERATION;
            if (newVel.Length() > maxVel)
            {
                newVel = newVel.SafeNormalize(dir) * maxVel;
            }
            Projectile.velocity = newVel;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1 - Projectile.alpha / 255f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

            float theta = -0.05f * timer;

            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture, drawPos, null, color.MultiplyRGBA(new(0.5f, 0.5f, 0.5f, 1f)), 0.5f * theta, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, color, 1.5f * theta, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
        
        private bool CycleRocketAmmo(out Item chosenAmmoItem)
        {
            bool toRet = false;
            if (!Projectile.TryGetOwner(out Player owner))
            {
                chosenAmmoItem = null;
                return toRet;
            }
            List<Item> ammoIDs = [];
            foreach (Item ammo in owner.inventory)
            {
                if (!HelperMethodsSF2.CanItemBeShot(ammo, true) || !(HelperMethodsSF2.IsRocket(ammo.ammo) || HelperMethodsSF2.IsOther(ammo.ammo)))
                {
                    continue;
                }
                ammoIDs.Add(ammo);
            }
            if (ammoIDs.Count > 0)
            {
                chosenAmmoItem = ammoIDs[RocketAmmoCycle % ammoIDs.Count];
                toRet = true;
                RocketAmmoCycle++;
            }
            else
            {
                chosenAmmoItem = null;
            }
            return toRet;
        }
    }   
}