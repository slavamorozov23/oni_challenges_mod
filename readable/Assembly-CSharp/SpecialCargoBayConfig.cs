using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class SpecialCargoBayConfig : IBuildingConfig
{
	// Token: 0x06001641 RID: 5697 RVA: 0x0007ECC8 File Offset: 0x0007CEC8
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x0007ECD0 File Offset: 0x0007CED0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpecialCargoBay";
		int width = 5;
		int height = 5;
		string anim = "rocket_storage_live_kanim";
		int hitpoints = 1000;
		float construction_time = 480f;
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
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		return buildingDef;
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x0007ED90 File Offset: 0x0007CF90
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

	// Token: 0x06001644 RID: 5700 RVA: 0x0007EDF4 File Offset: 0x0007CFF4
	public override void DoPostConfigureComplete(GameObject go)
	{
		CargoBay cargoBay = go.AddOrGet<CargoBay>();
		cargoBay.storage = go.AddOrGet<Storage>();
		cargoBay.storageType = CargoBay.CargoType.Entities;
		cargoBay.storage.capacityKg = 100f;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_storage_live_bg_kanim", false);
	}

	// Token: 0x04000D37 RID: 3383
	public const string ID = "SpecialCargoBay";
}
