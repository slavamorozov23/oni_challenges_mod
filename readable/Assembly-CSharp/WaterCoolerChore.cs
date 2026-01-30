using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020004CA RID: 1226
public class WaterCoolerChore : Chore<WaterCoolerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x060019C4 RID: 6596 RVA: 0x00090268 File Offset: 0x0008E468
	public WaterCoolerChore(IStateMachineTarget master, Workable chat_workable, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null) : base(Db.Get().ChoreTypes.Relax, master, master.GetComponent<ChoreProvider>(), true, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new WaterCoolerChore.StatesInstance(this);
		base.smi.sm.chitchatlocator.Set(chat_workable, base.smi);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, chat_workable);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x0009033A File Offset: 0x0008E53A
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.drinker.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x0009036C File Offset: 0x0008E56C
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04000EE8 RID: 3816
	public int basePriority = RELAXATION.PRIORITY.TIER2;

	// Token: 0x04000EE9 RID: 3817
	public string specificEffect = "Socialized";

	// Token: 0x04000EEA RID: 3818
	public string trackingEffect = "RecentlySocialized";

	// Token: 0x02001337 RID: 4919
	public class States : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore>
	{
		// Token: 0x06008B44 RID: 35652 RVA: 0x0035E8FC File Offset: 0x0035CAFC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.drink_move;
			base.Target(this.drinker);
			this.drink_move.InitializeStates(this.drinker, this.masterTarget, this.drink, null, null, null);
			this.drink.ToggleAnims(new Func<WaterCoolerChore.StatesInstance, KAnimFile>(WaterCoolerChore.States.GetAnimFileName)).DefaultState(this.drink.drink);
			this.drink.drink.Face(this.masterTarget, 0.5f).PlayAnim("working_pre").QueueAnim("working_loop", false, null).OnAnimQueueComplete(this.drink.post);
			this.drink.post.Enter("Drink", new StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State.Callback(this.TriggerDrink)).Enter("Mark", new StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State.Callback(this.MarkAsRecentlySocialized)).PlayAnim("working_pst").OnAnimQueueComplete(this.chat_move);
			this.chat_move.InitializeStates(this.drinker, this.chitchatlocator, this.chat, null, null, null);
			this.chat.ToggleWork<SocialGatheringPointWorkable>(this.chitchatlocator, this.success, null, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x06008B45 RID: 35653 RVA: 0x0035EA3C File Offset: 0x0035CC3C
		public static KAnimFile GetAnimFileName(WaterCoolerChore.StatesInstance smi)
		{
			GameObject gameObject = smi.sm.drinker.Get(smi);
			if (gameObject == null)
			{
				return Assets.GetAnim("anim_interacts_watercooler_kanim");
			}
			MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
			if (component != null && component.model == BionicMinionConfig.MODEL)
			{
				return Assets.GetAnim("anim_bionic_interacts_watercooler_kanim");
			}
			return Assets.GetAnim("anim_interacts_watercooler_kanim");
		}

		// Token: 0x06008B46 RID: 35654 RVA: 0x0035EAB8 File Offset: 0x0035CCB8
		private void MarkAsRecentlySocialized(WaterCoolerChore.StatesInstance smi)
		{
			Effects component = this.stateTarget.Get<WorkerBase>(smi).GetComponent<Effects>();
			if (!string.IsNullOrEmpty(smi.master.trackingEffect))
			{
				component.Add(smi.master.trackingEffect, true);
			}
		}

		// Token: 0x06008B47 RID: 35655 RVA: 0x0035EAFC File Offset: 0x0035CCFC
		private void TriggerDrink(WaterCoolerChore.StatesInstance smi)
		{
			WorkerBase workerBase = this.stateTarget.Get<WorkerBase>(smi);
			smi.master.target.gameObject.GetSMI<WaterCooler.StatesInstance>().Drink(workerBase.gameObject, true);
		}

		// Token: 0x04006AA6 RID: 27302
		public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter drinker;

		// Token: 0x04006AA7 RID: 27303
		public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter chitchatlocator;

		// Token: 0x04006AA8 RID: 27304
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<WaterCooler> drink_move;

		// Token: 0x04006AA9 RID: 27305
		public WaterCoolerChore.States.DrinkStates drink;

		// Token: 0x04006AAA RID: 27306
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<IApproachable> chat_move;

		// Token: 0x04006AAB RID: 27307
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State chat;

		// Token: 0x04006AAC RID: 27308
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State success;

		// Token: 0x020027F2 RID: 10226
		public class DrinkStates : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State
		{
			// Token: 0x0400B115 RID: 45333
			public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State drink;

			// Token: 0x0400B116 RID: 45334
			public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State post;
		}
	}

	// Token: 0x02001338 RID: 4920
	public class StatesInstance : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.GameInstance
	{
		// Token: 0x06008B49 RID: 35657 RVA: 0x0035EB3F File Offset: 0x0035CD3F
		public StatesInstance(WaterCoolerChore master) : base(master)
		{
		}
	}
}
