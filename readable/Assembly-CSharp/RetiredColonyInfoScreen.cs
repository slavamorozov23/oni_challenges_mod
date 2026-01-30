using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF6 RID: 3574
public class RetiredColonyInfoScreen : KModalScreen
{
	// Token: 0x060070D9 RID: 28889 RVA: 0x002AFB80 File Offset: 0x002ADD80
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		RetiredColonyInfoScreen.Instance = this;
		this.ConfigButtons();
		this.LoadExplorer();
		this.PopulateAchievements();
		base.ConsumeMouseScroll = true;
		this.explorerSearch.text = "";
		this.explorerSearch.onValueChanged.AddListener(delegate(string value)
		{
			if (this.colonyDataRoot.activeSelf)
			{
				this.FilterColonyData(this.explorerSearch.text);
				return;
			}
			this.FilterExplorer(this.explorerSearch.text);
		});
		this.clearExplorerSearchButton.onClick += delegate()
		{
			this.explorerSearch.text = "";
		};
		this.achievementSearch.text = "";
		this.achievementSearch.onValueChanged.AddListener(delegate(string value)
		{
			this.FilterAchievements(this.achievementSearch.text);
		});
		this.clearAchievementSearchButton.onClick += delegate()
		{
			this.achievementSearch.text = "";
		};
		this.RefreshUIScale(null);
		base.Subscribe(-810220474, new Action<object>(this.RefreshUIScale));
	}

	// Token: 0x060070DA RID: 28890 RVA: 0x002AFC57 File Offset: 0x002ADE57
	private void RefreshUIScale(object data = null)
	{
		base.StartCoroutine(this.DelayedRefreshScale());
	}

	// Token: 0x060070DB RID: 28891 RVA: 0x002AFC66 File Offset: 0x002ADE66
	private IEnumerator DelayedRefreshScale()
	{
		int num;
		for (int i = 0; i < 3; i = num + 1)
		{
			yield return 0;
			num = i;
		}
		float num2 = 36f;
		if (GameObject.Find("ScreenSpaceOverlayCanvas") != null)
		{
			this.explorerRoot.transform.parent.localScale = Vector3.one * ((this.colonyScroll.rectTransform().rect.width - num2) / this.explorerRoot.transform.parent.rectTransform().rect.width);
		}
		else
		{
			this.explorerRoot.transform.parent.localScale = Vector3.one * ((this.colonyScroll.rectTransform().rect.width - num2) / this.explorerRoot.transform.parent.rectTransform().rect.width);
		}
		yield break;
	}

	// Token: 0x060070DC RID: 28892 RVA: 0x002AFC78 File Offset: 0x002ADE78
	private void ConfigButtons()
	{
		this.closeButton.ClearOnClick();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.viewOtherColoniesButton.ClearOnClick();
		this.viewOtherColoniesButton.onClick += delegate()
		{
			this.ToggleExplorer(true);
		};
		this.quitToMainMenuButton.ClearOnClick();
		this.quitToMainMenuButton.onClick += delegate()
		{
			this.ConfirmDecision(UI.FRONTEND.MAINMENU.QUITCONFIRM, new System.Action(this.OnQuitConfirm));
		};
		this.closeScreenButton.ClearOnClick();
		this.closeScreenButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.viewOtherColoniesButton.gameObject.SetActive(false);
		if (Game.Instance != null)
		{
			this.closeScreenButton.gameObject.SetActive(true);
			this.closeScreenButton.GetComponentInChildren<LocText>().SetText(UI.RETIRED_COLONY_INFO_SCREEN.BUTTONS.RETURN_TO_GAME);
			this.quitToMainMenuButton.gameObject.SetActive(true);
			return;
		}
		this.closeScreenButton.gameObject.SetActive(true);
		this.closeScreenButton.GetComponentInChildren<LocText>().SetText(UI.RETIRED_COLONY_INFO_SCREEN.BUTTONS.CLOSE);
		this.quitToMainMenuButton.gameObject.SetActive(false);
	}

	// Token: 0x060070DD RID: 28893 RVA: 0x002AFDA4 File Offset: 0x002ADFA4
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (!show && Game.Instance != null)
		{
			Game.Instance.Trigger(-821118536, this);
		}
	}

	// Token: 0x060070DE RID: 28894 RVA: 0x002AFDD0 File Offset: 0x002ADFD0
	private void ConfirmDecision(string text, System.Action onConfirm)
	{
		base.gameObject.SetActive(false);
		((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(text, onConfirm, new System.Action(this.OnCancelPopup), null, null, null, null, null, null);
	}

	// Token: 0x060070DF RID: 28895 RVA: 0x002AFE31 File Offset: 0x002AE031
	private void OnCancelPopup()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x060070E0 RID: 28896 RVA: 0x002AFE3F File Offset: 0x002AE03F
	private void OnQuitConfirm()
	{
		LoadingOverlay.Load(delegate
		{
			this.Deactivate();
			PauseScreen.TriggerQuitGame();
		});
	}

	// Token: 0x060070E1 RID: 28897 RVA: 0x002AFE52 File Offset: 0x002AE052
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.GetCanvasRef();
		this.wasPixelPerfect = this.canvasRef.pixelPerfect;
		this.canvasRef.pixelPerfect = false;
	}

	// Token: 0x060070E2 RID: 28898 RVA: 0x002AFE7D File Offset: 0x002AE07D
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			this.Show(false);
		}
		else if (e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060070E3 RID: 28899 RVA: 0x002AFEB4 File Offset: 0x002AE0B4
	private void GetCanvasRef()
	{
		if (base.transform.parent.GetComponent<Canvas>() != null)
		{
			this.canvasRef = base.transform.parent.GetComponent<Canvas>();
			return;
		}
		this.canvasRef = base.transform.parent.parent.GetComponent<Canvas>();
	}

	// Token: 0x060070E4 RID: 28900 RVA: 0x002AFF0B File Offset: 0x002AE10B
	protected override void OnCmpDisable()
	{
		this.canvasRef.pixelPerfect = this.wasPixelPerfect;
		base.OnCmpDisable();
	}

	// Token: 0x060070E5 RID: 28901 RVA: 0x002AFF24 File Offset: 0x002AE124
	public RetiredColonyData GetColonyDataByBaseName(string name)
	{
		name = RetireColonyUtility.StripInvalidCharacters(name);
		for (int i = 0; i < this.retiredColonyData.Length; i++)
		{
			if (RetireColonyUtility.StripInvalidCharacters(this.retiredColonyData[i].colonyName) == name)
			{
				return this.retiredColonyData[i];
			}
		}
		return null;
	}

	// Token: 0x060070E6 RID: 28902 RVA: 0x002AFF70 File Offset: 0x002AE170
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.explorerSearch.text = "";
			this.achievementSearch.text = "";
			this.RefreshUIScale(null);
		}
		else
		{
			this.InstantClearAchievementVeils();
		}
		if (Game.Instance != null)
		{
			if (!show)
			{
				if (MusicManager.instance.SongIsPlaying("Music_Victory_03_StoryAndSummary"))
				{
					MusicManager.instance.StopSong("Music_Victory_03_StoryAndSummary", true, STOP_MODE.ALLOWFADEOUT);
				}
			}
			else
			{
				this.retiredColonyData = RetireColonyUtility.LoadRetiredColonies(true);
				if (MusicManager.instance.SongIsPlaying("Music_Victory_03_StoryAndSummary"))
				{
					MusicManager.instance.SetSongParameter("Music_Victory_03_StoryAndSummary", "songSection", 2f, true);
				}
			}
		}
		else if (Game.Instance == null)
		{
			this.ToggleExplorer(true);
		}
		this.disabledPlatformUnlocks.SetActive(SaveGame.Instance != null);
		if (SaveGame.Instance != null)
		{
			this.disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("enabled").gameObject.SetActive(!DebugHandler.InstantBuildMode && !SaveGame.Instance.sandboxEnabled && !Game.Instance.debugWasUsed);
			this.disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("disabled").gameObject.SetActive(DebugHandler.InstantBuildMode || SaveGame.Instance.sandboxEnabled || Game.Instance.debugWasUsed);
		}
	}

	// Token: 0x060070E7 RID: 28903 RVA: 0x002B00E0 File Offset: 0x002AE2E0
	public void LoadColony(RetiredColonyData data)
	{
		this.colonyName.text = data.colonyName.ToUpper();
		this.cycleCount.text = string.Format(UI.RETIRED_COLONY_INFO_SCREEN.CYCLE_COUNT, data.cycleCount.ToString());
		this.focusedWorld = data.startWorld;
		this.ToggleExplorer(false);
		this.RefreshUIScale(null);
		if (Game.Instance == null)
		{
			this.viewOtherColoniesButton.gameObject.SetActive(true);
		}
		this.ClearColony();
		if (SaveGame.Instance != null)
		{
			ColonyAchievementTracker component = SaveGame.Instance.GetComponent<ColonyAchievementTracker>();
			this.UpdateAchievementData(data, component.achievementsToDisplay.ToArray());
			component.ClearDisplayAchievements();
			this.PopulateAchievementProgress(component);
		}
		else
		{
			this.UpdateAchievementData(data, null);
		}
		this.DisplayStatistics(data);
		this.colonyDataRoot.transform.parent.rectTransform().SetPosition(new Vector3(this.colonyDataRoot.transform.parent.rectTransform().position.x, 0f, 0f));
	}

	// Token: 0x060070E8 RID: 28904 RVA: 0x002B01FC File Offset: 0x002AE3FC
	private void PopulateAchievementProgress(ColonyAchievementTracker tracker)
	{
		if (tracker != null)
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.achievementEntries)
			{
				ColonyAchievementStatus colonyAchievementStatus;
				tracker.achievements.TryGetValue(keyValuePair.Key, out colonyAchievementStatus);
				if (colonyAchievementStatus != null)
				{
					AchievementWidget component = keyValuePair.Value.GetComponent<AchievementWidget>();
					if (component != null)
					{
						component.ShowProgress(colonyAchievementStatus);
						if (colonyAchievementStatus.failed)
						{
							component.SetFailed();
						}
					}
				}
			}
		}
	}

	// Token: 0x060070E9 RID: 28905 RVA: 0x002B0294 File Offset: 0x002AE494
	private bool LoadSlideshow(RetiredColonyData data)
	{
		this.clearCurrentSlideshow();
		this.currentSlideshowFiles = RetireColonyUtility.LoadColonySlideshowFiles(data.colonyName, this.focusedWorld);
		this.slideshow.SetFiles(this.currentSlideshowFiles, -1);
		return this.currentSlideshowFiles != null && this.currentSlideshowFiles.Length != 0;
	}

	// Token: 0x060070EA RID: 28906 RVA: 0x002B02E4 File Offset: 0x002AE4E4
	private void clearCurrentSlideshow()
	{
		this.currentSlideshowFiles = new string[0];
	}

	// Token: 0x060070EB RID: 28907 RVA: 0x002B02F4 File Offset: 0x002AE4F4
	private bool LoadScreenshot(RetiredColonyData data, string world)
	{
		this.clearCurrentSlideshow();
		Sprite sprite = RetireColonyUtility.LoadRetiredColonyPreview(data.colonyName, world);
		if (sprite != null)
		{
			this.slideshow.setSlide(sprite);
			this.CorrectTimelapseImageSize(sprite);
		}
		return sprite != null;
	}

	// Token: 0x060070EC RID: 28908 RVA: 0x002B0338 File Offset: 0x002AE538
	private void ClearColony()
	{
		foreach (GameObject obj in this.activeColonyWidgetContainers)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.activeColonyWidgetContainers.Clear();
		this.activeColonyWidgets.Clear();
		this.UpdateAchievementData(null, null);
	}

	// Token: 0x060070ED RID: 28909 RVA: 0x002B03A8 File Offset: 0x002AE5A8
	private bool IsAchievementValidForDLCContext(IHasDlcRestrictions restrictions, string clusterTag)
	{
		return DlcManager.IsCorrectDlcSubscribed(restrictions) && (!(SaveLoader.Instance != null) || ((clusterTag == null || CustomGameSettings.Instance.GetCurrentClusterLayout().clusterTags.Contains(clusterTag)) && Game.IsCorrectDlcActiveForCurrentSave(restrictions)));
	}

	// Token: 0x060070EE RID: 28910 RVA: 0x002B03E8 File Offset: 0x002AE5E8
	private void PopulateAchievements()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (this.IsAchievementValidForDLCContext(colonyAchievement, null))
			{
				GameObject gameObject = global::Util.KInstantiateUI(colonyAchievement.isVictoryCondition ? this.victoryAchievementsPrefab : this.achievementsPrefab, this.achievementsContainer, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("nameLabel").SetText(colonyAchievement.Name);
				component.GetReference<LocText>("descriptionLabel").SetText(colonyAchievement.description);
				if (string.IsNullOrEmpty(colonyAchievement.icon) || Assets.GetSprite(colonyAchievement.icon) == null)
				{
					if (Assets.GetSprite(colonyAchievement.Name) != null)
					{
						component.GetReference<Image>("icon").sprite = Assets.GetSprite(colonyAchievement.Name);
					}
					else
					{
						component.GetReference<Image>("icon").sprite = Assets.GetSprite("check");
					}
				}
				else
				{
					component.GetReference<Image>("icon").sprite = Assets.GetSprite(colonyAchievement.icon);
				}
				if (colonyAchievement.isVictoryCondition)
				{
					gameObject.transform.SetAsFirstSibling();
				}
				KImage reference = component.GetReference<KImage>("dlc_overlay");
				if (DlcManager.IsDlcId(colonyAchievement.dlcIdFrom))
				{
					reference.gameObject.SetActive(true);
					reference.sprite = Assets.GetSprite(DlcManager.GetDlcBanner(colonyAchievement.dlcIdFrom));
					reference.color = DlcManager.GetDlcBannerColor(colonyAchievement.dlcIdFrom);
				}
				else
				{
					reference.gameObject.SetActive(false);
				}
				gameObject.GetComponent<MultiToggle>().ChangeState(2);
				gameObject.GetComponent<AchievementWidget>().dlcIdFrom = colonyAchievement.dlcIdFrom;
				this.achievementEntries.Add(colonyAchievement.Id, gameObject);
			}
		}
		this.UpdateAchievementData(null, null);
	}

	// Token: 0x060070EF RID: 28911 RVA: 0x002B0600 File Offset: 0x002AE800
	private void InstantClearAchievementVeils()
	{
		GameObject[] array = this.achievementVeils;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
		}
		array = this.achievementVeils;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.achievementEntries)
		{
			AchievementWidget component = keyValuePair.Value.GetComponent<AchievementWidget>();
			component.StopAllCoroutines();
			component.CompleteFlourish();
		}
	}

	// Token: 0x060070F0 RID: 28912 RVA: 0x002B06BC File Offset: 0x002AE8BC
	private IEnumerator ClearAchievementVeil(float delay = 0f)
	{
		yield return new WaitForSecondsRealtime(delay);
		for (float i = 0.7f; i >= 0f; i -= Time.unscaledDeltaTime)
		{
			GameObject[] array = this.achievementVeils;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].GetComponent<Image>().color = new Color(0f, 0f, 0f, i);
			}
			yield return 0;
		}
		this.InstantClearAchievementVeils();
		yield break;
	}

	// Token: 0x060070F1 RID: 28913 RVA: 0x002B06D2 File Offset: 0x002AE8D2
	private IEnumerator ShowAchievementVeil()
	{
		float targetAlpha = 0.7f;
		GameObject[] array = this.achievementVeils;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetActive(true);
		}
		for (float i = 0f; i <= targetAlpha; i += Time.unscaledDeltaTime)
		{
			array = this.achievementVeils;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].GetComponent<Image>().color = new Color(0f, 0f, 0f, i);
			}
			yield return 0;
		}
		for (float num = 0f; num <= targetAlpha; num += Time.unscaledDeltaTime)
		{
			array = this.achievementVeils;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].GetComponent<Image>().color = new Color(0f, 0f, 0f, targetAlpha);
			}
		}
		yield break;
	}

	// Token: 0x060070F2 RID: 28914 RVA: 0x002B06E4 File Offset: 0x002AE8E4
	private void UpdateAchievementData(RetiredColonyData data, string[] newlyAchieved = null)
	{
		int num = 0;
		float num2 = 2f;
		float num3 = 1f;
		if (newlyAchieved != null && newlyAchieved.Length != 0)
		{
			this.retiredColonyData = RetireColonyUtility.LoadRetiredColonies(true);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.achievementEntries)
		{
			bool flag = false;
			bool flag2 = false;
			if (data != null)
			{
				string[] achievements = data.achievements;
				for (int i = 0; i < achievements.Length; i++)
				{
					if (achievements[i] == keyValuePair.Key)
					{
						flag = true;
						break;
					}
				}
			}
			ColonyAchievement colonyAchievement = Db.Get().ColonyAchievements.TryGet(keyValuePair.Key);
			if (colonyAchievement != null && !this.IsAchievementValidForDLCContext(colonyAchievement, colonyAchievement.clusterTag))
			{
				keyValuePair.Value.SetActive(false);
			}
			else
			{
				keyValuePair.Value.SetActive(true);
			}
			if (!(Game.Instance != null) || colonyAchievement.IsValidForSave())
			{
				if (!flag && data == null && this.retiredColonyData != null)
				{
					RetiredColonyData[] array = this.retiredColonyData;
					for (int i = 0; i < array.Length; i++)
					{
						string[] achievements = array[i].achievements;
						for (int j = 0; j < achievements.Length; j++)
						{
							if (achievements[j] == keyValuePair.Key)
							{
								flag2 = true;
							}
						}
					}
				}
				bool flag3 = false;
				if (newlyAchieved != null)
				{
					for (int k = 0; k < newlyAchieved.Length; k++)
					{
						if (newlyAchieved[k] == keyValuePair.Key)
						{
							flag3 = true;
						}
					}
				}
				if (flag || flag3)
				{
					if (flag3)
					{
						keyValuePair.Value.GetComponent<AchievementWidget>().ActivateNewlyAchievedFlourish(num3 + (float)num * num2);
						num++;
					}
					else
					{
						keyValuePair.Value.GetComponent<AchievementWidget>().SetAchievedNow();
					}
				}
				else if (flag2)
				{
					keyValuePair.Value.GetComponent<AchievementWidget>().SetAchievedBefore();
				}
				else if (data == null)
				{
					keyValuePair.Value.GetComponent<AchievementWidget>().SetNeverAchieved();
				}
				else
				{
					keyValuePair.Value.GetComponent<AchievementWidget>().SetNotAchieved();
				}
			}
		}
		if (newlyAchieved != null && newlyAchieved.Length != 0)
		{
			base.StartCoroutine(this.ShowAchievementVeil());
			base.StartCoroutine(this.ClearAchievementVeil(num3 + (float)num * num2));
			return;
		}
		this.InstantClearAchievementVeils();
	}

	// Token: 0x060070F3 RID: 28915 RVA: 0x002B0938 File Offset: 0x002AEB38
	private void DisplayInfoBlock(RetiredColonyData data, GameObject container)
	{
		container.GetComponent<HierarchyReferences>().GetReference<LocText>("ColonyNameLabel").SetText(data.colonyName);
		container.GetComponent<HierarchyReferences>().GetReference<LocText>("CycleCountLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.CYCLE_COUNT, data.cycleCount.ToString()));
	}

	// Token: 0x060070F4 RID: 28916 RVA: 0x002B0994 File Offset: 0x002AEB94
	private void CorrectTimelapseImageSize(Sprite sprite)
	{
		Vector2 sizeDelta = this.slideshow.transform.parent.GetComponent<RectTransform>().sizeDelta;
		Vector2 fittedSize = this.slideshow.GetFittedSize(sprite, sizeDelta.x, sizeDelta.y);
		LayoutElement component = this.slideshow.GetComponent<LayoutElement>();
		if (fittedSize.y > component.preferredHeight)
		{
			component.minHeight = component.preferredHeight / (fittedSize.y / fittedSize.x);
			component.minHeight = component.preferredHeight;
			return;
		}
		component.minWidth = (component.preferredWidth = fittedSize.x);
		component.minHeight = (component.preferredHeight = fittedSize.y);
	}

	// Token: 0x060070F5 RID: 28917 RVA: 0x002B0A40 File Offset: 0x002AEC40
	private void DisplayTimelapse(RetiredColonyData data, GameObject container)
	{
		container.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.RETIRED_COLONY_INFO_SCREEN.TITLES.TIMELAPSE);
		RectTransform reference = container.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Worlds");
		this.DisplayWorlds(data, reference.gameObject);
		RectTransform reference2 = container.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PlayIcon");
		this.slideshow = container.GetComponent<HierarchyReferences>().GetReference<Slideshow>("Slideshow");
		this.slideshow.updateType = SlideshowUpdateType.loadOnDemand;
		this.slideshow.SetPaused(true);
		this.slideshow.onBeforePlay = delegate()
		{
			this.LoadSlideshow(data);
		};
		this.slideshow.onEndingPlay = delegate()
		{
			this.LoadScreenshot(data, this.focusedWorld);
		};
		if (!this.LoadScreenshot(data, this.focusedWorld))
		{
			this.slideshow.gameObject.SetActive(false);
			reference2.gameObject.SetActive(false);
			return;
		}
		this.slideshow.gameObject.SetActive(true);
		reference2.gameObject.SetActive(true);
	}

	// Token: 0x060070F6 RID: 28918 RVA: 0x002B0B60 File Offset: 0x002AED60
	private void DisplayDuplicants(RetiredColonyData data, GameObject container, int range_min = -1, int range_max = -1)
	{
		for (int i = container.transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.DestroyImmediate(container.transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < data.Duplicants.Length; j++)
		{
			if (j < range_min || (j > range_max && range_max != -1))
			{
				new GameObject().transform.SetParent(container.transform);
			}
			else
			{
				RetiredColonyData.RetiredDuplicantData retiredDuplicantData = data.Duplicants[j];
				GameObject gameObject = global::Util.KInstantiateUI(this.duplicantPrefab, container, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("NameLabel").SetText(retiredDuplicantData.name);
				component.GetReference<LocText>("AgeLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.DUPLICANT_AGE, retiredDuplicantData.age.ToString()));
				component.GetReference<LocText>("SkillLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.SKILL_LEVEL, retiredDuplicantData.skillPointsGained.ToString()));
				SymbolOverrideController reference = component.GetReference<SymbolOverrideController>("SymbolOverrideController");
				reference.RemoveAllSymbolOverrides(0);
				KBatchedAnimController componentInChildren = gameObject.GetComponentInChildren<KBatchedAnimController>();
				componentInChildren.SetSymbolVisiblity("snapTo_neck", false);
				componentInChildren.SetSymbolVisiblity("snapTo_goggles", false);
				componentInChildren.SetSymbolVisiblity("snapTo_hat", false);
				componentInChildren.SetSymbolVisiblity("snapTo_headfx", false);
				componentInChildren.SetSymbolVisiblity("snapTo_hat_hair", false);
				foreach (KeyValuePair<string, string> keyValuePair in retiredDuplicantData.accessories)
				{
					if (Db.Get().Accessories.Exists(keyValuePair.Value))
					{
						KAnim.Build.Symbol symbol = Db.Get().Accessories.Get(keyValuePair.Value).symbol;
						AccessorySlot accessorySlot = Db.Get().AccessorySlots.Get(keyValuePair.Key);
						reference.AddSymbolOverride(accessorySlot.targetSymbolId, symbol, 0);
						gameObject.GetComponentInChildren<KBatchedAnimController>().SetSymbolVisiblity(keyValuePair.Key, true);
					}
				}
				reference.ApplyOverrides();
			}
		}
		base.StartCoroutine(this.ActivatePortraitsWhenReady(container));
	}

	// Token: 0x060070F7 RID: 28919 RVA: 0x002B0DAC File Offset: 0x002AEFAC
	private IEnumerator ActivatePortraitsWhenReady(GameObject container)
	{
		yield return 0;
		if (container == null)
		{
			global::Debug.LogError("RetiredColonyInfoScreen minion container is null");
		}
		else
		{
			for (int i = 0; i < container.transform.childCount; i++)
			{
				KBatchedAnimController componentInChildren = container.transform.GetChild(i).GetComponentInChildren<KBatchedAnimController>();
				if (componentInChildren != null)
				{
					componentInChildren.transform.localScale = Vector3.one;
				}
			}
		}
		yield break;
	}

	// Token: 0x060070F8 RID: 28920 RVA: 0x002B0DBC File Offset: 0x002AEFBC
	private void DisplayBuildings(RetiredColonyData data, GameObject container)
	{
		for (int i = container.transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(container.transform.GetChild(i).gameObject);
		}
		data.buildings.Sort(delegate(global::Tuple<string, int> a, global::Tuple<string, int> b)
		{
			if (a.second > b.second)
			{
				return 1;
			}
			if (a.second == b.second)
			{
				return 0;
			}
			return -1;
		});
		data.buildings.Reverse();
		foreach (global::Tuple<string, int> tuple in data.buildings)
		{
			GameObject prefab = Assets.GetPrefab(tuple.first);
			if (!(prefab == null))
			{
				HierarchyReferences component = global::Util.KInstantiateUI(this.buildingPrefab, container, true).GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("NameLabel").SetText(GameUtil.ApplyBoldString(prefab.GetProperName()));
				component.GetReference<LocText>("CountLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.BUILDING_COUNT, tuple.second.ToString()));
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(prefab, "ui", false);
				component.GetReference<Image>("Portrait").sprite = uisprite.first;
			}
		}
	}

	// Token: 0x060070F9 RID: 28921 RVA: 0x002B0F08 File Offset: 0x002AF108
	private void DisplayWorlds(RetiredColonyData data, GameObject container)
	{
		container.SetActive(data.worldIdentities.Count > 0);
		for (int i = container.transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(container.transform.GetChild(i).gameObject);
		}
		if (data.worldIdentities.Count <= 0)
		{
			return;
		}
		using (Dictionary<string, string>.Enumerator enumerator = data.worldIdentities.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> worldPair = enumerator.Current;
				GameObject gameObject = global::Util.KInstantiateUI(this.worldPrefab, container, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				ProcGen.World worldData = SettingsCache.worlds.GetWorldData(worldPair.Value);
				Sprite sprite = (worldData != null) ? ColonyDestinationAsteroidBeltData.GetUISprite(worldData.asteroidIcon) : null;
				if (sprite != null)
				{
					component.GetReference<Image>("Portrait").sprite = sprite;
				}
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.focusedWorld = worldPair.Key;
					this.LoadScreenshot(data, this.focusedWorld);
				};
			}
		}
	}

	// Token: 0x060070FA RID: 28922 RVA: 0x002B1054 File Offset: 0x002AF254
	private IEnumerator ComputeSizeStatGrid()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		GridLayoutGroup component = this.statsContainer.GetComponent<GridLayoutGroup>();
		component.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		component.constraintCount = ((Screen.width < 1920) ? 2 : 3);
		yield return SequenceUtil.WaitForEndOfFrame;
		float num = base.gameObject.rectTransform().rect.width - this.explorerRoot.transform.parent.rectTransform().rect.width - 50f;
		num = Mathf.Min(830f, num);
		this.achievementsSection.GetComponent<LayoutElement>().preferredWidth = num;
		yield break;
	}

	// Token: 0x060070FB RID: 28923 RVA: 0x002B1063 File Offset: 0x002AF263
	private IEnumerator ComputeSizeExplorerGrid()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		GridLayoutGroup component = this.explorerGrid.GetComponent<GridLayoutGroup>();
		component.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		component.constraintCount = ((Screen.width < 1920) ? 2 : 3);
		yield return SequenceUtil.WaitForEndOfFrame;
		float num = base.gameObject.rectTransform().rect.width - this.explorerRoot.transform.parent.rectTransform().rect.width - 50f;
		num = Mathf.Min(830f, num);
		this.achievementsSection.GetComponent<LayoutElement>().preferredWidth = num;
		yield break;
	}

	// Token: 0x060070FC RID: 28924 RVA: 0x002B1074 File Offset: 0x002AF274
	private void DisplayStatistics(RetiredColonyData data)
	{
		GameObject gameObject = global::Util.KInstantiateUI(this.specialMediaBlock, this.statsContainer, true);
		this.activeColonyWidgetContainers.Add(gameObject);
		this.activeColonyWidgets.Add("timelapse", gameObject);
		this.DisplayTimelapse(data, gameObject);
		GameObject duplicantBlock = global::Util.KInstantiateUI(this.tallFeatureBlock, this.statsContainer, true);
		this.activeColonyWidgetContainers.Add(duplicantBlock);
		this.activeColonyWidgets.Add("duplicants", duplicantBlock);
		duplicantBlock.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.RETIRED_COLONY_INFO_SCREEN.TITLES.DUPLICANTS);
		PageView pageView = duplicantBlock.GetComponentInChildren<PageView>();
		pageView.OnChangePage = delegate(int page)
		{
			this.DisplayDuplicants(data, duplicantBlock.GetComponent<HierarchyReferences>().GetReference("Content").gameObject, page * pageView.ChildrenPerPage, (page + 1) * pageView.ChildrenPerPage);
		};
		this.DisplayDuplicants(data, duplicantBlock.GetComponent<HierarchyReferences>().GetReference("Content").gameObject, -1, -1);
		GameObject gameObject2 = global::Util.KInstantiateUI(this.tallFeatureBlock, this.statsContainer, true);
		this.activeColonyWidgetContainers.Add(gameObject2);
		this.activeColonyWidgets.Add("buildings", gameObject2);
		gameObject2.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.RETIRED_COLONY_INFO_SCREEN.TITLES.BUILDINGS);
		this.DisplayBuildings(data, gameObject2.GetComponent<HierarchyReferences>().GetReference("Content").gameObject);
		int num = 2;
		for (int i = 0; i < data.Stats.Length; i += num)
		{
			GameObject gameObject3 = global::Util.KInstantiateUI(this.standardStatBlock, this.statsContainer, true);
			this.activeColonyWidgetContainers.Add(gameObject3);
			for (int j = 0; j < num; j++)
			{
				if (i + j <= data.Stats.Length - 1)
				{
					RetiredColonyData.RetiredColonyStatistic retiredColonyStatistic = data.Stats[i + j];
					this.ConfigureGraph(this.GetStatistic(retiredColonyStatistic.id, data), gameObject3);
				}
			}
		}
		base.StartCoroutine(this.ComputeSizeStatGrid());
	}

	// Token: 0x060070FD RID: 28925 RVA: 0x002B1298 File Offset: 0x002AF498
	private void ConfigureGraph(RetiredColonyData.RetiredColonyStatistic statistic, GameObject layoutBlockGameObject)
	{
		GameObject gameObject = global::Util.KInstantiateUI(this.lineGraphPrefab, layoutBlockGameObject, true);
		this.activeColonyWidgets.Add(statistic.name, gameObject);
		GraphBase componentInChildren = gameObject.GetComponentInChildren<GraphBase>();
		componentInChildren.graphName = statistic.name;
		componentInChildren.label_title.SetText(componentInChildren.graphName);
		componentInChildren.axis_x.name = statistic.nameX;
		componentInChildren.axis_y.name = statistic.nameY;
		componentInChildren.label_x.SetText(componentInChildren.axis_x.name);
		componentInChildren.label_y.SetText(componentInChildren.axis_y.name);
		LineLayer componentInChildren2 = gameObject.GetComponentInChildren<LineLayer>();
		componentInChildren.axis_y.min_value = 0f;
		componentInChildren.axis_y.max_value = statistic.GetByMaxValue().second * 1.2f;
		if (float.IsNaN(componentInChildren.axis_y.max_value))
		{
			componentInChildren.axis_y.max_value = 1f;
		}
		componentInChildren.axis_x.min_value = 0f;
		componentInChildren.axis_x.max_value = statistic.GetByMaxKey().first;
		componentInChildren.axis_x.guide_frequency = (componentInChildren.axis_x.max_value - componentInChildren.axis_x.min_value) / 10f;
		componentInChildren.axis_y.guide_frequency = (componentInChildren.axis_y.max_value - componentInChildren.axis_y.min_value) / 10f;
		componentInChildren.RefreshGuides();
		global::Tuple<float, float>[] value = statistic.value;
		GraphedLine graphedLine = componentInChildren2.NewLine(value, statistic.id);
		if (this.statColors.ContainsKey(statistic.id))
		{
			componentInChildren2.line_formatting[componentInChildren2.line_formatting.Length - 1].color = this.statColors[statistic.id];
		}
		graphedLine.line_renderer.color = componentInChildren2.line_formatting[componentInChildren2.line_formatting.Length - 1].color;
	}

	// Token: 0x060070FE RID: 28926 RVA: 0x002B1480 File Offset: 0x002AF680
	private RetiredColonyData.RetiredColonyStatistic GetStatistic(string id, RetiredColonyData data)
	{
		foreach (RetiredColonyData.RetiredColonyStatistic retiredColonyStatistic in data.Stats)
		{
			if (retiredColonyStatistic.id == id)
			{
				return retiredColonyStatistic;
			}
		}
		return null;
	}

	// Token: 0x060070FF RID: 28927 RVA: 0x002B14B8 File Offset: 0x002AF6B8
	private void ToggleExplorer(bool active)
	{
		if (active && Game.Instance == null)
		{
			WorldGen.LoadSettings(false);
		}
		this.ConfigButtons();
		this.explorerRoot.SetActive(active);
		this.colonyDataRoot.SetActive(!active);
		if (!this.explorerGridConfigured)
		{
			this.explorerGridConfigured = true;
			base.StartCoroutine(this.ComputeSizeExplorerGrid());
		}
		this.explorerHeaderContainer.SetActive(active);
		this.colonyHeaderContainer.SetActive(!active);
		if (active)
		{
			this.colonyDataRoot.transform.parent.rectTransform().SetPosition(new Vector3(this.colonyDataRoot.transform.parent.rectTransform().position.x, 0f, 0f));
		}
		this.UpdateAchievementData(null, null);
		this.explorerSearch.text = "";
	}

	// Token: 0x06007100 RID: 28928 RVA: 0x002B1598 File Offset: 0x002AF798
	private void LoadExplorer()
	{
		if (SaveGame.Instance != null)
		{
			return;
		}
		this.ToggleExplorer(true);
		this.retiredColonyData = RetireColonyUtility.LoadRetiredColonies(false);
		RetiredColonyData[] array = this.retiredColonyData;
		for (int i = 0; i < array.Length; i++)
		{
			RetiredColonyData retiredColonyData = array[i];
			RetiredColonyData data = retiredColonyData;
			GameObject gameObject = global::Util.KInstantiateUI(this.colonyButtonPrefab, this.explorerGrid, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			Sprite sprite = RetireColonyUtility.LoadRetiredColonyPreview(RetireColonyUtility.StripInvalidCharacters(data.colonyName), data.startWorld);
			Image reference = component.GetReference<Image>("ColonyImage");
			RectTransform reference2 = component.GetReference<RectTransform>("PreviewUnavailableText");
			if (sprite != null)
			{
				reference.enabled = true;
				reference.sprite = sprite;
				reference2.gameObject.SetActive(false);
			}
			else
			{
				reference.enabled = false;
				reference2.gameObject.SetActive(true);
			}
			component.GetReference<LocText>("ColonyNameLabel").SetText(retiredColonyData.colonyName);
			component.GetReference<LocText>("CycleCountLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.CYCLE_COUNT, retiredColonyData.cycleCount.ToString()));
			component.GetReference<LocText>("DateLabel").SetText(retiredColonyData.date);
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				this.LoadColony(data);
			};
			string key = retiredColonyData.colonyName;
			int num = 0;
			while (this.explorerColonyWidgets.ContainsKey(key))
			{
				num++;
				key = retiredColonyData.colonyName + "_" + num.ToString();
			}
			this.explorerColonyWidgets.Add(key, gameObject);
		}
	}

	// Token: 0x06007101 RID: 28929 RVA: 0x002B174C File Offset: 0x002AF94C
	private void FilterExplorer(string search)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.explorerColonyWidgets)
		{
			if (string.IsNullOrEmpty(search) || keyValuePair.Key.ToUpper().Contains(search.ToUpper()))
			{
				keyValuePair.Value.SetActive(true);
			}
			else
			{
				keyValuePair.Value.SetActive(false);
			}
		}
	}

	// Token: 0x06007102 RID: 28930 RVA: 0x002B17D8 File Offset: 0x002AF9D8
	private void FilterColonyData(string search)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.activeColonyWidgets)
		{
			if (string.IsNullOrEmpty(search) || keyValuePair.Key.ToUpper().Contains(search.ToUpper()))
			{
				keyValuePair.Value.SetActive(true);
			}
			else
			{
				keyValuePair.Value.SetActive(false);
			}
		}
	}

	// Token: 0x06007103 RID: 28931 RVA: 0x002B1864 File Offset: 0x002AFA64
	private void FilterAchievements(string search)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.achievementEntries)
		{
			if (string.IsNullOrEmpty(search) || Db.Get().ColonyAchievements.Get(keyValuePair.Key).Name.ToUpper().Contains(search.ToUpper()))
			{
				keyValuePair.Value.SetActive(true);
			}
			else
			{
				keyValuePair.Value.SetActive(false);
			}
		}
	}

	// Token: 0x04004DD3 RID: 19923
	public static RetiredColonyInfoScreen Instance;

	// Token: 0x04004DD4 RID: 19924
	private bool wasPixelPerfect;

	// Token: 0x04004DD5 RID: 19925
	[Header("Screen")]
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004DD6 RID: 19926
	[Header("Header References")]
	[SerializeField]
	private GameObject explorerHeaderContainer;

	// Token: 0x04004DD7 RID: 19927
	[SerializeField]
	private GameObject colonyHeaderContainer;

	// Token: 0x04004DD8 RID: 19928
	[SerializeField]
	private LocText colonyName;

	// Token: 0x04004DD9 RID: 19929
	[SerializeField]
	private LocText cycleCount;

	// Token: 0x04004DDA RID: 19930
	[Header("Timelapse References")]
	[SerializeField]
	private Slideshow slideshow;

	// Token: 0x04004DDB RID: 19931
	[SerializeField]
	private GameObject worldPrefab;

	// Token: 0x04004DDC RID: 19932
	private string focusedWorld;

	// Token: 0x04004DDD RID: 19933
	private string[] currentSlideshowFiles = new string[0];

	// Token: 0x04004DDE RID: 19934
	[Header("Main Layout")]
	[SerializeField]
	private GameObject coloniesSection;

	// Token: 0x04004DDF RID: 19935
	[SerializeField]
	private GameObject achievementsSection;

	// Token: 0x04004DE0 RID: 19936
	[Header("Achievement References")]
	[SerializeField]
	private GameObject achievementsContainer;

	// Token: 0x04004DE1 RID: 19937
	[SerializeField]
	private GameObject achievementsPrefab;

	// Token: 0x04004DE2 RID: 19938
	[SerializeField]
	private GameObject victoryAchievementsPrefab;

	// Token: 0x04004DE3 RID: 19939
	[SerializeField]
	private KInputTextField achievementSearch;

	// Token: 0x04004DE4 RID: 19940
	[SerializeField]
	private KButton clearAchievementSearchButton;

	// Token: 0x04004DE5 RID: 19941
	[SerializeField]
	private GameObject[] achievementVeils;

	// Token: 0x04004DE6 RID: 19942
	[Header("Duplicant References")]
	[SerializeField]
	private GameObject duplicantPrefab;

	// Token: 0x04004DE7 RID: 19943
	[Header("Building References")]
	[SerializeField]
	private GameObject buildingPrefab;

	// Token: 0x04004DE8 RID: 19944
	[Header("Colony Stat References")]
	[SerializeField]
	private GameObject statsContainer;

	// Token: 0x04004DE9 RID: 19945
	[SerializeField]
	private GameObject specialMediaBlock;

	// Token: 0x04004DEA RID: 19946
	[SerializeField]
	private GameObject tallFeatureBlock;

	// Token: 0x04004DEB RID: 19947
	[SerializeField]
	private GameObject standardStatBlock;

	// Token: 0x04004DEC RID: 19948
	[SerializeField]
	private GameObject lineGraphPrefab;

	// Token: 0x04004DED RID: 19949
	public RetiredColonyData[] retiredColonyData;

	// Token: 0x04004DEE RID: 19950
	[Header("Explorer References")]
	[SerializeField]
	private GameObject colonyScroll;

	// Token: 0x04004DEF RID: 19951
	[SerializeField]
	private GameObject explorerRoot;

	// Token: 0x04004DF0 RID: 19952
	[SerializeField]
	private GameObject explorerGrid;

	// Token: 0x04004DF1 RID: 19953
	[SerializeField]
	private GameObject colonyDataRoot;

	// Token: 0x04004DF2 RID: 19954
	[SerializeField]
	private GameObject colonyButtonPrefab;

	// Token: 0x04004DF3 RID: 19955
	[SerializeField]
	private KInputTextField explorerSearch;

	// Token: 0x04004DF4 RID: 19956
	[SerializeField]
	private KButton clearExplorerSearchButton;

	// Token: 0x04004DF5 RID: 19957
	[Header("Navigation Buttons")]
	[SerializeField]
	private KButton closeScreenButton;

	// Token: 0x04004DF6 RID: 19958
	[SerializeField]
	private KButton viewOtherColoniesButton;

	// Token: 0x04004DF7 RID: 19959
	[SerializeField]
	private KButton quitToMainMenuButton;

	// Token: 0x04004DF8 RID: 19960
	[SerializeField]
	private GameObject disabledPlatformUnlocks;

	// Token: 0x04004DF9 RID: 19961
	private bool explorerGridConfigured;

	// Token: 0x04004DFA RID: 19962
	private Dictionary<string, GameObject> achievementEntries = new Dictionary<string, GameObject>();

	// Token: 0x04004DFB RID: 19963
	private List<GameObject> activeColonyWidgetContainers = new List<GameObject>();

	// Token: 0x04004DFC RID: 19964
	private Dictionary<string, GameObject> activeColonyWidgets = new Dictionary<string, GameObject>();

	// Token: 0x04004DFD RID: 19965
	private const float maxAchievementWidth = 830f;

	// Token: 0x04004DFE RID: 19966
	private Canvas canvasRef;

	// Token: 0x04004DFF RID: 19967
	private Dictionary<string, Color> statColors = new Dictionary<string, Color>
	{
		{
			RetiredColonyData.DataIDs.OxygenProduced,
			new Color(0.17f, 0.91f, 0.91f, 1f)
		},
		{
			RetiredColonyData.DataIDs.OxygenConsumed,
			new Color(0.17f, 0.91f, 0.91f, 1f)
		},
		{
			RetiredColonyData.DataIDs.CaloriesProduced,
			new Color(0.24f, 0.49f, 0.32f, 1f)
		},
		{
			RetiredColonyData.DataIDs.CaloriesRemoved,
			new Color(0.24f, 0.49f, 0.32f, 1f)
		},
		{
			RetiredColonyData.DataIDs.PowerProduced,
			new Color(0.98f, 0.69f, 0.23f, 1f)
		},
		{
			RetiredColonyData.DataIDs.PowerWasted,
			new Color(0.82f, 0.3f, 0.35f, 1f)
		},
		{
			RetiredColonyData.DataIDs.WorkTime,
			new Color(0.99f, 0.51f, 0.28f, 1f)
		},
		{
			RetiredColonyData.DataIDs.TravelTime,
			new Color(0.55f, 0.55f, 0.75f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageWorkTime,
			new Color(0.99f, 0.51f, 0.28f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageTravelTime,
			new Color(0.55f, 0.55f, 0.75f, 1f)
		},
		{
			RetiredColonyData.DataIDs.LiveDuplicants,
			new Color(0.98f, 0.69f, 0.23f, 1f)
		},
		{
			RetiredColonyData.DataIDs.RocketsInFlight,
			new Color(0.9f, 0.9f, 0.16f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageStressCreated,
			new Color(0.8f, 0.32f, 0.33f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageStressRemoved,
			new Color(0.8f, 0.32f, 0.33f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageGerms,
			new Color(0.68f, 0.79f, 0.18f, 1f)
		},
		{
			RetiredColonyData.DataIDs.DomesticatedCritters,
			new Color(0.62f, 0.31f, 0.47f, 1f)
		},
		{
			RetiredColonyData.DataIDs.WildCritters,
			new Color(0.62f, 0.31f, 0.47f, 1f)
		}
	};

	// Token: 0x04004E00 RID: 19968
	private Dictionary<string, GameObject> explorerColonyWidgets = new Dictionary<string, GameObject>();
}
