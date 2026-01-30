using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000937 RID: 2359
public class EspressoMachine : StateMachineComponent<EspressoMachine.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060041FE RID: 16894 RVA: 0x00174654 File Offset: 0x00172854
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x060041FF RID: 16895 RVA: 0x001746A8 File Offset: 0x001728A8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004200 RID: 16896 RVA: 0x001746B0 File Offset: 0x001728B0
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06004201 RID: 16897 RVA: 0x00174718 File Offset: 0x00172918
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Espresso", true);
		this.AddRequirementDesc(list, EspressoMachine.INGREDIENT_TAG, EspressoMachine.INGREDIENT_MASS_PER_USE);
		this.AddRequirementDesc(list, GameTags.Water, EspressoMachine.WATER_MASS_PER_USE);
		return list;
	}

	// Token: 0x04002935 RID: 10549
	public const string SPECIFIC_EFFECT = "Espresso";

	// Token: 0x04002936 RID: 10550
	public const string TRACKING_EFFECT = "RecentlyRecDrink";

	// Token: 0x04002937 RID: 10551
	public static Tag INGREDIENT_TAG = new Tag("SpiceNut");

	// Token: 0x04002938 RID: 10552
	public static float INGREDIENT_MASS_PER_USE = 1f;

	// Token: 0x04002939 RID: 10553
	public static float WATER_MASS_PER_USE = 1f;

	// Token: 0x0200192F RID: 6447
	public class States : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine>
	{
		// Token: 0x0600A196 RID: 41366 RVA: 0x003AC16C File Offset: 0x003AA36C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<EspressoMachine.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((EspressoMachine.StatesInstance smi) => smi.master.GetComponent<EspressoMachineWorkable>(), this.ready.working).Transition(this.operational, GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Not(new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Not(new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady)));
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((EspressoMachine.StatesInstance smi) => smi.master.GetComponent<EspressoMachineWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x0600A197 RID: 41367 RVA: 0x003AC330 File Offset: 0x003AA530
		private Chore CreateChore(EspressoMachine.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<EspressoMachineWorkable>();
			WorkChore<EspressoMachineWorkable> workChore = new WorkChore<EspressoMachineWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600A198 RID: 41368 RVA: 0x003AC390 File Offset: 0x003AA590
		private bool IsReady(EspressoMachine.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			return !(primaryElement == null) && primaryElement.Mass >= EspressoMachine.WATER_MASS_PER_USE && smi.GetComponent<Storage>().GetAmountAvailable(EspressoMachine.INGREDIENT_TAG) >= EspressoMachine.INGREDIENT_MASS_PER_USE;
		}

		// Token: 0x04007D21 RID: 32033
		private GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State unoperational;

		// Token: 0x04007D22 RID: 32034
		private GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State operational;

		// Token: 0x04007D23 RID: 32035
		private EspressoMachine.States.ReadyStates ready;

		// Token: 0x020029A5 RID: 10661
		public class ReadyStates : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State
		{
			// Token: 0x0400B824 RID: 47140
			public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State idle;

			// Token: 0x0400B825 RID: 47141
			public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State working;

			// Token: 0x0400B826 RID: 47142
			public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State post;
		}
	}

	// Token: 0x02001930 RID: 6448
	public class StatesInstance : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.GameInstance
	{
		// Token: 0x0600A19A RID: 41370 RVA: 0x003AC3EA File Offset: 0x003AA5EA
		public StatesInstance(EspressoMachine smi) : base(smi)
		{
		}
	}
}
