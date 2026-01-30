using System;
using STRINGS;

// Token: 0x020008ED RID: 2285
public class RocketFuelDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F81 RID: 16257 RVA: 0x00165090 File Offset: 0x00163290
	public RocketFuelDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ROCKETFUELDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<RocketFuelTracker>(worldID);
		this.icon = "rocket_fuel";
	}

	// Token: 0x06003F82 RID: 16258 RVA: 0x001650BF File Offset: 0x001632BF
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06003F83 RID: 16259 RVA: 0x001650C8 File Offset: 0x001632C8
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.worldID).gameObject.GetComponent<Clustercraft>();
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.ROCKETFUELDIAGNOSTIC.NORMAL;
		if (component.ModuleInterface.FuelRemaining == 0f)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.ROCKETFUELDIAGNOSTIC.WARNING;
		}
		return result;
	}
}
