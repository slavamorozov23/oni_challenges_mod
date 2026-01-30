using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000794 RID: 1940
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDuplicantSensor : Switch, ISim1000ms, ISim200ms
{
	// Token: 0x060031EF RID: 12783 RVA: 0x0011F4F0 File Offset: 0x0011D6F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x0011F500 File Offset: 0x0011D700
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.RefreshReachableCells();
		this.wasOn = this.switchedOn;
		Vector2I vector2I = Grid.CellToXY(this.NaturalBuildingCell());
		int cell = Grid.XYToCell(vector2I.x, vector2I.y + this.pickupRange / 2);
		CellOffset rotatedCellOffset = new CellOffset(0, this.pickupRange / 2);
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(rotatedCellOffset);
			if (Grid.IsCellOffsetValid(this.NaturalBuildingCell(), rotatedCellOffset))
			{
				cell = Grid.OffsetCell(this.NaturalBuildingCell(), rotatedCellOffset);
			}
		}
		this.pickupableExtents = new Extents(cell, this.pickupRange / 2);
		this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("DuplicantSensor.PickupablesChanged", base.gameObject, this.pickupableExtents, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
		this.pickupablesDirty = true;
	}

	// Token: 0x060031F1 RID: 12785 RVA: 0x0011F602 File Offset: 0x0011D802
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x0011F61A File Offset: 0x0011D81A
	public void Sim1000ms(float dt)
	{
		this.RefreshReachableCells();
	}

	// Token: 0x060031F3 RID: 12787 RVA: 0x0011F622 File Offset: 0x0011D822
	public void Sim200ms(float dt)
	{
		this.RefreshPickupables();
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x0011F62C File Offset: 0x0011D82C
	private void RefreshReachableCells()
	{
		ListPool<int, LogicDuplicantSensor>.PooledList pooledList = ListPool<int, LogicDuplicantSensor>.Allocate(this.reachableCells);
		this.reachableCells.Clear();
		int num;
		int num2;
		Grid.CellToXY(this.NaturalBuildingCell(), out num, out num2);
		int num3 = num - this.pickupRange / 2;
		for (int i = num2; i < num2 + this.pickupRange + 1; i++)
		{
			for (int j = num3; j < num3 + this.pickupRange + 1; j++)
			{
				int num4 = Grid.XYToCell(j, i);
				CellOffset rotatedCellOffset = new CellOffset(j - num, i - num2);
				if (this.rotatable)
				{
					rotatedCellOffset = this.rotatable.GetRotatedCellOffset(rotatedCellOffset);
					if (Grid.IsCellOffsetValid(this.NaturalBuildingCell(), rotatedCellOffset))
					{
						num4 = Grid.OffsetCell(this.NaturalBuildingCell(), rotatedCellOffset);
						Vector2I vector2I = Grid.CellToXY(num4);
						if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, vector2I.x, vector2I.y, true))
						{
							this.reachableCells.Add(num4);
						}
					}
				}
				else if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, j, i, true))
				{
					this.reachableCells.Add(num4);
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x0011F75F File Offset: 0x0011D95F
	public bool IsCellReachable(int cell)
	{
		return this.reachableCells.Contains(cell);
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x0011F770 File Offset: 0x0011D970
	private void RefreshPickupables()
	{
		if (!this.pickupablesDirty)
		{
			return;
		}
		this.duplicants.Clear();
		ListPool<ScenePartitionerEntry, LogicDuplicantSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicDuplicantSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(this.pickupableExtents.x, this.pickupableExtents.y, this.pickupableExtents.width, this.pickupableExtents.height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		int cell_a = Grid.PosToCell(this);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			int pickupableCell = this.GetPickupableCell(pickupable);
			int cellRange = Grid.GetCellRange(cell_a, pickupableCell);
			if (this.IsPickupableRelevantToMyInterestsAndReachable(pickupable) && cellRange <= this.pickupRange)
			{
				this.duplicants.Add(pickupable);
			}
		}
		this.SetState(this.duplicants.Count > 0);
		this.pickupablesDirty = false;
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x0011F850 File Offset: 0x0011DA50
	private void OnPickupablesChanged(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable && this.IsPickupableRelevantToMyInterests(pickupable))
		{
			this.pickupablesDirty = true;
		}
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x0011F87C File Offset: 0x0011DA7C
	private bool IsPickupableRelevantToMyInterests(Pickupable pickupable)
	{
		return pickupable.KPrefabID.HasTag(GameTags.DupeBrain);
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x0011F894 File Offset: 0x0011DA94
	private bool IsPickupableRelevantToMyInterestsAndReachable(Pickupable pickupable)
	{
		if (!this.IsPickupableRelevantToMyInterests(pickupable))
		{
			return false;
		}
		int pickupableCell = this.GetPickupableCell(pickupable);
		return this.IsCellReachable(pickupableCell);
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x0011F8C0 File Offset: 0x0011DAC0
	private int GetPickupableCell(Pickupable pickupable)
	{
		return pickupable.cachedCell;
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x0011F8C8 File Offset: 0x0011DAC8
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x0011F8D7 File Offset: 0x0011DAD7
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060031FD RID: 12797 RVA: 0x0011F8F8 File Offset: 0x0011DAF8
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x0011F980 File Offset: 0x0011DB80
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001E09 RID: 7689
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04001E0A RID: 7690
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001E0B RID: 7691
	public int pickupRange = 4;

	// Token: 0x04001E0C RID: 7692
	private bool wasOn;

	// Token: 0x04001E0D RID: 7693
	private List<Pickupable> duplicants = new List<Pickupable>();

	// Token: 0x04001E0E RID: 7694
	private HandleVector<int>.Handle pickupablesChangedEntry;

	// Token: 0x04001E0F RID: 7695
	private bool pickupablesDirty;

	// Token: 0x04001E10 RID: 7696
	private Extents pickupableExtents;

	// Token: 0x04001E11 RID: 7697
	private List<int> reachableCells = new List<int>(100);
}
