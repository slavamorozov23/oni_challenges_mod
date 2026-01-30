using System;
using System.Collections.Generic;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000FD5 RID: 4053
	public class DUPLICANTSTATS
	{
		// Token: 0x06007F1E RID: 32542 RVA: 0x00330BA0 File Offset: 0x0032EDA0
		public static DUPLICANTSTATS.TraitVal GetTraitVal(string id)
		{
			foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.SPECIALTRAITS)
			{
				if (id == traitVal.id)
				{
					return traitVal;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.GOODTRAITS)
			{
				if (id == traitVal2.id)
				{
					return traitVal2;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.BADTRAITS)
			{
				if (id == traitVal3.id)
				{
					return traitVal3;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.CONGENITALTRAITS)
			{
				if (id == traitVal4.id)
				{
					return traitVal4;
				}
			}
			DebugUtil.Assert(true, "Could not find TraitVal with ID: " + id);
			return DUPLICANTSTATS.INVALID_TRAIT_VAL;
		}

		// Token: 0x06007F1F RID: 32543 RVA: 0x00330D08 File Offset: 0x0032EF08
		public static DUPLICANTSTATS GetStatsFor(GameObject gameObject)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null)
			{
				return DUPLICANTSTATS.GetStatsFor(component);
			}
			return null;
		}

		// Token: 0x06007F20 RID: 32544 RVA: 0x00330D30 File Offset: 0x0032EF30
		public static DUPLICANTSTATS GetStatsFor(KPrefabID prefabID)
		{
			if (!prefabID.HasTag(GameTags.BaseMinion))
			{
				return null;
			}
			foreach (Tag tag in GameTags.Minions.Models.AllModels)
			{
				if (prefabID.HasTag(tag))
				{
					return DUPLICANTSTATS.GetStatsFor(tag);
				}
			}
			return null;
		}

		// Token: 0x06007F21 RID: 32545 RVA: 0x00330D79 File Offset: 0x0032EF79
		public static DUPLICANTSTATS GetStatsFor(Tag type)
		{
			if (DUPLICANTSTATS.DUPLICANT_TYPES.ContainsKey(type))
			{
				return DUPLICANTSTATS.DUPLICANT_TYPES[type];
			}
			return null;
		}

		// Token: 0x04005E47 RID: 24135
		public const float RANCHING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x04005E48 RID: 24136
		public const float FARMING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x04005E49 RID: 24137
		public const float POWER_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.025f;

		// Token: 0x04005E4A RID: 24138
		public const float RANCHING_CAPTURABLE_MULTIPLIER_BONUS_PER_POINT = 0.05f;

		// Token: 0x04005E4B RID: 24139
		public const float STANDARD_STRESS_PENALTY = 0.016666668f;

		// Token: 0x04005E4C RID: 24140
		public const float STANDARD_STRESS_BONUS = -0.033333335f;

		// Token: 0x04005E4D RID: 24141
		public const float STRESS_BELOW_EXPECTATIONS_FOOD = 0.25f;

		// Token: 0x04005E4E RID: 24142
		public const float STRESS_ABOVE_EXPECTATIONS_FOOD = -0.5f;

		// Token: 0x04005E4F RID: 24143
		public const float STANDARD_STRESS_PENALTY_SECOND = 0.25f;

		// Token: 0x04005E50 RID: 24144
		public const float STANDARD_STRESS_BONUS_SECOND = -0.5f;

		// Token: 0x04005E51 RID: 24145
		public const float TRAVEL_TIME_WARNING_THRESHOLD = 0.4f;

		// Token: 0x04005E52 RID: 24146
		public static string[] ALL_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching",
			"Athletics",
			"SpaceNavigation"
		};

		// Token: 0x04005E53 RID: 24147
		public static string[] DISTRIBUTED_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching"
		};

		// Token: 0x04005E54 RID: 24148
		public static string[] ROLLED_ATTRIBUTES = new string[]
		{
			"Athletics"
		};

		// Token: 0x04005E55 RID: 24149
		public static int[] APTITUDE_ATTRIBUTE_BONUSES = new int[]
		{
			7,
			3,
			1
		};

		// Token: 0x04005E56 RID: 24150
		public static int ROLLED_ATTRIBUTE_MAX = 5;

		// Token: 0x04005E57 RID: 24151
		public static float ROLLED_ATTRIBUTE_POWER = 4f;

		// Token: 0x04005E58 RID: 24152
		public static Dictionary<string, List<string>> ARCHETYPE_TRAIT_EXCLUSIONS = new Dictionary<string, List<string>>
		{
			{
				"Mining",
				new List<string>
				{
					"Anemic",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Building",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"ConstructionDown",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Farming",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"BotanistDown",
					"RanchingDown",
					"Narcolepsy"
				}
			},
			{
				"Ranching",
				new List<string>
				{
					"RanchingDown",
					"BotanistDown",
					"Narcolepsy"
				}
			},
			{
				"Cooking",
				new List<string>
				{
					"NoodleArms",
					"CookingDown"
				}
			},
			{
				"Art",
				new List<string>
				{
					"ArtDown",
					"DecorDown"
				}
			},
			{
				"Research",
				new List<string>
				{
					"SlowLearner"
				}
			},
			{
				"Suits",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Hauling",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"Narcolepsy"
				}
			},
			{
				"Technicals",
				new List<string>
				{
					"MachineryDown"
				}
			},
			{
				"MedicalAid",
				new List<string>
				{
					"CaringDown",
					"WeakImmuneSystem"
				}
			},
			{
				"Basekeeping",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Rocketry",
				new List<string>()
			}
		};

		// Token: 0x04005E59 RID: 24153
		public static Dictionary<string, List<string>> ARCHETYPE_BIONIC_TRAIT_COMPATIBILITY = new Dictionary<string, List<string>>
		{
			{
				"Mining",
				new List<string>
				{
					"Booster_Dig1",
					"Booster_Dig2"
				}
			},
			{
				"Building",
				new List<string>
				{
					"Booster_Construct1"
				}
			},
			{
				"Farming",
				new List<string>
				{
					"Booster_Farm1"
				}
			},
			{
				"Ranching",
				new List<string>
				{
					"Booster_Ranch1"
				}
			},
			{
				"Cooking",
				new List<string>
				{
					"Booster_Cook1"
				}
			},
			{
				"Art",
				new List<string>
				{
					"Booster_Art1"
				}
			},
			{
				"Research",
				new List<string>
				{
					"Booster_Research1",
					"Booster_Research2",
					"Booster_Research3"
				}
			},
			{
				"Suits",
				new List<string>
				{
					"Booster_Suits1"
				}
			},
			{
				"Hauling",
				new List<string>
				{
					"Booster_Tidy1",
					"Booster_Carry1"
				}
			},
			{
				"Technicals",
				new List<string>
				{
					"Booster_Op1",
					"Booster_Op2"
				}
			},
			{
				"MedicalAid",
				new List<string>
				{
					"Booster_Medicine1"
				}
			},
			{
				"Basekeeping",
				new List<string>
				{
					"Booster_Tidy1",
					"Booster_Carry1"
				}
			},
			{
				"Rocketry",
				new List<string>
				{
					"Booster_PilotVanilla1",
					"Booster_Pilot1"
				}
			}
		};

		// Token: 0x04005E5A RID: 24154
		public static int RARITY_LEGENDARY = 5;

		// Token: 0x04005E5B RID: 24155
		public static int RARITY_EPIC = 4;

		// Token: 0x04005E5C RID: 24156
		public static int RARITY_RARE = 3;

		// Token: 0x04005E5D RID: 24157
		public static int RARITY_UNCOMMON = 2;

		// Token: 0x04005E5E RID: 24158
		public static int RARITY_COMMON = 1;

		// Token: 0x04005E5F RID: 24159
		public static int NO_STATPOINT_BONUS = 0;

		// Token: 0x04005E60 RID: 24160
		public static int TINY_STATPOINT_BONUS = 1;

		// Token: 0x04005E61 RID: 24161
		public static int SMALL_STATPOINT_BONUS = 2;

		// Token: 0x04005E62 RID: 24162
		public static int MEDIUM_STATPOINT_BONUS = 3;

		// Token: 0x04005E63 RID: 24163
		public static int LARGE_STATPOINT_BONUS = 4;

		// Token: 0x04005E64 RID: 24164
		public static int HUGE_STATPOINT_BONUS = 5;

		// Token: 0x04005E65 RID: 24165
		public static int COMMON = 1;

		// Token: 0x04005E66 RID: 24166
		public static int UNCOMMON = 2;

		// Token: 0x04005E67 RID: 24167
		public static int RARE = 3;

		// Token: 0x04005E68 RID: 24168
		public static int EPIC = 4;

		// Token: 0x04005E69 RID: 24169
		public static int LEGENDARY = 5;

		// Token: 0x04005E6A RID: 24170
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(1, 1);

		// Token: 0x04005E6B RID: 24171
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(2, 1);

		// Token: 0x04005E6C RID: 24172
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(1, 2);

		// Token: 0x04005E6D RID: 24173
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(2, 2);

		// Token: 0x04005E6E RID: 24174
		public static global::Tuple<int, int> TRAITS_THREE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(3, 1);

		// Token: 0x04005E6F RID: 24175
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_THREE_NEGATIVE = new global::Tuple<int, int>(1, 3);

		// Token: 0x04005E70 RID: 24176
		public static int MIN_STAT_POINTS = 0;

		// Token: 0x04005E71 RID: 24177
		public static int MAX_STAT_POINTS = 0;

		// Token: 0x04005E72 RID: 24178
		public static int MAX_TRAITS = 4;

		// Token: 0x04005E73 RID: 24179
		public static int APTITUDE_BONUS = 1;

		// Token: 0x04005E74 RID: 24180
		public static List<int> RARITY_DECK = new List<int>
		{
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_LEGENDARY
		};

		// Token: 0x04005E75 RID: 24181
		public static List<int> rarityDeckActive = new List<int>(DUPLICANTSTATS.RARITY_DECK);

		// Token: 0x04005E76 RID: 24182
		public static List<global::Tuple<int, int>> POD_TRAIT_CONFIGURATIONS_DECK = new List<global::Tuple<int, int>>
		{
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_THREE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_THREE_NEGATIVE
		};

		// Token: 0x04005E77 RID: 24183
		public static List<global::Tuple<int, int>> podTraitConfigurationsActive = new List<global::Tuple<int, int>>(DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);

		// Token: 0x04005E78 RID: 24184
		public static List<string> CONTRACTEDTRAITS_HEALING = new List<string>
		{
			"IrritableBowel",
			"Aggressive",
			"SlowLearner",
			"WeakImmuneSystem",
			"Snorer",
			"CantDig"
		};

		// Token: 0x04005E79 RID: 24185
		public static List<DUPLICANTSTATS.TraitVal> CONGENITALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "None"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Joshua",
				mutuallyExclusiveTraits = new List<string>
				{
					"ScaredyCat",
					"Aggressive"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Ellie",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator",
					"MouthBreather",
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Stinky",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Liam",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			}
		};

		// Token: 0x04005E7A RID: 24186
		public static readonly DUPLICANTSTATS.TraitVal INVALID_TRAIT_VAL = new DUPLICANTSTATS.TraitVal
		{
			id = "INVALID"
		};

		// Token: 0x04005E7B RID: 24187
		public static List<DUPLICANTSTATS.TraitVal> BADTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantResearch",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Research"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantDig",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantCook",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantBuild",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Building"
				},
				mutuallyExclusiveTraits = new List<string>
				{
					"GrantSkill_Engineering1"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Hemophobia",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"MedicalAid"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ScaredyCat",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionUp",
					"CantBuild"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingUp"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CaringDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BotanistDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ArtDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CookingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie",
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MachineryDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiggingDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"MoleHands",
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SlowLearner",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"FastLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NoodleArms",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorDown",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Anemic",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Flatulence",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IrritableBowel",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Snorer",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MouthBreather",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SmallBladder",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CalorieBurner",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "WeakImmuneSystem",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Allergies",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightLight",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Narcolepsy",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			}
		};

		// Token: 0x04005E7C RID: 24188
		public static List<DUPLICANTSTATS.TraitVal> STRESSTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Aggressive"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StressVomiter"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "UglyCrier"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BingeEater"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Banshee"
			}
		};

		// Token: 0x04005E7D RID: 24189
		public static List<DUPLICANTSTATS.TraitVal> JOYTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "BalloonArtist"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SparkleStreaker"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StickerBomber"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SuperProductive"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "HappySinger"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DataRainer",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RoboDancer",
				requiredDlcIds = DlcManager.DLC3
			}
		};

		// Token: 0x04005E7E RID: 24190
		public static List<DUPLICANTSTATS.TraitVal> GENESHUFFLERTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Regeneration"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DeeperDiversLungs"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SunnyDisposition"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RockCrusher"
			}
		};

		// Token: 0x04005E7F RID: 24191
		public static List<DUPLICANTSTATS.TraitVal> BIONICBUGTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug1",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug2",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug3",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug4",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug5",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug6",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug7",
				requiredDlcIds = DlcManager.DLC3
			}
		};

		// Token: 0x04005E80 RID: 24192
		public static readonly List<DUPLICANTSTATS.TraitVal> BIONICUPGRADETRAITS = new List<DUPLICANTSTATS.TraitVal>();

		// Token: 0x04005E81 RID: 24193
		public static List<DUPLICANTSTATS.TraitVal> SPECIALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "AncientKnowledge",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				requiredDlcIds = DlcManager.EXPANSION1,
				doNotGenerateTrait = true,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantResearch",
					"CantBuild",
					"CantCook",
					"CantDig",
					"Hemophobia",
					"ScaredyCat",
					"Anemic",
					"SlowLearner",
					"NoodleArms",
					"ConstructionDown",
					"RanchingDown",
					"DiggingDown",
					"MachineryDown",
					"CookingDown",
					"ArtDown",
					"CaringDown",
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Chatty",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				doNotGenerateTrait = true
			}
		};

		// Token: 0x04005E82 RID: 24194
		public static List<DUPLICANTSTATS.TraitVal> GOODTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Twinkletoes",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongArm",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"NoodleArms"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Greasemonkey",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"MachineryDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiversLung",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"MouthBreather"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IronGut",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongImmuneSystem",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"WeakImmuneSystem"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "EarlyBird",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"NightOwl"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightOwl",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"EarlyBird"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Meteorphile",
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MoleHands",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig",
					"DiggingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "FastLearner",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"SlowLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "InteriorDecorator",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured",
					"ArtDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Uncultured",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Art"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SimpleTastes",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Foodie",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"SimpleTastes",
					"CantCook",
					"CookingDown"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BedsideManner",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia",
					"CaringDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"DecorDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Thriver",
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GreenThumb",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionDown",
					"CantBuild"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Loner",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				requiredDlcIds = DlcManager.EXPANSION1
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StarryEyed",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				requiredDlcIds = DlcManager.EXPANSION1
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GlowStick",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				requiredDlcIds = DlcManager.EXPANSION1
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RadiationEater",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				requiredDlcIds = DlcManager.EXPANSION1
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "FrostProof",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				requiredDlcIds = DlcManager.DLC2
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Farming2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Ranching1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Cooking1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Suits1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Technicals2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Engineering1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Basekeeping2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Medicine2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			}
		};

		// Token: 0x04005E83 RID: 24195
		public static List<DUPLICANTSTATS.TraitVal> NEEDTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Claustrophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersWarmer",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersColder"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersColder",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersWarmer"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SensitiveFeet",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Fashionable",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Climacophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SolitarySleeper",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			}
		};

		// Token: 0x04005E84 RID: 24196
		public static DUPLICANTSTATS STANDARD = new DUPLICANTSTATS();

		// Token: 0x04005E85 RID: 24197
		public static DUPLICANTSTATS BIONICS = new DUPLICANTSTATS
		{
			BaseStats = new DUPLICANTSTATS.BASESTATS
			{
				MAX_CALORIES = 0f
			},
			DiseaseImmunities = new DUPLICANTSTATS.DISEASEIMMUNITIES
			{
				IMMUNITIES = new string[]
				{
					"FoodSickness"
				}
			}
		};

		// Token: 0x04005E86 RID: 24198
		private static Dictionary<Tag, DUPLICANTSTATS> DUPLICANT_TYPES = new Dictionary<Tag, DUPLICANTSTATS>
		{
			{
				GameTags.Minions.Models.Standard,
				DUPLICANTSTATS.STANDARD
			},
			{
				GameTags.Minions.Models.Bionic,
				DUPLICANTSTATS.BIONICS
			}
		};

		// Token: 0x04005E87 RID: 24199
		public DUPLICANTSTATS.BASESTATS BaseStats = new DUPLICANTSTATS.BASESTATS();

		// Token: 0x04005E88 RID: 24200
		public DUPLICANTSTATS.DISEASEIMMUNITIES DiseaseImmunities = new DUPLICANTSTATS.DISEASEIMMUNITIES();

		// Token: 0x04005E89 RID: 24201
		public DUPLICANTSTATS.TEMPERATURE Temperature = new DUPLICANTSTATS.TEMPERATURE();

		// Token: 0x04005E8A RID: 24202
		public DUPLICANTSTATS.BREATH Breath = new DUPLICANTSTATS.BREATH();

		// Token: 0x04005E8B RID: 24203
		public DUPLICANTSTATS.LIGHT Light = new DUPLICANTSTATS.LIGHT();

		// Token: 0x04005E8C RID: 24204
		public DUPLICANTSTATS.COMBAT Combat = new DUPLICANTSTATS.COMBAT();

		// Token: 0x04005E8D RID: 24205
		public DUPLICANTSTATS.SECRETIONS Secretions = new DUPLICANTSTATS.SECRETIONS();

		// Token: 0x02002204 RID: 8708
		public static class RADIATION_DIFFICULTY_MODIFIERS
		{
			// Token: 0x04009C97 RID: 40087
			public static float HARDEST = 0.33f;

			// Token: 0x04009C98 RID: 40088
			public static float HARDER = 0.66f;

			// Token: 0x04009C99 RID: 40089
			public static float DEFAULT = 1f;

			// Token: 0x04009C9A RID: 40090
			public static float EASIER = 2f;

			// Token: 0x04009C9B RID: 40091
			public static float EASIEST = 100f;
		}

		// Token: 0x02002205 RID: 8709
		public static class RADIATION_EXPOSURE_LEVELS
		{
			// Token: 0x04009C9C RID: 40092
			public const float LOW = 100f;

			// Token: 0x04009C9D RID: 40093
			public const float MODERATE = 300f;

			// Token: 0x04009C9E RID: 40094
			public const float HIGH = 600f;

			// Token: 0x04009C9F RID: 40095
			public const float DEADLY = 900f;
		}

		// Token: 0x02002206 RID: 8710
		public static class MOVEMENT_MODIFIERS
		{
			// Token: 0x04009CA0 RID: 40096
			public static float NEUTRAL = 1f;

			// Token: 0x04009CA1 RID: 40097
			public static float BONUS_1 = 1.1f;

			// Token: 0x04009CA2 RID: 40098
			public static float BONUS_2 = 1.25f;

			// Token: 0x04009CA3 RID: 40099
			public static float BONUS_3 = 1.5f;

			// Token: 0x04009CA4 RID: 40100
			public static float BONUS_4 = 1.75f;

			// Token: 0x04009CA5 RID: 40101
			public static float PENALTY_1 = 0.9f;

			// Token: 0x04009CA6 RID: 40102
			public static float PENALTY_2 = 0.75f;

			// Token: 0x04009CA7 RID: 40103
			public static float PENALTY_3 = 0.5f;

			// Token: 0x04009CA8 RID: 40104
			public static float PENALTY_4 = 0.25f;
		}

		// Token: 0x02002207 RID: 8711
		public static class QOL_STRESS
		{
			// Token: 0x04009CA9 RID: 40105
			public const float ABOVE_EXPECTATIONS = -0.016666668f;

			// Token: 0x04009CAA RID: 40106
			public const float AT_EXPECTATIONS = -0.008333334f;

			// Token: 0x04009CAB RID: 40107
			public const float MIN_STRESS = -0.033333335f;

			// Token: 0x02002A9B RID: 10907
			public static class BELOW_EXPECTATIONS
			{
				// Token: 0x0400BBEE RID: 48110
				public const float EASY = 0.0033333334f;

				// Token: 0x0400BBEF RID: 48111
				public const float NEUTRAL = 0.004166667f;

				// Token: 0x0400BBF0 RID: 48112
				public const float HARD = 0.008333334f;

				// Token: 0x0400BBF1 RID: 48113
				public const float VERYHARD = 0.016666668f;
			}

			// Token: 0x02002A9C RID: 10908
			public static class MAX_STRESS
			{
				// Token: 0x0400BBF2 RID: 48114
				public const float EASY = 0.016666668f;

				// Token: 0x0400BBF3 RID: 48115
				public const float NEUTRAL = 0.041666668f;

				// Token: 0x0400BBF4 RID: 48116
				public const float HARD = 0.05f;

				// Token: 0x0400BBF5 RID: 48117
				public const float VERYHARD = 0.083333336f;
			}
		}

		// Token: 0x02002208 RID: 8712
		public static class CLOTHING
		{
			// Token: 0x02002A9D RID: 10909
			public class DECOR_MODIFICATION
			{
				// Token: 0x0400BBF6 RID: 48118
				public const int NEGATIVE_SIGNIFICANT = -30;

				// Token: 0x0400BBF7 RID: 48119
				public const int NEGATIVE_MILD = -10;

				// Token: 0x0400BBF8 RID: 48120
				public const int BASIC = -5;

				// Token: 0x0400BBF9 RID: 48121
				public const int POSITIVE_MILD = 10;

				// Token: 0x0400BBFA RID: 48122
				public const int POSITIVE_SIGNIFICANT = 30;

				// Token: 0x0400BBFB RID: 48123
				public const int POSITIVE_MAJOR = 40;
			}

			// Token: 0x02002A9E RID: 10910
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x0400BBFC RID: 48124
				public const float THIN = 0.0005f;

				// Token: 0x0400BBFD RID: 48125
				public const float BASIC = 0.0025f;

				// Token: 0x0400BBFE RID: 48126
				public const float THICK = 0.008f;
			}

			// Token: 0x02002A9F RID: 10911
			public class SWEAT_EFFICIENCY_MULTIPLIER
			{
				// Token: 0x0400BBFF RID: 48127
				public const float DIMINISH_SIGNIFICANT = -2.5f;

				// Token: 0x0400BC00 RID: 48128
				public const float DIMINISH_MILD = -1.25f;

				// Token: 0x0400BC01 RID: 48129
				public const float NEUTRAL = 0f;

				// Token: 0x0400BC02 RID: 48130
				public const float IMPROVE = 2f;
			}
		}

		// Token: 0x02002209 RID: 8713
		public static class NOISE
		{
			// Token: 0x04009CAC RID: 40108
			public const int THRESHOLD_PEACEFUL = 0;

			// Token: 0x04009CAD RID: 40109
			public const int THRESHOLD_QUIET = 36;

			// Token: 0x04009CAE RID: 40110
			public const int THRESHOLD_TOSS_AND_TURN = 45;

			// Token: 0x04009CAF RID: 40111
			public const int THRESHOLD_WAKE_UP = 60;

			// Token: 0x04009CB0 RID: 40112
			public const int THRESHOLD_MINOR_REACTION = 80;

			// Token: 0x04009CB1 RID: 40113
			public const int THRESHOLD_MAJOR_REACTION = 106;

			// Token: 0x04009CB2 RID: 40114
			public const int THRESHOLD_EXTREME_REACTION = 125;
		}

		// Token: 0x0200220A RID: 8714
		public static class ROOM
		{
			// Token: 0x04009CB3 RID: 40115
			public const float LABORATORY_RESEARCH_EFFICIENCY_BONUS = 0.1f;
		}

		// Token: 0x0200220B RID: 8715
		public class DISTRIBUTIONS
		{
			// Token: 0x0600BE9A RID: 48794 RVA: 0x00407EAF File Offset: 0x004060AF
			public static int[] GetRandomDistribution()
			{
				return DUPLICANTSTATS.DISTRIBUTIONS.TYPES[UnityEngine.Random.Range(0, DUPLICANTSTATS.DISTRIBUTIONS.TYPES.Count)];
			}

			// Token: 0x04009CB4 RID: 40116
			public static readonly List<int[]> TYPES = new List<int[]>
			{
				new int[]
				{
					5,
					4,
					4,
					3,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					2,
					2,
					1
				},
				new int[]
				{
					5,
					1
				},
				new int[]
				{
					5,
					3,
					1
				},
				new int[]
				{
					3,
					3,
					3,
					3,
					1
				},
				new int[]
				{
					4
				},
				new int[]
				{
					3
				},
				new int[]
				{
					2
				},
				new int[]
				{
					1
				}
			};
		}

		// Token: 0x0200220C RID: 8716
		public struct TraitVal : IHasDlcRestrictions
		{
			// Token: 0x0600BE9D RID: 48797 RVA: 0x00407FB4 File Offset: 0x004061B4
			public string[] GetRequiredDlcIds()
			{
				return this.requiredDlcIds;
			}

			// Token: 0x0600BE9E RID: 48798 RVA: 0x00407FBC File Offset: 0x004061BC
			public string[] GetForbiddenDlcIds()
			{
				return this.forbiddenDlcIds;
			}

			// Token: 0x04009CB5 RID: 40117
			public string id;

			// Token: 0x04009CB6 RID: 40118
			public int statBonus;

			// Token: 0x04009CB7 RID: 40119
			public int impact;

			// Token: 0x04009CB8 RID: 40120
			public int rarity;

			// Token: 0x04009CB9 RID: 40121
			public List<string> mutuallyExclusiveTraits;

			// Token: 0x04009CBA RID: 40122
			public List<HashedString> mutuallyExclusiveAptitudes;

			// Token: 0x04009CBB RID: 40123
			public bool doNotGenerateTrait;

			// Token: 0x04009CBC RID: 40124
			public string[] requiredDlcIds;

			// Token: 0x04009CBD RID: 40125
			public string[] forbiddenDlcIds;
		}

		// Token: 0x0200220D RID: 8717
		public class ATTRIBUTE_LEVELING
		{
			// Token: 0x04009CBE RID: 40126
			public static int MAX_GAINED_ATTRIBUTE_LEVEL = 20;

			// Token: 0x04009CBF RID: 40127
			public static int TARGET_MAX_LEVEL_CYCLE = 400;

			// Token: 0x04009CC0 RID: 40128
			public static float EXPERIENCE_LEVEL_POWER = 1.7f;

			// Token: 0x04009CC1 RID: 40129
			public static float FULL_EXPERIENCE = 1f;

			// Token: 0x04009CC2 RID: 40130
			public static float ALL_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.8f;

			// Token: 0x04009CC3 RID: 40131
			public static float MOST_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.5f;

			// Token: 0x04009CC4 RID: 40132
			public static float PART_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.25f;

			// Token: 0x04009CC5 RID: 40133
			public static float BARELY_EVER_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.1f;
		}

		// Token: 0x0200220E RID: 8718
		public class BASESTATS
		{
			// Token: 0x17000D50 RID: 3408
			// (get) Token: 0x0600BEA1 RID: 48801 RVA: 0x0040803E File Offset: 0x0040623E
			public float CALORIES_BURNED_PER_SECOND
			{
				get
				{
					return this.CALORIES_BURNED_PER_CYCLE / 600f;
				}
			}

			// Token: 0x17000D51 RID: 3409
			// (get) Token: 0x0600BEA2 RID: 48802 RVA: 0x0040804C File Offset: 0x0040624C
			public float HUNGRY_THRESHOLD
			{
				get
				{
					return this.SATISFIED_THRESHOLD - -this.CALORIES_BURNED_PER_CYCLE * 0.5f / this.MAX_CALORIES;
				}
			}

			// Token: 0x17000D52 RID: 3410
			// (get) Token: 0x0600BEA3 RID: 48803 RVA: 0x00408069 File Offset: 0x00406269
			public float STARVING_THRESHOLD
			{
				get
				{
					return -this.CALORIES_BURNED_PER_CYCLE / this.MAX_CALORIES;
				}
			}

			// Token: 0x17000D53 RID: 3411
			// (get) Token: 0x0600BEA4 RID: 48804 RVA: 0x00408079 File Offset: 0x00406279
			public float DUPLICANT_COOLING_KILOWATTS
			{
				get
				{
					return this.COOLING_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000D54 RID: 3412
			// (get) Token: 0x0600BEA5 RID: 48805 RVA: 0x0040809C File Offset: 0x0040629C
			public float DUPLICANT_WARMING_KILOWATTS
			{
				get
				{
					return this.WARMING_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000D55 RID: 3413
			// (get) Token: 0x0600BEA6 RID: 48806 RVA: 0x004080BF File Offset: 0x004062BF
			public float DUPLICANT_BASE_GENERATION_KILOWATTS
			{
				get
				{
					return this.HEAT_GENERATION_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000D56 RID: 3414
			// (get) Token: 0x0600BEA7 RID: 48807 RVA: 0x004080E2 File Offset: 0x004062E2
			public float GUESSTIMATE_CALORIES_BURNED_PER_SECOND
			{
				get
				{
					return this.CALORIES_BURNED_PER_CYCLE / 600f;
				}
			}

			// Token: 0x04009CC6 RID: 40134
			public float DEFAULT_MASS = 30f;

			// Token: 0x04009CC7 RID: 40135
			public float STAMINA_USED_PER_SECOND = -0.11666667f;

			// Token: 0x04009CC8 RID: 40136
			public float TRANSIT_TUBE_TRAVEL_SPEED = 18f;

			// Token: 0x04009CC9 RID: 40137
			public float OXYGEN_USED_PER_SECOND = 0.1f;

			// Token: 0x04009CCA RID: 40138
			public float OXYGEN_TO_CO2_CONVERSION = 0.02f;

			// Token: 0x04009CCB RID: 40139
			public float LOW_OXYGEN_THRESHOLD = 0.52f;

			// Token: 0x04009CCC RID: 40140
			public float NO_OXYGEN_THRESHOLD = 0.05f;

			// Token: 0x04009CCD RID: 40141
			public float RECOVER_BREATH_DELTA = 3f;

			// Token: 0x04009CCE RID: 40142
			public float MIN_CO2_TO_EMIT = 0.02f;

			// Token: 0x04009CCF RID: 40143
			public float BLADDER_INCREASE_PER_SECOND = 0.16666667f;

			// Token: 0x04009CD0 RID: 40144
			public float DECOR_EXPECTATION;

			// Token: 0x04009CD1 RID: 40145
			public float FOOD_QUALITY_EXPECTATION;

			// Token: 0x04009CD2 RID: 40146
			public float RECREATION_EXPECTATION = 2f;

			// Token: 0x04009CD3 RID: 40147
			public float MAX_PROFESSION_DECOR_EXPECTATION = 75f;

			// Token: 0x04009CD4 RID: 40148
			public float MAX_PROFESSION_FOOD_EXPECTATION;

			// Token: 0x04009CD5 RID: 40149
			public int MAX_UNDERWATER_TRAVEL_COST = 8;

			// Token: 0x04009CD6 RID: 40150
			public float TOILET_EFFICIENCY = 1f;

			// Token: 0x04009CD7 RID: 40151
			public float ROOM_TEMPERATURE_PREFERENCE;

			// Token: 0x04009CD8 RID: 40152
			public int BUILDING_DAMAGE_ACTING_OUT = 100;

			// Token: 0x04009CD9 RID: 40153
			public float IMMUNE_LEVEL_MAX = 100f;

			// Token: 0x04009CDA RID: 40154
			public float IMMUNE_LEVEL_RECOVERY = 0.025f;

			// Token: 0x04009CDB RID: 40155
			public float CARRY_CAPACITY = 200f;

			// Token: 0x04009CDC RID: 40156
			public float HIT_POINTS = 100f;

			// Token: 0x04009CDD RID: 40157
			public float RADIATION_RESISTANCE;

			// Token: 0x04009CDE RID: 40158
			public string NAV_GRID_NAME = "MinionNavGrid";

			// Token: 0x04009CDF RID: 40159
			public float KCAL2JOULES = 4184f;

			// Token: 0x04009CE0 RID: 40160
			public float MAX_CALORIES = 4000000f;

			// Token: 0x04009CE1 RID: 40161
			public float CALORIES_BURNED_PER_CYCLE = -1000000f;

			// Token: 0x04009CE2 RID: 40162
			public float SATISFIED_THRESHOLD = 0.95f;

			// Token: 0x04009CE3 RID: 40163
			public float COOLING_EFFICIENCY = 0.08f;

			// Token: 0x04009CE4 RID: 40164
			public float WARMING_EFFICIENCY = 0.08f;

			// Token: 0x04009CE5 RID: 40165
			public float HEAT_GENERATION_EFFICIENCY = 0.012f;

			// Token: 0x04009CE6 RID: 40166
			public float GUESSTIMATE_CALORIES_PER_CYCLE = -1600000f;
		}

		// Token: 0x0200220F RID: 8719
		public class DISEASEIMMUNITIES
		{
			// Token: 0x04009CE7 RID: 40167
			public string[] IMMUNITIES;
		}

		// Token: 0x02002210 RID: 8720
		public class TEMPERATURE
		{
			// Token: 0x04009CE8 RID: 40168
			public DUPLICANTSTATS.TEMPERATURE.EXTERNAL External = new DUPLICANTSTATS.TEMPERATURE.EXTERNAL();

			// Token: 0x04009CE9 RID: 40169
			public DUPLICANTSTATS.TEMPERATURE.INTERNAL Internal = new DUPLICANTSTATS.TEMPERATURE.INTERNAL();

			// Token: 0x04009CEA RID: 40170
			public DUPLICANTSTATS.TEMPERATURE.CONDUCTIVITY_BARRIER_MODIFICATION Conductivity_Barrier_Modification = new DUPLICANTSTATS.TEMPERATURE.CONDUCTIVITY_BARRIER_MODIFICATION();

			// Token: 0x04009CEB RID: 40171
			public float SKIN_THICKNESS = 0.002f;

			// Token: 0x04009CEC RID: 40172
			public float SURFACE_AREA = 1f;

			// Token: 0x04009CED RID: 40173
			public float GROUND_TRANSFER_SCALE;

			// Token: 0x02002AA0 RID: 10912
			public class EXTERNAL
			{
				// Token: 0x0400BC03 RID: 48131
				public float THRESHOLD_COLD = 283.15f;

				// Token: 0x0400BC04 RID: 48132
				public float THRESHOLD_HOT = 306.15f;

				// Token: 0x0400BC05 RID: 48133
				public float THRESHOLD_SCALDING = 345f;
			}

			// Token: 0x02002AA1 RID: 10913
			public class INTERNAL
			{
				// Token: 0x0400BC06 RID: 48134
				public float IDEAL = 310.15f;

				// Token: 0x0400BC07 RID: 48135
				public float THRESHOLD_HYPOTHERMIA = 308.15f;

				// Token: 0x0400BC08 RID: 48136
				public float THRESHOLD_HYPERTHERMIA = 312.15f;

				// Token: 0x0400BC09 RID: 48137
				public float THRESHOLD_FATAL_HOT = 320.15f;

				// Token: 0x0400BC0A RID: 48138
				public float THRESHOLD_FATAL_COLD = 300.15f;
			}

			// Token: 0x02002AA2 RID: 10914
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x0400BC0B RID: 48139
				public float SKINNY = -0.005f;

				// Token: 0x0400BC0C RID: 48140
				public float PUDGY = 0.005f;
			}
		}

		// Token: 0x02002211 RID: 8721
		public class BREATH
		{
			// Token: 0x17000D57 RID: 3415
			// (get) Token: 0x0600BEAB RID: 48811 RVA: 0x00408277 File Offset: 0x00406477
			public float RETREAT_AMOUNT
			{
				get
				{
					return this.RETREAT_AT_SECONDS / this.BREATH_BAR_TOTAL_SECONDS * this.BREATH_BAR_TOTAL_AMOUNT;
				}
			}

			// Token: 0x17000D58 RID: 3416
			// (get) Token: 0x0600BEAC RID: 48812 RVA: 0x0040828D File Offset: 0x0040648D
			public float SUFFOCATE_AMOUNT
			{
				get
				{
					return this.SUFFOCATION_WARN_AT_SECONDS / this.BREATH_BAR_TOTAL_SECONDS * this.BREATH_BAR_TOTAL_AMOUNT;
				}
			}

			// Token: 0x17000D59 RID: 3417
			// (get) Token: 0x0600BEAD RID: 48813 RVA: 0x004082A3 File Offset: 0x004064A3
			public float BREATH_RATE
			{
				get
				{
					return this.BREATH_BAR_TOTAL_AMOUNT / this.BREATH_BAR_TOTAL_SECONDS;
				}
			}

			// Token: 0x04009CEE RID: 40174
			private float BREATH_BAR_TOTAL_SECONDS = 110f;

			// Token: 0x04009CEF RID: 40175
			private float RETREAT_AT_SECONDS = 80f;

			// Token: 0x04009CF0 RID: 40176
			private float SUFFOCATION_WARN_AT_SECONDS = 50f;

			// Token: 0x04009CF1 RID: 40177
			public float BREATH_BAR_TOTAL_AMOUNT = 100f;
		}

		// Token: 0x02002212 RID: 8722
		public class LIGHT
		{
			// Token: 0x04009CF2 RID: 40178
			public int LUX_SUNBURN = 72000;

			// Token: 0x04009CF3 RID: 40179
			public float SUNBURN_DELAY_TIME = 120f;

			// Token: 0x04009CF4 RID: 40180
			public int LUX_PLEASANT_LIGHT = 40000;

			// Token: 0x04009CF5 RID: 40181
			public float LIGHT_WORK_EFFICIENCY_BONUS = 0.15f;

			// Token: 0x04009CF6 RID: 40182
			public int NO_LIGHT;

			// Token: 0x04009CF7 RID: 40183
			public int VERY_LOW_LIGHT = 1;

			// Token: 0x04009CF8 RID: 40184
			public int LOW_LIGHT = 500;

			// Token: 0x04009CF9 RID: 40185
			public int MEDIUM_LIGHT = 1000;

			// Token: 0x04009CFA RID: 40186
			public int HIGH_LIGHT = 10000;

			// Token: 0x04009CFB RID: 40187
			public int VERY_HIGH_LIGHT = 50000;

			// Token: 0x04009CFC RID: 40188
			public int MAX_LIGHT = 100000;
		}

		// Token: 0x02002213 RID: 8723
		public class COMBAT
		{
			// Token: 0x04009CFD RID: 40189
			public DUPLICANTSTATS.COMBAT.BASICWEAPON BasicWeapon = new DUPLICANTSTATS.COMBAT.BASICWEAPON();

			// Token: 0x04009CFE RID: 40190
			public Health.HealthState FLEE_THRESHOLD = Health.HealthState.Critical;

			// Token: 0x02002AA3 RID: 10915
			public class BASICWEAPON
			{
				// Token: 0x0400BC0D RID: 48141
				public float ATTACKS_PER_SECOND = 2f;

				// Token: 0x0400BC0E RID: 48142
				public float MIN_DAMAGE_PER_HIT = 1f;

				// Token: 0x0400BC0F RID: 48143
				public float MAX_DAMAGE_PER_HIT = 1f;

				// Token: 0x0400BC10 RID: 48144
				public AttackProperties.TargetType TARGET_TYPE;

				// Token: 0x0400BC11 RID: 48145
				public AttackProperties.DamageType DAMAGE_TYPE;

				// Token: 0x0400BC12 RID: 48146
				public int MAX_HITS = 1;

				// Token: 0x0400BC13 RID: 48147
				public float AREA_OF_EFFECT_RADIUS;
			}
		}

		// Token: 0x02002214 RID: 8724
		public class SECRETIONS
		{
			// Token: 0x04009CFF RID: 40191
			public float PEE_FUSE_TIME = 120f;

			// Token: 0x04009D00 RID: 40192
			public float PEE_PER_FLOOR_PEE = 2f;

			// Token: 0x04009D01 RID: 40193
			public float PEE_PER_TOILET_PEE = 6.7f;

			// Token: 0x04009D02 RID: 40194
			public string PEE_DISEASE = "FoodPoisoning";

			// Token: 0x04009D03 RID: 40195
			public int DISEASE_PER_PEE = 100000;

			// Token: 0x04009D04 RID: 40196
			public int DISEASE_PER_VOMIT = 100000;
		}
	}
}
