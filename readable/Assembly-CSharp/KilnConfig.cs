using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000286 RID: 646
public class KilnConfig : IBuildingConfig
{
	// Token: 0x06000D1F RID: 3359 RVA: 0x0004DF7C File Offset: 0x0004C17C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Kiln";
		int width = 2;
		int height = 2;
		string anim = "kiln_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = false;
		buildingDef.ExhaustKilowattsWhenActive = 16f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		return buildingDef;
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x0004E004 File Offset: 0x0004C204
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 353.15f;
		complexFabricator.duplicantOperated = false;
		complexFabricator.showProgressBar = true;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		this.ConfigureRecipes();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x0004E080 File Offset: 0x0004C280
	private void ConfigureRecipes()
	{
		Tag tag = SimHashes.Ceramic.CreateTag();
		Tag material = SimHashes.Clay.CreateTag();
		Tag tag2 = SimHashes.Carbon.CreateTag();
		Tag tag3 = SimHashes.Peat.CreateTag();
		float amount = 100f;
		float amount2 = 25f;
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(material, amount),
			new ComplexRecipe.RecipeElement(GameTags.BasicWoods.Append(new Tag[]
			{
				SimHashes.Carbon.CreateTag(),
				SimHashes.Peat.CreateTag()
			}), amount2)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(tag, amount, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag);
		string text = ComplexRecipeManager.MakeRecipeID("Kiln", array, array2);
		ComplexRecipe complexRecipe = new ComplexRecipe(text, array, array2);
		complexRecipe.time = 40f;
		complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Clay).name, ElementLoader.FindElementByHash(SimHashes.Ceramic).name);
		complexRecipe.fabricators = new List<Tag>
		{
			TagManager.Create("Kiln")
		};
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe.sortOrder = 100;
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		Tag tag4 = SimHashes.RefinedCarbon.CreateTag();
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(GameTags.BasicWoods.Append(new Tag[]
			{
				tag2,
				tag3
			}), new float[]
			{
				200f,
				200f,
				125f,
				300f
			})
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(tag4, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		string obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag4);
		string text2 = ComplexRecipeManager.MakeRecipeID("Kiln", array3, array4);
		ComplexRecipe complexRecipe2 = new ComplexRecipe(text2, array3, array4);
		complexRecipe2.time = 40f;
		complexRecipe2.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Carbon).name, ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).name);
		complexRecipe2.fabricators = new List<Tag>
		{
			TagManager.Create("Kiln")
		};
		complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe2.sortOrder = 200;
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0004E2CF File Offset: 0x0004C4CF
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x040008FE RID: 2302
	public const string ID = "Kiln";

	// Token: 0x040008FF RID: 2303
	public const float INPUT_CLAY_PER_SECOND = 1f;

	// Token: 0x04000900 RID: 2304
	public const float CERAMIC_PER_SECOND = 1f;

	// Token: 0x04000901 RID: 2305
	public const float CO2_RATIO = 0.1f;

	// Token: 0x04000902 RID: 2306
	public const float OUTPUT_TEMP = 353.15f;

	// Token: 0x04000903 RID: 2307
	public const float REFILL_RATE = 2400f;

	// Token: 0x04000904 RID: 2308
	public const float CERAMIC_STORAGE_AMOUNT = 2400f;

	// Token: 0x04000905 RID: 2309
	public const float COAL_RATE = 0.1f;
}
