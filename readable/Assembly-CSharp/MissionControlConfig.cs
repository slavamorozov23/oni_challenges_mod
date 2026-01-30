using System;
using TUNING;
using UnityEngine;

// Token: 0x02000351 RID: 849
public class MissionControlConfig : IBuildingConfig
{
	// Token: 0x060011B1 RID: 4529 RVA: 0x000680B8 File Offset: 0x000662B8
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x000680C0 File Offset: 0x000662C0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControl";
		int width = 3;
		int height = 3;
		string anim = "mission_control_station_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.DefaultAnimState = "off";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanMissionControl.Id;
		return buildingDef;
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x00068174 File Offset: 0x00066374
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControl.Def>();
		MissionControlWorkable missionControlWorkable = go.AddOrGet<MissionControlWorkable>();
		missionControlWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x000681F5 File Offset: 0x000663F5
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x00068223 File Offset: 0x00066423
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x0006822B File Offset: 0x0006642B
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x00068233 File Offset: 0x00066433
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000B24 RID: 2852
	public const string ID = "MissionControl";

	// Token: 0x04000B25 RID: 2853
	public const float EFFECT_DURATION = 600f;

	// Token: 0x04000B26 RID: 2854
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x04000B27 RID: 2855
	public const int SCAN_RADIUS = 1;

	// Token: 0x04000B28 RID: 2856
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x04000B29 RID: 2857
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
