using System;

// Token: 0x0200005C RID: 92
public class PoweredActiveStoppableController : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance>
{
	// Token: 0x060001C1 RID: 449 RVA: 0x0000C474 File Offset: 0x0000A674
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (PoweredActiveStoppableController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.stop, (PoweredActiveStoppableController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working_pst, (PoweredActiveStoppableController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.stop.PlayAnim("stop").OnAnimQueueComplete(this.off);
	}

	// Token: 0x04000115 RID: 277
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000116 RID: 278
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x04000117 RID: 279
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x04000118 RID: 280
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x04000119 RID: 281
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State stop;

	// Token: 0x02001093 RID: 4243
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001094 RID: 4244
	public new class Instance : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600826C RID: 33388 RVA: 0x00341E46 File Offset: 0x00340046
		public Instance(IStateMachineTarget master, PoweredActiveStoppableController.Def def) : base(master, def)
		{
		}
	}
}
