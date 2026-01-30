using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200080C RID: 2060
[AddComponentMenu("KMonoBehaviour/scripts/SuitMarker")]
public class SuitMarker : KMonoBehaviour
{
	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06003798 RID: 14232 RVA: 0x001386E8 File Offset: 0x001368E8
	// (set) Token: 0x06003799 RID: 14233 RVA: 0x00138708 File Offset: 0x00136908
	private bool OnlyTraverseIfUnequipAvailable
	{
		get
		{
			DebugUtil.Assert(this.onlyTraverseIfUnequipAvailable == (this.gridFlags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) > (Grid.SuitMarker.Flags)0);
			return this.onlyTraverseIfUnequipAvailable;
		}
		set
		{
			this.onlyTraverseIfUnequipAvailable = value;
			this.UpdateGridFlag(Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable, this.onlyTraverseIfUnequipAvailable);
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x0600379A RID: 14234 RVA: 0x0013871E File Offset: 0x0013691E
	// (set) Token: 0x0600379B RID: 14235 RVA: 0x0013872B File Offset: 0x0013692B
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

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x0600379C RID: 14236 RVA: 0x00138735 File Offset: 0x00136935
	// (set) Token: 0x0600379D RID: 14237 RVA: 0x00138742 File Offset: 0x00136942
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

	// Token: 0x0600379E RID: 14238 RVA: 0x0013874C File Offset: 0x0013694C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnlyTraverseIfUnequipAvailable = this.onlyTraverseIfUnequipAvailable;
		global::Debug.Assert(this.interactAnim != null, "interactAnim is null");
		base.Subscribe<SuitMarker>(493375141, SuitMarker.OnRefreshUserMenuDelegate);
		this.isOperational = base.GetComponent<Operational>().IsOperational;
		base.Subscribe<SuitMarker>(-592767678, SuitMarker.OnOperationalChangedDelegate);
		this.isRotated = base.GetComponent<Rotatable>().IsRotated;
		base.Subscribe<SuitMarker>(-1643076535, SuitMarker.OnRotatedDelegate);
		this.CreateNewEquipReactable();
		this.CreateNewUnequipReactable();
		this.cell = Grid.PosToCell(this);
		Grid.RegisterSuitMarker(this.cell);
		base.GetComponent<KAnimControllerBase>().Play("no_suit", KAnim.PlayMode.Once, 1f, 0f);
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
		this.RefreshTraverseIfUnequipStatusItem();
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x00138848 File Offset: 0x00136A48
	private void CreateNewEquipReactable()
	{
		this.equipReactable = new SuitMarker.EquipSuitReactable(this);
	}

	// Token: 0x060037A0 RID: 14240 RVA: 0x00138856 File Offset: 0x00136A56
	private void CreateNewUnequipReactable()
	{
		this.unequipReactable = new SuitMarker.UnequipSuitReactable(this);
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x00138864 File Offset: 0x00136A64
	public void GetAttachedLockers(List<SuitLocker> suit_lockers)
	{
		int num = this.isRotated ? 1 : -1;
		int num2 = 1;
		for (;;)
		{
			int num3 = Grid.OffsetCell(this.cell, num2 * num, 0);
			GameObject gameObject = Grid.Objects[num3, 1];
			if (gameObject == null)
			{
				break;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (!(component == null))
			{
				if (!component.IsAnyPrefabID(this.LockerTags))
				{
					break;
				}
				SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
				if (component2 == null)
				{
					break;
				}
				Operational component3 = gameObject.GetComponent<Operational>();
				if ((!(component3 != null) || component3.GetFlag(BuildingEnabledButton.EnabledFlag)) && !suit_lockers.Contains(component2))
				{
					suit_lockers.Add(component2);
				}
			}
			num2++;
		}
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x00138914 File Offset: 0x00136B14
	public static bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell, Grid.SuitMarker.Flags flags)
	{
		return Grid.CellColumn(dest_cell) > Grid.CellColumn(source_cell) == ((flags & Grid.SuitMarker.Flags.Rotated) == (Grid.SuitMarker.Flags)0);
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x0013892C File Offset: 0x00136B2C
	public bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell)
	{
		return SuitMarker.DoesTraversalDirectionRequireSuit(source_cell, dest_cell, this.gridFlags);
	}

	// Token: 0x060037A4 RID: 14244 RVA: 0x0013893C File Offset: 0x00136B3C
	private void Update()
	{
		ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
		this.GetAttachedLockers(pooledList);
		int num = 0;
		int num2 = 0;
		KPrefabID x = null;
		foreach (SuitLocker suitLocker in pooledList)
		{
			if (suitLocker.CanDropOffSuit())
			{
				num++;
			}
			if (suitLocker.GetPartiallyChargedOutfit() != null)
			{
				num2++;
			}
			if (x == null)
			{
				x = suitLocker.GetStoredOutfit();
			}
		}
		pooledList.Recycle();
		bool flag = x != null;
		if (flag != this.hasAvailableSuit)
		{
			base.GetComponent<KAnimControllerBase>().Play(flag ? "off" : "no_suit", KAnim.PlayMode.Once, 1f, 0f);
			this.hasAvailableSuit = flag;
		}
		Grid.UpdateSuitMarker(this.cell, num2, num, this.gridFlags, this.PathFlag);
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x00138A30 File Offset: 0x00136C30
	private void RefreshTraverseIfUnequipStatusItem()
	{
		if (this.OnlyTraverseIfUnequipAvailable)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, false);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, null);
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x00138AB6 File Offset: 0x00136CB6
	private void OnEnableTraverseIfUnequipAvailable()
	{
		this.OnlyTraverseIfUnequipAvailable = true;
		this.RefreshTraverseIfUnequipStatusItem();
	}

	// Token: 0x060037A7 RID: 14247 RVA: 0x00138AC5 File Offset: 0x00136CC5
	private void OnDisableTraverseIfUnequipAvailable()
	{
		this.OnlyTraverseIfUnequipAvailable = false;
		this.RefreshTraverseIfUnequipStatusItem();
	}

	// Token: 0x060037A8 RID: 14248 RVA: 0x00138AD4 File Offset: 0x00136CD4
	private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
	{
		if (state)
		{
			this.gridFlags |= flag;
			return;
		}
		this.gridFlags &= ~flag;
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x00138AF8 File Offset: 0x00136CF8
	private void OnOperationalChanged(bool isOperational)
	{
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
		this.isOperational = isOperational;
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x00138B1C File Offset: 0x00136D1C
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = (!this.OnlyTraverseIfUnequipAvailable) ? new KIconButtonMenu.ButtonInfo("action_clearance", UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.NAME, new System.Action(this.OnEnableTraverseIfUnequipAvailable), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_clearance", UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.NAME, new System.Action(this.OnDisableTraverseIfUnequipAvailable), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x00138BB8 File Offset: 0x00136DB8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			Grid.UnregisterSuitMarker(this.cell);
		}
		if (this.equipReactable != null)
		{
			this.equipReactable.Cleanup();
		}
		if (this.unequipReactable != null)
		{
			this.unequipReactable.Cleanup();
		}
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), null);
	}

	// Token: 0x040021D7 RID: 8663
	[MyCmpGet]
	private Building building;

	// Token: 0x040021D8 RID: 8664
	private SuitMarker.SuitMarkerReactable equipReactable;

	// Token: 0x040021D9 RID: 8665
	private SuitMarker.SuitMarkerReactable unequipReactable;

	// Token: 0x040021DA RID: 8666
	private bool hasAvailableSuit;

	// Token: 0x040021DB RID: 8667
	[Serialize]
	private bool onlyTraverseIfUnequipAvailable;

	// Token: 0x040021DC RID: 8668
	private Grid.SuitMarker.Flags gridFlags;

	// Token: 0x040021DD RID: 8669
	private int cell;

	// Token: 0x040021DE RID: 8670
	public Tag[] LockerTags;

	// Token: 0x040021DF RID: 8671
	public PathFinder.PotentialPath.Flags PathFlag;

	// Token: 0x040021E0 RID: 8672
	public KAnimFile interactAnim = Assets.GetAnim("anim_equip_clothing_kanim");

	// Token: 0x040021E1 RID: 8673
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040021E2 RID: 8674
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.OnOperationalChanged(((Boxed<bool>)data).value);
	});

	// Token: 0x040021E3 RID: 8675
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRotatedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.isRotated = ((Rotatable)data).IsRotated;
	});

	// Token: 0x0200179D RID: 6045
	private class EquipSuitReactable : SuitMarker.SuitMarkerReactable
	{
		// Token: 0x06009BC0 RID: 39872 RVA: 0x00396F63 File Offset: 0x00395163
		public EquipSuitReactable(SuitMarker marker) : base("EquipSuitReactable", marker)
		{
		}

		// Token: 0x06009BC1 RID: 39873 RVA: 0x00396F76 File Offset: 0x00395176
		public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			return !newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) && base.InternalCanBegin(newReactor, transition);
		}

		// Token: 0x06009BC2 RID: 39874 RVA: 0x00396FA6 File Offset: 0x003951A6
		protected override void InternalBegin()
		{
			base.InternalBegin();
			this.suitMarker.CreateNewEquipReactable();
		}

		// Token: 0x06009BC3 RID: 39875 RVA: 0x00396FBC File Offset: 0x003951BC
		protected override bool MovingTheRightWay(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.navGridTransition.x < 0;
			return this.IsRocketDoorExitEquip(newReactor, transition) || flag == this.suitMarker.isRotated;
		}

		// Token: 0x06009BC4 RID: 39876 RVA: 0x00396FF4 File Offset: 0x003951F4
		private bool IsRocketDoorExitEquip(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.end != NavType.Teleport && transition.start != NavType.Teleport;
			return transition.navGridTransition.x == 0 && new_reactor.GetMyWorld().IsModuleInterior && !flag;
		}

		// Token: 0x06009BC5 RID: 39877 RVA: 0x0039703C File Offset: 0x0039523C
		protected override void Run()
		{
			ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
			this.suitMarker.GetAttachedLockers(pooledList);
			SuitLocker suitLocker = null;
			for (int i = 0; i < pooledList.Count; i++)
			{
				float suitScore = pooledList[i].GetSuitScore();
				if (suitScore >= 1f)
				{
					suitLocker = pooledList[i];
					break;
				}
				if (suitLocker == null || suitScore > suitLocker.GetSuitScore())
				{
					suitLocker = pooledList[i];
				}
			}
			pooledList.Recycle();
			if (suitLocker != null)
			{
				Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
				SuitWearer.Instance smi = this.reactor.GetSMI<SuitWearer.Instance>();
				suitLocker.EquipTo(equipment);
				smi.UnreserveSuits();
				this.suitMarker.Update();
			}
		}
	}

	// Token: 0x0200179E RID: 6046
	private class UnequipSuitReactable : SuitMarker.SuitMarkerReactable
	{
		// Token: 0x06009BC6 RID: 39878 RVA: 0x003970EB File Offset: 0x003952EB
		public UnequipSuitReactable(SuitMarker marker) : base("UnequipSuitReactable", marker)
		{
		}

		// Token: 0x06009BC7 RID: 39879 RVA: 0x00397100 File Offset: 0x00395300
		public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			Navigator component = newReactor.GetComponent<Navigator>();
			return newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) && component != null && (component.flags & this.suitMarker.PathFlag) > PathFinder.PotentialPath.Flags.None && base.InternalCanBegin(newReactor, transition);
		}

		// Token: 0x06009BC8 RID: 39880 RVA: 0x00397168 File Offset: 0x00395368
		protected override void InternalBegin()
		{
			base.InternalBegin();
			this.suitMarker.CreateNewUnequipReactable();
		}

		// Token: 0x06009BC9 RID: 39881 RVA: 0x0039717C File Offset: 0x0039537C
		protected override bool MovingTheRightWay(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.navGridTransition.x < 0;
			return transition.navGridTransition.x != 0 && flag != this.suitMarker.isRotated;
		}

		// Token: 0x06009BCA RID: 39882 RVA: 0x003971B8 File Offset: 0x003953B8
		protected override void Run()
		{
			Navigator component = this.reactor.GetComponent<Navigator>();
			Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
			if (component != null && (component.flags & this.suitMarker.PathFlag) > PathFinder.PotentialPath.Flags.None)
			{
				ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
				this.suitMarker.GetAttachedLockers(pooledList);
				SuitLocker suitLocker = null;
				int num = 0;
				while (suitLocker == null && num < pooledList.Count)
				{
					if (pooledList[num].CanDropOffSuit())
					{
						suitLocker = pooledList[num];
					}
					num++;
				}
				pooledList.Recycle();
				if (suitLocker != null)
				{
					suitLocker.UnequipFrom(equipment);
					component.GetSMI<SuitWearer.Instance>().UnreserveSuits();
					this.suitMarker.Update();
					return;
				}
			}
			Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
			if (assignable != null)
			{
				assignable.Unassign();
				Notification notification = new Notification(MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP, null, true, 0f, null, null, null, true, false, false);
				assignable.GetComponent<Notifier>().Add(notification, "");
			}
		}
	}

	// Token: 0x0200179F RID: 6047
	private abstract class SuitMarkerReactable : Reactable
	{
		// Token: 0x06009BCB RID: 39883 RVA: 0x003972F8 File Offset: 0x003954F8
		public SuitMarkerReactable(HashedString id, SuitMarker suit_marker) : base(suit_marker.gameObject, id, Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.suitMarker = suit_marker;
		}

		// Token: 0x06009BCC RID: 39884 RVA: 0x00397341 File Offset: 0x00395541
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.suitMarker == null)
			{
				base.Cleanup();
				return false;
			}
			return this.suitMarker.isOperational && this.MovingTheRightWay(new_reactor, transition);
		}

		// Token: 0x06009BCD RID: 39885 RVA: 0x00397380 File Offset: 0x00395580
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(this.suitMarker.interactAnim, 1f);
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			if (this.suitMarker.HasTag(GameTags.JetSuitBlocker))
			{
				KBatchedAnimController component2 = this.suitMarker.GetComponent<KBatchedAnimController>();
				component2.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
				component2.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
				component2.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06009BCE RID: 39886 RVA: 0x00397478 File Offset: 0x00395678
		public override void Update(float dt)
		{
			Facing facing = this.reactor ? this.reactor.GetComponent<Facing>() : null;
			if (facing && this.suitMarker)
			{
				facing.SetFacing(this.suitMarker.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
			}
			if (Time.time - this.startTime > 2.8f)
			{
				if (this.reactor != null && this.suitMarker != null)
				{
					this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
					this.Run();
				}
				base.Cleanup();
			}
		}

		// Token: 0x06009BCF RID: 39887 RVA: 0x00397525 File Offset: 0x00395725
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
			}
		}

		// Token: 0x06009BD0 RID: 39888 RVA: 0x00397550 File Offset: 0x00395750
		protected override void InternalCleanup()
		{
		}

		// Token: 0x06009BD1 RID: 39889
		protected abstract bool MovingTheRightWay(GameObject reactor, Navigator.ActiveTransition transition);

		// Token: 0x06009BD2 RID: 39890
		protected abstract void Run();

		// Token: 0x04007838 RID: 30776
		protected SuitMarker suitMarker;

		// Token: 0x04007839 RID: 30777
		protected float startTime;
	}
}
