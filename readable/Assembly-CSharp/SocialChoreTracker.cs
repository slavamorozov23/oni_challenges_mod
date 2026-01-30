using System;
using UnityEngine;

// Token: 0x02000A7C RID: 2684
public class SocialChoreTracker
{
	// Token: 0x06004E16 RID: 19990 RVA: 0x001C65E8 File Offset: 0x001C47E8
	public SocialChoreTracker(GameObject owner, CellOffset[] chore_offsets)
	{
		this.owner = owner;
		this.choreOffsets = chore_offsets;
		this.chores = new Chore[this.choreOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(owner), this.choreOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("PrintingPodSocialize", owner, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
	}

	// Token: 0x06004E17 RID: 19991 RVA: 0x001C665C File Offset: 0x001C485C
	public void Update(bool update = true)
	{
		if (this.updating)
		{
			return;
		}
		this.updating = true;
		int num = 0;
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			CellOffset offset = this.choreOffsets[i];
			Chore chore = this.chores[i];
			if (update && num < this.choreCount && this.IsOffsetValid(offset))
			{
				num++;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = ((this.CreateChoreCB != null) ? this.CreateChoreCB(i) : null);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
		this.updating = false;
	}

	// Token: 0x06004E18 RID: 19992 RVA: 0x001C670D File Offset: 0x001C490D
	private void OnCellChanged(object data)
	{
		if (this.owner.HasTag(GameTags.Operational))
		{
			this.Update(true);
		}
	}

	// Token: 0x06004E19 RID: 19993 RVA: 0x001C6728 File Offset: 0x001C4928
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.Update(false);
	}

	// Token: 0x06004E1A RID: 19994 RVA: 0x001C6744 File Offset: 0x001C4944
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this.owner), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, true);
	}

	// Token: 0x04003405 RID: 13317
	public Func<int, Chore> CreateChoreCB;

	// Token: 0x04003406 RID: 13318
	public int choreCount;

	// Token: 0x04003407 RID: 13319
	private GameObject owner;

	// Token: 0x04003408 RID: 13320
	private CellOffset[] choreOffsets;

	// Token: 0x04003409 RID: 13321
	private Chore[] chores;

	// Token: 0x0400340A RID: 13322
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x0400340B RID: 13323
	private bool updating;
}
