using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E47 RID: 3655
public class LaunchButtonSideScreen : SideScreenContent
{
	// Token: 0x060073D7 RID: 29655 RVA: 0x002C3863 File Offset: 0x002C1A63
	protected override void OnSpawn()
	{
		this.Refresh();
		this.launchButton.onClick += this.TriggerLaunch;
	}

	// Token: 0x060073D8 RID: 29656 RVA: 0x002C3882 File Offset: 0x002C1A82
	public override int GetSideScreenSortOrder()
	{
		return -1000;
	}

	// Token: 0x060073D9 RID: 29657 RVA: 0x002C3889 File Offset: 0x002C1A89
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<RocketModuleCluster>() != null || (target.GetComponent<LaunchPad>() && target.GetComponent<LaunchPad>().HasRocketWithCommandModule());
	}

	// Token: 0x060073DA RID: 29658 RVA: 0x002C38B8 File Offset: 0x002C1AB8
	public override void SetTarget(GameObject target)
	{
		bool flag = this.rocketModule == null || this.rocketModule.gameObject != target;
		this.selectedPad = null;
		this.rocketModule = target.GetComponent<RocketModuleCluster>();
		if (this.rocketModule == null)
		{
			this.selectedPad = target.GetComponent<LaunchPad>();
			if (this.selectedPad != null)
			{
				foreach (Ref<RocketModuleCluster> @ref in this.selectedPad.LandedRocket.CraftInterface.ClusterModules)
				{
					if (@ref.Get().GetComponent<LaunchableRocketCluster>())
					{
						this.rocketModule = @ref.Get().GetComponent<RocketModuleCluster>();
						break;
					}
				}
			}
		}
		if (this.selectedPad == null)
		{
			CraftModuleInterface craftInterface = this.rocketModule.CraftInterface;
			this.selectedPad = craftInterface.CurrentPad;
		}
		if (flag)
		{
			this.acknowledgeWarnings = false;
		}
		this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate);
		this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate);
		this.Refresh();
	}

	// Token: 0x060073DB RID: 29659 RVA: 0x002C39FC File Offset: 0x002C1BFC
	public override void ClearTarget()
	{
		if (this.rocketModule != null)
		{
			this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate, false);
			this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate, false);
			this.rocketModule = null;
		}
	}

	// Token: 0x060073DC RID: 29660 RVA: 0x002C3A54 File Offset: 0x002C1C54
	private void TriggerLaunch()
	{
		bool flag = !this.acknowledgeWarnings && this.rocketModule.CraftInterface.HasLaunchWarnings();
		bool flag2 = this.rocketModule.CraftInterface.IsLaunchRequested();
		if (flag)
		{
			this.acknowledgeWarnings = true;
		}
		else if (flag2)
		{
			this.rocketModule.CraftInterface.CancelLaunch();
			this.acknowledgeWarnings = false;
		}
		else
		{
			this.rocketModule.CraftInterface.TriggerLaunch(false);
		}
		this.Refresh();
	}

	// Token: 0x060073DD RID: 29661 RVA: 0x002C3ACB File Offset: 0x002C1CCB
	public void Update()
	{
		if (Time.unscaledTime > this.lastRefreshTime + 1f)
		{
			this.lastRefreshTime = Time.unscaledTime;
			this.Refresh();
		}
	}

	// Token: 0x060073DE RID: 29662 RVA: 0x002C3AF4 File Offset: 0x002C1CF4
	private void Refresh()
	{
		if (this.rocketModule == null || this.selectedPad == null)
		{
			return;
		}
		bool flag = !this.acknowledgeWarnings && this.rocketModule.CraftInterface.HasLaunchWarnings();
		bool flag2 = this.rocketModule.CraftInterface.IsLaunchRequested();
		bool flag3 = this.selectedPad.IsLogicInputConnected();
		bool flag4 = flag3 ? this.rocketModule.CraftInterface.CheckReadyForAutomatedLaunchCommand() : this.rocketModule.CraftInterface.CheckPreppedForLaunch();
		bool flag5 = this.rocketModule.CraftInterface.HasTag(GameTags.RocketNotOnGround);
		if (flag3)
		{
			this.launchButton.isInteractable = false;
			this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED;
			this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP;
		}
		else if (DebugHandler.InstantBuildMode || flag4)
		{
			this.launchButton.isInteractable = true;
			if (flag2)
			{
				this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON_TOOLTIP;
			}
			else if (flag)
			{
				this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON_TOOLTIP;
			}
			else
			{
				LocString loc_string = DebugHandler.InstantBuildMode ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_DEBUG : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
				this.launchButton.GetComponentInChildren<LocText>().text = loc_string;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_TOOLTIP;
			}
		}
		else
		{
			this.launchButton.isInteractable = false;
			this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
			this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_NOT_READY_TOOLTIP;
		}
		UnityEngine.Object interiorWorld = this.rocketModule.CraftInterface.GetInteriorWorld();
		RoboPilotModule robotPilotModule = this.rocketModule.CraftInterface.GetRobotPilotModule();
		PassengerRocketModule passengerModule = this.rocketModule.CraftInterface.GetPassengerModule();
		if (!(interiorWorld != null))
		{
			if (robotPilotModule != null)
			{
				if (!flag4)
				{
					this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
					return;
				}
				if (!flag2)
				{
					this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.READY_FOR_LAUNCH;
					return;
				}
				if (!flag5)
				{
					this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.COUNTING_DOWN;
					return;
				}
			}
			else
			{
				this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			}
			return;
		}
		List<RocketControlStation> worldItems = Components.RocketControlStations.GetWorldItems(this.rocketModule.CraftInterface.GetInteriorWorld().id, false);
		RocketControlStationLaunchWorkable rocketControlStationLaunchWorkable = null;
		if (worldItems != null && worldItems.Count > 0)
		{
			rocketControlStationLaunchWorkable = worldItems[0].GetComponent<RocketControlStationLaunchWorkable>();
		}
		if (passengerModule == null || rocketControlStationLaunchWorkable == null || robotPilotModule == null)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			return;
		}
		bool flag6 = passengerModule.CheckPassengersBoarded(robotPilotModule == null);
		if (!flag6 && robotPilotModule != null)
		{
			flag6 |= !passengerModule.HasCrewAssigned();
		}
		bool flag7 = !passengerModule.CheckExtraPassengers();
		bool flag8 = robotPilotModule != null || rocketControlStationLaunchWorkable.worker != null;
		if (!flag4)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			return;
		}
		if (!flag2)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.READY_FOR_LAUNCH;
			return;
		}
		if (!flag6)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.LOADING_CREW;
			return;
		}
		if (!flag7)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.UNLOADING_PASSENGERS;
			return;
		}
		if (!flag8)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.WAITING_FOR_PILOT;
			return;
		}
		if (!flag5)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.COUNTING_DOWN;
			return;
		}
		this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.TAKING_OFF;
	}

	// Token: 0x04005018 RID: 20504
	public KButton launchButton;

	// Token: 0x04005019 RID: 20505
	public LocText statusText;

	// Token: 0x0400501A RID: 20506
	private RocketModuleCluster rocketModule;

	// Token: 0x0400501B RID: 20507
	private LaunchPad selectedPad;

	// Token: 0x0400501C RID: 20508
	private bool acknowledgeWarnings;

	// Token: 0x0400501D RID: 20509
	private float lastRefreshTime;

	// Token: 0x0400501E RID: 20510
	private const float UPDATE_FREQUENCY = 1f;

	// Token: 0x0400501F RID: 20511
	public const int ROCKET_SIDESCREENS_ORDER_ROOT = 100;

	// Token: 0x04005020 RID: 20512
	private static readonly EventSystem.IntraObjectHandler<LaunchButtonSideScreen> RefreshDelegate = new EventSystem.IntraObjectHandler<LaunchButtonSideScreen>(delegate(LaunchButtonSideScreen cmp, object data)
	{
		cmp.Refresh();
	});
}
