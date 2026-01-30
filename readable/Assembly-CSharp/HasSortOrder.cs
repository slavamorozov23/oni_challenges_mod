using System;
using UnityEngine;

// Token: 0x02000D29 RID: 3369
[AddComponentMenu("KMonoBehaviour/scripts/HasSortOrder")]
public class HasSortOrder : KMonoBehaviour, IHasSortOrder
{
	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x0600681A RID: 26650 RVA: 0x00274C10 File Offset: 0x00272E10
	// (set) Token: 0x0600681B RID: 26651 RVA: 0x00274C18 File Offset: 0x00272E18
	public int sortOrder { get; set; }
}
