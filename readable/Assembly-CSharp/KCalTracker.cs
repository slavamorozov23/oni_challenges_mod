using System;

// Token: 0x0200065A RID: 1626
public class KCalTracker : WorldTracker
{
	// Token: 0x0600277E RID: 10110 RVA: 0x000E2E88 File Offset: 0x000E1088
	public KCalTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x000E2E91 File Offset: 0x000E1091
	public override void UpdateData()
	{
		base.AddPoint(WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x000E2EBA File Offset: 0x000E10BA
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedCalories(value, GameUtil.TimeSlice.None, true);
	}
}
