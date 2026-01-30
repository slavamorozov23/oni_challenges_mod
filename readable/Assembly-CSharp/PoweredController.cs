using System;

// Token: 0x0200005E RID: 94
public class PoweredController : GameStateMachine<PoweredController, PoweredController.Instance>
{
	// Token: 0x060001C5 RID: 453 RVA: 0x0000C754 File Offset: 0x0000A954
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (PoweredController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (PoweredController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x0400011F RID: 287
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000120 RID: 288
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02001099 RID: 4249
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200109A RID: 4250
	public new class Instance : GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600827E RID: 33406 RVA: 0x00341F5F File Offset: 0x0034015F
		public Instance(IStateMachineTarget master, PoweredController.Def def) : base(master, def)
		{
		}

		// Token: 0x040062D7 RID: 25303
		public bool ShowWorkingStatus;
	}
}
