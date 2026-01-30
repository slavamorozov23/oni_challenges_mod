using System;

// Token: 0x02000625 RID: 1573
public class ReachabilityMonitor : GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable>
{
	// Token: 0x06002571 RID: 9585 RVA: 0x000D6F50 File Offset: 0x000D5150
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unreachable;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.FastUpdate("UpdateReachability", ReachabilityMonitor.updateReachabilityCB, UpdateRate.SIM_1000ms, true);
		this.reachable.Enter(delegate(ReachabilityMonitor.Instance smi)
		{
			smi.Get<KPrefabID>().AddTag(GameTags.Reachable, false);
		}).Exit(delegate(ReachabilityMonitor.Instance smi)
		{
			smi.Get<KPrefabID>().RemoveTag(GameTags.Reachable);
		}).Enter("TriggerEvent", delegate(ReachabilityMonitor.Instance smi)
		{
			smi.TriggerEvent();
		}).ParamTransition<bool>(this.isReachable, this.unreachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsFalse);
		this.unreachable.Enter("TriggerEvent", delegate(ReachabilityMonitor.Instance smi)
		{
			smi.TriggerEvent();
		}).ParamTransition<bool>(this.isReachable, this.reachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsTrue);
	}

	// Token: 0x040015F2 RID: 5618
	public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State reachable;

	// Token: 0x040015F3 RID: 5619
	public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State unreachable;

	// Token: 0x040015F4 RID: 5620
	public StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter isReachable = new StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter(false);

	// Token: 0x040015F5 RID: 5621
	private static ReachabilityMonitor.UpdateReachabilityCB updateReachabilityCB = new ReachabilityMonitor.UpdateReachabilityCB();

	// Token: 0x020014F6 RID: 5366
	private class UpdateReachabilityCB : UpdateBucketWithUpdater<ReachabilityMonitor.Instance>.IUpdater
	{
		// Token: 0x060091A7 RID: 37287 RVA: 0x00371998 File Offset: 0x0036FB98
		public void Update(ReachabilityMonitor.Instance smi, float dt)
		{
			smi.UpdateReachability();
		}
	}

	// Token: 0x020014F7 RID: 5367
	public new class Instance : GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.GameInstance
	{
		// Token: 0x060091A9 RID: 37289 RVA: 0x003719A8 File Offset: 0x0036FBA8
		public Instance(Workable workable) : base(workable)
		{
			this.UpdateReachability();
		}

		// Token: 0x060091AA RID: 37290 RVA: 0x003719B7 File Offset: 0x0036FBB7
		public void TriggerEvent()
		{
			base.Trigger(-1432940121, BoxedBools.Box(base.sm.isReachable.Get(base.smi)));
		}

		// Token: 0x060091AB RID: 37291 RVA: 0x003719E0 File Offset: 0x0036FBE0
		public void UpdateReachability()
		{
			if (base.master == null)
			{
				return;
			}
			int cell = base.master.GetCell();
			base.sm.isReachable.Set(MinionGroupProber.Get().IsAllReachable(cell, base.master.GetOffsets(cell)), base.smi, false);
		}
	}
}
