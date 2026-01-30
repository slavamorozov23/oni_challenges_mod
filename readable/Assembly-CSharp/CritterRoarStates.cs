using System;

// Token: 0x020000E9 RID: 233
public class CritterRoarStates : GameStateMachine<CritterRoarStates, CritterRoarStates.Instance, IStateMachineTarget, CritterRoarStates.Def>
{
	// Token: 0x06000441 RID: 1089 RVA: 0x00023488 File Offset: 0x00021688
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.roar;
		this.roar.PlayAnims((CritterRoarStates.Instance smi) => CritterRoarStates.ANIM_SEQUENCE, KAnim.PlayMode.Once).ScheduleGoTo(10f, this.behaviourComplete).OnAnimQueueComplete(this.behaviourComplete);
		this.behaviourComplete.BehaviourComplete(CritterRoarStates.TAG, false);
	}

	// Token: 0x0400032B RID: 811
	private readonly GameStateMachine<CritterRoarStates, CritterRoarStates.Instance, IStateMachineTarget, CritterRoarStates.Def>.State roar;

	// Token: 0x0400032C RID: 812
	private readonly GameStateMachine<CritterRoarStates, CritterRoarStates.Instance, IStateMachineTarget, CritterRoarStates.Def>.State behaviourComplete;

	// Token: 0x0400032D RID: 813
	private const float FALLBACK_TIMEOUT = 10f;

	// Token: 0x0400032E RID: 814
	private static HashedString ANIM = "roar";

	// Token: 0x0400032F RID: 815
	private static readonly HashedString[] ANIM_SEQUENCE = new HashedString[]
	{
		CritterRoarStates.ANIM
	};

	// Token: 0x04000330 RID: 816
	private static Tag TAG = CritterRoarMonitor.TAG;

	// Token: 0x02001114 RID: 4372
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001115 RID: 4373
	public new class Instance : GameStateMachine<CritterRoarStates, CritterRoarStates.Instance, IStateMachineTarget, CritterRoarStates.Def>.GameInstance
	{
		// Token: 0x060083B6 RID: 33718 RVA: 0x00343D1E File Offset: 0x00341F1E
		public Instance(Chore<CritterRoarStates.Instance> chore, CritterRoarStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, CritterRoarStates.TAG);
		}
	}
}
