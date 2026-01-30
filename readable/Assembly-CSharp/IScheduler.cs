using System;

// Token: 0x02000516 RID: 1302
public interface IScheduler
{
	// Token: 0x06001C2E RID: 7214
	SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null);
}
