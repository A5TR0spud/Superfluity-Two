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

            //Panic Necklace
            Recipe.Create(ItemID.PanicNecklace)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient(ItemID.LifeCrystal)
                .DisableDecraft()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

			/*//ExtendoGrip
            Recipe.Create(ItemID.ExtendoGrip)
                .AddIngredient(ItemID.ZombieArm)
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .DisableDecraft()
                .AddTile(TileID.Anvils)
                .Register();*/

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

            /*//HoneyComb
            Recipe.Create(ItemID.HoneyComb)
                .AddIngredient(ItemID.BeeWax, 10)
				.AddIngredient(ItemID.Hive, 5)
                .DisableDecraft()
                .AddTile(TileID.WorkBenches)
                .Register();*/

            //Leather
            Recipe.Create(ItemID.Leather)
                .AddIngredient(ItemID.Vertebrae, 5)
                .DisableDecraft()
                .AddTile(TileID.WorkBenches)
                .Register();

            /*//WaterWalkingBoots
            Recipe.Create(ItemID.WaterWalkingBoots)
                .AddIngredient(ItemID.Leather, 5)
                .AddIngredient(ItemID.WaterWalkingPotion, 3)
                .DisableDecraft()
                .AddTile(TileID.WorkBenches)
                .Register();

            //HermesBoots
            Recipe.Create(ItemID.HermesBoots)
                .AddIngredient(ItemID.Leather, 5)
                .AddIngredient(ItemID.SwiftnessPotion, 3)
                .DisableDecraft()
                .AddTile(TileID.WorkBenches)
                .Register();*/

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
                .AddIngredient(ItemID.SnowBlock, 50)
                .DisableDecraft()
                .Register();

            //MagicQuiver
            Recipe.Create(ItemID.MagicQuiver)
                .AddIngredient(ItemID.WoodenArrow, 3996)
                .AddTile(TileID.CrystalBall)
                .DisableDecraft()
                .Register();

            /*//NimbusRod
            Recipe.Create(ItemID.NimbusRod)
                .AddIngredient(ItemID.RainCloud, 100)
                .AddTile(TileID.CrystalBall)
                .DisableDecraft()
                .Register();*/

            /*//FrozenTurtleShell
            Recipe.Create(ItemID.FrozenTurtleShell)
                .AddIngredient(ItemID.TurtleShell)
                .AddIngredient(ItemID.IceBlock, 100)
                .AddTile(TileID.IceMachine)
                .DisableDecraft()
                .Register();*/

            //PutridScent
            Recipe.Create(ItemID.PutridScent)
                .AddIngredient(ItemID.FleshKnuckles)
                .AddIngredient(ItemID.ManaCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //FleshKnuckles
            Recipe.Create(ItemID.FleshKnuckles)
                .AddIngredient(ItemID.PutridScent)
                .AddIngredient(ItemID.LifeCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //WormHook
            Recipe.Create(ItemID.WormHook)
                .AddIngredient(ItemID.TendonHook)
                .AddIngredient(ItemID.ManaCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();
                
            //TendonHook
            Recipe.Create(ItemID.TendonHook)
                .AddIngredient(ItemID.WormHook)
                .AddIngredient(ItemID.LifeCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //ChainGuillotines
            Recipe.Create(ItemID.ChainGuillotines)
                .AddIngredient(ItemID.FetidBaghnakhs)
                .AddIngredient(ItemID.ManaCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //FetidBaghnakhs
            Recipe.Create(ItemID.FetidBaghnakhs)
                .AddIngredient(ItemID.ChainGuillotines)
                .AddIngredient(ItemID.LifeCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //ClingerStaff
            Recipe.Create(ItemID.ClingerStaff)
                .AddIngredient(ItemID.SoulDrain)
                .AddIngredient(ItemID.ManaCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //SoulDrain
            Recipe.Create(ItemID.SoulDrain)
                .AddIngredient(ItemID.ClingerStaff)
                .AddIngredient(ItemID.LifeCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //PutridScent
            Recipe.Create(ItemID.PutridScent)
                .AddIngredient(ItemID.FleshKnuckles)
                .AddIngredient(ItemID.ManaCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //FleshKnuckles
            Recipe.Create(ItemID.FleshKnuckles)
                .AddIngredient(ItemID.PutridScent)
                .AddIngredient(ItemID.LifeCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //DartRifle
            Recipe.Create(ItemID.DartRifle)
                .AddIngredient(ItemID.DartPistol)
                .AddIngredient(ItemID.ManaCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();

            //DartPistol
            Recipe.Create(ItemID.DartPistol)
                .AddIngredient(ItemID.DartRifle)
                .AddIngredient(ItemID.LifeCrystal)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.TinkerersWorkbench)
                .DisableDecraft()
                .Register();
            

            int[] mimicMootShimCraft = {ItemID.DualHook, ItemID.StarCloak, ItemID.MagicDagger, ItemID.PhilosophersStone, ItemID.TitanGlove, ItemID.CrossNecklace};
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.DualHook, ItemID.Hook);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.StarCloak, ItemID.FallenStar);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.MagicDagger, ItemID.ThrowingKnife);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.PhilosophersStone, ItemID.LifeCrystal);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.TitanGlove, ItemID.FeralClaws);
            RegisterMutualShimmerCraft(mimicMootShimCraft, ItemID.CrossNecklace, ItemID.Chain);
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
    }
}