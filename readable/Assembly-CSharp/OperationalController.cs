using System;

// Token: 0x0200005A RID: 90
public class OperationalController : GameStateMachine<OperationalController, OperationalController.Instance>
{
	// Token: 0x060001BD RID: 445 RVA: 0x0000C180 File Offset: 0x0000A380
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventTransition(GameHashes.OperationalChanged, this.off, (OperationalController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.working_pre, (OperationalController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.working_pst, (OperationalController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x0400010E RID: 270
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x0400010F RID: 271
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x04000110 RID: 272
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x04000111 RID: 273
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x0200108C RID: 4236
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200108D RID: 4237
	public new class Instance : GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008259 RID: 33369 RVA: 0x00341D25 File Offset: 0x0033FF25
		public Instance(IStateMachineTarget master, OperationalController.Def def) : base(master, def)
		{
		}
	}
}
