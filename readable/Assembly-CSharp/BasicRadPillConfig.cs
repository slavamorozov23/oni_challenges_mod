using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E4 RID: 740
public class BasicRadPillConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000F0A RID: 3850 RVA: 0x00057221 File Offset: 0x00055421
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x00057228 File Offset: 0x00055428
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x0005722C File Offset: 0x0005542C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicRadPill", STRINGS.ITEMS.PILLS.BASICRADPILL.NAME, STRINGS.ITEMS.PILLS.BASICRADPILL.DESC, 1f, true, Assets.GetAnim("pill_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.BASICRADPILL);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicRadPill".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicRadPillConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 50f,
			description = STRINGS.ITEMS.PILLS.BASICRADPILL.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 10
		};
		return gameObject;
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0005733E File Offset: 0x0005553E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x00057340 File Offset: 0x00055540
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009BE RID: 2494
	public const string ID = "BasicRadPill";

	// Token: 0x040009BF RID: 2495
	public static ComplexRecipe recipe;
}
