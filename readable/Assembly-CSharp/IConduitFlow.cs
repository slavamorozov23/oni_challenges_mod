using System;

// Token: 0x02000876 RID: 2166
public interface IConduitFlow
{
	// Token: 0x06003B74 RID: 15220
	void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default);

	// Token: 0x06003B75 RID: 15221
	void RemoveConduitUpdater(Action<float> callback);

	// Token: 0x06003B76 RID: 15222
	bool IsConduitEmpty(int cell);
}
