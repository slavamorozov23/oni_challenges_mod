using System;
using STRINGS;

// Token: 0x02000BC7 RID: 3015
public class ConditionOnLaunchPad : ProcessCondition
{
	// Token: 0x06005A74 RID: 23156 RVA: 0x0020CE15 File Offset: 0x0020B015
	public ConditionOnLaunchPad(CraftModuleInterface craftInterface)
	{
		this.craftInterface = craftInterface;
	}

	// Token: 0x06005A75 RID: 23157 RVA: 0x0020CE24 File Offset: 0x0020B024
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!(this.craftInterface.CurrentPad != null))
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005A76 RID: 23158 RVA: 0x0020CE3C File Offset: 0x0020B03C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A77 RID: 23159 RVA: 0x0020CE7C File Offset: 0x0020B07C
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A78 RID: 23160 RVA: 0x0020CEBC File Offset: 0x0020B0BC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C6A RID: 15466
	private CraftModuleInterface craftInterface;
}
