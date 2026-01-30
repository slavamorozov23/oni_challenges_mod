using System;
using STRINGS;

// Token: 0x020006ED RID: 1773
public class BionicUpgrade_OnGoingEffect : BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>
{
	// Token: 0x06002BC9 RID: 11209 RVA: 0x000FF5E8 File Offset: 0x000FD7E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.ToggleEffect(new Func<BionicUpgrade_OnGoingEffect.Instance, string>(BionicUpgrade_OnGoingEffect.GetEffectName)).Enter(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.State.Callback(BionicUpgrade_OnGoingEffect.ApplySkills)).Exit(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.State.Callback(BionicUpgrade_OnGoingEffect.RemoveSkills)).EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.ScheduleChanged, this.Inactive, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.BionicOffline, this.Inactive, GameStateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Not(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsOnline))).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
	}

	// Token: 0x06002BCA RID: 11210 RVA: 0x000FF708 File Offset: 0x000FD908
	public static string GetEffectName(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		return ((BionicUpgrade_OnGoingEffect.Def)smi.def).EFFECT_NAME;
	}

	// Token: 0x06002BCB RID: 11211 RVA: 0x000FF71A File Offset: 0x000FD91A
	public static void ApplySkills(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		smi.ApplySkills();
	}

	// Token: 0x06002BCC RID: 11212 RVA: 0x000FF722 File Offset: 0x000FD922
	public static void RemoveSkills(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		smi.RemoveSkills();
	}

	// Token: 0x06002BCD RID: 11213 RVA: 0x000FF72A File Offset: 0x000FD92A
	public static bool IsOnlineAndNotInBatterySaveMode(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsOnline(smi) && !BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBedTimeChore(smi);
	}

	// Token: 0x020015AC RID: 5548
	public new class Def : BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def
	{
		// Token: 0x06009417 RID: 37911 RVA: 0x0037789E File Offset: 0x00375A9E
		public Def(string upgradeID, string effectID, string[] skills = null) : base(upgradeID)
		{
			this.EFFECT_NAME = effectID;
			this.SKILLS_IDS = skills;
		}

		// Token: 0x06009418 RID: 37912 RVA: 0x003778B5 File Offset: 0x00375AB5
		public override string GetDescription()
		{
			return "BionicUpgrade_OnGoingEffect.Def description not implemented";
		}

		// Token: 0x0400725C RID: 29276
		public string EFFECT_NAME;

		// Token: 0x0400725D RID: 29277
		public string[] SKILLS_IDS;
	}

	// Token: 0x020015AD RID: 5549
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.BaseInstance
	{
		// Token: 0x06009419 RID: 37913 RVA: 0x003778BC File Offset: 0x00375ABC
		public Instance(IStateMachineTarget master, BionicUpgrade_OnGoingEffect.Def def) : base(master, def)
		{
			this.resume = base.GetComponent<MinionResume>();
		}

		// Token: 0x0600941A RID: 37914 RVA: 0x003778D2 File Offset: 0x00375AD2
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x0600941B RID: 37915 RVA: 0x003778F8 File Offset: 0x00375AF8
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				string str = "<b>" + ((currentWattageCost >= 0f) ? "+" : "-") + "</b>";
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), str + GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x0600941C RID: 37916 RVA: 0x00377998 File Offset: 0x00375B98
		public void ApplySkills()
		{
			BionicUpgrade_OnGoingEffect.Def def = (BionicUpgrade_OnGoingEffect.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.GrantSkill(skillId);
				}
			}
		}

		// Token: 0x0600941D RID: 37917 RVA: 0x003779E4 File Offset: 0x00375BE4
		public void RemoveSkills()
		{
			BionicUpgrade_OnGoingEffect.Def def = (BionicUpgrade_OnGoingEffect.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.UngrantSkill(skillId);
				}
			}
		}

		// Token: 0x0400725E RID: 29278
		private MinionResume resume;
	}
}
