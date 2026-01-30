using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x02000FF9 RID: 4089
	public class DUPLICANTS
	{
		// Token: 0x04006041 RID: 24641
		public static LocString RACE_PREFIX = "Species: {0}";

		// Token: 0x04006042 RID: 24642
		public static LocString RACE = "Duplicant";

		// Token: 0x04006043 RID: 24643
		public static LocString MODELTITLE = "Species: ";

		// Token: 0x04006044 RID: 24644
		public static LocString NAMETITLE = "Name: ";

		// Token: 0x04006045 RID: 24645
		public static LocString GENDERTITLE = "Gender: ";

		// Token: 0x04006046 RID: 24646
		public static LocString ARRIVALTIME = "Age: ";

		// Token: 0x04006047 RID: 24647
		public static LocString ARRIVALTIME_TOOLTIP = "This {1} was printed on <b>Cycle {0}</b>";

		// Token: 0x04006048 RID: 24648
		public static LocString DESC_TOOLTIP = "About {0}s";

		// Token: 0x0200257C RID: 9596
		public class MODEL
		{
			// Token: 0x020034C1 RID: 13505
			public class STANDARD
			{
				// Token: 0x0400DC9B RID: 56475
				public static LocString NAME = UI.FormatAsLink("Standard Duplicant", "DUPLICANTS");

				// Token: 0x0400DC9C RID: 56476
				public static LocString DESC = string.Concat(new string[]
				{
					"Standard Duplicants are hard workers who enjoy good ",
					UI.FormatAsLink("Food", "FOOD"),
					", fresh ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and creative colony-building.\n\nThey will complete errands in order of ",
					UI.FormatAsLink("Priority", "PRIORITY"),
					"."
				});

				// Token: 0x0400DC9D RID: 56477
				public static LocString NAME_ADJECTIVE = UI.FormatAsLink("Standard", "DUPLICANTS");
			}

			// Token: 0x020034C2 RID: 13506
			public class BIONIC
			{
				// Token: 0x0400DC9E RID: 56478
				public static LocString NAME = UI.FormatAsLink("Bionic Duplicant", "DUPLICANTS");

				// Token: 0x0400DC9F RID: 56479
				public static LocString NAME_TOOLTIP = "This Duplicant is a curious combination of organic and inorganic parts";

				// Token: 0x0400DCA0 RID: 56480
				public static LocString DESC = string.Concat(new string[]
				{
					"Bionic Duplicants run on ",
					UI.FormatAsLink("Power Banks", "ELECTROBANK"),
					", ",
					UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
					" and unbridled enthusiasm.\n\nThey should not be permitted to use standard ",
					UI.FormatAsLink("Toilets", "MISCELLANEOUSTIPS"),
					"."
				});

				// Token: 0x0400DCA1 RID: 56481
				public static LocString NAME_ADJECTIVE = UI.FormatAsLink("Bionic", "DUPLICANTS");
			}

			// Token: 0x020034C3 RID: 13507
			public class REMOTEWORKER
			{
				// Token: 0x0400DCA2 RID: 56482
				public static LocString NAME = "Remote Worker";

				// Token: 0x0400DCA3 RID: 56483
				public static LocString DESC = "A remotely operated work robot.\n\nIt performs chores as instructed by a " + UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL") + " on the same planetoid.";
			}
		}

		// Token: 0x0200257D RID: 9597
		public class GENDER
		{
			// Token: 0x020034C4 RID: 13508
			public class MALE
			{
				// Token: 0x0400DCA4 RID: 56484
				public static LocString NAME = "M";

				// Token: 0x02003D7B RID: 15739
				public class PLURALS
				{
					// Token: 0x0400F2B2 RID: 62130
					public static LocString ONE = "he";

					// Token: 0x0400F2B3 RID: 62131
					public static LocString TWO = "his";
				}
			}

			// Token: 0x020034C5 RID: 13509
			public class FEMALE
			{
				// Token: 0x0400DCA5 RID: 56485
				public static LocString NAME = "F";

				// Token: 0x02003D7C RID: 15740
				public class PLURALS
				{
					// Token: 0x0400F2B4 RID: 62132
					public static LocString ONE = "she";

					// Token: 0x0400F2B5 RID: 62133
					public static LocString TWO = "her";
				}
			}

			// Token: 0x020034C6 RID: 13510
			public class NB
			{
				// Token: 0x0400DCA6 RID: 56486
				public static LocString NAME = "X";

				// Token: 0x02003D7D RID: 15741
				public class PLURALS
				{
					// Token: 0x0400F2B6 RID: 62134
					public static LocString ONE = "they";

					// Token: 0x0400F2B7 RID: 62135
					public static LocString TWO = "their";
				}
			}
		}

		// Token: 0x0200257E RID: 9598
		public class STATS
		{
			// Token: 0x020034C7 RID: 13511
			public class SUBJECTS
			{
				// Token: 0x0400DCA7 RID: 56487
				public static LocString DUPLICANT = "Duplicant";

				// Token: 0x0400DCA8 RID: 56488
				public static LocString DUPLICANT_POSSESSIVE = "Duplicant's";

				// Token: 0x0400DCA9 RID: 56489
				public static LocString DUPLICANT_PLURAL = "Duplicants";

				// Token: 0x0400DCAA RID: 56490
				public static LocString CREATURE = "critter";

				// Token: 0x0400DCAB RID: 56491
				public static LocString CREATURE_POSSESSIVE = "critter's";

				// Token: 0x0400DCAC RID: 56492
				public static LocString CREATURE_PLURAL = "critters";

				// Token: 0x0400DCAD RID: 56493
				public static LocString PLANT = "plant";

				// Token: 0x0400DCAE RID: 56494
				public static LocString PLANT_POSESSIVE = "plant's";

				// Token: 0x0400DCAF RID: 56495
				public static LocString PLANT_PLURAL = "plants";
			}

			// Token: 0x020034C8 RID: 13512
			public class BIONICINTERNALBATTERY
			{
				// Token: 0x0400DCB0 RID: 56496
				public static LocString NAME = "Power Banks";

				// Token: 0x0400DCB1 RID: 56497
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Bionic Duplicant with zero remaining ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" will become incapacitated until replacement ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					" are installed"
				});
			}

			// Token: 0x020034C9 RID: 13513
			public class BIONICOXYGENTANK
			{
				// Token: 0x0400DCB2 RID: 56498
				public static LocString NAME = "Oxygen Tank";

				// Token: 0x0400DCB3 RID: 56499
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants have internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tanks that enable them to work in low breathability areas\n\nThey will prioritize ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" intake from equipped ",
					UI.PRE_KEYWORD,
					"Exosuits",
					UI.PST_KEYWORD,
					" to conserve their internal tanks"
				});

				// Token: 0x0400DCB4 RID: 56500
				public static LocString TOOLTIP_MASS_LINE = "Current mass: {0} / {1}";

				// Token: 0x0400DCB5 RID: 56501
				public static LocString TOOLTIP_MASS_ROW_DETAIL = "    • {0}: {1}{2}";

				// Token: 0x0400DCB6 RID: 56502
				public static LocString TOOLTIP_GERM_DETAIL = " - {0}";
			}

			// Token: 0x020034CA RID: 13514
			public class BIONICOIL
			{
				// Token: 0x0400DCB7 RID: 56503
				public static LocString NAME = "Gear Oil";

				// Token: 0x0400DCB8 RID: 56504
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants will slow down significantly when ",
					UI.PRE_KEYWORD,
					"Gear Oil",
					UI.PST_KEYWORD,
					" levels reach zero\n\nThey can oil their joints by visiting a ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD,
					" or using ",
					UI.PRE_KEYWORD,
					"Gear Balm",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x020034CB RID: 13515
			public class BIONICGUNK
			{
				// Token: 0x0400DCB9 RID: 56505
				public static LocString NAME = "Gunk";

				// Token: 0x0400DCBA RID: 56506
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants become ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" when too much ",
					UI.PRE_KEYWORD,
					"Gunk",
					UI.PST_KEYWORD,
					" builds up in their bionic parts\n\nRegular visits to the ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" are required"
				});
			}

			// Token: 0x020034CC RID: 13516
			public class BREATH
			{
				// Token: 0x0400DCBB RID: 56507
				public static LocString NAME = "Breath";

				// Token: 0x0400DCBC RID: 56508
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant with zero remaining ",
					UI.PRE_KEYWORD,
					"Breath",
					UI.PST_KEYWORD,
					" will die immediately"
				});
			}

			// Token: 0x020034CD RID: 13517
			public class STAMINA
			{
				// Token: 0x0400DCBD RID: 56509
				public static LocString NAME = "Stamina";

				// Token: 0x0400DCBE RID: 56510
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will pass out from fatigue when ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					" reaches zero"
				});
			}

			// Token: 0x020034CE RID: 13518
			public class CALORIES
			{
				// Token: 0x0400DCBF RID: 56511
				public static LocString NAME = "Calories";

				// Token: 0x0400DCC0 RID: 56512
				public static LocString TOOLTIP = "This {1} can burn <b>{0}</b> before starving";
			}

			// Token: 0x020034CF RID: 13519
			public class TEMPERATURE
			{
				// Token: 0x0400DCC1 RID: 56513
				public static LocString NAME = "Body Temperature";

				// Token: 0x0400DCC2 RID: 56514
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A healthy Duplicant's ",
					UI.PRE_KEYWORD,
					"Body Temperature",
					UI.PST_KEYWORD,
					" is <b>{1}</b>"
				});

				// Token: 0x0400DCC3 RID: 56515
				public static LocString TOOLTIP_DOMESTICATEDCRITTER = string.Concat(new string[]
				{
					"This critter's ",
					UI.PRE_KEYWORD,
					"Body Temperature",
					UI.PST_KEYWORD,
					" is <b>{1}</b>"
				});
			}

			// Token: 0x020034D0 RID: 13520
			public class EXTERNALTEMPERATURE
			{
				// Token: 0x0400DCC4 RID: 56516
				public static LocString NAME = "External Temperature";

				// Token: 0x0400DCC5 RID: 56517
				public static LocString TOOLTIP = "This Duplicant's environment is <b>{0}</b>";
			}

			// Token: 0x020034D1 RID: 13521
			public class DECOR
			{
				// Token: 0x0400DCC6 RID: 56518
				public static LocString NAME = "Decor";

				// Token: 0x0400DCC7 RID: 56519
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants become stressed in areas with ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" lower than their expectations\n\nOpen the ",
					UI.FormatAsOverlay("Decor Overlay", global::Action.Overlay8),
					" to view current ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values"
				});

				// Token: 0x0400DCC8 RID: 56520
				public static LocString TOOLTIP_CURRENT = "\n\nCurrent Environmental Decor: <b>{0}</b>";

				// Token: 0x0400DCC9 RID: 56521
				public static LocString TOOLTIP_AVERAGE_TODAY = "\nAverage Decor This Cycle: <b>{0}</b>";

				// Token: 0x0400DCCA RID: 56522
				public static LocString TOOLTIP_AVERAGE_YESTERDAY = "\nAverage Decor Last Cycle: <b>{0}</b>";
			}

			// Token: 0x020034D2 RID: 13522
			public class STRESS
			{
				// Token: 0x0400DCCB RID: 56523
				public static LocString NAME = "Stress";

				// Token: 0x0400DCCC RID: 56524
				public static LocString TOOLTIP = "Duplicants exhibit their Stress Reactions at one hundred percent " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x020034D3 RID: 13523
			public class RADIATIONBALANCE
			{
				// Token: 0x0400DCCD RID: 56525
				public static LocString NAME = "Absorbed Rad Dose";

				// Token: 0x0400DCCE RID: 56526
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover when using the toilet\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});

				// Token: 0x0400DCCF RID: 56527
				public static LocString TOOLTIP_CURRENT_BALANCE = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover when using the toilet\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});

				// Token: 0x0400DCD0 RID: 56528
				public static LocString CURRENT_EXPOSURE = "Current Exposure: {0}/cycle";

				// Token: 0x0400DCD1 RID: 56529
				public static LocString CURRENT_REJUVENATION = "Current Rejuvenation: {0}/cycle";
			}

			// Token: 0x020034D4 RID: 13524
			public class BLADDER
			{
				// Token: 0x0400DCD2 RID: 56530
				public static LocString NAME = "Bladder";

				// Token: 0x0400DCD3 RID: 56531
				public static LocString TOOLTIP = "Duplicants make \"messes\" if no toilets are available at one hundred percent " + UI.PRE_KEYWORD + "Bladder" + UI.PST_KEYWORD;
			}

			// Token: 0x020034D5 RID: 13525
			public class HITPOINTS
			{
				// Token: 0x0400DCD4 RID: 56532
				public static LocString NAME = "Health";

				// Token: 0x0400DCD5 RID: 56533
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"When Duplicants reach zero ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" they become incapacitated and require rescuing\n\nWhen critters reach zero ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					", they will die immediately"
				});
			}

			// Token: 0x020034D6 RID: 13526
			public class SKIN_THICKNESS
			{
				// Token: 0x0400DCD6 RID: 56534
				public static LocString NAME = "Skin Thickness";
			}

			// Token: 0x020034D7 RID: 13527
			public class SKIN_DURABILITY
			{
				// Token: 0x0400DCD7 RID: 56535
				public static LocString NAME = "Skin Durability";
			}

			// Token: 0x020034D8 RID: 13528
			public class DISEASERECOVERYTIME
			{
				// Token: 0x0400DCD8 RID: 56536
				public static LocString NAME = "Disease Recovery";
			}

			// Token: 0x020034D9 RID: 13529
			public class TRUNKHEALTH
			{
				// Token: 0x0400DCD9 RID: 56537
				public static LocString NAME = "Trunk Health";

				// Token: 0x0400DCDA RID: 56538
				public static LocString TOOLTIP = "Tree branches will die if they do not have a healthy trunk to grow from";
			}

			// Token: 0x020034DA RID: 13530
			public class VINEMOTHERHEALTH
			{
				// Token: 0x0400DCDB RID: 56539
				public static LocString NAME = "Node Health";

				// Token: 0x0400DCDC RID: 56540
				public static LocString TOOLTIP = "Vines cannot grow if they do not have a healthy node to grow from";
			}
		}

		// Token: 0x0200257F RID: 9599
		public class DEATHS
		{
			// Token: 0x020034DB RID: 13531
			public class GENERIC
			{
				// Token: 0x0400DCDD RID: 56541
				public static LocString NAME = "Death";

				// Token: 0x0400DCDE RID: 56542
				public static LocString DESCRIPTION = "{Target} has died.";
			}

			// Token: 0x020034DC RID: 13532
			public class FROZEN
			{
				// Token: 0x0400DCDF RID: 56543
				public static LocString NAME = "Frozen";

				// Token: 0x0400DCE0 RID: 56544
				public static LocString DESCRIPTION = "{Target} has frozen to death.";
			}

			// Token: 0x020034DD RID: 13533
			public class SUFFOCATION
			{
				// Token: 0x0400DCE1 RID: 56545
				public static LocString NAME = "Suffocation";

				// Token: 0x0400DCE2 RID: 56546
				public static LocString DESCRIPTION = "{Target} has suffocated to death.";
			}

			// Token: 0x020034DE RID: 13534
			public class STARVATION
			{
				// Token: 0x0400DCE3 RID: 56547
				public static LocString NAME = "Starvation";

				// Token: 0x0400DCE4 RID: 56548
				public static LocString DESCRIPTION = "{Target} has starved to death.";
			}

			// Token: 0x020034DF RID: 13535
			public class OVERHEATING
			{
				// Token: 0x0400DCE5 RID: 56549
				public static LocString NAME = "Overheated";

				// Token: 0x0400DCE6 RID: 56550
				public static LocString DESCRIPTION = "{Target} overheated to death.";
			}

			// Token: 0x020034E0 RID: 13536
			public class DROWNED
			{
				// Token: 0x0400DCE7 RID: 56551
				public static LocString NAME = "Drowned";

				// Token: 0x0400DCE8 RID: 56552
				public static LocString DESCRIPTION = "{Target} has drowned.";
			}

			// Token: 0x020034E1 RID: 13537
			public class EXPLOSION
			{
				// Token: 0x0400DCE9 RID: 56553
				public static LocString NAME = "Explosion";

				// Token: 0x0400DCEA RID: 56554
				public static LocString DESCRIPTION = "{Target} has died in an explosion.";
			}

			// Token: 0x020034E2 RID: 13538
			public class COMBAT
			{
				// Token: 0x0400DCEB RID: 56555
				public static LocString NAME = "Slain";

				// Token: 0x0400DCEC RID: 56556
				public static LocString DESCRIPTION = "{Target} succumbed to their wounds after being incapacitated.";
			}

			// Token: 0x020034E3 RID: 13539
			public class FATALDISEASE
			{
				// Token: 0x0400DCED RID: 56557
				public static LocString NAME = "Succumbed to Disease";

				// Token: 0x0400DCEE RID: 56558
				public static LocString DESCRIPTION = "{Target} has died of a fatal illness.";
			}

			// Token: 0x020034E4 RID: 13540
			public class RADIATION
			{
				// Token: 0x0400DCEF RID: 56559
				public static LocString NAME = "Irradiated";

				// Token: 0x0400DCF0 RID: 56560
				public static LocString DESCRIPTION = "{Target} perished from excessive radiation exposure.";
			}

			// Token: 0x020034E5 RID: 13541
			public class HITBYHIGHENERGYPARTICLE
			{
				// Token: 0x0400DCF1 RID: 56561
				public static LocString NAME = "Struck by Radbolt";

				// Token: 0x0400DCF2 RID: 56562
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{Target} was struck by a radioactive ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" and perished."
				});
			}
		}

		// Token: 0x02002580 RID: 9600
		public class CHORES
		{
			// Token: 0x0400A986 RID: 43398
			public static LocString NOT_EXISTING_TASK = "Not Existing";

			// Token: 0x0400A987 RID: 43399
			public static LocString IS_DEAD_TASK = "Dead";

			// Token: 0x020034E6 RID: 13542
			public class THINKING
			{
				// Token: 0x0400DCF3 RID: 56563
				public static LocString NAME = "Ponder";

				// Token: 0x0400DCF4 RID: 56564
				public static LocString STATUS = "Pondering";

				// Token: 0x0400DCF5 RID: 56565
				public static LocString TOOLTIP = "This Duplicant is mulling over what they should do next";
			}

			// Token: 0x020034E7 RID: 13543
			public class ASTRONAUT
			{
				// Token: 0x0400DCF6 RID: 56566
				public static LocString NAME = "Space Mission";

				// Token: 0x0400DCF7 RID: 56567
				public static LocString STATUS = "On space mission";

				// Token: 0x0400DCF8 RID: 56568
				public static LocString TOOLTIP = "This Duplicant is exploring the vast universe";
			}

			// Token: 0x020034E8 RID: 13544
			public class DIE
			{
				// Token: 0x0400DCF9 RID: 56569
				public static LocString NAME = "Die";

				// Token: 0x0400DCFA RID: 56570
				public static LocString STATUS = "Dying";

				// Token: 0x0400DCFB RID: 56571
				public static LocString TOOLTIP = "Fare thee well, brave soul";
			}

			// Token: 0x020034E9 RID: 13545
			public class ENTOMBED
			{
				// Token: 0x0400DCFC RID: 56572
				public static LocString NAME = "Entombed";

				// Token: 0x0400DCFD RID: 56573
				public static LocString STATUS = "Entombed";

				// Token: 0x0400DCFE RID: 56574
				public static LocString TOOLTIP = "Entombed Duplicants are at risk of suffocating and must be dug out by others in the colony";
			}

			// Token: 0x020034EA RID: 13546
			public class BEINCAPACITATED
			{
				// Token: 0x0400DCFF RID: 56575
				public static LocString NAME = "Incapacitated";

				// Token: 0x0400DD00 RID: 56576
				public static LocString STATUS = "Dying";

				// Token: 0x0400DD01 RID: 56577
				public static LocString TOOLTIP = "This Duplicant will die soon if they do not receive assistance";
			}

			// Token: 0x020034EB RID: 13547
			public class BEOFFLINE
			{
				// Token: 0x0400DD02 RID: 56578
				public static LocString NAME = "Powerless";

				// Token: 0x0400DD03 RID: 56579
				public static LocString STATUS = "Powerless";

				// Token: 0x0400DD04 RID: 56580
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant does not have enough ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x020034EC RID: 13548
			public class BIONICBEDTIMEMODE
			{
				// Token: 0x0400DD05 RID: 56581
				public static LocString NAME = "Defragment";

				// Token: 0x0400DD06 RID: 56582
				public static LocString STATUS = "Defragmenting";

				// Token: 0x0400DD07 RID: 56583
				public static LocString TOOLTIP = "This Duplicant is reorganizing their data cache during bedtime";
			}

			// Token: 0x020034ED RID: 13549
			public class GENESHUFFLE
			{
				// Token: 0x0400DD08 RID: 56584
				public static LocString NAME = "Use Neural Vacillator";

				// Token: 0x0400DD09 RID: 56585
				public static LocString STATUS = "Using Neural Vacillator";

				// Token: 0x0400DD0A RID: 56586
				public static LocString TOOLTIP = "This Duplicant is being experimented on!";
			}

			// Token: 0x020034EE RID: 13550
			public class MIGRATE
			{
				// Token: 0x0400DD0B RID: 56587
				public static LocString NAME = "Use Teleporter";

				// Token: 0x0400DD0C RID: 56588
				public static LocString STATUS = "Using Teleporter";

				// Token: 0x0400DD0D RID: 56589
				public static LocString TOOLTIP = "This Duplicant's molecules are hurtling through the air!";
			}

			// Token: 0x020034EF RID: 13551
			public class DEBUGGOTO
			{
				// Token: 0x0400DD0E RID: 56590
				public static LocString NAME = "DebugGoTo";

				// Token: 0x0400DD0F RID: 56591
				public static LocString STATUS = "DebugGoTo";
			}

			// Token: 0x020034F0 RID: 13552
			public class DISINFECT
			{
				// Token: 0x0400DD10 RID: 56592
				public static LocString NAME = "Disinfect";

				// Token: 0x0400DD11 RID: 56593
				public static LocString STATUS = "Going to disinfect";

				// Token: 0x0400DD12 RID: 56594
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Buildings can be disinfected to remove contagious ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" from their surface"
				});
			}

			// Token: 0x020034F1 RID: 13553
			public class EQUIPPINGSUIT
			{
				// Token: 0x0400DD13 RID: 56595
				public static LocString NAME = "Equip Exosuit";

				// Token: 0x0400DD14 RID: 56596
				public static LocString STATUS = "Equipping exosuit";

				// Token: 0x0400DD15 RID: 56597
				public static LocString TOOLTIP = "This Duplicant is putting on protective gear";
			}

			// Token: 0x020034F2 RID: 13554
			public class STRESSIDLE
			{
				// Token: 0x0400DD16 RID: 56598
				public static LocString NAME = "Antsy";

				// Token: 0x0400DD17 RID: 56599
				public static LocString STATUS = "Antsy";

				// Token: 0x0400DD18 RID: 56600
				public static LocString TOOLTIP = "This Duplicant is a workaholic and gets stressed when they have nothing to do";
			}

			// Token: 0x020034F3 RID: 13555
			public class MOVETO
			{
				// Token: 0x0400DD19 RID: 56601
				public static LocString NAME = "Move to";

				// Token: 0x0400DD1A RID: 56602
				public static LocString STATUS = "Moving to location";

				// Token: 0x0400DD1B RID: 56603
				public static LocString TOOLTIP = "This Duplicant was manually directed to move to a specific location";
			}

			// Token: 0x020034F4 RID: 13556
			public class ROCKETENTEREXIT
			{
				// Token: 0x0400DD1C RID: 56604
				public static LocString NAME = "Rocket Recrewing";

				// Token: 0x0400DD1D RID: 56605
				public static LocString STATUS = "Recrewing Rocket";

				// Token: 0x0400DD1E RID: 56606
				public static LocString TOOLTIP = "This Duplicant is getting into (or out of) their assigned rocket";
			}

			// Token: 0x020034F5 RID: 13557
			public class DROPUNUSEDINVENTORY
			{
				// Token: 0x0400DD1F RID: 56607
				public static LocString NAME = "Drop Inventory";

				// Token: 0x0400DD20 RID: 56608
				public static LocString STATUS = "Dropping unused inventory";

				// Token: 0x0400DD21 RID: 56609
				public static LocString TOOLTIP = "This Duplicant is dropping carried items they no longer need";
			}

			// Token: 0x020034F6 RID: 13558
			public class PEE
			{
				// Token: 0x0400DD22 RID: 56610
				public static LocString NAME = "Relieve Self";

				// Token: 0x0400DD23 RID: 56611
				public static LocString STATUS = "Relieving self";

				// Token: 0x0400DD24 RID: 56612
				public static LocString TOOLTIP = "This Duplicant didn't find a toilet in time. Oops";
			}

			// Token: 0x020034F7 RID: 13559
			public class EXPELLGUNK
			{
				// Token: 0x0400DD25 RID: 56613
				public static LocString NAME = "Expel Gunk";

				// Token: 0x0400DD26 RID: 56614
				public static LocString STATUS = "Expelling gunk";

				// Token: 0x0400DD27 RID: 56615
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant didn't get to a ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" in time. Urgh"
				});
			}

			// Token: 0x020034F8 RID: 13560
			public class OILCHANGE
			{
				// Token: 0x0400DD28 RID: 56616
				public static LocString NAME = "Refill Oil";

				// Token: 0x0400DD29 RID: 56617
				public static LocString STATUS = "Refilling oil";

				// Token: 0x0400DD2A RID: 56618
				public static LocString TOOLTIP = "This Duplicant is making sure their internal mechanisms stay lubricated";
			}

			// Token: 0x020034F9 RID: 13561
			public class BREAK_PEE
			{
				// Token: 0x0400DD2B RID: 56619
				public static LocString NAME = "Downtime: Use Toilet";

				// Token: 0x0400DD2C RID: 56620
				public static LocString STATUS = "Downtime: Going to use toilet";

				// Token: 0x0400DD2D RID: 56621
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has scheduled ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" and is using their break to go to the toilet\n\nDuplicants have to use the toilet at least once per day"
				});
			}

			// Token: 0x020034FA RID: 13562
			public class STRESSVOMIT
			{
				// Token: 0x0400DD2E RID: 56622
				public static LocString NAME = "Stress Vomit";

				// Token: 0x0400DD2F RID: 56623
				public static LocString STATUS = "Stress vomiting";

				// Token: 0x0400DD30 RID: 56624
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Some people deal with ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" better than others"
				});
			}

			// Token: 0x020034FB RID: 13563
			public class UGLY_CRY
			{
				// Token: 0x0400DD31 RID: 56625
				public static LocString NAME = "Ugly Cry";

				// Token: 0x0400DD32 RID: 56626
				public static LocString STATUS = "Ugly crying";

				// Token: 0x0400DD33 RID: 56627
				public static LocString TOOLTIP = "This Duplicant is having a healthy cry to alleviate their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x020034FC RID: 13564
			public class STRESSSHOCK
			{
				// Token: 0x0400DD34 RID: 56628
				public static LocString NAME = "Shock";

				// Token: 0x0400DD35 RID: 56629
				public static LocString STATUS = "Shocking";

				// Token: 0x0400DD36 RID: 56630
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's inability to handle ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is pretty shocking"
				});
			}

			// Token: 0x020034FD RID: 13565
			public class BINGE_EAT
			{
				// Token: 0x0400DD37 RID: 56631
				public static LocString NAME = "Binge Eat";

				// Token: 0x0400DD38 RID: 56632
				public static LocString STATUS = "Binge eating";

				// Token: 0x0400DD39 RID: 56633
				public static LocString TOOLTIP = "This Duplicant is attempting to eat their emotions due to " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x020034FE RID: 13566
			public class BANSHEE_WAIL
			{
				// Token: 0x0400DD3A RID: 56634
				public static LocString NAME = "Banshee Wail";

				// Token: 0x0400DD3B RID: 56635
				public static LocString STATUS = "Wailing";

				// Token: 0x0400DD3C RID: 56636
				public static LocString TOOLTIP = "This Duplicant is emitting ear-piercing shrieks to relieve pent-up " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x020034FF RID: 13567
			public class EMOTEHIGHPRIORITY
			{
				// Token: 0x0400DD3D RID: 56637
				public static LocString NAME = "Express Themselves";

				// Token: 0x0400DD3E RID: 56638
				public static LocString STATUS = "Expressing themselves";

				// Token: 0x0400DD3F RID: 56639
				public static LocString TOOLTIP = "This Duplicant needs a moment to express their feelings, then they'll be on their way";
			}

			// Token: 0x02003500 RID: 13568
			public class HUG
			{
				// Token: 0x0400DD40 RID: 56640
				public static LocString NAME = "Hug";

				// Token: 0x0400DD41 RID: 56641
				public static LocString STATUS = "Hugging";

				// Token: 0x0400DD42 RID: 56642
				public static LocString TOOLTIP = "This Duplicant is enjoying a big warm hug";
			}

			// Token: 0x02003501 RID: 13569
			public class FLEE
			{
				// Token: 0x0400DD43 RID: 56643
				public static LocString NAME = "Flee";

				// Token: 0x0400DD44 RID: 56644
				public static LocString STATUS = "Fleeing";

				// Token: 0x0400DD45 RID: 56645
				public static LocString TOOLTIP = "Run away!";
			}

			// Token: 0x02003502 RID: 13570
			public class RECOVERBREATH
			{
				// Token: 0x0400DD46 RID: 56646
				public static LocString NAME = "Recover Breath";

				// Token: 0x0400DD47 RID: 56647
				public static LocString STATUS = "Recovering breath";

				// Token: 0x0400DD48 RID: 56648
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003503 RID: 13571
			public class RECOVERFROMHEAT
			{
				// Token: 0x0400DD49 RID: 56649
				public static LocString NAME = "Recover from Heat";

				// Token: 0x0400DD4A RID: 56650
				public static LocString STATUS = "Recovering from heat";

				// Token: 0x0400DD4B RID: 56651
				public static LocString TOOLTIP = "This Duplicant's trying to cool down";
			}

			// Token: 0x02003504 RID: 13572
			public class RECOVERWARMTH
			{
				// Token: 0x0400DD4C RID: 56652
				public static LocString NAME = "Recover from Cold";

				// Token: 0x0400DD4D RID: 56653
				public static LocString STATUS = "Recovering from cold";

				// Token: 0x0400DD4E RID: 56654
				public static LocString TOOLTIP = "This Duplicant's trying to warm up";
			}

			// Token: 0x02003505 RID: 13573
			public class MOVETOQUARANTINE
			{
				// Token: 0x0400DD4F RID: 56655
				public static LocString NAME = "Move to Quarantine";

				// Token: 0x0400DD50 RID: 56656
				public static LocString STATUS = "Moving to quarantine";

				// Token: 0x0400DD51 RID: 56657
				public static LocString TOOLTIP = "This Duplicant will isolate themselves to keep their illness away from the colony";
			}

			// Token: 0x02003506 RID: 13574
			public class ATTACK
			{
				// Token: 0x0400DD52 RID: 56658
				public static LocString NAME = "Attack";

				// Token: 0x0400DD53 RID: 56659
				public static LocString STATUS = "Attacking";

				// Token: 0x0400DD54 RID: 56660
				public static LocString TOOLTIP = "Chaaaarge!";
			}

			// Token: 0x02003507 RID: 13575
			public class CAPTURE
			{
				// Token: 0x0400DD55 RID: 56661
				public static LocString NAME = "Wrangle";

				// Token: 0x0400DD56 RID: 56662
				public static LocString STATUS = "Wrangling";

				// Token: 0x0400DD57 RID: 56663
				public static LocString TOOLTIP = "Duplicants that possess the Critter Ranching Skill can wrangle most critters without traps";
			}

			// Token: 0x02003508 RID: 13576
			public class SINGTOEGG
			{
				// Token: 0x0400DD58 RID: 56664
				public static LocString NAME = "Sing To Egg";

				// Token: 0x0400DD59 RID: 56665
				public static LocString STATUS = "Singing to egg";

				// Token: 0x0400DD5A RID: 56666
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A gentle lullaby from a supportive Duplicant encourages developing ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					"\n\nIncreases ",
					UI.PRE_KEYWORD,
					"Incubation Rate",
					UI.PST_KEYWORD,
					"\n\nDuplicants must possess the ",
					DUPLICANTS.ROLES.RANCHER.NAME,
					" skill to sing to an egg"
				});
			}

			// Token: 0x02003509 RID: 13577
			public class USETOILET
			{
				// Token: 0x0400DD5B RID: 56667
				public static LocString NAME = "Use Toilet";

				// Token: 0x0400DD5C RID: 56668
				public static LocString STATUS = "Going to use toilet";

				// Token: 0x0400DD5D RID: 56669
				public static LocString TOOLTIP = "Duplicants have to use the toilet at least once per day";
			}

			// Token: 0x0200350A RID: 13578
			public class WASHHANDS
			{
				// Token: 0x0400DD5E RID: 56670
				public static LocString NAME = "Wash Hands";

				// Token: 0x0400DD5F RID: 56671
				public static LocString STATUS = "Washing hands";

				// Token: 0x0400DD60 RID: 56672
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Good hygiene removes ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" and prevents the spread of ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200350B RID: 13579
			public class SLIP
			{
				// Token: 0x0400DD61 RID: 56673
				public static LocString NAME = "Slip";

				// Token: 0x0400DD62 RID: 56674
				public static LocString STATUS = "Slipping";

				// Token: 0x0400DD63 RID: 56675
				public static LocString TOOLTIP = "Slippery surfaces can cause Duplicants to fall \"seat over tea kettle\"";
			}

			// Token: 0x0200350C RID: 13580
			public class CHECKPOINT
			{
				// Token: 0x0400DD64 RID: 56676
				public static LocString NAME = "Wait at Checkpoint";

				// Token: 0x0400DD65 RID: 56677
				public static LocString STATUS = "Waiting at Checkpoint";

				// Token: 0x0400DD66 RID: 56678
				public static LocString TOOLTIP = "This Duplicant is waiting for permission to pass";
			}

			// Token: 0x0200350D RID: 13581
			public class TRAVELTUBEENTRANCE
			{
				// Token: 0x0400DD67 RID: 56679
				public static LocString NAME = "Enter Transit Tube";

				// Token: 0x0400DD68 RID: 56680
				public static LocString STATUS = "Entering Transit Tube";

				// Token: 0x0400DD69 RID: 56681
				public static LocString TOOLTIP = "Nyoooom!";
			}

			// Token: 0x0200350E RID: 13582
			public class SCRUBORE
			{
				// Token: 0x0400DD6A RID: 56682
				public static LocString NAME = "Scrub Ore";

				// Token: 0x0400DD6B RID: 56683
				public static LocString STATUS = "Scrubbing ore";

				// Token: 0x0400DD6C RID: 56684
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material ore can be scrubbed to remove ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" present on its surface"
				});
			}

			// Token: 0x0200350F RID: 13583
			public class EAT
			{
				// Token: 0x0400DD6D RID: 56685
				public static LocString NAME = "Eat";

				// Token: 0x0400DD6E RID: 56686
				public static LocString STATUS = "Going to eat";

				// Token: 0x0400DD6F RID: 56687
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants eat to replenish their ",
					UI.PRE_KEYWORD,
					"Calorie",
					UI.PST_KEYWORD,
					" stores"
				});
			}

			// Token: 0x02003510 RID: 13584
			public class RELOADELECTROBANK
			{
				// Token: 0x0400DD70 RID: 56688
				public static LocString NAME = "Power Up";

				// Token: 0x0400DD71 RID: 56689
				public static LocString STATUS = "Looking for power banks";

				// Token: 0x0400DD72 RID: 56690
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants need ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x02003511 RID: 13585
			public class FINDOXYGENSOURCEITEM
			{
				// Token: 0x0400DD73 RID: 56691
				public static LocString NAME = "Seek Oxygen Refill";

				// Token: 0x0400DD74 RID: 56692
				public static LocString STATUS = "Looking for oxygen refills";

				// Token: 0x0400DD75 RID: 56693
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants are fitted with internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tanks that must be refilled"
				});
			}

			// Token: 0x02003512 RID: 13586
			public class BIONICABSORBOXYGEN
			{
				// Token: 0x0400DD76 RID: 56694
				public static LocString NAME = "Refill Oxygen Tank";

				// Token: 0x0400DD77 RID: 56695
				public static LocString STATUS = "Refilling oxygen tank";

				// Token: 0x0400DD78 RID: 56696
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is refilling their internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tank: {0} O<sub>2</sub>\n\nBionic Duplicants automatically refill their internal tanks in highly breathable areas during scheduled ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003513 RID: 13587
			public class BIONICABSORBOXYGENCRITICAL
			{
				// Token: 0x0400DD79 RID: 56697
				public static LocString NAME = "Urgent Oxygen Refill";

				// Token: 0x0400DD7A RID: 56698
				public static LocString STATUS = "Urgently refilling oxygen tank";

				// Token: 0x0400DD7B RID: 56699
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is refilling their depleted ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tank in the nearest breathable area: {0} O<sub>2</sub>\n\nImproving colony breathability and scheduling regular ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" will prevent future emergencies"
				});
			}

			// Token: 0x02003514 RID: 13588
			public class UNLOADELECTROBANK
			{
				// Token: 0x0400DD7C RID: 56700
				public static LocString NAME = "Offload";

				// Token: 0x0400DD7D RID: 56701
				public static LocString STATUS = "Offloading empty power banks";

				// Token: 0x0400DD7E RID: 56702
				public static LocString TOOLTIP = "Bionic Duplicants automatically offload depleted " + UI.PRE_KEYWORD + "Power Banks" + UI.PST_KEYWORD;
			}

			// Token: 0x02003515 RID: 13589
			public class SEEKANDINSTALLUPGRADE
			{
				// Token: 0x0400DD7F RID: 56703
				public static LocString NAME = "Retrieve Booster";

				// Token: 0x0400DD80 RID: 56704
				public static LocString STATUS = "Retrieving booster";

				// Token: 0x0400DD81 RID: 56705
				public static LocString TOOLTIP = "This Duplicant is on its way to retrieve a booster that was assigned to them";
			}

			// Token: 0x02003516 RID: 13590
			public class VOMIT
			{
				// Token: 0x0400DD82 RID: 56706
				public static LocString NAME = "Vomit";

				// Token: 0x0400DD83 RID: 56707
				public static LocString STATUS = "Vomiting";

				// Token: 0x0400DD84 RID: 56708
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Vomiting produces ",
					ELEMENTS.DIRTYWATER.NAME,
					" and can spread ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003517 RID: 13591
			public class RADIATIONPAIN
			{
				// Token: 0x0400DD85 RID: 56709
				public static LocString NAME = "Radiation Aches";

				// Token: 0x0400DD86 RID: 56710
				public static LocString STATUS = "Feeling radiation aches";

				// Token: 0x0400DD87 RID: 56711
				public static LocString TOOLTIP = "Radiation Aches are a symptom of " + DUPLICANTS.DISEASES.RADIATIONSICKNESS.NAME;
			}

			// Token: 0x02003518 RID: 13592
			public class COUGH
			{
				// Token: 0x0400DD88 RID: 56712
				public static LocString NAME = "Cough";

				// Token: 0x0400DD89 RID: 56713
				public static LocString STATUS = "Coughing";

				// Token: 0x0400DD8A RID: 56714
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Coughing is a symptom of ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" and spreads airborne ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003519 RID: 13593
			public class WATERDAMAGEZAP
			{
				// Token: 0x0400DD8B RID: 56715
				public static LocString NAME = "Glitch";

				// Token: 0x0400DD8C RID: 56716
				public static LocString STATUS = "Glitching";

				// Token: 0x0400DD8D RID: 56717
				public static LocString TOOLTIP = "Glitching is a symptom of Bionic Duplicant systems malfunctioning due to contact with incompatible " + UI.PRE_KEYWORD + "Liquids" + UI.PST_KEYWORD;
			}

			// Token: 0x0200351A RID: 13594
			public class SLEEP
			{
				// Token: 0x0400DD8E RID: 56718
				public static LocString NAME = "Sleep";

				// Token: 0x0400DD8F RID: 56719
				public static LocString STATUS = "Sleeping";

				// Token: 0x0400DD90 RID: 56720
				public static LocString TOOLTIP = "Zzzzzz...";
			}

			// Token: 0x0200351B RID: 13595
			public class NARCOLEPSY
			{
				// Token: 0x0400DD91 RID: 56721
				public static LocString NAME = "Narcoleptic Nap";

				// Token: 0x0400DD92 RID: 56722
				public static LocString STATUS = "Narcoleptic napping";

				// Token: 0x0400DD93 RID: 56723
				public static LocString TOOLTIP = "Zzzzzz...";
			}

			// Token: 0x0200351C RID: 13596
			public class FLOORSLEEP
			{
				// Token: 0x0400DD94 RID: 56724
				public static LocString NAME = "Sleep on Floor";

				// Token: 0x0400DD95 RID: 56725
				public static LocString STATUS = "Sleeping on floor";

				// Token: 0x0400DD96 RID: 56726
				public static LocString TOOLTIP = "Zzzzzz...\n\nSleeping on the floor will give Duplicants a " + DUPLICANTS.MODIFIERS.SOREBACK.NAME;
			}

			// Token: 0x0200351D RID: 13597
			public class TAKEMEDICINE
			{
				// Token: 0x0400DD97 RID: 56727
				public static LocString NAME = "Take Medicine";

				// Token: 0x0400DD98 RID: 56728
				public static LocString STATUS = "Taking medicine";

				// Token: 0x0400DD99 RID: 56729
				public static LocString TOOLTIP = "This Duplicant is taking a dose of medicine to ward off " + UI.PRE_KEYWORD + "Disease" + UI.PST_KEYWORD;
			}

			// Token: 0x0200351E RID: 13598
			public class GETDOCTORED
			{
				// Token: 0x0400DD9A RID: 56730
				public static LocString NAME = "Visit Doctor";

				// Token: 0x0400DD9B RID: 56731
				public static LocString STATUS = "Visiting doctor";

				// Token: 0x0400DD9C RID: 56732
				public static LocString TOOLTIP = "This Duplicant is visiting a doctor to receive treatment";
			}

			// Token: 0x0200351F RID: 13599
			public class DOCTOR
			{
				// Token: 0x0400DD9D RID: 56733
				public static LocString NAME = "Treat Patient";

				// Token: 0x0400DD9E RID: 56734
				public static LocString STATUS = "Treating patient";

				// Token: 0x0400DD9F RID: 56735
				public static LocString TOOLTIP = "This Duplicant is trying to make one of their peers feel better";
			}

			// Token: 0x02003520 RID: 13600
			public class DELIVERFOOD
			{
				// Token: 0x0400DDA0 RID: 56736
				public static LocString NAME = "Deliver Food";

				// Token: 0x0400DDA1 RID: 56737
				public static LocString STATUS = "Delivering food";

				// Token: 0x0400DDA2 RID: 56738
				public static LocString TOOLTIP = "Under thirty minutes or it's free";
			}

			// Token: 0x02003521 RID: 13601
			public class SHOWER
			{
				// Token: 0x0400DDA3 RID: 56739
				public static LocString NAME = "Shower";

				// Token: 0x0400DDA4 RID: 56740
				public static LocString STATUS = "Showering";

				// Token: 0x0400DDA5 RID: 56741
				public static LocString TOOLTIP = "This Duplicant is having a refreshing shower";
			}

			// Token: 0x02003522 RID: 13602
			public class SIGH
			{
				// Token: 0x0400DDA6 RID: 56742
				public static LocString NAME = "Sigh";

				// Token: 0x0400DDA7 RID: 56743
				public static LocString STATUS = "Sighing";

				// Token: 0x0400DDA8 RID: 56744
				public static LocString TOOLTIP = "Ho-hum.";
			}

			// Token: 0x02003523 RID: 13603
			public class RESTDUETODISEASE
			{
				// Token: 0x0400DDA9 RID: 56745
				public static LocString NAME = "Rest";

				// Token: 0x0400DDAA RID: 56746
				public static LocString STATUS = "Resting";

				// Token: 0x0400DDAB RID: 56747
				public static LocString TOOLTIP = "This Duplicant isn't feeling well and is taking a rest";
			}

			// Token: 0x02003524 RID: 13604
			public class HEAL
			{
				// Token: 0x0400DDAC RID: 56748
				public static LocString NAME = "Heal";

				// Token: 0x0400DDAD RID: 56749
				public static LocString STATUS = "Healing";

				// Token: 0x0400DDAE RID: 56750
				public static LocString TOOLTIP = "This Duplicant is taking some time to recover from their wounds";
			}

			// Token: 0x02003525 RID: 13605
			public class STRESSACTINGOUT
			{
				// Token: 0x0400DDAF RID: 56751
				public static LocString NAME = "Lash Out";

				// Token: 0x0400DDB0 RID: 56752
				public static LocString STATUS = "Lashing out";

				// Token: 0x0400DDB1 RID: 56753
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is having a ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					"-induced tantrum"
				});
			}

			// Token: 0x02003526 RID: 13606
			public class RELAX
			{
				// Token: 0x0400DDB2 RID: 56754
				public static LocString NAME = "Relax";

				// Token: 0x0400DDB3 RID: 56755
				public static LocString STATUS = "Relaxing";

				// Token: 0x0400DDB4 RID: 56756
				public static LocString TOOLTIP = "This Duplicant is taking it easy";
			}

			// Token: 0x02003527 RID: 13607
			public class STRESSHEAL
			{
				// Token: 0x0400DDB5 RID: 56757
				public static LocString NAME = "De-Stress";

				// Token: 0x0400DDB6 RID: 56758
				public static LocString STATUS = "De-stressing";

				// Token: 0x0400DDB7 RID: 56759
				public static LocString TOOLTIP = "This Duplicant taking some time to recover from their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003528 RID: 13608
			public class EQUIP
			{
				// Token: 0x0400DDB8 RID: 56760
				public static LocString NAME = "Equip";

				// Token: 0x0400DDB9 RID: 56761
				public static LocString STATUS = "Moving to equip";

				// Token: 0x0400DDBA RID: 56762
				public static LocString TOOLTIP = "This Duplicant is putting on a piece of equipment";
			}

			// Token: 0x02003529 RID: 13609
			public class LEARNSKILL
			{
				// Token: 0x0400DDBB RID: 56763
				public static LocString NAME = "Learn Skill";

				// Token: 0x0400DDBC RID: 56764
				public static LocString STATUS = "Learning skill";

				// Token: 0x0400DDBD RID: 56765
				public static LocString TOOLTIP = "This Duplicant is learning a new " + UI.PRE_KEYWORD + "Skill" + UI.PST_KEYWORD;
			}

			// Token: 0x0200352A RID: 13610
			public class UNLEARNSKILL
			{
				// Token: 0x0400DDBE RID: 56766
				public static LocString NAME = "Unlearn Skills";

				// Token: 0x0400DDBF RID: 56767
				public static LocString STATUS = "Unlearning skills";

				// Token: 0x0400DDC0 RID: 56768
				public static LocString TOOLTIP = "This Duplicant is unlearning " + UI.PRE_KEYWORD + "Skills" + UI.PST_KEYWORD;
			}

			// Token: 0x0200352B RID: 13611
			public class RECHARGE
			{
				// Token: 0x0400DDC1 RID: 56769
				public static LocString NAME = "Recharge Equipment";

				// Token: 0x0400DDC2 RID: 56770
				public static LocString STATUS = "Recharging equipment";

				// Token: 0x0400DDC3 RID: 56771
				public static LocString TOOLTIP = "This Duplicant is recharging their equipment";
			}

			// Token: 0x0200352C RID: 13612
			public class UNEQUIP
			{
				// Token: 0x0400DDC4 RID: 56772
				public static LocString NAME = "Unequip";

				// Token: 0x0400DDC5 RID: 56773
				public static LocString STATUS = "Moving to unequip";

				// Token: 0x0400DDC6 RID: 56774
				public static LocString TOOLTIP = "This Duplicant is removing a piece of their equipment";
			}

			// Token: 0x0200352D RID: 13613
			public class MOURN
			{
				// Token: 0x0400DDC7 RID: 56775
				public static LocString NAME = "Mourn";

				// Token: 0x0400DDC8 RID: 56776
				public static LocString STATUS = "Mourning";

				// Token: 0x0400DDC9 RID: 56777
				public static LocString TOOLTIP = "This Duplicant is mourning the loss of a friend";
			}

			// Token: 0x0200352E RID: 13614
			public class WARMUP
			{
				// Token: 0x0400DDCA RID: 56778
				public static LocString NAME = "Warm Up";

				// Token: 0x0400DDCB RID: 56779
				public static LocString STATUS = "Going to warm up";

				// Token: 0x0400DDCC RID: 56780
				public static LocString TOOLTIP = "This Duplicant got too cold and is going somewhere to warm up";
			}

			// Token: 0x0200352F RID: 13615
			public class COOLDOWN
			{
				// Token: 0x0400DDCD RID: 56781
				public static LocString NAME = "Cool Off";

				// Token: 0x0400DDCE RID: 56782
				public static LocString STATUS = "Going to cool off";

				// Token: 0x0400DDCF RID: 56783
				public static LocString TOOLTIP = "This Duplicant got too hot and is going somewhere to cool off";
			}

			// Token: 0x02003530 RID: 13616
			public class EMPTYSTORAGE
			{
				// Token: 0x0400DDD0 RID: 56784
				public static LocString NAME = "Empty Storage";

				// Token: 0x0400DDD1 RID: 56785
				public static LocString STATUS = "Going to empty storage";

				// Token: 0x0400DDD2 RID: 56786
				public static LocString TOOLTIP = "This Duplicant is taking items out of storage";
			}

			// Token: 0x02003531 RID: 13617
			public class ART
			{
				// Token: 0x0400DDD3 RID: 56787
				public static LocString NAME = "Decorate";

				// Token: 0x0400DDD4 RID: 56788
				public static LocString STATUS = "Going to decorate";

				// Token: 0x0400DDD5 RID: 56789
				public static LocString TOOLTIP = "This Duplicant is going to work on their art";
			}

			// Token: 0x02003532 RID: 13618
			public class MOP
			{
				// Token: 0x0400DDD6 RID: 56790
				public static LocString NAME = "Mop";

				// Token: 0x0400DDD7 RID: 56791
				public static LocString STATUS = "Going to mop";

				// Token: 0x0400DDD8 RID: 56792
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Mopping removes ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" from the floor and bottles them for transport"
				});
			}

			// Token: 0x02003533 RID: 13619
			public class RELOCATE
			{
				// Token: 0x0400DDD9 RID: 56793
				public static LocString NAME = "Relocate";

				// Token: 0x0400DDDA RID: 56794
				public static LocString STATUS = "Going to relocate";

				// Token: 0x0400DDDB RID: 56795
				public static LocString TOOLTIP = "This Duplicant is moving a building to a new location";
			}

			// Token: 0x02003534 RID: 13620
			public class TOGGLE
			{
				// Token: 0x0400DDDC RID: 56796
				public static LocString NAME = "Change Setting";

				// Token: 0x0400DDDD RID: 56797
				public static LocString STATUS = "Going to change setting";

				// Token: 0x0400DDDE RID: 56798
				public static LocString TOOLTIP = "This Duplicant is going to change the settings on a building";
			}

			// Token: 0x02003535 RID: 13621
			public class RESCUEINCAPACITATED
			{
				// Token: 0x0400DDDF RID: 56799
				public static LocString NAME = "Rescue Friend";

				// Token: 0x0400DDE0 RID: 56800
				public static LocString STATUS = "Rescuing friend";

				// Token: 0x0400DDE1 RID: 56801
				public static LocString TOOLTIP = "This Duplicant is rescuing another Duplicant that has been incapacitated";
			}

			// Token: 0x02003536 RID: 13622
			public class REPAIR
			{
				// Token: 0x0400DDE2 RID: 56802
				public static LocString NAME = "Repair";

				// Token: 0x0400DDE3 RID: 56803
				public static LocString STATUS = "Going to repair";

				// Token: 0x0400DDE4 RID: 56804
				public static LocString TOOLTIP = "This Duplicant is fixing a broken building";
			}

			// Token: 0x02003537 RID: 13623
			public class DECONSTRUCT
			{
				// Token: 0x0400DDE5 RID: 56805
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400DDE6 RID: 56806
				public static LocString STATUS = "Going to deconstruct";

				// Token: 0x0400DDE7 RID: 56807
				public static LocString TOOLTIP = "This Duplicant is deconstructing a building";
			}

			// Token: 0x02003538 RID: 13624
			public class RESEARCH
			{
				// Token: 0x0400DDE8 RID: 56808
				public static LocString NAME = "Research";

				// Token: 0x0400DDE9 RID: 56809
				public static LocString STATUS = "Going to research";

				// Token: 0x0400DDEA RID: 56810
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is working on the current ",
					UI.PRE_KEYWORD,
					"Research",
					UI.PST_KEYWORD,
					" focus"
				});
			}

			// Token: 0x02003539 RID: 13625
			public class ANALYZEARTIFACT
			{
				// Token: 0x0400DDEB RID: 56811
				public static LocString NAME = "Artifact Analysis";

				// Token: 0x0400DDEC RID: 56812
				public static LocString STATUS = "Going to analyze artifacts";

				// Token: 0x0400DDED RID: 56813
				public static LocString TOOLTIP = "This Duplicant is analyzing " + UI.PRE_KEYWORD + "Artifacts" + UI.PST_KEYWORD;
			}

			// Token: 0x0200353A RID: 13626
			public class ANALYZESEED
			{
				// Token: 0x0400DDEE RID: 56814
				public static LocString NAME = "Seed Analysis";

				// Token: 0x0400DDEF RID: 56815
				public static LocString STATUS = "Going to analyze seeds";

				// Token: 0x0400DDF0 RID: 56816
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is analyzing ",
					UI.PRE_KEYWORD,
					"Seeds",
					UI.PST_KEYWORD,
					" to find mutations"
				});
			}

			// Token: 0x0200353B RID: 13627
			public class RETURNSUIT
			{
				// Token: 0x0400DDF1 RID: 56817
				public static LocString NAME = "Dock Exosuit";

				// Token: 0x0400DDF2 RID: 56818
				public static LocString STATUS = "Docking exosuit";

				// Token: 0x0400DDF3 RID: 56819
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is plugging an ",
					UI.PRE_KEYWORD,
					"Exosuit",
					UI.PST_KEYWORD,
					" in for refilling"
				});
			}

			// Token: 0x0200353C RID: 13628
			public class GENERATEPOWER
			{
				// Token: 0x0400DDF4 RID: 56820
				public static LocString NAME = "Generate Power";

				// Token: 0x0400DDF5 RID: 56821
				public static LocString STATUS = "Going to generate power";

				// Token: 0x0400DDF6 RID: 56822
				public static LocString TOOLTIP = "This Duplicant is producing electrical " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
			}

			// Token: 0x0200353D RID: 13629
			public class HARVEST
			{
				// Token: 0x0400DDF7 RID: 56823
				public static LocString NAME = "Harvest";

				// Token: 0x0400DDF8 RID: 56824
				public static LocString STATUS = "Going to harvest";

				// Token: 0x0400DDF9 RID: 56825
				public static LocString TOOLTIP = "This Duplicant is harvesting usable materials from a mature " + UI.PRE_KEYWORD + "Plant" + UI.PST_KEYWORD;
			}

			// Token: 0x0200353E RID: 13630
			public class UPROOT
			{
				// Token: 0x0400DDFA RID: 56826
				public static LocString NAME = "Uproot";

				// Token: 0x0400DDFB RID: 56827
				public static LocString STATUS = "Going to uproot";

				// Token: 0x0400DDFC RID: 56828
				public static LocString TOOLTIP = "This Duplicant is uprooting a plant to retrieve a " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;
			}

			// Token: 0x0200353F RID: 13631
			public class CLEANTOILET
			{
				// Token: 0x0400DDFD RID: 56829
				public static LocString NAME = "Clean Outhouse";

				// Token: 0x0400DDFE RID: 56830
				public static LocString STATUS = "Going to clean";

				// Token: 0x0400DDFF RID: 56831
				public static LocString TOOLTIP = "This Duplicant is cleaning out the " + BUILDINGS.PREFABS.OUTHOUSE.NAME;
			}

			// Token: 0x02003540 RID: 13632
			public class EMPTYDESALINATOR
			{
				// Token: 0x0400DE00 RID: 56832
				public static LocString NAME = "Empty Desalinator";

				// Token: 0x0400DE01 RID: 56833
				public static LocString STATUS = "Going to clean";

				// Token: 0x0400DE02 RID: 56834
				public static LocString TOOLTIP = "This Duplicant is emptying out the " + BUILDINGS.PREFABS.DESALINATOR.NAME;
			}

			// Token: 0x02003541 RID: 13633
			public class LIQUIDCOOLEDFAN
			{
				// Token: 0x0400DE03 RID: 56835
				public static LocString NAME = "Use Fan";

				// Token: 0x0400DE04 RID: 56836
				public static LocString STATUS = "Going to use fan";

				// Token: 0x0400DE05 RID: 56837
				public static LocString TOOLTIP = "This Duplicant is attempting to cool down the area";
			}

			// Token: 0x02003542 RID: 13634
			public class ICECOOLEDFAN
			{
				// Token: 0x0400DE06 RID: 56838
				public static LocString NAME = "Use Fan";

				// Token: 0x0400DE07 RID: 56839
				public static LocString STATUS = "Going to use fan";

				// Token: 0x0400DE08 RID: 56840
				public static LocString TOOLTIP = "This Duplicant is attempting to cool down the area";
			}

			// Token: 0x02003543 RID: 13635
			public class PROCESSCRITTER
			{
				// Token: 0x0400DE09 RID: 56841
				public static LocString NAME = "Process Critter";

				// Token: 0x0400DE0A RID: 56842
				public static LocString STATUS = "Going to process critter";

				// Token: 0x0400DE0B RID: 56843
				public static LocString TOOLTIP = "This Duplicant is processing " + UI.PRE_KEYWORD + "Critters" + UI.PST_KEYWORD;
			}

			// Token: 0x02003544 RID: 13636
			public class COOK
			{
				// Token: 0x0400DE0C RID: 56844
				public static LocString NAME = "Cook";

				// Token: 0x0400DE0D RID: 56845
				public static LocString STATUS = "Going to cook";

				// Token: 0x0400DE0E RID: 56846
				public static LocString TOOLTIP = "This Duplicant is cooking " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02003545 RID: 13637
			public class COMPOUND
			{
				// Token: 0x0400DE0F RID: 56847
				public static LocString NAME = "Compound Medicine";

				// Token: 0x0400DE10 RID: 56848
				public static LocString STATUS = "Going to compound medicine";

				// Token: 0x0400DE11 RID: 56849
				public static LocString TOOLTIP = "This Duplicant is fabricating " + UI.PRE_KEYWORD + "Medicine" + UI.PST_KEYWORD;
			}

			// Token: 0x02003546 RID: 13638
			public class TRAIN
			{
				// Token: 0x0400DE12 RID: 56850
				public static LocString NAME = "Train";

				// Token: 0x0400DE13 RID: 56851
				public static LocString STATUS = "Training";

				// Token: 0x0400DE14 RID: 56852
				public static LocString TOOLTIP = "This Duplicant is busy training";
			}

			// Token: 0x02003547 RID: 13639
			public class MUSH
			{
				// Token: 0x0400DE15 RID: 56853
				public static LocString NAME = "Mush";

				// Token: 0x0400DE16 RID: 56854
				public static LocString STATUS = "Going to mush";

				// Token: 0x0400DE17 RID: 56855
				public static LocString TOOLTIP = "This Duplicant is producing " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02003548 RID: 13640
			public class COMPOSTWORKABLE
			{
				// Token: 0x0400DE18 RID: 56856
				public static LocString NAME = "Compost";

				// Token: 0x0400DE19 RID: 56857
				public static LocString STATUS = "Going to compost";

				// Token: 0x0400DE1A RID: 56858
				public static LocString TOOLTIP = "This Duplicant is dropping off organic material at the " + BUILDINGS.PREFABS.COMPOST.NAME;
			}

			// Token: 0x02003549 RID: 13641
			public class FLIPCOMPOST
			{
				// Token: 0x0400DE1B RID: 56859
				public static LocString NAME = "Flip";

				// Token: 0x0400DE1C RID: 56860
				public static LocString STATUS = "Going to flip compost";

				// Token: 0x0400DE1D RID: 56861
				public static LocString TOOLTIP = BUILDINGS.PREFABS.COMPOST.NAME + "s need to be flipped in order for their contents to compost";
			}

			// Token: 0x0200354A RID: 13642
			public class DEPRESSURIZE
			{
				// Token: 0x0400DE1E RID: 56862
				public static LocString NAME = "Depressurize Well";

				// Token: 0x0400DE1F RID: 56863
				public static LocString STATUS = "Going to depressurize well";

				// Token: 0x0400DE20 RID: 56864
				public static LocString TOOLTIP = BUILDINGS.PREFABS.OILWELLCAP.NAME + "s need to be periodically depressurized to function";
			}

			// Token: 0x0200354B RID: 13643
			public class FABRICATE
			{
				// Token: 0x0400DE21 RID: 56865
				public static LocString NAME = "Fabricate";

				// Token: 0x0400DE22 RID: 56866
				public static LocString STATUS = "Going to fabricate";

				// Token: 0x0400DE23 RID: 56867
				public static LocString TOOLTIP = "This Duplicant is crafting something";
			}

			// Token: 0x0200354C RID: 13644
			public class BUILD
			{
				// Token: 0x0400DE24 RID: 56868
				public static LocString NAME = "Build";

				// Token: 0x0400DE25 RID: 56869
				public static LocString STATUS = "Going to build";

				// Token: 0x0400DE26 RID: 56870
				public static LocString TOOLTIP = "This Duplicant is constructing a new building";
			}

			// Token: 0x0200354D RID: 13645
			public class BUILDDIG
			{
				// Token: 0x0400DE27 RID: 56871
				public static LocString NAME = "Construction Dig";

				// Token: 0x0400DE28 RID: 56872
				public static LocString STATUS = "Going to construction dig";

				// Token: 0x0400DE29 RID: 56873
				public static LocString TOOLTIP = "This Duplicant is making room for a planned construction task by performing this dig";
			}

			// Token: 0x0200354E RID: 13646
			public class BUILDUPROOT
			{
				// Token: 0x0400DE2A RID: 56874
				public static LocString NAME = "Construction Uproot";

				// Token: 0x0400DE2B RID: 56875
				public static LocString STATUS = "Going to construction uproot";

				// Token: 0x0400DE2C RID: 56876
				public static LocString TOOLTIP = "This Duplicant is making room for a planned construction task by uprooting a plant";
			}

			// Token: 0x0200354F RID: 13647
			public class DIG
			{
				// Token: 0x0400DE2D RID: 56877
				public static LocString NAME = "Dig";

				// Token: 0x0400DE2E RID: 56878
				public static LocString STATUS = "Going to dig";

				// Token: 0x0400DE2F RID: 56879
				public static LocString TOOLTIP = "This Duplicant is digging out a tile";
			}

			// Token: 0x02003550 RID: 13648
			public class FETCH
			{
				// Token: 0x0400DE30 RID: 56880
				public static LocString NAME = "Deliver";

				// Token: 0x0400DE31 RID: 56881
				public static LocString STATUS = "Delivering";

				// Token: 0x0400DE32 RID: 56882
				public static LocString TOOLTIP = "This Duplicant is delivering materials where they need to go";

				// Token: 0x0400DE33 RID: 56883
				public static LocString REPORT_NAME = "Deliver to {0}";
			}

			// Token: 0x02003551 RID: 13649
			public class JOYREACTION
			{
				// Token: 0x0400DE34 RID: 56884
				public static LocString NAME = "Joy Reaction";

				// Token: 0x0400DE35 RID: 56885
				public static LocString STATUS = "Overjoyed";

				// Token: 0x0400DE36 RID: 56886
				public static LocString TOOLTIP = "This Duplicant is taking a moment to relish in their own happiness";

				// Token: 0x0400DE37 RID: 56887
				public static LocString REPORT_NAME = "Overjoyed Reaction";
			}

			// Token: 0x02003552 RID: 13650
			public class ROCKETCONTROL
			{
				// Token: 0x0400DE38 RID: 56888
				public static LocString NAME = "Rocket Control";

				// Token: 0x0400DE39 RID: 56889
				public static LocString STATUS = "Controlling rocket";

				// Token: 0x0400DE3A RID: 56890
				public static LocString TOOLTIP = "This Duplicant is keeping their spacecraft on course";

				// Token: 0x0400DE3B RID: 56891
				public static LocString REPORT_NAME = "Rocket Control";
			}

			// Token: 0x02003553 RID: 13651
			public class STORAGEFETCH
			{
				// Token: 0x0400DE3C RID: 56892
				public static LocString NAME = "Store Materials";

				// Token: 0x0400DE3D RID: 56893
				public static LocString STATUS = "Storing materials";

				// Token: 0x0400DE3E RID: 56894
				public static LocString TOOLTIP = "This Duplicant is moving materials into storage for later use";

				// Token: 0x0400DE3F RID: 56895
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x02003554 RID: 13652
			public class EQUIPMENTFETCH
			{
				// Token: 0x0400DE40 RID: 56896
				public static LocString NAME = "Store Equipment";

				// Token: 0x0400DE41 RID: 56897
				public static LocString STATUS = "Storing equipment";

				// Token: 0x0400DE42 RID: 56898
				public static LocString TOOLTIP = "This Duplicant is transporting equipment for storage";

				// Token: 0x0400DE43 RID: 56899
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x02003555 RID: 13653
			public class REPAIRFETCH
			{
				// Token: 0x0400DE44 RID: 56900
				public static LocString NAME = "Repair Supply";

				// Token: 0x0400DE45 RID: 56901
				public static LocString STATUS = "Supplying repair materials";

				// Token: 0x0400DE46 RID: 56902
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed to repair buildings";
			}

			// Token: 0x02003556 RID: 13654
			public class RESEARCHFETCH
			{
				// Token: 0x0400DE47 RID: 56903
				public static LocString NAME = "Research Supply";

				// Token: 0x0400DE48 RID: 56904
				public static LocString STATUS = "Supplying research materials";

				// Token: 0x0400DE49 RID: 56905
				public static LocString TOOLTIP = "This Duplicant is delivering materials where they'll be needed to conduct " + UI.PRE_KEYWORD + "Research" + UI.PST_KEYWORD;
			}

			// Token: 0x02003557 RID: 13655
			public class EXCAVATEFOSSIL
			{
				// Token: 0x0400DE4A RID: 56906
				public static LocString NAME = "Excavate Fossil";

				// Token: 0x0400DE4B RID: 56907
				public static LocString STATUS = "Excavating a fossil";

				// Token: 0x0400DE4C RID: 56908
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is excavating a ",
					UI.PRE_KEYWORD,
					"Fossil",
					UI.PST_KEYWORD,
					" site"
				});
			}

			// Token: 0x02003558 RID: 13656
			public class ARMTRAP
			{
				// Token: 0x0400DE4D RID: 56909
				public static LocString NAME = "Arm Trap";

				// Token: 0x0400DE4E RID: 56910
				public static LocString STATUS = "Arming a trap";

				// Token: 0x0400DE4F RID: 56911
				public static LocString TOOLTIP = "This Duplicant is arming a trap";
			}

			// Token: 0x02003559 RID: 13657
			public class FARMFETCH
			{
				// Token: 0x0400DE50 RID: 56912
				public static LocString NAME = "Farming Supply";

				// Token: 0x0400DE51 RID: 56913
				public static LocString STATUS = "Supplying farming materials";

				// Token: 0x0400DE52 RID: 56914
				public static LocString TOOLTIP = "This Duplicant is delivering farming materials where they're needed to tend " + UI.PRE_KEYWORD + "Crops" + UI.PST_KEYWORD;
			}

			// Token: 0x0200355A RID: 13658
			public class FETCHCRITICAL
			{
				// Token: 0x0400DE53 RID: 56915
				public static LocString NAME = "Life Support Supply";

				// Token: 0x0400DE54 RID: 56916
				public static LocString STATUS = "Supplying critical materials";

				// Token: 0x0400DE55 RID: 56917
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is delivering materials required to perform ",
					UI.PRE_KEYWORD,
					"Life Support",
					UI.PST_KEYWORD,
					" Errands"
				});

				// Token: 0x0400DE56 RID: 56918
				public static LocString REPORT_NAME = "Life Support Supply to {0}";
			}

			// Token: 0x0200355B RID: 13659
			public class MACHINEFETCH
			{
				// Token: 0x0400DE57 RID: 56919
				public static LocString NAME = "Operational Supply";

				// Token: 0x0400DE58 RID: 56920
				public static LocString STATUS = "Supplying operational materials";

				// Token: 0x0400DE59 RID: 56921
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed for machine operation";

				// Token: 0x0400DE5A RID: 56922
				public static LocString REPORT_NAME = "Operational Supply to {0}";
			}

			// Token: 0x0200355C RID: 13660
			public class COOKFETCH
			{
				// Token: 0x0400DE5B RID: 56923
				public static LocString NAME = "Cook Supply";

				// Token: 0x0400DE5C RID: 56924
				public static LocString STATUS = "Supplying cook ingredients";

				// Token: 0x0400DE5D RID: 56925
				public static LocString TOOLTIP = "This Duplicant is delivering materials required to cook " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x0200355D RID: 13661
			public class DOCTORFETCH
			{
				// Token: 0x0400DE5E RID: 56926
				public static LocString NAME = "Medical Supply";

				// Token: 0x0400DE5F RID: 56927
				public static LocString STATUS = "Supplying medical resources";

				// Token: 0x0400DE60 RID: 56928
				public static LocString TOOLTIP = "This Duplicant is delivering the materials that will be needed to treat sick patients";

				// Token: 0x0400DE61 RID: 56929
				public static LocString REPORT_NAME = "Medical Supply to {0}";
			}

			// Token: 0x0200355E RID: 13662
			public class FOODFETCH
			{
				// Token: 0x0400DE62 RID: 56930
				public static LocString NAME = "Store Food";

				// Token: 0x0400DE63 RID: 56931
				public static LocString STATUS = "Storing food";

				// Token: 0x0400DE64 RID: 56932
				public static LocString TOOLTIP = "This Duplicant is moving edible resources into proper storage";

				// Token: 0x0400DE65 RID: 56933
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x0200355F RID: 13663
			public class POWERFETCH
			{
				// Token: 0x0400DE66 RID: 56934
				public static LocString NAME = "Power Supply";

				// Token: 0x0400DE67 RID: 56935
				public static LocString STATUS = "Supplying power materials";

				// Token: 0x0400DE68 RID: 56936
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed for " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;

				// Token: 0x0400DE69 RID: 56937
				public static LocString REPORT_NAME = "Power Supply to {0}";
			}

			// Token: 0x02003560 RID: 13664
			public class FABRICATEFETCH
			{
				// Token: 0x0400DE6A RID: 56938
				public static LocString NAME = "Fabrication Supply";

				// Token: 0x0400DE6B RID: 56939
				public static LocString STATUS = "Supplying fabrication materials";

				// Token: 0x0400DE6C RID: 56940
				public static LocString TOOLTIP = "This Duplicant is delivering materials required to fabricate new objects";

				// Token: 0x0400DE6D RID: 56941
				public static LocString REPORT_NAME = "Fabrication Supply to {0}";
			}

			// Token: 0x02003561 RID: 13665
			public class BUILDFETCH
			{
				// Token: 0x0400DE6E RID: 56942
				public static LocString NAME = "Construction Supply";

				// Token: 0x0400DE6F RID: 56943
				public static LocString STATUS = "Supplying construction materials";

				// Token: 0x0400DE70 RID: 56944
				public static LocString TOOLTIP = "This delivery will provide materials to a planned construction site";
			}

			// Token: 0x02003562 RID: 13666
			public class FETCHCREATURE
			{
				// Token: 0x0400DE71 RID: 56945
				public static LocString NAME = "Relocate Critter";

				// Token: 0x0400DE72 RID: 56946
				public static LocString STATUS = "Relocating critter";

				// Token: 0x0400DE73 RID: 56947
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is moving a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to a new location"
				});
			}

			// Token: 0x02003563 RID: 13667
			public class FETCHRANCHING
			{
				// Token: 0x0400DE74 RID: 56948
				public static LocString NAME = "Ranching Supply";

				// Token: 0x0400DE75 RID: 56949
				public static LocString STATUS = "Supplying ranching materials";

				// Token: 0x0400DE76 RID: 56950
				public static LocString TOOLTIP = "This Duplicant is delivering materials for ranching activities";
			}

			// Token: 0x02003564 RID: 13668
			public class TRANSPORT
			{
				// Token: 0x0400DE77 RID: 56951
				public static LocString NAME = "Sweep";

				// Token: 0x0400DE78 RID: 56952
				public static LocString STATUS = "Going to sweep";

				// Token: 0x0400DE79 RID: 56953
				public static LocString TOOLTIP = "Moving debris off the ground and into storage improves colony " + UI.PRE_KEYWORD + "Decor" + UI.PST_KEYWORD;
			}

			// Token: 0x02003565 RID: 13669
			public class MOVETOSAFETY
			{
				// Token: 0x0400DE7A RID: 56954
				public static LocString NAME = "Find Safe Area";

				// Token: 0x0400DE7B RID: 56955
				public static LocString STATUS = "Finding safer area";

				// Token: 0x0400DE7C RID: 56956
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is ",
					UI.PRE_KEYWORD,
					"Idle",
					UI.PST_KEYWORD,
					" and looking for somewhere safe and comfy to chill"
				});
			}

			// Token: 0x02003566 RID: 13670
			public class PARTY
			{
				// Token: 0x0400DE7D RID: 56957
				public static LocString NAME = "Party";

				// Token: 0x0400DE7E RID: 56958
				public static LocString STATUS = "Partying";

				// Token: 0x0400DE7F RID: 56959
				public static LocString TOOLTIP = "This Duplicant is partying hard";
			}

			// Token: 0x02003567 RID: 13671
			public class REMOTEWORK
			{
				// Token: 0x0400DE80 RID: 56960
				public static LocString NAME = "Remote Work";

				// Token: 0x0400DE81 RID: 56961
				public static LocString STATUS = "Working remotely";

				// Token: 0x0400DE82 RID: 56962
				public static LocString TOOLTIP = "This Duplicant's body is here, but their work is elsewhere";
			}

			// Token: 0x02003568 RID: 13672
			public class POWER_TINKER
			{
				// Token: 0x0400DE83 RID: 56963
				public static LocString NAME = "Tinker";

				// Token: 0x0400DE84 RID: 56964
				public static LocString STATUS = "Tinkering";

				// Token: 0x0400DE85 RID: 56965
				public static LocString TOOLTIP = "Tinkering with buildings improves their functionality";
			}

			// Token: 0x02003569 RID: 13673
			public class RANCH
			{
				// Token: 0x0400DE86 RID: 56966
				public static LocString NAME = "Ranch";

				// Token: 0x0400DE87 RID: 56967
				public static LocString STATUS = "Ranching";

				// Token: 0x0400DE88 RID: 56968
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is tending to a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"'s well-being"
				});

				// Token: 0x0400DE89 RID: 56969
				public static LocString REPORT_NAME = "Deliver to {0}";
			}

			// Token: 0x0200356A RID: 13674
			public class CROP_TEND
			{
				// Token: 0x0400DE8A RID: 56970
				public static LocString NAME = "Tend";

				// Token: 0x0400DE8B RID: 56971
				public static LocString STATUS = "Tending plant";

				// Token: 0x0400DE8C RID: 56972
				public static LocString TOOLTIP = "Tending to plants increases their " + UI.PRE_KEYWORD + "Growth Rate" + UI.PST_KEYWORD;
			}

			// Token: 0x0200356B RID: 13675
			public class DEMOLISH
			{
				// Token: 0x0400DE8D RID: 56973
				public static LocString NAME = "Demolish";

				// Token: 0x0400DE8E RID: 56974
				public static LocString STATUS = "Demolishing object";

				// Token: 0x0400DE8F RID: 56975
				public static LocString TOOLTIP = "Demolishing an object removes it permanently";
			}

			// Token: 0x0200356C RID: 13676
			public class IDLE
			{
				// Token: 0x0400DE90 RID: 56976
				public static LocString NAME = "Idle";

				// Token: 0x0400DE91 RID: 56977
				public static LocString STATUS = "Idle";

				// Token: 0x0400DE92 RID: 56978
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending " + UI.PRE_KEYWORD + "Errands" + UI.PST_KEYWORD;
			}

			// Token: 0x0200356D RID: 13677
			public class PRECONDITIONS
			{
				// Token: 0x0400DE93 RID: 56979
				public static LocString HEADER = "The selected {Selected} could:";

				// Token: 0x0400DE94 RID: 56980
				public static LocString SUCCESS_ROW = "{Duplicant} -- {Rank}";

				// Token: 0x0400DE95 RID: 56981
				public static LocString CURRENT_ERRAND = "Current Errand";

				// Token: 0x0400DE96 RID: 56982
				public static LocString RANK_FORMAT = "#{0}";

				// Token: 0x0400DE97 RID: 56983
				public static LocString FAILURE_ROW = "{Duplicant} -- {Reason}";

				// Token: 0x0400DE98 RID: 56984
				public static LocString CONTAINS_OXYGEN = "Not enough Oxygen";

				// Token: 0x0400DE99 RID: 56985
				public static LocString IS_PREEMPTABLE = "Already assigned to {Assignee}";

				// Token: 0x0400DE9A RID: 56986
				public static LocString HAS_URGE = "No current need";

				// Token: 0x0400DE9B RID: 56987
				public static LocString IS_VALID = "Invalid";

				// Token: 0x0400DE9C RID: 56988
				public static LocString IS_PERMITTED = "Not permitted";

				// Token: 0x0400DE9D RID: 56989
				public static LocString IS_ASSIGNED_TO_ME = "Not assigned to {Selected}";

				// Token: 0x0400DE9E RID: 56990
				public static LocString IS_IN_MY_WORLD = "Outside world";

				// Token: 0x0400DE9F RID: 56991
				public static LocString IS_CELL_NOT_IN_MY_WORLD = "Already there";

				// Token: 0x0400DEA0 RID: 56992
				public static LocString IS_IN_MY_ROOM = "Outside {Selected}'s room";

				// Token: 0x0400DEA1 RID: 56993
				public static LocString IS_PREFERRED_ASSIGNABLE = "Not preferred assignment";

				// Token: 0x0400DEA2 RID: 56994
				public static LocString IS_PREFERRED_ASSIGNABLE_OR_URGENT_BLADDER = "Not preferred assignment";

				// Token: 0x0400DEA3 RID: 56995
				public static LocString HAS_SKILL_PERK = "Requires learned skill";

				// Token: 0x0400DEA4 RID: 56996
				public static LocString IS_MORE_SATISFYING = "Low priority";

				// Token: 0x0400DEA5 RID: 56997
				public static LocString CAN_CHAT = "Unreachable";

				// Token: 0x0400DEA6 RID: 56998
				public static LocString IS_NOT_RED_ALERT = "Unavailable in Red Alert";

				// Token: 0x0400DEA7 RID: 56999
				public static LocString NO_DEAD_BODIES = "Unburied Duplicant";

				// Token: 0x0400DEA8 RID: 57000
				public static LocString NOT_A_ROBOT = "Unavailable to Robots";

				// Token: 0x0400DEA9 RID: 57001
				public static LocString IS_A_BIONIC = "Must be a Bionic Duplicant";

				// Token: 0x0400DEAA RID: 57002
				public static LocString NOT_A_BIONIC = "Unavailable to Bionic Duplicants";

				// Token: 0x0400DEAB RID: 57003
				public static LocString VALID_MOURNING_SITE = "Nowhere to mourn";

				// Token: 0x0400DEAC RID: 57004
				public static LocString HAS_PLACE_TO_STAND = "Nowhere to stand";

				// Token: 0x0400DEAD RID: 57005
				public static LocString IS_SCHEDULED_TIME = "Not allowed by schedule";

				// Token: 0x0400DEAE RID: 57006
				public static LocString CAN_MOVE_TO = "Unreachable";

				// Token: 0x0400DEAF RID: 57007
				public static LocString CAN_PICKUP = "Cannot pickup";

				// Token: 0x0400DEB0 RID: 57008
				public static LocString CANPICKUPANYASSIGNEDUPGRADE = "Cannot pick up any assigned boosters";

				// Token: 0x0400DEB1 RID: 57009
				public static LocString IS_AWAKE = "{Selected} is sleeping";

				// Token: 0x0400DEB2 RID: 57010
				public static LocString IS_STANDING = "{Selected} must stand";

				// Token: 0x0400DEB3 RID: 57011
				public static LocString IS_MOVING = "{Selected} is not moving";

				// Token: 0x0400DEB4 RID: 57012
				public static LocString IS_OFF_LADDER = "{Selected} is busy climbing";

				// Token: 0x0400DEB5 RID: 57013
				public static LocString NOT_IN_TUBE = "{Selected} is busy in transit";

				// Token: 0x0400DEB6 RID: 57014
				public static LocString HAS_TRAIT = "Missing required trait";

				// Token: 0x0400DEB7 RID: 57015
				public static LocString IS_OPERATIONAL = "Not operational";

				// Token: 0x0400DEB8 RID: 57016
				public static LocString IS_MARKED_FOR_DECONSTRUCTION = "Being deconstructed";

				// Token: 0x0400DEB9 RID: 57017
				public static LocString IS_NOT_BURROWED = "Is not burrowed";

				// Token: 0x0400DEBA RID: 57018
				public static LocString IS_CREATURE_AVAILABLE_FOR_RANCHING = "No Critters Available";

				// Token: 0x0400DEBB RID: 57019
				public static LocString IS_CREATURE_AVAILABLE_FOR_FIXED_CAPTURE = "Pen Status OK";

				// Token: 0x0400DEBC RID: 57020
				public static LocString IS_MARKED_FOR_DISABLE = "Building Disabled";

				// Token: 0x0400DEBD RID: 57021
				public static LocString IS_FUNCTIONAL = "Not functioning";

				// Token: 0x0400DEBE RID: 57022
				public static LocString IS_OVERRIDE_TARGET_NULL_OR_ME = "DebugIsOverrideTargetNullOrMe";

				// Token: 0x0400DEBF RID: 57023
				public static LocString NOT_CHORE_CREATOR = "DebugNotChoreCreator";

				// Token: 0x0400DEC0 RID: 57024
				public static LocString IS_GETTING_MORE_STRESSED = "{Selected}'s stress is decreasing";

				// Token: 0x0400DEC1 RID: 57025
				public static LocString IS_ALLOWED_BY_AUTOMATION = "Automated";

				// Token: 0x0400DEC2 RID: 57026
				public static LocString CAN_DO_RECREATION = "Not Interested";

				// Token: 0x0400DEC3 RID: 57027
				public static LocString DOES_SUIT_NEED_RECHARGING_IDLE = "Suit is currently charged";

				// Token: 0x0400DEC4 RID: 57028
				public static LocString DOES_SUIT_NEED_RECHARGING_URGENT = "Suit is currently charged";

				// Token: 0x0400DEC5 RID: 57029
				public static LocString HAS_SUIT_MARKER = "No Suit Checkpoint";

				// Token: 0x0400DEC6 RID: 57030
				public static LocString ALLOWED_TO_DEPRESSURIZE = "Not currently overpressure";

				// Token: 0x0400DEC7 RID: 57031
				public static LocString IS_STRESS_ABOVE_ACTIVATION_RANGE = "{Selected} is not stressed right now";

				// Token: 0x0400DEC8 RID: 57032
				public static LocString IS_NOT_ANGRY = "{Selected} is too angry";

				// Token: 0x0400DEC9 RID: 57033
				public static LocString IS_NOT_BEING_ATTACKED = "{Selected} is in combat";

				// Token: 0x0400DECA RID: 57034
				public static LocString IS_CONSUMPTION_PERMITTED = "Disallowed by consumable permissions";

				// Token: 0x0400DECB RID: 57035
				public static LocString CAN_CURE = "No applicable illness";

				// Token: 0x0400DECC RID: 57036
				public static LocString TREATMENT_AVAILABLE = "No treatable illness";

				// Token: 0x0400DECD RID: 57037
				public static LocString DOCTOR_AVAILABLE = "No doctors available\n(Duplicants cannot treat themselves)";

				// Token: 0x0400DECE RID: 57038
				public static LocString IS_OKAY_TIME_TO_SLEEP = "No current need";

				// Token: 0x0400DECF RID: 57039
				public static LocString IS_NARCOLEPSING = "{Selected} is currently napping";

				// Token: 0x0400DED0 RID: 57040
				public static LocString IS_FETCH_TARGET_AVAILABLE = "No pending deliveries";

				// Token: 0x0400DED1 RID: 57041
				public static LocString EDIBLE_IS_NOT_NULL = "Consumable Permission not allowed";

				// Token: 0x0400DED2 RID: 57042
				public static LocString HAS_MINGLE_CELL = "Nowhere to Mingle";

				// Token: 0x0400DED3 RID: 57043
				public static LocString EXCLUSIVELY_AVAILABLE = "Building Already Busy";

				// Token: 0x0400DED4 RID: 57044
				public static LocString BLADDER_FULL = "Bladder isn't full";

				// Token: 0x0400DED5 RID: 57045
				public static LocString BLADDER_NOT_FULL = "Bladder too full";

				// Token: 0x0400DED6 RID: 57046
				public static LocString CURRENTLY_PEEING = "Currently Peeing";

				// Token: 0x0400DED7 RID: 57047
				public static LocString HAS_BALLOON_STALL_CELL = "Has a location for a Balloon Stall";

				// Token: 0x0400DED8 RID: 57048
				public static LocString IS_MINION = "Must be a Duplicant";

				// Token: 0x0400DED9 RID: 57049
				public static LocString IS_ROCKET_TRAVELLING = "Rocket must be travelling";

				// Token: 0x0400DEDA RID: 57050
				public static LocString REMOTE_CHORE_SUBCHORE_PRECONDITIONS = "No Eligible Remote Chores";

				// Token: 0x0400DEDB RID: 57051
				public static LocString REMOTE_CHORE_NO_REMOTE_DOCK = "No Dock Assigned";

				// Token: 0x0400DEDC RID: 57052
				public static LocString REMOTE_CHORE_DOCK_INOPERABLE = "Remote Worker Dock Unusable";

				// Token: 0x0400DEDD RID: 57053
				public static LocString REMOTE_CHORE_NO_REMOTE_WORKER = "No Remote Worker at Dock";

				// Token: 0x0400DEDE RID: 57054
				public static LocString REMOTE_CHORE_DOCK_UNAVAILABLE = "Remote Worker Already Busy";

				// Token: 0x0400DEDF RID: 57055
				public static LocString CAN_FETCH_DRONE_COMPLETE_FETCH = "Flydo cannot complete chore";
			}
		}

		// Token: 0x02002581 RID: 9601
		public class SKILLGROUPS
		{
			// Token: 0x0200356E RID: 13678
			public class MINING
			{
				// Token: 0x0400DEE0 RID: 57056
				public static LocString NAME = "Digger";
			}

			// Token: 0x0200356F RID: 13679
			public class BUILDING
			{
				// Token: 0x0400DEE1 RID: 57057
				public static LocString NAME = "Builder";
			}

			// Token: 0x02003570 RID: 13680
			public class FARMING
			{
				// Token: 0x0400DEE2 RID: 57058
				public static LocString NAME = "Farmer";
			}

			// Token: 0x02003571 RID: 13681
			public class RANCHING
			{
				// Token: 0x0400DEE3 RID: 57059
				public static LocString NAME = "Rancher";
			}

			// Token: 0x02003572 RID: 13682
			public class COOKING
			{
				// Token: 0x0400DEE4 RID: 57060
				public static LocString NAME = "Cooker";
			}

			// Token: 0x02003573 RID: 13683
			public class ART
			{
				// Token: 0x0400DEE5 RID: 57061
				public static LocString NAME = "Decorator";
			}

			// Token: 0x02003574 RID: 13684
			public class RESEARCH
			{
				// Token: 0x0400DEE6 RID: 57062
				public static LocString NAME = "Researcher";
			}

			// Token: 0x02003575 RID: 13685
			public class SUITS
			{
				// Token: 0x0400DEE7 RID: 57063
				public static LocString NAME = "Suit Wearer";
			}

			// Token: 0x02003576 RID: 13686
			public class HAULING
			{
				// Token: 0x0400DEE8 RID: 57064
				public static LocString NAME = "Supplier";
			}

			// Token: 0x02003577 RID: 13687
			public class TECHNICALS
			{
				// Token: 0x0400DEE9 RID: 57065
				public static LocString NAME = "Operator";
			}

			// Token: 0x02003578 RID: 13688
			public class MEDICALAID
			{
				// Token: 0x0400DEEA RID: 57066
				public static LocString NAME = "Doctor";
			}

			// Token: 0x02003579 RID: 13689
			public class BASEKEEPING
			{
				// Token: 0x0400DEEB RID: 57067
				public static LocString NAME = "Tidier";
			}

			// Token: 0x0200357A RID: 13690
			public class ROCKETRY
			{
				// Token: 0x0400DEEC RID: 57068
				public static LocString NAME = "Pilot";
			}
		}

		// Token: 0x02002582 RID: 9602
		public class CHOREGROUPS
		{
			// Token: 0x0200357B RID: 13691
			public class ART
			{
				// Token: 0x0400DEED RID: 57069
				public static LocString NAME = "Decorating";

				// Token: 0x0400DEEE RID: 57070
				public static LocString DESC = string.Concat(new string[]
				{
					"Sculpt or paint to improve colony ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400DEEF RID: 57071
				public static LocString ARCHETYPE_NAME = "Decorator";
			}

			// Token: 0x0200357C RID: 13692
			public class COMBAT
			{
				// Token: 0x0400DEF0 RID: 57072
				public static LocString NAME = "Attacking";

				// Token: 0x0400DEF1 RID: 57073
				public static LocString DESC = "Fight wild " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400DEF2 RID: 57074
				public static LocString ARCHETYPE_NAME = "Attacker";
			}

			// Token: 0x0200357D RID: 13693
			public class LIFESUPPORT
			{
				// Token: 0x0400DEF3 RID: 57075
				public static LocString NAME = "Life Support";

				// Token: 0x0400DEF4 RID: 57076
				public static LocString DESC = string.Concat(new string[]
				{
					"Maintain ",
					BUILDINGS.PREFABS.ALGAEHABITAT.NAME,
					"s, ",
					BUILDINGS.PREFABS.AIRFILTER.NAME,
					"s, and ",
					BUILDINGS.PREFABS.WATERPURIFIER.NAME,
					"s to support colony life."
				});

				// Token: 0x0400DEF5 RID: 57077
				public static LocString ARCHETYPE_NAME = "Life Supporter";
			}

			// Token: 0x0200357E RID: 13694
			public class TOGGLE
			{
				// Token: 0x0400DEF6 RID: 57078
				public static LocString NAME = "Toggling";

				// Token: 0x0400DEF7 RID: 57079
				public static LocString DESC = "Enable or disable buildings, adjust building settings, and set or flip switches and sensors.";

				// Token: 0x0400DEF8 RID: 57080
				public static LocString ARCHETYPE_NAME = "Toggler";
			}

			// Token: 0x0200357F RID: 13695
			public class COOK
			{
				// Token: 0x0400DEF9 RID: 57081
				public static LocString NAME = "Cooking";

				// Token: 0x0400DEFA RID: 57082
				public static LocString DESC = string.Concat(new string[]
				{
					"Operate ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" preparation buildings."
				});

				// Token: 0x0400DEFB RID: 57083
				public static LocString ARCHETYPE_NAME = "Cooker";
			}

			// Token: 0x02003580 RID: 13696
			public class RESEARCH
			{
				// Token: 0x0400DEFC RID: 57084
				public static LocString NAME = "Researching";

				// Token: 0x0400DEFD RID: 57085
				public static LocString DESC = string.Concat(new string[]
				{
					"Use ",
					UI.PRE_KEYWORD,
					"Research Stations",
					UI.PST_KEYWORD,
					" to unlock new technologies."
				});

				// Token: 0x0400DEFE RID: 57086
				public static LocString ARCHETYPE_NAME = "Researcher";
			}

			// Token: 0x02003581 RID: 13697
			public class REPAIR
			{
				// Token: 0x0400DEFF RID: 57087
				public static LocString NAME = "Repairing";

				// Token: 0x0400DF00 RID: 57088
				public static LocString DESC = "Repair damaged buildings.";

				// Token: 0x0400DF01 RID: 57089
				public static LocString ARCHETYPE_NAME = "Repairer";
			}

			// Token: 0x02003582 RID: 13698
			public class FARMING
			{
				// Token: 0x0400DF02 RID: 57090
				public static LocString NAME = "Farming";

				// Token: 0x0400DF03 RID: 57091
				public static LocString DESC = string.Concat(new string[]
				{
					"Gather crops from mature ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400DF04 RID: 57092
				public static LocString ARCHETYPE_NAME = "Farmer";
			}

			// Token: 0x02003583 RID: 13699
			public class RANCHING
			{
				// Token: 0x0400DF05 RID: 57093
				public static LocString NAME = "Ranching";

				// Token: 0x0400DF06 RID: 57094
				public static LocString DESC = "Tend to domesticated " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400DF07 RID: 57095
				public static LocString ARCHETYPE_NAME = "Rancher";
			}

			// Token: 0x02003584 RID: 13700
			public class BUILD
			{
				// Token: 0x0400DF08 RID: 57096
				public static LocString NAME = "Building";

				// Token: 0x0400DF09 RID: 57097
				public static LocString DESC = "Construct new buildings.";

				// Token: 0x0400DF0A RID: 57098
				public static LocString ARCHETYPE_NAME = "Builder";
			}

			// Token: 0x02003585 RID: 13701
			public class HAULING
			{
				// Token: 0x0400DF0B RID: 57099
				public static LocString NAME = "Supplying";

				// Token: 0x0400DF0C RID: 57100
				public static LocString DESC = "Run resources to critical buildings and urgent storage.";

				// Token: 0x0400DF0D RID: 57101
				public static LocString ARCHETYPE_NAME = "Supplier";
			}

			// Token: 0x02003586 RID: 13702
			public class STORAGE
			{
				// Token: 0x0400DF0E RID: 57102
				public static LocString NAME = "Storing";

				// Token: 0x0400DF0F RID: 57103
				public static LocString DESC = "Fill storage buildings with resources when no other errands are available.";

				// Token: 0x0400DF10 RID: 57104
				public static LocString ARCHETYPE_NAME = "Storer";
			}

			// Token: 0x02003587 RID: 13703
			public class RECREATION
			{
				// Token: 0x0400DF11 RID: 57105
				public static LocString NAME = "Relaxing";

				// Token: 0x0400DF12 RID: 57106
				public static LocString DESC = "Use leisure facilities, chat with other Duplicants, and relieve Stress.";

				// Token: 0x0400DF13 RID: 57107
				public static LocString ARCHETYPE_NAME = "Relaxer";
			}

			// Token: 0x02003588 RID: 13704
			public class BASEKEEPING
			{
				// Token: 0x0400DF14 RID: 57108
				public static LocString NAME = "Tidying";

				// Token: 0x0400DF15 RID: 57109
				public static LocString DESC = "Sweep, mop, and disinfect objects within the colony.";

				// Token: 0x0400DF16 RID: 57110
				public static LocString ARCHETYPE_NAME = "Tidier";
			}

			// Token: 0x02003589 RID: 13705
			public class DIG
			{
				// Token: 0x0400DF17 RID: 57111
				public static LocString NAME = "Digging";

				// Token: 0x0400DF18 RID: 57112
				public static LocString DESC = "Mine raw resources.";

				// Token: 0x0400DF19 RID: 57113
				public static LocString ARCHETYPE_NAME = "Digger";
			}

			// Token: 0x0200358A RID: 13706
			public class MEDICALAID
			{
				// Token: 0x0400DF1A RID: 57114
				public static LocString NAME = "Doctoring";

				// Token: 0x0400DF1B RID: 57115
				public static LocString DESC = "Treat sick and injured Duplicants.";

				// Token: 0x0400DF1C RID: 57116
				public static LocString ARCHETYPE_NAME = "Doctor";
			}

			// Token: 0x0200358B RID: 13707
			public class MASSAGE
			{
				// Token: 0x0400DF1D RID: 57117
				public static LocString NAME = "Relaxing";

				// Token: 0x0400DF1E RID: 57118
				public static LocString DESC = "Take breaks for massages.";

				// Token: 0x0400DF1F RID: 57119
				public static LocString ARCHETYPE_NAME = "Relaxer";
			}

			// Token: 0x0200358C RID: 13708
			public class MACHINEOPERATING
			{
				// Token: 0x0400DF20 RID: 57120
				public static LocString NAME = "Operating";

				// Token: 0x0400DF21 RID: 57121
				public static LocString DESC = "Operating machinery for production, fabrication, and utility purposes.";

				// Token: 0x0400DF22 RID: 57122
				public static LocString ARCHETYPE_NAME = "Operator";
			}

			// Token: 0x0200358D RID: 13709
			public class SUITS
			{
				// Token: 0x0400DF23 RID: 57123
				public static LocString ARCHETYPE_NAME = "Suit Wearer";
			}

			// Token: 0x0200358E RID: 13710
			public class ROCKETRY
			{
				// Token: 0x0400DF24 RID: 57124
				public static LocString NAME = "Rocketry";

				// Token: 0x0400DF25 RID: 57125
				public static LocString DESC = "Pilot rockets";

				// Token: 0x0400DF26 RID: 57126
				public static LocString ARCHETYPE_NAME = "Pilot";
			}
		}

		// Token: 0x02002583 RID: 9603
		public class STATUSITEMS
		{
			// Token: 0x0200358F RID: 13711
			public class SLIPPERING
			{
				// Token: 0x0400DF27 RID: 57127
				public static LocString NAME = "Slipping";

				// Token: 0x0400DF28 RID: 57128
				public static LocString TOOLTIP = "This Duplicant is losing their balance on a slippery surface\n\nIt's not fun";
			}

			// Token: 0x02003590 RID: 13712
			public class WAXEDFORTRANSITTUBE
			{
				// Token: 0x0400DF29 RID: 57129
				public static LocString NAME = "Smooth Rider";

				// Token: 0x0400DF2A RID: 57130
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slapped on some ",
					ELEMENTS.MILKFAT.NAME,
					" before starting their commute\n\nThis boosts their ",
					BUILDINGS.PREFABS.TRAVELTUBE.NAME,
					" travel speed by {0}"
				});
			}

			// Token: 0x02003591 RID: 13713
			public class ARMINGTRAP
			{
				// Token: 0x0400DF2B RID: 57131
				public static LocString NAME = "Arming trap";

				// Token: 0x0400DF2C RID: 57132
				public static LocString TOOLTIP = "This Duplicant is arming a trap";
			}

			// Token: 0x02003592 RID: 13714
			public class GENERIC_DELIVER
			{
				// Token: 0x0400DF2D RID: 57133
				public static LocString NAME = "Delivering resources to {Target}";

				// Token: 0x0400DF2E RID: 57134
				public static LocString TOOLTIP = "This Duplicant is transporting materials to <b>{Target}</b>";
			}

			// Token: 0x02003593 RID: 13715
			public class COUGHING
			{
				// Token: 0x0400DF2F RID: 57135
				public static LocString NAME = "Yucky Lungs Coughing";

				// Token: 0x0400DF30 RID: 57136
				public static LocString TOOLTIP = "Hey! Do that into your elbow\n• Coughing fit was caused by " + DUPLICANTS.MODIFIERS.CONTAMINATEDLUNGS.NAME;
			}

			// Token: 0x02003594 RID: 13716
			public class WEARING_PAJAMAS
			{
				// Token: 0x0400DF31 RID: 57137
				public static LocString NAME = "Wearing " + UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");

				// Token: 0x0400DF32 RID: 57138
				public static LocString TOOLTIP = "This Duplicant can now produce " + UI.FormatAsLink("Dream Journals", "DREAMJOURNAL") + " when sleeping";
			}

			// Token: 0x02003595 RID: 13717
			public class DREAMING
			{
				// Token: 0x0400DF33 RID: 57139
				public static LocString NAME = "Dreaming";

				// Token: 0x0400DF34 RID: 57140
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is adventuring through their own subconscious\n\nDreams are caused by wearing ",
					UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS"),
					"\n\n",
					UI.FormatAsLink("Dream Journal", "DREAMJOURNAL"),
					" will be ready in {time}"
				});
			}

			// Token: 0x02003596 RID: 13718
			public class FOSSILHUNT
			{
				// Token: 0x02003D7E RID: 15742
				public class WORKEREXCAVATING
				{
					// Token: 0x0400F2B8 RID: 62136
					public static LocString NAME = "Excavating Fossil";

					// Token: 0x0400F2B9 RID: 62137
					public static LocString TOOLTIP = "This Duplicant is carefully uncovering a " + UI.FormatAsLink("Fossil", "FOSSIL");
				}
			}

			// Token: 0x02003597 RID: 13719
			public class SLEEPING
			{
				// Token: 0x0400DF35 RID: 57141
				public static LocString NAME = "Sleeping";

				// Token: 0x0400DF36 RID: 57142
				public static LocString TOOLTIP = "This Duplicant is recovering stamina";

				// Token: 0x0400DF37 RID: 57143
				public static LocString TOOLTIP_DISTURBER = "\n\nThey were sleeping peacefully until they were disturbed by <b>{Disturber}</b>";
			}

			// Token: 0x02003598 RID: 13720
			public class SLEEPINGEXHAUSTED
			{
				// Token: 0x0400DF38 RID: 57144
				public static LocString NAME = "Unscheduled Nap";

				// Token: 0x0400DF39 RID: 57145
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Cold ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" or lack of rest depleted this Duplicant's ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					"\n\nThey didn't have enough energy to make it to bedtime"
				});
			}

			// Token: 0x02003599 RID: 13721
			public class SLEEPINGPEACEFULLY
			{
				// Token: 0x0400DF3A RID: 57146
				public static LocString NAME = "Sleeping peacefully";

				// Token: 0x0400DF3B RID: 57147
				public static LocString TOOLTIP = "This Duplicant is getting well-deserved, quality sleep\n\nAt this rate they're sure to feel " + UI.FormatAsLink("Well Rested", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x0200359A RID: 13722
			public class SLEEPINGBADLY
			{
				// Token: 0x0400DF3C RID: 57148
				public static LocString NAME = "Sleeping badly";

				// Token: 0x0400DF3D RID: 57149
				public static LocString TOOLTIP = "This Duplicant's having trouble falling asleep due to noise from <b>{Disturber}</b>\n\nThey're going to feel a bit " + UI.FormatAsLink("Unrested", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x0200359B RID: 13723
			public class SLEEPINGTERRIBLY
			{
				// Token: 0x0400DF3E RID: 57150
				public static LocString NAME = "Can't sleep";

				// Token: 0x0400DF3F RID: 57151
				public static LocString TOOLTIP = "This Duplicant was woken up by noise from <b>{Disturber}</b> and can't get back to sleep\n\nThey're going to feel " + UI.FormatAsLink("Dead Tired", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x0200359C RID: 13724
			public class SLEEPINGINTERRUPTEDBYLIGHT
			{
				// Token: 0x0400DF40 RID: 57152
				public static LocString NAME = "Interrupted Sleep: Bright Light";

				// Token: 0x0400DF41 RID: 57153
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant can't sleep because the ",
					UI.PRE_KEYWORD,
					"Lights",
					UI.PST_KEYWORD,
					" are still on"
				});
			}

			// Token: 0x0200359D RID: 13725
			public class SLEEPINGINTERRUPTEDBYNOISE
			{
				// Token: 0x0400DF42 RID: 57154
				public static LocString NAME = "Interrupted Sleep: Snoring Friend";

				// Token: 0x0400DF43 RID: 57155
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping thanks to a certain noisy someone";
			}

			// Token: 0x0200359E RID: 13726
			public class SLEEPINGINTERRUPTEDBYFEAROFDARK
			{
				// Token: 0x0400DF44 RID: 57156
				public static LocString NAME = "Interrupted Sleep: Afraid of Dark";

				// Token: 0x0400DF45 RID: 57157
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping because of their fear of the dark";
			}

			// Token: 0x0200359F RID: 13727
			public class SLEEPINGINTERRUPTEDBYMOVEMENT
			{
				// Token: 0x0400DF46 RID: 57158
				public static LocString NAME = "Interrupted Sleep: Bed Jostling";

				// Token: 0x0400DF47 RID: 57159
				public static LocString TOOLTIP = "This Duplicant was woken up because their bed was moved";
			}

			// Token: 0x020035A0 RID: 13728
			public class SLEEPINGINTERRUPTEDBYCOLD
			{
				// Token: 0x0400DF48 RID: 57160
				public static LocString NAME = "Interrupted Sleep: Cold Room";

				// Token: 0x0400DF49 RID: 57161
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping because this room is too cold";
			}

			// Token: 0x020035A1 RID: 13729
			public class REDALERT
			{
				// Token: 0x0400DF4A RID: 57162
				public static LocString NAME = "Red Alert!";

				// Token: 0x0400DF4B RID: 57163
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony is in a state of ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					". Duplicants will not eat, sleep, use the bathroom, or engage in leisure activities while the ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					" is active"
				});
			}

			// Token: 0x020035A2 RID: 13730
			public class ROLE
			{
				// Token: 0x0400DF4C RID: 57164
				public static LocString NAME = "{Role}: {Progress} Mastery";

				// Token: 0x0400DF4D RID: 57165
				public static LocString TOOLTIP = "This Duplicant is working as a <b>{Role}</b>\n\nThey have <b>{Progress}</b> mastery of this job";
			}

			// Token: 0x020035A3 RID: 13731
			public class LOWOXYGEN
			{
				// Token: 0x0400DF4E RID: 57166
				public static LocString NAME = "Oxygen low";

				// Token: 0x0400DF4F RID: 57167
				public static LocString TOOLTIP = "This Duplicant is working in a low breathability area";

				// Token: 0x0400DF50 RID: 57168
				public static LocString NOTIFICATION_NAME = "Low " + ELEMENTS.OXYGEN.NAME + " area entered";

				// Token: 0x0400DF51 RID: 57169
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are working in areas with low " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x020035A4 RID: 13732
			public class SEVEREWOUNDS
			{
				// Token: 0x0400DF52 RID: 57170
				public static LocString NAME = "Severely injured";

				// Token: 0x0400DF53 RID: 57171
				public static LocString TOOLTIP = "This Duplicant is badly hurt";

				// Token: 0x0400DF54 RID: 57172
				public static LocString NOTIFICATION_NAME = "Severely injured";

				// Token: 0x0400DF55 RID: 57173
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are badly hurt and require medical attention";
			}

			// Token: 0x020035A5 RID: 13733
			public class INCAPACITATED
			{
				// Token: 0x0400DF56 RID: 57174
				public static LocString NAME = "Incapacitated: {CauseOfIncapacitation}\nTime until death: {TimeUntilDeath}\n";

				// Token: 0x0400DF57 RID: 57175
				public static LocString TOOLTIP = "This Duplicant is near death!\n\nAssign them to a Triage Cot for rescue";

				// Token: 0x0400DF58 RID: 57176
				public static LocString NOTIFICATION_NAME = "Incapacitated";

				// Token: 0x0400DF59 RID: 57177
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are near death.\nA " + BUILDINGS.PREFABS.MEDICALCOT.NAME + " is required for rescue:";
			}

			// Token: 0x020035A6 RID: 13734
			public class BIONICOFFLINEINCAPACITATED
			{
				// Token: 0x0400DF5A RID: 57178
				public static LocString NAME = "Incapacitated: Powerless";

				// Token: 0x0400DF5B RID: 57179
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is non-functional!\n\nDeliver a charged ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" and reboot their systems to revive them"
				});

				// Token: 0x0400DF5C RID: 57180
				public static LocString NOTIFICATION_NAME = "Bionic Duplicant Incapacitated";

				// Token: 0x0400DF5D RID: 57181
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Bionic Duplicants are non-functional.\n\nA charged ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" and full reboot by a skilled Duplicant are required for rescue:"
				});
			}

			// Token: 0x020035A7 RID: 13735
			public class BIONICMICROCHIPGENERATION
			{
				// Token: 0x0400DF5E RID: 57182
				public static LocString NAME = "Programming Microchip: {0}";

				// Token: 0x0400DF5F RID: 57183
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is programming a microchip for use in ",
					UI.PRE_KEYWORD,
					"Booster",
					UI.PST_KEYWORD,
					" production\n\nBionic Duplicants will program microchips while defragmenting\n\nThey will produce 1 microchip every {0}"
				});
			}

			// Token: 0x020035A8 RID: 13736
			public class BIONICWANTSOILCHANGE
			{
				// Token: 0x0400DF60 RID: 57184
				public static LocString NAME = "Low Gear Oil";

				// Token: 0x0400DF61 RID: 57185
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is almost out of ",
					UI.PRE_KEYWORD,
					"Gear Oil",
					UI.PST_KEYWORD,
					"\n\nThey need to find ",
					UI.PRE_KEYWORD,
					"Gear Balm",
					UI.PST_KEYWORD,
					" or visit a ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035A9 RID: 13737
			public class BIONICWAITINGFORREBOOT
			{
				// Token: 0x0400DF62 RID: 57186
				public static LocString NAME = "Awaiting Reboot";

				// Token: 0x0400DF63 RID: 57187
				public static LocString TOOLTIP = "This Duplicant needs someone to reboot their bionic systems so they can get back to work";
			}

			// Token: 0x020035AA RID: 13738
			public class BIONICBEINGREBOOTED
			{
				// Token: 0x0400DF64 RID: 57188
				public static LocString NAME = "Reboot in progress";

				// Token: 0x0400DF65 RID: 57189
				public static LocString TOOLTIP = "This Duplicant's bionic systems are being rebooted";
			}

			// Token: 0x020035AB RID: 13739
			public class BIONICREQUIRESSKILLPERK
			{
				// Token: 0x0400DF66 RID: 57190
				public static LocString NAME = "Skill-Required Operation";

				// Token: 0x0400DF67 RID: 57191
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can reboot this Duplicant's bionic systems:\n\n{Skills}"
				});
			}

			// Token: 0x020035AC RID: 13740
			public class CLOGGINGTOILET
			{
				// Token: 0x0400DF68 RID: 57192
				public static LocString NAME = "Clogging a toilet";

				// Token: 0x0400DF69 RID: 57193
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is clogging a toilet with ",
					UI.PRE_KEYWORD,
					"Gunk",
					UI.PST_KEYWORD,
					"\n\nThey couldn't get to a ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" in time"
				});
			}

			// Token: 0x020035AD RID: 13741
			public class BEDUNREACHABLE
			{
				// Token: 0x0400DF6A RID: 57194
				public static LocString NAME = "Cannot reach bed";

				// Token: 0x0400DF6B RID: 57195
				public static LocString TOOLTIP = "This Duplicant cannot reach their bed";

				// Token: 0x0400DF6C RID: 57196
				public static LocString NOTIFICATION_NAME = "Unreachable bed";

				// Token: 0x0400DF6D RID: 57197
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot sleep because their ",
					UI.PRE_KEYWORD,
					"Beds",
					UI.PST_KEYWORD,
					" are beyond their reach:"
				});
			}

			// Token: 0x020035AE RID: 13742
			public class COLD
			{
				// Token: 0x0400DF6E RID: 57198
				public static LocString NAME = "Chilly surroundings";

				// Token: 0x0400DF6F RID: 57199
				public static LocString TOOLTIP = "This Duplicant cannot retain enough heat to stay warm and may be under-insulated for this area\n\nThey will begin to recover shortly after they leave this area\n\nStress: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>\n\nCurrent Environmental Exchange: <b>{currentTransferWattage}</b>\n\nInsulation Thickness: {conductivityBarrier}";
			}

			// Token: 0x020035AF RID: 13743
			public class EXITINGCOLD
			{
				// Token: 0x0400DF70 RID: 57200
				public static LocString NAME = "Shivering";

				// Token: 0x0400DF71 RID: 57201
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was recently exposed to cold ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" and wants to warm up\n\nWithout a warming station, it will take {0} for them to recover\n\nStress: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>"
				});
			}

			// Token: 0x020035B0 RID: 13744
			public class DAILYRATIONLIMITREACHED
			{
				// Token: 0x0400DF72 RID: 57202
				public static LocString NAME = "Daily calorie limit reached";

				// Token: 0x0400DF73 RID: 57203
				public static LocString TOOLTIP = "This Duplicant has consumed their allotted " + UI.FormatAsLink("Rations", "FOOD") + " for the day";

				// Token: 0x0400DF74 RID: 57204
				public static LocString NOTIFICATION_NAME = "Daily calorie limit reached";

				// Token: 0x0400DF75 RID: 57205
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants have consumed their allotted " + UI.FormatAsLink("Rations", "FOOD") + " for the day:";
			}

			// Token: 0x020035B1 RID: 13745
			public class DOCTOR
			{
				// Token: 0x0400DF76 RID: 57206
				public static LocString NAME = "Treating Patient";

				// Token: 0x0400DF77 RID: 57207
				public static LocString STATUS = "This Duplicant is going to administer medical care to an ailing friend";
			}

			// Token: 0x020035B2 RID: 13746
			public class HOLDINGBREATH
			{
				// Token: 0x0400DF78 RID: 57208
				public static LocString NAME = "Holding breath";

				// Token: 0x0400DF79 RID: 57209
				public static LocString TOOLTIP = "This Duplicant cannot breathe in their current location";
			}

			// Token: 0x020035B3 RID: 13747
			public class RECOVERINGBREATH
			{
				// Token: 0x0400DF7A RID: 57210
				public static LocString NAME = "Recovering breath";

				// Token: 0x0400DF7B RID: 57211
				public static LocString TOOLTIP = "This Duplicant held their breath too long and needs a moment";
			}

			// Token: 0x020035B4 RID: 13748
			public class HOT
			{
				// Token: 0x0400DF7C RID: 57212
				public static LocString NAME = "Toasty surroundings";

				// Token: 0x0400DF7D RID: 57213
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant cannot let off enough ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" to stay cool and may be over-insulated for this area\n\nThey will begin to recover shortly after they leave this area\n\nStress Modification: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>\n\nCurrent Environmental Exchange: <b>{currentTransferWattage}</b>\n\nInsulation Thickness: {conductivityBarrier}"
				});
			}

			// Token: 0x020035B5 RID: 13749
			public class EXITINGHOT
			{
				// Token: 0x0400DF7E RID: 57214
				public static LocString NAME = "Sweaty";

				// Token: 0x0400DF7F RID: 57215
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was recently exposed to hot ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" and wants to cool down\n\nWithout a cooling station, it will take {0} for them to recover\n\nStress: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>"
				});
			}

			// Token: 0x020035B6 RID: 13750
			public class HUNGRY
			{
				// Token: 0x0400DF80 RID: 57216
				public static LocString NAME = "Hungry";

				// Token: 0x0400DF81 RID: 57217
				public static LocString TOOLTIP = "This Duplicant would really like something to eat";
			}

			// Token: 0x020035B7 RID: 13751
			public class POORDECOR
			{
				// Token: 0x0400DF82 RID: 57218
				public static LocString NAME = "Drab decor";

				// Token: 0x0400DF83 RID: 57219
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is depressed by the lack of ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" in this area"
				});
			}

			// Token: 0x020035B8 RID: 13752
			public class POORQUALITYOFLIFE
			{
				// Token: 0x0400DF84 RID: 57220
				public static LocString NAME = "Low Morale";

				// Token: 0x0400DF85 RID: 57221
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The bad in this Duplicant's life is starting to outweigh the good\n\nImproved amenities and additional ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" would help improve their ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035B9 RID: 13753
			public class POOR_FOOD_QUALITY
			{
				// Token: 0x0400DF86 RID: 57222
				public static LocString NAME = "Lousy Meal";

				// Token: 0x0400DF87 RID: 57223
				public static LocString TOOLTIP = "The last meal this Duplicant ate didn't quite meet their expectations";
			}

			// Token: 0x020035BA RID: 13754
			public class GOOD_FOOD_QUALITY
			{
				// Token: 0x0400DF88 RID: 57224
				public static LocString NAME = "Decadent Meal";

				// Token: 0x0400DF89 RID: 57225
				public static LocString TOOLTIP = "The last meal this Duplicant ate exceeded their expectations!";
			}

			// Token: 0x020035BB RID: 13755
			public class NERVOUSBREAKDOWN
			{
				// Token: 0x0400DF8A RID: 57226
				public static LocString NAME = "Nervous breakdown";

				// Token: 0x0400DF8B RID: 57227
				public static LocString TOOLTIP = UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD + " has completely eroded this Duplicant's ability to function";

				// Token: 0x0400DF8C RID: 57228
				public static LocString NOTIFICATION_NAME = "Nervous breakdown";

				// Token: 0x0400DF8D RID: 57229
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have cracked under the ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" and need assistance:"
				});
			}

			// Token: 0x020035BC RID: 13756
			public class STRESSED
			{
				// Token: 0x0400DF8E RID: 57230
				public static LocString NAME = "Stressed";

				// Token: 0x0400DF8F RID: 57231
				public static LocString TOOLTIP = "This Duplicant is feeling the pressure";

				// Token: 0x0400DF90 RID: 57232
				public static LocString NOTIFICATION_NAME = "High stress";

				// Token: 0x0400DF91 RID: 57233
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants are ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" and need to unwind:"
				});
			}

			// Token: 0x020035BD RID: 13757
			public class NORATIONSAVAILABLE
			{
				// Token: 0x0400DF92 RID: 57234
				public static LocString NAME = "No food available";

				// Token: 0x0400DF93 RID: 57235
				public static LocString TOOLTIP = "There's nothing in the colony for this Duplicant to eat";

				// Token: 0x0400DF94 RID: 57236
				public static LocString NOTIFICATION_NAME = "No food available";

				// Token: 0x0400DF95 RID: 57237
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants have nothing to eat:";
			}

			// Token: 0x020035BE RID: 13758
			public class QUARANTINEAREAUNREACHABLE
			{
				// Token: 0x0400DF96 RID: 57238
				public static LocString NAME = "Cannot reach quarantine";

				// Token: 0x0400DF97 RID: 57239
				public static LocString TOOLTIP = "This Duplicant cannot reach their quarantine zone";

				// Token: 0x0400DF98 RID: 57240
				public static LocString NOTIFICATION_NAME = "Unreachable quarantine";

				// Token: 0x0400DF99 RID: 57241
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants cannot reach their assigned quarantine zones:";
			}

			// Token: 0x020035BF RID: 13759
			public class QUARANTINED
			{
				// Token: 0x0400DF9A RID: 57242
				public static LocString NAME = "Quarantined";

				// Token: 0x0400DF9B RID: 57243
				public static LocString TOOLTIP = "This Duplicant has been isolated from the colony";
			}

			// Token: 0x020035C0 RID: 13760
			public class RATIONSUNREACHABLE
			{
				// Token: 0x0400DF9C RID: 57244
				public static LocString NAME = "Cannot reach food";

				// Token: 0x0400DF9D RID: 57245
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" in the colony that this Duplicant cannot reach"
				});

				// Token: 0x0400DF9E RID: 57246
				public static LocString NOTIFICATION_NAME = "Unreachable food";

				// Token: 0x0400DF9F RID: 57247
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot access the colony's ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020035C1 RID: 13761
			public class RATIONSNOTPERMITTED
			{
				// Token: 0x0400DFA0 RID: 57248
				public static LocString NAME = "Food Type Not Permitted";

				// Token: 0x0400DFA1 RID: 57249
				public static LocString TOOLTIP = "This Duplicant is not allowed to eat any of the " + UI.FormatAsLink("Food", "FOOD") + " in their reach\n\nEnter the <color=#833A5FFF>CONSUMABLES</color> <color=#F44A47><b>[F]</b></color> to adjust their food permissions";

				// Token: 0x0400DFA2 RID: 57250
				public static LocString NOTIFICATION_NAME = "Unpermitted food";

				// Token: 0x0400DFA3 RID: 57251
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants' <color=#833A5FFF>CONSUMABLES</color> <color=#F44A47><b>[F]</b></color> permissions prevent them from eating any of the " + UI.FormatAsLink("Food", "FOOD") + " within their reach:";
			}

			// Token: 0x020035C2 RID: 13762
			public class ROTTEN
			{
				// Token: 0x0400DFA4 RID: 57252
				public static LocString NAME = "Rotten";

				// Token: 0x0400DFA5 RID: 57253
				public static LocString TOOLTIP = "Gross!";
			}

			// Token: 0x020035C3 RID: 13763
			public class STARVING
			{
				// Token: 0x0400DFA6 RID: 57254
				public static LocString NAME = "Starving";

				// Token: 0x0400DFA7 RID: 57255
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is about to die and needs ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					"!"
				});

				// Token: 0x0400DFA8 RID: 57256
				public static LocString NOTIFICATION_NAME = "Starvation";

				// Token: 0x0400DFA9 RID: 57257
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants are starving and will die if they can't find ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020035C4 RID: 13764
			public class STRESS_SIGNAL_AGGRESIVE
			{
				// Token: 0x0400DFAA RID: 57258
				public static LocString NAME = "Frustrated";

				// Token: 0x0400DFAB RID: 57259
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is trying to keep their cool\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they destroy something to let off steam"
				});
			}

			// Token: 0x020035C5 RID: 13765
			public class STRESS_SIGNAL_BINGE_EAT
			{
				// Token: 0x0400DFAC RID: 57260
				public static LocString NAME = "Stress Cravings";

				// Token: 0x0400DFAD RID: 57261
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is consumed by hunger\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they eat all the colony's ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" stores"
				});
			}

			// Token: 0x020035C6 RID: 13766
			public class STRESS_SIGNAL_UGLY_CRIER
			{
				// Token: 0x0400DFAE RID: 57262
				public static LocString NAME = "Misty Eyed";

				// Token: 0x0400DFAF RID: 57263
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is trying and failing to swallow their emotions\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they have a good ugly cry"
				});
			}

			// Token: 0x020035C7 RID: 13767
			public class STRESS_SIGNAL_VOMITER
			{
				// Token: 0x0400DFB0 RID: 57264
				public static LocString NAME = "Stress Burp";

				// Token: 0x0400DFB1 RID: 57265
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Sort of like having butterflies in your stomach, except they're burps\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they start to stress vomit"
				});
			}

			// Token: 0x020035C8 RID: 13768
			public class STRESS_SIGNAL_BANSHEE
			{
				// Token: 0x0400DFB2 RID: 57266
				public static LocString NAME = "Suppressed Screams";

				// Token: 0x0400DFB3 RID: 57267
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is fighting the urge to scream\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they start wailing uncontrollably"
				});
			}

			// Token: 0x020035C9 RID: 13769
			public class STRESS_SIGNAL_STRESS_SHOCKER
			{
				// Token: 0x0400DFB4 RID: 57268
				public static LocString NAME = "Dangerously Frayed";

				// Token: 0x0400DFB5 RID: 57269
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's hanging by a thread...except the thread is a live wire\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they zap someone"
				});
			}

			// Token: 0x020035CA RID: 13770
			public class ENTOMBEDCHORE
			{
				// Token: 0x0400DFB6 RID: 57270
				public static LocString NAME = "Entombed";

				// Token: 0x0400DFB7 RID: 57271
				public static LocString TOOLTIP = "This Duplicant needs someone to help dig them out!";

				// Token: 0x0400DFB8 RID: 57272
				public static LocString NOTIFICATION_NAME = "Entombed";

				// Token: 0x0400DFB9 RID: 57273
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are trapped:";
			}

			// Token: 0x020035CB RID: 13771
			public class EARLYMORNING
			{
				// Token: 0x0400DFBA RID: 57274
				public static LocString NAME = "Early Bird";

				// Token: 0x0400DFBB RID: 57275
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is jazzed to start the day\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+2</b> in the morning"
				});
			}

			// Token: 0x020035CC RID: 13772
			public class NIGHTTIME
			{
				// Token: 0x0400DFBC RID: 57276
				public static LocString NAME = "Night Owl";

				// Token: 0x0400DFBD RID: 57277
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is more efficient on a nighttime ",
					UI.PRE_KEYWORD,
					"Schedule",
					UI.PST_KEYWORD,
					"\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+3</b> at night"
				});
			}

			// Token: 0x020035CD RID: 13773
			public class METEORPHILE
			{
				// Token: 0x0400DFBE RID: 57278
				public static LocString NAME = "Rock Fan";

				// Token: 0x0400DFBF RID: 57279
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is <i>really</i> into meteor showers\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+3</b> during meteor showers"
				});
			}

			// Token: 0x020035CE RID: 13774
			public class SUFFOCATING
			{
				// Token: 0x0400DFC0 RID: 57280
				public static LocString NAME = "Suffocating";

				// Token: 0x0400DFC1 RID: 57281
				public static LocString TOOLTIP = "This Duplicant cannot breathe!";

				// Token: 0x0400DFC2 RID: 57282
				public static LocString NOTIFICATION_NAME = "Suffocating";

				// Token: 0x0400DFC3 RID: 57283
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants cannot breathe:";
			}

			// Token: 0x020035CF RID: 13775
			public class TIRED
			{
				// Token: 0x0400DFC4 RID: 57284
				public static LocString NAME = "Tired";

				// Token: 0x0400DFC5 RID: 57285
				public static LocString TOOLTIP = "This Duplicant could use a nice nap";
			}

			// Token: 0x020035D0 RID: 13776
			public class IDLE
			{
				// Token: 0x0400DFC6 RID: 57286
				public static LocString NAME = "Idle";

				// Token: 0x0400DFC7 RID: 57287
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending errands";

				// Token: 0x0400DFC8 RID: 57288
				public static LocString NOTIFICATION_NAME = "Idle";

				// Token: 0x0400DFC9 RID: 57289
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach any pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020035D1 RID: 13777
			public class IDLEINROCKETS
			{
				// Token: 0x0400DFCA RID: 57290
				public static LocString NAME = "Idle";

				// Token: 0x0400DFCB RID: 57291
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending errands";

				// Token: 0x0400DFCC RID: 57292
				public static LocString NOTIFICATION_NAME = "Idle";

				// Token: 0x0400DFCD RID: 57293
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach any pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020035D2 RID: 13778
			public class FIGHTING
			{
				// Token: 0x0400DFCE RID: 57294
				public static LocString NAME = "In combat";

				// Token: 0x0400DFCF RID: 57295
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is attacking a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"!"
				});

				// Token: 0x0400DFD0 RID: 57296
				public static LocString NOTIFICATION_NAME = "Combat!";

				// Token: 0x0400DFD1 RID: 57297
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have engaged a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" in combat:"
				});
			}

			// Token: 0x020035D3 RID: 13779
			public class FLEEING
			{
				// Token: 0x0400DFD2 RID: 57298
				public static LocString NAME = "Fleeing";

				// Token: 0x0400DFD3 RID: 57299
				public static LocString TOOLTIP = "This Duplicant is trying to escape something scary!";

				// Token: 0x0400DFD4 RID: 57300
				public static LocString NOTIFICATION_NAME = "Fleeing!";

				// Token: 0x0400DFD5 RID: 57301
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are trying to escape:";
			}

			// Token: 0x020035D4 RID: 13780
			public class DEAD
			{
				// Token: 0x0400DFD6 RID: 57302
				public static LocString NAME = "Dead: {Death}";

				// Token: 0x0400DFD7 RID: 57303
				public static LocString TOOLTIP = "This Duplicant definitely isn't sleeping";
			}

			// Token: 0x020035D5 RID: 13781
			public class LASHINGOUT
			{
				// Token: 0x0400DFD8 RID: 57304
				public static LocString NAME = "Lashing out";

				// Token: 0x0400DFD9 RID: 57305
				public static LocString TOOLTIP = "This Duplicant is breaking buildings to relieve their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400DFDA RID: 57306
				public static LocString NOTIFICATION_NAME = "Lashing out";

				// Token: 0x0400DFDB RID: 57307
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants broke buildings to relieve their ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020035D6 RID: 13782
			public class MOVETOSUITNOTREQUIRED
			{
				// Token: 0x0400DFDC RID: 57308
				public static LocString NAME = "Exiting Exosuit area";

				// Token: 0x0400DFDD RID: 57309
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is leaving an area where a ",
					UI.PRE_KEYWORD,
					"Suit",
					UI.PST_KEYWORD,
					" was required"
				});
			}

			// Token: 0x020035D7 RID: 13783
			public class NOROLE
			{
				// Token: 0x0400DFDE RID: 57310
				public static LocString NAME = "No Job";

				// Token: 0x0400DFDF RID: 57311
				public static LocString TOOLTIP = "This Duplicant does not have a Job Assignment\n\nEnter the " + UI.FormatAsManagementMenu("Jobs Panel", "[J]") + " to view all available Jobs";
			}

			// Token: 0x020035D8 RID: 13784
			public class DROPPINGUNUSEDINVENTORY
			{
				// Token: 0x0400DFE0 RID: 57312
				public static LocString NAME = "Dropping objects";

				// Token: 0x0400DFE1 RID: 57313
				public static LocString TOOLTIP = "This Duplicant is dropping what they're holding";
			}

			// Token: 0x020035D9 RID: 13785
			public class MOVINGTOSAFEAREA
			{
				// Token: 0x0400DFE2 RID: 57314
				public static LocString NAME = "Moving to safe area";

				// Token: 0x0400DFE3 RID: 57315
				public static LocString TOOLTIP = "This Duplicant is finding a less dangerous place";
			}

			// Token: 0x020035DA RID: 13786
			public class TOILETUNREACHABLE
			{
				// Token: 0x0400DFE4 RID: 57316
				public static LocString NAME = "Unreachable toilet";

				// Token: 0x0400DFE5 RID: 57317
				public static LocString TOOLTIP = "This Duplicant cannot reach a functioning " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " or " + UI.FormatAsLink("Lavatory", "FLUSHTOILET");

				// Token: 0x0400DFE6 RID: 57318
				public static LocString NOTIFICATION_NAME = "Unreachable toilet";

				// Token: 0x0400DFE7 RID: 57319
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach a functioning ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatory", "FLUSHTOILET"),
					":"
				});
			}

			// Token: 0x020035DB RID: 13787
			public class NOUSABLETOILETS
			{
				// Token: 0x0400DFE8 RID: 57320
				public static LocString NAME = "Toilet out of order";

				// Token: 0x0400DFE9 RID: 57321
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The only ",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatories", "FLUSHTOILET"),
					" in this Duplicant's reach are out of order"
				});

				// Token: 0x0400DFEA RID: 57322
				public static LocString NOTIFICATION_NAME = "Toilet out of order";

				// Token: 0x0400DFEB RID: 57323
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants want to use an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatory", "FLUSHTOILET"),
					" that is out of order:"
				});
			}

			// Token: 0x020035DC RID: 13788
			public class NOTOILETS
			{
				// Token: 0x0400DFEC RID: 57324
				public static LocString NAME = "No Outhouses";

				// Token: 0x0400DFED RID: 57325
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There are no ",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" available for this Duplicant\n\n",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400DFEE RID: 57326
				public static LocString NOTIFICATION_NAME = "No Outhouses built";

				// Token: 0x0400DFEF RID: 57327
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					".\n\nThese Duplicants are in need of an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					":"
				});
			}

			// Token: 0x020035DD RID: 13789
			public class FULLBLADDER
			{
				// Token: 0x0400DFF0 RID: 57328
				public static LocString NAME = "Full bladder";

				// Token: 0x0400DFF1 RID: 57329
				public static LocString TOOLTIP = "This Duplicant would really appreciate an " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " or " + UI.FormatAsLink("Lavatory", "FLUSHTOILET");
			}

			// Token: 0x020035DE RID: 13790
			public class STRESSFULLYEMPTYINGOIL
			{
				// Token: 0x0400DFF2 RID: 57330
				public static LocString NAME = "Expelling gunk";

				// Token: 0x0400DFF3 RID: 57331
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Bionic Duplicant couldn't get to a ",
					UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER"),
					" in time and got desperate\n\n",
					UI.FormatAsLink("Gunk Extractors", "GUNKEMPTIER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400DFF4 RID: 57332
				public static LocString NOTIFICATION_NAME = "Expelled gunk";

				// Token: 0x0400DFF5 RID: 57333
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can be used to clean up Duplicant-related \"spills\"\n\nThese Duplicants made messes that require cleaning up:\n";
			}

			// Token: 0x020035DF RID: 13791
			public class STRESSFULLYEMPTYINGBLADDER
			{
				// Token: 0x0400DFF6 RID: 57334
				public static LocString NAME = "Making a mess";

				// Token: 0x0400DFF7 RID: 57335
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This poor Duplicant couldn't find an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" in time and is super embarrassed\n\n",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400DFF8 RID: 57336
				public static LocString NOTIFICATION_NAME = "Made a mess";

				// Token: 0x0400DFF9 RID: 57337
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can be used to clean up Duplicant-related \"spills\"\n\nThese Duplicants made messes that require cleaning up:\n";
			}

			// Token: 0x020035E0 RID: 13792
			public class WASHINGHANDS
			{
				// Token: 0x0400DFFA RID: 57338
				public static LocString NAME = "Washing hands";

				// Token: 0x0400DFFB RID: 57339
				public static LocString TOOLTIP = "This Duplicant is washing their hands";
			}

			// Token: 0x020035E1 RID: 13793
			public class SHOWERING
			{
				// Token: 0x0400DFFC RID: 57340
				public static LocString NAME = "Showering";

				// Token: 0x0400DFFD RID: 57341
				public static LocString TOOLTIP = "This Duplicant is gonna be squeaky clean";
			}

			// Token: 0x020035E2 RID: 13794
			public class RELAXING
			{
				// Token: 0x0400DFFE RID: 57342
				public static LocString NAME = "Relaxing";

				// Token: 0x0400DFFF RID: 57343
				public static LocString TOOLTIP = "This Duplicant's just taking it easy";
			}

			// Token: 0x020035E3 RID: 13795
			public class VOMITING
			{
				// Token: 0x0400E000 RID: 57344
				public static LocString NAME = "Throwing up";

				// Token: 0x0400E001 RID: 57345
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has unceremoniously hurled as the result of a ",
					UI.FormatAsLink("Disease", "DISEASE"),
					"\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400E002 RID: 57346
				public static LocString NOTIFICATION_NAME = "Throwing up";

				// Token: 0x0400E003 RID: 57347
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" can be used to clean up Duplicant-related \"spills\"\n\nA ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" has caused these Duplicants to throw up:"
				});
			}

			// Token: 0x020035E4 RID: 13796
			public class STRESSVOMITING
			{
				// Token: 0x0400E004 RID: 57348
				public static LocString NAME = "Stress vomiting";

				// Token: 0x0400E005 RID: 57349
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is relieving their ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" all over the floor\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400E006 RID: 57350
				public static LocString NOTIFICATION_NAME = "Stress vomiting";

				// Token: 0x0400E007 RID: 57351
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" can used to clean up Duplicant-related \"spills\"\n\nThese Duplicants became so ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" they threw up:"
				});
			}

			// Token: 0x020035E5 RID: 13797
			public class RADIATIONVOMITING
			{
				// Token: 0x0400E008 RID: 57352
				public static LocString NAME = "Radiation vomiting";

				// Token: 0x0400E009 RID: 57353
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is sick due to ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" poisoning.\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400E00A RID: 57354
				public static LocString NOTIFICATION_NAME = "Radiation vomiting";

				// Token: 0x0400E00B RID: 57355
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can clean up Duplicant-related \"spills\"\n\nRadiation Sickness caused these Duplicants to throw up:";
			}

			// Token: 0x020035E6 RID: 13798
			public class HASDISEASE
			{
				// Token: 0x0400E00C RID: 57356
				public static LocString NAME = "Feeling ill";

				// Token: 0x0400E00D RID: 57357
				public static LocString TOOLTIP = "This Duplicant has contracted a " + UI.FormatAsLink("Disease", "DISEASE") + " and requires recovery time at a " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION");

				// Token: 0x0400E00E RID: 57358
				public static LocString NOTIFICATION_NAME = "Illness";

				// Token: 0x0400E00F RID: 57359
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have contracted a ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" and require recovery time at a ",
					UI.FormatAsLink("Sick Bay", "DOCTORSTATION"),
					":"
				});
			}

			// Token: 0x020035E7 RID: 13799
			public class BODYREGULATINGHEATING
			{
				// Token: 0x0400E010 RID: 57360
				public static LocString NAME = "Regulating temperature at: {TempDelta}";

				// Token: 0x0400E011 RID: 57361
				public static LocString TOOLTIP = "This Duplicant is regulating their internal " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x020035E8 RID: 13800
			public class BODYREGULATINGCOOLING
			{
				// Token: 0x0400E012 RID: 57362
				public static LocString NAME = "Regulating temperature at: {TempDelta}";

				// Token: 0x0400E013 RID: 57363
				public static LocString TOOLTIP = "This Duplicant is regulating their internal " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x020035E9 RID: 13801
			public class BREATHINGO2
			{
				// Token: 0x0400E014 RID: 57364
				public static LocString NAME = "Inhaling {ConsumptionRate} O<sub>2</sub>";

				// Token: 0x0400E015 RID: 57365
				public static LocString TOOLTIP = "Duplicants require " + UI.FormatAsLink("Oxygen", "OXYGEN") + " to live";
			}

			// Token: 0x020035EA RID: 13802
			public class EMITTINGCO2
			{
				// Token: 0x0400E016 RID: 57366
				public static LocString NAME = "Exhaling {EmittingRate} CO<sub>2</sub>";

				// Token: 0x0400E017 RID: 57367
				public static LocString TOOLTIP = "Duplicants breathe out " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE");
			}

			// Token: 0x020035EB RID: 13803
			public class BREATHINGO2BIONIC
			{
				// Token: 0x0400E018 RID: 57368
				public static LocString NAME = "Oxygen Tank: {ConsumptionRate} O<sub>2</sub>";

				// Token: 0x0400E019 RID: 57369
				public static LocString TOOLTIP = "Bionic Duplicants consume " + UI.FormatAsLink("Oxygen", "OXYGEN") + " from their internal tanks";
			}

			// Token: 0x020035EC RID: 13804
			public class PICKUPDELIVERSTATUS
			{
				// Token: 0x0400E01A RID: 57370
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E01B RID: 57371
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035ED RID: 13805
			public class STOREDELIVERSTATUS
			{
				// Token: 0x0400E01C RID: 57372
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E01D RID: 57373
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035EE RID: 13806
			public class CLEARDELIVERSTATUS
			{
				// Token: 0x0400E01E RID: 57374
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E01F RID: 57375
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035EF RID: 13807
			public class STOREFORBUILDDELIVERSTATUS
			{
				// Token: 0x0400E020 RID: 57376
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E021 RID: 57377
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035F0 RID: 13808
			public class STOREFORBUILDPRIORITIZEDDELIVERSTATUS
			{
				// Token: 0x0400E022 RID: 57378
				public static LocString NAME = "Allocating {Item} to {Target}";

				// Token: 0x0400E023 RID: 57379
				public static LocString TOOLTIP = "This Duplicant is delivering materials to a <b>{Target}</b> construction errand";
			}

			// Token: 0x020035F1 RID: 13809
			public class BUILDDELIVERSTATUS
			{
				// Token: 0x0400E024 RID: 57380
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E025 RID: 57381
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035F2 RID: 13810
			public class BUILDPRIORITIZEDSTATUS
			{
				// Token: 0x0400E026 RID: 57382
				public static LocString NAME = "Building {Target}";

				// Token: 0x0400E027 RID: 57383
				public static LocString TOOLTIP = "This Duplicant is constructing a <b>{Target}</b>";
			}

			// Token: 0x020035F3 RID: 13811
			public class FABRICATEDELIVERSTATUS
			{
				// Token: 0x0400E028 RID: 57384
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E029 RID: 57385
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035F4 RID: 13812
			public class USEITEMDELIVERSTATUS
			{
				// Token: 0x0400E02A RID: 57386
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E02B RID: 57387
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035F5 RID: 13813
			public class STOREPRIORITYDELIVERSTATUS
			{
				// Token: 0x0400E02C RID: 57388
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E02D RID: 57389
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035F6 RID: 13814
			public class STORECRITICALDELIVERSTATUS
			{
				// Token: 0x0400E02E RID: 57390
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E02F RID: 57391
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035F7 RID: 13815
			public class COMPOSTFLIPSTATUS
			{
				// Token: 0x0400E030 RID: 57392
				public static LocString NAME = "Going to flip compost";

				// Token: 0x0400E031 RID: 57393
				public static LocString TOOLTIP = "This Duplicant is going to flip the " + BUILDINGS.PREFABS.COMPOST.NAME;
			}

			// Token: 0x020035F8 RID: 13816
			public class DECONSTRUCTDELIVERSTATUS
			{
				// Token: 0x0400E032 RID: 57394
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E033 RID: 57395
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035F9 RID: 13817
			public class TOGGLEDELIVERSTATUS
			{
				// Token: 0x0400E034 RID: 57396
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E035 RID: 57397
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035FA RID: 13818
			public class EMPTYSTORAGEDELIVERSTATUS
			{
				// Token: 0x0400E036 RID: 57398
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E037 RID: 57399
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035FB RID: 13819
			public class HARVESTDELIVERSTATUS
			{
				// Token: 0x0400E038 RID: 57400
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E039 RID: 57401
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035FC RID: 13820
			public class SLEEPDELIVERSTATUS
			{
				// Token: 0x0400E03A RID: 57402
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E03B RID: 57403
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035FD RID: 13821
			public class EATDELIVERSTATUS
			{
				// Token: 0x0400E03C RID: 57404
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E03D RID: 57405
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x020035FE RID: 13822
			public class WATCHROBODANCERWORKABLE
			{
				// Token: 0x0400E03E RID: 57406
				public static LocString NAME = "Watching Flash Mobber";

				// Token: 0x0400E03F RID: 57407
				public static LocString STATUS = "Watching Flash Mobber";

				// Token: 0x0400E040 RID: 57408
				public static LocString TOOLTIP = "This Duplicant is blown away by their friend's dance moves!";
			}

			// Token: 0x020035FF RID: 13823
			public class WARMUPDELIVERSTATUS
			{
				// Token: 0x0400E041 RID: 57409
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E042 RID: 57410
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003600 RID: 13824
			public class REPAIRDELIVERSTATUS
			{
				// Token: 0x0400E043 RID: 57411
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E044 RID: 57412
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003601 RID: 13825
			public class REPAIRWORKSTATUS
			{
				// Token: 0x0400E045 RID: 57413
				public static LocString NAME = "Repairing {Target}";

				// Token: 0x0400E046 RID: 57414
				public static LocString TOOLTIP = "This Duplicant is fixing the <b>{Target}</b>";
			}

			// Token: 0x02003602 RID: 13826
			public class BREAKDELIVERSTATUS
			{
				// Token: 0x0400E047 RID: 57415
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E048 RID: 57416
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003603 RID: 13827
			public class BREAKWORKSTATUS
			{
				// Token: 0x0400E049 RID: 57417
				public static LocString NAME = "Breaking {Target}";

				// Token: 0x0400E04A RID: 57418
				public static LocString TOOLTIP = "This Duplicant is going totally bananas on the <b>{Target}</b>!";
			}

			// Token: 0x02003604 RID: 13828
			public class EQUIPDELIVERSTATUS
			{
				// Token: 0x0400E04B RID: 57419
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E04C RID: 57420
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003605 RID: 13829
			public class COOKDELIVERSTATUS
			{
				// Token: 0x0400E04D RID: 57421
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E04E RID: 57422
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003606 RID: 13830
			public class MUSHDELIVERSTATUS
			{
				// Token: 0x0400E04F RID: 57423
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E050 RID: 57424
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003607 RID: 13831
			public class PACIFYDELIVERSTATUS
			{
				// Token: 0x0400E051 RID: 57425
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E052 RID: 57426
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003608 RID: 13832
			public class RESCUEDELIVERSTATUS
			{
				// Token: 0x0400E053 RID: 57427
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E054 RID: 57428
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003609 RID: 13833
			public class RESCUEWORKSTATUS
			{
				// Token: 0x0400E055 RID: 57429
				public static LocString NAME = "Rescuing {Target}";

				// Token: 0x0400E056 RID: 57430
				public static LocString TOOLTIP = "This Duplicant is saving <b>{Target}</b> from certain peril!";
			}

			// Token: 0x0200360A RID: 13834
			public class MOPDELIVERSTATUS
			{
				// Token: 0x0400E057 RID: 57431
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400E058 RID: 57432
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200360B RID: 13835
			public class DIGGING
			{
				// Token: 0x0400E059 RID: 57433
				public static LocString NAME = "Digging";

				// Token: 0x0400E05A RID: 57434
				public static LocString TOOLTIP = "This Duplicant is excavating raw resources";
			}

			// Token: 0x0200360C RID: 13836
			public class EATING
			{
				// Token: 0x0400E05B RID: 57435
				public static LocString NAME = "Eating {Target}";

				// Token: 0x0400E05C RID: 57436
				public static LocString TOOLTIP = "This Duplicant is having a meal";
			}

			// Token: 0x0200360D RID: 13837
			public class CLEANING
			{
				// Token: 0x0400E05D RID: 57437
				public static LocString NAME = "Cleaning {Target}";

				// Token: 0x0400E05E RID: 57438
				public static LocString TOOLTIP = "This Duplicant is cleaning the <b>{Target}</b>";
			}

			// Token: 0x0200360E RID: 13838
			public class LIGHTWORKEFFICIENCYBONUS
			{
				// Token: 0x0400E05F RID: 57439
				public static LocString NAME = "Lit Workspace";

				// Token: 0x0400E060 RID: 57440
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Better visibility from the ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is allowing this Duplicant to work faster:\n    {0}"
				});

				// Token: 0x0400E061 RID: 57441
				public static LocString NO_BUILDING_WORK_ATTRIBUTE = "{0} Speed";
			}

			// Token: 0x0200360F RID: 13839
			public class LABORATORYWORKEFFICIENCYBONUS
			{
				// Token: 0x0400E062 RID: 57442
				public static LocString NAME = "Lab Workspace";

				// Token: 0x0400E063 RID: 57443
				public static LocString TOOLTIP = "Working in a Laboratory is allowing this Duplicant to work faster:\n    {0}";

				// Token: 0x0400E064 RID: 57444
				public static LocString NO_BUILDING_WORK_ATTRIBUTE = "{0} Speed";
			}

			// Token: 0x02003610 RID: 13840
			public class PICKINGUP
			{
				// Token: 0x0400E065 RID: 57445
				public static LocString NAME = "Picking up {Target}";

				// Token: 0x0400E066 RID: 57446
				public static LocString TOOLTIP = "This Duplicant is retrieving <b>{Target}</b>";
			}

			// Token: 0x02003611 RID: 13841
			public class MOPPING
			{
				// Token: 0x0400E067 RID: 57447
				public static LocString NAME = "Mopping";

				// Token: 0x0400E068 RID: 57448
				public static LocString TOOLTIP = "This Duplicant is cleaning up a nasty spill";
			}

			// Token: 0x02003612 RID: 13842
			public class ARTING
			{
				// Token: 0x0400E069 RID: 57449
				public static LocString NAME = "Decorating";

				// Token: 0x0400E06A RID: 57450
				public static LocString TOOLTIP = "This Duplicant is hard at work on their art";
			}

			// Token: 0x02003613 RID: 13843
			public class MUSHING
			{
				// Token: 0x0400E06B RID: 57451
				public static LocString NAME = "Mushing {Item}";

				// Token: 0x0400E06C RID: 57452
				public static LocString TOOLTIP = "This Duplicant is cooking a <b>{Item}</b>";
			}

			// Token: 0x02003614 RID: 13844
			public class COOKING
			{
				// Token: 0x0400E06D RID: 57453
				public static LocString NAME = "Cooking {Item}";

				// Token: 0x0400E06E RID: 57454
				public static LocString TOOLTIP = "This Duplicant is cooking up a tasty <b>{Item}</b>";
			}

			// Token: 0x02003615 RID: 13845
			public class RESEARCHING
			{
				// Token: 0x0400E06F RID: 57455
				public static LocString NAME = "Researching {Tech}";

				// Token: 0x0400E070 RID: 57456
				public static LocString TOOLTIP = "This Duplicant is intently researching <b>{Tech}</b> technology";
			}

			// Token: 0x02003616 RID: 13846
			public class RESEARCHING_FROM_POI
			{
				// Token: 0x0400E071 RID: 57457
				public static LocString NAME = "Unlocking Research";

				// Token: 0x0400E072 RID: 57458
				public static LocString TOOLTIP = "This Duplicant is unlocking crucial technology";
			}

			// Token: 0x02003617 RID: 13847
			public class MISSIONCONTROLLING
			{
				// Token: 0x0400E073 RID: 57459
				public static LocString NAME = "Mission Controlling";

				// Token: 0x0400E074 RID: 57460
				public static LocString TOOLTIP = "This Duplicant is guiding a " + UI.PRE_KEYWORD + "Rocket" + UI.PST_KEYWORD;
			}

			// Token: 0x02003618 RID: 13848
			public class STORING
			{
				// Token: 0x0400E075 RID: 57461
				public static LocString NAME = "Storing {Item}";

				// Token: 0x0400E076 RID: 57462
				public static LocString TOOLTIP = "This Duplicant is putting <b>{Item}</b> away in <b>{Target}</b>";
			}

			// Token: 0x02003619 RID: 13849
			public class BUILDING
			{
				// Token: 0x0400E077 RID: 57463
				public static LocString NAME = "Building {Target}";

				// Token: 0x0400E078 RID: 57464
				public static LocString TOOLTIP = "This Duplicant is constructing a <b>{Target}</b>";
			}

			// Token: 0x0200361A RID: 13850
			public class EQUIPPING
			{
				// Token: 0x0400E079 RID: 57465
				public static LocString NAME = "Equipping {Target}";

				// Token: 0x0400E07A RID: 57466
				public static LocString TOOLTIP = "This Duplicant is equipping a <b>{Target}</b>";
			}

			// Token: 0x0200361B RID: 13851
			public class WARMINGUP
			{
				// Token: 0x0400E07B RID: 57467
				public static LocString NAME = "Warming up";

				// Token: 0x0400E07C RID: 57468
				public static LocString TOOLTIP = "This Duplicant got too cold and is trying to warm up";
			}

			// Token: 0x0200361C RID: 13852
			public class GENERATINGPOWER
			{
				// Token: 0x0400E07D RID: 57469
				public static LocString NAME = "Generating power";

				// Token: 0x0400E07E RID: 57470
				public static LocString TOOLTIP = "This Duplicant is using the <b>{Target}</b> to produce electrical " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
			}

			// Token: 0x0200361D RID: 13853
			public class HARVESTING
			{
				// Token: 0x0400E07F RID: 57471
				public static LocString NAME = "Harvesting {Target}";

				// Token: 0x0400E080 RID: 57472
				public static LocString TOOLTIP = "This Duplicant is gathering resources from a <b>{Target}</b>";
			}

			// Token: 0x0200361E RID: 13854
			public class UPROOTING
			{
				// Token: 0x0400E081 RID: 57473
				public static LocString NAME = "Uprooting {Target}";

				// Token: 0x0400E082 RID: 57474
				public static LocString TOOLTIP = "This Duplicant is digging up a <b>{Target}</b>";
			}

			// Token: 0x0200361F RID: 13855
			public class EMPTYING
			{
				// Token: 0x0400E083 RID: 57475
				public static LocString NAME = "Emptying {Target}";

				// Token: 0x0400E084 RID: 57476
				public static LocString TOOLTIP = "This Duplicant is removing materials from the <b>{Target}</b>";
			}

			// Token: 0x02003620 RID: 13856
			public class TOGGLING
			{
				// Token: 0x0400E085 RID: 57477
				public static LocString NAME = "Change {Target} setting";

				// Token: 0x0400E086 RID: 57478
				public static LocString TOOLTIP = "This Duplicant is changing the <b>{Target}</b>'s setting";
			}

			// Token: 0x02003621 RID: 13857
			public class DECONSTRUCTING
			{
				// Token: 0x0400E087 RID: 57479
				public static LocString NAME = "Deconstructing {Target}";

				// Token: 0x0400E088 RID: 57480
				public static LocString TOOLTIP = "This Duplicant is deconstructing the <b>{Target}</b>";
			}

			// Token: 0x02003622 RID: 13858
			public class DEMOLISHING
			{
				// Token: 0x0400E089 RID: 57481
				public static LocString NAME = "Demolishing {Target}";

				// Token: 0x0400E08A RID: 57482
				public static LocString TOOLTIP = "This Duplicant is demolishing the <b>{Target}</b>";
			}

			// Token: 0x02003623 RID: 13859
			public class DISINFECTING
			{
				// Token: 0x0400E08B RID: 57483
				public static LocString NAME = "Disinfecting {Target}";

				// Token: 0x0400E08C RID: 57484
				public static LocString TOOLTIP = "This Duplicant is disinfecting <b>{Target}</b>";
			}

			// Token: 0x02003624 RID: 13860
			public class FABRICATING
			{
				// Token: 0x0400E08D RID: 57485
				public static LocString NAME = "Fabricating {Item}";

				// Token: 0x0400E08E RID: 57486
				public static LocString TOOLTIP = "This Duplicant is crafting a <b>{Item}</b>";
			}

			// Token: 0x02003625 RID: 13861
			public class PROCESSING
			{
				// Token: 0x0400E08F RID: 57487
				public static LocString NAME = "Refining {Item}";

				// Token: 0x0400E090 RID: 57488
				public static LocString TOOLTIP = "This Duplicant is refining <b>{Item}</b>";
			}

			// Token: 0x02003626 RID: 13862
			public class SPICING
			{
				// Token: 0x0400E091 RID: 57489
				public static LocString NAME = "Spicing Food";

				// Token: 0x0400E092 RID: 57490
				public static LocString TOOLTIP = "This Duplicant is making a tasty meal even tastier";
			}

			// Token: 0x02003627 RID: 13863
			public class CLEARING
			{
				// Token: 0x0400E093 RID: 57491
				public static LocString NAME = "Sweeping {Target}";

				// Token: 0x0400E094 RID: 57492
				public static LocString TOOLTIP = "This Duplicant is sweeping away <b>{Target}</b>";
			}

			// Token: 0x02003628 RID: 13864
			public class STUDYING
			{
				// Token: 0x0400E095 RID: 57493
				public static LocString NAME = "Analyzing";

				// Token: 0x0400E096 RID: 57494
				public static LocString TOOLTIP = "This Duplicant is conducting a field study of a Natural Feature";
			}

			// Token: 0x02003629 RID: 13865
			public class INSTALLINGELECTROBANK
			{
				// Token: 0x0400E097 RID: 57495
				public static LocString NAME = "Rescuing Bionic Friend";

				// Token: 0x0400E098 RID: 57496
				public static LocString TOOLTIP = "This Duplicant is rebooting a powerless Bionic Duplicant";
			}

			// Token: 0x0200362A RID: 13866
			public class SOCIALIZING
			{
				// Token: 0x0400E099 RID: 57497
				public static LocString NAME = "Socializing";

				// Token: 0x0400E09A RID: 57498
				public static LocString TOOLTIP = "This Duplicant is using their break to hang out";
			}

			// Token: 0x0200362B RID: 13867
			public class BIONICEXPLORERBOOSTER
			{
				// Token: 0x0400E09B RID: 57499
				public static LocString NOTIFICATION_NAME = "Dowsing Complete: Geyser Discovered";

				// Token: 0x0400E09C RID: 57500
				public static LocString NOTIFICATION_TOOLTIP = "Click to see the geyser recently discovered by a Bionic Duplicant";

				// Token: 0x0400E09D RID: 57501
				public static LocString NAME = "Dowsing {0}";

				// Token: 0x0400E09E RID: 57502
				public static LocString TOOLTIP = "This Duplicant's always gathering geodata\n\nWhen dowsing is complete, a new geyser will be revealed in the world";
			}

			// Token: 0x0200362C RID: 13868
			public class MINGLING
			{
				// Token: 0x0400E09F RID: 57503
				public static LocString NAME = "Mingling";

				// Token: 0x0400E0A0 RID: 57504
				public static LocString TOOLTIP = "This Duplicant is using their break to chat with friends";
			}

			// Token: 0x0200362D RID: 13869
			public class NOISEPEACEFUL
			{
				// Token: 0x0400E0A1 RID: 57505
				public static LocString NAME = "Peace and Quiet";

				// Token: 0x0400E0A2 RID: 57506
				public static LocString TOOLTIP = "This Duplicant has found a quiet place to concentrate";
			}

			// Token: 0x0200362E RID: 13870
			public class NOISEMINOR
			{
				// Token: 0x0400E0A3 RID: 57507
				public static LocString NAME = "Loud Noises";

				// Token: 0x0400E0A4 RID: 57508
				public static LocString TOOLTIP = "This area is a bit too loud for comfort";
			}

			// Token: 0x0200362F RID: 13871
			public class NOISEMAJOR
			{
				// Token: 0x0400E0A5 RID: 57509
				public static LocString NAME = "Cacophony!";

				// Token: 0x0400E0A6 RID: 57510
				public static LocString TOOLTIP = "It's very, very loud in here!";
			}

			// Token: 0x02003630 RID: 13872
			public class LOWIMMUNITY
			{
				// Token: 0x0400E0A7 RID: 57511
				public static LocString NAME = "Under the Weather";

				// Token: 0x0400E0A8 RID: 57512
				public static LocString TOOLTIP = "This Duplicant has a weakened immune system and will become ill if it reaches zero";

				// Token: 0x0400E0A9 RID: 57513
				public static LocString NOTIFICATION_NAME = "Low Immunity";

				// Token: 0x0400E0AA RID: 57514
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are at risk of becoming sick:";
			}

			// Token: 0x02003631 RID: 13873
			public abstract class TINKERING
			{
				// Token: 0x0400E0AB RID: 57515
				public static LocString NAME = "Tinkering";

				// Token: 0x0400E0AC RID: 57516
				public static LocString TOOLTIP = "This Duplicant is making functional improvements to a building";
			}

			// Token: 0x02003632 RID: 13874
			public class CONTACTWITHGERMS
			{
				// Token: 0x0400E0AD RID: 57517
				public static LocString NAME = "Contact with {Sickness} Germs";

				// Token: 0x0400E0AE RID: 57518
				public static LocString TOOLTIP = "This Duplicant has encountered {Sickness} Germs and is at risk of dangerous exposure if contact continues\n\n<i>" + UI.CLICK(UI.ClickType.Click) + " to jump to last contact location</i>";
			}

			// Token: 0x02003633 RID: 13875
			public class EXPOSEDTOGERMS
			{
				// Token: 0x0400E0AF RID: 57519
				public static LocString TIER1 = "Mild Exposure";

				// Token: 0x0400E0B0 RID: 57520
				public static LocString TIER2 = "Medium Exposure";

				// Token: 0x0400E0B1 RID: 57521
				public static LocString TIER3 = "Exposure";

				// Token: 0x0400E0B2 RID: 57522
				public static readonly LocString[] EXPOSURE_TIERS = new LocString[]
				{
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER1,
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER2,
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER3
				};

				// Token: 0x0400E0B3 RID: 57523
				public static LocString NAME = "{Severity} to {Sickness} Germs";

				// Token: 0x0400E0B4 RID: 57524
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has been exposed to a concentration of {Sickness} Germs and is at risk of waking up sick on their next shift\n\nExposed {Source}\n\nRate of Contracting {Sickness}: {Chance}\n\nResistance Rating: {Total}\n    • Base {Sickness} Resistance: {Base}\n    • ",
					DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.NAME,
					": {Dupe}\n    • {Severity} Exposure: {ExposureLevelBonus}\n\n<i>",
					UI.CLICK(UI.ClickType.Click),
					" to jump to last exposure location</i>"
				});
			}

			// Token: 0x02003634 RID: 13876
			public class GASLIQUIDEXPOSURE
			{
				// Token: 0x0400E0B5 RID: 57525
				public static LocString NAME_MINOR = "Eye Irritation";

				// Token: 0x0400E0B6 RID: 57526
				public static LocString NAME_MAJOR = "Major Eye Irritation";

				// Token: 0x0400E0B7 RID: 57527
				public static LocString TOOLTIP = "Ah, it stings!\n\nThis poor Duplicant got a faceful of an irritating gas or liquid";

				// Token: 0x0400E0B8 RID: 57528
				public static LocString TOOLTIP_EXPOSED = "Current exposure to {element} is {rate} eye irritation";

				// Token: 0x0400E0B9 RID: 57529
				public static LocString TOOLTIP_RATE_INCREASE = "increasing";

				// Token: 0x0400E0BA RID: 57530
				public static LocString TOOLTIP_RATE_DECREASE = "decreasing";

				// Token: 0x0400E0BB RID: 57531
				public static LocString TOOLTIP_RATE_STAYS = "maintaining";

				// Token: 0x0400E0BC RID: 57532
				public static LocString TOOLTIP_EXPOSURE_LEVEL = "Time Remaining: {time}";
			}

			// Token: 0x02003635 RID: 13877
			public class BEINGPRODUCTIVE
			{
				// Token: 0x0400E0BD RID: 57533
				public static LocString NAME = "Super Focused";

				// Token: 0x0400E0BE RID: 57534
				public static LocString TOOLTIP = "This Duplicant is focused on being super productive right now";
			}

			// Token: 0x02003636 RID: 13878
			public class BALLOONARTISTPLANNING
			{
				// Token: 0x0400E0BF RID: 57535
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400E0C0 RID: 57536
				public static LocString TOOLTIP = "This Duplicant is planning to hand out balloons in their downtime";
			}

			// Token: 0x02003637 RID: 13879
			public class BALLOONARTISTHANDINGOUT
			{
				// Token: 0x0400E0C1 RID: 57537
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400E0C2 RID: 57538
				public static LocString TOOLTIP = "This Duplicant is handing out balloons to other Duplicants";
			}

			// Token: 0x02003638 RID: 13880
			public class EXPELLINGRADS
			{
				// Token: 0x0400E0C3 RID: 57539
				public static LocString NAME = "Cleansing Rads";

				// Token: 0x0400E0C4 RID: 57540
				public static LocString TOOLTIP = "This Duplicant is, uh... \"expelling\" absorbed radiation from their system";
			}

			// Token: 0x02003639 RID: 13881
			public class ANALYZINGGENES
			{
				// Token: 0x0400E0C5 RID: 57541
				public static LocString NAME = "Analyzing Plant Genes";

				// Token: 0x0400E0C6 RID: 57542
				public static LocString TOOLTIP = "This Duplicant is peering deep into the genetic code of an odd seed";
			}

			// Token: 0x0200363A RID: 13882
			public class ANALYZINGARTIFACT
			{
				// Token: 0x0400E0C7 RID: 57543
				public static LocString NAME = "Analyzing Artifact";

				// Token: 0x0400E0C8 RID: 57544
				public static LocString TOOLTIP = "This Duplicant is studying an artifact";
			}

			// Token: 0x0200363B RID: 13883
			public class RANCHING
			{
				// Token: 0x0400E0C9 RID: 57545
				public static LocString NAME = "Ranching";

				// Token: 0x0400E0CA RID: 57546
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is tending to a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"'s well-being"
				});
			}

			// Token: 0x0200363C RID: 13884
			public class CARVING
			{
				// Token: 0x0400E0CB RID: 57547
				public static LocString NAME = "Carving {Target}";

				// Token: 0x0400E0CC RID: 57548
				public static LocString TOOLTIP = "This Duplicant is carving away at a <b>{Target}</b>";
			}

			// Token: 0x0200363D RID: 13885
			public class DATARAINERPLANNING
			{
				// Token: 0x0400E0CD RID: 57549
				public static LocString NAME = "Rainmaker";

				// Token: 0x0400E0CE RID: 57550
				public static LocString TOOLTIP = "This Duplicant is planning to dish out microchips in their downtime";
			}

			// Token: 0x0200363E RID: 13886
			public class DATARAINERRAINING
			{
				// Token: 0x0400E0CF RID: 57551
				public static LocString NAME = "Rainmaker";

				// Token: 0x0400E0D0 RID: 57552
				public static LocString TOOLTIP = "This Duplicant is making it \"rain\" microchips";
			}

			// Token: 0x0200363F RID: 13887
			public class ROBODANCERPLANNING
			{
				// Token: 0x0400E0D1 RID: 57553
				public static LocString NAME = "Flash Mobber";

				// Token: 0x0400E0D2 RID: 57554
				public static LocString TOOLTIP = "This Duplicant is planning to show off their dance moves in their downtime";
			}

			// Token: 0x02003640 RID: 13888
			public class ROBODANCERDANCING
			{
				// Token: 0x0400E0D3 RID: 57555
				public static LocString NAME = "Flash Mobber";

				// Token: 0x0400E0D4 RID: 57556
				public static LocString TOOLTIP = "This Duplicant is showing off their dance moves to other Duplicants";
			}

			// Token: 0x02003641 RID: 13889
			public class BIONICCRITICALBATTERY
			{
				// Token: 0x0400E0D5 RID: 57557
				public static LocString NAME = "Critical Power Level";

				// Token: 0x0400E0D6 RID: 57558
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" is dangerously low\n\nThey will become incapacitated unless new ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					" are delivered"
				});

				// Token: 0x0400E0D7 RID: 57559
				public static LocString NOTIFICATION_NAME = "Critical Power Level";

				// Token: 0x0400E0D8 RID: 57560
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants will become incapacitated if they can't find ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003642 RID: 13890
			public class REMOTEWORKER
			{
				// Token: 0x02003D7F RID: 15743
				public class ENTERINGDOCK
				{
					// Token: 0x0400F2BA RID: 62138
					public static LocString NAME = "Docking";

					// Token: 0x0400F2BB RID: 62139
					public static LocString TOOLTIP = "This remote worker is entering its dock";
				}

				// Token: 0x02003D80 RID: 15744
				public class UNREACHABLEDOCK
				{
					// Token: 0x0400F2BC RID: 62140
					public static LocString NAME = "Unreachable Dock";

					// Token: 0x0400F2BD RID: 62141
					public static LocString TOOLTIP = "This remote worker cannot reach its dock";
				}

				// Token: 0x02003D81 RID: 15745
				public class NOHOMEDOCK
				{
					// Token: 0x0400F2BE RID: 62142
					public static LocString NAME = "No Dock";

					// Token: 0x0400F2BF RID: 62143
					public static LocString TOOLTIP = "This remote worker has no home dock and will self-destruct";
				}

				// Token: 0x02003D82 RID: 15746
				public class POWERSTATUS
				{
					// Token: 0x0400F2C0 RID: 62144
					public static LocString NAME = "Power Remaining: {CHARGE} ({RATIO})";

					// Token: 0x0400F2C1 RID: 62145
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker has {CHARGE} remaining power\n\nWhen ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" gets low, the remote worker will return to its dock to recharge"
					});
				}

				// Token: 0x02003D83 RID: 15747
				public class LOWPOWER
				{
					// Token: 0x0400F2C2 RID: 62146
					public static LocString NAME = "Low Power";

					// Token: 0x0400F2C3 RID: 62147
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker has low ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						"\n\nIt will recharge at its dock before accepting new chores"
					});
				}

				// Token: 0x02003D84 RID: 15748
				public class OUTOFPOWER
				{
					// Token: 0x0400F2C4 RID: 62148
					public static LocString NAME = "No Power";

					// Token: 0x0400F2C5 RID: 62149
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker cannot function without ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						"\n\nIt must be returned to its dock"
					});
				}

				// Token: 0x02003D85 RID: 15749
				public class HIGHGUNK
				{
					// Token: 0x0400F2C6 RID: 62150
					public static LocString NAME = "Gunk Buildup";

					// Token: 0x0400F2C7 RID: 62151
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker will return to its dock to remove ",
						UI.PRE_KEYWORD,
						"Gunk",
						UI.PST_KEYWORD,
						" buildup before accepting new chores"
					});
				}

				// Token: 0x02003D86 RID: 15750
				public class FULLGUNK
				{
					// Token: 0x0400F2C8 RID: 62152
					public static LocString NAME = "Gunk Clogged";

					// Token: 0x0400F2C9 RID: 62153
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker cannot function due to excessive ",
						UI.PRE_KEYWORD,
						"Gunk",
						UI.PST_KEYWORD,
						" buildup\n\nIt must be returned to its dock"
					});
				}

				// Token: 0x02003D87 RID: 15751
				public class LOWOIL
				{
					// Token: 0x0400F2CA RID: 62154
					public static LocString NAME = "Low Gear Oil";

					// Token: 0x0400F2CB RID: 62155
					public static LocString TOOLTIP = "This remote worker is low on gear oil\n\nIt will dock to replenish its stores before accepting new chores";
				}

				// Token: 0x02003D88 RID: 15752
				public class OUTOFOIL
				{
					// Token: 0x0400F2CC RID: 62156
					public static LocString NAME = "No Gear Oil";

					// Token: 0x0400F2CD RID: 62157
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker cannot function without ",
						UI.PRE_KEYWORD,
						"Gear Oil",
						UI.PST_KEYWORD,
						"\n\nIt must be returned to its dock"
					});
				}

				// Token: 0x02003D89 RID: 15753
				public class RECHARGING
				{
					// Token: 0x0400F2CE RID: 62158
					public static LocString NAME = "Recharging";

					// Token: 0x0400F2CF RID: 62159
					public static LocString TOOLTIP = "This remote worker is recharging its capacitor";
				}

				// Token: 0x02003D8A RID: 15754
				public class OILING
				{
					// Token: 0x0400F2D0 RID: 62160
					public static LocString NAME = "Refilling Gear Oil";

					// Token: 0x0400F2D1 RID: 62161
					public static LocString TOOLTIP = "This remote worker is lubricating its joints";
				}

				// Token: 0x02003D8B RID: 15755
				public class DRAINING
				{
					// Token: 0x0400F2D2 RID: 62162
					public static LocString NAME = "Draining Gunk";

					// Token: 0x0400F2D3 RID: 62163
					public static LocString TOOLTIP = "This remote worker is unclogging its gears";
				}
			}
		}

		// Token: 0x02002584 RID: 9604
		public class DISEASES
		{
			// Token: 0x0400A988 RID: 43400
			public static LocString CURED_POPUP = "Cured of {0}";

			// Token: 0x0400A989 RID: 43401
			public static LocString INFECTED_POPUP = "Became infected by {0}";

			// Token: 0x0400A98A RID: 43402
			public static LocString ADDED_POPFX = "{0}: {1} Germs";

			// Token: 0x0400A98B RID: 43403
			public static LocString NOTIFICATION_TOOLTIP = "{0} contracted {1} from: {2}";

			// Token: 0x0400A98C RID: 43404
			public static LocString GERMS = "Germs";

			// Token: 0x0400A98D RID: 43405
			public static LocString GERMS_CONSUMED_DESCRIPTION = "A count of the number of germs this Duplicant is host to";

			// Token: 0x0400A98E RID: 43406
			public static LocString RECUPERATING = "Recuperating";

			// Token: 0x0400A98F RID: 43407
			public static LocString INFECTION_MODIFIER = "Recently consumed {0} ({1})";

			// Token: 0x0400A990 RID: 43408
			public static LocString INFECTION_MODIFIER_SOURCE = "Fighting off {0} from {1}";

			// Token: 0x0400A991 RID: 43409
			public static LocString INFECTED_MODIFIER = "Suppressed immune system";

			// Token: 0x0400A992 RID: 43410
			public static LocString LEGEND_POSTAMBLE = "\n•  Select an infected object for more details";

			// Token: 0x0400A993 RID: 43411
			public static LocString ATTRIBUTE_BY_MODEL_MODIFIER_SYMPTOMS = "({0}) {1}: {2}";

			// Token: 0x0400A994 RID: 43412
			public static LocString ATTRIBUTE_MODIFIER_SYMPTOMS = "{0}: {1}";

			// Token: 0x0400A995 RID: 43413
			public static LocString ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP = "Modifies {0} by {1}";

			// Token: 0x0400A996 RID: 43414
			public static LocString DEATH_SYMPTOM = "Death in {0} if untreated";

			// Token: 0x0400A997 RID: 43415
			public static LocString DEATH_SYMPTOM_TOOLTIP = "Without medical treatment, this Duplicant will die of their illness in {0}";

			// Token: 0x0400A998 RID: 43416
			public static LocString RESISTANCES_PANEL_TOOLTIP = "{0}";

			// Token: 0x0400A999 RID: 43417
			public static LocString IMMUNE_FROM_MISSING_REQUIRED_TRAIT = "Immune: Does not have {0}";

			// Token: 0x0400A99A RID: 43418
			public static LocString IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT = "Immune: Has {0}";

			// Token: 0x0400A99B RID: 43419
			public static LocString IMMUNE_FROM_HAVING_EXCLUDED_EFFECT = "Immunity: Has {0}";

			// Token: 0x0400A99C RID: 43420
			public static LocString CONTRACTION_PROBABILITY = "{0} of {1}'s exposures to these germs will result in {2}";

			// Token: 0x02003643 RID: 13891
			public class STATUS_ITEM_TOOLTIP
			{
				// Token: 0x0400E0D9 RID: 57561
				public static LocString TEMPLATE = "{InfectionSource}{Duration}{Doctor}{Fatality}{Cures}{Bedrest}\n\n\n{Symptoms}";

				// Token: 0x0400E0DA RID: 57562
				public static LocString DESCRIPTOR = "<b>{0} {1}</b>\n";

				// Token: 0x0400E0DB RID: 57563
				public static LocString SYMPTOMS = "{0}\n";

				// Token: 0x0400E0DC RID: 57564
				public static LocString INFECTION_SOURCE = "Contracted by: {0}\n";

				// Token: 0x0400E0DD RID: 57565
				public static LocString DURATION = "Time to recovery: {0}\n";

				// Token: 0x0400E0DE RID: 57566
				public static LocString CURES = "Remedies taken: {0}\n";

				// Token: 0x0400E0DF RID: 57567
				public static LocString NOMEDICINETAKEN = "Remedies taken: None\n";

				// Token: 0x0400E0E0 RID: 57568
				public static LocString FATALITY = "Fatal if untreated in: {0}\n";

				// Token: 0x0400E0E1 RID: 57569
				public static LocString BEDREST = "Sick Bay assignment will allow faster recovery\n";

				// Token: 0x0400E0E2 RID: 57570
				public static LocString DOCTOR_REQUIRED = "Sick Bay assignment required for recovery\n";

				// Token: 0x0400E0E3 RID: 57571
				public static LocString DOCTORED = "Received medical treatment, recovery speed is increased\n";
			}

			// Token: 0x02003644 RID: 13892
			public class MEDICINE
			{
				// Token: 0x0400E0E4 RID: 57572
				public static LocString SELF_ADMINISTERED_BOOSTER = "Self-Administered: Anytime";

				// Token: 0x0400E0E5 RID: 57573
				public static LocString SELF_ADMINISTERED_BOOSTER_TOOLTIP = "Duplicants can give themselves this medicine, whether they are currently sick or not";

				// Token: 0x0400E0E6 RID: 57574
				public static LocString SELF_ADMINISTERED_CURE = "Self-Administered: Sick Only";

				// Token: 0x0400E0E7 RID: 57575
				public static LocString SELF_ADMINISTERED_CURE_TOOLTIP = "Duplicants can give themselves this medicine, but only while they are sick";

				// Token: 0x0400E0E8 RID: 57576
				public static LocString DOCTOR_ADMINISTERED_BOOSTER = "Doctor Administered: Anytime";

				// Token: 0x0400E0E9 RID: 57577
				public static LocString DOCTOR_ADMINISTERED_BOOSTER_TOOLTIP = "Duplicants can receive this medicine at a {Station}, whether they are currently sick or not\n\nThey cannot give it to themselves and must receive it from a friend with " + UI.PRE_KEYWORD + "Doctoring Skills" + UI.PST_KEYWORD;

				// Token: 0x0400E0EA RID: 57578
				public static LocString DOCTOR_ADMINISTERED_CURE = "Doctor Administered: Sick Only";

				// Token: 0x0400E0EB RID: 57579
				public static LocString DOCTOR_ADMINISTERED_CURE_TOOLTIP = "Duplicants can receive this medicine at a {Station}, but only while they are sick\n\nThey cannot give it to themselves and must receive it from a friend with " + UI.PRE_KEYWORD + "Doctoring Skills" + UI.PST_KEYWORD;

				// Token: 0x0400E0EC RID: 57580
				public static LocString BOOSTER = UI.FormatAsLink("Immune Booster", "IMMUNE SYSTEM");

				// Token: 0x0400E0ED RID: 57581
				public static LocString BOOSTER_TOOLTIP = "Boosters can be taken by both healthy and sick Duplicants to prevent potential disease";

				// Token: 0x0400E0EE RID: 57582
				public static LocString CURES_ANY = "Alleviates " + UI.FormatAsLink("All Diseases", "DISEASE");

				// Token: 0x0400E0EF RID: 57583
				public static LocString CURES_ANY_TOOLTIP = string.Concat(new string[]
				{
					"This is a nonspecific ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" treatment that can be taken by any sick Duplicant"
				});

				// Token: 0x0400E0F0 RID: 57584
				public static LocString CURES = "Alleviates {0}";

				// Token: 0x0400E0F1 RID: 57585
				public static LocString CURES_TOOLTIP = "This medicine is used to treat {0} and can only be taken by sick Duplicants";
			}

			// Token: 0x02003645 RID: 13893
			public class SEVERITY
			{
				// Token: 0x0400E0F2 RID: 57586
				public static LocString BENIGN = "Benign";

				// Token: 0x0400E0F3 RID: 57587
				public static LocString MINOR = "Minor";

				// Token: 0x0400E0F4 RID: 57588
				public static LocString MAJOR = "Major";

				// Token: 0x0400E0F5 RID: 57589
				public static LocString CRITICAL = "Critical";
			}

			// Token: 0x02003646 RID: 13894
			public class TYPE
			{
				// Token: 0x0400E0F6 RID: 57590
				public static LocString PATHOGEN = "Illness";

				// Token: 0x0400E0F7 RID: 57591
				public static LocString AILMENT = "Ailment";

				// Token: 0x0400E0F8 RID: 57592
				public static LocString INJURY = "Injury";
			}

			// Token: 0x02003647 RID: 13895
			public class TRIGGERS
			{
				// Token: 0x0400E0F9 RID: 57593
				public static LocString EATCOMPLETEEDIBLE = "May cause {Diseases}";

				// Token: 0x02003D8C RID: 15756
				public class TOOLTIPS
				{
					// Token: 0x0400F2D4 RID: 62164
					public static LocString EATCOMPLETEEDIBLE = "May cause {Diseases}";
				}
			}

			// Token: 0x02003648 RID: 13896
			public class INFECTIONSOURCES
			{
				// Token: 0x0400E0FA RID: 57594
				public static LocString INTERNAL_TEMPERATURE = "Extreme internal temperatures";

				// Token: 0x0400E0FB RID: 57595
				public static LocString TOXIC_AREA = "Exposure to toxic areas";

				// Token: 0x0400E0FC RID: 57596
				public static LocString FOOD = "Eating a germ-covered {0}";

				// Token: 0x0400E0FD RID: 57597
				public static LocString AIR = "Breathing germ-filled {0}";

				// Token: 0x0400E0FE RID: 57598
				public static LocString SKIN = "Skin contamination";

				// Token: 0x0400E0FF RID: 57599
				public static LocString UNKNOWN = "Unknown source";
			}

			// Token: 0x02003649 RID: 13897
			public class DESCRIPTORS
			{
				// Token: 0x02003D8D RID: 15757
				public class INFO
				{
					// Token: 0x0400F2D5 RID: 62165
					public static LocString FOODBORNE = "Contracted via ingestion\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400F2D6 RID: 62166
					public static LocString FOODBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by ingesting ",
						UI.PRE_KEYWORD,
						"Food",
						UI.PST_KEYWORD,
						" contaminated with these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400F2D7 RID: 62167
					public static LocString AIRBORNE = "Contracted via inhalation\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400F2D8 RID: 62168
					public static LocString AIRBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by breathing ",
						ELEMENTS.OXYGEN.NAME,
						" containing these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400F2D9 RID: 62169
					public static LocString SKINBORNE = "Contracted via physical contact\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400F2DA RID: 62170
					public static LocString SKINBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by touching objects contaminated with these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400F2DB RID: 62171
					public static LocString SUNBORNE = "Contracted via environmental exposure\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400F2DC RID: 62172
					public static LocString SUNBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" through exposure to hazardous environments"
					});

					// Token: 0x0400F2DD RID: 62173
					public static LocString GROWS_ON = "Multiplies in:";

					// Token: 0x0400F2DE RID: 62174
					public static LocString GROWS_ON_TOOLTIP = string.Concat(new string[]
					{
						"These substances allow ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" to spread and reproduce"
					});

					// Token: 0x0400F2DF RID: 62175
					public static LocString NEUTRAL_ON = "Survives in:";

					// Token: 0x0400F2E0 RID: 62176
					public static LocString NEUTRAL_ON_TOOLTIP = UI.PRE_KEYWORD + "Germs" + UI.PST_KEYWORD + " will survive contact with these substances, but will not reproduce";

					// Token: 0x0400F2E1 RID: 62177
					public static LocString DIES_SLOWLY_ON = "Inhibited by:";

					// Token: 0x0400F2E2 RID: 62178
					public static LocString DIES_SLOWLY_ON_TOOLTIP = string.Concat(new string[]
					{
						"Contact with these substances will slowly reduce ",
						UI.PRE_KEYWORD,
						"Germ",
						UI.PST_KEYWORD,
						" numbers"
					});

					// Token: 0x0400F2E3 RID: 62179
					public static LocString DIES_ON = "Killed by:";

					// Token: 0x0400F2E4 RID: 62180
					public static LocString DIES_ON_TOOLTIP = string.Concat(new string[]
					{
						"Contact with these substances kills ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" over time"
					});

					// Token: 0x0400F2E5 RID: 62181
					public static LocString DIES_QUICKLY_ON = "Disinfected by:";

					// Token: 0x0400F2E6 RID: 62182
					public static LocString DIES_QUICKLY_ON_TOOLTIP = "Contact with these substances will quickly kill these " + UI.PRE_KEYWORD + "Germs" + UI.PST_KEYWORD;

					// Token: 0x0400F2E7 RID: 62183
					public static LocString GROWS = "Multiplies";

					// Token: 0x0400F2E8 RID: 62184
					public static LocString GROWS_TOOLTIP = "Doubles germ count every {0}";

					// Token: 0x0400F2E9 RID: 62185
					public static LocString NEUTRAL = "Survives";

					// Token: 0x0400F2EA RID: 62186
					public static LocString NEUTRAL_TOOLTIP = "Germ count remains static";

					// Token: 0x0400F2EB RID: 62187
					public static LocString DIES_SLOWLY = "Inhibited";

					// Token: 0x0400F2EC RID: 62188
					public static LocString DIES_SLOWLY_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400F2ED RID: 62189
					public static LocString DIES = "Dies";

					// Token: 0x0400F2EE RID: 62190
					public static LocString DIES_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400F2EF RID: 62191
					public static LocString DIES_QUICKLY = "Disinfected";

					// Token: 0x0400F2F0 RID: 62192
					public static LocString DIES_QUICKLY_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400F2F1 RID: 62193
					public static LocString GROWTH_FORMAT = "    • {0}";

					// Token: 0x0400F2F2 RID: 62194
					public static LocString TEMPERATURE_RANGE = "Temperature range: {0} to {1}";

					// Token: 0x0400F2F3 RID: 62195
					public static LocString TEMPERATURE_RANGE_TOOLTIP = string.Concat(new string[]
					{
						"These ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" can survive ",
						UI.PRE_KEYWORD,
						"Temperatures",
						UI.PST_KEYWORD,
						" between <b>{0}</b> and <b>{1}</b>\n\nThey thrive in ",
						UI.PRE_KEYWORD,
						"Temperatures",
						UI.PST_KEYWORD,
						" between <b>{2}</b> and <b>{3}</b>"
					});

					// Token: 0x0400F2F4 RID: 62196
					public static LocString PRESSURE_RANGE = "Pressure range: {0} to {1}\n";

					// Token: 0x0400F2F5 RID: 62197
					public static LocString PRESSURE_RANGE_TOOLTIP = string.Concat(new string[]
					{
						"These ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" can survive between <b>{0}</b> and <b>{1}</b> of pressure\n\nThey thrive in pressures between <b>{2}</b> and <b>{3}</b>"
					});
				}
			}

			// Token: 0x0200364A RID: 13898
			public class ALLDISEASES
			{
				// Token: 0x0400E100 RID: 57600
				public static LocString NAME = "All Diseases";
			}

			// Token: 0x0200364B RID: 13899
			public class NODISEASES
			{
				// Token: 0x0400E101 RID: 57601
				public static LocString NAME = "NO";
			}

			// Token: 0x0200364C RID: 13900
			public class FOODPOISONING
			{
				// Token: 0x0400E102 RID: 57602
				public static LocString NAME = UI.FormatAsLink("Food Poisoning", "FOODPOISONING");

				// Token: 0x0400E103 RID: 57603
				public static LocString LEGEND_HOVERTEXT = "Food Poisoning Germs present\n";

				// Token: 0x0400E104 RID: 57604
				public static LocString DESC = "Food and drinks tainted with Food Poisoning germs are unsafe to consume, as they cause vomiting and other...bodily unpleasantness.";
			}

			// Token: 0x0200364D RID: 13901
			public class SLIMELUNG
			{
				// Token: 0x0400E105 RID: 57605
				public static LocString NAME = UI.FormatAsLink("Slimelung", "SLIMELUNG");

				// Token: 0x0400E106 RID: 57606
				public static LocString LEGEND_HOVERTEXT = "Slimelung Germs present\n";

				// Token: 0x0400E107 RID: 57607
				public static LocString DESC = string.Concat(new string[]
				{
					"Slimelung germs are found in ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" and ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					". Inhaling these germs can cause Duplicants to cough and struggle to breathe."
				});
			}

			// Token: 0x0200364E RID: 13902
			public class POLLENGERMS
			{
				// Token: 0x0400E108 RID: 57608
				public static LocString NAME = UI.FormatAsLink("Floral Scent", "POLLENGERMS");

				// Token: 0x0400E109 RID: 57609
				public static LocString LEGEND_HOVERTEXT = "Floral Scent allergens present\n";

				// Token: 0x0400E10A RID: 57610
				public static LocString DESC = "Floral Scent allergens trigger excessive sneezing fits in Duplicants who possess the Allergies trait.";
			}

			// Token: 0x0200364F RID: 13903
			public class ZOMBIESPORES
			{
				// Token: 0x0400E10B RID: 57611
				public static LocString NAME = UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES");

				// Token: 0x0400E10C RID: 57612
				public static LocString LEGEND_HOVERTEXT = "Zombie Spores present\n";

				// Token: 0x0400E10D RID: 57613
				public static LocString DESC = "Zombie Spores are a parasitic brain fungus released by " + UI.FormatAsLink("Sporechids", "EVIL_FLOWER") + ". Duplicants who touch or inhale the spores risk becoming infected and temporarily losing motor control.";
			}

			// Token: 0x02003650 RID: 13904
			public class RADIATIONPOISONING
			{
				// Token: 0x0400E10E RID: 57614
				public static LocString NAME = UI.FormatAsLink("Radioactive Contamination", "RADIATIONPOISONING");

				// Token: 0x0400E10F RID: 57615
				public static LocString LEGEND_HOVERTEXT = "Radioactive contamination present\n";

				// Token: 0x0400E110 RID: 57616
				public static LocString DESC = string.Concat(new string[]
				{
					"Items tainted with Radioactive Contaminants emit low levels of ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" that can cause ",
					UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
					". They are unaffected by pressure or temperature, but do degrade over time."
				});
			}

			// Token: 0x02003651 RID: 13905
			public class FOODSICKNESS
			{
				// Token: 0x0400E111 RID: 57617
				public static LocString NAME = UI.FormatAsLink("Food Poisoning", "FOODSICKNESS");

				// Token: 0x0400E112 RID: 57618
				public static LocString DESCRIPTION = "This Duplicant's last meal wasn't exactly food safe";

				// Token: 0x0400E113 RID: 57619
				public static LocString VOMIT_SYMPTOM = "Vomiting";

				// Token: 0x0400E114 RID: 57620
				public static LocString VOMIT_SYMPTOM_TOOLTIP = string.Concat(new string[]
				{
					"Duplicants periodically vomit throughout the day, producing additional ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" and losing ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD
				});

				// Token: 0x0400E115 RID: 57621
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. A Duplicant's body \"purges\" from both ends, causing extreme fatigue.";

				// Token: 0x0400E116 RID: 57622
				public static LocString DISEASE_SOURCE_DESCRIPTOR = "Currently infected with {2}.\n\nThis Duplicant will produce {1} when vomiting.";

				// Token: 0x0400E117 RID: 57623
				public static LocString DISEASE_SOURCE_DESCRIPTOR_TOOLTIP = "This Duplicant will vomit approximately every <b>{0}</b>\n\nEach time they vomit, they will release <b>{1}</b> and lose " + UI.PRE_KEYWORD + "Calories" + UI.PST_KEYWORD;
			}

			// Token: 0x02003652 RID: 13906
			public class SLIMESICKNESS
			{
				// Token: 0x0400E118 RID: 57624
				public static LocString NAME = UI.FormatAsLink("Slimelung", "SLIMESICKNESS");

				// Token: 0x0400E119 RID: 57625
				public static LocString DESCRIPTION = "This Duplicant's chest congestion is making it difficult to breathe";

				// Token: 0x0400E11A RID: 57626
				public static LocString COUGH_SYMPTOM = "Coughing";

				// Token: 0x0400E11B RID: 57627
				public static LocString COUGH_SYMPTOM_TOOLTIP = string.Concat(new string[]
				{
					"Duplicants periodically cough up ",
					ELEMENTS.CONTAMINATEDOXYGEN.NAME,
					", producing additional ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD
				});

				// Token: 0x0400E11C RID: 57628
				public static LocString DESCRIPTIVE_SYMPTOMS = "Lethal without medical treatment. Duplicants experience coughing and shortness of breath.";

				// Token: 0x0400E11D RID: 57629
				public static LocString DISEASE_SOURCE_DESCRIPTOR = "Currently infected with {2}.\n\nThis Duplicant will produce <b>{1}</b> when coughing.";

				// Token: 0x0400E11E RID: 57630
				public static LocString DISEASE_SOURCE_DESCRIPTOR_TOOLTIP = "This Duplicant will cough approximately every <b>{0}</b>\n\nEach time they cough, they will release <b>{1}</b>";
			}

			// Token: 0x02003653 RID: 13907
			public class ZOMBIESICKNESS
			{
				// Token: 0x0400E11F RID: 57631
				public static LocString NAME = UI.FormatAsLink("Zombie Spores", "ZOMBIESICKNESS");

				// Token: 0x0400E120 RID: 57632
				public static LocString DESCRIPTIVE_SYMPTOMS = "Duplicants lose much of their motor control and experience extreme discomfort.";

				// Token: 0x0400E121 RID: 57633
				public static LocString DESCRIPTION = "Fungal spores have infiltrated the Duplicant's head and are sending unnatural electrical impulses to their brain";

				// Token: 0x0400E122 RID: 57634
				public static LocString LEGEND_HOVERTEXT = "Area Causes Zombie Spores\n";
			}

			// Token: 0x02003654 RID: 13908
			public class ALLERGIES
			{
				// Token: 0x0400E123 RID: 57635
				public static LocString NAME = UI.FormatAsLink("Allergic Reaction", "ALLERGIES");

				// Token: 0x0400E124 RID: 57636
				public static LocString DESCRIPTIVE_SYMPTOMS = "Allergens cause excessive sneezing fits";

				// Token: 0x0400E125 RID: 57637
				public static LocString DESCRIPTION = "Pollen and other irritants are causing this poor Duplicant's immune system to overreact, resulting in needless sneezing and congestion";
			}

			// Token: 0x02003655 RID: 13909
			public class SUNBURNSICKNESS
			{
				// Token: 0x0400E126 RID: 57638
				public static LocString NAME = UI.FormatAsLink("Sunburn", "SUNBURNSICKNESS");

				// Token: 0x0400E127 RID: 57639
				public static LocString DESCRIPTION = "Extreme sun exposure has given this Duplicant a nasty burn.";

				// Token: 0x0400E128 RID: 57640
				public static LocString LEGEND_HOVERTEXT = "Area Causes Sunburn\n";

				// Token: 0x0400E129 RID: 57641
				public static LocString SUNEXPOSURE = "Sun Exposure";

				// Token: 0x0400E12A RID: 57642
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. Duplicants experience temporary discomfort due to dermatological damage.";
			}

			// Token: 0x02003656 RID: 13910
			public class RADIATIONSICKNESS
			{
				// Token: 0x0400E12B RID: 57643
				public static LocString NAME = UI.FormatAsLink("Radioactive Contaminants", "RADIATIONSICKNESS");

				// Token: 0x0400E12C RID: 57644
				public static LocString DESCRIPTIVE_SYMPTOMS = "Extremely lethal. This Duplicant is not expected to survive.";

				// Token: 0x0400E12D RID: 57645
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"This Duplicant is leaving a trail of ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" behind them."
				});

				// Token: 0x0400E12E RID: 57646
				public static LocString LEGEND_HOVERTEXT = "Area Causes Radiation Sickness\n";

				// Token: 0x0400E12F RID: 57647
				public static LocString DESC = DUPLICANTS.DISEASES.RADIATIONPOISONING.DESC;
			}

			// Token: 0x02003657 RID: 13911
			public class PUTRIDODOUR
			{
				// Token: 0x0400E130 RID: 57648
				public static LocString NAME = UI.FormatAsLink("Trench Stench", "PUTRIDODOUR");

				// Token: 0x0400E131 RID: 57649
				public static LocString DESCRIPTION = "\nThe pungent odor wafting off this Duplicant is nauseating to their peers";

				// Token: 0x0400E132 RID: 57650
				public static LocString CRINGE_EFFECT = "Smelled a putrid odor";

				// Token: 0x0400E133 RID: 57651
				public static LocString LEGEND_HOVERTEXT = "Trench Stench Germs Present\n";
			}
		}

		// Token: 0x02002585 RID: 9605
		public class MODIFIERS
		{
			// Token: 0x0400A99D RID: 43421
			public static LocString MODIFIER_FORMAT = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + ": {1}";

			// Token: 0x0400A99E RID: 43422
			public static LocString IMMUNITY_FORMAT = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

			// Token: 0x0400A99F RID: 43423
			public static LocString TIME_REMAINING = "Time Remaining: {0}";

			// Token: 0x0400A9A0 RID: 43424
			public static LocString TIME_TOTAL = "\nDuration: {0}";

			// Token: 0x0400A9A1 RID: 43425
			public static LocString EFFECT_IMMUNITIES_HEADER = UI.PRE_POS_MODIFIER + "Immune to:" + UI.PST_POS_MODIFIER;

			// Token: 0x0400A9A2 RID: 43426
			public static LocString EFFECT_HEADER = UI.PRE_POS_MODIFIER + "Effects:" + UI.PST_POS_MODIFIER;

			// Token: 0x02003658 RID: 13912
			public class BREAK_BONUS
			{
				// Token: 0x0400E134 RID: 57652
				public static LocString NAME = "Downtime Bonus";

				// Token: 0x0400E135 RID: 57653
				public static LocString MAX_NAME = "Max Downtime Bonus";
			}

			// Token: 0x02003659 RID: 13913
			public class SKILLLEVEL
			{
				// Token: 0x0400E136 RID: 57654
				public static LocString NAME = "Attribute Level";
			}

			// Token: 0x0200365A RID: 13914
			public class ROOMPARK
			{
				// Token: 0x0400E137 RID: 57655
				public static LocString NAME = "Park";

				// Token: 0x0400E138 RID: 57656
				public static LocString TOOLTIP = "This Duplicant recently passed through a Park\n\nWow, nature sure is neat!";
			}

			// Token: 0x0200365B RID: 13915
			public class ROOMNATURERESERVE
			{
				// Token: 0x0400E139 RID: 57657
				public static LocString NAME = "Nature Reserve";

				// Token: 0x0400E13A RID: 57658
				public static LocString TOOLTIP = "This Duplicant recently passed through a splendid Nature Reserve\n\nWow, nature sure is neat!";
			}

			// Token: 0x0200365C RID: 13916
			public class ROOMLATRINE
			{
				// Token: 0x0400E13B RID: 57659
				public static LocString NAME = "Latrine";

				// Token: 0x0400E13C RID: 57660
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used an ",
					BUILDINGS.PREFABS.OUTHOUSE.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Latrine",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200365D RID: 13917
			public class ROOMBATHROOM
			{
				// Token: 0x0400E13D RID: 57661
				public static LocString NAME = "Washroom";

				// Token: 0x0400E13E RID: 57662
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used a ",
					BUILDINGS.PREFABS.FLUSHTOILET.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Washroom",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200365E RID: 13918
			public class ROOMBIONICUPKEEP
			{
				// Token: 0x0400E13F RID: 57663
				public static LocString NAME = "";

				// Token: 0x0400E140 RID: 57664
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200365F RID: 13919
			public class FRESHOIL
			{
				// Token: 0x0400E141 RID: 57665
				public static LocString NAME = "Fresh Oil";

				// Token: 0x0400E142 RID: 57666
				public static LocString TOOLTIP = "This Duplicant recently used a " + BUILDINGS.PREFABS.OILCHANGER.NAME + " and feels pretty slick";
			}

			// Token: 0x02003660 RID: 13920
			public class ROOMBARRACKS
			{
				// Token: 0x0400E143 RID: 57667
				public static LocString NAME = "Barracks";

				// Token: 0x0400E144 RID: 57668
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in the ",
					UI.PRE_KEYWORD,
					"Barracks",
					UI.PST_KEYWORD,
					" last night and feels refreshed"
				});
			}

			// Token: 0x02003661 RID: 13921
			public class ROOMBEDROOM
			{
				// Token: 0x0400E145 RID: 57669
				public static LocString NAME = "Luxury Barracks";

				// Token: 0x0400E146 RID: 57670
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in a ",
					UI.PRE_KEYWORD,
					"Luxury Barracks",
					UI.PST_KEYWORD,
					" last night and feels extra refreshed"
				});
			}

			// Token: 0x02003662 RID: 13922
			public class ROOMPRIVATEBEDROOM
			{
				// Token: 0x0400E147 RID: 57671
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400E148 RID: 57672
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in a ",
					UI.PRE_KEYWORD,
					"Private Bedroom",
					UI.PST_KEYWORD,
					" last night and feels super refreshed"
				});
			}

			// Token: 0x02003663 RID: 13923
			public class BEDHEALTH
			{
				// Token: 0x0400E149 RID: 57673
				public static LocString NAME = "Bed Rest";

				// Token: 0x0400E14A RID: 57674
				public static LocString TOOLTIP = "This Duplicant will incrementally heal over while on " + UI.PRE_KEYWORD + "Bed Rest" + UI.PST_KEYWORD;
			}

			// Token: 0x02003664 RID: 13924
			public class BEDSTAMINA
			{
				// Token: 0x0400E14B RID: 57675
				public static LocString NAME = "Sleeping in a cot";

				// Token: 0x0400E14C RID: 57676
				public static LocString TOOLTIP = "This Duplicant's sleeping arrangements are adequate";
			}

			// Token: 0x02003665 RID: 13925
			public class LUXURYBEDSTAMINA
			{
				// Token: 0x0400E14D RID: 57677
				public static LocString NAME = "Sleeping in a comfy bed";

				// Token: 0x0400E14E RID: 57678
				public static LocString TOOLTIP = "This Duplicant loves their snuggly bed";
			}

			// Token: 0x02003666 RID: 13926
			public class BARRACKSSTAMINA
			{
				// Token: 0x0400E14F RID: 57679
				public static LocString NAME = "Barracks";

				// Token: 0x0400E150 RID: 57680
				public static LocString TOOLTIP = "This Duplicant shares sleeping quarters with others";
			}

			// Token: 0x02003667 RID: 13927
			public class LADDERBEDSTAMINA
			{
				// Token: 0x0400E151 RID: 57681
				public static LocString NAME = "Sleeping in a ladder bed";

				// Token: 0x0400E152 RID: 57682
				public static LocString TOOLTIP = "This Duplicant's sleeping arrangements are adequate";
			}

			// Token: 0x02003668 RID: 13928
			public class BEDROOMSTAMINA
			{
				// Token: 0x0400E153 RID: 57683
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400E154 RID: 57684
				public static LocString TOOLTIP = "This lucky Duplicant has their own private bedroom";
			}

			// Token: 0x02003669 RID: 13929
			public class ROOMMESSHALL
			{
				// Token: 0x0400E155 RID: 57685
				public static LocString NAME = "Mess Hall";

				// Token: 0x0400E156 RID: 57686
				public static LocString TOOLTIP = "This Duplicant's most recent meal was eaten in a " + UI.PRE_KEYWORD + "Mess Hall" + UI.PST_KEYWORD;
			}

			// Token: 0x0200366A RID: 13930
			public class ROOMGREATHALL
			{
				// Token: 0x0400E157 RID: 57687
				public static LocString NAME = "Great Hall";

				// Token: 0x0400E158 RID: 57688
				public static LocString TOOLTIP = "This Duplicant's most recent meal was eaten in a fancy " + UI.PRE_KEYWORD + "Great Hall" + UI.PST_KEYWORD;
			}

			// Token: 0x0200366B RID: 13931
			public class ROOMBANQUETHALL
			{
				// Token: 0x0400E159 RID: 57689
				public static LocString NAME = "Banquet Hall";

				// Token: 0x0400E15A RID: 57690
				public static LocString TOOLTIP = "This Duplicant's most recent meal was eaten in a bustling " + UI.PRE_KEYWORD + "Banquet Hall" + UI.PST_KEYWORD;
			}

			// Token: 0x0200366C RID: 13932
			public class ENTITLEMENT
			{
				// Token: 0x0400E15B RID: 57691
				public static LocString NAME = "Entitlement";

				// Token: 0x0400E15C RID: 57692
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will demand better ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" and accommodations with each Expertise level they gain"
				});
			}

			// Token: 0x0200366D RID: 13933
			public class HOMEOSTASIS
			{
				// Token: 0x0400E15D RID: 57693
				public static LocString NAME = "Homeostasis";
			}

			// Token: 0x0200366E RID: 13934
			public class WARMAIR
			{
				// Token: 0x0400E15E RID: 57694
				public static LocString NAME = "Toasty Surroundings";
			}

			// Token: 0x0200366F RID: 13935
			public class COLDAIR
			{
				// Token: 0x0400E15F RID: 57695
				public static LocString NAME = "Chilly Surroundings";

				// Token: 0x0400E160 RID: 57696
				public static LocString CAUSE = "Duplicants tire quickly and lose body heat in cold environments";
			}

			// Token: 0x02003670 RID: 13936
			public class CLAUSTROPHOBIC
			{
				// Token: 0x0400E161 RID: 57697
				public static LocString NAME = "Claustrophobic";

				// Token: 0x0400E162 RID: 57698
				public static LocString TOOLTIP = "This Duplicant recently found themselves in an upsettingly cramped space";

				// Token: 0x0400E163 RID: 57699
				public static LocString CAUSE = "This Duplicant got so good at their job that they became claustrophobic";
			}

			// Token: 0x02003671 RID: 13937
			public class VERTIGO
			{
				// Token: 0x0400E164 RID: 57700
				public static LocString NAME = "Vertigo";

				// Token: 0x0400E165 RID: 57701
				public static LocString TOOLTIP = "This Duplicant had to climb a tall ladder that left them dizzy and unsettled";

				// Token: 0x0400E166 RID: 57702
				public static LocString CAUSE = "This Duplicant got so good at their job they became bad at ladders";
			}

			// Token: 0x02003672 RID: 13938
			public class UNCOMFORTABLEFEET
			{
				// Token: 0x0400E167 RID: 57703
				public static LocString NAME = "Aching Feet";

				// Token: 0x0400E168 RID: 57704
				public static LocString TOOLTIP = "This Duplicant recently walked across floor without tile, much to their chagrin";

				// Token: 0x0400E169 RID: 57705
				public static LocString CAUSE = "This Duplicant got so good at their job that their feet became sensitive";
			}

			// Token: 0x02003673 RID: 13939
			public class PEOPLETOOCLOSEWHILESLEEPING
			{
				// Token: 0x0400E16A RID: 57706
				public static LocString NAME = "Personal Bubble Burst";

				// Token: 0x0400E16B RID: 57707
				public static LocString TOOLTIP = "This Duplicant had to sleep too close to others and it was awkward for them";

				// Token: 0x0400E16C RID: 57708
				public static LocString CAUSE = "This Duplicant got so good at their job that they stopped being comfortable sleeping near other people";
			}

			// Token: 0x02003674 RID: 13940
			public class RESTLESS
			{
				// Token: 0x0400E16D RID: 57709
				public static LocString NAME = "Restless";

				// Token: 0x0400E16E RID: 57710
				public static LocString TOOLTIP = "This Duplicant went a few minutes without working and is now completely awash with guilt";

				// Token: 0x0400E16F RID: 57711
				public static LocString CAUSE = "This Duplicant got so good at their job that they forgot how to be comfortable doing anything else";
			}

			// Token: 0x02003675 RID: 13941
			public class UNFASHIONABLECLOTHING
			{
				// Token: 0x0400E170 RID: 57712
				public static LocString NAME = "Fashion Crime";

				// Token: 0x0400E171 RID: 57713
				public static LocString TOOLTIP = "This Duplicant had to wear something that was an affront to fashion";

				// Token: 0x0400E172 RID: 57714
				public static LocString CAUSE = "This Duplicant got so good at their job that they became incapable of tolerating unfashionable clothing";
			}

			// Token: 0x02003676 RID: 13942
			public class BURNINGCALORIES
			{
				// Token: 0x0400E173 RID: 57715
				public static LocString NAME = "Homeostasis";
			}

			// Token: 0x02003677 RID: 13943
			public class EATINGCALORIES
			{
				// Token: 0x0400E174 RID: 57716
				public static LocString NAME = "Eating";
			}

			// Token: 0x02003678 RID: 13944
			public class TEMPEXCHANGE
			{
				// Token: 0x0400E175 RID: 57717
				public static LocString NAME = "Environmental Exchange";
			}

			// Token: 0x02003679 RID: 13945
			public class CLOTHING
			{
				// Token: 0x0400E176 RID: 57718
				public static LocString NAME = "Clothing";
			}

			// Token: 0x0200367A RID: 13946
			public class CRYFACE
			{
				// Token: 0x0400E177 RID: 57719
				public static LocString NAME = "Cry Face";

				// Token: 0x0400E178 RID: 57720
				public static LocString TOOLTIP = "This Duplicant recently had a crying fit and it shows";

				// Token: 0x0400E179 RID: 57721
				public static LocString CAUSE = string.Concat(new string[]
				{
					"Obtained from the ",
					UI.PRE_KEYWORD,
					"Ugly Crier",
					UI.PST_KEYWORD,
					" stress reaction"
				});
			}

			// Token: 0x0200367B RID: 13947
			public class WARMTOUCH
			{
				// Token: 0x0400E17A RID: 57722
				public static LocString NAME = "Frost Resistant";

				// Token: 0x0400E17B RID: 57723
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently visited a warming station, sauna, or hot tub\n\nThey are impervious to ",
					UI.PRE_KEYWORD,
					"Chilly Surroundings",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Soggy Feet",
					UI.PST_KEYWORD,
					" as a result"
				});

				// Token: 0x0400E17C RID: 57724
				public static LocString PROVIDERS_NAME = "Frost Resistance";

				// Token: 0x0400E17D RID: 57725
				public static LocString PROVIDERS_TOOLTIP = string.Concat(new string[]
				{
					"Using this building provides temporary immunity to ",
					UI.PRE_KEYWORD,
					"Chilly Surroundings",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Soggy Feet",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200367C RID: 13948
			public class REFRESHINGTOUCH
			{
				// Token: 0x0400E17E RID: 57726
				public static LocString NAME = "Heat Resistant";

				// Token: 0x0400E17F RID: 57727
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently visited a cooling station and is impervious to ",
					UI.PRE_KEYWORD,
					"Toasty Surroundings",
					UI.PST_KEYWORD,
					" as a result"
				});
			}

			// Token: 0x0200367D RID: 13949
			public class GUNKSICK
			{
				// Token: 0x0400E180 RID: 57728
				public static LocString NAME = "Gunk Extraction Required";

				// Token: 0x0400E181 RID: 57729
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant needs to visit a ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" as soon as possible\n\nThey will use a toilet as a last resort"
				});
			}

			// Token: 0x0200367E RID: 13950
			public class EXPELLINGGUNK
			{
				// Token: 0x0400E182 RID: 57730
				public static LocString NAME = "Making a mess";

				// Token: 0x0400E183 RID: 57731
				public static LocString TOOLTIP = "This Duplicant just couldn't hold it all in anymore";
			}

			// Token: 0x0200367F RID: 13951
			public class GUNKHUNGOVER
			{
				// Token: 0x0400E184 RID: 57732
				public static LocString NAME = "Gunk Mouth";

				// Token: 0x0400E185 RID: 57733
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently expelled built-up ",
					UI.PRE_KEYWORD,
					"Gunk",
					UI.PST_KEYWORD,
					" and can still taste it"
				});
			}

			// Token: 0x02003680 RID: 13952
			public class NOLUBRICATIONMINOR
			{
				// Token: 0x0400E186 RID: 57734
				public static LocString NAME = "Grinding Gears (Reduced)";

				// Token: 0x0400E187 RID: 57735
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's out of ",
					UI.PRE_KEYWORD,
					"Gear Oil",
					UI.PST_KEYWORD,
					" and cannot function properly\n\nThey need to find ",
					UI.PRE_KEYWORD,
					"Gear Balm",
					UI.PST_KEYWORD,
					" or visit a ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD,
					" as soon as possible"
				});
			}

			// Token: 0x02003681 RID: 13953
			public class NOLUBRICATIONMAJOR
			{
				// Token: 0x0400E188 RID: 57736
				public static LocString NAME = "Grinding Gears";

				// Token: 0x0400E189 RID: 57737
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's out of ",
					UI.PRE_KEYWORD,
					"Gear Oil",
					UI.PST_KEYWORD,
					" and cannot function properly\n\nThey need to find ",
					UI.PRE_KEYWORD,
					"Gear Balm",
					UI.PST_KEYWORD,
					" or visit a ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD,
					" as soon as possible"
				});
			}

			// Token: 0x02003682 RID: 13954
			public class BIONICOFFLINE
			{
				// Token: 0x0400E18A RID: 57738
				public static LocString NAME = "Powerless";

				// Token: 0x0400E18B RID: 57739
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is non-functional!\n\nDeliver a charged ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" and reboot their systems to revive them"
				});
			}

			// Token: 0x02003683 RID: 13955
			public class BIONICWATERSTRESS
			{
				// Token: 0x0400E18C RID: 57740
				public static LocString NAME = "Liquid Exposure";

				// Token: 0x0400E18D RID: 57741
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's bionic parts are currently in contact with incompatible ",
					UI.PRE_KEYWORD,
					"Liquids",
					UI.PST_KEYWORD,
					"\n\nProlonged exposure could have serious ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" consequences"
				});
			}

			// Token: 0x02003684 RID: 13956
			public class SLIPPED
			{
				// Token: 0x0400E18E RID: 57742
				public static LocString NAME = "Slipped";

				// Token: 0x0400E18F RID: 57743
				public static LocString TOOLTIP = "This Duplicant recently lost their footing on a slippery floor and feels embarrassed";
			}

			// Token: 0x02003685 RID: 13957
			public class BIONICBEDTIMEEFFECT
			{
				// Token: 0x0400E190 RID: 57744
				public static LocString NAME = "Defragmenting";

				// Token: 0x0400E191 RID: 57745
				public static LocString TOOLTIP = "This Duplicant is decluttering their internal data cache\n\nIt's helping them relax";
			}

			// Token: 0x02003686 RID: 13958
			public class DUPLICANTGOTMILK
			{
				// Token: 0x0400E192 RID: 57746
				public static LocString NAME = "Extra Hydrated";

				// Token: 0x0400E193 RID: 57747
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently drank ",
					UI.PRE_KEYWORD,
					"Brackene",
					UI.PST_KEYWORD,
					". It's helping them relax"
				});
			}

			// Token: 0x02003687 RID: 13959
			public class SOILEDSUIT
			{
				// Token: 0x0400E194 RID: 57748
				public static LocString NAME = "Soiled Suit";

				// Token: 0x0400E195 RID: 57749
				public static LocString TOOLTIP = "This Duplicant's suit needs to be emptied of waste\n\n(Preferably soon)";

				// Token: 0x0400E196 RID: 57750
				public static LocString CAUSE = "Obtained when a Duplicant wears a suit filled with... \"fluids\"";
			}

			// Token: 0x02003688 RID: 13960
			public class SHOWERED
			{
				// Token: 0x0400E197 RID: 57751
				public static LocString NAME = "Showered";

				// Token: 0x0400E198 RID: 57752
				public static LocString TOOLTIP = "This Duplicant recently had a shower and feels squeaky clean!";
			}

			// Token: 0x02003689 RID: 13961
			public class SOREBACK
			{
				// Token: 0x0400E199 RID: 57753
				public static LocString NAME = "Sore Back";

				// Token: 0x0400E19A RID: 57754
				public static LocString TOOLTIP = "This Duplicant feels achy from sleeping on the floor last night and would like a bed";

				// Token: 0x0400E19B RID: 57755
				public static LocString CAUSE = "Obtained by sleeping on the ground";
			}

			// Token: 0x0200368A RID: 13962
			public class GOODEATS
			{
				// Token: 0x0400E19C RID: 57756
				public static LocString NAME = "Soul Food";

				// Token: 0x0400E19D RID: 57757
				public static LocString TOOLTIP = "This Duplicant had a yummy home cooked meal and is totally stuffed";

				// Token: 0x0400E19E RID: 57758
				public static LocString CAUSE = "Obtained by eating a hearty home cooked meal";

				// Token: 0x0400E19F RID: 57759
				public static LocString DESCRIPTION = "Duplicants find this home cooked meal is emotionally comforting";
			}

			// Token: 0x0200368B RID: 13963
			public class HOTSTUFF
			{
				// Token: 0x0400E1A0 RID: 57760
				public static LocString NAME = "Hot Stuff";

				// Token: 0x0400E1A1 RID: 57761
				public static LocString TOOLTIP = "This Duplicant had an extremely spicy meal and is both exhilarated and a little " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;

				// Token: 0x0400E1A2 RID: 57762
				public static LocString CAUSE = "Obtained by eating a very spicy meal";

				// Token: 0x0400E1A3 RID: 57763
				public static LocString DESCRIPTION = "Duplicants find this spicy meal quite invigorating";
			}

			// Token: 0x0200368C RID: 13964
			public class WARMTOUCHFOOD
			{
				// Token: 0x0400E1A4 RID: 57764
				public static LocString NAME = "Frost Resistant: Spicy Diet";

				// Token: 0x0400E1A5 RID: 57765
				public static LocString TOOLTIP = "This Duplicant ate spicy food and feels so warm inside that they don't even notice the cold right now";

				// Token: 0x0400E1A6 RID: 57766
				public static LocString CAUSE = "Obtained by eating a very spicy meal";

				// Token: 0x0400E1A7 RID: 57767
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Eating this provides temporary immunity to ",
					UI.PRE_KEYWORD,
					"Chilly Surroundings",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Soggy Feet",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200368D RID: 13965
			public class SEAFOODRADIATIONRESISTANCE
			{
				// Token: 0x0400E1A8 RID: 57768
				public static LocString NAME = "Radiation Resistant: Aquatic Diet";

				// Token: 0x0400E1A9 RID: 57769
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant ate sea-grown foods, which boost ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" resistance"
				});

				// Token: 0x0400E1AA RID: 57770
				public static LocString CAUSE = "Obtained by eating sea-grown foods like fish or lettuce";

				// Token: 0x0400E1AB RID: 57771
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Eating this improves ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" resistance"
				});
			}

			// Token: 0x0200368E RID: 13966
			public class RECENTLYPARTIED
			{
				// Token: 0x0400E1AC RID: 57772
				public static LocString NAME = "Partied Hard";

				// Token: 0x0400E1AD RID: 57773
				public static LocString TOOLTIP = "This Duplicant recently attended a great party!";
			}

			// Token: 0x0200368F RID: 13967
			public class NOFUNALLOWED
			{
				// Token: 0x0400E1AE RID: 57774
				public static LocString NAME = "Fun Interrupted";

				// Token: 0x0400E1AF RID: 57775
				public static LocString TOOLTIP = "This Duplicant is upset a party was rejected";
			}

			// Token: 0x02003690 RID: 13968
			public class CONTAMINATEDLUNGS
			{
				// Token: 0x0400E1B0 RID: 57776
				public static LocString NAME = "Yucky Lungs";

				// Token: 0x0400E1B1 RID: 57777
				public static LocString TOOLTIP = "This Duplicant got a big nasty lungful of " + ELEMENTS.CONTAMINATEDOXYGEN.NAME;
			}

			// Token: 0x02003691 RID: 13969
			public class MINORIRRITATION
			{
				// Token: 0x0400E1B2 RID: 57778
				public static LocString NAME = "Minor Eye Irritation";

				// Token: 0x0400E1B3 RID: 57779
				public static LocString TOOLTIP = "A gas or liquid made this Duplicant's eyes sting a little";

				// Token: 0x0400E1B4 RID: 57780
				public static LocString CAUSE = "Obtained by exposure to a harsh liquid or gas";
			}

			// Token: 0x02003692 RID: 13970
			public class MAJORIRRITATION
			{
				// Token: 0x0400E1B5 RID: 57781
				public static LocString NAME = "Major Eye Irritation";

				// Token: 0x0400E1B6 RID: 57782
				public static LocString TOOLTIP = "Woah, something really messed up this Duplicant's eyes!\n\nCaused by exposure to a harsh liquid or gas";

				// Token: 0x0400E1B7 RID: 57783
				public static LocString CAUSE = "Obtained by exposure to a harsh liquid or gas";
			}

			// Token: 0x02003693 RID: 13971
			public class FRESH_AND_CLEAN
			{
				// Token: 0x0400E1B8 RID: 57784
				public static LocString NAME = "Refreshingly Clean";

				// Token: 0x0400E1B9 RID: 57785
				public static LocString TOOLTIP = "This Duplicant took a warm shower and it was great!";

				// Token: 0x0400E1BA RID: 57786
				public static LocString CAUSE = "Obtained by taking a comfortably heated shower";
			}

			// Token: 0x02003694 RID: 13972
			public class BURNED_BY_SCALDING_WATER
			{
				// Token: 0x0400E1BB RID: 57787
				public static LocString NAME = "Scalded";

				// Token: 0x0400E1BC RID: 57788
				public static LocString TOOLTIP = "Ouch! This Duplicant showered or was doused in water that was way too hot";

				// Token: 0x0400E1BD RID: 57789
				public static LocString CAUSE = "Obtained by exposure to hot water";
			}

			// Token: 0x02003695 RID: 13973
			public class STRESSED_BY_COLD_WATER
			{
				// Token: 0x0400E1BE RID: 57790
				public static LocString NAME = "Numb";

				// Token: 0x0400E1BF RID: 57791
				public static LocString TOOLTIP = "Brr! This Duplicant was showered or doused in water that was way too cold";

				// Token: 0x0400E1C0 RID: 57792
				public static LocString CAUSE = "Obtained by exposure to icy water";
			}

			// Token: 0x02003696 RID: 13974
			public class SMELLEDSTINKY
			{
				// Token: 0x0400E1C1 RID: 57793
				public static LocString NAME = "Smelled Stinky";

				// Token: 0x0400E1C2 RID: 57794
				public static LocString TOOLTIP = "This Duplicant got a whiff of a certain somebody";
			}

			// Token: 0x02003697 RID: 13975
			public class STRESSREDUCTION
			{
				// Token: 0x0400E1C3 RID: 57795
				public static LocString NAME = "Receiving Massage";

				// Token: 0x0400E1C4 RID: 57796
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is just melting away"
				});
			}

			// Token: 0x02003698 RID: 13976
			public class STRESSREDUCTION_CLINIC
			{
				// Token: 0x0400E1C5 RID: 57797
				public static LocString NAME = "Receiving Clinic Massage";

				// Token: 0x0400E1C6 RID: 57798
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Clinical facilities are improving the effectiveness of this massage\n\nThis Duplicant's ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is just melting away"
				});
			}

			// Token: 0x02003699 RID: 13977
			public class UGLY_CRYING
			{
				// Token: 0x0400E1C7 RID: 57799
				public static LocString NAME = "Ugly Crying";

				// Token: 0x0400E1C8 RID: 57800
				public static LocString TOOLTIP = "This Duplicant is having a cathartic ugly cry as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400E1C9 RID: 57801
				public static LocString NOTIFICATION_NAME = "Ugly Crying";

				// Token: 0x0400E1CA RID: 57802
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they broke down crying:";
			}

			// Token: 0x0200369A RID: 13978
			public class BINGE_EATING
			{
				// Token: 0x0400E1CB RID: 57803
				public static LocString NAME = "Insatiable Hunger";

				// Token: 0x0400E1CC RID: 57804
				public static LocString TOOLTIP = "This Duplicant is stuffing their face as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400E1CD RID: 57805
				public static LocString NOTIFICATION_NAME = "Binge Eating";

				// Token: 0x0400E1CE RID: 57806
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began overeating:";
			}

			// Token: 0x0200369B RID: 13979
			public class BANSHEE_WAILING
			{
				// Token: 0x0400E1CF RID: 57807
				public static LocString NAME = "Deafening Shriek";

				// Token: 0x0400E1D0 RID: 57808
				public static LocString TOOLTIP = "This Duplicant is wailing at the top of their lungs as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400E1D1 RID: 57809
				public static LocString NOTIFICATION_NAME = "Banshee Wailing";

				// Token: 0x0400E1D2 RID: 57810
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began wailing:";
			}

			// Token: 0x0200369C RID: 13980
			public class STRESSSHOCKER
			{
				// Token: 0x0400E1D3 RID: 57811
				public static LocString NAME = "Shocking Temper";

				// Token: 0x0400E1D4 RID: 57812
				public static LocString TOOLTIP = "This Duplicant is short-circuiting as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400E1D5 RID: 57813
				public static LocString NOTIFICATION_NAME = "Stress Zapping";

				// Token: 0x0400E1D6 RID: 57814
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began emitting electrical zaps:";
			}

			// Token: 0x0200369D RID: 13981
			public class BANSHEE_WAILING_RECOVERY
			{
				// Token: 0x0400E1D7 RID: 57815
				public static LocString NAME = "Guzzling Air";

				// Token: 0x0400E1D8 RID: 57816
				public static LocString TOOLTIP = "This Duplicant needs a little extra oxygen to catch their breath";
			}

			// Token: 0x0200369E RID: 13982
			public class METABOLISM_CALORIE_MODIFIER
			{
				// Token: 0x0400E1D9 RID: 57817
				public static LocString NAME = "Metabolism";

				// Token: 0x0400E1DA RID: 57818
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Metabolism",
					UI.PST_KEYWORD,
					" determines how quickly a critter burns ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200369F RID: 13983
			public class WORKING
			{
				// Token: 0x0400E1DB RID: 57819
				public static LocString NAME = "Working";

				// Token: 0x0400E1DC RID: 57820
				public static LocString TOOLTIP = "This Duplicant is working up a sweat";
			}

			// Token: 0x020036A0 RID: 13984
			public class UNCOMFORTABLESLEEP
			{
				// Token: 0x0400E1DD RID: 57821
				public static LocString NAME = "Sleeping Uncomfortably";

				// Token: 0x0400E1DE RID: 57822
				public static LocString TOOLTIP = "This Duplicant collapsed on the floor from sheer exhaustion";
			}

			// Token: 0x020036A1 RID: 13985
			public class MANAGERIALDUTIES
			{
				// Token: 0x0400E1DF RID: 57823
				public static LocString NAME = "Managerial Duties";

				// Token: 0x0400E1E0 RID: 57824
				public static LocString TOOLTIP = "Being a manager is stressful";
			}

			// Token: 0x020036A2 RID: 13986
			public class MANAGEDCOLONY
			{
				// Token: 0x0400E1E1 RID: 57825
				public static LocString NAME = "Managed Colony";

				// Token: 0x0400E1E2 RID: 57826
				public static LocString TOOLTIP = "A Duplicant is in the colony manager job";
			}

			// Token: 0x020036A3 RID: 13987
			public class BIONIC_WATTS
			{
				// Token: 0x0400E1E3 RID: 57827
				public static LocString NAME = "Wattage";

				// Token: 0x0400E1E4 RID: 57828
				public static LocString BASE_NAME = "Base";

				// Token: 0x0400E1E5 RID: 57829
				public static LocString SAVING_MODE_NAME = "Standby Mode";

				// Token: 0x02003D8E RID: 15758
				public class TOOLTIP
				{
					// Token: 0x0400F2F6 RID: 62198
					public static LocString ESTIMATED_LIFE_TIME_REMAINING = string.Concat(new string[]
					{
						"Estimated ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" supply remaining: {0}"
					});

					// Token: 0x0400F2F7 RID: 62199
					public static LocString ELECTROBANK_DETAILS_LABEL = "Total Electrobanks {0} / {1}";

					// Token: 0x0400F2F8 RID: 62200
					public static LocString ELECTROBANK_ROW = "{0} {1}: {2}";

					// Token: 0x0400F2F9 RID: 62201
					public static LocString ELECTROBANK_EMPTY_ROW = "{0} Empty";

					// Token: 0x0400F2FA RID: 62202
					public static LocString CURRENT_WATTAGE_LABEL = "Current Wattage: {0}";

					// Token: 0x0400F2FB RID: 62203
					public static LocString POTENTIAL_EXTRA_WATTAGE_LABEL = "Potential Wattage: {0}";

					// Token: 0x0400F2FC RID: 62204
					public static LocString STANDARD_ACTIVE_TEMPLATE = "{0}: {1}";

					// Token: 0x0400F2FD RID: 62205
					public static LocString STANDARD_INACTIVE_TEMPLATE = "{0}: {1}";
				}
			}

			// Token: 0x020036A4 RID: 13988
			public class FLOORSLEEP
			{
				// Token: 0x0400E1E6 RID: 57830
				public static LocString NAME = "Sleeping On Floor";

				// Token: 0x0400E1E7 RID: 57831
				public static LocString TOOLTIP = "This Duplicant is uncomfortably recovering " + UI.PRE_KEYWORD + "Stamina" + UI.PST_KEYWORD;
			}

			// Token: 0x020036A5 RID: 13989
			public class PASSEDOUTSLEEP
			{
				// Token: 0x0400E1E8 RID: 57832
				public static LocString NAME = "Exhausted";

				// Token: 0x0400E1E9 RID: 57833
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Lack of rest depleted this Duplicant's ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					"\n\nThey passed out from the fatigue"
				});
			}

			// Token: 0x020036A6 RID: 13990
			public class SLEEP
			{
				// Token: 0x0400E1EA RID: 57834
				public static LocString NAME = "Sleeping";

				// Token: 0x0400E1EB RID: 57835
				public static LocString TOOLTIP = "This Duplicant is recovering " + UI.PRE_KEYWORD + "Stamina" + UI.PST_KEYWORD;
			}

			// Token: 0x020036A7 RID: 13991
			public class SLEEPCLINIC
			{
				// Token: 0x0400E1EC RID: 57836
				public static LocString NAME = "Nodding Off";

				// Token: 0x0400E1ED RID: 57837
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is losing ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					" because of their ",
					UI.PRE_KEYWORD,
					"Pajamas",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020036A8 RID: 13992
			public class RESTFULSLEEP
			{
				// Token: 0x0400E1EE RID: 57838
				public static LocString NAME = "Sleeping Peacefully";

				// Token: 0x0400E1EF RID: 57839
				public static LocString TOOLTIP = "This Duplicant is getting a good night's rest";
			}

			// Token: 0x020036A9 RID: 13993
			public class SLEEPY
			{
				// Token: 0x0400E1F0 RID: 57840
				public static LocString NAME = "Sleepy";

				// Token: 0x0400E1F1 RID: 57841
				public static LocString TOOLTIP = "This Duplicant is getting tired";
			}

			// Token: 0x020036AA RID: 13994
			public class HUNGRY
			{
				// Token: 0x0400E1F2 RID: 57842
				public static LocString NAME = "Hungry";

				// Token: 0x0400E1F3 RID: 57843
				public static LocString TOOLTIP = "This Duplicant is ready for lunch";
			}

			// Token: 0x020036AB RID: 13995
			public class STARVING
			{
				// Token: 0x0400E1F4 RID: 57844
				public static LocString NAME = "Starving";

				// Token: 0x0400E1F5 RID: 57845
				public static LocString TOOLTIP = "This Duplicant needs to eat something, soon";
			}

			// Token: 0x020036AC RID: 13996
			public class HOT
			{
				// Token: 0x0400E1F6 RID: 57846
				public static LocString NAME = "Hot";

				// Token: 0x0400E1F7 RID: 57847
				public static LocString TOOLTIP = "This Duplicant is uncomfortably warm";
			}

			// Token: 0x020036AD RID: 13997
			public class COLD
			{
				// Token: 0x0400E1F8 RID: 57848
				public static LocString NAME = "Cold";

				// Token: 0x0400E1F9 RID: 57849
				public static LocString TOOLTIP = "This Duplicant is uncomfortably cold";
			}

			// Token: 0x020036AE RID: 13998
			public class CARPETFEET
			{
				// Token: 0x0400E1FA RID: 57850
				public static LocString NAME = "Tickled Tootsies";

				// Token: 0x0400E1FB RID: 57851
				public static LocString TOOLTIP = "Walking on carpet has made this Duplicant's day a little more luxurious";
			}

			// Token: 0x020036AF RID: 13999
			public class WETFEET
			{
				// Token: 0x0400E1FC RID: 57852
				public static LocString NAME = "Soggy Feet";

				// Token: 0x0400E1FD RID: 57853
				public static LocString TOOLTIP = "This Duplicant recently stepped in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

				// Token: 0x0400E1FE RID: 57854
				public static LocString CAUSE = "Obtained by walking through liquid";
			}

			// Token: 0x020036B0 RID: 14000
			public class SOAKINGWET
			{
				// Token: 0x0400E1FF RID: 57855
				public static LocString NAME = "Sopping Wet";

				// Token: 0x0400E200 RID: 57856
				public static LocString TOOLTIP = "This Duplicant was recently submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

				// Token: 0x0400E201 RID: 57857
				public static LocString CAUSE = "Obtained from submergence in liquid";
			}

			// Token: 0x020036B1 RID: 14001
			public class POPPEDEARDRUMS
			{
				// Token: 0x0400E202 RID: 57858
				public static LocString NAME = "Popped Eardrums";

				// Token: 0x0400E203 RID: 57859
				public static LocString TOOLTIP = "This Duplicant was exposed to an over-pressurized area that popped their eardrums";
			}

			// Token: 0x020036B2 RID: 14002
			public class ANEWHOPE
			{
				// Token: 0x0400E204 RID: 57860
				public static LocString NAME = "New Hope";

				// Token: 0x0400E205 RID: 57861
				public static LocString TOOLTIP = "This Duplicant feels pretty optimistic about their new home";
			}

			// Token: 0x020036B3 RID: 14003
			public class MEGABRAINTANKBONUS
			{
				// Token: 0x0400E206 RID: 57862
				public static LocString NAME = "Maximum Aptitude";

				// Token: 0x0400E207 RID: 57863
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is smarter and stronger than usual thanks to the ",
					UI.PRE_KEYWORD,
					"Somnium Synthesizer",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x020036B4 RID: 14004
			public class PRICKLEFRUITDAMAGE
			{
				// Token: 0x0400E208 RID: 57864
				public static LocString NAME = "Ouch!";

				// Token: 0x0400E209 RID: 57865
				public static LocString TOOLTIP = "This Duplicant ate a raw " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " and it gave their mouth ouchies";
			}

			// Token: 0x020036B5 RID: 14005
			public class NOOXYGEN
			{
				// Token: 0x0400E20A RID: 57866
				public static LocString NAME = "No Oxygen";

				// Token: 0x0400E20B RID: 57867
				public static LocString TOOLTIP = "There is no breathable air in this area";
			}

			// Token: 0x020036B6 RID: 14006
			public class LOWOXYGEN
			{
				// Token: 0x0400E20C RID: 57868
				public static LocString NAME = "Low Oxygen";

				// Token: 0x0400E20D RID: 57869
				public static LocString TOOLTIP = "The air is thin in this area";
			}

			// Token: 0x020036B7 RID: 14007
			public class LOWOXYGENBIONIC
			{
				// Token: 0x0400E20E RID: 57870
				public static LocString NAME = "Low Oxygen Tank";

				// Token: 0x0400E20F RID: 57871
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tank is dangerously low"
				});
			}

			// Token: 0x020036B8 RID: 14008
			public class MOURNING
			{
				// Token: 0x0400E210 RID: 57872
				public static LocString NAME = "Mourning";

				// Token: 0x0400E211 RID: 57873
				public static LocString TOOLTIP = "This Duplicant is grieving the loss of a friend";
			}

			// Token: 0x020036B9 RID: 14009
			public class NARCOLEPTICSLEEP
			{
				// Token: 0x0400E212 RID: 57874
				public static LocString NAME = "Narcoleptic Nap";

				// Token: 0x0400E213 RID: 57875
				public static LocString TOOLTIP = "This Duplicant just needs to rest their eyes for a second";
			}

			// Token: 0x020036BA RID: 14010
			public class BADSLEEP
			{
				// Token: 0x0400E214 RID: 57876
				public static LocString NAME = "Unrested: Too Bright";

				// Token: 0x0400E215 RID: 57877
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant tossed and turned all night because a ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" was left on where they were trying to sleep"
				});
			}

			// Token: 0x020036BB RID: 14011
			public class BADSLEEPAFRAIDOFDARK
			{
				// Token: 0x0400E216 RID: 57878
				public static LocString NAME = "Unrested: Afraid of Dark";

				// Token: 0x0400E217 RID: 57879
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant didn't get much sleep because they were too anxious about the lack of ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" to relax"
				});
			}

			// Token: 0x020036BC RID: 14012
			public class BADSLEEPMOVEMENT
			{
				// Token: 0x0400E218 RID: 57880
				public static LocString NAME = "Unrested: Bed Jostling";

				// Token: 0x0400E219 RID: 57881
				public static LocString TOOLTIP = "This Duplicant was woken up when a friend climbed on their ladder bed";
			}

			// Token: 0x020036BD RID: 14013
			public class BADSLEEPCOLD
			{
				// Token: 0x0400E21A RID: 57882
				public static LocString NAME = "Unrested: Cold Bedroom";

				// Token: 0x0400E21B RID: 57883
				public static LocString TOOLTIP = "This Duplicant was shivering instead of sleeping";
			}

			// Token: 0x020036BE RID: 14014
			public class BADSLEEPDEFRAGMENTING
			{
				// Token: 0x0400E21C RID: 57884
				public static LocString NAME = "Unrested: Too Bright";

				// Token: 0x0400E21D RID: 57885
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant kept waking up because of the ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" produced by a Bionic Duplicant defragmenting nearby"
				});
			}

			// Token: 0x020036BF RID: 14015
			public class TERRIBLESLEEP
			{
				// Token: 0x0400E21E RID: 57886
				public static LocString NAME = "Dead Tired: Snoring Friend";

				// Token: 0x0400E21F RID: 57887
				public static LocString TOOLTIP = "This Duplicant didn't get any shuteye last night because of all the racket from a friend's snoring";
			}

			// Token: 0x020036C0 RID: 14016
			public class PEACEFULSLEEP
			{
				// Token: 0x0400E220 RID: 57888
				public static LocString NAME = "Well Rested";

				// Token: 0x0400E221 RID: 57889
				public static LocString TOOLTIP = "This Duplicant had a blissfully quiet sleep last night";
			}

			// Token: 0x020036C1 RID: 14017
			public class CENTEROFATTENTION
			{
				// Token: 0x0400E222 RID: 57890
				public static LocString NAME = "Center of Attention";

				// Token: 0x0400E223 RID: 57891
				public static LocString TOOLTIP = "This Duplicant feels like someone's watching over them...";
			}

			// Token: 0x020036C2 RID: 14018
			public class INSPIRED
			{
				// Token: 0x0400E224 RID: 57892
				public static LocString NAME = "Inspired";

				// Token: 0x0400E225 RID: 57893
				public static LocString TOOLTIP = "This Duplicant has had a creative vision!";
			}

			// Token: 0x020036C3 RID: 14019
			public class NEWCREWARRIVAL
			{
				// Token: 0x0400E226 RID: 57894
				public static LocString NAME = "New Friend";

				// Token: 0x0400E227 RID: 57895
				public static LocString TOOLTIP = "This Duplicant is happy to see a new face in the colony";
			}

			// Token: 0x020036C4 RID: 14020
			public class UNDERWATER
			{
				// Token: 0x0400E228 RID: 57896
				public static LocString NAME = "Underwater";

				// Token: 0x0400E229 RID: 57897
				public static LocString TOOLTIP = "This Duplicant's movement is slowed";
			}

			// Token: 0x020036C5 RID: 14021
			public class NIGHTMARES
			{
				// Token: 0x0400E22A RID: 57898
				public static LocString NAME = "Nightmares";

				// Token: 0x0400E22B RID: 57899
				public static LocString TOOLTIP = "This Duplicant was visited by something in the night";
			}

			// Token: 0x020036C6 RID: 14022
			public class WASATTACKED
			{
				// Token: 0x0400E22C RID: 57900
				public static LocString NAME = "Recently assailed";

				// Token: 0x0400E22D RID: 57901
				public static LocString TOOLTIP = "This Duplicant is stressed out after having been attacked";
			}

			// Token: 0x020036C7 RID: 14023
			public class LIGHTWOUNDS
			{
				// Token: 0x0400E22E RID: 57902
				public static LocString NAME = "Light Wounds";

				// Token: 0x0400E22F RID: 57903
				public static LocString TOOLTIP = "This Duplicant sustained injuries that are a bit uncomfortable";
			}

			// Token: 0x020036C8 RID: 14024
			public class MODERATEWOUNDS
			{
				// Token: 0x0400E230 RID: 57904
				public static LocString NAME = "Moderate Wounds";

				// Token: 0x0400E231 RID: 57905
				public static LocString TOOLTIP = "This Duplicant sustained injuries that are affecting their ability to work";
			}

			// Token: 0x020036C9 RID: 14025
			public class SEVEREWOUNDS
			{
				// Token: 0x0400E232 RID: 57906
				public static LocString NAME = "Severe Wounds";

				// Token: 0x0400E233 RID: 57907
				public static LocString TOOLTIP = "This Duplicant sustained serious injuries that are impacting their work and well-being";
			}

			// Token: 0x020036CA RID: 14026
			public class LIGHTWOUNDSCRITTER
			{
				// Token: 0x0400E234 RID: 57908
				public static LocString NAME = "Light Wounds";

				// Token: 0x0400E235 RID: 57909
				public static LocString TOOLTIP = "This Critter sustained injuries that are a bit uncomfortable";
			}

			// Token: 0x020036CB RID: 14027
			public class MODERATEWOUNDSCRITTER
			{
				// Token: 0x0400E236 RID: 57910
				public static LocString NAME = "Moderate Wounds";

				// Token: 0x0400E237 RID: 57911
				public static LocString TOOLTIP = "This Critter sustained injuries that are really affecting their health";
			}

			// Token: 0x020036CC RID: 14028
			public class SEVEREWOUNDSCRITTER
			{
				// Token: 0x0400E238 RID: 57912
				public static LocString NAME = "Severe Wounds";

				// Token: 0x0400E239 RID: 57913
				public static LocString TOOLTIP = "This Critter sustained serious injuries that could prove life-threatening";
			}

			// Token: 0x020036CD RID: 14029
			public class SANDBOXMORALEADJUSTMENT
			{
				// Token: 0x0400E23A RID: 57914
				public static LocString NAME = "Sandbox Morale Adjustment";

				// Token: 0x0400E23B RID: 57915
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had their ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" temporarily adjusted using the Sandbox tools"
				});
			}

			// Token: 0x020036CE RID: 14030
			public class ROTTEMPERATURE
			{
				// Token: 0x0400E23C RID: 57916
				public static LocString UNREFRIGERATED = "Unrefrigerated";

				// Token: 0x0400E23D RID: 57917
				public static LocString REFRIGERATED = "Refrigerated";

				// Token: 0x0400E23E RID: 57918
				public static LocString FROZEN = "Frozen";
			}

			// Token: 0x020036CF RID: 14031
			public class ROTATMOSPHERE
			{
				// Token: 0x0400E23F RID: 57919
				public static LocString CONTAMINATED = "Contaminated Air";

				// Token: 0x0400E240 RID: 57920
				public static LocString NORMAL = "Normal Atmosphere";

				// Token: 0x0400E241 RID: 57921
				public static LocString STERILE = "Sterile Atmosphere";
			}

			// Token: 0x020036D0 RID: 14032
			public class BASEROT
			{
				// Token: 0x0400E242 RID: 57922
				public static LocString NAME = "Base Decay Rate";
			}

			// Token: 0x020036D1 RID: 14033
			public class FULLBLADDER
			{
				// Token: 0x0400E243 RID: 57923
				public static LocString NAME = "Full Bladder";

				// Token: 0x0400E244 RID: 57924
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					" is full"
				});
			}

			// Token: 0x020036D2 RID: 14034
			public class DIARRHEA
			{
				// Token: 0x0400E245 RID: 57925
				public static LocString NAME = "Diarrhea";

				// Token: 0x0400E246 RID: 57926
				public static LocString TOOLTIP = "This Duplicant's gut is giving them some trouble";

				// Token: 0x0400E247 RID: 57927
				public static LocString CAUSE = "Obtained by eating a disgusting meal";

				// Token: 0x0400E248 RID: 57928
				public static LocString DESCRIPTION = "Most Duplicants experience stomach upset from this meal";
			}

			// Token: 0x020036D3 RID: 14035
			public class STRESSFULYEMPTYINGBLADDER
			{
				// Token: 0x0400E249 RID: 57929
				public static LocString NAME = "Making a mess";

				// Token: 0x0400E24A RID: 57930
				public static LocString TOOLTIP = "This Duplicant had no choice but to empty their " + UI.PRE_KEYWORD + "Bladder" + UI.PST_KEYWORD;
			}

			// Token: 0x020036D4 RID: 14036
			public class REDALERT
			{
				// Token: 0x0400E24B RID: 57931
				public static LocString NAME = "Red Alert!";

				// Token: 0x0400E24C RID: 57932
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					" is stressing this Duplicant out"
				});
			}

			// Token: 0x020036D5 RID: 14037
			public class FUSSY
			{
				// Token: 0x0400E24D RID: 57933
				public static LocString NAME = "Fussy";

				// Token: 0x0400E24E RID: 57934
				public static LocString TOOLTIP = "This Duplicant is hard to please";
			}

			// Token: 0x020036D6 RID: 14038
			public class WARMINGUP
			{
				// Token: 0x0400E24F RID: 57935
				public static LocString NAME = "Warming Up";

				// Token: 0x0400E250 RID: 57936
				public static LocString TOOLTIP = "This Duplicant is trying to warm back up";
			}

			// Token: 0x020036D7 RID: 14039
			public class COOLINGDOWN
			{
				// Token: 0x0400E251 RID: 57937
				public static LocString NAME = "Cooling Down";

				// Token: 0x0400E252 RID: 57938
				public static LocString TOOLTIP = "This Duplicant is trying to cool back down";
			}

			// Token: 0x020036D8 RID: 14040
			public class DARKNESS
			{
				// Token: 0x0400E253 RID: 57939
				public static LocString NAME = "Darkness";

				// Token: 0x0400E254 RID: 57940
				public static LocString TOOLTIP = "Eep! This Duplicant doesn't like being in the dark!";
			}

			// Token: 0x020036D9 RID: 14041
			public class STEPPEDINCONTAMINATEDWATER
			{
				// Token: 0x0400E255 RID: 57941
				public static LocString NAME = "Stepped in polluted water";

				// Token: 0x0400E256 RID: 57942
				public static LocString TOOLTIP = "Gross! This Duplicant stepped in something yucky";
			}

			// Token: 0x020036DA RID: 14042
			public class WELLFED
			{
				// Token: 0x0400E257 RID: 57943
				public static LocString NAME = "Well fed";

				// Token: 0x0400E258 RID: 57944
				public static LocString TOOLTIP = "This Duplicant feels satisfied after having a big meal";
			}

			// Token: 0x020036DB RID: 14043
			public class STALEFOOD
			{
				// Token: 0x0400E259 RID: 57945
				public static LocString NAME = "Bad leftovers";

				// Token: 0x0400E25A RID: 57946
				public static LocString TOOLTIP = "This Duplicant is in a bad mood from having to eat stale " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x020036DC RID: 14044
			public class ATEFROZENFOOD
			{
				// Token: 0x0400E25B RID: 57947
				public static LocString NAME = "Ate frozen food";

				// Token: 0x0400E25C RID: 57948
				public static LocString TOOLTIP = "This Duplicant is in a bad mood from having to eat deep-frozen " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x020036DD RID: 14045
			public class SMELLEDPUTRIDODOUR
			{
				// Token: 0x0400E25D RID: 57949
				public static LocString NAME = "Smelled a putrid odor";

				// Token: 0x0400E25E RID: 57950
				public static LocString TOOLTIP = "This Duplicant got a whiff of something unspeakably foul";
			}

			// Token: 0x020036DE RID: 14046
			public class VOMITING
			{
				// Token: 0x0400E25F RID: 57951
				public static LocString NAME = "Vomiting";

				// Token: 0x0400E260 RID: 57952
				public static LocString TOOLTIP = "Better out than in, as they say";
			}

			// Token: 0x020036DF RID: 14047
			public class BREATHING
			{
				// Token: 0x0400E261 RID: 57953
				public static LocString NAME = "Breathing";
			}

			// Token: 0x020036E0 RID: 14048
			public class HOLDINGBREATH
			{
				// Token: 0x0400E262 RID: 57954
				public static LocString NAME = "Holding breath";
			}

			// Token: 0x020036E1 RID: 14049
			public class RECOVERINGBREATH
			{
				// Token: 0x0400E263 RID: 57955
				public static LocString NAME = "Recovering breath";
			}

			// Token: 0x020036E2 RID: 14050
			public class ROTTING
			{
				// Token: 0x0400E264 RID: 57956
				public static LocString NAME = "Rotting";
			}

			// Token: 0x020036E3 RID: 14051
			public class DEAD
			{
				// Token: 0x0400E265 RID: 57957
				public static LocString NAME = "Dead";
			}

			// Token: 0x020036E4 RID: 14052
			public class TOXICENVIRONMENT
			{
				// Token: 0x0400E266 RID: 57958
				public static LocString NAME = "Toxic environment";
			}

			// Token: 0x020036E5 RID: 14053
			public class RESTING
			{
				// Token: 0x0400E267 RID: 57959
				public static LocString NAME = "Resting";
			}

			// Token: 0x020036E6 RID: 14054
			public class INTRAVENOUS_NUTRITION
			{
				// Token: 0x0400E268 RID: 57960
				public static LocString NAME = "Intravenous Feeding";
			}

			// Token: 0x020036E7 RID: 14055
			public class CATHETERIZED
			{
				// Token: 0x0400E269 RID: 57961
				public static LocString NAME = "Catheterized";

				// Token: 0x0400E26A RID: 57962
				public static LocString TOOLTIP = "Let's leave it at that";
			}

			// Token: 0x020036E8 RID: 14056
			public class NOISEPEACEFUL
			{
				// Token: 0x0400E26B RID: 57963
				public static LocString NAME = "Peace and Quiet";

				// Token: 0x0400E26C RID: 57964
				public static LocString TOOLTIP = "This Duplicant has found a quiet place to concentrate";
			}

			// Token: 0x020036E9 RID: 14057
			public class NOISEMINOR
			{
				// Token: 0x0400E26D RID: 57965
				public static LocString NAME = "Loud Noises";

				// Token: 0x0400E26E RID: 57966
				public static LocString TOOLTIP = "This area is a bit too loud for comfort";
			}

			// Token: 0x020036EA RID: 14058
			public class NOISEMAJOR
			{
				// Token: 0x0400E26F RID: 57967
				public static LocString NAME = "Cacophony!";

				// Token: 0x0400E270 RID: 57968
				public static LocString TOOLTIP = "It's very, very loud in here!";
			}

			// Token: 0x020036EB RID: 14059
			public class MEDICALCOT
			{
				// Token: 0x0400E271 RID: 57969
				public static LocString NAME = "Triage Cot Rest";

				// Token: 0x0400E272 RID: 57970
				public static LocString TOOLTIP = "Bedrest is improving this Duplicant's physical recovery time";
			}

			// Token: 0x020036EC RID: 14060
			public class MEDICALCOTDOCTORED
			{
				// Token: 0x0400E273 RID: 57971
				public static LocString NAME = "Receiving treatment";

				// Token: 0x0400E274 RID: 57972
				public static LocString TOOLTIP = "This Duplicant is receiving treatment for their physical injuries";
			}

			// Token: 0x020036ED RID: 14061
			public class DOCTOREDOFFCOTEFFECT
			{
				// Token: 0x0400E275 RID: 57973
				public static LocString NAME = "Runaway Patient";

				// Token: 0x0400E276 RID: 57974
				public static LocString TOOLTIP = "Tsk tsk!\nThis Duplicant cannot receive treatment while out of their medical bed!";
			}

			// Token: 0x020036EE RID: 14062
			public class POSTDISEASERECOVERY
			{
				// Token: 0x0400E277 RID: 57975
				public static LocString NAME = "Feeling better";

				// Token: 0x0400E278 RID: 57976
				public static LocString TOOLTIP = "This Duplicant is up and about, but they still have some lingering effects from their " + UI.PRE_KEYWORD + "Disease" + UI.PST_KEYWORD;

				// Token: 0x0400E279 RID: 57977
				public static LocString ADDITIONAL_EFFECTS = "This Duplicant has temporary immunity to diseases from having beaten an infection";
			}

			// Token: 0x020036EF RID: 14063
			public class IMMUNESYSTEMOVERWHELMED
			{
				// Token: 0x0400E27A RID: 57978
				public static LocString NAME = "Immune System Overwhelmed";

				// Token: 0x0400E27B RID: 57979
				public static LocString TOOLTIP = "This Duplicant's immune system is slowly being overwhelmed by a high concentration of germs";
			}

			// Token: 0x020036F0 RID: 14064
			public class MEDICINE_GENERICPILL
			{
				// Token: 0x0400E27C RID: 57980
				public static LocString NAME = "Placebo";

				// Token: 0x0400E27D RID: 57981
				public static LocString TOOLTIP = ITEMS.PILLS.PLACEBO.DESC;

				// Token: 0x0400E27E RID: 57982
				public static LocString EFFECT_DESC = string.Concat(new string[]
				{
					"Applies the ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" effect"
				});
			}

			// Token: 0x020036F1 RID: 14065
			public class MEDICINE_BASICBOOSTER
			{
				// Token: 0x0400E27F RID: 57983
				public static LocString NAME = ITEMS.PILLS.BASICBOOSTER.NAME;

				// Token: 0x0400E280 RID: 57984
				public static LocString TOOLTIP = ITEMS.PILLS.BASICBOOSTER.DESC;
			}

			// Token: 0x020036F2 RID: 14066
			public class MEDICINE_INTERMEDIATEBOOSTER
			{
				// Token: 0x0400E281 RID: 57985
				public static LocString NAME = ITEMS.PILLS.INTERMEDIATEBOOSTER.NAME;

				// Token: 0x0400E282 RID: 57986
				public static LocString TOOLTIP = ITEMS.PILLS.INTERMEDIATEBOOSTER.DESC;
			}

			// Token: 0x020036F3 RID: 14067
			public class MEDICINE_BASICRADPILL
			{
				// Token: 0x0400E283 RID: 57987
				public static LocString NAME = ITEMS.PILLS.BASICRADPILL.NAME;

				// Token: 0x0400E284 RID: 57988
				public static LocString TOOLTIP = ITEMS.PILLS.BASICRADPILL.DESC;
			}

			// Token: 0x020036F4 RID: 14068
			public class MEDICINE_INTERMEDIATERADPILL
			{
				// Token: 0x0400E285 RID: 57989
				public static LocString NAME = ITEMS.PILLS.INTERMEDIATERADPILL.NAME;

				// Token: 0x0400E286 RID: 57990
				public static LocString TOOLTIP = ITEMS.PILLS.INTERMEDIATERADPILL.DESC;
			}

			// Token: 0x020036F5 RID: 14069
			public class SUNLIGHT_PLEASANT
			{
				// Token: 0x0400E287 RID: 57991
				public static LocString NAME = "Bright and Cheerful";

				// Token: 0x0400E288 RID: 57992
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The strong natural ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is making this Duplicant feel light on their feet"
				});
			}

			// Token: 0x020036F6 RID: 14070
			public class SUNLIGHT_BURNING
			{
				// Token: 0x0400E289 RID: 57993
				public static LocString NAME = "Intensely Bright";

				// Token: 0x0400E28A RID: 57994
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The bright ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is significantly improving this Duplicant's mood, but prolonged exposure may result in burning"
				});
			}

			// Token: 0x020036F7 RID: 14071
			public class TOOKABREAK
			{
				// Token: 0x0400E28B RID: 57995
				public static LocString NAME = "Downtime";

				// Token: 0x0400E28C RID: 57996
				public static LocString TOOLTIP = "This Duplicant has a bit of time off from work to attend to personal matters";
			}

			// Token: 0x020036F8 RID: 14072
			public class SOCIALIZED
			{
				// Token: 0x0400E28D RID: 57997
				public static LocString NAME = "Socialized";

				// Token: 0x0400E28E RID: 57998
				public static LocString TOOLTIP = "This Duplicant had some free time to hang out with buddies";
			}

			// Token: 0x020036F9 RID: 14073
			public class GOODCONVERSATION
			{
				// Token: 0x0400E28F RID: 57999
				public static LocString NAME = "Pleasant Chitchat";

				// Token: 0x0400E290 RID: 58000
				public static LocString TOOLTIP = "This Duplicant recently had a chance to chat with a friend";
			}

			// Token: 0x020036FA RID: 14074
			public class WORKENCOURAGED
			{
				// Token: 0x0400E291 RID: 58001
				public static LocString NAME = "Appreciated";

				// Token: 0x0400E292 RID: 58002
				public static LocString TOOLTIP = "Someone saw how hard this Duplicant was working and gave them a compliment\n\nThis Duplicant feels great about themselves now!";
			}

			// Token: 0x020036FB RID: 14075
			public class ISSTICKERBOMBING
			{
				// Token: 0x0400E293 RID: 58003
				public static LocString NAME = "Sticker Bombing";

				// Token: 0x0400E294 RID: 58004
				public static LocString TOOLTIP = "This Duplicant is slapping stickers onto everything!\n\nEveryone's gonna love these";
			}

			// Token: 0x020036FC RID: 14076
			public class ISSPARKLESTREAKER
			{
				// Token: 0x0400E295 RID: 58005
				public static LocString NAME = "Sparkle Streaking";

				// Token: 0x0400E296 RID: 58006
				public static LocString TOOLTIP = "This Duplicant is currently Sparkle Streaking!\n\nBaa-ling!";
			}

			// Token: 0x020036FD RID: 14077
			public class SAWSPARKLESTREAKER
			{
				// Token: 0x0400E297 RID: 58007
				public static LocString NAME = "Sparkle Flattered";

				// Token: 0x0400E298 RID: 58008
				public static LocString TOOLTIP = "A Sparkle Streaker's sparkles dazzled this Duplicant\n\nThis Duplicant has a spring in their step now!";
			}

			// Token: 0x020036FE RID: 14078
			public class ISJOYSINGER
			{
				// Token: 0x0400E299 RID: 58009
				public static LocString NAME = "Yodeling";

				// Token: 0x0400E29A RID: 58010
				public static LocString TOOLTIP = "This Duplicant is currently Yodeling!\n\nHow melodious!";
			}

			// Token: 0x020036FF RID: 14079
			public class HEARDJOYSINGER
			{
				// Token: 0x0400E29B RID: 58011
				public static LocString NAME = "Serenaded";

				// Token: 0x0400E29C RID: 58012
				public static LocString TOOLTIP = "A Yodeler's singing thrilled this Duplicant\n\nThis Duplicant works at a higher tempo now!";
			}

			// Token: 0x02003700 RID: 14080
			public class ISROBODANCER
			{
				// Token: 0x0400E29D RID: 58013
				public static LocString NAME = "Doing the Robot";

				// Token: 0x0400E29E RID: 58014
				public static LocString TOOLTIP = "This Duplicant is dancing like everybody's watching\n\nThey're a flash mob of one!";
			}

			// Token: 0x02003701 RID: 14081
			public class SAWROBODANCER
			{
				// Token: 0x0400E29F RID: 58015
				public static LocString NAME = "Hyped";

				// Token: 0x0400E2A0 RID: 58016
				public static LocString TOOLTIP = "A Flash Mobber's dance moves wowed this Duplicant\n\nThis Duplicant feels amped up now!";
			}

			// Token: 0x02003702 RID: 14082
			public class HASBALLOON
			{
				// Token: 0x0400E2A1 RID: 58017
				public static LocString NAME = "Balloon Buddy";

				// Token: 0x0400E2A2 RID: 58018
				public static LocString TOOLTIP = "A Balloon Artist gave this Duplicant a balloon!\n\nThis Duplicant feels super crafty now!";
			}

			// Token: 0x02003703 RID: 14083
			public class GREETING
			{
				// Token: 0x0400E2A3 RID: 58019
				public static LocString NAME = "Saw Friend";

				// Token: 0x0400E2A4 RID: 58020
				public static LocString TOOLTIP = "This Duplicant recently saw a friend in the halls and got to say \"hi\"\n\nIt wasn't even awkward!";
			}

			// Token: 0x02003704 RID: 14084
			public class HUGGED
			{
				// Token: 0x0400E2A5 RID: 58021
				public static LocString NAME = "Hugged";

				// Token: 0x0400E2A6 RID: 58022
				public static LocString TOOLTIP = "This Duplicant recently received a hug from a friendly critter\n\nIt was so fluffy!";
			}

			// Token: 0x02003705 RID: 14085
			public class ARCADEPLAYING
			{
				// Token: 0x0400E2A7 RID: 58023
				public static LocString NAME = "Gaming";

				// Token: 0x0400E2A8 RID: 58024
				public static LocString TOOLTIP = "This Duplicant is playing a video game\n\nIt looks like fun!";
			}

			// Token: 0x02003706 RID: 14086
			public class PLAYEDARCADE
			{
				// Token: 0x0400E2A9 RID: 58025
				public static LocString NAME = "Played Video Games";

				// Token: 0x0400E2AA RID: 58026
				public static LocString TOOLTIP = "This Duplicant recently played video games and is feeling like a champ";
			}

			// Token: 0x02003707 RID: 14087
			public class DANCING
			{
				// Token: 0x0400E2AB RID: 58027
				public static LocString NAME = "Dancing";

				// Token: 0x0400E2AC RID: 58028
				public static LocString TOOLTIP = "This Duplicant is showing off their best moves.";
			}

			// Token: 0x02003708 RID: 14088
			public class DANCED
			{
				// Token: 0x0400E2AD RID: 58029
				public static LocString NAME = "Recently Danced";

				// Token: 0x0400E2AE RID: 58030
				public static LocString TOOLTIP = "This Duplicant had a chance to cut loose!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003709 RID: 14089
			public class JUICER
			{
				// Token: 0x0400E2AF RID: 58031
				public static LocString NAME = "Drank Juice";

				// Token: 0x0400E2B0 RID: 58032
				public static LocString TOOLTIP = "This Duplicant had delicious fruity drink!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200370A RID: 14090
			public class ESPRESSO
			{
				// Token: 0x0400E2B1 RID: 58033
				public static LocString NAME = "Drank Espresso";

				// Token: 0x0400E2B2 RID: 58034
				public static LocString TOOLTIP = "This Duplicant had delicious drink!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200370B RID: 14091
			public class MECHANICALSURFBOARD
			{
				// Token: 0x0400E2B3 RID: 58035
				public static LocString NAME = "Stoked";

				// Token: 0x0400E2B4 RID: 58036
				public static LocString TOOLTIP = "This Duplicant had a rad experience on a surfboard.\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200370C RID: 14092
			public class MECHANICALSURFING
			{
				// Token: 0x0400E2B5 RID: 58037
				public static LocString NAME = "Surfin'";

				// Token: 0x0400E2B6 RID: 58038
				public static LocString TOOLTIP = "This Duplicant is surfin' some artificial waves!";
			}

			// Token: 0x0200370D RID: 14093
			public class SAUNA
			{
				// Token: 0x0400E2B7 RID: 58039
				public static LocString NAME = "Steam Powered";

				// Token: 0x0400E2B8 RID: 58040
				public static LocString TOOLTIP = "This Duplicant just had a relaxing time in a sauna\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200370E RID: 14094
			public class SAUNARELAXING
			{
				// Token: 0x0400E2B9 RID: 58041
				public static LocString NAME = "Relaxing";

				// Token: 0x0400E2BA RID: 58042
				public static LocString TOOLTIP = "This Duplicant is relaxing in a sauna";
			}

			// Token: 0x0200370F RID: 14095
			public class HOTTUB
			{
				// Token: 0x0400E2BB RID: 58043
				public static LocString NAME = "Hot Tubbed";

				// Token: 0x0400E2BC RID: 58044
				public static LocString TOOLTIP = "This Duplicant recently unwound in a Hot Tub\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003710 RID: 14096
			public class HOTTUBRELAXING
			{
				// Token: 0x0400E2BD RID: 58045
				public static LocString NAME = "Relaxing";

				// Token: 0x0400E2BE RID: 58046
				public static LocString TOOLTIP = "This Duplicant is unwinding in a hot tub\n\nThey sure look relaxed";
			}

			// Token: 0x02003711 RID: 14097
			public class SODAFOUNTAIN
			{
				// Token: 0x0400E2BF RID: 58047
				public static LocString NAME = "Soda Filled";

				// Token: 0x0400E2C0 RID: 58048
				public static LocString TOOLTIP = "This Duplicant just enjoyed a bubbly beverage\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003712 RID: 14098
			public class VERTICALWINDTUNNELFLYING
			{
				// Token: 0x0400E2C1 RID: 58049
				public static LocString NAME = "Airborne";

				// Token: 0x0400E2C2 RID: 58050
				public static LocString TOOLTIP = "This Duplicant is having an exhilarating time in the wind tunnel\n\nWhoosh!";
			}

			// Token: 0x02003713 RID: 14099
			public class VERTICALWINDTUNNEL
			{
				// Token: 0x0400E2C3 RID: 58051
				public static LocString NAME = "Wind Swept";

				// Token: 0x0400E2C4 RID: 58052
				public static LocString TOOLTIP = "This Duplicant recently had an exhilarating wind tunnel experience\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003714 RID: 14100
			public class BEACHCHAIRRELAXING
			{
				// Token: 0x0400E2C5 RID: 58053
				public static LocString NAME = "Totally Chill";

				// Token: 0x0400E2C6 RID: 58054
				public static LocString TOOLTIP = "This Duplicant is totally chillin' in a beach chair";
			}

			// Token: 0x02003715 RID: 14101
			public class BEACHCHAIRLIT
			{
				// Token: 0x0400E2C7 RID: 58055
				public static LocString NAME = "Sun Kissed";

				// Token: 0x0400E2C8 RID: 58056
				public static LocString TOOLTIP = "This Duplicant had an amazing experience at the Beach\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003716 RID: 14102
			public class BEACHCHAIRUNLIT
			{
				// Token: 0x0400E2C9 RID: 58057
				public static LocString NAME = "Passably Relaxed";

				// Token: 0x0400E2CA RID: 58058
				public static LocString TOOLTIP = "This Duplicant just had a mediocre beach experience\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003717 RID: 14103
			public class TELEPHONECHAT
			{
				// Token: 0x0400E2CB RID: 58059
				public static LocString NAME = "Full of Gossip";

				// Token: 0x0400E2CC RID: 58060
				public static LocString TOOLTIP = "This Duplicant chatted on the phone with at least one other Duplicant\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003718 RID: 14104
			public class TELEPHONEBABBLE
			{
				// Token: 0x0400E2CD RID: 58061
				public static LocString NAME = "Less Anxious";

				// Token: 0x0400E2CE RID: 58062
				public static LocString TOOLTIP = "This Duplicant got some things off their chest by talking to themselves on the phone\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003719 RID: 14105
			public class TELEPHONELONGDISTANCE
			{
				// Token: 0x0400E2CF RID: 58063
				public static LocString NAME = "Sociable";

				// Token: 0x0400E2D0 RID: 58064
				public static LocString TOOLTIP = "This Duplicant is feeling sociable after chatting on the phone with someone across space\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200371A RID: 14106
			public class EDIBLEMINUS3
			{
				// Token: 0x0400E2D1 RID: 58065
				public static LocString NAME = "Grisly Meal";

				// Token: 0x0400E2D2 RID: 58066
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Grisly",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be better"
				});
			}

			// Token: 0x0200371B RID: 14107
			public class EDIBLEMINUS2
			{
				// Token: 0x0400E2D3 RID: 58067
				public static LocString NAME = "Terrible Meal";

				// Token: 0x0400E2D4 RID: 58068
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Terrible",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be better"
				});
			}

			// Token: 0x0200371C RID: 14108
			public class EDIBLEMINUS1
			{
				// Token: 0x0400E2D5 RID: 58069
				public static LocString NAME = "Poor Meal";

				// Token: 0x0400E2D6 RID: 58070
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Poor",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be a little better"
				});
			}

			// Token: 0x0200371D RID: 14109
			public class EDIBLE0
			{
				// Token: 0x0400E2D7 RID: 58071
				public static LocString NAME = "Standard Meal";

				// Token: 0x0400E2D8 RID: 58072
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Average",
					UI.PST_KEYWORD,
					"\n\nThey thought it was sort of okay"
				});
			}

			// Token: 0x0200371E RID: 14110
			public class EDIBLE1
			{
				// Token: 0x0400E2D9 RID: 58073
				public static LocString NAME = "Good Meal";

				// Token: 0x0400E2DA RID: 58074
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Good",
					UI.PST_KEYWORD,
					"\n\nThey thought it was pretty good!"
				});
			}

			// Token: 0x0200371F RID: 14111
			public class EDIBLE2
			{
				// Token: 0x0400E2DB RID: 58075
				public static LocString NAME = "Great Meal";

				// Token: 0x0400E2DC RID: 58076
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Great",
					UI.PST_KEYWORD,
					"\n\nThey thought it was pretty good!"
				});
			}

			// Token: 0x02003720 RID: 14112
			public class EDIBLE3
			{
				// Token: 0x0400E2DD RID: 58077
				public static LocString NAME = "Superb Meal";

				// Token: 0x0400E2DE RID: 58078
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Superb",
					UI.PST_KEYWORD,
					"\n\nThey thought it was really good!"
				});
			}

			// Token: 0x02003721 RID: 14113
			public class EDIBLE4
			{
				// Token: 0x0400E2DF RID: 58079
				public static LocString NAME = "Ambrosial Meal";

				// Token: 0x0400E2E0 RID: 58080
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Ambrosial",
					UI.PST_KEYWORD,
					"\n\nThey thought it was super tasty!"
				});
			}

			// Token: 0x02003722 RID: 14114
			public class DECORMINUS1
			{
				// Token: 0x0400E2E1 RID: 58081
				public static LocString NAME = "Last Cycle's Decor: Ugly";

				// Token: 0x0400E2E2 RID: 58082
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was downright depressing"
				});
			}

			// Token: 0x02003723 RID: 14115
			public class DECOR0
			{
				// Token: 0x0400E2E3 RID: 58083
				public static LocString NAME = "Last Cycle's Decor: Poor";

				// Token: 0x0400E2E4 RID: 58084
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was quite poor"
				});
			}

			// Token: 0x02003724 RID: 14116
			public class DECOR1
			{
				// Token: 0x0400E2E5 RID: 58085
				public static LocString NAME = "Last Cycle's Decor: Mediocre";

				// Token: 0x0400E2E6 RID: 58086
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant had no strong opinions about the colony's ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday"
				});
			}

			// Token: 0x02003725 RID: 14117
			public class DECOR2
			{
				// Token: 0x0400E2E7 RID: 58087
				public static LocString NAME = "Last Cycle's Decor: Average";

				// Token: 0x0400E2E8 RID: 58088
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was pretty alright"
				});
			}

			// Token: 0x02003726 RID: 14118
			public class DECOR3
			{
				// Token: 0x0400E2E9 RID: 58089
				public static LocString NAME = "Last Cycle's Decor: Nice";

				// Token: 0x0400E2EA RID: 58090
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was quite nice!"
				});
			}

			// Token: 0x02003727 RID: 14119
			public class DECOR4
			{
				// Token: 0x0400E2EB RID: 58091
				public static LocString NAME = "Last Cycle's Decor: Charming";

				// Token: 0x0400E2EC RID: 58092
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was downright charming!"
				});
			}

			// Token: 0x02003728 RID: 14120
			public class DECOR5
			{
				// Token: 0x0400E2ED RID: 58093
				public static LocString NAME = "Last Cycle's Decor: Gorgeous";

				// Token: 0x0400E2EE RID: 58094
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was fantastic\n\nThey love what I've done with the place!"
				});
			}

			// Token: 0x02003729 RID: 14121
			public class BREAKX
			{
				// Token: 0x0400E2EF RID: 58095
				public static LocString NAME = "{0} Shift Break";
			}

			// Token: 0x0200372A RID: 14122
			public class BREAKX_BIONIC
			{
				// Token: 0x0400E2F0 RID: 58096
				public static LocString NAME = "{0} Shift Break (Bionic)";
			}

			// Token: 0x0200372B RID: 14123
			public class POWERTINKER
			{
				// Token: 0x0400E2F1 RID: 58097
				public static LocString NAME = "Engie's Tune-Up";

				// Token: 0x0400E2F2 RID: 58098
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has improved this generator's ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" output efficiency\n\nApplying this effect consumed one ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200372C RID: 14124
			public class FARMTINKER
			{
				// Token: 0x0400E2F3 RID: 58099
				public static LocString NAME = "Farmer's Touch";

				// Token: 0x0400E2F4 RID: 58100
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has encouraged this ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					" to grow a little bit faster\n\nApplying this effect consumed one dose of ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME,
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200372D RID: 14125
			public class MACHINETINKER
			{
				// Token: 0x0400E2F5 RID: 58101
				public static LocString NAME = "Engie's Jerry Rig";

				// Token: 0x0400E2F6 RID: 58102
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has jerry rigged this ",
					UI.PRE_KEYWORD,
					"Generator",
					UI.PST_KEYWORD,
					" to temporarily run faster"
				});
			}

			// Token: 0x0200372E RID: 14126
			public class SPACETOURIST
			{
				// Token: 0x0400E2F7 RID: 58103
				public static LocString NAME = "Visited Space";

				// Token: 0x0400E2F8 RID: 58104
				public static LocString TOOLTIP = "This Duplicant went on a trip to space and saw the wonders of the universe";
			}

			// Token: 0x0200372F RID: 14127
			public class SUDDENMORALEHELPER
			{
				// Token: 0x0400E2F9 RID: 58105
				public static LocString NAME = "Morale Upgrade Helper";

				// Token: 0x0400E2FA RID: 58106
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant will receive a temporary ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" bonus to buffer the new ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" system introduction"
				});
			}

			// Token: 0x02003730 RID: 14128
			public class EXPOSEDTOFOODGERMS
			{
				// Token: 0x0400E2FB RID: 58107
				public static LocString NAME = "Food Poisoning Exposure";

				// Token: 0x0400E2FC RID: 58108
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.FOODPOISONING.NAME,
					" Germs and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003731 RID: 14129
			public class EXPOSEDTOSLIMEGERMS
			{
				// Token: 0x0400E2FD RID: 58109
				public static LocString NAME = "Slimelung Exposure";

				// Token: 0x0400E2FE RID: 58110
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.SLIMELUNG.NAME,
					" and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003732 RID: 14130
			public class EXPOSEDTOZOMBIESPORES
			{
				// Token: 0x0400E2FF RID: 58111
				public static LocString NAME = "Zombie Spores Exposure";

				// Token: 0x0400E300 RID: 58112
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.ZOMBIESPORES.NAME,
					" and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003733 RID: 14131
			public class FEELINGSICKFOODGERMS
			{
				// Token: 0x0400E301 RID: 58113
				public static LocString NAME = "Contracted: Food Poisoning";

				// Token: 0x0400E302 RID: 58114
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.FOODSICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02003734 RID: 14132
			public class FEELINGSICKSLIMEGERMS
			{
				// Token: 0x0400E303 RID: 58115
				public static LocString NAME = "Contracted: Slimelung";

				// Token: 0x0400E304 RID: 58116
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02003735 RID: 14133
			public class FEELINGSICKZOMBIESPORES
			{
				// Token: 0x0400E305 RID: 58117
				public static LocString NAME = "Contracted: Zombie Spores";

				// Token: 0x0400E306 RID: 58118
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02003736 RID: 14134
			public class SMELLEDFLOWERS
			{
				// Token: 0x0400E307 RID: 58119
				public static LocString NAME = "Smelled Flowers";

				// Token: 0x0400E308 RID: 58120
				public static LocString TOOLTIP = "A pleasant " + DUPLICANTS.DISEASES.POLLENGERMS.NAME + " wafted over this Duplicant and brightened their day";
			}

			// Token: 0x02003737 RID: 14135
			public class HISTAMINESUPPRESSION
			{
				// Token: 0x0400E309 RID: 58121
				public static LocString NAME = "Antihistamines";

				// Token: 0x0400E30A RID: 58122
				public static LocString TOOLTIP = "This Duplicant's allergic reactions have been suppressed by medication";
			}

			// Token: 0x02003738 RID: 14136
			public class FOODSICKNESSRECOVERY
			{
				// Token: 0x0400E30B RID: 58123
				public static LocString NAME = "Food Poisoning Antibodies";

				// Token: 0x0400E30C RID: 58124
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.FOODSICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003739 RID: 14137
			public class SLIMESICKNESSRECOVERY
			{
				// Token: 0x0400E30D RID: 58125
				public static LocString NAME = "Slimelung Antibodies";

				// Token: 0x0400E30E RID: 58126
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200373A RID: 14138
			public class ZOMBIESICKNESSRECOVERY
			{
				// Token: 0x0400E30F RID: 58127
				public static LocString NAME = "Zombie Spores Antibodies";

				// Token: 0x0400E310 RID: 58128
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200373B RID: 14139
			public class MESSTABLESALT
			{
				// Token: 0x0400E311 RID: 58129
				public static LocString NAME = "Salted Food";

				// Token: 0x0400E312 RID: 58130
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant had the luxury of using ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME,
					UI.PST_KEYWORD,
					" with their last meal at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME
				});
			}

			// Token: 0x0200373C RID: 14140
			public class COMMUNALDINING
			{
				// Token: 0x0400E313 RID: 58131
				public static LocString NAME = "Communal Dining";

				// Token: 0x0400E314 RID: 58132
				public static LocString TOOLTIP = "This Duplicant recently had the pleasure of dining with friends at a " + BUILDINGS.PREFABS.MULTIMINIONDININGTABLE.NAME;
			}

			// Token: 0x0200373D RID: 14141
			public class RADIATIONEXPOSUREMINOR
			{
				// Token: 0x0400E315 RID: 58133
				public static LocString NAME = "Minor Radiation Sickness";

				// Token: 0x0400E316 RID: 58134
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A bit of ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has made this Duplicant feel sluggish"
				});
			}

			// Token: 0x0200373E RID: 14142
			public class RADIATIONEXPOSUREMAJOR
			{
				// Token: 0x0400E317 RID: 58135
				public static LocString NAME = "Major Radiation Sickness";

				// Token: 0x0400E318 RID: 58136
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Significant ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has left this Duplicant totally exhausted"
				});
			}

			// Token: 0x0200373F RID: 14143
			public class RADIATIONEXPOSUREEXTREME
			{
				// Token: 0x0400E319 RID: 58137
				public static LocString NAME = "Extreme Radiation Sickness";

				// Token: 0x0400E31A RID: 58138
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Dangerously high ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure is making this Duplicant wish they'd never been printed"
				});
			}

			// Token: 0x02003740 RID: 14144
			public class RADIATIONEXPOSUREDEADLY
			{
				// Token: 0x0400E31B RID: 58139
				public static LocString NAME = "Deadly Radiation Sickness";

				// Token: 0x0400E31C RID: 58140
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Extreme ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has incapacitated this Duplicant"
				});
			}

			// Token: 0x02003741 RID: 14145
			public class BIONICRADIATIONEXPOSUREMINOR
			{
				// Token: 0x0400E31D RID: 58141
				public static LocString NAME = "Minor Radiation Sickness";

				// Token: 0x0400E31E RID: 58142
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A bit of ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has made this Duplicant feel sluggish"
				});
			}

			// Token: 0x02003742 RID: 14146
			public class BIONICRADIATIONEXPOSUREMAJOR
			{
				// Token: 0x0400E31F RID: 58143
				public static LocString NAME = "Major Radiation Sickness";

				// Token: 0x0400E320 RID: 58144
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Significant ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has left this Duplicant totally exhausted"
				});
			}

			// Token: 0x02003743 RID: 14147
			public class BIONICRADIATIONEXPOSUREEXTREME
			{
				// Token: 0x0400E321 RID: 58145
				public static LocString NAME = "Extreme Radiation Sickness";

				// Token: 0x0400E322 RID: 58146
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Dangerously high ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure is making this Duplicant wish they'd never been printed"
				});
			}

			// Token: 0x02003744 RID: 14148
			public class BIONICRADIATIONEXPOSUREDEADLY
			{
				// Token: 0x0400E323 RID: 58147
				public static LocString NAME = "Deadly Radiation Sickness";

				// Token: 0x0400E324 RID: 58148
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Extreme ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has incapacitated this Duplicant"
				});
			}

			// Token: 0x02003745 RID: 14149
			public class CHARGING
			{
				// Token: 0x0400E325 RID: 58149
				public static LocString NAME = "Charging";

				// Token: 0x0400E326 RID: 58150
				public static LocString TOOLTIP = "This lil bot is charging its internal battery";
			}

			// Token: 0x02003746 RID: 14150
			public class BOTSWEEPING
			{
				// Token: 0x0400E327 RID: 58151
				public static LocString NAME = "Sweeping";

				// Token: 0x0400E328 RID: 58152
				public static LocString TOOLTIP = "This lil bot is picking up debris from the floor";
			}

			// Token: 0x02003747 RID: 14151
			public class BOTMOPPING
			{
				// Token: 0x0400E329 RID: 58153
				public static LocString NAME = "Mopping";

				// Token: 0x0400E32A RID: 58154
				public static LocString TOOLTIP = "This lil bot is clearing liquids from the ground";
			}

			// Token: 0x02003748 RID: 14152
			public class SCOUTBOTCHARGING
			{
				// Token: 0x0400E32B RID: 58155
				public static LocString NAME = "Charging";

				// Token: 0x0400E32C RID: 58156
				public static LocString TOOLTIP = ROBOTS.MODELS.SCOUT.NAME + " is happily charging inside " + BUILDINGS.PREFABS.SCOUTMODULE.NAME;
			}

			// Token: 0x02003749 RID: 14153
			public class CRYOFRIEND
			{
				// Token: 0x0400E32D RID: 58157
				public static LocString NAME = "Motivated By Friend";

				// Token: 0x0400E32E RID: 58158
				public static LocString TOOLTIP = "This Duplicant feels motivated after meeting a long lost friend";
			}

			// Token: 0x0200374A RID: 14154
			public class BONUSDREAM1
			{
				// Token: 0x0400E32F RID: 58159
				public static LocString NAME = "Good Dream";

				// Token: 0x0400E330 RID: 58160
				public static LocString TOOLTIP = "This Duplicant had a good dream and is feeling psyched!";
			}

			// Token: 0x0200374B RID: 14155
			public class BONUSDREAM2
			{
				// Token: 0x0400E331 RID: 58161
				public static LocString NAME = "Really Good Dream";

				// Token: 0x0400E332 RID: 58162
				public static LocString TOOLTIP = "This Duplicant had a really good dream and is full of possibilities!";
			}

			// Token: 0x0200374C RID: 14156
			public class BONUSDREAM3
			{
				// Token: 0x0400E333 RID: 58163
				public static LocString NAME = "Great Dream";

				// Token: 0x0400E334 RID: 58164
				public static LocString TOOLTIP = "This Duplicant had a great dream last night and periodically remembers another great moment they previously forgot";
			}

			// Token: 0x0200374D RID: 14157
			public class BONUSDREAM4
			{
				// Token: 0x0400E335 RID: 58165
				public static LocString NAME = "Dream Inspired";

				// Token: 0x0400E336 RID: 58166
				public static LocString TOOLTIP = "This Duplicant is inspired from all the unforgettable dreams they had";
			}

			// Token: 0x0200374E RID: 14158
			public class BONUSRESEARCH
			{
				// Token: 0x0400E337 RID: 58167
				public static LocString NAME = "Inspired Learner";

				// Token: 0x0400E338 RID: 58168
				public static LocString TOOLTIP = "This Duplicant is looking forward to some learning";
			}

			// Token: 0x0200374F RID: 14159
			public class BONUSTOILET1
			{
				// Token: 0x0400E339 RID: 58169
				public static LocString NAME = "Small Comforts";

				// Token: 0x0400E33A RID: 58170
				public static LocString TOOLTIP = "This Duplicant visited the {building} and appreciated the small comforts";
			}

			// Token: 0x02003750 RID: 14160
			public class BONUSTOILET2
			{
				// Token: 0x0400E33B RID: 58171
				public static LocString NAME = "Greater Comforts";

				// Token: 0x0400E33C RID: 58172
				public static LocString TOOLTIP = "This Duplicant used a " + BUILDINGS.PREFABS.OUTHOUSE.NAME + "and liked how comfortable it felt";
			}

			// Token: 0x02003751 RID: 14161
			public class BONUSTOILET3
			{
				// Token: 0x0400E33D RID: 58173
				public static LocString NAME = "Small Luxury";

				// Token: 0x0400E33E RID: 58174
				public static LocString TOOLTIP = "This Duplicant visited a " + ROOMS.TYPES.LATRINE.NAME + " and feels they could get used to this luxury";
			}

			// Token: 0x02003752 RID: 14162
			public class BONUSTOILET4
			{
				// Token: 0x0400E33F RID: 58175
				public static LocString NAME = "Luxurious";

				// Token: 0x0400E340 RID: 58176
				public static LocString TOOLTIP = "This Duplicant feels endless luxury from the " + ROOMS.TYPES.PRIVATE_BATHROOM.NAME;
			}

			// Token: 0x02003753 RID: 14163
			public class BONUSDIGGING1
			{
				// Token: 0x0400E341 RID: 58177
				public static LocString NAME = "Hot Diggity!";

				// Token: 0x0400E342 RID: 58178
				public static LocString TOOLTIP = "This Duplicant did a lot of excavating and is really digging digging";
			}

			// Token: 0x02003754 RID: 14164
			public class BONUSSTORAGE
			{
				// Token: 0x0400E343 RID: 58179
				public static LocString NAME = "Something in Store";

				// Token: 0x0400E344 RID: 58180
				public static LocString TOOLTIP = "This Duplicant stored something in a " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " and is feeling organized";
			}

			// Token: 0x02003755 RID: 14165
			public class BONUSBUILDER
			{
				// Token: 0x0400E345 RID: 58181
				public static LocString NAME = "Accomplished Builder";

				// Token: 0x0400E346 RID: 58182
				public static LocString TOOLTIP = "This Duplicant has built many buildings and has a sense of accomplishment!";
			}

			// Token: 0x02003756 RID: 14166
			public class BONUSOXYGEN
			{
				// Token: 0x0400E347 RID: 58183
				public static LocString NAME = "Fresh Air";

				// Token: 0x0400E348 RID: 58184
				public static LocString TOOLTIP = "This Duplicant breathed in some fresh air and is feeling refreshed";
			}

			// Token: 0x02003757 RID: 14167
			public class BONUSGENERATOR
			{
				// Token: 0x0400E349 RID: 58185
				public static LocString NAME = "Exercised";

				// Token: 0x0400E34A RID: 58186
				public static LocString TOOLTIP = "This Duplicant ran in a Generator and has benefited from the exercise";
			}

			// Token: 0x02003758 RID: 14168
			public class BONUSDOOR
			{
				// Token: 0x0400E34B RID: 58187
				public static LocString NAME = "Open and Shut";

				// Token: 0x0400E34C RID: 58188
				public static LocString TOOLTIP = "This Duplicant closed a door and appreciates the privacy";
			}

			// Token: 0x02003759 RID: 14169
			public class BONUSHITTHEBOOKS
			{
				// Token: 0x0400E34D RID: 58189
				public static LocString NAME = "Hit the Books";

				// Token: 0x0400E34E RID: 58190
				public static LocString TOOLTIP = "This Duplicant did some research and is feeling smarter";
			}

			// Token: 0x0200375A RID: 14170
			public class BONUSLITWORKSPACE
			{
				// Token: 0x0400E34F RID: 58191
				public static LocString NAME = "Lit";

				// Token: 0x0400E350 RID: 58192
				public static LocString TOOLTIP = "This Duplicant was in a well-lit environment and is feeling lit";
			}

			// Token: 0x0200375B RID: 14171
			public class BONUSTALKER
			{
				// Token: 0x0400E351 RID: 58193
				public static LocString NAME = "Talker";

				// Token: 0x0400E352 RID: 58194
				public static LocString TOOLTIP = "This Duplicant engaged in small talk with a coworker and is feeling connected";
			}

			// Token: 0x0200375C RID: 14172
			public class THRIVER
			{
				// Token: 0x0400E353 RID: 58195
				public static LocString NAME = "Clutchy";

				// Token: 0x0400E354 RID: 58196
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" and has kicked into hyperdrive"
				});
			}

			// Token: 0x0200375D RID: 14173
			public class LONER
			{
				// Token: 0x0400E355 RID: 58197
				public static LocString NAME = "Alone";

				// Token: 0x0400E356 RID: 58198
				public static LocString TOOLTIP = "This Duplicant is feeling more focused now that they're alone";
			}

			// Token: 0x0200375E RID: 14174
			public class STARRYEYED
			{
				// Token: 0x0400E357 RID: 58199
				public static LocString NAME = "Starry Eyed";

				// Token: 0x0400E358 RID: 58200
				public static LocString TOOLTIP = "This Duplicant loves being in space!";
			}

			// Token: 0x0200375F RID: 14175
			public class WAILEDAT
			{
				// Token: 0x0400E359 RID: 58201
				public static LocString NAME = "Disturbed by Wailing";

				// Token: 0x0400E35A RID: 58202
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is feeling ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" by someone's Banshee Wail"
				});
			}

			// Token: 0x02003760 RID: 14176
			public class SPACEBUZZ
			{
				// Token: 0x0400E35B RID: 58203
				public static LocString NAME = "Thrilling Flight";

				// Token: 0x0400E35C RID: 58204
				public static LocString TOOLTIP = "This Duplicant is getting a real adrenaline rush from being in space!";
			}
		}

		// Token: 0x02002586 RID: 9606
		public class CONGENITALTRAITS
		{
			// Token: 0x02003761 RID: 14177
			public class NONE
			{
				// Token: 0x0400E35D RID: 58205
				public static LocString NAME = "None";

				// Token: 0x0400E35E RID: 58206
				public static LocString DESC = "This Duplicant seems pretty average overall";
			}

			// Token: 0x02003762 RID: 14178
			public class JOSHUA
			{
				// Token: 0x0400E35F RID: 58207
				public static LocString NAME = "Cheery Disposition";

				// Token: 0x0400E360 RID: 58208
				public static LocString DESC = "This Duplicant brightens others' days wherever he goes";
			}

			// Token: 0x02003763 RID: 14179
			public class ELLIE
			{
				// Token: 0x0400E361 RID: 58209
				public static LocString NAME = "Fastidious";

				// Token: 0x0400E362 RID: 58210
				public static LocString DESC = "This Duplicant needs things done in a very particular way";
			}

			// Token: 0x02003764 RID: 14180
			public class LIAM
			{
				// Token: 0x0400E363 RID: 58211
				public static LocString NAME = "Germaphobe";

				// Token: 0x0400E364 RID: 58212
				public static LocString DESC = "This Duplicant has an all-consuming fear of bacteria";
			}

			// Token: 0x02003765 RID: 14181
			public class BANHI
			{
				// Token: 0x0400E365 RID: 58213
				public static LocString NAME = "";

				// Token: 0x0400E366 RID: 58214
				public static LocString DESC = "";
			}

			// Token: 0x02003766 RID: 14182
			public class STINKY
			{
				// Token: 0x0400E367 RID: 58215
				public static LocString NAME = "Stinkiness";

				// Token: 0x0400E368 RID: 58216
				public static LocString DESC = "This Duplicant is genetically cursed by a pungent bodily odor";
			}
		}

		// Token: 0x02002587 RID: 9607
		public class TRAITS
		{
			// Token: 0x0400A9A3 RID: 43427
			public static LocString TRAIT_DESCRIPTION_LIST_ENTRY = "\n• ";

			// Token: 0x0400A9A4 RID: 43428
			public static LocString ATTRIBUTE_MODIFIERS = "{0}: {1}";

			// Token: 0x0400A9A5 RID: 43429
			public static LocString CANNOT_DO_TASK = "Cannot do <b>{0} Errands</b>";

			// Token: 0x0400A9A6 RID: 43430
			public static LocString CANNOT_DO_TASK_TOOLTIP = "{0}: {1}";

			// Token: 0x0400A9A7 RID: 43431
			public static LocString REFUSES_TO_DO_TASK = "Cannot do <b>{0} Errands</b>";

			// Token: 0x0400A9A8 RID: 43432
			public static LocString IGNORED_EFFECTS = "Immune to <b>{0}</b>";

			// Token: 0x0400A9A9 RID: 43433
			public static LocString IGNORED_EFFECTS_TOOLTIP = "{0}: {1}";

			// Token: 0x0400A9AA RID: 43434
			public static LocString STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP = string.Concat(new string[]
			{
				"Bionic Duplicants use boosters to increase their skills and attributes\n\nBoosters can be crafted at the ",
				BUILDINGS.PREFABS.CRAFTINGTABLE.NAME,
				" and ",
				BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME,
				"\n\nPreinstalled booster effects:"
			});

			// Token: 0x0400A9AB RID: 43435
			public static LocString GRANTED_SKILL_SHARED_NAME = "Skilled: ";

			// Token: 0x0400A9AC RID: 43436
			public static LocString GRANTED_SKILL_SHARED_DESC = string.Concat(new string[]
			{
				"This Duplicant begins with a pre-learned ",
				UI.FormatAsKeyWord("Skill"),
				", but does not have increased ",
				UI.FormatAsKeyWord(DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME),
				".\n\n{0}\n{1}"
			});

			// Token: 0x0400A9AD RID: 43437
			public static LocString GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP = "This Duplicant receives a free " + UI.FormatAsKeyWord("Skill") + " without the drawback of increased " + UI.FormatAsKeyWord(DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME);

			// Token: 0x02003767 RID: 14183
			public class CHATTY
			{
				// Token: 0x0400E369 RID: 58217
				public static LocString NAME = "Charismatic";

				// Token: 0x0400E36A RID: 58218
				public static LocString DESC = string.Concat(new string[]
				{
					"This Duplicant's so charming, chatting with them is sometimes enough to trigger an ",
					UI.PRE_KEYWORD,
					"Overjoyed",
					UI.PST_KEYWORD,
					" response"
				});
			}

			// Token: 0x02003768 RID: 14184
			public class NEEDS
			{
				// Token: 0x02003D8F RID: 15759
				public class CLAUSTROPHOBIC
				{
					// Token: 0x0400F2FE RID: 62206
					public static LocString NAME = "Claustrophobic";

					// Token: 0x0400F2FF RID: 62207
					public static LocString DESC = "This Duplicant feels suffocated in spaces fewer than four tiles high or three tiles wide";
				}

				// Token: 0x02003D90 RID: 15760
				public class FASHIONABLE
				{
					// Token: 0x0400F300 RID: 62208
					public static LocString NAME = "Fashionista";

					// Token: 0x0400F301 RID: 62209
					public static LocString DESC = "This Duplicant dies a bit inside when forced to wear unstylish clothing";
				}

				// Token: 0x02003D91 RID: 15761
				public class CLIMACOPHOBIC
				{
					// Token: 0x0400F302 RID: 62210
					public static LocString NAME = "Vertigo Prone";

					// Token: 0x0400F303 RID: 62211
					public static LocString DESC = "Climbing ladders more than four tiles tall makes this Duplicant's stomach do flips";
				}

				// Token: 0x02003D92 RID: 15762
				public class SOLITARYSLEEPER
				{
					// Token: 0x0400F304 RID: 62212
					public static LocString NAME = "Solitary Sleeper";

					// Token: 0x0400F305 RID: 62213
					public static LocString DESC = "This Duplicant prefers to sleep alone";
				}

				// Token: 0x02003D93 RID: 15763
				public class PREFERSWARMER
				{
					// Token: 0x0400F306 RID: 62214
					public static LocString NAME = "Skinny";

					// Token: 0x0400F307 RID: 62215
					public static LocString DESC = string.Concat(new string[]
					{
						"This Duplicant doesn't have much ",
						UI.PRE_KEYWORD,
						"Insulation",
						UI.PST_KEYWORD,
						", so they are more ",
						UI.PRE_KEYWORD,
						"Temperature",
						UI.PST_KEYWORD,
						" sensitive than others"
					});
				}

				// Token: 0x02003D94 RID: 15764
				public class PREFERSCOOLER
				{
					// Token: 0x0400F308 RID: 62216
					public static LocString NAME = "Pudgy";

					// Token: 0x0400F309 RID: 62217
					public static LocString DESC = string.Concat(new string[]
					{
						"This Duplicant has some extra ",
						UI.PRE_KEYWORD,
						"Insulation",
						UI.PST_KEYWORD,
						", so the room ",
						UI.PRE_KEYWORD,
						"Temperature",
						UI.PST_KEYWORD,
						" affects them a little less"
					});
				}

				// Token: 0x02003D95 RID: 15765
				public class SENSITIVEFEET
				{
					// Token: 0x0400F30A RID: 62218
					public static LocString NAME = "Delicate Feetsies";

					// Token: 0x0400F30B RID: 62219
					public static LocString DESC = "This Duplicant is a sensitive sole and would rather walk on tile than raw bedrock";
				}

				// Token: 0x02003D96 RID: 15766
				public class WORKAHOLIC
				{
					// Token: 0x0400F30C RID: 62220
					public static LocString NAME = "Workaholic";

					// Token: 0x0400F30D RID: 62221
					public static LocString DESC = "This Duplicant gets antsy when left idle";
				}
			}

			// Token: 0x02003769 RID: 14185
			public class ANCIENTKNOWLEDGE
			{
				// Token: 0x0400E36B RID: 58219
				public static LocString NAME = "Ancient Knowledge";

				// Token: 0x0400E36C RID: 58220
				public static LocString DESC = "This Duplicant has knowledge from the before times\n• Starts with 3 skill points";
			}

			// Token: 0x0200376A RID: 14186
			public class CANTRESEARCH
			{
				// Token: 0x0400E36D RID: 58221
				public static LocString NAME = "Yokel";

				// Token: 0x0400E36E RID: 58222
				public static LocString DESC = "This Duplicant isn't the brightest star in the solar system";
			}

			// Token: 0x0200376B RID: 14187
			public class CANTBUILD
			{
				// Token: 0x0400E36F RID: 58223
				public static LocString NAME = "Unconstructive";

				// Token: 0x0400E370 RID: 58224
				public static LocString DESC = "This Duplicant is incapable of building even the most basic of structures";
			}

			// Token: 0x0200376C RID: 14188
			public class CANTCOOK
			{
				// Token: 0x0400E371 RID: 58225
				public static LocString NAME = "Gastrophobia";

				// Token: 0x0400E372 RID: 58226
				public static LocString DESC = "This Duplicant has a deep-seated distrust of the culinary arts";
			}

			// Token: 0x0200376D RID: 14189
			public class CANTDIG
			{
				// Token: 0x0400E373 RID: 58227
				public static LocString NAME = "Trypophobia";

				// Token: 0x0400E374 RID: 58228
				public static LocString DESC = "This Duplicant's fear of holes makes it impossible for them to dig";
			}

			// Token: 0x0200376E RID: 14190
			public class HEMOPHOBIA
			{
				// Token: 0x0400E375 RID: 58229
				public static LocString NAME = "Squeamish";

				// Token: 0x0400E376 RID: 58230
				public static LocString DESC = "This Duplicant is of delicate disposition and cannot tend to the sick";
			}

			// Token: 0x0200376F RID: 14191
			public class BEDSIDEMANNER
			{
				// Token: 0x0400E377 RID: 58231
				public static LocString NAME = "Caregiver";

				// Token: 0x0400E378 RID: 58232
				public static LocString DESC = "This Duplicant has good bedside manner and a healing touch";
			}

			// Token: 0x02003770 RID: 14192
			public class MOUTHBREATHER
			{
				// Token: 0x0400E379 RID: 58233
				public static LocString NAME = "Mouth Breather";

				// Token: 0x0400E37A RID: 58234
				public static LocString DESC = "This Duplicant sucks up way more than their fair share of " + ELEMENTS.OXYGEN.NAME;
			}

			// Token: 0x02003771 RID: 14193
			public class FUSSY
			{
				// Token: 0x0400E37B RID: 58235
				public static LocString NAME = "Fussy";

				// Token: 0x0400E37C RID: 58236
				public static LocString DESC = "Nothing's ever quite good enough for this Duplicant";
			}

			// Token: 0x02003772 RID: 14194
			public class TWINKLETOES
			{
				// Token: 0x0400E37D RID: 58237
				public static LocString NAME = "Twinkletoes";

				// Token: 0x0400E37E RID: 58238
				public static LocString DESC = "This Duplicant is light as a feather on their feet";
			}

			// Token: 0x02003773 RID: 14195
			public class STRONGARM
			{
				// Token: 0x0400E37F RID: 58239
				public static LocString NAME = "Buff";

				// Token: 0x0400E380 RID: 58240
				public static LocString DESC = "This Duplicant has muscles on their muscles";
			}

			// Token: 0x02003774 RID: 14196
			public class NOODLEARMS
			{
				// Token: 0x0400E381 RID: 58241
				public static LocString NAME = "Noodle Arms";

				// Token: 0x0400E382 RID: 58242
				public static LocString DESC = "This Duplicant's arms have all the tensile strength of overcooked linguine";
			}

			// Token: 0x02003775 RID: 14197
			public class AGGRESSIVE
			{
				// Token: 0x0400E383 RID: 58243
				public static LocString NAME = "Destructive";

				// Token: 0x0400E384 RID: 58244
				public static LocString DESC = "This Duplicant handles stress by taking their frustrations out on defenseless machines";

				// Token: 0x0400E385 RID: 58245
				public static LocString NOREPAIR = "• Will not repair buildings while above 60% " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003776 RID: 14198
			public class UGLYCRIER
			{
				// Token: 0x0400E386 RID: 58246
				public static LocString NAME = "Ugly Crier";

				// Token: 0x0400E387 RID: 58247
				public static LocString DESC = string.Concat(new string[]
				{
					"If this Duplicant gets too ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" it won't be pretty"
				});
			}

			// Token: 0x02003777 RID: 14199
			public class BINGEEATER
			{
				// Token: 0x0400E388 RID: 58248
				public static LocString NAME = "Binge Eater";

				// Token: 0x0400E389 RID: 58249
				public static LocString DESC = "This Duplicant will dangerously overeat when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x02003778 RID: 14200
			public class ANXIOUS
			{
				// Token: 0x0400E38A RID: 58250
				public static LocString NAME = "Anxious";

				// Token: 0x0400E38B RID: 58251
				public static LocString DESC = "This Duplicant collapses when put under too much pressure";
			}

			// Token: 0x02003779 RID: 14201
			public class STRESSVOMITER
			{
				// Token: 0x0400E38C RID: 58252
				public static LocString NAME = "Vomiter";

				// Token: 0x0400E38D RID: 58253
				public static LocString DESC = "This Duplicant is liable to puke everywhere when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x0200377A RID: 14202
			public class STRESSSHOCKER
			{
				// Token: 0x0400E38E RID: 58254
				public static LocString NAME = "Stunner";

				// Token: 0x0400E38F RID: 58255
				public static LocString DESC = "This Duplicant emits electrical shocks when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;

				// Token: 0x0400E390 RID: 58256
				public static LocString DRAIN_ATTRIBUTE = "Stress Zapping";
			}

			// Token: 0x0200377B RID: 14203
			public class BANSHEE
			{
				// Token: 0x0400E391 RID: 58257
				public static LocString NAME = "Banshee";

				// Token: 0x0400E392 RID: 58258
				public static LocString DESC = "This Duplicant wails uncontrollably when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x0200377C RID: 14204
			public class BALLOONARTIST
			{
				// Token: 0x0400E393 RID: 58259
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400E394 RID: 58260
				public static LocString DESC = "This Duplicant hands out balloons when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x0200377D RID: 14205
			public class SPARKLESTREAKER
			{
				// Token: 0x0400E395 RID: 58261
				public static LocString NAME = "Sparkle Streaker";

				// Token: 0x0400E396 RID: 58262
				public static LocString DESC = "This Duplicant leaves a trail of happy sparkles when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x0200377E RID: 14206
			public class STICKERBOMBER
			{
				// Token: 0x0400E397 RID: 58263
				public static LocString NAME = "Sticker Bomber";

				// Token: 0x0400E398 RID: 58264
				public static LocString DESC = "This Duplicant will spontaneously redecorate a room when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x0200377F RID: 14207
			public class SUPERPRODUCTIVE
			{
				// Token: 0x0400E399 RID: 58265
				public static LocString NAME = "Super Productive";

				// Token: 0x0400E39A RID: 58266
				public static LocString DESC = "This Duplicant is super productive when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x02003780 RID: 14208
			public class HAPPYSINGER
			{
				// Token: 0x0400E39B RID: 58267
				public static LocString NAME = "Yodeler";

				// Token: 0x0400E39C RID: 58268
				public static LocString DESC = "This Duplicant belts out catchy tunes when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x02003781 RID: 14209
			public class DATARAINER
			{
				// Token: 0x0400E39D RID: 58269
				public static LocString NAME = "Rainmaker";

				// Token: 0x0400E39E RID: 58270
				public static LocString DESC = "This Duplicant distributes microchips when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x02003782 RID: 14210
			public class ROBODANCER
			{
				// Token: 0x0400E39F RID: 58271
				public static LocString NAME = "Flash Mobber";

				// Token: 0x0400E3A0 RID: 58272
				public static LocString DESC = "This Duplicant breaks into dance when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x02003783 RID: 14211
			public class IRONGUT
			{
				// Token: 0x0400E3A1 RID: 58273
				public static LocString NAME = "Iron Gut";

				// Token: 0x0400E3A2 RID: 58274
				public static LocString DESC = "This Duplicant can eat just about anything without getting sick";

				// Token: 0x0400E3A3 RID: 58275
				public static LocString SHORT_DESC = "Immune to <b>" + DUPLICANTS.DISEASES.FOODSICKNESS.NAME + "</b>";

				// Token: 0x0400E3A4 RID: 58276
				public static LocString SHORT_DESC_TOOLTIP = "Eating food contaminated with " + DUPLICANTS.DISEASES.FOODSICKNESS.NAME + " Germs will not affect this Duplicant";
			}

			// Token: 0x02003784 RID: 14212
			public class STRONGIMMUNESYSTEM
			{
				// Token: 0x0400E3A5 RID: 58277
				public static LocString NAME = "Germ Resistant";

				// Token: 0x0400E3A6 RID: 58278
				public static LocString DESC = "This Duplicant's immune system bounces back faster than most";
			}

			// Token: 0x02003785 RID: 14213
			public class SCAREDYCAT
			{
				// Token: 0x0400E3A7 RID: 58279
				public static LocString NAME = "Pacifist";

				// Token: 0x0400E3A8 RID: 58280
				public static LocString DESC = "This Duplicant abhors violence";
			}

			// Token: 0x02003786 RID: 14214
			public class ALLERGIES
			{
				// Token: 0x0400E3A9 RID: 58281
				public static LocString NAME = "Allergies";

				// Token: 0x0400E3AA RID: 58282
				public static LocString DESC = "This Duplicant will sneeze uncontrollably when exposed to the pollen present in " + DUPLICANTS.DISEASES.POLLENGERMS.NAME;

				// Token: 0x0400E3AB RID: 58283
				public static LocString SHORT_DESC = "Allergic reaction to <b>" + DUPLICANTS.DISEASES.POLLENGERMS.NAME + "</b>";

				// Token: 0x0400E3AC RID: 58284
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.DISEASES.ALLERGIES.DESCRIPTIVE_SYMPTOMS;
			}

			// Token: 0x02003787 RID: 14215
			public class WEAKIMMUNESYSTEM
			{
				// Token: 0x0400E3AD RID: 58285
				public static LocString NAME = "Biohazardous";

				// Token: 0x0400E3AE RID: 58286
				public static LocString DESC = "All the vitamin C in space couldn't stop this Duplicant from getting sick";
			}

			// Token: 0x02003788 RID: 14216
			public class IRRITABLEBOWEL
			{
				// Token: 0x0400E3AF RID: 58287
				public static LocString NAME = "Irritable Bowel";

				// Token: 0x0400E3B0 RID: 58288
				public static LocString DESC = "This Duplicant needs a little extra time to \"do their business\"";
			}

			// Token: 0x02003789 RID: 14217
			public class CALORIEBURNER
			{
				// Token: 0x0400E3B1 RID: 58289
				public static LocString NAME = "Bottomless Stomach";

				// Token: 0x0400E3B2 RID: 58290
				public static LocString DESC = "This Duplicant might actually be several black holes in a trench coat";
			}

			// Token: 0x0200378A RID: 14218
			public class SMALLBLADDER
			{
				// Token: 0x0400E3B3 RID: 58291
				public static LocString NAME = "Small Bladder";

				// Token: 0x0400E3B4 RID: 58292
				public static LocString DESC = string.Concat(new string[]
				{
					"This Duplicant has a tiny, pea-sized ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					". Adorable!"
				});
			}

			// Token: 0x0200378B RID: 14219
			public class ANEMIC
			{
				// Token: 0x0400E3B5 RID: 58293
				public static LocString NAME = "Anemic";

				// Token: 0x0400E3B6 RID: 58294
				public static LocString DESC = "This Duplicant has trouble keeping up with the others";
			}

			// Token: 0x0200378C RID: 14220
			public class GREASEMONKEY
			{
				// Token: 0x0400E3B7 RID: 58295
				public static LocString NAME = "Grease Monkey";

				// Token: 0x0400E3B8 RID: 58296
				public static LocString DESC = "This Duplicant likes to throw a wrench into the colony's plans... in a good way";
			}

			// Token: 0x0200378D RID: 14221
			public class MOLEHANDS
			{
				// Token: 0x0400E3B9 RID: 58297
				public static LocString NAME = "Mole Hands";

				// Token: 0x0400E3BA RID: 58298
				public static LocString DESC = "They're great for tunneling, but finding good gloves is a nightmare";
			}

			// Token: 0x0200378E RID: 14222
			public class FASTLEARNER
			{
				// Token: 0x0400E3BB RID: 58299
				public static LocString NAME = "Quick Learner";

				// Token: 0x0400E3BC RID: 58300
				public static LocString DESC = "This Duplicant's sharp as a tack and learns new skills with amazing speed";
			}

			// Token: 0x0200378F RID: 14223
			public class SLOWLEARNER
			{
				// Token: 0x0400E3BD RID: 58301
				public static LocString NAME = "Slow Learner";

				// Token: 0x0400E3BE RID: 58302
				public static LocString DESC = "This Duplicant's a little slow on the uptake, but gosh do they try";
			}

			// Token: 0x02003790 RID: 14224
			public class DIVERSLUNG
			{
				// Token: 0x0400E3BF RID: 58303
				public static LocString NAME = "Diver's Lungs";

				// Token: 0x0400E3C0 RID: 58304
				public static LocString DESC = "This Duplicant could have been a talented opera singer in another life";
			}

			// Token: 0x02003791 RID: 14225
			public class FLATULENCE
			{
				// Token: 0x0400E3C1 RID: 58305
				public static LocString NAME = "Flatulent";

				// Token: 0x0400E3C2 RID: 58306
				public static LocString DESC = "Some Duplicants are just full of it";

				// Token: 0x0400E3C3 RID: 58307
				public static LocString SHORT_DESC = "Farts frequently";

				// Token: 0x0400E3C4 RID: 58308
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant will periodically \"output\" " + ELEMENTS.METHANE.NAME;
			}

			// Token: 0x02003792 RID: 14226
			public class SNORER
			{
				// Token: 0x0400E3C5 RID: 58309
				public static LocString NAME = "Loud Sleeper";

				// Token: 0x0400E3C6 RID: 58310
				public static LocString DESC = "In space, everyone can hear you snore";

				// Token: 0x0400E3C7 RID: 58311
				public static LocString SHORT_DESC = "Snores loudly";

				// Token: 0x0400E3C8 RID: 58312
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant's snoring will rudely awake nearby friends";
			}

			// Token: 0x02003793 RID: 14227
			public class NARCOLEPSY
			{
				// Token: 0x0400E3C9 RID: 58313
				public static LocString NAME = "Narcoleptic";

				// Token: 0x0400E3CA RID: 58314
				public static LocString DESC = "This Duplicant can and will fall asleep anytime, anyplace";

				// Token: 0x0400E3CB RID: 58315
				public static LocString SHORT_DESC = "Falls asleep periodically";

				// Token: 0x0400E3CC RID: 58316
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant's work will be periodically interrupted by naps";
			}

			// Token: 0x02003794 RID: 14228
			public class INTERIORDECORATOR
			{
				// Token: 0x0400E3CD RID: 58317
				public static LocString NAME = "Interior Decorator";

				// Token: 0x0400E3CE RID: 58318
				public static LocString DESC = "\"Move it a little to the left...\"";
			}

			// Token: 0x02003795 RID: 14229
			public class UNCULTURED
			{
				// Token: 0x0400E3CF RID: 58319
				public static LocString NAME = "Uncultured";

				// Token: 0x0400E3D0 RID: 58320
				public static LocString DESC = "This Duplicant has simply no appreciation for the arts";
			}

			// Token: 0x02003796 RID: 14230
			public class EARLYBIRD
			{
				// Token: 0x0400E3D1 RID: 58321
				public static LocString NAME = "Early Bird";

				// Token: 0x0400E3D2 RID: 58322
				public static LocString DESC = "This Duplicant always wakes up feeling fresh and efficient!";

				// Token: 0x0400E3D3 RID: 58323
				public static LocString EXTENDED_DESC = string.Concat(new string[]
				{
					"• Morning: <b>{0}</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: 5 Schedule Blocks"
				});

				// Token: 0x0400E3D4 RID: 58324
				public static LocString SHORT_DESC = "Gains morning Attribute bonuses";

				// Token: 0x0400E3D5 RID: 58325
				public static LocString SHORT_DESC_TOOLTIP = string.Concat(new string[]
				{
					"Morning: <b>+2</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: 5 Schedule Blocks"
				});
			}

			// Token: 0x02003797 RID: 14231
			public class NIGHTOWL
			{
				// Token: 0x0400E3D6 RID: 58326
				public static LocString NAME = "Night Owl";

				// Token: 0x0400E3D7 RID: 58327
				public static LocString DESC = "This Duplicant does their best work when they'd ought to be sleeping";

				// Token: 0x0400E3D8 RID: 58328
				public static LocString EXTENDED_DESC = string.Concat(new string[]
				{
					"• Nighttime: <b>{0}</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: All Night"
				});

				// Token: 0x0400E3D9 RID: 58329
				public static LocString SHORT_DESC = "Gains nighttime Attribute bonuses";

				// Token: 0x0400E3DA RID: 58330
				public static LocString SHORT_DESC_TOOLTIP = string.Concat(new string[]
				{
					"Nighttime: <b>+3</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: All Night"
				});
			}

			// Token: 0x02003798 RID: 14232
			public class METEORPHILE
			{
				// Token: 0x0400E3DB RID: 58331
				public static LocString NAME = "Rock Fan";

				// Token: 0x0400E3DC RID: 58332
				public static LocString DESC = "Meteor showers get this Duplicant really, really hyped";

				// Token: 0x0400E3DD RID: 58333
				public static LocString EXTENDED_DESC = "• During meteor showers: <b>{0}</b> bonus to all " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD;

				// Token: 0x0400E3DE RID: 58334
				public static LocString SHORT_DESC = "Gains Attribute bonuses during meteor showers.";

				// Token: 0x0400E3DF RID: 58335
				public static LocString SHORT_DESC_TOOLTIP = "During meteor showers: <b>+3</b> bonus to all " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD;
			}

			// Token: 0x02003799 RID: 14233
			public class REGENERATION
			{
				// Token: 0x0400E3E0 RID: 58336
				public static LocString NAME = "Regenerative";

				// Token: 0x0400E3E1 RID: 58337
				public static LocString DESC = "This robust Duplicant is constantly regenerating health";
			}

			// Token: 0x0200379A RID: 14234
			public class DEEPERDIVERSLUNGS
			{
				// Token: 0x0400E3E2 RID: 58338
				public static LocString NAME = "Deep Diver's Lungs";

				// Token: 0x0400E3E3 RID: 58339
				public static LocString DESC = "This Duplicant has a frankly impressive ability to hold their breath";
			}

			// Token: 0x0200379B RID: 14235
			public class SUNNYDISPOSITION
			{
				// Token: 0x0400E3E4 RID: 58340
				public static LocString NAME = "Sunny Disposition";

				// Token: 0x0400E3E5 RID: 58341
				public static LocString DESC = "This Duplicant has an unwaveringly positive outlook on life";
			}

			// Token: 0x0200379C RID: 14236
			public class ROCKCRUSHER
			{
				// Token: 0x0400E3E6 RID: 58342
				public static LocString NAME = "Beefsteak";

				// Token: 0x0400E3E7 RID: 58343
				public static LocString DESC = "This Duplicant's got muscles on their muscles!";
			}

			// Token: 0x0200379D RID: 14237
			public class SIMPLETASTES
			{
				// Token: 0x0400E3E8 RID: 58344
				public static LocString NAME = "Shrivelled Tastebuds";

				// Token: 0x0400E3E9 RID: 58345
				public static LocString DESC = "This Duplicant could lick a Puft's backside and taste nothing";
			}

			// Token: 0x0200379E RID: 14238
			public class FOODIE
			{
				// Token: 0x0400E3EA RID: 58346
				public static LocString NAME = "Gourmet";

				// Token: 0x0400E3EB RID: 58347
				public static LocString DESC = "This Duplicant's refined palate demands only the most luxurious dishes the colony can offer";
			}

			// Token: 0x0200379F RID: 14239
			public class ARCHAEOLOGIST
			{
				// Token: 0x0400E3EC RID: 58348
				public static LocString NAME = "Relic Hunter";

				// Token: 0x0400E3ED RID: 58349
				public static LocString DESC = "This Duplicant was never taught the phrase \"take only pictures, leave only footprints\"";
			}

			// Token: 0x020037A0 RID: 14240
			public class DECORUP
			{
				// Token: 0x0400E3EE RID: 58350
				public static LocString NAME = "Innately Stylish";

				// Token: 0x0400E3EF RID: 58351
				public static LocString DESC = "This Duplicant's radiant self-confidence makes even the rattiest outfits look trendy";
			}

			// Token: 0x020037A1 RID: 14241
			public class DECORDOWN
			{
				// Token: 0x0400E3F0 RID: 58352
				public static LocString NAME = "Shabby Dresser";

				// Token: 0x0400E3F1 RID: 58353
				public static LocString DESC = "This Duplicant's clearly never heard of ironing";
			}

			// Token: 0x020037A2 RID: 14242
			public class THRIVER
			{
				// Token: 0x0400E3F2 RID: 58354
				public static LocString NAME = "Duress to Impress";

				// Token: 0x0400E3F3 RID: 58355
				public static LocString DESC = "This Duplicant kicks into hyperdrive when the stress is on";

				// Token: 0x0400E3F4 RID: 58356
				public static LocString SHORT_DESC = "Attribute bonuses while stressed";

				// Token: 0x0400E3F5 RID: 58357
				public static LocString SHORT_DESC_TOOLTIP = "More than 60% Stress: <b>+7</b> bonus to all " + UI.FormatAsKeyWord("Attributes");
			}

			// Token: 0x020037A3 RID: 14243
			public class LONER
			{
				// Token: 0x0400E3F6 RID: 58358
				public static LocString NAME = "Loner";

				// Token: 0x0400E3F7 RID: 58359
				public static LocString DESC = "This Duplicant prefers solitary pursuits";

				// Token: 0x0400E3F8 RID: 58360
				public static LocString SHORT_DESC = "Attribute bonuses while alone";

				// Token: 0x0400E3F9 RID: 58361
				public static LocString SHORT_DESC_TOOLTIP = "Only Duplicant on a world: <b>+4</b> bonus to all " + UI.FormatAsKeyWord("Attributes");
			}

			// Token: 0x020037A4 RID: 14244
			public class STARRYEYED
			{
				// Token: 0x0400E3FA RID: 58362
				public static LocString NAME = "Starry Eyed";

				// Token: 0x0400E3FB RID: 58363
				public static LocString DESC = "This Duplicant loves being in space";

				// Token: 0x0400E3FC RID: 58364
				public static LocString SHORT_DESC = "Morale bonus while in space";

				// Token: 0x0400E3FD RID: 58365
				public static LocString SHORT_DESC_TOOLTIP = "In outer space: <b>+10</b> " + UI.FormatAsKeyWord("Morale");
			}

			// Token: 0x020037A5 RID: 14245
			public class GLOWSTICK
			{
				// Token: 0x0400E3FE RID: 58366
				public static LocString NAME = "Glow Stick";

				// Token: 0x0400E3FF RID: 58367
				public static LocString DESC = "This Duplicant is positively glowing";

				// Token: 0x0400E400 RID: 58368
				public static LocString SHORT_DESC = "Emits low amounts of rads and light";

				// Token: 0x0400E401 RID: 58369
				public static LocString SHORT_DESC_TOOLTIP = "Emits low amounts of rads and light";
			}

			// Token: 0x020037A6 RID: 14246
			public class RADIATIONEATER
			{
				// Token: 0x0400E402 RID: 58370
				public static LocString NAME = "Radiation Eater";

				// Token: 0x0400E403 RID: 58371
				public static LocString DESC = "This Duplicant eats radiation for breakfast (and dinner)";

				// Token: 0x0400E404 RID: 58372
				public static LocString SHORT_DESC = "Converts radiation exposure into calories";

				// Token: 0x0400E405 RID: 58373
				public static LocString SHORT_DESC_TOOLTIP = "Converts radiation exposure into calories";
			}

			// Token: 0x020037A7 RID: 14247
			public class NIGHTLIGHT
			{
				// Token: 0x0400E406 RID: 58374
				public static LocString NAME = "Nyctophobic";

				// Token: 0x0400E407 RID: 58375
				public static LocString DESC = "This Duplicant will imagine scary shapes in the dark all night if no one leaves a light on";

				// Token: 0x0400E408 RID: 58376
				public static LocString SHORT_DESC = "Requires light to sleep";

				// Token: 0x0400E409 RID: 58377
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant can't sleep in complete darkness";
			}

			// Token: 0x020037A8 RID: 14248
			public class GREENTHUMB
			{
				// Token: 0x0400E40A RID: 58378
				public static LocString NAME = "Green Thumb";

				// Token: 0x0400E40B RID: 58379
				public static LocString DESC = "This Duplicant regards every plant as a potential friend";
			}

			// Token: 0x020037A9 RID: 14249
			public class FROSTPROOF
			{
				// Token: 0x0400E40C RID: 58380
				public static LocString NAME = "Frost Proof";

				// Token: 0x0400E40D RID: 58381
				public static LocString DESC = "This Duplicant is too cool to be bothered by the cold";
			}

			// Token: 0x020037AA RID: 14250
			public class CONSTRUCTIONUP
			{
				// Token: 0x0400E40E RID: 58382
				public static LocString NAME = "Handy";

				// Token: 0x0400E40F RID: 58383
				public static LocString DESC = "This Duplicant is a swift and skilled builder";
			}

			// Token: 0x020037AB RID: 14251
			public class RANCHINGUP
			{
				// Token: 0x0400E410 RID: 58384
				public static LocString NAME = "Animal Lover";

				// Token: 0x0400E411 RID: 58385
				public static LocString DESC = "The fuzzy snoots! The little claws! The chitinous exoskeletons! This Duplicant's never met a critter they didn't like";
			}

			// Token: 0x020037AC RID: 14252
			public class CONSTRUCTIONDOWN
			{
				// Token: 0x0400E412 RID: 58386
				public static LocString NAME = "Building Impaired";

				// Token: 0x0400E413 RID: 58387
				public static LocString DESC = "This Duplicant has trouble constructing anything besides meaningful friendships";
			}

			// Token: 0x020037AD RID: 14253
			public class RANCHINGDOWN
			{
				// Token: 0x0400E414 RID: 58388
				public static LocString NAME = "Critter Aversion";

				// Token: 0x0400E415 RID: 58389
				public static LocString DESC = "This Duplicant just doesn't trust those beady little eyes";
			}

			// Token: 0x020037AE RID: 14254
			public class DIGGINGDOWN
			{
				// Token: 0x0400E416 RID: 58390
				public static LocString NAME = "Undigging";

				// Token: 0x0400E417 RID: 58391
				public static LocString DESC = "This Duplicant couldn't dig themselves out of a paper bag";
			}

			// Token: 0x020037AF RID: 14255
			public class MACHINERYDOWN
			{
				// Token: 0x0400E418 RID: 58392
				public static LocString NAME = "Luddite";

				// Token: 0x0400E419 RID: 58393
				public static LocString DESC = "This Duplicant always invites friends over just to make them hook up their electronics";
			}

			// Token: 0x020037B0 RID: 14256
			public class COOKINGDOWN
			{
				// Token: 0x0400E41A RID: 58394
				public static LocString NAME = "Kitchen Menace";

				// Token: 0x0400E41B RID: 58395
				public static LocString DESC = "This Duplicant could probably figure out a way to burn ice cream";
			}

			// Token: 0x020037B1 RID: 14257
			public class ARTDOWN
			{
				// Token: 0x0400E41C RID: 58396
				public static LocString NAME = "Unpracticed Artist";

				// Token: 0x0400E41D RID: 58397
				public static LocString DESC = "This Duplicant proudly proclaims they \"can't even draw a stick figure\"";
			}

			// Token: 0x020037B2 RID: 14258
			public class CARINGDOWN
			{
				// Token: 0x0400E41E RID: 58398
				public static LocString NAME = "Unempathetic";

				// Token: 0x0400E41F RID: 58399
				public static LocString DESC = "This Duplicant's lack of bedside manner makes it difficult for them to nurse peers back to health";
			}

			// Token: 0x020037B3 RID: 14259
			public class BOTANISTDOWN
			{
				// Token: 0x0400E420 RID: 58400
				public static LocString NAME = "Plant Murderer";

				// Token: 0x0400E421 RID: 58401
				public static LocString DESC = "Never ask this Duplicant to watch your ferns when you go on vacation";
			}

			// Token: 0x020037B4 RID: 14260
			public class GRANTSKILL_MINING1
			{
				// Token: 0x0400E422 RID: 58402
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_MINER.NAME;

				// Token: 0x0400E423 RID: 58403
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_MINER.DESCRIPTION;

				// Token: 0x0400E424 RID: 58404
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E425 RID: 58405
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037B5 RID: 14261
			public class GRANTSKILL_MINING2
			{
				// Token: 0x0400E426 RID: 58406
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MINER.NAME;

				// Token: 0x0400E427 RID: 58407
				public static LocString DESC = DUPLICANTS.ROLES.MINER.DESCRIPTION;

				// Token: 0x0400E428 RID: 58408
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E429 RID: 58409
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037B6 RID: 14262
			public class GRANTSKILL_MINING3
			{
				// Token: 0x0400E42A RID: 58410
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_MINER.NAME;

				// Token: 0x0400E42B RID: 58411
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_MINER.DESCRIPTION;

				// Token: 0x0400E42C RID: 58412
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E42D RID: 58413
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037B7 RID: 14263
			public class GRANTSKILL_MINING4
			{
				// Token: 0x0400E42E RID: 58414
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MASTER_MINER.NAME;

				// Token: 0x0400E42F RID: 58415
				public static LocString DESC = DUPLICANTS.ROLES.MASTER_MINER.DESCRIPTION;

				// Token: 0x0400E430 RID: 58416
				public static LocString SHORT_DESC = "Starts with a Tier 4 <b>Skill</b>";

				// Token: 0x0400E431 RID: 58417
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037B8 RID: 14264
			public class GRANTSKILL_BUILDING1
			{
				// Token: 0x0400E432 RID: 58418
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME;

				// Token: 0x0400E433 RID: 58419
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_BUILDER.DESCRIPTION;

				// Token: 0x0400E434 RID: 58420
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E435 RID: 58421
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037B9 RID: 14265
			public class GRANTSKILL_BUILDING2
			{
				// Token: 0x0400E436 RID: 58422
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.BUILDER.NAME;

				// Token: 0x0400E437 RID: 58423
				public static LocString DESC = DUPLICANTS.ROLES.BUILDER.DESCRIPTION;

				// Token: 0x0400E438 RID: 58424
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E439 RID: 58425
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037BA RID: 14266
			public class GRANTSKILL_BUILDING3
			{
				// Token: 0x0400E43A RID: 58426
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_BUILDER.NAME;

				// Token: 0x0400E43B RID: 58427
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_BUILDER.DESCRIPTION;

				// Token: 0x0400E43C RID: 58428
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E43D RID: 58429
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037BB RID: 14267
			public class GRANTSKILL_FARMING1
			{
				// Token: 0x0400E43E RID: 58430
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_FARMER.NAME;

				// Token: 0x0400E43F RID: 58431
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_FARMER.DESCRIPTION;

				// Token: 0x0400E440 RID: 58432
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E441 RID: 58433
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037BC RID: 14268
			public class GRANTSKILL_FARMING2
			{
				// Token: 0x0400E442 RID: 58434
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.FARMER.NAME;

				// Token: 0x0400E443 RID: 58435
				public static LocString DESC = DUPLICANTS.ROLES.FARMER.DESCRIPTION;

				// Token: 0x0400E444 RID: 58436
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E445 RID: 58437
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037BD RID: 14269
			public class GRANTSKILL_FARMING3
			{
				// Token: 0x0400E446 RID: 58438
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_FARMER.NAME;

				// Token: 0x0400E447 RID: 58439
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_FARMER.DESCRIPTION;

				// Token: 0x0400E448 RID: 58440
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E449 RID: 58441
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037BE RID: 14270
			public class GRANTSKILL_RANCHING1
			{
				// Token: 0x0400E44A RID: 58442
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.RANCHER.NAME;

				// Token: 0x0400E44B RID: 58443
				public static LocString DESC = DUPLICANTS.ROLES.RANCHER.DESCRIPTION;

				// Token: 0x0400E44C RID: 58444
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E44D RID: 58445
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037BF RID: 14271
			public class GRANTSKILL_RANCHING2
			{
				// Token: 0x0400E44E RID: 58446
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_RANCHER.NAME;

				// Token: 0x0400E44F RID: 58447
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_RANCHER.DESCRIPTION;

				// Token: 0x0400E450 RID: 58448
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E451 RID: 58449
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C0 RID: 14272
			public class GRANTSKILL_RESEARCHING1
			{
				// Token: 0x0400E452 RID: 58450
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME;

				// Token: 0x0400E453 RID: 58451
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400E454 RID: 58452
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E455 RID: 58453
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C1 RID: 14273
			public class GRANTSKILL_RESEARCHING2
			{
				// Token: 0x0400E456 RID: 58454
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.RESEARCHER.NAME;

				// Token: 0x0400E457 RID: 58455
				public static LocString DESC = DUPLICANTS.ROLES.RESEARCHER.DESCRIPTION;

				// Token: 0x0400E458 RID: 58456
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E459 RID: 58457
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C2 RID: 14274
			public class GRANTSKILL_RESEARCHING3
			{
				// Token: 0x0400E45A RID: 58458
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME;

				// Token: 0x0400E45B RID: 58459
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400E45C RID: 58460
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E45D RID: 58461
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C3 RID: 14275
			public class GRANTSKILL_RESEARCHING4
			{
				// Token: 0x0400E45E RID: 58462
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME;

				// Token: 0x0400E45F RID: 58463
				public static LocString DESC = DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400E460 RID: 58464
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E461 RID: 58465
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C4 RID: 14276
			public class GRANTSKILL_COOKING1
			{
				// Token: 0x0400E462 RID: 58466
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_COOK.NAME;

				// Token: 0x0400E463 RID: 58467
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_COOK.DESCRIPTION;

				// Token: 0x0400E464 RID: 58468
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E465 RID: 58469
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C5 RID: 14277
			public class GRANTSKILL_COOKING2
			{
				// Token: 0x0400E466 RID: 58470
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.COOK.NAME;

				// Token: 0x0400E467 RID: 58471
				public static LocString DESC = DUPLICANTS.ROLES.COOK.DESCRIPTION;

				// Token: 0x0400E468 RID: 58472
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E469 RID: 58473
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C6 RID: 14278
			public class GRANTSKILL_ARTING1
			{
				// Token: 0x0400E46A RID: 58474
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME;

				// Token: 0x0400E46B RID: 58475
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_ARTIST.DESCRIPTION;

				// Token: 0x0400E46C RID: 58476
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E46D RID: 58477
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C7 RID: 14279
			public class GRANTSKILL_ARTING2
			{
				// Token: 0x0400E46E RID: 58478
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ARTIST.NAME;

				// Token: 0x0400E46F RID: 58479
				public static LocString DESC = DUPLICANTS.ROLES.ARTIST.DESCRIPTION;

				// Token: 0x0400E470 RID: 58480
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E471 RID: 58481
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C8 RID: 14280
			public class GRANTSKILL_ARTING3
			{
				// Token: 0x0400E472 RID: 58482
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MASTER_ARTIST.NAME;

				// Token: 0x0400E473 RID: 58483
				public static LocString DESC = DUPLICANTS.ROLES.MASTER_ARTIST.DESCRIPTION;

				// Token: 0x0400E474 RID: 58484
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E475 RID: 58485
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037C9 RID: 14281
			public class GRANTSKILL_HAULING1
			{
				// Token: 0x0400E476 RID: 58486
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.HAULER.NAME;

				// Token: 0x0400E477 RID: 58487
				public static LocString DESC = DUPLICANTS.ROLES.HAULER.DESCRIPTION;

				// Token: 0x0400E478 RID: 58488
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E479 RID: 58489
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037CA RID: 14282
			public class GRANTSKILL_HAULING2
			{
				// Token: 0x0400E47A RID: 58490
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME;

				// Token: 0x0400E47B RID: 58491
				public static LocString DESC = DUPLICANTS.ROLES.MATERIALS_MANAGER.DESCRIPTION;

				// Token: 0x0400E47C RID: 58492
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E47D RID: 58493
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037CB RID: 14283
			public class GRANTSKILL_SUITS1
			{
				// Token: 0x0400E47E RID: 58494
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SUIT_EXPERT.NAME;

				// Token: 0x0400E47F RID: 58495
				public static LocString DESC = DUPLICANTS.ROLES.SUIT_EXPERT.DESCRIPTION;

				// Token: 0x0400E480 RID: 58496
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E481 RID: 58497
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037CC RID: 14284
			public class GRANTSKILL_TECHNICALS1
			{
				// Token: 0x0400E482 RID: 58498
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME;

				// Token: 0x0400E483 RID: 58499
				public static LocString DESC = DUPLICANTS.ROLES.MACHINE_TECHNICIAN.DESCRIPTION;

				// Token: 0x0400E484 RID: 58500
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E485 RID: 58501
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037CD RID: 14285
			public class GRANTSKILL_TECHNICALS2
			{
				// Token: 0x0400E486 RID: 58502
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400E487 RID: 58503
				public static LocString DESC = DUPLICANTS.ROLES.POWER_TECHNICIAN.DESCRIPTION;

				// Token: 0x0400E488 RID: 58504
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E489 RID: 58505
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037CE RID: 14286
			public class GRANTSKILL_ENGINEERING1
			{
				// Token: 0x0400E48A RID: 58506
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400E48B RID: 58507
				public static LocString DESC = DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.DESCRIPTION;

				// Token: 0x0400E48C RID: 58508
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E48D RID: 58509
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037CF RID: 14287
			public class GRANTSKILL_BASEKEEPING1
			{
				// Token: 0x0400E48E RID: 58510
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.HANDYMAN.NAME;

				// Token: 0x0400E48F RID: 58511
				public static LocString DESC = DUPLICANTS.ROLES.HANDYMAN.DESCRIPTION;

				// Token: 0x0400E490 RID: 58512
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E491 RID: 58513
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037D0 RID: 14288
			public class GRANTSKILL_BASEKEEPING2
			{
				// Token: 0x0400E492 RID: 58514
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.PLUMBER.NAME;

				// Token: 0x0400E493 RID: 58515
				public static LocString DESC = DUPLICANTS.ROLES.PLUMBER.DESCRIPTION;

				// Token: 0x0400E494 RID: 58516
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E495 RID: 58517
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037D1 RID: 14289
			public class GRANTSKILL_ASTRONAUTING1
			{
				// Token: 0x0400E496 RID: 58518
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ASTRONAUTTRAINEE.NAME;

				// Token: 0x0400E497 RID: 58519
				public static LocString DESC = DUPLICANTS.ROLES.ASTRONAUTTRAINEE.DESCRIPTION;

				// Token: 0x0400E498 RID: 58520
				public static LocString SHORT_DESC = "Starts with a Tier 4 <b>Skill</b>";

				// Token: 0x0400E499 RID: 58521
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037D2 RID: 14290
			public class GRANTSKILL_ASTRONAUTING2
			{
				// Token: 0x0400E49A RID: 58522
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ASTRONAUT.NAME;

				// Token: 0x0400E49B RID: 58523
				public static LocString DESC = DUPLICANTS.ROLES.ASTRONAUT.DESCRIPTION;

				// Token: 0x0400E49C RID: 58524
				public static LocString SHORT_DESC = "Starts with a Tier 5 <b>Skill</b>";

				// Token: 0x0400E49D RID: 58525
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037D3 RID: 14291
			public class GRANTSKILL_MEDICINE1
			{
				// Token: 0x0400E49E RID: 58526
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME;

				// Token: 0x0400E49F RID: 58527
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_MEDIC.DESCRIPTION;

				// Token: 0x0400E4A0 RID: 58528
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400E4A1 RID: 58529
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037D4 RID: 14292
			public class GRANTSKILL_MEDICINE2
			{
				// Token: 0x0400E4A2 RID: 58530
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MEDIC.NAME;

				// Token: 0x0400E4A3 RID: 58531
				public static LocString DESC = DUPLICANTS.ROLES.MEDIC.DESCRIPTION;

				// Token: 0x0400E4A4 RID: 58532
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400E4A5 RID: 58533
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037D5 RID: 14293
			public class GRANTSKILL_MEDICINE3
			{
				// Token: 0x0400E4A6 RID: 58534
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_MEDIC.NAME;

				// Token: 0x0400E4A7 RID: 58535
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_MEDIC.DESCRIPTION;

				// Token: 0x0400E4A8 RID: 58536
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E4A9 RID: 58537
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037D6 RID: 14294
			public class GRANTSKILL_PYROTECHNICS
			{
				// Token: 0x0400E4AA RID: 58538
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.PYROTECHNIC.NAME;

				// Token: 0x0400E4AB RID: 58539
				public static LocString DESC = DUPLICANTS.ROLES.PYROTECHNIC.DESCRIPTION;

				// Token: 0x0400E4AC RID: 58540
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400E4AD RID: 58541
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020037D7 RID: 14295
			public class STARTWITHBOOSTER_DIG1
			{
				// Token: 0x0400E4AE RID: 58542
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_DIG1.NAME;

				// Token: 0x0400E4AF RID: 58543
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_DIG1.DESC;

				// Token: 0x0400E4B0 RID: 58544
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_DIG1.NAME + "</b>";

				// Token: 0x0400E4B1 RID: 58545
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037D8 RID: 14296
			public class STARTWITHBOOSTER_CONSTRUCT1
			{
				// Token: 0x0400E4B2 RID: 58546
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_CONSTRUCT1.NAME;

				// Token: 0x0400E4B3 RID: 58547
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_CONSTRUCT1.DESC;

				// Token: 0x0400E4B4 RID: 58548
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_CONSTRUCT1.NAME + "</b>";

				// Token: 0x0400E4B5 RID: 58549
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037D9 RID: 14297
			public class STARTWITHBOOSTER_CARRY1
			{
				// Token: 0x0400E4B6 RID: 58550
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_CARRY1.NAME;

				// Token: 0x0400E4B7 RID: 58551
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_CARRY1.DESC;

				// Token: 0x0400E4B8 RID: 58552
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_CARRY1.NAME + "</b>";

				// Token: 0x0400E4B9 RID: 58553
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037DA RID: 14298
			public class STARTWITHBOOSTER_MEDICINE1
			{
				// Token: 0x0400E4BA RID: 58554
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_MEDICINE1.NAME;

				// Token: 0x0400E4BB RID: 58555
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_MEDICINE1.DESC;

				// Token: 0x0400E4BC RID: 58556
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_MEDICINE1.NAME + "</b>";

				// Token: 0x0400E4BD RID: 58557
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037DB RID: 14299
			public class STARTWITHBOOSTER_DIG2
			{
				// Token: 0x0400E4BE RID: 58558
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_DIG2.NAME;

				// Token: 0x0400E4BF RID: 58559
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_DIG2.DESC;

				// Token: 0x0400E4C0 RID: 58560
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_DIG2.NAME + "</b>";

				// Token: 0x0400E4C1 RID: 58561
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037DC RID: 14300
			public class STARTWITHBOOSTER_FARM1
			{
				// Token: 0x0400E4C2 RID: 58562
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_FARM1.NAME;

				// Token: 0x0400E4C3 RID: 58563
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_FARM1.DESC;

				// Token: 0x0400E4C4 RID: 58564
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_FARM1.NAME + "</b>";

				// Token: 0x0400E4C5 RID: 58565
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037DD RID: 14301
			public class STARTWITHBOOSTER_RANCH1
			{
				// Token: 0x0400E4C6 RID: 58566
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_RANCH1.NAME;

				// Token: 0x0400E4C7 RID: 58567
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_RANCH1.DESC;

				// Token: 0x0400E4C8 RID: 58568
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_RANCH1.NAME + "</b>";

				// Token: 0x0400E4C9 RID: 58569
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037DE RID: 14302
			public class STARTWITHBOOSTER_COOK1
			{
				// Token: 0x0400E4CA RID: 58570
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_COOK1.NAME;

				// Token: 0x0400E4CB RID: 58571
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_COOK1.DESC;

				// Token: 0x0400E4CC RID: 58572
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_COOK1.NAME + "</b>";

				// Token: 0x0400E4CD RID: 58573
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037DF RID: 14303
			public class STARTWITHBOOSTER_OP1
			{
				// Token: 0x0400E4CE RID: 58574
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_OP1.NAME;

				// Token: 0x0400E4CF RID: 58575
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_OP1.DESC;

				// Token: 0x0400E4D0 RID: 58576
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_OP1.NAME + "</b>";

				// Token: 0x0400E4D1 RID: 58577
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037E0 RID: 14304
			public class STARTWITHBOOSTER_ART1
			{
				// Token: 0x0400E4D2 RID: 58578
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_ART1.NAME;

				// Token: 0x0400E4D3 RID: 58579
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_ART1.DESC;

				// Token: 0x0400E4D4 RID: 58580
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_ART1.NAME + "</b>";

				// Token: 0x0400E4D5 RID: 58581
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037E1 RID: 14305
			public class STARTWITHBOOSTER_SUITS1
			{
				// Token: 0x0400E4D6 RID: 58582
				public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BOOSTER_SUITS1.NAME;

				// Token: 0x0400E4D7 RID: 58583
				public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BOOSTER_SUITS1.DESC;

				// Token: 0x0400E4D8 RID: 58584
				public static LocString SHORT_DESC = "Starts with a preinstalled <b>" + ITEMS.BIONIC_BOOSTERS.BOOSTER_SUITS1.NAME + "</b>";

				// Token: 0x0400E4D9 RID: 58585
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
			}

			// Token: 0x020037E2 RID: 14306
			public class BIONICBUG1
			{
				// Token: 0x0400E4DA RID: 58586
				public static LocString NAME = "Bionic Bug: Rigid Thinking";

				// Token: 0x0400E4DB RID: 58587
				public static LocString DESC = "This Duplicant's bionic systems are quite inflexible";

				// Token: 0x0400E4DC RID: 58588
				public static LocString SHORT_DESC = "No passive attribute leveling";

				// Token: 0x0400E4DD RID: 58589
				public static LocString SHORT_DESC_TOOLTIP = "Does not level up attributes while performing errands\n\nRequires boosters to improve skills";
			}

			// Token: 0x020037E3 RID: 14307
			public class BIONICBUG2
			{
				// Token: 0x0400E4DE RID: 58590
				public static LocString NAME = "Bionic Bug: Dissociative";

				// Token: 0x0400E4DF RID: 58591
				public static LocString DESC = "This Duplicant's bionic systems are built without \"connector\" parts";

				// Token: 0x0400E4E0 RID: 58592
				public static LocString SHORT_DESC = "No passive attribute leveling";

				// Token: 0x0400E4E1 RID: 58593
				public static LocString SHORT_DESC_TOOLTIP = "Does not level up attributes while performing errands\n\nRequires boosters to improve skills";
			}

			// Token: 0x020037E4 RID: 14308
			public class BIONICBUG3
			{
				// Token: 0x0400E4E2 RID: 58594
				public static LocString NAME = "Bionic Bug: All Thumbs";

				// Token: 0x0400E4E3 RID: 58595
				public static LocString DESC = "This Duplicant's bionic systems aren't designed to operate other systems";

				// Token: 0x0400E4E4 RID: 58596
				public static LocString SHORT_DESC = "No passive attribute leveling";

				// Token: 0x0400E4E5 RID: 58597
				public static LocString SHORT_DESC_TOOLTIP = "Does not level up attributes while performing errands\n\nRequires boosters to improve skills";
			}

			// Token: 0x020037E5 RID: 14309
			public class BIONICBUG4
			{
				// Token: 0x0400E4E6 RID: 58598
				public static LocString NAME = "Bionic Bug: Overengineered";

				// Token: 0x0400E4E7 RID: 58599
				public static LocString DESC = "This Duplicant's bionic systems rarely get past the processing stage";

				// Token: 0x0400E4E8 RID: 58600
				public static LocString SHORT_DESC = "No passive attribute leveling";

				// Token: 0x0400E4E9 RID: 58601
				public static LocString SHORT_DESC_TOOLTIP = "Does not level up attributes while performing errands\n\nRequires boosters to improve skills";
			}

			// Token: 0x020037E6 RID: 14310
			public class BIONICBUG5
			{
				// Token: 0x0400E4EA RID: 58602
				public static LocString NAME = "Bionic Bug: Late Bloomer";

				// Token: 0x0400E4EB RID: 58603
				public static LocString DESC = "This Duplicant's bionic systems weren't built for speed";

				// Token: 0x0400E4EC RID: 58604
				public static LocString SHORT_DESC = "No passive attribute leveling";

				// Token: 0x0400E4ED RID: 58605
				public static LocString SHORT_DESC_TOOLTIP = "Does not level up attributes while performing errands\n\nRequires boosters to improve skills";
			}

			// Token: 0x020037E7 RID: 14311
			public class BIONICBUG6
			{
				// Token: 0x0400E4EE RID: 58606
				public static LocString NAME = "Bionic Bug: Urbanite";

				// Token: 0x0400E4EF RID: 58607
				public static LocString DESC = "This Duplicant's bionic systems were designed by someone who'd never seen a plant in real life";

				// Token: 0x0400E4F0 RID: 58608
				public static LocString SHORT_DESC = "No passive attribute leveling";

				// Token: 0x0400E4F1 RID: 58609
				public static LocString SHORT_DESC_TOOLTIP = "Does not level up attributes while performing errands\n\nRequires boosters to improve skills";
			}

			// Token: 0x020037E8 RID: 14312
			public class BIONICBUG7
			{
				// Token: 0x0400E4F2 RID: 58610
				public static LocString NAME = "Bionic Bug: Error Prone";

				// Token: 0x0400E4F3 RID: 58611
				public static LocString DESC = "This Duplicant's bionic systems err on the side of erring";

				// Token: 0x0400E4F4 RID: 58612
				public static LocString SHORT_DESC = "No passive attribute leveling";

				// Token: 0x0400E4F5 RID: 58613
				public static LocString SHORT_DESC_TOOLTIP = "Does not level up attributes while performing errands\n\nRequires boosters to improve skills";
			}
		}

		// Token: 0x02002588 RID: 9608
		public class PERSONALITIES
		{
			// Token: 0x020037E9 RID: 14313
			public class CATALINA
			{
				// Token: 0x0400E4F6 RID: 58614
				public static LocString NAME = "Catalina";

				// Token: 0x0400E4F7 RID: 58615
				public static LocString DESC = "A {0} is admired by all for her seemingly tireless work ethic. Little do people know, she's dying on the inside.";
			}

			// Token: 0x020037EA RID: 14314
			public class NISBET
			{
				// Token: 0x0400E4F8 RID: 58616
				public static LocString NAME = "Nisbet";

				// Token: 0x0400E4F9 RID: 58617
				public static LocString DESC = "This {0} likes to punch people to show her affection. Everyone's too afraid of her to tell her it hurts.";
			}

			// Token: 0x020037EB RID: 14315
			public class ELLIE
			{
				// Token: 0x0400E4FA RID: 58618
				public static LocString NAME = "Ellie";

				// Token: 0x0400E4FB RID: 58619
				public static LocString DESC = "Nothing makes an {0} happier than a big tin of glitter and a pack of unicorn stickers.";
			}

			// Token: 0x020037EC RID: 14316
			public class RUBY
			{
				// Token: 0x0400E4FC RID: 58620
				public static LocString NAME = "Ruby";

				// Token: 0x0400E4FD RID: 58621
				public static LocString DESC = "This {0} asks the pressing questions, like \"Where can I get a leather jacket in space?\"";
			}

			// Token: 0x020037ED RID: 14317
			public class LEIRA
			{
				// Token: 0x0400E4FE RID: 58622
				public static LocString NAME = "Leira";

				// Token: 0x0400E4FF RID: 58623
				public static LocString DESC = "{0}s just want everyone to be happy.";
			}

			// Token: 0x020037EE RID: 14318
			public class BUBBLES
			{
				// Token: 0x0400E500 RID: 58624
				public static LocString NAME = "Bubbles";

				// Token: 0x0400E501 RID: 58625
				public static LocString DESC = "This {0} is constantly challenging others to fight her, regardless of whether or not she can actually take them.";
			}

			// Token: 0x020037EF RID: 14319
			public class MIMA
			{
				// Token: 0x0400E502 RID: 58626
				public static LocString NAME = "Mi-Ma";

				// Token: 0x0400E503 RID: 58627
				public static LocString DESC = "Ol' {0} here can't stand lookin' at people's knees.";
			}

			// Token: 0x020037F0 RID: 14320
			public class NAILS
			{
				// Token: 0x0400E504 RID: 58628
				public static LocString NAME = "Nails";

				// Token: 0x0400E505 RID: 58629
				public static LocString DESC = "People often expect a Duplicant named \"{0}\" to be tough, but they're all pretty huge wimps.";
			}

			// Token: 0x020037F1 RID: 14321
			public class MAE
			{
				// Token: 0x0400E506 RID: 58630
				public static LocString NAME = "Mae";

				// Token: 0x0400E507 RID: 58631
				public static LocString DESC = "There's nothing a {0} can't do if she sets her mind to it.";
			}

			// Token: 0x020037F2 RID: 14322
			public class GOSSMANN
			{
				// Token: 0x0400E508 RID: 58632
				public static LocString NAME = "Gossmann";

				// Token: 0x0400E509 RID: 58633
				public static LocString DESC = "{0}s are major goofballs who can make anyone laugh.";
			}

			// Token: 0x020037F3 RID: 14323
			public class MARIE
			{
				// Token: 0x0400E50A RID: 58634
				public static LocString NAME = "Marie";

				// Token: 0x0400E50B RID: 58635
				public static LocString DESC = "This {0} is positively glowing! What's her secret? Radioactive isotopes, of course.";
			}

			// Token: 0x020037F4 RID: 14324
			public class LINDSAY
			{
				// Token: 0x0400E50C RID: 58636
				public static LocString NAME = "Lindsay";

				// Token: 0x0400E50D RID: 58637
				public static LocString DESC = "A {0} is a charming woman, unless you make the mistake of messing with one of her friends.";
			}

			// Token: 0x020037F5 RID: 14325
			public class DEVON
			{
				// Token: 0x0400E50E RID: 58638
				public static LocString NAME = "Devon";

				// Token: 0x0400E50F RID: 58639
				public static LocString DESC = "This {0} dreams of owning their own personal computer so they can start a blog full of pictures of toast.";
			}

			// Token: 0x020037F6 RID: 14326
			public class REN
			{
				// Token: 0x0400E510 RID: 58640
				public static LocString NAME = "Ren";

				// Token: 0x0400E511 RID: 58641
				public static LocString DESC = "Every {0} has this unshakable feeling that his life's already happened and he's just watching it unfold from a memory.";
			}

			// Token: 0x020037F7 RID: 14327
			public class FRANKIE
			{
				// Token: 0x0400E512 RID: 58642
				public static LocString NAME = "Frankie";

				// Token: 0x0400E513 RID: 58643
				public static LocString DESC = "There's nothing {0}s are more proud of than their thick, dignified eyebrows.";
			}

			// Token: 0x020037F8 RID: 14328
			public class BANHI
			{
				// Token: 0x0400E514 RID: 58644
				public static LocString NAME = "Banhi";

				// Token: 0x0400E515 RID: 58645
				public static LocString DESC = "The \"cool loner\" vibes that radiate off a {0} never fail to make the colony swoon.";
			}

			// Token: 0x020037F9 RID: 14329
			public class ADA
			{
				// Token: 0x0400E516 RID: 58646
				public static LocString NAME = "Ada";

				// Token: 0x0400E517 RID: 58647
				public static LocString DESC = "{0}s enjoy writing poetry in their downtime. Dark poetry.";
			}

			// Token: 0x020037FA RID: 14330
			public class HASSAN
			{
				// Token: 0x0400E518 RID: 58648
				public static LocString NAME = "Hassan";

				// Token: 0x0400E519 RID: 58649
				public static LocString DESC = "If someone says something nice to a {0} he'll think about it nonstop for no less than three weeks.";
			}

			// Token: 0x020037FB RID: 14331
			public class STINKY
			{
				// Token: 0x0400E51A RID: 58650
				public static LocString NAME = "Stinky";

				// Token: 0x0400E51B RID: 58651
				public static LocString DESC = "This {0} has never been invited to a party, which is a shame. His dance moves are incredible.";
			}

			// Token: 0x020037FC RID: 14332
			public class JOSHUA
			{
				// Token: 0x0400E51C RID: 58652
				public static LocString NAME = "Joshua";

				// Token: 0x0400E51D RID: 58653
				public static LocString DESC = "{0}s are precious goobers. Other Duplicants are strangely incapable of cursing in a {0}'s presence.";
			}

			// Token: 0x020037FD RID: 14333
			public class LIAM
			{
				// Token: 0x0400E51E RID: 58654
				public static LocString NAME = "Liam";

				// Token: 0x0400E51F RID: 58655
				public static LocString DESC = "No matter how much this {0} scrubs, he can never truly feel clean.";
			}

			// Token: 0x020037FE RID: 14334
			public class ABE
			{
				// Token: 0x0400E520 RID: 58656
				public static LocString NAME = "Abe";

				// Token: 0x0400E521 RID: 58657
				public static LocString DESC = "{0}s are sweet, delicate flowers. They need to be treated gingerly, with great consideration for their feelings.";
			}

			// Token: 0x020037FF RID: 14335
			public class BURT
			{
				// Token: 0x0400E522 RID: 58658
				public static LocString NAME = "Burt";

				// Token: 0x0400E523 RID: 58659
				public static LocString DESC = "This {0} always feels great after a bubble bath and a good long cry.";
			}

			// Token: 0x02003800 RID: 14336
			public class TRAVALDO
			{
				// Token: 0x0400E524 RID: 58660
				public static LocString NAME = "Travaldo";

				// Token: 0x0400E525 RID: 58661
				public static LocString DESC = "A {0}'s monotonous voice and lack of facial expression makes it impossible for others to tell when he's messing with them.";
			}

			// Token: 0x02003801 RID: 14337
			public class HAROLD
			{
				// Token: 0x0400E526 RID: 58662
				public static LocString NAME = "Harold";

				// Token: 0x0400E527 RID: 58663
				public static LocString DESC = "Get a bunch of {0}s together in a room, and you'll have... a bunch of {0}s together in a room.";
			}

			// Token: 0x02003802 RID: 14338
			public class MAX
			{
				// Token: 0x0400E528 RID: 58664
				public static LocString NAME = "Max";

				// Token: 0x0400E529 RID: 58665
				public static LocString DESC = "At any given moment a {0} is viscerally reliving ten different humiliating memories.";
			}

			// Token: 0x02003803 RID: 14339
			public class ROWAN
			{
				// Token: 0x0400E52A RID: 58666
				public static LocString NAME = "Rowan";

				// Token: 0x0400E52B RID: 58667
				public static LocString DESC = "{0}s have exceptionally large hearts and express their emotions most efficiently by yelling.";
			}

			// Token: 0x02003804 RID: 14340
			public class OTTO
			{
				// Token: 0x0400E52C RID: 58668
				public static LocString NAME = "Otto";

				// Token: 0x0400E52D RID: 58669
				public static LocString DESC = "{0}s always insult people by accident and generally exist in a perpetual state of deep regret.";
			}

			// Token: 0x02003805 RID: 14341
			public class TURNER
			{
				// Token: 0x0400E52E RID: 58670
				public static LocString NAME = "Turner";

				// Token: 0x0400E52F RID: 58671
				public static LocString DESC = "This {0} is paralyzed by the knowledge that others have memories and perceptions of them they can't control.";
			}

			// Token: 0x02003806 RID: 14342
			public class NIKOLA
			{
				// Token: 0x0400E530 RID: 58672
				public static LocString NAME = "Nikola";

				// Token: 0x0400E531 RID: 58673
				public static LocString DESC = "This {0} once claimed he could build a laser so powerful it would rip the colony in half. No one asked him to prove it.";
			}

			// Token: 0x02003807 RID: 14343
			public class MEEP
			{
				// Token: 0x0400E532 RID: 58674
				public static LocString NAME = "Meep";

				// Token: 0x0400E533 RID: 58675
				public static LocString DESC = "{0}s have a face only a two tonne Printing Pod could love.";
			}

			// Token: 0x02003808 RID: 14344
			public class ARI
			{
				// Token: 0x0400E534 RID: 58676
				public static LocString NAME = "Ari";

				// Token: 0x0400E535 RID: 58677
				public static LocString DESC = "{0}s tend to space out from time to time, but they always pay attention when it counts.";
			}

			// Token: 0x02003809 RID: 14345
			public class JEAN
			{
				// Token: 0x0400E536 RID: 58678
				public static LocString NAME = "Jean";

				// Token: 0x0400E537 RID: 58679
				public static LocString DESC = "Just because {0}s are a little slow doesn't mean they can't suffer from soul-crushing existential crises.";
			}

			// Token: 0x0200380A RID: 14346
			public class CAMILLE
			{
				// Token: 0x0400E538 RID: 58680
				public static LocString NAME = "Camille";

				// Token: 0x0400E539 RID: 58681
				public static LocString DESC = "This {0} loves anything that makes her feel nostalgic, including things that haven't aged well.";
			}

			// Token: 0x0200380B RID: 14347
			public class ASHKAN
			{
				// Token: 0x0400E53A RID: 58682
				public static LocString NAME = "Ashkan";

				// Token: 0x0400E53B RID: 58683
				public static LocString DESC = "{0}s have what can only be described as a \"seriously infectious giggle\".";
			}

			// Token: 0x0200380C RID: 14348
			public class STEVE
			{
				// Token: 0x0400E53C RID: 58684
				public static LocString NAME = "Steve";

				// Token: 0x0400E53D RID: 58685
				public static LocString DESC = "This {0} is convinced that he has psychic powers. And he knows exactly what his friends think about that.";
			}

			// Token: 0x0200380D RID: 14349
			public class AMARI
			{
				// Token: 0x0400E53E RID: 58686
				public static LocString NAME = "Amari";

				// Token: 0x0400E53F RID: 58687
				public static LocString DESC = "{0}s likes to keep the peace. Ironically, they're a riot at parties.";
			}

			// Token: 0x0200380E RID: 14350
			public class PEI
			{
				// Token: 0x0400E540 RID: 58688
				public static LocString NAME = "Pei";

				// Token: 0x0400E541 RID: 58689
				public static LocString DESC = "Every {0} spends at least half the day pretending that they remember what they came into this room for.";
			}

			// Token: 0x0200380F RID: 14351
			public class QUINN
			{
				// Token: 0x0400E542 RID: 58690
				public static LocString NAME = "Quinn";

				// Token: 0x0400E543 RID: 58691
				public static LocString DESC = "This {0}'s favorite genre of music is \"festive power ballad\".";
			}

			// Token: 0x02003810 RID: 14352
			public class JORGE
			{
				// Token: 0x0400E544 RID: 58692
				public static LocString NAME = "Jorge";

				// Token: 0x0400E545 RID: 58693
				public static LocString DESC = "{0} loves his new colony, even if their collective body odor makes his eyes water.";
			}

			// Token: 0x02003811 RID: 14353
			public class FREYJA
			{
				// Token: 0x0400E546 RID: 58694
				public static LocString NAME = "Freyja";

				// Token: 0x0400E547 RID: 58695
				public static LocString DESC = "This {0} has never stopped anyone from eating yellow snow.";
			}

			// Token: 0x02003812 RID: 14354
			public class CHIP
			{
				// Token: 0x0400E548 RID: 58696
				public static LocString NAME = "Chip";

				// Token: 0x0400E549 RID: 58697
				public static LocString DESC = "This {0} is extremely good at guessing their friends' passwords.";
			}

			// Token: 0x02003813 RID: 14355
			public class EDWIREDO
			{
				// Token: 0x0400E54A RID: 58698
				public static LocString NAME = "Edwiredo";

				// Token: 0x0400E54B RID: 58699
				public static LocString DESC = "This {0} once rolled his eye so hard that he powered himself off and on again.";
			}

			// Token: 0x02003814 RID: 14356
			public class GIZMO
			{
				// Token: 0x0400E54C RID: 58700
				public static LocString NAME = "Gizmo";

				// Token: 0x0400E54D RID: 58701
				public static LocString DESC = "{0}s love nothing more than a big juicy info dump.";
			}

			// Token: 0x02003815 RID: 14357
			public class STEELA
			{
				// Token: 0x0400E54E RID: 58702
				public static LocString NAME = "Steela";

				// Token: 0x0400E54F RID: 58703
				public static LocString DESC = "{0}s aren't programmed to put up with nonsense, but they do enjoy the occasional shenanigan.";
			}

			// Token: 0x02003816 RID: 14358
			public class SONYAR
			{
				// Token: 0x0400E550 RID: 58704
				public static LocString NAME = "Sonyar";

				// Token: 0x0400E551 RID: 58705
				public static LocString DESC = "{0}s would sooner burn down the colony than read an instruction manual.";
			}

			// Token: 0x02003817 RID: 14359
			public class ULTI
			{
				// Token: 0x0400E552 RID: 58706
				public static LocString NAME = "Ulti";

				// Token: 0x0400E553 RID: 58707
				public static LocString DESC = "This {0}'s favorite dance move is The Robot. They don't get why others think that's funny.";
			}

			// Token: 0x02003818 RID: 14360
			public class HIGBY
			{
				// Token: 0x0400E554 RID: 58708
				public static LocString NAME = "Higby";

				// Token: 0x0400E555 RID: 58709
				public static LocString DESC = "This {0}'s got a song in his heart. Now if only he could remember how it goes.";
			}

			// Token: 0x02003819 RID: 14361
			public class MAYA
			{
				// Token: 0x0400E556 RID: 58710
				public static LocString NAME = "Maya";

				// Token: 0x0400E557 RID: 58711
				public static LocString DESC = "This {0} got her hand crushed in a pneumatic door once. It was the most alive she's ever felt.";
			}

			// Token: 0x0200381A RID: 14362
			public class SENA
			{
				// Token: 0x0400E558 RID: 58712
				public static LocString NAME = "Sena";

				// Token: 0x0400E559 RID: 58713
				public static LocString DESC = "{0}s only care about three things: gold accents, true crime, and the advancement of nuclear physics.";
			}
		}

		// Token: 0x02002589 RID: 9609
		public class NEEDS
		{
			// Token: 0x0200381B RID: 14363
			public class DECOR
			{
				// Token: 0x0400E55A RID: 58714
				public static LocString NAME = "Decor Expectation";

				// Token: 0x0400E55B RID: 58715
				public static LocString PROFESSION_NAME = "Critic";

				// Token: 0x0400E55C RID: 58716
				public static LocString OBSERVED_DECOR = "Current Surroundings";

				// Token: 0x0400E55D RID: 58717
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"Most objects have ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values that alter Duplicants' opinions of their surroundings.\nThis Duplicant desires ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values of <b>{0}</b> or higher, and becomes ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" in areas with lower ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400E55E RID: 58718
				public static LocString EXPECTATION_MOD_NAME = "Job Tier Request";
			}

			// Token: 0x0200381C RID: 14364
			public class FOOD_QUALITY
			{
				// Token: 0x0400E55F RID: 58719
				public static LocString NAME = "Food Quality";

				// Token: 0x0400E560 RID: 58720
				public static LocString PROFESSION_NAME = "Gourmet";

				// Token: 0x0400E561 RID: 58721
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"Each Duplicant has a minimum quality of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" they'll tolerate eating.\nThis Duplicant desires <b>Tier {0}<b> or better ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					", and becomes ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" when they eat meals of lower quality."
				});

				// Token: 0x0400E562 RID: 58722
				public static LocString BAD_FOOD_MOD = "Food Quality";

				// Token: 0x0400E563 RID: 58723
				public static LocString NORMAL_FOOD_MOD = "Food Quality";

				// Token: 0x0400E564 RID: 58724
				public static LocString GOOD_FOOD_MOD = "Food Quality";

				// Token: 0x0400E565 RID: 58725
				public static LocString EXPECTATION_MOD_NAME = "Job Tier Request";

				// Token: 0x0400E566 RID: 58726
				public static LocString ADJECTIVE_FORMAT_POSITIVE = "{0} [{1}]";

				// Token: 0x0400E567 RID: 58727
				public static LocString ADJECTIVE_FORMAT_NEGATIVE = "{0} [{1}]";

				// Token: 0x0400E568 RID: 58728
				public static LocString FOODQUALITY = "\nFood Quality Score of {0}";

				// Token: 0x0400E569 RID: 58729
				public static LocString FOODQUALITY_EXPECTATION = string.Concat(new string[]
				{
					"\nThis Duplicant is content to eat ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" with a ",
					UI.PRE_KEYWORD,
					"Food Quality",
					UI.PST_KEYWORD,
					" of <b>{0}</b> or higher"
				});

				// Token: 0x0400E56A RID: 58730
				public static int ADJECTIVE_INDEX_OFFSET = -1;

				// Token: 0x02003D97 RID: 15767
				public class ADJECTIVES
				{
					// Token: 0x0400F30E RID: 62222
					public static LocString MINUS_1 = "Grisly";

					// Token: 0x0400F30F RID: 62223
					public static LocString ZERO = "Terrible";

					// Token: 0x0400F310 RID: 62224
					public static LocString PLUS_1 = "Poor";

					// Token: 0x0400F311 RID: 62225
					public static LocString PLUS_2 = "Standard";

					// Token: 0x0400F312 RID: 62226
					public static LocString PLUS_3 = "Good";

					// Token: 0x0400F313 RID: 62227
					public static LocString PLUS_4 = "Great";

					// Token: 0x0400F314 RID: 62228
					public static LocString PLUS_5 = "Superb";

					// Token: 0x0400F315 RID: 62229
					public static LocString PLUS_6 = "Ambrosial";
				}
			}

			// Token: 0x0200381D RID: 14365
			public class QUALITYOFLIFE
			{
				// Token: 0x0400E56B RID: 58731
				public static LocString NAME = "Morale Requirements";

				// Token: 0x0400E56C RID: 58732
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"The more responsibilities and stressors a Duplicant has, the more they will desire additional leisure time and improved amenities.\n\nFailing to keep a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" at or above their ",
					UI.PRE_KEYWORD,
					"Morale Need",
					UI.PST_KEYWORD,
					" means they will not be able to unwind, causing them ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" over time."
				});

				// Token: 0x0400E56D RID: 58733
				public static LocString EXPECTATION_MOD_NAME = "Skills Learned";

				// Token: 0x0400E56E RID: 58734
				public static LocString APTITUDE_SKILLS_MOD_NAME = "Interested Skills Learned";

				// Token: 0x0400E56F RID: 58735
				public static LocString TOTAL_SKILL_POINTS = "Total Skill Points: {0}";

				// Token: 0x0400E570 RID: 58736
				public static LocString GOOD_MODIFIER = "High Morale";

				// Token: 0x0400E571 RID: 58737
				public static LocString NEUTRAL_MODIFIER = "Sufficient Morale";

				// Token: 0x0400E572 RID: 58738
				public static LocString BAD_MODIFIER = "Low Morale";
			}

			// Token: 0x0200381E RID: 14366
			public class NOISE
			{
				// Token: 0x0400E573 RID: 58739
				public static LocString NAME = "Noise Expectation";
			}
		}

		// Token: 0x0200258A RID: 9610
		public class ATTRIBUTES
		{
			// Token: 0x0400A9AE RID: 43438
			public static LocString VALUE = "{0}: {1}";

			// Token: 0x0400A9AF RID: 43439
			public static LocString TOTAL_VALUE = "\n\nTotal <b>{1}</b>: {0}";

			// Token: 0x0400A9B0 RID: 43440
			public static LocString BASE_VALUE = "\nBase: {0}";

			// Token: 0x0400A9B1 RID: 43441
			public static LocString MODIFIER_ENTRY = "\n    • {0}: {1}";

			// Token: 0x0400A9B2 RID: 43442
			public static LocString UNPROFESSIONAL_NAME = "Lump";

			// Token: 0x0400A9B3 RID: 43443
			public static LocString UNPROFESSIONAL_DESC = "This Duplicant has no discernible skills";

			// Token: 0x0400A9B4 RID: 43444
			public static LocString PROFESSION_DESC = string.Concat(new string[]
			{
				"Expertise is determined by a Duplicant's highest ",
				UI.PRE_KEYWORD,
				"Attribute",
				UI.PST_KEYWORD,
				"\n\nDuplicants develop higher expectations as their Expertise level increases"
			});

			// Token: 0x0400A9B5 RID: 43445
			public static LocString STORED_VALUE = "Stored value";

			// Token: 0x0200381F RID: 14367
			public class CONSTRUCTION
			{
				// Token: 0x0400E574 RID: 58740
				public static LocString NAME = "Construction";

				// Token: 0x0400E575 RID: 58741
				public static LocString DESC = "Determines a Duplicant's building Speed.";

				// Token: 0x0400E576 RID: 58742
				public static LocString SPEEDMODIFIER = "{0} Construction Speed";
			}

			// Token: 0x02003820 RID: 14368
			public class SCALDINGTHRESHOLD
			{
				// Token: 0x0400E577 RID: 58743
				public static LocString NAME = "Scalding Threshold";

				// Token: 0x0400E578 RID: 58744
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" at which a Duplicant will get burned."
				});
			}

			// Token: 0x02003821 RID: 14369
			public class SCOLDINGTHRESHOLD
			{
				// Token: 0x0400E579 RID: 58745
				public static LocString NAME = "Frostbite Threshold";

				// Token: 0x0400E57A RID: 58746
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" at which a Duplicant will get frostbitten."
				});
			}

			// Token: 0x02003822 RID: 14370
			public class DIGGING
			{
				// Token: 0x0400E57B RID: 58747
				public static LocString NAME = "Excavation";

				// Token: 0x0400E57C RID: 58748
				public static LocString DESC = "Determines a Duplicant's mining speed.";

				// Token: 0x0400E57D RID: 58749
				public static LocString SPEEDMODIFIER = "{0} Digging Speed";

				// Token: 0x0400E57E RID: 58750
				public static LocString ATTACK_MODIFIER = "{0} Attack Damage";
			}

			// Token: 0x02003823 RID: 14371
			public class MACHINERY
			{
				// Token: 0x0400E57F RID: 58751
				public static LocString NAME = "Machinery";

				// Token: 0x0400E580 RID: 58752
				public static LocString DESC = "Determines how quickly a Duplicant uses machines.";

				// Token: 0x0400E581 RID: 58753
				public static LocString SPEEDMODIFIER = "{0} Machine Operation Speed";

				// Token: 0x0400E582 RID: 58754
				public static LocString TINKER_EFFECT_MODIFIER = "{0} Engie's Tune-Up Effect Duration";
			}

			// Token: 0x02003824 RID: 14372
			public class LIFESUPPORT
			{
				// Token: 0x0400E583 RID: 58755
				public static LocString NAME = "Life Support";

				// Token: 0x0400E584 RID: 58756
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how efficiently a Duplicant maintains ",
					BUILDINGS.PREFABS.ALGAEHABITAT.NAME,
					"s, ",
					BUILDINGS.PREFABS.AIRFILTER.NAME,
					"s, and ",
					BUILDINGS.PREFABS.WATERPURIFIER.NAME,
					"s"
				});
			}

			// Token: 0x02003825 RID: 14373
			public class TOGGLE
			{
				// Token: 0x0400E585 RID: 58757
				public static LocString NAME = "Toggle";

				// Token: 0x0400E586 RID: 58758
				public static LocString DESC = "Determines how efficiently a Duplicant tunes machinery, flips switches, and sets sensors.";
			}

			// Token: 0x02003826 RID: 14374
			public class ATHLETICS
			{
				// Token: 0x0400E587 RID: 58759
				public static LocString NAME = "Athletics";

				// Token: 0x0400E588 RID: 58760
				public static LocString DESC = "Determines a Duplicant's default runspeed.";

				// Token: 0x0400E589 RID: 58761
				public static LocString SPEEDMODIFIER = "{0} Runspeed";
			}

			// Token: 0x02003827 RID: 14375
			public class LUMINESCENCE
			{
				// Token: 0x0400E58A RID: 58762
				public static LocString NAME = "Luminescence";

				// Token: 0x0400E58B RID: 58763
				public static LocString DESC = "Determines how much light a Duplicant emits.";
			}

			// Token: 0x02003828 RID: 14376
			public class TRANSITTUBETRAVELSPEED
			{
				// Token: 0x0400E58C RID: 58764
				public static LocString NAME = "Transit Speed";

				// Token: 0x0400E58D RID: 58765
				public static LocString DESC = "Determines a Duplicant's default " + BUILDINGS.PREFABS.TRAVELTUBE.NAME + " travel speed.";

				// Token: 0x0400E58E RID: 58766
				public static LocString SPEEDMODIFIER = "{0} Transit Tube Travel Speed";
			}

			// Token: 0x02003829 RID: 14377
			public class DOCTOREDLEVEL
			{
				// Token: 0x0400E58F RID: 58767
				public static LocString NAME = UI.FormatAsLink("Treatment Received", "MEDICINE") + " Effect";

				// Token: 0x0400E590 RID: 58768
				public static LocString DESC = string.Concat(new string[]
				{
					"Duplicants who receive medical care while in a ",
					BUILDINGS.PREFABS.DOCTORSTATION.NAME,
					" or ",
					BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME,
					" will gain the ",
					UI.PRE_KEYWORD,
					"Treatment Received",
					UI.PST_KEYWORD,
					" effect\n\nThis effect reduces the severity of ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" symptoms"
				});
			}

			// Token: 0x0200382A RID: 14378
			public class SNEEZYNESS
			{
				// Token: 0x0400E591 RID: 58769
				public static LocString NAME = "Sneeziness";

				// Token: 0x0400E592 RID: 58770
				public static LocString DESC = "Determines how frequently a Duplicant sneezes.";
			}

			// Token: 0x0200382B RID: 14379
			public class GERMRESISTANCE
			{
				// Token: 0x0400E593 RID: 58771
				public static LocString NAME = "Germ Resistance";

				// Token: 0x0400E594 RID: 58772
				public static LocString DESC = string.Concat(new string[]
				{
					"Duplicants with a higher ",
					UI.PRE_KEYWORD,
					"Germ Resistance",
					UI.PST_KEYWORD,
					" rating are less likely to contract germ-based ",
					UI.PRE_KEYWORD,
					"Diseases",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x02003D98 RID: 15768
				public class MODIFIER_DESCRIPTORS
				{
					// Token: 0x0400F316 RID: 62230
					public static LocString NEGATIVE_LARGE = "{0} (Large Loss)";

					// Token: 0x0400F317 RID: 62231
					public static LocString NEGATIVE_MEDIUM = "{0} (Medium Loss)";

					// Token: 0x0400F318 RID: 62232
					public static LocString NEGATIVE_SMALL = "{0} (Small Loss)";

					// Token: 0x0400F319 RID: 62233
					public static LocString NONE = "No Effect";

					// Token: 0x0400F31A RID: 62234
					public static LocString POSITIVE_SMALL = "{0} (Small Boost)";

					// Token: 0x0400F31B RID: 62235
					public static LocString POSITIVE_MEDIUM = "{0} (Medium Boost)";

					// Token: 0x0400F31C RID: 62236
					public static LocString POSITIVE_LARGE = "{0} (Large Boost)";
				}
			}

			// Token: 0x0200382C RID: 14380
			public class LEARNING
			{
				// Token: 0x0400E595 RID: 58773
				public static LocString NAME = "Science";

				// Token: 0x0400E596 RID: 58774
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant conducts ",
					UI.PRE_KEYWORD,
					"Research",
					UI.PST_KEYWORD,
					" and levels up their ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400E597 RID: 58775
				public static LocString SPEEDMODIFIER = "{0} Attribute Leveling Speed";

				// Token: 0x0400E598 RID: 58776
				public static LocString RESEARCHSPEED = "{0} Research Speed";

				// Token: 0x0400E599 RID: 58777
				public static LocString GEOTUNER_SPEED_MODIFIER = "{0} Geotuning Speed";
			}

			// Token: 0x0200382D RID: 14381
			public class COOKING
			{
				// Token: 0x0400E59A RID: 58778
				public static LocString NAME = "Cuisine";

				// Token: 0x0400E59B RID: 58779
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant prepares ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400E59C RID: 58780
				public static LocString SPEEDMODIFIER = "{0} Cooking Speed";
			}

			// Token: 0x0200382E RID: 14382
			public class HAPPINESSDELTA
			{
				// Token: 0x0400E59D RID: 58781
				public static LocString NAME = "Happiness";

				// Token: 0x0400E59E RID: 58782
				public static LocString DESC = "Contented " + UI.FormatAsLink("Critters", "CREATURES") + " produce usable materials with increased frequency.";
			}

			// Token: 0x0200382F RID: 14383
			public class RADIATIONBALANCEDELTA
			{
				// Token: 0x0400E59F RID: 58783
				public static LocString NAME = "Absorbed Radiation Dose";

				// Token: 0x0400E5A0 RID: 58784
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover at very slow rates\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});
			}

			// Token: 0x02003830 RID: 14384
			public class INSULATION
			{
				// Token: 0x0400E5A1 RID: 58785
				public static LocString NAME = "Insulation";

				// Token: 0x0400E5A2 RID: 58786
				public static LocString DESC = string.Concat(new string[]
				{
					"Highly ",
					UI.PRE_KEYWORD,
					"Insulated",
					UI.PST_KEYWORD,
					" Duplicants retain body heat easily, while low ",
					UI.PRE_KEYWORD,
					"Insulation",
					UI.PST_KEYWORD,
					" Duplicants are easier to keep cool."
				});

				// Token: 0x0400E5A3 RID: 58787
				public static LocString SPEEDMODIFIER = "{0} Temperature Retention";
			}

			// Token: 0x02003831 RID: 14385
			public class STRENGTH
			{
				// Token: 0x0400E5A4 RID: 58788
				public static LocString NAME = "Strength";

				// Token: 0x0400E5A5 RID: 58789
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines a Duplicant's ",
					UI.PRE_KEYWORD,
					"Carrying Capacity",
					UI.PST_KEYWORD,
					" and cleaning speed."
				});

				// Token: 0x0400E5A6 RID: 58790
				public static LocString CARRYMODIFIER = "{0} " + DUPLICANTS.ATTRIBUTES.CARRYAMOUNT.NAME;

				// Token: 0x0400E5A7 RID: 58791
				public static LocString SPEEDMODIFIER = "{0} Tidying Speed";
			}

			// Token: 0x02003832 RID: 14386
			public class CARING
			{
				// Token: 0x0400E5A8 RID: 58792
				public static LocString NAME = "Medicine";

				// Token: 0x0400E5A9 RID: 58793
				public static LocString DESC = "Determines a Duplicant's ability to care for sick peers.";

				// Token: 0x0400E5AA RID: 58794
				public static LocString SPEEDMODIFIER = "{0} Treatment Speed";

				// Token: 0x0400E5AB RID: 58795
				public static LocString FABRICATE_SPEEDMODIFIER = "{0} Medicine Fabrication Speed";
			}

			// Token: 0x02003833 RID: 14387
			public class IMMUNITY
			{
				// Token: 0x0400E5AC RID: 58796
				public static LocString NAME = "Immunity";

				// Token: 0x0400E5AD RID: 58797
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines a Duplicant's ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" susceptibility and recovery time."
				});

				// Token: 0x0400E5AE RID: 58798
				public static LocString BOOST_MODIFIER = "{0} Immunity Regen";

				// Token: 0x0400E5AF RID: 58799
				public static LocString BOOST_STAT = "Immunity Attribute";
			}

			// Token: 0x02003834 RID: 14388
			public class BOTANIST
			{
				// Token: 0x0400E5B0 RID: 58800
				public static LocString NAME = "Agriculture";

				// Token: 0x0400E5B1 RID: 58801
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly and efficiently a Duplicant cultivates ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400E5B2 RID: 58802
				public static LocString HARVEST_SPEED_MODIFIER = "{0} Harvesting Speed";

				// Token: 0x0400E5B3 RID: 58803
				public static LocString TINKER_MODIFIER = "{0} Tending Speed";

				// Token: 0x0400E5B4 RID: 58804
				public static LocString BONUS_SEEDS = "{0} Seed Chance";

				// Token: 0x0400E5B5 RID: 58805
				public static LocString TINKER_EFFECT_MODIFIER = "{0} Farmer's Touch Effect Duration";
			}

			// Token: 0x02003835 RID: 14389
			public class RANCHING
			{
				// Token: 0x0400E5B6 RID: 58806
				public static LocString NAME = "Husbandry";

				// Token: 0x0400E5B7 RID: 58807
				public static LocString DESC = "Determines how efficiently a Duplicant tends " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400E5B8 RID: 58808
				public static LocString EFFECTMODIFIER = "{0} Groom Effect Duration";

				// Token: 0x0400E5B9 RID: 58809
				public static LocString CAPTURABLESPEED = "{0} Wrangling Speed";
			}

			// Token: 0x02003836 RID: 14390
			public class ART
			{
				// Token: 0x0400E5BA RID: 58810
				public static LocString NAME = "Creativity";

				// Token: 0x0400E5BB RID: 58811
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant produces ",
					UI.PRE_KEYWORD,
					"Artwork",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400E5BC RID: 58812
				public static LocString SPEEDMODIFIER = "{0} Decorating Speed";
			}

			// Token: 0x02003837 RID: 14391
			public class DECOR
			{
				// Token: 0x0400E5BD RID: 58813
				public static LocString NAME = "Decor";

				// Token: 0x0400E5BE RID: 58814
				public static LocString DESC = string.Concat(new string[]
				{
					"Affects a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" and their opinion of their surroundings."
				});
			}

			// Token: 0x02003838 RID: 14392
			public class THERMALCONDUCTIVITYBARRIER
			{
				// Token: 0x0400E5BF RID: 58815
				public static LocString NAME = "Insulation Thickness";

				// Token: 0x0400E5C0 RID: 58816
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant retains or loses body ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" in any given area.\n\nIt is the sum of a Duplicant's ",
					UI.PRE_KEYWORD,
					"Equipment",
					UI.PST_KEYWORD,
					" and their natural ",
					UI.PRE_KEYWORD,
					"Insulation",
					UI.PST_KEYWORD,
					" values."
				});
			}

			// Token: 0x02003839 RID: 14393
			public class DECORRADIUS
			{
				// Token: 0x0400E5C1 RID: 58817
				public static LocString NAME = "Decor Radius";

				// Token: 0x0400E5C2 RID: 58818
				public static LocString DESC = string.Concat(new string[]
				{
					"The influence range of an object's ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" value."
				});
			}

			// Token: 0x0200383A RID: 14394
			public class DECOREXPECTATION
			{
				// Token: 0x0400E5C3 RID: 58819
				public static LocString NAME = "Decor Morale Bonus";

				// Token: 0x0400E5C4 RID: 58820
				public static LocString DESC = string.Concat(new string[]
				{
					"A Decor Morale Bonus allows Duplicants to receive ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" boosts from lower ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values.\n\nMaintaining high ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will allow Duplicants to learn more ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x0200383B RID: 14395
			public class FOODEXPECTATION
			{
				// Token: 0x0400E5C5 RID: 58821
				public static LocString NAME = "Food Morale Bonus";

				// Token: 0x0400E5C6 RID: 58822
				public static LocString DESC = string.Concat(new string[]
				{
					"A Food Morale Bonus allows Duplicants to receive ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" boosts from lower quality ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					".\n\nMaintaining high ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will allow Duplicants to learn more ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x0200383C RID: 14396
			public class QUALITYOFLIFEEXPECTATION
			{
				// Token: 0x0400E5C7 RID: 58823
				public static LocString NAME = "Morale Need";

				// Token: 0x0400E5C8 RID: 58824
				public static LocString DESC = string.Concat(new string[]
				{
					"Dictates how high a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" must be kept to prevent them from gaining ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200383D RID: 14397
			public class HYGIENE
			{
				// Token: 0x0400E5C9 RID: 58825
				public static LocString NAME = "Hygiene";

				// Token: 0x0400E5CA RID: 58826
				public static LocString DESC = "Affects a Duplicant's sense of cleanliness.";
			}

			// Token: 0x0200383E RID: 14398
			public class CARRYAMOUNT
			{
				// Token: 0x0400E5CB RID: 58827
				public static LocString NAME = "Carrying Capacity";

				// Token: 0x0400E5CC RID: 58828
				public static LocString DESC = "Determines the maximum weight that a Duplicant can carry.";
			}

			// Token: 0x0200383F RID: 14399
			public class SPACENAVIGATION
			{
				// Token: 0x0400E5CD RID: 58829
				public static LocString NAME = "Piloting";

				// Token: 0x0400E5CE RID: 58830
				public static LocString DESC = "Determines how long it takes a Duplicant to complete a space mission.";

				// Token: 0x0400E5CF RID: 58831
				public static LocString DLC1_DESC = "Determines how much of a speed bonus a Duplicant provides to a rocket they are piloting.";

				// Token: 0x0400E5D0 RID: 58832
				public static LocString SPEED_MODIFIER = "{0} Rocket Speed";
			}

			// Token: 0x02003840 RID: 14400
			public class QUALITYOFLIFE
			{
				// Token: 0x0400E5D1 RID: 58833
				public static LocString NAME = "Morale";

				// Token: 0x0400E5D2 RID: 58834
				public static LocString DESC = string.Concat(new string[]
				{
					"A Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" must exceed their ",
					UI.PRE_KEYWORD,
					"Morale Need",
					UI.PST_KEYWORD,
					", or they'll begin to accumulate ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					".\n\n",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" can be increased by providing Duplicants higher quality ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					", allotting more ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" in\nthe colony schedule, or building better ",
					UI.PRE_KEYWORD,
					"Bathrooms",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Bedrooms",
					UI.PST_KEYWORD,
					" for them to live in."
				});

				// Token: 0x0400E5D3 RID: 58835
				public static LocString DESC_FORMAT = "{0} / {1}";

				// Token: 0x0400E5D4 RID: 58836
				public static LocString TOOLTIP_EXPECTATION = "Total <b>Morale Need</b>: {0}\n    • Skills Learned: +{0}";

				// Token: 0x0400E5D5 RID: 58837
				public static LocString TOOLTIP_EXPECTATION_OVER = "This Duplicant has sufficiently high " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;

				// Token: 0x0400E5D6 RID: 58838
				public static LocString TOOLTIP_EXPECTATION_UNDER = string.Concat(new string[]
				{
					"This Duplicant's low ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will cause ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" over time"
				});
			}

			// Token: 0x02003841 RID: 14401
			public class AIRCONSUMPTIONRATE
			{
				// Token: 0x0400E5D7 RID: 58839
				public static LocString NAME = "Air Consumption Rate";

				// Token: 0x0400E5D8 RID: 58840
				public static LocString DESC = "Air Consumption determines how much " + ELEMENTS.OXYGEN.NAME + " a Duplicant requires per minute to live.";
			}

			// Token: 0x02003842 RID: 14402
			public class RADIATIONRESISTANCE
			{
				// Token: 0x0400E5D9 RID: 58841
				public static LocString NAME = "Radiation Resistance";

				// Token: 0x0400E5DA RID: 58842
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how easily a Duplicant repels ",
					UI.PRE_KEYWORD,
					"Radiation Sickness",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02003843 RID: 14403
			public class RADIATIONRECOVERY
			{
				// Token: 0x0400E5DB RID: 58843
				public static LocString NAME = "Radiation Absorption";

				// Token: 0x0400E5DC RID: 58844
				public static LocString DESC = string.Concat(new string[]
				{
					"The rate at which ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is neutralized within a Duplicant body."
				});
			}

			// Token: 0x02003844 RID: 14404
			public class STRESSDELTA
			{
				// Token: 0x0400E5DD RID: 58845
				public static LocString NAME = "Stress";

				// Token: 0x0400E5DE RID: 58846
				public static LocString DESC = "Determines how quickly a Duplicant gains or reduces " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003845 RID: 14405
			public class BREATHDELTA
			{
				// Token: 0x0400E5DF RID: 58847
				public static LocString NAME = "Breath";

				// Token: 0x0400E5E0 RID: 58848
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant gains or reduces ",
					UI.PRE_KEYWORD,
					"Breath",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02003846 RID: 14406
			public class BIONICOILDELTA
			{
				// Token: 0x0400E5E1 RID: 58849
				public static LocString NAME = "Gear Oil";

				// Token: 0x0400E5E2 RID: 58850
				public static LocString DESC = "Determines how quickly a Duplicant's bionic parts lose " + UI.PRE_KEYWORD + "Gear Oil" + UI.PST_KEYWORD;
			}

			// Token: 0x02003847 RID: 14407
			public class BLADDERDELTA
			{
				// Token: 0x0400E5E3 RID: 58851
				public static LocString NAME = "Bladder";

				// Token: 0x0400E5E4 RID: 58852
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant's ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					" fills or depletes."
				});
			}

			// Token: 0x02003848 RID: 14408
			public class CALORIESDELTA
			{
				// Token: 0x0400E5E5 RID: 58853
				public static LocString NAME = "Calories";

				// Token: 0x0400E5E6 RID: 58854
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant burns or stores ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02003849 RID: 14409
			public class STAMINADELTA
			{
				// Token: 0x0400E5E7 RID: 58855
				public static LocString NAME = "Stamina";

				// Token: 0x0400E5E8 RID: 58856
				public static LocString DESC = "";
			}

			// Token: 0x0200384A RID: 14410
			public class TOXICITYDELTA
			{
				// Token: 0x0400E5E9 RID: 58857
				public static LocString NAME = "Toxicity";

				// Token: 0x0400E5EA RID: 58858
				public static LocString DESC = "";
			}

			// Token: 0x0200384B RID: 14411
			public class IMMUNELEVELDELTA
			{
				// Token: 0x0400E5EB RID: 58859
				public static LocString NAME = "Immunity";

				// Token: 0x0400E5EC RID: 58860
				public static LocString DESC = "";
			}

			// Token: 0x0200384C RID: 14412
			public class TOILETEFFICIENCY
			{
				// Token: 0x0400E5ED RID: 58861
				public static LocString NAME = "Bathroom Use Speed";

				// Token: 0x0400E5EE RID: 58862
				public static LocString DESC = "Determines how long a Duplicant needs to do their \"business\".";

				// Token: 0x0400E5EF RID: 58863
				public static LocString SPEEDMODIFIER = "{0} Bathroom Use Speed";
			}

			// Token: 0x0200384D RID: 14413
			public class METABOLISM
			{
				// Token: 0x0400E5F0 RID: 58864
				public static LocString NAME = "Critter Metabolism";

				// Token: 0x0400E5F1 RID: 58865
				public static LocString DESC = string.Concat(new string[]
				{
					"Affects the rate at which a critter burns ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD,
					" and produces materials"
				});
			}

			// Token: 0x0200384E RID: 14414
			public class ROOMTEMPERATUREPREFERENCE
			{
				// Token: 0x0400E5F2 RID: 58866
				public static LocString NAME = "Temperature Preference";

				// Token: 0x0400E5F3 RID: 58867
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the minimum body ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" a Duplicant prefers to maintain."
				});
			}

			// Token: 0x0200384F RID: 14415
			public class MAXUNDERWATERTRAVELCOST
			{
				// Token: 0x0400E5F4 RID: 58868
				public static LocString NAME = "Underwater Movement";

				// Token: 0x0400E5F5 RID: 58869
				public static LocString DESC = "Determines a Duplicant's runspeed when submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;
			}

			// Token: 0x02003850 RID: 14416
			public class OVERHEATTEMPERATURE
			{
				// Token: 0x0400E5F6 RID: 58870
				public static LocString NAME = "Overheat Temperature";

				// Token: 0x0400E5F7 RID: 58871
				public static LocString DESC = string.Concat(new string[]
				{
					"A building at Overheat ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will take damage and break down if not cooled"
				});
			}

			// Token: 0x02003851 RID: 14417
			public class FATALTEMPERATURE
			{
				// Token: 0x0400E5F8 RID: 58872
				public static LocString NAME = "Break Down Temperature";

				// Token: 0x0400E5F9 RID: 58873
				public static LocString DESC = string.Concat(new string[]
				{
					"A building at break down ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will lose functionality and take damage"
				});
			}

			// Token: 0x02003852 RID: 14418
			public class HITPOINTSDELTA
			{
				// Token: 0x0400E5FA RID: 58874
				public static LocString NAME = UI.FormatAsLink("Health", "HEALTH");

				// Token: 0x0400E5FB RID: 58875
				public static LocString DESC = "Health regeneration is increased when another Duplicant provides medical care to the patient";
			}

			// Token: 0x02003853 RID: 14419
			public class DISEASECURESPEED
			{
				// Token: 0x0400E5FC RID: 58876
				public static LocString NAME = UI.FormatAsLink("Disease", "DISEASE") + " Recovery Speed Bonus";

				// Token: 0x0400E5FD RID: 58877
				public static LocString DESC = "Recovery speed bonus is increased when another Duplicant provides medical care to the patient";
			}

			// Token: 0x02003854 RID: 14420
			public abstract class MACHINERYSPEED
			{
				// Token: 0x0400E5FE RID: 58878
				public static LocString NAME = "Machinery Speed";

				// Token: 0x0400E5FF RID: 58879
				public static LocString DESC = "Speed Bonus";
			}

			// Token: 0x02003855 RID: 14421
			public abstract class GENERATOROUTPUT
			{
				// Token: 0x0400E600 RID: 58880
				public static LocString NAME = "Power Output";
			}

			// Token: 0x02003856 RID: 14422
			public abstract class ROCKETBURDEN
			{
				// Token: 0x0400E601 RID: 58881
				public static LocString NAME = "Burden";
			}

			// Token: 0x02003857 RID: 14423
			public abstract class ROCKETENGINEPOWER
			{
				// Token: 0x0400E602 RID: 58882
				public static LocString NAME = "Engine Power";
			}

			// Token: 0x02003858 RID: 14424
			public abstract class FUELRANGEPERKILOGRAM
			{
				// Token: 0x0400E603 RID: 58883
				public static LocString NAME = "Range";
			}

			// Token: 0x02003859 RID: 14425
			public abstract class HEIGHT
			{
				// Token: 0x0400E604 RID: 58884
				public static LocString NAME = "Height";
			}

			// Token: 0x0200385A RID: 14426
			public class WILTTEMPRANGEMOD
			{
				// Token: 0x0400E605 RID: 58885
				public static LocString NAME = "Viable Temperature Range";

				// Token: 0x0400E606 RID: 58886
				public static LocString DESC = "Variance growth temperature relative to the base crop";
			}

			// Token: 0x0200385B RID: 14427
			public class YIELDAMOUNT
			{
				// Token: 0x0400E607 RID: 58887
				public static LocString NAME = "Yield Amount";

				// Token: 0x0400E608 RID: 58888
				public static LocString DESC = "Plant production relative to the base crop";
			}

			// Token: 0x0200385C RID: 14428
			public class HARVESTTIME
			{
				// Token: 0x0400E609 RID: 58889
				public static LocString NAME = "Harvest Duration";

				// Token: 0x0400E60A RID: 58890
				public static LocString DESC = "Time it takes an unskilled Duplicant to harvest this plant";
			}

			// Token: 0x0200385D RID: 14429
			public class DECORBONUS
			{
				// Token: 0x0400E60B RID: 58891
				public static LocString NAME = "Decor Bonus";

				// Token: 0x0400E60C RID: 58892
				public static LocString DESC = "Change in Decor value relative to the base crop";
			}

			// Token: 0x0200385E RID: 14430
			public class MINLIGHTLUX
			{
				// Token: 0x0400E60D RID: 58893
				public static LocString NAME = "Light";

				// Token: 0x0400E60E RID: 58894
				public static LocString DESC = "Minimum lux this plant requires for growth";
			}

			// Token: 0x0200385F RID: 14431
			public class FERTILIZERUSAGEMOD
			{
				// Token: 0x0400E60F RID: 58895
				public static LocString NAME = "Fertilizer Usage";

				// Token: 0x0400E610 RID: 58896
				public static LocString DESC = "Fertilizer and irrigation amounts this plant requires relative to the base crop";
			}

			// Token: 0x02003860 RID: 14432
			public class MINRADIATIONTHRESHOLD
			{
				// Token: 0x0400E611 RID: 58897
				public static LocString NAME = "Minimum Radiation";

				// Token: 0x0400E612 RID: 58898
				public static LocString DESC = "Smallest amount of ambient Radiation required for this plant to grow";
			}

			// Token: 0x02003861 RID: 14433
			public class MAXRADIATIONTHRESHOLD
			{
				// Token: 0x0400E613 RID: 58899
				public static LocString NAME = "Maximum Radiation";

				// Token: 0x0400E614 RID: 58900
				public static LocString DESC = "Largest amount of ambient Radiation this plant can tolerate";
			}

			// Token: 0x02003862 RID: 14434
			public class BIONICBOOSTERSLOTS
			{
				// Token: 0x0400E615 RID: 58901
				public static LocString NAME = "Booster Slots";

				// Token: 0x0400E616 RID: 58902
				public static LocString DESC = "The number of boosters this Bionic Duplicant can install at once";
			}

			// Token: 0x02003863 RID: 14435
			public class BIONICBATTERYCOUNTCAPACITY
			{
				// Token: 0x0400E617 RID: 58903
				public static LocString NAME = "Power Banks";

				// Token: 0x0400E618 RID: 58904
				public static LocString DESC = "The number of power banks this Bionic Duplicant can store";
			}
		}

		// Token: 0x0200258B RID: 9611
		public class ROLES
		{
			// Token: 0x02003864 RID: 14436
			public class GROUPS
			{
				// Token: 0x0400E619 RID: 58905
				public static LocString APTITUDE_DESCRIPTION = string.Concat(new string[]
				{
					"This Duplicant will gain <b>{1}</b> ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when learning ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" Skills"
				});

				// Token: 0x0400E61A RID: 58906
				public static LocString APTITUDE_DESCRIPTION_CHOREGROUP = string.Concat(new string[]
				{
					"{2}\n\nThis Duplicant will gain <b>+{1}</b> ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when learning ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" Skills"
				});

				// Token: 0x0400E61B RID: 58907
				public static LocString SUITS = "Suit Wearing";
			}

			// Token: 0x02003865 RID: 14437
			public class NO_ROLE
			{
				// Token: 0x0400E61C RID: 58908
				public static LocString NAME = UI.FormatAsLink("Unemployed", "NO_ROLE");

				// Token: 0x0400E61D RID: 58909
				public static LocString DESCRIPTION = "No job assignment";
			}

			// Token: 0x02003866 RID: 14438
			public class JUNIOR_ARTIST
			{
				// Token: 0x0400E61E RID: 58910
				public static LocString NAME = UI.FormatAsLink("Art Fundamentals", "ARTING1");

				// Token: 0x0400E61F RID: 58911
				public static LocString DESCRIPTION = "Teaches the most basic level of art skill";
			}

			// Token: 0x02003867 RID: 14439
			public class ARTIST
			{
				// Token: 0x0400E620 RID: 58912
				public static LocString NAME = UI.FormatAsLink("Aesthetic Design", "ARTING2");

				// Token: 0x0400E621 RID: 58913
				public static LocString DESCRIPTION = "Allows moderately attractive art to be created";
			}

			// Token: 0x02003868 RID: 14440
			public class MASTER_ARTIST
			{
				// Token: 0x0400E622 RID: 58914
				public static LocString NAME = UI.FormatAsLink("Masterworks", "ARTING3");

				// Token: 0x0400E623 RID: 58915
				public static LocString DESCRIPTION = "Enables the painting and sculpting of masterpieces";
			}

			// Token: 0x02003869 RID: 14441
			public class JUNIOR_BUILDER
			{
				// Token: 0x0400E624 RID: 58916
				public static LocString NAME = UI.FormatAsLink("Improved Construction I", "BUILDING1");

				// Token: 0x0400E625 RID: 58917
				public static LocString DESCRIPTION = "Marginally improves a Duplicant's construction speeds";
			}

			// Token: 0x0200386A RID: 14442
			public class BUILDER
			{
				// Token: 0x0400E626 RID: 58918
				public static LocString NAME = UI.FormatAsLink("Improved Construction II", "BUILDING2");

				// Token: 0x0400E627 RID: 58919
				public static LocString DESCRIPTION = "Further increases a Duplicant's construction speeds";
			}

			// Token: 0x0200386B RID: 14443
			public class SENIOR_BUILDER
			{
				// Token: 0x0400E628 RID: 58920
				public static LocString NAME = UI.FormatAsLink("Demolition", "BUILDING3");

				// Token: 0x0400E629 RID: 58921
				public static LocString DESCRIPTION = "Enables a Duplicant to deconstruct Gravitas buildings";
			}

			// Token: 0x0200386C RID: 14444
			public class JUNIOR_RESEARCHER
			{
				// Token: 0x0400E62A RID: 58922
				public static LocString NAME = UI.FormatAsLink("Advanced Research", "RESEARCHING1");

				// Token: 0x0400E62B RID: 58923
				public static LocString DESCRIPTION = "Allows Duplicants to perform research using a " + BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME;
			}

			// Token: 0x0200386D RID: 14445
			public class RESEARCHER
			{
				// Token: 0x0400E62C RID: 58924
				public static LocString NAME = UI.FormatAsLink("Field Research", "RESEARCHING2");

				// Token: 0x0400E62D RID: 58925
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Duplicants can perform studies on ",
					UI.PRE_KEYWORD,
					"Geysers",
					UI.PST_KEYWORD,
					", ",
					UI.CLUSTERMAP.PLANETOID_KEYWORD,
					", and other geographical phenomena"
				});
			}

			// Token: 0x0200386E RID: 14446
			public class SENIOR_RESEARCHER
			{
				// Token: 0x0400E62E RID: 58926
				public static LocString NAME = UI.FormatAsLink("Astronomy", "ASTRONOMY");

				// Token: 0x0400E62F RID: 58927
				public static LocString DESCRIPTION = "Enables Duplicants to study outer space using the " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME;
			}

			// Token: 0x0200386F RID: 14447
			public class NUCLEAR_RESEARCHER
			{
				// Token: 0x0400E630 RID: 58928
				public static LocString NAME = UI.FormatAsLink("Applied Sciences Research", "ATOMICRESEARCH");

				// Token: 0x0400E631 RID: 58929
				public static LocString DESCRIPTION = "Enables Duplicants to study matter using the " + BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME;
			}

			// Token: 0x02003870 RID: 14448
			public class SPACE_RESEARCHER
			{
				// Token: 0x0400E632 RID: 58930
				public static LocString NAME = UI.FormatAsLink("Data Analysis Researcher", "SPACERESEARCH");

				// Token: 0x0400E633 RID: 58931
				public static LocString DESCRIPTION = "Enables Duplicants to conduct research using the " + BUILDINGS.PREFABS.DLC1COSMICRESEARCHCENTER.NAME;
			}

			// Token: 0x02003871 RID: 14449
			public class JUNIOR_COOK
			{
				// Token: 0x0400E634 RID: 58932
				public static LocString NAME = UI.FormatAsLink("Grilling", "COOKING1");

				// Token: 0x0400E635 RID: 58933
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows Duplicants to cook using the ",
					BUILDINGS.PREFABS.COOKINGSTATION.NAME,
					", ",
					BUILDINGS.PREFABS.GOURMETCOOKINGSTATION.NAME,
					", and ",
					BUILDINGS.PREFABS.DEEPFRYER.NAME
				});
			}

			// Token: 0x02003872 RID: 14450
			public class COOK
			{
				// Token: 0x0400E636 RID: 58934
				public static LocString NAME = UI.FormatAsLink("Grilling II", "COOKING2");

				// Token: 0x0400E637 RID: 58935
				public static LocString DESCRIPTION = "Improves a Duplicant's cooking speed";
			}

			// Token: 0x02003873 RID: 14451
			public class JUNIOR_MEDIC
			{
				// Token: 0x0400E638 RID: 58936
				public static LocString NAME = UI.FormatAsLink("Medicine Compounding", "MEDICINE1");

				// Token: 0x0400E639 RID: 58937
				public static LocString DESCRIPTION = "Allows Duplicants to produce medicines at the " + BUILDINGS.PREFABS.APOTHECARY.NAME;
			}

			// Token: 0x02003874 RID: 14452
			public class MEDIC
			{
				// Token: 0x0400E63A RID: 58938
				public static LocString NAME = UI.FormatAsLink("Bedside Manner", "MEDICINE2");

				// Token: 0x0400E63B RID: 58939
				public static LocString DESCRIPTION = "Trains Duplicants to administer medicine at the " + BUILDINGS.PREFABS.DOCTORSTATION.NAME;
			}

			// Token: 0x02003875 RID: 14453
			public class SENIOR_MEDIC
			{
				// Token: 0x0400E63C RID: 58940
				public static LocString NAME = UI.FormatAsLink("Advanced Medical Care", "MEDICINE3");

				// Token: 0x0400E63D RID: 58941
				public static LocString DESCRIPTION = "Trains Duplicants to operate the " + BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME;
			}

			// Token: 0x02003876 RID: 14454
			public class MACHINE_TECHNICIAN
			{
				// Token: 0x0400E63E RID: 58942
				public static LocString NAME = UI.FormatAsLink("Improved Tinkering", "TECHNICALS1");

				// Token: 0x0400E63F RID: 58943
				public static LocString DESCRIPTION = "Marginally improves a Duplicant's tinkering speeds";
			}

			// Token: 0x02003877 RID: 14455
			public class OIL_TECHNICIAN
			{
				// Token: 0x0400E640 RID: 58944
				public static LocString NAME = UI.FormatAsLink("Oil Engineering", "OIL_TECHNICIAN");

				// Token: 0x0400E641 RID: 58945
				public static LocString DESCRIPTION = "Allows the extraction and refinement of " + ELEMENTS.CRUDEOIL.NAME;
			}

			// Token: 0x02003878 RID: 14456
			public class HAULER
			{
				// Token: 0x0400E642 RID: 58946
				public static LocString NAME = UI.FormatAsLink("Improved Carrying I", "HAULING1");

				// Token: 0x0400E643 RID: 58947
				public static LocString DESCRIPTION = "Minorly increase a Duplicant's strength and carrying capacity";
			}

			// Token: 0x02003879 RID: 14457
			public class MATERIALS_MANAGER
			{
				// Token: 0x0400E644 RID: 58948
				public static LocString NAME = UI.FormatAsLink("Improved Carrying II", "HAULING2");

				// Token: 0x0400E645 RID: 58949
				public static LocString DESCRIPTION = "Further increases a Duplicant's strength and carrying capacity for even swifter deliveries";
			}

			// Token: 0x0200387A RID: 14458
			public class JUNIOR_FARMER
			{
				// Token: 0x0400E646 RID: 58950
				public static LocString NAME = UI.FormatAsLink("Improved Farming I", "FARMING1");

				// Token: 0x0400E647 RID: 58951
				public static LocString DESCRIPTION = "Minorly increase a Duplicant's farming skills, increasing their chances of harvesting new plant " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;
			}

			// Token: 0x0200387B RID: 14459
			public class FARMER
			{
				// Token: 0x0400E648 RID: 58952
				public static LocString NAME = UI.FormatAsLink("Crop Tending", "FARMING2");

				// Token: 0x0400E649 RID: 58953
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Enables tending ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					", which will increase their growth speed"
				});
			}

			// Token: 0x0200387C RID: 14460
			public class SENIOR_FARMER
			{
				// Token: 0x0400E64A RID: 58954
				public static LocString NAME = UI.FormatAsLink("Improved Farming II", "FARMING3");

				// Token: 0x0400E64B RID: 58955
				public static LocString DESCRIPTION = "Enables a Duplicant to gather " + ITEMS.INDUSTRIAL_PRODUCTS.PLANT_FIBER.NAME + "s as a byproduct when harvesting certain " + UI.CODEX.CATEGORYNAMES.PLANTS;
			}

			// Token: 0x0200387D RID: 14461
			public class JUNIOR_MINER
			{
				// Token: 0x0400E64C RID: 58956
				public static LocString NAME = UI.FormatAsLink("Hard Digging", "MINING1");

				// Token: 0x0400E64D RID: 58957
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows the excavation of ",
					UI.PRE_KEYWORD,
					ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM,
					UI.PST_KEYWORD,
					" materials such as ",
					ELEMENTS.GRANITE.NAME
				});
			}

			// Token: 0x0200387E RID: 14462
			public class MINER
			{
				// Token: 0x0400E64E RID: 58958
				public static LocString NAME = UI.FormatAsLink("Superhard Digging", "MINING2");

				// Token: 0x0400E64F RID: 58959
				public static LocString DESCRIPTION = "Allows the excavation of the element " + ELEMENTS.KATAIRITE.NAME;
			}

			// Token: 0x0200387F RID: 14463
			public class SENIOR_MINER
			{
				// Token: 0x0400E650 RID: 58960
				public static LocString NAME = UI.FormatAsLink("Super-Duperhard Digging", "MINING3");

				// Token: 0x0400E651 RID: 58961
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows the excavation of ",
					UI.PRE_KEYWORD,
					ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE,
					UI.PST_KEYWORD,
					" elements, including ",
					ELEMENTS.DIAMOND.NAME,
					" and ",
					ELEMENTS.OBSIDIAN.NAME
				});
			}

			// Token: 0x02003880 RID: 14464
			public class MASTER_MINER
			{
				// Token: 0x0400E652 RID: 58962
				public static LocString NAME = UI.FormatAsLink("Hazmat Digging", "MINING4");

				// Token: 0x0400E653 RID: 58963
				public static LocString DESCRIPTION = "Allows the excavation of dangerous materials like " + ELEMENTS.CORIUM.NAME;
			}

			// Token: 0x02003881 RID: 14465
			public class SUIT_DURABILITY
			{
				// Token: 0x0400E654 RID: 58964
				public static LocString NAME = UI.FormatAsLink("Suit Sustainability Training", "SUITDURABILITY");

				// Token: 0x0400E655 RID: 58965
				public static LocString DESCRIPTION = "Suits equipped by this Duplicant lose durability " + GameUtil.GetFormattedPercent(EQUIPMENT.SUITS.SUIT_DURABILITY_SKILL_BONUS * 100f, GameUtil.TimeSlice.None) + " slower.";
			}

			// Token: 0x02003882 RID: 14466
			public class SUIT_EXPERT
			{
				// Token: 0x0400E656 RID: 58966
				public static LocString NAME = UI.FormatAsLink("Exosuit Training", "SUITS1");

				// Token: 0x0400E657 RID: 58967
				public static LocString DESCRIPTION = "Eliminates the runspeed loss experienced while wearing exosuits";
			}

			// Token: 0x02003883 RID: 14467
			public class POWER_TECHNICIAN
			{
				// Token: 0x0400E658 RID: 58968
				public static LocString NAME = UI.FormatAsLink("Electrical Engineering", "TECHNICALS2");

				// Token: 0x0400E659 RID: 58969
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Enables generator ",
					UI.PRE_KEYWORD,
					"Tune-Up",
					UI.PST_KEYWORD,
					", which will temporarily provide improved ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" output"
				});
			}

			// Token: 0x02003884 RID: 14468
			public class MECHATRONIC_ENGINEER
			{
				// Token: 0x0400E65A RID: 58970
				public static LocString NAME = UI.FormatAsLink("Mechatronics Engineering", "ENGINEERING1");

				// Token: 0x0400E65B RID: 58971
				public static LocString DESCRIPTION = "Allows the construction and maintenance of " + BUILDINGS.PREFABS.SOLIDCONDUIT.NAME + " systems";
			}

			// Token: 0x02003885 RID: 14469
			public class HANDYMAN
			{
				// Token: 0x0400E65C RID: 58972
				public static LocString NAME = UI.FormatAsLink("Improved Strength", "BASEKEEPING1");

				// Token: 0x0400E65D RID: 58973
				public static LocString DESCRIPTION = "Minorly improves a Duplicant's physical strength";
			}

			// Token: 0x02003886 RID: 14470
			public class PLUMBER
			{
				// Token: 0x0400E65E RID: 58974
				public static LocString NAME = UI.FormatAsLink("Plumbing", "BASEKEEPING2");

				// Token: 0x0400E65F RID: 58975
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows a Duplicant to empty ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" without making a mess"
				});
			}

			// Token: 0x02003887 RID: 14471
			public class PYROTECHNIC
			{
				// Token: 0x0400E660 RID: 58976
				public static LocString NAME = UI.FormatAsLink("Pyrotechnics", "PYROTECHNICS");

				// Token: 0x0400E661 RID: 58977
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows a Duplicant to make ",
					UI.PRE_KEYWORD,
					"Blastshot",
					UI.PST_KEYWORD,
					" for the ",
					UI.PRE_KEYWORD,
					"Meteor Blaster",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003888 RID: 14472
			public class RANCHER
			{
				// Token: 0x0400E662 RID: 58978
				public static LocString NAME = UI.FormatAsLink("Critter Ranching I", "RANCHING1");

				// Token: 0x0400E663 RID: 58979
				public static LocString DESCRIPTION = "Allows a Duplicant to handle and care for " + UI.FormatAsLink("Critters", "CREATURES");
			}

			// Token: 0x02003889 RID: 14473
			public class SENIOR_RANCHER
			{
				// Token: 0x0400E664 RID: 58980
				public static LocString NAME = UI.FormatAsLink("Critter Ranching II", "RANCHING2");

				// Token: 0x0400E665 RID: 58981
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Improves a Duplicant's ",
					UI.PRE_KEYWORD,
					"Ranching",
					UI.PST_KEYWORD,
					" skills"
				});
			}

			// Token: 0x0200388A RID: 14474
			public class ASTRONAUTTRAINEE
			{
				// Token: 0x0400E666 RID: 58982
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting", "ASTRONAUTING1");

				// Token: 0x0400E667 RID: 58983
				public static LocString DESCRIPTION = "Allows a Duplicant to operate a " + BUILDINGS.PREFABS.COMMANDMODULE.NAME + " to pilot a rocket ship";
			}

			// Token: 0x0200388B RID: 14475
			public class ASTRONAUT
			{
				// Token: 0x0400E668 RID: 58984
				public static LocString NAME = UI.FormatAsLink("Rocket Navigation", "ASTRONAUTING2");

				// Token: 0x0400E669 RID: 58985
				public static LocString DESCRIPTION = "Improves the speed that space missions are completed";
			}

			// Token: 0x0200388C RID: 14476
			public class ROCKETPILOT
			{
				// Token: 0x0400E66A RID: 58986
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1");

				// Token: 0x0400E66B RID: 58987
				public static LocString DESCRIPTION = "Allows a Duplicant to operate a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME + " and pilot rockets";
			}

			// Token: 0x0200388D RID: 14477
			public class SENIOR_ROCKETPILOT
			{
				// Token: 0x0400E66C RID: 58988
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting II", "ROCKETPILOTING2");

				// Token: 0x0400E66D RID: 58989
				public static LocString DESCRIPTION = "Allows Duplicants to pilot rockets at faster speeds";
			}

			// Token: 0x0200388E RID: 14478
			public class USELESSSKILL
			{
				// Token: 0x0400E66E RID: 58990
				public static LocString NAME = "W.I.P. Skill";

				// Token: 0x0400E66F RID: 58991
				public static LocString DESCRIPTION = "This skill doesn't really do anything right now.";
			}

			// Token: 0x0200388F RID: 14479
			public class BIONICS_A1
			{
				// Token: 0x0400E670 RID: 58992
				public static LocString NAME = UI.FormatAsLink("Booster Processing I", "BIONICS_A1");

				// Token: 0x0400E671 RID: 58993
				public static LocString DESCRIPTION = "Allows Bionic Duplicants to install an additional booster.";
			}

			// Token: 0x02003890 RID: 14480
			public class BIONICS_A2
			{
				// Token: 0x0400E672 RID: 58994
				public static LocString NAME = UI.FormatAsLink("Booster Processing II", "BIONICS_A2");

				// Token: 0x0400E673 RID: 58995
				public static LocString DESCRIPTION = "Allows Bionic Duplicants to install an additional booster, and increases runspeed.";
			}

			// Token: 0x02003891 RID: 14481
			public class BIONICS_A3
			{
				// Token: 0x0400E674 RID: 58996
				public static LocString NAME = UI.FormatAsLink("Complex Processing", "BIONICS_A3");

				// Token: 0x0400E675 RID: 58997
				public static LocString DESCRIPTION = "Allows Bionic Duplicants to install an additional booster, and reduces the runspeed loss experienced while wearing exosuits.";
			}

			// Token: 0x02003892 RID: 14482
			public class BIONICS_B1
			{
				// Token: 0x0400E676 RID: 58998
				public static LocString NAME = UI.FormatAsLink("Improved Gears I", "BIONICS_B1");

				// Token: 0x0400E677 RID: 58999
				public static LocString DESCRIPTION = "Significantly reduces the negative impacts of low " + UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL") + ".";
			}

			// Token: 0x02003893 RID: 14483
			public class BIONICS_B2
			{
				// Token: 0x0400E678 RID: 59000
				public static LocString NAME = UI.FormatAsLink("Improved Gears II", "BIONICS_B2");

				// Token: 0x0400E679 RID: 59001
				public static LocString DESCRIPTION = "Allows Bionic Duplicants to install an additional booster.";
			}

			// Token: 0x02003894 RID: 14484
			public class BIONICS_B3
			{
				// Token: 0x0400E67A RID: 59002
				public static LocString NAME = UI.FormatAsLink("Top Gear", "BIONICS_B3");

				// Token: 0x0400E67B RID: 59003
				public static LocString DESCRIPTION = "Allows Bionic Duplicants to install an additional booster, and eliminates the runspeed loss experienced while wearing exosuits.";
			}

			// Token: 0x02003895 RID: 14485
			public class BIONICS_C1
			{
				// Token: 0x0400E67C RID: 59004
				public static LocString NAME = UI.FormatAsLink("Schematics", "BIONICS_C1");

				// Token: 0x0400E67D RID: 59005
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows Bionic Duplicants to perform research using a ",
					BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME,
					", and craft items at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME,
					"."
				});
			}

			// Token: 0x02003896 RID: 14486
			public class BIONICS_C2
			{
				// Token: 0x0400E67E RID: 59006
				public static LocString NAME = UI.FormatAsLink("Advanced Schematics", "BIONICS_C2");

				// Token: 0x0400E67F RID: 59007
				public static LocString DESCRIPTION = "Allows Bionic Duplicants to install an additional booster and increase their runspeed.";
			}

			// Token: 0x02003897 RID: 14487
			public class BIONICS_C3
			{
				// Token: 0x0400E680 RID: 59008
				public static LocString NAME = UI.FormatAsLink("Power Banking", "BIONICS_C3");

				// Token: 0x0400E681 RID: 59009
				public static LocString DESCRIPTION = "Increases " + UI.FormatAsLink("Power Bank", "ELECTROBANK") + " storage capacity to maximize work time between replacements.";
			}

			// Token: 0x02003898 RID: 14488
			public class BIONICS_D1
			{
				// Token: 0x0400E682 RID: 59010
				public static LocString NAME = UI.FormatAsLink("Improved Hardware", "BIONICS_D1");

				// Token: 0x0400E683 RID: 59011
				public static LocString DESCRIPTION = "Increases resistance to environmental irritants and pressure-related injuries";
			}

			// Token: 0x02003899 RID: 14489
			public class BIONICS_D2
			{
				// Token: 0x0400E684 RID: 59012
				public static LocString NAME = UI.FormatAsLink("Climate Control", "BIONICS_D2");

				// Token: 0x0400E685 RID: 59013
				public static LocString DESCRIPTION = "Allows Bionic Duplicants to work comfortably in a wider range of ambient temperatures";
			}
		}

		// Token: 0x0200258C RID: 9612
		public class THOUGHTS
		{
			// Token: 0x0200389A RID: 14490
			public class STARVING
			{
				// Token: 0x0400E686 RID: 59014
				public static LocString TOOLTIP = "Starving";
			}

			// Token: 0x0200389B RID: 14491
			public class HOT
			{
				// Token: 0x0400E687 RID: 59015
				public static LocString TOOLTIP = "Hot";
			}

			// Token: 0x0200389C RID: 14492
			public class COLD
			{
				// Token: 0x0400E688 RID: 59016
				public static LocString TOOLTIP = "Cold";
			}

			// Token: 0x0200389D RID: 14493
			public class BREAKBLADDER
			{
				// Token: 0x0400E689 RID: 59017
				public static LocString TOOLTIP = "Washroom Break";
			}

			// Token: 0x0200389E RID: 14494
			public class FULLBLADDER
			{
				// Token: 0x0400E68A RID: 59018
				public static LocString TOOLTIP = "Full Bladder";
			}

			// Token: 0x0200389F RID: 14495
			public class EXPELLGUNKDESIRE
			{
				// Token: 0x0400E68B RID: 59019
				public static LocString TOOLTIP = "Expel Gunk";
			}

			// Token: 0x020038A0 RID: 14496
			public class REFILLOILDESIRE
			{
				// Token: 0x0400E68C RID: 59020
				public static LocString TOOLTIP = "Low Gear Oil";
			}

			// Token: 0x020038A1 RID: 14497
			public class EXPELLINGSPOILEDOIL
			{
				// Token: 0x0400E68D RID: 59021
				public static LocString TOOLTIP = "Spilling Oil";
			}

			// Token: 0x020038A2 RID: 14498
			public class HAPPY
			{
				// Token: 0x0400E68E RID: 59022
				public static LocString TOOLTIP = "Happy";
			}

			// Token: 0x020038A3 RID: 14499
			public class UNHAPPY
			{
				// Token: 0x0400E68F RID: 59023
				public static LocString TOOLTIP = "Unhappy";
			}

			// Token: 0x020038A4 RID: 14500
			public class POORDECOR
			{
				// Token: 0x0400E690 RID: 59024
				public static LocString TOOLTIP = "Poor Decor";
			}

			// Token: 0x020038A5 RID: 14501
			public class POOR_FOOD_QUALITY
			{
				// Token: 0x0400E691 RID: 59025
				public static LocString TOOLTIP = "Lousy Meal";
			}

			// Token: 0x020038A6 RID: 14502
			public class GOOD_FOOD_QUALITY
			{
				// Token: 0x0400E692 RID: 59026
				public static LocString TOOLTIP = "Delicious Meal";
			}

			// Token: 0x020038A7 RID: 14503
			public class SLEEPY
			{
				// Token: 0x0400E693 RID: 59027
				public static LocString TOOLTIP = "Sleepy";
			}

			// Token: 0x020038A8 RID: 14504
			public class DREAMY
			{
				// Token: 0x0400E694 RID: 59028
				public static LocString TOOLTIP = "Dreaming";
			}

			// Token: 0x020038A9 RID: 14505
			public class SUFFOCATING
			{
				// Token: 0x0400E695 RID: 59029
				public static LocString TOOLTIP = "Suffocating";
			}

			// Token: 0x020038AA RID: 14506
			public class ANGRY
			{
				// Token: 0x0400E696 RID: 59030
				public static LocString TOOLTIP = "Angry";
			}

			// Token: 0x020038AB RID: 14507
			public class RAGING
			{
				// Token: 0x0400E697 RID: 59031
				public static LocString TOOLTIP = "Raging";
			}

			// Token: 0x020038AC RID: 14508
			public class GOTINFECTED
			{
				// Token: 0x0400E698 RID: 59032
				public static LocString TOOLTIP = "Got Infected";
			}

			// Token: 0x020038AD RID: 14509
			public class PUTRIDODOUR
			{
				// Token: 0x0400E699 RID: 59033
				public static LocString TOOLTIP = "Smelled Something Putrid";
			}

			// Token: 0x020038AE RID: 14510
			public class NOISY
			{
				// Token: 0x0400E69A RID: 59034
				public static LocString TOOLTIP = "Loud Area";
			}

			// Token: 0x020038AF RID: 14511
			public class NEWROLE
			{
				// Token: 0x0400E69B RID: 59035
				public static LocString TOOLTIP = "New Skill";
			}

			// Token: 0x020038B0 RID: 14512
			public class CHATTY
			{
				// Token: 0x0400E69C RID: 59036
				public static LocString TOOLTIP = "Greeting";
			}

			// Token: 0x020038B1 RID: 14513
			public class ENCOURAGE
			{
				// Token: 0x0400E69D RID: 59037
				public static LocString TOOLTIP = "Encouraging";
			}

			// Token: 0x020038B2 RID: 14514
			public class CONVERSATION
			{
				// Token: 0x0400E69E RID: 59038
				public static LocString TOOLTIP = "Chatting";
			}

			// Token: 0x020038B3 RID: 14515
			public class CATCHYTUNE
			{
				// Token: 0x0400E69F RID: 59039
				public static LocString TOOLTIP = "WHISTLING";
			}
		}
	}
}
