using System;
using UnityEngine;

// Token: 0x02000596 RID: 1430
public interface IApproachable
{
	// Token: 0x06002017 RID: 8215
	CellOffset[] GetOffsets();

	// Token: 0x06002018 RID: 8216
	int GetCell();

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06002019 RID: 8217
	Transform transform { get; }
}
