using System;

// Token: 0x02000A2E RID: 2606
public class IdleMonitor : GameStateMachine<IdleMonitor, IdleMonitor.Instance>
{
	// Token: 0x06004C2B RID: 19499 RVA: 0x001BAAC4 File Offset: 0x001B8CC4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.TagTransition(GameTags.Dying, this.stopped, false).ToggleRecurringChore(new Func<IdleMonitor.Instance, Chore>(this.CreateIdleChore), null);
		this.stopped.DoNothing();
	}

	// Token: 0x06004C2C RID: 19500 RVA: 0x001BAB04 File Offset: 0x001B8D04
	private Chore CreateIdleChore(IdleMonitor.Instance smi)
	{
		return new IdleChore(smi.master);
	}

	// Token: 0x04003290 RID: 12944
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04003291 RID: 12945
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State stopped;

	// Token: 0x02001AF7 RID: 6903
	public new class Instance : GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A7E3 RID: 42979 RVA: 0x003BDEE6 File Offset: 0x003BC0E6
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
