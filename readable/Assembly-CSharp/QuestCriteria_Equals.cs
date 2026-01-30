using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ABA RID: 2746
public class QuestCriteria_Equals : QuestCriteria
{
	// Token: 0x06004FF5 RID: 20469 RVA: 0x001D0EA7 File Offset: 0x001CF0A7
	public QuestCriteria_Equals(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004FF6 RID: 20470 RVA: 0x001D0EB6 File Offset: 0x001CF0B6
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return Mathf.Abs(target - current) <= Mathf.Epsilon;
	}
}
