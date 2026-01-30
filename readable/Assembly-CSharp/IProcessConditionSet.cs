using System;
using System.Collections.Generic;

// Token: 0x02000E2D RID: 3629
public interface IProcessConditionSet
{
	// Token: 0x0600733D RID: 29501
	List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType);

	// Token: 0x0600733E RID: 29502
	int PopulateConditionSet(ProcessCondition.ProcessConditionType conditionType, List<ProcessCondition> conditions);
}
