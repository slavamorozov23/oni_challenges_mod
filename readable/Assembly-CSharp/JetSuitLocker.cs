using System;
using Klei;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000783 RID: 1923
public class JetSuitLocker : StateMachineComponent<JetSuitLocker.StatesInstance>, ISecondaryInput
{
	// Token: 0x1700029C RID: 668
	// (get) Token: 0x0600310C RID: 12556 RVA: 0x0011B168 File Offset: 0x00119368
	public float FuelAvailable
	{
		get
		{
			return Math.Min((0f + this.storage.GetMassAvailable(this.fuel_tag)) / 100f, 1f);
		}
	}

	// Token: 0x0600310D RID: 12557 RVA: 0x0011B194 File Offset: 0x00119394
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.fuel_tag = GameTags.CombustibleLiquid;
		this.fuel_consumer = base.gameObject.AddComponent<ConduitConsumer>();
		this.fuel_consumer.conduitType = this.portInfo.conduitType;
		this.fuel_consumer.consumptionRate = 10f;
		this.fuel_consumer.capacityTag = GameTags.CombustibleLiquid;
		this.fuel_consumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		this.fuel_consumer.forceAlwaysSatisfied = true;
		this.fuel_consumer.capacityKG = 100f;
		this.fuel_consumer.useSecondaryInput = true;
		RequireInputs requireInputs = base.gameObject.AddComponent<RequireInputs>();
		requireInputs.conduitConsumer = this.fuel_consumer;
		requireInputs.SetRequirements(false, true);
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.portInfo.offset);
		this.secondaryInputCell = Grid.OffsetCell(cell, rotatedOffset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.flowNetworkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.secondaryInputCell, base.gameObject);
		networkManager.AddToNetworks(this.secondaryInputCell, this.flowNetworkItem, true);
		this.fuel_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_1", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_1"
		});
		this.o2_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_2", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_2"
		});
		base.smi.StartSM();
	}

	// Token: 0x0600310E RID: 12558 RVA: 0x0011B332 File Offset: 0x00119532
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInputCell, this.flowNetworkItem, true);
		base.OnCleanUp();
	}

	// Token: 0x0600310F RID: 12559 RVA: 0x0011B35C File Offset: 0x0011955C
	public bool IsSuitFullyCharged()
	{
		return this.suit_locker.IsSuitFullyCharged();
	}

	// Token: 0x06003110 RID: 12560 RVA: 0x0011B369 File Offset: 0x00119569
	public KPrefabID GetStoredOutfit()
	{
		return this.suit_locker.GetStoredOutfit();
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x0011B378 File Offset: 0x00119578
	private void FuelSuit(float dt)
	{
		KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		if (!this.HasFuel())
		{
			return;
		}
		JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
		float num = dt * 10f;
		num = Mathf.Min(num, 100f - component.amount);
		while (num > 0f && this.HasFuel())
		{
			float num2 = this.storage.GetMassAvailable(this.fuel_tag);
			num2 = Mathf.Min(num2, num);
			component.amount += num2;
			num -= num2;
			SimHashes lastFuelUsed = SimHashes.Petroleum;
			float num3;
			SimUtil.DiseaseInfo diseaseInfo;
			float num4;
			this.storage.ConsumeAndGetDisease(this.fuel_tag, num2, out num3, out diseaseInfo, out num4, out lastFuelUsed);
			component.lastFuelUsed = lastFuelUsed;
		}
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x0011B42D File Offset: 0x0011962D
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x0011B43D File Offset: 0x0011963D
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x0011B45E File Offset: 0x0011965E
	public bool HasFuel()
	{
		return this.storage.Has(this.fuel_tag) && this.storage.GetMassAvailable(this.fuel_tag) > 0f;
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x0011B490 File Offset: 0x00119690
	private void RefreshMeter()
	{
		this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
		this.fuel_meter.SetPositionPercent(this.FuelAvailable);
		this.anim_controller.SetSymbolVisiblity("oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
		this.anim_controller.SetSymbolVisiblity("petrol_yes_bloom", this.IsFuelTankAboveMinimumLevel());
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x0011B4FC File Offset: 0x001196FC
	public bool IsOxygenTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x0011B540 File Offset: 0x00119740
	public bool IsFuelTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
			return component == null || component.PercentFull() >= TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x04001D60 RID: 7520
	[MyCmpReq]
	private Building building;

	// Token: 0x04001D61 RID: 7521
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001D62 RID: 7522
	[MyCmpReq]
	private SuitLocker suit_locker;

	// Token: 0x04001D63 RID: 7523
	[MyCmpReq]
	private KBatchedAnimController anim_controller;

	// Token: 0x04001D64 RID: 7524
	public const float FUEL_CAPACITY = 100f;

	// Token: 0x04001D65 RID: 7525
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04001D66 RID: 7526
	private int secondaryInputCell = -1;

	// Token: 0x04001D67 RID: 7527
	private FlowUtilityNetwork.NetworkItem flowNetworkItem;

	// Token: 0x04001D68 RID: 7528
	private ConduitConsumer fuel_consumer;

	// Token: 0x04001D69 RID: 7529
	private Tag fuel_tag;

	// Token: 0x04001D6A RID: 7530
	private MeterController o2_meter;

	// Token: 0x04001D6B RID: 7531
	private MeterController fuel_meter;

	// Token: 0x020016A0 RID: 5792
	public class States : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker>
	{
		// Token: 0x06009802 RID: 38914 RVA: 0x00386CC4 File Offset: 0x00384EC4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(JetSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.EventTransition(GameHashes.OnStorageChange, this.charging, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null);
			this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).Transition(this.charged, (JetSuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false);
			this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nofuel, (JetSuitLocker.StatesInstance smi) => !smi.master.HasFuel(), UpdateRate.SIM_200ms).Update("FuelSuit", delegate(JetSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.FuelSuit(dt);
			}, UpdateRate.SIM_1000ms, false);
			this.charging.nofuel.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (JetSuitLocker.StatesInstance smi) => smi.master.HasFuel(), UpdateRate.SIM_200ms).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.TOOLTIP, "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, default(HashedString), 129022, null, null, null);
			this.charged.Transition(this.charging, (JetSuitLocker.StatesInstance smi) => !smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.empty, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null);
		}

		// Token: 0x04007575 RID: 30069
		public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State empty;

		// Token: 0x04007576 RID: 30070
		public JetSuitLocker.States.ChargingStates charging;

		// Token: 0x04007577 RID: 30071
		public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State charged;

		// Token: 0x02002904 RID: 10500
		public class ChargingStates : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State
		{
			// Token: 0x0400B539 RID: 46393
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State notoperational;

			// Token: 0x0400B53A RID: 46394
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State operational;

			// Token: 0x0400B53B RID: 46395
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State nofuel;
		}
	}

	// Token: 0x020016A1 RID: 5793
	public class StatesInstance : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.GameInstance
	{
		// Token: 0x06009804 RID: 38916 RVA: 0x00386F45 File Offset: 0x00385145
		public StatesInstance(JetSuitLocker jet_suit_locker) : base(jet_suit_locker)
		{
		}
	}
}
