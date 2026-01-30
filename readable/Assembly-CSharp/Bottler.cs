using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000711 RID: 1809
[AddComponentMenu("KMonoBehaviour/Workable/Bottler")]
public class Bottler : Workable, IUserControlledCapacity
{
	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06002D0B RID: 11531 RVA: 0x001057BB File Offset: 0x001039BB
	// (set) Token: 0x06002D0C RID: 11532 RVA: 0x001057E7 File Offset: 0x001039E7
	public float UserMaxCapacity
	{
		get
		{
			if (this.consumer != null)
			{
				return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
			}
			return 0f;
		}
		set
		{
			this.userMaxCapacity = value;
			this.SetConsumerCapacity(value);
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06002D0D RID: 11533 RVA: 0x001057F7 File Offset: 0x001039F7
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06002D0E RID: 11534 RVA: 0x00105804 File Offset: 0x00103A04
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06002D0F RID: 11535 RVA: 0x0010580B File Offset: 0x00103A0B
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06002D10 RID: 11536 RVA: 0x00105818 File Offset: 0x00103A18
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06002D11 RID: 11537 RVA: 0x0010581B File Offset: 0x00103A1B
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06002D12 RID: 11538 RVA: 0x00105823 File Offset: 0x00103A23
	private Tag SourceTag
	{
		get
		{
			if (this.smi.master.consumer.conduitType != ConduitType.Gas)
			{
				return GameTags.LiquidSource;
			}
			return GameTags.GasSource;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06002D13 RID: 11539 RVA: 0x00105848 File Offset: 0x00103A48
	private Tag ElementTag
	{
		get
		{
			if (this.smi.master.consumer.conduitType != ConduitType.Gas)
			{
				return GameTags.Liquid;
			}
			return GameTags.Gas;
		}
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x00105870 File Offset: 0x00103A70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_bottler_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"pick_up"
		};
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.synchronizeAnims = true;
		base.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
		base.SetWorkTime(this.overrideAnims[0].GetData().GetAnim("pick_up").totalTime);
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x0010591C File Offset: 0x00103B1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new Bottler.Controller.Instance(this);
		this.smi.StartSM();
		base.Subscribe<Bottler>(-905833192, Bottler.OnCopySettingsDelegate);
		this.UpdateStoredItemState();
		this.SetConsumerCapacity(this.userMaxCapacity);
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x0010596C File Offset: 0x00103B6C
	protected override void OnForcedCleanUp()
	{
		if (base.worker != null)
		{
			ChoreDriver component = base.worker.GetComponent<ChoreDriver>();
			if (component != null)
			{
				component.StopChore();
			}
			else
			{
				base.worker.StopWork();
			}
		}
		if (this.workerMeter != null)
		{
			this.CleanupBottleProxyObject();
		}
		base.OnForcedCleanUp();
	}

	// Token: 0x06002D17 RID: 11543 RVA: 0x001059C3 File Offset: 0x00103BC3
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.CreateBottleProxyObject(worker);
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x001059D4 File Offset: 0x00103BD4
	private void CreateBottleProxyObject(WorkerBase worker)
	{
		if (this.workerMeter != null)
		{
			this.CleanupBottleProxyObject();
		}
		PrimaryElement firstPrimaryElement = this.smi.master.GetFirstPrimaryElement();
		if (firstPrimaryElement == null)
		{
			return;
		}
		this.workerMeter = new MeterController(worker.GetComponent<KBatchedAnimController>(), "snapto_chest", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"snapto_chest"
		});
		this.workerMeter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
		this.workerMeter.meterController.Play("empty", KAnim.PlayMode.Paused, 1f, 0f);
		Color32 colour = firstPrimaryElement.Element.substance.colour;
		colour.a = byte.MaxValue;
		this.workerMeter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("water1"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), colour);
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x00105AF0 File Offset: 0x00103CF0
	private void CleanupBottleProxyObject()
	{
		if (this.workerMeter != null && !this.workerMeter.gameObject.IsNullOrDestroyed())
		{
			this.workerMeter.Unlink();
			this.workerMeter.gameObject.DeleteObject();
		}
		else
		{
			string str = "Bottler finished work but could not clean up the proxy bottle object. workerMeter=";
			MeterController meterController = this.workerMeter;
			DebugUtil.DevLogError(str + ((meterController != null) ? meterController.ToString() : null));
			KCrashReporter.ReportDevNotification("Bottle emptier could not clean up proxy object", Environment.StackTrace, "", false, null);
		}
		this.workerMeter = null;
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x00105B72 File Offset: 0x00103D72
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.CleanupBottleProxyObject();
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x00105B81 File Offset: 0x00103D81
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		this.GetAnimController().Play("ready", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002D1C RID: 11548 RVA: 0x00105BAC File Offset: 0x00103DAC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		if (pickupableStartWorkInfo.amount > 0f)
		{
			this.storage.TransferMass(component, pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(), pickupableStartWorkInfo.amount, false, false, false);
		}
		GameObject gameObject = component.FindFirst(pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID());
		if (gameObject != null)
		{
			Pickupable component2 = gameObject.GetComponent<Pickupable>();
			component2.targetWorkable = component2;
			component2.RemoveTag(this.SourceTag);
			FetchableMonitor.Instance instance = component2.GetSMI<FetchableMonitor.Instance>();
			if (instance != null)
			{
				instance.SetForceUnfetchable(false);
			}
			pickupableStartWorkInfo.setResultCb(gameObject);
		}
		else
		{
			pickupableStartWorkInfo.setResultCb(null);
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x06002D1D RID: 11549 RVA: 0x00105C6C File Offset: 0x00103E6C
	private void OnReservationsChanged(Pickupable _ignore, bool _ignore2, Pickupable.Reservation _ignore3)
	{
		bool forceUnfetchable = false;
		using (List<GameObject>.Enumerator enumerator = this.storage.items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Pickupable>().ReservedAmount > 0f)
				{
					forceUnfetchable = true;
					break;
				}
			}
		}
		foreach (GameObject go in this.storage.items)
		{
			FetchableMonitor.Instance instance = go.GetSMI<FetchableMonitor.Instance>();
			if (instance != null)
			{
				instance.SetForceUnfetchable(forceUnfetchable);
			}
		}
	}

	// Token: 0x06002D1E RID: 11550 RVA: 0x00105D24 File Offset: 0x00103F24
	private void SetConsumerCapacity(float value)
	{
		if (this.consumer != null)
		{
			this.consumer.capacityKG = value;
			float num = this.storage.MassStored() - this.userMaxCapacity;
			if (num > 0f)
			{
				this.storage.DropSome(this.storage.FindFirstWithMass(this.smi.master.ElementTag, 0f).ElementID.CreateTag(), num, false, false, new Vector3(0.8f, 0f, 0f), true, false);
			}
		}
	}

	// Token: 0x06002D1F RID: 11551 RVA: 0x00105DB5 File Offset: 0x00103FB5
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
		}
		base.OnCleanUp();
	}

	// Token: 0x06002D20 RID: 11552 RVA: 0x00105DD8 File Offset: 0x00103FD8
	private PrimaryElement GetFirstPrimaryElement()
	{
		for (int i = 0; i < this.storage.Count; i++)
		{
			GameObject gameObject = this.storage[i];
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null))
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x00105E24 File Offset: 0x00104024
	private void UpdateStoredItemState()
	{
		this.storage.allowItemRemoval = (this.smi != null && this.smi.GetCurrentState() == this.smi.sm.operational.ready);
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				gameObject.Trigger(-778359855, this.storage);
			}
		}
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x00105EC8 File Offset: 0x001040C8
	private void OnCopySettings(object data)
	{
		Bottler component = ((GameObject)data).GetComponent<Bottler>();
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x04001AD1 RID: 6865
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001AD2 RID: 6866
	public Storage storage;

	// Token: 0x04001AD3 RID: 6867
	public ConduitConsumer consumer;

	// Token: 0x04001AD4 RID: 6868
	public CellOffset workCellOffset = new CellOffset(0, 0);

	// Token: 0x04001AD5 RID: 6869
	[Serialize]
	public float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001AD6 RID: 6870
	private Bottler.Controller.Instance smi;

	// Token: 0x04001AD7 RID: 6871
	private int storageHandle;

	// Token: 0x04001AD8 RID: 6872
	private MeterController workerMeter;

	// Token: 0x04001AD9 RID: 6873
	private static readonly EventSystem.IntraObjectHandler<Bottler> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Bottler>(delegate(Bottler component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020015DA RID: 5594
	private class Controller : GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler>
	{
		// Token: 0x060094D9 RID: 38105 RVA: 0x0037A488 File Offset: 0x00378688
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.nonoperational;
			this.root.Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = false;
			});
			this.nonoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.EnterTransition(this.operational.ready, new StateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Transition.ConditionCallback(Bottler.Controller.IsFull)).DefaultState(this.operational.empty).TagTransition(GameTags.Operational, this.nonoperational, true);
			this.operational.empty.PlayAnim("off").EventHandlerTransition(GameHashes.OnStorageChange, this.operational.filling, (Bottler.Controller.Instance smi, object o) => Bottler.Controller.IsFull(smi));
			this.operational.filling.PlayAnim("working").Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.UpdateMeter();
			}).OnAnimQueueComplete(this.operational.ready);
			this.operational.ready.EventTransition(GameHashes.OnStorageChange, this.operational.empty, GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Not(new StateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Transition.ConditionCallback(Bottler.Controller.IsFull))).PlayAnim("ready").Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = true;
			}).Exit(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = false;
			}).Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = true;
				smi.UpdateMeter();
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					Pickupable component = gameObject.GetComponent<Pickupable>();
					component.targetWorkable = smi.master;
					component.SetOffsets(new CellOffset[]
					{
						smi.master.workCellOffset
					});
					Pickupable pickupable = component;
					pickupable.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Combine(pickupable.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(smi.master.OnReservationsChanged));
					component.KPrefabID.AddTag(smi.master.SourceTag, false);
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			}).Exit(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = false;
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					Pickupable component = gameObject.GetComponent<Pickupable>();
					component.targetWorkable = component;
					component.SetOffsetTable(OffsetGroups.InvertedStandardTable);
					component.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Remove(component.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(smi.master.OnReservationsChanged));
					component.KPrefabID.RemoveTag(smi.master.SourceTag);
					FetchableMonitor.Instance smi2 = component.GetSMI<FetchableMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.SetForceUnfetchable(false);
					}
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			});
		}

		// Token: 0x060094DA RID: 38106 RVA: 0x0037A68E File Offset: 0x0037888E
		public static bool IsFull(Bottler.Controller.Instance smi)
		{
			return smi.master.storage.MassStored() >= smi.master.userMaxCapacity && smi.master.userMaxCapacity > 0f;
		}

		// Token: 0x040072E3 RID: 29411
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State nonoperational;

		// Token: 0x040072E4 RID: 29412
		public Bottler.Controller.OperationalStates operational;

		// Token: 0x020028CA RID: 10442
		public class OperationalStates : GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State
		{
			// Token: 0x0400B3BF RID: 46015
			public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State empty;

			// Token: 0x0400B3C0 RID: 46016
			public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State filling;

			// Token: 0x0400B3C1 RID: 46017
			public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State ready;
		}

		// Token: 0x020028CB RID: 10443
		public new class Instance : GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.GameInstance
		{
			// Token: 0x17000D6A RID: 3434
			// (get) Token: 0x0600CD6F RID: 52591 RVA: 0x004316FB File Offset: 0x0042F8FB
			// (set) Token: 0x0600CD70 RID: 52592 RVA: 0x00431703 File Offset: 0x0042F903
			public MeterController meter { get; private set; }

			// Token: 0x0600CD71 RID: 52593 RVA: 0x0043170C File Offset: 0x0042F90C
			public Instance(Bottler master) : base(master)
			{
				this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "bottle", "off", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, new string[]
				{
					"bottle",
					"substance_tinter",
					"substance_tinter_cap"
				});
			}

			// Token: 0x0600CD72 RID: 52594 RVA: 0x0043175C File Offset: 0x0042F95C
			public void UpdateMeter()
			{
				PrimaryElement firstPrimaryElement = base.smi.master.GetFirstPrimaryElement();
				if (firstPrimaryElement == null)
				{
					return;
				}
				this.meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
				this.meter.meterController.Play(OreSizeVisualizerComponents.GetAnimForMass(firstPrimaryElement.Mass), KAnim.PlayMode.Paused, 1f, 0f);
				Color32 colour = firstPrimaryElement.Element.substance.colour;
				colour.a = byte.MaxValue;
				this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("water1"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), colour);
			}
		}
	}
}
