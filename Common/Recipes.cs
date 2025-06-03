using SuperfluityTwo.Content.Items.Accessories;
using SuperfluityTwo.Content.Items.Accessories.Mage;
using SuperfluityTwo.Content.Items.Accessories.Summoner;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluousSummoning.Content
{
	public class Recipes : ModSystem
	{
        public override void AddRecipes()
        {
            RegisterQoLCrafts();
            RegisterTransmuteCrafts();
        }

        private static void RegisterQoLCrafts() {
            //Band of Starpower
            Recipe.Create(ItemID.BandofStarpower)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient(ItemID.ManaCrystal, 4)
                .DisableDecraft()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            
            //Band of Regen
            Recipe.Create(ItemID.BandofRegeneration)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient(ItemID.LifeCrystal)
                .DisableDecraft()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            //Panic Necklace
            Recipe.Create(ItemID.PanicNecklace)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient(ItemID.LifeCrystal)
                .DisableDecraft()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

			//PiranhaGun
            Recipe.Create(ItemID.PiranhaGun)
                .AddIngredient(ItemID.Harpoon)
				.AddIngredient(ItemID.JungleKey)
				.AddIngredient(ItemID.Ectoplasm)
                .DisableDecraft()
                .AddTile(TileID.Anvils)
                .Register();
			
			//ScourgeoftheCorruptor
            Recipe.Create(ItemID.ScourgeoftheCorruptor)
                .AddIngredient(ItemID.DarkLance)
				.AddIngredient(ItemID.CorruptionKey)
				.AddIngredient(ItemID.Ectoplasm)
                .DisableDecraft()
                .AddTile(TileID.Anvils)
                .Register();

			//VampireKnives
            Recipe.Create(ItemID.VampireKnives)
                .AddIngredient(ItemID.MagicDagger)
				.AddIngredient(ItemID.CrimsonKey)
				.AddIngredient(ItemID.Ectoplasm)
                .DisableDecraft()
                .AddTile(TileID.Anvils)
                .Register();

			//RainbowGun
            Recipe.Create(ItemID.RainbowGun)
                .AddIngredient(ItemID.NimbusRod)
				.AddIngredient(ItemID.HallowedKey)
				.AddIngredient(ItemID.Ectoplasm)
                .DisableDecraft()
                .AddTile(TileID.Anvils)
                .Register();

			//StaffoftheFrostHydra
            Recipe.Create(ItemID.StaffoftheFrostHydra)
                .AddIngredient(ItemID.IceRod)
				.AddIngredient(ItemID.FrozenKey)
				.AddIngredient(ItemID.Ectoplasm)
                .DisableDecraft()
                .AddTile(TileID.Anvils)
                .Register();

			//desert tiger staff
            Recipe.Create(ItemID.StormTigerStaff)
                .AddIngredient(ItemID.AntlionClaw) //mandible blade
				.AddIngredient(ItemID.DungeonDesertKey)
				.AddIngredient(ItemID.Ectoplasm)
                .DisableDecraft()
                .AddTile(TileID.Anvils)
                .Register();

            //HoneyComb
            Recipe.Create(ItemID.HoneyComb)
                .AddIngredient(ItemID.BeeWax, 10)
				.AddIngredient(ItemID.Hive, 5)
                .DisableDecraft()
                .AddTile(TileID.WorkBenches)
                .Register();

            //Leather
            Recipe.Create(ItemID.Leather)
                .AddIngredient(ItemID.Vertebrae, 5)
                .DisableDecraft()
                .AddTile(TileID.WorkBenches)
                .Register();

            //IceSkates
            Recipe.Create(ItemID.IceSkates)
                .AddIngredient(ItemID.Leather, 5)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .DisableDecraft()
                .AddTile(TileID.IceMachine)
                .Register();

            //IceMachine
            Recipe.Create(ItemID.IceMachine)
                .AddIngredient(ItemID.IceChest)
                .AddIngredient(ItemID.IceBlock, 100)
                .DisableDecraft()
                .Register();

            //MagicQuiver
            Recipe.Create(ItemID.MagicQuiver)
                .AddIngredient(ItemID.WoodenArrow, 3996)
                .AddTile(TileID.CrystalBall)
                .DisableDecraft()
                .Register();

            //Finch Staff
            Recipe.Create(ItemID.BabyBirdStaff)
                .AddRecipeGroup(RecipeGroupID.Wood, 50)
                .AddRecipeGroup(RecipeGroupID.Birds)
                .AddTile(TileID.LivingLoom)
                .DisableDecraft()
                .Register();
        }

        private static void RegisterTransmuteCrafts()
        {
            RegisterMutualLifeManaCraft(ItemID.Musket, ItemID.TheUndertaker);
            RegisterMutualLifeManaCraft(ItemID.ShadowOrb, ItemID.CrimsonHeart);
            RegisterMutualLifeManaCraft(ItemID.Vilethorn, ItemID.CrimsonRod);
            RegisterMutualLifeManaCraft(ItemID.BallOHurt, ItemID.TheRottedFork);

            RegisterMutualLifeManaCraft(ItemID.ClingerStaff, ItemID.SoulDrain);
            RegisterMutualLifeManaCraft(ItemID.DartRifle, ItemID.DartPistol);
            RegisterMutualLifeManaCraft(ItemID.ChainGuillotines, ItemID.FetidBaghnakhs);
            RegisterMutualLifeManaCraft(ItemID.PutridScent, ItemID.FleshKnuckles);
            RegisterMutualLifeManaCraft(ItemID.WormHook, ItemID.TendonHook);

            RegisterMutualShimmerCraftList([
                [ItemID.ClingerStaff, ItemID.ManaCrystal],
                [ItemID.DartRifle, ItemID.Blowgun],
                [ItemID.ChainGuillotines, ItemID.Chain],
                [ItemID.PutridScent, ItemID.JungleRose],
                [ItemID.WormHook, ItemID.Hook]
            ]);

            RegisterMutualShimmerCraftList([
                [ItemID.SoulDrain, ItemID.ManaCrystal],
                [ItemID.DartPistol, ItemID.Blowgun],
                [ItemID.FetidBaghnakhs, ItemID.FeralClaws],
                [ItemID.FleshKnuckles, ItemID.LifeCrystal],
                [ItemID.TendonHook, ItemID.Hook]
            ]);

            RegisterMutualShimmerCraftList([
                [ItemID.DaedalusStormbow, ItemID.PearlwoodBow],
                [ItemID.FlyingKnife, ItemID.ThrowingKnife],
                [ItemID.CrystalVileShard, ItemID.Vilethorn],
                [ItemID.IlluminantHook, ItemID.Hook]
            ]);

            RegisterMutualShimmerCraftList([
                [ItemID.DualHook, ItemID.Hook],
                [ItemID.StarCloak, ItemID.FallenStar],
                [ItemID.MagicDagger, ItemID.ThrowingKnife],
                [ItemID.PhilosophersStone, ItemID.LifeCrystal],
                [ItemID.TitanGlove, ItemID.FeralClaws],
                [ItemID.CrossNecklace, ItemID.Chain]
            ]);

            RegisterMutualShimmerCraftList([
                [ItemID.Frostbrand, ItemID.IceBlade],
                [ItemID.IceBow, ItemID.BorealWoodBow],
                [ItemID.FlowerofFrost, ItemID.FlowerofFire]
            ]);

            RegisterMutualLifeManaCraft(ItemID.NaturesGift, ItemID.JungleRose);
            RegisterMutualLightDarkCraft(ItemID.DarkShard, ItemID.LightShard, 10);

            RegisterMutualShimmerCraftList([
                ItemID.NimbusRod,
                ModContent.ItemType<ThunderScroll>()
            ]);


            RegisterMutualShimmerCraftList([
                ItemID.SpectreStaff,
                ModContent.ItemType<Zygoma>()
            ]);

            RegisterMutualShimmerCraftList([
                ItemID.InfernoFork,
                ModContent.ItemType<CultivatingFlame>()
            ]);

            RegisterMutualShimmerCraftList([
                ItemID.ShadowbeamStaff,
                ModContent.ItemType<Phylactery>()
            ]);
        }

        private static void RegisterMutualLifeManaCraft(int CorruptOrManaItem, int CrimsonOrLifeItem) {
            Recipe.Create(CorruptOrManaItem)
                .AddIngredient(CrimsonOrLifeItem)
                .AddIngredient(ItemID.ManaCrystal)
                .AddTile(TileID.TinkerersWorkbench)
                .AddCondition(Condition.InGraveyard)
                .DisableDecraft()
                .Register();
            Recipe.Create(CrimsonOrLifeItem)
                .AddIngredient(CorruptOrManaItem)
                .AddIngredient(ItemID.LifeCrystal)
                .AddTile(TileID.TinkerersWorkbench)
                .AddCondition(Condition.InGraveyard)
                .DisableDecraft()
                .Register();
        }

        private static void RegisterMutualLightDarkCraft(int DarkItem, int LightItem, int SoulCount = 1) {
            RegisterShimmerCraft(DarkItem, LightItem, ItemID.SoulofLight, SoulCount);
            RegisterShimmerCraft(LightItem, DarkItem, ItemID.SoulofNight, SoulCount);
        }

        private static void RegisterMutualShimmerCraftList(int[][] itemsAndKeys) {
            int resultItem;
            int transformerItemID;
            int inItem;
            foreach (int[] keyPair in itemsAndKeys) {
                resultItem = keyPair[0];
                transformerItemID = keyPair[1];
                foreach (int[] inKeyPair in itemsAndKeys) {
                    inItem = inKeyPair[0];
                    if (inItem == resultItem) continue;
                    RegisterShimmerCraft(inItem, resultItem, transformerItemID);
                }
            }
        }
        
        private static void RegisterMutualShimmerCraftList(int[] items) {
            foreach (int resultItem in items) {
                foreach (int inItem in items) {
                    if (inItem == resultItem) continue;
                    RegisterShimmerCraft(inItem, resultItem);
                }
            }
        }

        private static void RegisterShimmerCraft(int inItem, int outItem, int transformItem = -1, int transformItemCount = 1)
        {
            Recipe r = Recipe.Create(outItem)
                .AddIngredient(inItem);
            if (transformItem != -1)
            {
                r.AddIngredient(transformItem, transformItemCount);
            }
            r.AddTile(TileID.ShimmerMonolith)//.AddCondition(Condition.NearShimmer)
                .DisableDecraft()
                .Register();
        }
    }
}