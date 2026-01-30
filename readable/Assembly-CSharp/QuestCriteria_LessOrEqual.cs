using System;
using System.Collections.Generic;

// Token: 0x02000ABE RID: 2750
public class QuestCriteria_LessOrEqual : QuestCriteria
{
	// Token: 0x06004FFD RID: 20477 RVA: 0x001D0F0C File Offset: 0x001CF10C
	public QuestCriteria_LessOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004FFE RID: 20478 RVA: 0x001D0F1B File Offset: 0x001CF11B
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current <= target;
	}
}
