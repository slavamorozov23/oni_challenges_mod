using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000900 RID: 2304
[AddComponentMenu("KMonoBehaviour/Workable/DropAllWorkable")]
public class DropAllWorkable : Workable
{
	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x06003FFB RID: 16379 RVA: 0x0016813D File Offset: 0x0016633D
	// (set) Token: 0x06003FFC RID: 16380 RVA: 0x00168145 File Offset: 0x00166345
	private Chore Chore
	{
		get
		{
			return this._chore;
		}
		set
		{
			this._chore = value;
			this.markedForDrop = (this._chore != null);
		}
	}

	// Token: 0x06003FFD RID: 16381 RVA: 0x0016815D File Offset: 0x0016635D
	protected DropAllWorkable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06003FFE RID: 16382 RVA: 0x0016817C File Offset: 0x0016637C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DropAllWorkable>(493375141, DropAllWorkable.OnRefreshUserMenuDelegate);
		base.Subscribe<DropAllWorkable>(-1697596308, DropAllWorkable.OnStorageChangeDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.synchronizeAnims = false;
		base.SetWorkTime(this.dropWorkTime);
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06003FFF RID: 16383 RVA: 0x001681E4 File Offset: 0x001663E4
	private Storage[] GetStorages()
	{
		if (this.storages == null)
		{
			this.storages = base.GetComponents<Storage>();
		}
		return this.storages;
	}

	// Token: 0x06004000 RID: 16384 RVA: 0x00168200 File Offset: 0x00166400
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.showCmd = this.GetNewShowCmd();
		if (this.markedForDrop)
		{
			this.DropAll();
		}
	}

	// Token: 0x06004001 RID: 16385 RVA: 0x00168224 File Offset: 0x00166424
	public void DropAll()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
		}
		else if (this.Chore == null)
		{
			ChoreType chore_type = (!string.IsNullOrEmpty(this.choreTypeID)) ? Db.Get().ChoreTypes.Get(this.choreTypeID) : Db.Get().ChoreTypes.EmptyStorage;
			this.Chore = new WorkChore<DropAllWorkable>(chore_type, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
		else
		{
			this.Chore.Cancel("Cancelled emptying");
			this.Chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
			base.ShowProgressBar(false);
		}
		this.RefreshStatusItem();
	}

	// Token: 0x06004002 RID: 16386 RVA: 0x001682D8 File Offset: 0x001664D8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			List<GameObject> list = new List<GameObject>(array[i].items);
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject = array[i].Drop(list[j], true);
				if (gameObject != null)
				{
					foreach (Tag tag in this.removeTags)
					{
						gameObject.RemoveTag(tag);
					}
					gameObject.Trigger(580035959, worker);
					if (this.resetTargetWorkableOnCompleteWork)
					{
						Pickupable component = gameObject.GetComponent<Pickupable>();
						component.targetWorkable = component;
						component.SetOffsetTable(OffsetGroups.InvertedStandardTable);
					}
				}
			}
		}
		this.Chore = null;
		this.RefreshStatusItem();
		base.Trigger(-1957399615, null);
	}

	// Token: 0x06004003 RID: 16387 RVA: 0x001683D4 File Offset: 0x001665D4
	private void OnRefreshUserMenu(object data)
	{
		if (this.showCmd)
		{
			KIconButtonMenu.ButtonInfo button = (this.Chore == null) ? new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME_OFF, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP_OFF, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x06004004 RID: 16388 RVA: 0x00168478 File Offset: 0x00166678
	private bool GetNewShowCmd()
	{
		bool flag = false;
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			flag = (flag || !array[i].IsEmpty());
		}
		return flag;
	}

	// Token: 0x06004005 RID: 16389 RVA: 0x001684B0 File Offset: 0x001666B0
	private void OnStorageChange(object data)
	{
		bool newShowCmd = this.GetNewShowCmd();
		if (newShowCmd != this.showCmd)
		{
			this.showCmd = newShowCmd;
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

	// Token: 0x06004006 RID: 16390 RVA: 0x001684EC File Offset: 0x001666EC
	private void RefreshStatusItem()
	{
		if (this.Chore != null && this.statusItem == Guid.Empty)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.statusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
			return;
		}
		if (this.Chore == null && this.statusItem != Guid.Empty)
		{
			KSelectable component2 = base.GetComponent<KSelectable>();
			this.statusItem = component2.RemoveStatusItem(this.statusItem, false);
		}
	}

	// Token: 0x040027A3 RID: 10147
	[Serialize]
	private bool markedForDrop;

	// Token: 0x040027A4 RID: 10148
	private Chore _chore;

	// Token: 0x040027A5 RID: 10149
	private bool showCmd;

	// Token: 0x040027A6 RID: 10150
	private Storage[] storages;

	// Token: 0x040027A7 RID: 10151
	public float dropWorkTime = 0.1f;

	// Token: 0x040027A8 RID: 10152
	public string choreTypeID;

	// Token: 0x040027A9 RID: 10153
	[MyCmpAdd]
	private Prioritizable _prioritizable;

	// Token: 0x040027AA RID: 10154
	public List<Tag> removeTags;

	// Token: 0x040027AB RID: 10155
	public bool resetTargetWorkableOnCompleteWork;

	// Token: 0x040027AC RID: 10156
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040027AD RID: 10157
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x040027AE RID: 10158
	private Guid statusItem;
}
