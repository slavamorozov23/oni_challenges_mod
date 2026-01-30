using System;

// Token: 0x0200091B RID: 2331
public interface IEnergyConsumer : ICircuitConnected
{
	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x0600410F RID: 16655
	float WattsUsed { get; }

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06004110 RID: 16656
	float WattsNeededWhenActive { get; }

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06004111 RID: 16657
	int PowerSortOrder { get; }

	// Token: 0x06004112 RID: 16658
	void SetConnectionStatus(CircuitManager.ConnectionStatus status);

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06004113 RID: 16659
	string Name { get; }

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06004114 RID: 16660
	bool IsConnected { get; }

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06004115 RID: 16661
	bool IsPowered { get; }
}
