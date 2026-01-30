using System;

// Token: 0x020005CF RID: 1487
public class WorldSpawnableMonitor : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>
{
	// Token: 0x0600221D RID: 8733 RVA: 0x000C61BC File Offset: 0x000C43BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x020014A2 RID: 5282
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006F18 RID: 28440
		public Func<int, int> adjustSpawnLocationCb;
	}

	// Token: 0x020014A3 RID: 5283
	public new class Instance : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>.GameInstance
	{
		// Token: 0x06009076 RID: 36982 RVA: 0x0036EBB9 File Offset: 0x0036CDB9
		public Instance(IStateMachineTarget master, WorldSpawnableMonitor.Def def) : base(master, def)
		{
		}
	}
}
