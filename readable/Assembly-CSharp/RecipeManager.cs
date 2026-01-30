using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C41 RID: 3137
public class RecipeManager
{
	// Token: 0x06005ED6 RID: 24278 RVA: 0x0022ADC3 File Offset: 0x00228FC3
	public static RecipeManager Get()
	{
		if (RecipeManager._Instance == null)
		{
			RecipeManager._Instance = new RecipeManager();
		}
		return RecipeManager._Instance;
	}

	// Token: 0x06005ED7 RID: 24279 RVA: 0x0022ADDB File Offset: 0x00228FDB
	public static void DestroyInstance()
	{
		RecipeManager._Instance = null;
	}

	// Token: 0x06005ED8 RID: 24280 RVA: 0x0022ADE3 File Offset: 0x00228FE3
	public void Add(Recipe recipe)
	{
		this.recipes.Add(recipe);
		if (recipe.FabricationVisualizer != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
		}
	}

	// Token: 0x04003F56 RID: 16214
	private static RecipeManager _Instance;

	// Token: 0x04003F57 RID: 16215
	public List<Recipe> recipes = new List<Recipe>();
}
