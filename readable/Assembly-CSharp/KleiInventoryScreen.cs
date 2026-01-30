using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D40 RID: 3392
public class KleiInventoryScreen : KModalScreen
{
	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x060068FA RID: 26874 RVA: 0x0027B96D File Offset: 0x00279B6D
	// (set) Token: 0x060068FB RID: 26875 RVA: 0x0027B975 File Offset: 0x00279B75
	private PermitResource SelectedPermit { get; set; }

	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x060068FC RID: 26876 RVA: 0x0027B97E File Offset: 0x00279B7E
	// (set) Token: 0x060068FD RID: 26877 RVA: 0x0027B986 File Offset: 0x00279B86
	private string SelectedCategoryId { get; set; }

	// Token: 0x060068FE RID: 26878 RVA: 0x0027B990 File Offset: 0x00279B90
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		base.ConsumeMouseScroll = true;
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = new List<GridLayoutGroup>()
		};
		this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
		InventoryOrganization.Initialize();
	}

	// Token: 0x060068FF RID: 26879 RVA: 0x0027BA03 File Offset: 0x00279C03
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (KPrivacyPrefs.instance.disableDataCollection)
		{
			this.barterOfflineLabel.GetComponent<ToolTip>().SetSimpleTooltip(UI.LOCKER_MENU.OFFLINE_ICON_TOOLTIP_DATA_COLLECTIONS);
		}
	}

	// Token: 0x06006900 RID: 26880 RVA: 0x0027BA31 File Offset: 0x00279C31
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006901 RID: 26881 RVA: 0x0027BA53 File Offset: 0x00279C53
	public override float GetSortKey()
	{
		return 20f;
	}

	// Token: 0x06006902 RID: 26882 RVA: 0x0027BA5A File Offset: 0x00279C5A
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x06006903 RID: 26883 RVA: 0x0027BA63 File Offset: 0x00279C63
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.InitConfig();
			this.ToggleDoublesOnly(0);
			this.ClearSearch();
			return;
		}
		this.dlcFilter.ResetToDefault();
		this.dlcFilter.HideDropdown();
	}

	// Token: 0x06006904 RID: 26884 RVA: 0x0027BA9C File Offset: 0x00279C9C
	private void ToggleDoublesOnly(int newState)
	{
		this.showFilterState = newState;
		this.doublesOnlyToggle.ChangeState(this.showFilterState);
		this.doublesOnlyToggle.GetComponentInChildren<LocText>().text = this.showFilterState.ToString() + "+";
		string simpleTooltip = "";
		switch (this.showFilterState)
		{
		case 0:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_ALL_ITEMS;
			break;
		case 1:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_OWNED_ONLY;
			break;
		case 2:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_DOUBLES_ONLY;
			break;
		}
		ToolTip component = this.doublesOnlyToggle.GetComponent<ToolTip>();
		component.SetSimpleTooltip(simpleTooltip);
		component.refreshWhileHovering = true;
		component.forceRefresh = true;
		this.RefreshGallery();
	}

	// Token: 0x06006905 RID: 26885 RVA: 0x0027BB54 File Offset: 0x00279D54
	private void InitConfig()
	{
		if (this.initConfigComplete)
		{
			return;
		}
		this.initConfigComplete = true;
		this.galleryGridLayouter.RequestGridResize();
		this.categoryListContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		this.dlcFilter.ConfigButtons();
		this.dlcFilter.onDLCFilterChanged = new System.Action(this.RefreshGallery);
		this.PopulateCategories();
		this.PopulateGallery();
		this.SelectCategory("BUILDINGS");
		this.searchField.onValueChanged.RemoveAllListeners();
		this.searchField.onValueChanged.AddListener(delegate(string value)
		{
			this.RefreshGallery();
		});
		this.clearSearchButton.ClearOnClick();
		this.clearSearchButton.onClick += this.ClearSearch;
		MultiToggle multiToggle = this.doublesOnlyToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			int newState = (this.showFilterState + 1) % 3;
			this.ToggleDoublesOnly(newState);
		}));
	}

	// Token: 0x06006906 RID: 26886 RVA: 0x0027BC49 File Offset: 0x00279E49
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
			return false;
		};
		LockerNavigator.Instance.preventScreenPop.Add(this.preventScreenPopFn);
	}

	// Token: 0x06006907 RID: 26887 RVA: 0x0027BC78 File Offset: 0x00279E78
	private void UnregisterPreventScreenPop()
	{
		if (this.preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
			this.preventScreenPopFn = null;
		}
	}

	// Token: 0x06006908 RID: 26888 RVA: 0x0027BCA0 File Offset: 0x00279EA0
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.dlcFilter.ResetToDefault();
		this.ToggleDoublesOnly(0);
		this.ClearSearch();
		if (!this.initConfigComplete)
		{
			this.InitConfig();
		}
		this.RefreshUI();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshUI();
		});
		this.RegisterPreventScreenPop();
	}

	// Token: 0x06006909 RID: 26889 RVA: 0x0027BCFC File Offset: 0x00279EFC
	private void ClearSearch()
	{
		this.searchField.text = "";
		this.searchField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.KLEI_INVENTORY_SCREEN.SEARCH_PLACEHOLDER;
		this.RefreshGallery();
	}

	// Token: 0x0600690A RID: 26890 RVA: 0x0027BD33 File Offset: 0x00279F33
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x0600690B RID: 26891 RVA: 0x0027BD40 File Offset: 0x00279F40
	private void RefreshUI()
	{
		this.IS_ONLINE = ThreadedHttps<KleiAccount>.Instance.HasValidTicket();
		this.RefreshCategories();
		this.RefreshGallery();
		if (this.SelectedCategoryId.IsNullOrWhiteSpace())
		{
			this.SelectCategory("BUILDINGS");
		}
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x0600690C RID: 26892 RVA: 0x0027BD8D File Offset: 0x00279F8D
	private GameObject GetAvailableGridButton()
	{
		if (this.recycledGalleryGridButtons.Count == 0)
		{
			return Util.KInstantiateUI(this.gridItemPrefab, this.galleryGridContent.gameObject, true);
		}
		GameObject result = this.recycledGalleryGridButtons[0];
		this.recycledGalleryGridButtons.RemoveAt(0);
		return result;
	}

	// Token: 0x0600690D RID: 26893 RVA: 0x0027BDCC File Offset: 0x00279FCC
	private void RecycleGalleryGridButton(GameObject button)
	{
		button.GetComponent<MultiToggle>().onClick = null;
		this.recycledGalleryGridButtons.Add(button);
	}

	// Token: 0x0600690E RID: 26894 RVA: 0x0027BDE8 File Offset: 0x00279FE8
	public void PopulateCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
		}
		this.categoryToggles.Clear();
		foreach (KeyValuePair<string, List<string>> keyValuePair2 in InventoryOrganization.categoryIdToSubcategoryIdsMap)
		{
			string categoryId2;
			List<string> list;
			keyValuePair2.Deconstruct(out categoryId2, out list);
			string categoryId = categoryId2;
			GameObject gameObject = Util.KInstantiateUI(this.categoryRowPrefab, this.categoryListContent.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(InventoryOrganization.GetCategoryName(categoryId));
			component.GetReference<Image>("Icon").sprite = InventoryOrganization.categoryIdToIconMap[categoryId];
			MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = component2;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
			component2.onClick = delegate()
			{
				this.SelectCategory(categoryId);
			};
			this.categoryToggles.Add(categoryId, component2);
			this.SetCatogoryClickUISound(categoryId, component2);
		}
	}

	// Token: 0x0600690F RID: 26895 RVA: 0x0027BF68 File Offset: 0x0027A168
	public void PopulateGallery()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			this.RecycleGalleryGridButton(keyValuePair.Value.gameObject);
		}
		this.galleryGridButtons.Clear();
		this.galleryGridLayouter.ImmediateSizeGridToScreenResolution();
		HashSet<string> hashSet = new HashSet<string>();
		foreach (KeyValuePair<string, List<string>> keyValuePair2 in InventoryOrganization.subcategoryIdToPermitIdsMap)
		{
			foreach (string text in keyValuePair2.Value)
			{
				PermitResource permitResource = Db.Get().Permits.TryGet(text);
				if (permitResource != null)
				{
					this.AddItemToGallery(permitResource);
					hashSet.Add(text);
				}
			}
		}
		this.subcategories.Sort((KleiInventoryUISubcategory a, KleiInventoryUISubcategory b) => InventoryOrganization.subcategoryIdToPresentationDataMap[a.subcategoryID].sortKey.CompareTo(InventoryOrganization.subcategoryIdToPresentationDataMap[b.subcategoryID].sortKey));
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.gameObject.transform.SetAsLastSibling();
		}
		this.CollectSubcategoryGridLayouts();
		this.CloseSubcategory("UNCATEGORIZED");
	}

	// Token: 0x06006910 RID: 26896 RVA: 0x0027C108 File Offset: 0x0027A308
	private void CloseSubcategory(string subcategoryID)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryID);
		if (kleiInventoryUISubcategory != null)
		{
			kleiInventoryUISubcategory.ToggleOpen(false);
		}
	}

	// Token: 0x06006911 RID: 26897 RVA: 0x0027C14C File Offset: 0x0027A34C
	private void AddItemToSubcategoryUIContainer(GameObject itemButton, string subcategoryId)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryId);
		if (kleiInventoryUISubcategory == null)
		{
			kleiInventoryUISubcategory = Util.KInstantiateUI(this.subcategoryPrefab, this.galleryGridContent.gameObject, true).GetComponent<KleiInventoryUISubcategory>();
			kleiInventoryUISubcategory.subcategoryID = subcategoryId;
			this.subcategories.Add(kleiInventoryUISubcategory);
			kleiInventoryUISubcategory.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId), InventoryOrganization.subcategoryIdToPresentationDataMap[subcategoryId].icon);
		}
		itemButton.transform.SetParent(kleiInventoryUISubcategory.gridLayout.transform);
	}

	// Token: 0x06006912 RID: 26898 RVA: 0x0027C1F8 File Offset: 0x0027A3F8
	private void CollectSubcategoryGridLayouts()
	{
		this.galleryGridLayouter.OnSizeGridComplete = null;
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			this.galleryGridLayouter.targetGridLayouts.Add(kleiInventoryUISubcategory.gridLayout);
			GridLayouter gridLayouter = this.galleryGridLayouter;
			gridLayouter.OnSizeGridComplete = (System.Action)Delegate.Combine(gridLayouter.OnSizeGridComplete, new System.Action(kleiInventoryUISubcategory.RefreshDisplay));
		}
		this.galleryGridLayouter.RequestGridResize();
	}

	// Token: 0x06006913 RID: 26899 RVA: 0x0027C298 File Offset: 0x0027A498
	private void AddItemToGallery(PermitResource permit)
	{
		if (this.galleryGridButtons.ContainsKey(permit))
		{
			return;
		}
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		GameObject availableGridButton = this.GetAvailableGridButton();
		this.AddItemToSubcategoryUIContainer(availableGridButton, InventoryOrganization.GetPermitSubcategory(permit));
		HierarchyReferences component = availableGridButton.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Icon");
		LocText reference2 = component.GetReference<LocText>("OwnedCountLabel");
		Image reference3 = component.GetReference<Image>("IsUnownedOverlay");
		Image reference4 = component.GetReference<Image>("DlcBanner");
		MultiToggle component2 = availableGridButton.GetComponent<MultiToggle>();
		reference.sprite = permitPresentationInfo.sprite;
		if (permit.IsOwnableOnServer())
		{
			int ownedCount = PermitItems.GetOwnedCount(permit);
			reference2.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
			reference2.gameObject.SetActive(ownedCount > 0);
			reference3.gameObject.SetActive(ownedCount <= 0);
		}
		else
		{
			reference2.gameObject.SetActive(false);
			reference3.gameObject.SetActive(false);
		}
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			reference4.gameObject.SetActive(true);
			reference4.sprite = Assets.GetSprite(DlcManager.GetDlcBannerSprite(dlcIdFrom));
			reference4.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
		}
		else
		{
			reference4.gameObject.SetActive(false);
		}
		MultiToggle multiToggle = component2;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
		component2.onClick = delegate()
		{
			this.SelectItem(permit);
		};
		this.galleryGridButtons.Add(permit, component2);
		this.SetItemClickUISound(permit, component2);
		KleiItemsUI.ConfigureTooltipOn(availableGridButton, KleiItemsUI.GetTooltipStringFor(permit));
	}

	// Token: 0x06006914 RID: 26900 RVA: 0x0027C47B File Offset: 0x0027A67B
	public void SelectCategory(string categoryId)
	{
		if (InventoryOrganization.categoryIdToIsEmptyMap[categoryId])
		{
			return;
		}
		this.SelectedCategoryId = categoryId;
		this.galleryHeaderLabel.SetText(InventoryOrganization.GetCategoryName(categoryId));
		this.RefreshCategories();
		this.SelectDefaultCategoryItem();
	}

	// Token: 0x06006915 RID: 26901 RVA: 0x0027C4B0 File Offset: 0x0027A6B0
	private void SelectDefaultCategoryItem()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			if (InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(InventoryOrganization.GetPermitSubcategory(keyValuePair.Key)))
			{
				this.SelectItem(keyValuePair.Key);
				return;
			}
		}
		this.SelectItem(null);
	}

	// Token: 0x06006916 RID: 26902 RVA: 0x0027C538 File Offset: 0x0027A738
	public void SelectItem(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x06006917 RID: 26903 RVA: 0x0027C554 File Offset: 0x0027A754
	private void RefreshGallery()
	{
		string value = this.searchField.text.ToUpper();
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			PermitResource permitResource;
			MultiToggle multiToggle;
			keyValuePair.Deconstruct(out permitResource, out multiToggle);
			PermitResource permitResource2 = permitResource;
			MultiToggle multiToggle2 = multiToggle;
			string permitSubcategory = InventoryOrganization.GetPermitSubcategory(permitResource2);
			bool flag = permitSubcategory == "UNCATEGORIZED" || InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(permitSubcategory);
			flag = (flag && (permitResource2.Name.ToUpper().Contains(value) || permitResource2.Id.ToUpper().Contains(value) || permitResource2.Description.ToUpper().Contains(value)));
			multiToggle2.ChangeState((permitResource2 == this.SelectedPermit) ? 1 : 0);
			HierarchyReferences component = multiToggle2.gameObject.GetComponent<HierarchyReferences>();
			LocText reference = component.GetReference<LocText>("OwnedCountLabel");
			Image reference2 = component.GetReference<Image>("IsUnownedOverlay");
			if (permitResource2.IsOwnableOnServer())
			{
				int ownedCount = PermitItems.GetOwnedCount(permitResource2);
				reference.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
				reference.gameObject.SetActive(ownedCount > 0);
				reference2.gameObject.SetActive(ownedCount <= 0);
				if (this.showFilterState == 2 && ownedCount < 2)
				{
					flag = false;
				}
				else if (this.showFilterState == 1 && ownedCount == 0)
				{
					flag = false;
				}
			}
			else if (!permitResource2.IsUnlocked())
			{
				reference.gameObject.SetActive(false);
				reference2.gameObject.SetActive(true);
				if (this.showFilterState != 0)
				{
					flag = false;
				}
			}
			else
			{
				reference.gameObject.SetActive(false);
				reference2.gameObject.SetActive(false);
				if (this.showFilterState == 2)
				{
					flag = false;
				}
			}
			if (this.dlcFilter.SelectedDLCID != null && permitResource2.GetDlcIdFrom() != this.dlcFilter.SelectedDLCID)
			{
				flag = false;
			}
			if (multiToggle2.gameObject.activeSelf != flag)
			{
				multiToggle2.gameObject.SetActive(flag);
			}
		}
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.RefreshDisplay();
		}
	}

	// Token: 0x06006918 RID: 26904 RVA: 0x0027C7E0 File Offset: 0x0027A9E0
	private void RefreshCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			if (InventoryOrganization.categoryIdToIsEmptyMap[keyValuePair.Key])
			{
				keyValuePair.Value.ChangeState(2);
			}
			else
			{
				keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			}
		}
	}

	// Token: 0x06006919 RID: 26905 RVA: 0x0027C898 File Offset: 0x0027AA98
	private void RefreshDetails()
	{
		PermitResource selectedPermit = this.SelectedPermit;
		PermitPresentationInfo permitPresentationInfo = selectedPermit.GetPermitPresentationInfo();
		this.permitVis.ConfigureWith(selectedPermit);
		this.selectionDetailsScrollRect.rectTransform().anchorMin = new Vector2(0f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRect.rectTransform().sizeDelta = new Vector2(-24f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchoredPosition = Vector2.zero;
		this.selectionDetailsScrollRect.content.rectTransform().sizeDelta = new Vector2(0f, this.selectionDetailsScrollRect.content.rectTransform().sizeDelta.y);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMin = new Vector2(1f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRectScrollBarContainer.sizeDelta = new Vector2(24f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchoredPosition = Vector2.zero;
		this.selectionHeaderLabel.SetText(selectedPermit.Name);
		this.selectionNameLabel.SetText(selectedPermit.Name);
		this.selectionDescriptionLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(selectedPermit.Description));
		this.selectionDescriptionLabel.SetText(selectedPermit.Description);
		this.selectionFacadeForLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(permitPresentationInfo.facadeFor));
		this.selectionFacadeForLabel.SetText(permitPresentationInfo.facadeFor);
		string dlcIdFrom = selectedPermit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			this.selectionRarityDetailsLabel.gameObject.SetActive(false);
			this.selectionOwnedCount.gameObject.SetActive(false);
			this.selectionCollectionLabel.gameObject.SetActive(true);
			if (selectedPermit.Rarity == PermitRarity.UniversalLocked)
			{
				this.selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
				return;
			}
			this.selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
			return;
		}
		else
		{
			this.selectionCollectionLabel.gameObject.SetActive(false);
			string text = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", selectedPermit.Rarity.GetLocStringName());
			this.selectionRarityDetailsLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(text));
			this.selectionRarityDetailsLabel.SetText(text);
			this.selectionOwnedCount.gameObject.SetActive(true);
			if (!selectedPermit.IsOwnableOnServer())
			{
				this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE);
				return;
			}
			int ownedCount = PermitItems.GetOwnedCount(selectedPermit);
			if (ownedCount > 0)
			{
				this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT.Replace("{OwnedCount}", ownedCount.ToString()));
				return;
			}
			this.selectionOwnedCount.SetText(KleiItemsUI.WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
			return;
		}
	}

	// Token: 0x0600691A RID: 26906 RVA: 0x0027CBA8 File Offset: 0x0027ADA8
	private KleiInventoryScreen.PermitPrintabilityState GetPermitPrintabilityState(PermitResource permit)
	{
		if (!this.IS_ONLINE)
		{
			return KleiInventoryScreen.PermitPrintabilityState.UserOffline;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out num, out num2);
		if (num == 0UL)
		{
			if (permit.Rarity == PermitRarity.Universal || permit.Rarity == PermitRarity.UniversalLocked || permit.Rarity == PermitRarity.Loyalty || permit.Rarity == PermitRarity.Unknown)
			{
				return KleiInventoryScreen.PermitPrintabilityState.NotForSale;
			}
			return KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet;
		}
		else
		{
			if (PermitItems.GetOwnedCount(permit) > 0)
			{
				return KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned;
			}
			if (KleiItems.GetFilamentAmount() < num)
			{
				return KleiInventoryScreen.PermitPrintabilityState.TooExpensive;
			}
			return KleiInventoryScreen.PermitPrintabilityState.Printable;
		}
	}

	// Token: 0x0600691B RID: 26907 RVA: 0x0027CC1C File Offset: 0x0027AE1C
	private void RefreshBarterPanel()
	{
		this.barterBuyButton.ClearOnClick();
		this.barterSellButton.ClearOnClick();
		this.barterBuyButton.isInteractable = this.IS_ONLINE;
		this.barterSellButton.isInteractable = this.IS_ONLINE;
		HierarchyReferences component = this.barterBuyButton.GetComponent<HierarchyReferences>();
		HierarchyReferences component2 = this.barterSellButton.GetComponent<HierarchyReferences>();
		new Color(1f, 0.69411767f, 0.69411767f);
		Color color = new Color(0.6f, 0.9529412f, 0.5019608f);
		LocText reference = component.GetReference<LocText>("CostLabel");
		LocText reference2 = component2.GetReference<LocText>("CostLabel");
		this.barterPanelBG.color = (this.IS_ONLINE ? Util.ColorFromHex("575D6F") : Util.ColorFromHex("6F6F6F"));
		this.filamentWalletSection.gameObject.SetActive(this.IS_ONLINE);
		this.barterOfflineLabel.gameObject.SetActive(!this.IS_ONLINE);
		ulong filamentAmount = KleiItems.GetFilamentAmount();
		this.filamentWalletSection.GetComponent<ToolTip>().SetSimpleTooltip((filamentAmount > 1UL) ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_PLURAL_TOOLTIP, filamentAmount) : string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_TOOLTIP, filamentAmount));
		KleiInventoryScreen.PermitPrintabilityState permitPrintabilityState = this.GetPermitPrintabilityState(this.SelectedPermit);
		if (!this.IS_ONLINE)
		{
			component.GetReference<LocText>("CostLabel").SetText("");
			reference2.SetText("");
			reference2.color = Color.white;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out num, out num2);
		this.filamentWalletSection.GetComponentInChildren<LocText>().SetText(KleiItems.GetFilamentAmount().ToString());
		switch (permitPrintabilityState)
		{
		case KleiInventoryScreen.PermitPrintabilityState.Printable:
			this.barterBuyButton.isInteractable = true;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_ACTIVE, num.ToString()));
			reference.SetText("-" + num.ToString());
			this.barterBuyButton.onClick += delegate()
			{
				GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
				gameObject.rectTransform().sizeDelta = Vector2.zero;
				gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, true);
			};
			break;
		case KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_ALREADY_OWNED);
			reference.SetText("-" + num.ToString());
			break;
		case KleiInventoryScreen.PermitPrintabilityState.TooExpensive:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_CANT_AFFORD.text);
			reference.SetText("-" + num.ToString());
			break;
		case KleiInventoryScreen.PermitPrintabilityState.NotForSale:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE);
			reference.SetText("");
			break;
		case KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_BETA);
			reference.SetText("");
			break;
		}
		if (num2 == 0UL)
		{
			this.barterSellButton.isInteractable = false;
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNSELLABLE);
			reference2.SetText("");
			reference2.color = Color.white;
			return;
		}
		bool flag = PermitItems.GetOwnedCount(this.SelectedPermit) > 0;
		this.barterSellButton.isInteractable = flag;
		this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_SELL_ACTIVE, num2.ToString()) : UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_NONE_TO_SELL.text);
		if (flag)
		{
			reference2.color = color;
			reference2.SetText("+" + num2.ToString());
		}
		else
		{
			reference2.color = Color.white;
			reference2.SetText("+" + num2.ToString());
		}
		this.barterSellButton.onClick += delegate()
		{
			GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
			gameObject.rectTransform().sizeDelta = Vector2.zero;
			gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, false);
		};
	}

	// Token: 0x0600691C RID: 26908 RVA: 0x0027D074 File Offset: 0x0027B274
	private void SetCatogoryClickUISound(string categoryID, MultiToggle toggle)
	{
		if (!this.categoryToggles.ContainsKey(categoryID))
		{
			toggle.states[1].on_click_override_sound_path = "";
			toggle.states[0].on_click_override_sound_path = "";
			return;
		}
		toggle.states[1].on_click_override_sound_path = "General_Category_Click";
		toggle.states[0].on_click_override_sound_path = "General_Category_Click";
	}

	// Token: 0x0600691D RID: 26909 RVA: 0x0027D0E8 File Offset: 0x0027B2E8
	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		string facadeItemSoundName = KleiInventoryScreen.GetFacadeItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x0600691E RID: 26910 RVA: 0x0027D1D0 File Offset: 0x0027B3D0
	public static string GetFacadeItemSoundName(PermitResource permit)
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
		case PermitCategory.AtmoSuitHelmet:
			return "atmosuit_helmet";
		case PermitCategory.AtmoSuitBody:
			return "tops";
		case PermitCategory.AtmoSuitGloves:
			return "gloves";
		case PermitCategory.AtmoSuitBelt:
			return "belt";
		case PermitCategory.AtmoSuitShoes:
			return "shoes";
		}
		if (permit.Category == PermitCategory.Building)
		{
			BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef == null)
			{
				return "HUD";
			}
			string prefabID = buildingDef.PrefabID;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(prefabID);
			if (num <= 1943253450U)
			{
				if (num <= 1036100273U)
				{
					if (num <= 297556592U)
					{
						if (num <= 228062815U)
						{
							if (num != 38823703U)
							{
								if (num != 112031228U)
								{
									if (num != 228062815U)
									{
										goto IL_8E5;
									}
									if (!(prefabID == "LuxuryBed"))
									{
										goto IL_8E5;
									}
									string id = permit.Id;
									if (id == "LuxuryBed_boat")
									{
										return "elegantbed_boat";
									}
									if (!(id == "LuxuryBed_bouncy"))
									{
										return "elegantbed";
									}
									return "elegantbed_bouncy";
								}
								else
								{
									if (!(prefabID == "LogicGateDemultiplexer"))
									{
										goto IL_8E5;
									}
									goto IL_8A9;
								}
							}
							else
							{
								if (!(prefabID == "LogicGateXOR"))
								{
									goto IL_8E5;
								}
								goto IL_8A9;
							}
						}
						else if (num != 228549509U)
						{
							if (num != 296872528U)
							{
								if (num != 297556592U)
								{
									goto IL_8E5;
								}
								if (!(prefabID == "LogicRibbonBridge"))
								{
									goto IL_8E5;
								}
								goto IL_8A9;
							}
							else if (!(prefabID == "ItemPedestal"))
							{
								goto IL_8E5;
							}
						}
						else
						{
							if (!(prefabID == "WashSink"))
							{
								goto IL_8E5;
							}
							return "sink";
						}
					}
					else if (num <= 595816591U)
					{
						if (num != 301047391U)
						{
							if (num != 585850236U)
							{
								if (num != 595816591U)
								{
									goto IL_8E5;
								}
								if (!(prefabID == "FlowerVase"))
								{
									goto IL_8E5;
								}
								goto IL_811;
							}
							else if (!(prefabID == "GravitasPedestal"))
							{
								goto IL_8E5;
							}
						}
						else
						{
							if (!(prefabID == "WireRefined"))
							{
								goto IL_8E5;
							}
							goto IL_891;
						}
					}
					else if (num != 674245745U)
					{
						if (num != 781890915U)
						{
							if (num != 1036100273U)
							{
								goto IL_8E5;
							}
							if (!(prefabID == "WireRefinedBridgeHighWattage"))
							{
								goto IL_8E5;
							}
							goto IL_891;
						}
						else
						{
							if (!(prefabID == "LogicGateNOT"))
							{
								goto IL_8E5;
							}
							goto IL_8A9;
						}
					}
					else
					{
						if (!(prefabID == "CraftingTable"))
						{
							goto IL_8E5;
						}
						return "craftingstation";
					}
					return "sculpture";
				}
				if (num <= 1526604543U)
				{
					if (num <= 1232204109U)
					{
						if (num != 1038415088U)
						{
							if (num != 1089791339U)
							{
								if (num != 1232204109U)
								{
									goto IL_8E5;
								}
								if (!(prefabID == "WireBridge"))
								{
									goto IL_8E5;
								}
								goto IL_891;
							}
							else
							{
								if (!(prefabID == "Refrigerator"))
								{
									goto IL_8E5;
								}
								return "refrigerator";
							}
						}
						else
						{
							if (!(prefabID == "LogicGateFILTER"))
							{
								goto IL_8E5;
							}
							goto IL_8A9;
						}
					}
					else if (num != 1269853127U)
					{
						if (num != 1398532937U)
						{
							if (num != 1526604543U)
							{
								goto IL_8E5;
							}
							if (!(prefabID == "StorageLockerSmart"))
							{
								goto IL_8E5;
							}
							return "storagelockersmart";
						}
						else
						{
							if (!(prefabID == "LogicGateMultiplexer"))
							{
								goto IL_8E5;
							}
							goto IL_8A9;
						}
					}
					else
					{
						if (!(prefabID == "AdvancedResearchCenter"))
						{
							goto IL_8E5;
						}
						return "advancedresearchcenter";
					}
				}
				else if (num <= 1734850496U)
				{
					if (num != 1607642960U)
					{
						if (num != 1633134164U)
						{
							if (num != 1734850496U)
							{
								goto IL_8E5;
							}
							if (!(prefabID == "RockCrusher"))
							{
								goto IL_8E5;
							}
							return "rockrefinery";
						}
						else
						{
							if (!(prefabID == "CeilingLight"))
							{
								goto IL_8E5;
							}
							goto IL_855;
						}
					}
					else
					{
						if (!(prefabID == "FlushToilet"))
						{
							goto IL_8E5;
						}
						return "flushtoilate";
					}
				}
				else if (num <= 1908704479U)
				{
					if (num != 1815117387U)
					{
						if (num != 1908704479U)
						{
							goto IL_8E5;
						}
						if (!(prefabID == "LogicGateAND"))
						{
							goto IL_8E5;
						}
						goto IL_8A9;
					}
					else
					{
						if (!(prefabID == "LogicGateOR"))
						{
							goto IL_8E5;
						}
						goto IL_8A9;
					}
				}
				else if (num != 1938276536U)
				{
					if (num != 1943253450U)
					{
						goto IL_8E5;
					}
					if (!(prefabID == "WaterCooler"))
					{
						goto IL_8E5;
					}
					return "watercooler";
				}
				else
				{
					if (!(prefabID == "Wire"))
					{
						goto IL_8E5;
					}
					goto IL_891;
				}
			}
			else if (num <= 3132083755U)
			{
				if (num <= 2691468069U)
				{
					if (num <= 2076384603U)
					{
						if (num != 2028863301U)
						{
							if (num != 2041738741U)
							{
								if (num != 2076384603U)
								{
									goto IL_8E5;
								}
								if (!(prefabID == "GasReservoir"))
								{
									goto IL_8E5;
								}
								return "gasstorage";
							}
							else
							{
								if (!(prefabID == "CookingStation"))
								{
									goto IL_8E5;
								}
								return "grill";
							}
						}
						else if (!(prefabID == "FlowerVaseHanging"))
						{
							goto IL_8E5;
						}
					}
					else if (num != 2402859370U)
					{
						if (num != 2406622476U)
						{
							if (num != 2691468069U)
							{
								goto IL_8E5;
							}
							if (!(prefabID == "ResearchCenter"))
							{
								goto IL_8E5;
							}
							return "researchcenter";
						}
						else
						{
							if (!(prefabID == "WireBridgeHighWattage"))
							{
								goto IL_8E5;
							}
							goto IL_891;
						}
					}
					else
					{
						if (!(prefabID == "StorageLocker"))
						{
							goto IL_8E5;
						}
						return "storagelocker";
					}
				}
				else if (num <= 2818521706U)
				{
					if (num != 2701698824U)
					{
						if (num != 2722382738U)
						{
							if (num != 2818521706U)
							{
								goto IL_8E5;
							}
							if (!(prefabID == "GourmetCookingStation"))
							{
								goto IL_8E5;
							}
							return "gasrange";
						}
						else
						{
							if (!(prefabID == "PlanterBox"))
							{
								goto IL_8E5;
							}
							return "planterbox";
						}
					}
					else
					{
						if (!(prefabID == "ManualGenerator"))
						{
							goto IL_8E5;
						}
						return "manualgenerator";
					}
				}
				else if (num <= 3048425356U)
				{
					if (num != 2899744071U)
					{
						if (num != 3048425356U)
						{
							goto IL_8E5;
						}
						if (!(prefabID == "Bed"))
						{
							goto IL_8E5;
						}
						return "bed";
					}
					else
					{
						if (!(prefabID == "ExteriorWall"))
						{
							goto IL_8E5;
						}
						return "wall";
					}
				}
				else if (num != 3080524513U)
				{
					if (num != 3132083755U)
					{
						goto IL_8E5;
					}
					if (!(prefabID == "FlowerVaseWall"))
					{
						goto IL_8E5;
					}
				}
				else
				{
					if (!(prefabID == "MilkPress"))
					{
						goto IL_8E5;
					}
					return "pulverizer";
				}
			}
			else if (num <= 3562718686U)
			{
				if (num <= 3371266309U)
				{
					if (num != 3228988836U)
					{
						if (num != 3347778080U)
						{
							if (num != 3371266309U)
							{
								goto IL_8E5;
							}
							if (!(prefabID == "LogicRibbon"))
							{
								goto IL_8E5;
							}
							goto IL_8A9;
						}
						else
						{
							if (!(prefabID == "LogicGateBUFFER"))
							{
								goto IL_8E5;
							}
							goto IL_8A9;
						}
					}
					else
					{
						if (!(prefabID == "LogicWire"))
						{
							goto IL_8E5;
						}
						goto IL_8A9;
					}
				}
				else if (num != 3422134480U)
				{
					if (num != 3534553076U)
					{
						if (num != 3562718686U)
						{
							goto IL_8E5;
						}
						if (!(prefabID == "Headquarters"))
						{
							goto IL_8E5;
						}
						return "headquarters";
					}
					else
					{
						if (!(prefabID == "MassageTable"))
						{
							goto IL_8E5;
						}
						return "massagetable";
					}
				}
				else
				{
					if (!(prefabID == "MicrobeMusher"))
					{
						goto IL_8E5;
					}
					return "microbemusher";
				}
			}
			else if (num <= 3873680366U)
			{
				if (num != 3681463987U)
				{
					if (num != 3716494409U)
					{
						if (num != 3873680366U)
						{
							goto IL_8E5;
						}
						if (!(prefabID == "WireRefinedBridge"))
						{
							goto IL_8E5;
						}
						goto IL_891;
					}
					else
					{
						if (!(prefabID == "HighWattageWire"))
						{
							goto IL_8E5;
						}
						goto IL_891;
					}
				}
				else
				{
					if (!(prefabID == "FloorLamp"))
					{
						goto IL_8E5;
					}
					goto IL_855;
				}
			}
			else if (num <= 3958671086U)
			{
				if (num != 3903452895U)
				{
					if (num != 3958671086U)
					{
						goto IL_8E5;
					}
					if (!(prefabID == "FlowerVaseHangingFancy"))
					{
						goto IL_8E5;
					}
				}
				else
				{
					if (!(prefabID == "EggCracker"))
					{
						goto IL_8E5;
					}
					return "eggcracker";
				}
			}
			else if (num != 4217645425U)
			{
				if (num != 4243975822U)
				{
					goto IL_8E5;
				}
				if (!(prefabID == "WireRefinedHighWattage"))
				{
					goto IL_8E5;
				}
				goto IL_891;
			}
			else
			{
				if (!(prefabID == "LogicWireBridge"))
				{
					goto IL_8E5;
				}
				goto IL_8A9;
			}
			IL_811:
			return "flowervase";
			IL_855:
			return "ceilingLight";
			IL_891:
			return "wire";
			IL_8A9:
			return "logicwire";
		}
		IL_8E5:
		if (permit.Category == PermitCategory.Artwork)
		{
			BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef2 == null)
			{
				return "HUD";
			}
			if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|81_0<Sculpture>(buildingDef2))
			{
				string prefabID = buildingDef2.PrefabID;
				if (prefabID == "IceSculpture")
				{
					return "icesculpture";
				}
				if (!(prefabID == "WoodSculpture"))
				{
					return "sculpture";
				}
				return "woodsculpture";
			}
			else
			{
				if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|81_0<Painting>(buildingDef2))
				{
					return "painting";
				}
				if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|81_0<MonumentPart>(buildingDef2))
				{
					return "monument";
				}
			}
		}
		if (permit.Category == PermitCategory.JoyResponse && permit is BalloonArtistFacadeResource)
		{
			return "balloon";
		}
		return "HUD";
	}

	// Token: 0x0600691F RID: 26911 RVA: 0x0027DB5E File Offset: 0x0027BD5E
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06006928 RID: 26920 RVA: 0x0027DC7F File Offset: 0x0027BE7F
	[CompilerGenerated]
	internal static bool <GetFacadeItemSoundName>g__Has|81_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x04004818 RID: 18456
	[Header("Header")]
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004819 RID: 18457
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x0400481A RID: 18458
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x0400481B RID: 18459
	private Dictionary<string, MultiToggle> categoryToggles = new Dictionary<string, MultiToggle>();

	// Token: 0x0400481C RID: 18460
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x0400481D RID: 18461
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x0400481E RID: 18462
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x0400481F RID: 18463
	[SerializeField]
	private GameObject subcategoryPrefab;

	// Token: 0x04004820 RID: 18464
	[SerializeField]
	private GameObject itemDummyPrefab;

	// Token: 0x04004821 RID: 18465
	[Header("GalleryFilters")]
	[SerializeField]
	private KInputTextField searchField;

	// Token: 0x04004822 RID: 18466
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04004823 RID: 18467
	[SerializeField]
	private MultiToggle doublesOnlyToggle;

	// Token: 0x04004824 RID: 18468
	[SerializeField]
	private KleiInventoryDLCFilter dlcFilter;

	// Token: 0x04004825 RID: 18469
	public const int FILTER_SHOW_ALL = 0;

	// Token: 0x04004826 RID: 18470
	public const int FILTER_SHOW_OWNED_ONLY = 1;

	// Token: 0x04004827 RID: 18471
	public const int FILTER_SHOW_DOUBLES_ONLY = 2;

	// Token: 0x04004828 RID: 18472
	private int showFilterState;

	// Token: 0x04004829 RID: 18473
	private Func<bool> preventScreenPopFn;

	// Token: 0x0400482A RID: 18474
	[Header("BarterSection")]
	[SerializeField]
	private Image barterPanelBG;

	// Token: 0x0400482B RID: 18475
	[SerializeField]
	private KButton barterBuyButton;

	// Token: 0x0400482C RID: 18476
	[SerializeField]
	private KButton barterSellButton;

	// Token: 0x0400482D RID: 18477
	[SerializeField]
	private GameObject barterConfirmationScreenPrefab;

	// Token: 0x0400482E RID: 18478
	[SerializeField]
	private GameObject filamentWalletSection;

	// Token: 0x0400482F RID: 18479
	[SerializeField]
	private GameObject barterOfflineLabel;

	// Token: 0x04004830 RID: 18480
	private Dictionary<PermitResource, MultiToggle> galleryGridButtons = new Dictionary<PermitResource, MultiToggle>();

	// Token: 0x04004831 RID: 18481
	private List<KleiInventoryUISubcategory> subcategories = new List<KleiInventoryUISubcategory>();

	// Token: 0x04004832 RID: 18482
	private List<GameObject> recycledGalleryGridButtons = new List<GameObject>();

	// Token: 0x04004833 RID: 18483
	private GridLayouter galleryGridLayouter;

	// Token: 0x04004834 RID: 18484
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04004835 RID: 18485
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x04004836 RID: 18486
	[SerializeField]
	private KScrollRect selectionDetailsScrollRect;

	// Token: 0x04004837 RID: 18487
	[SerializeField]
	private RectTransform selectionDetailsScrollRectScrollBarContainer;

	// Token: 0x04004838 RID: 18488
	[SerializeField]
	private LocText selectionNameLabel;

	// Token: 0x04004839 RID: 18489
	[SerializeField]
	private LocText selectionDescriptionLabel;

	// Token: 0x0400483A RID: 18490
	[SerializeField]
	private LocText selectionFacadeForLabel;

	// Token: 0x0400483B RID: 18491
	[SerializeField]
	private LocText selectionCollectionLabel;

	// Token: 0x0400483C RID: 18492
	[SerializeField]
	private LocText selectionRarityDetailsLabel;

	// Token: 0x0400483D RID: 18493
	[SerializeField]
	private LocText selectionOwnedCount;

	// Token: 0x0400483F RID: 18495
	private bool IS_ONLINE;

	// Token: 0x04004840 RID: 18496
	private bool initConfigComplete;

	// Token: 0x02001F72 RID: 8050
	private enum PermitPrintabilityState
	{
		// Token: 0x040092E6 RID: 37606
		Printable,
		// Token: 0x040092E7 RID: 37607
		AlreadyOwned,
		// Token: 0x040092E8 RID: 37608
		TooExpensive,
		// Token: 0x040092E9 RID: 37609
		NotForSale,
		// Token: 0x040092EA RID: 37610
		NotForSaleYet,
		// Token: 0x040092EB RID: 37611
		UserOffline
	}

	// Token: 0x02001F73 RID: 8051
	private enum MultiToggleState
	{
		// Token: 0x040092ED RID: 37613
		Default,
		// Token: 0x040092EE RID: 37614
		Selected,
		// Token: 0x040092EF RID: 37615
		NonInteractable
	}
}
