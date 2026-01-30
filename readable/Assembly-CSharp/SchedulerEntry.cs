using System;
using UnityEngine;

// Token: 0x02000519 RID: 1305
public struct SchedulerEntry
{
	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06001C3A RID: 7226 RVA: 0x0009B8F6 File Offset: 0x00099AF6
	// (set) Token: 0x06001C3B RID: 7227 RVA: 0x0009B8FE File Offset: 0x00099AFE
	public SchedulerEntry.Details details { readonly get; private set; }

	// Token: 0x06001C3C RID: 7228 RVA: 0x0009B907 File Offset: 0x00099B07
	public SchedulerEntry(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		this.time = time;
		this.details = new SchedulerEntry.Details(name, callback, callback_data, time_interval, profiler_obj);
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x0009B923 File Offset: 0x00099B23
	public void FreeResources()
	{
		this.details = null;
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06001C3E RID: 7230 RVA: 0x0009B92C File Offset: 0x00099B2C
	public Action<object> callback
	{
		get
		{
			return this.details.callback;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06001C3F RID: 7231 RVA: 0x0009B939 File Offset: 0x00099B39
	public object callbackData
	{
		get
		{
			return this.details.callbackData;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06001C40 RID: 7232 RVA: 0x0009B946 File Offset: 0x00099B46
	public float timeInterval
	{
		get
		{
			return this.details.timeInterval;
		}
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x0009B953 File Offset: 0x00099B53
	public override string ToString()
	{
		return this.time.ToString();
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x0009B960 File Offset: 0x00099B60
	public void Clear()
	{
		this.details.callback = null;
	}

	// Token: 0x040010A2 RID: 4258
	public float time;

	// Token: 0x020013A6 RID: 5030
	public class Details
	{
		// Token: 0x06008CA6 RID: 36006 RVA: 0x0036249B File Offset: 0x0036069B
		public Details(string name, Action<object> callback, object callback_data, float time_interval, GameObject profiler_obj)
		{
			this.timeInterval = time_interval;
			this.callback = callback;
			this.callbackData = callback_data;
		}

		// Token: 0x04006C0F RID: 27663
		public Action<object> callback;

		// Token: 0x04006C10 RID: 27664
		public object callbackData;

		// Token: 0x04006C11 RID: 27665
		public float timeInterval;
	}
}
