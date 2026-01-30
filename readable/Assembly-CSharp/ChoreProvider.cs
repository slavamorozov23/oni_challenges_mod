using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreProvider")]
public class ChoreProvider : KMonoBehaviour
{
	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06001A8C RID: 6796 RVA: 0x0009266B File Offset: 0x0009086B
	// (set) Token: 0x06001A8D RID: 6797 RVA: 0x00092673 File Offset: 0x00090873
	public string Name { get; private set; }

	// Token: 0x06001A8E RID: 6798 RVA: 0x0009267C File Offset: 0x0009087C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.worldParentChangedHandle = Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		this.minionMigrationHandle = Game.Instance.Subscribe(586301400, new Action<object>(this.OnMinionMigrated));
		this.enitityMigrationHandle = Game.Instance.Subscribe(1142724171, new Action<object>(this.OnEntityMigrated));
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x000926F8 File Offset: 0x000908F8
	protected override void OnSpawn()
	{
		if (ClusterManager.Instance != null)
		{
			this.worldRemovedHandle = ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		}
		base.OnSpawn();
		this.Name = base.name;
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x00092748 File Offset: 0x00090948
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(ref this.worldParentChangedHandle);
		Game.Instance.Unsubscribe(ref this.minionMigrationHandle);
		Game.Instance.Unsubscribe(ref this.enitityMigrationHandle);
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Unsubscribe(ref this.worldRemovedHandle);
		}
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000927A8 File Offset: 0x000909A8
	protected virtual void OnWorldRemoved(object data)
	{
		int value = ((Boxed<int>)data).value;
		int parentWorldId = ClusterManager.Instance.GetWorld(value).ParentWorldId;
		List<Chore> chores;
		if (this.choreWorldMap.TryGetValue(parentWorldId, out chores))
		{
			this.ClearWorldChores<Chore>(chores, value);
		}
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x000927EC File Offset: 0x000909EC
	protected virtual void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		List<Chore> oldChores;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255 || worldParentChangedEventArgs.lastParentId == worldParentChangedEventArgs.world.ParentWorldId || !this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x00092894 File Offset: 0x00090A94
	protected virtual void OnEntityMigrated(object data)
	{
		MigrationEventArgs migrationEventArgs = data as MigrationEventArgs;
		List<Chore> oldChores;
		if (migrationEventArgs == null || !(migrationEventArgs.entity == base.gameObject) || migrationEventArgs.prevWorldId == migrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(migrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(migrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[migrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, migrationEventArgs.targetWorldId);
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x00092928 File Offset: 0x00090B28
	protected virtual void OnMinionMigrated(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		List<Chore> oldChores;
		if (minionMigrationEventArgs == null || !(minionMigrationEventArgs.minionId.gameObject == base.gameObject) || minionMigrationEventArgs.prevWorldId == minionMigrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(minionMigrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(minionMigrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[minionMigrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, minionMigrationEventArgs.targetWorldId);
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x000929C4 File Offset: 0x00090BC4
	protected void TransferChores<T>(List<T> oldChores, List<T> newChores, int transferId) where T : Chore
	{
		int num = oldChores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			T t = oldChores[i];
			if (t.isNull)
			{
				DebugUtil.DevLogError(string.Concat(new string[]
				{
					"[",
					t.GetType().Name,
					"] ",
					t.GetReportName(null),
					" has no target"
				}));
			}
			else if (t.gameObject.GetMyParentWorldId() == transferId)
			{
				newChores.Add(t);
				oldChores[i] = oldChores[num];
				oldChores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x00092A80 File Offset: 0x00090C80
	protected void ClearWorldChores<T>(List<T> chores, int worldId) where T : Chore
	{
		int num = chores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			if (chores[i].gameObject.GetMyWorldId() == worldId)
			{
				chores[i] = chores[num];
				chores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x00092AD4 File Offset: 0x00090CD4
	public virtual void AddChore(Chore chore)
	{
		chore.provider = this;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list = (this.choreWorldMap[myParentWorldId] = new List<Chore>());
		}
		list.Add(chore);
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x00092B20 File Offset: 0x00090D20
	public virtual void RemoveChore(Chore chore)
	{
		if (chore == null)
		{
			return;
		}
		chore.provider = null;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list.Remove(chore);
		}
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x00092B60 File Offset: 0x00090D60
	public virtual void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		List<Chore> list = null;
		int myParentWorldId = consumer_state.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i].provider == null)
			{
				list[i].Cancel("no provider");
				list[i] = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
		}
		int num = 48;
		if (list.Count > num)
		{
			ChoreProvider.batch_context.Setup(list, consumer_state);
			ChoreProvider.batch_work_items.Reset(ChoreProvider.batch_context);
			for (int j = 0; j < list.Count; j += 16)
			{
				ChoreProvider.batch_work_items.Add(new MultithreadedCollectChoreContext<List<Chore>>.WorkBlock<ChoreProvider.ChoreProviderCollectContext>(j, Math.Min(j + 16, list.Count)));
			}
			GlobalJobManager.Run(ChoreProvider.batch_work_items);
			ChoreProvider.batch_context.Finish(succeeded, failed_contexts);
			return;
		}
		foreach (Chore chore in list)
		{
			chore.CollectChores(consumer_state, succeeded, failed_contexts, false);
		}
	}

	// Token: 0x04000F4D RID: 3917
	public Dictionary<int, List<Chore>> choreWorldMap = new Dictionary<int, List<Chore>>();

	// Token: 0x04000F4E RID: 3918
	private int worldParentChangedHandle = -1;

	// Token: 0x04000F4F RID: 3919
	private int minionMigrationHandle = -1;

	// Token: 0x04000F50 RID: 3920
	private int enitityMigrationHandle = -1;

	// Token: 0x04000F51 RID: 3921
	private int worldRemovedHandle = -1;

	// Token: 0x04000F52 RID: 3922
	private static ChoreProvider.ChoreProviderCollectContext batch_context = new ChoreProvider.ChoreProviderCollectContext();

	// Token: 0x04000F53 RID: 3923
	private static WorkItemCollection<MultithreadedCollectChoreContext<List<Chore>>.WorkBlock<ChoreProvider.ChoreProviderCollectContext>, ChoreProvider.ChoreProviderCollectContext> batch_work_items = new WorkItemCollection<MultithreadedCollectChoreContext<List<Chore>>.WorkBlock<ChoreProvider.ChoreProviderCollectContext>, ChoreProvider.ChoreProviderCollectContext>();

	// Token: 0x02001346 RID: 4934
	private class ChoreProviderCollectContext : MultithreadedCollectChoreContext<List<Chore>>
	{
		// Token: 0x06008B71 RID: 35697 RVA: 0x0035F2DB File Offset: 0x0035D4DB
		public override void CollectChore(int index, List<Chore.Precondition.Context> succeed, List<Chore.Precondition.Context> incomplete, List<Chore.Precondition.Context> failed)
		{
			this.provider[index].CollectChores(this.consumerState, succeed, incomplete, failed, false);
		}
	}
}
