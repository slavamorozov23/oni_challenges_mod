using System;
using STRINGS;

// Token: 0x020008DF RID: 2271
public class ChoreGroupDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F3A RID: 16186 RVA: 0x0016347C File Offset: 0x0016167C
	public ChoreGroupDiagnostic(int worldID, ChoreGroup choreGroup) : base(worldID, UI.COLONY_DIAGNOSTICS.CHOREGROUPDIAGNOSTIC.ALL_NAME)
	{
		this.choreGroup = choreGroup;
		this.tracker = TrackerTool.Instance.GetChoreGroupTracker(worldID, choreGroup);
		this.name = choreGroup.Name;
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
		this.id = "ChoreGroupDiagnostic_" + choreGroup.Id;
	}

	// Token: 0x06003F3B RID: 16187 RVA: 0x001634E8 File Offset: 0x001616E8
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ((this.tracker.GetCurrentValue() > 0f) ? ColonyDiagnostic.DiagnosticResult.Opinion.Good : ColonyDiagnostic.DiagnosticResult.Opinion.Normal),
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}

	// Token: 0x0400274C RID: 10060
	public ChoreGroup choreGroup;
}
