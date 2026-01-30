using System;

// Token: 0x020009EA RID: 2538
public interface ILogicEventSender : ILogicNetworkConnection
{
	// Token: 0x060049F8 RID: 18936
	void LogicTick();

	// Token: 0x060049F9 RID: 18937
	int GetLogicCell();

	// Token: 0x060049FA RID: 18938
	int GetLogicValue();
}
