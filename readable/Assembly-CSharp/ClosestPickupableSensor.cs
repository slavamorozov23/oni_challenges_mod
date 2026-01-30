using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000522 RID: 1314
public abstract class ClosestPickupableSensor<T> : Sensor where T : Component
{
	// Token: 0x06001C60 RID: 7264 RVA: 0x0009C33D File Offset: 0x0009A53D
	public ClosestPickupableSensor(Sensors sensors, Tag itemSearchTag, bool shouldStartActive) : base(sensors, shouldStartActive)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.consumableConsumer = base.GetComponent<ConsumableConsumer>();
		this.storage = base.GetComponent<Storage>();
		this.itemSearchTag = itemSearchTag;
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x0009C37D File Offset: 0x0009A57D
	public T GetItem()
	{
		return this.item;
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x0009C385 File Offset: 0x0009A585
	public int GetItemNavCost()
	{
		if (!(this.item == null))
		{
			return this.itemNavCost;
		}
		return int.MaxValue;
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x0009C3A6 File Offset: 0x0009A5A6
	public virtual HashSet<Tag> GetForbbidenTags()
	{
		if (!(this.consumableConsumer == null))
		{
			return this.consumableConsumer.forbiddenTagSet;
		}
		return new HashSet<Tag>(0);
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x0009C3C8 File Offset: 0x0009A5C8
	public override void Update()
	{
		HashSet<Tag> forbbidenTags = this.GetForbbidenTags();
		int maxValue = int.MaxValue;
		Pickupable pickupable = this.FindClosestPickupable(this.storage, forbbidenTags, out maxValue, this.itemSearchTag, this.requiredTags);
		bool flag = this.itemInReachButNotPermitted;
		T t = default(T);
		bool flag2 = false;
		if (pickupable != null)
		{
			t = pickupable.GetComponent<T>();
			flag2 = true;
			flag = false;
		}
		else
		{
			int num;
			flag = (this.FindClosestPickupable(this.storage, new HashSet<Tag>(), out num, this.itemSearchTag, this.requiredTags) != null);
		}
		if (t != this.item || this.isThereAnyItemAvailable != flag2)
		{
			this.item = t;
			this.itemNavCost = maxValue;
			this.isThereAnyItemAvailable = flag2;
			this.itemInReachButNotPermitted = flag;
			this.ItemChanged();
		}
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x0009C498 File Offset: 0x0009A698
	public Pickupable FindClosestPickupable(Storage destination, HashSet<Tag> exclude_tags, out int cost, Tag categoryTag, Tag[] otherRequiredTags = null)
	{
		ICollection<Pickupable> pickupables = base.gameObject.GetMyWorld().worldInventory.GetPickupables(categoryTag, false);
		if (pickupables == null)
		{
			cost = int.MaxValue;
			return null;
		}
		if (otherRequiredTags == null)
		{
			otherRequiredTags = new Tag[]
			{
				categoryTag
			};
		}
		Pickupable result = null;
		int num = int.MaxValue;
		foreach (Pickupable pickupable in pickupables)
		{
			if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedFetchAmount, exclude_tags, otherRequiredTags, destination))
			{
				int navigationCost = pickupable.GetNavigationCost(this.navigator, pickupable.cachedCell);
				if (navigationCost != -1 && navigationCost < num)
				{
					result = pickupable;
					num = navigationCost;
				}
			}
		}
		cost = num;
		return result;
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x0009C568 File Offset: 0x0009A768
	public virtual void ItemChanged()
	{
		Action<T> onItemChanged = this.OnItemChanged;
		if (onItemChanged == null)
		{
			return;
		}
		onItemChanged(this.item);
	}

	// Token: 0x040010B6 RID: 4278
	public Action<T> OnItemChanged;

	// Token: 0x040010B7 RID: 4279
	protected T item;

	// Token: 0x040010B8 RID: 4280
	protected int itemNavCost = int.MaxValue;

	// Token: 0x040010B9 RID: 4281
	protected Tag itemSearchTag;

	// Token: 0x040010BA RID: 4282
	protected Tag[] requiredTags;

	// Token: 0x040010BB RID: 4283
	protected bool isThereAnyItemAvailable;

	// Token: 0x040010BC RID: 4284
	protected bool itemInReachButNotPermitted;

	// Token: 0x040010BD RID: 4285
	private Navigator navigator;

	// Token: 0x040010BE RID: 4286
	protected ConsumableConsumer consumableConsumer;

	// Token: 0x040010BF RID: 4287
	private Storage storage;
}
