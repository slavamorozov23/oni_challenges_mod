using System;
using Klei.AI;

// Token: 0x020006EA RID: 1770
public class BionicUpgrade_Effect : GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>
{
	// Token: 0x06002BBA RID: 11194 RVA: 0x000FF0D4 File Offset: 0x000FD2D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
		this.root.Enter(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.DisableEffect));
	}

	// Token: 0x06002BBB RID: 11195 RVA: 0x000FF10E File Offset: 0x000FD30E
	public static void EnableEffect(BionicUpgrade_Effect.Instance smi)
	{
		smi.ApplyEffect();
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x000FF116 File Offset: 0x000FD316
	public static void DisableEffect(BionicUpgrade_Effect.Instance smi)
	{
		smi.RemoveEffect();
	}

	// Token: 0x020015A3 RID: 5539
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007252 RID: 29266
		public string EFFECT_NAME;
	}

	// Token: 0x020015A4 RID: 5540
	public new class Instance : GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.GameInstance
	{
		// Token: 0x060093F9 RID: 37881 RVA: 0x0037752A File Offset: 0x0037572A
		public Instance(IStateMachineTarget master, BionicUpgrade_Effect.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x060093FA RID: 37882 RVA: 0x00377540 File Offset: 0x00375740
		public void ApplyEffect()
		{
			Effect newEffect = Db.Get().effects.Get(base.def.EFFECT_NAME);
			this.effects.Add(newEffect, false);
		}

		// Token: 0x060093FB RID: 37883 RVA: 0x00377578 File Offset: 0x00375778
		public void RemoveEffect()
		{
			Effect effect = Db.Get().effects.Get(base.def.EFFECT_NAME);
			this.effects.Remove(effect);
		}

		// Token: 0x04007253 RID: 29267
		private Effects effects;
	}
}
