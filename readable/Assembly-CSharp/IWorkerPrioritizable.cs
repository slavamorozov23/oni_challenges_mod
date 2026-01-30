using System;

// Token: 0x0200049A RID: 1178
public interface IWorkerPrioritizable
{
	// Token: 0x06001909 RID: 6409
	bool GetWorkerPriority(WorkerBase worker, out int priority);
}
