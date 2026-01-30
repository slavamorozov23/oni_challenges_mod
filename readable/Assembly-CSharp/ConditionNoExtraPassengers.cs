using System;
using STRINGS;

// Token: 0x02000BC6 RID: 3014
public class ConditionNoExtraPassengers : ProcessCondition
{
	// Token: 0x06005A6F RID: 23151 RVA: 0x0020CDBB File Offset: 0x0020AFBB
	public ConditionNoExtraPassengers(PassengerRocketModule module)
	{
		this.module = module;
	}

	// Token: 0x06005A70 RID: 23152 RVA: 0x0020CDCA File Offset: 0x0020AFCA
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!this.module.CheckExtraPassengers())
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005A71 RID: 23153 RVA: 0x0020CDDC File Offset: 0x0020AFDC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.FAILURE;
	}

	// Token: 0x06005A72 RID: 23154 RVA: 0x0020CDF7 File Offset: 0x0020AFF7
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.TOOLTIP.FAILURE;
	}

	// Token: 0x06005A73 RID: 23155 RVA: 0x0020CE12 File Offset: 0x0020B012
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C69 RID: 15465
	private PassengerRocketModule module;
}
