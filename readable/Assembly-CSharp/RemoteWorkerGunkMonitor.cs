using System;

// Token: 0x02000AD4 RID: 2772
public class RemoteWorkerGunkMonitor : StateMachineComponent<RemoteWorkerGunkMonitor.StatesInstance>
{
	// Token: 0x06005093 RID: 20627 RVA: 0x001D3834 File Offset: 0x001D1A34
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06005094 RID: 20628 RVA: 0x001D3847 File Offset: 0x001D1A47
	public float Gunk
	{
		get
		{
			return this.storage.GetMassAvailable(SimHashes.LiquidGunk);
		}
	}

	// Token: 0x06005095 RID: 20629 RVA: 0x001D3859 File Offset: 0x001D1A59
	public float GunkLevel()
	{
		return this.Gunk / 20.000002f;
	}

	// Token: 0x040035C0 RID: 13760
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040035C1 RID: 13761
	public const float CAPACITY_KG = 20.000002f;

	// Token: 0x040035C2 RID: 13762
	public const float HIGH_LEVEL = 16.000002f;

	// Token: 0x040035C3 RID: 13763
	public const float DRAIN_AMOUNT_KG_PER_S = 3.3333337f;

	// Token: 0x02001C15 RID: 7189
	public class StatesInstance : GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.GameInstance
	{
		// Token: 0x0600AC68 RID: 44136 RVA: 0x003CC0D7 File Offset: 0x003CA2D7
		public StatesInstance(RemoteWorkerGunkMonitor master) : base(master)
		{
		}
	}

	// Token: 0x02001C16 RID: 7190
	public class States : GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor>
	{
		// Token: 0x0600AC69 RID: 44137 RVA: 0x003CC0E0 File Offset: 0x003CA2E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.ok.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk), UpdateRate.SIM_200ms).Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh), UpdateRate.SIM_200ms);
			this.high_gunk.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerHighGunkLevel, null);
			this.full_gunk.Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerFullGunkLevel, null);
		}

		// Token: 0x0600AC6A RID: 44138 RVA: 0x003CC1CB File Offset: 0x003CA3CB
		public static bool IsGunkLevelOk(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk < 16.000002f;
		}

		// Token: 0x0600AC6B RID: 44139 RVA: 0x003CC1DF File Offset: 0x003CA3DF
		public static bool IsGunkHigh(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk >= 16.000002f && smi.master.Gunk < 20.000002f;
		}

		// Token: 0x0600AC6C RID: 44140 RVA: 0x003CC207 File Offset: 0x003CA407
		public static bool IsFullOfGunk(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk >= 20.000002f;
		}

		// Token: 0x040086DF RID: 34527
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State ok;

		// Token: 0x040086E0 RID: 34528
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State high_gunk;

		// Token: 0x040086E1 RID: 34529
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State full_gunk;
	}
}
