using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C63 RID: 3171
public class ManagementMenu : KIconToggleMenu
{
	// Token: 0x06006088 RID: 24712 RVA: 0x00237595 File Offset: 0x00235795
	public static void DestroyInstance()
	{
		ManagementMenu.Instance = null;
	}

	// Token: 0x06006089 RID: 24713 RVA: 0x0023759D File Offset: 0x0023579D
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x0600608A RID: 24714 RVA: 0x002375A4 File Offset: 0x002357A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ManagementMenu.Instance = this;
		this.notificationDisplayer.onNotificationsChanged += this.OnNotificationsChanged;
		CodexCache.CodexCacheInit();
		ScheduledUIInstantiation component = GameScreenManager.Instance.GetComponent<ScheduledUIInstantiation>();
		this.starmapScreen = component.GetInstantiatedObject<StarmapScreen>();
		this.clusterMapScreen = component.GetInstantiatedObject<ClusterMapScreen>();
		this.skillsScreen = component.GetInstantiatedObject<SkillsScreen>();
		this.researchScreen = component.GetInstantiatedObject<ResearchScreen>();
		this.fullscreenUIs = new ManagementMenu.ManagementMenuToggleInfo[]
		{
			this.researchInfo,
			this.skillsInfo,
			this.starmapInfo,
			this.clusterMapInfo
		};
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		this.consumablesInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CONSUMABLES, "OverviewUI_consumables_icon", null, global::Action.ManageConsumables, UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, "");
		this.AddToggleTooltip(this.consumablesInfo, null);
		this.vitalsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.VITALS, "OverviewUI_vitals_icon", null, global::Action.ManageVitals, UI.TOOLTIPS.MANAGEMENTMENU_VITALS, "");
		this.AddToggleTooltip(this.vitalsInfo, null);
		this.researchInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.RESEARCH, "OverviewUI_research_nav_icon", null, global::Action.ManageResearch, UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, "");
		this.AddToggleTooltipForResearch(this.researchInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_RESEARCH);
		this.researchInfo.prefabOverride = this.researchButtonPrefab;
		this.jobsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.JOBS, "OverviewUI_priority_icon", null, global::Action.ManagePriorities, UI.TOOLTIPS.MANAGEMENTMENU_JOBS, "");
		this.AddToggleTooltip(this.jobsInfo, null);
		this.skillsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SKILLS, "OverviewUI_jobs_icon", null, global::Action.ManageSkills, UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, "");
		this.AddToggleTooltip(this.skillsInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_SKILL_STATION);
		this.starmapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.starmapInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_TELESCOPE);
		this.clusterMapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.clusterMapInfo, null);
		this.scheduleInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SCHEDULE, "OverviewUI_schedule2_icon", null, global::Action.ManageSchedule, UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, "");
		this.AddToggleTooltip(this.scheduleInfo, null);
		this.reportsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.REPORT, "OverviewUI_reports_icon", null, global::Action.ManageReport, UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, "");
		this.AddToggleTooltip(this.reportsInfo, null);
		this.reportsInfo.prefabOverride = this.smallPrefab;
		this.codexInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CODEX.MANAGEMENT_BUTTON, "OverviewUI_database_icon", null, global::Action.ManageDatabase, UI.TOOLTIPS.MANAGEMENTMENU_CODEX, "");
		this.AddToggleTooltip(this.codexInfo, null);
		this.codexInfo.prefabOverride = this.smallPrefab;
		this.ScreenInfoMatch.Add(this.consumablesInfo, new ManagementMenu.ScreenData
		{
			screen = this.consumablesScreen,
			tabIdx = 3,
			toggleInfo = this.consumablesInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.vitalsInfo, new ManagementMenu.ScreenData
		{
			screen = this.vitalsScreen,
			tabIdx = 2,
			toggleInfo = this.vitalsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.reportsInfo, new ManagementMenu.ScreenData
		{
			screen = this.reportsScreen,
			tabIdx = 4,
			toggleInfo = this.reportsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.jobsInfo, new ManagementMenu.ScreenData
		{
			screen = this.jobsScreen,
			tabIdx = 1,
			toggleInfo = this.jobsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.skillsInfo, new ManagementMenu.ScreenData
		{
			screen = this.skillsScreen,
			tabIdx = 0,
			toggleInfo = this.skillsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.codexInfo, new ManagementMenu.ScreenData
		{
			screen = this.codexScreen,
			tabIdx = 6,
			toggleInfo = this.codexInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.scheduleInfo, new ManagementMenu.ScreenData
		{
			screen = this.scheduleScreen,
			tabIdx = 7,
			toggleInfo = this.scheduleInfo,
			cancelHandler = null
		});
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.ScreenInfoMatch.Add(this.clusterMapInfo, new ManagementMenu.ScreenData
			{
				screen = this.clusterMapScreen,
				tabIdx = 7,
				toggleInfo = this.clusterMapInfo,
				cancelHandler = new Func<bool>(this.clusterMapScreen.TryHandleCancel)
			});
		}
		else
		{
			this.ScreenInfoMatch.Add(this.starmapInfo, new ManagementMenu.ScreenData
			{
				screen = this.starmapScreen,
				tabIdx = 7,
				toggleInfo = this.starmapInfo,
				cancelHandler = null
			});
		}
		this.ScreenInfoMatch.Add(this.researchInfo, new ManagementMenu.ScreenData
		{
			screen = this.researchScreen,
			tabIdx = 5,
			toggleInfo = this.researchInfo,
			cancelHandler = null
		});
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		list.Add(this.vitalsInfo);
		list.Add(this.consumablesInfo);
		list.Add(this.jobsInfo);
		list.Add(this.scheduleInfo);
		list.Add(this.skillsInfo);
		list.Add(this.researchInfo);
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(this.clusterMapInfo);
		}
		else
		{
			list.Add(this.starmapInfo);
		}
		list.Add(this.reportsInfo);
		list.Add(this.codexInfo);
		base.Setup(list);
		base.onSelect += this.OnButtonClick;
		this.PauseMenuButton.onClick += this.OnPauseMenuClicked;
		this.PauseMenuButton.transform.SetAsLastSibling();
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		KInputManager.InputChange.AddListener(new UnityAction(this.OnInputChanged));
		Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearch);
		Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearch);
		Components.RoleStations.OnAdd += new Action<RoleStation>(this.CheckSkills);
		Components.RoleStations.OnRemove += new Action<RoleStation>(this.CheckSkills);
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckResearch));
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckSkills));
		Game.Instance.Subscribe(445618876, new Action<object>(this.OnResolutionChanged));
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			Components.Telescopes.OnAdd += new Action<Telescope>(this.CheckStarmap);
			Components.Telescopes.OnRemove += new Action<Telescope>(this.CheckStarmap);
		}
		this.CheckResearch(null);
		this.CheckSkills(null);
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.CheckStarmap(null);
		}
		this.researchInfo.toggle.soundPlayer.AcceptClickCondition = (() => this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.soundPlayer.toggle_widget_sound_events[0].PlaySound = false;
			ktoggle.soundPlayer.toggle_widget_sound_events[1].PlaySound = false;
		}
		this.OnResolutionChanged(null);
	}

	// Token: 0x0600608B RID: 24715 RVA: 0x00237DEC File Offset: 0x00235FEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.mutuallyExclusiveScreens.Add(AllResourcesScreen.Instance);
		this.mutuallyExclusiveScreens.Add(AllDiagnosticsScreen.Instance);
		this.OnNotificationsChanged();
	}

	// Token: 0x0600608C RID: 24716 RVA: 0x00237E1A File Offset: 0x0023601A
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.OnInputChanged));
		base.OnForcedCleanUp();
	}

	// Token: 0x0600608D RID: 24717 RVA: 0x00237E38 File Offset: 0x00236038
	private void OnInputChanged()
	{
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		this.consumablesInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, this.consumablesInfo.hotKey);
		this.vitalsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_VITALS, this.vitalsInfo.hotKey);
		this.researchInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, this.researchInfo.hotKey);
		this.jobsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_JOBS, this.jobsInfo.hotKey);
		this.skillsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, this.skillsInfo.hotKey);
		this.starmapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.starmapInfo.hotKey);
		this.clusterMapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.clusterMapInfo.hotKey);
		this.scheduleInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, this.scheduleInfo.hotKey);
		this.reportsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, this.reportsInfo.hotKey);
		this.codexInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CODEX, this.codexInfo.hotKey);
	}

	// Token: 0x0600608E RID: 24718 RVA: 0x00237FD8 File Offset: 0x002361D8
	private void OnResolutionChanged(object data = null)
	{
		bool flag = (float)Screen.width < 1300f;
		foreach (KToggle ktoggle in this.toggles)
		{
			HierarchyReferences component = ktoggle.GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				RectTransform reference = component.GetReference<RectTransform>("TextContainer");
				if (!(reference == null))
				{
					reference.gameObject.SetActive(!flag);
				}
			}
		}
	}

	// Token: 0x0600608F RID: 24719 RVA: 0x00238064 File Offset: 0x00236264
	private void OnNotificationsChanged()
	{
		foreach (KeyValuePair<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> keyValuePair in this.ScreenInfoMatch)
		{
			keyValuePair.Key.SetNotificationDisplay(false, false, null, this.noAlertColorStyle);
		}
	}

	// Token: 0x06006090 RID: 24720 RVA: 0x002380C8 File Offset: 0x002362C8
	private ToolTip.ComplexTooltipDelegate CreateToggleTooltip(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		return delegate()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (disabledTooltip != null && !toggleInfo.toggle.interactable)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				return list;
			}
			if (toggleInfo.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		};
	}

	// Token: 0x06006091 RID: 24721 RVA: 0x002380E8 File Offset: 0x002362E8
	private void AddToggleTooltip(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		toggleInfo.getTooltipText = this.CreateToggleTooltip(toggleInfo, disabledTooltip);
	}

	// Token: 0x06006092 RID: 24722 RVA: 0x002380F8 File Offset: 0x002362F8
	private void AddToggleTooltipForResearch(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		toggleInfo.getTooltipText = delegate()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			string a = (activeResearch == null) ? UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_NO_RESEARCH : string.Format(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_CARD_NAME, activeResearch.tech.Name);
			list.Add(new global::Tuple<string, TextStyleSetting>(a, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			if (activeResearch != null)
			{
				string text = "";
				for (int i = 0; i < activeResearch.tech.unlockedItems.Count; i++)
				{
					TechItem techItem = activeResearch.tech.unlockedItems[i];
					text = text + "\n" + string.Format(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_ITEM_LINE, techItem.Name);
				}
				list.Add(new global::Tuple<string, TextStyleSetting>(text, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			}
			if (disabledTooltip != null && !toggleInfo.toggle.interactable)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				return list;
			}
			if (toggleInfo.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>("\n" + toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		};
	}

	// Token: 0x06006093 RID: 24723 RVA: 0x00238130 File Offset: 0x00236330
	public bool IsFullscreenUIActive()
	{
		if (this.activeScreen == null)
		{
			return false;
		}
		foreach (ManagementMenu.ManagementMenuToggleInfo managementMenuToggleInfo in this.fullscreenUIs)
		{
			if (this.activeScreen.toggleInfo == managementMenuToggleInfo)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006094 RID: 24724 RVA: 0x00238171 File Offset: 0x00236371
	private void OnPauseMenuClicked()
	{
		PauseScreen.Instance.Show(true);
		this.PauseMenuButton.isOn = false;
	}

	// Token: 0x06006095 RID: 24725 RVA: 0x0023818A File Offset: 0x0023638A
	public void Refresh()
	{
		this.CheckResearch(null);
		this.CheckSkills(null);
		this.CheckStarmap(null);
	}

	// Token: 0x06006096 RID: 24726 RVA: 0x002381A4 File Offset: 0x002363A4
	public void CheckResearch(object o)
	{
		if (this.researchInfo.toggle == null)
		{
			return;
		}
		bool flag = Components.ResearchCenters.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = !flag && this.activeScreen != null && this.activeScreen.toggleInfo == this.researchInfo;
		this.ConfigureToggle(this.researchInfo.toggle, flag, active);
	}

	// Token: 0x06006097 RID: 24727 RVA: 0x00238220 File Offset: 0x00236420
	public void CheckSkills(object o = null)
	{
		if (this.skillsInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.RoleStations.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.skillsInfo;
		this.ConfigureToggle(this.skillsInfo.toggle, disabled, active);
	}

	// Token: 0x06006098 RID: 24728 RVA: 0x00238298 File Offset: 0x00236498
	public void CheckStarmap(object o = null)
	{
		if (this.starmapInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.Telescopes.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.starmapInfo;
		this.ConfigureToggle(this.starmapInfo.toggle, disabled, active);
	}

	// Token: 0x06006099 RID: 24729 RVA: 0x00238310 File Offset: 0x00236510
	private void ConfigureToggle(KToggle toggle, bool disabled, bool active)
	{
		toggle.interactable = !disabled;
		if (disabled)
		{
			toggle.GetComponentInChildren<ImageToggleState>().SetDisabled();
			return;
		}
		toggle.GetComponentInChildren<ImageToggleState>().SetActiveState(active);
	}

	// Token: 0x0600609A RID: 24730 RVA: 0x00238337 File Offset: 0x00236537
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.activeScreen != null && e.TryConsume(global::Action.Escape))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x0600609B RID: 24731 RVA: 0x00238365 File Offset: 0x00236565
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.activeScreen != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x0600609C RID: 24732 RVA: 0x00238398 File Offset: 0x00236598
	private void ToggleIfCancelUnhandled(ManagementMenu.ScreenData screenData)
	{
		if (screenData.cancelHandler == null || !screenData.cancelHandler())
		{
			this.ToggleScreen(screenData);
		}
	}

	// Token: 0x0600609D RID: 24733 RVA: 0x002383B6 File Offset: 0x002365B6
	private bool ResearchAvailable()
	{
		return Components.ResearchCenters.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x0600609E RID: 24734 RVA: 0x002383D8 File Offset: 0x002365D8
	private bool SkillsAvailable()
	{
		return Components.RoleStations.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x0600609F RID: 24735 RVA: 0x002383FA File Offset: 0x002365FA
	public static bool StarmapAvailable()
	{
		return Components.Telescopes.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x060060A0 RID: 24736 RVA: 0x0023841C File Offset: 0x0023661C
	public void CloseAll()
	{
		if (this.activeScreen == null)
		{
			return;
		}
		if (this.activeScreen.toggleInfo != null)
		{
			this.ToggleScreen(this.activeScreen);
		}
		this.CloseActive();
		this.ClearSelection();
	}

	// Token: 0x060060A1 RID: 24737 RVA: 0x0023844C File Offset: 0x0023664C
	private void OnUIClear(object data)
	{
		this.CloseAll();
	}

	// Token: 0x060060A2 RID: 24738 RVA: 0x00238454 File Offset: 0x00236654
	public void ToggleScreen(ManagementMenu.ScreenData screenData)
	{
		if (screenData == null)
		{
			return;
		}
		if (screenData.toggleInfo == this.researchInfo && !this.ResearchAvailable())
		{
			this.CheckResearch(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.skillsInfo && !this.SkillsAvailable())
		{
			this.CheckSkills(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.starmapInfo && !ManagementMenu.StarmapAvailable())
		{
			this.CheckStarmap(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().IsDisabled)
		{
			return;
		}
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
		}
		if (this.activeScreen != screenData)
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			if (this.activeScreen != null)
			{
				this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenMigrated);
			screenData.toggleInfo.toggle.ActivateFlourish(true);
			screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetActive();
			this.CloseActive();
			this.activeScreen = screenData;
			if (!this.activeScreen.screen.IsActive())
			{
				this.activeScreen.screen.Activate();
			}
			this.activeScreen.screen.Show(true);
			foreach (ManagementMenuNotification managementMenuNotification in this.notificationDisplayer.GetNotificationsForAction(screenData.toggleInfo.hotKey))
			{
				if (managementMenuNotification.customClickCallback != null)
				{
					managementMenuNotification.customClickCallback(managementMenuNotification.customClickData);
					break;
				}
			}
			using (List<KScreen>.Enumerator enumerator2 = this.mutuallyExclusiveScreens.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KScreen kscreen = enumerator2.Current;
					kscreen.Show(false);
				}
				return;
			}
		}
		this.activeScreen.screen.Show(false);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenMigrated, STOP_MODE.ALLOWFADEOUT);
		this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
		this.activeScreen = null;
		screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
	}

	// Token: 0x060060A3 RID: 24739 RVA: 0x0023870C File Offset: 0x0023690C
	public void OnButtonClick(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.ToggleScreen(this.ScreenInfoMatch[(ManagementMenu.ManagementMenuToggleInfo)toggle_info]);
	}

	// Token: 0x060060A4 RID: 24740 RVA: 0x00238725 File Offset: 0x00236925
	private void CloseActive()
	{
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.screen.Show(false);
			this.activeScreen = null;
		}
	}

	// Token: 0x060060A5 RID: 24741 RVA: 0x00238760 File Offset: 0x00236960
	public void ToggleResearch()
	{
		if ((this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]) && this.researchInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
	}

	// Token: 0x060060A6 RID: 24742 RVA: 0x002387B5 File Offset: 0x002369B5
	public void ToggleCodex()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.codexInfo]);
	}

	// Token: 0x060060A7 RID: 24743 RVA: 0x002387D4 File Offset: 0x002369D4
	public void OpenCodexToLockId(string lockId, bool focusContent = false)
	{
		string entryForLock = CodexCache.GetEntryForLock(lockId);
		if (entryForLock == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Could not open codex to lockId \"" + lockId + "\", couldn't find an entry that contained that lockId"
			});
			return;
		}
		ContentContainer contentContainer = null;
		if (focusContent)
		{
			CodexEntry codexEntry = CodexCache.FindEntry(entryForLock);
			int num = 0;
			while (contentContainer == null && num < codexEntry.contentContainers.Count)
			{
				if (!(codexEntry.contentContainers[num].lockID != lockId))
				{
					contentContainer = codexEntry.contentContainers[num];
				}
				num++;
			}
		}
		this.OpenCodexToEntry(entryForLock, contentContainer);
	}

	// Token: 0x060060A8 RID: 24744 RVA: 0x00238860 File Offset: 0x00236A60
	public void OpenCodexToEntry(string id, ContentContainer targetContainer = null)
	{
		if (!this.codexScreen.gameObject.activeInHierarchy)
		{
			this.ToggleCodex();
		}
		this.codexScreen.ChangeArticle(id, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		this.codexScreen.FocusContainer(targetContainer);
	}

	// Token: 0x060060A9 RID: 24745 RVA: 0x002388A8 File Offset: 0x00236AA8
	public void ToggleSkills()
	{
		if ((this.SkillsAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]) && this.skillsInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x060060AA RID: 24746 RVA: 0x002388FD File Offset: 0x00236AFD
	public void ToggleStarmap()
	{
		if (this.starmapInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x060060AB RID: 24747 RVA: 0x00238922 File Offset: 0x00236B22
	public void ToggleClusterMap()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
	}

	// Token: 0x060060AC RID: 24748 RVA: 0x0023893F File Offset: 0x00236B3F
	public void TogglePriorities()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.jobsInfo]);
	}

	// Token: 0x060060AD RID: 24749 RVA: 0x0023895C File Offset: 0x00236B5C
	public void OpenReports(int day)
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo]);
		}
		ReportScreen.Instance.ShowReport(day);
	}

	// Token: 0x060060AE RID: 24750 RVA: 0x002389AC File Offset: 0x00236BAC
	public void OpenResearch(string zoomToTech = null)
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
		if (zoomToTech != null)
		{
			UIScheduler.Instance.Schedule("ResearchCameraFocus", 0.25f, delegate(object data)
			{
				this.researchScreen.ZoomToTech(zoomToTech, true);
			}, null, null);
		}
	}

	// Token: 0x060060AF RID: 24751 RVA: 0x00238A30 File Offset: 0x00236C30
	public void OpenStarmap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x060060B0 RID: 24752 RVA: 0x00238A6A File Offset: 0x00236C6A
	public void OpenClusterMap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x060060B1 RID: 24753 RVA: 0x00238AA4 File Offset: 0x00236CA4
	public void CloseClusterMap()
	{
		if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x060060B2 RID: 24754 RVA: 0x00238AE0 File Offset: 0x00236CE0
	public void OpenSkills(MinionIdentity minionIdentity)
	{
		this.skillsScreen.CurrentlySelectedMinion = minionIdentity;
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x060060B3 RID: 24755 RVA: 0x00238B31 File Offset: 0x00236D31
	public bool IsScreenOpen(KScreen screen)
	{
		return this.activeScreen != null && this.activeScreen.screen == screen;
	}

	// Token: 0x04004075 RID: 16501
	private const float UI_WIDTH_COMPRESS_THRESHOLD = 1300f;

	// Token: 0x04004076 RID: 16502
	[MyCmpReq]
	public ManagementMenuNotificationDisplayer notificationDisplayer;

	// Token: 0x04004077 RID: 16503
	public static ManagementMenu Instance;

	// Token: 0x04004078 RID: 16504
	[Header("Management Menu Specific")]
	[SerializeField]
	private KToggle smallPrefab;

	// Token: 0x04004079 RID: 16505
	[SerializeField]
	private KToggle researchButtonPrefab;

	// Token: 0x0400407A RID: 16506
	public KToggle PauseMenuButton;

	// Token: 0x0400407B RID: 16507
	[Header("Top Right Screen References")]
	public JobsTableScreen jobsScreen;

	// Token: 0x0400407C RID: 16508
	public VitalsTableScreen vitalsScreen;

	// Token: 0x0400407D RID: 16509
	public ScheduleScreen scheduleScreen;

	// Token: 0x0400407E RID: 16510
	public ReportScreen reportsScreen;

	// Token: 0x0400407F RID: 16511
	public CodexScreen codexScreen;

	// Token: 0x04004080 RID: 16512
	public ConsumablesTableScreen consumablesScreen;

	// Token: 0x04004081 RID: 16513
	private StarmapScreen starmapScreen;

	// Token: 0x04004082 RID: 16514
	private ClusterMapScreen clusterMapScreen;

	// Token: 0x04004083 RID: 16515
	private SkillsScreen skillsScreen;

	// Token: 0x04004084 RID: 16516
	private ResearchScreen researchScreen;

	// Token: 0x04004085 RID: 16517
	[Header("Notification Styles")]
	public ColorStyleSetting noAlertColorStyle;

	// Token: 0x04004086 RID: 16518
	public List<ColorStyleSetting> alertColorStyle;

	// Token: 0x04004087 RID: 16519
	public List<TextStyleSetting> alertTextStyle;

	// Token: 0x04004088 RID: 16520
	private ManagementMenu.ManagementMenuToggleInfo jobsInfo;

	// Token: 0x04004089 RID: 16521
	private ManagementMenu.ManagementMenuToggleInfo consumablesInfo;

	// Token: 0x0400408A RID: 16522
	private ManagementMenu.ManagementMenuToggleInfo scheduleInfo;

	// Token: 0x0400408B RID: 16523
	private ManagementMenu.ManagementMenuToggleInfo vitalsInfo;

	// Token: 0x0400408C RID: 16524
	private ManagementMenu.ManagementMenuToggleInfo reportsInfo;

	// Token: 0x0400408D RID: 16525
	private ManagementMenu.ManagementMenuToggleInfo researchInfo;

	// Token: 0x0400408E RID: 16526
	private ManagementMenu.ManagementMenuToggleInfo codexInfo;

	// Token: 0x0400408F RID: 16527
	private ManagementMenu.ManagementMenuToggleInfo starmapInfo;

	// Token: 0x04004090 RID: 16528
	private ManagementMenu.ManagementMenuToggleInfo clusterMapInfo;

	// Token: 0x04004091 RID: 16529
	private ManagementMenu.ManagementMenuToggleInfo skillsInfo;

	// Token: 0x04004092 RID: 16530
	private ManagementMenu.ManagementMenuToggleInfo[] fullscreenUIs;

	// Token: 0x04004093 RID: 16531
	private Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> ScreenInfoMatch = new Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData>();

	// Token: 0x04004094 RID: 16532
	private ManagementMenu.ScreenData activeScreen;

	// Token: 0x04004095 RID: 16533
	private KButton activeButton;

	// Token: 0x04004096 RID: 16534
	private string skillsTooltip;

	// Token: 0x04004097 RID: 16535
	private string skillsTooltipDisabled;

	// Token: 0x04004098 RID: 16536
	private string researchTooltip;

	// Token: 0x04004099 RID: 16537
	private string researchTooltipDisabled;

	// Token: 0x0400409A RID: 16538
	private string starmapTooltip;

	// Token: 0x0400409B RID: 16539
	private string starmapTooltipDisabled;

	// Token: 0x0400409C RID: 16540
	private string clusterMapTooltip;

	// Token: 0x0400409D RID: 16541
	private string clusterMapTooltipDisabled;

	// Token: 0x0400409E RID: 16542
	private List<KScreen> mutuallyExclusiveScreens = new List<KScreen>();

	// Token: 0x02001E16 RID: 7702
	public class ScreenData
	{
		// Token: 0x04008D57 RID: 36183
		public KScreen screen;

		// Token: 0x04008D58 RID: 36184
		public ManagementMenu.ManagementMenuToggleInfo toggleInfo;

		// Token: 0x04008D59 RID: 36185
		public Func<bool> cancelHandler;

		// Token: 0x04008D5A RID: 36186
		public int tabIdx;
	}

	// Token: 0x02001E17 RID: 7703
	public class ManagementMenuToggleInfo : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x0600B310 RID: 45840 RVA: 0x003E26A3 File Offset: 0x003E08A3
		public ManagementMenuToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon, user_data, hotkey, tooltip, tooltip_header)
		{
			this.tooltip = GameUtil.ReplaceHotkeyString(this.tooltip, this.hotKey);
		}

		// Token: 0x0600B311 RID: 45841 RVA: 0x003E26CC File Offset: 0x003E08CC
		public void SetNotificationDisplay(bool showAlertImage, bool showGlow, ColorStyleSetting buttonColorStyle, ColorStyleSetting alertColorStyle)
		{
			ImageToggleState component = this.toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				if (buttonColorStyle != null)
				{
					component.SetColorStyle(buttonColorStyle);
				}
				else
				{
					component.SetColorStyle(this.originalButtonSetting);
				}
			}
			if (this.alertImage != null)
			{
				this.alertImage.gameObject.SetActive(showAlertImage);
				this.alertImage.SetColorStyle(alertColorStyle);
			}
			if (this.glowImage != null)
			{
				this.glowImage.gameObject.SetActive(showGlow);
				if (buttonColorStyle != null)
				{
					this.glowImage.SetColorStyle(buttonColorStyle);
				}
			}
		}

		// Token: 0x0600B312 RID: 45842 RVA: 0x003E276C File Offset: 0x003E096C
		public override void SetToggle(KToggle toggle)
		{
			base.SetToggle(toggle);
			ImageToggleState component = toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				this.originalButtonSetting = component.colorStyleSetting;
			}
			HierarchyReferences component2 = toggle.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				this.alertImage = component2.GetReference<ImageToggleState>("AlertImage");
				this.glowImage = component2.GetReference<ImageToggleState>("GlowImage");
			}
		}

		// Token: 0x04008D5B RID: 36187
		public ImageToggleState alertImage;

		// Token: 0x04008D5C RID: 36188
		public ImageToggleState glowImage;

		// Token: 0x04008D5D RID: 36189
		private ColorStyleSetting originalButtonSetting;
	}
}
