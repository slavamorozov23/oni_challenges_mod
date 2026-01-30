using System;
using Klei.AI;

// Token: 0x02000A10 RID: 2576
public class BladderMonitor : GameStateMachine<BladderMonitor, BladderMonitor.Instance>
{
	// Token: 0x06004B95 RID: 19349 RVA: 0x001B736C File Offset: 0x001B556C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).Transition(this.breakwant, (BladderMonitor.Instance smi) => smi.WantsToPee(), UpdateRate.SIM_200ms);
		this.urgentwant.InitializeStates(this.satisfied).ToggleThought(Db.Get().Thoughts.FullBladder, null).ToggleExpression(Db.Get().Expressions.FullBladder, null).ToggleStateMachine((BladderMonitor.Instance smi) => new PeeChoreMonitor.Instance(smi.master)).ToggleEffect("FullBladder");
		this.breakwant.InitializeStates(this.satisfied);
		this.breakwant.wanting.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).EventTransition(GameHashes.ScheduleBlocksChanged, this.satisfied, (BladderMonitor.Instance smi) => !smi.WantsToPee());
		this.breakwant.peeing.ToggleThought(Db.Get().Thoughts.BreakBladder, null);
	}

	// Token: 0x04003217 RID: 12823
	public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003218 RID: 12824
	public BladderMonitor.WantsToPeeStates urgentwant;

	// Token: 0x04003219 RID: 12825
	public BladderMonitor.WantsToPeeStates breakwant;

	// Token: 0x02001AA2 RID: 6818
	public class WantsToPeeStates : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0600A689 RID: 42633 RVA: 0x003BA520 File Offset: 0x003B8720
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State InitializeStates(GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State donePeeingState)
		{
			base.DefaultState(this.wanting).ToggleUrge(Db.Get().Urges.Pee).ToggleStateMachine((BladderMonitor.Instance smi) => new ToiletMonitor.Instance(smi.master));
			this.wanting.EventTransition(GameHashes.BeginChore, this.peeing, (BladderMonitor.Instance smi) => smi.IsPeeing());
			this.peeing.EventTransition(GameHashes.EndChore, donePeeingState, (BladderMonitor.Instance smi) => !smi.IsPeeing());
			return this;
		}

		// Token: 0x0400824F RID: 33359
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State wanting;

		// Token: 0x04008250 RID: 33360
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State peeing;
	}

	// Token: 0x02001AA3 RID: 6819
	public new class Instance : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A68B RID: 42635 RVA: 0x003BA5E2 File Offset: 0x003B87E2
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.bladder = Db.Get().Amounts.Bladder.Lookup(master.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}

		// Token: 0x0600A68C RID: 42636 RVA: 0x003BA618 File Offset: 0x003B8818
		public bool NeedsToPee()
		{
			if (base.master.IsNullOrDestroyed())
			{
				return false;
			}
			if (base.master.GetComponent<KPrefabID>().HasTag(GameTags.Asleep))
			{
				return false;
			}
			DebugUtil.DevAssert(this.bladder != null, "bladder is null", null);
			return this.bladder.value >= 100f;
		}

		// Token: 0x0600A68D RID: 42637 RVA: 0x003BA676 File Offset: 0x003B8876
		public bool WantsToPee()
		{
			return this.NeedsToPee() || (this.IsPeeTime() && this.bladder.value >= 40f);
		}

		// Token: 0x0600A68E RID: 42638 RVA: 0x003BA6A1 File Offset: 0x003B88A1
		public bool IsPeeing()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Pee);
		}

		// Token: 0x0600A68F RID: 42639 RVA: 0x003BA6D1 File Offset: 0x003B88D1
		public bool IsPeeTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
		}

		// Token: 0x04008251 RID: 33361
		private AmountInstance bladder;

		// Token: 0x04008252 RID: 33362
		private ChoreDriver choreDriver;
	}
}
