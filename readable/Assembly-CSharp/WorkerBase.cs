using System;
using Klei.AI;

// Token: 0x02000676 RID: 1654
public abstract class WorkerBase : KMonoBehaviour
{
	// Token: 0x06002876 RID: 10358
	public abstract bool UsesMultiTool();

	// Token: 0x06002877 RID: 10359
	public abstract bool IsFetchDrone();

	// Token: 0x06002878 RID: 10360
	public abstract KBatchedAnimController GetAnimController();

	// Token: 0x06002879 RID: 10361
	public abstract WorkerBase.State GetState();

	// Token: 0x0600287A RID: 10362
	public abstract WorkerBase.StartWorkInfo GetStartWorkInfo();

	// Token: 0x0600287B RID: 10363
	public abstract Workable GetWorkable();

	// Token: 0x0600287C RID: 10364
	public abstract Attributes GetAttributes();

	// Token: 0x0600287D RID: 10365
	public abstract AttributeConverterInstance GetAttributeConverter(string id);

	// Token: 0x0600287E RID: 10366
	public abstract Guid OfferStatusItem(StatusItem item, object data = null);

	// Token: 0x0600287F RID: 10367
	public abstract void RevokeStatusItem(Guid id);

	// Token: 0x06002880 RID: 10368
	public abstract void StartWork(WorkerBase.StartWorkInfo start_work_info);

	// Token: 0x06002881 RID: 10369
	public abstract void StopWork();

	// Token: 0x06002882 RID: 10370
	public abstract bool InstantlyFinish();

	// Token: 0x06002883 RID: 10371
	public abstract WorkerBase.WorkResult Work(float dt);

	// Token: 0x06002884 RID: 10372
	public abstract void SetWorkCompleteData(object data);

	// Token: 0x0200154D RID: 5453
	public class StartWorkInfo
	{
		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060092E4 RID: 37604 RVA: 0x00374C53 File Offset: 0x00372E53
		// (set) Token: 0x060092E5 RID: 37605 RVA: 0x00374C5B File Offset: 0x00372E5B
		public Workable workable { get; set; }

		// Token: 0x060092E6 RID: 37606 RVA: 0x00374C64 File Offset: 0x00372E64
		public StartWorkInfo(Workable workable)
		{
			this.workable = workable;
		}
	}

	// Token: 0x0200154E RID: 5454
	public enum State
	{
		// Token: 0x04007170 RID: 29040
		Idle,
		// Token: 0x04007171 RID: 29041
		Working,
		// Token: 0x04007172 RID: 29042
		PendingCompletion,
		// Token: 0x04007173 RID: 29043
		Completing
	}

	// Token: 0x0200154F RID: 5455
	public enum WorkResult
	{
		// Token: 0x04007175 RID: 29045
		Success,
		// Token: 0x04007176 RID: 29046
		InProgress,
		// Token: 0x04007177 RID: 29047
		Failed
	}
}
