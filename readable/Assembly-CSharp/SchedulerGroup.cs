using System;
using System.Collections.Generic;

// Token: 0x0200051A RID: 1306
public class SchedulerGroup
{
	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06001C43 RID: 7235 RVA: 0x0009B96E File Offset: 0x00099B6E
	// (set) Token: 0x06001C44 RID: 7236 RVA: 0x0009B976 File Offset: 0x00099B76
	public Scheduler scheduler { get; private set; }

	// Token: 0x06001C45 RID: 7237 RVA: 0x0009B97F File Offset: 0x00099B7F
	public SchedulerGroup(Scheduler scheduler)
	{
		this.scheduler = scheduler;
		this.Reset();
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x0009B99F File Offset: 0x00099B9F
	public void FreeResources()
	{
		if (this.scheduler != null)
		{
			this.scheduler.FreeResources();
		}
		this.scheduler = null;
		if (this.handles != null)
		{
			this.handles.Clear();
		}
		this.handles = null;
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x0009B9D8 File Offset: 0x00099BD8
	public void Reset()
	{
		foreach (SchedulerHandle schedulerHandle in this.handles)
		{
			schedulerHandle.ClearScheduler();
		}
		this.handles.Clear();
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x0009BA38 File Offset: 0x00099C38
	public void Add(SchedulerHandle handle)
	{
		this.handles.Add(handle);
	}

	// Token: 0x040010A5 RID: 4261
	private List<SchedulerHandle> handles = new List<SchedulerHandle>();
}
