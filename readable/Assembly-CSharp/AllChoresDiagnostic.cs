using System;
using STRINGS;

// Token: 0x020008D8 RID: 2264
public class AllChoresDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F1B RID: 16155 RVA: 0x0016232F File Offset: 0x0016052F
	public AllChoresDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<AllChoresCountTracker>(worldID);
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
		this.icon = "icon_errand_operate";
	}

	// Token: 0x06003F1C RID: 16156 RVA: 0x00162370 File Offset: 0x00160570
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}
}
