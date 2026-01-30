using System;
using UnityEngine;

// Token: 0x0200060C RID: 1548
[AddComponentMenu("KMonoBehaviour/scripts/AntiCluster")]
public class MoverLayerOccupier : KMonoBehaviour, ISim200ms
{
	// Token: 0x0600243C RID: 9276 RVA: 0x000D1860 File Offset: 0x000CFA60
	private void RefreshCellOccupy()
	{
		int cell = Grid.PosToCell(this);
		foreach (CellOffset offset in this.cellOffsets)
		{
			int current_cell = Grid.OffsetCell(cell, offset);
			if (this.previousCell != Grid.InvalidCell)
			{
				int previous_cell = Grid.OffsetCell(this.previousCell, offset);
				this.UpdateCell(previous_cell, current_cell);
			}
			else
			{
				this.UpdateCell(this.previousCell, current_cell);
			}
		}
		this.previousCell = cell;
	}

	// Token: 0x0600243D RID: 9277 RVA: 0x000D18D6 File Offset: 0x000CFAD6
	public void Sim200ms(float dt)
	{
		this.RefreshCellOccupy();
	}

	// Token: 0x0600243E RID: 9278 RVA: 0x000D18E0 File Offset: 0x000CFAE0
	private void UpdateCell(int previous_cell, int current_cell)
	{
		foreach (ObjectLayer layer in this.objectLayers)
		{
			if (previous_cell != Grid.InvalidCell && previous_cell != current_cell && Grid.Objects[previous_cell, (int)layer] == base.gameObject)
			{
				Grid.Objects[previous_cell, (int)layer] = null;
			}
			GameObject gameObject = Grid.Objects[current_cell, (int)layer];
			if (gameObject == null)
			{
				Grid.Objects[current_cell, (int)layer] = base.gameObject;
			}
			else
			{
				KPrefabID component = base.GetComponent<KPrefabID>();
				KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
				if (component.InstanceID > component2.InstanceID)
				{
					Grid.Objects[current_cell, (int)layer] = base.gameObject;
				}
			}
		}
	}

	// Token: 0x0600243F RID: 9279 RVA: 0x000D1998 File Offset: 0x000CFB98
	private void CleanUpOccupiedCells()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		foreach (CellOffset offset in this.cellOffsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			foreach (ObjectLayer layer in this.objectLayers)
			{
				if (Grid.Objects[cell2, (int)layer] == base.gameObject)
				{
					Grid.Objects[cell2, (int)layer] = null;
				}
			}
		}
	}

	// Token: 0x06002440 RID: 9280 RVA: 0x000D1A28 File Offset: 0x000CFC28
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.RefreshCellOccupy();
	}

	// Token: 0x06002441 RID: 9281 RVA: 0x000D1A36 File Offset: 0x000CFC36
	protected override void OnCleanUp()
	{
		this.CleanUpOccupiedCells();
		base.OnCleanUp();
	}

	// Token: 0x0400151F RID: 5407
	private int previousCell = Grid.InvalidCell;

	// Token: 0x04001520 RID: 5408
	public ObjectLayer[] objectLayers;

	// Token: 0x04001521 RID: 5409
	public CellOffset[] cellOffsets;
}
