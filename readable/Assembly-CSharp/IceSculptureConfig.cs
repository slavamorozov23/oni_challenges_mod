using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200026E RID: 622
public class IceSculptureConfig : IBuildingConfig
{
	// Token: 0x06000CA4 RID: 3236 RVA: 0x0004C234 File Offset: 0x0004A434
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceSculpture";
		int width = 2;
		int height = 2;
		string anim = "icesculpture_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] construction_materials = new string[]
		{
			"Ice"
		};
		float melting_point = 273.15f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, new EffectorValues
		{
			amount = 35,
			radius = 8
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.Temperature = 253.15f;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanArt.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STATUE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.ARTWORK);
		return buildingDef;
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x0004C32E File Offset: 0x0004A52E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x0004C34D File Offset: 0x0004A54D
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x040008C2 RID: 2242
	public const string ID = "IceSculpture";
}
