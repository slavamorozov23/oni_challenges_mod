using System;
using Klei.AI;
using TUNING;

// Token: 0x02000A13 RID: 2579
public class CalorieMonitor : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance>
{
	// Token: 0x06004BAE RID: 19374 RVA: 0x001B7B70 File Offset: 0x001B5D70
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.Transition(this.hungry, (CalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_200ms);
		this.hungry.DefaultState(this.hungry.normal).Transition(this.satisfied, (CalorieMonitor.Instance smi) => smi.IsSatisfied(), UpdateRate.SIM_200ms).EventTransition(GameHashes.BeginChore, this.eating, (CalorieMonitor.Instance smi) => smi.IsEating());
		this.hungry.working.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.normal, (CalorieMonitor.Instance smi) => smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null);
		this.hungry.normal.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.working, (CalorieMonitor.Instance smi) => !smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.hungry.starving.Transition(this.hungry.normal, (CalorieMonitor.Instance smi) => !smi.IsStarving(), UpdateRate.SIM_200ms).Transition(this.depleted, (CalorieMonitor.Instance smi) => smi.IsDepleted(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Starving, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.eating.EventTransition(GameHashes.EndChore, this.satisfied, (CalorieMonitor.Instance smi) => !smi.IsEating());
		this.depleted.ToggleTag(GameTags.CaloriesDepleted).Enter(delegate(CalorieMonitor.Instance smi)
		{
			smi.Kill();
		});
	}

	// Token: 0x04003221 RID: 12833
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003222 RID: 12834
	public CalorieMonitor.HungryState hungry;

	// Token: 0x04003223 RID: 12835
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State eating;

	// Token: 0x04003224 RID: 12836
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x04003225 RID: 12837
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State depleted;

	// Token: 0x02001AAB RID: 6827
	public class HungryState : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400826A RID: 33386
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State working;

		// Token: 0x0400826B RID: 33387
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x0400826C RID: 33388
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State starving;
	}

	// Token: 0x02001AAC RID: 6828
	public new class Instance : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A6AA RID: 42666 RVA: 0x003BA8AD File Offset: 0x003B8AAD
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
		}

		// Token: 0x0600A6AB RID: 42667 RVA: 0x003BA8D6 File Offset: 0x003B8AD6
		private float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x0600A6AC RID: 42668 RVA: 0x003BA8EF File Offset: 0x003B8AEF
		public bool IsEatTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
		}

		// Token: 0x0600A6AD RID: 42669 RVA: 0x003BA910 File Offset: 0x003B8B10
		public bool IsHungry()
		{
			return this.GetCalories0to1() < DUPLICANTSTATS.STANDARD.BaseStats.HUNGRY_THRESHOLD;
		}

		// Token: 0x0600A6AE RID: 42670 RVA: 0x003BA929 File Offset: 0x003B8B29
		public bool IsStarving()
		{
			return this.GetCalories0to1() < DUPLICANTSTATS.STANDARD.BaseStats.STARVING_THRESHOLD;
		}

		// Token: 0x0600A6AF RID: 42671 RVA: 0x003BA942 File Offset: 0x003B8B42
		public bool IsSatisfied()
		{
			return this.GetCalories0to1() > DUPLICANTSTATS.STANDARD.BaseStats.SATISFIED_THRESHOLD;
		}

		// Token: 0x0600A6B0 RID: 42672 RVA: 0x003BA95C File Offset: 0x003B8B5C
		public bool IsEating()
		{
			ChoreDriver component = base.master.GetComponent<ChoreDriver>();
			return component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
		}

		// Token: 0x0600A6B1 RID: 42673 RVA: 0x003BA9A0 File Offset: 0x003B8BA0
		public bool IsDepleted()
		{
			return this.calories.value <= 0f;
		}

		// Token: 0x0600A6B2 RID: 42674 RVA: 0x003BA9B7 File Offset: 0x003B8BB7
		public bool ShouldExitInfirmary()
		{
			return !this.IsStarving();
		}

		// Token: 0x0600A6B3 RID: 42675 RVA: 0x003BA9C2 File Offset: 0x003B8BC2
		public void Kill()
		{
			if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
			{
				base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
			}
		}

		// Token: 0x0400826D RID: 33389
		public AmountInstance calories;
	}
}
