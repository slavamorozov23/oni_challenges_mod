using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200080B RID: 2059
public class SuitLocker : StateMachineComponent<SuitLocker.StatesInstance>
{
	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x0600377A RID: 14202 RVA: 0x00137B18 File Offset: 0x00135D18
	public float OxygenAvailable
	{
		get
		{
			KPrefabID storedOutfit = this.GetStoredOutfit();
			if (storedOutfit == null)
			{
				return 0f;
			}
			return storedOutfit.GetComponent<SuitTank>().PercentFull();
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x0600377B RID: 14203 RVA: 0x00137B48 File Offset: 0x00135D48
	public float BatteryAvailable
	{
		get
		{
			KPrefabID storedOutfit = this.GetStoredOutfit();
			if (storedOutfit == null)
			{
				return 0f;
			}
			return storedOutfit.GetComponent<LeadSuitTank>().batteryCharge;
		}
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x00137B78 File Offset: 0x00135D78
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		DebugUtil.DevAssert(this.OutfitTags.Length == 1, "Suit Locker " + base.name + " requesting more than one suit type, this will break the fetch chore", null);
		if (this.OutfitTags.Length == 1)
		{
			GameObject prefab = Assets.GetPrefab(this.OutfitTags[0]);
			if (prefab != null)
			{
				PrimaryElement component = prefab.GetComponent<PrimaryElement>();
				this.OutfitMass = component.MassPerUnit;
			}
		}
		else
		{
			this.OutfitMass = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_MASS;
		}
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x00137C68 File Offset: 0x00135E68
	public KPrefabID GetStoredOutfit()
	{
		foreach (GameObject gameObject in base.GetComponent<Storage>().items)
		{
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!(component == null) && component.IsAnyPrefabID(this.OutfitTags))
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x00137CE8 File Offset: 0x00135EE8
	public float GetSuitScore()
	{
		float num = -1f;
		KPrefabID partiallyChargedOutfit = this.GetPartiallyChargedOutfit();
		if (partiallyChargedOutfit)
		{
			num = partiallyChargedOutfit.GetComponent<SuitTank>().PercentFull();
			JetSuitTank component = partiallyChargedOutfit.GetComponent<JetSuitTank>();
			if (component && component.PercentFull() < num)
			{
				num = component.PercentFull();
			}
		}
		return num;
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x00137D38 File Offset: 0x00135F38
	public KPrefabID GetPartiallyChargedOutfit()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (!storedOutfit)
		{
			return null;
		}
		if (storedOutfit.GetComponent<SuitTank>().PercentFull() < TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
		{
			return null;
		}
		JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
		if (component && component.PercentFull() < TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
		{
			return null;
		}
		return storedOutfit;
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x00137D8C File Offset: 0x00135F8C
	public KPrefabID GetFullyChargedOutfit()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (!storedOutfit)
		{
			return null;
		}
		if (!storedOutfit.GetComponent<SuitTank>().IsFull())
		{
			return null;
		}
		JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
		if (component && !component.IsFull())
		{
			return null;
		}
		return storedOutfit;
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x00137DD4 File Offset: 0x00135FD4
	private void CreateFetchChore()
	{
		this.fetchChore = new FetchChore(Db.Get().ChoreTypes.EquipmentFetch, base.GetComponent<Storage>(), this.OutfitMass, new HashSet<Tag>(this.OutfitTags), FetchChore.MatchCriteria.MatchID, Tag.Invalid, new Tag[]
		{
			GameTags.Assigned
		}, null, true, null, null, null, Operational.State.None, 0);
		this.fetchChore.allowMultifetch = false;
	}

	// Token: 0x06003782 RID: 14210 RVA: 0x00137E3D File Offset: 0x0013603D
	private void CancelFetchChore()
	{
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("SuitLocker.CancelFetchChore");
			this.fetchChore = null;
		}
	}

	// Token: 0x06003783 RID: 14211 RVA: 0x00137E60 File Offset: 0x00136060
	public bool HasOxygen()
	{
		GameObject oxygen = this.GetOxygen();
		return oxygen != null && oxygen.GetComponent<PrimaryElement>().Mass > 0f;
	}

	// Token: 0x06003784 RID: 14212 RVA: 0x00137E94 File Offset: 0x00136094
	private void RefreshMeter()
	{
		GameObject oxygen = this.GetOxygen();
		float num = 0f;
		if (oxygen != null)
		{
			num = oxygen.GetComponent<PrimaryElement>().Mass / base.GetComponent<ConduitConsumer>().capacityKG;
			num = Math.Min(num, 1f);
		}
		this.meter.SetPositionPercent(num);
	}

	// Token: 0x06003785 RID: 14213 RVA: 0x00137EE8 File Offset: 0x001360E8
	public bool IsSuitFullyCharged()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (!(storedOutfit != null))
		{
			return false;
		}
		SuitTank component = storedOutfit.GetComponent<SuitTank>();
		if (component != null && component.PercentFull() < 1f)
		{
			return false;
		}
		JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
		if (component2 != null && component2.PercentFull() < 1f)
		{
			return false;
		}
		LeadSuitTank leadSuitTank = (storedOutfit != null) ? storedOutfit.GetComponent<LeadSuitTank>() : null;
		return !(leadSuitTank != null) || leadSuitTank.PercentFull() >= 1f;
	}

	// Token: 0x06003786 RID: 14214 RVA: 0x00137F74 File Offset: 0x00136174
	public bool IsOxygenTankFull()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= 1f;
		}
		return false;
	}

	// Token: 0x06003787 RID: 14215 RVA: 0x00137FB5 File Offset: 0x001361B5
	private void OnRequestOutfit()
	{
		base.smi.sm.isWaitingForSuit.Set(true, base.smi, false);
	}

	// Token: 0x06003788 RID: 14216 RVA: 0x00137FD5 File Offset: 0x001361D5
	private void OnCancelRequest()
	{
		base.smi.sm.isWaitingForSuit.Set(false, base.smi, false);
	}

	// Token: 0x06003789 RID: 14217 RVA: 0x00137FF8 File Offset: 0x001361F8
	public void DropSuit()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		base.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x0013802C File Offset: 0x0013622C
	public void EquipTo(Equipment equipment)
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		base.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
		Prioritizable component = storedOutfit.GetComponent<Prioritizable>();
		PrioritySetting masterPriority = component.GetMasterPriority();
		PrioritySetting masterPriority2 = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
		if (component != null && component.GetMasterPriority().priority_class == PriorityScreen.PriorityClass.topPriority)
		{
			component.SetMasterPriority(masterPriority2);
		}
		storedOutfit.GetComponent<Equippable>().Assign(equipment.GetComponent<IAssignableIdentity>());
		storedOutfit.GetComponent<EquippableWorkable>().CancelChore("Manual equip");
		if (component != null && component.GetMasterPriority() != masterPriority)
		{
			component.SetMasterPriority(masterPriority);
		}
		equipment.Equip(storedOutfit.GetComponent<Equippable>());
		this.returnSuitWorkable.CreateChore();
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x001380E8 File Offset: 0x001362E8
	public void UnequipFrom(Equipment equipment)
	{
		Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
		assignable.Unassign();
		Durability component = assignable.GetComponent<Durability>();
		if (component != null && component.IsWornOut())
		{
			this.ConfigRequestSuit();
			return;
		}
		base.GetComponent<Storage>().Store(assignable.gameObject, false, false, true, false);
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x00138146 File Offset: 0x00136346
	public void ConfigRequestSuit()
	{
		base.smi.sm.isConfigured.Set(true, base.smi, false);
		base.smi.sm.isWaitingForSuit.Set(true, base.smi, false);
	}

	// Token: 0x0600378D RID: 14221 RVA: 0x00138184 File Offset: 0x00136384
	public void ConfigNoSuit()
	{
		base.smi.sm.isConfigured.Set(true, base.smi, false);
		base.smi.sm.isWaitingForSuit.Set(false, base.smi, false);
	}

	// Token: 0x0600378E RID: 14222 RVA: 0x001381C4 File Offset: 0x001363C4
	public bool CanDropOffSuit()
	{
		return base.smi.sm.isConfigured.Get(base.smi) && !base.smi.sm.isWaitingForSuit.Get(base.smi) && this.GetStoredOutfit() == null;
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x00138219 File Offset: 0x00136419
	private GameObject GetOxygen()
	{
		return base.GetComponent<Storage>().FindFirst(GameTags.Oxygen);
	}

	// Token: 0x06003790 RID: 14224 RVA: 0x0013822C File Offset: 0x0013642C
	private void ChargeSuit(float dt)
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		GameObject oxygen = this.GetOxygen();
		if (oxygen == null)
		{
			return;
		}
		SuitTank component = storedOutfit.GetComponent<SuitTank>();
		float num = component.capacity * 15f * dt / 600f;
		num = Mathf.Min(num, component.capacity - component.GetTankAmount());
		num = Mathf.Min(oxygen.GetComponent<PrimaryElement>().Mass, num);
		if (num > 0f)
		{
			base.GetComponent<Storage>().Transfer(component.storage, component.elementTag, num, false, true);
		}
	}

	// Token: 0x06003791 RID: 14225 RVA: 0x001382C0 File Offset: 0x001364C0
	public void SetSuitMarker(SuitMarker suit_marker)
	{
		SuitLocker.SuitMarkerState suitMarkerState = SuitLocker.SuitMarkerState.HasMarker;
		if (suit_marker == null)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.NoMarker;
		}
		else if (suit_marker.transform.GetPosition().x > base.transform.GetPosition().x && suit_marker.GetComponent<Rotatable>().IsRotated)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
		}
		else if (suit_marker.transform.GetPosition().x < base.transform.GetPosition().x && !suit_marker.GetComponent<Rotatable>().IsRotated)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
		}
		else if (!suit_marker.GetComponent<Operational>().IsOperational)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.NotOperational;
		}
		if (suitMarkerState != this.suitMarkerState)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker, false);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide, false);
			switch (suitMarkerState)
			{
			case SuitLocker.SuitMarkerState.NoMarker:
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker, null);
				break;
			case SuitLocker.SuitMarkerState.WrongSide:
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide, null);
				break;
			}
			this.suitMarkerState = suitMarkerState;
		}
	}

	// Token: 0x06003792 RID: 14226 RVA: 0x001383EA File Offset: 0x001365EA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), null);
	}

	// Token: 0x06003793 RID: 14227 RVA: 0x00138408 File Offset: 0x00136608
	private static void GatherSuitBuildings(int cell, int dir, List<SuitLocker.SuitLockerEntry> suit_lockers, List<SuitLocker.SuitMarkerEntry> suit_markers)
	{
		int num = dir;
		for (;;)
		{
			int cell2 = Grid.OffsetCell(cell, num, 0);
			if (Grid.IsValidCell(cell2) && !SuitLocker.GatherSuitBuildingsOnCell(cell2, suit_lockers, suit_markers))
			{
				break;
			}
			num += dir;
		}
	}

	// Token: 0x06003794 RID: 14228 RVA: 0x00138438 File Offset: 0x00136638
	private static bool GatherSuitBuildingsOnCell(int cell, List<SuitLocker.SuitLockerEntry> suit_lockers, List<SuitLocker.SuitMarkerEntry> suit_markers)
	{
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject == null)
		{
			return false;
		}
		SuitMarker component = gameObject.GetComponent<SuitMarker>();
		if (component != null)
		{
			suit_markers.Add(new SuitLocker.SuitMarkerEntry
			{
				suitMarker = component,
				cell = cell
			});
			return true;
		}
		SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
		if (component2 != null)
		{
			suit_lockers.Add(new SuitLocker.SuitLockerEntry
			{
				suitLocker = component2,
				cell = cell
			});
			return true;
		}
		return false;
	}

	// Token: 0x06003795 RID: 14229 RVA: 0x001384C4 File Offset: 0x001366C4
	private static SuitMarker FindSuitMarker(int cell, List<SuitLocker.SuitMarkerEntry> suit_markers)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		foreach (SuitLocker.SuitMarkerEntry suitMarkerEntry in suit_markers)
		{
			if (suitMarkerEntry.cell == cell)
			{
				return suitMarkerEntry.suitMarker;
			}
		}
		return null;
	}

	// Token: 0x06003796 RID: 14230 RVA: 0x0013852C File Offset: 0x0013672C
	public static void UpdateSuitMarkerStates(int cell, GameObject self)
	{
		ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
		ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.PooledList pooledList2 = ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.Allocate();
		if (self != null)
		{
			SuitLocker component = self.GetComponent<SuitLocker>();
			if (component != null)
			{
				pooledList.Add(new SuitLocker.SuitLockerEntry
				{
					suitLocker = component,
					cell = cell
				});
			}
			SuitMarker component2 = self.GetComponent<SuitMarker>();
			if (component2 != null)
			{
				pooledList2.Add(new SuitLocker.SuitMarkerEntry
				{
					suitMarker = component2,
					cell = cell
				});
			}
		}
		SuitLocker.GatherSuitBuildings(cell, 1, pooledList, pooledList2);
		SuitLocker.GatherSuitBuildings(cell, -1, pooledList, pooledList2);
		pooledList.Sort(SuitLocker.SuitLockerEntry.comparer);
		for (int i = 0; i < pooledList.Count; i++)
		{
			SuitLocker.SuitLockerEntry suitLockerEntry = pooledList[i];
			SuitLocker.SuitLockerEntry suitLockerEntry2 = suitLockerEntry;
			ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList3 = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
			pooledList3.Add(suitLockerEntry);
			for (int j = i + 1; j < pooledList.Count; j++)
			{
				SuitLocker.SuitLockerEntry suitLockerEntry3 = pooledList[j];
				if (Grid.CellRight(suitLockerEntry2.cell) != suitLockerEntry3.cell)
				{
					break;
				}
				i++;
				suitLockerEntry2 = suitLockerEntry3;
				pooledList3.Add(suitLockerEntry3);
			}
			int cell2 = Grid.CellLeft(suitLockerEntry.cell);
			int cell3 = Grid.CellRight(suitLockerEntry2.cell);
			SuitMarker suitMarker = SuitLocker.FindSuitMarker(cell2, pooledList2);
			if (suitMarker == null)
			{
				suitMarker = SuitLocker.FindSuitMarker(cell3, pooledList2);
			}
			foreach (SuitLocker.SuitLockerEntry suitLockerEntry4 in pooledList3)
			{
				suitLockerEntry4.suitLocker.SetSuitMarker(suitMarker);
			}
			pooledList3.Recycle();
		}
		pooledList.Recycle();
		pooledList2.Recycle();
	}

	// Token: 0x040021D0 RID: 8656
	[MyCmpGet]
	private Building building;

	// Token: 0x040021D1 RID: 8657
	public Tag[] OutfitTags;

	// Token: 0x040021D2 RID: 8658
	private float OutfitMass;

	// Token: 0x040021D3 RID: 8659
	private FetchChore fetchChore;

	// Token: 0x040021D4 RID: 8660
	[MyCmpAdd]
	public SuitLocker.ReturnSuitWorkable returnSuitWorkable;

	// Token: 0x040021D5 RID: 8661
	private MeterController meter;

	// Token: 0x040021D6 RID: 8662
	private SuitLocker.SuitMarkerState suitMarkerState;

	// Token: 0x02001797 RID: 6039
	[AddComponentMenu("KMonoBehaviour/Workable/ReturnSuitWorkable")]
	public class ReturnSuitWorkable : Workable
	{
		// Token: 0x06009BB1 RID: 39857 RVA: 0x00396480 File Offset: 0x00394680
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.workTime = 0.25f;
			this.synchronizeAnims = false;
		}

		// Token: 0x06009BB2 RID: 39858 RVA: 0x003964A4 File Offset: 0x003946A4
		public void CreateChore()
		{
			if (this.urgentChore == null)
			{
				SuitLocker component = base.GetComponent<SuitLocker>();
				this.urgentChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitUrgent, this, null, true, null, null, null, true, null, false, false, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
				this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingUrgent, null);
				this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.HasSuitMarker, component);
				this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.SuitTypeMatchesLocker, component);
				this.idleChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitIdle, this, null, true, null, null, null, true, null, false, false, null, false, true, false, PriorityScreen.PriorityClass.idle, 5, false, false);
				this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingIdle, null);
				this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.HasSuitMarker, component);
				this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.SuitTypeMatchesLocker, component);
			}
		}

		// Token: 0x06009BB3 RID: 39859 RVA: 0x00396581 File Offset: 0x00394781
		public void CancelChore()
		{
			if (this.urgentChore != null)
			{
				this.urgentChore.Cancel("ReturnSuitWorkable.CancelChore");
				this.urgentChore = null;
			}
			if (this.idleChore != null)
			{
				this.idleChore.Cancel("ReturnSuitWorkable.CancelChore");
				this.idleChore = null;
			}
		}

		// Token: 0x06009BB4 RID: 39860 RVA: 0x003965C1 File Offset: 0x003947C1
		protected override void OnStartWork(WorkerBase worker)
		{
			base.ShowProgressBar(false);
		}

		// Token: 0x06009BB5 RID: 39861 RVA: 0x003965CA File Offset: 0x003947CA
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			return true;
		}

		// Token: 0x06009BB6 RID: 39862 RVA: 0x003965D0 File Offset: 0x003947D0
		protected override void OnCompleteWork(WorkerBase worker)
		{
			Equipment equipment = worker.GetComponent<MinionIdentity>().GetEquipment();
			if (equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit))
			{
				if (base.GetComponent<SuitLocker>().CanDropOffSuit())
				{
					base.GetComponent<SuitLocker>().UnequipFrom(equipment);
				}
				else
				{
					equipment.GetAssignable(Db.Get().AssignableSlots.Suit).Unassign();
				}
			}
			if (this.urgentChore != null)
			{
				this.CancelChore();
				this.CreateChore();
			}
		}

		// Token: 0x06009BB7 RID: 39863 RVA: 0x00396649 File Offset: 0x00394849
		public override HashedString[] GetWorkAnims(WorkerBase worker)
		{
			return new HashedString[]
			{
				new HashedString("none")
			};
		}

		// Token: 0x04007821 RID: 30753
		public static readonly Chore.Precondition DoesSuitNeedRechargingUrgent = new Chore.Precondition
		{
			id = "DoesSuitNeedRechargingUrgent",
			description = DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_URGENT,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Equipment equipment = context.consumerState.equipment;
				if (equipment == null)
				{
					return false;
				}
				AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
				if (slot.assignable == null)
				{
					return false;
				}
				Equippable component = slot.assignable.GetComponent<Equippable>();
				if (component == null || !component.isEquipped)
				{
					return false;
				}
				SuitTank component2 = slot.assignable.GetComponent<SuitTank>();
				if (component2 != null && component2.NeedsRecharging())
				{
					return true;
				}
				JetSuitTank component3 = slot.assignable.GetComponent<JetSuitTank>();
				if (component3 != null && component3.NeedsRecharging())
				{
					return true;
				}
				LeadSuitTank component4 = slot.assignable.GetComponent<LeadSuitTank>();
				return component4 != null && component4.NeedsRecharging();
			}
		};

		// Token: 0x04007822 RID: 30754
		public static readonly Chore.Precondition DoesSuitNeedRechargingIdle = new Chore.Precondition
		{
			id = "DoesSuitNeedRechargingIdle",
			description = DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_IDLE,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Equipment equipment = context.consumerState.equipment;
				if (equipment == null)
				{
					return false;
				}
				AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
				if (slot.assignable == null)
				{
					return false;
				}
				Equippable component = slot.assignable.GetComponent<Equippable>();
				return !(component == null) && component.isEquipped && (slot.assignable.GetComponent<SuitTank>() != null || slot.assignable.GetComponent<JetSuitTank>() != null || slot.assignable.GetComponent<LeadSuitTank>() != null);
			}
		};

		// Token: 0x04007823 RID: 30755
		public static Chore.Precondition HasSuitMarker = new Chore.Precondition
		{
			id = "IsValid",
			description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return ((SuitLocker)data).suitMarkerState == SuitLocker.SuitMarkerState.HasMarker;
			}
		};

		// Token: 0x04007824 RID: 30756
		public static Chore.Precondition SuitTypeMatchesLocker = new Chore.Precondition
		{
			id = "IsValid",
			description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				SuitLocker suitLocker = (SuitLocker)data;
				Equipment equipment = context.consumerState.equipment;
				if (equipment == null)
				{
					return false;
				}
				AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
				return !(slot.assignable == null) && slot.assignable.GetComponent<KPrefabID>().IsAnyPrefabID(suitLocker.OutfitTags);
			}
		};

		// Token: 0x04007825 RID: 30757
		private WorkChore<SuitLocker.ReturnSuitWorkable> urgentChore;

		// Token: 0x04007826 RID: 30758
		private WorkChore<SuitLocker.ReturnSuitWorkable> idleChore;
	}

	// Token: 0x02001798 RID: 6040
	public class StatesInstance : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.GameInstance
	{
		// Token: 0x06009BBA RID: 39866 RVA: 0x00396781 File Offset: 0x00394981
		public StatesInstance(SuitLocker suit_locker) : base(suit_locker)
		{
		}
	}

	// Token: 0x02001799 RID: 6041
	public class States : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker>
	{
		// Token: 0x06009BBB RID: 39867 RVA: 0x0039678C File Offset: 0x0039498C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(SuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.DefaultState(this.empty.notconfigured).EventTransition(GameHashes.OnStorageChange, this.charging, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null).ParamTransition<bool>(this.isWaitingForSuit, this.waitingforsuit, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue).Enter("CreateReturnSuitChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.returnSuitWorkable.CreateChore();
			}).RefreshUserMenuOnEnter().Exit("CancelReturnSuitChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.returnSuitWorkable.CancelChore();
			}).PlayAnim("no_suit_pre").QueueAnim("no_suit", false, null);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state = this.empty.notconfigured.ParamTransition<bool>(this.isConfigured, this.empty.configured, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue);
			string name = BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.NAME;
			string tooltip = BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.TOOLTIP;
			string icon = "status_item_no_filter_set";
			StatusItem.IconType icon_type = StatusItem.IconType.Custom;
			NotificationType notification_type = NotificationType.BadMinor;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state2 = this.empty.configured.RefreshUserMenuOnEnter();
			string name2 = BUILDING.STATUSITEMS.SUIT_LOCKER.READY.NAME;
			string tooltip2 = BUILDING.STATUSITEMS.SUIT_LOCKER.READY.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			main = Db.Get().StatusItemCategories.Main;
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state3 = this.waitingforsuit.EventTransition(GameHashes.OnStorageChange, this.charging, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null).Enter("CreateFetchChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.CreateFetchChore();
			}).ParamTransition<bool>(this.isWaitingForSuit, this.empty, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsFalse).RefreshUserMenuOnEnter().PlayAnim("no_suit_pst").QueueAnim("awaiting_suit", false, null).Exit("ClearIsWaitingForSuit", delegate(SuitLocker.StatesInstance smi)
			{
				this.isWaitingForSuit.Set(false, smi, false);
			}).Exit("CancelFetchChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.CancelFetchChore();
			});
			string name3 = BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.NAME;
			string tooltip3 = BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.TOOLTIP;
			string icon3 = "";
			StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
			NotificationType notification_type3 = NotificationType.Neutral;
			bool allow_multiples3 = false;
			main = Db.Get().StatusItemCategories.Main;
			state3.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main);
			this.charging.DefaultState(this.charging.pre).RefreshUserMenuOnEnter().EventTransition(GameHashes.OnStorageChange, this.empty, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).ToggleStatusItem(Db.Get().MiscStatusItems.StoredItemDurability, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit().gameObject).Enter(delegate(SuitLocker.StatesInstance smi)
			{
				KAnim.Build.Symbol symbol = smi.master.GetStoredOutfit().GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol("suit");
				SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
				component.TryRemoveSymbolOverride("suit_swap", 0);
				if (symbol != null)
				{
					component.AddSymbolOverride("suit_swap", symbol, 0);
				}
			});
			this.charging.pre.Enter(delegate(SuitLocker.StatesInstance smi)
			{
				if (smi.master.IsSuitFullyCharged())
				{
					smi.GoTo(this.suitfullycharged);
					return;
				}
				smi.GetComponent<KBatchedAnimController>().Play("no_suit_pst", KAnim.PlayMode.Once, 1f, 0f);
				smi.GetComponent<KBatchedAnimController>().Queue("charging_pre", KAnim.PlayMode.Once, 1f, 0f);
			}).OnAnimQueueComplete(this.charging.operational);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state4 = this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nooxygen, (SuitLocker.StatesInstance smi) => !smi.master.HasOxygen(), UpdateRate.SIM_200ms).PlayAnim("charging_loop", KAnim.PlayMode.Loop).Enter("SetActive", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(true, false);
			}).Transition(this.charging.pst, (SuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms).Update("ChargeSuit", delegate(SuitLocker.StatesInstance smi, float dt)
			{
				smi.master.ChargeSuit(dt);
			}, UpdateRate.SIM_200ms, false).Exit("ClearActive", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(false, false);
			});
			string name4 = BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.NAME;
			string tooltip4 = BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.TOOLTIP;
			string icon4 = "";
			StatusItem.IconType icon_type4 = StatusItem.IconType.Info;
			NotificationType notification_type4 = NotificationType.Neutral;
			bool allow_multiples4 = false;
			main = Db.Get().StatusItemCategories.Main;
			state4.ToggleStatusItem(name4, tooltip4, icon4, icon_type4, notification_type4, allow_multiples4, default(HashedString), 129022, null, null, main);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state5 = this.charging.nooxygen.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (SuitLocker.StatesInstance smi) => smi.master.HasOxygen(), UpdateRate.SIM_200ms).Transition(this.charging.pst, (SuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms).PlayAnim("no_o2_loop", KAnim.PlayMode.Loop);
			string name5 = BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.NAME;
			string tooltip5 = BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.TOOLTIP;
			string icon5 = "status_item_suit_locker_no_oxygen";
			StatusItem.IconType icon_type5 = StatusItem.IconType.Custom;
			NotificationType notification_type5 = NotificationType.BadMinor;
			bool allow_multiples5 = false;
			main = Db.Get().StatusItemCategories.Main;
			state5.ToggleStatusItem(name5, tooltip5, icon5, icon_type5, notification_type5, allow_multiples5, default(HashedString), 129022, null, null, main);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state6 = this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false).PlayAnim("not_charging_loop", KAnim.PlayMode.Loop).Transition(this.charging.pst, (SuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			string name6 = BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.NAME;
			string tooltip6 = BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.TOOLTIP;
			string icon6 = "";
			StatusItem.IconType icon_type6 = StatusItem.IconType.Info;
			NotificationType notification_type6 = NotificationType.Neutral;
			bool allow_multiples6 = false;
			main = Db.Get().StatusItemCategories.Main;
			state6.ToggleStatusItem(name6, tooltip6, icon6, icon_type6, notification_type6, allow_multiples6, default(HashedString), 129022, null, null, main);
			this.charging.pst.PlayAnim("charging_pst").OnAnimQueueComplete(this.suitfullycharged);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state7 = this.suitfullycharged.EventTransition(GameHashes.OnStorageChange, this.empty, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).PlayAnim("has_suit").RefreshUserMenuOnEnter().ToggleStatusItem(Db.Get().MiscStatusItems.StoredItemDurability, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit().gameObject);
			string name7 = BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.NAME;
			string tooltip7 = BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.TOOLTIP;
			string icon7 = "";
			StatusItem.IconType icon_type7 = StatusItem.IconType.Info;
			NotificationType notification_type7 = NotificationType.Neutral;
			bool allow_multiples7 = false;
			main = Db.Get().StatusItemCategories.Main;
			state7.ToggleStatusItem(name7, tooltip7, icon7, icon_type7, notification_type7, allow_multiples7, default(HashedString), 129022, null, null, main);
		}

		// Token: 0x04007827 RID: 30759
		public SuitLocker.States.EmptyStates empty;

		// Token: 0x04007828 RID: 30760
		public SuitLocker.States.ChargingStates charging;

		// Token: 0x04007829 RID: 30761
		public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State waitingforsuit;

		// Token: 0x0400782A RID: 30762
		public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State suitfullycharged;

		// Token: 0x0400782B RID: 30763
		public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isWaitingForSuit;

		// Token: 0x0400782C RID: 30764
		public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isConfigured;

		// Token: 0x0400782D RID: 30765
		public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter hasSuitMarker;

		// Token: 0x0200294D RID: 10573
		public class ChargingStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
		{
			// Token: 0x0400B6B9 RID: 46777
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pre;

			// Token: 0x0400B6BA RID: 46778
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pst;

			// Token: 0x0400B6BB RID: 46779
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State operational;

			// Token: 0x0400B6BC RID: 46780
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State nooxygen;

			// Token: 0x0400B6BD RID: 46781
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notoperational;
		}

		// Token: 0x0200294E RID: 10574
		public class EmptyStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
		{
			// Token: 0x0400B6BE RID: 46782
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State configured;

			// Token: 0x0400B6BF RID: 46783
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notconfigured;
		}
	}

	// Token: 0x0200179A RID: 6042
	private enum SuitMarkerState
	{
		// Token: 0x0400782F RID: 30767
		HasMarker,
		// Token: 0x04007830 RID: 30768
		NoMarker,
		// Token: 0x04007831 RID: 30769
		WrongSide,
		// Token: 0x04007832 RID: 30770
		NotOperational
	}

	// Token: 0x0200179B RID: 6043
	private struct SuitLockerEntry
	{
		// Token: 0x04007833 RID: 30771
		public SuitLocker suitLocker;

		// Token: 0x04007834 RID: 30772
		public int cell;

		// Token: 0x04007835 RID: 30773
		public static SuitLocker.SuitLockerEntry.Comparer comparer = new SuitLocker.SuitLockerEntry.Comparer();

		// Token: 0x02002950 RID: 10576
		public class Comparer : IComparer<SuitLocker.SuitLockerEntry>
		{
			// Token: 0x0600D06B RID: 53355 RVA: 0x00435424 File Offset: 0x00433624
			public int Compare(SuitLocker.SuitLockerEntry a, SuitLocker.SuitLockerEntry b)
			{
				return a.cell - b.cell;
			}
		}
	}

	// Token: 0x0200179C RID: 6044
	private struct SuitMarkerEntry
	{
		// Token: 0x04007836 RID: 30774
		public SuitMarker suitMarker;

		// Token: 0x04007837 RID: 30775
		public int cell;
	}
}
