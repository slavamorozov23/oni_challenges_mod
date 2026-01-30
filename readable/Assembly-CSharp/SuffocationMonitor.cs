using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000A4E RID: 2638
public class SuffocationMonitor : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>
{
	// Token: 0x06004CD9 RID: 19673 RVA: 0x001BF104 File Offset: 0x001BD304
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.satisfied;
		this.root.TagTransition(GameTags.Dead, this.dead, false);
		this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (SuffocationMonitor.Instance smi) => smi.increaseBreathModifier, null).EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.noOxygen, (SuffocationMonitor.Instance smi) => !smi.CanBreath()).Transition(this.noOxygen, (SuffocationMonitor.Instance smi) => !smi.CanBreath(), UpdateRate.SIM_200ms);
		this.satisfied.normal.Transition(this.satisfied.low, (SuffocationMonitor.Instance smi) => smi.oxygenBreather.IsLowOxygen(), UpdateRate.SIM_200ms);
		this.satisfied.low.Transition(this.satisfied.normal, (SuffocationMonitor.Instance smi) => !smi.oxygenBreather.IsLowOxygen(), UpdateRate.SIM_200ms).ToggleEffect("LowOxygen");
		this.noOxygen.EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.satisfied, (SuffocationMonitor.Instance smi) => smi.CanBreath()).TagTransition(GameTags.RecoveringBreath, this.satisfied, false).ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (SuffocationMonitor.Instance smi) => smi.decreaseBreathModifier, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
		this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.noOxygen.suffocating, (SuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (SuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(SuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
		this.dead.DoNothing();
	}

	// Token: 0x04003331 RID: 13105
	public SuffocationMonitor.SatisfiedState satisfied;

	// Token: 0x04003332 RID: 13106
	public SuffocationMonitor.NoOxygenState noOxygen;

	// Token: 0x04003333 RID: 13107
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State death;

	// Token: 0x04003334 RID: 13108
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State dead;

	// Token: 0x02001B4C RID: 6988
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B4D RID: 6989
	public class NoOxygenState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State
	{
		// Token: 0x04008467 RID: 33895
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State holdingbreath;

		// Token: 0x04008468 RID: 33896
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State suffocating;
	}

	// Token: 0x02001B4E RID: 6990
	public class SatisfiedState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State
	{
		// Token: 0x04008469 RID: 33897
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State normal;

		// Token: 0x0400846A RID: 33898
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State low;
	}

	// Token: 0x02001B4F RID: 6991
	public new class Instance : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.GameInstance
	{
		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x0600A948 RID: 43336 RVA: 0x003C0F15 File Offset: 0x003BF115
		// (set) Token: 0x0600A949 RID: 43337 RVA: 0x003C0F1D File Offset: 0x003BF11D
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x0600A94A RID: 43338 RVA: 0x003C0F28 File Offset: 0x003BF128
		public Instance(IStateMachineTarget master, SuffocationMonitor.Def def) : base(master, def)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.increaseBreathModifier = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.decreaseBreathModifier = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.oxygenBreather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x0600A94B RID: 43339 RVA: 0x003C0FCD File Offset: 0x003BF1CD
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x0600A94C RID: 43340 RVA: 0x003C0FD5 File Offset: 0x003BF1D5
		public bool CanBreath()
		{
			return this.oxygenBreather.prefabID.HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.prefabID.HasTag(GameTags.InTransitTube) || this.oxygenBreather.HasOxygen;
		}

		// Token: 0x0600A94D RID: 43341 RVA: 0x003C1012 File Offset: 0x003BF212
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x0600A94E RID: 43342 RVA: 0x003C1029 File Offset: 0x003BF229
		public bool IsSuffocating()
		{
			return this.breath.deltaAttribute.GetTotalValue() <= 0f && this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x0600A94F RID: 43343 RVA: 0x003C1063 File Offset: 0x003BF263
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x0400846B RID: 33899
		private AmountInstance breath;

		// Token: 0x0400846C RID: 33900
		public AttributeModifier increaseBreathModifier;

		// Token: 0x0400846D RID: 33901
		public AttributeModifier decreaseBreathModifier;
	}
}
