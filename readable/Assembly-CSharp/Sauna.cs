using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000B25 RID: 2853
public class Sauna : StateMachineComponent<Sauna.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600535D RID: 21341 RVA: 0x001E5D7C File Offset: 0x001E3F7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600535E RID: 21342 RVA: 0x001E5D8F File Offset: 0x001E3F8F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600535F RID: 21343 RVA: 0x001E5D98 File Offset: 0x001E3F98
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06005360 RID: 21344 RVA: 0x001E5E00 File Offset: 0x001E4000
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Steam);
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, element.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, element.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement, false));
		Element element2 = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, element2.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, element2.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Effect, false));
		list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false));
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		return list;
	}

	// Token: 0x0400386E RID: 14446
	public string specificEffect;

	// Token: 0x0400386F RID: 14447
	public string trackingEffect;

	// Token: 0x04003870 RID: 14448
	public float steamPerUseKG;

	// Token: 0x04003871 RID: 14449
	public float waterOutputTemp;

	// Token: 0x04003872 RID: 14450
	public static readonly Operational.Flag sufficientSteam = new Operational.Flag("sufficientSteam", Operational.Flag.Type.Requirement);

	// Token: 0x02001C73 RID: 7283
	public class States : GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna>
	{
		// Token: 0x0600ADB3 RID: 44467 RVA: 0x003D0FE8 File Offset: 0x003CF1E8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.operational.TagTransition(GameTags.Operational, this.inoperational, true).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GettingReady, null).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<Sauna.StatesInstance, Chore>(this.CreateChore), this.inoperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.WorkableStartTransition((Sauna.StatesInstance smi) => smi.master.GetComponent<SaunaWorkable>(), this.ready.working).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.Not(new StateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.Transition.ConditionCallback(this.IsReady)));
			this.ready.working.WorkableCompleteTransition((Sauna.StatesInstance smi) => smi.master.GetComponent<SaunaWorkable>(), this.ready.idle).WorkableStopTransition((Sauna.StatesInstance smi) => smi.master.GetComponent<SaunaWorkable>(), this.ready.idle);
		}

		// Token: 0x0600ADB4 RID: 44468 RVA: 0x003D1198 File Offset: 0x003CF398
		private Chore CreateChore(Sauna.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<SaunaWorkable>();
			WorkChore<SaunaWorkable> workChore = new WorkChore<SaunaWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600ADB5 RID: 44469 RVA: 0x003D11F8 File Offset: 0x003CF3F8
		private bool IsReady(Sauna.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Steam);
			return primaryElement != null && primaryElement.Mass >= smi.master.steamPerUseKG;
		}

		// Token: 0x04008826 RID: 34854
		private GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State inoperational;

		// Token: 0x04008827 RID: 34855
		private GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State operational;

		// Token: 0x04008828 RID: 34856
		private Sauna.States.ReadyStates ready;

		// Token: 0x02002A1D RID: 10781
		public class ReadyStates : GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State
		{
			// Token: 0x0400BA2F RID: 47663
			public GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State idle;

			// Token: 0x0400BA30 RID: 47664
			public GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State working;
		}
	}

	// Token: 0x02001C74 RID: 7284
	public class StatesInstance : GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.GameInstance
	{
		// Token: 0x0600ADB7 RID: 44471 RVA: 0x003D123F File Offset: 0x003CF43F
		public StatesInstance(Sauna smi) : base(smi)
		{
		}
	}
}
