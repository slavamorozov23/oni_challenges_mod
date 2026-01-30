using System;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
public class TakeOffHatChore : Chore<TakeOffHatChore.StatesInstance>
{
	// Token: 0x060019BB RID: 6587 RVA: 0x0008FE40 File Offset: 0x0008E040
	public TakeOffHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new TakeOffHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200132E RID: 4910
	public class StatesInstance : GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.GameInstance
	{
		// Token: 0x06008B2B RID: 35627 RVA: 0x0035DE48 File Offset: 0x0035C048
		public StatesInstance(TakeOffHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x0200132F RID: 4911
	public class States : GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore>
	{
		// Token: 0x06008B2C RID: 35628 RVA: 0x0035DE6C File Offset: 0x0035C06C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.remove_hat_pre;
			base.Target(this.duplicant);
			this.remove_hat_pre.Enter(delegate(TakeOffHatChore.StatesInstance smi)
			{
				if (this.duplicant.Get(smi).GetComponent<MinionResume>().CurrentHat != null)
				{
					smi.GoTo(this.remove_hat);
					return;
				}
				smi.GoTo(this.complete);
			});
			this.remove_hat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("hat_off").OnAnimQueueComplete(this.complete);
			this.complete.Enter(delegate(TakeOffHatChore.StatesInstance smi)
			{
				smi.master.GetComponent<MinionResume>().RemoveHat();
			}).ReturnSuccess();
		}

		// Token: 0x04006A87 RID: 27271
		public StateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.TargetParameter duplicant;

		// Token: 0x04006A88 RID: 27272
		public GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.State remove_hat_pre;

		// Token: 0x04006A89 RID: 27273
		public GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.State remove_hat;

		// Token: 0x04006A8A RID: 27274
		public GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.State complete;
	}
}
