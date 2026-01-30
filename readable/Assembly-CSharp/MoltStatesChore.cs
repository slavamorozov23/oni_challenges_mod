using System;

// Token: 0x0200010B RID: 267
public class MoltStatesChore : GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>
{
	// Token: 0x060004E6 RID: 1254 RVA: 0x000277C0 File Offset: 0x000259C0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.molting;
		this.molting.PlayAnim((MoltStatesChore.Instance smi) => smi.def.moltAnimName, KAnim.PlayMode.Once).ScheduleGoTo(5f, this.complete).OnAnimQueueComplete(this.complete);
		this.complete.BehaviourComplete(GameTags.Creatures.ReadyToMolt, false);
	}

	// Token: 0x04000392 RID: 914
	public GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.State molting;

	// Token: 0x04000393 RID: 915
	public GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.State complete;

	// Token: 0x0200117C RID: 4476
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040064CC RID: 25804
		public string moltAnimName;
	}

	// Token: 0x0200117D RID: 4477
	public new class Instance : GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.GameInstance
	{
		// Token: 0x060084A5 RID: 33957 RVA: 0x003455F3 File Offset: 0x003437F3
		public Instance(Chore<MoltStatesChore.Instance> chore, MoltStatesChore.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.ReadyToMolt);
		}
	}
}
