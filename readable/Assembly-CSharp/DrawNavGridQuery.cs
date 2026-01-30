using System;
using UnityEngine;

// Token: 0x02000504 RID: 1284
public class DrawNavGridQuery : PathFinderQuery
{
	// Token: 0x06001BC3 RID: 7107 RVA: 0x00099632 File Offset: 0x00097832
	public DrawNavGridQuery Reset(MinionBrain brain)
	{
		return this;
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x00099638 File Offset: 0x00097838
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (parent_cell == Grid.InvalidCell || (int)Grid.WorldIdx[parent_cell] != ClusterManager.Instance.activeWorldId || (int)Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
		{
			return false;
		}
		GL.Color(Color.white);
		GL.Vertex(Grid.CellToPosCCC(parent_cell, Grid.SceneLayer.Move));
		GL.Vertex(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
		return false;
	}
}
