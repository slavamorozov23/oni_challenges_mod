using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200050F RID: 1295
public class StaterpillarCellQuery : PathFinderQuery
{
	// Token: 0x06001BEE RID: 7150 RVA: 0x0009A854 File Offset: 0x00098A54
	public StaterpillarCellQuery Reset(int max_results, GameObject tester, ObjectLayer conduitLayer)
	{
		this.max_results = max_results;
		this.tester = tester;
		this.result_cells.Clear();
		ObjectLayer objectLayer;
		if (conduitLayer <= ObjectLayer.LiquidConduit)
		{
			if (conduitLayer == ObjectLayer.GasConduit)
			{
				objectLayer = ObjectLayer.GasConduitConnection;
				goto IL_4A;
			}
			if (conduitLayer == ObjectLayer.LiquidConduit)
			{
				objectLayer = ObjectLayer.LiquidConduitConnection;
				goto IL_4A;
			}
		}
		else
		{
			if (conduitLayer == ObjectLayer.SolidConduit)
			{
				objectLayer = ObjectLayer.SolidConduitConnection;
				goto IL_4A;
			}
			if (conduitLayer == ObjectLayer.Wire)
			{
				objectLayer = ObjectLayer.WireConnectors;
				goto IL_4A;
			}
		}
		objectLayer = conduitLayer;
		IL_4A:
		this.connectorLayer = objectLayer;
		return this;
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x0009A8B3 File Offset: 0x00098AB3
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidRoofCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x0009A8F0 File Offset: 0x00098AF0
	private bool CheckValidRoofCell(int testCell)
	{
		if (!this.tester.GetComponent<Navigator>().NavGrid.NavTable.IsValid(testCell, NavType.Ceiling))
		{
			return false;
		}
		int cellInDirection = Grid.GetCellInDirection(testCell, Direction.Down);
		return !Grid.ObjectLayers[1].ContainsKey(testCell) && !Grid.ObjectLayers[1].ContainsKey(cellInDirection) && !Grid.Objects[cellInDirection, (int)this.connectorLayer] && Grid.IsValidBuildingCell(testCell) && Grid.IsValidCell(cellInDirection) && Grid.IsValidBuildingCell(cellInDirection) && !Grid.IsSolidCell(cellInDirection);
	}

	// Token: 0x04001075 RID: 4213
	public List<int> result_cells = new List<int>();

	// Token: 0x04001076 RID: 4214
	private int max_results;

	// Token: 0x04001077 RID: 4215
	private GameObject tester;

	// Token: 0x04001078 RID: 4216
	private ObjectLayer connectorLayer;
}
