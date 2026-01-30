using System;

// Token: 0x02000500 RID: 1280
public class CellArrayQuery : PathFinderQuery
{
	// Token: 0x06001BB6 RID: 7094 RVA: 0x0009953D File Offset: 0x0009773D
	public CellArrayQuery Reset(int[] target_cells)
	{
		this.targetCells = target_cells;
		return this;
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x00099548 File Offset: 0x00097748
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		for (int i = 0; i < this.targetCells.Length; i++)
		{
			if (this.targetCells[i] == cell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0400102D RID: 4141
	private int[] targetCells;
}
