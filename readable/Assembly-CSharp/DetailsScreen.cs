using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D01 RID: 3329
public class DetailsScreen : KTabMenu
{
	// Token: 0x060066DA RID: 26330 RVA: 0x0026B8D8 File Offset: 0x00269AD8
	public static void DestroyInstance()
	{
		DetailsScreen.Instance = null;
	}

	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x060066DB RID: 26331 RVA: 0x0026B8E0 File Offset: 0x00269AE0
	// (set) Token: 0x060066DC RID: 26332 RVA: 0x0026B8E8 File Offset: 0x00269AE8
	public GameObject target { get; private set; }

	// Token: 0x060066DD RID: 26333 RVA: 0x0026B8F4 File Offset: 0x00269AF4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.SortScreenOrder();
		base.ConsumeMouseScroll = true;
		global::Debug.Assert(DetailsScreen.Instance == null);
		DetailsScreen.Instance = this;
		this.InitiateSidescreenTabs();
		this.DeactivateSideContent();
		this.Show(false);
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
		this.tabHeader.Init();
	}

	// Token: 0x060066DE RID: 26334 RVA: 0x0026B96C File Offset: 0x00269B6C
	public bool CanObjectDisplayTabOfType(GameObject obj, DetailsScreen.SidescreenTabTypes type)
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			if (sidescreenTab.type == type)
			{
				return sidescreenTab.ValidateTarget(obj);
			}
		}
		return false;
	}

	// Token: 0x060066DF RID: 26335 RVA: 0x0026B9A8 File Offset: 0x00269BA8
	public DetailsScreen.SidescreenTab GetTabOfType(DetailsScreen.SidescreenTabTypes type)
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			if (sidescreenTab.type == type)
			{
				return sidescreenTab;
			}
		}
		return null;
	}

	// Token: 0x060066E0 RID: 26336 RVA: 0x0026B9E0 File Offset: 0x00269BE0
	public void InitiateSidescreenTabs()
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			sidescreenTab.Initiate(this.original_tab, this.original_tab_body, delegate(DetailsScreen.SidescreenTab _tab)
			{
				this.SelectSideScreenTab(_tab.type);
			});
			switch (sidescreenTab.type)
			{
			case DetailsScreen.SidescreenTabTypes.Errands:
				sidescreenTab.ValidateTargetCallback = ((GameObject target, DetailsScreen.SidescreenTab _tab) => target.GetComponent<MinionIdentity>() != null);
				break;
			case DetailsScreen.SidescreenTabTypes.Material:
				sidescreenTab.ValidateTargetCallback = delegate(GameObject target, DetailsScreen.SidescreenTab _tab)
				{
					Reconstructable component = target.GetComponent<Reconstructable>();
					return component != null && component.AllowReconstruct;
				};
				break;
			case DetailsScreen.SidescreenTabTypes.Blueprints:
				sidescreenTab.ValidateTargetCallback = delegate(GameObject target, DetailsScreen.SidescreenTab _tab)
				{
					UnityEngine.Object component = target.GetComponent<MinionIdentity>();
					BuildingFacade component2 = target.GetComponent<BuildingFacade>();
					return component != null || component2 != null;
				};
				break;
			}
		}
	}

	// Token: 0x060066E1 RID: 26337 RVA: 0x0026BAC0 File Offset: 0x00269CC0
	private void OnSelectObject(object data)
	{
		if (data == null)
		{
			this.previouslyActiveTab = -1;
			this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
			return;
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		if (!(component == null) && !(this.previousTargetID != component.PrefabID()))
		{
			this.SelectSideScreenTab(this.selectedSidescreenTabID);
			return;
		}
		if (component != null && component.GetComponent<MinionIdentity>())
		{
			this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Errands);
			return;
		}
		this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
	}

	// Token: 0x060066E2 RID: 26338 RVA: 0x0026BB3C File Offset: 0x00269D3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CodexEntryButton.onClick += this.CodexEntryButton_OnClick;
		this.PinResourceButton.onClick += this.PinResourceButton_OnClick;
		this.CloseButton.onClick += this.DeselectAndClose;
		this.TabTitle.OnNameChanged += this.OnNameChanged;
		this.TabTitle.OnStartedEditing += this.OnStartedEditing;
		this.sideScreen2.SetActive(false);
		base.Subscribe<DetailsScreen>(-1514841199, DetailsScreen.OnRefreshDataDelegate);
	}

	// Token: 0x060066E3 RID: 26339 RVA: 0x0026BBDF File Offset: 0x00269DDF
	private void OnStartedEditing()
	{
		base.isEditing = true;
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x060066E4 RID: 26340 RVA: 0x0026BBF4 File Offset: 0x00269DF4
	private void OnNameChanged(string newName)
	{
		base.isEditing = false;
		if (string.IsNullOrEmpty(newName))
		{
			return;
		}
		MinionIdentity component = this.target.GetComponent<MinionIdentity>();
		UserNameable component2 = this.target.GetComponent<UserNameable>();
		ClustercraftExteriorDoor component3 = this.target.GetComponent<ClustercraftExteriorDoor>();
		CommandModule component4 = this.target.GetComponent<CommandModule>();
		if (component != null)
		{
			component.SetName(newName);
			if (ScheduleScreen.Instance != null)
			{
				ScheduleScreen.Instance.Trigger(1980521255, null);
			}
		}
		else if (component4 != null)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component4.GetComponent<LaunchConditionManager>()).SetRocketName(newName);
		}
		else if (component3 != null)
		{
			component3.GetTargetWorld().GetComponent<UserNameable>().SetName(newName);
		}
		else if (component2 != null)
		{
			component2.SetName(newName);
		}
		this.TabTitle.UpdateRenameTooltip(this.target);
	}

	// Token: 0x060066E5 RID: 26341 RVA: 0x0026BCCE File Offset: 0x00269ECE
	protected override void OnDeactivate()
	{
		if (this.target != null && this.setRocketTitleHandle != -1)
		{
			this.target.Unsubscribe(this.setRocketTitleHandle);
		}
		this.setRocketTitleHandle = -1;
		this.DeactivateSideContent();
		base.OnDeactivate();
	}

	// Token: 0x060066E6 RID: 26342 RVA: 0x0026BD0B File Offset: 0x00269F0B
	protected override void OnShow(bool show)
	{
		if (!show)
		{
			this.DeactivateSideContent();
		}
		else
		{
			this.MaskSideContent(false);
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
		}
		base.OnShow(show);
	}

	// Token: 0x060066E7 RID: 26343 RVA: 0x0026BD3B File Offset: 0x00269F3B
	protected override void OnCmpDisable()
	{
		this.DeactivateSideContent();
		base.OnCmpDisable();
	}

	// Token: 0x060066E8 RID: 26344 RVA: 0x0026BD49 File Offset: 0x00269F49
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.isEditing && this.target != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.DeselectAndClose();
		}
	}

	// Token: 0x060066E9 RID: 26345 RVA: 0x0026BD78 File Offset: 0x00269F78
	private static Component GetComponent(GameObject go, string name)
	{
		Type type = Type.GetType(name);
		Component component;
		if (type != null)
		{
			component = go.GetComponent(type);
		}
		else
		{
			component = go.GetComponent(name);
		}
		return component;
	}

	// Token: 0x060066EA RID: 26346 RVA: 0x0026BDAC File Offset: 0x00269FAC
	private static bool IsExcludedPrefabTag(GameObject go, Tag[] excluded_tags)
	{
		if (excluded_tags == null || excluded_tags.Length == 0)
		{
			return false;
		}
		bool result = false;
		KPrefabID component = go.GetComponent<KPrefabID>();
		foreach (Tag b in excluded_tags)
		{
			if (component.PrefabTag == b)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x060066EB RID: 26347 RVA: 0x0026BDF4 File Offset: 0x00269FF4
	private string CodexEntryButton_GetCodexId()
	{
		global::Debug.Assert(this.target != null, "Details Screen has no target");
		KSelectable component = this.target.GetComponent<KSelectable>();
		DebugUtil.AssertArgs(component != null, new object[]
		{
			"Details Screen target is not a KSelectable",
			this.target
		});
		CellSelectionObject component2 = component.GetComponent<CellSelectionObject>();
		CodexEntryRedirector component3 = component.GetComponent<CodexEntryRedirector>();
		BuildingUnderConstruction component4 = component.GetComponent<BuildingUnderConstruction>();
		CreatureBrain component5 = component.GetComponent<CreatureBrain>();
		PlantableSeed component6 = component.GetComponent<PlantableSeed>();
		string text;
		if (component2 != null)
		{
			text = CodexCache.FormatLinkID(component2.element.id.ToString());
		}
		else if (component3 != null && !string.IsNullOrEmpty(component3.CodexID))
		{
			text = CodexCache.FormatLinkID(component3.CodexID);
		}
		else if (component4 != null)
		{
			text = CodexCache.FormatLinkID(component4.Def.PrefabID);
		}
		else if (component5 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("BABY", "");
		}
		else if (component6 != null)
		{
			text = CodexCache.FormatLinkID(component6.PrefabID().ToString());
		}
		else
		{
			text = UI.ExtractLinkID(component.GetProperName());
			if (string.IsNullOrEmpty(text))
			{
				text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			}
		}
		if (CodexCache.entries.ContainsKey(text) || CodexCache.FindSubEntry(text) != null)
		{
			return text;
		}
		return "";
	}

	// Token: 0x060066EC RID: 26348 RVA: 0x0026BF90 File Offset: 0x0026A190
	private void CodexEntryButton_Refresh()
	{
		string a = this.CodexEntryButton_GetCodexId();
		this.CodexEntryButton.isInteractable = (a != "");
		this.CodexEntryButton.GetComponent<ToolTip>().SetSimpleTooltip(this.CodexEntryButton.isInteractable ? UI.TOOLTIPS.OPEN_CODEX_ENTRY : UI.TOOLTIPS.NO_CODEX_ENTRY);
	}

	// Token: 0x060066ED RID: 26349 RVA: 0x0026BFE8 File Offset: 0x0026A1E8
	public void CodexEntryButton_OnClick()
	{
		string text = this.CodexEntryButton_GetCodexId();
		if (text != "")
		{
			ManagementMenu.Instance.OpenCodexToEntry(text, null);
		}
	}

	// Token: 0x060066EE RID: 26350 RVA: 0x0026C018 File Offset: 0x0026A218
	private bool PinResourceButton_TryGetResourceTagAndProperName(out Tag targetTag, out string targetProperName)
	{
		KPrefabID component = this.target.GetComponent<KPrefabID>();
		if (component != null && DetailsScreen.<PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(component.PrefabTag))
		{
			targetTag = component.PrefabTag;
			targetProperName = component.GetProperName();
			return true;
		}
		CellSelectionObject component2 = this.target.GetComponent<CellSelectionObject>();
		if (component2 != null && DetailsScreen.<PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(component2.element.tag))
		{
			targetTag = component2.element.tag;
			targetProperName = component2.GetProperName();
			return true;
		}
		targetTag = null;
		targetProperName = null;
		return false;
	}

	// Token: 0x060066EF RID: 26351 RVA: 0x0026C0B0 File Offset: 0x0026A2B0
	private void PinResourceButton_Refresh()
	{
		Tag tag;
		string arg;
		if (this.PinResourceButton_TryGetResourceTagAndProperName(out tag, out arg))
		{
			ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(tag);
			GameUtil.MeasureUnit measureUnit;
			if (!AllResourcesScreen.Instance.units.TryGetValue(tag, out measureUnit))
			{
				measureUnit = GameUtil.MeasureUnit.quantity;
			}
			string arg2;
			switch (measureUnit)
			{
			case GameUtil.MeasureUnit.mass:
				arg2 = GameUtil.GetFormattedMass(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, false), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
				break;
			case GameUtil.MeasureUnit.kcal:
				arg2 = GameUtil.GetFormattedCalories(WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(tag.Name, ClusterManager.Instance.activeWorld.worldInventory, true), GameUtil.TimeSlice.None, true);
				break;
			case GameUtil.MeasureUnit.quantity:
				arg2 = GameUtil.GetFormattedUnits(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, false), GameUtil.TimeSlice.None, true, "");
				break;
			default:
				arg2 = "";
				break;
			}
			this.PinResourceButton.gameObject.SetActive(true);
			this.PinResourceButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.TOOLTIPS.OPEN_RESOURCE_INFO, arg2, arg));
			return;
		}
		this.PinResourceButton.gameObject.SetActive(false);
	}

	// Token: 0x060066F0 RID: 26352 RVA: 0x0026C1D4 File Offset: 0x0026A3D4
	public void PinResourceButton_OnClick()
	{
		Tag tag;
		string text;
		if (this.PinResourceButton_TryGetResourceTagAndProperName(out tag, out text))
		{
			AllResourcesScreen.Instance.SetFilter(UI.StripLinkFormatting(text));
			AllResourcesScreen.Instance.Show(true);
		}
	}

	// Token: 0x060066F1 RID: 26353 RVA: 0x0026C208 File Offset: 0x0026A408
	public void OnRefreshData(object obj)
	{
		this.RefreshTitle();
		for (int i = 0; i < this.tabs.Count; i++)
		{
			if (this.tabs[i].gameObject.activeInHierarchy)
			{
				this.tabs[i].Trigger(-1514841199, obj);
			}
		}
	}

	// Token: 0x060066F2 RID: 26354 RVA: 0x0026C260 File Offset: 0x0026A460
	public void Refresh(GameObject go)
	{
		if (this.screens == null)
		{
			return;
		}
		if (this.target != go)
		{
			if (this.setRocketTitleHandle != -1)
			{
				this.target.Unsubscribe(this.setRocketTitleHandle);
				this.setRocketTitleHandle = -1;
			}
			if (this.target != null)
			{
				KPrefabID component = this.target.GetComponent<KPrefabID>();
				if (component != null)
				{
					this.previousTargetID = component.PrefabID();
				}
				else
				{
					this.previousTargetID = null;
				}
			}
		}
		this.target = go;
		this.sortedSideScreens.Clear();
		CellSelectionObject component2 = this.target.GetComponent<CellSelectionObject>();
		if (component2)
		{
			component2.OnObjectSelected(null);
		}
		this.UpdateTitle();
		this.tabHeader.RefreshTabDisplayForTarget(this.target);
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			bool flag = false;
			foreach (DetailsScreen.SideScreenRef sideScreenRef in this.sideScreens)
			{
				if (!sideScreenRef.screenPrefab.IsValidForTarget(this.target))
				{
					if (sideScreenRef.screenInstance != null && sideScreenRef.screenInstance.gameObject.activeSelf)
					{
						sideScreenRef.screenInstance.gameObject.SetActive(false);
					}
				}
				else
				{
					flag = true;
					if (sideScreenRef.screenInstance == null)
					{
						DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(sideScreenRef.tab);
						sideScreenRef.screenInstance = global::Util.KInstantiateUI<SideScreenContent>(sideScreenRef.screenPrefab.gameObject, tabOfType.bodyInstance, false);
					}
					if (!this.sideScreen.activeSelf)
					{
						this.sideScreen.SetActive(true);
					}
					sideScreenRef.screenInstance.SetTarget(this.target);
					sideScreenRef.screenInstance.Show(true);
					int sideScreenSortOrder = sideScreenRef.screenInstance.GetSideScreenSortOrder();
					this.sortedSideScreens.Add(new KeyValuePair<DetailsScreen.SideScreenRef, int>(sideScreenRef, sideScreenSortOrder));
				}
			}
			if (!flag)
			{
				if (!this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Material) && !this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Blueprints))
				{
					this.sideScreen.SetActive(false);
				}
				else
				{
					this.sideScreen.SetActive(true);
				}
			}
		}
		this.sortedSideScreens.Sort(delegate(KeyValuePair<DetailsScreen.SideScreenRef, int> x, KeyValuePair<DetailsScreen.SideScreenRef, int> y)
		{
			if (x.Value <= y.Value)
			{
				return 1;
			}
			return -1;
		});
		for (int i = 0; i < this.sortedSideScreens.Count; i++)
		{
			this.sortedSideScreens[i].Key.screenInstance.transform.SetSiblingIndex(i);
		}
		for (int j = 0; j < this.sidescreenTabs.Length; j++)
		{
			DetailsScreen.SidescreenTab tab = this.sidescreenTabs[j];
			tab.RepositionTitle();
			KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((KeyValuePair<DetailsScreen.SideScreenRef, int> t) => t.Key.tab == tab.type);
			tab.SetNoConfigMessageVisibility(keyValuePair.Key == null);
		}
		this.RefreshTitle();
	}

	// Token: 0x060066F3 RID: 26355 RVA: 0x0026C59C File Offset: 0x0026A79C
	public void RefreshTitle()
	{
		if (this.target == null)
		{
			return;
		}
		this.TabTitle.SetTitle(this.target.GetProperName());
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab tab = this.sidescreenTabs[i];
			if (tab.IsVisible)
			{
				KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((KeyValuePair<DetailsScreen.SideScreenRef, int> match) => match.Key.tab == tab.type);
				if (keyValuePair.Key != null)
				{
					tab.SetTitleVisibility(keyValuePair.Key.screenInstance.CheckShouldShowTopTitle == null || keyValuePair.Key.screenInstance.CheckShouldShowTopTitle());
					tab.SetTitle(keyValuePair.Key.screenInstance.GetTitle());
				}
				else
				{
					tab.SetTitle(UI.UISIDESCREENS.NOCONFIG.TITLE);
					tab.SetTitleVisibility(tab.type == DetailsScreen.SidescreenTabTypes.Config || tab.type == DetailsScreen.SidescreenTabTypes.Errands);
				}
			}
			if (tab.type == DetailsScreen.SidescreenTabTypes.Config)
			{
				if (this.target.GetComponent<MinionIdentity>() == null)
				{
					tab.Tooltip_Key = "STRINGS.UI.DETAILTABS.CONFIGURATION.TOOLTIP";
				}
				else
				{
					tab.Tooltip_Key = "STRINGS.UI.DETAILTABS.CONFIGURATION.TOOLTIP_DUPLICANT";
				}
				tab.tabInstance.GetComponent<ToolTip>().SetSimpleTooltip(Strings.Get(tab.Tooltip_Key));
			}
		}
	}

	// Token: 0x060066F4 RID: 26356 RVA: 0x0026C72B File Offset: 0x0026A92B
	private void SelectSideScreenTab(DetailsScreen.SidescreenTabTypes tabID)
	{
		this.selectedSidescreenTabID = tabID;
		this.RefreshSideScreenTabs();
	}

	// Token: 0x060066F5 RID: 26357 RVA: 0x0026C73C File Offset: 0x0026A93C
	private void RefreshSideScreenTabs()
	{
		int num = 1;
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			bool flag = sidescreenTab.ValidateTarget(this.target);
			sidescreenTab.SetVisible(flag);
			sidescreenTab.SetSelected(this.selectedSidescreenTabID == sidescreenTab.type);
			num += (flag ? 1 : 0);
		}
		this.RefreshTitle();
		DetailsScreen.SidescreenTabTypes sidescreenTabTypes = this.selectedSidescreenTabID;
		if (sidescreenTabTypes != DetailsScreen.SidescreenTabTypes.Material)
		{
			if (sidescreenTabTypes == DetailsScreen.SidescreenTabTypes.Blueprints)
			{
				DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints);
				CosmeticsPanel reference = tabOfType.bodyInstance.GetComponent<HierarchyReferences>().GetReference<CosmeticsPanel>("CosmeticsPanel");
				reference.SetTarget(this.target);
				reference.Refresh();
				CosmeticsPanel reference2 = tabOfType.OverrideBody.GetComponent<HierarchyReferences>().GetReference<CosmeticsPanel>("CosmeticsPanel");
				LayoutRebuilder.ForceRebuildLayoutImmediate(reference2.GetComponent<RectTransform>());
				float num2 = Mathf.Min(384f, reference2.GetComponent<RectTransform>().sizeDelta.y + 16f);
				tabOfType.OverrideBody.GetComponent<LayoutElement>().minHeight = num2;
				tabOfType.OverrideBody.GetComponent<LayoutElement>().preferredHeight = num2;
			}
		}
		else
		{
			this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material).bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(this.target);
		}
		this.sidescreenTabHeader.SetActive(num > 1);
	}

	// Token: 0x060066F6 RID: 26358 RVA: 0x0026C87C File Offset: 0x0026AA7C
	public KScreen SetSecondarySideScreen(KScreen secondaryPrefab, string title)
	{
		this.ClearSecondarySideScreen();
		if (this.instantiatedSecondarySideScreens.ContainsKey(secondaryPrefab))
		{
			this.activeSideScreen2 = this.instantiatedSecondarySideScreens[secondaryPrefab];
			this.activeSideScreen2.gameObject.SetActive(true);
		}
		else
		{
			this.activeSideScreen2 = KScreenManager.Instance.InstantiateScreen(secondaryPrefab.gameObject, this.sideScreen2ContentBody);
			this.activeSideScreen2.Activate();
			this.instantiatedSecondarySideScreens.Add(secondaryPrefab, this.activeSideScreen2);
		}
		this.sideScreen2Title.text = title;
		this.sideScreen2.SetActive(true);
		return this.activeSideScreen2;
	}

	// Token: 0x060066F7 RID: 26359 RVA: 0x0026C919 File Offset: 0x0026AB19
	public void ClearSecondarySideScreen()
	{
		if (this.activeSideScreen2 != null)
		{
			this.activeSideScreen2.gameObject.SetActive(false);
			this.activeSideScreen2 = null;
		}
		this.sideScreen2.SetActive(false);
	}

	// Token: 0x060066F8 RID: 26360 RVA: 0x0026C950 File Offset: 0x0026AB50
	public void DeactivateSideContent()
	{
		if (SideDetailsScreen.Instance != null && SideDetailsScreen.Instance.gameObject.activeInHierarchy)
		{
			SideDetailsScreen.Instance.Show(false);
		}
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			this.sideScreens.ForEach(delegate(DetailsScreen.SideScreenRef scn)
			{
				if (scn.screenInstance != null)
				{
					scn.screenInstance.ClearTarget();
					scn.screenInstance.Show(false);
				}
			});
		}
		DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material);
		DetailsScreen.SidescreenTab tabOfType2 = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints);
		tabOfType.bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(null);
		tabOfType2.bodyInstance.GetComponentInChildren<CosmeticsPanel>().SetTarget(null);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenHalfEffect, STOP_MODE.ALLOWFADEOUT);
		this.sideScreen.SetActive(false);
	}

	// Token: 0x060066F9 RID: 26361 RVA: 0x0026CA18 File Offset: 0x0026AC18
	public void MaskSideContent(bool hide)
	{
		if (hide)
		{
			this.sideScreen.transform.localScale = Vector3.zero;
			return;
		}
		this.sideScreen.transform.localScale = Vector3.one;
	}

	// Token: 0x060066FA RID: 26362 RVA: 0x0026CA48 File Offset: 0x0026AC48
	public void DeselectAndClose()
	{
		if (base.gameObject.activeInHierarchy)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
		}
		if (this.GetActiveTab() != null)
		{
			this.GetActiveTab().SetTarget(null);
		}
		SelectTool.Instance.Select(null, false);
		ClusterMapSelectTool.Instance.Select(null, false);
		if (this.target == null)
		{
			return;
		}
		this.target = null;
		this.previousTargetID = null;
		this.DeactivateSideContent();
		this.Show(false);
	}

	// Token: 0x060066FB RID: 26363 RVA: 0x0026CAD3 File Offset: 0x0026ACD3
	private void SortScreenOrder()
	{
		Array.Sort<DetailsScreen.Screens>(this.screens, (DetailsScreen.Screens x, DetailsScreen.Screens y) => x.displayOrderPriority.CompareTo(y.displayOrderPriority));
	}

	// Token: 0x060066FC RID: 26364 RVA: 0x0026CB00 File Offset: 0x0026AD00
	public void UpdatePortrait(GameObject target)
	{
		KSelectable component = target.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		this.TabTitle.portrait.ClearPortrait();
		Building component2 = component.GetComponent<Building>();
		if (component2)
		{
			Sprite uisprite = component2.Def.GetUISprite("ui", false);
			if (uisprite != null)
			{
				this.TabTitle.portrait.SetPortrait(uisprite);
				return;
			}
		}
		if (target.GetComponent<MinionIdentity>())
		{
			this.TabTitle.SetPortrait(component.gameObject);
			return;
		}
		Edible component3 = target.GetComponent<Edible>();
		if (component3 != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component3.GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim);
			return;
		}
		PrimaryElement component4 = target.GetComponent<PrimaryElement>();
		if (component4 != null)
		{
			this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(ElementLoader.FindElementByHash(component4.ElementID).substance.anim, "ui", false, ""));
			return;
		}
		CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
		if (component5 != null)
		{
			string animName = component5.element.IsSolid ? "ui" : component5.element.substance.name;
			Sprite uispriteFromMultiObjectAnim2 = Def.GetUISpriteFromMultiObjectAnim(component5.element.substance.anim, animName, false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim2);
			return;
		}
	}

	// Token: 0x060066FD RID: 26365 RVA: 0x0026CC84 File Offset: 0x0026AE84
	public bool CompareTargetWith(GameObject compare)
	{
		return this.target == compare;
	}

	// Token: 0x060066FE RID: 26366 RVA: 0x0026CC94 File Offset: 0x0026AE94
	public void UpdateTitle()
	{
		this.CodexEntryButton_Refresh();
		this.PinResourceButton_Refresh();
		this.TabTitle.SetTitle(this.target.GetProperName());
		if (this.TabTitle != null)
		{
			this.TabTitle.SetTitle(this.target.GetProperName());
			MinionIdentity minionIdentity = null;
			UserNameable x = null;
			ClustercraftExteriorDoor clustercraftExteriorDoor = null;
			CommandModule commandModule = null;
			if (this.target != null)
			{
				minionIdentity = this.target.gameObject.GetComponent<MinionIdentity>();
				x = this.target.gameObject.GetComponent<UserNameable>();
				clustercraftExteriorDoor = this.target.gameObject.GetComponent<ClustercraftExteriorDoor>();
				commandModule = this.target.gameObject.GetComponent<CommandModule>();
			}
			if (minionIdentity != null)
			{
				this.TabTitle.SetSubText(minionIdentity.GetComponent<MinionResume>().GetSkillsSubtitle(), "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (x != null)
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (commandModule != null)
			{
				this.TrySetRocketTitle(commandModule);
			}
			else if (clustercraftExteriorDoor != null)
			{
				this.TrySetRocketTitle(clustercraftExteriorDoor);
			}
			else
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(false);
			}
			this.TabTitle.UpdateRenameTooltip(this.target);
		}
	}

	// Token: 0x060066FF RID: 26367 RVA: 0x0026CDF8 File Offset: 0x0026AFF8
	private void TrySetRocketTitle(ClustercraftExteriorDoor clusterCraftDoor)
	{
		if (clusterCraftDoor2.HasTargetWorld())
		{
			WorldContainer targetWorld = clusterCraftDoor2.GetTargetWorld();
			this.TabTitle.SetTitle(targetWorld.GetComponent<ClusterGridEntity>().Name);
			this.TabTitle.SetUserEditable(true);
			this.TabTitle.SetSubText(this.target.GetProperName(), "");
			this.setRocketTitleHandle = -1;
			return;
		}
		if (this.setRocketTitleHandle == -1)
		{
			this.setRocketTitleHandle = this.target.Subscribe(-71801987, delegate(object clusterCraftDoor)
			{
				this.OnRefreshData(null);
				this.target.Unsubscribe(this.setRocketTitleHandle);
				this.setRocketTitleHandle = -1;
			});
		}
	}

	// Token: 0x06006700 RID: 26368 RVA: 0x0026CE84 File Offset: 0x0026B084
	private void TrySetRocketTitle(CommandModule commandModule)
	{
		if (commandModule != null)
		{
			this.TabTitle.SetTitle(SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(commandModule.GetComponent<LaunchConditionManager>()).GetRocketName());
			this.TabTitle.SetUserEditable(true);
		}
		this.TabTitle.SetSubText(this.target.GetProperName(), "");
	}

	// Token: 0x06006701 RID: 26369 RVA: 0x0026CEE1 File Offset: 0x0026B0E1
	public TargetPanel GetActiveTab()
	{
		return this.tabHeader.ActivePanel;
	}

	// Token: 0x06006705 RID: 26373 RVA: 0x0026CF4C File Offset: 0x0026B14C
	[CompilerGenerated]
	internal static bool <PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(Tag targetTag)
	{
		foreach (Tag tag in GameTags.MaterialCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag).Contains(targetTag))
			{
				return true;
			}
		}
		foreach (Tag tag2 in GameTags.CalorieCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag2).Contains(targetTag))
			{
				return true;
			}
		}
		foreach (Tag tag3 in GameTags.UnitCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag3).Contains(targetTag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0400465B RID: 18011
	public static DetailsScreen Instance;

	// Token: 0x0400465C RID: 18012
	[SerializeField]
	private KButton CodexEntryButton;

	// Token: 0x0400465D RID: 18013
	[SerializeField]
	private KButton PinResourceButton;

	// Token: 0x0400465E RID: 18014
	[Header("Panels")]
	public Transform UserMenuPanel;

	// Token: 0x0400465F RID: 18015
	[Header("Name Editing (disabled)")]
	[SerializeField]
	private KButton CloseButton;

	// Token: 0x04004660 RID: 18016
	[Header("Tabs")]
	[SerializeField]
	private DetailTabHeader tabHeader;

	// Token: 0x04004661 RID: 18017
	[SerializeField]
	private EditableTitleBar TabTitle;

	// Token: 0x04004662 RID: 18018
	[SerializeField]
	private DetailsScreen.Screens[] screens;

	// Token: 0x04004663 RID: 18019
	[SerializeField]
	private GameObject tabHeaderContainer;

	// Token: 0x04004664 RID: 18020
	[Header("Side Screen Tabs")]
	[SerializeField]
	private DetailsScreen.SidescreenTab[] sidescreenTabs;

	// Token: 0x04004665 RID: 18021
	[SerializeField]
	private GameObject sidescreenTabHeader;

	// Token: 0x04004666 RID: 18022
	[SerializeField]
	private GameObject original_tab;

	// Token: 0x04004667 RID: 18023
	[SerializeField]
	private GameObject original_tab_body;

	// Token: 0x04004668 RID: 18024
	[Header("Side Screens")]
	[SerializeField]
	private GameObject sideScreen;

	// Token: 0x04004669 RID: 18025
	[SerializeField]
	private List<DetailsScreen.SideScreenRef> sideScreens;

	// Token: 0x0400466A RID: 18026
	[SerializeField]
	private LayoutElement tabBodyLayoutElement;

	// Token: 0x0400466B RID: 18027
	[Header("Secondary Side Screens")]
	[SerializeField]
	private GameObject sideScreen2ContentBody;

	// Token: 0x0400466C RID: 18028
	[SerializeField]
	private GameObject sideScreen2;

	// Token: 0x0400466D RID: 18029
	[SerializeField]
	private LocText sideScreen2Title;

	// Token: 0x0400466E RID: 18030
	private KScreen activeSideScreen2;

	// Token: 0x04004670 RID: 18032
	private Tag previousTargetID = null;

	// Token: 0x04004671 RID: 18033
	private bool HasActivated;

	// Token: 0x04004672 RID: 18034
	private DetailsScreen.SidescreenTabTypes selectedSidescreenTabID;

	// Token: 0x04004673 RID: 18035
	private Dictionary<KScreen, KScreen> instantiatedSecondarySideScreens = new Dictionary<KScreen, KScreen>();

	// Token: 0x04004674 RID: 18036
	private static readonly EventSystem.IntraObjectHandler<DetailsScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DetailsScreen>(delegate(DetailsScreen component, object data)
	{
		component.OnRefreshData(data);
	});

	// Token: 0x04004675 RID: 18037
	private List<KeyValuePair<DetailsScreen.SideScreenRef, int>> sortedSideScreens = new List<KeyValuePair<DetailsScreen.SideScreenRef, int>>();

	// Token: 0x04004676 RID: 18038
	private int setRocketTitleHandle = -1;

	// Token: 0x02001F29 RID: 7977
	[Serializable]
	private struct Screens
	{
		// Token: 0x040091AF RID: 37295
		public string name;

		// Token: 0x040091B0 RID: 37296
		public string displayName;

		// Token: 0x040091B1 RID: 37297
		public string tooltip;

		// Token: 0x040091B2 RID: 37298
		public Sprite icon;

		// Token: 0x040091B3 RID: 37299
		public TargetPanel screen;

		// Token: 0x040091B4 RID: 37300
		public int displayOrderPriority;

		// Token: 0x040091B5 RID: 37301
		public bool hideWhenDead;

		// Token: 0x040091B6 RID: 37302
		public HashedString focusInViewMode;

		// Token: 0x040091B7 RID: 37303
		[HideInInspector]
		public int tabIdx;
	}

	// Token: 0x02001F2A RID: 7978
	public enum SidescreenTabTypes
	{
		// Token: 0x040091B9 RID: 37305
		Config,
		// Token: 0x040091BA RID: 37306
		Errands,
		// Token: 0x040091BB RID: 37307
		Material,
		// Token: 0x040091BC RID: 37308
		Blueprints
	}

	// Token: 0x02001F2B RID: 7979
	[Serializable]
	public class SidescreenTab
	{
		// Token: 0x0600B586 RID: 46470 RVA: 0x003EE2BA File Offset: 0x003EC4BA
		private void OnTabClicked()
		{
			System.Action onClicked = this.OnClicked;
			if (onClicked == null)
			{
				return;
			}
			onClicked();
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x0600B588 RID: 46472 RVA: 0x003EE2D5 File Offset: 0x003EC4D5
		// (set) Token: 0x0600B587 RID: 46471 RVA: 0x003EE2CC File Offset: 0x003EC4CC
		public bool IsVisible { get; private set; }

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x0600B58A RID: 46474 RVA: 0x003EE2E6 File Offset: 0x003EC4E6
		// (set) Token: 0x0600B589 RID: 46473 RVA: 0x003EE2DD File Offset: 0x003EC4DD
		public bool IsSelected { get; private set; }

		// Token: 0x0600B58B RID: 46475 RVA: 0x003EE2F0 File Offset: 0x003EC4F0
		public void Initiate(GameObject originalTabInstance, GameObject originalBodyInstance, Action<DetailsScreen.SidescreenTab> on_tab_clicked_callback)
		{
			if (on_tab_clicked_callback != null)
			{
				this.OnClicked = delegate()
				{
					on_tab_clicked_callback(this);
				};
			}
			originalBodyInstance.gameObject.SetActive(false);
			if (this.OverrideBody == null)
			{
				this.bodyInstance = UnityEngine.Object.Instantiate<GameObject>(originalBodyInstance);
				this.bodyInstance.name = this.type.ToString() + " Tab - body instance";
				this.bodyInstance.SetActive(true);
				this.bodyInstance.transform.SetParent(originalBodyInstance.transform.parent, false);
			}
			else
			{
				this.bodyInstance = this.OverrideBody;
			}
			this.bodyReferences = this.bodyInstance.GetComponent<HierarchyReferences>();
			originalTabInstance.gameObject.SetActive(false);
			if (this.tabInstance == null)
			{
				this.tabInstance = UnityEngine.Object.Instantiate<GameObject>(originalTabInstance.gameObject).GetComponent<MultiToggle>();
				this.tabInstance.name = this.type.ToString() + " Tab Instance";
				this.tabInstance.gameObject.SetActive(true);
				this.tabInstance.transform.SetParent(originalTabInstance.transform.parent, false);
				MultiToggle multiToggle = this.tabInstance;
				multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnTabClicked));
				HierarchyReferences component = this.tabInstance.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("label").SetText(Strings.Get(this.Title_Key));
				component.GetReference<Image>("icon").sprite = this.Icon;
				this.tabInstance.GetComponent<ToolTip>().SetSimpleTooltip(Strings.Get(this.Tooltip_Key));
			}
		}

		// Token: 0x0600B58C RID: 46476 RVA: 0x003EE4CB File Offset: 0x003EC6CB
		public void SetSelected(bool isSelected)
		{
			this.IsSelected = isSelected;
			this.tabInstance.ChangeState(isSelected ? 1 : 0);
			this.bodyInstance.SetActive(isSelected);
		}

		// Token: 0x0600B58D RID: 46477 RVA: 0x003EE4F2 File Offset: 0x003EC6F2
		public void SetTitle(string title)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("TitleLabel"))
			{
				this.bodyReferences.GetReference<LocText>("TitleLabel").SetText(title);
			}
		}

		// Token: 0x0600B58E RID: 46478 RVA: 0x003EE52C File Offset: 0x003EC72C
		public void SetTitleVisibility(bool visible)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("Title"))
			{
				Component reference = this.bodyReferences.GetReference("Title");
				reference.gameObject.SetActive(visible);
				reference.transform.parent.GetComponent<LayoutGroup>().padding.top = (visible ? 24 : 0);
			}
		}

		// Token: 0x0600B58F RID: 46479 RVA: 0x003EE596 File Offset: 0x003EC796
		public void SetNoConfigMessageVisibility(bool visible)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("NoConfigMessage"))
			{
				this.bodyReferences.GetReference("NoConfigMessage").gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600B590 RID: 46480 RVA: 0x003EE5D4 File Offset: 0x003EC7D4
		public void RepositionTitle()
		{
			if (this.bodyReferences != null && this.bodyReferences.GetReference("Title") != null)
			{
				this.bodyReferences.GetReference("Title").transform.SetSiblingIndex(0);
			}
		}

		// Token: 0x0600B591 RID: 46481 RVA: 0x003EE622 File Offset: 0x003EC822
		public void SetVisible(bool visible)
		{
			this.IsVisible = visible;
			this.tabInstance.gameObject.SetActive(visible);
			this.bodyInstance.SetActive(this.IsSelected && this.IsVisible);
		}

		// Token: 0x0600B592 RID: 46482 RVA: 0x003EE658 File Offset: 0x003EC858
		public bool ValidateTarget(GameObject target)
		{
			return !(target == null) && (this.ValidateTargetCallback == null || this.ValidateTargetCallback(target, this));
		}

		// Token: 0x040091BD RID: 37309
		public DetailsScreen.SidescreenTabTypes type;

		// Token: 0x040091BE RID: 37310
		public string Title_Key;

		// Token: 0x040091BF RID: 37311
		public string Tooltip_Key;

		// Token: 0x040091C0 RID: 37312
		public Sprite Icon;

		// Token: 0x040091C1 RID: 37313
		public GameObject OverrideBody;

		// Token: 0x040091C2 RID: 37314
		public Func<GameObject, DetailsScreen.SidescreenTab, bool> ValidateTargetCallback;

		// Token: 0x040091C3 RID: 37315
		public System.Action OnClicked;

		// Token: 0x040091C6 RID: 37318
		[NonSerialized]
		public MultiToggle tabInstance;

		// Token: 0x040091C7 RID: 37319
		[NonSerialized]
		public GameObject bodyInstance;

		// Token: 0x040091C8 RID: 37320
		private HierarchyReferences bodyReferences;

		// Token: 0x040091C9 RID: 37321
		private const string bodyRef_Title = "Title";

		// Token: 0x040091CA RID: 37322
		private const string bodyRef_TitleLabel = "TitleLabel";

		// Token: 0x040091CB RID: 37323
		private const string bodyRef_NoConfigMessage = "NoConfigMessage";
	}

	// Token: 0x02001F2C RID: 7980
	[Serializable]
	public class SideScreenRef
	{
		// Token: 0x040091CC RID: 37324
		public string name;

		// Token: 0x040091CD RID: 37325
		public SideScreenContent screenPrefab;

		// Token: 0x040091CE RID: 37326
		public Vector2 offset;

		// Token: 0x040091CF RID: 37327
		public DetailsScreen.SidescreenTabTypes tab;

		// Token: 0x040091D0 RID: 37328
		[HideInInspector]
		public SideScreenContent screenInstance;
	}
}
