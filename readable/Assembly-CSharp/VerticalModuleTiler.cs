using System;
using UnityEngine;

// Token: 0x02000459 RID: 1113
public class VerticalModuleTiler : KMonoBehaviour
{
	// Token: 0x0600173A RID: 5946 RVA: 0x00083F1C File Offset: 0x0008211C
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		if (this.manageTopCap)
		{
			this.topCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.topCapStr);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.bottomCapStr);
		}
		this.PostReorderMove();
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x00083F90 File Offset: 0x00082190
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x00083FA8 File Offset: 0x000821A8
	public void PostReorderMove()
	{
		this.dirty = true;
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x00083FB1 File Offset: 0x000821B1
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

	// Token: 0x0600173E RID: 5950 RVA: 0x00083FE0 File Offset: 0x000821E0
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellTop = this.GetCellTop();
		int cellBottom = this.GetCellBottom();
		if (Grid.IsValidCell(cellTop))
		{
			if (this.HasWideNeighbor(cellTop))
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (Grid.IsValidCell(cellBottom))
		{
			if (this.HasWideNeighbor(cellBottom))
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (this.manageTopCap)
		{
			this.topCapWide.Enable(this.topCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide.Enable(this.bottomCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
	}

	// Token: 0x0600173F RID: 5951 RVA: 0x00084084 File Offset: 0x00082284
	private int GetCellTop()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x000840C8 File Offset: 0x000822C8
	private int GetCellBottom()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 - 1);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x00084104 File Offset: 0x00082304
	private bool HasWideNeighbor(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.GetComponent<ReorderableBuilding>() != null && component.GetComponent<Building>().Def.WidthInCells >= 5)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x00084164 File Offset: 0x00082364
	private void LateUpdate()
	{
		if (this.animController.Offset != this.m_previousAnimControllerOffset)
		{
			this.m_previousAnimControllerOffset = this.animController.Offset;
			this.bottomCapWide.Dirty();
			this.topCapWide.Dirty();
		}
		if (this.dirty)
		{
			if (this.partitionerEntry != HandleVector<int>.InvalidHandle)
			{
				GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			}
			OccupyArea component = base.GetComponent<OccupyArea>();
			if (component != null)
			{
				this.extents = component.GetExtents();
			}
			Extents extents = new Extents(this.extents.x, this.extents.y - 1, this.extents.width, this.extents.height + 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("VerticalModuleTiler.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
			this.UpdateEndCaps();
			this.dirty = false;
		}
	}

	// Token: 0x04000DAA RID: 3498
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04000DAB RID: 3499
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04000DAC RID: 3500
	private Extents extents;

	// Token: 0x04000DAD RID: 3501
	private VerticalModuleTiler.AnimCapType topCapSetting;

	// Token: 0x04000DAE RID: 3502
	private VerticalModuleTiler.AnimCapType bottomCapSetting;

	// Token: 0x04000DAF RID: 3503
	private bool manageTopCap = true;

	// Token: 0x04000DB0 RID: 3504
	private bool manageBottomCap = true;

	// Token: 0x04000DB1 RID: 3505
	private KAnimSynchronizedController topCapWide;

	// Token: 0x04000DB2 RID: 3506
	private KAnimSynchronizedController bottomCapWide;

	// Token: 0x04000DB3 RID: 3507
	private static readonly string topCapStr = "#cap_top_5";

	// Token: 0x04000DB4 RID: 3508
	private static readonly string bottomCapStr = "#cap_bottom_5";

	// Token: 0x04000DB5 RID: 3509
	private bool dirty;

	// Token: 0x04000DB6 RID: 3510
	[MyCmpGet]
	private KAnimControllerBase animController;

	// Token: 0x04000DB7 RID: 3511
	private Vector3 m_previousAnimControllerOffset;

	// Token: 0x0200127F RID: 4735
	private enum AnimCapType
	{
		// Token: 0x0400680B RID: 26635
		ThreeWide,
		// Token: 0x0400680C RID: 26636
		FiveWide
	}
}
