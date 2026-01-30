using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E3 RID: 739
public class BasicCureConfig : IEntityConfig
{
	// Token: 0x06000F06 RID: 3846 RVA: 0x000570EC File Offset: 0x000552EC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicCure", STRINGS.ITEMS.PILLS.BASICCURE.NAME, STRINGS.ITEMS.PILLS.BASICCURE.DESC, 1f, true, Assets.GetAnim("pill_foodpoisoning_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.BASICCURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Carbon.CreateTag(), 1f),
			new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicCure", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 50f,
			description = STRINGS.ITEMS.PILLS.BASICCURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 10
		};
		return gameObject;
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x00057215 File Offset: 0x00055415
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x00057217 File Offset: 0x00055417
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009BC RID: 2492
	public const string ID = "BasicCure";

	// Token: 0x040009BD RID: 2493
	public static ComplexRecipe recipe;
}
