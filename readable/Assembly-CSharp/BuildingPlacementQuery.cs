using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004FF RID: 1279
public class BuildingPlacementQuery : PathFinderQuery
{
	// Token: 0x06001BB2 RID: 7090 RVA: 0x000993E1 File Offset: 0x000975E1
	public BuildingPlacementQuery Reset(int max_results, GameObject toPlace)
	{
		this.max_results = max_results;
		this.toPlace = toPlace;
		this.cellOffsets = toPlace.GetComponent<OccupyArea>().OccupiedCellsOffsets;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x0009940E File Offset: 0x0009760E
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidPlaceCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x0009944C File Offset: 0x0009764C
	private bool CheckValidPlaceCell(int testCell)
	{
		if (!Grid.IsValidCell(testCell) || Grid.IsSolidCell(testCell) || Grid.ObjectLayers[1].ContainsKey(testCell))
		{
			return false;
		}
		bool flag = true;
		int widthInCells = this.toPlace.GetComponent<OccupyArea>().GetWidthInCells();
		int cell = testCell;
		for (int i = 0; i < widthInCells; i++)
		{
			int cellInDirection = Grid.GetCellInDirection(cell, Direction.Down);
			if (!Grid.IsValidCell(cellInDirection) || !Grid.IsSolidCell(cellInDirection))
			{
				flag = false;
				break;
			}
			cell = Grid.GetCellInDirection(cell, Direction.Right);
		}
		if (flag)
		{
			for (int j = 0; j < this.cellOffsets.Length; j++)
			{
				CellOffset offset = this.cellOffsets[j];
				int num = Grid.OffsetCell(testCell, offset);
				if (!Grid.IsValidCell(num) || Grid.IsSolidCell(num) || !Grid.IsValidBuildingCell(num) || Grid.ObjectLayers[1].ContainsKey(num))
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x04001029 RID: 4137
	public List<int> result_cells = new List<int>();

	// Token: 0x0400102A RID: 4138
	private int max_results;

	// Token: 0x0400102B RID: 4139
	private GameObject toPlace;

	// Token: 0x0400102C RID: 4140
	private CellOffset[] cellOffsets;
}
