using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E84 RID: 3716
public class SummonCrewSideScreen : SideScreenContent, ISim1000ms
{
	// Token: 0x06007644 RID: 30276 RVA: 0x002D1CFA File Offset: 0x002CFEFA
	protected override void OnSpawn()
	{
		this.button.onClick += this.OnButtonPressed;
	}

	// Token: 0x06007645 RID: 30277 RVA: 0x002D1D13 File Offset: 0x002CFF13
	public override int GetSideScreenSortOrder()
	{
		return 101;
	}

	// Token: 0x06007646 RID: 30278 RVA: 0x002D1D18 File Offset: 0x002CFF18
	public override bool IsValidForTarget(GameObject target)
	{
		RocketModuleCluster component = target.GetComponent<RocketModuleCluster>();
		RocketControlStation component2 = target.GetComponent<RocketControlStation>();
		bool flag = component != null && component.GetComponent<PassengerRocketModule>() != null;
		bool flag2 = component != null && component.GetComponent<RoboPilotModule>() != null;
		if (flag || flag2)
		{
			return true;
		}
		if (component2 != null)
		{
			RocketControlStation.StatesInstance smi = component2.GetSMI<RocketControlStation.StatesInstance>();
			return !smi.sm.IsInFlight(smi) && !smi.sm.IsLaunching(smi);
		}
		return false;
	}

	// Token: 0x06007647 RID: 30279 RVA: 0x002D1D9C File Offset: 0x002CFF9C
	public override void SetTarget(GameObject target)
	{
		RocketModuleCluster component = target.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			this.craftModuleInterface = component.CraftInterface;
		}
		else if (target.GetComponent<RocketControlStation>() != null)
		{
			this.craftModuleInterface = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface;
		}
		this.craftModuleInterface.Unsubscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
		this.craftModuleInterface.Subscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
		Game.Instance.Unsubscribe(586301400, new Action<object>(this.OnMinionsChangedWorld));
		Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		Game.Instance.Subscribe(586301400, new Action<object>(this.OnMinionsChangedWorld));
		Game.Instance.Subscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		this.Refresh();
	}

	// Token: 0x06007648 RID: 30280 RVA: 0x002D1E98 File Offset: 0x002D0098
	private void OnMinionsChangedWorld(object o)
	{
		this.Refresh();
	}

	// Token: 0x06007649 RID: 30281 RVA: 0x002D1EA0 File Offset: 0x002D00A0
	public override void ClearTarget()
	{
		this.refreshInUpdate = false;
		if (this.craftModuleInterface != null)
		{
			this.craftModuleInterface.Unsubscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
		}
		base.ClearTarget();
		Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		Game.Instance.Unsubscribe(586301400, new Action<object>(this.OnMinionsChangedWorld));
		this.craftModuleInterface = null;
	}

	// Token: 0x0600764A RID: 30282 RVA: 0x002D1F21 File Offset: 0x002D0121
	private void OnRocketModuleCountChanged(object o)
	{
		this.Refresh();
	}

	// Token: 0x0600764B RID: 30283 RVA: 0x002D1F29 File Offset: 0x002D0129
	private void OnAssignmentGroupChanged(object o)
	{
		this.Refresh();
	}

	// Token: 0x0600764C RID: 30284 RVA: 0x002D1F31 File Offset: 0x002D0131
	private void OnButtonPressed()
	{
		this.ToggleCrewRequestState();
		this.Refresh();
	}

	// Token: 0x0600764D RID: 30285 RVA: 0x002D1F40 File Offset: 0x002D0140
	private void ToggleCrewRequestState()
	{
		PassengerRocketModule passengerModule = this.craftModuleInterface.GetPassengerModule();
		if (passengerModule != null)
		{
			if (passengerModule.PassengersRequested == PassengerRocketModule.RequestCrewState.Request)
			{
				passengerModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
				return;
			}
			passengerModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
		}
	}

	// Token: 0x0600764E RID: 30286 RVA: 0x002D1F7A File Offset: 0x002D017A
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
	}

	// Token: 0x0600764F RID: 30287 RVA: 0x002D1F84 File Offset: 0x002D0184
	private void Refresh()
	{
		this.refreshInUpdate = false;
		PassengerRocketModule passengerModule = this.craftModuleInterface.GetPassengerModule();
		UnityEngine.Object robotPilotModule = this.craftModuleInterface.GetRobotPilotModule();
		int num = (passengerModule == null) ? 0 : passengerModule.GetCrewCount();
		bool flag = passengerModule != null;
		bool flag2 = num > 0;
		bool flag3 = robotPilotModule != null;
		global::Tuple<int, int> tuple = null;
		this.button.isInteractable = (passengerModule != null && flag2);
		SummonCrewSideScreen.CurrentState currentState;
		if (!flag || !flag2)
		{
			currentState = SummonCrewSideScreen.CurrentState.NoCrewFound;
			if (flag3)
			{
				currentState = SummonCrewSideScreen.CurrentState.NoCrewNeeded;
			}
		}
		else if (passengerModule.PassengersRequested == PassengerRocketModule.RequestCrewState.Release)
		{
			currentState = SummonCrewSideScreen.CurrentState.PublicAccess;
		}
		else
		{
			tuple = passengerModule.GetCrewBoardedFraction();
			if (tuple.first < tuple.second)
			{
				currentState = SummonCrewSideScreen.CurrentState.AwaitingCrew;
			}
			else
			{
				currentState = SummonCrewSideScreen.CurrentState.Ready;
			}
		}
		Sprite sprite = null;
		Color color = this.defaultColor;
		string text = "";
		string simpleTooltip = "";
		string text2 = "";
		string simpleTooltip2 = "";
		switch (currentState)
		{
		case SummonCrewSideScreen.CurrentState.NoCrewFound:
			sprite = Assets.GetSprite("rocket_red_icon");
			color = this.noCrewColor;
			text = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_NO_CREW_FOUND;
			simpleTooltip = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_NO_CREW_FOUND;
			text2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_LABEL;
			simpleTooltip2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_TOOLTIP;
			break;
		case SummonCrewSideScreen.CurrentState.NoCrewNeeded:
			sprite = Assets.GetSprite("ic_checklist");
			color = this.readyColor;
			text = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_NO_CREW_NEEDED;
			simpleTooltip = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_NO_CREW_NEEDED;
			text2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_LABEL;
			simpleTooltip2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_TOOLTIP;
			break;
		case SummonCrewSideScreen.CurrentState.PublicAccess:
			sprite = Assets.GetSprite("status_item_change_door_control_state");
			text = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_PUBLIC_ACCESS;
			simpleTooltip = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_PUBLIC_ACCESS;
			text2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_LABEL;
			simpleTooltip2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_TOOLTIP;
			break;
		case SummonCrewSideScreen.CurrentState.AwaitingCrew:
			this.refreshInUpdate = true;
			sprite = Assets.GetSprite("crew_boarded");
			text = GameUtil.SafeStringFormat(UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_AWAITING_CREW, new object[]
			{
				GameUtil.GetFormattedInt((float)tuple.first, GameUtil.TimeSlice.None),
				GameUtil.GetFormattedInt((float)tuple.second, GameUtil.TimeSlice.None)
			});
			simpleTooltip = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_AWAITING_CREW;
			text2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.CANCEL_BUTTON_LABEL;
			simpleTooltip2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.CANCEL_BUTTON_TOOLTIP;
			break;
		case SummonCrewSideScreen.CurrentState.Ready:
			sprite = Assets.GetSprite("ic_checklist");
			color = this.readyColor;
			text = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_CREW_READY;
			simpleTooltip = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_CREW_READY;
			text2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.CANCEL_BUTTON_LABEL;
			simpleTooltip2 = UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.CANCEL_BUTTON_TOOLTIP;
			break;
		}
		this.infoLabel.SetText(text);
		this.infoLabelTooltip.SetSimpleTooltip(simpleTooltip);
		this.buttonLabel.SetText(text2);
		this.buttonTooltip.SetSimpleTooltip(simpleTooltip2);
		this.image.sprite = sprite;
		this.image.color = color;
	}

	// Token: 0x06007650 RID: 30288 RVA: 0x002D2263 File Offset: 0x002D0463
	public void Sim1000ms(float dt)
	{
		if (this.refreshInUpdate)
		{
			this.Refresh();
		}
	}

	// Token: 0x040051CE RID: 20942
	public const string READY_ICON_NAME = "ic_checklist";

	// Token: 0x040051CF RID: 20943
	public const string NOT_APPLICABLE_ICON_NAME = "rocket_red_icon";

	// Token: 0x040051D0 RID: 20944
	public const string PUBLIC_ACCESS_ICON_NAME = "status_item_change_door_control_state";

	// Token: 0x040051D1 RID: 20945
	public const string AWAITING_ICON_NAME = "crew_boarded";

	// Token: 0x040051D2 RID: 20946
	public Image image;

	// Token: 0x040051D3 RID: 20947
	public LocText infoLabel;

	// Token: 0x040051D4 RID: 20948
	public ToolTip infoLabelTooltip;

	// Token: 0x040051D5 RID: 20949
	public KButton button;

	// Token: 0x040051D6 RID: 20950
	public LocText buttonLabel;

	// Token: 0x040051D7 RID: 20951
	public ToolTip buttonTooltip;

	// Token: 0x040051D8 RID: 20952
	private CraftModuleInterface craftModuleInterface;

	// Token: 0x040051D9 RID: 20953
	private Color noCrewColor = Color.white;

	// Token: 0x040051DA RID: 20954
	private Color defaultColor = new Color(0.5568628f, 0.5568628f, 0.5568628f, 1f);

	// Token: 0x040051DB RID: 20955
	private Color readyColor = new Color(0f, 0.58431375f, 0.23137255f, 1f);

	// Token: 0x040051DC RID: 20956
	private bool refreshInUpdate;

	// Token: 0x020020EB RID: 8427
	private enum CurrentState
	{
		// Token: 0x04009784 RID: 38788
		NoCrewFound,
		// Token: 0x04009785 RID: 38789
		NoCrewNeeded,
		// Token: 0x04009786 RID: 38790
		PublicAccess,
		// Token: 0x04009787 RID: 38791
		AwaitingCrew,
		// Token: 0x04009788 RID: 38792
		Ready
	}
}
