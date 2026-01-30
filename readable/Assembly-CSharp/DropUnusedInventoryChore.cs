using System;
using UnityEngine;

// Token: 0x02000540 RID: 1344
public class DropUnusedInventoryChore : Chore<DropUnusedInventoryChore.StatesInstance>
{
	// Token: 0x06001D14 RID: 7444 RVA: 0x0009E52C File Offset: 0x0009C72C
	public DropUnusedInventoryChore(ChoreType chore_type, IStateMachineTarget target) : base(chore_type, target, target.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new DropUnusedInventoryChore.StatesInstance(this);
	}

	// Token: 0x020013E0 RID: 5088
	public class StatesInstance : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.GameInstance
	{
		// Token: 0x06008E2F RID: 36399 RVA: 0x00368203 File Offset: 0x00366403
		public StatesInstance(DropUnusedInventoryChore master) : base(master)
		{
		}
	}

	// Token: 0x020013E1 RID: 5089
	public class States : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore>
	{
		// Token: 0x06008E30 RID: 36400 RVA: 0x0036820C File Offset: 0x0036640C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.dropping;
			this.dropping.Enter(delegate(DropUnusedInventoryChore.StatesInstance smi)
			{
				smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
			}).GoTo(this.success);
			this.success.ReturnSuccess();
		}

		// Token: 0x04006C90 RID: 27792
		public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State dropping;

		// Token: 0x04006C91 RID: 27793
		public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State success;
	}
}
