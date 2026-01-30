using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200061C RID: 1564
[AddComponentMenu("KMonoBehaviour/scripts/Pathfinding")]
public class Pathfinding : KMonoBehaviour
{
	// Token: 0x060024E7 RID: 9447 RVA: 0x000D3F98 File Offset: 0x000D2198
	public static void DestroyInstance()
	{
		Pathfinding.Instance = null;
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x000D3FA5 File Offset: 0x000D21A5
	protected override void OnPrefabInit()
	{
		Pathfinding.Instance = this;
	}

	// Token: 0x060024E9 RID: 9449 RVA: 0x000D3FB0 File Offset: 0x000D21B0
	public int MaxLinksPerCell()
	{
		int num = 0;
		foreach (NavGrid navGrid in this.NavGrids)
		{
			num = Mathf.Max(num, navGrid.maxLinksPerCell);
		}
		return num;
	}

	// Token: 0x060024EA RID: 9450 RVA: 0x000D400C File Offset: 0x000D220C
	public void AddNavGrid(NavGrid nav_grid)
	{
		this.NavGrids.Add(nav_grid);
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x000D401C File Offset: 0x000D221C
	public NavGrid GetNavGrid(string id)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			if (navGrid.id == id)
			{
				return navGrid;
			}
		}
		global::Debug.LogError("Could not find nav grid: " + id);
		return null;
	}

	// Token: 0x060024EC RID: 9452 RVA: 0x000D4090 File Offset: 0x000D2290
	public List<NavGrid> GetNavGrids()
	{
		return this.NavGrids;
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x000D4098 File Offset: 0x000D2298
	public void ResetNavGrids()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.InitializeGraph();
		}
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x000D40E8 File Offset: 0x000D22E8
	public void FlushNavGridsOnLoad()
	{
		if (this.navGridsHaveBeenFlushedOnLoad)
		{
			return;
		}
		this.navGridsHaveBeenFlushedOnLoad = true;
		this.UpdateNavGrids(true);
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x000D4104 File Offset: 0x000D2304
	public void UpdateNavGrids(bool update_all = false)
	{
		update_all = true;
		if (update_all)
		{
			using (List<NavGrid>.Enumerator enumerator = this.NavGrids.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NavGrid navGrid = enumerator.Current;
					navGrid.UpdateGraph();
				}
				return;
			}
		}
		foreach (NavGrid navGrid2 in this.NavGrids)
		{
			if (navGrid2.updateEveryFrame)
			{
				navGrid2.UpdateGraph();
			}
		}
		this.NavGrids[this.UpdateIdx].UpdateGraph();
		this.UpdateIdx = (this.UpdateIdx + 1) % this.NavGrids.Count;
	}

	// Token: 0x060024F0 RID: 9456 RVA: 0x000D41D4 File Offset: 0x000D23D4
	public void RenderEveryTick()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.DebugUpdate();
		}
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x000D4224 File Offset: 0x000D2424
	public void AddDirtyNavGridCell(int cell)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.AddDirtyCell(cell);
		}
	}

	// Token: 0x060024F2 RID: 9458 RVA: 0x000D4278 File Offset: 0x000D2478
	public void RefreshNavCell(int cell)
	{
		ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
		pooledList.Add(cell);
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.UpdateGraph(pooledList);
		}
		pooledList.Recycle();
	}

	// Token: 0x060024F3 RID: 9459 RVA: 0x000D42DC File Offset: 0x000D24DC
	protected override void OnCleanUp()
	{
		this.NavGrids.Clear();
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x04001595 RID: 5525
	private List<NavGrid> NavGrids = new List<NavGrid>();

	// Token: 0x04001596 RID: 5526
	private int UpdateIdx;

	// Token: 0x04001597 RID: 5527
	private bool navGridsHaveBeenFlushedOnLoad;

	// Token: 0x04001598 RID: 5528
	public static Pathfinding Instance;
}
