using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DCC RID: 3532
public class OutfitBrowserScreen : KMonoBehaviour
{
	// Token: 0x06006E5F RID: 28255 RVA: 0x0029C32C File Offset: 0x0029A52C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
		this.gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.categoriesAndSearchBar.InitializeWith(this);
		this.pickOutfitButton.onClick += this.OnClickPickOutfit;
		this.editOutfitButton.onClick += delegate()
		{
			if (this.state.SelectedOutfitOpt.IsNone())
			{
				return;
			}
			new OutfitDesignerScreenConfig(this.state.SelectedOutfitOpt.Unwrap(), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen();
		};
		this.renameOutfitButton.onClick += delegate()
		{
			ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
			OutfitBrowserScreen.MakeRenamePopup(this.inputFieldPrefab, selectedOutfit, () => selectedOutfit.ReadName(), delegate(string new_name)
			{
				selectedOutfit.WriteName(new_name);
				this.Configure(this.Config.WithOutfit(selectedOutfit));
			});
		};
		this.deleteOutfitButton.onClick += delegate()
		{
			ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
			OutfitBrowserScreen.MakeDeletePopup(selectedOutfit, delegate
			{
				selectedOutfit.Delete();
				this.Configure(this.Config.WithOutfit(Option.None));
			});
		};
	}

	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x06006E60 RID: 28256 RVA: 0x0029C3FA File Offset: 0x0029A5FA
	// (set) Token: 0x06006E61 RID: 28257 RVA: 0x0029C402 File Offset: 0x0029A602
	public OutfitBrowserScreenConfig Config { get; private set; }

	// Token: 0x06006E62 RID: 28258 RVA: 0x0029C40C File Offset: 0x0029A60C
	protected override void OnCmpEnable()
	{
		if (this.isFirstDisplay)
		{
			this.isFirstDisplay = false;
			this.dioramaMinionOrMannequin.TrySpawn();
			this.FirstTimeSetup();
			this.postponeConfiguration = false;
			this.Configure(this.Config);
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshGallery();
			this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, ClothingOutfitUtility.OutfitType.Clothing, this.Config.minionPersonality);
		});
	}

	// Token: 0x06006E63 RID: 28259 RVA: 0x0029C464 File Offset: 0x0029A664
	private void FirstTimeSetup()
	{
		this.state.OnCurrentOutfitTypeChanged += delegate()
		{
			this.PopulateGallery();
			OutfitBrowserScreenConfig config = this.Config;
			Option<ClothingOutfitTarget> selectedOutfitOpt;
			if (!config.minionPersonality.HasValue)
			{
				config = this.Config;
				if (!config.selectedTarget.HasValue)
				{
					selectedOutfitOpt = ClothingOutfitTarget.GetRandom(this.state.CurrentOutfitType);
					goto IL_4F;
				}
			}
			selectedOutfitOpt = this.Config.selectedTarget;
			IL_4F:
			if (selectedOutfitOpt.IsSome() && selectedOutfitOpt.Unwrap().DoesExist())
			{
				this.state.SelectedOutfitOpt = selectedOutfitOpt;
				return;
			}
			this.state.SelectedOutfitOpt = Option.None;
		};
		this.state.OnSelectedOutfitOptChanged += delegate()
		{
			if (this.state.SelectedOutfitOpt.IsSome())
			{
				this.selectionHeaderLabel.text = this.state.SelectedOutfitOpt.Unwrap().ReadName();
			}
			else
			{
				this.selectionHeaderLabel.text = UI.OUTFIT_NAME.NONE;
			}
			this.dioramaMinionOrMannequin.current.SetOutfit(this.state.CurrentOutfitType, this.state.SelectedOutfitOpt);
			this.dioramaMinionOrMannequin.current.ReactToFullOutfitChange();
			this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, this.state.CurrentOutfitType, this.Config.minionPersonality);
			this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.state.CurrentOutfitType);
			this.pickOutfitButton.gameObject.SetActive(this.Config.isPickingOutfitForDupe);
			OutfitBrowserScreenConfig config = this.Config;
			if (config.minionPersonality.IsSome())
			{
				this.pickOutfitButton.isInteractable = (!this.state.SelectedOutfitOpt.IsSome() || !this.state.SelectedOutfitOpt.Unwrap().DoesContainLockedItems());
				GameObject gameObject = this.pickOutfitButton.gameObject;
				Option<string> tooltipText;
				if (!this.pickOutfitButton.isInteractable)
				{
					LocString tooltip_PICK_OUTFIT_ERROR_LOCKED = UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_PICK_OUTFIT_ERROR_LOCKED;
					string search = "{MinionName}";
					config = this.Config;
					tooltipText = Option.Some<string>(tooltip_PICK_OUTFIT_ERROR_LOCKED.Replace(search, config.GetMinionName()));
				}
				else
				{
					tooltipText = Option.None;
				}
				KleiItemsUI.ConfigureTooltipOn(gameObject, tooltipText);
			}
			this.editOutfitButton.isInteractable = this.state.SelectedOutfitOpt.IsSome();
			this.renameOutfitButton.isInteractable = (this.state.SelectedOutfitOpt.IsSome() && this.state.SelectedOutfitOpt.Unwrap().CanWriteName);
			KleiItemsUI.ConfigureTooltipOn(this.renameOutfitButton.gameObject, this.renameOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT_ERROR_READONLY);
			this.deleteOutfitButton.isInteractable = (this.state.SelectedOutfitOpt.IsSome() && this.state.SelectedOutfitOpt.Unwrap().CanDelete);
			KleiItemsUI.ConfigureTooltipOn(this.deleteOutfitButton.gameObject, this.deleteOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT_ERROR_READONLY);
			this.state.OnSelectedOutfitOptChanged += this.RefreshGallery;
			this.state.OnFilterChanged += this.RefreshGallery;
			this.state.OnCurrentOutfitTypeChanged += this.RefreshGallery;
			this.RefreshGallery();
		};
	}

	// Token: 0x06006E64 RID: 28260 RVA: 0x0029C494 File Offset: 0x0029A694
	public void Configure(OutfitBrowserScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.dioramaMinionOrMannequin.SetFrom(config.minionPersonality);
		if (config.targetMinionInstance.HasValue)
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.targetMinionInstance.Value.GetProperName());
		}
		else if (config.minionPersonality.HasValue)
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.minionPersonality.Value.Name);
		}
		else
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.GALLERY_HEADER;
		}
		this.state.CurrentOutfitType = config.onlyShowOutfitType.UnwrapOr(this.lastShownOutfitType.UnwrapOr(ClothingOutfitUtility.OutfitType.Clothing, null), null);
		if (base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006E65 RID: 28261 RVA: 0x0029C598 File Offset: 0x0029A798
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x06006E66 RID: 28262 RVA: 0x0029C5B0 File Offset: 0x0029A7B0
	private void PopulateGallery()
	{
		this.outfits.Clear();
		this.galleryGridItemPool.ReturnAll();
		this.RefreshGalleryFn = null;
		if (this.Config.isPickingOutfitForDupe)
		{
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(Option.None);
		}
		OutfitBrowserScreenConfig config = this.Config;
		if (config.targetMinionInstance.HasValue)
		{
			ClothingOutfitUtility.OutfitType currentOutfitType = this.state.CurrentOutfitType;
			config = this.Config;
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(ClothingOutfitTarget.FromMinion(currentOutfitType, config.targetMinionInstance.Value));
		}
		foreach (ClothingOutfitTarget value in from outfit in ClothingOutfitTarget.GetAllTemplates()
		where outfit.OutfitType == this.state.CurrentOutfitType
		select outfit)
		{
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(value);
		}
		this.addButtonGridItem.transform.SetAsLastSibling();
		this.addButtonGridItem.SetActive(true);
		this.addButtonGridItem.GetComponent<MultiToggle>().onClick = delegate()
		{
			new OutfitDesignerScreenConfig(ClothingOutfitTarget.ForNewTemplateOutfit(this.state.CurrentOutfitType), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen();
		};
		this.RefreshGallery();
	}

	// Token: 0x06006E67 RID: 28263 RVA: 0x0029C6D0 File Offset: 0x0029A8D0
	private void OnOutfitDesignerWritesToOutfitTarget(ClothingOutfitTarget outfit)
	{
		this.Configure(this.Config.WithOutfit(outfit));
	}

	// Token: 0x06006E68 RID: 28264 RVA: 0x0029C6F7 File Offset: 0x0029A8F7
	private void Update()
	{
		this.gridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06006E69 RID: 28265 RVA: 0x0029C704 File Offset: 0x0029A904
	private void OnClickPickOutfit()
	{
		OutfitBrowserScreenConfig config = this.Config;
		if (config.targetMinionInstance.IsSome())
		{
			config = this.Config;
			WearableAccessorizer component = config.targetMinionInstance.Unwrap().GetComponent<WearableAccessorizer>();
			ClothingOutfitUtility.OutfitType currentOutfitType = this.state.CurrentOutfitType;
			Option<ClothingOutfitTarget> selectedOutfitOpt = this.state.SelectedOutfitOpt;
			component.ApplyClothingItems(currentOutfitType, selectedOutfitOpt.AndThen<IEnumerable<ClothingItemResource>>((ClothingOutfitTarget outfit) => outfit.ReadItemValues()).UnwrapOr(ClothingOutfitTarget.NO_ITEM_VALUES, null));
		}
		else
		{
			config = this.Config;
			if (config.minionPersonality.IsSome())
			{
				config = this.Config;
				Personality value = config.minionPersonality.Value;
				ClothingOutfitUtility.OutfitType currentOutfitType2 = this.state.CurrentOutfitType;
				Option<ClothingOutfitTarget> selectedOutfitOpt = this.state.SelectedOutfitOpt;
				value.SetSelectedTemplateOutfitId(currentOutfitType2, selectedOutfitOpt.AndThen<string>((ClothingOutfitTarget o) => o.OutfitId));
			}
		}
		LockerNavigator.Instance.PopScreen();
	}

	// Token: 0x06006E6A RID: 28266 RVA: 0x0029C808 File Offset: 0x0029AA08
	public static void MakeDeletePopup(ClothingOutfitTarget sourceTarget, System.Action deleteFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.HEADER.Replace("{OutfitName}", sourceTarget.ReadName())).AddPlainText(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BODY.Replace("{OutfitName}", sourceTarget.ReadName()));
			string text = UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_YES_DELETE;
			Action<InfoDialogScreen> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(InfoDialogScreen d)
				{
					deleteFn();
					d.Deactivate();
				});
			}
			infoDialogScreen.AddOption(text, action, true).AddOption(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_DONT_DELETE, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
		});
	}

	// Token: 0x06006E6B RID: 28267 RVA: 0x0029C840 File Offset: 0x0029AA40
	public static void MakeRenamePopup(KInputTextField inputFieldPrefab, ClothingOutfitTarget sourceTarget, Func<string> readName, Action<string> writeName)
	{
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.RENAME_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeRenamePopup>g__Refresh|1));
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
				writeName(inputField.text);
				dialog.Deactivate();
			};
			base.<MakeRenamePopup>g__Refresh|1(readName());
		});
	}

	// Token: 0x06006E6C RID: 28268 RVA: 0x0029C888 File Offset: 0x0029AA88
	private void SetButtonClickUISound(Option<ClothingOutfitTarget> target, MultiToggle toggle)
	{
		if (!target.HasValue)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		bool flag = !target.Value.DoesContainLockedItems();
		toggle.states[1].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x06006E6D RID: 28269 RVA: 0x0029C99A File Offset: 0x0029AB9A
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06006E75 RID: 28277 RVA: 0x0029CE94 File Offset: 0x0029B094
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIconForTarget|35_0(Option<ClothingOutfitTarget> target)
	{
		GameObject spawn = this.galleryGridItemPool.Borrow();
		GameObject gameObject = spawn.transform.GetChild(1).gameObject;
		GameObject isUnownedOverlayGO = spawn.transform.GetChild(2).gameObject;
		GameObject dlcBannerGO = spawn.transform.GetChild(3).gameObject;
		gameObject.SetActive(true);
		bool shouldShowOutfitWithDefaultItems = target.IsNone() || this.state.CurrentOutfitType == ClothingOutfitUtility.OutfitType.AtmoSuit || this.state.CurrentOutfitType == ClothingOutfitUtility.OutfitType.JetSuit;
		UIMannequin componentInChildren = gameObject.GetComponentInChildren<UIMannequin>();
		this.dioramaMinionOrMannequin.mannequin.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
		componentInChildren.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
		componentInChildren.personalityToUseForDefaultClothing = this.Config.minionPersonality;
		componentInChildren.SetOutfit(this.state.CurrentOutfitType, target);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		float x;
		float num;
		float num2;
		float y;
		switch (this.state.CurrentOutfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			x = 8f;
			num = 8f;
			num2 = 8f;
			y = 8f;
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			throw new NotSupportedException();
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			x = 24f;
			num = 16f;
			num2 = 32f;
			y = 8f;
			break;
		case ClothingOutfitUtility.OutfitType.JetSuit:
			x = 32f;
			num = 24f;
			num2 = 32f;
			y = 8f;
			break;
		default:
			throw new NotImplementedException();
		}
		component.offsetMin = new Vector2(x, y);
		component.offsetMax = new Vector2(-num, -num2);
		MultiToggle button = spawn.GetComponent<MultiToggle>();
		MultiToggle button2 = button;
		button2.onEnter = (System.Action)Delegate.Combine(button2.onEnter, new System.Action(this.OnMouseOverToggle));
		button.onClick = delegate()
		{
			this.state.SelectedOutfitOpt = target;
		};
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			button.ChangeState((target == this.state.SelectedOutfitOpt) ? 1 : 0);
			if (string.IsNullOrWhiteSpace(this.state.Filter) || target.IsNone())
			{
				spawn.SetActive(true);
			}
			else
			{
				spawn.SetActive(target.Unwrap().ReadName().ToLower().Contains(this.state.Filter.ToLower()));
			}
			if (!target.HasValue)
			{
				KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(KleiItemsUI.GetNoneOutfitName(this.state.CurrentOutfitType)));
				isUnownedOverlayGO.SetActive(false);
			}
			else
			{
				KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(target.Value.ReadName()));
				isUnownedOverlayGO.SetActive(target.Value.DoesContainLockedItems());
			}
			if (target.IsSome())
			{
				ClothingOutfitTarget.Implementation impl = target.Unwrap().impl;
				if (impl is ClothingOutfitTarget.DatabaseAuthoredTemplate)
				{
					ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate = (ClothingOutfitTarget.DatabaseAuthoredTemplate)impl;
					string dlcIdFrom = databaseAuthoredTemplate.resource.GetDlcIdFrom();
					if (DlcManager.IsDlcId(dlcIdFrom))
					{
						Image component2 = dlcBannerGO.GetComponent<Image>();
						component2.sprite = Assets.GetSprite(DlcManager.GetDlcBannerSprite(dlcIdFrom));
						component2.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
						dlcBannerGO.SetActive(true);
						return;
					}
					dlcBannerGO.SetActive(false);
					return;
				}
			}
			dlcBannerGO.SetActive(false);
		}));
		this.SetButtonClickUISound(target, button);
	}

	// Token: 0x04004B67 RID: 19303
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04004B68 RID: 19304
	[SerializeField]
	private OutfitBrowserScreen_CategoriesAndSearchBar categoriesAndSearchBar;

	// Token: 0x04004B69 RID: 19305
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04004B6A RID: 19306
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04004B6B RID: 19307
	[SerializeField]
	private GameObject addButtonGridItem;

	// Token: 0x04004B6C RID: 19308
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04004B6D RID: 19309
	private GridLayouter gridLayouter;

	// Token: 0x04004B6E RID: 19310
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04004B6F RID: 19311
	[SerializeField]
	private UIMinionOrMannequin dioramaMinionOrMannequin;

	// Token: 0x04004B70 RID: 19312
	[SerializeField]
	private Image dioramaBG;

	// Token: 0x04004B71 RID: 19313
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04004B72 RID: 19314
	[SerializeField]
	private KButton pickOutfitButton;

	// Token: 0x04004B73 RID: 19315
	[SerializeField]
	private KButton editOutfitButton;

	// Token: 0x04004B74 RID: 19316
	[SerializeField]
	private KButton renameOutfitButton;

	// Token: 0x04004B75 RID: 19317
	[SerializeField]
	private KButton deleteOutfitButton;

	// Token: 0x04004B76 RID: 19318
	[Header("Misc")]
	[SerializeField]
	private KInputTextField inputFieldPrefab;

	// Token: 0x04004B77 RID: 19319
	[SerializeField]
	public ColorStyleSetting selectedCategoryStyle;

	// Token: 0x04004B78 RID: 19320
	[SerializeField]
	public ColorStyleSetting notSelectedCategoryStyle;

	// Token: 0x04004B79 RID: 19321
	public OutfitBrowserScreen.State state = new OutfitBrowserScreen.State();

	// Token: 0x04004B7A RID: 19322
	public Option<ClothingOutfitUtility.OutfitType> lastShownOutfitType = Option.None;

	// Token: 0x04004B7B RID: 19323
	private Dictionary<string, MultiToggle> outfits = new Dictionary<string, MultiToggle>();

	// Token: 0x04004B7D RID: 19325
	private bool postponeConfiguration = true;

	// Token: 0x04004B7E RID: 19326
	private bool isFirstDisplay = true;

	// Token: 0x04004B7F RID: 19327
	private System.Action RefreshGalleryFn;

	// Token: 0x0200201A RID: 8218
	public class State
	{
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x0600B83F RID: 47167 RVA: 0x003F49FC File Offset: 0x003F2BFC
		// (remove) Token: 0x0600B840 RID: 47168 RVA: 0x003F4A34 File Offset: 0x003F2C34
		public event System.Action OnSelectedOutfitOptChanged;

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x0600B841 RID: 47169 RVA: 0x003F4A69 File Offset: 0x003F2C69
		// (set) Token: 0x0600B842 RID: 47170 RVA: 0x003F4A71 File Offset: 0x003F2C71
		public Option<ClothingOutfitTarget> SelectedOutfitOpt
		{
			get
			{
				return this.m_selectedOutfitOpt;
			}
			set
			{
				this.m_selectedOutfitOpt = value;
				if (this.OnSelectedOutfitOptChanged != null)
				{
					this.OnSelectedOutfitOptChanged();
				}
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x0600B843 RID: 47171 RVA: 0x003F4A90 File Offset: 0x003F2C90
		// (remove) Token: 0x0600B844 RID: 47172 RVA: 0x003F4AC8 File Offset: 0x003F2CC8
		public event System.Action OnCurrentOutfitTypeChanged;

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x0600B845 RID: 47173 RVA: 0x003F4AFD File Offset: 0x003F2CFD
		// (set) Token: 0x0600B846 RID: 47174 RVA: 0x003F4B05 File Offset: 0x003F2D05
		public ClothingOutfitUtility.OutfitType CurrentOutfitType
		{
			get
			{
				return this.m_currentOutfitType;
			}
			set
			{
				this.m_currentOutfitType = value;
				if (this.OnCurrentOutfitTypeChanged != null)
				{
					this.OnCurrentOutfitTypeChanged();
				}
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x0600B847 RID: 47175 RVA: 0x003F4B24 File Offset: 0x003F2D24
		// (remove) Token: 0x0600B848 RID: 47176 RVA: 0x003F4B5C File Offset: 0x003F2D5C
		public event System.Action OnFilterChanged;

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x0600B849 RID: 47177 RVA: 0x003F4B91 File Offset: 0x003F2D91
		// (set) Token: 0x0600B84A RID: 47178 RVA: 0x003F4B99 File Offset: 0x003F2D99
		public string Filter
		{
			get
			{
				return this.m_filter;
			}
			set
			{
				this.m_filter = value;
				if (this.OnFilterChanged != null)
				{
					this.OnFilterChanged();
				}
			}
		}

		// Token: 0x040094D5 RID: 38101
		private Option<ClothingOutfitTarget> m_selectedOutfitOpt;

		// Token: 0x040094D6 RID: 38102
		private ClothingOutfitUtility.OutfitType m_currentOutfitType;

		// Token: 0x040094D7 RID: 38103
		private string m_filter;
	}

	// Token: 0x0200201B RID: 8219
	private enum MultiToggleState
	{
		// Token: 0x040094DC RID: 38108
		Default,
		// Token: 0x040094DD RID: 38109
		Selected,
		// Token: 0x040094DE RID: 38110
		NonInteractable
	}
}
