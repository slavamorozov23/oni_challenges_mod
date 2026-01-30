using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000396 RID: 918
public class PlanterBoxConfig : IBuildingConfig
{
	// Token: 0x060012FD RID: 4861 RVA: 0x0006E4DC File Offset: 0x0006C6DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PlanterBox";
		int width = 1;
		int height = 1;
		string anim = "planterbox_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] farmable = MATERIALS.FARMABLE;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, farmable, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.AddSearchTerms(SEARCH_TERMS.FOOD);
		buildingDef.AddSearchTerms(SEARCH_TERMS.FARM);
		return buildingDef;
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x0006E570 File Offset: 0x0006C770
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.CodexCategories.FarmBuilding, false);
		Storage storage = go.AddOrGet<Storage>();
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.IsOffGround = true;
		plantablePlot.tagOnPlanted = GameTags.PlantedOnFloorVessel;
		plantablePlot.AddDepositTag(GameTags.CropSeed);
		plantablePlot.SetFertilizationFlags(true, false);
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
		BuildingTemplates.CreateDefaultStorage(go, false);
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<PlanterBox>();
		go.AddOrGet<AnimTileable>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x0006E5FB File Offset: 0x0006C7FB
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000BFE RID: 3070
	public const string ID = "PlanterBox";
}
