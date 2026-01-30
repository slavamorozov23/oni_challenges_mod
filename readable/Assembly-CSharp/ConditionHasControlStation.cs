using System;
using STRINGS;

// Token: 0x02000BC1 RID: 3009
public class ConditionHasControlStation : ProcessCondition
{
	// Token: 0x06005A55 RID: 23125 RVA: 0x0020C525 File Offset: 0x0020A725
	public ConditionHasControlStation(RocketModuleCluster module)
	{
		this.module = module;
	}

	// Token: 0x06005A56 RID: 23126 RVA: 0x0020C534 File Offset: 0x0020A734
	public override ProcessCondition.Status EvaluateCondition()
	{
		ProcessCondition.Status result = ProcessCondition.Status.Failure;
		if (Components.RocketControlStations.GetWorldItems(this.module.CraftInterface.GetComponent<WorldContainer>().id, false).Count > 0)
		{
			result = ProcessCondition.Status.Ready;
		}
		else if (this.module.CraftInterface.GetRobotPilotModule() != null)
		{
			result = ProcessCondition.Status.Warning;
		}
		return result;
	}

	// Token: 0x06005A57 RID: 23127 RVA: 0x0020C58A File Offset: 0x0020A78A
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.READY;
		}
		if (status == ProcessCondition.Status.Warning)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.FAILURE;
	}

	// Token: 0x06005A58 RID: 23128 RVA: 0x0020C5B4 File Offset: 0x0020A7B4
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.READY;
		}
		if (status == ProcessCondition.Status.Warning)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.WARNING_ROBO_PILOT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.FAILURE;
	}

	// Token: 0x06005A59 RID: 23129 RVA: 0x0020C5DE File Offset: 0x0020A7DE
	public override bool ShowInUI()
	{
		return this.EvaluateCondition() != ProcessCondition.Status.Ready;
	}

	// Token: 0x04003C62 RID: 15458
	private RocketModuleCluster module;
}
