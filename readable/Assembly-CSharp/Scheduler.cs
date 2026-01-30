using System;
using UnityEngine;

// Token: 0x02000517 RID: 1303
public class Scheduler : IScheduler
{
	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06001C2F RID: 7215 RVA: 0x0009B731 File Offset: 0x00099931
	public int Count
	{
		get
		{
			return this.entries.Count;
		}
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x0009B73E File Offset: 0x0009993E
	public Scheduler(SchedulerClock clock)
	{
		this.clock = clock;
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x0009B763 File Offset: 0x00099963
	public float GetTime()
	{
		return this.clock.GetTime();
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x0009B770 File Offset: 0x00099970
	private SchedulerHandle Schedule(SchedulerEntry entry)
	{
		this.entries.Enqueue(entry.time, entry);
		return new SchedulerHandle(this, entry);
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x0009B78C File Offset: 0x0009998C
	private SchedulerHandle Schedule(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		SchedulerEntry entry = new SchedulerEntry(name, time + this.clock.GetTime(), time_interval, callback, callback_data, profiler_obj);
		return this.Schedule(entry);
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x0009B7BC File Offset: 0x000999BC
	public void FreeResources()
	{
		this.clock = null;
		if (this.entries != null)
		{
			while (this.entries.Count > 0)
			{
				this.entries.Dequeue().Value.FreeResources();
			}
		}
		this.entries = null;
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x0009B80C File Offset: 0x00099A0C
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		if (group != null && group.scheduler != this)
		{
			global::Debug.LogError("Scheduler group mismatch!");
		}
		SchedulerHandle schedulerHandle = this.Schedule(name, time, -1f, callback, callback_data, null);
		if (group != null)
		{
			group.Add(schedulerHandle);
		}
		return schedulerHandle;
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x0009B850 File Offset: 0x00099A50
	public void Clear(SchedulerHandle handle)
	{
		handle.entry.Clear();
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x0009B860 File Offset: 0x00099A60
	public void Update()
	{
		if (this.Count == 0)
		{
			return;
		}
		int count = this.Count;
		int num = 0;
		float time = this.clock.GetTime();
		if (this.previousTime == time)
		{
			return;
		}
		this.previousTime = time;
		while (num < count && time >= this.entries.Peek().Key)
		{
			SchedulerEntry value = this.entries.Dequeue().Value;
			if (value.callback != null)
			{
				value.callback(value.callbackData);
			}
			num++;
		}
	}

	// Token: 0x0400109F RID: 4255
	public FloatHOTQueue<SchedulerEntry> entries = new FloatHOTQueue<SchedulerEntry>();

	// Token: 0x040010A0 RID: 4256
	private SchedulerClock clock;

	// Token: 0x040010A1 RID: 4257
	private float previousTime = float.NegativeInfinity;
}
