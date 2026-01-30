using System;
using TUNING;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class DevPumpLiquidConfig : IBuildingConfig
{
	// Token: 0x06000242 RID: 578 RVA: 0x0000FDC8 File Offset: 0x0000DFC8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevPumpLiquid";
		int width = 2;
		int height = 2;
		string anim = "dev_pump_liquid_kanim";
		int hitpoints = 100;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityOutputOffset = this.primaryPort.offset;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "DevPumpLiquid");
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000FE75 File Offset: 0x0000E075
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		base.ConfigureBuildingTemplate(go, prefab_tag);
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000FE90 File Offset: 0x0000E090
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<DevPump>().elementState = Filterable.ElementState.Liquid;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 20f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		go.AddTag(GameTags.CorrosionProof);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
		go.AddOrGetDef<OperationalController.Def>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x0400016D RID: 365
	public const string ID = "DevPumpLiquid";

	// Token: 0x0400016E RID: 366
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x0400016F RID: 367
	private ConduitPortInfo primaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));
}
