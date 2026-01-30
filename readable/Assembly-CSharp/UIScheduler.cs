using System;
using UnityEngine;

// Token: 0x02000EBD RID: 3773
[AddComponentMenu("KMonoBehaviour/scripts/UIScheduler")]
public class UIScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x060078E3 RID: 30947 RVA: 0x002E7D89 File Offset: 0x002E5F89
	public static void DestroyInstance()
	{
		UIScheduler.Instance = null;
	}

	// Token: 0x060078E4 RID: 30948 RVA: 0x002E7D91 File Offset: 0x002E5F91
	protected override void OnPrefabInit()
	{
		UIScheduler.Instance = this;
	}

	// Token: 0x060078E5 RID: 30949 RVA: 0x002E7D99 File Offset: 0x002E5F99
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x060078E6 RID: 30950 RVA: 0x002E7DAD File Offset: 0x002E5FAD
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x060078E7 RID: 30951 RVA: 0x002E7DC4 File Offset: 0x002E5FC4
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x060078E8 RID: 30952 RVA: 0x002E7DD1 File Offset: 0x002E5FD1
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x060078E9 RID: 30953 RVA: 0x002E7DE5 File Offset: 0x002E5FE5
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x060078EA RID: 30954 RVA: 0x002E7DF2 File Offset: 0x002E5FF2
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x04005443 RID: 21571
	private Scheduler scheduler = new Scheduler(new UIScheduler.UISchedulerClock());

	// Token: 0x04005444 RID: 21572
	public static UIScheduler Instance;

	// Token: 0x02002122 RID: 8482
	public class UISchedulerClock : SchedulerClock
	{
		// Token: 0x0600BB7D RID: 47997 RVA: 0x003FD4E1 File Offset: 0x003FB6E1
		public override float GetTime()
		{
			return Time.unscaledTime;
		}
	}
}
