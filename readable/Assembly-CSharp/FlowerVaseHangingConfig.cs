using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001CA RID: 458
public class FlowerVaseHangingConfig : IBuildingConfig
{
	// Token: 0x0600094C RID: 2380 RVA: 0x0003E544 File Offset: 0x0003C744
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVaseHanging";
		int width = 1;
		int height = 2;
		string anim = "flowervase_hanging_basic_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		buildingDef.GenerateOffsets(1, 1);
		return buildingDef;
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0003E5D4 File Offset: 0x0003C7D4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.AddDepositTag(GameTags.DecorSeed);
		plantablePlot.occupyingObjectVisualOffset = new Vector3(0f, -0.25f, 0f);
		go.AddOrGet<FlowerVase>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0003E630 File Offset: 0x0003C830
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040006F2 RID: 1778
	public const string ID = "FlowerVaseHanging";
}
