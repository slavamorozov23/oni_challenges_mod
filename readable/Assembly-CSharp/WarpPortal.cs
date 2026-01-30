using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000829 RID: 2089
public class WarpPortal : Workable
{
	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x060038EB RID: 14571 RVA: 0x0013E874 File Offset: 0x0013CA74
	public bool ReadyToWarp
	{
		get
		{
			return this.warpPortalSMI.IsInsideState(this.warpPortalSMI.sm.occupied.waiting);
		}
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x060038EC RID: 14572 RVA: 0x0013E896 File Offset: 0x0013CA96
	public bool IsWorking
	{
		get
		{
			return this.warpPortalSMI.IsInsideState(this.warpPortalSMI.sm.occupied);
		}
	}

	// Token: 0x060038ED RID: 14573 RVA: 0x0013E8B3 File Offset: 0x0013CAB3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.assignable.OnAssign += this.Assign;
	}

	// Token: 0x060038EE RID: 14574 RVA: 0x0013E8D4 File Offset: 0x0013CAD4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.warpPortalSMI = new WarpPortal.WarpPortalSM.Instance(this);
		this.warpPortalSMI.sm.isCharged.Set(!this.IsConsumed, this.warpPortalSMI, false);
		this.warpPortalSMI.StartSM();
		this.selectEventHandle = Game.Instance.Subscribe(-1503271301, new Action<object>(this.OnObjectSelected));
	}

	// Token: 0x060038EF RID: 14575 RVA: 0x0013E945 File Offset: 0x0013CB45
	private void OnObjectSelected(object data)
	{
		if (data != null && (GameObject)data == base.gameObject && Components.LiveMinionIdentities.Count > 0)
		{
			this.Discover();
		}
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x0013E970 File Offset: 0x0013CB70
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(this.selectEventHandle);
		base.OnCleanUp();
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x0013E988 File Offset: 0x0013CB88
	private void Discover()
	{
		if (this.discovered)
		{
			return;
		}
		ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).SetDiscovered(true);
		SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.WarpWorldReveal, -1, null).smi as SimpleEvent.StatesInstance;
		statesInstance.minions = new GameObject[]
		{
			Components.LiveMinionIdentities[0].gameObject
		};
		statesInstance.callback = delegate()
		{
			ManagementMenu.Instance.OpenClusterMap();
			ClusterMapScreen.Instance.SetTargetFocusPosition(ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).GetMyWorldLocation(), 0.5f);
		};
		statesInstance.ShowEventPopup();
		this.discovered = true;
	}

	// Token: 0x060038F2 RID: 14578 RVA: 0x0013EA18 File Offset: 0x0013CC18
	public void StartWarpSequence()
	{
		this.warpPortalSMI.GoTo(this.warpPortalSMI.sm.occupied.warping);
	}

	// Token: 0x060038F3 RID: 14579 RVA: 0x0013EA3A File Offset: 0x0013CC3A
	public void CancelAssignment()
	{
		this.CancelChore();
		this.assignable.Unassign();
		this.warpPortalSMI.GoTo(this.warpPortalSMI.sm.idle);
	}

	// Token: 0x060038F4 RID: 14580 RVA: 0x0013EA68 File Offset: 0x0013CC68
	private int GetTargetWorldID()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
		foreach (WarpReceiver component in UnityEngine.Object.FindObjectsOfType<WarpReceiver>())
		{
			if (component.GetMyWorldId() != this.GetMyWorldId())
			{
				return component.GetMyWorldId();
			}
		}
		global::Debug.LogError("No receiver world found for warp portal sender");
		return -1;
	}

	// Token: 0x060038F5 RID: 14581 RVA: 0x0013EAC4 File Offset: 0x0013CCC4
	private void Warp()
	{
		if (base.worker == null || base.worker.HasTag(GameTags.Dying) || base.worker.HasTag(GameTags.Dead))
		{
			return;
		}
		WarpReceiver warpReceiver = null;
		foreach (WarpReceiver warpReceiver2 in UnityEngine.Object.FindObjectsOfType<WarpReceiver>())
		{
			if (warpReceiver2.GetMyWorldId() != this.GetMyWorldId())
			{
				warpReceiver = warpReceiver2;
				break;
			}
		}
		if (warpReceiver == null)
		{
			SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
			warpReceiver = UnityEngine.Object.FindObjectOfType<WarpReceiver>();
		}
		if (warpReceiver != null)
		{
			this.delayWarpRoutine = base.StartCoroutine(this.DelayedWarp(warpReceiver));
		}
		else
		{
			global::Debug.LogWarning("No warp receiver found - maybe POI stomping or failure to spawn?");
		}
		if (SelectTool.Instance.selected == base.GetComponent<KSelectable>())
		{
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x0013EB9E File Offset: 0x0013CD9E
	public IEnumerator DelayedWarp(WarpReceiver receiver)
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		int myWorldId = base.worker.GetMyWorldId();
		int myWorldId2 = receiver.GetMyWorldId();
		GameUtil.FocusCameraOnWorld(myWorldId2, Grid.CellToPos(Grid.PosToCell(receiver)), 10f, null, true);
		WorkerBase worker = base.worker;
		worker.StopWork();
		receiver.ReceiveWarpedDuplicant(worker);
		ClusterManager.Instance.MigrateMinion(worker.GetComponent<MinionIdentity>(), myWorldId2, myWorldId);
		this.delayWarpRoutine = null;
		yield break;
	}

	// Token: 0x060038F7 RID: 14583 RVA: 0x0013EBB4 File Offset: 0x0013CDB4
	public void SetAssignable(bool set_it)
	{
		this.assignable.SetCanBeAssigned(set_it);
		this.RefreshSideScreen();
	}

	// Token: 0x060038F8 RID: 14584 RVA: 0x0013EBC8 File Offset: 0x0013CDC8
	private void Assign(IAssignableIdentity new_assignee)
	{
		this.CancelChore();
		if (new_assignee != null)
		{
			this.ActivateChore();
		}
	}

	// Token: 0x060038F9 RID: 14585 RVA: 0x0013EBDC File Offset: 0x0013CDDC
	private void ActivateChore()
	{
		global::Debug.Assert(this.chore == null);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.Migrate, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim("anim_interacts_warp_portal_sender_kanim"), false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		this.workLayer = Grid.SceneLayer.Building;
		this.workAnims = new HashedString[]
		{
			"sending_pre",
			"sending_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"sending_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"idle_loop"
		};
		this.showProgressBar = false;
	}

	// Token: 0x060038FA RID: 14586 RVA: 0x0013ECBE File Offset: 0x0013CEBE
	private void CancelChore()
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
		if (this.delayWarpRoutine != null)
		{
			base.StopCoroutine(this.delayWarpRoutine);
			this.delayWarpRoutine = null;
		}
	}

	// Token: 0x060038FB RID: 14587 RVA: 0x0013ECFB File Offset: 0x0013CEFB
	private void CompleteChore()
	{
		this.IsConsumed = true;
		this.chore.Cleanup();
		this.chore = null;
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x0013ED16 File Offset: 0x0013CF16
	public void RefreshSideScreen()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x040022C4 RID: 8900
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x040022C5 RID: 8901
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x040022C6 RID: 8902
	private Chore chore;

	// Token: 0x040022C7 RID: 8903
	private WarpPortal.WarpPortalSM.Instance warpPortalSMI;

	// Token: 0x040022C8 RID: 8904
	private Notification notification;

	// Token: 0x040022C9 RID: 8905
	public const float RECHARGE_TIME = 3000f;

	// Token: 0x040022CA RID: 8906
	[Serialize]
	public bool IsConsumed;

	// Token: 0x040022CB RID: 8907
	[Serialize]
	public float rechargeProgress;

	// Token: 0x040022CC RID: 8908
	[Serialize]
	private bool discovered;

	// Token: 0x040022CD RID: 8909
	private int selectEventHandle = -1;

	// Token: 0x040022CE RID: 8910
	private Coroutine delayWarpRoutine;

	// Token: 0x040022CF RID: 8911
	private static readonly HashedString[] printing_anim = new HashedString[]
	{
		"printing_pre",
		"printing_loop",
		"printing_pst"
	};

	// Token: 0x020017D2 RID: 6098
	public class WarpPortalSM : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal>
	{
		// Token: 0x06009CA6 RID: 40102 RVA: 0x0039A974 File Offset: 0x00398B74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				if (smi.master.rechargeProgress != 0f)
				{
					smi.GoTo(this.recharging);
				}
			}).DefaultState(this.idle);
			this.idle.PlayAnim("idle", KAnim.PlayMode.Loop).Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.IsConsumed = false;
				smi.sm.isCharged.Set(true, smi, false);
				smi.master.SetAssignable(true);
			}).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.SetAssignable(false);
			}).WorkableStartTransition((WarpPortal.WarpPortalSM.Instance smi) => smi.master, this.become_occupied).ParamTransition<bool>(this.isCharged, this.recharging, GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.IsFalse);
			this.become_occupied.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				this.worker.Set(smi.master.worker, smi);
				smi.GoTo(this.occupied.get_on);
			});
			this.occupied.OnTargetLost(this.worker, this.idle).Target(this.worker).TagTransition(GameTags.Dying, this.idle, false).Target(this.masterTarget).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				this.worker.Set(null, smi);
			});
			this.occupied.get_on.PlayAnim("sending_pre").OnAnimQueueComplete(this.occupied.waiting);
			this.occupied.waiting.PlayAnim("sending_loop", KAnim.PlayMode.Loop).ToggleNotification((WarpPortal.WarpPortalSM.Instance smi) => smi.CreateDupeWaitingNotification()).Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			}).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			});
			this.occupied.warping.PlayAnim("sending_pst").OnAnimQueueComplete(this.do_warp);
			this.do_warp.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.Warp();
			}).GoTo(this.recharging);
			this.recharging.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.SetAssignable(false);
				smi.master.IsConsumed = true;
				this.isCharged.Set(false, smi, false);
			}).PlayAnim("recharge", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.WarpPortalCharging, (WarpPortal.WarpPortalSM.Instance smi) => smi.master).Update(delegate(WarpPortal.WarpPortalSM.Instance smi, float dt)
			{
				smi.master.rechargeProgress += dt;
				if (smi.master.rechargeProgress > 3000f)
				{
					this.isCharged.Set(true, smi, false);
					smi.master.rechargeProgress = 0f;
					smi.GoTo(this.idle);
				}
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x040078DC RID: 30940
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State idle;

		// Token: 0x040078DD RID: 30941
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State become_occupied;

		// Token: 0x040078DE RID: 30942
		public WarpPortal.WarpPortalSM.OccupiedStates occupied;

		// Token: 0x040078DF RID: 30943
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State do_warp;

		// Token: 0x040078E0 RID: 30944
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State recharging;

		// Token: 0x040078E1 RID: 30945
		public StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.BoolParameter isCharged;

		// Token: 0x040078E2 RID: 30946
		private StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.TargetParameter worker;

		// Token: 0x02002966 RID: 10598
		public class OccupiedStates : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State
		{
			// Token: 0x0400B73F RID: 46911
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State get_on;

			// Token: 0x0400B740 RID: 46912
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State waiting;

			// Token: 0x0400B741 RID: 46913
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State warping;
		}

		// Token: 0x02002967 RID: 10599
		public new class Instance : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.GameInstance
		{
			// Token: 0x0600D0D7 RID: 53463 RVA: 0x00435D1A File Offset: 0x00433F1A
			public Instance(WarpPortal master) : base(master)
			{
			}

			// Token: 0x0600D0D8 RID: 53464 RVA: 0x00435D24 File Offset: 0x00433F24
			public Notification CreateDupeWaitingNotification()
			{
				if (base.master.worker != null)
				{
					return new Notification(MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.NAME.Replace("{dupe}", base.master.worker.name), NotificationType.Neutral, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.TOOLTIP.Replace("{dupe}", base.master.worker.name), null, false, 0f, null, null, base.master.transform, true, false, false);
				}
				return null;
			}
		}
	}
}
