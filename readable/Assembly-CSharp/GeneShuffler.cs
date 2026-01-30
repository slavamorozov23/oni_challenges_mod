using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000765 RID: 1893
[AddComponentMenu("KMonoBehaviour/Workable/GeneShuffler")]
public class GeneShuffler : Workable
{
	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06002FE5 RID: 12261 RVA: 0x00114776 File Offset: 0x00112976
	public bool WorkComplete
	{
		get
		{
			return this.geneShufflerSMI.IsInsideState(this.geneShufflerSMI.sm.working.complete);
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x00114798 File Offset: 0x00112998
	public bool IsWorking
	{
		get
		{
			return this.geneShufflerSMI.IsInsideState(this.geneShufflerSMI.sm.working);
		}
	}

	// Token: 0x06002FE7 RID: 12263 RVA: 0x001147B5 File Offset: 0x001129B5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.assignable.OnAssign += this.Assign;
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x06002FE8 RID: 12264 RVA: 0x001147DC File Offset: 0x001129DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.showProgressBar = false;
		this.geneShufflerSMI = new GeneShuffler.GeneShufflerSM.Instance(this);
		this.RefreshRechargeChore();
		this.RefreshConsumedState();
		base.Subscribe<GeneShuffler>(-1697596308, GeneShuffler.OnStorageChangeDelegate);
		this.geneShufflerSMI.StartSM();
	}

	// Token: 0x06002FE9 RID: 12265 RVA: 0x0011482A File Offset: 0x00112A2A
	private void Assign(IAssignableIdentity new_assignee)
	{
		this.CancelChore();
		if (new_assignee != null)
		{
			this.ActivateChore();
		}
	}

	// Token: 0x06002FEA RID: 12266 RVA: 0x0011483B File Offset: 0x00112A3B
	private void Recharge()
	{
		this.SetConsumed(false);
		this.RequestRecharge(false);
		this.RefreshRechargeChore();
		this.RefreshSideScreen();
	}

	// Token: 0x06002FEB RID: 12267 RVA: 0x00114857 File Offset: 0x00112A57
	private void SetConsumed(bool consumed)
	{
		this.IsConsumed = consumed;
		this.RefreshConsumedState();
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x00114866 File Offset: 0x00112A66
	private void RefreshConsumedState()
	{
		this.geneShufflerSMI.sm.isCharged.Set(!this.IsConsumed, this.geneShufflerSMI, false);
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x00114890 File Offset: 0x00112A90
	private void OnStorageChange(object data)
	{
		if (this.storage_recursion_guard)
		{
			return;
		}
		this.storage_recursion_guard = true;
		if (this.IsConsumed)
		{
			for (int i = this.storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.storage.items[i];
				if (!(gameObject == null) && gameObject.IsPrefabID(GeneShuffler.RechargeTag))
				{
					this.storage.ConsumeIgnoringDisease(gameObject);
					this.Recharge();
					break;
				}
			}
		}
		this.storage_recursion_guard = false;
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x00114918 File Offset: 0x00112B18
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.notification = new Notification(MISC.NOTIFICATIONS.GENESHUFFLER.NAME, NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.GENESHUFFLER.TOOLTIP + notificationList.ReduceMessages(false), null, false, 0f, null, null, null, true, false, false);
		this.notifier.Add(this.notification, "");
		this.DeSelectBuilding();
	}

	// Token: 0x06002FEF RID: 12271 RVA: 0x0011498A File Offset: 0x00112B8A
	private void DeSelectBuilding()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x001149A5 File Offset: 0x00112BA5
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x001149AF File Offset: 0x00112BAF
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		if (this.chore != null)
		{
			this.chore.Cancel("aborted");
		}
		this.notifier.Remove(this.notification);
	}

	// Token: 0x06002FF2 RID: 12274 RVA: 0x001149E1 File Offset: 0x00112BE1
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.chore != null)
		{
			this.chore.Cancel("stopped");
		}
		this.notifier.Remove(this.notification);
	}

	// Token: 0x06002FF3 RID: 12275 RVA: 0x00114A14 File Offset: 0x00112C14
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		CameraController.Instance.CameraGoTo(base.transform.GetPosition(), 1f, false);
		this.ApplyRandomTrait(worker);
		this.assignable.Unassign();
		this.DeSelectBuilding();
		this.notifier.Remove(this.notification);
	}

	// Token: 0x06002FF4 RID: 12276 RVA: 0x00114A6C File Offset: 0x00112C6C
	private void ApplyRandomTrait(WorkerBase worker)
	{
		Traits component = worker.GetComponent<Traits>();
		List<string> list = new List<string>();
		foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.GENESHUFFLERTRAITS)
		{
			if (!component.HasTrait(traitVal.id))
			{
				list.Add(traitVal.id);
			}
		}
		if (list.Count > 0)
		{
			string id = list[UnityEngine.Random.Range(0, list.Count)];
			Trait trait = Db.Get().traits.TryGet(id);
			worker.GetComponent<Traits>().Add(trait);
			InfoDialogScreen infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
			string text = string.Format(UI.GENESHUFFLERMESSAGE.BODY_SUCCESS, worker.GetProperName(), trait.GetName(), trait.GetTooltip());
			infoDialogScreen.SetHeader(UI.GENESHUFFLERMESSAGE.HEADER).AddPlainText(text).AddDefaultOK(false);
			this.SetConsumed(true);
			return;
		}
		InfoDialogScreen infoDialogScreen2 = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		string text2 = string.Format(UI.GENESHUFFLERMESSAGE.BODY_FAILURE, worker.GetProperName());
		infoDialogScreen2.SetHeader(UI.GENESHUFFLERMESSAGE.HEADER).AddPlainText(text2).AddDefaultOK(false);
	}

	// Token: 0x06002FF5 RID: 12277 RVA: 0x00114BFC File Offset: 0x00112DFC
	private void ActivateChore()
	{
		global::Debug.Assert(this.chore == null);
		base.GetComponent<Workable>().SetWorkTime(float.PositiveInfinity);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.GeneShuffle, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim("anim_interacts_neuralvacillator_kanim"), false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x06002FF6 RID: 12278 RVA: 0x00114C6C File Offset: 0x00112E6C
	private void CancelChore()
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06002FF7 RID: 12279 RVA: 0x00114C8E File Offset: 0x00112E8E
	private void CompleteChore()
	{
		this.chore.Cleanup();
		this.chore = null;
	}

	// Token: 0x06002FF8 RID: 12280 RVA: 0x00114CA2 File Offset: 0x00112EA2
	public void RequestRecharge(bool request)
	{
		this.RechargeRequested = request;
		this.RefreshRechargeChore();
	}

	// Token: 0x06002FF9 RID: 12281 RVA: 0x00114CB1 File Offset: 0x00112EB1
	private void RefreshRechargeChore()
	{
		this.delivery.Pause(!this.RechargeRequested, "No recharge requested");
	}

	// Token: 0x06002FFA RID: 12282 RVA: 0x00114CCC File Offset: 0x00112ECC
	public void RefreshSideScreen()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x00114CEB File Offset: 0x00112EEB
	public void SetAssignable(bool set_it)
	{
		this.assignable.SetCanBeAssigned(set_it);
		this.RefreshSideScreen();
	}

	// Token: 0x04001C87 RID: 7303
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04001C88 RID: 7304
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001C89 RID: 7305
	[MyCmpReq]
	public ManualDeliveryKG delivery;

	// Token: 0x04001C8A RID: 7306
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04001C8B RID: 7307
	[Serialize]
	public bool IsConsumed;

	// Token: 0x04001C8C RID: 7308
	[Serialize]
	public bool RechargeRequested;

	// Token: 0x04001C8D RID: 7309
	private Chore chore;

	// Token: 0x04001C8E RID: 7310
	private GeneShuffler.GeneShufflerSM.Instance geneShufflerSMI;

	// Token: 0x04001C8F RID: 7311
	private Notification notification;

	// Token: 0x04001C90 RID: 7312
	private static Tag RechargeTag = new Tag("GeneShufflerRecharge");

	// Token: 0x04001C91 RID: 7313
	private static readonly EventSystem.IntraObjectHandler<GeneShuffler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<GeneShuffler>(delegate(GeneShuffler component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04001C92 RID: 7314
	private bool storage_recursion_guard;

	// Token: 0x02001652 RID: 5714
	public class GeneShufflerSM : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler>
	{
		// Token: 0x060096D1 RID: 38609 RVA: 0x00381258 File Offset: 0x0037F458
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("on").Enter(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.SetAssignable(true);
			}).Exit(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.SetAssignable(false);
			}).WorkableStartTransition((GeneShuffler.GeneShufflerSM.Instance smi) => smi.master, this.working.pre).ParamTransition<bool>(this.isCharged, this.consumed, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsFalse);
			this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
			this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, this.working.complete);
			this.working.complete.ToggleStatusItem(Db.Get().BuildingStatusItems.GeneShuffleCompleted, null).Enter(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			}).WorkableStopTransition((GeneShuffler.GeneShufflerSM.Instance smi) => smi.master, this.working.pst);
			this.working.pst.OnAnimQueueComplete(this.consumed);
			this.consumed.PlayAnim("off", KAnim.PlayMode.Once).ParamTransition<bool>(this.isCharged, this.recharging, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsTrue);
			this.recharging.PlayAnim("recharging", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		}

		// Token: 0x0400747A RID: 29818
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State idle;

		// Token: 0x0400747B RID: 29819
		public GeneShuffler.GeneShufflerSM.WorkingStates working;

		// Token: 0x0400747C RID: 29820
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State consumed;

		// Token: 0x0400747D RID: 29821
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State recharging;

		// Token: 0x0400747E RID: 29822
		public StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.BoolParameter isCharged;

		// Token: 0x020028E6 RID: 10470
		public class WorkingStates : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State
		{
			// Token: 0x0400B47F RID: 46207
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pre;

			// Token: 0x0400B480 RID: 46208
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State loop;

			// Token: 0x0400B481 RID: 46209
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State complete;

			// Token: 0x0400B482 RID: 46210
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pst;
		}

		// Token: 0x020028E7 RID: 10471
		public new class Instance : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.GameInstance
		{
			// Token: 0x0600CE2F RID: 52783 RVA: 0x004325EB File Offset: 0x004307EB
			public Instance(GeneShuffler master) : base(master)
			{
			}
		}
	}
}
