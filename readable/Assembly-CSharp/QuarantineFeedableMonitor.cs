using System;

// Token: 0x02000A3B RID: 2619
public class QuarantineFeedableMonitor : GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance>
{
	// Token: 0x06004C78 RID: 19576 RVA: 0x001BC14C File Offset: 0x001BA34C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.EventTransition(GameHashes.AddUrge, this.hungry, (QuarantineFeedableMonitor.Instance smi) => smi.IsHungry());
		this.hungry.EventTransition(GameHashes.RemoveUrge, this.satisfied, (QuarantineFeedableMonitor.Instance smi) => !smi.IsHungry());
	}

	// Token: 0x040032C2 RID: 12994
	public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040032C3 RID: 12995
	public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State hungry;

	// Token: 0x02001B16 RID: 6934
	public new class Instance : GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A852 RID: 43090 RVA: 0x003BEBF6 File Offset: 0x003BCDF6
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A853 RID: 43091 RVA: 0x003BEBFF File Offset: 0x003BCDFF
		public bool IsHungry()
		{
			return base.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Eat);
		}
	}
}
