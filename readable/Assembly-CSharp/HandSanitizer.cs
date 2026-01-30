using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000777 RID: 1911
public class HandSanitizer : StateMachineComponent<HandSanitizer.SMInstance>, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06003097 RID: 12439 RVA: 0x00118917 File Offset: 0x00116B17
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.FindOrAddComponent<Workable>();
	}

	// Token: 0x06003098 RID: 12440 RVA: 0x0011892C File Offset: 0x00116B2C
	private void RefreshMeters()
	{
		if (!this.hasMeters)
		{
			return;
		}
		float positionPercent = 0f;
		PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
		float num = (float)this.maxUses * this.massConsumedPerUse;
		ConduitConsumer component = base.GetComponent<ConduitConsumer>();
		if (component != null)
		{
			num = component.capacityKG;
		}
		if (primaryElement != null)
		{
			positionPercent = Mathf.Clamp01(primaryElement.Mass / num);
		}
		float positionPercent2 = 0f;
		PrimaryElement primaryElement2 = base.GetComponent<Storage>().FindPrimaryElement(this.outputElement);
		if (primaryElement2 != null)
		{
			positionPercent2 = Mathf.Clamp01(primaryElement2.Mass / ((float)this.maxUses * this.massConsumedPerUse));
		}
		this.cleanMeter.SetPositionPercent(positionPercent);
		this.dirtyMeter.SetPositionPercent(positionPercent2);
	}

	// Token: 0x06003099 RID: 12441 RVA: 0x001189F4 File Offset: 0x00116BF4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.hasMeters)
		{
			this.cleanMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", this.cleanMeterOffset, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_clean_target"
			});
			this.dirtyMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_dirty_target", "meter_dirty", this.dirtyMeterOffset, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_dirty_target"
			});
		}
		this.RefreshMeters();
		Components.HandSanitizers.Add(this);
		Components.BasicBuildings.Add(this);
		base.Subscribe<HandSanitizer>(-1697596308, HandSanitizer.OnStorageChangeDelegate);
		DirectionControl component = base.GetComponent<DirectionControl>();
		component.onDirectionChanged = (Action<WorkableReactable.AllowedDirection>)Delegate.Combine(component.onDirectionChanged, new Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged));
		this.OnDirectionChanged(base.GetComponent<DirectionControl>().allowedDirection);
	}

	// Token: 0x0600309A RID: 12442 RVA: 0x00118AE1 File Offset: 0x00116CE1
	protected override void OnCleanUp()
	{
		Components.BasicBuildings.Remove(this);
		Components.HandSanitizers.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600309B RID: 12443 RVA: 0x00118AFF File Offset: 0x00116CFF
	private void OnDirectionChanged(WorkableReactable.AllowedDirection allowed_direction)
	{
		if (this.reactable != null)
		{
			this.reactable.allowedDirection = allowed_direction;
		}
	}

	// Token: 0x0600309C RID: 12444 RVA: 0x00118B18 File Offset: 0x00116D18
	public List<Descriptor> RequirementDescriptors()
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, ElementLoader.FindElementByHash(this.consumedElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, ElementLoader.FindElementByHash(this.consumedElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x0600309D RID: 12445 RVA: 0x00118B9C File Offset: 0x00116D9C
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.outputElement != SimHashes.Vacuum)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x0600309E RID: 12446 RVA: 0x00118C71 File Offset: 0x00116E71
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x00118C90 File Offset: 0x00116E90
	private void OnStorageChange(object data)
	{
		if (this.dumpWhenFull && base.smi.OutputFull())
		{
			base.smi.DumpOutput();
		}
		this.RefreshMeters();
	}

	// Token: 0x04001CF2 RID: 7410
	public float massConsumedPerUse = 1f;

	// Token: 0x04001CF3 RID: 7411
	public SimHashes consumedElement = SimHashes.BleachStone;

	// Token: 0x04001CF4 RID: 7412
	public int diseaseRemovalCount = 10000;

	// Token: 0x04001CF5 RID: 7413
	public int maxUses = 10;

	// Token: 0x04001CF6 RID: 7414
	public SimHashes outputElement = SimHashes.Vacuum;

	// Token: 0x04001CF7 RID: 7415
	public bool dumpWhenFull;

	// Token: 0x04001CF8 RID: 7416
	public bool alwaysUse;

	// Token: 0x04001CF9 RID: 7417
	public bool canSanitizeSuit;

	// Token: 0x04001CFA RID: 7418
	public bool canSanitizeStorage;

	// Token: 0x04001CFB RID: 7419
	public bool hasMeters = true;

	// Token: 0x04001CFC RID: 7420
	private WorkableReactable reactable;

	// Token: 0x04001CFD RID: 7421
	private MeterController cleanMeter;

	// Token: 0x04001CFE RID: 7422
	private MeterController dirtyMeter;

	// Token: 0x04001CFF RID: 7423
	public Meter.Offset cleanMeterOffset;

	// Token: 0x04001D00 RID: 7424
	public Meter.Offset dirtyMeterOffset;

	// Token: 0x04001D01 RID: 7425
	[Serialize]
	public int maxPossiblyRemoved;

	// Token: 0x04001D02 RID: 7426
	private static readonly EventSystem.IntraObjectHandler<HandSanitizer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<HandSanitizer>(delegate(HandSanitizer component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001682 RID: 5762
	private class WashHandsReactable : WorkableReactable
	{
		// Token: 0x06009786 RID: 38790 RVA: 0x00384DBC File Offset: 0x00382FBC
		public WashHandsReactable(Workable workable, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable, "WashHands", chore_type, allowed_direction)
		{
		}

		// Token: 0x06009787 RID: 38791 RVA: 0x00384DD4 File Offset: 0x00382FD4
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				HandSanitizer component = this.workable.GetComponent<HandSanitizer>();
				if (!component.smi.IsReady())
				{
					return false;
				}
				if (component.alwaysUse)
				{
					return true;
				}
				PrimaryElement component2 = new_reactor.GetComponent<PrimaryElement>();
				if (component2 != null)
				{
					return component2.DiseaseIdx != byte.MaxValue;
				}
			}
			return false;
		}
	}

	// Token: 0x02001683 RID: 5763
	public class SMInstance : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.GameInstance
	{
		// Token: 0x06009788 RID: 38792 RVA: 0x00384E32 File Offset: 0x00383032
		public SMInstance(HandSanitizer master) : base(master)
		{
		}

		// Token: 0x06009789 RID: 38793 RVA: 0x00384E3C File Offset: 0x0038303C
		private bool HasSufficientMass()
		{
			bool result = false;
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.consumedElement);
			if (primaryElement != null)
			{
				result = (primaryElement.Mass >= base.master.massConsumedPerUse);
			}
			return result;
		}

		// Token: 0x0600978A RID: 38794 RVA: 0x00384E84 File Offset: 0x00383084
		public bool OutputFull()
		{
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.outputElement);
			return primaryElement != null && primaryElement.Mass >= (float)base.master.maxUses * base.master.massConsumedPerUse;
		}

		// Token: 0x0600978B RID: 38795 RVA: 0x00384ED6 File Offset: 0x003830D6
		public bool IsReady()
		{
			return this.HasSufficientMass() && !this.OutputFull();
		}

		// Token: 0x0600978C RID: 38796 RVA: 0x00384EF0 File Offset: 0x003830F0
		public void DumpOutput()
		{
			Storage component = base.master.GetComponent<Storage>();
			if (base.master.outputElement != SimHashes.Vacuum)
			{
				component.Drop(ElementLoader.FindElementByHash(base.master.outputElement).tag);
			}
		}
	}

	// Token: 0x02001684 RID: 5764
	public class States : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer>
	{
		// Token: 0x0600978D RID: 38797 RVA: 0x00384F38 File Offset: 0x00383138
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notready;
			this.root.Update(new Action<HandSanitizer.SMInstance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
			this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
			this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready, (HandSanitizer.SMInstance smi) => smi.IsReady()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((HandSanitizer.SMInstance smi) => smi.master.reactable = new HandSanitizer.WashHandsReactable(smi.master.GetComponent<HandSanitizer.Work>(), Db.Get().ChoreTypes.WashHands, smi.master.GetComponent<DirectionControl>().allowedDirection)).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((HandSanitizer.SMInstance smi) => smi.GetComponent<HandSanitizer.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Enter(delegate(HandSanitizer.SMInstance smi)
			{
				ConduitConsumer component = smi.GetComponent<ConduitConsumer>();
				if (component != null)
				{
					component.enabled = false;
				}
			}).Exit(delegate(HandSanitizer.SMInstance smi)
			{
				ConduitConsumer component = smi.GetComponent<ConduitConsumer>();
				if (component != null)
				{
					component.enabled = true;
				}
			}).WorkableStopTransition((HandSanitizer.SMInstance smi) => smi.GetComponent<HandSanitizer.Work>(), this.notready);
		}

		// Token: 0x0600978E RID: 38798 RVA: 0x00385100 File Offset: 0x00383300
		private void UpdateStatusItems(HandSanitizer.SMInstance smi, float dt)
		{
			if (smi.OutputFull())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
		}

		// Token: 0x0400751B RID: 29979
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notready;

		// Token: 0x0400751C RID: 29980
		public HandSanitizer.States.ReadyStates ready;

		// Token: 0x0400751D RID: 29981
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notoperational;

		// Token: 0x0400751E RID: 29982
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State full;

		// Token: 0x0400751F RID: 29983
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State empty;

		// Token: 0x020028FA RID: 10490
		public class ReadyStates : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State
		{
			// Token: 0x0400B506 RID: 46342
			public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State free;

			// Token: 0x0400B507 RID: 46343
			public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State occupied;
		}
	}

	// Token: 0x02001685 RID: 5765
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x06009790 RID: 38800 RVA: 0x00385160 File Offset: 0x00383360
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.shouldTransferDiseaseWithWorker = false;
			GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true);
			}, null, null);
		}

		// Token: 0x06009791 RID: 38801 RVA: 0x003851B8 File Offset: 0x003833B8
		public override Workable.AnimInfo GetAnim(WorkerBase worker)
		{
			KAnimFile[] overrideAnims = null;
			if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
			{
				this.overrideAnims = overrideAnims;
			}
			return base.GetAnim(worker);
		}

		// Token: 0x06009792 RID: 38802 RVA: 0x003851EA File Offset: 0x003833EA
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			this.diseaseRemoved = 0;
		}

		// Token: 0x06009793 RID: 38803 RVA: 0x003851FC File Offset: 0x003833FC
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			HandSanitizer component = base.GetComponent<HandSanitizer>();
			Storage component2 = base.GetComponent<Storage>();
			float massAvailable = component2.GetMassAvailable(component.consumedElement);
			if (massAvailable == 0f)
			{
				return true;
			}
			PrimaryElement component3 = worker.GetComponent<PrimaryElement>();
			float amount = Mathf.Min(component.massConsumedPerUse * dt / this.workTime, massAvailable);
			int num = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), component3.DiseaseCount);
			this.diseaseRemoved += num;
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			invalid.idx = component3.DiseaseIdx;
			invalid.count = num;
			component3.ModifyDiseaseCount(-num, "HandSanitizer.OnWorkTick");
			component.maxPossiblyRemoved += num;
			if (component.canSanitizeStorage && worker.GetComponent<Storage>())
			{
				foreach (GameObject gameObject in worker.GetComponent<Storage>().GetItems())
				{
					PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
					if (component4)
					{
						int num2 = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), component4.DiseaseCount);
						component4.ModifyDiseaseCount(-num2, "HandSanitizer.OnWorkTick");
						component.maxPossiblyRemoved += num2;
					}
				}
			}
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			float mass;
			float temperature;
			component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component.consumedElement).tag, amount, out mass, out diseaseInfo, out temperature);
			if (component.outputElement != SimHashes.Vacuum)
			{
				diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(invalid, diseaseInfo);
				component2.AddLiquid(component.outputElement, mass, temperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			}
			return false;
		}

		// Token: 0x06009794 RID: 38804 RVA: 0x003853C8 File Offset: 0x003835C8
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			if (this.removeIrritation && !worker.HasTag(GameTags.HasSuitTank))
			{
				GasLiquidExposureMonitor.Instance smi = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
				if (smi != null)
				{
					smi.ResetExposure();
				}
			}
		}

		// Token: 0x04007520 RID: 29984
		public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

		// Token: 0x04007521 RID: 29985
		public bool removeIrritation;

		// Token: 0x04007522 RID: 29986
		private int diseaseRemoved;
	}
}
