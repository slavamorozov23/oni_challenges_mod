using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200038E RID: 910
public class PhonoboxConfig : IBuildingConfig
{
	// Token: 0x060012D8 RID: 4824 RVA: 0x0006DB24 File Offset: 0x0006BD24
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Phonobox";
		int width = 5;
		int height = 3;
		string anim = "jukebot_kanim";
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
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x0006DBBC File Offset: 0x0006BDBC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<Phonobox>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x0006DC15 File Offset: 0x0006BE15
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000BF5 RID: 3061
	public const string ID = "Phonobox";
}
