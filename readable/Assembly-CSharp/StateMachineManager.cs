using System;
using System.Collections.Generic;

// Token: 0x0200053E RID: 1342
public class StateMachineManager : Singleton<StateMachineManager>, IScheduler
{
	// Token: 0x06001D01 RID: 7425 RVA: 0x0009E06D File Offset: 0x0009C26D
	public void RegisterScheduler(Scheduler scheduler)
	{
		this.scheduler = scheduler;
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x0009E076 File Offset: 0x0009C276
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x0009E08A File Offset: 0x0009C28A
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x0009E0A1 File Offset: 0x0009C2A1
	public SchedulerGroup CreateSchedulerGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x0009E0B0 File Offset: 0x0009C2B0
	public StateMachine CreateStateMachine(Type type)
	{
		StateMachine stateMachine = null;
		if (!this.stateMachines.TryGetValue(type, out stateMachine))
		{
			stateMachine = (StateMachine)Activator.CreateInstance(type);
			stateMachine.CreateStates(stateMachine);
			stateMachine.BindStates();
			stateMachine.InitializeStateMachine();
			this.stateMachines[type] = stateMachine;
			List<Action<StateMachine>> list;
			if (this.stateMachineCreatedCBs.TryGetValue(type, out list))
			{
				foreach (Action<StateMachine> action in list)
				{
					action(stateMachine);
				}
			}
		}
		return stateMachine;
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x0009E14C File Offset: 0x0009C34C
	public T CreateStateMachine<T>()
	{
		return (T)((object)this.CreateStateMachine(typeof(T)));
	}

	// Token: 0x06001D07 RID: 7431 RVA: 0x0009E164 File Offset: 0x0009C364
	public static void ResetParameters()
	{
		for (int i = 0; i < StateMachineManager.parameters.Length; i++)
		{
			StateMachineManager.parameters[i] = null;
		}
	}

	// Token: 0x06001D08 RID: 7432 RVA: 0x0009E18B File Offset: 0x0009C38B
	public StateMachine.Instance CreateSMIFromDef(IStateMachineTarget master, StateMachine.BaseDef def)
	{
		StateMachineManager.parameters[0] = master;
		StateMachineManager.parameters[1] = def;
		return (StateMachine.Instance)Activator.CreateInstance(Singleton<StateMachineManager>.Instance.CreateStateMachine(def.GetStateMachineType()).GetStateMachineInstanceType(), StateMachineManager.parameters);
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x0009E1C1 File Offset: 0x0009C3C1
	public void Clear()
	{
		if (this.scheduler != null)
		{
			this.scheduler.FreeResources();
		}
		if (this.stateMachines != null)
		{
			this.stateMachines.Clear();
		}
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x0009E1EC File Offset: 0x0009C3EC
	public void AddStateMachineCreatedCallback(Type sm_type, Action<StateMachine> cb)
	{
		List<Action<StateMachine>> list;
		if (!this.stateMachineCreatedCBs.TryGetValue(sm_type, out list))
		{
			list = new List<Action<StateMachine>>();
			this.stateMachineCreatedCBs[sm_type] = list;
		}
		list.Add(cb);
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x0009E224 File Offset: 0x0009C424
	public void RemoveStateMachineCreatedCallback(Type sm_type, Action<StateMachine> cb)
	{
		List<Action<StateMachine>> list;
		if (this.stateMachineCreatedCBs.TryGetValue(sm_type, out list))
		{
			list.Remove(cb);
		}
	}

	// Token: 0x04001108 RID: 4360
	private Scheduler scheduler;

	// Token: 0x04001109 RID: 4361
	private Dictionary<Type, StateMachine> stateMachines = new Dictionary<Type, StateMachine>();

	// Token: 0x0400110A RID: 4362
	private Dictionary<Type, List<Action<StateMachine>>> stateMachineCreatedCBs = new Dictionary<Type, List<Action<StateMachine>>>();

	// Token: 0x0400110B RID: 4363
	private static object[] parameters = new object[2];
}
