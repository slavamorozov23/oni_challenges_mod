using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000709 RID: 1801
public class ArtifactAnalysisStation : GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>
{
	// Token: 0x06002C99 RID: 11417 RVA: 0x001038F0 File Offset: 0x00101AF0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.HasArtifactToStudy));
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.HasArtifactToStudy))).ToggleChore(new Func<ArtifactAnalysisStation.StatesInstance, Chore>(this.CreateChore), new Action<ArtifactAnalysisStation.StatesInstance, Chore>(ArtifactAnalysisStation.SetRemoteChore), this.operational);
	}

	// Token: 0x06002C9A RID: 11418 RVA: 0x001039D8 File Offset: 0x00101BD8
	private static void SetRemoteChore(ArtifactAnalysisStation.StatesInstance smi, Chore chore)
	{
		smi.remoteChore.SetChore(chore);
	}

	// Token: 0x06002C9B RID: 11419 RVA: 0x001039E6 File Offset: 0x00101BE6
	private bool HasArtifactToStudy(ArtifactAnalysisStation.StatesInstance smi)
	{
		return smi.storage.GetMassAvailable(GameTags.CharmedArtifact) >= 1f;
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x00103A02 File Offset: 0x00101C02
	private bool IsOperational(ArtifactAnalysisStation.StatesInstance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x00103A10 File Offset: 0x00101C10
	private Chore CreateChore(ArtifactAnalysisStation.StatesInstance smi)
	{
		return new WorkChore<ArtifactAnalysisStationWorkable>(Db.Get().ChoreTypes.AnalyzeArtifact, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04001A85 RID: 6789
	public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State inoperational;

	// Token: 0x04001A86 RID: 6790
	public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State operational;

	// Token: 0x04001A87 RID: 6791
	public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State ready;

	// Token: 0x020015CC RID: 5580
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015CD RID: 5581
	public class StatesInstance : GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.GameInstance
	{
		// Token: 0x060094A4 RID: 38052 RVA: 0x00379D65 File Offset: 0x00377F65
		public StatesInstance(IStateMachineTarget master, ArtifactAnalysisStation.Def def) : base(master, def)
		{
			this.workable.statesInstance = this;
		}

		// Token: 0x060094A5 RID: 38053 RVA: 0x00379D7B File Offset: 0x00377F7B
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x040072BF RID: 29375
		[MyCmpReq]
		public Storage storage;

		// Token: 0x040072C0 RID: 29376
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x040072C1 RID: 29377
		[MyCmpReq]
		public ArtifactAnalysisStationWorkable workable;

		// Token: 0x040072C2 RID: 29378
		[MyCmpAdd]
		public ManuallySetRemoteWorkTargetComponent remoteChore;

		// Token: 0x040072C3 RID: 29379
		[Serialize]
		private HashSet<Tag> forbiddenSeeds;
	}
}
