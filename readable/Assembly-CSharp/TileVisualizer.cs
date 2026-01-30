using System;
using UnityEngine;

// Token: 0x0200064B RID: 1611
public class TileVisualizer
{
	// Token: 0x0600273C RID: 10044 RVA: 0x000E18F4 File Offset: 0x000DFAF4
	private static void RefreshCellInternal(int cell, ObjectLayer tile_layer)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, (int)tile_layer];
		if (gameObject != null)
		{
			World.Instance.blockTileRenderer.Rebuild(tile_layer, cell);
			KAnimGraphTileVisualizer componentInChildren = gameObject.GetComponentInChildren<KAnimGraphTileVisualizer>();
			if (componentInChildren != null)
			{
				componentInChildren.Refresh();
				return;
			}
		}
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x000E1950 File Offset: 0x000DFB50
	private static void RefreshCell(int cell, ObjectLayer tile_layer)
	{
		if (tile_layer == ObjectLayer.NumLayers)
		{
			return;
		}
		TileVisualizer.RefreshCellInternal(cell, tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellAbove(cell), tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellBelow(cell), tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellLeft(cell), tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellRight(cell), tile_layer);
	}

	// Token: 0x0600273E RID: 10046 RVA: 0x000E198F File Offset: 0x000DFB8F
	public static void RefreshCell(int cell, ObjectLayer tile_layer, ObjectLayer replacement_layer)
	{
		TileVisualizer.RefreshCell(cell, tile_layer);
		TileVisualizer.RefreshCell(cell, replacement_layer);
	}
}
