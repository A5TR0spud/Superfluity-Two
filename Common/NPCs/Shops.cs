using SuperfluityTwo.Common;
using SuperfluityTwo.Content.Items.Weapons.Magic;
using SuperfluityTwo.Content.Items.Weapons.Magic.FairyKnife;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluityTwo.NPCs
{
	public class Shops : GlobalNPC
	{
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Dryad, "Shop"))
            {
                shop.Add(new Item(ItemID.AnkletoftheWind) {
					shopCustomPrice = Item.buyPrice(gold: 6)
				}, Condition.InJungle, Condition.MoonPhasesEven);

                shop.Add(ItemID.Daybloom, Condition.TimeDay);
                shop.Add(ItemID.Moonglow, Condition.TimeNight);
                shop.Add(ItemID.Blinkroot, Condition.MoonPhases37);
                shop.Add(ItemID.Deathweed, Condition.BloodMoon);
                shop.Add(ItemID.Waterleaf, Condition.InRain);
                shop.Add(ItemID.Fireblossom, Condition.TimeDay);
                shop.Add(ItemID.Shiverthorn, Condition.MoonPhases26);
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.ArmsDealer, "Shop"))
            {
                shop.InsertAfter(shop.GetEntry(ItemID.MusketBall), new Item(ItemID.MeteorShot), Condition.DownedEowOrBoc);
                shop.InsertAfter(shop.GetEntry(ItemID.SilverBullet), new Item(ItemID.CrystalBullet), Condition.DownedQueenSlime);
                shop.InsertAfter(shop.GetEntry(ItemID.SilverBullet), new Item(ItemID.ChlorophyteBullet), Condition.DownedPlantera);
                shop.InsertAfter(shop.GetEntry(ItemID.SilverBullet), new Item(ItemID.MoonlordBullet), Condition.DownedMoonLord);
                
                shop.InsertAfter(shop.GetEntry(ItemID.FlintlockPistol), new Item(ItemID.Handgun), Condition.DownedSkeletron);
                shop.InsertAfter(shop.GetEntry(ItemID.Minishark), new Item(ItemID.ClockworkAssaultRifle), Condition.Hardmode);
                shop.InsertAfter(shop.GetEntry(ItemID.Minishark), new Item(ItemID.Gatligator), Condition.Hardmode, Condition.MoonPhasesNearNew, Condition.NightOrEclipse);
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Merchant, "Shop"))
            {
                shop.Add(ItemID.RecallPotion);
                shop.Add(ItemID.WormholePotion);
                shop.Add(new Item(ItemID.PotionOfReturn) {
					shopCustomPrice = Item.buyPrice(gold: 1),
				});
                shop.Add(new Item(ItemID.TeleportationPotion) {
					shopCustomPrice = Item.buyPrice(gold: 1),
				});
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.BestiaryGirl, "Shop"))
            {
                shop.Add(new Item(ItemID.CreativeWings)
                {
                    shopCustomPrice = Item.buyPrice(gold: 30)
                }, Condition.BestiaryFilledPercent(30));

                shop.InsertAfter(
                    shop.GetEntry(ItemID.BlandWhip),
                    new Item(ModContent.ItemType<FairyKnife>()),
                    SF2Conditions.EncounteredAnyFairy
                );

                //shop.Add(ItemID.FairyBell, Condition.NpcIsPresent(NPCID.Wizard), Condition.DownedTwins);
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Clothier, "Shop"))
            {
                shop.Add(ItemID.Silk, Condition.MoonPhasesEven);
                shop.Add(ItemID.Leather, Condition.MoonPhasesOdd);
                shop.Add(new Item(ItemID.GoldenKey) {
					shopCustomPrice = Item.buyPrice(gold: 5),
				}, Condition.DownedSkeletron);
                shop.Add(new Item(ItemID.ShadowKey) {
					shopCustomPrice = Item.buyPrice(gold: 15),
				}, Condition.DownedSkeletron);
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.GoblinTinkerer, "Shop"))
            {
                shop.Add(ItemID.GoldBar, Condition.InBelowSurface);
                shop.Add(new Item(ItemID.PlatinumWatch) {
					shopCustomPrice = Item.buyPrice(gold: 6),
				}, Condition.TimeNight);
                shop.Add(new Item(ItemID.DepthMeter) {
					shopCustomPrice = Item.buyPrice(gold: 6),
				}, Condition.MoonPhasesOdd);
                shop.Add(new Item(ItemID.Compass) {
					shopCustomPrice = Item.buyPrice(gold: 6),
				}, Condition.MoonPhasesEven);
                shop.Add(new Item(ItemID.Radar) {
					shopCustomPrice = Item.buyPrice(gold: 6),
				}, Condition.MoonPhaseFull);
                shop.Add(new Item(ItemID.TallyCounter) {
					shopCustomPrice = Item.buyPrice(gold: 10),
				}, Condition.NpcIsPresent(NPCID.Mechanic));
                shop.Add(new Item(ItemID.LifeformAnalyzer) {
					shopCustomPrice = Item.buyPrice(gold: 10),
				}, Condition.EclipseOrBloodMoon);
                shop.Add(new Item(ItemID.MetalDetector) {
					shopCustomPrice = Item.buyPrice(gold: 15),
				}, Condition.MoonPhasesNearNew);
                shop.Add(new Item(ItemID.WeatherRadio) {
					shopCustomPrice = Item.buyPrice(gold: 6),
				}, Condition.InRain, Condition.NpcIsPresent(NPCID.Angler));
                shop.Add(new Item(ItemID.DPSMeter) {
					shopCustomPrice = Item.buyPrice(gold: 6),
				}, Condition.NpcIsPresent(NPCID.Mechanic));
                shop.Add(new Item(ItemID.Stopwatch) {
					shopCustomPrice = Item.buyPrice(gold: 6),
				}, Condition.MoonPhasesOddQuarters);
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Wizard, "Shop"))
            {
                shop.Add(ItemID.MagicMirror);
                shop.Add(ItemID.IceMirror, Condition.InSnow);
                shop.Add(ItemID.MagicConch, Condition.InRain);
                shop.Add(ItemID.DemonConch, Condition.EclipseOrBloodMoon);
                shop.Add(ItemID.FallenStar, Condition.TimeNight);
                shop.Add(ItemID.StarCloak, Condition.InHallow, Condition.TimeNight);
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.PartyGirl, "Shop"))
            {
                shop.Add(ItemID.SliceOfCake, Condition.BirthdayParty);
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Painter, "Shop"))
            {
                shop.Add(ItemID.PainterPaintballGun, Condition.NpcIsPresent(NPCID.ArmsDealer));
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Princess, "Shop"))
            {
                shop.Add(ItemID.OcularResonance, Condition.NpcIsPresent(NPCID.Wizard));
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Stylist, "Shop"))
            {
                shop.Add(ItemID.StylistKilLaKillScissorsIWish);
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Mechanic, "Shop"))
            {
                shop.Add(ItemID.CombatWrench, Condition.NpcIsPresent(NPCID.GoblinTinkerer));
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Demolitionist, "Shop"))
            {
                shop.InsertAfter(shop.GetEntry(ItemID.Dynamite), new Item(ItemID.ScarabBomb), Condition.InDesert);
                shop.Add(ItemID.MiningPotion);
                shop.Add(new Item(ItemID.SpelunkerPotion) {
					shopCustomPrice = Item.buyPrice(gold: 1),
				});
                shop.Add(new Item(ItemID.Ale) {
					shopCustomPrice = Item.buyPrice(silver: 3),
				}, Condition.NpcIsPresent(NPCID.DD2Bartender));
                shop.Add(ItemID.PlatinumBar, Condition.InBelowSurface);
                shop.Add(ItemID.Hellstone, Condition.InBelowSurface, Condition.DownedEowOrBoc, Condition.EclipseOrBloodMoon);
                shop.Add(ItemID.PalladiumBar, Condition.InBelowSurface, Condition.Hardmode);
                shop.Add(ItemID.MythrilBar, Condition.InBelowSurface, Condition.DownedMechBossAny);
                shop.Add(ItemID.ChlorophyteOre, Condition.InBelowSurface, Condition.InJungle, Condition.DownedMechBossAll);
                return;
            }
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Truffle, "Shop"))
            {
                shop.Add(ItemID.GlowingMushroom);
                return;
            }
        }
    }
}