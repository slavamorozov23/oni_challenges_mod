using System;

// Token: 0x0200065E RID: 1630
public class RocketFuelTracker : WorldTracker
{
	// Token: 0x0600278A RID: 10122 RVA: 0x000E3051 File Offset: 0x000E1251
	public RocketFuelTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x000E305C File Offset: 0x000E125C
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.FuelRemaining : 0f);
	}

	// Token: 0x0600278C RID: 10124 RVA: 0x000E30A0 File Offset: 0x000E12A0
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
