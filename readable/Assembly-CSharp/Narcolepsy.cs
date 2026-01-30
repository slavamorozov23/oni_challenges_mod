using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A61 RID: 2657
[SkipSaveFileSerialization]
public class Narcolepsy : StateMachineComponent<Narcolepsy.StatesInstance>
{
	// Token: 0x06004D5B RID: 19803 RVA: 0x001C26F2 File Offset: 0x001C08F2
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004D5C RID: 19804 RVA: 0x001C26FF File Offset: 0x001C08FF
	public bool IsNarcolepsing()
	{
		return base.smi.IsNarcolepsing();
	}

	// Token: 0x040033A3 RID: 13219
	public static readonly Chore.Precondition IsNarcolepsingPrecondition = new Chore.Precondition
	{
		id = "IsNarcolepsingPrecondition",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NARCOLEPSING,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
			return component != null && component.IsNarcolepsing();
		}
	};

	// Token: 0x02001B7D RID: 7037
	public class StatesInstance : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.GameInstance
	{
		// Token: 0x0600AA2D RID: 43565 RVA: 0x003C3824 File Offset: 0x003C1A24
		public StatesInstance(Narcolepsy master) : base(master)
		{
		}

		// Token: 0x0600AA2E RID: 43566 RVA: 0x003C3830 File Offset: 0x003C1A30
		public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x0600AA2F RID: 43567 RVA: 0x003C3854 File Offset: 0x003C1A54
		public bool IsNarcolepsing()
		{
			return this.GetCurrentState() == base.sm.sleepy;
		}

		// Token: 0x0600AA30 RID: 43568 RVA: 0x003C3869 File Offset: 0x003C1A69
		public GameObject CreateFloorLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "NarcolepticSleep";
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}
	}

	// Token: 0x02001B7E RID: 7038
	public class States : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy>
	{
		// Token: 0x0600AA31 RID: 43569 RVA: 0x003C3894 File Offset: 0x003C1A94
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Enter("ScheduleNextSleep", delegate(Narcolepsy.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_INTERVAL_MIN, TRAITS.NARCOLEPSY_INTERVAL_MAX), this.sleepy);
			});
			this.sleepy.Enter("Is Already Sleeping Check", delegate(Narcolepsy.StatesInstance smi)
			{
				if (smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping())
				{
					smi.GoTo(this.idle);
					return;
				}
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_SLEEPDURATION_MIN, TRAITS.NARCOLEPSY_SLEEPDURATION_MAX), this.idle);
			}).ToggleUrge(Db.Get().Urges.Narcolepsy).ToggleChore(new Func<Narcolepsy.StatesInstance, Chore>(this.CreateNarcolepsyChore), this.idle);
		}

		// Token: 0x0600AA32 RID: 43570 RVA: 0x003C3924 File Offset: 0x003C1B24
		private Chore CreateNarcolepsyChore(Narcolepsy.StatesInstance smi)
		{
			GameObject bed = smi.CreateFloorLocator();
			SleepChore sleepChore = new SleepChore(Db.Get().ChoreTypes.Narcolepsy, smi.master, bed, true, false);
			sleepChore.AddPrecondition(Narcolepsy.IsNarcolepsingPrecondition, null);
			return sleepChore;
		}

		// Token: 0x0600AA33 RID: 43571 RVA: 0x003C3961 File Offset: 0x003C1B61
		private float GetNewInterval(float min, float max)
		{
			Mathf.Min(Mathf.Max(Util.GaussianRandom(max - min, 1f), min), max);
			return UnityEngine.Random.Range(min, max);
		}

		// Token: 0x04008533 RID: 34099
		public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State idle;

		// Token: 0x04008534 RID: 34100
		public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State sleepy;
	}
}
