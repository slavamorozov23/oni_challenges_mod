using System;
using UnityEngine;

// Token: 0x020005E2 RID: 1506
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Floodable")]
public class Floodable : KMonoBehaviour
{
	// Token: 0x17000160 RID: 352
	// (get) Token: 0x060022DD RID: 8925 RVA: 0x000CB323 File Offset: 0x000C9523
	public bool IsFlooded
	{
		get
		{
			return this.isFlooded;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x060022DE RID: 8926 RVA: 0x000CB32B File Offset: 0x000C952B
	public BuildingDef Def
	{
		get
		{
			return this.building.Def;
		}
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x000CB338 File Offset: 0x000C9538
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Floodable.OnSpawn", base.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
		this.OnElementChanged(null);
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x000CB390 File Offset: 0x000C9590
	private void OnElementChanged(object data)
	{
		bool flag = false;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			if (Grid.IsSubstantialLiquid(this.building.PlacementCells[i], 0.35f))
			{
				flag = true;
				break;
			}
		}
		if (flag != this.isFlooded)
		{
			this.isFlooded = flag;
			this.operational.SetFlag(Floodable.notFloodedFlag, !this.isFlooded);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Flooded, this.isFlooded, this);
		}
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x000CB41F File Offset: 0x000C961F
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x0400146E RID: 5230
	[MyCmpReq]
	private Building building;

	// Token: 0x0400146F RID: 5231
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04001470 RID: 5232
	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	// Token: 0x04001471 RID: 5233
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001472 RID: 5234
	public static Operational.Flag notFloodedFlag = new Operational.Flag("not_flooded", Operational.Flag.Type.Functional);

	// Token: 0x04001473 RID: 5235
	private bool isFlooded;

	// Token: 0x04001474 RID: 5236
	private HandleVector<int>.Handle partitionerEntry;
}
