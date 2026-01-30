using System;

// Token: 0x02000BDB RID: 3035
public class Splat : GameStateMachine<Splat, Splat.StatesInstance>
{
	// Token: 0x06005AEB RID: 23275 RVA: 0x0020F108 File Offset: 0x0020D308
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleChore((Splat.StatesInstance smi) => new WorkChore<SplatWorkable>(Db.Get().ChoreTypes.Mop, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), this.complete);
		this.complete.Enter(delegate(Splat.StatesInstance smi)
		{
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x04003C92 RID: 15506
	public GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.State complete;

	// Token: 0x02001D6A RID: 7530
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001D6B RID: 7531
	public class StatesInstance : GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600B126 RID: 45350 RVA: 0x003DC6FD File Offset: 0x003DA8FD
		public StatesInstance(IStateMachineTarget master, Splat.Def def) : base(master, def)
		{
		}
	}
}
