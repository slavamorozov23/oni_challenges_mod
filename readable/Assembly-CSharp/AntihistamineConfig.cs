using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E1 RID: 737
public class AntihistamineConfig : IEntityConfig
{
	// Token: 0x06000EFC RID: 3836 RVA: 0x00056E5C File Offset: 0x0005505C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Antihistamine", STRINGS.ITEMS.PILLS.ANTIHISTAMINE.NAME, STRINGS.ITEMS.PILLS.ANTIHISTAMINE.DESC, 1f, true, Assets.GetAnim("pill_allergies_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.ANTIHISTAMINE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Antihistamine", 10f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(new Tag[]
			{
				"PrickleFlowerSeed",
				KelpConfig.ID
			}, new float[]
			{
				1f,
				10f
			}),
			new ComplexRecipe.RecipeElement(SimHashes.Dirt.CreateTag(), 1f)
		};
		string recipeID = ComplexRecipeManager.MakeRecipeID("Apothecary", array2, array);
		AntihistamineConfig.recipes.Add(this.CreateComplexRecipe(recipeID, array2, array));
		return gameObject;
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x00056F68 File Offset: 0x00055168
	public ComplexRecipe CreateComplexRecipe(string recipeID, ComplexRecipe.RecipeElement[] input, ComplexRecipe.RecipeElement[] output)
	{
		return new ComplexRecipe(recipeID, input, output)
		{
			time = 100f,
			description = STRINGS.ITEMS.PILLS.ANTIHISTAMINE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 10
		};
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x00056FC2 File Offset: 0x000551C2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x00056FC4 File Offset: 0x000551C4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009B8 RID: 2488
	public const string ID = "Antihistamine";

	// Token: 0x040009B9 RID: 2489
	public static List<ComplexRecipe> recipes = new List<ComplexRecipe>();
}
