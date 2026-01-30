using System;
using TUNING;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class DevPumpSolidConfig : IBuildingConfig
{
	// Token: 0x06000246 RID: 582 RVA: 0x0000FF34 File Offset: 0x0000E134
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevPumpSolid";
		int width = 2;
		int height = 2;
		string anim = "dev_pump_solid_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityOutputOffset = this.primaryPort.offset;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "DevPumpSolid");
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000FFE1 File Offset: 0x0000E1E1
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		base.ConfigureBuildingTemplate(go, prefab_tag);
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000FFFC File Offset: 0x0000E1FC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<DevPump>().elementState = Filterable.ElementState.Solid;
		go.AddOrGet<Storage>().capacityKg = 20f;
		go.AddTag(GameTags.CorrosionProof);
		SolidConduitDispenser solidConduitDispenser = go.AddOrGet<SolidConduitDispenser>();
		solidConduitDispenser.alwaysDispense = true;
		solidConduitDispenser.elementFilter = null;
		go.AddOrGetDef<OperationalController.Def>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000170 RID: 368
	public const string ID = "DevPumpSolid";

	// Token: 0x04000171 RID: 369
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

	// Token: 0x04000172 RID: 370
	private ConduitPortInfo primaryPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(1, 1));
}
