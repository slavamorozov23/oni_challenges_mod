using System;
using Klei.AI;

// Token: 0x02000A4A RID: 2634
public class StaminaMonitor : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance>
{
	// Token: 0x06004CC7 RID: 19655 RVA: 0x001BE76C File Offset: 0x001BC96C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleStateMachine((StaminaMonitor.Instance smi) => new UrgeMonitor.Instance(smi.master, Db.Get().Urges.Sleep, Db.Get().Amounts.Stamina, Db.Get().ScheduleBlockTypes.Sleep, 100f, 0f, false)).ToggleStateMachine((StaminaMonitor.Instance smi) => new SleepChoreMonitor.Instance(smi.master));
		this.satisfied.Transition(this.sleepy, (StaminaMonitor.Instance smi) => smi.NeedsToSleep() || smi.WantsToSleep(), UpdateRate.SIM_200ms);
		this.sleepy.Update("Check Sleep State", delegate(StaminaMonitor.Instance smi, float dt)
		{
			smi.TryExitSleepState();
		}, UpdateRate.SIM_1000ms, false).DefaultState(this.sleepy.needssleep);
		this.sleepy.needssleep.Transition(this.sleepy.sleeping, (StaminaMonitor.Instance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleStatusItem(Db.Get().DuplicantStatusItems.Tired, null).ToggleThought(Db.Get().Thoughts.Sleepy, null);
		this.sleepy.sleeping.Enter(delegate(StaminaMonitor.Instance smi)
		{
			smi.CheckDebugFastWorkMode();
		}).Transition(this.satisfied, (StaminaMonitor.Instance smi) => !smi.IsSleeping(), UpdateRate.SIM_200ms);
	}

	// Token: 0x0400331F RID: 13087
	public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003320 RID: 13088
	public StaminaMonitor.SleepyState sleepy;

	// Token: 0x04003321 RID: 13089
	private const float OUTSIDE_SCHEDULE_STAMINA_THRESHOLD = 0f;

	// Token: 0x02001B40 RID: 6976
	public class SleepyState : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04008439 RID: 33849
		public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State needssleep;

		// Token: 0x0400843A RID: 33850
		public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State sleeping;
	}

	// Token: 0x02001B41 RID: 6977
	public new class Instance : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A910 RID: 43280 RVA: 0x003C08E8 File Offset: 0x003BEAE8
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.stamina = Db.Get().Amounts.Stamina.Lookup(base.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x0600A911 RID: 43281 RVA: 0x003C0934 File Offset: 0x003BEB34
		public bool NeedsToSleep()
		{
			return this.stamina.value <= 0f;
		}

		// Token: 0x0600A912 RID: 43282 RVA: 0x003C094B File Offset: 0x003BEB4B
		public bool WantsToSleep()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Sleep);
		}

		// Token: 0x0600A913 RID: 43283 RVA: 0x003C097B File Offset: 0x003BEB7B
		public void TryExitSleepState()
		{
			if (!this.NeedsToSleep() && !this.WantsToSleep())
			{
				base.smi.GoTo(base.smi.sm.satisfied);
			}
		}

		// Token: 0x0600A914 RID: 43284 RVA: 0x003C09A8 File Offset: 0x003BEBA8
		public bool IsSleeping()
		{
			bool result = false;
			if (this.WantsToSleep() && this.choreDriver.GetComponent<WorkerBase>().GetWorkable() != null)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600A915 RID: 43285 RVA: 0x003C09DA File Offset: 0x003BEBDA
		public void CheckDebugFastWorkMode()
		{
			if (Game.Instance.FastWorkersModeActive)
			{
				this.stamina.value = this.stamina.GetMax();
			}
		}

		// Token: 0x0600A916 RID: 43286 RVA: 0x003C0A00 File Offset: 0x003BEC00
		public bool ShouldExitSleep()
		{
			if (this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
			{
				return false;
			}
			Narcolepsy component = base.GetComponent<Narcolepsy>();
			return (!(component != null) || !component.IsNarcolepsing()) && this.stamina.value >= this.stamina.GetMax();
		}

		// Token: 0x0400843B RID: 33851
		private ChoreDriver choreDriver;

		// Token: 0x0400843C RID: 33852
		private Schedulable schedulable;

		// Token: 0x0400843D RID: 33853
		public AmountInstance stamina;
	}
}
