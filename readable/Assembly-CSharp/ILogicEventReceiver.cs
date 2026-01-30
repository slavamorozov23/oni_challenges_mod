using System;

// Token: 0x020009EB RID: 2539
public interface ILogicEventReceiver : ILogicNetworkConnection
{
	// Token: 0x060049FB RID: 18939
	void ReceiveLogicEvent(int value);

	// Token: 0x060049FC RID: 18940
	int GetLogicCell();
}
