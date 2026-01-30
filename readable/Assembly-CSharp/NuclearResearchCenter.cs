using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A72 RID: 2674
public class NuclearResearchCenter : StateMachineComponent<NuclearResearchCenter.StatesInstance>, IResearchCenter, IGameObjectEffectDescriptor
{
	// Token: 0x06004DBA RID: 19898 RVA: 0x001C38B8 File Offset: 0x001C1AB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.ResearchCenters.Add(this);
		this.particleMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.particleMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target"
		});
		base.Subscribe<NuclearResearchCenter>(-1837862626, NuclearResearchCenter.OnStorageChangeDelegate);
		this.RefreshMeter();
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x06004DBB RID: 19899 RVA: 0x001C3937 File Offset: 0x001C1B37
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.ResearchCenters.Remove(this);
	}

	// Token: 0x06004DBC RID: 19900 RVA: 0x001C394A File Offset: 0x001C1B4A
	public string GetResearchType()
	{
		return this.researchTypeID;
	}

	// Token: 0x06004DBD RID: 19901 RVA: 0x001C3952 File Offset: 0x001C1B52
	private void OnStorageChange(object data)
	{
		this.RefreshMeter();
	}

	// Token: 0x06004DBE RID: 19902 RVA: 0x001C395C File Offset: 0x001C1B5C
	private void RefreshMeter()
	{
		float positionPercent = Mathf.Clamp01(this.particleStorage.Particles / this.particleStorage.Capacity());
		this.particleMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06004DBF RID: 19903 RVA: 0x001C3994 File Offset: 0x001C1B94
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.materialPerPoint, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.materialPerPoint, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Requirement, false),
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.researchTypeID).name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.researchTypeID).name), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x040033C8 RID: 13256
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040033C9 RID: 13257
	public string researchTypeID;

	// Token: 0x040033CA RID: 13258
	public float materialPerPoint = 50f;

	// Token: 0x040033CB RID: 13259
	public float timePerPoint;

	// Token: 0x040033CC RID: 13260
	public Tag inputMaterial;

	// Token: 0x040033CD RID: 13261
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x040033CE RID: 13262
	public Meter.Offset particleMeterOffset;

	// Token: 0x040033CF RID: 13263
	private MeterController particleMeter;

	// Token: 0x040033D0 RID: 13264
	private static readonly EventSystem.IntraObjectHandler<NuclearResearchCenter> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<NuclearResearchCenter>(delegate(NuclearResearchCenter component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001B94 RID: 7060
	public class States : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter>
	{
		// Token: 0x0600AA65 RID: 43621 RVA: 0x003C4110 File Offset: 0x003C2310
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.requirements, false);
			this.requirements.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.requirements.highEnergyParticlesNeeded);
			this.requirements.highEnergyParticlesNeeded.ToggleMainStatusItem(Db.Get().BuildingStatusItems.WaitingForHighEnergyParticles, null).EventTransition(GameHashes.OnParticleStorageChanged, this.requirements.noResearchSelected, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsReady));
			this.requirements.noResearchSelected.Enter(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				this.UpdateNoResearchSelectedStatusItem(smi, true);
			}).Exit(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				this.UpdateNoResearchSelectedStatusItem(smi, false);
			}).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noApplicableResearch, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected));
			this.requirements.noApplicableResearch.EventTransition(GameHashes.ActiveResearchChanged, this.ready, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable)).EventTransition(GameHashes.ActiveResearchChanged, this.requirements, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected)));
			this.ready.Enter(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.CreateChore();
			}).TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).Exit(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.DestroyChore();
			}).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noResearchSelected, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected))).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noApplicableResearch, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable))).EventTransition(GameHashes.ResearchPointsChanged, this.requirements.noApplicableResearch, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable))).EventTransition(GameHashes.OnParticleStorageEmpty, this.requirements.highEnergyParticlesNeeded, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.HasRadiation)));
			this.ready.idle.WorkableStartTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.working);
			this.ready.working.Enter("SetActive(true)", delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).WorkableStopTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.idle).WorkableCompleteTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.idle);
		}

		// Token: 0x0600AA66 RID: 43622 RVA: 0x003C4454 File Offset: 0x003C2654
		protected bool IsAllResearchComplete()
		{
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600AA67 RID: 43623 RVA: 0x003C44B8 File Offset: 0x003C26B8
		private void UpdateNoResearchSelectedStatusItem(NuclearResearchCenter.StatesInstance smi, bool entering)
		{
			bool flag = entering && !this.IsResearchSelected(smi) && !this.IsAllResearchComplete();
			KSelectable component = smi.GetComponent<KSelectable>();
			if (flag)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NoResearchSelected, null);
				return;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
		}

		// Token: 0x0600AA68 RID: 43624 RVA: 0x003C4524 File Offset: 0x003C2724
		private bool IsReady(NuclearResearchCenter.StatesInstance smi)
		{
			return smi.GetComponent<HighEnergyParticleStorage>().Particles > smi.master.materialPerPoint;
		}

		// Token: 0x0600AA69 RID: 43625 RVA: 0x003C453E File Offset: 0x003C273E
		private bool IsResearchSelected(NuclearResearchCenter.StatesInstance smi)
		{
			return Research.Instance.GetActiveResearch() != null;
		}

		// Token: 0x0600AA6A RID: 43626 RVA: 0x003C4550 File Offset: 0x003C2750
		private bool IsResearchApplicable(NuclearResearchCenter.StatesInstance smi)
		{
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			if (activeResearch != null && activeResearch.tech.costsByResearchTypeID.ContainsKey(smi.master.researchTypeID))
			{
				float num = activeResearch.progressInventory.PointsByTypeID[smi.master.researchTypeID];
				float num2 = activeResearch.tech.costsByResearchTypeID[smi.master.researchTypeID];
				return num < num2;
			}
			return false;
		}

		// Token: 0x0600AA6B RID: 43627 RVA: 0x003C45C4 File Offset: 0x003C27C4
		private bool HasRadiation(NuclearResearchCenter.StatesInstance smi)
		{
			return !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty();
		}

		// Token: 0x0400854A RID: 34122
		public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State inoperational;

		// Token: 0x0400854B RID: 34123
		public NuclearResearchCenter.States.RequirementsState requirements;

		// Token: 0x0400854C RID: 34124
		public NuclearResearchCenter.States.ReadyState ready;

		// Token: 0x020029E6 RID: 10726
		public class RequirementsState : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State
		{
			// Token: 0x0400B92D RID: 47405
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State highEnergyParticlesNeeded;

			// Token: 0x0400B92E RID: 47406
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State noResearchSelected;

			// Token: 0x0400B92F RID: 47407
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State noApplicableResearch;
		}

		// Token: 0x020029E7 RID: 10727
		public class ReadyState : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State
		{
			// Token: 0x0400B930 RID: 47408
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State idle;

			// Token: 0x0400B931 RID: 47409
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State working;
		}
	}

	// Token: 0x02001B95 RID: 7061
	public class StatesInstance : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.GameInstance
	{
		// Token: 0x0600AA6F RID: 43631 RVA: 0x003C45F0 File Offset: 0x003C27F0
		public StatesInstance(NuclearResearchCenter master) : base(master)
		{
		}

		// Token: 0x0600AA70 RID: 43632 RVA: 0x003C45FC File Offset: 0x003C27FC
		public void CreateChore()
		{
			Workable component = base.smi.master.GetComponent<NuclearResearchCenterWorkable>();
			this.chore = new WorkChore<NuclearResearchCenterWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.preemption_cb = new Func<Chore.Precondition.Context, bool>(NuclearResearchCenter.StatesInstance.CanPreemptCB);
		}

		// Token: 0x0600AA71 RID: 43633 RVA: 0x003C465D File Offset: 0x003C285D
		public void DestroyChore()
		{
			this.chore.Cancel("destroy me!");
			this.chore = null;
		}

		// Token: 0x0600AA72 RID: 43634 RVA: 0x003C4678 File Offset: 0x003C2878
		private static bool CanPreemptCB(Chore.Precondition.Context context)
		{
			WorkerBase component = context.chore.driver.GetComponent<WorkerBase>();
			float num = Db.Get().AttributeConverters.ResearchSpeed.Lookup(component).Evaluate();
			WorkerBase worker = context.consumerState.worker;
			float num2 = Db.Get().AttributeConverters.ResearchSpeed.Lookup(worker).Evaluate();
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			if (activeResearch != null)
			{
				NuclearResearchCenter.StatesInstance smi = context.chore.gameObject.GetSMI<NuclearResearchCenter.StatesInstance>();
				if (smi != null)
				{
					return num2 > num && activeResearch.PercentageCompleteResearchType(smi.master.researchTypeID) < 1f;
				}
			}
			return false;
		}

		// Token: 0x0400854D RID: 34125
		private WorkChore<NuclearResearchCenterWorkable> chore;
	}
}
