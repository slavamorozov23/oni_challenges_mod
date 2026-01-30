using System;

namespace STRINGS
{
	// Token: 0x02000FFC RID: 4092
	public class ELEMENTS
	{
		// Token: 0x0400604A RID: 24650
		public static LocString ELEMENTDESCSOLID = "Resource Type: {0}\nMelting point: {1}\nHardness: {2}";

		// Token: 0x0400604B RID: 24651
		public static LocString ELEMENTDESCLIQUID = "Resource Type: {0}\nFreezing point: {1}\nEvaporation point: {2}";

		// Token: 0x0400604C RID: 24652
		public static LocString ELEMENTDESCGAS = "Resource Type: {0}\nCondensation point: {1}";

		// Token: 0x0400604D RID: 24653
		public static LocString ELEMENTDESCVACUUM = "Resource Type: {0}";

		// Token: 0x0400604E RID: 24654
		public static LocString BREATHABLEDESC = "<color=#{0}>({1})</color>";

		// Token: 0x0400604F RID: 24655
		public static LocString THERMALPROPERTIES = "\nSpecific Heat Capacity: {SPECIFIC_HEAT_CAPACITY}\nThermal Conductivity: {THERMAL_CONDUCTIVITY}";

		// Token: 0x04006050 RID: 24656
		public static LocString RADIATIONPROPERTIES = "Radiation Absorption Factor: {0}\nRadiation Emission/1000kg: {1}";

		// Token: 0x04006051 RID: 24657
		public static LocString ELEMENTPROPERTIES = "Properties: {0}";

		// Token: 0x020025A2 RID: 9634
		public class STATE
		{
			// Token: 0x0400A9D2 RID: 43474
			public static LocString SOLID = "Solid";

			// Token: 0x0400A9D3 RID: 43475
			public static LocString LIQUID = "Liquid";

			// Token: 0x0400A9D4 RID: 43476
			public static LocString GAS = "Gas";

			// Token: 0x0400A9D5 RID: 43477
			public static LocString VACUUM = "None";
		}

		// Token: 0x020025A3 RID: 9635
		public class MATERIAL_MODIFIERS
		{
			// Token: 0x0400A9D6 RID: 43478
			public static LocString EFFECTS_HEADER = "<b>Resource Effects:</b>";

			// Token: 0x0400A9D7 RID: 43479
			public static LocString DECOR = UI.FormatAsLink("Decor", "DECOR") + ": {0}";

			// Token: 0x0400A9D8 RID: 43480
			public static LocString OVERHEATTEMPERATURE = UI.FormatAsLink("Overheat Temperature", "HEAT") + ": {0}";

			// Token: 0x0400A9D9 RID: 43481
			public static LocString HIGH_THERMAL_CONDUCTIVITY = UI.FormatAsLink("High Thermal Conductivity", "HEAT");

			// Token: 0x0400A9DA RID: 43482
			public static LocString LOW_THERMAL_CONDUCTIVITY = UI.FormatAsLink("Insulator", "HEAT");

			// Token: 0x0400A9DB RID: 43483
			public static LocString LOW_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Thermally Reactive", "HEAT");

			// Token: 0x0400A9DC RID: 43484
			public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Slow Heating", "HEAT");

			// Token: 0x0400A9DD RID: 43485
			public static LocString EXCELLENT_RADIATION_SHIELD = UI.FormatAsLink("Excellent Radiation Shield", "RADIATION");

			// Token: 0x02003955 RID: 14677
			public class TOOLTIP
			{
				// Token: 0x0400E836 RID: 59446
				public static LocString EFFECTS_HEADER = "Buildings constructed from this material will have these properties";

				// Token: 0x0400E837 RID: 59447
				public static LocString DECOR = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Decor" + UI.PST_KEYWORD;

				// Token: 0x0400E838 RID: 59448
				public static LocString OVERHEATTEMPERATURE = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Overheat Temperature" + UI.PST_KEYWORD;

				// Token: 0x0400E839 RID: 59449
				public static LocString HIGH_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material disperses ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers quickly through materials with high ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400E83A RID: 59450
				public static LocString LOW_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material retains ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers slowly through materials with low ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400E83B RID: 59451
				public static LocString LOW_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Thermally Reactive",
					UI.PST_KEYWORD,
					" materials require little energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool quickly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400E83C RID: 59452
				public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Slow Heating",
					UI.PST_KEYWORD,
					" materials require a large amount of energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool slowly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400E83D RID: 59453
				public static LocString EXCELLENT_RADIATION_SHIELD = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Excellent Radiation Shield",
					UI.PST_KEYWORD,
					" radiation has a hard time passing through materials with a high ",
					UI.PRE_KEYWORD,
					"Radiation Absorption Factor",
					UI.PST_KEYWORD,
					" value. \n\nRadiation Absorption Factor: {1}"
				});
			}
		}

		// Token: 0x020025A4 RID: 9636
		public class HARDNESS
		{
			// Token: 0x0400A9DE RID: 43486
			public static LocString NA = "N/A";

			// Token: 0x0400A9DF RID: 43487
			public static LocString SOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.SOFT + ")";

			// Token: 0x0400A9E0 RID: 43488
			public static LocString VERYSOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYSOFT + ")";

			// Token: 0x0400A9E1 RID: 43489
			public static LocString FIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.FIRM + ")";

			// Token: 0x0400A9E2 RID: 43490
			public static LocString VERYFIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + ")";

			// Token: 0x0400A9E3 RID: 43491
			public static LocString NEARLYIMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE + ")";

			// Token: 0x0400A9E4 RID: 43492
			public static LocString IMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.IMPENETRABLE + ")";

			// Token: 0x02003956 RID: 14678
			public class HARDNESS_DESCRIPTOR
			{
				// Token: 0x0400E83E RID: 59454
				public static LocString SOFT = "Soft";

				// Token: 0x0400E83F RID: 59455
				public static LocString VERYSOFT = "Very Soft";

				// Token: 0x0400E840 RID: 59456
				public static LocString FIRM = "Firm";

				// Token: 0x0400E841 RID: 59457
				public static LocString VERYFIRM = "Very Firm";

				// Token: 0x0400E842 RID: 59458
				public static LocString NEARLYIMPENETRABLE = "Nearly Impenetrable";

				// Token: 0x0400E843 RID: 59459
				public static LocString IMPENETRABLE = "Impenetrable";
			}
		}

		// Token: 0x020025A5 RID: 9637
		public class AEROGEL
		{
			// Token: 0x0400A9E5 RID: 43493
			public static LocString NAME = UI.FormatAsLink("Aerogel", "AEROGEL");

			// Token: 0x0400A9E6 RID: 43494
			public static LocString DESC = "";
		}

		// Token: 0x020025A6 RID: 9638
		public class ALGAE
		{
			// Token: 0x0400A9E7 RID: 43495
			public static LocString NAME = UI.FormatAsLink("Algae", "ALGAE");

			// Token: 0x0400A9E8 RID: 43496
			public static LocString DESC = string.Concat(new string[]
			{
				"Algae is a cluster of non-motile, single-celled lifeforms.\n\nIt can be used to produce ",
				ELEMENTS.OXYGEN.NAME,
				" when used in an ",
				BUILDINGS.PREFABS.MINERALDEOXIDIZER.NAME,
				"."
			});
		}

		// Token: 0x020025A7 RID: 9639
		public class ALUMINUMORE
		{
			// Token: 0x0400A9E9 RID: 43497
			public static LocString NAME = UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE");

			// Token: 0x0400A9EA RID: 43498
			public static LocString DESC = "Aluminum ore, also known as Bauxite, is a sedimentary rock high in metal content.\n\nIt can be refined into " + UI.FormatAsLink("Aluminum", "ALUMINUM") + ".";
		}

		// Token: 0x020025A8 RID: 9640
		public class ALUMINUM
		{
			// Token: 0x0400A9EB RID: 43499
			public static LocString NAME = UI.FormatAsLink("Aluminum", "ALUMINUM");

			// Token: 0x0400A9EC RID: 43500
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				".\n\nIt has high Thermal Conductivity and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025A9 RID: 9641
		public class MOLTENALUMINUM
		{
			// Token: 0x0400A9ED RID: 43501
			public static LocString NAME = UI.FormatAsLink("Molten Aluminum", "MOLTENALUMINUM");

			// Token: 0x0400A9EE RID: 43502
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Molten Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x020025AA RID: 9642
		public class ALUMINUMGAS
		{
			// Token: 0x0400A9EF RID: 43503
			public static LocString NAME = UI.FormatAsLink("Aluminum Gas", "ALUMINUMGAS");

			// Token: 0x0400A9F0 RID: 43504
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum Gas is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020025AB RID: 9643
		public class BLEACHSTONE
		{
			// Token: 0x0400A9F1 RID: 43505
			public static LocString NAME = UI.FormatAsLink("Bleach Stone", "BLEACHSTONE");

			// Token: 0x0400A9F2 RID: 43506
			public static LocString DESC = string.Concat(new string[]
			{
				"Bleach stone is an unstable compound that emits unbreathable ",
				UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
				".\n\nIt is often used in ",
				UI.FormatAsLink("Hygienic", "HANDSANITIZER"),
				" processes."
			});
		}

		// Token: 0x020025AC RID: 9644
		public class BITUMEN
		{
			// Token: 0x0400A9F3 RID: 43507
			public static LocString NAME = UI.FormatAsLink("Bitumen", "BITUMEN");

			// Token: 0x0400A9F4 RID: 43508
			public static LocString DESC = "Bitumen is a sticky viscous residue left behind from " + ELEMENTS.PETROLEUM.NAME + " production.";
		}

		// Token: 0x020025AD RID: 9645
		public class BOTTLEDWATER
		{
			// Token: 0x0400A9F5 RID: 43509
			public static LocString NAME = UI.FormatAsLink("Water", "BOTTLEDWATER");

			// Token: 0x0400A9F6 RID: 43510
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + ELEMENTS.WATER.NAME + ", prepped for transport.";
		}

		// Token: 0x020025AE RID: 9646
		public class BRINEICE
		{
			// Token: 0x0400A9F7 RID: 43511
			public static LocString NAME = UI.FormatAsLink("Brine Ice", "BRINEICE");

			// Token: 0x0400A9F8 RID: 43512
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine Ice is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				" and frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x020025AF RID: 9647
		public class MILKICE
		{
			// Token: 0x0400A9F9 RID: 43513
			public static LocString NAME = UI.FormatAsLink("Frozen Brackene", "MILKICE");

			// Token: 0x0400A9FA RID: 43514
			public static LocString DESC = string.Concat(new string[]
			{
				"Frozen Brackene is ",
				UI.FormatAsLink("Brackene", "MILK"),
				" frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x020025B0 RID: 9648
		public class BRINE
		{
			// Token: 0x0400A9FB RID: 43515
			public static LocString NAME = UI.FormatAsLink("Brine", "BRINE");

			// Token: 0x0400A9FC RID: 43516
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x020025B1 RID: 9649
		public class CARBON
		{
			// Token: 0x0400A9FD RID: 43517
			public static LocString NAME = UI.FormatAsLink("Coal", "CARBON");

			// Token: 0x0400A9FE RID: 43518
			public static LocString DESC = "(C) Coal is a combustible fossil fuel composed of carbon.\n\nIt is useful in " + UI.FormatAsLink("Power", "POWER") + " production.";
		}

		// Token: 0x020025B2 RID: 9650
		public class REFINEDCARBON
		{
			// Token: 0x0400A9FF RID: 43519
			public static LocString NAME = UI.FormatAsLink("Refined Carbon", "REFINEDCARBON");

			// Token: 0x0400AA00 RID: 43520
			public static LocString DESC = "(C) Refined carbon is solid element purified from raw " + ELEMENTS.CARBON.NAME + ".";
		}

		// Token: 0x020025B3 RID: 9651
		public class PEAT
		{
			// Token: 0x0400AA01 RID: 43521
			public static LocString NAME = UI.FormatAsLink("Peat", "PEAT");

			// Token: 0x0400AA02 RID: 43522
			public static LocString DESC = "Peat is a densely packed material made up of partially decomposed organic matter.\n\nIt is a combustible fuel, useful in " + UI.FormatAsLink("Power", "POWER") + " production.";
		}

		// Token: 0x020025B4 RID: 9652
		public class ETHANOLGAS
		{
			// Token: 0x0400AA03 RID: 43523
			public static LocString NAME = UI.FormatAsLink("Ethanol Gas", "ETHANOLGAS");

			// Token: 0x0400AA04 RID: 43524
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol Gas is an advanced chemical compound heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x020025B5 RID: 9653
		public class ETHANOL
		{
			// Token: 0x0400AA05 RID: 43525
			public static LocString NAME = UI.FormatAsLink("Ethanol", "ETHANOL");

			// Token: 0x0400AA06 RID: 43526
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol is an advanced chemical compound in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x020025B6 RID: 9654
		public class SOLIDETHANOL
		{
			// Token: 0x0400AA07 RID: 43527
			public static LocString NAME = UI.FormatAsLink("Solid Ethanol", "SOLIDETHANOL");

			// Token: 0x0400AA08 RID: 43528
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Solid Ethanol is an advanced chemical compound.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x020025B7 RID: 9655
		public class CARBONDIOXIDE
		{
			// Token: 0x0400AA09 RID: 43529
			public static LocString NAME = UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE");

			// Token: 0x0400AA0A RID: 43530
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an atomically heavy chemical compound in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.\n\nIt tends to sink below other gases.";
		}

		// Token: 0x020025B8 RID: 9656
		public class CARBONFIBRE
		{
			// Token: 0x0400AA0B RID: 43531
			public static LocString NAME = UI.FormatAsLink("Carbon Fiber", "CARBONFIBRE");

			// Token: 0x0400AA0C RID: 43532
			public static LocString DESC = "Carbon Fiber is a " + UI.FormatAsLink("Manufactured Material", "REFINEDMINERAL") + " with high tensile strength.";
		}

		// Token: 0x020025B9 RID: 9657
		public class CARBONGAS
		{
			// Token: 0x0400AA0D RID: 43533
			public static LocString NAME = UI.FormatAsLink("Carbon Gas", "CARBONGAS");

			// Token: 0x0400AA0E RID: 43534
			public static LocString DESC = "(C) Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x020025BA RID: 9658
		public class CHLORINE
		{
			// Token: 0x0400AA0F RID: 43535
			public static LocString NAME = UI.FormatAsLink("Liquid Chlorine", "CHLORINE");

			// Token: 0x0400AA10 RID: 43536
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x020025BB RID: 9659
		public class CHLORINEGAS
		{
			// Token: 0x0400AA11 RID: 43537
			public static LocString NAME = UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS");

			// Token: 0x0400AA12 RID: 43538
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020025BC RID: 9660
		public class CLAY
		{
			// Token: 0x0400AA13 RID: 43539
			public static LocString NAME = UI.FormatAsLink("Clay", "CLAY");

			// Token: 0x0400AA14 RID: 43540
			public static LocString DESC = "Clay is a soft, naturally occurring composite of stone and soil that hardens at high " + UI.FormatAsLink("Temperatures", "HEAT") + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x020025BD RID: 9661
		public class BRICK
		{
			// Token: 0x0400AA15 RID: 43541
			public static LocString NAME = UI.FormatAsLink("Brick", "BRICK");

			// Token: 0x0400AA16 RID: 43542
			public static LocString DESC = "Brick is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x020025BE RID: 9662
		public class CERAMIC
		{
			// Token: 0x0400AA17 RID: 43543
			public static LocString NAME = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x0400AA18 RID: 43544
			public static LocString DESC = "Ceramic is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x020025BF RID: 9663
		public class CEMENT
		{
			// Token: 0x0400AA19 RID: 43545
			public static LocString NAME = UI.FormatAsLink("Cement", "CEMENT");

			// Token: 0x0400AA1A RID: 43546
			public static LocString DESC = "Cement is a refined building material used for assembling advanced buildings.";
		}

		// Token: 0x020025C0 RID: 9664
		public class CEMENTMIX
		{
			// Token: 0x0400AA1B RID: 43547
			public static LocString NAME = UI.FormatAsLink("Cement Mix", "CEMENTMIX");

			// Token: 0x0400AA1C RID: 43548
			public static LocString DESC = "Cement Mix can be used to create " + ELEMENTS.CEMENT.NAME + " for advanced building assembly.";
		}

		// Token: 0x020025C1 RID: 9665
		public class CONTAMINATEDOXYGEN
		{
			// Token: 0x0400AA1D RID: 43549
			public static LocString NAME = UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN");

			// Token: 0x0400AA1E RID: 43550
			public static LocString DESC = "(O<sub>2</sub>) Polluted Oxygen is dirty, unfiltered air.\n\nIt is breathable.";
		}

		// Token: 0x020025C2 RID: 9666
		public class COPPER
		{
			// Token: 0x0400AA1F RID: 43551
			public static LocString NAME = UI.FormatAsLink("Copper", "COPPER");

			// Token: 0x0400AA20 RID: 43552
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025C3 RID: 9667
		public class COPPERGAS
		{
			// Token: 0x0400AA21 RID: 43553
			public static LocString NAME = UI.FormatAsLink("Copper Gas", "COPPERGAS");

			// Token: 0x0400AA22 RID: 43554
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper Gas is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				" heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020025C4 RID: 9668
		public class NICKELORE
		{
			// Token: 0x0400AA23 RID: 43555
			public static LocString NAME = UI.FormatAsLink("Nickel Ore", "NICKELORE");

			// Token: 0x0400AA24 RID: 43556
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ni) Nickel Ore is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt can be refined into ",
				UI.FormatAsLink("Nickel", "NICKEL"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025C5 RID: 9669
		public class NICKEL
		{
			// Token: 0x0400AA25 RID: 43557
			public static LocString NAME = UI.FormatAsLink("Nickel", "NICKEL");

			// Token: 0x0400AA26 RID: 43558
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ni) Nickel is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025C6 RID: 9670
		public class MOLTENNICKEL
		{
			// Token: 0x0400AA27 RID: 43559
			public static LocString NAME = UI.FormatAsLink("Molten Nickel", "MOLTENNICKEL");

			// Token: 0x0400AA28 RID: 43560
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ni) Molten Nickel is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x020025C7 RID: 9671
		public class NICKELGAS
		{
			// Token: 0x0400AA29 RID: 43561
			public static LocString NAME = UI.FormatAsLink("Nickel Gas", "NICKELGAS");

			// Token: 0x0400AA2A RID: 43562
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ni) Nickel Gas is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				" heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020025C8 RID: 9672
		public class CREATURE
		{
			// Token: 0x0400AA2B RID: 43563
			public static LocString NAME = UI.FormatAsLink("Genetic Ooze", "CREATURE");

			// Token: 0x0400AA2C RID: 43564
			public static LocString DESC = "(DuPe) Ooze is a slurry of water, carbon, and dozens and dozens of trace elements.\n\nDuplicants are printed from pure Ooze.";
		}

		// Token: 0x020025C9 RID: 9673
		public class PHYTOOIL
		{
			// Token: 0x0400AA2D RID: 43565
			public static LocString NAME = UI.FormatAsLink("Phyto Oil", "PHYTOOIL");

			// Token: 0x0400AA2E RID: 43566
			public static LocString DESC = string.Concat(new string[]
			{
				"Phyto Oil is a thick, slippery ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" extracted from pureed ",
				UI.FormatAsLink("Slime", "SLIME"),
				"."
			});
		}

		// Token: 0x020025CA RID: 9674
		public class REFINEDLIPID
		{
			// Token: 0x0400AA2F RID: 43567
			public static LocString NAME = UI.FormatAsLink("Biodiesel", "REFINEDLIPID");

			// Token: 0x0400AA30 RID: 43568
			public static LocString DESC = string.Concat(new string[]
			{
				"Biodiesel is a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" composed of highly processed fatty acids derived from purified natural oils.\n\nIts ",
				UI.FormatAsLink("combustibility", "COMBUSTIBLELIQUID"),
				" makes it useful for ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x020025CB RID: 9675
		public class FROZENPHYTOOIL
		{
			// Token: 0x0400AA31 RID: 43569
			public static LocString NAME = UI.FormatAsLink("Frozen Phyto Oil", "FROZENPHYTOOIL");

			// Token: 0x0400AA32 RID: 43570
			public static LocString DESC = string.Concat(new string[]
			{
				"Frozen Phyto Oil is thick, slippery ",
				UI.FormatAsLink("Slime", "SLIME"),
				" extract, frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x020025CC RID: 9676
		public class CRUDEOIL
		{
			// Token: 0x0400AA33 RID: 43571
			public static LocString NAME = UI.FormatAsLink("Crude Oil", "CRUDEOIL");

			// Token: 0x0400AA34 RID: 43572
			public static LocString DESC = "Crude Oil is a raw potential " + UI.FormatAsLink("Power", "POWER") + " source composed of billions of dead, primordial organisms.\n\nIt is also a useful lubricant for certain types of machinery.";
		}

		// Token: 0x020025CD RID: 9677
		public class PETROLEUM
		{
			// Token: 0x0400AA35 RID: 43573
			public static LocString NAME = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x0400AA36 RID: 43574
			public static LocString NAME_TWO = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x0400AA37 RID: 43575
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source refined from ",
				UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
				".\n\nIt is also an essential ingredient in the production of ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				"."
			});
		}

		// Token: 0x020025CE RID: 9678
		public class SOURGAS
		{
			// Token: 0x0400AA38 RID: 43576
			public static LocString NAME = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x0400AA39 RID: 43577
			public static LocString NAME_TWO = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x0400AA3A RID: 43578
			public static LocString DESC = string.Concat(new string[]
			{
				"Sour Gas is a hydrocarbon ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" containing high concentrations of hydrogen sulfide.\n\nIt is a byproduct of highly heated ",
				UI.FormatAsLink("Petroleum", "PETROLEUM"),
				"."
			});
		}

		// Token: 0x020025CF RID: 9679
		public class CRUSHEDICE
		{
			// Token: 0x0400AA3B RID: 43579
			public static LocString NAME = UI.FormatAsLink("Crushed Ice", "CRUSHEDICE");

			// Token: 0x0400AA3C RID: 43580
			public static LocString DESC = "(H<sub>2</sub>O) A slush of crushed, semi-solid ice.";
		}

		// Token: 0x020025D0 RID: 9680
		public class CRUSHEDROCK
		{
			// Token: 0x0400AA3D RID: 43581
			public static LocString NAME = UI.FormatAsLink("Crushed Rock", "CRUSHEDROCK");

			// Token: 0x0400AA3E RID: 43582
			public static LocString DESC = "Crushed Rock is " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + " crushed into a mechanical mixture.";
		}

		// Token: 0x020025D1 RID: 9681
		public class CUPRITE
		{
			// Token: 0x0400AA3F RID: 43583
			public static LocString NAME = UI.FormatAsLink("Copper Ore", "CUPRITE");

			// Token: 0x0400AA40 RID: 43584
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu<sub>2</sub>O) Copper Ore is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025D2 RID: 9682
		public class DEPLETEDURANIUM
		{
			// Token: 0x0400AA41 RID: 43585
			public static LocString NAME = UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM");

			// Token: 0x0400AA42 RID: 43586
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Depleted Uranium is ",
				UI.FormatAsLink("Uranium", "URANIUMORE"),
				" with a low U-235 content.\n\nIt is created as a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				" and is no longer suitable as fuel."
			});
		}

		// Token: 0x020025D3 RID: 9683
		public class DIAMOND
		{
			// Token: 0x0400AA43 RID: 43587
			public static LocString NAME = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x0400AA44 RID: 43588
			public static LocString DESC = "(C) Diamond is industrial-grade, high density carbon.\n\nIt is very difficult to excavate.";
		}

		// Token: 0x020025D4 RID: 9684
		public class DIRT
		{
			// Token: 0x0400AA45 RID: 43589
			public static LocString NAME = UI.FormatAsLink("Dirt", "DIRT");

			// Token: 0x0400AA46 RID: 43590
			public static LocString DESC = "Dirt is a soft, nutrient-rich substance capable of supporting life.\n\nIt is necessary in some forms of " + UI.FormatAsLink("Food", "FOOD") + " production.";
		}

		// Token: 0x020025D5 RID: 9685
		public class DIRTYICE
		{
			// Token: 0x0400AA47 RID: 43591
			public static LocString NAME = UI.FormatAsLink("Polluted Ice", "DIRTYICE");

			// Token: 0x0400AA48 RID: 43592
			public static LocString DESC = "Polluted Ice is dirty, unfiltered water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x020025D6 RID: 9686
		public class DIRTYWATER
		{
			// Token: 0x0400AA49 RID: 43593
			public static LocString NAME = UI.FormatAsLink("Polluted Water", "DIRTYWATER");

			// Token: 0x0400AA4A RID: 43594
			public static LocString DESC = "Polluted Water is dirty, unfiltered " + UI.FormatAsLink("Water", "WATER") + ".\n\nIt is not fit for consumption.";
		}

		// Token: 0x020025D7 RID: 9687
		public class ELECTRUM
		{
			// Token: 0x0400AA4B RID: 43595
			public static LocString NAME = UI.FormatAsLink("Electrum", "ELECTRUM");

			// Token: 0x0400AA4C RID: 43596
			public static LocString DESC = string.Concat(new string[]
			{
				"Electrum is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy composed of gold and silver.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025D8 RID: 9688
		public class ENRICHEDURANIUM
		{
			// Token: 0x0400AA4D RID: 43597
			public static LocString NAME = UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM");

			// Token: 0x0400AA4E RID: 43598
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Enriched Uranium is a refined substance primarily used to ",
				UI.FormatAsLink("Power", "POWER"),
				" potent research reactors.\n\nIt becomes highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				" when consumed."
			});
		}

		// Token: 0x020025D9 RID: 9689
		public class FERTILIZER
		{
			// Token: 0x0400AA4F RID: 43599
			public static LocString NAME = UI.FormatAsLink("Fertilizer", "FERTILIZER");

			// Token: 0x0400AA50 RID: 43600
			public static LocString DESC = "Fertilizer is a processed mixture of biological nutrients.\n\nIt aids in the growth of certain " + UI.FormatAsLink("Plants", "PLANTS") + ".";
		}

		// Token: 0x020025DA RID: 9690
		public class PONDSCUM
		{
			// Token: 0x0400AA51 RID: 43601
			public static LocString NAME = UI.FormatAsLink("Pondscum", "PONDSCUM");

			// Token: 0x0400AA52 RID: 43602
			public static LocString DESC = string.Concat(new string[]
			{
				"Pondscum is a soft, naturally occurring composite of biological nutrients.\n\nIt may be processed into ",
				UI.FormatAsLink("Fertilizer", "FERTILIZER"),
				" and aids in the growth of certain ",
				UI.FormatAsLink("Plants", "PLANTS"),
				"."
			});
		}

		// Token: 0x020025DB RID: 9691
		public class FALLOUT
		{
			// Token: 0x0400AA53 RID: 43603
			public static LocString NAME = UI.FormatAsLink("Nuclear Fallout", "FALLOUT");

			// Token: 0x0400AA54 RID: 43604
			public static LocString DESC = string.Concat(new string[]
			{
				"Nuclear Fallout is a highly toxic gas full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				". Condenses into ",
				UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE"),
				"."
			});
		}

		// Token: 0x020025DC RID: 9692
		public class FOOLSGOLD
		{
			// Token: 0x0400AA55 RID: 43605
			public static LocString NAME = UI.FormatAsLink("Pyrite", "FOOLSGOLD");

			// Token: 0x0400AA56 RID: 43606
			public static LocString DESC = string.Concat(new string[]
			{
				"(FeS<sub>2</sub>) Pyrite is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nAlso known as \"Fool's Gold\", is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025DD RID: 9693
		public class FULLERENE
		{
			// Token: 0x0400AA57 RID: 43607
			public static LocString NAME = UI.FormatAsLink("Fullerene", "FULLERENE");

			// Token: 0x0400AA58 RID: 43608
			public static LocString DESC = "(C<sub>60</sub>) Fullerene is a form of " + UI.FormatAsLink("Coal", "CARBON") + " consisting of spherical molecules.";
		}

		// Token: 0x020025DE RID: 9694
		public class GLASS
		{
			// Token: 0x0400AA59 RID: 43609
			public static LocString NAME = UI.FormatAsLink("Glass", "GLASS");

			// Token: 0x0400AA5A RID: 43610
			public static LocString DESC = "Glass is a brittle, transparent substance formed from " + UI.FormatAsLink("Sand", "SAND") + " fired at high temperatures.";
		}

		// Token: 0x020025DF RID: 9695
		public class GOLD
		{
			// Token: 0x0400AA5B RID: 43611
			public static LocString NAME = UI.FormatAsLink("Gold", "GOLD");

			// Token: 0x0400AA5C RID: 43612
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025E0 RID: 9696
		public class GOLDAMALGAM
		{
			// Token: 0x0400AA5D RID: 43613
			public static LocString NAME = UI.FormatAsLink("Gold Amalgam", "GOLDAMALGAM");

			// Token: 0x0400AA5E RID: 43614
			public static LocString DESC = "Gold Amalgam is a conductive amalgam of gold and mercury.\n\nIt is suitable for building " + UI.FormatAsLink("Power", "POWER") + " systems.";
		}

		// Token: 0x020025E1 RID: 9697
		public class GOLDGAS
		{
			// Token: 0x0400AA5F RID: 43615
			public static LocString NAME = UI.FormatAsLink("Gold Gas", "GOLDGAS");

			// Token: 0x0400AA60 RID: 43616
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold Gas is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020025E2 RID: 9698
		public class GRANITE
		{
			// Token: 0x0400AA61 RID: 43617
			public static LocString NAME = UI.FormatAsLink("Granite", "GRANITE");

			// Token: 0x0400AA62 RID: 43618
			public static LocString DESC = "Granite is a dense composite of " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + ".\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020025E3 RID: 9699
		public class GRAPHITE
		{
			// Token: 0x0400AA63 RID: 43619
			public static LocString NAME = UI.FormatAsLink("Graphite", "GRAPHITE");

			// Token: 0x0400AA64 RID: 43620
			public static LocString DESC = "(C) Graphite is the most stable form of carbon.\n\nIt has high thermal conductivity and is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020025E4 RID: 9700
		public class LIQUIDGUNK
		{
			// Token: 0x0400AA65 RID: 43621
			public static LocString NAME = UI.FormatAsLink("Gunk", "LIQUIDGUNK");

			// Token: 0x0400AA66 RID: 43622
			public static LocString DESC = "Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms.\n\nIt is unpleasantly viscous.";
		}

		// Token: 0x020025E5 RID: 9701
		public class GUNK
		{
			// Token: 0x0400AA67 RID: 43623
			public static LocString NAME = UI.FormatAsLink("Solid Gunk", "GUNK");

			// Token: 0x0400AA68 RID: 43624
			public static LocString DESC = "Solid Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms, which has been frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x020025E6 RID: 9702
		public class SOLIDNUCLEARWASTE
		{
			// Token: 0x0400AA69 RID: 43625
			public static LocString NAME = UI.FormatAsLink("Solid Nuclear Waste", "SOLIDNUCLEARWASTE");

			// Token: 0x0400AA6A RID: 43626
			public static LocString DESC = "Highly toxic solid full of " + UI.FormatAsLink("Radioactive Contaminants", "RADIATION") + ".";
		}

		// Token: 0x020025E7 RID: 9703
		public class HELIUM
		{
			// Token: 0x0400AA6B RID: 43627
			public static LocString NAME = UI.FormatAsLink("Helium", "HELIUM");

			// Token: 0x0400AA6C RID: 43628
			public static LocString DESC = "(He) Helium is an atomically lightweight, chemical " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".";
		}

		// Token: 0x020025E8 RID: 9704
		public class HYDROGEN
		{
			// Token: 0x0400AA6D RID: 43629
			public static LocString NAME = UI.FormatAsLink("Hydrogen Gas", "HYDROGEN");

			// Token: 0x0400AA6E RID: 43630
			public static LocString DESC = "(H) Hydrogen Gas is the universe's most common and atomically light element in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x020025E9 RID: 9705
		public class ICE
		{
			// Token: 0x0400AA6F RID: 43631
			public static LocString NAME = UI.FormatAsLink("Ice", "ICE");

			// Token: 0x0400AA70 RID: 43632
			public static LocString DESC = "(H<sub>2</sub>O) Ice is clean water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x020025EA RID: 9706
		public class IGNEOUSROCK
		{
			// Token: 0x0400AA71 RID: 43633
			public static LocString NAME = UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK");

			// Token: 0x0400AA72 RID: 43634
			public static LocString DESC = "Igneous Rock is a composite of solidified volcanic rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020025EB RID: 9707
		public class IRIDIUM
		{
			// Token: 0x0400AA73 RID: 43635
			public static LocString NAME = UI.FormatAsLink("Iridium", "IRIDIUM");

			// Token: 0x0400AA74 RID: 43636
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ir) Iridium is a firm and highly conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				" that can withstand extreme  ",
				UI.FormatAsLink("Heat", "HEAT"),
				"."
			});
		}

		// Token: 0x020025EC RID: 9708
		public class MOLTENIRIDIUM
		{
			// Token: 0x0400AA75 RID: 43637
			public static LocString NAME = UI.FormatAsLink("Molten Iridium", "MOLTENIRIDIUM");

			// Token: 0x0400AA76 RID: 43638
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ir) Molten Iridium is a highly conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				" heated to a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				"  state."
			});
		}

		// Token: 0x020025ED RID: 9709
		public class IRIDIUMGAS
		{
			// Token: 0x0400AA77 RID: 43639
			public static LocString NAME = UI.FormatAsLink("Iridium Gas", "IRIDIUMGAS");

			// Token: 0x0400AA78 RID: 43640
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ir) Iridium Gas is a highly conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				" heated into a  ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020025EE RID: 9710
		public class AMBER
		{
			// Token: 0x0400AA79 RID: 43641
			public static LocString NAME = UI.FormatAsLink("Amber", "AMBER");

			// Token: 0x0400AA7A RID: 43642
			public static LocString DESC = string.Concat(new string[]
			{
				"Amber is a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" organic composite of ",
				UI.FormatAsLink("Resin", "NATURALRESIN"),
				" and ",
				UI.FormatAsLink("Fossil", "FOSSIL"),
				"."
			});
		}

		// Token: 0x020025EF RID: 9711
		public class NATURALRESIN
		{
			// Token: 0x0400AA7B RID: 43643
			public static LocString NAME = UI.FormatAsLink("Resin", "NATURALRESIN");

			// Token: 0x0400AA7C RID: 43644
			public static LocString DESC = string.Concat(new string[]
			{
				"Resin is a viscous organic ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				".\n\nIt can be treated to become ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				" in the ",
				UI.FormatAsLink("Polymer Press", "POLYMERIZER"),
				"."
			});
		}

		// Token: 0x020025F0 RID: 9712
		public class NATURALSOLIDRESIN
		{
			// Token: 0x0400AA7D RID: 43645
			public static LocString NAME = UI.FormatAsLink("Solid Resin", "NATURALSOLIDRESIN");

			// Token: 0x0400AA7E RID: 43646
			public static LocString DESC = string.Concat(new string[]
			{
				"Resin that has been cooled to a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt can be treated to become ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				" in the ",
				UI.FormatAsLink("Polymer Press", "POLYMERIZER"),
				"."
			});
		}

		// Token: 0x020025F1 RID: 9713
		public class ISORESIN
		{
			// Token: 0x0400AA7F RID: 43647
			public static LocString NAME = UI.FormatAsLink("Isosap", "ISORESIN");

			// Token: 0x0400AA80 RID: 43648
			public static LocString DESC = "Isosap is a crystallized sap composed of long-chain polymers.\n\nIt is used in the production of rare, high grade materials.";
		}

		// Token: 0x020025F2 RID: 9714
		public class RESIN
		{
			// Token: 0x0400AA81 RID: 43649
			public static LocString NAME = UI.FormatAsLink("Sap", "RESIN");

			// Token: 0x0400AA82 RID: 43650
			public static LocString DESC = "Sticky goo harvested from a grumpy tree.\n\nIt can be polymerized into " + UI.FormatAsLink("Isosap", "ISORESIN") + " by boiling away its excess moisture.";
		}

		// Token: 0x020025F3 RID: 9715
		public class SOLIDRESIN
		{
			// Token: 0x0400AA83 RID: 43651
			public static LocString NAME = UI.FormatAsLink("Solid Sap", "SOLIDRESIN");

			// Token: 0x0400AA84 RID: 43652
			public static LocString DESC = "Solidified goo harvested from a grumpy tree.\n\nIt is used in the production of " + UI.FormatAsLink("Isosap", "ISORESIN") + ".";
		}

		// Token: 0x020025F4 RID: 9716
		public class IRON
		{
			// Token: 0x0400AA85 RID: 43653
			public static LocString NAME = UI.FormatAsLink("Iron", "IRON");

			// Token: 0x0400AA86 RID: 43654
			public static LocString DESC = "(Fe) Iron is a common industrial " + UI.FormatAsLink("Metal", "RAWMETAL") + ".";
		}

		// Token: 0x020025F5 RID: 9717
		public class IRONGAS
		{
			// Token: 0x0400AA87 RID: 43655
			public static LocString NAME = UI.FormatAsLink("Iron Gas", "IRONGAS");

			// Token: 0x0400AA88 RID: 43656
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Gas is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x020025F6 RID: 9718
		public class IRONORE
		{
			// Token: 0x0400AA89 RID: 43657
			public static LocString NAME = UI.FormatAsLink("Iron Ore", "IRONORE");

			// Token: 0x0400AA8A RID: 43658
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Ore is a soft ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025F7 RID: 9719
		public class COBALTGAS
		{
			// Token: 0x0400AA8B RID: 43659
			public static LocString NAME = UI.FormatAsLink("Cobalt Gas", "COBALTGAS");

			// Token: 0x0400AA8C RID: 43660
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x020025F8 RID: 9720
		public class COBALT
		{
			// Token: 0x0400AA8D RID: 43661
			public static LocString NAME = UI.FormatAsLink("Cobalt", "COBALT");

			// Token: 0x0400AA8E RID: 43662
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" made from ",
				UI.FormatAsLink("Cobalt Ore", "COBALTITE"),
				"."
			});
		}

		// Token: 0x020025F9 RID: 9721
		public class COBALTITE
		{
			// Token: 0x0400AA8F RID: 43663
			public static LocString NAME = UI.FormatAsLink("Cobalt Ore", "COBALTITE");

			// Token: 0x0400AA90 RID: 43664
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt Ore is a blue-hued ",
				UI.FormatAsLink("Metal", "BUILDINGMATERIALCLASSES"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025FA RID: 9722
		public class KATAIRITE
		{
			// Token: 0x0400AA91 RID: 43665
			public static LocString NAME = UI.FormatAsLink("Abyssalite", "KATAIRITE");

			// Token: 0x0400AA92 RID: 43666
			public static LocString DESC = "(Ab) Abyssalite is a resilient, crystalline element.";
		}

		// Token: 0x020025FB RID: 9723
		public class LIME
		{
			// Token: 0x0400AA93 RID: 43667
			public static LocString NAME = UI.FormatAsLink("Lime", "LIME");

			// Token: 0x0400AA94 RID: 43668
			public static LocString DESC = "(CaCO<sub>3</sub>) Lime is a mineral commonly found in " + UI.FormatAsLink("Critter", "CREATURES") + " egg shells.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020025FC RID: 9724
		public class FOSSIL
		{
			// Token: 0x0400AA95 RID: 43669
			public static LocString NAME = UI.FormatAsLink("Fossil", "FOSSIL");

			// Token: 0x0400AA96 RID: 43670
			public static LocString DESC = "Fossil is organic matter, highly compressed and hardened into a mineral state.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020025FD RID: 9725
		public class LEADGAS
		{
			// Token: 0x0400AA97 RID: 43671
			public static LocString NAME = UI.FormatAsLink("Lead Gas", "LEADGAS");

			// Token: 0x0400AA98 RID: 43672
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead Gas is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x020025FE RID: 9726
		public class LEAD
		{
			// Token: 0x0400AA99 RID: 43673
			public static LocString NAME = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x0400AA9A RID: 43674
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				".\n\nIt has a low Overheat Temperature and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020025FF RID: 9727
		public class LIQUIDCARBONDIOXIDE
		{
			// Token: 0x0400AA9B RID: 43675
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon Dioxide", "LIQUIDCARBONDIOXIDE");

			// Token: 0x0400AA9C RID: 43676
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable chemical compound.\n\nThis selection is currently in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002600 RID: 9728
		public class LIQUIDHELIUM
		{
			// Token: 0x0400AA9D RID: 43677
			public static LocString NAME = UI.FormatAsLink("Helium", "LIQUIDHELIUM");

			// Token: 0x0400AA9E RID: 43678
			public static LocString DESC = "(He) Helium is an atomically lightweight chemical element cooled into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002601 RID: 9729
		public class LIQUIDHYDROGEN
		{
			// Token: 0x0400AA9F RID: 43679
			public static LocString NAME = UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN");

			// Token: 0x0400AAA0 RID: 43680
			public static LocString DESC = "(H) Hydrogen in its " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt freezes most substances that come into contact with it.";
		}

		// Token: 0x02002602 RID: 9730
		public class LIQUIDOXYGEN
		{
			// Token: 0x0400AAA1 RID: 43681
			public static LocString NAME = UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN");

			// Token: 0x0400AAA2 RID: 43682
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is a breathable chemical.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002603 RID: 9731
		public class LIQUIDMETHANE
		{
			// Token: 0x0400AAA3 RID: 43683
			public static LocString NAME = UI.FormatAsLink("Liquid Methane", "LIQUIDMETHANE");

			// Token: 0x0400AAA4 RID: 43684
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002604 RID: 9732
		public class LIQUIDPHOSPHORUS
		{
			// Token: 0x0400AAA5 RID: 43685
			public static LocString NAME = UI.FormatAsLink("Liquid Phosphorus", "LIQUIDPHOSPHORUS");

			// Token: 0x0400AAA6 RID: 43686
			public static LocString DESC = "(P) Phosphorus is a chemical element.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002605 RID: 9733
		public class LIQUIDPROPANE
		{
			// Token: 0x0400AAA7 RID: 43687
			public static LocString NAME = UI.FormatAsLink("Liquid Propane", "LIQUIDPROPANE");

			// Token: 0x0400AAA8 RID: 43688
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane is an alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02002606 RID: 9734
		public class LIQUIDSULFUR
		{
			// Token: 0x0400AAA9 RID: 43689
			public static LocString NAME = UI.FormatAsLink("Liquid Sulfur", "LIQUIDSULFUR");

			// Token: 0x0400AAAA RID: 43690
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002607 RID: 9735
		public class MAFICROCK
		{
			// Token: 0x0400AAAB RID: 43691
			public static LocString NAME = UI.FormatAsLink("Mafic Rock", "MAFICROCK");

			// Token: 0x0400AAAC RID: 43692
			public static LocString DESC = string.Concat(new string[]
			{
				"Mafic Rock is a variation of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" that is rich in ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful as a <b>Construction Material</b>."
			});
		}

		// Token: 0x02002608 RID: 9736
		public class MAGMA
		{
			// Token: 0x0400AAAD RID: 43693
			public static LocString NAME = UI.FormatAsLink("Magma", "MAGMA");

			// Token: 0x0400AAAE RID: 43694
			public static LocString DESC = string.Concat(new string[]
			{
				"Magma is a composite of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" heated into a molten, ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002609 RID: 9737
		public class WOODLOG
		{
			// Token: 0x0400AAAF RID: 43695
			public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");

			// Token: 0x0400AAB0 RID: 43696
			public static LocString DESC = string.Concat(new string[]
			{
				"Wood is a good source of ",
				UI.FormatAsLink("Heat", "HEAT"),
				" and ",
				UI.FormatAsLink("Power", "POWER"),
				".\n\nIts insulation properties and positive ",
				UI.FormatAsLink("Decor", "DECOR"),
				" also make it a useful <b>Construction Material</b>."
			});
		}

		// Token: 0x0200260A RID: 9738
		public class FABRICATEDWOOD
		{
			// Token: 0x0400AAB1 RID: 43697
			public static LocString NAME = UI.FormatAsLink("Plywood", "FABRICATEDWOOD");

			// Token: 0x0400AAB2 RID: 43698
			public static LocString DESC = string.Concat(new string[]
			{
				"Plywood is a good source of ",
				UI.FormatAsLink("Heat", "HEAT"),
				" and ",
				UI.FormatAsLink("Power", "POWER"),
				".\n\nIts insulation properties make it a useful <b>Construction Material</b>."
			});
		}

		// Token: 0x0200260B RID: 9739
		public class CINNABAR
		{
			// Token: 0x0400AAB3 RID: 43699
			public static LocString NAME = UI.FormatAsLink("Cinnabar Ore", "CINNABAR");

			// Token: 0x0400AAB4 RID: 43700
			public static LocString DESC = string.Concat(new string[]
			{
				"(HgS) Cinnabar Ore, also known as mercury sulfide, is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" that can be refined into ",
				UI.FormatAsLink("Mercury", "MERCURY"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x0200260C RID: 9740
		public class TALLOW
		{
			// Token: 0x0400AAB5 RID: 43701
			public static LocString NAME = UI.FormatAsLink("Tallow", "TALLOW");

			// Token: 0x0400AAB6 RID: 43702
			public static LocString DESC = "A chunk of raw grease that can be used in " + UI.FormatAsLink("Food", "FOOD") + " production or industrial processes.";
		}

		// Token: 0x0200260D RID: 9741
		public class MERCURY
		{
			// Token: 0x0400AAB7 RID: 43703
			public static LocString NAME = UI.FormatAsLink("Mercury", "MERCURY");

			// Token: 0x0400AAB8 RID: 43704
			public static LocString DESC = "(Hg) Mercury is a metallic " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".";
		}

		// Token: 0x0200260E RID: 9742
		public class MERCURYGAS
		{
			// Token: 0x0400AAB9 RID: 43705
			public static LocString NAME = UI.FormatAsLink("Mercury Gas", "MERCURYGAS");

			// Token: 0x0400AABA RID: 43706
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury Gas is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x0200260F RID: 9743
		public class METHANE
		{
			// Token: 0x0400AABB RID: 43707
			public static LocString NAME = UI.FormatAsLink("Natural Gas", "METHANE");

			// Token: 0x0400AABC RID: 43708
			public static LocString DESC = string.Concat(new string[]
			{
				"Natural Gas is a mixture of various alkanes in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02002610 RID: 9744
		public class MILK
		{
			// Token: 0x0400AABD RID: 43709
			public static LocString NAME = UI.FormatAsLink("Brackene", "MILK");

			// Token: 0x0400AABE RID: 43710
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackene is a sodium-rich ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				".\n\nIt is useful in ",
				UI.FormatAsLink("Ranching", "RANCHING"),
				"."
			});
		}

		// Token: 0x02002611 RID: 9745
		public class MILKFAT
		{
			// Token: 0x0400AABF RID: 43711
			public static LocString NAME = UI.FormatAsLink("Brackwax", "MILKFAT");

			// Token: 0x0400AAC0 RID: 43712
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackwax is a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" byproduct of ",
				UI.FormatAsLink("Brackene", "MILK"),
				"."
			});
		}

		// Token: 0x02002612 RID: 9746
		public class MOLTENCARBON
		{
			// Token: 0x0400AAC1 RID: 43713
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon", "MOLTENCARBON");

			// Token: 0x0400AAC2 RID: 43714
			public static LocString DESC = "(C) Liquid Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002613 RID: 9747
		public class MOLTENCOPPER
		{
			// Token: 0x0400AAC3 RID: 43715
			public static LocString NAME = UI.FormatAsLink("Molten Copper", "MOLTENCOPPER");

			// Token: 0x0400AAC4 RID: 43716
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Molten Copper is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002614 RID: 9748
		public class MOLTENGLASS
		{
			// Token: 0x0400AAC5 RID: 43717
			public static LocString NAME = UI.FormatAsLink("Molten Glass", "MOLTENGLASS");

			// Token: 0x0400AAC6 RID: 43718
			public static LocString DESC = "Molten Glass is a composite of granular rock, heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002615 RID: 9749
		public class MOLTENGOLD
		{
			// Token: 0x0400AAC7 RID: 43719
			public static LocString NAME = UI.FormatAsLink("Molten Gold", "MOLTENGOLD");

			// Token: 0x0400AAC8 RID: 43720
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold, a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002616 RID: 9750
		public class MOLTENIRON
		{
			// Token: 0x0400AAC9 RID: 43721
			public static LocString NAME = UI.FormatAsLink("Molten Iron", "MOLTENIRON");

			// Token: 0x0400AACA RID: 43722
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Molten Iron is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002617 RID: 9751
		public class MOLTENCOBALT
		{
			// Token: 0x0400AACB RID: 43723
			public static LocString NAME = UI.FormatAsLink("Molten Cobalt", "MOLTENCOBALT");

			// Token: 0x0400AACC RID: 43724
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Molten Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002618 RID: 9752
		public class MOLTENLEAD
		{
			// Token: 0x0400AACD RID: 43725
			public static LocString NAME = UI.FormatAsLink("Molten Lead", "MOLTENLEAD");

			// Token: 0x0400AACE RID: 43726
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is an extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002619 RID: 9753
		public class MOLTENNIOBIUM
		{
			// Token: 0x0400AACF RID: 43727
			public static LocString NAME = UI.FormatAsLink("Molten Niobium", "MOLTENNIOBIUM");

			// Token: 0x0400AAD0 RID: 43728
			public static LocString DESC = "(Nb) Molten Niobium is a rare metal heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x0200261A RID: 9754
		public class MOLTENTUNGSTEN
		{
			// Token: 0x0400AAD1 RID: 43729
			public static LocString NAME = UI.FormatAsLink("Molten Tungsten", "MOLTENTUNGSTEN");

			// Token: 0x0400AAD2 RID: 43730
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Molten Tungsten is a crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200261B RID: 9755
		public class MOLTENTUNGSTENDISELENIDE
		{
			// Token: 0x0400AAD3 RID: 43731
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "MOLTENTUNGSTENDISELENIDE");

			// Token: 0x0400AAD4 RID: 43732
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200261C RID: 9756
		public class MOLTENSTEEL
		{
			// Token: 0x0400AAD5 RID: 43733
			public static LocString NAME = UI.FormatAsLink("Molten Steel", "MOLTENSTEEL");

			// Token: 0x0400AAD6 RID: 43734
			public static LocString DESC = string.Concat(new string[]
			{
				"Molten Steel is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy of iron and carbon, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200261D RID: 9757
		public class MOLTENURANIUM
		{
			// Token: 0x0400AAD7 RID: 43735
			public static LocString NAME = UI.FormatAsLink("Liquid Uranium", "MOLTENURANIUM");

			// Token: 0x0400AAD8 RID: 43736
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Liquid Uranium is a highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				" substance, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				"."
			});
		}

		// Token: 0x0200261E RID: 9758
		public class NIOBIUM
		{
			// Token: 0x0400AAD9 RID: 43737
			public static LocString NAME = UI.FormatAsLink("Niobium", "NIOBIUM");

			// Token: 0x0400AADA RID: 43738
			public static LocString DESC = "(Nb) Niobium is a rare metal with many practical applications in metallurgy and superconductor " + UI.FormatAsLink("Research", "RESEARCH") + ".";
		}

		// Token: 0x0200261F RID: 9759
		public class NIOBIUMGAS
		{
			// Token: 0x0400AADB RID: 43739
			public static LocString NAME = UI.FormatAsLink("Niobium Gas", "NIOBIUMGAS");

			// Token: 0x0400AADC RID: 43740
			public static LocString DESC = "(Nb) Niobium Gas is a rare metal.\n\nThis selection is in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02002620 RID: 9760
		public class NUCLEARWASTE
		{
			// Token: 0x0400AADD RID: 43741
			public static LocString NAME = UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE");

			// Token: 0x0400AADE RID: 43742
			public static LocString DESC = string.Concat(new string[]
			{
				"Highly toxic liquid full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				" which emit ",
				UI.FormatAsLink("Radiation", "RADIATION"),
				" that can be absorbed by ",
				UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER"),
				"."
			});
		}

		// Token: 0x02002621 RID: 9761
		public class OBSIDIAN
		{
			// Token: 0x0400AADF RID: 43743
			public static LocString NAME = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x0400AAE0 RID: 43744
			public static LocString DESC = "Obsidian is a brittle composite of volcanic " + UI.FormatAsLink("Glass", "GLASS") + ".";
		}

		// Token: 0x02002622 RID: 9762
		public class OXYGEN
		{
			// Token: 0x0400AAE1 RID: 43745
			public static LocString NAME = UI.FormatAsLink("Oxygen", "OXYGEN");

			// Token: 0x0400AAE2 RID: 43746
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is an atomically lightweight and breathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ", necessary for sustaining life.\n\nIt tends to rise above other gases.";
		}

		// Token: 0x02002623 RID: 9763
		public class OXYROCK
		{
			// Token: 0x0400AAE3 RID: 43747
			public static LocString NAME = UI.FormatAsLink("Oxylite", "OXYROCK");

			// Token: 0x0400AAE4 RID: 43748
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ir<sub>3</sub>O<sub>2</sub>) Oxylite is a chemical compound that slowly emits breathable ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				".\n\nExcavating ",
				ELEMENTS.OXYROCK.NAME,
				" increases its emission rate, but depletes the ore more rapidly."
			});
		}

		// Token: 0x02002624 RID: 9764
		public class PHOSPHATENODULES
		{
			// Token: 0x0400AAE5 RID: 43749
			public static LocString NAME = UI.FormatAsLink("Phosphate Nodules", "PHOSPHATENODULES");

			// Token: 0x0400AAE6 RID: 43750
			public static LocString DESC = "(PO<sup>3-</sup><sub>4</sub>) Nodules of sedimentary rock containing high concentrations of phosphate.";
		}

		// Token: 0x02002625 RID: 9765
		public class PHOSPHORITE
		{
			// Token: 0x0400AAE7 RID: 43751
			public static LocString NAME = UI.FormatAsLink("Phosphorite", "PHOSPHORITE");

			// Token: 0x0400AAE8 RID: 43752
			public static LocString DESC = "Phosphorite is a composite of sedimentary rock, saturated with phosphate.";
		}

		// Token: 0x02002626 RID: 9766
		public class PHOSPHORUS
		{
			// Token: 0x0400AAE9 RID: 43753
			public static LocString NAME = UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS");

			// Token: 0x0400AAEA RID: 43754
			public static LocString DESC = "(P) Refined Phosphorus is a chemical element in its " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002627 RID: 9767
		public class PHOSPHORUSGAS
		{
			// Token: 0x0400AAEB RID: 43755
			public static LocString NAME = UI.FormatAsLink("Phosphorus Gas", "PHOSPHORUSGAS");

			// Token: 0x0400AAEC RID: 43756
			public static LocString DESC = string.Concat(new string[]
			{
				"(P) Phosphorus Gas is the ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state of ",
				UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS"),
				"."
			});
		}

		// Token: 0x02002628 RID: 9768
		public class PROPANE
		{
			// Token: 0x0400AAED RID: 43757
			public static LocString NAME = UI.FormatAsLink("Propane Gas", "PROPANE");

			// Token: 0x0400AAEE RID: 43758
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane Gas is a natural alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02002629 RID: 9769
		public class RADIUM
		{
			// Token: 0x0400AAEF RID: 43759
			public static LocString NAME = UI.FormatAsLink("Radium", "RADIUM");

			// Token: 0x0400AAF0 RID: 43760
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ra) Radium is a ",
				UI.FormatAsLink("Light", "LIGHT"),
				" emitting radioactive substance.\n\nIt is useful as a ",
				UI.FormatAsLink("Power", "POWER"),
				" source."
			});
		}

		// Token: 0x0200262A RID: 9770
		public class YELLOWCAKE
		{
			// Token: 0x0400AAF1 RID: 43761
			public static LocString NAME = UI.FormatAsLink("Yellowcake", "YELLOWCAKE");

			// Token: 0x0400AAF2 RID: 43762
			public static LocString DESC = string.Concat(new string[]
			{
				"(U<sub>3</sub>O<sub>8</sub>) Yellowcake is a byproduct of ",
				UI.FormatAsLink("Uranium", "URANIUM"),
				" mining.\n\nIt is useful in preparing fuel for ",
				UI.FormatAsLink("Research Reactors", "NUCLEARREACTOR"),
				".\n\nNote: Do not eat."
			});
		}

		// Token: 0x0200262B RID: 9771
		public class ROCKGAS
		{
			// Token: 0x0400AAF3 RID: 43763
			public static LocString NAME = UI.FormatAsLink("Rock Gas", "ROCKGAS");

			// Token: 0x0400AAF4 RID: 43764
			public static LocString DESC = "Rock Gas is rock that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x0200262C RID: 9772
		public class RUST
		{
			// Token: 0x0400AAF5 RID: 43765
			public static LocString NAME = UI.FormatAsLink("Rust", "RUST");

			// Token: 0x0400AAF6 RID: 43766
			public static LocString DESC = string.Concat(new string[]
			{
				"Rust is an iron oxide that forms from the breakdown of ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful in some ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				" production processes."
			});
		}

		// Token: 0x0200262D RID: 9773
		public class REGOLITH
		{
			// Token: 0x0400AAF7 RID: 43767
			public static LocString NAME = UI.FormatAsLink("Regolith", "REGOLITH");

			// Token: 0x0400AAF8 RID: 43768
			public static LocString DESC = "Regolith is a sandy substance composed of the various particles that collect atop terrestrial objects.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "FILTER") + ".";
		}

		// Token: 0x0200262E RID: 9774
		public class SALTGAS
		{
			// Token: 0x0400AAF9 RID: 43769
			public static LocString NAME = UI.FormatAsLink("Salt Gas", "SALTGAS");

			// Token: 0x0400AAFA RID: 43770
			public static LocString DESC = "(NaCl) Salt Gas is an edible chemical compound that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x0200262F RID: 9775
		public class MOLTENSALT
		{
			// Token: 0x0400AAFB RID: 43771
			public static LocString NAME = UI.FormatAsLink("Molten Salt", "MOLTENSALT");

			// Token: 0x0400AAFC RID: 43772
			public static LocString DESC = "(NaCl) Molten Salt is an edible chemical compound that has been heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002630 RID: 9776
		public class SALT
		{
			// Token: 0x0400AAFD RID: 43773
			public static LocString NAME = UI.FormatAsLink("Salt", "SALT");

			// Token: 0x0400AAFE RID: 43774
			public static LocString DESC = "(NaCl) Salt, also known as sodium chloride, is an edible chemical compound.\n\nWhen refined, it can be eaten with meals to increase Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
		}

		// Token: 0x02002631 RID: 9777
		public class SALTWATER
		{
			// Token: 0x0400AAFF RID: 43775
			public static LocString NAME = UI.FormatAsLink("Salt Water", "SALTWATER");

			// Token: 0x0400AB00 RID: 43776
			public static LocString DESC = string.Concat(new string[]
			{
				"Salt Water is a natural, lightly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x02002632 RID: 9778
		public class SAND
		{
			// Token: 0x0400AB01 RID: 43777
			public static LocString NAME = UI.FormatAsLink("Sand", "SAND");

			// Token: 0x0400AB02 RID: 43778
			public static LocString DESC = "Sand is a composite of granular rock.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "FILTER") + ".";
		}

		// Token: 0x02002633 RID: 9779
		public class SANDCEMENT
		{
			// Token: 0x0400AB03 RID: 43779
			public static LocString NAME = UI.FormatAsLink("Sand Cement", "SANDCEMENT");

			// Token: 0x0400AB04 RID: 43780
			public static LocString DESC = "";
		}

		// Token: 0x02002634 RID: 9780
		public class SANDSTONE
		{
			// Token: 0x0400AB05 RID: 43781
			public static LocString NAME = UI.FormatAsLink("Sandstone", "SANDSTONE");

			// Token: 0x0400AB06 RID: 43782
			public static LocString DESC = "Sandstone is a composite of relatively soft sedimentary rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02002635 RID: 9781
		public class SEDIMENTARYROCK
		{
			// Token: 0x0400AB07 RID: 43783
			public static LocString NAME = UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK");

			// Token: 0x0400AB08 RID: 43784
			public static LocString DESC = "Sedimentary Rock is a hardened composite of sediment layers.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02002636 RID: 9782
		public class SHALE
		{
			// Token: 0x0400AB09 RID: 43785
			public static LocString NAME = UI.FormatAsLink("Shale", "SHALE");

			// Token: 0x0400AB0A RID: 43786
			public static LocString DESC = "Shale is a brittle composite of sediment layers.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02002637 RID: 9783
		public class SLIMEMOLD
		{
			// Token: 0x0400AB0B RID: 43787
			public static LocString NAME = UI.FormatAsLink("Slime", "SLIMEMOLD");

			// Token: 0x0400AB0C RID: 43788
			public static LocString DESC = string.Concat(new string[]
			{
				"Slime is a thick biomixture of algae, fungi, and mucopolysaccharides.\n\nIt can be distilled into ",
				UI.FormatAsLink("Algae", "ALGAE"),
				" and emits ",
				ELEMENTS.CONTAMINATEDOXYGEN.NAME,
				" once dug up."
			});
		}

		// Token: 0x02002638 RID: 9784
		public class SNOW
		{
			// Token: 0x0400AB0D RID: 43789
			public static LocString NAME = UI.FormatAsLink("Snow", "SNOW");

			// Token: 0x0400AB0E RID: 43790
			public static LocString DESC = "(H<sub>2</sub>O) Snow is a mass of loose, crystalline ice particles.\n\nIt becomes " + UI.FormatAsLink("Water", "WATER") + " when melted.";
		}

		// Token: 0x02002639 RID: 9785
		public class STABLESNOW
		{
			// Token: 0x0400AB0F RID: 43791
			public static LocString NAME = "Packed " + ELEMENTS.SNOW.NAME;

			// Token: 0x0400AB10 RID: 43792
			public static LocString DESC = ELEMENTS.SNOW.DESC;
		}

		// Token: 0x0200263A RID: 9786
		public class SOLIDCARBONDIOXIDE
		{
			// Token: 0x0400AB11 RID: 43793
			public static LocString NAME = UI.FormatAsLink("Solid Carbon Dioxide", "SOLIDCARBONDIOXIDE");

			// Token: 0x0400AB12 RID: 43794
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable compound in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x0200263B RID: 9787
		public class SOLIDCHLORINE
		{
			// Token: 0x0400AB13 RID: 43795
			public static LocString NAME = UI.FormatAsLink("Solid Chlorine", "SOLIDCHLORINE");

			// Token: 0x0400AB14 RID: 43796
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x0200263C RID: 9788
		public class SOLIDCRUDEOIL
		{
			// Token: 0x0400AB15 RID: 43797
			public static LocString NAME = UI.FormatAsLink("Solid Crude Oil", "SOLIDCRUDEOIL");

			// Token: 0x0400AB16 RID: 43798
			public static LocString DESC = "";
		}

		// Token: 0x0200263D RID: 9789
		public class SOLIDHYDROGEN
		{
			// Token: 0x0400AB17 RID: 43799
			public static LocString NAME = UI.FormatAsLink("Solid Hydrogen", "SOLIDHYDROGEN");

			// Token: 0x0400AB18 RID: 43800
			public static LocString DESC = "(H) Solid Hydrogen is the universe's most common element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x0200263E RID: 9790
		public class SOLIDMERCURY
		{
			// Token: 0x0400AB19 RID: 43801
			public static LocString NAME = UI.FormatAsLink("Mercury", "SOLIDMERCURY");

			// Token: 0x0400AB1A RID: 43802
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury is a rare ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x0200263F RID: 9791
		public class SOLIDOXYGEN
		{
			// Token: 0x0400AB1B RID: 43803
			public static LocString NAME = UI.FormatAsLink("Solid Oxygen", "SOLIDOXYGEN");

			// Token: 0x0400AB1C RID: 43804
			public static LocString DESC = "(O<sub>2</sub>) Solid Oxygen is a breathable element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002640 RID: 9792
		public class SOLIDMETHANE
		{
			// Token: 0x0400AB1D RID: 43805
			public static LocString NAME = UI.FormatAsLink("Solid Methane", "SOLIDMETHANE");

			// Token: 0x0400AB1E RID: 43806
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002641 RID: 9793
		public class SOLIDNAPHTHA
		{
			// Token: 0x0400AB1F RID: 43807
			public static LocString NAME = UI.FormatAsLink("Solid Naphtha", "SOLIDNAPHTHA");

			// Token: 0x0400AB20 RID: 43808
			public static LocString DESC = "Naphtha is a distilled hydrocarbon mixture in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002642 RID: 9794
		public class CORIUM
		{
			// Token: 0x0400AB21 RID: 43809
			public static LocString NAME = UI.FormatAsLink("Corium", "CORIUM");

			// Token: 0x0400AB22 RID: 43810
			public static LocString DESC = "A radioactive mixture of nuclear waste and melted reactor materials.\n\nReleases " + UI.FormatAsLink("Nuclear Fallout", "FALLOUT") + " gas.";
		}

		// Token: 0x02002643 RID: 9795
		public class SOLIDPETROLEUM
		{
			// Token: 0x0400AB23 RID: 43811
			public static LocString NAME = UI.FormatAsLink("Solid Petroleum", "SOLIDPETROLEUM");

			// Token: 0x0400AB24 RID: 43812
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02002644 RID: 9796
		public class SOLIDPROPANE
		{
			// Token: 0x0400AB25 RID: 43813
			public static LocString NAME = UI.FormatAsLink("Solid Propane", "SOLIDPROPANE");

			// Token: 0x0400AB26 RID: 43814
			public static LocString DESC = "(C<sub>3</sub>H<sub>8</sub>) Solid Propane is a natural gas in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002645 RID: 9797
		public class SOLIDSUPERCOOLANT
		{
			// Token: 0x0400AB27 RID: 43815
			public static LocString NAME = UI.FormatAsLink("Solid Super Coolant", "SOLIDSUPERCOOLANT");

			// Token: 0x0400AB28 RID: 43816
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02002646 RID: 9798
		public class SOLIDVISCOGEL
		{
			// Token: 0x0400AB29 RID: 43817
			public static LocString NAME = UI.FormatAsLink("Solid Visco-Gel", "SOLIDVISCOGEL");

			// Token: 0x0400AB2A RID: 43818
			public static LocString DESC = string.Concat(new string[]
			{
				"Visco-Gel is a polymer that has high surface tension when in ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" form.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02002647 RID: 9799
		public class SYNGAS
		{
			// Token: 0x0400AB2B RID: 43819
			public static LocString NAME = UI.FormatAsLink("Synthesis Gas", "SYNGAS");

			// Token: 0x0400AB2C RID: 43820
			public static LocString DESC = "Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02002648 RID: 9800
		public class MOLTENSYNGAS
		{
			// Token: 0x0400AB2D RID: 43821
			public static LocString NAME = UI.FormatAsLink("Molten Synthesis Gas", "SYNGAS");

			// Token: 0x0400AB2E RID: 43822
			public static LocString DESC = "Molten Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02002649 RID: 9801
		public class SOLIDSYNGAS
		{
			// Token: 0x0400AB2F RID: 43823
			public static LocString NAME = UI.FormatAsLink("Solid Synthesis Gas", "SYNGAS");

			// Token: 0x0400AB30 RID: 43824
			public static LocString DESC = "Solid Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x0200264A RID: 9802
		public class STEAM
		{
			// Token: 0x0400AB31 RID: 43825
			public static LocString NAME = UI.FormatAsLink("Steam", "STEAM");

			// Token: 0x0400AB32 RID: 43826
			public static LocString DESC = string.Concat(new string[]
			{
				"(H<sub>2</sub>O) Steam is ",
				ELEMENTS.WATER.NAME,
				" that has been heated into a scalding ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x0200264B RID: 9803
		public class STEEL
		{
			// Token: 0x0400AB33 RID: 43827
			public static LocString NAME = UI.FormatAsLink("Steel", "STEEL");

			// Token: 0x0400AB34 RID: 43828
			public static LocString DESC = "Steel is a " + UI.FormatAsLink("Metal Alloy", "REFINEDMETAL") + " composed of iron and carbon.";
		}

		// Token: 0x0200264C RID: 9804
		public class STEELGAS
		{
			// Token: 0x0400AB35 RID: 43829
			public static LocString NAME = UI.FormatAsLink("Steel Gas", "STEELGAS");

			// Token: 0x0400AB36 RID: 43830
			public static LocString DESC = string.Concat(new string[]
			{
				"Steel Gas is a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" composed of iron and carbon."
			});
		}

		// Token: 0x0200264D RID: 9805
		public class SUGARWATER
		{
			// Token: 0x0400AB37 RID: 43831
			public static LocString NAME = UI.FormatAsLink("Nectar", "SUGARWATER");

			// Token: 0x0400AB38 RID: 43832
			public static LocString DESC = string.Concat(new string[]
			{
				"Nectar is a natural, lightly concentrated solution of ",
				UI.FormatAsLink("Sucrose", "SUCROSE"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				"."
			});
		}

		// Token: 0x0200264E RID: 9806
		public class SULFUR
		{
			// Token: 0x0400AB39 RID: 43833
			public static LocString NAME = UI.FormatAsLink("Sulfur", "SULFUR");

			// Token: 0x0400AB3A RID: 43834
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x0200264F RID: 9807
		public class SULFURGAS
		{
			// Token: 0x0400AB3B RID: 43835
			public static LocString NAME = UI.FormatAsLink("Sulfur Gas", "SULFURGAS");

			// Token: 0x0400AB3C RID: 43836
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02002650 RID: 9808
		public class SUPERCOOLANT
		{
			// Token: 0x0400AB3D RID: 43837
			public static LocString NAME = UI.FormatAsLink("Super Coolant", "SUPERCOOLANT");

			// Token: 0x0400AB3E RID: 43838
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade coolant that utilizes the unusual energy states of ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002651 RID: 9809
		public class SUPERCOOLANTGAS
		{
			// Token: 0x0400AB3F RID: 43839
			public static LocString NAME = UI.FormatAsLink("Super Coolant Gas", "SUPERCOOLANTGAS");

			// Token: 0x0400AB40 RID: 43840
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02002652 RID: 9810
		public class SUPERINSULATOR
		{
			// Token: 0x0400AB41 RID: 43841
			public static LocString NAME = UI.FormatAsLink("Insulite", "SUPERINSULATOR");

			// Token: 0x0400AB42 RID: 43842
			public static LocString DESC = string.Concat(new string[]
			{
				"Insulite reduces ",
				UI.FormatAsLink("Heat Transfer", "HEAT"),
				" and is composed of recrystallized ",
				UI.FormatAsLink("Abyssalite", "KATAIRITE"),
				"."
			});
		}

		// Token: 0x02002653 RID: 9811
		public class TEMPCONDUCTORSOLID
		{
			// Token: 0x0400AB43 RID: 43843
			public static LocString NAME = UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID");

			// Token: 0x0400AB44 RID: 43844
			public static LocString DESC = "Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.";
		}

		// Token: 0x02002654 RID: 9812
		public class TUNGSTEN
		{
			// Token: 0x0400AB45 RID: 43845
			public static LocString NAME = UI.FormatAsLink("Tungsten", "TUNGSTEN");

			// Token: 0x0400AB46 RID: 43846
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is an extremely tough crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002655 RID: 9813
		public class TUNGSTENGAS
		{
			// Token: 0x0400AB47 RID: 43847
			public static LocString NAME = UI.FormatAsLink("Tungsten Gas", "TUNGSTENGAS");

			// Token: 0x0400AB48 RID: 43848
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is a superheated crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02002656 RID: 9814
		public class TUNGSTENDISELENIDE
		{
			// Token: 0x0400AB49 RID: 43849
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "TUNGSTENDISELENIDE");

			// Token: 0x0400AB4A RID: 43850
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound with a crystalline structure.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002657 RID: 9815
		public class TUNGSTENDISELENIDEGAS
		{
			// Token: 0x0400AB4B RID: 43851
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide Gas", "TUNGSTENDISELENIDEGAS");

			// Token: 0x0400AB4C RID: 43852
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide Gasis a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02002658 RID: 9816
		public class TOXICSAND
		{
			// Token: 0x0400AB4D RID: 43853
			public static LocString NAME = UI.FormatAsLink("Polluted Dirt", "TOXICSAND");

			// Token: 0x0400AB4E RID: 43854
			public static LocString DESC = "Polluted Dirt is unprocessed biological waste.\n\nIt emits " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " over time.";
		}

		// Token: 0x02002659 RID: 9817
		public class UNOBTANIUM
		{
			// Token: 0x0400AB4F RID: 43855
			public static LocString NAME = UI.FormatAsLink("Neutronium", "UNOBTANIUM");

			// Token: 0x0400AB50 RID: 43856
			public static LocString DESC = "(Nt) Neutronium is a mysterious and extremely resilient element.\n\nIt cannot be excavated by any Duplicant mining tool.";
		}

		// Token: 0x0200265A RID: 9818
		public class URANIUMORE
		{
			// Token: 0x0400AB51 RID: 43857
			public static LocString NAME = UI.FormatAsLink("Uranium Ore", "URANIUMORE");

			// Token: 0x0400AB52 RID: 43858
			public static LocString DESC = "(U) Uranium Ore is a highly " + UI.FormatAsLink("Radioactive", "RADIATION") + " substance.\n\nIt can be refined into fuel for research reactors.";
		}

		// Token: 0x0200265B RID: 9819
		public class VACUUM
		{
			// Token: 0x0400AB53 RID: 43859
			public static LocString NAME = UI.FormatAsLink("Vacuum", "VACUUM");

			// Token: 0x0400AB54 RID: 43860
			public static LocString DESC = "A vacuum is a space devoid of all matter.";
		}

		// Token: 0x0200265C RID: 9820
		public class VISCOGEL
		{
			// Token: 0x0400AB55 RID: 43861
			public static LocString NAME = UI.FormatAsLink("Visco-Gel Fluid", "VISCOGEL");

			// Token: 0x0400AB56 RID: 43862
			public static LocString DESC = "Visco-Gel Fluid is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension, preventing typical liquid flow and allowing for unusual configurations.";
		}

		// Token: 0x0200265D RID: 9821
		public class VOID
		{
			// Token: 0x0400AB57 RID: 43863
			public static LocString NAME = UI.FormatAsLink("Void", "VOID");

			// Token: 0x0400AB58 RID: 43864
			public static LocString DESC = "Cold, infinite nothingness.";
		}

		// Token: 0x0200265E RID: 9822
		public class COMPOSITION
		{
			// Token: 0x0400AB59 RID: 43865
			public static LocString NAME = UI.FormatAsLink("Composition", "COMPOSITION");

			// Token: 0x0400AB5A RID: 43866
			public static LocString DESC = "A mixture of two or more elements.";
		}

		// Token: 0x0200265F RID: 9823
		public class WATER
		{
			// Token: 0x0400AB5B RID: 43867
			public static LocString NAME = UI.FormatAsLink("Water", "WATER");

			// Token: 0x0400AB5C RID: 43868
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + UI.FormatAsLink("Water", "WATER") + ", suitable for consumption.";
		}

		// Token: 0x02002660 RID: 9824
		public class WOLFRAMITE
		{
			// Token: 0x0400AB5D RID: 43869
			public static LocString NAME = UI.FormatAsLink("Wolframite", "WOLFRAMITE");

			// Token: 0x0400AB5E RID: 43870
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002661 RID: 9825
		public class TESTELEMENT
		{
			// Token: 0x0400AB5F RID: 43871
			public static LocString NAME = UI.FormatAsLink("Test Element", "TESTELEMENT");

			// Token: 0x0400AB60 RID: 43872
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002662 RID: 9826
		public class POLYPROPYLENE
		{
			// Token: 0x0400AB61 RID: 43873
			public static LocString NAME = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x0400AB62 RID: 43874
			public static LocString DESC = "(C<sub>3</sub>H<sub>6</sub>)<sub>n</sub> " + ELEMENTS.POLYPROPYLENE.NAME + " is a thermoplastic polymer.\n\nIt is useful for constructing a variety of advanced buildings and equipment.";

			// Token: 0x0400AB63 RID: 43875
			public static LocString BUILD_DESC = "Buildings made of this " + ELEMENTS.POLYPROPYLENE.NAME + " have antiseptic properties";
		}

		// Token: 0x02002663 RID: 9827
		public class HARDPOLYPROPYLENE
		{
			// Token: 0x0400AB64 RID: 43876
			public static LocString NAME = UI.FormatAsLink("Plastium", "HARDPOLYPROPYLENE");

			// Token: 0x0400AB65 RID: 43877
			public static LocString DESC = string.Concat(new string[]
			{
				ELEMENTS.HARDPOLYPROPYLENE.NAME,
				" is an advanced thermoplastic polymer made from ",
				UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID"),
				", ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				" and ",
				UI.FormatAsLink("Brackwax", "MILKFAT"),
				".\n\nIt is highly heat-resistant and suitable for use in space buildings."
			});
		}

		// Token: 0x02002664 RID: 9828
		public class NAPHTHA
		{
			// Token: 0x0400AB66 RID: 43878
			public static LocString NAME = UI.FormatAsLink("Liquid Naphtha", "NAPHTHA");

			// Token: 0x0400AB67 RID: 43879
			public static LocString DESC = "Naphtha a distilled hydrocarbon mixture produced from the burning of " + UI.FormatAsLink("Plastic", "POLYPROPYLENE") + ".";
		}

		// Token: 0x02002665 RID: 9829
		public class SLABS
		{
			// Token: 0x0400AB68 RID: 43880
			public static LocString NAME = UI.FormatAsLink("Building Slab", "SLABS");

			// Token: 0x0400AB69 RID: 43881
			public static LocString DESC = "Slabs are a refined mineral building block used for assembling advanced buildings.";
		}

		// Token: 0x02002666 RID: 9830
		public class TOXICMUD
		{
			// Token: 0x0400AB6A RID: 43882
			public static LocString NAME = UI.FormatAsLink("Polluted Mud", "TOXICMUD");

			// Token: 0x0400AB6B RID: 43883
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				" and ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02002667 RID: 9831
		public class MUD
		{
			// Token: 0x0400AB6C RID: 43884
			public static LocString NAME = UI.FormatAsLink("Mud", "MUD");

			// Token: 0x0400AB6D RID: 43885
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Dirt", "DIRT"),
				" and ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02002668 RID: 9832
		public class SUCROSE
		{
			// Token: 0x0400AB6E RID: 43886
			public static LocString NAME = UI.FormatAsLink("Sucrose", "SUCROSE");

			// Token: 0x0400AB6F RID: 43887
			public static LocString DESC = "(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Sucrose is the raw form of sugar.\n\nIt can be used for cooking higher-quality " + UI.FormatAsLink("Food", "FOOD") + ".";
		}

		// Token: 0x02002669 RID: 9833
		public class MOLTENSUCROSE
		{
			// Token: 0x0400AB70 RID: 43888
			public static LocString NAME = UI.FormatAsLink("Liquid Sucrose", "MOLTENSUCROSE");

			// Token: 0x0400AB71 RID: 43889
			public static LocString DESC = "(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Liquid Sucrose is the raw form of sugar, heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}
	}
}
