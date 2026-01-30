using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036B RID: 875
public class NuclearReactorConfig : IBuildingConfig
{
	// Token: 0x06001240 RID: 4672 RVA: 0x0006A448 File Offset: 0x00068648
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x0006A450 File Offset: 0x00068650
	public override BuildingDef CreateBuildingDef()
	{
		string id = "NuclearReactor";
		int width = 5;
		int height = 6;
		string anim = "generatornuclear_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 0f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.ThermalConductivity = 0.1f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Overheatable = false;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.UtilityInputOffset = new CellOffset(-2, 2);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("CONTROL_FUEL_DELIVERY", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_INACTIVE, false, true)
		};
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Breakable = false;
		buildingDef.Invincible = true;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x0006A5B0 File Offset: 0x000687B0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		UnityEngine.Object.Destroy(go.GetComponent<BuildingEnabledButton>());
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 25;
		radiationEmitter.emitRadiusY = 25;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emissionOffset = new Vector3(0f, 2f, 0f);
		Storage storage = go.AddComponent<Storage>();
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.RequestedItemTag = ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag;
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
		manualDeliveryKG.capacity = 180f;
		manualDeliveryKG.MinimumMass = 0.5f;
		go.AddOrGet<Reactor>();
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 90f;
		conduitConsumer.capacityTag = GameTags.AnyWater;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x0006A72E File Offset: 0x0006892E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddTag(GameTags.CorrosionProof);
	}

	// Token: 0x04000B84 RID: 2948
	public const string ID = "NuclearReactor";

	// Token: 0x04000B85 RID: 2949
	private const float FUEL_CAPACITY = 180f;

	// Token: 0x04000B86 RID: 2950
	public const float VENT_STEAM_TEMPERATURE = 673.15f;

	// Token: 0x04000B87 RID: 2951
	public const float MELT_DOWN_TEMPERATURE = 3000f;

	// Token: 0x04000B88 RID: 2952
	public const float MAX_VENT_PRESSURE = 150f;

	// Token: 0x04000B89 RID: 2953
	public const float INCREASED_CONDUCTION_SCALE = 5f;

	// Token: 0x04000B8A RID: 2954
	public const float REACTION_STRENGTH = 100f;

	// Token: 0x04000B8B RID: 2955
	public const int RADIATION_EMITTER_RANGE = 25;

	// Token: 0x04000B8C RID: 2956
	public const float OPERATIONAL_RADIATOR_INTENSITY = 2400f;

	// Token: 0x04000B8D RID: 2957
	public const float MELT_DOWN_RADIATOR_INTENSITY = 4800f;

	// Token: 0x04000B8E RID: 2958
	public const float FUEL_CONSUMPTION_SPEED = 0.016666668f;

	// Token: 0x04000B8F RID: 2959
	public const float BEGIN_REACTION_MASS = 0.5f;

	// Token: 0x04000B90 RID: 2960
	public const float STOP_REACTION_MASS = 0.25f;

	// Token: 0x04000B91 RID: 2961
	public const float DUMP_WASTE_AMOUNT = 100f;

	// Token: 0x04000B92 RID: 2962
	public const float WASTE_MASS_MULTIPLIER = 100f;

	// Token: 0x04000B93 RID: 2963
	public const float REACTION_MASS_TARGET = 60f;

	// Token: 0x04000B94 RID: 2964
	public const float COOLANT_AMOUNT = 30f;

	// Token: 0x04000B95 RID: 2965
	public const float COOLANT_CAPACITY = 90f;

	// Token: 0x04000B96 RID: 2966
	public const float MINIMUM_COOLANT_MASS = 30f;

	// Token: 0x04000B97 RID: 2967
	public const float WASTE_GERMS_PER_KG = 50f;

	// Token: 0x04000B98 RID: 2968
	public const float PST_MELTDOWN_COOLING_TIME = 3000f;

	// Token: 0x04000B99 RID: 2969
	public const string INPUT_PORT_ID = "CONTROL_FUEL_DELIVERY";
}
