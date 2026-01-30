using System;
using System.Collections.Generic;

// Token: 0x020004D8 RID: 1240
internal class ClearableManager
{
	// Token: 0x06001AB8 RID: 6840 RVA: 0x00093558 File Offset: 0x00091758
	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return this.markedClearables.Allocate(new ClearableManager.MarkedClearable
		{
			clearable = clearable,
			pickupable = clearable.GetComponent<Pickupable>(),
			prioritizable = clearable.GetComponent<Prioritizable>()
		});
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x0009359B File Offset: 0x0009179B
	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		this.markedClearables.Free(handle);
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x000935AC File Offset: 0x000917AC
	public void CollectAndSortClearables(Navigator navigator)
	{
		this.sortedClearables.Clear();
		foreach (ClearableManager.MarkedClearable markedClearable in this.markedClearables.GetDataList())
		{
			int navigationCost = markedClearable.pickupable.GetNavigationCost(navigator, markedClearable.pickupable.cachedCell);
			if (navigationCost != -1)
			{
				this.sortedClearables.Add(new ClearableManager.SortedClearable
				{
					pickupable = markedClearable.pickupable,
					masterPriority = markedClearable.prioritizable.GetMasterPriority(),
					cost = navigationCost
				});
			}
		}
		this.sortedClearables.Sort(ClearableManager.SortedClearable.comparer);
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x00093670 File Offset: 0x00091870
	public void CollectChores(List<GlobalChoreProvider.Fetch> fetches, ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		ChoreType transport = Db.Get().ChoreTypes.Transport;
		int personalPriority = consumer_state.consumer.GetPersonalPriority(transport);
		int priority = Game.Instance.advancedPersonalPriorities ? transport.explicitPriority : transport.priority;
		bool flag = false;
		for (int i = 0; i < this.sortedClearables.Count; i++)
		{
			ClearableManager.SortedClearable sortedClearable = this.sortedClearables[i];
			Pickupable pickupable = sortedClearable.pickupable;
			PrioritySetting masterPriority = sortedClearable.masterPriority;
			Chore.Precondition.Context item = default(Chore.Precondition.Context);
			item.personalPriority = personalPriority;
			KPrefabID kprefabID = pickupable.KPrefabID;
			int num = 0;
			while (fetches != null && num < fetches.Count)
			{
				GlobalChoreProvider.Fetch fetch = fetches[num];
				if ((fetch.chore.criteria == FetchChore.MatchCriteria.MatchID && fetch.chore.tags.Contains(kprefabID.PrefabTag)) || (fetch.chore.criteria == FetchChore.MatchCriteria.MatchTags && kprefabID.HasTag(fetch.chore.tagsFirst)))
				{
					item.Set(fetch.chore, consumer_state, false, pickupable);
					item.choreTypeForPermission = transport;
					item.RunPreconditions();
					if (item.IsSuccess())
					{
						item.masterPriority = masterPriority;
						item.priority = priority;
						item.interruptPriority = transport.interruptPriority;
						succeeded.Add(item);
						flag = true;
						break;
					}
				}
				num++;
			}
			if (flag)
			{
				break;
			}
		}
	}

	// Token: 0x04000F62 RID: 3938
	private KCompactedVector<ClearableManager.MarkedClearable> markedClearables = new KCompactedVector<ClearableManager.MarkedClearable>(0);

	// Token: 0x04000F63 RID: 3939
	private List<ClearableManager.SortedClearable> sortedClearables = new List<ClearableManager.SortedClearable>();

	// Token: 0x0200134F RID: 4943
	private struct MarkedClearable
	{
		// Token: 0x04006AE3 RID: 27363
		public Clearable clearable;

		// Token: 0x04006AE4 RID: 27364
		public Pickupable pickupable;

		// Token: 0x04006AE5 RID: 27365
		public Prioritizable prioritizable;
	}

	// Token: 0x02001350 RID: 4944
	private struct SortedClearable
	{
		// Token: 0x04006AE6 RID: 27366
		public Pickupable pickupable;

		// Token: 0x04006AE7 RID: 27367
		public PrioritySetting masterPriority;

		// Token: 0x04006AE8 RID: 27368
		public int cost;

		// Token: 0x04006AE9 RID: 27369
		public static ClearableManager.SortedClearable.Comparer comparer = new ClearableManager.SortedClearable.Comparer();

		// Token: 0x020027F9 RID: 10233
		public class Comparer : IComparer<ClearableManager.SortedClearable>
		{
			// Token: 0x0600CABF RID: 51903 RVA: 0x0042BB50 File Offset: 0x00429D50
			public int Compare(ClearableManager.SortedClearable a, ClearableManager.SortedClearable b)
			{
				int num = b.masterPriority.priority_value - a.masterPriority.priority_value;
				if (num == 0)
				{
					return a.cost - b.cost;
				}
				return num;
			}
		}
	}
}
