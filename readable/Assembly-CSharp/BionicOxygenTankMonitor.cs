using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000A0C RID: 2572
public class BionicOxygenTankMonitor : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>
{
	// Token: 0x06004B69 RID: 19305 RVA: 0x001B63BC File Offset: 0x001B45BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.fistSpawn;
		this.fistSpawn.ParamTransition<bool>(this.HasSpawnedBefore, this.safe, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.IsTrue).Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.StartWithFullTank));
		this.safe.Transition(this.low, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)), UpdateRate.SIM_200ms);
		this.low.DefaultState(this.low.idle);
		this.low.idle.Transition(this.critical, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical), UpdateRate.SIM_200ms).Transition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe), UpdateRate.SIM_200ms).ScheduleChange(this.low.schedule, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule));
		this.low.schedule.ToggleUrge(Db.Get().Urges.FindOxygenRefill).DefaultState(this.low.schedule.enableSensors).Transition(this.critical, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCriticalAndNotConsumingOxygen), UpdateRate.SIM_200ms).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors));
		this.low.schedule.enableSensors.Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).GoTo(this.low.schedule.oxygenCanisterMode);
		this.low.schedule.oxygenCanisterMode.DefaultState(this.low.schedule.oxygenCanisterMode.running);
		this.low.schedule.oxygenCanisterMode.running.ScheduleChange(this.low.idle, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsNotAllowedToSeekOxygenSourceItemsByScheduleAndSeekChoreHasNotBegun)).OnSignal(this.OxygenSourceItemLostSignal, this.low.schedule.environmentAbsorbMode, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback(BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable)).OnSignal(this.AbsorbCellChangedSignal, this.low.schedule.environmentAbsorbMode, (BionicOxygenTankMonitor.Instance smi, StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter param) => !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi) && BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi)).Update(new Action<BionicOxygenTankMonitor.Instance, float>(BionicOxygenTankMonitor.UpdateAbsorbCellIfNoOxygenSourceAvailable), UpdateRate.SIM_200ms, false).ToggleChore((BionicOxygenTankMonitor.Instance smi) => new FindAndConsumeOxygenSourceChore(smi.master, false), this.low.schedule.oxygenCanisterMode.ends, this.low.schedule.oxygenCanisterMode.ends);
		this.low.schedule.oxygenCanisterMode.ends.EnterTransition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)).GoTo(this.low.idle);
		this.low.schedule.environmentAbsorbMode.DefaultState(this.low.schedule.environmentAbsorbMode.running);
		this.low.schedule.environmentAbsorbMode.running.ScheduleChange(this.low.idle, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsNotAllowedToSeekOxygenSourceItemsByScheduleAndAbsorbChoreHasNotBegun)).OnSignal(this.ClosestOxygenSourceChanged, this.low.schedule.oxygenCanisterMode, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback(BionicOxygenTankMonitor.OxygenSourceItemAvailableAndAbsorbChoreNotStarted)).ToggleChore((BionicOxygenTankMonitor.Instance smi) => new BionicMassOxygenAbsorbChore(smi.master, false), this.low.schedule.environmentAbsorbMode.ends, this.low.schedule.environmentAbsorbMode.ends);
		this.low.schedule.environmentAbsorbMode.ends.EnterTransition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)).GoTo(this.low.idle);
		this.critical.ToggleUrge(Db.Get().Urges.FindOxygenRefill).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors)).DefaultState(this.critical.enableSensors).ToggleExpression(Db.Get().Expressions.RecoverBreath, null).Update(delegate(BionicOxygenTankMonitor.Instance smi, float dt)
		{
			if (smi.master.gameObject.GetAmounts().Get("Breath").value <= DUPLICANTSTATS.BIONICS.Breath.SUFFOCATE_AMOUNT)
			{
				smi.isRecoveringFromSuffocation = true;
			}
		}, UpdateRate.SIM_200ms, false).Exit(delegate(BionicOxygenTankMonitor.Instance smi)
		{
			smi.isRecoveringFromSuffocation = false;
		});
		this.critical.enableSensors.Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).GoTo(this.critical.oxygenCanisterMode);
		this.critical.oxygenCanisterMode.DefaultState(this.critical.oxygenCanisterMode.running);
		this.critical.oxygenCanisterMode.running.OnSignal(this.ClosestOxygenSourceChanged, this.critical.environmentAbsorbMode, (BionicOxygenTankMonitor.Instance smi, StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter param) => !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi) && BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi)).OnSignal(this.OxygenSourceItemLostSignal, this.critical.environmentAbsorbMode, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback(BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable)).OnSignal(this.AbsorbCellChangedSignal, this.critical.environmentAbsorbMode, (BionicOxygenTankMonitor.Instance smi, StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter param) => !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi) && BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi)).Update(new Action<BionicOxygenTankMonitor.Instance, float>(BionicOxygenTankMonitor.UpdateAbsorbCellIfNoOxygenSourceAvailable), UpdateRate.SIM_200ms, false).ToggleChore((BionicOxygenTankMonitor.Instance smi) => new FindAndConsumeOxygenSourceChore(smi.master, true), this.critical.oxygenCanisterMode.ends, this.critical.oxygenCanisterMode.ends);
		this.critical.oxygenCanisterMode.ends.EnterTransition(this.low, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo(this.critical.oxygenCanisterMode.running);
		this.critical.environmentAbsorbMode.DefaultState(this.critical.environmentAbsorbMode.running);
		this.critical.environmentAbsorbMode.running.OnSignal(this.ClosestOxygenSourceChanged, this.critical.oxygenCanisterMode, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback(BionicOxygenTankMonitor.OxygenSourceItemAvailableAndAbsorbChoreNotStarted)).ToggleChore((BionicOxygenTankMonitor.Instance smi) => new BionicMassOxygenAbsorbChore(smi.master, true), this.critical.environmentAbsorbMode.ends, this.critical.environmentAbsorbMode.ends);
		this.critical.environmentAbsorbMode.ends.EnterTransition(this.low, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo(this.critical.oxygenCanisterMode);
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x001B6A8D File Offset: 0x001B4C8D
	public static bool IsAllowedToSeekOxygenBySchedule(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.IsAllowedToSeekOxygenBySchedule;
	}

	// Token: 0x06004B6B RID: 19307 RVA: 0x001B6A95 File Offset: 0x001B4C95
	public static bool IsNotAllowedToSeekOxygenSourceItemsByScheduleAndSeekChoreHasNotBegun(BionicOxygenTankMonitor.Instance smi)
	{
		return !BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi) && !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi);
	}

	// Token: 0x06004B6C RID: 19308 RVA: 0x001B6AAA File Offset: 0x001B4CAA
	public static bool IsNotAllowedToSeekOxygenSourceItemsByScheduleAndAbsorbChoreHasNotBegun(BionicOxygenTankMonitor.Instance smi)
	{
		return !BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi) && !BionicOxygenTankMonitor.AbsorbChoreIsRunning(smi);
	}

	// Token: 0x06004B6D RID: 19309 RVA: 0x001B6ABF File Offset: 0x001B4CBF
	public static bool AreOxygenLevelsSafe(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.OxygenPercentage >= 0.85f;
	}

	// Token: 0x06004B6E RID: 19310 RVA: 0x001B6AD1 File Offset: 0x001B4CD1
	public static bool AreOxygenLevelsCritical(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.OxygenPercentage <= 0f;
	}

	// Token: 0x06004B6F RID: 19311 RVA: 0x001B6AE3 File Offset: 0x001B4CE3
	public static bool AreOxygenLevelsCriticalAndNotConsumingOxygen(BionicOxygenTankMonitor.Instance smi)
	{
		return BionicOxygenTankMonitor.AreOxygenLevelsCritical(smi) && !BionicOxygenTankMonitor.IsConsumingOxygen(smi);
	}

	// Token: 0x06004B70 RID: 19312 RVA: 0x001B6AF8 File Offset: 0x001B4CF8
	public static bool IsThereAnOxygenSourceItemAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.GetClosestOxygenSource() != null;
	}

	// Token: 0x06004B71 RID: 19313 RVA: 0x001B6B06 File Offset: 0x001B4D06
	public static bool AbsorbCellUnavailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.AbsorbOxygenCell == Grid.InvalidCell;
	}

	// Token: 0x06004B72 RID: 19314 RVA: 0x001B6B15 File Offset: 0x001B4D15
	public static bool AbsorbCellAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.AbsorbOxygenCell != Grid.InvalidCell;
	}

	// Token: 0x06004B73 RID: 19315 RVA: 0x001B6B27 File Offset: 0x001B4D27
	public static bool NoOxygenSourceAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.GetClosestOxygenSource() == null;
	}

	// Token: 0x06004B74 RID: 19316 RVA: 0x001B6B35 File Offset: 0x001B4D35
	public static bool NoOxygenSourceAvailableButAbsorbCellAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi, null);
	}

	// Token: 0x06004B75 RID: 19317 RVA: 0x001B6B3E File Offset: 0x001B4D3E
	public static bool NoOxygenSourceAvailableButAbsorbCellAvailable(BionicOxygenTankMonitor.Instance smi, StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter param)
	{
		return BionicOxygenTankMonitor.NoOxygenSourceAvailable(smi) && BionicOxygenTankMonitor.AbsorbCellAvailable(smi);
	}

	// Token: 0x06004B76 RID: 19318 RVA: 0x001B6B50 File Offset: 0x001B4D50
	public static bool OxygenSourceItemAvailableAndAbsorbChoreNotStarted(BionicOxygenTankMonitor.Instance smi, StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter param)
	{
		return BionicOxygenTankMonitor.IsThereAnOxygenSourceItemAvailable(smi) && !BionicOxygenTankMonitor.AbsorbChoreIsRunning(smi);
	}

	// Token: 0x06004B77 RID: 19319 RVA: 0x001B6B65 File Offset: 0x001B4D65
	public static bool AbsorbChoreIsRunning(BionicOxygenTankMonitor.Instance smi)
	{
		return BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.BionicAbsorbOxygen) || BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.BionicAbsorbOxygen_Critical);
	}

	// Token: 0x06004B78 RID: 19320 RVA: 0x001B6B95 File Offset: 0x001B4D95
	public static bool FindOxygenSourceChoreIsRunning(BionicOxygenTankMonitor.Instance smi)
	{
		return BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.FindOxygenSourceItem) || BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.FindOxygenSourceItem_Critical);
	}

	// Token: 0x06004B79 RID: 19321 RVA: 0x001B6BC5 File Offset: 0x001B4DC5
	public static bool ChoreIsRunning(BionicOxygenTankMonitor.Instance smi, ChoreType type)
	{
		return smi.ChoreIsRunning(type);
	}

	// Token: 0x06004B7A RID: 19322 RVA: 0x001B6BCE File Offset: 0x001B4DCE
	public static bool IsConsumingOxygen(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.IsConsumingOxygen();
	}

	// Token: 0x06004B7B RID: 19323 RVA: 0x001B6BD6 File Offset: 0x001B4DD6
	public static void StartWithFullTank(BionicOxygenTankMonitor.Instance smi)
	{
		smi.AddFirstTimeSpawnedOxygen();
	}

	// Token: 0x06004B7C RID: 19324 RVA: 0x001B6BDE File Offset: 0x001B4DDE
	public static void EnableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
	{
		smi.SetOxygenSourceSensorsActiveState(true);
	}

	// Token: 0x06004B7D RID: 19325 RVA: 0x001B6BE7 File Offset: 0x001B4DE7
	public static void DisableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
	{
		smi.SetOxygenSourceSensorsActiveState(false);
	}

	// Token: 0x06004B7E RID: 19326 RVA: 0x001B6BF0 File Offset: 0x001B4DF0
	public static void UpdateAbsorbCellIfNoOxygenSourceAvailable(BionicOxygenTankMonitor.Instance smi, float dt)
	{
		if (BionicOxygenTankMonitor.NoOxygenSourceAvailable(smi))
		{
			smi.UpdatePotentialCellToAbsorbOxygen(Grid.InvalidCell);
		}
	}

	// Token: 0x040031F9 RID: 12793
	public const SimHashes INITIAL_TANK_ELEMENT = SimHashes.Oxygen;

	// Token: 0x040031FA RID: 12794
	public static readonly Tag INITIAL_TANK_ELEMENT_TAG = SimHashes.Oxygen.CreateTag();

	// Token: 0x040031FB RID: 12795
	public const float SAFE_TRESHOLD = 0.85f;

	// Token: 0x040031FC RID: 12796
	public const float CRITICAL_TRESHOLD = 0f;

	// Token: 0x040031FD RID: 12797
	public const float OXYGEN_TANK_CAPACITY_IN_SECONDS = 2400f;

	// Token: 0x040031FE RID: 12798
	public static readonly float OXYGEN_TANK_CAPACITY_KG = 2400f * DUPLICANTSTATS.BIONICS.BaseStats.OXYGEN_USED_PER_SECOND;

	// Token: 0x040031FF RID: 12799
	public static float INITIAL_OXYGEN_TEMP = DUPLICANTSTATS.BIONICS.Temperature.Internal.IDEAL;

	// Token: 0x04003200 RID: 12800
	public static float SECONDS_PER_PATH_COST_UNIT = 0.3f;

	// Token: 0x04003201 RID: 12801
	public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State fistSpawn;

	// Token: 0x04003202 RID: 12802
	public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State safe;

	// Token: 0x04003203 RID: 12803
	public BionicOxygenTankMonitor.LowOxygenStates low;

	// Token: 0x04003204 RID: 12804
	public BionicOxygenTankMonitor.SeekOxygenStates critical;

	// Token: 0x04003205 RID: 12805
	private StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.BoolParameter HasSpawnedBefore;

	// Token: 0x04003206 RID: 12806
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal AbsorbCellChangedSignal;

	// Token: 0x04003207 RID: 12807
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal OxygenSourceItemLostSignal;

	// Token: 0x04003208 RID: 12808
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal ClosestOxygenSourceChanged;

	// Token: 0x02001A8E RID: 6798
	public interface IChore
	{
		// Token: 0x0600A601 RID: 42497
		bool IsConsumingOxygen();
	}

	// Token: 0x02001A8F RID: 6799
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A90 RID: 6800
	public class ChoreState : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x04008203 RID: 33283
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State running;

		// Token: 0x04008204 RID: 33284
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State ends;
	}

	// Token: 0x02001A91 RID: 6801
	public class SeekOxygenStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x04008205 RID: 33285
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State enableSensors;

		// Token: 0x04008206 RID: 33286
		public BionicOxygenTankMonitor.ChoreState oxygenCanisterMode;

		// Token: 0x04008207 RID: 33287
		public BionicOxygenTankMonitor.ChoreState environmentAbsorbMode;
	}

	// Token: 0x02001A92 RID: 6802
	public class LowOxygenStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x04008208 RID: 33288
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State idle;

		// Token: 0x04008209 RID: 33289
		public BionicOxygenTankMonitor.SeekOxygenStates schedule;
	}

	// Token: 0x02001A93 RID: 6803
	public new class Instance : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.GameInstance, OxygenBreather.IGasProvider
	{
		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x0600A606 RID: 42502 RVA: 0x003B8C18 File Offset: 0x003B6E18
		public bool IsAllowedToSeekOxygenBySchedule
		{
			get
			{
				return ScheduleManager.Instance.IsAllowed(this.schedulable, Db.Get().ScheduleBlockTypes.Eat);
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600A607 RID: 42503 RVA: 0x003B8C39 File Offset: 0x003B6E39
		public bool IsEmpty
		{
			get
			{
				return this.AvailableOxygen == 0f;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600A608 RID: 42504 RVA: 0x003B8C48 File Offset: 0x003B6E48
		public float OxygenPercentage
		{
			get
			{
				return this.AvailableOxygen / this.storage.capacityKg;
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x0600A609 RID: 42505 RVA: 0x003B8C5C File Offset: 0x003B6E5C
		public float AvailableOxygen
		{
			get
			{
				return this.storage.GetMassAvailable(GameTags.Breathable);
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x0600A60A RID: 42506 RVA: 0x003B8C6E File Offset: 0x003B6E6E
		public float SpaceAvailableInTank
		{
			get
			{
				return this.storage.capacityKg - this.AvailableOxygen;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x0600A60C RID: 42508 RVA: 0x003B8C8B File Offset: 0x003B6E8B
		// (set) Token: 0x0600A60B RID: 42507 RVA: 0x003B8C82 File Offset: 0x003B6E82
		public int AbsorbOxygenCell { get; private set; } = Grid.InvalidCell;

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x0600A60E RID: 42510 RVA: 0x003B8C9C File Offset: 0x003B6E9C
		// (set) Token: 0x0600A60D RID: 42509 RVA: 0x003B8C93 File Offset: 0x003B6E93
		public Storage storage { get; private set; }

		// Token: 0x0600A60F RID: 42511 RVA: 0x003B8CA4 File Offset: 0x003B6EA4
		public Instance(IStateMachineTarget master, BionicOxygenTankMonitor.Def def) : base(master, def)
		{
			this.query = new AbsorbCellQuery();
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
			Sensors component = base.GetComponent<Sensors>();
			this.schedulable = base.GetComponent<Schedulable>();
			this.navigator = base.GetComponent<Navigator>();
			float movementSpeedMultiplier = BipedTransitionLayer.GetMovementSpeedMultiplier(Db.Get().AttributeConverters.MovementSpeed.Lookup(this.navigator.gameObject));
			this.movementRate = movementSpeedMultiplier / BionicOxygenTankMonitor.SECONDS_PER_PATH_COST_UNIT;
			this.oxygenBreather = base.GetComponent<OxygenBreather>();
			this.brain = base.GetComponent<MinionBrain>();
			this.dataHolder = base.GetComponent<MinionStorageDataHolder>();
			MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
			minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Combine(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			this.oxygenSourceSensors = new ClosestPickupableSensor<Pickupable>[]
			{
				component.GetSensor<ClosestOxygenCanisterSensor>()
			};
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				closestPickupableSensor.OnItemChanged = (Action<Pickupable>)Delegate.Combine(closestPickupableSensor.OnItemChanged, new Action<Pickupable>(this.OnOxygenSourceSensorItemChanged));
			}
			this.storage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicOxygenTankStorage);
			this.oxygenTankAmountInstance = Db.Get().Amounts.BionicOxygenTank.Lookup(base.gameObject);
			this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(base.gameObject);
			Storage storage = this.storage;
			storage.OnStorageChange = (Action<GameObject>)Delegate.Combine(storage.OnStorageChange, new Action<GameObject>(this.OnOxygenTankStorageChanged));
			this.choreDriver = base.gameObject.GetComponent<ChoreDriver>();
		}

		// Token: 0x0600A610 RID: 42512 RVA: 0x003B8E7C File Offset: 0x003B707C
		public bool ChoreIsRunning(ChoreType type)
		{
			if (this.choreDriver == null)
			{
				return false;
			}
			Chore currentChore = this.choreDriver.GetCurrentChore();
			return currentChore != null && currentChore.choreType == type;
		}

		// Token: 0x0600A611 RID: 42513 RVA: 0x003B8EB4 File Offset: 0x003B70B4
		public bool IsConsumingOxygen()
		{
			this.choreDriver = base.smi.GetComponent<ChoreDriver>();
			if (this.choreDriver == null)
			{
				return false;
			}
			BionicOxygenTankMonitor.IChore chore = this.choreDriver.GetCurrentChore() as BionicOxygenTankMonitor.IChore;
			return chore != null && chore.IsConsumingOxygen();
		}

		// Token: 0x0600A612 RID: 42514 RVA: 0x003B8EFE File Offset: 0x003B70FE
		public Pickupable GetClosestOxygenSource()
		{
			return this.closestOxygenSource;
		}

		// Token: 0x0600A613 RID: 42515 RVA: 0x003B8F06 File Offset: 0x003B7106
		private void OnOxygenSourceSensorItemChanged(object o)
		{
			this.CompareOxygenSources();
		}

		// Token: 0x0600A614 RID: 42516 RVA: 0x003B8F0E File Offset: 0x003B710E
		private void OnOxygenTankStorageChanged(object o)
		{
			this.RefreshAmountInstance();
		}

		// Token: 0x0600A615 RID: 42517 RVA: 0x003B8F16 File Offset: 0x003B7116
		public void RefreshAmountInstance()
		{
			this.oxygenTankAmountInstance.SetValue(this.AvailableOxygen);
		}

		// Token: 0x0600A616 RID: 42518 RVA: 0x003B8F2C File Offset: 0x003B712C
		public void AddFirstTimeSpawnedOxygen()
		{
			this.storage.AddElement(SimHashes.Oxygen, this.storage.capacityKg - this.AvailableOxygen, BionicOxygenTankMonitor.INITIAL_OXYGEN_TEMP, byte.MaxValue, 0, false, true);
			base.sm.HasSpawnedBefore.Set(true, this, false);
		}

		// Token: 0x0600A617 RID: 42519 RVA: 0x003B8F80 File Offset: 0x003B7180
		private void OnCopyMinionBegins(StoredMinionIdentity destination)
		{
			MinionStorageDataHolder.DataPackData data = new MinionStorageDataHolder.DataPackData
			{
				Bools = new bool[]
				{
					base.sm.HasSpawnedBefore.Get(this)
				}
			};
			this.dataHolder.UpdateData(data);
		}

		// Token: 0x0600A618 RID: 42520 RVA: 0x003B8FC1 File Offset: 0x003B71C1
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshAmountInstance();
		}

		// Token: 0x0600A619 RID: 42521 RVA: 0x003B8FD0 File Offset: 0x003B71D0
		public override void PostParamsInitialized()
		{
			MinionStorageDataHolder.DataPack dataPack = this.dataHolder.GetDataPack<BionicOxygenTankMonitor.Instance>();
			if (dataPack != null && dataPack.IsStoringNewData)
			{
				MinionStorageDataHolder.DataPackData dataPackData = dataPack.ReadData();
				if (dataPackData != null)
				{
					bool value = dataPackData.Bools[0];
					base.sm.HasSpawnedBefore.Set(value, this, false);
				}
			}
			base.PostParamsInitialized();
		}

		// Token: 0x0600A61A RID: 42522 RVA: 0x003B9024 File Offset: 0x003B7224
		private void CompareOxygenSources()
		{
			Pickupable pickupable = null;
			float num = 2.1474836E+09f;
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				int itemNavCost = closestPickupableSensor.GetItemNavCost();
				if ((float)itemNavCost < num)
				{
					num = (float)itemNavCost;
					pickupable = closestPickupableSensor.GetItem();
				}
			}
			if (pickupable != null && base.IsInsideState(base.sm.critical))
			{
				float num2 = num / this.movementRate * this.oxygenBreather.ConsumptionRate;
				if (this.oxygenBreather.GetAmounts().Get(Db.Get().Amounts.Breath).value < num2)
				{
					pickupable = null;
				}
			}
			if (this.closestOxygenSource != pickupable)
			{
				this.closestOxygenSource = pickupable;
				base.sm.ClosestOxygenSourceChanged.Trigger(this);
			}
		}

		// Token: 0x0600A61B RID: 42523 RVA: 0x003B90F4 File Offset: 0x003B72F4
		public void UpdatePotentialCellToAbsorbOxygen(int previouslyReservedCell)
		{
			float breathPercentage = this.brain.GetAmounts().Get(Db.Get().Amounts.Breath).value / this.brain.GetAmounts().Get(Db.Get().Amounts.Breath).GetMax();
			this.query.Reset(this.brain, BionicOxygenTankMonitor.AreOxygenLevelsCritical(this), this.AvailableOxygen, breathPercentage, previouslyReservedCell, this.isRecoveringFromSuffocation);
			this.navigator.RunQuery(base.smi.query);
			int num = base.smi.query.GetResultCell();
			if (num == Grid.PosToCell(base.gameObject) && !GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(num, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, this.oxygenBreather).IsBreathable)
			{
				num = PathFinder.InvalidCell;
			}
			bool flag = this.AbsorbOxygenCell != num;
			this.AbsorbOxygenCell = num;
			if (flag)
			{
				base.sm.AbsorbCellChangedSignal.Trigger(this);
			}
		}

		// Token: 0x0600A61C RID: 42524 RVA: 0x003B91EA File Offset: 0x003B73EA
		public float AddGas(Sim.MassConsumedCallback mass_cb_info)
		{
			return this.AddGas(ElementLoader.elements[(int)mass_cb_info.elemIdx].id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
		}

		// Token: 0x0600A61D RID: 42525 RVA: 0x003B9220 File Offset: 0x003B7420
		public float AddGas(SimHashes element, float mass, float temperature, byte disseaseIDX = 255, int _disseaseCount = 0)
		{
			float num = Mathf.Min(mass, this.SpaceAvailableInTank);
			float result = mass - num;
			float num2 = num / mass;
			int disease_count = Mathf.CeilToInt((float)_disseaseCount * num2);
			this.storage.AddElement(element, num, temperature, disseaseIDX, disease_count, false, true);
			return result;
		}

		// Token: 0x0600A61E RID: 42526 RVA: 0x003B9260 File Offset: 0x003B7460
		public void SetOxygenSourceSensorsActiveState(bool shouldItBeActive)
		{
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				closestPickupableSensor.SetActive(shouldItBeActive);
				if (shouldItBeActive)
				{
					closestPickupableSensor.Update();
				}
			}
		}

		// Token: 0x0600A61F RID: 42527 RVA: 0x003B929C File Offset: 0x003B749C
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.IsEmpty)
			{
				return false;
			}
			SimHashes elementConsumed = SimHashes.Vacuum;
			float temperature = 0f;
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out temperature, out elementConsumed);
			OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, amount, temperature, diseaseInfo.idx, diseaseInfo.count);
			return true;
		}

		// Token: 0x0600A620 RID: 42528 RVA: 0x003B92EE File Offset: 0x003B74EE
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x0600A621 RID: 42529 RVA: 0x003B92F0 File Offset: 0x003B74F0
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x0600A622 RID: 42530 RVA: 0x003B92F2 File Offset: 0x003B74F2
		public bool IsLowOxygen()
		{
			return this.OxygenPercentage <= 0f;
		}

		// Token: 0x0600A623 RID: 42531 RVA: 0x003B9304 File Offset: 0x003B7504
		public bool HasOxygen()
		{
			return !this.IsEmpty;
		}

		// Token: 0x0600A624 RID: 42532 RVA: 0x003B930F File Offset: 0x003B750F
		public bool IsBlocked()
		{
			return false;
		}

		// Token: 0x0600A625 RID: 42533 RVA: 0x003B9312 File Offset: 0x003B7512
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x0600A626 RID: 42534 RVA: 0x003B9315 File Offset: 0x003B7515
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x0600A627 RID: 42535 RVA: 0x003B9318 File Offset: 0x003B7518
		protected override void OnCleanUp()
		{
			if (this.dataHolder != null)
			{
				MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
				minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Remove(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			}
			if (this.storage != null)
			{
				Storage storage = this.storage;
				storage.OnStorageChange = (Action<GameObject>)Delegate.Remove(storage.OnStorageChange, new Action<GameObject>(this.OnOxygenTankStorageChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x0400820A RID: 33290
		public AttributeInstance airConsumptionRate;

		// Token: 0x0400820B RID: 33291
		private Schedulable schedulable;

		// Token: 0x0400820C RID: 33292
		private AmountInstance oxygenTankAmountInstance;

		// Token: 0x0400820D RID: 33293
		private ClosestPickupableSensor<Pickupable>[] oxygenSourceSensors;

		// Token: 0x0400820E RID: 33294
		private Pickupable closestOxygenSource;

		// Token: 0x0400820F RID: 33295
		private Navigator navigator;

		// Token: 0x04008210 RID: 33296
		private float movementRate;

		// Token: 0x04008211 RID: 33297
		private AbsorbCellQuery query;

		// Token: 0x04008212 RID: 33298
		private OxygenBreather oxygenBreather;

		// Token: 0x04008213 RID: 33299
		private MinionBrain brain;

		// Token: 0x04008214 RID: 33300
		private MinionStorageDataHolder dataHolder;

		// Token: 0x04008215 RID: 33301
		private ChoreDriver choreDriver;

		// Token: 0x04008216 RID: 33302
		public bool isRecoveringFromSuffocation;
	}
}
