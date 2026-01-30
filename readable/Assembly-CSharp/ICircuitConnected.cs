using System;

// Token: 0x02000842 RID: 2114
public interface ICircuitConnected
{
	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x060039A0 RID: 14752
	bool IsVirtual { get; }

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x060039A1 RID: 14753
	int PowerCell { get; }

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x060039A2 RID: 14754
	object VirtualCircuitKey { get; }
}
