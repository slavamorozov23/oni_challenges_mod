using System;
using STRINGS;

// Token: 0x02000BC9 RID: 3017
public class ConditionPilotOnBoard : ProcessCondition
{
	// Token: 0x06005A7E RID: 23166 RVA: 0x0020CFDF File Offset: 0x0020B1DF
	public ConditionPilotOnBoard(PassengerRocketModule module)
	{
		this.module = module;
		this.rocketModule = module.GetComponent<RocketModuleCluster>();
	}

	// Token: 0x06005A7F RID: 23167 RVA: 0x0020CFFA File Offset: 0x0020B1FA
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.CheckPilotBoarded())
		{
			return ProcessCondition.Status.Ready;
		}
		if (this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005A80 RID: 23168 RVA: 0x0020D028 File Offset: 0x0020B228
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.READY;
		}
		if (status == ProcessCondition.Status.Warning && this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.ROBO_PILOT_WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.FAILURE;
	}

	// Token: 0x06005A81 RID: 23169 RVA: 0x0020D078 File Offset: 0x0020B278
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.READY;
		}
		if (status == ProcessCondition.Status.Warning && this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.ROBO_PILOT_WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.FAILURE;
	}

	// Token: 0x06005A82 RID: 23170 RVA: 0x0020D0C5 File Offset: 0x0020B2C5
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C6C RID: 15468
	private PassengerRocketModule module;

	// Token: 0x04003C6D RID: 15469
	private RocketModuleCluster rocketModule;
}
