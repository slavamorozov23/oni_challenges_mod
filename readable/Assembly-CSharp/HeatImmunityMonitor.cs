using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A2C RID: 2604
public class HeatImmunityMonitor : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance>
{
	// Token: 0x06004C1F RID: 19487 RVA: 0x001BA6B4 File Offset: 0x001B88B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingWarm, this.warm, false).ParamTransition<float>(this.heatCountdown, this.warm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.idle.feelingFine.DoNothing();
		this.idle.leftWithDesireToCooldownAfterBeingWarm.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), this.idle.feelingFine, this.idle.feelingFine);
		this.warm.DefaultState(this.warm.exiting).TagTransition(GameTags.FeelingCold, this.idle, false).ToggleAnims("anim_idle_hot_kanim", 0f).ToggleAnims("anim_loco_run_hot_kanim", 0f).ToggleAnims("anim_loco_walk_hot_kanim", 0f).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleEffect("WarmAir").Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), this.idle, this.warm);
		this.warm.exiting.EventHandlerTransition(GameHashes.EffectAdded, this.idle, new Func<HeatImmunityMonitor.Instance, object, bool>(HeatImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingWarm, this.warm.idle, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingHot, null).ParamTransition<float>(this.heatCountdown, this.idle.leftWithDesireToCooldownAfterBeingWarm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.HeatTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ClearTimer));
		this.warm.idle.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ResetHeatTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hot, (HeatImmunityMonitor.Instance smi) => smi).TagTransition(GameTags.FeelingWarm, this.warm.exiting, true);
	}

	// Token: 0x06004C20 RID: 19488 RVA: 0x001BA938 File Offset: 0x001B8B38
	public static bool OnEffectAdded(HeatImmunityMonitor.Instance smi, object data)
	{
		return true;
	}

	// Token: 0x06004C21 RID: 19489 RVA: 0x001BA93B File Offset: 0x001B8B3B
	public static void ClearTimer(HeatImmunityMonitor.Instance smi)
	{
		smi.sm.heatCountdown.Set(0f, smi, false);
	}

	// Token: 0x06004C22 RID: 19490 RVA: 0x001BA955 File Offset: 0x001B8B55
	public static void ResetHeatTimer(HeatImmunityMonitor.Instance smi)
	{
		smi.sm.heatCountdown.Set(5f, smi, false);
	}

	// Token: 0x06004C23 RID: 19491 RVA: 0x001BA970 File Offset: 0x001B8B70
	public static void HeatTimerUpdate(HeatImmunityMonitor.Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.HeatCountdown - dt, 0f, 5f);
		smi.sm.heatCountdown.Set(value, smi, false);
	}

	// Token: 0x06004C24 RID: 19492 RVA: 0x001BA9A9 File Offset: 0x001B8BA9
	private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi, float dt)
	{
		smi.UpdateShelterCell();
	}

	// Token: 0x06004C25 RID: 19493 RVA: 0x001BA9B1 File Offset: 0x001B8BB1
	private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi)
	{
		smi.UpdateShelterCell();
	}

	// Token: 0x06004C26 RID: 19494 RVA: 0x001BA9BC File Offset: 0x001B8BBC
	public static bool HasImmunityEffect(HeatImmunityMonitor.Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		return component != null && component.HasEffect("RefreshingTouch");
	}

	// Token: 0x06004C27 RID: 19495 RVA: 0x001BA9E6 File Offset: 0x001B8BE6
	private static Chore CreateRecoverFromOverheatChore(HeatImmunityMonitor.Instance smi)
	{
		return new RecoverFromHeatChore(smi.master);
	}

	// Token: 0x04003289 RID: 12937
	private const float EFFECT_DURATION = 5f;

	// Token: 0x0400328A RID: 12938
	public HeatImmunityMonitor.IdleStates idle;

	// Token: 0x0400328B RID: 12939
	public HeatImmunityMonitor.WarmStates warm;

	// Token: 0x0400328C RID: 12940
	public StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter heatCountdown;

	// Token: 0x02001AF1 RID: 6897
	public class WarmStates : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400834F RID: 33615
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04008350 RID: 33616
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;

		// Token: 0x04008351 RID: 33617
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
	}

	// Token: 0x02001AF2 RID: 6898
	public class IdleStates : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04008352 RID: 33618
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;

		// Token: 0x04008353 RID: 33619
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToCooldownAfterBeingWarm;
	}

	// Token: 0x02001AF3 RID: 6899
	public new class Instance : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x0600A7CF RID: 42959 RVA: 0x003BDD15 File Offset: 0x003BBF15
		// (set) Token: 0x0600A7D0 RID: 42960 RVA: 0x003BDD1D File Offset: 0x003BBF1D
		public HeatImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x0600A7D1 RID: 42961 RVA: 0x003BDD26 File Offset: 0x003BBF26
		// (set) Token: 0x0600A7D2 RID: 42962 RVA: 0x003BDD2E File Offset: 0x003BBF2E
		public int ShelterCell { get; private set; }

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x0600A7D3 RID: 42963 RVA: 0x003BDD37 File Offset: 0x003BBF37
		public float HeatCountdown
		{
			get
			{
				return base.smi.sm.heatCountdown.Get(this);
			}
		}

		// Token: 0x0600A7D4 RID: 42964 RVA: 0x003BDD4F File Offset: 0x003BBF4F
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A7D5 RID: 42965 RVA: 0x003BDD58 File Offset: 0x003BBF58
		public override void StartSM()
		{
			this.navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		// Token: 0x0600A7D6 RID: 42966 RVA: 0x003BDD74 File Offset: 0x003BBF74
		public void UpdateShelterCell()
		{
			int myWorldId = this.navigator.GetMyWorldId();
			int shelterCell = Grid.InvalidCell;
			int num = int.MaxValue;
			HeatImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance instance in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is HeatImmunityProvider.Instance))
			{
				HeatImmunityProvider.Instance instance2 = instance as HeatImmunityProvider.Instance;
				if (instance2.GetMyWorldId() == myWorldId)
				{
					int maxValue = int.MaxValue;
					int bestAvailableCell = instance2.GetBestAvailableCell(this.navigator, out maxValue);
					if (maxValue < num)
					{
						num = maxValue;
						nearestImmunityProvider = instance2;
						shelterCell = bestAvailableCell;
					}
				}
			}
			this.NearestImmunityProvider = nearestImmunityProvider;
			this.ShelterCell = shelterCell;
		}

		// Token: 0x04008356 RID: 33622
		private Navigator navigator;
	}
}
