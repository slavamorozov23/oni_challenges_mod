using System;
using TUNING;
using UnityEngine;

// Token: 0x020003E3 RID: 995
public class ResearchModuleConfig : IBuildingConfig
{
	// Token: 0x0600146F RID: 5231 RVA: 0x00074195 File Offset: 0x00072395
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x0007419C File Offset: 0x0007239C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ResearchModule";
		int width = 5;
		int height = 5;
		string anim = "rocket_research_module_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] command_MODULE_MASS = BUILDINGS.ROCKETRY_MASS_KG.COMMAND_MODULE_MASS;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, command_MODULE_MASS, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		return buildingDef;
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x0007424C File Offset: 0x0007244C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<ResearchModule>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x000742B7 File Offset: 0x000724B7
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_research_module_bg_kanim", false);
	}

	// Token: 0x04000C5F RID: 3167
	public const string ID = "ResearchModule";
}
