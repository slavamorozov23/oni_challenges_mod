using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200089D RID: 2205
public class ElementGrowthMonitor : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>
{
	// Token: 0x06003CA2 RID: 15522 RVA: 0x001530F0 File Offset: 0x001512F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(ElementGrowthMonitor.Instance smi)
		{
			ElementGrowthMonitor.UpdateGrowth(smi, 0f);
		}).Update(new Action<ElementGrowthMonitor.Instance, float>(ElementGrowthMonitor.UpdateGrowth), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.EatSolidComplete, delegate(ElementGrowthMonitor.Instance smi, object data)
		{
			smi.OnEatSolidComplete(data);
		});
		this.growing.DefaultState(this.growing.growing).Transition(this.fullyGrown, new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsFullyGrown), UpdateRate.SIM_1000ms).TagTransition(this.HungryTags, this.halted, false);
		this.growing.growing.Transition(this.growing.stunted, GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Not(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsConsumedInTemperatureRange)), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthGrowing, null).Enter(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.RemoveModifier));
		this.growing.stunted.Transition(this.growing.growing, new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsConsumedInTemperatureRange), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthStunted, null).Enter(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.RemoveModifier));
		this.halted.TagTransition(this.HungryTags, this.growing, true).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthHalted, null);
		this.fullyGrown.ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthComplete, null).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (ElementGrowthMonitor.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).Transition(this.growing, GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Not(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06003CA3 RID: 15523 RVA: 0x00153303 File Offset: 0x00151503
	private static bool IsConsumedInTemperatureRange(ElementGrowthMonitor.Instance smi)
	{
		return smi.lastConsumedTemperature == 0f || (smi.lastConsumedTemperature >= smi.def.minTemperature && smi.lastConsumedTemperature <= smi.def.maxTemperature);
	}

	// Token: 0x06003CA4 RID: 15524 RVA: 0x0015333F File Offset: 0x0015153F
	private static bool IsFullyGrown(ElementGrowthMonitor.Instance smi)
	{
		return smi.elementGrowth.value >= smi.elementGrowth.GetMax();
	}

	// Token: 0x06003CA5 RID: 15525 RVA: 0x0015335C File Offset: 0x0015155C
	private static void ApplyModifier(ElementGrowthMonitor.Instance smi)
	{
		if (smi.IsInsideState(smi.sm.growing.growing))
		{
			smi.elementGrowth.deltaAttribute.Add(smi.growingGrowthModifier);
			return;
		}
		if (smi.IsInsideState(smi.sm.growing.stunted))
		{
			smi.elementGrowth.deltaAttribute.Add(smi.stuntedGrowthModifier);
		}
	}

	// Token: 0x06003CA6 RID: 15526 RVA: 0x001533C6 File Offset: 0x001515C6
	private static void RemoveModifier(ElementGrowthMonitor.Instance smi)
	{
		smi.elementGrowth.deltaAttribute.Remove(smi.growingGrowthModifier);
		smi.elementGrowth.deltaAttribute.Remove(smi.stuntedGrowthModifier);
	}

	// Token: 0x06003CA7 RID: 15527 RVA: 0x001533F4 File Offset: 0x001515F4
	private static void UpdateGrowth(ElementGrowthMonitor.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.elementGrowth.value / 100f);
		if (smi.currentGrowthLevel != num)
		{
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < ElementGrowthMonitor.GROWTH_SYMBOL_NAMES.Length; i++)
			{
				bool is_visible = i == num - 1;
				component.SetSymbolVisiblity(ElementGrowthMonitor.GROWTH_SYMBOL_NAMES[i], is_visible);
			}
			smi.currentGrowthLevel = num;
		}
	}

	// Token: 0x0400256F RID: 9583
	public Tag[] HungryTags = new Tag[]
	{
		GameTags.Creatures.Hungry
	};

	// Token: 0x04002570 RID: 9584
	public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State halted;

	// Token: 0x04002571 RID: 9585
	public ElementGrowthMonitor.GrowingState growing;

	// Token: 0x04002572 RID: 9586
	public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State fullyGrown;

	// Token: 0x04002573 RID: 9587
	private static HashedString[] GROWTH_SYMBOL_NAMES = new HashedString[]
	{
		"del_ginger1",
		"del_ginger2",
		"del_ginger3",
		"del_ginger4",
		"del_ginger5"
	};

	// Token: 0x02001883 RID: 6275
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009F2F RID: 40751 RVA: 0x003A53B5 File Offset: 0x003A35B5
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ElementGrowth.Id);
		}

		// Token: 0x06009F30 RID: 40752 RVA: 0x003A53DC File Offset: 0x003A35DC
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_TEMP.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{TempMin}", GameUtil.GetFormattedTemperature(this.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)).Replace("{TempMax}", GameUtil.GetFormattedTemperature(this.maxTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_TEMP.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{TempMin}", GameUtil.GetFormattedTemperature(this.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)).Replace("{TempMax}", GameUtil.GetFormattedTemperature(this.maxTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x04007B20 RID: 31520
		public int levelCount;

		// Token: 0x04007B21 RID: 31521
		public float defaultGrowthRate;

		// Token: 0x04007B22 RID: 31522
		public Tag itemDroppedOnShear;

		// Token: 0x04007B23 RID: 31523
		public float dropMass;

		// Token: 0x04007B24 RID: 31524
		public float minTemperature;

		// Token: 0x04007B25 RID: 31525
		public float maxTemperature;
	}

	// Token: 0x02001884 RID: 6276
	public class GrowingState : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State
	{
		// Token: 0x04007B26 RID: 31526
		public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State growing;

		// Token: 0x04007B27 RID: 31527
		public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State stunted;
	}

	// Token: 0x02001885 RID: 6277
	public new class Instance : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.GameInstance, IShearable
	{
		// Token: 0x06009F33 RID: 40755 RVA: 0x003A5520 File Offset: 0x003A3720
		public Instance(IStateMachineTarget master, ElementGrowthMonitor.Def def) : base(master, def)
		{
			this.elementGrowth = Db.Get().Amounts.ElementGrowth.Lookup(base.gameObject);
			this.elementGrowth.value = this.elementGrowth.GetMax();
			this.growingGrowthModifier = new AttributeModifier(this.elementGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME, false, false, true);
			this.stuntedGrowthModifier = new AttributeModifier(this.elementGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 20f, CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME, false, false, true);
		}

		// Token: 0x06009F34 RID: 40756 RVA: 0x003A55E4 File Offset: 0x003A37E4
		public void OnEatSolidComplete(object data)
		{
			KPrefabID kprefabID = (KPrefabID)data;
			if (kprefabID == null)
			{
				return;
			}
			PrimaryElement component = kprefabID.GetComponent<PrimaryElement>();
			this.lastConsumedElement = component.ElementID;
			this.lastConsumedTemperature = component.Temperature;
		}

		// Token: 0x06009F35 RID: 40757 RVA: 0x003A5621 File Offset: 0x003A3821
		public bool IsFullyGrown()
		{
			return this.currentGrowthLevel == base.def.levelCount;
		}

		// Token: 0x06009F36 RID: 40758 RVA: 0x003A5636 File Offset: 0x003A3836
		public void Shear()
		{
			this.elementGrowth.value = 0f;
			ElementGrowthMonitor.UpdateGrowth(this, 0f);
		}

		// Token: 0x06009F37 RID: 40759 RVA: 0x003A5653 File Offset: 0x003A3853
		public global::Tuple<Tag, float> GetItemDroppedOnShear()
		{
			return new global::Tuple<Tag, float>(base.def.itemDroppedOnShear, base.def.dropMass);
		}

		// Token: 0x04007B28 RID: 31528
		public AmountInstance elementGrowth;

		// Token: 0x04007B29 RID: 31529
		public AttributeModifier growingGrowthModifier;

		// Token: 0x04007B2A RID: 31530
		public AttributeModifier stuntedGrowthModifier;

		// Token: 0x04007B2B RID: 31531
		public int currentGrowthLevel = -1;

		// Token: 0x04007B2C RID: 31532
		[Serialize]
		public SimHashes lastConsumedElement;

		// Token: 0x04007B2D RID: 31533
		[Serialize]
		public float lastConsumedTemperature;
	}
}
