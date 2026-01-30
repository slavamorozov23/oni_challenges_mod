using System;
using System.Runtime.CompilerServices;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200060B RID: 1547
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Movable")]
public class Movable : Workable
{
	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06002425 RID: 9253 RVA: 0x000D1121 File Offset: 0x000CF321
	public bool IsMarkedForMove
	{
		get
		{
			return this.isMarkedForMove;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06002426 RID: 9254 RVA: 0x000D1129 File Offset: 0x000CF329
	public Storage StorageProxy
	{
		get
		{
			if (this.storageProxy == null)
			{
				return null;
			}
			return this.storageProxy.Get();
		}
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x000D1140 File Offset: 0x000CF340
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe(493375141, Movable.OnRefreshUserMenuDispatcher, this);
		base.Subscribe(1335436905, Movable.OnSplitFromChunkDispatcher, this);
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x000D116C File Offset: 0x000CF36C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				if (this.reachableChangedHandle < 0)
				{
					this.reachableChangedHandle = base.Subscribe(-1432940121, Movable.OnReachableChangedDispatcher, this);
				}
				if (this.storageReachableChangedHandle < 0)
				{
					this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, Movable.OnReachableChangedDispatcher, this);
				}
				if (this.cancelHandle < 0)
				{
					this.cancelHandle = base.Subscribe(2127324410, Movable.CleanupMoveDispatcher, this);
				}
				if (this.tagsChangedHandle < 0)
				{
					this.tagsChangedHandle = base.Subscribe(-1582839653, Movable.OnTagsChangedDispatcher, this);
				}
				base.gameObject.AddTag(GameTags.MarkedForMove);
			}
			else
			{
				this.isMarkedForMove = false;
			}
		}
		if (Movable.IsCritterPickupable(base.gameObject))
		{
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, Workable.UpdateStatusItemDispatcher, this);
			this.shouldShowSkillPerkStatusItem = this.isMarkedForMove;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
			this.UpdateStatusItem();
		}
	}

	// Token: 0x06002429 RID: 9257 RVA: 0x000D1290 File Offset: 0x000CF490
	private void OnReachableChanged(object _)
	{
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				int num = Grid.PosToCell(this.pickupable);
				int num2 = Grid.PosToCell(this.StorageProxy);
				if (num != num2)
				{
					bool flag = MinionGroupProber.Get().IsReachable(num, OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(num2, OffsetGroups.Standard);
					if (this.pickupable.KPrefabID.HasTag(GameTags.Creatures.Confined))
					{
						flag = false;
					}
					KSelectable component = base.GetComponent<KSelectable>();
					this.pendingMoveGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MarkedForMove, this.pendingMoveGuid, flag, this);
					this.storageUnreachableGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MoveStorageUnreachable, this.storageUnreachableGuid, !flag, this);
					return;
				}
			}
			else
			{
				this.ClearMove();
			}
		}
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x000D1370 File Offset: 0x000CF570
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable != null)
		{
			Movable component = pickupable.GetComponent<Movable>();
			if (component.isMarkedForMove)
			{
				this.storageProxy = new Ref<Storage>(component.StorageProxy);
				this.MarkForMove();
			}
		}
	}

	// Token: 0x0600242B RID: 9259 RVA: 0x000D13B3 File Offset: 0x000CF5B3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isMarkedForMove && this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().RemoveMovable(this);
			this.ClearStorageProxy();
		}
	}

	// Token: 0x0600242C RID: 9260 RVA: 0x000D13E8 File Offset: 0x000CF5E8
	private void CleanupMove(object _)
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x0600242D RID: 9261 RVA: 0x000D1409 File Offset: 0x000CF609
	private void OnTagsChanged(object data)
	{
		if (this.isMarkedForMove && !this.HasTagRequiredToMove() && this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x0600242E RID: 9262 RVA: 0x000D143C File Offset: 0x000CF63C
	public void ClearMove()
	{
		if (this.isMarkedForMove)
		{
			this.isMarkedForMove = false;
			KSelectable component = base.GetComponent<KSelectable>();
			this.pendingMoveGuid = component.RemoveStatusItem(this.pendingMoveGuid, false);
			this.storageUnreachableGuid = component.RemoveStatusItem(this.storageUnreachableGuid, false);
			this.ClearStorageProxy();
			base.gameObject.RemoveTag(GameTags.MarkedForMove);
			base.Unsubscribe(ref this.reachableChangedHandle);
			base.Unsubscribe(ref this.cancelHandle);
			base.Unsubscribe(ref this.tagsChangedHandle);
		}
		this.UpdateStatusItem();
	}

	// Token: 0x0600242F RID: 9263 RVA: 0x000D14C5 File Offset: 0x000CF6C5
	private void ClearStorageProxy()
	{
		this.StorageProxy.Unsubscribe(ref this.storageReachableChangedHandle);
		this.storageProxy = null;
	}

	// Token: 0x06002430 RID: 9264 RVA: 0x000D14DF File Offset: 0x000CF6DF
	private void OnClickMove()
	{
		MoveToLocationTool.Instance.Activate(this);
	}

	// Token: 0x06002431 RID: 9265 RVA: 0x000D14EC File Offset: 0x000CF6EC
	private void OnClickCancel()
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x06002432 RID: 9266 RVA: 0x000D1510 File Offset: 0x000CF710
	private void OnRefreshUserMenu(object data)
	{
		if (this.pickupable.KPrefabID.HasTag(GameTags.Stored) || !this.HasTagRequiredToMove())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForMove ? new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME, new System.Action(this.OnClickMove), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06002433 RID: 9267 RVA: 0x000D15C9 File Offset: 0x000CF7C9
	private bool HasTagRequiredToMove()
	{
		return this.tagRequiredForMove == Tag.Invalid || this.pickupable.KPrefabID.HasTag(this.tagRequiredForMove);
	}

	// Token: 0x06002434 RID: 9268 RVA: 0x000D15F5 File Offset: 0x000CF7F5
	public void MoveToLocation(int cell)
	{
		this.CreateStorageProxy(cell);
		this.MarkForMove();
		base.gameObject.Trigger(1122777325, base.gameObject);
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x000D161C File Offset: 0x000CF81C
	private void MarkForMove()
	{
		base.Trigger(2127324410, null);
		this.isMarkedForMove = true;
		this.OnReachableChanged(null);
		this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, Movable.OnReachableChangedDispatcher, this);
		this.reachableChangedHandle = base.Subscribe(-1432940121, Movable.OnReachableChangedDispatcher, this);
		this.StorageProxy.GetComponent<CancellableMove>().SetMovable(this);
		base.gameObject.AddTag(GameTags.MarkedForMove);
		this.cancelHandle = base.Subscribe(2127324410, Movable.CleanupMoveDispatcher, this);
		this.tagsChangedHandle = base.Subscribe(-1582839653, Movable.OnTagsChangedDispatcher, this);
		this.UpdateStatusItem();
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x000D16CB File Offset: 0x000CF8CB
	private void UpdateStatusItem()
	{
		if (Movable.IsCritterPickupable(base.gameObject))
		{
			this.shouldShowSkillPerkStatusItem = this.isMarkedForMove;
			base.UpdateStatusItem(null);
		}
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x000D16ED File Offset: 0x000CF8ED
	public bool CanMoveTo(int cell)
	{
		return !Grid.IsSolidCell(cell) && Grid.IsWorldValidCell(cell) && base.gameObject.IsMyParentWorld(cell);
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x000D1710 File Offset: 0x000CF910
	private void CreateStorageProxy(int cell)
	{
		if (this.storageProxy == null || this.storageProxy.Get() == null)
		{
			if (Grid.Objects[cell, 44] != null)
			{
				Storage component = Grid.Objects[cell, 44].GetComponent<Storage>();
				this.storageProxy = new Ref<Storage>(component);
				return;
			}
			Vector3 position = Grid.CellToPosCBC(cell, MoveToLocationTool.Instance.visualizerLayer);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MovePickupablePlacerConfig.ID), position);
			Storage component2 = gameObject.GetComponent<Storage>();
			gameObject.SetActive(true);
			this.storageProxy = new Ref<Storage>(component2);
		}
	}

	// Token: 0x06002439 RID: 9273 RVA: 0x000D17AC File Offset: 0x000CF9AC
	public static bool IsCritterPickupable(GameObject pickupable_go)
	{
		return pickupable_go.GetComponent<Capturable>();
	}

	// Token: 0x0400150E RID: 5390
	[MyCmpReq]
	private Pickupable pickupable;

	// Token: 0x0400150F RID: 5391
	public Tag tagRequiredForMove = Tag.Invalid;

	// Token: 0x04001510 RID: 5392
	[Serialize]
	private bool isMarkedForMove;

	// Token: 0x04001511 RID: 5393
	[Serialize]
	private Ref<Storage> storageProxy;

	// Token: 0x04001512 RID: 5394
	private int storageReachableChangedHandle = -1;

	// Token: 0x04001513 RID: 5395
	private int reachableChangedHandle = -1;

	// Token: 0x04001514 RID: 5396
	private int cancelHandle = -1;

	// Token: 0x04001515 RID: 5397
	private int tagsChangedHandle = -1;

	// Token: 0x04001516 RID: 5398
	private Guid pendingMoveGuid;

	// Token: 0x04001517 RID: 5399
	private Guid storageUnreachableGuid;

	// Token: 0x04001518 RID: 5400
	public Action<GameObject> onDeliveryComplete;

	// Token: 0x04001519 RID: 5401
	public Action<GameObject> onPickupComplete;

	// Token: 0x0400151A RID: 5402
	private static Action<object, object> OnReachableChangedDispatcher = delegate(object context, object data)
	{
		Unsafe.As<Movable>(context).OnReachableChanged(data);
	};

	// Token: 0x0400151B RID: 5403
	private static Action<object, object> OnSplitFromChunkDispatcher = delegate(object context, object data)
	{
		Unsafe.As<Movable>(context).OnSplitFromChunk(data);
	};

	// Token: 0x0400151C RID: 5404
	private static Action<object, object> CleanupMoveDispatcher = delegate(object context, object data)
	{
		Unsafe.As<Movable>(context).CleanupMove(data);
	};

	// Token: 0x0400151D RID: 5405
	private static Action<object, object> OnTagsChangedDispatcher = delegate(object context, object data)
	{
		Unsafe.As<Movable>(context).OnTagsChanged(data);
	};

	// Token: 0x0400151E RID: 5406
	private static Action<object, object> OnRefreshUserMenuDispatcher = delegate(object context, object data)
	{
		Unsafe.As<Movable>(context).OnRefreshUserMenu(data);
	};
}
