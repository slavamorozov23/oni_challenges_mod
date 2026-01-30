using System;
using System.Collections.Generic;

// Token: 0x02000ABD RID: 2749
public class QuestCriteria_GreaterOrEqual : QuestCriteria
{
	// Token: 0x06004FFB RID: 20475 RVA: 0x001D0EF4 File Offset: 0x001CF0F4
	public QuestCriteria_GreaterOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004FFC RID: 20476 RVA: 0x001D0F03 File Offset: 0x001CF103
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current >= target;
	}
}
