using System;

// Token: 0x020000EE RID: 238
public class DiggerStates : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>
{
	// Token: 0x0600045D RID: 1117 RVA: 0x000240E6 File Offset: 0x000222E6
	private static bool ShouldStopHiding(DiggerStates.Instance smi)
	{
		return !GameplayEventManager.Instance.IsGameplayEventRunningWithTag(GameTags.SpaceDanger);
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x000240FC File Offset: 0x000222FC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.move;
		this.move.MoveTo((DiggerStates.Instance smi) => smi.GetTunnelCell(), this.hide, this.behaviourcomplete, false);
		this.hide.Transition(this.behaviourcomplete, new StateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.Transition.ConditionCallback(DiggerStates.ShouldStopHiding), UpdateRate.SIM_4000ms);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Tunnel, false);
	}

	// Token: 0x04000340 RID: 832
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State move;

	// Token: 0x04000341 RID: 833
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State hide;

	// Token: 0x04000342 RID: 834
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State behaviourcomplete;

	// Token: 0x02001126 RID: 4390
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001127 RID: 4391
	public new class Instance : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.GameInstance
	{
		// Token: 0x060083DB RID: 33755 RVA: 0x003440E3 File Offset: 0x003422E3
		public Instance(Chore<DiggerStates.Instance> chore, DiggerStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Tunnel);
		}

		// Token: 0x060083DC RID: 33756 RVA: 0x00344108 File Offset: 0x00342308
		public int GetTunnelCell()
		{
			DiggerMonitor.Instance smi = base.smi.GetSMI<DiggerMonitor.Instance>();
			if (smi != null)
			{
				return smi.lastDigCell;
			}
			return -1;
		}
	}
}
