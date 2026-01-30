using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000959 RID: 2393
[AddComponentMenu("KMonoBehaviour/scripts/FloorSwitchActivator")]
public class FloorSwitchActivator : KMonoBehaviour
{
	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x060042E0 RID: 17120 RVA: 0x0017A22F File Offset: 0x0017842F
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x0017A237 File Offset: 0x00178437
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x0017A24B File Offset: 0x0017844B
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x060042E3 RID: 17123 RVA: 0x0017A25C File Offset: 0x0017845C
	private void OnCellChange()
	{
		int num = Grid.PosToCell(this);
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, num);
		if (Grid.IsValidCell(this.last_cell_occupied) && num != this.last_cell_occupied)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.NotifyChanged(num);
		this.last_cell_occupied = num;
	}

	// Token: 0x060042E4 RID: 17124 RVA: 0x0017A2B1 File Offset: 0x001784B1
	private void NotifyChanged(int cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, this);
	}

	// Token: 0x060042E5 RID: 17125 RVA: 0x0017A2C9 File Offset: 0x001784C9
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x060042E6 RID: 17126 RVA: 0x0017A2D7 File Offset: 0x001784D7
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x060042E7 RID: 17127 RVA: 0x0017A2E8 File Offset: 0x001784E8
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("FloorSwitchActivator.Register", this, cell, GameScenePartitioner.Instance.floorSwitchActivatorLayer, null);
		this.cellChangeHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, FloorSwitchActivator.OnCellChangeDispatcher, this, "FloorSwitchActivator.Register");
		this.registered = true;
	}

	// Token: 0x060042E8 RID: 17128 RVA: 0x0017A350 File Offset: 0x00178550
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeHandlerID);
		if (this.last_cell_occupied > -1)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.registered = false;
	}

	// Token: 0x04002A0C RID: 10764
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04002A0D RID: 10765
	private bool registered;

	// Token: 0x04002A0E RID: 10766
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002A0F RID: 10767
	private int last_cell_occupied = -1;

	// Token: 0x04002A10 RID: 10768
	private ulong cellChangeHandlerID;

	// Token: 0x04002A11 RID: 10769
	private static readonly Action<object> OnCellChangeDispatcher = delegate(object obj)
	{
		Unsafe.As<FloorSwitchActivator>(obj).OnCellChange();
	};
}
