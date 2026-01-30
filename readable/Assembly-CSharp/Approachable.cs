using System;
using UnityEngine;

// Token: 0x02000597 RID: 1431
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Approachable")]
public class Approachable : KMonoBehaviour, IApproachable
{
	// Token: 0x0600201A RID: 8218 RVA: 0x000B9346 File Offset: 0x000B7546
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x000B934D File Offset: 0x000B754D
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}
