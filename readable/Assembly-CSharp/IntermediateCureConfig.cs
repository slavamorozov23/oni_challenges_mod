using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E6 RID: 742
public class IntermediateCureConfig : IEntityConfig
{
	// Token: 0x06000F14 RID: 3860 RVA: 0x0005746C File Offset: 0x0005566C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateCure", STRINGS.ITEMS.PILLS.INTERMEDIATECURE.NAME, STRINGS.ITEMS.PILLS.INTERMEDIATECURE.DESC, 1f, true, Assets.GetAnim("iv_slimelung_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject = EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATECURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SwampLilyFlowerConfig.ID, 1f),
			new ComplexRecipe.RecipeElement(SimHashes.Phosphorite.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateCure", 1f)
		};
		string text = "Apothecary";
		IntermediateCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(text, array, array2), array, array2)
		{
			time = 100f,
			description = STRINGS.ITEMS.PILLS.INTERMEDIATECURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				text
			},
			sortOrder = 10,
			requiredTech = "MedicineII"
		};
		return gameObject;
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0005759C File Offset: 0x0005579C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x0005759E File Offset: 0x0005579E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009C2 RID: 2498
	public const string ID = "IntermediateCure";

	// Token: 0x040009C3 RID: 2499
	public static ComplexRecipe recipe;
}
