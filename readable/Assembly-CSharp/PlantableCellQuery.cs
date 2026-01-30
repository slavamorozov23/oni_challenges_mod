using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000509 RID: 1289
public class PlantableCellQuery : PathFinderQuery
{
	// Token: 0x06001BD6 RID: 7126 RVA: 0x000999E3 File Offset: 0x00097BE3
	public PlantableCellQuery Reset(PlantableSeed seed, int max_results)
	{
		this.seed = seed;
		this.max_results = max_results;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x00099A00 File Offset: 0x00097C00
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidPlotCell(this.seed, cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x00099A4C File Offset: 0x00097C4C
	private bool CheckValidPlotCell(PlantableSeed seed, int plant_cell)
	{
		if (!Grid.IsValidCell(plant_cell))
		{
			return false;
		}
		int num;
		if (seed.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(plant_cell);
		}
		else
		{
			num = Grid.CellBelow(plant_cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (!Grid.Solid[num])
		{
			return false;
		}
		if (Grid.Objects[plant_cell, 5])
		{
			return false;
		}
		if (Grid.Objects[plant_cell, 1])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[num, 1];
		if (gameObject)
		{
			PlantablePlot component = gameObject.GetComponent<PlantablePlot>();
			if (component == null)
			{
				return false;
			}
			if (component.Direction != seed.Direction)
			{
				return false;
			}
			if (component.Occupant != null)
			{
				return false;
			}
		}
		else
		{
			if (!seed.TestSuitableGround(plant_cell))
			{
				return false;
			}
			if (PlantableCellQuery.CountNearbyPlants(num, this.plantDetectionRadius) > this.maxPlantsInRadius)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x00099B28 File Offset: 0x00097D28
	private static int CountNearbyPlants(int cell, int radius)
	{
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(Grid.CellToPos(cell), out num, out num2);
		int num3 = radius * 2;
		num -= radius;
		num2 -= radius;
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(num, num2, num3, num3, GameScenePartitioner.Instance.plants, pooledList);
		int num4 = 0;
		using (List<ScenePartitionerEntry>.Enumerator enumerator = pooledList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!((KPrefabID)enumerator.Current.obj).GetComponent<TreeBud>())
				{
					num4++;
				}
			}
		}
		pooledList.Recycle();
		return num4;
	}

	// Token: 0x0400103F RID: 4159
	public List<int> result_cells = new List<int>();

	// Token: 0x04001040 RID: 4160
	private PlantableSeed seed;

	// Token: 0x04001041 RID: 4161
	private int max_results;

	// Token: 0x04001042 RID: 4162
	private int plantDetectionRadius = 6;

	// Token: 0x04001043 RID: 4163
	private int maxPlantsInRadius = 2;
}
