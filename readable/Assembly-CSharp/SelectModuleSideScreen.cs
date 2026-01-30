using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E73 RID: 3699
public class SelectModuleSideScreen : KScreen
{
	// Token: 0x0600758F RID: 30095 RVA: 0x002CD6D3 File Offset: 0x002CB8D3
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
		}
	}

	// Token: 0x06007590 RID: 30096 RVA: 0x002CD6E9 File Offset: 0x002CB8E9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SelectModuleSideScreen.Instance = this;
		this.SpawnButtons(null);
		this.buildSelectedModuleButton.onClick += this.OnClickBuildSelectedModule;
	}

	// Token: 0x06007591 RID: 30097 RVA: 0x002CD715 File Offset: 0x002CB915
	protected override void OnForcedCleanUp()
	{
		SelectModuleSideScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06007592 RID: 30098 RVA: 0x002CD723 File Offset: 0x002CB923
	protected override void OnCmpDisable()
	{
		this.ClearSubscriptionHandles();
		this.module = null;
		base.OnCmpDisable();
	}

	// Token: 0x06007593 RID: 30099 RVA: 0x002CD738 File Offset: 0x002CB938
	private void ClearSubscriptionHandles()
	{
		foreach (int id in this.gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(id);
		}
		this.gameSubscriptionHandles.Clear();
	}

	// Token: 0x06007594 RID: 30100 RVA: 0x002CD79C File Offset: 0x002CB99C
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.ClearSubscriptionHandles();
		this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, new Action<object>(this.UpdateBuildableStates)));
		this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-1948169901, new Action<object>(this.UpdateBuildableStates)));
	}

	// Token: 0x06007595 RID: 30101 RVA: 0x002CD804 File Offset: 0x002CBA04
	protected override void OnCleanUp()
	{
		foreach (int id in this.gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(id);
		}
		this.gameSubscriptionHandles.Clear();
		base.OnCleanUp();
	}

	// Token: 0x06007596 RID: 30102 RVA: 0x002CD86C File Offset: 0x002CBA6C
	public void SetLaunchPad(LaunchPad pad)
	{
		this.launchPad = pad;
		this.module = null;
		this.UpdateBuildableStates(null);
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			this.SetupBuildingTooltip(keyValuePair.Value.GetComponent<ToolTip>(), keyValuePair.Key);
		}
	}

	// Token: 0x06007597 RID: 30103 RVA: 0x002CD8E8 File Offset: 0x002CBAE8
	public void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.module = new_target.GetComponent<RocketModuleCluster>();
		if (this.module == null)
		{
			global::Debug.LogError("The gameObject received does not contain a RocketModuleCluster component");
			return;
		}
		this.launchPad = null;
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			this.SetupBuildingTooltip(keyValuePair.Value.GetComponent<ToolTip>(), keyValuePair.Key);
		}
		this.UpdateBuildableStates(null);
		this.buildSelectedModuleButton.isInteractable = false;
		if (this.selectedModuleDef != null)
		{
			this.SelectModule(this.selectedModuleDef);
		}
	}

	// Token: 0x06007598 RID: 30104 RVA: 0x002CD9BC File Offset: 0x002CBBBC
	private void UpdateBuildableStates(object data = null)
	{
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			if (!this.moduleBuildableState.ContainsKey(keyValuePair.Key))
			{
				this.moduleBuildableState.Add(keyValuePair.Key, false);
			}
			TechItem techItem = Db.Get().TechItems.TryGet(keyValuePair.Key.PrefabID);
			if (techItem != null)
			{
				bool active = DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem.IsComplete();
				keyValuePair.Value.SetActive(active);
			}
			else
			{
				keyValuePair.Value.SetActive(true);
			}
			this.moduleBuildableState[keyValuePair.Key] = this.TestBuildable(keyValuePair.Key);
		}
		if (this.selectedModuleDef != null)
		{
			this.ConfigureMaterialSelector();
		}
		this.SetButtonColors();
	}

	// Token: 0x06007599 RID: 30105 RVA: 0x002CDAC8 File Offset: 0x002CBCC8
	private void OnClickBuildSelectedModule()
	{
		if (this.selectedModuleDef != null)
		{
			this.OrderBuildSelectedModule();
		}
	}

	// Token: 0x0600759A RID: 30106 RVA: 0x002CDAE0 File Offset: 0x002CBCE0
	private void ConfigureMaterialSelector()
	{
		this.buildSelectedModuleButton.isInteractable = false;
		if (this.materialSelectionPanel == null)
		{
			this.materialSelectionPanel = Util.KInstantiateUI<MaterialSelectionPanel>(this.materialSelectionPanelPrefab.gameObject, base.gameObject, true);
			this.materialSelectionPanel.transform.SetSiblingIndex(this.buildSelectedModuleButton.transform.GetSiblingIndex());
		}
		this.materialSelectionPanel.ClearSelectActions();
		this.materialSelectionPanel.ConfigureScreen(this.selectedModuleDef.CraftRecipe, new MaterialSelectionPanel.GetBuildableStateDelegate(this.IsDefBuildable), new MaterialSelectionPanel.GetBuildableTooltipDelegate(this.GetErrorTooltips));
		this.materialSelectionPanel.ToggleShowDescriptorPanels(false);
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.UpdateBuildButton));
		this.materialSelectionPanel.AutoSelectAvailableMaterial();
	}

	// Token: 0x0600759B RID: 30107 RVA: 0x002CDBAC File Offset: 0x002CBDAC
	private void ConfigureFacadeSelector()
	{
		if (this.facadeSelectionPanel == null)
		{
			this.facadeSelectionPanel = Util.KInstantiateUI<FacadeSelectionPanel>(this.facadeSelectionPanelPrefab, base.gameObject, true);
			this.facadeSelectionPanel.transform.SetSiblingIndex(this.materialSelectionPanel.transform.GetSiblingIndex());
		}
		this.facadeSelectionPanel.SetBuildingDef(this.selectedModuleDef.PrefabID, null);
	}

	// Token: 0x0600759C RID: 30108 RVA: 0x002CDC16 File Offset: 0x002CBE16
	private bool IsDefBuildable(BuildingDef def)
	{
		return this.moduleBuildableState.ContainsKey(def) && this.moduleBuildableState[def];
	}

	// Token: 0x0600759D RID: 30109 RVA: 0x002CDC34 File Offset: 0x002CBE34
	private void UpdateBuildButton()
	{
		this.buildSelectedModuleButton.isInteractable = (this.materialSelectionPanel != null && this.materialSelectionPanel.AllSelectorsSelected() && this.selectedModuleDef != null && this.moduleBuildableState[this.selectedModuleDef]);
	}

	// Token: 0x0600759E RID: 30110 RVA: 0x002CDC8C File Offset: 0x002CBE8C
	public void SetButtonColors()
	{
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			MultiToggle component = keyValuePair.Value.GetComponent<MultiToggle>();
			HierarchyReferences component2 = keyValuePair.Value.GetComponent<HierarchyReferences>();
			if (!this.moduleBuildableState[keyValuePair.Key])
			{
				component2.GetReference<Image>("FG").material = PlanScreen.Instance.desaturatedUIMaterial;
				if (keyValuePair.Key == this.selectedModuleDef)
				{
					component.ChangeState(1);
				}
				else
				{
					component.ChangeState(0);
				}
			}
			else
			{
				component2.GetReference<Image>("FG").material = PlanScreen.Instance.defaultUIMaterial;
				if (keyValuePair.Key == this.selectedModuleDef)
				{
					component.ChangeState(3);
				}
				else
				{
					component.ChangeState(2);
				}
			}
		}
		this.UpdateBuildButton();
	}

	// Token: 0x0600759F RID: 30111 RVA: 0x002CDD90 File Offset: 0x002CBF90
	private bool TestBuildable(BuildingDef def)
	{
		GameObject buildingComplete = def.BuildingComplete;
		SelectModuleCondition.SelectionContext selectionContext = this.GetSelectionContext(def);
		if (selectionContext == SelectModuleCondition.SelectionContext.AddModuleAbove && this.module != null)
		{
			BuildingAttachPoint component = this.module.GetComponent<BuildingAttachPoint>();
			if (component != null && component.points[0].attachedBuilding != null && component.points[0].attachedBuilding.HasTag(GameTags.RocketModule) && !component.points[0].attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(def.HeightInCells, null))
			{
				return false;
			}
		}
		if (selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow && !this.module.GetComponent<ReorderableBuilding>().CanMoveVertically(def.HeightInCells, null))
		{
			return false;
		}
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule && this.module != null && def != null && this.module.GetComponent<Building>().Def == def)
		{
			return false;
		}
		foreach (SelectModuleCondition selectModuleCondition in buildingComplete.GetComponent<ReorderableBuilding>().buildConditions)
		{
			if ((!selectModuleCondition.IgnoreInSanboxMode() || (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive)) && !selectModuleCondition.EvaluateCondition((this.module == null) ? this.launchPad.gameObject : this.module.gameObject, def, selectionContext))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060075A0 RID: 30112 RVA: 0x002CDF20 File Offset: 0x002CC120
	private void ClearButtons()
	{
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		for (int i = this.categories.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.categories[i]);
		}
		this.categories.Clear();
		this.buttons.Clear();
	}

	// Token: 0x060075A1 RID: 30113 RVA: 0x002CDFB8 File Offset: 0x002CC1B8
	public void SpawnButtons(object data = null)
	{
		this.ClearButtons();
		GameObject gameObject = Util.KInstantiateUI(this.categoryPrefab, this.categoryContent, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		this.categories.Add(gameObject);
		component.GetReference<LocText>("label");
		Transform reference = component.GetReference<Transform>("content");
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<RocketModuleCluster>();
		using (List<string>.Enumerator enumerator = SelectModuleSideScreen.moduleButtonSortOrder.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SelectModuleSideScreen.<>c__DisplayClass42_0 CS$<>8__locals1 = new SelectModuleSideScreen.<>c__DisplayClass42_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.id = enumerator.Current;
				GameObject part = prefabsWithComponent.Find((GameObject p) => p.PrefabID().Name == CS$<>8__locals1.id);
				if (part == null)
				{
					global::Debug.LogWarning("Found an id [" + CS$<>8__locals1.id + "] in moduleButtonSortOrder in SelectModuleSideScreen.cs that doesn't have a corresponding rocket part!");
				}
				else
				{
					GameObject gameObject2 = Util.KInstantiateUI(this.moduleButtonPrefab, reference.gameObject, true);
					gameObject2.GetComponentsInChildren<Image>()[1].sprite = Def.GetUISprite(part, "ui", false).first;
					LocText componentInChildren = gameObject2.GetComponentInChildren<LocText>();
					componentInChildren.text = part.GetProperName();
					componentInChildren.alignment = TextAlignmentOptions.Bottom;
					componentInChildren.enableWordWrapping = true;
					MultiToggle component2 = gameObject2.GetComponent<MultiToggle>();
					component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
					{
						CS$<>8__locals1.<>4__this.SelectModule(part.GetComponent<Building>().Def);
					}));
					this.buttons.Add(part.GetComponent<Building>().Def, gameObject2);
					if (this.selectedModuleDef != null)
					{
						this.SelectModule(this.selectedModuleDef);
					}
				}
			}
		}
		this.UpdateBuildableStates(null);
	}

	// Token: 0x060075A2 RID: 30114 RVA: 0x002CE1A0 File Offset: 0x002CC3A0
	private void SetupBuildingTooltip(ToolTip tooltip, BuildingDef def)
	{
		tooltip.ClearMultiStringTooltip();
		string name = def.Name;
		string text = def.Effect;
		RocketModuleCluster component = def.BuildingComplete.GetComponent<RocketModuleCluster>();
		BuildingDef buildingDef = (this.GetSelectionContext(def) == SelectModuleCondition.SelectionContext.ReplaceModule) ? this.module.GetComponent<Building>().Def : null;
		if (component != null)
		{
			text = text + "\n\n" + UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.TITLE;
			float burden = component.performanceStats.burden;
			float fuelKilogramPerDistance = component.performanceStats.FuelKilogramPerDistance;
			float enginePower = component.performanceStats.enginePower;
			int heightInCells = component.GetComponent<Building>().Def.HeightInCells;
			CraftModuleInterface craftModuleInterface = null;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			int num4 = 0;
			if (base.GetComponentInParent<DetailsScreen>() != null && base.GetComponentInParent<DetailsScreen>().target.GetComponent<RocketModuleCluster>() != null)
			{
				craftModuleInterface = base.GetComponentInParent<DetailsScreen>().target.GetComponent<RocketModuleCluster>().CraftInterface;
			}
			int num5 = -1;
			if (craftModuleInterface != null)
			{
				num5 = craftModuleInterface.MaxHeight;
			}
			RocketEngineCluster component2 = component.GetComponent<RocketEngineCluster>();
			if (component2 != null)
			{
				num5 = component2.maxHeight;
			}
			float num6;
			float num7;
			if (craftModuleInterface == null)
			{
				num = burden;
				num2 = fuelKilogramPerDistance;
				num3 = enginePower;
				num6 = num3 / num;
				num7 = num6;
				num4 = heightInCells;
			}
			else
			{
				if (buildingDef != null)
				{
					RocketModulePerformance performanceStats = this.module.GetComponent<RocketModuleCluster>().performanceStats;
					num -= performanceStats.burden;
					num2 -= performanceStats.fuelKilogramPerDistance;
					num3 -= performanceStats.enginePower;
					num4 -= buildingDef.HeightInCells;
				}
				num = burden + craftModuleInterface.TotalBurden;
				num2 = fuelKilogramPerDistance + craftModuleInterface.Range;
				num3 = component.performanceStats.enginePower + craftModuleInterface.EnginePower;
				num6 = (component.performanceStats.enginePower + craftModuleInterface.EnginePower) / num;
				num7 = num6 - craftModuleInterface.EnginePower / craftModuleInterface.TotalBurden;
				num4 = craftModuleInterface.RocketHeight + heightInCells;
			}
			string arg = (burden >= 0f) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, burden), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, burden);
			string arg2 = (fuelKilogramPerDistance >= 0f) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, Math.Round((double)fuelKilogramPerDistance, 2)), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, Math.Round((double)fuelKilogramPerDistance, 2));
			string arg3 = (enginePower >= 0f) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, enginePower), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, enginePower);
			string arg4 = (num7 >= num6) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, Math.Round((double)num7, 3)), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, Math.Round((double)num7, 2));
			string arg5 = (heightInCells >= 0) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, heightInCells), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, heightInCells);
			if (num5 != -1)
			{
				text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.HEIGHT, num4, arg5, num5);
			}
			else
			{
				text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.HEIGHT_NOMAX, num4, arg5);
			}
			text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.BURDEN, num, arg);
			text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.RANGE, Math.Round((double)num2, 2), arg2);
			text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.ENGINEPOWER, num3, arg3);
			text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.SPEED, Math.Round((double)num6, 3), arg4);
			if (component.GetComponent<RocketEngineCluster>() != null)
			{
				text = text + "\n\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.ENGINE_MAX_HEIGHT, num5);
			}
		}
		tooltip.AddMultiStringTooltip(name, PlanScreen.Instance.buildingToolTipSettings.BuildButtonName);
		tooltip.AddMultiStringTooltip(text, PlanScreen.Instance.buildingToolTipSettings.BuildButtonDescription);
		this.AddErrorTooltips(tooltip, def, false);
	}

	// Token: 0x060075A3 RID: 30115 RVA: 0x002CE658 File Offset: 0x002CC858
	private SelectModuleCondition.SelectionContext GetSelectionContext(BuildingDef def)
	{
		SelectModuleCondition.SelectionContext result = SelectModuleCondition.SelectionContext.AddModuleAbove;
		if (this.launchPad == null)
		{
			if (!this.addingNewModule)
			{
				result = SelectModuleCondition.SelectionContext.ReplaceModule;
			}
			else
			{
				List<SelectModuleCondition> buildConditions = Assets.GetPrefab(this.module.GetComponent<KPrefabID>().PrefabID()).GetComponent<ReorderableBuilding>().buildConditions;
				ReorderableBuilding component = def.BuildingComplete.GetComponent<ReorderableBuilding>();
				if (buildConditions.Find((SelectModuleCondition match) => match is TopOnly) == null)
				{
					if (component.buildConditions.Find((SelectModuleCondition match) => match is EngineOnBottom) == null)
					{
						return result;
					}
				}
				result = SelectModuleCondition.SelectionContext.AddModuleBelow;
			}
		}
		return result;
	}

	// Token: 0x060075A4 RID: 30116 RVA: 0x002CE704 File Offset: 0x002CC904
	private string GetErrorTooltips(BuildingDef def)
	{
		List<SelectModuleCondition> buildConditions = def.BuildingComplete.GetComponent<ReorderableBuilding>().buildConditions;
		SelectModuleCondition.SelectionContext selectionContext = this.GetSelectionContext(def);
		string text = "";
		for (int i = 0; i < buildConditions.Count; i++)
		{
			if (!buildConditions[i].IgnoreInSanboxMode() || (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive))
			{
				GameObject gameObject = (this.module == null) ? this.launchPad.gameObject : this.module.gameObject;
				if (!buildConditions[i].EvaluateCondition(gameObject, def, selectionContext))
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "\n";
					}
					text += buildConditions[i].GetStatusTooltip(false, gameObject, def);
				}
			}
		}
		return text;
	}

	// Token: 0x060075A5 RID: 30117 RVA: 0x002CE7D0 File Offset: 0x002CC9D0
	private void AddErrorTooltips(ToolTip tooltip, BuildingDef def, bool clearFirst = false)
	{
		if (clearFirst)
		{
			tooltip.ClearMultiStringTooltip();
		}
		if (!clearFirst)
		{
			tooltip.AddMultiStringTooltip("\n", PlanScreen.Instance.buildingToolTipSettings.MaterialRequirement);
		}
		tooltip.AddMultiStringTooltip(this.GetErrorTooltips(def), PlanScreen.Instance.buildingToolTipSettings.MaterialRequirement);
	}

	// Token: 0x060075A6 RID: 30118 RVA: 0x002CE822 File Offset: 0x002CCA22
	public void SelectModule(BuildingDef def)
	{
		this.selectedModuleDef = def;
		this.ConfigureMaterialSelector();
		this.ConfigureFacadeSelector();
		this.SetButtonColors();
		this.UpdateBuildButton();
		this.AddErrorTooltips(this.buildSelectedModuleButton.GetComponent<ToolTip>(), this.selectedModuleDef, true);
	}

	// Token: 0x060075A7 RID: 30119 RVA: 0x002CE85C File Offset: 0x002CCA5C
	private void OrderBuildSelectedModule()
	{
		BuildingDef previousSelectedDef = this.selectedModuleDef;
		GameObject gameObject2;
		if (this.module != null)
		{
			GameObject gameObject = this.module.gameObject;
			if (this.addingNewModule)
			{
				gameObject2 = this.module.GetComponent<ReorderableBuilding>().AddModule(this.selectedModuleDef, this.materialSelectionPanel.GetSelectedElementAsList);
			}
			else
			{
				gameObject2 = this.module.GetComponent<ReorderableBuilding>().ConvertModule(this.selectedModuleDef, this.materialSelectionPanel.GetSelectedElementAsList);
			}
		}
		else
		{
			gameObject2 = this.launchPad.AddBaseModule(this.selectedModuleDef, this.materialSelectionPanel.GetSelectedElementAsList);
		}
		if (gameObject2 != null)
		{
			Vector2 anchoredPosition = this.mainContents.GetComponent<KScrollRect>().content.anchoredPosition;
			if (this.facadeSelectionPanel.SelectedFacade != null && this.facadeSelectionPanel.SelectedFacade != "DEFAULT_FACADE")
			{
				gameObject2.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.facadeSelectionPanel.SelectedFacade), false);
			}
			SelectTool.Instance.StartCoroutine(this.SelectNextFrame(gameObject2.GetComponent<KSelectable>(), previousSelectedDef, anchoredPosition.y));
		}
	}

	// Token: 0x060075A8 RID: 30120 RVA: 0x002CE97B File Offset: 0x002CCB7B
	private IEnumerator SelectNextFrame(KSelectable selectable, BuildingDef previousSelectedDef, float scrollPosition)
	{
		yield return 0;
		SelectTool.Instance.Select(selectable, false);
		RocketModuleSideScreen.instance.ClickAddNew(scrollPosition, previousSelectedDef);
		yield break;
	}

	// Token: 0x04005156 RID: 20822
	public RocketModule module;

	// Token: 0x04005157 RID: 20823
	private LaunchPad launchPad;

	// Token: 0x04005158 RID: 20824
	public GameObject mainContents;

	// Token: 0x04005159 RID: 20825
	[Header("Category")]
	public GameObject categoryPrefab;

	// Token: 0x0400515A RID: 20826
	public GameObject moduleButtonPrefab;

	// Token: 0x0400515B RID: 20827
	public GameObject categoryContent;

	// Token: 0x0400515C RID: 20828
	private BuildingDef selectedModuleDef;

	// Token: 0x0400515D RID: 20829
	public List<GameObject> categories = new List<GameObject>();

	// Token: 0x0400515E RID: 20830
	public Dictionary<BuildingDef, GameObject> buttons = new Dictionary<BuildingDef, GameObject>();

	// Token: 0x0400515F RID: 20831
	private Dictionary<BuildingDef, bool> moduleBuildableState = new Dictionary<BuildingDef, bool>();

	// Token: 0x04005160 RID: 20832
	public static SelectModuleSideScreen Instance;

	// Token: 0x04005161 RID: 20833
	public bool addingNewModule;

	// Token: 0x04005162 RID: 20834
	public GameObject materialSelectionPanelPrefab;

	// Token: 0x04005163 RID: 20835
	private MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x04005164 RID: 20836
	public GameObject facadeSelectionPanelPrefab;

	// Token: 0x04005165 RID: 20837
	private FacadeSelectionPanel facadeSelectionPanel;

	// Token: 0x04005166 RID: 20838
	public KButton buildSelectedModuleButton;

	// Token: 0x04005167 RID: 20839
	public ColorStyleSetting colorStyleButton;

	// Token: 0x04005168 RID: 20840
	public ColorStyleSetting colorStyleButtonSelected;

	// Token: 0x04005169 RID: 20841
	public ColorStyleSetting colorStyleButtonInactive;

	// Token: 0x0400516A RID: 20842
	public ColorStyleSetting colorStyleButtonInactiveSelected;

	// Token: 0x0400516B RID: 20843
	private List<int> gameSubscriptionHandles = new List<int>();

	// Token: 0x0400516C RID: 20844
	public static List<string> moduleButtonSortOrder = new List<string>
	{
		"CO2Engine",
		"SugarEngine",
		"SteamEngineCluster",
		"KeroseneEngineClusterSmall",
		"KeroseneEngineCluster",
		"BiodieselEngineCluster",
		"HEPEngine",
		"HydrogenEngineCluster",
		"HabitatModuleSmall",
		"HabitatModuleMedium",
		"RoboPilotModule",
		"NoseconeBasic",
		"NoseconeHarvest",
		"OrbitalCargoModule",
		"ScoutModule",
		"PioneerModule",
		"LiquidFuelTankCluster",
		"SmallOxidizerTank",
		"OxidizerTankCluster",
		"OxidizerTankLiquidCluster",
		"SolidCargoBaySmall",
		"LiquidCargoBaySmall",
		"GasCargoBaySmall",
		"CargoBayCluster",
		"LiquidCargoBayCluster",
		"GasCargoBayCluster",
		"ResearchClusterModule",
		"SpecialCargoBayCluster",
		"BatteryModule",
		"SolarPanelModule",
		"ArtifactCargoBay",
		"ScannerModule"
	};
}
