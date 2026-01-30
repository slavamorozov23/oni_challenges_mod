using System;
using STRINGS;

// Token: 0x020008F4 RID: 2292
public class WorkTimeDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F9A RID: 16282 RVA: 0x00165E34 File Offset: 0x00164034
	public WorkTimeDiagnostic(int worldID, ChoreGroup choreGroup) : base(worldID, UI.COLONY_DIAGNOSTICS.WORKTIMEDIAGNOSTIC.ALL_NAME)
	{
		this.choreGroup = choreGroup;
		this.tracker = TrackerTool.Instance.GetWorkTimeTracker(worldID, choreGroup);
		this.trackerSampleCountSeconds = 100f;
		this.name = choreGroup.Name;
		this.id = "WorkTimeDiagnostic_" + choreGroup.Id;
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
	}

	// Token: 0x06003F9B RID: 16283 RVA: 0x00165EAC File Offset: 0x001640AC
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ((this.tracker.GetAverageValue(this.trackerSampleCountSeconds) > 0f) ? ColonyDiagnostic.DiagnosticResult.Opinion.Good : ColonyDiagnostic.DiagnosticResult.Opinion.Normal),
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetAverageValue(this.trackerSampleCountSeconds)))
		};
	}

	// Token: 0x04002769 RID: 10089
	public ChoreGroup choreGroup;
}
