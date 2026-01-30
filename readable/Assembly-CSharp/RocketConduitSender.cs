using System;
using UnityEngine;

// Token: 0x020007F1 RID: 2033
public class RocketConduitSender : StateMachineComponent<RocketConduitSender.StatesInstance>, ISecondaryInput
{
	// Token: 0x0600366D RID: 13933 RVA: 0x00133358 File Offset: 0x00131558
	public void AddConduitPortToNetwork()
	{
		if (this.conduitPort == null)
		{
			return;
		}
		int num = Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.conduitPortInfo.offset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.conduitPortInfo.conduitType);
		this.conduitPort.inputCell = num;
		this.conduitPort.networkItem = new FlowUtilityNetwork.NetworkItem(this.conduitPortInfo.conduitType, Endpoint.Sink, num, base.gameObject);
		networkManager.AddToNetworks(num, this.conduitPort.networkItem, true);
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x001333DB File Offset: 0x001315DB
	public void RemoveConduitPortFromNetwork()
	{
		if (this.conduitPort == null)
		{
			return;
		}
		Conduit.GetNetworkManager(this.conduitPortInfo.conduitType).RemoveFromNetworks(this.conduitPort.inputCell, this.conduitPort.networkItem, true);
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x00133414 File Offset: 0x00131614
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		base.Subscribe<RocketConduitSender>(-1118736034, RocketConduitSender.TryFindPartnerDelegate);
		base.Subscribe<RocketConduitSender>(546421097, RocketConduitSender.OnLaunchedDelegate);
		base.Subscribe<RocketConduitSender>(-735346771, RocketConduitSender.OnLandedDelegate);
		base.smi.StartSM();
		Components.RocketConduitSenders.Add(this);
	}

	// Token: 0x06003670 RID: 13936 RVA: 0x00133476 File Offset: 0x00131676
	protected override void OnCleanUp()
	{
		this.RemoveConduitPortFromNetwork();
		base.OnCleanUp();
		Components.RocketConduitSenders.Remove(this);
	}

	// Token: 0x06003671 RID: 13937 RVA: 0x00133490 File Offset: 0x00131690
	private void FindPartner()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(base.gameObject.GetMyWorldId());
		if (world != null && world.IsModuleInterior)
		{
			foreach (RocketConduitReceiver rocketConduitReceiver in world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponents<RocketConduitReceiver>())
			{
				if (rocketConduitReceiver.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
				{
					this.partnerReceiver = rocketConduitReceiver;
					break;
				}
			}
		}
		else
		{
			ClustercraftExteriorDoor component = base.gameObject.GetComponent<ClustercraftExteriorDoor>();
			if (component.HasTargetWorld())
			{
				WorldContainer targetWorld = component.GetTargetWorld();
				foreach (RocketConduitReceiver rocketConduitReceiver2 in Components.RocketConduitReceivers.GetWorldItems(targetWorld.id, false))
				{
					if (rocketConduitReceiver2.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
					{
						this.partnerReceiver = rocketConduitReceiver2;
						break;
					}
				}
			}
		}
		if (this.partnerReceiver == null)
		{
			global::Debug.LogWarning("No rocket conduit receiver found?");
			return;
		}
		this.conduitPort = new RocketConduitSender.ConduitPort(base.gameObject, this.conduitPortInfo, this.conduitStorage);
		if (world != null)
		{
			this.AddConduitPortToNetwork();
		}
		this.partnerReceiver.SetStorage(this.conduitStorage);
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x001335F8 File Offset: 0x001317F8
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.conduitPortInfo.conduitType == type;
	}

	// Token: 0x06003673 RID: 13939 RVA: 0x00133608 File Offset: 0x00131808
	CellOffset ISecondaryInput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.conduitPortInfo.conduitType == type)
		{
			return this.conduitPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x0400210A RID: 8458
	public Storage conduitStorage;

	// Token: 0x0400210B RID: 8459
	[SerializeField]
	public ConduitPortInfo conduitPortInfo;

	// Token: 0x0400210C RID: 8460
	private RocketConduitSender.ConduitPort conduitPort;

	// Token: 0x0400210D RID: 8461
	private RocketConduitReceiver partnerReceiver;

	// Token: 0x0400210E RID: 8462
	private static readonly EventSystem.IntraObjectHandler<RocketConduitSender> TryFindPartnerDelegate = new EventSystem.IntraObjectHandler<RocketConduitSender>(delegate(RocketConduitSender component, object data)
	{
		component.FindPartner();
	});

	// Token: 0x0400210F RID: 8463
	private static readonly EventSystem.IntraObjectHandler<RocketConduitSender> OnLandedDelegate = new EventSystem.IntraObjectHandler<RocketConduitSender>(delegate(RocketConduitSender component, object data)
	{
		component.AddConduitPortToNetwork();
	});

	// Token: 0x04002110 RID: 8464
	private static readonly EventSystem.IntraObjectHandler<RocketConduitSender> OnLaunchedDelegate = new EventSystem.IntraObjectHandler<RocketConduitSender>(delegate(RocketConduitSender component, object data)
	{
		component.RemoveConduitPortFromNetwork();
	});

	// Token: 0x02001762 RID: 5986
	private class ConduitPort
	{
		// Token: 0x06009AFD RID: 39677 RVA: 0x0039313C File Offset: 0x0039133C
		public ConduitPort(GameObject parent, ConduitPortInfo info, Storage targetStorage)
		{
			this.conduitPortInfo = info;
			ConduitConsumer conduitConsumer = parent.AddComponent<ConduitConsumer>();
			conduitConsumer.conduitType = this.conduitPortInfo.conduitType;
			conduitConsumer.useSecondaryInput = true;
			conduitConsumer.storage = targetStorage;
			conduitConsumer.capacityKG = targetStorage.capacityKg;
			conduitConsumer.alwaysConsume = true;
			conduitConsumer.forceAlwaysSatisfied = true;
			this.conduitConsumer = conduitConsumer;
			this.conduitConsumer.keepZeroMassObject = false;
		}

		// Token: 0x0400779E RID: 30622
		public ConduitPortInfo conduitPortInfo;

		// Token: 0x0400779F RID: 30623
		public int inputCell;

		// Token: 0x040077A0 RID: 30624
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x040077A1 RID: 30625
		private ConduitConsumer conduitConsumer;
	}

	// Token: 0x02001763 RID: 5987
	public class StatesInstance : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.GameInstance
	{
		// Token: 0x06009AFE RID: 39678 RVA: 0x003931A9 File Offset: 0x003913A9
		public StatesInstance(RocketConduitSender smi) : base(smi)
		{
		}
	}

	// Token: 0x02001764 RID: 5988
	public class States : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender>
	{
		// Token: 0x06009AFF RID: 39679 RVA: 0x003931B4 File Offset: 0x003913B4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.on;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.on.DefaultState(this.on.waiting);
			this.on.waiting.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).EventTransition(GameHashes.OnStorageChange, this.on.working, (RocketConduitSender.StatesInstance smi) => smi.GetComponent<Storage>().MassStored() > 0f);
			this.on.working.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).DefaultState(this.on.working.ground);
			this.on.working.notOnGround.Enter(delegate(RocketConduitSender.StatesInstance smi)
			{
				smi.gameObject.GetSMI<AutoStorageDropper.Instance>().SetInvertElementFilter(true);
			}).UpdateTransition(this.on.working.ground, delegate(RocketConduitSender.StatesInstance smi, float f)
			{
				WorldContainer myWorld = smi.master.GetMyWorld();
				return myWorld && myWorld.IsModuleInterior && !myWorld.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().HasTag(GameTags.RocketNotOnGround);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(RocketConduitSender.StatesInstance smi)
			{
				if (smi.gameObject != null)
				{
					AutoStorageDropper.Instance smi2 = smi.gameObject.GetSMI<AutoStorageDropper.Instance>();
					if (smi2 != null)
					{
						smi2.SetInvertElementFilter(false);
					}
				}
			});
			this.on.working.ground.Enter(delegate(RocketConduitSender.StatesInstance smi)
			{
				if (smi.master.partnerReceiver != null)
				{
					smi.master.partnerReceiver.conduitPort.conduitDispenser.alwaysDispense = true;
				}
			}).UpdateTransition(this.on.working.notOnGround, delegate(RocketConduitSender.StatesInstance smi, float f)
			{
				WorldContainer myWorld = smi.master.GetMyWorld();
				return myWorld && myWorld.IsModuleInterior && myWorld.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().HasTag(GameTags.RocketNotOnGround);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(RocketConduitSender.StatesInstance smi)
			{
				if (smi.master.partnerReceiver != null)
				{
					smi.master.partnerReceiver.conduitPort.conduitDispenser.alwaysDispense = false;
				}
			});
		}

		// Token: 0x040077A2 RID: 30626
		public RocketConduitSender.States.onStates on;

		// Token: 0x02002934 RID: 10548
		public class onStates : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State
		{
			// Token: 0x0400B648 RID: 46664
			public RocketConduitSender.States.workingStates working;

			// Token: 0x0400B649 RID: 46665
			public GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State waiting;
		}

		// Token: 0x02002935 RID: 10549
		public class workingStates : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State
		{
			// Token: 0x0400B64A RID: 46666
			public GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State notOnGround;

			// Token: 0x0400B64B RID: 46667
			public GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State ground;
		}
	}
}
