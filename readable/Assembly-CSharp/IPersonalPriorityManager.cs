using System;

// Token: 0x02000D7F RID: 3455
public interface IPersonalPriorityManager
{
	// Token: 0x06006B22 RID: 27426
	int GetAssociatedSkillLevel(ChoreGroup group);

	// Token: 0x06006B23 RID: 27427
	int GetPersonalPriority(ChoreGroup group);

	// Token: 0x06006B24 RID: 27428
	void SetPersonalPriority(ChoreGroup group, int value);

	// Token: 0x06006B25 RID: 27429
	bool IsChoreGroupDisabled(ChoreGroup group);

	// Token: 0x06006B26 RID: 27430
	void ResetPersonalPriorities();
}
