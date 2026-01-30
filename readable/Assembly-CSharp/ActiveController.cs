using System;

// Token: 0x02000057 RID: 87
public class ActiveController : GameStateMachine<ActiveController, ActiveController.Instance>
{
	// Token: 0x060001B5 RID: 437 RVA: 0x0000BE34 File Offset: 0x0000A034
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (ActiveController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.working_pst, (ActiveController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x04000105 RID: 261
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000106 RID: 262
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x04000107 RID: 263
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x04000108 RID: 264
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x02001082 RID: 4226
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001083 RID: 4227
	public new class Instance : GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008241 RID: 33345 RVA: 0x00341B94 File Offset: 0x0033FD94
		public Instance(IStateMachineTarget master, ActiveController.Def def) : base(master, def)
		{
		}
	}
}
