using System;
using System.IO;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.Content.Items.Weapons.Ranged.Meridian
{
    public class MeridianBullet : ModProjectile
    {
        // How much more damage the beams do when the Prism is fully charged. Damage smoothly scales up to this multiplier.
        private const float MaxDamageMultiplier = 1.5f;

        // Beams increase their scale from 0 to this value as the Prism charges up.
        private const float MaxBeamScale = 1.8f;

        // The maximum possible range of the beam. Don't set this too high or it will cause significant lag.
        private const float MaxBeamLength = 2400f;

        // The width of the beam in pixels for the purposes of tile collision.
        // This should generally be left at 1, otherwise the beam tends to stop early when touching tiles.
        private const float BeamTileCollisionWidth = 1f;

        // The width of the beam in pixels for the purposes of entity hitbox collision.
        // This gets scaled with the beam's scale value, so as the beam visually grows its hitbox gets wider as well.
        private const float BeamHitboxCollisionWidth = 22f;

        // The number of sample points to use when performing a collision hitscan for the beam.
        // More points theoretically leads to a higher quality result, but can cause more lag. 3 tends to be enough.
        private const int NumSamplePoints = 3;

        // How quickly the beam adjusts to sudden changes in length.
        // Every frame, the beam replaces this ratio of its current length with its intended length.
        // Generally you shouldn't need to change this.
        // Setting it too low will make the beam lazily pass through walls before being blocked by them.
        private const float BeamLengthChangeFactor = 0.75f;

        // The charge percentage required on the host prism for the beam to begin visual effects (e.g. impact dust).
        private const float VisualEffectThreshold = 0.1f;

        // Each Last Prism beam draws two lasers separately: an inner beam and an outer beam. This controls their opacity.
        private const float OuterBeamOpacityMultiplier = 0.75f;

        // The maximum brightness of the light emitted by the beams. Brightness scales from 0 to this value as the Prism's charge increases.
        private const float BeamLightBrightness = 0.75f;

        // These variables control the beam's potential coloration.
        // As a value, hue ranges from 0f to 1f, both of which are pure red. The laser beams vary from 0.57 to 0.75, which winds up being a blue-to-purple gradient.
        // Saturation ranges from 0f to 1f and controls how greyed out the color is. 0 is fully grayscale, 1 is vibrant, intense color.
        // Lightness ranges from 0f to 1f and controls how dark or light the color is. 0 is pitch black. 1 is pure white.
        private const float BeamColorHue = 0.57f;
        private const float BeamColorSaturation = 0.66f;
        private const float BeamColorLightness = 0.53f;

        private const int DamageStart = 30;
        private const int MaxCharge = 120;

        public ref float HostPrismIndex => ref Projectile.ai[0];

        // This property encloses the internal AI variable Projectile.localAI[1].
        // Normally, localAI is not synced over the network. This beam manually syncs this variable using SendExtraAI and ReceiveExtraAI.
        private ref float BeamLength => ref Projectile.localAI[1];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            //Projectile.extraUpdates = 3;
            //Projectile.aiStyle = ProjAIStyleID.ThickLaser;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }


        // Send beam length over the network to prevent hitbox-affecting and thus cascading desyncs in multiplayer.
        public override void SendExtraAI(BinaryWriter writer) => writer.Write(BeamLength);
        public override void ReceiveExtraAI(BinaryReader reader) => BeamLength = reader.ReadSingle();

        public override void AI()
        {
            // If something has gone wrong with either the beam or the host Prism, destroy the beam.
            Projectile hostPrism = Main.projectile[(int)HostPrismIndex];
            if (Projectile.type != ModContent.ProjectileType<MeridianBullet>() || !hostPrism.active || hostPrism.type != ModContent.ProjectileType<MeridianHoldout>())
            {
                Projectile.Kill();
                return;
            }
            if (hostPrism.TryGetOwner(out Player owner) && owner.channel)
                Projectile.timeLeft = 2;

            // Grab some variables from the host Prism.
                Vector2 hostPrismDir = Vector2.Normalize(hostPrism.velocity);
            float chargeRatio = MathHelper.Clamp(hostPrism.ai[0] / MaxCharge, 0f, 1f);

            // Update the beam's damage every frame based on charge and the host Prism's damage.
            Projectile.damage = (int)(hostPrism.damage * GetDamageMultiplier(chargeRatio));

            // The beam cannot strike enemies until the host Prism is at a certain charge level.
            Projectile.friendly = hostPrism.ai[0] > DamageStart;

            // Variables scale smoothly while the host Prism is charging up.
            if (chargeRatio < 1f)
            {
                Projectile.scale = MathHelper.Lerp(0f, MaxBeamScale, chargeRatio);

                // For the first 2/3 of charge time, the opacity scales up from 0% to 40%.
                // Spin rate increases slowly during this time.
                if (chargeRatio <= 0.66f)
                {
                    float phaseRatio = chargeRatio * 1.5f;
                    Projectile.Opacity = MathHelper.Lerp(0f, 0.4f, phaseRatio);
                }

                // For the last 1/3 of charge time, the opacity scales up from 40% to 100%.
                // Spin rate increases dramatically during this time.
                else
                {
                    float phaseRatio = (chargeRatio - 0.66f) * 3f;
                    Projectile.Opacity = MathHelper.Lerp(0.4f, 1f, phaseRatio);
                }
            }

            // If the host Prism is already at max charge, don't calculate anything. Just use the max values.
            else
            {
                Projectile.scale = MaxBeamScale;
                Projectile.Opacity = 1f;
            }

            // This trigonometry calculates where the beam is supposed to be pointing.
            Vector2 unitRot = Vector2.UnitY;
            Vector2 yVec = new Vector2(4f, 0);
            float hostPrismAngle = hostPrism.velocity.ToRotation();
            Vector2 beamSpanVector = (unitRot * yVec).RotatedBy(hostPrismAngle);

            // Calculate the beam's emanating position. Start with the Prism's center.
            Projectile.Center = hostPrism.Center;
            // Add a fixed offset to align with the Prism's sprite sheet.
            Projectile.position += hostPrismDir * 16f + new Vector2(0f, -hostPrism.gfxOffY);
            // Add the forwards offset, measured in pixels.
            Projectile.position += hostPrismDir * 1;
            // Add the sideways offset vector, which is calculated for the current angle of the beam and scales with the beam's sideways offset.
            Projectile.position += beamSpanVector;

            // Set the beam's velocity to point towards its current spread direction and sanity check it. It should have magnitude 1.
            Projectile.velocity = hostPrismDir;
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Update the beam's length by performing a hitscan collision check.
            float hitscanBeamLength = PerformBeamHitscan(hostPrism, chargeRatio >= 1f);
            BeamLength = MathHelper.Lerp(BeamLength, hitscanBeamLength, BeamLengthChangeFactor);

            // This Vector2 stores the beam's hitbox statistics. X = beam length. Y = beam width.
            Vector2 beamDims = new Vector2(Projectile.velocity.Length() * BeamLength, Projectile.width * Projectile.scale);

            // Only produce dust and cause water ripples if the beam is above a certain charge level.
            Color beamColor = GetOuterBeamColor();
            if (chargeRatio >= VisualEffectThreshold)
            {
                ProduceBeamDust(beamColor);

                // If the game is rendering (i.e. isn't a dedicated server), make the beam disturb water.
                if (Main.netMode != NetmodeID.Server)
                {
                    ProduceWaterRipples(beamDims);
                }
            }

            // Make the beam cast light along its length. The brightness of the light scales with the charge.
            // v3_1 is an unnamed decompiled variable which is the color of the light cast by DelegateMethods.CastLight.
            DelegateMethods.v3_1 = beamColor.ToVector3() * BeamLightBrightness * chargeRatio;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * BeamLength, beamDims.Y, new Utils.TileActionAttempt(DelegateMethods.CastLight));
        }

        // Uses a simple polynomial (x^3) to get sudden but smooth damage increase near the end of the charge-up period.
        private float GetDamageMultiplier(float chargeRatio)
        {
            float f = chargeRatio * chargeRatio * chargeRatio;
            return MathHelper.Lerp(1f, MaxDamageMultiplier, f);
        }

        private float PerformBeamHitscan(Projectile prism, bool fullCharge)
        {
            // By default, the hitscan interpolation starts at the Projectile's center.
            // If the host Prism is fully charged, the interpolation starts at the Prism's center instead.
            Vector2 samplingPoint = Projectile.Center;
            if (fullCharge)
            {
                samplingPoint = prism.Center;
            }

            // Overriding that, if the player shoves the Prism into or through a wall, the interpolation starts at the player's center.
            // This last part prevents the player from projecting beams through walls under any circumstances.
            Player player = Main.player[Projectile.owner];
            if (!Collision.CanHitLine(player.Center, 0, 0, prism.Center, 0, 0))
            {
                samplingPoint = player.Center;
            }

            // Perform a laser scan to calculate the correct length of the beam.
            // Alternatively, if you want the beam to ignore tiles, just set it to be the max beam length with the following line.
            // return MaxBeamLength;
            float[] laserScanResults = new float[NumSamplePoints];
            Collision.LaserScan(samplingPoint, Projectile.velocity, 0 * Projectile.scale, MaxBeamLength, laserScanResults);
            float averageLengthSample = 0f;
            for (int i = 0; i < laserScanResults.Length; ++i)
            {
                averageLengthSample += laserScanResults[i];
            }
            averageLengthSample /= NumSamplePoints;

            return averageLengthSample;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // If the beam doesn't have a defined direction, don't draw anything.
            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centerFloored = Projectile.Center.Floor() + Projectile.velocity * Projectile.scale * 10.5f;
            Vector2 drawScale = new Vector2(Projectile.scale);

            // Reduce the beam length proportional to its square area to reduce block penetration.
            float visualBeamLength = BeamLength - 14.5f * Projectile.scale * Projectile.scale;

            DelegateMethods.f_1 = 1f; // f_1 is an unnamed decompiled variable whose function is unknown. Leave it at 1.
            Vector2 startPosition = centerFloored - Main.screenPosition;
            Vector2 endPosition = startPosition + Projectile.velocity * visualBeamLength;

            // Draw the outer beam.
            DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, GetOuterBeamColor() * OuterBeamOpacityMultiplier * Projectile.Opacity);

            // Returning false prevents Terraria from trying to draw the Projectile itself.
            return false;
        }

        private void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
        {
            Utils.LaserLineFraming lineFraming = new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw);

            // c_1 is an unnamed decompiled variable which is the render color of the beam drawn by DelegateMethods.RainbowLaserDraw.
            DelegateMethods.c_1 = beamColor;
            Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
        }

        private void ProduceBeamDust(Color beamColor)
        {
            // Create one dust per frame a small distance from where the beam ends.
            const int type = 15;
            Vector2 endPosition = Projectile.Center + Projectile.velocity * (BeamLength - 14.5f * Projectile.scale);

            // Main.rand.NextBool is used to give a 50/50 chance for the angle to point to the left or right.
            // This gives the dust a 50/50 chance to fly off on either side of the beam.
            float angle = Projectile.rotation + (Main.rand.NextBool() ? 1f : -1f) * MathHelper.PiOver2;
            float startDistance = Main.rand.NextFloat(1f, 1.8f);
            float scale = Main.rand.NextFloat(0.7f, 1.1f);
            Vector2 velocity = angle.ToRotationVector2() * startDistance;
            Dust dust = Dust.NewDustDirect(endPosition, 0, 0, type, velocity.X, velocity.Y, 0, beamColor, scale);
            dust.color = beamColor;
            dust.noGravity = true;

            // If the beam is currently large, make the dust faster and larger to match.
            if (Projectile.scale > 1f)
            {
                dust.velocity *= Projectile.scale;
                dust.scale *= Projectile.scale;
            }
        }

        private Color GetOuterBeamColor()
        {
            // This hue calculation produces a unique color for each beam based on its Beam ID.
            float hue = BeamColorHue;

            // Main.hslToRgb converts Hue, Saturation, Lightness into a Color for general purpose use.
            Color c = Main.hslToRgb(hue, BeamColorSaturation, BeamColorLightness);

            // Manually reduce the opacity of the color so beams can overlap without completely overwriting each other.
            c.A = 64;
            return c;
        }

        private void ProduceWaterRipples(Vector2 beamDims)
        {
            WaterShaderData shaderData = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();

            // A universal time-based sinusoid which updates extremely rapidly. GlobalTime is 0 to 3600, measured in seconds.
            float waveSine = 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
            Vector2 ripplePos = Projectile.position + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(Projectile.rotation);

            // WaveData is encoded as a Color. Not really sure why.
            Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
            shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, Projectile.rotation);
        }
        
        // Determines whether the specified target hitbox is intersecting with the beam.
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// If the target is touching the beam's hitbox (which is a small rectangle vaguely overlapping the host Prism), that's good enough.
			if (projHitbox.Intersects(targetHitbox)) {
				return true;
			}

			// Otherwise, perform an AABB line collision check to check the whole beam.
			float _ = float.NaN;
			Vector2 beamEndPos = Projectile.Center + Projectile.velocity * BeamLength;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, BeamHitboxCollisionWidth * Projectile.scale, ref _);
		}
        
        // Automatically iterates through every tile the laser is overlapping to cut grass at all those locations.
        public override void CutTiles()
        {
            // tilecut_0 is an unnamed decompiled variable which tells CutTiles how the tiles are being cut (in this case, via a Projectile).
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.TileActionAttempt cut = new Utils.TileActionAttempt(DelegateMethods.CutTiles);
            Vector2 beamStartPos = Projectile.Center;
            Vector2 beamEndPos = beamStartPos + Projectile.velocity * BeamLength;

            // PlotTileLine is a function which performs the specified action to all tiles along a drawn line, with a specified width.
            // In this case, it is cutting all tiles which can be destroyed by Projectiles, for example grass or pots.
            Utils.PlotTileLine(beamStartPos, beamEndPos, Projectile.width * Projectile.scale, cut);
        }
    }   
}