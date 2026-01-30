using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200075C RID: 1884
public class FlushToilet : StateMachineComponent<FlushToilet.SMInstance>, IUsable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06002FB4 RID: 12212 RVA: 0x00113460 File Offset: 0x00111660
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		ConduitFlow liquidConduitFlow = Game.Instance.liquidConduitFlow;
		liquidConduitFlow.onConduitsRebuilt += this.OnConduitsRebuilt;
		liquidConduitFlow.AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.Default);
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.fillMeter = new MeterController(component2, "meter_target", "meter", this.meterOffset, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), Array.Empty<string>());
		this.contaminationMeter = new MeterController(component2, "meter_target", "meter_dirty", this.meterOffset, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), Array.Empty<string>());
		this.gunkMeter = new MeterController(component2, "meter_target", "meter_gunky", this.meterOffset, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), Array.Empty<string>());
		Components.Toilets.Add(this);
		Components.BasicBuildings.Add(this);
		base.smi.StartSM();
		base.smi.ShowFillMeter();
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x00113598 File Offset: 0x00111798
	protected override void OnCleanUp()
	{
		Game.Instance.liquidConduitFlow.onConduitsRebuilt -= this.OnConduitsRebuilt;
		Components.BasicBuildings.Remove(this);
		Components.Toilets.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x001135D1 File Offset: 0x001117D1
	private void OnConduitsRebuilt()
	{
		base.Trigger(-2094018600, BoxedBools.False);
	}

	// Token: 0x06002FB7 RID: 12215 RVA: 0x001135E3 File Offset: 0x001117E3
	public bool IsUsable()
	{
		return base.smi.HasTag(GameTags.Usable);
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x001135F8 File Offset: 0x001117F8
	private void AddDisseaseToWorker(WorkerBase worker)
	{
		if (worker != null)
		{
			byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
			worker.GetComponent<PrimaryElement>().AddDisease(index, this.diseaseOnDupePerFlush, "FlushToilet.Flush");
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, this.diseasePerFlush + this.diseaseOnDupePerFlush), base.transform, Vector3.up, 1.5f, false, false);
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
			return;
		}
		DebugUtil.LogWarningArgs(new object[]
		{
			"Tried to add disease on toilet use but worker was null"
		});
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x001136C4 File Offset: 0x001118C4
	private void Flush(WorkerBase worker)
	{
		ToiletWorkableUse component = base.GetComponent<ToiletWorkableUse>();
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.storage.Find(FlushToilet.WaterTag, pooledList);
		float num = 0f;
		float num2 = this.massConsumedPerUse;
		foreach (GameObject gameObject in pooledList)
		{
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			float num3 = Mathf.Min(component2.Mass, num2);
			component2.Mass -= num3;
			num2 -= num3;
			num += num3 * component2.Temperature;
		}
		pooledList.Recycle();
		float lastAmountOfWasteMassRemovedFromDupe = component.lastAmountOfWasteMassRemovedFromDupe;
		num += lastAmountOfWasteMassRemovedFromDupe * this.newPeeTemperature;
		float num4 = this.massConsumedPerUse + lastAmountOfWasteMassRemovedFromDupe;
		float temperature = num / num4;
		byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
		this.storage.AddLiquid(component.lastElementRemovedFromDupe, num4, temperature, index, this.diseasePerFlush, false, true);
	}

	// Token: 0x06002FBA RID: 12218 RVA: 0x001137D8 File Offset: 0x001119D8
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(SimHashes.Water).tag.ProperName();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x06002FBB RID: 12219 RVA: 0x00113854 File Offset: 0x00111A54
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag.ProperName();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(this.massEmittedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.newPeeTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(this.massEmittedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.newPeeTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
		Disease disease = Db.Get().Diseases.Get(this.diseaseId);
		int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.DiseaseSource, false));
		return list;
	}

	// Token: 0x06002FBC RID: 12220 RVA: 0x00113955 File Offset: 0x00111B55
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x06002FBD RID: 12221 RVA: 0x00113974 File Offset: 0x00111B74
	private void OnConduitUpdate(float dt)
	{
		if (this.GetSMI() == null)
		{
			return;
		}
		ConduitFlow liquidConduitFlow = Game.Instance.liquidConduitFlow;
		bool value = base.smi.master.requireOutput && liquidConduitFlow.GetContents(this.outputCell).mass > 0f && base.smi.HasContaminatedMass();
		base.smi.sm.outputBlocked.Set(value, base.smi, false);
	}

	// Token: 0x04001C5D RID: 7261
	private static readonly HashedString[] CLOGGED_ANIMS = new HashedString[]
	{
		"full_gunk_pre",
		"full_gunk"
	};

	// Token: 0x04001C5E RID: 7262
	private const string UNCLOG_ANIM = "full_gunk_pst";

	// Token: 0x04001C5F RID: 7263
	private MeterController fillMeter;

	// Token: 0x04001C60 RID: 7264
	private MeterController contaminationMeter;

	// Token: 0x04001C61 RID: 7265
	private MeterController gunkMeter;

	// Token: 0x04001C62 RID: 7266
	public Meter.Offset meterOffset = Meter.Offset.Behind;

	// Token: 0x04001C63 RID: 7267
	[SerializeField]
	public float massConsumedPerUse = 5f;

	// Token: 0x04001C64 RID: 7268
	[SerializeField]
	public float massEmittedPerUse = 5f;

	// Token: 0x04001C65 RID: 7269
	[SerializeField]
	public float newPeeTemperature;

	// Token: 0x04001C66 RID: 7270
	[SerializeField]
	public string diseaseId;

	// Token: 0x04001C67 RID: 7271
	[SerializeField]
	public int diseasePerFlush;

	// Token: 0x04001C68 RID: 7272
	[SerializeField]
	public int diseaseOnDupePerFlush;

	// Token: 0x04001C69 RID: 7273
	[SerializeField]
	public bool requireOutput = true;

	// Token: 0x04001C6A RID: 7274
	[MyCmpGet]
	private ConduitConsumer conduitConsumer;

	// Token: 0x04001C6B RID: 7275
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001C6C RID: 7276
	public static readonly Tag WaterTag = GameTagExtensions.Create(SimHashes.Water);

	// Token: 0x04001C6D RID: 7277
	private int inputCell;

	// Token: 0x04001C6E RID: 7278
	private int outputCell;

	// Token: 0x02001645 RID: 5701
	public class SMInstance : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.GameInstance
	{
		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x0600967C RID: 38524 RVA: 0x0037FDC5 File Offset: 0x0037DFC5
		public bool IsClogged
		{
			get
			{
				return base.sm.isClogged.Get(this);
			}
		}

		// Token: 0x0600967D RID: 38525 RVA: 0x0037FDD8 File Offset: 0x0037DFD8
		public SMInstance(FlushToilet master) : base(master)
		{
			this.activeUseChores = new List<Chore>();
			this.UpdateFullnessState();
			this.UpdateDirtyState();
		}

		// Token: 0x0600967E RID: 38526 RVA: 0x0037FDFC File Offset: 0x0037DFFC
		public void CreateCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("dupe");
			}
			ToiletWorkableClean component = base.GetComponent<ToiletWorkableClean>();
			component.SetIsCloggedByGunk(this.IsClogged);
			this.cleanChore = new WorkChore<ToiletWorkableClean>(Db.Get().ChoreTypes.CleanToilet, component, null, true, new Action<Chore>(this.OnCleanComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x0600967F RID: 38527 RVA: 0x0037FE6B File Offset: 0x0037E06B
		public void CancelCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("Cancelled");
				this.cleanChore = null;
			}
		}

		// Token: 0x06009680 RID: 38528 RVA: 0x0037FE8C File Offset: 0x0037E08C
		private void OnCleanComplete(object o)
		{
			base.sm.isClogged.Set(false, this, false);
		}

		// Token: 0x06009681 RID: 38529 RVA: 0x0037FEA4 File Offset: 0x0037E0A4
		public bool HasValidConnections()
		{
			return Game.Instance.liquidConduitFlow.HasConduit(base.master.inputCell) && (!base.master.requireOutput || Game.Instance.liquidConduitFlow.HasConduit(base.master.outputCell));
		}

		// Token: 0x06009682 RID: 38530 RVA: 0x0037FEF8 File Offset: 0x0037E0F8
		public bool UpdateFullnessState()
		{
			float num = 0f;
			ListPool<GameObject, FlushToilet>.PooledList pooledList = ListPool<GameObject, FlushToilet>.Allocate();
			base.master.storage.Find(FlushToilet.WaterTag, pooledList);
			foreach (GameObject gameObject in pooledList)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				num += component.Mass;
			}
			pooledList.Recycle();
			bool flag = num >= base.master.massConsumedPerUse;
			base.master.conduitConsumer.enabled = !flag;
			float positionPercent = Mathf.Clamp01(num / base.master.massConsumedPerUse);
			base.master.fillMeter.SetPositionPercent(positionPercent);
			return flag;
		}

		// Token: 0x06009683 RID: 38531 RVA: 0x0037FFC4 File Offset: 0x0037E1C4
		public void SetDirtyStatesForClogged()
		{
			bool flag = base.GetComponent<ToiletWorkableUse>().last_user_id == BionicMinionConfig.ID;
			this.SetDirtyStateMeterPercentage((float)(flag ? 0 : 1), (float)(flag ? 1 : 0));
		}

		// Token: 0x06009684 RID: 38532 RVA: 0x00380002 File Offset: 0x0037E202
		public void SetDirtyStateMeterPercentage(float contaminationPercentage, float gunkPercentage)
		{
			base.master.contaminationMeter.SetPositionPercent(contaminationPercentage);
			base.master.gunkMeter.SetPositionPercent(gunkPercentage);
		}

		// Token: 0x06009685 RID: 38533 RVA: 0x00380028 File Offset: 0x0037E228
		public void UpdateDirtyState()
		{
			ToiletWorkableUse component = base.GetComponent<ToiletWorkableUse>();
			float percentComplete = component.GetPercentComplete();
			bool flag = component.last_user_id == BionicMinionConfig.ID;
			this.SetDirtyStateMeterPercentage(flag ? 0f : percentComplete, flag ? percentComplete : 0f);
		}

		// Token: 0x06009686 RID: 38534 RVA: 0x00380074 File Offset: 0x0037E274
		public void AddDisseaseToWorker()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.AddDisseaseToWorker(worker);
		}

		// Token: 0x06009687 RID: 38535 RVA: 0x003800A0 File Offset: 0x0037E2A0
		public void Flush()
		{
			bool flag = base.GetComponent<ToiletWorkableUse>().last_user_id == BionicMinionConfig.ID;
			base.master.fillMeter.SetPositionPercent(0f);
			base.master.contaminationMeter.SetPositionPercent(flag ? 0f : 1f);
			base.master.gunkMeter.SetPositionPercent(flag ? 1f : 0f);
			base.smi.ShowFillMeter();
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.Flush(worker);
		}

		// Token: 0x06009688 RID: 38536 RVA: 0x00380144 File Offset: 0x0037E344
		public void ShowFillMeter()
		{
			base.master.fillMeter.gameObject.SetActive(true);
			base.master.contaminationMeter.gameObject.SetActive(false);
			base.master.gunkMeter.gameObject.SetActive(false);
		}

		// Token: 0x06009689 RID: 38537 RVA: 0x00380194 File Offset: 0x0037E394
		public bool HasContaminatedMass()
		{
			foreach (GameObject gameObject in base.GetComponent<Storage>().items)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && (component.ElementID == SimHashes.DirtyWater || component.ElementID == GunkMonitor.GunkElement) && component.Mass > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600968A RID: 38538 RVA: 0x00380224 File Offset: 0x0037E424
		public void ShowContaminatedMeter()
		{
			bool flag = base.GetComponent<ToiletWorkableUse>().last_user_id == BionicMinionConfig.ID;
			base.master.fillMeter.gameObject.SetActive(false);
			base.master.contaminationMeter.gameObject.SetActive(!flag);
			base.master.gunkMeter.gameObject.SetActive(flag);
		}

		// Token: 0x0400743A RID: 29754
		public List<Chore> activeUseChores;

		// Token: 0x0400743B RID: 29755
		private Chore cleanChore;
	}

	// Token: 0x02001646 RID: 5702
	public class States : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet>
	{
		// Token: 0x0600968B RID: 38539 RVA: 0x00380294 File Offset: 0x0037E494
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disconnected;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.disconnected.PlayAnim("off").EventTransition(GameHashes.ConduitConnectionChanged, this.backedup, (FlushToilet.SMInstance smi) => smi.HasValidConnections()).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.backedup.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, null).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (FlushToilet.SMInstance smi) => !smi.HasValidConnections()).ParamTransition<bool>(this.outputBlocked, this.fillingInactive, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsFalse).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.filling.PlayAnim("on").Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			}).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (FlushToilet.SMInstance smi) => !smi.HasValidConnections()).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).EventTransition(GameHashes.OnStorageChange, this.ready, (FlushToilet.SMInstance smi) => smi.UpdateFullnessState()).EventTransition(GameHashes.OperationalChanged, this.fillingInactive, (FlushToilet.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.fillingInactive.PlayAnim("on").Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			}).EventTransition(GameHashes.OperationalChanged, this.filling, (FlushToilet.SMInstance smi) => smi.GetComponent<Operational>().IsOperational).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue);
			this.ready.DefaultState(this.ready.idle).ToggleTag(GameTags.Usable).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.master.fillMeter.SetPositionPercent(1f);
				smi.master.contaminationMeter.SetPositionPercent(0f);
				smi.master.gunkMeter.SetPositionPercent(0f);
			}).PlayAnim("on").EventHandler(GameHashes.FlushGunk, new GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.GameEvent.Callback(this.OnFlushedGunk)).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (FlushToilet.SMInstance smi) => !smi.HasValidConnections()).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).ToggleRecurringChore(new Func<FlushToilet.SMInstance, Chore>(this.CreateUrgentUseChore), null).ToggleRecurringChore(new Func<FlushToilet.SMInstance, Chore>(this.CreateBreakUseChore), null);
			this.ready.idle.ParamTransition<bool>(this.isClogged, this.clogged, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.FlushToilet, null).WorkableStartTransition((FlushToilet.SMInstance smi) => smi.master.GetComponent<ToiletWorkableUse>(), this.ready.inuse);
			this.ready.inuse.Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.ShowContaminatedMeter();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.FlushToiletInUse, null).Update(delegate(FlushToilet.SMInstance smi, float dt)
			{
				smi.UpdateDirtyState();
			}, UpdateRate.SIM_200ms, false).WorkableCompleteTransition((FlushToilet.SMInstance smi) => smi.master.GetComponent<ToiletWorkableUse>(), this.ready.completed).WorkableStopTransition((FlushToilet.SMInstance smi) => smi.master.GetComponent<ToiletWorkableUse>(), this.flushed);
			this.ready.completed.Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.AddDisseaseToWorker();
			}).EnterTransition(this.clogged, (FlushToilet.SMInstance smi) => smi.IsClogged).EnterGoTo(this.flushing);
			this.clogged.PlayAnims((FlushToilet.SMInstance smi) => FlushToilet.CLOGGED_ANIMS, KAnim.PlayMode.Once).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.ShowContaminatedMeter();
			}).Enter(new StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback(this.SetDirtyStatesForClogged)).Enter(new StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback(this.CreateCleanChore)).Exit(new StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback(this.CancelCleanChore)).ParamTransition<bool>(this.isClogged, this.unclogged, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsFalse);
			this.unclogged.PlayAnim("full_gunk_pst").OnAnimQueueComplete(this.flushing);
			this.flushing.Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.Flush();
			}).PlayAnim("flush").OnAnimQueueComplete(this.flushed);
			this.flushed.EventTransition(GameHashes.OnStorageChange, this.fillingInactive, (FlushToilet.SMInstance smi) => !smi.HasContaminatedMass()).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).PlayAnim("on");
		}

		// Token: 0x0600968C RID: 38540 RVA: 0x003808C1 File Offset: 0x0037EAC1
		public void OnFlushedGunk(FlushToilet.SMInstance smi, object o)
		{
			smi.sm.isClogged.Set(true, smi, false);
		}

		// Token: 0x0600968D RID: 38541 RVA: 0x003808D7 File Offset: 0x0037EAD7
		public void SetDirtyStatesForClogged(FlushToilet.SMInstance smi)
		{
			smi.SetDirtyStatesForClogged();
		}

		// Token: 0x0600968E RID: 38542 RVA: 0x003808DF File Offset: 0x0037EADF
		public void CreateCleanChore(FlushToilet.SMInstance smi)
		{
			smi.CreateCleanChore();
		}

		// Token: 0x0600968F RID: 38543 RVA: 0x003808E7 File Offset: 0x0037EAE7
		public void CancelCleanChore(FlushToilet.SMInstance smi)
		{
			smi.CancelCleanChore();
		}

		// Token: 0x06009690 RID: 38544 RVA: 0x003808EF File Offset: 0x0037EAEF
		private Chore CreateUrgentUseChore(FlushToilet.SMInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, null);
			return chore;
		}

		// Token: 0x06009691 RID: 38545 RVA: 0x00380929 File Offset: 0x0037EB29
		private Chore CreateBreakUseChore(FlushToilet.SMInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, null);
			return chore;
		}

		// Token: 0x06009692 RID: 38546 RVA: 0x00380954 File Offset: 0x0037EB54
		private Chore CreateUseChore(FlushToilet.SMInstance smi, ChoreType choreType)
		{
			WorkChore<ToiletWorkableUse> workChore = new WorkChore<ToiletWorkableUse>(choreType, smi.master, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
			smi.activeUseChores.Add(workChore);
			WorkChore<ToiletWorkableUse> workChore2 = workChore;
			workChore2.onExit = (Action<Chore>)Delegate.Combine(workChore2.onExit, new Action<Chore>(delegate(Chore exiting_chore)
			{
				smi.activeUseChores.Remove(exiting_chore);
			}));
			workChore.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
			workChore.AddPrecondition(ChorePreconditions.instance.IsExclusivelyAvailableWithOtherChores, smi.activeUseChores);
			return workChore;
		}

		// Token: 0x0400743C RID: 29756
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State disconnected;

		// Token: 0x0400743D RID: 29757
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State backedup;

		// Token: 0x0400743E RID: 29758
		public FlushToilet.States.ReadyStates ready;

		// Token: 0x0400743F RID: 29759
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State fillingInactive;

		// Token: 0x04007440 RID: 29760
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State filling;

		// Token: 0x04007441 RID: 29761
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State clogged;

		// Token: 0x04007442 RID: 29762
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State unclogged;

		// Token: 0x04007443 RID: 29763
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State flushing;

		// Token: 0x04007444 RID: 29764
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State flushed;

		// Token: 0x04007445 RID: 29765
		public StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.BoolParameter outputBlocked;

		// Token: 0x04007446 RID: 29766
		public StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.BoolParameter isClogged;

		// Token: 0x020028E2 RID: 10466
		public class ReadyStates : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State
		{
			// Token: 0x0400B45B RID: 46171
			public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State idle;

			// Token: 0x0400B45C RID: 46172
			public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State inuse;

			// Token: 0x0400B45D RID: 46173
			public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State completed;
		}
	}
}
