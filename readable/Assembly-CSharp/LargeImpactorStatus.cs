using System;
using UnityEngine;

// Token: 0x02000B8A RID: 2954
public class LargeImpactorStatus : GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>
{
	// Token: 0x0600582B RID: 22571 RVA: 0x002007C4 File Offset: 0x001FE9C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.alive;
		this.alive.ParamTransition<bool>(this.HasArrived, this.landing, GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.IsTrue).ParamTransition<int>(this.Health, this.destroyed, GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.IsZero_Int).EventHandler(GameHashes.MissileDamageEncountered, new GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.GameEvent.Callback(LargeImpactorStatus.HandleIncommingDamage)).ToggleStatusItem(Db.Get().MiscStatusItems.ImpactorHealth, null).EventTransition(GameHashes.ClusterDestinationReached, this.landing, null).UpdateTransition(this.landing, new Func<LargeImpactorStatus.Instance, float, bool>(LargeImpactorStatus.CheckArrivalUpdate), UpdateRate.SIM_200ms, false);
		this.landing.Enter(new StateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.State.Callback(LargeImpactorStatus.SetHasArrived)).TriggerOnEnter(GameHashes.LargeImpactorArrived, null);
		this.destroyed.TriggerOnEnter(GameHashes.Died, null);
	}

	// Token: 0x0600582C RID: 22572 RVA: 0x0020089D File Offset: 0x001FEA9D
	private static void HandleIncommingDamage(LargeImpactorStatus.Instance smi, object obj)
	{
		LargeImpactorStatus.DealDamage(smi, (obj as MissileLongRangeConfig.DamageEventPayload).damage);
	}

	// Token: 0x0600582D RID: 22573 RVA: 0x002008B0 File Offset: 0x001FEAB0
	private static void SetHasArrived(LargeImpactorStatus.Instance smi)
	{
		smi.sm.HasArrived.Set(true, smi, false);
	}

	// Token: 0x0600582E RID: 22574 RVA: 0x002008C6 File Offset: 0x001FEAC6
	private static void DealDamage(LargeImpactorStatus.Instance smi, int damage)
	{
		smi.DealDamage(damage);
	}

	// Token: 0x0600582F RID: 22575 RVA: 0x002008CF File Offset: 0x001FEACF
	private static void DeleteObject(LargeImpactorStatus.Instance smi)
	{
		smi.gameObject.DeleteObject();
	}

	// Token: 0x06005830 RID: 22576 RVA: 0x002008DC File Offset: 0x001FEADC
	private static bool CheckArrivalUpdate(LargeImpactorStatus.Instance smi, float dt)
	{
		return smi.TimeRemainingBeforeCollision <= 0f;
	}

	// Token: 0x04003B38 RID: 15160
	public StateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.IntParameter Health;

	// Token: 0x04003B39 RID: 15161
	public StateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.BoolParameter HasArrived;

	// Token: 0x04003B3A RID: 15162
	public GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.State alive;

	// Token: 0x04003B3B RID: 15163
	public GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.State landing;

	// Token: 0x04003B3C RID: 15164
	public GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.State destroyed;

	// Token: 0x02001D11 RID: 7441
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008A3B RID: 35387
		public int MAX_HEALTH;

		// Token: 0x04008A3C RID: 35388
		public string EventID;
	}

	// Token: 0x02001D12 RID: 7442
	public new class Instance : GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.GameInstance
	{
		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x0600AFE4 RID: 45028 RVA: 0x003D7E12 File Offset: 0x003D6012
		public int Health
		{
			get
			{
				return base.sm.Health.Get(this);
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x0600AFE5 RID: 45029 RVA: 0x003D7E25 File Offset: 0x003D6025
		public float ArrivalTime
		{
			get
			{
				if (!(this.clusterTraveler == null))
				{
					return this.ArrivalTime_SO;
				}
				return this.ArrivalTime_Vanilla;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x0600AFE6 RID: 45030 RVA: 0x003D7E42 File Offset: 0x003D6042
		public float TimeRemainingBeforeCollision
		{
			get
			{
				if (!(this.clusterTraveler == null))
				{
					return this.TimeRemainingBeforeCollision_SO;
				}
				return this.TimeRemainingBeforeCollision_Vanilla;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x0600AFE7 RID: 45031 RVA: 0x003D7E5F File Offset: 0x003D605F
		private float ArrivalTime_Vanilla
		{
			get
			{
				return this.eventInstance.eventStartTime * 600f + LargeImpactorEvent.GetImpactTime();
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x0600AFE8 RID: 45032 RVA: 0x003D7E78 File Offset: 0x003D6078
		private float TimeRemainingBeforeCollision_Vanilla
		{
			get
			{
				return Mathf.Clamp(this.ArrivalTime_Vanilla - GameUtil.GetCurrentTimeInCycles() * 600f, 0f, float.MaxValue);
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x0600AFE9 RID: 45033 RVA: 0x003D7E9B File Offset: 0x003D609B
		private float ArrivalTime_SO
		{
			get
			{
				return GameUtil.GetCurrentTimeInCycles() * 600f + this.TimeRemainingBeforeCollision_SO;
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x0600AFEA RID: 45034 RVA: 0x003D7EAF File Offset: 0x003D60AF
		private float TimeRemainingBeforeCollision_SO
		{
			get
			{
				return Mathf.Clamp(this.clusterTraveler.EstimatedTimeToReachDestination(), 0f, float.MaxValue);
			}
		}

		// Token: 0x0600AFEB RID: 45035 RVA: 0x003D7ECB File Offset: 0x003D60CB
		public Instance(IStateMachineTarget master, LargeImpactorStatus.Def def) : base(master, def)
		{
			base.sm.Health.Set(def.MAX_HEALTH, base.smi, false);
		}

		// Token: 0x0600AFEC RID: 45036 RVA: 0x003D7EF3 File Offset: 0x003D60F3
		public override void StartSM()
		{
			this.clusterTraveler = base.GetComponent<ClusterTraveler>();
			this.eventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(base.def.EventID, -1);
			base.StartSM();
		}

		// Token: 0x0600AFED RID: 45037 RVA: 0x003D7F28 File Offset: 0x003D6128
		public void DealDamage(int damage)
		{
			int value = Mathf.Clamp(this.Health - damage, 0, base.def.MAX_HEALTH);
			base.sm.Health.Set(value, this, false);
			Action<int> onDamaged = this.OnDamaged;
			if (onDamaged == null)
			{
				return;
			}
			onDamaged(this.Health);
		}

		// Token: 0x04008A3D RID: 35389
		public Action<int> OnDamaged;

		// Token: 0x04008A3E RID: 35390
		private ClusterTraveler clusterTraveler;

		// Token: 0x04008A3F RID: 35391
		private GameplayEventInstance eventInstance;
	}
}
