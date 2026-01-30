using System;

// Token: 0x02000A68 RID: 2664
[SkipSaveFileSerialization]
public class SensitiveFeet : StateMachineComponent<SensitiveFeet.StatesInstance>
{
	// Token: 0x06004D78 RID: 19832 RVA: 0x001C2A82 File Offset: 0x001C0C82
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004D79 RID: 19833 RVA: 0x001C2A90 File Offset: 0x001C0C90
	protected bool IsUncomfortable()
	{
		int num = Grid.CellBelow(Grid.PosToCell(base.gameObject));
		return Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Objects[num, 9] == null;
	}

	// Token: 0x02001B8B RID: 7051
	public class StatesInstance : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.GameInstance
	{
		// Token: 0x0600AA50 RID: 43600 RVA: 0x003C3D3C File Offset: 0x003C1F3C
		public StatesInstance(SensitiveFeet master) : base(master)
		{
		}
	}

	// Token: 0x02001B8C RID: 7052
	public class States : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet>
	{
		// Token: 0x0600AA51 RID: 43601 RVA: 0x003C3D48 File Offset: 0x003C1F48
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("SensitiveFeetCheck", delegate(SensitiveFeet.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("UncomfortableFeet").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04008541 RID: 34113
		public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State satisfied;

		// Token: 0x04008542 RID: 34114
		public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State suffering;
	}
}
