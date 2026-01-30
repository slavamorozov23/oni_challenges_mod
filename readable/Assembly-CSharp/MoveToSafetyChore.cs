using System;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class MoveToSafetyChore : Chore<MoveToSafetyChore.StatesInstance>
{
	// Token: 0x0600197A RID: 6522 RVA: 0x0008E660 File Offset: 0x0008C860
	public MoveToSafetyChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.MoveToSafety, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveToSafetyChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x020012FB RID: 4859
	public class StatesInstance : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.GameInstance
	{
		// Token: 0x06008A51 RID: 35409 RVA: 0x00358744 File Offset: 0x00356944
		public StatesInstance(MoveToSafetyChore master, GameObject mover) : base(master)
		{
			base.sm.mover.Set(mover, base.smi, false);
			this.sensor = base.sm.mover.Get<Sensors>(base.smi).GetSensor<SafeCellSensor>();
			this.targetCell = this.sensor.GetSensorCell();
		}

		// Token: 0x06008A52 RID: 35410 RVA: 0x003587A3 File Offset: 0x003569A3
		public void UpdateTargetCell()
		{
			this.targetCell = this.sensor.GetSensorCell();
		}

		// Token: 0x040069D5 RID: 27093
		private SafeCellSensor sensor;

		// Token: 0x040069D6 RID: 27094
		public int targetCell;
	}

	// Token: 0x020012FC RID: 4860
	public class States : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore>
	{
		// Token: 0x06008A53 RID: 35411 RVA: 0x003587B8 File Offset: 0x003569B8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.move;
			base.Target(this.mover);
			this.root.ToggleTag(GameTags.Idle);
			this.move.Enter("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi)
			{
				smi.UpdateTargetCell();
			}).Update("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi, float dt)
			{
				smi.UpdateTargetCell();
			}, UpdateRate.SIM_200ms, false).MoveTo((MoveToSafetyChore.StatesInstance smi) => smi.targetCell, null, null, true);
		}

		// Token: 0x040069D7 RID: 27095
		public StateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.TargetParameter mover;

		// Token: 0x040069D8 RID: 27096
		public GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.State move;
	}
}
