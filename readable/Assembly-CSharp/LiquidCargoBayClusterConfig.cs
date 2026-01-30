using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000294 RID: 660
public class LiquidCargoBayClusterConfig : IBuildingConfig
{
	// Token: 0x06000D6C RID: 3436 RVA: 0x0004FB8F File Offset: 0x0004DD8F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000D6D RID: 3437 RVA: 0x0004FB98 File Offset: 0x0004DD98
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidCargoBayCluster";
		int width = 5;
		int height = 5;
		string anim = "rocket_cluster_storage_liquid_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] dense_TIER = TUNING.BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER2;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, dense_TIER, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		return buildingDef;
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x0004FC64 File Offset: 0x0004DE64
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

	// Token: 0x06000D6F RID: 3439 RVA: 0x0004FCC8 File Offset: 0x0004DEC8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.LIQUIDS, CargoBay.CargoType.Liquids);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE_PLUS, 0f, 0f);
	}

	// Token: 0x0400091E RID: 2334
	public const string ID = "LiquidCargoBayCluster";

	// Token: 0x0400091F RID: 2335
	public float CAPACITY = ROCKETRY.LIQUID_CARGO_BAY_CLUSTER_CAPACITY * ROCKETRY.CARGO_CAPACITY_SCALE;
}
