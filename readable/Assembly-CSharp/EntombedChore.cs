using System;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class EntombedChore : Chore<EntombedChore.StatesInstance>
{
	// Token: 0x06001925 RID: 6437 RVA: 0x0008C43C File Offset: 0x0008A63C
	public EntombedChore(IStateMachineTarget target, string entombedAnimOverride) : base(Db.Get().ChoreTypes.Entombed, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EntombedChore.StatesInstance(this, target.gameObject, entombedAnimOverride);
	}

	// Token: 0x020012D5 RID: 4821
	public class StatesInstance : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.GameInstance
	{
		// Token: 0x060089CD RID: 35277 RVA: 0x00354A7A File Offset: 0x00352C7A
		public StatesInstance(EntombedChore master, GameObject entombable, string entombedAnimOverride) : base(master)
		{
			base.sm.entombable.Set(entombable, base.smi, false);
			this.entombedAnimOverride = entombedAnimOverride;
		}

		// Token: 0x060089CE RID: 35278 RVA: 0x00354AA4 File Offset: 0x00352CA4
		public void UpdateFaceEntombed()
		{
			int num = Grid.CellAbove(Grid.PosToCell(base.transform.GetPosition()));
			base.sm.isFaceEntombed.Set(Grid.IsValidCell(num) && Grid.Solid[num], base.smi, false);
		}

		// Token: 0x0400694A RID: 26954
		public string entombedAnimOverride;
	}

	// Token: 0x020012D6 RID: 4822
	public class States : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore>
	{
		// Token: 0x060089CF RID: 35279 RVA: 0x00354AF8 File Offset: 0x00352CF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.entombedbody;
			base.Target(this.entombable);
			this.root.ToggleAnims((EntombedChore.StatesInstance smi) => smi.entombedAnimOverride).Update("IsFaceEntombed", delegate(EntombedChore.StatesInstance smi, float dt)
			{
				smi.UpdateFaceEntombed();
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.EntombedChore, null);
			this.entombedface.PlayAnim("entombed_ceiling", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isFaceEntombed, this.entombedbody, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsFalse);
			this.entombedbody.PlayAnim("entombed_floor", KAnim.PlayMode.Loop).StopMoving().ParamTransition<bool>(this.isFaceEntombed, this.entombedface, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsTrue);
		}

		// Token: 0x0400694B RID: 26955
		public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.BoolParameter isFaceEntombed;

		// Token: 0x0400694C RID: 26956
		public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.TargetParameter entombable;

		// Token: 0x0400694D RID: 26957
		public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedface;

		// Token: 0x0400694E RID: 26958
		public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedbody;
	}
}
