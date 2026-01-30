using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Token: 0x02000A04 RID: 2564
[AddComponentMenu("KMonoBehaviour/scripts/MinionGroupProber")]
public class MinionGroupProber : KMonoBehaviour
{
	// Token: 0x06004AF7 RID: 19191 RVA: 0x001B2ED4 File Offset: 0x001B10D4
	public static void DestroyInstance()
	{
		MinionGroupProber.Instance = null;
	}

	// Token: 0x06004AF8 RID: 19192 RVA: 0x001B2EDC File Offset: 0x001B10DC
	public static MinionGroupProber Get()
	{
		return MinionGroupProber.Instance;
	}

	// Token: 0x06004AF9 RID: 19193 RVA: 0x001B2EE3 File Offset: 0x001B10E3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MinionGroupProber.Instance = this;
		this.cells = new int[Grid.CellCount];
	}

	// Token: 0x06004AFA RID: 19194 RVA: 0x001B2F01 File Offset: 0x001B1101
	public bool IsReachable(int cell)
	{
		return Grid.IsValidCell(cell) && this.cells[cell] > 0;
	}

	// Token: 0x06004AFB RID: 19195 RVA: 0x001B2F18 File Offset: 0x001B1118
	public bool IsReachable(int cell, CellOffset[] offsets)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		foreach (CellOffset offset in offsets)
		{
			if (this.IsReachable(Grid.OffsetCell(cell, offset)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004AFC RID: 19196 RVA: 0x001B2F5C File Offset: 0x001B115C
	public bool IsAllReachable(int cell, CellOffset[] offsets)
	{
		if (this.IsReachable(cell))
		{
			return true;
		}
		foreach (CellOffset offset in offsets)
		{
			if (this.IsReachable(Grid.OffsetCell(cell, offset)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004AFD RID: 19197 RVA: 0x001B2F9E File Offset: 0x001B119E
	public bool IsReachable(Workable workable)
	{
		return this.IsReachable(Grid.PosToCell(workable), workable.GetOffsets());
	}

	// Token: 0x06004AFE RID: 19198 RVA: 0x001B2FB4 File Offset: 0x001B11B4
	public void Occupy(List<int> cells)
	{
		foreach (int num in cells)
		{
			Interlocked.Increment(ref this.cells[num]);
		}
	}

	// Token: 0x06004AFF RID: 19199 RVA: 0x001B3010 File Offset: 0x001B1210
	public void OccupyST(List<int> cells)
	{
		foreach (int num in cells)
		{
			this.cells[num]++;
		}
	}

	// Token: 0x06004B00 RID: 19200 RVA: 0x001B3068 File Offset: 0x001B1268
	public void Occupy(int cell)
	{
		Interlocked.Increment(ref this.cells[cell]);
	}

	// Token: 0x06004B01 RID: 19201 RVA: 0x001B307C File Offset: 0x001B127C
	public void Vacate(List<int> cells)
	{
		foreach (int num in cells)
		{
			Interlocked.Decrement(ref this.cells[num]);
		}
	}

	// Token: 0x06004B02 RID: 19202 RVA: 0x001B30D8 File Offset: 0x001B12D8
	public void VacateST(List<int> cells)
	{
		foreach (int num in cells)
		{
			this.cells[num]--;
		}
	}

	// Token: 0x06004B03 RID: 19203 RVA: 0x001B3130 File Offset: 0x001B1330
	public void Vacate(int cell)
	{
		Interlocked.Decrement(ref this.cells[cell]);
	}

	// Token: 0x040031B4 RID: 12724
	private static MinionGroupProber Instance;

	// Token: 0x040031B5 RID: 12725
	private int[] cells;
}
