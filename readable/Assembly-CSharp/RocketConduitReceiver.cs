using System;
using UnityEngine;

// Token: 0x020007F0 RID: 2032
public class RocketConduitReceiver : StateMachineComponent<RocketConduitReceiver.StatesInstance>, ISecondaryOutput
{
	// Token: 0x06003662 RID: 13922 RVA: 0x00132F6C File Offset: 0x0013116C
	public void AddConduitPortToNetwork()
	{
		if (this.conduitPort.conduitDispenser == null)
		{
			return;
		}
		int num = Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.conduitPortInfo.offset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.conduitPortInfo.conduitType);
		this.conduitPort.outputCell = num;
		this.conduitPort.networkItem = new FlowUtilityNetwork.NetworkItem(this.conduitPortInfo.conduitType, Endpoint.Source, num, base.gameObject);
		networkManager.AddToNetworks(num, this.conduitPort.networkItem, true);
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x00132FFC File Offset: 0x001311FC
	public void RemoveConduitPortFromNetwork()
	{
		if (this.conduitPort.conduitDispenser == null)
		{
			return;
		}
		Conduit.GetNetworkManager(this.conduitPortInfo.conduitType).RemoveFromNetworks(this.conduitPort.outputCell, this.conduitPort.networkItem, true);
	}

	// Token: 0x06003664 RID: 13924 RVA: 0x0013304C File Offset: 0x0013124C
	private bool CanTransferFromSender()
	{
		bool result = false;
		if ((base.smi.master.senderConduitStorage.MassStored() > 0f || base.smi.master.senderConduitStorage.items.Count > 0) && base.smi.master.conduitPort.conduitDispenser.GetConduitManager().GetPermittedFlow(base.smi.master.conduitPort.outputCell) != ConduitFlow.FlowDirections.None)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x001330D0 File Offset: 0x001312D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		base.Subscribe<RocketConduitReceiver>(-1118736034, RocketConduitReceiver.TryFindPartner);
		base.Subscribe<RocketConduitReceiver>(546421097, RocketConduitReceiver.OnLaunchedDelegate);
		base.Subscribe<RocketConduitReceiver>(-735346771, RocketConduitReceiver.OnLandedDelegate);
		base.smi.StartSM();
		Components.RocketConduitReceivers.Add(this);
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x00133132 File Offset: 0x00131332
	protected override void OnCleanUp()
	{
		this.RemoveConduitPortFromNetwork();
		base.OnCleanUp();
		Components.RocketConduitReceivers.Remove(this);
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x0013314C File Offset: 0x0013134C
	private void FindPartner()
	{
		if (this.senderConduitStorage != null)
		{
			return;
		}
		RocketConduitSender rocketConduitSender = null;
		WorldContainer world = ClusterManager.Instance.GetWorld(base.gameObject.GetMyWorldId());
		if (world != null && world.IsModuleInterior)
		{
			foreach (RocketConduitSender rocketConduitSender2 in world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponents<RocketConduitSender>())
			{
				if (rocketConduitSender2.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
				{
					rocketConduitSender = rocketConduitSender2;
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
				foreach (RocketConduitSender rocketConduitSender3 in Components.RocketConduitSenders.GetWorldItems(targetWorld.id, false))
				{
					if (rocketConduitSender3.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
					{
						rocketConduitSender = rocketConduitSender3;
						break;
					}
				}
			}
		}
		if (rocketConduitSender == null)
		{
			global::Debug.LogWarning("No warp conduit sender found?");
			return;
		}
		this.SetStorage(rocketConduitSender.conduitStorage);
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x00133288 File Offset: 0x00131488
	public void SetStorage(Storage conduitStorage)
	{
		this.senderConduitStorage = conduitStorage;
		this.conduitPort.SetPortInfo(base.gameObject, this.conduitPortInfo, conduitStorage);
		if (base.gameObject.GetMyWorld() != null)
		{
			this.AddConduitPortToNetwork();
		}
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x001332C2 File Offset: 0x001314C2
	bool ISecondaryOutput.HasSecondaryConduitType(ConduitType type)
	{
		return type == this.conduitPortInfo.conduitType;
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x001332D2 File Offset: 0x001314D2
	CellOffset ISecondaryOutput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.conduitPortInfo.conduitType)
		{
			return this.conduitPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04002104 RID: 8452
	[SerializeField]
	public ConduitPortInfo conduitPortInfo;

	// Token: 0x04002105 RID: 8453
	public RocketConduitReceiver.ConduitPort conduitPort;

	// Token: 0x04002106 RID: 8454
	public Storage senderConduitStorage;

	// Token: 0x04002107 RID: 8455
	private static readonly EventSystem.IntraObjectHandler<RocketConduitReceiver> TryFindPartner = new EventSystem.IntraObjectHandler<RocketConduitReceiver>(delegate(RocketConduitReceiver component, object data)
	{
		component.FindPartner();
	});

	// Token: 0x04002108 RID: 8456
	private static readonly EventSystem.IntraObjectHandler<RocketConduitReceiver> OnLandedDelegate = new EventSystem.IntraObjectHandler<RocketConduitReceiver>(delegate(RocketConduitReceiver component, object data)
	{
		component.AddConduitPortToNetwork();
	});

	// Token: 0x04002109 RID: 8457
	private static readonly EventSystem.IntraObjectHandler<RocketConduitReceiver> OnLaunchedDelegate = new EventSystem.IntraObjectHandler<RocketConduitReceiver>(delegate(RocketConduitReceiver component, object data)
	{
		component.RemoveConduitPortFromNetwork();
	});

	// Token: 0x0200175E RID: 5982
	public struct ConduitPort
	{
		// Token: 0x06009AF2 RID: 39666 RVA: 0x00392FA0 File Offset: 0x003911A0
		public void SetPortInfo(GameObject parent, ConduitPortInfo info, Storage senderStorage)
		{
			this.portInfo = info;
			ConduitDispenser conduitDispenser = parent.AddComponent<ConduitDispenser>();
			conduitDispenser.conduitType = this.portInfo.conduitType;
			conduitDispenser.useSecondaryOutput = true;
			conduitDispenser.alwaysDispense = true;
			conduitDispenser.storage = senderStorage;
			this.conduitDispenser = conduitDispenser;
		}

		// Token: 0x04007797 RID: 30615
		public ConduitPortInfo portInfo;

		// Token: 0x04007798 RID: 30616
		public int outputCell;

		// Token: 0x04007799 RID: 30617
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x0400779A RID: 30618
		public ConduitDispenser conduitDispenser;
	}

	// Token: 0x0200175F RID: 5983
	public class StatesInstance : GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.GameInstance
	{
		// Token: 0x06009AF3 RID: 39667 RVA: 0x00392FE8 File Offset: 0x003911E8
		public StatesInstance(RocketConduitReceiver master) : base(master)
		{
		}
	}

	// Token: 0x02001760 RID: 5984
	public class States : GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver>
	{
		// Token: 0x06009AF4 RID: 39668 RVA: 0x00392FF4 File Offset: 0x003911F4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.EventTransition(GameHashes.OperationalFlagChanged, this.on, (RocketConduitReceiver.StatesInstance smi) => smi.GetComponent<Operational>().GetFlag(WarpConduitStatus.warpConnectedFlag));
			this.on.DefaultState(this.on.empty);
			this.on.empty.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(RocketConduitReceiver.StatesInstance smi, float dt)
			{
				if (smi.master.CanTransferFromSender())
				{
					smi.GoTo(this.on.hasResources);
				}
			}, UpdateRate.SIM_200ms, false);
			this.on.hasResources.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(RocketConduitReceiver.StatesInstance smi, float dt)
			{
				if (!smi.master.CanTransferFromSender())
				{
					smi.GoTo(this.on.empty);
				}
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x0400779B RID: 30619
		public GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State off;

		// Token: 0x0400779C RID: 30620
		public RocketConduitReceiver.States.onStates on;

		// Token: 0x02002932 RID: 10546
		public class onStates : GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State
		{
			// Token: 0x0400B644 RID: 46660
			public GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State hasResources;

			// Token: 0x0400B645 RID: 46661
			public GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State empty;
		}
	}
}
