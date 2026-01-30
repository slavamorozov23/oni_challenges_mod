using System;

// Token: 0x020005C8 RID: 1480
public class RanchableMonitor : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>
{
	// Token: 0x060021F2 RID: 8690 RVA: 0x000C50A1 File Offset: 0x000C32A1
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetRanched, (RanchableMonitor.Instance smi) => smi.ShouldGoGetRanched(), null);
	}

	// Token: 0x0200148B RID: 5259
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200148C RID: 5260
	public new class Instance : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>.GameInstance
	{
		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06009030 RID: 36912 RVA: 0x0036DE38 File Offset: 0x0036C038
		// (set) Token: 0x06009031 RID: 36913 RVA: 0x0036DE40 File Offset: 0x0036C040
		public ChoreConsumer ChoreConsumer { get; private set; }

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06009032 RID: 36914 RVA: 0x0036DE49 File Offset: 0x0036C049
		public Navigator NavComponent
		{
			get
			{
				return this.navComponent;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06009033 RID: 36915 RVA: 0x0036DE51 File Offset: 0x0036C051
		public RanchedStates.Instance States
		{
			get
			{
				if (this.states == null)
				{
					this.states = this.controller.GetSMI<RanchedStates.Instance>();
				}
				return this.states;
			}
		}

		// Token: 0x06009034 RID: 36916 RVA: 0x0036DE72 File Offset: 0x0036C072
		public Instance(IStateMachineTarget master, RanchableMonitor.Def def) : base(master, def)
		{
			this.ChoreConsumer = base.GetComponent<ChoreConsumer>();
			this.navComponent = base.GetComponent<Navigator>();
		}

		// Token: 0x06009035 RID: 36917 RVA: 0x0036DE94 File Offset: 0x0036C094
		public bool ShouldGoGetRanched()
		{
			return this.TargetRanchStation != null && this.TargetRanchStation.IsRunning() && this.TargetRanchStation.IsRancherReady;
		}

		// Token: 0x04006EEB RID: 28395
		public RanchStation.Instance TargetRanchStation;

		// Token: 0x04006EEC RID: 28396
		private Navigator navComponent;

		// Token: 0x04006EED RID: 28397
		private RanchedStates.Instance states;
	}
}
