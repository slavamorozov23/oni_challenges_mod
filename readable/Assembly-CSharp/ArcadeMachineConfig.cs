using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class ArcadeMachineConfig : IBuildingConfig
{
	// Token: 0x0600007E RID: 126 RVA: 0x00005740 File Offset: 0x00003940
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ArcadeMachine";
		int width = 3;
		int height = 3;
		string anim = "arcade_cabinet_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x000057D8 File Offset: 0x000039D8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<ArcadeMachine>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000080 RID: 128 RVA: 0x0000582A File Offset: 0x00003A2A
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400006C RID: 108
	public const string ID = "ArcadeMachine";

	// Token: 0x0400006D RID: 109
	public const string SPECIFIC_EFFECT = "PlayedArcade";

	// Token: 0x0400006E RID: 110
	public const string TRACKING_EFFECT = "RecentlyPlayedArcade";
}
