using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000AD5 RID: 2773
public class RemoteWorkerCapacitor : StateMachineComponent<RemoteWorkerCapacitor.StatesInstance>
{
	// Token: 0x06005097 RID: 20631 RVA: 0x001D386F File Offset: 0x001D1A6F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005098 RID: 20632 RVA: 0x001D3884 File Offset: 0x001D1A84
	public float ApplyDeltaEnergy(float delta)
	{
		float num = this.charge;
		this.charge = Mathf.Clamp(this.charge + delta, 0f, 60f);
		return this.charge - num;
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06005099 RID: 20633 RVA: 0x001D38BD File Offset: 0x001D1ABD
	public float ChargeRatio
	{
		get
		{
			return this.charge / 60f;
		}
	}

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x0600509A RID: 20634 RVA: 0x001D38CB File Offset: 0x001D1ACB
	public float Charge
	{
		get
		{
			return this.charge;
		}
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x0600509B RID: 20635 RVA: 0x001D38D3 File Offset: 0x001D1AD3
	public bool IsLowPower
	{
		get
		{
			return this.charge < 12f;
		}
	}

	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x0600509C RID: 20636 RVA: 0x001D38E2 File Offset: 0x001D1AE2
	public bool IsOutOfPower
	{
		get
		{
			return this.charge < float.Epsilon;
		}
	}

	// Token: 0x040035C4 RID: 13764
	[Serialize]
	private float charge;

	// Token: 0x040035C5 RID: 13765
	public const float LOW_LEVEL = 12f;

	// Token: 0x040035C6 RID: 13766
	public const float POWER_USE_RATE_J_PER_S = -0.1f;

	// Token: 0x040035C7 RID: 13767
	public const float POWER_CHARGE_RATE_J_PER_S = 7.5f;

	// Token: 0x040035C8 RID: 13768
	public const float CAPACITY_J = 60f;

	// Token: 0x02001C17 RID: 7191
	public class StatesInstance : GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.GameInstance
	{
		// Token: 0x0600AC6E RID: 44142 RVA: 0x003CC226 File Offset: 0x003CA426
		public StatesInstance(RemoteWorkerCapacitor master) : base(master)
		{
		}
	}

	// Token: 0x02001C18 RID: 7192
	public class States : GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor>
	{
		// Token: 0x0600AC6F RID: 44143 RVA: 0x003CC230 File Offset: 0x003CA430
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerCapacitorStatus, (RemoteWorkerCapacitor.StatesInstance smi) => smi.master);
			this.ok.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower), UpdateRate.SIM_200ms).Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower), UpdateRate.SIM_200ms);
			this.low_power.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowPower, null);
			this.out_of_power.Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfPower, null);
		}

		// Token: 0x0600AC70 RID: 44144 RVA: 0x003CC355 File Offset: 0x003CA555
		public static bool IsOkForPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return !smi.master.IsLowPower;
		}

		// Token: 0x0600AC71 RID: 44145 RVA: 0x003CC365 File Offset: 0x003CA565
		public static bool IsLowPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return smi.master.IsLowPower && !smi.master.IsOutOfPower;
		}

		// Token: 0x0600AC72 RID: 44146 RVA: 0x003CC384 File Offset: 0x003CA584
		public static bool IsOutOfPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return smi.master.IsOutOfPower;
		}

		// Token: 0x040086E2 RID: 34530
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State ok;

		// Token: 0x040086E3 RID: 34531
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State low_power;

		// Token: 0x040086E4 RID: 34532
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State out_of_power;
	}
}
