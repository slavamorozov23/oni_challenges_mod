using System;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class MoveChore : Chore<MoveChore.StatesInstance>
{
	// Token: 0x06001972 RID: 6514 RVA: 0x0008E1DC File Offset: 0x0008C3DC
	public MoveChore(IStateMachineTarget target, ChoreType chore_type, Func<MoveChore.StatesInstance, int> get_cell_callback, bool update_cell = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveChore.StatesInstance(this, target.gameObject, get_cell_callback, update_cell);
	}

	// Token: 0x020012F3 RID: 4851
	public class StatesInstance : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.GameInstance
	{
		// Token: 0x06008A37 RID: 35383 RVA: 0x00357F50 File Offset: 0x00356150
		public StatesInstance(MoveChore master, GameObject mover, Func<MoveChore.StatesInstance, int> get_cell_callback, bool update_cell = false) : base(master)
		{
			this.getCellCallback = get_cell_callback;
			base.sm.mover.Set(mover, base.smi, false);
		}

		// Token: 0x040069BE RID: 27070
		public Func<MoveChore.StatesInstance, int> getCellCallback;
	}

	// Token: 0x020012F4 RID: 4852
	public class States : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore>
	{
		// Token: 0x06008A38 RID: 35384 RVA: 0x00357F7C File Offset: 0x0035617C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.mover);
			this.root.MoveTo((MoveChore.StatesInstance smi) => smi.getCellCallback(smi), null, null, false);
		}

		// Token: 0x040069BF RID: 27071
		public GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040069C0 RID: 27072
		public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter mover;

		// Token: 0x040069C1 RID: 27073
		public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter locator;
	}
}
