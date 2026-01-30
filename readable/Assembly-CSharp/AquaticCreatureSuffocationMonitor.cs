using System;
using UnityEngine;

// Token: 0x020005B1 RID: 1457
public class AquaticCreatureSuffocationMonitor : GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>
{
	// Token: 0x06002172 RID: 8562 RVA: 0x000C2500 File Offset: 0x000C0700
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.root.TagTransition(GameTags.Dead, this.dead, false);
		this.safe.Transition(this.suffocating, new StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.Transition.ConditionCallback(AquaticCreatureSuffocationMonitor.IsSuffocating), UpdateRate.SIM_1000ms).Update(new Action<AquaticCreatureSuffocationMonitor.Instance, float>(AquaticCreatureSuffocationMonitor.RecoveryDeathTimerUpdate), UpdateRate.SIM_200ms, false);
		this.suffocating.ParamTransition<float>(this.DeathTimer, this.die, new StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.Parameter<float>.Callback(AquaticCreatureSuffocationMonitor.CanNotHoldAnymore)).Transition(this.safe, new StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.Transition.ConditionCallback(AquaticCreatureSuffocationMonitor.CanBreath), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.AquaticCreatureSuffocating, null).Update(new Action<AquaticCreatureSuffocationMonitor.Instance, float>(AquaticCreatureSuffocationMonitor.DeathTimerUpdate), UpdateRate.SIM_200ms, false);
		this.die.Enter(new StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State.Callback(AquaticCreatureSuffocationMonitor.Kill));
		this.dead.DoNothing();
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x000C25EE File Offset: 0x000C07EE
	public static bool IsSuffocating(AquaticCreatureSuffocationMonitor.Instance smi)
	{
		return !smi.CanBreath();
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x000C25F9 File Offset: 0x000C07F9
	public static bool CanBreath(AquaticCreatureSuffocationMonitor.Instance smi)
	{
		return smi.CanBreath();
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x000C2601 File Offset: 0x000C0801
	public static bool CanNotHoldAnymore(AquaticCreatureSuffocationMonitor.Instance smi, float deathTimerValue)
	{
		return deathTimerValue > smi.def.DeathTimerDuration;
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x000C2611 File Offset: 0x000C0811
	public static void DeathTimerUpdate(AquaticCreatureSuffocationMonitor.Instance smi, float dt)
	{
		smi.sm.DeathTimer.Set(smi.DeathTimerValue + dt, smi, false);
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x000C262E File Offset: 0x000C082E
	public static void RecoveryDeathTimerUpdate(AquaticCreatureSuffocationMonitor.Instance smi, float dt)
	{
		if (smi.DeathTimerValue > 0f)
		{
			smi.sm.DeathTimer.Set(Mathf.Max(smi.DeathTimerValue - dt * smi.def.RecoveryModifier, 0f), smi, false);
		}
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x000C266E File Offset: 0x000C086E
	public static void Kill(AquaticCreatureSuffocationMonitor.Instance smi)
	{
		smi.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
	}

	// Token: 0x04001385 RID: 4997
	public GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State safe;

	// Token: 0x04001386 RID: 4998
	public GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State suffocating;

	// Token: 0x04001387 RID: 4999
	public GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State die;

	// Token: 0x04001388 RID: 5000
	public GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State dead;

	// Token: 0x04001389 RID: 5001
	public StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.FloatParameter DeathTimer;

	// Token: 0x02001441 RID: 5185
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006DFC RID: 28156
		public float DeathTimerDuration = 2400f;

		// Token: 0x04006DFD RID: 28157
		public float RecoveryModifier = 4f;
	}

	// Token: 0x02001442 RID: 5186
	public new class Instance : GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.GameInstance
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06008F1D RID: 36637 RVA: 0x0036A5C9 File Offset: 0x003687C9
		public float DeathTimerValue
		{
			get
			{
				return base.sm.DeathTimer.Get(this);
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06008F1E RID: 36638 RVA: 0x0036A5DC File Offset: 0x003687DC
		public float TimeUntilDeath
		{
			get
			{
				return Mathf.Max(base.smi.def.DeathTimerDuration - this.DeathTimerValue, 0f);
			}
		}

		// Token: 0x06008F1F RID: 36639 RVA: 0x0036A5FF File Offset: 0x003687FF
		public Instance(IStateMachineTarget master, AquaticCreatureSuffocationMonitor.Def def) : base(master, def)
		{
			this.pickupable = base.GetComponent<Pickupable>();
		}

		// Token: 0x06008F20 RID: 36640 RVA: 0x0036A618 File Offset: 0x00368818
		public bool CanBreath()
		{
			int cell = Grid.PosToCell(this);
			return !(this.pickupable.storage == null) || Grid.IsSubstantialLiquid(cell, 0.35f);
		}

		// Token: 0x04006DFE RID: 28158
		private Pickupable pickupable;
	}
}
