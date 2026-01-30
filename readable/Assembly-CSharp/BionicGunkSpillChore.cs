using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class BionicGunkSpillChore : Chore<BionicGunkSpillChore.StatesInstance>
{
	// Token: 0x060018F2 RID: 6386 RVA: 0x0008A640 File Offset: 0x00088840
	public static bool HasSuit(BionicGunkSpillChore.StatesInstance smi)
	{
		return smi.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x0008A654 File Offset: 0x00088854
	public static void ExpellGunkUpdate(BionicGunkSpillChore.StatesInstance smi, float dt)
	{
		float num = GunkMonitor.GUNK_CAPACITY * (dt / 10f);
		if (num >= smi.gunkMonitor.CurrentGunkMass)
		{
			smi.GoTo(smi.sm.pst);
			return;
		}
		smi.gunkMonitor.ExpellGunk(num, null);
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x0008A69C File Offset: 0x0008889C
	public BionicGunkSpillChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.ExpellGunk, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicGunkSpillChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x04000E65 RID: 3685
	public const float EVENT_DURATION = 10f;

	// Token: 0x04000E66 RID: 3686
	public const string PRE_ANIM_NAME = "oiloverload_pre";

	// Token: 0x04000E67 RID: 3687
	public const string LOOP_ANIM_NAME = "oiloverload_loop";

	// Token: 0x04000E68 RID: 3688
	public const string PST_ANIM_NAME = "overload_pst";

	// Token: 0x04000E69 RID: 3689
	public const string SUIT_PRE_ANIM_NAME = "oiloverload_helmet_pre";

	// Token: 0x04000E6A RID: 3690
	public const string SUIT_LOOP_ANIM_NAME = "oiloverload_helmet_loop";

	// Token: 0x04000E6B RID: 3691
	public const string SUIT_PST_ANIM_NAME = "oiloverload_helmet_pst";

	// Token: 0x020012C4 RID: 4804
	public class States : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore>
	{
		// Token: 0x0600895E RID: 35166 RVA: 0x00351F68 File Offset: 0x00350168
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.enter;
			base.Target(this.worker);
			this.root.ToggleAnims("anim_bionic_oil_overload_kanim", 0f).ToggleEffect("ExpellingGunk").ToggleTag(GameTags.MakingMess).DoNotification((BionicGunkSpillChore.StatesInstance smi) => smi.stressfullyEmptyingGunk).Enter(delegate(BionicGunkSpillChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled() && smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			});
			this.enter.DefaultState(this.enter.noSuit);
			this.enter.noSuit.EventTransition(GameHashes.EquippedItemEquipper, this.enter.suit, new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit)).PlayAnim("oiloverload_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.running);
			this.enter.suit.EventTransition(GameHashes.UnequippedItemEquipper, this.enter.noSuit, GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Not(new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit))).PlayAnim("oiloverload_helmet_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.running);
			this.running.DefaultState(this.running.noSuit).Update(new Action<BionicGunkSpillChore.StatesInstance, float>(BionicGunkSpillChore.ExpellGunkUpdate), UpdateRate.SIM_200ms, false);
			this.running.noSuit.EventTransition(GameHashes.EquippedItemEquipper, this.running.suit, new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit)).PlayAnim("oiloverload_loop", KAnim.PlayMode.Loop);
			this.running.suit.EventTransition(GameHashes.UnequippedItemEquipper, this.running.noSuit, GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Not(new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit))).PlayAnim("oiloverload_helmet_loop", KAnim.PlayMode.Loop);
			this.pst.DefaultState(this.pst.noSuit);
			this.pst.noSuit.EventTransition(GameHashes.EquippedItemEquipper, this.pst.suit, new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit)).PlayAnim("overload_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
			this.pst.suit.EventTransition(GameHashes.UnequippedItemEquipper, this.pst.noSuit, GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Not(new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit))).PlayAnim("oiloverload_helmet_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x040068DC RID: 26844
		public BionicGunkSpillChore.States.SuitAnimState enter;

		// Token: 0x040068DD RID: 26845
		public BionicGunkSpillChore.States.SuitAnimState running;

		// Token: 0x040068DE RID: 26846
		public BionicGunkSpillChore.States.SuitAnimState pst;

		// Token: 0x040068DF RID: 26847
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State complete;

		// Token: 0x040068E0 RID: 26848
		public StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.TargetParameter worker;

		// Token: 0x0200279F RID: 10143
		public class SuitAnimState : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State
		{
			// Token: 0x0400AFBA RID: 44986
			public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State noSuit;

			// Token: 0x0400AFBB RID: 44987
			public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State suit;
		}
	}

	// Token: 0x020012C5 RID: 4805
	public class StatesInstance : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.GameInstance
	{
		// Token: 0x06008960 RID: 35168 RVA: 0x003521F4 File Offset: 0x003503F4
		public StatesInstance(BionicGunkSpillChore master, GameObject worker) : base(master)
		{
			this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
			base.sm.worker.Set(worker, base.smi, false);
		}

		// Token: 0x040068E1 RID: 26849
		public Notification stressfullyEmptyingGunk = new Notification(DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);

		// Token: 0x040068E2 RID: 26850
		public GunkMonitor.Instance gunkMonitor;
	}
}
