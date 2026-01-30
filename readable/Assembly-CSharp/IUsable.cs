using System;
using UnityEngine;

// Token: 0x02000818 RID: 2072
public interface IUsable
{
	// Token: 0x0600383A RID: 14394
	bool IsUsable();

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x0600383B RID: 14395
	Transform transform { get; }
}
