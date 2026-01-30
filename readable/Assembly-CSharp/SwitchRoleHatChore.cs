using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public class SwitchRoleHatChore : Chore<SwitchRoleHatChore.StatesInstance>
{
	// Token: 0x060019B7 RID: 6583 RVA: 0x0008FC48 File Offset: 0x0008DE48
	public SwitchRoleHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SwitchRoleHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001329 RID: 4905
	public class StatesInstance : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.GameInstance
	{
		// Token: 0x06008B1D RID: 35613 RVA: 0x0035DB23 File Offset: 0x0035BD23
		public StatesInstance(SwitchRoleHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x0200132A RID: 4906
	public class States : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore>
	{
		// Token: 0x06008B1E RID: 35614 RVA: 0x0035DB48 File Offset: 0x0035BD48
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.start;
			base.Target(this.duplicant);
			this.start.Enter(delegate(SwitchRoleHatChore.StatesInstance smi)
			{
				if (this.duplicant.Get(smi).GetComponent<MinionResume>().CurrentHat == null)
				{
					smi.GoTo(this.delay);
					return;
				}
				smi.GoTo(this.remove_hat);
			});
			this.remove_hat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("hat_off").OnAnimQueueComplete(this.delay);
			this.delay.ToggleThought(Db.Get().Thoughts.NewRole, null).ToggleExpression(Db.Get().Expressions.Happy, null).ToggleAnims("anim_selfish_kanim", 0f).QueueAnim("working_pre", false, null).QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.applyHat_pre);
			this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0f).Enter(delegate(SwitchRoleHatChore.StatesInstance smi)
			{
				this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat();
			}).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
			this.applyHat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x04006A77 RID: 27255
		public StateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.TargetParameter duplicant;

		// Token: 0x04006A78 RID: 27256
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State remove_hat;

		// Token: 0x04006A79 RID: 27257
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State start;

		// Token: 0x04006A7A RID: 27258
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay;

		// Token: 0x04006A7B RID: 27259
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay_pst;

		// Token: 0x04006A7C RID: 27260
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat_pre;

		// Token: 0x04006A7D RID: 27261
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat;

		// Token: 0x04006A7E RID: 27262
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State complete;
	}
}
