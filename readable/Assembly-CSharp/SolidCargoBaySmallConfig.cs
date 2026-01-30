using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000421 RID: 1057
public class SolidCargoBaySmallConfig : IBuildingConfig
{
	// Token: 0x060015D2 RID: 5586 RVA: 0x0007CC51 File Offset: 0x0007AE51
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060015D3 RID: 5587 RVA: 0x0007CC58 File Offset: 0x0007AE58
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidCargoBaySmall";
		int width = 3;
		int height = 3;
		string anim = "rocket_storage_solid_small_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = TUNING.BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.Invincible = true;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		return buildingDef;
	}

	// Token: 0x060015D4 RID: 5588 RVA: 0x0007CD0C File Offset: 0x0007AF0C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, null)
		};
	}

	// Token: 0x060015D5 RID: 5589 RVA: 0x0007CD70 File Offset: 0x0007AF70
	public override void DoPostConfigureComplete(GameObject go)
	{
		go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.STORAGE_SOLID_CARGO_BAY, new List<Tag>(2)
		{
			GameTags.Liquid,
			GameTags.Gas
		}, CargoBay.CargoType.Solids);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE, 0f, 0f);
	}

	// Token: 0x04000D01 RID: 3329
	public const string ID = "SolidCargoBaySmall";

	// Token: 0x04000D02 RID: 3330
	public float CAPACITY = 1200f * ROCKETRY.CARGO_CAPACITY_SCALE;
}
