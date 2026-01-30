using System;

// Token: 0x0200065F RID: 1631
public class RocketOxidizerTracker : WorldTracker
{
	// Token: 0x0600278D RID: 10125 RVA: 0x000E30B0 File Offset: 0x000E12B0
	public RocketOxidizerTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x000E30BC File Offset: 0x000E12BC
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.OxidizerPowerRemaining : 0f);
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x000E3100 File Offset: 0x000E1300
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
