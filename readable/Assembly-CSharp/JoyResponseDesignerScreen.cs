using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D35 RID: 3381
public class JoyResponseDesignerScreen : KMonoBehaviour
{
	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x06006875 RID: 26741 RVA: 0x002790FB File Offset: 0x002772FB
	// (set) Token: 0x06006876 RID: 26742 RVA: 0x00279103 File Offset: 0x00277303
	public JoyResponseScreenConfig Config { get; private set; }

	// Token: 0x06006877 RID: 26743 RVA: 0x0027910C File Offset: 0x0027730C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(this.categoryRowPrefab.transform.parent == this.categoryListContent.transform);
		global::Debug.Assert(this.galleryItemPrefab.transform.parent == this.galleryGridContent.transform);
		this.categoryRowPrefab.SetActive(false);
		this.galleryItemPrefab.SetActive(false);
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
		this.galleryGridItemPool = new UIPrefabLocalPool(this.galleryItemPrefab, this.galleryGridContent.gameObject);
		JoyResponseDesignerScreen.JoyResponseCategory[] array = new JoyResponseDesignerScreen.JoyResponseCategory[1];
		int num = 0;
		JoyResponseDesignerScreen.JoyResponseCategory joyResponseCategory = new JoyResponseDesignerScreen.JoyResponseCategory();
		joyResponseCategory.displayName = UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST;
		joyResponseCategory.icon = Assets.GetSprite("icon_inventory_balloonartist");
		JoyResponseDesignerScreen.GalleryItem[] items = (from r in Db.Get().Permits.BalloonArtistFacades.resources
		select JoyResponseDesignerScreen.GalleryItem.Of(r)).Prepend(JoyResponseDesignerScreen.GalleryItem.Of(Option.None)).ToArray<JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget>();
		joyResponseCategory.items = items;
		array[num] = joyResponseCategory;
		this.joyResponseCategories = array;
		this.dioramaVis.ConfigureSetup();
	}

	// Token: 0x06006878 RID: 26744 RVA: 0x0027928D File Offset: 0x0027748D
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06006879 RID: 26745 RVA: 0x0027929A File Offset: 0x0027749A
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		if (this.Config.isValid)
		{
			this.Configure(this.Config);
			return;
		}
		throw new InvalidOperationException("Cannot open up JoyResponseDesignerScreen without a target personality or minion instance");
	}

	// Token: 0x0600687A RID: 26746 RVA: 0x002792C7 File Offset: 0x002774C7
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.Configure(this.Config);
		});
	}

	// Token: 0x0600687B RID: 26747 RVA: 0x002792E8 File Offset: 0x002774E8
	public void Configure(JoyResponseScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.RegisterPreventScreenPop();
		this.primaryButton.ClearOnClick();
		TMP_Text componentInChildren = this.primaryButton.GetComponentInChildren<LocText>();
		LocString button_APPLY_TO_MINION = UI.JOY_RESPONSE_DESIGNER_SCREEN.BUTTON_APPLY_TO_MINION;
		string search = "{MinionName}";
		JoyResponseScreenConfig config2 = this.Config;
		componentInChildren.SetText(button_APPLY_TO_MINION.Replace(search, config2.target.GetMinionName()));
		this.primaryButton.onClick += delegate()
		{
			Option<PermitResource> permitResource = this.selectedGalleryItem.GetPermitResource();
			if (permitResource.IsSome())
			{
				string str = "Save selected balloon ";
				string name = this.selectedGalleryItem.GetName();
				string str2 = " for ";
				JoyResponseScreenConfig config3 = this.Config;
				global::Debug.Log(str + name + str2 + config3.target.GetMinionName());
				if (this.CanSaveSelection())
				{
					config3 = this.Config;
					config3.target.WriteFacadeId(permitResource.Unwrap().Id);
				}
			}
			else
			{
				string str3 = "Save selected balloon ";
				string name2 = this.selectedGalleryItem.GetName();
				string str4 = " for ";
				JoyResponseScreenConfig config3 = this.Config;
				global::Debug.Log(str3 + name2 + str4 + config3.target.GetMinionName());
				config3 = this.Config;
				config3.target.WriteFacadeId(Option.None);
			}
			LockerNavigator.Instance.PopScreen();
		};
		this.PopulateCategories();
		this.PopulateGallery();
		this.PopulatePreview();
		config2 = this.Config;
		if (config2.initalSelectedItem.IsSome())
		{
			config2 = this.Config;
			this.SelectGalleryItem(config2.initalSelectedItem.Unwrap());
		}
	}

	// Token: 0x0600687C RID: 26748 RVA: 0x002793A0 File Offset: 0x002775A0
	private bool CanSaveSelection()
	{
		return this.GetSaveSelectionError().IsNone();
	}

	// Token: 0x0600687D RID: 26749 RVA: 0x002793BC File Offset: 0x002775BC
	private Option<string> GetSaveSelectionError()
	{
		if (!this.selectedGalleryItem.IsUnlocked())
		{
			return Option.Some<string>(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED.Replace("{MinionName}", this.Config.target.GetMinionName()));
		}
		return Option.None;
	}

	// Token: 0x0600687E RID: 26750 RVA: 0x00279408 File Offset: 0x00277608
	private void RefreshCategories()
	{
		if (this.RefreshCategoriesFn != null)
		{
			this.RefreshCategoriesFn();
		}
	}

	// Token: 0x0600687F RID: 26751 RVA: 0x00279420 File Offset: 0x00277620
	public void PopulateCategories()
	{
		this.RefreshCategoriesFn = null;
		this.categoryRowPool.ReturnAll();
		JoyResponseDesignerScreen.JoyResponseCategory[] array = this.joyResponseCategories;
		for (int i = 0; i < array.Length; i++)
		{
			JoyResponseDesignerScreen.<>c__DisplayClass28_0 CS$<>8__locals1 = new JoyResponseDesignerScreen.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.category = array[i];
			GameObject gameObject = this.categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(CS$<>8__locals1.category.displayName);
			component.GetReference<Image>("Icon").sprite = CS$<>8__locals1.category.icon;
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle toggle2 = toggle;
			toggle2.onEnter = (System.Action)Delegate.Combine(toggle2.onEnter, new System.Action(this.OnMouseOverToggle));
			toggle.onClick = delegate()
			{
				CS$<>8__locals1.<>4__this.SelectCategory(CS$<>8__locals1.category);
			};
			this.RefreshCategoriesFn = (System.Action)Delegate.Combine(this.RefreshCategoriesFn, new System.Action(delegate()
			{
				toggle.ChangeState((CS$<>8__locals1.category == CS$<>8__locals1.<>4__this.selectedCategoryOpt) ? 1 : 0);
			}));
			this.SetCatogoryClickUISound(CS$<>8__locals1.category, toggle);
		}
		this.SelectCategory(this.joyResponseCategories[0]);
	}

	// Token: 0x06006880 RID: 26752 RVA: 0x00279567 File Offset: 0x00277767
	public void SelectCategory(JoyResponseDesignerScreen.JoyResponseCategory category)
	{
		this.selectedCategoryOpt = category;
		this.galleryHeaderLabel.text = category.displayName;
		this.RefreshCategories();
		this.PopulateGallery();
		this.RefreshPreview();
	}

	// Token: 0x06006881 RID: 26753 RVA: 0x00279598 File Offset: 0x00277798
	private void SetCatogoryClickUISound(JoyResponseDesignerScreen.JoyResponseCategory category, MultiToggle toggle)
	{
	}

	// Token: 0x06006882 RID: 26754 RVA: 0x0027959A File Offset: 0x0027779A
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x06006883 RID: 26755 RVA: 0x002795B0 File Offset: 0x002777B0
	public void PopulateGallery()
	{
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		if (this.selectedCategoryOpt.IsNone())
		{
			return;
		}
		JoyResponseDesignerScreen.JoyResponseCategory joyResponseCategory = this.selectedCategoryOpt.Unwrap();
		foreach (JoyResponseDesignerScreen.GalleryItem item in joyResponseCategory.items)
		{
			this.<PopulateGallery>g__AddGridIcon|36_0(item);
		}
		this.SelectGalleryItem(joyResponseCategory.items[0]);
	}

	// Token: 0x06006884 RID: 26756 RVA: 0x00279617 File Offset: 0x00277817
	public void SelectGalleryItem(JoyResponseDesignerScreen.GalleryItem item)
	{
		this.selectedGalleryItem = item;
		this.RefreshGallery();
		this.RefreshPreview();
	}

	// Token: 0x06006885 RID: 26757 RVA: 0x0027962C File Offset: 0x0027782C
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06006886 RID: 26758 RVA: 0x0027963E File Offset: 0x0027783E
	public void RefreshPreview()
	{
		if (this.RefreshPreviewFn != null)
		{
			this.RefreshPreviewFn();
		}
	}

	// Token: 0x06006887 RID: 26759 RVA: 0x00279653 File Offset: 0x00277853
	public void PopulatePreview()
	{
		this.RefreshPreviewFn = (System.Action)Delegate.Combine(this.RefreshPreviewFn, new System.Action(delegate()
		{
			JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget balloonArtistFacadeTarget = this.selectedGalleryItem as JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget;
			if (balloonArtistFacadeTarget == null)
			{
				throw new NotImplementedException();
			}
			Option<PermitResource> permitResource = balloonArtistFacadeTarget.GetPermitResource();
			this.selectionHeaderLabel.SetText(balloonArtistFacadeTarget.GetName());
			KleiPermitDioramaVis_JoyResponseBalloon kleiPermitDioramaVis_JoyResponseBalloon = this.dioramaVis;
			JoyResponseScreenConfig config = this.Config;
			kleiPermitDioramaVis_JoyResponseBalloon.SetMinion(config.target.GetPersonality());
			this.dioramaVis.ConfigureWith(balloonArtistFacadeTarget.permit);
			OutfitDescriptionPanel outfitDescriptionPanel = this.outfitDescriptionPanel;
			PermitResource permitResource2 = permitResource.UnwrapOr(null, null);
			ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.JoyResponse;
			config = this.Config;
			outfitDescriptionPanel.Refresh(permitResource2, outfitType, config.target.GetPersonality());
			Option<string> saveSelectionError = this.GetSaveSelectionError();
			if (saveSelectionError.IsSome())
			{
				this.primaryButton.isInteractable = false;
				this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(saveSelectionError.Unwrap());
				return;
			}
			this.primaryButton.isInteractable = true;
			this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}));
		this.RefreshPreview();
	}

	// Token: 0x06006888 RID: 26760 RVA: 0x0027967D File Offset: 0x0027787D
	private void RegisterPreventScreenPop()
	{
		this.UnregisterPreventScreenPop();
		this.preventScreenPopFn = delegate()
		{
			if (this.Config.target.ReadFacadeId() != this.selectedGalleryItem.GetPermitResource().AndThen<string>((PermitResource r) => r.Id))
			{
				this.RegisterPreventScreenPop();
				JoyResponseDesignerScreen.MakeSaveWarningPopup(this.Config.target, delegate
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

	// Token: 0x06006889 RID: 26761 RVA: 0x002796AC File Offset: 0x002778AC
	private void UnregisterPreventScreenPop()
	{
		if (this.preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
			this.preventScreenPopFn = null;
		}
	}

	// Token: 0x0600688A RID: 26762 RVA: 0x002796D4 File Offset: 0x002778D4
	public static void MakeSaveWarningPopup(JoyResponseOutfitTarget target, System.Action discardChangesFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.JOY_RESPONSE_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{MinionName}", target.GetMinionName())).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY);
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

	// Token: 0x0600688E RID: 26766 RVA: 0x00279804 File Offset: 0x00277A04
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIcon|36_0(JoyResponseDesignerScreen.GalleryItem item)
	{
		GameObject gameObject = this.galleryGridItemPool.Borrow();
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = item.GetIcon();
		component.GetReference<Image>("IsUnownedOverlay").gameObject.SetActive(!item.IsUnlocked());
		Option<PermitResource> permitResource = item.GetPermitResource();
		if (permitResource.IsSome())
		{
			KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetTooltipStringFor(permitResource.Unwrap()));
		}
		else
		{
			KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetNoneTooltipStringFor(PermitCategory.JoyResponse));
		}
		MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
		MultiToggle toggle3 = toggle;
		toggle3.onEnter = (System.Action)Delegate.Combine(toggle3.onEnter, new System.Action(this.OnMouseOverToggle));
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			this.SelectGalleryItem(item);
		}));
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			toggle.ChangeState((item == this.selectedGalleryItem) ? 1 : 0);
		}));
	}

	// Token: 0x040047C1 RID: 18369
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x040047C2 RID: 18370
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x040047C3 RID: 18371
	[Header("GalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x040047C4 RID: 18372
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x040047C5 RID: 18373
	[SerializeField]
	private GameObject galleryItemPrefab;

	// Token: 0x040047C6 RID: 18374
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x040047C7 RID: 18375
	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon dioramaVis;

	// Token: 0x040047C8 RID: 18376
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x040047C9 RID: 18377
	[SerializeField]
	private KButton primaryButton;

	// Token: 0x040047CB RID: 18379
	public JoyResponseDesignerScreen.JoyResponseCategory[] joyResponseCategories;

	// Token: 0x040047CC RID: 18380
	private bool postponeConfiguration = true;

	// Token: 0x040047CD RID: 18381
	private Option<JoyResponseDesignerScreen.JoyResponseCategory> selectedCategoryOpt;

	// Token: 0x040047CE RID: 18382
	private UIPrefabLocalPool categoryRowPool;

	// Token: 0x040047CF RID: 18383
	private System.Action RefreshCategoriesFn;

	// Token: 0x040047D0 RID: 18384
	private JoyResponseDesignerScreen.GalleryItem selectedGalleryItem;

	// Token: 0x040047D1 RID: 18385
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x040047D2 RID: 18386
	private GridLayouter galleryGridLayouter;

	// Token: 0x040047D3 RID: 18387
	private System.Action RefreshGalleryFn;

	// Token: 0x040047D4 RID: 18388
	public System.Action RefreshPreviewFn;

	// Token: 0x040047D5 RID: 18389
	private Func<bool> preventScreenPopFn;

	// Token: 0x02001F58 RID: 8024
	public class JoyResponseCategory
	{
		// Token: 0x0400928D RID: 37517
		public string displayName;

		// Token: 0x0400928E RID: 37518
		public Sprite icon;

		// Token: 0x0400928F RID: 37519
		public JoyResponseDesignerScreen.GalleryItem[] items;
	}

	// Token: 0x02001F59 RID: 8025
	private enum MultiToggleState
	{
		// Token: 0x04009291 RID: 37521
		Default,
		// Token: 0x04009292 RID: 37522
		Selected
	}

	// Token: 0x02001F5A RID: 8026
	public abstract class GalleryItem : IEquatable<JoyResponseDesignerScreen.GalleryItem>
	{
		// Token: 0x0600B610 RID: 46608
		public abstract string GetName();

		// Token: 0x0600B611 RID: 46609
		public abstract Sprite GetIcon();

		// Token: 0x0600B612 RID: 46610
		public abstract string GetUniqueId();

		// Token: 0x0600B613 RID: 46611
		public abstract bool IsUnlocked();

		// Token: 0x0600B614 RID: 46612
		public abstract Option<PermitResource> GetPermitResource();

		// Token: 0x0600B615 RID: 46613 RVA: 0x003EF570 File Offset: 0x003ED770
		public override bool Equals(object obj)
		{
			JoyResponseDesignerScreen.GalleryItem galleryItem = obj as JoyResponseDesignerScreen.GalleryItem;
			return galleryItem != null && this.Equals(galleryItem);
		}

		// Token: 0x0600B616 RID: 46614 RVA: 0x003EF590 File Offset: 0x003ED790
		public bool Equals(JoyResponseDesignerScreen.GalleryItem other)
		{
			return this.GetHashCode() == other.GetHashCode();
		}

		// Token: 0x0600B617 RID: 46615 RVA: 0x003EF5A0 File Offset: 0x003ED7A0
		public override int GetHashCode()
		{
			return Hash.SDBMLower(this.GetUniqueId());
		}

		// Token: 0x0600B618 RID: 46616 RVA: 0x003EF5AD File Offset: 0x003ED7AD
		public override string ToString()
		{
			return this.GetUniqueId();
		}

		// Token: 0x0600B619 RID: 46617 RVA: 0x003EF5B5 File Offset: 0x003ED7B5
		public static JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget Of(Option<BalloonArtistFacadeResource> permit)
		{
			return new JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget
			{
				permit = permit
			};
		}

		// Token: 0x02002A77 RID: 10871
		public class BalloonArtistFacadeTarget : JoyResponseDesignerScreen.GalleryItem
		{
			// Token: 0x0600D4BA RID: 54458 RVA: 0x0043D70C File Offset: 0x0043B90C
			public override Sprite GetIcon()
			{
				return this.permit.AndThen<Sprite>((BalloonArtistFacadeResource p) => p.GetPermitPresentationInfo().sprite).UnwrapOrElse(() => KleiItemsUI.GetNoneBalloonArtistIcon(), null);
			}

			// Token: 0x0600D4BB RID: 54459 RVA: 0x0043D76C File Offset: 0x0043B96C
			public override string GetName()
			{
				return this.permit.AndThen<string>((BalloonArtistFacadeResource p) => p.Name).UnwrapOrElse(() => KleiItemsUI.GetNoneClothingItemStrings(PermitCategory.JoyResponse).Item1, null);
			}

			// Token: 0x0600D4BC RID: 54460 RVA: 0x0043D7CC File Offset: 0x0043B9CC
			public override string GetUniqueId()
			{
				return "balloon_artist_facade::" + this.permit.AndThen<string>((BalloonArtistFacadeResource p) => p.Id).UnwrapOr("<none>", null);
			}

			// Token: 0x0600D4BD RID: 54461 RVA: 0x0043D81B File Offset: 0x0043BA1B
			public override Option<PermitResource> GetPermitResource()
			{
				return this.permit.AndThen<PermitResource>((BalloonArtistFacadeResource p) => p);
			}

			// Token: 0x0600D4BE RID: 54462 RVA: 0x0043D848 File Offset: 0x0043BA48
			public override bool IsUnlocked()
			{
				return this.GetPermitResource().AndThen<bool>((PermitResource p) => p.IsUnlocked()).UnwrapOr(true, null);
			}

			// Token: 0x0400BB83 RID: 48003
			public Option<BalloonArtistFacadeResource> permit;
		}
	}
}
