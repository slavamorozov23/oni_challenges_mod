using System;

// Token: 0x02000891 RID: 2193
public class CreatureSleepMonitor : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>
{
	// Token: 0x06003C5C RID: 15452 RVA: 0x00151BDE File Offset: 0x0014FDDE
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.SleepBehaviour, new StateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.Transition.ConditionCallback(CreatureSleepMonitor.ShouldSleep), null);
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x00151C06 File Offset: 0x0014FE06
	public static bool ShouldSleep(CreatureSleepMonitor.Instance smi)
	{
		return GameClock.Instance.IsNighttime();
	}

	// Token: 0x02001865 RID: 6245
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001866 RID: 6246
	public new class Instance : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.GameInstance
	{
		// Token: 0x06009EC8 RID: 40648 RVA: 0x003A4126 File Offset: 0x003A2326
		public Instance(IStateMachineTarget master, CreatureSleepMonitor.Def def) : base(master, def)
		{
		}
	}
}
