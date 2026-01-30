using System;
using TUNING;

// Token: 0x020004E8 RID: 1256
public class RoboDancer : GameStateMachine<RoboDancer, RoboDancer.Instance>
{
	// Token: 0x06001B23 RID: 6947 RVA: 0x00095118 File Offset: 0x00093318
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<float>(this.timeSpentDancing, this.overjoyed.exitEarly, (RoboDancer.Instance smi, float p) => p >= TRAITS.JOY_REACTIONS.ROBO_DANCER.DANCE_DURATION && !this.hasAudience.Get(smi)).Exit(delegate(RoboDancer.Instance smi)
		{
			this.timeSpentDancing.Set(0f, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(RoboDancer.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.dancing);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerPlanning, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.dancing, (RoboDancer.Instance smi) => smi.IsRecTime());
		this.overjoyed.dancing.ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerDancing, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.idle, (RoboDancer.Instance smi) => !smi.IsRecTime()).ToggleChore((RoboDancer.Instance smi) => new RoboDancerChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(RoboDancer.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x04000F9F RID: 3999
	public StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.FloatParameter timeSpentDancing;

	// Token: 0x04000FA0 RID: 4000
	public StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.BoolParameter hasAudience;

	// Token: 0x04000FA1 RID: 4001
	public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000FA2 RID: 4002
	public RoboDancer.OverjoyedStates overjoyed;

	// Token: 0x02001376 RID: 4982
	public class OverjoyedStates : GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B3F RID: 27455
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006B40 RID: 27456
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State dancing;

		// Token: 0x04006B41 RID: 27457
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x02001377 RID: 4983
	public new class Instance : GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008BFB RID: 35835 RVA: 0x003603BB File Offset: 0x0035E5BB
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008BFC RID: 35836 RVA: 0x003603C4 File Offset: 0x0035E5C4
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06008BFD RID: 35837 RVA: 0x003603E8 File Offset: 0x0035E5E8
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}
	}
}
