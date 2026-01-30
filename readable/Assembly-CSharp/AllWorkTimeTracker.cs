using System;

// Token: 0x02000651 RID: 1617
public class AllWorkTimeTracker : WorldTracker
{
	// Token: 0x06002761 RID: 10081 RVA: 0x000E2842 File Offset: 0x000E0A42
	public AllWorkTimeTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x000E284C File Offset: 0x000E0A4C
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			num += TrackerTool.Instance.GetWorkTimeTracker(base.WorldID, Db.Get().ChoreGroups[i]).GetCurrentValue();
		}
		base.AddPoint(num);
	}

	// Token: 0x06002763 RID: 10083 RVA: 0x000E28A8 File Offset: 0x000E0AA8
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(value, GameUtil.TimeSlice.None).ToString();
	}
}
