using System;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DD6 RID: 3542
public class PauseScreen : KModalButtonMenu
{
	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x06006EE6 RID: 28390 RVA: 0x002A0551 File Offset: 0x0029E751
	public static PauseScreen Instance
	{
		get
		{
			return PauseScreen.instance;
		}
	}

	// Token: 0x06006EE7 RID: 28391 RVA: 0x002A0558 File Offset: 0x0029E758
	public static void DestroyInstance()
	{
		PauseScreen.instance = null;
	}

	// Token: 0x06006EE8 RID: 28392 RVA: 0x002A0560 File Offset: 0x0029E760
	protected override void OnPrefabInit()
	{
		this.keepMenuOpen = true;
		base.OnPrefabInit();
		this.ConfigureButtonInfos();
		this.closeButton.onClick += this.OnResume;
		PauseScreen.instance = this;
		this.Show(false);
	}

	// Token: 0x06006EE9 RID: 28393 RVA: 0x002A059C File Offset: 0x0029E79C
	private void ConfigureButtonInfos()
	{
		if (!GenericGameSettings.instance.demoMode)
		{
			this.buttons = new KButtonMenu.ButtonInfo[]
			{
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
				new KButtonMenu.ButtonInfo(this.recentlySaved ? UI.FRONTEND.PAUSE_SCREEN.ALREADY_SAVED : UI.FRONTEND.PAUSE_SCREEN.SAVE, global::Action.NumActions, new UnityAction(this.OnSave), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.SAVEAS, global::Action.NumActions, new UnityAction(this.OnSaveAs), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOAD, global::Action.NumActions, new UnityAction(this.OnLoad), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.COLONY_SUMMARY, global::Action.NumActions, new UnityAction(this.OnColonySummary), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOCKERMENU, global::Action.NumActions, new UnityAction(this.OnLockerMenu), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
			};
			return;
		}
		this.buttons = new KButtonMenu.ButtonInfo[]
		{
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
		};
	}

	// Token: 0x06006EEA RID: 28394 RVA: 0x002A07C4 File Offset: 0x0029E9C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.clipboard.GetText = new Func<string>(this.GetClipboardText);
		this.title.SetText(UI.FRONTEND.PAUSE_SCREEN.TITLE);
		try
		{
			string settingsCoordinate = CustomGameSettings.Instance.GetSettingsCoordinate();
			string[] array = CustomGameSettings.ParseSettingCoordinate(settingsCoordinate);
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, settingsCoordinate));
			this.worldSeed.GetComponent<ToolTip>().toolTip = string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED_TOOLTIP, new object[]
			{
				array[1],
				array[2],
				array[3],
				array[4],
				array[5]
			});
		}
		catch (Exception arg)
		{
			global::Debug.LogWarning(string.Format("Failed to load Coordinates on ClusterLayout {0}, please report this error on the forums", arg));
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, "0"));
		}
	}

	// Token: 0x06006EEB RID: 28395 RVA: 0x002A08EC File Offset: 0x0029EAEC
	public override float GetSortKey()
	{
		return 30f;
	}

	// Token: 0x06006EEC RID: 28396 RVA: 0x002A08F4 File Offset: 0x0029EAF4
	private string GetClipboardText()
	{
		string result;
		try
		{
			result = CustomGameSettings.Instance.GetSettingsCoordinate();
		}
		catch
		{
			result = "";
		}
		return result;
	}

	// Token: 0x06006EED RID: 28397 RVA: 0x002A0928 File Offset: 0x0029EB28
	private void OnResume()
	{
		this.Show(false);
	}

	// Token: 0x06006EEE RID: 28398 RVA: 0x002A0934 File Offset: 0x0029EB34
	protected override void OnShow(bool show)
	{
		this.recentlySaved = false;
		this.ConfigureButtonInfos();
		base.OnShow(show);
		if (show)
		{
			this.RefreshButtons();
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().ESCPauseSnapshot);
			MusicManager.instance.OnEscapeMenu(true);
			MusicManager.instance.PlaySong("Music_ESC_Menu", false);
			this.RefreshDLCActivationButtons();
			return;
		}
		ToolTipScreen.Instance.ClearToolTip(this.closeButton.GetComponent<ToolTip>());
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ESCPauseSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.OnEscapeMenu(false);
		if (MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
		{
			MusicManager.instance.StopSong("Music_ESC_Menu", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06006EEF RID: 28399 RVA: 0x002A09ED File Offset: 0x0029EBED
	private void OnOptions()
	{
		base.ActivateChildScreen(this.optionsScreen.gameObject);
	}

	// Token: 0x06006EF0 RID: 28400 RVA: 0x002A0A01 File Offset: 0x0029EC01
	private void OnSaveAs()
	{
		base.ActivateChildScreen(this.saveScreenPrefab.gameObject);
	}

	// Token: 0x06006EF1 RID: 28401 RVA: 0x002A0A18 File Offset: 0x0029EC18
	private void OnSave()
	{
		string filename = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
		{
			base.gameObject.SetActive(false);
			((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, System.IO.Path.GetFileNameWithoutExtension(filename)), delegate
			{
				this.DoSave(filename);
				this.gameObject.SetActive(true);
			}, new System.Action(this.OnCancelPopup), null, null, null, null, null, null);
			return;
		}
		this.OnSaveAs();
	}

	// Token: 0x06006EF2 RID: 28402 RVA: 0x002A0AD9 File Offset: 0x0029ECD9
	public void OnSaveComplete()
	{
		this.recentlySaved = true;
		this.ConfigureButtonInfos();
		this.RefreshButtons();
	}

	// Token: 0x06006EF3 RID: 28403 RVA: 0x002A0AF0 File Offset: 0x0029ECF0
	private void DoSave(string filename)
	{
		try
		{
			SaveLoader.Instance.Save(filename, false, true);
			this.OnSaveComplete();
		}
		catch (IOException ex)
		{
			IOException e2 = ex;
			IOException e = e2;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.IO_ERROR, e.ToString()), delegate
			{
				this.Deactivate();
			}, null, UI.FRONTEND.SAVESCREEN.REPORT_BUG, delegate
			{
				KCrashReporter.ReportError(e.Message, e.StackTrace.ToString(), null, null, null, true, new string[]
				{
					KCrashReporter.CRASH_CATEGORY.FILEIO
				}, null);
			}, null, null, null, null);
		}
	}

	// Token: 0x06006EF4 RID: 28404 RVA: 0x002A0BA8 File Offset: 0x0029EDA8
	private void ConfirmDecision(string questionText, string primaryButtonText, System.Action primaryButtonAction, string alternateButtonText = null, System.Action alternateButtonAction = null)
	{
		base.gameObject.SetActive(false);
		((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(questionText, primaryButtonAction, new System.Action(this.OnCancelPopup), alternateButtonText, alternateButtonAction, null, primaryButtonText, null, null);
	}

	// Token: 0x06006EF5 RID: 28405 RVA: 0x002A0C0B File Offset: 0x0029EE0B
	private void OnLoad()
	{
		base.ActivateChildScreen(this.loadScreenPrefab.gameObject);
	}

	// Token: 0x06006EF6 RID: 28406 RVA: 0x002A0C20 File Offset: 0x0029EE20
	private void OnColonySummary()
	{
		RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
	}

	// Token: 0x06006EF7 RID: 28407 RVA: 0x002A0C4D File Offset: 0x0029EE4D
	private void OnLockerMenu()
	{
		LockerMenuScreen.Instance.Show(true);
	}

	// Token: 0x06006EF8 RID: 28408 RVA: 0x002A0C5A File Offset: 0x0029EE5A
	private void OnQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.QUITCONFIRM, UI.FRONTEND.MAINMENU.SAVEANDQUITTITLE, delegate
		{
			this.OnQuitConfirm(true);
		}, UI.FRONTEND.MAINMENU.QUIT, delegate
		{
			this.OnQuitConfirm(false);
		});
	}

	// Token: 0x06006EF9 RID: 28409 RVA: 0x002A0C98 File Offset: 0x0029EE98
	private void OnDesktopQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.DESKTOPQUITCONFIRM, UI.FRONTEND.MAINMENU.SAVEANDQUITDESKTOP, delegate
		{
			this.OnDesktopQuitConfirm(true);
		}, UI.FRONTEND.MAINMENU.QUIT, delegate
		{
			this.OnDesktopQuitConfirm(false);
		});
	}

	// Token: 0x06006EFA RID: 28410 RVA: 0x002A0CD6 File Offset: 0x0029EED6
	private void OnCancelPopup()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x06006EFB RID: 28411 RVA: 0x002A0CE4 File Offset: 0x0029EEE4
	private void OnLoadConfirm()
	{
		LoadingOverlay.Load(delegate
		{
			LoadScreen.ForceStopGame();
			this.Deactivate();
			App.LoadScene("frontend");
		});
	}

	// Token: 0x06006EFC RID: 28412 RVA: 0x002A0CF7 File Offset: 0x0029EEF7
	private void OnRetireConfirm()
	{
		RetireColonyUtility.SaveColonySummaryData();
	}

	// Token: 0x06006EFD RID: 28413 RVA: 0x002A0D00 File Offset: 0x0029EF00
	private void OnQuitConfirm(bool saveFirst)
	{
		if (saveFirst)
		{
			string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
			if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
			{
				this.DoSave(activeSaveFilePath);
			}
			else
			{
				this.OnSaveAs();
			}
		}
		LoadingOverlay.Load(delegate
		{
			this.Deactivate();
			PauseScreen.TriggerQuitGame();
		});
	}

	// Token: 0x06006EFE RID: 28414 RVA: 0x002A0D48 File Offset: 0x0029EF48
	private void OnDesktopQuitConfirm(bool saveFirst)
	{
		if (saveFirst)
		{
			string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
			if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
			{
				this.DoSave(activeSaveFilePath);
			}
			else
			{
				this.OnSaveAs();
			}
		}
		App.Quit();
	}

	// Token: 0x06006EFF RID: 28415 RVA: 0x002A0D82 File Offset: 0x0029EF82
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006F00 RID: 28416 RVA: 0x002A0DA5 File Offset: 0x0029EFA5
	public static void TriggerQuitGame()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndGame();
		LoadScreen.ForceStopGame();
		App.LoadScene("frontend");
	}

	// Token: 0x06006F01 RID: 28417 RVA: 0x002A0DC0 File Offset: 0x0029EFC0
	private void RefreshDLCActivationButtons()
	{
		foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
		{
			if (!keyValuePair.Value.isCosmetic && !this.dlcActivationButtons.ContainsKey(keyValuePair.Key))
			{
				GameObject gameObject = global::Util.KInstantiateUI(this.dlcActivationButtonPrefab, this.dlcActivationButtonPrefab.transform.parent.gameObject, true);
				Sprite sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(keyValuePair.Key));
				gameObject.GetComponent<Image>().sprite = sprite;
				gameObject.GetComponent<MultiToggle>().states[0].sprite = sprite;
				gameObject.GetComponent<MultiToggle>().states[1].sprite = sprite;
				this.dlcActivationButtons.Add(keyValuePair.Key, gameObject);
			}
		}
		this.RefreshDLCButton("EXPANSION1_ID", this.dlc1ActivationButton, false);
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.dlcActivationButtons)
		{
			this.RefreshDLCButton(keyValuePair2.Key, keyValuePair2.Value.GetComponent<MultiToggle>(), true);
		}
	}

	// Token: 0x06006F02 RID: 28418 RVA: 0x002A0F28 File Offset: 0x0029F128
	private void RefreshDLCButton(string DLCID, MultiToggle button, bool userEditable)
	{
		button.GetComponent<MultiToggle>().states[0].sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(DLCID));
		button.GetComponent<MultiToggle>().states[1].sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(DLCID));
		button.ChangeState(Game.IsDlcActiveForCurrentSave(DLCID) ? 1 : 0);
		button.GetComponent<Image>().material = (Game.IsDlcActiveForCurrentSave(DLCID) ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		ToolTip component = button.GetComponent<ToolTip>();
		string dlcTitle = DlcManager.GetDlcTitle(DLCID);
		if (!DlcManager.IsContentSubscribed(DLCID))
		{
			component.SetSimpleTooltip(string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, dlcTitle));
			button.onClick = null;
			return;
		}
		if (userEditable)
		{
			component.SetSimpleTooltip(Game.IsDlcActiveForCurrentSave(DLCID) ? string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, dlcTitle) : string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_TOOLTIP, dlcTitle));
			button.onClick = delegate()
			{
				this.OnClickAddDLCButton(DLCID);
			};
			return;
		}
		component.SetSimpleTooltip(Game.IsDlcActiveForCurrentSave(DLCID) ? string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, dlcTitle) : string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, dlcTitle));
		button.onClick = null;
	}

	// Token: 0x06006F03 RID: 28419 RVA: 0x002A10AC File Offset: 0x0029F2AC
	private void OnClickAddDLCButton(string dlcID)
	{
		if (!Game.IsDlcActiveForCurrentSave(dlcID))
		{
			this.ConfirmDecision(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.ENABLE_QUESTION, UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.CONFIRM, delegate
			{
				this.OnConfirmAddDLC(dlcID);
			}, null, null);
		}
	}

	// Token: 0x06006F04 RID: 28420 RVA: 0x002A1102 File Offset: 0x0029F302
	private void OnConfirmAddDLC(string dlcId)
	{
		SaveLoader.Instance.UpgradeActiveSaveDLCInfo(dlcId, true);
	}

	// Token: 0x04004BDD RID: 19421
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	// Token: 0x04004BDE RID: 19422
	[SerializeField]
	private SaveScreen saveScreenPrefab;

	// Token: 0x04004BDF RID: 19423
	[SerializeField]
	private LoadScreen loadScreenPrefab;

	// Token: 0x04004BE0 RID: 19424
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004BE1 RID: 19425
	[SerializeField]
	private LocText title;

	// Token: 0x04004BE2 RID: 19426
	[SerializeField]
	private LocText worldSeed;

	// Token: 0x04004BE3 RID: 19427
	[SerializeField]
	private CopyTextFieldToClipboard clipboard;

	// Token: 0x04004BE4 RID: 19428
	[SerializeField]
	private MultiToggle dlc1ActivationButton;

	// Token: 0x04004BE5 RID: 19429
	[SerializeField]
	private GameObject dlcActivationButtonPrefab;

	// Token: 0x04004BE6 RID: 19430
	private Dictionary<string, GameObject> dlcActivationButtons = new Dictionary<string, GameObject>();

	// Token: 0x04004BE7 RID: 19431
	private float originalTimeScale;

	// Token: 0x04004BE8 RID: 19432
	private bool recentlySaved;

	// Token: 0x04004BE9 RID: 19433
	private static PauseScreen instance;
}
