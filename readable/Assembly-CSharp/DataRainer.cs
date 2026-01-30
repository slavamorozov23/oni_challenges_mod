using System;
using TUNING;

// Token: 0x020004E6 RID: 1254
public class DataRainer : GameStateMachine<DataRainer, DataRainer.Instance>
{
	// Token: 0x06001B1B RID: 6939 RVA: 0x00094D40 File Offset: 0x00092F40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>(this.databanksCreated, this.overjoyed.exitEarly, (DataRainer.Instance smi, int p) => p >= TRAITS.JOY_REACTIONS.DATA_RAINER.NUM_MICROCHIPS).Exit(delegate(DataRainer.Instance smi)
		{
			this.databanksCreated.Set(0, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(DataRainer.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.raining);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerPlanning, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.raining, (DataRainer.Instance smi) => smi.IsRecTime());
		this.overjoyed.raining.ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerRaining, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.idle, (DataRainer.Instance smi) => !smi.IsRecTime()).ToggleChore((DataRainer.Instance smi) => new DataRainerChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(DataRainer.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x04000F97 RID: 3991
	public StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.IntParameter databanksCreated;

	// Token: 0x04000F98 RID: 3992
	public static float databankSpawnInterval = 1.8f;

	// Token: 0x04000F99 RID: 3993
	public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000F9A RID: 3994
	public DataRainer.OverjoyedStates overjoyed;

	// Token: 0x02001370 RID: 4976
	public class OverjoyedStates : GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B2B RID: 27435
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006B2C RID: 27436
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State raining;

		// Token: 0x04006B2D RID: 27437
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x02001371 RID: 4977
	public new class Instance : GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008BE0 RID: 35808 RVA: 0x00360114 File Offset: 0x0035E314
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008BE1 RID: 35809 RVA: 0x0036011D File Offset: 0x0035E31D
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06008BE2 RID: 35810 RVA: 0x00360140 File Offset: 0x0035E340
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}
	}
}
