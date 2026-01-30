using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000229 RID: 553
public class GasCargoBaySmallConfig : IBuildingConfig
{
	// Token: 0x06000B32 RID: 2866 RVA: 0x00043538 File Offset: 0x00041738
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x00043540 File Offset: 0x00041740
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasCargoBaySmall";
		int width = 3;
		int height = 3;
		string anim = "rocket_storage_gas_small_kanim";
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
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		return buildingDef;
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x000435E8 File Offset: 0x000417E8
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

	// Token: 0x06000B35 RID: 2869 RVA: 0x0004364C File Offset: 0x0004184C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.GASES, CargoBay.CargoType.Gasses);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR, 0f, 0f);
	}

	// Token: 0x040007D1 RID: 2001
	public const string ID = "GasCargoBaySmall";

	// Token: 0x040007D2 RID: 2002
	public float CAPACITY = 360f * ROCKETRY.CARGO_CAPACITY_SCALE;
}
