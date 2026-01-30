using System;

// Token: 0x020000DD RID: 221
public class BeeHiveMonitor : GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>
{
	// Token: 0x06000400 RID: 1024 RVA: 0x00021A60 File Offset: 0x0001FC60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.Nighttime, (BeeHiveMonitor.Instance smi) => GameClock.Instance, this.night, (BeeHiveMonitor.Instance smi) => GameClock.Instance.IsNighttime());
		this.night.EventTransition(GameHashes.NewDay, (BeeHiveMonitor.Instance smi) => GameClock.Instance, this.idle, (BeeHiveMonitor.Instance smi) => !GameClock.Instance.IsNighttime()).ToggleBehaviour(GameTags.Creatures.WantsToMakeHome, new StateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.Transition.ConditionCallback(this.ShouldMakeHome), null);
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x00021B36 File Offset: 0x0001FD36
	public bool ShouldMakeHome(BeeHiveMonitor.Instance smi)
	{
		return !this.CanGoHome(smi);
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00021B42 File Offset: 0x0001FD42
	public bool CanGoHome(BeeHiveMonitor.Instance smi)
	{
		return smi.gameObject.GetComponent<Bee>().FindHiveInRoom() != null;
	}

	// Token: 0x04000307 RID: 775
	public GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.State idle;

	// Token: 0x04000308 RID: 776
	public GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.State night;

	// Token: 0x020010EB RID: 4331
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010EC RID: 4332
	public new class Instance : GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.GameInstance
	{
		// Token: 0x06008348 RID: 33608 RVA: 0x0034335F File Offset: 0x0034155F
		public Instance(IStateMachineTarget master, BeeHiveMonitor.Def def) : base(master, def)
		{
		}
	}
}
