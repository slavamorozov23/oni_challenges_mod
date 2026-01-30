using System;

// Token: 0x020000FD RID: 253
public class HiveGrowthMonitor : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>
{
	// Token: 0x060004A7 RID: 1191 RVA: 0x00026083 File Offset: 0x00024283
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, new StateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.Transition.ConditionCallback(HiveGrowthMonitor.IsGrowing), null);
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x000260AB File Offset: 0x000242AB
	public static bool IsGrowing(HiveGrowthMonitor.Instance smi)
	{
		return !smi.GetSMI<BeeHive.StatesInstance>().IsFullyGrown();
	}

	// Token: 0x02001157 RID: 4439
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001158 RID: 4440
	public new class Instance : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.GameInstance
	{
		// Token: 0x0600844C RID: 33868 RVA: 0x00344C4B File Offset: 0x00342E4B
		public Instance(IStateMachineTarget master, HiveGrowthMonitor.Def def) : base(master, def)
		{
		}
	}
}
