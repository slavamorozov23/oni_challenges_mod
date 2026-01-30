using System;

// Token: 0x02000AD9 RID: 2777
internal class RemoteWorkerDockAnimSM : StateMachineComponent<RemoteWorkerDockAnimSM.StatesInstance>
{
	// Token: 0x060050CD RID: 20685 RVA: 0x001D41FB File Offset: 0x001D23FB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x040035EE RID: 13806
	[MyCmpAdd]
	private RemoteWorkerDock dock;

	// Token: 0x040035EF RID: 13807
	[MyCmpGet]
	private Operational operational;

	// Token: 0x02001C22 RID: 7202
	public class StatesInstance : GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.GameInstance
	{
		// Token: 0x0600ACA8 RID: 44200 RVA: 0x003CD6F4 File Offset: 0x003CB8F4
		public StatesInstance(RemoteWorkerDockAnimSM master) : base(master)
		{
		}
	}

	// Token: 0x02001C23 RID: 7203
	public class States : GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM>
	{
		// Token: 0x0600ACA9 RID: 44201 RVA: 0x003CD700 File Offset: 0x003CB900
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.EnterTransition(this.off.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)).EnterTransition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored))).Transition(this.on, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.IsOnline), UpdateRate.SIM_200ms);
			this.off.full.QueueAnim("off_full", false, null).Transition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)), UpdateRate.SIM_200ms);
			this.off.empty.QueueAnim("off_empty", false, null).Transition(this.off.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored), UpdateRate.SIM_200ms);
			this.on.EnterTransition(this.on.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)).EnterTransition(this.on.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored))).Transition(this.off, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.IsOnline)), UpdateRate.SIM_200ms);
			this.on.full.QueueAnim("on_full", false, null).Transition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)), UpdateRate.SIM_200ms);
			this.on.empty.QueueAnim("on_empty", false, null).Transition(this.on.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored), UpdateRate.SIM_200ms);
		}

		// Token: 0x0600ACAA RID: 44202 RVA: 0x003CD8B0 File Offset: 0x003CBAB0
		public static bool IsOnline(RemoteWorkerDockAnimSM.StatesInstance smi)
		{
			return smi.master.operational.IsOperational && smi.master.dock.RemoteWorker != null;
		}

		// Token: 0x0600ACAB RID: 44203 RVA: 0x003CD8DC File Offset: 0x003CBADC
		public static bool HasWorkerStored(RemoteWorkerDockAnimSM.StatesInstance smi)
		{
			return smi.master.dock.RemoteWorker != null && smi.master.dock.RemoteWorker.Docked;
		}

		// Token: 0x040086FB RID: 34555
		public RemoteWorkerDockAnimSM.States.FullOrEmptyState on;

		// Token: 0x040086FC RID: 34556
		public RemoteWorkerDockAnimSM.States.FullOrEmptyState off;

		// Token: 0x02002A16 RID: 10774
		public class FullOrEmptyState : GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State
		{
			// Token: 0x0400BA14 RID: 47636
			public GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State full;

			// Token: 0x0400BA15 RID: 47637
			public GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State empty;
		}
	}
}
