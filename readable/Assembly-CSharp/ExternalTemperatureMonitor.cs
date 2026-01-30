using System;
using Klei.AI;
using TUNING;

// Token: 0x02000A25 RID: 2597
public class ExternalTemperatureMonitor : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance>
{
	// Token: 0x06004BF0 RID: 19440 RVA: 0x001B9469 File Offset: 0x001B7669
	public static float GetExternalColdThreshold(Attributes affected_attributes)
	{
		return -0.039f;
	}

	// Token: 0x06004BF1 RID: 19441 RVA: 0x001B9470 File Offset: 0x001B7670
	public static float GetExternalWarmThreshold(Attributes affected_attributes)
	{
		return 0.008f;
	}

	// Token: 0x06004BF2 RID: 19442 RVA: 0x001B9478 File Offset: 0x001B7678
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.comfortable.Transition(this.transitionToTooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).Transition(this.transitionToTooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms);
		this.transitionToTooWarm.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot(), UpdateRate.SIM_200ms).Transition(this.tooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.transitionToTooCool.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold(), UpdateRate.SIM_200ms).Transition(this.tooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.tooWarm.ToggleTag(GameTags.FeelingWarm).Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.EffectAdded, this.comfortable, (ExternalTemperatureMonitor.Instance smi, object obj) => !smi.IsTooHot()).Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
		this.tooCool.ToggleTag(GameTags.FeelingCold).Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.EffectAdded, this.comfortable, (ExternalTemperatureMonitor.Instance smi, object obj) => !smi.IsTooCold()).Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
	}

	// Token: 0x04003259 RID: 12889
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State comfortable;

	// Token: 0x0400325A RID: 12890
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooWarm;

	// Token: 0x0400325B RID: 12891
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooWarm;

	// Token: 0x0400325C RID: 12892
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooCool;

	// Token: 0x0400325D RID: 12893
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooCool;

	// Token: 0x0400325E RID: 12894
	private const float BODY_TEMPERATURE_AFFECT_EXTERNAL_FEEL_THRESHOLD = 0.5f;

	// Token: 0x0400325F RID: 12895
	public const string CHILLY_SURROUNDINGS_EFFECT_NAME = "ColdAir";

	// Token: 0x04003260 RID: 12896
	public const string TOASTY_SURROUNDINGS_EFFECT_NAME = "WarmAir";

	// Token: 0x04003261 RID: 12897
	public static readonly float BASE_STRESS_TOLERANCE_COLD = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS * 0.2f;

	// Token: 0x04003262 RID: 12898
	public static readonly float BASE_STRESS_TOLERANCE_WARM = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS * 0.2f;

	// Token: 0x04003263 RID: 12899
	private const float START_GAME_AVERAGING_DELAY = 6f;

	// Token: 0x04003264 RID: 12900
	private const float TRANSITION_TO_DELAY = 1f;

	// Token: 0x04003265 RID: 12901
	private const float TRANSITION_OUT_DELAY = 6f;

	// Token: 0x02001ADC RID: 6876
	public new class Instance : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x0600A759 RID: 42841 RVA: 0x003BC08E File Offset: 0x003BA28E
		public float GetCurrentColdThreshold
		{
			get
			{
				if (this.internalTemperatureMonitor.IdealTemperatureDelta() > 0.5f)
				{
					return 0f;
				}
				return CreatureSimTemperatureTransfer.PotentialEnergyFlowToCreature(Grid.PosToCell(base.gameObject), this.primaryElement, this.temperatureTransferer, 1f);
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x0600A75A RID: 42842 RVA: 0x003BC0C9 File Offset: 0x003BA2C9
		public float GetCurrentHotThreshold
		{
			get
			{
				return this.HotThreshold;
			}
		}

		// Token: 0x0600A75B RID: 42843 RVA: 0x003BC0D4 File Offset: 0x003BA2D4
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.attributes = base.gameObject.GetAttributes();
			this.internalTemperatureMonitor = base.gameObject.GetSMI<TemperatureMonitor.Instance>();
			this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.temperatureTransferer = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			this.effects = base.gameObject.GetComponent<Effects>();
			this.traits = base.gameObject.GetComponent<Traits>();
		}

		// Token: 0x0600A75C RID: 42844 RVA: 0x003BC1E8 File Offset: 0x003BA3E8
		public bool IsTooHot()
		{
			return !this.effects.HasEffect("RefreshingTouch") && !this.effects.HasImmunityTo(this.warmAirEffect) && this.temperatureTransferer.LastTemperatureRecordIsReliable && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage > ExternalTemperatureMonitor.GetExternalWarmThreshold(base.smi.attributes);
		}

		// Token: 0x0600A75D RID: 42845 RVA: 0x003BC258 File Offset: 0x003BA458
		public bool IsTooCold()
		{
			for (int i = 0; i < this.immunityToColdEffects.Length; i++)
			{
				if (this.effects.HasEffect(this.immunityToColdEffects[i]))
				{
					return false;
				}
			}
			return !this.effects.HasImmunityTo(this.coldAirEffect) && (!(this.traits != null) || !this.traits.IsEffectIgnored(this.coldAirEffect)) && !WarmthProvider.IsWarmCell(Grid.PosToCell(this)) && this.temperatureTransferer.LastTemperatureRecordIsReliable && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage < ExternalTemperatureMonitor.GetExternalColdThreshold(base.smi.attributes);
		}

		// Token: 0x040082E8 RID: 33512
		public float HotThreshold = 306.15f;

		// Token: 0x040082E9 RID: 33513
		public Effects effects;

		// Token: 0x040082EA RID: 33514
		public Traits traits;

		// Token: 0x040082EB RID: 33515
		public Attributes attributes;

		// Token: 0x040082EC RID: 33516
		public AmountInstance internalTemperature;

		// Token: 0x040082ED RID: 33517
		private TemperatureMonitor.Instance internalTemperatureMonitor;

		// Token: 0x040082EE RID: 33518
		public CreatureSimTemperatureTransfer temperatureTransferer;

		// Token: 0x040082EF RID: 33519
		public PrimaryElement primaryElement;

		// Token: 0x040082F0 RID: 33520
		private Effect warmAirEffect = Db.Get().effects.Get("WarmAir");

		// Token: 0x040082F1 RID: 33521
		private Effect coldAirEffect = Db.Get().effects.Get("ColdAir");

		// Token: 0x040082F2 RID: 33522
		private Effect[] immunityToColdEffects = new Effect[]
		{
			Db.Get().effects.Get("WarmTouch"),
			Db.Get().effects.Get("WarmTouchFood")
		};
	}
}
