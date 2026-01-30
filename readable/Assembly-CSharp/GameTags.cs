using System;
using System.Collections.Generic;
using System.Reflection;
using STRINGS;

// Token: 0x02000966 RID: 2406
public class GameTags
{
	// Token: 0x06004384 RID: 17284 RVA: 0x0018380C File Offset: 0x00181A0C
	public static Tag[] Reflection_GetTagsInClass(Type classAddress, BindingFlags variableFlags = BindingFlags.Static | BindingFlags.Public)
	{
		List<FieldInfo> list = new List<FieldInfo>(classAddress.GetFields(variableFlags)).FindAll((FieldInfo f) => f.FieldType == typeof(Tag));
		Tag[] array = new Tag[list.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = list[i].Name;
		}
		return array;
	}

	// Token: 0x04002CAE RID: 11438
	public static readonly Tag DeprecatedContent = TagManager.Create("DeprecatedContent");

	// Token: 0x04002CAF RID: 11439
	public static readonly Tag Any = TagManager.Create("Any");

	// Token: 0x04002CB0 RID: 11440
	public static readonly Tag SpawnsInWorld = TagManager.Create("SpawnsInWorld");

	// Token: 0x04002CB1 RID: 11441
	public static readonly Tag Experimental = TagManager.Create("Experimental");

	// Token: 0x04002CB2 RID: 11442
	public static readonly Tag Gravitas = TagManager.Create("Gravitas");

	// Token: 0x04002CB3 RID: 11443
	public static readonly Tag Miscellaneous = TagManager.Create("Miscellaneous");

	// Token: 0x04002CB4 RID: 11444
	public static readonly Tag Specimen = TagManager.Create("Specimen");

	// Token: 0x04002CB5 RID: 11445
	public static readonly Tag Seed = TagManager.Create("Seed");

	// Token: 0x04002CB6 RID: 11446
	public static readonly Tag Dehydrated = TagManager.Create("Dehydrated");

	// Token: 0x04002CB7 RID: 11447
	public static readonly Tag Rehydrated = TagManager.Create("Rehydrated");

	// Token: 0x04002CB8 RID: 11448
	public static readonly Tag Edible = TagManager.Create("Edible");

	// Token: 0x04002CB9 RID: 11449
	public static readonly Tag CookingIngredient = TagManager.Create("CookingIngredient");

	// Token: 0x04002CBA RID: 11450
	public static readonly Tag Medicine = TagManager.Create("Medicine");

	// Token: 0x04002CBB RID: 11451
	public static readonly Tag MedicalSupplies = TagManager.Create("MedicalSupplies");

	// Token: 0x04002CBC RID: 11452
	public static readonly Tag Plant = TagManager.Create("Plant");

	// Token: 0x04002CBD RID: 11453
	public static readonly Tag FibrousPlant = TagManager.Create("FibrousPlant");

	// Token: 0x04002CBE RID: 11454
	public static readonly Tag PlantBranch = TagManager.Create("PlantBranch");

	// Token: 0x04002CBF RID: 11455
	public static readonly Tag GrowingPlant = TagManager.Create("GrowingPlant");

	// Token: 0x04002CC0 RID: 11456
	public static readonly Tag FullyGrown = TagManager.Create("FullyGrown");

	// Token: 0x04002CC1 RID: 11457
	public static readonly Tag PlantedOnFloorVessel = TagManager.Create("PlantedOnFloorVessel");

	// Token: 0x04002CC2 RID: 11458
	public static readonly Tag Pickupable = TagManager.Create("Pickupable");

	// Token: 0x04002CC3 RID: 11459
	public static readonly Tag Liquifiable = TagManager.Create("Liquifiable");

	// Token: 0x04002CC4 RID: 11460
	public static readonly Tag IceOre = TagManager.Create("IceOre");

	// Token: 0x04002CC5 RID: 11461
	public static readonly Tag OxyRock = TagManager.Create("OxyRock");

	// Token: 0x04002CC6 RID: 11462
	public static readonly Tag Life = TagManager.Create("Life");

	// Token: 0x04002CC7 RID: 11463
	public static readonly Tag Fertilizer = TagManager.Create("Fertilizer");

	// Token: 0x04002CC8 RID: 11464
	public static readonly Tag Farmable = TagManager.Create("Farmable");

	// Token: 0x04002CC9 RID: 11465
	public static readonly Tag Agriculture = TagManager.Create("Agriculture");

	// Token: 0x04002CCA RID: 11466
	public static readonly Tag Organics = TagManager.Create("Organics");

	// Token: 0x04002CCB RID: 11467
	public static readonly Tag IndustrialProduct = TagManager.Create("IndustrialProduct");

	// Token: 0x04002CCC RID: 11468
	public static readonly Tag IndustrialIngredient = TagManager.Create("IndustrialIngredient");

	// Token: 0x04002CCD RID: 11469
	public static readonly Tag TechComponents = TagManager.Create("TechComponents");

	// Token: 0x04002CCE RID: 11470
	public static readonly Tag Other = TagManager.Create("Other");

	// Token: 0x04002CCF RID: 11471
	public static readonly Tag ManufacturedMaterial = TagManager.Create("ManufacturedMaterial");

	// Token: 0x04002CD0 RID: 11472
	public static readonly Tag Plastic = TagManager.Create("Plastic");

	// Token: 0x04002CD1 RID: 11473
	public static readonly Tag Steel = TagManager.Create("Steel");

	// Token: 0x04002CD2 RID: 11474
	public static readonly Tag BuildableAny = TagManager.Create("BuildableAny");

	// Token: 0x04002CD3 RID: 11475
	public static readonly Tag Decoration = TagManager.Create("Decoration");

	// Token: 0x04002CD4 RID: 11476
	public static readonly Tag Ornament = TagManager.Create("Ornament");

	// Token: 0x04002CD5 RID: 11477
	public static readonly Tag OrnamentDisplayer = TagManager.Create("OrnamentDisplay");

	// Token: 0x04002CD6 RID: 11478
	public static readonly Tag Window = TagManager.Create("Window");

	// Token: 0x04002CD7 RID: 11479
	public static readonly Tag Bunker = TagManager.Create("Bunker");

	// Token: 0x04002CD8 RID: 11480
	public static readonly Tag Transition = TagManager.Create("Transition");

	// Token: 0x04002CD9 RID: 11481
	public static readonly Tag Detecting = TagManager.Create("Detecting");

	// Token: 0x04002CDA RID: 11482
	public static readonly Tag RareMaterials = TagManager.Create("RareMaterials");

	// Token: 0x04002CDB RID: 11483
	public static readonly Tag BuildingFiber = TagManager.Create("BuildingFiber");

	// Token: 0x04002CDC RID: 11484
	public static readonly Tag Transparent = TagManager.Create("Transparent");

	// Token: 0x04002CDD RID: 11485
	public static readonly Tag Insulator = TagManager.Create("Insulator");

	// Token: 0x04002CDE RID: 11486
	public static readonly Tag Plumbable = TagManager.Create("Plumbable");

	// Token: 0x04002CDF RID: 11487
	public static readonly Tag BuildingWood = TagManager.Create("BuildingWood");

	// Token: 0x04002CE0 RID: 11488
	public static readonly Tag PreciousRock = TagManager.Create("PreciousRock");

	// Token: 0x04002CE1 RID: 11489
	public static readonly Tag Artifact = TagManager.Create("Artifact");

	// Token: 0x04002CE2 RID: 11490
	public static readonly Tag BionicUpgrade = TagManager.Create("BionicUpgrade");

	// Token: 0x04002CE3 RID: 11491
	public static readonly Tag BionicBedTime = TagManager.Create("BionicBedTime");

	// Token: 0x04002CE4 RID: 11492
	public static readonly Tag CharmedArtifact = TagManager.Create("CharmedArtifact");

	// Token: 0x04002CE5 RID: 11493
	public static readonly Tag TerrestrialArtifact = TagManager.Create("TerrestrialArtifact");

	// Token: 0x04002CE6 RID: 11494
	public static readonly Tag Keepsake = TagManager.Create("Keepsake");

	// Token: 0x04002CE7 RID: 11495
	public static readonly Tag MiscPickupable = TagManager.Create("MiscPickupable");

	// Token: 0x04002CE8 RID: 11496
	public static readonly Tag PlastifiableLiquid = TagManager.Create("PlastifiableLiquid");

	// Token: 0x04002CE9 RID: 11497
	public static readonly Tag CombustibleGas = TagManager.Create("CombustibleGas");

	// Token: 0x04002CEA RID: 11498
	public static readonly Tag CombustibleLiquid = TagManager.Create("CombustibleLiquid");

	// Token: 0x04002CEB RID: 11499
	public static readonly Tag CombustibleSolid = TagManager.Create("CombustibleSolid");

	// Token: 0x04002CEC RID: 11500
	public static readonly Tag FlyingCritterEdible = TagManager.Create("FlyingCritterEdible");

	// Token: 0x04002CED RID: 11501
	public static readonly Tag Comet = TagManager.Create("Comet");

	// Token: 0x04002CEE RID: 11502
	public static readonly Tag DeadReactor = TagManager.Create("DeadReactor");

	// Token: 0x04002CEF RID: 11503
	public static readonly Tag Robot = TagManager.Create("Robot");

	// Token: 0x04002CF0 RID: 11504
	public static readonly Tag StoryTraitResource = TagManager.Create("StoryTraitResource");

	// Token: 0x04002CF1 RID: 11505
	public static readonly Tag RoomProberBuilding = TagManager.Create("RoomProberBuilding");

	// Token: 0x04002CF2 RID: 11506
	public static readonly Tag DevBuilding = TagManager.Create("DevBuilding");

	// Token: 0x04002CF3 RID: 11507
	public static readonly Tag MarkedForMove = TagManager.Create("MarkedForMove");

	// Token: 0x04002CF4 RID: 11508
	public static readonly Tag HideHealthBar = TagManager.Create("HideHealthBar");

	// Token: 0x04002CF5 RID: 11509
	public static readonly Tag LongRangeMissile = TagManager.Create("LongRangeMissile");

	// Token: 0x04002CF6 RID: 11510
	public static readonly Tag LightSource = TagManager.Create("LightSource");

	// Token: 0x04002CF7 RID: 11511
	public static readonly Tag Incapacitated = TagManager.Create("Incapacitated");

	// Token: 0x04002CF8 RID: 11512
	public static readonly Tag CaloriesDepleted = TagManager.Create("CaloriesDepleted");

	// Token: 0x04002CF9 RID: 11513
	public static readonly Tag HitPointsDepleted = TagManager.Create("HitPointsDepleted");

	// Token: 0x04002CFA RID: 11514
	public static readonly Tag RadiationSicknessIncapacitation = TagManager.Create("RadiationSickness");

	// Token: 0x04002CFB RID: 11515
	public static readonly Tag Wilting = TagManager.Create("Wilting");

	// Token: 0x04002CFC RID: 11516
	public static readonly Tag Blighted = TagManager.Create("Blighted");

	// Token: 0x04002CFD RID: 11517
	public static readonly Tag PreventEmittingDisease = TagManager.Create("EmittingDisease");

	// Token: 0x04002CFE RID: 11518
	public static readonly Tag Creature = TagManager.Create("Creature");

	// Token: 0x04002CFF RID: 11519
	public static readonly Tag OriginalCreature = TagManager.Create("OriginalCreature");

	// Token: 0x04002D00 RID: 11520
	public static readonly Tag Hexaped = TagManager.Create("Hexaped");

	// Token: 0x04002D01 RID: 11521
	public static readonly Tag HeatBulb = TagManager.Create("HeatBulb");

	// Token: 0x04002D02 RID: 11522
	public static readonly Tag Egg = TagManager.Create("Egg");

	// Token: 0x04002D03 RID: 11523
	public static readonly Tag IncubatableEgg = TagManager.Create("IncubatableEgg");

	// Token: 0x04002D04 RID: 11524
	public static readonly Tag Trapped = TagManager.Create("Trapped");

	// Token: 0x04002D05 RID: 11525
	public static readonly Tag BagableCreature = TagManager.Create("BagableCreature");

	// Token: 0x04002D06 RID: 11526
	public static readonly Tag SwimmingCreature = TagManager.Create("SwimmingCreature");

	// Token: 0x04002D07 RID: 11527
	public static readonly Tag Spawner = TagManager.Create("Spawner");

	// Token: 0x04002D08 RID: 11528
	public static readonly Tag FullyIncubated = TagManager.Create("FullyIncubated");

	// Token: 0x04002D09 RID: 11529
	public static readonly Tag Amphibious = TagManager.Create("Amphibious");

	// Token: 0x04002D0A RID: 11530
	public static readonly Tag LargeCreature = TagManager.Create("LargeCreature");

	// Token: 0x04002D0B RID: 11531
	public static readonly Tag MoltShell = TagManager.Create("MoltShell");

	// Token: 0x04002D0C RID: 11532
	public static readonly Tag BaseMinion = TagManager.Create("BaseMinion");

	// Token: 0x04002D0D RID: 11533
	public static readonly Tag Corpse = TagManager.Create("Corpse");

	// Token: 0x04002D0E RID: 11534
	public static readonly Tag Alloy = TagManager.Create("Alloy");

	// Token: 0x04002D0F RID: 11535
	public static readonly Tag Metal = TagManager.Create("Metal");

	// Token: 0x04002D10 RID: 11536
	public static readonly Tag RefinedMetal = TagManager.Create("RefinedMetal");

	// Token: 0x04002D11 RID: 11537
	public static readonly Tag PreciousMetal = TagManager.Create("PreciousMetal");

	// Token: 0x04002D12 RID: 11538
	public static readonly Tag StoredMetal = TagManager.Create("StoredMetal");

	// Token: 0x04002D13 RID: 11539
	public static readonly Tag Solid = TagManager.Create("Solid");

	// Token: 0x04002D14 RID: 11540
	public static readonly Tag Liquid = TagManager.Create("Liquid");

	// Token: 0x04002D15 RID: 11541
	public static readonly Tag LiquidSource = TagManager.Create("LiquidSource");

	// Token: 0x04002D16 RID: 11542
	public static readonly Tag GasSource = TagManager.Create("GasSource");

	// Token: 0x04002D17 RID: 11543
	public static readonly Tag Water = TagManager.Create("Water");

	// Token: 0x04002D18 RID: 11544
	public static readonly Tag DirtyWater = TagManager.Create("DirtyWater");

	// Token: 0x04002D19 RID: 11545
	public static readonly Tag AnyWater = TagManager.Create("AnyWater");

	// Token: 0x04002D1A RID: 11546
	public static readonly Tag LubricatingOil = TagManager.Create("LubricatingOil");

	// Token: 0x04002D1B RID: 11547
	public static readonly Tag Algae = TagManager.Create("Algae");

	// Token: 0x04002D1C RID: 11548
	public static readonly Tag Void = TagManager.Create("Void");

	// Token: 0x04002D1D RID: 11549
	public static readonly Tag Chlorine = TagManager.Create("Chlorine");

	// Token: 0x04002D1E RID: 11550
	public static readonly Tag Oxygen = TagManager.Create("Oxygen");

	// Token: 0x04002D1F RID: 11551
	public static readonly Tag Hydrogen = TagManager.Create("Hydrogen");

	// Token: 0x04002D20 RID: 11552
	public static readonly Tag Methane = TagManager.Create("Methane");

	// Token: 0x04002D21 RID: 11553
	public static readonly Tag CarbonDioxide = TagManager.Create("CarbonDioxide");

	// Token: 0x04002D22 RID: 11554
	public static readonly Tag Carbon = TagManager.Create("Carbon");

	// Token: 0x04002D23 RID: 11555
	public static readonly Tag BuildableRaw = TagManager.Create("BuildableRaw");

	// Token: 0x04002D24 RID: 11556
	public static readonly Tag BuildableProcessed = TagManager.Create("BuildableProcessed");

	// Token: 0x04002D25 RID: 11557
	public static readonly Tag Phosphorus = TagManager.Create("Phosphorus");

	// Token: 0x04002D26 RID: 11558
	public static readonly Tag Phosphorite = TagManager.Create("Phosphorite");

	// Token: 0x04002D27 RID: 11559
	public static readonly Tag SlimeMold = TagManager.Create("SlimeMold");

	// Token: 0x04002D28 RID: 11560
	public static readonly Tag Filler = TagManager.Create("Filler");

	// Token: 0x04002D29 RID: 11561
	public static readonly Tag Item = TagManager.Create("Item");

	// Token: 0x04002D2A RID: 11562
	public static readonly Tag Ore = TagManager.Create("Ore");

	// Token: 0x04002D2B RID: 11563
	public static readonly Tag GenericOre = TagManager.Create("GenericOre");

	// Token: 0x04002D2C RID: 11564
	public static readonly Tag Ingot = TagManager.Create("Ingot");

	// Token: 0x04002D2D RID: 11565
	public static readonly Tag Dirt = TagManager.Create("Dirt");

	// Token: 0x04002D2E RID: 11566
	public static readonly Tag Filter = TagManager.Create("Filter");

	// Token: 0x04002D2F RID: 11567
	public static readonly Tag ConsumableOre = TagManager.Create("ConsumableOre");

	// Token: 0x04002D30 RID: 11568
	public static readonly Tag Unstable = TagManager.Create("Unstable");

	// Token: 0x04002D31 RID: 11569
	public static readonly Tag Slippery = TagManager.Create("Slippery");

	// Token: 0x04002D32 RID: 11570
	public static readonly Tag Sublimating = TagManager.Create("Sublimating");

	// Token: 0x04002D33 RID: 11571
	public static readonly Tag HideFromSpawnTool = TagManager.Create("HideFromSpawnTool");

	// Token: 0x04002D34 RID: 11572
	public static readonly Tag HideFromCodex = TagManager.Create("HideFromCodex");

	// Token: 0x04002D35 RID: 11573
	public static readonly Tag EmitsLight = TagManager.Create("EmitsLight");

	// Token: 0x04002D36 RID: 11574
	public static readonly Tag Special = TagManager.Create("Special");

	// Token: 0x04002D37 RID: 11575
	public static readonly Tag Breathable = TagManager.Create("Breathable");

	// Token: 0x04002D38 RID: 11576
	public static readonly Tag Unbreathable = TagManager.Create("Unbreathable");

	// Token: 0x04002D39 RID: 11577
	public static readonly Tag Gas = TagManager.Create("Gas");

	// Token: 0x04002D3A RID: 11578
	public static readonly Tag Crushable = TagManager.Create("Crushable");

	// Token: 0x04002D3B RID: 11579
	public static readonly Tag Noncrushable = TagManager.Create("Noncrushable");

	// Token: 0x04002D3C RID: 11580
	public static readonly Tag IronOre = TagManager.Create("IronOre");

	// Token: 0x04002D3D RID: 11581
	public static readonly Tag HighEnergyParticle = TagManager.Create("HighEnergyParticle");

	// Token: 0x04002D3E RID: 11582
	public static readonly Tag IgnoreMaterialCategory = TagManager.Create("IgnoreMaterialCategory");

	// Token: 0x04002D3F RID: 11583
	public static readonly Tag Oxidizer = TagManager.Create("Oxidizer");

	// Token: 0x04002D40 RID: 11584
	public static readonly Tag UnrefinedOil = TagManager.Create("UnrefinedOil");

	// Token: 0x04002D41 RID: 11585
	public static readonly Tag RiverSource = TagManager.Create("RiverSource");

	// Token: 0x04002D42 RID: 11586
	public static readonly Tag RiverSink = TagManager.Create("RiverSink");

	// Token: 0x04002D43 RID: 11587
	public static readonly Tag Garbage = TagManager.Create("Garbage");

	// Token: 0x04002D44 RID: 11588
	public static readonly Tag OilWell = TagManager.Create("OilWell");

	// Token: 0x04002D45 RID: 11589
	public static readonly Tag Glass = TagManager.Create("Glass");

	// Token: 0x04002D46 RID: 11590
	public static readonly Tag Door = TagManager.Create("Door");

	// Token: 0x04002D47 RID: 11591
	public static readonly Tag Farm = TagManager.Create("Farm");

	// Token: 0x04002D48 RID: 11592
	public static readonly Tag StorageLocker = TagManager.Create("StorageLocker");

	// Token: 0x04002D49 RID: 11593
	public static readonly Tag LadderBed = TagManager.Create("LadderBed");

	// Token: 0x04002D4A RID: 11594
	public static readonly Tag FloorTiles = TagManager.Create("FloorTiles");

	// Token: 0x04002D4B RID: 11595
	public static readonly Tag Carpeted = TagManager.Create("Carpeted");

	// Token: 0x04002D4C RID: 11596
	public static readonly Tag FarmTiles = TagManager.Create("FarmTiles");

	// Token: 0x04002D4D RID: 11597
	public static readonly Tag Ladders = TagManager.Create("Ladders");

	// Token: 0x04002D4E RID: 11598
	public static readonly Tag NavTeleporters = TagManager.Create("NavTeleporters");

	// Token: 0x04002D4F RID: 11599
	public static readonly Tag Wires = TagManager.Create("Wires");

	// Token: 0x04002D50 RID: 11600
	public static readonly Tag Vents = TagManager.Create("Vents");

	// Token: 0x04002D51 RID: 11601
	public static readonly Tag Pipes = TagManager.Create("Pipes");

	// Token: 0x04002D52 RID: 11602
	public static readonly Tag WireBridges = TagManager.Create("WireBridges");

	// Token: 0x04002D53 RID: 11603
	public static readonly Tag TravelTubeBridges = TagManager.Create("TravelTubeBridges");

	// Token: 0x04002D54 RID: 11604
	public static readonly Tag Backwall = TagManager.Create("Backwall");

	// Token: 0x04002D55 RID: 11605
	public static readonly Tag MISSING_TAG = TagManager.Create("MISSING_TAG");

	// Token: 0x04002D56 RID: 11606
	public static readonly Tag PlantRenderer = TagManager.Create("PlantRenderer");

	// Token: 0x04002D57 RID: 11607
	public static readonly Tag Usable = TagManager.Create("Usable");

	// Token: 0x04002D58 RID: 11608
	public static readonly Tag PedestalDisplayable = TagManager.Create("PedestalDisplayable");

	// Token: 0x04002D59 RID: 11609
	public static readonly Tag HasChores = TagManager.Create("HasChores");

	// Token: 0x04002D5A RID: 11610
	public static readonly Tag Suit = TagManager.Create("Suit");

	// Token: 0x04002D5B RID: 11611
	public static readonly Tag AirtightSuit = TagManager.Create("AirtightSuit");

	// Token: 0x04002D5C RID: 11612
	public static readonly Tag AtmoSuit = TagManager.Create("Atmo_Suit");

	// Token: 0x04002D5D RID: 11613
	public static readonly Tag OxygenMask = TagManager.Create("Oxygen_Mask");

	// Token: 0x04002D5E RID: 11614
	public static readonly Tag LeadSuit = TagManager.Create("Lead_Suit");

	// Token: 0x04002D5F RID: 11615
	public static readonly Tag JetSuit = TagManager.Create("Jet_Suit");

	// Token: 0x04002D60 RID: 11616
	public static readonly Tag JetSuitOutOfFuel = TagManager.Create("JetSuitOutOfFuel");

	// Token: 0x04002D61 RID: 11617
	public static readonly Tag SuitBatteryLow = TagManager.Create("SuitBatteryLow");

	// Token: 0x04002D62 RID: 11618
	public static readonly Tag SuitBatteryOut = TagManager.Create("SuitBatteryOut");

	// Token: 0x04002D63 RID: 11619
	public static readonly List<Tag> AllSuitTags = new List<Tag>
	{
		GameTags.Suit,
		GameTags.AtmoSuit,
		GameTags.JetSuit,
		GameTags.LeadSuit
	};

	// Token: 0x04002D64 RID: 11620
	public static readonly List<Tag> OxygenSuitTags = new List<Tag>
	{
		GameTags.AtmoSuit,
		GameTags.JetSuit,
		GameTags.LeadSuit
	};

	// Token: 0x04002D65 RID: 11621
	public static readonly Tag EquippableBalloon = TagManager.Create("EquippableBalloon");

	// Token: 0x04002D66 RID: 11622
	public static readonly Tag Clothes = TagManager.Create("Clothes");

	// Token: 0x04002D67 RID: 11623
	public static readonly Tag WarmVest = TagManager.Create("Warm_Vest");

	// Token: 0x04002D68 RID: 11624
	public static readonly Tag FunkyVest = TagManager.Create("Funky_Vest");

	// Token: 0x04002D69 RID: 11625
	public static readonly List<Tag> AllClothesTags = new List<Tag>
	{
		GameTags.Clothes,
		GameTags.WarmVest,
		GameTags.FunkyVest
	};

	// Token: 0x04002D6A RID: 11626
	public static readonly Tag Assigned = TagManager.Create("Assigned");

	// Token: 0x04002D6B RID: 11627
	public static readonly Tag Helmet = TagManager.Create("Helmet");

	// Token: 0x04002D6C RID: 11628
	public static readonly Tag Equipped = TagManager.Create("Equipped");

	// Token: 0x04002D6D RID: 11629
	public static readonly Tag DisposablePortableBattery = TagManager.Create("DisposablePortableBattery");

	// Token: 0x04002D6E RID: 11630
	public static readonly Tag ChargedPortableBattery = TagManager.Create("ChargedPortableBattery");

	// Token: 0x04002D6F RID: 11631
	public static readonly Tag EmptyPortableBattery = TagManager.Create("EmptyPortableBattery");

	// Token: 0x04002D70 RID: 11632
	public static readonly Tag SolidLubricant = TagManager.Create("SolidLubricant");

	// Token: 0x04002D71 RID: 11633
	public static readonly Tag Entombed = TagManager.Create("Entombed");

	// Token: 0x04002D72 RID: 11634
	public static readonly Tag Uprooted = TagManager.Create("Uprooted");

	// Token: 0x04002D73 RID: 11635
	public static readonly Tag Preserved = TagManager.Create("Preserved");

	// Token: 0x04002D74 RID: 11636
	public static readonly Tag Compostable = TagManager.Create("Compostable");

	// Token: 0x04002D75 RID: 11637
	public static readonly Tag Pickled = TagManager.Create("Pickled");

	// Token: 0x04002D76 RID: 11638
	public static readonly Tag UnspicedFood = TagManager.Create("UnspicedFood");

	// Token: 0x04002D77 RID: 11639
	public static readonly Tag SpicedFood = TagManager.Create("SpicedFood");

	// Token: 0x04002D78 RID: 11640
	public static readonly Tag Dying = TagManager.Create("Dying");

	// Token: 0x04002D79 RID: 11641
	public static readonly Tag Dead = TagManager.Create("Dead");

	// Token: 0x04002D7A RID: 11642
	public static readonly Tag PreventDeadAnimation = TagManager.Create("PreventDeadAnimation");

	// Token: 0x04002D7B RID: 11643
	public static readonly Tag Reachable = TagManager.Create("Reachable");

	// Token: 0x04002D7C RID: 11644
	public static readonly Tag PreventChoreInterruption = TagManager.Create("PreventChoreInterruption");

	// Token: 0x04002D7D RID: 11645
	public static readonly Tag PerformingWorkRequest = TagManager.Create("PerformingWorkRequest");

	// Token: 0x04002D7E RID: 11646
	public static readonly Tag RecoveringBreath = TagManager.Create("RecoveringBreath");

	// Token: 0x04002D7F RID: 11647
	public static readonly Tag FeelingCold = TagManager.Create("FeelingCold");

	// Token: 0x04002D80 RID: 11648
	public static readonly Tag FeelingWarm = TagManager.Create("FeelingWarm");

	// Token: 0x04002D81 RID: 11649
	public static readonly Tag RecoveringWarmnth = TagManager.Create("RecoveringWarmnth");

	// Token: 0x04002D82 RID: 11650
	public static readonly Tag RecoveringFromHeat = TagManager.Create("RecoveringFromHeat");

	// Token: 0x04002D83 RID: 11651
	public static readonly Tag NoOxygen = TagManager.Create("NoOxygen");

	// Token: 0x04002D84 RID: 11652
	public static readonly Tag Idle = TagManager.Create("Idle");

	// Token: 0x04002D85 RID: 11653
	public static readonly Tag StationaryIdling = TagManager.Create("StationaryIdling");

	// Token: 0x04002D86 RID: 11654
	public static readonly Tag AlwaysConverse = TagManager.Create("AlwaysConverse");

	// Token: 0x04002D87 RID: 11655
	public static readonly Tag SuppressConversation = TagManager.Create("SuppressConversation");

	// Token: 0x04002D88 RID: 11656
	public static readonly Tag HasDebugDestination = TagManager.Create("HasDebugDestination");

	// Token: 0x04002D89 RID: 11657
	public static readonly Tag Shaded = TagManager.Create("Shaded");

	// Token: 0x04002D8A RID: 11658
	public static readonly Tag TakingMedicine = TagManager.Create("TakingMedicine");

	// Token: 0x04002D8B RID: 11659
	public static readonly Tag Partying = TagManager.Create("Partying");

	// Token: 0x04002D8C RID: 11660
	public static readonly Tag MakingMess = TagManager.Create("MakingMess");

	// Token: 0x04002D8D RID: 11661
	public static readonly Tag DupeBrain = TagManager.Create("DupeBrain");

	// Token: 0x04002D8E RID: 11662
	public static readonly Tag CreatureBrain = TagManager.Create("CreatureBrain");

	// Token: 0x04002D8F RID: 11663
	public static readonly Tag Asleep = TagManager.Create("Asleep");

	// Token: 0x04002D90 RID: 11664
	public static readonly Tag HoldingBreath = TagManager.Create("HoldingBreath");

	// Token: 0x04002D91 RID: 11665
	public static readonly Tag Overjoyed = TagManager.Create("Overjoyed");

	// Token: 0x04002D92 RID: 11666
	public static readonly Tag PleasantConversation = TagManager.Create("PleasantConversation");

	// Token: 0x04002D93 RID: 11667
	public static readonly Tag CommunalDining = TagManager.Create("CommunalDining");

	// Token: 0x04002D94 RID: 11668
	public static readonly Tag WantsToTalk = TagManager.Create("WantsToTalk");

	// Token: 0x04002D95 RID: 11669
	public static readonly Tag DoNotInterruptMe = TagManager.Create("DoNotInterruptMe");

	// Token: 0x04002D96 RID: 11670
	public static readonly Tag HasSuitTank = TagManager.Create("HasSuitTank");

	// Token: 0x04002D97 RID: 11671
	public static readonly Tag HasAirtightSuit = TagManager.Create("HasAirtightSuit");

	// Token: 0x04002D98 RID: 11672
	public static readonly Tag NoCreatureIdling = TagManager.Create("NoCreatureIdling");

	// Token: 0x04002D99 RID: 11673
	public static readonly Tag UnderConstruction = TagManager.Create("UnderConstruction");

	// Token: 0x04002D9A RID: 11674
	public static readonly Tag Operational = TagManager.Create("Operational");

	// Token: 0x04002D9B RID: 11675
	public static readonly Tag JetSuitBlocker = TagManager.Create("JetSuitBlocker");

	// Token: 0x04002D9C RID: 11676
	public static readonly Tag HasInvalidPorts = TagManager.Create("HasInvalidPorts");

	// Token: 0x04002D9D RID: 11677
	public static readonly Tag NotRoomAssignable = TagManager.Create("NotRoomAssignable");

	// Token: 0x04002D9E RID: 11678
	public static readonly Tag OneTimeUseLure = TagManager.Create("OneTimeUseLure");

	// Token: 0x04002D9F RID: 11679
	public static readonly Tag LureUsed = TagManager.Create("LureUsed");

	// Token: 0x04002DA0 RID: 11680
	public static readonly Tag TemplateBuilding = TagManager.Create("TemplateBuilding");

	// Token: 0x04002DA1 RID: 11681
	public static readonly Tag ModularConduitPort = TagManager.Create("ModularConduitPort");

	// Token: 0x04002DA2 RID: 11682
	public static readonly Tag WarpTech = TagManager.Create("WarpTech");

	// Token: 0x04002DA3 RID: 11683
	public static readonly Tag HEPPassThrough = TagManager.Create("HEPPassThrough");

	// Token: 0x04002DA4 RID: 11684
	public static readonly Tag TelephoneRinging = TagManager.Create("TelephoneRinging");

	// Token: 0x04002DA5 RID: 11685
	public static readonly Tag LongDistanceCall = TagManager.Create("LongDistanceCall");

	// Token: 0x04002DA6 RID: 11686
	public static readonly Tag Telepad = TagManager.Create("Telepad");

	// Token: 0x04002DA7 RID: 11687
	public static readonly Tag InTransitTube = TagManager.Create("InTransitTube");

	// Token: 0x04002DA8 RID: 11688
	public static readonly Tag TrapArmed = TagManager.Create("TrapArmed");

	// Token: 0x04002DA9 RID: 11689
	public static readonly Tag GeyserFeature = TagManager.Create("GeyserFeature");

	// Token: 0x04002DAA RID: 11690
	public static readonly Tag Rocket = TagManager.Create("Rocket");

	// Token: 0x04002DAB RID: 11691
	public static readonly Tag RocketOnGround = TagManager.Create("RocketOnGround");

	// Token: 0x04002DAC RID: 11692
	public static readonly Tag RocketNotOnGround = TagManager.Create("RocketNotOnGround");

	// Token: 0x04002DAD RID: 11693
	public static readonly Tag RocketInSpace = TagManager.Create("RocketInSpace");

	// Token: 0x04002DAE RID: 11694
	public static readonly Tag RocketStranded = TagManager.Create("RocketStranded");

	// Token: 0x04002DAF RID: 11695
	public static readonly Tag RailGunPayloadEmptyable = TagManager.Create("RailGunPayloadEmptyable");

	// Token: 0x04002DB0 RID: 11696
	public static readonly Tag TransferringCargoComplete = TagManager.Create("TransferringCargoComplete");

	// Token: 0x04002DB1 RID: 11697
	public static readonly Tag NoseRocketModule = TagManager.Create("NoseRocketModule");

	// Token: 0x04002DB2 RID: 11698
	public static readonly Tag LaunchButtonRocketModule = TagManager.Create("LaunchButtonRocketModule");

	// Token: 0x04002DB3 RID: 11699
	public static readonly Tag RocketInteriorBuilding = TagManager.Create("RocketInteriorBuilding");

	// Token: 0x04002DB4 RID: 11700
	public static readonly Tag NotRocketInteriorBuilding = TagManager.Create("NotRocketInteriorBuilding");

	// Token: 0x04002DB5 RID: 11701
	public static readonly Tag UniquePerWorld = TagManager.Create("UniquePerWorld");

	// Token: 0x04002DB6 RID: 11702
	public static readonly Tag RocketEnvelopeTile = TagManager.Create("RocketEnvelopeTile");

	// Token: 0x04002DB7 RID: 11703
	public static readonly Tag NoRocketRefund = TagManager.Create("NoRocketRefund");

	// Token: 0x04002DB8 RID: 11704
	public static readonly Tag RocketModule = TagManager.Create("RocketModule");

	// Token: 0x04002DB9 RID: 11705
	public static readonly Tag GantryExtended = TagManager.Create("GantryExtended");

	// Token: 0x04002DBA RID: 11706
	public static readonly Tag RocketDrilling = TagManager.Create("RocketDrilling");

	// Token: 0x04002DBB RID: 11707
	public static readonly Tag RocketCollectingResources = TagManager.Create("RocketCollectingResources");

	// Token: 0x04002DBC RID: 11708
	public static readonly Tag BallisticEntityLanding = TagManager.Create("BallisticEntityLanding");

	// Token: 0x04002DBD RID: 11709
	public static readonly Tag BallisticEntityLaunching = TagManager.Create("BallisticEntityLaunching");

	// Token: 0x04002DBE RID: 11710
	public static readonly Tag BallisticEntityMoving = TagManager.Create("BallisticEntityMoving");

	// Token: 0x04002DBF RID: 11711
	public static readonly Tag ClusterEntityGrounded = TagManager.Create("ClusterEntityGrounded ");

	// Token: 0x04002DC0 RID: 11712
	public static readonly Tag LongRangeMissileMoving = TagManager.Create("LongRangeMissileMoving");

	// Token: 0x04002DC1 RID: 11713
	public static readonly Tag LongRangeMissileIdle = TagManager.Create("LongRangeMissileIdle");

	// Token: 0x04002DC2 RID: 11714
	public static readonly Tag LongRangeMissileExploding = TagManager.Create("LongRangeMissileExploding");

	// Token: 0x04002DC3 RID: 11715
	public static readonly Tag EntityInSpace = TagManager.Create("EntityInSpace");

	// Token: 0x04002DC4 RID: 11716
	public static readonly Tag Monument = TagManager.Create("Monument");

	// Token: 0x04002DC5 RID: 11717
	public static readonly Tag Stored = TagManager.Create("Stored");

	// Token: 0x04002DC6 RID: 11718
	public static readonly Tag StoredPrivate = TagManager.Create("StoredPrivate");

	// Token: 0x04002DC7 RID: 11719
	public static readonly Tag Sealed = TagManager.Create("Sealed");

	// Token: 0x04002DC8 RID: 11720
	public static readonly Tag CorrosionProof = TagManager.Create("CorrosionProof");

	// Token: 0x04002DC9 RID: 11721
	public static readonly Tag PickupableStorage = TagManager.Create("PickupableStorage");

	// Token: 0x04002DCA RID: 11722
	public static readonly Tag UnidentifiedSeed = TagManager.Create("UnidentifiedSeed");

	// Token: 0x04002DCB RID: 11723
	public static readonly Tag CropSeed = TagManager.Create("CropSeed");

	// Token: 0x04002DCC RID: 11724
	public static readonly Tag DecorSeed = TagManager.Create("DecorSeed");

	// Token: 0x04002DCD RID: 11725
	public static readonly Tag WaterSeed = TagManager.Create("WaterSeed");

	// Token: 0x04002DCE RID: 11726
	public static readonly Tag Harvestable = TagManager.Create("Harvestable");

	// Token: 0x04002DCF RID: 11727
	public static readonly Tag Hanging = TagManager.Create("Hanging");

	// Token: 0x04002DD0 RID: 11728
	public static readonly Tag FarmingMaterial = TagManager.Create("FarmingMaterial");

	// Token: 0x04002DD1 RID: 11729
	public static readonly Tag MutatedSeed = TagManager.Create("MutatedSeed");

	// Token: 0x04002DD2 RID: 11730
	public static readonly Tag OverlayInFrontOfConduits = TagManager.Create("OverlayFrontLayer");

	// Token: 0x04002DD3 RID: 11731
	public static readonly Tag OverlayBehindConduits = TagManager.Create("OverlayBackLayer");

	// Token: 0x04002DD4 RID: 11732
	public static readonly Tag MassChunk = TagManager.Create("MassChunk");

	// Token: 0x04002DD5 RID: 11733
	public static readonly Tag UnitChunk = TagManager.Create("UnitChunk");

	// Token: 0x04002DD6 RID: 11734
	public static readonly Tag NotConversationTopic = TagManager.Create("NotConversationTopic");

	// Token: 0x04002DD7 RID: 11735
	public static readonly Tag MinionSelectPreview = TagManager.Create("MinionSelectPreview");

	// Token: 0x04002DD8 RID: 11736
	public static readonly Tag Empty = TagManager.Create("Empty");

	// Token: 0x04002DD9 RID: 11737
	public static readonly Tag ExcludeFromTemplate = TagManager.Create("ExcludeFromTemplate");

	// Token: 0x04002DDA RID: 11738
	public static readonly Tag SpaceDanger = TagManager.Create("SpaceDanger");

	// Token: 0x04002DDB RID: 11739
	public static TagSet SolidElements = new TagSet();

	// Token: 0x04002DDC RID: 11740
	public static TagSet LiquidElements = new TagSet();

	// Token: 0x04002DDD RID: 11741
	public static TagSet GasElements = new TagSet();

	// Token: 0x04002DDE RID: 11742
	public static TagSet CalorieCategories = new TagSet
	{
		GameTags.Edible
	};

	// Token: 0x04002DDF RID: 11743
	public static TagSet UnitCategories = new TagSet
	{
		GameTags.Medicine,
		GameTags.MedicalSupplies,
		GameTags.Seed,
		GameTags.Egg,
		GameTags.Clothes,
		GameTags.IndustrialIngredient,
		GameTags.IndustrialProduct,
		GameTags.TechComponents,
		GameTags.Compostable,
		GameTags.HighEnergyParticle,
		GameTags.StoryTraitResource,
		GameTags.Dehydrated,
		GameTags.ChargedPortableBattery,
		GameTags.BionicUpgrade
	};

	// Token: 0x04002DE0 RID: 11744
	public static TagSet IgnoredMaterialCategories = new TagSet
	{
		GameTags.Special,
		GameTags.IgnoreMaterialCategory
	};

	// Token: 0x04002DE1 RID: 11745
	public static TagSet MaterialCategories = new TagSet
	{
		GameTags.Alloy,
		GameTags.Metal,
		GameTags.RefinedMetal,
		GameTags.BuildableRaw,
		GameTags.BuildableProcessed,
		GameTags.Filter,
		GameTags.Liquifiable,
		GameTags.Liquid,
		GameTags.Breathable,
		GameTags.Unbreathable,
		GameTags.ConsumableOre,
		GameTags.Sublimating,
		GameTags.Organics,
		GameTags.Farmable,
		GameTags.Agriculture,
		GameTags.Other,
		GameTags.ManufacturedMaterial,
		GameTags.CookingIngredient,
		GameTags.RareMaterials
	};

	// Token: 0x04002DE2 RID: 11746
	public static TagSet BionicCompatibleBatteries = new TagSet
	{
		"Electrobank",
		GameTags.DisposablePortableBattery,
		GameTags.EmptyPortableBattery
	};

	// Token: 0x04002DE3 RID: 11747
	public static TagSet BionicIncompatibleBatteries = new TagSet
	{
		"SelfChargingElectrobank"
	};

	// Token: 0x04002DE4 RID: 11748
	public static TagSet MaterialBuildingElements = new TagSet
	{
		GameTags.BuildingFiber,
		GameTags.BuildingWood
	};

	// Token: 0x04002DE5 RID: 11749
	public static TagSet OtherEntityTags = new TagSet
	{
		GameTags.BagableCreature,
		GameTags.SwimmingCreature,
		GameTags.MiscPickupable
	};

	// Token: 0x04002DE6 RID: 11750
	public static TagSet AllCategories = new TagSet(new TagSet[]
	{
		GameTags.CalorieCategories,
		GameTags.UnitCategories,
		GameTags.MaterialCategories,
		GameTags.MaterialBuildingElements,
		GameTags.OtherEntityTags
	});

	// Token: 0x04002DE7 RID: 11751
	public static TagSet DisplayAsCalories = new TagSet(GameTags.CalorieCategories);

	// Token: 0x04002DE8 RID: 11752
	public static TagSet DisplayAsUnits = new TagSet(GameTags.UnitCategories);

	// Token: 0x04002DE9 RID: 11753
	public static TagSet DisplayAsInformation = new TagSet();

	// Token: 0x04002DEA RID: 11754
	public static Tag StartingMetalOre = new Tag("StartingMetalOre");

	// Token: 0x04002DEB RID: 11755
	public static Tag StartingRefinedMetal = new Tag("StartingRefinedMetal");

	// Token: 0x04002DEC RID: 11756
	public static Tag[] StartingMetalOres;

	// Token: 0x04002DED RID: 11757
	public static Tag[] StartingRefinedMetals = null;

	// Token: 0x04002DEE RID: 11758
	public static Tag[] BasicWoods = new Tag[]
	{
		SimHashes.WoodLog.CreateTag(),
		SimHashes.FabricatedWood.CreateTag()
	};

	// Token: 0x04002DEF RID: 11759
	public static Tag[] BasicMetalOres = new Tag[]
	{
		SimHashes.IronOre.CreateTag()
	};

	// Token: 0x04002DF0 RID: 11760
	public static Tag[] BasicRefinedMetals = new Tag[]
	{
		SimHashes.Iron.CreateTag()
	};

	// Token: 0x04002DF1 RID: 11761
	public static TagSet HiddenElementTags = new TagSet
	{
		GameTags.HideFromCodex,
		GameTags.HideFromSpawnTool,
		GameTags.StartingMetalOre,
		GameTags.StartingRefinedMetal
	};

	// Token: 0x04002DF2 RID: 11762
	public static Tag[] Fabrics = new Tag[]
	{
		"BasicFabric".ToTag(),
		FeatherFabricConfig.ID
	};

	// Token: 0x02001977 RID: 6519
	public static class Worlds
	{
		// Token: 0x04007DE0 RID: 32224
		public static readonly Tag Ceres = TagManager.Create("Ceres");
	}

	// Token: 0x02001978 RID: 6520
	public abstract class ChoreTypes
	{
		// Token: 0x04007DE1 RID: 32225
		public static readonly Tag Farming = TagManager.Create("Farming");

		// Token: 0x04007DE2 RID: 32226
		public static readonly Tag Ranching = TagManager.Create("Ranching");

		// Token: 0x04007DE3 RID: 32227
		public static readonly Tag Research = TagManager.Create("Research");

		// Token: 0x04007DE4 RID: 32228
		public static readonly Tag Power = TagManager.Create("Power");

		// Token: 0x04007DE5 RID: 32229
		public static readonly Tag Building = TagManager.Create("Building");

		// Token: 0x04007DE6 RID: 32230
		public static readonly Tag Cooking = TagManager.Create("Cooking");

		// Token: 0x04007DE7 RID: 32231
		public static readonly Tag Fabricating = TagManager.Create("Fabricating");

		// Token: 0x04007DE8 RID: 32232
		public static readonly Tag Wiring = TagManager.Create("Wiring");

		// Token: 0x04007DE9 RID: 32233
		public static readonly Tag Art = TagManager.Create("Art");

		// Token: 0x04007DEA RID: 32234
		public static readonly Tag Digging = TagManager.Create("Digging");

		// Token: 0x04007DEB RID: 32235
		public static readonly Tag Doctoring = TagManager.Create("Doctoring");

		// Token: 0x04007DEC RID: 32236
		public static readonly Tag Conveyor = TagManager.Create("Conveyor");
	}

	// Token: 0x02001979 RID: 6521
	public static class RotModifierTags
	{
		// Token: 0x04007DED RID: 32237
		public static readonly Tag Fresh = TagManager.Create("Fresh");

		// Token: 0x04007DEE RID: 32238
		public static readonly Tag Stale = TagManager.Create("Stale");

		// Token: 0x04007DEF RID: 32239
		public static readonly Tag DeepFrozen = TagManager.Create("DeepFrozen");

		// Token: 0x04007DF0 RID: 32240
		public static readonly Tag Refrigerated = TagManager.Create("Refrigerated");
	}

	// Token: 0x0200197A RID: 6522
	public static class Creatures
	{
		// Token: 0x04007DF1 RID: 32241
		public static readonly Tag ReservedByCreature = TagManager.Create("ReservedByCreature");

		// Token: 0x04007DF2 RID: 32242
		public static readonly Tag PreventGrowAnimation = TagManager.Create("PreventGrowAnimation");

		// Token: 0x04007DF3 RID: 32243
		public static readonly Tag TrappedInCargoBay = TagManager.Create("TrappedInCargoBay");

		// Token: 0x04007DF4 RID: 32244
		public static readonly Tag PausedHunger = TagManager.Create("PausedHunger");

		// Token: 0x04007DF5 RID: 32245
		public static readonly Tag PausedReproduction = TagManager.Create("PausedReproduction");

		// Token: 0x04007DF6 RID: 32246
		public static readonly Tag Bagged = TagManager.Create("Bagged");

		// Token: 0x04007DF7 RID: 32247
		public static readonly Tag InIncubator = TagManager.Create("InIncubator");

		// Token: 0x04007DF8 RID: 32248
		public static readonly Tag Deliverable = TagManager.Create("Deliverable");

		// Token: 0x04007DF9 RID: 32249
		public static readonly Tag StunnedForCapture = TagManager.Create("StunnedForCapture");

		// Token: 0x04007DFA RID: 32250
		public static readonly Tag StunnedBeingEaten = TagManager.Create("StunnedBeingEaten");

		// Token: 0x04007DFB RID: 32251
		public static readonly Tag Falling = TagManager.Create("Falling");

		// Token: 0x04007DFC RID: 32252
		public static readonly Tag Flopping = TagManager.Create("Flopping");

		// Token: 0x04007DFD RID: 32253
		public static readonly Tag WantsToEnterBurrow = TagManager.Create("WantsToBurrow");

		// Token: 0x04007DFE RID: 32254
		public static readonly Tag Burrowed = TagManager.Create("Burrowed");

		// Token: 0x04007DFF RID: 32255
		public static readonly Tag WantsToExitBurrow = TagManager.Create("WantsToExitBurrow");

		// Token: 0x04007E00 RID: 32256
		public static readonly Tag WantsToEat = TagManager.Create("WantsToEat");

		// Token: 0x04007E01 RID: 32257
		public static readonly Tag SuppressedDiet = TagManager.Create("SuppressedDiet");

		// Token: 0x04007E02 RID: 32258
		public static readonly Tag UrgeToPoke = TagManager.Create("UrgeToPoke");

		// Token: 0x04007E03 RID: 32259
		public static readonly Tag WantsToStomp = TagManager.Create("WantsToStomp");

		// Token: 0x04007E04 RID: 32260
		public static readonly Tag WantsToHarvest = TagManager.Create("WantsToHarvest");

		// Token: 0x04007E05 RID: 32261
		public static readonly Tag Behaviour_TryToDrinkMilkFromFeeder = TagManager.Create("Behaviour_TryToDrinkMilkFromFeeder");

		// Token: 0x04007E06 RID: 32262
		public static readonly Tag Behaviour_InteractWithCritterCondo = TagManager.Create("Behaviour_InteractWithCritterCondo");

		// Token: 0x04007E07 RID: 32263
		public static readonly Tag WantsToGetRanched = TagManager.Create("WantsToGetRanched");

		// Token: 0x04007E08 RID: 32264
		public static readonly Tag WantsToGetCaptured = TagManager.Create("WantsToGetCaptured");

		// Token: 0x04007E09 RID: 32265
		public static readonly Tag WantsToClimbTree = TagManager.Create("WantsToClimbTree");

		// Token: 0x04007E0A RID: 32266
		public static readonly Tag WantsToPlantSeed = TagManager.Create("WantsToPlantSeed");

		// Token: 0x04007E0B RID: 32267
		public static readonly Tag WantsToForage = TagManager.Create("WantsToForage");

		// Token: 0x04007E0C RID: 32268
		public static readonly Tag WantsToLayEgg = TagManager.Create("WantsToLayEgg");

		// Token: 0x04007E0D RID: 32269
		public static readonly Tag WantsToTendEgg = TagManager.Create("WantsToTendEgg");

		// Token: 0x04007E0E RID: 32270
		public static readonly Tag WantsAHug = TagManager.Create("WantsAHug");

		// Token: 0x04007E0F RID: 32271
		public static readonly Tag WantsConduitConnection = TagManager.Create("WantsConduitConnection");

		// Token: 0x04007E10 RID: 32272
		public static readonly Tag WantsToGoHome = TagManager.Create("WantsToGoHome");

		// Token: 0x04007E11 RID: 32273
		public static readonly Tag WantsToMakeHome = TagManager.Create("WantsToMakeHome");

		// Token: 0x04007E12 RID: 32274
		public static readonly Tag BeeWantsToSleep = TagManager.Create("BeeWantsToSleep");

		// Token: 0x04007E13 RID: 32275
		public static readonly Tag WantsToTendCrops = TagManager.Create("WantsToTendPlants");

		// Token: 0x04007E14 RID: 32276
		public static readonly Tag WantsToStore = TagManager.Create("WantsToStore");

		// Token: 0x04007E15 RID: 32277
		public static readonly Tag WantsToBeckon = TagManager.Create("WantsToBeckon");

		// Token: 0x04007E16 RID: 32278
		public static readonly Tag Flee = TagManager.Create("Flee");

		// Token: 0x04007E17 RID: 32279
		public static readonly Tag Attack = TagManager.Create("Attack");

		// Token: 0x04007E18 RID: 32280
		public static readonly Tag Defend = TagManager.Create("Defend");

		// Token: 0x04007E19 RID: 32281
		public static readonly Tag ReturnToEgg = TagManager.Create("ReturnToEgg");

		// Token: 0x04007E1A RID: 32282
		public static readonly Tag CrabFriend = TagManager.Create("CrabFriend");

		// Token: 0x04007E1B RID: 32283
		public static readonly Tag Die = TagManager.Create("Die");

		// Token: 0x04007E1C RID: 32284
		public static readonly Tag Poop = TagManager.Create("Poop");

		// Token: 0x04007E1D RID: 32285
		public static readonly Tag MoveToLure = TagManager.Create("MoveToLure");

		// Token: 0x04007E1E RID: 32286
		public static readonly Tag Drowning = TagManager.Create("Drowning");

		// Token: 0x04007E1F RID: 32287
		public static readonly Tag Hungry = TagManager.Create("Hungry");

		// Token: 0x04007E20 RID: 32288
		public static readonly Tag Flyer = TagManager.Create("Flyer");

		// Token: 0x04007E21 RID: 32289
		public static readonly Tag FishTrapLure = TagManager.Create("FishTrapLure");

		// Token: 0x04007E22 RID: 32290
		public static readonly Tag FlyersLure = TagManager.Create("MasterLure");

		// Token: 0x04007E23 RID: 32291
		public static readonly Tag Walker = TagManager.Create("Walker");

		// Token: 0x04007E24 RID: 32292
		public static readonly Tag Hoverer = TagManager.Create("Hoverer");

		// Token: 0x04007E25 RID: 32293
		public static readonly Tag Swimmer = TagManager.Create("Swimmer");

		// Token: 0x04007E26 RID: 32294
		public static readonly Tag Fertile = TagManager.Create("Fertile");

		// Token: 0x04007E27 RID: 32295
		public static readonly Tag Submerged = TagManager.Create("Submerged");

		// Token: 0x04007E28 RID: 32296
		public static readonly Tag ExitSubmerged = TagManager.Create("ExitSubmerged");

		// Token: 0x04007E29 RID: 32297
		public static readonly Tag WantsToDropElements = TagManager.Create("WantsToDropElements");

		// Token: 0x04007E2A RID: 32298
		public static readonly Tag OriginallyWild = TagManager.Create("Wild");

		// Token: 0x04007E2B RID: 32299
		public static readonly Tag Wild = TagManager.Create("Wild");

		// Token: 0x04007E2C RID: 32300
		public static readonly Tag Overcrowded = TagManager.Create("Overcrowded");

		// Token: 0x04007E2D RID: 32301
		public static readonly Tag Expecting = TagManager.Create("Expecting");

		// Token: 0x04007E2E RID: 32302
		public static readonly Tag Confined = TagManager.Create("Confined");

		// Token: 0x04007E2F RID: 32303
		public static readonly Tag Digger = TagManager.Create("Digger");

		// Token: 0x04007E30 RID: 32304
		public static readonly Tag Tunnel = TagManager.Create("Tunnel");

		// Token: 0x04007E31 RID: 32305
		public static readonly Tag Builder = TagManager.Create("Builder");

		// Token: 0x04007E32 RID: 32306
		public static readonly Tag ScalesGrown = TagManager.Create("ScalesGrown");

		// Token: 0x04007E33 RID: 32307
		public static readonly Tag CanMolt = TagManager.Create("CanMolt");

		// Token: 0x04007E34 RID: 32308
		public static readonly Tag ReadyToMolt = TagManager.Create("ReadyToMolt");

		// Token: 0x04007E35 RID: 32309
		public static readonly Tag CantReachEgg = TagManager.Create("CantReachEgg");

		// Token: 0x04007E36 RID: 32310
		public static readonly Tag HasNoFoundation = TagManager.Create("HasNoFoundation");

		// Token: 0x04007E37 RID: 32311
		public static readonly Tag Cleaning = TagManager.Create("Cleaning");

		// Token: 0x04007E38 RID: 32312
		public static readonly Tag Happy = TagManager.Create("Happy");

		// Token: 0x04007E39 RID: 32313
		public static readonly Tag Unhappy = TagManager.Create("Unhappy");

		// Token: 0x04007E3A RID: 32314
		public static readonly Tag RequiresMilking = TagManager.Create("RequiresMilking");

		// Token: 0x04007E3B RID: 32315
		public static readonly Tag TargetedPreyBehaviour = TagManager.Create("TargetedPrey");

		// Token: 0x04007E3C RID: 32316
		public static readonly Tag WantsToPollinate = TagManager.Create("WantsToPollinate");

		// Token: 0x04007E3D RID: 32317
		public static readonly Tag Pollinator = TagManager.Create("Pollinator");

		// Token: 0x020029AD RID: 10669
		public static class Species
		{
			// Token: 0x0600D1F2 RID: 53746 RVA: 0x00438740 File Offset: 0x00436940
			public static Tag[] AllSpecies_REFLECTION()
			{
				return GameTags.Reflection_GetTagsInClass(typeof(GameTags.Creatures.Species), BindingFlags.Static | BindingFlags.Public);
			}

			// Token: 0x0400B836 RID: 47158
			public static readonly Tag HatchSpecies = TagManager.Create("HatchSpecies", CREATURES.FAMILY_PLURAL.HATCHSPECIES);

			// Token: 0x0400B837 RID: 47159
			public static readonly Tag LightBugSpecies = TagManager.Create("LightBugSpecies", CREATURES.FAMILY_PLURAL.LIGHTBUGSPECIES);

			// Token: 0x0400B838 RID: 47160
			public static readonly Tag OilFloaterSpecies = TagManager.Create("OilFloaterSpecies", CREATURES.FAMILY_PLURAL.OILFLOATERSPECIES);

			// Token: 0x0400B839 RID: 47161
			public static readonly Tag DreckoSpecies = TagManager.Create("DreckoSpecies", CREATURES.FAMILY_PLURAL.DRECKOSPECIES);

			// Token: 0x0400B83A RID: 47162
			public static readonly Tag GlomSpecies = TagManager.Create("GlomSpecies", CREATURES.FAMILY_PLURAL.GLOMSPECIES);

			// Token: 0x0400B83B RID: 47163
			public static readonly Tag PuftSpecies = TagManager.Create("PuftSpecies", CREATURES.FAMILY_PLURAL.PUFTSPECIES);

			// Token: 0x0400B83C RID: 47164
			public static readonly Tag MosquitoSpecies = TagManager.Create("MosquitoSpecies", CREATURES.FAMILY_PLURAL.MOSQUITOSPECIES);

			// Token: 0x0400B83D RID: 47165
			public static readonly Tag PacuSpecies = TagManager.Create("PacuSpecies", CREATURES.FAMILY_PLURAL.PACUSPECIES);

			// Token: 0x0400B83E RID: 47166
			public static readonly Tag MooSpecies = TagManager.Create("MooSpecies", CREATURES.FAMILY_PLURAL.MOOSPECIES);

			// Token: 0x0400B83F RID: 47167
			public static readonly Tag MoleSpecies = TagManager.Create("MoleSpecies", CREATURES.FAMILY_PLURAL.MOLESPECIES);

			// Token: 0x0400B840 RID: 47168
			public static readonly Tag SquirrelSpecies = TagManager.Create("SquirrelSpecies", CREATURES.FAMILY_PLURAL.SQUIRRELSPECIES);

			// Token: 0x0400B841 RID: 47169
			public static readonly Tag CrabSpecies = TagManager.Create("CrabSpecies", CREATURES.FAMILY_PLURAL.CRABSPECIES);

			// Token: 0x0400B842 RID: 47170
			public static readonly Tag StaterpillarSpecies = TagManager.Create("StaterpillarSpecies", CREATURES.FAMILY_PLURAL.STATERPILLARSPECIES);

			// Token: 0x0400B843 RID: 47171
			public static readonly Tag BeetaSpecies = TagManager.Create("BeetaSpecies", CREATURES.FAMILY_PLURAL.BEETASPECIES);

			// Token: 0x0400B844 RID: 47172
			public static readonly Tag DivergentSpecies = TagManager.Create("DivergentSpecies", CREATURES.FAMILY_PLURAL.DIVERGENTSPECIES);

			// Token: 0x0400B845 RID: 47173
			public static readonly Tag DeerSpecies = TagManager.Create("DeerSpecies", CREATURES.FAMILY_PLURAL.DEERSPECIES);

			// Token: 0x0400B846 RID: 47174
			public static readonly Tag BellySpecies = TagManager.Create("BellySpecies", CREATURES.FAMILY_PLURAL.BELLYSPECIES);

			// Token: 0x0400B847 RID: 47175
			public static readonly Tag SealSpecies = TagManager.Create("SealSpecies", CREATURES.FAMILY_PLURAL.SEALSPECIES);

			// Token: 0x0400B848 RID: 47176
			public static readonly Tag RaptorSpecies = TagManager.Create("RaptorSpecies", CREATURES.FAMILY_PLURAL.RAPTORSPECIES);

			// Token: 0x0400B849 RID: 47177
			public static readonly Tag ChameleonSpecies = TagManager.Create("ChameleonSpecies", CREATURES.FAMILY_PLURAL.CHAMELEONSPECIES);

			// Token: 0x0400B84A RID: 47178
			public static readonly Tag PrehistoricPacuSpecies = TagManager.Create("PrehistoricPacuSpecies", CREATURES.FAMILY_PLURAL.PREHISTORICPACUSPECIES);

			// Token: 0x0400B84B RID: 47179
			public static readonly Tag StegoSpecies = TagManager.Create("StegoSpecies", CREATURES.FAMILY_PLURAL.STEGOSPECIES);

			// Token: 0x0400B84C RID: 47180
			public static readonly Tag ButterflySpecies = TagManager.Create("ButterflySpecies", CREATURES.FAMILY_PLURAL.BUTTERFLYSPECIES);
		}

		// Token: 0x020029AE RID: 10670
		public static class Behaviours
		{
			// Token: 0x0400B84D RID: 47181
			public static readonly Tag HarvestHiveBehaviour = TagManager.Create("HarvestHiveBehaviour");

			// Token: 0x0400B84E RID: 47182
			public static readonly Tag GrowUpBehaviour = TagManager.Create("GrowUpBehaviour");

			// Token: 0x0400B84F RID: 47183
			public static readonly Tag SleepBehaviour = TagManager.Create("SleepBehaviour");

			// Token: 0x0400B850 RID: 47184
			public static readonly Tag CallAdultBehaviour = TagManager.Create("CallAdultBehaviour");

			// Token: 0x0400B851 RID: 47185
			public static readonly Tag SearchForEggBehaviour = TagManager.Create("SearchForEggBehaviour");

			// Token: 0x0400B852 RID: 47186
			public static readonly Tag PlayInterruptAnim = TagManager.Create("PlayInterruptAnim");

			// Token: 0x0400B853 RID: 47187
			public static readonly Tag DisableCreature = TagManager.Create("DisableCreature");

			// Token: 0x0400B854 RID: 47188
			public static readonly Tag CritterEmoteBehaviour = TagManager.Create("CritterEmoteBehaviour");

			// Token: 0x0400B855 RID: 47189
			public static readonly Tag CritterRoarBehaviour = TagManager.Create("CritterRoarBehaviour");
		}
	}

	// Token: 0x0200197B RID: 6523
	public static class StoragesIds
	{
		// Token: 0x04007E3E RID: 32318
		public static readonly Tag DefaultStorage = TagManager.Create("Storage");

		// Token: 0x04007E3F RID: 32319
		public static readonly Tag BionicBatteryStorage = TagManager.Create("BionicBatteryStorage");

		// Token: 0x04007E40 RID: 32320
		public static readonly Tag BionicUpgradeStorage = TagManager.Create("BionicUpgradeStorage");

		// Token: 0x04007E41 RID: 32321
		public static readonly Tag BionicOxygenTankStorage = TagManager.Create("BionicOxygenTankStorage");
	}

	// Token: 0x0200197C RID: 6524
	public static class Minions
	{
		// Token: 0x020029AF RID: 10671
		public static class Models
		{
			// Token: 0x0600D1F5 RID: 53749 RVA: 0x00438A34 File Offset: 0x00436C34
			public static string GetModelTooltipForTag(Tag modelTag)
			{
				if (modelTag == GameTags.Minions.Models.Bionic)
				{
					return DUPLICANTS.MODEL.BIONIC.NAME_TOOLTIP;
				}
				return "";
			}

			// Token: 0x0400B856 RID: 47190
			public static readonly Tag Standard = TagManager.Create("Minion", DUPLICANTS.MODEL.STANDARD.NAME);

			// Token: 0x0400B857 RID: 47191
			public static readonly Tag Bionic = TagManager.Create("BionicMinion", DUPLICANTS.MODEL.BIONIC.NAME);

			// Token: 0x0400B858 RID: 47192
			public static readonly Tag[] AllModels = new Tag[]
			{
				GameTags.Minions.Models.Standard,
				GameTags.Minions.Models.Bionic
			};
		}
	}

	// Token: 0x0200197D RID: 6525
	public static class CodexCategories
	{
		// Token: 0x0600A272 RID: 41586 RVA: 0x003AEFEC File Offset: 0x003AD1EC
		public static string GetCategoryLabelText(Tag tag)
		{
			StringEntry entry = null;
			string text = "STRINGS.CODEX.CATEGORIES." + tag.ToString().ToUpper() + ".NAME";
			if (!Strings.TryGet(new StringKey(text), out entry))
			{
				return ROOMS.CRITERIA.IN_CODE_ERROR.text.Replace("{0}", text);
			}
			return entry;
		}

		// Token: 0x04007E42 RID: 32322
		public static List<Tag> AllTags = new List<Tag>();

		// Token: 0x04007E43 RID: 32323
		public static Tag CreatureRelocator = GameTags.CodexCategories.AllTags.AddAndReturn(TagManager.Create("CreatureRelocator"));

		// Token: 0x04007E44 RID: 32324
		public static Tag FarmBuilding = GameTags.CodexCategories.AllTags.AddAndReturn("FarmBuilding".ToTag());

		// Token: 0x04007E45 RID: 32325
		public static Tag BionicBuilding = GameTags.CodexCategories.AllTags.AddAndReturn("BionicBuilding".ToTag());

		// Token: 0x04007E46 RID: 32326
		public static Tag Ornament = GameTags.CodexCategories.AllTags.AddAndReturn("Ornament".ToTag());
	}

	// Token: 0x0200197E RID: 6526
	public static class Robots
	{
		// Token: 0x020029B0 RID: 10672
		public static class Models
		{
			// Token: 0x0400B859 RID: 47193
			public static readonly Tag SweepBot = TagManager.Create("SweepBot");

			// Token: 0x0400B85A RID: 47194
			public static readonly Tag ScoutRover = TagManager.Create("ScoutRover");

			// Token: 0x0400B85B RID: 47195
			public static readonly Tag MorbRover = TagManager.Create("MorbRover");

			// Token: 0x0400B85C RID: 47196
			public static readonly Tag FetchDrone = TagManager.Create("FetchDrone");

			// Token: 0x0400B85D RID: 47197
			public static readonly Tag RemoteWorker = TagManager.Create("RemoteWorker");
		}

		// Token: 0x020029B1 RID: 10673
		public static class Behaviours
		{
			// Token: 0x0400B85E RID: 47198
			public static readonly Tag UnloadBehaviour = TagManager.Create("UnloadBehaviour");

			// Token: 0x0400B85F RID: 47199
			public static readonly Tag RechargeBehaviour = TagManager.Create("RechargeBehaviour");

			// Token: 0x0400B860 RID: 47200
			public static readonly Tag EmoteBehaviour = TagManager.Create("EmoteBehaviour");

			// Token: 0x0400B861 RID: 47201
			public static readonly Tag TrappedBehaviour = TagManager.Create("TrappedBehaviour");

			// Token: 0x0400B862 RID: 47202
			public static readonly Tag NoElectroBank = TagManager.Create("NoElectroBank");

			// Token: 0x0400B863 RID: 47203
			public static readonly Tag HasDoorPermissions = TagManager.Create("HasDoorPermissions");
		}
	}

	// Token: 0x0200197F RID: 6527
	public class Search
	{
		// Token: 0x04007E47 RID: 32327
		public static readonly Tag Tile = TagManager.Create("Tile");

		// Token: 0x04007E48 RID: 32328
		public static readonly Tag Ladder = TagManager.Create("Ladder");

		// Token: 0x04007E49 RID: 32329
		public static readonly Tag Powered = TagManager.Create("Powered");

		// Token: 0x04007E4A RID: 32330
		public static readonly Tag Rocket = TagManager.Create("Rocket");

		// Token: 0x04007E4B RID: 32331
		public static readonly Tag Monument = TagManager.Create("Monument");

		// Token: 0x04007E4C RID: 32332
		public static readonly Tag Farming = TagManager.Create("Farming");

		// Token: 0x04007E4D RID: 32333
		public static readonly Tag Cooking = TagManager.Create("Cooking");
	}
}
