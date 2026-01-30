using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public class BionicMassOxygenAbsorbChore : Chore<BionicMassOxygenAbsorbChore.Instance>
{
	// Token: 0x060018F5 RID: 6389 RVA: 0x0008A6E4 File Offset: 0x000888E4
	public BionicMassOxygenAbsorbChore(IStateMachineTarget target, bool critical) : base(critical ? Db.Get().ChoreTypes.BionicAbsorbOxygen_Critical : Db.Get().ChoreTypes.BionicAbsorbOxygen, target, target.GetComponent<ChoreProvider>(), false, null, null, null, critical ? PriorityScreen.PriorityClass.compulsory : PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicMassOxygenAbsorbChore.Instance(this, target.gameObject);
		Func<int> data = new Func<int>(base.smi.UpdateTargetCell);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCellUntilBegun, data);
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x0008A77C File Offset: 0x0008897C
	public override string ResolveString(string str)
	{
		float mass = (base.smi == null) ? 0f : base.smi.GetAverageMassConsumedPerSecond();
		return string.Format(base.ResolveString(str), GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x0008A7C0 File Offset: 0x000889C0
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("BionicMassAbsorbOxygenChore null context.consumer");
			return;
		}
		if (context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>() == null)
		{
			global::Debug.LogError("BionicMassAbsorbOxygenChore null BionicOxygenTankMonitor.Instance");
			return;
		}
		base.smi.ResetMassTrackHistory();
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x0008A840 File Offset: 0x00088A40
	public static bool IsNotAllowedByScheduleAndChoreIsNotCritical(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return !BionicMassOxygenAbsorbChore.IsCriticalChore(smi) && !BionicMassOxygenAbsorbChore.IsAllowedBySchedule(smi);
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x0008A855 File Offset: 0x00088A55
	public static bool IsAllowedBySchedule(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi.oxygenTankMonitor);
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x0008A862 File Offset: 0x00088A62
	public static bool IsCriticalChore(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return smi.master.choreType == Db.Get().ChoreTypes.BionicAbsorbOxygen_Critical;
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x0008A880 File Offset: 0x00088A80
	public static void ResetOxygenTimer(BionicMassOxygenAbsorbChore.Instance smi)
	{
		smi.sm.SecondsPassedWithoutOxygen.Set(0f, smi, false);
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x0008A89A File Offset: 0x00088A9A
	public static void RefreshTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi)
	{
		smi.UpdateTargetCell();
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x0008A8A3 File Offset: 0x00088AA3
	public static void UpdateTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		BionicMassOxygenAbsorbChore.RefreshTargetSafeCell(smi);
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x0008A8AB File Offset: 0x00088AAB
	public static bool HasSpaceInOxygenTank(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return smi.oxygenTankMonitor.SpaceAvailableInTank > 0f;
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x0008A8BF File Offset: 0x00088ABF
	public static bool ChoreIsCriticalModeAndGiveUpOxygenLevelReached(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return BionicMassOxygenAbsorbChore.IsCriticalChore(smi) && smi.oxygenTankMonitor.OxygenPercentage >= 0.25f;
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x0008A8E0 File Offset: 0x00088AE0
	public static bool BreathIsFull(BionicMassOxygenAbsorbChore.Instance smi)
	{
		AmountInstance amountInstance = smi.gameObject.GetAmounts().Get(Db.Get().Amounts.Breath);
		return amountInstance.value >= amountInstance.GetMax();
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x0008A91E File Offset: 0x00088B1E
	public static void UpdateTargetSafeCellOnlyInCriticalMode(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		if (BionicMassOxygenAbsorbChore.IsCriticalChore(smi))
		{
			BionicMassOxygenAbsorbChore.RefreshTargetSafeCell(smi);
		}
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x0008A930 File Offset: 0x00088B30
	public static void AbsorbUpdate(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		float mass = Mathf.Min(dt * BionicMassOxygenAbsorbChore.ABSORB_RATE, smi.oxygenTankMonitor.SpaceAvailableInTank);
		BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = new BionicMassOxygenAbsorbChore.AbsorbUpdateData(smi, dt);
		int gameCell;
		SimHashes nearBreathableElement = BionicMassOxygenAbsorbChore.GetNearBreathableElement(gameCell = Grid.PosToCell(smi.sm.dupe.Get(smi)), BionicMassOxygenAbsorbChore.ABSORB_RANGE, out gameCell);
		HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(BionicMassOxygenAbsorbChore.OnSimConsumeCallback), absorbUpdateData, "BionicMassOxygenAbsorbChore");
		SimMessages.ConsumeMass(gameCell, nearBreathableElement, mass, 6, handle.index);
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x0008A9BC File Offset: 0x00088BBC
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = (BionicMassOxygenAbsorbChore.AbsorbUpdateData)data;
		absorbUpdateData.smi.OnSimConsume(mass_cb_info, absorbUpdateData.dt);
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x0008A9E2 File Offset: 0x00088BE2
	private static void ShowOxygenBar(BionicMassOxygenAbsorbChore.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBionicOxygenTankDisplay(smi.gameObject, new Func<float>(smi.GetOxygen), true);
		}
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x0008AA0E File Offset: 0x00088C0E
	private static void HideOxygenBar(BionicMassOxygenAbsorbChore.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBionicOxygenTankDisplay(smi.gameObject, null, false);
		}
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x0008AA30 File Offset: 0x00088C30
	public static SimHashes GetNearBreathableElement(int centralCell, CellOffset[] range, out int elementCell)
	{
		float num = 0f;
		int num2 = centralCell;
		SimHashes simHashes = SimHashes.Vacuum;
		foreach (CellOffset offset in range)
		{
			int num3 = Grid.OffsetCell(centralCell, offset);
			SimHashes simHashes2 = SimHashes.Vacuum;
			float breathableMassInCell = BionicMassOxygenAbsorbChore.GetBreathableMassInCell(num3, out simHashes2);
			if (breathableMassInCell > Mathf.Epsilon && (simHashes == SimHashes.Vacuum || breathableMassInCell > num))
			{
				simHashes = simHashes2;
				num = breathableMassInCell;
				num2 = num3;
			}
		}
		elementCell = num2;
		return simHashes;
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x0008AAA8 File Offset: 0x00088CA8
	private static float GetBreathableMassInCell(int cell, out SimHashes elementID)
	{
		if (Grid.IsValidCell(cell))
		{
			Element element = Grid.Element[cell];
			if (element.HasTag(GameTags.Breathable))
			{
				elementID = element.id;
				return Grid.Mass[cell];
			}
		}
		elementID = SimHashes.Vacuum;
		return 0f;
	}

	// Token: 0x04000E6C RID: 3692
	public static CellOffset[] ABSORB_RANGE = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(-1, 1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x04000E6D RID: 3693
	public const float ABSORB_RATE_IDEAL_CHORE_DURATION = 30f;

	// Token: 0x04000E6E RID: 3694
	public static readonly float ABSORB_RATE = BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG / 30f;

	// Token: 0x04000E6F RID: 3695
	public const int HISTORY_ROW_COUNT = 15;

	// Token: 0x04000E70 RID: 3696
	public const float LOW_OXYGEN_TRESHOLD = 2f;

	// Token: 0x04000E71 RID: 3697
	public const float GIVE_UP_DURATION_CRTICIAL_MODE = 2f;

	// Token: 0x04000E72 RID: 3698
	public const float GIVE_UP_DURATION_LOW_OXYGEN_MODE = 4f;

	// Token: 0x04000E73 RID: 3699
	public const float CRITICAL_CHORE_GIVE_UP_OXYGEN_LEVEL_TRESHOLD = 0.25f;

	// Token: 0x04000E74 RID: 3700
	public const string ABSORB_ANIM_FILE = "anim_bionic_absorb_kanim";

	// Token: 0x04000E75 RID: 3701
	public const string ABSORB_PRE_ANIM_NAME = "absorb_pre";

	// Token: 0x04000E76 RID: 3702
	public const string ABSORB_LOOP_ANIM_NAME = "absorb_loop";

	// Token: 0x04000E77 RID: 3703
	public const string ABSORB_PST_ANIM_NAME = "absorb_pst";

	// Token: 0x04000E78 RID: 3704
	public static CellOffset MouthCellOffset = new CellOffset(0, 1);

	// Token: 0x020012C6 RID: 4806
	public class States : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore>
	{
		// Token: 0x06008961 RID: 35169 RVA: 0x00352270 File Offset: 0x00350470
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.move;
			base.Target(this.dupe);
			this.root.Exit(delegate(BionicMassOxygenAbsorbChore.Instance smi)
			{
				smi.ChangeCellReservation(Grid.InvalidCell);
			});
			this.move.DefaultState(this.move.onGoing).ScheduleChange(this.fail, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.IsNotAllowedByScheduleAndChoreIsNotCritical));
			this.move.onGoing.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.RefreshTargetSafeCell)).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.UpdateTargetSafeCellOnlyInCriticalMode), UpdateRate.RENDER_1000ms, false).MoveTo((BionicMassOxygenAbsorbChore.Instance smi) => smi.targetCell, this.absorb, this.move.fail, true);
			this.move.fail.ReturnFailure();
			this.absorb.ScheduleChange(this.fail, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.IsNotAllowedByScheduleAndChoreIsNotCritical)).ToggleTag(GameTags.RecoveringBreath).ToggleAnims("anim_bionic_absorb_kanim", 0f).Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ShowOxygenBar)).Exit(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.HideOxygenBar)).DefaultState(this.absorb.pre);
			this.absorb.pre.PlayAnim("absorb_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.absorb.loop).ScheduleGoTo(3f, this.absorb.loop).Exit(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer));
			this.absorb.loop.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer)).ParamTransition<float>(this.SecondsPassedWithoutOxygen, this.absorb.pst, (BionicMassOxygenAbsorbChore.Instance smi, float secondsPassed) => secondsPassed > smi.GetGiveupTimerTimeout()).OnSignal(this.TankFilledSignal, this.absorb.pst).PlayAnim("absorb_loop", KAnim.PlayMode.Loop).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.AbsorbUpdate), UpdateRate.SIM_200ms, false).Transition(this.absorb.pst, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.ChoreIsCriticalModeAndGiveUpOxygenLevelReached), UpdateRate.SIM_200ms);
			this.absorb.pst.Transition(this.absorb.criticalRecoverBreath.pre, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.IsCriticalChore), UpdateRate.SIM_200ms).PlayAnim("absorb_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.absorb.criticalRecoverBreath.ToggleAnims("anim_emotes_default_kanim", 0f).DefaultState(this.absorb.criticalRecoverBreath.pre);
			this.absorb.criticalRecoverBreath.pre.PlayAnim("breathe_pre").QueueAnim("breathe_loop", false, null).OnAnimQueueComplete(this.absorb.criticalRecoverBreath.loop);
			this.absorb.criticalRecoverBreath.loop.PlayAnim("breathe_loop", KAnim.PlayMode.Loop).ToggleAttributeModifier("Recovering Breath", (BionicMassOxygenAbsorbChore.Instance smi) => smi.recoveringbreath, null).Transition(this.absorb.criticalRecoverBreath.pst, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.BreathIsFull), UpdateRate.SIM_200ms).Transition(this.absorb.criticalRecoverBreath.pst, (BionicMassOxygenAbsorbChore.Instance smi) => smi.UpdateTargetCell() == Grid.InvalidCell, UpdateRate.SIM_200ms);
			this.absorb.criticalRecoverBreath.pst.PlayAnim("breathe_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.fail.ReturnFailure();
			this.complete.ReturnSuccess();
		}

		// Token: 0x040068E3 RID: 26851
		public BionicMassOxygenAbsorbChore.States.MoveStates move;

		// Token: 0x040068E4 RID: 26852
		public BionicMassOxygenAbsorbChore.States.MassAbsorbStates absorb;

		// Token: 0x040068E5 RID: 26853
		public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State fail;

		// Token: 0x040068E6 RID: 26854
		public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State complete;

		// Token: 0x040068E7 RID: 26855
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.FloatParameter SecondsPassedWithoutOxygen;

		// Token: 0x040068E8 RID: 26856
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.TargetParameter dupe;

		// Token: 0x040068E9 RID: 26857
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Signal TankFilledSignal;

		// Token: 0x020027A2 RID: 10146
		public class MoveStates : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
		{
			// Token: 0x0400AFC1 RID: 44993
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State onGoing;

			// Token: 0x0400AFC2 RID: 44994
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State fail;
		}

		// Token: 0x020027A3 RID: 10147
		public class MassAbsorbStates : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
		{
			// Token: 0x0400AFC3 RID: 44995
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pre;

			// Token: 0x0400AFC4 RID: 44996
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State loop;

			// Token: 0x0400AFC5 RID: 44997
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pst;

			// Token: 0x0400AFC6 RID: 44998
			public BionicMassOxygenAbsorbChore.States.MassAbsorbStates.CriticalRecover criticalRecoverBreath;

			// Token: 0x02003A3C RID: 14908
			public class CriticalRecover : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
			{
				// Token: 0x0400EB5C RID: 60252
				public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pre;

				// Token: 0x0400EB5D RID: 60253
				public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State loop;

				// Token: 0x0400EB5E RID: 60254
				public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pst;
			}
		}
	}

	// Token: 0x020012C7 RID: 4807
	public struct AbsorbUpdateData
	{
		// Token: 0x06008963 RID: 35171 RVA: 0x0035266E File Offset: 0x0035086E
		public AbsorbUpdateData(BionicMassOxygenAbsorbChore.Instance smi, float dt)
		{
			this.smi = smi;
			this.dt = dt;
		}

		// Token: 0x040068EA RID: 26858
		public BionicMassOxygenAbsorbChore.Instance smi;

		// Token: 0x040068EB RID: 26859
		public float dt;
	}

	// Token: 0x020012C8 RID: 4808
	public class Instance : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.GameInstance, BionicOxygenTankMonitor.IChore
	{
		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06008964 RID: 35172 RVA: 0x0035267E File Offset: 0x0035087E
		public float CRITICAL_OXYGEN_MASS_GIVE_UP_TRESHOLD
		{
			get
			{
				return this.oxygenBreather.ConsumptionRate * 8f;
			}
		}

		// Token: 0x06008965 RID: 35173 RVA: 0x00352691 File Offset: 0x00350891
		public float GetGiveupTimerTimeout()
		{
			if (this.oxygenTankMonitor == null)
			{
				return 2f;
			}
			if (!BionicOxygenTankMonitor.AreOxygenLevelsCritical(this.oxygenTankMonitor))
			{
				return 4f;
			}
			return 2f;
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06008967 RID: 35175 RVA: 0x003526C2 File Offset: 0x003508C2
		// (set) Token: 0x06008966 RID: 35174 RVA: 0x003526B9 File Offset: 0x003508B9
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x06008968 RID: 35176 RVA: 0x003526CC File Offset: 0x003508CC
		public Instance(BionicMassOxygenAbsorbChore master, GameObject duplicant) : base(master)
		{
			base.sm.dupe.Set(duplicant, base.smi, false);
			this.oxygenTankMonitor = duplicant.GetSMI<BionicOxygenTankMonitor.Instance>();
			this.oxygenBreather = duplicant.GetComponent<OxygenBreather>();
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float recover_BREATH_DELTA = DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA;
			this.recoveringbreath = new AttributeModifier(deltaAttribute.Id, recover_BREATH_DELTA, DUPLICANTS.MODIFIERS.RECOVERINGBREATH.NAME, false, false, true);
		}

		// Token: 0x06008969 RID: 35177 RVA: 0x0035276B File Offset: 0x0035096B
		public bool IsConsumingOxygen()
		{
			return !base.IsInsideState(base.sm.move);
		}

		// Token: 0x0600896A RID: 35178 RVA: 0x00352784 File Offset: 0x00350984
		public void ChangeCellReservation(int newCell)
		{
			if (this.targetCell != Grid.InvalidCell && Grid.Reserved[this.targetCell])
			{
				Grid.Reserved[this.targetCell] = false;
			}
			if (newCell != Grid.InvalidCell && !Grid.Reserved[newCell])
			{
				Grid.Reserved[newCell] = true;
			}
		}

		// Token: 0x0600896B RID: 35179 RVA: 0x003527E2 File Offset: 0x003509E2
		public override void StopSM(string reason)
		{
			this.ChangeCellReservation(Grid.InvalidCell);
			base.StopSM(reason);
		}

		// Token: 0x0600896C RID: 35180 RVA: 0x003527F8 File Offset: 0x003509F8
		public int UpdateTargetCell()
		{
			this.oxygenTankMonitor.UpdatePotentialCellToAbsorbOxygen(this.targetCell);
			int absorbOxygenCell = this.oxygenTankMonitor.AbsorbOxygenCell;
			this.ChangeCellReservation(absorbOxygenCell);
			this.targetCell = absorbOxygenCell;
			return absorbOxygenCell;
		}

		// Token: 0x0600896D RID: 35181 RVA: 0x00352834 File Offset: 0x00350A34
		public void ResetMassTrackHistory()
		{
			this.massAbsorbedHistory.Clear();
			for (int i = 0; i < 15; i++)
			{
				this.massAbsorbedHistory.Enqueue(0f);
			}
		}

		// Token: 0x0600896E RID: 35182 RVA: 0x00352869 File Offset: 0x00350A69
		public void AddMassToHistory(float mass_rate_this_tick)
		{
			if (this.massAbsorbedHistory.Count == 15)
			{
				this.massAbsorbedHistory.Dequeue();
			}
			this.massAbsorbedHistory.Enqueue(mass_rate_this_tick);
		}

		// Token: 0x0600896F RID: 35183 RVA: 0x00352894 File Offset: 0x00350A94
		public float GetAverageMassConsumedPerSecond()
		{
			float num = 0f;
			int num2 = 0;
			foreach (float num3 in this.massAbsorbedHistory)
			{
				num += num3;
				num2++;
			}
			if (num2 <= 0)
			{
				return 0f;
			}
			num /= (float)num2;
			return num;
		}

		// Token: 0x06008970 RID: 35184 RVA: 0x00352900 File Offset: 0x00350B00
		public void OnSimConsume(Sim.MassConsumedCallback mass_cb_info, float dt)
		{
			if (this.oxygenBreather == null || this.oxygenTankMonitor == null || this.oxygenBreather.prefabID.HasTag(GameTags.Dead))
			{
				return;
			}
			this.AddMassToHistory(mass_cb_info.mass / dt);
			GameObject gameObject = this.oxygenBreather.gameObject;
			bool flag = BionicOxygenTankMonitor.AreOxygenLevelsCritical(this.oxygenTankMonitor);
			float num = flag ? this.CRITICAL_OXYGEN_MASS_GIVE_UP_TRESHOLD : 2f;
			if (this.GetAverageMassConsumedPerSecond() <= num)
			{
				base.sm.SecondsPassedWithoutOxygen.Set(base.sm.SecondsPassedWithoutOxygen.Get(base.smi) + dt, base.smi, false);
			}
			else
			{
				BionicMassOxygenAbsorbChore.ResetOxygenTimer(base.smi);
			}
			if (flag)
			{
				float num2 = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE * DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND;
				if (mass_cb_info.mass == 0f)
				{
					mass_cb_info.temperature = DUPLICANTSTATS.BIONICS.Temperature.Internal.IDEAL;
				}
				mass_cb_info.mass += DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA * num2 * dt + DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * dt;
			}
			float num3 = this.oxygenTankMonitor.AddGas(mass_cb_info);
			if (num3 > Mathf.Epsilon)
			{
				SimMessages.EmitMass(Grid.PosToCell(gameObject), mass_cb_info.elemIdx, num3, mass_cb_info.temperature, byte.MaxValue, 0, -1);
			}
			if (!BionicMassOxygenAbsorbChore.HasSpaceInOxygenTank(this))
			{
				base.sm.TankFilledSignal.Trigger(this);
			}
		}

		// Token: 0x06008971 RID: 35185 RVA: 0x00352A83 File Offset: 0x00350C83
		public float GetOxygen()
		{
			if (this.oxygenTankMonitor != null)
			{
				return this.oxygenTankMonitor.OxygenPercentage;
			}
			return 0f;
		}

		// Token: 0x040068EC RID: 26860
		public AttributeModifier recoveringbreath;

		// Token: 0x040068EE RID: 26862
		public Queue<float> massAbsorbedHistory = new Queue<float>();

		// Token: 0x040068EF RID: 26863
		public int targetCell = Grid.InvalidCell;

		// Token: 0x040068F0 RID: 26864
		public BionicOxygenTankMonitor.Instance oxygenTankMonitor;
	}
}
