using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SuperfluityTwo.Common;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Meridian
{
    public class MeridianHoldout : ModProjectile
    {
        public SoundStyle BULLET_SHOOT_SOUND = SoundID.Item11;
        public SoundStyle ARROW_SHOOT_SOUND = SoundID.Item5;
        public SoundStyle ROCKET_SHOOT_SOUND = SoundID.Item38;
        //public override string Texture => $"{nameof(SuperfluityTwo)}/Content/Items/Weapons/Ranged/Meridian/MeridianHoldout";
        private static Asset<Texture2D> texAsset;
        internal const int MAX_FRAMES = 6;
        public ref float FrameCounter => ref Projectile.ai[0];

        public override void Load()
        {
            texAsset = ModContent.Request<Texture2D>($"{nameof(SuperfluityTwo)}/Content/Items/Weapons/Ranged/Meridian/MeridianHoldout", AssetRequestMode.AsyncLoad);
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = MAX_FRAMES;
            // Signals to Terraria that this Projectile requires a unique identifier beyond its index in the Projectile array.
            // This prevents the issue with the vanilla Last Prism where the beams are invisible in multiplayer.
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;

            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            //Projectile.timeLeft = 600;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 84;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = ProjAIStyleID.HeldProjectile;
            Projectile.netImportant = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, false);

            // Update Projectile visuals and sound.
            FrameCounter++;
            UpdateAnimation();
            //PlaySounds();

            // Update the Prism's position in the world and relevant variables of the player holding it.
            UpdatePlayerVisuals(player, rrp);

            bool stillInUse = player.channel && !player.noItems && !player.CCed;
            // Update the Prism's behavior: project beams on frame 1, consume mana, and despawn if out of mana.
            if (Projectile.owner == Main.myPlayer)
            {
                // Slightly re-aim the Prism every frame so that it gradually sweeps to point towards the mouse.
                UpdateAim(rrp, player.HeldItem.shootSpeed);

                // The Prism immediately stops functioning if the player is Cursed (player.noItems) or "Crowd Controlled", e.g. the Frozen debuff.
                // player.channel indicates whether the player is still holding down the mouse button to use the item.


                // Spawn in the Prism's lasers on the first frame if the player is capable of using the item.
                /*if (stillInUse && FrameCounter == 1f)
                {
                    FireBeams();
                }*/


                // If the Prism cannot continue to be used, then destroy it immediately.
                /*else*/
                if (!stillInUse)
                {
                    Projectile.Kill();
                    return;
                }
            }

            if (stillInUse)
            {
                HandleFiring();
            }

            // This ensures that the Prism never times out while in use.
            Projectile.timeLeft = 2;
        }

        public override void OnSpawn(IEntitySource source)
        {
            ArrowFireCounter = 0;
            ArrowSoundTimer = 0;
            BulletFireCounter = 0;
            BulletSoundTimer = 0;
            ArrowAmmoCycle = 0;
            BulletAmmoCycle = 0;
            RocketFireCounter = 0;
        }

        private void HandleFiring()
        {
            HandleArrows(Projectile.owner == Main.myPlayer);
            HandleBullets(Projectile.owner == Main.myPlayer);
            HandleRockets(Projectile.owner == Main.myPlayer);
        }

        const float ARROW_ARC = MathHelper.Pi * 0.66f;
        float ArrowFireCounter = 0;
        int ArrowSoundTimer = 0;
        float BulletFireCounter = 0;
        int BulletSoundTimer = 0;
        int ArrowAmmoCycle = 0;
        int BulletAmmoCycle = 0;
        float RocketFireCounter = 0;

        private void HandleArrows(bool actuallyFire)
        {
            Player owner = Main.player[Projectile.owner];
            int dmg = Projectile.damage;
            float time = 12 / (2 * owner.GetWeaponAttackSpeed(owner.HeldItem));
            float kb = Projectile.knockBack;
            float shootSpeed = owner.HeldItem.shootSpeed;
            Vector2 dir = Projectile.velocity.SafeNormalize(-Vector2.UnitY);
            Vector2 pos = Projectile.Center + dir * 64 + dir.RotatedByRandom(ARROW_ARC) * 48;
            float rot = dir.ToRotation();
            Vector2 velDir = rot.AngleTowards((Main.MouseWorld - pos).ToRotation(), MathHelper.PiOver4 * 0.5f).ToRotationVector2();
            if (ArrowFireCounter <= 0)
            {
                if (actuallyFire)
                {
                    int arrowType = CycleArrowAmmo(out Item ammoChosen);
                    int damageAdd = 0;
                    float kbAdd = 0;
                    float speedAdd = 0;
                    if (HelperMethodsSF2.CanItemBeShot(ammoChosen))
                    {
                        damageAdd = Math.Max(ammoChosen.damage, 15);
                        kbAdd = ammoChosen.knockBack;
                        speedAdd = ammoChosen.shootSpeed;
                        if (arrowType == ProjectileID.WoodenArrowFriendly)
                            arrowType = ModContent.ProjectileType<MeridianArrow>();
                    }
                    Vector2 vel = velDir * shootSpeed;
                    Projectile proj = Projectile.NewProjectileDirect(
                        owner.GetSource_FromThis(),
                        pos,
                        vel + dir * speedAdd,
                        arrowType,
                        dmg + 3 * damageAdd,
                        kb + kbAdd,
                        owner.whoAmI
                    );
                    if (proj.penetrate == 1 && proj.damage < Projectile.damage * 1.5f) proj.damage *= 2;
                    if (proj.penetrate > 0 && arrowType != ModContent.ProjectileType<MeridianArrow>())
                    {
                        proj.damage = Math.Max(proj.damage, (int)(proj.damage * 2f / proj.penetrate));
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    Vector2 dustDir = (MathHelper.TwoPi * i / 12f).ToRotationVector2();
                    Dust.NewDustPerfect(pos + dir * 6, DustID.ShimmerSpark, dustDir * 0.3f);
                }
                ArrowFireCounter += time;
                if (ArrowSoundTimer <= 0)
                {
                    SoundEngine.PlaySound(ARROW_SHOOT_SOUND, Projectile.position);
                    ArrowSoundTimer = 6;
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 arcDir = (ARROW_ARC * (i - 18) / 36f + dir.ToRotation()).ToRotationVector2();
                        Dust.NewDustPerfect(Projectile.Center + dir * 64 + arcDir * 48, DustID.ShimmerSpark, dir * 0.3f);
                    }
                }
                ArrowSoundTimer--;
            }
            ArrowFireCounter--;
        }

        bool BulletAlternator = false;

        private void HandleBullets(bool actuallyFire)
        {
            if (FrameCounter == 1f && actuallyFire)
            {
                // If for some reason the beam velocity can't be correctly normalized, set it to a default value.
                Vector2 beamVelocity = Vector2.Normalize(Projectile.velocity);
                if (beamVelocity.HasNaNs())
                {
                    beamVelocity = -Vector2.UnitY;
                }

                // This UUID will be the same between all players in multiplayer, ensuring that the beams are properly anchored on the Prism on everyone's screen.
                int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI);

                int damage = Projectile.damage;
                float knockback = Projectile.knockBack;
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    beamVelocity,
                    ModContent.ProjectileType<MeridianBullet>(),
                    damage,
                    knockback,
                    Projectile.owner,
                    uuid);

                // After creating the beams, mark the Prism as having an important network event. This will make Terraria sync its data to other players ASAP.
                Projectile.netUpdate = true;
            }

            Vector2 dir = Projectile.velocity.SafeNormalize(-Vector2.UnitY);
            for (int i = 0; i < 2; i++)
            {
                float theta = 0.1f * FrameCounter + i * MathHelper.Pi;
                Vector2 arcDir = new Vector2((float)Math.Cos(theta) * 4, (float)Math.Sin(theta) * 32).RotatedBy(dir.ToRotation());
                Dust.NewDustPerfect(Projectile.Center + dir * 64 + arcDir, DustID.ShimmerSpark, dir * 0.3f);
            }
            if (BulletFireCounter <= 0)
            {
                Player owner = Main.player[Projectile.owner];
                int dmg = Projectile.damage;
                float time = 9 / (2 * owner.GetWeaponAttackSpeed(owner.HeldItem));
                float kb = Projectile.knockBack;
                float shootSpeed = owner.HeldItem.shootSpeed;
                BulletFireCounter += time;
                if (BulletSoundTimer <= 2)
                {
                    SoundEngine.PlaySound(BULLET_SHOOT_SOUND, Projectile.position);
                    BulletSoundTimer += 5;
                }
                BulletSoundTimer--;
                if (CycleBulletAmmo(out Item chosenAmmoItem) && actuallyFire)
                {
                    Vector2 vel = dir * shootSpeed;
                    float theta = 0.1f * FrameCounter + (BulletAlternator ? MathHelper.Pi : 0);
                    Vector2 arcDir = new Vector2((float)Math.Cos(theta) * 4, (float)Math.Sin(theta) * 32).RotatedBy(dir.ToRotation());
                    Projectile.NewProjectile(
                        owner.GetSource_FromThis(),
                        Projectile.Center + dir * 64 + arcDir,
                        vel + dir * chosenAmmoItem.shootSpeed,
                        chosenAmmoItem.shoot,
                        dmg + 2 * chosenAmmoItem.damage,
                        kb + chosenAmmoItem.knockBack,
                        owner.whoAmI
                    );
                    BulletAlternator = !BulletAlternator;
                }
            }
            BulletFireCounter--;
        }


        private void HandleRockets(bool actuallyFire)
        {
            Player owner = Main.player[Projectile.owner];
            int dmg = (int)(Projectile.damage * 1.5f);
            float time = 90 / owner.GetWeaponAttackSpeed(owner.HeldItem);
            float kb = Projectile.knockBack;
            float shootSpeed = owner.HeldItem.shootSpeed;
            Vector2 dir = Projectile.velocity.SafeNormalize(-Vector2.UnitY);
            Vector2 pos = Projectile.Center + dir * 64;
            if (RocketFireCounter <= -20)
            {
                if (actuallyFire)
                {
                    Projectile.NewProjectile(
                        owner.GetSource_FromThis(),
                        pos,
                        dir * shootSpeed,
                        ModContent.ProjectileType<MeridianRocket>(),
                        dmg,
                        kb,
                        owner.whoAmI
                    );
                }
                RocketFireCounter += time;
                SoundEngine.PlaySound(ROCKET_SHOOT_SOUND, Projectile.position);
            }
            RocketFireCounter--;
        }

        private void UpdateAnimation()
        {
            // If necessary, change which specific frame of the animation is displayed.
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % MAX_FRAMES;
            }
            Projectile.frameCounter++;
        }

        private void UpdateAim(Vector2 source, float speed)
        {
            // Get the player's current aiming direction as a normalized vector.
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = Projectile.velocity.SafeNormalize(-Vector2.UnitY);
            }

            // Change a portion of the Prism's current velocity so that it points to the mouse. This gives smooth movement over time.
            //aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, AimResponsiveness));
            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            float rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction;

            Vector2 holdoutOffset = new Vector2(-6, 12);
            holdoutOffset.Y += -0.5f * (float)Math.Sin(FrameCounter * 0.4f + MathHelper.Pi);
            rotation += 0.04f * (float)Math.Sin(FrameCounter * 0.2f);

            // Place the Prism directly into the player's hand at all times.
            Projectile.Center = playerHandPos + new Vector2(holdoutOffset.X * Projectile.direction, holdoutOffset.Y).RotatedBy(Projectile.rotation);
            Projectile.rotation = rotation;

            // The Prism is a holdout Projectile, so change the player's variables to reflect that.
            // Constantly resetting player.itemTime and player.itemAnimation prevents the player from switching items or doing anything else.
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // If you do not multiply by Projectile.direction, the player's hand will point the wrong direction while facing left.
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        // Because the Prism is a holdout Projectile and stays glued to its user, it needs custom drawcode.
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            Texture2D texture = texAsset.Value;
            int frameHeight = texture.Height / MAX_FRAMES;
            int spriteSheetOffset = frameHeight * Projectile.frame;
            Vector2 sheetInsertPosition = (Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();

            // The Prism is always at full brightness, regardless of the surrounding light. This is equivalent to it being its own glowmask.
            // It is drawn in a non-white color to distinguish it from the vanilla Last Prism.
            Color drawColor = Color.White;
            Main.EntitySpriteDraw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight - 2)), drawColor, Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, effects, 0f);
            return false;
        }
        
        
        private int CycleArrowAmmo(out Item chosenAmmoItem)
        {
            int toRet = ModContent.ProjectileType<MeridianArrow>();
            if (!Projectile.TryGetOwner(out Player owner))
            {
                chosenAmmoItem = null;
                return toRet;
            }
            List<Item> ammoIDs = [];
            foreach (Item ammo in owner.inventory)
            {
                if (!HelperMethodsSF2.CanItemBeShot(ammo) || !HelperMethodsSF2.IsArrow(ammo.ammo))
                {
                    continue;
                }
                ammoIDs.Add(ammo);
            }
            if (ammoIDs.Count > 0)
            {
                int index = ArrowAmmoCycle % (ammoIDs.Count + 1);
                if (index == 0) {
                    chosenAmmoItem = null;
                    ArrowAmmoCycle++;
                    return toRet;
                }
                index--;
                chosenAmmoItem = ammoIDs[index];
                toRet = chosenAmmoItem.shoot;
                ArrowAmmoCycle++;
            }
            else
            {
                chosenAmmoItem = null;
            }
            return toRet;
        }

        private bool CycleBulletAmmo(out Item chosenAmmoItem)
        {
            if (!Projectile.TryGetOwner(out Player owner))
            {
                chosenAmmoItem = null;
                return false;
            }
            List<Item> ammoIDs = [];
            foreach (Item ammo in owner.inventory)
            {
                if (!HelperMethodsSF2.CanItemBeShot(ammo) || !HelperMethodsSF2.IsBullet(ammo.ammo))
                {
                    continue;
                }
                ammoIDs.Add(ammo);
            }
            if (ammoIDs.Count > 0)
            {
                chosenAmmoItem = ammoIDs[BulletAmmoCycle % ammoIDs.Count];
                BulletAmmoCycle++;
                return true;
            }
            chosenAmmoItem = null;
            return false;
        }
    }   
}