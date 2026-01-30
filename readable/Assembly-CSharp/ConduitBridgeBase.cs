using System;

// Token: 0x0200072E RID: 1838
public class ConduitBridgeBase : KMonoBehaviour
{
	// Token: 0x06002E2E RID: 11822 RVA: 0x0010BA86 File Offset: 0x00109C86
	protected void SendEmptyOnMassTransfer()
	{
		if (this.OnMassTransfer != null)
		{
			this.OnMassTransfer(SimHashes.Void, 0f, 0f, 0, 0, null);
		}
	}

	// Token: 0x04001B6D RID: 7021
	public ConduitBridgeBase.DesiredMassTransfer desiredMassTransfer;

	// Token: 0x04001B6E RID: 7022
	public ConduitBridgeBase.ConduitBridgeEvent OnMassTransfer;

	// Token: 0x02001606 RID: 5638
	// (Invoke) Token: 0x060095BC RID: 38332
	public delegate float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);

	// Token: 0x02001607 RID: 5639
	// (Invoke) Token: 0x060095C0 RID: 38336
	public delegate void ConduitBridgeEvent(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);
}
