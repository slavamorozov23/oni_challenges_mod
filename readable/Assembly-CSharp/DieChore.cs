using System;

// Token: 0x0200049F RID: 1183
public class DieChore : Chore<DieChore.StatesInstance>
{
	// Token: 0x06001914 RID: 6420 RVA: 0x0008BEE8 File Offset: 0x0008A0E8
	public DieChore(IStateMachineTarget master, Death death) : base(Db.Get().ChoreTypes.Die, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new DieChore.StatesInstance(this, death);
	}

	// Token: 0x020012CE RID: 4814
	public class StatesInstance : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.GameInstance
	{
		// Token: 0x060089AD RID: 35245 RVA: 0x00353D7C File Offset: 0x00351F7C
		public StatesInstance(DieChore master, Death death) : base(master)
		{
			base.sm.death.Set(death, base.smi, false);
		}

		// Token: 0x060089AE RID: 35246 RVA: 0x00353DA0 File Offset: 0x00351FA0
		public void PlayPreAnim()
		{
			string preAnim = base.sm.death.Get(base.smi).preAnim;
			base.GetComponent<KAnimControllerBase>().Play(preAnim, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x020012CF RID: 4815
	public class States : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore>
	{
		// Token: 0x060089AF RID: 35247 RVA: 0x00353DE8 File Offset: 0x00351FE8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.dying;
			this.dying.OnAnimQueueComplete(this.dead).Enter("PlayAnim", delegate(DieChore.StatesInstance smi)
			{
				smi.PlayPreAnim();
			});
			this.dead.ReturnSuccess();
		}

		// Token: 0x04006932 RID: 26930
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dying;

		// Token: 0x04006933 RID: 26931
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dead;

		// Token: 0x04006934 RID: 26932
		public StateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.ResourceParameter<Death> death;
	}
}
