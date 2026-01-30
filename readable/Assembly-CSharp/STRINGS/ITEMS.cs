using System;

namespace STRINGS
{
	// Token: 0x02000FFA RID: 4090
	public class ITEMS
	{
		// Token: 0x0200258D RID: 9613
		public class PILLS
		{
			// Token: 0x020038B4 RID: 14516
			public class PLACEBO
			{
				// Token: 0x0400E6A0 RID: 59040
				public static LocString NAME = "Placebo";

				// Token: 0x0400E6A1 RID: 59041
				public static LocString DESC = "A general, all-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".\n\nThe less one knows about it, the better it works.";

				// Token: 0x0400E6A2 RID: 59042
				public static LocString RECIPEDESC = "All-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".";
			}

			// Token: 0x020038B5 RID: 14517
			public class BASICBOOSTER
			{
				// Token: 0x0400E6A3 RID: 59043
				public static LocString NAME = UI.FormatAsLink("Vitamin Chews", "BASICBOOSTER");

				// Token: 0x0400E6A4 RID: 59044
				public static LocString DESC = "Minorly reduces the chance of becoming sick.";

				// Token: 0x0400E6A5 RID: 59045
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that minorly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x020038B6 RID: 14518
			public class INTERMEDIATEBOOSTER
			{
				// Token: 0x0400E6A6 RID: 59046
				public static LocString NAME = UI.FormatAsLink("Immuno Booster", "INTERMEDIATEBOOSTER");

				// Token: 0x0400E6A7 RID: 59047
				public static LocString DESC = "Significantly reduces the chance of becoming sick.";

				// Token: 0x0400E6A8 RID: 59048
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that significantly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x020038B7 RID: 14519
			public class ANTIHISTAMINE
			{
				// Token: 0x0400E6A9 RID: 59049
				public static LocString NAME = UI.FormatAsLink("Allergy Medication", "ANTIHISTAMINE");

				// Token: 0x0400E6AA RID: 59050
				public static LocString DESC = "Suppresses and prevents allergic reactions.";

				// Token: 0x0400E6AB RID: 59051
				public static LocString RECIPEDESC = "A strong antihistamine Duplicants can take to halt an allergic reaction. " + ITEMS.PILLS.ANTIHISTAMINE.NAME + " will also prevent further reactions from occurring for a short time after ingestion.";
			}

			// Token: 0x020038B8 RID: 14520
			public class BASICCURE
			{
				// Token: 0x0400E6AC RID: 59052
				public static LocString NAME = UI.FormatAsLink("Curative Tablet", "BASICCURE");

				// Token: 0x0400E6AD RID: 59053
				public static LocString DESC = "A simple, easy-to-take remedy for minor germ-based diseases.";

				// Token: 0x0400E6AE RID: 59054
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Duplicants can take this to cure themselves of minor ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nCurative Tablets are very effective against ",
					DUPLICANTS.DISEASES.FOODPOISONING.NAME,
					"."
				});
			}

			// Token: 0x020038B9 RID: 14521
			public class INTERMEDIATECURE
			{
				// Token: 0x0400E6AF RID: 59055
				public static LocString NAME = UI.FormatAsLink("Medical Pack", "INTERMEDIATECURE");

				// Token: 0x0400E6B0 RID: 59056
				public static LocString DESC = "A doctor-administered cure for moderate ailments.";

				// Token: 0x0400E6B1 RID: 59057
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A doctor-administered cure for moderate ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.INTERMEDIATECURE.NAME,
					"s are very effective against ",
					UI.FormatAsLink("Slimelung", "SLIMESICKNESS"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x020038BA RID: 14522
			public class ADVANCEDCURE
			{
				// Token: 0x0400E6B2 RID: 59058
				public static LocString NAME = UI.FormatAsLink("Serum Vial", "ADVANCEDCURE");

				// Token: 0x0400E6B3 RID: 59059
				public static LocString DESC = "A doctor-administered cure for severe ailments.";

				// Token: 0x0400E6B4 RID: 59060
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"An extremely powerful medication created to treat severe ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.ADVANCEDCURE.NAME,
					" is very effective against ",
					UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.SENIOR_MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x020038BB RID: 14523
			public class BASICRADPILL
			{
				// Token: 0x0400E6B5 RID: 59061
				public static LocString NAME = UI.FormatAsLink("Basic Rad Pill", "BASICRADPILL");

				// Token: 0x0400E6B6 RID: 59062
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400E6B7 RID: 59063
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}

			// Token: 0x020038BC RID: 14524
			public class INTERMEDIATERADPILL
			{
				// Token: 0x0400E6B8 RID: 59064
				public static LocString NAME = UI.FormatAsLink("Intermediate Rad Pill", "INTERMEDIATERADPILL");

				// Token: 0x0400E6B9 RID: 59065
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400E6BA RID: 59066
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}
		}

		// Token: 0x0200258E RID: 9614
		public class LUBRICATIONSTICK
		{
			// Token: 0x0400A9B6 RID: 43446
			public static LocString NAME = UI.FormatAsLink("Gear Balm", "LUBRICATIONSTICK");

			// Token: 0x0400A9B7 RID: 43447
			public static LocString SUBHEADER = "Mechanical Lubricant";

			// Token: 0x0400A9B8 RID: 43448
			public static LocString DESC = string.Concat(new string[]
			{
				"Provides a small amount of lubricating ",
				UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
				".\n\nCan be produced at the ",
				BUILDINGS.PREFABS.APOTHECARY.NAME,
				"."
			});

			// Token: 0x0400A9B9 RID: 43449
			public static LocString RECIPEDESC = "A self-administered mechanical lubricant for Duplicants with bionic parts.";
		}

		// Token: 0x0200258F RID: 9615
		public class TALLOWLUBRICATIONSTICK
		{
			// Token: 0x0400A9BA RID: 43450
			public static LocString NAME = UI.FormatAsLink("Tallow Gear Balm", "TALLOWLUBRICATIONSTICK");

			// Token: 0x0400A9BB RID: 43451
			public static LocString SUBHEADER = "Mechanical Lubricant";

			// Token: 0x0400A9BC RID: 43452
			public static LocString DESC = string.Concat(new string[]
			{
				"Provides a small amount of extra-silky lubricating ",
				UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
				".\n\nCan be produced at the ",
				BUILDINGS.PREFABS.APOTHECARY.NAME,
				"."
			});

			// Token: 0x0400A9BD RID: 43453
			public static LocString RECIPEDESC = "An advanced self-administered mechanical lubricant for Duplicants with bionic parts.";
		}

		// Token: 0x02002590 RID: 9616
		public class BIONIC_BOOSTERS
		{
			// Token: 0x0400A9BE RID: 43454
			public static LocString FABRICATION_SOURCE = "This booster can be manufactured at the {0}.";

			// Token: 0x020038BD RID: 14525
			public class BOOSTER_DIG1
			{
				// Token: 0x0400E6BB RID: 59067
				public static LocString NAME = UI.FormatAsLink("Digging Booster", "BOOSTER_DIG1");

				// Token: 0x0400E6BC RID: 59068
				public static LocString DESC = "Grants a Bionic Duplicant the skill required to dig hard things.";
			}

			// Token: 0x020038BE RID: 14526
			public class BOOSTER_DIG2
			{
				// Token: 0x0400E6BD RID: 59069
				public static LocString NAME = UI.FormatAsLink("Extreme Digging Booster", "BOOSTER_DIG2");

				// Token: 0x0400E6BE RID: 59070
				public static LocString DESC = "Grants a Bionic Duplicant the digging skill required to get through anything.";
			}

			// Token: 0x020038BF RID: 14527
			public class BOOSTER_CONSTRUCT1
			{
				// Token: 0x0400E6BF RID: 59071
				public static LocString NAME = UI.FormatAsLink("Construction Booster", "BOOSTER_CONSTRUCT1");

				// Token: 0x0400E6C0 RID: 59072
				public static LocString DESC = "Grants a Bionic Duplicant the ability to build fast, and demolish buildings that others cannot.";
			}

			// Token: 0x020038C0 RID: 14528
			public class BOOSTER_FARM1
			{
				// Token: 0x0400E6C1 RID: 59073
				public static LocString NAME = UI.FormatAsLink("Crop Tending Booster", "BOOSTER_FARM1");

				// Token: 0x0400E6C2 RID: 59074
				public static LocString DESC = "Grants a Bionic Duplicant unparalleled farming and botanical analysis skills.";
			}

			// Token: 0x020038C1 RID: 14529
			public class BOOSTER_RANCH1
			{
				// Token: 0x0400E6C3 RID: 59075
				public static LocString NAME = UI.FormatAsLink("Ranching Booster", "BOOSTER_RANCH1");

				// Token: 0x0400E6C4 RID: 59076
				public static LocString DESC = "Grants a Bionic Duplicant the skills required to care for " + UI.FormatAsLink("Critters", "CREATURES") + " in every way.";
			}

			// Token: 0x020038C2 RID: 14530
			public class BOOSTER_COOK1
			{
				// Token: 0x0400E6C5 RID: 59077
				public static LocString NAME = UI.FormatAsLink("Grilling Booster", "BOOSTER_COOK1");

				// Token: 0x0400E6C6 RID: 59078
				public static LocString DESC = "Grants a Bionic Duplicant deliciously professional culinary skills.";
			}

			// Token: 0x020038C3 RID: 14531
			public class BOOSTER_ART1
			{
				// Token: 0x0400E6C7 RID: 59079
				public static LocString NAME = UI.FormatAsLink("Masterworks Art Booster", "BOOSTER_ART1");

				// Token: 0x0400E6C8 RID: 59080
				public static LocString DESC = "Grants a Bionic Duplicant flawless decorating skills.";
			}

			// Token: 0x020038C4 RID: 14532
			public class BOOSTER_RESEARCH1
			{
				// Token: 0x0400E6C9 RID: 59081
				public static LocString NAME = UI.FormatAsLink("Researching Booster", "BOOSTER_RESEARCH1");

				// Token: 0x0400E6CA RID: 59082
				public static LocString DESC = "Grants a Bionic Duplicant the expertise required to study " + UI.FormatAsLink("geysers", "GEYSERS") + " and other advanced topics.";
			}

			// Token: 0x020038C5 RID: 14533
			public class BOOSTER_RESEARCH2
			{
				// Token: 0x0400E6CB RID: 59083
				public static LocString NAME = UI.FormatAsLink("Astronomy Booster", "BOOSTER_RESEARCH2");

				// Token: 0x0400E6CC RID: 59084
				public static LocString DESC = "Grants a Bionic Duplicant a keen grasp of science and usage of space-research buildings.";
			}

			// Token: 0x020038C6 RID: 14534
			public class BOOSTER_RESEARCH3
			{
				// Token: 0x0400E6CD RID: 59085
				public static LocString NAME = UI.FormatAsLink("Applied Sciences Booster", "BOOSTER_RESEARCH3");

				// Token: 0x0400E6CE RID: 59086
				public static LocString DESC = "Grants a Bionic Duplicant a deeply pragmatic approach to scientific research.";
			}

			// Token: 0x020038C7 RID: 14535
			public class BOOSTER_PILOT1
			{
				// Token: 0x0400E6CF RID: 59087
				public static LocString NAME = UI.FormatAsLink("Piloting Booster", "BOOSTER_PILOT1");

				// Token: 0x0400E6D0 RID: 59088
				public static LocString DESC = "Grants a Bionic Duplicant the expertise required to explore the skies in person.";
			}

			// Token: 0x020038C8 RID: 14536
			public class BOOSTER_PILOTVANILLA1
			{
				// Token: 0x0400E6D1 RID: 59089
				public static LocString NAME = UI.FormatAsLink("Rocketry Booster", "BOOSTER_PILOTVANILLA1");

				// Token: 0x0400E6D2 RID: 59090
				public static LocString DESC = "Grants a Bionic Duplicant the expertise required to command a rocket.";
			}

			// Token: 0x020038C9 RID: 14537
			public class BOOSTER_SUITS1
			{
				// Token: 0x0400E6D3 RID: 59091
				public static LocString NAME = UI.FormatAsLink("Suit Training Booster", "BOOSTER_SUITS1");

				// Token: 0x0400E6D4 RID: 59092
				public static LocString DESC = "Enables a Bionic Duplicant to maximize durability of equipped " + UI.FormatAsLink("Exosuits", "EQUIPMENT") + " and maintain their runspeed.";
			}

			// Token: 0x020038CA RID: 14538
			public class BOOSTER_CARRY1
			{
				// Token: 0x0400E6D5 RID: 59093
				public static LocString NAME = UI.FormatAsLink("Strength Booster", "BOOSTER_CARRY1");

				// Token: 0x0400E6D6 RID: 59094
				public static LocString DESC = "Grants a Bionic Duplicant increased carrying capacity and athletic prowess.";
			}

			// Token: 0x020038CB RID: 14539
			public class BOOSTER_OP1
			{
				// Token: 0x0400E6D7 RID: 59095
				public static LocString NAME = UI.FormatAsLink("Electrical Engineering Booster", "BOOSTER_OP1");

				// Token: 0x0400E6D8 RID: 59096
				public static LocString DESC = "Grants a Bionic Duplicant the skills requried to tinker and solder to their heart's content.";
			}

			// Token: 0x020038CC RID: 14540
			public class BOOSTER_OP2
			{
				// Token: 0x0400E6D9 RID: 59097
				public static LocString NAME = UI.FormatAsLink("Mechatronics Engineering Booster", "BOOSTER_OP2");

				// Token: 0x0400E6DA RID: 59098
				public static LocString DESC = "Grants a Bionic Duplicant complete mastery of engineering skills.";
			}

			// Token: 0x020038CD RID: 14541
			public class BOOSTER_MEDICINE1
			{
				// Token: 0x0400E6DB RID: 59099
				public static LocString NAME = UI.FormatAsLink("Advanced Medical Booster", "BOOSTER_MEDICINE1");

				// Token: 0x0400E6DC RID: 59100
				public static LocString DESC = "Grants a Bionic Duplicant the ability to perform all doctoring errands.";
			}

			// Token: 0x020038CE RID: 14542
			public class BOOSTER_TIDY1
			{
				// Token: 0x0400E6DD RID: 59101
				public static LocString NAME = UI.FormatAsLink("Tidying Booster", "BOOSTER_TIDY1");

				// Token: 0x0400E6DE RID: 59102
				public static LocString DESC = "Grants a Bionic Duplicant the full range of tidying skills, including blasting unwanted meteors out of the sky.";
			}
		}

		// Token: 0x02002591 RID: 9617
		public class FOOD
		{
			// Token: 0x0400A9BF RID: 43455
			public static LocString COMPOST = "Compost";

			// Token: 0x020038CF RID: 14543
			public class FOODSPLAT
			{
				// Token: 0x0400E6DF RID: 59103
				public static LocString NAME = "Food Splatter";

				// Token: 0x0400E6E0 RID: 59104
				public static LocString DESC = "Food smeared on the wall from a recent Food Fight";
			}

			// Token: 0x020038D0 RID: 14544
			public class BURGER
			{
				// Token: 0x0400E6E1 RID: 59105
				public static LocString NAME = UI.FormatAsLink("Frost Burger", "BURGER");

				// Token: 0x0400E6E2 RID: 59106
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					".\n\nIt's the only burger best served cold."
				});

				// Token: 0x0400E6E3 RID: 59107
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					"."
				});

				// Token: 0x02003D99 RID: 15769
				public class DEHYDRATED
				{
					// Token: 0x0400F31D RID: 62237
					public static LocString NAME = UI.FormatAsLink("Dried Frost Burger", "BURGER");

					// Token: 0x0400F31E RID: 62238
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Frost Burger", "BURGER"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020038D1 RID: 14545
			public class FIELDRATION
			{
				// Token: 0x0400E6E4 RID: 59108
				public static LocString NAME = UI.FormatAsLink("Nutrient Bar", "FIELDRATION");

				// Token: 0x0400E6E5 RID: 59109
				public static LocString DESC = "A nourishing nutrient paste, sandwiched between thin wafer layers.";
			}

			// Token: 0x020038D2 RID: 14546
			public class MUSHBAR
			{
				// Token: 0x0400E6E6 RID: 59110
				public static LocString NAME = UI.FormatAsLink("Mush Bar", "MUSHBAR");

				// Token: 0x0400E6E7 RID: 59111
				public static LocString DESC = "An edible, putrefied mudslop.\n\nMush Bars are preferable to starvation, but only just barely.";

				// Token: 0x0400E6E8 RID: 59112
				public static LocString RECIPEDESC = "An edible, putrefied mudslop.\n\n" + ITEMS.FOOD.MUSHBAR.NAME + "s are preferable to starvation, but only just barely.";
			}

			// Token: 0x020038D3 RID: 14547
			public class MUSHROOMWRAP
			{
				// Token: 0x0400E6E9 RID: 59113
				public static LocString NAME = UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP");

				// Token: 0x0400E6EA RID: 59114
				public static LocString DESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nIt has an earthy flavor punctuated by a refreshing crunch."
				});

				// Token: 0x0400E6EB RID: 59115
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});

				// Token: 0x02003D9A RID: 15770
				public class DEHYDRATED
				{
					// Token: 0x0400F31F RID: 62239
					public static LocString NAME = UI.FormatAsLink("Dried Mushroom Wrap", "MUSHROOMWRAP");

					// Token: 0x0400F320 RID: 62240
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020038D4 RID: 14548
			public class MICROWAVEDLETTUCE
			{
				// Token: 0x0400E6EC RID: 59116
				public static LocString NAME = UI.FormatAsLink("Microwaved Lettuce", "MICROWAVEDLETTUCE");

				// Token: 0x0400E6ED RID: 59117
				public static LocString DESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";

				// Token: 0x0400E6EE RID: 59118
				public static LocString RECIPEDESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x020038D5 RID: 14549
			public class GAMMAMUSH
			{
				// Token: 0x0400E6EF RID: 59119
				public static LocString NAME = UI.FormatAsLink("Gamma Mush", "GAMMAMUSH");

				// Token: 0x0400E6F0 RID: 59120
				public static LocString DESC = "A disturbingly delicious mixture of irradiated dirt and water.";

				// Token: 0x0400E6F1 RID: 59121
				public static LocString RECIPEDESC = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR") + " reheated in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x020038D6 RID: 14550
			public class FRUITCAKE
			{
				// Token: 0x0400E6F2 RID: 59122
				public static LocString NAME = UI.FormatAsLink("Berry Sludge", "FRUITCAKE");

				// Token: 0x0400E6F3 RID: 59123
				public static LocString DESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.\n\nIts aggressive, overbearing sweetness can leave the tongue feeling temporarily numb.";

				// Token: 0x0400E6F4 RID: 59124
				public static LocString RECIPEDESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.";
			}

			// Token: 0x020038D7 RID: 14551
			public class POPCORN
			{
				// Token: 0x0400E6F5 RID: 59125
				public static LocString NAME = UI.FormatAsLink("Popcorn", "POPCORN");

				// Token: 0x0400E6F6 RID: 59126
				public static LocString DESC = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " popped in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".\n\nCompletely devoid of any fancy flavorings.";

				// Token: 0x0400E6F7 RID: 59127
				public static LocString RECIPEDESC = "Gamma-radiated " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + ".";
			}

			// Token: 0x020038D8 RID: 14552
			public class SUSHI
			{
				// Token: 0x0400E6F8 RID: 59128
				public static LocString NAME = UI.FormatAsLink("Sushi", "SUSHI");

				// Token: 0x0400E6F9 RID: 59129
				public static LocString DESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nWhile the salt of the lettuce may initially overpower the flavor, a keen palate can discern the subtle sweetness of the fillet beneath."
				});

				// Token: 0x0400E6FA RID: 59130
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});
			}

			// Token: 0x020038D9 RID: 14553
			public class HATCHEGG
			{
				// Token: 0x0400E6FB RID: 59131
				public static LocString NAME = CREATURES.SPECIES.HATCH.EGG_NAME;

				// Token: 0x0400E6FC RID: 59132
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Hatch", "HATCH"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Hatchling", "HATCH"),
					"."
				});

				// Token: 0x0400E6FD RID: 59133
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Hatch", "HATCH") + ".";
			}

			// Token: 0x020038DA RID: 14554
			public class DRECKOEGG
			{
				// Token: 0x0400E6FE RID: 59134
				public static LocString NAME = CREATURES.SPECIES.DRECKO.EGG_NAME;

				// Token: 0x0400E6FF RID: 59135
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Drecko", "DRECKO"),
					".\n\nIf incubated, it will hatch into a new ",
					UI.FormatAsLink("Drecklet", "DRECKO"),
					"."
				});

				// Token: 0x0400E700 RID: 59136
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Drecko", "DRECKO") + ".";
			}

			// Token: 0x020038DB RID: 14555
			public class LIGHTBUGEGG
			{
				// Token: 0x0400E701 RID: 59137
				public static LocString NAME = CREATURES.SPECIES.LIGHTBUG.EGG_NAME;

				// Token: 0x0400E702 RID: 59138
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Shine Bug", "LIGHTBUG"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Shine Nymph", "LIGHTBUG"),
					"."
				});

				// Token: 0x0400E703 RID: 59139
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Shine Bug", "LIGHTBUG") + ".";
			}

			// Token: 0x020038DC RID: 14556
			public class LETTUCE
			{
				// Token: 0x0400E704 RID: 59140
				public static LocString NAME = UI.FormatAsLink("Lettuce", "LETTUCE");

				// Token: 0x0400E705 RID: 59141
				public static LocString DESC = "Crunchy, slightly salty leaves from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + " plant.";

				// Token: 0x0400E706 RID: 59142
				public static LocString RECIPEDESC = "Edible roughage from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + ".";
			}

			// Token: 0x020038DD RID: 14557
			public class PASTA
			{
				// Token: 0x0400E707 RID: 59143
				public static LocString NAME = UI.FormatAsLink("Pasta", "PASTA");

				// Token: 0x0400E708 RID: 59144
				public static LocString DESC = "pasta made from egg and wheat";

				// Token: 0x0400E709 RID: 59145
				public static LocString RECIPEDESC = "pasta made from egg and wheat";
			}

			// Token: 0x020038DE RID: 14558
			public class PANCAKES
			{
				// Token: 0x0400E70A RID: 59146
				public static LocString NAME = UI.FormatAsLink("Soufflé Pancakes", "PANCAKES");

				// Token: 0x0400E70B RID: 59147
				public static LocString DESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					ITEMS.FOOD.COLDWHEATSEED.NAME,
					".\n\nThey're so thick!"
				});

				// Token: 0x0400E70C RID: 59148
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					ITEMS.FOOD.COLDWHEATSEED.NAME,
					"."
				});
			}

			// Token: 0x020038DF RID: 14559
			public class OILFLOATEREGG
			{
				// Token: 0x0400E70D RID: 59149
				public static LocString NAME = CREATURES.SPECIES.OILFLOATER.EGG_NAME;

				// Token: 0x0400E70E RID: 59150
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Slickster", "OILFLOATER"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Slickster Larva", "OILFLOATER"),
					"."
				});

				// Token: 0x0400E70F RID: 59151
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Slickster", "OILFLOATER") + ".";
			}

			// Token: 0x020038E0 RID: 14560
			public class PUFTEGG
			{
				// Token: 0x0400E710 RID: 59152
				public static LocString NAME = CREATURES.SPECIES.PUFT.EGG_NAME;

				// Token: 0x0400E711 RID: 59153
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Puft", "PUFT"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Puftlet", "PUFT"),
					"."
				});

				// Token: 0x0400E712 RID: 59154
				public static LocString RECIPEDESC = "An egg laid by a " + CREATURES.SPECIES.PUFT.NAME + ".";
			}

			// Token: 0x020038E1 RID: 14561
			public class PREHISTORICPACUFILLET
			{
				// Token: 0x0400E713 RID: 59155
				public static LocString NAME = UI.FormatAsLink("Jawbo Fillet", "PREHISTORICPACUFILLET");

				// Token: 0x0400E714 RID: 59156
				public static LocString DESC = "An uncooked fillet from a very dead " + CREATURES.SPECIES.PREHISTORICPACU.NAME + ". It has a silky texture.";
			}

			// Token: 0x020038E2 RID: 14562
			public class FISHMEAT
			{
				// Token: 0x0400E715 RID: 59157
				public static LocString NAME = UI.FormatAsLink("Pacu Fillet", "FISHMEAT");

				// Token: 0x0400E716 RID: 59158
				public static LocString DESC = "An uncooked fillet from a very dead " + CREATURES.SPECIES.PACU.NAME + ". Yum!";
			}

			// Token: 0x020038E3 RID: 14563
			public class MEAT
			{
				// Token: 0x0400E717 RID: 59159
				public static LocString NAME = UI.FormatAsLink("Meat", "MEAT");

				// Token: 0x0400E718 RID: 59160
				public static LocString DESC = "Uncooked meat from a very dead critter. Yum!";
			}

			// Token: 0x020038E4 RID: 14564
			public class DINOSAURMEAT
			{
				// Token: 0x0400E719 RID: 59161
				public static LocString NAME = UI.FormatAsLink("Tough Meat", "DINOSAURMEAT");

				// Token: 0x0400E71A RID: 59162
				public static LocString DESC = "Uncooked meat from a very dead critter.\n\nIt's inedible until cooked in the " + BUILDINGS.PREFABS.SMOKER.NAME + ".";
			}

			// Token: 0x020038E5 RID: 14565
			public class SMOKEDDINOSAURMEAT
			{
				// Token: 0x0400E71B RID: 59163
				public static LocString NAME = UI.FormatAsLink("Tender Brisket", "SMOKEDDINOSAURMEAT");

				// Token: 0x0400E71C RID: 59164
				public static LocString DESC = "A cooked stack of tough meat that's been marinated and slow-smoked to tender perfection.";

				// Token: 0x0400E71D RID: 59165
				public static LocString RECIPEDESC = "A stack of tender, slow-smoked meat.";
			}

			// Token: 0x020038E6 RID: 14566
			public class SMOKEDFISH
			{
				// Token: 0x0400E71E RID: 59166
				public static LocString NAME = UI.FormatAsLink("Smoked Fish", "SMOKEDFISH");

				// Token: 0x0400E71F RID: 59167
				public static LocString DESC = "A buttery smoked fish fillet.\n\nIt flakes nicely when pulled apart with a fork.";

				// Token: 0x0400E720 RID: 59168
				public static LocString RECIPEDESC = "A buttery smoked fish fillet.";
			}

			// Token: 0x020038E7 RID: 14567
			public class SMOKEDVEGETABLES
			{
				// Token: 0x0400E721 RID: 59169
				public static LocString NAME = UI.FormatAsLink("Veggie Poppers", "SMOKEDVEGETABLES");

				// Token: 0x0400E722 RID: 59170
				public static LocString DESC = "Crisp vegetables stuffed with herbs and smoked for hours.";

				// Token: 0x0400E723 RID: 59171
				public static LocString RECIPEDESC = "Crisp vegetables stuffed with herbs.";
			}

			// Token: 0x020038E8 RID: 14568
			public class PLANTMEAT
			{
				// Token: 0x0400E724 RID: 59172
				public static LocString NAME = UI.FormatAsLink("Plant Meat", "PLANTMEAT");

				// Token: 0x0400E725 RID: 59173
				public static LocString DESC = "Planty plant meat from a plant. How nice!";
			}

			// Token: 0x020038E9 RID: 14569
			public class SHELLFISHMEAT
			{
				// Token: 0x0400E726 RID: 59174
				public static LocString NAME = UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT");

				// Token: 0x0400E727 RID: 59175
				public static LocString DESC = "An uncooked chunk of very dead " + CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME + ". Yum!";
			}

			// Token: 0x020038EA RID: 14570
			public class MUSHROOM
			{
				// Token: 0x0400E728 RID: 59176
				public static LocString NAME = UI.FormatAsLink("Mushroom", "MUSHROOM");

				// Token: 0x0400E729 RID: 59177
				public static LocString DESC = "An edible, flavorless fungus that grew in the dark.";
			}

			// Token: 0x020038EB RID: 14571
			public class COOKEDFISH
			{
				// Token: 0x0400E72A RID: 59178
				public static LocString NAME = UI.FormatAsLink("Cooked Seafood", "COOKEDFISH");

				// Token: 0x0400E72B RID: 59179
				public static LocString DESC = "A cooked piece of freshly caught aquatic critter.\n\nUnsurprisingly, it tastes a bit fishy.";

				// Token: 0x0400E72C RID: 59180
				public static LocString RECIPEDESC = "A cooked piece of freshly caught aquatic critter.";
			}

			// Token: 0x020038EC RID: 14572
			public class COOKEDMEAT
			{
				// Token: 0x0400E72D RID: 59181
				public static LocString NAME = UI.FormatAsLink("Barbeque", "COOKEDMEAT");

				// Token: 0x0400E72E RID: 59182
				public static LocString DESC = "The cooked meat of a defeated critter.\n\nIt has a delightful smoky aftertaste.";

				// Token: 0x0400E72F RID: 59183
				public static LocString RECIPEDESC = "The cooked meat of a defeated critter.";
			}

			// Token: 0x020038ED RID: 14573
			public class FRIESCARROT
			{
				// Token: 0x0400E730 RID: 59184
				public static LocString NAME = UI.FormatAsLink("Squash Fries", "FRIESCARROT");

				// Token: 0x0400E731 RID: 59185
				public static LocString DESC = "Irresistibly crunchy.\n\nBest eaten hot.";

				// Token: 0x0400E732 RID: 59186
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Crunchy sticks of ",
					UI.FormatAsLink("Plume Squash", "CARROT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020038EE RID: 14574
			public class DEEPFRIEDFISH
			{
				// Token: 0x0400E733 RID: 59187
				public static LocString NAME = UI.FormatAsLink("Fish Taco", "DEEPFRIEDFISH");

				// Token: 0x0400E734 RID: 59188
				public static LocString DESC = "Deep-fried fish cradled in a crunchy fin.";

				// Token: 0x0400E735 RID: 59189
				public static LocString RECIPEDESC = UI.FormatAsLink("Pacu Fillet", "FISHMEAT") + " lightly battered and deep-fried in " + UI.FormatAsLink("Tallow", "TALLOW") + ".";
			}

			// Token: 0x020038EF RID: 14575
			public class DEEPFRIEDSHELLFISH
			{
				// Token: 0x0400E736 RID: 59190
				public static LocString NAME = UI.FormatAsLink("Shellfish Tempura", "DEEPFRIEDSHELLFISH");

				// Token: 0x0400E737 RID: 59191
				public static LocString DESC = "A crispy deep-fried critter claw.";

				// Token: 0x0400E738 RID: 59192
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A tender chunk of battered ",
					UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020038F0 RID: 14576
			public class DEEPFRIEDMEAT
			{
				// Token: 0x0400E739 RID: 59193
				public static LocString NAME = UI.FormatAsLink("Deep Fried Steak", "DEEPFRIEDMEAT");

				// Token: 0x0400E73A RID: 59194
				public static LocString DESC = "A juicy slab of meat with a crunchy deep-fried upper layer.";

				// Token: 0x0400E73B RID: 59195
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A juicy slab of ",
					UI.FormatAsLink("Raw Meat", "MEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020038F1 RID: 14577
			public class DEEPFRIEDNOSH
			{
				// Token: 0x0400E73C RID: 59196
				public static LocString NAME = UI.FormatAsLink("Nosh Noms", "DEEPFRIEDNOSH");

				// Token: 0x0400E73D RID: 59197
				public static LocString DESC = "A snackable handful of crunchy beans.";

				// Token: 0x0400E73E RID: 59198
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A crunchy stack of ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020038F2 RID: 14578
			public class PICKLEDMEAL
			{
				// Token: 0x0400E73F RID: 59199
				public static LocString NAME = UI.FormatAsLink("Pickled Meal", "PICKLEDMEAL");

				// Token: 0x0400E740 RID: 59200
				public static LocString DESC = "Meal Lice preserved in vinegar.\n\nIt's a rarely acquired taste.";

				// Token: 0x0400E741 RID: 59201
				public static LocString RECIPEDESC = ITEMS.FOOD.BASICPLANTFOOD.NAME + " regrettably preserved in vinegar.";
			}

			// Token: 0x020038F3 RID: 14579
			public class FRIEDMUSHBAR
			{
				// Token: 0x0400E742 RID: 59202
				public static LocString NAME = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR");

				// Token: 0x0400E743 RID: 59203
				public static LocString DESC = "Pan-fried, solidified mudslop.\n\nThe inside is almost completely uncooked, despite the crunch on the outside.";

				// Token: 0x0400E744 RID: 59204
				public static LocString RECIPEDESC = "Pan-fried, solidified mudslop.";
			}

			// Token: 0x020038F4 RID: 14580
			public class RAWEGG
			{
				// Token: 0x0400E745 RID: 59205
				public static LocString NAME = UI.FormatAsLink("Raw Egg", "RAWEGG");

				// Token: 0x0400E746 RID: 59206
				public static LocString DESC = "A raw Egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.\n\nIt will never hatch.";

				// Token: 0x0400E747 RID: 59207
				public static LocString RECIPEDESC = "A raw egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.";
			}

			// Token: 0x020038F5 RID: 14581
			public class COOKEDEGG
			{
				// Token: 0x0400E748 RID: 59208
				public static LocString NAME = UI.FormatAsLink("Omelette", "COOKEDEGG");

				// Token: 0x0400E749 RID: 59209
				public static LocString DESC = "Fluffed and folded Egg innards.\n\nIt turns out you do, in fact, have to break a few eggs to make it.";

				// Token: 0x0400E74A RID: 59210
				public static LocString RECIPEDESC = "Fluffed and folded egg innards.";
			}

			// Token: 0x020038F6 RID: 14582
			public class FRIEDMUSHROOM
			{
				// Token: 0x0400E74B RID: 59211
				public static LocString NAME = UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM");

				// Token: 0x0400E74C RID: 59212
				public static LocString DESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".\n\nIt has a thick, savory flavor with subtle earthy undertones.";

				// Token: 0x0400E74D RID: 59213
				public static LocString RECIPEDESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".";
			}

			// Token: 0x020038F7 RID: 14583
			public class COOKEDPIKEAPPLE
			{
				// Token: 0x0400E74E RID: 59214
				public static LocString NAME = UI.FormatAsLink("Pikeapple Skewer", "COOKEDPIKEAPPLE");

				// Token: 0x0400E74F RID: 59215
				public static LocString DESC = "Grilling a " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + " softens its spikes, making it slighly less awkward to eat.\n\nIt does not diminish the smell.";

				// Token: 0x0400E750 RID: 59216
				public static LocString RECIPEDESC = "A grilled dish made with a fruiting " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + ".";
			}

			// Token: 0x020038F8 RID: 14584
			public class PRICKLEFRUIT
			{
				// Token: 0x0400E751 RID: 59217
				public static LocString NAME = UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT");

				// Token: 0x0400E752 RID: 59218
				public static LocString DESC = "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";
			}

			// Token: 0x020038F9 RID: 14585
			public class GRILLEDPRICKLEFRUIT
			{
				// Token: 0x0400E753 RID: 59219
				public static LocString NAME = UI.FormatAsLink("Gristle Berry", "GRILLEDPRICKLEFRUIT");

				// Token: 0x0400E754 RID: 59220
				public static LocString DESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".\n\nHeat unlocked an exquisite taste in the fruit, though the burnt spines leave something to be desired.";

				// Token: 0x0400E755 RID: 59221
				public static LocString RECIPEDESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".";
			}

			// Token: 0x020038FA RID: 14586
			public class SWAMPFRUIT
			{
				// Token: 0x0400E756 RID: 59222
				public static LocString NAME = UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT");

				// Token: 0x0400E757 RID: 59223
				public static LocString DESC = "A fruit with an outer film that contains chewy gelatinous cubes.";
			}

			// Token: 0x020038FB RID: 14587
			public class SWAMPDELIGHTS
			{
				// Token: 0x0400E758 RID: 59224
				public static LocString NAME = UI.FormatAsLink("Swampy Delights", "SWAMPDELIGHTS");

				// Token: 0x0400E759 RID: 59225
				public static LocString DESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".\n\nEach cube has a wonderfully chewy texture and is lightly coated in a delicate powder.";

				// Token: 0x0400E75A RID: 59226
				public static LocString RECIPEDESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".";
			}

			// Token: 0x020038FC RID: 14588
			public class WORMBASICFRUIT
			{
				// Token: 0x0400E75B RID: 59227
				public static LocString NAME = UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT");

				// Token: 0x0400E75C RID: 59228
				public static LocString DESC = "A " + UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT") + " that failed to develop properly.\n\nIt is nonetheless edible, and vaguely tasty.";
			}

			// Token: 0x020038FD RID: 14589
			public class WORMBASICFOOD
			{
				// Token: 0x0400E75D RID: 59229
				public static LocString NAME = UI.FormatAsLink("Roast Grubfruit Nut", "WORMBASICFOOD");

				// Token: 0x0400E75E RID: 59230
				public static LocString DESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".\n\nIt has a smoky aroma and tastes of coziness.";

				// Token: 0x0400E75F RID: 59231
				public static LocString RECIPEDESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".";
			}

			// Token: 0x020038FE RID: 14590
			public class WORMSUPERFRUIT
			{
				// Token: 0x0400E760 RID: 59232
				public static LocString NAME = UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT");

				// Token: 0x0400E761 RID: 59233
				public static LocString DESC = "A plump, healthy fruit with a honey-like taste.";
			}

			// Token: 0x020038FF RID: 14591
			public class WORMSUPERFOOD
			{
				// Token: 0x0400E762 RID: 59234
				public static LocString NAME = UI.FormatAsLink("Grubfruit Preserve", "WORMSUPERFOOD");

				// Token: 0x0400E763 RID: 59235
				public static LocString DESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					".\n\nThe thick, goopy jam retains the shape of the jar when poured out, but the sweet taste can't be matched."
				});

				// Token: 0x0400E764 RID: 59236
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					"."
				});
			}

			// Token: 0x02003900 RID: 14592
			public class VINEFRUITJAM
			{
				// Token: 0x0400E765 RID: 59237
				public static LocString NAME = UI.FormatAsLink("", "VINEFRUITJAM");

				// Token: 0x0400E766 RID: 59238
				public static LocString DESC = "";

				// Token: 0x0400E767 RID: 59239
				public static LocString RECIPEDESC = "";
			}

			// Token: 0x02003901 RID: 14593
			public class BERRYPIE
			{
				// Token: 0x0400E768 RID: 59240
				public static LocString NAME = UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE");

				// Token: 0x0400E769 RID: 59241
				public static LocString DESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					".\n\nThe mixture of berries creates a fragrant, colorful filling that packs a sweet punch."
				});

				// Token: 0x0400E76A RID: 59242
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					"."
				});

				// Token: 0x02003D9B RID: 15771
				public class DEHYDRATED
				{
					// Token: 0x0400F321 RID: 62241
					public static LocString NAME = UI.FormatAsLink("Dried Berry Pie", "BERRYPIE");

					// Token: 0x0400F322 RID: 62242
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003902 RID: 14594
			public class COLDWHEATBREAD
			{
				// Token: 0x0400E76B RID: 59243
				public static LocString NAME = UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD");

				// Token: 0x0400E76C RID: 59244
				public static LocString DESC = "A simple bun baked from " + ITEMS.FOOD.COLDWHEATSEED.NAME + ".\n\nEach bite leaves a mild cooling sensation in one's mouth, even when the bun itself is warm.";

				// Token: 0x0400E76D RID: 59245
				public static LocString RECIPEDESC = "A simple bun baked from " + ITEMS.FOOD.COLDWHEATSEED.NAME + ".";
			}

			// Token: 0x02003903 RID: 14595
			public class BEAN
			{
				// Token: 0x0400E76E RID: 59246
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEAN");

				// Token: 0x0400E76F RID: 59247
				public static LocString DESC = "The crisp bean of a " + UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT") + ".\n\nEach bite tastes refreshingly natural and wholesome.";
			}

			// Token: 0x02003904 RID: 14596
			public class SPICENUT
			{
				// Token: 0x0400E770 RID: 59248
				public static LocString NAME = UI.FormatAsLink("Pincha Peppernut", "SPICENUT");

				// Token: 0x0400E771 RID: 59249
				public static LocString DESC = "The flavorful nut of a " + UI.FormatAsLink("Pincha Pepperplant", "SPICE_VINE") + ".\n\nThe bitter outer rind hides a rich, peppery core that is useful in cooking.";
			}

			// Token: 0x02003905 RID: 14597
			public class VINEFRUIT
			{
				// Token: 0x0400E772 RID: 59250
				public static LocString NAME = UI.FormatAsLink("Ovagro Fig", "VINEFRUIT");

				// Token: 0x0400E773 RID: 59251
				public static LocString DESC = "The fruit from an " + UI.FormatAsLink("Ovagro Vine", "VINEMOTHER") + ".\n\nIt's fun to squeeze as many as possible into a single mouthful.";
			}

			// Token: 0x02003906 RID: 14598
			public class SPICEBREAD
			{
				// Token: 0x0400E774 RID: 59252
				public static LocString NAME = UI.FormatAsLink("Pepper Bread", "SPICEBREAD");

				// Token: 0x0400E775 RID: 59253
				public static LocString DESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.\n\nThere's a simple joy to be had in pulling it apart in one's fingers.";

				// Token: 0x0400E776 RID: 59254
				public static LocString RECIPEDESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.";

				// Token: 0x02003D9C RID: 15772
				public class DEHYDRATED
				{
					// Token: 0x0400F323 RID: 62243
					public static LocString NAME = UI.FormatAsLink("Dried Pepper Bread", "SPICEBREAD");

					// Token: 0x0400F324 RID: 62244
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Pepper Bread", "SPICEBREAD"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003907 RID: 14599
			public class SURFANDTURF
			{
				// Token: 0x0400E777 RID: 59255
				public static LocString NAME = UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF");

				// Token: 0x0400E778 RID: 59256
				public static LocString DESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea.\n\nIt's hearty and satisfying."
				});

				// Token: 0x0400E779 RID: 59257
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea."
				});

				// Token: 0x02003D9D RID: 15773
				public class DEHYDRATED
				{
					// Token: 0x0400F325 RID: 62245
					public static LocString NAME = UI.FormatAsLink("Dried Surf'n'Turf", "SURFANDTURF");

					// Token: 0x0400F326 RID: 62246
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003908 RID: 14600
			public class TOFU
			{
				// Token: 0x0400E77A RID: 59258
				public static LocString NAME = UI.FormatAsLink("Tofu", "TOFU");

				// Token: 0x0400E77B RID: 59259
				public static LocString DESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".\n\nIt has an unusual but pleasant consistency.";

				// Token: 0x0400E77C RID: 59260
				public static LocString RECIPEDESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".";
			}

			// Token: 0x02003909 RID: 14601
			public class SPICYTOFU
			{
				// Token: 0x0400E77D RID: 59261
				public static LocString NAME = UI.FormatAsLink("Spicy Tofu", "SPICYTOFU");

				// Token: 0x0400E77E RID: 59262
				public static LocString DESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.\n\nIt packs a delightful punch.";

				// Token: 0x0400E77F RID: 59263
				public static LocString RECIPEDESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.";

				// Token: 0x02003D9E RID: 15774
				public class DEHYDRATED
				{
					// Token: 0x0400F327 RID: 62247
					public static LocString NAME = UI.FormatAsLink("Dried Spicy Tofu", "SPICYTOFU");

					// Token: 0x0400F328 RID: 62248
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Spicy Tofu", "SPICYTOFU"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200390A RID: 14602
			public class CURRY
			{
				// Token: 0x0400E780 RID: 59264
				public static LocString NAME = UI.FormatAsLink("Curried Beans", "CURRY");

				// Token: 0x0400E781 RID: 59265
				public static LocString DESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					".\n\nIt's so spicy!"
				});

				// Token: 0x0400E782 RID: 59266
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					"."
				});

				// Token: 0x02003D9F RID: 15775
				public class DEHYDRATED
				{
					// Token: 0x0400F329 RID: 62249
					public static LocString NAME = UI.FormatAsLink("Dried Curried Beans", "CURRY");

					// Token: 0x0400F32A RID: 62250
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Curried Beans", "CURRY"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200390B RID: 14603
			public class SALSA
			{
				// Token: 0x0400E783 RID: 59267
				public static LocString NAME = UI.FormatAsLink("Stuffed Berry", "SALSA");

				// Token: 0x0400E784 RID: 59268
				public static LocString DESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x0400E785 RID: 59269
				public static LocString RECIPEDESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x02003DA0 RID: 15776
				public class DEHYDRATED
				{
					// Token: 0x0400F32B RID: 62251
					public static LocString NAME = UI.FormatAsLink("Dried Stuffed Berry", "SALSA");

					// Token: 0x0400F32C RID: 62252
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Stuffed Berry", "SALSA"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200390C RID: 14604
			public class HARDSKINBERRY
			{
				// Token: 0x0400E786 RID: 59270
				public static LocString NAME = UI.FormatAsLink("Pikeapple", "HARDSKINBERRY");

				// Token: 0x0400E787 RID: 59271
				public static LocString DESC = "An edible fruit encased in a thorny husk.";
			}

			// Token: 0x0200390D RID: 14605
			public class CARROT
			{
				// Token: 0x0400E788 RID: 59272
				public static LocString NAME = UI.FormatAsLink("Plume Squash", "CARROT");

				// Token: 0x0400E789 RID: 59273
				public static LocString DESC = "An edible tuber with an earthy, elegant flavor.";
			}

			// Token: 0x0200390E RID: 14606
			public class FERNFOOD
			{
				// Token: 0x0400E78A RID: 59274
				public static LocString NAME = UI.FormatAsLink("Megafrond Grain", "FERNFOOD");

				// Token: 0x0400E78B RID: 59275
				public static LocString DESC = "An ancient grain that can be processed into " + UI.FormatAsLink("Food", "FOOD") + ".";
			}

			// Token: 0x0200390F RID: 14607
			public class PEMMICAN
			{
				// Token: 0x0400E78C RID: 59276
				public static LocString NAME = UI.FormatAsLink("Pemmican", "PEMMICAN");

				// Token: 0x0400E78D RID: 59277
				public static LocString DESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a calorie-dense brick with an exceptionally long shelf life.\n\nSurvival never tasted so good.";

				// Token: 0x0400E78E RID: 59278
				public static LocString RECIPEDESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a nutrient-dense brick with an exceptionally long shelf life.";
			}

			// Token: 0x02003910 RID: 14608
			public class BASICPLANTFOOD
			{
				// Token: 0x0400E78F RID: 59279
				public static LocString NAME = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD");

				// Token: 0x0400E790 RID: 59280
				public static LocString DESC = "A flavorless grain that almost never wiggles on its own.";
			}

			// Token: 0x02003911 RID: 14609
			public class BASICPLANTBAR
			{
				// Token: 0x0400E791 RID: 59281
				public static LocString NAME = UI.FormatAsLink("Liceloaf", "BASICPLANTBAR");

				// Token: 0x0400E792 RID: 59282
				public static LocString DESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";

				// Token: 0x0400E793 RID: 59283
				public static LocString RECIPEDESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";
			}

			// Token: 0x02003912 RID: 14610
			public class BASICFORAGEPLANT
			{
				// Token: 0x0400E794 RID: 59284
				public static LocString NAME = UI.FormatAsLink("Muckroot", "BASICFORAGEPLANT");

				// Token: 0x0400E795 RID: 59285
				public static LocString DESC = "A seedless fruit with an upsettingly bland aftertaste.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a " + ITEMS.FOOD.BASICFORAGEPLANT.NAME + ".";
			}

			// Token: 0x02003913 RID: 14611
			public class FORESTFORAGEPLANT
			{
				// Token: 0x0400E796 RID: 59286
				public static LocString NAME = UI.FormatAsLink("Hexalent Fruit", "FORESTFORAGEPLANT");

				// Token: 0x0400E797 RID: 59287
				public static LocString DESC = "A seedless fruit with an unusual rubbery texture.\n\nIt cannot be replanted.\n\nHexalent fruit is much more calorie dense than Muckroot fruit.";
			}

			// Token: 0x02003914 RID: 14612
			public class SWAMPFORAGEPLANT
			{
				// Token: 0x0400E798 RID: 59288
				public static LocString NAME = UI.FormatAsLink("Swamp Chard Heart", "SWAMPFORAGEPLANT");

				// Token: 0x0400E799 RID: 59289
				public static LocString DESC = "A seedless plant with a squishy, juicy center and an awful smell.\n\nIt cannot be replanted.";
			}

			// Token: 0x02003915 RID: 14613
			public class ICECAVESFORAGEPLANT
			{
				// Token: 0x0400E79A RID: 59290
				public static LocString NAME = UI.FormatAsLink("Sherberry", "ICECAVESFORAGEPLANT");

				// Token: 0x0400E79B RID: 59291
				public static LocString DESC = "A cold seedless fruit that triggers mild brain freeze.\n\nIt cannot be replanted.";
			}

			// Token: 0x02003916 RID: 14614
			public class ROTPILE
			{
				// Token: 0x0400E79C RID: 59292
				public static LocString NAME = UI.FormatAsLink("Rot Pile", "COMPOST");

				// Token: 0x0400E79D RID: 59293
				public static LocString DESC = string.Concat(new string[]
				{
					"An inedible glop of former foodstuff.\n\n",
					ITEMS.FOOD.ROTPILE.NAME,
					"s break down into ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" over time."
				});
			}

			// Token: 0x02003917 RID: 14615
			public class COLDWHEATSEED
			{
				// Token: 0x0400E79E RID: 59294
				public static LocString NAME = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED");

				// Token: 0x0400E79F RID: 59295
				public static LocString DESC = "An edible grain that leaves a cool taste on the tongue.";
			}

			// Token: 0x02003918 RID: 14616
			public class BEANPLANTSEED
			{
				// Token: 0x0400E7A0 RID: 59296
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED");

				// Token: 0x0400E7A1 RID: 59297
				public static LocString DESC = "An inedible bean that can be processed into delicious foods.";
			}

			// Token: 0x02003919 RID: 14617
			public class QUICHE
			{
				// Token: 0x0400E7A2 RID: 59298
				public static LocString NAME = UI.FormatAsLink("Mushroom Quiche", "QUICHE");

				// Token: 0x0400E7A3 RID: 59299
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust.\n\nSomehow, it's both soggy <i>and</i> crispy."
				});

				// Token: 0x0400E7A4 RID: 59300
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust."
				});

				// Token: 0x02003DA1 RID: 15777
				public class DEHYDRATED
				{
					// Token: 0x0400F32D RID: 62253
					public static LocString NAME = UI.FormatAsLink("Dried Mushroom Quiche", "QUICHE");

					// Token: 0x0400F32E RID: 62254
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mushroom Quiche", "QUICHE"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200391A RID: 14618
			public class GARDENFOODPLANTFOOD
			{
				// Token: 0x0400E7A5 RID: 59301
				public static LocString NAME = UI.FormatAsLink("Sweatcorn", "GARDENFOODPLANTFOOD");

				// Token: 0x0400E7A6 RID: 59302
				public static LocString DESC = "The sugary vegetable produced by " + UI.FormatAsLink("Sweatcorn Stalks", "GARDENFOODPLANT") + ".\n\nIt tastes a lot better deep-fried.";
			}

			// Token: 0x0200391B RID: 14619
			public class GARDENFORAGEPLANT
			{
				// Token: 0x0400E7A7 RID: 59303
				public static LocString NAME = UI.FormatAsLink("Snac Fruit", "GARDENFORAGEPLANT");

				// Token: 0x0400E7A8 RID: 59304
				public static LocString DESC = "A seedless fruit that loses its flavor long before it is fully chewed.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a " + ITEMS.FOOD.GARDENFORAGEPLANT.NAME + ".";
			}

			// Token: 0x0200391C RID: 14620
			public class BUTTERFLYPLANTSEED
			{
				// Token: 0x0400E7A9 RID: 59305
				public static LocString NAME = UI.FormatAsLink("Mimillet", "BUTTERFLYPLANTSEED");

				// Token: 0x0400E7AA RID: 59306
				public static LocString DESC = string.Concat(new string[]
				{
					"An inedible seed from a ",
					UI.FormatAsLink("Mimika Bud", "BUTTERFLYPLANT"),
					".\n\nIt can be sown to cultivate more plants, or processed into ",
					UI.FormatAsLink("Food", "FOOD"),
					".\n\nDigging up Buried Objects may uncover a Mimillet Seed."
				});

				// Token: 0x0400E7AB RID: 59307
				public static LocString RECIPEDESC = "An inedible " + CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.NAME + " seed.";
			}

			// Token: 0x0200391D RID: 14621
			public class BUTTERFLYFOOD
			{
				// Token: 0x0400E7AC RID: 59308
				public static LocString NAME = UI.FormatAsLink("Toasted Mimillet", "BUTTERFLYFOOD");

				// Token: 0x0400E7AD RID: 59309
				public static LocString DESC = "A lightly toasted " + CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.NAME + ".\n\nIt makes the tummy feel a bit fluttery.";

				// Token: 0x0400E7AE RID: 59310
				public static LocString RECIPEDESC = "A lightly toasted " + CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.NAME + ".";
			}
		}

		// Token: 0x02002592 RID: 9618
		public class INGREDIENTS
		{
			// Token: 0x0200391E RID: 14622
			public class SWAMPLILYFLOWER
			{
				// Token: 0x0400E7AF RID: 59311
				public static LocString NAME = UI.FormatAsLink("Balm Lily Flower", "SWAMPLILYFLOWER");

				// Token: 0x0400E7B0 RID: 59312
				public static LocString DESC = "A medicinal flower that soothes most minor maladies.\n\nIt is exceptionally fragrant.";
			}

			// Token: 0x0200391F RID: 14623
			public class GINGER
			{
				// Token: 0x0400E7B1 RID: 59313
				public static LocString NAME = UI.FormatAsLink("Tonic Root", "GINGERCONFIG");

				// Token: 0x0400E7B2 RID: 59314
				public static LocString DESC = "A chewy, fibrous rhizome with a fiery aftertaste.";
			}

			// Token: 0x02003920 RID: 14624
			public class KELP
			{
				// Token: 0x0400E7B3 RID: 59315
				public static LocString NAME = UI.FormatAsLink("Seakomb Leaf", "KELP");

				// Token: 0x0400E7B4 RID: 59316
				public static LocString DESC = string.Concat(new string[]
				{
					"The leaf of a ",
					UI.FormatAsLink("Seakomb", "KELPPLANT"),
					".\n\nIt can be processed into ",
					UI.FormatAsLink("Phyto Oil", "PHYTOOIL"),
					" or used as an ingredient in ",
					UI.FormatAsLink("Allergy Medication", "ANTIHISTAMINE"),
					"."
				});
			}
		}

		// Token: 0x02002593 RID: 9619
		public class INDUSTRIAL_PRODUCTS
		{
			// Token: 0x02003921 RID: 14625
			public class ELECTROBANK_URANIUM_ORE
			{
				// Token: 0x0400E7B5 RID: 59317
				public static LocString NAME = UI.FormatAsLink("Uranium Ore Power Bank", "ELECTROBANK_URANIUM_ORE");

				// Token: 0x0400E7B6 RID: 59318
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Uranium Ore Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					".\n\nMust be kept dry."
				});
			}

			// Token: 0x02003922 RID: 14626
			public class ELECTROBANK_METAL_ORE
			{
				// Token: 0x0400E7B7 RID: 59319
				public static LocString NAME = UI.FormatAsLink("Metal Power Bank", "ELECTROBANK_METAL_ORE");

				// Token: 0x0400E7B8 RID: 59320
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Metal Ore", "METAL"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Metal Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					".\n\nMust be kept dry."
				});
			}

			// Token: 0x02003923 RID: 14627
			public class ELECTROBANK_SELFCHARGING
			{
				// Token: 0x0400E7B9 RID: 59321
				public static LocString NAME = UI.FormatAsLink("Atomic Power Bank", "ELECTROBANK_SELFCHARGING");

				// Token: 0x0400E7BA RID: 59322
				public static LocString DESC = string.Concat(new string[]
				{
					"A self-charging ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					ELEMENTS.ENRICHEDURANIUM.NAME,
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER"),
					".\n\nIts low ",
					UI.FormatAsLink("wattage", "POWER"),
					" and high ",
					UI.FormatAsLink("Radioactivity", "RADIATION"),
					" make it unsuitable for Bionic Duplicant use."
				});
			}

			// Token: 0x02003924 RID: 14628
			public class ELECTROBANK
			{
				// Token: 0x0400E7BB RID: 59323
				public static LocString NAME = UI.FormatAsLink("Eco Power Bank", "ELECTROBANK");

				// Token: 0x0400E7BC RID: 59324
				public static LocString DESC = string.Concat(new string[]
				{
					"A rechargeable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Eco Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Soldering Station", "ADVANCEDCRAFTINGTABLE"),
					".\n\nMust be kept dry."
				});
			}

			// Token: 0x02003925 RID: 14629
			public class ELECTROBANK_EMPTY
			{
				// Token: 0x0400E7BD RID: 59325
				public static LocString NAME = UI.FormatAsLink("Empty Eco Power Bank", "ELECTROBANK");

				// Token: 0x0400E7BE RID: 59326
				public static LocString DESC = string.Concat(new string[]
				{
					"A depleted ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					".\n\nIt must be recharged at a ",
					UI.FormatAsLink("Power Bank Charger", "ELECTROBANKCHARGER"),
					" before it can be reused."
				});
			}

			// Token: 0x02003926 RID: 14630
			public class ELECTROBANK_GARBAGE
			{
				// Token: 0x0400E7BF RID: 59327
				public static LocString NAME = UI.FormatAsLink("Power Bank Scrap", "ELECTROBANK");

				// Token: 0x0400E7C0 RID: 59328
				public static LocString DESC = string.Concat(new string[]
				{
					"A ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" that has reached the end of its lifetime.\n\nIt can be salvaged for ",
					UI.FormatAsLink("Abyssalite", "KATAIRITE"),
					" at the ",
					UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
					"."
				});
			}

			// Token: 0x02003927 RID: 14631
			public class FUEL_BRICK
			{
				// Token: 0x0400E7C1 RID: 59329
				public static LocString NAME = "Fuel Brick";

				// Token: 0x0400E7C2 RID: 59330
				public static LocString DESC = "A densely compressed brick of combustible material.\n\nIt can be burned to produce a one-time burst of " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			// Token: 0x02003928 RID: 14632
			public class BASIC_FABRIC
			{
				// Token: 0x0400E7C3 RID: 59331
				public static LocString NAME = UI.FormatAsLink("Reed Fiber", "BASIC_FABRIC");

				// Token: 0x0400E7C4 RID: 59332
				public static LocString DESC = "A ball of raw cellulose used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}

			// Token: 0x02003929 RID: 14633
			public class PLANT_FIBER
			{
				// Token: 0x0400E7C5 RID: 59333
				public static LocString NAME = UI.FormatAsLink("Plant Husk", "PLANT_FIBER");

				// Token: 0x0400E7C6 RID: 59334
				public static LocString DESC = string.Concat(new string[]
				{
					"A bundle of dried plant matter.\n\nIt can be eaten by ",
					CREATURES.FAMILY_PLURAL.MOOSPECIES,
					", or processed into ",
					UI.FormatAsLink("fuel", "POWER"),
					" or ",
					UI.FormatAsLink("building materials", "BUILDINGMATERIALCLASSES"),
					"."
				});
			}

			// Token: 0x0200392A RID: 14634
			public class FEATHER_FABRIC
			{
				// Token: 0x0400E7C7 RID: 59335
				public static LocString NAME = UI.FormatAsLink("Feather Fiber", "FEATHER_FABRIC");

				// Token: 0x0400E7C8 RID: 59336
				public static LocString DESC = "A stalk of raw keratin used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}

			// Token: 0x0200392B RID: 14635
			public class DEWDRIP
			{
				// Token: 0x0400E7C9 RID: 59337
				public static LocString NAME = UI.FormatAsLink("Dewdrip", "DEWDRIP");

				// Token: 0x0400E7CA RID: 59338
				public static LocString DESC = string.Concat(new string[]
				{
					"A crystallized blob of ",
					UI.FormatAsLink("Brackene", "MILK"),
					" from the ",
					UI.FormatAsLink("Dew Dripper", "DEWDRIPPERPLANT"),
					"."
				});
			}

			// Token: 0x0200392C RID: 14636
			public class TRAP_PARTS
			{
				// Token: 0x0400E7CB RID: 59339
				public static LocString NAME = "Trap Components";

				// Token: 0x0400E7CC RID: 59340
				public static LocString DESC = string.Concat(new string[]
				{
					"These components can be assembled into a ",
					BUILDINGS.PREFABS.CREATURETRAP.NAME,
					" and used to catch ",
					UI.FormatAsLink("Critters", "CREATURES"),
					"."
				});
			}

			// Token: 0x0200392D RID: 14637
			public class POWER_STATION_TOOLS
			{
				// Token: 0x0400E7CD RID: 59341
				public static LocString NAME = UI.FormatAsLink("Microchip", "POWER_STATION_TOOLS");

				// Token: 0x0400E7CE RID: 59342
				public static LocString DESC = string.Concat(new string[]
				{
					"A specialized ",
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					" created by a professional engineer.\n\nTunes up ",
					UI.FormatAsLink("Generators", "REQUIREMENTCLASSGENERATORTYPE"),
					" to increase their ",
					UI.FormatAsLink("Power", "POWER"),
					" output.\n\nAlso used in the production of ",
					UI.FormatAsLink("Boosters", "BOOSTER"),
					" for Bionic Duplicants."
				});

				// Token: 0x0400E7CF RID: 59343
				public static LocString TINKER_REQUIREMENT_NAME = "Skill: " + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400E7D0 RID: 59344
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400E7D1 RID: 59345
				public static LocString TINKER_EFFECT_NAME = "Engie's Tune-Up: {0} {1}";

				// Token: 0x0400E7D2 RID: 59346
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					" a generator, increasing its {0} by <b>{1}</b>."
				});

				// Token: 0x0400E7D3 RID: 59347
				public static LocString RECIPE_DESCRIPTION = "Make " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " from {0}";
			}

			// Token: 0x0200392E RID: 14638
			public class FARM_STATION_TOOLS
			{
				// Token: 0x0400E7D4 RID: 59348
				public static LocString NAME = UI.FormatAsLink("Micronutrient Fertilizer", "FARM_STATION_TOOLS");

				// Token: 0x0400E7D5 RID: 59349
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized ",
					UI.FormatAsLink("Fertilizer", "FERTILIZER"),
					" mixed by a Duplicant with the ",
					DUPLICANTS.ROLES.FARMER.NAME,
					" Skill.\n\nIncreases the ",
					UI.PRE_KEYWORD,
					"Growth Rate",
					UI.PST_KEYWORD,
					" of one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					"."
				});
			}

			// Token: 0x0200392F RID: 14639
			public class MACHINE_PARTS
			{
				// Token: 0x0400E7D6 RID: 59350
				public static LocString NAME = "Custom Parts";

				// Token: 0x0400E7D7 RID: 59351
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized Parts crafted by a professional engineer.\n\n",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" machine buildings to increase their efficiency."
				});

				// Token: 0x0400E7D8 RID: 59352
				public static LocString TINKER_REQUIREMENT_NAME = "Job: " + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400E7D9 RID: 59353
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400E7DA RID: 59354
				public static LocString TINKER_EFFECT_NAME = "Engineer's Jerry Rig: {0} {1}";

				// Token: 0x0400E7DB RID: 59355
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" upgrades to a machine building, increasing its {0} by <b>{1}</b>."
				});
			}

			// Token: 0x02003930 RID: 14640
			public class RESEARCH_DATABANK
			{
				// Token: 0x0400E7DC RID: 59356
				public static LocString NAME = UI.FormatAsLink("Data Bank", "DATABANK");

				// Token: 0x0400E7DD RID: 59357
				public static LocString NAME_PLURAL = UI.FormatAsLink("Data Banks", "DATABANK");

				// Token: 0x0400E7DE RID: 59358
				public static LocString DESC = "Raw data that can be processed into " + UI.FormatAsLink("Interstellar Research", "RESEARCH") + " points.";
			}

			// Token: 0x02003931 RID: 14641
			public class ORBITAL_RESEARCH_DATABANK
			{
				// Token: 0x0400E7DF RID: 59359
				public static LocString NAME = UI.FormatAsLink("Data Bank", "DATABANK");

				// Token: 0x0400E7E0 RID: 59360
				public static LocString NAME_PLURAL = UI.FormatAsLink("Data Banks", "DATABANK");

				// Token: 0x0400E7E1 RID: 59361
				public static LocString DESC = "Raw Data that can be processed into " + UI.FormatAsLink("Data Analysis Research", "RESEARCHDLC1") + " points.";

				// Token: 0x0400E7E2 RID: 59362
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Data Banks of raw data generated from exploring, either by exploring new areas with Duplicants, or by using an ",
					UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER"),
					".\n\nUsed by the ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to conduct research."
				});
			}

			// Token: 0x02003932 RID: 14642
			public class EGG_SHELL
			{
				// Token: 0x0400E7E3 RID: 59363
				public static LocString NAME = UI.FormatAsLink("Egg Shell", "EGG_SHELL");

				// Token: 0x0400E7E4 RID: 59364
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";
			}

			// Token: 0x02003933 RID: 14643
			public class GOLD_BELLY_CROWN
			{
				// Token: 0x0400E7E5 RID: 59365
				public static LocString NAME = UI.FormatAsLink("Regal Bammoth Crest", "GOLD_BELLY_CROWN");

				// Token: 0x0400E7E6 RID: 59366
				public static LocString DESC = "Can be crushed to produce " + ELEMENTS.GOLDAMALGAM.NAME + ".";
			}

			// Token: 0x02003934 RID: 14644
			public class CRAB_SHELL
			{
				// Token: 0x0400E7E7 RID: 59367
				public static LocString NAME = UI.FormatAsLink("Pokeshell Molt", "CRAB_SHELL");

				// Token: 0x0400E7E8 RID: 59368
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x02003DA2 RID: 15778
				public class VARIANT_WOOD
				{
					// Token: 0x0400F32F RID: 62255
					public static LocString NAME = UI.FormatAsLink("Oakshell Molt", "CRABWOODSHELL");

					// Token: 0x0400F330 RID: 62256
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x02003935 RID: 14645
			public class BABY_CRAB_SHELL
			{
				// Token: 0x0400E7E9 RID: 59369
				public static LocString NAME = UI.FormatAsLink("Small Pokeshell Molt", "CRAB_SHELL");

				// Token: 0x0400E7EA RID: 59370
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x02003DA3 RID: 15779
				public class VARIANT_WOOD
				{
					// Token: 0x0400F331 RID: 62257
					public static LocString NAME = UI.FormatAsLink("Small Oakshell Molt", "CRABWOODSHELL");

					// Token: 0x0400F332 RID: 62258
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x02003936 RID: 14646
			public class WOOD
			{
				// Token: 0x0400E7EB RID: 59371
				public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");

				// Token: 0x0400E7EC RID: 59372
				public static LocString DESC = string.Concat(new string[]
				{
					"Natural resource harvested from certain ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" and ",
					UI.FormatAsLink("Plants", "PLANTS"),
					".\n\nUsed in construction or ",
					UI.FormatAsLink("Heat", "HEAT"),
					" production."
				});
			}

			// Token: 0x02003937 RID: 14647
			public class GENE_SHUFFLER_RECHARGE
			{
				// Token: 0x0400E7ED RID: 59373
				public static LocString NAME = "Vacillator Recharge";

				// Token: 0x0400E7EE RID: 59374
				public static LocString DESC = "Replenishes one charge to a depleted " + BUILDINGS.PREFABS.GENESHUFFLER.NAME + ".";
			}

			// Token: 0x02003938 RID: 14648
			public class TABLE_SALT
			{
				// Token: 0x0400E7EF RID: 59375
				public static LocString NAME = UI.FormatAsLink("Table Salt", "IDTABLESALT");

				// Token: 0x0400E7F0 RID: 59376
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Table Salt while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}

			// Token: 0x02003939 RID: 14649
			public class REFINED_SUGAR
			{
				// Token: 0x0400E7F1 RID: 59377
				public static LocString NAME = "Refined Sugar";

				// Token: 0x0400E7F2 RID: 59378
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Refined Sugar while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}

			// Token: 0x0200393A RID: 14650
			public class ICE_BELLY_POOP
			{
				// Token: 0x0400E7F3 RID: 59379
				public static LocString NAME = UI.FormatAsLink("Bammoth Patty", "ICE_BELLY_POOP");

				// Token: 0x0400E7F4 RID: 59380
				public static LocString DESC = string.Concat(new string[]
				{
					"A little treat left behind by a very large critter.\n\nIt can be crushed to extract ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					" and ",
					UI.FormatAsLink("Clay", "CLAY"),
					"."
				});
			}
		}

		// Token: 0x02002594 RID: 9620
		public class CARGO_CAPSULE
		{
			// Token: 0x0400A9C0 RID: 43456
			public static LocString NAME = "Care Package";

			// Token: 0x0400A9C1 RID: 43457
			public static LocString DESC = "A delivery system for recently printed resources.\n\nIt will dematerialize shortly.";
		}

		// Token: 0x02002595 RID: 9621
		public class RAILGUNPAYLOAD
		{
			// Token: 0x0400A9C2 RID: 43458
			public static LocString NAME = UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD");

			// Token: 0x0400A9C3 RID: 43459
			public static LocString DESC = string.Concat(new string[]
			{
				"Contains resources packed for interstellar shipping.\n\nCan be launched by a ",
				BUILDINGS.PREFABS.RAILGUN.NAME,
				" or unpacked with a ",
				BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME,
				"."
			});
		}

		// Token: 0x02002596 RID: 9622
		public class MISSILE_BASIC
		{
			// Token: 0x0400A9C4 RID: 43460
			public static LocString NAME = UI.FormatAsLink("Blastshot", "MISSILE_BASIC");

			// Token: 0x0400A9C5 RID: 43461
			public static LocString DESC = "An explosive projectile designed to defend against meteor showers.\n\nMust be launched by a " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}

		// Token: 0x02002597 RID: 9623
		public class MISSILE_LONGRANGE_VANILLADLC4
		{
			// Token: 0x0400A9C6 RID: 43462
			public static LocString NAME = UI.FormatAsLink("Intracosmic Blastshot", "MISSILE_LONGRANGE_VANILLADLC4");

			// Token: 0x0400A9C7 RID: 43463
			public static LocString DESC = "A long-range explosive projectile that defends against distant space objects.\n\nMust be launched by " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}

		// Token: 0x02002598 RID: 9624
		public class MISSILE_LONGRANGE
		{
			// Token: 0x0400A9C8 RID: 43464
			public static LocString NAME = UI.FormatAsLink("Intracosmic Blastshot", "MISSILE_LONGRANGE");

			// Token: 0x0400A9C9 RID: 43465
			public static LocString DESC = "A long-range explosive projectile that defends against distant space objects.\n\nMust be launched by " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}

		// Token: 0x02002599 RID: 9625
		public class DEBRISPAYLOAD
		{
			// Token: 0x0400A9CA RID: 43466
			public static LocString NAME = "Rocket Debris";

			// Token: 0x0400A9CB RID: 43467
			public static LocString DESC = "Whatever is left over from a Rocket Self-Destruct can be recovered once it has crash-landed.";
		}

		// Token: 0x0200259A RID: 9626
		public class RADIATION
		{
			// Token: 0x0200393B RID: 14651
			public class HIGHENERGYPARITCLE
			{
				// Token: 0x0400E7F5 RID: 59381
				public static LocString NAME = "Radbolts";

				// Token: 0x0400E7F6 RID: 59382
				public static LocString DESC = string.Concat(new string[]
				{
					"A concentrated field of ",
					UI.FormatAsKeyWord("Radbolts"),
					" that can be largely redirected using a ",
					UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR"),
					"."
				});
			}
		}

		// Token: 0x0200259B RID: 9627
		public class DREAMJOURNAL
		{
			// Token: 0x0400A9CC RID: 43468
			public static LocString NAME = UI.FormatAsLink("Dream Journal", "STORYTRAITMEGABRAINTANK");

			// Token: 0x0400A9CD RID: 43469
			public static LocString DESC = string.Concat(new string[]
			{
				"A hand-scrawled account of ",
				UI.FormatAsLink("Pajama", "SLEEP_CLINIC_PAJAMAS"),
				"-induced dreams.\n\nCan be analyzed using a ",
				UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK"),
				"."
			});
		}

		// Token: 0x0200259C RID: 9628
		public class DEHYDRATEDFOODPACKAGE
		{
			// Token: 0x0400A9CE RID: 43470
			public static LocString NAME = "Dry Ration";

			// Token: 0x0400A9CF RID: 43471
			public static LocString DESC = "A package of non-perishable dehydrated food.\n\nIt requires no refrigeration, but must be rehydrated before consumption.";

			// Token: 0x0400A9D0 RID: 43472
			public static LocString CONSUMED = "Ate Rehydrated Food";

			// Token: 0x0400A9D1 RID: 43473
			public static LocString CONTENTS = "Dried {0}";
		}

		// Token: 0x0200259D RID: 9629
		public class SPICES
		{
			// Token: 0x0200393C RID: 14652
			public class MACHINERY_SPICE
			{
				// Token: 0x0400E7F7 RID: 59383
				public static LocString NAME = UI.FormatAsLink("Machinist Spice", "MACHINERY_SPICE");

				// Token: 0x0400E7F8 RID: 59384
				public static LocString DESC = "Improves operating skills when ingested.";
			}

			// Token: 0x0200393D RID: 14653
			public class PILOTING_SPICE
			{
				// Token: 0x0400E7F9 RID: 59385
				public static LocString NAME = UI.FormatAsLink("Rocketeer Spice", "PILOTING_SPICE");

				// Token: 0x0400E7FA RID: 59386
				public static LocString DESC = "Provides a boost to piloting abilities.";
			}

			// Token: 0x0200393E RID: 14654
			public class PRESERVING_SPICE
			{
				// Token: 0x0400E7FB RID: 59387
				public static LocString NAME = UI.FormatAsLink("Freshener Spice", "PRESERVING_SPICE");

				// Token: 0x0400E7FC RID: 59388
				public static LocString DESC = "Slows the decomposition of perishable foods.";
			}

			// Token: 0x0200393F RID: 14655
			public class STRENGTH_SPICE
			{
				// Token: 0x0400E7FD RID: 59389
				public static LocString NAME = UI.FormatAsLink("Brawny Spice", "STRENGTH_SPICE");

				// Token: 0x0400E7FE RID: 59390
				public static LocString DESC = "Strengthens even the weakest of muscles.";
			}
		}
	}
}
