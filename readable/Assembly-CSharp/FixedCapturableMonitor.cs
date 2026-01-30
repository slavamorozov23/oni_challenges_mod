using System;

// Token: 0x020005BE RID: 1470
public class FixedCapturableMonitor : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>
{
	// Token: 0x060021C0 RID: 8640 RVA: 0x000C4378 File Offset: 0x000C2578
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetCaptured, (FixedCapturableMonitor.Instance smi) => smi.ShouldGoGetCaptured(), null).Enter(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Add(smi);
		}).Exit(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Remove(smi);
		});
	}

	// Token: 0x0200146D RID: 5229
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200146E RID: 5230
	public new class Instance : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.GameInstance
	{
		// Token: 0x06008FC3 RID: 36803 RVA: 0x0036C9FC File Offset: 0x0036ABFC
		public Instance(IStateMachineTarget master, FixedCapturableMonitor.Def def) : base(master, def)
		{
			this.ChoreConsumer = base.GetComponent<ChoreConsumer>();
			this.Navigator = base.GetComponent<Navigator>();
			this.PrefabTag = base.GetComponent<KPrefabID>().PrefabTag;
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			this.isBaby = (def2 != null);
		}

		// Token: 0x06008FC4 RID: 36804 RVA: 0x0036CA50 File Offset: 0x0036AC50
		public bool ShouldGoGetCaptured()
		{
			return this.targetCapturePoint != null && this.targetCapturePoint.IsRunning() && this.targetCapturePoint.shouldCreatureGoGetCaptured && (!this.isBaby || this.targetCapturePoint.def.allowBabies);
		}

		// Token: 0x04006E8B RID: 28299
		public FixedCapturePoint.Instance targetCapturePoint;

		// Token: 0x04006E8C RID: 28300
		public ChoreConsumer ChoreConsumer;

		// Token: 0x04006E8D RID: 28301
		public Navigator Navigator;

		// Token: 0x04006E8E RID: 28302
		public Tag PrefabTag;

		// Token: 0x04006E8F RID: 28303
		public bool isBaby;
	}
}
