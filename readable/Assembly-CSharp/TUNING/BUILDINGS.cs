using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02000FCB RID: 4043
	public class BUILDINGS
	{
		// Token: 0x04005DF5 RID: 24053
		public const float DEFAULT_STORAGE_CAPACITY = 2000f;

		// Token: 0x04005DF6 RID: 24054
		public const float STANDARD_MANUAL_REFILL_LEVEL = 0.2f;

		// Token: 0x04005DF7 RID: 24055
		public const float MASS_TEMPERATURE_SCALE = 0.2f;

		// Token: 0x04005DF8 RID: 24056
		public const float AIRCONDITIONER_TEMPDELTA = -14f;

		// Token: 0x04005DF9 RID: 24057
		public const float MAX_ENVIRONMENT_DELTA = -50f;

		// Token: 0x04005DFA RID: 24058
		public const float COMPOST_FLIP_TIME = 20f;

		// Token: 0x04005DFB RID: 24059
		public const int TUBE_LAUNCHER_MAX_CHARGES = 3;

		// Token: 0x04005DFC RID: 24060
		public const float TUBE_LAUNCHER_RECHARGE_TIME = 10f;

		// Token: 0x04005DFD RID: 24061
		public const float TUBE_LAUNCHER_WORK_TIME = 1f;

		// Token: 0x04005DFE RID: 24062
		public const float SMELTER_INGOT_INPUTKG = 500f;

		// Token: 0x04005DFF RID: 24063
		public const float SMELTER_INGOT_OUTPUTKG = 100f;

		// Token: 0x04005E00 RID: 24064
		public const float SMELTER_FABRICATIONTIME = 120f;

		// Token: 0x04005E01 RID: 24065
		public const float GEOREFINERY_SLAB_INPUTKG = 1000f;

		// Token: 0x04005E02 RID: 24066
		public const float GEOREFINERY_SLAB_OUTPUTKG = 200f;

		// Token: 0x04005E03 RID: 24067
		public const float GEOREFINERY_FABRICATIONTIME = 120f;

		// Token: 0x04005E04 RID: 24068
		public const float MASS_BURN_RATE_HYDROGENGENERATOR = 0.1f;

		// Token: 0x04005E05 RID: 24069
		public const float COOKER_FOOD_TEMPERATURE = 368.15f;

		// Token: 0x04005E06 RID: 24070
		public const float OVERHEAT_DAMAGE_INTERVAL = 7.5f;

		// Token: 0x04005E07 RID: 24071
		public const float MIN_BUILD_TEMPERATURE = 0f;

		// Token: 0x04005E08 RID: 24072
		public const float MAX_BUILD_TEMPERATURE = 318.15f;

		// Token: 0x04005E09 RID: 24073
		public const float MELTDOWN_TEMPERATURE = 533.15f;

		// Token: 0x04005E0A RID: 24074
		public const float REPAIR_FORCE_TEMPERATURE = 293.15f;

		// Token: 0x04005E0B RID: 24075
		public const int REPAIR_EFFECTIVENESS_BASE = 10;

		// Token: 0x04005E0C RID: 24076
		public static Dictionary<string, string> PLANSUBCATEGORYSORTING = new Dictionary<string, string>
		{
			{
				"Ladder",
				"ladders"
			},
			{
				"FirePole",
				"ladders"
			},
			{
				"LadderFast",
				"ladders"
			},
			{
				"Tile",
				"tiles"
			},
			{
				"SnowTile",
				"tiles"
			},
			{
				"WoodTile",
				"tiles"
			},
			{
				"GasPermeableMembrane",
				"tiles"
			},
			{
				"MeshTile",
				"tiles"
			},
			{
				"InsulationTile",
				"tiles"
			},
			{
				"PlasticTile",
				"tiles"
			},
			{
				"MetalTile",
				"tiles"
			},
			{
				"GlassTile",
				"tiles"
			},
			{
				"StorageTile",
				"tiles"
			},
			{
				"BunkerTile",
				"tiles"
			},
			{
				"ExteriorWall",
				"tiles"
			},
			{
				"CarpetTile",
				"tiles"
			},
			{
				"ExobaseHeadquarters",
				"printingpods"
			},
			{
				"Door",
				"doors"
			},
			{
				"WoodenDoor",
				"doors"
			},
			{
				"ManualPressureDoor",
				"doors"
			},
			{
				"InsulatedDoor",
				"doors"
			},
			{
				"PressureDoor",
				"doors"
			},
			{
				"BunkerDoor",
				"doors"
			},
			{
				"StorageLocker",
				"storage"
			},
			{
				"StorageLockerSmart",
				"storage"
			},
			{
				"LiquidReservoir",
				"storage"
			},
			{
				"GasReservoir",
				"storage"
			},
			{
				"ObjectDispenser",
				"storage"
			},
			{
				"TravelTube",
				"transport"
			},
			{
				"TravelTubeEntrance",
				"transport"
			},
			{
				"TravelTubeWallBridge",
				"transport"
			},
			{
				RemoteWorkerDockConfig.ID,
				"operations"
			},
			{
				RemoteWorkTerminalConfig.ID,
				"operations"
			},
			{
				"MineralDeoxidizer",
				"producers"
			},
			{
				"SublimationStation",
				"producers"
			},
			{
				"Oxysconce",
				"producers"
			},
			{
				"Electrolyzer",
				"producers"
			},
			{
				"RustDeoxidizer",
				"producers"
			},
			{
				"AirFilter",
				"scrubbers"
			},
			{
				"CO2Scrubber",
				"scrubbers"
			},
			{
				"AlgaeHabitat",
				"scrubbers"
			},
			{
				"DevGenerator",
				"generators"
			},
			{
				"ManualGenerator",
				"generators"
			},
			{
				"Generator",
				"generators"
			},
			{
				"WoodGasGenerator",
				"generators"
			},
			{
				"PeatGenerator",
				"generators"
			},
			{
				"HydrogenGenerator",
				"generators"
			},
			{
				"MethaneGenerator",
				"generators"
			},
			{
				"PetroleumGenerator",
				"generators"
			},
			{
				"SteamTurbine",
				"generators"
			},
			{
				"SteamTurbine2",
				"generators"
			},
			{
				"SolarPanel",
				"generators"
			},
			{
				"Wire",
				"wires"
			},
			{
				"WireBridge",
				"wires"
			},
			{
				"HighWattageWire",
				"wires"
			},
			{
				"WireBridgeHighWattage",
				"wires"
			},
			{
				"WireRefined",
				"wires"
			},
			{
				"WireRefinedBridge",
				"wires"
			},
			{
				"WireRefinedHighWattage",
				"wires"
			},
			{
				"WireRefinedBridgeHighWattage",
				"wires"
			},
			{
				"Battery",
				"batteries"
			},
			{
				"BatteryMedium",
				"batteries"
			},
			{
				"BatterySmart",
				"batteries"
			},
			{
				"ElectrobankCharger",
				"electrobankbuildings"
			},
			{
				"SmallElectrobankDischarger",
				"electrobankbuildings"
			},
			{
				"LargeElectrobankDischarger",
				"electrobankbuildings"
			},
			{
				"PowerTransformerSmall",
				"powercontrol"
			},
			{
				"PowerTransformer",
				"powercontrol"
			},
			{
				SwitchConfig.ID,
				"switches"
			},
			{
				LogicPowerRelayConfig.ID,
				"switches"
			},
			{
				TemperatureControlledSwitchConfig.ID,
				"switches"
			},
			{
				PressureSwitchLiquidConfig.ID,
				"switches"
			},
			{
				PressureSwitchGasConfig.ID,
				"switches"
			},
			{
				"MicrobeMusher",
				"cooking"
			},
			{
				"CookingStation",
				"cooking"
			},
			{
				"Deepfryer",
				"cooking"
			},
			{
				"GourmetCookingStation",
				"cooking"
			},
			{
				"SpiceGrinder",
				"cooking"
			},
			{
				"FoodDehydrator",
				"cooking"
			},
			{
				"FoodRehydrator",
				"cooking"
			},
			{
				"Smoker",
				"cooking"
			},
			{
				"PlanterBox",
				"farming"
			},
			{
				"FarmTile",
				"farming"
			},
			{
				"HydroponicFarm",
				"farming"
			},
			{
				"RationBox",
				"storage"
			},
			{
				"Refrigerator",
				"storage"
			},
			{
				"CreatureDeliveryPoint",
				"ranching"
			},
			{
				"CritterDropOff",
				"ranching"
			},
			{
				"CritterPickUp",
				"ranching"
			},
			{
				"FishDeliveryPoint",
				"ranching"
			},
			{
				"CreatureFeeder",
				"ranching"
			},
			{
				"FishFeeder",
				"ranching"
			},
			{
				"MilkFeeder",
				"ranching"
			},
			{
				"EggIncubator",
				"ranching"
			},
			{
				"EggCracker",
				"ranching"
			},
			{
				"CreatureGroundTrap",
				"ranching"
			},
			{
				"CreatureAirTrap",
				"ranching"
			},
			{
				"WaterTrap",
				"ranching"
			},
			{
				"CritterCondo",
				"ranching"
			},
			{
				"UnderwaterCritterCondo",
				"ranching"
			},
			{
				"AirBorneCritterCondo",
				"ranching"
			},
			{
				"Outhouse",
				"washroom"
			},
			{
				"FlushToilet",
				"washroom"
			},
			{
				"WallToilet",
				"washroom"
			},
			{
				ShowerConfig.ID,
				"washroom"
			},
			{
				"GunkEmptier",
				"washroom"
			},
			{
				"LiquidConduit",
				"pipes"
			},
			{
				"InsulatedLiquidConduit",
				"pipes"
			},
			{
				"LiquidConduitRadiant",
				"pipes"
			},
			{
				"LiquidConduitBridge",
				"pipes"
			},
			{
				"ContactConductivePipeBridge",
				"pipes"
			},
			{
				"LiquidVent",
				"pipes"
			},
			{
				"LiquidPump",
				"pumps"
			},
			{
				"LiquidMiniPump",
				"pumps"
			},
			{
				"LiquidPumpingStation",
				"pumps"
			},
			{
				"DevPumpLiquid",
				"pumps"
			},
			{
				"BottleEmptier",
				"valves"
			},
			{
				"LiquidFilter",
				"valves"
			},
			{
				"LiquidConduitPreferentialFlow",
				"valves"
			},
			{
				"LiquidConduitOverflow",
				"valves"
			},
			{
				"LiquidValve",
				"valves"
			},
			{
				"LiquidLogicValve",
				"valves"
			},
			{
				"LiquidLimitValve",
				"valves"
			},
			{
				"LiquidBottler",
				"valves"
			},
			{
				"BottleEmptierConduitLiquid",
				"valves"
			},
			{
				LiquidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortLiquid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortLiquidUnloader",
				"buildmenuports"
			},
			{
				"GasConduit",
				"pipes"
			},
			{
				"InsulatedGasConduit",
				"pipes"
			},
			{
				"GasConduitRadiant",
				"pipes"
			},
			{
				"GasConduitBridge",
				"pipes"
			},
			{
				"GasVent",
				"pipes"
			},
			{
				"GasVentHighPressure",
				"pipes"
			},
			{
				"GasPump",
				"pumps"
			},
			{
				"GasMiniPump",
				"pumps"
			},
			{
				"DevPumpGas",
				"pumps"
			},
			{
				"GasBottler",
				"valves"
			},
			{
				"BottleEmptierGas",
				"valves"
			},
			{
				"BottleEmptierConduitGas",
				"valves"
			},
			{
				"GasFilter",
				"valves"
			},
			{
				"GasConduitPreferentialFlow",
				"valves"
			},
			{
				"GasConduitOverflow",
				"valves"
			},
			{
				"GasValve",
				"valves"
			},
			{
				"GasLogicValve",
				"valves"
			},
			{
				"GasLimitValve",
				"valves"
			},
			{
				GasConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortGas",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortGasUnloader",
				"buildmenuports"
			},
			{
				"Compost",
				"organic"
			},
			{
				"FertilizerMaker",
				"organic"
			},
			{
				"AlgaeDistillery",
				"organic"
			},
			{
				"EthanolDistillery",
				"organic"
			},
			{
				"SludgePress",
				"organic"
			},
			{
				"MilkFatSeparator",
				"organic"
			},
			{
				"MilkPress",
				"organic"
			},
			{
				"IceKettle",
				"materials"
			},
			{
				"WaterPurifier",
				"materials"
			},
			{
				"Desalinator",
				"materials"
			},
			{
				"RockCrusher",
				"materials"
			},
			{
				"Kiln",
				"materials"
			},
			{
				"FabricatedWoodMaker",
				"materials"
			},
			{
				"MetalRefinery",
				"materials"
			},
			{
				"GlassForge",
				"materials"
			},
			{
				"OilRefinery",
				"oil"
			},
			{
				"Polymerizer",
				"oil"
			},
			{
				"OxyliteRefinery",
				"advanced"
			},
			{
				"ChemicalRefinery",
				"advanced"
			},
			{
				"SupermaterialRefinery",
				"advanced"
			},
			{
				"DiamondPress",
				"advanced"
			},
			{
				"Chlorinator",
				"advanced"
			},
			{
				"WashBasin",
				"hygiene"
			},
			{
				"WashSink",
				"hygiene"
			},
			{
				"HandSanitizer",
				"hygiene"
			},
			{
				"DecontaminationShower",
				"hygiene"
			},
			{
				"Apothecary",
				"medical"
			},
			{
				"DoctorStation",
				"medical"
			},
			{
				"AdvancedDoctorStation",
				"medical"
			},
			{
				"MedicalCot",
				"medical"
			},
			{
				"DevLifeSupport",
				"medical"
			},
			{
				"MassageTable",
				"wellness"
			},
			{
				"Grave",
				"wellness"
			},
			{
				"OilChanger",
				"wellness"
			},
			{
				"Bed",
				"beds"
			},
			{
				"LuxuryBed",
				"beds"
			},
			{
				LadderBedConfig.ID,
				"beds"
			},
			{
				"FloorLamp",
				"lights"
			},
			{
				"CeilingLight",
				"lights"
			},
			{
				"SunLamp",
				"lights"
			},
			{
				"DevLightGenerator",
				"lights"
			},
			{
				"MercuryCeilingLight",
				"lights"
			},
			{
				"DiningTable",
				"dining"
			},
			{
				"MultiMinionDiningTable",
				"dining"
			},
			{
				"WaterCooler",
				"recreation"
			},
			{
				"Phonobox",
				"recreation"
			},
			{
				"ArcadeMachine",
				"recreation"
			},
			{
				"EspressoMachine",
				"recreation"
			},
			{
				"HotTub",
				"recreation"
			},
			{
				"MechanicalSurfboard",
				"recreation"
			},
			{
				"Sauna",
				"recreation"
			},
			{
				"Juicer",
				"recreation"
			},
			{
				"SodaFountain",
				"recreation"
			},
			{
				"BeachChair",
				"recreation"
			},
			{
				"VerticalWindTunnel",
				"recreation"
			},
			{
				"Telephone",
				"recreation"
			},
			{
				"FlowerVase",
				"decor"
			},
			{
				"FlowerVaseWall",
				"decor"
			},
			{
				"FlowerVaseHanging",
				"decor"
			},
			{
				"FlowerVaseHangingFancy",
				"decor"
			},
			{
				PixelPackConfig.ID,
				"decor"
			},
			{
				"SmallSculpture",
				"decor"
			},
			{
				"Sculpture",
				"decor"
			},
			{
				"IceSculpture",
				"decor"
			},
			{
				"MarbleSculpture",
				"decor"
			},
			{
				"MetalSculpture",
				"decor"
			},
			{
				"WoodSculpture",
				"decor"
			},
			{
				"FossilSculpture",
				"decor"
			},
			{
				"CeilingFossilSculpture",
				"decor"
			},
			{
				"CrownMoulding",
				"decor"
			},
			{
				"CornerMoulding",
				"decor"
			},
			{
				"Canvas",
				"decor"
			},
			{
				"CanvasWide",
				"decor"
			},
			{
				"CanvasTall",
				"decor"
			},
			{
				"ItemPedestal",
				"decor"
			},
			{
				"Shelf",
				"decor"
			},
			{
				"ParkSign",
				"decor"
			},
			{
				"MonumentBottom",
				"decor"
			},
			{
				"MonumentMiddle",
				"decor"
			},
			{
				"MonumentTop",
				"decor"
			},
			{
				"ResearchCenter",
				"research"
			},
			{
				"AdvancedResearchCenter",
				"research"
			},
			{
				"GeoTuner",
				"research"
			},
			{
				"NuclearResearchCenter",
				"research"
			},
			{
				"OrbitalResearchCenter",
				"research"
			},
			{
				"CosmicResearchCenter",
				"research"
			},
			{
				"DLC1CosmicResearchCenter",
				"research"
			},
			{
				"DataMiner",
				"research"
			},
			{
				"ArtifactAnalysisStation",
				"archaeology"
			},
			{
				"MissileFabricator",
				"meteordefense"
			},
			{
				"AstronautTrainingCenter",
				"exploration"
			},
			{
				"PowerControlStation",
				"industrialstation"
			},
			{
				"ResetSkillsStation",
				"industrialstation"
			},
			{
				"RoleStation",
				"workstations"
			},
			{
				"RanchStation",
				"ranching"
			},
			{
				"ShearingStation",
				"ranching"
			},
			{
				"MilkingStation",
				"ranching"
			},
			{
				"FarmStation",
				"farming"
			},
			{
				"GeneticAnalysisStation",
				"farming"
			},
			{
				"CraftingTable",
				"manufacturing"
			},
			{
				"AdvancedCraftingTable",
				"manufacturing"
			},
			{
				"ClothingFabricator",
				"manufacturing"
			},
			{
				"ClothingAlterationStation",
				"manufacturing"
			},
			{
				"SuitFabricator",
				"manufacturing"
			},
			{
				"OxygenMaskMarker",
				"equipment"
			},
			{
				"OxygenMaskLocker",
				"equipment"
			},
			{
				"SuitMarker",
				"equipment"
			},
			{
				"SuitLocker",
				"equipment"
			},
			{
				"JetSuitMarker",
				"equipment"
			},
			{
				"JetSuitLocker",
				"equipment"
			},
			{
				"MissileLauncher",
				"missiles"
			},
			{
				"LeadSuitMarker",
				"equipment"
			},
			{
				"LeadSuitLocker",
				"equipment"
			},
			{
				"Campfire",
				"temperature"
			},
			{
				"DevHeater",
				"temperature"
			},
			{
				"SpaceHeater",
				"temperature"
			},
			{
				"LiquidHeater",
				"temperature"
			},
			{
				"LiquidConditioner",
				"temperature"
			},
			{
				"LiquidCooledFan",
				"temperature"
			},
			{
				"IceCooledFan",
				"temperature"
			},
			{
				"IceMachine",
				"temperature"
			},
			{
				"AirConditioner",
				"temperature"
			},
			{
				"ThermalBlock",
				"temperature"
			},
			{
				"OreScrubber",
				"sanitation"
			},
			{
				"OilWellCap",
				"oil"
			},
			{
				"SweepBotStation",
				"sanitation"
			},
			{
				"LogicWire",
				"wires"
			},
			{
				"LogicWireBridge",
				"wires"
			},
			{
				"LogicRibbon",
				"wires"
			},
			{
				"LogicRibbonBridge",
				"wires"
			},
			{
				LogicRibbonReaderConfig.ID,
				"wires"
			},
			{
				LogicRibbonWriterConfig.ID,
				"wires"
			},
			{
				"LogicDuplicantSensor",
				"sensors"
			},
			{
				LogicPressureSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicPressureSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				LogicLightSensorConfig.ID,
				"sensors"
			},
			{
				LogicWattageSensorConfig.ID,
				"sensors"
			},
			{
				LogicTimeOfDaySensorConfig.ID,
				"sensors"
			},
			{
				LogicTimerSensorConfig.ID,
				"sensors"
			},
			{
				LogicDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicCritterCountSensorConfig.ID,
				"sensors"
			},
			{
				LogicRadiationSensorConfig.ID,
				"sensors"
			},
			{
				LogicHEPSensorConfig.ID,
				"sensors"
			},
			{
				CometDetectorConfig.ID,
				"sensors"
			},
			{
				LogicCounterConfig.ID,
				"logicmanager"
			},
			{
				"Checkpoint",
				"logicmanager"
			},
			{
				LogicAlarmConfig.ID,
				"logicmanager"
			},
			{
				LogicHammerConfig.ID,
				"logicaudio"
			},
			{
				LogicSwitchConfig.ID,
				"switches"
			},
			{
				"FloorSwitch",
				"switches"
			},
			{
				"LogicGateNOT",
				"logicgates"
			},
			{
				"LogicGateAND",
				"logicgates"
			},
			{
				"LogicGateOR",
				"logicgates"
			},
			{
				"LogicGateBUFFER",
				"logicgates"
			},
			{
				"LogicGateFILTER",
				"logicgates"
			},
			{
				"LogicGateXOR",
				"logicgates"
			},
			{
				LogicMemoryConfig.ID,
				"logicgates"
			},
			{
				"LogicGateMultiplexer",
				"logicgates"
			},
			{
				"LogicGateDemultiplexer",
				"logicgates"
			},
			{
				"LogicInterasteroidSender",
				"transmissions"
			},
			{
				"LogicInterasteroidReceiver",
				"transmissions"
			},
			{
				"SolidConduit",
				"conveyancestructures"
			},
			{
				"SolidConduitBridge",
				"conveyancestructures"
			},
			{
				"SolidConduitInbox",
				"conveyancestructures"
			},
			{
				"SolidConduitOutbox",
				"conveyancestructures"
			},
			{
				"SolidFilter",
				"conveyancestructures"
			},
			{
				"SolidVent",
				"conveyancestructures"
			},
			{
				"DevPumpSolid",
				"pumps"
			},
			{
				"SolidLogicValve",
				"valves"
			},
			{
				"SolidLimitValve",
				"valves"
			},
			{
				SolidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"AutoMiner",
				"automated"
			},
			{
				"SolidTransferArm",
				"automated"
			},
			{
				"ModularLaunchpadPortSolid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortSolidUnloader",
				"buildmenuports"
			},
			{
				"Telescope",
				"telescopes"
			},
			{
				"ClusterTelescope",
				"telescopes"
			},
			{
				"ClusterTelescopeEnclosed",
				"telescopes"
			},
			{
				"LaunchPad",
				"rocketstructures"
			},
			{
				"Gantry",
				"rocketstructures"
			},
			{
				"ModularLaunchpadPortBridge",
				"rocketstructures"
			},
			{
				"RailGun",
				"fittings"
			},
			{
				"RailGunPayloadOpener",
				"fittings"
			},
			{
				"LandingBeacon",
				"rocketnav"
			},
			{
				"SteamEngine",
				"engines"
			},
			{
				"KeroseneEngine",
				"engines"
			},
			{
				"BiodieselEngine",
				"engines"
			},
			{
				"HydrogenEngine",
				"engines"
			},
			{
				"SolidBooster",
				"engines"
			},
			{
				"LiquidFuelTank",
				"tanks"
			},
			{
				"OxidizerTank",
				"tanks"
			},
			{
				"OxidizerTankLiquid",
				"tanks"
			},
			{
				"CargoBay",
				"cargo"
			},
			{
				"GasCargoBay",
				"cargo"
			},
			{
				"LiquidCargoBay",
				"cargo"
			},
			{
				"SpecialCargoBay",
				"cargo"
			},
			{
				"CommandModule",
				"rocketnav"
			},
			{
				RocketControlStationConfig.ID,
				"rocketnav"
			},
			{
				LogicClusterLocationSensorConfig.ID,
				"rocketnav"
			},
			{
				"MissionControl",
				"rocketnav"
			},
			{
				"MissionControlCluster",
				"rocketnav"
			},
			{
				"RoboPilotCommandModule",
				"rocketnav"
			},
			{
				"TouristModule",
				"module"
			},
			{
				"ResearchModule",
				"module"
			},
			{
				"RocketInteriorPowerPlug",
				"fittings"
			},
			{
				"RocketInteriorLiquidInput",
				"fittings"
			},
			{
				"RocketInteriorLiquidOutput",
				"fittings"
			},
			{
				"RocketInteriorGasInput",
				"fittings"
			},
			{
				"RocketInteriorGasOutput",
				"fittings"
			},
			{
				"RocketInteriorSolidInput",
				"fittings"
			},
			{
				"RocketInteriorSolidOutput",
				"fittings"
			},
			{
				"ManualHighEnergyParticleSpawner",
				"producers"
			},
			{
				"HighEnergyParticleSpawner",
				"producers"
			},
			{
				"DevHEPSpawner",
				"producers"
			},
			{
				"HighEnergyParticleRedirector",
				"transmissions"
			},
			{
				"HEPBattery",
				"batteries"
			},
			{
				"HEPBridgeTile",
				"transmissions"
			},
			{
				"NuclearReactor",
				"producers"
			},
			{
				"UraniumCentrifuge",
				"producers"
			},
			{
				"RadiationLight",
				"producers"
			},
			{
				"DevRadiationGenerator",
				"producers"
			}
		};

		// Token: 0x04005E0D RID: 24077
		public static List<PlanScreen.PlanInfo> PLANORDER = new List<PlanScreen.PlanInfo>
		{
			new PlanScreen.PlanInfo(new HashedString("Base"), false, new List<string>
			{
				"Ladder",
				"FirePole",
				"LadderFast",
				"Tile",
				"SnowTile",
				"WoodTile",
				"GasPermeableMembrane",
				"MeshTile",
				"InsulationTile",
				"PlasticTile",
				"MetalTile",
				"GlassTile",
				"StorageTile",
				"BunkerTile",
				"CarpetTile",
				"ExteriorWall",
				"ExobaseHeadquarters",
				"Door",
				"WoodenDoor",
				"ManualPressureDoor",
				"InsulatedDoor",
				"PressureDoor",
				"BunkerDoor",
				"StorageLocker",
				"StorageLockerSmart",
				"LiquidReservoir",
				"GasReservoir",
				"ObjectDispenser",
				"TravelTube",
				"TravelTubeEntrance",
				"TravelTubeWallBridge"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Oxygen"), false, new List<string>
			{
				"MineralDeoxidizer",
				"SublimationStation",
				"Oxysconce",
				"AlgaeHabitat",
				"AirFilter",
				"CO2Scrubber",
				"Electrolyzer",
				"RustDeoxidizer"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Power"), false, new List<string>
			{
				"DevGenerator",
				"ManualGenerator",
				"Generator",
				"WoodGasGenerator",
				"PeatGenerator",
				"HydrogenGenerator",
				"MethaneGenerator",
				"PetroleumGenerator",
				"SteamTurbine",
				"SteamTurbine2",
				"SolarPanel",
				"Wire",
				"WireBridge",
				"HighWattageWire",
				"WireBridgeHighWattage",
				"WireRefined",
				"WireRefinedBridge",
				"WireRefinedHighWattage",
				"WireRefinedBridgeHighWattage",
				"Battery",
				"BatteryMedium",
				"BatterySmart",
				"ElectrobankCharger",
				"SmallElectrobankDischarger",
				"LargeElectrobankDischarger",
				"PowerTransformerSmall",
				"PowerTransformer",
				SwitchConfig.ID,
				LogicPowerRelayConfig.ID,
				TemperatureControlledSwitchConfig.ID,
				PressureSwitchLiquidConfig.ID,
				PressureSwitchGasConfig.ID
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Food"), false, new List<string>
			{
				"MicrobeMusher",
				"CookingStation",
				"Deepfryer",
				"GourmetCookingStation",
				"SpiceGrinder",
				"FoodDehydrator",
				"FoodRehydrator",
				"Smoker",
				"PlanterBox",
				"FarmTile",
				"HydroponicFarm",
				"RationBox",
				"Refrigerator",
				"CreatureDeliveryPoint",
				"CritterPickUp",
				"CritterDropOff",
				"FishDeliveryPoint",
				"CreatureFeeder",
				"FishFeeder",
				"MilkFeeder",
				"EggIncubator",
				"EggCracker",
				"CreatureGroundTrap",
				"WaterTrap",
				"CreatureAirTrap",
				"CritterCondo",
				"UnderwaterCritterCondo",
				"AirBorneCritterCondo"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Plumbing"), false, new List<string>
			{
				"DevPumpLiquid",
				"Outhouse",
				"FlushToilet",
				"WallToilet",
				ShowerConfig.ID,
				"GunkEmptier",
				"LiquidPumpingStation",
				"BottleEmptier",
				"BottleEmptierConduitLiquid",
				"LiquidBottler",
				"LiquidConduit",
				"InsulatedLiquidConduit",
				"LiquidConduitRadiant",
				"LiquidConduitBridge",
				"LiquidConduitPreferentialFlow",
				"LiquidConduitOverflow",
				"LiquidPump",
				"LiquidMiniPump",
				"LiquidVent",
				"LiquidFilter",
				"LiquidValve",
				"LiquidLogicValve",
				"LiquidLimitValve",
				LiquidConduitElementSensorConfig.ID,
				LiquidConduitDiseaseSensorConfig.ID,
				LiquidConduitTemperatureSensorConfig.ID,
				"ModularLaunchpadPortLiquid",
				"ModularLaunchpadPortLiquidUnloader",
				"ContactConductivePipeBridge"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("HVAC"), false, new List<string>
			{
				"DevPumpGas",
				"GasConduit",
				"InsulatedGasConduit",
				"GasConduitRadiant",
				"GasConduitBridge",
				"GasConduitPreferentialFlow",
				"GasConduitOverflow",
				"GasPump",
				"GasMiniPump",
				"GasVent",
				"GasVentHighPressure",
				"GasFilter",
				"GasValve",
				"GasLogicValve",
				"GasLimitValve",
				"GasBottler",
				"BottleEmptierGas",
				"BottleEmptierConduitGas",
				"ModularLaunchpadPortGas",
				"ModularLaunchpadPortGasUnloader",
				GasConduitElementSensorConfig.ID,
				GasConduitDiseaseSensorConfig.ID,
				GasConduitTemperatureSensorConfig.ID
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Refining"), false, new List<string>
			{
				"FabricatedWoodMaker",
				"Compost",
				"WaterPurifier",
				"Desalinator",
				"FertilizerMaker",
				"AlgaeDistillery",
				"EthanolDistillery",
				"RockCrusher",
				"Kiln",
				"SludgePress",
				"MetalRefinery",
				"GlassForge",
				"OilRefinery",
				"Polymerizer",
				"OxyliteRefinery",
				"Chlorinator",
				"ChemicalRefinery",
				"SupermaterialRefinery",
				"DiamondPress",
				"MilkFatSeparator",
				"MilkPress"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Medical"), false, new List<string>
			{
				"DevLifeSupport",
				"WashBasin",
				"WashSink",
				"HandSanitizer",
				"DecontaminationShower",
				"OilChanger",
				"Apothecary",
				"DoctorStation",
				"AdvancedDoctorStation",
				"MedicalCot",
				"MassageTable",
				"Grave"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Furniture"), false, new List<string>
			{
				"Shelf",
				"Bed",
				"LuxuryBed",
				LadderBedConfig.ID,
				"FloorLamp",
				"CeilingLight",
				"SunLamp",
				"DevLightGenerator",
				"MercuryCeilingLight",
				"DiningTable",
				"MultiMinionDiningTable",
				"WaterCooler",
				"Phonobox",
				"ArcadeMachine",
				"EspressoMachine",
				"HotTub",
				"MechanicalSurfboard",
				"Sauna",
				"Juicer",
				"SodaFountain",
				"BeachChair",
				"VerticalWindTunnel",
				PixelPackConfig.ID,
				"Telephone",
				"FlowerVase",
				"FlowerVaseWall",
				"FlowerVaseHanging",
				"FlowerVaseHangingFancy",
				"SmallSculpture",
				"Sculpture",
				"IceSculpture",
				"WoodSculpture",
				"MarbleSculpture",
				"MetalSculpture",
				"FossilSculpture",
				"CeilingFossilSculpture",
				"CrownMoulding",
				"CornerMoulding",
				"Canvas",
				"CanvasWide",
				"CanvasTall",
				"ItemPedestal",
				"MonumentBottom",
				"MonumentMiddle",
				"MonumentTop",
				"ParkSign"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Equipment"), false, new List<string>
			{
				"ResearchCenter",
				"AdvancedResearchCenter",
				"NuclearResearchCenter",
				"OrbitalResearchCenter",
				"CosmicResearchCenter",
				"DLC1CosmicResearchCenter",
				"Telescope",
				"GeoTuner",
				"DataMiner",
				"PowerControlStation",
				"FarmStation",
				"GeneticAnalysisStation",
				"RanchStation",
				"ShearingStation",
				"MilkingStation",
				"RoleStation",
				"ResetSkillsStation",
				"ArtifactAnalysisStation",
				RemoteWorkerDockConfig.ID,
				RemoteWorkTerminalConfig.ID,
				"MissileFabricator",
				"CraftingTable",
				"AdvancedCraftingTable",
				"ClothingFabricator",
				"ClothingAlterationStation",
				"SuitFabricator",
				"OxygenMaskMarker",
				"OxygenMaskLocker",
				"SuitMarker",
				"SuitLocker",
				"JetSuitMarker",
				"JetSuitLocker",
				"LeadSuitMarker",
				"LeadSuitLocker",
				"AstronautTrainingCenter"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Utilities"), true, new List<string>
			{
				"Campfire",
				"DevHeater",
				"IceKettle",
				"SpaceHeater",
				"LiquidHeater",
				"LiquidCooledFan",
				"IceCooledFan",
				"IceMachine",
				"AirConditioner",
				"LiquidConditioner",
				"OreScrubber",
				"OilWellCap",
				"ThermalBlock",
				"SweepBotStation"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Automation"), true, new List<string>
			{
				"LogicWire",
				"LogicWireBridge",
				"LogicRibbon",
				"LogicRibbonBridge",
				LogicSwitchConfig.ID,
				"LogicDuplicantSensor",
				LogicPressureSensorGasConfig.ID,
				LogicPressureSensorLiquidConfig.ID,
				LogicTemperatureSensorConfig.ID,
				LogicLightSensorConfig.ID,
				LogicWattageSensorConfig.ID,
				LogicTimeOfDaySensorConfig.ID,
				LogicTimerSensorConfig.ID,
				LogicDiseaseSensorConfig.ID,
				LogicElementSensorGasConfig.ID,
				LogicElementSensorLiquidConfig.ID,
				LogicCritterCountSensorConfig.ID,
				LogicRadiationSensorConfig.ID,
				LogicHEPSensorConfig.ID,
				LogicCounterConfig.ID,
				LogicAlarmConfig.ID,
				LogicHammerConfig.ID,
				"LogicInterasteroidSender",
				"LogicInterasteroidReceiver",
				LogicRibbonReaderConfig.ID,
				LogicRibbonWriterConfig.ID,
				"FloorSwitch",
				"Checkpoint",
				CometDetectorConfig.ID,
				"LogicGateNOT",
				"LogicGateAND",
				"LogicGateOR",
				"LogicGateBUFFER",
				"LogicGateFILTER",
				"LogicGateXOR",
				LogicMemoryConfig.ID,
				"LogicGateMultiplexer",
				"LogicGateDemultiplexer"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Conveyance"), true, new List<string>
			{
				"DevPumpSolid",
				"SolidTransferArm",
				"SolidConduit",
				"SolidConduitBridge",
				"SolidConduitInbox",
				"SolidConduitOutbox",
				"SolidFilter",
				"SolidVent",
				"SolidLogicValve",
				"SolidLimitValve",
				SolidConduitDiseaseSensorConfig.ID,
				SolidConduitElementSensorConfig.ID,
				SolidConduitTemperatureSensorConfig.ID,
				"AutoMiner",
				"ModularLaunchpadPortSolid",
				"ModularLaunchpadPortSolidUnloader"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Rocketry"), true, new List<string>
			{
				"ClusterTelescope",
				"ClusterTelescopeEnclosed",
				"MissionControl",
				"MissionControlCluster",
				"LaunchPad",
				"Gantry",
				"SteamEngine",
				"KeroseneEngine",
				"BiodieselEngine",
				"SolidBooster",
				"LiquidFuelTank",
				"OxidizerTank",
				"OxidizerTankLiquid",
				"CargoBay",
				"GasCargoBay",
				"LiquidCargoBay",
				"CommandModule",
				"RoboPilotCommandModule",
				"TouristModule",
				"ResearchModule",
				"SpecialCargoBay",
				"HydrogenEngine",
				RocketControlStationConfig.ID,
				"RocketInteriorPowerPlug",
				"RocketInteriorLiquidInput",
				"RocketInteriorLiquidOutput",
				"RocketInteriorGasInput",
				"RocketInteriorGasOutput",
				"RocketInteriorSolidInput",
				"RocketInteriorSolidOutput",
				LogicClusterLocationSensorConfig.ID,
				"RailGun",
				"RailGunPayloadOpener",
				"LandingBeacon",
				"MissileLauncher",
				"ModularLaunchpadPortBridge"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("HEP"), true, new List<string>
			{
				"RadiationLight",
				"ManualHighEnergyParticleSpawner",
				"NuclearReactor",
				"UraniumCentrifuge",
				"HighEnergyParticleSpawner",
				"DevHEPSpawner",
				"HighEnergyParticleRedirector",
				"HEPBattery",
				"HEPBridgeTile",
				"DevRadiationGenerator"
			}, DlcManager.EXPANSION1, null)
		};

		// Token: 0x04005E0E RID: 24078
		public static List<Type> COMPONENT_DESCRIPTION_ORDER = new List<Type>
		{
			typeof(BottleEmptier),
			typeof(CookingStation),
			typeof(GourmetCookingStation),
			typeof(RoleStation),
			typeof(ResearchCenter),
			typeof(NuclearResearchCenter),
			typeof(LiquidCooledFan),
			typeof(HandSanitizer),
			typeof(HandSanitizer.Work),
			typeof(PlantAirConditioner),
			typeof(Clinic),
			typeof(BuildingElementEmitter),
			typeof(ElementConverter),
			typeof(ElementConsumer),
			typeof(PassiveElementConsumer),
			typeof(TinkerStation),
			typeof(EnergyConsumer),
			typeof(AirConditioner),
			typeof(Storage),
			typeof(Battery),
			typeof(AirFilter),
			typeof(FlushToilet),
			typeof(Toilet),
			typeof(EnergyGenerator),
			typeof(MassageTable),
			typeof(Shower),
			typeof(Ownable),
			typeof(PlantablePlot),
			typeof(RelaxationPoint),
			typeof(BuildingComplete),
			typeof(Building),
			typeof(BuildingPreview),
			typeof(BuildingUnderConstruction),
			typeof(Crop),
			typeof(Growing),
			typeof(Equippable),
			typeof(ColdBreather),
			typeof(ResearchPointObject),
			typeof(SuitTank),
			typeof(IlluminationVulnerable),
			typeof(TemperatureVulnerable),
			typeof(ExternalTemperatureMonitor),
			typeof(CritterTemperatureMonitor),
			typeof(PressureVulnerable),
			typeof(SubmersionMonitor),
			typeof(BatterySmart),
			typeof(Compost),
			typeof(Refrigerator),
			typeof(Bed),
			typeof(OreScrubber),
			typeof(OreScrubber.Work),
			typeof(MinimumOperatingTemperature),
			typeof(RoomTracker),
			typeof(EnergyConsumerSelfSustaining),
			typeof(ArcadeMachine),
			typeof(Telescope),
			typeof(EspressoMachine),
			typeof(JetSuitTank),
			typeof(Phonobox),
			typeof(ArcadeMachine),
			typeof(BeachChair),
			typeof(Sauna),
			typeof(VerticalWindTunnel),
			typeof(HotTub),
			typeof(Juicer),
			typeof(SodaFountain),
			typeof(MechanicalSurfboard),
			typeof(BottleEmptier),
			typeof(AccessControl),
			typeof(GammaRayOven),
			typeof(Reactor),
			typeof(HighEnergyParticlePort),
			typeof(LeadSuitTank),
			typeof(ActiveParticleConsumer.Def),
			typeof(WaterCooler),
			typeof(Edible),
			typeof(PlantableSeed),
			typeof(SicknessTrigger),
			typeof(MedicinalPill),
			typeof(SeedProducer),
			typeof(Geyser),
			typeof(SpaceHeater),
			typeof(Overheatable),
			typeof(CreatureCalorieMonitor.Def),
			typeof(LureableMonitor.Def),
			typeof(FertilizationMonitor.Def),
			typeof(IrrigationMonitor.Def),
			typeof(ScaleGrowthMonitor.Def),
			typeof(TravelTubeEntrance.Work),
			typeof(ToiletWorkableUse),
			typeof(ReceptacleMonitor),
			typeof(Light2D),
			typeof(Ladder),
			typeof(SimCellOccupier),
			typeof(Vent),
			typeof(LogicPorts),
			typeof(Capturable),
			typeof(Trappable),
			typeof(SpaceArtifact),
			typeof(MessStation),
			typeof(PlantElementEmitter),
			typeof(Radiator),
			typeof(DecorProvider)
		};

		// Token: 0x020021D4 RID: 8660
		public class PHARMACY
		{
			// Token: 0x02002A8B RID: 10891
			public class FABRICATIONTIME
			{
				// Token: 0x0400BBA5 RID: 48037
				public const float TIER0 = 50f;

				// Token: 0x0400BBA6 RID: 48038
				public const float TIER1 = 100f;

				// Token: 0x0400BBA7 RID: 48039
				public const float TIER2 = 200f;
			}
		}

		// Token: 0x020021D5 RID: 8661
		public class NUCLEAR_REACTOR
		{
			// Token: 0x02002A8C RID: 10892
			public class REACTOR_MASSES
			{
				// Token: 0x0400BBA8 RID: 48040
				public const float MIN = 1f;

				// Token: 0x0400BBA9 RID: 48041
				public const float MAX = 10f;
			}
		}

		// Token: 0x020021D6 RID: 8662
		public class OVERPRESSURE
		{
			// Token: 0x04009B86 RID: 39814
			public const float TIER0 = 1.8f;
		}

		// Token: 0x020021D7 RID: 8663
		public class OVERHEAT_TEMPERATURES
		{
			// Token: 0x04009B87 RID: 39815
			public const float LOW_3 = 10f;

			// Token: 0x04009B88 RID: 39816
			public const float LOW_2 = 328.15f;

			// Token: 0x04009B89 RID: 39817
			public const float LOW_1 = 338.15f;

			// Token: 0x04009B8A RID: 39818
			public const float NORMAL = 348.15f;

			// Token: 0x04009B8B RID: 39819
			public const float HIGH_1 = 363.15f;

			// Token: 0x04009B8C RID: 39820
			public const float HIGH_2 = 398.15f;

			// Token: 0x04009B8D RID: 39821
			public const float HIGH_3 = 1273.15f;

			// Token: 0x04009B8E RID: 39822
			public const float HIGH_4 = 2273.15f;
		}

		// Token: 0x020021D8 RID: 8664
		public class OVERHEAT_MATERIAL_MOD
		{
			// Token: 0x04009B8F RID: 39823
			public const float LOW_3 = -200f;

			// Token: 0x04009B90 RID: 39824
			public const float LOW_2 = -20f;

			// Token: 0x04009B91 RID: 39825
			public const float LOW_1 = -10f;

			// Token: 0x04009B92 RID: 39826
			public const float NORMAL = 0f;

			// Token: 0x04009B93 RID: 39827
			public const float HIGH_1 = 15f;

			// Token: 0x04009B94 RID: 39828
			public const float HIGH_2 = 50f;

			// Token: 0x04009B95 RID: 39829
			public const float HIGH_3 = 200f;

			// Token: 0x04009B96 RID: 39830
			public const float HIGH_4 = 500f;

			// Token: 0x04009B97 RID: 39831
			public const float HIGH_5 = 900f;
		}

		// Token: 0x020021D9 RID: 8665
		public class DECOR_MATERIAL_MOD
		{
			// Token: 0x04009B98 RID: 39832
			public const float NORMAL = 0f;

			// Token: 0x04009B99 RID: 39833
			public const float HIGH_1 = 0.1f;

			// Token: 0x04009B9A RID: 39834
			public const float HIGH_2 = 0.2f;

			// Token: 0x04009B9B RID: 39835
			public const float HIGH_3 = 0.5f;

			// Token: 0x04009B9C RID: 39836
			public const float HIGH_4 = 1f;
		}

		// Token: 0x020021DA RID: 8666
		public class CONSTRUCTION_MASS_KG
		{
			// Token: 0x04009B9D RID: 39837
			public static readonly float[] TIER_TINY = new float[]
			{
				5f
			};

			// Token: 0x04009B9E RID: 39838
			public static readonly float[] TIER0 = new float[]
			{
				25f
			};

			// Token: 0x04009B9F RID: 39839
			public static readonly float[] TIER1 = new float[]
			{
				50f
			};

			// Token: 0x04009BA0 RID: 39840
			public static readonly float[] TIER2 = new float[]
			{
				100f
			};

			// Token: 0x04009BA1 RID: 39841
			public static readonly float[] TIER3 = new float[]
			{
				200f
			};

			// Token: 0x04009BA2 RID: 39842
			public static readonly float[] TIER4 = new float[]
			{
				400f
			};

			// Token: 0x04009BA3 RID: 39843
			public static readonly float[] TIER5 = new float[]
			{
				800f
			};

			// Token: 0x04009BA4 RID: 39844
			public static readonly float[] TIER6 = new float[]
			{
				1200f
			};

			// Token: 0x04009BA5 RID: 39845
			public static readonly float[] TIER7 = new float[]
			{
				2000f
			};
		}

		// Token: 0x020021DB RID: 8667
		public class ROCKETRY_MASS_KG
		{
			// Token: 0x04009BA6 RID: 39846
			public static float[] COMMAND_MODULE_MASS = new float[]
			{
				200f
			};

			// Token: 0x04009BA7 RID: 39847
			public static float[] CARGO_MASS = new float[]
			{
				1000f
			};

			// Token: 0x04009BA8 RID: 39848
			public static float[] CARGO_MASS_SMALL = new float[]
			{
				400f
			};

			// Token: 0x04009BA9 RID: 39849
			public static float[] FUEL_TANK_DRY_MASS = new float[]
			{
				100f
			};

			// Token: 0x04009BAA RID: 39850
			public static float[] FUEL_TANK_WET_MASS = new float[]
			{
				900f
			};

			// Token: 0x04009BAB RID: 39851
			public static float[] FUEL_TANK_WET_MASS_SMALL = new float[]
			{
				300f
			};

			// Token: 0x04009BAC RID: 39852
			public static float[] FUEL_TANK_WET_MASS_GAS = new float[]
			{
				100f
			};

			// Token: 0x04009BAD RID: 39853
			public static float[] FUEL_TANK_WET_MASS_GAS_LARGE = new float[]
			{
				150f
			};

			// Token: 0x04009BAE RID: 39854
			public static float[] OXIDIZER_TANK_OXIDIZER_MASS = new float[]
			{
				900f
			};

			// Token: 0x04009BAF RID: 39855
			public static float[] ENGINE_MASS_SMALL = new float[]
			{
				200f
			};

			// Token: 0x04009BB0 RID: 39856
			public static float[] ENGINE_MASS_LARGE = new float[]
			{
				500f
			};

			// Token: 0x04009BB1 RID: 39857
			public static float[] NOSE_CONE_TIER1 = new float[]
			{
				200f,
				100f
			};

			// Token: 0x04009BB2 RID: 39858
			public static float[] NOSE_CONE_TIER2 = new float[]
			{
				400f,
				200f
			};

			// Token: 0x04009BB3 RID: 39859
			public static float[] HOLLOW_TIER1 = new float[]
			{
				200f
			};

			// Token: 0x04009BB4 RID: 39860
			public static float[] HOLLOW_TIER2 = new float[]
			{
				400f
			};

			// Token: 0x04009BB5 RID: 39861
			public static float[] HOLLOW_TIER3 = new float[]
			{
				800f
			};

			// Token: 0x04009BB6 RID: 39862
			public static float[] DENSE_TIER0 = new float[]
			{
				200f
			};

			// Token: 0x04009BB7 RID: 39863
			public static float[] DENSE_TIER1 = new float[]
			{
				500f
			};

			// Token: 0x04009BB8 RID: 39864
			public static float[] DENSE_TIER2 = new float[]
			{
				1000f
			};

			// Token: 0x04009BB9 RID: 39865
			public static float[] DENSE_TIER3 = new float[]
			{
				2000f
			};
		}

		// Token: 0x020021DC RID: 8668
		public class ENERGY_CONSUMPTION_WHEN_ACTIVE
		{
			// Token: 0x04009BBA RID: 39866
			public const float TIER0 = 0f;

			// Token: 0x04009BBB RID: 39867
			public const float TIER1 = 5f;

			// Token: 0x04009BBC RID: 39868
			public const float TIER2 = 60f;

			// Token: 0x04009BBD RID: 39869
			public const float TIER3 = 120f;

			// Token: 0x04009BBE RID: 39870
			public const float TIER4 = 240f;

			// Token: 0x04009BBF RID: 39871
			public const float TIER5 = 480f;

			// Token: 0x04009BC0 RID: 39872
			public const float TIER6 = 960f;

			// Token: 0x04009BC1 RID: 39873
			public const float TIER7 = 1200f;

			// Token: 0x04009BC2 RID: 39874
			public const float TIER8 = 1600f;
		}

		// Token: 0x020021DD RID: 8669
		public class EXHAUST_ENERGY_ACTIVE
		{
			// Token: 0x04009BC3 RID: 39875
			public const float TIER0 = 0f;

			// Token: 0x04009BC4 RID: 39876
			public const float TIER1 = 0.125f;

			// Token: 0x04009BC5 RID: 39877
			public const float TIER2 = 0.25f;

			// Token: 0x04009BC6 RID: 39878
			public const float TIER3 = 0.5f;

			// Token: 0x04009BC7 RID: 39879
			public const float TIER4 = 1f;

			// Token: 0x04009BC8 RID: 39880
			public const float TIER5 = 2f;

			// Token: 0x04009BC9 RID: 39881
			public const float TIER6 = 4f;

			// Token: 0x04009BCA RID: 39882
			public const float TIER7 = 8f;

			// Token: 0x04009BCB RID: 39883
			public const float TIER8 = 16f;
		}

		// Token: 0x020021DE RID: 8670
		public class JOULES_LEAK_PER_CYCLE
		{
			// Token: 0x04009BCC RID: 39884
			public const float TIER0 = 400f;

			// Token: 0x04009BCD RID: 39885
			public const float TIER1 = 1000f;

			// Token: 0x04009BCE RID: 39886
			public const float TIER2 = 2000f;
		}

		// Token: 0x020021DF RID: 8671
		public class SELF_HEAT_KILOWATTS
		{
			// Token: 0x04009BCF RID: 39887
			public const float TIER0 = 0f;

			// Token: 0x04009BD0 RID: 39888
			public const float TIER1 = 0.5f;

			// Token: 0x04009BD1 RID: 39889
			public const float TIER2 = 1f;

			// Token: 0x04009BD2 RID: 39890
			public const float TIER3 = 2f;

			// Token: 0x04009BD3 RID: 39891
			public const float TIER4 = 4f;

			// Token: 0x04009BD4 RID: 39892
			public const float TIER5 = 8f;

			// Token: 0x04009BD5 RID: 39893
			public const float TIER6 = 16f;

			// Token: 0x04009BD6 RID: 39894
			public const float TIER7 = 32f;

			// Token: 0x04009BD7 RID: 39895
			public const float TIER8 = 64f;

			// Token: 0x04009BD8 RID: 39896
			public const float TIER_NUCLEAR = 16384f;
		}

		// Token: 0x020021E0 RID: 8672
		public class MELTING_POINT_KELVIN
		{
			// Token: 0x04009BD9 RID: 39897
			public const float TIER0 = 800f;

			// Token: 0x04009BDA RID: 39898
			public const float TIER1 = 1600f;

			// Token: 0x04009BDB RID: 39899
			public const float TIER2 = 2400f;

			// Token: 0x04009BDC RID: 39900
			public const float TIER3 = 3200f;

			// Token: 0x04009BDD RID: 39901
			public const float TIER4 = 9999f;
		}

		// Token: 0x020021E1 RID: 8673
		public class CONSTRUCTION_TIME_SECONDS
		{
			// Token: 0x04009BDE RID: 39902
			public const float TIER0 = 3f;

			// Token: 0x04009BDF RID: 39903
			public const float TIER1 = 10f;

			// Token: 0x04009BE0 RID: 39904
			public const float TIER2 = 30f;

			// Token: 0x04009BE1 RID: 39905
			public const float TIER3 = 60f;

			// Token: 0x04009BE2 RID: 39906
			public const float TIER4 = 120f;

			// Token: 0x04009BE3 RID: 39907
			public const float TIER5 = 240f;

			// Token: 0x04009BE4 RID: 39908
			public const float TIER6 = 480f;
		}

		// Token: 0x020021E2 RID: 8674
		public class HITPOINTS
		{
			// Token: 0x04009BE5 RID: 39909
			public const int TIER0 = 10;

			// Token: 0x04009BE6 RID: 39910
			public const int TIER1 = 30;

			// Token: 0x04009BE7 RID: 39911
			public const int TIER2 = 100;

			// Token: 0x04009BE8 RID: 39912
			public const int TIER3 = 250;

			// Token: 0x04009BE9 RID: 39913
			public const int TIER4 = 1000;
		}

		// Token: 0x020021E3 RID: 8675
		public class DAMAGE_SOURCES
		{
			// Token: 0x04009BEA RID: 39914
			public const int CONDUIT_CONTENTS_BOILED = 1;

			// Token: 0x04009BEB RID: 39915
			public const int CONDUIT_CONTENTS_FROZE = 1;

			// Token: 0x04009BEC RID: 39916
			public const int BAD_INPUT_ELEMENT = 1;

			// Token: 0x04009BED RID: 39917
			public const int BUILDING_OVERHEATED = 1;

			// Token: 0x04009BEE RID: 39918
			public const int HIGH_LIQUID_PRESSURE = 10;

			// Token: 0x04009BEF RID: 39919
			public const int MICROMETEORITE = 1;

			// Token: 0x04009BF0 RID: 39920
			public const int CORROSIVE_ELEMENT = 1;
		}

		// Token: 0x020021E4 RID: 8676
		public class RELOCATION_TIME_SECONDS
		{
			// Token: 0x04009BF1 RID: 39921
			public const float DECONSTRUCT = 4f;

			// Token: 0x04009BF2 RID: 39922
			public const float CONSTRUCT = 4f;
		}

		// Token: 0x020021E5 RID: 8677
		public class WORK_TIME_SECONDS
		{
			// Token: 0x04009BF3 RID: 39923
			public const float VERYSHORT_WORK_TIME = 5f;

			// Token: 0x04009BF4 RID: 39924
			public const float SHORT_WORK_TIME = 15f;

			// Token: 0x04009BF5 RID: 39925
			public const float MEDIUM_WORK_TIME = 30f;

			// Token: 0x04009BF6 RID: 39926
			public const float LONG_WORK_TIME = 90f;

			// Token: 0x04009BF7 RID: 39927
			public const float VERY_LONG_WORK_TIME = 150f;

			// Token: 0x04009BF8 RID: 39928
			public const float EXTENSIVE_WORK_TIME = 180f;
		}

		// Token: 0x020021E6 RID: 8678
		public class FABRICATION_TIME_SECONDS
		{
			// Token: 0x04009BF9 RID: 39929
			public const float VERY_SHORT = 20f;

			// Token: 0x04009BFA RID: 39930
			public const float SHORT = 40f;

			// Token: 0x04009BFB RID: 39931
			public const float MODERATE = 80f;

			// Token: 0x04009BFC RID: 39932
			public const float LONG = 250f;
		}

		// Token: 0x020021E7 RID: 8679
		public class DECOR
		{
			// Token: 0x04009BFD RID: 39933
			public static readonly EffectorValues NONE = new EffectorValues
			{
				amount = 0,
				radius = 1
			};

			// Token: 0x02002A8D RID: 10893
			public class BONUS
			{
				// Token: 0x0400BBAA RID: 48042
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = 5,
					radius = 1
				};

				// Token: 0x0400BBAB RID: 48043
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = 10,
					radius = 2
				};

				// Token: 0x0400BBAC RID: 48044
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = 15,
					radius = 3
				};

				// Token: 0x0400BBAD RID: 48045
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = 20,
					radius = 4
				};

				// Token: 0x0400BBAE RID: 48046
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = 25,
					radius = 5
				};

				// Token: 0x0400BBAF RID: 48047
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = 30,
					radius = 6
				};

				// Token: 0x02003A58 RID: 14936
				public class MONUMENT
				{
					// Token: 0x0400EBA9 RID: 60329
					public static readonly EffectorValues COMPLETE = new EffectorValues
					{
						amount = 40,
						radius = 10
					};

					// Token: 0x0400EBAA RID: 60330
					public static readonly EffectorValues INCOMPLETE = new EffectorValues
					{
						amount = 10,
						radius = 5
					};
				}
			}

			// Token: 0x02002A8E RID: 10894
			public class PENALTY
			{
				// Token: 0x0400BBB0 RID: 48048
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = -5,
					radius = 1
				};

				// Token: 0x0400BBB1 RID: 48049
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = -10,
					radius = 2
				};

				// Token: 0x0400BBB2 RID: 48050
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = -15,
					radius = 3
				};

				// Token: 0x0400BBB3 RID: 48051
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = -20,
					radius = 4
				};

				// Token: 0x0400BBB4 RID: 48052
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = -20,
					radius = 5
				};

				// Token: 0x0400BBB5 RID: 48053
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = -25,
					radius = 6
				};
			}
		}

		// Token: 0x020021E8 RID: 8680
		public class MASS_KG
		{
			// Token: 0x04009BFE RID: 39934
			public const float TIER0 = 25f;

			// Token: 0x04009BFF RID: 39935
			public const float TIER1 = 50f;

			// Token: 0x04009C00 RID: 39936
			public const float TIER2 = 100f;

			// Token: 0x04009C01 RID: 39937
			public const float TIER3 = 200f;

			// Token: 0x04009C02 RID: 39938
			public const float TIER4 = 400f;

			// Token: 0x04009C03 RID: 39939
			public const float TIER5 = 800f;

			// Token: 0x04009C04 RID: 39940
			public const float TIER6 = 1200f;

			// Token: 0x04009C05 RID: 39941
			public const float TIER7 = 2000f;
		}

		// Token: 0x020021E9 RID: 8681
		public class UPGRADES
		{
			// Token: 0x04009C06 RID: 39942
			public const float BUILDTIME_TIER0 = 120f;

			// Token: 0x02002A8F RID: 10895
			public class MATERIALTAGS
			{
				// Token: 0x0400BBB6 RID: 48054
				public const string METAL = "Metal";

				// Token: 0x0400BBB7 RID: 48055
				public const string REFINEDMETAL = "RefinedMetal";

				// Token: 0x0400BBB8 RID: 48056
				public const string CARBON = "Carbon";
			}

			// Token: 0x02002A90 RID: 10896
			public class MATERIALMASS
			{
				// Token: 0x0400BBB9 RID: 48057
				public const int TIER0 = 100;

				// Token: 0x0400BBBA RID: 48058
				public const int TIER1 = 200;

				// Token: 0x0400BBBB RID: 48059
				public const int TIER2 = 400;

				// Token: 0x0400BBBC RID: 48060
				public const int TIER3 = 500;
			}

			// Token: 0x02002A91 RID: 10897
			public class MODIFIERAMOUNTS
			{
				// Token: 0x0400BBBD RID: 48061
				public const float MANUALGENERATOR_ENERGYGENERATION = 1.2f;

				// Token: 0x0400BBBE RID: 48062
				public const float MANUALGENERATOR_CAPACITY = 2f;

				// Token: 0x0400BBBF RID: 48063
				public const float PROPANEGENERATOR_ENERGYGENERATION = 1.6f;

				// Token: 0x0400BBC0 RID: 48064
				public const float PROPANEGENERATOR_HEATGENERATION = 1.6f;

				// Token: 0x0400BBC1 RID: 48065
				public const float GENERATOR_HEATGENERATION = 0.8f;

				// Token: 0x0400BBC2 RID: 48066
				public const float GENERATOR_ENERGYGENERATION = 1.3f;

				// Token: 0x0400BBC3 RID: 48067
				public const float TURBINE_ENERGYGENERATION = 1.2f;

				// Token: 0x0400BBC4 RID: 48068
				public const float TURBINE_CAPACITY = 1.2f;

				// Token: 0x0400BBC5 RID: 48069
				public const float SUITRECHARGER_EXECUTIONTIME = 1.2f;

				// Token: 0x0400BBC6 RID: 48070
				public const float SUITRECHARGER_HEATGENERATION = 1.2f;

				// Token: 0x0400BBC7 RID: 48071
				public const float STORAGELOCKER_CAPACITY = 2f;

				// Token: 0x0400BBC8 RID: 48072
				public const float SOLARPANEL_ENERGYGENERATION = 1.2f;

				// Token: 0x0400BBC9 RID: 48073
				public const float SMELTER_HEATGENERATION = 0.7f;
			}
		}
	}
}
