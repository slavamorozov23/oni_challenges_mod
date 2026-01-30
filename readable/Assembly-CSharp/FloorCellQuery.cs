using System;
using System.Collections.Generic;

// Token: 0x02000505 RID: 1285
public class FloorCellQuery : PathFinderQuery
{
	// Token: 0x06001BC6 RID: 7110 RVA: 0x000996A2 File Offset: 0x000978A2
	public FloorCellQuery Reset(int max_results, int adjacent_cells_buffer = 0)
	{
		this.max_results = max_results;
		this.adjacent_cells_buffer = adjacent_cells_buffer;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x000996BE File Offset: 0x000978BE
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidFloorCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x000996FC File Offset: 0x000978FC
	private bool CheckValidFloorCell(int testCell)
	{
		if (!Grid.IsValidCell(testCell) || Grid.IsSolidCell(testCell))
		{
			return false;
		}
		int cellInDirection = Grid.GetCellInDirection(testCell, Direction.Up);
		int cellInDirection2 = Grid.GetCellInDirection(testCell, Direction.Down);
		if (!Grid.ObjectLayers[1].ContainsKey(testCell) && Grid.IsValidCell(cellInDirection2) && Grid.IsSolidCell(cellInDirection2) && Grid.IsValidCell(cellInDirection) && !Grid.IsSolidCell(cellInDirection))
		{
			int cell = testCell;
			int cell2 = testCell;
			for (int i = 0; i < this.adjacent_cells_buffer; i++)
			{
				cell = Grid.CellLeft(cell);
				cell2 = Grid.CellRight(cell2);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell))
				{
					return false;
				}
				if (!Grid.IsValidCell(cell2) || Grid.IsSolidCell(cell2))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x04001032 RID: 4146
	public List<int> result_cells = new List<int>();

	// Token: 0x04001033 RID: 4147
	private int max_results;

	// Token: 0x04001034 RID: 4148
	private int adjacent_cells_buffer;
}
