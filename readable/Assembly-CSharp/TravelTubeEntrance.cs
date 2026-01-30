using System;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200081F RID: 2079
[SerializationConfig(MemberSerialization.OptIn)]
public class TravelTubeEntrance : StateMachineComponent<TravelTubeEntrance.SMInstance>, ISaveLoadable, ISim200ms
{
	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06003870 RID: 14448 RVA: 0x0013BD33 File Offset: 0x00139F33
	public float AvailableJoules
	{
		get
		{
			return this.availableJoules;
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06003871 RID: 14449 RVA: 0x0013BD3B File Offset: 0x00139F3B
	public float TotalCapacity
	{
		get
		{
			return this.jouleCapacity;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06003872 RID: 14450 RVA: 0x0013BD43 File Offset: 0x00139F43
	public float UsageJoules
	{
		get
		{
			return this.joulesPerLaunch;
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06003873 RID: 14451 RVA: 0x0013BD4B File Offset: 0x00139F4B
	public bool HasLaunchPower
	{
		get
		{
			return this.availableJoules > this.joulesPerLaunch;
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x06003874 RID: 14452 RVA: 0x0013BD5B File Offset: 0x00139F5B
	public bool HasWaxForGreasyLaunch
	{
		get
		{
			return this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) >= this.waxPerLaunch;
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x06003875 RID: 14453 RVA: 0x0013BD7D File Offset: 0x00139F7D
	public int WaxLaunchesAvailable
	{
		get
		{
			return Mathf.FloorToInt(this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) / this.waxPerLaunch);
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x06003876 RID: 14454 RVA: 0x0013BDA0 File Offset: 0x00139FA0
	private bool ShouldUseWaxLaunchAnimation
	{
		get
		{
			return this.deliverAndUseWax && this.HasWaxForGreasyLaunch;
		}
	}

	// Token: 0x06003877 RID: 14455 RVA: 0x0013BDB4 File Offset: 0x00139FB4
	public static void SetTravelerGleamEffect(TravelTubeEntrance.SMInstance smi)
	{
		TravelTubeEntrance.Work component = smi.GetComponent<TravelTubeEntrance.Work>();
		if (component.worker != null)
		{
			component.worker.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("gleam", smi.master.ShouldUseWaxLaunchAnimation);
		}
	}

	// Token: 0x06003878 RID: 14456 RVA: 0x0013BDFB File Offset: 0x00139FFB
	public static string GetLaunchAnimName(TravelTubeEntrance.SMInstance smi)
	{
		if (!smi.master.ShouldUseWaxLaunchAnimation)
		{
			return "working_pre";
		}
		return "wax";
	}

	// Token: 0x06003879 RID: 14457 RVA: 0x0013BE15 File Offset: 0x0013A015
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.energyConsumer.OnConnectionChanged += this.OnConnectionChanged;
	}

	// Token: 0x0600387A RID: 14458 RVA: 0x0013BE34 File Offset: 0x0013A034
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetWaxUse(this.deliverAndUseWax);
		int x = (int)base.transform.GetPosition().x;
		int y = (int)base.transform.GetPosition().y + 2;
		Extents extents = new Extents(x, y, 1, 1);
		UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(Grid.XYToCell(x, y), true);
		this.TubeConnectionsChanged(connections);
		this.tubeChangedEntry = GameScenePartitioner.Instance.Add("TravelTubeEntrance.TubeListener", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[35], new Action<object>(this.TubeChanged));
		base.Subscribe<TravelTubeEntrance>(-592767678, TravelTubeEntrance.OnOperationalChangedDelegate);
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.waxMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "wax_meter_target", "wax_meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.CreateNewWaitReactable();
		Grid.RegisterTubeEntrance(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		base.smi.StartSM();
		this.UpdateWaxCharge();
		this.UpdateCharge();
		base.Subscribe<TravelTubeEntrance>(493375141, TravelTubeEntrance.OnRefreshUserMenuDelegate);
	}

	// Token: 0x0600387B RID: 14459 RVA: 0x0013BF83 File Offset: 0x0013A183
	private void OnStorageChanged(object obj)
	{
		this.UpdateWaxCharge();
	}

	// Token: 0x0600387C RID: 14460 RVA: 0x0013BF8C File Offset: 0x0013A18C
	protected override void OnCleanUp()
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		Grid.UnregisterTubeEntrance(Grid.PosToCell(this));
		this.ClearWaitReactable();
		GameScenePartitioner.Instance.Free(ref this.tubeChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x0600387D RID: 14461 RVA: 0x0013BFF1 File Offset: 0x0013A1F1
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, base.smi.buttonInfo, 1f);
		this.RefreshStatusItem();
	}

	// Token: 0x0600387E RID: 14462 RVA: 0x0013C020 File Offset: 0x0013A220
	public void RefreshStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		bool flag = this.deliverAndUseWax && this.WaxLaunchesAvailable > 0;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, this);
				return;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, false);
		}
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x0013C084 File Offset: 0x0013A284
	public void SetWaxUse(bool usingWax)
	{
		this.deliverAndUseWax = usingWax;
		this.manualDelivery.AbortDelivery("Switching to new delivery request");
		this.manualDelivery.capacity = (usingWax ? this.storage.capacityKg : 0f);
		this.manualDelivery.refillMass = (usingWax ? this.waxPerLaunch : 0f);
		this.manualDelivery.MinimumMass = (usingWax ? this.waxPerLaunch : 0f);
		if (!usingWax)
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		base.smi.RefreshUIButton();
		this.RefreshStatusItem();
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x0013C12C File Offset: 0x0013A32C
	private void TubeChanged(object data)
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		GameObject gameObject = data as GameObject;
		if (data == null)
		{
			this.TubeConnectionsChanged((UtilityConnections)0);
			return;
		}
		TravelTube component = gameObject.GetComponent<TravelTube>();
		if (component != null)
		{
			component.Subscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = component;
			return;
		}
		this.TubeConnectionsChanged((UtilityConnections)0);
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x0013C1B3 File Offset: 0x0013A3B3
	private void TubeConnectionsChanged(object data)
	{
		this.TubeConnectionsChanged(((Boxed<UtilityConnections>)data).value);
	}

	// Token: 0x06003882 RID: 14466 RVA: 0x0013C1C8 File Offset: 0x0013A3C8
	private void TubeConnectionsChanged(UtilityConnections connections)
	{
		bool value = connections == UtilityConnections.Up;
		this.operational.SetFlag(TravelTubeEntrance.tubeConnected, value);
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x0013C1EC File Offset: 0x0013A3EC
	private bool CanAcceptMorePower()
	{
		return this.operational.IsOperational && (this.button == null || this.button.IsEnabled) && this.energyConsumer.IsExternallyPowered && this.availableJoules < this.jouleCapacity;
	}

	// Token: 0x06003884 RID: 14468 RVA: 0x0013C240 File Offset: 0x0013A440
	public void Sim200ms(float dt)
	{
		if (this.CanAcceptMorePower())
		{
			this.availableJoules = Mathf.Min(this.jouleCapacity, this.availableJoules + this.energyConsumer.WattsUsed * dt);
			this.UpdateCharge();
		}
		this.energyConsumer.SetSustained(this.HasLaunchPower);
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x06003885 RID: 14469 RVA: 0x0013C29D File Offset: 0x0013A49D
	public void Reserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, true);
	}

	// Token: 0x06003886 RID: 14470 RVA: 0x0013C2AD File Offset: 0x0013A4AD
	public void Unreserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, false);
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x0013C2BD File Offset: 0x0013A4BD
	public bool IsTraversable(Navigator agent)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x0013C2DA File Offset: 0x0013A4DA
	public bool HasChargeSlotReserved(Navigator agent)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x06003889 RID: 14473 RVA: 0x0013C2F7 File Offset: 0x0013A4F7
	public bool HasChargeSlotReserved(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x0600388A RID: 14474 RVA: 0x0013C305 File Offset: 0x0013A505
	public bool IsChargedSlotAvailable(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x0600388B RID: 14475 RVA: 0x0013C314 File Offset: 0x0013A514
	public bool ShouldWait(GameObject reactor)
	{
		if (!this.operational.IsOperational)
		{
			return false;
		}
		if (!this.HasLaunchPower)
		{
			return false;
		}
		if (this.launch_workable.worker == null)
		{
			return false;
		}
		TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
		return this.HasChargeSlotReserved(smi, reactor.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x0600388C RID: 14476 RVA: 0x0013C368 File Offset: 0x0013A568
	public void ConsumeCharge(GameObject reactor)
	{
		if (this.HasLaunchPower)
		{
			this.availableJoules -= this.joulesPerLaunch;
			if (this.deliverAndUseWax && this.HasWaxForGreasyLaunch)
			{
				TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
				if (smi != null)
				{
					Tag tag = SimHashes.MilkFat.CreateTag();
					float num;
					SimUtil.DiseaseInfo diseaseInfo;
					float num2;
					this.storage.ConsumeAndGetDisease(tag, this.waxPerLaunch, out num, out diseaseInfo, out num2);
					GermExposureMonitor.Instance smi2 = reactor.GetSMI<GermExposureMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, tag, Sickness.InfectionVector.Contact);
					}
					smi.SetWaxState(true);
				}
			}
			this.UpdateCharge();
			this.UpdateWaxCharge();
		}
	}

	// Token: 0x0600388D RID: 14477 RVA: 0x0013C404 File Offset: 0x0013A604
	private void CreateNewWaitReactable()
	{
		if (this.wait_reactable == null)
		{
			this.wait_reactable = new TravelTubeEntrance.WaitReactable(this);
		}
	}

	// Token: 0x0600388E RID: 14478 RVA: 0x0013C41A File Offset: 0x0013A61A
	private void OrphanWaitReactable()
	{
		this.wait_reactable = null;
	}

	// Token: 0x0600388F RID: 14479 RVA: 0x0013C423 File Offset: 0x0013A623
	private void ClearWaitReactable()
	{
		if (this.wait_reactable != null)
		{
			this.wait_reactable.Cleanup();
			this.wait_reactable = null;
		}
	}

	// Token: 0x06003890 RID: 14480 RVA: 0x0013C440 File Offset: 0x0013A640
	private void OnOperationalChanged(object data)
	{
		bool value = ((Boxed<bool>)data).value;
		Grid.SetTubeEntranceOperational(Grid.PosToCell(this), value);
		this.UpdateActive();
	}

	// Token: 0x06003891 RID: 14481 RVA: 0x0013C46B File Offset: 0x0013A66B
	private void OnConnectionChanged()
	{
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x06003892 RID: 14482 RVA: 0x0013C479 File Offset: 0x0013A679
	private void UpdateActive()
	{
		this.operational.SetActive(this.CanAcceptMorePower(), false);
	}

	// Token: 0x06003893 RID: 14483 RVA: 0x0013C490 File Offset: 0x0013A690
	private void UpdateCharge()
	{
		base.smi.sm.hasLaunchCharges.Set(this.HasLaunchPower, base.smi, false);
		float positionPercent = Mathf.Clamp01(this.availableJoules / this.jouleCapacity);
		this.meter.SetPositionPercent(positionPercent);
		this.energyConsumer.UpdatePoweredStatus();
		Grid.SetTubeEntranceReservationCapacity(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		base.smi.RefreshUIButton();
		this.RefreshStatusItem();
	}

	// Token: 0x06003894 RID: 14484 RVA: 0x0013C518 File Offset: 0x0013A718
	private void UpdateWaxCharge()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.waxMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06003895 RID: 14485 RVA: 0x0013C550 File Offset: 0x0013A750
	private void UpdateConnectionStatus()
	{
		bool flag = this.button != null && !this.button.IsEnabled;
		bool isConnected = this.energyConsumer.IsConnected;
		bool hasLaunchPower = this.HasLaunchPower;
		if (flag || !isConnected || hasLaunchPower)
		{
			this.connectedStatus = this.selectable.RemoveStatusItem(this.connectedStatus, false);
			return;
		}
		if (this.connectedStatus == Guid.Empty)
		{
			this.connectedStatus = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotEnoughPower, null);
		}
	}

	// Token: 0x04002249 RID: 8777
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400224A RID: 8778
	[MyCmpReq]
	private TravelTubeEntrance.Work launch_workable;

	// Token: 0x0400224B RID: 8779
	[MyCmpReq]
	private EnergyConsumerSelfSustaining energyConsumer;

	// Token: 0x0400224C RID: 8780
	[MyCmpGet]
	private BuildingEnabledButton button;

	// Token: 0x0400224D RID: 8781
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400224E RID: 8782
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400224F RID: 8783
	[MyCmpReq]
	private ManualDeliveryKG manualDelivery;

	// Token: 0x04002250 RID: 8784
	public float jouleCapacity = 1f;

	// Token: 0x04002251 RID: 8785
	public float joulesPerLaunch = 1f;

	// Token: 0x04002252 RID: 8786
	public float waxPerLaunch;

	// Token: 0x04002253 RID: 8787
	[Serialize]
	private float availableJoules;

	// Token: 0x04002254 RID: 8788
	[Serialize]
	private bool deliverAndUseWax;

	// Token: 0x04002255 RID: 8789
	private TravelTube travelTube;

	// Token: 0x04002256 RID: 8790
	public const string WAX_LAUNCH_ANIM_NAME = "wax";

	// Token: 0x04002257 RID: 8791
	private TravelTubeEntrance.WaitReactable wait_reactable;

	// Token: 0x04002258 RID: 8792
	private MeterController meter;

	// Token: 0x04002259 RID: 8793
	private MeterController waxMeter;

	// Token: 0x0400225A RID: 8794
	private const int MAX_CHARGES = 3;

	// Token: 0x0400225B RID: 8795
	private const float RECHARGE_TIME = 10f;

	// Token: 0x0400225C RID: 8796
	private static readonly Operational.Flag tubeConnected = new Operational.Flag("tubeConnected", Operational.Flag.Type.Functional);

	// Token: 0x0400225D RID: 8797
	private HandleVector<int>.Handle tubeChangedEntry;

	// Token: 0x0400225E RID: 8798
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0400225F RID: 8799
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002260 RID: 8800
	private Guid connectedStatus;

	// Token: 0x020017B8 RID: 6072
	private class LaunchReactable : WorkableReactable
	{
		// Token: 0x06009C4D RID: 40013 RVA: 0x00398E74 File Offset: 0x00397074
		public LaunchReactable(Workable workable, TravelTubeEntrance entrance) : base(workable, "LaunchReactable", Db.Get().ChoreTypes.TravelTubeEntrance, WorkableReactable.AllowedDirection.Any)
		{
			this.entrance = entrance;
		}

		// Token: 0x06009C4E RID: 40014 RVA: 0x00398EA0 File Offset: 0x003970A0
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Navigator component = new_reactor.GetComponent<Navigator>();
				return component && this.entrance.HasChargeSlotReserved(component);
			}
			return false;
		}

		// Token: 0x0400788D RID: 30861
		private TravelTubeEntrance entrance;
	}

	// Token: 0x020017B9 RID: 6073
	private class WaitReactable : Reactable
	{
		// Token: 0x06009C4F RID: 40015 RVA: 0x00398ED8 File Offset: 0x003970D8
		public WaitReactable(TravelTubeEntrance entrance) : base(entrance.gameObject, "WaitReactable", Db.Get().ChoreTypes.TravelTubeEntrance, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.entrance = entrance;
			this.preventChoreInterruption = false;
		}

		// Token: 0x06009C50 RID: 40016 RVA: 0x00398F31 File Offset: 0x00397131
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.entrance == null)
			{
				base.Cleanup();
				return false;
			}
			return this.entrance.ShouldWait(new_reactor);
		}

		// Token: 0x06009C51 RID: 40017 RVA: 0x00398F68 File Offset: 0x00397168
		protected override void InternalBegin()
		{
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.entrance.OrphanWaitReactable();
			this.entrance.CreateNewWaitReactable();
		}

		// Token: 0x06009C52 RID: 40018 RVA: 0x00398FE5 File Offset: 0x003971E5
		public override void Update(float dt)
		{
			if (this.entrance == null)
			{
				base.Cleanup();
				return;
			}
			if (!this.entrance.ShouldWait(this.reactor))
			{
				base.Cleanup();
			}
		}

		// Token: 0x06009C53 RID: 40019 RVA: 0x00399015 File Offset: 0x00397215
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x06009C54 RID: 40020 RVA: 0x00399044 File Offset: 0x00397244
		protected override void InternalCleanup()
		{
		}

		// Token: 0x0400788E RID: 30862
		private TravelTubeEntrance entrance;
	}

	// Token: 0x020017BA RID: 6074
	public class States : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance>
	{
		// Token: 0x06009C55 RID: 40021 RVA: 0x00399048 File Offset: 0x00397248
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notoperational;
			this.root.ToggleStatusItem(Db.Get().BuildingStatusItems.StoredCharge, null);
			this.notoperational.DefaultState(this.notoperational.normal).PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.notoperational.normal.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.notube, (TravelTubeEntrance.SMInstance smi) => !smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected));
			this.notoperational.notube.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.normal, (TravelTubeEntrance.SMInstance smi) => smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected)).ToggleStatusItem(Db.Get().BuildingStatusItems.NoTubeConnected, null);
			this.notready.PlayAnim("off").ParamTransition<bool>(this.hasLaunchCharges, this.ready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((TravelTubeEntrance.SMInstance smi) => new TravelTubeEntrance.LaunchReactable(smi.master.GetComponent<TravelTubeEntrance.Work>(), smi.master.GetComponent<TravelTubeEntrance>())).ParamTransition<bool>(this.hasLaunchCharges, this.notready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => !hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim(new Func<TravelTubeEntrance.SMInstance, string>(TravelTubeEntrance.GetLaunchAnimName), KAnim.PlayMode.Once).QueueAnim("working_loop", true, null).Enter(new StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State.Callback(TravelTubeEntrance.SetTravelerGleamEffect)).WorkableStopTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x0400788F RID: 30863
		public StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.BoolParameter hasLaunchCharges;

		// Token: 0x04007890 RID: 30864
		public TravelTubeEntrance.States.NotOperationalStates notoperational;

		// Token: 0x04007891 RID: 30865
		public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notready;

		// Token: 0x04007892 RID: 30866
		public TravelTubeEntrance.States.ReadyStates ready;

		// Token: 0x0200295C RID: 10588
		public class NotOperationalStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x0400B711 RID: 46865
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State normal;

			// Token: 0x0400B712 RID: 46866
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notube;
		}

		// Token: 0x0200295D RID: 10589
		public class ReadyStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x0400B713 RID: 46867
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State free;

			// Token: 0x0400B714 RID: 46868
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State occupied;

			// Token: 0x0400B715 RID: 46869
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State post;
		}
	}

	// Token: 0x020017BB RID: 6075
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x06009C57 RID: 40023 RVA: 0x003992E6 File Offset: 0x003974E6
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.showProgressBar = false;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_tube_launcher_kanim")
			};
			this.workLayer = Grid.SceneLayer.BuildingUse;
		}

		// Token: 0x06009C58 RID: 40024 RVA: 0x00399322 File Offset: 0x00397522
		protected override void OnStartWork(WorkerBase worker)
		{
			base.SetWorkTime(1f);
		}

		// Token: 0x04007893 RID: 30867
		public const string DEFAULT_LAUNCH_ANIM_NAME = "anim_interacts_tube_launcher_kanim";
	}

	// Token: 0x020017BC RID: 6076
	public class SMInstance : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.GameInstance
	{
		// Token: 0x06009C5A RID: 40026 RVA: 0x00399338 File Offset: 0x00397538
		public SMInstance(TravelTubeEntrance master) : base(master)
		{
			this.buttonInfo = new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.TRANSITTUBEWAX.NAME, delegate()
			{
				master.SetWaxUse(true);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.TRANSITTUBEWAX.TOOLTIP, true);
		}

		// Token: 0x06009C5B RID: 40027 RVA: 0x00399398 File Offset: 0x00397598
		public void RefreshUIButton()
		{
			if (!base.master.deliverAndUseWax)
			{
				this.buttonInfo.iconName = "action_speed_up";
				this.buttonInfo.text = UI.USERMENUACTIONS.TRANSITTUBEWAX.NAME;
				this.buttonInfo.onClick = delegate()
				{
					base.master.SetWaxUse(true);
				};
				this.buttonInfo.tooltipText = UI.USERMENUACTIONS.TRANSITTUBEWAX.TOOLTIP;
				return;
			}
			this.buttonInfo.iconName = "action_speed_up";
			this.buttonInfo.text = UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.NAME;
			this.buttonInfo.onClick = delegate()
			{
				base.master.SetWaxUse(false);
			};
			this.buttonInfo.tooltipText = UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.TOOLTIP;
		}

		// Token: 0x04007894 RID: 30868
		public KIconButtonMenu.ButtonInfo buttonInfo;
	}
}
