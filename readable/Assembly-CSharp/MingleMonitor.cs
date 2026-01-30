using System;

// Token: 0x02000A35 RID: 2613
public class MingleMonitor : GameStateMachine<MingleMonitor, MingleMonitor.Instance>
{
	// Token: 0x06004C45 RID: 19525 RVA: 0x001BB642 File Offset: 0x001B9842
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.mingle;
		base.serializable = StateMachine.SerializeType.Never;
		this.mingle.ToggleRecurringChore(new Func<MingleMonitor.Instance, Chore>(this.CreateMingleChore), null);
	}

	// Token: 0x06004C46 RID: 19526 RVA: 0x001BB66C File Offset: 0x001B986C
	private Chore CreateMingleChore(MingleMonitor.Instance smi)
	{
		return new MingleChore(smi.master);
	}

	// Token: 0x040032AB RID: 12971
	public GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.State mingle;

	// Token: 0x02001B09 RID: 6921
	public new class Instance : GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A831 RID: 43057 RVA: 0x003BE8DF File Offset: 0x003BCADF
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
