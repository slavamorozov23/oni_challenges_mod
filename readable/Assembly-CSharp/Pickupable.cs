using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200061E RID: 1566
[AddComponentMenu("KMonoBehaviour/Workable/Pickupable")]
public class Pickupable : Workable, IHasSortOrder
{
	// Token: 0x17000192 RID: 402
	// (get) Token: 0x060024F7 RID: 9463 RVA: 0x000D4315 File Offset: 0x000D2515
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x060024F8 RID: 9464 RVA: 0x000D431D File Offset: 0x000D251D
	// (set) Token: 0x060024F9 RID: 9465 RVA: 0x000D4325 File Offset: 0x000D2525
	public int sortOrder
	{
		get
		{
			return this._sortOrder;
		}
		set
		{
			this._sortOrder = value;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x060024FA RID: 9466 RVA: 0x000D432E File Offset: 0x000D252E
	// (set) Token: 0x060024FB RID: 9467 RVA: 0x000D4336 File Offset: 0x000D2536
	public Storage storage { get; set; }

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060024FC RID: 9468 RVA: 0x000D433F File Offset: 0x000D253F
	// (set) Token: 0x060024FD RID: 9469 RVA: 0x000D4347 File Offset: 0x000D2547
	public bool MinTakeAmount { get; set; }

	// Token: 0x060024FE RID: 9470 RVA: 0x000D4350 File Offset: 0x000D2550
	public bool isChoreAllowedToPickup(ChoreType choreType)
	{
		return this.allowedChoreTypes == null || this.allowedChoreTypes.Contains(choreType);
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x060024FF RID: 9471 RVA: 0x000D4368 File Offset: 0x000D2568
	// (set) Token: 0x06002500 RID: 9472 RVA: 0x000D4370 File Offset: 0x000D2570
	public bool prevent_absorb_until_stored { get; set; }

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06002501 RID: 9473 RVA: 0x000D4379 File Offset: 0x000D2579
	// (set) Token: 0x06002502 RID: 9474 RVA: 0x000D4381 File Offset: 0x000D2581
	public bool isKinematic { get; set; }

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06002503 RID: 9475 RVA: 0x000D438A File Offset: 0x000D258A
	// (set) Token: 0x06002504 RID: 9476 RVA: 0x000D4392 File Offset: 0x000D2592
	public bool wasAbsorbed { get; private set; }

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06002505 RID: 9477 RVA: 0x000D439B File Offset: 0x000D259B
	// (set) Token: 0x06002506 RID: 9478 RVA: 0x000D43A3 File Offset: 0x000D25A3
	public int cachedCell { get; private set; }

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06002507 RID: 9479 RVA: 0x000D43AC File Offset: 0x000D25AC
	// (set) Token: 0x06002508 RID: 9480 RVA: 0x000D43B4 File Offset: 0x000D25B4
	public bool IsEntombed
	{
		get
		{
			return this.isEntombed;
		}
		set
		{
			if (value != this.isEntombed)
			{
				this.isEntombed = value;
				if (this.isEntombed)
				{
					this.KPrefabID.AddTag(GameTags.Entombed, false);
				}
				else
				{
					this.KPrefabID.RemoveTag(GameTags.Entombed);
				}
				base.Trigger(-1089732772, BoxedBools.Box(this.isEntombed));
				this.UpdateEntombedVisualizer();
			}
		}
	}

	// Token: 0x06002509 RID: 9481 RVA: 0x000D4418 File Offset: 0x000D2618
	[Obsolete("Use Instance ID")]
	private bool CouldBePickedUpCommon(GameObject carrier)
	{
		return this.CouldBePickedUpCommon(carrier.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x000D442B File Offset: 0x000D262B
	private bool CouldBePickedUpCommon(int carrierID)
	{
		return this.UnreservedFetchAmount > 0f || this.FindReservedAmount(carrierID) > 0f;
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x000D444A File Offset: 0x000D264A
	[Obsolete("Use Instance ID")]
	public bool CouldBePickedUpByMinion(GameObject carrier)
	{
		return this.CouldBePickedUpByMinion(carrier.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x0600250C RID: 9484 RVA: 0x000D4460 File Offset: 0x000D2660
	public bool CouldBePickedUpByMinion(int carrierID)
	{
		return this.CouldBePickedUpCommon(carrierID) && (this.storage == null || !this.storage.automatable || !this.storage.automatable.GetAutomationOnly());
	}

	// Token: 0x0600250D RID: 9485 RVA: 0x000D44AD File Offset: 0x000D26AD
	[Obsolete("Use Instance ID")]
	public bool CouldBePickedUpByTransferArm(GameObject carrier)
	{
		return this.CouldBePickedUpByTransferArm(carrier.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x0600250E RID: 9486 RVA: 0x000D44C0 File Offset: 0x000D26C0
	public bool CouldBePickedUpByTransferArm(int carrierID)
	{
		return this.CouldBePickedUpCommon(carrierID) && (this.fetchable_monitor == null || this.fetchable_monitor.IsFetchable());
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x000D44E2 File Offset: 0x000D26E2
	[Obsolete("Use Instance ID")]
	public float FindReservedAmount(GameObject reserver)
	{
		return this.FindReservedAmount(reserver.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000D44F8 File Offset: 0x000D26F8
	public float FindReservedAmount(int reserverID)
	{
		for (int i = 0; i < this.reservations.Count; i++)
		{
			if (this.reservations[i].reserverID == reserverID)
			{
				return this.reservations[i].amount;
			}
		}
		return 0f;
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06002511 RID: 9489 RVA: 0x000D4546 File Offset: 0x000D2746
	public float UnreservedAmount
	{
		get
		{
			return this.TotalAmount - this.ReservedAmount;
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06002512 RID: 9490 RVA: 0x000D4555 File Offset: 0x000D2755
	// (set) Token: 0x06002513 RID: 9491 RVA: 0x000D455D File Offset: 0x000D275D
	public float ReservedAmount { get; private set; }

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06002514 RID: 9492 RVA: 0x000D4566 File Offset: 0x000D2766
	public float FetchTotalAmount
	{
		get
		{
			return this.primaryElement.MassPerUnit * this.primaryElement.Units;
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06002515 RID: 9493 RVA: 0x000D457F File Offset: 0x000D277F
	public float UnreservedFetchAmount
	{
		get
		{
			return this.FetchTotalAmount - this.ReservedAmount;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06002516 RID: 9494 RVA: 0x000D458E File Offset: 0x000D278E
	// (set) Token: 0x06002517 RID: 9495 RVA: 0x000D459C File Offset: 0x000D279C
	public float TotalAmount
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			DebugUtil.Assert(this.primaryElement != null);
			this.primaryElement.Units = value;
			if (value < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.primaryElement.KeepZeroMassObject)
			{
				base.gameObject.DeleteObject();
			}
			this.NotifyChanged(Grid.PosToCell(this));
		}
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x000D45F4 File Offset: 0x000D27F4
	private void RefreshReservedAmount()
	{
		this.ReservedAmount = 0f;
		for (int i = 0; i < this.reservations.Count; i++)
		{
			this.ReservedAmount += this.reservations[i].amount;
		}
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x000D4640 File Offset: 0x000D2840
	[Conditional("UNITY_EDITOR")]
	private void Log(string evt, string param, float value)
	{
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x000D4642 File Offset: 0x000D2842
	public void ClearReservations()
	{
		this.reservations.Clear();
		this.RefreshReservedAmount();
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x000D4658 File Offset: 0x000D2858
	[ContextMenu("Print Reservations")]
	public void PrintReservations()
	{
		foreach (Pickupable.Reservation reservation in this.reservations)
		{
			global::Debug.Log(reservation.ToString());
		}
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x000D46B8 File Offset: 0x000D28B8
	public int Reserve(string context, int reserverID, float amount)
	{
		int num = this.nextTicketNumber;
		this.nextTicketNumber = num + 1;
		int num2 = num;
		Pickupable.Reservation reservation = new Pickupable.Reservation(reserverID, amount, num2);
		this.reservations.Add(reservation);
		this.RefreshReservedAmount();
		if (this.OnReservationsChanged != null)
		{
			this.OnReservationsChanged(this, true, reservation);
		}
		return num2;
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x000D470C File Offset: 0x000D290C
	public void Unreserve(string context, int ticket)
	{
		int i = 0;
		while (i < this.reservations.Count)
		{
			if (this.reservations[i].ticket == ticket)
			{
				Pickupable.Reservation arg = this.reservations[i];
				this.reservations.RemoveAt(i);
				this.RefreshReservedAmount();
				if (this.OnReservationsChanged != null)
				{
					this.OnReservationsChanged(this, false, arg);
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x000D477C File Offset: 0x000D297C
	private Pickupable()
	{
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x0600251F RID: 9503 RVA: 0x000D47FC File Offset: 0x000D29FC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.OnSolidChangedClosure = new Action<object>(this.OnSolidChanged);
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.log = new LoggerFSSF("Pickupable");
		this.workerStatusItem = Db.Get().DuplicantStatusItems.PickingUp;
		base.SetWorkTime(1.5f);
		this.targetWorkable = this;
		this.resetProgressOnStop = true;
		base.gameObject.layer = Game.PickupableLayer;
		Vector3 position = base.transform.GetPosition();
		this.UpdateCachedCell(Grid.PosToCell(position));
		base.Subscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate);
		base.Subscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate);
		base.Subscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate);
		base.Subscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate);
		base.Subscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate);
		base.Subscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset);
		this.KPrefabID.AddTag(GameTags.Pickupable, false);
		Components.Pickupables.Add(this);
	}

	// Token: 0x06002520 RID: 9504 RVA: 0x000D4917 File Offset: 0x000D2B17
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06002521 RID: 9505 RVA: 0x000D4920 File Offset: 0x000D2B20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num) && this.deleteOffGrid)
		{
			base.gameObject.DeleteObject();
			return;
		}
		if (base.GetComponent<Health>() != null)
		{
			this.handleFallerComponents = false;
		}
		this.UpdateCachedCell(num);
		new ReachabilityMonitor.Instance(this).StartSM();
		this.fetchable_monitor = new FetchableMonitor.Instance(this);
		this.fetchable_monitor.StartSM();
		base.SetWorkTime(1.5f);
		this.faceTargetWhenWorking = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusIndicatorOffset(new Vector3(0f, -0.65f, 0f));
		}
		this.OnTagsChanged(null);
		this.TryToOffsetIfBuried(CellOffset.none);
		DecorProvider component2 = base.GetComponent<DecorProvider>();
		if (component2 != null && string.IsNullOrEmpty(component2.overrideName))
		{
			component2.overrideName = UI.OVERLAYS.DECOR.CLUTTER;
		}
		this.UpdateEntombedVisualizer();
		base.Subscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate);
		this.NotifyChanged(num);
	}

	// Token: 0x06002522 RID: 9506 RVA: 0x000D4A30 File Offset: 0x000D2C30
	[OnDeserialized]
	public void OnDeserialize()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 28) && base.transform.position.z == 0f)
		{
			KBatchedAnimController component = base.transform.GetComponent<KBatchedAnimController>();
			component.SetSceneLayer(component.sceneLayer);
		}
	}

	// Token: 0x06002523 RID: 9507 RVA: 0x000D4A84 File Offset: 0x000D2C84
	public void UpdateListeners(bool worldSpace)
	{
		if (this.cleaningUp)
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (worldSpace)
		{
			if (this.solidPartitionerEntry.IsValid())
			{
				return;
			}
			GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
			this.objectLayerListItem = new ObjectLayerListItem(base.gameObject, this, ObjectLayer.Pickupables, num);
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterSolidListener", base.gameObject, num, GameScenePartitioner.Instance.solidChangedLayer, this.OnSolidChangedClosure);
			this.worldPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterPickupable", this, num, GameScenePartitioner.Instance.pickupablesLayer, null);
			this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, Pickupable.OnCellChangeDispatcher, this, "Pickupable.OnCellChange");
			Singleton<CellChangeMonitor>.Instance.MarkDirty(base.transform);
			Singleton<CellChangeMonitor>.Instance.ClearLastKnownCell(base.transform);
			return;
		}
		else
		{
			if (this.storedPartitionerEntry.IsValid())
			{
				return;
			}
			this.storedPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterStoredPickupable", this, num, GameScenePartitioner.Instance.storedPickupablesLayer, null);
			if (this.objectLayerListItem != null)
			{
				this.objectLayerListItem.Clear();
				this.objectLayerListItem = null;
			}
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
			return;
		}
	}

	// Token: 0x06002524 RID: 9508 RVA: 0x000D4BE3 File Offset: 0x000D2DE3
	public void RegisterListeners()
	{
		this.UpdateListeners(true);
	}

	// Token: 0x06002525 RID: 9509 RVA: 0x000D4BEC File Offset: 0x000D2DEC
	public void UnregisterListeners()
	{
		if (this.objectLayerListItem != null)
		{
			this.objectLayerListItem.Clear();
			this.objectLayerListItem = null;
		}
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
		base.Unsubscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate, false);
		base.Unsubscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate, false);
		base.Unsubscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate, false);
		base.Unsubscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate, false);
		base.Unsubscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate, false);
		base.Unsubscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset, false);
		if (base.isSpawned)
		{
			base.Unsubscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate, false);
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x000D4CD2 File Offset: 0x000D2ED2
	private void OnSolidChanged(object data)
	{
		this.TryToOffsetIfBuried(CellOffset.none);
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x000D4CE0 File Offset: 0x000D2EE0
	private void SetWorkableOffset(object data)
	{
		CellOffset offset = CellOffset.none;
		WorkerBase workerBase = data as WorkerBase;
		if (workerBase != null)
		{
			int num = Grid.PosToCell(workerBase);
			int base_cell = Grid.PosToCell(this);
			offset = (Grid.IsValidCell(num) ? Grid.GetCellOffsetDirection(base_cell, num) : CellOffset.none);
		}
		this.TryToOffsetIfBuried(offset);
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x000D4D30 File Offset: 0x000D2F30
	private CellOffset[] GetPreferedOffsets(CellOffset preferedDirectionOffset)
	{
		if (preferedDirectionOffset == CellOffset.left || preferedDirectionOffset == CellOffset.leftup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.left,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.right || preferedDirectionOffset == CellOffset.rightup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.right,
				CellOffset.rightup
			};
		}
		if (preferedDirectionOffset == CellOffset.up)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.rightup,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.leftdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.left
			};
		}
		if (preferedDirectionOffset == CellOffset.rightdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.rightdown,
				CellOffset.right
			};
		}
		if (preferedDirectionOffset == CellOffset.down)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.rightdown
			};
		}
		return new CellOffset[0];
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000D4EB0 File Offset: 0x000D30B0
	public void TryToOffsetIfBuried(CellOffset offset)
	{
		if (this.KPrefabID.HasTag(GameTags.Stored) || this.KPrefabID.HasTag(GameTags.Equipped))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
		if ((smi == null || smi.IsDead()) && ((Grid.Solid[num] && Grid.Foundation[num]) || Grid.Properties[num] != 0))
		{
			CellOffset[] array = this.GetPreferedOffsets(offset).Concat(Pickupable.displacementOffsets);
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = Grid.OffsetCell(num, array[i]);
				if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					Vector3 position = Grid.CellToPosCBC(num2, Grid.SceneLayer.Move);
					KCollider2D component = base.GetComponent<KCollider2D>();
					if (component != null)
					{
						position.y += base.transform.GetPosition().y - component.bounds.min.y;
					}
					base.transform.SetPosition(position);
					num = num2;
					this.RemoveFaller();
					this.AddFaller(Vector2.zero);
					break;
				}
			}
		}
		this.HandleSolidCell(num);
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x000D5004 File Offset: 0x000D3204
	private bool HandleSolidCell(int cell)
	{
		bool flag = this.IsEntombed;
		bool flag2 = false;
		if (Grid.IsValidCell(cell) && Grid.Solid[cell])
		{
			DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
			if (smi == null || smi.IsDead())
			{
				this.Clearable.CancelClearing();
				flag2 = true;
			}
		}
		if (flag2 != flag && !this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.IsEntombed = flag2;
			base.GetComponent<KSelectable>().IsSelectable = !this.IsEntombed;
		}
		this.UpdateEntombedVisualizer();
		return this.IsEntombed;
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x000D5094 File Offset: 0x000D3294
	private void OnCellChange()
	{
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		if (!Grid.IsValidCell(num))
		{
			Vector2 vector = new Vector2(-0.1f * (float)Grid.WidthInCells, 1.1f * (float)Grid.WidthInCells);
			Vector2 vector2 = new Vector2(-0.1f * (float)Grid.HeightInCells, 1.1f * (float)Grid.HeightInCells);
			if (this.deleteOffGrid && (position.x < vector.x || vector.y < position.x || position.y < vector2.x || vector2.y < position.y))
			{
				this.DeleteObject();
				return;
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
			if (this.HandleSolidCell(num))
			{
				return;
			}
			this.objectLayerListItem.Update(num);
			bool flag = false;
			if (this.absorbable && !this.KPrefabID.HasTag(GameTags.Stored))
			{
				int num2 = Grid.CellBelow(num);
				if (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					ObjectLayerListItem nextItem = this.objectLayerListItem.nextItem;
					while (nextItem != null)
					{
						Pickupable pickupable = nextItem.pickupable;
						nextItem = nextItem.nextItem;
						if (pickupable != null)
						{
							flag = pickupable.TryAbsorb(this, false, false);
							if (flag)
							{
								break;
							}
						}
					}
				}
			}
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, num);
			GameScenePartitioner.Instance.UpdatePosition(this.worldPartitionerEntry, num);
			int cachedCell = this.cachedCell;
			this.UpdateCachedCell(num);
			if (!flag)
			{
				this.NotifyChanged(num);
			}
			if (Grid.IsValidCell(cachedCell) && num != cachedCell)
			{
				this.NotifyChanged(cachedCell);
			}
		}
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x000D5238 File Offset: 0x000D3438
	private void OnTagsChanged(object _)
	{
		if (!this.KPrefabID.HasTag(GameTags.Stored) && !this.KPrefabID.HasTag(GameTags.Equipped))
		{
			this.UpdateListeners(true);
			this.AddFaller(Vector2.zero);
			return;
		}
		this.UpdateListeners(false);
		this.RemoveFaller();
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x000D5289 File Offset: 0x000D3489
	private void NotifyChanged(int new_cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(new_cell, GameScenePartitioner.Instance.pickupablesChangedLayer, this);
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x000D52A4 File Offset: 0x000D34A4
	public bool TryAbsorb(Pickupable other, bool hide_effects, bool allow_cross_storage = false)
	{
		if (other == null)
		{
			return false;
		}
		if (other.wasAbsorbed)
		{
			return false;
		}
		if (this.wasAbsorbed)
		{
			return false;
		}
		if (!other.CanAbsorb(this))
		{
			return false;
		}
		if (this.prevent_absorb_until_stored)
		{
			return false;
		}
		if (!allow_cross_storage && this.storage == null != (other.storage == null))
		{
			return false;
		}
		this.Absorb(other);
		if (!hide_effects && EffectPrefabs.Instance != null && !this.storage)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
			global::Util.KInstantiate(Assets.GetPrefab(EffectConfigs.OreAbsorbId), position, Quaternion.identity, null, null, true, 0).SetActive(true);
		}
		return true;
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x000D536C File Offset: 0x000D356C
	protected override void OnCleanUp()
	{
		this.cleaningUp = true;
		this.ReleaseEntombedVisualizerAndAddFaller(false);
		this.RemoveFaller();
		if (this.storage)
		{
			this.storage.Remove(base.gameObject, true);
		}
		this.UnregisterListeners();
		this.fetchable_monitor = null;
		Components.Pickupables.Remove(this);
		if (this.reservations.Count > 0)
		{
			Pickupable.Reservation[] array = this.reservations.ToArray();
			this.reservations.Clear();
			if (this.OnReservationsChanged != null)
			{
				foreach (Pickupable.Reservation arg in array)
				{
					this.OnReservationsChanged(this, false, arg);
				}
			}
		}
		if (Grid.IsValidCell(this.cachedCell))
		{
			this.NotifyChanged(this.cachedCell);
		}
		base.OnCleanUp();
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x000D5436 File Offset: 0x000D3636
	public Pickupable TakeUnit(float units)
	{
		return this.Take(units * this.primaryElement.MassPerUnit);
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x000D544C File Offset: 0x000D364C
	public Pickupable Take(float amount)
	{
		if (amount <= 0f)
		{
			return null;
		}
		if (this.OnTake == null)
		{
			if (this.storage != null)
			{
				this.storage.Remove(base.gameObject, true);
			}
			return this;
		}
		float num = this.TotalAmount * this.primaryElement.MassPerUnit;
		if (amount >= num && this.storage != null && !this.primaryElement.KeepZeroMassObject)
		{
			this.storage.Remove(base.gameObject, true);
		}
		float num2 = Math.Min(num, amount) / this.primaryElement.MassPerUnit;
		if (num2 <= 0f)
		{
			return null;
		}
		return this.OnTake(this, num2);
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x000D54FC File Offset: 0x000D36FC
	private void Absorb(Pickupable pickupable)
	{
		global::Debug.Assert(!this.wasAbsorbed);
		global::Debug.Assert(!pickupable.wasAbsorbed);
		base.Trigger(-2064133523, pickupable);
		pickupable.Trigger(-1940207677, base.gameObject);
		pickupable.wasAbsorbed = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == pickupable.GetComponent<KSelectable>())
		{
			SelectTool.Instance.Select(component, false);
		}
		pickupable.gameObject.DeleteObject();
		this.NotifyChanged(Grid.PosToCell(this));
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x000D55AC File Offset: 0x000D37AC
	private void RefreshStorageTags(object data = null)
	{
		bool flag = data is Storage || (data != null && ((Boxed<bool>)data).value);
		if (flag && data is Storage && ((Storage)data).gameObject == base.gameObject)
		{
			return;
		}
		if (!flag)
		{
			this.KPrefabID.RemoveTag(GameTags.Stored);
			this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
			return;
		}
		this.KPrefabID.AddTag(GameTags.Stored, false);
		if (this.storage == null || !this.storage.allowItemRemoval)
		{
			this.KPrefabID.AddTag(GameTags.StoredPrivate, false);
			return;
		}
		this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x000D566C File Offset: 0x000D386C
	public void OnStore(object data)
	{
		this.storage = (data as Storage);
		bool flag = data is Storage || (data != null && ((Boxed<bool>)data).value);
		SaveLoadRoot component = base.GetComponent<SaveLoadRoot>();
		if (this.carryAnimOverride != null && this.lastCarrier != null)
		{
			this.lastCarrier.RemoveAnimOverrides(this.carryAnimOverride);
			this.lastCarrier = null;
		}
		KSelectable component2 = base.GetComponent<KSelectable>();
		if (component2)
		{
			component2.IsSelectable = !flag;
		}
		if (flag)
		{
			int cachedCell = this.cachedCell;
			this.RefreshStorageTags(data);
			this.RemoveFaller();
			if (this.storage != null)
			{
				if (this.carryAnimOverride != null && this.storage.GetComponent<Navigator>() != null)
				{
					this.lastCarrier = this.storage.GetComponent<KBatchedAnimController>();
					if (this.lastCarrier != null && this.lastCarrier.HasTag(GameTags.BaseMinion))
					{
						this.lastCarrier.AddAnimOverrides(this.carryAnimOverride, 0f);
					}
				}
				this.UpdateCachedCell(Grid.PosToCell(this.storage));
			}
			this.NotifyChanged(cachedCell);
			if (component != null)
			{
				component.SetRegistered(false);
				return;
			}
		}
		else
		{
			if (component != null)
			{
				component.SetRegistered(true);
			}
			this.RemovedFromStorage();
		}
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x000D57C0 File Offset: 0x000D39C0
	private void RemovedFromStorage()
	{
		this.storage = null;
		this.UpdateCachedCell(Grid.PosToCell(this));
		this.RefreshStorageTags(null);
		this.AddFaller(Vector2.zero);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.enabled = true;
		base.gameObject.transform.rotation = Quaternion.identity;
		this.UpdateListeners(true);
		component.GetBatchInstanceData().ClearOverrideTransformMatrix();
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x000D5825 File Offset: 0x000D3A25
	public void UpdateCachedCellFromStoragePosition()
	{
		global::Debug.Assert(this.storage != null, "Only call UpdateCachedCellFromStoragePosition on pickupables in storage!");
		this.UpdateCachedCell(Grid.PosToCell(this.storage));
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x000D5850 File Offset: 0x000D3A50
	public void UpdateCachedCell(int cell)
	{
		if (this.cachedCell != cell && this.storedPartitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.UpdatePosition(this.storedPartitionerEntry, cell);
		}
		this.cachedCell = cell;
		this.GetOffsets(this.cachedCell);
		if (this.KPrefabID.HasTag(GameTags.PickupableStorage))
		{
			base.GetComponent<Storage>().UpdateStoredItemCachedCells();
		}
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000D58B5 File Offset: 0x000D3AB5
	public override int GetCell()
	{
		return this.cachedCell;
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000D58C0 File Offset: 0x000D3AC0
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		if (this.useGunforPickup && worker.UsesMultiTool())
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "pickup", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000D5918 File Offset: 0x000D3B18
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		float amount = pickupableStartWorkInfo.amount;
		if (!(this != null))
		{
			pickupableStartWorkInfo.setResultCb(null);
			return;
		}
		Pickupable pickupable = this.Take(amount);
		if (pickupable != null)
		{
			component.Store(pickupable.gameObject, false, false, true, false);
			worker.SetWorkCompleteData(pickupable);
			pickupableStartWorkInfo.setResultCb(pickupable.gameObject);
			return;
		}
		pickupableStartWorkInfo.setResultCb(null);
	}

	// Token: 0x0600253B RID: 9531 RVA: 0x000D59A3 File Offset: 0x000D3BA3
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x0600253C RID: 9532 RVA: 0x000D59A6 File Offset: 0x000D3BA6
	public override Vector3 GetTargetPoint()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x000D59B3 File Offset: 0x000D3BB3
	public bool IsReachable()
	{
		return this.isReachable;
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x000D59BC File Offset: 0x000D3BBC
	private void OnReachableChanged(object data)
	{
		this.isReachable = ((Boxed<bool>)data).value;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.isReachable)
		{
			component.RemoveStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, this);
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x000D5A18 File Offset: 0x000D3C18
	private void AddFaller(Vector2 initial_velocity)
	{
		if (!this.handleFallerComponents)
		{
			return;
		}
		if (!GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Add(base.gameObject, initial_velocity);
		}
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x000D5A47 File Offset: 0x000D3C47
	private void RemoveFaller()
	{
		if (!this.handleFallerComponents)
		{
			return;
		}
		if (GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Remove(base.gameObject);
		}
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x000D5A74 File Offset: 0x000D3C74
	private void OnOreSizeChanged(object data)
	{
		Vector3 v = Vector3.zero;
		HandleVector<int>.Handle handle = GameComps.Gravities.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			v = GameComps.Gravities.GetData(handle).velocity;
		}
		this.RemoveFaller();
		if (!this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.AddFaller(v);
		}
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x000D5ADC File Offset: 0x000D3CDC
	private void OnLanded(object data)
	{
		if (CameraController.Instance == null)
		{
			return;
		}
		Vector3 position = base.transform.GetPosition();
		Vector2I vector2I = Grid.PosToXY(position);
		if (vector2I.x < 0 || Grid.WidthInCells <= vector2I.x || vector2I.y < 0 || Grid.HeightInCells <= vector2I.y)
		{
			this.DeleteObject();
			return;
		}
		Vector2 value = ((Boxed<Vector2>)data).value;
		if (value.sqrMagnitude <= 0.2f || SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		Element element = this.primaryElement.Element;
		if (element.substance != null)
		{
			string text = element.substance.GetOreBumpSound();
			if (text == null)
			{
				if (element.HasTag(GameTags.RefinedMetal))
				{
					text = "RefinedMetal";
				}
				else if (element.HasTag(GameTags.Metal))
				{
					text = "RawMetal";
				}
				else
				{
					text = "Rock";
				}
			}
			if (element.tag.ToString() == "Creature" && !base.gameObject.HasTag(GameTags.Seed))
			{
				text = "Bodyfall_rock";
			}
			else
			{
				text = "Ore_bump_" + text;
			}
			string text2 = GlobalAssets.GetSound(text, true);
			text2 = ((text2 != null) ? text2 : GlobalAssets.GetSound("Ore_bump_rock", false));
			if (CameraController.Instance.IsAudibleSound(base.transform.GetPosition(), text2))
			{
				int num = Grid.PosToCell(position);
				bool isLiquid = Grid.Element[num].IsLiquid;
				float value2 = 0f;
				if (isLiquid)
				{
					value2 = SoundUtil.GetLiquidDepth(num);
				}
				FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(text2, CameraController.Instance.GetVerticallyScaledPosition(base.transform.GetPosition(), false), 1f);
				instance.setParameterByName("velocity", value.magnitude, false);
				instance.setParameterByName("liquidDepth", value2, false);
				KFMOD.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x000D5CBC File Offset: 0x000D3EBC
	private void UpdateEntombedVisualizer()
	{
		if (this.IsEntombed)
		{
			if (this.entombedCell == -1)
			{
				int cell = Grid.PosToCell(this);
				if (EntombedItemManager.CanEntomb(this))
				{
					SaveGame.Instance.entombedItemManager.Add(this);
				}
				if (Grid.Objects[cell, 1] == null)
				{
					KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
					if (component != null && Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(cell))
					{
						this.entombedCell = cell;
						component.enabled = false;
						this.RemoveFaller();
						return;
					}
				}
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
		}
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x000D5D4C File Offset: 0x000D3F4C
	private void ReleaseEntombedVisualizerAndAddFaller(bool add_faller_if_necessary)
	{
		if (this.entombedCell != -1)
		{
			Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.entombedCell);
			this.entombedCell = -1;
			base.GetComponent<KBatchedAnimController>().enabled = true;
			if (add_faller_if_necessary)
			{
				this.AddFaller(Vector2.zero);
			}
		}
	}

	// Token: 0x0400159A RID: 5530
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400159B RID: 5531
	public const float WorkTime = 1.5f;

	// Token: 0x0400159C RID: 5532
	[SerializeField]
	private int _sortOrder;

	// Token: 0x0400159F RID: 5535
	[MyCmpReq]
	[NonSerialized]
	public KPrefabID KPrefabID;

	// Token: 0x040015A0 RID: 5536
	[MyCmpAdd]
	[NonSerialized]
	public Clearable Clearable;

	// Token: 0x040015A1 RID: 5537
	[MyCmpAdd]
	[NonSerialized]
	public Prioritizable prioritizable;

	// Token: 0x040015A2 RID: 5538
	[SerializeField]
	public List<ChoreType> allowedChoreTypes;

	// Token: 0x040015A3 RID: 5539
	public bool absorbable;

	// Token: 0x040015A5 RID: 5541
	public Func<Pickupable, bool> CanAbsorb = (Pickupable other) => false;

	// Token: 0x040015A6 RID: 5542
	public Func<Pickupable, float, Pickupable> OnTake;

	// Token: 0x040015A7 RID: 5543
	public Action<Pickupable, bool, Pickupable.Reservation> OnReservationsChanged;

	// Token: 0x040015A8 RID: 5544
	public ObjectLayerListItem objectLayerListItem;

	// Token: 0x040015A9 RID: 5545
	public Workable targetWorkable;

	// Token: 0x040015AA RID: 5546
	public KAnimFile carryAnimOverride;

	// Token: 0x040015AB RID: 5547
	private KBatchedAnimController lastCarrier;

	// Token: 0x040015AC RID: 5548
	public bool useGunforPickup = true;

	// Token: 0x040015AE RID: 5550
	public static CellOffset[] displacementOffsets = new CellOffset[]
	{
		new CellOffset(0, 1),
		new CellOffset(0, -1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(-1, -1)
	};

	// Token: 0x040015AF RID: 5551
	private bool isReachable;

	// Token: 0x040015B0 RID: 5552
	private bool isEntombed;

	// Token: 0x040015B1 RID: 5553
	private bool cleaningUp;

	// Token: 0x040015B3 RID: 5555
	public bool trackOnPickup = true;

	// Token: 0x040015B5 RID: 5557
	private int nextTicketNumber;

	// Token: 0x040015B6 RID: 5558
	private ulong cellChangedHandlerID;

	// Token: 0x040015B7 RID: 5559
	[Serialize]
	public bool deleteOffGrid = true;

	// Token: 0x040015B8 RID: 5560
	private List<Pickupable.Reservation> reservations = new List<Pickupable.Reservation>();

	// Token: 0x040015B9 RID: 5561
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x040015BA RID: 5562
	private HandleVector<int>.Handle worldPartitionerEntry;

	// Token: 0x040015BB RID: 5563
	private HandleVector<int>.Handle storedPartitionerEntry;

	// Token: 0x040015BC RID: 5564
	private FetchableMonitor.Instance fetchable_monitor;

	// Token: 0x040015BD RID: 5565
	public bool handleFallerComponents = true;

	// Token: 0x040015BE RID: 5566
	private LoggerFSSF log;

	// Token: 0x040015C0 RID: 5568
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x040015C1 RID: 5569
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnLandedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnLanded(data);
	});

	// Token: 0x040015C2 RID: 5570
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnOreSizeChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnOreSizeChanged(data);
	});

	// Token: 0x040015C3 RID: 5571
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x040015C4 RID: 5572
	private static readonly EventSystem.IntraObjectHandler<Pickupable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.RefreshStorageTags(data);
	});

	// Token: 0x040015C5 RID: 5573
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnWorkableEntombOffset = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.SetWorkableOffset(data);
	});

	// Token: 0x040015C6 RID: 5574
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x040015C7 RID: 5575
	private Action<object> OnSolidChangedClosure;

	// Token: 0x040015C8 RID: 5576
	private static Action<object> OnCellChangeDispatcher = delegate(object obj)
	{
		Unsafe.As<Pickupable>(obj).OnCellChange();
	};

	// Token: 0x040015C9 RID: 5577
	private int entombedCell = -1;

	// Token: 0x020014F1 RID: 5361
	public struct Reservation
	{
		// Token: 0x06009190 RID: 37264 RVA: 0x00371859 File Offset: 0x0036FA59
		public Reservation(int reserverID, float amount, int ticket)
		{
			this.reserverID = reserverID;
			this.amount = amount;
			this.ticket = ticket;
		}

		// Token: 0x06009191 RID: 37265 RVA: 0x00371870 File Offset: 0x0036FA70
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.reserverID.ToString(),
				", ",
				this.amount.ToString(),
				", ",
				this.ticket.ToString()
			});
		}

		// Token: 0x04007008 RID: 28680
		public int reserverID;

		// Token: 0x04007009 RID: 28681
		public float amount;

		// Token: 0x0400700A RID: 28682
		public int ticket;
	}

	// Token: 0x020014F2 RID: 5362
	public class PickupableStartWorkInfo : WorkerBase.StartWorkInfo
	{
		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06009192 RID: 37266 RVA: 0x003718C2 File Offset: 0x0036FAC2
		// (set) Token: 0x06009193 RID: 37267 RVA: 0x003718CA File Offset: 0x0036FACA
		public float amount { get; private set; }

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06009194 RID: 37268 RVA: 0x003718D3 File Offset: 0x0036FAD3
		// (set) Token: 0x06009195 RID: 37269 RVA: 0x003718DB File Offset: 0x0036FADB
		public Pickupable originalPickupable { get; private set; }

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06009196 RID: 37270 RVA: 0x003718E4 File Offset: 0x0036FAE4
		// (set) Token: 0x06009197 RID: 37271 RVA: 0x003718EC File Offset: 0x0036FAEC
		public Action<GameObject> setResultCb { get; private set; }

		// Token: 0x06009198 RID: 37272 RVA: 0x003718F5 File Offset: 0x0036FAF5
		public PickupableStartWorkInfo(Pickupable pickupable, float amount, Action<GameObject> set_result_cb) : base(pickupable.targetWorkable)
		{
			this.originalPickupable = pickupable;
			this.amount = amount;
			this.setResultCb = set_result_cb;
		}
	}
}
