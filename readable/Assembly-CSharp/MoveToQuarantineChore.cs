using System;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class MoveToQuarantineChore : Chore<MoveToQuarantineChore.StatesInstance>
{
	// Token: 0x06001979 RID: 6521 RVA: 0x0008E5F4 File Offset: 0x0008C7F4
	public MoveToQuarantineChore(IStateMachineTarget target, KMonoBehaviour quarantine_area) : base(Db.Get().ChoreTypes.MoveToQuarantine, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveToQuarantineChore.StatesInstance(this, target.gameObject);
		base.smi.sm.locator.Set(quarantine_area.gameObject, base.smi, false);
	}

	// Token: 0x020012F9 RID: 4857
	public class StatesInstance : GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.GameInstance
	{
		// Token: 0x06008A4E RID: 35406 RVA: 0x003586E3 File Offset: 0x003568E3
		public StatesInstance(MoveToQuarantineChore master, GameObject quarantined) : base(master)
		{
			base.sm.quarantined.Set(quarantined, base.smi, false);
		}
	}

	// Token: 0x020012FA RID: 4858
	public class States : GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore>
	{
		// Token: 0x06008A4F RID: 35407 RVA: 0x00358705 File Offset: 0x00356905
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			this.approach.InitializeStates(this.quarantined, this.locator, this.success, null, null, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x040069D1 RID: 27089
		public StateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.TargetParameter locator;

		// Token: 0x040069D2 RID: 27090
		public StateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.TargetParameter quarantined;

		// Token: 0x040069D3 RID: 27091
		public GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040069D4 RID: 27092
		public GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.State success;
	}
}
