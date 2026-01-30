using System;
using System.Collections.Generic;

// Token: 0x02000E35 RID: 3637
public interface IDispenser
{
	// Token: 0x06007371 RID: 29553
	List<Tag> DispensedItems();

	// Token: 0x06007372 RID: 29554
	Tag SelectedItem();

	// Token: 0x06007373 RID: 29555
	void SelectItem(Tag tag);

	// Token: 0x06007374 RID: 29556
	void OnOrderDispense();

	// Token: 0x06007375 RID: 29557
	void OnCancelDispense();

	// Token: 0x06007376 RID: 29558
	bool HasOpenChore();

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x06007377 RID: 29559
	// (remove) Token: 0x06007378 RID: 29560
	event System.Action OnStopWorkEvent;
}
