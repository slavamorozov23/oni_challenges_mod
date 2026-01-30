using System;
using UnityEngine;

// Token: 0x02000996 RID: 2454
public interface ITelepadDeliverableContainer
{
	// Token: 0x06004687 RID: 18055
	void SelectDeliverable();

	// Token: 0x06004688 RID: 18056
	void DeselectDeliverable();

	// Token: 0x06004689 RID: 18057
	GameObject GetGameObject();
}
