using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020008C3 RID: 2243
public class WildnessMonitor : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>
{
	// Token: 0x06003DDB RID: 15835 RVA: 0x00158D60 File Offset: 0x00156F60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.tame;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.wild.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.HideDomesticationSymbol)).Transition(this.tame, (WildnessMonitor.Instance smi) => !WildnessMonitor.IsWild(smi), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.wildEffect).ToggleTag(GameTags.Creatures.Wild);
		this.tame.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.ShowDomesticationSymbol)).Transition(this.wild, new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback(WildnessMonitor.IsWild), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.tameEffect).Enter(delegate(WildnessMonitor.Instance smi)
		{
			SaveGame.Instance.ColonyAchievementTracker.LogCritterTamed(smi.PrefabID());
		});
	}

	// Token: 0x06003DDC RID: 15836 RVA: 0x00158E88 File Offset: 0x00157088
	private static void HideDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, false);
		}
	}

	// Token: 0x06003DDD RID: 15837 RVA: 0x00158EC0 File Offset: 0x001570C0
	private static void ShowDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, true);
		}
	}

	// Token: 0x06003DDE RID: 15838 RVA: 0x00158EF6 File Offset: 0x001570F6
	private static bool IsWild(WildnessMonitor.Instance smi)
	{
		return smi.wildness.value > 0f;
	}

	// Token: 0x06003DDF RID: 15839 RVA: 0x00158F0C File Offset: 0x0015710C
	private static void RefreshAmounts(WildnessMonitor.Instance smi)
	{
		bool flag = WildnessMonitor.IsWild(smi);
		smi.wildness.hide = !flag;
		AttributeInstance attributeInstance = Db.Get().CritterAttributes.Happiness.Lookup(smi.gameObject);
		if (attributeInstance != null)
		{
			attributeInstance.hide = flag;
		}
		AttributeInstance attributeInstance2 = Db.Get().CritterAttributes.Metabolism.Lookup(smi.gameObject);
		if (attributeInstance2 != null)
		{
			attributeInstance2.hide = flag;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
		if (amountInstance != null)
		{
			amountInstance.hide = flag;
		}
		AmountInstance amountInstance2 = Db.Get().Amounts.Temperature.Lookup(smi.gameObject);
		if (amountInstance2 != null)
		{
			amountInstance2.hide = flag;
		}
		AmountInstance amountInstance3 = Db.Get().Amounts.Fertility.Lookup(smi.gameObject);
		if (amountInstance3 != null)
		{
			amountInstance3.hide = flag;
		}
		AmountInstance amountInstance4 = Db.Get().Amounts.MilkProduction.Lookup(smi.gameObject);
		if (amountInstance4 != null)
		{
			amountInstance4.hide = flag;
		}
		AmountInstance amountInstance5 = Db.Get().Amounts.Beckoning.Lookup(smi.gameObject);
		if (amountInstance5 != null)
		{
			amountInstance5.hide = flag;
		}
	}

	// Token: 0x0400262D RID: 9773
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State wild;

	// Token: 0x0400262E RID: 9774
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State tame;

	// Token: 0x0400262F RID: 9775
	private static readonly KAnimHashedString[] DOMESTICATION_SYMBOLS = new KAnimHashedString[]
	{
		"tag",
		"snapto_tag"
	};

	// Token: 0x020018DC RID: 6364
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600A084 RID: 41092 RVA: 0x003AA0D1 File Offset: 0x003A82D1
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
		}

		// Token: 0x04007C3C RID: 31804
		public Effect wildEffect;

		// Token: 0x04007C3D RID: 31805
		public Effect tameEffect;
	}

	// Token: 0x020018DD RID: 6365
	public new class Instance : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.GameInstance
	{
		// Token: 0x0600A086 RID: 41094 RVA: 0x003AA0FF File Offset: 0x003A82FF
		public Instance(IStateMachineTarget master, WildnessMonitor.Def def) : base(master, def)
		{
			this.wildness = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			this.wildness.value = this.wildness.GetMax();
		}

		// Token: 0x0600A087 RID: 41095 RVA: 0x003AA13F File Offset: 0x003A833F
		public bool IsWild()
		{
			return WildnessMonitor.IsWild(this);
		}

		// Token: 0x0600A088 RID: 41096 RVA: 0x003AA148 File Offset: 0x003A8348
		[ContextMenu("Tame Critter")]
		public void DebugTame()
		{
			AmountInstance amountInstance = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			if (amountInstance != null)
			{
				amountInstance.value = 0f;
				base.smi.GoTo(base.sm.tame);
			}
		}

		// Token: 0x04007C3E RID: 31806
		public AmountInstance wildness;
	}
}
