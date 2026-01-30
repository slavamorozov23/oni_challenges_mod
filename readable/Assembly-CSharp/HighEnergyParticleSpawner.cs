using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200077A RID: 1914
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticleSpawner : StateMachineComponent<HighEnergyParticleSpawner.StatesInstance>, IHighEnergyParticleDirection, IProgressBarSideScreen, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000297 RID: 663
	// (get) Token: 0x060030B8 RID: 12472 RVA: 0x00119448 File Offset: 0x00117648
	public float PredictedPerCycleConsumptionRate
	{
		get
		{
			return (float)Mathf.FloorToInt(this.recentPerSecondConsumptionRate * 0.1f * 600f);
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x060030B9 RID: 12473 RVA: 0x00119462 File Offset: 0x00117662
	// (set) Token: 0x060030BA RID: 12474 RVA: 0x0011946C File Offset: 0x0011766C
	public EightDirection Direction
	{
		get
		{
			return this._direction;
		}
		set
		{
			this._direction = value;
			if (this.directionController != null)
			{
				this.directionController.SetRotation((float)(45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
				this.directionController.controller.enabled = false;
				this.directionController.controller.enabled = true;
			}
		}
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x001194C4 File Offset: 0x001176C4
	private void OnCopySettings(object data)
	{
		HighEnergyParticleSpawner component = ((GameObject)data).GetComponent<HighEnergyParticleSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.particleThreshold = component.particleThreshold;
		}
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x001194FE File Offset: 0x001176FE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HighEnergyParticleSpawner>(-905833192, HighEnergyParticleSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x00119518 File Offset: 0x00117718
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x001195C3 File Offset: 0x001177C3
	public float GetProgressBarMaxValue()
	{
		return this.particleThreshold;
	}

	// Token: 0x060030BF RID: 12479 RVA: 0x001195CB File Offset: 0x001177CB
	public float GetProgressBarFillPercentage()
	{
		return this.particleStorage.Particles / this.particleThreshold;
	}

	// Token: 0x060030C0 RID: 12480 RVA: 0x001195DF File Offset: 0x001177DF
	public string GetProgressBarTitleLabel()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_LABEL;
	}

	// Token: 0x060030C1 RID: 12481 RVA: 0x001195EC File Offset: 0x001177EC
	public string GetProgressBarLabel()
	{
		return Mathf.FloorToInt(this.particleStorage.Particles).ToString() + "/" + Mathf.FloorToInt(this.particleThreshold).ToString();
	}

	// Token: 0x060030C2 RID: 12482 RVA: 0x0011962E File Offset: 0x0011782E
	public string GetProgressBarTooltip()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_TOOLTIP;
	}

	// Token: 0x060030C3 RID: 12483 RVA: 0x0011963A File Offset: 0x0011783A
	public void DoConsumeParticlesWhileDisabled(float dt)
	{
		this.particleStorage.ConsumeAndGet(dt * 0.05f);
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
	}

	// Token: 0x060030C4 RID: 12484 RVA: 0x00119660 File Offset: 0x00117860
	public void LauncherUpdate(float dt)
	{
		this.radiationSampleTimer += dt;
		if (this.radiationSampleTimer >= this.radiationSampleRate)
		{
			this.radiationSampleTimer -= this.radiationSampleRate;
			int i = Grid.PosToCell(this);
			float num = Grid.Radiation[i];
			if (num != 0f && this.particleStorage.RemainingCapacity() > 0f)
			{
				base.smi.sm.isAbsorbingRadiation.Set(true, base.smi, false);
				this.recentPerSecondConsumptionRate = num / 600f;
				this.particleStorage.Store(this.recentPerSecondConsumptionRate * this.radiationSampleRate * 0.1f);
				float num2 = 286f;
				this.energyConsumer.BaseWattageRating = Mathf.Clamp(this.recentPerSecondConsumptionRate * num2, 60f, 480f);
			}
			else
			{
				this.recentPerSecondConsumptionRate = 0f;
				base.smi.sm.isAbsorbingRadiation.Set(false, base.smi, false);
			}
		}
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
		if (!this.particleVisualPlaying && this.particleStorage.Particles > this.particleThreshold / 2f)
		{
			this.particleController.meterController.Play("orb_pre", KAnim.PlayMode.Once, 1f, 0f);
			this.particleController.meterController.Queue("orb_idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.particleVisualPlaying = true;
		}
		this.launcherTimer += dt;
		if (this.launcherTimer < this.minLaunchInterval)
		{
			return;
		}
		if (this.particleStorage.Particles >= this.particleThreshold)
		{
			this.launcherTimer = 0f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.particleStorage.ConsumeAndGet(this.particleThreshold);
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
				this.particleVisualPlaying = false;
			}
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x060030C5 RID: 12485 RVA: 0x00119920 File Offset: 0x00117B20
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x060030C6 RID: 12486 RVA: 0x00119927 File Offset: 0x00117B27
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x060030C7 RID: 12487 RVA: 0x00119933 File Offset: 0x00117B33
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x060030C8 RID: 12488 RVA: 0x00119936 File Offset: 0x00117B36
	public float GetSliderMin(int index)
	{
		return (float)this.minSlider;
	}

	// Token: 0x060030C9 RID: 12489 RVA: 0x0011993F File Offset: 0x00117B3F
	public float GetSliderMax(int index)
	{
		return (float)this.maxSlider;
	}

	// Token: 0x060030CA RID: 12490 RVA: 0x00119948 File Offset: 0x00117B48
	public float GetSliderValue(int index)
	{
		return this.particleThreshold;
	}

	// Token: 0x060030CB RID: 12491 RVA: 0x00119950 File Offset: 0x00117B50
	public void SetSliderValue(float value, int index)
	{
		this.particleThreshold = value;
	}

	// Token: 0x060030CC RID: 12492 RVA: 0x00119959 File Offset: 0x00117B59
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x00119960 File Offset: 0x00117B60
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
	}

	// Token: 0x04001D1C RID: 7452
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04001D1D RID: 7453
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001D1E RID: 7454
	[MyCmpGet]
	private EnergyConsumer energyConsumer;

	// Token: 0x04001D1F RID: 7455
	private float recentPerSecondConsumptionRate;

	// Token: 0x04001D20 RID: 7456
	public int minSlider;

	// Token: 0x04001D21 RID: 7457
	public int maxSlider;

	// Token: 0x04001D22 RID: 7458
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04001D23 RID: 7459
	public float minLaunchInterval;

	// Token: 0x04001D24 RID: 7460
	public float radiationSampleRate;

	// Token: 0x04001D25 RID: 7461
	[Serialize]
	public float particleThreshold = 50f;

	// Token: 0x04001D26 RID: 7462
	private EightDirectionController directionController;

	// Token: 0x04001D27 RID: 7463
	private float launcherTimer;

	// Token: 0x04001D28 RID: 7464
	private float radiationSampleTimer;

	// Token: 0x04001D29 RID: 7465
	private MeterController particleController;

	// Token: 0x04001D2A RID: 7466
	private bool particleVisualPlaying;

	// Token: 0x04001D2B RID: 7467
	private MeterController progressMeterController;

	// Token: 0x04001D2C RID: 7468
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x04001D2D RID: 7469
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001D2E RID: 7470
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleSpawner>(delegate(HighEnergyParticleSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200168C RID: 5772
	public class StatesInstance : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.GameInstance
	{
		// Token: 0x060097A8 RID: 38824 RVA: 0x00385813 File Offset: 0x00383A13
		public StatesInstance(HighEnergyParticleSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x0200168D RID: 5773
	public class States : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner>
	{
		// Token: 0x060097A9 RID: 38825 RVA: 0x0038581C File Offset: 0x00383A1C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false).DefaultState(this.inoperational.empty);
			this.inoperational.empty.EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.losing, (HighEnergyParticleSpawner.StatesInstance smi) => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.inoperational.losing.ToggleStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.DoConsumeParticlesWhileDisabled(dt);
			}, UpdateRate.SIM_1000ms, false).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.empty, (HighEnergyParticleSpawner.StatesInstance smi) => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
			this.ready.idle.ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.absorbing, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsTrue).PlayAnim("on");
			this.ready.absorbing.Enter("SetActive(true)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.idle, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.CollectingHEP, (HighEnergyParticleSpawner.StatesInstance smi) => smi.master).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x0400752D RID: 29997
		public StateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x0400752E RID: 29998
		public HighEnergyParticleSpawner.States.ReadyStates ready;

		// Token: 0x0400752F RID: 29999
		public HighEnergyParticleSpawner.States.InoperationalStates inoperational;

		// Token: 0x020028FF RID: 10495
		public class InoperationalStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x0400B51E RID: 46366
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State empty;

			// Token: 0x0400B51F RID: 46367
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State losing;
		}

		// Token: 0x02002900 RID: 10496
		public class ReadyStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x0400B520 RID: 46368
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State idle;

			// Token: 0x0400B521 RID: 46369
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State absorbing;
		}
	}
}
