using System;
using STRINGS;

// Token: 0x02000BCC RID: 3020
public class ConditionRocketHeight : ProcessCondition
{
	// Token: 0x06005A8F RID: 23183 RVA: 0x0020D720 File Offset: 0x0020B920
	public ConditionRocketHeight(RocketEngineCluster engine)
	{
		this.engine = engine;
	}

	// Token: 0x06005A90 RID: 23184 RVA: 0x0020D72F File Offset: 0x0020B92F
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.engine.maxHeight < this.engine.GetComponent<RocketModuleCluster>().CraftInterface.RocketHeight)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005A91 RID: 23185 RVA: 0x0020D758 File Offset: 0x0020B958
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A92 RID: 23186 RVA: 0x0020D798 File Offset: 0x0020B998
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A93 RID: 23187 RVA: 0x0020D7D8 File Offset: 0x0020B9D8
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C72 RID: 15474
	private RocketEngineCluster engine;
}
