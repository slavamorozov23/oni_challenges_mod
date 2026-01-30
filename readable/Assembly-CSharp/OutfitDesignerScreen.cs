using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DD0 RID: 3536
public class OutfitDesignerScreen : KMonoBehaviour
{
	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x06006E8C RID: 28300 RVA: 0x0029DB89 File Offset: 0x0029BD89
	// (set) Token: 0x06006E8D RID: 28301 RVA: 0x0029DB91 File Offset: 0x0029BD91
	public OutfitDesignerScreenConfig Config { get; private set; }

	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x06006E8E RID: 28302 RVA: 0x0029DB9A File Offset: 0x0029BD9A
	// (set) Token: 0x06006E8F RID: 28303 RVA: 0x0029DBA2 File Offset: 0x0029BDA2
	public PermitResource SelectedPermit { get; private set; }

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x06006E90 RID: 28304 RVA: 0x0029DBAB File Offset: 0x0029BDAB
	// (set) Token: 0x06006E91 RID: 28305 RVA: 0x0029DBB3 File Offset: 0x0029BDB3
	public PermitCategory SelectedCategory { get; private set; }

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x06006E92 RID: 28306 RVA: 0x0029DBBC File Offset: 0x0029BDBC
	// (set) Token: 0x06006E93 RID: 28307 RVA: 0x0029DBC4 File Offset: 0x0029BDC4
	public OutfitDesignerScreen_OutfitState outfitState { get; private set; }

	// Token: 0x06006E94 RID: 28308 RVA: 0x0029DBD0 File Offset: 0x0029BDD0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(this.categoryRowPrefab.transform.parent == this.categoryListContent.transform);
		global::Debug.Assert(this.gridItemPrefab.transform.parent == this.galleryGridContent.transform);
		global::Debug.Assert(this.subcategoryUiPrefab.transform.parent == this.galleryGridContent.transform);
		this.categoryRowPrefab.SetActive(false);
		this.gridItemPrefab.SetActive(false);
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
		this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
		this.subcategoryUiPool = new UIPrefabLocalPool(this.subcategoryUiPrefab, this.galleryGridContent.gameObject);
		if (OutfitDesignerScreen.outfitTypeToCategoriesDict == null)
		{
			Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> dictionary = new Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]>();
			dictionary[ClothingOutfitUtility.OutfitType.Clothing] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING;
			dictionary[ClothingOutfitUtility.OutfitType.AtmoSuit] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS;
			dictionary[ClothingOutfitUtility.OutfitType.JetSuit] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_JET_SUITS;
			OutfitDesignerScreen.outfitTypeToCategoriesDict = dictionary;
		}
		InventoryOrganization.Initialize();
	}

	// Token: 0x06006E95 RID: 28309 RVA: 0x0029DD40 File Offset: 0x0029BF40
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06006E96 RID: 28310 RVA: 0x0029DD50 File Offset: 0x0029BF50
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		this.minionOrMannequin.TrySpawn();
		if (!this.Config.isValid)
		{
			throw new NotSupportedException("Cannot open OutfitDesignerScreen without a config. Make sure to call Configure() before enabling the screen");
		}
		this.dlcFilter.ConfigButtons();
		this.dlcFilter.onDLCFilterChanged = new System.Action(this.RefreshGallery);
		this.Configure(this.Config);
	}

	// Token: 0x06006E97 RID: 28311 RVA: 0x0029DDB6 File Offset: 0x0029BFB6
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.dlcFilter.ResetToDefault();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshCategories();
			this.RefreshGallery();
			this.RefreshOutfitState();
		});
	}

	// Token: 0x06006E98 RID: 28312 RVA: 0x0029DDE0 File Offset: 0x0029BFE0
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.dlcFilter.ResetToDefault();
		this.dlcFilter.HideDropdown();
		this.UnregisterPreventScreenPop();
	}

	// Token: 0x06006E99 RID: 28313 RVA: 0x0029DE04 File Offset: 0x0029C004
	private void UpdateSaveButtons()
	{
		if (this.updateSaveButtonsFn != null)
		{
			this.updateSaveButtonsFn();
		}
	}

	// Token: 0x06006E9A RID: 28314 RVA: 0x0029DE1C File Offset: 0x0029C01C
	public void Configure(OutfitDesignerScreenConfig config)
	{
		this.Config = config;
		if (config.targetMinionInstance.HasValue)
		{
			this.outfitState = OutfitDesignerScreen_OutfitState.ForMinionInstance(this.Config.sourceTarget, config.targetMinionInstance.Value);
		}
		else
		{
			this.outfitState = OutfitDesignerScreen_OutfitState.ForTemplateOutfit(this.Config.sourceTarget);
		}
		if (this.postponeConfiguration)
		{
			return;
		}
		this.RegisterPreventScreenPop();
		this.minionOrMannequin.SetFrom(config.minionPersonality).SpawnedAvatar.GetComponent<WearableAccessorizer>();
		using (ListPool<ClothingItemResource, OutfitDesignerScreen>.PooledList pooledList = PoolsFor<OutfitDesignerScreen>.AllocateList<ClothingItemResource>())
		{
			this.outfitState.AddItemValuesTo(pooledList);
			this.minionOrMannequin.SetFrom(config.minionPersonality).SetOutfit(config.sourceTarget.OutfitType, pooledList);
		}
		this.PopulateCategories();
		this.SelectCategory(OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType][0]);
		this.galleryGridLayouter.RequestGridResize();
		this.RefreshOutfitState();
		OutfitDesignerScreenConfig config2 = this.Config;
		if (config2.targetMinionInstance.HasValue)
		{
			this.updateSaveButtonsFn = null;
			this.primaryButton.ClearOnClick();
			TMP_Text componentInChildren = this.primaryButton.GetComponentInChildren<LocText>();
			LocString button_APPLY_TO_MINION = UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_MINION;
			string search = "{MinionName}";
			config2 = this.Config;
			componentInChildren.SetText(button_APPLY_TO_MINION.Replace(search, config2.targetMinionInstance.Value.GetProperName()));
			this.primaryButton.onClick += delegate()
			{
				OutfitDesignerScreenConfig config3 = this.Config;
				ClothingOutfitUtility.OutfitType outfitType = config3.sourceTarget.OutfitType;
				config3 = this.Config;
				ClothingOutfitTarget obj = ClothingOutfitTarget.FromMinion(outfitType, config3.targetMinionInstance.Value);
				config3 = this.Config;
				obj.WriteItems(config3.sourceTarget.OutfitType, this.outfitState.GetItems());
				if (this.Config.onWriteToOutfitTargetFn != null)
				{
					this.Config.onWriteToOutfitTargetFn(obj);
				}
				LockerNavigator.Instance.PopScreen();
			};
			this.secondaryButton.ClearOnClick();
			this.secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_TEMPLATE);
			this.secondaryButton.onClick += delegate()
			{
				OutfitDesignerScreen.MakeApplyToTemplatePopup(this.inputFieldPrefab, this.outfitState, this.Config.targetMinionInstance.Value, this.Config.outfitTemplate, this.Config.onWriteToOutfitTargetFn);
			};
			this.updateSaveButtonsFn = (System.Action)Delegate.Combine(this.updateSaveButtonsFn, new System.Action(delegate()
			{
				if (this.outfitState.DoesContainLockedItems())
				{
					this.primaryButton.isInteractable = false;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					this.secondaryButton.isInteractable = false;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					return;
				}
				this.primaryButton.isInteractable = true;
				this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
				this.secondaryButton.isInteractable = true;
				this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
			}));
		}
		else
		{
			config2 = this.Config;
			if (!config2.outfitTemplate.HasValue)
			{
				throw new NotSupportedException();
			}
			this.updateSaveButtonsFn = null;
			this.primaryButton.ClearOnClick();
			this.primaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_SAVE);
			this.primaryButton.onClick += delegate()
			{
				this.outfitState.destinationTarget.WriteName(this.outfitState.name);
				this.outfitState.destinationTarget.WriteItems(this.outfitState.outfitType, this.outfitState.GetItems());
				OutfitDesignerScreenConfig config3 = this.Config;
				if (config3.minionPersonality.HasValue)
				{
					config3 = this.Config;
					config3.minionPersonality.Value.SetSelectedTemplateOutfitId(this.outfitState.destinationTarget.OutfitType, this.outfitState.destinationTarget.OutfitId);
				}
				if (this.Config.onWriteToOutfitTargetFn != null)
				{
					this.Config.onWriteToOutfitTargetFn(this.outfitState.destinationTarget);
				}
				LockerNavigator.Instance.PopScreen();
			};
			this.secondaryButton.ClearOnClick();
			this.secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_COPY);
			this.secondaryButton.onClick += delegate()
			{
				OutfitDesignerScreen.MakeCopyPopup(this, this.inputFieldPrefab, this.outfitState, this.Config.outfitTemplate.Value, this.Config.minionPersonality, this.Config.onWriteToOutfitTargetFn);
			};
			this.updateSaveButtonsFn = (System.Action)Delegate.Combine(this.updateSaveButtonsFn, new System.Action(delegate()
			{
				if (!this.outfitState.destinationTarget.CanWriteItems)
				{
					this.primaryButton.isInteractable = false;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_READONLY);
					if (this.outfitState.DoesContainLockedItems())
					{
						this.secondaryButton.isInteractable = false;
						this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						return;
					}
					this.secondaryButton.isInteractable = true;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					return;
				}
				else
				{
					if (this.outfitState.DoesContainLockedItems())
					{
						this.primaryButton.isInteractable = false;
						this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						this.secondaryButton.isInteractable = false;
						this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						return;
					}
					this.primaryButton.isInteractable = true;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					this.secondaryButton.isInteractable = true;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					return;
				}
			}));
		}
		this.UpdateSaveButtons();
	}

	// Token: 0x06006E9B RID: 28315 RVA: 0x0029E0CC File Offset: 0x0029C2CC
	private void RefreshOutfitState()
	{
		this.selectionHeaderLabel.text = this.outfitState.name;
		this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
		this.UpdateSaveButtons();
	}

	// Token: 0x06006E9C RID: 28316 RVA: 0x0029E106 File Offset: 0x0029C306
	private void RefreshCategories()
	{
		if (this.RefreshCategoriesFn != null)
		{
			this.RefreshCategoriesFn();
		}
	}

	// Token: 0x06006E9D RID: 28317 RVA: 0x0029E11C File Offset: 0x0029C31C
	public void PopulateCategories()
	{
		this.RefreshCategoriesFn = null;
		this.categoryRowPool.ReturnAll();
		PermitCategory[] array = OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType];
		for (int i = 0; i < array.Length; i++)
		{
			OutfitDesignerScreen.<>c__DisplayClass50_0 CS$<>8__locals1 = new OutfitDesignerScreen.<>c__DisplayClass50_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.permitCategory = array[i];
			GameObject gameObject = this.categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(PermitCategories.GetUppercaseDisplayName(CS$<>8__locals1.permitCategory));
			component.GetReference<Image>("Icon").sprite = Assets.GetSprite(PermitCategories.GetIconName(CS$<>8__locals1.permitCategory));
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle toggle2 = toggle;
			toggle2.onEnter = (System.Action)Delegate.Combine(toggle2.onEnter, new System.Action(this.OnMouseOverToggle));
			toggle.onClick = delegate()
			{
				CS$<>8__locals1.<>4__this.SelectCategory(CS$<>8__locals1.permitCategory);
			};
			this.RefreshCategoriesFn = (System.Action)Delegate.Combine(this.RefreshCategoriesFn, new System.Action(delegate()
			{
				toggle.ChangeState((CS$<>8__locals1.permitCategory == CS$<>8__locals1.<>4__this.SelectedCategory) ? 1 : 0);
			}));
			this.SetCatogoryClickUISound(CS$<>8__locals1.permitCategory, toggle);
		}
	}

	// Token: 0x06006E9E RID: 28318 RVA: 0x0029E270 File Offset: 0x0029C470
	public void SelectCategory(PermitCategory permitCategory)
	{
		this.SelectedCategory = permitCategory;
		this.galleryHeaderLabel.text = PermitCategories.GetDisplayName(permitCategory);
		this.RefreshCategories();
		this.PopulateGallery();
		Option<ClothingItemResource> itemForCategory = this.outfitState.GetItemForCategory(permitCategory);
		if (itemForCategory.HasValue)
		{
			this.SelectPermit(itemForCategory.Value);
			return;
		}
		this.SelectPermit(null);
	}

	// Token: 0x06006E9F RID: 28319 RVA: 0x0029E2CC File Offset: 0x0029C4CC
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.permitItemToggles)
		{
			string dlcIdFrom = Db.Get().Permits.ClothingItems.Get(keyValuePair.Key).GetDlcIdFrom();
			keyValuePair.Value.SetActive(this.dlcFilter.SelectedDLCID == null || dlcIdFrom == this.dlcFilter.SelectedDLCID);
		}
		foreach (GameObject gameObject in this.nonPermitItemToggles)
		{
			gameObject.SetActive(this.dlcFilter.SelectedDLCID == null);
		}
		foreach (GameObject gameObject2 in this.subcategoryUiPool.GetBorrowedObjects())
		{
			gameObject2.GetComponent<KleiInventoryUISubcategory>().RefreshDisplay();
		}
	}

	// Token: 0x06006EA0 RID: 28320 RVA: 0x0029E410 File Offset: 0x0029C610
	public void PopulateGallery()
	{
		OutfitDesignerScreen.<>c__DisplayClass54_0 CS$<>8__locals1 = new OutfitDesignerScreen.<>c__DisplayClass54_0();
		CS$<>8__locals1.<>4__this = this;
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		this.subcategoryUiPool.ReturnAll();
		this.galleryGridLayouter.targetGridLayouts.Clear();
		this.galleryGridLayouter.OnSizeGridComplete = null;
		this.permitItemToggles.Clear();
		this.nonPermitItemToggles.Clear();
		CS$<>8__locals1.onFirstDisplayCategoryDecided = new Promise<KleiInventoryUISubcategory>();
		CS$<>8__locals1.<PopulateGallery>g__AddGridIconForPermit|0(null);
		foreach (ClothingItemResource clothingItemResource in Db.Get().Permits.ClothingItems.resources)
		{
			if (clothingItemResource.Category == this.SelectedCategory && clothingItemResource.outfitType == this.Config.sourceTarget.OutfitType && !clothingItemResource.Id.StartsWith("visonly_"))
			{
				CS$<>8__locals1.<PopulateGallery>g__AddGridIconForPermit|0(clothingItemResource);
			}
		}
		foreach (GameObject gameObject3 in this.subcategoryUiPool.GetBorrowedObjects().StableSort(Comparer<GameObject>.Create(delegate(GameObject a, GameObject b)
		{
			KleiInventoryUISubcategory component = a.GetComponent<KleiInventoryUISubcategory>();
			KleiInventoryUISubcategory component2 = b.GetComponent<KleiInventoryUISubcategory>();
			int sortKey = InventoryOrganization.subcategoryIdToPresentationDataMap[component.subcategoryID].sortKey;
			int sortKey2 = InventoryOrganization.subcategoryIdToPresentationDataMap[component2.subcategoryID].sortKey;
			return sortKey.CompareTo(sortKey2);
		})))
		{
			gameObject3.transform.SetAsLastSibling();
		}
		GameObject gameObject2 = this.subcategoryUiPool.GetBorrowedObjects().FirstOrDefault((GameObject gameObject) => gameObject.GetComponent<KleiInventoryUISubcategory>().IsOpen);
		if (gameObject2 != null)
		{
			CS$<>8__locals1.onFirstDisplayCategoryDecided.Resolve(gameObject2.GetComponent<KleiInventoryUISubcategory>());
		}
		this.galleryGridLayouter.RequestGridResize();
		this.RefreshGallery();
	}

	// Token: 0x06006EA1 RID: 28321 RVA: 0x0029E5EC File Offset: 0x0029C7EC
	public void SelectPermit(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.UpdateSelectedItemDetails();
		this.UpdateSaveButtons();
	}

	// Token: 0x06006EA2 RID: 28322 RVA: 0x0029E608 File Offset: 0x0029C808
	public void UpdateSelectedItemDetails()
	{
		Option<ClothingItemResource> item = Option.None;
		if (this.SelectedPermit != null)
		{
			ClothingItemResource clothingItemResource = this.SelectedPermit as ClothingItemResource;
			if (clothingItemResource != null)
			{
				item = clothingItemResource;
			}
		}
		this.outfitState.SetItemForCategory(this.SelectedCategory, item);
		this.minionOrMannequin.current.SetOutfit(this.outfitState);
		this.minionOrMannequin.current.ReactToClothingItemChange(this.SelectedCategory);
		this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
		this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.SelectedCategory);
	}

	// Token: 0x06006EA3 RID: 28323 RVA: 0x0029E6AE File Offset: 0x0029C8AE
	private void RegisterPreventScreenPop()
	{
		this.UnregisterPreventScreenPop();
		this.preventScreenPopFn = delegate()
		{
			if (this.dlcFilter.IsDropdownVisible())
			{
				this.RegisterPreventScreenPop();
				this.dlcFilter.ResetToDefault();
				this.dlcFilter.HideDropdown();
				return true;
			}
			if (this.outfitState.IsDirty())
			{
				this.RegisterPreventScreenPop();
				OutfitDesignerScreen.MakeSaveWarningPopup(this.outfitState, delegate
				{
					this.UnregisterPreventScreenPop();
					LockerNavigator.Instance.PopScreen();
				});
				return true;
			}
			return false;
		};
		LockerNavigator.Instance.preventScreenPop.Add(this.preventScreenPopFn);
	}

	// Token: 0x06006EA4 RID: 28324 RVA: 0x0029E6DD File Offset: 0x0029C8DD
	private void UnregisterPreventScreenPop()
	{
		if (this.preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
			this.preventScreenPopFn = null;
		}
	}

	// Token: 0x06006EA5 RID: 28325 RVA: 0x0029E704 File Offset: 0x0029C904
	public static void MakeSaveWarningPopup(OutfitDesignerScreen_OutfitState outfitState, System.Action discardChangesFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY);
			string text = UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_DISCARD;
			Action<InfoDialogScreen> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					discardChangesFn();
				});
			}
			infoDialogScreen.AddOption(text, action, true).AddOption(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_RETURN, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
		});
	}

	// Token: 0x06006EA6 RID: 28326 RVA: 0x0029E73C File Offset: 0x0029C93C
	public static void MakeApplyToTemplatePopup(KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, GameObject targetMinionInstance, Option<ClothingOutfitTarget> existingOutfitTemplate, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		Color errorTextColor = Util.ColorFromHex("F44A47");
		Color defaultTextColor = Util.ColorFromHex("FFFFFF");
		KInputTextField inputField;
		InfoScreenPlainText descLabel;
		KButton saveButton;
		LocText saveButtonText;
		LocText descLocText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out descLabel).AddOption(true, out saveButton, out saveButtonText).AddDefaultCancel();
			descLocText = descLabel.gameObject.GetComponent<LocText>();
			descLocText.allowOverride = true;
			descLocText.alignment = TextAlignmentOptions.BottomLeft;
			descLocText.color = errorTextColor;
			descLocText.fontSize = 14f;
			descLabel.SetText("");
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeApplyToTemplatePopup>g__Refresh|1));
			saveButton.onClick += delegate()
			{
				ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.FromMinion(outfitState.outfitType, targetMinionInstance);
				ClothingOutfitNameProposal.Result result = proposal.result;
				ClothingOutfitTarget obj;
				if (result != ClothingOutfitNameProposal.Result.NewOutfit)
				{
					if (result != ClothingOutfitNameProposal.Result.SameOutfit)
					{
						throw new NotSupportedException(string.Format("Can't save outfit with name \"{0}\", failed with result: {1}", proposal.candidateName, proposal.result));
					}
					obj = existingOutfitTemplate.Value;
				}
				else
				{
					obj = ClothingOutfitTarget.ForNewTemplateOutfit(outfitState.outfitType, proposal.candidateName);
				}
				obj.WriteItems(outfitState.outfitType, outfitState.GetItems());
				clothingOutfitTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
				if (onWriteToOutfitTargetFn != null)
				{
					onWriteToOutfitTargetFn(obj);
				}
				dialog.Deactivate();
				LockerNavigator.Instance.PopScreen();
			};
			if (!existingOutfitTemplate.HasValue)
			{
				base.<MakeApplyToTemplatePopup>g__Refresh|1(outfitState.name);
				return;
			}
			if (existingOutfitTemplate.Value.CanWriteName && existingOutfitTemplate.Value.CanWriteItems)
			{
				base.<MakeApplyToTemplatePopup>g__Refresh|1(existingOutfitTemplate.Value.OutfitId);
				return;
			}
			base.<MakeApplyToTemplatePopup>g__Refresh|1(ClothingOutfitTarget.ForTemplateCopyOf(existingOutfitTemplate.Value).OutfitId);
		});
	}

	// Token: 0x06006EA7 RID: 28327 RVA: 0x0029E7B8 File Offset: 0x0029C9B8
	public static void MakeCopyPopup(OutfitDesignerScreen screen, KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, ClothingOutfitTarget outfitTemplate, Option<Personality> minionPersonality, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.COPY_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeCopyPopup>g__Refresh|1));
			errorText.gameObject.SetActive(false);
			LocText component = errorText.gameObject.GetComponent<LocText>();
			component.allowOverride = true;
			component.alignment = TextAlignmentOptions.BottomLeft;
			component.color = Util.ColorFromHex("F44A47");
			component.fontSize = 14f;
			errorText.SetText("");
			okButtonText.text = UI.CONFIRMDIALOG.OK;
			okButton.onClick += delegate()
			{
				if (proposal.result == ClothingOutfitNameProposal.Result.NewOutfit)
				{
					ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.ForNewTemplateOutfit(outfitTemplate.OutfitType, proposal.candidateName);
					clothingOutfitTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
					if (minionPersonality.HasValue)
					{
						minionPersonality.Value.SetSelectedTemplateOutfitId(clothingOutfitTarget.OutfitType, clothingOutfitTarget.OutfitId);
					}
					if (onWriteToOutfitTargetFn != null)
					{
						onWriteToOutfitTargetFn(clothingOutfitTarget);
					}
					dialog.Deactivate();
					screen.Configure(screen.Config.WithOutfit(clothingOutfitTarget));
					return;
				}
				throw new NotSupportedException(string.Format("Can't save outfit with name \"{0}\", failed with result: {1}", proposal.candidateName, proposal.result));
			};
			base.<MakeCopyPopup>g__Refresh|1(ClothingOutfitTarget.ForTemplateCopyOf(outfitTemplate).OutfitId);
		});
	}

	// Token: 0x06006EA8 RID: 28328 RVA: 0x0029E81C File Offset: 0x0029CA1C
	private void SetCatogoryClickUISound(PermitCategory category, MultiToggle toggle)
	{
		toggle.states[1].on_click_override_sound_path = category.ToString() + "_Click";
		toggle.states[0].on_click_override_sound_path = category.ToString() + "_Click";
	}

	// Token: 0x06006EA9 RID: 28329 RVA: 0x0029E87C File Offset: 0x0029CA7C
	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		if (permit == null)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		string clothingItemSoundName = OutfitDesignerScreen.GetClothingItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x06006EAA RID: 28330 RVA: 0x0029E994 File Offset: 0x0029CB94
	public static string GetClothingItemSoundName(PermitResource permit)
	{
		if (permit == null)
		{
			return "HUD";
		}
		switch (permit.Category)
		{
		case PermitCategory.DupeTops:
			return "tops";
		case PermitCategory.DupeBottoms:
			return "bottoms";
		case PermitCategory.DupeGloves:
			return "gloves";
		case PermitCategory.DupeShoes:
			return "shoes";
		case PermitCategory.DupeHats:
			return "hats";
		default:
			return "HUD";
		}
	}

	// Token: 0x06006EAB RID: 28331 RVA: 0x0029E9F5 File Offset: 0x0029CBF5
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x04004B95 RID: 19349
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x04004B96 RID: 19350
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x04004B97 RID: 19351
	private UIPrefabLocalPool categoryRowPool;

	// Token: 0x04004B98 RID: 19352
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04004B99 RID: 19353
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04004B9A RID: 19354
	[SerializeField]
	private GameObject subcategoryUiPrefab;

	// Token: 0x04004B9B RID: 19355
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04004B9C RID: 19356
	private UIPrefabLocalPool subcategoryUiPool;

	// Token: 0x04004B9D RID: 19357
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04004B9E RID: 19358
	private List<GameObject> nonPermitItemToggles = new List<GameObject>();

	// Token: 0x04004B9F RID: 19359
	private Dictionary<string, GameObject> permitItemToggles = new Dictionary<string, GameObject>();

	// Token: 0x04004BA0 RID: 19360
	private GridLayouter galleryGridLayouter;

	// Token: 0x04004BA1 RID: 19361
	[SerializeField]
	private KleiInventoryDLCFilter dlcFilter;

	// Token: 0x04004BA2 RID: 19362
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04004BA3 RID: 19363
	[SerializeField]
	private UIMinionOrMannequin minionOrMannequin;

	// Token: 0x04004BA4 RID: 19364
	[SerializeField]
	private Image dioramaBG;

	// Token: 0x04004BA5 RID: 19365
	[SerializeField]
	private KButton primaryButton;

	// Token: 0x04004BA6 RID: 19366
	[SerializeField]
	private KButton secondaryButton;

	// Token: 0x04004BA7 RID: 19367
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04004BA8 RID: 19368
	[SerializeField]
	private KInputTextField inputFieldPrefab;

	// Token: 0x04004BAD RID: 19373
	public static Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> outfitTypeToCategoriesDict;

	// Token: 0x04004BAE RID: 19374
	private bool postponeConfiguration = true;

	// Token: 0x04004BAF RID: 19375
	private System.Action updateSaveButtonsFn;

	// Token: 0x04004BB0 RID: 19376
	private System.Action RefreshCategoriesFn;

	// Token: 0x04004BB1 RID: 19377
	private System.Action RefreshGalleryFn;

	// Token: 0x04004BB2 RID: 19378
	private Func<bool> preventScreenPopFn;

	// Token: 0x02002027 RID: 8231
	private enum MultiToggleState
	{
		// Token: 0x0400950C RID: 38156
		Default,
		// Token: 0x0400950D RID: 38157
		Selected,
		// Token: 0x0400950E RID: 38158
		NonInteractable
	}
}
