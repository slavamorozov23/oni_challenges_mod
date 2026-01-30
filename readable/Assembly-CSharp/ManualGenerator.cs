using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007B1 RID: 1969
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/ManualGenerator")]
public class ManualGenerator : RemoteWorkable, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700032C RID: 812
	// (get) Token: 0x060033E1 RID: 13281 RVA: 0x00126EA3 File Offset: 0x001250A3
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TITLE";
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x060033E2 RID: 13282 RVA: 0x00126EAA File Offset: 0x001250AA
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x060033E3 RID: 13283 RVA: 0x00126EB6 File Offset: 0x001250B6
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x00126EB9 File Offset: 0x001250B9
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x060033E5 RID: 13285 RVA: 0x00126EC0 File Offset: 0x001250C0
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x060033E6 RID: 13286 RVA: 0x00126EC7 File Offset: 0x001250C7
	public float GetSliderValue(int index)
	{
		return this.batteryRefillPercent * 100f;
	}

	// Token: 0x060033E7 RID: 13287 RVA: 0x00126ED5 File Offset: 0x001250D5
	public void SetSliderValue(float value, int index)
	{
		this.batteryRefillPercent = value / 100f;
	}

	// Token: 0x060033E8 RID: 13288 RVA: 0x00126EE4 File Offset: 0x001250E4
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP";
	}

	// Token: 0x060033E9 RID: 13289 RVA: 0x00126EEB File Offset: 0x001250EB
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP"), this.batteryRefillPercent * 100f);
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x060033EA RID: 13290 RVA: 0x00126F12 File Offset: 0x00125112
	public bool IsPowered
	{
		get
		{
			return this.operational.IsActive;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x060033EB RID: 13291 RVA: 0x00126F1F File Offset: 0x0012511F
	public override Chore RemoteDockChore
	{
		get
		{
			return this.chore;
		}
	}

	// Token: 0x060033EC RID: 13292 RVA: 0x00126F27 File Offset: 0x00125127
	private ManualGenerator()
	{
		this.showProgressBar = false;
	}

	// Token: 0x060033ED RID: 13293 RVA: 0x00126F44 File Offset: 0x00125144
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ManualGenerator>(-592767678, ManualGenerator.OnOperationalChangedDelegate);
		base.Subscribe<ManualGenerator>(824508782, ManualGenerator.OnActiveChangedDelegate);
		base.Subscribe<ManualGenerator>(-905833192, ManualGenerator.OnCopySettingsDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.GeneratingPower;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		EnergyGenerator.EnsureStatusItemAvailable();
	}

	// Token: 0x060033EE RID: 13294 RVA: 0x00126FEC File Offset: 0x001251EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		foreach (KAnimHashedString symbol in ManualGenerator.symbol_names)
		{
			component.SetSymbolVisiblity(symbol, false);
		}
		Building component2 = base.GetComponent<Building>();
		this.powerCell = component2.GetPowerOutputCell();
		this.OnActiveChanged(null);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_generatormanual_kanim")
		};
		this.smi = new ManualGenerator.GeneratePowerSM.Instance(this);
		this.smi.StartSM();
		Game.Instance.energySim.AddManualGenerator(this);
	}

	// Token: 0x060033EF RID: 13295 RVA: 0x00127096 File Offset: 0x00125296
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveManualGenerator(this);
		this.smi.StopSM("cleanup");
		base.OnCleanUp();
	}

	// Token: 0x060033F0 RID: 13296 RVA: 0x001270BE File Offset: 0x001252BE
	protected void OnActiveChanged(object is_active)
	{
		if (this.operational.IsActive)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ManualGeneratorChargingUp, null);
		}
	}

	// Token: 0x060033F1 RID: 13297 RVA: 0x001270F8 File Offset: 0x001252F8
	private void OnCopySettings(object data)
	{
		GameObject gameObject = data as GameObject;
		if (gameObject != null)
		{
			ManualGenerator component = gameObject.GetComponent<ManualGenerator>();
			if (component != null)
			{
				this.batteryRefillPercent = component.batteryRefillPercent;
			}
		}
	}

	// Token: 0x060033F2 RID: 13298 RVA: 0x0012712C File Offset: 0x0012532C
	public void EnergySim200ms(float dt)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.operational.IsActive)
		{
			this.generator.GenerateJoules(this.generator.WattageRating * dt, false);
			component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this.generator);
			return;
		}
		this.generator.ResetJoules();
		component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.GeneratorOffline, null);
		if (this.operational.IsOperational)
		{
			CircuitManager circuitManager = Game.Instance.circuitManager;
			if (circuitManager == null)
			{
				return;
			}
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag = circuitManager.HasBatteries(circuitID);
			bool flag2 = false;
			if (!flag && circuitManager.HasConsumers(circuitID))
			{
				flag2 = true;
			}
			else if (flag)
			{
				if (this.batteryRefillPercent <= 0f && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) <= 0f)
				{
					flag2 = true;
				}
				else if (circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < this.batteryRefillPercent)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				if (this.chore == null && this.smi.GetCurrentState() == this.smi.sm.on)
				{
					this.chore = new WorkChore<ManualGenerator>(Db.Get().ChoreTypes.GeneratePower, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				}
			}
			else if (this.chore != null)
			{
				this.chore.Cancel("No refill needed");
				this.chore = null;
			}
			component.ToggleStatusItem(EnergyGenerator.BatteriesSufficientlyFull, !flag2, null);
		}
	}

	// Token: 0x060033F3 RID: 13299 RVA: 0x001272C8 File Offset: 0x001254C8
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.operational.SetActive(true, false);
	}

	// Token: 0x060033F4 RID: 13300 RVA: 0x001272E0 File Offset: 0x001254E0
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		CircuitManager circuitManager = Game.Instance.circuitManager;
		bool flag = false;
		if (circuitManager != null)
		{
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag2 = circuitManager.HasBatteries(circuitID);
			flag = ((flag2 && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < 1f) || (!flag2 && circuitManager.HasConsumers(circuitID)));
		}
		AttributeLevels component = worker.GetComponent<AttributeLevels>();
		if (component != null)
		{
			component.AddExperience(Db.Get().Attributes.Athletics.Id, dt, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
		}
		return !flag;
	}

	// Token: 0x060033F5 RID: 13301 RVA: 0x0012736C File Offset: 0x0012556C
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.operational.SetActive(false, false);
	}

	// Token: 0x060033F6 RID: 13302 RVA: 0x00127382 File Offset: 0x00125582
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		if (this.chore != null)
		{
			this.chore.Cancel("complete");
			this.chore = null;
		}
	}

	// Token: 0x060033F7 RID: 13303 RVA: 0x001273B0 File Offset: 0x001255B0
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x060033F8 RID: 13304 RVA: 0x001273B3 File Offset: 0x001255B3
	private void OnOperationalChanged(object _)
	{
		if (!this.buildingEnabledButton.IsEnabled)
		{
			this.generator.ResetJoules();
		}
	}

	// Token: 0x04001F4D RID: 8013
	[Serialize]
	[SerializeField]
	private float batteryRefillPercent = 0.5f;

	// Token: 0x04001F4E RID: 8014
	private const float batteryStopRunningPercent = 1f;

	// Token: 0x04001F4F RID: 8015
	[MyCmpReq]
	private Generator generator;

	// Token: 0x04001F50 RID: 8016
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001F51 RID: 8017
	[MyCmpGet]
	private BuildingEnabledButton buildingEnabledButton;

	// Token: 0x04001F52 RID: 8018
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001F53 RID: 8019
	private Chore chore;

	// Token: 0x04001F54 RID: 8020
	private int powerCell;

	// Token: 0x04001F55 RID: 8021
	private ManualGenerator.GeneratePowerSM.Instance smi;

	// Token: 0x04001F56 RID: 8022
	private static readonly KAnimHashedString[] symbol_names = new KAnimHashedString[]
	{
		"meter",
		"meter_target",
		"meter_fill",
		"meter_frame",
		"meter_light",
		"meter_tubing"
	};

	// Token: 0x04001F57 RID: 8023
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001F58 RID: 8024
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x04001F59 RID: 8025
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020016DC RID: 5852
	public class GeneratePowerSM : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance>
	{
		// Token: 0x060098D0 RID: 39120 RVA: 0x00389E34 File Offset: 0x00388034
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).PlayAnim("off");
			this.on.EventTransition(GameHashes.OperationalChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pre, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsActive).PlayAnim("on");
			this.working.DefaultState(this.working.pre);
			this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
			this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => this.masterTarget.Get(smi) != null && !smi.master.GetComponent<Operational>().IsActive);
		}

		// Token: 0x040075FE RID: 30206
		public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State off;

		// Token: 0x040075FF RID: 30207
		public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State on;

		// Token: 0x04007600 RID: 30208
		public ManualGenerator.GeneratePowerSM.WorkingStates working;

		// Token: 0x0200290D RID: 10509
		public class WorkingStates : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400B573 RID: 46451
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pre;

			// Token: 0x0400B574 RID: 46452
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State loop;

			// Token: 0x0400B575 RID: 46453
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pst;
		}

		// Token: 0x0200290E RID: 10510
		public new class Instance : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x0600CF21 RID: 53025 RVA: 0x00433692 File Offset: 0x00431892
			public Instance(IStateMachineTarget master) : base(master)
			{
			}
		}
	}
}
