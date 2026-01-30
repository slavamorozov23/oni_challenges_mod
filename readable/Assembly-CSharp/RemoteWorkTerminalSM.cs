using System;

// Token: 0x02000ACD RID: 2765
public class RemoteWorkTerminalSM : StateMachineComponent<RemoteWorkTerminalSM.StatesInstance>
{
	// Token: 0x0600507C RID: 20604 RVA: 0x001D3723 File Offset: 0x001D1923
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x040035B8 RID: 13752
	[MyCmpGet]
	private RemoteWorkTerminal terminal;

	// Token: 0x040035B9 RID: 13753
	[MyCmpGet]
	private Operational operational;

	// Token: 0x02001C11 RID: 7185
	public class StatesInstance : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.GameInstance
	{
		// Token: 0x0600AC5C RID: 44124 RVA: 0x003CBE56 File Offset: 0x003CA056
		public StatesInstance(RemoteWorkTerminalSM master) : base(master)
		{
		}
	}

	// Token: 0x02001C12 RID: 7186
	public class States : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM>
	{
		// Token: 0x0600AC5D RID: 44125 RVA: 0x003CBE60 File Offset: 0x003CA060
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.offline;
			this.offline.Transition(this.online, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational)), UpdateRate.SIM_200ms).Transition(this.offline.no_dock, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock)), UpdateRate.SIM_200ms);
			this.offline.no_dock.Transition(this.offline, new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().BuildingStatusItems.RemoteWorkTerminalNoDock, null);
			this.online.ToggleRecurringChore(new Func<RemoteWorkTerminalSM.StatesInstance, Chore>(RemoteWorkTerminalSM.States.CreateChore), null).Transition(this.offline, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational))), UpdateRate.SIM_200ms);
		}

		// Token: 0x0600AC5E RID: 44126 RVA: 0x003CBF47 File Offset: 0x003CA147
		public static bool IsOperational(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return smi.master.operational.IsOperational;
		}

		// Token: 0x0600AC5F RID: 44127 RVA: 0x003CBF59 File Offset: 0x003CA159
		public static bool HasAssignedDock(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return smi.master.terminal.CurrentDock != null;
		}

		// Token: 0x0600AC60 RID: 44128 RVA: 0x003CBF71 File Offset: 0x003CA171
		public static Chore CreateChore(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return new RemoteChore(smi.master.terminal);
		}

		// Token: 0x040086DA RID: 34522
		public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State online;

		// Token: 0x040086DB RID: 34523
		public RemoteWorkTerminalSM.States.OfflineStates offline;

		// Token: 0x02002A10 RID: 10768
		public class OfflineStates : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State
		{
			// Token: 0x0400B9F7 RID: 47607
			public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State no_dock;
		}
	}
}
