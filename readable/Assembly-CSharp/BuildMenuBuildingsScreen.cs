using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C48 RID: 3144
public class BuildMenuBuildingsScreen : KIconToggleMenu
{
	// Token: 0x06005F25 RID: 24357 RVA: 0x0022C801 File Offset: 0x0022AA01
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x06005F26 RID: 24358 RVA: 0x0022C808 File Offset: 0x0022AA08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateBuildableStates();
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
		base.onSelect += this.OnClickBuilding;
		Game.Instance.Subscribe(-1190690038, new Action<object>(this.OnBuildToolDeactivated));
	}

	// Token: 0x06005F27 RID: 24359 RVA: 0x0022C86C File Offset: 0x0022AA6C
	public void Configure(HashedString category, IList<BuildMenu.BuildingInfo> building_infos)
	{
		this.ClearButtons();
		this.SetHasFocus(true);
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		string text = HashCache.Get().Get(category).ToUpper();
		text = text.Replace(" ", "");
		this.titleLabel.text = Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".BUILDMENUTITLE");
		foreach (BuildMenu.BuildingInfo buildingInfo in building_infos)
		{
			BuildingDef def = Assets.GetBuildingDef(buildingInfo.id);
			if (def.ShouldShowInBuildMenu() && def.IsAvailable())
			{
				KIconToggleMenu.ToggleInfo item = new KIconToggleMenu.ToggleInfo(def.Name, new BuildMenuBuildingsScreen.UserData(def, PlanScreen.RequirementsState.Tech), def.HotKey, () => def.GetUISprite("ui", false));
				list.Add(item);
			}
		}
		base.Setup(list);
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			this.RefreshToggle(this.toggleInfo[i]);
		}
		int num = 0;
		using (IEnumerator enumerator2 = this.gridSizer.transform.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (((Transform)enumerator2.Current).gameObject.activeSelf)
				{
					num++;
				}
			}
		}
		this.gridSizer.constraintCount = Mathf.Min(num, 3);
		int num2 = Mathf.Min(num, this.gridSizer.constraintCount);
		int num3 = (num + this.gridSizer.constraintCount - 1) / this.gridSizer.constraintCount;
		int num4 = num2 - 1;
		int num5 = num3 - 1;
		Vector2 vector = new Vector2((float)num2 * this.gridSizer.cellSize.x + (float)num4 * this.gridSizer.spacing.x + (float)this.gridSizer.padding.left + (float)this.gridSizer.padding.right, (float)num3 * this.gridSizer.cellSize.y + (float)num5 * this.gridSizer.spacing.y + (float)this.gridSizer.padding.top + (float)this.gridSizer.padding.bottom);
		this.contentSizeLayout.minWidth = vector.x;
		this.contentSizeLayout.minHeight = vector.y;
	}

	// Token: 0x06005F28 RID: 24360 RVA: 0x0022CB2C File Offset: 0x0022AD2C
	private void ConfigureToolTip(ToolTip tooltip, BuildingDef def)
	{
		tooltip.ClearMultiStringTooltip();
		tooltip.AddMultiStringTooltip(def.Name, this.buildingToolTipSettings.BuildButtonName);
		tooltip.AddMultiStringTooltip(def.Effect, this.buildingToolTipSettings.BuildButtonDescription);
	}

	// Token: 0x06005F29 RID: 24361 RVA: 0x0022CB64 File Offset: 0x0022AD64
	public void CloseRecipe(bool playSound = false)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		ToolMenu.Instance.ClearSelection();
		this.DeactivateBuildTools();
		if (PlayerController.Instance.ActiveTool == PrebuildTool.Instance)
		{
			SelectTool.Instance.Activate();
		}
		this.selectedBuilding = null;
		this.onBuildingSelected(this.selectedBuilding);
	}

	// Token: 0x06005F2A RID: 24362 RVA: 0x0022CBCC File Offset: 0x0022ADCC
	private void RefreshToggle(KIconToggleMenu.ToggleInfo info)
	{
		if (info == null || info.toggle == null)
		{
			return;
		}
		BuildingDef def = (info.userData as BuildMenuBuildingsScreen.UserData).def;
		TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		bool flag = DebugHandler.InstantBuildMode || techItem == null || techItem.IsComplete();
		bool flag2 = flag || techItem == null || techItem.ParentTech.ArePrerequisitesComplete();
		KToggle toggle = info.toggle;
		if (toggle.gameObject.activeSelf != flag2)
		{
			toggle.gameObject.SetActive(flag2);
		}
		if (toggle.bgImage == null)
		{
			return;
		}
		Image image = toggle.bgImage.GetComponentsInChildren<Image>()[1];
		Sprite uisprite = def.GetUISprite("ui", false);
		image.sprite = uisprite;
		image.SetNativeSize();
		image.rectTransform().sizeDelta /= 4f;
		ToolTip component = toggle.gameObject.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		string text = def.Name;
		string effect = def.Effect;
		if (def.HotKey != global::Action.NumActions)
		{
			text += GameUtil.GetHotkeyString(def.HotKey);
		}
		component.AddMultiStringTooltip(text, this.buildingToolTipSettings.BuildButtonName);
		component.AddMultiStringTooltip(effect, this.buildingToolTipSettings.BuildButtonDescription);
		LocText componentInChildren = toggle.GetComponentInChildren<LocText>();
		if (componentInChildren != null)
		{
			componentInChildren.text = def.Name;
		}
		PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
		int num = (requirementsState == PlanScreen.RequirementsState.Complete) ? 1 : 0;
		ImageToggleState.State state;
		if (def == this.selectedBuilding && (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode))
		{
			state = ImageToggleState.State.Active;
		}
		else
		{
			state = ((requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode) ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled);
		}
		if (def == this.selectedBuilding && state == ImageToggleState.State.Disabled)
		{
			state = ImageToggleState.State.DisabledActive;
		}
		else if (state == ImageToggleState.State.Disabled)
		{
			state = ImageToggleState.State.Disabled;
		}
		toggle.GetComponent<ImageToggleState>().SetState(state);
		Material material;
		Color color;
		if (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode)
		{
			material = this.defaultUIMaterial;
			color = Color.white;
		}
		else
		{
			material = this.desaturatedUIMaterial;
			Color color3;
			if (!flag)
			{
				Graphic graphic = image;
				Color color2 = new Color(1f, 1f, 1f, 0.15f);
				graphic.color = color2;
				color3 = color2;
			}
			else
			{
				color3 = new Color(1f, 1f, 1f, 0.6f);
			}
			color = color3;
		}
		if (image.material != material)
		{
			image.material = material;
			image.color = color;
		}
		Image fgImage = toggle.gameObject.GetComponent<KToggle>().fgImage;
		fgImage.gameObject.SetActive(false);
		if (!flag)
		{
			fgImage.sprite = this.Overlay_NeedTech;
			fgImage.gameObject.SetActive(true);
			string newString = string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, techItem.ParentTech.Name);
			component.AddMultiStringTooltip("\n", this.buildingToolTipSettings.ResearchRequirement);
			component.AddMultiStringTooltip(newString, this.buildingToolTipSettings.ResearchRequirement);
			return;
		}
		if (requirementsState != PlanScreen.RequirementsState.Complete)
		{
			fgImage.gameObject.SetActive(false);
			component.AddMultiStringTooltip("\n", this.buildingToolTipSettings.ResearchRequirement);
			string newString2 = UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
			component.AddMultiStringTooltip(newString2, this.buildingToolTipSettings.ResearchRequirement);
			foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
			{
				string newString3 = string.Format("{0}{1}: {2}", "• ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				component.AddMultiStringTooltip(newString3, this.buildingToolTipSettings.ResearchRequirement);
			}
			component.AddMultiStringTooltip("", this.buildingToolTipSettings.ResearchRequirement);
		}
	}

	// Token: 0x06005F2B RID: 24363 RVA: 0x0022CFBC File Offset: 0x0022B1BC
	public void ClearUI()
	{
		this.Show(false);
		this.ClearButtons();
	}

	// Token: 0x06005F2C RID: 24364 RVA: 0x0022CFCC File Offset: 0x0022B1CC
	private void ClearButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.gameObject.SetActive(false);
			ktoggle.gameObject.transform.SetParent(null);
			UnityEngine.Object.DestroyImmediate(ktoggle.gameObject);
		}
		if (this.toggles != null)
		{
			this.toggles.Clear();
		}
		if (this.toggleInfo != null)
		{
			this.toggleInfo.Clear();
		}
	}

	// Token: 0x06005F2D RID: 24365 RVA: 0x0022D064 File Offset: 0x0022B264
	private void OnClickBuilding(KIconToggleMenu.ToggleInfo toggle_info)
	{
		BuildMenuBuildingsScreen.UserData userData = toggle_info.userData as BuildMenuBuildingsScreen.UserData;
		this.OnSelectBuilding(userData.def);
	}

	// Token: 0x06005F2E RID: 24366 RVA: 0x0022D08C File Offset: 0x0022B28C
	private void OnSelectBuilding(BuildingDef def)
	{
		PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
		if (requirementsState - PlanScreen.RequirementsState.Materials <= 1)
		{
			if (def != this.selectedBuilding)
			{
				this.selectedBuilding = def;
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
			}
			else
			{
				this.selectedBuilding = null;
				this.ClearSelection();
				this.CloseRecipe(true);
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
			}
		}
		else
		{
			this.selectedBuilding = null;
			this.ClearSelection();
			this.CloseRecipe(true);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		this.onBuildingSelected(this.selectedBuilding);
	}

	// Token: 0x06005F2F RID: 24367 RVA: 0x0022D130 File Offset: 0x0022B330
	public void UpdateBuildableStates()
	{
		if (this.toggleInfo == null || this.toggleInfo.Count <= 0)
		{
			return;
		}
		BuildingDef buildingDef = null;
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
		{
			this.RefreshToggle(toggleInfo);
			BuildMenuBuildingsScreen.UserData userData = toggleInfo.userData as BuildMenuBuildingsScreen.UserData;
			BuildingDef def = userData.def;
			if (def.IsAvailable())
			{
				PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
				if (requirementsState != userData.requirementsState)
				{
					if (def == BuildMenu.Instance.SelectedBuildingDef)
					{
						buildingDef = def;
					}
					this.RefreshToggle(toggleInfo);
					userData.requirementsState = requirementsState;
				}
			}
		}
		if (buildingDef != null)
		{
			BuildMenu.Instance.RefreshProductInfoScreen(buildingDef);
		}
	}

	// Token: 0x06005F30 RID: 24368 RVA: 0x0022D204 File Offset: 0x0022B404
	private void OnResearchComplete(object data)
	{
		this.UpdateBuildableStates();
	}

	// Token: 0x06005F31 RID: 24369 RVA: 0x0022D20C File Offset: 0x0022B40C
	private void DeactivateBuildTools()
	{
		InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
		if (activeTool != null)
		{
			Type type = activeTool.GetType();
			if (type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type) || typeof(PrebuildTool).IsAssignableFrom(type))
			{
				activeTool.DeactivateTool(null);
			}
		}
	}

	// Token: 0x06005F32 RID: 24370 RVA: 0x0022D274 File Offset: 0x0022B474
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.mouseOver && base.ConsumeMouseScroll && !e.TryConsume(global::Action.ZoomIn))
		{
			e.TryConsume(global::Action.ZoomOut);
		}
		if (!this.HasFocus)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			Game.Instance.Trigger(288942073, null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			return;
		}
		base.OnKeyDown(e);
		if (!e.Consumed)
		{
			global::Action action = e.GetAction();
			if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
			{
				e.TryConsume(action);
			}
		}
	}

	// Token: 0x06005F33 RID: 24371 RVA: 0x0022D2F8 File Offset: 0x0022B4F8
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!this.HasFocus)
		{
			return;
		}
		if (this.selectedBuilding != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			Game.Instance.Trigger(288942073, null);
			return;
		}
		base.OnKeyUp(e);
		if (!e.Consumed)
		{
			global::Action action = e.GetAction();
			if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
			{
				e.TryConsume(action);
			}
		}
	}

	// Token: 0x06005F34 RID: 24372 RVA: 0x0022D370 File Offset: 0x0022B570
	public override void Close()
	{
		ToolMenu.Instance.ClearSelection();
		this.DeactivateBuildTools();
		if (PlayerController.Instance.ActiveTool == PrebuildTool.Instance)
		{
			SelectTool.Instance.Activate();
		}
		this.selectedBuilding = null;
		this.ClearButtons();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005F35 RID: 24373 RVA: 0x0022D3C6 File Offset: 0x0022B5C6
	public override void SetHasFocus(bool has_focus)
	{
		base.SetHasFocus(has_focus);
		if (this.focusIndicator != null)
		{
			this.focusIndicator.color = (has_focus ? this.focusedColour : this.unfocusedColour);
		}
	}

	// Token: 0x06005F36 RID: 24374 RVA: 0x0022D3FE File Offset: 0x0022B5FE
	private void OnBuildToolDeactivated(object data)
	{
		this.CloseRecipe(false);
	}

	// Token: 0x04003F7C RID: 16252
	[SerializeField]
	private Image focusIndicator;

	// Token: 0x04003F7D RID: 16253
	[SerializeField]
	private Color32 focusedColour;

	// Token: 0x04003F7E RID: 16254
	[SerializeField]
	private Color32 unfocusedColour;

	// Token: 0x04003F7F RID: 16255
	public Action<BuildingDef> onBuildingSelected;

	// Token: 0x04003F80 RID: 16256
	[SerializeField]
	private LocText titleLabel;

	// Token: 0x04003F81 RID: 16257
	[SerializeField]
	private BuildMenuBuildingsScreen.BuildingToolTipSettings buildingToolTipSettings;

	// Token: 0x04003F82 RID: 16258
	[SerializeField]
	private LayoutElement contentSizeLayout;

	// Token: 0x04003F83 RID: 16259
	[SerializeField]
	private GridLayoutGroup gridSizer;

	// Token: 0x04003F84 RID: 16260
	[SerializeField]
	private Sprite Overlay_NeedTech;

	// Token: 0x04003F85 RID: 16261
	[SerializeField]
	private Material defaultUIMaterial;

	// Token: 0x04003F86 RID: 16262
	[SerializeField]
	private Material desaturatedUIMaterial;

	// Token: 0x04003F87 RID: 16263
	private BuildingDef selectedBuilding;

	// Token: 0x02001DEA RID: 7658
	[Serializable]
	public struct BuildingToolTipSettings
	{
		// Token: 0x04008CA4 RID: 36004
		public TextStyleSetting BuildButtonName;

		// Token: 0x04008CA5 RID: 36005
		public TextStyleSetting BuildButtonDescription;

		// Token: 0x04008CA6 RID: 36006
		public TextStyleSetting MaterialRequirement;

		// Token: 0x04008CA7 RID: 36007
		public TextStyleSetting ResearchRequirement;
	}

	// Token: 0x02001DEB RID: 7659
	[Serializable]
	public struct BuildingNameTextSetting
	{
		// Token: 0x04008CA8 RID: 36008
		public TextStyleSetting ActiveSelected;

		// Token: 0x04008CA9 RID: 36009
		public TextStyleSetting ActiveDeselected;

		// Token: 0x04008CAA RID: 36010
		public TextStyleSetting InactiveSelected;

		// Token: 0x04008CAB RID: 36011
		public TextStyleSetting InactiveDeselected;
	}

	// Token: 0x02001DEC RID: 7660
	private class UserData
	{
		// Token: 0x0600B28D RID: 45709 RVA: 0x003E0739 File Offset: 0x003DE939
		public UserData(BuildingDef def, PlanScreen.RequirementsState state)
		{
			this.def = def;
			this.requirementsState = state;
		}

		// Token: 0x04008CAC RID: 36012
		public BuildingDef def;

		// Token: 0x04008CAD RID: 36013
		public PlanScreen.RequirementsState requirementsState;
	}
}
