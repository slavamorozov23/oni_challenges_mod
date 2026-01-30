using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000901 RID: 2305
[AddComponentMenu("KMonoBehaviour/Workable/DropToUserCapacity")]
public class DropToUserCapacity : Workable
{
	// Token: 0x06004008 RID: 16392 RVA: 0x001685A1 File Offset: 0x001667A1
	protected DropToUserCapacity()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06004009 RID: 16393 RVA: 0x001685B4 File Offset: 0x001667B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		base.Subscribe<DropToUserCapacity>(-945020481, DropToUserCapacity.OnStorageCapacityChangedHandler);
		base.Subscribe<DropToUserCapacity>(-1697596308, DropToUserCapacity.OnStorageChangedHandler);
		this.synchronizeAnims = false;
		base.SetWorkTime(0.1f);
	}

	// Token: 0x0600400A RID: 16394 RVA: 0x00168610 File Offset: 0x00166810
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateChore();
	}

	// Token: 0x0600400B RID: 16395 RVA: 0x0016861E File Offset: 0x0016681E
	private Storage[] GetStorages()
	{
		if (this.storages == null)
		{
			this.storages = base.GetComponents<Storage>();
		}
		return this.storages;
	}

	// Token: 0x0600400C RID: 16396 RVA: 0x0016863A File Offset: 0x0016683A
	private void OnStorageChanged(object data)
	{
		this.UpdateChore();
	}

	// Token: 0x0600400D RID: 16397 RVA: 0x00168644 File Offset: 0x00166844
	public void UpdateChore()
	{
		IUserControlledCapacity component = base.GetComponent<IUserControlledCapacity>();
		if (component != null && component.AmountStored > component.UserMaxCapacity)
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<DropToUserCapacity>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("Cancelled emptying");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
			base.ShowProgressBar(false);
		}
	}

	// Token: 0x0600400E RID: 16398 RVA: 0x001686D8 File Offset: 0x001668D8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		IUserControlledCapacity component2 = base.GetComponent<IUserControlledCapacity>();
		float num = Mathf.Max(0f, component2.AmountStored - component2.UserMaxCapacity);
		List<GameObject> list = new List<GameObject>(component.items);
		for (int i = 0; i < list.Count; i++)
		{
			Pickupable component3 = list[i].GetComponent<Pickupable>();
			if (component3.PrimaryElement.Mass > num)
			{
				component3.Take(num).transform.SetPosition(base.transform.GetPosition());
				return;
			}
			num -= component3.PrimaryElement.Mass;
			component.Drop(component3.gameObject, true);
		}
		this.chore = null;
	}

	// Token: 0x040027AF RID: 10159
	private Chore chore;

	// Token: 0x040027B0 RID: 10160
	private bool showCmd;

	// Token: 0x040027B1 RID: 10161
	private Storage[] storages;

	// Token: 0x040027B2 RID: 10162
	private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageCapacityChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>(delegate(DropToUserCapacity component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x040027B3 RID: 10163
	private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>(delegate(DropToUserCapacity component, object data)
	{
		component.OnStorageChanged(data);
	});
}
