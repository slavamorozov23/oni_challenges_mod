using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public class FlowerVaseConfig : IBuildingConfig
{
	// Token: 0x06000948 RID: 2376 RVA: 0x0003E474 File Offset: 0x0003C674
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVase";
		int width = 1;
		int height = 1;
		string anim = "flowervase_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0003E4F9 File Offset: 0x0003C6F9
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.IsOffGround = true;
		plantablePlot.AddDepositTag(GameTags.DecorSeed);
		go.AddOrGet<FlowerVase>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0003E537 File Offset: 0x0003C737
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040006F1 RID: 1777
	public const string ID = "FlowerVase";
}
