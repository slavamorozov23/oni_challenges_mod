using System;
using TUNING;
using UnityEngine;

// Token: 0x02000350 RID: 848
public class MissionControlClusterConfig : IBuildingConfig
{
	// Token: 0x060011A8 RID: 4520 RVA: 0x00067EF1 File Offset: 0x000660F1
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x00067EF8 File Offset: 0x000660F8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControlCluster";
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

	// Token: 0x060011AA RID: 4522 RVA: 0x00067FAC File Offset: 0x000661AC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlClusterConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControlCluster.Def>();
		MissionControlClusterWorkable missionControlClusterWorkable = go.AddOrGet<MissionControlClusterWorkable>();
		missionControlClusterWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlClusterWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x0006802D File Offset: 0x0006622D
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x060011AC RID: 4524 RVA: 0x0006805B File Offset: 0x0006625B
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x060011AD RID: 4525 RVA: 0x00068063 File Offset: 0x00066263
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x0006806B File Offset: 0x0006626B
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000B1D RID: 2845
	public const string ID = "MissionControlCluster";

	// Token: 0x04000B1E RID: 2846
	public const int WORK_RANGE_RADIUS = 2;

	// Token: 0x04000B1F RID: 2847
	public const float EFFECT_DURATION = 600f;

	// Token: 0x04000B20 RID: 2848
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x04000B21 RID: 2849
	public const int SCAN_RADIUS = 1;

	// Token: 0x04000B22 RID: 2850
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x04000B23 RID: 2851
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
