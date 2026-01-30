using System;
using TUNING;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class LiquidVentConfig : IBuildingConfig
{
	// Token: 0x06000DD3 RID: 3539 RVA: 0x00051EEC File Offset: 0x000500EC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidVent";
		int width = 1;
		int height = 1;
		string anim = "ventliquid_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidVent");
		SoundEventVolumeCache.instance.AddVolume("ventliquid_kanim", "LiquidVent_squirt", NOISE_POLLUTION.NOISY.TIER0);
		return buildingDef;
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x00051FB4 File Offset: 0x000501B4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Exhaust>();
		go.AddOrGet<LogicOperationalController>();
		Vent vent = go.AddOrGet<Vent>();
		vent.conduitType = ConduitType.Liquid;
		vent.endpointType = Endpoint.Sink;
		vent.overpressureMass = 1000f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.ignoreMinMassCheck = true;
		BuildingTemplates.CreateDefaultStorage(go, false).showInUI = true;
		go.AddOrGet<SimpleVent>();
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0005201B File Offset: 0x0005021B
	public override void DoPostConfigureComplete(GameObject go)
	{
		VentController.Def def = go.AddOrGetDef<VentController.Def>();
		def.usingDynamicColor = true;
		def.outputSubstanceAnimName = "leak";
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000949 RID: 2377
	public const string ID = "LiquidVent";

	// Token: 0x0400094A RID: 2378
	public const float OVERPRESSURE_MASS = 1000f;

	// Token: 0x0400094B RID: 2379
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
}
