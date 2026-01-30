using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FMOD.Studio;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DDA RID: 3546
public class PlanScreen : KIconToggleMenu
{
	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x06006F2D RID: 28461 RVA: 0x002A2620 File Offset: 0x002A0820
	// (set) Token: 0x06006F2E RID: 28462 RVA: 0x002A2627 File Offset: 0x002A0827
	public static PlanScreen Instance { get; private set; }

	// Token: 0x06006F2F RID: 28463 RVA: 0x002A262F File Offset: 0x002A082F
	public static void DestroyInstance()
	{
		PlanScreen.Instance = null;
	}

	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x06006F30 RID: 28464 RVA: 0x002A2637 File Offset: 0x002A0837
	public static Dictionary<HashedString, string> IconNameMap
	{
		get
		{
			return PlanScreen.iconNameMap;
		}
	}

	// Token: 0x06006F31 RID: 28465 RVA: 0x002A263E File Offset: 0x002A083E
	private static HashedString CacheHashedString(string str)
	{
		return HashCache.Get().Add(str);
	}

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x06006F32 RID: 28466 RVA: 0x002A264B File Offset: 0x002A084B
	// (set) Token: 0x06006F33 RID: 28467 RVA: 0x002A2653 File Offset: 0x002A0853
	public ProductInfoScreen ProductInfoScreen { get; private set; }

	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x06006F34 RID: 28468 RVA: 0x002A265C File Offset: 0x002A085C
	public KIconToggleMenu.ToggleInfo ActiveCategoryToggleInfo
	{
		get
		{
			return this.activeCategoryInfo;
		}
	}

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x06006F35 RID: 28469 RVA: 0x002A2664 File Offset: 0x002A0864
	// (set) Token: 0x06006F36 RID: 28470 RVA: 0x002A266C File Offset: 0x002A086C
	public GameObject SelectedBuildingGameObject { get; private set; }

	// Token: 0x06006F37 RID: 28471 RVA: 0x002A2675 File Offset: 0x002A0875
	public override float GetSortKey()
	{
		return 2f;
	}

	// Token: 0x06006F38 RID: 28472 RVA: 0x002A267C File Offset: 0x002A087C
	public PlanScreen.RequirementsState GetBuildableState(BuildingDef def)
	{
		if (def == null)
		{
			return PlanScreen.RequirementsState.Materials;
		}
		return this._buildableStatesByID[def.PrefabID];
	}

	// Token: 0x06006F39 RID: 28473 RVA: 0x002A269C File Offset: 0x002A089C
	private bool IsDefResearched(BuildingDef def)
	{
		bool result = false;
		if (!this._researchedDefs.TryGetValue(def, out result))
		{
			result = this.UpdateDefResearched(def);
		}
		return result;
	}

	// Token: 0x06006F3A RID: 28474 RVA: 0x002A26C4 File Offset: 0x002A08C4
	private bool UpdateDefResearched(BuildingDef def)
	{
		return this._researchedDefs[def] = Db.Get().TechItems.IsTechItemComplete(def.PrefabID);
	}

	// Token: 0x06006F3B RID: 28475 RVA: 0x002A26F8 File Offset: 0x002A08F8
	protected override void OnPrefabInit()
	{
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.OnPrefabInit();
			PlanScreen.Instance = this;
			this.ProductInfoScreen = global::Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, this.recipeInfoScreenParent, false);
			this.ProductInfoScreen.rectTransform().pivot = new Vector2(0f, 0f);
			this.ProductInfoScreen.rectTransform().SetLocalPosition(new Vector3(326f, 0f, 0f));
			this.ProductInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
			KInputManager.InputChange.AddListener(new UnityAction(this.RefreshToolTip));
			this.planScreenScrollRect = base.transform.parent.GetComponentInParent<KScrollRect>();
			Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
			Game.Instance.Subscribe(1174281782, new Action<object>(this.OnActiveToolChanged));
			Game.Instance.Subscribe(1557339983, new Action<object>(this.ForceUpdateAllCategoryToggles));
		}
		this.buildingGroupsRoot.gameObject.SetActive(false);
	}

	// Token: 0x06006F3C RID: 28476 RVA: 0x002A2830 File Offset: 0x002A0A30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.useSubCategoryLayout = (KPlayerPrefs.GetInt("usePlanScreenListView") == 1);
		this.initTime = KTime.Instance.UnscaledGameTime;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			this._buildableStatesByID.Add(buildingDef.PrefabID, PlanScreen.RequirementsState.Materials);
		}
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.onSelect += this.OnClickCategory;
			this.Refresh();
			foreach (KToggle ktoggle in this.toggles)
			{
				ktoggle.group = base.GetComponent<ToggleGroup>();
			}
			this.RefreshBuildableStates(true);
			Game.Instance.Subscribe(288942073, new Action<object>(this.OnUIClear));
		}
		this.copyBuildingButton.GetComponent<MultiToggle>().onClick = delegate()
		{
			this.OnClickCopyBuilding();
		};
		this.RefreshCopyBuildingButton(null);
		Game.Instance.Subscribe(-1503271301, new Action<object>(this.RefreshCopyBuildingButton));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.CloseRecipe(false);
		});
		this.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(this.pointerEnterActions, new KScreen.PointerEnterActions(this.PointerEnter));
		this.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(this.pointerExitActions, new KScreen.PointerExitActions(this.PointerExit));
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
		this.RefreshScale(null);
		this.refreshScaleHandle = Game.Instance.Subscribe(-442024484, new Action<object>(this.RefreshScale));
		this.CacheSearchCaches();
		this.BuildButtonList();
		this.gridViewButton.onClick += this.OnClickGridView;
		this.listViewButton.onClick += this.OnClickListView;
	}

	// Token: 0x06006F3D RID: 28477 RVA: 0x002A2A78 File Offset: 0x002A0C78
	private void RefreshScale(object data = null)
	{
		base.GetComponent<GridLayoutGroup>().cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(54f, 50f) : new Vector2(45f, 45f));
		this.toggles.ForEach(delegate(KToggle to)
		{
			to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
		});
		LayoutElement component = this.copyBuildingButton.GetComponent<LayoutElement>();
		component.minWidth = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		component.minHeight = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		base.gameObject.rectTransform().anchoredPosition = new Vector2(0f, (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? -68 : -74));
		this.adjacentPinnedButtons.GetComponent<HorizontalLayoutGroup>().padding.bottom = (ScreenResolutionMonitor.UsingGamepadUIMode() ? 14 : 6);
		Vector2 sizeDelta = this.buildingGroupsRoot.rectTransform().sizeDelta;
		Vector2 vector = ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(320f, sizeDelta.y) : new Vector2(264f, sizeDelta.y);
		this.buildingGroupsRoot.rectTransform().sizeDelta = vector;
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (this.useSubCategoryLayout)
			{
				componentInChildren.constraintCount = 1;
				componentInChildren.cellSize = new Vector2(vector.x - 24f, 36f);
			}
			else
			{
				componentInChildren.constraintCount = 3;
				componentInChildren.cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize : PlanScreen.standarduildingButtonSize);
			}
		}
		this.ProductInfoScreen.rectTransform().anchoredPosition = new Vector2(vector.x + 8f, this.ProductInfoScreen.rectTransform().anchoredPosition.y);
	}

	// Token: 0x06006F3E RID: 28478 RVA: 0x002A2C80 File Offset: 0x002A0E80
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x06006F3F RID: 28479 RVA: 0x002A2C9E File Offset: 0x002A0E9E
	protected override void OnCleanUp()
	{
		if (Game.Instance != null)
		{
			Game.Instance.Unsubscribe(this.refreshScaleHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06006F40 RID: 28480 RVA: 0x002A2CC4 File Offset: 0x002A0EC4
	private void OnClickCopyBuilding()
	{
		if (!this.LastSelectedBuilding.IsNullOrDestroyed() && this.LastSelectedBuilding.gameObject.activeInHierarchy && (!this.lastSelectedBuilding.Def.DebugOnly || DebugHandler.InstantBuildMode))
		{
			PlanScreen.Instance.CopyBuildingOrder(this.LastSelectedBuilding);
			return;
		}
		if (this.lastSelectedBuildingDef != null && (!this.lastSelectedBuildingDef.DebugOnly || DebugHandler.InstantBuildMode))
		{
			PlanScreen.Instance.CopyBuildingOrder(this.lastSelectedBuildingDef, this.LastSelectedBuildingFacade);
		}
	}

	// Token: 0x06006F41 RID: 28481 RVA: 0x002A2D52 File Offset: 0x002A0F52
	private void OnClickListView()
	{
		this.useSubCategoryLayout = true;
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 1);
	}

	// Token: 0x06006F42 RID: 28482 RVA: 0x002A2D7A File Offset: 0x002A0F7A
	private void OnClickGridView()
	{
		this.useSubCategoryLayout = false;
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 0);
	}

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x06006F43 RID: 28483 RVA: 0x002A2DA2 File Offset: 0x002A0FA2
	// (set) Token: 0x06006F44 RID: 28484 RVA: 0x002A2DAC File Offset: 0x002A0FAC
	private Building LastSelectedBuilding
	{
		get
		{
			return this.lastSelectedBuilding;
		}
		set
		{
			this.lastSelectedBuilding = value;
			if (this.lastSelectedBuilding != null)
			{
				this.lastSelectedBuildingDef = this.lastSelectedBuilding.Def;
				if (this.lastSelectedBuilding.gameObject.activeInHierarchy)
				{
					this.LastSelectedBuildingFacade = this.lastSelectedBuilding.GetComponent<BuildingFacade>().CurrentFacade;
				}
			}
		}
	}

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x06006F45 RID: 28485 RVA: 0x002A2E07 File Offset: 0x002A1007
	// (set) Token: 0x06006F46 RID: 28486 RVA: 0x002A2E0F File Offset: 0x002A100F
	public string LastSelectedBuildingFacade
	{
		get
		{
			return this.lastSelectedBuildingFacade;
		}
		set
		{
			this.lastSelectedBuildingFacade = value;
		}
	}

	// Token: 0x06006F47 RID: 28487 RVA: 0x002A2E18 File Offset: 0x002A1018
	public void RefreshCopyBuildingButton(object _ = null)
	{
		this.adjacentPinnedButtons.rectTransform().anchoredPosition = new Vector2(Mathf.Min(base.gameObject.rectTransform().sizeDelta.x, base.transform.parent.rectTransform().rect.width), 0f);
		MultiToggle component = this.copyBuildingButton.GetComponent<MultiToggle>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Building component2 = SelectTool.Instance.selected.GetComponent<Building>();
			if (component2 != null && component2.Def.ShouldShowInBuildMenu() && component2.Def.IsAvailable())
			{
				this.LastSelectedBuilding = component2;
			}
		}
		if (this.lastSelectedBuildingDef != null)
		{
			component.gameObject.SetActive(PlanScreen.Instance.gameObject.activeInHierarchy);
			Sprite sprite = this.lastSelectedBuildingDef.GetUISprite("ui", false);
			if (this.LastSelectedBuildingFacade != null && this.LastSelectedBuildingFacade != "DEFAULT_FACADE" && Db.Get().Permits.BuildingFacades.TryGet(this.LastSelectedBuildingFacade) != null)
			{
				sprite = Def.GetFacadeUISprite(this.LastSelectedBuildingFacade);
			}
			component.transform.Find("FG").GetComponent<Image>().sprite = sprite;
			component.transform.Find("FG").GetComponent<Image>().color = Color.white;
			component.ChangeState(1);
			return;
		}
		component.gameObject.SetActive(false);
		component.ChangeState(0);
	}

	// Token: 0x06006F48 RID: 28488 RVA: 0x002A2FB0 File Offset: 0x002A11B0
	public void RefreshToolTip()
	{
		for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
		{
			PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
			if (Game.IsCorrectDlcActiveForCurrentSave(planInfo))
			{
				global::Action action = (i < 14) ? (global::Action.Plan1 + i) : global::Action.NumActions;
				string str = HashCache.Get().Get(planInfo.category).ToUpper();
				this.toggleInfo[i].tooltip = GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action);
			}
		}
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
	}

	// Token: 0x06006F49 RID: 28489 RVA: 0x002A3068 File Offset: 0x002A1268
	public void Refresh()
	{
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		if (this.tagCategoryMap == null)
		{
			int num = 0;
			this.tagCategoryMap = new Dictionary<Tag, HashedString>();
			this.tagOrderMap = new Dictionary<Tag, int>();
			if (TUNING.BUILDINGS.PLANORDER.Count > 15)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Insufficient keys to cover root plan menu",
					"Max of 14 keys supported but TUNING.BUILDINGS.PLANORDER has " + TUNING.BUILDINGS.PLANORDER.Count.ToString()
				});
			}
			this.toggleEntries.Clear();
			for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
			{
				PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
				if (Game.IsCorrectDlcActiveForCurrentSave(planInfo))
				{
					global::Action action = (i < 15) ? (global::Action.Plan1 + i) : global::Action.NumActions;
					string icon = PlanScreen.iconNameMap[planInfo.category];
					string str = HashCache.Get().Get(planInfo.category).ToUpper();
					KIconToggleMenu.ToggleInfo toggleInfo = new KIconToggleMenu.ToggleInfo(UI.StripLinkFormatting(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".NAME")), icon, planInfo.category, action, GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action), "");
					list.Add(toggleInfo);
					PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, this.tagCategoryMap, this.tagOrderMap, ref num);
					List<BuildingDef> list2 = new List<BuildingDef>();
					foreach (BuildingDef buildingDef in Assets.BuildingDefs)
					{
						HashedString x;
						if (buildingDef.IsAvailable() && this.tagCategoryMap.TryGetValue(buildingDef.Tag, out x) && !(x != planInfo.category))
						{
							list2.Add(buildingDef);
						}
					}
					this.toggleEntries.Add(new PlanScreen.ToggleEntry(toggleInfo, planInfo.category, list2, planInfo.hideIfNotResearched));
				}
			}
			base.Setup(list);
			this.toggleBouncers.Clear();
			this.toggles.ForEach(delegate(KToggle to)
			{
				foreach (ImageToggleState imageToggleState in to.GetComponents<ImageToggleState>())
				{
					if (imageToggleState.TargetImage.sprite != null && imageToggleState.TargetImage.name == "FG" && !imageToggleState.useSprites)
					{
						imageToggleState.SetSprites(Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"), imageToggleState.TargetImage.sprite, imageToggleState.TargetImage.sprite, Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"));
					}
				}
				to.GetComponent<KToggle>().soundPlayer.Enabled = false;
				to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
				this.toggleBouncers.Add(to, to.GetComponent<Bouncer>());
			});
			for (int j = 0; j < this.toggleEntries.Count; j++)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[j];
				toggleEntry.CollectToggleImages();
				this.toggleEntries[j] = toggleEntry;
			}
			this.ForceUpdateAllCategoryToggles(null);
		}
	}

	// Token: 0x06006F4A RID: 28490 RVA: 0x002A32F8 File Offset: 0x002A14F8
	private void ForceUpdateAllCategoryToggles(object data = null)
	{
		this.forceUpdateAllCategoryToggles = true;
	}

	// Token: 0x06006F4B RID: 28491 RVA: 0x002A3301 File Offset: 0x002A1501
	public void ForceRefreshAllBuildingToggles()
	{
		this.forceRefreshAllBuildings = true;
	}

	// Token: 0x06006F4C RID: 28492 RVA: 0x002A330C File Offset: 0x002A150C
	public void CopyBuildingOrder(BuildingDef buildingDef, string facadeID)
	{
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				if (buildingDef.PrefabID == keyValuePair.Key)
				{
					this.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
					this.OnSelectBuilding(this.activeCategoryBuildingToggles[buildingDef].gameObject, buildingDef, facadeID);
					this.ProductInfoScreen.ToggleExpandedInfo(true);
					break;
				}
			}
		}
	}

	// Token: 0x06006F4D RID: 28493 RVA: 0x002A33EC File Offset: 0x002A15EC
	public void CopyBuildingOrder(Building building)
	{
		this.CopyBuildingOrder(building.Def, building.GetComponent<BuildingFacade>().CurrentFacade);
		if (this.ProductInfoScreen.materialSelectionPanel == null)
		{
			DebugUtil.DevLogError(building.Def.name + " def likely needs to be marked def.ShowInBuildMenu = false");
			return;
		}
		this.ProductInfoScreen.materialSelectionPanel.SelectSourcesMaterials(building);
		Rotatable component = building.GetComponent<Rotatable>();
		if (component != null)
		{
			BuildTool.Instance.SetToolOrientation(component.GetOrientation());
		}
	}

	// Token: 0x06006F4E RID: 28494 RVA: 0x002A3470 File Offset: 0x002A1670
	private static void PopulateOrderInfo(HashedString category, object data, Dictionary<Tag, HashedString> category_map, Dictionary<Tag, int> order_map, ref int building_index)
	{
		if (data.GetType() == typeof(PlanScreen.PlanInfo))
		{
			PlanScreen.PlanInfo planInfo = (PlanScreen.PlanInfo)data;
			PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, category_map, order_map, ref building_index);
			return;
		}
		foreach (KeyValuePair<string, string> keyValuePair in ((List<KeyValuePair<string, string>>)data))
		{
			Tag key = new Tag(keyValuePair.Key);
			category_map[key] = category;
			order_map[key] = building_index;
			building_index++;
		}
	}

	// Token: 0x06006F4F RID: 28495 RVA: 0x002A3518 File Offset: 0x002A1718
	protected override void OnCmpEnable()
	{
		this.Refresh();
		this.RefreshCopyBuildingButton(null);
	}

	// Token: 0x06006F50 RID: 28496 RVA: 0x002A3527 File Offset: 0x002A1727
	protected override void OnCmpDisable()
	{
		this.ClearButtons();
	}

	// Token: 0x06006F51 RID: 28497 RVA: 0x002A3530 File Offset: 0x002A1730
	private void ClearButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
		}
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair2 in this.allBuildingToggles)
		{
			keyValuePair2.Value.gameObject.SetActive(false);
		}
		this.activeCategoryBuildingToggles.Clear();
		this.copyBuildingButton.gameObject.SetActive(false);
		this.copyBuildingButton.GetComponent<MultiToggle>().ChangeState(0);
	}

	// Token: 0x06006F52 RID: 28498 RVA: 0x002A35F8 File Offset: 0x002A17F8
	public void OnSelectBuilding(GameObject button_go, BuildingDef def, string facadeID = null)
	{
		if (button_go == null)
		{
			global::Debug.Log("Button gameObject is null", base.gameObject);
			return;
		}
		if (button_go == this.SelectedBuildingGameObject)
		{
			this.CloseRecipe(true);
			return;
		}
		this.ignoreToolChangeMessages++;
		PlanBuildingToggle planBuildingToggle = null;
		if (this.currentlySelectedToggle != null)
		{
			planBuildingToggle = this.currentlySelectedToggle.GetComponent<PlanBuildingToggle>();
		}
		this.SelectedBuildingGameObject = button_go;
		this.currentlySelectedToggle = button_go.GetComponent<KToggle>();
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		HashedString category = this.tagCategoryMap[def.Tag];
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry) && toggleEntry.pendingResearchAttentions.Contains(def.Tag))
		{
			toggleEntry.pendingResearchAttentions.Remove(def.Tag);
			button_go.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			if (toggleEntry.pendingResearchAttentions.Count == 0)
			{
				toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			}
		}
		this.ProductInfoScreen.ClearProduct(false);
		if (planBuildingToggle != null)
		{
			planBuildingToggle.Refresh(BuildingGroupScreen.SearchIsEmpty ? null : new bool?(this.buildingDefSearchCaches[def.PrefabID].IsPassingScore()));
		}
		ToolMenu.Instance.ClearSelection();
		PrebuildTool.Instance.Activate(def, this.GetTooltipForBuildable(def));
		this.LastSelectedBuilding = def.BuildingComplete.GetComponent<Building>();
		this.RefreshCopyBuildingButton(null);
		this.ProductInfoScreen.Show(true);
		this.ProductInfoScreen.ConfigureScreen(def, facadeID);
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x06006F53 RID: 28499 RVA: 0x002A3798 File Offset: 0x002A1998
	private void RefreshBuildableStates(bool force_update)
	{
		if (Assets.BuildingDefs == null || Assets.BuildingDefs.Count == 0)
		{
			return;
		}
		if (this.timeSinceNotificationPing < this.specialNotificationEmbellishDelay)
		{
			this.timeSinceNotificationPing += Time.unscaledDeltaTime;
		}
		if (this.timeSinceNotificationPing >= this.notificationPingExpire)
		{
			this.notificationPingCount = 0;
		}
		int num = 10;
		if (force_update)
		{
			num = Assets.BuildingDefs.Count;
			this.buildable_state_update_idx = 0;
		}
		ListPool<HashedString, PlanScreen>.PooledList pooledList = ListPool<HashedString, PlanScreen>.Allocate();
		for (int i = 0; i < num; i++)
		{
			this.buildable_state_update_idx = (this.buildable_state_update_idx + 1) % Assets.BuildingDefs.Count;
			BuildingDef buildingDef = Assets.BuildingDefs[this.buildable_state_update_idx];
			PlanScreen.RequirementsState buildableStateForDef = this.GetBuildableStateForDef(buildingDef);
			HashedString hashedString;
			if (this.tagCategoryMap.TryGetValue(buildingDef.Tag, out hashedString) && this._buildableStatesByID[buildingDef.PrefabID] != buildableStateForDef)
			{
				this._buildableStatesByID[buildingDef.PrefabID] = buildableStateForDef;
				if (this.ProductInfoScreen.currentDef == buildingDef)
				{
					this.ignoreToolChangeMessages++;
					this.ProductInfoScreen.ClearProduct(false);
					this.ProductInfoScreen.Show(true);
					this.ProductInfoScreen.ConfigureScreen(buildingDef);
					this.ignoreToolChangeMessages--;
				}
				if (buildableStateForDef == PlanScreen.RequirementsState.Complete)
				{
					foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
					{
						if ((HashedString)toggleInfo.userData == hashedString)
						{
							Bouncer bouncer = this.toggleBouncers[toggleInfo.toggle];
							if (bouncer != null && !bouncer.IsBouncing() && !pooledList.Contains(hashedString))
							{
								pooledList.Add(hashedString);
								bouncer.Bounce();
								if (KTime.Instance.UnscaledGameTime - this.initTime > 1.5f)
								{
									if (this.timeSinceNotificationPing >= this.specialNotificationEmbellishDelay)
									{
										string sound = GlobalAssets.GetSound("NewBuildable_Embellishment", false);
										if (sound != null)
										{
											SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
										}
									}
									string sound2 = GlobalAssets.GetSound("NewBuildable", false);
									if (sound2 != null)
									{
										EventInstance instance = SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
										instance.setParameterByName("playCount", (float)this.notificationPingCount, false);
										SoundEvent.EndOneShot(instance);
									}
								}
								this.timeSinceNotificationPing = 0f;
								this.notificationPingCount++;
							}
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06006F54 RID: 28500 RVA: 0x002A3A68 File Offset: 0x002A1C68
	private PlanScreen.RequirementsState GetBuildableStateForDef(BuildingDef def)
	{
		if (!def.IsAvailable())
		{
			return PlanScreen.RequirementsState.Invalid;
		}
		PlanScreen.RequirementsState result = PlanScreen.RequirementsState.Complete;
		KPrefabID component = def.BuildingComplete.GetComponent<KPrefabID>();
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !this.IsDefResearched(def))
		{
			result = PlanScreen.RequirementsState.Tech;
		}
		else if (component.HasTag(GameTags.Telepad) && ClusterUtil.ActiveWorldHasPrinter())
		{
			result = PlanScreen.RequirementsState.TelepadBuilt;
		}
		else if (component.HasTag(GameTags.RocketInteriorBuilding) && !ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorOnly;
		}
		else if (component.HasTag(GameTags.NotRocketInteriorBuilding) && ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorForbidden;
		}
		else if (component.HasTag(GameTags.UniquePerWorld) && BuildingInventory.Instance.BuildingCountForWorld_BAD_PERF(def.Tag, ClusterManager.Instance.activeWorldId) > 0)
		{
			result = PlanScreen.RequirementsState.UniquePerWorld;
		}
		else if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !ProductInfoScreen.MaterialsMet(def.CraftRecipe))
		{
			result = PlanScreen.RequirementsState.Materials;
		}
		return result;
	}

	// Token: 0x06006F55 RID: 28501 RVA: 0x002A3B4C File Offset: 0x002A1D4C
	private void SetCategoryButtonState()
	{
		this.nextCategoryToUpdateIDX = (this.nextCategoryToUpdateIDX + 1) % this.toggleEntries.Count;
		for (int i = 0; i < this.toggleEntries.Count; i++)
		{
			if (this.forceUpdateAllCategoryToggles || i == this.nextCategoryToUpdateIDX)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[i];
				KIconToggleMenu.ToggleInfo toggleInfo = toggleEntry.toggleInfo;
				toggleInfo.toggle.ActivateFlourish(this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData);
				bool flag = false;
				bool flag2 = true;
				if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
				{
					flag = true;
					flag2 = false;
				}
				else
				{
					foreach (BuildingDef def in toggleEntry.buildingDefs)
					{
						if (this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete)
						{
							flag = true;
							flag2 = false;
							break;
						}
					}
					if (flag2 && toggleEntry.AreAnyRequiredTechItemsAvailable())
					{
						flag2 = false;
					}
				}
				this.CategoryInteractive[toggleInfo] = !flag2;
				GameObject gameObject = toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject;
				if (!flag)
				{
					if (flag2 && toggleEntry.hideIfNotResearched)
					{
						toggleInfo.toggle.gameObject.SetActive(false);
					}
					else if (flag2)
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(true);
					}
					else
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(false);
					}
					ImageToggleState.State state = (this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData) ? ImageToggleState.State.DisabledActive : ImageToggleState.State.Disabled;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state);
					}
				}
				else
				{
					toggleInfo.toggle.gameObject.SetActive(true);
					gameObject.gameObject.SetActive(false);
					ImageToggleState.State state2 = (this.activeCategoryInfo == null || toggleInfo.userData != this.activeCategoryInfo.userData) ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state2);
					}
				}
			}
		}
		this.RefreshCopyBuildingButton(null);
		this.forceUpdateAllCategoryToggles = false;
	}

	// Token: 0x06006F56 RID: 28502 RVA: 0x002A3DB8 File Offset: 0x002A1FB8
	private void DeactivateBuildTools()
	{
		InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
		if (activeTool != null)
		{
			Type type = activeTool.GetType();
			if (type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type) || type == typeof(PrebuildTool))
			{
				activeTool.DeactivateTool(null);
				PlayerController.Instance.ActivateTool(SelectTool.Instance);
			}
		}
	}

	// Token: 0x06006F57 RID: 28503 RVA: 0x002A3E2C File Offset: 0x002A202C
	public void CloseRecipe(bool playSound = false)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
		this.SelectedBuildingGameObject = null;
	}

	// Token: 0x06006F58 RID: 28504 RVA: 0x002A3EB4 File Offset: 0x002A20B4
	public void SoftCloseRecipe()
	{
		this.ignoreToolChangeMessages++;
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		this.currentlySelectedToggle = null;
		this.SelectedBuildingGameObject = null;
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x06006F59 RID: 28505 RVA: 0x002A3F38 File Offset: 0x002A2138
	public void CloseCategoryPanel(bool playSound = true)
	{
		this.activeCategoryInfo = null;
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Collapse(delegate(object s)
		{
			this.ClearButtons();
			this.buildingGroupsRoot.gameObject.SetActive(false);
			this.ForceUpdateAllCategoryToggles(null);
		});
		this.PlanCategoryLabel.text = "";
		this.ForceUpdateAllCategoryToggles(null);
	}

	// Token: 0x06006F5A RID: 28506 RVA: 0x002A3F94 File Offset: 0x002A2194
	private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.CloseRecipe(false);
		if (!this.CategoryInteractive.ContainsKey(toggle_info) || !this.CategoryInteractive[toggle_info])
		{
			this.CloseCategoryPanel(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		if (this.activeCategoryInfo == toggle_info)
		{
			this.CloseCategoryPanel(true);
		}
		else
		{
			this.OpenCategoryPanel(toggle_info, true);
		}
		this.ConfigurePanelSize(null);
		this.SetScrollPoint(0f);
	}

	// Token: 0x06006F5B RID: 28507 RVA: 0x002A4008 File Offset: 0x002A2208
	private void OpenCategoryPanel(KIconToggleMenu.ToggleInfo toggle_info, bool play_sound = true)
	{
		HashedString hashedString = (HashedString)toggle_info.userData;
		if (BuildingGroupScreen.Instance != null)
		{
			BuildingGroupScreen.Instance.ClearSearch();
		}
		this.ClearButtons();
		this.buildingGroupsRoot.gameObject.SetActive(true);
		this.activeCategoryInfo = toggle_info;
		if (play_sound)
		{
			UISounds.PlaySound(UISounds.Sound.ClickObject);
		}
		this.BuildButtonList();
		this.UpdateBuildingButtonList(this.activeCategoryInfo);
		this.RefreshCategoryPanelTitle();
		this.ForceUpdateAllCategoryToggles(null);
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Expand(null);
	}

	// Token: 0x06006F5C RID: 28508 RVA: 0x002A4090 File Offset: 0x002A2290
	public void RefreshCategoryPanelTitle()
	{
		if (this.activeCategoryInfo != null)
		{
			this.PlanCategoryLabel.text = this.activeCategoryInfo.text.ToUpper();
		}
		if (!BuildingGroupScreen.SearchIsEmpty)
		{
			this.PlanCategoryLabel.text = UI.BUILDMENU.SEARCH_RESULTS_HEADER;
		}
	}

	// Token: 0x06006F5D RID: 28509 RVA: 0x002A40DC File Offset: 0x002A22DC
	public void RefreshSearch()
	{
		if (BuildingGroupScreen.SearchIsEmpty)
		{
			using (Dictionary<string, SearchUtil.SubcategoryCache>.Enumerator enumerator = this.subcategorySearchCaches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, SearchUtil.SubcategoryCache> keyValuePair = enumerator.Current;
					keyValuePair.Value.Reset();
				}
				goto IL_C5;
			}
		}
		string searchStringUpper = BuildingGroupScreen.Instance.inputField.text.ToUpper().Trim();
		foreach (KeyValuePair<string, SearchUtil.SubcategoryCache> keyValuePair2 in this.subcategorySearchCaches)
		{
			try
			{
				keyValuePair2.Value.Bind(searchStringUpper);
			}
			catch (Exception ex)
			{
				KCrashReporter.ReportDevNotification("Fuzzy score bind failed", Environment.StackTrace, ex.Message, false, null);
				keyValuePair2.Value.Reset();
			}
		}
		IL_C5:
		this.SortButtons();
		this.SortSubcategories();
		this.ForceRefreshAllBuildingToggles();
	}

	// Token: 0x06006F5E RID: 28510 RVA: 0x002A41E8 File Offset: 0x002A23E8
	public void OpenCategoryByName(string category)
	{
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry))
		{
			this.OpenCategoryPanel(toggleEntry.toggleInfo, false);
			this.ConfigurePanelSize(null);
		}
	}

	// Token: 0x06006F5F RID: 28511 RVA: 0x002A421C File Offset: 0x002A241C
	private void UpdateBuildingButton(int i, bool checkScore)
	{
		KeyValuePair<string, PlanBuildingToggle> keyValuePair = this.allBuildingToggles.ElementAt(i);
		bool? passesSearchFilter = null;
		if (checkScore)
		{
			passesSearchFilter = new bool?(this.buildingDefSearchCaches[keyValuePair.Key].IsPassingScore() || this.subcategorySearchCachesByBuildingPrefab[keyValuePair.Key].subcategory.IsPassingScore());
		}
		if (keyValuePair.Value.Refresh(passesSearchFilter))
		{
			this.categoryPanelSizeNeedsRefresh = true;
		}
		keyValuePair.Value.SwitchViewMode(this.useSubCategoryLayout);
	}

	// Token: 0x06006F60 RID: 28512 RVA: 0x002A42A8 File Offset: 0x002A24A8
	private void UpdateBuildingButtonList(KIconToggleMenu.ToggleInfo toggle_info)
	{
		KToggle toggle = toggle_info.toggle;
		if (toggle == null)
		{
			foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
			{
				if (toggleInfo.userData == toggle_info.userData)
				{
					toggle = toggleInfo.toggle;
					break;
				}
			}
		}
		bool checkScore = !BuildingGroupScreen.SearchIsEmpty;
		bool flag = false;
		if (toggle != null && this.allBuildingToggles.Count != 0)
		{
			if (this.forceRefreshAllBuildings)
			{
				this.forceRefreshAllBuildings = false;
				for (int num = 0; num != this.allBuildingToggles.Count; num++)
				{
					this.UpdateBuildingButton(num, checkScore);
				}
				flag = this.categoryPanelSizeNeedsRefresh;
			}
			else
			{
				for (int i = 0; i < this.maxToggleRefreshPerFrame; i++)
				{
					if (this.building_button_refresh_idx >= this.allBuildingToggles.Count)
					{
						this.building_button_refresh_idx = 0;
					}
					this.UpdateBuildingButton(this.building_button_refresh_idx, checkScore);
					this.building_button_refresh_idx++;
				}
			}
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (!(componentInChildren == null))
			{
				bool flag2 = false;
				for (int j = 0; j < componentInChildren.transform.childCount; j++)
				{
					if (componentInChildren.transform.GetChild(j).gameObject.activeSelf)
					{
						flag2 = true;
						break;
					}
				}
				if (keyValuePair.Value.activeSelf != flag2)
				{
					keyValuePair.Value.SetActive(flag2);
				}
			}
		}
		if (flag || (this.categoryPanelSizeNeedsRefresh && this.building_button_refresh_idx >= this.activeCategoryBuildingToggles.Count))
		{
			this.categoryPanelSizeNeedsRefresh = false;
			this.ConfigurePanelSize(null);
		}
	}

	// Token: 0x06006F61 RID: 28513 RVA: 0x002A44A4 File Offset: 0x002A26A4
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		this.RefreshBuildableStates(false);
		this.SetCategoryButtonState();
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
	}

	// Token: 0x06006F62 RID: 28514 RVA: 0x002A44CE File Offset: 0x002A26CE
	private static bool TryGetNewSubCategoryName(string subCategoryName, out StringEntry newBuildCategory)
	{
		return Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + subCategoryName.ToUpper() + ".BUILDMENUTITLE", out newBuildCategory);
	}

	// Token: 0x06006F63 RID: 28515 RVA: 0x002A44EC File Offset: 0x002A26EC
	private void CacheSearchCaches()
	{
		this.<CacheSearchCaches>g__ManifestSubcategoryCache|130_0("default", string.Empty);
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				SearchUtil.BuildingDefCache buildingDefCache = null;
				bool flag = buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && Game.IsCorrectDlcActiveForCurrentSave(buildingDef);
				if (flag && !this.buildingDefSearchCaches.TryGetValue(buildingDef.PrefabID, out buildingDefCache))
				{
					buildingDefCache = SearchUtil.MakeBuildingDefCache(buildingDef);
					this.buildingDefSearchCaches[buildingDef.PrefabID] = buildingDefCache;
				}
				string value = keyValuePair.Value;
				StringEntry stringEntry;
				PlanScreen.TryGetNewSubCategoryName(value, out stringEntry);
				SearchUtil.SubcategoryCache subcategoryCache = this.<CacheSearchCaches>g__ManifestSubcategoryCache|130_0(value, (stringEntry != null) ? stringEntry.String : null);
				if (buildingDefCache != null)
				{
					subcategoryCache.buildingDefs.Add(buildingDefCache);
				}
				if (flag)
				{
					this.subcategorySearchCachesByBuildingPrefab[buildingDef.PrefabID] = subcategoryCache;
				}
			}
		}
	}

	// Token: 0x06006F64 RID: 28516 RVA: 0x002A463C File Offset: 0x002A283C
	private void CollectRequiredBuildingDefs(List<BuildingDef> defs)
	{
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && Game.IsCorrectDlcActiveForCurrentSave(buildingDef))
				{
					defs.Add(buildingDef);
				}
			}
		}
	}

	// Token: 0x06006F65 RID: 28517 RVA: 0x002A46EC File Offset: 0x002A28EC
	private int CompareScores(global::Tuple<PlanBuildingToggle, string> a, global::Tuple<PlanBuildingToggle, string> b)
	{
		return this.buildingDefSearchCaches[a.second].CompareTo(this.buildingDefSearchCaches[b.second]);
	}

	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x06006F66 RID: 28518 RVA: 0x002A4715 File Offset: 0x002A2915
	private Comparer<global::Tuple<PlanBuildingToggle, string>> BuildingDefComparer
	{
		get
		{
			if (this.buildingDefComparer == null)
			{
				this.buildingDefComparer = Comparer<global::Tuple<PlanBuildingToggle, string>>.Create(new Comparison<global::Tuple<PlanBuildingToggle, string>>(this.CompareScores));
			}
			return this.buildingDefComparer;
		}
	}

	// Token: 0x06006F67 RID: 28519 RVA: 0x002A473C File Offset: 0x002A293C
	private void SortButtons()
	{
		ListPool<BuildingDef, PlanScreen>.PooledList pooledList = ListPool<BuildingDef, PlanScreen>.Allocate();
		this.CollectRequiredBuildingDefs(pooledList);
		ListPool<global::Tuple<PlanBuildingToggle, string>, PlanScreen>.PooledList pooledList2 = ListPool<global::Tuple<PlanBuildingToggle, string>, PlanScreen>.Allocate();
		foreach (BuildingDef buildingDef in pooledList)
		{
			global::Tuple<PlanBuildingToggle, string> tuple = new global::Tuple<PlanBuildingToggle, string>(this.allBuildingToggles[buildingDef.PrefabID], buildingDef.PrefabID);
			int num = pooledList2.BinarySearch(tuple, this.BuildingDefComparer);
			if (num < 0)
			{
				num = ~num;
			}
			while (num < pooledList2.Count && this.CompareScores(tuple, pooledList2[num]) == 0)
			{
				num++;
			}
			pooledList2.Insert(num, tuple);
		}
		pooledList.Recycle();
		foreach (global::Tuple<PlanBuildingToggle, string> tuple2 in pooledList2)
		{
			tuple2.first.transform.SetAsLastSibling();
		}
		pooledList2.Recycle();
	}

	// Token: 0x06006F68 RID: 28520 RVA: 0x002A4850 File Offset: 0x002A2A50
	private void SortSubcategories()
	{
		Comparer<global::Tuple<GameObject, string>> comparer = Comparer<global::Tuple<GameObject, string>>.Create(new Comparison<global::Tuple<GameObject, string>>(this.<SortSubcategories>g__CompareScores|137_0));
		ListPool<global::Tuple<GameObject, string>, PlanScreen>.PooledList pooledList = ListPool<global::Tuple<GameObject, string>, PlanScreen>.Allocate();
		foreach (string text in this.stableSubcategoryOrder)
		{
			global::Tuple<GameObject, string> tuple = new global::Tuple<GameObject, string>(this.allSubCategoryObjects[text], text);
			int num = pooledList.BinarySearch(tuple, comparer);
			if (num < 0)
			{
				num = ~num;
			}
			while (num < pooledList.Count && this.<SortSubcategories>g__CompareScores|137_0(tuple, pooledList[num]) == 0)
			{
				num++;
			}
			pooledList.Insert(num, tuple);
		}
		foreach (global::Tuple<GameObject, string> tuple2 in pooledList)
		{
			tuple2.first.transform.SetAsLastSibling();
		}
		pooledList.Recycle();
	}

	// Token: 0x06006F69 RID: 28521 RVA: 0x002A4958 File Offset: 0x002A2B58
	private void BuildButtonList()
	{
		this.activeCategoryBuildingToggles.Clear();
		this.CacheSearchCaches();
		DictionaryPool<string, HashedString, PlanScreen>.PooledDictionary pooledDictionary = DictionaryPool<string, HashedString, PlanScreen>.Allocate();
		DictionaryPool<string, List<BuildingDef>, PlanScreen>.PooledDictionary pooledDictionary2 = DictionaryPool<string, List<BuildingDef>, PlanScreen>.Allocate();
		if (!pooledDictionary2.ContainsKey("default"))
		{
			pooledDictionary2.Add("default", new List<BuildingDef>());
		}
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && Game.IsCorrectDlcActiveForCurrentSave(buildingDef))
				{
					pooledDictionary.Add(buildingDef.PrefabID, planInfo.category);
					if (!pooledDictionary2.ContainsKey(keyValuePair.Value))
					{
						pooledDictionary2.Add(keyValuePair.Value, new List<BuildingDef>());
					}
					pooledDictionary2[keyValuePair.Value].Add(buildingDef);
				}
			}
		}
		if (this.stableSubcategoryOrder.Count == 0)
		{
			foreach (PlanScreen.PlanInfo ptr in TUNING.BUILDINGS.PLANORDER)
			{
				this.<BuildButtonList>g__RegisterSubcategory|138_0("default");
				foreach (KeyValuePair<string, string> keyValuePair2 in ptr.buildingAndSubcategoryData)
				{
					this.<BuildButtonList>g__RegisterSubcategory|138_0(keyValuePair2.Value);
				}
			}
		}
		GameObject gameObject = this.allSubCategoryObjects["default"].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
		foreach (string text in this.stableSubcategoryOrder)
		{
			List<BuildingDef> list;
			if (pooledDictionary2.TryGetValue(text, out list))
			{
				if (text == "default")
				{
					this.allSubCategoryObjects[text].SetActive(this.useSubCategoryLayout);
				}
				HierarchyReferences component = this.allSubCategoryObjects[text].GetComponent<HierarchyReferences>();
				GameObject parent;
				if (this.useSubCategoryLayout)
				{
					component.GetReference<RectTransform>("Header").gameObject.SetActive(true);
					parent = this.allSubCategoryObjects[text].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
					StringEntry entry;
					if (PlanScreen.TryGetNewSubCategoryName(text, out entry))
					{
						component.GetReference<LocText>("HeaderLabel").SetText(entry);
					}
				}
				else
				{
					component.GetReference<RectTransform>("Header").gameObject.SetActive(false);
					parent = gameObject;
				}
				foreach (BuildingDef buildingDef2 in list)
				{
					HashedString hashedString = pooledDictionary[buildingDef2.PrefabID];
					GameObject gameObject2 = this.CreateButton(buildingDef2, parent, hashedString);
					PlanScreen.ToggleEntry toggleEntry;
					this.GetToggleEntryForCategory(hashedString, out toggleEntry);
					if (toggleEntry != null && toggleEntry.pendingResearchAttentions.Contains(buildingDef2.PrefabID))
					{
						gameObject2.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
					}
				}
			}
		}
		pooledDictionary2.Recycle();
		pooledDictionary.Recycle();
		if (!BuildingGroupScreen.SearchIsEmpty)
		{
			this.RefreshSearch();
		}
		this.ForceRefreshAllBuildingToggles();
		this.RefreshScale(null);
	}

	// Token: 0x06006F6A RID: 28522 RVA: 0x002A4D60 File Offset: 0x002A2F60
	public void ConfigurePanelSize(object data = null)
	{
		if (this.useSubCategoryLayout)
		{
			this.buildGrid_bg_rowHeight = 48f;
		}
		else
		{
			this.buildGrid_bg_rowHeight = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize.y : PlanScreen.standarduildingButtonSize.y);
		}
		GridLayoutGroup reference = this.subgroupPrefab.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid");
		this.buildGrid_bg_rowHeight += reference.spacing.y;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.GroupsTransform.childCount; i++)
		{
			int num3 = 0;
			HierarchyReferences component = this.GroupsTransform.GetChild(i).GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				GridLayoutGroup reference2 = component.GetReference<GridLayoutGroup>("Grid");
				if (!(reference2 == null))
				{
					for (int j = 0; j < reference2.transform.childCount; j++)
					{
						if (reference2.transform.GetChild(j).gameObject.activeSelf)
						{
							num3++;
						}
					}
					if (num3 > 0)
					{
						num2 += 24;
					}
					num += num3 / reference2.constraintCount;
					if (num3 % reference2.constraintCount != 0)
					{
						num++;
					}
				}
			}
		}
		num2 = Math.Min(72, num2);
		this.noResultMessage.SetActive(num == 0);
		int num4 = num;
		int num5 = Math.Max(1, Screen.height / (int)this.buildGrid_bg_rowHeight - 3);
		num5 = Math.Min(num5, this.useSubCategoryLayout ? 12 : 6);
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num4 = Mathf.Min(num5, this.useSubCategoryLayout ? 8 : 4);
		}
		this.BuildingGroupContentsRect.GetComponent<ScrollRect>().verticalScrollbar.gameObject.SetActive(num4 >= num5 - 1);
		float num6 = this.buildGrid_bg_borderHeight + (float)num2 + 36f + (float)Mathf.Clamp(num4, 0, num5) * this.buildGrid_bg_rowHeight;
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num6 = Mathf.Max(num6, this.buildingGroupsRoot.sizeDelta.y);
		}
		this.buildingGroupsRoot.sizeDelta = new Vector2(this.buildGrid_bg_width, num6);
		this.RefreshScale(null);
	}

	// Token: 0x06006F6B RID: 28523 RVA: 0x002A4F88 File Offset: 0x002A3188
	private void SetScrollPoint(float targetY)
	{
		this.BuildingGroupContentsRect.anchoredPosition = new Vector2(this.BuildingGroupContentsRect.anchoredPosition.x, targetY);
	}

	// Token: 0x06006F6C RID: 28524 RVA: 0x002A4FAC File Offset: 0x002A31AC
	private GameObject CreateButton(BuildingDef def, GameObject parent, HashedString plan_category)
	{
		PlanBuildingToggle componentInChildren;
		GameObject gameObject;
		if (this.allBuildingToggles.TryGetValue(def.PrefabID, out componentInChildren))
		{
			gameObject = componentInChildren.gameObject;
			componentInChildren.Refresh(null);
		}
		else
		{
			gameObject = global::Util.KInstantiateUI(this.planButtonPrefab, parent, false);
			gameObject.name = UI.StripLinkFormatting(def.name) + " Group:" + plan_category.ToString();
			componentInChildren = gameObject.GetComponentInChildren<PlanBuildingToggle>();
			componentInChildren.Config(def, this, plan_category);
			componentInChildren.soundPlayer.Enabled = false;
			componentInChildren.SwitchViewMode(this.useSubCategoryLayout);
			this.allBuildingToggles.Add(def.PrefabID, componentInChildren);
		}
		if (gameObject.transform.parent != parent)
		{
			gameObject.transform.SetParent(parent.transform);
		}
		this.activeCategoryBuildingToggles.Add(def, componentInChildren);
		return gameObject;
	}

	// Token: 0x06006F6D RID: 28525 RVA: 0x002A5089 File Offset: 0x002A3289
	public static bool TechRequirementsMet(TechItem techItem)
	{
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x06006F6E RID: 28526 RVA: 0x002A50A9 File Offset: 0x002A32A9
	private static bool TechRequirementsUpcoming(TechItem techItem)
	{
		return PlanScreen.TechRequirementsMet(techItem);
	}

	// Token: 0x06006F6F RID: 28527 RVA: 0x002A50B4 File Offset: 0x002A32B4
	private bool GetToggleEntryForCategory(HashedString category, out PlanScreen.ToggleEntry toggleEntry)
	{
		toggleEntry = null;
		foreach (PlanScreen.ToggleEntry toggleEntry2 in this.toggleEntries)
		{
			if (toggleEntry2.planCategory == category)
			{
				toggleEntry = toggleEntry2;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006F70 RID: 28528 RVA: 0x002A511C File Offset: 0x002A331C
	public bool IsDefBuildable(BuildingDef def)
	{
		return this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete;
	}

	// Token: 0x06006F71 RID: 28529 RVA: 0x002A5128 File Offset: 0x002A3328
	public string GetTooltipForBuildable(BuildingDef def)
	{
		PlanScreen.RequirementsState buildableState = this.GetBuildableState(def);
		return PlanScreen.GetTooltipForRequirementsState(def, buildableState);
	}

	// Token: 0x06006F72 RID: 28530 RVA: 0x002A5144 File Offset: 0x002A3344
	public static string GetTooltipForRequirementsState(BuildingDef def, PlanScreen.RequirementsState state)
	{
		TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		string text = null;
		if (Game.Instance.SandboxModeActive)
		{
			text = UIConstants.ColorPrefixYellow + UI.SANDBOXTOOLS.SETTINGS.INSTANT_BUILD.NAME + UIConstants.ColorSuffix;
		}
		else if (DebugHandler.InstantBuildMode)
		{
			text = UIConstants.ColorPrefixYellow + UI.DEBUG_TOOLS.DEBUG_ACTIVE + UIConstants.ColorSuffix;
		}
		else
		{
			switch (state)
			{
			case PlanScreen.RequirementsState.Tech:
				text = string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, techItem.ParentTech.Name);
				break;
			case PlanScreen.RequirementsState.Materials:
				text = UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
				foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
				{
					string str = string.Format("{0}{1}: {2}", "• ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
					text = text + "\n" + str;
				}
				break;
			case PlanScreen.RequirementsState.TelepadBuilt:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.UniquePerWorld:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.RocketInteriorOnly:
				text = UI.PRODUCTINFO_ROCKET_INTERIOR;
				break;
			case PlanScreen.RequirementsState.RocketInteriorForbidden:
				text = UI.PRODUCTINFO_ROCKET_NOT_INTERIOR;
				break;
			}
		}
		return text;
	}

	// Token: 0x06006F73 RID: 28531 RVA: 0x002A52D0 File Offset: 0x002A34D0
	private void PointerEnter(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = true;
	}

	// Token: 0x06006F74 RID: 28532 RVA: 0x002A52DE File Offset: 0x002A34DE
	private void PointerExit(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = false;
	}

	// Token: 0x06006F75 RID: 28533 RVA: 0x002A52EC File Offset: 0x002A34EC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyDown(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.IsAction(global::Action.CopyBuilding) && e.TryConsume(global::Action.CopyBuilding))
		{
			this.OnClickCopyBuilding();
		}
		if (this.toggles == null)
		{
			return;
		}
		if (!e.Consumed && this.activeCategoryInfo != null && e.TryConsume(global::Action.Escape))
		{
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			this.ClearSelection();
			return;
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06006F76 RID: 28534 RVA: 0x002A53B4 File Offset: 0x002A35B4
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyUp(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.Consumed)
		{
			return;
		}
		if (this.SelectedBuildingGameObject != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.CloseRecipe(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		else if (this.activeCategoryInfo != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.OnUIClear(null);
		}
		if (e.TryConsume(global::Action.Find))
		{
			if (this.activeCategoryInfo == null)
			{
				this.OpenCategoryByName("Base");
			}
			BuildingGroupScreen.Instance.inputField.Select();
			e.Consumed = true;
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06006F77 RID: 28535 RVA: 0x002A54AC File Offset: 0x002A36AC
	private void OnRecipeElementsFullySelected()
	{
		BuildingDef buildingDef = null;
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair in this.allBuildingToggles)
		{
			if (keyValuePair.Value == this.currentlySelectedToggle)
			{
				buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				break;
			}
		}
		DebugUtil.DevAssert(buildingDef, "def is null", null);
		if (buildingDef)
		{
			if (buildingDef.isKAnimTile && buildingDef.isUtility)
			{
				IList<Tag> getSelectedElementAsList = this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
				((buildingDef.BuildingComplete.GetComponent<Wire>() != null) ? WireBuildTool.Instance : UtilityBuildTool.Instance).Activate(buildingDef, getSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
				return;
			}
			BuildTool.Instance.Activate(buildingDef, this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
		}
	}

	// Token: 0x06006F78 RID: 28536 RVA: 0x002A55BC File Offset: 0x002A37BC
	public void OnResearchComplete(object tech)
	{
		if (tech is Tech)
		{
			using (List<TechItem>.Enumerator enumerator = ((Tech)tech).unlockedItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TechItem techItem = enumerator.Current;
					BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
					this.AddResearchedBuildingCategory(buildingDef);
				}
				return;
			}
		}
		if (tech is BuildingDef)
		{
			BuildingDef def = tech as BuildingDef;
			this.AddResearchedBuildingCategory(def);
		}
	}

	// Token: 0x06006F79 RID: 28537 RVA: 0x002A563C File Offset: 0x002A383C
	private void AddResearchedBuildingCategory(BuildingDef def)
	{
		if (def != null && Game.IsCorrectDlcActiveForCurrentSave(def))
		{
			this.UpdateDefResearched(def);
			if (this.tagCategoryMap.ContainsKey(def.Tag))
			{
				HashedString category = this.tagCategoryMap[def.Tag];
				PlanScreen.ToggleEntry toggleEntry;
				if (this.GetToggleEntryForCategory(category, out toggleEntry))
				{
					toggleEntry.pendingResearchAttentions.Add(def.Tag);
					toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
					toggleEntry.Refresh();
				}
			}
		}
	}

	// Token: 0x06006F7A RID: 28538 RVA: 0x002A56C0 File Offset: 0x002A38C0
	private void OnUIClear(object data)
	{
		if (this.activeCategoryInfo != null)
		{
			this.selected = -1;
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x06006F7B RID: 28539 RVA: 0x002A5710 File Offset: 0x002A3910
	private void OnActiveToolChanged(object data)
	{
		if (data == null)
		{
			return;
		}
		if (this.ignoreToolChangeMessages > 0)
		{
			return;
		}
		Type type = data.GetType();
		if (!typeof(BuildTool).IsAssignableFrom(type) && !typeof(PrebuildTool).IsAssignableFrom(type) && !typeof(BaseUtilityBuildTool).IsAssignableFrom(type))
		{
			this.CloseRecipe(false);
			this.CloseCategoryPanel(false);
		}
	}

	// Token: 0x06006F7C RID: 28540 RVA: 0x002A5776 File Offset: 0x002A3976
	public PrioritySetting GetBuildingPriority()
	{
		return this.ProductInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
	}

	// Token: 0x06006F83 RID: 28547 RVA: 0x002A5B40 File Offset: 0x002A3D40
	[CompilerGenerated]
	private SearchUtil.SubcategoryCache <CacheSearchCaches>g__ManifestSubcategoryCache|130_0(string subcategory, string _text = null)
	{
		SearchUtil.SubcategoryCache subcategoryCache;
		if (!this.subcategorySearchCaches.TryGetValue(subcategory, out subcategoryCache))
		{
			subcategoryCache = new SearchUtil.SubcategoryCache
			{
				subcategory = new SearchUtil.MatchCache
				{
					text = SearchUtil.Canonicalize(_text ?? subcategory)
				},
				buildingDefs = new HashSet<SearchUtil.BuildingDefCache>()
			};
			this.subcategorySearchCaches[subcategory] = subcategoryCache;
		}
		return subcategoryCache;
	}

	// Token: 0x06006F84 RID: 28548 RVA: 0x002A5B98 File Offset: 0x002A3D98
	[CompilerGenerated]
	private int <SortSubcategories>g__CompareScores|137_0(global::Tuple<GameObject, string> a, global::Tuple<GameObject, string> b)
	{
		return this.subcategorySearchCaches[a.second].CompareTo(this.subcategorySearchCaches[b.second]);
	}

	// Token: 0x06006F85 RID: 28549 RVA: 0x002A5BC4 File Offset: 0x002A3DC4
	[CompilerGenerated]
	private void <BuildButtonList>g__RegisterSubcategory|138_0(string subcategory)
	{
		if (this.allSubCategoryObjects.ContainsKey(subcategory))
		{
			return;
		}
		GameObject gameObject = global::Util.KInstantiateUI(this.subgroupPrefab, this.GroupsTransform.gameObject, true);
		this.stableSubcategoryOrder.Add(subcategory);
		this.allSubCategoryObjects[subcategory] = gameObject;
		gameObject.SetActive(false);
	}

	// Token: 0x04004C06 RID: 19462
	[SerializeField]
	private GameObject planButtonPrefab;

	// Token: 0x04004C07 RID: 19463
	[SerializeField]
	private GameObject recipeInfoScreenParent;

	// Token: 0x04004C08 RID: 19464
	[SerializeField]
	private GameObject productInfoScreenPrefab;

	// Token: 0x04004C09 RID: 19465
	[SerializeField]
	private GameObject copyBuildingButton;

	// Token: 0x04004C0A RID: 19466
	[SerializeField]
	private KButton gridViewButton;

	// Token: 0x04004C0B RID: 19467
	[SerializeField]
	private KButton listViewButton;

	// Token: 0x04004C0C RID: 19468
	private bool useSubCategoryLayout;

	// Token: 0x04004C0D RID: 19469
	private int refreshScaleHandle = -1;

	// Token: 0x04004C0E RID: 19470
	[SerializeField]
	private GameObject adjacentPinnedButtons;

	// Token: 0x04004C0F RID: 19471
	private static Dictionary<HashedString, string> iconNameMap = new Dictionary<HashedString, string>
	{
		{
			PlanScreen.CacheHashedString("Base"),
			"icon_category_base"
		},
		{
			PlanScreen.CacheHashedString("Oxygen"),
			"icon_category_oxygen"
		},
		{
			PlanScreen.CacheHashedString("Power"),
			"icon_category_electrical"
		},
		{
			PlanScreen.CacheHashedString("Food"),
			"icon_category_food"
		},
		{
			PlanScreen.CacheHashedString("Plumbing"),
			"icon_category_plumbing"
		},
		{
			PlanScreen.CacheHashedString("HVAC"),
			"icon_category_ventilation"
		},
		{
			PlanScreen.CacheHashedString("Refining"),
			"icon_category_refinery"
		},
		{
			PlanScreen.CacheHashedString("Medical"),
			"icon_category_medical"
		},
		{
			PlanScreen.CacheHashedString("Furniture"),
			"icon_category_furniture"
		},
		{
			PlanScreen.CacheHashedString("Equipment"),
			"icon_category_misc"
		},
		{
			PlanScreen.CacheHashedString("Utilities"),
			"icon_category_utilities"
		},
		{
			PlanScreen.CacheHashedString("Automation"),
			"icon_category_automation"
		},
		{
			PlanScreen.CacheHashedString("Conveyance"),
			"icon_category_shipping"
		},
		{
			PlanScreen.CacheHashedString("Rocketry"),
			"icon_category_rocketry"
		},
		{
			PlanScreen.CacheHashedString("HEP"),
			"icon_category_radiation"
		}
	};

	// Token: 0x04004C10 RID: 19472
	private Dictionary<KIconToggleMenu.ToggleInfo, bool> CategoryInteractive = new Dictionary<KIconToggleMenu.ToggleInfo, bool>();

	// Token: 0x04004C12 RID: 19474
	[SerializeField]
	public PlanScreen.BuildingToolTipSettings buildingToolTipSettings;

	// Token: 0x04004C13 RID: 19475
	public PlanScreen.BuildingNameTextSetting buildingNameTextSettings;

	// Token: 0x04004C14 RID: 19476
	private KIconToggleMenu.ToggleInfo activeCategoryInfo;

	// Token: 0x04004C15 RID: 19477
	public Dictionary<BuildingDef, PlanBuildingToggle> activeCategoryBuildingToggles = new Dictionary<BuildingDef, PlanBuildingToggle>();

	// Token: 0x04004C16 RID: 19478
	private float timeSinceNotificationPing;

	// Token: 0x04004C17 RID: 19479
	private float notificationPingExpire = 0.5f;

	// Token: 0x04004C18 RID: 19480
	private float specialNotificationEmbellishDelay = 8f;

	// Token: 0x04004C19 RID: 19481
	private int notificationPingCount;

	// Token: 0x04004C1A RID: 19482
	private Dictionary<KToggle, Bouncer> toggleBouncers = new Dictionary<KToggle, Bouncer>();

	// Token: 0x04004C1B RID: 19483
	public const string DEFAULT_SUBCATEGORY_KEY = "default";

	// Token: 0x04004C1C RID: 19484
	private Dictionary<string, GameObject> allSubCategoryObjects = new Dictionary<string, GameObject>();

	// Token: 0x04004C1D RID: 19485
	private Dictionary<string, PlanBuildingToggle> allBuildingToggles = new Dictionary<string, PlanBuildingToggle>();

	// Token: 0x04004C1E RID: 19486
	private readonly Dictionary<string, SearchUtil.BuildingDefCache> buildingDefSearchCaches = new Dictionary<string, SearchUtil.BuildingDefCache>();

	// Token: 0x04004C1F RID: 19487
	private readonly Dictionary<string, SearchUtil.SubcategoryCache> subcategorySearchCaches = new Dictionary<string, SearchUtil.SubcategoryCache>();

	// Token: 0x04004C20 RID: 19488
	private readonly Dictionary<string, SearchUtil.SubcategoryCache> subcategorySearchCachesByBuildingPrefab = new Dictionary<string, SearchUtil.SubcategoryCache>();

	// Token: 0x04004C21 RID: 19489
	private readonly List<string> stableSubcategoryOrder = new List<string>();

	// Token: 0x04004C22 RID: 19490
	private static Vector2 bigBuildingButtonSize = new Vector2(98f, 123f);

	// Token: 0x04004C23 RID: 19491
	private static Vector2 standarduildingButtonSize = PlanScreen.bigBuildingButtonSize * 0.8f;

	// Token: 0x04004C24 RID: 19492
	public static int fontSizeBigMode = 16;

	// Token: 0x04004C25 RID: 19493
	public static int fontSizeStandardMode = 14;

	// Token: 0x04004C27 RID: 19495
	[SerializeField]
	private GameObject subgroupPrefab;

	// Token: 0x04004C28 RID: 19496
	public Transform GroupsTransform;

	// Token: 0x04004C29 RID: 19497
	public Sprite Overlay_NeedTech;

	// Token: 0x04004C2A RID: 19498
	public RectTransform buildingGroupsRoot;

	// Token: 0x04004C2B RID: 19499
	public RectTransform BuildButtonBGPanel;

	// Token: 0x04004C2C RID: 19500
	public RectTransform BuildingGroupContentsRect;

	// Token: 0x04004C2D RID: 19501
	public Sprite defaultBuildingIconSprite;

	// Token: 0x04004C2E RID: 19502
	private KScrollRect planScreenScrollRect;

	// Token: 0x04004C2F RID: 19503
	public Material defaultUIMaterial;

	// Token: 0x04004C30 RID: 19504
	public Material desaturatedUIMaterial;

	// Token: 0x04004C31 RID: 19505
	public LocText PlanCategoryLabel;

	// Token: 0x04004C32 RID: 19506
	public GameObject noResultMessage;

	// Token: 0x04004C33 RID: 19507
	private int nextCategoryToUpdateIDX = -1;

	// Token: 0x04004C34 RID: 19508
	private bool forceUpdateAllCategoryToggles;

	// Token: 0x04004C35 RID: 19509
	private bool forceRefreshAllBuildings = true;

	// Token: 0x04004C36 RID: 19510
	private List<PlanScreen.ToggleEntry> toggleEntries = new List<PlanScreen.ToggleEntry>();

	// Token: 0x04004C37 RID: 19511
	private int ignoreToolChangeMessages;

	// Token: 0x04004C38 RID: 19512
	private Dictionary<string, PlanScreen.RequirementsState> _buildableStatesByID = new Dictionary<string, PlanScreen.RequirementsState>();

	// Token: 0x04004C39 RID: 19513
	private Dictionary<Def, bool> _researchedDefs = new Dictionary<Def, bool>();

	// Token: 0x04004C3A RID: 19514
	[SerializeField]
	private TextStyleSetting[] CategoryLabelTextStyles;

	// Token: 0x04004C3B RID: 19515
	private float initTime;

	// Token: 0x04004C3C RID: 19516
	private Dictionary<Tag, HashedString> tagCategoryMap;

	// Token: 0x04004C3D RID: 19517
	private Dictionary<Tag, int> tagOrderMap;

	// Token: 0x04004C3E RID: 19518
	private BuildingDef lastSelectedBuildingDef;

	// Token: 0x04004C3F RID: 19519
	private Building lastSelectedBuilding;

	// Token: 0x04004C40 RID: 19520
	private string lastSelectedBuildingFacade = "DEFAULT_FACADE";

	// Token: 0x04004C41 RID: 19521
	private int buildable_state_update_idx;

	// Token: 0x04004C42 RID: 19522
	private int building_button_refresh_idx;

	// Token: 0x04004C43 RID: 19523
	private readonly int maxToggleRefreshPerFrame = 10;

	// Token: 0x04004C44 RID: 19524
	private bool categoryPanelSizeNeedsRefresh;

	// Token: 0x04004C45 RID: 19525
	private Comparer<global::Tuple<PlanBuildingToggle, string>> buildingDefComparer;

	// Token: 0x04004C46 RID: 19526
	private float buildGrid_bg_width = 320f;

	// Token: 0x04004C47 RID: 19527
	private float buildGrid_bg_borderHeight = 48f;

	// Token: 0x04004C48 RID: 19528
	private const float BUILDGRID_SEARCHBAR_HEIGHT = 36f;

	// Token: 0x04004C49 RID: 19529
	private const int SUBCATEGORY_HEADER_HEIGHT = 24;

	// Token: 0x04004C4A RID: 19530
	private float buildGrid_bg_rowHeight;

	// Token: 0x02002044 RID: 8260
	public struct PlanInfo : IHasDlcRestrictions
	{
		// Token: 0x0600B8B7 RID: 47287 RVA: 0x003F67E4 File Offset: 0x003F49E4
		public PlanInfo(HashedString category, bool hideIfNotResearched, List<string> listData, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (string key in listData)
			{
				list.Add(new KeyValuePair<string, string>(key, TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(key) ? TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[key] : "uncategorized"));
			}
			this.category = category;
			this.hideIfNotResearched = hideIfNotResearched;
			this.data = listData;
			this.buildingAndSubcategoryData = list;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
		}

		// Token: 0x0600B8B8 RID: 47288 RVA: 0x003F6888 File Offset: 0x003F4A88
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600B8B9 RID: 47289 RVA: 0x003F6890 File Offset: 0x003F4A90
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x04009573 RID: 38259
		public HashedString category;

		// Token: 0x04009574 RID: 38260
		public bool hideIfNotResearched;

		// Token: 0x04009575 RID: 38261
		[Obsolete("Modders: Use ModUtil.AddBuildingToPlanScreen")]
		public List<string> data;

		// Token: 0x04009576 RID: 38262
		public List<KeyValuePair<string, string>> buildingAndSubcategoryData;

		// Token: 0x04009577 RID: 38263
		private string[] requiredDlcIds;

		// Token: 0x04009578 RID: 38264
		private string[] forbiddenDlcIds;
	}

	// Token: 0x02002045 RID: 8261
	[Serializable]
	public struct BuildingToolTipSettings
	{
		// Token: 0x04009579 RID: 38265
		public TextStyleSetting BuildButtonName;

		// Token: 0x0400957A RID: 38266
		public TextStyleSetting BuildButtonDescription;

		// Token: 0x0400957B RID: 38267
		public TextStyleSetting MaterialRequirement;

		// Token: 0x0400957C RID: 38268
		public TextStyleSetting ResearchRequirement;
	}

	// Token: 0x02002046 RID: 8262
	[Serializable]
	public struct BuildingNameTextSetting
	{
		// Token: 0x0400957D RID: 38269
		public TextStyleSetting ActiveSelected;

		// Token: 0x0400957E RID: 38270
		public TextStyleSetting ActiveDeselected;

		// Token: 0x0400957F RID: 38271
		public TextStyleSetting InactiveSelected;

		// Token: 0x04009580 RID: 38272
		public TextStyleSetting InactiveDeselected;
	}

	// Token: 0x02002047 RID: 8263
	private class ToggleEntry
	{
		// Token: 0x0600B8BA RID: 47290 RVA: 0x003F6898 File Offset: 0x003F4A98
		public ToggleEntry(KIconToggleMenu.ToggleInfo toggle_info, HashedString plan_category, List<BuildingDef> building_defs, bool hideIfNotResearched)
		{
			this.toggleInfo = toggle_info;
			this.planCategory = plan_category;
			building_defs.RemoveAll((BuildingDef def) => !Game.IsCorrectDlcActiveForCurrentSave(def));
			this.buildingDefs = building_defs;
			this.hideIfNotResearched = hideIfNotResearched;
			this.pendingResearchAttentions = new List<Tag>();
			this.requiredTechItems = new List<TechItem>();
			this.toggleImages = null;
			foreach (BuildingDef buildingDef in building_defs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(buildingDef.PrefabID);
				if (techItem == null)
				{
					this.requiredTechItems.Clear();
					break;
				}
				if (!this.requiredTechItems.Contains(techItem))
				{
					this.requiredTechItems.Add(techItem);
				}
			}
			this._areAnyRequiredTechItemsAvailable = false;
			this.Refresh();
		}

		// Token: 0x0600B8BB RID: 47291 RVA: 0x003F6994 File Offset: 0x003F4B94
		public bool AreAnyRequiredTechItemsAvailable()
		{
			return this._areAnyRequiredTechItemsAvailable;
		}

		// Token: 0x0600B8BC RID: 47292 RVA: 0x003F699C File Offset: 0x003F4B9C
		public void Refresh()
		{
			if (this._areAnyRequiredTechItemsAvailable)
			{
				return;
			}
			if (this.requiredTechItems.Count == 0)
			{
				this._areAnyRequiredTechItemsAvailable = true;
				return;
			}
			using (List<TechItem>.Enumerator enumerator = this.requiredTechItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (PlanScreen.TechRequirementsUpcoming(enumerator.Current))
					{
						this._areAnyRequiredTechItemsAvailable = true;
						break;
					}
				}
			}
		}

		// Token: 0x0600B8BD RID: 47293 RVA: 0x003F6A18 File Offset: 0x003F4C18
		public void CollectToggleImages()
		{
			this.toggleImages = this.toggleInfo.toggle.gameObject.GetComponents<ImageToggleState>();
		}

		// Token: 0x04009581 RID: 38273
		public KIconToggleMenu.ToggleInfo toggleInfo;

		// Token: 0x04009582 RID: 38274
		public HashedString planCategory;

		// Token: 0x04009583 RID: 38275
		public List<BuildingDef> buildingDefs;

		// Token: 0x04009584 RID: 38276
		public List<Tag> pendingResearchAttentions;

		// Token: 0x04009585 RID: 38277
		private List<TechItem> requiredTechItems;

		// Token: 0x04009586 RID: 38278
		public ImageToggleState[] toggleImages;

		// Token: 0x04009587 RID: 38279
		public bool hideIfNotResearched;

		// Token: 0x04009588 RID: 38280
		private bool _areAnyRequiredTechItemsAvailable;
	}

	// Token: 0x02002048 RID: 8264
	public enum RequirementsState
	{
		// Token: 0x0400958A RID: 38282
		Invalid,
		// Token: 0x0400958B RID: 38283
		Tech,
		// Token: 0x0400958C RID: 38284
		Materials,
		// Token: 0x0400958D RID: 38285
		Complete,
		// Token: 0x0400958E RID: 38286
		TelepadBuilt,
		// Token: 0x0400958F RID: 38287
		UniquePerWorld,
		// Token: 0x04009590 RID: 38288
		RocketInteriorOnly,
		// Token: 0x04009591 RID: 38289
		RocketInteriorForbidden
	}
}
