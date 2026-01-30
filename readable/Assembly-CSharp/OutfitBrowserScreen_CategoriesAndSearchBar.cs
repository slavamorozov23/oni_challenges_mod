using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DCD RID: 3533
[Serializable]
public class OutfitBrowserScreen_CategoriesAndSearchBar
{
	// Token: 0x06006E78 RID: 28280 RVA: 0x0029D120 File Offset: 0x0029B320
	public void InitializeWith(OutfitBrowserScreen outfitBrowserScreen)
	{
		this.outfitBrowserScreen = outfitBrowserScreen;
		this.clothingOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.categoryRow, true));
		this.clothingOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.Clothing);
		};
		this.clothingOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_equipment");
		KleiItemsUI.ConfigureTooltipOn(this.clothingOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_CLOTHING);
		this.atmosuitOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.categoryRow, true));
		this.atmosuitOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.AtmoSuit);
		};
		this.atmosuitOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_atmosuits");
		KleiItemsUI.ConfigureTooltipOn(this.atmosuitOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_ATMO_SUITS);
		this.jetsuitOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.categoryRow, true));
		this.jetsuitOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.JetSuit);
		};
		this.jetsuitOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_jetsuits");
		KleiItemsUI.ConfigureTooltipOn(this.jetsuitOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_JET_SUITS);
		this.searchTextField.onValueChanged.AddListener(delegate(string newFilter)
		{
			outfitBrowserScreen.state.Filter = newFilter;
		});
		this.searchTextField.transform.SetAsLastSibling();
		outfitBrowserScreen.state.OnCurrentOutfitTypeChanged += delegate()
		{
			if (outfitBrowserScreen.Config.onlyShowOutfitType.IsSome())
			{
				this.clothingOutfitTypeButton.root.gameObject.SetActive(false);
				this.atmosuitOutfitTypeButton.root.gameObject.SetActive(false);
				this.jetsuitOutfitTypeButton.root.gameObject.SetActive(false);
				return;
			}
			this.clothingOutfitTypeButton.root.gameObject.SetActive(true);
			this.atmosuitOutfitTypeButton.root.gameObject.SetActive(true);
			this.jetsuitOutfitTypeButton.root.gameObject.SetActive(true);
			this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			this.jetsuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			switch (outfitBrowserScreen.state.CurrentOutfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
				this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
				return;
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
				return;
			case ClothingOutfitUtility.OutfitType.JetSuit:
				this.jetsuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
				return;
			}
			throw new NotImplementedException();
		};
	}

	// Token: 0x06006E79 RID: 28281 RVA: 0x0029D31B File Offset: 0x0029B51B
	public void SetOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.outfitBrowserScreen.state.CurrentOutfitType = outfitType;
	}

	// Token: 0x04004B80 RID: 19328
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton clothingOutfitTypeButton;

	// Token: 0x04004B81 RID: 19329
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton atmosuitOutfitTypeButton;

	// Token: 0x04004B82 RID: 19330
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton jetsuitOutfitTypeButton;

	// Token: 0x04004B83 RID: 19331
	[NonSerialized]
	public OutfitBrowserScreen outfitBrowserScreen;

	// Token: 0x04004B84 RID: 19332
	public KButton selectOutfitType_Prefab;

	// Token: 0x04004B85 RID: 19333
	public KInputTextField searchTextField;

	// Token: 0x04004B86 RID: 19334
	public GameObject categoryRow;

	// Token: 0x02002023 RID: 8227
	public enum SelectOutfitTypeButtonState
	{
		// Token: 0x040094FC RID: 38140
		Disabled,
		// Token: 0x040094FD RID: 38141
		Unselected,
		// Token: 0x040094FE RID: 38142
		Selected
	}

	// Token: 0x02002024 RID: 8228
	public readonly struct SelectOutfitTypeButton
	{
		// Token: 0x0600B862 RID: 47202 RVA: 0x003F51C0 File Offset: 0x003F33C0
		public SelectOutfitTypeButton(OutfitBrowserScreen outfitBrowserScreen, GameObject rootGameObject)
		{
			this.outfitBrowserScreen = outfitBrowserScreen;
			this.root = rootGameObject.GetComponent<RectTransform>();
			this.button = rootGameObject.GetComponent<KButton>();
			this.buttonImage = rootGameObject.GetComponent<KImage>();
			this.icon = this.root.GetChild(0).GetComponent<Image>();
			global::Debug.Assert(this.root != null);
			global::Debug.Assert(this.button != null);
			global::Debug.Assert(this.buttonImage != null);
			global::Debug.Assert(this.icon != null);
		}

		// Token: 0x0600B863 RID: 47203 RVA: 0x003F5254 File Offset: 0x003F3454
		public void SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState state)
		{
			switch (state)
			{
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Disabled:
				this.button.isInteractable = false;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.selectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x040094FF RID: 38143
		public readonly OutfitBrowserScreen outfitBrowserScreen;

		// Token: 0x04009500 RID: 38144
		public readonly RectTransform root;

		// Token: 0x04009501 RID: 38145
		public readonly KButton button;

		// Token: 0x04009502 RID: 38146
		public readonly KImage buttonImage;

		// Token: 0x04009503 RID: 38147
		public readonly Image icon;
	}
}
