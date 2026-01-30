using System;
using STRINGS;

// Token: 0x02000BD0 RID: 3024
public class LoadingCompleteCondition : ProcessCondition
{
	// Token: 0x06005AA3 RID: 23203 RVA: 0x0020D9DA File Offset: 0x0020BBDA
	public LoadingCompleteCondition(Storage target)
	{
		this.target = target;
		this.userControlledTarget = target.GetComponent<IUserControlledCapacity>();
	}

	// Token: 0x06005AA4 RID: 23204 RVA: 0x0020D9F5 File Offset: 0x0020BBF5
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.userControlledTarget != null)
		{
			if (this.userControlledTarget.AmountStored < this.userControlledTarget.UserMaxCapacity)
			{
				return ProcessCondition.Status.Warning;
			}
			return ProcessCondition.Status.Ready;
		}
		else
		{
			if (!this.target.IsFull())
			{
				return ProcessCondition.Status.Warning;
			}
			return ProcessCondition.Status.Ready;
		}
	}

	// Token: 0x06005AA5 RID: 23205 RVA: 0x0020DA2B File Offset: 0x0020BC2B
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.STATUS.WARNING;
	}

	// Token: 0x06005AA6 RID: 23206 RVA: 0x0020DA42 File Offset: 0x0020BC42
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.TOOLTIP.WARNING;
	}

	// Token: 0x06005AA7 RID: 23207 RVA: 0x0020DA59 File Offset: 0x0020BC59
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C76 RID: 15478
	private Storage target;

	// Token: 0x04003C77 RID: 15479
	private IUserControlledCapacity userControlledTarget;
}
