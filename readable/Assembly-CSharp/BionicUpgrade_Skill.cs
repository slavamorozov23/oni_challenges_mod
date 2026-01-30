using System;

// Token: 0x020006EF RID: 1775
public class BionicUpgrade_Skill : GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>
{
	// Token: 0x06002BD3 RID: 11219 RVA: 0x000FF770 File Offset: 0x000FD970
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
		this.root.Enter(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.DisableEffect));
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x000FF7AA File Offset: 0x000FD9AA
	public static void EnableEffect(BionicUpgrade_Skill.Instance smi)
	{
		smi.ApplySkill();
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x000FF7B2 File Offset: 0x000FD9B2
	public static void DisableEffect(BionicUpgrade_Skill.Instance smi)
	{
		smi.RemoveSkill();
	}

	// Token: 0x020015B0 RID: 5552
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007264 RID: 29284
		public string SKILL_ID;
	}

	// Token: 0x020015B1 RID: 5553
	public new class Instance : GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.GameInstance
	{
		// Token: 0x0600942A RID: 37930 RVA: 0x00377B51 File Offset: 0x00375D51
		public Instance(IStateMachineTarget master, BionicUpgrade_Skill.Def def) : base(master, def)
		{
			this.resume = base.GetComponent<MinionResume>();
		}

		// Token: 0x0600942B RID: 37931 RVA: 0x00377B67 File Offset: 0x00375D67
		public void ApplySkill()
		{
			this.resume.GrantSkill(base.def.SKILL_ID);
		}

		// Token: 0x0600942C RID: 37932 RVA: 0x00377B7F File Offset: 0x00375D7F
		public void RemoveSkill()
		{
			this.resume.UngrantSkill(base.def.SKILL_ID);
		}

		// Token: 0x04007265 RID: 29285
		private MinionResume resume;
	}
}
