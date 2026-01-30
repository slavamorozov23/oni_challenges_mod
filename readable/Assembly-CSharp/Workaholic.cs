using System;

// Token: 0x02000A6A RID: 2666
[SkipSaveFileSerialization]
public class Workaholic : StateMachineComponent<Workaholic.StatesInstance>
{
	// Token: 0x06004D7E RID: 19838 RVA: 0x001C2BF4 File Offset: 0x001C0DF4
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004D7F RID: 19839 RVA: 0x001C2C01 File Offset: 0x001C0E01
	protected bool IsUncomfortable()
	{
		return base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore() is IdleChore;
	}

	// Token: 0x02001B8F RID: 7055
	public class StatesInstance : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.GameInstance
	{
		// Token: 0x0600AA59 RID: 43609 RVA: 0x003C3EF3 File Offset: 0x003C20F3
		public StatesInstance(Workaholic master) : base(master)
		{
		}
	}

	// Token: 0x02001B90 RID: 7056
	public class States : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic>
	{
		// Token: 0x0600AA5A RID: 43610 RVA: 0x003C3EFC File Offset: 0x003C20FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("WorkaholicCheck", delegate(Workaholic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Restless").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04008545 RID: 34117
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State satisfied;

		// Token: 0x04008546 RID: 34118
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State suffering;
	}
}
