using System;
using UnityEngine;

// Token: 0x02000826 RID: 2086
public class WarpConduitReceiver : StateMachineComponent<WarpConduitReceiver.StatesInstance>, ISecondaryOutput
{
	// Token: 0x060038D8 RID: 14552 RVA: 0x0013DFF8 File Offset: 0x0013C1F8
	private bool IsReceiving()
	{
		return base.smi.master.gasPort.IsOn() || base.smi.master.liquidPort.IsOn() || base.smi.master.solidPort.IsOn();
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x0013E04A File Offset: 0x0013C24A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		if (this.solidPort.solidDispenser != null)
		{
			this.solidPort.solidDispenser.solidOnly = true;
		}
		base.smi.StartSM();
	}

	// Token: 0x060038DA RID: 14554 RVA: 0x0013E088 File Offset: 0x0013C288
	private void FindPartner()
	{
		if (this.senderGasStorage != null)
		{
			return;
		}
		WarpConduitSender warpConduitSender = null;
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitSender");
		foreach (WarpConduitSender warpConduitSender2 in UnityEngine.Object.FindObjectsOfType<WarpConduitSender>())
		{
			if (warpConduitSender2.GetMyWorldId() != this.GetMyWorldId())
			{
				warpConduitSender = warpConduitSender2;
				break;
			}
		}
		if (warpConduitSender == null)
		{
			global::Debug.LogWarning("No warp conduit sender found - maybe POI stomping or failure to spawn?");
			return;
		}
		this.SetStorage(warpConduitSender.gasStorage, warpConduitSender.liquidStorage, warpConduitSender.solidStorage);
		WarpConduitStatus.UpdateWarpConduitsOperational(warpConduitSender.gameObject, base.gameObject);
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x0013E124 File Offset: 0x0013C324
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.outputCell, this.liquidPort.networkItem, true);
		if (this.gasPort.portInfo != null)
		{
			Conduit.GetNetworkManager(this.gasPort.portInfo.conduitType).RemoveFromNetworks(this.gasPort.outputCell, this.gasPort.networkItem, true);
		}
		else
		{
			global::Debug.LogWarning("Conduit Receiver gasPort portInfo is null in OnCleanUp");
		}
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.outputCell, this.solidPort.networkItem, true);
		base.OnCleanUp();
	}

	// Token: 0x060038DC RID: 14556 RVA: 0x0013E1D3 File Offset: 0x0013C3D3
	public void OnActivatedChanged(object _)
	{
		if (this.senderGasStorage == null)
		{
			this.FindPartner();
		}
		WarpConduitStatus.UpdateWarpConduitsOperational((this.senderGasStorage != null) ? this.senderGasStorage.gameObject : null, base.gameObject);
	}

	// Token: 0x060038DD RID: 14557 RVA: 0x0013E210 File Offset: 0x0013C410
	public void SetStorage(Storage gasStorage, Storage liquidStorage, Storage solidStorage)
	{
		this.senderGasStorage = gasStorage;
		this.senderLiquidStorage = liquidStorage;
		this.senderSolidStorage = solidStorage;
		this.gasPort.SetPortInfo(base.gameObject, this.gasPortInfo, gasStorage, 1);
		this.liquidPort.SetPortInfo(base.gameObject, this.liquidPortInfo, liquidStorage, 2);
		this.solidPort.SetPortInfo(base.gameObject, this.solidPortInfo, solidStorage, 3);
		Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
	}

	// Token: 0x060038DE RID: 14558 RVA: 0x0013E307 File Offset: 0x0013C507
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return type == this.gasPortInfo.conduitType || type == this.liquidPortInfo.conduitType || type == this.solidPortInfo.conduitType;
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x0013E338 File Offset: 0x0013C538
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.gasPortInfo.conduitType)
		{
			return this.gasPortInfo.offset;
		}
		if (type == this.liquidPortInfo.conduitType)
		{
			return this.liquidPortInfo.offset;
		}
		if (type == this.solidPortInfo.conduitType)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x040022AF RID: 8879
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x040022B0 RID: 8880
	private WarpConduitReceiver.ConduitPort liquidPort;

	// Token: 0x040022B1 RID: 8881
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x040022B2 RID: 8882
	private WarpConduitReceiver.ConduitPort solidPort;

	// Token: 0x040022B3 RID: 8883
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x040022B4 RID: 8884
	private WarpConduitReceiver.ConduitPort gasPort;

	// Token: 0x040022B5 RID: 8885
	public Storage senderGasStorage;

	// Token: 0x040022B6 RID: 8886
	public Storage senderLiquidStorage;

	// Token: 0x040022B7 RID: 8887
	public Storage senderSolidStorage;

	// Token: 0x020017CC RID: 6092
	public struct ConduitPort
	{
		// Token: 0x06009C96 RID: 40086 RVA: 0x0039A03C File Offset: 0x0039823C
		public void SetPortInfo(GameObject parent, ConduitPortInfo info, Storage senderStorage, int number)
		{
			this.portInfo = info;
			this.outputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
			if (this.portInfo.conduitType != ConduitType.Solid)
			{
				ConduitDispenser conduitDispenser = parent.AddComponent<ConduitDispenser>();
				conduitDispenser.conduitType = this.portInfo.conduitType;
				conduitDispenser.useSecondaryOutput = true;
				conduitDispenser.alwaysDispense = true;
				conduitDispenser.storage = senderStorage;
				this.dispenser = conduitDispenser;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
				this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.outputCell, parent);
				networkManager.AddToNetworks(this.outputCell, this.networkItem, true);
			}
			else
			{
				SolidConduitDispenser solidConduitDispenser = parent.AddComponent<SolidConduitDispenser>();
				solidConduitDispenser.storage = senderStorage;
				solidConduitDispenser.alwaysDispense = true;
				solidConduitDispenser.useSecondaryOutput = true;
				this.solidDispenser = solidConduitDispenser;
				this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Source, this.outputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(this.outputCell, this.networkItem, true);
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

		// Token: 0x06009C97 RID: 40087 RVA: 0x0039A1DC File Offset: 0x003983DC
		public bool IsOn()
		{
			if (this.solidDispenser != null)
			{
				return this.solidDispenser.IsDispensing;
			}
			return this.dispenser != null && !this.dispenser.blocked && !this.dispenser.empty;
		}

		// Token: 0x06009C98 RID: 40088 RVA: 0x0039A230 File Offset: 0x00398430
		public void UpdatePortAnim()
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

		// Token: 0x040078C4 RID: 30916
		public ConduitPortInfo portInfo;

		// Token: 0x040078C5 RID: 30917
		public int outputCell;

		// Token: 0x040078C6 RID: 30918
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x040078C7 RID: 30919
		public ConduitDispenser dispenser;

		// Token: 0x040078C8 RID: 30920
		public SolidConduitDispenser solidDispenser;

		// Token: 0x040078C9 RID: 30921
		public MeterController airlock;

		// Token: 0x040078CA RID: 30922
		private bool open;

		// Token: 0x040078CB RID: 30923
		private string pre;

		// Token: 0x040078CC RID: 30924
		private string loop;

		// Token: 0x040078CD RID: 30925
		private string pst;
	}

	// Token: 0x020017CD RID: 6093
	public class StatesInstance : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.GameInstance
	{
		// Token: 0x06009C99 RID: 40089 RVA: 0x0039A2D2 File Offset: 0x003984D2
		public StatesInstance(WarpConduitReceiver master) : base(master)
		{
		}
	}

	// Token: 0x020017CE RID: 6094
	public class States : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver>
	{
		// Token: 0x06009C9A RID: 40090 RVA: 0x0039A2DC File Offset: 0x003984DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventHandler(GameHashes.BuildingActivated, delegate(WarpConduitReceiver.StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			this.off.PlayAnim("off").Enter(delegate(WarpConduitReceiver.StatesInstance smi)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}).EventTransition(GameHashes.OperationalFlagChanged, this.on, (WarpConduitReceiver.StatesInstance smi) => smi.GetComponent<Operational>().GetFlag(WarpConduitStatus.warpConnectedFlag));
			this.on.DefaultState(this.on.idle).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}, UpdateRate.SIM_1000ms, false);
			this.on.idle.QueueAnim("idle", false, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				if (smi.master.IsReceiving())
				{
					smi.GoTo(this.on.working);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				if (!smi.master.IsReceiving())
				{
					smi.GoTo(this.on.idle);
				}
			}, UpdateRate.SIM_1000ms, false).Exit(delegate(WarpConduitReceiver.StatesInstance smi)
			{
				smi.Play("working_pst", KAnim.PlayMode.Once);
			});
		}

		// Token: 0x040078CE RID: 30926
		public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State off;

		// Token: 0x040078CF RID: 30927
		public WarpConduitReceiver.States.onStates on;

		// Token: 0x02002962 RID: 10594
		public class onStates : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State
		{
			// Token: 0x0400B72F RID: 46895
			public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State working;

			// Token: 0x0400B730 RID: 46896
			public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State idle;
		}
	}
}
