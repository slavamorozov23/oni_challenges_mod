using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020008A5 RID: 2213
[AddComponentMenu("KMonoBehaviour/scripts/FoundationMonitor")]
public class FoundationMonitor : KMonoBehaviour
{
	// Token: 0x06003CEC RID: 15596 RVA: 0x00154550 File Offset: 0x00152750
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.position = Grid.PosToCell(base.gameObject);
		foreach (CellOffset offset in this.monitorCells)
		{
			int cell = Grid.OffsetCell(this.position, offset);
			if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
			{
				this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("FoundationMonitor.OnSpawn", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
			}
			this.OnGroundChanged(null);
		}
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x001545F4 File Offset: 0x001527F4
	protected override void OnCleanUp()
	{
		foreach (HandleVector<int>.Handle handle in this.partitionerEntries)
		{
			GameScenePartitioner.Instance.Free(ref handle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06003CEE RID: 15598 RVA: 0x00154654 File Offset: 0x00152854
	public bool CheckFoundationValid()
	{
		return !this.needsFoundation || this.IsSuitableFoundation(this.position);
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x0015466C File Offset: 0x0015286C
	public bool IsSuitableFoundation(int cell)
	{
		bool flag = true;
		foreach (CellOffset offset in this.monitorCells)
		{
			if (!Grid.IsCellOffsetValid(cell, offset))
			{
				return false;
			}
			int i2 = Grid.OffsetCell(cell, offset);
			flag = Grid.Solid[i2];
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x06003CF0 RID: 15600 RVA: 0x001546C0 File Offset: 0x001528C0
	public void OnGroundChanged(object callbackData)
	{
		if (!this.hasFoundation && this.CheckFoundationValid())
		{
			this.hasFoundation = true;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.HasNoFoundation);
			base.Trigger(-1960061727, null);
		}
		if (this.hasFoundation && !this.CheckFoundationValid())
		{
			this.hasFoundation = false;
			base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.HasNoFoundation, false);
			base.Trigger(-1960061727, null);
		}
	}

	// Token: 0x0400259A RID: 9626
	private int position;

	// Token: 0x0400259B RID: 9627
	[Serialize]
	public bool needsFoundation = true;

	// Token: 0x0400259C RID: 9628
	[Serialize]
	private bool hasFoundation = true;

	// Token: 0x0400259D RID: 9629
	public CellOffset[] monitorCells = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x0400259E RID: 9630
	private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();
}
