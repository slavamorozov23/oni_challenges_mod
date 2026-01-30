using System;

// Token: 0x02000A3D RID: 2621
public class RationMonitor : GameStateMachine<RationMonitor, RationMonitor.Instance>
{
	// Token: 0x06004C80 RID: 19584 RVA: 0x001BCBE0 File Offset: 0x001BADE0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.rationsavailable;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.EatCompleteEater, delegate(RationMonitor.Instance smi, object d)
		{
			smi.OnEatComplete(d);
		}).EventHandler(GameHashes.NewDay, (RationMonitor.Instance smi) => GameClock.Instance, delegate(RationMonitor.Instance smi)
		{
			smi.OnNewDay();
		}).ParamTransition<float>(this.rationsAteToday, this.rationsavailable, (RationMonitor.Instance smi, float p) => smi.HasRationsAvailable()).ParamTransition<float>(this.rationsAteToday, this.outofrations, (RationMonitor.Instance smi, float p) => !smi.HasRationsAvailable());
		this.rationsavailable.DefaultState(this.rationsavailable.noediblesavailable);
		this.rationsavailable.noediblesavailable.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.NoRationsAvailable).EventTransition(GameHashes.ColonyHasRationsChanged, new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame), this.rationsavailable.ediblesunreachable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereAnyEdibles));
		this.rationsavailable.ediblereachablebutnotpermitted.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.RationsNotPermitted).EventTransition(GameHashes.ColonyHasRationsChanged, new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame), this.rationsavailable.noediblesavailable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereNoEdibles)).EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.ediblesunreachable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.NotIsEdibleInReachButNotPermitted));
		this.rationsavailable.ediblesunreachable.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.RationsUnreachable).EventTransition(GameHashes.ColonyHasRationsChanged, new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame), this.rationsavailable.noediblesavailable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereNoEdibles)).EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.edibleavailable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.IsEdibleAvailable)).EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.ediblereachablebutnotpermitted, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.IsEdibleInReachButNotPermitted));
		this.rationsavailable.edibleavailable.ToggleChore((RationMonitor.Instance smi) => new EatChore(smi.master), this.rationsavailable.noediblesavailable).DefaultState(this.rationsavailable.edibleavailable.readytoeat);
		this.rationsavailable.edibleavailable.readytoeat.EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.noediblesavailable, null).EventTransition(GameHashes.BeginChore, this.rationsavailable.edibleavailable.eating, (RationMonitor.Instance smi) => smi.IsEating());
		this.rationsavailable.edibleavailable.eating.DoNothing();
		this.outofrations.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.DailyRationLimitReached);
	}

	// Token: 0x06004C81 RID: 19585 RVA: 0x001BCF2E File Offset: 0x001BB12E
	private static bool AreThereNoEdibles(RationMonitor.Instance smi)
	{
		return !RationMonitor.AreThereAnyEdibles(smi);
	}

	// Token: 0x06004C82 RID: 19586 RVA: 0x001BCF3C File Offset: 0x001BB13C
	private static bool AreThereAnyEdibles(RationMonitor.Instance smi)
	{
		if (SaveGame.Instance != null)
		{
			ColonyRationMonitor.Instance smi2 = SaveGame.Instance.GetSMI<ColonyRationMonitor.Instance>();
			if (smi2 != null)
			{
				return !smi2.IsOutOfRations();
			}
		}
		return false;
	}

	// Token: 0x06004C83 RID: 19587 RVA: 0x001BCF6F File Offset: 0x001BB16F
	private static KMonoBehaviour GetSaveGame(RationMonitor.Instance smi)
	{
		return SaveGame.Instance;
	}

	// Token: 0x06004C84 RID: 19588 RVA: 0x001BCF76 File Offset: 0x001BB176
	private static bool IsEdibleAvailable(RationMonitor.Instance smi)
	{
		return smi.GetEdible() != null;
	}

	// Token: 0x06004C85 RID: 19589 RVA: 0x001BCF84 File Offset: 0x001BB184
	private static bool NotIsEdibleInReachButNotPermitted(RationMonitor.Instance smi)
	{
		return !RationMonitor.IsEdibleInReachButNotPermitted(smi);
	}

	// Token: 0x06004C86 RID: 19590 RVA: 0x001BCF8F File Offset: 0x001BB18F
	private static bool IsEdibleInReachButNotPermitted(RationMonitor.Instance smi)
	{
		return smi.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().edibleInReachButNotPermitted;
	}

	// Token: 0x040032DE RID: 13022
	public StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.FloatParameter rationsAteToday;

	// Token: 0x040032DF RID: 13023
	public RationMonitor.RationsAvailableState rationsavailable;

	// Token: 0x040032E0 RID: 13024
	public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState outofrations;

	// Token: 0x02001B1C RID: 6940
	public class EdibleAvailablestate : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040083C3 RID: 33731
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State readytoeat;

		// Token: 0x040083C4 RID: 33732
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State eating;
	}

	// Token: 0x02001B1D RID: 6941
	public class RationsAvailableState : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040083C5 RID: 33733
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState noediblesavailable;

		// Token: 0x040083C6 RID: 33734
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblereachablebutnotpermitted;

		// Token: 0x040083C7 RID: 33735
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblesunreachable;

		// Token: 0x040083C8 RID: 33736
		public RationMonitor.EdibleAvailablestate edibleavailable;
	}

	// Token: 0x02001B1E RID: 6942
	public new class Instance : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A872 RID: 43122 RVA: 0x003BEFCF File Offset: 0x003BD1CF
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.choreDriver = master.GetComponent<ChoreDriver>();
		}

		// Token: 0x0600A873 RID: 43123 RVA: 0x003BEFE4 File Offset: 0x003BD1E4
		public Edible GetEdible()
		{
			return base.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().GetEdible();
		}

		// Token: 0x0600A874 RID: 43124 RVA: 0x003BEFF6 File Offset: 0x003BD1F6
		public bool HasRationsAvailable()
		{
			return true;
		}

		// Token: 0x0600A875 RID: 43125 RVA: 0x003BEFF9 File Offset: 0x003BD1F9
		public float GetRationsAteToday()
		{
			return base.sm.rationsAteToday.Get(base.smi);
		}

		// Token: 0x0600A876 RID: 43126 RVA: 0x003BF011 File Offset: 0x003BD211
		public float GetRationsRemaining()
		{
			return 1f;
		}

		// Token: 0x0600A877 RID: 43127 RVA: 0x003BF018 File Offset: 0x003BD218
		public bool IsEating()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
		}

		// Token: 0x0600A878 RID: 43128 RVA: 0x003BF04F File Offset: 0x003BD24F
		public void OnNewDay()
		{
			base.smi.sm.rationsAteToday.Set(0f, base.smi, false);
		}

		// Token: 0x0600A879 RID: 43129 RVA: 0x003BF074 File Offset: 0x003BD274
		public void OnEatComplete(object data)
		{
			Edible edible = (Edible)data;
			base.sm.rationsAteToday.Delta(edible.caloriesConsumed, base.smi);
			WorldResourceAmountTracker<RationTracker>.Get().RegisterAmountConsumed(edible.FoodInfo.Id, edible.caloriesConsumed);
		}

		// Token: 0x040083C9 RID: 33737
		private ChoreDriver choreDriver;
	}
}
