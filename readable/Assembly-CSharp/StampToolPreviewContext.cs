using System;
using UnityEngine;

// Token: 0x020009C6 RID: 2502
public class StampToolPreviewContext
{
	// Token: 0x0400305E RID: 12382
	public Transform previewParent;

	// Token: 0x0400305F RID: 12383
	public InterfaceTool tool;

	// Token: 0x04003060 RID: 12384
	public TemplateContainer stampTemplate;

	// Token: 0x04003061 RID: 12385
	public System.Action frameAfterSetupFn;

	// Token: 0x04003062 RID: 12386
	public Action<int> refreshFn;

	// Token: 0x04003063 RID: 12387
	public System.Action onPlaceFn;

	// Token: 0x04003064 RID: 12388
	public Action<string> onErrorChangeFn;

	// Token: 0x04003065 RID: 12389
	public System.Action cleanupFn;
}
