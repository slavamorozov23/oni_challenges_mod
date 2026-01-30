using System;
using STRINGS;

// Token: 0x020008D9 RID: 2265
public class AllWorkTimeDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F1D RID: 16157 RVA: 0x001623C5 File Offset: 0x001605C5
	public AllWorkTimeDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<AllWorkTimeTracker>(worldID);
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
	}

	// Token: 0x06003F1E RID: 16158 RVA: 0x001623FC File Offset: 0x001605FC
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}
}
