using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002EE RID: 750
public class MetalSculptureConfig : IBuildingConfig
{
	// Token: 0x06000F43 RID: 3907 RVA: 0x00058A00 File Offset: 0x00056C00
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MetalSculpture";
		int width = 1;
		int height = 3;
		string anim = "sculpture_metal_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 20,
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
		buildingDef.AddSearchTerms(SEARCH_TERMS.METAL);
		return buildingDef;
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x00058AF6 File Offset: 0x00056CF6
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00058B15 File Offset: 0x00056D15
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x040009F6 RID: 2550
	public const string ID = "MetalSculpture";
}
