using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200062A RID: 1578
[AddComponentMenu("KMonoBehaviour/scripts/RoomTracker")]
public class RoomTracker : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x060025A3 RID: 9635 RVA: 0x000D8279 File Offset: 0x000D6479
	// (set) Token: 0x060025A4 RID: 9636 RVA: 0x000D8281 File Offset: 0x000D6481
	public Room room { get; private set; }

	// Token: 0x060025A5 RID: 9637 RVA: 0x000D828C File Offset: 0x000D648C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(!string.IsNullOrEmpty(this.requiredRoomType) && this.requiredRoomType != Db.Get().RoomTypes.Neutral.Id, "RoomTracker must have a requiredRoomType!");
		base.Subscribe<RoomTracker>(144050788, RoomTracker.OnUpdateRoomDelegate);
		this.FindAndSetRoom();
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x000D82F0 File Offset: 0x000D64F0
	public void FindAndSetRoom()
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
		if (cavityForCell != null && cavityForCell.room != null)
		{
			this.OnUpdateRoom(cavityForCell.room);
			return;
		}
		this.OnUpdateRoom(null);
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x000D8337 File Offset: 0x000D6537
	public bool IsInCorrectRoom()
	{
		return this.room != null && this.room.roomType.Id == this.requiredRoomType;
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x000D8360 File Offset: 0x000D6560
	public bool SufficientBuildLocation(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		if (this.requirement == RoomTracker.Requirement.Required || this.requirement == RoomTracker.Requirement.CustomRequired)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (((cavityForCell != null) ? cavityForCell.room : null) == null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x000D83AC File Offset: 0x000D65AC
	private void OnUpdateRoom(object data)
	{
		this.room = (Room)data;
		if (this.room != null && !(this.room.roomType.Id != this.requiredRoomType))
		{
			this.statusItemGuid = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
			return;
		}
		switch (this.requirement)
		{
		case RoomTracker.Requirement.TrackingOnly:
			this.statusItemGuid = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
			return;
		case RoomTracker.Requirement.Recommended:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRecommendedRoom, this.requiredRoomType);
			return;
		case RoomTracker.Requirement.Required:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRequiredRoom, this.requiredRoomType);
			return;
		case RoomTracker.Requirement.CustomRecommended:
		case RoomTracker.Requirement.CustomRequired:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.Get(this.customStatusItemID), this.requiredRoomType);
			return;
		default:
			return;
		}
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000D84E8 File Offset: 0x000D66E8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (!string.IsNullOrEmpty(this.requiredRoomType))
		{
			string name = Db.Get().RoomTypes.Get(this.requiredRoomType).Name;
			switch (this.requirement)
			{
			case RoomTracker.Requirement.Recommended:
			case RoomTracker.Requirement.CustomRecommended:
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.PREFERS_ROOM, name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PREFERS_ROOM, name), Descriptor.DescriptorType.Requirement, false));
				break;
			case RoomTracker.Requirement.Required:
			case RoomTracker.Requirement.CustomRequired:
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESROOM, name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESROOM, name), Descriptor.DescriptorType.Requirement, false));
				break;
			}
		}
		return list;
	}

	// Token: 0x0400160C RID: 5644
	public RoomTracker.Requirement requirement;

	// Token: 0x0400160D RID: 5645
	public string requiredRoomType;

	// Token: 0x0400160E RID: 5646
	public string customStatusItemID;

	// Token: 0x0400160F RID: 5647
	private Guid statusItemGuid;

	// Token: 0x04001611 RID: 5649
	private static readonly EventSystem.IntraObjectHandler<RoomTracker> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<RoomTracker>(delegate(RoomTracker component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x02001508 RID: 5384
	public enum Requirement
	{
		// Token: 0x04007072 RID: 28786
		TrackingOnly,
		// Token: 0x04007073 RID: 28787
		Recommended,
		// Token: 0x04007074 RID: 28788
		Required,
		// Token: 0x04007075 RID: 28789
		CustomRecommended,
		// Token: 0x04007076 RID: 28790
		CustomRequired
	}
}
