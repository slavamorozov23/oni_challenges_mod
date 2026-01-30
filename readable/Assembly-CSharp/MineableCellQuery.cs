using System;
using System.Collections.Generic;

// Token: 0x02000508 RID: 1288
public class MineableCellQuery : PathFinderQuery
{
	// Token: 0x06001BD1 RID: 7121 RVA: 0x000998A8 File Offset: 0x00097AA8
	public MineableCellQuery Reset(Tag element, int max_results)
	{
		this.element = element;
		this.max_results = max_results;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000998C4 File Offset: 0x00097AC4
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidMineCell(this.element, cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x00099910 File Offset: 0x00097B10
	private bool CheckValidMineCell(Tag element, int testCell)
	{
		if (!Grid.IsValidCell(testCell))
		{
			return false;
		}
		foreach (Direction d in MineableCellQuery.DIRECTION_CHECKS)
		{
			int cellInDirection = Grid.GetCellInDirection(testCell, d);
			if (Grid.IsValidCell(cellInDirection) && Grid.IsSolidCell(cellInDirection) && !Grid.Foundation[cellInDirection] && Grid.Element[cellInDirection].tag == element)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0400103B RID: 4155
	public List<int> result_cells = new List<int>();

	// Token: 0x0400103C RID: 4156
	private Tag element;

	// Token: 0x0400103D RID: 4157
	private int max_results;

	// Token: 0x0400103E RID: 4158
	public static List<Direction> DIRECTION_CHECKS = new List<Direction>
	{
		Direction.Down,
		Direction.Right,
		Direction.Left,
		Direction.Up
	};
}
