using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000A6C RID: 2668
[SkipSaveFileSerialization]
public class NightOwl : StateMachineComponent<NightOwl.StatesInstance>
{
	// Token: 0x06004D84 RID: 19844 RVA: 0x001C2EE0 File Offset: 0x001C10E0
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x06004D85 RID: 19845 RVA: 0x001C305C File Offset: 0x001C125C
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x06004D86 RID: 19846 RVA: 0x001C3098 File Offset: 0x001C1298
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x040033AE RID: 13230
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x040033AF RID: 13231
	private AttributeModifier[] attributeModifiers;

	// Token: 0x02001B91 RID: 7057
	public class StatesInstance : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.GameInstance
	{
		// Token: 0x0600AA5D RID: 43613 RVA: 0x003C3F92 File Offset: 0x003C2192
		public StatesInstance(NightOwl master) : base(master)
		{
		}

		// Token: 0x0600AA5E RID: 43614 RVA: 0x003C3F9B File Offset: 0x003C219B
		public bool IsNight()
		{
			return !(GameClock.Instance == null) && !(base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview) && GameClock.Instance.IsNighttime();
		}
	}

	// Token: 0x02001B92 RID: 7058
	public class States : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl>
	{
		// Token: 0x0600AA5F RID: 43615 RVA: 0x003C3FD4 File Offset: 0x003C21D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (NightOwl.StatesInstance smi) => smi.IsNight(), UpdateRate.SIM_200ms);
			this.early.Enter("Night", delegate(NightOwl.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotNight", delegate(NightOwl.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.NightTime, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (NightOwl.StatesInstance smi) => !smi.IsNight(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04008547 RID: 34119
		public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State idle;

		// Token: 0x04008548 RID: 34120
		public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State early;
	}
}
