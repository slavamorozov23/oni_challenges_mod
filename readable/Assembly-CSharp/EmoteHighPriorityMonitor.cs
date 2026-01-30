using System;

// Token: 0x02000A23 RID: 2595
public class EmoteHighPriorityMonitor : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance>
{
	// Token: 0x06004BEC RID: 19436 RVA: 0x001B9354 File Offset: 0x001B7554
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.ready;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.ready.ToggleUrge(Db.Get().Urges.EmoteHighPriority).EventHandler(GameHashes.BeginChore, delegate(EmoteHighPriorityMonitor.Instance smi, object o)
		{
			smi.OnStartChore(o);
		});
		this.resetting.GoTo(this.ready);
	}

	// Token: 0x04003255 RID: 12885
	public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State ready;

	// Token: 0x04003256 RID: 12886
	public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State resetting;

	// Token: 0x02001AD8 RID: 6872
	public new class Instance : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A74E RID: 42830 RVA: 0x003BBFD8 File Offset: 0x003BA1D8
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A74F RID: 42831 RVA: 0x003BBFE1 File Offset: 0x003BA1E1
		public void OnStartChore(object o)
		{
			if (((Chore)o).SatisfiesUrge(Db.Get().Urges.EmoteHighPriority))
			{
				this.GoTo(base.sm.resetting);
			}
		}
	}
}
