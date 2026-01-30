using System;
using TUNING;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class TouristModuleConfig : IBuildingConfig
{
	// Token: 0x060016F4 RID: 5876 RVA: 0x00082E6C File Offset: 0x0008106C
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060016F5 RID: 5877 RVA: 0x00082E74 File Offset: 0x00081074
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TouristModule";
		int width = 5;
		int height = 5;
		string anim = "rocket_tourist_kanim";
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

	// Token: 0x060016F6 RID: 5878 RVA: 0x00082F24 File Offset: 0x00081124
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<TouristModule>();
		go.AddOrGet<CommandModuleWorkable>();
		go.AddOrGet<ArtifactFinder>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
		go.AddOrGet<Storage>();
		go.AddOrGet<MinionStorage>();
	}

	// Token: 0x060016F7 RID: 5879 RVA: 0x00082FAB File Offset: 0x000811AB
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_tourist_bg_kanim", false);
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.RocketCommandModule.Id;
		ownable.canBePublic = false;
	}

	// Token: 0x04000D98 RID: 3480
	public const string ID = "TouristModule";
}
