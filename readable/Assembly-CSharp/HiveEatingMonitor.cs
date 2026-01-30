using System;

// Token: 0x020000FB RID: 251
public class HiveEatingMonitor : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>
{
	// Token: 0x060004A1 RID: 1185 RVA: 0x00025EF5 File Offset: 0x000240F5
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToEat, new StateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.Transition.ConditionCallback(HiveEatingMonitor.ShouldEat), null);
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00025F1D File Offset: 0x0002411D
	public static bool ShouldEat(HiveEatingMonitor.Instance smi)
	{
		return smi.storage.FindFirst(smi.def.consumedOre) != null;
	}

	// Token: 0x02001151 RID: 4433
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400646B RID: 25707
		public Tag consumedOre;
	}

	// Token: 0x02001152 RID: 4434
	public new class Instance : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.GameInstance
	{
		// Token: 0x06008442 RID: 33858 RVA: 0x00344BBA File Offset: 0x00342DBA
		public Instance(IStateMachineTarget master, HiveEatingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0400646C RID: 25708
		[MyCmpReq]
		public Storage storage;
	}
}
