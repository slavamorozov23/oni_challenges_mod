using System;
using UnityEngine;

// Token: 0x020004F6 RID: 1270
public static class NavTypeHelper
{
	// Token: 0x06001B77 RID: 7031 RVA: 0x0009802C File Offset: 0x0009622C
	public static Vector3 GetNavPos(int cell, NavType nav_type)
	{
		Vector3 zero = Vector3.zero;
		switch (nav_type)
		{
		case NavType.Floor:
			return Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
		case NavType.LeftWall:
			return Grid.CellToPosLCC(cell, Grid.SceneLayer.Move);
		case NavType.RightWall:
			return Grid.CellToPosRCC(cell, Grid.SceneLayer.Move);
		case NavType.Ceiling:
			return Grid.CellToPosCTC(cell, Grid.SceneLayer.Move);
		case NavType.Ladder:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		case NavType.Pole:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		case NavType.Tube:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		case NavType.Solid:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		}
		return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x000980D4 File Offset: 0x000962D4
	public static int GetAnchorCell(NavType nav_type, int cell)
	{
		int result = Grid.InvalidCell;
		if (Grid.IsValidCell(cell))
		{
			switch (nav_type)
			{
			case NavType.Floor:
				result = Grid.CellBelow(cell);
				break;
			case NavType.LeftWall:
				result = Grid.CellLeft(cell);
				break;
			case NavType.RightWall:
				result = Grid.CellRight(cell);
				break;
			case NavType.Ceiling:
				result = Grid.CellAbove(cell);
				break;
			default:
				if (nav_type == NavType.Solid)
				{
					result = cell;
				}
				break;
			}
		}
		return result;
	}
}
