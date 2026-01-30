using System;
using TUNING;
using UnityEngine;

// Token: 0x0200023C RID: 572
public class GenericFabricatorConfig : IBuildingConfig
{
	// Token: 0x06000B90 RID: 2960 RVA: 0x00045680 File Offset: 0x00043880
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GenericFabricator";
		int width = 3;
		int height = 3;
		string anim = "fabricator_generic_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x00045718 File Offset: 0x00043918
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fabricator_generic_kanim")
		};
		go.AddOrGet<ComplexFabricatorWorkable>();
		complexFabricator.fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x000457AC File Offset: 0x000439AC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveStoppableController.Def>();
	}

	// Token: 0x040007F8 RID: 2040
	public const string ID = "GenericFabricator";
}
