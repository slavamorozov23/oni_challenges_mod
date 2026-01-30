using System;

// Token: 0x020008B7 RID: 2231
public class RobotIdleMonitor : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance>
{
	// Token: 0x06003D85 RID: 15749 RVA: 0x00157540 File Offset: 0x00155740
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Transition(this.working, (RobotIdleMonitor.Instance smi) => !RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
		this.working.Transition(this.idle, (RobotIdleMonitor.Instance smi) => RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x001575C4 File Offset: 0x001557C4
	private static bool CheckShouldIdle(RobotIdleMonitor.Instance smi)
	{
		FallMonitor.Instance smi2 = smi.master.gameObject.GetSMI<FallMonitor.Instance>();
		return smi2 == null || (!smi.master.gameObject.GetComponent<ChoreConsumer>().choreDriver.HasChore() && smi2.GetCurrentState() == smi2.sm.standing);
	}

	// Token: 0x040025FA RID: 9722
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040025FB RID: 9723
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State working;

	// Token: 0x020018C0 RID: 6336
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018C1 RID: 6337
	public new class Instance : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A039 RID: 41017 RVA: 0x003A9554 File Offset: 0x003A7754
		public Instance(IStateMachineTarget master, RobotIdleMonitor.Def def) : base(master)
		{
		}

		// Token: 0x04007BEA RID: 31722
		public KBatchedAnimController eyes;
	}
}
