using System;

// Token: 0x02000A63 RID: 2659
[SkipSaveFileSerialization]
public class Claustrophobic : StateMachineComponent<Claustrophobic.StatesInstance>
{
	// Token: 0x06004D6A RID: 19818 RVA: 0x001C2892 File Offset: 0x001C0A92
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004D6B RID: 19819 RVA: 0x001C28A0 File Offset: 0x001C0AA0
	protected bool IsUncomfortable()
	{
		int num = 4;
		int cell = Grid.PosToCell(base.gameObject);
		for (int i = 0; i < num - 1; i++)
		{
			int num2 = Grid.OffsetCell(cell, 0, i);
			if (Grid.IsValidCell(num2) && Grid.Solid[num2])
			{
				return true;
			}
			if (Grid.IsValidCell(Grid.CellRight(cell)) && Grid.IsValidCell(Grid.CellLeft(cell)) && Grid.Solid[Grid.CellRight(cell)] && Grid.Solid[Grid.CellLeft(cell)])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x02001B81 RID: 7041
	public class StatesInstance : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.GameInstance
	{
		// Token: 0x0600AA3B RID: 43579 RVA: 0x003C3A33 File Offset: 0x003C1C33
		public StatesInstance(Claustrophobic master) : base(master)
		{
		}
	}

	// Token: 0x02001B82 RID: 7042
	public class States : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic>
	{
		// Token: 0x0600AA3C RID: 43580 RVA: 0x003C3A3C File Offset: 0x003C1C3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("ClaustrophobicCheck", delegate(Claustrophobic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Claustrophobic").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04008539 RID: 34105
		public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State satisfied;

		// Token: 0x0400853A RID: 34106
		public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State suffering;
	}
}
