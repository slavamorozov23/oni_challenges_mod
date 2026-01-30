using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000228 RID: 552
public class GasCargoBayConfig : IBuildingConfig
{
	// Token: 0x06000B2D RID: 2861 RVA: 0x00043382 File Offset: 0x00041582
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0004338C File Offset: 0x0004158C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasCargoBay";
		int width = 5;
		int height = 5;
		string anim = "rocket_storage_gas_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] cargo_MASS = TUNING.BUILDINGS.ROCKETRY_MASS_KG.CARGO_MASS;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, cargo_MASS, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 3);
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		return buildingDef;
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x00043460 File Offset: 0x00041660
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x000434C4 File Offset: 0x000416C4
	public override void DoPostConfigureComplete(GameObject go)
	{
		CargoBay cargoBay = go.AddOrGet<CargoBay>();
		cargoBay.storage = go.AddOrGet<Storage>();
		cargoBay.storageType = CargoBay.CargoType.Gasses;
		cargoBay.storage.capacityKg = 1000f;
		cargoBay.storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.storage = cargoBay.storage;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_storage_gas_bg_kanim", false);
	}

	// Token: 0x040007D0 RID: 2000
	public const string ID = "GasCargoBay";
}
