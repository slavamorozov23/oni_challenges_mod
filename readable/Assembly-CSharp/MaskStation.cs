using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020007B3 RID: 1971
public class MaskStation : StateMachineComponent<MaskStation.SMInstance>, IBasicBuilding
{
	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06003402 RID: 13314 RVA: 0x0012768E File Offset: 0x0012588E
	// (set) Token: 0x06003403 RID: 13315 RVA: 0x0012769B File Offset: 0x0012589B
	private bool isRotated
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06003404 RID: 13316 RVA: 0x001276A5 File Offset: 0x001258A5
	// (set) Token: 0x06003405 RID: 13317 RVA: 0x001276B2 File Offset: 0x001258B2
	private bool isOperational
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
		}
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x001276BC File Offset: 0x001258BC
	public void UpdateOperational()
	{
		bool flag = this.GetTotalOxygenAmount() >= this.oxygenConsumedPerMask * (float)this.maxUses;
		this.shouldPump = this.IsPumpable();
		if (this.operational.IsOperational && this.shouldPump && !flag)
		{
			this.operational.SetActive(true, false);
		}
		else
		{
			this.operational.SetActive(false, false);
		}
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidMaskStationConsumptionState, this.noElementStatusGuid, !this.shouldPump, null);
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x00127754 File Offset: 0x00125954
	private bool IsPumpable()
	{
		ElementConsumer[] components = base.GetComponents<ElementConsumer>();
		int num = Grid.PosToCell(base.transform.GetPosition());
		bool result = false;
		foreach (ElementConsumer elementConsumer in components)
		{
			for (int j = 0; j < (int)elementConsumer.consumptionRadius; j++)
			{
				for (int k = 0; k < (int)elementConsumer.consumptionRadius; k++)
				{
					int num2 = num + k + Grid.WidthInCells * j;
					bool flag = Grid.Element[num2].IsState(Element.State.Gas);
					bool flag2 = Grid.Element[num2].id == elementConsumer.elementToConsume;
					if (flag && flag2)
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06003408 RID: 13320 RVA: 0x001277F8 File Offset: 0x001259F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, null, false, fetch_chore_type);
	}

	// Token: 0x06003409 RID: 13321 RVA: 0x00127834 File Offset: 0x00125A34
	private List<GameObject> GetPossibleMaterials()
	{
		List<GameObject> result = new List<GameObject>();
		this.materialStorage.Find(this.materialTag, result);
		return result;
	}

	// Token: 0x0600340A RID: 13322 RVA: 0x0012785B File Offset: 0x00125A5B
	private float GetTotalMaterialAmount()
	{
		return this.materialStorage.GetMassAvailable(this.materialTag);
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x0012786E File Offset: 0x00125A6E
	private float GetTotalOxygenAmount()
	{
		return this.oxygenStorage.GetMassAvailable(this.oxygenTag);
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x00127884 File Offset: 0x00125A84
	private void RefreshMeters()
	{
		float num = this.GetTotalMaterialAmount();
		num = Mathf.Clamp01(num / ((float)this.maxUses * this.materialConsumedPerMask));
		float num2 = this.GetTotalOxygenAmount();
		num2 = Mathf.Clamp01(num2 / ((float)this.maxUses * this.oxygenConsumedPerMask));
		this.materialsMeter.SetPositionPercent(num);
		this.oxygenMeter.SetPositionPercent(num2);
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x001278E4 File Offset: 0x00125AE4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.CreateNewReactable();
		this.cell = Grid.PosToCell(this);
		Grid.RegisterSuitMarker(this.cell);
		this.isOperational = base.GetComponent<Operational>().IsOperational;
		base.Subscribe<MaskStation>(-592767678, MaskStation.OnOperationalChangedDelegate);
		this.isRotated = base.GetComponent<Rotatable>().IsRotated;
		base.Subscribe<MaskStation>(-1643076535, MaskStation.OnRotatedDelegate);
		this.materialsMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_resources_target", "meter_resources", this.materialsMeterOffset, Grid.SceneLayer.BuildingBack, new string[]
		{
			"meter_resources_target"
		});
		this.oxygenMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_oxygen_target", "meter_oxygen", this.oxygenMeterOffset, Grid.SceneLayer.BuildingFront, new string[]
		{
			"meter_oxygen_target"
		});
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
		base.Subscribe<MaskStation>(-1697596308, MaskStation.OnStorageChangeDelegate);
		this.RefreshMeters();
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x001279F0 File Offset: 0x00125BF0
	private void Update()
	{
		float a = this.GetTotalMaterialAmount() / this.materialConsumedPerMask;
		float b = this.GetTotalOxygenAmount() / this.oxygenConsumedPerMask;
		int fullLockerCount = (int)Mathf.Min(a, b);
		int emptyLockerCount = 0;
		Grid.UpdateSuitMarker(this.cell, fullLockerCount, emptyLockerCount, this.gridFlags, this.PathFlag);
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x00127A3C File Offset: 0x00125C3C
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		if (base.isSpawned)
		{
			Grid.UnregisterSuitMarker(this.cell);
		}
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
		}
		base.OnCleanUp();
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x00127A88 File Offset: 0x00125C88
	private void OnOperationalChanged(bool isOperational)
	{
		this.isOperational = isOperational;
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x00127A91 File Offset: 0x00125C91
	private void OnStorageChange(object data)
	{
		this.RefreshMeters();
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x00127A99 File Offset: 0x00125C99
	private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
	{
		if (state)
		{
			this.gridFlags |= flag;
			return;
		}
		this.gridFlags &= ~flag;
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x00127ABD File Offset: 0x00125CBD
	private void CreateNewReactable()
	{
		this.reactable = new MaskStation.OxygenMaskReactable(this);
	}

	// Token: 0x04001F60 RID: 8032
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04001F61 RID: 8033
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnOperationalChanged(((Boxed<bool>)data).value);
	});

	// Token: 0x04001F62 RID: 8034
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.isRotated = ((Rotatable)data).IsRotated;
	});

	// Token: 0x04001F63 RID: 8035
	public float materialConsumedPerMask = 1f;

	// Token: 0x04001F64 RID: 8036
	public float oxygenConsumedPerMask = 1f;

	// Token: 0x04001F65 RID: 8037
	public Tag materialTag = GameTags.Metal;

	// Token: 0x04001F66 RID: 8038
	public Tag oxygenTag = GameTags.Breathable;

	// Token: 0x04001F67 RID: 8039
	public int maxUses = 10;

	// Token: 0x04001F68 RID: 8040
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001F69 RID: 8041
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04001F6A RID: 8042
	public Storage materialStorage;

	// Token: 0x04001F6B RID: 8043
	public Storage oxygenStorage;

	// Token: 0x04001F6C RID: 8044
	private bool shouldPump;

	// Token: 0x04001F6D RID: 8045
	private MaskStation.OxygenMaskReactable reactable;

	// Token: 0x04001F6E RID: 8046
	private MeterController materialsMeter;

	// Token: 0x04001F6F RID: 8047
	private MeterController oxygenMeter;

	// Token: 0x04001F70 RID: 8048
	public Meter.Offset materialsMeterOffset = Meter.Offset.Behind;

	// Token: 0x04001F71 RID: 8049
	public Meter.Offset oxygenMeterOffset;

	// Token: 0x04001F72 RID: 8050
	public string choreTypeID;

	// Token: 0x04001F73 RID: 8051
	protected FilteredStorage filteredStorage;

	// Token: 0x04001F74 RID: 8052
	public KAnimFile interactAnim = Assets.GetAnim("anim_equip_clothing_kanim");

	// Token: 0x04001F75 RID: 8053
	private int cell;

	// Token: 0x04001F76 RID: 8054
	public PathFinder.PotentialPath.Flags PathFlag;

	// Token: 0x04001F77 RID: 8055
	private Guid noElementStatusGuid;

	// Token: 0x04001F78 RID: 8056
	private Grid.SuitMarker.Flags gridFlags;

	// Token: 0x020016E1 RID: 5857
	private class OxygenMaskReactable : Reactable
	{
		// Token: 0x060098DF RID: 39135 RVA: 0x0038A1B0 File Offset: 0x003883B0
		public OxygenMaskReactable(MaskStation mask_station) : base(mask_station.gameObject, "OxygenMask", Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.maskStation = mask_station;
		}

		// Token: 0x060098E0 RID: 39136 RVA: 0x0038A204 File Offset: 0x00388404
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.maskStation == null)
			{
				base.Cleanup();
				return false;
			}
			bool flag = !new_reactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			int x = transition.navGridTransition.x;
			if (x == 0)
			{
				return false;
			}
			if (!flag)
			{
				return (x >= 0 || !this.maskStation.isRotated) && (x <= 0 || this.maskStation.isRotated);
			}
			return this.maskStation.smi.IsReady() && (x <= 0 || !this.maskStation.isRotated) && (x >= 0 || this.maskStation.isRotated);
		}

		// Token: 0x060098E1 RID: 39137 RVA: 0x0038A2D4 File Offset: 0x003884D4
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(this.maskStation.interactAnim, 1f);
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.maskStation.CreateNewReactable();
		}

		// Token: 0x060098E2 RID: 39138 RVA: 0x0038A368 File Offset: 0x00388568
		public override void Update(float dt)
		{
			Facing facing = this.reactor ? this.reactor.GetComponent<Facing>() : null;
			if (facing && this.maskStation)
			{
				facing.SetFacing(this.maskStation.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
			}
			if (Time.time - this.startTime > 2.8f)
			{
				this.Run();
				base.Cleanup();
			}
		}

		// Token: 0x060098E3 RID: 39139 RVA: 0x0038A3E0 File Offset: 0x003885E0
		private void Run()
		{
			GameObject reactor = this.reactor;
			Equipment equipment = reactor.GetComponent<MinionIdentity>().GetEquipment();
			bool flag = !equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			Navigator component = reactor.GetComponent<Navigator>();
			bool flag2 = component != null && (component.flags & this.maskStation.PathFlag) > PathFinder.PotentialPath.Flags.None;
			if (flag)
			{
				if (!this.maskStation.smi.IsReady())
				{
					return;
				}
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Oxygen_Mask".ToTag()), null, null);
				gameObject.SetActive(true);
				SimHashes elementID = this.maskStation.GetPossibleMaterials()[0].GetComponent<PrimaryElement>().ElementID;
				gameObject.GetComponent<PrimaryElement>().SetElement(elementID, false);
				SuitTank component2 = gameObject.GetComponent<SuitTank>();
				this.maskStation.materialStorage.ConsumeIgnoringDisease(this.maskStation.materialTag, this.maskStation.materialConsumedPerMask);
				this.maskStation.oxygenStorage.Transfer(component2.storage, component2.elementTag, this.maskStation.oxygenConsumedPerMask, false, true);
				Equippable component3 = gameObject.GetComponent<Equippable>();
				component3.Assign(equipment.GetComponent<IAssignableIdentity>());
				component3.isEquipped = true;
			}
			if (!flag)
			{
				Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
				assignable.Unassign();
				if (!flag2)
				{
					Notification notification = new Notification(MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP, null, true, 0f, null, null, null, true, false, false);
					assignable.GetComponent<Notifier>().Add(notification, "");
				}
			}
		}

		// Token: 0x060098E4 RID: 39140 RVA: 0x0038A587 File Offset: 0x00388787
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.maskStation.interactAnim);
			}
		}

		// Token: 0x060098E5 RID: 39141 RVA: 0x0038A5B2 File Offset: 0x003887B2
		protected override void InternalCleanup()
		{
		}

		// Token: 0x04007605 RID: 30213
		private MaskStation maskStation;

		// Token: 0x04007606 RID: 30214
		private float startTime;
	}

	// Token: 0x020016E2 RID: 5858
	public class SMInstance : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.GameInstance
	{
		// Token: 0x060098E6 RID: 39142 RVA: 0x0038A5B4 File Offset: 0x003887B4
		public SMInstance(MaskStation master) : base(master)
		{
		}

		// Token: 0x060098E7 RID: 39143 RVA: 0x0038A5BD File Offset: 0x003887BD
		private bool HasSufficientMaterials()
		{
			return base.master.GetTotalMaterialAmount() >= base.master.materialConsumedPerMask;
		}

		// Token: 0x060098E8 RID: 39144 RVA: 0x0038A5DA File Offset: 0x003887DA
		private bool HasSufficientOxygen()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask;
		}

		// Token: 0x060098E9 RID: 39145 RVA: 0x0038A5F7 File Offset: 0x003887F7
		public bool OxygenIsFull()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask * (float)base.master.maxUses;
		}

		// Token: 0x060098EA RID: 39146 RVA: 0x0038A621 File Offset: 0x00388821
		public bool IsReady()
		{
			return this.HasSufficientMaterials() && this.HasSufficientOxygen();
		}
	}

	// Token: 0x020016E3 RID: 5859
	public class States : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation>
	{
		// Token: 0x060098EB RID: 39147 RVA: 0x0038A638 File Offset: 0x00388838
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notOperational;
			this.notOperational.PlayAnim("off").TagTransition(GameTags.Operational, this.charging, false);
			this.charging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.notCharging, (MaskStation.SMInstance smi) => smi.OxygenIsFull() || !smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (smi.OxygenIsFull() || !smi.master.shouldPump)
				{
					smi.GoTo(this.notCharging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.charging.openChargingPre);
					return;
				}
				smi.GoTo(this.charging.closedChargingPre);
			});
			this.charging.opening.QueueAnim("opening_charging", false, null).OnAnimQueueComplete(this.charging.open);
			this.charging.open.PlayAnim("open_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.charging.closing.QueueAnim("closing_charging", false, null).OnAnimQueueComplete(this.charging.closed);
			this.charging.closed.PlayAnim("closed_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.charging.openChargingPre.PlayAnim("open_charging_pre").OnAnimQueueComplete(this.charging.open);
			this.charging.closedChargingPre.PlayAnim("closed_charging_pre").OnAnimQueueComplete(this.charging.closed);
			this.notCharging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.charging, (MaskStation.SMInstance smi) => !smi.OxygenIsFull() && smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (!smi.OxygenIsFull() && smi.master.shouldPump)
				{
					smi.GoTo(this.charging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.notCharging.openChargingPst);
					return;
				}
				smi.GoTo(this.notCharging.closedChargingPst);
			});
			this.notCharging.opening.PlayAnim("opening_not_charging").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.open.PlayAnim("open_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.notCharging.closing.PlayAnim("closing_not_charging").OnAnimQueueComplete(this.notCharging.closed);
			this.notCharging.closed.PlayAnim("closed_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.notCharging.openChargingPst.PlayAnim("open_charging_pst").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.closedChargingPst.PlayAnim("closed_charging_pst").OnAnimQueueComplete(this.notCharging.closed);
		}

		// Token: 0x04007607 RID: 30215
		public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State notOperational;

		// Token: 0x04007608 RID: 30216
		public MaskStation.States.ChargingStates charging;

		// Token: 0x04007609 RID: 30217
		public MaskStation.States.NotChargingStates notCharging;

		// Token: 0x02002913 RID: 10515
		public class ChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x0400B586 RID: 46470
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x0400B587 RID: 46471
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x0400B588 RID: 46472
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x0400B589 RID: 46473
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x0400B58A RID: 46474
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPre;

			// Token: 0x0400B58B RID: 46475
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPre;
		}

		// Token: 0x02002914 RID: 10516
		public class NotChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x0400B58C RID: 46476
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x0400B58D RID: 46477
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x0400B58E RID: 46478
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x0400B58F RID: 46479
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x0400B590 RID: 46480
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPst;

			// Token: 0x0400B591 RID: 46481
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPst;
		}
	}
}
