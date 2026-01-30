using System;
using UnityEngine;

// Token: 0x02000A7B RID: 2683
public class OffsetTracker
{
	// Token: 0x06004E0B RID: 19979 RVA: 0x001C64A8 File Offset: 0x001C46A8
	public virtual CellOffset[] GetOffsets(int current_cell)
	{
		if (current_cell != this.previousCell)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateCell(this.previousCell, current_cell);
			this.previousCell = current_cell;
		}
		if (this.offsets == null)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateOffsets(this.previousCell);
		}
		return this.offsets;
	}

	// Token: 0x06004E0C RID: 19980 RVA: 0x001C6510 File Offset: 0x001C4710
	public virtual bool ValidateOffsets(int current_cell)
	{
		return current_cell == this.previousCell && this.offsets != null;
	}

	// Token: 0x06004E0D RID: 19981 RVA: 0x001C6528 File Offset: 0x001C4728
	public void ForceRefresh()
	{
		int cell = this.previousCell;
		this.previousCell = Grid.InvalidCell;
		this.Refresh(cell);
	}

	// Token: 0x06004E0E RID: 19982 RVA: 0x001C654E File Offset: 0x001C474E
	public void Refresh(int cell)
	{
		this.GetOffsets(cell);
	}

	// Token: 0x06004E0F RID: 19983 RVA: 0x001C6558 File Offset: 0x001C4758
	protected virtual void UpdateCell(int previous_cell, int current_cell)
	{
	}

	// Token: 0x06004E10 RID: 19984 RVA: 0x001C655A File Offset: 0x001C475A
	protected virtual void UpdateOffsets(int current_cell)
	{
	}

	// Token: 0x06004E11 RID: 19985 RVA: 0x001C655C File Offset: 0x001C475C
	public virtual void Clear()
	{
	}

	// Token: 0x06004E12 RID: 19986 RVA: 0x001C655E File Offset: 0x001C475E
	public virtual void DebugDrawExtents()
	{
	}

	// Token: 0x06004E13 RID: 19987 RVA: 0x001C6560 File Offset: 0x001C4760
	public virtual void DebugDrawEditor()
	{
	}

	// Token: 0x06004E14 RID: 19988 RVA: 0x001C6564 File Offset: 0x001C4764
	public virtual void DebugDrawOffsets(int cell)
	{
		foreach (CellOffset offset in this.GetOffsets(cell))
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
			Gizmos.DrawWireCube(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Move), new Vector3(0.95f, 0.95f, 0.95f));
		}
	}

	// Token: 0x04003402 RID: 13314
	public static bool isExecutingWithinJob;

	// Token: 0x04003403 RID: 13315
	protected CellOffset[] offsets;

	// Token: 0x04003404 RID: 13316
	protected int previousCell = Grid.InvalidCell;
}
