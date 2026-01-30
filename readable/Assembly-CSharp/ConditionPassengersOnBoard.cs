using System;
using STRINGS;

// Token: 0x02000BC8 RID: 3016
public class ConditionPassengersOnBoard : ProcessCondition
{
	// Token: 0x06005A79 RID: 23161 RVA: 0x0020CEBF File Offset: 0x0020B0BF
	public ConditionPassengersOnBoard(PassengerRocketModule module)
	{
		this.module = module;
	}

	// Token: 0x06005A7A RID: 23162 RVA: 0x0020CED0 File Offset: 0x0020B0D0
	public override ProcessCondition.Status EvaluateCondition()
	{
		global::Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
		if (crewBoardedFraction.first != crewBoardedFraction.second)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005A7B RID: 23163 RVA: 0x0020CEFA File Offset: 0x0020B0FA
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.FAILURE;
	}

	// Token: 0x06005A7C RID: 23164 RVA: 0x0020CF18 File Offset: 0x0020B118
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		global::Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
		if (status == ProcessCondition.Status.Ready)
		{
			if (crewBoardedFraction.second != 0)
			{
				return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.READY, crewBoardedFraction.first, crewBoardedFraction.second);
			}
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.NONE, crewBoardedFraction.first, crewBoardedFraction.second);
		}
		else
		{
			if (crewBoardedFraction.first == 0)
			{
				return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.FAILURE, crewBoardedFraction.first, crewBoardedFraction.second);
			}
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.WARNING, crewBoardedFraction.first, crewBoardedFraction.second);
		}
	}

	// Token: 0x06005A7D RID: 23165 RVA: 0x0020CFDC File Offset: 0x0020B1DC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C6B RID: 15467
	private PassengerRocketModule module;
}
