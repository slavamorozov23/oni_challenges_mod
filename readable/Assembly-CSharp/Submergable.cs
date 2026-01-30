using System;
using UnityEngine;

// Token: 0x02000647 RID: 1607
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Submergable")]
public class Submergable : KMonoBehaviour
{
	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06002726 RID: 10022 RVA: 0x000E1468 File Offset: 0x000DF668
	public bool IsSubmerged
	{
		get
		{
			return this.isSubmerged;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06002727 RID: 10023 RVA: 0x000E1470 File Offset: 0x000DF670
	public BuildingDef Def
	{
		get
		{
			return this.building.Def;
		}
	}

	// Token: 0x06002728 RID: 10024 RVA: 0x000E1480 File Offset: 0x000DF680
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Submergable.OnSpawn", base.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
		this.OnElementChanged(null);
		this.operational.SetFlag(Submergable.notSubmergedFlag, this.isSubmerged);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !this.isSubmerged, this);
	}

	// Token: 0x06002729 RID: 10025 RVA: 0x000E1514 File Offset: 0x000DF714
	private void OnElementChanged(object data)
	{
		bool flag = true;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			if (!Grid.IsLiquid(this.building.PlacementCells[i]))
			{
				flag = false;
				break;
			}
		}
		if (flag != this.isSubmerged)
		{
			this.isSubmerged = flag;
			this.operational.SetFlag(Submergable.notSubmergedFlag, this.isSubmerged);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !this.isSubmerged, this);
		}
	}

	// Token: 0x0600272A RID: 10026 RVA: 0x000E159E File Offset: 0x000DF79E
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04001724 RID: 5924
	[MyCmpReq]
	private Building building;

	// Token: 0x04001725 RID: 5925
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04001726 RID: 5926
	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	// Token: 0x04001727 RID: 5927
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001728 RID: 5928
	public static Operational.Flag notSubmergedFlag = new Operational.Flag("submerged", Operational.Flag.Type.Functional);

	// Token: 0x04001729 RID: 5929
	private bool isSubmerged;

	// Token: 0x0400172A RID: 5930
	private HandleVector<int>.Handle partitionerEntry;
}
