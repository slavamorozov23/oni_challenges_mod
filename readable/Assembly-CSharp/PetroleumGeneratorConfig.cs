using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200038D RID: 909
public class PetroleumGeneratorConfig : IBuildingConfig
{
	// Token: 0x060012D5 RID: 4821 RVA: 0x0006D8B4 File Offset: 0x0006BAB4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PetroleumGenerator";
		int width = 3;
		int height = 4;
		string anim = "generatorpetrol_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		string[] array = new string[]
		{
			"Metal"
		};
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0]
		};
		string[] construction_materials = array;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier, 0.2f);
		buildingDef.GeneratorWattageRating = 2000f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 4f;
		buildingDef.SelfHeatKilowattsWhenActive = 16f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(1, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.GENERATOR);
		return buildingDef;
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x0006D9B0 File Offset: 0x0006BBB0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType, false);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Storage>();
		BuildingDef def = go.GetComponent<Building>().Def;
		float num = 20f;
		go.AddOrGet<LoopingSounds>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = def.InputConduitType;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTags.CombustibleLiquid;
		conduitConsumer.capacityKG = num;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
		energyGenerator.powerDistributionOrder = 8;
		energyGenerator.ignoreBatteryRefillPercent = true;
		energyGenerator.hasMeter = true;
		energyGenerator.formula = new EnergyGenerator.Formula
		{
			inputs = new EnergyGenerator.InputItem[]
			{
				new EnergyGenerator.InputItem(GameTags.CombustibleLiquid, 2f, num)
			},
			outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, 0.5f, false, new CellOffset(0, 3), 383.15f),
				new EnergyGenerator.OutputItem(SimHashes.DirtyWater, 0.75f, false, new CellOffset(1, 1), 313.15f)
			}
		};
		Tinkerable.MakePowerTinkerable(go);
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000BEB RID: 3051
	public const string ID = "PetroleumGenerator";

	// Token: 0x04000BEC RID: 3052
	public const float CONSUMPTION_RATE = 2f;

	// Token: 0x04000BED RID: 3053
	private const SimHashes INPUT_ELEMENT = SimHashes.Petroleum;

	// Token: 0x04000BEE RID: 3054
	private const SimHashes EXHAUST_ELEMENT_GAS = SimHashes.CarbonDioxide;

	// Token: 0x04000BEF RID: 3055
	private const SimHashes EXHAUST_ELEMENT_LIQUID = SimHashes.DirtyWater;

	// Token: 0x04000BF0 RID: 3056
	public const float EFFICIENCY_RATE = 0.5f;

	// Token: 0x04000BF1 RID: 3057
	public const float EXHAUST_GAS_RATE = 0.5f;

	// Token: 0x04000BF2 RID: 3058
	public const float EXHAUST_LIQUID_RATE = 0.75f;

	// Token: 0x04000BF3 RID: 3059
	private const int WIDTH = 3;

	// Token: 0x04000BF4 RID: 3060
	private const int HEIGHT = 4;
}
