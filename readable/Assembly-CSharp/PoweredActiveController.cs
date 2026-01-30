using System;

// Token: 0x0200005B RID: 91
public class PoweredActiveController : GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>
{
	// Token: 0x060001BF RID: 447 RVA: 0x0000C28C File Offset: 0x0000A48C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (PoweredActiveController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (PoweredActiveController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pre, (PoweredActiveController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working.Enter(delegate(PoweredActiveController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.Working, null);
			}
		}).Exit(delegate(PoweredActiveController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.Working, false);
			}
		});
		this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
		this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.working.pst, (PoweredActiveController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pst, (PoweredActiveController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on);
	}

	// Token: 0x04000112 RID: 274
	public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State off;

	// Token: 0x04000113 RID: 275
	public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State on;

	// Token: 0x04000114 RID: 276
	public PoweredActiveController.WorkingStates working;

	// Token: 0x0200108F RID: 4239
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040062BE RID: 25278
		public bool showWorkingStatus;
	}

	// Token: 0x02001090 RID: 4240
	public class WorkingStates : GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State
	{
		// Token: 0x040062BF RID: 25279
		public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State pre;

		// Token: 0x040062C0 RID: 25280
		public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State loop;

		// Token: 0x040062C1 RID: 25281
		public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State pst;
	}

	// Token: 0x02001091 RID: 4241
	public new class Instance : GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.GameInstance
	{
		// Token: 0x06008261 RID: 33377 RVA: 0x00341D80 File Offset: 0x0033FF80
		public Instance(IStateMachineTarget master, PoweredActiveController.Def def) : base(master, def)
		{
		}
	}
}
