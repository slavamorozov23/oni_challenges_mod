using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A2B RID: 2603
public class GunkMonitor : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>
{
	// Token: 0x06004C11 RID: 19473 RVA: 0x001BA36C File Offset: 0x001B856C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Update(new Action<GunkMonitor.Instance, float>(GunkMonitor.GunkAmountWatcherUpdate), UpdateRate.SIM_200ms, false);
		this.idle.OnSignal(this.gunkValueChangedSignal, this.mildUrge, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.IsGunkLevelsOverMildUrgeThreshold));
		this.mildUrge.OnSignal(this.gunkValueChangedSignal, this.criticalUrge, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold)).OnSignal(this.gunkValueChangedSignal, this.idle, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.DoesNotWantToExpellGunk)).DefaultState(this.mildUrge.prevented);
		this.mildUrge.prevented.ScheduleChange(this.mildUrge.allowed, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling));
		this.mildUrge.allowed.ScheduleChange(this.mildUrge.prevented, GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Not(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling))).ToggleUrge(Db.Get().Urges.Pee).ToggleUrge(Db.Get().Urges.GunkPee);
		this.criticalUrge.OnSignal(this.gunkValueChangedSignal, this.idle, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.DoesNotWantToExpellGunk)).OnSignal(this.gunkValueChangedSignal, this.mildUrge, (GunkMonitor.Instance smi, StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter param) => !GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold(smi)).OnSignal(this.gunkValueChangedSignal, this.cantHold, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.CanNotHoldGunkAnymore)).ToggleUrge(Db.Get().Urges.GunkPee).ToggleUrge(Db.Get().Urges.Pee).ToggleEffect("GunkSick").ToggleExpression(Db.Get().Expressions.FullBladder, null).ToggleThought(Db.Get().Thoughts.ExpellGunkDesire, null).ToggleAnims("anim_loco_walk_slouch_kanim", 0f).ToggleAnims("anim_idle_slouch_kanim", 0f);
		this.cantHold.ToggleUrge(Db.Get().Urges.GunkPee).ToggleThought(Db.Get().Thoughts.ExpellingGunk, null).ToggleChore((GunkMonitor.Instance smi) => new BionicGunkSpillChore(smi.master), this.emptyRemaining);
		this.emptyRemaining.Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ExpellAllGunk)).Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ApplyGunkHungoverEffect)).GoTo(this.idle);
	}

	// Token: 0x06004C12 RID: 19474 RVA: 0x001BA60D File Offset: 0x001B880D
	public static bool IsGunkLevelsOverCriticalUrgeThreshold(GunkMonitor.Instance smi, StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter param)
	{
		return GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold(smi);
	}

	// Token: 0x06004C13 RID: 19475 RVA: 0x001BA615 File Offset: 0x001B8815
	public static bool IsGunkLevelsOverCriticalUrgeThreshold(GunkMonitor.Instance smi)
	{
		return smi.CurrentGunkPercentage >= smi.def.DesperetlySeekForGunkToiletTreshold;
	}

	// Token: 0x06004C14 RID: 19476 RVA: 0x001BA62D File Offset: 0x001B882D
	public static bool IsGunkLevelsOverMildUrgeThreshold(GunkMonitor.Instance smi, StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter param)
	{
		return GunkMonitor.IsGunkLevelsOverMildUrgeThreshold(smi);
	}

	// Token: 0x06004C15 RID: 19477 RVA: 0x001BA635 File Offset: 0x001B8835
	public static bool IsGunkLevelsOverMildUrgeThreshold(GunkMonitor.Instance smi)
	{
		return smi.CurrentGunkPercentage >= smi.def.SeekForGunkToiletTreshold_InSchedule;
	}

	// Token: 0x06004C16 RID: 19478 RVA: 0x001BA64D File Offset: 0x001B884D
	public static bool ScheduleAllowsExpelling(GunkMonitor.Instance smi)
	{
		return smi.DoesCurrentScheduleAllowsGunkToilet;
	}

	// Token: 0x06004C17 RID: 19479 RVA: 0x001BA655 File Offset: 0x001B8855
	public static bool DoesNotWantToExpellGunk(GunkMonitor.Instance smi, StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter p)
	{
		return !GunkMonitor.IsGunkLevelsOverMildUrgeThreshold(smi);
	}

	// Token: 0x06004C18 RID: 19480 RVA: 0x001BA660 File Offset: 0x001B8860
	public static bool CanNotHoldGunkAnymore(GunkMonitor.Instance smi, StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter p)
	{
		return GunkMonitor.CanNotHoldGunkAnymore(smi);
	}

	// Token: 0x06004C19 RID: 19481 RVA: 0x001BA668 File Offset: 0x001B8868
	public static bool CanNotHoldGunkAnymore(GunkMonitor.Instance smi)
	{
		return smi.IsGunkBuildupAtMax;
	}

	// Token: 0x06004C1A RID: 19482 RVA: 0x001BA670 File Offset: 0x001B8870
	public static void ExpellAllGunk(GunkMonitor.Instance smi)
	{
		smi.ExpellAllGunk(null);
	}

	// Token: 0x06004C1B RID: 19483 RVA: 0x001BA679 File Offset: 0x001B8879
	public static void ApplyGunkHungoverEffect(GunkMonitor.Instance smi)
	{
		smi.GetComponent<Effects>().Add("GunkHungover", true);
	}

	// Token: 0x06004C1C RID: 19484 RVA: 0x001BA68D File Offset: 0x001B888D
	public static void GunkAmountWatcherUpdate(GunkMonitor.Instance smi, float dt)
	{
		smi.GunkAmountWatcherUpdate(dt);
	}

	// Token: 0x0400327E RID: 12926
	public const float BIONIC_RADS_REMOVED_WHEN_PEE = 300f;

	// Token: 0x0400327F RID: 12927
	public static readonly float GUNK_CAPACITY = 80f;

	// Token: 0x04003280 RID: 12928
	public const string GUNK_FULL_EFFECT_NAME = "GunkSick";

	// Token: 0x04003281 RID: 12929
	public const string GUNK_HUNGOVER_EFFECT_NAME = "GunkHungover";

	// Token: 0x04003282 RID: 12930
	public static SimHashes GunkElement = SimHashes.LiquidGunk;

	// Token: 0x04003283 RID: 12931
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State idle;

	// Token: 0x04003284 RID: 12932
	public GunkMonitor.MildUrgeStates mildUrge;

	// Token: 0x04003285 RID: 12933
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State criticalUrge;

	// Token: 0x04003286 RID: 12934
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State cantHold;

	// Token: 0x04003287 RID: 12935
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State emptyRemaining;

	// Token: 0x04003288 RID: 12936
	public StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Signal gunkValueChangedSignal;

	// Token: 0x02001AED RID: 6893
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008343 RID: 33603
		public float SeekForGunkToiletTreshold_InSchedule = 0.6f;

		// Token: 0x04008344 RID: 33604
		public float DesperetlySeekForGunkToiletTreshold = 0.9f;
	}

	// Token: 0x02001AEE RID: 6894
	public class MildUrgeStates : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State
	{
		// Token: 0x04008345 RID: 33605
		public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State allowed;

		// Token: 0x04008346 RID: 33606
		public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State prevented;
	}

	// Token: 0x02001AEF RID: 6895
	public new class Instance : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.GameInstance
	{
		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x0600A7BC RID: 42940 RVA: 0x003BD872 File Offset: 0x003BBA72
		public bool HasGunk
		{
			get
			{
				return this.CurrentGunkMass > 0f;
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x0600A7BD RID: 42941 RVA: 0x003BD881 File Offset: 0x003BBA81
		public bool IsGunkBuildupAtMax
		{
			get
			{
				return this.CurrentGunkPercentage >= 1f;
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x0600A7BE RID: 42942 RVA: 0x003BD893 File Offset: 0x003BBA93
		public float CurrentGunkMass
		{
			get
			{
				if (this.gunkAmount != null)
				{
					return this.gunkAmount.value;
				}
				return 0f;
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x0600A7BF RID: 42943 RVA: 0x003BD8AE File Offset: 0x003BBAAE
		public float CurrentGunkPercentage
		{
			get
			{
				return this.CurrentGunkMass / this.gunkAmount.GetMax();
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x0600A7C0 RID: 42944 RVA: 0x003BD8C2 File Offset: 0x003BBAC2
		public bool DoesCurrentScheduleAllowsGunkToilet
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat) || this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
			}
		}

		// Token: 0x0600A7C1 RID: 42945 RVA: 0x003BD8FC File Offset: 0x003BBAFC
		public Instance(IStateMachineTarget master, GunkMonitor.Def def) : base(master, def)
		{
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.gunkAmount = Db.Get().Amounts.BionicGunk.Lookup(base.gameObject);
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x0600A7C2 RID: 42946 RVA: 0x003BD960 File Offset: 0x003BBB60
		public override void StartSM()
		{
			this.oilMonitor = base.gameObject.GetSMI<BionicOilMonitor.Instance>();
			BionicOilMonitor.Instance instance = this.oilMonitor;
			instance.OnOilValueChanged = (Action<float>)Delegate.Combine(instance.OnOilValueChanged, new Action<float>(this.OnOilValueChanged));
			this.LastAmountOfGunkObserved = this.CurrentGunkMass;
			base.StartSM();
		}

		// Token: 0x0600A7C3 RID: 42947 RVA: 0x003BD9B7 File Offset: 0x003BBBB7
		public void GunkAmountWatcherUpdate(float dt)
		{
			if (this.LastAmountOfGunkObserved != this.CurrentGunkMass)
			{
				this.LastAmountOfGunkObserved = this.CurrentGunkMass;
				base.sm.gunkValueChangedSignal.Trigger(this);
			}
		}

		// Token: 0x0600A7C4 RID: 42948 RVA: 0x003BD9E4 File Offset: 0x003BBBE4
		protected override void OnCleanUp()
		{
			if (this.oilMonitor != null)
			{
				BionicOilMonitor.Instance instance = this.oilMonitor;
				instance.OnOilValueChanged = (Action<float>)Delegate.Remove(instance.OnOilValueChanged, new Action<float>(this.OnOilValueChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x0600A7C5 RID: 42949 RVA: 0x003BDA1C File Offset: 0x003BBC1C
		private void OnOilValueChanged(float delta)
		{
			float num = (delta < 0f) ? Mathf.Abs(delta) : 0f;
			float gunkMassValue = Mathf.Clamp(this.CurrentGunkMass + num, 0f, this.gunkAmount.GetMax());
			this.SetGunkMassValue(gunkMassValue);
		}

		// Token: 0x0600A7C6 RID: 42950 RVA: 0x003BDA64 File Offset: 0x003BBC64
		public void SetGunkMassValue(float value)
		{
			float currentGunkMass = this.CurrentGunkMass;
			this.gunkAmount.SetValue(value);
			this.LastAmountOfGunkObserved = this.CurrentGunkMass;
			base.sm.gunkValueChangedSignal.Trigger(this);
		}

		// Token: 0x0600A7C7 RID: 42951 RVA: 0x003BDA98 File Offset: 0x003BBC98
		public void ExpellGunk(float mass, Storage targetStorage = null)
		{
			if (this.HasGunk)
			{
				float currentGunkMass = this.CurrentGunkMass;
				float num = Mathf.Min(mass, this.CurrentGunkMass);
				num = Mathf.Max(num, Mathf.Epsilon);
				int gameCell = Grid.PosToCell(base.transform.position);
				byte index = Db.Get().Diseases.GetIndex(DUPLICANTSTATS.BIONICS.Secretions.PEE_DISEASE);
				float num2 = num / GunkMonitor.GUNK_CAPACITY;
				if (targetStorage != null)
				{
					targetStorage.AddLiquid(GunkMonitor.GunkElement, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), false, true);
				}
				else
				{
					Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						equippable.GetComponent<Storage>().AddLiquid(GunkMonitor.GunkElement, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), false, true);
					}
					else
					{
						SimMessages.AddRemoveSubstance(gameCell, GunkMonitor.GunkElement, CellEventLogger.Instance.Vomit, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), true, -1);
					}
				}
				if (Sim.IsRadiationEnabled())
				{
					MinionIdentity component = base.transform.GetComponent<MinionIdentity>();
					AmountInstance amountInstance = Db.Get().Amounts.RadiationBalance.Lookup(component);
					RadiationMonitor.Instance smi = component.GetSMI<RadiationMonitor.Instance>();
					float num3 = DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND / DUPLICANTSTATS.BIONICS.BaseStats.BLADDER_INCREASE_PER_SECOND;
					float num4 = Math.Min(amountInstance.value, 300f * num3 * smi.difficultySettingMod * num2);
					if (num4 >= 1f)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double)num4).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, component.transform, Vector3.up * 2f, 1.5f, false, false);
					}
					amountInstance.ApplyDelta(-num4);
				}
				this.SetGunkMassValue(Mathf.Clamp(this.CurrentGunkMass - num, 0f, this.gunkAmount.GetMax()));
			}
		}

		// Token: 0x0600A7C8 RID: 42952 RVA: 0x003BDCCA File Offset: 0x003BBECA
		public void ExpellAllGunk(Storage targetStorage = null)
		{
			this.ExpellGunk(this.CurrentGunkMass, targetStorage);
		}

		// Token: 0x04008347 RID: 33607
		private float LastAmountOfGunkObserved;

		// Token: 0x04008348 RID: 33608
		private BionicOilMonitor.Instance oilMonitor;

		// Token: 0x04008349 RID: 33609
		private AmountInstance gunkAmount;

		// Token: 0x0400834A RID: 33610
		private AmountInstance bodyTemperature;

		// Token: 0x0400834B RID: 33611
		private Schedulable schedulable;
	}
}
