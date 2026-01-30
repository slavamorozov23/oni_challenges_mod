using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020002E8 RID: 744
public class LubricationStickConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000F1E RID: 3870 RVA: 0x000576D4 File Offset: 0x000558D4
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x000576DB File Offset: 0x000558DB
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x000576E0 File Offset: 0x000558E0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("LubricationStick", ITEMS.LUBRICATIONSTICK.NAME, ITEMS.LUBRICATIONSTICK.DESC, LubricationStickConfig.MASS_PER_RECIPE, true, Assets.GetAnim("lubricant_applicator_kanim"), "idle1", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.4f, 1f, true, 0, SimHashes.LiquidGunk, null);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddTag(GameTags.MedicalSupplies);
		gameObject.AddTag(GameTags.SolidLubricant);
		gameObject.AddTag(GameTags.PedestalDisplayable);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.LiquidGunk.CreateTag(), GunkMonitor.GUNK_CAPACITY),
			new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 200f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("LubricationStick".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
			new ComplexRecipe.RecipeElement(SimHashes.DirtyWater.CreateTag(), 200f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		LubricationStickConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 100f,
			description = ITEMS.LUBRICATIONSTICK.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 1,
			requiredTech = Db.Get().TechItems.lubricationStick.parentTechId
		};
		return gameObject;
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x00057848 File Offset: 0x00055A48
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0005784A File Offset: 0x00055A4A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009C6 RID: 2502
	public const string ID = "LubricationStick";

	// Token: 0x040009C7 RID: 2503
	public static ComplexRecipe recipe;

	// Token: 0x040009C8 RID: 2504
	private const float WATER_MASS = 200f;

	// Token: 0x040009C9 RID: 2505
	public static float MASS_PER_RECIPE = GunkMonitor.GUNK_CAPACITY;
}
