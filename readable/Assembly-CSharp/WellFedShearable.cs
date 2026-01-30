using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008C2 RID: 2242
public class WellFedShearable : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>
{
	// Token: 0x06003DD7 RID: 15831 RVA: 0x00158A90 File Offset: 0x00156C90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).Enter(delegate(WellFedShearable.Instance smi)
		{
			if (smi.def.hideSymbols != null)
			{
				foreach (KAnimHashedString symbol in smi.def.hideSymbols)
				{
					smi.animController.SetSymbolVisiblity(symbol, false);
				}
			}
		}).Update(new Action<WellFedShearable.Instance, float>(WellFedShearable.UpdateScales), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.CaloriesConsumed, delegate(WellFedShearable.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		this.growing.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).DefaultState(this.growing.stalled).Transition(this.fullyGrown, new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
		this.growing.stalled.ToggleCritterEmotion(Db.Get().CritterEmotions.WellFed, null).Transition(this.growing.growing, (WellFedShearable.Instance smi) => smi.HasWellFedEffect(), UpdateRate.SIM_1000ms);
		this.growing.growing.Transition(this.growing.stalled, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not((WellFedShearable.Instance smi) => smi.HasWellFedEffect()), UpdateRate.SIM_1000ms).ToggleCritterEmotion(Db.Get().CritterEmotions.WellFed, null);
		this.fullyGrown.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (WellFedShearable.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).EventTransition(GameHashes.Molt, this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown))).Transition(this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06003DD8 RID: 15832 RVA: 0x00158CBD File Offset: 0x00156EBD
	private static bool AreScalesFullyGrown(WellFedShearable.Instance smi)
	{
		return smi.scaleGrowth.value >= smi.scaleGrowth.GetMax();
	}

	// Token: 0x06003DD9 RID: 15833 RVA: 0x00158CDC File Offset: 0x00156EDC
	private static void UpdateScales(WellFedShearable.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.scaleGrowth.value / 100f);
		if (smi.currentScaleLevel != num)
		{
			for (int i = 0; i < smi.def.scaleGrowthSymbols.Length; i++)
			{
				bool is_visible = i <= num - 1;
				smi.animController.SetSymbolVisiblity(smi.def.scaleGrowthSymbols[i], is_visible);
			}
			smi.currentScaleLevel = num;
		}
	}

	// Token: 0x0400262B RID: 9771
	public WellFedShearable.GrowingState growing;

	// Token: 0x0400262C RID: 9772
	public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State fullyGrown;

	// Token: 0x020018D8 RID: 6360
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600A06F RID: 41071 RVA: 0x003A9D2B File Offset: 0x003A7F2B
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
		}

		// Token: 0x0600A070 RID: 41072 RVA: 0x003A9D54 File Offset: 0x003A7F54
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f, "F1", false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_FED.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f, "F1", false)), Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x04007C23 RID: 31779
		public string effectId;

		// Token: 0x04007C24 RID: 31780
		public float caloriesPerCycle;

		// Token: 0x04007C25 RID: 31781
		public float growthDurationCycles;

		// Token: 0x04007C26 RID: 31782
		public int levelCount;

		// Token: 0x04007C27 RID: 31783
		public Tag itemDroppedOnShear;

		// Token: 0x04007C28 RID: 31784
		public float dropMass;

		// Token: 0x04007C29 RID: 31785
		public Tag requiredDiet = null;

		// Token: 0x04007C2A RID: 31786
		public KAnimHashedString[] scaleGrowthSymbols = WellFedShearable.Def.SCALE_SYMBOL_NAMES;

		// Token: 0x04007C2B RID: 31787
		public KAnimHashedString[] hideSymbols;

		// Token: 0x04007C2C RID: 31788
		public static KAnimHashedString[] SCALE_SYMBOL_NAMES = new KAnimHashedString[]
		{
			"scale_0",
			"scale_1",
			"scale_2",
			"scale_3",
			"scale_4"
		};
	}

	// Token: 0x020018D9 RID: 6361
	public class GrowingState : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State
	{
		// Token: 0x04007C2D RID: 31789
		public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State growing;

		// Token: 0x04007C2E RID: 31790
		public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State stalled;
	}

	// Token: 0x020018DA RID: 6362
	public new class Instance : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.GameInstance, IShearable
	{
		// Token: 0x0600A074 RID: 41076 RVA: 0x003A9EBC File Offset: 0x003A80BC
		public Instance(IStateMachineTarget master, WellFedShearable.Def def) : base(master, def)
		{
			this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(base.gameObject);
			this.scaleGrowth.value = this.scaleGrowth.GetMax();
		}

		// Token: 0x0600A075 RID: 41077 RVA: 0x003A9F0E File Offset: 0x003A810E
		public bool IsFullyGrown()
		{
			return this.currentScaleLevel == base.def.levelCount;
		}

		// Token: 0x0600A076 RID: 41078 RVA: 0x003A9F24 File Offset: 0x003A8124
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>.Unbox(data);
			if (base.def.requiredDiet != null && caloriesConsumedEvent.tag != base.def.requiredDiet)
			{
				return;
			}
			EffectInstance effectInstance = this.effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		// Token: 0x0600A077 RID: 41079 RVA: 0x003A9FCF File Offset: 0x003A81CF
		public bool HasWellFedEffect()
		{
			return this.effects.Get(base.def.effectId) != null;
		}

		// Token: 0x0600A078 RID: 41080 RVA: 0x003A9FEA File Offset: 0x003A81EA
		public void Shear()
		{
			this.scaleGrowth.value = 0f;
			WellFedShearable.UpdateScales(this, 0f);
		}

		// Token: 0x0600A079 RID: 41081 RVA: 0x003AA007 File Offset: 0x003A8207
		public global::Tuple<Tag, float> GetItemDroppedOnShear()
		{
			return new global::Tuple<Tag, float>(base.def.itemDroppedOnShear, base.def.dropMass);
		}

		// Token: 0x04007C2F RID: 31791
		[MyCmpGet]
		private Effects effects;

		// Token: 0x04007C30 RID: 31792
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x04007C31 RID: 31793
		public AmountInstance scaleGrowth;

		// Token: 0x04007C32 RID: 31794
		public int currentScaleLevel = -1;
	}
}
