using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class MicrobeMusherConfig : IBuildingConfig
{
	// Token: 0x06000F50 RID: 3920 RVA: 0x00058F80 File Offset: 0x00057180
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MicrobeMusher";
		int width = 2;
		int height = 3;
		string anim = "microbemusher_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, MicrobeMusherConfig.DECOR, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.AddSearchTerms(SEARCH_TERMS.FOOD);
		return buildingDef;
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00059020 File Offset: 0x00057220
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<ConduitConsumer>().conduitType = ConduitType.Liquid;
		MicrobeMusher microbeMusher = go.AddOrGet<MicrobeMusher>();
		microbeMusher.mushbarSpawnOffset = new Vector3(1f, 0f, 0f);
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_musher_kanim")
		};
		microbeMusher.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		BuildingTemplates.CreateComplexFabricatorStorage(go, microbeMusher);
		this.ConfigureRecipes();
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x000590C0 File Offset: 0x000572C0
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Dirt".ToTag(), 75f),
			new ComplexRecipe.RecipeElement("Water".ToTag(), 75f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("MushBar".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		MushBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array, array2), array, array2)
		{
			time = 40f,
			description = STRINGS.ITEMS.FOOD.MUSHBAR.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"MicrobeMusher"
			},
			sortOrder = 1
		};
		MushBarConfig.recipe.SetFabricationAnim("mushbar_kanim");
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicPlantFood", 2f),
			new ComplexRecipe.RecipeElement("Water".ToTag(), 50f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicPlantBar".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicPlantBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array3, array4), array3, array4)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.BASICPLANTBAR.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"MicrobeMusher"
			},
			sortOrder = 2
		};
		BasicPlantBarConfig.recipe.SetFabricationAnim("liceloaf_kanim");
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BeanPlantSeed", 6f),
			new ComplexRecipe.RecipeElement("Water".ToTag(), 50f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Tofu".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		TofuConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array5, array6), array5, array6)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.TOFU.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"MicrobeMusher"
			},
			sortOrder = 3
		};
		TofuConfig.recipe.SetFabricationAnim("loafu_kanim");
		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(new Tag[]
			{
				"ColdWheatSeed",
				FernFoodConfig.ID
			}, 5f),
			new ComplexRecipe.RecipeElement(new Tag[]
			{
				PrickleFruitConfig.ID,
				"HardSkinBerry"
			}, new float[]
			{
				1f,
				2f
			})
		};
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("FruitCake".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		FruitCakeConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array7, array8), array7, array8)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.FRUITCAKE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"MicrobeMusher"
			},
			sortOrder = 3
		};
		FruitCakeConfig.recipe.SetFabricationAnim("fruitcake_kanim");
		if (DlcManager.IsContentSubscribed("DLC2_ID"))
		{
			ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("Meat", 1f),
				new ComplexRecipe.RecipeElement("Tallow", 1f)
			};
			ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("Pemmican".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
			};
			PemmicanConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array9, array10), array9, array10, DlcManager.DLC2)
			{
				time = FOOD.RECIPES.STANDARD_COOK_TIME,
				description = STRINGS.ITEMS.FOOD.PEMMICAN.RECIPEDESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
				fabricators = new List<Tag>
				{
					"MicrobeMusher"
				},
				sortOrder = 4
			};
			PemmicanConfig.recipe.SetFabricationAnim("pemmican_kanim");
		}
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x00059500 File Offset: 0x00057700
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040009FF RID: 2559
	public const string ID = "MicrobeMusher";

	// Token: 0x04000A00 RID: 2560
	public static EffectorValues DECOR = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
}
