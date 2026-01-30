using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000B58 RID: 2904
public class SodaFountain : StateMachineComponent<SodaFountain.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060055CA RID: 21962 RVA: 0x001F4524 File Offset: 0x001F2724
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x060055CB RID: 21963 RVA: 0x001F4578 File Offset: 0x001F2778
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060055CC RID: 21964 RVA: 0x001F4580 File Offset: 0x001F2780
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x060055CD RID: 21965 RVA: 0x001F45E8 File Offset: 0x001F27E8
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		this.AddRequirementDesc(list, this.ingredientTag, this.ingredientMassPerUse);
		this.AddRequirementDesc(list, GameTags.Water, this.waterMassPerUse);
		return list;
	}

	// Token: 0x040039E9 RID: 14825
	public string specificEffect;

	// Token: 0x040039EA RID: 14826
	public string trackingEffect;

	// Token: 0x040039EB RID: 14827
	public Tag ingredientTag;

	// Token: 0x040039EC RID: 14828
	public float ingredientMassPerUse;

	// Token: 0x040039ED RID: 14829
	public float waterMassPerUse;

	// Token: 0x02001CBF RID: 7359
	public class States : GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain>
	{
		// Token: 0x0600AE78 RID: 44664 RVA: 0x003D3908 File Offset: 0x003D1B08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<SodaFountain.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.Transition(this.operational, GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Not(new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Not(new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady))).WorkableStartTransition((SodaFountain.StatesInstance smi) => smi.master.GetComponent<SodaFountainWorkable>(), this.ready.working);
			this.ready.working.PlayAnim("working_pre").WorkableStopTransition((SodaFountain.StatesInstance smi) => smi.master.GetComponent<SodaFountainWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x0600AE79 RID: 44665 RVA: 0x003D3AB4 File Offset: 0x003D1CB4
		private Chore CreateChore(SodaFountain.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<SodaFountainWorkable>();
			WorkChore<SodaFountainWorkable> workChore = new WorkChore<SodaFountainWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600AE7A RID: 44666 RVA: 0x003D3B14 File Offset: 0x003D1D14
		private bool IsReady(SodaFountain.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			return !(primaryElement == null) && primaryElement.Mass >= smi.master.waterMassPerUse && smi.GetComponent<Storage>().GetAmountAvailable(smi.master.ingredientTag) >= smi.master.ingredientMassPerUse;
		}

		// Token: 0x04008914 RID: 35092
		private GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State unoperational;

		// Token: 0x04008915 RID: 35093
		private GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State operational;

		// Token: 0x04008916 RID: 35094
		private SodaFountain.States.ReadyStates ready;

		// Token: 0x02002A28 RID: 10792
		public class ReadyStates : GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State
		{
			// Token: 0x0400BA54 RID: 47700
			public GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State idle;

			// Token: 0x0400BA55 RID: 47701
			public GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State working;

			// Token: 0x0400BA56 RID: 47702
			public GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State post;
		}
	}

	// Token: 0x02001CC0 RID: 7360
	public class StatesInstance : GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.GameInstance
	{
		// Token: 0x0600AE7C RID: 44668 RVA: 0x003D3B80 File Offset: 0x003D1D80
		public StatesInstance(SodaFountain smi) : base(smi)
		{
		}
	}
}
