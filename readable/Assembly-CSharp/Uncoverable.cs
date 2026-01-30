using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200066F RID: 1647
[AddComponentMenu("KMonoBehaviour/scripts/Uncoverable")]
public class Uncoverable : KMonoBehaviour
{
	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060027F5 RID: 10229 RVA: 0x000E64B4 File Offset: 0x000E46B4
	public bool IsUncovered
	{
		get
		{
			return this.hasBeenUncovered;
		}
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x000E64BC File Offset: 0x000E46BC
	private bool IsAnyCellShowing()
	{
		int rootCell = Grid.PosToCell(this);
		return !this.occupyArea.TestArea(rootCell, null, Uncoverable.IsCellBlockedDelegate);
	}

	// Token: 0x060027F7 RID: 10231 RVA: 0x000E64E5 File Offset: 0x000E46E5
	private static bool IsCellBlocked(int cell, object data)
	{
		return Grid.Element[cell].IsSolid && !Grid.Foundation[cell];
	}

	// Token: 0x060027F8 RID: 10232 RVA: 0x000E6505 File Offset: 0x000E4705
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x000E6510 File Offset: 0x000E4710
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.IsAnyCellShowing())
		{
			this.hasBeenUncovered = true;
		}
		if (!this.hasBeenUncovered)
		{
			base.GetComponent<KSelectable>().IsSelectable = false;
			Extents extents = this.occupyArea.GetExtents();
			this.partitionerEntry = GameScenePartitioner.Instance.Add("Uncoverable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		}
	}

	// Token: 0x060027FA RID: 10234 RVA: 0x000E6584 File Offset: 0x000E4784
	private void OnSolidChanged(object data)
	{
		if (this.IsAnyCellShowing() && !this.hasBeenUncovered && this.partitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			this.hasBeenUncovered = true;
			base.GetComponent<KSelectable>().IsSelectable = true;
			Notification notification = new Notification(MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION, NotificationType.Good, new Func<List<Notification>, object, string>(Uncoverable.OnNotificationToolTip), this, true, 0f, null, null, null, true, false, false);
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x060027FB RID: 10235 RVA: 0x000E6614 File Offset: 0x000E4814
	private static string OnNotificationToolTip(List<Notification> notifications, object data)
	{
		Uncoverable cmp = (Uncoverable)data;
		return MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION_TOOLTIP.Replace("{Uncoverable}", cmp.GetProperName());
	}

	// Token: 0x060027FC RID: 10236 RVA: 0x000E663D File Offset: 0x000E483D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04001778 RID: 6008
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x04001779 RID: 6009
	[Serialize]
	private bool hasBeenUncovered;

	// Token: 0x0400177A RID: 6010
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400177B RID: 6011
	private static readonly Func<int, object, bool> IsCellBlockedDelegate = (int cell, object data) => Uncoverable.IsCellBlocked(cell, data);
}
