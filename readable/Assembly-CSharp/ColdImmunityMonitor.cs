using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A15 RID: 2581
public class ColdImmunityMonitor : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance>
{
	// Token: 0x06004BB6 RID: 19382 RVA: 0x001B809C File Offset: 0x001B629C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingCold, this.cold, false).ParamTransition<float>(this.coldCountdown, this.cold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.idle.feelingFine.DoNothing();
		this.idle.leftWithDesireToWarmupAfterBeingCold.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), this.idle.feelingFine, this.idle.feelingFine);
		this.cold.DefaultState(this.cold.exiting).TagTransition(GameTags.FeelingWarm, this.idle, false).ToggleAnims("anim_idle_cold_kanim", 0f).ToggleAnims("anim_loco_run_cold_kanim", 0f).ToggleAnims("anim_loco_walk_cold_kanim", 0f).ToggleExpression(Db.Get().Expressions.Cold, null).ToggleThought(Db.Get().Thoughts.Cold, null).ToggleEffect("ColdAir").Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), this.idle, this.cold);
		this.cold.exiting.EventHandlerTransition(GameHashes.EffectAdded, this.idle, new Func<ColdImmunityMonitor.Instance, object, bool>(ColdImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingCold, this.cold.idle, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingCold, null).ParamTransition<float>(this.coldCountdown, this.idle.leftWithDesireToWarmupAfterBeingCold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.ColdTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ClearTimer));
		this.cold.idle.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ResetColdTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Cold, (ColdImmunityMonitor.Instance smi) => smi).TagTransition(GameTags.FeelingCold, this.cold.exiting, true);
	}

	// Token: 0x06004BB7 RID: 19383 RVA: 0x001B8320 File Offset: 0x001B6520
	public static bool OnEffectAdded(ColdImmunityMonitor.Instance smi, object data)
	{
		return true;
	}

	// Token: 0x06004BB8 RID: 19384 RVA: 0x001B8323 File Offset: 0x001B6523
	public static void ClearTimer(ColdImmunityMonitor.Instance smi)
	{
		smi.sm.coldCountdown.Set(0f, smi, false);
	}

	// Token: 0x06004BB9 RID: 19385 RVA: 0x001B833D File Offset: 0x001B653D
	public static void ResetColdTimer(ColdImmunityMonitor.Instance smi)
	{
		smi.sm.coldCountdown.Set(5f, smi, false);
	}

	// Token: 0x06004BBA RID: 19386 RVA: 0x001B8358 File Offset: 0x001B6558
	public static void ColdTimerUpdate(ColdImmunityMonitor.Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.ColdCountdown - dt, 0f, 5f);
		smi.sm.coldCountdown.Set(value, smi, false);
	}

	// Token: 0x06004BBB RID: 19387 RVA: 0x001B8391 File Offset: 0x001B6591
	private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi, float dt)
	{
		smi.UpdateWarmUpCell();
	}

	// Token: 0x06004BBC RID: 19388 RVA: 0x001B8399 File Offset: 0x001B6599
	private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi)
	{
		smi.UpdateWarmUpCell();
	}

	// Token: 0x06004BBD RID: 19389 RVA: 0x001B83A4 File Offset: 0x001B65A4
	public static bool HasImmunityEffect(ColdImmunityMonitor.Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		return component != null && component.HasEffect("WarmTouch");
	}

	// Token: 0x06004BBE RID: 19390 RVA: 0x001B83CE File Offset: 0x001B65CE
	private static Chore CreateRecoverFromChillyBonesChore(ColdImmunityMonitor.Instance smi)
	{
		return new RecoverFromColdChore(smi.master);
	}

	// Token: 0x0400322D RID: 12845
	private const float EFFECT_DURATION = 5f;

	// Token: 0x0400322E RID: 12846
	public ColdImmunityMonitor.IdleStates idle;

	// Token: 0x0400322F RID: 12847
	public ColdImmunityMonitor.ColdStates cold;

	// Token: 0x04003230 RID: 12848
	public StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter coldCountdown;

	// Token: 0x02001AB0 RID: 6832
	public class ColdStates : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400827E RID: 33406
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x0400827F RID: 33407
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;

		// Token: 0x04008280 RID: 33408
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
	}

	// Token: 0x02001AB1 RID: 6833
	public class IdleStates : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04008281 RID: 33409
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;

		// Token: 0x04008282 RID: 33410
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToWarmupAfterBeingCold;
	}

	// Token: 0x02001AB2 RID: 6834
	public new class Instance : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x0600A6CA RID: 42698 RVA: 0x003BAB0E File Offset: 0x003B8D0E
		// (set) Token: 0x0600A6CB RID: 42699 RVA: 0x003BAB16 File Offset: 0x003B8D16
		public ColdImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x0600A6CC RID: 42700 RVA: 0x003BAB1F File Offset: 0x003B8D1F
		// (set) Token: 0x0600A6CD RID: 42701 RVA: 0x003BAB27 File Offset: 0x003B8D27
		public int WarmUpCell { get; private set; }

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600A6CE RID: 42702 RVA: 0x003BAB30 File Offset: 0x003B8D30
		public float ColdCountdown
		{
			get
			{
				return base.smi.sm.coldCountdown.Get(this);
			}
		}

		// Token: 0x0600A6CF RID: 42703 RVA: 0x003BAB48 File Offset: 0x003B8D48
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A6D0 RID: 42704 RVA: 0x003BAB51 File Offset: 0x003B8D51
		public override void StartSM()
		{
			this.navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		// Token: 0x0600A6D1 RID: 42705 RVA: 0x003BAB6C File Offset: 0x003B8D6C
		public void UpdateWarmUpCell()
		{
			int myWorldId = this.navigator.GetMyWorldId();
			int warmUpCell = Grid.InvalidCell;
			int num = int.MaxValue;
			ColdImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance instance in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is ColdImmunityProvider.Instance))
			{
				ColdImmunityProvider.Instance instance2 = instance as ColdImmunityProvider.Instance;
				if (instance2.GetMyWorldId() == myWorldId)
				{
					int maxValue = int.MaxValue;
					int bestAvailableCell = instance2.GetBestAvailableCell(this.navigator, out maxValue);
					if (maxValue < num)
					{
						num = maxValue;
						nearestImmunityProvider = instance2;
						warmUpCell = bestAvailableCell;
					}
				}
			}
			this.NearestImmunityProvider = nearestImmunityProvider;
			this.WarmUpCell = warmUpCell;
		}

		// Token: 0x04008285 RID: 33413
		private Navigator navigator;
	}
}
