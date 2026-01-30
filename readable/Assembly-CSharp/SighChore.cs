using System;
using UnityEngine;

// Token: 0x020004BF RID: 1215
public class SighChore : Chore<SighChore.StatesInstance>
{
	// Token: 0x060019A6 RID: 6566 RVA: 0x0008F7C0 File Offset: 0x0008D9C0
	public SighChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Sigh, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SighChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200131E RID: 4894
	public class StatesInstance : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.GameInstance
	{
		// Token: 0x06008AED RID: 35565 RVA: 0x0035BA52 File Offset: 0x00359C52
		public StatesInstance(SighChore master, GameObject sigher) : base(master)
		{
			base.sm.sigher.Set(sigher, base.smi, false);
		}
	}

	// Token: 0x0200131F RID: 4895
	public class States : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore>
	{
		// Token: 0x06008AEE RID: 35566 RVA: 0x0035BA74 File Offset: 0x00359C74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.sigher);
			this.root.PlayAnim("emote_depressed").OnAnimQueueComplete(null);
		}

		// Token: 0x04006A4A RID: 27210
		public StateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.TargetParameter sigher;
	}
}
