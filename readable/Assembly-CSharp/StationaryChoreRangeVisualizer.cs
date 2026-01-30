using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200063F RID: 1599
[AddComponentMenu("KMonoBehaviour/scripts/StationaryChoreRangeVisualizer")]
[Obsolete("Deprecated, use RangeVisualizer")]
public class StationaryChoreRangeVisualizer : KMonoBehaviour
{
	// Token: 0x06002648 RID: 9800 RVA: 0x000DC390 File Offset: 0x000DA590
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate);
		if (this.movable)
		{
			this.cellChangeMonitorHandle = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, StationaryChoreRangeVisualizer.OnCellChangeDispatcher, this, "StationaryChoreRangeVisualizer.OnSpawn");
			base.Subscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate);
		}
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x000DC3F0 File Offset: 0x000DA5F0
	protected override void OnCleanUp()
	{
		if (this.cellChangeMonitorHandle != 0UL)
		{
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeMonitorHandle);
		}
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate, false);
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate, false);
		this.ClearVisualizers();
		base.OnCleanUp();
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x000DC444 File Offset: 0x000DA644
	private void OnSelect(object data)
	{
		if (((Boxed<bool>)data).value)
		{
			SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_form", false), base.transform.position, 1f);
			this.UpdateVisualizers();
			return;
		}
		SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_disappear", false), base.transform.position, 1f);
		this.ClearVisualizers();
	}

	// Token: 0x0600264B RID: 9803 RVA: 0x000DC4AD File Offset: 0x000DA6AD
	private void OnRotated(object data)
	{
		this.UpdateVisualizers();
	}

	// Token: 0x0600264C RID: 9804 RVA: 0x000DC4B8 File Offset: 0x000DA6B8
	private void UpdateVisualizers()
	{
		this.newCells.Clear();
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int num;
		int num2;
		Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out num, out num2);
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num3 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num3))
				{
					int x;
					int y;
					Grid.CellToXY(num3, out x, out y);
					if (Grid.TestLineOfSight(num, num2, x, y, this.blocking_cb, this.blocking_tile_visible, false))
					{
						this.newCells.Add(num3);
					}
				}
			}
		}
		for (int k = this.visualizers.Count - 1; k >= 0; k--)
		{
			if (this.newCells.Contains(this.visualizers[k].cell))
			{
				this.newCells.Remove(this.visualizers[k].cell);
			}
			else
			{
				this.DestroyEffect(this.visualizers[k].controller);
				this.visualizers.RemoveAt(k);
			}
		}
		for (int l = 0; l < this.newCells.Count; l++)
		{
			KBatchedAnimController controller = this.CreateEffect(this.newCells[l]);
			this.visualizers.Add(new StationaryChoreRangeVisualizer.VisData
			{
				cell = this.newCells[l],
				controller = controller
			});
		}
	}

	// Token: 0x0600264D RID: 9805 RVA: 0x000DC6A8 File Offset: 0x000DA8A8
	private void ClearVisualizers()
	{
		for (int i = 0; i < this.visualizers.Count; i++)
		{
			this.DestroyEffect(this.visualizers[i].controller);
		}
		this.visualizers.Clear();
	}

	// Token: 0x0600264E RID: 9806 RVA: 0x000DC6F0 File Offset: 0x000DA8F0
	private KBatchedAnimController CreateEffect(int cell)
	{
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(StationaryChoreRangeVisualizer.AnimName, Grid.CellToPosCCC(cell, this.sceneLayer), null, false, this.sceneLayer, true);
		kbatchedAnimController.destroyOnAnimComplete = false;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.Play(StationaryChoreRangeVisualizer.PreAnims, KAnim.PlayMode.Loop);
		return kbatchedAnimController;
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x000DC742 File Offset: 0x000DA942
	private void DestroyEffect(KBatchedAnimController controller)
	{
		controller.destroyOnAnimComplete = true;
		controller.Play(StationaryChoreRangeVisualizer.PostAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001695 RID: 5781
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001696 RID: 5782
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001697 RID: 5783
	public int x;

	// Token: 0x04001698 RID: 5784
	public int y;

	// Token: 0x04001699 RID: 5785
	public int width;

	// Token: 0x0400169A RID: 5786
	public int height;

	// Token: 0x0400169B RID: 5787
	public bool movable;

	// Token: 0x0400169C RID: 5788
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.FXFront;

	// Token: 0x0400169D RID: 5789
	public CellOffset vision_offset;

	// Token: 0x0400169E RID: 5790
	public Func<int, bool> blocking_cb = new Func<int, bool>(Grid.PhysicalBlockingCB);

	// Token: 0x0400169F RID: 5791
	public bool blocking_tile_visible = true;

	// Token: 0x040016A0 RID: 5792
	private static readonly string AnimName = "transferarmgrid_kanim";

	// Token: 0x040016A1 RID: 5793
	private static readonly HashedString[] PreAnims = new HashedString[]
	{
		"grid_pre",
		"grid_loop"
	};

	// Token: 0x040016A2 RID: 5794
	private static readonly HashedString PostAnim = "grid_pst";

	// Token: 0x040016A3 RID: 5795
	private List<StationaryChoreRangeVisualizer.VisData> visualizers = new List<StationaryChoreRangeVisualizer.VisData>();

	// Token: 0x040016A4 RID: 5796
	private List<int> newCells = new List<int>();

	// Token: 0x040016A5 RID: 5797
	private ulong cellChangeMonitorHandle;

	// Token: 0x040016A6 RID: 5798
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnSelectDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnSelect(data);
	});

	// Token: 0x040016A7 RID: 5799
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnRotated(data);
	});

	// Token: 0x040016A8 RID: 5800
	private static readonly Action<object> OnCellChangeDispatcher = delegate(object obj)
	{
		Unsafe.As<StationaryChoreRangeVisualizer>(obj).UpdateVisualizers();
	};

	// Token: 0x02001519 RID: 5401
	private struct VisData
	{
		// Token: 0x040070AE RID: 28846
		public int cell;

		// Token: 0x040070AF RID: 28847
		public KBatchedAnimController controller;
	}
}
