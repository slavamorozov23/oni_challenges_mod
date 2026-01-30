using System;

// Token: 0x020005E0 RID: 1504
public class FallWhenDeadMonitor : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance>
{
	// Token: 0x060022D8 RID: 8920 RVA: 0x000CB144 File Offset: 0x000C9344
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.standing;
		this.standing.Transition(this.entombed, (FallWhenDeadMonitor.Instance smi) => smi.IsEntombed(), UpdateRate.SIM_200ms).Transition(this.falling, (FallWhenDeadMonitor.Instance smi) => smi.IsFalling(), UpdateRate.SIM_200ms);
		this.falling.ToggleGravity(this.standing);
		this.entombed.Transition(this.standing, (FallWhenDeadMonitor.Instance smi) => !smi.IsEntombed(), UpdateRate.SIM_200ms);
	}

	// Token: 0x04001467 RID: 5223
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State standing;

	// Token: 0x04001468 RID: 5224
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State falling;

	// Token: 0x04001469 RID: 5225
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State entombed;

	// Token: 0x020014BE RID: 5310
	public new class Instance : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060090E8 RID: 37096 RVA: 0x0036FD5B File Offset: 0x0036DF5B
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060090E9 RID: 37097 RVA: 0x0036FD64 File Offset: 0x0036DF64
		public bool IsEntombed()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			return component != null && component.IsEntombed;
		}

		// Token: 0x060090EA RID: 37098 RVA: 0x0036FD8C File Offset: 0x0036DF8C
		public bool IsFalling()
		{
			int num = Grid.CellBelow(Grid.PosToCell(base.master.transform.GetPosition()));
			return Grid.IsValidCell(num) && !Grid.Solid[num];
		}
	}
}
