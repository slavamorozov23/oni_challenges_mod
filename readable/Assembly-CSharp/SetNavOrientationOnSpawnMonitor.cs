using System;

// Token: 0x020005C9 RID: 1481
public class SetNavOrientationOnSpawnMonitor : GameStateMachine<SetNavOrientationOnSpawnMonitor, SetNavOrientationOnSpawnMonitor.Instance, IStateMachineTarget, SetNavOrientationOnSpawnMonitor.Def>
{
	// Token: 0x060021F4 RID: 8692 RVA: 0x000C50E4 File Offset: 0x000C32E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x0200148E RID: 5262
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200148F RID: 5263
	public new class Instance : GameStateMachine<SetNavOrientationOnSpawnMonitor, SetNavOrientationOnSpawnMonitor.Instance, IStateMachineTarget, SetNavOrientationOnSpawnMonitor.Def>.GameInstance
	{
		// Token: 0x0600903A RID: 36922 RVA: 0x0036DEDC File Offset: 0x0036C0DC
		public Instance(IStateMachineTarget master, SetNavOrientationOnSpawnMonitor.Def def) : base(master, def)
		{
			this.setSpawnOrientationHandler = base.Subscribe(1119167081, new Action<object>(this.SetSpawnOrientation));
		}

		// Token: 0x0600903B RID: 36923 RVA: 0x0036DF0C File Offset: 0x0036C10C
		public void SetSpawnOrientation(object o)
		{
			int cell = Grid.PosToCell(this);
			if (!Grid.IsValidCell(cell))
			{
				return;
			}
			int num = Grid.CellAbove(cell);
			int num2 = Grid.CellBelow(cell);
			if (Grid.IsValidCell(num) && Grid.Solid[num] && (!Grid.IsValidCell(num2) || !Grid.Solid[num2]))
			{
				base.gameObject.GetComponent<Navigator>().CurrentNavType = NavType.Ceiling;
			}
		}

		// Token: 0x0600903C RID: 36924 RVA: 0x0036DF7B File Offset: 0x0036C17B
		protected override void OnCleanUp()
		{
			base.Unsubscribe(ref this.setSpawnOrientationHandler);
			base.OnCleanUp();
		}

		// Token: 0x04006EF0 RID: 28400
		private int setSpawnOrientationHandler = -1;
	}
}
