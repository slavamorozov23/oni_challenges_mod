using System;
using Klei.AI;
using Klei.CustomSettings;

// Token: 0x02000A4D RID: 2637
public class StressMonitor : GameStateMachine<StressMonitor, StressMonitor.Instance>
{
	// Token: 0x06004CD7 RID: 19671 RVA: 0x001BEF5C File Offset: 0x001BD15C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		default_state = this.satisfied;
		this.root.Update("StressMonitor", delegate(StressMonitor.Instance smi, float dt)
		{
			smi.ReportStress(dt);
		}, UpdateRate.SIM_200ms, false);
		this.satisfied.TriggerOnEnter(GameHashes.NotStressed, null).Transition(this.stressed.tier1, (StressMonitor.Instance smi) => smi.stress.value >= 60f, UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Neutral, null);
		this.stressed.ToggleStatusItem(Db.Get().DuplicantStatusItems.Stressed, null).Transition(this.satisfied, (StressMonitor.Instance smi) => smi.stress.value < 60f, UpdateRate.SIM_200ms).ToggleReactable((StressMonitor.Instance smi) => smi.CreateConcernReactable()).TriggerOnEnter(GameHashes.Stressed, null);
		this.stressed.tier1.Transition(this.stressed.tier2, (StressMonitor.Instance smi) => smi.HasHadEnough(), UpdateRate.SIM_200ms);
		this.stressed.tier2.TriggerOnEnter(GameHashes.StressedHadEnough, null).Transition(this.stressed.tier1, (StressMonitor.Instance smi) => !smi.HasHadEnough(), UpdateRate.SIM_200ms);
	}

	// Token: 0x0400332D RID: 13101
	public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x0400332E RID: 13102
	public StressMonitor.Stressed stressed;

	// Token: 0x0400332F RID: 13103
	private const float StressThreshold_One = 60f;

	// Token: 0x04003330 RID: 13104
	private const float StressThreshold_Two = 100f;

	// Token: 0x02001B49 RID: 6985
	public class Stressed : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400845C RID: 33884
		public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier1;

		// Token: 0x0400845D RID: 33885
		public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier2;
	}

	// Token: 0x02001B4A RID: 6986
	public new class Instance : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A938 RID: 43320 RVA: 0x003C0D0C File Offset: 0x003BEF0C
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.stress = Db.Get().Amounts.Stress.Lookup(base.gameObject);
			SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.StressBreaks.id];
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.StressBreaks);
			this.allowStressBreak = settingConfig.IsDefaultLevel(currentQualitySetting.id);
		}

		// Token: 0x0600A939 RID: 43321 RVA: 0x003C0D83 File Offset: 0x003BEF83
		public bool IsStressed()
		{
			return base.IsInsideState(base.sm.stressed);
		}

		// Token: 0x0600A93A RID: 43322 RVA: 0x003C0D96 File Offset: 0x003BEF96
		public bool HasHadEnough()
		{
			return this.allowStressBreak && this.stress.value >= 100f;
		}

		// Token: 0x0600A93B RID: 43323 RVA: 0x003C0DB8 File Offset: 0x003BEFB8
		public void ReportStress(float dt)
		{
			for (int num = 0; num != this.stress.deltaAttribute.Modifiers.Count; num++)
			{
				AttributeModifier attributeModifier = this.stress.deltaAttribute.Modifiers[num];
				DebugUtil.DevAssert(!attributeModifier.IsMultiplier, "Reporting stress for multipliers not supported yet.", null);
				ReportManager.Instance.ReportValue(ReportManager.ReportType.StressDelta, attributeModifier.Value * dt, attributeModifier.GetDescription(), base.gameObject.GetProperName());
			}
		}

		// Token: 0x0600A93C RID: 43324 RVA: 0x003C0E34 File Offset: 0x003BF034
		public Reactable CreateConcernReactable()
		{
			return new EmoteReactable(base.master.gameObject, "StressConcern", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 30f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Concern);
		}

		// Token: 0x0400845E RID: 33886
		public AmountInstance stress;

		// Token: 0x0400845F RID: 33887
		private bool allowStressBreak = true;
	}
}
