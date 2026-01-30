using System;
using System.Collections.Generic;

// Token: 0x02000E57 RID: 3671
public interface INToggleSideScreenControl
{
	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x06007461 RID: 29793
	string SidescreenTitleKey { get; }

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x06007462 RID: 29794
	List<LocString> Options { get; }

	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x06007463 RID: 29795
	List<LocString> Tooltips { get; }

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x06007464 RID: 29796
	string Description { get; }

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x06007465 RID: 29797
	int SelectedOption { get; }

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x06007466 RID: 29798
	int QueuedOption { get; }

	// Token: 0x06007467 RID: 29799
	void QueueSelectedOption(int option);
}
