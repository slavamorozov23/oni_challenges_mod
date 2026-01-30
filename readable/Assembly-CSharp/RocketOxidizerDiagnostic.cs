using System;
using STRINGS;

// Token: 0x020008EE RID: 2286
public class RocketOxidizerDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F84 RID: 16260 RVA: 0x00165155 File Offset: 0x00163355
	public RocketOxidizerDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ROCKETOXIDIZERDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<RocketOxidizerTracker>(worldID);
		this.icon = "rocket_oxidizer";
	}

	// Token: 0x06003F85 RID: 16261 RVA: 0x00165184 File Offset: 0x00163384
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06003F86 RID: 16262 RVA: 0x0016518C File Offset: 0x0016338C
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.worldID).gameObject.GetComponent<Clustercraft>();
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.ROCKETOXIDIZERDIAGNOSTIC.NORMAL;
		RocketEngineCluster engine = component.ModuleInterface.GetEngine();
		if (component.ModuleInterface.OxidizerPowerRemaining == 0f && engine != null && engine.requireOxidizer)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.ROCKETOXIDIZERDIAGNOSTIC.WARNING;
		}
		return result;
	}
}
