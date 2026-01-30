using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020002E9 RID: 745
public class TallowLubricationStickConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000F25 RID: 3877 RVA: 0x00057860 File Offset: 0x00055A60
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x00057867 File Offset: 0x00055A67
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x0005786C File Offset: 0x00055A6C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("TallowLubricationStick", ITEMS.TALLOWLUBRICATIONSTICK.NAME, ITEMS.TALLOWLUBRICATIONSTICK.DESC, TallowLubricationStickConfig.MASS_PER_RECIPE, true, Assets.GetAnim("lubricant_applicator_tallow_kanim"), "idle1", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.4f, 1f, true, 0, SimHashes.Tallow, null);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddTag(GameTags.MedicalSupplies);
		gameObject.AddTag(GameTags.SolidLubricant);
		gameObject.AddTag(GameTags.PedestalDisplayable);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Tallow.CreateTag(), 10f),
			new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), GunkMonitor.GUNK_CAPACITY - 10f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("TallowLubricationStick".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		TallowLubricationStickConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 100f,
			description = ITEMS.TALLOWLUBRICATIONSTICK.RECIPEDESC,
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

	// Token: 0x06000F28 RID: 3880 RVA: 0x000579C1 File Offset: 0x00055BC1
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x000579C3 File Offset: 0x00055BC3
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009CA RID: 2506
	public const string ID = "TallowLubricationStick";

	// Token: 0x040009CB RID: 2507
	public static ComplexRecipe recipe;

	// Token: 0x040009CC RID: 2508
	public static float MASS_PER_RECIPE = GunkMonitor.GUNK_CAPACITY;
}
