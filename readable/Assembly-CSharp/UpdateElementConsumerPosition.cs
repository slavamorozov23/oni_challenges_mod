using System;
using UnityEngine;

// Token: 0x02000154 RID: 340
[AddComponentMenu("KMonoBehaviour/scripts/UpdateElementConsumerPosition")]
public class UpdateElementConsumerPosition : KMonoBehaviour, ISim200ms
{
	// Token: 0x0600067D RID: 1661 RVA: 0x0002F602 File Offset: 0x0002D802
	protected override void OnSpawn()
	{
		this.consumer = base.GetComponent<ElementConsumer>();
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x0002F610 File Offset: 0x0002D810
	public void Sim200ms(float dt)
	{
		this.consumer.GetComponent<ElementConsumer>().RefreshConsumptionRate();
	}

	// Token: 0x040004EB RID: 1259
	private ElementConsumer consumer;
}
