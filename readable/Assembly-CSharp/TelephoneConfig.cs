using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class TelephoneConfig : IBuildingConfig
{
	// Token: 0x060016C7 RID: 5831 RVA: 0x0008215D File Offset: 0x0008035D
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060016C8 RID: 5832 RVA: 0x00082164 File Offset: 0x00080364
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Telephone";
		int width = 1;
		int height = 2;
		string anim = "telephone_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060016C9 RID: 5833 RVA: 0x00082208 File Offset: 0x00080408
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Telephone telephone = go.AddOrGet<Telephone>();
		telephone.babbleEffect = "TelephoneBabble";
		telephone.chatEffect = "TelephoneChat";
		telephone.longDistanceEffect = "TelephoneLongDistance";
		telephone.trackingEffect = "RecentlyTelephoned";
		go.AddOrGet<TelephoneCallerWorkable>().basePriority = RELAXATION.PRIORITY.TIER5;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x00082294 File Offset: 0x00080494
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000D7D RID: 3453
	public const string ID = "Telephone";

	// Token: 0x04000D7E RID: 3454
	public const float ringTime = 15f;

	// Token: 0x04000D7F RID: 3455
	public const float callTime = 25f;
}
