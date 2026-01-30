using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E74 RID: 3700
public class SelectedRecipeQueueScreen : KScreen
{
	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x060075AA RID: 30122 RVA: 0x002CEB0F File Offset: 0x002CCD0F
	private ComplexRecipe selectedRecipe
	{
		get
		{
			return this.CalculateSelectedRecipe();
		}
	}

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x060075AB RID: 30123 RVA: 0x002CEB17 File Offset: 0x002CCD17
	private List<ComplexRecipe> selectedRecipes
	{
		get
		{
			return this.target.GetRecipesWithCategoryID(this.selectedRecipeCategoryID);
		}
	}

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x060075AC RID: 30124 RVA: 0x002CEB2A File Offset: 0x002CCD2A
	private ComplexRecipe firstSelectedRecipe
	{
		get
		{
			return this.selectedRecipes[0];
		}
	}

	// Token: 0x060075AD RID: 30125 RVA: 0x002CEB38 File Offset: 0x002CCD38
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DecrementButton.onClick = delegate()
		{
			if (this.selectedRecipe == null)
			{
				return;
			}
			this.target.DecrementRecipeQueueCount(this.selectedRecipe, false);
			this.RefreshIngredientDescriptors();
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
		};
		this.IncrementButton.onClick = delegate()
		{
			if (this.selectedRecipe == null)
			{
				return;
			}
			this.target.IncrementRecipeQueueCount(this.selectedRecipe);
			this.RefreshIngredientDescriptors();
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
		};
		this.InfiniteButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_FOREVER;
		this.InfiniteButton.onClick += delegate()
		{
			if (this.selectedRecipe == null)
			{
				return;
			}
			if (this.target.GetRecipeQueueCount(this.selectedRecipe) != ComplexFabricator.QUEUE_INFINITE)
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, ComplexFabricator.QUEUE_INFINITE);
			}
			else
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, 0);
			}
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
		};
		this.QueueCount.onEndEdit += delegate()
		{
			base.isEditing = false;
			if (this.selectedRecipe == null)
			{
				return;
			}
			this.target.SetRecipeQueueCount(this.selectedRecipe, Mathf.RoundToInt(this.QueueCount.currentValue));
			this.RefreshIngredientDescriptors();
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
		};
		this.QueueCount.onStartEdit += delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
		};
		MultiToggle multiToggle = this.previousRecipeButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.CyclePreviousRecipe));
		MultiToggle multiToggle2 = this.nextRecipeButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.CycleNextRecipe));
	}

	// Token: 0x060075AE RID: 30126 RVA: 0x002CEC28 File Offset: 0x002CCE28
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.firstSelectedRecipe != null)
		{
			GameObject prefab = Assets.GetPrefab(this.firstSelectedRecipe.results[0].material);
			Equippable equippable = (prefab != null) ? prefab.GetComponent<Equippable>() : null;
			if (equippable != null && equippable.GetBuildOverride() != null)
			{
				this.minionWidget.RemoveEquipment(equippable);
			}
		}
	}

	// Token: 0x060075AF RID: 30127 RVA: 0x002CEC94 File Offset: 0x002CCE94
	private void AutoSelectBestRecipeInCategory()
	{
		int num = -1;
		List<ComplexRecipe> list = new List<ComplexRecipe>();
		this.selectedMaterialOption.Clear();
		ComplexRecipe complexRecipe = null;
		if (this.target.mostRecentRecipeSelectionByCategory.ContainsKey(this.selectedRecipeCategoryID))
		{
			complexRecipe = this.target.GetRecipe(this.target.mostRecentRecipeSelectionByCategory[this.selectedRecipeCategoryID]);
		}
		if (complexRecipe != null)
		{
			foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
			{
				this.selectedMaterialOption.Add(recipeElement.material);
			}
		}
		else
		{
			foreach (ComplexRecipe complexRecipe2 in this.selectedRecipes)
			{
				int num2 = this.target.GetRecipeQueueCount(complexRecipe2);
				if (num2 == ComplexFabricator.QUEUE_INFINITE)
				{
					num2 = int.MaxValue;
				}
				if (num2 >= num)
				{
					if (num2 > num)
					{
						list.Clear();
						num = num2;
					}
					list.Add(complexRecipe2);
				}
			}
			int num3 = list[0].ingredients.Length;
			Tag[] array = new Tag[num3];
			for (int j = 0; j < num3; j++)
			{
				float num4 = -1f;
				foreach (ComplexRecipe complexRecipe3 in list)
				{
					float amount = this.target.GetMyWorld().worldInventory.GetAmount(complexRecipe3.ingredients[j].material, true);
					if (amount > num4)
					{
						array[j] = complexRecipe3.ingredients[j].material;
						num4 = amount;
					}
				}
			}
			this.selectedMaterialOption.AddRange(array);
		}
		this.RefreshIngredientDescriptors();
		this.RefreshQueueCountDisplay();
	}

	// Token: 0x060075B0 RID: 30128 RVA: 0x002CEE78 File Offset: 0x002CD078
	public bool IsSelectedMaterials(ComplexRecipe recipe)
	{
		if (this.selectedRecipeCategoryID != recipe.recipeCategoryID)
		{
			return false;
		}
		for (int i = 0; i < recipe.ingredients.Length; i++)
		{
			if (recipe.ingredients[i].material != this.selectedMaterialOption[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060075B1 RID: 30129 RVA: 0x002CEED0 File Offset: 0x002CD0D0
	public void SelectNextQueuedRecipeInCategory()
	{
		this.cycleRecipeVariantIdx++;
		this.selectedMaterialOption.Clear();
		List<ComplexRecipe> list = (from match in this.selectedRecipes
		where this.target.IsRecipeQueued(match)
		select match).ToList<ComplexRecipe>();
		if (list.Count == 0)
		{
			this.AutoSelectBestRecipeInCategory();
			return;
		}
		ComplexRecipe complexRecipe = list[this.cycleRecipeVariantIdx % list.Count];
		for (int i = 0; i < complexRecipe.ingredients.Length; i++)
		{
			this.selectedMaterialOption.Add(complexRecipe.ingredients[i].material);
		}
		this.RefreshIngredientDescriptors();
		this.RefreshQueueCountDisplay();
	}

	// Token: 0x060075B2 RID: 30130 RVA: 0x002CEF70 File Offset: 0x002CD170
	public void SetRecipeCategory(ComplexFabricatorSideScreen owner, ComplexFabricator target, string recipeCategoryID)
	{
		this.ownerScreen = owner;
		this.target = target;
		this.selectedRecipeCategoryID = recipeCategoryID;
		this.AutoSelectBestRecipeInCategory();
		this.recipeName.text = this.firstSelectedRecipe.GetUIName(false);
		global::Tuple<Sprite, Color> uisprite;
		if (this.firstSelectedRecipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient)
		{
			uisprite = Def.GetUISprite(this.firstSelectedRecipe.ingredients[0].material, "ui", false);
		}
		else if (this.firstSelectedRecipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Custom && !string.IsNullOrEmpty(this.firstSelectedRecipe.customSpritePrefabID))
		{
			uisprite = Def.GetUISprite(this.firstSelectedRecipe.customSpritePrefabID, "ui", false);
		}
		else
		{
			uisprite = Def.GetUISprite(this.firstSelectedRecipe.results[0].material, this.firstSelectedRecipe.results[0].facadeID);
		}
		if (this.firstSelectedRecipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
		{
			this.recipeIcon.sprite = owner.radboltSprite;
			this.recipeIcon.sprite = owner.radboltSprite;
		}
		else
		{
			this.recipeIcon.sprite = uisprite.first;
			this.recipeIcon.color = uisprite.second;
		}
		string text = (this.firstSelectedRecipe.time.ToString() + " " + UI.UNITSUFFIXES.SECONDS).ToLower();
		this.recipeMainDescription.SetText(this.firstSelectedRecipe.description);
		this.recipeDuration.SetText(text);
		string simpleTooltip = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPE_WORKTIME, text);
		this.recipeDurationTooltip.SetSimpleTooltip(simpleTooltip);
		this.cycleRecipeVariantIdx = 0;
		this.RefreshIngredientDescriptors();
		this.RefreshResultDescriptors();
		this.RefreshSizeScrollContainerSize();
		this.RefreshQueueCountDisplay();
		this.ToggleAndRefreshMinionDisplay();
	}

	// Token: 0x060075B3 RID: 30131 RVA: 0x002CF128 File Offset: 0x002CD328
	private void RefreshSizeScrollContainerSize()
	{
		float num = 16f;
		float num2 = 0f;
		float num3 = (float)((this.selectedRecipe.consumedHEP > 0) ? 94 : 0);
		num2 += (float)(this.materialSelectionRowsByContainer.Count * 32);
		foreach (KeyValuePair<GameObject, List<GameObject>> keyValuePair in this.materialSelectionRowsByContainer)
		{
			num2 += (float)(Mathf.Max(1, keyValuePair.Value.Count) * 48);
		}
		num2 += (float)((this.materialSelectionRowsByContainer.Count - 1) * 12);
		float num4 = (float)Mathf.Max(this.selectedRecipes[0].results.Length * 32 + (this.recipeEffectsDescriptorRows.Count - this.selectedRecipes[0].results.Length) * 16, 40);
		num4 += 46f;
		float b = num + num2 + num3 + num4;
		this.scrollContainer.minHeight = Mathf.Min((float)(Screen.height - 448), b);
	}

	// Token: 0x060075B4 RID: 30132 RVA: 0x002CF248 File Offset: 0x002CD448
	private void CyclePreviousRecipe()
	{
		this.ownerScreen.CycleRecipe(-1);
	}

	// Token: 0x060075B5 RID: 30133 RVA: 0x002CF256 File Offset: 0x002CD456
	private void CycleNextRecipe()
	{
		this.ownerScreen.CycleRecipe(1);
	}

	// Token: 0x060075B6 RID: 30134 RVA: 0x002CF264 File Offset: 0x002CD464
	private void ToggleAndRefreshMinionDisplay()
	{
		this.minionWidget.gameObject.SetActive(this.RefreshMinionDisplayAnim());
	}

	// Token: 0x060075B7 RID: 30135 RVA: 0x002CF27C File Offset: 0x002CD47C
	private bool RefreshMinionDisplayAnim()
	{
		GameObject prefab = Assets.GetPrefab(this.firstSelectedRecipe.results[0].material);
		if (prefab == null)
		{
			return false;
		}
		Equippable component = prefab.GetComponent<Equippable>();
		if (component == null)
		{
			return false;
		}
		KAnimFile buildOverride = component.GetBuildOverride();
		if (buildOverride == null)
		{
			return false;
		}
		this.minionWidget.SetDefaultPortraitAnimator();
		KAnimFile animFile = buildOverride;
		if (!this.firstSelectedRecipe.results[0].facadeID.IsNullOrWhiteSpace())
		{
			EquippableFacadeResource equippableFacadeResource = Db.GetEquippableFacades().TryGet(this.firstSelectedRecipe.results[0].facadeID);
			if (equippableFacadeResource != null)
			{
				animFile = Assets.GetAnim(equippableFacadeResource.BuildOverride);
			}
		}
		this.minionWidget.UpdateEquipment(component, animFile);
		return true;
	}

	// Token: 0x060075B8 RID: 30136 RVA: 0x002CF338 File Offset: 0x002CD538
	private ComplexRecipe CalculateSelectedRecipe()
	{
		foreach (ComplexRecipe complexRecipe in this.target.GetRecipesWithCategoryID(this.selectedRecipeCategoryID))
		{
			bool flag = true;
			for (int i = 0; i < this.selectedMaterialOption.Count; i++)
			{
				if (complexRecipe.ingredients[i].material != this.selectedMaterialOption[i])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return complexRecipe;
			}
		}
		return null;
	}

	// Token: 0x060075B9 RID: 30137 RVA: 0x002CF3D8 File Offset: 0x002CD5D8
	private void RefreshQueueCountDisplay()
	{
		this.ResearchRequiredContainer.SetActive(!this.selectedRecipes[0].IsRequiredTechUnlocked());
		if (this.selectedRecipe == null)
		{
			return;
		}
		bool flag = true;
		foreach (Tag tag in this.selectedMaterialOption)
		{
			if (!DiscoveredResources.Instance.IsDiscovered(tag))
			{
				flag = DebugHandler.InstantBuildMode;
			}
		}
		this.UndiscoveredMaterialsContainer.SetActive(!flag);
		int recipeQueueCount = this.target.GetRecipeQueueCount(this.selectedRecipe);
		bool flag2 = recipeQueueCount == ComplexFabricator.QUEUE_INFINITE;
		if (!flag2)
		{
			this.QueueCount.SetAmount((float)recipeQueueCount);
		}
		else
		{
			this.QueueCount.SetDisplayValue("");
		}
		this.InfiniteIcon.gameObject.SetActive(flag2);
	}

	// Token: 0x060075BA RID: 30138 RVA: 0x002CF4C0 File Offset: 0x002CD6C0
	private void RefreshResultDescriptors()
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		list.AddRange(this.GetResultDescriptions(this.selectedRecipes[0]));
		foreach (Descriptor desc in this.target.AdditionalEffectsForRecipe(this.selectedRecipes[0]))
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
		}
		if (list.Count > 0)
		{
			this.EffectsDescriptorPanel.gameObject.SetActive(true);
			foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> keyValuePair in this.recipeEffectsDescriptorRows)
			{
				Util.KDestroyGameObject(keyValuePair.Value);
			}
			this.recipeEffectsDescriptorRows.Clear();
			bool flag = true;
			foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list)
			{
				GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.EffectsDescriptorPanel.gameObject, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				Image reference = component.GetReference<Image>("Icon");
				bool flag2 = descriptorWithSprite.tintedSprite != null && descriptorWithSprite.tintedSprite.first != null;
				reference.sprite = ((descriptorWithSprite.tintedSprite == null) ? null : descriptorWithSprite.tintedSprite.first);
				reference.gameObject.SetActive(true);
				if (!flag2)
				{
					reference.color = Color.clear;
					if (flag)
					{
						gameObject.GetComponent<VerticalLayoutGroup>().padding.top = -8;
						flag = false;
					}
				}
				else
				{
					reference.color = ((descriptorWithSprite.tintedSprite == null) ? Color.white : descriptorWithSprite.tintedSprite.second);
					flag = true;
				}
				reference.gameObject.GetComponent<LayoutElement>().minWidth = (float)(flag2 ? 32 : 40);
				reference.gameObject.GetComponent<LayoutElement>().minHeight = (float)(flag2 ? 32 : 0);
				reference.gameObject.GetComponent<LayoutElement>().preferredHeight = (float)(flag2 ? 32 : 0);
				component.GetReference<LocText>("Label").SetText(flag2 ? descriptorWithSprite.descriptor.IndentedText() : descriptorWithSprite.descriptor.text);
				component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
				component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(descriptorWithSprite.descriptor.tooltipText);
				this.recipeEffectsDescriptorRows.Add(descriptorWithSprite, gameObject);
			}
		}
	}

	// Token: 0x060075BB RID: 30139 RVA: 0x002CF7AC File Offset: 0x002CD9AC
	private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetResultDescriptions(ComplexRecipe recipe)
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		if (recipe.producedHEP > 0)
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format("<b>{0}</b>: {1}", UI.FormatAsLink(ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP"), recipe.producedHEP), string.Format("<b>{0}</b>: {1}", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, recipe.producedHEP), Descriptor.DescriptorType.Requirement, false), new global::Tuple<Sprite, Color>(Assets.GetSprite("radbolt"), Color.white), false));
		}
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.results)
		{
			GameObject prefab = Assets.GetPrefab(recipeElement.material);
			string formattedByTag = GameUtil.GetFormattedByTag(recipeElement.material, recipeElement.amount, GameUtil.TimeSlice.None);
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), Descriptor.DescriptorType.Requirement, false), Def.GetUISprite(recipeElement.material, recipeElement.facadeID), false));
			Element element = ElementLoader.GetElement(recipeElement.material);
			if (element != null)
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list2 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc in GameUtil.GetMaterialDescriptors(element))
				{
					list2.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list2)
				{
					descriptorWithSprite.descriptor.IncreaseIndent();
				}
				list.AddRange(list2);
			}
			else
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list3 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc2 in GameUtil.GetEffectDescriptors(GameUtil.GetAllDescriptors(prefab, false)))
				{
					list3.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc2, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite2 in list3)
				{
					descriptorWithSprite2.descriptor.IncreaseIndent();
				}
				list.AddRange(list3);
			}
		}
		return list;
	}

	// Token: 0x060075BC RID: 30140 RVA: 0x002CFA7C File Offset: 0x002CDC7C
	private void RefreshIngredientDescriptors()
	{
		new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		this.IngredientsDescriptorPanel.gameObject.SetActive(true);
		this.radboltSpacer.gameObject.SetActive(this.selectedRecipe.consumedHEP > 0);
		this.radboltHeader.gameObject.SetActive(this.selectedRecipe.consumedHEP > 0);
		this.RadboltDescriptorPanel.gameObject.SetActive(this.selectedRecipe.consumedHEP > 0);
		this.radboltLabel.SetText(GameUtil.SafeStringFormat(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_RADBOLTS_REQUIRED, new object[]
		{
			this.selectedRecipe.consumedHEP.ToString()
		}));
		this.materialSelectionContainers.ForEach(delegate(GameObject container)
		{
			Util.KDestroyGameObject(container);
		});
		this.materialSelectionContainers.Clear();
		this.materialSelectionRowsByContainer.Clear();
		for (int i = 0; i < this.selectedRecipes[0].ingredients.Length; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.materialSelectionContainerPrefab, this.IngredientsDescriptorPanel.gameObject, true);
			this.materialSelectionContainers.Add(gameObject);
			this.materialSelectionRowsByContainer.Add(this.materialSelectionContainers[i], new List<GameObject>());
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			int idx = i;
			List<Tag> list = new List<Tag>();
			bool flag = false;
			HashSet<Tag> hashSet = new HashSet<Tag>();
			for (int j = 0; j < this.selectedRecipes.Count; j++)
			{
				Tag newTag = this.selectedRecipes[j].ingredients[idx].material;
				if (!list.Contains(newTag))
				{
					bool flag2 = DiscoveredResources.Instance.IsDiscovered(newTag);
					if (!flag2)
					{
						hashSet.Add(newTag);
					}
					if (flag2 || DebugHandler.InstantBuildMode)
					{
						flag = true;
						GameObject gameObject2 = Util.KInstantiateUI(this.materialFilterRowPrefab, this.materialSelectionContainers[idx].gameObject, true);
						this.materialSelectionRowsByContainer[this.materialSelectionContainers[idx]].Add(gameObject2);
						list.Add(newTag);
						LocText reference = gameObject2.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
						bool flag3 = false;
						string ingredientDescription = this.GetIngredientDescription(this.selectedRecipes[j].ingredients[idx], out flag3);
						bool flag4 = this.selectedMaterialOption[i] == this.selectedRecipes[j].ingredients[i].material;
						if (flag4)
						{
							component.GetReference<Image>("HeaderBG").color = (flag3 ? Util.ColorFromHex("D9DAE3") : Util.ColorFromHex("E3DAD9"));
						}
						reference.color = (flag3 ? Color.black : new Color(0.2f, 0.2f, 0.2f, 1f));
						HierarchyReferences component2 = gameObject2.GetComponent<HierarchyReferences>();
						component2.GetReference<RectTransform>("SelectionHover").gameObject.SetActive(flag4);
						component2.GetReference<RectTransform>("SelectionHover").GetComponent<Image>().color = (flag3 ? Util.ColorFromHex("F0F6FC") : Util.ColorFromHex("FBE9EB"));
						component2.GetReference<LocText>("OrderCountLabel").SetText(this.target.GetIngredientQueueCount(this.selectedRecipeCategoryID, newTag).ToString());
						Image reference2 = component2.GetReference<Image>("Icon");
						reference2.material = ((!flag3) ? GlobalResources.Instance().AnimMaterialUIDesaturated : GlobalResources.Instance().AnimUIMaterial);
						reference2.color = (flag3 ? Color.white : new Color(1f, 1f, 1f, 0.55f));
						reference.SetText(ingredientDescription);
						reference2.sprite = Def.GetUISprite(newTag, "").first;
						MultiToggle component3 = gameObject2.GetComponent<MultiToggle>();
						component3.ChangeState(flag4 ? 1 : 0);
						component3.onClick = (System.Action)Delegate.Combine(component3.onClick, new System.Action(delegate()
						{
							Tag newTag = newTag;
							this.selectedMaterialOption[idx] = newTag;
							this.RefreshIngredientDescriptors();
							this.RefreshQueueCountDisplay();
							this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
						}));
					}
				}
			}
			ToolTip reference3 = component.GetReference<ToolTip>("HeaderTooltip");
			string source = UI.UISIDESCREENS.FABRICATORSIDESCREEN.UNDISCOVERED_INGREDIENTS_IN_CATEGORY;
			object[] array = new object[1];
			array[0] = "    • " + string.Join("\n    • ", (from t in hashSet
			select t.ProperName()).ToArray<string>());
			string text = GameUtil.SafeStringFormat(source, array);
			reference3.SetSimpleTooltip((hashSet.Count == 0) ? UI.UISIDESCREENS.FABRICATORSIDESCREEN.ALL_INGREDIENTS_IN_CATEGORY_DISOVERED : text);
			RectTransform reference4 = component.GetReference<RectTransform>("NoDiscoveredRow");
			reference4.gameObject.SetActive(!flag);
			if (!flag)
			{
				reference4.GetComponent<ToolTip>().SetSimpleTooltip(text);
			}
			string text2 = GameUtil.SafeStringFormat(UI.UISIDESCREENS.FABRICATORSIDESCREEN.INGREDIENT_CATEGORY, new object[]
			{
				i + 1
			});
			if (!flag)
			{
				component.GetReference<Image>("HeaderBG").color = Util.ColorFromHex("E3DAD9");
			}
			if (hashSet.Count > 0)
			{
				text2 = string.Concat(new string[]
				{
					text2,
					" <color=#bf5858>(",
					list.Count.ToString(),
					"/",
					(list.Count + hashSet.Count).ToString(),
					")",
					UIConstants.ColorSuffix
				});
			}
			component.GetReference<LocText>("HeaderLabel").SetText(text2);
		}
		if (!this.target.mostRecentRecipeSelectionByCategory.ContainsKey(this.selectedRecipeCategoryID))
		{
			this.target.mostRecentRecipeSelectionByCategory.Add(this.selectedRecipeCategoryID, null);
		}
		this.target.mostRecentRecipeSelectionByCategory[this.selectedRecipeCategoryID] = this.selectedRecipe.id;
	}

	// Token: 0x060075BD RID: 30141 RVA: 0x002D00B0 File Offset: 0x002CE2B0
	private string GetIngredientDescription(ComplexRecipe.RecipeElement ingredient, out bool hasEnoughMaterial)
	{
		GameObject prefab = Assets.GetPrefab(ingredient.material);
		string formattedByTag = GameUtil.GetFormattedByTag(ingredient.material, ingredient.amount, GameUtil.TimeSlice.None);
		float amount = this.target.GetMyWorld().worldInventory.GetAmount(ingredient.material, true);
		string formattedByTag2 = GameUtil.GetFormattedByTag(ingredient.material, amount, GameUtil.TimeSlice.None);
		hasEnoughMaterial = (amount >= ingredient.amount);
		string text = GameUtil.SafeStringFormat(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_REQUIREMENT, new object[]
		{
			prefab.GetProperName(),
			formattedByTag
		});
		text += "\n";
		if (hasEnoughMaterial)
		{
			text = text + "<size=12>" + GameUtil.SafeStringFormat(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_AVAILABLE, new object[]
			{
				formattedByTag2
			}) + "</size>";
		}
		else
		{
			text = text + "<size=12><color=#E68280>" + GameUtil.SafeStringFormat(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_AVAILABLE, new object[]
			{
				formattedByTag2
			}) + "</color></size>";
		}
		return text;
	}

	// Token: 0x0400516D RID: 20845
	public Image recipeIcon;

	// Token: 0x0400516E RID: 20846
	public LocText recipeName;

	// Token: 0x0400516F RID: 20847
	public LocText recipeMainDescription;

	// Token: 0x04005170 RID: 20848
	public LocText recipeDuration;

	// Token: 0x04005171 RID: 20849
	public ToolTip recipeDurationTooltip;

	// Token: 0x04005172 RID: 20850
	public GameObject IngredientsDescriptorPanel;

	// Token: 0x04005173 RID: 20851
	public GameObject radboltSpacer;

	// Token: 0x04005174 RID: 20852
	public GameObject radboltHeader;

	// Token: 0x04005175 RID: 20853
	public GameObject RadboltDescriptorPanel;

	// Token: 0x04005176 RID: 20854
	public LocText radboltLabel;

	// Token: 0x04005177 RID: 20855
	public GameObject EffectsDescriptorPanel;

	// Token: 0x04005178 RID: 20856
	public KNumberInputField QueueCount;

	// Token: 0x04005179 RID: 20857
	public MultiToggle DecrementButton;

	// Token: 0x0400517A RID: 20858
	public MultiToggle IncrementButton;

	// Token: 0x0400517B RID: 20859
	public KButton InfiniteButton;

	// Token: 0x0400517C RID: 20860
	public GameObject InfiniteIcon;

	// Token: 0x0400517D RID: 20861
	public GameObject ResearchRequiredContainer;

	// Token: 0x0400517E RID: 20862
	public GameObject UndiscoveredMaterialsContainer;

	// Token: 0x0400517F RID: 20863
	[SerializeField]
	private GameObject materialFilterRowPrefab;

	// Token: 0x04005180 RID: 20864
	[SerializeField]
	private GameObject materialSelectionContainerPrefab;

	// Token: 0x04005181 RID: 20865
	private List<GameObject> materialSelectionContainers = new List<GameObject>();

	// Token: 0x04005182 RID: 20866
	private Dictionary<GameObject, List<GameObject>> materialSelectionRowsByContainer = new Dictionary<GameObject, List<GameObject>>();

	// Token: 0x04005183 RID: 20867
	private ComplexFabricator target;

	// Token: 0x04005184 RID: 20868
	private ComplexFabricatorSideScreen ownerScreen;

	// Token: 0x04005185 RID: 20869
	private List<Tag> selectedMaterialOption = new List<Tag>();

	// Token: 0x04005186 RID: 20870
	private string selectedRecipeCategoryID;

	// Token: 0x04005187 RID: 20871
	[SerializeField]
	private GameObject recipeElementDescriptorPrefab;

	// Token: 0x04005188 RID: 20872
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeIngredientDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x04005189 RID: 20873
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeEffectsDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x0400518A RID: 20874
	[SerializeField]
	private FullBodyUIMinionWidget minionWidget;

	// Token: 0x0400518B RID: 20875
	[SerializeField]
	private MultiToggle previousRecipeButton;

	// Token: 0x0400518C RID: 20876
	[SerializeField]
	private MultiToggle nextRecipeButton;

	// Token: 0x0400518D RID: 20877
	[SerializeField]
	private LayoutElement scrollContainer;

	// Token: 0x0400518E RID: 20878
	private int cycleRecipeVariantIdx;

	// Token: 0x020020E4 RID: 8420
	private class DescriptorWithSprite
	{
		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x0600BAA0 RID: 47776 RVA: 0x003FB8AF File Offset: 0x003F9AAF
		public Descriptor descriptor { get; }

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x0600BAA1 RID: 47777 RVA: 0x003FB8B7 File Offset: 0x003F9AB7
		public global::Tuple<Sprite, Color> tintedSprite { get; }

		// Token: 0x0600BAA2 RID: 47778 RVA: 0x003FB8BF File Offset: 0x003F9ABF
		public DescriptorWithSprite(Descriptor desc, global::Tuple<Sprite, Color> sprite, bool filterRowVisible = false)
		{
			this.descriptor = desc;
			this.tintedSprite = sprite;
			this.showFilterRow = filterRowVisible;
		}

		// Token: 0x04009773 RID: 38771
		public bool showFilterRow;
	}
}
