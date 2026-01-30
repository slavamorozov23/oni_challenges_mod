using System;
using System.Collections.Generic;
using UnityEngine.Pool;

// Token: 0x020004FD RID: 1277
public static class PathProber
{
	// Token: 0x06001BAB RID: 7083 RVA: 0x00099200 File Offset: 0x00097400
	public static void Run(int root_cell, PathFinderAbilities abilities, NavGrid nav_grid, NavType starting_nav_type, PathGrid path_grid, ushort serialNo, PathFinder.PotentialScratchPad scratchPad, PathFinder.PotentialList potentials, PathFinder.PotentialPath.Flags flags, List<int> found_cells = null)
	{
		path_grid.BeginUpdate(serialNo, root_cell, found_cells);
		bool flag;
		PathFinder.Cell cell = path_grid.GetCell(root_cell, starting_nav_type, out flag);
		PathFinder.AddPotential(new PathFinder.PotentialPath(root_cell, starting_nav_type, flags), Grid.InvalidCell, NavType.NumNavTypes, 0, 0, potentials, path_grid, ref cell);
		while (potentials.Count > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = potentials.Next();
			cell = path_grid.GetCell(keyValuePair.Value, out flag);
			if (cell.cost == keyValuePair.Key)
			{
				PathFinder.AddPotentials(scratchPad, keyValuePair.Value, cell.cost, ref abilities, null, nav_grid.maxLinksPerCell, nav_grid.Links, potentials, path_grid, cell.parent, cell.parentNavType);
			}
		}
		path_grid.EndUpdate();
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x000992B4 File Offset: 0x000974B4
	public static void Run(Navigator navigator, List<int> found_cells = null)
	{
		PathFinder.PotentialScratchPad potentialScratchPad = PathProber.ScratchPadPool.Get();
		PathFinder.PotentialList potentialList = PathProber.PotentialListPool.Get();
		ushort num = navigator.PathGrid.SerialNo + 1;
		if (num == 0)
		{
			num += 1;
			navigator.PathGrid.ResetProberCells();
		}
		PathFinderAbilities currentAbilities = navigator.GetCurrentAbilities();
		PathProber.Run(navigator.cachedCell, currentAbilities, navigator.NavGrid, navigator.CurrentNavType, navigator.PathGrid, num, potentialScratchPad, potentialList, navigator.flags, found_cells);
		PathProber.ScratchPadPool.Release(potentialScratchPad);
		PathProber.PotentialListPool.Release(potentialList);
	}

	// Token: 0x04001020 RID: 4128
	public const int InvalidHandle = -1;

	// Token: 0x04001021 RID: 4129
	public const int InvalidIdx = -1;

	// Token: 0x04001022 RID: 4130
	public const int InvalidCell = -1;

	// Token: 0x04001023 RID: 4131
	public const int InvalidCost = -1;

	// Token: 0x04001024 RID: 4132
	public const ushort InvalidSerialNo = 0;

	// Token: 0x04001025 RID: 4133
	private static ObjectPool<PathFinder.PotentialScratchPad> ScratchPadPool = new ObjectPool<PathFinder.PotentialScratchPad>(() => new PathFinder.PotentialScratchPad(Pathfinding.Instance.MaxLinksPerCell()), null, null, null, false, 1, 4);

	// Token: 0x04001026 RID: 4134
	private static ObjectPool<PathFinder.PotentialList> PotentialListPool = new ObjectPool<PathFinder.PotentialList>(() => new PathFinder.PotentialList(), null, delegate(PathFinder.PotentialList list)
	{
		list.Clear();
	}, null, false, 1, 4);
}
