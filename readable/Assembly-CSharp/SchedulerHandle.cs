using System;

// Token: 0x0200051B RID: 1307
public struct SchedulerHandle
{
	// Token: 0x06001C49 RID: 7241 RVA: 0x0009BA46 File Offset: 0x00099C46
	public SchedulerHandle(Scheduler scheduler, SchedulerEntry entry)
	{
		this.entry = entry;
		this.scheduler = scheduler;
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06001C4A RID: 7242 RVA: 0x0009BA56 File Offset: 0x00099C56
	public float TimeRemaining
	{
		get
		{
			if (!this.IsValid)
			{
				return -1f;
			}
			return this.entry.time - this.scheduler.GetTime();
		}
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x0009BA7D File Offset: 0x00099C7D
	public void FreeResources()
	{
		this.entry.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x0009BA91 File Offset: 0x00099C91
	public void ClearScheduler()
	{
		if (this.scheduler == null)
		{
			return;
		}
		this.scheduler.Clear(this);
		this.scheduler = null;
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06001C4D RID: 7245 RVA: 0x0009BAB4 File Offset: 0x00099CB4
	public bool IsValid
	{
		get
		{
			return this.scheduler != null;
		}
	}

	// Token: 0x040010A6 RID: 4262
	public SchedulerEntry entry;

	// Token: 0x040010A7 RID: 4263
	private Scheduler scheduler;
}
