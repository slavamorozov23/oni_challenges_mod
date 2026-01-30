using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class BeachChairConfig : IBuildingConfig
{
	// Token: 0x060000C2 RID: 194 RVA: 0x00006C8C File Offset: 0x00004E8C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BeachChair";
		int width = 2;
		int height = 3;
		string anim = "beach_chair_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			400f,
			2f
		};
		string[] construction_materials = new string[]
		{
			"BuildableRaw",
			"BuildingFiber"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER4, none, 0.2f);
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00006D20 File Offset: 0x00004F20
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<BeachChairWorkable>().basePriority = RELAXATION.PRIORITY.TIER4;
		BeachChair beachChair = go.AddOrGet<BeachChair>();
		beachChair.specificEffectUnlit = "BeachChairUnlit";
		beachChair.specificEffectLit = "BeachChairLit";
		beachChair.trackingEffect = "RecentlyBeachChair";
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGet<AnimTileable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00006DA8 File Offset: 0x00004FA8
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000089 RID: 137
	public const string ID = "BeachChair";

	// Token: 0x0400008A RID: 138
	public static readonly int TAN_LUX = DUPLICANTSTATS.STANDARD.Light.HIGH_LIGHT;

	// Token: 0x0400008B RID: 139
	private const float TANK_SIZE_KG = 20f;

	// Token: 0x0400008C RID: 140
	private const float SPILL_RATE_KG = 0.05f;
}
