using System;

// Token: 0x02000109 RID: 265
public class MinionChoreBrokerStates : GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>
{
	// Token: 0x060004E0 RID: 1248 RVA: 0x00027620 File Offset: 0x00025820
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.hasChore;
		this.root.DoNothing();
		this.hasChore.Enter(delegate(MinionChoreBrokerStates.Instance smi)
		{
		});
	}

	// Token: 0x0400038E RID: 910
	private GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.State hasChore;

	// Token: 0x02001177 RID: 4471
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001178 RID: 4472
	public new class Instance : GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.GameInstance
	{
		// Token: 0x0600849E RID: 33950 RVA: 0x0034559F File Offset: 0x0034379F
		public Instance(Chore<MinionChoreBrokerStates.Instance> chore, MinionChoreBrokerStates.Def def) : base(chore, def)
		{
		}
	}
}
