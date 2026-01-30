using System;
using System.Collections.Generic;

// Token: 0x020004D7 RID: 1239
public class GlobalChoreProvider : ChoreProvider, IRender200ms
{
	// Token: 0x06001AA8 RID: 6824 RVA: 0x00092F6B File Offset: 0x0009116B
	public static void DestroyInstance()
	{
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x00092F73 File Offset: 0x00091173
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GlobalChoreProvider.Instance = this;
		this.clearableManager = new ClearableManager();
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x00092F8C File Offset: 0x0009118C
	protected override void OnWorldRemoved(object data)
	{
		int value = ((Boxed<int>)data).value;
		int parentWorldId = ClusterManager.Instance.GetWorld(value).ParentWorldId;
		List<FetchChore> chores;
		if (this.fetchMap.TryGetValue(parentWorldId, out chores))
		{
			base.ClearWorldChores<FetchChore>(chores, value);
		}
		base.OnWorldRemoved(data);
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x00092FD8 File Offset: 0x000911D8
	protected override void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255)
		{
			return;
		}
		base.OnWorldParentChanged(data);
		List<FetchChore> oldChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<FetchChore> newChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.fetchMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<FetchChore>());
		}
		base.TransferChores<FetchChore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x00093064 File Offset: 0x00091264
	public override void AddChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list = (this.fetchMap[myParentWorldId] = new List<FetchChore>());
			}
			chore.provider = this;
			list.Add(fetchChore);
			return;
		}
		base.AddChore(chore);
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x000930C0 File Offset: 0x000912C0
	public override void RemoveChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list.Remove(fetchChore);
			}
			chore.provider = null;
			return;
		}
		base.RemoveChore(chore);
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x0009310C File Offset: 0x0009130C
	public void UpdateFetches(Navigator navigator)
	{
		List<FetchChore> list = null;
		int myParentWorldId = navigator.gameObject.GetMyParentWorldId();
		if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		this.fetches.Clear();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			FetchChore fetchChore = list[i];
			if (!(fetchChore.driver != null) && (!(fetchChore.automatable != null) || !fetchChore.automatable.GetAutomationOnly()))
			{
				if (fetchChore.provider == null)
				{
					fetchChore.Cancel("no provider");
					list[i] = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
				}
				else
				{
					Storage destination = fetchChore.destination;
					if (!(destination == null))
					{
						int navigationCost = navigator.GetNavigationCost(destination);
						if (navigationCost != -1)
						{
							this.fetches.Add(new GlobalChoreProvider.Fetch
							{
								chore = fetchChore,
								idsHash = fetchChore.tagsHash,
								cost = navigationCost,
								priority = fetchChore.masterPriority,
								category = destination.fetchCategory
							});
						}
					}
				}
			}
		}
		if (this.fetches.Count > 0)
		{
			this.fetches.Sort(GlobalChoreProvider.Comparer);
			int j = 1;
			int num = 0;
			while (j < this.fetches.Count)
			{
				if (!this.fetches[num].IsBetterThan(this.fetches[j]))
				{
					num++;
					this.fetches[num] = this.fetches[j];
				}
				j++;
			}
			this.fetches.RemoveRange(num + 1, this.fetches.Count - num - 1);
		}
		this.clearableManager.CollectAndSortClearables(navigator);
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000932F0 File Offset: 0x000914F0
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		base.CollectChores(consumer_state, succeeded, failed_contexts);
		this.clearableManager.CollectChores(this.fetches, consumer_state, succeeded, failed_contexts);
		if (this.fetches.Count > 48)
		{
			GlobalChoreProvider.batch_context.Setup(this, consumer_state);
			GlobalChoreProvider.batch_work_items.Reset(GlobalChoreProvider.batch_context);
			for (int i = 0; i < this.fetches.Count; i += 16)
			{
				GlobalChoreProvider.batch_work_items.Add(new MultithreadedCollectChoreContext<GlobalChoreProvider>.WorkBlock<GlobalChoreProvider.GlobalChoreProviderMultithreader>(i, Math.Min(i + 16, this.fetches.Count)));
			}
			GlobalJobManager.Run(GlobalChoreProvider.batch_work_items);
			GlobalChoreProvider.batch_context.Finish(succeeded, failed_contexts);
			return;
		}
		for (int j = 0; j < this.fetches.Count; j++)
		{
			this.fetches[j].chore.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded, failed_contexts, false);
		}
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000933C6 File Offset: 0x000915C6
	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return this.clearableManager.RegisterClearable(clearable);
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000933D4 File Offset: 0x000915D4
	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		this.clearableManager.UnregisterClearable(handle);
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000933E2 File Offset: 0x000915E2
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x000933F0 File Offset: 0x000915F0
	public void Render200ms(float dt)
	{
		this.UpdateStorageFetchableBits();
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x000933F8 File Offset: 0x000915F8
	private void UpdateStorageFetchableBits()
	{
		ChoreType storageFetch = Db.Get().ChoreTypes.StorageFetch;
		ChoreType foodFetch = Db.Get().ChoreTypes.FoodFetch;
		this.storageFetchableTags.Clear();
		List<int> worldIDsSorted = ClusterManager.Instance.GetWorldIDsSorted();
		for (int i = 0; i < worldIDsSorted.Count; i++)
		{
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(worldIDsSorted[i], out list))
			{
				for (int j = 0; j < list.Count; j++)
				{
					FetchChore fetchChore = list[j];
					if ((fetchChore.choreType == storageFetch || fetchChore.choreType == foodFetch) && fetchChore.destination)
					{
						int cell = Grid.PosToCell(fetchChore.destination);
						if (MinionGroupProber.Get().IsReachable(cell, fetchChore.destination.GetOffsets(cell)))
						{
							this.storageFetchableTags.UnionWith(fetchChore.tags);
						}
					}
				}
			}
		}
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x000934E8 File Offset: 0x000916E8
	public bool ClearableHasDestination(Pickupable pickupable)
	{
		KPrefabID kprefabID = pickupable.KPrefabID;
		return this.storageFetchableTags.Contains(kprefabID.PrefabTag);
	}

	// Token: 0x04000F5A RID: 3930
	public static GlobalChoreProvider Instance;

	// Token: 0x04000F5B RID: 3931
	public Dictionary<int, List<FetchChore>> fetchMap = new Dictionary<int, List<FetchChore>>();

	// Token: 0x04000F5C RID: 3932
	public List<GlobalChoreProvider.Fetch> fetches = new List<GlobalChoreProvider.Fetch>();

	// Token: 0x04000F5D RID: 3933
	private static readonly GlobalChoreProvider.FetchComparer Comparer = new GlobalChoreProvider.FetchComparer();

	// Token: 0x04000F5E RID: 3934
	private ClearableManager clearableManager;

	// Token: 0x04000F5F RID: 3935
	private HashSet<Tag> storageFetchableTags = new HashSet<Tag>();

	// Token: 0x04000F60 RID: 3936
	private static GlobalChoreProvider.GlobalChoreProviderMultithreader batch_context = new GlobalChoreProvider.GlobalChoreProviderMultithreader();

	// Token: 0x04000F61 RID: 3937
	private static WorkItemCollection<MultithreadedCollectChoreContext<GlobalChoreProvider>.WorkBlock<GlobalChoreProvider.GlobalChoreProviderMultithreader>, GlobalChoreProvider.GlobalChoreProviderMultithreader> batch_work_items = new WorkItemCollection<MultithreadedCollectChoreContext<GlobalChoreProvider>.WorkBlock<GlobalChoreProvider.GlobalChoreProviderMultithreader>, GlobalChoreProvider.GlobalChoreProviderMultithreader>();

	// Token: 0x0200134B RID: 4939
	public struct Fetch
	{
		// Token: 0x06008B81 RID: 35713 RVA: 0x0035F730 File Offset: 0x0035D930
		public bool IsBetterThan(GlobalChoreProvider.Fetch fetch)
		{
			if (this.category != fetch.category)
			{
				return false;
			}
			if (this.idsHash != fetch.idsHash)
			{
				return false;
			}
			if (this.chore.choreType != fetch.chore.choreType)
			{
				return false;
			}
			if (this.priority.priority_class > fetch.priority.priority_class)
			{
				return true;
			}
			if (this.priority.priority_class == fetch.priority.priority_class)
			{
				if (this.priority.priority_value > fetch.priority.priority_value)
				{
					return true;
				}
				if (this.priority.priority_value == fetch.priority.priority_value)
				{
					return this.cost <= fetch.cost;
				}
			}
			return false;
		}

		// Token: 0x04006AD9 RID: 27353
		public FetchChore chore;

		// Token: 0x04006ADA RID: 27354
		public int idsHash;

		// Token: 0x04006ADB RID: 27355
		public int cost;

		// Token: 0x04006ADC RID: 27356
		public PrioritySetting priority;

		// Token: 0x04006ADD RID: 27357
		public Storage.FetchCategory category;
	}

	// Token: 0x0200134C RID: 4940
	private class GlobalChoreProviderMultithreader : MultithreadedCollectChoreContext<GlobalChoreProvider>
	{
		// Token: 0x06008B82 RID: 35714 RVA: 0x0035F7EE File Offset: 0x0035D9EE
		public override void CollectChore(int index, List<Chore.Precondition.Context> succeed, List<Chore.Precondition.Context> incomplete, List<Chore.Precondition.Context> failed)
		{
			this.provider.fetches[index].chore.CollectChoresFromGlobalChoreProvider(this.consumerState, succeed, incomplete, failed, false);
		}
	}

	// Token: 0x0200134D RID: 4941
	private class FetchComparer : IComparer<GlobalChoreProvider.Fetch>
	{
		// Token: 0x06008B84 RID: 35716 RVA: 0x0035F820 File Offset: 0x0035DA20
		public int Compare(GlobalChoreProvider.Fetch a, GlobalChoreProvider.Fetch b)
		{
			int num = b.priority.priority_class - a.priority.priority_class;
			if (num != 0)
			{
				return num;
			}
			int num2 = b.priority.priority_value - a.priority.priority_value;
			if (num2 != 0)
			{
				return num2;
			}
			return a.cost - b.cost;
		}
	}

	// Token: 0x0200134E RID: 4942
	private struct FindTopPriorityTask : IWorkItem<object>
	{
		// Token: 0x06008B86 RID: 35718 RVA: 0x0035F87C File Offset: 0x0035DA7C
		public FindTopPriorityTask(int start, int end, List<Prioritizable> worldCollection)
		{
			this.start = start;
			this.end = end;
			this.worldCollection = worldCollection;
			this.found = false;
		}

		// Token: 0x06008B87 RID: 35719 RVA: 0x0035F89C File Offset: 0x0035DA9C
		public void Run(object context, int threadIndex)
		{
			if (GlobalChoreProvider.FindTopPriorityTask.abort)
			{
				return;
			}
			int num = this.start;
			while (num != this.end && this.worldCollection.Count > num)
			{
				if (!(this.worldCollection[num] == null) && this.worldCollection[num].IsTopPriority())
				{
					this.found = true;
					break;
				}
				num++;
			}
			if (this.found)
			{
				GlobalChoreProvider.FindTopPriorityTask.abort = true;
			}
		}

		// Token: 0x04006ADE RID: 27358
		private int start;

		// Token: 0x04006ADF RID: 27359
		private int end;

		// Token: 0x04006AE0 RID: 27360
		private List<Prioritizable> worldCollection;

		// Token: 0x04006AE1 RID: 27361
		public bool found;

		// Token: 0x04006AE2 RID: 27362
		public static bool abort;
	}
}
