using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C43 RID: 3139
public class ComplexRecipe : IHasDlcRestrictions
{
	// Token: 0x06005EEB RID: 24299 RVA: 0x0022B741 File Offset: 0x00229941
	public void SetDLCRestrictions(string[] required, string[] forbidden)
	{
		this.requiredDlcIds = required;
		this.forbiddenDlcIds = forbidden;
	}

	// Token: 0x06005EEC RID: 24300 RVA: 0x0022B751 File Offset: 0x00229951
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06005EED RID: 24301 RVA: 0x0022B759 File Offset: 0x00229959
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x06005EEE RID: 24302 RVA: 0x0022B761 File Offset: 0x00229961
	// (set) Token: 0x06005EEF RID: 24303 RVA: 0x0022B769 File Offset: 0x00229969
	public bool ProductHasFacade { get; set; }

	// Token: 0x06005EF0 RID: 24304 RVA: 0x0022B774 File Offset: 0x00229974
	public bool IsAnyProductDeprecated()
	{
		ComplexRecipe.RecipeElement[] array = this.results;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject prefab = Assets.GetPrefab(array[i].material);
			if (prefab != null && prefab.HasTag(GameTags.DeprecatedContent))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x06005EF1 RID: 24305 RVA: 0x0022B7BD File Offset: 0x002299BD
	// (set) Token: 0x06005EF2 RID: 24306 RVA: 0x0022B7C5 File Offset: 0x002299C5
	public bool RequiresAllIngredientsDiscovered { get; set; }

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x06005EF3 RID: 24307 RVA: 0x0022B7CE File Offset: 0x002299CE
	public Tag FirstResult
	{
		get
		{
			return this.results[0].material;
		}
	}

	// Token: 0x06005EF4 RID: 24308 RVA: 0x0022B7E0 File Offset: 0x002299E0
	private static GameObject CreateFabricationVisualizer(string anim, string nameRoot = null)
	{
		GameObject gameObject = new GameObject();
		if (nameRoot != null)
		{
			gameObject.name = nameRoot + "Visualizer";
		}
		gameObject.SetActive(false);
		gameObject.transform.SetLocalPosition(Vector3.zero);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(anim)
		};
		kbatchedAnimController.initialAnim = "fabricating";
		kbatchedAnimController.isMovable = true;
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = new HashedString("meter_ration");
		kbatchedAnimTracker.offset = Vector3.zero;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		return gameObject;
	}

	// Token: 0x06005EF5 RID: 24309 RVA: 0x0022B87C File Offset: 0x00229A7C
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results)
	{
		this.id = id;
		this.ingredients = ingredients;
		this.results = results;
		this.recipeCategoryID = ComplexRecipeManager.MakeRecipeCategoryID(id, "Default", results[0].material.ToString());
		if (!ComplexRecipeManager.Get().IsPostProcessing)
		{
			ComplexRecipeManager.Get().preProcessRecipes.Add(this);
		}
	}

	// Token: 0x06005EF6 RID: 24310 RVA: 0x0022B8E5 File Offset: 0x00229AE5
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP) : this(id, ingredients, results)
	{
		this.consumedHEP = consumedHEP;
		this.producedHEP = producedHEP;
	}

	// Token: 0x06005EF7 RID: 24311 RVA: 0x0022B900 File Offset: 0x00229B00
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP) : this(id, ingredients, results, consumedHEP, 0)
	{
	}

	// Token: 0x06005EF8 RID: 24312 RVA: 0x0022B90E File Offset: 0x00229B0E
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, string[] requiredDlcIds) : this(id, ingredients, results, requiredDlcIds, null)
	{
	}

	// Token: 0x06005EF9 RID: 24313 RVA: 0x0022B91C File Offset: 0x00229B1C
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, string[] requiredDlcIds, string[] forbiddenDlcIds) : this(id, ingredients, results)
	{
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06005EFA RID: 24314 RVA: 0x0022B937 File Offset: 0x00229B37
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP, string[] requiredDlcIds) : this(id, ingredients, results, consumedHEP, producedHEP, requiredDlcIds, null)
	{
	}

	// Token: 0x06005EFB RID: 24315 RVA: 0x0022B949 File Offset: 0x00229B49
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP, string[] requiredDlcIds, string[] forbiddenDlcIds) : this(id, ingredients, results, consumedHEP, producedHEP)
	{
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06005EFC RID: 24316 RVA: 0x0022B968 File Offset: 0x00229B68
	public void SetFabricationAnim(string anim)
	{
		this.FabricationVisualizer = ComplexRecipe.CreateFabricationVisualizer(anim, this.id);
	}

	// Token: 0x06005EFD RID: 24317 RVA: 0x0022B97C File Offset: 0x00229B7C
	public float TotalResultUnits()
	{
		float num = 0f;
		foreach (ComplexRecipe.RecipeElement recipeElement in this.results)
		{
			num += recipeElement.amount;
		}
		return num;
	}

	// Token: 0x06005EFE RID: 24318 RVA: 0x0022B9B2 File Offset: 0x00229BB2
	public bool RequiresTechUnlock()
	{
		return !string.IsNullOrEmpty(this.requiredTech);
	}

	// Token: 0x06005EFF RID: 24319 RVA: 0x0022B9C2 File Offset: 0x00229BC2
	public bool IsRequiredTechUnlocked()
	{
		return string.IsNullOrEmpty(this.requiredTech) || Db.Get().Techs.Get(this.requiredTech).IsComplete();
	}

	// Token: 0x06005F00 RID: 24320 RVA: 0x0022B9F0 File Offset: 0x00229BF0
	public Sprite GetUIIcon()
	{
		Sprite result = null;
		Tag tag = (this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient) ? this.ingredients[0].material : this.results[0].material;
		if (this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Custom && !string.IsNullOrEmpty(this.customSpritePrefabID))
		{
			tag = this.customSpritePrefabID;
		}
		KBatchedAnimController component = Assets.GetPrefab(tag).GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
		}
		return result;
	}

	// Token: 0x06005F01 RID: 24321 RVA: 0x0022BA75 File Offset: 0x00229C75
	public Color GetUIColor()
	{
		return Color.white;
	}

	// Token: 0x06005F02 RID: 24322 RVA: 0x0022BA7C File Offset: 0x00229C7C
	public string GetUIName(bool includeAmounts)
	{
		string text = this.results[0].facadeID.IsNullOrWhiteSpace() ? this.results[0].material.ProperName() : this.results[0].facadeID.ProperName();
		switch (this.nameDisplay)
		{
		case ComplexRecipe.RecipeNameDisplay.Result:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, text, this.results[0].amount);
			}
			return text;
		case ComplexRecipe.RecipeNameDisplay.IngredientToResult:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.ResultWithIngredient:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Composite:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.results[0].amount,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE, this.ingredients[0].material.ProperName(), text, this.results[1].material.ProperName());
		case ComplexRecipe.RecipeNameDisplay.HEP:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.producedHEP,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Custom:
			return this.customName;
		}
		if (includeAmounts)
		{
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, this.ingredients[0].material.ProperName(), this.ingredients[0].amount);
		}
		return this.ingredients[0].material.ProperName();
	}

	// Token: 0x04003F5D RID: 16221
	public string id;

	// Token: 0x04003F5E RID: 16222
	public string recipeCategoryID;

	// Token: 0x04003F5F RID: 16223
	public ComplexRecipe.RecipeElement[] ingredients;

	// Token: 0x04003F60 RID: 16224
	public ComplexRecipe.RecipeElement[] results;

	// Token: 0x04003F61 RID: 16225
	public float time;

	// Token: 0x04003F62 RID: 16226
	public GameObject FabricationVisualizer;

	// Token: 0x04003F63 RID: 16227
	public int consumedHEP;

	// Token: 0x04003F64 RID: 16228
	public int producedHEP;

	// Token: 0x04003F65 RID: 16229
	private string[] requiredDlcIds;

	// Token: 0x04003F66 RID: 16230
	private string[] forbiddenDlcIds;

	// Token: 0x04003F68 RID: 16232
	public ComplexRecipe.RecipeNameDisplay nameDisplay;

	// Token: 0x04003F69 RID: 16233
	public string customName;

	// Token: 0x04003F6A RID: 16234
	public string customSpritePrefabID;

	// Token: 0x04003F6B RID: 16235
	public string description;

	// Token: 0x04003F6C RID: 16236
	public Func<string> runTimeDescription;

	// Token: 0x04003F6D RID: 16237
	public List<Tag> fabricators;

	// Token: 0x04003F6E RID: 16238
	public int sortOrder;

	// Token: 0x04003F6F RID: 16239
	public string requiredTech;

	// Token: 0x02001DE7 RID: 7655
	public enum RecipeNameDisplay
	{
		// Token: 0x04008C91 RID: 35985
		Ingredient,
		// Token: 0x04008C92 RID: 35986
		Result,
		// Token: 0x04008C93 RID: 35987
		IngredientToResult,
		// Token: 0x04008C94 RID: 35988
		ResultWithIngredient,
		// Token: 0x04008C95 RID: 35989
		Composite,
		// Token: 0x04008C96 RID: 35990
		HEP,
		// Token: 0x04008C97 RID: 35991
		Custom
	}

	// Token: 0x02001DE8 RID: 7656
	public class RecipeElement
	{
		// Token: 0x0600B27E RID: 45694 RVA: 0x003E04D3 File Offset: 0x003DE6D3
		public RecipeElement(Tag[] materialOptions, float amount)
		{
			this.material = null;
			this.possibleMaterials = materialOptions;
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
		}

		// Token: 0x0600B27F RID: 45695 RVA: 0x003E04FC File Offset: 0x003DE6FC
		public RecipeElement(Tag[] materialOptions, float[] amounts)
		{
			this.material = null;
			this.possibleMaterials = materialOptions;
			this.possibleMaterialAmounts = amounts;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
		}

		// Token: 0x0600B280 RID: 45696 RVA: 0x003E0528 File Offset: 0x003DE728
		public RecipeElement(Tag[] materialOptions, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, string facadeID, bool storeElement = false, bool inheritElement = false)
		{
			this.material = null;
			this.possibleMaterials = materialOptions;
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
			this.facadeID = facadeID;
			this.inheritElement = inheritElement;
		}

		// Token: 0x0600B281 RID: 45697 RVA: 0x003E0574 File Offset: 0x003DE774
		public RecipeElement(Tag[] materialOptions, float[] amounts, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, string facadeID, bool storeElement = false, bool inheritElement = false, bool doNotConsume = false)
		{
			this.material = null;
			this.possibleMaterials = materialOptions;
			this.possibleMaterialAmounts = amounts;
			this.amount = this.amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
			this.facadeID = facadeID;
			this.inheritElement = inheritElement;
			this.doNotConsume = doNotConsume;
		}

		// Token: 0x0600B282 RID: 45698 RVA: 0x003E05D4 File Offset: 0x003DE7D4
		public RecipeElement(Tag material, float amount, bool inheritElement)
		{
			this.material = material;
			this.possibleMaterials = new Tag[]
			{
				material
			};
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
			this.inheritElement = inheritElement;
		}

		// Token: 0x0600B283 RID: 45699 RVA: 0x003E060C File Offset: 0x003DE80C
		public RecipeElement(Tag material, float amount)
		{
			this.material = material;
			this.possibleMaterials = new Tag[]
			{
				material
			};
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
		}

		// Token: 0x0600B284 RID: 45700 RVA: 0x003E063D File Offset: 0x003DE83D
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, bool storeElement = false)
		{
			this.material = material;
			this.possibleMaterials = new Tag[]
			{
				material
			};
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
		}

		// Token: 0x0600B285 RID: 45701 RVA: 0x003E0678 File Offset: 0x003DE878
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, string facadeID, bool storeElement = false)
		{
			this.material = material;
			this.possibleMaterials = new Tag[]
			{
				material
			};
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
			this.facadeID = facadeID;
		}

		// Token: 0x0600B286 RID: 45702 RVA: 0x003E06C4 File Offset: 0x003DE8C4
		public RecipeElement(EdiblesManager.FoodInfo foodInfo, float amount, bool DoNotConsume = false)
		{
			this.material = foodInfo.Id;
			this.possibleMaterials = new Tag[]
			{
				this.material
			};
			this.amount = amount;
			this.doNotConsume = DoNotConsume;
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x0600B287 RID: 45703 RVA: 0x003E0704 File Offset: 0x003DE904
		// (set) Token: 0x0600B288 RID: 45704 RVA: 0x003E070C File Offset: 0x003DE90C
		public float amount { get; set; }

		// Token: 0x04008C98 RID: 35992
		public Tag material;

		// Token: 0x04008C99 RID: 35993
		public Tag[] possibleMaterials;

		// Token: 0x04008C9A RID: 35994
		public float[] possibleMaterialAmounts;

		// Token: 0x04008C9C RID: 35996
		public ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation;

		// Token: 0x04008C9D RID: 35997
		public bool storeElement;

		// Token: 0x04008C9E RID: 35998
		public bool inheritElement;

		// Token: 0x04008C9F RID: 35999
		public string facadeID;

		// Token: 0x04008CA0 RID: 36000
		public bool doNotConsume;

		// Token: 0x02002A54 RID: 10836
		public struct IngredientDataSet
		{
			// Token: 0x0600D462 RID: 54370 RVA: 0x0043C439 File Offset: 0x0043A639
			public IngredientDataSet(Tag[] substitutionOptions, float[] amounts)
			{
				this.substitutionOptions = substitutionOptions;
				this.amounts = amounts;
			}

			// Token: 0x0400BB0D RID: 47885
			public Tag[] substitutionOptions;

			// Token: 0x0400BB0E RID: 47886
			public float[] amounts;
		}

		// Token: 0x02002A55 RID: 10837
		public enum TemperatureOperation
		{
			// Token: 0x0400BB10 RID: 47888
			AverageTemperature,
			// Token: 0x0400BB11 RID: 47889
			Heated,
			// Token: 0x0400BB12 RID: 47890
			Melted,
			// Token: 0x0400BB13 RID: 47891
			Dehydrated
		}
	}
}
