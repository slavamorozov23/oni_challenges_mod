using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000905 RID: 2309
[SkipSaveFileSerialization]
public class EarlyBird : StateMachineComponent<EarlyBird.StatesInstance>
{
	// Token: 0x0600402F RID: 16431 RVA: 0x0016932C File Offset: 0x0016752C
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x001694A8 File Offset: 0x001676A8
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x001694E4 File Offset: 0x001676E4
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x040027C4 RID: 10180
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x040027C5 RID: 10181
	private AttributeModifier[] attributeModifiers;

	// Token: 0x02001901 RID: 6401
	public class StatesInstance : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.GameInstance
	{
		// Token: 0x0600A0FC RID: 41212 RVA: 0x003AAF24 File Offset: 0x003A9124
		public StatesInstance(EarlyBird master) : base(master)
		{
		}

		// Token: 0x0600A0FD RID: 41213 RVA: 0x003AAF30 File Offset: 0x003A9130
		public bool IsMorning()
		{
			return !(ScheduleManager.Instance == null) && !(base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview) && Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23) < TRAITS.EARLYBIRD_SCHEDULEBLOCK;
		}
	}

	// Token: 0x02001902 RID: 6402
	public class States : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird>
	{
		// Token: 0x0600A0FE RID: 41214 RVA: 0x003AAF88 File Offset: 0x003A9188
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (EarlyBird.StatesInstance smi) => smi.IsMorning(), UpdateRate.SIM_200ms);
			this.early.Enter("Morning", delegate(EarlyBird.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotMorning", delegate(EarlyBird.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.EarlyMorning, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (EarlyBird.StatesInstance smi) => !smi.IsMorning(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04007C9D RID: 31901
		public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State idle;

		// Token: 0x04007C9E RID: 31902
		public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State early;
	}
}
