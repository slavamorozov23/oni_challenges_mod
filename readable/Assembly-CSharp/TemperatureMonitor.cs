using System;
using Klei.AI;
using TUNING;

// Token: 0x02000A51 RID: 2641
public class TemperatureMonitor : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance>
{
	// Token: 0x06004CDF RID: 19679 RVA: 0x001BF57C File Offset: 0x001BD77C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.homeostatic;
		this.root.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.averageTemperature = smi.primaryElement.Temperature;
		}).Update("UpdateTemperature", delegate(TemperatureMonitor.Instance smi, float dt)
		{
			smi.UpdateTemperature(dt);
		}, UpdateRate.SIM_200ms, false);
		this.homeostatic.Transition(this.hyperthermic_pre, (TemperatureMonitor.Instance smi) => smi.IsHyperthermic(), UpdateRate.SIM_200ms).Transition(this.hypothermic_pre, (TemperatureMonitor.Instance smi) => smi.IsHypothermic(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null);
		this.hyperthermic_pre.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.GoTo(this.hyperthermic);
		});
		this.hypothermic_pre.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.GoTo(this.hypothermic);
		});
		this.hyperthermic.Transition(this.homeostatic, (TemperatureMonitor.Instance smi) => !smi.IsHyperthermic(), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.CoolDown);
		this.hypothermic.Transition(this.homeostatic, (TemperatureMonitor.Instance smi) => !smi.IsHypothermic(), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.WarmUp);
	}

	// Token: 0x04003339 RID: 13113
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State homeostatic;

	// Token: 0x0400333A RID: 13114
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic;

	// Token: 0x0400333B RID: 13115
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic;

	// Token: 0x0400333C RID: 13116
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic_pre;

	// Token: 0x0400333D RID: 13117
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic_pre;

	// Token: 0x0400333E RID: 13118
	private const float TEMPERATURE_AVERAGING_RANGE = 4f;

	// Token: 0x0400333F RID: 13119
	public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter warmUpCell;

	// Token: 0x04003340 RID: 13120
	public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter coolDownCell;

	// Token: 0x02001B55 RID: 6997
	public new class Instance : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A96E RID: 43374 RVA: 0x003C1518 File Offset: 0x003BF718
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.primaryElement = base.GetComponent<PrimaryElement>();
			this.temperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.warmUpQuery = new SafetyQuery(Game.Instance.safetyConditions.WarmUpChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.coolDownQuery = new SafetyQuery(Game.Instance.safetyConditions.CoolDownChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = base.GetComponent<Navigator>();
		}

		// Token: 0x0600A96F RID: 43375 RVA: 0x003C15C4 File Offset: 0x003BF7C4
		public void UpdateTemperature(float dt)
		{
			base.smi.averageTemperature *= 1f - dt / 4f;
			base.smi.averageTemperature += base.smi.primaryElement.Temperature * (dt / 4f);
			base.smi.temperature.SetValue(base.smi.averageTemperature);
		}

		// Token: 0x0600A970 RID: 43376 RVA: 0x003C1636 File Offset: 0x003BF836
		public bool IsHyperthermic()
		{
			return this.temperature.value > this.HyperthermiaThreshold;
		}

		// Token: 0x0600A971 RID: 43377 RVA: 0x003C164B File Offset: 0x003BF84B
		public bool IsHypothermic()
		{
			return this.temperature.value < this.HypothermiaThreshold;
		}

		// Token: 0x0600A972 RID: 43378 RVA: 0x003C1660 File Offset: 0x003BF860
		public float ExtremeTemperatureDelta()
		{
			if (this.temperature.value > this.HyperthermiaThreshold)
			{
				return this.temperature.value - this.HyperthermiaThreshold;
			}
			if (this.temperature.value < this.HypothermiaThreshold)
			{
				return this.temperature.value - this.HypothermiaThreshold;
			}
			return 0f;
		}

		// Token: 0x0600A973 RID: 43379 RVA: 0x003C16BE File Offset: 0x003BF8BE
		public float IdealTemperatureDelta()
		{
			return this.temperature.value - DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;
		}

		// Token: 0x0600A974 RID: 43380 RVA: 0x003C16E0 File Offset: 0x003BF8E0
		public int GetWarmUpCell()
		{
			return base.sm.warmUpCell.Get(base.smi);
		}

		// Token: 0x0600A975 RID: 43381 RVA: 0x003C16F8 File Offset: 0x003BF8F8
		public int GetCoolDownCell()
		{
			return base.sm.coolDownCell.Get(base.smi);
		}

		// Token: 0x0600A976 RID: 43382 RVA: 0x003C1710 File Offset: 0x003BF910
		public void UpdateWarmUpCell()
		{
			this.warmUpQuery.Reset();
			this.navigator.RunQuery(this.warmUpQuery);
			base.sm.warmUpCell.Set(this.warmUpQuery.GetResultCell(), base.smi, false);
		}

		// Token: 0x0600A977 RID: 43383 RVA: 0x003C175C File Offset: 0x003BF95C
		public void UpdateCoolDownCell()
		{
			this.coolDownQuery.Reset();
			this.navigator.RunQuery(this.coolDownQuery);
			base.sm.coolDownCell.Set(this.coolDownQuery.GetResultCell(), base.smi, false);
		}

		// Token: 0x04008487 RID: 33927
		public AmountInstance temperature;

		// Token: 0x04008488 RID: 33928
		public PrimaryElement primaryElement;

		// Token: 0x04008489 RID: 33929
		private Navigator navigator;

		// Token: 0x0400848A RID: 33930
		private SafetyQuery warmUpQuery;

		// Token: 0x0400848B RID: 33931
		private SafetyQuery coolDownQuery;

		// Token: 0x0400848C RID: 33932
		public float averageTemperature;

		// Token: 0x0400848D RID: 33933
		public float HypothermiaThreshold = 307.15f;

		// Token: 0x0400848E RID: 33934
		public float HyperthermiaThreshold = 313.15f;
	}
}
