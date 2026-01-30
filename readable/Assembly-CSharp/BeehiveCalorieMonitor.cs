using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005B2 RID: 1458
public class BeehiveCalorieMonitor : GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>
{
	// Token: 0x0600217A RID: 8570 RVA: 0x000C2698 File Offset: 0x000C0898
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(BeehiveCalorieMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).Update(new Action<BeehiveCalorieMonitor.Instance, float>(BeehiveCalorieMonitor.UpdateMetabolismCalorieModifier), UpdateRate.SIM_200ms, false);
		this.normal.Transition(this.hungry, (BeehiveCalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_1000ms);
		this.hungry.ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (BeehiveCalorieMonitor.Instance smi) => !smi.IsHungry()).ToggleStatusItem(Db.Get().CreatureStatusItems.HiveHungry, null).Transition(this.normal, (BeehiveCalorieMonitor.Instance smi) => !smi.IsHungry(), UpdateRate.SIM_1000ms);
	}

	// Token: 0x0600217B RID: 8571 RVA: 0x000C27A9 File Offset: 0x000C09A9
	private static bool ReadyToPoop(BeehiveCalorieMonitor.Instance smi)
	{
		return smi.stomach.IsReadyToPoop() && Time.time - smi.lastMealOrPoopTime >= smi.def.minimumTimeBeforePooping;
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000C27D6 File Offset: 0x000C09D6
	private static void UpdateMetabolismCalorieModifier(BeehiveCalorieMonitor.Instance smi, float dt)
	{
		smi.deltaCalorieMetabolismModifier.SetValue(1f - smi.metabolism.GetTotalValue() / 100f);
		if (BeehiveCalorieMonitor.ReadyToPoop(smi))
		{
			smi.Poop();
		}
	}

	// Token: 0x0400138A RID: 5002
	public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State normal;

	// Token: 0x0400138B RID: 5003
	public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State hungry;

	// Token: 0x02001443 RID: 5187
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008F21 RID: 36641 RVA: 0x0036A653 File Offset: 0x00368853
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
		}

		// Token: 0x06008F22 RID: 36642 RVA: 0x0036A67C File Offset: 0x0036887C
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER, Descriptor.DescriptorType.Effect, false));
			float calorie_loss_per_second = 0f;
			foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
			{
				if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
				{
					calorie_loss_per_second = attributeModifier.Value;
				}
			}
			BeehiveCalorieMonitor.Instance smi = obj.GetSMI<BeehiveCalorieMonitor.Instance>();
			string newValue = string.Join(", ", (from t in smi.stomach.diet.consumedTags
			select t.Key.ProperName()).ToArray<string>());
			string newValue2 = string.Join("\n", (from t in smi.stomach.diet.consumedTags
			select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue2), Descriptor.DescriptorType.Effect, false));
			string newValue3 = string.Join(", ", (from t in smi.stomach.diet.producedTags
			select t.Key.ProperName()).ToArray<string>());
			string newValue4 = string.Join("\n", (from t in smi.stomach.diet.producedTags
			select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f, GameUtil.TimeSlice.None))).ToArray<string>());
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue3), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", newValue4), Descriptor.DescriptorType.Effect, false));
			return list;
		}

		// Token: 0x04006DFF RID: 28159
		public Diet diet;

		// Token: 0x04006E00 RID: 28160
		public float minConsumedCaloriesBeforePooping = 100f;

		// Token: 0x04006E01 RID: 28161
		public float minimumTimeBeforePooping = 10f;

		// Token: 0x04006E02 RID: 28162
		public bool storePoop = true;
	}

	// Token: 0x02001444 RID: 5188
	public new class Instance : GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.GameInstance
	{
		// Token: 0x06008F24 RID: 36644 RVA: 0x0036A8F8 File Offset: 0x00368AF8
		public Instance(IStateMachineTarget master, BeehiveCalorieMonitor.Def def) : base(master, def)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
			this.calories.value = this.calories.GetMax() * 0.9f;
			this.stomach = new CreatureCalorieMonitor.Stomach(master.gameObject, def.minConsumedCaloriesBeforePooping, -1f, def.storePoop);
			this.metabolism = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
			this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, false, false);
			this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
		}

		// Token: 0x06008F25 RID: 36645 RVA: 0x0036A9DC File Offset: 0x00368BDC
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent value = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>)data).value;
			this.calories.value += value.calories;
			this.stomach.Consume(value.tag, value.calories);
			this.lastMealOrPoopTime = Time.time;
		}

		// Token: 0x06008F26 RID: 36646 RVA: 0x0036AA2F File Offset: 0x00368C2F
		public void Poop()
		{
			this.lastMealOrPoopTime = Time.time;
			this.stomach.Poop();
		}

		// Token: 0x06008F27 RID: 36647 RVA: 0x0036AA47 File Offset: 0x00368C47
		public float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x06008F28 RID: 36648 RVA: 0x0036AA60 File Offset: 0x00368C60
		public bool IsHungry()
		{
			return this.GetCalories0to1() < 0.9f;
		}

		// Token: 0x04006E03 RID: 28163
		public const float HUNGRY_RATIO = 0.9f;

		// Token: 0x04006E04 RID: 28164
		public AmountInstance calories;

		// Token: 0x04006E05 RID: 28165
		[Serialize]
		public CreatureCalorieMonitor.Stomach stomach;

		// Token: 0x04006E06 RID: 28166
		public float lastMealOrPoopTime;

		// Token: 0x04006E07 RID: 28167
		public AttributeInstance metabolism;

		// Token: 0x04006E08 RID: 28168
		public AttributeModifier deltaCalorieMetabolismModifier;
	}
}
