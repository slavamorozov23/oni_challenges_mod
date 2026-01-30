using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000AC8 RID: 2760
[DebuggerDisplay("{Name}")]
public class Recipe : IHasSortOrder
{
	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06005047 RID: 20551 RVA: 0x001D24C7 File Offset: 0x001D06C7
	// (set) Token: 0x06005048 RID: 20552 RVA: 0x001D24CF File Offset: 0x001D06CF
	public int sortOrder { get; set; }

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x0600504A RID: 20554 RVA: 0x001D24E1 File Offset: 0x001D06E1
	// (set) Token: 0x06005049 RID: 20553 RVA: 0x001D24D8 File Offset: 0x001D06D8
	public string Name
	{
		get
		{
			if (this.nameOverride != null)
			{
				return this.nameOverride;
			}
			return this.Result.ProperName();
		}
		set
		{
			this.nameOverride = value;
		}
	}

	// Token: 0x0600504B RID: 20555 RVA: 0x001D24FD File Offset: 0x001D06FD
	public Recipe()
	{
	}

	// Token: 0x0600504C RID: 20556 RVA: 0x001D2510 File Offset: 0x001D0710
	public Recipe(string prefabId, float outputUnits = 1f, SimHashes elementOverride = (SimHashes)0, string nameOverride = null, string recipeDescription = null, int sortOrder = 0)
	{
		global::Debug.Assert(prefabId != null);
		this.Result = TagManager.Create(prefabId);
		this.ResultElementOverride = elementOverride;
		this.nameOverride = nameOverride;
		this.OutputUnits = outputUnits;
		this.Ingredients = new List<Recipe.Ingredient>();
		this.recipeDescription = recipeDescription;
		this.sortOrder = sortOrder;
		this.FabricationVisualizer = null;
	}

	// Token: 0x0600504D RID: 20557 RVA: 0x001D257B File Offset: 0x001D077B
	public Recipe SetFabricator(string fabricator, float fabricationTime)
	{
		this.fabricators = new string[]
		{
			fabricator
		};
		this.FabricationTime = fabricationTime;
		RecipeManager.Get().Add(this);
		return this;
	}

	// Token: 0x0600504E RID: 20558 RVA: 0x001D25A0 File Offset: 0x001D07A0
	public Recipe SetFabricators(string[] fabricators, float fabricationTime)
	{
		this.fabricators = fabricators;
		this.FabricationTime = fabricationTime;
		RecipeManager.Get().Add(this);
		return this;
	}

	// Token: 0x0600504F RID: 20559 RVA: 0x001D25BC File Offset: 0x001D07BC
	public Recipe SetIcon(Sprite Icon)
	{
		this.Icon = Icon;
		this.IconColor = Color.white;
		return this;
	}

	// Token: 0x06005050 RID: 20560 RVA: 0x001D25D1 File Offset: 0x001D07D1
	public Recipe SetIcon(Sprite Icon, Color IconColor)
	{
		this.Icon = Icon;
		this.IconColor = IconColor;
		return this;
	}

	// Token: 0x06005051 RID: 20561 RVA: 0x001D25E2 File Offset: 0x001D07E2
	public Recipe AddIngredient(Recipe.Ingredient ingredient)
	{
		this.Ingredients.Add(ingredient);
		return this;
	}

	// Token: 0x06005052 RID: 20562 RVA: 0x001D25F4 File Offset: 0x001D07F4
	public Recipe.Ingredient[] GetAllIngredients(IList<Tag> selectedTags)
	{
		List<Recipe.Ingredient> list = new List<Recipe.Ingredient>();
		for (int i = 0; i < this.Ingredients.Count; i++)
		{
			float amount = this.Ingredients[i].amount;
			if (i < selectedTags.Count)
			{
				list.Add(new Recipe.Ingredient(selectedTags[i], amount));
			}
			else
			{
				list.Add(new Recipe.Ingredient(this.Ingredients[i].tag, amount));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06005053 RID: 20563 RVA: 0x001D2670 File Offset: 0x001D0870
	public Recipe.Ingredient[] GetAllIngredients(IList<Element> selected_elements)
	{
		List<Recipe.Ingredient> list = new List<Recipe.Ingredient>();
		for (int i = 0; i < this.Ingredients.Count; i++)
		{
			int num = (int)this.Ingredients[i].amount;
			bool flag = false;
			if (i < selected_elements.Count)
			{
				Element element = selected_elements[i];
				if (element != null && element.HasTag(this.Ingredients[i].tag))
				{
					list.Add(new Recipe.Ingredient(GameTagExtensions.Create(element.id), (float)num));
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(new Recipe.Ingredient(this.Ingredients[i].tag, (float)num));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06005054 RID: 20564 RVA: 0x001D2728 File Offset: 0x001D0928
	public GameObject Craft(Storage resource_storage, IList<Tag> selectedTags)
	{
		Recipe.Ingredient[] allIngredients = this.GetAllIngredients(selectedTags);
		return this.CraftRecipe(resource_storage, allIngredients);
	}

	// Token: 0x06005055 RID: 20565 RVA: 0x001D2748 File Offset: 0x001D0948
	private GameObject CraftRecipe(Storage resource_storage, Recipe.Ingredient[] ingredientTags)
	{
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		float num = 0f;
		float num2 = 0f;
		foreach (Recipe.Ingredient ingredient in ingredientTags)
		{
			GameObject gameObject = resource_storage.FindFirst(ingredient.tag);
			if (gameObject != null)
			{
				Edible component = gameObject.GetComponent<Edible>();
				if (component)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
				}
			}
			SimUtil.DiseaseInfo b;
			float temp;
			resource_storage.ConsumeAndGetDisease(ingredient, out b, out temp);
			diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo, b);
			num = SimUtil.CalculateFinalTemperature(num2, num, ingredient.amount, temp);
			num2 += ingredient.amount;
		}
		GameObject prefab = Assets.GetPrefab(this.Result);
		GameObject gameObject2 = null;
		if (prefab != null)
		{
			gameObject2 = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
			gameObject2.GetComponent<KSelectable>().entityName = this.Name;
			if (component2 != null)
			{
				gameObject2.GetComponent<KPrefabID>().RemoveTag(TagManager.Create("Vacuum"));
				if (this.ResultElementOverride != (SimHashes)0)
				{
					if (component2.GetComponent<ElementChunk>() != null)
					{
						component2.SetElement(this.ResultElementOverride, true);
					}
					else
					{
						component2.ElementID = this.ResultElementOverride;
					}
				}
				component2.Temperature = num;
				component2.Units = this.OutputUnits;
			}
			Edible component3 = gameObject2.GetComponent<Edible>();
			if (component3)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component3.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED, "{0}", component3.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
			}
			gameObject2.SetActive(true);
			if (component2 != null)
			{
				component2.AddDisease(diseaseInfo.idx, diseaseInfo.count, "Recipe.CraftRecipe");
			}
			gameObject2.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
		}
		return gameObject2;
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06005056 RID: 20566 RVA: 0x001D2950 File Offset: 0x001D0B50
	public string[] MaterialOptionNames
	{
		get
		{
			List<string> list = new List<string>();
			foreach (Element element in ElementLoader.elements)
			{
				if (Array.IndexOf<Tag>(element.oreTags, this.Ingredients[0].tag) >= 0)
				{
					list.Add(element.id.ToString());
				}
			}
			return list.ToArray();
		}
	}

	// Token: 0x06005057 RID: 20567 RVA: 0x001D29E0 File Offset: 0x001D0BE0
	public Element[] MaterialOptions()
	{
		List<Element> list = new List<Element>();
		foreach (Element element in ElementLoader.elements)
		{
			if (Array.IndexOf<Tag>(element.oreTags, this.Ingredients[0].tag) >= 0)
			{
				list.Add(element);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06005058 RID: 20568 RVA: 0x001D2A60 File Offset: 0x001D0C60
	public BuildingDef GetBuildingDef()
	{
		BuildingComplete component = Assets.GetPrefab(this.Result).GetComponent<BuildingComplete>();
		if (component != null)
		{
			return component.Def;
		}
		return null;
	}

	// Token: 0x06005059 RID: 20569 RVA: 0x001D2A90 File Offset: 0x001D0C90
	public Sprite GetUIIcon()
	{
		Sprite result = null;
		if (this.Icon != null)
		{
			result = this.Icon;
		}
		else
		{
			KBatchedAnimController component = Assets.GetPrefab(this.Result).GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
			}
		}
		return result;
	}

	// Token: 0x0600505A RID: 20570 RVA: 0x001D2AEA File Offset: 0x001D0CEA
	public Color GetUIColor()
	{
		if (!(this.Icon != null))
		{
			return Color.white;
		}
		return this.IconColor;
	}

	// Token: 0x04003594 RID: 13716
	private string nameOverride;

	// Token: 0x04003595 RID: 13717
	public string HotKey;

	// Token: 0x04003596 RID: 13718
	public string Type;

	// Token: 0x04003597 RID: 13719
	public List<Recipe.Ingredient> Ingredients;

	// Token: 0x04003598 RID: 13720
	public string recipeDescription;

	// Token: 0x04003599 RID: 13721
	public Tag Result;

	// Token: 0x0400359A RID: 13722
	public GameObject FabricationVisualizer;

	// Token: 0x0400359B RID: 13723
	public SimHashes ResultElementOverride;

	// Token: 0x0400359C RID: 13724
	public Sprite Icon;

	// Token: 0x0400359D RID: 13725
	public Color IconColor = Color.white;

	// Token: 0x0400359E RID: 13726
	public string[] fabricators;

	// Token: 0x0400359F RID: 13727
	public float OutputUnits;

	// Token: 0x040035A0 RID: 13728
	public float FabricationTime;

	// Token: 0x040035A1 RID: 13729
	public string TechUnlock;

	// Token: 0x02001C0E RID: 7182
	[DebuggerDisplay("{tag} {amount}")]
	[Serializable]
	public class Ingredient
	{
		// Token: 0x0600AC4F RID: 44111 RVA: 0x003CBC1F File Offset: 0x003C9E1F
		public Ingredient(string tag, float amount)
		{
			this.tag = TagManager.Create(tag);
			this.amount = amount;
		}

		// Token: 0x0600AC50 RID: 44112 RVA: 0x003CBC3A File Offset: 0x003C9E3A
		public Ingredient(Tag tag, float amount)
		{
			this.tag = tag;
			this.amount = amount;
		}

		// Token: 0x0600AC51 RID: 44113 RVA: 0x003CBC50 File Offset: 0x003C9E50
		public List<Element> GetElementOptions()
		{
			List<Element> list = new List<Element>(ElementLoader.elements);
			list.RemoveAll((Element e) => !e.IsSolid);
			list.RemoveAll((Element e) => !e.HasTag(this.tag));
			return list;
		}

		// Token: 0x040086D5 RID: 34517
		public Tag tag;

		// Token: 0x040086D6 RID: 34518
		public float amount;
	}
}
