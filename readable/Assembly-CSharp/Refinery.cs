using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007E9 RID: 2025
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Refinery")]
public class Refinery : KMonoBehaviour
{
	// Token: 0x06003619 RID: 13849 RVA: 0x001310CC File Offset: 0x0012F2CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x02001751 RID: 5969
	[Serializable]
	public struct OrderSaveData
	{
		// Token: 0x06009AC6 RID: 39622 RVA: 0x00392BE8 File Offset: 0x00390DE8
		public OrderSaveData(string id, bool infinite)
		{
			this.id = id;
			this.infinite = infinite;
		}

		// Token: 0x0400776D RID: 30573
		public string id;

		// Token: 0x0400776E RID: 30574
		public bool infinite;
	}
}
