using System;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class WarpConduitSender : StateMachineComponent<WarpConduitSender.StatesInstance>, ISecondaryInput
{
	// Token: 0x060038E3 RID: 14563 RVA: 0x0013E4F4 File Offset: 0x0013C6F4
	private bool IsSending()
	{
		return base.smi.master.gasPort.IsOn() || base.smi.master.liquidPort.IsOn() || base.smi.master.solidPort.IsOn();
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x0013E548 File Offset: 0x0013C748
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage[] components = base.GetComponents<Storage>();
		this.gasStorage = components[0];
		this.liquidStorage = components[1];
		this.solidStorage = components[2];
		this.gasPort = new WarpConduitSender.ConduitPort(base.gameObject, this.gasPortInfo, 1, this.gasStorage);
		this.liquidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.liquidPortInfo, 2, this.liquidStorage);
		this.solidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.solidPortInfo, 3, this.solidStorage);
		Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
		this.FindPartner();
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
		base.smi.StartSM();
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x0013E699 File Offset: 0x0013C899
	public void OnActivatedChanged(object _)
	{
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
	}

	// Token: 0x060038E6 RID: 14566 RVA: 0x0013E6C4 File Offset: 0x0013C8C4
	private void FindPartner()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitReceiver");
		foreach (WarpConduitReceiver component in UnityEngine.Object.FindObjectsOfType<WarpConduitReceiver>())
		{
			if (component.GetMyWorldId() != this.GetMyWorldId())
			{
				this.receiver = component;
				break;
			}
		}
		if (this.receiver == null)
		{
			global::Debug.LogWarning("No warp conduit receiver found - maybe POI stomping or failure to spawn?");
			return;
		}
		this.receiver.SetStorage(this.gasStorage, this.liquidStorage, this.solidStorage);
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x0013E74C File Offset: 0x0013C94C
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.inputCell, this.liquidPort.networkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasPort.inputCell, this.gasPort.networkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.inputCell, this.solidPort.solidConsumer, true);
		base.OnCleanUp();
	}

	// Token: 0x060038E8 RID: 14568 RVA: 0x0013E7DD File Offset: 0x0013C9DD
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
	}

	// Token: 0x060038E9 RID: 14569 RVA: 0x0013E80C File Offset: 0x0013CA0C
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.liquidPortInfo.conduitType == type)
		{
			return this.liquidPortInfo.offset;
		}
		if (this.gasPortInfo.conduitType == type)
		{
			return this.gasPortInfo.offset;
		}
		if (this.solidPortInfo.conduitType == type)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x040022B9 RID: 8889
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040022BA RID: 8890
	public Storage gasStorage;

	// Token: 0x040022BB RID: 8891
	public Storage liquidStorage;

	// Token: 0x040022BC RID: 8892
	public Storage solidStorage;

	// Token: 0x040022BD RID: 8893
	public WarpConduitReceiver receiver;

	// Token: 0x040022BE RID: 8894
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x040022BF RID: 8895
	private WarpConduitSender.ConduitPort liquidPort;

	// Token: 0x040022C0 RID: 8896
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x040022C1 RID: 8897
	private WarpConduitSender.ConduitPort gasPort;

	// Token: 0x040022C2 RID: 8898
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x040022C3 RID: 8899
	private WarpConduitSender.ConduitPort solidPort;

	// Token: 0x020017CF RID: 6095
	private class ConduitPort
	{
		// Token: 0x06009C9E RID: 40094 RVA: 0x0039A4C0 File Offset: 0x003986C0
		public ConduitPort(GameObject parent, ConduitPortInfo info, int number, Storage targetStorage)
		{
			this.portInfo = info;
			this.inputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
			if (this.portInfo.conduitType != ConduitType.Solid)
			{
				ConduitConsumer conduitConsumer = parent.AddComponent<ConduitConsumer>();
				conduitConsumer.conduitType = this.portInfo.conduitType;
				conduitConsumer.useSecondaryInput = true;
				conduitConsumer.storage = targetStorage;
				conduitConsumer.capacityKG = targetStorage.capacityKg;
				conduitConsumer.alwaysConsume = false;
				this.conduitConsumer = conduitConsumer;
				this.conduitConsumer.keepZeroMassObject = false;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
				this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.inputCell, parent);
				networkManager.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			else
			{
				this.solidConsumer = parent.AddComponent<SolidConduitConsumer>();
				this.solidConsumer.useSecondaryInput = true;
				this.solidConsumer.storage = targetStorage;
				this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, this.inputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			string meter_animation = "airlock_" + number.ToString();
			string text = "airlock_target_" + number.ToString();
			this.pre = "airlock_" + number.ToString() + "_pre";
			this.loop = "airlock_" + number.ToString() + "_loop";
			this.pst = "airlock_" + number.ToString() + "_pst";
			this.airlock = new MeterController(parent.GetComponent<KBatchedAnimController>(), text, meter_animation, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				text
			});
		}

		// Token: 0x06009C9F RID: 40095 RVA: 0x0039A684 File Offset: 0x00398884
		public bool IsOn()
		{
			if (this.solidConsumer != null)
			{
				return this.solidConsumer.IsConsuming;
			}
			return this.conduitConsumer != null && (this.conduitConsumer.IsConnected && this.conduitConsumer.IsSatisfied) && this.conduitConsumer.consumedLastTick;
		}

		// Token: 0x06009CA0 RID: 40096 RVA: 0x0039A6E4 File Offset: 0x003988E4
		public void Update()
		{
			bool flag = this.IsOn();
			if (flag != this.open)
			{
				this.open = flag;
				if (this.open)
				{
					this.airlock.meterController.Play(this.pre, KAnim.PlayMode.Once, 1f, 0f);
					this.airlock.meterController.Queue(this.loop, KAnim.PlayMode.Loop, 1f, 0f);
					return;
				}
				this.airlock.meterController.Play(this.pst, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x040078D0 RID: 30928
		public ConduitPortInfo portInfo;

		// Token: 0x040078D1 RID: 30929
		public int inputCell;

		// Token: 0x040078D2 RID: 30930
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x040078D3 RID: 30931
		private ConduitConsumer conduitConsumer;

		// Token: 0x040078D4 RID: 30932
		public SolidConduitConsumer solidConsumer;

		// Token: 0x040078D5 RID: 30933
		public MeterController airlock;

		// Token: 0x040078D6 RID: 30934
		private bool open;

		// Token: 0x040078D7 RID: 30935
		private string pre;

		// Token: 0x040078D8 RID: 30936
		private string loop;

		// Token: 0x040078D9 RID: 30937
		private string pst;
	}

	// Token: 0x020017D0 RID: 6096
	public class StatesInstance : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.GameInstance
	{
		// Token: 0x06009CA1 RID: 40097 RVA: 0x0039A786 File Offset: 0x00398986
		public StatesInstance(WarpConduitSender smi) : base(smi)
		{
		}
	}

	// Token: 0x020017D1 RID: 6097
	public class States : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender>
	{
		// Token: 0x06009CA2 RID: 40098 RVA: 0x0039A790 File Offset: 0x00398990
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventHandler(GameHashes.BuildingActivated, delegate(WarpConduitSender.StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			this.off.PlayAnim("off").Enter(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}).EventTransition(GameHashes.OperationalChanged, this.on, (WarpConduitSender.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.waiting).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}, UpdateRate.SIM_1000ms, false);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (!smi.master.IsSending())
				{
					smi.GoTo(this.on.waiting);
				}
			}, UpdateRate.SIM_1000ms, false).Exit(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.Play("working_pst", KAnim.PlayMode.Once);
			});
			this.on.waiting.QueueAnim("idle", false, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (smi.master.IsSending())
				{
					smi.GoTo(this.on.working);
				}
			}, UpdateRate.SIM_1000ms, false);
		}

		// Token: 0x040078DA RID: 30938
		public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State off;

		// Token: 0x040078DB RID: 30939
		public WarpConduitSender.States.onStates on;

		// Token: 0x02002964 RID: 10596
		public class onStates : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State
		{
			// Token: 0x0400B737 RID: 46903
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State working;

			// Token: 0x0400B738 RID: 46904
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State waiting;
		}
	}
}
