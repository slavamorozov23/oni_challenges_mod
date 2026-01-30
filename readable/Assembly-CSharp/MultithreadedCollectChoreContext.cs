using System;
using System.Collections.Generic;

// Token: 0x020004CD RID: 1229
public abstract class MultithreadedCollectChoreContext<ProviderType>
{
	// Token: 0x060019FF RID: 6655 RVA: 0x0009083B File Offset: 0x0008EA3B
	public MultithreadedCollectChoreContext()
	{
	}

	// Token: 0x06001A00 RID: 6656 RVA: 0x00090843 File Offset: 0x0008EA43
	public void Setup(ProviderType provider, ChoreConsumerState consumerState)
	{
		this.provider = provider;
		this.consumerState = consumerState;
		if (this.succeeded == null || this.succeeded.Length != GlobalJobManager.ThreadCount)
		{
			this.SetupThreadContext();
		}
	}

	// Token: 0x06001A01 RID: 6657 RVA: 0x00090870 File Offset: 0x0008EA70
	private void SetupThreadContext()
	{
		if (this.succeeded != null)
		{
			this.TearDownThreadContext();
		}
		int threadCount = GlobalJobManager.ThreadCount;
		this.succeeded = new ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[threadCount];
		this.failed = new ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[threadCount];
		this.incomplete = new ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[threadCount];
		for (int i = 0; i < threadCount; i++)
		{
			this.succeeded[i] = ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.Allocate();
			this.failed[i] = ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.Allocate();
			this.incomplete[i] = ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.Allocate();
		}
	}

	// Token: 0x06001A02 RID: 6658 RVA: 0x000908E8 File Offset: 0x0008EAE8
	private void TearDownThreadContext()
	{
		int threadCount = GlobalJobManager.ThreadCount;
		for (int i = 0; i < threadCount; i++)
		{
			this.succeeded[i].Recycle();
			this.failed[i].Recycle();
			this.incomplete[i].Recycle();
		}
		this.succeeded = null;
		this.failed = null;
		this.incomplete = null;
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x00090944 File Offset: 0x0008EB44
	public void Finish(List<Chore.Precondition.Context> pass, List<Chore.Precondition.Context> fail)
	{
		int threadCount = GlobalJobManager.ThreadCount;
		for (int i = 0; i < threadCount; i++)
		{
			pass.AddRange(this.succeeded[i]);
			this.succeeded[i].Clear();
			fail.AddRange(this.failed[i]);
			this.failed[i].Clear();
			foreach (Chore.Precondition.Context item in this.incomplete[i])
			{
				item.FinishPreconditions();
				if (item.IsSuccess())
				{
					pass.Add(item);
				}
				else
				{
					fail.Add(item);
				}
			}
			this.incomplete[i].Clear();
		}
	}

	// Token: 0x06001A04 RID: 6660
	public abstract void CollectChore(int index, List<Chore.Precondition.Context> succeed, List<Chore.Precondition.Context> incomplete, List<Chore.Precondition.Context> failed);

	// Token: 0x06001A05 RID: 6661 RVA: 0x00090A10 File Offset: 0x0008EC10
	public void DefaultCollectChore(int index, int threadIndex)
	{
		this.CollectChore(index, this.succeeded[threadIndex], this.incomplete[threadIndex], this.failed[threadIndex]);
	}

	// Token: 0x04000EFD RID: 3837
	public ProviderType provider;

	// Token: 0x04000EFE RID: 3838
	public ChoreConsumerState consumerState;

	// Token: 0x04000EFF RID: 3839
	public ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[] succeeded;

	// Token: 0x04000F00 RID: 3840
	public ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[] failed;

	// Token: 0x04000F01 RID: 3841
	public ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[] incomplete;

	// Token: 0x0200133E RID: 4926
	public struct WorkBlock<Parent> : IWorkItem<Parent> where Parent : MultithreadedCollectChoreContext<ProviderType>
	{
		// Token: 0x06008B53 RID: 35667 RVA: 0x0035ECD9 File Offset: 0x0035CED9
		public WorkBlock(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

		// Token: 0x06008B54 RID: 35668 RVA: 0x0035ECEC File Offset: 0x0035CEEC
		void IWorkItem<!1>.Run(Parent shared_data, int threadIndex)
		{
			for (int i = this.start; i < this.end; i++)
			{
				shared_data.DefaultCollectChore(i, threadIndex);
			}
		}

		// Token: 0x04006ABA RID: 27322
		private int start;

		// Token: 0x04006ABB RID: 27323
		private int end;
	}
}
