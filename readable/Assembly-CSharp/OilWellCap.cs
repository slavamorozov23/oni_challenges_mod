using System;
using Klei;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007D1 RID: 2001
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/OilWellCap")]
public class OilWellCap : RemoteWorkable, ISingleSliderControl, ISliderControl, IElementEmitter
{
	// Token: 0x1700034F RID: 847
	// (get) Token: 0x0600350D RID: 13581 RVA: 0x0012C696 File Offset: 0x0012A896
	public SimHashes Element
	{
		get
		{
			return this.gasElement;
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x0600350E RID: 13582 RVA: 0x0012C69E File Offset: 0x0012A89E
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x0600350F RID: 13583 RVA: 0x0012C6B5 File Offset: 0x0012A8B5
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06003510 RID: 13584 RVA: 0x0012C6BC File Offset: 0x0012A8BC
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06003511 RID: 13585 RVA: 0x0012C6C8 File Offset: 0x0012A8C8
	public override Chore RemoteDockChore
	{
		get
		{
			return this.DepressurizeChore;
		}
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x0012C6D0 File Offset: 0x0012A8D0
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x0012C6D3 File Offset: 0x0012A8D3
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x0012C6DA File Offset: 0x0012A8DA
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x0012C6E1 File Offset: 0x0012A8E1
	public float GetSliderValue(int index)
	{
		return this.depressurizePercent * 100f;
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x0012C6EF File Offset: 0x0012A8EF
	public void SetSliderValue(float value, int index)
	{
		this.depressurizePercent = value / 100f;
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x0012C6FE File Offset: 0x0012A8FE
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x0012C705 File Offset: 0x0012A905
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP"), this.depressurizePercent * 100f);
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x0012C72C File Offset: 0x0012A92C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OilWellCap>(-905833192, OilWellCap.OnCopySettingsDelegate);
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x0012C748 File Offset: 0x0012A948
	private void OnCopySettings(object data)
	{
		OilWellCap component = ((GameObject)data).GetComponent<OilWellCap>();
		if (component != null)
		{
			this.depressurizePercent = component.depressurizePercent;
		}
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x0012C778 File Offset: 0x0012A978
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.accumulator = Game.Instance.accumulators.Add("pressuregas", this);
		this.showProgressBar = false;
		base.SetWorkTime(float.PositiveInfinity);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_oil_cap_kanim")
		};
		this.workingStatusItem = Db.Get().BuildingStatusItems.ReleasingPressure;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.pressureMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.smi = new OilWellCap.StatesInstance(this);
		this.smi.StartSM();
		this.UpdatePressurePercent();
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x0012C88D File Offset: 0x0012AA8D
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x0012C8B6 File Offset: 0x0012AAB6
	public void AddGasPressure(float dt)
	{
		this.storage.AddGasChunk(this.gasElement, this.addGasRate * dt, this.gasTemperature, 0, 0, true, true);
		this.UpdatePressurePercent();
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x0012C8E4 File Offset: 0x0012AAE4
	public void ReleaseGasPressure(float dt)
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.gasElement);
		if (primaryElement != null && primaryElement.Mass > 0f)
		{
			float num = this.releaseGasRate * dt;
			if (base.worker != null)
			{
				num *= this.GetEfficiencyMultiplier(base.worker);
			}
			num = Mathf.Min(num, primaryElement.Mass);
			SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(primaryElement, num / primaryElement.Mass);
			primaryElement.Mass -= num;
			Game.Instance.accumulators.Accumulate(this.accumulator, num);
			SimMessages.AddRemoveSubstance(Grid.PosToCell(this), ElementLoader.GetElementIndex(this.gasElement), null, num, primaryElement.Temperature, percentOfDisease.idx, percentOfDisease.count, true, -1);
		}
		this.UpdatePressurePercent();
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x0012C9B8 File Offset: 0x0012ABB8
	private void UpdatePressurePercent()
	{
		float num = this.storage.GetMassAvailable(this.gasElement) / this.maxGasPressure;
		num = Mathf.Clamp01(num);
		this.smi.sm.pressurePercent.Set(num, this.smi, false);
		this.pressureMeter.SetPositionPercent(num);
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x0012CA0F File Offset: 0x0012AC0F
	public bool NeedsDepressurizing()
	{
		return this.smi.GetPressurePercent() >= this.depressurizePercent;
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x0012CA28 File Offset: 0x0012AC28
	private WorkChore<OilWellCap> CreateWorkChore()
	{
		this.DepressurizeChore = new WorkChore<OilWellCap>(Db.Get().ChoreTypes.Depressurize, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.DepressurizeChore.AddPrecondition(OilWellCap.AllowedToDepressurize, this);
		return this.DepressurizeChore;
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x0012CA78 File Offset: 0x0012AC78
	private void CancelChore(string reason)
	{
		if (this.DepressurizeChore != null)
		{
			this.DepressurizeChore.Cancel(reason);
		}
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x0012CA8E File Offset: 0x0012AC8E
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.smi.sm.working.Set(true, this.smi, false);
	}

	// Token: 0x06003524 RID: 13604 RVA: 0x0012CAB5 File Offset: 0x0012ACB5
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.smi.sm.working.Set(false, this.smi, false);
		this.DepressurizeChore = null;
	}

	// Token: 0x06003525 RID: 13605 RVA: 0x0012CAE3 File Offset: 0x0012ACE3
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return this.smi.GetPressurePercent() <= 0f;
	}

	// Token: 0x06003526 RID: 13606 RVA: 0x0012CAFA File Offset: 0x0012ACFA
	public override bool InstantlyFinish(WorkerBase worker)
	{
		this.ReleaseGasPressure(60f);
		return true;
	}

	// Token: 0x0400201F RID: 8223
	private OilWellCap.StatesInstance smi;

	// Token: 0x04002020 RID: 8224
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002021 RID: 8225
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002022 RID: 8226
	public SimHashes gasElement;

	// Token: 0x04002023 RID: 8227
	public float gasTemperature;

	// Token: 0x04002024 RID: 8228
	public float addGasRate = 1f;

	// Token: 0x04002025 RID: 8229
	public float maxGasPressure = 10f;

	// Token: 0x04002026 RID: 8230
	public float releaseGasRate = 10f;

	// Token: 0x04002027 RID: 8231
	[Serialize]
	private float depressurizePercent = 0.75f;

	// Token: 0x04002028 RID: 8232
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04002029 RID: 8233
	private MeterController pressureMeter;

	// Token: 0x0400202A RID: 8234
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400202B RID: 8235
	private static readonly EventSystem.IntraObjectHandler<OilWellCap> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OilWellCap>(delegate(OilWellCap component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400202C RID: 8236
	private WorkChore<OilWellCap> DepressurizeChore;

	// Token: 0x0400202D RID: 8237
	private static readonly Chore.Precondition AllowedToDepressurize = new Chore.Precondition
	{
		id = "AllowedToDepressurize",
		description = DUPLICANTS.CHORES.PRECONDITIONS.ALLOWED_TO_DEPRESSURIZE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((OilWellCap)data).NeedsDepressurizing();
		}
	};

	// Token: 0x02001722 RID: 5922
	public class StatesInstance : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.GameInstance
	{
		// Token: 0x06009A1B RID: 39451 RVA: 0x0038FB08 File Offset: 0x0038DD08
		public StatesInstance(OilWellCap master) : base(master)
		{
		}

		// Token: 0x06009A1C RID: 39452 RVA: 0x0038FB11 File Offset: 0x0038DD11
		public float GetPressurePercent()
		{
			return base.sm.pressurePercent.Get(base.smi);
		}
	}

	// Token: 0x02001723 RID: 5923
	public class States : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap>
	{
		// Token: 0x06009A1D RID: 39453 RVA: 0x0038FB2C File Offset: 0x0038DD2C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational));
			this.operational.DefaultState(this.operational.idle).ToggleRecurringChore((OilWellCap.StatesInstance smi) => smi.master.CreateWorkChore(), null).EventHandler(GameHashes.WorkChoreDisabled, delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.CancelChore("WorkChoreDisabled");
			});
			this.operational.idle.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational.active, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsAbleToPump));
			this.operational.active.DefaultState(this.operational.active.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).Enter(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.AddGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.active.pre.PlayAnim("working_pre").ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).OnAnimQueueComplete(this.operational.active.loop);
			this.operational.active.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.pressurePercent, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping)).EventTransition(GameHashes.OnStorageChange, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping));
			this.operational.active.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.idle);
			this.operational.overpressure.PlayAnim("over_pressured_pre", KAnim.PlayMode.Once).QueueAnim("over_pressured_loop", true, null).ToggleStatusItem(Db.Get().BuildingStatusItems.WellOverpressure, null).ParamTransition<float>(this.pressurePercent, this.operational.idle, (OilWellCap.StatesInstance smi, float p) => p <= 0f).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue);
			this.operational.releasing_pressure.DefaultState(this.operational.releasing_pressure.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingElement, (OilWellCap.StatesInstance smi) => smi.master);
			this.operational.releasing_pressure.pre.PlayAnim("steam_out_pre").OnAnimQueueComplete(this.operational.releasing_pressure.loop);
			this.operational.releasing_pressure.loop.PlayAnim("steam_out_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).ParamTransition<bool>(this.working, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsFalse).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.ReleaseGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.releasing_pressure.pst.PlayAnim("steam_out_pst").OnAnimQueueComplete(this.operational.idle);
		}

		// Token: 0x06009A1E RID: 39454 RVA: 0x00390017 File Offset: 0x0038E217
		private bool IsOperational(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational;
		}

		// Token: 0x06009A1F RID: 39455 RVA: 0x00390029 File Offset: 0x0038E229
		private bool IsAbleToPump(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational && smi.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x06009A20 RID: 39456 RVA: 0x0039004B File Offset: 0x0038E24B
		private bool MustStopPumping(OilWellCap.StatesInstance smi)
		{
			return !smi.master.operational.IsOperational || !smi.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x040076F9 RID: 30457
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.FloatParameter pressurePercent;

		// Token: 0x040076FA RID: 30458
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.BoolParameter working;

		// Token: 0x040076FB RID: 30459
		public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State inoperational;

		// Token: 0x040076FC RID: 30460
		public OilWellCap.States.OperationalStates operational;

		// Token: 0x02002920 RID: 10528
		public class OperationalStates : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State
		{
			// Token: 0x0400B5D0 RID: 46544
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State idle;

			// Token: 0x0400B5D1 RID: 46545
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState active;

			// Token: 0x0400B5D2 RID: 46546
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State overpressure;

			// Token: 0x0400B5D3 RID: 46547
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState releasing_pressure;
		}
	}
}
