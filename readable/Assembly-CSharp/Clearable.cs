using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005A8 RID: 1448
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Clearable")]
public class Clearable : Workable, ISaveLoadable, IRender1000ms
{
	// Token: 0x060020F1 RID: 8433 RVA: 0x000BE6F4 File Offset: 0x000BC8F4
	protected override void OnPrefabInit()
	{
		base.Subscribe<Clearable>(2127324410, Clearable.OnCancelDelegate);
		base.Subscribe<Clearable>(856640610, Clearable.OnStoreDelegate);
		base.Subscribe<Clearable>(-2064133523, Clearable.OnAbsorbDelegate);
		base.Subscribe<Clearable>(493375141, Clearable.OnRefreshUserMenuDelegate);
		base.Subscribe<Clearable>(-1617557748, Clearable.OnEquippedDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Clearing;
		this.simRenderLoadBalance = true;
		this.autoRegisterSimRender = false;
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x000BE77C File Offset: 0x000BC97C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForClear)
		{
			if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
			{
				if (!base.transform.parent.GetComponent<Storage>().allowClearable)
				{
					this.isMarkedForClear = false;
				}
				else
				{
					this.MarkForClear(true, true);
				}
			}
			else
			{
				this.MarkForClear(true, false);
			}
		}
		this.RefreshClearableStatus(true);
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x000BE7E7 File Offset: 0x000BC9E7
	private void OnStore(object _)
	{
		this.CancelClearing();
	}

	// Token: 0x060020F4 RID: 8436 RVA: 0x000BE7F0 File Offset: 0x000BC9F0
	private void OnCancel(object _)
	{
		for (ObjectLayerListItem objectLayerListItem = this.pickupable.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
		{
			if (objectLayerListItem.pickupable != null)
			{
				objectLayerListItem.pickupable.Clearable.CancelClearing();
			}
		}
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x000BE834 File Offset: 0x000BCA34
	public void CancelClearing()
	{
		if (this.isMarkedForClear)
		{
			this.isMarkedForClear = false;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Garbage);
			Prioritizable.RemoveRef(base.gameObject);
			if (this.clearHandle.IsValid())
			{
				GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
				this.clearHandle.Clear();
			}
			this.RefreshClearableStatus(true);
			SimAndRenderScheduler.instance.Remove(this);
		}
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x000BE8A8 File Offset: 0x000BCAA8
	public void MarkForClear(bool restoringFromSave = false, bool allowWhenStored = false)
	{
		if (!this.isClearable)
		{
			return;
		}
		if ((!this.isMarkedForClear || restoringFromSave) && !this.pickupable.IsEntombed && !this.clearHandle.IsValid() && (!this.HasTag(GameTags.Stored) || allowWhenStored))
		{
			Prioritizable.AddRef(base.gameObject);
			this.pickupable.KPrefabID.AddTag(GameTags.Garbage, false);
			this.isMarkedForClear = true;
			this.clearHandle = GlobalChoreProvider.Instance.RegisterClearable(this);
			this.RefreshClearableStatus(true);
			SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
		}
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x000BE948 File Offset: 0x000BCB48
	private void OnClickClear()
	{
		this.MarkForClear(false, false);
	}

	// Token: 0x060020F8 RID: 8440 RVA: 0x000BE952 File Offset: 0x000BCB52
	private void OnClickCancel()
	{
		this.CancelClearing();
	}

	// Token: 0x060020F9 RID: 8441 RVA: 0x000BE95A File Offset: 0x000BCB5A
	private void OnEquipped(object data)
	{
		this.CancelClearing();
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x000BE962 File Offset: 0x000BCB62
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.clearHandle.IsValid())
		{
			GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
			this.clearHandle.Clear();
		}
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x000BE994 File Offset: 0x000BCB94
	private void OnRefreshUserMenu(object data)
	{
		if (!this.isClearable || base.GetComponent<Health>() != null || this.pickupable.KPrefabID.HasTag(GameTags.Stored) || this.pickupable.KPrefabID.HasTag(GameTags.MarkedForMove))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForClear ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.CLEAR.NAME_OFF, new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEAR.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.CLEAR.NAME, new System.Action(this.OnClickClear), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEAR.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x000BEA74 File Offset: 0x000BCC74
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			Clearable component = pickupable.GetComponent<Clearable>();
			if (component != null && component.isMarkedForClear)
			{
				this.MarkForClear(false, false);
			}
		}
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x000BEAB1 File Offset: 0x000BCCB1
	public void Render1000ms(float dt)
	{
		this.RefreshClearableStatus(false);
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x000BEABC File Offset: 0x000BCCBC
	public void RefreshClearableStatus(bool force_update)
	{
		if (force_update || this.isMarkedForClear)
		{
			bool show = false;
			bool show2 = false;
			if (this.isMarkedForClear)
			{
				show2 = !(show = GlobalChoreProvider.Instance.ClearableHasDestination(this.pickupable));
			}
			this.pendingClearGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClear, this.pendingClearGuid, show, this);
			this.pendingClearNoStorageGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClearNoStorage, this.pendingClearNoStorageGuid, show2, this);
		}
	}

	// Token: 0x0400132E RID: 4910
	[MyCmpReq]
	private Pickupable pickupable;

	// Token: 0x0400132F RID: 4911
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001330 RID: 4912
	[Serialize]
	private bool isMarkedForClear;

	// Token: 0x04001331 RID: 4913
	private HandleVector<int>.Handle clearHandle;

	// Token: 0x04001332 RID: 4914
	public bool isClearable = true;

	// Token: 0x04001333 RID: 4915
	private Guid pendingClearGuid;

	// Token: 0x04001334 RID: 4916
	private Guid pendingClearNoStorageGuid;

	// Token: 0x04001335 RID: 4917
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04001336 RID: 4918
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04001337 RID: 4919
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x04001338 RID: 4920
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001339 RID: 4921
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnEquipped(data);
	});
}
