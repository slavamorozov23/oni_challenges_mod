using System;
using UnityEngine;

// Token: 0x020005D9 RID: 1497
public struct ElementSplitter
{
	// Token: 0x060022A2 RID: 8866 RVA: 0x000C9898 File Offset: 0x000C7A98
	public ElementSplitter(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.kPrefabID = go.GetComponent<KPrefabID>();
		this.onTakeCB = null;
		this.canAbsorbCB = null;
	}

	// Token: 0x0400143E RID: 5182
	public PrimaryElement primaryElement;

	// Token: 0x0400143F RID: 5183
	public Func<Pickupable, float, Pickupable> onTakeCB;

	// Token: 0x04001440 RID: 5184
	public Func<Pickupable, bool> canAbsorbCB;

	// Token: 0x04001441 RID: 5185
	public KPrefabID kPrefabID;
}
