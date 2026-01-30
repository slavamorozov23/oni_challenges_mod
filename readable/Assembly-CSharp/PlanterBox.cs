using System;

// Token: 0x020007DE RID: 2014
[SkipSaveFileSerialization]
public class PlanterBox : StateMachineComponent<PlanterBox.SMInstance>
{
	// Token: 0x0600358A RID: 13706 RVA: 0x0012E86A File Offset: 0x0012CA6A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0400206C RID: 8300
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x02001737 RID: 5943
	public class SMInstance : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.GameInstance
	{
		// Token: 0x06009A58 RID: 39512 RVA: 0x00390830 File Offset: 0x0038EA30
		public SMInstance(PlanterBox master) : base(master)
		{
		}
	}

	// Token: 0x02001738 RID: 5944
	public class States : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox>
	{
		// Token: 0x06009A59 RID: 39513 RVA: 0x0039083C File Offset: 0x0038EA3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x04007717 RID: 30487
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State empty;

		// Token: 0x04007718 RID: 30488
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State full;
	}
}
