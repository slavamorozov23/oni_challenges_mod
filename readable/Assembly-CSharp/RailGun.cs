using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007E3 RID: 2019
public class RailGun : StateMachineComponent<RailGun.StatesInstance>, ISim200ms, ISecondaryInput
{
	// Token: 0x17000367 RID: 871
	// (get) Token: 0x060035BE RID: 13758 RVA: 0x0012F296 File Offset: 0x0012D496
	public float MaxLaunchMass
	{
		get
		{
			return 200f;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x060035BF RID: 13759 RVA: 0x0012F29D File Offset: 0x0012D49D
	public float EnergyCost
	{
		get
		{
			return base.smi.EnergyCost();
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x060035C0 RID: 13760 RVA: 0x0012F2AA File Offset: 0x0012D4AA
	public float CurrentEnergy
	{
		get
		{
			return this.hepStorage.Particles;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x060035C1 RID: 13761 RVA: 0x0012F2B7 File Offset: 0x0012D4B7
	public bool AllowLaunchingFromLogic
	{
		get
		{
			return !this.hasLogicWire || (this.hasLogicWire && this.isLogicActive);
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x060035C2 RID: 13762 RVA: 0x0012F2D3 File Offset: 0x0012D4D3
	public bool HasLogicWire
	{
		get
		{
			return this.hasLogicWire;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x060035C3 RID: 13763 RVA: 0x0012F2DB File Offset: 0x0012D4DB
	public bool IsLogicActive
	{
		get
		{
			return this.isLogicActive;
		}
	}

	// Token: 0x060035C4 RID: 13764 RVA: 0x0012F2E4 File Offset: 0x0012D4E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.destinationSelector = base.GetComponent<ClusterDestinationSelector>();
		this.resourceStorage = base.GetComponent<Storage>();
		this.particleStorage = base.GetComponent<HighEnergyParticleStorage>();
		if (RailGun.noSurfaceSightStatusItem == null)
		{
			RailGun.noSurfaceSightStatusItem = new StatusItem("RAILGUN_PATH_NOT_CLEAR", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		}
		if (RailGun.noDestinationStatusItem == null)
		{
			RailGun.noDestinationStatusItem = new StatusItem("RAILGUN_NO_DESTINATION", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		}
		this.gasInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.gasPortInfo.offset);
		this.gasConsumer = this.CreateConduitConsumer(ConduitType.Gas, this.gasInputCell, out this.gasNetworkItem);
		this.liquidInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.liquidPortInfo.offset);
		this.liquidConsumer = this.CreateConduitConsumer(ConduitType.Liquid, this.liquidInputCell, out this.liquidNetworkItem);
		this.solidInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.solidPortInfo.offset);
		this.solidConsumer = this.CreateSolidConduitConsumer(this.solidInputCell, out this.solidNetworkItem);
		this.CreateMeters();
		base.smi.StartSM();
		if (RailGun.infoStatusItemLogic == null)
		{
			RailGun.infoStatusItemLogic = new StatusItem("LogicOperationalInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			RailGun.infoStatusItemLogic.resolveStringCallback = new Func<string, object, string>(RailGun.ResolveInfoStatusItemString);
		}
		this.CheckLogicWireState();
		base.Subscribe<RailGun>(-801688580, RailGun.OnLogicValueChangedDelegate);
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x0012F484 File Offset: 0x0012D684
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidInputCell, this.liquidNetworkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasInputCell, this.gasNetworkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidInputCell, this.solidConsumer, true);
		base.OnCleanUp();
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x0012F4F8 File Offset: 0x0012D6F8
	private void CreateMeters()
	{
		this.resourceMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_storage_target", "meter_storage", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_orb_target", "meter_orb", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x060035C7 RID: 13767 RVA: 0x0012F54B File Offset: 0x0012D74B
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
	}

	// Token: 0x060035C8 RID: 13768 RVA: 0x0012F57C File Offset: 0x0012D77C
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

	// Token: 0x060035C9 RID: 13769 RVA: 0x0012F5DC File Offset: 0x0012D7DC
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(RailGun.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x060035CA RID: 13770 RVA: 0x0012F60C File Offset: 0x0012D80C
	private void CheckLogicWireState()
	{
		LogicCircuitNetwork network = this.GetNetwork();
		this.hasLogicWire = (network != null);
		int value = (network != null) ? network.OutputValue : 1;
		bool flag = LogicCircuitNetwork.IsBitActive(0, value);
		this.isLogicActive = flag;
		base.smi.sm.allowedFromLogic.Set(this.AllowLaunchingFromLogic, base.smi, false);
		base.GetComponent<KSelectable>().ToggleStatusItem(RailGun.infoStatusItemLogic, network != null, this);
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x0012F67F File Offset: 0x0012D87F
	private void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == RailGun.PORT_ID)
		{
			this.CheckLogicWireState();
		}
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x0012F69E File Offset: 0x0012D89E
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		RailGun railGun = (RailGun)data;
		Operational operational = railGun.operational;
		return railGun.AllowLaunchingFromLogic ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED;
	}

	// Token: 0x060035CD RID: 13773 RVA: 0x0012F6C8 File Offset: 0x0012D8C8
	public void Sim200ms(float dt)
	{
		WorldContainer myWorld = this.GetMyWorld();
		Extents extents = base.GetComponent<Building>().GetExtents();
		int x = extents.x;
		int x2 = extents.x + extents.width - 2;
		int y = extents.y + extents.height;
		int num = Grid.XYToCell(x, y);
		int num2 = Grid.XYToCell(x2, y);
		bool flag = true;
		int num3 = (int)myWorld.maximumBounds.y;
		for (int i = num; i <= num2; i++)
		{
			int num4 = i;
			while (Grid.CellRow(num4) <= num3)
			{
				if (!Grid.IsValidCell(num4) || Grid.Solid[num4])
				{
					flag = false;
					break;
				}
				num4 = Grid.CellAbove(num4);
			}
		}
		this.operational.SetFlag(RailGun.noSurfaceSight, flag);
		this.operational.SetFlag(RailGun.noDestination, this.destinationSelector.GetDestinationWorld() >= 0);
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(RailGun.noSurfaceSightStatusItem, !flag, null);
		component.ToggleStatusItem(RailGun.noDestinationStatusItem, this.destinationSelector.GetDestinationWorld() < 0, null);
		this.UpdateMeters();
	}

	// Token: 0x060035CE RID: 13774 RVA: 0x0012F7E0 File Offset: 0x0012D9E0
	private void UpdateMeters()
	{
		this.resourceMeter.SetPositionPercent(Mathf.Clamp01(this.resourceStorage.MassStored() / this.resourceStorage.capacityKg));
		this.particleMeter.SetPositionPercent(Mathf.Clamp01(this.particleStorage.Particles / this.particleStorage.capacity));
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x0012F83C File Offset: 0x0012DA3C
	private void LaunchProjectile()
	{
		Extents extents = base.GetComponent<Building>().GetExtents();
		Vector2I vector2I = Grid.PosToXY(base.transform.position);
		vector2I.y += extents.height + 1;
		int cell = Grid.XYToCell(vector2I.x, vector2I.y);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RailGunPayload"), Grid.CellToPosCBC(cell, Grid.SceneLayer.Front));
		Storage component = gameObject.GetComponent<Storage>();
		float num = 0f;
		while (num < this.launchMass && this.resourceStorage.MassStored() > 0f)
		{
			num += this.resourceStorage.Transfer(component, GameTags.Stored, this.launchMass - num, false, true);
		}
		component.SetContentsDeleteOffGrid(false);
		this.particleStorage.ConsumeAndGet(base.smi.EnergyCost());
		gameObject.SetActive(true);
		if (this.destinationSelector.GetDestinationWorld() >= 0)
		{
			RailGunPayload.StatesInstance smi = gameObject.GetSMI<RailGunPayload.StatesInstance>();
			smi.takeoffVelocity = 35f;
			smi.StartSM();
			smi.Launch(base.gameObject.GetMyWorldLocation(), this.destinationSelector.GetDestination());
		}
	}

	// Token: 0x060035D0 RID: 13776 RVA: 0x0012F95D File Offset: 0x0012DB5D
	private ConduitConsumer CreateConduitConsumer(ConduitType inputType, int inputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		ConduitConsumer conduitConsumer = base.gameObject.AddComponent<ConduitConsumer>();
		conduitConsumer.conduitType = inputType;
		conduitConsumer.useSecondaryInput = true;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(inputType);
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(inputType, Endpoint.Sink, inputCell, base.gameObject);
		networkManager.AddToNetworks(inputCell, flowNetworkItem, true);
		return conduitConsumer;
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x0012F997 File Offset: 0x0012DB97
	private SolidConduitConsumer CreateSolidConduitConsumer(int inputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		SolidConduitConsumer solidConduitConsumer = base.gameObject.AddComponent<SolidConduitConsumer>();
		solidConduitConsumer.useSecondaryInput = true;
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, inputCell, base.gameObject);
		Game.Instance.solidConduitSystem.AddToNetworks(inputCell, flowNetworkItem, true);
		return solidConduitConsumer;
	}

	// Token: 0x04002091 RID: 8337
	[Serialize]
	public float launchMass = 200f;

	// Token: 0x04002092 RID: 8338
	public float MinLaunchMass = 2f;

	// Token: 0x04002093 RID: 8339
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002094 RID: 8340
	[MyCmpGet]
	private KAnimControllerBase kac;

	// Token: 0x04002095 RID: 8341
	[MyCmpGet]
	public HighEnergyParticleStorage hepStorage;

	// Token: 0x04002096 RID: 8342
	public Storage resourceStorage;

	// Token: 0x04002097 RID: 8343
	private MeterController resourceMeter;

	// Token: 0x04002098 RID: 8344
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04002099 RID: 8345
	private MeterController particleMeter;

	// Token: 0x0400209A RID: 8346
	private ClusterDestinationSelector destinationSelector;

	// Token: 0x0400209B RID: 8347
	public static readonly Operational.Flag noSurfaceSight = new Operational.Flag("noSurfaceSight", Operational.Flag.Type.Requirement);

	// Token: 0x0400209C RID: 8348
	private static StatusItem noSurfaceSightStatusItem;

	// Token: 0x0400209D RID: 8349
	public static readonly Operational.Flag noDestination = new Operational.Flag("noDestination", Operational.Flag.Type.Requirement);

	// Token: 0x0400209E RID: 8350
	private static StatusItem noDestinationStatusItem;

	// Token: 0x0400209F RID: 8351
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x040020A0 RID: 8352
	private int liquidInputCell = -1;

	// Token: 0x040020A1 RID: 8353
	private FlowUtilityNetwork.NetworkItem liquidNetworkItem;

	// Token: 0x040020A2 RID: 8354
	private ConduitConsumer liquidConsumer;

	// Token: 0x040020A3 RID: 8355
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x040020A4 RID: 8356
	private int gasInputCell = -1;

	// Token: 0x040020A5 RID: 8357
	private FlowUtilityNetwork.NetworkItem gasNetworkItem;

	// Token: 0x040020A6 RID: 8358
	private ConduitConsumer gasConsumer;

	// Token: 0x040020A7 RID: 8359
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x040020A8 RID: 8360
	private int solidInputCell = -1;

	// Token: 0x040020A9 RID: 8361
	private FlowUtilityNetwork.NetworkItem solidNetworkItem;

	// Token: 0x040020AA RID: 8362
	private SolidConduitConsumer solidConsumer;

	// Token: 0x040020AB RID: 8363
	public static readonly HashedString PORT_ID = "LogicLaunching";

	// Token: 0x040020AC RID: 8364
	private bool hasLogicWire;

	// Token: 0x040020AD RID: 8365
	private bool isLogicActive;

	// Token: 0x040020AE RID: 8366
	private static StatusItem infoStatusItemLogic;

	// Token: 0x040020AF RID: 8367
	public bool FreeStartHex;

	// Token: 0x040020B0 RID: 8368
	public bool FreeDestinationHex;

	// Token: 0x040020B1 RID: 8369
	private static readonly EventSystem.IntraObjectHandler<RailGun> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<RailGun>(delegate(RailGun component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0200173D RID: 5949
	public class StatesInstance : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.GameInstance
	{
		// Token: 0x06009A64 RID: 39524 RVA: 0x00390A8E File Offset: 0x0038EC8E
		public StatesInstance(RailGun smi) : base(smi)
		{
		}

		// Token: 0x06009A65 RID: 39525 RVA: 0x00390A97 File Offset: 0x0038EC97
		public bool HasResources()
		{
			return base.smi.master.resourceStorage.MassStored() >= base.smi.master.launchMass;
		}

		// Token: 0x06009A66 RID: 39526 RVA: 0x00390AC3 File Offset: 0x0038ECC3
		public bool HasEnergy()
		{
			return base.smi.master.particleStorage.Particles > this.EnergyCost();
		}

		// Token: 0x06009A67 RID: 39527 RVA: 0x00390AE2 File Offset: 0x0038ECE2
		public bool HasDestination()
		{
			return base.smi.master.destinationSelector.GetDestinationWorld() != base.smi.master.GetMyWorldId();
		}

		// Token: 0x06009A68 RID: 39528 RVA: 0x00390B0E File Offset: 0x0038ED0E
		public bool IsDestinationReachable(bool forceRefresh = false)
		{
			if (forceRefresh)
			{
				this.UpdatePath();
			}
			return base.smi.master.destinationSelector.GetDestinationWorld() != base.smi.master.GetMyWorldId() && this.PathLength() != -1;
		}

		// Token: 0x06009A69 RID: 39529 RVA: 0x00390B50 File Offset: 0x0038ED50
		public int PathLength()
		{
			if (base.smi.m_cachedPath == null)
			{
				this.UpdatePath();
			}
			if (base.smi.m_cachedPath == null)
			{
				return -1;
			}
			int num = base.smi.m_cachedPath.Count;
			if (base.master.FreeStartHex)
			{
				num--;
			}
			if (base.master.FreeDestinationHex)
			{
				num--;
			}
			return num;
		}

		// Token: 0x06009A6A RID: 39530 RVA: 0x00390BB4 File Offset: 0x0038EDB4
		public void UpdatePath()
		{
			this.m_cachedPath = ClusterGrid.Instance.GetPath(base.gameObject.GetMyWorldLocation(), base.smi.master.destinationSelector.GetDestination(), base.smi.master.destinationSelector);
		}

		// Token: 0x06009A6B RID: 39531 RVA: 0x00390C01 File Offset: 0x0038EE01
		public float EnergyCost()
		{
			return Mathf.Max(0f, 0f + (float)this.PathLength() * 10f);
		}

		// Token: 0x06009A6C RID: 39532 RVA: 0x00390C20 File Offset: 0x0038EE20
		public bool MayTurnOn()
		{
			return this.HasEnergy() && this.IsDestinationReachable(false) && base.master.operational.IsOperational && base.sm.allowedFromLogic.Get(this);
		}

		// Token: 0x0400771E RID: 30494
		public const int INVALID_PATH_LENGTH = -1;

		// Token: 0x0400771F RID: 30495
		private List<AxialI> m_cachedPath;
	}

	// Token: 0x0200173E RID: 5950
	public class States : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun>
	{
		// Token: 0x06009A6D RID: 39533 RVA: 0x00390C58 File Offset: 0x0038EE58
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.root.EventHandler(GameHashes.ClusterDestinationChanged, delegate(RailGun.StatesInstance smi)
			{
				smi.UpdatePath();
			});
			this.off.PlayAnim("off").EventTransition(GameHashes.OnParticleStorageChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).EventTransition(GameHashes.ClusterDestinationChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).EventTransition(GameHashes.OperationalChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).ParamTransition<bool>(this.allowedFromLogic, this.on, (RailGun.StatesInstance smi, bool p) => smi.MayTurnOn());
			this.on.DefaultState(this.on.power_on).EventTransition(GameHashes.OperationalChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.ClusterDestinationChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.IsDestinationReachable(false)).EventTransition(GameHashes.ClusterFogOfWarRevealed, (RailGun.StatesInstance smi) => Game.Instance, this.on.power_off, (RailGun.StatesInstance smi) => !smi.IsDestinationReachable(true)).EventTransition(GameHashes.OnParticleStorageChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.MayTurnOn()).ParamTransition<bool>(this.allowedFromLogic, this.on.power_off, (RailGun.StatesInstance smi, bool p) => !p).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null);
			this.on.power_on.PlayAnim("power_on").OnAnimQueueComplete(this.on.wait_for_storage);
			this.on.power_off.PlayAnim("power_off").OnAnimQueueComplete(this.off);
			this.on.wait_for_storage.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.ClusterDestinationChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.HasEnergy()).EventTransition(GameHashes.OnStorageChange, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).EventTransition(GameHashes.OperationalChanged, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).EventTransition(GameHashes.RailGunLaunchMassChanged, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).ParamTransition<float>(this.cooldownTimer, this.on.cooldown, (RailGun.StatesInstance smi, float p) => p > 0f);
			this.on.working.DefaultState(this.on.working.pre).Enter(delegate(RailGun.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(RailGun.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working.loop);
			this.on.working.loop.PlayAnim("working_loop").OnAnimQueueComplete(this.on.working.fire);
			this.on.working.fire.Enter(delegate(RailGun.StatesInstance smi)
			{
				if (smi.IsDestinationReachable(false))
				{
					smi.master.LaunchProjectile();
					smi.sm.payloadsFiredSinceCooldown.Delta(1, smi);
					if (smi.sm.payloadsFiredSinceCooldown.Get(smi) >= 6)
					{
						smi.sm.cooldownTimer.Set(30f, smi, false);
					}
				}
			}).GoTo(this.on.working.bounce);
			this.on.working.bounce.ParamTransition<float>(this.cooldownTimer, this.on.working.pst, (RailGun.StatesInstance smi, float p) => p > 0f || !smi.HasResources()).ParamTransition<int>(this.payloadsFiredSinceCooldown, this.on.working.loop, (RailGun.StatesInstance smi, int p) => p < 6 && smi.HasResources());
			this.on.working.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.wait_for_storage);
			this.on.cooldown.DefaultState(this.on.cooldown.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.RailGunCooldown, null);
			this.on.cooldown.pre.PlayAnim("cooldown_pre").OnAnimQueueComplete(this.on.cooldown.loop);
			this.on.cooldown.loop.PlayAnim("cooldown_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.cooldownTimer, this.on.cooldown.pst, (RailGun.StatesInstance smi, float p) => p <= 0f).Update(delegate(RailGun.StatesInstance smi, float dt)
			{
				this.cooldownTimer.Delta(-dt, smi);
			}, UpdateRate.SIM_1000ms, false);
			this.on.cooldown.pst.PlayAnim("cooldown_pst").OnAnimQueueComplete(this.on.wait_for_storage).Exit(delegate(RailGun.StatesInstance smi)
			{
				smi.sm.payloadsFiredSinceCooldown.Set(0, smi, false);
			});
		}

		// Token: 0x04007720 RID: 30496
		public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State off;

		// Token: 0x04007721 RID: 30497
		public RailGun.States.OnStates on;

		// Token: 0x04007722 RID: 30498
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.FloatParameter cooldownTimer;

		// Token: 0x04007723 RID: 30499
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.IntParameter payloadsFiredSinceCooldown;

		// Token: 0x04007724 RID: 30500
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.BoolParameter allowedFromLogic;

		// Token: 0x04007725 RID: 30501
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.BoolParameter updatePath;

		// Token: 0x02002928 RID: 10536
		public class WorkingStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x0400B5FB RID: 46587
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pre;

			// Token: 0x0400B5FC RID: 46588
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State loop;

			// Token: 0x0400B5FD RID: 46589
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State fire;

			// Token: 0x0400B5FE RID: 46590
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State bounce;

			// Token: 0x0400B5FF RID: 46591
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pst;
		}

		// Token: 0x02002929 RID: 10537
		public class CooldownStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x0400B600 RID: 46592
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pre;

			// Token: 0x0400B601 RID: 46593
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State loop;

			// Token: 0x0400B602 RID: 46594
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pst;
		}

		// Token: 0x0200292A RID: 10538
		public class OnStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x0400B603 RID: 46595
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State power_on;

			// Token: 0x0400B604 RID: 46596
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State wait_for_storage;

			// Token: 0x0400B605 RID: 46597
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State power_off;

			// Token: 0x0400B606 RID: 46598
			public RailGun.States.WorkingStates working;

			// Token: 0x0400B607 RID: 46599
			public RailGun.States.CooldownStates cooldown;
		}
	}
}
