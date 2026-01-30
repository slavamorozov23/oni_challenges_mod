using System;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class PutOnHatChore : Chore<PutOnHatChore.StatesInstance>
{
	// Token: 0x06001980 RID: 6528 RVA: 0x0008E850 File Offset: 0x0008CA50
	public PutOnHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new PutOnHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001301 RID: 4865
	public class StatesInstance : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.GameInstance
	{
		// Token: 0x06008A5F RID: 35423 RVA: 0x00358C75 File Offset: 0x00356E75
		public StatesInstance(PutOnHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x02001302 RID: 4866
	public class States : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore>
	{
		// Token: 0x06008A60 RID: 35424 RVA: 0x00358C98 File Offset: 0x00356E98
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.applyHat_pre;
			base.Target(this.duplicant);
			this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0f).Enter(delegate(PutOnHatChore.StatesInstance smi)
			{
				this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat();
			}).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
			this.applyHat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x040069E4 RID: 27108
		public StateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.TargetParameter duplicant;

		// Token: 0x040069E5 RID: 27109
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat_pre;

		// Token: 0x040069E6 RID: 27110
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat;

		// Token: 0x040069E7 RID: 27111
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State complete;
	}
}
