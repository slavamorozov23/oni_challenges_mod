using System;
using System.Collections.Generic;

// Token: 0x020007D9 RID: 2009
public class PartyCake : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>
{
	// Token: 0x06003559 RID: 13657 RVA: 0x0012D538 File Offset: 0x0012B738
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.creating.ready;
		this.creating.ready.PlayAnim("base").GoTo(this.creating.tier1);
		this.creating.tier1.InitializeStates(this.masterTarget, "tier_1", this.creating.tier2);
		this.creating.tier2.InitializeStates(this.masterTarget, "tier_2", this.creating.tier3);
		this.creating.tier3.InitializeStates(this.masterTarget, "tier_3", this.ready_to_party);
		this.ready_to_party.PlayAnim("unlit");
		this.party.PlayAnim("lit");
		this.complete.PlayAnim("finished");
	}

	// Token: 0x0600355A RID: 13658 RVA: 0x0012D61C File Offset: 0x0012B81C
	private static Chore CreateFetchChore(PartyCake.StatesInstance smi)
	{
		return new FetchChore(Db.Get().ChoreTypes.FarmFetch, smi.GetComponent<Storage>(), 10f, new HashSet<Tag>
		{
			"MushBar".ToTag()
		}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.Functional, 0);
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x0012D66C File Offset: 0x0012B86C
	private static Chore CreateWorkChore(PartyCake.StatesInstance smi)
	{
		Workable component = smi.master.GetComponent<PartyCakeWorkable>();
		return new WorkChore<PartyCakeWorkable>(Db.Get().ChoreTypes.Cook, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Work, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x04002044 RID: 8260
	public PartyCake.CreatingStates creating;

	// Token: 0x04002045 RID: 8261
	public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State ready_to_party;

	// Token: 0x04002046 RID: 8262
	public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State party;

	// Token: 0x04002047 RID: 8263
	public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State complete;

	// Token: 0x02001730 RID: 5936
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001731 RID: 5937
	public class CreatingStates : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State
	{
		// Token: 0x0400770D RID: 30477
		public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State ready;

		// Token: 0x0400770E RID: 30478
		public PartyCake.CreatingStates.Tier tier1;

		// Token: 0x0400770F RID: 30479
		public PartyCake.CreatingStates.Tier tier2;

		// Token: 0x04007710 RID: 30480
		public PartyCake.CreatingStates.Tier tier3;

		// Token: 0x04007711 RID: 30481
		public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State finish;

		// Token: 0x02002925 RID: 10533
		public class Tier : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State
		{
			// Token: 0x0600CF91 RID: 53137 RVA: 0x00433FF8 File Offset: 0x004321F8
			public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State InitializeStates(StateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.TargetParameter targ, string animPrefix, GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State success)
			{
				base.root.Target(targ).DefaultState(this.fetch);
				this.fetch.PlayAnim(animPrefix + "_ready").ToggleChore(new Func<PartyCake.StatesInstance, Chore>(PartyCake.CreateFetchChore), this.work);
				this.work.Enter(delegate(PartyCake.StatesInstance smi)
				{
					KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
					component.Play(animPrefix + "_working", KAnim.PlayMode.Once, 1f, 0f);
					component.SetPositionPercent(0f);
				}).ToggleChore(new Func<PartyCake.StatesInstance, Chore>(PartyCake.CreateWorkChore), success, this.work);
				return this;
			}

			// Token: 0x0400B5EE RID: 46574
			public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State fetch;

			// Token: 0x0400B5EF RID: 46575
			public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State work;
		}
	}

	// Token: 0x02001732 RID: 5938
	public class StatesInstance : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.GameInstance
	{
		// Token: 0x06009A49 RID: 39497 RVA: 0x0039077B File Offset: 0x0038E97B
		public StatesInstance(IStateMachineTarget smi, PartyCake.Def def) : base(smi, def)
		{
		}
	}
}
