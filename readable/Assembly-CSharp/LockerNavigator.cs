using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D64 RID: 3428
public class LockerNavigator : KModalScreen
{
	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x06006A33 RID: 27187 RVA: 0x00282795 File Offset: 0x00280995
	public GameObject ContentSlot
	{
		get
		{
			return this.slot.gameObject;
		}
	}

	// Token: 0x06006A34 RID: 27188 RVA: 0x002827A2 File Offset: 0x002809A2
	protected override void OnActivate()
	{
		LockerNavigator.Instance = this;
		this.Show(false);
		this.backButton.onClick += this.OnClickBack;
	}

	// Token: 0x06006A35 RID: 27189 RVA: 0x002827C8 File Offset: 0x002809C8
	public override float GetSortKey()
	{
		return 41f;
	}

	// Token: 0x06006A36 RID: 27190 RVA: 0x002827CF File Offset: 0x002809CF
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.PopScreen();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006A37 RID: 27191 RVA: 0x002827F1 File Offset: 0x002809F1
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (!show)
		{
			this.PopAllScreens();
		}
		StreamedTextures.SetBundlesLoaded(show);
	}

	// Token: 0x06006A38 RID: 27192 RVA: 0x00282809 File Offset: 0x00280A09
	private void OnClickBack()
	{
		this.PopScreen();
	}

	// Token: 0x06006A39 RID: 27193 RVA: 0x00282814 File Offset: 0x00280A14
	public void PushScreen(GameObject screen, System.Action onClose = null)
	{
		if (screen == null)
		{
			return;
		}
		if (this.navigationHistory.Count == 0)
		{
			this.Show(true);
			if (!LockerNavigator.didDisplayDataCollectionWarningPopupOnce && KPrivacyPrefs.instance.disableDataCollection)
			{
				LockerNavigator.MakeDataCollectionWarningPopup(base.gameObject.transform.parent.gameObject);
				LockerNavigator.didDisplayDataCollectionWarningPopupOnce = true;
			}
		}
		if (this.navigationHistory.Count > 0 && screen == this.navigationHistory[this.navigationHistory.Count - 1].screen)
		{
			return;
		}
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(false);
		}
		this.navigationHistory.Add(new LockerNavigator.HistoryEntry(screen, onClose));
		this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.RefreshButtons();
	}

	// Token: 0x06006A3A RID: 27194 RVA: 0x0028292C File Offset: 0x00280B2C
	public bool PopScreen()
	{
		while (this.preventScreenPop.Count > 0)
		{
			int index = this.preventScreenPop.Count - 1;
			Func<bool> func = this.preventScreenPop[index];
			this.preventScreenPop.RemoveAt(index);
			if (func())
			{
				return true;
			}
		}
		int index2 = this.navigationHistory.Count - 1;
		LockerNavigator.HistoryEntry historyEntry = this.navigationHistory[index2];
		historyEntry.screen.SetActive(false);
		if (historyEntry.onClose.IsSome())
		{
			historyEntry.onClose.Unwrap()();
		}
		this.navigationHistory.RemoveAt(index2);
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
			this.RefreshButtons();
			return true;
		}
		this.Show(false);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "initial", true);
		return false;
	}

	// Token: 0x06006A3B RID: 27195 RVA: 0x00282A28 File Offset: 0x00280C28
	public void PopAllScreens()
	{
		if (this.navigationHistory.Count == 0 && this.preventScreenPop.Count == 0)
		{
			return;
		}
		int num = 0;
		while (this.PopScreen())
		{
			if (num > 100)
			{
				DebugUtil.DevAssert(false, string.Format("Can't close all LockerNavigator screens, hit limit of trying to close {0} screens", 100), null);
				return;
			}
			num++;
		}
	}

	// Token: 0x06006A3C RID: 27196 RVA: 0x00282A7E File Offset: 0x00280C7E
	private void RefreshButtons()
	{
		this.backButton.isInteractable = true;
	}

	// Token: 0x06006A3D RID: 27197 RVA: 0x00282A8C File Offset: 0x00280C8C
	public void ShowDialogPopup(Action<InfoDialogScreen> configureDialogFn)
	{
		InfoDialogScreen dialog = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.ContentSlot, false);
		configureDialogFn(dialog);
		dialog.Activate();
		dialog.gameObject.AddOrGet<LayoutElement>().ignoreLayout = true;
		dialog.gameObject.AddOrGet<RectTransform>().Fill();
		Func<bool> preventScreenPopFn = delegate()
		{
			dialog.Deactivate();
			return true;
		};
		this.preventScreenPop.Add(preventScreenPopFn);
		InfoDialogScreen dialog2 = dialog;
		dialog2.onDeactivateFn = (System.Action)Delegate.Combine(dialog2.onDeactivateFn, new System.Action(delegate()
		{
			this.preventScreenPop.Remove(preventScreenPopFn);
		}));
	}

	// Token: 0x06006A3E RID: 27198 RVA: 0x00282B54 File Offset: 0x00280D54
	public static void MakeDataCollectionWarningPopup(GameObject fullscreenParent)
	{
		Action<InfoDialogScreen> <>9__2;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.HEADER).AddPlainText(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BODY).AddOption(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OK, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, true);
			string text = UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OPEN_SETTINGS;
			Action<InfoDialogScreen> action;
			if ((action = <>9__2) == null)
			{
				action = (<>9__2 = delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					LockerNavigator.Instance.PopAllScreens();
					LockerMenuScreen.Instance.Show(false);
					Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, fullscreenParent, true).ShowMetricsScreen();
				});
			}
			infoDialogScreen.AddOption(text, action, false);
		});
	}

	// Token: 0x040048FA RID: 18682
	public static LockerNavigator Instance;

	// Token: 0x040048FB RID: 18683
	[SerializeField]
	private RectTransform slot;

	// Token: 0x040048FC RID: 18684
	[SerializeField]
	private KButton backButton;

	// Token: 0x040048FD RID: 18685
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040048FE RID: 18686
	[SerializeField]
	public GameObject kleiInventoryScreen;

	// Token: 0x040048FF RID: 18687
	[SerializeField]
	public GameObject duplicantCatalogueScreen;

	// Token: 0x04004900 RID: 18688
	[SerializeField]
	public GameObject outfitDesignerScreen;

	// Token: 0x04004901 RID: 18689
	[SerializeField]
	public GameObject outfitBrowserScreen;

	// Token: 0x04004902 RID: 18690
	[SerializeField]
	public GameObject joyResponseDesignerScreen;

	// Token: 0x04004903 RID: 18691
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x04004904 RID: 18692
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x04004905 RID: 18693
	private List<LockerNavigator.HistoryEntry> navigationHistory = new List<LockerNavigator.HistoryEntry>();

	// Token: 0x04004906 RID: 18694
	private Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();

	// Token: 0x04004907 RID: 18695
	private static bool didDisplayDataCollectionWarningPopupOnce;

	// Token: 0x04004908 RID: 18696
	public List<Func<bool>> preventScreenPop = new List<Func<bool>>();

	// Token: 0x02001F94 RID: 8084
	public readonly struct HistoryEntry
	{
		// Token: 0x0600B6C5 RID: 46789 RVA: 0x003F178F File Offset: 0x003EF98F
		public HistoryEntry(GameObject screen, System.Action onClose = null)
		{
			this.screen = screen;
			this.onClose = onClose;
		}

		// Token: 0x04009348 RID: 37704
		public readonly GameObject screen;

		// Token: 0x04009349 RID: 37705
		public readonly Option<System.Action> onClose;
	}
}
