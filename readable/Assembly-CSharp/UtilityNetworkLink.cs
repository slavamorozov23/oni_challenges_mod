using System;
using UnityEngine;

// Token: 0x02000C0B RID: 3083
public abstract class UtilityNetworkLink : KMonoBehaviour
{
	// Token: 0x06005C9A RID: 23706 RVA: 0x00218AD6 File Offset: 0x00216CD6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate);
		base.Subscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate);
		this.Connect();
	}

	// Token: 0x06005C9B RID: 23707 RVA: 0x00218B06 File Offset: 0x00216D06
	protected override void OnCleanUp()
	{
		base.Unsubscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate, false);
		this.Disconnect();
		base.OnCleanUp();
	}

	// Token: 0x06005C9C RID: 23708 RVA: 0x00218B38 File Offset: 0x00216D38
	protected void Connect()
	{
		if (!this.visualizeOnly && !this.connected)
		{
			this.connected = true;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnConnect(cell, cell2);
		}
	}

	// Token: 0x06005C9D RID: 23709 RVA: 0x00218B6E File Offset: 0x00216D6E
	protected virtual void OnConnect(int cell1, int cell2)
	{
	}

	// Token: 0x06005C9E RID: 23710 RVA: 0x00218B70 File Offset: 0x00216D70
	protected void Disconnect()
	{
		if (!this.visualizeOnly && this.connected)
		{
			this.connected = false;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnDisconnect(cell, cell2);
		}
	}

	// Token: 0x06005C9F RID: 23711 RVA: 0x00218BA6 File Offset: 0x00216DA6
	protected virtual void OnDisconnect(int cell1, int cell2)
	{
	}

	// Token: 0x06005CA0 RID: 23712 RVA: 0x00218BA8 File Offset: 0x00216DA8
	public void GetCells(out int linked_cell1, out int linked_cell2)
	{
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			Orientation orientation = component.Orientation;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.GetCells(cell, orientation, out linked_cell1, out linked_cell2);
			return;
		}
		linked_cell1 = -1;
		linked_cell2 = -1;
	}

	// Token: 0x06005CA1 RID: 23713 RVA: 0x00218BF0 File Offset: 0x00216DF0
	public void GetCells(int cell, Orientation orientation, out int linked_cell1, out int linked_cell2)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		linked_cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
		linked_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
	}

	// Token: 0x06005CA2 RID: 23714 RVA: 0x00218C2C File Offset: 0x00216E2C
	public bool AreCellsValid(int cell, Orientation orientation)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		return Grid.IsCellOffsetValid(cell, rotatedCellOffset) && Grid.IsCellOffsetValid(cell, rotatedCellOffset2);
	}

	// Token: 0x06005CA3 RID: 23715 RVA: 0x00218C65 File Offset: 0x00216E65
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06005CA4 RID: 23716 RVA: 0x00218C6D File Offset: 0x00216E6D
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06005CA5 RID: 23717 RVA: 0x00218C78 File Offset: 0x00216E78
	public int GetNetworkCell()
	{
		int result;
		int num;
		this.GetCells(out result, out num);
		return result;
	}

	// Token: 0x04003DB1 RID: 15793
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04003DB2 RID: 15794
	[SerializeField]
	public CellOffset link1;

	// Token: 0x04003DB3 RID: 15795
	[SerializeField]
	public CellOffset link2;

	// Token: 0x04003DB4 RID: 15796
	[SerializeField]
	public bool visualizeOnly;

	// Token: 0x04003DB5 RID: 15797
	private bool connected;

	// Token: 0x04003DB6 RID: 15798
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04003DB7 RID: 15799
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
