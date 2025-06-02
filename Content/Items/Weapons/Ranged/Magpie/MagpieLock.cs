using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SuperfluityTwo.Common;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Magpie
{
    public class MagpieLock : ModProjectile
    {
        public override string Texture => $"{nameof(SuperfluityTwo)}/Content/Items/Weapons/Ranged/Magpie/MagpieLock";
        public ref float HostMagpieIndex => ref Projectile.ai[0];
        public ref float FireTimer => ref Projectile.ai[1];
        public ref float TrackedNPC => ref Projectile.ai[2];
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            FireTimer = Magpie.USE_TIME;
            /*if (Main.myPlayer != Projectile.owner)
            {
                Projectile.Opacity = 0;
            }*/
        }

        /*public override void OnKill(int timeLeft)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("kil"), Color.Red);
        }*/

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile hostMagpie = Main.projectile[(int)HostMagpieIndex];
            if (Projectile.type != ModContent.ProjectileType<MagpieLock>() || !hostMagpie.active || hostMagpie.type != ModContent.ProjectileType<MagpieHoldout>())
            {
                Projectile.Kill();
                return;
            }
            if (hostMagpie.TryGetOwner(out Player owner) && owner.channel)
                Projectile.timeLeft = 2;

            bool stillInUse = player.channel && !player.noItems && !player.CCed;
            if (Projectile.owner == Main.myPlayer)
            {
                UpdateAim();

                if (!stillInUse)
                {
                    Projectile.Kill();
                    return;
                }
            }
        }

        private void UpdateAim()
        {
            Player owner = Main.player[Projectile.owner];
            int targetID = (int)TrackedNPC;
            NPC target = Main.npc[targetID];
            if (Main.myPlayer == Projectile.owner)
            {
                if (!MagpieReticle.trackedNPCs.Contains(targetID))
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.width = target.width;
                Projectile.height = target.height;
                Projectile.Center = target.Center;
            }
            if (FireTimer >= Magpie.USE_TIME)
            {
                if (!owner.PickAmmo(owner.HeldItem, out int proj, out float speed, out int dmg, out float kB, out int ammoItemID))
                {
                    Projectile.Kill();
                    return;
                }
                ///#TODO: Seriously revamp this to scan basically everything periodically and save the best scan result
                Vector2 aimpointTopRight    = target.TopRight    + new Vector2(-2, 2);
                Vector2 aimpointBottomRight = target.BottomRight + new Vector2(-2, -2);
                Vector2 aimpointBottomLeft  = target.BottomLeft  + new Vector2(2, -2);
                Vector2 aimpointTopLeft     = target.TopLeft     + new Vector2(2, 2);

                Vector2 aimpoint = target.Center;

                bool justFireMyGuy = new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height).Contains((int)owner.Center.X, (int)owner.Center.Y);
                List<Vector2> aimpoints = [];
                bool tryToFire = justFireMyGuy;
                if (!justFireMyGuy)
                {
                    const int AIMPOINT_DENSITY = 5;
                    int lengthLeft;
                    int pointsToAdd;
                    // checkTop
                    bool checkTop = owner.Center.Y < target.Top.Y;
                    if (checkTop)
                    {
                        lengthLeft = target.width - 2 * AIMPOINT_DENSITY;
                        pointsToAdd = Math.Max(lengthLeft / AIMPOINT_DENSITY, 0) + 1;
                        aimpoints.Add(aimpointTopLeft);
                        for (int i = 0; i < pointsToAdd; i++)
                        {
                            float ratio = (float)(i + 1) / pointsToAdd;
                            aimpoints.Add(Vector2.Lerp(aimpointTopLeft, aimpointTopRight, ratio));
                        }
                        aimpoints.Add(aimpointTopRight);
                    }
                    // checkRight
                    if (owner.Center.X > target.Right.X)
                    {
                        lengthLeft = target.height - 2 * AIMPOINT_DENSITY;
                        pointsToAdd = Math.Max(lengthLeft / AIMPOINT_DENSITY, 0) + 1;
                        aimpoints.Add(aimpointTopRight);
                        for (int i = 0; i < pointsToAdd; i++)
                        {
                            float ratio = (float)(i + 1) / pointsToAdd;
                            aimpoints.Add(Vector2.Lerp(aimpointTopRight, aimpointBottomRight, ratio));
                        }
                        aimpoints.Add(aimpointBottomRight);
                    }
                    // checkBottom
                    if (owner.Center.Y > target.Bottom.Y)
                    {
                        lengthLeft = target.width - 2 * AIMPOINT_DENSITY;
                        pointsToAdd = Math.Max(lengthLeft / AIMPOINT_DENSITY, 0) + 1;
                        aimpoints.Add(aimpointBottomRight);
                        for (int i = 0; i < pointsToAdd; i++)
                        {
                            float ratio = (float)(i + 1) / pointsToAdd;
                            aimpoints.Add(Vector2.Lerp(aimpointBottomRight, aimpointBottomLeft, ratio));
                        }
                        aimpoints.Add(aimpointBottomLeft);
                    }
                    // checkLeft
                    if (owner.Center.X < target.Left.X)
                    {
                        lengthLeft = target.height - 2 * AIMPOINT_DENSITY;
                        pointsToAdd = Math.Max(lengthLeft / AIMPOINT_DENSITY, 0) + 1;
                        //temp will keep the ordering of the list in a clockwise fashion
                        List<Vector2> temp = [];
                        if (checkTop)
                        {
                            temp = aimpoints;
                            aimpoints = [];
                        }
                        aimpoints.Add(aimpointBottomLeft);
                        for (int i = 0; i < pointsToAdd; i++)
                        {
                            float ratio = (float)(i + 1) / pointsToAdd;
                            aimpoints.Add(Vector2.Lerp(aimpointBottomLeft, aimpointTopLeft, ratio));
                        }
                        aimpoints.Add(aimpointTopLeft);
                        aimpoints.AddRange(temp);
                    }

                    int run = 0;
                    int bestScorer = 0;
                    int bestScorerRun = 0;

                    for (int i = 0; i < aimpoints.Count; i++)
                    {
                        Vector2 point = aimpoints[i];
                        bool canHit = HelperMethodsSF2.RaycastReliable(owner.Center, point);
                        /*Dust.NewDustPerfect(
                            point,
                            canHit ? DustID.Confetti_Green : DustID.Clentaminator_Red,
                            Vector2.Zero
                        ).noGravity = true;*/
                        if (canHit)
                        {
                            run++;
                            tryToFire = true;
                            if (run > bestScorerRun)
                            {
                                bestScorerRun = run;
                                bestScorer = i;
                            }
                        }
                        else
                        {
                            run = 0;
                        }
                    }
                    /*if (bestScorerRun >= aimpoints.Count)
                    {
                        bestScorer = aimpoints.Count / 2;
                    }
                    else if (bestScorerRun > 1)
                    {*/
                    bestScorer = (bestScorer - (bestScorerRun / 2)) % aimpoints.Count;
                    //}
                    //bestScorer = Math.Clamp(bestScorer, 0, aimpoints.Count);
                    aimpoint = aimpoints[bestScorer];
                }
                
                if (tryToFire)
                {
                    Vector2 targetPos = aimpoint;
                    MagpieHoldout hostMagpie = (MagpieHoldout)Main.projectile[(int)HostMagpieIndex].ModProjectile;
                    float dist = owner.Center.Distance(targetPos);
                    Vector2 aimCompensation = target.velocity * dist / speed;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        //int extraUpdates = Main.projectile
                        Projectile.NewProjectile(
                            owner.GetSource_FromThis(),
                            owner.Center,
                            owner.Center.DirectionTo(targetPos + aimCompensation).RotatedByRandom(MathHelper.ToRadians(0.5f)) * speed,
                            proj,
                            dmg,
                            kB,
                            Projectile.owner
                        );
                    }
                    hostMagpie.overrideAim = owner.Center.DirectionTo(targetPos + aimCompensation);
                    hostMagpie.overrideAimTime = Magpie.USE_TIME;
                    SoundEngine.PlaySound(SoundID.Item41, owner.Center);
                }
                FireTimer -= Magpie.USE_TIME;
            }
            FireTimer += owner.GetWeaponAttackSpeed(owner.HeldItem) / (0.75f + 0.25f * owner.ownedProjectileCounts[Type]);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.myPlayer != Projectile.owner) return false;
            
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 orig = new Vector2(texture.Width / 2, texture.Height / 2);

            Color drawColor = Projectile.GetAlpha(lightColor);

            int heightLeftToDraw = Projectile.height - 12;
            while (heightLeftToDraw > 0)
            {
                int heightToChop = Math.Min(heightLeftToDraw, 6);
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopLeft + new Vector2(0, Projectile.height - heightLeftToDraw - 6) - Main.screenPosition).Floor() + orig,
                    new Rectangle(0, 8, 6, heightToChop),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopRight + new Vector2(0, Projectile.height - heightLeftToDraw - 6) - Main.screenPosition + new Vector2(-6, 0)).Floor() + orig,
                    new Rectangle(40, 8, 6, heightToChop),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                heightLeftToDraw -= heightToChop;
            }

            int widthLeftToDraw = Projectile.width - 12;
            while (widthLeftToDraw > 0)
            {
                int widthToChop = Math.Min(widthLeftToDraw, 6);
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.TopLeft + new Vector2(Projectile.width - widthLeftToDraw - 6, 0) - Main.screenPosition).Floor() + orig,
                    new Rectangle(8, 0, widthToChop, 6),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );
                Main.EntitySpriteDraw(
                    texture,
                    (Projectile.BottomLeft + new Vector2(Projectile.width - widthLeftToDraw - 6, -6) - Main.screenPosition).Floor() + orig,
                    new Rectangle(8, 16, widthToChop, 6),
                    drawColor,
                    Projectile.rotation,
                    orig,
                    1,
                    SpriteEffects.None,
                    0f
                );

                widthLeftToDraw -= widthToChop;
            }

            Main.EntitySpriteDraw(
                texture,
                (Projectile.TopLeft - Main.screenPosition).Floor() + orig,
                new Rectangle(0, 0, 6, 6),
                drawColor,
                Projectile.rotation,
                orig,
                1,
                SpriteEffects.None,
                0f
            );
            Main.EntitySpriteDraw(
                texture,
                (Projectile.TopRight - Main.screenPosition + new Vector2(-6, 0)).Floor() + orig,
                new Rectangle(40, 0, 6, 6),
                drawColor,
                Projectile.rotation,
                orig,
                1,
                SpriteEffects.None,
                0f
            );
            Main.EntitySpriteDraw(
                texture,
                (Projectile.BottomLeft - Main.screenPosition + new Vector2(0, -6)).Floor() + orig,
                new Rectangle(0, 16, 6, 6),
                drawColor,
                Projectile.rotation,
                orig,
                1,
                SpriteEffects.None,
                0f
            );
            Main.EntitySpriteDraw(
                texture,
                (Projectile.BottomRight - Main.screenPosition + new Vector2(-6, -6)).Floor() + orig,
                new Rectangle(40, 16, 6, 6),
                drawColor,
                Projectile.rotation,
                orig,
                1,
                SpriteEffects.None,
                0f
            );

            Main.EntitySpriteDraw(
                texture,
                (Projectile.Center - Main.screenPosition + new Vector2(-3, -3)).Floor() + orig,
                new Rectangle(8, 8, 6, 6),
                drawColor,
                Projectile.rotation,
                orig,
                1,
                SpriteEffects.None,
                0f
            );
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }
    }
}