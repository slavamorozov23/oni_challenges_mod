using System;
using System.Collections.Generic;

// Token: 0x02000688 RID: 1672
public interface IDevQuickAction
{
	// Token: 0x06002936 RID: 10550
	List<DevQuickActionInstruction> GetDevInstructions();

	// Token: 0x02001556 RID: 5462
	public enum CommonMenusNames
	{
		// Token: 0x04007185 RID: 29061
		Storage
	}
}
