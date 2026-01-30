using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E15 RID: 3605
public class AssignPilotAndCrewSideScreen : SideScreenContent
{
	// Token: 0x0600723D RID: 29245 RVA: 0x002BA71F File Offset: 0x002B891F
	protected override void OnSpawn()
	{
		this.editCrewButton.onClick += this.OnChangeCrewButtonPressed;
	}

	// Token: 0x0600723E RID: 29246 RVA: 0x002BA738 File Offset: 0x002B8938
	public override int GetSideScreenSortOrder()
	{
		return 102;
	}

	// Token: 0x0600723F RID: 29247 RVA: 0x002BA73C File Offset: 0x002B893C
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

	// Token: 0x06007240 RID: 29248 RVA: 0x002BA7C0 File Offset: 0x002B89C0
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
		Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		Game.Instance.Subscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		this.Refresh();
	}

	// Token: 0x06007241 RID: 29249 RVA: 0x002BA888 File Offset: 0x002B8A88
	public override void ClearTarget()
	{
		if (this.craftModuleInterface != null)
		{
			this.craftModuleInterface.Unsubscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
		}
		base.ClearTarget();
		Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		this.craftModuleInterface = null;
	}

	// Token: 0x06007242 RID: 29250 RVA: 0x002BA8E7 File Offset: 0x002B8AE7
	private void OnRocketModuleCountChanged(object o)
	{
		this.Refresh();
	}

	// Token: 0x06007243 RID: 29251 RVA: 0x002BA8EF File Offset: 0x002B8AEF
	private void OnAssignmentGroupChanged(object o)
	{
		this.Refresh();
	}

	// Token: 0x06007244 RID: 29252 RVA: 0x002BA8F8 File Offset: 0x002B8AF8
	private void OnChangeCrewButtonPressed()
	{
		if (this.activeChangeCrewSideScreen == null)
		{
			this.activeChangeCrewSideScreen = (AssignmentGroupControllerSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeCrewSideScreenPrefab, UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TITLE);
			this.activeChangeCrewSideScreen.SetTarget(this.craftModuleInterface.GetPassengerModule().gameObject);
			return;
		}
		this.CloseSecondaryScreen();
	}

	// Token: 0x06007245 RID: 29253 RVA: 0x002BA95A File Offset: 0x002B8B5A
	private void CloseSecondaryScreen()
	{
		DetailsScreen.Instance.ClearSecondarySideScreen();
		this.activeChangeCrewSideScreen = null;
	}

	// Token: 0x06007246 RID: 29254 RVA: 0x002BA96D File Offset: 0x002B8B6D
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
			this.activeChangeCrewSideScreen = null;
		}
	}

	// Token: 0x06007247 RID: 29255 RVA: 0x002BA98C File Offset: 0x002B8B8C
	private void Refresh()
	{
		PassengerRocketModule passengerModule = this.craftModuleInterface.GetPassengerModule();
		GameObject gameObject = (passengerModule == null) ? null : passengerModule.GetDupePilot();
		bool flag = this.craftModuleInterface.GetRobotPilotModule() != null;
		bool flag2 = gameObject != null;
		bool flag3 = flag && !flag2;
		bool flag4 = flag || flag2;
		bool flag5 = flag && flag2;
		if (passengerModule == null && this.activeChangeCrewSideScreen != null)
		{
			this.CloseSecondaryScreen();
		}
		if (flag5)
		{
			this.copilotImage.sprite = Assets.GetSprite("Dreamicon_robopilot");
		}
		this.copilotImage.gameObject.SetActive(flag5);
		this.editCrewButton.isInteractable = (passengerModule != null);
		this.editCrewTooltip.SetSimpleTooltip((passengerModule != null) ? UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.EDIT_CREW_BUTTON_TOOLTIP : UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.EDIT_CREW_BUTTON_DISABLED_TOOLTIP);
		Sprite sprite;
		if (!flag4)
		{
			sprite = Assets.GetSprite("dreamIcon_Unknown");
			this.infoLabel.SetText(GameUtil.SafeStringFormat(UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.INFO_LABEL, new object[]
			{
				UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.NO_ASSIGNED_NAME
			}));
		}
		else
		{
			sprite = (flag3 ? Assets.GetSprite("Dreamicon_robopilot") : Db.Get().Personalities.Get(gameObject.GetComponent<MinionIdentity>().personalityResourceId).GetMiniIcon());
			if (flag3)
			{
				this.infoLabel.SetText(UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.INFO_LABEL_ROBOT_ONLY);
			}
			else
			{
				this.infoLabel.SetText(GameUtil.SafeStringFormat(UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.INFO_LABEL, new object[]
				{
					gameObject.GetProperName()
				}));
			}
		}
		this.pilotImage.sprite = sprite;
	}

	// Token: 0x04004EEA RID: 20202
	public const string NO_PILOT_SPRITE_NAME = "dreamIcon_Unknown";

	// Token: 0x04004EEB RID: 20203
	public const string ROBOPILOT_SPRITE_NAME = "Dreamicon_robopilot";

	// Token: 0x04004EEC RID: 20204
	public LocText infoLabel;

	// Token: 0x04004EED RID: 20205
	public ToolTip editCrewTooltip;

	// Token: 0x04004EEE RID: 20206
	public Image pilotImage;

	// Token: 0x04004EEF RID: 20207
	public Image copilotImage;

	// Token: 0x04004EF0 RID: 20208
	private Dictionary<KToggle, PassengerRocketModule.RequestCrewState> toggleMap = new Dictionary<KToggle, PassengerRocketModule.RequestCrewState>();

	// Token: 0x04004EF1 RID: 20209
	public KButton editCrewButton;

	// Token: 0x04004EF2 RID: 20210
	public KScreen changeCrewSideScreenPrefab;

	// Token: 0x04004EF3 RID: 20211
	private CraftModuleInterface craftModuleInterface;

	// Token: 0x04004EF4 RID: 20212
	private AssignmentGroupControllerSideScreen activeChangeCrewSideScreen;
}
