using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008BA RID: 2234
public class ScaleGrowthMonitor : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>
{
	// Token: 0x06003D8D RID: 15757 RVA: 0x00157708 File Offset: 0x00155908
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(ScaleGrowthMonitor.Instance smi)
		{
			ScaleGrowthMonitor.UpdateScales(smi, 0f);
		}).Update(new Action<ScaleGrowthMonitor.Instance, float>(ScaleGrowthMonitor.UpdateScales), UpdateRate.SIM_1000ms, false);
		this.growing.DefaultState(this.growing.growing).Transition(this.fullyGrown, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
		this.growing.growing.Transition(this.growing.stunted, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere)), UpdateRate.SIM_1000ms).Enter(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.RemoveModifier));
		GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State state = this.growing.stunted.Transition(this.growing.growing, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere), UpdateRate.SIM_1000ms);
		string name = CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.NAME;
		string tooltip = CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.fullyGrown.ToggleBehaviour(GameTags.Creatures.ScalesGrown, (ScaleGrowthMonitor.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).Transition(this.growing, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06003D8E RID: 15758 RVA: 0x00157894 File Offset: 0x00155A94
	private static bool IsInCorrectAtmosphere(ScaleGrowthMonitor.Instance smi)
	{
		if (smi.def.targetAtmosphere == (SimHashes)0)
		{
			return true;
		}
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].id == smi.def.targetAtmosphere;
	}

	// Token: 0x06003D8F RID: 15759 RVA: 0x001578DA File Offset: 0x00155ADA
	private static bool AreScalesFullyGrown(ScaleGrowthMonitor.Instance smi)
	{
		return smi.scaleGrowth.value >= smi.scaleGrowth.GetMax();
	}

	// Token: 0x06003D90 RID: 15760 RVA: 0x001578F7 File Offset: 0x00155AF7
	private static void ApplyModifier(ScaleGrowthMonitor.Instance smi)
	{
		smi.scaleGrowth.deltaAttribute.Add(smi.scaleGrowthModifier);
	}

	// Token: 0x06003D91 RID: 15761 RVA: 0x0015790F File Offset: 0x00155B0F
	private static void RemoveModifier(ScaleGrowthMonitor.Instance smi)
	{
		smi.scaleGrowth.deltaAttribute.Remove(smi.scaleGrowthModifier);
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x00157928 File Offset: 0x00155B28
	private static void UpdateScales(ScaleGrowthMonitor.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.scaleGrowth.value / 100f);
		if (smi.currentScaleLevel != num)
		{
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < ScaleGrowthMonitor.SCALE_SYMBOL_NAMES.Length; i++)
			{
				bool is_visible = i <= num - 1;
				component.SetSymbolVisiblity(ScaleGrowthMonitor.SCALE_SYMBOL_NAMES[i], is_visible);
			}
			smi.currentScaleLevel = num;
		}
	}

	// Token: 0x040025FF RID: 9727
	public ScaleGrowthMonitor.GrowingState growing;

	// Token: 0x04002600 RID: 9728
	public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State fullyGrown;

	// Token: 0x04002601 RID: 9729
	private AttributeModifier scaleGrowthModifier;

	// Token: 0x04002602 RID: 9730
	private static HashedString[] SCALE_SYMBOL_NAMES = new HashedString[]
	{
		"scale_0",
		"scale_1",
		"scale_2",
		"scale_3",
		"scale_4"
	};

	// Token: 0x020018C8 RID: 6344
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600A047 RID: 41031 RVA: 0x003A95D0 File Offset: 0x003A77D0
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
		}

		// Token: 0x0600A048 RID: 41032 RVA: 0x003A95F8 File Offset: 0x003A77F8
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			if (this.targetAtmosphere == (SimHashes)0)
			{
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x04007BF4 RID: 31732
		public int levelCount;

		// Token: 0x04007BF5 RID: 31733
		public float defaultGrowthRate;

		// Token: 0x04007BF6 RID: 31734
		public SimHashes targetAtmosphere;

		// Token: 0x04007BF7 RID: 31735
		public Tag itemDroppedOnShear;

		// Token: 0x04007BF8 RID: 31736
		public float dropMass;
	}

	// Token: 0x020018C9 RID: 6345
	public class GrowingState : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State
	{
		// Token: 0x04007BF9 RID: 31737
		public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State growing;

		// Token: 0x04007BFA RID: 31738
		public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State stunted;
	}

	// Token: 0x020018CA RID: 6346
	public new class Instance : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.GameInstance, IShearable
	{
		// Token: 0x0600A04B RID: 41035 RVA: 0x003A97DC File Offset: 0x003A79DC
		public Instance(IStateMachineTarget master, ScaleGrowthMonitor.Def def) : base(master, def)
		{
			this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(base.gameObject);
			this.scaleGrowth.value = this.scaleGrowth.GetMax();
			this.scaleGrowthModifier = new AttributeModifier(this.scaleGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, CREATURES.MODIFIERS.SCALE_GROWTH_RATE.NAME, false, false, true);
		}

		// Token: 0x0600A04C RID: 41036 RVA: 0x003A9867 File Offset: 0x003A7A67
		public bool IsFullyGrown()
		{
			return this.currentScaleLevel == base.def.levelCount;
		}

		// Token: 0x0600A04D RID: 41037 RVA: 0x003A987C File Offset: 0x003A7A7C
		public void Shear()
		{
			this.scaleGrowth.value = 0f;
			ScaleGrowthMonitor.UpdateScales(this, 0f);
		}

		// Token: 0x0600A04E RID: 41038 RVA: 0x003A9899 File Offset: 0x003A7A99
		public global::Tuple<Tag, float> GetItemDroppedOnShear()
		{
			return new global::Tuple<Tag, float>(base.def.itemDroppedOnShear, base.def.dropMass);
		}

		// Token: 0x04007BFB RID: 31739
		public AmountInstance scaleGrowth;

		// Token: 0x04007BFC RID: 31740
		public AttributeModifier scaleGrowthModifier;

		// Token: 0x04007BFD RID: 31741
		public int currentScaleLevel = -1;
	}
}
