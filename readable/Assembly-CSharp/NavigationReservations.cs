using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200060E RID: 1550
[AddComponentMenu("KMonoBehaviour/scripts/NavigationReservations")]
public class NavigationReservations : KMonoBehaviour
{
	// Token: 0x06002450 RID: 9296 RVA: 0x000D1E06 File Offset: 0x000D0006
	public static void DestroyInstance()
	{
		NavigationReservations.Instance = null;
	}

	// Token: 0x06002451 RID: 9297 RVA: 0x000D1E0E File Offset: 0x000D000E
	public int GetOccupancyCount(int cell)
	{
		if (this.cellOccupancyDensity.ContainsKey(cell))
		{
			return this.cellOccupancyDensity[cell];
		}
		return 0;
	}

	// Token: 0x06002452 RID: 9298 RVA: 0x000D1E2C File Offset: 0x000D002C
	public void AddOccupancy(int cell)
	{
		if (!this.cellOccupancyDensity.ContainsKey(cell))
		{
			this.cellOccupancyDensity.Add(cell, 1);
			return;
		}
		Dictionary<int, int> dictionary = this.cellOccupancyDensity;
		dictionary[cell]++;
	}

	// Token: 0x06002453 RID: 9299 RVA: 0x000D1E70 File Offset: 0x000D0070
	public void RemoveOccupancy(int cell)
	{
		int num = 0;
		if (this.cellOccupancyDensity.TryGetValue(cell, out num))
		{
			if (num == 1)
			{
				this.cellOccupancyDensity.Remove(cell);
				return;
			}
			this.cellOccupancyDensity[cell] = num - 1;
		}
	}

	// Token: 0x06002454 RID: 9300 RVA: 0x000D1EB0 File Offset: 0x000D00B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NavigationReservations.Instance = this;
	}

	// Token: 0x04001527 RID: 5415
	public static NavigationReservations Instance;

	// Token: 0x04001528 RID: 5416
	public static int InvalidReservation = -1;

	// Token: 0x04001529 RID: 5417
	private Dictionary<int, int> cellOccupancyDensity = new Dictionary<int, int>();
}
