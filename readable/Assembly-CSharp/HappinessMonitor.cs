using System;
using Klei.AI;
using STRINGS;

// Token: 0x020008A8 RID: 2216
public class HappinessMonitor : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>
{
	// Token: 0x06003D11 RID: 15633 RVA: 0x00154BE4 File Offset: 0x00152DE4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.happy, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition(this.neutral, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsNeutral), UpdateRate.SIM_1000ms).Transition(this.glum, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsGlum), UpdateRate.SIM_1000ms).Transition(this.miserable, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsMisirable), UpdateRate.SIM_1000ms);
		this.happy.DefaultState(this.happy.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Happy).ToggleCritterEmotion(Db.Get().CritterEmotions.Happy, null);
		this.happy.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.happyWildEffect).TagTransition(GameTags.Creatures.Wild, this.happy.tame, true);
		this.happy.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.happyTameEffect).TagTransition(GameTags.Creatures.Wild, this.happy.wild, false);
		this.neutral.DefaultState(this.neutral.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsNeutral)), UpdateRate.SIM_1000ms);
		this.neutral.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.neutralWildEffect).TagTransition(GameTags.Creatures.Wild, this.neutral.tame, true);
		this.neutral.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.neutralTameEffect).TagTransition(GameTags.Creatures.Wild, this.neutral.wild, false);
		this.glum.DefaultState(this.glum.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsGlum)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
		this.glum.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.glumWildEffect).TagTransition(GameTags.Creatures.Wild, this.glum.tame, true);
		this.glum.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.glumTameEffect).TagTransition(GameTags.Creatures.Wild, this.glum.wild, false);
		this.miserable.DefaultState(this.miserable.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsMisirable)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
		this.miserable.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.miserableWildEffect).TagTransition(GameTags.Creatures.Wild, this.miserable.tame, true);
		this.miserable.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.miserableTameEffect).TagTransition(GameTags.Creatures.Wild, this.miserable.wild, false);
		this.happyWildEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_WILD.NAME, CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.happyTameEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_TAME.NAME, CREATURES.MODIFIERS.HAPPY_TAME.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.neutralWildEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.neutralTameEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.glumWildEffect = new Effect("Glum", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.glumTameEffect = new Effect("Glum", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.miserableWildEffect = new Effect("Miserable", CREATURES.MODIFIERS.MISERABLE.NAME, CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.miserableTameEffect = new Effect("Miserable", CREATURES.MODIFIERS.MISERABLE.NAME, CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.happyTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 9f, CREATURES.MODIFIERS.HAPPY_TAME.NAME, true, false, true));
		this.glumWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, CREATURES.MODIFIERS.GLUM.NAME, false, false, true));
		this.glumTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, CREATURES.MODIFIERS.GLUM.NAME, false, false, true));
		this.miserableTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, CREATURES.MODIFIERS.MISERABLE.NAME, false, false, true));
		this.miserableTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, true, false, true));
		this.miserableTameEffect.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, true, false, true));
		this.miserableWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, CREATURES.MODIFIERS.MISERABLE.NAME, false, false, true));
		this.miserableWildEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, true, false, true));
		this.miserableWildEffect.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, true, false, true));
	}

	// Token: 0x06003D12 RID: 15634 RVA: 0x001552E2 File Offset: 0x001534E2
	private static bool IsHappy(HappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.happyThreshold;
	}

	// Token: 0x06003D13 RID: 15635 RVA: 0x00155300 File Offset: 0x00153500
	private static bool IsNeutral(HappinessMonitor.Instance smi)
	{
		float totalValue = smi.happiness.GetTotalValue();
		return totalValue > smi.def.glumThreshold && totalValue < smi.def.happyThreshold;
	}

	// Token: 0x06003D14 RID: 15636 RVA: 0x00155338 File Offset: 0x00153538
	private static bool IsGlum(HappinessMonitor.Instance smi)
	{
		float totalValue = smi.happiness.GetTotalValue();
		return totalValue > smi.def.miserableThreshold && totalValue <= smi.def.glumThreshold;
	}

	// Token: 0x06003D15 RID: 15637 RVA: 0x00155372 File Offset: 0x00153572
	private static bool IsMisirable(HappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() <= smi.def.miserableThreshold;
	}

	// Token: 0x040025AD RID: 9645
	private GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State satisfied;

	// Token: 0x040025AE RID: 9646
	private HappinessMonitor.HappyState happy;

	// Token: 0x040025AF RID: 9647
	private HappinessMonitor.NeutralState neutral;

	// Token: 0x040025B0 RID: 9648
	private HappinessMonitor.UnhappyState glum;

	// Token: 0x040025B1 RID: 9649
	private HappinessMonitor.MiserableState miserable;

	// Token: 0x040025B2 RID: 9650
	private Effect happyWildEffect;

	// Token: 0x040025B3 RID: 9651
	private Effect happyTameEffect;

	// Token: 0x040025B4 RID: 9652
	private Effect neutralTameEffect;

	// Token: 0x040025B5 RID: 9653
	private Effect neutralWildEffect;

	// Token: 0x040025B6 RID: 9654
	private Effect glumWildEffect;

	// Token: 0x040025B7 RID: 9655
	private Effect glumTameEffect;

	// Token: 0x040025B8 RID: 9656
	private Effect miserableWildEffect;

	// Token: 0x040025B9 RID: 9657
	private Effect miserableTameEffect;

	// Token: 0x0200189A RID: 6298
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007B6E RID: 31598
		public float happyThreshold = 4f;

		// Token: 0x04007B6F RID: 31599
		public float glumThreshold = -1f;

		// Token: 0x04007B70 RID: 31600
		public float miserableThreshold = -10f;
	}

	// Token: 0x0200189B RID: 6299
	public class MiserableState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04007B71 RID: 31601
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04007B72 RID: 31602
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x0200189C RID: 6300
	public class NeutralState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04007B73 RID: 31603
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04007B74 RID: 31604
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x0200189D RID: 6301
	public class UnhappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04007B75 RID: 31605
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04007B76 RID: 31606
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x0200189E RID: 6302
	public class HappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04007B77 RID: 31607
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04007B78 RID: 31608
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x0200189F RID: 6303
	public new class Instance : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.GameInstance
	{
		// Token: 0x06009F98 RID: 40856 RVA: 0x003A6EDF File Offset: 0x003A50DF
		public Instance(IStateMachineTarget master, HappinessMonitor.Def def) : base(master, def)
		{
			this.happiness = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
		}

		// Token: 0x04007B79 RID: 31609
		public AttributeInstance happiness;
	}
}
