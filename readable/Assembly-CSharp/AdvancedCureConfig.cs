using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E0 RID: 736
public class AdvancedCureConfig : IEntityConfig
{
	// Token: 0x06000EF8 RID: 3832 RVA: 0x00056D1C File Offset: 0x00054F1C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("AdvancedCure", STRINGS.ITEMS.PILLS.ADVANCEDCURE.NAME, STRINGS.ITEMS.PILLS.ADVANCEDCURE.DESC, 1f, true, Assets.GetAnim("vial_spore_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject = EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.ADVANCEDCURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Steel.CreateTag(), 1f),
			new ComplexRecipe.RecipeElement("LightBugOrangeEgg", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("AdvancedCure", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string text = "Apothecary";
		AdvancedCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(text, array, array2), array, array2)
		{
			time = 200f,
			description = STRINGS.ITEMS.PILLS.ADVANCEDCURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				text
			},
			sortOrder = 20,
			requiredTech = "MedicineIV"
		};
		return gameObject;
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x00056E4E File Offset: 0x0005504E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x00056E50 File Offset: 0x00055050
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009B6 RID: 2486
	public const string ID = "AdvancedCure";

	// Token: 0x040009B7 RID: 2487
	public static ComplexRecipe recipe;
}
