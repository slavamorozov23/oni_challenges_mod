using System;

// Token: 0x0200075B RID: 1883
public class FlowerVase : StateMachineComponent<FlowerVase.SMInstance>
{
	// Token: 0x06002FB1 RID: 12209 RVA: 0x0011343C File Offset: 0x0011163C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002FB2 RID: 12210 RVA: 0x00113444 File Offset: 0x00111644
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001C5B RID: 7259
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x04001C5C RID: 7260
	[MyCmpReq]
	private KBoxCollider2D boxCollider;

	// Token: 0x02001643 RID: 5699
	public class SMInstance : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.GameInstance
	{
		// Token: 0x06009679 RID: 38521 RVA: 0x0037FD1E File Offset: 0x0037DF1E
		public SMInstance(FlowerVase master) : base(master)
		{
		}
	}

	// Token: 0x02001644 RID: 5700
	public class States : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase>
	{
		// Token: 0x0600967A RID: 38522 RVA: 0x0037FD28 File Offset: 0x0037DF28
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x04007438 RID: 29752
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State empty;

		// Token: 0x04007439 RID: 29753
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State full;
	}
}
