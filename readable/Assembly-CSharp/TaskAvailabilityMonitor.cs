using System;

// Token: 0x02000A50 RID: 2640
public class TaskAvailabilityMonitor : GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance>
{
	// Token: 0x06004CDD RID: 19677 RVA: 0x001BF4B0 File Offset: 0x001BD6B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventTransition(GameHashes.NewDay, (TaskAvailabilityMonitor.Instance smi) => GameClock.Instance, this.unavailable, (TaskAvailabilityMonitor.Instance smi) => GameClock.Instance.GetCycle() > 0);
		this.unavailable.Enter("RefreshStatusItem", delegate(TaskAvailabilityMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		}).EventHandler(GameHashes.ScheduleChanged, delegate(TaskAvailabilityMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		});
	}

	// Token: 0x04003337 RID: 13111
	public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003338 RID: 13112
	public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State unavailable;

	// Token: 0x02001B53 RID: 6995
	public new class Instance : GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A966 RID: 43366 RVA: 0x003C1445 File Offset: 0x003BF645
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A967 RID: 43367 RVA: 0x003C1450 File Offset: 0x003BF650
		public void RefreshStatusItem()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			WorldContainer myWorld = base.gameObject.GetMyWorld();
			if (myWorld != null && myWorld.IsModuleInterior && myWorld.ParentWorldId == myWorld.id)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.IdleInRockets, null);
				return;
			}
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Idle, null);
		}
	}
}
