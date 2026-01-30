using System;
using Klei.AI;

// Token: 0x02000A2D RID: 2605
public class HygieneMonitor : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance>
{
	// Token: 0x06004C29 RID: 19497 RVA: 0x001BA9FC File Offset: 0x001B8BFC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.needsshower;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.clean.EventTransition(GameHashes.EffectRemoved, this.needsshower, (HygieneMonitor.Instance smi) => smi.NeedsShower());
		this.needsshower.EventTransition(GameHashes.EffectAdded, this.clean, (HygieneMonitor.Instance smi) => !smi.NeedsShower()).ToggleUrge(Db.Get().Urges.Shower).Enter(delegate(HygieneMonitor.Instance smi)
		{
			smi.SetDirtiness(1f);
		});
	}

	// Token: 0x0400328D RID: 12941
	public StateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.FloatParameter dirtiness;

	// Token: 0x0400328E RID: 12942
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State clean;

	// Token: 0x0400328F RID: 12943
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State needsshower;

	// Token: 0x02001AF5 RID: 6901
	public new class Instance : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A7DA RID: 42970 RVA: 0x003BDE5F File Offset: 0x003BC05F
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x0600A7DB RID: 42971 RVA: 0x003BDE74 File Offset: 0x003BC074
		public float GetDirtiness()
		{
			return base.sm.dirtiness.Get(this);
		}

		// Token: 0x0600A7DC RID: 42972 RVA: 0x003BDE87 File Offset: 0x003BC087
		public void SetDirtiness(float dirtiness)
		{
			base.sm.dirtiness.Set(dirtiness, this, false);
		}

		// Token: 0x0600A7DD RID: 42973 RVA: 0x003BDE9D File Offset: 0x003BC09D
		public bool NeedsShower()
		{
			return !this.effects.HasEffect(Shower.SHOWER_EFFECT);
		}

		// Token: 0x04008359 RID: 33625
		private Effects effects;
	}
}
