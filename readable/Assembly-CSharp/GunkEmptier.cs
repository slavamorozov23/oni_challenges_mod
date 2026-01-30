using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000775 RID: 1909
public class GunkEmptier : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>
{
	// Token: 0x0600308B RID: 12427 RVA: 0x001186DC File Offset: 0x001168DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.noOperational, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational))).DefaultState(this.operational.noStorageSpace);
		this.operational.noStorageSpace.ToggleStatusItem(Db.Get().BuildingStatusItems.GunkEmptierFull, null).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank));
		this.operational.ready.EventTransition(GameHashes.OnStorageChange, this.operational.noStorageSpace, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank))).ToggleRecurringChore(new Func<GunkEmptier.Instance, Chore>(GunkEmptier.CreateChore), null);
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x001187D9 File Offset: 0x001169D9
	public static bool HasSpaceToEmptyABionicGunkTank(GunkEmptier.Instance smi)
	{
		return smi.RemainingStorageCapacity >= GunkMonitor.GUNK_CAPACITY;
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x001187EB File Offset: 0x001169EB
	public static bool IsOperational(GunkEmptier.Instance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x001187F4 File Offset: 0x001169F4
	private static WorkChore<GunkEmptierWorkable> CreateChore(GunkEmptier.Instance smi)
	{
		WorkChore<GunkEmptierWorkable> workChore = new WorkChore<GunkEmptierWorkable>(Db.Get().ChoreTypes.ExpellGunk, smi.master, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
		workChore.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
		return workChore;
	}

	// Token: 0x04001CED RID: 7405
	private static string DISEASE_ID = DUPLICANTSTATS.BIONICS.Secretions.PEE_DISEASE;

	// Token: 0x04001CEE RID: 7406
	private static int DISEASE_ON_DUPE_COUNT_PER_USE = DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE / 20;

	// Token: 0x04001CEF RID: 7407
	public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noOperational;

	// Token: 0x04001CF0 RID: 7408
	public GunkEmptier.OperationalStates operational;

	// Token: 0x0200167E RID: 5758
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200167F RID: 5759
	public class OperationalStates : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State
	{
		// Token: 0x04007516 RID: 29974
		public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noStorageSpace;

		// Token: 0x04007517 RID: 29975
		public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State ready;
	}

	// Token: 0x02001680 RID: 5760
	public new class Instance : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.GameInstance
	{
		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x0600977C RID: 38780 RVA: 0x00384BD7 File Offset: 0x00382DD7
		public float RemainingStorageCapacity
		{
			get
			{
				return this.storage.RemainingCapacity();
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x0600977D RID: 38781 RVA: 0x00384BE4 File Offset: 0x00382DE4
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x0600977E RID: 38782 RVA: 0x00384BF4 File Offset: 0x00382DF4
		public Instance(IStateMachineTarget master, GunkEmptier.Def def) : base(master, def)
		{
			GunkEmptierWorkable component = base.GetComponent<GunkEmptierWorkable>();
			GunkEmptierWorkable gunkEmptierWorkable = component;
			gunkEmptierWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(gunkEmptierWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnGunkEmptierUsed));
			Components.GunkExtractors.Add(component);
			this.storage = base.GetComponent<Storage>();
			this.operational = base.GetComponent<Operational>();
			base.gameObject.AddOrGet<Ownable>().AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_OnlyOnBionics));
		}

		// Token: 0x0600977F RID: 38783 RVA: 0x00384C74 File Offset: 0x00382E74
		protected override void OnCleanUp()
		{
			GunkEmptierWorkable component = base.GetComponent<GunkEmptierWorkable>();
			GunkEmptierWorkable gunkEmptierWorkable = component;
			gunkEmptierWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(gunkEmptierWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnGunkEmptierUsed));
			Components.GunkExtractors.Remove(component);
			base.OnCleanUp();
		}

		// Token: 0x06009780 RID: 38784 RVA: 0x00384CBB File Offset: 0x00382EBB
		private bool AssignablePrecondition_OnlyOnBionics(MinionAssignablesProxy worker)
		{
			return worker.GetMinionModel() == BionicMinionConfig.MODEL;
		}

		// Token: 0x06009781 RID: 38785 RVA: 0x00384CCD File Offset: 0x00382ECD
		public void OnGunkEmptierUsed(Workable workable, Workable.WorkableEvent ev)
		{
			if (ev == Workable.WorkableEvent.WorkCompleted)
			{
				this.AddDisseaseToWorker(workable.worker);
			}
		}

		// Token: 0x06009782 RID: 38786 RVA: 0x00384CE0 File Offset: 0x00382EE0
		public void AddDisseaseToWorker(WorkerBase worker)
		{
			if (worker != null)
			{
				byte index = Db.Get().Diseases.GetIndex(GunkEmptier.DISEASE_ID);
				worker.GetComponent<PrimaryElement>().AddDisease(index, GunkEmptier.DISEASE_ON_DUPE_COUNT_PER_USE, "GunkEmptier.Flush");
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, GunkEmptier.DISEASE_ON_DUPE_COUNT_PER_USE), base.transform, Vector3.up, 1.5f, false, false);
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
				return;
			}
			DebugUtil.LogWarningArgs(new object[]
			{
				"Tried to add disease on gunk emptier use but worker was null"
			});
		}

		// Token: 0x04007518 RID: 29976
		private Operational operational;

		// Token: 0x04007519 RID: 29977
		private Storage storage;
	}
}
