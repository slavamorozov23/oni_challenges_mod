using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class ClothingFabricatorConfig : IBuildingConfig
{
	// Token: 0x06000151 RID: 337 RVA: 0x0000A33C File Offset: 0x0000853C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ClothingFabricator";
		int width = 4;
		int height = 3;
		string anim = "clothingfactory_kanim";
		int hitpoints = 100;
		float construction_time = 240f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(2, 0);
		return buildingDef;
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000A3B8 File Offset: 0x000085B8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<DropAllWorkable>();
		Prioritizable.AddRef(go);
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_clothingfactory_kanim")
		};
		go.AddOrGet<ComplexFabricatorWorkable>();
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		this.ConfigureRecipes();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000A434 File Offset: 0x00008634
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(GameTags.Fabrics, (float)TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, false)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Warm_Vest".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		WarmVestConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", array, array2), array, array2)
		{
			time = TUNING.EQUIPMENT.VESTS.WARM_VEST_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.WARM_VEST.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"ClothingFabricator"
			},
			sortOrder = 1
		};
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(GameTags.Fabrics, (float)TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, false)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Funky_Vest".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		FunkyVestConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", array3, array4), array3, array4)
		{
			time = TUNING.EQUIPMENT.VESTS.FUNKY_VEST_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"ClothingFabricator"
			},
			sortOrder = 1
		};
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000A57D File Offset: 0x0000877D
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		};
	}

	// Token: 0x040000D3 RID: 211
	public const string ID = "ClothingFabricator";
}
