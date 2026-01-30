using System;
using Klei.AI;
using TUNING;

// Token: 0x020004B2 RID: 1202
public class PartyChore : Chore<PartyChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x0600197B RID: 6523 RVA: 0x0008E6A8 File Offset: 0x0008C8A8
	public PartyChore(IStateMachineTarget master, Workable chat_workable, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null) : base(Db.Get().ChoreTypes.Party, master, master.GetComponent<ChoreProvider>(), true, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new PartyChore.StatesInstance(this);
		base.smi.sm.chitchatlocator.Set(chat_workable, base.smi);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, chat_workable);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x0008E734 File Offset: 0x0008C934
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.partyer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
		base.smi.sm.partyer.Get(base.smi).gameObject.AddTag(GameTags.Partying);
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x0008E79C File Offset: 0x0008C99C
	protected override void End(string reason)
	{
		if (base.smi.sm.partyer.Get(base.smi) != null)
		{
			base.smi.sm.partyer.Get(base.smi).gameObject.RemoveTag(GameTags.Partying);
		}
		base.End(reason);
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x0008E7FD File Offset: 0x0008C9FD
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000ED4 RID: 3796
	public int basePriority = RELAXATION.PRIORITY.SPECIAL_EVENT;

	// Token: 0x04000ED5 RID: 3797
	public const string specificEffect = "Socialized";

	// Token: 0x04000ED6 RID: 3798
	public const string trackingEffect = "RecentlySocialized";

	// Token: 0x020012FD RID: 4861
	public class States : GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore>
	{
		// Token: 0x06008A55 RID: 35413 RVA: 0x00358874 File Offset: 0x00356A74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.stand;
			base.Target(this.partyer);
			this.stand.InitializeStates(this.partyer, this.masterTarget, this.chat, null, null, null);
			this.chat_move.InitializeStates(this.partyer, this.chitchatlocator, this.chat, null, null, null);
			this.chat.ToggleWork<Workable>(this.chitchatlocator, this.success, null, null);
			this.success.Enter(delegate(PartyChore.StatesInstance smi)
			{
				this.partyer.Get(smi).gameObject.GetComponent<Effects>().Add("RecentlyPartied", true);
			}).ReturnSuccess();
		}

		// Token: 0x040069D9 RID: 27097
		public StateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.TargetParameter partyer;

		// Token: 0x040069DA RID: 27098
		public StateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.TargetParameter chitchatlocator;

		// Token: 0x040069DB RID: 27099
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.ApproachSubState<IApproachable> stand;

		// Token: 0x040069DC RID: 27100
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.ApproachSubState<IApproachable> chat_move;

		// Token: 0x040069DD RID: 27101
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.State chat;

		// Token: 0x040069DE RID: 27102
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.State success;
	}

	// Token: 0x020012FE RID: 4862
	public class StatesInstance : GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.GameInstance
	{
		// Token: 0x06008A58 RID: 35416 RVA: 0x0035893A File Offset: 0x00356B3A
		public StatesInstance(PartyChore master) : base(master)
		{
		}
	}
}
