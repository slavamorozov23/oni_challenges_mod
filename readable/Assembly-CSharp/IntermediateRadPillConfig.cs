using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public class IntermediateRadPillConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000F18 RID: 3864 RVA: 0x000575A8 File Offset: 0x000557A8
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x000575AF File Offset: 0x000557AF
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x000575B4 File Offset: 0x000557B4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateRadPill", STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.NAME, STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.DESC, 1f, true, Assets.GetAnim("vial_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATERADPILL);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateRadPill".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		IntermediateRadPillConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedApothecary", array, array2), array, array2)
		{
			time = 50f,
			description = STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"AdvancedApothecary"
			},
			sortOrder = 21
		};
		return gameObject;
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x000576C8 File Offset: 0x000558C8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x000576CA File Offset: 0x000558CA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009C4 RID: 2500
	public const string ID = "IntermediateRadPill";

	// Token: 0x040009C5 RID: 2501
	public static ComplexRecipe recipe;
}
