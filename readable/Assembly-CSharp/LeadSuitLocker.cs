using System;
using TUNING;
using UnityEngine;

// Token: 0x02000787 RID: 1927
public class LeadSuitLocker : StateMachineComponent<LeadSuitLocker.StatesInstance>
{
	// Token: 0x06003125 RID: 12581 RVA: 0x0011B9EC File Offset: 0x00119BEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.o2_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_top", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_top"
		});
		this.battery_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_side", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_side"
		});
		base.smi.StartSM();
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x0011BA6C File Offset: 0x00119C6C
	public bool IsSuitFullyCharged()
	{
		return this.suit_locker.IsSuitFullyCharged();
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x0011BA79 File Offset: 0x00119C79
	public KPrefabID GetStoredOutfit()
	{
		return this.suit_locker.GetStoredOutfit();
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x0011BA88 File Offset: 0x00119C88
	private void FillBattery(float dt)
	{
		KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
		if (!component.IsFull())
		{
			component.batteryCharge += dt / this.batteryChargeTime;
		}
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x0011BAD0 File Offset: 0x00119CD0
	private void RefreshMeter()
	{
		this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
		this.battery_meter.SetPositionPercent(this.suit_locker.BatteryAvailable);
		this.anim_controller.SetSymbolVisiblity("oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
		this.anim_controller.SetSymbolVisiblity("petrol_yes_bloom", this.IsBatteryAboveMinimumLevel());
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x0011BB40 File Offset: 0x00119D40
	public bool IsOxygenTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x0011BB84 File Offset: 0x00119D84
	public bool IsBatteryAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
			return component == null || component.PercentFull() >= EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x04001D76 RID: 7542
	[MyCmpReq]
	private Building building;

	// Token: 0x04001D77 RID: 7543
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001D78 RID: 7544
	[MyCmpReq]
	private SuitLocker suit_locker;

	// Token: 0x04001D79 RID: 7545
	[MyCmpReq]
	private KBatchedAnimController anim_controller;

	// Token: 0x04001D7A RID: 7546
	private MeterController o2_meter;

	// Token: 0x04001D7B RID: 7547
	private MeterController battery_meter;

	// Token: 0x04001D7C RID: 7548
	private float batteryChargeTime = 60f;

	// Token: 0x020016A8 RID: 5800
	public class States : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker>
	{
		// Token: 0x06009817 RID: 38935 RVA: 0x00387340 File Offset: 0x00385540
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(LeadSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.EventTransition(GameHashes.OnStorageChange, this.charging, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null);
			this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).Transition(this.charged, (LeadSuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false);
			this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Update("FillBattery", delegate(LeadSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.FillBattery(dt);
			}, UpdateRate.SIM_1000ms, false);
			this.charged.EventTransition(GameHashes.OnStorageChange, this.empty, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null);
		}

		// Token: 0x0400758A RID: 30090
		public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State empty;

		// Token: 0x0400758B RID: 30091
		public LeadSuitLocker.States.ChargingStates charging;

		// Token: 0x0400758C RID: 30092
		public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State charged;

		// Token: 0x02002906 RID: 10502
		public class ChargingStates : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State
		{
			// Token: 0x0400B546 RID: 46406
			public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State notoperational;

			// Token: 0x0400B547 RID: 46407
			public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State operational;
		}
	}

	// Token: 0x020016A9 RID: 5801
	public class StatesInstance : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.GameInstance
	{
		// Token: 0x06009819 RID: 38937 RVA: 0x003874E2 File Offset: 0x003856E2
		public StatesInstance(LeadSuitLocker lead_suit_locker) : base(lead_suit_locker)
		{
		}
	}
}
