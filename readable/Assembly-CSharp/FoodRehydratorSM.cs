using System;

// Token: 0x0200075F RID: 1887
public class FoodRehydratorSM : GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>
{
	// Token: 0x06002FC7 RID: 12231 RVA: 0x00113E1C File Offset: 0x0011201C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EnterTransition(this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(this.on, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsFunctional);
		this.off.PlayAnim("off", KAnim.PlayMode.Loop).EnterTransition(this.on, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsFunctional).EventTransition(GameHashes.FunctionalChanged, this.on, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsFunctional);
		this.on.PlayAnim("on", KAnim.PlayMode.Loop).EnterTransition(this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(this.active, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsActive).EventTransition(GameHashes.FunctionalChanged, this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EventTransition(GameHashes.ActiveChanged, this.active, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsActive);
		this.active.EnterTransition(this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(this.on, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsActive).EventTransition(GameHashes.FunctionalChanged, this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EventTransition(GameHashes.ActiveChanged, this.postactive, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsActive);
		this.postactive.OnAnimQueueComplete(this.on);
	}

	// Token: 0x04001C77 RID: 7287
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State off;

	// Token: 0x04001C78 RID: 7288
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State on;

	// Token: 0x04001C79 RID: 7289
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State active;

	// Token: 0x04001C7A RID: 7290
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State postactive;

	// Token: 0x0200164A RID: 5706
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200164B RID: 5707
	public class StatesInstance : GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.GameInstance
	{
		// Token: 0x060096AC RID: 38572 RVA: 0x00380DA1 File Offset: 0x0037EFA1
		public StatesInstance(IStateMachineTarget master, FoodRehydratorSM.Def def) : base(master, def)
		{
		}

		// Token: 0x04007459 RID: 29785
		[MyCmpReq]
		public Operational operational;
	}
}
