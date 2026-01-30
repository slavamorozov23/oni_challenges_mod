using System;
using UnityEngine;

// Token: 0x02000A30 RID: 2608
public class IncapacitationMonitor : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance>
{
	// Token: 0x06004C31 RID: 19505 RVA: 0x001BAC40 File Offset: 0x001B8E40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.healthy;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.healthy.Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.RecoverBleedOutStamina(dt, smi);
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.BecameIncapacitated, this.incapacitated, null);
		this.incapacitated.EventTransition(GameHashes.IncapacitationRecovery, this.healthy, null).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((IncapacitationMonitor.Instance smi) => new BeIncapacitatedChore(smi.master), null).ParamTransition<float>(this.bleedOutStamina, this.die, GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ToggleUrge(Db.Get().Urges.BeIncapacitated).Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.Bleed(dt, smi);
		}, UpdateRate.SIM_200ms, false);
		this.die.Enter(delegate(IncapacitationMonitor.Instance smi)
		{
			smi.master.gameObject.GetSMI<DeathMonitor.Instance>().Kill(smi.GetCauseOfIncapacitation());
		});
	}

	// Token: 0x04003295 RID: 12949
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x04003296 RID: 12950
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State start_recovery;

	// Token: 0x04003297 RID: 12951
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x04003298 RID: 12952
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State die;

	// Token: 0x04003299 RID: 12953
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter bleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x0400329A RID: 12954
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseBleedOutSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x0400329B RID: 12955
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseStaminaRecoverSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x0400329C RID: 12956
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter maxBleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x02001AFA RID: 6906
	public new class Instance : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A7ED RID: 42989 RVA: 0x003BDFB0 File Offset: 0x003BC1B0
		public Instance(IStateMachineTarget master) : base(master)
		{
			Health component = master.GetComponent<Health>();
			if (component)
			{
				component.canBeIncapacitated = true;
			}
		}

		// Token: 0x0600A7EE RID: 42990 RVA: 0x003BDFDA File Offset: 0x003BC1DA
		public void Bleed(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(dt * -smi.sm.baseBleedOutSpeed.Get(smi), smi);
		}

		// Token: 0x0600A7EF RID: 42991 RVA: 0x003BE004 File Offset: 0x003BC204
		public void RecoverBleedOutStamina(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(Mathf.Min(dt * smi.sm.baseStaminaRecoverSpeed.Get(smi), smi.sm.maxBleedOutStamina.Get(smi) - smi.sm.bleedOutStamina.Get(smi)), smi);
		}

		// Token: 0x0600A7F0 RID: 42992 RVA: 0x003BE05E File Offset: 0x003BC25E
		public float GetBleedLifeTime(IncapacitationMonitor.Instance smi)
		{
			return Mathf.Floor(smi.sm.bleedOutStamina.Get(smi) / smi.sm.baseBleedOutSpeed.Get(smi));
		}

		// Token: 0x0600A7F1 RID: 42993 RVA: 0x003BE088 File Offset: 0x003BC288
		public Death GetCauseOfIncapacitation()
		{
			Health component = base.GetComponent<Health>();
			if (component.CauseOfIncapacitation == GameTags.RadiationSicknessIncapacitation)
			{
				return Db.Get().Deaths.Radiation;
			}
			if (component.CauseOfIncapacitation == GameTags.HitPointsDepleted)
			{
				return Db.Get().Deaths.Slain;
			}
			return Db.Get().Deaths.Generic;
		}
	}
}
