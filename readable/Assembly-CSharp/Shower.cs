using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020007F7 RID: 2039
[AddComponentMenu("KMonoBehaviour/Workable/Shower")]
public class Shower : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06003690 RID: 13968 RVA: 0x00133CC9 File Offset: 0x00131EC9
	private Shower()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06003691 RID: 13969 RVA: 0x00133CD9 File Offset: 0x00131ED9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetProgressOnStop = true;
		this.smi = new Shower.ShowerSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06003692 RID: 13970 RVA: 0x00133D00 File Offset: 0x00131F00
	protected override void OnStartWork(WorkerBase worker)
	{
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		base.WorkTimeRemaining = this.workTime * instance.GetDirtiness();
		this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
		this.smi.SetActive(true);
		base.OnStartWork(worker);
	}

	// Token: 0x06003693 RID: 13971 RVA: 0x00133D45 File Offset: 0x00131F45
	protected override void OnStopWork(WorkerBase worker)
	{
		this.smi.SetActive(false);
	}

	// Token: 0x06003694 RID: 13972 RVA: 0x00133D54 File Offset: 0x00131F54
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
		{
			string effect_id = Shower.EffectsRemoved[i];
			component.Remove(effect_id);
		}
		if (!worker.HasTag(GameTags.HasSuitTank))
		{
			GasLiquidExposureMonitor.Instance instance = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
			if (instance != null)
			{
				instance.ResetExposure();
			}
		}
		component.Add(Shower.SHOWER_EFFECT, true);
		HygieneMonitor.Instance instance2 = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance2 != null)
		{
			instance2.SetDirtiness(0f);
		}
	}

	// Token: 0x06003695 RID: 13973 RVA: 0x00133DD4 File Offset: 0x00131FD4
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		PrimaryElement component = worker.GetComponent<PrimaryElement>();
		if (component.DiseaseCount > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = new SimUtil.DiseaseInfo
			{
				idx = component.DiseaseIdx,
				count = Mathf.CeilToInt((float)component.DiseaseCount * (1f - Mathf.Pow(this.fractionalDiseaseRemoval, dt)) - (float)this.absoluteDiseaseRemoval)
			};
			component.ModifyDiseaseCount(-diseaseInfo.count, "Shower.RemoveDisease");
			this.accumulatedDisease = SimUtil.CalculateFinalDiseaseInfo(this.accumulatedDisease, diseaseInfo);
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.outputTargetElement);
			if (primaryElement != null)
			{
				primaryElement.GetComponent<PrimaryElement>().AddDisease(this.accumulatedDisease.idx, this.accumulatedDisease.count, "Shower.RemoveDisease");
				this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
			}
		}
		return false;
	}

	// Token: 0x06003696 RID: 13974 RVA: 0x00133EAC File Offset: 0x001320AC
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance != null)
		{
			instance.SetDirtiness(1f - this.GetPercentComplete());
		}
	}

	// Token: 0x06003697 RID: 13975 RVA: 0x00133EDC File Offset: 0x001320DC
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (Shower.EffectsRemoved.Length != 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			descriptors.Add(item);
			for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
			{
				string text = Shower.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item2 = default(Descriptor);
				item2.IncreaseIndent();
				item2.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				descriptors.Add(item2);
			}
		}
		Effect.AddModifierDescriptions(base.gameObject, descriptors, Shower.SHOWER_EFFECT, true);
		return descriptors;
	}

	// Token: 0x04002120 RID: 8480
	private Shower.ShowerSM.Instance smi;

	// Token: 0x04002121 RID: 8481
	public static string SHOWER_EFFECT = "Showered";

	// Token: 0x04002122 RID: 8482
	public SimHashes outputTargetElement;

	// Token: 0x04002123 RID: 8483
	public float fractionalDiseaseRemoval;

	// Token: 0x04002124 RID: 8484
	public int absoluteDiseaseRemoval;

	// Token: 0x04002125 RID: 8485
	private SimUtil.DiseaseInfo accumulatedDisease;

	// Token: 0x04002126 RID: 8486
	public const float WATER_PER_USE = 5f;

	// Token: 0x04002127 RID: 8487
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoakingWet",
		"WetFeet",
		"MinorIrritation",
		"MajorIrritation"
	};

	// Token: 0x0200176E RID: 5998
	public class ShowerSM : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower>
	{
		// Token: 0x06009B35 RID: 39733 RVA: 0x00394094 File Offset: 0x00392294
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.root.Update(new Action<Shower.ShowerSM.Instance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (Shower.ShowerSM.Instance smi) => smi.IsOperational).PlayAnim("off");
			this.operational.DefaultState(this.operational.not_ready).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Shower.ShowerSM.Instance smi) => !smi.IsOperational);
			this.operational.not_ready.EventTransition(GameHashes.OnStorageChange, this.operational.ready, (Shower.ShowerSM.Instance smi) => smi.IsReady()).PlayAnim("off");
			this.operational.ready.ToggleChore(new Func<Shower.ShowerSM.Instance, Chore>(this.CreateShowerChore), this.operational.not_ready);
		}

		// Token: 0x06009B36 RID: 39734 RVA: 0x003941BC File Offset: 0x003923BC
		private Chore CreateShowerChore(Shower.ShowerSM.Instance smi)
		{
			WorkChore<Shower> workChore = new WorkChore<Shower>(Db.Get().ChoreTypes.Shower, smi.master, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Hygiene, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.IsNotABionic, smi);
			return workChore;
		}

		// Token: 0x06009B37 RID: 39735 RVA: 0x00394214 File Offset: 0x00392414
		private void UpdateStatusItems(Shower.ShowerSM.Instance smi, float dt)
		{
			if (smi.OutputFull())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
		}

		// Token: 0x040077BC RID: 30652
		public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State unoperational;

		// Token: 0x040077BD RID: 30653
		public Shower.ShowerSM.OperationalState operational;

		// Token: 0x0200293B RID: 10555
		public class OperationalState : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State
		{
			// Token: 0x0400B66D RID: 46701
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State not_ready;

			// Token: 0x0400B66E RID: 46702
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State ready;
		}

		// Token: 0x0200293C RID: 10556
		public new class Instance : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.GameInstance
		{
			// Token: 0x0600D000 RID: 53248 RVA: 0x00434BB4 File Offset: 0x00432DB4
			public Instance(Shower master) : base(master)
			{
				this.operational = master.GetComponent<Operational>();
				this.consumer = master.GetComponent<ConduitConsumer>();
				this.dispenser = master.GetComponent<ConduitDispenser>();
			}

			// Token: 0x17000D6F RID: 3439
			// (get) Token: 0x0600D001 RID: 53249 RVA: 0x00434BE1 File Offset: 0x00432DE1
			public bool IsOperational
			{
				get
				{
					return this.operational.IsOperational && this.consumer.IsConnected && this.dispenser.IsConnected;
				}
			}

			// Token: 0x0600D002 RID: 53250 RVA: 0x00434C0A File Offset: 0x00432E0A
			public void SetActive(bool active)
			{
				this.operational.SetActive(active, false);
			}

			// Token: 0x0600D003 RID: 53251 RVA: 0x00434C1C File Offset: 0x00432E1C
			private bool HasSufficientMass()
			{
				bool result = false;
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
				if (primaryElement != null)
				{
					result = (primaryElement.Mass >= 5f);
				}
				return result;
			}

			// Token: 0x0600D004 RID: 53252 RVA: 0x00434C58 File Offset: 0x00432E58
			public bool OutputFull()
			{
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.DirtyWater);
				return primaryElement != null && primaryElement.Mass >= 5f;
			}

			// Token: 0x0600D005 RID: 53253 RVA: 0x00434C91 File Offset: 0x00432E91
			public bool IsReady()
			{
				return this.HasSufficientMass() && !this.OutputFull();
			}

			// Token: 0x0400B66F RID: 46703
			private Operational operational;

			// Token: 0x0400B670 RID: 46704
			private ConduitConsumer consumer;

			// Token: 0x0400B671 RID: 46705
			private ConduitDispenser dispenser;
		}
	}
}
