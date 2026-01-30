using System;
using STRINGS;

// Token: 0x020008E7 RID: 2279
public class HeatDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F6D RID: 16237 RVA: 0x00164548 File Offset: 0x00162748
	public HeatDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<BatteryTracker>(worldID);
		this.trackerSampleCountSeconds = 4f;
		base.AddCriterion("CheckHeat", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.CRITERIA.CHECKHEAT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHeat)));
	}

	// Token: 0x06003F6E RID: 16238 RVA: 0x001645A8 File Offset: 0x001627A8
	private ColonyDiagnostic.DiagnosticResult CheckHeat()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.NORMAL
		};
	}
}
