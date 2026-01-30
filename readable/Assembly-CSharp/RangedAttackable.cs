using System;
using UnityEngine;

// Token: 0x02000624 RID: 1572
public class RangedAttackable : AttackableBase
{
	// Token: 0x0600256C RID: 9580 RVA: 0x000D6E97 File Offset: 0x000D5097
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600256D RID: 9581 RVA: 0x000D6E9F File Offset: 0x000D509F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.preferUnreservedCell = true;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x000D6EB9 File Offset: 0x000D50B9
	public new int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x000D6EC4 File Offset: 0x000D50C4
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0f, 0.5f, 0.5f, 0.15f);
		foreach (CellOffset offset in base.GetOffsets())
		{
			Gizmos.DrawCube(new Vector3(0.5f, 0.5f, 0f) + Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset)), Vector3.one);
		}
	}
}
