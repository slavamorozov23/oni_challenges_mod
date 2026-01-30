using System;
using System.Collections.Generic;

// Token: 0x02000ABC RID: 2748
public class QuestCriteria_LessThan : QuestCriteria
{
	// Token: 0x06004FF9 RID: 20473 RVA: 0x001D0EDF File Offset: 0x001CF0DF
	public QuestCriteria_LessThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004FFA RID: 20474 RVA: 0x001D0EEE File Offset: 0x001CF0EE
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current < target;
	}
}
