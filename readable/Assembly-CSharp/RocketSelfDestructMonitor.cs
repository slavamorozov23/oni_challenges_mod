using System;

// Token: 0x020008B8 RID: 2232
public class RocketSelfDestructMonitor : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance>
{
	// Token: 0x06003D88 RID: 15752 RVA: 0x00157620 File Offset: 0x00155820
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.RocketSelfDestructRequested, this.exploding, null);
		this.exploding.Update(delegate(RocketSelfDestructMonitor.Instance smi, float dt)
		{
			if (smi.timeinstate >= 3f)
			{
				smi.master.Trigger(-1311384361, null);
				smi.GoTo(this.idle);
			}
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x040025FC RID: 9724
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040025FD RID: 9725
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State exploding;

	// Token: 0x020018C3 RID: 6339
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018C4 RID: 6340
	public new class Instance : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A03F RID: 41023 RVA: 0x003A958C File Offset: 0x003A778C
		public Instance(IStateMachineTarget master, RocketSelfDestructMonitor.Def def) : base(master)
		{
		}

		// Token: 0x04007BEE RID: 31726
		public KBatchedAnimController eyes;
	}
}
