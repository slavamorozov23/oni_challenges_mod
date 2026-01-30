using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000888 RID: 2184
public class BeeHappinessMonitor : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>
{
	// Token: 0x06003C1D RID: 15389 RVA: 0x00150A70 File Offset: 0x0014EC70
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.TriggerOnEnter(GameHashes.Satisfied, null).Transition(this.happy, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition(this.unhappy, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsUnhappy), UpdateRate.SIM_1000ms).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.neutralEffect);
		this.happy.TriggerOnEnter(GameHashes.Happy, null).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.happyEffect).Transition(this.satisfied, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms);
		this.unhappy.TriggerOnEnter(GameHashes.Unhappy, null).Transition(this.satisfied, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsUnhappy)), UpdateRate.SIM_1000ms).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.unhappyEffect);
		this.happyEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_WILD.NAME, CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.neutralEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.unhappyEffect = new Effect("Unhappy", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
	}

	// Token: 0x06003C1E RID: 15390 RVA: 0x00150C0F File Offset: 0x0014EE0F
	private static bool IsHappy(BeeHappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.happyThreshold;
	}

	// Token: 0x06003C1F RID: 15391 RVA: 0x00150C2C File Offset: 0x0014EE2C
	private static bool IsUnhappy(BeeHappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() <= smi.def.unhappyThreshold;
	}

	// Token: 0x04002514 RID: 9492
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State satisfied;

	// Token: 0x04002515 RID: 9493
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State happy;

	// Token: 0x04002516 RID: 9494
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State unhappy;

	// Token: 0x04002517 RID: 9495
	private Effect happyEffect;

	// Token: 0x04002518 RID: 9496
	private Effect neutralEffect;

	// Token: 0x04002519 RID: 9497
	private Effect unhappyEffect;

	// Token: 0x02001855 RID: 6229
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007AA6 RID: 31398
		public float happyThreshold = 4f;

		// Token: 0x04007AA7 RID: 31399
		public float unhappyThreshold = -1f;
	}

	// Token: 0x02001856 RID: 6230
	public new class Instance : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.GameInstance
	{
		// Token: 0x06009E9A RID: 40602 RVA: 0x003A3C6A File Offset: 0x003A1E6A
		public Instance(IStateMachineTarget master, BeeHappinessMonitor.Def def) : base(master, def)
		{
			this.happiness = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
		}

		// Token: 0x04007AA8 RID: 31400
		public AttributeInstance happiness;
	}
}
