using System;
using UnityEngine;

// Token: 0x020005A7 RID: 1447
[AddComponentMenu("KMonoBehaviour/scripts/Chattable")]
public class Chattable : KMonoBehaviour, IApproachable
{
	// Token: 0x060020EE RID: 8430 RVA: 0x000BE6DD File Offset: 0x000BC8DD
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Chat;
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x000BE6E4 File Offset: 0x000BC8E4
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}
