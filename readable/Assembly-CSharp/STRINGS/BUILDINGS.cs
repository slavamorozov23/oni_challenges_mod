using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x02000FEB RID: 4075
	public class BUILDINGS
	{
		// Token: 0x02002250 RID: 8784
		public class PREFABS
		{
			// Token: 0x02002ABB RID: 10939
			public class SHELF
			{
				// Token: 0x0400BC79 RID: 48249
				public static LocString NAME = UI.FormatAsLink("Display Shelf", "SHELF");

				// Token: 0x0400BC7A RID: 48250
				public static LocString DESC = "It looks great even when it's empty.";

				// Token: 0x0400BC7B RID: 48251
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Displays cherished items and increases ",
					CODEX.DECORSYSTEM.TITLE,
					", contributing to ",
					CODEX.MORALE.TITLE,
					".\n\nMust be installed on a back wall."
				});
			}

			// Token: 0x02002ABC RID: 10940
			public class FOSSILSCULPTURE
			{
				// Token: 0x0400BC7C RID: 48252
				public static LocString NAME = UI.FormatAsLink("Fossil Block", "FOSSILSCULPTURE");

				// Token: 0x0400BC7D RID: 48253
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x0400BC7E RID: 48254
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});
			}

			// Token: 0x02002ABD RID: 10941
			public class CEILINGFOSSILSCULPTURE
			{
				// Token: 0x0400BC7F RID: 48255
				public static LocString NAME = UI.FormatAsLink("Hanging Fossil Block", "CEILINGFOSSILSCULPTURE");

				// Token: 0x0400BC80 RID: 48256
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative ceiling sculptures.";

				// Token: 0x0400BC81 RID: 48257
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});
			}

			// Token: 0x02002ABE RID: 10942
			public class HEADQUARTERSCOMPLETE
			{
				// Token: 0x0400BC82 RID: 48258
				public static LocString NAME = UI.FormatAsLink("Printing Pod", "HEADQUARTERS");

				// Token: 0x0400BC83 RID: 48259
				public static LocString UNIQUE_POPTEXT = "A clone of the cloning machine? What a novel thought.\n\nAlas, it won't work.";
			}

			// Token: 0x02002ABF RID: 10943
			public class EXOBASEHEADQUARTERS
			{
				// Token: 0x0400BC84 RID: 48260
				public static LocString NAME = UI.FormatAsLink("Mini-Pod", "EXOBASEHEADQUARTERS");

				// Token: 0x0400BC85 RID: 48261
				public static LocString DESC = "A quick and easy substitute, though it'll never live up to the original.";

				// Token: 0x0400BC86 RID: 48262
				public static LocString EFFECT = "A portable bioprinter that produces new Duplicants or care packages containing resources.\n\nOnly one Printing Pod or Mini-Pod is permitted per Planetoid.";
			}

			// Token: 0x02002AC0 RID: 10944
			public class AIRCONDITIONER
			{
				// Token: 0x0400BC87 RID: 48263
				public static LocString NAME = UI.FormatAsLink("Thermo Regulator", "AIRCONDITIONER");

				// Token: 0x0400BC88 RID: 48264
				public static LocString DESC = "A thermo regulator doesn't remove heat, but relocates it to a new area.";

				// Token: 0x0400BC89 RID: 48265
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Cools the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" piped through it, but outputs ",
					UI.FormatAsLink("Heat", "HEAT"),
					" in its immediate vicinity."
				});
			}

			// Token: 0x02002AC1 RID: 10945
			public class STATERPILLAREGG
			{
				// Token: 0x0400BC8A RID: 48266
				public static LocString NAME = UI.FormatAsLink("Slug Egg", "STATERPILLAREGG");

				// Token: 0x0400BC8B RID: 48267
				public static LocString DESC = "The electrifying egg of the " + UI.FormatAsLink("Plug Slug", "STATERPILLAR") + ".";

				// Token: 0x0400BC8C RID: 48268
				public static LocString EFFECT = "Slug Eggs can be connected to a " + UI.FormatAsLink("Power", "POWER") + " circuit as an energy source.";
			}

			// Token: 0x02002AC2 RID: 10946
			public class STATERPILLARGENERATOR
			{
				// Token: 0x0400BC8D RID: 48269
				public static LocString NAME = UI.FormatAsLink("Plug Slug", "STATERPILLAR");

				// Token: 0x02003A59 RID: 14937
				public class MODIFIERS
				{
					// Token: 0x0400EBAB RID: 60331
					public static LocString WILD = "Wild!";

					// Token: 0x0400EBAC RID: 60332
					public static LocString HUNGRY = "Hungry!";
				}
			}

			// Token: 0x02002AC3 RID: 10947
			public class BEEHIVE
			{
				// Token: 0x0400BC8E RID: 48270
				public static LocString NAME = UI.FormatAsLink("Beeta Hive", "BEEHIVE");

				// Token: 0x0400BC8F RID: 48271
				public static LocString DESC = string.Concat(new string[]
				{
					"A moderately ",
					UI.FormatAsLink("Radioactive", "RADIATION"),
					" nest made by ",
					UI.FormatAsLink("Beetas", "BEE"),
					".\n\nConverts ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" into ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" when worked by a Beeta.\nWill not function if ground below has been destroyed."
				});

				// Token: 0x0400BC90 RID: 48272
				public static LocString EFFECT = "The cozy home of a Beeta.";
			}

			// Token: 0x02002AC4 RID: 10948
			public class ETHANOLDISTILLERY
			{
				// Token: 0x0400BC91 RID: 48273
				public static LocString NAME = UI.FormatAsLink("Ethanol Distiller", "ETHANOLDISTILLERY");

				// Token: 0x0400BC92 RID: 48274
				public static LocString DESC = string.Concat(new string[]
				{
					"Ethanol distillers convert ",
					ITEMS.INDUSTRIAL_PRODUCTS.WOOD.NAME,
					" into burnable ",
					ELEMENTS.ETHANOL.NAME,
					" fuel."
				});

				// Token: 0x0400BC93 RID: 48275
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					UI.FormatAsLink("Wood", "WOOD"),
					" into ",
					UI.FormatAsLink("Ethanol", "ETHANOL"),
					"."
				});
			}

			// Token: 0x02002AC5 RID: 10949
			public class ALGAEDISTILLERY
			{
				// Token: 0x0400BC94 RID: 48276
				public static LocString NAME = UI.FormatAsLink("Algae Distiller", "ALGAEDISTILLERY");

				// Token: 0x0400BC95 RID: 48277
				public static LocString DESC = "Algae distillers convert disease-causing slime into algae for oxygen production.";

				// Token: 0x0400BC96 RID: 48278
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" into ",
					UI.FormatAsLink("Algae", "ALGAE"),
					"."
				});
			}

			// Token: 0x02002AC6 RID: 10950
			public class GUNKEMPTIER
			{
				// Token: 0x0400BC97 RID: 48279
				public static LocString NAME = UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER");

				// Token: 0x0400BC98 RID: 48280
				public static LocString DESC = "Bionic Duplicants are much more relaxed after a visit to the gunk extractor.";

				// Token: 0x0400BC99 RID: 48281
				public static LocString EFFECT = "Cleanses stale " + UI.FormatAsLink("Gunk", "LIQUIDGUNK") + " build-up from Duplicants' bionic parts.";
			}

			// Token: 0x02002AC7 RID: 10951
			public class OILCHANGER
			{
				// Token: 0x0400BC9A RID: 48282
				public static LocString NAME = UI.FormatAsLink("Lubrication Station", "OILCHANGER");

				// Token: 0x0400BC9B RID: 48283
				public static LocString DESC = "A fresh supply of oil keeps the ol' joints from getting too creaky.";

				// Token: 0x0400BC9C RID: 48284
				public static LocString EFFECT = "Uses " + UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL") + " to keep Duplicants' bionic parts running smoothly.";
			}

			// Token: 0x02002AC8 RID: 10952
			public class OXYLITEREFINERY
			{
				// Token: 0x0400BC9D RID: 48285
				public static LocString NAME = UI.FormatAsLink("Oxylite Refinery", "OXYLITEREFINERY");

				// Token: 0x0400BC9E RID: 48286
				public static LocString DESC = "Oxylite is a solid and easily transportable source of consumable oxygen.";

				// Token: 0x0400BC9F RID: 48287
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Synthesizes ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" using ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and a small amount of ",
					UI.FormatAsLink("Gold", "GOLD"),
					"."
				});
			}

			// Token: 0x02002AC9 RID: 10953
			public class OXYSCONCE
			{
				// Token: 0x0400BCA0 RID: 48288
				public static LocString NAME = UI.FormatAsLink("Oxylite Sconce", "OXYSCONCE");

				// Token: 0x0400BCA1 RID: 48289
				public static LocString DESC = "Sconces prevent diffused oxygen from being wasted inside storage bins.";

				// Token: 0x0400BCA2 RID: 48290
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores a small chunk of ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" which gradually releases ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" into the environment."
				});
			}

			// Token: 0x02002ACA RID: 10954
			public class FERTILIZERMAKER
			{
				// Token: 0x0400BCA3 RID: 48291
				public static LocString NAME = UI.FormatAsLink("Fertilizer Synthesizer", "FERTILIZERMAKER");

				// Token: 0x0400BCA4 RID: 48292
				public static LocString DESC = "Fertilizer synthesizers convert polluted dirt into fertilizer for domestic plants.";

				// Token: 0x0400BCA5 RID: 48293
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" and ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					" to produce ",
					UI.FormatAsLink("Fertilizer", "FERTILIZER"),
					"."
				});
			}

			// Token: 0x02002ACB RID: 10955
			public class ALGAEHABITAT
			{
				// Token: 0x0400BCA6 RID: 48294
				public static LocString NAME = UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT");

				// Token: 0x0400BCA7 RID: 48295
				public static LocString DESC = "Algae colony, Duplicant colony... we're more alike than we are different.";

				// Token: 0x0400BCA8 RID: 48296
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Algae", "ALGAE"),
					" to produce ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and remove some ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					".\n\nGains a 10% efficiency boost in direct ",
					UI.FormatAsLink("Light", "LIGHT"),
					"."
				});

				// Token: 0x0400BCA9 RID: 48297
				public static LocString SIDESCREEN_TITLE = "Empty " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + " Threshold";
			}

			// Token: 0x02002ACC RID: 10956
			public class BATTERY
			{
				// Token: 0x0400BCAA RID: 48298
				public static LocString NAME = UI.FormatAsLink("Battery", "BATTERY");

				// Token: 0x0400BCAB RID: 48299
				public static LocString DESC = "Batteries allow power from generators to be stored for later.";

				// Token: 0x0400BCAC RID: 48300
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that power to buildings.\n\nLoses charge over time.";

				// Token: 0x0400BCAD RID: 48301
				public static LocString CHARGE_LOSS = "{Battery} charge loss";
			}

			// Token: 0x02002ACD RID: 10957
			public class FLYINGCREATUREBAIT
			{
				// Token: 0x0400BCAE RID: 48302
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Bait", "FLYINGCREATUREBAIT");

				// Token: 0x0400BCAF RID: 48303
				public static LocString DESC = "The type of critter attracted by critter bait depends on the construction material.";

				// Token: 0x0400BCB0 RID: 48304
				public static LocString EFFECT = "Attracts one type of airborne critter.\n\nSingle use.";
			}

			// Token: 0x02002ACE RID: 10958
			public class WATERTRAP
			{
				// Token: 0x0400BCB1 RID: 48305
				public static LocString NAME = UI.FormatAsLink("Fish Trap", "WATERTRAP");

				// Token: 0x0400BCB2 RID: 48306
				public static LocString DESC = "Trapped fish will automatically be bagged for transport.";

				// Token: 0x0400BCB3 RID: 48307
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts and traps swimming ",
					UI.FormatAsLink("Pacu", "PACU"),
					".\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			// Token: 0x02002ACF RID: 10959
			public class REUSABLETRAP
			{
				// Token: 0x0400BCB4 RID: 48308
				public static LocString LOGIC_PORT = "Trap Occupied";

				// Token: 0x0400BCB5 RID: 48309
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when a critter has been trapped";

				// Token: 0x0400BCB6 RID: 48310
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400BCB7 RID: 48311
				public static LocString INPUT_LOGIC_PORT = "Trap Setter";

				// Token: 0x0400BCB8 RID: 48312
				public static LocString INPUT_LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set trap";

				// Token: 0x0400BCB9 RID: 48313
				public static LocString INPUT_LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disarm and empty trap";
			}

			// Token: 0x02002AD0 RID: 10960
			public class CREATUREAIRTRAP
			{
				// Token: 0x0400BCBA RID: 48314
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Trap", "CREATUREAIRTRAP");

				// Token: 0x0400BCBB RID: 48315
				public static LocString DESC = "It needs to be armed prior to use.";

				// Token: 0x0400BCBC RID: 48316
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts and captures airborne ",
					UI.FormatAsLink("Critters", "CREATURES"),
					".\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			// Token: 0x02002AD1 RID: 10961
			public class AIRBORNECREATURELURE
			{
				// Token: 0x0400BCBD RID: 48317
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Lure", "AIRBORNECREATURELURE");

				// Token: 0x0400BCBE RID: 48318
				public static LocString DESC = "Lures can relocate Pufts or Shine Bugs to specific locations in my colony.";

				// Token: 0x0400BCBF RID: 48319
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts one type of airborne critter at a time.\n\nMust be baited with ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" or ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					"."
				});
			}

			// Token: 0x02002AD2 RID: 10962
			public class BATTERYMEDIUM
			{
				// Token: 0x0400BCC0 RID: 48320
				public static LocString NAME = UI.FormatAsLink("Jumbo Battery", "BATTERYMEDIUM");

				// Token: 0x0400BCC1 RID: 48321
				public static LocString DESC = "Larger batteries hold more power and keep systems running longer before recharging.";

				// Token: 0x0400BCC2 RID: 48322
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that power to buildings.\n\nSlightly loses charge over time.";
			}

			// Token: 0x02002AD3 RID: 10963
			public class BATTERYSMART
			{
				// Token: 0x0400BCC3 RID: 48323
				public static LocString NAME = UI.FormatAsLink("Smart Battery", "BATTERYSMART");

				// Token: 0x0400BCC4 RID: 48324
				public static LocString DESC = "Smart batteries send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when they require charging.";

				// Token: 0x0400BCC5 RID: 48325
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Power", "POWER"),
					" from generators, then provides that power to buildings.\n\nSends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the configuration of the Logic Activation Parameters.\n\nVery slightly loses charge over time."
				});

				// Token: 0x0400BCC6 RID: 48326
				public static LocString LOGIC_PORT = "Charge Parameters";

				// Token: 0x0400BCC7 RID: 48327
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>Low Threshold</b> charged, until <b>High Threshold</b> is reached again";

				// Token: 0x0400BCC8 RID: 48328
				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when the battery is more than <b>High Threshold</b> charged, until <b>Low Threshold</b> is reached again";

				// Token: 0x0400BCC9 RID: 48329
				public static LocString ACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>{0}%</b> charged, until it is <b>{1}% (High Threshold)</b> charged";

				// Token: 0x0400BCCA RID: 48330
				public static LocString DEACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when battery is <b>{0}%</b> charged, until it is less than <b>{1}% (Low Threshold)</b> charged";

				// Token: 0x0400BCCB RID: 48331
				public static LocString SIDESCREEN_TITLE = "Logic Activation Parameters";

				// Token: 0x0400BCCC RID: 48332
				public static LocString SIDESCREEN_ACTIVATE = "Low Threshold:";

				// Token: 0x0400BCCD RID: 48333
				public static LocString SIDESCREEN_DEACTIVATE = "High Threshold:";
			}

			// Token: 0x02002AD4 RID: 10964
			public class BED
			{
				// Token: 0x0400BCCE RID: 48334
				public static LocString NAME = UI.FormatAsLink("Cot", "BED");

				// Token: 0x0400BCCF RID: 48335
				public static LocString DESC = "Duplicants without a bed will develop sore backs from sleeping on the floor.";

				// Token: 0x0400BCD0 RID: 48336
				public static LocString EFFECT = "Gives one Duplicant a place to sleep.\n\nDuplicants will automatically return to their cots to sleep at night.";

				// Token: 0x02003A5A RID: 14938
				public class FACADES
				{
					// Token: 0x02003DB8 RID: 15800
					public class DEFAULT_BED
					{
						// Token: 0x0400F43E RID: 62526
						public static LocString NAME = UI.FormatAsLink("Cot", "BED");

						// Token: 0x0400F43F RID: 62527
						public static LocString DESC = "A safe place to sleep.";
					}

					// Token: 0x02003DB9 RID: 15801
					public class STARCURTAIN
					{
						// Token: 0x0400F440 RID: 62528
						public static LocString NAME = UI.FormatAsLink("Stargazer Cot", "BED");

						// Token: 0x0400F441 RID: 62529
						public static LocString DESC = "Now Duplicants can sleep beneath the stars without wearing an Atmo Suit to bed.";
					}

					// Token: 0x02003DBA RID: 15802
					public class SCIENCELAB
					{
						// Token: 0x0400F442 RID: 62530
						public static LocString NAME = UI.FormatAsLink("Lab Cot", "BED");

						// Token: 0x0400F443 RID: 62531
						public static LocString DESC = "For the Duplicant who dreams of scientific discoveries.";
					}

					// Token: 0x02003DBB RID: 15803
					public class STAYCATION
					{
						// Token: 0x0400F444 RID: 62532
						public static LocString NAME = UI.FormatAsLink("Staycation Cot", "BED");

						// Token: 0x0400F445 RID: 62533
						public static LocString DESC = "Like a weekend away, except... not.";
					}

					// Token: 0x02003DBC RID: 15804
					public class CREAKY
					{
						// Token: 0x0400F446 RID: 62534
						public static LocString NAME = UI.FormatAsLink("Camping Cot", "BED");

						// Token: 0x0400F447 RID: 62535
						public static LocString DESC = "It's sturdier than it looks.";
					}

					// Token: 0x02003DBD RID: 15805
					public class STRINGLIGHTS
					{
						// Token: 0x0400F448 RID: 62536
						public static LocString NAME = "Good Job Cot";

						// Token: 0x0400F449 RID: 62537
						public static LocString DESC = "Wrapped in shiny gold stars, to help sleepy Duplicants feel accomplished.";
					}
				}
			}

			// Token: 0x02002AD5 RID: 10965
			public class BOTTLEEMPTIER
			{
				// Token: 0x0400BCD1 RID: 48337
				public static LocString NAME = UI.FormatAsLink("Bottle Emptier", "BOTTLEEMPTIER");

				// Token: 0x0400BCD2 RID: 48338
				public static LocString DESC = "A bottle emptier's Element Filter can be used to designate areas for specific liquid storage.";

				// Token: 0x0400BCD3 RID: 48339
				public static LocString EFFECT = "Empties bottled " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " back into the world.";
			}

			// Token: 0x02002AD6 RID: 10966
			public class BOTTLEEMPTIERGAS
			{
				// Token: 0x0400BCD4 RID: 48340
				public static LocString NAME = UI.FormatAsLink("Canister Emptier", "BOTTLEEMPTIERGAS");

				// Token: 0x0400BCD5 RID: 48341
				public static LocString DESC = "A canister emptier's Element Filter can designate areas for specific gas storage.";

				// Token: 0x0400BCD6 RID: 48342
				public static LocString EFFECT = "Empties " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " canisters back into the world.";
			}

			// Token: 0x02002AD7 RID: 10967
			public class BOTTLEEMPTIERCONDUITLIQUID
			{
				// Token: 0x0400BCD7 RID: 48343
				public static LocString NAME = UI.FormatAsLink("Bottle Drainer", "BOTTLEEMPTIERCONDUITLIQUID");

				// Token: 0x0400BCD8 RID: 48344
				public static LocString DESC = "A bottle drainer's Element Filter can be used to designate areas for specific liquid storage.";

				// Token: 0x0400BCD9 RID: 48345
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Drains bottled ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" into ",
					UI.FormatAsLink("Liquid Pipes", "LIQUIDCONDUIT"),
					"."
				});
			}

			// Token: 0x02002AD8 RID: 10968
			public class BOTTLEEMPTIERCONDUITGAS
			{
				// Token: 0x0400BCDA RID: 48346
				public static LocString NAME = UI.FormatAsLink("Canister Drainer", "BOTTLEEMPTIERCONDUITGAS");

				// Token: 0x0400BCDB RID: 48347
				public static LocString DESC = "A canister drainer's Element Filter can designate areas for specific gas storage.";

				// Token: 0x0400BCDC RID: 48348
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Drains ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" canisters into ",
					UI.FormatAsLink("Gas Pipes", "GASCONDUIT"),
					"."
				});
			}

			// Token: 0x02002AD9 RID: 10969
			public class ARTIFACTCARGOBAY
			{
				// Token: 0x0400BCDD RID: 48349
				public static LocString NAME = UI.FormatAsLink("Artifact Transport Module", "ARTIFACTCARGOBAY");

				// Token: 0x0400BCDE RID: 48350
				public static LocString DESC = "Holds artifacts found in space.";

				// Token: 0x0400BCDF RID: 48351
				public static LocString EFFECT = "Allows Duplicants to store any artifacts they uncover during space missions.\n\nArtifacts become available to the colony upon the rocket's return. \n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			// Token: 0x02002ADA RID: 10970
			public class CARGOBAY
			{
				// Token: 0x0400BCE0 RID: 48352
				public static LocString NAME = UI.FormatAsLink("Cargo Bay", "CARGOBAY");

				// Token: 0x0400BCE1 RID: 48353
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x0400BCE2 RID: 48354
				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x02002ADB RID: 10971
			public class CARGOBAYCLUSTER
			{
				// Token: 0x0400BCE3 RID: 48355
				public static LocString NAME = UI.FormatAsLink("Large Cargo Bay", "CARGOBAYCLUSTER");

				// Token: 0x0400BCE4 RID: 48356
				public static LocString DESC = "Holds more than a regular cargo bay.";

				// Token: 0x0400BCE5 RID: 48357
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002ADC RID: 10972
			public class SOLIDCARGOBAYSMALL
			{
				// Token: 0x0400BCE6 RID: 48358
				public static LocString NAME = UI.FormatAsLink("Cargo Bay", "SOLIDCARGOBAYSMALL");

				// Token: 0x0400BCE7 RID: 48359
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x0400BCE8 RID: 48360
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002ADD RID: 10973
			public class SPECIALCARGOBAY
			{
				// Token: 0x0400BCE9 RID: 48361
				public static LocString NAME = UI.FormatAsLink("Biological Cargo Bay", "SPECIALCARGOBAY");

				// Token: 0x0400BCEA RID: 48362
				public static LocString DESC = "Biological cargo bays allow Duplicants to retrieve alien plants and wildlife from space.";

				// Token: 0x0400BCEB RID: 48363
				public static LocString EFFECT = "Allows Duplicants to store unusual or organic resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x02002ADE RID: 10974
			public class SPECIALCARGOBAYCLUSTER
			{
				// Token: 0x0400BCEC RID: 48364
				public static LocString NAME = UI.FormatAsLink("Critter Cargo Bay", "SPECIALCARGOBAYCLUSTER");

				// Token: 0x0400BCED RID: 48365
				public static LocString DESC = "Critters do not require feeding during transit.";

				// Token: 0x0400BCEE RID: 48366
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to transport ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					" through space.\n\nSpecimens can be released into the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				// Token: 0x0400BCEF RID: 48367
				public static LocString RELEASE_BTN = "Release Critter";

				// Token: 0x0400BCF0 RID: 48368
				public static LocString RELEASE_BTN_TOOLTIP = "Release the critter stored inside";
			}

			// Token: 0x02002ADF RID: 10975
			public class COMMANDMODULE
			{
				// Token: 0x0400BCF1 RID: 48369
				public static LocString NAME = UI.FormatAsLink("Command Capsule", "COMMANDMODULE");

				// Token: 0x0400BCF2 RID: 48370
				public static LocString DESC = "At least one astronaut must be assigned to the command module to pilot a rocket.";

				// Token: 0x0400BCF3 RID: 48371
				public static LocString EFFECT = "Contains passenger seating for Duplicant " + UI.FormatAsLink("Astronauts", "ASTRONAUTING1") + ".\n\nA Command Capsule must be the last module installed at the top of a rocket.";

				// Token: 0x0400BCF4 RID: 48372
				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				// Token: 0x0400BCF5 RID: 48373
				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket launch checklist is complete";

				// Token: 0x0400BCF6 RID: 48374
				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400BCF7 RID: 48375
				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				// Token: 0x0400BCF8 RID: 48376
				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				// Token: 0x0400BCF9 RID: 48377
				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Awaits launch command";
			}

			// Token: 0x02002AE0 RID: 10976
			public class CLUSTERCOMMANDMODULE
			{
				// Token: 0x0400BCFA RID: 48378
				public static LocString NAME = UI.FormatAsLink("Command Capsule", "CLUSTERCOMMANDMODULE");

				// Token: 0x0400BCFB RID: 48379
				public static LocString DESC = "";

				// Token: 0x0400BCFC RID: 48380
				public static LocString EFFECT = "";

				// Token: 0x0400BCFD RID: 48381
				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				// Token: 0x0400BCFE RID: 48382
				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket launch checklist is complete";

				// Token: 0x0400BCFF RID: 48383
				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400BD00 RID: 48384
				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				// Token: 0x0400BD01 RID: 48385
				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				// Token: 0x0400BD02 RID: 48386
				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Awaits launch command";
			}

			// Token: 0x02002AE1 RID: 10977
			public class CLUSTERCRAFTINTERIORDOOR
			{
				// Token: 0x0400BD03 RID: 48387
				public static LocString NAME = UI.FormatAsLink("Interior Hatch", "CLUSTERCRAFTINTERIORDOOR");

				// Token: 0x0400BD04 RID: 48388
				public static LocString DESC = "A hatch for getting in and out of the rocket.";

				// Token: 0x0400BD05 RID: 48389
				public static LocString EFFECT = "Warning: Do not open mid-flight.";
			}

			// Token: 0x02002AE2 RID: 10978
			public class ROBOPILOTMODULE
			{
				// Token: 0x0400BD06 RID: 48390
				public static LocString NAME = UI.FormatAsLink("Robo-Pilot Module", "ROBOPILOTMODULE");

				// Token: 0x0400BD07 RID: 48391
				public static LocString DESC = "Robo-pilot modules do not require a Duplicant astronaut.";

				// Token: 0x0400BD08 RID: 48392
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Enables rockets to travel swfitly without a ",
					UI.FormatAsLink("Rocket Control Station", "ROCKETCONTROLSTATION"),
					".\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002AE3 RID: 10979
			public class ROBOPILOTCOMMANDMODULE
			{
				// Token: 0x0400BD09 RID: 48393
				public static LocString NAME = UI.FormatAsLink("Robo-Pilot Capsule", "ROBOPILOTCOMMANDMODULE");

				// Token: 0x0400BD0A RID: 48394
				public static LocString DESC = "Robo-pilot modules do not require a Duplicant astronaut.";

				// Token: 0x0400BD0B RID: 48395
				public static LocString EFFECT = "Enables rockets to travel swiftly and safely without a " + UI.FormatAsLink("Command Capsule", "COMMANDMODULE") + ".\n\nA Robo-Pilot Capsule must be the last module installed at the top of a rocket.";
			}

			// Token: 0x02002AE4 RID: 10980
			public class ROCKETCONTROLSTATION
			{
				// Token: 0x0400BD0C RID: 48396
				public static LocString NAME = UI.FormatAsLink("Rocket Control Station", "ROCKETCONTROLSTATION");

				// Token: 0x0400BD0D RID: 48397
				public static LocString DESC = "Someone needs to be around to jiggle the controls when the screensaver comes on.";

				// Token: 0x0400BD0E RID: 48398
				public static LocString EFFECT = "Allows Duplicants to use pilot-operated rockets and control access to interior buildings.\n\nAssigned Duplicants must have the " + UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1") + " skill.";

				// Token: 0x0400BD0F RID: 48399
				public static LocString LOGIC_PORT = "Restrict Building Usage";

				// Token: 0x0400BD10 RID: 48400
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Restrict access to interior buildings";

				// Token: 0x0400BD11 RID: 48401
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Unrestrict access to interior buildings";
			}

			// Token: 0x02002AE5 RID: 10981
			public class RESEARCHMODULE
			{
				// Token: 0x0400BD12 RID: 48402
				public static LocString NAME = UI.FormatAsLink("Research Module", "RESEARCHMODULE");

				// Token: 0x0400BD13 RID: 48403
				public static LocString DESC = "Data banks can be used at virtual planetariums to produce additional research.";

				// Token: 0x0400BD14 RID: 48404
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Completes one ",
					UI.FormatAsLink("Research Task", "RESEARCH"),
					" per space mission.\n\nProduces a small Data Bank regardless of mission destination.\n\nGenerated ",
					UI.FormatAsLink("Research Points", "RESEARCH"),
					" become available upon the rocket's return."
				});
			}

			// Token: 0x02002AE6 RID: 10982
			public class RESEARCHCLUSTERMODULE
			{
				// Token: 0x0400BD15 RID: 48405
				public static LocString NAME = UI.FormatAsLink("Research Module", "RESEARCHMODULE");

				// Token: 0x0400BD16 RID: 48406
				public static LocString DESC = "Unlocks the possibility of Gathering Databanks floating in space";

				// Token: 0x0400BD17 RID: 48407
				public static LocString EFFECT = "The Research module allows the rocket to gather Data banks that happen to be on the same hex cell than the rocket when it travels through the galaxy";
			}

			// Token: 0x02002AE7 RID: 10983
			public class TOURISTMODULE
			{
				// Token: 0x0400BD18 RID: 48408
				public static LocString NAME = UI.FormatAsLink("Sight-Seeing Module", "TOURISTMODULE");

				// Token: 0x0400BD19 RID: 48409
				public static LocString DESC = "An astronaut must accompany sight seeing Duplicants on rocket flights.";

				// Token: 0x0400BD1A RID: 48410
				public static LocString EFFECT = "Allows one non-Astronaut Duplicant to visit space.\n\nSight-Seeing Rocket flights decrease " + UI.FormatAsLink("Stress", "STRESS") + ".";
			}

			// Token: 0x02002AE8 RID: 10984
			public class SCANNERMODULE
			{
				// Token: 0x0400BD1B RID: 48411
				public static LocString NAME = UI.FormatAsLink("Cartographic Module", "SCANNERMODULE");

				// Token: 0x0400BD1C RID: 48412
				public static LocString DESC = "Allows Duplicants to boldly go where other Duplicants haven't been yet.";

				// Token: 0x0400BD1D RID: 48413
				public static LocString EFFECT = "Automatically analyzes adjacent space while on a voyage. \n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			// Token: 0x02002AE9 RID: 10985
			public class HABITATMODULESMALL
			{
				// Token: 0x0400BD1E RID: 48414
				public static LocString NAME = UI.FormatAsLink("Solo Spacefarer Nosecone", "HABITATMODULESMALL");

				// Token: 0x0400BD1F RID: 48415
				public static LocString DESC = "One lucky Duplicant gets the best view from the whole rocket.";

				// Token: 0x0400BD20 RID: 48416
				public static LocString EFFECT = "Functions as a Command Module and a Nosecone.\n\nHolds one Duplicant traveller.\n\nOne Command Module may be installed per rocket.\n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built at the top of a rocket.";
			}

			// Token: 0x02002AEA RID: 10986
			public class HABITATMODULEMEDIUM
			{
				// Token: 0x0400BD21 RID: 48417
				public static LocString NAME = UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM");

				// Token: 0x0400BD22 RID: 48418
				public static LocString DESC = "Allows Duplicants to survive space travel... Hopefully.";

				// Token: 0x0400BD23 RID: 48419
				public static LocString EFFECT = "Functions as a Command Module.\n\nHolds up to 10 Duplicant travellers.\n\nOne Command Module may be installed per rocket. \n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			// Token: 0x02002AEB RID: 10987
			public class NOSECONEBASIC
			{
				// Token: 0x0400BD24 RID: 48420
				public static LocString NAME = UI.FormatAsLink("Basic Nosecone", "NOSECONEBASIC");

				// Token: 0x0400BD25 RID: 48421
				public static LocString DESC = "Every rocket requires a nosecone to fly.";

				// Token: 0x0400BD26 RID: 48422
				public static LocString EFFECT = "Protects a rocket during takeoff and entry, enabling space travel.\n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ", and built at the top of a rocket.";
			}

			// Token: 0x02002AEC RID: 10988
			public class NOSECONEHARVEST
			{
				// Token: 0x0400BD27 RID: 48423
				public static LocString NAME = UI.FormatAsLink("Drillcone", "NOSECONEHARVEST");

				// Token: 0x0400BD28 RID: 48424
				public static LocString DESC = "Collecting the drilled-out resources requires a storage module.";

				// Token: 0x0400BD29 RID: 48425
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Enables a rocket to drill into interstellar debris to free up ",
					UI.FormatAsLink("gas", "ELEMENTS_GAS"),
					", ",
					UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
					" resources in space.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nMust be built at the top of a rocket with ",
					UI.FormatAsLink("gas", "ELEMENTS_GAS"),
					", ",
					UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
					" Cargo Module attached to store the appropriate resources."
				});
			}

			// Token: 0x02002AED RID: 10989
			public class CO2ENGINE
			{
				// Token: 0x0400BD2A RID: 48426
				public static LocString NAME = UI.FormatAsLink("Carbon Dioxide Engine", "CO2ENGINE");

				// Token: 0x0400BD2B RID: 48427
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400BD2C RID: 48428
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses pressurized ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" to propel rockets for short range space exploration.\n\nCarbon Dioxide Engines are relatively fast engine for their size but with limited height restrictions.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002AEE RID: 10990
			public class KEROSENEENGINE
			{
				// Token: 0x0400BD2D RID: 48429
				public static LocString NAME = UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINE");

				// Token: 0x0400BD2E RID: 48430
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400BD2F RID: 48431
				public static LocString EFFECT = "Burns " + UI.FormatAsLink("Petroleum", "PETROLEUM") + " to propel rockets for mid-range space exploration.\n\nPetroleum Engines have generous height restrictions, ideal for hauling many modules.\n\nThe engine must be built first before more rocket modules can be added.";
			}

			// Token: 0x02002AEF RID: 10991
			public class KEROSENEENGINECLUSTER
			{
				// Token: 0x0400BD30 RID: 48432
				public static LocString NAME = UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINECLUSTER");

				// Token: 0x0400BD31 RID: 48433
				public static LocString DESC = "More powerful rocket engines can propel heavier burdens.";

				// Token: 0x0400BD32 RID: 48434
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" to propel rockets for mid-range space exploration.\n\nPetroleum Engines have generous height restrictions, ideal for hauling many modules.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					", and requires an oxidizer tank. \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002AF0 RID: 10992
			public class KEROSENEENGINECLUSTERSMALL
			{
				// Token: 0x0400BD33 RID: 48435
				public static LocString NAME = UI.FormatAsLink("Small Petroleum Engine", "KEROSENEENGINECLUSTERSMALL");

				// Token: 0x0400BD34 RID: 48436
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400BD35 RID: 48437
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" to propel rockets for mid-range space exploration.\n\nSmall Petroleum Engines possess the same speed as a ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but have smaller height restrictions.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					", and requires an oxidizer tank.\n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002AF1 RID: 10993
			public class BIODIESELENGINE
			{
				// Token: 0x0400BD36 RID: 48438
				public static LocString NAME = UI.FormatAsLink("Biodiesel Engine", "BIODIESELENGINE");

				// Token: 0x0400BD37 RID: 48439
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400BD38 RID: 48440
				public static LocString EFFECT = "Burns " + ELEMENTS.REFINEDLIPID.NAME + " to propel rockets for mid-range space exploration.\n\nBiodiesel Engines have generous height restrictions, ideal for hauling a larger number of modules.\n\nOnce the engine has been built, more rocket modules can be added.";
			}

			// Token: 0x02002AF2 RID: 10994
			public class BIODIESELENGINECLUSTER
			{
				// Token: 0x0400BD39 RID: 48441
				public static LocString NAME = UI.FormatAsLink("Biodiesel Engine", "BIODIESELENGINECLUSTER");

				// Token: 0x0400BD3A RID: 48442
				public static LocString DESC = "More powerful rocket engines can propel heavier burdens.";

				// Token: 0x0400BD3B RID: 48443
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					ELEMENTS.REFINEDLIPID.NAME,
					" to propel rockets for mid-range space exploration.\n\nBiodiesel Engines have generous height restrictions, ideal for hauling many modules.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					", and requires an oxidizer tank.\n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002AF3 RID: 10995
			public class HYDROGENENGINE
			{
				// Token: 0x0400BD3C RID: 48444
				public static LocString NAME = UI.FormatAsLink("Hydrogen Engine", "HYDROGENENGINE");

				// Token: 0x0400BD3D RID: 48445
				public static LocString DESC = "Hydrogen engines can propel rockets further than steam or petroleum engines.";

				// Token: 0x0400BD3E RID: 48446
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN"),
					" to propel rockets for long-range space exploration.\n\nHydrogen Engines have the same generous height restrictions as ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but are slightly faster.\n\nThe engine must be built first before more rocket modules can be added."
				});
			}

			// Token: 0x02002AF4 RID: 10996
			public class HYDROGENENGINECLUSTER
			{
				// Token: 0x0400BD3F RID: 48447
				public static LocString NAME = UI.FormatAsLink("Hydrogen Engine", "HYDROGENENGINECLUSTER");

				// Token: 0x0400BD40 RID: 48448
				public static LocString DESC = "Hydrogen engines can propel rockets further than steam or petroleum engines.";

				// Token: 0x0400BD41 RID: 48449
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN"),
					" to propel rockets for long-range space exploration.\n\nHydrogen Engines have the same generous height restrictions as ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but are slightly faster.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					", and requires an oxidizer tank.\n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002AF5 RID: 10997
			public class SUGARENGINE
			{
				// Token: 0x0400BD42 RID: 48450
				public static LocString NAME = UI.FormatAsLink("Sugar Engine", "SUGARENGINE");

				// Token: 0x0400BD43 RID: 48451
				public static LocString DESC = "Not the most stylish way to travel space, but certainly the tastiest.";

				// Token: 0x0400BD44 RID: 48452
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					" to propel rockets for short range space exploration.\n\nSugar Engines have higher height restrictions than ",
					UI.FormatAsLink("Carbon Dioxide Engines", "CO2ENGINE"),
					", but move slower.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					", and requires a ",
					BUILDINGS.PREFABS.OXIDIZERTANKCLUSTER.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002AF6 RID: 10998
			public class HEPENGINE
			{
				// Token: 0x0400BD45 RID: 48453
				public static LocString NAME = UI.FormatAsLink("Radbolt Engine", "HEPENGINE");

				// Token: 0x0400BD46 RID: 48454
				public static LocString DESC = "Radbolt-fueled rockets support few modules, but travel exceptionally far.";

				// Token: 0x0400BD47 RID: 48455
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Injects ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" into a reaction chamber to propel rockets for long-range space exploration.\n\nRadbolt Engines are faster than ",
					UI.FormatAsLink("Hydrogen Engines", "HYDROGENENGINE"),
					" but with a more restrictive height allowance.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});

				// Token: 0x0400BD48 RID: 48456
				public static LocString LOGIC_PORT_STORAGE = "Radbolt Storage";

				// Token: 0x0400BD49 RID: 48457
				public static LocString LOGIC_PORT_STORAGE_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its Radbolt Storage is full";

				// Token: 0x0400BD4A RID: 48458
				public static LocString LOGIC_PORT_STORAGE_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002AF7 RID: 10999
			public class ORBITALCARGOMODULE
			{
				// Token: 0x0400BD4B RID: 48459
				public static LocString NAME = UI.FormatAsLink("Orbital Cargo Module", "ORBITALCARGOMODULE");

				// Token: 0x0400BD4C RID: 48460
				public static LocString DESC = "It's a generally good idea to pack some supplies when exploring unknown worlds.";

				// Token: 0x0400BD4D RID: 48461
				public static LocString EFFECT = "Delivers cargo to the surface of Planetoids that do not yet have a " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built via Rocket Platform.";
			}

			// Token: 0x02002AF8 RID: 11000
			public class BATTERYMODULE
			{
				// Token: 0x0400BD4E RID: 48462
				public static LocString NAME = UI.FormatAsLink("Battery Module", "BATTERYMODULE");

				// Token: 0x0400BD4F RID: 48463
				public static LocString DESC = "Charging a battery module before takeoff makes it easier to power buildings during flight.";

				// Token: 0x0400BD50 RID: 48464
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the excess ",
					UI.FormatAsLink("Power", "POWER"),
					" generated by a Rocket Engine or ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					".\n\nProvides stored power to ",
					UI.FormatAsLink("Interior Rocket Outlets", "ROCKETINTERIORPOWERPLUG"),
					".\n\nLoses charge over time. \n\nMust be built via Rocket Platform."
				});
			}

			// Token: 0x02002AF9 RID: 11001
			public class PIONEERMODULE
			{
				// Token: 0x0400BD51 RID: 48465
				public static LocString NAME = UI.FormatAsLink("Trailblazer Module", "PIONEERMODULE");

				// Token: 0x0400BD52 RID: 48466
				public static LocString DESC = "That's one small step for Dupekind.";

				// Token: 0x0400BD53 RID: 48467
				public static LocString EFFECT = "Enables travel to Planetoids that do not yet have a " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".\n\nCan hold one Duplicant traveller.\n\nDeployment is available while in a Starmap hex adjacent to a Planetoid. \n\nMust be built via Rocket Platform.";
			}

			// Token: 0x02002AFA RID: 11002
			public class SOLARPANELMODULE
			{
				// Token: 0x0400BD54 RID: 48468
				public static LocString NAME = UI.FormatAsLink("Solar Panel Module", "SOLARPANELMODULE");

				// Token: 0x0400BD55 RID: 48469
				public static LocString DESC = "Collect solar energy before takeoff and during flight.";

				// Token: 0x0400BD56 RID: 48470
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					" for use on rockets.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nMust be exposed to space."
				});
			}

			// Token: 0x02002AFB RID: 11003
			public class SCOUTMODULE
			{
				// Token: 0x0400BD57 RID: 48471
				public static LocString NAME = UI.FormatAsLink("Rover's Module", "SCOUTMODULE");

				// Token: 0x0400BD58 RID: 48472
				public static LocString DESC = "Rover can conduct explorations of planetoids that don't have rocket platforms built.";

				// Token: 0x0400BD59 RID: 48473
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Deploys one ",
					UI.FormatAsLink("Rover Bot", "SCOUT"),
					" for remote Planetoid exploration.\n\nDeployment is available while in a Starmap hex adjacent to a Planetoid. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002AFC RID: 11004
			public class PIONEERLANDER
			{
				// Token: 0x0400BD5A RID: 48474
				public static LocString NAME = UI.FormatAsLink("Trailblazer Lander", "PIONEERMODULE");

				// Token: 0x0400BD5B RID: 48475
				public static LocString DESC = "Lands a Duplicant on a Planetoid from an orbiting " + BUILDINGS.PREFABS.PIONEERMODULE.NAME + ".";
			}

			// Token: 0x02002AFD RID: 11005
			public class SCOUTLANDER
			{
				// Token: 0x0400BD5C RID: 48476
				public static LocString NAME = UI.FormatAsLink("Rover's Lander", "SCOUTMODULE");

				// Token: 0x0400BD5D RID: 48477
				public static LocString DESC = string.Concat(new string[]
				{
					"Lands ",
					UI.FormatAsLink("Rover", "SCOUT"),
					" on a Planetoid when ",
					BUILDINGS.PREFABS.SCOUTMODULE.NAME,
					" is in orbit."
				});
			}

			// Token: 0x02002AFE RID: 11006
			public class GANTRY
			{
				// Token: 0x0400BD5E RID: 48478
				public static LocString NAME = UI.FormatAsLink("Gantry", "GANTRY");

				// Token: 0x0400BD5F RID: 48479
				public static LocString DESC = "A gantry can be built over rocket pieces where ladders and tile cannot.";

				// Token: 0x0400BD60 RID: 48480
				public static LocString EFFECT = "Provides scaffolding across rocket modules to allow Duplicant access.";

				// Token: 0x0400BD61 RID: 48481
				public static LocString LOGIC_PORT = "Extend/Retract";

				// Token: 0x0400BD62 RID: 48482
				public static LocString LOGIC_PORT_ACTIVE = "<b>Extends gantry</b> when a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " signal is received";

				// Token: 0x0400BD63 RID: 48483
				public static LocString LOGIC_PORT_INACTIVE = "<b>Retracts gantry</b> when a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " signal is received";
			}

			// Token: 0x02002AFF RID: 11007
			public class ROCKETINTERIORPOWERPLUG
			{
				// Token: 0x0400BD64 RID: 48484
				public static LocString NAME = UI.FormatAsLink("Power Outlet Fitting", "ROCKETINTERIORPOWERPLUG");

				// Token: 0x0400BD65 RID: 48485
				public static LocString DESC = "Outlets conveniently power buildings inside a cockpit using their rocket's power stores.";

				// Token: 0x0400BD66 RID: 48486
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Power", "POWER"),
					" to connected buildings.\n\nPulls power from ",
					UI.FormatAsLink("Battery Modules", "BATTERYMODULE"),
					" and Rocket Engines.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x02002B00 RID: 11008
			public class ROCKETINTERIORLIQUIDINPUT
			{
				// Token: 0x0400BD67 RID: 48487
				public static LocString NAME = UI.FormatAsLink("Liquid Intake Fitting", "ROCKETINTERIORLIQUIDINPUT");

				// Token: 0x0400BD68 RID: 48488
				public static LocString DESC = "Begone, foul waters!";

				// Token: 0x0400BD69 RID: 48489
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to be pumped into rocket storage via ",
					UI.FormatAsLink("Pipes", "LIQUIDCONDUIT"),
					".\n\nSends liquid to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x02002B01 RID: 11009
			public class ROCKETINTERIORLIQUIDOUTPUT
			{
				// Token: 0x0400BD6A RID: 48490
				public static LocString NAME = UI.FormatAsLink("Liquid Output Fitting", "ROCKETINTERIORLIQUIDOUTPUT");

				// Token: 0x0400BD6B RID: 48491
				public static LocString DESC = "Now if only we had some water balloons...";

				// Token: 0x0400BD6C RID: 48492
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to be drawn from rocket storage via ",
					UI.FormatAsLink("Pipes", "LIQUIDCONDUIT"),
					".\n\nDraws liquid from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x02002B02 RID: 11010
			public class ROCKETINTERIORGASINPUT
			{
				// Token: 0x0400BD6D RID: 48493
				public static LocString NAME = UI.FormatAsLink("Gas Intake Fitting", "ROCKETINTERIORGASINPUT");

				// Token: 0x0400BD6E RID: 48494
				public static LocString DESC = "It's basically central-vac.";

				// Token: 0x0400BD6F RID: 48495
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to be pumped into rocket storage via ",
					UI.FormatAsLink("Pipes", "GASCONDUIT"),
					".\n\nSends gas to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x02002B03 RID: 11011
			public class ROCKETINTERIORGASOUTPUT
			{
				// Token: 0x0400BD70 RID: 48496
				public static LocString NAME = UI.FormatAsLink("Gas Output Fitting", "ROCKETINTERIORGASOUTPUT");

				// Token: 0x0400BD71 RID: 48497
				public static LocString DESC = "Refreshing breezes, on-demand.";

				// Token: 0x0400BD72 RID: 48498
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to be drawn from rocket storage via ",
					UI.FormatAsLink("Pipes", "GASCONDUIT"),
					".\n\nDraws gas from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x02002B04 RID: 11012
			public class ROCKETINTERIORSOLIDINPUT
			{
				// Token: 0x0400BD73 RID: 48499
				public static LocString NAME = UI.FormatAsLink("Conveyor Receptacle Fitting", "ROCKETINTERIORSOLIDINPUT");

				// Token: 0x0400BD74 RID: 48500
				public static LocString DESC = "Why organize your shelves when you can just shove everything in here?";

				// Token: 0x0400BD75 RID: 48501
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" to be moved into rocket storage via ",
					UI.FormatAsLink("Conveyor Rails", "SOLIDCONDUIT"),
					".\n\nSends solid material to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x02002B05 RID: 11013
			public class ROCKETINTERIORSOLIDOUTPUT
			{
				// Token: 0x0400BD76 RID: 48502
				public static LocString NAME = UI.FormatAsLink("Conveyor Loader Fitting", "ROCKETINTERIORSOLIDOUTPUT");

				// Token: 0x0400BD77 RID: 48503
				public static LocString DESC = "For accessing your stored luggage mid-flight.";

				// Token: 0x0400BD78 RID: 48504
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" to be moved out of rocket storage via ",
					UI.FormatAsLink("Conveyor Rails", "SOLIDCONDUIT"),
					".\n\nDraws solid material from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x02002B06 RID: 11014
			public class WATERCOOLER
			{
				// Token: 0x0400BD79 RID: 48505
				public static LocString NAME = UI.FormatAsLink("Water Cooler", "WATERCOOLER");

				// Token: 0x0400BD7A RID: 48506
				public static LocString DESC = "Chatting with friends improves Duplicants' moods and reduces their stress.";

				// Token: 0x0400BD7B RID: 48507
				public static LocString EFFECT = "Provides a gathering place for Duplicants during Downtime.\n\nImproves Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";

				// Token: 0x02003A5B RID: 14939
				public class OPTION_TOOLTIPS
				{
					// Token: 0x0400EBAD RID: 60333
					public static LocString WATER = ELEMENTS.WATER.NAME + "\nPlain potable water";

					// Token: 0x0400EBAE RID: 60334
					public static LocString MILK = ELEMENTS.MILK.NAME + "\nA salty, green-hued beverage";
				}

				// Token: 0x02003A5C RID: 14940
				public class FACADES
				{
					// Token: 0x02003DBE RID: 15806
					public class DEFAULT_WATERCOOLER
					{
						// Token: 0x0400F44A RID: 62538
						public static LocString NAME = UI.FormatAsLink("Water Cooler", "WATERCOOLER");

						// Token: 0x0400F44B RID: 62539
						public static LocString DESC = "Where Duplicants sip and socialize.";
					}

					// Token: 0x02003DBF RID: 15807
					public class ROUND_BODY
					{
						// Token: 0x0400F44C RID: 62540
						public static LocString NAME = UI.FormatAsLink("Elegant Water Cooler", "WATERCOOLER");

						// Token: 0x0400F44D RID: 62541
						public static LocString DESC = "It really classes up a breakroom.";
					}

					// Token: 0x02003DC0 RID: 15808
					public class BALLOON
					{
						// Token: 0x0400F44E RID: 62542
						public static LocString NAME = UI.FormatAsLink("Inflatable Water Cooler", "WATERCOOLER");

						// Token: 0x0400F44F RID: 62543
						public static LocString DESC = "There's a funny aftertaste.";
					}

					// Token: 0x02003DC1 RID: 15809
					public class YELLOW_TARTAR
					{
						// Token: 0x0400F450 RID: 62544
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Water Cooler", "WATERCOOLER");

						// Token: 0x0400F451 RID: 62545
						public static LocString DESC = "Did someone boil eggs in this water?";
					}

					// Token: 0x02003DC2 RID: 15810
					public class RED_ROSE
					{
						// Token: 0x0400F452 RID: 62546
						public static LocString NAME = UI.FormatAsLink("Puce Pink Water Cooler", "WATERCOOLER");

						// Token: 0x0400F453 RID: 62547
						public static LocString DESC = "Rose-colored paper cups: the shatter-proof alternative to rose-colored glasses.";
					}

					// Token: 0x02003DC3 RID: 15811
					public class GREEN_MUSH
					{
						// Token: 0x0400F454 RID: 62548
						public static LocString NAME = UI.FormatAsLink("Mush Green Water Cooler", "WATERCOOLER");

						// Token: 0x0400F455 RID: 62549
						public static LocString DESC = "Ideal for post-Mush Bar palate cleansing.";
					}

					// Token: 0x02003DC4 RID: 15812
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400F456 RID: 62550
						public static LocString NAME = UI.FormatAsLink("Faint Purple Water Cooler", "WATERCOOLER");

						// Token: 0x0400F457 RID: 62551
						public static LocString DESC = "Most Duplicants agree that it really should dispense juice.";
					}

					// Token: 0x02003DC5 RID: 15813
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400F458 RID: 62552
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Water Cooler", "WATERCOOLER");

						// Token: 0x0400F459 RID: 62553
						public static LocString DESC = "Lightly salted with Duplicants' tears.";
					}
				}
			}

			// Token: 0x02002B07 RID: 11015
			public class ARCADEMACHINE
			{
				// Token: 0x0400BD7C RID: 48508
				public static LocString NAME = UI.FormatAsLink("Arcade Cabinet", "ARCADEMACHINE");

				// Token: 0x0400BD7D RID: 48509
				public static LocString DESC = "Komet Kablam-O!\nFor up to two players.";

				// Token: 0x0400BD7E RID: 48510
				public static LocString EFFECT = "Allows Duplicants to play video games on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x02002B08 RID: 11016
			public class SINGLEPLAYERARCADE
			{
				// Token: 0x0400BD7F RID: 48511
				public static LocString NAME = UI.FormatAsLink("Single Player Arcade", "SINGLEPLAYERARCADE");

				// Token: 0x0400BD80 RID: 48512
				public static LocString DESC = "Space Brawler IV! For one player.";

				// Token: 0x0400BD81 RID: 48513
				public static LocString EFFECT = "Allows a Duplicant to play video games solo on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x02002B09 RID: 11017
			public class PHONOBOX
			{
				// Token: 0x0400BD82 RID: 48514
				public static LocString NAME = UI.FormatAsLink("Jukebot", "PHONOBOX");

				// Token: 0x0400BD83 RID: 48515
				public static LocString DESC = "Dancing helps Duplicants get their innermost feelings out.";

				// Token: 0x0400BD84 RID: 48516
				public static LocString EFFECT = "Plays music for Duplicants to dance to on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x02002B0A RID: 11018
			public class JUICER
			{
				// Token: 0x0400BD85 RID: 48517
				public static LocString NAME = UI.FormatAsLink("Juicer", "JUICER");

				// Token: 0x0400BD86 RID: 48518
				public static LocString DESC = "Fruity juice can really brighten a Duplicant's breaktime";

				// Token: 0x0400BD87 RID: 48519
				public static LocString EFFECT = "Provides refreshment for Duplicants on their breaks.\n\nDrinking juice increases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x02002B0B RID: 11019
			public class ESPRESSOMACHINE
			{
				// Token: 0x0400BD88 RID: 48520
				public static LocString NAME = UI.FormatAsLink("Espresso Machine", "ESPRESSOMACHINE");

				// Token: 0x0400BD89 RID: 48521
				public static LocString DESC = "A shot of espresso helps Duplicants relax after a long day.";

				// Token: 0x0400BD8A RID: 48522
				public static LocString EFFECT = "Provides refreshment for Duplicants on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x02002B0C RID: 11020
			public class TELEPHONE
			{
				// Token: 0x0400BD8B RID: 48523
				public static LocString NAME = UI.FormatAsLink("Party Line Phone", "TELEPHONE");

				// Token: 0x0400BD8C RID: 48524
				public static LocString DESC = "You never know who you'll meet on the other line.";

				// Token: 0x0400BD8D RID: 48525
				public static LocString EFFECT = "Can be used by one Duplicant to chat with themselves or with other Duplicants in different locations.\n\nChatting increases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";

				// Token: 0x0400BD8E RID: 48526
				public static LocString EFFECT_BABBLE = "{attrib}: {amount} (No One)";

				// Token: 0x0400BD8F RID: 48527
				public static LocString EFFECT_BABBLE_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat only with themselves.";

				// Token: 0x0400BD90 RID: 48528
				public static LocString EFFECT_CHAT = "{attrib}: {amount} (At least one Duplicant)";

				// Token: 0x0400BD91 RID: 48529
				public static LocString EFFECT_CHAT_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat with at least one other Duplicant.";

				// Token: 0x0400BD92 RID: 48530
				public static LocString EFFECT_LONG_DISTANCE = "{attrib}: {amount} (At least one Duplicant across space)";

				// Token: 0x0400BD93 RID: 48531
				public static LocString EFFECT_LONG_DISTANCE_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat with at least one other Duplicant across space.";
			}

			// Token: 0x02002B0D RID: 11021
			public class MODULARLIQUIDINPUT
			{
				// Token: 0x0400BD94 RID: 48532
				public static LocString NAME = UI.FormatAsLink("Liquid Input Hub", "MODULARLIQUIDINPUT");

				// Token: 0x0400BD95 RID: 48533
				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + ".";
			}

			// Token: 0x02002B0E RID: 11022
			public class MODULARSOLIDINPUT
			{
				// Token: 0x0400BD96 RID: 48534
				public static LocString NAME = UI.FormatAsLink("Solid Input Hub", "MODULARSOLIDINPUT");

				// Token: 0x0400BD97 RID: 48535
				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Solids", "ELEMENTS_SOLID") + ".";
			}

			// Token: 0x02002B0F RID: 11023
			public class MODULARGASINPUT
			{
				// Token: 0x0400BD98 RID: 48536
				public static LocString NAME = UI.FormatAsLink("Gas Input Hub", "MODULARGASINPUT");

				// Token: 0x0400BD99 RID: 48537
				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".";
			}

			// Token: 0x02002B10 RID: 11024
			public class MECHANICALSURFBOARD
			{
				// Token: 0x0400BD9A RID: 48538
				public static LocString NAME = UI.FormatAsLink("Mechanical Surfboard", "MECHANICALSURFBOARD");

				// Token: 0x0400BD9B RID: 48539
				public static LocString DESC = "Mechanical waves make for radical relaxation time.";

				// Token: 0x0400BD9C RID: 48540
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nSome ",
					UI.FormatAsLink("Water", "WATER"),
					" gets splashed on the floor during use."
				});

				// Token: 0x0400BD9D RID: 48541
				public static LocString WATER_REQUIREMENT = "{element}: {amount}";

				// Token: 0x0400BD9E RID: 48542
				public static LocString WATER_REQUIREMENT_TOOLTIP = "This building must be filled with {amount} {element} in order to function.";

				// Token: 0x0400BD9F RID: 48543
				public static LocString LEAK_REQUIREMENT = "Spillage: {amount}";

				// Token: 0x0400BDA0 RID: 48544
				public static LocString LEAK_REQUIREMENT_TOOLTIP = "This building will spill {amount} of its contents on to the floor during use, which must be replenished.";
			}

			// Token: 0x02002B11 RID: 11025
			public class SAUNA
			{
				// Token: 0x0400BDA1 RID: 48545
				public static LocString NAME = UI.FormatAsLink("Sauna", "SAUNA");

				// Token: 0x0400BDA2 RID: 48546
				public static LocString DESC = "A steamy sauna soothes away all the aches and pains.";

				// Token: 0x0400BDA3 RID: 48547
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Steam", "STEAM"),
					" to create a relaxing atmosphere.\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and provides a lingering sense of warmth."
				});
			}

			// Token: 0x02002B12 RID: 11026
			public class BEACHCHAIR
			{
				// Token: 0x0400BDA4 RID: 48548
				public static LocString NAME = UI.FormatAsLink("Beach Chair", "BEACHCHAIR");

				// Token: 0x0400BDA5 RID: 48549
				public static LocString DESC = "Soak up some relaxing sun rays.";

				// Token: 0x0400BDA6 RID: 48550
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Duplicants can relax by lounging in ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					".\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x0400BDA7 RID: 48551
				public static LocString LIGHTEFFECT_LOW = "{attrib}: {amount} (Dim Light)";

				// Token: 0x0400BDA8 RID: 48552
				public static LocString LIGHTEFFECT_LOW_TOOLTIP = "Duplicants will gain {amount} {attrib} if this building is in light dimmer than {lux}.";

				// Token: 0x0400BDA9 RID: 48553
				public static LocString LIGHTEFFECT_HIGH = "{attrib}: {amount} (Bright Light)";

				// Token: 0x0400BDAA RID: 48554
				public static LocString LIGHTEFFECT_HIGH_TOOLTIP = "Duplicants will gain {amount} {attrib} if this building is in at least {lux} light.";
			}

			// Token: 0x02002B13 RID: 11027
			public class SUNLAMP
			{
				// Token: 0x0400BDAB RID: 48555
				public static LocString NAME = UI.FormatAsLink("Sun Lamp", "SUNLAMP");

				// Token: 0x0400BDAC RID: 48556
				public static LocString DESC = "An artificial ray of sunshine.";

				// Token: 0x0400BDAD RID: 48557
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Gives off ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" level Lux.\n\nCan be paired with ",
					UI.FormatAsLink("Beach Chairs", "BEACHCHAIR"),
					"."
				});
			}

			// Token: 0x02002B14 RID: 11028
			public class VERTICALWINDTUNNEL
			{
				// Token: 0x0400BDAE RID: 48558
				public static LocString NAME = UI.FormatAsLink("Vertical Wind Tunnel", "VERTICALWINDTUNNEL");

				// Token: 0x0400BDAF RID: 48559
				public static LocString DESC = "Duplicants love the feeling of high-powered wind through their hair.";

				// Token: 0x0400BDB0 RID: 48560
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Must be connected to a ",
					UI.FormatAsLink("Power Source", "POWER"),
					". To properly function, the area under this building must be left vacant.\n\nIncreases Duplicants ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x0400BDB1 RID: 48561
				public static LocString DISPLACEMENTEFFECT = "Gas Displacement: {amount}";

				// Token: 0x0400BDB2 RID: 48562
				public static LocString DISPLACEMENTEFFECT_TOOLTIP = "This building will displace {amount} Gas while in use.";
			}

			// Token: 0x02002B15 RID: 11029
			public class TELEPORTALPAD
			{
				// Token: 0x0400BDB3 RID: 48563
				public static LocString NAME = "Teleporter Pad";

				// Token: 0x0400BDB4 RID: 48564
				public static LocString DESC = "Duplicants are just atoms as far as the pad's concerned.";

				// Token: 0x0400BDB5 RID: 48565
				public static LocString EFFECT = "Instantly transports Duplicants and items to another portal with the same portal code.";

				// Token: 0x0400BDB6 RID: 48566
				public static LocString LOGIC_PORT = "Portal Code Input";

				// Token: 0x0400BDB7 RID: 48567
				public static LocString LOGIC_PORT_ACTIVE = "1";

				// Token: 0x0400BDB8 RID: 48568
				public static LocString LOGIC_PORT_INACTIVE = "0";
			}

			// Token: 0x02002B16 RID: 11030
			public class CHECKPOINT
			{
				// Token: 0x0400BDB9 RID: 48569
				public static LocString NAME = UI.FormatAsLink("Duplicant Checkpoint", "CHECKPOINT");

				// Token: 0x0400BDBA RID: 48570
				public static LocString DESC = "Checkpoints can be connected to automated sensors to determine when it's safe to enter.";

				// Token: 0x0400BDBB RID: 48571
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to pass when receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					".\n\nPrevents Duplicants from passing when receiving a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x0400BDBC RID: 48572
				public static LocString LOGIC_PORT = "Duplicant Stop/Go";

				// Token: 0x0400BDBD RID: 48573
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Duplicant passage";

				// Token: 0x0400BDBE RID: 48574
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Duplicant passage";
			}

			// Token: 0x02002B17 RID: 11031
			public class FIREPOLE
			{
				// Token: 0x0400BDBF RID: 48575
				public static LocString NAME = UI.FormatAsLink("Fire Pole", "FIREPOLE");

				// Token: 0x0400BDC0 RID: 48576
				public static LocString DESC = "Build these in addition to ladders for efficient upward and downward movement.";

				// Token: 0x0400BDC1 RID: 48577
				public static LocString EFFECT = "Allows rapid Duplicant descent.\n\nSignificantly slows upward climbing.";
			}

			// Token: 0x02002B18 RID: 11032
			public class FLOORSWITCH
			{
				// Token: 0x0400BDC2 RID: 48578
				public static LocString NAME = UI.FormatAsLink("Weight Plate", "FLOORSWITCH");

				// Token: 0x0400BDC3 RID: 48579
				public static LocString DESC = "Weight plates can be used to turn on amenities only when Duplicants pass by.";

				// Token: 0x0400BDC4 RID: 48580
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when an object or Duplicant is placed atop of it.\n\nCannot be triggered by ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" or ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					"."
				});

				// Token: 0x0400BDC5 RID: 48581
				public static LocString LOGIC_PORT_DESC = UI.FormatAsLink("Active", "LOGIC") + "/" + UI.FormatAsLink("Inactive", "LOGIC");
			}

			// Token: 0x02002B19 RID: 11033
			public class KILN
			{
				// Token: 0x0400BDC6 RID: 48582
				public static LocString NAME = UI.FormatAsLink("Kiln", "KILN");

				// Token: 0x0400BDC7 RID: 48583
				public static LocString DESC = "It gets quite hot.";

				// Token: 0x0400BDC8 RID: 48584
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Fires ",
					UI.FormatAsLink("Clay", "CLAY"),
					" to produce ",
					UI.FormatAsLink("Ceramic", "CERAMIC"),
					", and ",
					UI.FormatAsLink("Coal", "CARBON"),
					" or ",
					UI.FormatAsLink("Wood", "WOOD"),
					" to produce ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x02002B1A RID: 11034
			public class LIQUIDFUELTANK
			{
				// Token: 0x0400BDC9 RID: 48585
				public static LocString NAME = UI.FormatAsLink("Liquid Fuel Tank", "LIQUIDFUELTANK");

				// Token: 0x0400BDCA RID: 48586
				public static LocString DESC = "Storing additional fuel increases the distance a rocket can travel before returning.";

				// Token: 0x0400BDCB RID: 48587
				public static LocString EFFECT = "Stores the " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " fuel piped into it to supply rocket engines.\n\nThe stored fuel type is determined by the rocket engine it is built upon.";
			}

			// Token: 0x02002B1B RID: 11035
			public class LIQUIDFUELTANKCLUSTER
			{
				// Token: 0x0400BDCC RID: 48588
				public static LocString NAME = UI.FormatAsLink("Large Liquid Fuel Tank", "LIQUIDFUELTANKCLUSTER");

				// Token: 0x0400BDCD RID: 48589
				public static LocString DESC = "Storing additional fuel increases the distance a rocket can travel before returning.";

				// Token: 0x0400BDCE RID: 48590
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" fuel piped into it to supply rocket engines.\n\nThe stored fuel type is determined by the rocket engine it is built upon. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002B1C RID: 11036
			public class LANDING_POD
			{
				// Token: 0x0400BDCF RID: 48591
				public static LocString NAME = "Spacefarer Deploy Pod";

				// Token: 0x0400BDD0 RID: 48592
				public static LocString DESC = "Geronimo!";

				// Token: 0x0400BDD1 RID: 48593
				public static LocString EFFECT = "Contains a Duplicant deployed from orbit.\n\nPod will disintegrate on arrival.";
			}

			// Token: 0x02002B1D RID: 11037
			public class ROCKETPOD
			{
				// Token: 0x0400BDD2 RID: 48594
				public static LocString NAME = UI.FormatAsLink("Trailblazer Deploy Pod", "ROCKETPOD");

				// Token: 0x0400BDD3 RID: 48595
				public static LocString DESC = "The Duplicant inside is equal parts nervous and excited.";

				// Token: 0x0400BDD4 RID: 48596
				public static LocString EFFECT = "Contains a Duplicant deployed from orbit by a " + BUILDINGS.PREFABS.PIONEERMODULE.NAME + ".\n\nPod will disintegrate on arrival.";
			}

			// Token: 0x02002B1E RID: 11038
			public class SCOUTROCKETPOD
			{
				// Token: 0x0400BDD5 RID: 48597
				public static LocString NAME = UI.FormatAsLink("Rover's Doghouse", "SCOUTROCKETPOD");

				// Token: 0x0400BDD6 RID: 48598
				public static LocString DESC = "Good luck out there, boy!";

				// Token: 0x0400BDD7 RID: 48599
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Contains a ",
					UI.FormatAsLink("Rover", "SCOUT"),
					" deployed from an orbiting ",
					BUILDINGS.PREFABS.SCOUTMODULE.NAME,
					".\n\nPod will disintegrate on arrival."
				});
			}

			// Token: 0x02002B1F RID: 11039
			public class ROCKETCOMMANDCONSOLE
			{
				// Token: 0x0400BDD8 RID: 48600
				public static LocString NAME = UI.FormatAsLink("Rocket Cockpit", "ROCKETCOMMANDCONSOLE");

				// Token: 0x0400BDD9 RID: 48601
				public static LocString DESC = "Looks kinda fun.";

				// Token: 0x0400BDDA RID: 48602
				public static LocString EFFECT = "Allows a Duplicant to pilot a rocket.\n\nCargo rockets must possess a Rocket Cockpit in order to function.";
			}

			// Token: 0x02002B20 RID: 11040
			public class ROCKETENVELOPETILE
			{
				// Token: 0x0400BDDB RID: 48603
				public static LocString NAME = UI.FormatAsLink("Rocket", "ROCKETENVELOPETILE");

				// Token: 0x0400BDDC RID: 48604
				public static LocString DESC = "Keeps the space out.";

				// Token: 0x0400BDDD RID: 48605
				public static LocString EFFECT = "The walls of a rocket.";
			}

			// Token: 0x02002B21 RID: 11041
			public class ROCKETENVELOPEWINDOWTILE
			{
				// Token: 0x0400BDDE RID: 48606
				public static LocString NAME = UI.FormatAsLink("Rocket Window", "ROCKETENVELOPEWINDOWTILE");

				// Token: 0x0400BDDF RID: 48607
				public static LocString DESC = "I can see my asteroid from here!";

				// Token: 0x0400BDE0 RID: 48608
				public static LocString EFFECT = "The window of a rocket.";
			}

			// Token: 0x02002B22 RID: 11042
			public class ROCKETWALLTILE
			{
				// Token: 0x0400BDE1 RID: 48609
				public static LocString NAME = UI.FormatAsLink("Rocket Wall", "ROCKETENVELOPETILE");

				// Token: 0x0400BDE2 RID: 48610
				public static LocString DESC = "Keeps the space out.";

				// Token: 0x0400BDE3 RID: 48611
				public static LocString EFFECT = "The walls of a rocket.";
			}

			// Token: 0x02002B23 RID: 11043
			public class SMALLOXIDIZERTANK
			{
				// Token: 0x0400BDE4 RID: 48612
				public static LocString NAME = UI.FormatAsLink("Small Solid Oxidizer Tank", "SMALLOXIDIZERTANK");

				// Token: 0x0400BDE5 RID: 48613
				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				// Token: 0x0400BDE6 RID: 48614
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Fertilizer", "Fertilizer"),
					" and ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" for burning rocket fuels. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				// Token: 0x0400BDE7 RID: 48615
				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			// Token: 0x02002B24 RID: 11044
			public class OXIDIZERTANK
			{
				// Token: 0x0400BDE8 RID: 48616
				public static LocString NAME = UI.FormatAsLink("Solid Oxidizer Tank", "OXIDIZERTANK");

				// Token: 0x0400BDE9 RID: 48617
				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				// Token: 0x0400BDEA RID: 48618
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Oxylite", "OXYROCK") + " and other oxidizers for burning rocket fuels.";

				// Token: 0x0400BDEB RID: 48619
				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			// Token: 0x02002B25 RID: 11045
			public class OXIDIZERTANKCLUSTER
			{
				// Token: 0x0400BDEC RID: 48620
				public static LocString NAME = UI.FormatAsLink("Large Solid Oxidizer Tank", "OXIDIZERTANKCLUSTER");

				// Token: 0x0400BDED RID: 48621
				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				// Token: 0x0400BDEE RID: 48622
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" and other oxidizers for burning rocket fuels.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				// Token: 0x0400BDEF RID: 48623
				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			// Token: 0x02002B26 RID: 11046
			public class OXIDIZERTANKLIQUID
			{
				// Token: 0x0400BDF0 RID: 48624
				public static LocString NAME = UI.FormatAsLink("Liquid Oxidizer Tank", "OXIDIZERTANKLIQUID");

				// Token: 0x0400BDF1 RID: 48625
				public static LocString DESC = "Liquid oxygen improves the thrust-to-mass ratio of rocket fuels.";

				// Token: 0x0400BDF2 RID: 48626
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN") + " for burning rocket fuels.";
			}

			// Token: 0x02002B27 RID: 11047
			public class OXIDIZERTANKLIQUIDCLUSTER
			{
				// Token: 0x0400BDF3 RID: 48627
				public static LocString NAME = UI.FormatAsLink("Liquid Oxidizer Tank", "OXIDIZERTANKLIQUIDCLUSTER");

				// Token: 0x0400BDF4 RID: 48628
				public static LocString DESC = "Liquid oxygen improves the thrust-to-mass ratio of rocket fuels.";

				// Token: 0x0400BDF5 RID: 48629
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN"),
					" for burning rocket fuels. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002B28 RID: 11048
			public class LIQUIDCONDITIONER
			{
				// Token: 0x0400BDF6 RID: 48630
				public static LocString NAME = UI.FormatAsLink("Thermo Aquatuner", "LIQUIDCONDITIONER");

				// Token: 0x0400BDF7 RID: 48631
				public static LocString DESC = "A thermo aquatuner cools liquid and outputs the heat elsewhere.";

				// Token: 0x0400BDF8 RID: 48632
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Cools the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" piped through it, but outputs ",
					UI.FormatAsLink("Heat", "HEAT"),
					" in its immediate vicinity."
				});
			}

			// Token: 0x02002B29 RID: 11049
			public class LIQUIDCARGOBAY
			{
				// Token: 0x0400BDF9 RID: 48633
				public static LocString NAME = UI.FormatAsLink("Liquid Cargo Tank", "LIQUIDCARGOBAY");

				// Token: 0x0400BDFA RID: 48634
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x0400BDFB RID: 48635
				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x02002B2A RID: 11050
			public class LIQUIDCARGOBAYCLUSTER
			{
				// Token: 0x0400BDFC RID: 48636
				public static LocString NAME = UI.FormatAsLink("Large Liquid Cargo Tank", "LIQUIDCARGOBAYCLUSTER");

				// Token: 0x0400BDFD RID: 48637
				public static LocString DESC = "Holds more than a regular cargo tank.";

				// Token: 0x0400BDFE RID: 48638
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002B2B RID: 11051
			public class LIQUIDCARGOBAYSMALL
			{
				// Token: 0x0400BDFF RID: 48639
				public static LocString NAME = UI.FormatAsLink("Liquid Cargo Tank", "LIQUIDCARGOBAYSMALL");

				// Token: 0x0400BE00 RID: 48640
				public static LocString DESC = "Duplicants will fill cargo tanks with whatever resources they find during space missions.";

				// Token: 0x0400BE01 RID: 48641
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002B2C RID: 11052
			public class LUXURYBED
			{
				// Token: 0x0400BE02 RID: 48642
				public static LocString NAME = UI.FormatAsLink("Comfy Bed", "LUXURYBED");

				// Token: 0x0400BE03 RID: 48643
				public static LocString DESC = "Duplicants prefer comfy beds to cots and wake up more rested after sleeping in them.";

				// Token: 0x0400BE04 RID: 48644
				public static LocString EFFECT = "Provides a sleeping area for one Duplicant and restores additional stamina.\n\nDuplicants will automatically sleep in their assigned beds at night.";

				// Token: 0x02003A5D RID: 14941
				public class FACADES
				{
					// Token: 0x02003DC6 RID: 15814
					public class DEFAULT_LUXURYBED
					{
						// Token: 0x0400F45A RID: 62554
						public static LocString NAME = UI.FormatAsLink("Comfy Bed", "LUXURYBED");

						// Token: 0x0400F45B RID: 62555
						public static LocString DESC = "Much comfier than a cot.";
					}

					// Token: 0x02003DC7 RID: 15815
					public class GRANDPRIX
					{
						// Token: 0x0400F45C RID: 62556
						public static LocString NAME = UI.FormatAsLink("Grand Prix Bed", "LUXURYBED");

						// Token: 0x0400F45D RID: 62557
						public static LocString DESC = "Where every Duplicant wakes up a winner.";
					}

					// Token: 0x02003DC8 RID: 15816
					public class BOAT
					{
						// Token: 0x0400F45E RID: 62558
						public static LocString NAME = UI.FormatAsLink("Dreamboat Bed", "LUXURYBED");

						// Token: 0x0400F45F RID: 62559
						public static LocString DESC = "Ahoy! Set sail for zzzzz's.";
					}

					// Token: 0x02003DC9 RID: 15817
					public class ROCKET_BED
					{
						// Token: 0x0400F460 RID: 62560
						public static LocString NAME = UI.FormatAsLink("S.S. Napmaster Bed", "LUXURYBED");

						// Token: 0x0400F461 RID: 62561
						public static LocString DESC = "Launches sleepy Duplicants into a deep-space slumber.";
					}

					// Token: 0x02003DCA RID: 15818
					public class BOUNCY_BED
					{
						// Token: 0x0400F462 RID: 62562
						public static LocString NAME = UI.FormatAsLink("Bouncy Castle Bed", "LUXURYBED");

						// Token: 0x0400F463 RID: 62563
						public static LocString DESC = "An inflatable party prop makes a surprisingly good bed.";
					}

					// Token: 0x02003DCB RID: 15819
					public class PUFT_BED
					{
						// Token: 0x0400F464 RID: 62564
						public static LocString NAME = UI.FormatAsLink("Puft Bed", "LUXURYBED");

						// Token: 0x0400F465 RID: 62565
						public static LocString DESC = "A comfy, if somewhat 'fragrant', place to sleep.";
					}

					// Token: 0x02003DCC RID: 15820
					public class HAND
					{
						// Token: 0x0400F466 RID: 62566
						public static LocString NAME = UI.FormatAsLink("Cradled Bed", "LUXURYBED");

						// Token: 0x0400F467 RID: 62567
						public static LocString DESC = "It's so nice to be held.";
					}

					// Token: 0x02003DCD RID: 15821
					public class RUBIKS
					{
						// Token: 0x0400F468 RID: 62568
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Bed", "LUXURYBED");

						// Token: 0x0400F469 RID: 62569
						public static LocString DESC = "A little pattern recognition at bedtime soothes the mind.";
					}

					// Token: 0x02003DCE RID: 15822
					public class RED_ROSE
					{
						// Token: 0x0400F46A RID: 62570
						public static LocString NAME = UI.FormatAsLink("Comfy Puce Bed", "LUXURYBED");

						// Token: 0x0400F46B RID: 62571
						public static LocString DESC = "A pink-hued bed for rosy dreams.";
					}

					// Token: 0x02003DCF RID: 15823
					public class GREEN_MUSH
					{
						// Token: 0x0400F46C RID: 62572
						public static LocString NAME = UI.FormatAsLink("Comfy Mush Bed", "LUXURYBED");

						// Token: 0x0400F46D RID: 62573
						public static LocString DESC = "The mattress is so soft, it's almost impossible to climb out of.";
					}

					// Token: 0x02003DD0 RID: 15824
					public class YELLOW_TARTAR
					{
						// Token: 0x0400F46E RID: 62574
						public static LocString NAME = UI.FormatAsLink("Comfy Ick Bed", "LUXURYBED");

						// Token: 0x0400F46F RID: 62575
						public static LocString DESC = "When life is icky, bed rest is the only answer.";
					}

					// Token: 0x02003DD1 RID: 15825
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400F470 RID: 62576
						public static LocString NAME = UI.FormatAsLink("Comfy Fainting Bed", "LUXURYBED");

						// Token: 0x0400F471 RID: 62577
						public static LocString DESC = "A soft landing spot for swooners.";
					}
				}
			}

			// Token: 0x02002B2D RID: 11053
			public class LADDERBED
			{
				// Token: 0x0400BE05 RID: 48645
				public static LocString NAME = UI.FormatAsLink("Ladder Bed", "LADDERBED");

				// Token: 0x0400BE06 RID: 48646
				public static LocString DESC = "Duplicant's sleep will be interrupted if another Duplicant uses the ladder.";

				// Token: 0x0400BE07 RID: 48647
				public static LocString EFFECT = "Provides a sleeping area for one Duplicant and also functions as a ladder.\n\nDuplicants will automatically sleep in their assigned beds at night.";
			}

			// Token: 0x02002B2E RID: 11054
			public class MEDICALCOT
			{
				// Token: 0x0400BE08 RID: 48648
				public static LocString NAME = UI.FormatAsLink("Triage Cot", "MEDICALCOT");

				// Token: 0x0400BE09 RID: 48649
				public static LocString DESC = "Duplicants use triage cots to recover from physical injuries and receive aid from peers.";

				// Token: 0x0400BE0A RID: 48650
				public static LocString EFFECT = "Accelerates " + UI.FormatAsLink("Health", "HEALTH") + " restoration and the healing of physical injuries.\n\nRevives incapacitated Duplicants.";
			}

			// Token: 0x02002B2F RID: 11055
			public class DOCTORSTATION
			{
				// Token: 0x0400BE0B RID: 48651
				public static LocString NAME = UI.FormatAsLink("Sick Bay", "DOCTORSTATION");

				// Token: 0x0400BE0C RID: 48652
				public static LocString DESC = "Sick bays can be placed in hospital rooms to decrease the likelihood of disease spreading.";

				// Token: 0x0400BE0D RID: 48653
				public static LocString EFFECT = "Allows Duplicants to administer basic treatments to sick Duplicants.\n\nDuplicants must possess the Bedside Manner " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.";
			}

			// Token: 0x02002B30 RID: 11056
			public class ADVANCEDDOCTORSTATION
			{
				// Token: 0x0400BE0E RID: 48654
				public static LocString NAME = UI.FormatAsLink("Disease Clinic", "ADVANCEDDOCTORSTATION");

				// Token: 0x0400BE0F RID: 48655
				public static LocString DESC = "Disease clinics require power, but treat more serious illnesses than sick bays alone.";

				// Token: 0x0400BE10 RID: 48656
				public static LocString EFFECT = "Allows Duplicants to administer powerful treatments to sick Duplicants.\n\nDuplicants must possess the Advanced Medical Care " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.";
			}

			// Token: 0x02002B31 RID: 11057
			public class MASSAGETABLE
			{
				// Token: 0x0400BE11 RID: 48657
				public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

				// Token: 0x0400BE12 RID: 48658
				public static LocString DESC = "Massage tables quickly reduce extreme stress, at the cost of power production.";

				// Token: 0x0400BE13 RID: 48659
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Rapidly reduces ",
					UI.FormatAsLink("Stress", "STRESS"),
					" for the Duplicant user.\n\nDuplicants will automatically seek a massage table when ",
					UI.FormatAsLink("Stress", "STRESS"),
					" exceeds breaktime range."
				});

				// Token: 0x0400BE14 RID: 48660
				public static LocString ACTIVATE_TOOLTIP = "Duplicants must take a massage break when their " + UI.FormatAsKeyWord("Stress") + " reaches {0}%";

				// Token: 0x0400BE15 RID: 48661
				public static LocString DEACTIVATE_TOOLTIP = "Breaktime ends when " + UI.FormatAsKeyWord("Stress") + " is reduced to {0}%";

				// Token: 0x02003A5E RID: 14942
				public class FACADES
				{
					// Token: 0x02003DD2 RID: 15826
					public class DEFAULT_MASSAGETABLE
					{
						// Token: 0x0400F472 RID: 62578
						public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

						// Token: 0x0400F473 RID: 62579
						public static LocString DESC = "Massage tables quickly reduce extreme stress, at the cost of power production.";
					}

					// Token: 0x02003DD3 RID: 15827
					public class SHIATSU
					{
						// Token: 0x0400F474 RID: 62580
						public static LocString NAME = UI.FormatAsLink("Shiatsu Table", "MASSAGETABLE");

						// Token: 0x0400F475 RID: 62581
						public static LocString DESC = "Deep pressure for deep-seated stress.";
					}

					// Token: 0x02003DD4 RID: 15828
					public class MASSEUR_BALLOON
					{
						// Token: 0x0400F476 RID: 62582
						public static LocString NAME = UI.FormatAsLink("Inflatable Massage Table", "MASSAGETABLE");

						// Token: 0x0400F477 RID: 62583
						public static LocString DESC = "Inflates well-being, deflates stress.";
					}
				}
			}

			// Token: 0x02002B32 RID: 11058
			public class CEILINGLIGHT
			{
				// Token: 0x0400BE16 RID: 48662
				public static LocString NAME = UI.FormatAsLink("Ceiling Light", "CEILINGLIGHT");

				// Token: 0x0400BE17 RID: 48663
				public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";

				// Token: 0x0400BE18 RID: 48664
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Light", "LIGHT"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					".\n\nIncreases Duplicant workspeed within light radius."
				});

				// Token: 0x02003A5F RID: 14943
				public class FACADES
				{
					// Token: 0x02003DD5 RID: 15829
					public class DEFAULT_CEILINGLIGHT
					{
						// Token: 0x0400F478 RID: 62584
						public static LocString NAME = UI.FormatAsLink("Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400F479 RID: 62585
						public static LocString DESC = "It does not go on the floor.";
					}

					// Token: 0x02003DD6 RID: 15830
					public class LABFLASK
					{
						// Token: 0x0400F47A RID: 62586
						public static LocString NAME = UI.FormatAsLink("Lab Flask Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400F47B RID: 62587
						public static LocString DESC = "For best results, do not fill with liquids.";
					}

					// Token: 0x02003DD7 RID: 15831
					public class FAUXPIPE
					{
						// Token: 0x0400F47C RID: 62588
						public static LocString NAME = UI.FormatAsLink("Faux Pipe Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400F47D RID: 62589
						public static LocString DESC = "The height of plumbing-inspired interior design.";
					}

					// Token: 0x02003DD8 RID: 15832
					public class MINING
					{
						// Token: 0x0400F47E RID: 62590
						public static LocString NAME = UI.FormatAsLink("Mining Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400F47F RID: 62591
						public static LocString DESC = "The protective cage makes it the safest choice for underground parties.";
					}

					// Token: 0x02003DD9 RID: 15833
					public class BLOSSOM
					{
						// Token: 0x0400F480 RID: 62592
						public static LocString NAME = UI.FormatAsLink("Blossom Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400F481 RID: 62593
						public static LocString DESC = "For Duplicants who can't keep real plants alive.";
					}

					// Token: 0x02003DDA RID: 15834
					public class POLKADOT
					{
						// Token: 0x0400F482 RID: 62594
						public static LocString NAME = UI.FormatAsLink("Polka Dot Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400F483 RID: 62595
						public static LocString DESC = "A fun lampshade for fun spaces.";
					}

					// Token: 0x02003DDB RID: 15835
					public class RUBIKS
					{
						// Token: 0x0400F484 RID: 62596
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400F485 RID: 62597
						public static LocString DESC = "The initials E.R. are sewn into the lampshade.";
					}
				}
			}

			// Token: 0x02002B33 RID: 11059
			public class MERCURYCEILINGLIGHT
			{
				// Token: 0x0400BE19 RID: 48665
				public static LocString NAME = UI.FormatAsLink("Mercury Ceiling Light", "MERCURYCEILINGLIGHT");

				// Token: 0x0400BE1A RID: 48666
				public static LocString DESC = "Mercury ceiling lights take a while to reach full brightness, but once they do...zowie!";

				// Token: 0x0400BE1B RID: 48667
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Mercury", "MERCURY"),
					" and ",
					UI.FormatAsLink("Power", "POWER"),
					" to produce ",
					UI.FormatAsLink("Light", "LIGHT"),
					".\n\nLight reduces Duplicant stress and is required to grow certain plants."
				});
			}

			// Token: 0x02002B34 RID: 11060
			public class AIRFILTER
			{
				// Token: 0x0400BE1C RID: 48668
				public static LocString NAME = UI.FormatAsLink("Deodorizer", "AIRFILTER");

				// Token: 0x0400BE1D RID: 48669
				public static LocString DESC = "Oh! Citrus scented!";

				// Token: 0x0400BE1E RID: 48670
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Sand", "SAND"),
					" to filter ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					" from the air, reducing ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" spread."
				});
			}

			// Token: 0x02002B35 RID: 11061
			public class ARTIFACTANALYSISSTATION
			{
				// Token: 0x0400BE1F RID: 48671
				public static LocString NAME = UI.FormatAsLink("Artifact Analysis Station", "ARTIFACTANALYSISSTATION");

				// Token: 0x0400BE20 RID: 48672
				public static LocString DESC = "Discover the mysteries of the past.";

				// Token: 0x0400BE21 RID: 48673
				public static LocString EFFECT = "Analyses and extracts " + UI.FormatAsLink("Neutronium", "UNOBTANIUM") + " from artifacts of interest.";

				// Token: 0x0400BE22 RID: 48674
				public static LocString PAYLOAD_DROP_RATE = ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME + " drop chance: {chance}";

				// Token: 0x0400BE23 RID: 48675
				public static LocString PAYLOAD_DROP_RATE_TOOLTIP = "This artifact has a {chance} to drop a " + ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME + " when analyzed at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			// Token: 0x02002B36 RID: 11062
			public class CANVAS
			{
				// Token: 0x0400BE24 RID: 48676
				public static LocString NAME = UI.FormatAsLink("Blank Canvas", "CANVAS");

				// Token: 0x0400BE25 RID: 48677
				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				// Token: 0x0400BE26 RID: 48678
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				// Token: 0x0400BE27 RID: 48679
				public static LocString POORQUALITYNAME = "Crude Painting";

				// Token: 0x0400BE28 RID: 48680
				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				// Token: 0x0400BE29 RID: 48681
				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				// Token: 0x02003A60 RID: 14944
				public class FACADES
				{
					// Token: 0x02003DDC RID: 15836
					public class ART_A
					{
						// Token: 0x0400F486 RID: 62598
						public static LocString NAME = UI.FormatAsLink("Doodle Dee Duplicant", "ART_A");

						// Token: 0x0400F487 RID: 62599
						public static LocString DESC = "A sweet, amateurish interpretation of the Duplicant form.";
					}

					// Token: 0x02003DDD RID: 15837
					public class ART_B
					{
						// Token: 0x0400F488 RID: 62600
						public static LocString NAME = UI.FormatAsLink("Midnight Meal", "ART_B");

						// Token: 0x0400F489 RID: 62601
						public static LocString DESC = "The fast-food equivalent of high art.";
					}

					// Token: 0x02003DDE RID: 15838
					public class ART_C
					{
						// Token: 0x0400F48A RID: 62602
						public static LocString NAME = UI.FormatAsLink("Dupa Leesa", "ART_C");

						// Token: 0x0400F48B RID: 62603
						public static LocString DESC = "Some viewers swear they've seen it blink.";
					}

					// Token: 0x02003DDF RID: 15839
					public class ART_D
					{
						// Token: 0x0400F48C RID: 62604
						public static LocString NAME = UI.FormatAsLink("The Screech", "ART_D");

						// Token: 0x0400F48D RID: 62605
						public static LocString DESC = "If art could speak, this piece would be far less popular.";
					}

					// Token: 0x02003DE0 RID: 15840
					public class ART_E
					{
						// Token: 0x0400F48E RID: 62606
						public static LocString NAME = UI.FormatAsLink("Fridup Kallo", "ART_E");

						// Token: 0x0400F48F RID: 62607
						public static LocString DESC = "Scratching and sniffing the flower yields no scent.";
					}

					// Token: 0x02003DE1 RID: 15841
					public class ART_F
					{
						// Token: 0x0400F490 RID: 62608
						public static LocString NAME = UI.FormatAsLink("Moopoleon Bonafarte", "ART_F");

						// Token: 0x0400F491 RID: 62609
						public static LocString DESC = "Portrait of a leader astride their mighty steed.";
					}

					// Token: 0x02003DE2 RID: 15842
					public class ART_G
					{
						// Token: 0x0400F492 RID: 62610
						public static LocString NAME = UI.FormatAsLink("Expressive Genius", "ART_G");

						// Token: 0x0400F493 RID: 62611
						public static LocString DESC = "The raw emotion conveyed here often renders viewers speechless.";
					}

					// Token: 0x02003DE3 RID: 15843
					public class ART_H
					{
						// Token: 0x0400F494 RID: 62612
						public static LocString NAME = UI.FormatAsLink("The Smooch", "ART_H");

						// Token: 0x0400F495 RID: 62613
						public static LocString DESC = "A candid moment of affection between two organisms.";
					}

					// Token: 0x02003DE4 RID: 15844
					public class ART_I
					{
						// Token: 0x0400F496 RID: 62614
						public static LocString NAME = UI.FormatAsLink("Self-Self-Self Portrait", "ART_I");

						// Token: 0x0400F497 RID: 62615
						public static LocString DESC = "A multi-layered exploration of the artist as a subject.";
					}

					// Token: 0x02003DE5 RID: 15845
					public class ART_J
					{
						// Token: 0x0400F498 RID: 62616
						public static LocString NAME = UI.FormatAsLink("Nikola Devouring His Mush Bar", "ART_J");

						// Token: 0x0400F499 RID: 62617
						public static LocString DESC = "A painting that captures the true nature of hunger.";
					}

					// Token: 0x02003DE6 RID: 15846
					public class ART_K
					{
						// Token: 0x0400F49A RID: 62618
						public static LocString NAME = UI.FormatAsLink("Sketchy Fungi", "ART_K");

						// Token: 0x0400F49B RID: 62619
						public static LocString DESC = "The perfect painting for dark, dank spaces.";
					}

					// Token: 0x02003DE7 RID: 15847
					public class ART_L
					{
						// Token: 0x0400F49C RID: 62620
						public static LocString NAME = UI.FormatAsLink("Post-Ear Era", "ART_L");

						// Token: 0x0400F49D RID: 62621
						public static LocString DESC = "The furry hat helped keep the artist's bandage on.";
					}

					// Token: 0x02003DE8 RID: 15848
					public class ART_M
					{
						// Token: 0x0400F49E RID: 62622
						public static LocString NAME = UI.FormatAsLink("Maternal Gaze", "ART_M");

						// Token: 0x0400F49F RID: 62623
						public static LocString DESC = "She's not angry, just disappointed.";
					}

					// Token: 0x02003DE9 RID: 15849
					public class ART_O
					{
						// Token: 0x0400F4A0 RID: 62624
						public static LocString NAME = UI.FormatAsLink("Hands-On", "ART_O");

						// Token: 0x0400F4A1 RID: 62625
						public static LocString DESC = "It's all about cooperation, really.";
					}

					// Token: 0x02003DEA RID: 15850
					public class ART_N
					{
						// Token: 0x0400F4A2 RID: 62626
						public static LocString NAME = UI.FormatAsLink("Always Hope", "ART_N");

						// Token: 0x0400F4A3 RID: 62627
						public static LocString DESC = "Most Duplicants believe that the balloon in this image is about to be caught.";
					}

					// Token: 0x02003DEB RID: 15851
					public class ART_P
					{
						// Token: 0x0400F4A4 RID: 62628
						public static LocString NAME = UI.FormatAsLink("Pour Soul", "ART_P");

						// Token: 0x0400F4A5 RID: 62629
						public static LocString DESC = "It is a cruel guest who does not RSVP.";
					}

					// Token: 0x02003DEC RID: 15852
					public class ART_Q
					{
						// Token: 0x0400F4A6 RID: 62630
						public static LocString NAME = UI.FormatAsLink("Ore Else", "ART_Q");

						// Token: 0x0400F4A7 RID: 62631
						public static LocString DESC = "The only kind of gift that poorly behaved Duplicants can expect to receive.";
					}

					// Token: 0x02003DED RID: 15853
					public class ART_R
					{
						// Token: 0x0400F4A8 RID: 62632
						public static LocString NAME = UI.FormatAsLink("Lazer Pipz", "ART_R");

						// Token: 0x0400F4A9 RID: 62633
						public static LocString DESC = "It combines two things that everyone loves: pips and lasers.";
					}
				}
			}

			// Token: 0x02002B37 RID: 11063
			public class CANVASWIDE
			{
				// Token: 0x0400BE2A RID: 48682
				public static LocString NAME = UI.FormatAsLink("Landscape Canvas", "CANVASWIDE");

				// Token: 0x0400BE2B RID: 48683
				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				// Token: 0x0400BE2C RID: 48684
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				// Token: 0x0400BE2D RID: 48685
				public static LocString POORQUALITYNAME = "Crude Painting";

				// Token: 0x0400BE2E RID: 48686
				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				// Token: 0x0400BE2F RID: 48687
				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				// Token: 0x02003A61 RID: 14945
				public class FACADES
				{
					// Token: 0x02003DEE RID: 15854
					public class ART_WIDE_A
					{
						// Token: 0x0400F4AA RID: 62634
						public static LocString NAME = UI.FormatAsLink("The Twins", "ART_WIDE_A");

						// Token: 0x0400F4AB RID: 62635
						public static LocString DESC = "The effort is admirable, though the execution is not.";
					}

					// Token: 0x02003DEF RID: 15855
					public class ART_WIDE_B
					{
						// Token: 0x0400F4AC RID: 62636
						public static LocString NAME = UI.FormatAsLink("Ground Zero", "ART_WIDE_B");

						// Token: 0x0400F4AD RID: 62637
						public static LocString DESC = "Every story has its origin.";
					}

					// Token: 0x02003DF0 RID: 15856
					public class ART_WIDE_C
					{
						// Token: 0x0400F4AE RID: 62638
						public static LocString NAME = UI.FormatAsLink("Still Life with Barbeque and Frost Bun", "ART_WIDE_C");

						// Token: 0x0400F4AF RID: 62639
						public static LocString DESC = "Food this good deserves to be immortalized.";
					}

					// Token: 0x02003DF1 RID: 15857
					public class ART_WIDE_D
					{
						// Token: 0x0400F4B0 RID: 62640
						public static LocString NAME = UI.FormatAsLink("Composition with Three Colors", "ART_WIDE_D");

						// Token: 0x0400F4B1 RID: 62641
						public static LocString DESC = "All the other colors in the artist's palette had dried up.";
					}

					// Token: 0x02003DF2 RID: 15858
					public class ART_WIDE_E
					{
						// Token: 0x0400F4B2 RID: 62642
						public static LocString NAME = UI.FormatAsLink("Behold, A Fork", "ART_WIDE_E");

						// Token: 0x0400F4B3 RID: 62643
						public static LocString DESC = "Each tine represents a branch of science.";
					}

					// Token: 0x02003DF3 RID: 15859
					public class ART_WIDE_F
					{
						// Token: 0x0400F4B4 RID: 62644
						public static LocString NAME = UI.FormatAsLink("The Astronomer at Home", "ART_WIDE_F");

						// Token: 0x0400F4B5 RID: 62645
						public static LocString DESC = "Its companion piece, \"The Astronomer at Work\" was lost in a meteor shower.";
					}

					// Token: 0x02003DF4 RID: 15860
					public class ART_WIDE_G
					{
						// Token: 0x0400F4B6 RID: 62646
						public static LocString NAME = UI.FormatAsLink("Iconic Iteration", "ART_WIDE_G");

						// Token: 0x0400F4B7 RID: 62647
						public static LocString DESC = "For the art collector who doesn't mind a bit of repetition.";
					}

					// Token: 0x02003DF5 RID: 15861
					public class ART_WIDE_H
					{
						// Token: 0x0400F4B8 RID: 62648
						public static LocString NAME = UI.FormatAsLink("La Belle Meep", "ART_WIDE_H");

						// Token: 0x0400F4B9 RID: 62649
						public static LocString DESC = "A daring piece, guaranteed to cause a stir.";
					}

					// Token: 0x02003DF6 RID: 15862
					public class ART_WIDE_I
					{
						// Token: 0x0400F4BA RID: 62650
						public static LocString NAME = UI.FormatAsLink("Glorious Vole", "ART_WIDE_I");

						// Token: 0x0400F4BB RID: 62651
						public static LocString DESC = "A moody study of the renowned tunneler.";
					}

					// Token: 0x02003DF7 RID: 15863
					public class ART_WIDE_J
					{
						// Token: 0x0400F4BC RID: 62652
						public static LocString NAME = UI.FormatAsLink("The Swell Swell", "ART_WIDE_J");

						// Token: 0x0400F4BD RID: 62653
						public static LocString DESC = "As far as wave-themed art goes, it's great.";
					}

					// Token: 0x02003DF8 RID: 15864
					public class ART_WIDE_K
					{
						// Token: 0x0400F4BE RID: 62654
						public static LocString NAME = UI.FormatAsLink("Flight of the Slicksters", "ART_WIDE_K");

						// Token: 0x0400F4BF RID: 62655
						public static LocString DESC = "The delight on the subjects' faces is contagious.";
					}

					// Token: 0x02003DF9 RID: 15865
					public class ART_WIDE_L
					{
						// Token: 0x0400F4C0 RID: 62656
						public static LocString NAME = UI.FormatAsLink("The Shiny Night", "ART_WIDE_L");

						// Token: 0x0400F4C1 RID: 62657
						public static LocString DESC = "A dreamy abundance of swirls, whirls and whorls.";
					}

					// Token: 0x02003DFA RID: 15866
					public class ART_WIDE_M
					{
						// Token: 0x0400F4C2 RID: 62658
						public static LocString NAME = UI.FormatAsLink("Hot Afternoon", "ART_WIDE_M");

						// Token: 0x0400F4C3 RID: 62659
						public static LocString DESC = "Things get a bit melty if they're forgotten in the sun.";
					}

					// Token: 0x02003DFB RID: 15867
					public class ART_WIDE_O
					{
						// Token: 0x0400F4C4 RID: 62660
						public static LocString NAME = UI.FormatAsLink("Super Old Mural", "ART_WIDE_O");

						// Token: 0x0400F4C5 RID: 62661
						public static LocString DESC = "Even just exhaling nearby could damage this historical work.";
					}
				}
			}

			// Token: 0x02002B38 RID: 11064
			public class CANVASTALL
			{
				// Token: 0x0400BE30 RID: 48688
				public static LocString NAME = UI.FormatAsLink("Portrait Canvas", "CANVASTALL");

				// Token: 0x0400BE31 RID: 48689
				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				// Token: 0x0400BE32 RID: 48690
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				// Token: 0x0400BE33 RID: 48691
				public static LocString POORQUALITYNAME = "Crude Painting";

				// Token: 0x0400BE34 RID: 48692
				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				// Token: 0x0400BE35 RID: 48693
				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				// Token: 0x02003A62 RID: 14946
				public class FACADES
				{
					// Token: 0x02003DFC RID: 15868
					public class ART_TALL_A
					{
						// Token: 0x0400F4C6 RID: 62662
						public static LocString NAME = UI.FormatAsLink("Ode to O2", "ART_TALL_A");

						// Token: 0x0400F4C7 RID: 62663
						public static LocString DESC = "Even amateur art is essential to life.";
					}

					// Token: 0x02003DFD RID: 15869
					public class ART_TALL_B
					{
						// Token: 0x0400F4C8 RID: 62664
						public static LocString NAME = UI.FormatAsLink("A Cool Wheeze", "ART_TALL_B");

						// Token: 0x0400F4C9 RID: 62665
						public static LocString DESC = "It certainly is colorful.";
					}

					// Token: 0x02003DFE RID: 15870
					public class ART_TALL_C
					{
						// Token: 0x0400F4CA RID: 62666
						public static LocString NAME = UI.FormatAsLink("Luxe Splatter", "ART_TALL_C");

						// Token: 0x0400F4CB RID: 62667
						public static LocString DESC = "Chaotic, yet compelling.";
					}

					// Token: 0x02003DFF RID: 15871
					public class ART_TALL_D
					{
						// Token: 0x0400F4CC RID: 62668
						public static LocString NAME = UI.FormatAsLink("Pickled Meal Lice II", "ART_TALL_D");

						// Token: 0x0400F4CD RID: 62669
						public static LocString DESC = "It doesn't have to taste good, it's art.";
					}

					// Token: 0x02003E00 RID: 15872
					public class ART_TALL_E
					{
						// Token: 0x0400F4CE RID: 62670
						public static LocString NAME = UI.FormatAsLink("Fruit Face", "ART_TALL_E");

						// Token: 0x0400F4CF RID: 62671
						public static LocString DESC = "Rumor has it that the model was self-conscious about their uneven eyebrows.";
					}

					// Token: 0x02003E01 RID: 15873
					public class ART_TALL_F
					{
						// Token: 0x0400F4D0 RID: 62672
						public static LocString NAME = UI.FormatAsLink("Girl with the Blue Scarf", "ART_TALL_F");

						// Token: 0x0400F4D1 RID: 62673
						public static LocString DESC = "The earring is nice too.";
					}

					// Token: 0x02003E02 RID: 15874
					public class ART_TALL_G
					{
						// Token: 0x0400F4D2 RID: 62674
						public static LocString NAME = UI.FormatAsLink("A Farewell at Sunrise", "ART_TALL_G");

						// Token: 0x0400F4D3 RID: 62675
						public static LocString DESC = "A poetic ink painting depicting the beginning of an end.";
					}

					// Token: 0x02003E03 RID: 15875
					public class ART_TALL_H
					{
						// Token: 0x0400F4D4 RID: 62676
						public static LocString NAME = UI.FormatAsLink("Conqueror of Clusters", "ART_TALL_H");

						// Token: 0x0400F4D5 RID: 62677
						public static LocString DESC = "The type of painting that ambitious Duplicants gravitate to.";
					}

					// Token: 0x02003E04 RID: 15876
					public class ART_TALL_I
					{
						// Token: 0x0400F4D6 RID: 62678
						public static LocString NAME = UI.FormatAsLink("Pei Phone", "ART_TALL_I");

						// Token: 0x0400F4D7 RID: 62679
						public static LocString DESC = "When the future calls, Duplicants answer.";
					}

					// Token: 0x02003E05 RID: 15877
					public class ART_TALL_J
					{
						// Token: 0x0400F4D8 RID: 62680
						public static LocString NAME = UI.FormatAsLink("Duplicants of the Galaxy", "ART_TALL_J");

						// Token: 0x0400F4D9 RID: 62681
						public static LocString DESC = "A poster for a blockbuster film that was never made.";
					}

					// Token: 0x02003E06 RID: 15878
					public class ART_TALL_K
					{
						// Token: 0x0400F4DA RID: 62682
						public static LocString NAME = UI.FormatAsLink("Cubist Loo", "ART_TALL_K");

						// Token: 0x0400F4DB RID: 62683
						public static LocString DESC = "The glass and frame are hydrophobic, for easy cleaning.";
					}

					// Token: 0x02003E07 RID: 15879
					public class ART_TALL_M
					{
						// Token: 0x0400F4DC RID: 62684
						public static LocString NAME = UI.FormatAsLink("Do Not Disturb", "ART_TALL_M");

						// Token: 0x0400F4DD RID: 62685
						public static LocString DESC = "No one likes being interrupted when they're waiting for inspiration to strike.";
					}

					// Token: 0x02003E08 RID: 15880
					public class ART_TALL_L
					{
						// Token: 0x0400F4DE RID: 62686
						public static LocString NAME = UI.FormatAsLink("Mirror Ball", "ART_TALL_L");

						// Token: 0x0400F4DF RID: 62687
						public static LocString DESC = "Nearby, a companion animal waited for the object to be thrown.";
					}

					// Token: 0x02003E09 RID: 15881
					public class ART_TALL_P
					{
						// Token: 0x0400F4E0 RID: 62688
						public static LocString NAME = "The Feast";

						// Token: 0x0400F4E1 RID: 62689
						public static LocString DESC = "There were greasy fingerprints on the canvas even before the paint had dried.";
					}
				}
			}

			// Token: 0x02002B39 RID: 11065
			public class CO2SCRUBBER
			{
				// Token: 0x0400BE36 RID: 48694
				public static LocString NAME = UI.FormatAsLink("Carbon Skimmer", "CO2SCRUBBER");

				// Token: 0x0400BE37 RID: 48695
				public static LocString DESC = "Skimmers remove large amounts of carbon dioxide, but produce no breathable air.";

				// Token: 0x0400BE38 RID: 48696
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Water", "WATER"),
					" to filter ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" from the air."
				});
			}

			// Token: 0x02002B3A RID: 11066
			public class COMPOST
			{
				// Token: 0x0400BE39 RID: 48697
				public static LocString NAME = UI.FormatAsLink("Compost", "COMPOST");

				// Token: 0x0400BE3A RID: 48698
				public static LocString DESC = "Composts safely deal with biological waste, producing fresh dirt.";

				// Token: 0x0400BE3B RID: 48699
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reduces ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					", rotting ",
					UI.FormatAsLink("Foods", "FOOD"),
					", and discarded organics down into ",
					UI.FormatAsLink("Dirt", "DIRT"),
					"."
				});
			}

			// Token: 0x02002B3B RID: 11067
			public class COOKINGSTATION
			{
				// Token: 0x0400BE3C RID: 48700
				public static LocString NAME = UI.FormatAsLink("Electric Grill", "COOKINGSTATION");

				// Token: 0x0400BE3D RID: 48701
				public static LocString DESC = "Proper cooking eliminates foodborne disease and produces tasty, stress-relieving meals.";

				// Token: 0x0400BE3E RID: 48702
				public static LocString EFFECT = "Cooks a wide variety of improved " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002B3C RID: 11068
			public class CRYOTANK
			{
				// Token: 0x0400BE3F RID: 48703
				public static LocString NAME = UI.FormatAsLink("Cryotank 3000", "CRYOTANK");

				// Token: 0x0400BE40 RID: 48704
				public static LocString DESC = "The tank appears impossibly old, but smells crisp and brand new.\n\nA silhouette just barely visible through the frost of the glass.";

				// Token: 0x0400BE41 RID: 48705
				public static LocString DEFROSTBUTTON = "Defrost Friend";

				// Token: 0x0400BE42 RID: 48706
				public static LocString DEFROSTBUTTONTOOLTIP = "A new pal is just an icebreaker away";
			}

			// Token: 0x02002B3D RID: 11069
			public class GOURMETCOOKINGSTATION
			{
				// Token: 0x0400BE43 RID: 48707
				public static LocString NAME = UI.FormatAsLink("Gas Range", "GOURMETCOOKINGSTATION");

				// Token: 0x0400BE44 RID: 48708
				public static LocString DESC = "Luxury meals increase Duplicants' morale and prevent them from becoming stressed.";

				// Token: 0x0400BE45 RID: 48709
				public static LocString EFFECT = "Cooks a wide variety of quality " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002B3E RID: 11070
			public class DININGTABLE
			{
				// Token: 0x0400BE46 RID: 48710
				public static LocString NAME = UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x0400BE47 RID: 48711
				public static LocString DESC = "Duplicants prefer to dine at a table, rather than eat off the floor.";

				// Token: 0x0400BE48 RID: 48712
				public static LocString EFFECT = "Gives one Duplicant a place to eat.\n\nDuplicants will automatically eat at their assigned table when hungry.";
			}

			// Token: 0x02002B3F RID: 11071
			public class MULTIMINIONDININGTABLE
			{
				// Token: 0x0400BE49 RID: 48713
				public static LocString NAME = UI.FormatAsLink("Communal Table", "MULTIMINIONDININGTABLE");

				// Token: 0x0400BE4A RID: 48714
				public static LocString DESC = "Given the option, Duplicants prefer to dine with friends.";

				// Token: 0x0400BE4B RID: 48715
				public static LocString EFFECT = "Gives three Duplicants a place to eat.\n\nSharing a meal with one or more companions provides a " + UI.FormatAsLink("Morale", "MORALE") + " boost.";
			}

			// Token: 0x02002B40 RID: 11072
			public class DOOR
			{
				// Token: 0x0400BE4C RID: 48716
				public static LocString NAME = UI.FormatAsLink("Pneumatic Door", "DOOR");

				// Token: 0x0400BE4D RID: 48717
				public static LocString DESC = "Door controls can be used to prevent Duplicants from entering restricted areas.";

				// Token: 0x0400BE4E RID: 48718
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Encloses areas without blocking ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});

				// Token: 0x0400BE4F RID: 48719
				public static LocString PRESSURE_SUIT_REQUIRED = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " required {0}";

				// Token: 0x0400BE50 RID: 48720
				public static LocString PRESSURE_SUIT_NOT_REQUIRED = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " not required {0}";

				// Token: 0x0400BE51 RID: 48721
				public static LocString ABOVE = "above";

				// Token: 0x0400BE52 RID: 48722
				public static LocString BELOW = "below";

				// Token: 0x0400BE53 RID: 48723
				public static LocString LEFT = "on left";

				// Token: 0x0400BE54 RID: 48724
				public static LocString RIGHT = "on right";

				// Token: 0x0400BE55 RID: 48725
				public static LocString LOGIC_OPEN = "Open/Close";

				// Token: 0x0400BE56 RID: 48726
				public static LocString LOGIC_OPEN_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Open";

				// Token: 0x0400BE57 RID: 48727
				public static LocString LOGIC_OPEN_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Close and lock";

				// Token: 0x02003A63 RID: 14947
				public static class CONTROL_STATE
				{
					// Token: 0x02003E0A RID: 15882
					public class OPEN
					{
						// Token: 0x0400F4E2 RID: 62690
						public static LocString NAME = "Open";

						// Token: 0x0400F4E3 RID: 62691
						public static LocString TOOLTIP = "This door will remain open";
					}

					// Token: 0x02003E0B RID: 15883
					public class CLOSE
					{
						// Token: 0x0400F4E4 RID: 62692
						public static LocString NAME = "Lock";

						// Token: 0x0400F4E5 RID: 62693
						public static LocString TOOLTIP = "Nothing may pass through";
					}

					// Token: 0x02003E0C RID: 15884
					public class AUTO
					{
						// Token: 0x0400F4E6 RID: 62694
						public static LocString NAME = "Auto";

						// Token: 0x0400F4E7 RID: 62695
						public static LocString TOOLTIP = "Duplicants open and close this door as needed";
					}
				}
			}

			// Token: 0x02002B41 RID: 11073
			public class ELECTROBANKCHARGER
			{
				// Token: 0x0400BE58 RID: 48728
				public static LocString NAME = UI.FormatAsLink("Power Bank Charger", "ELECTROBANKCHARGER");

				// Token: 0x0400BE59 RID: 48729
				public static LocString DESC = "Bionic Duplicants rely on a steady supply of power to function.";

				// Token: 0x0400BE5A RID: 48730
				public static LocString EFFECT = "Converts empty " + UI.FormatAsLink("Eco Power Banks", "ELECTROBANK") + " into fully charged units ready for reuse.";
			}

			// Token: 0x02002B42 RID: 11074
			public class SMALLELECTROBANKDISCHARGER
			{
				// Token: 0x0400BE5B RID: 48731
				public static LocString NAME = UI.FormatAsLink("Compact Discharger", "SMALLELECTROBANKDISCHARGER");

				// Token: 0x0400BE5C RID: 48732
				public static LocString DESC = "A small standalone power center that can be mounted on the floor or wall.";

				// Token: 0x0400BE5D RID: 48733
				public static LocString EFFECT = "Converts stored energy from " + UI.FormatAsLink("Power Banks", "ELECTROBANK") + " into power for connected buildings.";
			}

			// Token: 0x02002B43 RID: 11075
			public class LARGEELECTROBANKDISCHARGER
			{
				// Token: 0x0400BE5E RID: 48734
				public static LocString NAME = UI.FormatAsLink("Large Discharger", "LARGEELECTROBANKDISCHARGER");

				// Token: 0x0400BE5F RID: 48735
				public static LocString DESC = "It's basically its own power grid.";

				// Token: 0x0400BE60 RID: 48736
				public static LocString EFFECT = "Efficiently converts stored energy from " + UI.FormatAsLink("Power Banks", "ELECTROBANK") + " into power for connected buildings.";
			}

			// Token: 0x02002B44 RID: 11076
			public class ELECTROLYZER
			{
				// Token: 0x0400BE61 RID: 48737
				public static LocString NAME = UI.FormatAsLink("Electrolyzer", "ELECTROLYZER");

				// Token: 0x0400BE62 RID: 48738
				public static LocString DESC = "Water goes in one end, life sustaining oxygen comes out the other.";

				// Token: 0x0400BE63 RID: 48739
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Water", "WATER"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x02002B45 RID: 11077
			public class RUSTDEOXIDIZER
			{
				// Token: 0x0400BE64 RID: 48740
				public static LocString NAME = UI.FormatAsLink("Rust Deoxidizer", "RUSTDEOXIDIZER");

				// Token: 0x0400BE65 RID: 48741
				public static LocString DESC = "Rust and salt goes in, oxygen comes out.";

				// Token: 0x0400BE66 RID: 48742
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Rust", "RUST"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Chlorine Gas", "CHLORINE"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x02002B46 RID: 11078
			public class DESALINATOR
			{
				// Token: 0x0400BE67 RID: 48743
				public static LocString NAME = UI.FormatAsLink("Desalinator", "DESALINATOR");

				// Token: 0x0400BE68 RID: 48744
				public static LocString DESC = "Salt can be refined into table salt for a mealtime morale boost.";

				// Token: 0x0400BE69 RID: 48745
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Removes ",
					UI.FormatAsLink("Salt", "SALT"),
					" from ",
					UI.FormatAsLink("Brine", "BRINE"),
					" or ",
					UI.FormatAsLink("Salt Water", "SALTWATER"),
					", producing ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});
			}

			// Token: 0x02002B47 RID: 11079
			public class POWERTRANSFORMERSMALL
			{
				// Token: 0x0400BE6A RID: 48746
				public static LocString NAME = UI.FormatAsLink("Power Transformer", "POWERTRANSFORMERSMALL");

				// Token: 0x0400BE6B RID: 48747
				public static LocString DESC = "Limiting the power drawn by wires prevents them from incurring overload damage.";

				// Token: 0x0400BE6C RID: 48748
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Limits ",
					UI.FormatAsLink("Power", "POWER"),
					" flowing through the Transformer to 1000 W.\n\nConnect ",
					UI.FormatAsLink("Batteries", "BATTERY"),
					" on the large side to act as a valve and prevent ",
					UI.FormatAsLink("Wires", "WIRE"),
					" from drawing more than 1000 W.\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002B48 RID: 11080
			public class POWERTRANSFORMER
			{
				// Token: 0x0400BE6D RID: 48749
				public static LocString NAME = UI.FormatAsLink("Large Power Transformer", "POWERTRANSFORMER");

				// Token: 0x0400BE6E RID: 48750
				public static LocString DESC = "It's a power transformer, but larger.";

				// Token: 0x0400BE6F RID: 48751
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Limits ",
					UI.FormatAsLink("Power", "POWER"),
					" flowing through the Transformer to 4 kW.\n\nConnect ",
					UI.FormatAsLink("Batteries", "BATTERY"),
					" on the large side to act as a valve and prevent ",
					UI.FormatAsLink("Wires", "WIRE"),
					" from drawing more than 4 kW.\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002B49 RID: 11081
			public class FLOORLAMP
			{
				// Token: 0x0400BE70 RID: 48752
				public static LocString NAME = UI.FormatAsLink("Lamp", "FLOORLAMP");

				// Token: 0x0400BE71 RID: 48753
				public static LocString DESC = "Any building's light emitting radius can be viewed in the light overlay.";

				// Token: 0x0400BE72 RID: 48754
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Light", "LIGHT"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					".\n\nIncreases Duplicant workspeed within light radius."
				});

				// Token: 0x02003A64 RID: 14948
				public class FACADES
				{
					// Token: 0x02003E0D RID: 15885
					public class DEFAULT_FLOORLAMP
					{
						// Token: 0x0400F4E8 RID: 62696
						public static LocString NAME = UI.FormatAsLink("Lamp", "FLOORLAMP");

						// Token: 0x0400F4E9 RID: 62697
						public static LocString DESC = "Any building's light emitting radius can be viewed in the light overlay.";
					}

					// Token: 0x02003E0E RID: 15886
					public class LEG
					{
						// Token: 0x0400F4EA RID: 62698
						public static LocString NAME = UI.FormatAsLink("Fragile Leg Lamp", "FLOORLAMP");

						// Token: 0x0400F4EB RID: 62699
						public static LocString DESC = "This lamp blazes forth in unparalleled glory.";
					}

					// Token: 0x02003E0F RID: 15887
					public class BRISTLEBLOSSOM
					{
						// Token: 0x0400F4EC RID: 62700
						public static LocString NAME = UI.FormatAsLink("Holiday Lamp", "FLOORLAMP");

						// Token: 0x0400F4ED RID: 62701
						public static LocString DESC = "It's a bit prickly, but it casts a festive glow.";
					}
				}
			}

			// Token: 0x02002B4A RID: 11082
			public class FLOWERVASE
			{
				// Token: 0x0400BE73 RID: 48755
				public static LocString NAME = UI.FormatAsLink("Flower Pot", "FLOWERVASE");

				// Token: 0x0400BE74 RID: 48756
				public static LocString DESC = "Flower pots allow decorative plants to be moved to new locations.";

				// Token: 0x0400BE75 RID: 48757
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					" when in use, contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x02003A65 RID: 14949
				public class FACADES
				{
					// Token: 0x02003E10 RID: 15888
					public class DEFAULT_FLOWERVASE
					{
						// Token: 0x0400F4EE RID: 62702
						public static LocString NAME = UI.FormatAsLink("Flower Pot", "FLOWERVASE");

						// Token: 0x0400F4EF RID: 62703
						public static LocString DESC = "The original container for plants on the move.";
					}

					// Token: 0x02003E11 RID: 15889
					public class RETRO_SUNNY
					{
						// Token: 0x0400F4F0 RID: 62704
						public static LocString NAME = UI.FormatAsLink("Sunny Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400F4F1 RID: 62705
						public static LocString DESC = "A funky yellow flower pot for plants on the move.";
					}

					// Token: 0x02003E12 RID: 15890
					public class RETRO_BOLD
					{
						// Token: 0x0400F4F2 RID: 62706
						public static LocString NAME = UI.FormatAsLink("Bold Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400F4F3 RID: 62707
						public static LocString DESC = "A funky red flower pot for plants on the move.";
					}

					// Token: 0x02003E13 RID: 15891
					public class RETRO_BRIGHT
					{
						// Token: 0x0400F4F4 RID: 62708
						public static LocString NAME = UI.FormatAsLink("Bright Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400F4F5 RID: 62709
						public static LocString DESC = "A funky green flower pot for plants on the move.";
					}

					// Token: 0x02003E14 RID: 15892
					public class RETRO_DREAMY
					{
						// Token: 0x0400F4F6 RID: 62710
						public static LocString NAME = UI.FormatAsLink("Dreamy Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400F4F7 RID: 62711
						public static LocString DESC = "A funky blue flower pot for plants on the move.";
					}

					// Token: 0x02003E15 RID: 15893
					public class RETRO_ELEGANT
					{
						// Token: 0x0400F4F8 RID: 62712
						public static LocString NAME = UI.FormatAsLink("Elegant Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400F4F9 RID: 62713
						public static LocString DESC = "A funky white flower pot for plants on the move.";
					}
				}
			}

			// Token: 0x02002B4B RID: 11083
			public class FLOWERVASEWALL
			{
				// Token: 0x0400BE76 RID: 48758
				public static LocString NAME = UI.FormatAsLink("Wall Pot", "FLOWERVASEWALL");

				// Token: 0x0400BE77 RID: 48759
				public static LocString DESC = "Placing a plant in a wall pot can add a spot of Decor to otherwise bare walls.";

				// Token: 0x0400BE78 RID: 48760
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					" when in use, contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a wall."
				});

				// Token: 0x02003A66 RID: 14950
				public class FACADES
				{
					// Token: 0x02003E16 RID: 15894
					public class DEFAULT_FLOWERVASEWALL
					{
						// Token: 0x0400F4FA RID: 62714
						public static LocString NAME = UI.FormatAsLink("Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400F4FB RID: 62715
						public static LocString DESC = "Facilitates vertical plant displays.";
					}

					// Token: 0x02003E17 RID: 15895
					public class RETRO_GREEN
					{
						// Token: 0x0400F4FC RID: 62716
						public static LocString NAME = UI.FormatAsLink("Bright Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400F4FD RID: 62717
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x02003E18 RID: 15896
					public class RETRO_YELLOW
					{
						// Token: 0x0400F4FE RID: 62718
						public static LocString NAME = UI.FormatAsLink("Sunny Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400F4FF RID: 62719
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x02003E19 RID: 15897
					public class RETRO_RED
					{
						// Token: 0x0400F500 RID: 62720
						public static LocString NAME = UI.FormatAsLink("Bold Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400F501 RID: 62721
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x02003E1A RID: 15898
					public class RETRO_BLUE
					{
						// Token: 0x0400F502 RID: 62722
						public static LocString NAME = UI.FormatAsLink("Dreamy Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400F503 RID: 62723
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x02003E1B RID: 15899
					public class RETRO_WHITE
					{
						// Token: 0x0400F504 RID: 62724
						public static LocString NAME = UI.FormatAsLink("Elegant Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400F505 RID: 62725
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}
				}
			}

			// Token: 0x02002B4C RID: 11084
			public class FLOWERVASEHANGING
			{
				// Token: 0x0400BE79 RID: 48761
				public static LocString NAME = UI.FormatAsLink("Hanging Pot", "FLOWERVASEHANGING");

				// Token: 0x0400BE7A RID: 48762
				public static LocString DESC = "Hanging pots can add some Decor to a room, without blocking buildings on the floor.";

				// Token: 0x0400BE7B RID: 48763
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					" when in use, contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a ceiling."
				});

				// Token: 0x02003A67 RID: 14951
				public class FACADES
				{
					// Token: 0x02003E1C RID: 15900
					public class RETRO_RED
					{
						// Token: 0x0400F506 RID: 62726
						public static LocString NAME = UI.FormatAsLink("Bold Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400F507 RID: 62727
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x02003E1D RID: 15901
					public class RETRO_GREEN
					{
						// Token: 0x0400F508 RID: 62728
						public static LocString NAME = UI.FormatAsLink("Bright Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400F509 RID: 62729
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x02003E1E RID: 15902
					public class RETRO_BLUE
					{
						// Token: 0x0400F50A RID: 62730
						public static LocString NAME = UI.FormatAsLink("Dreamy Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400F50B RID: 62731
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x02003E1F RID: 15903
					public class RETRO_YELLOW
					{
						// Token: 0x0400F50C RID: 62732
						public static LocString NAME = UI.FormatAsLink("Sunny Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400F50D RID: 62733
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x02003E20 RID: 15904
					public class RETRO_WHITE
					{
						// Token: 0x0400F50E RID: 62734
						public static LocString NAME = UI.FormatAsLink("Elegant Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400F50F RID: 62735
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x02003E21 RID: 15905
					public class BEAKER
					{
						// Token: 0x0400F510 RID: 62736
						public static LocString NAME = UI.FormatAsLink("Beaker Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400F511 RID: 62737
						public static LocString DESC = "A measured approach to indoor plant decor.";
					}

					// Token: 0x02003E22 RID: 15906
					public class RUBIKS
					{
						// Token: 0x0400F512 RID: 62738
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400F513 RID: 62739
						public static LocString DESC = "The real puzzle is how to keep indoor plants alive.";
					}
				}
			}

			// Token: 0x02002B4D RID: 11085
			public class FLOWERVASEHANGINGFANCY
			{
				// Token: 0x0400BE7C RID: 48764
				public static LocString NAME = UI.FormatAsLink("Aero Pot", "FLOWERVASEHANGINGFANCY");

				// Token: 0x0400BE7D RID: 48765
				public static LocString DESC = "Aero pots can be hung from the ceiling and have extremely high Decor.";

				// Token: 0x0400BE7E RID: 48766
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					" even when empty, contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a ceiling."
				});

				// Token: 0x02003A68 RID: 14952
				public class FACADES
				{
				}
			}

			// Token: 0x02002B4E RID: 11086
			public class FLUSHTOILET
			{
				// Token: 0x0400BE7F RID: 48767
				public static LocString NAME = UI.FormatAsLink("Lavatory", "FLUSHTOILET");

				// Token: 0x0400BE80 RID: 48768
				public static LocString DESC = "Lavatories transmit fewer germs to Duplicants' skin and require no emptying.";

				// Token: 0x0400BE81 RID: 48769
				public static LocString EFFECT = "Gives Duplicants a place to relieve themselves.\n\nSpreads very few " + UI.FormatAsLink("Germs", "DISEASE") + ".";

				// Token: 0x02003A69 RID: 14953
				public class FACADES
				{
					// Token: 0x02003E23 RID: 15907
					public class DEFAULT_FLUSHTOILET
					{
						// Token: 0x0400F514 RID: 62740
						public static LocString NAME = UI.FormatAsLink("Lavatory", "FLUSHTOILET");

						// Token: 0x0400F515 RID: 62741
						public static LocString DESC = "Lavatories transmit fewer germs to Duplicants' skin and require no emptying.";
					}

					// Token: 0x02003E24 RID: 15908
					public class POLKA_DARKPURPLERESIN
					{
						// Token: 0x0400F516 RID: 62742
						public static LocString NAME = UI.FormatAsLink("Mod Dot Lavatory", "FLUSHTOILET");

						// Token: 0x0400F517 RID: 62743
						public static LocString DESC = "For those who've really got to a-go-go.";
					}

					// Token: 0x02003E25 RID: 15909
					public class POLKA_DARKNAVYNOOKGREEN
					{
						// Token: 0x0400F518 RID: 62744
						public static LocString NAME = UI.FormatAsLink("Party Dot Lavatory", "FLUSHTOILET");

						// Token: 0x0400F519 RID: 62745
						public static LocString DESC = "Smooth moves happen here.";
					}

					// Token: 0x02003E26 RID: 15910
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400F51A RID: 62746
						public static LocString NAME = UI.FormatAsLink("Faint Purple Lavatory", "FLUSHTOILET");

						// Token: 0x0400F51B RID: 62747
						public static LocString DESC = "It's like pooping inside Hexalent fruit!";
					}

					// Token: 0x02003E27 RID: 15911
					public class YELLOW_TARTAR
					{
						// Token: 0x0400F51C RID: 62748
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Lavatory", "FLUSHTOILET");

						// Token: 0x0400F51D RID: 62749
						public static LocString DESC = "Someone thought it'd be a good idea to have the outside match the inside.";
					}

					// Token: 0x02003E28 RID: 15912
					public class RED_ROSE
					{
						// Token: 0x0400F51E RID: 62750
						public static LocString NAME = UI.FormatAsLink("Puce Pink Lavatory", "FLUSHTOILET");

						// Token: 0x0400F51F RID: 62751
						public static LocString DESC = "The scented pink toilet paper smells like a rosebush in a sewage plant.";
					}

					// Token: 0x02003E29 RID: 15913
					public class GREEN_MUSH
					{
						// Token: 0x0400F520 RID: 62752
						public static LocString NAME = UI.FormatAsLink("Mush Green Lavatory", "FLUSHTOILET");

						// Token: 0x0400F521 RID: 62753
						public static LocString DESC = "Mush in, mush out.";
					}

					// Token: 0x02003E2A RID: 15914
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400F522 RID: 62754
						public static LocString NAME = UI.FormatAsLink("Weepy Lavatory", "FLUSHTOILET");

						// Token: 0x0400F523 RID: 62755
						public static LocString DESC = "A private place to feel big feelings.";
					}
				}
			}

			// Token: 0x02002B4F RID: 11087
			public class SHOWER
			{
				// Token: 0x0400BE82 RID: 48770
				public static LocString NAME = UI.FormatAsLink("Shower", "SHOWER");

				// Token: 0x0400BE83 RID: 48771
				public static LocString DESC = "Regularly showering will prevent Duplicants spreading germs to the things they touch.";

				// Token: 0x0400BE84 RID: 48772
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Improves Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and removes surface ",
					UI.FormatAsLink("Germs", "DISEASE"),
					"."
				});
			}

			// Token: 0x02002B50 RID: 11088
			public class CONDUIT
			{
				// Token: 0x02003A6A RID: 14954
				public class STATUS_ITEM
				{
					// Token: 0x0400EBAF RID: 60335
					public static LocString NAME = "Marked for Emptying";

					// Token: 0x0400EBB0 RID: 60336
					public static LocString TOOLTIP = "Awaiting a " + UI.FormatAsLink("Plumber", "PLUMBER") + " to clear this pipe";
				}
			}

			// Token: 0x02002B51 RID: 11089
			public class MORBROVERMAKER
			{
				// Token: 0x0400BE85 RID: 48773
				public static LocString NAME = UI.FormatAsLink("Biobot Builder", "STORYTRAITMORBROVER");

				// Token: 0x0400BE86 RID: 48774
				public static LocString DESC = "Allows a skilled Duplicant to manufacture a steady supply of icky yet effective bots.";

				// Token: 0x0400BE87 RID: 48775
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
					" and ",
					UI.FormatAsLink("Steel", "STEEL"),
					" to craft biofueled machines that can be sent into hostile environments.\n\nDefunct ",
					UI.FormatAsLink("Biobots", "STORYTRAITMORBROVER"),
					" drop harvestable ",
					UI.FormatAsLink("Steel", "STEEL"),
					"."
				});
			}

			// Token: 0x02002B52 RID: 11090
			public class FOSSILDIG
			{
				// Token: 0x0400BE88 RID: 48776
				public static LocString NAME = "Ancient Specimen";

				// Token: 0x0400BE89 RID: 48777
				public static LocString DESC = "It's not from around here.";

				// Token: 0x0400BE8A RID: 48778
				public static LocString EFFECT = "Contains a partial " + UI.FormatAsLink("Fossil", "FOSSIL") + " left behind by a giant critter.\n\nStudying the full skeleton could yield the information required to access a valuable new resource.";
			}

			// Token: 0x02002B53 RID: 11091
			public class FOSSILDIG_COMPLETED
			{
				// Token: 0x0400BE8B RID: 48779
				public static LocString NAME = UI.FormatAsLink("Fossil Quarry", "STORYTRAITFOSSILHUNT");

				// Token: 0x0400BE8C RID: 48780
				public static LocString DESC = "There sure are a lot of old bones in this area.";

				// Token: 0x0400BE8D RID: 48781
				public static LocString EFFECT = "Contains a deep cache of harvestable " + UI.FormatAsLink("Fossils", "FOSSIL") + ".";
			}

			// Token: 0x02002B54 RID: 11092
			public class GAMMARAYOVEN
			{
				// Token: 0x0400BE8E RID: 48782
				public static LocString NAME = UI.FormatAsLink("Gamma Ray Oven", "GAMMARAYOVEN");

				// Token: 0x0400BE8F RID: 48783
				public static LocString DESC = "Nuke your food.";

				// Token: 0x0400BE90 RID: 48784
				public static LocString EFFECT = "Cooks a variety of " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002B55 RID: 11093
			public class GASCARGOBAY
			{
				// Token: 0x0400BE91 RID: 48785
				public static LocString NAME = UI.FormatAsLink("Gas Cargo Canister", "GASCARGOBAY");

				// Token: 0x0400BE92 RID: 48786
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x0400BE93 RID: 48787
				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x02002B56 RID: 11094
			public class GASCARGOBAYCLUSTER
			{
				// Token: 0x0400BE94 RID: 48788
				public static LocString NAME = UI.FormatAsLink("Large Gas Cargo Canister", "GASCARGOBAYCLUSTER");

				// Token: 0x0400BE95 RID: 48789
				public static LocString DESC = "Holds more than a typical gas cargo canister.";

				// Token: 0x0400BE96 RID: 48790
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002B57 RID: 11095
			public class GASCARGOBAYSMALL
			{
				// Token: 0x0400BE97 RID: 48791
				public static LocString NAME = UI.FormatAsLink("Gas Cargo Canister", "GASCARGOBAYSMALL");

				// Token: 0x0400BE98 RID: 48792
				public static LocString DESC = "Duplicants fill cargo canisters with any resources they find during space missions.";

				// Token: 0x0400BE99 RID: 48793
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002B58 RID: 11096
			public class GASCONDUIT
			{
				// Token: 0x0400BE9A RID: 48794
				public static LocString NAME = UI.FormatAsLink("Gas Pipe", "GASCONDUIT");

				// Token: 0x0400BE9B RID: 48795
				public static LocString DESC = "Gas pipes are used to connect the inputs and outputs of ventilated buildings.";

				// Token: 0x0400BE9C RID: 48796
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" between ",
					UI.FormatAsLink("Outputs", "GASPIPING"),
					" and ",
					UI.FormatAsLink("Intakes", "GASPIPING"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002B59 RID: 11097
			public class GASCONDUITBRIDGE
			{
				// Token: 0x0400BE9D RID: 48797
				public static LocString NAME = UI.FormatAsLink("Gas Bridge", "GASCONDUITBRIDGE");

				// Token: 0x0400BE9E RID: 48798
				public static LocString DESC = "Separate pipe systems prevent mingled contents from causing building damage.";

				// Token: 0x0400BE9F RID: 48799
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Gas Pipe", "GASPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002B5A RID: 11098
			public class GASCONDUITPREFERENTIALFLOW
			{
				// Token: 0x0400BEA0 RID: 48800
				public static LocString NAME = UI.FormatAsLink("Priority Gas Flow", "GASCONDUITPREFERENTIALFLOW");

				// Token: 0x0400BEA1 RID: 48801
				public static LocString DESC = "Priority flows ensure important buildings are filled first when on a system with other buildings.";

				// Token: 0x0400BEA2 RID: 48802
				public static LocString EFFECT = "Diverts " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " to a secondary input when its primary input overflows.";
			}

			// Token: 0x02002B5B RID: 11099
			public class LIQUIDCONDUITPREFERENTIALFLOW
			{
				// Token: 0x0400BEA3 RID: 48803
				public static LocString NAME = UI.FormatAsLink("Priority Liquid Flow", "LIQUIDCONDUITPREFERENTIALFLOW");

				// Token: 0x0400BEA4 RID: 48804
				public static LocString DESC = "Priority flows ensure important buildings are filled first when on a system with other buildings.";

				// Token: 0x0400BEA5 RID: 48805
				public static LocString EFFECT = "Diverts " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " to a secondary input when its primary input overflows.";
			}

			// Token: 0x02002B5C RID: 11100
			public class GASCONDUITOVERFLOW
			{
				// Token: 0x0400BEA6 RID: 48806
				public static LocString NAME = UI.FormatAsLink("Gas Overflow Valve", "GASCONDUITOVERFLOW");

				// Token: 0x0400BEA7 RID: 48807
				public static LocString DESC = "Overflow valves can be used to prioritize which buildings should receive precious resources first.";

				// Token: 0x0400BEA8 RID: 48808
				public static LocString EFFECT = "Fills a secondary" + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " output only when its primary output is blocked.";
			}

			// Token: 0x02002B5D RID: 11101
			public class LIQUIDCONDUITOVERFLOW
			{
				// Token: 0x0400BEA9 RID: 48809
				public static LocString NAME = UI.FormatAsLink("Liquid Overflow Valve", "LIQUIDCONDUITOVERFLOW");

				// Token: 0x0400BEAA RID: 48810
				public static LocString DESC = "Overflow valves can be used to prioritize which buildings should receive precious resources first.";

				// Token: 0x0400BEAB RID: 48811
				public static LocString EFFECT = "Fills a secondary" + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " output only when its primary output is blocked.";
			}

			// Token: 0x02002B5E RID: 11102
			public class LAUNCHPAD
			{
				// Token: 0x0400BEAC RID: 48812
				public static LocString NAME = UI.FormatAsLink("Rocket Platform", "LAUNCHPAD");

				// Token: 0x0400BEAD RID: 48813
				public static LocString DESC = "A platform from which rockets can be launched and on which they can land.";

				// Token: 0x0400BEAE RID: 48814
				public static LocString EFFECT = "Precursor to construction of all other Rocket modules.\n\nAllows Rockets to launch from or land on the host Planetoid.\n\nAutomatically links up to " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + UI.FormatAsLink("s", "MODULARLAUNCHPADPORTSOLID") + " built to either side of the platform.";

				// Token: 0x0400BEAF RID: 48815
				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				// Token: 0x0400BEB0 RID: 48816
				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket is ready for flight";

				// Token: 0x0400BEB1 RID: 48817
				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400BEB2 RID: 48818
				public static LocString LOGIC_PORT_LANDED_ROCKET = "Landed Rocket";

				// Token: 0x0400BEB3 RID: 48819
				public static LocString LOGIC_PORT_LANDED_ROCKET_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket is on the " + BUILDINGS.PREFABS.LAUNCHPAD.NAME;

				// Token: 0x0400BEB4 RID: 48820
				public static LocString LOGIC_PORT_LANDED_ROCKET_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400BEB5 RID: 48821
				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				// Token: 0x0400BEB6 RID: 48822
				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				// Token: 0x0400BEB7 RID: 48823
				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Cancel launch";
			}

			// Token: 0x02002B5F RID: 11103
			public class GASFILTER
			{
				// Token: 0x0400BEB8 RID: 48824
				public static LocString NAME = UI.FormatAsLink("Gas Filter", "GASFILTER");

				// Token: 0x0400BEB9 RID: 48825
				public static LocString DESC = "All gases are sent into the building's output pipe, except the gas chosen for filtering.";

				// Token: 0x0400BEBA RID: 48826
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sieves one ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from the air, sending it into a dedicated ",
					UI.FormatAsLink("Pipe", "GASPIPING"),
					"."
				});

				// Token: 0x0400BEBB RID: 48827
				public static LocString STATUS_ITEM = "Filters: {0}";

				// Token: 0x0400BEBC RID: 48828
				public static LocString ELEMENT_NOT_SPECIFIED = "Not Specified";
			}

			// Token: 0x02002B60 RID: 11104
			public class SOLIDFILTER
			{
				// Token: 0x0400BEBD RID: 48829
				public static LocString NAME = UI.FormatAsLink("Solid Filter", "SOLIDFILTER");

				// Token: 0x0400BEBE RID: 48830
				public static LocString DESC = "All solids are sent into the building's output conveyor, except the solid chosen for filtering.";

				// Token: 0x0400BEBF RID: 48831
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Separates one ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" from the conveyor, sending it into a dedicated ",
					BUILDINGS.PREFABS.SOLIDCONDUIT.NAME,
					"."
				});

				// Token: 0x0400BEC0 RID: 48832
				public static LocString STATUS_ITEM = "Filters: {0}";

				// Token: 0x0400BEC1 RID: 48833
				public static LocString ELEMENT_NOT_SPECIFIED = "Not Specified";
			}

			// Token: 0x02002B61 RID: 11105
			public class GASPERMEABLEMEMBRANE
			{
				// Token: 0x0400BEC2 RID: 48834
				public static LocString NAME = UI.FormatAsLink("Airflow Tile", "GASPERMEABLEMEMBRANE");

				// Token: 0x0400BEC3 RID: 48835
				public static LocString DESC = "Building with airflow tile promotes better gas circulation within a colony.";

				// Token: 0x0400BEC4 RID: 48836
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nBlocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow without obstructing ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002B62 RID: 11106
			public class DEVPUMPGAS
			{
				// Token: 0x0400BEC5 RID: 48837
				public static LocString NAME = "Dev Pump Gas";

				// Token: 0x0400BEC6 RID: 48838
				public static LocString DESC = "Piping a pump's output to a building's intake will send gas to that building.";

				// Token: 0x0400BEC7 RID: 48839
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002B63 RID: 11107
			public class GASPUMP
			{
				// Token: 0x0400BEC8 RID: 48840
				public static LocString NAME = UI.FormatAsLink("Gas Pump", "GASPUMP");

				// Token: 0x0400BEC9 RID: 48841
				public static LocString DESC = "Piping a pump's output to a building's intake will send gas to that building.";

				// Token: 0x0400BECA RID: 48842
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002B64 RID: 11108
			public class GASMINIPUMP
			{
				// Token: 0x0400BECB RID: 48843
				public static LocString NAME = UI.FormatAsLink("Mini Gas Pump", "GASMINIPUMP");

				// Token: 0x0400BECC RID: 48844
				public static LocString DESC = "Mini pumps are useful for moving small quantities of gas with minimum power.";

				// Token: 0x0400BECD RID: 48845
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in a small amount of ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002B65 RID: 11109
			public class GASVALVE
			{
				// Token: 0x0400BECE RID: 48846
				public static LocString NAME = UI.FormatAsLink("Gas Valve", "GASVALVE");

				// Token: 0x0400BECF RID: 48847
				public static LocString DESC = "Valves control the amount of gas that moves through pipes, preventing waste.";

				// Token: 0x0400BED0 RID: 48848
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Controls the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" volume permitted through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					"."
				});
			}

			// Token: 0x02002B66 RID: 11110
			public class GASLOGICVALVE
			{
				// Token: 0x0400BED1 RID: 48849
				public static LocString NAME = UI.FormatAsLink("Gas Shutoff", "GASLOGICVALVE");

				// Token: 0x0400BED2 RID: 48850
				public static LocString DESC = "Automated piping saves power and time by removing the need for Duplicant input.";

				// Token: 0x0400BED3 RID: 48851
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow on or off."
				});

				// Token: 0x0400BED4 RID: 48852
				public static LocString LOGIC_PORT = "Open/Close";

				// Token: 0x0400BED5 RID: 48853
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow gas flow";

				// Token: 0x0400BED6 RID: 48854
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent gas flow";
			}

			// Token: 0x02002B67 RID: 11111
			public class GASLIMITVALVE
			{
				// Token: 0x0400BED7 RID: 48855
				public static LocString NAME = UI.FormatAsLink("Gas Meter Valve", "GASLIMITVALVE");

				// Token: 0x0400BED8 RID: 48856
				public static LocString DESC = "Meter Valves let an exact amount of gas pass through before shutting off.";

				// Token: 0x0400BED9 RID: 48857
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow off when the specified amount has passed through it."
				});

				// Token: 0x0400BEDA RID: 48858
				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				// Token: 0x0400BEDB RID: 48859
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				// Token: 0x0400BEDC RID: 48860
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400BEDD RID: 48861
				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				// Token: 0x0400BEDE RID: 48862
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				// Token: 0x0400BEDF RID: 48863
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002B68 RID: 11112
			public class GASVENT
			{
				// Token: 0x0400BEE0 RID: 48864
				public static LocString NAME = UI.FormatAsLink("Gas Vent", "GASVENT");

				// Token: 0x0400BEE1 RID: 48865
				public static LocString DESC = "Vents are an exit point for gases from ventilation systems.";

				// Token: 0x0400BEE2 RID: 48866
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from ",
					UI.FormatAsLink("Gas Pipes", "GASPIPING"),
					"."
				});
			}

			// Token: 0x02002B69 RID: 11113
			public class GASVENTHIGHPRESSURE
			{
				// Token: 0x0400BEE3 RID: 48867
				public static LocString NAME = UI.FormatAsLink("High Pressure Gas Vent", "GASVENTHIGHPRESSURE");

				// Token: 0x0400BEE4 RID: 48868
				public static LocString DESC = "High pressure vents can expel gas into more highly pressurized environments.";

				// Token: 0x0400BEE5 RID: 48869
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from ",
					UI.FormatAsLink("Gas Pipes", "GASPIPING"),
					" into high pressure locations."
				});
			}

			// Token: 0x02002B6A RID: 11114
			public class GASBOTTLER
			{
				// Token: 0x0400BEE6 RID: 48870
				public static LocString NAME = UI.FormatAsLink("Canister Filler", "GASBOTTLER");

				// Token: 0x0400BEE7 RID: 48871
				public static LocString DESC = "Canisters allow Duplicants to manually deliver gases from place to place.";

				// Token: 0x0400BEE8 RID: 48872
				public static LocString EFFECT = "Automatically stores piped " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + " into canisters for manual transport.";
			}

			// Token: 0x02002B6B RID: 11115
			public class LIQUIDBOTTLER
			{
				// Token: 0x0400BEE9 RID: 48873
				public static LocString NAME = UI.FormatAsLink("Bottle Filler", "LIQUIDBOTTLER");

				// Token: 0x0400BEEA RID: 48874
				public static LocString DESC = "Bottle fillers allow Duplicants to manually deliver liquids from place to place.";

				// Token: 0x0400BEEB RID: 48875
				public static LocString EFFECT = "Automatically stores piped " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " into bottles for manual transport.";
			}

			// Token: 0x02002B6C RID: 11116
			public class GENERATOR
			{
				// Token: 0x0400BEEC RID: 48876
				public static LocString NAME = UI.FormatAsLink("Coal Generator", "GENERATOR");

				// Token: 0x0400BEED RID: 48877
				public static LocString DESC = "Burning coal produces more energy than manual power, but emits heat and exhaust.";

				// Token: 0x0400BEEE RID: 48878
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Coal", "CARBON"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					"."
				});

				// Token: 0x0400BEEF RID: 48879
				public static LocString OVERPRODUCTION = "{Generator} overproduction";
			}

			// Token: 0x02002B6D RID: 11117
			public class GENETICANALYSISSTATION
			{
				// Token: 0x0400BEF0 RID: 48880
				public static LocString NAME = UI.FormatAsLink("Botanical Analyzer", "GENETICANALYSISSTATION");

				// Token: 0x0400BEF1 RID: 48881
				public static LocString DESC = "Would a mutated rose still smell as sweet?";

				// Token: 0x0400BEF2 RID: 48882
				public static LocString EFFECT = "Identifies new " + UI.FormatAsLink("Seed", "PLANTS") + " subspecies.";
			}

			// Token: 0x02002B6E RID: 11118
			public class DEVGENERATOR
			{
				// Token: 0x0400BEF3 RID: 48883
				public static LocString NAME = "Dev Generator";

				// Token: 0x0400BEF4 RID: 48884
				public static LocString DESC = "Runs on coffee.";

				// Token: 0x0400BEF5 RID: 48885
				public static LocString EFFECT = "Generates testing power for late nights.";
			}

			// Token: 0x02002B6F RID: 11119
			public class DEVLIFESUPPORT
			{
				// Token: 0x0400BEF6 RID: 48886
				public static LocString NAME = "Dev Life Support";

				// Token: 0x0400BEF7 RID: 48887
				public static LocString DESC = "Keeps Duplicants cozy and breathing.";

				// Token: 0x0400BEF8 RID: 48888
				public static LocString EFFECT = "Generates warm, oxygen-rich air.";
			}

			// Token: 0x02002B70 RID: 11120
			public class DEVLIGHTGENERATOR
			{
				// Token: 0x0400BEF9 RID: 48889
				public static LocString NAME = "Dev Light Source";

				// Token: 0x0400BEFA RID: 48890
				public static LocString DESC = "Brightens up a dev's darkest hours.";

				// Token: 0x0400BEFB RID: 48891
				public static LocString EFFECT = "Generates dimmable light on demand.";

				// Token: 0x0400BEFC RID: 48892
				public static LocString FALLOFF_LABEL = "Falloff Rate";

				// Token: 0x0400BEFD RID: 48893
				public static LocString BRIGHTNESS_LABEL = "Brightness";

				// Token: 0x0400BEFE RID: 48894
				public static LocString RANGE_LABEL = "Range";
			}

			// Token: 0x02002B71 RID: 11121
			public class DEVRADIATIONGENERATOR
			{
				// Token: 0x0400BEFF RID: 48895
				public static LocString NAME = "Dev Radiation Emitter";

				// Token: 0x0400BF00 RID: 48896
				public static LocString DESC = "That's some <i>strong</i> coffee.";

				// Token: 0x0400BF01 RID: 48897
				public static LocString EFFECT = "Generates on-demand radiation to keep things clear. <i>Nu-</i>clear.";
			}

			// Token: 0x02002B72 RID: 11122
			public class DEVHEATER
			{
				// Token: 0x0400BF02 RID: 48898
				public static LocString NAME = "Dev Heater";

				// Token: 0x0400BF03 RID: 48899
				public static LocString DESC = "Did someone touch the thermostat?";

				// Token: 0x0400BF04 RID: 48900
				public static LocString EFFECT = "Generates on-demand heat for testing toastiness.";
			}

			// Token: 0x02002B73 RID: 11123
			public class GENERICFABRICATOR
			{
				// Token: 0x0400BF05 RID: 48901
				public static LocString NAME = UI.FormatAsLink("Omniprinter", "GENERICFABRICATOR");

				// Token: 0x0400BF06 RID: 48902
				public static LocString DESC = "Omniprinters are incapable of printing organic matter.";

				// Token: 0x0400BF07 RID: 48903
				public static LocString EFFECT = "Converts " + UI.FormatAsLink("Raw Mineral", "RAWMINERAL") + " into unique materials and objects.";
			}

			// Token: 0x02002B74 RID: 11124
			public class GEOTUNER
			{
				// Token: 0x0400BF08 RID: 48904
				public static LocString NAME = UI.FormatAsLink("Geotuner", "GEOTUNER");

				// Token: 0x0400BF09 RID: 48905
				public static LocString DESC = "The targeted geyser receives stored amplification data when it is erupting.";

				// Token: 0x0400BF0A RID: 48906
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases the ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" and output of an analyzed ",
					UI.FormatAsLink("Geyser", "GEYSERS"),
					".\n\nMultiple Geotuners can be directed at a single ",
					UI.FormatAsLink("Geyser", "GEYSERS"),
					" anywhere on an asteroid."
				});

				// Token: 0x0400BF0B RID: 48907
				public static LocString LOGIC_PORT = "Geyser Eruption Monitor";

				// Token: 0x0400BF0C RID: 48908
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when geyser is erupting";

				// Token: 0x0400BF0D RID: 48909
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002B75 RID: 11125
			public class GRAVE
			{
				// Token: 0x0400BF0E RID: 48910
				public static LocString NAME = UI.FormatAsLink("Tasteful Memorial", "GRAVE");

				// Token: 0x0400BF0F RID: 48911
				public static LocString DESC = "Burying dead Duplicants reduces health hazards and stress on the colony.";

				// Token: 0x0400BF10 RID: 48912
				public static LocString EFFECT = "Provides a final resting place for deceased Duplicants.\n\nLiving Duplicants will automatically place an unburied corpse inside.";
			}

			// Token: 0x02002B76 RID: 11126
			public class HEADQUARTERS
			{
				// Token: 0x0400BF11 RID: 48913
				public static LocString NAME = UI.FormatAsLink("Printing Pod", "HEADQUARTERS");

				// Token: 0x0400BF12 RID: 48914
				public static LocString DESC = "New Duplicants come out here, but thank goodness, they never go back in.";

				// Token: 0x0400BF13 RID: 48915
				public static LocString EFFECT = "An exceptionally advanced bioprinter of unknown origin.\n\nIt periodically produces new Duplicants or care packages containing resources.";
			}

			// Token: 0x02002B77 RID: 11127
			public class HYDROGENGENERATOR
			{
				// Token: 0x0400BF14 RID: 48916
				public static LocString NAME = UI.FormatAsLink("Hydrogen Generator", "HYDROGENGENERATOR");

				// Token: 0x0400BF15 RID: 48917
				public static LocString DESC = "Hydrogen generators are extremely efficient, emitting next to no waste.";

				// Token: 0x0400BF16 RID: 48918
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					"."
				});
			}

			// Token: 0x02002B78 RID: 11128
			public class METHANEGENERATOR
			{
				// Token: 0x0400BF17 RID: 48919
				public static LocString NAME = UI.FormatAsLink("Natural Gas Generator", "METHANEGENERATOR");

				// Token: 0x0400BF18 RID: 48920
				public static LocString DESC = "Natural gas generators leak polluted water and are best built above a waste reservoir.";

				// Token: 0x0400BF19 RID: 48921
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Natural Gas", "METHANE"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"."
				});
			}

			// Token: 0x02002B79 RID: 11129
			public class NUCLEARREACTOR
			{
				// Token: 0x0400BF1A RID: 48922
				public static LocString NAME = UI.FormatAsLink("Research Reactor", "NUCLEARREACTOR");

				// Token: 0x0400BF1B RID: 48923
				public static LocString DESC = "Radbolt generators and reflectors make radiation usable by other buildings.";

				// Token: 0x0400BF1C RID: 48924
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" to produce ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" for Radbolt production.\n\nGenerates a massive amount of ",
					UI.FormatAsLink("Heat", "HEAT"),
					". Overheating will result in an explosive meltdown."
				});

				// Token: 0x0400BF1D RID: 48925
				public static LocString LOGIC_PORT = "Fuel Delivery Control";

				// Token: 0x0400BF1E RID: 48926
				public static LocString INPUT_PORT_ACTIVE = "Fuel Delivery Enabled";

				// Token: 0x0400BF1F RID: 48927
				public static LocString INPUT_PORT_INACTIVE = "Fuel Delivery Disabled";
			}

			// Token: 0x02002B7A RID: 11130
			public class WOODGASGENERATOR
			{
				// Token: 0x0400BF20 RID: 48928
				public static LocString NAME = UI.FormatAsLink("Wood Burner", "WOODGASGENERATOR");

				// Token: 0x0400BF21 RID: 48929
				public static LocString DESC = "Wood burners are small and easy to maintain, but produce a fair amount of heat.";

				// Token: 0x0400BF22 RID: 48930
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Wood", "WOOD"),
					" to produce electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			// Token: 0x02002B7B RID: 11131
			public class PEATGENERATOR
			{
				// Token: 0x0400BF23 RID: 48931
				public static LocString NAME = UI.FormatAsLink("Peat Burner", "PEATGENERATOR");

				// Token: 0x0400BF24 RID: 48932
				public static LocString DESC = "It gives off an aroma that some Duplicants find inexplicably nostalgic.";

				// Token: 0x0400BF25 RID: 48933
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Peat", "PEAT"),
					" to produce electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces a small amount of ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					ELEMENTS.DIRTYWATER.NAME,
					"."
				});
			}

			// Token: 0x02002B7C RID: 11132
			public class FABRICATEDWOODMAKER
			{
				// Token: 0x0400BF26 RID: 48934
				public static LocString NAME = UI.FormatAsLink("Plywood Press", "FABRICATEDWOODMAKER");

				// Token: 0x0400BF27 RID: 48935
				public static LocString DESC = "Flattened plant bits are a useful wood substitute.";

				// Token: 0x0400BF28 RID: 48936
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Combines a Binder liquid and ",
					ITEMS.INDUSTRIAL_PRODUCTS.PLANT_FIBER.NAME,
					" to create ",
					ELEMENTS.FABRICATEDWOOD.NAME,
					"."
				});

				// Token: 0x0400BF29 RID: 48937
				public static LocString RECIPE_DESC = "Combines {0} and {1} to create {2}.";
			}

			// Token: 0x02002B7D RID: 11133
			public class PETROLEUMGENERATOR
			{
				// Token: 0x0400BF2A RID: 48938
				public static LocString NAME = UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR");

				// Token: 0x0400BF2B RID: 48939
				public static LocString DESC = "Petroleum generators have a high energy output but produce a great deal of waste.";

				// Token: 0x0400BF2C RID: 48940
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					", ",
					UI.FormatAsLink("Ethanol", "ETHANOL"),
					" or ",
					UI.FormatAsLink("Biodiesel", "REFINEDLIPID"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"."
				});
			}

			// Token: 0x02002B7E RID: 11134
			public class HYDROPONICFARM
			{
				// Token: 0x0400BF2D RID: 48941
				public static LocString NAME = UI.FormatAsLink("Hydroponic Farm", "HYDROPONICFARM");

				// Token: 0x0400BF2E RID: 48942
				public static LocString DESC = "Hydroponic farms reduce Duplicant traffic by automating irrigating crops.";

				// Token: 0x0400BF2F RID: 48943
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nCan be used as floor tile and rotated before construction.\n\nMust be irrigated through ",
					UI.FormatAsLink("Liquid Piping", "LIQUIDPIPING"),
					"."
				});
			}

			// Token: 0x02002B7F RID: 11135
			public class INSULATEDGASCONDUIT
			{
				// Token: 0x0400BF30 RID: 48944
				public static LocString NAME = UI.FormatAsLink("Insulated Gas Pipe", "INSULATEDGASCONDUIT");

				// Token: 0x0400BF31 RID: 48945
				public static LocString DESC = "Pipe insulation prevents gas contents from significantly changing temperature in transit.";

				// Token: 0x0400BF32 RID: 48946
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" with minimal change in ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002B80 RID: 11136
			public class GASCONDUITRADIANT
			{
				// Token: 0x0400BF33 RID: 48947
				public static LocString NAME = UI.FormatAsLink("Radiant Gas Pipe", "GASCONDUITRADIANT");

				// Token: 0x0400BF34 RID: 48948
				public static LocString DESC = "Radiant pipes pumping cold gas can be run through hot areas to help cool them down.";

				// Token: 0x0400BF35 RID: 48949
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with the surrounding environment.\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002B81 RID: 11137
			public class INSULATEDLIQUIDCONDUIT
			{
				// Token: 0x0400BF36 RID: 48950
				public static LocString NAME = UI.FormatAsLink("Insulated Liquid Pipe", "INSULATEDLIQUIDCONDUIT");

				// Token: 0x0400BF37 RID: 48951
				public static LocString DESC = "Pipe insulation prevents liquid contents from significantly changing temperature in transit.";

				// Token: 0x0400BF38 RID: 48952
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" with minimal change in ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002B82 RID: 11138
			public class LIQUIDCONDUITRADIANT
			{
				// Token: 0x0400BF39 RID: 48953
				public static LocString NAME = UI.FormatAsLink("Radiant Liquid Pipe", "LIQUIDCONDUITRADIANT");

				// Token: 0x0400BF3A RID: 48954
				public static LocString DESC = "Radiant pipes pumping cold liquid can be run through hot areas to help cool them down.";

				// Token: 0x0400BF3B RID: 48955
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with the surrounding environment.\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002B83 RID: 11139
			public class CONTACTCONDUCTIVEPIPEBRIDGE
			{
				// Token: 0x0400BF3C RID: 48956
				public static LocString NAME = UI.FormatAsLink("Conduction Panel", "CONTACTCONDUCTIVEPIPEBRIDGE");

				// Token: 0x0400BF3D RID: 48957
				public static LocString DESC = "It can transfer heat effectively even if no liquid is passing through.";

				// Token: 0x0400BF3E RID: 48958
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with overlapping buildings.\n\nCan function in a vacuum.\n\nCan be run through wall and floor tiles."
				});
			}

			// Token: 0x02002B84 RID: 11140
			public class INSULATEDWIRE
			{
				// Token: 0x0400BF3F RID: 48959
				public static LocString NAME = UI.FormatAsLink("Insulated Wire", "INSULATEDWIRE");

				// Token: 0x0400BF40 RID: 48960
				public static LocString DESC = "This stuff won't go melting if things get heated.";

				// Token: 0x0400BF41 RID: 48961
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects buildings to ",
					UI.FormatAsLink("Power", "POWER"),
					" sources in extreme ",
					UI.FormatAsLink("Heat", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002B85 RID: 11141
			public class INSULATIONTILE
			{
				// Token: 0x0400BF42 RID: 48962
				public static LocString NAME = UI.FormatAsLink("Insulated Tile", "INSULATIONTILE");

				// Token: 0x0400BF43 RID: 48963
				public static LocString DESC = "The low thermal conductivity of insulated tiles slows any heat passing through them.";

				// Token: 0x0400BF44 RID: 48964
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nReduces " + UI.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.";
			}

			// Token: 0x02002B86 RID: 11142
			public class EXTERIORWALL
			{
				// Token: 0x0400BF45 RID: 48965
				public static LocString NAME = UI.FormatAsLink("Drywall", "EXTERIORWALL");

				// Token: 0x0400BF46 RID: 48966
				public static LocString DESC = "Drywall can be used in conjunction with tiles to build airtight rooms on the surface.";

				// Token: 0x0400BF47 RID: 48967
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Prevents ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" loss in space.\n\nBuilds an insulating backwall behind buildings."
				});

				// Token: 0x02003A6B RID: 14955
				public class FACADES
				{
					// Token: 0x02003E2B RID: 15915
					public class DEFAULT_EXTERIORWALL
					{
						// Token: 0x0400F524 RID: 62756
						public static LocString NAME = UI.FormatAsLink("Drywall", "EXTERIORWALL");

						// Token: 0x0400F525 RID: 62757
						public static LocString DESC = "It gets the job done.";
					}

					// Token: 0x02003E2C RID: 15916
					public class BALM_LILY
					{
						// Token: 0x0400F526 RID: 62758
						public static LocString NAME = UI.FormatAsLink("Balm Lily Print", "EXTERIORWALL");

						// Token: 0x0400F527 RID: 62759
						public static LocString DESC = "A mellow floral wallpaper.";
					}

					// Token: 0x02003E2D RID: 15917
					public class CLOUDS
					{
						// Token: 0x0400F528 RID: 62760
						public static LocString NAME = UI.FormatAsLink("Cloud Print", "EXTERIORWALL");

						// Token: 0x0400F529 RID: 62761
						public static LocString DESC = "A soft, fluffy wallpaper.";
					}

					// Token: 0x02003E2E RID: 15918
					public class MUSHBAR
					{
						// Token: 0x0400F52A RID: 62762
						public static LocString NAME = UI.FormatAsLink("Mush Bar Print", "EXTERIORWALL");

						// Token: 0x0400F52B RID: 62763
						public static LocString DESC = "A gag-inducing wallpaper.";
					}

					// Token: 0x02003E2F RID: 15919
					public class PLAID
					{
						// Token: 0x0400F52C RID: 62764
						public static LocString NAME = UI.FormatAsLink("Aqua Plaid Print", "EXTERIORWALL");

						// Token: 0x0400F52D RID: 62765
						public static LocString DESC = "A cozy flannel wallpaper.";
					}

					// Token: 0x02003E30 RID: 15920
					public class RAIN
					{
						// Token: 0x0400F52E RID: 62766
						public static LocString NAME = UI.FormatAsLink("Rainy Print", "EXTERIORWALL");

						// Token: 0x0400F52F RID: 62767
						public static LocString DESC = "A precipitation-themed wallpaper.";
					}

					// Token: 0x02003E31 RID: 15921
					public class AQUATICMOSAIC
					{
						// Token: 0x0400F530 RID: 62768
						public static LocString NAME = UI.FormatAsLink("Aquatic Mosaic", "EXTERIORWALL");

						// Token: 0x0400F531 RID: 62769
						public static LocString DESC = "A multi-hued blue wallpaper.";
					}

					// Token: 0x02003E32 RID: 15922
					public class RAINBOW
					{
						// Token: 0x0400F532 RID: 62770
						public static LocString NAME = UI.FormatAsLink("Rainbow Stripe", "EXTERIORWALL");

						// Token: 0x0400F533 RID: 62771
						public static LocString DESC = "A wallpaper with <i>all</i> the colors.";
					}

					// Token: 0x02003E33 RID: 15923
					public class SNOW
					{
						// Token: 0x0400F534 RID: 62772
						public static LocString NAME = UI.FormatAsLink("Snowflake Print", "EXTERIORWALL");

						// Token: 0x0400F535 RID: 62773
						public static LocString DESC = "A wallpaper as unique as my colony.";
					}

					// Token: 0x02003E34 RID: 15924
					public class SUN
					{
						// Token: 0x0400F536 RID: 62774
						public static LocString NAME = UI.FormatAsLink("Sunshine Print", "EXTERIORWALL");

						// Token: 0x0400F537 RID: 62775
						public static LocString DESC = "A UV-free wallpaper.";
					}

					// Token: 0x02003E35 RID: 15925
					public class COFFEE
					{
						// Token: 0x0400F538 RID: 62776
						public static LocString NAME = UI.FormatAsLink("Cafe Print", "EXTERIORWALL");

						// Token: 0x0400F539 RID: 62777
						public static LocString DESC = "A caffeine-themed wallpaper.";
					}

					// Token: 0x02003E36 RID: 15926
					public class PASTELPOLKA
					{
						// Token: 0x0400F53A RID: 62778
						public static LocString NAME = UI.FormatAsLink("Pastel Polka Print", "EXTERIORWALL");

						// Token: 0x0400F53B RID: 62779
						public static LocString DESC = "A soothing, dotted wallpaper.";
					}

					// Token: 0x02003E37 RID: 15927
					public class PASTELBLUE
					{
						// Token: 0x0400F53C RID: 62780
						public static LocString NAME = UI.FormatAsLink("Pastel Blue", "EXTERIORWALL");

						// Token: 0x0400F53D RID: 62781
						public static LocString DESC = "A soothing blue wallpaper.";
					}

					// Token: 0x02003E38 RID: 15928
					public class PASTELGREEN
					{
						// Token: 0x0400F53E RID: 62782
						public static LocString NAME = UI.FormatAsLink("Pastel Green", "EXTERIORWALL");

						// Token: 0x0400F53F RID: 62783
						public static LocString DESC = "A soothing green wallpaper.";
					}

					// Token: 0x02003E39 RID: 15929
					public class PASTELPINK
					{
						// Token: 0x0400F540 RID: 62784
						public static LocString NAME = UI.FormatAsLink("Pastel Pink", "EXTERIORWALL");

						// Token: 0x0400F541 RID: 62785
						public static LocString DESC = "A soothing pink wallpaper.";
					}

					// Token: 0x02003E3A RID: 15930
					public class PASTELPURPLE
					{
						// Token: 0x0400F542 RID: 62786
						public static LocString NAME = UI.FormatAsLink("Pastel Purple", "EXTERIORWALL");

						// Token: 0x0400F543 RID: 62787
						public static LocString DESC = "A soothing purple wallpaper.";
					}

					// Token: 0x02003E3B RID: 15931
					public class PASTELYELLOW
					{
						// Token: 0x0400F544 RID: 62788
						public static LocString NAME = UI.FormatAsLink("Pastel Yellow", "EXTERIORWALL");

						// Token: 0x0400F545 RID: 62789
						public static LocString DESC = "A soothing yellow wallpaper.";
					}

					// Token: 0x02003E3C RID: 15932
					public class BASIC_WHITE
					{
						// Token: 0x0400F546 RID: 62790
						public static LocString NAME = UI.FormatAsLink("Fresh White", "EXTERIORWALL");

						// Token: 0x0400F547 RID: 62791
						public static LocString DESC = "It's just so fresh and so clean.";
					}

					// Token: 0x02003E3D RID: 15933
					public class DIAGONAL_RED_DEEP_WHITE
					{
						// Token: 0x0400F548 RID: 62792
						public static LocString NAME = UI.FormatAsLink("Magma Diagonal", "EXTERIORWALL");

						// Token: 0x0400F549 RID: 62793
						public static LocString DESC = "A red wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003E3E RID: 15934
					public class DIAGONAL_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400F54A RID: 62794
						public static LocString NAME = UI.FormatAsLink("Bright Diagonal", "EXTERIORWALL");

						// Token: 0x0400F54B RID: 62795
						public static LocString DESC = "An orange wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003E3F RID: 15935
					public class DIAGONAL_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400F54C RID: 62796
						public static LocString NAME = UI.FormatAsLink("Yellowcake Diagonal", "EXTERIORWALL");

						// Token: 0x0400F54D RID: 62797
						public static LocString DESC = "A radiation-free wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003E40 RID: 15936
					public class DIAGONAL_GREEN_KELLY_WHITE
					{
						// Token: 0x0400F54E RID: 62798
						public static LocString NAME = UI.FormatAsLink("Algae Diagonal", "EXTERIORWALL");

						// Token: 0x0400F54F RID: 62799
						public static LocString DESC = "A slippery wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003E41 RID: 15937
					public class DIAGONAL_BLUE_COBALT_WHITE
					{
						// Token: 0x0400F550 RID: 62800
						public static LocString NAME = UI.FormatAsLink("H2O Diagonal", "EXTERIORWALL");

						// Token: 0x0400F551 RID: 62801
						public static LocString DESC = "A damp wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003E42 RID: 15938
					public class DIAGONAL_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400F552 RID: 62802
						public static LocString NAME = UI.FormatAsLink("Petal Diagonal", "EXTERIORWALL");

						// Token: 0x0400F553 RID: 62803
						public static LocString DESC = "A pink wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003E43 RID: 15939
					public class DIAGONAL_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400F554 RID: 62804
						public static LocString NAME = UI.FormatAsLink("Charcoal Diagonal", "EXTERIORWALL");

						// Token: 0x0400F555 RID: 62805
						public static LocString DESC = "A sleek wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003E44 RID: 15940
					public class CIRCLE_RED_DEEP_WHITE
					{
						// Token: 0x0400F556 RID: 62806
						public static LocString NAME = UI.FormatAsLink("Magma Wedge", "EXTERIORWALL");

						// Token: 0x0400F557 RID: 62807
						public static LocString DESC = "It can be arranged into giant red polka dots.";
					}

					// Token: 0x02003E45 RID: 15941
					public class CIRCLE_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400F558 RID: 62808
						public static LocString NAME = UI.FormatAsLink("Bright Wedge", "EXTERIORWALL");

						// Token: 0x0400F559 RID: 62809
						public static LocString DESC = "It can be arranged into giant orange polka dots.";
					}

					// Token: 0x02003E46 RID: 15942
					public class CIRCLE_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400F55A RID: 62810
						public static LocString NAME = UI.FormatAsLink("Yellowcake Wedge", "EXTERIORWALL");

						// Token: 0x0400F55B RID: 62811
						public static LocString DESC = "A radiation-free pattern that can be arranged into giant polka dots.";
					}

					// Token: 0x02003E47 RID: 15943
					public class CIRCLE_GREEN_KELLY_WHITE
					{
						// Token: 0x0400F55C RID: 62812
						public static LocString NAME = UI.FormatAsLink("Algae Wedge", "EXTERIORWALL");

						// Token: 0x0400F55D RID: 62813
						public static LocString DESC = "It can be arranged into giant green polka dots.";
					}

					// Token: 0x02003E48 RID: 15944
					public class CIRCLE_BLUE_COBALT_WHITE
					{
						// Token: 0x0400F55E RID: 62814
						public static LocString NAME = UI.FormatAsLink("H2O Wedge", "EXTERIORWALL");

						// Token: 0x0400F55F RID: 62815
						public static LocString DESC = "It can be arranged into giant blue polka dots.";
					}

					// Token: 0x02003E49 RID: 15945
					public class CIRCLE_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400F560 RID: 62816
						public static LocString NAME = UI.FormatAsLink("Petal Wedge", "EXTERIORWALL");

						// Token: 0x0400F561 RID: 62817
						public static LocString DESC = "It can be arranged into giant pink polka dots.";
					}

					// Token: 0x02003E4A RID: 15946
					public class CIRCLE_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400F562 RID: 62818
						public static LocString NAME = UI.FormatAsLink("Charcoal Wedge", "EXTERIORWALL");

						// Token: 0x0400F563 RID: 62819
						public static LocString DESC = "It can be arranged into giant shadowy polka dots.";
					}

					// Token: 0x02003E4B RID: 15947
					public class BASIC_BLUE_COBALT
					{
						// Token: 0x0400F564 RID: 62820
						public static LocString NAME = UI.FormatAsLink("Solid Cobalt", "EXTERIORWALL");

						// Token: 0x0400F565 RID: 62821
						public static LocString DESC = "It doesn't cure the blues, so much as emphasize them.";
					}

					// Token: 0x02003E4C RID: 15948
					public class BASIC_GREEN_KELLY
					{
						// Token: 0x0400F566 RID: 62822
						public static LocString NAME = UI.FormatAsLink("Spring Green", "EXTERIORWALL");

						// Token: 0x0400F567 RID: 62823
						public static LocString DESC = "It's cheaper than having a garden.";
					}

					// Token: 0x02003E4D RID: 15949
					public class BASIC_GREY_CHARCOAL
					{
						// Token: 0x0400F568 RID: 62824
						public static LocString NAME = UI.FormatAsLink("Solid Charcoal", "EXTERIORWALL");

						// Token: 0x0400F569 RID: 62825
						public static LocString DESC = "An elevated take on \"gray\".";
					}

					// Token: 0x02003E4E RID: 15950
					public class BASIC_ORANGE_SATSUMA
					{
						// Token: 0x0400F56A RID: 62826
						public static LocString NAME = UI.FormatAsLink("Solid Satsuma", "EXTERIORWALL");

						// Token: 0x0400F56B RID: 62827
						public static LocString DESC = "Less fruit-forward, but just as fresh.";
					}

					// Token: 0x02003E4F RID: 15951
					public class BASIC_PINK_FLAMINGO
					{
						// Token: 0x0400F56C RID: 62828
						public static LocString NAME = UI.FormatAsLink("Solid Pink", "EXTERIORWALL");

						// Token: 0x0400F56D RID: 62829
						public static LocString DESC = "A bold statement, for bold Duplicants.";
					}

					// Token: 0x02003E50 RID: 15952
					public class BASIC_RED_DEEP
					{
						// Token: 0x0400F56E RID: 62830
						public static LocString NAME = UI.FormatAsLink("Chili Red", "EXTERIORWALL");

						// Token: 0x0400F56F RID: 62831
						public static LocString DESC = "It really spices up dull walls.";
					}

					// Token: 0x02003E51 RID: 15953
					public class BASIC_YELLOW_LEMON
					{
						// Token: 0x0400F570 RID: 62832
						public static LocString NAME = UI.FormatAsLink("Canary Yellow", "EXTERIORWALL");

						// Token: 0x0400F571 RID: 62833
						public static LocString DESC = "The original coal-mine chic.";
					}

					// Token: 0x02003E52 RID: 15954
					public class BLUEBERRIES
					{
						// Token: 0x0400F572 RID: 62834
						public static LocString NAME = UI.FormatAsLink("Juicy Blueberry", "EXTERIORWALL");

						// Token: 0x0400F573 RID: 62835
						public static LocString DESC = "It stains the fingers.";
					}

					// Token: 0x02003E53 RID: 15955
					public class GRAPES
					{
						// Token: 0x0400F574 RID: 62836
						public static LocString NAME = UI.FormatAsLink("Grape Escape", "EXTERIORWALL");

						// Token: 0x0400F575 RID: 62837
						public static LocString DESC = "It's seedless, if that matters.";
					}

					// Token: 0x02003E54 RID: 15956
					public class LEMON
					{
						// Token: 0x0400F576 RID: 62838
						public static LocString NAME = UI.FormatAsLink("Sour Lemon", "EXTERIORWALL");

						// Token: 0x0400F577 RID: 62839
						public static LocString DESC = "A bitter yet refreshing style.";
					}

					// Token: 0x02003E55 RID: 15957
					public class LIME
					{
						// Token: 0x0400F578 RID: 62840
						public static LocString NAME = UI.FormatAsLink("Juicy Lime", "EXTERIORWALL");

						// Token: 0x0400F579 RID: 62841
						public static LocString DESC = "Contains no actual vitamin C.";
					}

					// Token: 0x02003E56 RID: 15958
					public class SATSUMA
					{
						// Token: 0x0400F57A RID: 62842
						public static LocString NAME = UI.FormatAsLink("Satsuma Slice", "EXTERIORWALL");

						// Token: 0x0400F57B RID: 62843
						public static LocString DESC = "Adds some much-needed zest to the room.";
					}

					// Token: 0x02003E57 RID: 15959
					public class STRAWBERRY
					{
						// Token: 0x0400F57C RID: 62844
						public static LocString NAME = UI.FormatAsLink("Strawberry Speckle", "EXTERIORWALL");

						// Token: 0x0400F57D RID: 62845
						public static LocString DESC = "Fruity freckles for naturally sweet spaces.";
					}

					// Token: 0x02003E58 RID: 15960
					public class WATERMELON
					{
						// Token: 0x0400F57E RID: 62846
						public static LocString NAME = UI.FormatAsLink("Juicy Watermelon", "EXTERIORWALL");

						// Token: 0x0400F57F RID: 62847
						public static LocString DESC = "Far more practical than gluing real fruit on a wall.";
					}

					// Token: 0x02003E59 RID: 15961
					public class TROPICAL
					{
						// Token: 0x0400F580 RID: 62848
						public static LocString NAME = UI.FormatAsLink("Sporechid Print", "EXTERIORWALL");

						// Token: 0x0400F581 RID: 62849
						public static LocString DESC = "The original scratch-and-sniff version was immediately recalled.";
					}

					// Token: 0x02003E5A RID: 15962
					public class TOILETPAPER
					{
						// Token: 0x0400F582 RID: 62850
						public static LocString NAME = UI.FormatAsLink("De-loo-xe", "EXTERIORWALL");

						// Token: 0x0400F583 RID: 62851
						public static LocString DESC = "Softly undulating lines create an undeniable air of loo-xury.";
					}

					// Token: 0x02003E5B RID: 15963
					public class PLUNGER
					{
						// Token: 0x0400F584 RID: 62852
						public static LocString NAME = UI.FormatAsLink("Plunger Print", "EXTERIORWALL");

						// Token: 0x0400F585 RID: 62853
						public static LocString DESC = "Unclogs one's creative impulses.";
					}

					// Token: 0x02003E5C RID: 15964
					public class STRIPES_BLUE
					{
						// Token: 0x0400F586 RID: 62854
						public static LocString NAME = UI.FormatAsLink("Blue Awning Stripe", "EXTERIORWALL");

						// Token: 0x0400F587 RID: 62855
						public static LocString DESC = "Thick stripes in alternating shades of blue.";
					}

					// Token: 0x02003E5D RID: 15965
					public class STRIPES_DIAGONAL_BLUE
					{
						// Token: 0x0400F588 RID: 62856
						public static LocString NAME = UI.FormatAsLink("Blue Regimental Stripe", "EXTERIORWALL");

						// Token: 0x0400F589 RID: 62857
						public static LocString DESC = "Inspired by the ties worn during intraoffice sports.";
					}

					// Token: 0x02003E5E RID: 15966
					public class STRIPES_CIRCLE_BLUE
					{
						// Token: 0x0400F58A RID: 62858
						public static LocString NAME = UI.FormatAsLink("Blue Circle Stripe", "EXTERIORWALL");

						// Token: 0x0400F58B RID: 62859
						public static LocString DESC = "A stripe that curves to the right.";
					}

					// Token: 0x02003E5F RID: 15967
					public class SQUARES_RED_DEEP_WHITE
					{
						// Token: 0x0400F58C RID: 62860
						public static LocString NAME = UI.FormatAsLink("Magma Checkers", "EXTERIORWALL");

						// Token: 0x0400F58D RID: 62861
						public static LocString DESC = "They're so hot right now!";
					}

					// Token: 0x02003E60 RID: 15968
					public class SQUARES_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400F58E RID: 62862
						public static LocString NAME = UI.FormatAsLink("Bright Checkers", "EXTERIORWALL");

						// Token: 0x0400F58F RID: 62863
						public static LocString DESC = "Every tile feels like four tiles!";
					}

					// Token: 0x02003E61 RID: 15969
					public class SQUARES_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400F590 RID: 62864
						public static LocString NAME = UI.FormatAsLink("Yellowcake Checkers", "EXTERIORWALL");

						// Token: 0x0400F591 RID: 62865
						public static LocString DESC = "Any brighter, and they'd be radioactive!";
					}

					// Token: 0x02003E62 RID: 15970
					public class SQUARES_GREEN_KELLY_WHITE
					{
						// Token: 0x0400F592 RID: 62866
						public static LocString NAME = UI.FormatAsLink("Algae Checkers", "EXTERIORWALL");

						// Token: 0x0400F593 RID: 62867
						public static LocString DESC = "Now with real simulated algae color!";
					}

					// Token: 0x02003E63 RID: 15971
					public class SQUARES_BLUE_COBALT_WHITE
					{
						// Token: 0x0400F594 RID: 62868
						public static LocString NAME = UI.FormatAsLink("H2O Checkers", "EXTERIORWALL");

						// Token: 0x0400F595 RID: 62869
						public static LocString DESC = "Drink it all in!";
					}

					// Token: 0x02003E64 RID: 15972
					public class SQUARES_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400F596 RID: 62870
						public static LocString NAME = UI.FormatAsLink("Petal Checkers", "EXTERIORWALL");

						// Token: 0x0400F597 RID: 62871
						public static LocString DESC = "Fiercely fluorescent floral-inspired pink!";
					}

					// Token: 0x02003E65 RID: 15973
					public class SQUARES_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400F598 RID: 62872
						public static LocString NAME = UI.FormatAsLink("Charcoal Checkers", "EXTERIORWALL");

						// Token: 0x0400F599 RID: 62873
						public static LocString DESC = "So retro!";
					}

					// Token: 0x02003E66 RID: 15974
					public class KITCHEN_RETRO1
					{
						// Token: 0x0400F59A RID: 62874
						public static LocString NAME = UI.FormatAsLink("Cafeteria Kitsch", "EXTERIORWALL");

						// Token: 0x0400F59B RID: 62875
						public static LocString DESC = "Some diners find it nostalgic.";
					}

					// Token: 0x02003E67 RID: 15975
					public class PLUS_RED_DEEP_WHITE
					{
						// Token: 0x0400F59C RID: 62876
						public static LocString NAME = UI.FormatAsLink("Digital Chili", "EXTERIORWALL");

						// Token: 0x0400F59D RID: 62877
						public static LocString DESC = "A pixelated red-and-white print.";
					}

					// Token: 0x02003E68 RID: 15976
					public class PLUS_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400F59E RID: 62878
						public static LocString NAME = UI.FormatAsLink("Digital Satsuma", "EXTERIORWALL");

						// Token: 0x0400F59F RID: 62879
						public static LocString DESC = "A pixelated orange-and-white print.";
					}

					// Token: 0x02003E69 RID: 15977
					public class PLUS_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400F5A0 RID: 62880
						public static LocString NAME = UI.FormatAsLink("Digital Lemon", "EXTERIORWALL");

						// Token: 0x0400F5A1 RID: 62881
						public static LocString DESC = "A pixelated yellow-and-white print.";
					}

					// Token: 0x02003E6A RID: 15978
					public class PLUS_GREEN_KELLY_WHITE
					{
						// Token: 0x0400F5A2 RID: 62882
						public static LocString NAME = UI.FormatAsLink("Digital Lawn", "EXTERIORWALL");

						// Token: 0x0400F5A3 RID: 62883
						public static LocString DESC = "A pixelated green-and-white print.";
					}

					// Token: 0x02003E6B RID: 15979
					public class PLUS_BLUE_COBALT_WHITE
					{
						// Token: 0x0400F5A4 RID: 62884
						public static LocString NAME = UI.FormatAsLink("Digital Cobalt", "EXTERIORWALL");

						// Token: 0x0400F5A5 RID: 62885
						public static LocString DESC = "A pixelated blue-and-white print.";
					}

					// Token: 0x02003E6C RID: 15980
					public class PLUS_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400F5A6 RID: 62886
						public static LocString NAME = UI.FormatAsLink("Digital Pink", "EXTERIORWALL");

						// Token: 0x0400F5A7 RID: 62887
						public static LocString DESC = "A pixelated pink-and-white print.";
					}

					// Token: 0x02003E6D RID: 15981
					public class PLUS_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400F5A8 RID: 62888
						public static LocString NAME = UI.FormatAsLink("Digital Charcoal", "EXTERIORWALL");

						// Token: 0x0400F5A9 RID: 62889
						public static LocString DESC = "It's futuristic, so it must be good.";
					}

					// Token: 0x02003E6E RID: 15982
					public class STRIPES_ROSE
					{
						// Token: 0x0400F5AA RID: 62890
						public static LocString NAME = UI.FormatAsLink("Puce Stripe", "EXTERIORWALL");

						// Token: 0x0400F5AB RID: 62891
						public static LocString DESC = "Vertical stripes make it quite obvious when nearby objects are askew.";
					}

					// Token: 0x02003E6F RID: 15983
					public class STRIPES_DIAGONAL_ROSE
					{
						// Token: 0x0400F5AC RID: 62892
						public static LocString NAME = UI.FormatAsLink("Puce Diagonal", "EXTERIORWALL");

						// Token: 0x0400F5AD RID: 62893
						public static LocString DESC = "Some describe this color as \"squashed bug.\"";
					}

					// Token: 0x02003E70 RID: 15984
					public class STRIPES_CIRCLE_ROSE
					{
						// Token: 0x0400F5AE RID: 62894
						public static LocString NAME = UI.FormatAsLink("Puce Curves", "EXTERIORWALL");

						// Token: 0x0400F5AF RID: 62895
						public static LocString DESC = "It's pronounced \"peeyoo-ss,\" a sound that Duplicants just can't seem to reproduce.";
					}

					// Token: 0x02003E71 RID: 15985
					public class STRIPES_MUSH
					{
						// Token: 0x0400F5B0 RID: 62896
						public static LocString NAME = UI.FormatAsLink("Mush Stripe", "EXTERIORWALL");

						// Token: 0x0400F5B1 RID: 62897
						public static LocString DESC = "The kind of green that makes one feel slightly nauseated.";
					}

					// Token: 0x02003E72 RID: 15986
					public class STRIPES_DIAGONAL_MUSH
					{
						// Token: 0x0400F5B2 RID: 62898
						public static LocString NAME = UI.FormatAsLink("Mush Diagonal", "EXTERIORWALL");

						// Token: 0x0400F5B3 RID: 62899
						public static LocString DESC = "Diagonal stripes in alternating shades of mush bar.";
					}

					// Token: 0x02003E73 RID: 15987
					public class STRIPES_CIRCLE_MUSH
					{
						// Token: 0x0400F5B4 RID: 62900
						public static LocString NAME = UI.FormatAsLink("Mush Curves", "EXTERIORWALL");

						// Token: 0x0400F5B5 RID: 62901
						public static LocString DESC = "This wallpaper, like this colony's journey, is full of twists and turns.";
					}

					// Token: 0x02003E74 RID: 15988
					public class STRIPES_YELLOW_TARTAR
					{
						// Token: 0x0400F5B6 RID: 62902
						public static LocString NAME = UI.FormatAsLink("Ick Stripe", "EXTERIORWALL");

						// Token: 0x0400F5B7 RID: 62903
						public static LocString DESC = "Vertical stripes make it quite obvious when nearby objects are askew.";
					}

					// Token: 0x02003E75 RID: 15989
					public class STRIPES_DIAGONAL_YELLOW_TARTAR
					{
						// Token: 0x0400F5B8 RID: 62904
						public static LocString NAME = UI.FormatAsLink("Ick Diagonal", "EXTERIORWALL");

						// Token: 0x0400F5B9 RID: 62905
						public static LocString DESC = "Diagonal stripes in alternating shades of yellow.";
					}

					// Token: 0x02003E76 RID: 15990
					public class STRIPES_CIRCLE_YELLOW_TARTAR
					{
						// Token: 0x0400F5BA RID: 62906
						public static LocString NAME = UI.FormatAsLink("Ick Curves", "EXTERIORWALL");

						// Token: 0x0400F5BB RID: 62907
						public static LocString DESC = "This wallpaper, like this colony's journey, is full of twists and turns.";
					}

					// Token: 0x02003E77 RID: 15991
					public class STRIPES_PURPLE_BRAINFAT
					{
						// Token: 0x0400F5BC RID: 62908
						public static LocString NAME = UI.FormatAsLink("Fainting Stripe", "EXTERIORWALL");

						// Token: 0x0400F5BD RID: 62909
						public static LocString DESC = "Vertical stripes make it quite obvious when nearby objects are askew.";
					}

					// Token: 0x02003E78 RID: 15992
					public class STRIPES_DIAGONAL_PURPLE_BRAINFAT
					{
						// Token: 0x0400F5BE RID: 62910
						public static LocString NAME = UI.FormatAsLink("Fainting Diagonal", "EXTERIORWALL");

						// Token: 0x0400F5BF RID: 62911
						public static LocString DESC = "Diagonal stripes in alternating shades of purple.";
					}

					// Token: 0x02003E79 RID: 15993
					public class STRIPES_CIRCLE_PURPLE_BRAINFAT
					{
						// Token: 0x0400F5C0 RID: 62912
						public static LocString NAME = UI.FormatAsLink("Fainting Curves", "EXTERIORWALL");

						// Token: 0x0400F5C1 RID: 62913
						public static LocString DESC = "This wallpaper, like this colony's journey, is full of twists and turns.";
					}

					// Token: 0x02003E7A RID: 15994
					public class FLOPPY_AZULENE_VITRO
					{
						// Token: 0x0400F5C2 RID: 62914
						public static LocString NAME = UI.FormatAsLink("Waterlogged Databank", "EXTERIORWALL");

						// Token: 0x0400F5C3 RID: 62915
						public static LocString DESC = "A fun blue print in honor of information storage.";
					}

					// Token: 0x02003E7B RID: 15995
					public class FLOPPY_BLACK_WHITE
					{
						// Token: 0x0400F5C4 RID: 62916
						public static LocString NAME = UI.FormatAsLink("Monochrome Databank", "EXTERIORWALL");

						// Token: 0x0400F5C5 RID: 62917
						public static LocString DESC = "A chic black-and-white print in honor of information storage.";
					}

					// Token: 0x02003E7C RID: 15996
					public class FLOPPY_PEAGREEN_BALMY
					{
						// Token: 0x0400F5C6 RID: 62918
						public static LocString NAME = UI.FormatAsLink("Lush Databank", "EXTERIORWALL");

						// Token: 0x0400F5C7 RID: 62919
						public static LocString DESC = "A fun green print in honor of information storage.";
					}

					// Token: 0x02003E7D RID: 15997
					public class FLOPPY_SATSUMA_YELLOWCAKE
					{
						// Token: 0x0400F5C8 RID: 62920
						public static LocString NAME = UI.FormatAsLink("Hi-Vis Databank", "EXTERIORWALL");

						// Token: 0x0400F5C9 RID: 62921
						public static LocString DESC = "A fun orange print in honor of information storage.";
					}

					// Token: 0x02003E7E RID: 15998
					public class FLOPPY_MAGMA_AMINO
					{
						// Token: 0x0400F5CA RID: 62922
						public static LocString NAME = UI.FormatAsLink("Flashy Databank", "EXTERIORWALL");

						// Token: 0x0400F5CB RID: 62923
						public static LocString DESC = "A fun red print in honor of information storage.";
					}

					// Token: 0x02003E7F RID: 15999
					public class ORANGE_JUICE
					{
						// Token: 0x0400F5CC RID: 62924
						public static LocString NAME = UI.FormatAsLink("Infinite Spill", "EXTERIORWALL");

						// Token: 0x0400F5CD RID: 62925
						public static LocString DESC = "If the liquids never hit the floor, is it really a spill?";
					}

					// Token: 0x02003E80 RID: 16000
					public class PAINT_BLOTS
					{
						// Token: 0x0400F5CE RID: 62926
						public static LocString NAME = UI.FormatAsLink("Happy Accidents", "EXTERIORWALL");

						// Token: 0x0400F5CF RID: 62927
						public static LocString DESC = "There are no mistakes, only cheerful little splotches.";
					}

					// Token: 0x02003E81 RID: 16001
					public class TELESCOPE
					{
						// Token: 0x0400F5D0 RID: 62928
						public static LocString NAME = UI.FormatAsLink("Telescope Print", "EXTERIORWALL");

						// Token: 0x0400F5D1 RID: 62929
						public static LocString DESC = "The perfect wallpaper for skygazers.";
					}

					// Token: 0x02003E82 RID: 16002
					public class TICTACTOE_O
					{
						// Token: 0x0400F5D2 RID: 62930
						public static LocString NAME = UI.FormatAsLink("TicTacToe O", "EXTERIORWALL");

						// Token: 0x0400F5D3 RID: 62931
						public static LocString DESC = "A crisp black 'O' on a clean white background. Ideal for monochromatic games rooms.";
					}

					// Token: 0x02003E83 RID: 16003
					public class TICTACTOE_X
					{
						// Token: 0x0400F5D4 RID: 62932
						public static LocString NAME = UI.FormatAsLink("TicTacToe X", "EXTERIORWALL");

						// Token: 0x0400F5D5 RID: 62933
						public static LocString DESC = "A crisp black 'X' on a clean white background. Ideal for monochromatic games rooms.";
					}

					// Token: 0x02003E84 RID: 16004
					public class DICE_1
					{
						// Token: 0x0400F5D6 RID: 62934
						public static LocString NAME = UI.FormatAsLink("Roll One", "EXTERIORWALL");

						// Token: 0x0400F5D7 RID: 62935
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003E85 RID: 16005
					public class DICE_2
					{
						// Token: 0x0400F5D8 RID: 62936
						public static LocString NAME = UI.FormatAsLink("Roll Two", "EXTERIORWALL");

						// Token: 0x0400F5D9 RID: 62937
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003E86 RID: 16006
					public class DICE_3
					{
						// Token: 0x0400F5DA RID: 62938
						public static LocString NAME = UI.FormatAsLink("Roll Three", "EXTERIORWALL");

						// Token: 0x0400F5DB RID: 62939
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003E87 RID: 16007
					public class DICE_4
					{
						// Token: 0x0400F5DC RID: 62940
						public static LocString NAME = UI.FormatAsLink("Roll Four", "EXTERIORWALL");

						// Token: 0x0400F5DD RID: 62941
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003E88 RID: 16008
					public class DICE_5
					{
						// Token: 0x0400F5DE RID: 62942
						public static LocString NAME = UI.FormatAsLink("Roll Five", "EXTERIORWALL");

						// Token: 0x0400F5DF RID: 62943
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003E89 RID: 16009
					public class DICE_6
					{
						// Token: 0x0400F5E0 RID: 62944
						public static LocString NAME = UI.FormatAsLink("High Roller", "EXTERIORWALL");

						// Token: 0x0400F5E1 RID: 62945
						public static LocString DESC = "Inspired by classic dice.";
					}
				}
			}

			// Token: 0x02002B87 RID: 11143
			public class FARMTILE
			{
				// Token: 0x0400BF48 RID: 48968
				public static LocString NAME = UI.FormatAsLink("Farm Tile", "FARMTILE");

				// Token: 0x0400BF49 RID: 48969
				public static LocString DESC = "Duplicants can deliver fertilizer and liquids to farm tiles, accelerating plant growth.";

				// Token: 0x0400BF4A RID: 48970
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nCan be used as floor tile and rotated before construction."
				});
			}

			// Token: 0x02002B88 RID: 11144
			public class LADDER
			{
				// Token: 0x0400BF4B RID: 48971
				public static LocString NAME = UI.FormatAsLink("Ladder", "LADDER");

				// Token: 0x0400BF4C RID: 48972
				public static LocString DESC = "(That means they climb it.)";

				// Token: 0x0400BF4D RID: 48973
				public static LocString EFFECT = "Enables vertical mobility for Duplicants.";
			}

			// Token: 0x02002B89 RID: 11145
			public class LADDERFAST
			{
				// Token: 0x0400BF4E RID: 48974
				public static LocString NAME = UI.FormatAsLink("Plastic Ladder", "LADDERFAST");

				// Token: 0x0400BF4F RID: 48975
				public static LocString DESC = "Plastic ladders are mildly antiseptic and can help limit the spread of germs in a colony.";

				// Token: 0x0400BF50 RID: 48976
				public static LocString EFFECT = "Increases Duplicant climbing speed.";
			}

			// Token: 0x02002B8A RID: 11146
			public class LIQUIDCONDUIT
			{
				// Token: 0x0400BF51 RID: 48977
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

				// Token: 0x0400BF52 RID: 48978
				public static LocString DESC = "Liquid pipes are used to connect the inputs and outputs of plumbed buildings.";

				// Token: 0x0400BF53 RID: 48979
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" between ",
					UI.FormatAsLink("Outputs", "LIQUIDPIPING"),
					" and ",
					UI.FormatAsLink("Intakes", "LIQUIDPIPING"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002B8B RID: 11147
			public class LIQUIDCONDUITBRIDGE
			{
				// Token: 0x0400BF54 RID: 48980
				public static LocString NAME = UI.FormatAsLink("Liquid Bridge", "LIQUIDCONDUITBRIDGE");

				// Token: 0x0400BF55 RID: 48981
				public static LocString DESC = "Separate pipe systems help prevent building damage caused by mingled pipe contents.";

				// Token: 0x0400BF56 RID: 48982
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Liquid Pipe", "LIQUIDPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002B8C RID: 11148
			public class ICECOOLEDFAN
			{
				// Token: 0x0400BF57 RID: 48983
				public static LocString NAME = UI.FormatAsLink("Ice-E Fan", "ICECOOLEDFAN");

				// Token: 0x0400BF58 RID: 48984
				public static LocString DESC = "A Duplicant can work an Ice-E fan to temporarily cool small areas as needed.";

				// Token: 0x0400BF59 RID: 48985
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Ice", "ICEORE"),
					" to dissipate a small amount of the ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			// Token: 0x02002B8D RID: 11149
			public class ICEMACHINE
			{
				// Token: 0x0400BF5A RID: 48986
				public static LocString NAME = UI.FormatAsLink("Ice Maker", "ICEMACHINE");

				// Token: 0x0400BF5B RID: 48987
				public static LocString DESC = "Ice makers can be used as a small renewable source of ice and snow.";

				// Token: 0x0400BF5C RID: 48988
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Water", "WATER"),
					" into ",
					UI.FormatAsLink("Ice", "ICE"),
					" or ",
					UI.FormatAsLink("Snow", "SNOW"),
					"."
				});

				// Token: 0x02003A6C RID: 14956
				public class OPTION_TOOLTIPS
				{
					// Token: 0x0400EBB1 RID: 60337
					public static LocString ICE = "Convert " + UI.FormatAsLink("Water", "WATER") + " into " + UI.FormatAsLink("Ice", "ICE");

					// Token: 0x0400EBB2 RID: 60338
					public static LocString SNOW = "Convert " + UI.FormatAsLink("Water", "WATER") + " into " + UI.FormatAsLink("Snow", "SNOW");
				}
			}

			// Token: 0x02002B8E RID: 11150
			public class LIQUIDCOOLEDFAN
			{
				// Token: 0x0400BF5D RID: 48989
				public static LocString NAME = UI.FormatAsLink("Hydrofan", "LIQUIDCOOLEDFAN");

				// Token: 0x0400BF5E RID: 48990
				public static LocString DESC = "A Duplicant can work a hydrofan to temporarily cool small areas as needed.";

				// Token: 0x0400BF5F RID: 48991
				public static LocString EFFECT = "Dissipates a small amount of the " + UI.FormatAsLink("Heat", "HEAT") + ".";
			}

			// Token: 0x02002B8F RID: 11151
			public class CREATURETRAP
			{
				// Token: 0x0400BF60 RID: 48992
				public static LocString NAME = UI.FormatAsLink("Critter Trap", "CREATURETRAP");

				// Token: 0x0400BF61 RID: 48993
				public static LocString DESC = "Critter traps cannot catch swimming or flying critters.";

				// Token: 0x0400BF62 RID: 48994
				public static LocString EFFECT = "Captures a living " + UI.FormatAsLink("Critter", "CREATURES") + " for transport.\n\nSingle use.";
			}

			// Token: 0x02002B90 RID: 11152
			public class CREATUREGROUNDTRAP
			{
				// Token: 0x0400BF63 RID: 48995
				public static LocString NAME = UI.FormatAsLink("Critter Trap", "CREATUREGROUNDTRAP");

				// Token: 0x0400BF64 RID: 48996
				public static LocString DESC = "It's designed for land critters, but flopping fish sometimes find their way in too.";

				// Token: 0x0400BF65 RID: 48997
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Captures a living ",
					UI.FormatAsLink("Critter", "CREATURES"),
					" for transport.\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			// Token: 0x02002B91 RID: 11153
			public class CREATUREDELIVERYPOINT
			{
				// Token: 0x0400BF66 RID: 48998
				public static LocString NAME = UI.FormatAsLink("Critter Drop-Off", "CREATUREDELIVERYPOINT");

				// Token: 0x0400BF67 RID: 48999
				public static LocString DESC = "Duplicants automatically bring captured critters to these relocation points for release.";

				// Token: 0x0400BF68 RID: 49000
				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Critters", "CREATURES") + " back into the world.\n\nCan be used multiple times.";
			}

			// Token: 0x02002B92 RID: 11154
			public class CRITTERPICKUP
			{
				// Token: 0x0400BF69 RID: 49001
				public static LocString NAME = UI.FormatAsLink("Critter Pick-Up", "CRITTERPICKUP");

				// Token: 0x0400BF6A RID: 49002
				public static LocString DESC = "Duplicants will automatically wrangle excess critters.";

				// Token: 0x0400BF6B RID: 49003
				public static LocString EFFECT = "Ensures the prompt relocation of " + UI.FormatAsLink("Critters", "CREATURES") + " that exceed the maximum amount set.\n\nMonitoring and pick-up are limited to the specified species.";

				// Token: 0x02003A6D RID: 14957
				public class LOGIC_INPUT
				{
					// Token: 0x0400EBB3 RID: 60339
					public static LocString DESC = "Enable/Disable";

					// Token: 0x0400EBB4 RID: 60340
					public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Wrangle excess critters";

					// Token: 0x0400EBB5 RID: 60341
					public static LocString LOGIC_PORT_INACTIVE = BUILDINGS.PREFABS.CRITTERPICKUP.LOGIC_INPUT.LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Ignore excess critters";
				}
			}

			// Token: 0x02002B93 RID: 11155
			public class CRITTERDROPOFF
			{
				// Token: 0x0400BF6C RID: 49004
				public static LocString NAME = UI.FormatAsLink("Critter Drop-Off", "CRITTERDROPOFF");

				// Token: 0x0400BF6D RID: 49005
				public static LocString DESC = "Duplicants automatically bring captured critters to these relocation points for release.";

				// Token: 0x0400BF6E RID: 49006
				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Critters", "CREATURES") + " back into the world.\n\nMonitoring and drop-off are limited to the specified species.";

				// Token: 0x02003A6E RID: 14958
				public class LOGIC_INPUT
				{
					// Token: 0x0400EBB6 RID: 60342
					public static LocString DESC = "Enable/Disable";

					// Token: 0x0400EBB7 RID: 60343
					public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable critter drop-off";

					// Token: 0x0400EBB8 RID: 60344
					public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable critter drop-off";
				}
			}

			// Token: 0x02002B94 RID: 11156
			public class LIQUIDFILTER
			{
				// Token: 0x0400BF6F RID: 49007
				public static LocString NAME = UI.FormatAsLink("Liquid Filter", "LIQUIDFILTER");

				// Token: 0x0400BF70 RID: 49008
				public static LocString DESC = "All liquids are sent into the building's output pipe, except the liquid chosen for filtering.";

				// Token: 0x0400BF71 RID: 49009
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sieves one ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" out of a mix, sending it into a dedicated ",
					UI.FormatAsLink("Filtered Output Pipe", "LIQUIDPIPING"),
					".\n\nCan only filter one liquid type at a time."
				});
			}

			// Token: 0x02002B95 RID: 11157
			public class DEVPUMPLIQUID
			{
				// Token: 0x0400BF72 RID: 49010
				public static LocString NAME = "Dev Pump Liquid";

				// Token: 0x0400BF73 RID: 49011
				public static LocString DESC = "Piping a pump's output to a building's intake will send liquid to that building.";

				// Token: 0x0400BF74 RID: 49012
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002B96 RID: 11158
			public class LIQUIDPUMP
			{
				// Token: 0x0400BF75 RID: 49013
				public static LocString NAME = UI.FormatAsLink("Liquid Pump", "LIQUIDPUMP");

				// Token: 0x0400BF76 RID: 49014
				public static LocString DESC = "Piping a pump's output to a building's intake will send liquid to that building.";

				// Token: 0x0400BF77 RID: 49015
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002B97 RID: 11159
			public class LIQUIDMINIPUMP
			{
				// Token: 0x0400BF78 RID: 49016
				public static LocString NAME = UI.FormatAsLink("Mini Liquid Pump", "LIQUIDMINIPUMP");

				// Token: 0x0400BF79 RID: 49017
				public static LocString DESC = "Mini pumps are useful for moving small quantities of liquid with minimum power.";

				// Token: 0x0400BF7A RID: 49018
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in a small amount of ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002B98 RID: 11160
			public class LIQUIDPUMPINGSTATION
			{
				// Token: 0x0400BF7B RID: 49019
				public static LocString NAME = UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION");

				// Token: 0x0400BF7C RID: 49020
				public static LocString DESC = "Pitcher pumps allow Duplicants to bottle and deliver liquids from place to place.";

				// Token: 0x0400BF7D RID: 49021
				public static LocString EFFECT = "Manually pumps " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " into bottles for transport.\n\nDuplicants can only carry liquids that are bottled.";
			}

			// Token: 0x02002B99 RID: 11161
			public class LIQUIDVALVE
			{
				// Token: 0x0400BF7E RID: 49022
				public static LocString NAME = UI.FormatAsLink("Liquid Valve", "LIQUIDVALVE");

				// Token: 0x0400BF7F RID: 49023
				public static LocString DESC = "Valves control the amount of liquid that moves through pipes, preventing waste.";

				// Token: 0x0400BF80 RID: 49024
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Controls the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" volume permitted through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					"."
				});
			}

			// Token: 0x02002B9A RID: 11162
			public class LIQUIDLOGICVALVE
			{
				// Token: 0x0400BF81 RID: 49025
				public static LocString NAME = UI.FormatAsLink("Liquid Shutoff", "LIQUIDLOGICVALVE");

				// Token: 0x0400BF82 RID: 49026
				public static LocString DESC = "Automated piping saves power and time by removing the need for Duplicant input.";

				// Token: 0x0400BF83 RID: 49027
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow on or off."
				});

				// Token: 0x0400BF84 RID: 49028
				public static LocString LOGIC_PORT = "Open/Close";

				// Token: 0x0400BF85 RID: 49029
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Liquid flow";

				// Token: 0x0400BF86 RID: 49030
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Liquid flow";
			}

			// Token: 0x02002B9B RID: 11163
			public class LIQUIDLIMITVALVE
			{
				// Token: 0x0400BF87 RID: 49031
				public static LocString NAME = UI.FormatAsLink("Liquid Meter Valve", "LIQUIDLIMITVALVE");

				// Token: 0x0400BF88 RID: 49032
				public static LocString DESC = "Meter Valves let an exact amount of liquid pass through before shutting off.";

				// Token: 0x0400BF89 RID: 49033
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow off when the specified amount has passed through it."
				});

				// Token: 0x0400BF8A RID: 49034
				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				// Token: 0x0400BF8B RID: 49035
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				// Token: 0x0400BF8C RID: 49036
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400BF8D RID: 49037
				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				// Token: 0x0400BF8E RID: 49038
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				// Token: 0x0400BF8F RID: 49039
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002B9C RID: 11164
			public class LIQUIDVENT
			{
				// Token: 0x0400BF90 RID: 49040
				public static LocString NAME = UI.FormatAsLink("Liquid Vent", "LIQUIDVENT");

				// Token: 0x0400BF91 RID: 49041
				public static LocString DESC = "Vents are an exit point for liquids from plumbing systems.";

				// Token: 0x0400BF92 RID: 49042
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" from ",
					UI.FormatAsLink("Liquid Pipes", "LIQUIDPIPING"),
					"."
				});
			}

			// Token: 0x02002B9D RID: 11165
			public class MANUALGENERATOR
			{
				// Token: 0x0400BF93 RID: 49043
				public static LocString NAME = UI.FormatAsLink("Manual Generator", "MANUALGENERATOR");

				// Token: 0x0400BF94 RID: 49044
				public static LocString DESC = "Watching Duplicants run on it is adorable... the electrical power is just an added bonus.";

				// Token: 0x0400BF95 RID: 49045
				public static LocString EFFECT = "Converts manual labor into electrical " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			// Token: 0x02002B9E RID: 11166
			public class MANUALPRESSUREDOOR
			{
				// Token: 0x0400BF96 RID: 49046
				public static LocString NAME = UI.FormatAsLink("Manual Airlock", "MANUALPRESSUREDOOR");

				// Token: 0x0400BF97 RID: 49047
				public static LocString DESC = "Airlocks can quarter off dangerous areas and prevent gases from seeping into the colony.";

				// Token: 0x0400BF98 RID: 49048
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});
			}

			// Token: 0x02002B9F RID: 11167
			public class MESHTILE
			{
				// Token: 0x0400BF99 RID: 49049
				public static LocString NAME = UI.FormatAsLink("Mesh Tile", "MESHTILE");

				// Token: 0x0400BF9A RID: 49050
				public static LocString DESC = "Mesh tile can be used to make Duplicant pathways in areas where liquid flows.";

				// Token: 0x0400BF9B RID: 49051
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nDoes not obstruct ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow."
				});
			}

			// Token: 0x02002BA0 RID: 11168
			public class PLASTICTILE
			{
				// Token: 0x0400BF9C RID: 49052
				public static LocString NAME = UI.FormatAsLink("Plastic Tile", "PLASTICTILE");

				// Token: 0x0400BF9D RID: 49053
				public static LocString DESC = "Plastic tile is mildly antiseptic and can help limit the spread of germs in a colony.";

				// Token: 0x0400BF9E RID: 49054
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
			}

			// Token: 0x02002BA1 RID: 11169
			public class GLASSTILE
			{
				// Token: 0x0400BF9F RID: 49055
				public static LocString NAME = UI.FormatAsLink("Window Tile", "GLASSTILE");

				// Token: 0x0400BFA0 RID: 49056
				public static LocString DESC = "Window tiles provide a barrier against liquid and gas and are completely transparent.";

				// Token: 0x0400BFA1 RID: 49057
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nAllows ",
					UI.FormatAsLink("Light", "LIGHT"),
					" and ",
					UI.FormatAsLink("Decor", "DECOR"),
					" to pass through."
				});
			}

			// Token: 0x02002BA2 RID: 11170
			public class METALTILE
			{
				// Token: 0x0400BFA2 RID: 49058
				public static LocString NAME = UI.FormatAsLink("Metal Tile", "METALTILE");

				// Token: 0x0400BFA3 RID: 49059
				public static LocString DESC = "Heat travels much more quickly through metal tile than other types of flooring.";

				// Token: 0x0400BFA4 RID: 49060
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
			}

			// Token: 0x02002BA3 RID: 11171
			public class BUNKERTILE
			{
				// Token: 0x0400BFA5 RID: 49061
				public static LocString NAME = UI.FormatAsLink("Bunker Tile", "BUNKERTILE");

				// Token: 0x0400BFA6 RID: 49062
				public static LocString DESC = "Bunker tile can build strong shelters in otherwise dangerous environments.";

				// Token: 0x0400BFA7 RID: 49063
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nCan withstand extreme pressures and impacts.";
			}

			// Token: 0x02002BA4 RID: 11172
			public class STORAGETILE
			{
				// Token: 0x0400BFA8 RID: 49064
				public static LocString NAME = UI.FormatAsLink("Storage Tile", "STORAGETILE");

				// Token: 0x0400BFA9 RID: 49065
				public static LocString DESC = "Storage tiles keep selected non-edible solids out of the way.";

				// Token: 0x0400BFAA RID: 49066
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nProvides built-in storage for small spaces.";
			}

			// Token: 0x02002BA5 RID: 11173
			public class CARPETTILE
			{
				// Token: 0x0400BFAB RID: 49067
				public static LocString NAME = UI.FormatAsLink("Carpeted Tile", "CARPETTILE");

				// Token: 0x0400BFAC RID: 49068
				public static LocString DESC = "Soft on little Duplicant toesies.";

				// Token: 0x0400BFAD RID: 49069
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x02002BA6 RID: 11174
			public class MOULDINGTILE
			{
				// Token: 0x0400BFAE RID: 49070
				public static LocString NAME = UI.FormatAsLink("Trimming Tile", "MOUDLINGTILE");

				// Token: 0x0400BFAF RID: 49071
				public static LocString DESC = "Trimming is used as purely decorative lining for walls and structures.";

				// Token: 0x0400BFB0 RID: 49072
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x02002BA7 RID: 11175
			public class MONUMENTBOTTOM
			{
				// Token: 0x0400BFB1 RID: 49073
				public static LocString NAME = UI.FormatAsLink("Monument Base", "MONUMENTBOTTOM");

				// Token: 0x0400BFB2 RID: 49074
				public static LocString DESC = "The base of a monument must be constructed first.";

				// Token: 0x0400BFB3 RID: 49075
				public static LocString EFFECT = "Builds the bottom section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";

				// Token: 0x02003A6F RID: 14959
				public class FACADES
				{
					// Token: 0x02003E8A RID: 16010
					public class OPTION_A
					{
						// Token: 0x0400F5E2 RID: 62946
						public static LocString NAME = "On Asteroid I";

						// Token: 0x0400F5E3 RID: 62947
						public static LocString DESC = "Standing tall.";
					}

					// Token: 0x02003E8B RID: 16011
					public class OPTION_B
					{
						// Token: 0x0400F5E4 RID: 62948
						public static LocString NAME = "On Asteroid II";

						// Token: 0x0400F5E5 RID: 62949
						public static LocString DESC = "Standing purposefully.";
					}

					// Token: 0x02003E8C RID: 16012
					public class OPTION_C
					{
						// Token: 0x0400F5E6 RID: 62950
						public static LocString NAME = "On Asteroid III";

						// Token: 0x0400F5E7 RID: 62951
						public static LocString DESC = "Their knees were knockin'.";
					}

					// Token: 0x02003E8D RID: 16013
					public class OPTION_D
					{
						// Token: 0x0400F5E8 RID: 62952
						public static LocString NAME = "Scientific Seat";

						// Token: 0x0400F5E9 RID: 62953
						public static LocString DESC = "In celebration of science!";
					}

					// Token: 0x02003E8E RID: 16014
					public class OPTION_E
					{
						// Token: 0x0400F5EA RID: 62954
						public static LocString NAME = "On Asteroid IV";

						// Token: 0x0400F5EB RID: 62955
						public static LocString DESC = "It's a confident stance.";
					}

					// Token: 0x02003E8F RID: 16015
					public class OPTION_F
					{
						// Token: 0x0400F5EC RID: 62956
						public static LocString NAME = "On Asteroid V";

						// Token: 0x0400F5ED RID: 62957
						public static LocString DESC = "One knee tucked toward the other.";
					}

					// Token: 0x02003E90 RID: 16016
					public class OPTION_G
					{
						// Token: 0x0400F5EE RID: 62958
						public static LocString NAME = "On Asteroid VI";

						// Token: 0x0400F5EF RID: 62959
						public static LocString DESC = "One small step for Duplicantkind...";
					}

					// Token: 0x02003E91 RID: 16017
					public class OPTION_H
					{
						// Token: 0x0400F5F0 RID: 62960
						public static LocString NAME = "Hatch Hunter";

						// Token: 0x0400F5F1 RID: 62961
						public static LocString DESC = "Atop a pair of conquered critters.";
					}

					// Token: 0x02003E92 RID: 16018
					public class OPTION_I
					{
						// Token: 0x0400F5F2 RID: 62962
						public static LocString NAME = "Trash Tranquility";

						// Token: 0x0400F5F3 RID: 62963
						public static LocString DESC = "Finding peace amid the debris.";
					}

					// Token: 0x02003E93 RID: 16019
					public class OPTION_J
					{
						// Token: 0x0400F5F4 RID: 62964
						public static LocString NAME = "Fish Stomper";

						// Token: 0x0400F5F5 RID: 62965
						public static LocString DESC = "That can't be comfortable.";
					}

					// Token: 0x02003E94 RID: 16020
					public class OPTION_K
					{
						// Token: 0x0400F5F6 RID: 62966
						public static LocString NAME = "Egg Equanimity";

						// Token: 0x0400F5F7 RID: 62967
						public static LocString DESC = "One must give the soul time to truly hatch.";
					}

					// Token: 0x02003E95 RID: 16021
					public class OPTION_L
					{
						// Token: 0x0400F5F8 RID: 62968
						public static LocString NAME = "Tilted Nosecone";

						// Token: 0x0400F5F9 RID: 62969
						public static LocString DESC = "A slightly unbalanced base.";
					}

					// Token: 0x02003E96 RID: 16022
					public class OPTION_M
					{
						// Token: 0x0400F5FA RID: 62970
						public static LocString NAME = "Sweet Seat";

						// Token: 0x0400F5FB RID: 62971
						public static LocString DESC = "In honor of the sugar engine.";
					}

					// Token: 0x02003E97 RID: 16023
					public class OPTION_N
					{
						// Token: 0x0400F5FC RID: 62972
						public static LocString NAME = "CO2 Straddle";

						// Token: 0x0400F5FD RID: 62973
						public static LocString DESC = "Riding a carbon dioxide rocket engine to glory.";
					}

					// Token: 0x02003E98 RID: 16024
					public class OPTION_O
					{
						// Token: 0x0400F5FE RID: 62974
						public static LocString NAME = "Petroleum Pose";

						// Token: 0x0400F5FF RID: 62975
						public static LocString DESC = "Atop a small petroleum rocket engine.";
					}

					// Token: 0x02003E99 RID: 16025
					public class OPTION_P
					{
						// Token: 0x0400F600 RID: 62976
						public static LocString NAME = "Spacefarer Stance";

						// Token: 0x0400F601 RID: 62977
						public static LocString DESC = "Atop a solo spacefarer rocket nosecone.";
					}

					// Token: 0x02003E9A RID: 16026
					public class OPTION_Q
					{
						// Token: 0x0400F602 RID: 62978
						public static LocString NAME = "Seat of Power";

						// Token: 0x0400F603 RID: 62979
						public static LocString DESC = "Atop a radbolt rocket engine.";
					}

					// Token: 0x02003E9B RID: 16027
					public class OPTION_R
					{
						// Token: 0x0400F604 RID: 62980
						public static LocString NAME = "Sweepy I";

						// Token: 0x0400F605 RID: 62981
						public static LocString DESC = "Atop a sleeping Sweepy bot.";
					}

					// Token: 0x02003E9C RID: 16028
					public class OPTION_S
					{
						// Token: 0x0400F606 RID: 62982
						public static LocString NAME = "Sweepy II";

						// Token: 0x0400F607 RID: 62983
						public static LocString DESC = "Atop a curious Sweepy bot.";
					}

					// Token: 0x02003E9D RID: 16029
					public class OPTION_T
					{
						// Token: 0x0400F608 RID: 62984
						public static LocString NAME = "Sweepy III";

						// Token: 0x0400F609 RID: 62985
						public static LocString DESC = "Atop a happy lil' Sweepy bot.";
					}
				}
			}

			// Token: 0x02002BA8 RID: 11176
			public class MONUMENTMIDDLE
			{
				// Token: 0x0400BFB4 RID: 49076
				public static LocString NAME = UI.FormatAsLink("Monument Midsection", "MONUMENTMIDDLE");

				// Token: 0x0400BFB5 RID: 49077
				public static LocString DESC = "Customized sections of a Great Monument can be mixed and matched.";

				// Token: 0x0400BFB6 RID: 49078
				public static LocString EFFECT = "Builds the middle section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";

				// Token: 0x02003A70 RID: 14960
				public class FACADES
				{
					// Token: 0x02003E9E RID: 16030
					public class OPTION_A
					{
						// Token: 0x0400F60A RID: 62986
						public static LocString NAME = "Thumbs Up";

						// Token: 0x0400F60B RID: 62987
						public static LocString DESC = "Good job, sculptor!";
					}

					// Token: 0x02003E9F RID: 16031
					public class OPTION_B
					{
						// Token: 0x0400F60C RID: 62988
						public static LocString NAME = "Big Wrench";

						// Token: 0x0400F60D RID: 62989
						public static LocString DESC = "Lefty loose-y, righty tighty.";
					}

					// Token: 0x02003EA0 RID: 16032
					public class OPTION_C
					{
						// Token: 0x0400F60E RID: 62990
						public static LocString NAME = "Um, Excuse Me";

						// Token: 0x0400F60F RID: 62991
						public static LocString DESC = "Celebrates uncertainty.";
					}

					// Token: 0x02003EA1 RID: 16033
					public class OPTION_D
					{
						// Token: 0x0400F610 RID: 62992
						public static LocString NAME = "Hands on Hips";

						// Token: 0x0400F611 RID: 62993
						public static LocString DESC = "Makes the torso seem bigger and more intimidating than it is.";
					}

					// Token: 0x02003EA2 RID: 16034
					public class OPTION_E
					{
						// Token: 0x0400F612 RID: 62994
						public static LocString NAME = "The Shrug";

						// Token: 0x0400F613 RID: 62995
						public static LocString DESC = "Sometimes things are good enough just as they are.";
					}

					// Token: 0x02003EA3 RID: 16035
					public class OPTION_F
					{
						// Token: 0x0400F614 RID: 62996
						public static LocString NAME = "You Betcha";

						// Token: 0x0400F615 RID: 62997
						public static LocString DESC = "The finger gun of approval.";
					}

					// Token: 0x02003EA4 RID: 16036
					public class OPTION_G
					{
						// Token: 0x0400F616 RID: 62998
						public static LocString NAME = "Well Hello There";

						// Token: 0x0400F617 RID: 62999
						public static LocString DESC = "It's quite torso-forward.";
					}

					// Token: 0x02003EA5 RID: 16037
					public class OPTION_H
					{
						// Token: 0x0400F618 RID: 63000
						public static LocString NAME = "Fists of Fury";

						// Token: 0x0400F619 RID: 63001
						public static LocString DESC = "Let 'em fly!";
					}

					// Token: 0x02003EA6 RID: 16038
					public class OPTION_I
					{
						// Token: 0x0400F61A RID: 63002
						public static LocString NAME = "Hatch Hug";

						// Token: 0x0400F61B RID: 63003
						public static LocString DESC = "Cradling a cozy critter.";
					}

					// Token: 0x02003EA7 RID: 16039
					public class OPTION_J
					{
						// Token: 0x0400F61C RID: 63004
						public static LocString NAME = "Casual Elegance";

						// Token: 0x0400F61D RID: 63005
						public static LocString DESC = "Leaning casually, with grace.";
					}

					// Token: 0x02003EA8 RID: 16040
					public class OPTION_K
					{
						// Token: 0x0400F61E RID: 63006
						public static LocString NAME = "Arms Ajar";

						// Token: 0x0400F61F RID: 63007
						public static LocString DESC = "Hands hover slightly away from the body, as though raised in wonder.";
					}

					// Token: 0x02003EA9 RID: 16041
					public class OPTION_L
					{
						// Token: 0x0400F620 RID: 63008
						public static LocString NAME = "Babes in Arms I";

						// Token: 0x0400F621 RID: 63009
						public static LocString DESC = "Cradling a couple of smooth lil' critter babies.";
					}

					// Token: 0x02003EAA RID: 16042
					public class OPTION_M
					{
						// Token: 0x0400F622 RID: 63010
						public static LocString NAME = "Model Rocket";

						// Token: 0x0400F623 RID: 63011
						public static LocString DESC = "Celebrates a cosmic undertaking.";
					}

					// Token: 0x02003EAB RID: 16043
					public class OPTION_N
					{
						// Token: 0x0400F624 RID: 63012
						public static LocString NAME = "Babes in Arms II";

						// Token: 0x0400F625 RID: 63013
						public static LocString DESC = "An armful of chonky lil' critter babies.";
					}

					// Token: 0x02003EAC RID: 16044
					public class OPTION_O
					{
						// Token: 0x0400F626 RID: 63014
						public static LocString NAME = "Babes in Arms III";

						// Token: 0x0400F627 RID: 63015
						public static LocString DESC = "Embracing buggy lil' critter babies.";
					}
				}
			}

			// Token: 0x02002BA9 RID: 11177
			public class MONUMENTTOP
			{
				// Token: 0x0400BFB7 RID: 49079
				public static LocString NAME = UI.FormatAsLink("Monument Top", "MONUMENTTOP");

				// Token: 0x0400BFB8 RID: 49080
				public static LocString DESC = "Building a Great Monument will declare to the universe that this hunk of rock is your own.";

				// Token: 0x0400BFB9 RID: 49081
				public static LocString EFFECT = "Builds the top section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";

				// Token: 0x02003A71 RID: 14961
				public class FACADES
				{
					// Token: 0x02003EAD RID: 16045
					public class OPTION_A
					{
						// Token: 0x0400F628 RID: 63016
						public static LocString NAME = "Leira Noggin";

						// Token: 0x0400F629 RID: 63017
						public static LocString DESC = "A massive replica of Leira's smiling face.";
					}

					// Token: 0x02003EAE RID: 16046
					public class OPTION_B
					{
						// Token: 0x0400F62A RID: 63018
						public static LocString NAME = "Gossmann Noggin";

						// Token: 0x0400F62B RID: 63019
						public static LocString DESC = "A massive replica of Gossmann's determined gaze.";
					}

					// Token: 0x02003EAF RID: 16047
					public class OPTION_C
					{
						// Token: 0x0400F62C RID: 63020
						public static LocString NAME = "Puft Top";

						// Token: 0x0400F62D RID: 63021
						public static LocString DESC = "A great-monument-sized puft.";
					}

					// Token: 0x02003EB0 RID: 16048
					public class OPTION_D
					{
						// Token: 0x0400F62E RID: 63022
						public static LocString NAME = "Nikola Noggin";

						// Token: 0x0400F62F RID: 63023
						public static LocString DESC = "A massive replica of Nikola's post-explosion expression.";
					}

					// Token: 0x02003EB1 RID: 16049
					public class OPTION_E
					{
						// Token: 0x0400F630 RID: 63024
						public static LocString NAME = "Burt Noggin";

						// Token: 0x0400F631 RID: 63025
						public static LocString DESC = "A massive replica of Burt's critter-spotting expression.";
					}

					// Token: 0x02003EB2 RID: 16050
					public class OPTION_F
					{
						// Token: 0x0400F632 RID: 63026
						public static LocString NAME = "Rowan Noggin";

						// Token: 0x0400F633 RID: 63027
						public static LocString DESC = "A massive replica of Rowan's serene smile.";
					}

					// Token: 0x02003EB3 RID: 16051
					public class OPTION_G
					{
						// Token: 0x0400F634 RID: 63028
						public static LocString NAME = "Nisbet Noggin";

						// Token: 0x0400F635 RID: 63029
						public static LocString DESC = "A massive replica of Nisbet when she sees someone whose name she's forgotten.";
					}

					// Token: 0x02003EB4 RID: 16052
					public class OPTION_H
					{
						// Token: 0x0400F636 RID: 63030
						public static LocString NAME = "Ashkan Noggin";

						// Token: 0x0400F637 RID: 63031
						public static LocString DESC = "A massive replica of Ashkan's fossil-discovering expression.";
					}

					// Token: 0x02003EB5 RID: 16053
					public class OPTION_I
					{
						// Token: 0x0400F638 RID: 63032
						public static LocString NAME = "Ren Noggin";

						// Token: 0x0400F639 RID: 63033
						public static LocString DESC = "A massive replica of Ren's smoochy face.";
					}

					// Token: 0x02003EB6 RID: 16054
					public class OPTION_J
					{
						// Token: 0x0400F63A RID: 63034
						public static LocString NAME = "Hatch Top";

						// Token: 0x0400F63B RID: 63035
						public static LocString DESC = "A great-monument-sized Hatch.";
					}

					// Token: 0x02003EB7 RID: 16055
					public class OPTION_K
					{
						// Token: 0x0400F63C RID: 63036
						public static LocString NAME = "Glossy Drecko Top";

						// Token: 0x0400F63D RID: 63037
						public static LocString DESC = "A great-monument-sized Glossy Drecko.";
					}

					// Token: 0x02003EB8 RID: 16056
					public class OPTION_L
					{
						// Token: 0x0400F63E RID: 63038
						public static LocString NAME = "Shove Vole Top";

						// Token: 0x0400F63F RID: 63039
						public static LocString DESC = "A great-monument-sized Shove Vole.";
					}

					// Token: 0x02003EB9 RID: 16057
					public class OPTION_M
					{
						// Token: 0x0400F640 RID: 63040
						public static LocString NAME = "Gassy Moo Top";

						// Token: 0x0400F641 RID: 63041
						public static LocString DESC = "A great-monument-sized Gassy Moo. Gassier and moo-ier than ever.";
					}

					// Token: 0x02003EBA RID: 16058
					public class OPTION_N
					{
						// Token: 0x0400F642 RID: 63042
						public static LocString NAME = "Morb Top";

						// Token: 0x0400F643 RID: 63043
						public static LocString DESC = "A great-monument-sized Morb.";
					}

					// Token: 0x02003EBB RID: 16059
					public class OPTION_O
					{
						// Token: 0x0400F644 RID: 63044
						public static LocString NAME = "Shine Bug Top";

						// Token: 0x0400F645 RID: 63045
						public static LocString DESC = "A great-monument-sized Shine Bug.";
					}

					// Token: 0x02003EBC RID: 16060
					public class OPTION_P
					{
						// Token: 0x0400F646 RID: 63046
						public static LocString NAME = "Slickster Top";

						// Token: 0x0400F647 RID: 63047
						public static LocString DESC = "A great-monument-sized Slickster.";
					}

					// Token: 0x02003EBD RID: 16061
					public class OPTION_Q
					{
						// Token: 0x0400F648 RID: 63048
						public static LocString NAME = "Pacu Top";

						// Token: 0x0400F649 RID: 63049
						public static LocString DESC = "A great-monument-sized underbite.";
					}

					// Token: 0x02003EBE RID: 16062
					public class OPTION_R
					{
						// Token: 0x0400F64A RID: 63050
						public static LocString NAME = "Beeta Top";

						// Token: 0x0400F64B RID: 63051
						public static LocString DESC = "A great-monument-sized Beeta.";
					}

					// Token: 0x02003EBF RID: 16063
					public class OPTION_S
					{
						// Token: 0x0400F64C RID: 63052
						public static LocString NAME = "Sweetle Top";

						// Token: 0x0400F64D RID: 63053
						public static LocString DESC = "A great-monument-sized Sweetle.";
					}

					// Token: 0x02003EC0 RID: 16064
					public class OPTION_T
					{
						// Token: 0x0400F64E RID: 63054
						public static LocString NAME = "Plug Slug Top";

						// Token: 0x0400F64F RID: 63055
						public static LocString DESC = "A great-monument-sized Plug Slug. Does not require a power source.";
					}

					// Token: 0x02003EC1 RID: 16065
					public class OPTION_U
					{
						// Token: 0x0400F650 RID: 63056
						public static LocString NAME = "Grubgrub Top";

						// Token: 0x0400F651 RID: 63057
						public static LocString DESC = "A great-monument-sized garden critter.";
					}

					// Token: 0x02003EC2 RID: 16066
					public class OPTION_V
					{
						// Token: 0x0400F652 RID: 63058
						public static LocString NAME = "Rover Top";

						// Token: 0x0400F653 RID: 63059
						public static LocString DESC = "It has no mouth, but still looks like it's smiling.";
					}

					// Token: 0x02003EC3 RID: 16067
					public class OPTION_W
					{
						// Token: 0x0400F654 RID: 63060
						public static LocString NAME = "Radsick Top I";

						// Token: 0x0400F655 RID: 63061
						public static LocString DESC = "A visual reminder about radiation safety.";
					}

					// Token: 0x02003EC4 RID: 16068
					public class OPTION_X
					{
						// Token: 0x0400F656 RID: 63062
						public static LocString NAME = "Radsick Top II";

						// Token: 0x0400F657 RID: 63063
						public static LocString DESC = "Progress comes at a price.";
					}

					// Token: 0x02003EC5 RID: 16069
					public class OPTION_Y
					{
						// Token: 0x0400F658 RID: 63064
						public static LocString NAME = "Radsick Top III";

						// Token: 0x0400F659 RID: 63065
						public static LocString DESC = "A cautionary tale for careless Duplicants.";
					}

					// Token: 0x02003EC6 RID: 16070
					public class OPTION_Z
					{
						// Token: 0x0400F65A RID: 63066
						public static LocString NAME = "Radsick Top IV";

						// Token: 0x0400F65B RID: 63067
						public static LocString DESC = "Excellent choice of decor for the entrance to highly radioactive site.";
					}
				}
			}

			// Token: 0x02002BAA RID: 11178
			public class MICROBEMUSHER
			{
				// Token: 0x0400BFBA RID: 49082
				public static LocString NAME = UI.FormatAsLink("Microbe Musher", "MICROBEMUSHER");

				// Token: 0x0400BFBB RID: 49083
				public static LocString DESC = "Musher recipes will keep Duplicants fed, but may impact health and morale over time.";

				// Token: 0x0400BFBC RID: 49084
				public static LocString EFFECT = "Produces low quality " + UI.FormatAsLink("Food", "FOOD") + " using common ingredients.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x02003A72 RID: 14962
				public class FACADES
				{
					// Token: 0x02003EC7 RID: 16071
					public class DEFAULT_MICROBEMUSHER
					{
						// Token: 0x0400F65C RID: 63068
						public static LocString NAME = UI.FormatAsLink("Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400F65D RID: 63069
						public static LocString DESC = "Musher recipes will keep Duplicants fed, but may impact health and morale over time.";
					}

					// Token: 0x02003EC8 RID: 16072
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400F65E RID: 63070
						public static LocString NAME = UI.FormatAsLink("Faint Purple Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400F65F RID: 63071
						public static LocString DESC = "A colorful distraction from the actual quality of the food.";
					}

					// Token: 0x02003EC9 RID: 16073
					public class YELLOW_TARTAR
					{
						// Token: 0x0400F660 RID: 63072
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400F661 RID: 63073
						public static LocString DESC = "Makes meals that are memorable for all the wrong reasons.";
					}

					// Token: 0x02003ECA RID: 16074
					public class RED_ROSE
					{
						// Token: 0x0400F662 RID: 63074
						public static LocString NAME = UI.FormatAsLink("Puce Pink Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400F663 RID: 63075
						public static LocString DESC = "Hunger strikes are not an option, but color-coordination is.";
					}

					// Token: 0x02003ECB RID: 16075
					public class GREEN_MUSH
					{
						// Token: 0x0400F664 RID: 63076
						public static LocString NAME = UI.FormatAsLink("Mush Green Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400F665 RID: 63077
						public static LocString DESC = "Edible colloids for dinner <i>again</i>?";
					}

					// Token: 0x02003ECC RID: 16076
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400F666 RID: 63078
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400F667 RID: 63079
						public static LocString DESC = "Prioritizes nutritional value over flavor.";
					}
				}
			}

			// Token: 0x02002BAB RID: 11179
			public class MINERALDEOXIDIZER
			{
				// Token: 0x0400BFBD RID: 49085
				public static LocString NAME = UI.FormatAsLink("Oxygen Diffuser", "MINERALDEOXIDIZER");

				// Token: 0x0400BFBE RID: 49086
				public static LocString DESC = "Oxygen diffusers are inefficient, but output enough oxygen to keep a colony breathing.";

				// Token: 0x0400BFBF RID: 49087
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts large amounts of ",
					UI.FormatAsLink("Algae", "ALGAE"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x02002BAC RID: 11180
			public class SUBLIMATIONSTATION
			{
				// Token: 0x0400BFC0 RID: 49088
				public static LocString NAME = UI.FormatAsLink("Sublimation Station", "SUBLIMATIONSTATION");

				// Token: 0x0400BFC1 RID: 49089
				public static LocString DESC = "Sublimation is the sublime process by which solids convert directly into gas.";

				// Token: 0x0400BFC2 RID: 49090
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Speeds up the conversion of ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" into ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x02002BAD RID: 11181
			public class WOODTILE
			{
				// Token: 0x0400BFC3 RID: 49091
				public static LocString NAME = UI.FormatAsLink("Wood Tile", "WOODTILE");

				// Token: 0x0400BFC4 RID: 49092
				public static LocString DESC = "Rooms built with wood tile are cozy and pleasant.";

				// Token: 0x0400BFC5 RID: 49093
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nProvides good insulation and boosts ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x02002BAE RID: 11182
			public class SNOWTILE
			{
				// Token: 0x0400BFC6 RID: 49094
				public static LocString NAME = UI.FormatAsLink("Snow Tile", "SNOWTILE");

				// Token: 0x0400BFC7 RID: 49095
				public static LocString DESC = "Snow tiles have low thermal conductivity, but will melt if temperatures get too high.";

				// Token: 0x0400BFC8 RID: 49096
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nInsulates rooms to reduce " + UI.FormatAsLink("Heat", "HEAT") + " loss in cold climates.";
			}

			// Token: 0x02002BAF RID: 11183
			public class CAMPFIRE
			{
				// Token: 0x0400BFC9 RID: 49097
				public static LocString NAME = UI.FormatAsLink("Wood Heater", "CAMPFIRE");

				// Token: 0x0400BFCA RID: 49098
				public static LocString DESC = "Wood heaters dry out soggy feet and help Duplicants forget how cold they are.";

				// Token: 0x0400BFCB RID: 49099
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Wood", "WOOD"),
					" in order to ",
					UI.FormatAsLink("Heat", "HEAT"),
					" chilly surroundings."
				});
			}

			// Token: 0x02002BB0 RID: 11184
			public class ICEKETTLE
			{
				// Token: 0x0400BFCC RID: 49100
				public static LocString NAME = UI.FormatAsLink("Ice Liquefier", "ICEKETTLE");

				// Token: 0x0400BFCD RID: 49101
				public static LocString DESC = "The water never gets hot enough to burn the tongue.";

				// Token: 0x0400BFCE RID: 49102
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Wood", "WOOD"),
					" to melt ",
					UI.FormatAsLink("Ice", "ICE"),
					" into ",
					UI.FormatAsLink("Water", "WATER"),
					", which can be bottled for transport."
				});
			}

			// Token: 0x02002BB1 RID: 11185
			public class WOODSTORAGE
			{
				// Token: 0x0400BFCF RID: 49103
				public static LocString NAME = "Wood Pile";

				// Token: 0x0400BFD0 RID: 49104
				public static LocString DESC = "Once it's empty, there's no use pining for more.";

				// Token: 0x0400BFD1 RID: 49105
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores a finite supply of ",
					UI.FormatAsLink("Wood", "WOOD"),
					", which can be used for construction or to produce ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			// Token: 0x02002BB2 RID: 11186
			public class DLC2POITECHUNLOCKS
			{
				// Token: 0x0400BFD2 RID: 49106
				public static LocString NAME = "Research Portal";

				// Token: 0x0400BFD3 RID: 49107
				public static LocString DESC = "A functional research decrypter with one transmission remaining.\n\nIt was designed to support colony survival.";
			}

			// Token: 0x02002BB3 RID: 11187
			public class DLC4POITECHUNLOCKS
			{
				// Token: 0x0400BFD4 RID: 49108
				public static LocString NAME = "Research Portal";

				// Token: 0x0400BFD5 RID: 49109
				public static LocString DESC = "A functional research decrypter with one transmission remaining.\n\nIt was designed to support colony survival.";
			}

			// Token: 0x02002BB4 RID: 11188
			public class DEEPFRYER
			{
				// Token: 0x0400BFD6 RID: 49110
				public static LocString NAME = UI.FormatAsLink("Deep Fryer", "DEEPFRYER");

				// Token: 0x0400BFD7 RID: 49111
				public static LocString DESC = "Everything tastes better when it's deep-fried.";

				// Token: 0x0400BFD8 RID: 49112
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					" to cook a wide variety of improved ",
					UI.FormatAsLink("Foods", "FOOD"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x02003A73 RID: 14963
				public class STATUSITEMS
				{
					// Token: 0x02003ECD RID: 16077
					public class OUTSIDE_KITCHEN
					{
						// Token: 0x0400F668 RID: 63080
						public static LocString NAME = "Outside of Kitchen";

						// Token: 0x0400F669 RID: 63081
						public static LocString TOOLTIP = "This building must be in a Kitchen before it can be used";
					}
				}
			}

			// Token: 0x02002BB5 RID: 11189
			public class ORESCRUBBER
			{
				// Token: 0x0400BFD9 RID: 49113
				public static LocString NAME = UI.FormatAsLink("Ore Scrubber", "ORESCRUBBER");

				// Token: 0x0400BFDA RID: 49114
				public static LocString DESC = "Scrubbers sanitize freshly mined materials before they're brought into the colony.";

				// Token: 0x0400BFDB RID: 49115
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Kills a significant amount of ",
					UI.FormatAsLink("Germs", "DISEASE"),
					" present on ",
					UI.FormatAsLink("Raw Ore", "RAWMINERAL"),
					"."
				});
			}

			// Token: 0x02002BB6 RID: 11190
			public class OUTHOUSE
			{
				// Token: 0x0400BFDC RID: 49116
				public static LocString NAME = UI.FormatAsLink("Outhouse", "OUTHOUSE");

				// Token: 0x0400BFDD RID: 49117
				public static LocString DESC = "The colony that eats together, excretes together.";

				// Token: 0x0400BFDE RID: 49118
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Gives Duplicants a place to relieve themselves.\n\nRequires no ",
					UI.FormatAsLink("Piping", "LIQUIDPIPING"),
					".\n\nMust be periodically emptied of ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			// Token: 0x02002BB7 RID: 11191
			public class APOTHECARY
			{
				// Token: 0x0400BFDF RID: 49119
				public static LocString NAME = UI.FormatAsLink("Apothecary", "APOTHECARY");

				// Token: 0x0400BFE0 RID: 49120
				public static LocString DESC = "Some medications help prevent diseases, while others aim to alleviate existing illness.";

				// Token: 0x0400BFE1 RID: 49121
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Medicine", "MEDICINE"),
					" to cure most basic ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nDuplicants must possess the Medicine Compounding ",
					UI.FormatAsLink("Skill", "ROLES"),
					" to fabricate medicines.\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x02002BB8 RID: 11192
			public class ADVANCEDAPOTHECARY
			{
				// Token: 0x0400BFE2 RID: 49122
				public static LocString NAME = UI.FormatAsLink("Nuclear Apothecary", "ADVANCEDAPOTHECARY");

				// Token: 0x0400BFE3 RID: 49123
				public static LocString DESC = "Some medications help prevent diseases, while others aim to alleviate existing illness.";

				// Token: 0x0400BFE4 RID: 49124
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Medicine", "MEDICINE"),
					" to cure most basic ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nDuplicants must possess the Medicine Compounding ",
					UI.FormatAsLink("Skill", "ROLES"),
					" to fabricate medicines.\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x02002BB9 RID: 11193
			public class PLANTERBOX
			{
				// Token: 0x0400BFE5 RID: 49125
				public static LocString NAME = UI.FormatAsLink("Planter Box", "PLANTERBOX");

				// Token: 0x0400BFE6 RID: 49126
				public static LocString DESC = "Domestically grown seeds mature more quickly than wild plants.";

				// Token: 0x0400BFE7 RID: 49127
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					"."
				});

				// Token: 0x02003A74 RID: 14964
				public class FACADES
				{
					// Token: 0x02003ECE RID: 16078
					public class DEFAULT_PLANTERBOX
					{
						// Token: 0x0400F66A RID: 63082
						public static LocString NAME = UI.FormatAsLink("Planter Box", "PLANTERBOX");

						// Token: 0x0400F66B RID: 63083
						public static LocString DESC = "Domestically grown seeds mature more quickly than wild plants.";
					}

					// Token: 0x02003ECF RID: 16079
					public class MEALWOOD
					{
						// Token: 0x0400F66C RID: 63084
						public static LocString NAME = UI.FormatAsLink("Mealy Teal Planter Box", "PLANTERBOX");

						// Token: 0x0400F66D RID: 63085
						public static LocString DESC = "Inspired by genetically modified nature.";
					}

					// Token: 0x02003ED0 RID: 16080
					public class BRISTLEBLOSSOM
					{
						// Token: 0x0400F66E RID: 63086
						public static LocString NAME = UI.FormatAsLink("Bristly Green Planter Box", "PLANTERBOX");

						// Token: 0x0400F66F RID: 63087
						public static LocString DESC = "The interior is lined with tiny barbs.";
					}

					// Token: 0x02003ED1 RID: 16081
					public class WHEEZEWORT
					{
						// Token: 0x0400F670 RID: 63088
						public static LocString NAME = UI.FormatAsLink("Wheezy Whorl Planter Box", "PLANTERBOX");

						// Token: 0x0400F671 RID: 63089
						public static LocString DESC = "For the dreamy agriculturalist.";
					}

					// Token: 0x02003ED2 RID: 16082
					public class SLEETWHEAT
					{
						// Token: 0x0400F672 RID: 63090
						public static LocString NAME = UI.FormatAsLink("Sleet Blue Planter Box", "PLANTERBOX");

						// Token: 0x0400F673 RID: 63091
						public static LocString DESC = "The thick paint drips are invisible from a distance.";
					}

					// Token: 0x02003ED3 RID: 16083
					public class SALMON_PINK
					{
						// Token: 0x0400F674 RID: 63092
						public static LocString NAME = UI.FormatAsLink("Flashy Planter Box", "PLANTERBOX");

						// Token: 0x0400F675 RID: 63093
						public static LocString DESC = "It's not exactly a subtle color.";
					}
				}
			}

			// Token: 0x02002BBA RID: 11194
			public class PRESSUREDOOR
			{
				// Token: 0x0400BFE8 RID: 49128
				public static LocString NAME = UI.FormatAsLink("Mechanized Airlock", "PRESSUREDOOR");

				// Token: 0x0400BFE9 RID: 49129
				public static LocString DESC = "Mechanized airlocks open and close more quickly than other types of door.";

				// Token: 0x0400BFEA RID: 49130
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nFunctions as a ",
					UI.FormatAsLink("Manual Airlock", "MANUALPRESSUREDOOR"),
					" when no ",
					UI.FormatAsLink("Power", "POWER"),
					" is available.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});
			}

			// Token: 0x02002BBB RID: 11195
			public class BUNKERDOOR
			{
				// Token: 0x0400BFEB RID: 49131
				public static LocString NAME = UI.FormatAsLink("Bunker Door", "BUNKERDOOR");

				// Token: 0x0400BFEC RID: 49132
				public static LocString DESC = "A massive, slow-moving door which is nearly indestructible.";

				// Token: 0x0400BFED RID: 49133
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nCan withstand extremely high pressures and impacts."
				});
			}

			// Token: 0x02002BBC RID: 11196
			public class INSULATEDDOOR
			{
				// Token: 0x0400BFEE RID: 49134
				public static LocString NAME = UI.FormatAsLink("Insulated Door", "INSULATEDDOOR");

				// Token: 0x0400BFEF RID: 49135
				public static LocString DESC = "A slow-moving door that works best when it's closed.";

				// Token: 0x0400BFF0 RID: 49136
				public static LocString EFFECT = "Significantly reduces " + UI.FormatAsLink("Temperature", "HEAT") + " exchange between climate-controlled rooms.";
			}

			// Token: 0x02002BBD RID: 11197
			public class WOODENDOOR
			{
				// Token: 0x0400BFF1 RID: 49137
				public static LocString NAME = UI.FormatAsLink("Wicker Door", "WOODENDOOR");

				// Token: 0x0400BFF2 RID: 49138
				public static LocString DESC = "A breezy wooden door combines style and function.";

				// Token: 0x0400BFF3 RID: 49139
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Encloses areas without blocking ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});
			}

			// Token: 0x02002BBE RID: 11198
			public class RATIONBOX
			{
				// Token: 0x0400BFF4 RID: 49140
				public static LocString NAME = UI.FormatAsLink("Ration Box", "RATIONBOX");

				// Token: 0x0400BFF5 RID: 49141
				public static LocString DESC = "Ration boxes keep food safe from hungry critters, but don't slow food spoilage.";

				// Token: 0x0400BFF6 RID: 49142
				public static LocString EFFECT = "Stores a small amount of " + UI.FormatAsLink("Food", "FOOD") + ".\n\nFood must be delivered to boxes by Duplicants.";
			}

			// Token: 0x02002BBF RID: 11199
			public class PARKSIGN
			{
				// Token: 0x0400BFF7 RID: 49143
				public static LocString NAME = UI.FormatAsLink("Park Sign", "PARKSIGN");

				// Token: 0x0400BFF8 RID: 49144
				public static LocString DESC = "Passing through parks will increase Duplicant Morale.";

				// Token: 0x0400BFF9 RID: 49145
				public static LocString EFFECT = "Classifies an area as a Park or Nature Reserve.";
			}

			// Token: 0x02002BC0 RID: 11200
			public class RADIATIONLIGHT
			{
				// Token: 0x0400BFFA RID: 49146
				public static LocString NAME = UI.FormatAsLink("Radiation Lamp", "RADIATIONLIGHT");

				// Token: 0x0400BFFB RID: 49147
				public static LocString DESC = "Duplicants can become sick if exposed to radiation without protection.";

				// Token: 0x0400BFFC RID: 49148
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Emits ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					" that can be collected by a ",
					UI.FormatAsLink("Radbolt Generator", "HIGHENERGYPARTICLESPAWNER"),
					"."
				});
			}

			// Token: 0x02002BC1 RID: 11201
			public class REFRIGERATOR
			{
				// Token: 0x0400BFFD RID: 49149
				public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

				// Token: 0x0400BFFE RID: 49150
				public static LocString DESC = "Food spoilage can be slowed by ambient conditions as well as by refrigerators.";

				// Token: 0x0400BFFF RID: 49151
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Food", "FOOD"),
					" at an ideal ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" to prevent spoilage."
				});

				// Token: 0x0400C000 RID: 49152
				public static LocString LOGIC_PORT = "Full/Not Full";

				// Token: 0x0400C001 RID: 49153
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full";

				// Token: 0x0400C002 RID: 49154
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x02003A75 RID: 14965
				public class FACADES
				{
					// Token: 0x02003ED4 RID: 16084
					public class DEFAULT_REFRIGERATOR
					{
						// Token: 0x0400F676 RID: 63094
						public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

						// Token: 0x0400F677 RID: 63095
						public static LocString DESC = "Food spoilage can be slowed by ambient conditions as well as by refrigerators.";
					}

					// Token: 0x02003ED5 RID: 16085
					public class STRIPES_RED_WHITE
					{
						// Token: 0x0400F678 RID: 63096
						public static LocString NAME = UI.FormatAsLink("Bold Stripe Refrigerator", "REFRIGERATOR");

						// Token: 0x0400F679 RID: 63097
						public static LocString DESC = "Bold on the outside, cold on the inside!";
					}

					// Token: 0x02003ED6 RID: 16086
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400F67A RID: 63098
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Refrigerator", "REFRIGERATOR");

						// Token: 0x0400F67B RID: 63099
						public static LocString DESC = "For food so cold, it brings a tear to the eye.";
					}

					// Token: 0x02003ED7 RID: 16087
					public class GREEN_MUSH
					{
						// Token: 0x0400F67C RID: 63100
						public static LocString NAME = UI.FormatAsLink("Mush Green Refrigerator", "REFRIGERATOR");

						// Token: 0x0400F67D RID: 63101
						public static LocString DESC = "Honestly, this hue is particularly chilling.";
					}

					// Token: 0x02003ED8 RID: 16088
					public class RED_ROSE
					{
						// Token: 0x0400F67E RID: 63102
						public static LocString NAME = UI.FormatAsLink("Puce Pink Refrigerator", "REFRIGERATOR");

						// Token: 0x0400F67F RID: 63103
						public static LocString DESC = "Inspired by the Duplicant poem, \"Pretty in Puce.\"";
					}

					// Token: 0x02003ED9 RID: 16089
					public class YELLOW_TARTAR
					{
						// Token: 0x0400F680 RID: 63104
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Refrigerator", "REFRIGERATOR");

						// Token: 0x0400F681 RID: 63105
						public static LocString DESC = "Some Duplicants call it \"sunny\" yellow, but only because they've never seen the sun.";
					}

					// Token: 0x02003EDA RID: 16090
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400F682 RID: 63106
						public static LocString NAME = UI.FormatAsLink("Faint Purple Refrigerator", "REFRIGERATOR");

						// Token: 0x0400F683 RID: 63107
						public static LocString DESC = "This fridge makes color-coordination a (cold) snap.";
					}
				}
			}

			// Token: 0x02002BC2 RID: 11202
			public class ROLESTATION
			{
				// Token: 0x0400C003 RID: 49155
				public static LocString NAME = UI.FormatAsLink("Skills Board", "ROLESTATION");

				// Token: 0x0400C004 RID: 49156
				public static LocString DESC = "A skills board can teach special skills to Duplicants they can't learn on their own.";

				// Token: 0x0400C005 RID: 49157
				public static LocString EFFECT = "Allows Duplicants to spend Skill Points to learn new " + UI.FormatAsLink("Skills", "JOBS") + ".";
			}

			// Token: 0x02002BC3 RID: 11203
			public class RESETSKILLSSTATION
			{
				// Token: 0x0400C006 RID: 49158
				public static LocString NAME = UI.FormatAsLink("Skill Scrubber", "RESETSKILLSSTATION");

				// Token: 0x0400C007 RID: 49159
				public static LocString DESC = "Erase skills from a Duplicant's mind, returning them to their default abilities.";

				// Token: 0x0400C008 RID: 49160
				public static LocString EFFECT = "Refunds a Duplicant's Skill Points for reassignment.\n\nDuplicants will lose all assigned skills in the process.";
			}

			// Token: 0x02002BC4 RID: 11204
			public class RESEARCHCENTER
			{
				// Token: 0x0400C009 RID: 49161
				public static LocString NAME = UI.FormatAsLink("Research Station", "RESEARCHCENTER");

				// Token: 0x0400C00A RID: 49162
				public static LocString DESC = "Research stations are necessary for unlocking all research tiers.";

				// Token: 0x0400C00B RID: 49163
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Novice Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Dirt", "DIRT"),
					"."
				});
			}

			// Token: 0x02002BC5 RID: 11205
			public class ADVANCEDRESEARCHCENTER
			{
				// Token: 0x0400C00C RID: 49164
				public static LocString NAME = UI.FormatAsLink("Super Computer", "ADVANCEDRESEARCHCENTER");

				// Token: 0x0400C00D RID: 49165
				public static LocString DESC = "Super computers unlock higher technology tiers than research stations alone.";

				// Token: 0x0400C00E RID: 49166
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Advanced Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Water", "WATER"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Advanced Research", "RESEARCHING1"),
					" skill."
				});
			}

			// Token: 0x02002BC6 RID: 11206
			public class NUCLEARRESEARCHCENTER
			{
				// Token: 0x0400C00F RID: 49167
				public static LocString NAME = UI.FormatAsLink("Materials Study Terminal", "NUCLEARRESEARCHCENTER");

				// Token: 0x0400C010 RID: 49168
				public static LocString DESC = "Comes with a few ions thrown in, free of charge.";

				// Token: 0x0400C011 RID: 49169
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Materials Science Research", "RESEARCHDLC1"),
					" to unlock new technologies.\n\nConsumes Radbolts.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Applied Sciences Research", "ATOMICRESEARCH"),
					" skill."
				});
			}

			// Token: 0x02002BC7 RID: 11207
			public class ORBITALRESEARCHCENTER
			{
				// Token: 0x0400C012 RID: 49170
				public static LocString NAME = UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER");

				// Token: 0x0400C013 RID: 49171
				public static LocString DESC = "Orbital Data Collection Labs record data while orbiting a Planetoid and write it to a " + UI.FormatAsLink("Data Bank", "ORBITALRESEARCHDATABANK") + ". ";

				// Token: 0x0400C014 RID: 49172
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates ",
					UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK"),
					" that can be consumed at a ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
					" and ",
					UI.FormatAsLink("Power", "POWER"),
					"."
				});
			}

			// Token: 0x02002BC8 RID: 11208
			public class COSMICRESEARCHCENTER
			{
				// Token: 0x0400C015 RID: 49173
				public static LocString NAME = UI.FormatAsLink("Virtual Planetarium", "COSMICRESEARCHCENTER");

				// Token: 0x0400C016 RID: 49174
				public static LocString DESC = "Planetariums allow the simulated exploration of locations discovered with a telescope.";

				// Token: 0x0400C017 RID: 49175
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Interstellar Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes data from ",
					UI.FormatAsLink("Research Modules", "RESEARCHMODULE"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Astronomy", "ASTRONOMY"),
					" skill."
				});
			}

			// Token: 0x02002BC9 RID: 11209
			public class DLC1COSMICRESEARCHCENTER
			{
				// Token: 0x0400C018 RID: 49176
				public static LocString NAME = UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER");

				// Token: 0x0400C019 RID: 49177
				public static LocString DESC = "Planetariums allow the simulated exploration of locations recorded in " + UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK") + ".";

				// Token: 0x0400C01A RID: 49178
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Data Analysis Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK"),
					" generated by exploration."
				});
			}

			// Token: 0x02002BCA RID: 11210
			public class TELESCOPE
			{
				// Token: 0x0400C01B RID: 49179
				public static LocString NAME = UI.FormatAsLink("Telescope", "TELESCOPE");

				// Token: 0x0400C01C RID: 49180
				public static LocString DESC = "Telescopes are necessary for learning starmaps and conducting rocket missions.";

				// Token: 0x0400C01D RID: 49181
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Maps Starmap destinations, producing ",
					UI.FormatAsLink("Data Banks", "DATABANK"),
					" in the process.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Field Research", "RESEARCHING2"),
					" skill.\n\nBuilding must be exposed to space to function."
				});

				// Token: 0x0400C01E RID: 49182
				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			// Token: 0x02002BCB RID: 11211
			public class CLUSTERTELESCOPE
			{
				// Token: 0x0400C01F RID: 49183
				public static LocString NAME = UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE");

				// Token: 0x0400C020 RID: 49184
				public static LocString DESC = "Telescopes are necessary for studying space, allowing rocket travel to other worlds.";

				// Token: 0x0400C021 RID: 49185
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reveals visitable Planetoids in space, producing ",
					UI.FormatAsLink("Data Banks", "DATABANK"),
					" in the process.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Astronomy", "ASTRONOMY"),
					" skill.\n\nBuilding must be exposed to space to function."
				});

				// Token: 0x0400C022 RID: 49186
				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			// Token: 0x02002BCC RID: 11212
			public class CLUSTERTELESCOPEENCLOSED
			{
				// Token: 0x0400C023 RID: 49187
				public static LocString NAME = UI.FormatAsLink("Enclosed Telescope", "CLUSTERTELESCOPEENCLOSED");

				// Token: 0x0400C024 RID: 49188
				public static LocString DESC = "Telescopes are necessary for studying space, allowing rocket travel to other worlds.";

				// Token: 0x0400C025 RID: 49189
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reveals visitable Planetoids in space... in comfort!\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Astronomy", "ASTRONOMY"),
					" skill.\n\nExcellent sunburn protection (100%), partial ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" protection (",
					GameUtil.GetFormattedPercent(FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING * 100f, GameUtil.TimeSlice.None),
					") .\n\nBuilding must be exposed to space to function."
				});

				// Token: 0x0400C026 RID: 49190
				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			// Token: 0x02002BCD RID: 11213
			public class MISSIONCONTROL
			{
				// Token: 0x0400C027 RID: 49191
				public static LocString NAME = UI.FormatAsLink("Mission Control Station", "MISSIONCONTROL");

				// Token: 0x0400C028 RID: 49192
				public static LocString DESC = "Like a backseat driver who actually does know better.";

				// Token: 0x0400C029 RID: 49193
				public static LocString EFFECT = "Provides guidance data to rocket pilots, to improve rocket speed.\n\nMust be operated by a Duplicant with the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nRequires a clear line of sight to space in order to function.";
			}

			// Token: 0x02002BCE RID: 11214
			public class MISSIONCONTROLCLUSTER
			{
				// Token: 0x0400C02A RID: 49194
				public static LocString NAME = UI.FormatAsLink("Mission Control Station", "MISSIONCONTROLCLUSTER");

				// Token: 0x0400C02B RID: 49195
				public static LocString DESC = "Like a backseat driver who actually does know better.";

				// Token: 0x0400C02C RID: 49196
				public static LocString EFFECT = "Provides guidance data to rocket pilots within range, to improve rocket speed.\n\nMust be operated by a Duplicant with the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nRequires a clear line of sight to space in order to function.";
			}

			// Token: 0x02002BCF RID: 11215
			public class SCULPTURE
			{
				// Token: 0x0400C02D RID: 49197
				public static LocString NAME = UI.FormatAsLink("Large Sculpting Block", "SCULPTURE");

				// Token: 0x0400C02E RID: 49198
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x0400C02F RID: 49199
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400C030 RID: 49200
				public static LocString POORQUALITYNAME = "\"Abstract\" Sculpture";

				// Token: 0x0400C031 RID: 49201
				public static LocString AVERAGEQUALITYNAME = "Mediocre Sculpture";

				// Token: 0x0400C032 RID: 49202
				public static LocString EXCELLENTQUALITYNAME = "Genius Sculpture";

				// Token: 0x02003A76 RID: 14966
				public class FACADES
				{
					// Token: 0x02003EDB RID: 16091
					public class SCULPTURE_GOOD_1
					{
						// Token: 0x0400F684 RID: 63108
						public static LocString NAME = UI.FormatAsLink("O Cupid, My Cupid", "SCULPTURE_GOOD_1");

						// Token: 0x0400F685 RID: 63109
						public static LocString DESC = "Ode to the bow and arrow, love's equivalent to a mining gun...but for hearts.";
					}

					// Token: 0x02003EDC RID: 16092
					public class SCULPTURE_CRAP_1
					{
						// Token: 0x0400F686 RID: 63110
						public static LocString NAME = UI.FormatAsLink("Inexplicable", "SCULPTURE_CRAP_1");

						// Token: 0x0400F687 RID: 63111
						public static LocString DESC = "A valiant attempt at art.";
					}

					// Token: 0x02003EDD RID: 16093
					public class SCULPTURE_AMAZING_2
					{
						// Token: 0x0400F688 RID: 63112
						public static LocString NAME = UI.FormatAsLink("Plate Chucker", "SCULPTURE_AMAZING_2");

						// Token: 0x0400F689 RID: 63113
						public static LocString DESC = "A masterful portrayal of an athlete who's been banned from the communal kitchen.";
					}

					// Token: 0x02003EDE RID: 16094
					public class SCULPTURE_AMAZING_3
					{
						// Token: 0x0400F68A RID: 63114
						public static LocString NAME = UI.FormatAsLink("Before Battle", "SCULPTURE_AMAZING_3");

						// Token: 0x0400F68B RID: 63115
						public static LocString DESC = "A masterful portrayal of a slingshot-wielding hero.";
					}

					// Token: 0x02003EDF RID: 16095
					public class SCULPTURE_AMAZING_4
					{
						// Token: 0x0400F68C RID: 63116
						public static LocString NAME = UI.FormatAsLink("Grandiose Grub-Grub", "SCULPTURE_AMAZING_4");

						// Token: 0x0400F68D RID: 63117
						public static LocString DESC = "A masterful portrayal of a gentle, plant-tending critter.";
					}

					// Token: 0x02003EE0 RID: 16096
					public class SCULPTURE_AMAZING_1
					{
						// Token: 0x0400F68E RID: 63118
						public static LocString NAME = UI.FormatAsLink("The Hypothesizer", "SCULPTURE_AMAZING_1");

						// Token: 0x0400F68F RID: 63119
						public static LocString DESC = "A masterful portrayal of a scientist lost in thought.";
					}

					// Token: 0x02003EE1 RID: 16097
					public class SCULPTURE_AMAZING_5
					{
						// Token: 0x0400F690 RID: 63120
						public static LocString NAME = UI.FormatAsLink("Vertical Cosmos", "SCULPTURE_AMAZING_5");

						// Token: 0x0400F691 RID: 63121
						public static LocString DESC = "It contains multitudes.";
					}

					// Token: 0x02003EE2 RID: 16098
					public class SCULPTURE_AMAZING_6
					{
						// Token: 0x0400F692 RID: 63122
						public static LocString NAME = UI.FormatAsLink("Into the Voids", "SCULPTURE_AMAZING_6");

						// Token: 0x0400F693 RID: 63123
						public static LocString DESC = "No amount of material success will ever fill the void within.";
					}
				}
			}

			// Token: 0x02002BD0 RID: 11216
			public class ICESCULPTURE
			{
				// Token: 0x0400C033 RID: 49203
				public static LocString NAME = UI.FormatAsLink("Ice Block", "ICESCULPTURE");

				// Token: 0x0400C034 RID: 49204
				public static LocString DESC = "Prone to melting.";

				// Token: 0x0400C035 RID: 49205
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400C036 RID: 49206
				public static LocString POORQUALITYNAME = "\"Abstract\" Ice Sculpture";

				// Token: 0x0400C037 RID: 49207
				public static LocString AVERAGEQUALITYNAME = "Mediocre Ice Sculpture";

				// Token: 0x0400C038 RID: 49208
				public static LocString EXCELLENTQUALITYNAME = "Genius Ice Sculpture";

				// Token: 0x02003A77 RID: 14967
				public class FACADES
				{
					// Token: 0x02003EE3 RID: 16099
					public class ICESCULPTURE_CRAP
					{
						// Token: 0x0400F694 RID: 63124
						public static LocString NAME = UI.FormatAsLink("Cubi I", "ICESCULPTURE_CRAP");

						// Token: 0x0400F695 RID: 63125
						public static LocString DESC = "It's structurally unsound, but otherwise not entirely terrible.";
					}

					// Token: 0x02003EE4 RID: 16100
					public class ICESCULPTURE_AMAZING_1
					{
						// Token: 0x0400F696 RID: 63126
						public static LocString NAME = UI.FormatAsLink("Exquisite Chompers", "ICESCULPTURE_AMAZING_1");

						// Token: 0x0400F697 RID: 63127
						public static LocString DESC = "These incisors are the stuff of dental legend.";
					}

					// Token: 0x02003EE5 RID: 16101
					public class ICESCULPTURE_AMAZING_2
					{
						// Token: 0x0400F698 RID: 63128
						public static LocString NAME = UI.FormatAsLink("Frosty Crustacean", "ICESCULPTURE_AMAZING_2");

						// Token: 0x0400F699 RID: 63129
						public static LocString DESC = "A charming depiction of the mighty Pokeshell in mid-rampage.";
					}

					// Token: 0x02003EE6 RID: 16102
					public class ICESCULPTURE_AMAZING_3
					{
						// Token: 0x0400F69A RID: 63130
						public static LocString NAME = UI.FormatAsLink("The Chase", "ICESCULPTURE_AMAZING_3");

						// Token: 0x0400F69B RID: 63131
						public static LocString DESC = "Some aquarists posit that Pacus are the original creators of the game now known as \"Tag.\"";
					}
				}
			}

			// Token: 0x02002BD1 RID: 11217
			public class MARBLESCULPTURE
			{
				// Token: 0x0400C039 RID: 49209
				public static LocString NAME = UI.FormatAsLink("Marble Block", "MARBLESCULPTURE");

				// Token: 0x0400C03A RID: 49210
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x0400C03B RID: 49211
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400C03C RID: 49212
				public static LocString POORQUALITYNAME = "\"Abstract\" Marble Sculpture";

				// Token: 0x0400C03D RID: 49213
				public static LocString AVERAGEQUALITYNAME = "Mediocre Marble Sculpture";

				// Token: 0x0400C03E RID: 49214
				public static LocString EXCELLENTQUALITYNAME = "Genius Marble Sculpture";

				// Token: 0x02003A78 RID: 14968
				public class FACADES
				{
					// Token: 0x02003EE7 RID: 16103
					public class SCULPTURE_MARBLE_CRAP_1
					{
						// Token: 0x0400F69C RID: 63132
						public static LocString NAME = UI.FormatAsLink("Lumpy Fungus", "SCULPTURE_MARBLE_CRAP_1");

						// Token: 0x0400F69D RID: 63133
						public static LocString DESC = "The artist was a very fungi.";
					}

					// Token: 0x02003EE8 RID: 16104
					public class SCULPTURE_MARBLE_GOOD_1
					{
						// Token: 0x0400F69E RID: 63134
						public static LocString NAME = UI.FormatAsLink("Unicorn Bust", "SCULPTURE_MARBLE_GOOD_1");

						// Token: 0x0400F69F RID: 63135
						public static LocString DESC = "It has real \"mane\" character energy.";
					}

					// Token: 0x02003EE9 RID: 16105
					public class SCULPTURE_MARBLE_AMAZING_1
					{
						// Token: 0x0400F6A0 RID: 63136
						public static LocString NAME = UI.FormatAsLink("The Large-ish Mermaid", "SCULPTURE_MARBLE_AMAZING_1");

						// Token: 0x0400F6A1 RID: 63137
						public static LocString DESC = "She's not afraid to take up space.";
					}

					// Token: 0x02003EEA RID: 16106
					public class SCULPTURE_MARBLE_AMAZING_2
					{
						// Token: 0x0400F6A2 RID: 63138
						public static LocString NAME = UI.FormatAsLink("Grouchy Beast", "SCULPTURE_MARBLE_AMAZING_2");

						// Token: 0x0400F6A3 RID: 63139
						public static LocString DESC = "The artist took great pleasure in conveying their displeasure.";
					}

					// Token: 0x02003EEB RID: 16107
					public class SCULPTURE_MARBLE_AMAZING_3
					{
						// Token: 0x0400F6A4 RID: 63140
						public static LocString NAME = UI.FormatAsLink("The Guardian", "SCULPTURE_MARBLE_AMAZING_3");

						// Token: 0x0400F6A5 RID: 63141
						public static LocString DESC = "Will not play fetch.";
					}

					// Token: 0x02003EEC RID: 16108
					public class SCULPTURE_MARBLE_AMAZING_4
					{
						// Token: 0x0400F6A6 RID: 63142
						public static LocString NAME = UI.FormatAsLink("Truly A-Moo-Zing", "SCULPTURE_MARBLE_AMAZING_4");

						// Token: 0x0400F6A7 RID: 63143
						public static LocString DESC = "A masterful celebration of one of the universe's most mysterious - and flatulent - organisms.";
					}

					// Token: 0x02003EED RID: 16109
					public class SCULPTURE_MARBLE_AMAZING_5
					{
						// Token: 0x0400F6A8 RID: 63144
						public static LocString NAME = UI.FormatAsLink("Green Goddess", "SCULPTURE_MARBLE_AMAZING_5");

						// Token: 0x0400F6A9 RID: 63145
						public static LocString DESC = "A masterful celebration of the deep bond between a horticulturalist and her prize Bristle Blossom.";
					}
				}
			}

			// Token: 0x02002BD2 RID: 11218
			public class METALSCULPTURE
			{
				// Token: 0x0400C03F RID: 49215
				public static LocString NAME = UI.FormatAsLink("Metal Block", "METALSCULPTURE");

				// Token: 0x0400C040 RID: 49216
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x0400C041 RID: 49217
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400C042 RID: 49218
				public static LocString POORQUALITYNAME = "\"Abstract\" Metal Sculpture";

				// Token: 0x0400C043 RID: 49219
				public static LocString AVERAGEQUALITYNAME = "Mediocre Metal Sculpture";

				// Token: 0x0400C044 RID: 49220
				public static LocString EXCELLENTQUALITYNAME = "Genius Metal Sculpture";

				// Token: 0x02003A79 RID: 14969
				public class FACADES
				{
					// Token: 0x02003EEE RID: 16110
					public class SCULPTURE_METAL_CRAP_1
					{
						// Token: 0x0400F6AA RID: 63146
						public static LocString NAME = UI.FormatAsLink("Unnatural Beauty", "SCULPTURE_METAL_CRAP_1");

						// Token: 0x0400F6AB RID: 63147
						public static LocString DESC = "Actually, it's a very good likeness.";
					}

					// Token: 0x02003EEF RID: 16111
					public class SCULPTURE_METAL_GOOD_1
					{
						// Token: 0x0400F6AC RID: 63148
						public static LocString NAME = UI.FormatAsLink("Beautiful Biohazard", "SCULPTURE_METAL_GOOD_1");

						// Token: 0x0400F6AD RID: 63149
						public static LocString DESC = "The Morb's eye is mounted on a swivel that activates at random intervals.";
					}

					// Token: 0x02003EF0 RID: 16112
					public class SCULPTURE_METAL_AMAZING_1
					{
						// Token: 0x0400F6AE RID: 63150
						public static LocString NAME = UI.FormatAsLink("Insatiable Appetite", "SCULPTURE_METAL_AMAZING_1");

						// Token: 0x0400F6AF RID: 63151
						public static LocString DESC = "It's quite lovely, until someone stubs their toe on it in the dark.";
					}

					// Token: 0x02003EF1 RID: 16113
					public class SCULPTURE_METAL_AMAZING_2
					{
						// Token: 0x0400F6B0 RID: 63152
						public static LocString NAME = UI.FormatAsLink("Agape", "SCULPTURE_METAL_AMAZING_2");

						// Token: 0x0400F6B1 RID: 63153
						public static LocString DESC = "Not quite expressionist, but undeniably expressive.";
					}

					// Token: 0x02003EF2 RID: 16114
					public class SCULPTURE_METAL_AMAZING_3
					{
						// Token: 0x0400F6B2 RID: 63154
						public static LocString NAME = UI.FormatAsLink("Friendly Flier", "SCULPTURE_METAL_AMAZING_3");

						// Token: 0x0400F6B3 RID: 63155
						public static LocString DESC = "It emits no light, but it sure does brighten up a room.";
					}

					// Token: 0x02003EF3 RID: 16115
					public class SCULPTURE_METAL_AMAZING_4
					{
						// Token: 0x0400F6B4 RID: 63156
						public static LocString NAME = UI.FormatAsLink("Whatta Pip", "SCULPTURE_METAL_AMAZING_4");

						// Token: 0x0400F6B5 RID: 63157
						public static LocString DESC = "A masterful likeness of the mischievous critter that Duplicants love to love.";
					}

					// Token: 0x02003EF4 RID: 16116
					public class SCULPTURE_METAL_AMAZING_5
					{
						// Token: 0x0400F6B6 RID: 63158
						public static LocString NAME = UI.FormatAsLink("Phrenologist's Dream", "SCULPTURE_METAL_AMAZING_5");

						// Token: 0x0400F6B7 RID: 63159
						public static LocString DESC = "What if the entire head is one big bump?";
					}
				}
			}

			// Token: 0x02002BD3 RID: 11219
			public class SMALLSCULPTURE
			{
				// Token: 0x0400C045 RID: 49221
				public static LocString NAME = UI.FormatAsLink("Sculpting Block", "SMALLSCULPTURE");

				// Token: 0x0400C046 RID: 49222
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x0400C047 RID: 49223
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Minorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400C048 RID: 49224
				public static LocString POORQUALITYNAME = "\"Abstract\" Sculpture";

				// Token: 0x0400C049 RID: 49225
				public static LocString AVERAGEQUALITYNAME = "Mediocre Sculpture";

				// Token: 0x0400C04A RID: 49226
				public static LocString EXCELLENTQUALITYNAME = "Genius Sculpture";

				// Token: 0x02003A7A RID: 14970
				public class FACADES
				{
					// Token: 0x02003EF5 RID: 16117
					public class SCULPTURE_1x2_GOOD
					{
						// Token: 0x0400F6B8 RID: 63160
						public static LocString NAME = UI.FormatAsLink("Lunar Slice", "SCULPTURE_1x2_GOOD");

						// Token: 0x0400F6B9 RID: 63161
						public static LocString DESC = "It must be a moon, because there are no bananas in space.";
					}

					// Token: 0x02003EF6 RID: 16118
					public class SCULPTURE_1x2_CRAP
					{
						// Token: 0x0400F6BA RID: 63162
						public static LocString NAME = UI.FormatAsLink("Unrequited", "SCULPTURE_1x2_CRAP");

						// Token: 0x0400F6BB RID: 63163
						public static LocString DESC = "It's a heavy heart.";
					}

					// Token: 0x02003EF7 RID: 16119
					public class SCULPTURE_1x2_AMAZING_1
					{
						// Token: 0x0400F6BC RID: 63164
						public static LocString NAME = UI.FormatAsLink("Not a Funnel", "SCULPTURE_1x2_AMAZING_1");

						// Token: 0x0400F6BD RID: 63165
						public static LocString DESC = "<i>Ceci n'est pas un entonnoir.</i>";
					}

					// Token: 0x02003EF8 RID: 16120
					public class SCULPTURE_1x2_AMAZING_2
					{
						// Token: 0x0400F6BE RID: 63166
						public static LocString NAME = UI.FormatAsLink("Equilibrium", "SCULPTURE_1x2_AMAZING_2");

						// Token: 0x0400F6BF RID: 63167
						public static LocString DESC = "Part of a well-balanced exhibit.";
					}

					// Token: 0x02003EF9 RID: 16121
					public class SCULPTURE_1x2_AMAZING_3
					{
						// Token: 0x0400F6C0 RID: 63168
						public static LocString NAME = UI.FormatAsLink("Opaque Orb", "SCULPTURE_1x2_AMAZING_3");

						// Token: 0x0400F6C1 RID: 63169
						public static LocString DESC = "It lacks transparency.";
					}

					// Token: 0x02003EFA RID: 16122
					public class SCULPTURE_1x2_AMAZING_4
					{
						// Token: 0x0400F6C2 RID: 63170
						public static LocString NAME = UI.FormatAsLink("Employee of the Month", "SCULPTURE_1x2_AMAZING_4");

						// Token: 0x0400F6C3 RID: 63171
						public static LocString DESC = "A masterful celebration of the Sweepy's unbeatable work ethic and cheerful, can-clean attitude.";
					}

					// Token: 0x02003EFB RID: 16123
					public class SCULPTURE_1x2_AMAZING_5
					{
						// Token: 0x0400F6C4 RID: 63172
						public static LocString NAME = UI.FormatAsLink("Pointy Impossibility", "SCULPTURE_1x2_AMAZING_5");

						// Token: 0x0400F6C5 RID: 63173
						public static LocString DESC = "A three-dimensional rebellion against the rules of Euclidean space.";
					}

					// Token: 0x02003EFC RID: 16124
					public class SCULPTURE_1x2_AMAZING_6
					{
						// Token: 0x0400F6C6 RID: 63174
						public static LocString NAME = UI.FormatAsLink("Fireball", "SCULPTURE_1x2_AMAZING_6");

						// Token: 0x0400F6C7 RID: 63175
						public static LocString DESC = "Tribute to the artist's friend, who once attempted to catch a meteor with their bare hands.";
					}
				}
			}

			// Token: 0x02002BD4 RID: 11220
			public class WOODSCULPTURE
			{
				// Token: 0x0400C04B RID: 49227
				public static LocString NAME = UI.FormatAsLink("Wood Block", "WOODSCULPTURE");

				// Token: 0x0400C04C RID: 49228
				public static LocString DESC = "A great fit for smaller spaces.";

				// Token: 0x0400C04D RID: 49229
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400C04E RID: 49230
				public static LocString POORQUALITYNAME = "\"Abstract\" Wood Sculpture";

				// Token: 0x0400C04F RID: 49231
				public static LocString AVERAGEQUALITYNAME = "Mediocre Wood Sculpture";

				// Token: 0x0400C050 RID: 49232
				public static LocString EXCELLENTQUALITYNAME = "Genius Wood Sculpture";
			}

			// Token: 0x02002BD5 RID: 11221
			public class SHEARINGSTATION
			{
				// Token: 0x0400C051 RID: 49233
				public static LocString NAME = UI.FormatAsLink("Shearing Station", "SHEARINGSTATION");

				// Token: 0x0400C052 RID: 49234
				public static LocString DESC = "Those critters aren't gonna shear themselves.";

				// Token: 0x0400C053 RID: 49235
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Shearing stations allow eligible ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" to be safely sheared for useful raw materials.\n\nVisiting this building restores ",
					UI.FormatAsLink("Critters'", "CREATURES"),
					" physical and emotional well-being."
				});
			}

			// Token: 0x02002BD6 RID: 11222
			public class OXYGENMASKSTATION
			{
				// Token: 0x0400C054 RID: 49236
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Station", "OXYGENMASKSTATION");

				// Token: 0x0400C055 RID: 49237
				public static LocString DESC = "Duplicants can't pass by a station if it lacks enough oxygen to fill a mask.";

				// Token: 0x0400C056 RID: 49238
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses designated ",
					UI.FormatAsLink("Metal Ores", "METAL"),
					" from filter settings to create ",
					UI.FormatAsLink("Oxygen Masks", "OXYGENMASK"),
					".\n\nAutomatically draws in ambient ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" to fill masks.\n\nMarks a threshold where Duplicants must put on or take off a mask.\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002BD7 RID: 11223
			public class SWEEPBOTSTATION
			{
				// Token: 0x0400C057 RID: 49239
				public static LocString NAME = UI.FormatAsLink("Sweepy's Dock", "SWEEPBOTSTATION");

				// Token: 0x0400C058 RID: 49240
				public static LocString NAMEDSTATION = "{0}'s Dock";

				// Token: 0x0400C059 RID: 49241
				public static LocString DESC = "The cute little face comes pre-installed.";

				// Token: 0x0400C05A RID: 49242
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Deploys an automated ",
					UI.FormatAsLink("Sweepy Bot", "SWEEPBOT"),
					" to sweep up ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					" debris and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" spills.\n\nDock stores ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" gathered by the Sweepy.\n\nUses ",
					UI.FormatAsLink("Power", "POWER"),
					" to recharge the Sweepy.\n\nDuplicants will empty Dock storage into available storage bins."
				});
			}

			// Token: 0x02002BD8 RID: 11224
			public class OXYGENMASKMARKER
			{
				// Token: 0x0400C05B RID: 49243
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Checkpoint", "OXYGENMASKMARKER");

				// Token: 0x0400C05C RID: 49244
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x0400C05D RID: 49245
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must put on or take off an ",
					UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK"),
					".\n\nMust be built next to an ",
					UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002BD9 RID: 11225
			public class OXYGENMASKLOCKER
			{
				// Token: 0x0400C05E RID: 49246
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER");

				// Token: 0x0400C05F RID: 49247
				public static LocString DESC = "An oxygen mask dock will store and refill masks while they're not in use.";

				// Token: 0x0400C060 RID: 49248
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Oxygen Masks", "OXYGEN_MASK"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nBuild next to an ",
					UI.FormatAsLink("Oxygen Mask Checkpoint", "OXYGENMASKMARKER"),
					" to make Duplicants put on masks when passing by."
				});
			}

			// Token: 0x02002BDA RID: 11226
			public class SUITMARKER
			{
				// Token: 0x0400C061 RID: 49249
				public static LocString NAME = UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER");

				// Token: 0x0400C062 RID: 49250
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x0400C063 RID: 49251
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					".\n\nMust be built next to an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002BDB RID: 11227
			public class SUITLOCKER
			{
				// Token: 0x0400C064 RID: 49252
				public static LocString NAME = UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER");

				// Token: 0x0400C065 RID: 49253
				public static LocString DESC = "An atmo suit dock will empty atmo suits of waste, but only one suit can charge at a time.";

				// Token: 0x0400C066 RID: 49254
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to an ",
					UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			// Token: 0x02002BDC RID: 11228
			public class JETSUITMARKER
			{
				// Token: 0x0400C067 RID: 49255
				public static LocString NAME = UI.FormatAsLink("Jet Suit Checkpoint", "JETSUITMARKER");

				// Token: 0x0400C068 RID: 49256
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x0400C069 RID: 49257
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Jet Suits", "JET_SUIT"),
					".\n\nMust be built next to a ",
					UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002BDD RID: 11229
			public class JETSUITLOCKER
			{
				// Token: 0x0400C06A RID: 49258
				public static LocString NAME = UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER");

				// Token: 0x0400C06B RID: 49259
				public static LocString DESC = "Jet suit docks can refill jet suits with air and fuel, or empty them of waste.";

				// Token: 0x0400C06C RID: 49260
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Jet Suits", "JET_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					MISC.TAGS.COMBUSTIBLELIQUID,
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to a ",
					UI.FormatAsLink("Jet Suit Checkpoint", "JETSUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			// Token: 0x02002BDE RID: 11230
			public class LEADSUITMARKER
			{
				// Token: 0x0400C06D RID: 49261
				public static LocString NAME = UI.FormatAsLink("Lead Suit Checkpoint", "LEADSUITMARKER");

				// Token: 0x0400C06E RID: 49262
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x0400C06F RID: 49263
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Lead Suits", "LEAD_SUIT"),
					".\n\nMust be built next to a ",
					UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER"),
					"\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002BDF RID: 11231
			public class LEADSUITLOCKER
			{
				// Token: 0x0400C070 RID: 49264
				public static LocString NAME = UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER");

				// Token: 0x0400C071 RID: 49265
				public static LocString DESC = "Lead suit docks can refill lead suits with air and empty them of waste.";

				// Token: 0x0400C072 RID: 49266
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Lead Suits", "LEAD_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to a ",
					UI.FormatAsLink("Lead Suit Checkpoint", "LEADSUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			// Token: 0x02002BE0 RID: 11232
			public class CRAFTINGTABLE
			{
				// Token: 0x0400C073 RID: 49267
				public static LocString NAME = UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE");

				// Token: 0x0400C074 RID: 49268
				public static LocString DESC = "Crafting stations allow Duplicants to make oxygen masks to wear in low breathability areas.";

				// Token: 0x0400C075 RID: 49269
				public static LocString EFFECT = "Produces items and equipment for Duplicant use.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0400C076 RID: 49270
				public static LocString RECIPE_DESCRIPTION = "Converts {0} to {1}";
			}

			// Token: 0x02002BE1 RID: 11233
			public class ADVANCEDCRAFTINGTABLE
			{
				// Token: 0x0400C077 RID: 49271
				public static LocString NAME = UI.FormatAsLink("Soldering Station", "ADVANCEDCRAFTINGTABLE");

				// Token: 0x0400C078 RID: 49272
				public static LocString DESC = "Soldering stations allow Duplicants to build helpful Flydo retriever bots.";

				// Token: 0x0400C079 RID: 49273
				public static LocString EFFECT = "Produces advanced electronics and bionic " + UI.FormatAsLink("Boosters", "BIONIC_UPGRADE") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0400C07A RID: 49274
				public static LocString BIONIC_COMPONENT_RECIPE_DESC = "Converts {0} to {1}";

				// Token: 0x0400C07B RID: 49275
				public static LocString GENERIC_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				// Token: 0x0400C07C RID: 49276
				public static LocString COLONY_HAS_BOOSTER_ASSIGNED_NONE = "My colony has no Bionic Duplicants with this booster assigned";

				// Token: 0x0400C07D RID: 49277
				public static LocString COLONY_HAS_BOOSTER_ASSIGNED_COUNT = "My colony has {0} Bionic Duplicant(s) with this booster assigned";
			}

			// Token: 0x02002BE2 RID: 11234
			public class DATAMINER
			{
				// Token: 0x0400C07E RID: 49278
				public static LocString NAME = UI.FormatAsLink("Data Miner", "DATAMINER");

				// Token: 0x0400C07F RID: 49279
				public static LocString DESC = "Data banks can also be used to program robo-pilot rocket modules.";

				// Token: 0x0400C080 RID: 49280
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Mass-produces ",
					UI.FormatAsLink(DatabankHelper.NAME_PLURAL, "Databank"),
					" that can be processed into ",
					UI.FormatAsLink(DatabankHelper.RESEARCH_NAME, DatabankHelper.RESEARCH_CODEXID),
					" points.\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x0400C081 RID: 49281
				public static LocString RECIPE_DESCRIPTION = "Turns {0} into {1}.";
			}

			// Token: 0x02002BE3 RID: 11235
			public class REMOTEWORKTERMINAL
			{
				// Token: 0x0400C082 RID: 49282
				public static LocString NAME = UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL");

				// Token: 0x0400C083 RID: 49283
				public static LocString DESC = "Remote controllers cut down on colony commute times.";

				// Token: 0x0400C084 RID: 49284
				public static LocString EFFECT = "Enables Duplicants to operate machinery remotely via a connected " + UI.FormatAsLink("Remote Worker Dock", "REMOTEWORKERDOCK") + ".";
			}

			// Token: 0x02002BE4 RID: 11236
			public class REMOTEWORKERDOCK
			{
				// Token: 0x0400C085 RID: 49285
				public static LocString NAME = UI.FormatAsLink("Remote Worker Dock", "REMOTEWORKERDOCK");

				// Token: 0x0400C086 RID: 49286
				public static LocString NAME_FMT = "Dock {ID}";

				// Token: 0x0400C087 RID: 49287
				public static LocString DESC = "It's a Duplicant's duplicate's dock.";

				// Token: 0x0400C088 RID: 49288
				public static LocString EFFECT = UI.FormatAsLink("Remote Worker Docks", "REMOTEWORKERDOCK") + " deploy automatons that operate machinery based on instructions received from a connected " + UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL") + ".\n\nMust be placed within range of its target building.";
			}

			// Token: 0x02002BE5 RID: 11237
			public class SUITFABRICATOR
			{
				// Token: 0x0400C089 RID: 49289
				public static LocString NAME = UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR");

				// Token: 0x0400C08A RID: 49290
				public static LocString DESC = "Exosuits can be filled with oxygen to allow Duplicants to safely enter hazardous areas.";

				// Token: 0x0400C08B RID: 49291
				public static LocString EFFECT = "Forges protective " + UI.FormatAsLink("Exosuits", "EQUIPMENT") + " for Duplicants to wear.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002BE6 RID: 11238
			public class CLOTHINGALTERATIONSTATION
			{
				// Token: 0x0400C08C RID: 49292
				public static LocString NAME = UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION");

				// Token: 0x0400C08D RID: 49293
				public static LocString DESC = "Allows skilled Duplicants to add extra personal pizzazz to their wardrobe.";

				// Token: 0x0400C08E RID: 49294
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Upgrades ",
					UI.FormatAsLink("Snazzy Suits", "FUNKY_VEST"),
					" into ",
					UI.FormatAsLink("Primo Garb", "CUSTOM_CLOTHING"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x02002BE7 RID: 11239
			public class CLOTHINGFABRICATOR
			{
				// Token: 0x0400C08F RID: 49295
				public static LocString NAME = UI.FormatAsLink("Textile Loom", "CLOTHINGFABRICATOR");

				// Token: 0x0400C090 RID: 49296
				public static LocString DESC = "A textile loom can be used to spin Reed Fiber into wearable Duplicant clothing.";

				// Token: 0x0400C091 RID: 49297
				public static LocString EFFECT = "Tailors Duplicant " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " items.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002BE8 RID: 11240
			public class SOLIDBOOSTER
			{
				// Token: 0x0400C092 RID: 49298
				public static LocString NAME = UI.FormatAsLink("Solid Fuel Thruster", "SOLIDBOOSTER");

				// Token: 0x0400C093 RID: 49299
				public static LocString DESC = "Additional thrusters allow rockets to reach far away space destinations.";

				// Token: 0x0400C094 RID: 49300
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Iron", "IRON"),
					" and ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" to increase rocket exploration distance."
				});
			}

			// Token: 0x02002BE9 RID: 11241
			public class SPACEHEATER
			{
				// Token: 0x0400C095 RID: 49301
				public static LocString NAME = UI.FormatAsLink("Space Heater", "SPACEHEATER");

				// Token: 0x0400C096 RID: 49302
				public static LocString DESC = "Space heaters are a welcome cure for cold, soggy feet.";

				// Token: 0x0400C097 RID: 49303
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Radiates a moderate amount of ",
					UI.FormatAsLink("Heat", "HEAT"),
					".\n\nRequires ",
					UI.FormatAsLink("Power", "POWER"),
					" in order to function."
				});
			}

			// Token: 0x02002BEA RID: 11242
			public class SPICEGRINDER
			{
				// Token: 0x0400C098 RID: 49304
				public static LocString NAME = UI.FormatAsLink("Spice Grinder", "SPICEGRINDER");

				// Token: 0x0400C099 RID: 49305
				public static LocString DESC = "Crushed seeds and other edibles make excellent meal-enhancing additives.";

				// Token: 0x0400C09A RID: 49306
				public static LocString EFFECT = "Produces ingredients that add benefits to " + UI.FormatAsLink("foods", "FOOD") + " prepared at skilled cooking stations.";

				// Token: 0x0400C09B RID: 49307
				public static LocString INGREDIENTHEADER = "Ingredients per 1000kcal:";
			}

			// Token: 0x02002BEB RID: 11243
			public class SMOKER
			{
				// Token: 0x0400C09C RID: 49308
				public static LocString NAME = UI.FormatAsLink("Smoker", "SMOKER");

				// Token: 0x0400C09D RID: 49309
				public static LocString DESC = "With a little patience, even tough meat can become deliciously edible.";

				// Token: 0x0400C09E RID: 49310
				public static LocString EFFECT = "Cooks improved " + UI.FormatAsLink("foods", "FOOD") + " over low, slow heat.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002BEC RID: 11244
			public class STORAGELOCKER
			{
				// Token: 0x0400C09F RID: 49311
				public static LocString NAME = UI.FormatAsLink("Storage Bin", "STORAGELOCKER");

				// Token: 0x0400C0A0 RID: 49312
				public static LocString DESC = "Resources left on the floor become \"debris\" and lower decor when not put away.";

				// Token: 0x0400C0A1 RID: 49313
				public static LocString EFFECT = "Stores the " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " of your choosing.";

				// Token: 0x02003A7B RID: 14971
				public class FACADES
				{
					// Token: 0x02003EFD RID: 16125
					public class DEFAULT_STORAGELOCKER
					{
						// Token: 0x0400F6C8 RID: 63176
						public static LocString NAME = UI.FormatAsLink("Storage Bin", "STORAGELOCKER");

						// Token: 0x0400F6C9 RID: 63177
						public static LocString DESC = "Resources left on the floor become \"debris\" and lower decor when not put away.";
					}

					// Token: 0x02003EFE RID: 16126
					public class GREEN_MUSH
					{
						// Token: 0x0400F6CA RID: 63178
						public static LocString NAME = UI.FormatAsLink("Mush Green Storage Bin", "STORAGELOCKER");

						// Token: 0x0400F6CB RID: 63179
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003EFF RID: 16127
					public class RED_ROSE
					{
						// Token: 0x0400F6CC RID: 63180
						public static LocString NAME = UI.FormatAsLink("Puce Pink Storage Bin", "STORAGELOCKER");

						// Token: 0x0400F6CD RID: 63181
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003F00 RID: 16128
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400F6CE RID: 63182
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Storage Bin", "STORAGELOCKER");

						// Token: 0x0400F6CF RID: 63183
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003F01 RID: 16129
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400F6D0 RID: 63184
						public static LocString NAME = UI.FormatAsLink("Faint Purple Storage Bin", "STORAGELOCKER");

						// Token: 0x0400F6D1 RID: 63185
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003F02 RID: 16130
					public class YELLOW_TARTAR
					{
						// Token: 0x0400F6D2 RID: 63186
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Storage Bin", "STORAGELOCKER");

						// Token: 0x0400F6D3 RID: 63187
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003F03 RID: 16131
					public class POLKA_DARKNAVYNOOKGREEN
					{
						// Token: 0x0400F6D4 RID: 63188
						public static LocString NAME = UI.FormatAsLink("Party Dot Storage Bin", "STORAGELOCKER");

						// Token: 0x0400F6D5 RID: 63189
						public static LocString DESC = "A fun storage solution for fun-damental materials.";
					}

					// Token: 0x02003F04 RID: 16132
					public class POLKA_DARKPURPLERESIN
					{
						// Token: 0x0400F6D6 RID: 63190
						public static LocString NAME = UI.FormatAsLink("Mod Dot Storage Bin", "STORAGELOCKER");

						// Token: 0x0400F6D7 RID: 63191
						public static LocString DESC = "Groovy storage, because messy colonies are such a drag.";
					}

					// Token: 0x02003F05 RID: 16133
					public class STRIPES_RED_WHITE
					{
						// Token: 0x0400F6D8 RID: 63192
						public static LocString NAME = "Bold Stripe Storage Bin";

						// Token: 0x0400F6D9 RID: 63193
						public static LocString DESC = "It's the merriest storage bin of all.";
					}
				}
			}

			// Token: 0x02002BED RID: 11245
			public class STORAGELOCKERSMART
			{
				// Token: 0x0400C0A2 RID: 49314
				public static LocString NAME = UI.FormatAsLink("Smart Storage Bin", "STORAGELOCKERSMART");

				// Token: 0x0400C0A3 RID: 49315
				public static LocString DESC = "Smart storage bins can automate resource organization based on type and mass.";

				// Token: 0x0400C0A4 RID: 49316
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" of your choosing.\n\nSends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when bin is full."
				});

				// Token: 0x0400C0A5 RID: 49317
				public static LocString LOGIC_PORT = "Full/Not Full";

				// Token: 0x0400C0A6 RID: 49318
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full";

				// Token: 0x0400C0A7 RID: 49319
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002BEE RID: 11246
			public class OBJECTDISPENSER
			{
				// Token: 0x0400C0A8 RID: 49320
				public static LocString NAME = UI.FormatAsLink("Automatic Dispenser", "OBJECTDISPENSER");

				// Token: 0x0400C0A9 RID: 49321
				public static LocString DESC = "Automatic dispensers will store and drop resources in small quantities.";

				// Token: 0x0400C0AA RID: 49322
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores any ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" delivered to it by Duplicants.\n\nDumps stored materials back into the world when it receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				// Token: 0x0400C0AB RID: 49323
				public static LocString LOGIC_PORT = "Dump Trigger";

				// Token: 0x0400C0AC RID: 49324
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Dump all stored materials";

				// Token: 0x0400C0AD RID: 49325
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Store materials";
			}

			// Token: 0x02002BEF RID: 11247
			public class LIQUIDRESERVOIR
			{
				// Token: 0x0400C0AE RID: 49326
				public static LocString NAME = UI.FormatAsLink("Liquid Reservoir", "LIQUIDRESERVOIR");

				// Token: 0x0400C0AF RID: 49327
				public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";

				// Token: 0x0400C0B0 RID: 49328
				public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources piped into it.";
			}

			// Token: 0x02002BF0 RID: 11248
			public class GASRESERVOIR
			{
				// Token: 0x0400C0B1 RID: 49329
				public static LocString NAME = UI.FormatAsLink("Gas Reservoir", "GASRESERVOIR");

				// Token: 0x0400C0B2 RID: 49330
				public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";

				// Token: 0x0400C0B3 RID: 49331
				public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources piped into it.";

				// Token: 0x02003A7C RID: 14972
				public class FACADES
				{
					// Token: 0x02003F06 RID: 16134
					public class DEFAULT_GASRESERVOIR
					{
						// Token: 0x0400F6DA RID: 63194
						public static LocString NAME = UI.FormatAsLink("Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6DB RID: 63195
						public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";
					}

					// Token: 0x02003F07 RID: 16135
					public class LIGHTGOLD
					{
						// Token: 0x0400F6DC RID: 63196
						public static LocString NAME = UI.FormatAsLink("Golden Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6DD RID: 63197
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003F08 RID: 16136
					public class PEAGREEN
					{
						// Token: 0x0400F6DE RID: 63198
						public static LocString NAME = UI.FormatAsLink("Greenpea Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6DF RID: 63199
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003F09 RID: 16137
					public class LIGHTCOBALT
					{
						// Token: 0x0400F6E0 RID: 63200
						public static LocString NAME = UI.FormatAsLink("Bluemoon Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6E1 RID: 63201
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003F0A RID: 16138
					public class POLKA_DARKPURPLERESIN
					{
						// Token: 0x0400F6E2 RID: 63202
						public static LocString NAME = UI.FormatAsLink("Mod Dot Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6E3 RID: 63203
						public static LocString DESC = "It sports the cheeriest of paint jobs. What a gas!";
					}

					// Token: 0x02003F0B RID: 16139
					public class POLKA_DARKNAVYNOOKGREEN
					{
						// Token: 0x0400F6E4 RID: 63204
						public static LocString NAME = UI.FormatAsLink("Party Dot Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6E5 RID: 63205
						public static LocString DESC = "Safe gas storage doesn't have to be dull.";
					}

					// Token: 0x02003F0C RID: 16140
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400F6E6 RID: 63206
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6E7 RID: 63207
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003F0D RID: 16141
					public class YELLOW_TARTAR
					{
						// Token: 0x0400F6E8 RID: 63208
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6E9 RID: 63209
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003F0E RID: 16142
					public class GREEN_MUSH
					{
						// Token: 0x0400F6EA RID: 63210
						public static LocString NAME = UI.FormatAsLink("Mush Green Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6EB RID: 63211
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003F0F RID: 16143
					public class RED_ROSE
					{
						// Token: 0x0400F6EC RID: 63212
						public static LocString NAME = UI.FormatAsLink("Puce Pink Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6ED RID: 63213
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003F10 RID: 16144
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400F6EE RID: 63214
						public static LocString NAME = UI.FormatAsLink("Faint Purple Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400F6EF RID: 63215
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}
				}
			}

			// Token: 0x02002BF1 RID: 11249
			public class SMARTRESERVOIR
			{
				// Token: 0x0400C0B4 RID: 49332
				public static LocString LOGIC_PORT = "Refill Parameters";

				// Token: 0x0400C0B5 RID: 49333
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when reservoir is less than <b>Low Threshold</b> full, until <b>High Threshold</b> is reached again";

				// Token: 0x0400C0B6 RID: 49334
				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when reservoir is <b>High Threshold</b> full, until <b>Low Threshold</b> is reached again";

				// Token: 0x0400C0B7 RID: 49335
				public static LocString ACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when reservoir is less than <b>{0}%</b> full, until it is <b>{1}% (High Threshold)</b> full";

				// Token: 0x0400C0B8 RID: 49336
				public static LocString DEACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when reservoir is <b>{0}%</b> full, until it is less than <b>{1}% (Low Threshold)</b> full";

				// Token: 0x0400C0B9 RID: 49337
				public static LocString SIDESCREEN_TITLE = "Logic Activation Parameters";

				// Token: 0x0400C0BA RID: 49338
				public static LocString SIDESCREEN_ACTIVATE = "Low Threshold:";

				// Token: 0x0400C0BB RID: 49339
				public static LocString SIDESCREEN_DEACTIVATE = "High Threshold:";
			}

			// Token: 0x02002BF2 RID: 11250
			public class LIQUIDHEATER
			{
				// Token: 0x0400C0BC RID: 49340
				public static LocString NAME = UI.FormatAsLink("Liquid Tepidizer", "LIQUIDHEATER");

				// Token: 0x0400C0BD RID: 49341
				public static LocString DESC = "Tepidizers heat liquid which can kill waterborne germs.";

				// Token: 0x0400C0BE RID: 49342
				public static LocString EFFECT = "Warms large bodies of " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nMust be fully submerged.";
			}

			// Token: 0x02002BF3 RID: 11251
			public class SWITCH
			{
				// Token: 0x0400C0BF RID: 49343
				public static LocString NAME = UI.FormatAsLink("Switch", "SWITCH");

				// Token: 0x0400C0C0 RID: 49344
				public static LocString DESC = "Switches can only affect buildings that come after them on a circuit.";

				// Token: 0x0400C0C1 RID: 49345
				public static LocString EFFECT = "Turns " + UI.FormatAsLink("Power", "POWER") + " on or off.\n\nDoes not affect circuitry preceding the switch.";

				// Token: 0x0400C0C2 RID: 49346
				public static LocString SIDESCREEN_TITLE = "Switch";

				// Token: 0x0400C0C3 RID: 49347
				public static LocString TURN_ON = "Turn On";

				// Token: 0x0400C0C4 RID: 49348
				public static LocString TURN_ON_TOOLTIP = "Turn On {Hotkey}";

				// Token: 0x0400C0C5 RID: 49349
				public static LocString TURN_OFF = "Turn Off";

				// Token: 0x0400C0C6 RID: 49350
				public static LocString TURN_OFF_TOOLTIP = "Turn Off {Hotkey}";
			}

			// Token: 0x02002BF4 RID: 11252
			public class LOGICPOWERRELAY
			{
				// Token: 0x0400C0C7 RID: 49351
				public static LocString NAME = UI.FormatAsLink("Power Shutoff", "LOGICPOWERRELAY");

				// Token: 0x0400C0C8 RID: 49352
				public static LocString DESC = "Automated systems save power and time by removing the need for Duplicant input.";

				// Token: 0x0400C0C9 RID: 49353
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off.\n\nDoes not affect circuitry preceding the switch."
				});

				// Token: 0x0400C0CA RID: 49354
				public static LocString LOGIC_PORT = "Kill Power";

				// Token: 0x0400C0CB RID: 49355
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Allow ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" through connected circuits"
				});

				// Token: 0x0400C0CC RID: 49356
				public static LocString LOGIC_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Prevent ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from flowing through connected circuits"
				});
			}

			// Token: 0x02002BF5 RID: 11253
			public class LOGICINTERASTEROIDSENDER
			{
				// Token: 0x0400C0CD RID: 49357
				public static LocString NAME = UI.FormatAsLink("Automation Broadcaster", "LOGICINTERASTEROIDSENDER");

				// Token: 0x0400C0CE RID: 49358
				public static LocString DESC = "Sends automation signals into space.";

				// Token: 0x0400C0CF RID: 49359
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to an ",
					UI.FormatAsLink("Automation Receiver", "LOGICINTERASTEROIDRECEIVER"),
					" over vast distances in space.\n\nBoth the Automation Broadcaster and the Automation Receiver must be exposed to space to function."
				});

				// Token: 0x0400C0D0 RID: 49360
				public static LocString DEFAULTNAME = "Unnamed Broadcaster";

				// Token: 0x0400C0D1 RID: 49361
				public static LocString LOGIC_PORT = "Broadcasting Signal";

				// Token: 0x0400C0D2 RID: 49362
				public static LocString LOGIC_PORT_ACTIVE = "Broadcasting: " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400C0D3 RID: 49363
				public static LocString LOGIC_PORT_INACTIVE = "Broadcasting: " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002BF6 RID: 11254
			public class LOGICINTERASTEROIDRECEIVER
			{
				// Token: 0x0400C0D4 RID: 49364
				public static LocString NAME = UI.FormatAsLink("Automation Receiver", "LOGICINTERASTEROIDRECEIVER");

				// Token: 0x0400C0D5 RID: 49365
				public static LocString DESC = "Receives automation signals from space.";

				// Token: 0x0400C0D6 RID: 49366
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" from an ",
					UI.FormatAsLink("Automation Broadcaster", "LOGICINTERASTEROIDSENDER"),
					" over vast distances in space.\n\nBoth the Automation Receiver and the Automation Broadcaster must be exposed to space to function."
				});

				// Token: 0x0400C0D7 RID: 49367
				public static LocString LOGIC_PORT = "Receiving Signal";

				// Token: 0x0400C0D8 RID: 49368
				public static LocString LOGIC_PORT_ACTIVE = "Receiving: " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400C0D9 RID: 49369
				public static LocString LOGIC_PORT_INACTIVE = "Receiving: " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002BF7 RID: 11255
			public class TEMPERATURECONTROLLEDSWITCH
			{
				// Token: 0x0400C0DA RID: 49370
				public static LocString NAME = UI.FormatAsLink("Thermo Switch", "TEMPERATURECONTROLLEDSWITCH");

				// Token: 0x0400C0DB RID: 49371
				public static LocString DESC = "Automated switches can be used to manage circuits in areas where Duplicants cannot enter.";

				// Token: 0x0400C0DC RID: 49372
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nDoes not affect circuitry preceding the switch."
				});
			}

			// Token: 0x02002BF8 RID: 11256
			public class PRESSURESWITCHLIQUID
			{
				// Token: 0x0400C0DD RID: 49373
				public static LocString NAME = UI.FormatAsLink("Hydro Switch", "PRESSURESWITCHLIQUID");

				// Token: 0x0400C0DE RID: 49374
				public static LocString DESC = "A hydro switch shuts off power when the liquid pressure surrounding it surpasses the set threshold.";

				// Token: 0x0400C0DF RID: 49375
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Liquid Pressure", "PRESSURE"),
					".\n\nDoes not affect circuitry preceding the switch.\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002BF9 RID: 11257
			public class PRESSURESWITCHGAS
			{
				// Token: 0x0400C0E0 RID: 49376
				public static LocString NAME = UI.FormatAsLink("Atmo Switch", "PRESSURESWITCHGAS");

				// Token: 0x0400C0E1 RID: 49377
				public static LocString DESC = "An atmo switch shuts off power when the air pressure surrounding it surpasses the set threshold.";

				// Token: 0x0400C0E2 RID: 49378
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Gas Pressure", "PRESSURE"),
					" .\n\nDoes not affect circuitry preceding the switch."
				});
			}

			// Token: 0x02002BFA RID: 11258
			public class TILE
			{
				// Token: 0x0400C0E3 RID: 49379
				public static LocString NAME = UI.FormatAsLink("Tile", "TILE");

				// Token: 0x0400C0E4 RID: 49380
				public static LocString DESC = "Tile can be used to bridge gaps and get to unreachable areas.";

				// Token: 0x0400C0E5 RID: 49381
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nIncreases Duplicant runspeed.";
			}

			// Token: 0x02002BFB RID: 11259
			public class WALLTOILET
			{
				// Token: 0x0400C0E6 RID: 49382
				public static LocString NAME = UI.FormatAsLink("Wall Toilet", "WALLTOILET");

				// Token: 0x0400C0E7 RID: 49383
				public static LocString DESC = "Wall Toilets transmit fewer germs to Duplicants and require no emptying.";

				// Token: 0x0400C0E8 RID: 49384
				public static LocString EFFECT = "Gives Duplicants a place to relieve themselves. Empties directly on the other side of the wall.\n\nSpreads very few " + UI.FormatAsLink("Germs", "DISEASE") + ".";
			}

			// Token: 0x02002BFC RID: 11260
			public class WATERPURIFIER
			{
				// Token: 0x0400C0E9 RID: 49385
				public static LocString NAME = UI.FormatAsLink("Water Sieve", "WATERPURIFIER");

				// Token: 0x0400C0EA RID: 49386
				public static LocString DESC = "Sieves cannot kill germs and pass any they receive into their waste and water output.";

				// Token: 0x0400C0EB RID: 49387
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces clean ",
					UI.FormatAsLink("Water", "WATER"),
					" from ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" using ",
					UI.FormatAsLink("Sand", "SAND"),
					".\n\nProduces ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			// Token: 0x02002BFD RID: 11261
			public class DISTILLATIONCOLUMN
			{
				// Token: 0x0400C0EC RID: 49388
				public static LocString NAME = UI.FormatAsLink("Distillation Column", "DISTILLATIONCOLUMN");

				// Token: 0x0400C0ED RID: 49389
				public static LocString DESC = "Gets hot and steamy.";

				// Token: 0x0400C0EE RID: 49390
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Separates any ",
					UI.FormatAsLink("Contaminated Water", "DIRTYWATER"),
					" piped through it into ",
					UI.FormatAsLink("Steam", "STEAM"),
					" and ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			// Token: 0x02002BFE RID: 11262
			public class WIRE
			{
				// Token: 0x0400C0EF RID: 49391
				public static LocString NAME = UI.FormatAsLink("Wire", "WIRE");

				// Token: 0x0400C0F0 RID: 49392
				public static LocString DESC = "Electrical wire is used to connect generators, batteries, and buildings.";

				// Token: 0x0400C0F1 RID: 49393
				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002BFF RID: 11263
			public class WIREBRIDGE
			{
				// Token: 0x0400C0F2 RID: 49394
				public static LocString NAME = UI.FormatAsLink("Wire Bridge", "WIREBRIDGE");

				// Token: 0x0400C0F3 RID: 49395
				public static LocString DESC = "Splitting generators onto separate grids can prevent overloads and wasted electricity.";

				// Token: 0x0400C0F4 RID: 49396
				public static LocString EFFECT = "Runs one wire section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002C00 RID: 11264
			public class HIGHWATTAGEWIRE
			{
				// Token: 0x0400C0F5 RID: 49397
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE");

				// Token: 0x0400C0F6 RID: 49398
				public static LocString DESC = "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";

				// Token: 0x0400C0F7 RID: 49399
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than regular ",
					UI.FormatAsLink("Wire", "WIRE"),
					" without overloading.\n\nCannot be run through wall and floor tile."
				});
			}

			// Token: 0x02002C01 RID: 11265
			public class WIREBRIDGEHIGHWATTAGE
			{
				// Token: 0x0400C0F8 RID: 49400
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Joint Plate", "WIREBRIDGEHIGHWATTAGE");

				// Token: 0x0400C0F9 RID: 49401
				public static LocString DESC = "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";

				// Token: 0x0400C0FA RID: 49402
				public static LocString EFFECT = "Allows " + UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.";
			}

			// Token: 0x02002C02 RID: 11266
			public class WIREREFINED
			{
				// Token: 0x0400C0FB RID: 49403
				public static LocString NAME = UI.FormatAsLink("Conductive Wire", "WIREREFINED");

				// Token: 0x0400C0FC RID: 49404
				public static LocString DESC = "My Duplicants prefer the look of conductive wire to the regular raggedy stuff.";

				// Token: 0x0400C0FD RID: 49405
				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002C03 RID: 11267
			public class WIREREFINEDBRIDGE
			{
				// Token: 0x0400C0FE RID: 49406
				public static LocString NAME = UI.FormatAsLink("Conductive Wire Bridge", "WIREREFINEDBRIDGE");

				// Token: 0x0400C0FF RID: 49407
				public static LocString DESC = "Splitting generators onto separate systems can prevent overloads and wasted electricity.";

				// Token: 0x0400C100 RID: 49408
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than a regular ",
					UI.FormatAsLink("Wire Bridge", "WIREBRIDGE"),
					" without overloading.\n\nRuns one wire section over another without joining them.\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002C04 RID: 11268
			public class WIREREFINEDHIGHWATTAGE
			{
				// Token: 0x0400C101 RID: 49409
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Conductive Wire", "WIREREFINEDHIGHWATTAGE");

				// Token: 0x0400C102 RID: 49410
				public static LocString DESC = "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";

				// Token: 0x0400C103 RID: 49411
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than regular ",
					UI.FormatAsLink("Wire", "WIRE"),
					" without overloading.\n\nCannot be run through wall and floor tile."
				});
			}

			// Token: 0x02002C05 RID: 11269
			public class WIREREFINEDBRIDGEHIGHWATTAGE
			{
				// Token: 0x0400C104 RID: 49412
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Conductive Joint Plate", "WIREREFINEDBRIDGEHIGHWATTAGE");

				// Token: 0x0400C105 RID: 49413
				public static LocString DESC = "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";

				// Token: 0x0400C106 RID: 49414
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than a regular ",
					UI.FormatAsLink("Heavi-Watt Joint Plate", "WIREBRIDGEHIGHWATTAGE"),
					" without overloading.\n\nAllows ",
					UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE"),
					" to be run through wall and floor tile."
				});
			}

			// Token: 0x02002C06 RID: 11270
			public class HANDSANITIZER
			{
				// Token: 0x0400C107 RID: 49415
				public static LocString NAME = UI.FormatAsLink("Hand Sanitizer", "HANDSANITIZER");

				// Token: 0x0400C108 RID: 49416
				public static LocString DESC = "Hand sanitizers kill germs more effectively than wash basins.";

				// Token: 0x0400C109 RID: 49417
				public static LocString EFFECT = "Removes most " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Hand Sanitizers when passing by in the selected direction.";
			}

			// Token: 0x02002C07 RID: 11271
			public class WASHBASIN
			{
				// Token: 0x0400C10A RID: 49418
				public static LocString NAME = UI.FormatAsLink("Wash Basin", "WASHBASIN");

				// Token: 0x0400C10B RID: 49419
				public static LocString DESC = "Germ spread can be reduced by building these where Duplicants often get dirty.";

				// Token: 0x0400C10C RID: 49420
				public static LocString EFFECT = "Removes some " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Wash Basins when passing by in the selected direction.";
			}

			// Token: 0x02002C08 RID: 11272
			public class WASHSINK
			{
				// Token: 0x0400C10D RID: 49421
				public static LocString NAME = UI.FormatAsLink("Sink", "WASHSINK");

				// Token: 0x0400C10E RID: 49422
				public static LocString DESC = "Sinks are plumbed and do not need to be manually emptied or refilled.";

				// Token: 0x0400C10F RID: 49423
				public static LocString EFFECT = "Removes " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Sinks when passing by in the selected direction.";

				// Token: 0x02003A7D RID: 14973
				public class FACADES
				{
					// Token: 0x02003F11 RID: 16145
					public class DEFAULT_WASHSINK
					{
						// Token: 0x0400F6F0 RID: 63216
						public static LocString NAME = UI.FormatAsLink("Sink", "WASHSINK");

						// Token: 0x0400F6F1 RID: 63217
						public static LocString DESC = "Sinks are plumbed and do not need to be manually emptied or refilled.";
					}

					// Token: 0x02003F12 RID: 16146
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400F6F2 RID: 63218
						public static LocString NAME = UI.FormatAsLink("Faint Purple Sink", "WASHSINK");

						// Token: 0x0400F6F3 RID: 63219
						public static LocString DESC = "A refreshing splash of color for the light-headed.";
					}

					// Token: 0x02003F13 RID: 16147
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400F6F4 RID: 63220
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Sink", "WASHSINK");

						// Token: 0x0400F6F5 RID: 63221
						public static LocString DESC = "A calm, colorful sink for heavy-hearted Duplicants.";
					}

					// Token: 0x02003F14 RID: 16148
					public class GREEN_MUSH
					{
						// Token: 0x0400F6F6 RID: 63222
						public static LocString NAME = UI.FormatAsLink("Mush Green Sink", "WASHSINK");

						// Token: 0x0400F6F7 RID: 63223
						public static LocString DESC = "Even the soap is mush-colored.";
					}

					// Token: 0x02003F15 RID: 16149
					public class YELLOW_TARTAR
					{
						// Token: 0x0400F6F8 RID: 63224
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Sink", "WASHSINK");

						// Token: 0x0400F6F9 RID: 63225
						public static LocString DESC = "The juxtaposition of 'ick' and 'clean' can be very satisfying.";
					}

					// Token: 0x02003F16 RID: 16150
					public class RED_ROSE
					{
						// Token: 0x0400F6FA RID: 63226
						public static LocString NAME = UI.FormatAsLink("Puce Pink Sink", "WASHSINK");

						// Token: 0x0400F6FB RID: 63227
						public static LocString DESC = "Some Duplicants say it looks like a germ-devouring mouth.";
					}
				}
			}

			// Token: 0x02002C09 RID: 11273
			public class DECONTAMINATIONSHOWER
			{
				// Token: 0x0400C110 RID: 49424
				public static LocString NAME = UI.FormatAsLink("Decontamination Shower", "DECONTAMINATIONSHOWER");

				// Token: 0x0400C111 RID: 49425
				public static LocString DESC = "Don't forget to decontaminate behind your ears.";

				// Token: 0x0400C112 RID: 49426
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Water", "WATER"),
					" to remove ",
					UI.FormatAsLink("Germs", "DISEASE"),
					" and ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					".\n\nDecontaminates both Duplicants and their ",
					UI.FormatAsLink("Clothing", "EQUIPMENT"),
					"."
				});
			}

			// Token: 0x02002C0A RID: 11274
			public class TILEPOI
			{
				// Token: 0x0400C113 RID: 49427
				public static LocString NAME = UI.FormatAsLink("Tile", "TILEPOI");

				// Token: 0x0400C114 RID: 49428
				public static LocString DESC = "";

				// Token: 0x0400C115 RID: 49429
				public static LocString EFFECT = "Used to build the walls and floor of rooms.";
			}

			// Token: 0x02002C0B RID: 11275
			public class POLYMERIZER
			{
				// Token: 0x0400C116 RID: 49430
				public static LocString NAME = UI.FormatAsLink("Polymer Press", "POLYMERIZER");

				// Token: 0x0400C117 RID: 49431
				public static LocString DESC = "Plastic can be used to craft unique buildings and goods.";

				// Token: 0x0400C118 RID: 49432
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Plastic Monomers", "PLASTIFIABLELIQUID"),
					" into raw ",
					UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
					"."
				});
			}

			// Token: 0x02002C0C RID: 11276
			public class DIRECTIONALWORLDPUMPLIQUID
			{
				// Token: 0x0400C119 RID: 49433
				public static LocString NAME = UI.FormatAsLink("Liquid Channel", "DIRECTIONALWORLDPUMPLIQUID");

				// Token: 0x0400C11A RID: 49434
				public static LocString DESC = "Channels move more volume than pumps and require no power, but need sufficient pressure to function.";

				// Token: 0x0400C11B RID: 49435
				public static LocString EFFECT = "Directionally moves large volumes of " + UI.FormatAsLink("LIQUID", "ELEMENTS_LIQUID") + " through a channel.\n\nCan be used as floor tile and rotated before construction.";
			}

			// Token: 0x02002C0D RID: 11277
			public class STEAMTURBINE
			{
				// Token: 0x0400C11C RID: 49436
				public static LocString NAME = UI.FormatAsLink("[DEPRECATED] Steam Turbine", "STEAMTURBINE");

				// Token: 0x0400C11D RID: 49437
				public static LocString DESC = "Useful for converting the geothermal energy of magma into usable power.";

				// Token: 0x0400C11E RID: 49438
				public static LocString EFFECT = string.Concat(new string[]
				{
					"THIS BUILDING HAS BEEN DEPRECATED AND CANNOT BE BUILT.\n\nGenerates exceptional electrical ",
					UI.FormatAsLink("Power", "POWER"),
					" using pressurized, ",
					UI.FormatAsLink("Scalding", "HEAT"),
					" ",
					UI.FormatAsLink("Steam", "STEAM"),
					".\n\nOutputs significantly cooler ",
					UI.FormatAsLink("Steam", "STEAM"),
					" than it receives.\n\nAir pressure beneath this building must be higher than pressure above for air to flow."
				});
			}

			// Token: 0x02002C0E RID: 11278
			public class STEAMTURBINE2
			{
				// Token: 0x0400C11F RID: 49439
				public static LocString NAME = UI.FormatAsLink("Steam Turbine", "STEAMTURBINE2");

				// Token: 0x0400C120 RID: 49440
				public static LocString DESC = "Useful for converting the geothermal energy into usable power.";

				// Token: 0x0400C121 RID: 49441
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Steam", "STEAM"),
					" from the tiles directly below the machine's foundation and uses it to generate electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nOutputs ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});

				// Token: 0x0400C122 RID: 49442
				public static LocString HEAT_SOURCE = "Power Generation Waste";
			}

			// Token: 0x02002C0F RID: 11279
			public class STEAMENGINE
			{
				// Token: 0x0400C123 RID: 49443
				public static LocString NAME = UI.FormatAsLink("Steam Engine", "STEAMENGINE");

				// Token: 0x0400C124 RID: 49444
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400C125 RID: 49445
				public static LocString EFFECT = "Utilizes " + UI.FormatAsLink("Steam", "STEAM") + " to propel rockets for space exploration.\n\nThe engine of a rocket must be built first before more rocket modules may be added.";
			}

			// Token: 0x02002C10 RID: 11280
			public class STEAMENGINECLUSTER
			{
				// Token: 0x0400C126 RID: 49446
				public static LocString NAME = UI.FormatAsLink("Steam Engine", "STEAMENGINECLUSTER");

				// Token: 0x0400C127 RID: 49447
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400C128 RID: 49448
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Utilizes ",
					UI.FormatAsLink("Steam", "STEAM"),
					" to propel rockets for space exploration.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002C11 RID: 11281
			public class SOLARPANEL
			{
				// Token: 0x0400C129 RID: 49449
				public static LocString NAME = UI.FormatAsLink("Solar Panel", "SOLARPANEL");

				// Token: 0x0400C12A RID: 49450
				public static LocString DESC = "Solar panels convert high intensity sunlight into power and produce zero waste.";

				// Token: 0x0400C12B RID: 49451
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nMust be exposed to space."
				});
			}

			// Token: 0x02002C12 RID: 11282
			public class COMETDETECTOR
			{
				// Token: 0x0400C12C RID: 49452
				public static LocString NAME = UI.FormatAsLink("Space Scanner", "COMETDETECTOR");

				// Token: 0x0400C12D RID: 49453
				public static LocString DESC = "Networks of many scanners will scan more efficiently than one on its own.";

				// Token: 0x0400C12E RID: 49454
				public static LocString EFFECT = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to its automation circuit when it detects incoming objects from space.\n\nCan be configured to detect incoming meteor showers or returning space rockets.";
			}

			// Token: 0x02002C13 RID: 11283
			public class OILREFINERY
			{
				// Token: 0x0400C12F RID: 49455
				public static LocString NAME = UI.FormatAsLink("Oil Refinery", "OILREFINERY");

				// Token: 0x0400C130 RID: 49456
				public static LocString DESC = "Petroleum can only be produced from the refinement of crude oil.";

				// Token: 0x0400C131 RID: 49457
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					" into ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" and ",
					UI.FormatAsLink("Natural Gas", "METHANE"),
					"."
				});
			}

			// Token: 0x02002C14 RID: 11284
			public class OILWELLCAP
			{
				// Token: 0x0400C132 RID: 49458
				public static LocString NAME = UI.FormatAsLink("Oil Well", "OILWELLCAP");

				// Token: 0x0400C133 RID: 49459
				public static LocString DESC = "Water pumped into an oil reservoir cannot be recovered.";

				// Token: 0x0400C134 RID: 49460
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Extracts ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					" using clean ",
					UI.FormatAsLink("Water", "WATER"),
					".\n\nMust be built atop an ",
					UI.FormatAsLink("Oil Reservoir", "OIL_WELL"),
					"."
				});
			}

			// Token: 0x02002C15 RID: 11285
			public class METALREFINERY
			{
				// Token: 0x0400C135 RID: 49461
				public static LocString NAME = UI.FormatAsLink("Metal Refinery", "METALREFINERY");

				// Token: 0x0400C136 RID: 49462
				public static LocString DESC = "Refined metals are necessary to build advanced electronics and technologies.";

				// Token: 0x0400C137 RID: 49463
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
					" from raw ",
					UI.FormatAsLink("Metal Ore", "RAWMETAL"),
					".\n\nSignificantly ",
					UI.FormatAsLink("Heats", "HEAT"),
					" and outputs the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" piped into it.\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x0400C138 RID: 49464
				public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}.";
			}

			// Token: 0x02002C16 RID: 11286
			public class MISSILEFABRICATOR
			{
				// Token: 0x0400C139 RID: 49465
				public static LocString NAME = UI.FormatAsLink("Blastshot Maker", "MISSILEFABRICATOR");

				// Token: 0x0400C13A RID: 49466
				public static LocString DESC = "Blastshot shells are an effective defense against incoming meteor showers.";

				// Token: 0x0400C13B RID: 49467
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Blastshot", "MISSILELAUNCHER"),
					" from ",
					UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
					" combined with ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x0400C13C RID: 49468
				public static LocString RECIPE_DESCRIPTION = "Produces {0} from {1} and {2}.";

				// Token: 0x0400C13D RID: 49469
				public static LocString RECIPE_DESCRIPTION_LONGRANGE = "Produces {0} from {1}, {2}, and {3}.";
			}

			// Token: 0x02002C17 RID: 11287
			public class GLASSFORGE
			{
				// Token: 0x0400C13E RID: 49470
				public static LocString NAME = UI.FormatAsLink("Glass Forge", "GLASSFORGE");

				// Token: 0x0400C13F RID: 49471
				public static LocString DESC = "Glass can be used to construct window tile.";

				// Token: 0x0400C140 RID: 49472
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Molten Glass", "MOLTENGLASS"),
					" from raw ",
					UI.FormatAsLink("Sand", "SAND"),
					".\n\nOutputs ",
					UI.FormatAsLink("High Temperature", "HEAT"),
					" ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x0400C141 RID: 49473
				public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}.";
			}

			// Token: 0x02002C18 RID: 11288
			public class ROCKCRUSHER
			{
				// Token: 0x0400C142 RID: 49474
				public static LocString NAME = UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER");

				// Token: 0x0400C143 RID: 49475
				public static LocString DESC = "Rock Crushers loosen nuggets from raw ore and can process many different resources.";

				// Token: 0x0400C144 RID: 49476
				public static LocString EFFECT = "Inefficiently produces refined materials from raw resources.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0400C145 RID: 49477
				public static LocString RECIPE_DESCRIPTION = "Crushes {0} into {1}";

				// Token: 0x0400C146 RID: 49478
				public static LocString RECIPE_DESCRIPTION_TWO_OUTPUT = "Crushes {0} into {1} and {2}";

				// Token: 0x0400C147 RID: 49479
				public static LocString METAL_RECIPE_DESCRIPTION = "Crushes {1} into " + UI.FormatAsLink("Sand", "SAND") + " and pure {0}";

				// Token: 0x0400C148 RID: 49480
				public static LocString LIME_RECIPE_DESCRIPTION = "Crushes {1} into {0}";

				// Token: 0x0400C149 RID: 49481
				public static LocString LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION = "Crushes {0} into {1} and a small amount of pure {2}";

				// Token: 0x0400C14A RID: 49482
				public static LocString RESIN_FROM_AMBER_RECIPE_DESCRIPTION = "Crushes {0} into {1} and {2}, and a small amount of {3}";

				// Token: 0x0400C14B RID: 49483
				public static LocString SAND_FROM_RAW_MINERAL_NAME = UI.FormatAsLink("Raw Mineral", "BUILDABLERAW") + " to " + UI.FormatAsLink("Sand", "SAND");

				// Token: 0x0400C14C RID: 49484
				public static LocString SAND_FROM_RAW_MINERAL_DESCRIPTION = "Crushes " + UI.FormatAsLink("Raw Minerals", "BUILDABLERAW") + " into " + UI.FormatAsLink("Sand", "SAND");

				// Token: 0x02003A7E RID: 14974
				public class FACADES
				{
					// Token: 0x02003F17 RID: 16151
					public class DEFAULT_ROCKCRUSHER
					{
						// Token: 0x0400F6FC RID: 63228
						public static LocString NAME = UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400F6FD RID: 63229
						public static LocString DESC = "Rock Crushers loosen nuggets from raw ore and can process many different resources.";
					}

					// Token: 0x02003F18 RID: 16152
					public class HANDS
					{
						// Token: 0x0400F6FE RID: 63230
						public static LocString NAME = UI.FormatAsLink("Punchy Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400F6FF RID: 63231
						public static LocString DESC = "Smashy smashy!";
					}

					// Token: 0x02003F19 RID: 16153
					public class TEETH
					{
						// Token: 0x0400F700 RID: 63232
						public static LocString NAME = UI.FormatAsLink("Toothy Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400F701 RID: 63233
						public static LocString DESC = "Not designed to handle overcooked food waste.";
					}

					// Token: 0x02003F1A RID: 16154
					public class ROUNDSTAMP
					{
						// Token: 0x0400F702 RID: 63234
						public static LocString NAME = UI.FormatAsLink("Smooth Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400F703 RID: 63235
						public static LocString DESC = "Inspired by the traditional mortar and pestle.";
					}

					// Token: 0x02003F1B RID: 16155
					public class SPIKEBEDS
					{
						// Token: 0x0400F704 RID: 63236
						public static LocString NAME = UI.FormatAsLink("Spiked Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400F705 RID: 63237
						public static LocString DESC = "Mashes rocks into oblivion.";
					}

					// Token: 0x02003F1C RID: 16156
					public class CHOMP
					{
						// Token: 0x0400F706 RID: 63238
						public static LocString NAME = UI.FormatAsLink("Mani Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400F707 RID: 63239
						public static LocString DESC = "Buffs rough ore into smooth little nuggets.";
					}

					// Token: 0x02003F1D RID: 16157
					public class GEARS
					{
						// Token: 0x0400F708 RID: 63240
						public static LocString NAME = UI.FormatAsLink("Super-Mech Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400F709 RID: 63241
						public static LocString DESC = "Uncrushed ore really grinds its gears.";
					}

					// Token: 0x02003F1E RID: 16158
					public class BALLOON
					{
						// Token: 0x0400F70A RID: 63242
						public static LocString NAME = UI.FormatAsLink("Pop-A-Rocks-E", "ROCKCRUSHER");

						// Token: 0x0400F70B RID: 63243
						public static LocString DESC = "Wherever there's raw ore, there's a rock crusher lurking nearby.";
					}
				}
			}

			// Token: 0x02002C19 RID: 11289
			public class SLUDGEPRESS
			{
				// Token: 0x0400C14D RID: 49485
				public static LocString NAME = UI.FormatAsLink("Sludge Press", "SLUDGEPRESS");

				// Token: 0x0400C14E RID: 49486
				public static LocString DESC = "What Duplicant doesn't love playing with mud?";

				// Token: 0x0400C14F RID: 49487
				public static LocString EFFECT = "Separates " + UI.FormatAsLink("Mud", "MUD") + " and other sludges into their base elements.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0400C150 RID: 49488
				public static LocString RECIPE_DESCRIPTION = "Separates {0} into its base elements.";
			}

			// Token: 0x02002C1A RID: 11290
			public class CHEMICALREFINERY
			{
				// Token: 0x0400C151 RID: 49489
				public static LocString NAME = UI.FormatAsLink("Emulsifier", "CHEMICALREFINERY");

				// Token: 0x0400C152 RID: 49490
				public static LocString DESC = "It's like a blender, but better.";

				// Token: 0x0400C153 RID: 49491
				public static LocString EFFECT = "Combines " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " and other inputs into fluid compounds.\n\nDuplicants will not fabricate emulsions unless recipes are queued.";

				// Token: 0x0400C154 RID: 49492
				public static LocString REFINEDLIPID_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Biodiesel is a ",
					UI.FormatAsLink("Combustible Liquid", "COMBUSTIBLELIQUID"),
					" used in ",
					UI.FormatAsLink("Power", "POWER"),
					" production."
				});

				// Token: 0x0400C155 RID: 49493
				public static LocString SALTWATER_RECIPE_DESCRIPTION = "Salt Water is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " with insulating and radiation-absorbing properties.";
			}

			// Token: 0x02002C1B RID: 11291
			public class SUPERMATERIALREFINERY
			{
				// Token: 0x0400C156 RID: 49494
				public static LocString NAME = UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY");

				// Token: 0x0400C157 RID: 49495
				public static LocString DESC = "Rare materials can be procured through rocket missions into space.";

				// Token: 0x0400C158 RID: 49496
				public static LocString EFFECT = "Processes " + UI.FormatAsLink("Rare Materials", "RAREMATERIALS") + " into advanced industrial goods.\n\nRare materials can be retrieved from space missions.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0400C159 RID: 49497
				public static LocString SUPERCOOLANT_RECIPE_DESCRIPTION = "Super Coolant is an industrial-grade " + UI.FormatAsLink("Fullerene", "FULLERENE") + " coolant.";

				// Token: 0x0400C15A RID: 49498
				public static LocString SUPERINSULATOR_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Insulite reduces ",
					UI.FormatAsLink("Heat Transfer", "HEAT"),
					" and is composed of recrystallized ",
					UI.FormatAsLink("Abyssalite", "KATAIRITE"),
					"."
				});

				// Token: 0x0400C15B RID: 49499
				public static LocString TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION = "Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.";

				// Token: 0x0400C15C RID: 49500
				public static LocString VISCOGEL_RECIPE_DESCRIPTION = "Visco-Gel Fluid is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension.";

				// Token: 0x0400C15D RID: 49501
				public static LocString YELLOWCAKE_RECIPE_DESCRIPTION = "Yellowcake is a " + UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID") + " used in uranium enrichment.";

				// Token: 0x0400C15E RID: 49502
				public static LocString FULLERENE_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Fullerene is a ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" used in the production of ",
					UI.FormatAsLink("Super Coolant", "SUPERCOOLANT"),
					"."
				});

				// Token: 0x0400C15F RID: 49503
				public static LocString HARDPLASTIC_RECIPE_DESCRIPTION = "Plastium is a highly heat-resistant, plastic-like " + UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID") + " used for space buildings.";

				// Token: 0x0400C160 RID: 49504
				public static LocString SELF_CHARGING_POWERBANK_RECIPE_DESCRIPTION = "Atomic Power Banks are portable, self-charging units used for isolated " + UI.FormatAsLink("Power", "POWER") + " grids.";
			}

			// Token: 0x02002C1C RID: 11292
			public class THERMALBLOCK
			{
				// Token: 0x0400C161 RID: 49505
				public static LocString NAME = UI.FormatAsLink("Tempshift Plate", "THERMALBLOCK");

				// Token: 0x0400C162 RID: 49506
				public static LocString DESC = "The thermal properties of construction materials determine their heat retention.";

				// Token: 0x0400C163 RID: 49507
				public static LocString EFFECT = "Accelerates or buffers " + UI.FormatAsLink("Heat", "HEAT") + " dispersal based on the construction material used.\n\nHas a small area of effect.";
			}

			// Token: 0x02002C1D RID: 11293
			public class POWERCONTROLSTATION
			{
				// Token: 0x0400C164 RID: 49508
				public static LocString NAME = UI.FormatAsLink("Power Control Station", "POWERCONTROLSTATION");

				// Token: 0x0400C165 RID: 49509
				public static LocString DESC = "Only one Duplicant may be assigned to a station at a time.";

				// Token: 0x0400C166 RID: 49510
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					" to increase the ",
					UI.FormatAsLink("Power", "POWER"),
					" output of generators.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Tune Up", "TECHNICALS2"),
					" trait."
				});
			}

			// Token: 0x02002C1E RID: 11294
			public class FARMSTATION
			{
				// Token: 0x0400C167 RID: 49511
				public static LocString NAME = UI.FormatAsLink("Farm Station", "FARMSTATION");

				// Token: 0x0400C168 RID: 49512
				public static LocString DESC = "This station only has an effect on crops grown within the same room.";

				// Token: 0x0400C169 RID: 49513
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Micronutrient Fertilizer", "FARM_STATION_TOOLS"),
					" to increase ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" growth rates.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Crop Tending", "FARMING2"),
					" trait.\n\nThis building is a necessary component of the Greenhouse room."
				});
			}

			// Token: 0x02002C1F RID: 11295
			public class FISHDELIVERYPOINT
			{
				// Token: 0x0400C16A RID: 49514
				public static LocString NAME = UI.FormatAsLink("Fish Release", "FISHDELIVERYPOINT");

				// Token: 0x0400C16B RID: 49515
				public static LocString DESC = "A fish release must be built in liquid to prevent released fish from suffocating.";

				// Token: 0x0400C16C RID: 49516
				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Pacu", "PACU") + " back into the world.\n\nCan be used multiple times.";
			}

			// Token: 0x02002C20 RID: 11296
			public class FISHFEEDER
			{
				// Token: 0x0400C16D RID: 49517
				public static LocString NAME = UI.FormatAsLink("Fish Feeder", "FISHFEEDER");

				// Token: 0x0400C16E RID: 49518
				public static LocString DESC = "Build this feeder above a body of water to feed the fish within.";

				// Token: 0x0400C16F RID: 49519
				public static LocString EFFECT = "Automatically dispenses stored " + UI.FormatAsLink("Critter", "CREATURES") + " food into the area below.\n\nDispenses continuously as food is consumed.";
			}

			// Token: 0x02002C21 RID: 11297
			public class FISHTRAP
			{
				// Token: 0x0400C170 RID: 49520
				public static LocString NAME = UI.FormatAsLink("Fish Trap", "FISHTRAP");

				// Token: 0x0400C171 RID: 49521
				public static LocString DESC = "Trapped fish will automatically be bagged for transport.";

				// Token: 0x0400C172 RID: 49522
				public static LocString EFFECT = "Attracts and traps swimming " + UI.FormatAsLink("Pacu", "PACU") + ".\n\nSingle use.";
			}

			// Token: 0x02002C22 RID: 11298
			public class RANCHSTATION
			{
				// Token: 0x0400C173 RID: 49523
				public static LocString NAME = UI.FormatAsLink("Grooming Station", "RANCHSTATION");

				// Token: 0x0400C174 RID: 49524
				public static LocString DESC = "A groomed critter is a happy, healthy, productive critter.";

				// Token: 0x0400C175 RID: 49525
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows the assigned ",
					UI.FormatAsLink("Rancher", "RANCHER"),
					" to care for ",
					UI.FormatAsLink("Critters", "CREATURES"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Critter Ranching", "RANCHING1"),
					" skill."
				});
			}

			// Token: 0x02002C23 RID: 11299
			public class MACHINESHOP
			{
				// Token: 0x0400C176 RID: 49526
				public static LocString NAME = UI.FormatAsLink("Mechanics Station", "MACHINESHOP");

				// Token: 0x0400C177 RID: 49527
				public static LocString DESC = "Duplicants will only improve the efficiency of buildings in the same room as this station.";

				// Token: 0x0400C178 RID: 49528
				public static LocString EFFECT = "Allows the assigned " + UI.FormatAsLink("Engineer", "MACHINE_TECHNICIAN") + " to improve building production efficiency.\n\nThis building is a necessary component of the Machine Shop room.";
			}

			// Token: 0x02002C24 RID: 11300
			public class LOGICWIRE
			{
				// Token: 0x0400C179 RID: 49529
				public static LocString NAME = UI.FormatAsLink("Automation Wire", "LOGICWIRE");

				// Token: 0x0400C17A RID: 49530
				public static LocString DESC = "Automation wire is used to connect building ports to automation gates.";

				// Token: 0x0400C17B RID: 49531
				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Sensors", "LOGIC") + ".\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002C25 RID: 11301
			public class LOGICRIBBON
			{
				// Token: 0x0400C17C RID: 49532
				public static LocString NAME = UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON");

				// Token: 0x0400C17D RID: 49533
				public static LocString DESC = "Logic ribbons use significantly less space to carry multiple automation signals.";

				// Token: 0x0400C17E RID: 49534
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A 4-Bit ",
					BUILDINGS.PREFABS.LOGICWIRE.NAME,
					" which can carry up to four automation signals.\n\nUse a ",
					UI.FormatAsLink("Ribbon Writer", "LOGICRIBBONWRITER"),
					" to output to multiple Bits, and a ",
					UI.FormatAsLink("Ribbon Reader", "LOGICRIBBONREADER"),
					" to input from multiple Bits."
				});
			}

			// Token: 0x02002C26 RID: 11302
			public class LOGICWIREBRIDGE
			{
				// Token: 0x0400C17F RID: 49535
				public static LocString NAME = UI.FormatAsLink("Automation Wire Bridge", "LOGICWIREBRIDGE");

				// Token: 0x0400C180 RID: 49536
				public static LocString DESC = "Wire bridges allow multiple automation grids to exist in a small area without connecting.";

				// Token: 0x0400C181 RID: 49537
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Automation Wire", "LOGICWIRE") + " section over another without joining them.\n\nCan be run through wall and floor tile.";

				// Token: 0x0400C182 RID: 49538
				public static LocString LOGIC_PORT = "Transmit Signal";

				// Token: 0x0400C183 RID: 49539
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Pass through the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400C184 RID: 49540
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Pass through the " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C27 RID: 11303
			public class LOGICRIBBONBRIDGE
			{
				// Token: 0x0400C185 RID: 49541
				public static LocString NAME = UI.FormatAsLink("Automation Ribbon Bridge", "LOGICRIBBONBRIDGE");

				// Token: 0x0400C186 RID: 49542
				public static LocString DESC = "Wire bridges allow multiple automation grids to exist in a small area without connecting.";

				// Token: 0x0400C187 RID: 49543
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON") + " section over another without joining them.\n\nCan be run through wall and floor tile.";

				// Token: 0x0400C188 RID: 49544
				public static LocString LOGIC_PORT = "Transmit Signal";

				// Token: 0x0400C189 RID: 49545
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Pass through the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400C18A RID: 49546
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Pass through the " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C28 RID: 11304
			public class LOGICGATEAND
			{
				// Token: 0x0400C18B RID: 49547
				public static LocString NAME = UI.FormatAsLink("AND Gate", "LOGICGATEAND");

				// Token: 0x0400C18C RID: 49548
				public static LocString DESC = "This gate outputs a Green Signal when both its inputs are receiving Green Signals at the same time.";

				// Token: 0x0400C18D RID: 49549
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when both Input A <b>AND</b> Input B are receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when even one Input is receiving ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x0400C18E RID: 49550
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400C18F RID: 49551
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400C190 RID: 49552
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if any Input is receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);
			}

			// Token: 0x02002C29 RID: 11305
			public class LOGICGATEOR
			{
				// Token: 0x0400C191 RID: 49553
				public static LocString NAME = UI.FormatAsLink("OR Gate", "LOGICGATEOR");

				// Token: 0x0400C192 RID: 49554
				public static LocString DESC = "This gate outputs a Green Signal if receiving one or more Green Signals.";

				// Token: 0x0400C193 RID: 49555
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if at least one of Input A <b>OR</b> Input B is receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when neither Input A or Input B are receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					"."
				});

				// Token: 0x0400C194 RID: 49556
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400C195 RID: 49557
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if any Input is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400C196 RID: 49558
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);
			}

			// Token: 0x02002C2A RID: 11306
			public class LOGICGATENOT
			{
				// Token: 0x0400C197 RID: 49559
				public static LocString NAME = UI.FormatAsLink("NOT Gate", "LOGICGATENOT");

				// Token: 0x0400C198 RID: 49560
				public static LocString DESC = "This gate reverses automation signals, turning a Green Signal into a Red Signal and vice versa.";

				// Token: 0x0400C199 RID: 49561
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the Input is receiving a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when its Input is receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				// Token: 0x0400C19A RID: 49562
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400C19B RID: 49563
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				// Token: 0x0400C19C RID: 49564
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);
			}

			// Token: 0x02002C2B RID: 11307
			public class LOGICGATEXOR
			{
				// Token: 0x0400C19D RID: 49565
				public static LocString NAME = UI.FormatAsLink("XOR Gate", "LOGICGATEXOR");

				// Token: 0x0400C19E RID: 49566
				public static LocString DESC = "This gate outputs a Green Signal if exactly one of its Inputs is receiving a Green Signal.";

				// Token: 0x0400C19F RID: 49567
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if exactly one of its Inputs is receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" if both or neither Inputs are receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				// Token: 0x0400C1A0 RID: 49568
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400C1A1 RID: 49569
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if exactly one of its Inputs is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400C1A2 RID: 49570
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Input signals match (any color)";
			}

			// Token: 0x02002C2C RID: 11308
			public class LOGICGATEBUFFER
			{
				// Token: 0x0400C1A3 RID: 49571
				public static LocString NAME = UI.FormatAsLink("BUFFER Gate", "LOGICGATEBUFFER");

				// Token: 0x0400C1A4 RID: 49572
				public static LocString DESC = "This gate continues outputting a Green Signal for a short time after the gate stops receiving a Green Signal.";

				// Token: 0x0400C1A5 RID: 49573
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the Input is receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					".\n\nContinues sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for an amount of buffer time after the Input receives a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x0400C1A6 RID: 49574
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400C1A7 RID: 49575
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" while receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					". After receiving ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					", will continue sending ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" until the timer has expired"
				});

				// Token: 0x0400C1A8 RID: 49576
				public static LocString OUTPUT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".";
			}

			// Token: 0x02002C2D RID: 11309
			public class LOGICGATEFILTER
			{
				// Token: 0x0400C1A9 RID: 49577
				public static LocString NAME = UI.FormatAsLink("FILTER Gate", "LOGICGATEFILTER");

				// Token: 0x0400C1AA RID: 49578
				public static LocString DESC = "This gate only lets a Green Signal through if its Input has received a Green Signal that lasted longer than the selected filter time.";

				// Token: 0x0400C1AB RID: 49579
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Only lets a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" through if the Input has received a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for longer than the selected filter time.\n\nWill continue outputting a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" if the ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" did not last long enough."
				});

				// Token: 0x0400C1AC RID: 49580
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400C1AD RID: 49581
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" after receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" for longer than the selected filter timer"
				});

				// Token: 0x0400C1AE RID: 49582
				public static LocString OUTPUT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".";
			}

			// Token: 0x02002C2E RID: 11310
			public class LOGICMEMORY
			{
				// Token: 0x0400C1AF RID: 49583
				public static LocString NAME = UI.FormatAsLink("Memory Toggle", "LOGICMEMORY");

				// Token: 0x0400C1B0 RID: 49584
				public static LocString DESC = "A Memory stores a Green Signal received in the Set Port (S) until the Reset Port (R) receives a Green Signal.";

				// Token: 0x0400C1B1 RID: 49585
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Contains an internal Memory, and will output whatever signal is stored in that Memory.\n\nSignals sent to the Inputs <i>only</i> affect the Memory, and do not pass through to the Output. \n\nSending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to the Set Port (S) will set the memory to ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					". \n\nSending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to the Reset Port (R) will reset the memory back to ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x0400C1B2 RID: 49586
				public static LocString STATUS_ITEM_VALUE = "Current Value: {0}";

				// Token: 0x0400C1B3 RID: 49587
				public static LocString READ_PORT = "MEMORY OUTPUT";

				// Token: 0x0400C1B4 RID: 49588
				public static LocString READ_PORT_ACTIVE = "Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400C1B5 RID: 49589
				public static LocString READ_PORT_INACTIVE = "Outputs a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				// Token: 0x0400C1B6 RID: 49590
				public static LocString SET_PORT = "SET PORT (S)";

				// Token: 0x0400C1B7 RID: 49591
				public static LocString SET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set the internal Memory to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400C1B8 RID: 49592
				public static LocString SET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect";

				// Token: 0x0400C1B9 RID: 49593
				public static LocString RESET_PORT = "RESET PORT (R)";

				// Token: 0x0400C1BA RID: 49594
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the internal Memory to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				// Token: 0x0400C1BB RID: 49595
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect";
			}

			// Token: 0x02002C2F RID: 11311
			public class LOGICGATEMULTIPLEXER
			{
				// Token: 0x0400C1BC RID: 49596
				public static LocString NAME = UI.FormatAsLink("Signal Selector", "LOGICGATEMULTIPLEXER");

				// Token: 0x0400C1BD RID: 49597
				public static LocString DESC = "Signal Selectors can be used to select which automation signal is relevant to pass through to a given circuit";

				// Token: 0x0400C1BE RID: 49598
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Select which one of four Input signals should be sent out the Output, using Control Inputs.\n\nSend a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the two Control Inputs to determine which Input is selected."
				});

				// Token: 0x0400C1BF RID: 49599
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400C1C0 RID: 49600
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Receives a ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					" signal from the selected input"
				});

				// Token: 0x0400C1C1 RID: 49601
				public static LocString OUTPUT_INACTIVE = "Nothing";
			}

			// Token: 0x02002C30 RID: 11312
			public class LOGICGATEDEMULTIPLEXER
			{
				// Token: 0x0400C1C2 RID: 49602
				public static LocString NAME = UI.FormatAsLink("Signal Distributor", "LOGICGATEDEMULTIPLEXER");

				// Token: 0x0400C1C3 RID: 49603
				public static LocString DESC = "Signal Distributors can be used to choose which circuit should receive a given automation signal.";

				// Token: 0x0400C1C4 RID: 49604
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Route a single Input signal out one of four possible Outputs, based on the selection made by the Control Inputs.\n\nSend a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the two Control Inputs to determine which Output is selected."
				});

				// Token: 0x0400C1C5 RID: 49605
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400C1C6 RID: 49606
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					" signal to the selected output"
				});

				// Token: 0x0400C1C7 RID: 49607
				public static LocString OUTPUT_INACTIVE = "Nothing";
			}

			// Token: 0x02002C31 RID: 11313
			public class LOGICSWITCH
			{
				// Token: 0x0400C1C8 RID: 49608
				public static LocString NAME = UI.FormatAsLink("Signal Switch", "LOGICSWITCH");

				// Token: 0x0400C1C9 RID: 49609
				public static LocString DESC = "Signal switches don't turn grids on and off like power switches, but add an extra signal.";

				// Token: 0x0400C1CA RID: 49610
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" on an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid."
				});

				// Token: 0x0400C1CB RID: 49611
				public static LocString SIDESCREEN_TITLE = "Signal Switch";

				// Token: 0x0400C1CC RID: 49612
				public static LocString LOGIC_PORT = "Signal Toggle";

				// Token: 0x0400C1CD RID: 49613
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if toggled ON";

				// Token: 0x0400C1CE RID: 49614
				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if toggled OFF";
			}

			// Token: 0x02002C32 RID: 11314
			public class LOGICPRESSURESENSORGAS
			{
				// Token: 0x0400C1CF RID: 49615
				public static LocString NAME = UI.FormatAsLink("Atmo Sensor", "LOGICPRESSURESENSORGAS");

				// Token: 0x0400C1D0 RID: 49616
				public static LocString DESC = "Atmo sensors can be used to prevent excess oxygen production and overpressurization.";

				// Token: 0x0400C1D1 RID: 49617
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" pressure enters the chosen range."
				});

				// Token: 0x0400C1D2 RID: 49618
				public static LocString LOGIC_PORT = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Pressure";

				// Token: 0x0400C1D3 RID: 49619
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Gas pressure is within the selected range";

				// Token: 0x0400C1D4 RID: 49620
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C33 RID: 11315
			public class LOGICPRESSURESENSORLIQUID
			{
				// Token: 0x0400C1D5 RID: 49621
				public static LocString NAME = UI.FormatAsLink("Hydro Sensor", "LOGICPRESSURESENSORLIQUID");

				// Token: 0x0400C1D6 RID: 49622
				public static LocString DESC = "A hydro sensor can tell a pump to refill its basin as soon as it contains too little liquid.";

				// Token: 0x0400C1D7 RID: 49623
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" pressure enters the chosen range.\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});

				// Token: 0x0400C1D8 RID: 49624
				public static LocString LOGIC_PORT = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Pressure";

				// Token: 0x0400C1D9 RID: 49625
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Liquid pressure is within the selected range";

				// Token: 0x0400C1DA RID: 49626
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C34 RID: 11316
			public class LOGICTEMPERATURESENSOR
			{
				// Token: 0x0400C1DB RID: 49627
				public static LocString NAME = UI.FormatAsLink("Thermo Sensor", "LOGICTEMPERATURESENSOR");

				// Token: 0x0400C1DC RID: 49628
				public static LocString DESC = "Thermo sensors can disable buildings when they approach dangerous temperatures.";

				// Token: 0x0400C1DD RID: 49629
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" enters the chosen range."
				});

				// Token: 0x0400C1DE RID: 49630
				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x0400C1DF RID: 49631
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" is within the selected range"
				});

				// Token: 0x0400C1E0 RID: 49632
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C35 RID: 11317
			public class LOGICLIGHTSENSOR
			{
				// Token: 0x0400C1E1 RID: 49633
				public static LocString NAME = UI.FormatAsLink("Light Sensor", "LOGICLIGHTSENSOR");

				// Token: 0x0400C1E2 RID: 49634
				public static LocString DESC = "Light sensors can tell surface bunker doors above solar panels to open or close based on solar light levels.";

				// Token: 0x0400C1E3 RID: 49635
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Brightness", "LIGHT"),
					" enters the chosen range."
				});

				// Token: 0x0400C1E4 RID: 49636
				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Brightness", "LIGHT");

				// Token: 0x0400C1E5 RID: 49637
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Brightness", "LIGHT"),
					" is within the selected range"
				});

				// Token: 0x0400C1E6 RID: 49638
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C36 RID: 11318
			public class LOGICWATTAGESENSOR
			{
				// Token: 0x0400C1E7 RID: 49639
				public static LocString NAME = UI.FormatAsLink("Wattage Sensor", "LOGICWATTAGESENSOR");

				// Token: 0x0400C1E8 RID: 49640
				public static LocString DESC = "Wattage sensors can send a signal when a building has switched on or off.";

				// Token: 0x0400C1E9 RID: 49641
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Wattage", "POWER"),
					" consumed enters the chosen range."
				});

				// Token: 0x0400C1EA RID: 49642
				public static LocString LOGIC_PORT = "Consumed " + UI.FormatAsLink("Wattage", "POWER");

				// Token: 0x0400C1EB RID: 49643
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if current ",
					UI.FormatAsLink("Wattage", "POWER"),
					" is within the selected range"
				});

				// Token: 0x0400C1EC RID: 49644
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C37 RID: 11319
			public class LOGICHEPSENSOR
			{
				// Token: 0x0400C1ED RID: 49645
				public static LocString NAME = UI.FormatAsLink("Radbolt Sensor", "LOGICHEPSENSOR");

				// Token: 0x0400C1EE RID: 49646
				public static LocString DESC = "Radbolt sensors can send a signal when a Radbolt passes over them.";

				// Token: 0x0400C1EF RID: 49647
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when Radbolts detected enters the chosen range."
				});

				// Token: 0x0400C1F0 RID: 49648
				public static LocString LOGIC_PORT = "Detected Radbolts";

				// Token: 0x0400C1F1 RID: 49649
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if detected Radbolts are within the selected range";

				// Token: 0x0400C1F2 RID: 49650
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C38 RID: 11320
			public class LOGICTIMEOFDAYSENSOR
			{
				// Token: 0x0400C1F3 RID: 49651
				public static LocString NAME = UI.FormatAsLink("Cycle Sensor", "LOGICTIMEOFDAYSENSOR");

				// Token: 0x0400C1F4 RID: 49652
				public static LocString DESC = "Cycle sensors ensure systems always turn on at the same time, day or night, every cycle.";

				// Token: 0x0400C1F5 RID: 49653
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sets an automatic ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" and ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" schedule within one day-night cycle."
				});

				// Token: 0x0400C1F6 RID: 49654
				public static LocString LOGIC_PORT = "Cycle Time";

				// Token: 0x0400C1F7 RID: 49655
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if current time is within the selected ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" range"
				});

				// Token: 0x0400C1F8 RID: 49656
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C39 RID: 11321
			public class LOGICTIMERSENSOR
			{
				// Token: 0x0400C1F9 RID: 49657
				public static LocString NAME = UI.FormatAsLink("Timer Sensor", "LOGICTIMERSENSOR");

				// Token: 0x0400C1FA RID: 49658
				public static LocString DESC = "Timer sensors create automation schedules for very short or very long periods of time.";

				// Token: 0x0400C1FB RID: 49659
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates a timer to send ",
					UI.FormatAsAutomationState("Green Signals", UI.AutomationState.Active),
					" and ",
					UI.FormatAsAutomationState("Red Signals", UI.AutomationState.Standby),
					" for specific amounts of time."
				});

				// Token: 0x0400C1FC RID: 49660
				public static LocString LOGIC_PORT = "Timer Schedule";

				// Token: 0x0400C1FD RID: 49661
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for the selected amount of Green time";

				// Token: 0x0400C1FE RID: 49662
				public static LocString LOGIC_PORT_INACTIVE = "Then, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " for the selected amount of Red time";
			}

			// Token: 0x02002C3A RID: 11322
			public class LOGICCRITTERCOUNTSENSOR
			{
				// Token: 0x0400C1FF RID: 49663
				public static LocString NAME = UI.FormatAsLink("Critter Sensor", "LOGICCRITTERCOUNTSENSOR");

				// Token: 0x0400C200 RID: 49664
				public static LocString DESC = "Detecting critter populations can help adjust their automated feeding and care regimens.";

				// Token: 0x0400C201 RID: 49665
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the number of eggs and critters in a room."
				});

				// Token: 0x0400C202 RID: 49666
				public static LocString LOGIC_PORT = "Critter Count";

				// Token: 0x0400C203 RID: 49667
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Critters and Eggs in the Room is greater than the selected threshold.";

				// Token: 0x0400C204 RID: 49668
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400C205 RID: 49669
				public static LocString SIDESCREEN_TITLE = "Critter Sensor";

				// Token: 0x0400C206 RID: 49670
				public static LocString COUNT_CRITTER_LABEL = "Count Critters";

				// Token: 0x0400C207 RID: 49671
				public static LocString COUNT_EGG_LABEL = "Count Eggs";
			}

			// Token: 0x02002C3B RID: 11323
			public class LOGICCLUSTERLOCATIONSENSOR
			{
				// Token: 0x0400C208 RID: 49672
				public static LocString NAME = UI.FormatAsLink("Starmap Location Sensor", "LOGICCLUSTERLOCATIONSENSOR");

				// Token: 0x0400C209 RID: 49673
				public static LocString DESC = "Starmap Location sensors can signal when a spacecraft is at a certain location";

				// Token: 0x0400C20A RID: 49674
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Send ",
					UI.FormatAsAutomationState("Green Signals", UI.AutomationState.Active),
					" at the chosen Starmap locations and ",
					UI.FormatAsAutomationState("Red Signals", UI.AutomationState.Standby),
					" everywhere else."
				});

				// Token: 0x0400C20B RID: 49675
				public static LocString LOGIC_PORT = "Starmap Location Sensor";

				// Token: 0x0400C20C RID: 49676
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + "when a spacecraft is at the chosen Starmap locations";

				// Token: 0x0400C20D RID: 49677
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C3C RID: 11324
			public class LOGICDUPLICANTSENSOR
			{
				// Token: 0x0400C20E RID: 49678
				public static LocString NAME = UI.FormatAsLink("Duplicant Motion Sensor", "LOGICDUPLICANTSENSOR");

				// Token: 0x0400C20F RID: 49679
				public static LocString DESC = "Motion sensors save power by only enabling buildings when Duplicants are nearby.";

				// Token: 0x0400C210 RID: 49680
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on whether a Duplicant is in the sensor's range."
				});

				// Token: 0x0400C211 RID: 49681
				public static LocString LOGIC_PORT = "Duplicant Motion Sensor";

				// Token: 0x0400C212 RID: 49682
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " while a Duplicant is in the sensor's tile range";

				// Token: 0x0400C213 RID: 49683
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C3D RID: 11325
			public class LOGICDISEASESENSOR
			{
				// Token: 0x0400C214 RID: 49684
				public static LocString NAME = UI.FormatAsLink("Germ Sensor", "LOGICDISEASESENSOR");

				// Token: 0x0400C215 RID: 49685
				public static LocString DESC = "Detecting germ populations can help block off or clean up dangerous areas.";

				// Token: 0x0400C216 RID: 49686
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on quantity of surrounding ",
					UI.FormatAsLink("Germs", "DISEASE"),
					"."
				});

				// Token: 0x0400C217 RID: 49687
				public static LocString LOGIC_PORT = UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x0400C218 RID: 49688
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs is within the selected range";

				// Token: 0x0400C219 RID: 49689
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C3E RID: 11326
			public class LOGICELEMENTSENSORGAS
			{
				// Token: 0x0400C21A RID: 49690
				public static LocString NAME = UI.FormatAsLink("Gas Element Sensor", "LOGICELEMENTSENSORGAS");

				// Token: 0x0400C21B RID: 49691
				public static LocString DESC = "These sensors can detect the presence of a specific gas and alter systems accordingly.";

				// Token: 0x0400C21C RID: 49692
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is detected on this sensor's tile.\n\nSends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is not present."
				});

				// Token: 0x0400C21D RID: 49693
				public static LocString LOGIC_PORT = "Specific " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence";

				// Token: 0x0400C21E RID: 49694
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Gas is detected";

				// Token: 0x0400C21F RID: 49695
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C3F RID: 11327
			public class LOGICELEMENTSENSORLIQUID
			{
				// Token: 0x0400C220 RID: 49696
				public static LocString NAME = UI.FormatAsLink("Liquid Element Sensor", "LOGICELEMENTSENSORLIQUID");

				// Token: 0x0400C221 RID: 49697
				public static LocString DESC = "These sensors can detect the presence of a specific liquid and alter systems accordingly.";

				// Token: 0x0400C222 RID: 49698
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is detected on this sensor's tile.\n\nSends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is not present."
				});

				// Token: 0x0400C223 RID: 49699
				public static LocString LOGIC_PORT = "Specific " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence";

				// Token: 0x0400C224 RID: 49700
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Liquid is detected";

				// Token: 0x0400C225 RID: 49701
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C40 RID: 11328
			public class LOGICRADIATIONSENSOR
			{
				// Token: 0x0400C226 RID: 49702
				public static LocString NAME = UI.FormatAsLink("Radiation Sensor", "LOGICRADIATIONSENSOR");

				// Token: 0x0400C227 RID: 49703
				public static LocString DESC = "Radiation sensors can disable buildings when they detect dangerous levels of radiation.";

				// Token: 0x0400C228 RID: 49704
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" enters the chosen range."
				});

				// Token: 0x0400C229 RID: 49705
				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400C22A RID: 49706
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" is within the selected range"
				});

				// Token: 0x0400C22B RID: 49707
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C41 RID: 11329
			public class GASCONDUITDISEASESENSOR
			{
				// Token: 0x0400C22C RID: 49708
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Germ Sensor", "GASCONDUITDISEASESENSOR");

				// Token: 0x0400C22D RID: 49709
				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				// Token: 0x0400C22E RID: 49710
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the pipe."
				});

				// Token: 0x0400C22F RID: 49711
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x0400C230 RID: 49712
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range";

				// Token: 0x0400C231 RID: 49713
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C42 RID: 11330
			public class LIQUIDCONDUITDISEASESENSOR
			{
				// Token: 0x0400C232 RID: 49714
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Germ Sensor", "LIQUIDCONDUITDISEASESENSOR");

				// Token: 0x0400C233 RID: 49715
				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				// Token: 0x0400C234 RID: 49716
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the pipe."
				});

				// Token: 0x0400C235 RID: 49717
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x0400C236 RID: 49718
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range";

				// Token: 0x0400C237 RID: 49719
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C43 RID: 11331
			public class SOLIDCONDUITDISEASESENSOR
			{
				// Token: 0x0400C238 RID: 49720
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Germ Sensor", "SOLIDCONDUITDISEASESENSOR");

				// Token: 0x0400C239 RID: 49721
				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				// Token: 0x0400C23A RID: 49722
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the object on the rail."
				});

				// Token: 0x0400C23B RID: 49723
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x0400C23C RID: 49724
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs on the object on the rail is within the selected range";

				// Token: 0x0400C23D RID: 49725
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C44 RID: 11332
			public class GASCONDUITELEMENTSENSOR
			{
				// Token: 0x0400C23E RID: 49726
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Element Sensor", "GASCONDUITELEMENTSENSOR");

				// Token: 0x0400C23F RID: 49727
				public static LocString DESC = "Element sensors can be used to detect the presence of a specific gas in a pipe.";

				// Token: 0x0400C240 RID: 49728
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is detected within a pipe."
				});

				// Token: 0x0400C241 RID: 49729
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence";

				// Token: 0x0400C242 RID: 49730
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Gas is detected";

				// Token: 0x0400C243 RID: 49731
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C45 RID: 11333
			public class LIQUIDCONDUITELEMENTSENSOR
			{
				// Token: 0x0400C244 RID: 49732
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Element Sensor", "LIQUIDCONDUITELEMENTSENSOR");

				// Token: 0x0400C245 RID: 49733
				public static LocString DESC = "Element sensors can be used to detect the presence of a specific liquid in a pipe.";

				// Token: 0x0400C246 RID: 49734
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is detected within a pipe."
				});

				// Token: 0x0400C247 RID: 49735
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence";

				// Token: 0x0400C248 RID: 49736
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Liquid is detected within the pipe";

				// Token: 0x0400C249 RID: 49737
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C46 RID: 11334
			public class SOLIDCONDUITELEMENTSENSOR
			{
				// Token: 0x0400C24A RID: 49738
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Element Sensor", "SOLIDCONDUITELEMENTSENSOR");

				// Token: 0x0400C24B RID: 49739
				public static LocString DESC = "Element sensors can be used to detect the presence of a specific item on a rail.";

				// Token: 0x0400C24C RID: 49740
				public static LocString EFFECT = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected item is detected on a rail.";

				// Token: 0x0400C24D RID: 49741
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Item", "ELEMENTS_LIQUID") + " Presence";

				// Token: 0x0400C24E RID: 49742
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured item is detected on the rail";

				// Token: 0x0400C24F RID: 49743
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C47 RID: 11335
			public class GASCONDUITTEMPERATURESENSOR
			{
				// Token: 0x0400C250 RID: 49744
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Thermo Sensor", "GASCONDUITTEMPERATURESENSOR");

				// Token: 0x0400C251 RID: 49745
				public static LocString DESC = "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";

				// Token: 0x0400C252 RID: 49746
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when pipe contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				// Token: 0x0400C253 RID: 49747
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x0400C254 RID: 49748
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Gas is within the selected Temperature range";

				// Token: 0x0400C255 RID: 49749
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C48 RID: 11336
			public class LIQUIDCONDUITTEMPERATURESENSOR
			{
				// Token: 0x0400C256 RID: 49750
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Thermo Sensor", "LIQUIDCONDUITTEMPERATURESENSOR");

				// Token: 0x0400C257 RID: 49751
				public static LocString DESC = "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";

				// Token: 0x0400C258 RID: 49752
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when pipe contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				// Token: 0x0400C259 RID: 49753
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x0400C25A RID: 49754
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Liquid is within the selected Temperature range";

				// Token: 0x0400C25B RID: 49755
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C49 RID: 11337
			public class SOLIDCONDUITTEMPERATURESENSOR
			{
				// Token: 0x0400C25C RID: 49756
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Thermo Sensor", "SOLIDCONDUITTEMPERATURESENSOR");

				// Token: 0x0400C25D RID: 49757
				public static LocString DESC = "Thermo sensors disable buildings when their rail contents reach a certain temperature.";

				// Token: 0x0400C25E RID: 49758
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when rail contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				// Token: 0x0400C25F RID: 49759
				public static LocString LOGIC_PORT = "Internal Item " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x0400C260 RID: 49760
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained item is within the selected Temperature range";

				// Token: 0x0400C261 RID: 49761
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C4A RID: 11338
			public class LOGICCOUNTER
			{
				// Token: 0x0400C262 RID: 49762
				public static LocString NAME = UI.FormatAsLink("Signal Counter", "LOGICCOUNTER");

				// Token: 0x0400C263 RID: 49763
				public static LocString DESC = "For numbers higher than ten connect multiple counters together.";

				// Token: 0x0400C264 RID: 49764
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Counts how many times a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" has been received up to a chosen number.\n\nWhen the chosen number is reached it sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" until it receives another ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					", when it resets automatically and begins counting again."
				});

				// Token: 0x0400C265 RID: 49765
				public static LocString LOGIC_PORT = "Internal Counter Value";

				// Token: 0x0400C266 RID: 49766
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Increase counter by one";

				// Token: 0x0400C267 RID: 49767
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";

				// Token: 0x0400C268 RID: 49768
				public static LocString LOGIC_PORT_RESET = "Reset Counter";

				// Token: 0x0400C269 RID: 49769
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset counter";

				// Token: 0x0400C26A RID: 49770
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";

				// Token: 0x0400C26B RID: 49771
				public static LocString LOGIC_PORT_OUTPUT = "Number Reached";

				// Token: 0x0400C26C RID: 49772
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the counter matches the selected value";

				// Token: 0x0400C26D RID: 49773
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C4B RID: 11339
			public class LOGICALARM
			{
				// Token: 0x0400C26E RID: 49774
				public static LocString NAME = UI.FormatAsLink("Automated Notifier", "LOGICALARM");

				// Token: 0x0400C26F RID: 49775
				public static LocString DESC = "Sends a notification when it receives a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".";

				// Token: 0x0400C270 RID: 49776
				public static LocString EFFECT = "Attach to sensors to send a notification when certain conditions are met.\n\nNotifications can be customized.";

				// Token: 0x0400C271 RID: 49777
				public static LocString LOGIC_PORT = "Notification";

				// Token: 0x0400C272 RID: 49778
				public static LocString INPUT_NAME = "INPUT";

				// Token: 0x0400C273 RID: 49779
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Push notification";

				// Token: 0x0400C274 RID: 49780
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002C4C RID: 11340
			public class PIXELPACK
			{
				// Token: 0x0400C275 RID: 49781
				public static LocString NAME = UI.FormatAsLink("Pixel Pack", "PIXELPACK");

				// Token: 0x0400C276 RID: 49782
				public static LocString DESC = "Four pixels which can be individually designated different colors.";

				// Token: 0x0400C277 RID: 49783
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Pixels can be designated a color when it receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" and a different color when it receives a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					".\n\nInput from an ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE"),
					" controls the whole strip. Input from an ",
					UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON"),
					" can control individual pixels on the strip."
				});

				// Token: 0x0400C278 RID: 49784
				public static LocString LOGIC_PORT = "Color Selection";

				// Token: 0x0400C279 RID: 49785
				public static LocString INPUT_NAME = "RIBBON INPUT";

				// Token: 0x0400C27A RID: 49786
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Display the configured " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " pixels";

				// Token: 0x0400C27B RID: 49787
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Display the configured " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " pixels";

				// Token: 0x0400C27C RID: 49788
				public static LocString SIDESCREEN_TITLE = "Pixel Pack";
			}

			// Token: 0x02002C4D RID: 11341
			public class LOGICHAMMER
			{
				// Token: 0x0400C27D RID: 49789
				public static LocString NAME = UI.FormatAsLink("Hammer", "LOGICHAMMER");

				// Token: 0x0400C27E RID: 49790
				public static LocString DESC = "The hammer makes neat sounds when it strikes buildings.";

				// Token: 0x0400C27F RID: 49791
				public static LocString EFFECT = "In its default orientation, the hammer strikes the building to the left when it receives a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".\n\nEach building has a unique sound when struck by the hammer.\n\nThe hammer does no damage when it strikes.";

				// Token: 0x0400C280 RID: 49792
				public static LocString LOGIC_PORT = "Resonating Buildings";

				// Token: 0x0400C281 RID: 49793
				public static LocString INPUT_NAME = "INPUT";

				// Token: 0x0400C282 RID: 49794
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Hammer strikes once";

				// Token: 0x0400C283 RID: 49795
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002C4E RID: 11342
			public class LOGICRIBBONWRITER
			{
				// Token: 0x0400C284 RID: 49796
				public static LocString NAME = UI.FormatAsLink("Ribbon Writer", "LOGICRIBBONWRITER");

				// Token: 0x0400C285 RID: 49797
				public static LocString DESC = "Translates the signal from an " + UI.FormatAsLink("Automation Wire", "LOGICWIRE") + " to a single Bit in an " + UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON");

				// Token: 0x0400C286 RID: 49798
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Writes a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the specified Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					"\n\n",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					" must be used as the output wire to avoid overloading."
				});

				// Token: 0x0400C287 RID: 49799
				public static LocString LOGIC_PORT = "1-Bit Input";

				// Token: 0x0400C288 RID: 49800
				public static LocString INPUT_NAME = "INPUT";

				// Token: 0x0400C289 RID: 49801
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Receives " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to be written to selected Bit";

				// Token: 0x0400C28A RID: 49802
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Receives " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " to to be written selected Bit";

				// Token: 0x0400C28B RID: 49803
				public static LocString LOGIC_PORT_OUTPUT = "Bit Writing";

				// Token: 0x0400C28C RID: 49804
				public static LocString OUTPUT_NAME = "RIBBON OUTPUT";

				// Token: 0x0400C28D RID: 49805
				public static LocString OUTPUT_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Writes a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to selected Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME
				});

				// Token: 0x0400C28E RID: 49806
				public static LocString OUTPUT_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Writes a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to selected Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME
				});
			}

			// Token: 0x02002C4F RID: 11343
			public class LOGICRIBBONREADER
			{
				// Token: 0x0400C28F RID: 49807
				public static LocString NAME = UI.FormatAsLink("Ribbon Reader", "LOGICRIBBONREADER");

				// Token: 0x0400C290 RID: 49808
				public static LocString DESC = string.Concat(new string[]
				{
					"Inputs the signal from a single Bit in an ",
					UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON"),
					" into an ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE"),
					"."
				});

				// Token: 0x0400C291 RID: 49809
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reads a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" from the specified Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					" onto an ",
					BUILDINGS.PREFABS.LOGICWIRE.NAME,
					"."
				});

				// Token: 0x0400C292 RID: 49810
				public static LocString LOGIC_PORT = "4-Bit Input";

				// Token: 0x0400C293 RID: 49811
				public static LocString INPUT_NAME = "RIBBON INPUT";

				// Token: 0x0400C294 RID: 49812
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reads a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " from selected Bit";

				// Token: 0x0400C295 RID: 49813
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Reads a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " from selected Bit";

				// Token: 0x0400C296 RID: 49814
				public static LocString LOGIC_PORT_OUTPUT = "Bit Reading";

				// Token: 0x0400C297 RID: 49815
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400C298 RID: 49816
				public static LocString OUTPUT_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to attached ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE")
				});

				// Token: 0x0400C299 RID: 49817
				public static LocString OUTPUT_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Sends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to attached ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE")
				});
			}

			// Token: 0x02002C50 RID: 11344
			public class TRAVELTUBEENTRANCE
			{
				// Token: 0x0400C29A RID: 49818
				public static LocString NAME = UI.FormatAsLink("Transit Tube Access", "TRAVELTUBEENTRANCE");

				// Token: 0x0400C29B RID: 49819
				public static LocString DESC = "Duplicants require access points to enter tubes, but not to exit them.";

				// Token: 0x0400C29C RID: 49820
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to enter the connected ",
					UI.FormatAsLink("Transit Tube", "TRAVELTUBE"),
					" system.\n\nStops drawing ",
					UI.FormatAsLink("Power", "POWER"),
					" once fully charged."
				});
			}

			// Token: 0x02002C51 RID: 11345
			public class TRAVELTUBE
			{
				// Token: 0x0400C29D RID: 49821
				public static LocString NAME = UI.FormatAsLink("Transit Tube", "TRAVELTUBE");

				// Token: 0x0400C29E RID: 49822
				public static LocString DESC = "Duplicants will only exit a transit tube when a safe landing area is available beneath it.";

				// Token: 0x0400C29F RID: 49823
				public static LocString EFFECT = "Quickly transports Duplicants from a " + UI.FormatAsLink("Transit Tube Access", "TRAVELTUBEENTRANCE") + " to the tube's end.\n\nOnly transports Duplicants.";
			}

			// Token: 0x02002C52 RID: 11346
			public class TRAVELTUBEWALLBRIDGE
			{
				// Token: 0x0400C2A0 RID: 49824
				public static LocString NAME = UI.FormatAsLink("Transit Tube Crossing", "TRAVELTUBEWALLBRIDGE");

				// Token: 0x0400C2A1 RID: 49825
				public static LocString DESC = "Tube crossings can run transit tubes through walls without leaking gas or liquid.";

				// Token: 0x0400C2A2 RID: 49826
				public static LocString EFFECT = "Allows " + UI.FormatAsLink("Transit Tubes", "TRAVELTUBE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.";
			}

			// Token: 0x02002C53 RID: 11347
			public class SOLIDCONDUIT
			{
				// Token: 0x0400C2A3 RID: 49827
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT");

				// Token: 0x0400C2A4 RID: 49828
				public static LocString DESC = "Rails move materials where they'll be needed most, saving Duplicants the walk.";

				// Token: 0x0400C2A5 RID: 49829
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Transports ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" on a track between ",
					UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX"),
					" and ",
					UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002C54 RID: 11348
			public class SOLIDCONDUITINBOX
			{
				// Token: 0x0400C2A6 RID: 49830
				public static LocString NAME = UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX");

				// Token: 0x0400C2A7 RID: 49831
				public static LocString DESC = "Material filters can be used to determine what resources are sent down the rail.";

				// Token: 0x0400C2A8 RID: 49832
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" onto ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" for transport.\n\nOnly loads the resources of your choosing."
				});
			}

			// Token: 0x02002C55 RID: 11349
			public class SOLIDCONDUITOUTBOX
			{
				// Token: 0x0400C2A9 RID: 49833
				public static LocString NAME = UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX");

				// Token: 0x0400C2AA RID: 49834
				public static LocString DESC = "When materials reach the end of a rail they enter a receptacle to be used by Duplicants.";

				// Token: 0x0400C2AB RID: 49835
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" from a ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" into storage."
				});
			}

			// Token: 0x02002C56 RID: 11350
			public class SOLIDTRANSFERARM
			{
				// Token: 0x0400C2AC RID: 49836
				public static LocString NAME = UI.FormatAsLink("Auto-Sweeper", "SOLIDTRANSFERARM");

				// Token: 0x0400C2AD RID: 49837
				public static LocString DESC = "An auto-sweeper's range can be viewed at any time by " + UI.CLICK(UI.ClickType.clicking) + " on the building.";

				// Token: 0x0400C2AE RID: 49838
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automates ",
					UI.FormatAsLink("Sweeping", "CHORES"),
					" and ",
					UI.FormatAsLink("Supplying", "CHORES"),
					" errands by sucking up all nearby ",
					UI.FormatAsLink("Debris", "DECOR"),
					".\n\nMaterials are automatically delivered to any ",
					UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX"),
					", ",
					UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX"),
					", storage, or buildings within range."
				});
			}

			// Token: 0x02002C57 RID: 11351
			public class SOLIDCONDUITBRIDGE
			{
				// Token: 0x0400C2AF RID: 49839
				public static LocString NAME = UI.FormatAsLink("Conveyor Bridge", "SOLIDCONDUITBRIDGE");

				// Token: 0x0400C2B0 RID: 49840
				public static LocString DESC = "Separating rail systems helps ensure materials go to the intended destinations.";

				// Token: 0x0400C2B1 RID: 49841
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002C58 RID: 11352
			public class SOLIDVENT
			{
				// Token: 0x0400C2B2 RID: 49842
				public static LocString NAME = UI.FormatAsLink("Conveyor Chute", "SOLIDVENT");

				// Token: 0x0400C2B3 RID: 49843
				public static LocString DESC = "When materials reach the end of a rail they are dropped back into the world.";

				// Token: 0x0400C2B4 RID: 49844
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" from a ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" onto the floor."
				});
			}

			// Token: 0x02002C59 RID: 11353
			public class SOLIDLOGICVALVE
			{
				// Token: 0x0400C2B5 RID: 49845
				public static LocString NAME = UI.FormatAsLink("Conveyor Shutoff", "SOLIDLOGICVALVE");

				// Token: 0x0400C2B6 RID: 49846
				public static LocString DESC = "Automated conveyors save power and time by removing the need for Duplicant input.";

				// Token: 0x0400C2B7 RID: 49847
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" transport on or off."
				});

				// Token: 0x0400C2B8 RID: 49848
				public static LocString LOGIC_PORT = "Open/Close";

				// Token: 0x0400C2B9 RID: 49849
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow material transport";

				// Token: 0x0400C2BA RID: 49850
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent material transport";
			}

			// Token: 0x02002C5A RID: 11354
			public class SOLIDLIMITVALVE
			{
				// Token: 0x0400C2BB RID: 49851
				public static LocString NAME = UI.FormatAsLink("Conveyor Meter", "SOLIDLIMITVALVE");

				// Token: 0x0400C2BC RID: 49852
				public static LocString DESC = "Conveyor Meters let an exact amount of materials pass through before shutting off.";

				// Token: 0x0400C2BD RID: 49853
				public static LocString EFFECT = "Connects to an " + UI.FormatAsLink("Automation", "LOGIC") + " grid to automatically turn material transfer off when the specified amount has passed through it.";

				// Token: 0x0400C2BE RID: 49854
				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				// Token: 0x0400C2BF RID: 49855
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				// Token: 0x0400C2C0 RID: 49856
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400C2C1 RID: 49857
				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				// Token: 0x0400C2C2 RID: 49858
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				// Token: 0x0400C2C3 RID: 49859
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002C5B RID: 11355
			public class DEVPUMPSOLID
			{
				// Token: 0x0400C2C4 RID: 49860
				public static LocString NAME = "Dev Pump Solid";

				// Token: 0x0400C2C5 RID: 49861
				public static LocString DESC = "Piping a pump's output to a building's intake will send solids to that building.";

				// Token: 0x0400C2C6 RID: 49862
				public static LocString EFFECT = "Generates chosen " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " and runs it through " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT");
			}

			// Token: 0x02002C5C RID: 11356
			public class AUTOMINER
			{
				// Token: 0x0400C2C7 RID: 49863
				public static LocString NAME = UI.FormatAsLink("Robo-Miner", "AUTOMINER");

				// Token: 0x0400C2C8 RID: 49864
				public static LocString DESC = "A robo-miner's range can be viewed at any time by selecting the building.";

				// Token: 0x0400C2C9 RID: 49865
				public static LocString EFFECT = "Automatically digs out all materials in a set range.";
			}

			// Token: 0x02002C5D RID: 11357
			public class CREATUREFEEDER
			{
				// Token: 0x0400C2CA RID: 49866
				public static LocString NAME = UI.FormatAsLink("Critter Feeder", "CREATUREFEEDER");

				// Token: 0x0400C2CB RID: 49867
				public static LocString DESC = "Critters tend to stay close to their food source and wander less when given a feeder.";

				// Token: 0x0400C2CC RID: 49868
				public static LocString EFFECT = "Automatically dispenses food for hungry " + UI.FormatAsLink("Critters", "CREATURES") + ".";
			}

			// Token: 0x02002C5E RID: 11358
			public class GRAVITASPEDESTAL
			{
				// Token: 0x0400C2CD RID: 49869
				public static LocString NAME = UI.FormatAsLink("Gravitas Pedestal", "ITEMPEDESTAL");

				// Token: 0x0400C2CE RID: 49870
				public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";

				// Token: 0x0400C2CF RID: 49871
				public static LocString EFFECT = "Displays a single object, doubling its " + UI.FormatAsLink("Decor", "DECOR") + " value.\n\nObjects with negative Decor will gain some positive Decor when displayed.";

				// Token: 0x0400C2D0 RID: 49872
				public static LocString DISPLAYED_ITEM_FMT = "Displayed {0}";
			}

			// Token: 0x02002C5F RID: 11359
			public class ITEMPEDESTAL
			{
				// Token: 0x0400C2D1 RID: 49873
				public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

				// Token: 0x0400C2D2 RID: 49874
				public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";

				// Token: 0x0400C2D3 RID: 49875
				public static LocString EFFECT = "Displays a single object, doubling its " + UI.FormatAsLink("Decor", "DECOR") + " value.\n\nObjects with negative Decor will gain some positive Decor when displayed.";

				// Token: 0x0400C2D4 RID: 49876
				public static LocString DISPLAYED_ITEM_FMT = "Displayed {0}";

				// Token: 0x02003A7F RID: 14975
				public class FACADES
				{
					// Token: 0x02003F1F RID: 16159
					public class DEFAULT_ITEMPEDESTAL
					{
						// Token: 0x0400F70C RID: 63244
						public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

						// Token: 0x0400F70D RID: 63245
						public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";
					}

					// Token: 0x02003F20 RID: 16160
					public class HAND
					{
						// Token: 0x0400F70E RID: 63246
						public static LocString NAME = UI.FormatAsLink("Hand of Dupe Pedestal", "ITEMPEDESTAL");

						// Token: 0x0400F70F RID: 63247
						public static LocString DESC = "This pedestal cradles precious objects in the palm of its hand.";
					}
				}
			}

			// Token: 0x02002C60 RID: 11360
			public class CROWNMOULDING
			{
				// Token: 0x0400C2D5 RID: 49877
				public static LocString NAME = UI.FormatAsLink("Ceiling Trim", "CROWNMOULDING");

				// Token: 0x0400C2D6 RID: 49878
				public static LocString DESC = "Ceiling trim is a purely decorative addition to one's overhead area.";

				// Token: 0x0400C2D7 RID: 49879
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to decorate the ceilings of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x02003A80 RID: 14976
				public class FACADES
				{
					// Token: 0x02003F21 RID: 16161
					public class DEFAULT_CROWNMOULDING
					{
						// Token: 0x0400F710 RID: 63248
						public static LocString NAME = UI.FormatAsLink("Ceiling Trim", "CROWNMOULDING");

						// Token: 0x0400F711 RID: 63249
						public static LocString DESC = "Ceiling trim is a purely decorative addition to one's overhead area.";
					}

					// Token: 0x02003F22 RID: 16162
					public class SHINEORNAMENTS
					{
						// Token: 0x0400F712 RID: 63250
						public static LocString NAME = UI.FormatAsLink("Fancy Bug Ceiling Garland", "CROWNMOULDING");

						// Token: 0x0400F713 RID: 63251
						public static LocString DESC = "Someone spent their entire weekend gluing ribbons to paper Shine Bug cut-outs, and it shows.";
					}
				}
			}

			// Token: 0x02002C61 RID: 11361
			public class CORNERMOULDING
			{
				// Token: 0x0400C2D8 RID: 49880
				public static LocString NAME = UI.FormatAsLink("Corner Trim", "CORNERMOULDING");

				// Token: 0x0400C2D9 RID: 49881
				public static LocString DESC = "Corner trim is a purely decorative addition for ceiling corners.";

				// Token: 0x0400C2DA RID: 49882
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to decorate the ceiling corners of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x02003A81 RID: 14977
				public class FACADES
				{
					// Token: 0x02003F23 RID: 16163
					public class DEFAULT_CORNERMOULDING
					{
						// Token: 0x0400F714 RID: 63252
						public static LocString NAME = UI.FormatAsLink("Corner Trim", "CORNERMOULDING");

						// Token: 0x0400F715 RID: 63253
						public static LocString DESC = "It really dresses up a ceiling corner.";
					}

					// Token: 0x02003F24 RID: 16164
					public class SHINEORNAMENTS
					{
						// Token: 0x0400F716 RID: 63254
						public static LocString NAME = UI.FormatAsLink("Fancy Bug Corner Garland", "CORNERMOULDING");

						// Token: 0x0400F717 RID: 63255
						public static LocString DESC = "Why deck the halls, when you could <i>festoon</i> them?";
					}
				}
			}

			// Token: 0x02002C62 RID: 11362
			public class EGGINCUBATOR
			{
				// Token: 0x0400C2DB RID: 49883
				public static LocString NAME = UI.FormatAsLink("Incubator", "EGGINCUBATOR");

				// Token: 0x0400C2DC RID: 49884
				public static LocString DESC = "Incubators can maintain the ideal internal conditions for several species of critter egg.";

				// Token: 0x0400C2DD RID: 49885
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Incubates ",
					UI.FormatAsLink("Critter", "CREATURES"),
					" eggs until ready to hatch.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Critter Ranching", "RANCHING1"),
					" skill."
				});
			}

			// Token: 0x02002C63 RID: 11363
			public class EGGCRACKER
			{
				// Token: 0x0400C2DE RID: 49886
				public static LocString NAME = UI.FormatAsLink("Egg Cracker", "EGGCRACKER");

				// Token: 0x0400C2DF RID: 49887
				public static LocString DESC = "Raw eggs are an ingredient in certain high quality food recipes.";

				// Token: 0x0400C2E0 RID: 49888
				public static LocString EFFECT = "Converts viable " + UI.FormatAsLink("Critter", "CREATURES") + " eggs into cooking ingredients.\n\nCracked Eggs cannot hatch.\n\nDuplicants will not crack eggs unless tasks are queued.";

				// Token: 0x0400C2E1 RID: 49889
				public static LocString RECIPE_DESCRIPTION = "Turns {0} into {1}.";

				// Token: 0x0400C2E2 RID: 49890
				public static LocString RESULT_DESCRIPTION = "Cracked {0}";

				// Token: 0x02003A82 RID: 14978
				public class FACADES
				{
					// Token: 0x02003F25 RID: 16165
					public class DEFAULT_EGGCRACKER
					{
						// Token: 0x0400F718 RID: 63256
						public static LocString NAME = UI.FormatAsLink("Egg Cracker", "EGGCRACKER");

						// Token: 0x0400F719 RID: 63257
						public static LocString DESC = "It cracks eggs.";
					}

					// Token: 0x02003F26 RID: 16166
					public class BEAKER
					{
						// Token: 0x0400F71A RID: 63258
						public static LocString NAME = UI.FormatAsLink("Beaker Cracker", "EGGCRACKER");

						// Token: 0x0400F71B RID: 63259
						public static LocString DESC = "A practical exercise in physics.";
					}

					// Token: 0x02003F27 RID: 16167
					public class FLOWER
					{
						// Token: 0x0400F71C RID: 63260
						public static LocString NAME = UI.FormatAsLink("Blossom Cracker", "EGGCRACKER");

						// Token: 0x0400F71D RID: 63261
						public static LocString DESC = "Now with EZ-clean petals.";
					}

					// Token: 0x02003F28 RID: 16168
					public class HANDS
					{
						// Token: 0x0400F71E RID: 63262
						public static LocString NAME = UI.FormatAsLink("Handy Cracker", "EGGCRACKER");

						// Token: 0x0400F71F RID: 63263
						public static LocString DESC = "Just like Mi-Ma used to have.";
					}
				}
			}

			// Token: 0x02002C64 RID: 11364
			public class URANIUMCENTRIFUGE
			{
				// Token: 0x0400C2E3 RID: 49891
				public static LocString NAME = UI.FormatAsLink("Uranium Centrifuge", "URANIUMCENTRIFUGE");

				// Token: 0x0400C2E4 RID: 49892
				public static LocString DESC = "Enriched uranium is a specialized substance that can be used to fuel powerful research reactors.";

				// Token: 0x0400C2E5 RID: 49893
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Extracts ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" from ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					".\n\nOutputs ",
					UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM"),
					" in molten form."
				});

				// Token: 0x0400C2E6 RID: 49894
				public static LocString RECIPE_DESCRIPTION = "Convert Uranium ore to Molten Uranium and Enriched Uranium";
			}

			// Token: 0x02002C65 RID: 11365
			public class HIGHENERGYPARTICLEREDIRECTOR
			{
				// Token: 0x0400C2E7 RID: 49895
				public static LocString NAME = UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR");

				// Token: 0x0400C2E8 RID: 49896
				public static LocString DESC = "We were all out of mirrors.";

				// Token: 0x0400C2E9 RID: 49897
				public static LocString EFFECT = "Receives and redirects Radbolts from " + UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER") + ".";

				// Token: 0x0400C2EA RID: 49898
				public static LocString LOGIC_PORT = "Ignore incoming Radbolts";

				// Token: 0x0400C2EB RID: 49899
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow incoming Radbolts";

				// Token: 0x0400C2EC RID: 49900
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Ignore incoming Radbolts";
			}

			// Token: 0x02002C66 RID: 11366
			public class MANUALHIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x0400C2ED RID: 49901
				public static LocString NAME = UI.FormatAsLink("Manual Radbolt Generator", "MANUALHIGHENERGYPARTICLESPAWNER");

				// Token: 0x0400C2EE RID: 49902
				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				// Token: 0x0400C2EF RID: 49903
				public static LocString EFFECT = "Refines radioactive ores to generate Radbolts.\n\nEmits generated Radbolts in the direction of your choosing.";

				// Token: 0x0400C2F0 RID: 49904
				public static LocString RECIPE_DESCRIPTION = "Creates " + UI.FormatAsLink("Radbolts", "RADIATION") + " by processing {0}. Also creates {1} as a byproduct.";
			}

			// Token: 0x02002C67 RID: 11367
			public class HIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x0400C2F1 RID: 49905
				public static LocString NAME = UI.FormatAsLink("Radbolt Generator", "HIGHENERGYPARTICLESPAWNER");

				// Token: 0x0400C2F2 RID: 49906
				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				// Token: 0x0400C2F3 RID: 49907
				public static LocString EFFECT = "Attracts nearby " + UI.FormatAsLink("Radiation", "RADIATION") + " to generate Radbolts.\n\nEmits generated Radbolts in the direction of your choosing when the set Radbolt threshold is reached.\n\nRadbolts collected will gradually decay while this building is disabled.";

				// Token: 0x0400C2F4 RID: 49908
				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				// Token: 0x0400C2F5 RID: 49909
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				// Token: 0x0400C2F6 RID: 49910
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";
			}

			// Token: 0x02002C68 RID: 11368
			public class DEVHEPSPAWNER
			{
				// Token: 0x0400C2F7 RID: 49911
				public static LocString NAME = "Dev Radbolt Generator";

				// Token: 0x0400C2F8 RID: 49912
				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				// Token: 0x0400C2F9 RID: 49913
				public static LocString EFFECT = "Generates Radbolts.";

				// Token: 0x0400C2FA RID: 49914
				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				// Token: 0x0400C2FB RID: 49915
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				// Token: 0x0400C2FC RID: 49916
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";
			}

			// Token: 0x02002C69 RID: 11369
			public class HEPBATTERY
			{
				// Token: 0x0400C2FD RID: 49917
				public static LocString NAME = UI.FormatAsLink("Radbolt Chamber", "HEPBATTERY");

				// Token: 0x0400C2FE RID: 49918
				public static LocString DESC = "Particles packed up and ready to go.";

				// Token: 0x0400C2FF RID: 49919
				public static LocString EFFECT = "Stores Radbolts in a high-energy state, ready for transport.\n\nRequires a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to release radbolts from storage when the Radbolt threshold is reached.\n\nRadbolts in storage will rapidly decay while this building is disabled.";

				// Token: 0x0400C300 RID: 49920
				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				// Token: 0x0400C301 RID: 49921
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				// Token: 0x0400C302 RID: 49922
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";

				// Token: 0x0400C303 RID: 49923
				public static LocString LOGIC_PORT_STORAGE = "Radbolt Storage";

				// Token: 0x0400C304 RID: 49924
				public static LocString LOGIC_PORT_STORAGE_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its Radbolt Storage is full";

				// Token: 0x0400C305 RID: 49925
				public static LocString LOGIC_PORT_STORAGE_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002C6A RID: 11370
			public class HEPBRIDGETILE
			{
				// Token: 0x0400C306 RID: 49926
				public static LocString NAME = UI.FormatAsLink("Radbolt Joint Plate", "HEPBRIDGETILE");

				// Token: 0x0400C307 RID: 49927
				public static LocString DESC = "Allows Radbolts to pass through walls.";

				// Token: 0x0400C308 RID: 49928
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Receives ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" from ",
					UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER"),
					" and directs them through walls. All other materials and elements will be blocked from passage."
				});
			}

			// Token: 0x02002C6B RID: 11371
			public class ASTRONAUTTRAININGCENTER
			{
				// Token: 0x0400C309 RID: 49929
				public static LocString NAME = UI.FormatAsLink("Space Cadet Centrifuge", "ASTRONAUTTRAININGCENTER");

				// Token: 0x0400C30A RID: 49930
				public static LocString DESC = "Duplicants must complete astronaut training in order to pilot space rockets.";

				// Token: 0x0400C30B RID: 49931
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Trains Duplicants to become ",
					UI.FormatAsLink("Astronaut", "ROCKETPILOTING1"),
					".\n\nDuplicants must possess the ",
					UI.FormatAsLink("Astronaut", "ROCKETPILOTING1"),
					" trait to receive training."
				});
			}

			// Token: 0x02002C6C RID: 11372
			public class HOTTUB
			{
				// Token: 0x0400C30C RID: 49932
				public static LocString NAME = UI.FormatAsLink("Hot Tub", "HOTTUB");

				// Token: 0x0400C30D RID: 49933
				public static LocString DESC = "Relaxes Duplicants with massaging jets of heated liquid.";

				// Token: 0x0400C30E RID: 49934
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Requires ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					" to and from tub and ",
					UI.FormatAsLink("Power", "POWER"),
					" to run the jets.\n\nWater must be a comfortable temperature and will cool rapidly.\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and leaves them feeling deliciously warm."
				});

				// Token: 0x0400C30F RID: 49935
				public static LocString WATER_REQUIREMENT = "{element}: {amount}";

				// Token: 0x0400C310 RID: 49936
				public static LocString WATER_REQUIREMENT_TOOLTIP = "This building must be filled with {amount} {element} in order to function.";

				// Token: 0x0400C311 RID: 49937
				public static LocString TEMPERATURE_REQUIREMENT = "Minimum {element} Temperature: {temperature}";

				// Token: 0x0400C312 RID: 49938
				public static LocString TEMPERATURE_REQUIREMENT_TOOLTIP = "The Hot Tub will only be usable if supplied with {temperature} {element}. If the {element} gets too cold, the Hot Tub will drain and require refilling with {element}.";
			}

			// Token: 0x02002C6D RID: 11373
			public class SODAFOUNTAIN
			{
				// Token: 0x0400C313 RID: 49939
				public static LocString NAME = UI.FormatAsLink("Soda Fountain", "SODAFOUNTAIN");

				// Token: 0x0400C314 RID: 49940
				public static LocString DESC = "Sparkling water puts a twinkle in a Duplicant's eye.";

				// Token: 0x0400C315 RID: 49941
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates soda from ",
					UI.FormatAsLink("Water", "WATER"),
					" and ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					".\n\nConsuming soda water increases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x02002C6E RID: 11374
			public class UNCONSTRUCTEDROCKETMODULE
			{
				// Token: 0x0400C316 RID: 49942
				public static LocString NAME = "Empty Rocket Module";

				// Token: 0x0400C317 RID: 49943
				public static LocString DESC = "Something useful could be put here someday";

				// Token: 0x0400C318 RID: 49944
				public static LocString EFFECT = "Can be changed into a different rocket module";
			}

			// Token: 0x02002C6F RID: 11375
			public class MILKFATSEPARATOR
			{
				// Token: 0x0400C319 RID: 49945
				public static LocString NAME = UI.FormatAsLink("Brackwax Gleaner", "MILKFATSEPARATOR");

				// Token: 0x0400C31A RID: 49946
				public static LocString DESC = "Duplicants can slather up with brackwax to increase their travel speed in transit tubes.";

				// Token: 0x0400C31B RID: 49947
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					ELEMENTS.MILK.NAME,
					" into ",
					ELEMENTS.BRINE.NAME,
					" and ",
					ELEMENTS.MILKFAT.NAME,
					", and emits ",
					ELEMENTS.CARBONDIOXIDE.NAME,
					"."
				});
			}

			// Token: 0x02002C70 RID: 11376
			public class MILKFEEDER
			{
				// Token: 0x0400C31C RID: 49948
				public static LocString NAME = UI.FormatAsLink("Critter Fountain", "MILKFEEDER");

				// Token: 0x0400C31D RID: 49949
				public static LocString DESC = "It's easier to tolerate overcrowding when you're all hopped up on brackene.";

				// Token: 0x0400C31E RID: 49950
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Dispenses ",
					ELEMENTS.MILK.NAME,
					" to a wide variety of ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					".\n\nAccessing the fountain significantly improves ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					"' moods."
				});
			}

			// Token: 0x02002C71 RID: 11377
			public class MILKINGSTATION
			{
				// Token: 0x0400C31F RID: 49951
				public static LocString NAME = UI.FormatAsLink("Milking Station", "MILKINGSTATION");

				// Token: 0x0400C320 RID: 49952
				public static LocString DESC = "The harvested liquid is basically the equivalent of soda for critters.";

				// Token: 0x0400C321 RID: 49953
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants with the ",
					UI.FormatAsLink("Critter Ranching II", "RANCHING2"),
					" skill to milk ",
					UI.FormatAsLink("Gassy Moos", "MOO"),
					" for ",
					ELEMENTS.MILK.NAME,
					".\n\n",
					ELEMENTS.MILK.NAME,
					" can be used to refill the ",
					BUILDINGS.PREFABS.MILKFEEDER.NAME,
					"."
				});
			}

			// Token: 0x02002C72 RID: 11378
			public class MODULARLAUNCHPADPORT
			{
				// Token: 0x0400C322 RID: 49954
				public static LocString NAME = UI.FormatAsLink("Rocket Port", "MODULARLAUNCHPADPORTSOLID");

				// Token: 0x0400C323 RID: 49955
				public static LocString NAME_PLURAL = UI.FormatAsLink("Rocket Ports", "MODULARLAUNCHPADPORTSOLID");
			}

			// Token: 0x02002C73 RID: 11379
			public class MODULARLAUNCHPADPORTGAS
			{
				// Token: 0x0400C324 RID: 49956
				public static LocString NAME = UI.FormatAsLink("Gas Rocket Port Loader", "MODULARLAUNCHPADPORTGAS");

				// Token: 0x0400C325 RID: 49957
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400C326 RID: 49958
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the gas filters set on the rocket's cargo bays."
				});
			}

			// Token: 0x02002C74 RID: 11380
			public class MODULARLAUNCHPADPORTBRIDGE
			{
				// Token: 0x0400C327 RID: 49959
				public static LocString NAME = UI.FormatAsLink("Rocket Port Extension", "MODULARLAUNCHPADPORTBRIDGE");

				// Token: 0x0400C328 RID: 49960
				public static LocString DESC = "Allows rocket platforms to be built farther apart.";

				// Token: 0x0400C329 RID: 49961
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or any ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					"."
				});
			}

			// Token: 0x02002C75 RID: 11381
			public class MODULARLAUNCHPADPORTLIQUID
			{
				// Token: 0x0400C32A RID: 49962
				public static LocString NAME = UI.FormatAsLink("Liquid Rocket Port Loader", "MODULARLAUNCHPADPORTLIQUID");

				// Token: 0x0400C32B RID: 49963
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400C32C RID: 49964
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the liquid filters set on the rocket's cargo bays."
				});
			}

			// Token: 0x02002C76 RID: 11382
			public class MODULARLAUNCHPADPORTSOLID
			{
				// Token: 0x0400C32D RID: 49965
				public static LocString NAME = UI.FormatAsLink("Solid Rocket Port Loader", "MODULARLAUNCHPADPORTSOLID");

				// Token: 0x0400C32E RID: 49966
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400C32F RID: 49967
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the solid material filters set on the rocket's cargo bays."
				});
			}

			// Token: 0x02002C77 RID: 11383
			public class MODULARLAUNCHPADPORTGASUNLOADER
			{
				// Token: 0x0400C330 RID: 49968
				public static LocString NAME = UI.FormatAsLink("Gas Rocket Port Unloader", "MODULARLAUNCHPADPORTGASUNLOADER");

				// Token: 0x0400C331 RID: 49969
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400C332 RID: 49970
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the gas filters set on this unloader."
				});
			}

			// Token: 0x02002C78 RID: 11384
			public class MODULARLAUNCHPADPORTLIQUIDUNLOADER
			{
				// Token: 0x0400C333 RID: 49971
				public static LocString NAME = UI.FormatAsLink("Liquid Rocket Port Unloader", "MODULARLAUNCHPADPORTLIQUIDUNLOADER");

				// Token: 0x0400C334 RID: 49972
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400C335 RID: 49973
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the liquid filters set on this unloader."
				});
			}

			// Token: 0x02002C79 RID: 11385
			public class MODULARLAUNCHPADPORTSOLIDUNLOADER
			{
				// Token: 0x0400C336 RID: 49974
				public static LocString NAME = UI.FormatAsLink("Solid Rocket Port Unloader", "MODULARLAUNCHPADPORTSOLIDUNLOADER");

				// Token: 0x0400C337 RID: 49975
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400C338 RID: 49976
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the solid material filters set on this unloader."
				});
			}

			// Token: 0x02002C7A RID: 11386
			public class STICKERBOMB
			{
				// Token: 0x0400C339 RID: 49977
				public static LocString NAME = UI.FormatAsLink("Sticker Bomb", "STICKERBOMB");

				// Token: 0x0400C33A RID: 49978
				public static LocString DESC = "Surprise decor sneak attacks a Duplicant's gloomy day.";
			}

			// Token: 0x02002C7B RID: 11387
			public class HEATCOMPRESSOR
			{
				// Token: 0x0400C33B RID: 49979
				public static LocString NAME = UI.FormatAsLink("Liquid Heatquilizer", "HEATCOMPRESSOR");

				// Token: 0x0400C33C RID: 49980
				public static LocString DESC = "\"Room temperature\" is relative, really.";

				// Token: 0x0400C33D RID: 49981
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Heats or cools ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" to match ambient ",
					UI.FormatAsLink("Air Temperature", "HEAT"),
					"."
				});
			}

			// Token: 0x02002C7C RID: 11388
			public class PARTYCAKE
			{
				// Token: 0x0400C33E RID: 49982
				public static LocString NAME = UI.FormatAsLink("Triple Decker Cake", "PARTYCAKE");

				// Token: 0x0400C33F RID: 49983
				public static LocString DESC = "Any way you slice it, that's a good looking cake.";

				// Token: 0x0400C340 RID: 49984
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nAdds a ",
					UI.FormatAsLink("Morale", "MORALE"),
					" bonus to Duplicants' parties."
				});
			}

			// Token: 0x02002C7D RID: 11389
			public class RAILGUN
			{
				// Token: 0x0400C341 RID: 49985
				public static LocString NAME = UI.FormatAsLink("Interplanetary Launcher", "RAILGUN");

				// Token: 0x0400C342 RID: 49986
				public static LocString DESC = "It's tempting to climb inside but trust me... don't.";

				// Token: 0x0400C343 RID: 49987
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Launches ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" between Planetoids.\n\nPayloads can contain ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" materials.\n\nCannot transport Duplicants."
				});

				// Token: 0x0400C344 RID: 49988
				public static LocString SIDESCREEN_HEP_REQUIRED = "Launch cost: {current} / {required} radbolts";

				// Token: 0x0400C345 RID: 49989
				public static LocString LOGIC_PORT = "Launch Toggle";

				// Token: 0x0400C346 RID: 49990
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable payload launching.";

				// Token: 0x0400C347 RID: 49991
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable payload launching.";
			}

			// Token: 0x02002C7E RID: 11390
			public class RAILGUNPAYLOADOPENER
			{
				// Token: 0x0400C348 RID: 49992
				public static LocString NAME = UI.FormatAsLink("Payload Opener", "RAILGUNPAYLOADOPENER");

				// Token: 0x0400C349 RID: 49993
				public static LocString DESC = "Payload openers can be hooked up to conveyors, plumbing and ventilation for improved sorting.";

				// Token: 0x0400C34A RID: 49994
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unpacks ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" delivered by Duplicants.\n\nAutomatically separates ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" materials and distributes them to the appropriate systems."
				});
			}

			// Token: 0x02002C7F RID: 11391
			public class LANDINGBEACON
			{
				// Token: 0x0400C34B RID: 49995
				public static LocString NAME = UI.FormatAsLink("Targeting Beacon", "LANDINGBEACON");

				// Token: 0x0400C34C RID: 49996
				public static LocString DESC = "Microtarget where your " + UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD") + " lands on a Planetoid surface.";

				// Token: 0x0400C34D RID: 49997
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Guides ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" and ",
					UI.FormatAsLink("Orbital Cargo Modules", "ORBITALCARGOMODULE"),
					" to land nearby.\n\n",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" must be launched from a ",
					UI.FormatAsLink("Interplanetary Launcher", "RAILGUN"),
					"."
				});
			}

			// Token: 0x02002C80 RID: 11392
			public class DIAMONDPRESS
			{
				// Token: 0x0400C34E RID: 49998
				public static LocString NAME = UI.FormatAsLink("Diamond Press", "DIAMONDPRESS");

				// Token: 0x0400C34F RID: 49999
				public static LocString DESC = "Crushes refined carbon into diamond.";

				// Token: 0x0400C350 RID: 50000
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Power", "POWER"),
					" and ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" to crush ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					" into ",
					UI.FormatAsLink("Diamond", "DIAMOND"),
					".\n\nDuplicants will not fabricate items unless recipes are queued and ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					" has been discovered."
				});

				// Token: 0x0400C351 RID: 50001
				public static LocString REFINED_CARBON_RECIPE_DESCRIPTION = "Converts {1} to {0}";
			}

			// Token: 0x02002C81 RID: 11393
			public class ESCAPEPOD
			{
				// Token: 0x0400C352 RID: 50002
				public static LocString NAME = UI.FormatAsLink("Escape Pod", "ESCAPEPOD");

				// Token: 0x0400C353 RID: 50003
				public static LocString DESC = "Delivers a Duplicant from a stranded rocket to the nearest Planetoid.";
			}

			// Token: 0x02002C82 RID: 11394
			public class ROCKETINTERIORLIQUIDOUTPUTPORT
			{
				// Token: 0x0400C354 RID: 50004
				public static LocString NAME = UI.FormatAsLink("Liquid Spacefarer Output Port", "ROCKETINTERIORLIQUIDOUTPUTPORT");

				// Token: 0x0400C355 RID: 50005
				public static LocString DESC = "A direct attachment to the input port on the exterior of a rocket.";

				// Token: 0x0400C356 RID: 50006
				public static LocString EFFECT = "Allows a direct conduit connection into the " + UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM") + " of a rocket.";
			}

			// Token: 0x02002C83 RID: 11395
			public class ROCKETINTERIORLIQUIDINPUTPORT
			{
				// Token: 0x0400C357 RID: 50007
				public static LocString NAME = UI.FormatAsLink("Liquid Spacefarer Input Port", "ROCKETINTERIORLIQUIDINPUTPORT");

				// Token: 0x0400C358 RID: 50008
				public static LocString DESC = "A direct attachment to the output port on the exterior of a rocket.";

				// Token: 0x0400C359 RID: 50009
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows a direct conduit connection out of the ",
					UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM"),
					" of a rocket.\nCan be used to vent ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to space during flight."
				});
			}

			// Token: 0x02002C84 RID: 11396
			public class ROCKETINTERIORGASOUTPUTPORT
			{
				// Token: 0x0400C35A RID: 50010
				public static LocString NAME = UI.FormatAsLink("Gas Spacefarer Output Port", "ROCKETINTERIORGASOUTPUTPORT");

				// Token: 0x0400C35B RID: 50011
				public static LocString DESC = "A direct attachment to the input port on the exterior of a rocket.";

				// Token: 0x0400C35C RID: 50012
				public static LocString EFFECT = "Allows a direct conduit connection into the " + UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM") + " of a rocket.";
			}

			// Token: 0x02002C85 RID: 11397
			public class ROCKETINTERIORGASINPUTPORT
			{
				// Token: 0x0400C35D RID: 50013
				public static LocString NAME = UI.FormatAsLink("Gas Spacefarer Input Port", "ROCKETINTERIORGASINPUTPORT");

				// Token: 0x0400C35E RID: 50014
				public static LocString DESC = "A direct attachment leading to the output port on the exterior of the rocket.";

				// Token: 0x0400C35F RID: 50015
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows a direct conduit connection out of the ",
					UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM"),
					" of the rocket.\nCan be used to vent ",
					UI.FormatAsLink("Gasses", "ELEMENTS_GAS"),
					" to space during flight."
				});
			}

			// Token: 0x02002C86 RID: 11398
			public class MISSILELAUNCHER
			{
				// Token: 0x0400C360 RID: 50016
				public static LocString NAME = UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER");

				// Token: 0x0400C361 RID: 50017
				public static LocString DESC = "Some meteors drop harvestable resources when they're blown to smithereens.";

				// Token: 0x0400C362 RID: 50018
				public static LocString EFFECT = "Fires explosive projectiles at incoming space objects to defend the colony from impact-related damage.\n\nProjectiles must be crafted at a " + UI.FormatAsLink("Blastshot Maker", "MISSILEFABRICATOR") + ".\n\nRange: 16 tiles horizontally, 32 tiles vertically.";

				// Token: 0x0400C363 RID: 50019
				public static LocString TARGET_SELECTION_HEADER = "Short Range Target Selection";

				// Token: 0x02003A83 RID: 14979
				public class BODY
				{
					// Token: 0x0400EBB9 RID: 60345
					public static LocString CONTAINER1 = "Fires " + UI.FormatAsLink("Blastshot", "MISSILELAUNCHER") + " shells at meteor showers to defend the colony from impact-related damage.\n\nRange: 16 tiles horizontally, 32 tiles vertically.\n\nMeteors that have been blown to smithereens leave behind no harvestable resources.";
				}
			}

			// Token: 0x02002C87 RID: 11399
			public class CRITTERCONDO
			{
				// Token: 0x0400C364 RID: 50020
				public static LocString NAME = UI.FormatAsLink("Critter Condo", "CRITTERCONDO");

				// Token: 0x0400C365 RID: 50021
				public static LocString DESC = "It's nice to have nice things.";

				// Token: 0x0400C366 RID: 50022
				public static LocString EFFECT = "Provides a comfortable lounge area that boosts " + UI.FormatAsLink("Critter", "CREATURES") + " happiness.";
			}

			// Token: 0x02002C88 RID: 11400
			public class UNDERWATERCRITTERCONDO
			{
				// Token: 0x0400C367 RID: 50023
				public static LocString NAME = UI.FormatAsLink("Water Fort", "UNDERWATERCRITTERCONDO");

				// Token: 0x0400C368 RID: 50024
				public static LocString DESC = "Even wild critters are happier after they've had a little R&R.";

				// Token: 0x0400C369 RID: 50025
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A fancy respite area for adult ",
					UI.FormatAsLink("Pokeshells", "CRABSPECIES"),
					" and ",
					UI.FormatAsLink("Pacu", "PACUSPECIES"),
					"."
				});
			}

			// Token: 0x02002C89 RID: 11401
			public class AIRBORNECRITTERCONDO
			{
				// Token: 0x0400C36A RID: 50026
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Condo", "AIRBORNECRITTERCONDO");

				// Token: 0x0400C36B RID: 50027
				public static LocString DESC = "Triggers natural nesting instincts and improves critters' moods.";

				// Token: 0x0400C36C RID: 50028
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A hanging respite area for adult ",
					UI.FormatAsLink("Pufts", "PUFT"),
					", ",
					UI.FormatAsLink("Gassy Moos", "MOOSPECIES"),
					" and ",
					UI.FormatAsLink("Shine Bugs", "LIGHTBUG"),
					"."
				});
			}

			// Token: 0x02002C8A RID: 11402
			public class MASSIVEHEATSINK
			{
				// Token: 0x0400C36D RID: 50029
				public static LocString NAME = UI.FormatAsLink("Anti Entropy Thermo-Nullifier", "MASSIVEHEATSINK");

				// Token: 0x0400C36E RID: 50030
				public static LocString DESC = "";

				// Token: 0x0400C36F RID: 50031
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A self-sustaining machine powered by what appears to be refined ",
					UI.FormatAsLink("Neutronium", "UNOBTANIUM"),
					".\n\nAbsorbs and neutralizes ",
					UI.FormatAsLink("Heat", "HEAT"),
					" energy when provided with piped ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					"."
				});
			}

			// Token: 0x02002C8B RID: 11403
			public class MEGABRAINTANK
			{
				// Token: 0x0400C370 RID: 50032
				public static LocString NAME = UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK");

				// Token: 0x0400C371 RID: 50033
				public static LocString DESC = "";

				// Token: 0x0400C372 RID: 50034
				public static LocString EFFECT = string.Concat(new string[]
				{
					"An organic multi-cortex repository and processing system fuelled by ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nAnalyzes ",
					UI.FormatAsLink("Dream Journals", "DREAMJOURNAL"),
					" produced by Duplicants wearing ",
					UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS"),
					".\n\nProvides a sustainable boost to Duplicant skills and abilities throughout the colony."
				});
			}

			// Token: 0x02002C8C RID: 11404
			public class GRAVITASCREATUREMANIPULATOR
			{
				// Token: 0x0400C373 RID: 50035
				public static LocString NAME = UI.FormatAsLink("Critter Flux-O-Matic", "GRAVITASCREATUREMANIPULATOR");

				// Token: 0x0400C374 RID: 50036
				public static LocString DESC = "";

				// Token: 0x0400C375 RID: 50037
				public static LocString EFFECT = "An experimental DNA manipulator.\n\nAnalyzes " + UI.FormatAsLink("Critters", "CREATURES") + " to transform base morphs into random variants of their species.";
			}

			// Token: 0x02002C8D RID: 11405
			public class HIJACKEDHEADQUARTERS
			{
				// Token: 0x0400C376 RID: 50038
				public static LocString NAME = UI.FormatAsLink("Printerceptor", "HIJACKEDHEADQUARTERS");

				// Token: 0x0400C377 RID: 50039
				public static LocString DESC = "The access code required to reboot it for testing is located somewhere on this world.";

				// Token: 0x0400C378 RID: 50040
				public static LocString EFFECT = "An unsanctioned bioprinter that runs on power siphoned from the " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + ".";
			}

			// Token: 0x02002C8E RID: 11406
			public class HIJACKEDHEADQUARTERS_COMPLETED
			{
				// Token: 0x0400C379 RID: 50041
				public static LocString NAME = UI.FormatAsLink("Printerceptor", "HIJACKEDHEADQUARTERS");

				// Token: 0x0400C37A RID: 50042
				public static LocString DESC = "";

				// Token: 0x0400C37B RID: 50043
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Intercepts and stores ",
					CODEX.POWER.TITLE,
					" charges from the ",
					BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME,
					".\n\nConverts stored charges and ",
					UI.FormatAsLink(DatabankHelper.NAME_PLURAL, "Databank"),
					" into ",
					UI.FormatAsLink("Seeds", "CREATURES"),
					" and ",
					UI.FormatAsLink("Eggs", "CREATURES"),
					"."
				});
			}

			// Token: 0x02002C8F RID: 11407
			public class FACILITYBACKWALLWINDOW
			{
				// Token: 0x0400C37C RID: 50044
				public static LocString NAME = "Window";

				// Token: 0x0400C37D RID: 50045
				public static LocString DESC = "";

				// Token: 0x0400C37E RID: 50046
				public static LocString EFFECT = "A tall, thin window.";
			}

			// Token: 0x02002C90 RID: 11408
			public class POIBUNKEREXTERIORDOOR
			{
				// Token: 0x0400C37F RID: 50047
				public static LocString NAME = "Security Door";

				// Token: 0x0400C380 RID: 50048
				public static LocString EFFECT = "A strong door with a sophisticated genetic lock.";

				// Token: 0x0400C381 RID: 50049
				public static LocString DESC = "";
			}

			// Token: 0x02002C91 RID: 11409
			public class POIDOORINTERNAL
			{
				// Token: 0x0400C382 RID: 50050
				public static LocString NAME = "Security Door";

				// Token: 0x0400C383 RID: 50051
				public static LocString EFFECT = "A strong door with a sophisticated genetic lock.";

				// Token: 0x0400C384 RID: 50052
				public static LocString DESC = "";
			}

			// Token: 0x02002C92 RID: 11410
			public class POIFACILITYDOOR
			{
				// Token: 0x0400C385 RID: 50053
				public static LocString NAME = "Lobby Doors";

				// Token: 0x0400C386 RID: 50054
				public static LocString EFFECT = "Large double doors that were once the main entrance to a large facility.";

				// Token: 0x0400C387 RID: 50055
				public static LocString DESC = "";
			}

			// Token: 0x02002C93 RID: 11411
			public class POIDLC2SHOWROOMDOOR
			{
				// Token: 0x0400C388 RID: 50056
				public static LocString NAME = "Showroom Doors";

				// Token: 0x0400C389 RID: 50057
				public static LocString EFFECT = "Large double doors identical to those you might find at the main entrance to a large facility.";

				// Token: 0x0400C38A RID: 50058
				public static LocString DESC = "";
			}

			// Token: 0x02002C94 RID: 11412
			public class VENDINGMACHINE
			{
				// Token: 0x0400C38B RID: 50059
				public static LocString NAME = "Vending Machine";

				// Token: 0x0400C38C RID: 50060
				public static LocString DESC = "A pristine " + UI.FormatAsLink("Nutrient Bar", "FIELDRATION") + " dispenser.";
			}

			// Token: 0x02002C95 RID: 11413
			public class GENESHUFFLER
			{
				// Token: 0x0400C38D RID: 50061
				public static LocString NAME = "Neural Vacillator";

				// Token: 0x0400C38E RID: 50062
				public static LocString DESC = "A massive synthetic brain, suspended in saline solution.\n\nThere is a chair attached to the device with room for one person.";
			}

			// Token: 0x02002C96 RID: 11414
			public class PROPTALLPLANT
			{
				// Token: 0x0400C38F RID: 50063
				public static LocString NAME = "Potted Plant";

				// Token: 0x0400C390 RID: 50064
				public static LocString DESC = "Looking closely, it appears to be fake.";
			}

			// Token: 0x02002C97 RID: 11415
			public class PROPTABLE
			{
				// Token: 0x0400C391 RID: 50065
				public static LocString NAME = "Table";

				// Token: 0x0400C392 RID: 50066
				public static LocString DESC = "A table and some chairs.";
			}

			// Token: 0x02002C98 RID: 11416
			public class PROPDESK
			{
				// Token: 0x0400C393 RID: 50067
				public static LocString NAME = "Computer Desk";

				// Token: 0x0400C394 RID: 50068
				public static LocString DESC = "An intact office desk, decorated with several personal belongings and a barely functioning computer.";
			}

			// Token: 0x02002C99 RID: 11417
			public class PROPFACILITYCHAIR
			{
				// Token: 0x0400C395 RID: 50069
				public static LocString NAME = "Lobby Chair";

				// Token: 0x0400C396 RID: 50070
				public static LocString DESC = "A chair where visitors can comfortably wait before their appointments.";
			}

			// Token: 0x02002C9A RID: 11418
			public class PROPFACILITYCOUCH
			{
				// Token: 0x0400C397 RID: 50071
				public static LocString NAME = "Lobby Couch";

				// Token: 0x0400C398 RID: 50072
				public static LocString DESC = "A couch where visitors can comfortably wait before their appointments.";
			}

			// Token: 0x02002C9B RID: 11419
			public class PROPFACILITYDESK
			{
				// Token: 0x0400C399 RID: 50073
				public static LocString NAME = "Director's Desk";

				// Token: 0x0400C39A RID: 50074
				public static LocString DESC = "A spotless desk filled with impeccably organized office supplies.\n\nA photo peeks out from beneath the desk pad, depicting two beaming young women in caps and gowns.\n\nThe photo is quite old.";
			}

			// Token: 0x02002C9C RID: 11420
			public class PROPFACILITYTABLE
			{
				// Token: 0x0400C39B RID: 50075
				public static LocString NAME = "Coffee Table";

				// Token: 0x0400C39C RID: 50076
				public static LocString DESC = "A low coffee table that may have once held old science magazines.";
			}

			// Token: 0x02002C9D RID: 11421
			public class PROPFACILITYSTATUE
			{
				// Token: 0x0400C39D RID: 50077
				public static LocString NAME = "Gravitas Monument";

				// Token: 0x0400C39E RID: 50078
				public static LocString DESC = "A large, modern sculpture that sits in the center of the lobby.\n\nIt's an artistic cross between an hourglass shape and a double helix.";
			}

			// Token: 0x02002C9E RID: 11422
			public class PROPFACILITYCHANDELIER
			{
				// Token: 0x0400C39F RID: 50079
				public static LocString NAME = "Chandelier";

				// Token: 0x0400C3A0 RID: 50080
				public static LocString DESC = "A large chandelier that hangs from the ceiling.\n\nIt does not appear to function.";
			}

			// Token: 0x02002C9F RID: 11423
			public class PROPFACILITYGLOBEDROORS
			{
				// Token: 0x0400C3A1 RID: 50081
				public static LocString NAME = "Filing Cabinet";

				// Token: 0x0400C3A2 RID: 50082
				public static LocString DESC = "A filing cabinet for storing hard copy employee records.\n\nThe contents have been shredded.";
			}

			// Token: 0x02002CA0 RID: 11424
			public class PROPFACILITYDISPLAY1
			{
				// Token: 0x0400C3A3 RID: 50083
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400C3A4 RID: 50084
				public static LocString DESC = "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Printing Pod.";
			}

			// Token: 0x02002CA1 RID: 11425
			public class PROPFACILITYDISPLAY2
			{
				// Token: 0x0400C3A5 RID: 50085
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400C3A6 RID: 50086
				public static LocString DESC = "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Mining Gun.";
			}

			// Token: 0x02002CA2 RID: 11426
			public class PROPFACILITYDISPLAY3
			{
				// Token: 0x0400C3A7 RID: 50087
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400C3A8 RID: 50088
				public static LocString DESC = "An electronic display projecting the blueprint of a strange device.\n\nPerhaps these displays were used to entice visitors.";
			}

			// Token: 0x02002CA3 RID: 11427
			public class PROPFACILITYTALLPLANT
			{
				// Token: 0x0400C3A9 RID: 50089
				public static LocString NAME = "Office Plant";

				// Token: 0x0400C3AA RID: 50090
				public static LocString DESC = "It's survived the vacuum of space by virtue of being plastic.";
			}

			// Token: 0x02002CA4 RID: 11428
			public class PROPFACILITYLAMP
			{
				// Token: 0x0400C3AB RID: 50091
				public static LocString NAME = "Light Fixture";

				// Token: 0x0400C3AC RID: 50092
				public static LocString DESC = "A long light fixture that hangs from the ceiling.\n\nIt does not appear to function.";
			}

			// Token: 0x02002CA5 RID: 11429
			public class PROPFACILITYWALLDEGREE
			{
				// Token: 0x0400C3AD RID: 50093
				public static LocString NAME = "Doctorate Degree";

				// Token: 0x0400C3AE RID: 50094
				public static LocString DESC = "Certification in Applied Physics, awarded in recognition of one \"Jacquelyn A. Stern\".";
			}

			// Token: 0x02002CA6 RID: 11430
			public class PROPFACILITYPAINTING
			{
				// Token: 0x0400C3AF RID: 50095
				public static LocString NAME = "Landscape Portrait";

				// Token: 0x0400C3B0 RID: 50096
				public static LocString DESC = "A painting featuring a copse of fir trees and a magnificent mountain range on the horizon.\n\nThe air in the room prickles with the sensation that I'm not meant to be here.";
			}

			// Token: 0x02002CA7 RID: 11431
			public class PROPRECEPTIONDESK
			{
				// Token: 0x0400C3B1 RID: 50097
				public static LocString NAME = "Reception Desk";

				// Token: 0x0400C3B2 RID: 50098
				public static LocString DESC = "A full coffee cup and a note abandoned mid sentence sit behind the desk.\n\nIt gives me an eerie feeling, as if the receptionist has stepped out and will return any moment.";
			}

			// Token: 0x02002CA8 RID: 11432
			public class PROPELEVATOR
			{
				// Token: 0x0400C3B3 RID: 50099
				public static LocString NAME = "Broken Elevator";

				// Token: 0x0400C3B4 RID: 50100
				public static LocString DESC = "Out of service.\n\nThe buttons inside indicate it went down more than a dozen floors at one point in time.";
			}

			// Token: 0x02002CA9 RID: 11433
			public class SETLOCKER
			{
				// Token: 0x0400C3B5 RID: 50101
				public static LocString NAME = "Locker";

				// Token: 0x0400C3B6 RID: 50102
				public static LocString DESC = "A basic metal locker.\n\nIt contains an assortment of personal effects.";
			}

			// Token: 0x02002CAA RID: 11434
			public class PROPEXOSETLOCKER
			{
				// Token: 0x0400C3B7 RID: 50103
				public static LocString NAME = "Off-site Locker";

				// Token: 0x0400C3B8 RID: 50104
				public static LocString DESC = "A locker made with ultra-lightweight textiles.\n\nIt contains an assortment of personal effects.";
			}

			// Token: 0x02002CAB RID: 11435
			public class MISSILESETLOCKER
			{
				// Token: 0x0400C3B9 RID: 50105
				public static LocString NAME = "Explosives Locker";

				// Token: 0x0400C3BA RID: 50106
				public static LocString DESC = "A locker that once belonged to an explosives engineer.\n\nIt holds one " + UI.FormatAsLink("Intracosmic Blastshot", "MISSILELAUNCHER") + ".";
			}

			// Token: 0x02002CAC RID: 11436
			public class PROPGRAVITASSMALLSEEDLOCKER
			{
				// Token: 0x0400C3BB RID: 50107
				public static LocString NAME = "Wall Cabinet";

				// Token: 0x0400C3BC RID: 50108
				public static LocString DESC = "A small glass cabinet.\n\nThere's a biohazard symbol on it.";
			}

			// Token: 0x02002CAD RID: 11437
			public class PROPLIGHT
			{
				// Token: 0x0400C3BD RID: 50109
				public static LocString NAME = "Light Fixture";

				// Token: 0x0400C3BE RID: 50110
				public static LocString DESC = "An elegant ceiling lamp, slightly worse for wear.";
			}

			// Token: 0x02002CAE RID: 11438
			public class PROPLADDER
			{
				// Token: 0x0400C3BF RID: 50111
				public static LocString NAME = "Ladder";

				// Token: 0x0400C3C0 RID: 50112
				public static LocString DESC = "A hard plastic ladder.";
			}

			// Token: 0x02002CAF RID: 11439
			public class PROPSKELETON
			{
				// Token: 0x0400C3C1 RID: 50113
				public static LocString NAME = "Model Skeleton";

				// Token: 0x0400C3C2 RID: 50114
				public static LocString DESC = "A detailed anatomical model.\n\nIt appears to be made of resin.";
			}

			// Token: 0x02002CB0 RID: 11440
			public class PROPSURFACESATELLITE1
			{
				// Token: 0x0400C3C3 RID: 50115
				public static LocString NAME = "Crashed Satellite";

				// Token: 0x0400C3C4 RID: 50116
				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			// Token: 0x02002CB1 RID: 11441
			public class PROPSURFACESATELLITE2
			{
				// Token: 0x0400C3C5 RID: 50117
				public static LocString NAME = "Wrecked Satellite";

				// Token: 0x0400C3C6 RID: 50118
				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			// Token: 0x02002CB2 RID: 11442
			public class PROPSURFACESATELLITE3
			{
				// Token: 0x0400C3C7 RID: 50119
				public static LocString NAME = "Crushed Satellite";

				// Token: 0x0400C3C8 RID: 50120
				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			// Token: 0x02002CB3 RID: 11443
			public class PROPCLOCK
			{
				// Token: 0x0400C3C9 RID: 50121
				public static LocString NAME = "Clock";

				// Token: 0x0400C3CA RID: 50122
				public static LocString DESC = "A simple wall clock.\n\nIt is no longer ticking.";
			}

			// Token: 0x02002CB4 RID: 11444
			public class PROPGRAVITASDECORATIVEWINDOW
			{
				// Token: 0x0400C3CB RID: 50123
				public static LocString NAME = "Window";

				// Token: 0x0400C3CC RID: 50124
				public static LocString DESC = "A tall, thin window which once pointed to a courtyard.";
			}

			// Token: 0x02002CB5 RID: 11445
			public class PROPGRAVITASDESK
			{
				// Token: 0x0400C3CD RID: 50125
				public static LocString NAME = "Biophysics Research Desk";

				// Token: 0x0400C3CE RID: 50126
				public static LocString DESC = "The unkempt workspace of a long-departed scientist who expected to return.";
			}

			// Token: 0x02002CB6 RID: 11446
			public class PROPGRAVITASFRIDGE
			{
				// Token: 0x0400C3CF RID: 50127
				public static LocString NAME = "Mini Fridge";

				// Token: 0x0400C3D0 RID: 50128
				public static LocString DESC = "A non-functional cold storage unit full of expired samples.\n\nIt was originally someone's home appliance.";
			}

			// Token: 0x02002CB7 RID: 11447
			public class PROPGRAVITASCLOCKSQUARE
			{
				// Token: 0x0400C3D1 RID: 50129
				public static LocString NAME = "Clock";

				// Token: 0x0400C3D2 RID: 50130
				public static LocString DESC = "A square wall clock.\n\nIt's quite damaged.";
			}

			// Token: 0x02002CB8 RID: 11448
			public class PROPGRAVITASCEILINGLIGHT
			{
				// Token: 0x0400C3D3 RID: 50131
				public static LocString NAME = "Light Fixture";

				// Token: 0x0400C3D4 RID: 50132
				public static LocString DESC = "A flush mount ceiling light that no longer functions.\n\nThere's dust inside.";
			}

			// Token: 0x02002CB9 RID: 11449
			public class PROPGRAVITASPOSTERPLANTS
			{
				// Token: 0x0400C3D5 RID: 50133
				public static LocString NAME = "Wall Chart";

				// Token: 0x0400C3D6 RID: 50134
				public static LocString DESC = "A handy reference text with illustrations.\n\nIt details the genetic makeup of proprietary botanicals.";
			}

			// Token: 0x02002CBA RID: 11450
			public class PROPGRAVITASPOSTERSEED
			{
				// Token: 0x0400C3D7 RID: 50135
				public static LocString NAME = "Wall Chart";

				// Token: 0x0400C3D8 RID: 50136
				public static LocString DESC = "A handy reference text with illustrations.\n\nIt compares the genetic makeup of select organisms.";
			}

			// Token: 0x02002CBB RID: 11451
			public class PROPGRAVITASPOTPLANTDEAD
			{
				// Token: 0x0400C3D9 RID: 50137
				public static LocString NAME = "Dead Plant";

				// Token: 0x0400C3DA RID: 50138
				public static LocString DESC = "A very dead plant.\n\nIt's a wonder it hasn't crumbled into nothingness.";
			}

			// Token: 0x02002CBC RID: 11452
			public class PROPGRAVITASFLIPPHONE
			{
				// Token: 0x0400C3DB RID: 50139
				public static LocString NAME = "Flip Phone";

				// Token: 0x0400C3DC RID: 50140
				public static LocString DESC = "An outdated phone left behind by a distracted lab technician.\n\nIt doesn't work.";
			}

			// Token: 0x02002CBD RID: 11453
			public class GRAVITASBATHROOMSTALL
			{
				// Token: 0x0400C3DD RID: 50141
				public static LocString NAME = "Toilet";

				// Token: 0x0400C3DE RID: 50142
				public static LocString DESC = "";

				// Token: 0x0400C3DF RID: 50143
				public static LocString EFFECT = "A private toilet for senior scientists.\n\nIt was the site of many great scientific breakthroughs.";
			}

			// Token: 0x02002CBE RID: 11454
			public class PROPGRAVITASBATHROOMMIRROR
			{
				// Token: 0x0400C3E0 RID: 50144
				public static LocString NAME = "Mirror";

				// Token: 0x0400C3E1 RID: 50145
				public static LocString DESC = "A one-way mirror and shelf.\n\nThe skincare products still smell faintly of shea butter.";
			}

			// Token: 0x02002CBF RID: 11455
			public class PROPGRAVITASTRASHCAN
			{
				// Token: 0x0400C3E2 RID: 50146
				public static LocString NAME = "Trash Can";

				// Token: 0x0400C3E3 RID: 50147
				public static LocString DESC = "A wall-mounted garbage receptacle.\n\nThe lid does not close.";
			}

			// Token: 0x02002CC0 RID: 11456
			public class PROPGRAVITASBATHROOMTOILETPAPERHOLDER
			{
				// Token: 0x0400C3E4 RID: 50148
				public static LocString NAME = "Toilet Paper Holder";

				// Token: 0x0400C3E5 RID: 50149
				public static LocString DESC = "It holds one roll of sanitary paper.\n\nThe sheets are single-ply.";
			}

			// Token: 0x02002CC1 RID: 11457
			public class PROPGRAVITASBATHROOMSINK
			{
				// Token: 0x0400C3E6 RID: 50150
				public static LocString NAME = "Enamel Sink";

				// Token: 0x0400C3E7 RID: 50151
				public static LocString DESC = "A handwashing station that looks suspiciously under-utilized.\n\nIt was once part of a bulk office order.";
			}

			// Token: 0x02002CC2 RID: 11458
			public class PROPGRAVITASPAPERTOLELDISPENSER
			{
				// Token: 0x0400C3E8 RID: 50152
				public static LocString NAME = "Paper Towel Dispenser";

				// Token: 0x0400C3E9 RID: 50153
				public static LocString DESC = "It once dispensed paper made from 100% post-consumer recycled content.\n\nThe remaining sheet is jammed.";
			}

			// Token: 0x02002CC3 RID: 11459
			public class PROPGRAVITASLABWINDOW
			{
				// Token: 0x0400C3EA RID: 50154
				public static LocString NAME = "Lab Window";

				// Token: 0x0400C3EB RID: 50155
				public static LocString DESC = "";

				// Token: 0x0400C3EC RID: 50156
				public static LocString EFFECT = "A lab window. Formerly a portal to the outside world.";
			}

			// Token: 0x02002CC4 RID: 11460
			public class PROPGRAVITASLABWINDOWHORIZONTAL
			{
				// Token: 0x0400C3ED RID: 50157
				public static LocString NAME = "Lab Window";

				// Token: 0x0400C3EE RID: 50158
				public static LocString DESC = "";

				// Token: 0x0400C3EF RID: 50159
				public static LocString EFFECT = "A lab window.\n\nSomeone once stared out of this, contemplating the results of an experiment.";
			}

			// Token: 0x02002CC5 RID: 11461
			public class PROPGRAVITASLABWALL
			{
				// Token: 0x0400C3F0 RID: 50160
				public static LocString NAME = "Lab Wall";

				// Token: 0x0400C3F1 RID: 50161
				public static LocString DESC = "";

				// Token: 0x0400C3F2 RID: 50162
				public static LocString EFFECT = "A regular wall that once existed in a working lab.";
			}

			// Token: 0x02002CC6 RID: 11462
			public class GRAVITASCONTAINER
			{
				// Token: 0x0400C3F3 RID: 50163
				public static LocString NAME = "Pajama Cubby";

				// Token: 0x0400C3F4 RID: 50164
				public static LocString DESC = "";

				// Token: 0x0400C3F5 RID: 50165
				public static LocString EFFECT = "A clothing storage unit.\n\nIt contains ultra-soft sleepwear.";
			}

			// Token: 0x02002CC7 RID: 11463
			public class GRAVITASLABLIGHT
			{
				// Token: 0x0400C3F6 RID: 50166
				public static LocString NAME = "LED Light";

				// Token: 0x0400C3F7 RID: 50167
				public static LocString DESC = "";

				// Token: 0x0400C3F8 RID: 50168
				public static LocString EFFECT = "An overhead light therapy lamp designed to soothe the minds.";
			}

			// Token: 0x02002CC8 RID: 11464
			public class GRAVITASDOOR
			{
				// Token: 0x0400C3F9 RID: 50169
				public static LocString NAME = "Gravitas Door";

				// Token: 0x0400C3FA RID: 50170
				public static LocString DESC = "";

				// Token: 0x0400C3FB RID: 50171
				public static LocString EFFECT = "An office door to an office that no longer exists.";
			}

			// Token: 0x02002CC9 RID: 11465
			public class PROPGRAVITASWALL
			{
				// Token: 0x0400C3FC RID: 50172
				public static LocString NAME = "Wall";

				// Token: 0x0400C3FD RID: 50173
				public static LocString DESC = "";

				// Token: 0x0400C3FE RID: 50174
				public static LocString EFFECT = "The wall of a once-great scientific facility.";
			}

			// Token: 0x02002CCA RID: 11466
			public class PROPGRAVITASWALLPURPLE
			{
				// Token: 0x0400C3FF RID: 50175
				public static LocString NAME = "Wall";

				// Token: 0x0400C400 RID: 50176
				public static LocString DESC = "";

				// Token: 0x0400C401 RID: 50177
				public static LocString EFFECT = "The wall of an ambitious research and development department.";
			}

			// Token: 0x02002CCB RID: 11467
			public class PROPGRAVITASWALLPURPLEWHITEDIAGONAL
			{
				// Token: 0x0400C402 RID: 50178
				public static LocString NAME = "Wall";

				// Token: 0x0400C403 RID: 50179
				public static LocString DESC = "";

				// Token: 0x0400C404 RID: 50180
				public static LocString EFFECT = "The wall of an ambitious research and development department.";
			}

			// Token: 0x02002CCC RID: 11468
			public class PROPGRAVITASDISPLAY4
			{
				// Token: 0x0400C405 RID: 50181
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400C406 RID: 50182
				public static LocString DESC = "An electronic display projecting the blueprint of a robotic device.\n\nIt looks like a ceiling robot.";
			}

			// Token: 0x02002CCD RID: 11469
			public class PROPDLC2DISPLAY1
			{
				// Token: 0x0400C407 RID: 50183
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400C408 RID: 50184
				public static LocString DESC = "An electronic display projecting the blueprint of an engineering project.\n\nIt looks like a pump of some kind.";
			}

			// Token: 0x02002CCE RID: 11470
			public class PROPGRAVITASCEILINGROBOT
			{
				// Token: 0x0400C409 RID: 50185
				public static LocString NAME = "Ceiling Robot";

				// Token: 0x0400C40A RID: 50186
				public static LocString DESC = "Non-functioning robotic arms that once assisted lab technicians.";
			}

			// Token: 0x02002CCF RID: 11471
			public class PROPGRAVITASFLOORROBOT
			{
				// Token: 0x0400C40B RID: 50187
				public static LocString NAME = "Robotic Arm";

				// Token: 0x0400C40C RID: 50188
				public static LocString DESC = "The grasping robotic claw designed to assist technicians in a lab.";
			}

			// Token: 0x02002CD0 RID: 11472
			public class PROPGRAVITASJAR1
			{
				// Token: 0x0400C40D RID: 50189
				public static LocString NAME = "Big Brain Jar";

				// Token: 0x0400C40E RID: 50190
				public static LocString DESC = "An abnormally large brain floating in embalming liquid to prevent decomposition.";
			}

			// Token: 0x02002CD1 RID: 11473
			public class PROPGRAVITASCREATUREPOSTER
			{
				// Token: 0x0400C40F RID: 50191
				public static LocString NAME = "Anatomy Poster";

				// Token: 0x0400C410 RID: 50192
				public static LocString DESC = "An anatomical illustration of the very first " + UI.FormatAsLink("Hatch", "HATCH") + " ever produced.\n\nWhile the ratio of egg sac to brain may appear outlandish, it is in fact to scale.";
			}

			// Token: 0x02002CD2 RID: 11474
			public class PROPGRAVITASDESKPODIUM
			{
				// Token: 0x0400C411 RID: 50193
				public static LocString NAME = "Computer Podium";

				// Token: 0x0400C412 RID: 50194
				public static LocString DESC = "A clutter-proof desk to minimize distractions.\n\nThere appears to be something stored in the computer.";
			}

			// Token: 0x02002CD3 RID: 11475
			public class PROPGRAVITASFIRSTAIDKIT
			{
				// Token: 0x0400C413 RID: 50195
				public static LocString NAME = "First Aid Kit";

				// Token: 0x0400C414 RID: 50196
				public static LocString DESC = "It looks like it's been used a lot.";
			}

			// Token: 0x02002CD4 RID: 11476
			public class PROPGRAVITASHANDSCANNER
			{
				// Token: 0x0400C415 RID: 50197
				public static LocString NAME = "Hand Scanner";

				// Token: 0x0400C416 RID: 50198
				public static LocString DESC = "A sophisticated security device.\n\nIt appears to use a method other than fingerprints to verify an individual's identity.";
			}

			// Token: 0x02002CD5 RID: 11477
			public class PROPGRAVITASLABTABLE
			{
				// Token: 0x0400C417 RID: 50199
				public static LocString NAME = "Lab Desk";

				// Token: 0x0400C418 RID: 50200
				public static LocString DESC = "The quaint research desk of a departed lab technician.\n\nPerhaps the computer stores something of interest.";
			}

			// Token: 0x02002CD6 RID: 11478
			public class PROPGRAVITASROBTICTABLE
			{
				// Token: 0x0400C419 RID: 50201
				public static LocString NAME = "Robotics Research Desk";

				// Token: 0x0400C41A RID: 50202
				public static LocString DESC = "The work space of an extinct robotics technician who left behind some unfinished prototypes.";
			}

			// Token: 0x02002CD7 RID: 11479
			public class PROPDLC2GEOTHERMALCART
			{
				// Token: 0x0400C41B RID: 50203
				public static LocString NAME = "Service Cart";

				// Token: 0x0400C41C RID: 50204
				public static LocString DESC = "Maintenance equipment that once flushed debris out of complex mechanisms.\n\nOne of the wheels is squeaky.";
			}

			// Token: 0x02002CD8 RID: 11480
			public class PROPGRAVITASSHELF
			{
				// Token: 0x0400C41D RID: 50205
				public static LocString NAME = "Shelf";

				// Token: 0x0400C41E RID: 50206
				public static LocString DESC = "A shelf holding jars just out of reach for a short person.";
			}

			// Token: 0x02002CD9 RID: 11481
			public class PROPGRAVITASTOOLSHELF
			{
				// Token: 0x0400C41F RID: 50207
				public static LocString NAME = "Tool Rack";

				// Token: 0x0400C420 RID: 50208
				public static LocString DESC = "A wall-mounted rack for storing and displaying useful tools at a not-so-useful height.";
			}

			// Token: 0x02002CDA RID: 11482
			public class PROPGRAVITASTOOLCRATE
			{
				// Token: 0x0400C421 RID: 50209
				public static LocString NAME = "Tool Crate";

				// Token: 0x0400C422 RID: 50210
				public static LocString DESC = "A packing crate intended for safety equipment.\n\nIt has been repurposed for tool storage.";
			}

			// Token: 0x02002CDB RID: 11483
			public class PROPGRAVITASFIREEXTINGUISHER
			{
				// Token: 0x0400C423 RID: 50211
				public static LocString NAME = "Broken Fire Extinguisher";

				// Token: 0x0400C424 RID: 50212
				public static LocString DESC = "Essential lab equipment.\n\nThe inspection tag indicates it has long expired.";
			}

			// Token: 0x02002CDC RID: 11484
			public class PROPGRAVITASJAR2
			{
				// Token: 0x0400C425 RID: 50213
				public static LocString NAME = "Sample Jar";

				// Token: 0x0400C426 RID: 50214
				public static LocString DESC = "The corpse of a proto-hatch creature meticulously preserved in a jar.";
			}

			// Token: 0x02002CDD RID: 11485
			public class PROPEXOSHELFLONG
			{
				// Token: 0x0400C427 RID: 50215
				public static LocString NAME = "Long Prefab Shelf";

				// Token: 0x0400C428 RID: 50216
				public static LocString DESC = "A shelf made out of flat-packed pieces that can be assembled in various ways.\n\nThis is the long way.";
			}

			// Token: 0x02002CDE RID: 11486
			public class PROPEXOSHELSHORT
			{
				// Token: 0x0400C429 RID: 50217
				public static LocString NAME = "Prefab Shelf";

				// Token: 0x0400C42A RID: 50218
				public static LocString DESC = "A shelf made out of flat-packed pieces that can be assembled in various ways.\n\nIt looks nice, actually.";
			}

			// Token: 0x02002CDF RID: 11487
			public class PROPHUMANMURPHYBED
			{
				// Token: 0x0400C42B RID: 50219
				public static LocString NAME = "Murphy Bed";

				// Token: 0x0400C42C RID: 50220
				public static LocString DESC = "A bed that folds into the wall, for small live/work spaces.\n\nThis is the display model.";
			}

			// Token: 0x02002CE0 RID: 11488
			public class PROPHUMANCHESTERFIELDSOFA
			{
				// Token: 0x0400C42D RID: 50221
				public static LocString NAME = "Showroom Couch";

				// Token: 0x0400C42E RID: 50222
				public static LocString DESC = "A luxurious couch where potential residents can comfortably nap and dream of home.";
			}

			// Token: 0x02002CE1 RID: 11489
			public class PROPHUMANCHESTERFIELDCHAIR
			{
				// Token: 0x0400C42F RID: 50223
				public static LocString NAME = "Showroom Chair";

				// Token: 0x0400C430 RID: 50224
				public static LocString DESC = "A luxurious chair where future generations can comfortably sit and dream of home.";
			}

			// Token: 0x02002CE2 RID: 11490
			public class WARPCONDUITRECEIVER
			{
				// Token: 0x0400C431 RID: 50225
				public static LocString NAME = UI.FormatAsLink("Supply Teleporter Output", "WARPCONDUITRECEIVER");

				// Token: 0x0400C432 RID: 50226
				public static LocString DESC = "The tubes at the back disappear into nowhere.";

				// Token: 0x0400C433 RID: 50227
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A machine capable of teleporting ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources to another asteroid.\n\nIt can be activated by a Duplicant with the ",
					UI.FormatAsLink("Field Research", "RESEARCHING2"),
					" skill.\n\nThis is the receiving side."
				});
			}

			// Token: 0x02002CE3 RID: 11491
			public class WARPCONDUITSENDER
			{
				// Token: 0x0400C434 RID: 50228
				public static LocString NAME = UI.FormatAsLink("Supply Teleporter Input", "WARPCONDUITSENDER");

				// Token: 0x0400C435 RID: 50229
				public static LocString DESC = "The tubes at the back disappear into nowhere.";

				// Token: 0x0400C436 RID: 50230
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A machine capable of teleporting ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources to another asteroid.\n\nIt can be activated by a Duplicant with the ",
					UI.FormatAsLink("Field Research", "RESEARCHING2"),
					" skill.\n\nThis is the transmitting side."
				});
			}

			// Token: 0x02002CE4 RID: 11492
			public class WARPPORTAL
			{
				// Token: 0x0400C437 RID: 50231
				public static LocString NAME = "Teleporter Transmitter";

				// Token: 0x0400C438 RID: 50232
				public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the outgoing side, and has one pre-programmed destination.";
			}

			// Token: 0x02002CE5 RID: 11493
			public class WARPRECEIVER
			{
				// Token: 0x0400C439 RID: 50233
				public static LocString NAME = "Teleporter Receiver";

				// Token: 0x0400C43A RID: 50234
				public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the incoming side.";
			}

			// Token: 0x02002CE6 RID: 11494
			public class TEMPORALTEAROPENER
			{
				// Token: 0x0400C43B RID: 50235
				public static LocString NAME = "Temporal Tear Opener";

				// Token: 0x0400C43C RID: 50236
				public static LocString DESC = "Infinite possibilities, with a complimentary side of meteor showers.";

				// Token: 0x0400C43D RID: 50237
				public static LocString EFFECT = "A powerful mechanism capable of tearing through the fabric of reality.";

				// Token: 0x02003A84 RID: 14980
				public class SIDESCREEN
				{
					// Token: 0x0400EBBA RID: 60346
					public static LocString TEXT = "Fire!";

					// Token: 0x0400EBBB RID: 60347
					public static LocString TOOLTIP = "The big red button.";
				}
			}

			// Token: 0x02002CE7 RID: 11495
			public class LONELYMINIONHOUSE
			{
				// Token: 0x0400C43E RID: 50238
				public static LocString NAME = UI.FormatAsLink("Gravitas Shipping Container", "STORYTRAITLONELYMINION");

				// Token: 0x0400C43F RID: 50239
				public static LocString DESC = "Its occupant has been alone for so long, he's forgotten what friendship feels like.";

				// Token: 0x0400C440 RID: 50240
				public static LocString EFFECT = "A large transport unit from the facility's sub-sub-basement.\n\nIt has been modified into a crude yet functional temporary shelter.";
			}

			// Token: 0x02002CE8 RID: 11496
			public class LONELYMINIONHOUSE_COMPLETE
			{
				// Token: 0x0400C441 RID: 50241
				public static LocString NAME = UI.FormatAsLink("Gravitas Shipping Container", "STORYTRAITLONELYMINION");

				// Token: 0x0400C442 RID: 50242
				public static LocString DESC = "Someone lived inside it for a while.";

				// Token: 0x0400C443 RID: 50243
				public static LocString EFFECT = "A super-spacious container for the " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " of your choosing.";
			}

			// Token: 0x02002CE9 RID: 11497
			public class LONELYMAILBOX
			{
				// Token: 0x0400C444 RID: 50244
				public static LocString NAME = "Mailbox";

				// Token: 0x0400C445 RID: 50245
				public static LocString DESC = "There's nothing quite like receiving homemade gifts in the mail.";

				// Token: 0x0400C446 RID: 50246
				public static LocString EFFECT = "Displays a single edible object.";
			}

			// Token: 0x02002CEA RID: 11498
			public class PLASTICFLOWERS
			{
				// Token: 0x0400C447 RID: 50247
				public static LocString NAME = "Plastic Flowers";

				// Token: 0x0400C448 RID: 50248
				public static LocString DESCRIPTION = "Maintenance-free blooms that will outlive us all.";

				// Token: 0x0400C449 RID: 50249
				public static LocString LORE_DLC2 = "Manufactured by Home Staging Heroes Ltd. as commissioned by the Gravitas Facility, to <i>\"Make Space Feel More Like Home.\"</i>\n\nThis bouquet is designed to smell like freshly baked cookies.";
			}

			// Token: 0x02002CEB RID: 11499
			public class FOUNTAINPEN
			{
				// Token: 0x0400C44A RID: 50250
				public static LocString NAME = "Fountain Pen";

				// Token: 0x0400C44B RID: 50251
				public static LocString DESCRIPTION = "Cuts through red tape better than a sword ever could.";

				// Token: 0x0400C44C RID: 50252
				public static LocString LORE_DLC2 = "The handcrafted gold nib features a triangular logo with the letters V and I inside.\n\nIts owner was too proud to report it stolen, and would be shocked to learn of its whereabouts.";
			}

			// Token: 0x02002CEC RID: 11500
			public class PROPCLOTHESHANGER
			{
				// Token: 0x0400C44D RID: 50253
				public static LocString NAME = "Coat Rack";

				// Token: 0x0400C44E RID: 50254
				public static LocString DESC = "Holds one " + EQUIPMENT.PREFABS.WARM_VEST.NAME + ".\n\nIt'd be silly not to use it.";
			}

			// Token: 0x02002CED RID: 11501
			public class PROPCERESPOSTERA
			{
				// Token: 0x0400C44F RID: 50255
				public static LocString NAME = "Travel Poster";

				// Token: 0x0400C450 RID: 50256
				public static LocString DESC = "A poster promoting a local tourist attraction.\n\nActual scenery may vary.";
			}

			// Token: 0x02002CEE RID: 11502
			public class PROPCERESPOSTERB
			{
				// Token: 0x0400C451 RID: 50257
				public static LocString NAME = "Travel Poster";

				// Token: 0x0400C452 RID: 50258
				public static LocString DESC = "A poster promoting local wildlife.\n\nThe first in an unfinished series.";
			}

			// Token: 0x02002CEF RID: 11503
			public class PROPCERESPOSTERLARGE
			{
				// Token: 0x0400C453 RID: 50259
				public static LocString NAME = "Acoustic Art Panel";

				// Token: 0x0400C454 RID: 50260
				public static LocString DESC = "A sound-absorbing panel that makes small-space living more bearable.\n\nThe artwork features a  power source.";
			}

			// Token: 0x02002CF0 RID: 11504
			public class CHLORINATOR
			{
				// Token: 0x0400C455 RID: 50261
				public static LocString NAME = UI.FormatAsLink("Bleach Stone Hopper", "CHLORINATOR");

				// Token: 0x0400C456 RID: 50262
				public static LocString DESC = "Bleach stone is useful for sanitation and geotuning.";

				// Token: 0x0400C457 RID: 50263
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					ELEMENTS.SALT.NAME,
					" and ",
					ELEMENTS.GOLD.NAME,
					" to produce ",
					ELEMENTS.BLEACHSTONE.NAME,
					"."
				});
			}

			// Token: 0x02002CF1 RID: 11505
			public class MILKPRESS
			{
				// Token: 0x0400C458 RID: 50264
				public static LocString NAME = UI.FormatAsLink("Plant Pulverizer", "MILKPRESS");

				// Token: 0x0400C459 RID: 50265
				public static LocString DESC = "For Duplicants who are too squeamish to milk critters.";

				// Token: 0x0400C45A RID: 50266
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Crushes organic materials to extract liquids such as ",
					ELEMENTS.MILK.NAME,
					" or ",
					ELEMENTS.PHYTOOIL.NAME,
					".\n\n",
					ELEMENTS.MILK.NAME,
					" can be used to refill the ",
					BUILDINGS.PREFABS.MILKFEEDER.NAME,
					"."
				});

				// Token: 0x0400C45B RID: 50267
				public static LocString WHEAT_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				// Token: 0x0400C45C RID: 50268
				public static LocString VINEFRUIT_JAM_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				// Token: 0x0400C45D RID: 50269
				public static LocString NUT_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				// Token: 0x0400C45E RID: 50270
				public static LocString PHYTO_OIL_RECIPE_DESCRIPTION = "Converts {0} to {1} and {2}";

				// Token: 0x0400C45F RID: 50271
				public static LocString KELP_TO_PHYTO_OIL_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				// Token: 0x0400C460 RID: 50272
				public static LocString DEWDRIPPER_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				// Token: 0x0400C461 RID: 50273
				public static LocString RESIN_FROM_AMBER_RECIPE_DESCRIPTION = "Converts {0} into {1}, {2}, and a small amount of {3}";
			}

			// Token: 0x02002CF2 RID: 11506
			public class FOODDEHYDRATOR
			{
				// Token: 0x0400C462 RID: 50274
				public static LocString NAME = UI.FormatAsLink("Dehydrator", "FOODDEHYDRATOR");

				// Token: 0x0400C463 RID: 50275
				public static LocString DESC = "Some of the eliminated liquid inevitably ends up on the floor.";

				// Token: 0x0400C464 RID: 50276
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses low, even heat to eliminate moisture from eligible ",
					UI.FormatAsLink("Foods", "FOOD"),
					" and render them shelf-stable.\n\nDehydrated meals must be processed at the ",
					UI.FormatAsLink("Rehydrator", "FOODREHYDRATOR"),
					" before they can be eaten."
				});

				// Token: 0x0400C465 RID: 50277
				public static LocString RECIPE_NAME = "Dried {0}";

				// Token: 0x0400C466 RID: 50278
				public static LocString RESULT_DESCRIPTION = "Dehydrated portions of {0} do not require refrigeration.";
			}

			// Token: 0x02002CF3 RID: 11507
			public class FOODREHYDRATOR
			{
				// Token: 0x0400C467 RID: 50279
				public static LocString NAME = UI.FormatAsLink("Rehydrator", "FOODREHYDRATOR");

				// Token: 0x0400C468 RID: 50280
				public static LocString DESC = "Rehydrated food is nutritious and only slightly less delicious.";

				// Token: 0x0400C469 RID: 50281
				public static LocString EFFECT = "Restores moisture to convert shelf-stable packaged meals into edible " + UI.FormatAsLink("Food", "FOOD") + ".";
			}

			// Token: 0x02002CF4 RID: 11508
			public class GEOTHERMALCONTROLLER
			{
				// Token: 0x0400C46A RID: 50282
				public static LocString NAME = UI.FormatAsLink("Geothermal Heat Pump", "GEOTHERMALCONTROLLER");

				// Token: 0x0400C46B RID: 50283
				public static LocString DESC = "What comes out depends very much on the initial temperature of what goes in.";

				// Token: 0x0400C46C RID: 50284
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Heat", "HEAT"),
					" from the planet's core to dramatically increase the temperature of ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" inputs.\n\nMaterials will be emitted at connected Geo Vents."
				});
			}

			// Token: 0x02002CF5 RID: 11509
			public class GEOTHERMALVENT
			{
				// Token: 0x0400C46D RID: 50285
				public static LocString NAME = UI.FormatAsLink("Geo Vent", "GEOTHERMALVENT");

				// Token: 0x0400C46E RID: 50286
				public static LocString NAME_FMT = UI.FormatAsLink("Geo Vent C-{ID}", "GEOTHERMALVENT");

				// Token: 0x0400C46F RID: 50287
				public static LocString DESC = "Geo vents must finish their current emission before accepting new materials.";

				// Token: 0x0400C470 RID: 50288
				public static LocString EFFECT = "Emits high-" + UI.FormatAsLink("temperature", "HEAT") + " materials received from the Geothermal Heat Pump.";

				// Token: 0x0400C471 RID: 50289
				public static LocString BLOCKED_DESC = string.Concat(new string[]
				{
					"Blocked geo vents can be cleared by pumping in ",
					UI.FormatAsLink("liquids", "ELEMENTS_LIQUID"),
					" that are hot enough to melt ",
					UI.FormatAsLink("Lead", "LEAD"),
					"."
				});

				// Token: 0x0400C472 RID: 50290
				public static LocString LOGIC_PORT = "Material Content Monitor";

				// Token: 0x0400C473 RID: 50291
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when geo vent has materials to emit";

				// Token: 0x0400C474 RID: 50292
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}
		}

		// Token: 0x02002251 RID: 8785
		public static class DAMAGESOURCES
		{
			// Token: 0x04009E89 RID: 40585
			public static LocString NOTIFICATION_TOOLTIP = "A {0} sustained damage from {1}";

			// Token: 0x04009E8A RID: 40586
			public static LocString CONDUIT_CONTENTS_FROZE = "pipe contents becoming too cold";

			// Token: 0x04009E8B RID: 40587
			public static LocString CONDUIT_CONTENTS_BOILED = "pipe contents becoming too hot";

			// Token: 0x04009E8C RID: 40588
			public static LocString BUILDING_OVERHEATED = "overheating";

			// Token: 0x04009E8D RID: 40589
			public static LocString CORROSIVE_ELEMENT = "corrosive element";

			// Token: 0x04009E8E RID: 40590
			public static LocString BAD_INPUT_ELEMENT = "receiving an incorrect substance";

			// Token: 0x04009E8F RID: 40591
			public static LocString MINION_DESTRUCTION = "an angry Duplicant. Rude!";

			// Token: 0x04009E90 RID: 40592
			public static LocString LIQUID_PRESSURE = "neighboring liquid pressure";

			// Token: 0x04009E91 RID: 40593
			public static LocString CIRCUIT_OVERLOADED = "an overloaded circuit";

			// Token: 0x04009E92 RID: 40594
			public static LocString LOGIC_CIRCUIT_OVERLOADED = "an overloaded logic circuit";

			// Token: 0x04009E93 RID: 40595
			public static LocString MICROMETEORITE = "micrometeorite";

			// Token: 0x04009E94 RID: 40596
			public static LocString COMET = "falling space rocks";

			// Token: 0x04009E95 RID: 40597
			public static LocString ROCKET = "rocket engine";
		}

		// Token: 0x02002252 RID: 8786
		public static class AUTODISINFECTABLE
		{
			// Token: 0x02002CF6 RID: 11510
			public static class ENABLE_AUTODISINFECT
			{
				// Token: 0x0400C475 RID: 50293
				public static LocString NAME = "Enable Disinfect";

				// Token: 0x0400C476 RID: 50294
				public static LocString TOOLTIP = "Automatically disinfect this building when it becomes contaminated";
			}

			// Token: 0x02002CF7 RID: 11511
			public static class DISABLE_AUTODISINFECT
			{
				// Token: 0x0400C477 RID: 50295
				public static LocString NAME = "Disable Disinfect";

				// Token: 0x0400C478 RID: 50296
				public static LocString TOOLTIP = "Do not automatically disinfect this building";
			}

			// Token: 0x02002CF8 RID: 11512
			public static class NO_DISEASE
			{
				// Token: 0x0400C479 RID: 50297
				public static LocString TOOLTIP = "This building is clean";
			}
		}

		// Token: 0x02002253 RID: 8787
		public static class DISINFECTABLE
		{
			// Token: 0x02002CF9 RID: 11513
			public static class ENABLE_DISINFECT
			{
				// Token: 0x0400C47A RID: 50298
				public static LocString NAME = "Disinfect";

				// Token: 0x0400C47B RID: 50299
				public static LocString TOOLTIP = "Mark this building for disinfection";
			}

			// Token: 0x02002CFA RID: 11514
			public static class DISABLE_DISINFECT
			{
				// Token: 0x0400C47C RID: 50300
				public static LocString NAME = "Cancel Disinfect";

				// Token: 0x0400C47D RID: 50301
				public static LocString TOOLTIP = "Cancel this disinfect order";
			}

			// Token: 0x02002CFB RID: 11515
			public static class NO_DISEASE
			{
				// Token: 0x0400C47E RID: 50302
				public static LocString TOOLTIP = "This building is already clean";
			}
		}

		// Token: 0x02002254 RID: 8788
		public static class REPAIRABLE
		{
			// Token: 0x02002CFC RID: 11516
			public static class ENABLE_AUTOREPAIR
			{
				// Token: 0x0400C47F RID: 50303
				public static LocString NAME = "Enable Autorepair";

				// Token: 0x0400C480 RID: 50304
				public static LocString TOOLTIP = "Automatically repair this building when damaged";
			}

			// Token: 0x02002CFD RID: 11517
			public static class DISABLE_AUTOREPAIR
			{
				// Token: 0x0400C481 RID: 50305
				public static LocString NAME = "Disable Autorepair";

				// Token: 0x0400C482 RID: 50306
				public static LocString TOOLTIP = "Only repair this building when ordered";
			}
		}

		// Token: 0x02002255 RID: 8789
		public static class AUTOMATABLE
		{
			// Token: 0x02002CFE RID: 11518
			public static class ENABLE_AUTOMATIONONLY
			{
				// Token: 0x0400C483 RID: 50307
				public static LocString NAME = "Disable Manual";

				// Token: 0x0400C484 RID: 50308
				public static LocString TOOLTIP = "This building's storage may be accessed by Auto-Sweepers only\n\nDuplicants will not be permitted to add or remove materials from this building";
			}

			// Token: 0x02002CFF RID: 11519
			public static class DISABLE_AUTOMATIONONLY
			{
				// Token: 0x0400C485 RID: 50309
				public static LocString NAME = "Enable Manual";

				// Token: 0x0400C486 RID: 50310
				public static LocString TOOLTIP = "This building's storage may be accessed by both Duplicants and Auto-Sweeper buildings";
			}
		}
	}
}
