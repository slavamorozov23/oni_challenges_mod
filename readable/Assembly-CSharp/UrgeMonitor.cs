using System;
using Klei.AI;

// Token: 0x02000A56 RID: 2646
public class UrgeMonitor : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance>
{
	// Token: 0x06004CFB RID: 19707 RVA: 0x001BFEE0 File Offset: 0x001BE0E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.hasurge, (UrgeMonitor.Instance smi) => smi.HasUrge(), UpdateRate.SIM_200ms);
		this.hasurge.Transition(this.satisfied, (UrgeMonitor.Instance smi) => !smi.HasUrge(), UpdateRate.SIM_200ms).ToggleUrge((UrgeMonitor.Instance smi) => smi.GetUrge());
	}

	// Token: 0x0400334A RID: 13130
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x0400334B RID: 13131
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State hasurge;

	// Token: 0x02001B63 RID: 7011
	public new class Instance : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A9BC RID: 43452 RVA: 0x003C273C File Offset: 0x003C093C
		public Instance(IStateMachineTarget master, Urge urge, Amount amount, ScheduleBlockType schedule_block, float in_schedule_threshold, float out_of_schedule_threshold, bool is_threshold_minimum) : base(master)
		{
			this.urge = urge;
			this.scheduleBlock = schedule_block;
			this.schedulable = base.GetComponent<Schedulable>();
			this.amountInstance = base.gameObject.GetAmounts().Get(amount);
			this.isThresholdMinimum = is_threshold_minimum;
			this.inScheduleThreshold = in_schedule_threshold;
			this.outOfScheduleThreshold = out_of_schedule_threshold;
		}

		// Token: 0x0600A9BD RID: 43453 RVA: 0x003C279A File Offset: 0x003C099A
		private float GetThreshold()
		{
			if (this.schedulable.IsAllowed(this.scheduleBlock))
			{
				return this.inScheduleThreshold;
			}
			return this.outOfScheduleThreshold;
		}

		// Token: 0x0600A9BE RID: 43454 RVA: 0x003C27BC File Offset: 0x003C09BC
		public Urge GetUrge()
		{
			return this.urge;
		}

		// Token: 0x0600A9BF RID: 43455 RVA: 0x003C27C4 File Offset: 0x003C09C4
		public bool HasUrge()
		{
			if (this.isThresholdMinimum)
			{
				return this.amountInstance.value >= this.GetThreshold();
			}
			return this.amountInstance.value <= this.GetThreshold();
		}

		// Token: 0x040084C3 RID: 33987
		private AmountInstance amountInstance;

		// Token: 0x040084C4 RID: 33988
		private Urge urge;

		// Token: 0x040084C5 RID: 33989
		private ScheduleBlockType scheduleBlock;

		// Token: 0x040084C6 RID: 33990
		private Schedulable schedulable;

		// Token: 0x040084C7 RID: 33991
		private float inScheduleThreshold;

		// Token: 0x040084C8 RID: 33992
		private float outOfScheduleThreshold;

		// Token: 0x040084C9 RID: 33993
		private bool isThresholdMinimum;
	}
}
