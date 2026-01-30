using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E2B RID: 3627
public class ComplexFabricatorSideScreen : SideScreenContent
{
	// Token: 0x0600731E RID: 29470 RVA: 0x002BF3C4 File Offset: 0x002BD5C4
	public override string GetTitle()
	{
		if (this.targetFab == null)
		{
			return Strings.Get(this.titleKey).ToString().Replace("{0}", "");
		}
		return string.Format(Strings.Get(this.titleKey), this.targetFab.GetProperName());
	}

	// Token: 0x0600731F RID: 29471 RVA: 0x002BF420 File Offset: 0x002BD620
	public override bool IsValidForTarget(GameObject target)
	{
		ComplexFabricator component = target.GetComponent<ComplexFabricator>();
		return component != null && component.enabled;
	}

	// Token: 0x06007320 RID: 29472 RVA: 0x002BF448 File Offset: 0x002BD648
	public override void SetTarget(GameObject target)
	{
		ComplexFabricator component = target.GetComponent<ComplexFabricator>();
		if (component == null)
		{
			global::Debug.LogError("The object selected doesn't have a ComplexFabricator!");
			return;
		}
		this.UnsubscribeTarget();
		this.Initialize(component);
		this.targetOrdersUpdatedSubHandle = this.targetFab.Subscribe(1721324763, new Action<object>(this.UpdateQueueCountLabels));
		this.UpdateQueueCountLabels(null);
	}

	// Token: 0x06007321 RID: 29473 RVA: 0x002BF4A8 File Offset: 0x002BD6A8
	private void UpdateQueueCountLabels(object data = null)
	{
		ComplexRecipe[] recipes = this.targetFab.GetRecipes();
		for (int i = 0; i < recipes.Length; i++)
		{
			ComplexRecipe r = recipes[i];
			GameObject gameObject = this.recipeToggles.Find((GameObject match) => this.recipeCategoryToggleMap[match].Contains(r));
			if (gameObject != null)
			{
				this.RefreshQueueCountDisplay(gameObject, this.targetFab);
				this.RefreshQueueTooltip(gameObject);
			}
		}
		if (this.targetFab.CurrentWorkingOrder != null)
		{
			this.currentOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, this.targetFab.CurrentWorkingOrder.GetUIName(false));
		}
		else
		{
			this.currentOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
		}
		if (this.targetFab.NextOrder != null)
		{
			this.nextOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, this.targetFab.NextOrder.GetUIName(false));
			return;
		}
		this.nextOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
	}

	// Token: 0x06007322 RID: 29474 RVA: 0x002BF5CC File Offset: 0x002BD7CC
	protected override void OnShow(bool show)
	{
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot);
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			DetailsScreen.Instance.ClearSecondarySideScreen();
			this.selectedRecipeCategory = "";
			this.selectedToggle = null;
		}
		base.OnShow(show);
	}

	// Token: 0x06007323 RID: 29475 RVA: 0x002BF62C File Offset: 0x002BD82C
	public void Initialize(ComplexFabricator target)
	{
		if (target == null)
		{
			global::Debug.LogError("ComplexFabricator provided was null.");
			return;
		}
		this.targetFab = target;
		base.gameObject.SetActive(true);
		this.recipeCategoryToggleMap = new Dictionary<GameObject, List<ComplexRecipe>>();
		this.recipeToggles.ForEach(delegate(GameObject rbi)
		{
			UnityEngine.Object.Destroy(rbi.gameObject);
		});
		this.recipeToggles.Clear();
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.recipeCategories)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.transform.parent.gameObject);
		}
		this.recipeCategories.Clear();
		int num = 0;
		ComplexRecipe[] recipes = this.targetFab.GetRecipes();
		Dictionary<string, List<ComplexRecipe>> dictionary = new Dictionary<string, List<ComplexRecipe>>();
		foreach (ComplexRecipe complexRecipe in recipes)
		{
			if (!dictionary.ContainsKey(complexRecipe.recipeCategoryID))
			{
				dictionary.Add(complexRecipe.recipeCategoryID, new List<ComplexRecipe>());
			}
			dictionary[complexRecipe.recipeCategoryID].Add(complexRecipe);
		}
		HashSet<string> hashSet = new HashSet<string>();
		Predicate<ComplexRecipe> <>9__1;
		Predicate<ComplexRecipe> <>9__2;
		Predicate<ComplexRecipe> <>9__3;
		Predicate<ComplexRecipe> <>9__4;
		Predicate<ComplexRecipe> <>9__5;
		Predicate<ComplexRecipe> <>9__7;
		foreach (KeyValuePair<string, List<ComplexRecipe>> keyValuePair2 in dictionary)
		{
			ComplexRecipe complexRecipe2 = keyValuePair2.Value[0];
			bool flag = false;
			if (DebugHandler.InstantBuildMode)
			{
				flag = true;
			}
			else if (keyValuePair2.Value[0].RequiresTechUnlock())
			{
				if (keyValuePair2.Value[0].IsRequiredTechUnlocked() || Db.Get().Techs.Get(keyValuePair2.Value[0].requiredTech).ArePrerequisitesComplete())
				{
					if (keyValuePair2.Value[0].RequiresAllIngredientsDiscovered)
					{
						List<ComplexRecipe> value = keyValuePair2.Value;
						Predicate<ComplexRecipe> match7;
						if ((match7 = <>9__1) == null)
						{
							match7 = (<>9__1 = ((ComplexRecipe match) => this.AllRecipeRequirementsDiscovered(match)));
						}
						if (value.Find(match7) == null)
						{
							goto IL_375;
						}
					}
					flag = true;
				}
			}
			else
			{
				List<ComplexRecipe> value2 = keyValuePair2.Value;
				Predicate<ComplexRecipe> match2;
				if ((match2 = <>9__2) == null)
				{
					match2 = (<>9__2 = ((ComplexRecipe match) => target.GetRecipeQueueCount(match) != 0));
				}
				if (value2.Find(match2) != null)
				{
					flag = true;
				}
				else if (keyValuePair2.Value[0].RequiresAllIngredientsDiscovered)
				{
					List<ComplexRecipe> value3 = keyValuePair2.Value;
					Predicate<ComplexRecipe> match3;
					if ((match3 = <>9__3) == null)
					{
						match3 = (<>9__3 = ((ComplexRecipe match) => this.AllRecipeRequirementsDiscovered(match)));
					}
					if (value3.Find(match3) != null)
					{
						flag = true;
					}
				}
				else
				{
					List<ComplexRecipe> value4 = keyValuePair2.Value;
					Predicate<ComplexRecipe> match4;
					if ((match4 = <>9__4) == null)
					{
						match4 = (<>9__4 = ((ComplexRecipe match) => this.AnyRecipeRequirementsDiscovered(match)));
					}
					if (value4.Find(match4) != null)
					{
						flag = true;
					}
					else
					{
						List<ComplexRecipe> value5 = keyValuePair2.Value;
						Predicate<ComplexRecipe> match5;
						if ((match5 = <>9__5) == null)
						{
							match5 = (<>9__5 = ((ComplexRecipe match) => this.HasAnyRecipeRequirements(match)));
						}
						if (value5.Find(match5) != null)
						{
							flag = true;
						}
					}
				}
			}
			IL_375:
			if (!flag)
			{
				hashSet.Add(complexRecipe2.GetUIName(false));
			}
			else
			{
				num++;
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(complexRecipe2.ingredients[0].material, "ui", false);
				global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite(complexRecipe2.results[0].material, complexRecipe2.results[0].facadeID);
				KToggle newToggle = null;
				GameObject gameObject;
				if (target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid)
				{
					newToggle = global::Util.KInstantiateUI<KToggle>(this.recipeButtonQueueHybrid, this.recipeGrid, false);
					gameObject = newToggle.gameObject;
					this.recipeCategoryToggleMap.Add(gameObject, keyValuePair2.Value);
					Image image = gameObject.GetComponentsInChildrenOnly<Image>()[2];
					if (complexRecipe2.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient)
					{
						image.sprite = uisprite.first;
						image.color = uisprite.second;
					}
					else if (complexRecipe2.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
					{
						image.sprite = this.radboltSprite;
					}
					else if (complexRecipe2.nameDisplay == ComplexRecipe.RecipeNameDisplay.Custom)
					{
						image.sprite = complexRecipe2.GetUIIcon();
					}
					else
					{
						image.sprite = uisprite2.first;
						image.color = uisprite2.second;
					}
					gameObject.GetComponentInChildren<LocText>().text = complexRecipe2.GetUIName(false);
					List<ComplexRecipe> value6 = keyValuePair2.Value;
					Predicate<ComplexRecipe> match6;
					if ((match6 = <>9__7) == null)
					{
						match6 = (<>9__7 = ((ComplexRecipe match) => this.HasAllRecipeRequirements(match)));
					}
					bool flag2 = value6.Find(match6) != null;
					image.material = (flag2 ? Assets.UIPrefabs.TableScreenWidgets.DefaultUIMaterial : Assets.UIPrefabs.TableScreenWidgets.DesaturatedUIMaterial);
					this.RefreshQueueCountDisplay(gameObject, this.targetFab);
					this.RefreshQueueTooltip(gameObject);
					gameObject.gameObject.SetActive(true);
				}
				else
				{
					newToggle = global::Util.KInstantiateUI<KToggle>(this.recipeButton, this.recipeGrid, false);
					gameObject = newToggle.gameObject;
					Image componentInChildrenOnly = newToggle.gameObject.GetComponentInChildrenOnly<Image>();
					if (target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.GridInput || target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ListInput)
					{
						componentInChildrenOnly.sprite = uisprite.first;
						componentInChildrenOnly.color = uisprite.second;
					}
					else
					{
						componentInChildrenOnly.sprite = uisprite2.first;
						componentInChildrenOnly.color = uisprite2.second;
					}
				}
				ToolTip reference = gameObject.GetComponent<HierarchyReferences>().GetReference<ToolTip>("ButtonTooltip");
				reference.toolTipPosition = ToolTip.TooltipPosition.Custom;
				reference.parentPositionAnchor = new Vector2(0f, 0.5f);
				reference.tooltipPivot = new Vector2(1f, 1f);
				reference.tooltipPositionOffset = new Vector2(-24f, 20f);
				reference.ClearMultiStringTooltip();
				reference.AddMultiStringTooltip(complexRecipe2.GetUIName(false), this.styleTooltipHeader);
				reference.AddMultiStringTooltip(complexRecipe2.description, this.styleTooltipBody);
				if (complexRecipe2.runTimeDescription != null)
				{
					reference.AddMultiStringTooltip("\n" + complexRecipe2.runTimeDescription(), this.styleTooltipBody);
				}
				if (keyValuePair2.Value.Count > 1)
				{
					reference.AddMultiStringTooltip("\n" + UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.ADDITIONAL_INGREDIENT_OPTIONS_MESSAGE, this.styleTooltipBody);
				}
				newToggle.onClick += delegate()
				{
					this.ToggleClicked(newToggle);
				};
				gameObject.SetActive(true);
				this.recipeToggles.Add(gameObject);
			}
		}
		if (this.recipeToggles.Count > 0)
		{
			VerticalLayoutGroup component = this.buttonContentContainer.GetComponent<VerticalLayoutGroup>();
			this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = Mathf.Min(451f, (float)(component.padding.top + component.padding.bottom) + (float)num * this.recipeButtonQueueHybrid.GetComponent<LayoutElement>().minHeight + (float)(num - 1) * component.spacing);
			string text = this.targetFab.SideScreenSubtitleLabel;
			if (hashSet.Count > 0)
			{
				text = string.Concat(new string[]
				{
					text,
					"  <color=#f5b042>(",
					(dictionary.Count - hashSet.Count).ToString(),
					"/",
					dictionary.Count.ToString(),
					")</color>"
				});
			}
			this.subtitleLabel.SetText(text);
			this.noRecipesDiscoveredLabel.gameObject.SetActive(false);
		}
		else
		{
			string text = string.Concat(new string[]
			{
				UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED,
				"  <color=#f5b042>(",
				(dictionary.Count - hashSet.Count).ToString(),
				"/",
				dictionary.Count.ToString(),
				")</color>"
			});
			this.subtitleLabel.SetText(text);
			this.noRecipesDiscoveredLabel.SetText(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED_BODY);
			this.noRecipesDiscoveredLabel.gameObject.SetActive(true);
			this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = this.noRecipesDiscoveredLabel.GetComponent<LayoutElement>().minHeight + 10f;
		}
		if (hashSet.Count > 0)
		{
			this.subtitleTooltip.SetSimpleTooltip(UI.UISIDESCREENS.FABRICATORSIDESCREEN.UNDISCOVERED_RECIPES + "\n\n    • " + string.Join("\n    • ", hashSet.ToArray<string>()));
		}
		else
		{
			this.subtitleTooltip.SetSimpleTooltip("");
		}
		this.RefreshIngredientAvailabilityVis();
	}

	// Token: 0x06007324 RID: 29476 RVA: 0x002BFFA4 File Offset: 0x002BE1A4
	public void RefreshQueueCountDisplayForRecipeCategory(string recipeCategoryID, ComplexFabricator fabricator)
	{
		foreach (GameObject gameObject in this.recipeToggles)
		{
			if (this.recipeCategoryToggleMap[gameObject][0].recipeCategoryID == recipeCategoryID)
			{
				this.RefreshQueueCountDisplay(gameObject, fabricator);
				this.RefreshQueueTooltip(gameObject);
				break;
			}
		}
	}

	// Token: 0x06007325 RID: 29477 RVA: 0x002C0020 File Offset: 0x002BE220
	private void RefreshQueueCountDisplay(GameObject entryGO, ComplexFabricator fabricator)
	{
		HierarchyReferences component = entryGO.GetComponent<HierarchyReferences>();
		int recipeCategoryQueueCount = fabricator.GetRecipeCategoryQueueCount(this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID);
		bool flag = recipeCategoryQueueCount == ComplexFabricator.QUEUE_INFINITE;
		component.GetReference<LocText>("CountLabel").text = (flag ? "" : recipeCategoryQueueCount.ToString());
		component.GetReference<RectTransform>("InfiniteIcon").gameObject.SetActive(flag);
		bool flag2 = !this.recipeCategoryToggleMap[entryGO][0].IsRequiredTechUnlocked();
		GameObject gameObject = component.GetReference<RectTransform>("TechRequired").gameObject;
		gameObject.SetActive(flag2);
		KButton component2 = gameObject.GetComponent<KButton>();
		component2.ClearOnClick();
		if (flag2)
		{
			component2.onClick += delegate()
			{
				ManagementMenu.Instance.OpenResearch(this.recipeCategoryToggleMap[entryGO][0].requiredTech);
			};
		}
		KButton reference = component.GetReference<KButton>("QueueBoxButton");
		reference.bgImage.colorStyleSetting = ((recipeCategoryQueueCount == 0) ? this.emptyQueueColorStyle : this.standardQueueColorStyle);
		reference.bgImage.ApplyColorStyleSetting();
		reference.ClearOnClick();
		string recipeCategoryID = this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID;
		reference.onClick += delegate()
		{
			if (this.selectedToggle == null || this.selectedToggle.gameObject != entryGO.gameObject)
			{
				this.ToggleClicked(entryGO.GetComponent<KToggle>());
			}
			else
			{
				this.recipeScreen.SelectNextQueuedRecipeInCategory();
			}
			this.RefreshQueueTooltip(entryGO);
		};
		GameObject gameObject2 = component.GetReference<RectTransform>("DotContainer").gameObject;
		GameObject gameObject3 = component.GetReference<RectTransform>("DotPrefab").gameObject;
		for (int i = 0; i < gameObject2.transform.childCount; i++)
		{
			if (gameObject2.transform.GetChild(i).gameObject != gameObject3)
			{
				UnityEngine.Object.Destroy(gameObject2.transform.GetChild(i).gameObject);
			}
		}
		int num = (from match in fabricator.GetRecipesWithCategoryID(this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID)
		where this.targetFab.GetRecipeQueueCount(match) != 0
		select match).Count<ComplexRecipe>();
		if (num > 1)
		{
			for (int j = 0; j < Mathf.Min(num, 5); j++)
			{
				global::Util.KInstantiateUI(gameObject3, gameObject2, false).SetActive(true);
			}
		}
	}

	// Token: 0x06007326 RID: 29478 RVA: 0x002C0248 File Offset: 0x002BE448
	private void RefreshQueueTooltip(GameObject entryGO)
	{
		HierarchyReferences component = entryGO.GetComponent<HierarchyReferences>();
		string recipeCategoryID = this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID;
		ToolTip reference = component.GetReference<ToolTip>("QueueTooltip");
		int recipeCategoryQueueCount = this.targetFab.GetRecipeCategoryQueueCount(recipeCategoryID);
		if (recipeCategoryQueueCount != 0)
		{
			string text = "<b>" + UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_QUEUE + ((recipeCategoryQueueCount == ComplexFabricator.QUEUE_INFINITE) ? "99+" : recipeCategoryQueueCount.ToString()) + "</b>\n";
			foreach (ComplexRecipe complexRecipe in this.targetFab.GetRecipesWithCategoryID(this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID))
			{
				int recipeQueueCount = this.targetFab.GetRecipeQueueCount(complexRecipe);
				if (recipeQueueCount != 0)
				{
					string text2 = "";
					foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
					{
						if (text2 != "")
						{
							text2 += ", ";
						}
						text2 = text2 + "<color=#C76B99>" + TagManager.GetProperName(recipeElement.material, true) + "</color>";
					}
					if (recipeQueueCount == ComplexFabricator.QUEUE_INFINITE)
					{
						text2 = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_FOREVER + ": " + text2;
					}
					else
					{
						text2 = recipeQueueCount.ToString() + "x " + text2;
					}
					if (text != "")
					{
						text += "\n";
					}
					if (this.recipeScreen != null && this.recipeScreen.gameObject.activeInHierarchy && this.recipeScreen.IsSelectedMaterials(complexRecipe))
					{
						text2 = "<b>" + text2 + "</b>";
					}
					text += text2;
				}
			}
			text = text + "\n\n" + UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_QUEUE_CLICK_DESCRIPTION;
			reference.SetSimpleTooltip(text);
			return;
		}
		reference.SetSimpleTooltip(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_NONE);
	}

	// Token: 0x06007327 RID: 29479 RVA: 0x002C0478 File Offset: 0x002BE678
	private void ToggleClicked(KToggle toggle)
	{
		if (!this.recipeCategoryToggleMap.ContainsKey(toggle.gameObject))
		{
			global::Debug.LogError("Recipe not found on recipe list.");
			return;
		}
		if (this.selectedToggle == toggle)
		{
			this.selectedToggle.isOn = false;
			this.selectedToggle = null;
			this.selectedRecipeCategory = "";
		}
		else
		{
			this.selectedToggle = toggle;
			this.selectedToggle.isOn = true;
			this.selectedRecipeCategory = this.recipeCategoryToggleMap[toggle.gameObject][0].recipeCategoryID;
			this.selectedRecipeFabricatorMap[this.targetFab] = this.recipeToggles.IndexOf(toggle.gameObject);
		}
		this.RefreshIngredientAvailabilityVis();
		if (toggle.isOn)
		{
			this.recipeScreen = (SelectedRecipeQueueScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.recipeScreenPrefab, this.targetFab.SideScreenRecipeScreenTitle);
			this.recipeScreen.SetRecipeCategory(this, this.targetFab, this.selectedRecipeCategory);
			return;
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x06007328 RID: 29480 RVA: 0x002C0580 File Offset: 0x002BE780
	public void CycleRecipe(int increment)
	{
		int num = 0;
		if (this.selectedToggle != null)
		{
			num = this.recipeToggles.IndexOf(this.selectedToggle.gameObject);
		}
		int num2 = (num + increment) % this.recipeToggles.Count;
		if (num2 < 0)
		{
			num2 = this.recipeToggles.Count + num2;
		}
		this.ToggleClicked(this.recipeToggles[num2].GetComponent<KToggle>());
	}

	// Token: 0x06007329 RID: 29481 RVA: 0x002C05F0 File Offset: 0x002BE7F0
	private bool HasAnyRecipeRequirements(ComplexRecipe recipe)
	{
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (this.targetFab.GetMyWorld().worldInventory.GetAmountWithoutTag(recipeElement.material, true, this.targetFab.ForbiddenTags) + this.targetFab.inStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) + this.targetFab.buildStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) >= recipeElement.amount)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600732A RID: 29482 RVA: 0x002C0690 File Offset: 0x002BE890
	private bool HasAllRecipeRequirements(ComplexRecipe recipe)
	{
		bool result = true;
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (this.targetFab.GetMyWorld().worldInventory.GetAmountWithoutTag(recipeElement.material, true, this.targetFab.ForbiddenTags) + this.targetFab.inStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) + this.targetFab.buildStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) < recipeElement.amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600732B RID: 29483 RVA: 0x002C0734 File Offset: 0x002BE934
	private bool AnyRecipeRequirementsDiscovered(ComplexRecipe recipe)
	{
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (DiscoveredResources.Instance.IsDiscovered(recipeElement.material))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600732C RID: 29484 RVA: 0x002C0770 File Offset: 0x002BE970
	private bool AllRecipeRequirementsDiscovered(ComplexRecipe recipe)
	{
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (!DiscoveredResources.Instance.IsDiscovered(recipeElement.material))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600732D RID: 29485 RVA: 0x002C07AB File Offset: 0x002BE9AB
	private void Update()
	{
		this.RefreshIngredientAvailabilityVis();
	}

	// Token: 0x0600732E RID: 29486 RVA: 0x002C07B3 File Offset: 0x002BE9B3
	private void UnsubscribeTarget()
	{
		if (this.targetOrdersUpdatedSubHandle != -1 && this.targetFab != null)
		{
			this.targetFab.Unsubscribe(this.targetOrdersUpdatedSubHandle);
			this.targetOrdersUpdatedSubHandle = -1;
		}
	}

	// Token: 0x0600732F RID: 29487 RVA: 0x002C07E4 File Offset: 0x002BE9E4
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.UnsubscribeTarget();
	}

	// Token: 0x06007330 RID: 29488 RVA: 0x002C07F4 File Offset: 0x002BE9F4
	private void RefreshIngredientAvailabilityVis()
	{
		foreach (KeyValuePair<GameObject, List<ComplexRecipe>> keyValuePair in this.recipeCategoryToggleMap)
		{
			HierarchyReferences component = keyValuePair.Key.GetComponent<HierarchyReferences>();
			bool flag = keyValuePair.Value.Find((ComplexRecipe match) => this.HasAllRecipeRequirements(match)) != null;
			KToggle component2 = keyValuePair.Key.GetComponent<KToggle>();
			if (flag)
			{
				if (keyValuePair.Value[0].recipeCategoryID == this.selectedRecipeCategory)
				{
					component2.ActivateFlourish(true, ImageToggleState.State.Active);
				}
				else
				{
					component2.ActivateFlourish(false, ImageToggleState.State.Inactive);
				}
			}
			else if (keyValuePair.Value[0].recipeCategoryID == this.selectedRecipeCategory)
			{
				component2.ActivateFlourish(true, ImageToggleState.State.DisabledActive);
			}
			else
			{
				component2.ActivateFlourish(false, ImageToggleState.State.Disabled);
			}
			component.GetReference<LocText>("Label").color = (flag ? Color.black : new Color(0.22f, 0.22f, 0.22f, 1f));
		}
	}

	// Token: 0x04004F91 RID: 20369
	[Header("Recipe List")]
	[SerializeField]
	private GameObject recipeGrid;

	// Token: 0x04004F92 RID: 20370
	[Header("Recipe button variants")]
	[SerializeField]
	private GameObject recipeButton;

	// Token: 0x04004F93 RID: 20371
	[SerializeField]
	private GameObject recipeButtonMultiple;

	// Token: 0x04004F94 RID: 20372
	[SerializeField]
	private GameObject recipeButtonQueueHybrid;

	// Token: 0x04004F95 RID: 20373
	[SerializeField]
	private GameObject recipeCategoryHeader;

	// Token: 0x04004F96 RID: 20374
	[SerializeField]
	private Sprite buttonSelectedBG;

	// Token: 0x04004F97 RID: 20375
	[SerializeField]
	private Sprite buttonNormalBG;

	// Token: 0x04004F98 RID: 20376
	[SerializeField]
	private Sprite elementPlaceholderSpr;

	// Token: 0x04004F99 RID: 20377
	[SerializeField]
	public Sprite radboltSprite;

	// Token: 0x04004F9A RID: 20378
	private KToggle selectedToggle;

	// Token: 0x04004F9B RID: 20379
	public LayoutElement buttonScrollContainer;

	// Token: 0x04004F9C RID: 20380
	public RectTransform buttonContentContainer;

	// Token: 0x04004F9D RID: 20381
	[SerializeField]
	private GameObject elementContainer;

	// Token: 0x04004F9E RID: 20382
	[SerializeField]
	private LocText currentOrderLabel;

	// Token: 0x04004F9F RID: 20383
	[SerializeField]
	private LocText nextOrderLabel;

	// Token: 0x04004FA0 RID: 20384
	private Dictionary<ComplexFabricator, int> selectedRecipeFabricatorMap = new Dictionary<ComplexFabricator, int>();

	// Token: 0x04004FA1 RID: 20385
	public EventReference createOrderSound;

	// Token: 0x04004FA2 RID: 20386
	[SerializeField]
	private RectTransform content;

	// Token: 0x04004FA3 RID: 20387
	[SerializeField]
	private LocText subtitleLabel;

	// Token: 0x04004FA4 RID: 20388
	[SerializeField]
	private ToolTip subtitleTooltip;

	// Token: 0x04004FA5 RID: 20389
	[SerializeField]
	private LocText noRecipesDiscoveredLabel;

	// Token: 0x04004FA6 RID: 20390
	public TextStyleSetting styleTooltipHeader;

	// Token: 0x04004FA7 RID: 20391
	public TextStyleSetting styleTooltipBody;

	// Token: 0x04004FA8 RID: 20392
	public ColorStyleSetting emptyQueueColorStyle;

	// Token: 0x04004FA9 RID: 20393
	public ColorStyleSetting standardQueueColorStyle;

	// Token: 0x04004FAA RID: 20394
	private ComplexFabricator targetFab;

	// Token: 0x04004FAB RID: 20395
	private string selectedRecipeCategory;

	// Token: 0x04004FAC RID: 20396
	private Dictionary<GameObject, List<ComplexRecipe>> recipeCategoryToggleMap;

	// Token: 0x04004FAD RID: 20397
	private Dictionary<string, GameObject> recipeCategories = new Dictionary<string, GameObject>();

	// Token: 0x04004FAE RID: 20398
	private List<GameObject> recipeToggles = new List<GameObject>();

	// Token: 0x04004FAF RID: 20399
	public SelectedRecipeQueueScreen recipeScreenPrefab;

	// Token: 0x04004FB0 RID: 20400
	private SelectedRecipeQueueScreen recipeScreen;

	// Token: 0x04004FB1 RID: 20401
	private int targetOrdersUpdatedSubHandle = -1;

	// Token: 0x020020AE RID: 8366
	public enum StyleSetting
	{
		// Token: 0x040096E4 RID: 38628
		GridResult,
		// Token: 0x040096E5 RID: 38629
		ListResult,
		// Token: 0x040096E6 RID: 38630
		GridInput,
		// Token: 0x040096E7 RID: 38631
		ListInput,
		// Token: 0x040096E8 RID: 38632
		ListInputOutput,
		// Token: 0x040096E9 RID: 38633
		GridInputOutput,
		// Token: 0x040096EA RID: 38634
		ClassicFabricator,
		// Token: 0x040096EB RID: 38635
		ListQueueHybrid
	}
}
