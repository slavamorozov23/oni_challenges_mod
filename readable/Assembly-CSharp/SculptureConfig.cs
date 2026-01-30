using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000402 RID: 1026
public class SculptureConfig : IBuildingConfig
{
	// Token: 0x06001527 RID: 5415 RVA: 0x00079428 File Offset: 0x00077628
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Sculpture";
		int width = 1;
		int height = 3;
		string anim = "sculpture_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 10,
			radius = 8
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanArt.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STATUE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.ARTWORK);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x0007950E File Offset: 0x0007770E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x0007952D File Offset: 0x0007772D
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000CD1 RID: 3281
	public const string ID = "Sculpture";
}
