using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020009FF RID: 2559
[SkipSaveFileSerialization]
public class Meteorphile : StateMachineComponent<Meteorphile.StatesInstance>
{
	// Token: 0x06004AC1 RID: 19137 RVA: 0x001B1378 File Offset: 0x001AF578
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x06004AC2 RID: 19138 RVA: 0x001B14F4 File Offset: 0x001AF6F4
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x06004AC3 RID: 19139 RVA: 0x001B1530 File Offset: 0x001AF730
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x04003187 RID: 12679
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x04003188 RID: 12680
	private AttributeModifier[] attributeModifiers;

	// Token: 0x02001A6A RID: 6762
	public class StatesInstance : GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.GameInstance
	{
		// Token: 0x0600A567 RID: 42343 RVA: 0x003B6C0A File Offset: 0x003B4E0A
		public StatesInstance(Meteorphile master) : base(master)
		{
		}

		// Token: 0x0600A568 RID: 42344 RVA: 0x003B6C14 File Offset: 0x003B4E14
		public bool IsMeteors()
		{
			if (GameplayEventManager.Instance == null || base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview)
			{
				return false;
			}
			int myWorldId = this.GetMyWorldId();
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(myWorldId, ref list);
			for (int i = 0; i < list.Count; i++)
			{
				MeteorShowerEvent.StatesInstance statesInstance = list[i].smi as MeteorShowerEvent.StatesInstance;
				if (statesInstance != null && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x02001A6B RID: 6763
	public class States : GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile>
	{
		// Token: 0x0600A569 RID: 42345 RVA: 0x003B6CA8 File Offset: 0x003B4EA8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (Meteorphile.StatesInstance smi) => smi.IsMeteors(), UpdateRate.SIM_200ms);
			this.early.Enter("Meteors", delegate(Meteorphile.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotMeteors", delegate(Meteorphile.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.Meteorphile, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (Meteorphile.StatesInstance smi) => !smi.IsMeteors(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04008199 RID: 33177
		public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State idle;

		// Token: 0x0400819A RID: 33178
		public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State early;
	}
}
