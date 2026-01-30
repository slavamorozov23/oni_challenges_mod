using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000652 RID: 1618
public class WorkTimeTracker : WorldTracker
{
	// Token: 0x06002764 RID: 10084 RVA: 0x000E28B6 File Offset: 0x000E0AB6
	public WorkTimeTracker(int worldID, ChoreGroup group) : base(worldID)
	{
		this.choreGroup = group;
	}

	// Token: 0x06002765 RID: 10085 RVA: 0x000E28C8 File Offset: 0x000E0AC8
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		Chore chore;
		Predicate<ChoreType> <>9__0;
		foreach (MinionIdentity minionIdentity in worldItems)
		{
			chore = minionIdentity.GetComponent<ChoreConsumer>().choreDriver.GetCurrentChore();
			if (chore != null)
			{
				List<ChoreType> choreTypes = this.choreGroup.choreTypes;
				Predicate<ChoreType> match2;
				if ((match2 = <>9__0) == null)
				{
					match2 = (<>9__0 = ((ChoreType match) => match == chore.choreType));
				}
				if (choreTypes.Find(match2) != null)
				{
					num += 1f;
				}
			}
		}
		base.AddPoint(num / (float)worldItems.Count * 100f);
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x000E29A0 File Offset: 0x000E0BA0
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(Mathf.Round(value), GameUtil.TimeSlice.None).ToString();
	}

	// Token: 0x0400173F RID: 5951
	public ChoreGroup choreGroup;
}
