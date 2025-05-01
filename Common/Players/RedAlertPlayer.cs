using SuperfluityTwo.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SuperfluityTwo.Common.Players
{
    public class RedAlertPlayer : ModPlayer {
        public bool hasRedAlert = false;
        public bool visibleRedAlert = false;
        public bool forceVisibleRedAlert = false;
        public float alertRotation = 0;
        private int ticksRedAlertEquipped = 0;
        public bool RedAlertEquipped = false;
        public int graceTime = 5;

        public bool visibleMayday = false;
        public bool forceVisibleMayday = false;
        public override void ResetEffects()
        {
            forceVisibleRedAlert = false;
            hasRedAlert = false;
            forceVisibleMayday = false;
        }

        public override void UpdateEquips()
        {
            if (hasRedAlert)
                ticksRedAlertEquipped++;
            else
                ticksRedAlertEquipped = 0;
            RedAlertEquipped = hasRedAlert;
            if (ticksRedAlertEquipped == 1 && graceTime <= 0)
                Player.AddBuff(ModContent.BuffType<Alert>(), 25 * 60);
            alertRotation += 0.15f;
            if (graceTime > 0)
                graceTime--;
        }

        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (Player == Main.LocalPlayer && hasRedAlert && !Player.HasBuff(ModContent.BuffType<Alert>()))
            {
                Player.AddBuff(ModContent.BuffType<Alert>(), 25 * 60);
                Player.SetImmuneTimeForAllTypes(Player.longInvince ? 90 : 60);
                bool useSound = forceVisibleMayday || forceVisibleRedAlert || visibleMayday || visibleRedAlert;
                if (useSound) {
                    bool useMaydaySound = forceVisibleMayday || (visibleMayday && !forceVisibleRedAlert);
                    string soundID = useMaydaySound ? "DEFCONAlarm" : "KlaxonAlarm";
                    SoundStyle alarmSound = new SoundStyle($"{nameof(SuperfluityTwo)}/Assets/Sounds/" + soundID)
                        {
                            Volume = 1.45f,
                            PitchVariance = 0.005f
                        };
                        SoundEngine.PlaySound(alarmSound);
                }
                return true;
            } 

            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }
    }
}