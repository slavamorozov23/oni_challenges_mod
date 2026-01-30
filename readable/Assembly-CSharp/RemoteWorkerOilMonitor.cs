using System;

// Token: 0x02000AD3 RID: 2771
public class RemoteWorkerOilMonitor : StateMachineComponent<RemoteWorkerOilMonitor.StatesInstance>
{
	// Token: 0x0600508F RID: 20623 RVA: 0x001D37F9 File Offset: 0x001D19F9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06005090 RID: 20624 RVA: 0x001D380C File Offset: 0x001D1A0C
	public float Oil
	{
		get
		{
			return this.storage.GetMassAvailable(GameTags.LubricatingOil);
		}
	}

	// Token: 0x06005091 RID: 20625 RVA: 0x001D381E File Offset: 0x001D1A1E
	public float OilLevel()
	{
		return this.Oil / 20.000002f;
	}

	// Token: 0x040035BB RID: 13755
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040035BC RID: 13756
	public const float CAPACITY_KG = 20.000002f;

	// Token: 0x040035BD RID: 13757
	public const float LOW_LEVEL = 4.0000005f;

	// Token: 0x040035BE RID: 13758
	public const float FILL_RATE_KG_PER_S = 2.5000002f;

	// Token: 0x040035BF RID: 13759
	public const float CONSUMPTION_RATE_KG_PER_S = 0.033333335f;

	// Token: 0x02001C13 RID: 7187
	public class StatesInstance : GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.GameInstance
	{
		// Token: 0x0600AC62 RID: 44130 RVA: 0x003CBF8B File Offset: 0x003CA18B
		public StatesInstance(RemoteWorkerOilMonitor master) : base(master)
		{
		}
	}

	// Token: 0x02001C14 RID: 7188
	public class States : GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor>
	{
		// Token: 0x0600AC63 RID: 44131 RVA: 0x003CBF94 File Offset: 0x003CA194
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.ok.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil), UpdateRate.SIM_200ms).Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil), UpdateRate.SIM_200ms);
			this.low_oil.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowOil, null);
			this.out_of_oil.Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfOil, null);
		}

		// Token: 0x0600AC64 RID: 44132 RVA: 0x003CC07F File Offset: 0x003CA27F
		public static bool IsOkForOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil > 4.0000005f;
		}

		// Token: 0x0600AC65 RID: 44133 RVA: 0x003CC093 File Offset: 0x003CA293
		public static bool IsLowOnOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil >= float.Epsilon && smi.master.Oil < 4.0000005f;
		}

		// Token: 0x0600AC66 RID: 44134 RVA: 0x003CC0BB File Offset: 0x003CA2BB
		public static bool IsOutOfOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil < float.Epsilon;
		}

		// Token: 0x040086DC RID: 34524
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State ok;

		// Token: 0x040086DD RID: 34525
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State low_oil;

		// Token: 0x040086DE RID: 34526
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State out_of_oil;
	}
}
