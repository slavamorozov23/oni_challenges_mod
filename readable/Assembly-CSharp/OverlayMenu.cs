using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C69 RID: 3177
public class OverlayMenu : KIconToggleMenu
{
	// Token: 0x060060DA RID: 24794 RVA: 0x00239FAB File Offset: 0x002381AB
	public static void DestroyInstance()
	{
		OverlayMenu.Instance = null;
	}

	// Token: 0x060060DB RID: 24795 RVA: 0x00239FB4 File Offset: 0x002381B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		OverlayMenu.Instance = this;
		this.InitializeToggles();
		base.Setup(this.overlayToggleInfos);
		Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
		KInputManager.InputChange.AddListener(new UnityAction(this.Refresh));
		base.onSelect += this.OnToggleSelect;
	}

	// Token: 0x060060DC RID: 24796 RVA: 0x0023A03F File Offset: 0x0023823F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshButtons();
	}

	// Token: 0x060060DD RID: 24797 RVA: 0x0023A04D File Offset: 0x0023824D
	public void Refresh()
	{
		this.RefreshButtons();
	}

	// Token: 0x060060DE RID: 24798 RVA: 0x0023A058 File Offset: 0x00238258
	protected override void RefreshButtons()
	{
		base.RefreshButtons();
		if (Research.Instance == null)
		{
			return;
		}
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.overlayToggleInfos)
		{
			OverlayMenu.OverlayToggleInfo overlayToggleInfo = (OverlayMenu.OverlayToggleInfo)toggleInfo;
			toggleInfo.toggle.gameObject.SetActive(overlayToggleInfo.IsUnlocked());
			toggleInfo.tooltip = GameUtil.ReplaceHotkeyString(overlayToggleInfo.originalToolTipText, toggleInfo.hotKey);
		}
	}

	// Token: 0x060060DF RID: 24799 RVA: 0x0023A0EC File Offset: 0x002382EC
	private void OnResearchComplete(object data)
	{
		this.RefreshButtons();
	}

	// Token: 0x060060E0 RID: 24800 RVA: 0x0023A0F4 File Offset: 0x002382F4
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.Refresh));
		base.OnForcedCleanUp();
	}

	// Token: 0x060060E1 RID: 24801 RVA: 0x0023A112 File Offset: 0x00238312
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(1798162660, new Action<object>(this.OnOverlayChanged));
	}

	// Token: 0x060060E2 RID: 24802 RVA: 0x0023A135 File Offset: 0x00238335
	private void InitializeToggleGroups()
	{
	}

	// Token: 0x060060E3 RID: 24803 RVA: 0x0023A138 File Offset: 0x00238338
	private void InitializeToggles()
	{
		this.overlayToggleInfos = new List<KIconToggleMenu.ToggleInfo>
		{
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.OXYGEN.BUTTON, "overlay_oxygen", OverlayModes.Oxygen.ID, "", global::Action.Overlay1, UI.TOOLTIPS.OXYGENOVERLAYSTRING, UI.OVERLAYS.OXYGEN.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.ELECTRICAL.BUTTON, "overlay_power", OverlayModes.Power.ID, "", global::Action.Overlay2, UI.TOOLTIPS.POWEROVERLAYSTRING, UI.OVERLAYS.ELECTRICAL.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.TEMPERATURE.BUTTON, "overlay_temperature", OverlayModes.Temperature.ID, "", global::Action.Overlay3, UI.TOOLTIPS.TEMPERATUREOVERLAYSTRING, UI.OVERLAYS.TEMPERATURE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.TILEMODE.BUTTON, "overlay_materials", OverlayModes.TileMode.ID, "", global::Action.Overlay4, UI.TOOLTIPS.TILEMODE_OVERLAY_STRING, UI.OVERLAYS.TILEMODE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LIGHTING.BUTTON, "overlay_lights", OverlayModes.Light.ID, "", global::Action.Overlay5, UI.TOOLTIPS.LIGHTSOVERLAYSTRING, UI.OVERLAYS.LIGHTING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LIQUIDPLUMBING.BUTTON, "overlay_liquidvent", OverlayModes.LiquidConduits.ID, "", global::Action.Overlay6, UI.TOOLTIPS.LIQUIDVENTOVERLAYSTRING, UI.OVERLAYS.LIQUIDPLUMBING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.GASPLUMBING.BUTTON, "overlay_gasvent", OverlayModes.GasConduits.ID, "", global::Action.Overlay7, UI.TOOLTIPS.GASVENTOVERLAYSTRING, UI.OVERLAYS.GASPLUMBING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.DECOR.BUTTON, "overlay_decor", OverlayModes.Decor.ID, "", global::Action.Overlay8, UI.TOOLTIPS.DECOROVERLAYSTRING, UI.OVERLAYS.DECOR.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.DISEASE.BUTTON, "overlay_disease", OverlayModes.Disease.ID, "", global::Action.Overlay9, UI.TOOLTIPS.DISEASEOVERLAYSTRING, UI.OVERLAYS.DISEASE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.CROPS.BUTTON, "overlay_farming", OverlayModes.Crop.ID, "", global::Action.Overlay10, UI.TOOLTIPS.CROPS_OVERLAY_STRING, UI.OVERLAYS.CROPS.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.ROOMS.BUTTON, "overlay_rooms", OverlayModes.Rooms.ID, "", global::Action.Overlay11, UI.TOOLTIPS.ROOMSOVERLAYSTRING, UI.OVERLAYS.ROOMS.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.SUIT.BUTTON, "overlay_suit", OverlayModes.Suit.ID, "SuitsOverlay", global::Action.Overlay12, UI.TOOLTIPS.SUITOVERLAYSTRING, UI.OVERLAYS.SUIT.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LOGIC.BUTTON, "overlay_logic", OverlayModes.Logic.ID, "AutomationOverlay", global::Action.Overlay13, UI.TOOLTIPS.LOGICOVERLAYSTRING, UI.OVERLAYS.LOGIC.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.CONVEYOR.BUTTON, "overlay_conveyor", OverlayModes.SolidConveyor.ID, "ConveyorOverlay", global::Action.Overlay14, UI.TOOLTIPS.CONVEYOR_OVERLAY_STRING, UI.OVERLAYS.CONVEYOR.BUTTON)
		};
		if (Sim.IsRadiationEnabled())
		{
			this.overlayToggleInfos.Add(new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.RADIATION.BUTTON, "overlay_radiation", OverlayModes.Radiation.ID, "", global::Action.Overlay15, UI.TOOLTIPS.RADIATIONOVERLAYSTRING, UI.OVERLAYS.RADIATION.BUTTON));
		}
	}

	// Token: 0x060060E4 RID: 24804 RVA: 0x0023A4D8 File Offset: 0x002386D8
	private void OnToggleSelect(KIconToggleMenu.ToggleInfo toggle_info)
	{
		if (SimDebugView.Instance.GetMode() == ((OverlayMenu.OverlayToggleInfo)toggle_info).simView)
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			return;
		}
		if (((OverlayMenu.OverlayToggleInfo)toggle_info).IsUnlocked())
		{
			OverlayScreen.Instance.ToggleOverlay(((OverlayMenu.OverlayToggleInfo)toggle_info).simView, true);
		}
	}

	// Token: 0x060060E5 RID: 24805 RVA: 0x0023A538 File Offset: 0x00238738
	private void OnOverlayChanged(object overlay_data)
	{
		HashedString value = ((Boxed<HashedString>)overlay_data).value;
		for (int i = 0; i < this.overlayToggleInfos.Count; i++)
		{
			this.overlayToggleInfos[i].toggle.isOn = (((OverlayMenu.OverlayToggleInfo)this.overlayToggleInfos[i]).simView == value);
		}
	}

	// Token: 0x060060E6 RID: 24806 RVA: 0x0023A59C File Offset: 0x0023879C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && e.TryConsume(global::Action.Escape))
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x060060E7 RID: 24807 RVA: 0x0023A5F0 File Offset: 0x002387F0
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x040040B2 RID: 16562
	public static OverlayMenu Instance;

	// Token: 0x040040B3 RID: 16563
	private List<KIconToggleMenu.ToggleInfo> overlayToggleInfos;

	// Token: 0x040040B4 RID: 16564
	private UnityAction inputChangeReceiver;

	// Token: 0x02001E22 RID: 7714
	private class OverlayToggleGroup : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x0600B32D RID: 45869 RVA: 0x003E2D86 File Offset: 0x003E0F86
		public OverlayToggleGroup(string text, string icon_name, List<OverlayMenu.OverlayToggleInfo> toggle_group, string required_tech_item = "", global::Action hot_key = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon_name, null, hot_key, tooltip, tooltip_header)
		{
			this.toggleInfoGroup = toggle_group;
		}

		// Token: 0x0600B32E RID: 45870 RVA: 0x003E2D9E File Offset: 0x003E0F9E
		public bool IsUnlocked()
		{
			return DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem);
		}

		// Token: 0x0600B32F RID: 45871 RVA: 0x003E2DCB File Offset: 0x003E0FCB
		public OverlayMenu.OverlayToggleInfo GetActiveToggleInfo()
		{
			return this.toggleInfoGroup[this.activeToggleInfo];
		}

		// Token: 0x04008D7B RID: 36219
		public List<OverlayMenu.OverlayToggleInfo> toggleInfoGroup;

		// Token: 0x04008D7C RID: 36220
		public string requiredTechItem;

		// Token: 0x04008D7D RID: 36221
		[SerializeField]
		private int activeToggleInfo;
	}

	// Token: 0x02001E23 RID: 7715
	private class OverlayToggleInfo : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x0600B330 RID: 45872 RVA: 0x003E2DDE File Offset: 0x003E0FDE
		public OverlayToggleInfo(string text, string icon_name, HashedString sim_view, string required_tech_item = "", global::Action hotKey = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon_name, null, hotKey, tooltip, tooltip_header)
		{
			this.originalToolTipText = tooltip;
			tooltip = GameUtil.ReplaceHotkeyString(tooltip, hotKey);
			this.simView = sim_view;
			this.requiredTechItem = required_tech_item;
		}

		// Token: 0x0600B331 RID: 45873 RVA: 0x003E2E11 File Offset: 0x003E1011
		public bool IsUnlocked()
		{
			return DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem) || Game.Instance.SandboxModeActive;
		}

		// Token: 0x04008D7E RID: 36222
		public HashedString simView;

		// Token: 0x04008D7F RID: 36223
		public string requiredTechItem;

		// Token: 0x04008D80 RID: 36224
		public string originalToolTipText;
	}
}
