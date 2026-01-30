using System;
using System.Collections.Generic;

// Token: 0x02000ABB RID: 2747
public class QuestCriteria_GreaterThan : QuestCriteria
{
	// Token: 0x06004FF7 RID: 20471 RVA: 0x001D0ECA File Offset: 0x001CF0CA
	public QuestCriteria_GreaterThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004FF8 RID: 20472 RVA: 0x001D0ED9 File Offset: 0x001CF0D9
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current > target;
	}
}
