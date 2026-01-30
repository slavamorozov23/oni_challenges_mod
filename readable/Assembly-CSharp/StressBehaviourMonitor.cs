using System;
using TUNING;

// Token: 0x02000A4C RID: 2636
public class StressBehaviourMonitor : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance>
{
	// Token: 0x06004CD4 RID: 19668 RVA: 0x001BEC68 File Offset: 0x001BCE68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.satisfied.EventTransition(GameHashes.Stressed, this.stressed, (StressBehaviourMonitor.Instance smi) => smi.gameObject.GetSMI<StressMonitor.Instance>() != null && smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed());
		this.stressed.DefaultState(this.stressed.tierOne).ToggleExpression(Db.Get().Expressions.Unhappy, null).ToggleAnims((StressBehaviourMonitor.Instance smi) => smi.tierOneLocoAnim).Transition(this.satisfied, (StressBehaviourMonitor.Instance smi) => smi.gameObject.GetSMI<StressMonitor.Instance>() != null && !smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed(), UpdateRate.SIM_200ms);
		this.stressed.tierOne.DefaultState(this.stressed.tierOne.actingOut).EventTransition(GameHashes.StressedHadEnough, this.stressed.tierTwo, null);
		this.stressed.tierOne.actingOut.ToggleChore((StressBehaviourMonitor.Instance smi) => smi.CreateTierOneStressChore(), this.stressed.tierOne.reprieve);
		this.stressed.tierOne.reprieve.ScheduleGoTo(30f, this.stressed.tierOne.actingOut);
		this.stressed.tierTwo.DefaultState(this.stressed.tierTwo.actingOut).Update(delegate(StressBehaviourMonitor.Instance smi, float dt)
		{
			smi.sm.timeInTierTwoStressResponse.Set(smi.sm.timeInTierTwoStressResponse.Get(smi) + dt, smi, false);
		}, UpdateRate.SIM_200ms, false).Exit("ResetStress", delegate(StressBehaviourMonitor.Instance smi)
		{
			Db.Get().Amounts.Stress.Lookup(smi.gameObject).SetValue(STRESS.ACTING_OUT_RESET);
		});
		this.stressed.tierTwo.actingOut.ToggleChore((StressBehaviourMonitor.Instance smi) => smi.CreateTierTwoStressChore(), this.stressed.tierTwo.reprieve);
		this.stressed.tierTwo.reprieve.ToggleChore((StressBehaviourMonitor.Instance smi) => new StressIdleChore(smi.master), null).Enter(delegate(StressBehaviourMonitor.Instance smi)
		{
			if (smi.sm.timeInTierTwoStressResponse.Get(smi) >= 150f)
			{
				smi.sm.timeInTierTwoStressResponse.Set(0f, smi, false);
				smi.GoTo(this.stressed);
			}
		}).ScheduleGoTo((StressBehaviourMonitor.Instance smi) => smi.tierTwoReprieveDuration, this.stressed.tierTwo);
	}

	// Token: 0x04003329 RID: 13097
	public const float TIER2_STRESS_RESPONSE_TIMEOUT = 150f;

	// Token: 0x0400332A RID: 13098
	public StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeInTierTwoStressResponse;

	// Token: 0x0400332B RID: 13099
	public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x0400332C RID: 13100
	public StressBehaviourMonitor.StressedState stressed;

	// Token: 0x02001B44 RID: 6980
	public class StressedState : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04008448 RID: 33864
		public StressBehaviourMonitor.TierOneStates tierOne;

		// Token: 0x04008449 RID: 33865
		public StressBehaviourMonitor.TierTwoStates tierTwo;
	}

	// Token: 0x02001B45 RID: 6981
	public class TierOneStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400844A RID: 33866
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;

		// Token: 0x0400844B RID: 33867
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
	}

	// Token: 0x02001B46 RID: 6982
	public class TierTwoStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400844C RID: 33868
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;

		// Token: 0x0400844D RID: 33869
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
	}

	// Token: 0x02001B47 RID: 6983
	public new class Instance : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A928 RID: 43304 RVA: 0x003C0BBA File Offset: 0x003BEDBA
		public Instance(IStateMachineTarget master, Func<ChoreProvider, Chore> tier_one_stress_chore_creator, Func<ChoreProvider, Chore> tier_two_stress_chore_creator, string tier_one_loco_anim, float tier_two_reprieve_duration = 3f) : base(master)
		{
			this.tierOneLocoAnim = tier_one_loco_anim;
			this.tierTwoReprieveDuration = tier_two_reprieve_duration;
			this.tierOneStressChoreCreator = tier_one_stress_chore_creator;
			this.tierTwoStressChoreCreator = tier_two_stress_chore_creator;
		}

		// Token: 0x0600A929 RID: 43305 RVA: 0x003C0BEC File Offset: 0x003BEDEC
		public Chore CreateTierOneStressChore()
		{
			return this.tierOneStressChoreCreator(base.GetComponent<ChoreProvider>());
		}

		// Token: 0x0600A92A RID: 43306 RVA: 0x003C0BFF File Offset: 0x003BEDFF
		public Chore CreateTierTwoStressChore()
		{
			return this.tierTwoStressChoreCreator(base.GetComponent<ChoreProvider>());
		}

		// Token: 0x0600A92B RID: 43307 RVA: 0x003C0C12 File Offset: 0x003BEE12
		public void ManualSetStressTier2TimeCounter(float timerValue)
		{
			base.sm.timeInTierTwoStressResponse.Set(timerValue, this, false);
		}

		// Token: 0x0400844E RID: 33870
		public Func<ChoreProvider, Chore> tierOneStressChoreCreator;

		// Token: 0x0400844F RID: 33871
		public Func<ChoreProvider, Chore> tierTwoStressChoreCreator;

		// Token: 0x04008450 RID: 33872
		public string tierOneLocoAnim = "";

		// Token: 0x04008451 RID: 33873
		public float tierTwoReprieveDuration;
	}
}
