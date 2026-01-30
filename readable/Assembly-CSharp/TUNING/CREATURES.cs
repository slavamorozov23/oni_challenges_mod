using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;

namespace TUNING
{
	// Token: 0x02000FDD RID: 4061
	public class CREATURES
	{
		// Token: 0x04005ED3 RID: 24275
		public const float WILD_GROWTH_RATE_MODIFIER = 0.25f;

		// Token: 0x04005ED4 RID: 24276
		public const int DEFAULT_PROBING_RADIUS = 32;

		// Token: 0x04005ED5 RID: 24277
		public const float CREATURES_BASE_GENERATION_KILOWATTS = 10f;

		// Token: 0x04005ED6 RID: 24278
		public const float FERTILITY_TIME_BY_LIFESPAN = 0.6f;

		// Token: 0x04005ED7 RID: 24279
		public const float INCUBATION_TIME_BY_LIFESPAN = 0.2f;

		// Token: 0x04005ED8 RID: 24280
		public const float INCUBATOR_INCUBATION_MULTIPLIER = 4f;

		// Token: 0x04005ED9 RID: 24281
		public const float WILD_CALORIE_BURN_RATIO = 0.25f;

		// Token: 0x04005EDA RID: 24282
		public const float HUG_INCUBATION_MULTIPLIER = 1f;

		// Token: 0x04005EDB RID: 24283
		public const float VIABILITY_LOSS_RATE = -0.016666668f;

		// Token: 0x04005EDC RID: 24284
		public const float STATERPILLAR_POWER_CHARGE_LOSS_RATE = -0.055555556f;

		// Token: 0x04005EDD RID: 24285
		public const float HUNT_FAILED_DURATION = 45f;

		// Token: 0x04005EDE RID: 24286
		public const float EVADED_HUNT_DURATION = 10f;

		// Token: 0x02002223 RID: 8739
		public class HITPOINTS
		{
			// Token: 0x04009D8B RID: 40331
			public const float TIER0 = 5f;

			// Token: 0x04009D8C RID: 40332
			public const float TIER1 = 25f;

			// Token: 0x04009D8D RID: 40333
			public const float TIER2 = 50f;

			// Token: 0x04009D8E RID: 40334
			public const float TIER3 = 100f;

			// Token: 0x04009D8F RID: 40335
			public const float TIER4 = 150f;

			// Token: 0x04009D90 RID: 40336
			public const float TIER5 = 200f;

			// Token: 0x04009D91 RID: 40337
			public const float TIER6 = 400f;
		}

		// Token: 0x02002224 RID: 8740
		public class MASS_KG
		{
			// Token: 0x04009D92 RID: 40338
			public const float TIER0 = 5f;

			// Token: 0x04009D93 RID: 40339
			public const float TIER1 = 25f;

			// Token: 0x04009D94 RID: 40340
			public const float TIER2 = 50f;

			// Token: 0x04009D95 RID: 40341
			public const float TIER3 = 100f;

			// Token: 0x04009D96 RID: 40342
			public const float TIER4 = 200f;

			// Token: 0x04009D97 RID: 40343
			public const float TIER5 = 400f;
		}

		// Token: 0x02002225 RID: 8741
		public class TEMPERATURE
		{
			// Token: 0x04009D98 RID: 40344
			public const float SKIN_THICKNESS = 0.025f;

			// Token: 0x04009D99 RID: 40345
			public const float SURFACE_AREA = 17.5f;

			// Token: 0x04009D9A RID: 40346
			public const float GROUND_TRANSFER_SCALE = 0f;

			// Token: 0x04009D9B RID: 40347
			public static float FREEZING_10 = 173f;

			// Token: 0x04009D9C RID: 40348
			public static float FREEZING_9 = 183f;

			// Token: 0x04009D9D RID: 40349
			public static float FREEZING_3 = 243f;

			// Token: 0x04009D9E RID: 40350
			public static float FREEZING_2 = 253f;

			// Token: 0x04009D9F RID: 40351
			public static float FREEZING_1 = 263f;

			// Token: 0x04009DA0 RID: 40352
			public static float FREEZING = 273f;

			// Token: 0x04009DA1 RID: 40353
			public static float COOL = 283f;

			// Token: 0x04009DA2 RID: 40354
			public static float MODERATE = 293f;

			// Token: 0x04009DA3 RID: 40355
			public static float HOT = 303f;

			// Token: 0x04009DA4 RID: 40356
			public static float HOT_1 = 313f;

			// Token: 0x04009DA5 RID: 40357
			public static float HOT_2 = 323f;

			// Token: 0x04009DA6 RID: 40358
			public static float HOT_3 = 333f;

			// Token: 0x04009DA7 RID: 40359
			public static float HOT_7 = 373f;
		}

		// Token: 0x02002226 RID: 8742
		public class LIFESPAN
		{
			// Token: 0x04009DA8 RID: 40360
			public const float TIER0 = 5f;

			// Token: 0x04009DA9 RID: 40361
			public const float TIER1 = 25f;

			// Token: 0x04009DAA RID: 40362
			public const float TIER2 = 75f;

			// Token: 0x04009DAB RID: 40363
			public const float TIER3 = 100f;

			// Token: 0x04009DAC RID: 40364
			public const float TIER4 = 150f;

			// Token: 0x04009DAD RID: 40365
			public const float TIER5 = 200f;

			// Token: 0x04009DAE RID: 40366
			public const float TIER6 = 400f;
		}

		// Token: 0x02002227 RID: 8743
		public class CONVERSION_EFFICIENCY
		{
			// Token: 0x04009DAF RID: 40367
			public static float BAD_2 = 0.1f;

			// Token: 0x04009DB0 RID: 40368
			public static float BAD_1 = 0.25f;

			// Token: 0x04009DB1 RID: 40369
			public static float NORMAL = 0.5f;

			// Token: 0x04009DB2 RID: 40370
			public static float GOOD_1 = 0.75f;

			// Token: 0x04009DB3 RID: 40371
			public static float GOOD_2 = 0.95f;

			// Token: 0x04009DB4 RID: 40372
			public static float GOOD_3 = 1f;
		}

		// Token: 0x02002228 RID: 8744
		public class SPACE_REQUIREMENTS
		{
			// Token: 0x04009DB5 RID: 40373
			public static int TIER1 = 4;

			// Token: 0x04009DB6 RID: 40374
			public static int TIER2 = 8;

			// Token: 0x04009DB7 RID: 40375
			public static int TIER3 = 12;

			// Token: 0x04009DB8 RID: 40376
			public static int TIER4 = 16;
		}

		// Token: 0x02002229 RID: 8745
		public class EGG_CHANCE_MODIFIERS
		{
			// Token: 0x0600BEC8 RID: 48840 RVA: 0x0040917D File Offset: 0x0040737D
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, HashSet<Tag> foodTags, float modifierPerCal)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.DIET.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.DIET.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in foodTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(-2038961714, delegate(object data)
							{
								CreatureCalorieMonitor.CaloriesConsumedEvent value = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>)data).value;
								if (foodTags.Contains(value.tag))
								{
									inst.AddBreedingChance(eggType, value.calories * modifierPerCal);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BEC9 RID: 48841 RVA: 0x004091AB File Offset: 0x004073AB
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, Tag foodTag, float modifierPerCal)
			{
				return CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier(id, eggTag, new HashSet<Tag>
				{
					foodTag
				}, modifierPerCal);
			}

			// Token: 0x0600BECA RID: 48842 RVA: 0x004091C2 File Offset: 0x004073C2
			private static System.Action CreateNearbyCreatureModifier(string id, Tag eggTag, Tag nearbyCreatureBaby, Tag nearbyCreatureAdult, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.NAME : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.NAME;
					string text2 = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.DESC : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string descStr) => string.Format(descStr, nearbyCreatureAdult.ProperName())));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							NearbyCreatureMonitor.Instance instance = inst.gameObject.GetSMI<NearbyCreatureMonitor.Instance>();
							if (instance == null)
							{
								instance = new NearbyCreatureMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateNearbyCreatures += delegate(float dt, List<KPrefabID> creatures, List<KPrefabID> eggs)
							{
								bool flag = false;
								foreach (KPrefabID kprefabID in creatures)
								{
									if (kprefabID.PrefabTag == nearbyCreatureBaby || kprefabID.PrefabTag == nearbyCreatureAdult)
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BECB RID: 48843 RVA: 0x00409200 File Offset: 0x00407400
			private static System.Action CreateElementCreatureModifier(string id, Tag eggTag, Tag element, float modifierPerSecond, bool alsoInvert, bool checkSubstantialLiquid, string tooltipOverride = null)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							if (tooltipOverride == null)
							{
								return string.Format(descStr, ElementLoader.GetElement(element).name);
							}
							return tooltipOverride;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CritterElementMonitor.Instance instance = inst.gameObject.GetSMI<CritterElementMonitor.Instance>();
							if (instance == null)
							{
								instance = new CritterElementMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateEggChances += delegate(float dt)
							{
								int num = Grid.PosToCell(inst);
								if (!Grid.IsValidCell(num))
								{
									return;
								}
								if (Grid.Element[num].HasTag(element) && (!checkSubstantialLiquid || Grid.IsSubstantialLiquid(num, 0.35f)))
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BECC RID: 48844 RVA: 0x00409251 File Offset: 0x00407451
			private static System.Action CreateCropTendedModifier(string id, Tag eggTag, HashSet<Tag> cropTags, float modifierPerEvent)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in cropTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(90606262, delegate(object data)
							{
								CropTendingStates.CropTendingEventData cropTendingEventData = (CropTendingStates.CropTendingEventData)data;
								if (cropTags.Contains(cropTendingEventData.cropId))
								{
									inst.AddBreedingChance(eggType, modifierPerEvent);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BECD RID: 48845 RVA: 0x0040927F File Offset: 0x0040747F
			private static System.Action CreateTemperatureModifier(string id, Tag eggTag, float minTemp, float maxTemp, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.NAME;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = null;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string src) => string.Format(CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.DESC, GameUtil.GetFormattedTemperature(minTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(maxTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CritterTemperatureMonitor.Instance smi = inst.gameObject.GetSMI<CritterTemperatureMonitor.Instance>();
							if (smi != null)
							{
								CritterTemperatureMonitor.Instance instance = smi;
								instance.OnUpdate_GetTemperatureInternal = (Action<float, float>)Delegate.Combine(instance.OnUpdate_GetTemperatureInternal, new Action<float, float>(delegate(float dt, float newTemp)
								{
									if (newTemp > minTemp && newTemp < maxTemp)
									{
										inst.AddBreedingChance(eggType, dt * modifierPerSecond);
										return;
									}
									if (alsoInvert)
									{
										inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
									}
								}));
								return;
							}
							DebugUtil.LogErrorArgs(new object[]
							{
								"Ack! Trying to add temperature modifier",
								id,
								"to",
								inst.master.name,
								"but it doesn't have a CritterTemperatureMonitor.Instance"
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BECE RID: 48846 RVA: 0x004092BD File Offset: 0x004074BD
			private static System.Action CreateDecorModifier(string id, Tag eggTag, float minDecor, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.DECOR.NAME;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = null;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string src) => string.Format(CREATURES.FERTILITY_MODIFIERS.DECOR.DESC, GameUtil.GetFormattedDecor(minDecor, false))));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CreatureDecorMonitor.Instance smi = inst.gameObject.GetSMI<CreatureDecorMonitor.Instance>();
							if (smi != null)
							{
								CreatureDecorMonitor.Instance instance = smi;
								instance.OnHighDecorUpdate = (Action<float>)Delegate.Combine(instance.OnHighDecorUpdate, new Action<float>(delegate(float dt)
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
								}));
								if (alsoInvert)
								{
									CreatureDecorMonitor.Instance instance2 = smi;
									instance2.OnLowDecorUpdate = (Action<float>)Delegate.Combine(instance2.OnLowDecorUpdate, new Action<float>(delegate(float dt)
									{
										inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
									}));
									return;
								}
							}
							else
							{
								DebugUtil.LogErrorArgs(new object[]
								{
									"Ack! Trying to add decor modifier",
									id,
									"to",
									inst.master.name,
									"but it doesn't have a CreatureDecorMonitor.Instance"
								});
							}
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x04009DB9 RID: 40377
			public static List<System.Action> MODIFIER_CREATORS = new List<System.Action>
			{
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchHard", "HatchHardEgg".ToTag(), SimHashes.SedimentaryRock.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchVeggie", "HatchVeggieEgg".ToTag(), SimHashes.Dirt.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchMetal", "HatchMetalEgg".ToTag(), HatchMetalConfig.METAL_ORE_TAGS, 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaBalance", "PuftAlphaEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), -0.00025f, true),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyOxylite", "PuftOxyliteEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyBleachstone", "PuftBleachstoneEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterHighTemp", "OilfloaterHighTempEgg".ToTag(), 373.15f, 523.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterDecor", "OilfloaterDecorEgg".ToTag(), 293.15f, 333.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugOrange", "LightBugOrangeEgg".ToTag(), "GrilledPrickleFruit".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPurple", "LightBugPurpleEgg".ToTag(), "FriedMushroom".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPink", "LightBugPinkEgg".ToTag(), "SpiceBread".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlue", "LightBugBlueEgg".ToTag(), "Salsa".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlack", "LightBugBlackEgg".ToTag(), SimHashes.Phosphorus.CreateTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugCrystal", "LightBugCrystalEgg".ToTag(), "CookedMeat".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuTropical", "PacuTropicalEgg".ToTag(), 308.15f, 353.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuCleaner", "PacuCleanerEgg".ToTag(), 243.15f, 278.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("DreckoPlastic", "DreckoPlasticEgg".ToTag(), "BasicSingleHarvestPlant".ToTag(), 0.025f / DreckoTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("SquirrelHug", "SquirrelHugEgg".ToTag(), BasicFabricMaterialPlantConfig.ID.ToTag(), 0.025f / SquirrelTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateCropTendedModifier("DivergentWorm", "DivergentWormEgg".ToTag(), new HashSet<Tag>
				{
					"WormPlant".ToTag(),
					"SuperWormPlant".ToTag()
				}, 0.05f / (float)DivergentTuning.TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeLumber", "CrabWoodEgg".ToTag(), SimHashes.Ethanol.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeFreshWater", "CrabFreshWaterEgg".ToTag(), SimHashes.Water.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("MoleDelicacy", "MoleDelicacyEgg".ToTag(), MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MIN, MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MAX, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarGas", "StaterpillarGasEgg".ToTag(), GameTags.Unbreathable, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.UNBREATHABLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarLiquid", "StaterpillarLiquidEgg".ToTag(), GameTags.Liquid, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.LIQUID),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("BellyGold", "GoldBellyEgg".ToTag(), "FriesCarrot".ToTag(), 0.05f / BellyTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDecorModifier("GlassDeerDecor", "GlassDeerEgg".ToTag(), 100f, 8.333333E-05f, true),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("AlgaeStego", "AlgaeStegoEgg".ToTag(), SimHashes.CarbonDioxide.CreateTag(), 0.00025f, true, false, null)
			};
		}

		// Token: 0x0200222A RID: 8746
		public class MOO_SONG_MODIFIERS
		{
			// Token: 0x0600BED1 RID: 48849 RVA: 0x004097E2 File Offset: 0x004079E2
			private static System.Action CreateDietaryModifier(string id, Tag meteorTag, HashSet<Tag> foodTags, float modifierPerCal)
			{
				Func<string, string> <>9__1;
				MooSongModifier.MooSongModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.MOO_SONG_MODIFIERS.DIET.NAME;
					string text2 = CREATURES.MOO_SONG_MODIFIERS.DIET.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag meteorTag2 = meteorTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in foodTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					MooSongModifier.MooSongModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(BeckoningMonitor.Instance inst, Tag meteorID)
						{
							inst.gameObject.Subscribe(-2038961714, delegate(object data)
							{
								CreatureCalorieMonitor.CaloriesConsumedEvent value = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>)data).value;
								if (foodTags.Contains(value.tag))
								{
									inst.AddSongChance(meteorID, value.calories * modifierPerCal);
								}
							});
						});
					}
					modifierSet.CreateMooSongModifier(id2, meteorTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BED2 RID: 48850 RVA: 0x00409810 File Offset: 0x00407A10
			private static System.Action CreateDietaryModifier(string id, Tag meteorTag, Tag foodTag, float modifierPerCal)
			{
				return CREATURES.MOO_SONG_MODIFIERS.CreateDietaryModifier(id, meteorTag, new HashSet<Tag>
				{
					foodTag
				}, modifierPerCal);
			}

			// Token: 0x04009DBA RID: 40378
			public static List<System.Action> MODIFIER_CREATORS = new List<System.Action>
			{
				CREATURES.MOO_SONG_MODIFIERS.CreateDietaryModifier("GassyMoo", GassyMooCometConfig.ID, "GasGrass", 0.05f / MooTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.MOO_SONG_MODIFIERS.CreateDietaryModifier("DieselMoo", DieselMooCometConfig.ID, "PlantFiber", 0.05f / MooTuning.STANDARD_CALORIES_PER_CYCLE)
			};
		}

		// Token: 0x0200222B RID: 8747
		public class SORTING
		{
			// Token: 0x04009DBB RID: 40379
			public static Dictionary<string, int> CRITTER_ORDER = new Dictionary<string, int>
			{
				{
					"Hatch",
					10
				},
				{
					"Puft",
					20
				},
				{
					"Drecko",
					30
				},
				{
					"Squirrel",
					40
				},
				{
					"Pacu",
					50
				},
				{
					"Oilfloater",
					60
				},
				{
					"LightBug",
					70
				},
				{
					"Crab",
					80
				},
				{
					"DivergentBeetle",
					90
				},
				{
					"Staterpillar",
					100
				},
				{
					"Mole",
					110
				},
				{
					"Bee",
					120
				},
				{
					"Moo",
					130
				},
				{
					"Glom",
					140
				},
				{
					"WoodDeer",
					150
				},
				{
					"Seal",
					160
				},
				{
					"IceBelly",
					170
				},
				{
					"Stego",
					180
				},
				{
					"Butterfly",
					190
				},
				{
					"Mosquito",
					200
				},
				{
					"Chameleon",
					210
				},
				{
					"PrehistoricPacu",
					220
				}
			};
		}
	}
}
