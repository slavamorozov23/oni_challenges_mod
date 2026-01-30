using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000994 RID: 2452
public interface IStorage
{
	// Token: 0x0600467B RID: 18043
	bool ShouldShowInUI();

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x0600467C RID: 18044
	// (set) Token: 0x0600467D RID: 18045
	bool allowUIItemRemoval { get; set; }

	// Token: 0x0600467E RID: 18046
	GameObject Drop(GameObject go, bool do_disease_transfer = true);

	// Token: 0x0600467F RID: 18047
	List<GameObject> GetItems();

	// Token: 0x06004680 RID: 18048
	bool IsFull();

	// Token: 0x06004681 RID: 18049
	bool IsEmpty();

	// Token: 0x06004682 RID: 18050
	float Capacity();

	// Token: 0x06004683 RID: 18051
	float RemainingCapacity();

	// Token: 0x06004684 RID: 18052
	float GetAmountAvailable(Tag tag);

	// Token: 0x06004685 RID: 18053
	void ConsumeIgnoringDisease(Tag tag, float amount);
}
