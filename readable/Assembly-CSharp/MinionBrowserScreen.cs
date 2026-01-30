using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DAD RID: 3501
public class MinionBrowserScreen : KMonoBehaviour
{
	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x06006CF3 RID: 27891 RVA: 0x0029333A File Offset: 0x0029153A
	public MinionBrowserScreen.CyclerUI Cycler
	{
		get
		{
			return this.cycler;
		}
	}

	// Token: 0x06006CF4 RID: 27892 RVA: 0x00293344 File Offset: 0x00291544
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
	}

	// Token: 0x06006CF5 RID: 27893 RVA: 0x002933AC File Offset: 0x002915AC
	protected override void OnCmpEnable()
	{
		if (this.isFirstDisplay)
		{
			this.isFirstDisplay = false;
			this.PopulateGallery();
			this.RefreshPreview();
			this.cycler.Initialize(this.CreateCycleOptions());
			this.editButton.onClick += delegate()
			{
				if (this.OnEditClickedFn != null)
				{
					this.OnEditClickedFn();
				}
			};
			this.changeOutfitButton.onClick += this.OnClickChangeOutfit;
		}
		else
		{
			this.RefreshGallery();
			this.RefreshPreview();
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshGallery();
			this.RefreshPreview();
		});
	}

	// Token: 0x06006CF6 RID: 27894 RVA: 0x00293438 File Offset: 0x00291638
	private void Update()
	{
		this.gridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06006CF7 RID: 27895 RVA: 0x00293448 File Offset: 0x00291648
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		if (this.Config.isValid)
		{
			this.Configure(this.Config);
			return;
		}
		this.Configure(MinionBrowserScreenConfig.Personalities(default(Option<Personality>)));
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x06006CF8 RID: 27896 RVA: 0x0029348A File Offset: 0x0029168A
	// (set) Token: 0x06006CF9 RID: 27897 RVA: 0x00293492 File Offset: 0x00291692
	public MinionBrowserScreenConfig Config { get; private set; }

	// Token: 0x06006CFA RID: 27898 RVA: 0x0029349B File Offset: 0x0029169B
	public void Configure(MinionBrowserScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.PopulateGallery();
		this.RefreshPreview();
	}

	// Token: 0x06006CFB RID: 27899 RVA: 0x002934B9 File Offset: 0x002916B9
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x06006CFC RID: 27900 RVA: 0x002934D0 File Offset: 0x002916D0
	public void PopulateGallery()
	{
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		foreach (MinionBrowserScreen.GridItem item in this.Config.items)
		{
			this.<PopulateGallery>g__AddGridIcon|34_0(item);
		}
		this.RefreshGallery();
		this.SelectMinion(this.Config.defaultSelectedItem.Unwrap());
	}

	// Token: 0x06006CFD RID: 27901 RVA: 0x00293534 File Offset: 0x00291734
	private void SelectMinion(MinionBrowserScreen.GridItem item)
	{
		this.selectedGridItem = item;
		this.RefreshGallery();
		this.RefreshPreview();
		this.UIMinion.GetMinionVoice().PlaySoundUI("voice_land");
	}

	// Token: 0x06006CFE RID: 27902 RVA: 0x0029356C File Offset: 0x0029176C
	public void RefreshPreview()
	{
		this.UIMinion.SetMinion(this.selectedGridItem.GetPersonality());
		this.UIMinion.ReactToPersonalityChange();
		this.detailsHeaderText.SetText(this.selectedGridItem.GetName());
		this.detailHeaderIcon.sprite = this.selectedGridItem.GetIcon();
		this.RefreshOutfitDescription();
		this.RefreshPreviewButtonsInteractable();
		this.SetDioramaBG();
	}

	// Token: 0x06006CFF RID: 27903 RVA: 0x002935D8 File Offset: 0x002917D8
	private void RefreshOutfitDescription()
	{
		if (this.RefreshOutfitDescriptionFn != null)
		{
			this.RefreshOutfitDescriptionFn();
		}
	}

	// Token: 0x06006D00 RID: 27904 RVA: 0x002935F0 File Offset: 0x002917F0
	private void OnClickChangeOutfit()
	{
		if (this.selectedOutfitType.IsNone())
		{
			return;
		}
		OutfitBrowserScreenConfig.Minion(this.selectedOutfitType.Unwrap(), this.selectedGridItem).WithOutfit(this.selectedOutfit).ApplyAndOpenScreen();
	}

	// Token: 0x06006D01 RID: 27905 RVA: 0x00293638 File Offset: 0x00291838
	private void RefreshPreviewButtonsInteractable()
	{
		this.editButton.isInteractable = true;
		if (this.currentOutfitType == ClothingOutfitUtility.OutfitType.JoyResponse)
		{
			Option<string> joyResponseEditError = this.GetJoyResponseEditError();
			if (joyResponseEditError.IsSome())
			{
				this.editButton.isInteractable = false;
				this.editButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(joyResponseEditError.Unwrap());
				return;
			}
			this.editButton.isInteractable = true;
			this.editButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}
	}

	// Token: 0x06006D02 RID: 27906 RVA: 0x002936B4 File Offset: 0x002918B4
	private void SetDioramaBG()
	{
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.currentOutfitType);
	}

	// Token: 0x06006D03 RID: 27907 RVA: 0x002936CC File Offset: 0x002918CC
	private Option<string> GetJoyResponseEditError()
	{
		string joyTrait = this.selectedGridItem.GetPersonality().joyTrait;
		if (!(joyTrait == "BalloonArtist"))
		{
			return Option.Some<string>(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_NO_FACADES_FOR_JOY_TRAIT.Replace("{JoyResponseType}", Db.Get().traits.Get(joyTrait).Name));
		}
		return Option.None;
	}

	// Token: 0x06006D04 RID: 27908 RVA: 0x0029372C File Offset: 0x0029192C
	public void SetEditingOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentOutfitType = outfitType;
		this.cycler.SetLabel(outfitType.GetName());
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_JOY_RESPONSE;
			this.changeOutfitButton.gameObject.SetActive(false);
			break;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		case ClothingOutfitUtility.OutfitType.JetSuit:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_JET_SUIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		default:
			throw new NotImplementedException();
		}
		this.RefreshPreviewButtonsInteractable();
		this.OnEditClickedFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
			case ClothingOutfitUtility.OutfitType.JetSuit:
				OutfitDesignerScreenConfig.Minion(this.selectedOutfit.IsSome() ? this.selectedOutfit.Unwrap() : ClothingOutfitTarget.ForNewTemplateOutfit(outfitType), this.selectedGridItem).ApplyAndOpenScreen();
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				JoyResponseScreenConfig joyResponseScreenConfig = JoyResponseScreenConfig.From(this.selectedGridItem);
				joyResponseScreenConfig = joyResponseScreenConfig.WithInitialSelection(this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().AndThen<BalloonArtistFacadeResource>((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id)));
				joyResponseScreenConfig.ApplyAndOpenScreen();
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescriptionFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
			case ClothingOutfitUtility.OutfitType.JetSuit:
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(outfitType);
				this.UIMinion.SetOutfit(outfitType, this.selectedOutfit);
				this.outfitDescriptionPanel.Refresh(this.selectedOutfit, outfitType, this.selectedGridItem.GetPersonality());
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType.Clothing);
				this.UIMinion.SetOutfit(ClothingOutfitUtility.OutfitType.Clothing, this.selectedOutfit);
				string text = this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().UnwrapOr(null, null);
				this.outfitDescriptionPanel.Refresh((text != null) ? Db.Get().Permits.Get(text) : null, outfitType, this.selectedGridItem.GetPersonality());
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescription();
		this.RefreshPreview();
	}

	// Token: 0x06006D05 RID: 27909 RVA: 0x00293870 File Offset: 0x00291A70
	private MinionBrowserScreen.CyclerUI.OnSelectedFn[] CreateCycleOptions()
	{
		MinionBrowserScreen.CyclerUI.OnSelectedFn[] array = new MinionBrowserScreen.CyclerUI.OnSelectedFn[4];
		for (int i = 0; i < 4; i++)
		{
			ClothingOutfitUtility.OutfitType outfitType = (ClothingOutfitUtility.OutfitType)i;
			array[i] = delegate()
			{
				this.selectedOutfitType = Option.Some<ClothingOutfitUtility.OutfitType>(outfitType);
				this.SetEditingOutfitType(outfitType);
			};
		}
		return array;
	}

	// Token: 0x06006D06 RID: 27910 RVA: 0x002938B4 File Offset: 0x00291AB4
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06006D0A RID: 27914 RVA: 0x00293900 File Offset: 0x00291B00
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIcon|34_0(MinionBrowserScreen.GridItem item)
	{
		GameObject gameObject = this.galleryGridItemPool.Borrow();
		gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = item.GetIcon();
		gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(item.GetName());
		string requiredDlcId = item.GetPersonality().requiredDlcId;
		ToolTip component = gameObject.GetComponent<ToolTip>();
		Image component2 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
		if (DlcManager.IsDlcId(requiredDlcId))
		{
			component2.gameObject.SetActive(true);
			component2.sprite = Assets.GetSprite(DlcManager.GetDlcBannerSprite(requiredDlcId));
			component2.color = DlcManager.GetDlcBannerColor(requiredDlcId);
			component.SetSimpleTooltip(string.Format(UI.MINION_BROWSER_SCREEN.TOOLTIP_FROM_DLC, DlcManager.GetDlcTitle(requiredDlcId)));
		}
		else
		{
			component2.gameObject.SetActive(false);
			component.ClearMultiStringTooltip();
		}
		MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
		MultiToggle toggle3 = toggle;
		toggle3.onEnter = (System.Action)Delegate.Combine(toggle3.onEnter, new System.Action(this.OnMouseOverToggle));
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			this.SelectMinion(item);
		}));
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			toggle.ChangeState((item == this.selectedGridItem) ? 1 : 0);
		}));
	}

	// Token: 0x04004A7E RID: 19070
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04004A7F RID: 19071
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04004A80 RID: 19072
	private GridLayouter gridLayouter;

	// Token: 0x04004A81 RID: 19073
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x04004A82 RID: 19074
	[SerializeField]
	private UIMinion UIMinion;

	// Token: 0x04004A83 RID: 19075
	[SerializeField]
	private LocText detailsHeaderText;

	// Token: 0x04004A84 RID: 19076
	[SerializeField]
	private Image detailHeaderIcon;

	// Token: 0x04004A85 RID: 19077
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04004A86 RID: 19078
	[SerializeField]
	private MinionBrowserScreen.CyclerUI cycler;

	// Token: 0x04004A87 RID: 19079
	[SerializeField]
	private KButton editButton;

	// Token: 0x04004A88 RID: 19080
	[SerializeField]
	private LocText editButtonText;

	// Token: 0x04004A89 RID: 19081
	[SerializeField]
	private KButton changeOutfitButton;

	// Token: 0x04004A8A RID: 19082
	private Option<ClothingOutfitUtility.OutfitType> selectedOutfitType;

	// Token: 0x04004A8B RID: 19083
	private Option<ClothingOutfitTarget> selectedOutfit;

	// Token: 0x04004A8C RID: 19084
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x04004A8D RID: 19085
	private MinionBrowserScreen.GridItem selectedGridItem;

	// Token: 0x04004A8E RID: 19086
	private System.Action OnEditClickedFn;

	// Token: 0x04004A8F RID: 19087
	private bool isFirstDisplay = true;

	// Token: 0x04004A91 RID: 19089
	private bool postponeConfiguration = true;

	// Token: 0x04004A92 RID: 19090
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04004A93 RID: 19091
	private System.Action RefreshGalleryFn;

	// Token: 0x04004A94 RID: 19092
	private System.Action RefreshOutfitDescriptionFn;

	// Token: 0x04004A95 RID: 19093
	private ClothingOutfitUtility.OutfitType currentOutfitType;

	// Token: 0x02001FEE RID: 8174
	private enum MultiToggleState
	{
		// Token: 0x04009430 RID: 37936
		Default,
		// Token: 0x04009431 RID: 37937
		Selected,
		// Token: 0x04009432 RID: 37938
		NonInteractable
	}

	// Token: 0x02001FEF RID: 8175
	[Serializable]
	public class CyclerUI
	{
		// Token: 0x0600B7BF RID: 47039 RVA: 0x003F3304 File Offset: 0x003F1504
		public void Initialize(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			this.cyclePrevButton.onClick += this.CyclePrev;
			this.cycleNextButton.onClick += this.CycleNext;
			this.SetCycleOptions(cycleOptions);
		}

		// Token: 0x0600B7C0 RID: 47040 RVA: 0x003F333B File Offset: 0x003F153B
		public void SetCycleOptions(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			DebugUtil.Assert(cycleOptions != null);
			DebugUtil.Assert(cycleOptions.Length != 0);
			this.cycleOptions = cycleOptions;
			this.GoTo(0);
		}

		// Token: 0x0600B7C1 RID: 47041 RVA: 0x003F3360 File Offset: 0x003F1560
		public void GoTo(int wrappingIndex)
		{
			if (this.cycleOptions == null || this.cycleOptions.Length == 0)
			{
				return;
			}
			while (wrappingIndex < 0)
			{
				wrappingIndex += this.cycleOptions.Length;
			}
			while (wrappingIndex >= this.cycleOptions.Length)
			{
				wrappingIndex -= this.cycleOptions.Length;
			}
			this.selectedIndex = wrappingIndex;
			this.cycleOptions[this.selectedIndex]();
		}

		// Token: 0x0600B7C2 RID: 47042 RVA: 0x003F33C1 File Offset: 0x003F15C1
		public void CyclePrev()
		{
			this.GoTo(this.selectedIndex - 1);
		}

		// Token: 0x0600B7C3 RID: 47043 RVA: 0x003F33D1 File Offset: 0x003F15D1
		public void CycleNext()
		{
			this.GoTo(this.selectedIndex + 1);
		}

		// Token: 0x0600B7C4 RID: 47044 RVA: 0x003F33E4 File Offset: 0x003F15E4
		public void SetLabel(string text)
		{
			this.currentLabel.text = text;
			this.cyclePrevButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.MINION_BROWSER_SCREEN.TOOLTIP_CYCLE_PREVIOUS_OUTFIT_TYPE);
			this.cycleNextButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.MINION_BROWSER_SCREEN.TOOLTIP_CYCLE_NEXT_OUTFIT_TYPE);
		}

		// Token: 0x04009433 RID: 37939
		[SerializeField]
		public KButton cyclePrevButton;

		// Token: 0x04009434 RID: 37940
		[SerializeField]
		public KButton cycleNextButton;

		// Token: 0x04009435 RID: 37941
		[SerializeField]
		public LocText currentLabel;

		// Token: 0x04009436 RID: 37942
		[NonSerialized]
		private int selectedIndex = -1;

		// Token: 0x04009437 RID: 37943
		[NonSerialized]
		private MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions;

		// Token: 0x02002A7B RID: 10875
		// (Invoke) Token: 0x0600D4CD RID: 54477
		public delegate void OnSelectedFn();
	}

	// Token: 0x02001FF0 RID: 8176
	public abstract class GridItem : IEquatable<MinionBrowserScreen.GridItem>
	{
		// Token: 0x0600B7C6 RID: 47046
		public abstract string GetName();

		// Token: 0x0600B7C7 RID: 47047
		public abstract Sprite GetIcon();

		// Token: 0x0600B7C8 RID: 47048
		public abstract string GetUniqueId();

		// Token: 0x0600B7C9 RID: 47049
		public abstract Personality GetPersonality();

		// Token: 0x0600B7CA RID: 47050
		public abstract Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x0600B7CB RID: 47051
		public abstract JoyResponseOutfitTarget GetJoyResponseOutfitTarget();

		// Token: 0x0600B7CC RID: 47052 RVA: 0x003F3440 File Offset: 0x003F1640
		public override bool Equals(object obj)
		{
			MinionBrowserScreen.GridItem gridItem = obj as MinionBrowserScreen.GridItem;
			return gridItem != null && this.Equals(gridItem);
		}

		// Token: 0x0600B7CD RID: 47053 RVA: 0x003F3460 File Offset: 0x003F1660
		public bool Equals(MinionBrowserScreen.GridItem other)
		{
			return this.GetHashCode() == other.GetHashCode();
		}

		// Token: 0x0600B7CE RID: 47054 RVA: 0x003F3470 File Offset: 0x003F1670
		public override int GetHashCode()
		{
			return Hash.SDBMLower(this.GetUniqueId());
		}

		// Token: 0x0600B7CF RID: 47055 RVA: 0x003F347D File Offset: 0x003F167D
		public override string ToString()
		{
			return this.GetUniqueId();
		}

		// Token: 0x0600B7D0 RID: 47056 RVA: 0x003F3488 File Offset: 0x003F1688
		public static MinionBrowserScreen.GridItem.MinionInstanceTarget Of(GameObject minionInstance)
		{
			MinionIdentity component = minionInstance.GetComponent<MinionIdentity>();
			return new MinionBrowserScreen.GridItem.MinionInstanceTarget
			{
				minionInstance = minionInstance,
				minionIdentity = component,
				personality = Db.Get().Personalities.Get(component.personalityResourceId)
			};
		}

		// Token: 0x0600B7D1 RID: 47057 RVA: 0x003F34CA File Offset: 0x003F16CA
		public static MinionBrowserScreen.GridItem.PersonalityTarget Of(Personality personality)
		{
			return new MinionBrowserScreen.GridItem.PersonalityTarget
			{
				personality = personality
			};
		}

		// Token: 0x02002A7C RID: 10876
		public class MinionInstanceTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600D4D0 RID: 54480 RVA: 0x0043D894 File Offset: 0x0043BA94
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600D4D1 RID: 54481 RVA: 0x0043D8A1 File Offset: 0x0043BAA1
			public override string GetName()
			{
				return this.minionIdentity.GetProperName();
			}

			// Token: 0x0600D4D2 RID: 54482 RVA: 0x0043D8B0 File Offset: 0x0043BAB0
			public override string GetUniqueId()
			{
				return "minion_instance_id::" + this.minionInstance.GetInstanceID().ToString();
			}

			// Token: 0x0600D4D3 RID: 54483 RVA: 0x0043D8DA File Offset: 0x0043BADA
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600D4D4 RID: 54484 RVA: 0x0043D8E2 File Offset: 0x0043BAE2
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.FromMinion(outfitType, this.minionInstance);
			}

			// Token: 0x0600D4D5 RID: 54485 RVA: 0x0043D8F5 File Offset: 0x0043BAF5
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromMinion(this.minionInstance);
			}

			// Token: 0x0400BB84 RID: 48004
			public GameObject minionInstance;

			// Token: 0x0400BB85 RID: 48005
			public MinionIdentity minionIdentity;

			// Token: 0x0400BB86 RID: 48006
			public Personality personality;
		}

		// Token: 0x02002A7D RID: 10877
		public class PersonalityTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600D4D7 RID: 54487 RVA: 0x0043D90A File Offset: 0x0043BB0A
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600D4D8 RID: 54488 RVA: 0x0043D917 File Offset: 0x0043BB17
			public override string GetName()
			{
				return this.personality.Name;
			}

			// Token: 0x0600D4D9 RID: 54489 RVA: 0x0043D924 File Offset: 0x0043BB24
			public override string GetUniqueId()
			{
				return "personality::" + this.personality.nameStringKey;
			}

			// Token: 0x0600D4DA RID: 54490 RVA: 0x0043D93B File Offset: 0x0043BB3B
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600D4DB RID: 54491 RVA: 0x0043D943 File Offset: 0x0043BB43
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.TryFromTemplateId(this.personality.GetSelectedTemplateOutfitId(outfitType));
			}

			// Token: 0x0600D4DC RID: 54492 RVA: 0x0043D956 File Offset: 0x0043BB56
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromPersonality(this.personality);
			}

			// Token: 0x0400BB87 RID: 48007
			public Personality personality;
		}
	}
}
