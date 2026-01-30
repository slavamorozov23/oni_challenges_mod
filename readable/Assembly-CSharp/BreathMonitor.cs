using System;
using Klei.AI;
using TUNING;

// Token: 0x02000A12 RID: 2578
public class BreathMonitor : GameStateMachine<BreathMonitor, BreathMonitor.Instance>
{
	// Token: 0x06004BA1 RID: 19361 RVA: 0x001B77E0 File Offset: 0x001B59E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DefaultState(this.satisfied.full).Transition(this.lowbreath, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsLowBreath), UpdateRate.SIM_200ms);
		this.satisfied.full.Transition(this.satisfied.notfull, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsNotFullBreath), UpdateRate.SIM_200ms).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.HideBreathBar));
		this.satisfied.notfull.Transition(this.satisfied.full, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath), UpdateRate.SIM_200ms).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar));
		this.lowbreath.DefaultState(this.lowbreath.nowheretorecover).Transition(this.satisfied, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.RecoverBreath, new Func<BreathMonitor.Instance, bool>(BreathMonitor.IsOutOfOxygen)).ToggleUrge(Db.Get().Urges.RecoverBreath).ToggleThought(Db.Get().Thoughts.Suffocating, null).ToggleTag(GameTags.HoldingBreath).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell)).Update(new Action<BreathMonitor.Instance, float>(BreathMonitor.UpdateRecoverBreathCell), UpdateRate.RENDER_1000ms, true);
		this.lowbreath.nowheretorecover.ParamTransition<int>(this.recoverBreathCell, this.lowbreath.recoveryavailable, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsValidRecoverCell));
		this.lowbreath.recoveryavailable.ParamTransition<int>(this.recoverBreathCell, this.lowbreath.nowheretorecover, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsNotValidRecoverCell)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell)).ToggleChore(new Func<BreathMonitor.Instance, Chore>(BreathMonitor.CreateRecoverBreathChore), this.lowbreath.nowheretorecover);
	}

	// Token: 0x06004BA2 RID: 19362 RVA: 0x001B79D8 File Offset: 0x001B5BD8
	private static bool IsLowBreath(BreathMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
		if (!(myWorld == null) && myWorld.AlertManager.IsRedAlert())
		{
			return smi.breath.value < DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}
		return smi.breath.value < DUPLICANTSTATS.STANDARD.Breath.RETREAT_AMOUNT;
	}

	// Token: 0x06004BA3 RID: 19363 RVA: 0x001B7A45 File Offset: 0x001B5C45
	private static Chore CreateRecoverBreathChore(BreathMonitor.Instance smi)
	{
		return new RecoverBreathChore(smi.master);
	}

	// Token: 0x06004BA4 RID: 19364 RVA: 0x001B7A52 File Offset: 0x001B5C52
	private static bool IsNotFullBreath(BreathMonitor.Instance smi)
	{
		return !BreathMonitor.IsFullBreath(smi);
	}

	// Token: 0x06004BA5 RID: 19365 RVA: 0x001B7A5D File Offset: 0x001B5C5D
	private static bool IsFullBreath(BreathMonitor.Instance smi)
	{
		return smi.breath.value >= smi.breath.GetMax();
	}

	// Token: 0x06004BA6 RID: 19366 RVA: 0x001B7A7A File Offset: 0x001B5C7A
	private static bool IsOutOfOxygen(BreathMonitor.Instance smi)
	{
		return smi.breather.IsOutOfOxygen;
	}

	// Token: 0x06004BA7 RID: 19367 RVA: 0x001B7A87 File Offset: 0x001B5C87
	private static void ShowBreathBar(BreathMonitor.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, new Func<float>(smi.GetBreath), true);
		}
	}

	// Token: 0x06004BA8 RID: 19368 RVA: 0x001B7AB3 File Offset: 0x001B5CB3
	private static void HideBreathBar(BreathMonitor.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, null, false);
		}
	}

	// Token: 0x06004BA9 RID: 19369 RVA: 0x001B7AD4 File Offset: 0x001B5CD4
	private static bool IsValidRecoverCell(BreathMonitor.Instance smi, int cell)
	{
		return cell != Grid.InvalidCell;
	}

	// Token: 0x06004BAA RID: 19370 RVA: 0x001B7AE1 File Offset: 0x001B5CE1
	private static bool IsNotValidRecoverCell(BreathMonitor.Instance smi, int cell)
	{
		return !BreathMonitor.IsValidRecoverCell(smi, cell);
	}

	// Token: 0x06004BAB RID: 19371 RVA: 0x001B7AED File Offset: 0x001B5CED
	private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi, float dt)
	{
		BreathMonitor.UpdateRecoverBreathCell(smi);
	}

	// Token: 0x06004BAC RID: 19372 RVA: 0x001B7AF8 File Offset: 0x001B5CF8
	private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi)
	{
		if (smi.canRecoverBreath)
		{
			smi.query.Reset();
			smi.navigator.RunQuery(smi.query);
			int num = smi.query.GetResultCell();
			if (!GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(num, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, smi.breather).IsBreathable)
			{
				num = PathFinder.InvalidCell;
			}
			smi.sm.recoverBreathCell.Set(num, smi, false);
		}
	}

	// Token: 0x0400321E RID: 12830
	public BreathMonitor.SatisfiedState satisfied;

	// Token: 0x0400321F RID: 12831
	public BreathMonitor.LowBreathState lowbreath;

	// Token: 0x04003220 RID: 12832
	public StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IntParameter recoverBreathCell;

	// Token: 0x02001AA8 RID: 6824
	public class LowBreathState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04008261 RID: 33377
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State nowheretorecover;

		// Token: 0x04008262 RID: 33378
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State recoveryavailable;
	}

	// Token: 0x02001AA9 RID: 6825
	public class SatisfiedState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04008263 RID: 33379
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State full;

		// Token: 0x04008264 RID: 33380
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State notfull;
	}

	// Token: 0x02001AAA RID: 6826
	public new class Instance : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A6A6 RID: 42662 RVA: 0x003BA7FC File Offset: 0x003B89FC
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			this.query = new SafetyQuery(Game.Instance.safetyConditions.RecoverBreathChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = base.GetComponent<Navigator>();
			this.breather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x0600A6A7 RID: 42663 RVA: 0x003BA874 File Offset: 0x003B8A74
		public int GetRecoverCell()
		{
			return base.sm.recoverBreathCell.Get(base.smi);
		}

		// Token: 0x0600A6A8 RID: 42664 RVA: 0x003BA88C File Offset: 0x003B8A8C
		public float GetBreath()
		{
			return this.breath.value / this.breath.GetMax();
		}

		// Token: 0x04008265 RID: 33381
		public AmountInstance breath;

		// Token: 0x04008266 RID: 33382
		public SafetyQuery query;

		// Token: 0x04008267 RID: 33383
		public Navigator navigator;

		// Token: 0x04008268 RID: 33384
		public OxygenBreather breather;

		// Token: 0x04008269 RID: 33385
		public bool canRecoverBreath = true;
	}
}
