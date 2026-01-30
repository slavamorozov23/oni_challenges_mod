using System;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000C1A RID: 3098
public class WarmBlooded : StateMachineComponent<WarmBlooded.StatesInstance>
{
	// Token: 0x06005D22 RID: 23842 RVA: 0x0021B55F File Offset: 0x0021975F
	public static bool IsCold(WarmBlooded.StatesInstance smi)
	{
		return !smi.IsSimpleHeatProducer() && smi.IsCold();
	}

	// Token: 0x06005D23 RID: 23843 RVA: 0x0021B571 File Offset: 0x00219771
	public static bool IsHot(WarmBlooded.StatesInstance smi)
	{
		return !smi.IsSimpleHeatProducer() && smi.IsHot();
	}

	// Token: 0x06005D24 RID: 23844 RVA: 0x0021B584 File Offset: 0x00219784
	public static void WarmingRegulator(WarmBlooded.StatesInstance smi, float dt)
	{
		PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
		float num = SimUtil.EnergyFlowToTemperatureDelta(smi.master.CoolingKW, component.Element.specificHeatCapacity, component.Mass);
		float num2 = smi.IdealTemperature - smi.BodyTemperature;
		float num3 = 1f;
		if ((num - smi.baseTemperatureModification.Value) * dt < num2)
		{
			num3 = Mathf.Clamp(num2 / ((num - smi.baseTemperatureModification.Value) * dt), 0f, 1f);
		}
		smi.bodyRegulator.SetValue(-num * num3);
		if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue(-smi.master.CoolingKW * num3 / smi.master.KCal2Joules);
		}
	}

	// Token: 0x06005D25 RID: 23845 RVA: 0x0021B648 File Offset: 0x00219848
	public static void CoolingRegulator(WarmBlooded.StatesInstance smi, float dt)
	{
		PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
		float num = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component.Element.specificHeatCapacity, component.Mass);
		float num2 = SimUtil.EnergyFlowToTemperatureDelta(smi.master.WarmingKW, component.Element.specificHeatCapacity, component.Mass);
		float num3 = smi.IdealTemperature - smi.BodyTemperature;
		float num4 = 1f;
		if (num2 + num > num3)
		{
			num4 = Mathf.Max(0f, num3 - num) / num2;
		}
		smi.bodyRegulator.SetValue(num2 * num4);
		if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue(-smi.master.WarmingKW * num4 * 1000f / smi.master.KCal2Joules);
		}
	}

	// Token: 0x06005D26 RID: 23846 RVA: 0x0021B71A File Offset: 0x0021991A
	protected override void OnPrefabInit()
	{
		this.temperature = Db.Get().Amounts.Get(this.TemperatureAmountName).Lookup(base.gameObject);
		this.primaryElement = base.GetComponent<PrimaryElement>();
	}

	// Token: 0x06005D27 RID: 23847 RVA: 0x0021B74E File Offset: 0x0021994E
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06005D28 RID: 23848 RVA: 0x0021B75B File Offset: 0x0021995B
	public void SetTemperatureImmediate(float t)
	{
		this.temperature.value = t;
	}

	// Token: 0x04003E08 RID: 15880
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04003E09 RID: 15881
	public AmountInstance temperature;

	// Token: 0x04003E0A RID: 15882
	private PrimaryElement primaryElement;

	// Token: 0x04003E0B RID: 15883
	public WarmBlooded.ComplexityType complexity = WarmBlooded.ComplexityType.FullHomeostasis;

	// Token: 0x04003E0C RID: 15884
	public string TemperatureAmountName = "Temperature";

	// Token: 0x04003E0D RID: 15885
	public float IdealTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;

	// Token: 0x04003E0E RID: 15886
	public float BaseGenerationKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS;

	// Token: 0x04003E0F RID: 15887
	public string BaseTemperatureModifierDescription = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x04003E10 RID: 15888
	public float KCal2Joules = DUPLICANTSTATS.STANDARD.BaseStats.KCAL2JOULES;

	// Token: 0x04003E11 RID: 15889
	public float WarmingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS;

	// Token: 0x04003E12 RID: 15890
	public float CoolingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS;

	// Token: 0x04003E13 RID: 15891
	public string CaloriesModifierDescription = DUPLICANTS.MODIFIERS.BURNINGCALORIES.NAME;

	// Token: 0x04003E14 RID: 15892
	public string BodyRegulatorModifierDescription = DUPLICANTS.MODIFIERS.HOMEOSTASIS.NAME;

	// Token: 0x04003E15 RID: 15893
	public const float TRANSITION_DELAY_HOT = 3f;

	// Token: 0x04003E16 RID: 15894
	public const float TRANSITION_DELAY_COLD = 3f;

	// Token: 0x02001DAF RID: 7599
	public enum ComplexityType
	{
		// Token: 0x04008BF7 RID: 35831
		SimpleHeatProduction,
		// Token: 0x04008BF8 RID: 35832
		HomeostasisWithoutCaloriesImpact,
		// Token: 0x04008BF9 RID: 35833
		FullHomeostasis
	}

	// Token: 0x02001DB0 RID: 7600
	public class StatesInstance : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.GameInstance
	{
		// Token: 0x0600B1D7 RID: 45527 RVA: 0x003DE188 File Offset: 0x003DC388
		public StatesInstance(WarmBlooded smi) : base(smi)
		{
			this.baseTemperatureModification = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BaseTemperatureModifierDescription, false, true, false);
			base.master.GetAttributes().Add(this.baseTemperatureModification);
			if (base.master.complexity != WarmBlooded.ComplexityType.SimpleHeatProduction)
			{
				this.bodyRegulator = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BodyRegulatorModifierDescription, false, true, false);
				base.master.GetAttributes().Add(this.bodyRegulator);
			}
			if (base.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
			{
				this.burningCalories = new AttributeModifier("CaloriesDelta", 0f, base.master.CaloriesModifierDescription, false, false, false);
				base.master.GetAttributes().Add(this.burningCalories);
			}
			base.master.SetTemperatureImmediate(this.IdealTemperature);
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600B1D8 RID: 45528 RVA: 0x003DE293 File Offset: 0x003DC493
		public float IdealTemperature
		{
			get
			{
				return base.master.IdealTemperature;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x0600B1D9 RID: 45529 RVA: 0x003DE2A0 File Offset: 0x003DC4A0
		public float TemperatureDelta
		{
			get
			{
				return this.bodyRegulator.Value;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x0600B1DA RID: 45530 RVA: 0x003DE2AD File Offset: 0x003DC4AD
		public float BodyTemperature
		{
			get
			{
				return base.master.primaryElement.Temperature;
			}
		}

		// Token: 0x0600B1DB RID: 45531 RVA: 0x003DE2BF File Offset: 0x003DC4BF
		public bool IsSimpleHeatProducer()
		{
			return base.master.complexity == WarmBlooded.ComplexityType.SimpleHeatProduction;
		}

		// Token: 0x0600B1DC RID: 45532 RVA: 0x003DE2CF File Offset: 0x003DC4CF
		public bool IsHot()
		{
			return this.BodyTemperature > this.IdealTemperature;
		}

		// Token: 0x0600B1DD RID: 45533 RVA: 0x003DE2DF File Offset: 0x003DC4DF
		public bool IsCold()
		{
			return this.BodyTemperature < this.IdealTemperature;
		}

		// Token: 0x04008BFA RID: 35834
		public AttributeModifier baseTemperatureModification;

		// Token: 0x04008BFB RID: 35835
		public AttributeModifier bodyRegulator;

		// Token: 0x04008BFC RID: 35836
		public AttributeModifier burningCalories;
	}

	// Token: 0x02001DB1 RID: 7601
	public class States : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded>
	{
		// Token: 0x0600B1DE RID: 45534 RVA: 0x003DE2F0 File Offset: 0x003DC4F0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.alive.normal;
			this.root.TagTransition(GameTags.Dead, this.dead, false).Enter(delegate(WarmBlooded.StatesInstance smi)
			{
				PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
				float value = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component.Element.specificHeatCapacity, component.Mass);
				smi.baseTemperatureModification.SetValue(value);
				CreatureSimTemperatureTransfer component2 = smi.master.GetComponent<CreatureSimTemperatureTransfer>();
				component2.NonSimTemperatureModifiers.Add(smi.baseTemperatureModification);
				if (!smi.IsSimpleHeatProducer())
				{
					component2.NonSimTemperatureModifiers.Add(smi.bodyRegulator);
				}
			});
			this.alive.normal.Transition(this.alive.cold.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold), UpdateRate.SIM_200ms).Transition(this.alive.hot.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot), UpdateRate.SIM_200ms);
			this.alive.cold.transition.ScheduleGoTo(3f, this.alive.cold.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)), UpdateRate.SIM_200ms);
			this.alive.cold.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)), UpdateRate.SIM_200ms).Update("ColdRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.CoolingRegulator), UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
				if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
				{
					smi.burningCalories.SetValue(0f);
				}
			});
			this.alive.hot.transition.ScheduleGoTo(3f, this.alive.hot.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot)), UpdateRate.SIM_200ms);
			this.alive.hot.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot)), UpdateRate.SIM_200ms).Update("WarmRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.WarmingRegulator), UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
			});
			this.dead.Enter(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.master.enabled = false;
			});
		}

		// Token: 0x04008BFD RID: 35837
		public WarmBlooded.States.AliveState alive;

		// Token: 0x04008BFE RID: 35838
		public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State dead;

		// Token: 0x02002A4E RID: 10830
		public class RegulatingState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x0400BAF4 RID: 47860
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State transition;

			// Token: 0x0400BAF5 RID: 47861
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State regulating;
		}

		// Token: 0x02002A4F RID: 10831
		public class AliveState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x0400BAF6 RID: 47862
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State normal;

			// Token: 0x0400BAF7 RID: 47863
			public WarmBlooded.States.RegulatingState cold;

			// Token: 0x0400BAF8 RID: 47864
			public WarmBlooded.States.RegulatingState hot;
		}
	}
}
