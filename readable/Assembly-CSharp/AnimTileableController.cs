using System;
using UnityEngine;

// Token: 0x020006D2 RID: 1746
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/AnimTileableController")]
public class AnimTileableController : KMonoBehaviour
{
	// Token: 0x06002AB7 RID: 10935 RVA: 0x000FA211 File Offset: 0x000F8411
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				base.GetComponent<KPrefabID>().PrefabTag
			};
		}
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x000FA248 File Offset: 0x000F8448
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
		this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		KBatchedAnimController component3 = base.GetComponent<KBatchedAnimController>();
		this.left = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.leftName);
		this.right = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.rightName);
		this.top = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.topName);
		this.bottom = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.bottomName);
		this.UpdateEndCaps();
	}

	// Token: 0x06002AB9 RID: 10937 RVA: 0x000FA367 File Offset: 0x000F8567
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002ABA RID: 10938 RVA: 0x000FA380 File Offset: 0x000F8580
	private void UpdateEndCaps()
	{
		int cell = Grid.PosToCell(this);
		bool enable = true;
		bool enable2 = true;
		bool enable3 = true;
		bool enable4 = true;
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset rotatedCellOffset = new CellOffset(this.extents.x - num - 1, 0);
		CellOffset rotatedCellOffset2 = new CellOffset(this.extents.x - num + this.extents.width, 0);
		CellOffset rotatedCellOffset3 = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		CellOffset rotatedCellOffset4 = new CellOffset(0, this.extents.y - num2 - 1);
		Rotatable component = base.GetComponent<Rotatable>();
		if (component)
		{
			rotatedCellOffset = component.GetRotatedCellOffset(rotatedCellOffset);
			rotatedCellOffset2 = component.GetRotatedCellOffset(rotatedCellOffset2);
			rotatedCellOffset3 = component.GetRotatedCellOffset(rotatedCellOffset3);
			rotatedCellOffset4 = component.GetRotatedCellOffset(rotatedCellOffset4);
		}
		int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num4 = Grid.OffsetCell(cell, rotatedCellOffset2);
		int num5 = Grid.OffsetCell(cell, rotatedCellOffset3);
		int num6 = Grid.OffsetCell(cell, rotatedCellOffset4);
		if (Grid.IsValidCell(num3))
		{
			enable = !this.HasTileableNeighbour(num3);
		}
		if (Grid.IsValidCell(num4))
		{
			enable2 = !this.HasTileableNeighbour(num4);
		}
		if (Grid.IsValidCell(num5))
		{
			enable3 = !this.HasTileableNeighbour(num5);
		}
		if (Grid.IsValidCell(num6))
		{
			enable4 = !this.HasTileableNeighbour(num6);
		}
		this.left.Enable(enable);
		this.right.Enable(enable2);
		this.top.Enable(enable3);
		this.bottom.Enable(enable4);
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x000FA504 File Offset: 0x000F8704
	private bool HasTileableNeighbour(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.HasAnyTags(this.tags))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002ABC RID: 10940 RVA: 0x000FA54F File Offset: 0x000F874F
	private void OnNeighbourCellsUpdated(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		if (this.partitionerEntry.IsValid())
		{
			this.UpdateEndCaps();
		}
	}

	// Token: 0x04001974 RID: 6516
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001975 RID: 6517
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04001976 RID: 6518
	public Tag[] tags;

	// Token: 0x04001977 RID: 6519
	private Extents extents;

	// Token: 0x04001978 RID: 6520
	public string leftName = "#cap_left";

	// Token: 0x04001979 RID: 6521
	public string rightName = "#cap_right";

	// Token: 0x0400197A RID: 6522
	public string topName = "#cap_top";

	// Token: 0x0400197B RID: 6523
	public string bottomName = "#cap_bottom";

	// Token: 0x0400197C RID: 6524
	private KAnimSynchronizedController left;

	// Token: 0x0400197D RID: 6525
	private KAnimSynchronizedController right;

	// Token: 0x0400197E RID: 6526
	private KAnimSynchronizedController top;

	// Token: 0x0400197F RID: 6527
	private KAnimSynchronizedController bottom;
}
