using System;

namespace STRINGS
{
	// Token: 0x02000FFB RID: 4091
	public class ROBOTS
	{
		// Token: 0x04006049 RID: 24649
		public static LocString CATEGORY_NAME = "Robots";

		// Token: 0x0200259E RID: 9630
		public class STATS
		{
			// Token: 0x02003940 RID: 14656
			public class INTERNALBATTERY
			{
				// Token: 0x0400E7FF RID: 59391
				public static LocString NAME = "Rechargeable Battery";

				// Token: 0x0400E800 RID: 59392
				public static LocString TOOLTIP = "When this bot's battery runs out it must temporarily stop working to go recharge";
			}

			// Token: 0x02003941 RID: 14657
			public class INTERNALCHEMICALBATTERY
			{
				// Token: 0x0400E801 RID: 59393
				public static LocString NAME = "Chemical Battery";

				// Token: 0x0400E802 RID: 59394
				public static LocString TOOLTIP = "This bot will shut down permanently when its battery runs out";
			}

			// Token: 0x02003942 RID: 14658
			public class INTERNALBIOBATTERY
			{
				// Token: 0x0400E803 RID: 59395
				public static LocString NAME = "Biofuel";

				// Token: 0x0400E804 RID: 59396
				public static LocString TOOLTIP = "This bot will shut down permanently when its biofuel runs out";
			}

			// Token: 0x02003943 RID: 14659
			public class INTERNALELECTROBANK
			{
				// Token: 0x0400E805 RID: 59397
				public static LocString NAME = "Power Bank";

				// Token: 0x0400E806 RID: 59398
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"When this bot's ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" runs out, it will stop working until a fully charged one is delivered"
				});
			}
		}

		// Token: 0x0200259F RID: 9631
		public class ATTRIBUTES
		{
			// Token: 0x02003944 RID: 14660
			public class INTERNALBATTERYDELTA
			{
				// Token: 0x0400E807 RID: 59399
				public static LocString NAME = "Rechargeable Battery Drain";

				// Token: 0x0400E808 RID: 59400
				public static LocString TOOLTIP = "The rate at which battery life is depleted";
			}
		}

		// Token: 0x020025A0 RID: 9632
		public class STATUSITEMS
		{
			// Token: 0x02003945 RID: 14661
			public class CANTREACHSTATION
			{
				// Token: 0x0400E809 RID: 59401
				public static LocString NAME = "Unreachable Dock";

				// Token: 0x0400E80A RID: 59402
				public static LocString DESC = "Obstacles are preventing {0} from heading home";

				// Token: 0x0400E80B RID: 59403
				public static LocString TOOLTIP = "Obstacles are preventing {0} from heading home";
			}

			// Token: 0x02003946 RID: 14662
			public class MOVINGTOCHARGESTATION
			{
				// Token: 0x0400E80C RID: 59404
				public static LocString NAME = "Traveling to Dock";

				// Token: 0x0400E80D RID: 59405
				public static LocString DESC = "{0} is on its way home to recharge";

				// Token: 0x0400E80E RID: 59406
				public static LocString TOOLTIP = "{0} is on its way home to recharge";
			}

			// Token: 0x02003947 RID: 14663
			public class LOWBATTERY
			{
				// Token: 0x0400E80F RID: 59407
				public static LocString NAME = "Low Battery";

				// Token: 0x0400E810 RID: 59408
				public static LocString DESC = "{0}'s battery is low and needs to recharge";

				// Token: 0x0400E811 RID: 59409
				public static LocString TOOLTIP = "{0}'s battery is low and needs to recharge";
			}

			// Token: 0x02003948 RID: 14664
			public class LOWBATTERYNOCHARGE
			{
				// Token: 0x0400E812 RID: 59410
				public static LocString NAME = "Low Battery";

				// Token: 0x0400E813 RID: 59411
				public static LocString DESC = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";

				// Token: 0x0400E814 RID: 59412
				public static LocString TOOLTIP = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";
			}

			// Token: 0x02003949 RID: 14665
			public class DEADBATTERY
			{
				// Token: 0x0400E815 RID: 59413
				public static LocString NAME = "Shut Down";

				// Token: 0x0400E816 RID: 59414
				public static LocString DESC = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";

				// Token: 0x0400E817 RID: 59415
				public static LocString TOOLTIP = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";
			}

			// Token: 0x0200394A RID: 14666
			public class DEADBATTERYFLYDO
			{
				// Token: 0x0400E818 RID: 59416
				public static LocString NAME = "Shut Down";

				// Token: 0x0400E819 RID: 59417
				public static LocString DESC = "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";

				// Token: 0x0400E81A RID: 59418
				public static LocString TOOLTIP = "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";
			}

			// Token: 0x0200394B RID: 14667
			public class DUSTBINFULL
			{
				// Token: 0x0400E81B RID: 59419
				public static LocString NAME = "Dust Bin Full";

				// Token: 0x0400E81C RID: 59420
				public static LocString DESC = "{0} must return to its dock to unload";

				// Token: 0x0400E81D RID: 59421
				public static LocString TOOLTIP = "{0} must return to its dock to unload";
			}

			// Token: 0x0200394C RID: 14668
			public class WORKING
			{
				// Token: 0x0400E81E RID: 59422
				public static LocString NAME = "Working";

				// Token: 0x0400E81F RID: 59423
				public static LocString DESC = "{0} is working diligently. Great job, {0}!";

				// Token: 0x0400E820 RID: 59424
				public static LocString TOOLTIP = "{0} is working diligently. Great job, {0}!";
			}

			// Token: 0x0200394D RID: 14669
			public class UNLOADINGSTORAGE
			{
				// Token: 0x0400E821 RID: 59425
				public static LocString NAME = "Unloading";

				// Token: 0x0400E822 RID: 59426
				public static LocString DESC = "{0} is emptying out its dust bin";

				// Token: 0x0400E823 RID: 59427
				public static LocString TOOLTIP = "{0} is emptying out its dust bin";
			}

			// Token: 0x0200394E RID: 14670
			public class CHARGING
			{
				// Token: 0x0400E824 RID: 59428
				public static LocString NAME = "Charging";

				// Token: 0x0400E825 RID: 59429
				public static LocString DESC = "{0} is recharging its battery";

				// Token: 0x0400E826 RID: 59430
				public static LocString TOOLTIP = "{0} is recharging its battery";
			}

			// Token: 0x0200394F RID: 14671
			public class REACTPOSITIVE
			{
				// Token: 0x0400E827 RID: 59431
				public static LocString NAME = "Happy Reaction";

				// Token: 0x0400E828 RID: 59432
				public static LocString DESC = "This bot saw something nice!";

				// Token: 0x0400E829 RID: 59433
				public static LocString TOOLTIP = "This bot saw something nice!";
			}

			// Token: 0x02003950 RID: 14672
			public class REACTNEGATIVE
			{
				// Token: 0x0400E82A RID: 59434
				public static LocString NAME = "Bothered Reaction";

				// Token: 0x0400E82B RID: 59435
				public static LocString DESC = "This bot saw something upsetting";

				// Token: 0x0400E82C RID: 59436
				public static LocString TOOLTIP = "This bot saw something upsetting";
			}
		}

		// Token: 0x020025A1 RID: 9633
		public class MODELS
		{
			// Token: 0x02003951 RID: 14673
			public class MORB
			{
				// Token: 0x0400E82D RID: 59437
				public static LocString NAME = UI.FormatAsLink("Biobot", "STORYTRAITMORBROVER");

				// Token: 0x0400E82E RID: 59438
				public static LocString DESC = "A Pathogen-Fueled Extravehicular Geo-Exploratory Guidebot (model Y), aka \"P.E.G.G.Y.\"\n\nIt can be assigned basic building tasks and digging duties in hazardous environments.";

				// Token: 0x0400E82F RID: 59439
				public static LocString CODEX_DESC = "The pathogen-fueled guidebot is designed to maximize a colony's chances of surviving in hostile environments by meeting three core outcomes:\n\n1. Filtration and removal of toxins from environment;\n2. Safe disposal of filtered toxins through conversion into usable biofuel;\n3. Creation of geo-exploration equipment for colony expansion with minimal colonist endangerment.\n\nThe elements aggregated during this process may result in the unintentional spread of contaminants. Specialized training required for safe handling.";
			}

			// Token: 0x02003952 RID: 14674
			public class SCOUT
			{
				// Token: 0x0400E830 RID: 59440
				public static LocString NAME = UI.FormatAsLink("Rover", "SCOUTROVER");

				// Token: 0x0400E831 RID: 59441
				public static LocString DESC = "A curious bot that can remotely explore new " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " locations.";
			}

			// Token: 0x02003953 RID: 14675
			public class SWEEPBOT
			{
				// Token: 0x0400E832 RID: 59442
				public static LocString NAME = UI.FormatAsLink("Sweepy", "SWEEPY");

				// Token: 0x0400E833 RID: 59443
				public static LocString DESC = string.Concat(new string[]
				{
					"An automated sweeping robot.\n\nSweeps up ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					" debris and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" spills and stores the material back in its ",
					UI.FormatAsLink("Sweepy Dock", "SWEEPBOTSTATION"),
					"."
				});
			}

			// Token: 0x02003954 RID: 14676
			public class FLYDO
			{
				// Token: 0x0400E834 RID: 59444
				public static LocString NAME = UI.FormatAsLink("Flydo", "FETCHDRONE");

				// Token: 0x0400E835 RID: 59445
				public static LocString DESC = "A programmable delivery robot.\n\nPicks up " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " objects for delivery to selected destinations.";
			}
		}
	}
}
