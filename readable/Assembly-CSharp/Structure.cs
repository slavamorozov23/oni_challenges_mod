using System;
using UnityEngine;

// Token: 0x02000645 RID: 1605
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Structure")]
public class Structure : KMonoBehaviour
{
	// Token: 0x06002709 RID: 9993 RVA: 0x000E0E2A File Offset: 0x000DF02A
	public bool IsEntombed()
	{
		return this.isEntombed;
	}

	// Token: 0x0600270A RID: 9994 RVA: 0x000E0E34 File Offset: 0x000DF034
	public static bool IsBuildingEntombed(Building building)
	{
		if (!Grid.IsValidCell(Grid.PosToCell(building)))
		{
			return false;
		}
		for (int i = 0; i < building.PlacementCells.Length; i++)
		{
			int num = building.PlacementCells[i];
			if (Grid.Element[num].IsSolid && !Grid.Foundation[num])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600270B RID: 9995 RVA: 0x000E0E8C File Offset: 0x000DF08C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Extents extents = this.building.GetExtents();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Structure.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.OnSolidChanged(null);
		base.Subscribe<Structure>(-887025858, Structure.RocketLandedDelegate);
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x000E0EF5 File Offset: 0x000DF0F5
	public void UpdatePosition()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.building.GetExtents());
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x000E0F12 File Offset: 0x000DF112
	private void RocketChanged(object data)
	{
		this.OnSolidChanged(data);
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x000E0F1C File Offset: 0x000DF11C
	private void OnSolidChanged(object data)
	{
		bool flag = Structure.IsBuildingEntombed(this.building);
		if (flag != this.isEntombed)
		{
			this.isEntombed = flag;
			if (this.isEntombed)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			}
			this.operational.SetFlag(Structure.notEntombedFlag, !this.isEntombed);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, this.isEntombed, this);
			base.Trigger(-1089732772, BoxedBools.Box(this.isEntombed));
		}
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x000E0FC4 File Offset: 0x000DF1C4
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04001714 RID: 5908
	[MyCmpReq]
	private Building building;

	// Token: 0x04001715 RID: 5909
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04001716 RID: 5910
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001717 RID: 5911
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x04001718 RID: 5912
	private bool isEntombed;

	// Token: 0x04001719 RID: 5913
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400171A RID: 5914
	private static EventSystem.IntraObjectHandler<Structure> RocketLandedDelegate = new EventSystem.IntraObjectHandler<Structure>(delegate(Structure cmp, object data)
	{
		cmp.RocketChanged(data);
	});
}
