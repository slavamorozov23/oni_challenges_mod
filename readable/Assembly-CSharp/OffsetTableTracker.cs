using System;

// Token: 0x02000A7A RID: 2682
public class OffsetTableTracker : OffsetTracker
{
	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x06004E02 RID: 19970 RVA: 0x001C6201 File Offset: 0x001C4401
	private static NavGrid navGrid
	{
		get
		{
			if (OffsetTableTracker.navGridImpl == null)
			{
				OffsetTableTracker.navGridImpl = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
			}
			return OffsetTableTracker.navGridImpl;
		}
	}

	// Token: 0x06004E03 RID: 19971 RVA: 0x001C6223 File Offset: 0x001C4423
	public OffsetTableTracker(CellOffset[][] table, KMonoBehaviour cmp)
	{
		this.table = table;
		this.cmp = cmp;
		this.OnCellChangedClosure = new Action<object>(this.OnCellChanged);
	}

	// Token: 0x06004E04 RID: 19972 RVA: 0x001C624C File Offset: 0x001C444C
	protected override void UpdateCell(int previous_cell, int current_cell)
	{
		if (previous_cell == current_cell)
		{
			return;
		}
		base.UpdateCell(previous_cell, current_cell);
		Extents extents = new Extents(current_cell, this.table);
		extents.height += 2;
		extents.y--;
		if (!this.solidPartitionerEntry.IsValid())
		{
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, this.OnCellChangedClosure);
			this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, this.OnCellChangedClosure);
		}
		else
		{
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, extents);
			GameScenePartitioner.Instance.UpdatePosition(this.validNavCellChangedPartitionerEntry, extents);
		}
		this.offsets = null;
	}

	// Token: 0x06004E05 RID: 19973 RVA: 0x001C6328 File Offset: 0x001C4528
	private static bool IsValidRow(int current_cell, CellOffset[] row, int rowIdx, int[] debugIdxs)
	{
		for (int i = 1; i < row.Length; i++)
		{
			int num = Grid.OffsetCell(current_cell, row[i]);
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (Grid.Solid[num])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004E06 RID: 19974 RVA: 0x001C636C File Offset: 0x001C456C
	private void UpdateOffsets(int cell, CellOffset[][] table)
	{
		HashSetPool<CellOffset, OffsetTableTracker>.PooledHashSet pooledHashSet = HashSetPool<CellOffset, OffsetTableTracker>.Allocate();
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < table.Length; i++)
			{
				CellOffset[] array = table[i];
				if (!pooledHashSet.Contains(array[0]))
				{
					int cell2 = Grid.OffsetCell(cell, array[0]);
					for (int j = 0; j < OffsetTableTracker.navGrid.ValidNavTypes.Length; j++)
					{
						NavType navType = OffsetTableTracker.navGrid.ValidNavTypes[j];
						if (navType != NavType.Tube && OffsetTableTracker.navGrid.NavTable.IsValid(cell2, navType) && OffsetTableTracker.IsValidRow(cell, array, i, this.DEBUG_rowValidIdx))
						{
							pooledHashSet.Add(array[0]);
							break;
						}
					}
				}
			}
		}
		if (this.offsets == null || this.offsets.Length != pooledHashSet.Count)
		{
			this.offsets = new CellOffset[pooledHashSet.Count];
		}
		pooledHashSet.CopyTo(this.offsets);
		pooledHashSet.Recycle();
	}

	// Token: 0x06004E07 RID: 19975 RVA: 0x001C645D File Offset: 0x001C465D
	protected override void UpdateOffsets(int current_cell)
	{
		base.UpdateOffsets(current_cell);
		this.UpdateOffsets(current_cell, this.table);
	}

	// Token: 0x06004E08 RID: 19976 RVA: 0x001C6473 File Offset: 0x001C4673
	private void OnCellChanged(object data)
	{
		this.offsets = null;
	}

	// Token: 0x06004E09 RID: 19977 RVA: 0x001C647C File Offset: 0x001C467C
	public override void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
	}

	// Token: 0x06004E0A RID: 19978 RVA: 0x001C649E File Offset: 0x001C469E
	public static void OnPathfindingInvalidated()
	{
		OffsetTableTracker.navGridImpl = null;
	}

	// Token: 0x040033FB RID: 13307
	private readonly CellOffset[][] table;

	// Token: 0x040033FC RID: 13308
	public HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x040033FD RID: 13309
	public HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x040033FE RID: 13310
	private static NavGrid navGridImpl;

	// Token: 0x040033FF RID: 13311
	private KMonoBehaviour cmp;

	// Token: 0x04003400 RID: 13312
	private Action<object> OnCellChangedClosure;

	// Token: 0x04003401 RID: 13313
	private int[] DEBUG_rowValidIdx;
}
