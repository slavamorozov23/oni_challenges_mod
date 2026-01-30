using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020008C1 RID: 2241
[AddComponentMenu("KMonoBehaviour/scripts/UprootedMonitor")]
public class UprootedMonitor : KMonoBehaviour
{
	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06003DCB RID: 15819 RVA: 0x00158813 File Offset: 0x00156A13
	public bool IsUprooted
	{
		get
		{
			return this.uprooted || base.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
		}
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x0015882F File Offset: 0x00156A2F
	protected override void OnPrefabInit()
	{
		base.Subscribe<UprootedMonitor>(-216549700, UprootedMonitor.OnUprootedDelegate);
		this.position = Grid.PosToCell(base.gameObject);
		base.OnPrefabInit();
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x00158859 File Offset: 0x00156A59
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RegisterMonitoredCellsPartitionerEntries();
	}

	// Token: 0x06003DCE RID: 15822 RVA: 0x00158867 File Offset: 0x00156A67
	public void SetNewMonitorCells(CellOffset[] cellsOffsets)
	{
		this.UnregisterMonitoredCellsPartitionerEntries();
		this.monitorCells = cellsOffsets;
		this.RegisterMonitoredCellsPartitionerEntries();
	}

	// Token: 0x06003DCF RID: 15823 RVA: 0x0015887C File Offset: 0x00156A7C
	private void UnregisterMonitoredCellsPartitionerEntries()
	{
		foreach (HandleVector<int>.Handle handle in this.partitionerEntries)
		{
			GameScenePartitioner.Instance.Free(ref handle);
		}
		this.partitionerEntries.Clear();
	}

	// Token: 0x06003DD0 RID: 15824 RVA: 0x001588E0 File Offset: 0x00156AE0
	private void RegisterMonitoredCellsPartitionerEntries()
	{
		foreach (CellOffset offset in this.monitorCells)
		{
			int cell = Grid.OffsetCell(this.position, offset);
			if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
			{
				this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("UprootedMonitor.OnSpawn", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
			}
		}
		this.OnGroundChanged(null);
	}

	// Token: 0x06003DD1 RID: 15825 RVA: 0x0015896A File Offset: 0x00156B6A
	protected override void OnCleanUp()
	{
		this.UnregisterMonitoredCellsPartitionerEntries();
		base.OnCleanUp();
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x00158978 File Offset: 0x00156B78
	public bool CheckTileGrowable()
	{
		return !this.canBeUprooted || (!this.uprooted && this.IsSuitableFoundation(this.position));
	}

	// Token: 0x06003DD3 RID: 15827 RVA: 0x001589A0 File Offset: 0x00156BA0
	public bool IsSuitableFoundation(int cell)
	{
		bool flag = true;
		foreach (CellOffset offset in this.monitorCells)
		{
			if (!Grid.IsCellOffsetValid(cell, offset))
			{
				return false;
			}
			int num = Grid.OffsetCell(cell, offset);
			if (this.customFoundationCheckFn != null)
			{
				flag = this.customFoundationCheckFn(num);
			}
			else
			{
				flag = Grid.Solid[num];
			}
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x06003DD4 RID: 15828 RVA: 0x00158A09 File Offset: 0x00156C09
	public void OnGroundChanged(object callbackData)
	{
		if (!this.CheckTileGrowable())
		{
			this.uprooted = true;
		}
		if (this.uprooted)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			base.Trigger(-216549700, null);
		}
	}

	// Token: 0x04002624 RID: 9764
	private int position;

	// Token: 0x04002625 RID: 9765
	[Serialize]
	public bool canBeUprooted = true;

	// Token: 0x04002626 RID: 9766
	[Serialize]
	private bool uprooted;

	// Token: 0x04002627 RID: 9767
	public CellOffset[] monitorCells = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x04002628 RID: 9768
	public Func<int, bool> customFoundationCheckFn;

	// Token: 0x04002629 RID: 9769
	private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();

	// Token: 0x0400262A RID: 9770
	private static readonly EventSystem.IntraObjectHandler<UprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<UprootedMonitor>(delegate(UprootedMonitor component, object data)
	{
		if (!component.uprooted)
		{
			component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			component.uprooted = true;
			component.Trigger(-216549700, null);
		}
	});
}
