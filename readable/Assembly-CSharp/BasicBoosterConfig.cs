using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class BasicBoosterConfig : IEntityConfig
{
	// Token: 0x06000F02 RID: 3842 RVA: 0x00056FDC File Offset: 0x000551DC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicBooster", STRINGS.ITEMS.PILLS.BASICBOOSTER.NAME, STRINGS.ITEMS.PILLS.BASICBOOSTER.DESC, 1f, true, Assets.GetAnim("pill_2_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.BASICBOOSTER);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicBooster".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicBoosterConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 50f,
			description = STRINGS.ITEMS.PILLS.BASICBOOSTER.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 1
		};
		return gameObject;
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x000570DE File Offset: 0x000552DE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x000570E0 File Offset: 0x000552E0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009BA RID: 2490
	public const string ID = "BasicBooster";

	// Token: 0x040009BB RID: 2491
	public static ComplexRecipe recipe;
}
