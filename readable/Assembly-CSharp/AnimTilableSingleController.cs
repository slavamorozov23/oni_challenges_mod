using System;
using UnityEngine;

// Token: 0x020006D0 RID: 1744
public class AnimTilableSingleController : KMonoBehaviour
{
	// Token: 0x06002AA8 RID: 10920 RVA: 0x000F9A5A File Offset: 0x000F7C5A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.tagsOfNeightboursThatICanTileWith == null || this.tagsOfNeightboursThatICanTileWith.Length == 0)
		{
			this.tagsOfNeightboursThatICanTileWith = new Tag[]
			{
				base.GetComponent<KPrefabID>().PrefabTag
			};
		}
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x000F9A94 File Offset: 0x000F7C94
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		else
		{
			Building component2 = base.GetComponent<Building>();
			this.extents = component2.GetExtents();
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y - 1, this.extents.width + 2, this.extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileableSingleController.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		base.GetComponent<KBatchedAnimController>();
		this.RefreshAnim();
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x000F9B53 File Offset: 0x000F7D53
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x000F9B6C File Offset: 0x000F7D6C
	private void RefreshAnim()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.RefreshAnimCallback == null)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		bool arg = true;
		bool arg2 = true;
		bool arg3 = true;
		bool arg4 = true;
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset rotatedCellOffset = new CellOffset(this.extents.x - num - 1, 0);
		CellOffset rotatedCellOffset2 = new CellOffset(this.extents.x - num + this.extents.width, 0);
		CellOffset rotatedCellOffset3 = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		CellOffset rotatedCellOffset4 = new CellOffset(0, this.extents.y - num2 - 1);
		Rotatable component2 = base.GetComponent<Rotatable>();
		if (component2)
		{
			rotatedCellOffset = component2.GetRotatedCellOffset(rotatedCellOffset);
			rotatedCellOffset2 = component2.GetRotatedCellOffset(rotatedCellOffset2);
			rotatedCellOffset3 = component2.GetRotatedCellOffset(rotatedCellOffset3);
			rotatedCellOffset4 = component2.GetRotatedCellOffset(rotatedCellOffset4);
		}
		int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num4 = Grid.OffsetCell(cell, rotatedCellOffset2);
		int num5 = Grid.OffsetCell(cell, rotatedCellOffset3);
		int num6 = Grid.OffsetCell(cell, rotatedCellOffset4);
		if (Grid.IsValidCell(num3))
		{
			arg = this.HasTileableNeighbour(num3);
		}
		if (Grid.IsValidCell(num4))
		{
			arg2 = this.HasTileableNeighbour(num4);
		}
		if (Grid.IsValidCell(num5))
		{
			arg3 = this.HasTileableNeighbour(num5);
		}
		if (Grid.IsValidCell(num6))
		{
			arg4 = this.HasTileableNeighbour(num6);
		}
		this.RefreshAnimCallback(component, arg3, arg2, arg4, arg);
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x000F9CD4 File Offset: 0x000F7ED4
	private bool HasTileableNeighbour(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.HasAnyTags(this.tagsOfNeightboursThatICanTileWith))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x000F9D1F File Offset: 0x000F7F1F
	private void OnNeighbourCellsUpdated(object data)
	{
		if (this.partitionerEntry.IsValid())
		{
			this.RefreshAnim();
		}
	}

	// Token: 0x04001967 RID: 6503
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001968 RID: 6504
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04001969 RID: 6505
	public Tag[] tagsOfNeightboursThatICanTileWith;

	// Token: 0x0400196A RID: 6506
	private Extents extents;

	// Token: 0x0400196B RID: 6507
	public Action<KBatchedAnimController, bool, bool, bool, bool> RefreshAnimCallback;
}
