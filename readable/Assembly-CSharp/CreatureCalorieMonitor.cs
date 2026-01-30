using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005B8 RID: 1464
public class CreatureCalorieMonitor : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>
{
	// Token: 0x0600219E RID: 8606 RVA: 0x000C32A0 File Offset: 0x000C14A0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(CreatureCalorieMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.Poop, new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback(CreatureCalorieMonitor.ReadyToPoop), delegate(CreatureCalorieMonitor.Instance smi)
		{
			smi.Poop();
		}).Update(new Action<CreatureCalorieMonitor.Instance, float>(CreatureCalorieMonitor.UpdateMetabolismCalorieModifier), UpdateRate.SIM_200ms, false);
		this.normal.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).Transition(this.hungry, (CreatureCalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_1000ms);
		this.hungry.DefaultState(this.hungry.hungry).ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (CreatureCalorieMonitor.Instance smi) => !smi.IsHungry());
		this.hungry.hungry.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).Transition(this.normal, (CreatureCalorieMonitor.Instance smi) => !smi.IsHungry(), UpdateRate.SIM_1000ms).Transition(this.hungry.outofcalories, (CreatureCalorieMonitor.Instance smi) => smi.IsOutOfCalories(), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, null);
		this.hungry.outofcalories.DefaultState(this.hungry.outofcalories.wild).Transition(this.hungry.hungry, (CreatureCalorieMonitor.Instance smi) => !smi.IsOutOfCalories(), UpdateRate.SIM_1000ms);
		this.hungry.outofcalories.wild.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.tame, true).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, null).ToggleCritterEmotion(Db.Get().CritterEmotions.Hungry, null);
		this.hungry.outofcalories.tame.Enter("StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.StarvationStartTime)).Exit("ClearStarvationTime", delegate(CreatureCalorieMonitor.Instance smi)
		{
			this.starvationStartTime.Set(Mathf.Min(-(GameClock.Instance.GetTime() - this.starvationStartTime.Get(smi)), 0f), smi, false);
		}).Transition(this.hungry.outofcalories.starvedtodeath, (CreatureCalorieMonitor.Instance smi) => smi.GetDeathTimeRemaining() <= 0f, UpdateRate.SIM_1000ms).TagTransition(GameTags.Creatures.PausedHunger, this.pause.starvingPause, false).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.wild, false).ToggleStatusItem(CREATURES.STATUSITEMS.STARVING.NAME, CREATURES.STATUSITEMS.STARVING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, default(HashedString), 129022, (string str, CreatureCalorieMonitor.Instance smi) => str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(smi.GetDeathTimeRemaining(), "F1", false)), null, null).ToggleNotification((CreatureCalorieMonitor.Instance smi) => new Notification(CREATURES.STATUSITEMS.STARVING.NOTIFICATION_NAME, NotificationType.BadMinor, (List<Notification> notifications, object data) => CREATURES.STATUSITEMS.STARVING.NOTIFICATION_TOOLTIP + notifications.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false)).ToggleEffect((CreatureCalorieMonitor.Instance smi) => this.outOfCaloriesTame).ToggleCritterEmotion(Db.Get().CritterEmotions.Hungry, null);
		this.hungry.outofcalories.starvedtodeath.Enter(delegate(CreatureCalorieMonitor.Instance smi)
		{
			smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
		});
		this.pause.commonPause.TagTransition(GameTags.Creatures.PausedHunger, this.normal, true);
		this.pause.starvingPause.Exit("Recalculate StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.RecalculateStartTimeOnUnpause)).TagTransition(GameTags.Creatures.PausedHunger, this.hungry.outofcalories.tame, true);
		this.outOfCaloriesTame = new Effect("OutOfCaloriesTame", CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, CREATURES.MODIFIERS.OUT_OF_CALORIES.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
		this.outOfCaloriesTame.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, false, false, true));
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x000C375C File Offset: 0x000C195C
	private static bool ReadyToPoop(CreatureCalorieMonitor.Instance smi)
	{
		return smi.stomach.IsReadyToPoop() && Time.time - smi.lastMealOrPoopTime >= smi.def.minimumTimeBeforePooping && !smi.IsInsideState(smi.sm.pause);
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000C37A9 File Offset: 0x000C19A9
	private static void UpdateMetabolismCalorieModifier(CreatureCalorieMonitor.Instance smi, float dt)
	{
		if (smi.IsInsideState(smi.sm.pause))
		{
			return;
		}
		smi.deltaCalorieMetabolismModifier.SetValue(1f - smi.metabolism.GetTotalValue() / 100f);
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x000C37E1 File Offset: 0x000C19E1
	private static void StarvationStartTime(CreatureCalorieMonitor.Instance smi)
	{
		if (smi.sm.starvationStartTime.Get(smi) <= 0f)
		{
			smi.sm.starvationStartTime.Set(GameClock.Instance.GetTime(), smi, false);
		}
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x000C3818 File Offset: 0x000C1A18
	private static void RecalculateStartTimeOnUnpause(CreatureCalorieMonitor.Instance smi)
	{
		float num = smi.sm.starvationStartTime.Get(smi);
		if (num < 0f)
		{
			float value = GameClock.Instance.GetTime() - Mathf.Abs(num);
			smi.sm.starvationStartTime.Set(value, smi, false);
		}
	}

	// Token: 0x04001398 RID: 5016
	public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State normal;

	// Token: 0x04001399 RID: 5017
	public CreatureCalorieMonitor.PauseStates pause;

	// Token: 0x0400139A RID: 5018
	private CreatureCalorieMonitor.HungryStates hungry;

	// Token: 0x0400139B RID: 5019
	private Effect outOfCaloriesTame;

	// Token: 0x0400139C RID: 5020
	public StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.FloatParameter starvationStartTime;

	// Token: 0x02001456 RID: 5206
	public struct CaloriesConsumedEvent
	{
		// Token: 0x04006E40 RID: 28224
		public Tag tag;

		// Token: 0x04006E41 RID: 28225
		public float calories;
	}

	// Token: 0x02001457 RID: 5207
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008F82 RID: 36738 RVA: 0x0036B571 File Offset: 0x00369771
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
		}

		// Token: 0x06008F83 RID: 36739 RVA: 0x0036B598 File Offset: 0x00369798
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER, Descriptor.DescriptorType.Effect, false));
			CreatureCalorieMonitor.Stomach stomach = obj.GetSMI<CreatureCalorieMonitor.Instance>().stomach;
			float calorie_loss_per_second = 0f;
			foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
			{
				if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
				{
					calorie_loss_per_second = attributeModifier.Value;
				}
			}
			if (stomach.diet.consumedTags.Count > 0)
			{
				string newValue = string.Join(", ", (from t in stomach.diet.consumedTags
				select t.Key.ProperName()).ToArray<string>());
				string text = "";
				if (stomach.diet.CanEatAnyPlantDirectly)
				{
					text = string.Join("\n", (from s in stomach.diet.consumedTags.Select(delegate(KeyValuePair<Tag, float> t)
					{
						float consumer_caloriesLossPerCaloriesPerKG = -calorie_loss_per_second / t.Value;
						GameObject prefab = Assets.GetPrefab(t.Key.ToString());
						IPlantConsumptionInstructions plantConsumptionInstructions = prefab.GetComponent<IPlantConsumptionInstructions>();
						plantConsumptionInstructions = ((plantConsumptionInstructions != null) ? plantConsumptionInstructions : prefab.GetSMI<IPlantConsumptionInstructions>());
						if (plantConsumptionInstructions == null)
						{
							return null;
						}
						return UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", plantConsumptionInstructions.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG));
					})
					where !string.IsNullOrEmpty(s)
					select s).ToArray<string>());
				}
				Diet.Info info;
				if (this.diet.CanEatPreyCritter)
				{
					if (this.diet.CanEatAnyPlantDirectly)
					{
						text += "\n";
					}
					text += string.Join("\n", (from t in stomach.diet.consumedTags.FindAll((KeyValuePair<Tag, float> t) => this.diet.preyInfos.FirstOrDefault((Diet.Info info) => info.consumedTags.Contains(t.Key)) != null)
					select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedPreyConsumptionValuePerCycle(t.Key, -calorie_loss_per_second / t.Value, true))).ToArray<string>());
				}
				if (this.diet.CanEatAnySolid)
				{
					if (this.diet.CanEatAnyPlantDirectly || this.diet.CanEatPreyCritter)
					{
						text += "\n";
					}
					text += string.Join("\n", (from t in stomach.diet.consumedTags.FindAll((KeyValuePair<Tag, float> t) => this.diet.solidEdiblesInfo.FirstOrDefault((Diet.Info info) => info.consumedTags.Contains(t.Key)) != null)
					select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", text), Descriptor.DescriptorType.Effect, false));
			}
			if (stomach.diet.producedTags.Count > 0)
			{
				string newValue2 = string.Join(", ", (from t in stomach.diet.producedTags
				select t.Key.ProperName()).ToArray<string>());
				string text2 = "";
				if (stomach.diet.CanEatAnyPlantDirectly)
				{
					List<KeyValuePair<Tag, float>> list2 = new List<KeyValuePair<Tag, float>>();
					foreach (KeyValuePair<Tag, float> keyValuePair in stomach.diet.producedTags)
					{
						foreach (Diet.Info info in this.diet.directlyEatenPlantInfos)
						{
							if (info.producedElement == keyValuePair.Key)
							{
								float consumed_mass = -calorie_loss_per_second / info.caloriesPerKg * 600f;
								float num = info.ConvertConsumptionMassToProducedMass(consumed_mass);
								list2.Add(new KeyValuePair<Tag, float>(keyValuePair.Key, num / 600f));
							}
						}
					}
					text2 = string.Join("\n", (from t in list2
					select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM_FROM_PLANT.text.Replace("{Item}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
					text2 += "\n";
				}
				else if (stomach.diet.CanEatAnySolid)
				{
					List<KeyValuePair<Tag, float>> list3 = new List<KeyValuePair<Tag, float>>();
					foreach (KeyValuePair<Tag, float> keyValuePair2 in stomach.diet.producedTags)
					{
						foreach (Diet.Info info2 in this.diet.solidEdiblesInfo)
						{
							if (info2.producedElement == keyValuePair2.Key)
							{
								list3.Add(new KeyValuePair<Tag, float>(info2.producedElement, info2.producedConversionRate));
							}
						}
					}
					text2 += string.Join("\n", (from t in list3
					select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f, GameUtil.TimeSlice.None))).ToArray<string>());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue2), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", text2), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x04006E42 RID: 28226
		public Diet diet;

		// Token: 0x04006E43 RID: 28227
		public float hungryRatio = 0.9f;

		// Token: 0x04006E44 RID: 28228
		public float minConsumedCaloriesBeforePooping = 100f;

		// Token: 0x04006E45 RID: 28229
		public float maxPoopSizeKG = -1f;

		// Token: 0x04006E46 RID: 28230
		public float minimumTimeBeforePooping = 10f;

		// Token: 0x04006E47 RID: 28231
		public float deathTimer = 6000f;

		// Token: 0x04006E48 RID: 28232
		public bool storePoop;
	}

	// Token: 0x02001458 RID: 5208
	public class PauseStates : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
	{
		// Token: 0x04006E49 RID: 28233
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State commonPause;

		// Token: 0x04006E4A RID: 28234
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvingPause;
	}

	// Token: 0x02001459 RID: 5209
	public class HungryStates : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
	{
		// Token: 0x04006E4B RID: 28235
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State hungry;

		// Token: 0x04006E4C RID: 28236
		public CreatureCalorieMonitor.HungryStates.OutOfCaloriesState outofcalories;

		// Token: 0x0200289B RID: 10395
		public class OutOfCaloriesState : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
		{
			// Token: 0x0400B2F2 RID: 45810
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State wild;

			// Token: 0x0400B2F3 RID: 45811
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State tame;

			// Token: 0x0400B2F4 RID: 45812
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvedtodeath;
		}
	}

	// Token: 0x0200145A RID: 5210
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Stomach
	{
		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06008F87 RID: 36743 RVA: 0x0036BB53 File Offset: 0x00369D53
		// (set) Token: 0x06008F88 RID: 36744 RVA: 0x0036BB5B File Offset: 0x00369D5B
		public Diet diet { get; private set; }

		// Token: 0x06008F89 RID: 36745 RVA: 0x0036BB64 File Offset: 0x00369D64
		public Stomach(GameObject owner, float minConsumedCaloriesBeforePooping, float max_poop_size_in_kg, bool storePoop)
		{
			this.diet = DietManager.Instance.GetPrefabDiet(owner);
			this.owner = owner;
			this.minConsumedCaloriesBeforePooping = minConsumedCaloriesBeforePooping;
			this.storePoop = storePoop;
			this.maxPoopSizeInKG = max_poop_size_in_kg;
		}

		// Token: 0x06008F8A RID: 36746 RVA: 0x0036BBB0 File Offset: 0x00369DB0
		public void Poop()
		{
			this.shouldContinuingPooping = true;
			float num = 0f;
			Tag tag = Tag.Invalid;
			byte disease_idx = byte.MaxValue;
			int num2 = 0;
			int num3 = 0;
			bool flag = false;
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && (!(tag != Tag.Invalid) || !(tag != dietInfo.producedElement)))
					{
						float num4 = (this.maxPoopSizeInKG < 0f) ? float.MaxValue : this.maxPoopSizeInKG;
						float b = Mathf.Clamp(num4 - num, 0f, num4);
						float num5 = Mathf.Min(dietInfo.ConvertConsumptionMassToProducedMass(dietInfo.ConvertCaloriesToConsumptionMass(caloriesConsumedEntry.calories)), b);
						num += num5;
						tag = dietInfo.producedElement;
						if (dietInfo.diseaseIdx != 255)
						{
							disease_idx = dietInfo.diseaseIdx;
							if (!this.storePoop && dietInfo.emmitDiseaseOnCell)
							{
								num3 += (int)(dietInfo.diseasePerKgProduced * num5);
							}
							else
							{
								num2 += (int)(dietInfo.diseasePerKgProduced * num5);
							}
						}
						caloriesConsumedEntry.calories = Mathf.Clamp(caloriesConsumedEntry.calories - dietInfo.ConvertConsumptionMassToCalories(dietInfo.ConvertProducedMassToConsumptionMass(num5)), 0f, float.MaxValue);
						this.caloriesConsumed[i] = caloriesConsumedEntry;
						flag = (flag || dietInfo.produceSolidTile);
					}
				}
			}
			if (num <= 0f || tag == Tag.Invalid)
			{
				this.shouldContinuingPooping = false;
				return;
			}
			string text = null;
			Element element = ElementLoader.GetElement(tag);
			Sprite mainIcon = null;
			Color iconTint = Color.white;
			if (element != null)
			{
				text = element.name;
				mainIcon = global::Def.GetUISprite(element, "ui", false).first;
				iconTint = (element.IsSolid ? Color.white : element.substance.colour);
			}
			int num6 = Grid.PosToCell(this.owner.transform.GetPosition());
			float temperature = this.owner.GetComponent<PrimaryElement>().Temperature;
			DebugUtil.DevAssert(!this.storePoop || !flag, "Stomach cannot both store poop & create a solid tile.", null);
			if (this.storePoop)
			{
				Storage component = this.owner.GetComponent<Storage>();
				if (element == null)
				{
					GameObject prefab = Assets.GetPrefab(tag);
					GameObject gameObject = GameUtil.KInstantiate(prefab, Grid.CellToPos(num6, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, null, 0);
					PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
					component2.Mass = num;
					component2.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
					component2.Temperature = temperature;
					gameObject.SetActive(true);
					component.Store(gameObject, true, false, true, false);
					text = gameObject.GetProperName();
					mainIcon = global::Def.GetUISprite(prefab, "ui", false).first;
				}
				else if (element.IsLiquid)
				{
					component.AddLiquid(element.id, num, temperature, disease_idx, num2, false, true);
				}
				else if (element.IsGas)
				{
					component.AddGasChunk(element.id, num, temperature, disease_idx, num2, false, true);
				}
				else
				{
					component.AddOre(element.id, num, temperature, disease_idx, num2, false, true);
				}
			}
			else
			{
				if (element == null)
				{
					GameObject prefab2 = Assets.GetPrefab(tag);
					GameObject gameObject2 = GameUtil.KInstantiate(prefab2, Grid.CellToPos(num6, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, null, 0);
					PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
					component3.Mass = num;
					component3.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
					component3.Temperature = temperature;
					gameObject2.SetActive(true);
					text = gameObject2.GetProperName();
					mainIcon = global::Def.GetUISprite(prefab2, "ui", false).first;
				}
				else if (element.IsLiquid)
				{
					FallingWater.instance.AddParticle(num6, element.idx, num, temperature, disease_idx, num2, true, false, false, false);
				}
				else if (element.IsGas)
				{
					SimMessages.AddRemoveSubstance(num6, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
				}
				else if (flag)
				{
					int num7 = this.owner.GetComponent<Facing>().GetFrontCell();
					if (!Grid.IsValidCell(num7))
					{
						global::Debug.LogWarningFormat("{0} attemping to Poop {1} on invalid cell {2} from cell {3}", new object[]
						{
							this.owner,
							element.name,
							num7,
							num6
						});
						num7 = num6;
					}
					SimMessages.AddRemoveSubstance(num7, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
				}
				else
				{
					element.substance.SpawnResource(Grid.CellToPosCCC(num6, Grid.SceneLayer.Ore), num, temperature, disease_idx, num2, false, false, false);
				}
				if (num3 > 0)
				{
					SimMessages.ModifyDiseaseOnCell(num6, disease_idx, num3);
				}
			}
			if (this.GetTotalConsumedCalories() <= 0f)
			{
				this.shouldContinuingPooping = false;
			}
			KPrefabID component4 = this.owner.GetComponent<KPrefabID>();
			if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component4.PrefabTag))
			{
				Game.Instance.savedInfo.creaturePoopAmount.Add(component4.PrefabTag, 0f);
			}
			Dictionary<Tag, float> creaturePoopAmount = Game.Instance.savedInfo.creaturePoopAmount;
			Tag prefabTag = component4.PrefabTag;
			creaturePoopAmount[prefabTag] += num;
			PopFX popFX = PopFXManager.Instance.SpawnFX(mainIcon, PopFXManager.Instance.sprite_Plus, text, this.owner.transform, Vector3.zero, 1.5f, true, false, false);
			if (popFX != null)
			{
				popFX.SetIconTint(iconTint);
			}
			this.owner.Trigger(-1844238272, null);
		}

		// Token: 0x06008F8B RID: 36747 RVA: 0x0036C133 File Offset: 0x0036A333
		public List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> GetCalorieEntries()
		{
			return this.caloriesConsumed;
		}

		// Token: 0x06008F8C RID: 36748 RVA: 0x0036C13C File Offset: 0x0036A33C
		public float GetTotalConsumedCalories()
		{
			float num = 0f;
			foreach (CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry in this.caloriesConsumed)
			{
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						num += caloriesConsumedEntry.calories;
					}
				}
			}
			return num;
		}

		// Token: 0x06008F8D RID: 36749 RVA: 0x0036C1CC File Offset: 0x0036A3CC
		public float GetFullness()
		{
			return this.GetTotalConsumedCalories() / this.minConsumedCaloriesBeforePooping;
		}

		// Token: 0x06008F8E RID: 36750 RVA: 0x0036C1DC File Offset: 0x0036A3DC
		public bool IsReadyToPoop()
		{
			float totalConsumedCalories = this.GetTotalConsumedCalories();
			return totalConsumedCalories > 0f && (this.shouldContinuingPooping || totalConsumedCalories >= this.minConsumedCaloriesBeforePooping);
		}

		// Token: 0x06008F8F RID: 36751 RVA: 0x0036C210 File Offset: 0x0036A410
		public void Consume(Tag tag, float calories)
		{
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.tag == tag)
				{
					caloriesConsumedEntry.calories += calories;
					this.caloriesConsumed[i] = caloriesConsumedEntry;
					return;
				}
			}
			CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry item = default(CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry);
			item.tag = tag;
			item.calories = calories;
			this.caloriesConsumed.Add(item);
		}

		// Token: 0x06008F90 RID: 36752 RVA: 0x0036C28C File Offset: 0x0036A48C
		public Tag GetNextPoopEntry()
		{
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						return dietInfo.producedElement;
					}
				}
			}
			return Tag.Invalid;
		}

		// Token: 0x04006E4D RID: 28237
		[Serialize]
		private List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> caloriesConsumed = new List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry>();

		// Token: 0x04006E4E RID: 28238
		[Serialize]
		private bool shouldContinuingPooping;

		// Token: 0x04006E4F RID: 28239
		private float minConsumedCaloriesBeforePooping;

		// Token: 0x04006E50 RID: 28240
		private float maxPoopSizeInKG;

		// Token: 0x04006E52 RID: 28242
		private GameObject owner;

		// Token: 0x04006E53 RID: 28243
		private bool storePoop;

		// Token: 0x0200289C RID: 10396
		[Serializable]
		public struct CaloriesConsumedEntry
		{
			// Token: 0x0400B2F5 RID: 45813
			public Tag tag;

			// Token: 0x0400B2F6 RID: 45814
			public float calories;
		}
	}

	// Token: 0x0200145B RID: 5211
	public new class Instance : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GameInstance
	{
		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06008F91 RID: 36753 RVA: 0x0036C2FC File Offset: 0x0036A4FC
		public float HungryRatio
		{
			get
			{
				return base.def.hungryRatio;
			}
		}

		// Token: 0x06008F92 RID: 36754 RVA: 0x0036C30C File Offset: 0x0036A50C
		public Instance(IStateMachineTarget master, CreatureCalorieMonitor.Def def) : base(master, def)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
			this.calories.value = this.calories.GetMax() * 0.9f;
			this.stomach = new CreatureCalorieMonitor.Stomach(master.gameObject, def.minConsumedCaloriesBeforePooping, def.maxPoopSizeKG, def.storePoop);
			this.metabolism = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
			this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, false, false);
			this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
		}

		// Token: 0x06008F93 RID: 36755 RVA: 0x0036C3F1 File Offset: 0x0036A5F1
		public override void StartSM()
		{
			this.prefabID = base.gameObject.GetComponent<KPrefabID>();
			base.StartSM();
		}

		// Token: 0x06008F94 RID: 36756 RVA: 0x0036C40C File Offset: 0x0036A60C
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent value = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>)data).value;
			this.calories.value += value.calories;
			this.stomach.Consume(value.tag, value.calories);
			this.lastMealOrPoopTime = Time.time;
		}

		// Token: 0x06008F95 RID: 36757 RVA: 0x0036C45F File Offset: 0x0036A65F
		public float GetDeathTimeRemaining()
		{
			return base.smi.def.deathTimer - (GameClock.Instance.GetTime() - base.sm.starvationStartTime.Get(base.smi));
		}

		// Token: 0x06008F96 RID: 36758 RVA: 0x0036C493 File Offset: 0x0036A693
		public void Poop()
		{
			this.lastMealOrPoopTime = Time.time;
			this.stomach.Poop();
		}

		// Token: 0x06008F97 RID: 36759 RVA: 0x0036C4AB File Offset: 0x0036A6AB
		public float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x06008F98 RID: 36760 RVA: 0x0036C4C4 File Offset: 0x0036A6C4
		public bool IsHungry()
		{
			return this.GetCalories0to1() < this.HungryRatio;
		}

		// Token: 0x06008F99 RID: 36761 RVA: 0x0036C4D4 File Offset: 0x0036A6D4
		public bool IsOutOfCalories()
		{
			return this.GetCalories0to1() <= 0f;
		}

		// Token: 0x04006E54 RID: 28244
		public AmountInstance calories;

		// Token: 0x04006E55 RID: 28245
		[Serialize]
		public CreatureCalorieMonitor.Stomach stomach;

		// Token: 0x04006E56 RID: 28246
		public float lastMealOrPoopTime;

		// Token: 0x04006E57 RID: 28247
		public AttributeInstance metabolism;

		// Token: 0x04006E58 RID: 28248
		public AttributeModifier deltaCalorieMetabolismModifier;

		// Token: 0x04006E59 RID: 28249
		public KPrefabID prefabID;
	}
}
