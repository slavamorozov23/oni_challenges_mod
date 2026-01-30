using System;

// Token: 0x02000756 RID: 1878
public class FarmTile : StateMachineComponent<FarmTile.SMInstance>
{
	// Token: 0x06002F7F RID: 12159 RVA: 0x001127B3 File Offset: 0x001109B3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001C3D RID: 7229
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x04001C3E RID: 7230
	[MyCmpReq]
	private Storage storage;

	// Token: 0x02001639 RID: 5689
	public class SMInstance : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.GameInstance
	{
		// Token: 0x0600965F RID: 38495 RVA: 0x0037F40A File Offset: 0x0037D60A
		public SMInstance(FarmTile master) : base(master)
		{
		}

		// Token: 0x06009660 RID: 38496 RVA: 0x0037F414 File Offset: 0x0037D614
		public bool HasWater()
		{
			PrimaryElement primaryElement = base.master.storage.FindPrimaryElement(SimHashes.Water);
			return primaryElement != null && primaryElement.Mass > 0f;
		}
	}

	// Token: 0x0200163A RID: 5690
	public class States : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile>
	{
		// Token: 0x06009661 RID: 38497 RVA: 0x0037F450 File Offset: 0x0037D650
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant != null);
			this.empty.wet.EventTransition(GameHashes.OnStorageChange, this.empty.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.empty.dry.EventTransition(GameHashes.OnStorageChange, this.empty.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant == null);
			this.full.wet.EventTransition(GameHashes.OnStorageChange, this.full.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.dry.EventTransition(GameHashes.OnStorageChange, this.full.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
		}

		// Token: 0x0400741D RID: 29725
		public FarmTile.States.FarmStates empty;

		// Token: 0x0400741E RID: 29726
		public FarmTile.States.FarmStates full;

		// Token: 0x020028DF RID: 10463
		public class FarmStates : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State
		{
			// Token: 0x0400B44F RID: 46159
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State wet;

			// Token: 0x0400B450 RID: 46160
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State dry;
		}
	}
}
