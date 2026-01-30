using System;

// Token: 0x0200086E RID: 2158
public interface IConduitConsumer
{
	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06003B3A RID: 15162
	Storage Storage { get; }

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06003B3B RID: 15163
	ConduitType ConduitType { get; }
}
