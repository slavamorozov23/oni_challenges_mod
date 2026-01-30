using System;
using TUNING;

// Token: 0x02000A39 RID: 2617
public class PeeChoreMonitor : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance>
{
	// Token: 0x06004C66 RID: 19558 RVA: 0x001BBE0C File Offset: 0x001BA00C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.building;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.building.Update(delegate(PeeChoreMonitor.Instance smi, float dt)
		{
			this.pee_fuse.Delta(-dt, smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.paused, (PeeChoreMonitor.Instance smi) => this.IsSleeping(smi), UpdateRate.SIM_200ms).Transition(this.critical, (PeeChoreMonitor.Instance smi) => this.pee_fuse.Get(smi) <= 60f, UpdateRate.SIM_200ms);
		this.critical.Update(delegate(PeeChoreMonitor.Instance smi, float dt)
		{
			this.pee_fuse.Delta(-dt, smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.paused, (PeeChoreMonitor.Instance smi) => this.IsSleeping(smi), UpdateRate.SIM_200ms).Transition(this.pee, (PeeChoreMonitor.Instance smi) => this.pee_fuse.Get(smi) <= 0f, UpdateRate.SIM_200ms).Toggle("Components", delegate(PeeChoreMonitor.Instance smi)
		{
			Components.CriticalBladders.Add(smi);
		}, delegate(PeeChoreMonitor.Instance smi)
		{
			Components.CriticalBladders.Remove(smi);
		});
		this.paused.Transition(this.building, (PeeChoreMonitor.Instance smi) => !this.IsSleeping(smi), UpdateRate.SIM_200ms);
		this.pee.ToggleChore(new Func<PeeChoreMonitor.Instance, Chore>(this.CreatePeeChore), this.building);
	}

	// Token: 0x06004C67 RID: 19559 RVA: 0x001BBF44 File Offset: 0x001BA144
	private bool IsSleeping(PeeChoreMonitor.Instance smi)
	{
		StaminaMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StaminaMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.IsSleeping();
		}
		return false;
	}

	// Token: 0x06004C68 RID: 19560 RVA: 0x001BBF6D File Offset: 0x001BA16D
	private Chore CreatePeeChore(PeeChoreMonitor.Instance smi)
	{
		return new PeeChore(smi.master);
	}

	// Token: 0x040032B8 RID: 12984
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State building;

	// Token: 0x040032B9 RID: 12985
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State critical;

	// Token: 0x040032BA RID: 12986
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State paused;

	// Token: 0x040032BB RID: 12987
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State pee;

	// Token: 0x040032BC RID: 12988
	private StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter pee_fuse = new StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter(DUPLICANTSTATS.STANDARD.Secretions.PEE_FUSE_TIME);

	// Token: 0x02001B11 RID: 6929
	public new class Instance : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A845 RID: 43077 RVA: 0x003BEAD5 File Offset: 0x003BCCD5
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A846 RID: 43078 RVA: 0x003BEADE File Offset: 0x003BCCDE
		public bool IsCritical()
		{
			return base.IsInsideState(base.sm.critical);
		}
	}
}
