using System;

// Token: 0x0200065B RID: 1627
public class ElectrobankJoulesTracker : WorldTracker
{
	// Token: 0x06002781 RID: 10113 RVA: 0x000E2EC4 File Offset: 0x000E10C4
	public ElectrobankJoulesTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x000E2ECD File Offset: 0x000E10CD
	public override void UpdateData()
	{
		base.AddPoint(WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x000E2EF6 File Offset: 0x000E10F6
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedJoules(value, "F1", GameUtil.TimeSlice.None);
	}
}
