using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class OrbitalResearchCenterConfig : IBuildingConfig
{
	// Token: 0x06001262 RID: 4706 RVA: 0x0006B2CE File Offset: 0x000694CE
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x0006B2D8 File Offset: 0x000694D8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OrbitalResearchCenter";
		int width = 2;
		int height = 3;
		string anim = "orbital_research_station_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanMissionControl.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.RESEARCH);
		return buildingDef;
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x0006B394 File Offset: 0x00069594
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddOrGet<InOrbitRequired>();
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<Prioritizable>();
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 308.15f;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_orbital_research_station_kanim")
		};
		Prioritizable.AddRef(go);
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		this.ConfigureRecipes();
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x0006B438 File Offset: 0x00069638
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(OrbitalResearchCenterConfig.INPUT_MATERIAL, 5f, true)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("OrbitalResearchDatabank".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("OrbitalResearchCenter", array, array2), array, array2);
		complexRecipe.time = 33f;
		complexRecipe.description = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.RECIPE_DESC;
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient;
		complexRecipe.fabricators = new List<Tag>
		{
			"OrbitalResearchCenter"
		};
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x0006B4CE File Offset: 0x000696CE
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000BB6 RID: 2998
	public const string ID = "OrbitalResearchCenter";

	// Token: 0x04000BB7 RID: 2999
	public const float BASE_SECONDS_PER_POINT = 33f;

	// Token: 0x04000BB8 RID: 3000
	public const float MASS_PER_POINT = 5f;

	// Token: 0x04000BB9 RID: 3001
	public static readonly Tag INPUT_MATERIAL = SimHashes.Polypropylene.CreateTag();

	// Token: 0x04000BBA RID: 3002
	public const float OUTPUT_TEMPERATURE = 308.15f;
}
