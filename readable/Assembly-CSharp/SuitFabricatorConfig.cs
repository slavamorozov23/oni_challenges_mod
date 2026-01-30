using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000443 RID: 1091
public class SuitFabricatorConfig : IBuildingConfig
{
	// Token: 0x060016A5 RID: 5797 RVA: 0x00080CD8 File Offset: 0x0007EED8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SuitFabricator";
		int width = 4;
		int height = 3;
		string anim = "suit_maker_kanim";
		int hitpoints = 100;
		float construction_time = 240f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.AddSearchTerms(SEARCH_TERMS.ATMOSUIT);
		return buildingDef;
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x00080D64 File Offset: 0x0007EF64
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<Prioritizable>();
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 318.15f;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_suit_fabricator_kanim")
		};
		Prioritizable.AddRef(go);
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		this.ConfigureRecipes();
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x00080DF0 File Offset: 0x0007EFF0
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(GameTags.BasicRefinedMetals, 300f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, true),
			new ComplexRecipe.RecipeElement(GameTags.Fabrics, 2f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, false)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array, array2), array, array2)
		{
			time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"SuitFabricator"
			},
			requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
			sortOrder = 1,
			recipeCategoryID = ComplexRecipeManager.MakeRecipeCategoryID("SuitFabricator", "StartingMetals", "Atmo_Suit")
		};
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Worn_Atmo_Suit".ToTag(), 1f, true),
			new ComplexRecipe.RecipeElement(GameTags.Fabrics, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, false)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array3, array4), array3, array4);
		complexRecipe.time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME;
		complexRecipe.description = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.REPAIR_WORN_DESC;
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom;
		complexRecipe.fabricators = new List<Tag>
		{
			"SuitFabricator"
		};
		complexRecipe.requiredTech = Db.Get().TechItems.atmoSuit.parentTechId;
		complexRecipe.sortOrder = 2;
		complexRecipe.customName = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.REPAIR_WORN_RECIPE_NAME;
		complexRecipe.ProductHasFacade = true;
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Steel.ToString(), 200f),
			new ComplexRecipe.RecipeElement(GameTags.Fabrics, 2f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, false)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Jet_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		JetSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array5, array6), array5, array6)
		{
			time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"SuitFabricator"
			},
			requiredTech = Db.Get().TechItems.jetSuit.parentTechId,
			sortOrder = 3
		};
		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Worn_Jet_Suit".ToTag(), 1f),
			new ComplexRecipe.RecipeElement(GameTags.Fabrics, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, false)
		};
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Jet_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array7, array8), array7, array8);
		complexRecipe2.time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME;
		complexRecipe2.description = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.REPAIR_WORN_DESC;
		complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom;
		complexRecipe2.fabricators = new List<Tag>
		{
			"SuitFabricator"
		};
		complexRecipe2.requiredTech = Db.Get().TechItems.jetSuit.parentTechId;
		complexRecipe2.sortOrder = 4;
		complexRecipe2.customName = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.REPAIR_WORN_RECIPE_NAME;
		if (DlcManager.FeatureRadiationEnabled())
		{
			ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Lead.ToString(), 200f),
				new ComplexRecipe.RecipeElement(SimHashes.Glass.ToString(), 10f)
			};
			ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("Lead_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
			};
			LeadSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array9, array10), array9, array10)
			{
				time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
				description = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.RECIPE_DESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
				fabricators = new List<Tag>
				{
					"SuitFabricator"
				},
				requiredTech = Db.Get().TechItems.leadSuit.parentTechId,
				sortOrder = 5
			};
		}
		if (DlcManager.FeatureRadiationEnabled())
		{
			ComplexRecipe.RecipeElement[] array11 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("Worn_Lead_Suit".ToTag(), 1f),
				new ComplexRecipe.RecipeElement(SimHashes.Glass.ToString(), 5f)
			};
			ComplexRecipe.RecipeElement[] array12 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("Lead_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
			};
			ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array11, array12), array11, array12);
			complexRecipe3.time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME;
			complexRecipe3.description = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.REPAIR_WORN_DESC;
			complexRecipe3.nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom;
			complexRecipe3.fabricators = new List<Tag>
			{
				"SuitFabricator"
			};
			complexRecipe3.requiredTech = Db.Get().TechItems.leadSuit.parentTechId;
			complexRecipe3.sortOrder = 6;
			complexRecipe3.customName = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.REPAIR_WORN_RECIPE_NAME;
		}
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x0008137C File Offset: 0x0007F57C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
		};
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			DiscoveredResources.Instance.Discover("Worn_Atmo_Suit");
			DiscoveredResources.Instance.Discover("Worn_Jet_Suit");
			if (DlcManager.FeatureRadiationEnabled())
			{
				DiscoveredResources.Instance.Discover("Worn_Lead_Suit");
			}
		};
	}

	// Token: 0x04000D71 RID: 3441
	public const string ID = "SuitFabricator";
}
