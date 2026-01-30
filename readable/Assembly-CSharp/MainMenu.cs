using System;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using Klei;
using ProcGenGame;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D68 RID: 3432
public class MainMenu : KScreen
{
	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x06006A4A RID: 27210 RVA: 0x00282F26 File Offset: 0x00281126
	public static MainMenu Instance
	{
		get
		{
			return MainMenu._instance;
		}
	}

	// Token: 0x06006A4B RID: 27211 RVA: 0x00282F30 File Offset: 0x00281130
	private KButton MakeButton(MainMenu.ButtonInfo info)
	{
		KButton kbutton = global::Util.KInstantiateUI<KButton>(this.buttonPrefab.gameObject, this.buttonParent, true);
		kbutton.onClick += info.action;
		KImage component = kbutton.GetComponent<KImage>();
		component.colorStyleSetting = info.style;
		component.ApplyColorStyleSetting();
		LocText componentInChildren = kbutton.GetComponentInChildren<LocText>();
		componentInChildren.text = info.text;
		componentInChildren.fontSize = (float)info.fontSize;
		return kbutton;
	}

	// Token: 0x06006A4C RID: 27212 RVA: 0x00282F9C File Offset: 0x0028119C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MainMenu._instance = this;
		this.Button_NewGame = this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.NEWGAME, new System.Action(this.NewGame), 22, this.topButtonStyle));
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.LOADGAME, new System.Action(this.LoadGame), 22, this.normalButtonStyle));
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.RETIREDCOLONIES, delegate()
		{
			MainMenu.ActivateRetiredColoniesScreen(this.transform.gameObject, "");
		}, 14, this.normalButtonStyle));
		this.lockerButton = this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.LOCKERMENU, delegate()
		{
			MainMenu.ActivateLockerMenu();
		}, 14, this.normalButtonStyle));
		if (DistributionPlatform.Initialized)
		{
			this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.TRANSLATIONS, new System.Action(this.Translations), 14, this.normalButtonStyle));
			this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MODS.TITLE, new System.Action(this.Mods), 14, this.normalButtonStyle));
		}
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.OPTIONS, new System.Action(this.Options), 14, this.normalButtonStyle));
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.QUITTODESKTOP, new System.Action(this.QuitGame), 14, this.normalButtonStyle));
		this.RefreshResumeButton(false);
		this.Button_ResumeGame.onClick += this.ResumeGame;
		this.SpawnVideoScreen();
		this.StartFEAudio();
		this.CheckPlayerPrefsCorruption();
		if (PatchNotesScreen.ShouldShowScreen())
		{
			global::Util.KInstantiateUI(this.patchNotesScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);
		}
		this.CheckDoubleBoundKeys();
		bool flag = DistributionPlatform.Inst.IsDLCPurchased("EXPANSION1_ID");
		this.expansion1Toggle.gameObject.SetActive(flag);
		if (this.expansion1Ad != null)
		{
			this.expansion1Ad.gameObject.SetActive(!flag);
		}
		this.RefreshDLCLogos();
		this.motd.Setup();
		if (DistributionPlatform.Initialized && DistributionPlatform.Inst.IsPreviousVersionBranch)
		{
			UnityEngine.Object.Instantiate<GameObject>(ScreenPrefabs.Instance.OldVersionWarningScreen, this.uiCanvas.transform);
		}
		string targetExpansion1AdURL = "";
		Sprite sprite = Assets.GetSprite("expansionPromo_en");
		if (DistributionPlatform.Initialized && this.expansion1Ad != null)
		{
			string name = DistributionPlatform.Inst.Name;
			if (!(name == "Steam"))
			{
				if (!(name == "Epic"))
				{
					if (name == "Rail")
					{
						targetExpansion1AdURL = "https://www.wegame.com.cn/store/2001539/";
						sprite = Assets.GetSprite("expansionPromo_cn");
					}
				}
				else
				{
					targetExpansion1AdURL = "https://store.epicgames.com/en-US/p/oxygen-not-included--spaced-out";
				}
			}
			else
			{
				targetExpansion1AdURL = "https://store.steampowered.com/app/1452490/Oxygen_Not_Included__Spaced_Out/";
			}
			this.expansion1Ad.GetComponentInChildren<KButton>().onClick += delegate()
			{
				App.OpenWebURL(targetExpansion1AdURL);
			};
			this.expansion1Ad.GetComponent<HierarchyReferences>().GetReference<Image>("Image").sprite = sprite;
		}
		this.activateOnSpawn = true;
	}

	// Token: 0x06006A4D RID: 27213 RVA: 0x002832C8 File Offset: 0x002814C8
	private void RefreshDLCLogos()
	{
		this.logoDLC1.GetReference<Image>("icon").material = (DlcManager.IsContentSubscribed("EXPANSION1_ID") ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		this.logoDLC2.GetReference<Image>("icon").material = (DlcManager.IsContentSubscribed("DLC2_ID") ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		this.logoDLC3.GetReference<Image>("icon").material = (DlcManager.IsContentSubscribed("DLC3_ID") ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		this.logoDLC4.GetReference<Image>("icon").material = (DlcManager.IsContentSubscribed("DLC4_ID") ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		if (DistributionPlatform.Initialized || Application.isEditor)
		{
			string DLC1_STORE_URL = "";
			string DLC2_STORE_URL = "";
			string DLC3_STORE_URL = "";
			string DLC4_STORE_URL = "";
			string name = DistributionPlatform.Inst.Name;
			if (!(name == "Steam"))
			{
				if (!(name == "Epic"))
				{
					if (name == "Rail")
					{
						DLC1_STORE_URL = "https://www.wegame.com.cn/store/2001539/";
						DLC2_STORE_URL = "https://www.wegame.com.cn/store/2002196/";
						DLC3_STORE_URL = "https://www.wegame.com.cn/store/2002347";
						DLC4_STORE_URL = "https://www.wegame.com.cn/store/2002496";
						this.logoDLC1.GetReference<Image>("icon").sprite = Assets.GetSprite("dlc1_logo_crop_cn");
						this.logoDLC2.GetReference<Image>("icon").sprite = Assets.GetSprite("dlc2_logo_crop_cn");
						this.logoDLC3.GetReference<Image>("icon").sprite = Assets.GetSprite("dlc3_logo_crop_cn");
						this.logoDLC4.GetReference<Image>("icon").sprite = Assets.GetSprite("dlc4_logo_crop_cn");
					}
				}
				else
				{
					DLC1_STORE_URL = "https://store.epicgames.com/en-US/p/oxygen-not-included--spaced-out";
					DLC2_STORE_URL = "https://store.epicgames.com/p/oxygen-not-included-oxygen-not-included-the-frosty-planet-pack-915ba1";
					DLC3_STORE_URL = "https://store.epicgames.com/p/oxygen-not-included-oxygen-not-included-the-bionic-booster-pack-3ba9e9";
					DLC4_STORE_URL = "https://store.epicgames.com/p/oxygen-not-included-oxygen-not-included-the-prehistoric-planet-pack-c14f10";
				}
			}
			else
			{
				DLC1_STORE_URL = "https://store.steampowered.com/app/1452490/Oxygen_Not_Included__Spaced_Out/";
				DLC2_STORE_URL = "https://store.steampowered.com/app/2952300/Oxygen_Not_Included_The_Frosty_Planet_Pack/";
				DLC3_STORE_URL = "https://store.steampowered.com/app/3302470/Oxygen_Not_Included_The_Bionic_Booster_Pack/";
				DLC4_STORE_URL = "https://store.steampowered.com/app/3655420/Oxygen_Not_Included_The_Prehistoric_Planet_Pack/";
			}
			MultiToggle reference = this.logoDLC1.GetReference<MultiToggle>("multitoggle");
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
			{
				if (DlcManager.IsContentOwned("EXPANSION1_ID"))
				{
					this.logoDLC1.GetReference<DLCToggle>("dlctoggle").ToggleExpansion1Cicked();
					return;
				}
				App.OpenWebURL(DLC1_STORE_URL);
			}));
			string text = this.GetDLCStatusString("EXPANSION1_ID", true);
			if (!DlcManager.IsContentOwned("EXPANSION1_ID"))
			{
				text = text + "\n\n" + UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP;
			}
			else
			{
				text = (DlcManager.IsContentSubscribed("EXPANSION1_ID") ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_TOOLTIP : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_TOOLTIP);
			}
			this.logoDLC1.GetReference<ToolTip>("tooltip").SetSimpleTooltip(text);
			MultiToggle reference2 = this.logoDLC2.GetReference<MultiToggle>("multitoggle");
			reference2.onClick = (System.Action)Delegate.Combine(reference2.onClick, new System.Action(delegate()
			{
				App.OpenWebURL(DLC2_STORE_URL);
			}));
			this.logoDLC2.GetReference<LocText>("statuslabel").SetText(this.GetDLCStatusString("DLC2_ID", false));
			string text2 = this.GetDLCStatusString("DLC2_ID", true);
			text2 = text2 + "\n\n" + UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP;
			this.logoDLC2.GetReference<ToolTip>("tooltip").SetSimpleTooltip(text2);
			MultiToggle reference3 = this.logoDLC3.GetReference<MultiToggle>("multitoggle");
			reference3.onClick = (System.Action)Delegate.Combine(reference3.onClick, new System.Action(delegate()
			{
				App.OpenWebURL(DLC3_STORE_URL);
			}));
			this.logoDLC3.GetReference<LocText>("statuslabel").SetText(this.GetDLCStatusString("DLC3_ID", false));
			string text3 = this.GetDLCStatusString("DLC3_ID", true);
			text3 = text3 + "\n\n" + UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP;
			this.logoDLC3.GetReference<ToolTip>("tooltip").SetSimpleTooltip(text3);
			MultiToggle reference4 = this.logoDLC4.GetReference<MultiToggle>("multitoggle");
			reference4.onClick = (System.Action)Delegate.Combine(reference4.onClick, new System.Action(delegate()
			{
				App.OpenWebURL(DLC4_STORE_URL);
			}));
			this.logoDLC4.GetReference<LocText>("statuslabel").SetText(this.GetDLCStatusString("DLC4_ID", false));
			string text4 = this.GetDLCStatusString("DLC4_ID", true);
			text4 = text4 + "\n\n" + UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP;
			this.logoDLC4.GetReference<ToolTip>("tooltip").SetSimpleTooltip(text4);
		}
	}

	// Token: 0x06006A4E RID: 27214 RVA: 0x00283790 File Offset: 0x00281990
	public string GetDLCStatusString(string dlcID, bool tooltip = false)
	{
		if (!DlcManager.IsContentOwned(dlcID))
		{
			return tooltip ? UI.FRONTEND.MAINMENU.DLC.CONTENT_NOTOWNED_TOOLTIP : UI.FRONTEND.MAINMENU.WISHLIST_AD;
		}
		if (DlcManager.IsContentSubscribed(dlcID))
		{
			return tooltip ? UI.FRONTEND.MAINMENU.DLC.CONTENT_ACTIVE_TOOLTIP : UI.FRONTEND.MAINMENU.DLC.CONTENT_INSTALLED_LABEL;
		}
		return tooltip ? UI.FRONTEND.MAINMENU.DLC.CONTENT_OWNED_NOTINSTALLED_TOOLTIP : UI.FRONTEND.MAINMENU.DLC.CONTENT_OWNED_NOTINSTALLED_LABEL;
	}

	// Token: 0x06006A4F RID: 27215 RVA: 0x002837EB File Offset: 0x002819EB
	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			this.RefreshResumeButton(false);
		}
	}

	// Token: 0x06006A50 RID: 27216 RVA: 0x002837F8 File Offset: 0x002819F8
	public override void OnKeyDown(KButtonEvent e)
	{
		base.OnKeyDown(e);
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.DebugToggleUI))
		{
			this.m_screenshotMode = !this.m_screenshotMode;
			this.uiCanvas.alpha = (this.m_screenshotMode ? 0f : 1f);
		}
		KKeyCode key_code;
		switch (this.m_cheatInputCounter)
		{
		case 0:
			key_code = KKeyCode.K;
			break;
		case 1:
			key_code = KKeyCode.L;
			break;
		case 2:
			key_code = KKeyCode.E;
			break;
		case 3:
			key_code = KKeyCode.I;
			break;
		case 4:
			key_code = KKeyCode.P;
			break;
		case 5:
			key_code = KKeyCode.L;
			break;
		case 6:
			key_code = KKeyCode.A;
			break;
		default:
			key_code = KKeyCode.Y;
			break;
		}
		if (e.Controller.GetKeyDown(key_code))
		{
			e.Consumed = true;
			this.m_cheatInputCounter++;
			if (this.m_cheatInputCounter >= 8)
			{
				global::Debug.Log("Cheat Detected - enabling Debug Mode");
				DebugHandler.SetDebugEnabled(true);
				this.buildWatermark.RefreshText();
				this.m_cheatInputCounter = 0;
				return;
			}
		}
		else
		{
			this.m_cheatInputCounter = 0;
		}
	}

	// Token: 0x06006A51 RID: 27217 RVA: 0x002838F7 File Offset: 0x00281AF7
	private void PlayMouseOverSound()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06006A52 RID: 27218 RVA: 0x00283909 File Offset: 0x00281B09
	private void PlayMouseClickSound()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
	}

	// Token: 0x06006A53 RID: 27219 RVA: 0x0028391C File Offset: 0x00281B1C
	protected override void OnSpawn()
	{
		global::Debug.Log("-- MAIN MENU -- ");
		base.OnSpawn();
		this.m_cheatInputCounter = 0;
		Canvas.ForceUpdateCanvases();
		this.ShowLanguageConfirmation();
		this.InitLoadScreen();
		LoadScreen.Instance.ShowMigrationIfNecessary(true);
		string savePrefix = SaveLoader.GetSavePrefix();
		try
		{
			string path = Path.Combine(savePrefix, "__SPCCHK");
			using (FileStream fileStream = File.OpenWrite(path))
			{
				byte[] array = new byte[1024];
				for (int i = 0; i < 15360; i++)
				{
					fileStream.Write(array, 0, array.Length);
				}
			}
			File.Delete(path);
		}
		catch (Exception ex)
		{
			string format;
			if (ex is IOException)
			{
				format = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, savePrefix);
			}
			else
			{
				format = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, savePrefix);
			}
			string text = string.Format(format, savePrefix);
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
		}
		Global.Instance.modManager.Report(base.gameObject);
		if (GenericGameSettings.instance.devBootSmoke && !GenericGameSettings.instance.devBootModReport)
		{
			App.QuitCode(KCrashReporter.hasCrash ? 1 : 0);
		}
		if ((GenericGameSettings.instance.autoResumeGame && !MainMenu.HasAutoresumedOnce && !KCrashReporter.hasCrash) || !string.IsNullOrEmpty(GenericGameSettings.instance.scriptedProfile.saveGame) || KPlayerPrefs.HasKey("AutoResumeSaveFile"))
		{
			MainMenu.HasAutoresumedOnce = true;
			this.ResumeGame();
		}
		if (GenericGameSettings.instance.devAutoWorldGen && !KCrashReporter.hasCrash)
		{
			GenericGameSettings.instance.devAutoWorldGen = false;
			GenericGameSettings.instance.devAutoWorldGenActive = true;
			GenericGameSettings.instance.SaveSettings();
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.WorldGenScreen.gameObject, base.gameObject, true);
		}
		this.RefreshInventoryNotification();
	}

	// Token: 0x06006A54 RID: 27220 RVA: 0x00283B0C File Offset: 0x00281D0C
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	// Token: 0x06006A55 RID: 27221 RVA: 0x00283B14 File Offset: 0x00281D14
	private void RefreshInventoryNotification()
	{
		bool active = PermitItems.HasUnopenedItem();
		this.lockerButton.GetComponent<HierarchyReferences>().GetReference<RectTransform>("AttentionIcon").gameObject.SetActive(active);
	}

	// Token: 0x06006A56 RID: 27222 RVA: 0x00283B47 File Offset: 0x00281D47
	protected override void OnActivate()
	{
		if (!this.ambientLoopEventName.IsNullOrWhiteSpace())
		{
			this.ambientLoop = KFMOD.CreateInstance(GlobalAssets.GetSound(this.ambientLoopEventName, false));
			if (this.ambientLoop.isValid())
			{
				this.ambientLoop.start();
			}
		}
	}

	// Token: 0x06006A57 RID: 27223 RVA: 0x00283B86 File Offset: 0x00281D86
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		this.motd.CleanUp();
	}

	// Token: 0x06006A58 RID: 27224 RVA: 0x00283B9C File Offset: 0x00281D9C
	public override void ScreenUpdate(bool topLevel)
	{
		this.refreshResumeButton = topLevel;
		if (KleiItemDropScreen.Instance != null && KleiItemDropScreen.Instance.gameObject.activeInHierarchy != this.itemDropOpenFlag)
		{
			this.RefreshInventoryNotification();
			this.itemDropOpenFlag = KleiItemDropScreen.Instance.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06006A59 RID: 27225 RVA: 0x00283BEF File Offset: 0x00281DEF
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
		this.StopAmbience();
		this.motd.CleanUp();
	}

	// Token: 0x06006A5A RID: 27226 RVA: 0x00283C08 File Offset: 0x00281E08
	private void ShowLanguageConfirmation()
	{
		if (SteamManager.Initialized)
		{
			if (SteamUtils.GetSteamUILanguage() != "schinese")
			{
				return;
			}
			if (KPlayerPrefs.GetInt("LanguageConfirmationVersion") >= MainMenu.LANGUAGE_CONFIRMATION_VERSION)
			{
				return;
			}
			KPlayerPrefs.SetInt("LanguageConfirmationVersion", MainMenu.LANGUAGE_CONFIRMATION_VERSION);
			this.Translations();
		}
	}

	// Token: 0x06006A5B RID: 27227 RVA: 0x00283C58 File Offset: 0x00281E58
	private void ResumeGame()
	{
		string text;
		if (KPlayerPrefs.HasKey("AutoResumeSaveFile"))
		{
			text = KPlayerPrefs.GetString("AutoResumeSaveFile");
			KPlayerPrefs.DeleteKey("AutoResumeSaveFile");
		}
		else if (!string.IsNullOrEmpty(GenericGameSettings.instance.scriptedProfile.saveGame))
		{
			global::Debug.LogWarning("Scripted Profile run without KPROFILER_ENABLED!");
			text = GenericGameSettings.instance.scriptedProfile.saveGame;
		}
		else
		{
			text = SaveLoader.GetLatestSaveForCurrentDLC();
		}
		if (!string.IsNullOrEmpty(text))
		{
			KCrashReporter.MOST_RECENT_SAVEFILE = text;
			SaveLoader.SetActiveSaveFilePath(text);
			LoadingOverlay.Load(new System.Action(SaveLoader.LoadScene));
		}
	}

	// Token: 0x06006A5C RID: 27228 RVA: 0x00283CE5 File Offset: 0x00281EE5
	private void NewGame()
	{
		WorldGen.WaitForPendingLoadSettings();
		base.GetComponent<NewGameFlow>().BeginFlow();
	}

	// Token: 0x06006A5D RID: 27229 RVA: 0x00283CF7 File Offset: 0x00281EF7
	private void InitLoadScreen()
	{
		if (LoadScreen.Instance == null)
		{
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.LoadScreen.gameObject, base.gameObject, true).GetComponent<LoadScreen>();
		}
	}

	// Token: 0x06006A5E RID: 27230 RVA: 0x00283D27 File Offset: 0x00281F27
	private void LoadGame()
	{
		this.InitLoadScreen();
		LoadScreen.Instance.Activate();
	}

	// Token: 0x06006A5F RID: 27231 RVA: 0x00283D3C File Offset: 0x00281F3C
	public static void ActivateRetiredColoniesScreen(GameObject parent, string colonyID = "")
	{
		if (RetiredColonyInfoScreen.Instance == null)
		{
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, true);
		}
		RetiredColonyInfoScreen.Instance.Show(true);
		if (!string.IsNullOrEmpty(colonyID))
		{
			if (SaveGame.Instance != null)
			{
				RetireColonyUtility.SaveColonySummaryData();
			}
			RetiredColonyInfoScreen.Instance.LoadColony(RetiredColonyInfoScreen.Instance.GetColonyDataByBaseName(colonyID));
		}
	}

	// Token: 0x06006A60 RID: 27232 RVA: 0x00283DA8 File Offset: 0x00281FA8
	public static void ActivateRetiredColoniesScreenFromData(GameObject parent, RetiredColonyData data)
	{
		if (RetiredColonyInfoScreen.Instance == null)
		{
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, true);
		}
		RetiredColonyInfoScreen.Instance.Show(true);
		RetiredColonyInfoScreen.Instance.LoadColony(data);
	}

	// Token: 0x06006A61 RID: 27233 RVA: 0x00283DE4 File Offset: 0x00281FE4
	public static void ActivateInventoyScreen()
	{
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.kleiInventoryScreen, null);
	}

	// Token: 0x06006A62 RID: 27234 RVA: 0x00283DFB File Offset: 0x00281FFB
	public static void ActivateLockerMenu()
	{
		LockerMenuScreen.Instance.Show(true);
	}

	// Token: 0x06006A63 RID: 27235 RVA: 0x00283E08 File Offset: 0x00282008
	private void SpawnVideoScreen()
	{
		VideoScreen.Instance = global::Util.KInstantiateUI(ScreenPrefabs.Instance.VideoScreen.gameObject, base.gameObject, false).GetComponent<VideoScreen>();
	}

	// Token: 0x06006A64 RID: 27236 RVA: 0x00283E2F File Offset: 0x0028202F
	private void Update()
	{
		PerformanceCaptureMonitor.TryRecordMainMenuStats();
	}

	// Token: 0x06006A65 RID: 27237 RVA: 0x00283E38 File Offset: 0x00282038
	public void RefreshResumeButton(bool simpleCheck = false)
	{
		string latestSaveForCurrentDLC = SaveLoader.GetLatestSaveForCurrentDLC();
		bool flag = !string.IsNullOrEmpty(latestSaveForCurrentDLC) && File.Exists(latestSaveForCurrentDLC);
		if (flag)
		{
			try
			{
				if (GenericGameSettings.instance.demoMode)
				{
					flag = false;
				}
				System.DateTime lastWriteTime = File.GetLastWriteTime(latestSaveForCurrentDLC);
				MainMenu.SaveFileEntry saveFileEntry = default(MainMenu.SaveFileEntry);
				SaveGame.Header header = default(SaveGame.Header);
				SaveGame.GameInfo gameInfo = default(SaveGame.GameInfo);
				if (!this.saveFileEntries.TryGetValue(latestSaveForCurrentDLC, out saveFileEntry) || saveFileEntry.timeStamp != lastWriteTime)
				{
					gameInfo = SaveLoader.LoadHeader(latestSaveForCurrentDLC, out header);
					saveFileEntry = new MainMenu.SaveFileEntry
					{
						timeStamp = lastWriteTime,
						header = header,
						headerData = gameInfo
					};
					this.saveFileEntries[latestSaveForCurrentDLC] = saveFileEntry;
				}
				else
				{
					header = saveFileEntry.header;
					gameInfo = saveFileEntry.headerData;
				}
				if (header.buildVersion > 706793U || gameInfo.saveMajorVersion != 7 || gameInfo.saveMinorVersion > 37)
				{
					flag = false;
				}
				HashSet<string> hashSet;
				HashSet<string> hashSet2;
				if (!gameInfo.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2))
				{
					flag = false;
				}
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(latestSaveForCurrentDLC);
				if (!string.IsNullOrEmpty(gameInfo.baseName))
				{
					this.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = string.Format(UI.FRONTEND.MAINMENU.RESUMEBUTTON_BASENAME, gameInfo.baseName, gameInfo.numberOfCycles + 1);
				}
				else
				{
					this.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = fileNameWithoutExtension;
				}
			}
			catch (Exception obj)
			{
				global::Debug.LogWarning(obj);
				flag = false;
			}
		}
		if (this.Button_ResumeGame != null && this.Button_ResumeGame.gameObject != null)
		{
			this.Button_ResumeGame.gameObject.SetActive(flag);
			KImage component = this.Button_NewGame.GetComponent<KImage>();
			component.colorStyleSetting = (flag ? this.normalButtonStyle : this.topButtonStyle);
			component.ApplyColorStyleSetting();
			return;
		}
		global::Debug.LogWarning("Why is the resume game button null?");
	}

	// Token: 0x06006A66 RID: 27238 RVA: 0x00284020 File Offset: 0x00282220
	private void Translations()
	{
		global::Util.KInstantiateUI<LanguageOptionsScreen>(ScreenPrefabs.Instance.languageOptionsScreen.gameObject, base.transform.parent.gameObject, false);
	}

	// Token: 0x06006A67 RID: 27239 RVA: 0x00284048 File Offset: 0x00282248
	private void Mods()
	{
		global::Util.KInstantiateUI<ModsScreen>(ScreenPrefabs.Instance.modsMenu.gameObject, base.transform.parent.gameObject, false);
	}

	// Token: 0x06006A68 RID: 27240 RVA: 0x00284070 File Offset: 0x00282270
	private void Options()
	{
		global::Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, base.gameObject, true);
	}

	// Token: 0x06006A69 RID: 27241 RVA: 0x0028408E File Offset: 0x0028228E
	private void QuitGame()
	{
		App.Quit();
	}

	// Token: 0x06006A6A RID: 27242 RVA: 0x00284098 File Offset: 0x00282298
	public void StartFEAudio()
	{
		AudioMixer.instance.Reset();
		MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.ConfigureSongs();
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSnapshot);
		if (!AudioMixer.instance.SnapshotIsActive(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot))
		{
			AudioMixer.instance.StartUserVolumesSnapshot();
		}
		if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying(this.menuMusicEventName))
		{
			MusicManager.instance.PlaySong(this.menuMusicEventName, false);
		}
		this.CheckForAudioDriverIssue();
	}

	// Token: 0x06006A6B RID: 27243 RVA: 0x0028412E File Offset: 0x0028232E
	public void StopAmbience()
	{
		if (this.ambientLoop.isValid())
		{
			this.ambientLoop.stop(STOP_MODE.ALLOWFADEOUT);
			this.ambientLoop.release();
			this.ambientLoop.clearHandle();
		}
	}

	// Token: 0x06006A6C RID: 27244 RVA: 0x00284161 File Offset: 0x00282361
	public void StopMainMenuMusic()
	{
		if (MusicManager.instance.SongIsPlaying(this.menuMusicEventName))
		{
			MusicManager.instance.StopSong(this.menuMusicEventName, true, STOP_MODE.ALLOWFADEOUT);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06006A6D RID: 27245 RVA: 0x002841A0 File Offset: 0x002823A0
	private void CheckForAudioDriverIssue()
	{
		if (!KFMOD.didFmodInitializeSuccessfully)
		{
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS, null, null, UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS_MORE_INFO, delegate
			{
				App.OpenWebURL("http://support.kleientertainment.com/customer/en/portal/articles/2947881-no-audio-when-playing-oxygen-not-included");
			}, null, null, null, GlobalResources.Instance().sadDupeAudio);
		}
	}

	// Token: 0x06006A6E RID: 27246 RVA: 0x00284218 File Offset: 0x00282418
	private void CheckPlayerPrefsCorruption()
	{
		if (KPlayerPrefs.HasCorruptedFlag())
		{
			KPlayerPrefs.ResetCorruptedFlag();
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(UI.FRONTEND.SUPPORTWARNINGS.PLAYER_PREFS_CORRUPTED, null, null, null, null, null, null, null, GlobalResources.Instance().sadDupe);
		}
	}

	// Token: 0x06006A6F RID: 27247 RVA: 0x0028426C File Offset: 0x0028246C
	private void CheckDoubleBoundKeys()
	{
		string text = "";
		HashSet<BindingEntry> hashSet = new HashSet<BindingEntry>();
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			if (GameInputMapping.KeyBindings[i].mKeyCode != KKeyCode.Mouse1)
			{
				for (int j = 0; j < GameInputMapping.KeyBindings.Length; j++)
				{
					if (i != j)
					{
						BindingEntry bindingEntry = GameInputMapping.KeyBindings[j];
						if (!hashSet.Contains(bindingEntry))
						{
							BindingEntry bindingEntry2 = GameInputMapping.KeyBindings[i];
							if (bindingEntry2.mKeyCode != KKeyCode.None && bindingEntry2.mKeyCode == bindingEntry.mKeyCode && bindingEntry2.mModifier == bindingEntry.mModifier && bindingEntry2.mRebindable && bindingEntry.mRebindable)
							{
								string mGroup = GameInputMapping.KeyBindings[i].mGroup;
								string mGroup2 = GameInputMapping.KeyBindings[j].mGroup;
								if ((mGroup == "Root" || mGroup2 == "Root" || mGroup == mGroup2) && (!(mGroup == "Root") || !bindingEntry.mIgnoreRootConflics) && (!(mGroup2 == "Root") || !bindingEntry2.mIgnoreRootConflics))
								{
									text = string.Concat(new string[]
									{
										text,
										"\n\n",
										bindingEntry2.mAction.ToString(),
										": <b>",
										bindingEntry2.mKeyCode.ToString(),
										"</b>\n",
										bindingEntry.mAction.ToString(),
										": <b>",
										bindingEntry.mKeyCode.ToString(),
										"</b>"
									});
									BindingEntry bindingEntry3 = bindingEntry2;
									bindingEntry3.mKeyCode = KKeyCode.None;
									bindingEntry3.mModifier = Modifier.None;
									GameInputMapping.KeyBindings[i] = bindingEntry3;
									bindingEntry3 = bindingEntry;
									bindingEntry3.mKeyCode = KKeyCode.None;
									bindingEntry3.mModifier = Modifier.None;
									GameInputMapping.KeyBindings[j] = bindingEntry3;
								}
							}
						}
					}
				}
				hashSet.Add(GameInputMapping.KeyBindings[i]);
			}
		}
		if (text != "")
		{
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(string.Format(UI.FRONTEND.SUPPORTWARNINGS.DUPLICATE_KEY_BINDINGS, text), null, null, null, null, null, null, null, GlobalResources.Instance().sadDupe);
		}
	}

	// Token: 0x06006A70 RID: 27248 RVA: 0x002844F9 File Offset: 0x002826F9
	private void RestartGame()
	{
		App.instance.Restart();
	}

	// Token: 0x0400491A RID: 18714
	private static MainMenu _instance;

	// Token: 0x0400491B RID: 18715
	public KButton Button_ResumeGame;

	// Token: 0x0400491C RID: 18716
	private KButton Button_NewGame;

	// Token: 0x0400491D RID: 18717
	private GameObject GameSettingsScreen;

	// Token: 0x0400491E RID: 18718
	private bool m_screenshotMode;

	// Token: 0x0400491F RID: 18719
	[SerializeField]
	private CanvasGroup uiCanvas;

	// Token: 0x04004920 RID: 18720
	[SerializeField]
	private KButton buttonPrefab;

	// Token: 0x04004921 RID: 18721
	[SerializeField]
	private GameObject buttonParent;

	// Token: 0x04004922 RID: 18722
	[SerializeField]
	private ColorStyleSetting topButtonStyle;

	// Token: 0x04004923 RID: 18723
	[SerializeField]
	private ColorStyleSetting normalButtonStyle;

	// Token: 0x04004924 RID: 18724
	[SerializeField]
	private string menuMusicEventName;

	// Token: 0x04004925 RID: 18725
	[SerializeField]
	private string ambientLoopEventName;

	// Token: 0x04004926 RID: 18726
	private EventInstance ambientLoop;

	// Token: 0x04004927 RID: 18727
	[SerializeField]
	private MainMenu_Motd motd;

	// Token: 0x04004928 RID: 18728
	[SerializeField]
	private PatchNotesScreen patchNotesScreenPrefab;

	// Token: 0x04004929 RID: 18729
	[SerializeField]
	private NextUpdateTimer nextUpdateTimer;

	// Token: 0x0400492A RID: 18730
	[SerializeField]
	private DLCToggle expansion1Toggle;

	// Token: 0x0400492B RID: 18731
	[SerializeField]
	private GameObject expansion1Ad;

	// Token: 0x0400492C RID: 18732
	[SerializeField]
	private BuildWatermark buildWatermark;

	// Token: 0x0400492D RID: 18733
	[SerializeField]
	public string IntroShortName;

	// Token: 0x0400492E RID: 18734
	[SerializeField]
	private HierarchyReferences logoDLC1;

	// Token: 0x0400492F RID: 18735
	[SerializeField]
	private HierarchyReferences logoDLC2;

	// Token: 0x04004930 RID: 18736
	[SerializeField]
	private HierarchyReferences logoDLC3;

	// Token: 0x04004931 RID: 18737
	[SerializeField]
	private HierarchyReferences logoDLC4;

	// Token: 0x04004932 RID: 18738
	private KButton lockerButton;

	// Token: 0x04004933 RID: 18739
	private bool itemDropOpenFlag;

	// Token: 0x04004934 RID: 18740
	private static bool HasAutoresumedOnce = false;

	// Token: 0x04004935 RID: 18741
	private bool refreshResumeButton = true;

	// Token: 0x04004936 RID: 18742
	private int m_cheatInputCounter;

	// Token: 0x04004937 RID: 18743
	public const string AutoResumeSaveFileKey = "AutoResumeSaveFile";

	// Token: 0x04004938 RID: 18744
	public const string PLAY_SHORT_ON_LAUNCH = "PlayShortOnLaunch";

	// Token: 0x04004939 RID: 18745
	private static int LANGUAGE_CONFIRMATION_VERSION = 2;

	// Token: 0x0400493A RID: 18746
	private Dictionary<string, MainMenu.SaveFileEntry> saveFileEntries = new Dictionary<string, MainMenu.SaveFileEntry>();

	// Token: 0x02001F9A RID: 8090
	private struct ButtonInfo
	{
		// Token: 0x0600B6D9 RID: 46809 RVA: 0x003F1A86 File Offset: 0x003EFC86
		public ButtonInfo(LocString text, System.Action action, int font_size, ColorStyleSetting style)
		{
			this.text = text;
			this.action = action;
			this.fontSize = font_size;
			this.style = style;
		}

		// Token: 0x04009358 RID: 37720
		public LocString text;

		// Token: 0x04009359 RID: 37721
		public System.Action action;

		// Token: 0x0400935A RID: 37722
		public int fontSize;

		// Token: 0x0400935B RID: 37723
		public ColorStyleSetting style;
	}

	// Token: 0x02001F9B RID: 8091
	private struct SaveFileEntry
	{
		// Token: 0x0400935C RID: 37724
		public System.DateTime timeStamp;

		// Token: 0x0400935D RID: 37725
		public SaveGame.Header header;

		// Token: 0x0400935E RID: 37726
		public SaveGame.GameInfo headerData;
	}
}
