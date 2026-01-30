using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DF5 RID: 3573
public class ResourceRemainingDisplayScreen : KScreen
{
	// Token: 0x060070D0 RID: 28880 RVA: 0x002AF84E File Offset: 0x002ADA4E
	public static void DestroyInstance()
	{
		ResourceRemainingDisplayScreen.instance = null;
	}

	// Token: 0x060070D1 RID: 28881 RVA: 0x002AF856 File Offset: 0x002ADA56
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Activate();
		ResourceRemainingDisplayScreen.instance = this;
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x060070D2 RID: 28882 RVA: 0x002AF876 File Offset: 0x002ADA76
	public void ActivateDisplay(GameObject target)
	{
		this.numberOfPendingConstructions = 0;
		this.dispayPrefab.SetActive(true);
	}

	// Token: 0x060070D3 RID: 28883 RVA: 0x002AF88B File Offset: 0x002ADA8B
	public void DeactivateDisplay()
	{
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x060070D4 RID: 28884 RVA: 0x002AF89C File Offset: 0x002ADA9C
	public void SetResources(IList<Tag> _selected_elements, Recipe recipe)
	{
		this.selected_elements.Clear();
		foreach (Tag item in _selected_elements)
		{
			this.selected_elements.Add(item);
		}
		this.currentRecipe = recipe;
		global::Debug.Assert(this.selected_elements.Count == recipe.Ingredients.Count, string.Format("{0} Mismatch number of selected elements {1} and recipe requirements {2}", recipe.Name, this.selected_elements.Count, recipe.Ingredients.Count));
	}

	// Token: 0x060070D5 RID: 28885 RVA: 0x002AF948 File Offset: 0x002ADB48
	public void SetNumberOfPendingConstructions(int number)
	{
		this.numberOfPendingConstructions = number;
	}

	// Token: 0x060070D6 RID: 28886 RVA: 0x002AF954 File Offset: 0x002ADB54
	public void Update()
	{
		if (!this.dispayPrefab.activeSelf)
		{
			return;
		}
		if (base.canvas != null)
		{
			if (this.rect == null)
			{
				this.rect = base.GetComponent<RectTransform>();
			}
			this.rect.anchoredPosition = base.WorldToScreen(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
		}
		if (this.displayedConstructionCostMultiplier == this.numberOfPendingConstructions)
		{
			this.label.text = "";
			return;
		}
		this.displayedConstructionCostMultiplier = this.numberOfPendingConstructions;
	}

	// Token: 0x060070D7 RID: 28887 RVA: 0x002AF9E4 File Offset: 0x002ADBE4
	public string GetString()
	{
		string text = "";
		if (this.selected_elements != null && this.currentRecipe != null)
		{
			for (int i = 0; i < this.currentRecipe.Ingredients.Count; i++)
			{
				Tag tag = this.selected_elements[i];
				float num = this.currentRecipe.Ingredients[i].amount * (float)this.numberOfPendingConstructions;
				float num2 = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, true);
				num2 -= num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				string text2 = tag.ProperName();
				if (MaterialSelector.DeprioritizeAutoSelectElementList.Contains(tag) && MaterialSelector.GetValidMaterials(this.currentRecipe.Ingredients[i].tag, false).Count > 1)
				{
					text2 = string.Concat(new string[]
					{
						"<b>",
						UIConstants.ColorPrefixYellow,
						text2,
						UIConstants.ColorSuffix,
						"</b>"
					});
				}
				text = string.Concat(new string[]
				{
					text,
					text2,
					": ",
					GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
					" / ",
					GameUtil.GetFormattedMass(this.currentRecipe.Ingredients[i].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
				});
				if (i < this.selected_elements.Count - 1)
				{
					text += "\n";
				}
			}
		}
		return text;
	}

	// Token: 0x04004DCB RID: 19915
	public static ResourceRemainingDisplayScreen instance;

	// Token: 0x04004DCC RID: 19916
	public GameObject dispayPrefab;

	// Token: 0x04004DCD RID: 19917
	public LocText label;

	// Token: 0x04004DCE RID: 19918
	private Recipe currentRecipe;

	// Token: 0x04004DCF RID: 19919
	private List<Tag> selected_elements = new List<Tag>();

	// Token: 0x04004DD0 RID: 19920
	private int numberOfPendingConstructions;

	// Token: 0x04004DD1 RID: 19921
	private int displayedConstructionCostMultiplier;

	// Token: 0x04004DD2 RID: 19922
	private RectTransform rect;
}
