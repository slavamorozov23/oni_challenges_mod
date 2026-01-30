using System;

// Token: 0x0200064F RID: 1615
public class AllChoresCountTracker : WorldTracker
{
	// Token: 0x0600275B RID: 10075 RVA: 0x000E266E File Offset: 0x000E086E
	public AllChoresCountTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600275C RID: 10076 RVA: 0x000E2678 File Offset: 0x000E0878
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			Tracker choreGroupTracker = TrackerTool.Instance.GetChoreGroupTracker(base.WorldID, Db.Get().ChoreGroups[i]);
			num += ((choreGroupTracker == null) ? 0f : choreGroupTracker.GetCurrentValue());
		}
		base.AddPoint(num);
	}

	// Token: 0x0600275D RID: 10077 RVA: 0x000E26E0 File Offset: 0x000E08E0
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
