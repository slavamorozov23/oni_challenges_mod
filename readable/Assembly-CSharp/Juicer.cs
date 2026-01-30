using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020009D4 RID: 2516
public class Juicer : StateMachineComponent<Juicer.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600490C RID: 18700 RVA: 0x001A6ACC File Offset: 0x001A4CCC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x0600490D RID: 18701 RVA: 0x001A6B20 File Offset: 0x001A4D20
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600490E RID: 18702 RVA: 0x001A6B28 File Offset: 0x001A4D28
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		string arg2 = (EdiblesManager.GetFoodInfo(tag.Name) != null) ? GameUtil.GetFormattedCaloriesForItem(tag, mass, GameUtil.TimeSlice.None, true) : GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, arg2), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, arg2), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x0600490F RID: 18703 RVA: 0x001A6BA0 File Offset: 0x001A4DA0
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		for (int i = 0; i < this.ingredientTags.Length; i++)
		{
			this.AddRequirementDesc(list, this.ingredientTags[i], this.ingredientMassesPerUse[i]);
		}
		this.AddRequirementDesc(list, GameTags.Water, this.waterMassPerUse);
		return list;
	}

	// Token: 0x0400308E RID: 12430
	public string specificEffect;

	// Token: 0x0400308F RID: 12431
	public string trackingEffect;

	// Token: 0x04003090 RID: 12432
	public Tag[] ingredientTags;

	// Token: 0x04003091 RID: 12433
	public float[] ingredientMassesPerUse;

	// Token: 0x04003092 RID: 12434
	public float waterMassPerUse;

	// Token: 0x02001A30 RID: 6704
	public class States : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer>
	{
		// Token: 0x0600A47E RID: 42110 RVA: 0x003B4788 File Offset: 0x003B2988
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<Juicer.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.Transition(this.operational, GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Not(new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Not(new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady))).PlayAnim("on").WorkableStartTransition((Juicer.StatesInstance smi) => smi.master.GetComponent<JuicerWorkable>(), this.ready.working);
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((Juicer.StatesInstance smi) => smi.master.GetComponent<JuicerWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x0600A47F RID: 42111 RVA: 0x003B494C File Offset: 0x003B2B4C
		private Chore CreateChore(Juicer.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<JuicerWorkable>();
			WorkChore<JuicerWorkable> workChore = new WorkChore<JuicerWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600A480 RID: 42112 RVA: 0x003B49AC File Offset: 0x003B2BAC
		private bool IsReady(Juicer.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			if (primaryElement == null)
			{
				return false;
			}
			if (primaryElement.Mass < smi.master.waterMassPerUse)
			{
				return false;
			}
			for (int i = 0; i < smi.master.ingredientTags.Length; i++)
			{
				if (smi.GetComponent<Storage>().GetAmountAvailable(smi.master.ingredientTags[i]) < smi.master.ingredientMassesPerUse[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040080B1 RID: 32945
		private GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State unoperational;

		// Token: 0x040080B2 RID: 32946
		private GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State operational;

		// Token: 0x040080B3 RID: 32947
		private Juicer.States.ReadyStates ready;

		// Token: 0x020029C8 RID: 10696
		public class ReadyStates : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State
		{
			// Token: 0x0400B8DB RID: 47323
			public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State idle;

			// Token: 0x0400B8DC RID: 47324
			public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State working;

			// Token: 0x0400B8DD RID: 47325
			public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State post;
		}
	}

	// Token: 0x02001A31 RID: 6705
	public class StatesInstance : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.GameInstance
	{
		// Token: 0x0600A482 RID: 42114 RVA: 0x003B4A38 File Offset: 0x003B2C38
		public StatesInstance(Juicer smi) : base(smi)
		{
		}
	}
}
