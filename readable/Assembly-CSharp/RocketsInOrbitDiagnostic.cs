using System;
using STRINGS;

// Token: 0x020008EF RID: 2287
public class RocketsInOrbitDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F87 RID: 16263 RVA: 0x00165238 File Offset: 0x00163438
	public RocketsInOrbitDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_errand_rocketry";
		base.AddCriterion("RocketsOrbiting", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.CRITERIA.CHECKORBIT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckOrbit)));
	}

	// Token: 0x06003F88 RID: 16264 RVA: 0x00165287 File Offset: 0x00163487
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06003F89 RID: 16265 RVA: 0x00165290 File Offset: 0x00163490
	public ColonyDiagnostic.DiagnosticResult CheckOrbit()
	{
		AxialI myWorldLocation = ClusterManager.Instance.GetWorld(base.worldID).GetMyWorldLocation();
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		this.numRocketsInOrbit = 0;
		Clustercraft clustercraft = null;
		bool flag = false;
		foreach (Clustercraft clustercraft2 in Components.Clustercrafts.Items)
		{
			AxialI myWorldLocation2 = clustercraft2.GetMyWorldLocation();
			AxialI destination = clustercraft2.Destination;
			if (myWorldLocation2 != myWorldLocation && ClusterGrid.Instance.IsInRange(myWorldLocation2, myWorldLocation, 1) && ClusterGrid.Instance.IsInRange(myWorldLocation, destination, 1))
			{
				this.numRocketsInOrbit++;
				clustercraft = clustercraft2;
				flag = (flag || !clustercraft2.CanLandAtAsteroid(myWorldLocation, false));
			}
		}
		if (this.numRocketsInOrbit == 1 && clustercraft != null)
		{
			result.Message = string.Format(flag ? UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.WARNING_ONE_ROCKETS_STRANDED : UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.NORMAL_ONE_IN_ORBIT, clustercraft.Name);
		}
		else if (this.numRocketsInOrbit > 0)
		{
			result.Message = string.Format(flag ? UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.WARNING_ROCKETS_STRANDED : UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.NORMAL_IN_ORBIT, this.numRocketsInOrbit);
		}
		else
		{
			result.Message = UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.NORMAL_NO_ROCKETS;
		}
		if (flag)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
		}
		else if (this.numRocketsInOrbit > 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion;
		}
		return result;
	}

	// Token: 0x04002763 RID: 10083
	private int numRocketsInOrbit;
}
