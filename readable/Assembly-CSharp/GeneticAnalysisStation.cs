using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000766 RID: 1894
public class GeneticAnalysisStation : GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>
{
	// Token: 0x06002FFF RID: 12287 RVA: 0x00114D3C File Offset: 0x00112F3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.HasSeedToStudy));
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.HasSeedToStudy))).ToggleChore(new Func<GeneticAnalysisStation.StatesInstance, Chore>(this.CreateChore), new Action<GeneticAnalysisStation.StatesInstance, Chore>(GeneticAnalysisStation.SetRemoteChore), this.operational);
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x00114E24 File Offset: 0x00113024
	private static void SetRemoteChore(GeneticAnalysisStation.StatesInstance smi, Chore chore)
	{
		smi.remoteChore.SetChore(chore);
	}

	// Token: 0x06003001 RID: 12289 RVA: 0x00114E32 File Offset: 0x00113032
	private bool HasSeedToStudy(GeneticAnalysisStation.StatesInstance smi)
	{
		return smi.storage.GetMassAvailable(GameTags.UnidentifiedSeed) >= 1f;
	}

	// Token: 0x06003002 RID: 12290 RVA: 0x00114E4E File Offset: 0x0011304E
	private bool IsOperational(GeneticAnalysisStation.StatesInstance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x06003003 RID: 12291 RVA: 0x00114E5C File Offset: 0x0011305C
	private Chore CreateChore(GeneticAnalysisStation.StatesInstance smi)
	{
		return new WorkChore<GeneticAnalysisStationWorkable>(Db.Get().ChoreTypes.AnalyzeSeed, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04001C93 RID: 7315
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State inoperational;

	// Token: 0x04001C94 RID: 7316
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State operational;

	// Token: 0x04001C95 RID: 7317
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State ready;

	// Token: 0x02001654 RID: 5716
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001655 RID: 5717
	public class StatesInstance : GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.GameInstance
	{
		// Token: 0x060096D8 RID: 38616 RVA: 0x00381472 File Offset: 0x0037F672
		public StatesInstance(IStateMachineTarget master, GeneticAnalysisStation.Def def) : base(master, def)
		{
			this.workable.statesInstance = this;
		}

		// Token: 0x060096D9 RID: 38617 RVA: 0x00381488 File Offset: 0x0037F688
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshFetchTags();
		}

		// Token: 0x060096DA RID: 38618 RVA: 0x00381498 File Offset: 0x0037F698
		public void SetSeedForbidden(Tag seedID, bool forbidden)
		{
			if (this.forbiddenSeeds == null)
			{
				this.forbiddenSeeds = new HashSet<Tag>();
			}
			bool flag;
			if (forbidden)
			{
				flag = this.forbiddenSeeds.Add(seedID);
			}
			else
			{
				flag = this.forbiddenSeeds.Remove(seedID);
			}
			if (flag)
			{
				this.RefreshFetchTags();
			}
		}

		// Token: 0x060096DB RID: 38619 RVA: 0x003814E0 File Offset: 0x0037F6E0
		public bool GetSeedForbidden(Tag seedID)
		{
			if (this.forbiddenSeeds == null)
			{
				this.forbiddenSeeds = new HashSet<Tag>();
			}
			return this.forbiddenSeeds.Contains(seedID);
		}

		// Token: 0x060096DC RID: 38620 RVA: 0x00381504 File Offset: 0x0037F704
		private void RefreshFetchTags()
		{
			if (this.forbiddenSeeds == null)
			{
				this.manualDelivery.ForbiddenTags = null;
				return;
			}
			Tag[] array = new Tag[this.forbiddenSeeds.Count];
			int num = 0;
			foreach (Tag tag in this.forbiddenSeeds)
			{
				array[num++] = tag;
				this.storage.Drop(tag);
			}
			this.manualDelivery.ForbiddenTags = array;
		}

		// Token: 0x04007481 RID: 29825
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04007482 RID: 29826
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x04007483 RID: 29827
		[MyCmpReq]
		public GeneticAnalysisStationWorkable workable;

		// Token: 0x04007484 RID: 29828
		[MyCmpAdd]
		public ManuallySetRemoteWorkTargetComponent remoteChore;

		// Token: 0x04007485 RID: 29829
		[Serialize]
		private HashSet<Tag> forbiddenSeeds;
	}
}
