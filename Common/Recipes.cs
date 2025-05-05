using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperfluousSummoning.Content
{
	public class Recipes : ModSystem
	{
        public override void AddRecipes()
        {
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


            RegisterMutualLifeManaCraft(ItemID.Musket, ItemID.TheUndertaker);
            RegisterMutualLifeManaCraft(ItemID.ShadowOrb, ItemID.CrimsonHeart);
            RegisterMutualLifeManaCraft(ItemID.Vilethorn, ItemID.CrimsonRod);
            RegisterMutualLifeManaCraft(ItemID.BallOHurt, ItemID.TheRottedFork);

            RegisterMutualLifeManaCraft(ItemID.ClingerStaff, ItemID.SoulDrain);
            RegisterMutualLifeManaCraft(ItemID.DartRifle, ItemID.DartPistol);
            RegisterMutualLifeManaCraft(ItemID.ChainGuillotines, ItemID.FetidBaghnakhs);
            RegisterMutualLifeManaCraft(ItemID.PutridScent, ItemID.FleshKnuckles);
            RegisterMutualLifeManaCraft(ItemID.WormHook, ItemID.TendonHook);

            int[] corruptMimicMootShimCraft = [ItemID.ClingerStaff, ItemID.DartRifle, ItemID.ChainGuillotines, ItemID.PutridScent, ItemID.WormHook];
            RegisterMutualShimmerCraft(corruptMimicMootShimCraft, ItemID.ClingerStaff, ItemID.ManaCrystal);
            RegisterMutualShimmerCraft(corruptMimicMootShimCraft, ItemID.DartRifle, ItemID.Blowgun);
            RegisterMutualShimmerCraft(corruptMimicMootShimCraft, ItemID.ChainGuillotines, ItemID.Chain);
            RegisterMutualShimmerCraft(corruptMimicMootShimCraft, ItemID.PutridScent, ItemID.JungleRose);
            RegisterMutualShimmerCraft(corruptMimicMootShimCraft, ItemID.WormHook, ItemID.Hook);

            int[] crimsonMimicMootShimCraft = [ItemID.SoulDrain, ItemID.DartPistol, ItemID.FetidBaghnakhs, ItemID.FleshKnuckles, ItemID.TendonHook];
            RegisterMutualShimmerCraft(crimsonMimicMootShimCraft, ItemID.SoulDrain, ItemID.ManaCrystal);
            RegisterMutualShimmerCraft(crimsonMimicMootShimCraft, ItemID.DartPistol, ItemID.Blowgun);
            RegisterMutualShimmerCraft(crimsonMimicMootShimCraft, ItemID.FetidBaghnakhs, ItemID.FeralClaws);
            RegisterMutualShimmerCraft(crimsonMimicMootShimCraft, ItemID.FleshKnuckles, ItemID.LifeCrystal);
            RegisterMutualShimmerCraft(crimsonMimicMootShimCraft, ItemID.TendonHook, ItemID.Hook);

            int[] hallowMimicMootShimCraft = [ItemID.DaedalusStormbow, ItemID.FlyingKnife, ItemID.CrystalVileShard, ItemID.IlluminantHook];
            RegisterMutualShimmerCraft(hallowMimicMootShimCraft, ItemID.DaedalusStormbow, ItemID.PearlwoodBow);
            RegisterMutualShimmerCraft(hallowMimicMootShimCraft, ItemID.FlyingKnife, ItemID.ThrowingKnife);
            RegisterMutualShimmerCraft(hallowMimicMootShimCraft, ItemID.CrystalVileShard, ItemID.Vilethorn);
            RegisterMutualShimmerCraft(hallowMimicMootShimCraft, ItemID.IlluminantHook, ItemID.Hook);

            int[] mimicMootShimCraft = [ItemID.DualHook, ItemID.StarCloak, ItemID.MagicDagger, ItemID.PhilosophersStone, ItemID.TitanGlove, ItemID.CrossNecklace];
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.DualHook, ItemID.Hook);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.StarCloak, ItemID.FallenStar);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.MagicDagger, ItemID.ThrowingKnife);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.PhilosophersStone, ItemID.LifeCrystal);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.TitanGlove, ItemID.FeralClaws);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.CrossNecklace, ItemID.Chain);

            int[] iceMimicMootShimCraft = [ItemID.Frostbrand, ItemID.IceBow, ItemID.FlowerofFrost];
            RegisterMutualShimmerCraft(iceMimicMootShimCraft, ItemID.Frostbrand, ItemID.IceBlade);
            RegisterMutualShimmerCraft(iceMimicMootShimCraft, ItemID.IceBow, ItemID.BorealWoodBow);
            RegisterMutualShimmerCraft(iceMimicMootShimCraft, ItemID.FlowerofFrost, ItemID.FlowerofFire);

            RegisterShimmerCraft(ItemID.NaturesGift, ItemID.JungleRose, ItemID.LifeCrystal);
            RegisterShimmerCraft(ItemID.JungleRose, ItemID.NaturesGift, ItemID.ManaCrystal);
        }

        private void RegisterMutualLifeManaCraft(int CorruptItem, int CrimsonItem) {
            Recipe.Create(CorruptItem)
                .AddIngredient(CrimsonItem)
                .AddIngredient(ItemID.ManaCrystal)
                .AddTile(TileID.TinkerersWorkbench)
                .AddCondition(Condition.InGraveyard)
                .DisableDecraft()
                .Register();
            Recipe.Create(CrimsonItem)
                .AddIngredient(CorruptItem)
                .AddIngredient(ItemID.LifeCrystal)
                .AddTile(TileID.TinkerersWorkbench)
                .AddCondition(Condition.InGraveyard)
                .DisableDecraft()
                .Register();
        }

        private void RegisterMutualShimmerCraft(int[] items, int resultItem, int itemID) {
            foreach (int i in items) {
                if (i == resultItem) continue;
                Recipe.Create(resultItem)
                    .AddIngredient(i)
                    .AddIngredient(itemID)
                    .AddCondition(Condition.NearShimmer)
                    .DisableDecraft()
                    .Register();
            }
        }

        private void RegisterShimmerCraft(int inItem, int outItem, int transformItem = -1) {
            Recipe r = Recipe.Create(outItem)
                    .AddIngredient(inItem);
            if (transformItem != -1) {
                r.AddIngredient(transformItem);
            }
            r.AddCondition(Condition.NearShimmer)
                .DisableDecraft()
                .Register();
        }
    }
}