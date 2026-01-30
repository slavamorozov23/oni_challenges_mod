using System;
using System.Collections.Generic;
using UnityEngine.Pool;

// Token: 0x02000AB5 RID: 2741
public abstract class ProcessCondition
{
	// Token: 0x06004FA3 RID: 20387
	public abstract ProcessCondition.Status EvaluateCondition();

	// Token: 0x06004FA4 RID: 20388
	public abstract bool ShowInUI();

	// Token: 0x06004FA5 RID: 20389
	public abstract string GetStatusMessage(ProcessCondition.Status status);

	// Token: 0x06004FA6 RID: 20390 RVA: 0x001CE5CA File Offset: 0x001CC7CA
	public string GetStatusMessage()
	{
		return this.GetStatusMessage(this.EvaluateCondition());
	}

	// Token: 0x06004FA7 RID: 20391
	public abstract string GetStatusTooltip(ProcessCondition.Status status);

	// Token: 0x06004FA8 RID: 20392 RVA: 0x001CE5D8 File Offset: 0x001CC7D8
	public string GetStatusTooltip()
	{
		return this.GetStatusTooltip(this.EvaluateCondition());
	}

	// Token: 0x06004FA9 RID: 20393 RVA: 0x001CE5E6 File Offset: 0x001CC7E6
	public virtual StatusItem GetStatusItem(ProcessCondition.Status status)
	{
		return null;
	}

	// Token: 0x06004FAA RID: 20394 RVA: 0x001CE5E9 File Offset: 0x001CC7E9
	public virtual ProcessCondition GetParentCondition()
	{
		return this.parentCondition;
	}

	// Token: 0x04003538 RID: 13624
	protected ProcessCondition parentCondition;

	// Token: 0x04003539 RID: 13625
	public static ObjectPool<List<ProcessCondition>> ListPool = new ObjectPool<List<ProcessCondition>>(() => new List<ProcessCondition>(16), null, delegate(List<ProcessCondition> list)
	{
		list.Clear();
	}, null, false, 4, 4);

	// Token: 0x02001BFA RID: 7162
	public enum ProcessConditionType
	{
		// Token: 0x0400867E RID: 34430
		RocketFlight,
		// Token: 0x0400867F RID: 34431
		RocketPrep,
		// Token: 0x04008680 RID: 34432
		RocketStorage,
		// Token: 0x04008681 RID: 34433
		RocketBoard,
		// Token: 0x04008682 RID: 34434
		All
	}

	// Token: 0x02001BFB RID: 7163
	public enum Status
	{
		// Token: 0x04008684 RID: 34436
		Failure,
		// Token: 0x04008685 RID: 34437
		Warning,
		// Token: 0x04008686 RID: 34438
		Ready
	}
}
