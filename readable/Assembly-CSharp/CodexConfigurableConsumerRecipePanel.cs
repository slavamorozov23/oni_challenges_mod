using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CD2 RID: 3282
public class CodexConfigurableConsumerRecipePanel : CodexWidget<CodexConfigurableConsumerRecipePanel>
{
	// Token: 0x0600654F RID: 25935 RVA: 0x00261991 File Offset: 0x0025FB91
	public CodexConfigurableConsumerRecipePanel(IConfigurableConsumerOption data)
	{
		this.data = data;
	}

	// Token: 0x06006550 RID: 25936 RVA: 0x002619A0 File Offset: 0x0025FBA0
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		this.title = component.GetReference<LocText>("Title");
		this.result_description = component.GetReference<LocText>("ResultDescription");
		this.resultIcon = component.GetReference<Image>("ResultIcon");
		this.ingredient_original = component.GetReference<RectTransform>("IngredientPrefab").gameObject;
		this.ingredient_original.SetActive(false);
		CodexText codexText = new CodexText();
		LocText reference = this.ingredient_original.GetComponent<HierarchyReferences>().GetReference<LocText>("Name");
		codexText.ConfigureLabel(reference, textStyles);
		this.Clear();
		if (this.data != null)
		{
			this.title.text = this.data.GetName();
			this.result_description.text = this.data.GetDescription();
			this.result_description.color = Color.black;
			this.resultIcon.sprite = this.data.GetIcon();
			IConfigurableConsumerIngredient[] ingredients = this.data.GetIngredients();
			this._ingredientRows = new GameObject[ingredients.Length];
			for (int i = 0; i < this._ingredientRows.Length; i++)
			{
				this._ingredientRows[i] = this.CreateIngredientRow(ingredients[i]);
			}
		}
	}

	// Token: 0x06006551 RID: 25937 RVA: 0x00261ACC File Offset: 0x0025FCCC
	public GameObject CreateIngredientRow(IConfigurableConsumerIngredient ingredient)
	{
		Tag[] idsets = ingredient.GetIDSets();
		if (this.ingredient_original != null && idsets.Length != 0)
		{
			GameObject gameObject = Util.KInstantiateUI(this.ingredient_original, this.ingredient_original.transform.parent.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(idsets[0], "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("Name").text = idsets[0].ProperName();
			component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(idsets[0], ingredient.GetAmount(), GameUtil.TimeSlice.None);
			component.GetReference<LocText>("Amount").color = Color.black;
			return gameObject;
		}
		return null;
	}

	// Token: 0x06006552 RID: 25938 RVA: 0x00261BB8 File Offset: 0x0025FDB8
	public void Clear()
	{
		if (this._ingredientRows != null)
		{
			for (int i = 0; i < this._ingredientRows.Length; i++)
			{
				UnityEngine.Object.Destroy(this._ingredientRows[i]);
			}
			this._ingredientRows = null;
		}
	}

	// Token: 0x0400449F RID: 17567
	private LocText title;

	// Token: 0x040044A0 RID: 17568
	private LocText result_description;

	// Token: 0x040044A1 RID: 17569
	private Image resultIcon;

	// Token: 0x040044A2 RID: 17570
	private GameObject ingredient_original;

	// Token: 0x040044A3 RID: 17571
	private IConfigurableConsumerOption data;

	// Token: 0x040044A4 RID: 17572
	private GameObject[] _ingredientRows;
}
