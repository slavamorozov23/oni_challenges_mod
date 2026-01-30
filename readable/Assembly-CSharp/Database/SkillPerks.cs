using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x02000FAF RID: 4015
	public class SkillPerks : ResourceSet<SkillPerk>
	{
		// Token: 0x06007E3B RID: 32315 RVA: 0x00322AAC File Offset: 0x00320CAC
		public SkillPerks(ResourceSet parent) : base("SkillPerks", parent)
		{
			this.IncreaseDigSpeedSmall = base.Add(new SkillAttributePerk("IncreaseDigSpeedSmall", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MINER.NAME, false));
			this.IncreaseDigSpeedMedium = base.Add(new SkillAttributePerk("IncreaseDigSpeedMedium", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MINER.NAME, false));
			this.IncreaseDigSpeedLarge = base.Add(new SkillAttributePerk("IncreaseDigSpeedLarge", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MINER.NAME, false));
			this.CanDigVeryFirm = base.Add(new SimpleSkillPerk("CanDigVeryFirm", UI.ROLES_SCREEN.PERKS.CAN_DIG_VERY_FIRM.DESCRIPTION));
			this.CanDigNearlyImpenetrable = base.Add(new SimpleSkillPerk("CanDigAbyssalite", UI.ROLES_SCREEN.PERKS.CAN_DIG_NEARLY_IMPENETRABLE.DESCRIPTION));
			this.CanDigSuperDuperHard = base.Add(new SimpleSkillPerk("CanDigDiamondAndObsidan", UI.ROLES_SCREEN.PERKS.CAN_DIG_SUPER_SUPER_HARD.DESCRIPTION));
			this.CanDigRadioactiveMaterials = base.Add(new SimpleSkillPerk("CanDigCorium", UI.ROLES_SCREEN.PERKS.CAN_DIG_RADIOACTIVE_MATERIALS.DESCRIPTION));
			this.CanDigUnobtanium = base.Add(new SimpleSkillPerk("CanDigUnobtanium", UI.ROLES_SCREEN.PERKS.CAN_DIG_UNOBTANIUM.DESCRIPTION));
			this.IncreaseConstructionSmall = base.Add(new SkillAttributePerk("IncreaseConstructionSmall", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME, false));
			this.IncreaseConstructionMedium = base.Add(new SkillAttributePerk("IncreaseConstructionMedium", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.BUILDER.NAME, false));
			this.IncreaseConstructionLarge = base.Add(new SkillAttributePerk("IncreaseConstructionLarge", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_BUILDER.NAME, false));
			this.IncreaseConstructionMechatronics = base.Add(new SkillAttributePerk("IncreaseConstructionMechatronics", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME, false));
			this.CanDemolish = base.Add(new SimpleSkillPerk("CanDemonlish", UI.ROLES_SCREEN.PERKS.CAN_DEMOLISH.DESCRIPTION));
			this.IncreaseLearningSmall = base.Add(new SkillAttributePerk("IncreaseLearningSmall", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME, false));
			this.IncreaseLearningMedium = base.Add(new SkillAttributePerk("IncreaseLearningMedium", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.RESEARCHER.NAME, false));
			this.IncreaseLearningLarge = base.Add(new SkillAttributePerk("IncreaseLearningLarge", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, false));
			this.IncreaseLearningLargeSpace = base.Add(new SkillAttributePerk("IncreaseLearningLargeSpace", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SPACE_RESEARCHER.NAME, false));
			this.IncreaseBotanySmall = base.Add(new SkillAttributePerk("IncreaseBotanySmall", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_FARMER.NAME, false));
			this.IncreaseBotanyMedium = base.Add(new SkillAttributePerk("IncreaseBotanyMedium", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.FARMER.NAME, false));
			this.IncreaseBotanyLarge = base.Add(new SkillAttributePerk("IncreaseBotanyLarge", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_FARMER.NAME, false));
			this.CanFarmTinker = base.Add(new SimpleSkillPerk("CanFarmTinker", UI.ROLES_SCREEN.PERKS.CAN_FARM_TINKER.DESCRIPTION));
			this.CanIdentifyMutantSeeds = base.Add(new SimpleSkillPerk("CanIdentifyMutantSeeds", UI.ROLES_SCREEN.PERKS.CAN_IDENTIFY_MUTANT_SEEDS.DESCRIPTION));
			this.CanFarmStation = base.Add(new SimpleSkillPerk("CanFarmStation", UI.ROLES_SCREEN.PERKS.CAN_FARM_STATION.DESCRIPTION));
			this.CanSalvagePlantFiber = base.Add(new SimpleSkillPerk("CanSalvagePlantFiber", UI.ROLES_SCREEN.PERKS.CAN_SALVAGE_PLANT_FIBER.DESCRIPTION));
			this.IncreaseRanchingSmall = base.Add(new SkillAttributePerk("IncreaseRanchingSmall", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.RANCHER.NAME, false));
			this.IncreaseRanchingMedium = base.Add(new SkillAttributePerk("IncreaseRanchingMedium", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SENIOR_RANCHER.NAME, false));
			this.CanWrangleCreatures = base.Add(new SimpleSkillPerk("CanWrangleCreatures", UI.ROLES_SCREEN.PERKS.CAN_WRANGLE_CREATURES.DESCRIPTION));
			this.CanUseRanchStation = base.Add(new SimpleSkillPerk("CanUseRanchStation", UI.ROLES_SCREEN.PERKS.CAN_USE_RANCH_STATION.DESCRIPTION));
			this.CanUseMilkingStation = base.Add(new SimpleSkillPerk("CanUseMilkingStation", UI.ROLES_SCREEN.PERKS.CAN_USE_MILKING_STATION.DESCRIPTION));
			this.IncreaseAthleticsSmall = base.Add(new SkillAttributePerk("IncreaseAthleticsSmall", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME, false));
			this.IncreaseAthleticsMedium = base.Add(new SkillAttributePerk("IncreaseAthletics", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SUIT_EXPERT.NAME, false));
			this.IncreaseAthleticsLarge = base.Add(new SkillAttributePerk("IncreaseAthleticsLarge", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, false));
			this.IncreaseStrengthGofer = base.Add(new SkillAttributePerk("IncreaseStrengthGofer", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME, false));
			this.IncreaseStrengthCourier = base.Add(new SkillAttributePerk("IncreaseStrengthCourier", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, false));
			this.IncreaseStrengthGroundskeeper = base.Add(new SkillAttributePerk("IncreaseStrengthGroundskeeper", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HANDYMAN.NAME, false));
			this.IncreaseStrengthPlumber = base.Add(new SkillAttributePerk("IncreaseStrengthPlumber", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.PLUMBER.NAME, false));
			this.IncreaseCarryAmountSmall = base.Add(new SkillAttributePerk("IncreaseCarryAmountSmall", Db.Get().Attributes.CarryAmount.Id, 400f, DUPLICANTS.ROLES.HAULER.NAME, false));
			this.IncreaseCarryAmountMedium = base.Add(new SkillAttributePerk("IncreaseCarryAmountMedium", Db.Get().Attributes.CarryAmount.Id, 800f, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, false));
			this.IncreaseArtSmall = base.Add(new SkillAttributePerk("IncreaseArtSmall", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME, false));
			this.IncreaseArtMedium = base.Add(new SkillAttributePerk("IncreaseArt", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.ARTIST.NAME, false));
			this.IncreaseArtLarge = base.Add(new SkillAttributePerk("IncreaseArtLarge", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MASTER_ARTIST.NAME, false));
			this.CanArt = base.Add(new SimpleSkillPerk("CanArt", UI.ROLES_SCREEN.PERKS.CAN_ART.DESCRIPTION));
			this.CanArtUgly = base.Add(new SimpleSkillPerk("CanArtUgly", UI.ROLES_SCREEN.PERKS.CAN_ART_UGLY.DESCRIPTION));
			this.CanArtOkay = base.Add(new SimpleSkillPerk("CanArtOkay", UI.ROLES_SCREEN.PERKS.CAN_ART_OKAY.DESCRIPTION));
			this.CanArtGreat = base.Add(new SimpleSkillPerk("CanArtGreat", UI.ROLES_SCREEN.PERKS.CAN_ART_GREAT.DESCRIPTION));
			this.CanStudyArtifact = base.Add(new SimpleSkillPerk("CanStudyArtifact", UI.ROLES_SCREEN.PERKS.CAN_STUDY_ARTIFACTS.DESCRIPTION));
			this.CanClothingAlteration = base.Add(new SimpleSkillPerk("CanClothingAlteration", UI.ROLES_SCREEN.PERKS.CAN_CLOTHING_ALTERATION.DESCRIPTION));
			this.IncreaseMachinerySmall = base.Add(new SkillAttributePerk("IncreaseMachinerySmall", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME, false));
			this.IncreaseMachineryMedium = base.Add(new SkillAttributePerk("IncreaseMachineryMedium", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME, false));
			this.IncreaseMachineryLarge = base.Add(new SkillAttributePerk("IncreaseMachineryLarge", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME, false));
			this.ConveyorBuild = base.Add(new SimpleSkillPerk("ConveyorBuild", UI.ROLES_SCREEN.PERKS.CONVEYOR_BUILD.DESCRIPTION));
			this.CanPowerTinker = base.Add(new SimpleSkillPerk("CanPowerTinker", UI.ROLES_SCREEN.PERKS.CAN_POWER_TINKER.DESCRIPTION));
			this.CanMakeMissiles = base.Add(new SimpleSkillPerk("CanMakeMissiles", UI.ROLES_SCREEN.PERKS.CAN_MAKE_MISSILES.DESCRIPTION));
			this.CanCraftElectronics = base.Add(new SimpleSkillPerk("CanCraftElectronics", UI.ROLES_SCREEN.PERKS.CAN_CRAFT_ELECTRONICS.DESCRIPTION, DlcManager.DLC3));
			this.CanElectricGrill = base.Add(new SimpleSkillPerk("CanElectricGrill", UI.ROLES_SCREEN.PERKS.CAN_ELECTRIC_GRILL.DESCRIPTION));
			this.CanGasRange = base.Add(new SimpleSkillPerk("CanGasRange", UI.ROLES_SCREEN.PERKS.CAN_GAS_RANGE.DESCRIPTION));
			this.CanDeepFry = base.Add(new SimpleSkillPerk("CanDeepFry", UI.ROLES_SCREEN.PERKS.CAN_DEEP_FRYER.DESCRIPTION));
			this.IncreaseCookingSmall = base.Add(new SkillAttributePerk("IncreaseCookingSmall", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_COOK.NAME, false));
			this.IncreaseCookingMedium = base.Add(new SkillAttributePerk("IncreaseCookingMedium", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.COOK.NAME, false));
			this.CanSpiceGrinder = base.Add(new SimpleSkillPerk("CanSpiceGrinder ", UI.ROLES_SCREEN.PERKS.CAN_SPICE_GRINDER.DESCRIPTION));
			this.IncreaseCaringSmall = base.Add(new SkillAttributePerk("IncreaseCaringSmall", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME, false));
			this.IncreaseCaringMedium = base.Add(new SkillAttributePerk("IncreaseCaringMedium", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MEDIC.NAME, false));
			this.IncreaseCaringLarge = base.Add(new SkillAttributePerk("IncreaseCaringLarge", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MEDIC.NAME, false));
			this.CanCompound = base.Add(new SimpleSkillPerk("CanCompound", UI.ROLES_SCREEN.PERKS.CAN_COMPOUND.DESCRIPTION));
			this.CanDoctor = base.Add(new SimpleSkillPerk("CanDoctor", UI.ROLES_SCREEN.PERKS.CAN_DOCTOR.DESCRIPTION));
			this.CanAdvancedMedicine = base.Add(new SimpleSkillPerk("CanAdvancedMedicine", UI.ROLES_SCREEN.PERKS.CAN_ADVANCED_MEDICINE.DESCRIPTION));
			this.ExosuitExpertise = base.Add(new SimpleSkillPerk("ExosuitExpertise", UI.ROLES_SCREEN.PERKS.EXOSUIT_EXPERTISE.DESCRIPTION));
			this.ExosuitDurability = base.Add(new SimpleSkillPerk("ExosuitDurability", UI.ROLES_SCREEN.PERKS.EXOSUIT_DURABILITY.DESCRIPTION));
			this.AllowAdvancedResearch = base.Add(new SimpleSkillPerk("AllowAdvancedResearch", UI.ROLES_SCREEN.PERKS.ADVANCED_RESEARCH.DESCRIPTION));
			this.AllowInterstellarResearch = base.Add(new SimpleSkillPerk("AllowInterStellarResearch", UI.ROLES_SCREEN.PERKS.INTERSTELLAR_RESEARCH.DESCRIPTION));
			this.AllowNuclearResearch = base.Add(new SimpleSkillPerk("AllowNuclearResearch", UI.ROLES_SCREEN.PERKS.NUCLEAR_RESEARCH.DESCRIPTION));
			this.AllowOrbitalResearch = base.Add(new SimpleSkillPerk("AllowOrbitalResearch", UI.ROLES_SCREEN.PERKS.ORBITAL_RESEARCH.DESCRIPTION));
			this.AllowGeyserTuning = base.Add(new SimpleSkillPerk("AllowGeyserTuning", UI.ROLES_SCREEN.PERKS.GEYSER_TUNING.DESCRIPTION));
			this.AllowChemistry = base.Add(new SimpleSkillPerk("AllowChemistry", UI.ROLES_SCREEN.PERKS.CHEMISTRY.DESCRIPTION));
			this.CanStudyWorldObjects = base.Add(new SimpleSkillPerk("CanStudyWorldObjects", UI.ROLES_SCREEN.PERKS.CAN_STUDY_WORLD_OBJECTS.DESCRIPTION));
			this.CanUseClusterTelescope = base.Add(new SimpleSkillPerk("CanUseClusterTelescope", UI.ROLES_SCREEN.PERKS.CAN_USE_CLUSTER_TELESCOPE.DESCRIPTION));
			this.CanUseClusterTelescopeEnclosed = base.Add(new SimpleSkillPerk("CanUseClusterTelescopeEnclosed", UI.ROLES_SCREEN.PERKS.CAN_CLUSTERTELESCOPEENCLOSED.DESCRIPTION));
			this.CanDoPlumbing = base.Add(new SimpleSkillPerk("CanDoPlumbing", UI.ROLES_SCREEN.PERKS.CAN_DO_PLUMBING.DESCRIPTION));
			this.CanUseRockets = base.Add(new SimpleSkillPerk("CanUseRockets", UI.ROLES_SCREEN.PERKS.CAN_USE_ROCKETS.DESCRIPTION));
			this.FasterSpaceFlight = base.Add(new SkillAttributePerk("FasterSpaceFlight", Db.Get().Attributes.SpaceNavigation.Id, 0.1f, DUPLICANTS.ROLES.ASTRONAUT.NAME, false));
			this.CanTrainToBeAstronaut = base.Add(new SimpleSkillPerk("CanTrainToBeAstronaut", UI.ROLES_SCREEN.PERKS.CAN_DO_ASTRONAUT_TRAINING.DESCRIPTION));
			this.CanMissionControl = base.Add(new SimpleSkillPerk("CanMissionControl", UI.ROLES_SCREEN.PERKS.CAN_MISSION_CONTROL.DESCRIPTION));
			this.CanUseRocketControlStation = base.Add(new SimpleSkillPerk("CanUseRocketControlStation", UI.ROLES_SCREEN.PERKS.CAN_PILOT_ROCKET.DESCRIPTION));
			this.IncreaseRocketSpeedSmall = base.Add(new SkillAttributePerk("IncreaseRocketSpeedSmall", Db.Get().Attributes.SpaceNavigation.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.ROCKETPILOT.NAME, false));
			if (DlcManager.IsContentSubscribed("DLC3_ID"))
			{
				this.IncreaseCarryAmountBionic = base.Add(new SkillAttributePerk("IncreaseCarryAmountBionic", Db.Get().Attributes.CarryAmount.Id, 600f, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, false));
				this.ExtraBionicBooster1 = base.Add(new SkillAttributePerk("ExtraBionicBooster1", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBooster2 = base.Add(new SkillAttributePerk("ExtraBionicBooster2", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBooster3 = base.Add(new SkillAttributePerk("ExtraBionicBooster3", Db.Get().Attributes.BionicBoosterSlots.Id, 2f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBooster4 = base.Add(new SkillAttributePerk("ExtraBionicBooster4", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBooster5 = base.Add(new SkillAttributePerk("ExtraBionicBooster5", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, "", false));
				this.ExtraBionicBooster6 = base.Add(new SkillAttributePerk("ExtraBionicBooster6", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBatteries = base.Add(new SkillAttributePerk("ExtraBionicBatteries", Db.Get().Attributes.BionicBatteryCountCapacity.Id, 2f, UI.ROLES_SCREEN.PERKS.EXTRA_BIONIC_BATTERIES.DESCRIPTION, false));
				this.BionicEardrumsDefense = base.Add(new ImmunitySkillPerk("BionicEardrumsDefense", "PoppedEarDrums"));
				this.BionicMinorEyeIrritationDefense = base.Add(new ImmunitySkillPerk("BionicMinorEyeIrritationDefense", "MinorIrritation"));
				this.BionicMajorEyeIrritationDefense = base.Add(new ImmunitySkillPerk("BionicMajorEyeIrritationDefense", "MajorIrritation"));
				this.BionicToastySurroundingsDefense = base.Add(new ImmunitySkillPerk("BionicToastySurroundingsDefense", "WarmAir"));
				this.BionicChillySurroundingsDefense = base.Add(new ImmunitySkillPerk("BionicChillySurroundingsDefense", "ColdAir"));
				this.ReducedBionicGunkProduction = base.Add(new SimpleSkillPerk("ReducedBionicGunkProduction", UI.ROLES_SCREEN.PERKS.REDUCED_GUNK_PRODUCTION.DESCRIPTION));
				this.EfficientBionicGears = base.Add(new SimpleSkillPerk("EfficientBionicGears", UI.ROLES_SCREEN.PERKS.EFFICIENT_BIONIC_GEARS.DESCRIPTION));
				this.IncreaseAthleticsBionicsC1 = base.Add(new SkillAttributePerk("IncreaseAthleticsBionicsC1", Db.Get().Attributes.Athletics.Id, 2f, DUPLICANTS.ROLES.BIONICS_C1.NAME, false));
				this.IncreaseAthleticsBionicsC2 = base.Add(new SkillAttributePerk("IncreaseAthleticsBionicsC2", Db.Get().Attributes.Athletics.Id, 2f, DUPLICANTS.ROLES.BIONICS_C2.NAME, false));
				this.IncreaseAthleticsBionicsB2 = base.Add(new SkillAttributePerk("IncreaseAthleticsBionicsB2", Db.Get().Attributes.Athletics.Id, 2f, DUPLICANTS.ROLES.BIONICS_B2.NAME, false));
				this.IncreaseAthleticsBionicsA2 = base.Add(new SkillAttributePerk("IncreaseAthleticsBionicsA2", Db.Get().Attributes.Athletics.Id, 2f, DUPLICANTS.ROLES.BIONICS_A2.NAME, false));
				this.IncreasedCarryBionics = base.Add(new SkillAttributePerk("IncreasedCarryBionics", Db.Get().Attributes.CarryAmount.Id, 400f, STRINGS.ITEMS.BIONIC_BOOSTERS.BOOSTER_CARRY1.NAME, true));
			}
		}

		// Token: 0x04005CA8 RID: 23720
		public SkillPerk IncreaseDigSpeedSmall;

		// Token: 0x04005CA9 RID: 23721
		public SkillPerk IncreaseDigSpeedMedium;

		// Token: 0x04005CAA RID: 23722
		public SkillPerk IncreaseDigSpeedLarge;

		// Token: 0x04005CAB RID: 23723
		public SkillPerk CanDigVeryFirm;

		// Token: 0x04005CAC RID: 23724
		public SkillPerk CanDigNearlyImpenetrable;

		// Token: 0x04005CAD RID: 23725
		public SkillPerk CanDigSuperDuperHard;

		// Token: 0x04005CAE RID: 23726
		public SkillPerk CanDigRadioactiveMaterials;

		// Token: 0x04005CAF RID: 23727
		public SkillPerk CanDigUnobtanium;

		// Token: 0x04005CB0 RID: 23728
		public SkillPerk IncreaseConstructionSmall;

		// Token: 0x04005CB1 RID: 23729
		public SkillPerk IncreaseConstructionMedium;

		// Token: 0x04005CB2 RID: 23730
		public SkillPerk IncreaseConstructionLarge;

		// Token: 0x04005CB3 RID: 23731
		public SkillPerk IncreaseConstructionMechatronics;

		// Token: 0x04005CB4 RID: 23732
		public SkillPerk CanDemolish;

		// Token: 0x04005CB5 RID: 23733
		public SkillPerk IncreaseLearningSmall;

		// Token: 0x04005CB6 RID: 23734
		public SkillPerk IncreaseLearningMedium;

		// Token: 0x04005CB7 RID: 23735
		public SkillPerk IncreaseLearningLarge;

		// Token: 0x04005CB8 RID: 23736
		public SkillPerk IncreaseLearningLargeSpace;

		// Token: 0x04005CB9 RID: 23737
		public SkillPerk IncreaseBotanySmall;

		// Token: 0x04005CBA RID: 23738
		public SkillPerk IncreaseBotanyMedium;

		// Token: 0x04005CBB RID: 23739
		public SkillPerk IncreaseBotanyLarge;

		// Token: 0x04005CBC RID: 23740
		public SkillPerk CanFarmTinker;

		// Token: 0x04005CBD RID: 23741
		public SkillPerk CanIdentifyMutantSeeds;

		// Token: 0x04005CBE RID: 23742
		public SkillPerk CanFarmStation;

		// Token: 0x04005CBF RID: 23743
		public SkillPerk CanSalvagePlantFiber;

		// Token: 0x04005CC0 RID: 23744
		public SkillPerk CanWrangleCreatures;

		// Token: 0x04005CC1 RID: 23745
		public SkillPerk CanUseRanchStation;

		// Token: 0x04005CC2 RID: 23746
		public SkillPerk CanUseMilkingStation;

		// Token: 0x04005CC3 RID: 23747
		public SkillPerk IncreaseRanchingSmall;

		// Token: 0x04005CC4 RID: 23748
		public SkillPerk IncreaseRanchingMedium;

		// Token: 0x04005CC5 RID: 23749
		public SkillPerk IncreaseAthleticsSmall;

		// Token: 0x04005CC6 RID: 23750
		public SkillPerk IncreaseAthleticsMedium;

		// Token: 0x04005CC7 RID: 23751
		public SkillPerk IncreaseAthleticsLarge;

		// Token: 0x04005CC8 RID: 23752
		public SkillPerk IncreaseStrengthSmall;

		// Token: 0x04005CC9 RID: 23753
		public SkillPerk IncreaseStrengthMedium;

		// Token: 0x04005CCA RID: 23754
		public SkillPerk IncreaseStrengthGofer;

		// Token: 0x04005CCB RID: 23755
		public SkillPerk IncreaseStrengthCourier;

		// Token: 0x04005CCC RID: 23756
		public SkillPerk IncreaseStrengthGroundskeeper;

		// Token: 0x04005CCD RID: 23757
		public SkillPerk IncreaseStrengthPlumber;

		// Token: 0x04005CCE RID: 23758
		public SkillPerk IncreaseCarryAmountSmall;

		// Token: 0x04005CCF RID: 23759
		public SkillPerk IncreaseCarryAmountMedium;

		// Token: 0x04005CD0 RID: 23760
		public SkillPerk IncreaseCarryAmountBionic;

		// Token: 0x04005CD1 RID: 23761
		public SkillPerk IncreaseArtSmall;

		// Token: 0x04005CD2 RID: 23762
		public SkillPerk IncreaseArtMedium;

		// Token: 0x04005CD3 RID: 23763
		public SkillPerk IncreaseArtLarge;

		// Token: 0x04005CD4 RID: 23764
		public SkillPerk CanArt;

		// Token: 0x04005CD5 RID: 23765
		public SkillPerk CanArtUgly;

		// Token: 0x04005CD6 RID: 23766
		public SkillPerk CanArtOkay;

		// Token: 0x04005CD7 RID: 23767
		public SkillPerk CanArtGreat;

		// Token: 0x04005CD8 RID: 23768
		public SkillPerk CanStudyArtifact;

		// Token: 0x04005CD9 RID: 23769
		public SkillPerk CanClothingAlteration;

		// Token: 0x04005CDA RID: 23770
		public SkillPerk IncreaseMachinerySmall;

		// Token: 0x04005CDB RID: 23771
		public SkillPerk IncreaseMachineryMedium;

		// Token: 0x04005CDC RID: 23772
		public SkillPerk IncreaseMachineryLarge;

		// Token: 0x04005CDD RID: 23773
		public SkillPerk ConveyorBuild;

		// Token: 0x04005CDE RID: 23774
		public SkillPerk CanMakeMissiles;

		// Token: 0x04005CDF RID: 23775
		public SkillPerk CanPowerTinker;

		// Token: 0x04005CE0 RID: 23776
		public SkillPerk CanCraftElectronics;

		// Token: 0x04005CE1 RID: 23777
		public SkillPerk CanElectricGrill;

		// Token: 0x04005CE2 RID: 23778
		public SkillPerk CanGasRange;

		// Token: 0x04005CE3 RID: 23779
		public SkillPerk CanDeepFry;

		// Token: 0x04005CE4 RID: 23780
		public SkillPerk IncreaseCookingSmall;

		// Token: 0x04005CE5 RID: 23781
		public SkillPerk IncreaseCookingMedium;

		// Token: 0x04005CE6 RID: 23782
		public SkillPerk CanSpiceGrinder;

		// Token: 0x04005CE7 RID: 23783
		public SkillPerk IncreaseCaringSmall;

		// Token: 0x04005CE8 RID: 23784
		public SkillPerk IncreaseCaringMedium;

		// Token: 0x04005CE9 RID: 23785
		public SkillPerk IncreaseCaringLarge;

		// Token: 0x04005CEA RID: 23786
		public SkillPerk CanCompound;

		// Token: 0x04005CEB RID: 23787
		public SkillPerk CanDoctor;

		// Token: 0x04005CEC RID: 23788
		public SkillPerk CanAdvancedMedicine;

		// Token: 0x04005CED RID: 23789
		public SkillPerk ExosuitExpertise;

		// Token: 0x04005CEE RID: 23790
		public SkillPerk ExosuitDurability;

		// Token: 0x04005CEF RID: 23791
		public SkillPerk AllowAdvancedResearch;

		// Token: 0x04005CF0 RID: 23792
		public SkillPerk AllowInterstellarResearch;

		// Token: 0x04005CF1 RID: 23793
		public SkillPerk AllowNuclearResearch;

		// Token: 0x04005CF2 RID: 23794
		public SkillPerk AllowOrbitalResearch;

		// Token: 0x04005CF3 RID: 23795
		public SkillPerk AllowGeyserTuning;

		// Token: 0x04005CF4 RID: 23796
		public SkillPerk AllowChemistry;

		// Token: 0x04005CF5 RID: 23797
		public SkillPerk CanStudyWorldObjects;

		// Token: 0x04005CF6 RID: 23798
		public SkillPerk CanUseClusterTelescope;

		// Token: 0x04005CF7 RID: 23799
		public SkillPerk CanUseClusterTelescopeEnclosed;

		// Token: 0x04005CF8 RID: 23800
		public SkillPerk IncreaseRocketSpeedSmall;

		// Token: 0x04005CF9 RID: 23801
		public SkillPerk CanMissionControl;

		// Token: 0x04005CFA RID: 23802
		public SkillPerk CanDoPlumbing;

		// Token: 0x04005CFB RID: 23803
		public SkillPerk CanUseRockets;

		// Token: 0x04005CFC RID: 23804
		public SkillPerk FasterSpaceFlight;

		// Token: 0x04005CFD RID: 23805
		public SkillPerk CanTrainToBeAstronaut;

		// Token: 0x04005CFE RID: 23806
		public SkillPerk CanUseRocketControlStation;

		// Token: 0x04005CFF RID: 23807
		public SkillPerk ExtraBionicBooster1;

		// Token: 0x04005D00 RID: 23808
		public SkillPerk ExtraBionicBooster2;

		// Token: 0x04005D01 RID: 23809
		public SkillPerk ExtraBionicBooster3;

		// Token: 0x04005D02 RID: 23810
		public SkillPerk ExtraBionicBooster4;

		// Token: 0x04005D03 RID: 23811
		public SkillPerk ExtraBionicBooster5;

		// Token: 0x04005D04 RID: 23812
		public SkillPerk ExtraBionicBooster6;

		// Token: 0x04005D05 RID: 23813
		public SkillPerk ReducedBionicGunkProduction;

		// Token: 0x04005D06 RID: 23814
		public SkillPerk EfficientBionicGears;

		// Token: 0x04005D07 RID: 23815
		public SkillPerk ExtraBionicBatteries;

		// Token: 0x04005D08 RID: 23816
		public SkillPerk BionicEardrumsDefense;

		// Token: 0x04005D09 RID: 23817
		public SkillPerk BionicMinorEyeIrritationDefense;

		// Token: 0x04005D0A RID: 23818
		public SkillPerk BionicMajorEyeIrritationDefense;

		// Token: 0x04005D0B RID: 23819
		public SkillPerk BionicChillySurroundingsDefense;

		// Token: 0x04005D0C RID: 23820
		public SkillPerk BionicToastySurroundingsDefense;

		// Token: 0x04005D0D RID: 23821
		public SkillPerk IncreaseAthleticsBionicsC1;

		// Token: 0x04005D0E RID: 23822
		public SkillPerk IncreaseAthleticsBionicsC2;

		// Token: 0x04005D0F RID: 23823
		public SkillPerk IncreaseAthleticsBionicsB2;

		// Token: 0x04005D10 RID: 23824
		public SkillPerk IncreaseAthleticsBionicsA2;

		// Token: 0x04005D11 RID: 23825
		public SkillPerk IncreasedCarryBionics;
	}
}
