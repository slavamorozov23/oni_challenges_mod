using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000B4F RID: 2895
public readonly struct SkyVisibilityInfo
{
	// Token: 0x06005581 RID: 21889 RVA: 0x001F3394 File Offset: 0x001F1594
	public SkyVisibilityInfo(CellOffset scanLeftOffset, int scanLeftCount, CellOffset scanRightOffset, int scanRightCount, int verticalStep)
	{
		this.scanLeftOffset = scanLeftOffset;
		this.scanLeftCount = scanLeftCount;
		this.scanRightOffset = scanRightOffset;
		this.scanRightCount = scanRightCount;
		this.verticalStep = verticalStep;
		this.totalColumnsCount = scanLeftCount + scanRightCount + (scanRightOffset.x - scanLeftOffset.x + 1);
	}

	// Token: 0x06005582 RID: 21890 RVA: 0x001F33E0 File Offset: 0x001F15E0
	[return: TupleElementNames(new string[]
	{
		"isAnyVisible",
		"percentVisible01"
	})]
	public ValueTuple<bool, float> GetVisibilityOf(GameObject gameObject)
	{
		return this.GetVisibilityOf(Grid.PosToCell(gameObject));
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x001F33F0 File Offset: 0x001F15F0
	[return: TupleElementNames(new string[]
	{
		"isAnyVisible",
		"percentVisible01"
	})]
	public ValueTuple<bool, float> GetVisibilityOf(int buildingCenterCellId)
	{
		int num = 0;
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[buildingCenterCellId]);
		num += SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, world);
		num += SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, world);
		if (this.scanLeftOffset.x == this.scanRightOffset.x)
		{
			num = Mathf.Max(0, num - 1);
		}
		return new ValueTuple<bool, float>(num > 0, (float)num / (float)this.totalColumnsCount);
	}

	// Token: 0x06005584 RID: 21892 RVA: 0x001F348C File Offset: 0x001F168C
	public void CollectVisibleCellsTo(HashSet<int> visibleCells, int buildingBottomLeftCellId, WorldContainer originWorld)
	{
		SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, originWorld);
		SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, originWorld);
	}

	// Token: 0x06005585 RID: 21893 RVA: 0x001F34DC File Offset: 0x001F16DC
	private static void ScanAndCollectVisibleCellsTo(HashSet<int> visibleCells, int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			int num = Grid.OffsetCell(originCellId, i * stepX, i * stepY);
			if (!SkyVisibilityInfo.IsVisible(num, originWorld))
			{
				break;
			}
			visibleCells.Add(num);
		}
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x001F3518 File Offset: 0x001F1718
	private static int ScanAndGetVisibleCellCount(int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			if (!SkyVisibilityInfo.IsVisible(Grid.OffsetCell(originCellId, i * stepX, i * stepY), originWorld))
			{
				return i;
			}
		}
		return stepCountInclusive + 1;
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x001F354C File Offset: 0x001F174C
	public static bool IsVisible(int cellId, WorldContainer originWorld)
	{
		if (!Grid.IsValidCell(cellId))
		{
			return false;
		}
		if (Grid.ExposedToSunlight[cellId] > 0)
		{
			return true;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cellId]);
		if (world != null && world.IsModuleInterior)
		{
			return true;
		}
		originWorld != world;
		return false;
	}

	// Token: 0x040039B4 RID: 14772
	public readonly CellOffset scanLeftOffset;

	// Token: 0x040039B5 RID: 14773
	public readonly int scanLeftCount;

	// Token: 0x040039B6 RID: 14774
	public readonly CellOffset scanRightOffset;

	// Token: 0x040039B7 RID: 14775
	public readonly int scanRightCount;

	// Token: 0x040039B8 RID: 14776
	public readonly int verticalStep;

	// Token: 0x040039B9 RID: 14777
	public readonly int totalColumnsCount;
}
