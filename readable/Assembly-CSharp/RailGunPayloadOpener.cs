using System;
using UnityEngine;

// Token: 0x020007E5 RID: 2021
public class RailGunPayloadOpener : StateMachineComponent<RailGunPayloadOpener.StatesInstance>, ISecondaryOutput
{
	// Token: 0x060035D9 RID: 13785 RVA: 0x0012FEA4 File Offset: 0x0012E0A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.gasOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.gasPortInfo.offset);
		this.gasDispenser = this.CreateConduitDispenser(ConduitType.Gas, this.gasOutputCell, out this.gasNetworkItem);
		this.liquidOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.liquidPortInfo.offset);
		this.liquidDispenser = this.CreateConduitDispenser(ConduitType.Liquid, this.liquidOutputCell, out this.liquidNetworkItem);
		this.solidOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.solidPortInfo.offset);
		this.solidDispenser = this.CreateSolidConduitDispenser(this.solidOutputCell, out this.solidNetworkItem);
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.payloadStorage.gunTargetOffset = new Vector2(-1f, 1.5f);
		this.payloadMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_storage_target", "meter_storage", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.smi.StartSM();
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x0012FFAC File Offset: 0x0012E1AC
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidOutputCell, this.liquidNetworkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasOutputCell, this.gasNetworkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidOutputCell, this.solidDispenser, true);
		base.OnCleanUp();
	}

	// Token: 0x060035DB RID: 13787 RVA: 0x00130020 File Offset: 0x0012E220
	private ConduitDispenser CreateConduitDispenser(ConduitType outputType, int outputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		ConduitDispenser conduitDispenser = base.gameObject.AddComponent<ConduitDispenser>();
		conduitDispenser.conduitType = outputType;
		conduitDispenser.useSecondaryOutput = true;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.storage = this.resourceStorage;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(outputType);
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(outputType, Endpoint.Source, outputCell, base.gameObject);
		networkManager.AddToNetworks(outputCell, flowNetworkItem, true);
		return conduitDispenser;
	}

	// Token: 0x060035DC RID: 13788 RVA: 0x00130078 File Offset: 0x0012E278
	private SolidConduitDispenser CreateSolidConduitDispenser(int outputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		SolidConduitDispenser solidConduitDispenser = base.gameObject.AddComponent<SolidConduitDispenser>();
		solidConduitDispenser.storage = this.resourceStorage;
		solidConduitDispenser.alwaysDispense = true;
		solidConduitDispenser.useSecondaryOutput = true;
		solidConduitDispenser.solidOnly = true;
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Source, outputCell, base.gameObject);
		Game.Instance.solidConduitSystem.AddToNetworks(outputCell, flowNetworkItem, true);
		return solidConduitDispenser;
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x001300D4 File Offset: 0x0012E2D4
	public void EmptyPayload()
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null && component.items.Count > 0)
		{
			GameObject gameObject = this.payloadStorage.items[0];
			gameObject.GetComponent<Storage>().Transfer(this.resourceStorage, false, false);
			Util.KDestroyGameObject(gameObject);
			component.ConsumeIgnoringDisease(this.payloadStorage.items[0]);
		}
	}

	// Token: 0x060035DE RID: 13790 RVA: 0x00130140 File Offset: 0x0012E340
	public bool PowerOperationalChanged()
	{
		EnergyConsumer component = base.GetComponent<EnergyConsumer>();
		return component != null && component.IsPowered;
	}

	// Token: 0x060035DF RID: 13791 RVA: 0x00130165 File Offset: 0x0012E365
	bool ISecondaryOutput.HasSecondaryConduitType(ConduitType type)
	{
		return type == this.gasPortInfo.conduitType || type == this.liquidPortInfo.conduitType || type == this.solidPortInfo.conduitType;
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x00130194 File Offset: 0x0012E394
	CellOffset ISecondaryOutput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.gasPortInfo.conduitType)
		{
			return this.gasPortInfo.offset;
		}
		if (type == this.liquidPortInfo.conduitType)
		{
			return this.liquidPortInfo.offset;
		}
		if (type != this.solidPortInfo.conduitType)
		{
			return CellOffset.none;
		}
		return this.solidPortInfo.offset;
	}

	// Token: 0x040020BA RID: 8378
	public static float delivery_time = 10f;

	// Token: 0x040020BB RID: 8379
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x040020BC RID: 8380
	private int liquidOutputCell = -1;

	// Token: 0x040020BD RID: 8381
	private FlowUtilityNetwork.NetworkItem liquidNetworkItem;

	// Token: 0x040020BE RID: 8382
	private ConduitDispenser liquidDispenser;

	// Token: 0x040020BF RID: 8383
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x040020C0 RID: 8384
	private int gasOutputCell = -1;

	// Token: 0x040020C1 RID: 8385
	private FlowUtilityNetwork.NetworkItem gasNetworkItem;

	// Token: 0x040020C2 RID: 8386
	private ConduitDispenser gasDispenser;

	// Token: 0x040020C3 RID: 8387
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x040020C4 RID: 8388
	private int solidOutputCell = -1;

	// Token: 0x040020C5 RID: 8389
	private FlowUtilityNetwork.NetworkItem solidNetworkItem;

	// Token: 0x040020C6 RID: 8390
	private SolidConduitDispenser solidDispenser;

	// Token: 0x040020C7 RID: 8391
	public Storage payloadStorage;

	// Token: 0x040020C8 RID: 8392
	public Storage resourceStorage;

	// Token: 0x040020C9 RID: 8393
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x040020CA RID: 8394
	private MeterController payloadMeter;

	// Token: 0x02001747 RID: 5959
	public class StatesInstance : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.GameInstance
	{
		// Token: 0x06009A90 RID: 39568 RVA: 0x003918B1 File Offset: 0x0038FAB1
		public StatesInstance(RailGunPayloadOpener master) : base(master)
		{
		}

		// Token: 0x06009A91 RID: 39569 RVA: 0x003918BA File Offset: 0x0038FABA
		public bool HasPayload()
		{
			return base.smi.master.payloadStorage.items.Count > 0;
		}

		// Token: 0x06009A92 RID: 39570 RVA: 0x003918D9 File Offset: 0x0038FAD9
		public bool HasResources()
		{
			return base.smi.master.resourceStorage.MassStored() > 0f;
		}
	}

	// Token: 0x02001748 RID: 5960
	public class States : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener>
	{
		// Token: 0x06009A93 RID: 39571 RVA: 0x003918F8 File Offset: 0x0038FAF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.unoperational.PlayAnim("off").EventTransition(GameHashes.OperationalFlagChanged, this.operational, (RailGunPayloadOpener.StatesInstance smi) => smi.master.PowerOperationalChanged()).Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, true);
				smi.GetComponent<ManualDeliveryKG>().Pause(true, "no_power");
			});
			this.operational.Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<ManualDeliveryKG>().Pause(false, "power");
			}).EventTransition(GameHashes.OperationalFlagChanged, this.unoperational, (RailGunPayloadOpener.StatesInstance smi) => !smi.master.PowerOperationalChanged()).DefaultState(this.operational.idle).EventHandler(GameHashes.OnStorageChange, delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.master.payloadMeter.SetPositionPercent(Mathf.Clamp01((float)smi.master.payloadStorage.items.Count / smi.master.payloadStorage.capacityKg));
			});
			this.operational.idle.PlayAnim("on").EventTransition(GameHashes.OnStorageChange, this.operational.pre, (RailGunPayloadOpener.StatesInstance smi) => smi.HasPayload());
			this.operational.pre.Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, true);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.operational.loop);
			this.operational.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(10f, this.operational.pst);
			this.operational.pst.PlayAnim("working_pst").Exit(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.master.EmptyPayload();
				smi.GetComponent<Operational>().SetActive(false, true);
			}).OnAnimQueueComplete(this.operational.idle);
		}

		// Token: 0x04007743 RID: 30531
		public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State unoperational;

		// Token: 0x04007744 RID: 30532
		public RailGunPayloadOpener.States.OperationalStates operational;

		// Token: 0x0200292C RID: 10540
		public class OperationalStates : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State
		{
			// Token: 0x0400B620 RID: 46624
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State idle;

			// Token: 0x0400B621 RID: 46625
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State pre;

			// Token: 0x0400B622 RID: 46626
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State loop;

			// Token: 0x0400B623 RID: 46627
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State pst;
		}
	}
}
