using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200046C RID: 1132
public class WoodGasGeneratorConfig : IBuildingConfig
{
	// Token: 0x060017AC RID: 6060 RVA: 0x000861B0 File Offset: 0x000843B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WoodGasGenerator";
		int width = 2;
		int height = 2;
		string anim = "generatorwood_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] construction_materials = all_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 300f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 8f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.AddSearchTerms(SEARCH_TERMS.LUMBER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.GENERATOR);
		return buildingDef;
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x00086294 File Offset: 0x00084494
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType, false);
		go.AddOrGet<LoopingSounds>();
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		float max_stored_input_mass = 720f;
		go.AddOrGet<LoopingSounds>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = "BuildingWood";
		manualDeliveryKG.capacity = 360f;
		manualDeliveryKG.refillMass = 180f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
		energyGenerator.powerDistributionOrder = 8;
		energyGenerator.hasMeter = true;
		energyGenerator.formula = EnergyGenerator.CreateSimpleFormula("BuildingWood", 1.2f, max_stored_input_mass, SimHashes.CarbonDioxide, 0.17f, false, new CellOffset(0, 1), 383.15f);
		Tinkerable.MakePowerTinkerable(go);
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000DE2 RID: 3554
	public const string ID = "WoodGasGenerator";

	// Token: 0x04000DE3 RID: 3555
	private const float BRANCHES_PER_GENERATOR = 8f;

	// Token: 0x04000DE4 RID: 3556
	public const float CONSUMPTION_RATE = 1.2f;

	// Token: 0x04000DE5 RID: 3557
	private const float WOOD_PER_REFILL = 360f;

	// Token: 0x04000DE6 RID: 3558
	private const SimHashes EXHAUST_ELEMENT_GAS = SimHashes.CarbonDioxide;

	// Token: 0x04000DE7 RID: 3559
	private const SimHashes EXHAUST_ELEMENT_GAS2 = SimHashes.Syngas;

	// Token: 0x04000DE8 RID: 3560
	public const float CO2_EXHAUST_RATE = 0.17f;

	// Token: 0x04000DE9 RID: 3561
	private const int WIDTH = 2;

	// Token: 0x04000DEA RID: 3562
	private const int HEIGHT = 2;
}
