using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CD4 RID: 3284
public class CodexRecipePanel : CodexWidget<CodexRecipePanel>
{
	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x0600655B RID: 25947 RVA: 0x00262488 File Offset: 0x00260688
	// (set) Token: 0x0600655C RID: 25948 RVA: 0x00262490 File Offset: 0x00260690
	public string linkID { get; set; }

	// Token: 0x0600655D RID: 25949 RVA: 0x00262499 File Offset: 0x00260699
	public CodexRecipePanel()
	{
	}

	// Token: 0x0600655E RID: 25950 RVA: 0x002624A1 File Offset: 0x002606A1
	public CodexRecipePanel(ComplexRecipe recipe, bool shouldUseFabricatorForTitle = false)
	{
		this.complexRecipe = recipe;
		this.useFabricatorForTitle = shouldUseFabricatorForTitle;
	}

	// Token: 0x0600655F RID: 25951 RVA: 0x002624B7 File Offset: 0x002606B7
	public CodexRecipePanel(Recipe rec)
	{
		this.recipe = rec;
	}

	// Token: 0x06006560 RID: 25952 RVA: 0x002624C8 File Offset: 0x002606C8
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		this.title = component.GetReference<LocText>("Title");
		this.materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
		this.fabricatorPrefab = component.GetReference<RectTransform>("FabricatorPrefab").gameObject;
		this.ingredientsContainer = component.GetReference<RectTransform>("IngredientsContainer").gameObject;
		this.resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
		this.fabricatorContainer = component.GetReference<RectTransform>("FabricatorContainer").gameObject;
		this.ClearPanel();
		if (this.recipe != null)
		{
			this.ConfigureRecipe();
			return;
		}
		if (this.complexRecipe != null && Game.IsCorrectDlcActiveForCurrentSave(this.complexRecipe))
		{
			this.ConfigureComplexRecipe();
		}
	}

	// Token: 0x06006561 RID: 25953 RVA: 0x0026258C File Offset: 0x0026078C
	private void ConfigureRecipe()
	{
		this.title.text = this.recipe.Result.ProperName();
		foreach (Recipe.Ingredient ingredient in this.recipe.Ingredients)
		{
			GameObject gameObject = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(ingredient.tag, "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(ingredient.tag, ingredient.amount, GameUtil.TimeSlice.None);
			component.GetReference<LocText>("Amount").color = Color.black;
			string text = ingredient.tag.ProperName();
			GameObject prefab = Assets.GetPrefab(ingredient.tag);
			if (prefab.GetComponent<Edible>() != null)
			{
				text = text + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()));
			}
			gameObject.GetComponent<ToolTip>().toolTip = text;
		}
		GameObject gameObject2 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true);
		HierarchyReferences component2 = gameObject2.GetComponent<HierarchyReferences>();
		global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite(this.recipe.Result, "ui", false);
		component2.GetReference<Image>("Icon").sprite = uisprite2.first;
		component2.GetReference<Image>("Icon").color = uisprite2.second;
		component2.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(this.recipe.Result, this.recipe.OutputUnits, GameUtil.TimeSlice.None);
		component2.GetReference<LocText>("Amount").color = Color.black;
		string text2 = this.recipe.Result.ProperName();
		GameObject prefab2 = Assets.GetPrefab(this.recipe.Result);
		if (prefab2.GetComponent<Edible>() != null)
		{
			text2 = text2 + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab2.GetComponent<Edible>().GetQuality()));
		}
		gameObject2.GetComponent<ToolTip>().toolTip = text2;
	}

	// Token: 0x06006562 RID: 25954 RVA: 0x00262810 File Offset: 0x00260A10
	private void ConfigureComplexRecipe()
	{
		ComplexRecipe.RecipeElement[] array = this.complexRecipe.ingredients;
		for (int i = 0; i < array.Length; i++)
		{
			ComplexRecipe.RecipeElement ing = array[i];
			HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true).GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(ing.material, "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(ing.material, ing.amount, GameUtil.TimeSlice.None);
			component.GetReference<LocText>("Amount").color = Color.black;
			string text = ing.material.ProperName();
			GameObject prefab = Assets.GetPrefab(ing.material);
			if (prefab.GetComponent<Edible>() != null)
			{
				text = text + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()));
			}
			component.GetReference<ToolTip>("Tooltip").toolTip = text;
			component.GetReference<KButton>("Button").onClick += delegate()
			{
				ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(Assets.GetPrefab(ing.material).GetProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			};
		}
		array = this.complexRecipe.results;
		for (int i = 0; i < array.Length; i++)
		{
			ComplexRecipe.RecipeElement res = array[i];
			HierarchyReferences component2 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite(res.material, "ui", false);
			component2.GetReference<Image>("Icon").sprite = uisprite2.first;
			component2.GetReference<Image>("Icon").color = uisprite2.second;
			component2.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(res.material, res.amount, GameUtil.TimeSlice.None);
			component2.GetReference<LocText>("Amount").color = Color.black;
			string text2 = res.material.ProperName();
			GameObject prefab2 = Assets.GetPrefab(res.material);
			if (prefab2.GetComponent<Edible>() != null)
			{
				text2 = text2 + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab2.GetComponent<Edible>().GetQuality()));
			}
			component2.GetReference<ToolTip>("Tooltip").toolTip = text2;
			component2.GetReference<KButton>("Button").onClick += delegate()
			{
				ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(Assets.GetPrefab(res.material).GetProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			};
		}
		DebugUtil.DevAssert(this.complexRecipe.fabricators.Count > 0, "Codex assumes there is at most one fabricator per recipe, refactor if needed", null);
		string name = this.complexRecipe.fabricators[0].Name;
		HierarchyReferences component3 = Util.KInstantiateUI(this.fabricatorPrefab, this.fabricatorContainer, true).GetComponent<HierarchyReferences>();
		global::Tuple<Sprite, Color> uisprite3 = Def.GetUISprite(name, "ui", false);
		component3.GetReference<Image>("Icon").sprite = uisprite3.first;
		component3.GetReference<Image>("Icon").color = uisprite3.second;
		component3.GetReference<LocText>("Time").text = GameUtil.GetFormattedTime(this.complexRecipe.time, "F0");
		component3.GetReference<LocText>("Time").color = Color.black;
		GameObject fabricator = Assets.GetPrefab(name.ToTag());
		component3.GetReference<ToolTip>("Tooltip").toolTip = fabricator.GetProperName();
		component3.GetReference<KButton>("Button").onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(fabricator.GetProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
		if (this.useFabricatorForTitle)
		{
			this.title.text = fabricator.GetProperName();
			return;
		}
		this.title.text = this.complexRecipe.results[0].material.ProperName();
	}

	// Token: 0x06006563 RID: 25955 RVA: 0x00262C48 File Offset: 0x00260E48
	private void ClearPanel()
	{
		foreach (object obj in this.ingredientsContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		foreach (object obj2 in this.resultsContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj2).gameObject);
		}
		foreach (object obj3 in this.fabricatorContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj3).gameObject);
		}
	}

	// Token: 0x040044AE RID: 17582
	private LocText title;

	// Token: 0x040044AF RID: 17583
	private GameObject materialPrefab;

	// Token: 0x040044B0 RID: 17584
	private GameObject fabricatorPrefab;

	// Token: 0x040044B1 RID: 17585
	private GameObject ingredientsContainer;

	// Token: 0x040044B2 RID: 17586
	private GameObject resultsContainer;

	// Token: 0x040044B3 RID: 17587
	private GameObject fabricatorContainer;

	// Token: 0x040044B4 RID: 17588
	private ComplexRecipe complexRecipe;

	// Token: 0x040044B5 RID: 17589
	private Recipe recipe;

	// Token: 0x040044B6 RID: 17590
	private bool useFabricatorForTitle;
}
