using System;

// Token: 0x0200094D RID: 2381
public class FakeFloorAdder : KMonoBehaviour
{
	// Token: 0x06004251 RID: 16977 RVA: 0x00175FB0 File Offset: 0x001741B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.initiallyActive)
		{
			this.SetFloor(true);
		}
	}

	// Token: 0x06004252 RID: 16978 RVA: 0x00175FC8 File Offset: 0x001741C8
	public void SetFloor(bool active)
	{
		if (this.isActive == active)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		Rotatable component = base.GetComponent<Rotatable>();
		foreach (CellOffset cellOffset in this.floorOffsets)
		{
			CellOffset offset = (component == null) ? cellOffset : component.GetRotatedCellOffset(cellOffset);
			int num = Grid.OffsetCell(cell, offset);
			if (active)
			{
				Grid.FakeFloor.Add(num);
			}
			else
			{
				Grid.FakeFloor.Remove(num);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
		}
		this.isActive = active;
	}

	// Token: 0x06004253 RID: 16979 RVA: 0x0017605C File Offset: 0x0017425C
	protected override void OnCleanUp()
	{
		this.SetFloor(false);
		base.OnCleanUp();
	}

	// Token: 0x040029A7 RID: 10663
	public CellOffset[] floorOffsets;

	// Token: 0x040029A8 RID: 10664
	public bool initiallyActive = true;

	// Token: 0x040029A9 RID: 10665
	private bool isActive;
}
