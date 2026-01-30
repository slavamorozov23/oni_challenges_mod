using System;
using UnityEngine;

// Token: 0x02000515 RID: 1301
[AddComponentMenu("KMonoBehaviour/scripts/GameScheduler")]
public class GameScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x06001C25 RID: 7205 RVA: 0x0009B698 File Offset: 0x00099898
	public static void DestroyInstance()
	{
		GameScheduler.Instance = null;
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x0009B6A0 File Offset: 0x000998A0
	protected override void OnPrefabInit()
	{
		GameScheduler.Instance = this;
		Singleton<StateMachineManager>.Instance.RegisterScheduler(this.scheduler);
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x0009B6B8 File Offset: 0x000998B8
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x0009B6CC File Offset: 0x000998CC
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x0009B6E3 File Offset: 0x000998E3
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x0009B6F0 File Offset: 0x000998F0
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x0009B704 File Offset: 0x00099904
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x0009B711 File Offset: 0x00099911
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x0400109D RID: 4253
	private Scheduler scheduler = new Scheduler(new GameScheduler.GameSchedulerClock());

	// Token: 0x0400109E RID: 4254
	public static GameScheduler Instance;

	// Token: 0x020013A5 RID: 5029
	public class GameSchedulerClock : SchedulerClock
	{
		// Token: 0x06008CA4 RID: 36004 RVA: 0x00362487 File Offset: 0x00360687
		public override float GetTime()
		{
			return GameClock.Instance.GetTime();
		}
	}
}
