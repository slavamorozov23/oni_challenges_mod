using System;
using UnityEngine;

// Token: 0x020004C2 RID: 1218
public class StressIdleChore : Chore<StressIdleChore.StatesInstance>
{
	// Token: 0x060019B1 RID: 6577 RVA: 0x0008FB10 File Offset: 0x0008DD10
	public StressIdleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.StressIdle, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StressIdleChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001325 RID: 4901
	public class StatesInstance : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.GameInstance
	{
		// Token: 0x06008B0B RID: 35595 RVA: 0x0035C7DA File Offset: 0x0035A9DA
		public StatesInstance(StressIdleChore master, GameObject idler) : base(master)
		{
			base.sm.idler.Set(idler, base.smi, false);
		}
	}

	// Token: 0x02001326 RID: 4902
	public class States : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore>
	{
		// Token: 0x06008B0C RID: 35596 RVA: 0x0035C7FC File Offset: 0x0035A9FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.Target(this.idler);
			this.idle.PlayAnim("idle_default", KAnim.PlayMode.Loop);
		}

		// Token: 0x04006A65 RID: 27237
		public StateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.TargetParameter idler;

		// Token: 0x04006A66 RID: 27238
		public GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.State idle;
	}
}
