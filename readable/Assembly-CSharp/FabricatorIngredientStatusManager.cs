using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000755 RID: 1877
[AddComponentMenu("KMonoBehaviour/scripts/FabricatorIngredientStatusManager")]
public class FabricatorIngredientStatusManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06002F79 RID: 12153 RVA: 0x00112314 File Offset: 0x00110514
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.selectable = base.GetComponent<KSelectable>();
		this.fabricator = base.GetComponent<ComplexFabricator>();
		this.InitializeBalances();
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x0011233C File Offset: 0x0011053C
	private void InitializeBalances()
	{
		foreach (ComplexRecipe complexRecipe in this.fabricator.GetRecipes())
		{
			this.recipeRequiredResourceBalances.Add(complexRecipe, new Dictionary<Tag, float>());
			foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
			{
				this.recipeRequiredResourceBalances[complexRecipe].Add(recipeElement.material, 0f);
			}
		}
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x001123B4 File Offset: 0x001105B4
	public void Sim1000ms(float dt)
	{
		this.RefreshStatusItems();
	}

	// Token: 0x06002F7C RID: 12156 RVA: 0x001123BC File Offset: 0x001105BC
	private void RefreshStatusItems()
	{
		foreach (KeyValuePair<ComplexRecipe, Guid> keyValuePair in this.statusItems)
		{
			if (!this.fabricator.IsRecipeQueued(keyValuePair.Key))
			{
				this.deadOrderKeys.Add(keyValuePair.Key);
			}
		}
		foreach (ComplexRecipe complexRecipe in this.deadOrderKeys)
		{
			this.recipeRequiredResourceBalances[complexRecipe].Clear();
			foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
			{
				this.recipeRequiredResourceBalances[complexRecipe].Add(recipeElement.material, 0f);
			}
			this.selectable.RemoveStatusItem(this.statusItems[complexRecipe], false);
			this.statusItems.Remove(complexRecipe);
		}
		this.deadOrderKeys.Clear();
		foreach (ComplexRecipe complexRecipe2 in this.fabricator.GetRecipes())
		{
			if (this.fabricator.IsRecipeQueued(complexRecipe2))
			{
				bool flag = false;
				foreach (ComplexRecipe.RecipeElement recipeElement2 in complexRecipe2.ingredients)
				{
					float newBalance = this.fabricator.inStorage.GetAmountAvailable(recipeElement2.material) + this.fabricator.buildStorage.GetAmountAvailable(recipeElement2.material) + this.fabricator.GetMyWorld().worldInventory.GetTotalAmount(recipeElement2.material, true) - recipeElement2.amount;
					flag = (flag || this.ChangeRecipeRequiredResourceBalance(complexRecipe2, recipeElement2.material, newBalance) || (this.statusItems.ContainsKey(complexRecipe2) && this.fabricator.GetRecipeQueueCount(complexRecipe2) == 0));
				}
				if (flag)
				{
					if (this.statusItems.ContainsKey(complexRecipe2))
					{
						this.selectable.RemoveStatusItem(this.statusItems[complexRecipe2], false);
						this.statusItems.Remove(complexRecipe2);
					}
					if (this.fabricator.IsRecipeQueued(complexRecipe2))
					{
						using (Dictionary<Tag, float>.ValueCollection.Enumerator enumerator3 = this.recipeRequiredResourceBalances[complexRecipe2].Values.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								if (enumerator3.Current < 0f)
								{
									Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
									foreach (KeyValuePair<Tag, float> keyValuePair2 in this.recipeRequiredResourceBalances[complexRecipe2])
									{
										if (keyValuePair2.Value < 0f)
										{
											dictionary.Add(keyValuePair2.Key, -keyValuePair2.Value);
										}
									}
									Guid value = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, dictionary);
									this.statusItems.Add(complexRecipe2, value);
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06002F7D RID: 12157 RVA: 0x0011273C File Offset: 0x0011093C
	private bool ChangeRecipeRequiredResourceBalance(ComplexRecipe recipe, Tag tag, float newBalance)
	{
		bool result = false;
		if (this.recipeRequiredResourceBalances[recipe][tag] >= 0f != newBalance >= 0f)
		{
			result = true;
		}
		this.recipeRequiredResourceBalances[recipe][tag] = newBalance;
		return result;
	}

	// Token: 0x04001C38 RID: 7224
	private KSelectable selectable;

	// Token: 0x04001C39 RID: 7225
	private ComplexFabricator fabricator;

	// Token: 0x04001C3A RID: 7226
	private Dictionary<ComplexRecipe, Guid> statusItems = new Dictionary<ComplexRecipe, Guid>();

	// Token: 0x04001C3B RID: 7227
	private Dictionary<ComplexRecipe, Dictionary<Tag, float>> recipeRequiredResourceBalances = new Dictionary<ComplexRecipe, Dictionary<Tag, float>>();

	// Token: 0x04001C3C RID: 7228
	private List<ComplexRecipe> deadOrderKeys = new List<ComplexRecipe>();
}
