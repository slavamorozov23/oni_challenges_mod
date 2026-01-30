using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E5 RID: 741
public class IntermediateBoosterConfig : IEntityConfig
{
	// Token: 0x06000F10 RID: 3856 RVA: 0x0005734C File Offset: 0x0005554C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateBooster", STRINGS.ITEMS.PILLS.INTERMEDIATEBOOSTER.NAME, STRINGS.ITEMS.PILLS.INTERMEDIATEBOOSTER.DESC, 1f, true, Assets.GetAnim("pill_3_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATEBOOSTER);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SpiceNutConfig.ID, 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateBooster", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		IntermediateBoosterConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 100f,
			description = STRINGS.ITEMS.PILLS.INTERMEDIATEBOOSTER.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 5
		};
		return gameObject;
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0005745D File Offset: 0x0005565D
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x0005745F File Offset: 0x0005565F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009C0 RID: 2496
	public const string ID = "IntermediateBooster";

	// Token: 0x040009C1 RID: 2497
	public static ComplexRecipe recipe;
}
