using System;

namespace STRINGS
{
	// Token: 0x02000FF4 RID: 4084
	public class BUILDING
	{
		// Token: 0x02002563 RID: 9571
		public class STATUSITEMS
		{
			// Token: 0x02003156 RID: 12630
			public class GUNKEMPTIERFULL
			{
				// Token: 0x0400D4BD RID: 54461
				public static LocString NAME = "Storage Full";

				// Token: 0x0400D4BE RID: 54462
				public static LocString TOOLTIP = "This building's internal storage is at maximum capacity\n\nIt must be emptied before its next use";
			}

			// Token: 0x02003157 RID: 12631
			public class MERCURYLIGHT_CHARGING
			{
				// Token: 0x0400D4BF RID: 54463
				public static LocString NAME = "Powering Up: {0}";

				// Token: 0x0400D4C0 RID: 54464
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" levels are gradually increasing\n\nIf its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements continue to be met, it will reach maximum brightness in {0}"
				});
			}

			// Token: 0x02003158 RID: 12632
			public class MERCURYLIGHT_DEPLEATING
			{
				// Token: 0x0400D4C1 RID: 54465
				public static LocString NAME = "Brightness: {0}";

				// Token: 0x0400D4C2 RID: 54466
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" output is decreasing because its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements are not being met\n\nIt will power off once its stores are depleted"
				});
			}

			// Token: 0x02003159 RID: 12633
			public class MERCURYLIGHT_DEPLEATED
			{
				// Token: 0x0400D4C3 RID: 54467
				public static LocString NAME = "Powered Off";

				// Token: 0x0400D4C4 RID: 54468
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is non-operational due to a lack of resources\n\nIt will begin to power up when its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements are met"
				});
			}

			// Token: 0x0200315A RID: 12634
			public class MERCURYLIGHT_CHARGED
			{
				// Token: 0x0400D4C5 RID: 54469
				public static LocString NAME = "Fully Charged";

				// Token: 0x0400D4C6 RID: 54470
				public static LocString TOOLTIP = "This building is functioning at maximum capacity";
			}

			// Token: 0x0200315B RID: 12635
			public class SPECIALCARGOBAYCLUSTERCRITTERSTORED
			{
				// Token: 0x0400D4C7 RID: 54471
				public static LocString NAME = "Contents: {0}";

				// Token: 0x0400D4C8 RID: 54472
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200315C RID: 12636
			public class GEOTUNER_NEEDGEYSER
			{
				// Token: 0x0400D4C9 RID: 54473
				public static LocString NAME = "No Geyser Selected";

				// Token: 0x0400D4CA RID: 54474
				public static LocString TOOLTIP = "Select an analyzed geyser to increase its output";
			}

			// Token: 0x0200315D RID: 12637
			public class GEOTUNER_CHARGE_REQUIRED
			{
				// Token: 0x0400D4CB RID: 54475
				public static LocString NAME = "Experimentation Needed";

				// Token: 0x0400D4CC RID: 54476
				public static LocString TOOLTIP = "This building requires a Duplicant to produce amplification data through experimentation";
			}

			// Token: 0x0200315E RID: 12638
			public class GEOTUNER_CHARGING
			{
				// Token: 0x0400D4CD RID: 54477
				public static LocString NAME = "Compiling Data";

				// Token: 0x0400D4CE RID: 54478
				public static LocString TOOLTIP = "Compiling amplification data through experimentation";
			}

			// Token: 0x0200315F RID: 12639
			public class GEOTUNER_CHARGED
			{
				// Token: 0x0400D4CF RID: 54479
				public static LocString NAME = "Data Remaining: {0}";

				// Token: 0x0400D4D0 RID: 54480
				public static LocString TOOLTIP = "This building consumes amplification data while boosting a geyser\n\nTime remaining: {0} ({1} data per second)";
			}

			// Token: 0x02003160 RID: 12640
			public class GEOTUNER_GEYSER_STATUS
			{
				// Token: 0x0400D4D1 RID: 54481
				public static LocString NAME = "";

				// Token: 0x0400D4D2 RID: 54482
				public static LocString NAME_ERUPTING = "Target is Erupting";

				// Token: 0x0400D4D3 RID: 54483
				public static LocString NAME_DORMANT = "Target is Not Erupting";

				// Token: 0x0400D4D4 RID: 54484
				public static LocString NAME_IDLE = "Target is Not Erupting";

				// Token: 0x0400D4D5 RID: 54485
				public static LocString TOOLTIP = "";

				// Token: 0x0400D4D6 RID: 54486
				public static LocString TOOLTIP_ERUPTING = "The selected geyser is erupting and will receive stored amplification data";

				// Token: 0x0400D4D7 RID: 54487
				public static LocString TOOLTIP_DORMANT = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";

				// Token: 0x0400D4D8 RID: 54488
				public static LocString TOOLTIP_IDLE = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";
			}

			// Token: 0x02003161 RID: 12641
			public class GEYSER_GEOTUNED
			{
				// Token: 0x0400D4D9 RID: 54489
				public static LocString NAME = "Geotuned ({0}/{1})";

				// Token: 0x0400D4DA RID: 54490
				public static LocString TOOLTIP = "This geyser is being boosted by {0} out {1} of " + UI.PRE_KEYWORD + "Geotuners" + UI.PST_KEYWORD;
			}

			// Token: 0x02003162 RID: 12642
			public class RADIATOR_ENERGY_CURRENT_EMISSION_RATE
			{
				// Token: 0x0400D4DB RID: 54491
				public static LocString NAME = "Currently Emitting: {ENERGY_RATE}";

				// Token: 0x0400D4DC RID: 54492
				public static LocString TOOLTIP = "Currently Emitting: {ENERGY_RATE}";
			}

			// Token: 0x02003163 RID: 12643
			public class NOTLINKEDTOHEAD
			{
				// Token: 0x0400D4DD RID: 54493
				public static LocString NAME = "Not Linked";

				// Token: 0x0400D4DE RID: 54494
				public static LocString TOOLTIP = "This building must be built adjacent to a {headBuilding} or another {linkBuilding} in order to function";
			}

			// Token: 0x02003164 RID: 12644
			public class BAITED
			{
				// Token: 0x0400D4DF RID: 54495
				public static LocString NAME = "{0} Bait";

				// Token: 0x0400D4E0 RID: 54496
				public static LocString TOOLTIP = "This lure is baited with {0}\n\nBait material is set during the construction of the building";
			}

			// Token: 0x02003165 RID: 12645
			public class NOCOOLANT
			{
				// Token: 0x0400D4E1 RID: 54497
				public static LocString NAME = "No Coolant";

				// Token: 0x0400D4E2 RID: 54498
				public static LocString TOOLTIP = "This building needs coolant";
			}

			// Token: 0x02003166 RID: 12646
			public class ANGERDAMAGE
			{
				// Token: 0x0400D4E3 RID: 54499
				public static LocString NAME = "Damage: Duplicant Tantrum";

				// Token: 0x0400D4E4 RID: 54500
				public static LocString TOOLTIP = "A stressed Duplicant is damaging this building";

				// Token: 0x0400D4E5 RID: 54501
				public static LocString NOTIFICATION = "Building Damage: Duplicant Tantrum";

				// Token: 0x0400D4E6 RID: 54502
				public static LocString NOTIFICATION_TOOLTIP = "Stressed Duplicants are damaging these buildings:\n\n{0}";
			}

			// Token: 0x02003167 RID: 12647
			public class PIPECONTENTS
			{
				// Token: 0x0400D4E7 RID: 54503
				public static LocString EMPTY = "Empty";

				// Token: 0x0400D4E8 RID: 54504
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400D4E9 RID: 54505
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x02003168 RID: 12648
			public class CONVEYOR_CONTENTS
			{
				// Token: 0x0400D4EA RID: 54506
				public static LocString EMPTY = "Empty";

				// Token: 0x0400D4EB RID: 54507
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400D4EC RID: 54508
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x02003169 RID: 12649
			public class ASSIGNEDTO
			{
				// Token: 0x0400D4ED RID: 54509
				public static LocString NAME = "Assigned to: {Assignee}";

				// Token: 0x0400D4EE RID: 54510
				public static LocString TOOLTIP = "Only {Assignee} can use this amenity";
			}

			// Token: 0x0200316A RID: 12650
			public class ASSIGNEDPUBLIC
			{
				// Token: 0x0400D4EF RID: 54511
				public static LocString NAME = "Assigned to: Public";

				// Token: 0x0400D4F0 RID: 54512
				public static LocString TOOLTIP = "Any Duplicant can use this amenity";
			}

			// Token: 0x0200316B RID: 12651
			public class ASSIGNEDTOROOM
			{
				// Token: 0x0400D4F1 RID: 54513
				public static LocString NAME = "Assigned to: {0}";

				// Token: 0x0400D4F2 RID: 54514
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Any Duplicant assigned to this ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" can use this amenity"
				});
			}

			// Token: 0x0200316C RID: 12652
			public class AWAITINGSEEDDELIVERY
			{
				// Token: 0x0400D4F3 RID: 54515
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400D4F4 RID: 54516
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;
			}

			// Token: 0x0200316D RID: 12653
			public class AWAITINGBAITDELIVERY
			{
				// Token: 0x0400D4F5 RID: 54517
				public static LocString NAME = "Awaiting Bait";

				// Token: 0x0400D4F6 RID: 54518
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Bait" + UI.PST_KEYWORD;
			}

			// Token: 0x0200316E RID: 12654
			public class CLINICOUTSIDEHOSPITAL
			{
				// Token: 0x0400D4F7 RID: 54519
				public static LocString NAME = "Medical building outside Hospital";

				// Token: 0x0400D4F8 RID: 54520
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rebuild this medical equipment in a ",
					UI.PRE_KEYWORD,
					"Hospital",
					UI.PST_KEYWORD,
					" to more effectively quarantine sick Duplicants"
				});
			}

			// Token: 0x0200316F RID: 12655
			public class BOTTLE_EMPTIER
			{
				// Token: 0x02003CC8 RID: 15560
				public static class ALLOWED
				{
					// Token: 0x0400F116 RID: 61718
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400F117 RID: 61719
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a bottling station to bring to this location"
					});
				}

				// Token: 0x02003CC9 RID: 15561
				public static class DENIED
				{
					// Token: 0x0400F118 RID: 61720
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400F119 RID: 61721
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a bottling station to bring to this location"
					});
				}
			}

			// Token: 0x02003170 RID: 12656
			public class CANISTER_EMPTIER
			{
				// Token: 0x02003CCA RID: 15562
				public static class ALLOWED
				{
					// Token: 0x0400F11A RID: 61722
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400F11B RID: 61723
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a canister filling station to bring to this location"
					});
				}

				// Token: 0x02003CCB RID: 15563
				public static class DENIED
				{
					// Token: 0x0400F11C RID: 61724
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400F11D RID: 61725
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a canister filling station to bring to this location"
					});
				}
			}

			// Token: 0x02003171 RID: 12657
			public class BROKEN
			{
				// Token: 0x0400D4F9 RID: 54521
				public static LocString NAME = "Broken";

				// Token: 0x0400D4FA RID: 54522
				public static LocString TOOLTIP = "This building received damage from <b>{DamageInfo}</b>\n\nIt will not function until it receives repairs";
			}

			// Token: 0x02003172 RID: 12658
			public class CHANGESTORAGETILETARGET
			{
				// Token: 0x0400D4FB RID: 54523
				public static LocString NAME = "Set Storage: {TargetName}";

				// Token: 0x0400D4FC RID: 54524
				public static LocString TOOLTIP = "Waiting for a Duplicant to reassign this storage to {TargetName}";

				// Token: 0x0400D4FD RID: 54525
				public static LocString EMPTY = "Empty";
			}

			// Token: 0x02003173 RID: 12659
			public class CHANGEDOORCONTROLSTATE
			{
				// Token: 0x0400D4FE RID: 54526
				public static LocString NAME = "Pending Door State Change: {ControlState}";

				// Token: 0x0400D4FF RID: 54527
				public static LocString TOOLTIP = "Waiting for a Duplicant to change control state";
			}

			// Token: 0x02003174 RID: 12660
			public class DISPENSEREQUESTED
			{
				// Token: 0x0400D500 RID: 54528
				public static LocString NAME = "Dispense Requested";

				// Token: 0x0400D501 RID: 54529
				public static LocString TOOLTIP = "Waiting for a Duplicant to dispense the item";
			}

			// Token: 0x02003175 RID: 12661
			public class SUIT_LOCKER
			{
				// Token: 0x02003CCC RID: 15564
				public class NEED_CONFIGURATION
				{
					// Token: 0x0400F11E RID: 61726
					public static LocString NAME = "Current Status: Needs Configuration";

					// Token: 0x0400F11F RID: 61727
					public static LocString TOOLTIP = "Set this dock to store a suit or leave it empty";
				}

				// Token: 0x02003CCD RID: 15565
				public class READY
				{
					// Token: 0x0400F120 RID: 61728
					public static LocString NAME = "Current Status: Empty";

					// Token: 0x0400F121 RID: 61729
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock is ready to receive a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						", either by manual delivery or from a Duplicant returning the suit they're wearing"
					});
				}

				// Token: 0x02003CCE RID: 15566
				public class SUIT_REQUESTED
				{
					// Token: 0x0400F122 RID: 61730
					public static LocString NAME = "Current Status: Awaiting Delivery";

					// Token: 0x0400F123 RID: 61731
					public static LocString TOOLTIP = "Waiting for a Duplicant to deliver a " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD;
				}

				// Token: 0x02003CCF RID: 15567
				public class CHARGING
				{
					// Token: 0x0400F124 RID: 61732
					public static LocString NAME = "Current Status: Charging Suit";

					// Token: 0x0400F125 RID: 61733
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						" is docked and refueling"
					});
				}

				// Token: 0x02003CD0 RID: 15568
				public class NO_OXYGEN
				{
					// Token: 0x0400F126 RID: 61734
					public static LocString NAME = "Current Status: No Oxygen";

					// Token: 0x0400F127 RID: 61735
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.OXYGEN.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x02003CD1 RID: 15569
				public class NO_FUEL
				{
					// Token: 0x0400F128 RID: 61736
					public static LocString NAME = "Current Status: No Fuel";

					// Token: 0x0400F129 RID: 61737
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.PETROLEUM.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x02003CD2 RID: 15570
				public class NO_COOLANT
				{
					// Token: 0x0400F12A RID: 61738
					public static LocString NAME = "Current Status: No Coolant";

					// Token: 0x0400F12B RID: 61739
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.WATER.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x02003CD3 RID: 15571
				public class NOT_OPERATIONAL
				{
					// Token: 0x0400F12C RID: 61740
					public static LocString NAME = "Current Status: Offline";

					// Token: 0x0400F12D RID: 61741
					public static LocString TOOLTIP = "This dock requires " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
				}

				// Token: 0x02003CD4 RID: 15572
				public class FULLY_CHARGED
				{
					// Token: 0x0400F12E RID: 61742
					public static LocString NAME = "Current Status: Full Fueled";

					// Token: 0x0400F12F RID: 61743
					public static LocString TOOLTIP = "This suit is fully refueled and ready for use";
				}
			}

			// Token: 0x02003176 RID: 12662
			public class SUITMARKERTRAVERSALONLYWHENROOMAVAILABLE
			{
				// Token: 0x0400D502 RID: 54530
				public static LocString NAME = "Clearance: Vacancy Only";

				// Token: 0x0400D503 RID: 54531
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass only if there is room in a ",
					UI.PRE_KEYWORD,
					"Dock",
					UI.PST_KEYWORD,
					" to store their ",
					UI.PRE_KEYWORD,
					"Suit",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003177 RID: 12663
			public class SUITMARKERTRAVERSALANYTIME
			{
				// Token: 0x0400D504 RID: 54532
				public static LocString NAME = "Clearance: Always Permitted";

				// Token: 0x0400D505 RID: 54533
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass even if there is no room to store their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					"\n\nWhen all available docks are full, Duplicants will unequip their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					" and drop them on the floor"
				});
			}

			// Token: 0x02003178 RID: 12664
			public class SUIT_LOCKER_NEEDS_CONFIGURATION
			{
				// Token: 0x0400D506 RID: 54534
				public static LocString NAME = "Not Configured";

				// Token: 0x0400D507 RID: 54535
				public static LocString TOOLTIP = "Dock settings not configured";
			}

			// Token: 0x02003179 RID: 12665
			public class CURRENTDOORCONTROLSTATE
			{
				// Token: 0x0400D508 RID: 54536
				public static LocString NAME = "Current State: {ControlState}";

				// Token: 0x0400D509 RID: 54537
				public static LocString TOOLTIP = "Current State: {ControlState}\n\nOpen: Door will remain open\nAuto: Door will open and close as needed\nLocked: Nothing may pass through";

				// Token: 0x0400D50A RID: 54538
				public static LocString OPENED = "Opened";

				// Token: 0x0400D50B RID: 54539
				public static LocString AUTO = "Auto";

				// Token: 0x0400D50C RID: 54540
				public static LocString LOCKED = "Locked";
			}

			// Token: 0x0200317A RID: 12666
			public class CONDUITBLOCKED
			{
				// Token: 0x0400D50D RID: 54541
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400D50E RID: 54542
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200317B RID: 12667
			public class OUTPUTTILEBLOCKED
			{
				// Token: 0x0400D50F RID: 54543
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400D510 RID: 54544
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200317C RID: 12668
			public class CONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400D511 RID: 54545
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400D512 RID: 54546
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200317D RID: 12669
			public class SOLIDCONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400D513 RID: 54547
				public static LocString NAME = "Conveyor Rail Blocked";

				// Token: 0x0400D514 RID: 54548
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200317E RID: 12670
			public class OUTPUTPIPEFULL
			{
				// Token: 0x0400D515 RID: 54549
				public static LocString NAME = "Output Pipe Full";

				// Token: 0x0400D516 RID: 54550
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Unable to flush contents, output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200317F RID: 12671
			public class CONSTRUCTIONUNREACHABLE
			{
				// Token: 0x0400D517 RID: 54551
				public static LocString NAME = "Unreachable Build";

				// Token: 0x0400D518 RID: 54552
				public static LocString TOOLTIP = "Duplicants cannot reach this construction site";
			}

			// Token: 0x02003180 RID: 12672
			public class MOPUNREACHABLE
			{
				// Token: 0x0400D519 RID: 54553
				public static LocString NAME = "Unreachable Mop";

				// Token: 0x0400D51A RID: 54554
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x02003181 RID: 12673
			public class DEADREACTORCOOLINGOFF
			{
				// Token: 0x0400D51B RID: 54555
				public static LocString NAME = "Cooling ({CyclesRemaining} cycles remaining)";

				// Token: 0x0400D51C RID: 54556
				public static LocString TOOLTIP = "The radiation coming from this reactor is diminishing";
			}

			// Token: 0x02003182 RID: 12674
			public class DIGUNREACHABLE
			{
				// Token: 0x0400D51D RID: 54557
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400D51E RID: 54558
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x02003183 RID: 12675
			public class STORAGEUNREACHABLE
			{
				// Token: 0x0400D51F RID: 54559
				public static LocString NAME = "Unreachable Storage";

				// Token: 0x0400D520 RID: 54560
				public static LocString TOOLTIP = "Duplicants cannot reach this storage unit";
			}

			// Token: 0x02003184 RID: 12676
			public class PASSENGERMODULEUNREACHABLE
			{
				// Token: 0x0400D521 RID: 54561
				public static LocString NAME = "Unreachable Module";

				// Token: 0x0400D522 RID: 54562
				public static LocString TOOLTIP = "Duplicants cannot reach this rocket module";
			}

			// Token: 0x02003185 RID: 12677
			public class POWERBANKCHARGERINPROGRESS
			{
				// Token: 0x0400D523 RID: 54563
				public static LocString NAME = "Recharging Power Bank: {0}";

				// Token: 0x0400D524 RID: 54564
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is currently charging a ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" at {0}\n\nThe ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" will be dropped once charging is complete"
				});
			}

			// Token: 0x02003186 RID: 12678
			public class CONSTRUCTABLEDIGUNREACHABLE
			{
				// Token: 0x0400D525 RID: 54565
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400D526 RID: 54566
				public static LocString TOOLTIP = "This construction site contains cells that cannot be dug out";
			}

			// Token: 0x02003187 RID: 12679
			public class EMPTYPUMPINGSTATION
			{
				// Token: 0x0400D527 RID: 54567
				public static LocString NAME = "Empty";

				// Token: 0x0400D528 RID: 54568
				public static LocString TOOLTIP = "This pumping station cannot access any " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;
			}

			// Token: 0x02003188 RID: 12680
			public class ENTOMBED
			{
				// Token: 0x0400D529 RID: 54569
				public static LocString NAME = "Entombed";

				// Token: 0x0400D52A RID: 54570
				public static LocString TOOLTIP = "Must be dug out by a Duplicant";

				// Token: 0x0400D52B RID: 54571
				public static LocString NOTIFICATION_NAME = "Building entombment";

				// Token: 0x0400D52C RID: 54572
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are entombed and need to be dug out:";
			}

			// Token: 0x02003189 RID: 12681
			public class ELECTROBANKJOULESAVAILABLE
			{
				// Token: 0x0400D52D RID: 54573
				public static LocString NAME = "Power Remaining: {JoulesAvailable} / {JoulesCapacity}";

				// Token: 0x0400D52E RID: 54574
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"<b>{JoulesAvailable}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" available for use\n\nMaximum capacity: {JoulesCapacity}"
				});
			}

			// Token: 0x0200318A RID: 12682
			public class FABRICATORACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400D52F RID: 54575
				public static LocString NAME = "Fabricator accepts mutant seeds";

				// Token: 0x0400D530 RID: 54576
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fabricator is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x0200318B RID: 12683
			public class FISHFEEDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400D531 RID: 54577
				public static LocString NAME = "Fish Feeder accepts mutant seeds";

				// Token: 0x0400D532 RID: 54578
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fish feeder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as fish food"
				});
			}

			// Token: 0x0200318C RID: 12684
			public class INVALIDPORTOVERLAP
			{
				// Token: 0x0400D533 RID: 54579
				public static LocString NAME = "Invalid Port Overlap";

				// Token: 0x0400D534 RID: 54580
				public static LocString TOOLTIP = "Ports on this building overlap those on another building\n\nThis building must be rebuilt in a valid location";

				// Token: 0x0400D535 RID: 54581
				public static LocString NOTIFICATION_NAME = "Building has overlapping ports";

				// Token: 0x0400D536 RID: 54582
				public static LocString NOTIFICATION_TOOLTIP = "These buildings must be rebuilt with non-overlapping ports:";
			}

			// Token: 0x0200318D RID: 12685
			public class GENESHUFFLECOMPLETED
			{
				// Token: 0x0400D537 RID: 54583
				public static LocString NAME = "Vacillation Complete";

				// Token: 0x0400D538 RID: 54584
				public static LocString TOOLTIP = "The Duplicant has completed the neural vacillation process and is ready to be released";
			}

			// Token: 0x0200318E RID: 12686
			public class OVERHEATED
			{
				// Token: 0x0400D539 RID: 54585
				public static LocString NAME = "Damage: Overheating";

				// Token: 0x0400D53A RID: 54586
				public static LocString TOOLTIP = "This building is taking damage and will break down if not cooled";
			}

			// Token: 0x0200318F RID: 12687
			public class OVERLOADED
			{
				// Token: 0x0400D53B RID: 54587
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400D53C RID: 54588
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Wire",
					UI.PST_KEYWORD,
					" is taking damage because there are too many buildings pulling ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from this circuit\n\nSplit this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" circuit into multiple circuits, or use higher quality ",
					UI.PRE_KEYWORD,
					"Wires",
					UI.PST_KEYWORD,
					" to prevent overloading"
				});
			}

			// Token: 0x02003190 RID: 12688
			public class LOGICOVERLOADED
			{
				// Token: 0x0400D53D RID: 54589
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400D53E RID: 54590
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Logic Wire",
					UI.PST_KEYWORD,
					" is taking damage\n\nLimit the output to one Bit, or replace it with ",
					UI.PRE_KEYWORD,
					"Logic Ribbon",
					UI.PST_KEYWORD,
					" to prevent further damage"
				});
			}

			// Token: 0x02003191 RID: 12689
			public class OPERATINGENERGY
			{
				// Token: 0x0400D53F RID: 54591
				public static LocString NAME = "Heat Production: {0}/s";

				// Token: 0x0400D540 RID: 54592
				public static LocString TOOLTIP = "This building is producing <b>{0}</b> per second\n\nSources:\n{1}";

				// Token: 0x0400D541 RID: 54593
				public static LocString LINEITEM = "    • {0}: {1}\n";

				// Token: 0x0400D542 RID: 54594
				public static LocString OPERATING = "Normal operation";

				// Token: 0x0400D543 RID: 54595
				public static LocString EXHAUSTING = "Excess produced";

				// Token: 0x0400D544 RID: 54596
				public static LocString PIPECONTENTS_TRANSFER = "Transferred from pipes";

				// Token: 0x0400D545 RID: 54597
				public static LocString FOOD_TRANSFER = "Internal Cooling";
			}

			// Token: 0x02003192 RID: 12690
			public class FLOODED
			{
				// Token: 0x0400D546 RID: 54598
				public static LocString NAME = "Building Flooded";

				// Token: 0x0400D547 RID: 54599
				public static LocString TOOLTIP = "Building cannot function at current saturation";

				// Token: 0x0400D548 RID: 54600
				public static LocString NOTIFICATION_NAME = "Flooding";

				// Token: 0x0400D549 RID: 54601
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are flooded:";
			}

			// Token: 0x02003193 RID: 12691
			public class NOTSUBMERGED
			{
				// Token: 0x0400D54A RID: 54602
				public static LocString NAME = "Building Not Submerged";

				// Token: 0x0400D54B RID: 54603
				public static LocString TOOLTIP = "Building cannot function unless submerged in liquid";
			}

			// Token: 0x02003194 RID: 12692
			public class GASVENTOBSTRUCTED
			{
				// Token: 0x0400D54C RID: 54604
				public static LocString NAME = "Gas Vent Obstructed";

				// Token: 0x0400D54D RID: 54605
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x02003195 RID: 12693
			public class GASVENTOVERPRESSURE
			{
				// Token: 0x0400D54E RID: 54606
				public static LocString NAME = "Gas Vent Overpressure";

				// Token: 0x0400D54F RID: 54607
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x02003196 RID: 12694
			public class DIRECTION_CONTROL
			{
				// Token: 0x0400D550 RID: 54608
				public static LocString NAME = "Use Direction: {Direction}";

				// Token: 0x0400D551 RID: 54609
				public static LocString TOOLTIP = "Duplicants will only use this building when walking by it\n\nCurrently allowed direction: <b>{Direction}</b>";

				// Token: 0x02003CD5 RID: 15573
				public static class DIRECTIONS
				{
					// Token: 0x0400F130 RID: 61744
					public static LocString LEFT = "Left";

					// Token: 0x0400F131 RID: 61745
					public static LocString RIGHT = "Right";

					// Token: 0x0400F132 RID: 61746
					public static LocString BOTH = "Both";
				}
			}

			// Token: 0x02003197 RID: 12695
			public class WATTSONGAMEOVER
			{
				// Token: 0x0400D552 RID: 54610
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400D553 RID: 54611
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x02003198 RID: 12696
			public class INVALIDBUILDINGLOCATION
			{
				// Token: 0x0400D554 RID: 54612
				public static LocString NAME = "Invalid Building Location";

				// Token: 0x0400D555 RID: 54613
				public static LocString TOOLTIP = "Cannot construct a building in this location";
			}

			// Token: 0x02003199 RID: 12697
			public class LIQUIDVENTOBSTRUCTED
			{
				// Token: 0x0400D556 RID: 54614
				public static LocString NAME = "Liquid Vent Obstructed";

				// Token: 0x0400D557 RID: 54615
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x0200319A RID: 12698
			public class LIQUIDVENTOVERPRESSURE
			{
				// Token: 0x0400D558 RID: 54616
				public static LocString NAME = "Liquid Vent Overpressure";

				// Token: 0x0400D559 RID: 54617
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x0200319B RID: 12699
			public class MANUALLYCONTROLLED
			{
				// Token: 0x0400D55A RID: 54618
				public static LocString NAME = "Manually Controlled";

				// Token: 0x0400D55B RID: 54619
				public static LocString TOOLTIP = "This Duplicant is under my control";
			}

			// Token: 0x0200319C RID: 12700
			public class LIMITVALVELIMITREACHED
			{
				// Token: 0x0400D55C RID: 54620
				public static LocString NAME = "Limit Reached";

				// Token: 0x0400D55D RID: 54621
				public static LocString TOOLTIP = "No more Mass can be transferred";
			}

			// Token: 0x0200319D RID: 12701
			public class LIMITVALVELIMITNOTREACHED
			{
				// Token: 0x0400D55E RID: 54622
				public static LocString NAME = "Amount remaining: {0}";

				// Token: 0x0400D55F RID: 54623
				public static LocString TOOLTIP = "This building will stop transferring Mass when the amount remaining reaches 0";
			}

			// Token: 0x0200319E RID: 12702
			public class MATERIALSUNAVAILABLE
			{
				// Token: 0x0400D560 RID: 54624
				public static LocString NAME = "Insufficient Resources\n{ItemsRemaining}";

				// Token: 0x0400D561 RID: 54625
				public static LocString TOOLTIP = "Crucial materials for this building are beyond reach or unavailable";

				// Token: 0x0400D562 RID: 54626
				public static LocString NOTIFICATION_NAME = "Building lacks resources";

				// Token: 0x0400D563 RID: 54627
				public static LocString NOTIFICATION_TOOLTIP = "Crucial materials are unavailable or beyond reach for these buildings:";

				// Token: 0x0400D564 RID: 54628
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400D565 RID: 54629
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x0200319F RID: 12703
			public class MATERIALSUNAVAILABLEFORREFILL
			{
				// Token: 0x0400D566 RID: 54630
				public static LocString NAME = "Resources Low\n{ItemsRemaining}";

				// Token: 0x0400D567 RID: 54631
				public static LocString TOOLTIP = "This building will soon require materials that are unavailable";

				// Token: 0x0400D568 RID: 54632
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x020031A0 RID: 12704
			public class MELTINGDOWN
			{
				// Token: 0x0400D569 RID: 54633
				public static LocString NAME = "Breaking Down";

				// Token: 0x0400D56A RID: 54634
				public static LocString TOOLTIP = "This building is collapsing";

				// Token: 0x0400D56B RID: 54635
				public static LocString NOTIFICATION_NAME = "Building breakdown";

				// Token: 0x0400D56C RID: 54636
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are collapsing:";
			}

			// Token: 0x020031A1 RID: 12705
			public class MISSINGFOUNDATION
			{
				// Token: 0x0400D56D RID: 54637
				public static LocString NAME = "Missing Tile";

				// Token: 0x0400D56E RID: 54638
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Build ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" beneath this building to regain function\n\nTile can be found in the ",
					UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
					" of the Build Menu"
				});
			}

			// Token: 0x020031A2 RID: 12706
			public class MISSINGFOUNDATIONBACKWALL
			{
				// Token: 0x0400D56F RID: 54639
				public static LocString NAME = "Missing Back Wall";

				// Token: 0x0400D570 RID: 54640
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Build a ",
					UI.PRE_KEYWORD,
					"Drywall",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Tempshift Plate",
					UI.PST_KEYWORD,
					" behind this building to regain function\n\nThey can be found in the ",
					UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
					" and ",
					UI.FormatAsBuildMenuTab("Utilities Tab", global::Action.Plan11),
					" of the Build Menu"
				});
			}

			// Token: 0x020031A3 RID: 12707
			public class NEUTRONIUMUNMINABLE
			{
				// Token: 0x0400D571 RID: 54641
				public static LocString NAME = "Cannot Mine";

				// Token: 0x0400D572 RID: 54642
				public static LocString TOOLTIP = "This resource cannot be mined by Duplicant tools";
			}

			// Token: 0x020031A4 RID: 12708
			public class NEEDGASIN
			{
				// Token: 0x0400D573 RID: 54643
				public static LocString NAME = "No Gas Intake\n{GasRequired}";

				// Token: 0x0400D574 RID: 54644
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400D575 RID: 54645
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x020031A5 RID: 12709
			public class NEEDGASOUT
			{
				// Token: 0x0400D576 RID: 54646
				public static LocString NAME = "No Gas Output";

				// Token: 0x0400D577 RID: 54647
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x020031A6 RID: 12710
			public class NEEDLIQUIDIN
			{
				// Token: 0x0400D578 RID: 54648
				public static LocString NAME = "No Liquid Intake\n{LiquidRequired}";

				// Token: 0x0400D579 RID: 54649
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400D57A RID: 54650
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x020031A7 RID: 12711
			public class NEEDLIQUIDOUT
			{
				// Token: 0x0400D57B RID: 54651
				public static LocString NAME = "No Liquid Output";

				// Token: 0x0400D57C RID: 54652
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x020031A8 RID: 12712
			public class LIQUIDPIPEEMPTY
			{
				// Token: 0x0400D57D RID: 54653
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400D57E RID: 54654
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x020031A9 RID: 12713
			public class LIQUIDPIPEOBSTRUCTED
			{
				// Token: 0x0400D57F RID: 54655
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400D580 RID: 54656
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x020031AA RID: 12714
			public class GASPIPEEMPTY
			{
				// Token: 0x0400D581 RID: 54657
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400D582 RID: 54658
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x020031AB RID: 12715
			public class GASPIPEOBSTRUCTED
			{
				// Token: 0x0400D583 RID: 54659
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400D584 RID: 54660
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x020031AC RID: 12716
			public class NEEDSOLIDIN
			{
				// Token: 0x0400D585 RID: 54661
				public static LocString NAME = "No Conveyor Loader";

				// Token: 0x0400D586 RID: 54662
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be fed onto this Conveyor system for transport\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Loader",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031AD RID: 12717
			public class NEEDSOLIDOUT
			{
				// Token: 0x0400D587 RID: 54663
				public static LocString NAME = "No Conveyor Receptacle";

				// Token: 0x0400D588 RID: 54664
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be offloaded from this Conveyor system and will backup the rails\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031AE RID: 12718
			public class SOLIDPIPEOBSTRUCTED
			{
				// Token: 0x0400D589 RID: 54665
				public static LocString NAME = "Conveyor Rail Backup";

				// Token: 0x0400D58A RID: 54666
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" cannot carry anymore material\n\nRemove material from the ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD,
					" to free space for more objects"
				});
			}

			// Token: 0x020031AF RID: 12719
			public class NEEDPLANT
			{
				// Token: 0x0400D58B RID: 54667
				public static LocString NAME = "No Seeds";

				// Token: 0x0400D58C RID: 54668
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x020031B0 RID: 12720
			public class NEEDSEED
			{
				// Token: 0x0400D58D RID: 54669
				public static LocString NAME = "No Seed Selected";

				// Token: 0x0400D58E RID: 54670
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x020031B1 RID: 12721
			public class NEEDPOWER
			{
				// Token: 0x0400D58F RID: 54671
				public static LocString NAME = "No Power";

				// Token: 0x0400D590 RID: 54672
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"All connected ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" sources have lost charge"
				});
			}

			// Token: 0x020031B2 RID: 12722
			public class NOTENOUGHPOWER
			{
				// Token: 0x0400D591 RID: 54673
				public static LocString NAME = "Insufficient Power";

				// Token: 0x0400D592 RID: 54674
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building does not have enough stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" to run"
				});
			}

			// Token: 0x020031B3 RID: 12723
			public class POWERLOOPDETECTED
			{
				// Token: 0x0400D593 RID: 54675
				public static LocString NAME = "Power Loop Detected";

				// Token: 0x0400D594 RID: 54676
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Transformer's",
					UI.PST_KEYWORD,
					" ",
					UI.PRE_KEYWORD,
					"Power Output",
					UI.PST_KEYWORD,
					" has been connected back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031B4 RID: 12724
			public class NEEDRESOURCE
			{
				// Token: 0x0400D595 RID: 54677
				public static LocString NAME = "Resource Required";

				// Token: 0x0400D596 RID: 54678
				public static LocString TOOLTIP = "This building is missing required materials";
			}

			// Token: 0x020031B5 RID: 12725
			public class NEWDUPLICANTSAVAILABLE
			{
				// Token: 0x0400D597 RID: 54679
				public static LocString NAME = "Printables Available";

				// Token: 0x0400D598 RID: 54680
				public static LocString TOOLTIP = "I am ready to print a new colony member or care package";

				// Token: 0x0400D599 RID: 54681
				public static LocString NOTIFICATION_NAME = "New Printables are available";

				// Token: 0x0400D59A RID: 54682
				public static LocString NOTIFICATION_TOOLTIP = "The Printing Pod " + UI.FormatAsHotKey(global::Action.Plan1) + " is ready to print a new Duplicant or care package.\nI'll need to select a blueprint:";
			}

			// Token: 0x020031B6 RID: 12726
			public class NOAPPLICABLERESEARCHSELECTED
			{
				// Token: 0x0400D59B RID: 54683
				public static LocString NAME = "Inapplicable Research";

				// Token: 0x0400D59C RID: 54684
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the current ",
					UI.FormatAsLink("Research Focus", "TECH")
				});

				// Token: 0x0400D59D RID: 54685
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Research Center", "ADVANCEDRESEARCHCENTER") + " idle";

				// Token: 0x0400D59E RID: 54686
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These buildings cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the selected ",
					UI.FormatAsLink("Research Focus", "TECH"),
					":"
				});
			}

			// Token: 0x020031B7 RID: 12727
			public class NOAPPLICABLEANALYSISSELECTED
			{
				// Token: 0x0400D59F RID: 54687
				public static LocString NAME = "No Analysis Focus Selected";

				// Token: 0x0400D5A0 RID: 54688
				public static LocString TOOLTIP = "Select an unknown destination from the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to begin analysis";

				// Token: 0x0400D5A1 RID: 54689
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Telescope", "TELESCOPE") + " idle";

				// Token: 0x0400D5A2 RID: 54690
				public static LocString NOTIFICATION_TOOLTIP = "These buildings require an analysis focus:";
			}

			// Token: 0x020031B8 RID: 12728
			public class NOAVAILABLESEED
			{
				// Token: 0x0400D5A3 RID: 54691
				public static LocString NAME = "No Seed Available";

				// Token: 0x0400D5A4 RID: 54692
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Seed",
					UI.PST_KEYWORD,
					" is not available"
				});
			}

			// Token: 0x020031B9 RID: 12729
			public class NOSTORAGEFILTERSET
			{
				// Token: 0x0400D5A5 RID: 54693
				public static LocString NAME = "Filters Not Designated";

				// Token: 0x0400D5A6 RID: 54694
				public static LocString TOOLTIP = "No resources types are marked for storage in this building";
			}

			// Token: 0x020031BA RID: 12730
			public class NOSUITMARKER
			{
				// Token: 0x0400D5A7 RID: 54695
				public static LocString NAME = "No Checkpoint";

				// Token: 0x0400D5A8 RID: 54696
				public static LocString TOOLTIP = "Docks must be placed beside a " + BUILDINGS.PREFABS.CHECKPOINT.NAME + ", opposite the side the checkpoint faces";
			}

			// Token: 0x020031BB RID: 12731
			public class SUITMARKERWRONGSIDE
			{
				// Token: 0x0400D5A9 RID: 54697
				public static LocString NAME = "Invalid Checkpoint";

				// Token: 0x0400D5AA RID: 54698
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been built on the wrong side of a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					"\n\nDocks must be placed beside a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					", opposite the side the checkpoint faces"
				});
			}

			// Token: 0x020031BC RID: 12732
			public class NOFILTERELEMENTSELECTED
			{
				// Token: 0x0400D5AB RID: 54699
				public static LocString NAME = "No Filter Selected";

				// Token: 0x0400D5AC RID: 54700
				public static LocString TOOLTIP = "Select a resource to filter";
			}

			// Token: 0x020031BD RID: 12733
			public class NOLUREELEMENTSELECTED
			{
				// Token: 0x0400D5AD RID: 54701
				public static LocString NAME = "No Bait Selected";

				// Token: 0x0400D5AE RID: 54702
				public static LocString TOOLTIP = "Select a resource to use as bait";
			}

			// Token: 0x020031BE RID: 12734
			public class NOFISHABLEWATERBELOW
			{
				// Token: 0x0400D5AF RID: 54703
				public static LocString NAME = "No Fishable Water";

				// Token: 0x0400D5B0 RID: 54704
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There are no edible ",
					UI.PRE_KEYWORD,
					"Fish",
					UI.PST_KEYWORD,
					" beneath this structure"
				});
			}

			// Token: 0x020031BF RID: 12735
			public class NOPOWERCONSUMERS
			{
				// Token: 0x0400D5B1 RID: 54705
				public static LocString NAME = "No Power Consumers";

				// Token: 0x0400D5B2 RID: 54706
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"No buildings are connected to this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source"
				});
			}

			// Token: 0x020031C0 RID: 12736
			public class NOWIRECONNECTED
			{
				// Token: 0x0400D5B3 RID: 54707
				public static LocString NAME = "No Power Wire Connected";

				// Token: 0x0400D5B4 RID: 54708
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x020031C1 RID: 12737
			public class PENDINGDECONSTRUCTION
			{
				// Token: 0x0400D5B5 RID: 54709
				public static LocString NAME = "Deconstruction Errand";

				// Token: 0x0400D5B6 RID: 54710
				public static LocString TOOLTIP = "Building will be deconstructed once a Duplicant is available";
			}

			// Token: 0x020031C2 RID: 12738
			public class PENDINGDEMOLITION
			{
				// Token: 0x0400D5B7 RID: 54711
				public static LocString NAME = "Demolition Errand";

				// Token: 0x0400D5B8 RID: 54712
				public static LocString TOOLTIP = "Object will be permanently demolished once a Duplicant is available";
			}

			// Token: 0x020031C3 RID: 12739
			public class PENDINGFISH
			{
				// Token: 0x0400D5B9 RID: 54713
				public static LocString NAME = "Fishing Errand";

				// Token: 0x0400D5BA RID: 54714
				public static LocString TOOLTIP = "Spot will be fished once a Duplicant is available";
			}

			// Token: 0x020031C4 RID: 12740
			public class PENDINGHARVEST
			{
				// Token: 0x0400D5BB RID: 54715
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400D5BC RID: 54716
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x020031C5 RID: 12741
			public class PENDINGUPROOT
			{
				// Token: 0x0400D5BD RID: 54717
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400D5BE RID: 54718
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x020031C6 RID: 12742
			public class PENDINGREPAIR
			{
				// Token: 0x0400D5BF RID: 54719
				public static LocString NAME = "Repair Errand";

				// Token: 0x0400D5C0 RID: 54720
				public static LocString TOOLTIP = "Building will be repaired once a Duplicant is available\nReceived damage from {DamageInfo}";
			}

			// Token: 0x020031C7 RID: 12743
			public class PENDINGSWITCHTOGGLE
			{
				// Token: 0x0400D5C1 RID: 54721
				public static LocString NAME = "Settings Errand";

				// Token: 0x0400D5C2 RID: 54722
				public static LocString TOOLTIP = "Settings will be changed once a Duplicant is available";
			}

			// Token: 0x020031C8 RID: 12744
			public class PENDINGWORK
			{
				// Token: 0x0400D5C3 RID: 54723
				public static LocString NAME = "Work Errand";

				// Token: 0x0400D5C4 RID: 54724
				public static LocString TOOLTIP = "Building will be operated once a Duplicant is available";
			}

			// Token: 0x020031C9 RID: 12745
			public class POWERBUTTONOFF
			{
				// Token: 0x0400D5C5 RID: 54725
				public static LocString NAME = "Function Suspended";

				// Token: 0x0400D5C6 RID: 54726
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been toggled off\nPress ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume its use"
				});
			}

			// Token: 0x020031CA RID: 12746
			public class PUMPINGSTATION
			{
				// Token: 0x0400D5C7 RID: 54727
				public static LocString NAME = "Liquid Available: {Liquids}";

				// Token: 0x0400D5C8 RID: 54728
				public static LocString TOOLTIP = "This pumping station has access to: {Liquids}";
			}

			// Token: 0x020031CB RID: 12747
			public class PRESSUREOK
			{
				// Token: 0x0400D5C9 RID: 54729
				public static LocString NAME = "Max Gas Pressure";

				// Token: 0x0400D5CA RID: 54730
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ambient ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is preventing this building from emitting gas\n\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x020031CC RID: 12748
			public class UNDERPRESSURE
			{
				// Token: 0x0400D5CB RID: 54731
				public static LocString NAME = "Low Air Pressure";

				// Token: 0x0400D5CC RID: 54732
				public static LocString TOOLTIP = "A minimum atmospheric pressure of <b>{TargetPressure}</b> is needed for this building to operate";
			}

			// Token: 0x020031CD RID: 12749
			public class STORAGELOCKER
			{
				// Token: 0x0400D5CD RID: 54733
				public static LocString NAME = "Storing: {Stored} / {Capacity} {Units}";

				// Token: 0x0400D5CE RID: 54734
				public static LocString TOOLTIP = "This container is storing <b>{Stored}{Units}</b> of a maximum <b>{Capacity}{Units}</b>";
			}

			// Token: 0x020031CE RID: 12750
			public class CRITTERCAPACITY
			{
				// Token: 0x0400D5CF RID: 54735
				public static LocString NAME = "Storing: {Stored} / {Capacity} Critters";

				// Token: 0x0400D5D0 RID: 54736
				public static LocString TOOLTIP = "This container is storing <b>{Stored} {StoredUnits}</b> of a maximum <b>{Capacity} {CapacityUnits}</b>";

				// Token: 0x0400D5D1 RID: 54737
				public static LocString UNITS = "Critters";

				// Token: 0x0400D5D2 RID: 54738
				public static LocString UNIT = "Critter";
			}

			// Token: 0x020031CF RID: 12751
			public class SKILL_POINTS_AVAILABLE
			{
				// Token: 0x0400D5D3 RID: 54739
				public static LocString NAME = "Skill Points Available";

				// Token: 0x0400D5D4 RID: 54740
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant has ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" available"
				});
			}

			// Token: 0x020031D0 RID: 12752
			public class TANNINGLIGHTSUFFICIENT
			{
				// Token: 0x0400D5D5 RID: 54741
				public static LocString NAME = "Tanning Light Available";

				// Token: 0x0400D5D6 RID: 54742
				public static LocString TOOLTIP = "There is sufficient " + UI.FormatAsLink("Light", "LIGHT") + " here to create pleasing skin crisping";
			}

			// Token: 0x020031D1 RID: 12753
			public class TANNINGLIGHTINSUFFICIENT
			{
				// Token: 0x0400D5D7 RID: 54743
				public static LocString NAME = "Insufficient Tanning Light";

				// Token: 0x0400D5D8 RID: 54744
				public static LocString TOOLTIP = "The " + UI.FormatAsLink("Light", "LIGHT") + " here is not bright enough for that Sunny Day feeling";
			}

			// Token: 0x020031D2 RID: 12754
			public class UNASSIGNED
			{
				// Token: 0x0400D5D9 RID: 54745
				public static LocString NAME = "Unassigned";

				// Token: 0x0400D5DA RID: 54746
				public static LocString TOOLTIP = "Assign a Duplicant to use this amenity";
			}

			// Token: 0x020031D3 RID: 12755
			public class UNDERCONSTRUCTION
			{
				// Token: 0x0400D5DB RID: 54747
				public static LocString NAME = "Under Construction";

				// Token: 0x0400D5DC RID: 54748
				public static LocString TOOLTIP = "This building is currently being built";
			}

			// Token: 0x020031D4 RID: 12756
			public class UNDERCONSTRUCTIONNOWORKER
			{
				// Token: 0x0400D5DD RID: 54749
				public static LocString NAME = "Construction Errand";

				// Token: 0x0400D5DE RID: 54750
				public static LocString TOOLTIP = "Building will be constructed once a Duplicant is available";
			}

			// Token: 0x020031D5 RID: 12757
			public class WAITINGFORMATERIALS
			{
				// Token: 0x0400D5DF RID: 54751
				public static LocString NAME = "Awaiting Delivery\n{ItemsRemaining}";

				// Token: 0x0400D5E0 RID: 54752
				public static LocString TOOLTIP = "These materials will be delivered once a Duplicant is available";

				// Token: 0x0400D5E1 RID: 54753
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400D5E2 RID: 54754
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x020031D6 RID: 12758
			public class WAITINGFORRADIATION
			{
				// Token: 0x0400D5E3 RID: 54755
				public static LocString NAME = "Awaiting Radbolts";

				// Token: 0x0400D5E4 RID: 54756
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires Radbolts to function\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to view this building's radiation port"
				});
			}

			// Token: 0x020031D7 RID: 12759
			public class WAITINGFORREPAIRMATERIALS
			{
				// Token: 0x0400D5E5 RID: 54757
				public static LocString NAME = "Awaiting Repair Delivery\n{ItemsRemaining}\n";

				// Token: 0x0400D5E6 RID: 54758
				public static LocString TOOLTIP = "These materials must be delivered before this building can be repaired";

				// Token: 0x0400D5E7 RID: 54759
				public static LocString LINE_ITEM = string.Concat(new string[]
				{
					"• ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					": <b>{1}</b>"
				});
			}

			// Token: 0x020031D8 RID: 12760
			public class MISSINGGANTRY
			{
				// Token: 0x0400D5E8 RID: 54760
				public static LocString NAME = "Missing Gantry";

				// Token: 0x0400D5E9 RID: 54761
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.FormatAsLink("Gantry", "GANTRY"),
					" must be built below ",
					UI.FormatAsLink("Command Capsules", "COMMANDMODULE"),
					" and ",
					UI.FormatAsLink("Sight-Seeing Modules", "TOURISTMODULE"),
					" for Duplicant access"
				});
			}

			// Token: 0x020031D9 RID: 12761
			public class DISEMBARKINGDUPLICANT
			{
				// Token: 0x0400D5EA RID: 54762
				public static LocString NAME = "Waiting To Disembark";

				// Token: 0x0400D5EB RID: 54763
				public static LocString TOOLTIP = "The Duplicant inside this rocket can't come out until the " + UI.FormatAsLink("Gantry", "GANTRY") + " is extended";
			}

			// Token: 0x020031DA RID: 12762
			public class REACTORMELTDOWN
			{
				// Token: 0x0400D5EC RID: 54764
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400D5ED RID: 54765
				public static LocString TOOLTIP = "This reactor is spilling dangerous radioactive waste and cannot be stopped";
			}

			// Token: 0x020031DB RID: 12763
			public class ROCKETNAME
			{
				// Token: 0x0400D5EE RID: 54766
				public static LocString NAME = "Parent Rocket: {0}";

				// Token: 0x0400D5EF RID: 54767
				public static LocString TOOLTIP = "This module belongs to the rocket: " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020031DC RID: 12764
			public class HASGANTRY
			{
				// Token: 0x0400D5F0 RID: 54768
				public static LocString NAME = "Has Gantry";

				// Token: 0x0400D5F1 RID: 54769
				public static LocString TOOLTIP = "Duplicants may now enter this section of the rocket";
			}

			// Token: 0x020031DD RID: 12765
			public class NORMAL
			{
				// Token: 0x0400D5F2 RID: 54770
				public static LocString NAME = "Normal";

				// Token: 0x0400D5F3 RID: 54771
				public static LocString TOOLTIP = "Nothing out of the ordinary here";
			}

			// Token: 0x020031DE RID: 12766
			public class MANUALGENERATORCHARGINGUP
			{
				// Token: 0x0400D5F4 RID: 54772
				public static LocString NAME = "Charging Up";

				// Token: 0x0400D5F5 RID: 54773
				public static LocString TOOLTIP = "This power source is being charged";
			}

			// Token: 0x020031DF RID: 12767
			public class MANUALGENERATORRELEASINGENERGY
			{
				// Token: 0x0400D5F6 RID: 54774
				public static LocString NAME = "Powering";

				// Token: 0x0400D5F7 RID: 54775
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This generator is supplying energy to ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers"
				});
			}

			// Token: 0x020031E0 RID: 12768
			public class GENERATOROFFLINE
			{
				// Token: 0x0400D5F8 RID: 54776
				public static LocString NAME = "Generator Idle";

				// Token: 0x0400D5F9 RID: 54777
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source is idle"
				});
			}

			// Token: 0x020031E1 RID: 12769
			public class PIPE
			{
				// Token: 0x0400D5FA RID: 54778
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400D5FB RID: 54779
				public static LocString TOOLTIP = "This pipe is delivering {Contents}";
			}

			// Token: 0x020031E2 RID: 12770
			public class CONVEYOR
			{
				// Token: 0x0400D5FC RID: 54780
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400D5FD RID: 54781
				public static LocString TOOLTIP = "This conveyor is delivering {Contents}";
			}

			// Token: 0x020031E3 RID: 12771
			public class FABRICATORIDLE
			{
				// Token: 0x0400D5FE RID: 54782
				public static LocString NAME = "No Fabrications Queued";

				// Token: 0x0400D5FF RID: 54783
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x020031E4 RID: 12772
			public class FABRICATOREMPTY
			{
				// Token: 0x0400D600 RID: 54784
				public static LocString NAME = "Waiting For Materials";

				// Token: 0x0400D601 RID: 54785
				public static LocString TOOLTIP = "Fabrication will begin once materials have been delivered";
			}

			// Token: 0x020031E5 RID: 12773
			public class FABRICATORLACKSHEP
			{
				// Token: 0x0400D602 RID: 54786
				public static LocString NAME = "Waiting For Radbolts ({CurrentHEP}/{HEPRequired})";

				// Token: 0x0400D603 RID: 54787
				public static LocString TOOLTIP = "A queued recipe requires more Radbolts than are currently stored.\n\nCurrently stored: {CurrentHEP}\nRequired for recipe: {HEPRequired}";
			}

			// Token: 0x020031E6 RID: 12774
			public class TOILET
			{
				// Token: 0x0400D604 RID: 54788
				public static LocString NAME = "{FlushesRemaining} \"Visits\" Remaining";

				// Token: 0x0400D605 RID: 54789
				public static LocString TOOLTIP = "{FlushesRemaining} more Duplicants can use this amenity before it requires maintenance";
			}

			// Token: 0x020031E7 RID: 12775
			public class TOILETNEEDSEMPTYING
			{
				// Token: 0x0400D606 RID: 54790
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400D607 RID: 54791
				public static LocString TOOLTIP = "This amenity cannot be used while full\n\nEmptying it will produce " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND");
			}

			// Token: 0x020031E8 RID: 12776
			public class DESALINATORNEEDSEMPTYING
			{
				// Token: 0x0400D608 RID: 54792
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400D609 RID: 54793
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Salt", "SALT") + " to resume function";
			}

			// Token: 0x020031E9 RID: 12777
			public class MILKSEPARATORNEEDSEMPTYING
			{
				// Token: 0x0400D60A RID: 54794
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400D60B RID: 54795
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Brackwax", "MILKFAT") + " to resume function";
			}

			// Token: 0x020031EA RID: 12778
			public class HABITATNEEDSEMPTYING
			{
				// Token: 0x0400D60C RID: 54796
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400D60D RID: 54797
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT"),
					" needs to be emptied of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"\n\n",
					UI.FormatAsLink("Bottle Emptiers", "BOTTLEEMPTIER"),
					" can be used to transport and dispose of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" in designated areas"
				});
			}

			// Token: 0x020031EB RID: 12779
			public class UNUSABLE
			{
				// Token: 0x0400D60E RID: 54798
				public static LocString NAME = "Out of Order";

				// Token: 0x0400D60F RID: 54799
				public static LocString TOOLTIP = "This amenity requires maintenance";
			}

			// Token: 0x020031EC RID: 12780
			public class UNUSABLEGUNKED
			{
				// Token: 0x0400D610 RID: 54800
				public static LocString NAME = "Out of Order: Gunk";

				// Token: 0x0400D611 RID: 54801
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Someone dumped ",
					UI.FormatAsLink("Gunk", "LIQUIDGUNK"),
					" here instead of in a ",
					UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER"),
					"\n\nThis amenity requires maintenance"
				});
			}

			// Token: 0x020031ED RID: 12781
			public class NORESEARCHSELECTED
			{
				// Token: 0x0400D612 RID: 54802
				public static LocString NAME = "No Research Focus Selected";

				// Token: 0x0400D613 RID: 54803
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});

				// Token: 0x0400D614 RID: 54804
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " selected";

				// Token: 0x0400D615 RID: 54805
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});
			}

			// Token: 0x020031EE RID: 12782
			public class NORESEARCHORDESTINATIONSELECTED
			{
				// Token: 0x0400D616 RID: 54806
				public static LocString NAME = "No Research Focus or Starmap Destination Selected";

				// Token: 0x0400D617 RID: 54807
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap)
				});

				// Token: 0x0400D618 RID: 54808
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " or Starmap destination selected";

				// Token: 0x0400D619 RID: 54809
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", "[R]"),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", "[Z]")
				});
			}

			// Token: 0x020031EF RID: 12783
			public class RESEARCHING
			{
				// Token: 0x0400D61A RID: 54810
				public static LocString NAME = "Current " + UI.FormatAsLink("Research", "TECH") + ": {Tech}";

				// Token: 0x0400D61B RID: 54811
				public static LocString TOOLTIP = "Research produced at this station will be invested in {Tech}";
			}

			// Token: 0x020031F0 RID: 12784
			public class TINKERING
			{
				// Token: 0x0400D61C RID: 54812
				public static LocString NAME = "Tinkering: {0}";

				// Token: 0x0400D61D RID: 54813
				public static LocString TOOLTIP = "This Duplicant is creating {0} to use somewhere else";
			}

			// Token: 0x020031F1 RID: 12785
			public class VALVE
			{
				// Token: 0x0400D61E RID: 54814
				public static LocString NAME = "Max Flow Rate: {MaxFlow}";

				// Token: 0x0400D61F RID: 54815
				public static LocString TOOLTIP = "This valve is allowing flow at a volume of <b>{MaxFlow}</b>";
			}

			// Token: 0x020031F2 RID: 12786
			public class VALVEREQUEST
			{
				// Token: 0x0400D620 RID: 54816
				public static LocString NAME = "Requested Flow Rate: {QueuedMaxFlow}";

				// Token: 0x0400D621 RID: 54817
				public static LocString TOOLTIP = "Waiting for a Duplicant to adjust flow rate";
			}

			// Token: 0x020031F3 RID: 12787
			public class EMITTINGLIGHT
			{
				// Token: 0x0400D622 RID: 54818
				public static LocString NAME = "Emitting Light";

				// Token: 0x0400D623 RID: 54819
				public static LocString TOOLTIP = "Open the " + UI.FormatAsOverlay("Light Overlay", global::Action.Overlay5) + " to view this light's visibility radius";
			}

			// Token: 0x020031F4 RID: 12788
			public class KETTLEINSUFICIENTSOLIDS
			{
				// Token: 0x0400D624 RID: 54820
				public static LocString NAME = "Insufficient " + UI.FormatAsLink("Ice", "ICE");

				// Token: 0x0400D625 RID: 54821
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires a minimum of {0} ",
					UI.FormatAsLink("Ice", "ICE"),
					" in order to function\n\nDeliver more ",
					UI.FormatAsLink("Ice", "ICE"),
					" to operate this building"
				});
			}

			// Token: 0x020031F5 RID: 12789
			public class KETTLEINSUFICIENTFUEL
			{
				// Token: 0x0400D626 RID: 54822
				public static LocString NAME = "Insufficient " + UI.FormatAsLink("Wood", "WOODLOG");

				// Token: 0x0400D627 RID: 54823
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Colder ",
					UI.FormatAsLink("Ice", "ICE"),
					" increases the amount of ",
					UI.FormatAsLink("Wood", "WOODLOG"),
					" required for melting\n\nCurrent requirement: minimum {0} ",
					UI.FormatAsLink("Wood", "WOODLOG")
				});
			}

			// Token: 0x020031F6 RID: 12790
			public class KETTLEINSUFICIENTLIQUIDSPACE
			{
				// Token: 0x0400D628 RID: 54824
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400D629 RID: 54825
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.FormatAsLink("Ice Liquefier", "ICEKETTLE"),
					" needs to be emptied of ",
					UI.FormatAsLink("Water", "WATER"),
					" in order to resume function\n\nIt requires at least {2} of storage space in order to function properly\n\nCurrently storing {0} of a maximum {1} ",
					UI.FormatAsLink("Water", "WATER")
				});
			}

			// Token: 0x020031F7 RID: 12791
			public class KETTLEMELTING
			{
				// Token: 0x0400D62A RID: 54826
				public static LocString NAME = "Melting Ice";

				// Token: 0x0400D62B RID: 54827
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is currently melting stored ",
					UI.FormatAsLink("Ice", "ICE"),
					" to produce ",
					UI.FormatAsLink("Water", "WATER"),
					"\n\n",
					UI.FormatAsLink("Water", "WATER"),
					" output temperature: {0}"
				});
			}

			// Token: 0x020031F8 RID: 12792
			public class RATIONBOXCONTENTS
			{
				// Token: 0x0400D62C RID: 54828
				public static LocString NAME = "Storing: {Stored}";

				// Token: 0x0400D62D RID: 54829
				public static LocString TOOLTIP = "This box contains <b>{Stored}</b> of " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x020031F9 RID: 12793
			public class EMITTINGELEMENT
			{
				// Token: 0x0400D62E RID: 54830
				public static LocString NAME = "Emitting {ElementType}: {FlowRate}";

				// Token: 0x0400D62F RID: 54831
				public static LocString TOOLTIP = "Producing {ElementType} at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031FA RID: 12794
			public class EMITTINGCO2
			{
				// Token: 0x0400D630 RID: 54832
				public static LocString NAME = "Emitting CO<sub>2</sub>: {FlowRate}";

				// Token: 0x0400D631 RID: 54833
				public static LocString TOOLTIP = "Producing " + ELEMENTS.CARBONDIOXIDE.NAME + " at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031FB RID: 12795
			public class EMITTINGOXYGENAVG
			{
				// Token: 0x0400D632 RID: 54834
				public static LocString NAME = "Emitting " + UI.FormatAsLink("Oxygen", "OXYGEN") + ": {FlowRate}";

				// Token: 0x0400D633 RID: 54835
				public static LocString TOOLTIP = "Producing " + ELEMENTS.OXYGEN.NAME + " at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031FC RID: 12796
			public class EMITTINGGASAVG
			{
				// Token: 0x0400D634 RID: 54836
				public static LocString NAME = "Emitting {Element}: {FlowRate}";

				// Token: 0x0400D635 RID: 54837
				public static LocString TOOLTIP = "Producing {Element} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031FD RID: 12797
			public class EMITTINGBLOCKEDHIGHPRESSURE
			{
				// Token: 0x0400D636 RID: 54838
				public static LocString NAME = "Not Emitting: Overpressure";

				// Token: 0x0400D637 RID: 54839
				public static LocString TOOLTIP = "Ambient pressure is too high for {Element} to be released";
			}

			// Token: 0x020031FE RID: 12798
			public class EMITTINGBLOCKEDLOWTEMPERATURE
			{
				// Token: 0x0400D638 RID: 54840
				public static LocString NAME = "Not Emitting: Too Cold";

				// Token: 0x0400D639 RID: 54841
				public static LocString TOOLTIP = "Temperature is too low for {Element} to be released";
			}

			// Token: 0x020031FF RID: 12799
			public class PUMPINGLIQUIDORGAS
			{
				// Token: 0x0400D63A RID: 54842
				public static LocString NAME = "Average Flow Rate: {FlowRate}";

				// Token: 0x0400D63B RID: 54843
				public static LocString TOOLTIP = "This building is pumping an average volume of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02003200 RID: 12800
			public class WIRECIRCUITSTATUS
			{
				// Token: 0x0400D63C RID: 54844
				public static LocString NAME = "Current Load: {CurrentLoadAndColor} / {MaxLoad}";

				// Token: 0x0400D63D RID: 54845
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The current ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" load on this wire\n\nOverloading a wire will cause damage to the wire over time and cause it to break"
				});
			}

			// Token: 0x02003201 RID: 12801
			public class WIREMAXWATTAGESTATUS
			{
				// Token: 0x0400D63E RID: 54846
				public static LocString NAME = "Potential Load: {TotalPotentialLoadAndColor} / {MaxLoad}";

				// Token: 0x0400D63F RID: 54847
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"How much wattage this network will draw if all ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers on the network become active at once"
				});
			}

			// Token: 0x02003202 RID: 12802
			public class NOLIQUIDELEMENTTOPUMP
			{
				// Token: 0x0400D640 RID: 54848
				public static LocString NAME = "Pump Not In Liquid";

				// Token: 0x0400D641 RID: 54849
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x02003203 RID: 12803
			public class NOGASELEMENTTOPUMP
			{
				// Token: 0x0400D642 RID: 54850
				public static LocString NAME = "Pump Not In Gas";

				// Token: 0x0400D643 RID: 54851
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x02003204 RID: 12804
			public class INVALIDMASKSTATIONCONSUMPTIONSTATE
			{
				// Token: 0x0400D644 RID: 54852
				public static LocString NAME = "Station Not In Oxygen";

				// Token: 0x0400D645 RID: 54853
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This station must be submerged in ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x02003205 RID: 12805
			public class PIPEMAYMELT
			{
				// Token: 0x0400D646 RID: 54854
				public static LocString NAME = "High Melt Risk";

				// Token: 0x0400D647 RID: 54855
				public static LocString TOOLTIP = "This pipe is in danger of melting at the current " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x02003206 RID: 12806
			public class ELEMENTEMITTEROUTPUT
			{
				// Token: 0x0400D648 RID: 54856
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400D649 RID: 54857
				public static LocString TOOLTIP = "This object is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02003207 RID: 12807
			public class ELEMENTCONSUMER
			{
				// Token: 0x0400D64A RID: 54858
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400D64B RID: 54859
				public static LocString TOOLTIP = "This object is utilizing ambient {ElementTypes} from the environment";
			}

			// Token: 0x02003208 RID: 12808
			public class SPACECRAFTREADYTOLAND
			{
				// Token: 0x0400D64C RID: 54860
				public static LocString NAME = "Spacecraft ready to land";

				// Token: 0x0400D64D RID: 54861
				public static LocString TOOLTIP = "A spacecraft is ready to land";

				// Token: 0x0400D64E RID: 54862
				public static LocString NOTIFICATION = "Space mission complete";

				// Token: 0x0400D64F RID: 54863
				public static LocString NOTIFICATION_TOOLTIP = "Spacecrafts have completed their missions";
			}

			// Token: 0x02003209 RID: 12809
			public class CONSUMINGFROMSTORAGE
			{
				// Token: 0x0400D650 RID: 54864
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400D651 RID: 54865
				public static LocString TOOLTIP = "This building is consuming {ElementTypes} from storage";
			}

			// Token: 0x0200320A RID: 12810
			public class ELEMENTCONVERTEROUTPUT
			{
				// Token: 0x0400D652 RID: 54866
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400D653 RID: 54867
				public static LocString TOOLTIP = "This building is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x0200320B RID: 12811
			public class ELEMENTCONVERTERINPUT
			{
				// Token: 0x0400D654 RID: 54868
				public static LocString NAME = "Using {ElementTypes}: {FlowRate}";

				// Token: 0x0400D655 RID: 54869
				public static LocString TOOLTIP = "This building is using {ElementTypes} from storage at a rate of " + UI.FormatAsNegativeRate("{FlowRate}");
			}

			// Token: 0x0200320C RID: 12812
			public class AWAITINGCOMPOSTFLIP
			{
				// Token: 0x0400D656 RID: 54870
				public static LocString NAME = "Requires Flipping";

				// Token: 0x0400D657 RID: 54871
				public static LocString TOOLTIP = "Compost must be flipped periodically to produce " + UI.FormatAsLink("Dirt", "DIRT");
			}

			// Token: 0x0200320D RID: 12813
			public class AWAITINGWASTE
			{
				// Token: 0x0400D658 RID: 54872
				public static LocString NAME = "Awaiting Compostables";

				// Token: 0x0400D659 RID: 54873
				public static LocString TOOLTIP = "More waste material is required to begin the composting process";
			}

			// Token: 0x0200320E RID: 12814
			public class BATTERIESSUFFICIENTLYFULL
			{
				// Token: 0x0400D65A RID: 54874
				public static LocString NAME = "Batteries Sufficiently Full";

				// Token: 0x0400D65B RID: 54875
				public static LocString TOOLTIP = "All batteries are above the refill threshold";
			}

			// Token: 0x0200320F RID: 12815
			public class NEEDRESOURCEMASS
			{
				// Token: 0x0400D65C RID: 54876
				public static LocString NAME = "Insufficient Resources\n{ResourcesRequired}";

				// Token: 0x0400D65D RID: 54877
				public static LocString TOOLTIP = "The mass of material that was delivered to this building was too low\n\nDeliver more material to run this building";

				// Token: 0x0400D65E RID: 54878
				public static LocString LINE_ITEM = "• <b>{0}</b>";
			}

			// Token: 0x02003210 RID: 12816
			public class JOULESAVAILABLE
			{
				// Token: 0x0400D65F RID: 54879
				public static LocString NAME = "Power Available: {JoulesAvailable} / {JoulesCapacity}";

				// Token: 0x0400D660 RID: 54880
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"<b>{JoulesAvailable}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" available for use"
				});
			}

			// Token: 0x02003211 RID: 12817
			public class WATTAGE
			{
				// Token: 0x0400D661 RID: 54881
				public static LocString NAME = "Wattage: {Wattage}";

				// Token: 0x0400D662 RID: 54882
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003212 RID: 12818
			public class SOLARPANELWATTAGE
			{
				// Token: 0x0400D663 RID: 54883
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400D664 RID: 54884
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003213 RID: 12819
			public class MODULESOLARPANELWATTAGE
			{
				// Token: 0x0400D665 RID: 54885
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400D666 RID: 54886
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003214 RID: 12820
			public class WATTSON
			{
				// Token: 0x0400D667 RID: 54887
				public static LocString NAME = "Next Print: {TimeRemaining}";

				// Token: 0x0400D668 RID: 54888
				public static LocString TOOLTIP = "The Printing Pod can print out new Duplicants and useful resources over time.\nThe next print will be ready in <b>{TimeRemaining}</b>";

				// Token: 0x0400D669 RID: 54889
				public static LocString UNAVAILABLE = "UNAVAILABLE";
			}

			// Token: 0x02003215 RID: 12821
			public class FLUSHTOILET
			{
				// Token: 0x0400D66A RID: 54890
				public static LocString NAME = "{toilet} Ready";

				// Token: 0x0400D66B RID: 54891
				public static LocString TOOLTIP = "This bathroom is ready to receive visitors";
			}

			// Token: 0x02003216 RID: 12822
			public class FLUSHTOILETINUSE
			{
				// Token: 0x0400D66C RID: 54892
				public static LocString NAME = "{toilet} In Use";

				// Token: 0x0400D66D RID: 54893
				public static LocString TOOLTIP = "This bathroom is occupied";
			}

			// Token: 0x02003217 RID: 12823
			public class WIRECONNECTED
			{
				// Token: 0x0400D66E RID: 54894
				public static LocString NAME = "Wire Connected";

				// Token: 0x0400D66F RID: 54895
				public static LocString TOOLTIP = "This wire is connected to a network";
			}

			// Token: 0x02003218 RID: 12824
			public class WIRENOMINAL
			{
				// Token: 0x0400D670 RID: 54896
				public static LocString NAME = "Wire Nominal";

				// Token: 0x0400D671 RID: 54897
				public static LocString TOOLTIP = "This wire is able to handle the wattage it is receiving";
			}

			// Token: 0x02003219 RID: 12825
			public class WIREDISCONNECTED
			{
				// Token: 0x0400D672 RID: 54898
				public static LocString NAME = "Wire Disconnected";

				// Token: 0x0400D673 RID: 54899
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This wire is not connecting a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumer to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" generator"
				});
			}

			// Token: 0x0200321A RID: 12826
			public class COOLING
			{
				// Token: 0x0400D674 RID: 54900
				public static LocString NAME = "Cooling";

				// Token: 0x0400D675 RID: 54901
				public static LocString TOOLTIP = "This building is cooling the surrounding area";
			}

			// Token: 0x0200321B RID: 12827
			public class COOLINGSTALLEDHOTENV
			{
				// Token: 0x0400D676 RID: 54902
				public static LocString NAME = "Gas Too Hot";

				// Token: 0x0400D677 RID: 54903
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x0200321C RID: 12828
			public class COOLINGSTALLEDCOLDGAS
			{
				// Token: 0x0400D678 RID: 54904
				public static LocString NAME = "Gas Too Cold";

				// Token: 0x0400D679 RID: 54905
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x0200321D RID: 12829
			public class COOLINGSTALLEDHOTLIQUID
			{
				// Token: 0x0400D67A RID: 54906
				public static LocString NAME = "Liquid Too Hot";

				// Token: 0x0400D67B RID: 54907
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x0200321E RID: 12830
			public class COOLINGSTALLEDCOLDLIQUID
			{
				// Token: 0x0400D67C RID: 54908
				public static LocString NAME = "Liquid Too Cold";

				// Token: 0x0400D67D RID: 54909
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x0200321F RID: 12831
			public class CANNOTCOOLFURTHER
			{
				// Token: 0x0400D67E RID: 54910
				public static LocString NAME = "Minimum Temperature Reached";

				// Token: 0x0400D67F RID: 54911
				public static LocString TOOLTIP = "This building cannot cool the surrounding environment below <b>{0}</b>";
			}

			// Token: 0x02003220 RID: 12832
			public class HEATINGSTALLEDHOTENV
			{
				// Token: 0x0400D680 RID: 54912
				public static LocString NAME = "Target Temperature Reached";

				// Token: 0x0400D681 RID: 54913
				public static LocString TOOLTIP = "This building cannot heat the surrounding environment beyond <b>{0}</b>";
			}

			// Token: 0x02003221 RID: 12833
			public class HEATINGSTALLEDLOWMASS_GAS
			{
				// Token: 0x0400D682 RID: 54914
				public static LocString NAME = "Insufficient Atmosphere";

				// Token: 0x0400D683 RID: 54915
				public static LocString TOOLTIP = "This building cannot operate in a vacuum";
			}

			// Token: 0x02003222 RID: 12834
			public class HEATINGSTALLEDLOWMASS_LIQUID
			{
				// Token: 0x0400D684 RID: 54916
				public static LocString NAME = "Not Submerged In Liquid";

				// Token: 0x0400D685 RID: 54917
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x02003223 RID: 12835
			public class BUILDINGDISABLED
			{
				// Token: 0x0400D686 RID: 54918
				public static LocString NAME = "Building Disabled";

				// Token: 0x0400D687 RID: 54919
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Press ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume use"
				});
			}

			// Token: 0x02003224 RID: 12836
			public class MISSINGREQUIREMENTS
			{
				// Token: 0x0400D688 RID: 54920
				public static LocString NAME = "Missing Requirements";

				// Token: 0x0400D689 RID: 54921
				public static LocString TOOLTIP = "There are some problems that need to be fixed before this building is operational";
			}

			// Token: 0x02003225 RID: 12837
			public class GETTINGREADY
			{
				// Token: 0x0400D68A RID: 54922
				public static LocString NAME = "Getting Ready";

				// Token: 0x0400D68B RID: 54923
				public static LocString TOOLTIP = "This building will soon be ready to use";
			}

			// Token: 0x02003226 RID: 12838
			public class WORKING
			{
				// Token: 0x0400D68C RID: 54924
				public static LocString NAME = "Normal";

				// Token: 0x0400D68D RID: 54925
				public static LocString TOOLTIP = "This building is working as intended";
			}

			// Token: 0x02003227 RID: 12839
			public class GRAVEEMPTY
			{
				// Token: 0x0400D68E RID: 54926
				public static LocString NAME = "Empty";

				// Token: 0x0400D68F RID: 54927
				public static LocString TOOLTIP = "This memorial honors no one.";
			}

			// Token: 0x02003228 RID: 12840
			public class GRAVE
			{
				// Token: 0x0400D690 RID: 54928
				public static LocString NAME = "RIP {DeadDupe}";

				// Token: 0x0400D691 RID: 54929
				public static LocString TOOLTIP = "{Epitaph}";
			}

			// Token: 0x02003229 RID: 12841
			public class AWAITINGARTING
			{
				// Token: 0x0400D692 RID: 54930
				public static LocString NAME = "Incomplete Artwork";

				// Token: 0x0400D693 RID: 54931
				public static LocString TOOLTIP = "This building requires a Duplicant's artistic touch";
			}

			// Token: 0x0200322A RID: 12842
			public class LOOKINGUGLY
			{
				// Token: 0x0400D694 RID: 54932
				public static LocString NAME = "Crude";

				// Token: 0x0400D695 RID: 54933
				public static LocString TOOLTIP = "Honestly, Morbs could've done better";
			}

			// Token: 0x0200322B RID: 12843
			public class LOOKINGOKAY
			{
				// Token: 0x0400D696 RID: 54934
				public static LocString NAME = "Quaint";

				// Token: 0x0400D697 RID: 54935
				public static LocString TOOLTIP = "Duplicants find this art piece quite charming";
			}

			// Token: 0x0200322C RID: 12844
			public class LOOKINGGREAT
			{
				// Token: 0x0400D698 RID: 54936
				public static LocString NAME = "Masterpiece";

				// Token: 0x0400D699 RID: 54937
				public static LocString TOOLTIP = "This poignant piece stirs something deep within each Duplicant's soul";
			}

			// Token: 0x0200322D RID: 12845
			public class EXPIRED
			{
				// Token: 0x0400D69A RID: 54938
				public static LocString NAME = "Depleted";

				// Token: 0x0400D69B RID: 54939
				public static LocString TOOLTIP = "This building has no more use";
			}

			// Token: 0x0200322E RID: 12846
			public class COOLINGWATER
			{
				// Token: 0x0400D69C RID: 54940
				public static LocString NAME = "Cooling Water";

				// Token: 0x0400D69D RID: 54941
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is cooling ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" down to its freezing point"
				});
			}

			// Token: 0x0200322F RID: 12847
			public class EXCAVATOR_BOMB
			{
				// Token: 0x02003CD6 RID: 15574
				public class UNARMED
				{
					// Token: 0x0400F133 RID: 61747
					public static LocString NAME = "Unarmed";

					// Token: 0x0400F134 RID: 61748
					public static LocString TOOLTIP = "This explosive is currently inactive";
				}

				// Token: 0x02003CD7 RID: 15575
				public class ARMED
				{
					// Token: 0x0400F135 RID: 61749
					public static LocString NAME = "Armed";

					// Token: 0x0400F136 RID: 61750
					public static LocString TOOLTIP = "Stand back, this baby's ready to blow!";
				}

				// Token: 0x02003CD8 RID: 15576
				public class COUNTDOWN
				{
					// Token: 0x0400F137 RID: 61751
					public static LocString NAME = "Countdown: {0}";

					// Token: 0x0400F138 RID: 61752
					public static LocString TOOLTIP = "<b>{0}</b> seconds until detonation";
				}

				// Token: 0x02003CD9 RID: 15577
				public class DUPE_DANGER
				{
					// Token: 0x0400F139 RID: 61753
					public static LocString NAME = "Duplicant Preservation Override";

					// Token: 0x0400F13A RID: 61754
					public static LocString TOOLTIP = "Explosive disabled due to close Duplicant proximity";
				}

				// Token: 0x02003CDA RID: 15578
				public class EXPLODING
				{
					// Token: 0x0400F13B RID: 61755
					public static LocString NAME = "Exploding";

					// Token: 0x0400F13C RID: 61756
					public static LocString TOOLTIP = "Kaboom!";
				}
			}

			// Token: 0x02003230 RID: 12848
			public class BURNER
			{
				// Token: 0x02003CDB RID: 15579
				public class BURNING_FUEL
				{
					// Token: 0x0400F13D RID: 61757
					public static LocString NAME = "Consuming Fuel: {0}";

					// Token: 0x0400F13E RID: 61758
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}

				// Token: 0x02003CDC RID: 15580
				public class HAS_FUEL
				{
					// Token: 0x0400F13F RID: 61759
					public static LocString NAME = "Fueled: {0}";

					// Token: 0x0400F140 RID: 61760
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}
			}

			// Token: 0x02003231 RID: 12849
			public class CREATURE_REUSABLE_TRAP
			{
				// Token: 0x02003CDD RID: 15581
				public class NEEDS_ARMING
				{
					// Token: 0x0400F141 RID: 61761
					public static LocString NAME = "Waiting to be Armed";

					// Token: 0x0400F142 RID: 61762
					public static LocString TOOLTIP = "Waiting for a Duplicant to arm this trap\n\nOnly Duplicants with the " + DUPLICANTS.ROLES.RANCHER.NAME + " skill can arm traps";
				}

				// Token: 0x02003CDE RID: 15582
				public class READY
				{
					// Token: 0x0400F143 RID: 61763
					public static LocString NAME = "Armed";

					// Token: 0x0400F144 RID: 61764
					public static LocString TOOLTIP = "This trap has been armed and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x02003CDF RID: 15583
				public class SPRUNG
				{
					// Token: 0x0400F145 RID: 61765
					public static LocString NAME = "Sprung";

					// Token: 0x0400F146 RID: 61766
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x02003232 RID: 12850
			public class CREATURE_TRAP
			{
				// Token: 0x02003CE0 RID: 15584
				public class NEEDSBAIT
				{
					// Token: 0x0400F147 RID: 61767
					public static LocString NAME = "Needs Bait";

					// Token: 0x0400F148 RID: 61768
					public static LocString TOOLTIP = "This trap needs to be baited before it can be set";
				}

				// Token: 0x02003CE1 RID: 15585
				public class READY
				{
					// Token: 0x0400F149 RID: 61769
					public static LocString NAME = "Set";

					// Token: 0x0400F14A RID: 61770
					public static LocString TOOLTIP = "This trap has been set and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x02003CE2 RID: 15586
				public class SPRUNG
				{
					// Token: 0x0400F14B RID: 61771
					public static LocString NAME = "Sprung";

					// Token: 0x0400F14C RID: 61772
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x02003233 RID: 12851
			public class ACCESS_CONTROL
			{
				// Token: 0x02003CE3 RID: 15587
				public class ACTIVE
				{
					// Token: 0x0400F14D RID: 61773
					public static LocString NAME = "Access Restrictions";

					// Token: 0x0400F14E RID: 61774
					public static LocString TOOLTIP = "Some Duplicants are prohibited from passing through this door by the current " + UI.PRE_KEYWORD + "Access Permissions" + UI.PST_KEYWORD;
				}

				// Token: 0x02003CE4 RID: 15588
				public class OFFLINE
				{
					// Token: 0x0400F14F RID: 61775
					public static LocString NAME = "Access Control Offline";

					// Token: 0x0400F150 RID: 61776
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This door has granted Emergency ",
						UI.PRE_KEYWORD,
						"Access Permissions",
						UI.PST_KEYWORD,
						"\n\nAll Duplicants are permitted to pass through it until ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" is restored"
					});
				}
			}

			// Token: 0x02003234 RID: 12852
			public class REQUIRESSKILLPERK
			{
				// Token: 0x0400D69E RID: 54942
				public static LocString NAME = "Skill-Required Operation";

				// Token: 0x0400D69F RID: 54943
				public static LocString TOOLTIP = "Only Duplicants with the {Skills} Skill can operate this building";

				// Token: 0x0400D6A0 RID: 54944
				public static LocString TOOLTIP_DLC3 = "Only Duplicants with the {Skills} Skill or {Boosters} can operate this building";
			}

			// Token: 0x02003235 RID: 12853
			public class DIGREQUIRESSKILLPERK
			{
				// Token: 0x0400D6A1 RID: 54945
				public static LocString NAME = "Skill-Required Dig";

				// Token: 0x0400D6A2 RID: 54946
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with one of the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can mine this material:\n{Skills}"
				});

				// Token: 0x0400D6A3 RID: 54947
				public static LocString TOOLTIP_DLC3 = "Only Duplicants with the {Skills} Skill or {Boosters} can mine this material";
			}

			// Token: 0x02003236 RID: 12854
			public class COLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400D6A4 RID: 54948
				public static LocString NAME = "Colony Lacks {Skills} Skill";

				// Token: 0x0400D6A5 RID: 54949
				public static LocString TOOLTIP = "{Skills} Skill required to operate\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " to teach {Skills} to a Duplicant";

				// Token: 0x0400D6A6 RID: 54950
				public static LocString TOOLTIP_DLC3 = "{Skills} Skill or {Boosters} required to operate\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " to teach {Skills} to a Duplicant";
			}

			// Token: 0x02003237 RID: 12855
			public class CLUSTERCOLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400D6A7 RID: 54951
				public static LocString NAME = "Local Colony Lacks {Skills} Skill";

				// Token: 0x0400D6A8 RID: 54952
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.COLONYLACKSREQUIREDSKILLPERK.TOOLTIP + ", or bring a Duplicant with the skill from another " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400D6A9 RID: 54953
				public static LocString TOOLTIP_DLC3 = BUILDING.STATUSITEMS.COLONYLACKSREQUIREDSKILLPERK.TOOLTIP_DLC3 + ", or bring a Duplicant with this skill or booster from another " + UI.CLUSTERMAP.PLANETOID;
			}

			// Token: 0x02003238 RID: 12856
			public class WORKREQUIRESMINION
			{
				// Token: 0x0400D6AA RID: 54954
				public static LocString NAME = "Duplicant Operation Required";

				// Token: 0x0400D6AB RID: 54955
				public static LocString TOOLTIP = "A Duplicant must be present to complete this operation";
			}

			// Token: 0x02003239 RID: 12857
			public class SWITCHSTATUSACTIVE
			{
				// Token: 0x0400D6AC RID: 54956
				public static LocString NAME = "Active";

				// Token: 0x0400D6AD RID: 54957
				public static LocString TOOLTIP = "This switch is currently toggled <b>On</b>";
			}

			// Token: 0x0200323A RID: 12858
			public class SWITCHSTATUSINACTIVE
			{
				// Token: 0x0400D6AE RID: 54958
				public static LocString NAME = "Inactive";

				// Token: 0x0400D6AF RID: 54959
				public static LocString TOOLTIP = "This switch is currently toggled <b>Off</b>";
			}

			// Token: 0x0200323B RID: 12859
			public class LOGICSWITCHSTATUSACTIVE
			{
				// Token: 0x0400D6B0 RID: 54960
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400D6B1 RID: 54961
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x0200323C RID: 12860
			public class LOGICSWITCHSTATUSINACTIVE
			{
				// Token: 0x0400D6B2 RID: 54962
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400D6B3 RID: 54963
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x0200323D RID: 12861
			public class LOGICSENSORSTATUSACTIVE
			{
				// Token: 0x0400D6B4 RID: 54964
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400D6B5 RID: 54965
				public static LocString TOOLTIP = "This sensor is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x0200323E RID: 12862
			public class LOGICSENSORSTATUSINACTIVE
			{
				// Token: 0x0400D6B6 RID: 54966
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400D6B7 RID: 54967
				public static LocString TOOLTIP = "This sensor is currently sending " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x0200323F RID: 12863
			public class PLAYERCONTROLLEDTOGGLESIDESCREEN
			{
				// Token: 0x0400D6B8 RID: 54968
				public static LocString NAME = "Pending Toggle on Unpause";

				// Token: 0x0400D6B9 RID: 54969
				public static LocString TOOLTIP = "This will be toggled when time is unpaused";
			}

			// Token: 0x02003240 RID: 12864
			public class FOOD_CONTAINERS_OUTSIDE_RANGE
			{
				// Token: 0x0400D6BA RID: 54970
				public static LocString NAME = "Unreachable food";

				// Token: 0x0400D6BB RID: 54971
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x02003241 RID: 12865
			public class TOILETS_OUTSIDE_RANGE
			{
				// Token: 0x0400D6BC RID: 54972
				public static LocString NAME = "Unreachable restroom";

				// Token: 0x0400D6BD RID: 54973
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Toilets",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x02003242 RID: 12866
			public class BUILDING_DEPRECATED
			{
				// Token: 0x0400D6BE RID: 54974
				public static LocString NAME = "Building Deprecated";

				// Token: 0x0400D6BF RID: 54975
				public static LocString TOOLTIP = "This building is from an older version of the game and its use is not intended";
			}

			// Token: 0x02003243 RID: 12867
			public class TURBINE_BLOCKED_INPUT
			{
				// Token: 0x0400D6C0 RID: 54976
				public static LocString NAME = "All Inputs Blocked";

				// Token: 0x0400D6C1 RID: 54977
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine's ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are blocked, so it can't intake any ",
					ELEMENTS.STEAM.NAME,
					".\n\nThe ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are located directly below the foundation ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" this building is resting on."
				});
			}

			// Token: 0x02003244 RID: 12868
			public class TURBINE_PARTIALLY_BLOCKED_INPUT
			{
				// Token: 0x0400D6C2 RID: 54978
				public static LocString NAME = "{Blocked}/{Total} Inputs Blocked";

				// Token: 0x0400D6C3 RID: 54979
				public static LocString TOOLTIP = "<b>{Blocked}</b> of this turbine's <b>{Total}</b> inputs have been blocked, resulting in reduced throughput";
			}

			// Token: 0x02003245 RID: 12869
			public class TURBINE_TOO_HOT
			{
				// Token: 0x0400D6C4 RID: 54980
				public static LocString NAME = "Turbine Too Hot";

				// Token: 0x0400D6C5 RID: 54981
				public static LocString TOOLTIP = "This turbine must be below <b>{Overheat_Temperature}</b> to properly process {Src_Element} into {Dest_Element}";
			}

			// Token: 0x02003246 RID: 12870
			public class TURBINE_BLOCKED_OUTPUT
			{
				// Token: 0x0400D6C6 RID: 54982
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400D6C7 RID: 54983
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A blocked ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" has stopped this turbine from functioning"
				});
			}

			// Token: 0x02003247 RID: 12871
			public class TURBINE_INSUFFICIENT_MASS
			{
				// Token: 0x0400D6C8 RID: 54984
				public static LocString NAME = "Not Enough {Src_Element}";

				// Token: 0x0400D6C9 RID: 54985
				public static LocString TOOLTIP = "The {Src_Element} present below this turbine must be at least <b>{Min_Mass}</b> in order to turn the turbine";
			}

			// Token: 0x02003248 RID: 12872
			public class TURBINE_INSUFFICIENT_TEMPERATURE
			{
				// Token: 0x0400D6CA RID: 54986
				public static LocString NAME = "{Src_Element} Temperature Below {Active_Temperature}";

				// Token: 0x0400D6CB RID: 54987
				public static LocString TOOLTIP = "This turbine requires {Src_Element} that is a minimum of <b>{Active_Temperature}</b> in order to produce power";
			}

			// Token: 0x02003249 RID: 12873
			public class TURBINE_ACTIVE_WATTAGE
			{
				// Token: 0x0400D6CC RID: 54988
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400D6CD RID: 54989
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt is running at <b>{Efficiency}</b> of full capacity\n\nIncrease {Src_Element} ",
					UI.PRE_KEYWORD,
					"Mass",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" to improve output"
				});
			}

			// Token: 0x0200324A RID: 12874
			public class TURBINE_SPINNING_UP
			{
				// Token: 0x0400D6CE RID: 54990
				public static LocString NAME = "Spinning Up";

				// Token: 0x0400D6CF RID: 54991
				public static LocString TOOLTIP = "This turbine is currently spinning up\n\nSpinning up allows a turbine to continue running for a short period if the pressure it needs to run becomes unavailable";
			}

			// Token: 0x0200324B RID: 12875
			public class TURBINE_ACTIVE
			{
				// Token: 0x0400D6D0 RID: 54992
				public static LocString NAME = "Active";

				// Token: 0x0400D6D1 RID: 54993
				public static LocString TOOLTIP = "This turbine is running at <b>{0}RPM</b>";
			}

			// Token: 0x0200324C RID: 12876
			public class WELL_PRESSURIZING
			{
				// Token: 0x0400D6D2 RID: 54994
				public static LocString NAME = "Backpressure: {0}";

				// Token: 0x0400D6D3 RID: 54995
				public static LocString TOOLTIP = "Well pressure increases with each use and must be periodically relieved to prevent shutdown";
			}

			// Token: 0x0200324D RID: 12877
			public class WELL_OVERPRESSURE
			{
				// Token: 0x0400D6D4 RID: 54996
				public static LocString NAME = "Overpressure";

				// Token: 0x0400D6D5 RID: 54997
				public static LocString TOOLTIP = "This well can no longer function due to excessive backpressure";
			}

			// Token: 0x0200324E RID: 12878
			public class NOTINANYROOM
			{
				// Token: 0x0400D6D6 RID: 54998
				public static LocString NAME = "Outside of room";

				// Token: 0x0400D6D7 RID: 54999
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x0200324F RID: 12879
			public class NOTINREQUIREDROOM
			{
				// Token: 0x0400D6D8 RID: 55000
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400D6D9 RID: 55001
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a {0} for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x02003250 RID: 12880
			public class NOTINRECOMMENDEDROOM
			{
				// Token: 0x0400D6DA RID: 55002
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400D6DB RID: 55003
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"It is recommended to build this building inside a {0}\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x02003251 RID: 12881
			public class RELEASING_PRESSURE
			{
				// Token: 0x0400D6DC RID: 55004
				public static LocString NAME = "Releasing Pressure";

				// Token: 0x0400D6DD RID: 55005
				public static LocString TOOLTIP = "Pressure buildup is being safely released";
			}

			// Token: 0x02003252 RID: 12882
			public class LOGIC_FEEDBACK_LOOP
			{
				// Token: 0x0400D6DE RID: 55006
				public static LocString NAME = "Feedback Loop";

				// Token: 0x0400D6DF RID: 55007
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Feedback loops prevent automation grids from functioning\n\nFeedback loops occur when the ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" of an automated building connects back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD,
					" through the Automation grid"
				});
			}

			// Token: 0x02003253 RID: 12883
			public class ENOUGH_COOLANT
			{
				// Token: 0x0400D6E0 RID: 55008
				public static LocString NAME = "Awaiting Coolant";

				// Token: 0x0400D6E1 RID: 55009
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x02003254 RID: 12884
			public class ENOUGH_FUEL
			{
				// Token: 0x0400D6E2 RID: 55010
				public static LocString NAME = "Awaiting Fuel";

				// Token: 0x0400D6E3 RID: 55011
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x02003255 RID: 12885
			public class LOGIC
			{
				// Token: 0x0400D6E4 RID: 55012
				public static LocString LOGIC_CONTROLLED_ENABLED = "Enabled by Automation Grid";

				// Token: 0x0400D6E5 RID: 55013
				public static LocString LOGIC_CONTROLLED_DISABLED = "Disabled by Automation Grid";
			}

			// Token: 0x02003256 RID: 12886
			public class GANTRY
			{
				// Token: 0x0400D6E6 RID: 55014
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400D6E7 RID: 55015
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400D6E8 RID: 55016
				public static LocString EXTENDED = "Extended";

				// Token: 0x0400D6E9 RID: 55017
				public static LocString RETRACTED = "Retracted";
			}

			// Token: 0x02003257 RID: 12887
			public class OBJECTDISPENSER
			{
				// Token: 0x0400D6EA RID: 55018
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400D6EB RID: 55019
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400D6EC RID: 55020
				public static LocString OPENED = "Opened";

				// Token: 0x0400D6ED RID: 55021
				public static LocString CLOSED = "Closed";
			}

			// Token: 0x02003258 RID: 12888
			public class TOO_COLD
			{
				// Token: 0x0400D6EE RID: 55022
				public static LocString NAME = "Too Cold";

				// Token: 0x0400D6EF RID: 55023
				public static LocString TOOLTIP = "Either this building or its surrounding environment is too cold to operate";
			}

			// Token: 0x02003259 RID: 12889
			public class CHECKPOINT
			{
				// Token: 0x0400D6F0 RID: 55024
				public static LocString LOGIC_CONTROLLED_OPEN = "Clearance: Permitted";

				// Token: 0x0400D6F1 RID: 55025
				public static LocString LOGIC_CONTROLLED_CLOSED = "Clearance: Not Permitted";

				// Token: 0x0400D6F2 RID: 55026
				public static LocString LOGIC_CONTROLLED_DISCONNECTED = "No Automation";

				// Token: 0x02003CE5 RID: 15589
				public class TOOLTIPS
				{
					// Token: 0x0400F151 RID: 61777
					public static LocString LOGIC_CONTROLLED_OPEN = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ", preventing Duplicants from passing";

					// Token: 0x0400F152 RID: 61778
					public static LocString LOGIC_CONTROLLED_CLOSED = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ", allowing Duplicants to pass";

					// Token: 0x0400F153 RID: 61779
					public static LocString LOGIC_CONTROLLED_DISCONNECTED = string.Concat(new string[]
					{
						"This Checkpoint has not been connected to an ",
						UI.PRE_KEYWORD,
						"Automation",
						UI.PST_KEYWORD,
						" grid"
					});
				}
			}

			// Token: 0x0200325A RID: 12890
			public class HIGHENERGYPARTICLEREDIRECTOR
			{
				// Token: 0x0400D6F3 RID: 55027
				public static LocString LOGIC_CONTROLLED_STANDBY = "Incoming Radbolts: Ignore";

				// Token: 0x0400D6F4 RID: 55028
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Incoming Radbolts: Redirect";

				// Token: 0x0400D6F5 RID: 55029
				public static LocString NORMAL = "Normal";

				// Token: 0x02003CE6 RID: 15590
				public class TOOLTIPS
				{
					// Token: 0x0400F154 RID: 61780
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400F155 RID: 61781
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400F156 RID: 61782
					public static LocString NORMAL = "Incoming Radbolts will be accepted and redirected";
				}
			}

			// Token: 0x0200325B RID: 12891
			public class HIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x0400D6F6 RID: 55030
				public static LocString LOGIC_CONTROLLED_STANDBY = "Launch Radbolt: Off";

				// Token: 0x0400D6F7 RID: 55031
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Launch Radbolt: On";

				// Token: 0x0400D6F8 RID: 55032
				public static LocString NORMAL = "Normal";

				// Token: 0x02003CE7 RID: 15591
				public class TOOLTIPS
				{
					// Token: 0x0400F157 RID: 61783
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400F158 RID: 61784
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400F159 RID: 61785
					public static LocString NORMAL = string.Concat(new string[]
					{
						"Incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD,
						" will be accepted and redirected"
					});
				}
			}

			// Token: 0x0200325C RID: 12892
			public class AWAITINGFUEL
			{
				// Token: 0x0400D6F9 RID: 55033
				public static LocString NAME = "Awaiting Fuel: {0}";

				// Token: 0x0400D6FA RID: 55034
				public static LocString TOOLTIP = "This building requires <b>{1}</b> of {0} to operate";
			}

			// Token: 0x0200325D RID: 12893
			public class FOSSILHUNT
			{
				// Token: 0x02003CE8 RID: 15592
				public class PENDING_EXCAVATION
				{
					// Token: 0x0400F15A RID: 61786
					public static LocString NAME = "Awaiting Excavation";

					// Token: 0x0400F15B RID: 61787
					public static LocString TOOLTIP = "Currently awaiting excavation by a Duplicant";
				}

				// Token: 0x02003CE9 RID: 15593
				public class EXCAVATING
				{
					// Token: 0x0400F15C RID: 61788
					public static LocString NAME = "Excavation In Progress";

					// Token: 0x0400F15D RID: 61789
					public static LocString TOOLTIP = "Currently being excavated by a Duplicant";
				}
			}

			// Token: 0x0200325E RID: 12894
			public class MEGABRAINTANK
			{
				// Token: 0x02003CEA RID: 15594
				public class PROGRESS
				{
					// Token: 0x020040F4 RID: 16628
					public class PROGRESSIONRATE
					{
						// Token: 0x0400FAEA RID: 64234
						public static LocString NAME = "Dream Journals: {ActivationProgress}";

						// Token: 0x0400FAEB RID: 64235
						public static LocString TOOLTIP = "Currently awaiting the Dream Journals necessary to restore this building to full functionality";
					}

					// Token: 0x020040F5 RID: 16629
					public class DREAMANALYSIS
					{
						// Token: 0x0400FAEC RID: 64236
						public static LocString NAME = "Analyzing Dreams: {TimeToComplete}s";

						// Token: 0x0400FAED RID: 64237
						public static LocString TOOLTIP = "Maximum Aptitude effect sustained while dream analysis continues";
					}
				}

				// Token: 0x02003CEB RID: 15595
				public class COMPLETE
				{
					// Token: 0x0400F15E RID: 61790
					public static LocString NAME = "Fully Restored";

					// Token: 0x0400F15F RID: 61791
					public static LocString TOOLTIP = "This building is functioning at full capacity";
				}
			}

			// Token: 0x0200325F RID: 12895
			public class MEGABRAINNOTENOUGHOXYGEN
			{
				// Token: 0x0400D6FB RID: 55035
				public static LocString NAME = "Lacks Oxygen";

				// Token: 0x0400D6FC RID: 55036
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building needs ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" in order to function"
				});
			}

			// Token: 0x02003260 RID: 12896
			public class NOLOGICWIRECONNECTED
			{
				// Token: 0x0400D6FD RID: 55037
				public static LocString NAME = "No Automation Wire Connected";

				// Token: 0x0400D6FE RID: 55038
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to an ",
					UI.PRE_KEYWORD,
					"Automation",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x02003261 RID: 12897
			public class NOTUBECONNECTED
			{
				// Token: 0x0400D6FF RID: 55039
				public static LocString NAME = "No Tube Connected";

				// Token: 0x0400D700 RID: 55040
				public static LocString TOOLTIP = "The first section of tube extending from a " + BUILDINGS.PREFABS.TRAVELTUBEENTRANCE.NAME + " must connect directly upward";
			}

			// Token: 0x02003262 RID: 12898
			public class NOTUBEEXITS
			{
				// Token: 0x0400D701 RID: 55041
				public static LocString NAME = "No Landing Available";

				// Token: 0x0400D702 RID: 55042
				public static LocString TOOLTIP = "Duplicants can only exit a tube when there is somewhere for them to land within <b>two tiles</b>";
			}

			// Token: 0x02003263 RID: 12899
			public class STOREDCHARGE
			{
				// Token: 0x0400D703 RID: 55043
				public static LocString NAME = "Charge Available: {0}/{1}";

				// Token: 0x0400D704 RID: 55044
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has <b>{0}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt consumes ",
					UI.FormatAsNegativeRate("{2}"),
					" per use"
				});
			}

			// Token: 0x02003264 RID: 12900
			public class ORNAMENTDISABLED
			{
				// Token: 0x0400D705 RID: 55045
				public static LocString NAME = "Broken Display";

				// Token: 0x0400D706 RID: 55046
				public static LocString TOOLTIP = "This ornament's display structure is currently disabled\n\nIt will not be counted as a displayed ornament";
			}

			// Token: 0x02003265 RID: 12901
			public class PEDESTALNOITEMDISPLAYED
			{
				// Token: 0x0400D707 RID: 55047
				public static LocString NAME = "No Object Displayed";

				// Token: 0x0400D708 RID: 55048
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must display an object in order to be counted as a ",
					UI.PRE_KEYWORD,
					"Decor building",
					UI.PST_KEYWORD,
					" for a room"
				});
			}

			// Token: 0x02003266 RID: 12902
			public class NEEDEGG
			{
				// Token: 0x0400D709 RID: 55049
				public static LocString NAME = "No Egg Selected";

				// Token: 0x0400D70A RID: 55050
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Collect ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" from ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" to incubate"
				});
			}

			// Token: 0x02003267 RID: 12903
			public class NOAVAILABLEEGG
			{
				// Token: 0x0400D70B RID: 55051
				public static LocString NAME = "No Egg Available";

				// Token: 0x0400D70C RID: 55052
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" is not currently available"
				});
			}

			// Token: 0x02003268 RID: 12904
			public class AWAITINGEGGDELIVERY
			{
				// Token: 0x0400D70D RID: 55053
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400D70E RID: 55054
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Egg" + UI.PST_KEYWORD;
			}

			// Token: 0x02003269 RID: 12905
			public class INCUBATORPROGRESS
			{
				// Token: 0x0400D70F RID: 55055
				public static LocString NAME = "Incubating: {Percent}";

				// Token: 0x0400D710 RID: 55056
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" incubating cozily\n\nIt will hatch when ",
					UI.PRE_KEYWORD,
					"Incubation",
					UI.PST_KEYWORD,
					" reaches <b>100%</b>"
				});
			}

			// Token: 0x0200326A RID: 12906
			public class NETWORKQUALITY
			{
				// Token: 0x0400D711 RID: 55057
				public static LocString NAME = "Scan Network Quality: {TotalQuality}";

				// Token: 0x0400D712 RID: 55058
				public static LocString TOOLTIP = "This scanner network is scanning at <b>{TotalQuality}</b> effectiveness\n\nIt will detect incoming objects <b>{WorstTime}</b> to <b>{BestTime}</b> before they arrive\n\nBuild multiple " + BUILDINGS.PREFABS.COMETDETECTOR.NAME + "s to increase surface coverage and improve network quality\n\n    • Surface Coverage: <b>{Coverage}</b>";
			}

			// Token: 0x0200326B RID: 12907
			public class DETECTORSCANNING
			{
				// Token: 0x0400D713 RID: 55059
				public static LocString NAME = "Scanning";

				// Token: 0x0400D714 RID: 55060
				public static LocString TOOLTIP = "This scanner is currently scouring space for anything of interest";
			}

			// Token: 0x0200326C RID: 12908
			public class INCOMINGMETEORS
			{
				// Token: 0x0400D715 RID: 55061
				public static LocString NAME = "Incoming Object Detected";

				// Token: 0x0400D716 RID: 55062
				public static LocString TOOLTIP = "Warning!\n\nHigh velocity objects on approach!";
			}

			// Token: 0x0200326D RID: 12909
			public class SPACE_VISIBILITY_NONE
			{
				// Token: 0x0400D717 RID: 55063
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400D718 RID: 55064
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x0200326E RID: 12910
			public class SPACE_VISIBILITY_REDUCED
			{
				// Token: 0x0400D719 RID: 55065
				public static LocString NAME = "Reduced Visibility";

				// Token: 0x0400D71A RID: 55066
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo operate at maximum speed, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x0200326F RID: 12911
			public class LANDEDROCKETLACKSPASSENGERMODULE
			{
				// Token: 0x0400D71B RID: 55067
				public static LocString NAME = "Rocket lacks spacefarer module";

				// Token: 0x0400D71C RID: 55068
				public static LocString TOOLTIP = "A rocket must have a spacefarer module";
			}

			// Token: 0x02003270 RID: 12912
			public class PATH_NOT_CLEAR
			{
				// Token: 0x0400D71D RID: 55069
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400D71E RID: 55070
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this rocket:\n    • {0}\n\nThis rocket requires a clear flight path for launch";

				// Token: 0x0400D71F RID: 55071
				public static LocString TILE_FORMAT = "Solid {0}";
			}

			// Token: 0x02003271 RID: 12913
			public class RAILGUN_PATH_NOT_CLEAR
			{
				// Token: 0x0400D720 RID: 55072
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400D721 RID: 55073
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this " + UI.FormatAsLink("Interplanetary Launcher", "RAILGUN") + "\n\nThis launcher requires a clear path to launch payloads";
			}

			// Token: 0x02003272 RID: 12914
			public class RAILGUN_NO_DESTINATION
			{
				// Token: 0x0400D722 RID: 55074
				public static LocString NAME = "No Delivery Destination";

				// Token: 0x0400D723 RID: 55075
				public static LocString TOOLTIP = "A delivery destination has not been set";
			}

			// Token: 0x02003273 RID: 12915
			public class NOSURFACESIGHT
			{
				// Token: 0x0400D724 RID: 55076
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400D725 RID: 55077
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x02003274 RID: 12916
			public class ROCKETRESTRICTIONACTIVE
			{
				// Token: 0x0400D726 RID: 55078
				public static LocString NAME = "Access: Restricted";

				// Token: 0x0400D727 RID: 55079
				public static LocString TOOLTIP = "This building cannot be operated while restricted, though it can be filled\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02003275 RID: 12917
			public class ROCKETRESTRICTIONINACTIVE
			{
				// Token: 0x0400D728 RID: 55080
				public static LocString NAME = "Access: Not Restricted";

				// Token: 0x0400D729 RID: 55081
				public static LocString TOOLTIP = "This building's operation is not restricted\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02003276 RID: 12918
			public class NOROCKETRESTRICTION
			{
				// Token: 0x0400D72A RID: 55082
				public static LocString NAME = "Not Controlled";

				// Token: 0x0400D72B RID: 55083
				public static LocString TOOLTIP = "This building is not controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02003277 RID: 12919
			public class BROADCASTEROUTOFRANGE
			{
				// Token: 0x0400D72C RID: 55084
				public static LocString NAME = "Broadcaster Out of Range";

				// Token: 0x0400D72D RID: 55085
				public static LocString TOOLTIP = "This receiver is too far from the selected broadcaster to get signal updates";
			}

			// Token: 0x02003278 RID: 12920
			public class LOSINGRADBOLTS
			{
				// Token: 0x0400D72E RID: 55086
				public static LocString NAME = "Radbolt Decay";

				// Token: 0x0400D72F RID: 55087
				public static LocString TOOLTIP = "This building is unable to maintain the integrity of the radbolts it is storing";
			}

			// Token: 0x02003279 RID: 12921
			public class TOP_PRIORITY_CHORE
			{
				// Token: 0x0400D730 RID: 55088
				public static LocString NAME = "Top Priority";

				// Token: 0x0400D731 RID: 55089
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This errand has been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					"\n\nThe colony will be in ",
					UI.PRE_KEYWORD,
					"Yellow Alert",
					UI.PST_KEYWORD,
					" until this task is completed"
				});

				// Token: 0x0400D732 RID: 55090
				public static LocString NOTIFICATION_NAME = "Yellow Alert";

				// Token: 0x0400D733 RID: 55091
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The following errands have been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x0200327A RID: 12922
			public class HOTTUBWATERTOOCOLD
			{
				// Token: 0x0400D734 RID: 55092
				public static LocString NAME = "Water Too Cold";

				// Token: 0x0400D735 RID: 55093
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" is below <b>{temperature}</b>\n\nIt is draining so it can be refilled with warmer ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200327B RID: 12923
			public class HOTTUBTOOHOT
			{
				// Token: 0x0400D736 RID: 55094
				public static LocString NAME = "Building Too Hot";

				// Token: 0x0400D737 RID: 55095
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{temperature}</b>\n\nIt needs to cool before it can safely be used"
				});
			}

			// Token: 0x0200327C RID: 12924
			public class HOTTUBFILLING
			{
				// Token: 0x0400D738 RID: 55096
				public static LocString NAME = "Filling Up: ({fullness})";

				// Token: 0x0400D739 RID: 55097
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub is currently filling with ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					"\n\nIt will be available to use when the ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" level reaches <b>100%</b>"
				});
			}

			// Token: 0x0200327D RID: 12925
			public class WINDTUNNELINTAKE
			{
				// Token: 0x0400D73A RID: 55098
				public static LocString NAME = "Intake Requires Gas";

				// Token: 0x0400D73B RID: 55099
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A wind tunnel requires ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" at the top and bottom intakes in order to operate\n\nThe intakes for this wind tunnel don't have enough gas to operate"
				});
			}

			// Token: 0x0200327E RID: 12926
			public class TEMPORAL_TEAR_OPENER_NO_TARGET
			{
				// Token: 0x0400D73C RID: 55100
				public static LocString NAME = "Temporal Tear not revealed";

				// Token: 0x0400D73D RID: 55101
				public static LocString TOOLTIP = "This machine is meant to target something in space, but the target has not yet been revealed";
			}

			// Token: 0x0200327F RID: 12927
			public class TEMPORAL_TEAR_OPENER_NO_LOS
			{
				// Token: 0x0400D73E RID: 55102
				public static LocString NAME = "Line of Sight: Obstructed";

				// Token: 0x0400D73F RID: 55103
				public static LocString TOOLTIP = "This device needs a clear view of space to operate";
			}

			// Token: 0x02003280 RID: 12928
			public class TEMPORAL_TEAR_OPENER_INSUFFICIENT_COLONIES
			{
				// Token: 0x0400D740 RID: 55104
				public static LocString NAME = "Too few Printing Pods {progress}";

				// Token: 0x0400D741 RID: 55105
				public static LocString TOOLTIP = "To open the Temporal Tear, this device relies on a network of activated Printing Pods {progress}";
			}

			// Token: 0x02003281 RID: 12929
			public class TEMPORAL_TEAR_OPENER_PROGRESS
			{
				// Token: 0x0400D742 RID: 55106
				public static LocString NAME = "Charging Progress: {progress}";

				// Token: 0x0400D743 RID: 55107
				public static LocString TOOLTIP = "This device must be charged with a high number of Radbolts\n\nOperation can commence once this device is fully charged";
			}

			// Token: 0x02003282 RID: 12930
			public class TEMPORAL_TEAR_OPENER_READY
			{
				// Token: 0x0400D744 RID: 55108
				public static LocString NOTIFICATION = "Temporal Tear Opener fully charged";

				// Token: 0x0400D745 RID: 55109
				public static LocString NOTIFICATION_TOOLTIP = "Push the red button to activate";
			}

			// Token: 0x02003283 RID: 12931
			public class WARPPORTALCHARGING
			{
				// Token: 0x0400D746 RID: 55110
				public static LocString NAME = "Recharging: {charge}";

				// Token: 0x0400D747 RID: 55111
				public static LocString TOOLTIP = "This teleporter will be ready for use in {cycles} cycles";
			}

			// Token: 0x02003284 RID: 12932
			public class WARPCONDUITPARTNERDISABLED
			{
				// Token: 0x0400D748 RID: 55112
				public static LocString NAME = "Teleporter Disabled ({x}/2)";

				// Token: 0x0400D749 RID: 55113
				public static LocString TOOLTIP = "This teleporter cannot be used until both the transmitting and receiving sides have been activated";
			}

			// Token: 0x02003285 RID: 12933
			public class COLLECTINGHEP
			{
				// Token: 0x0400D74A RID: 55114
				public static LocString NAME = "Collecting Radbolts ({x}/cycle)";

				// Token: 0x0400D74B RID: 55115
				public static LocString TOOLTIP = "Collecting Radbolts from ambient radiation";
			}

			// Token: 0x02003286 RID: 12934
			public class INORBIT
			{
				// Token: 0x0400D74C RID: 55116
				public static LocString NAME = "In Orbit: {Destination}";

				// Token: 0x0400D74D RID: 55117
				public static LocString TOOLTIP = "This rocket is currently in orbit around {Destination}";
			}

			// Token: 0x02003287 RID: 12935
			public class WAITINGTOLAND
			{
				// Token: 0x0400D74E RID: 55118
				public static LocString NAME = "Waiting to land on {Destination}";

				// Token: 0x0400D74F RID: 55119
				public static LocString TOOLTIP = "This rocket is waiting for an available Rcoket Platform on {Destination}";
			}

			// Token: 0x02003288 RID: 12936
			public class INFLIGHT
			{
				// Token: 0x0400D750 RID: 55120
				public static LocString NAME = "In Flight To {Destination_Asteroid}: {ETA}";

				// Token: 0x0400D751 RID: 55121
				public static LocString TOOLTIP = "This rocket is currently traveling to {Destination_Pad} on {Destination_Asteroid}\n\nIt will arrive in {ETA}";

				// Token: 0x0400D752 RID: 55122
				public static LocString TOOLTIP_NO_PAD = "This rocket is currently traveling to {Destination_Asteroid}\n\nIt will arrive in {ETA}";
			}

			// Token: 0x02003289 RID: 12937
			public class DESTINATIONOUTOFRANGE
			{
				// Token: 0x0400D753 RID: 55123
				public static LocString NAME = "Destination Out Of Range";

				// Token: 0x0400D754 RID: 55124
				public static LocString TOOLTIP = "This rocket lacks the range to reach its destination\n\nRocket Range: {Range}\nDestination Distance: {Distance}";
			}

			// Token: 0x0200328A RID: 12938
			public class ROCKETSTRANDED
			{
				// Token: 0x0400D755 RID: 55125
				public static LocString NAME = "Stranded";

				// Token: 0x0400D756 RID: 55126
				public static LocString TOOLTIP = "This rocket has run out of fuel and cannot move";
			}

			// Token: 0x0200328B RID: 12939
			public class SPACEPOIHARVESTING
			{
				// Token: 0x0400D757 RID: 55127
				public static LocString NAME = "Drilling {0}: {1}";

				// Token: 0x0400D758 RID: 55128
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Drillcone",
					UI.PST_KEYWORD,
					" is breaking up space debris into harvestable resources\n\nHarvestable resources can be retrieved by a a rocket equipped with a ",
					UI.PRE_KEYWORD,
					"Cargo Bay",
					UI.PST_KEYWORD,
					" module"
				});
			}

			// Token: 0x0200328C RID: 12940
			public class COLLECTINGHEXCELLINVENTORYITEMS
			{
				// Token: 0x0400D759 RID: 55129
				public static LocString NAME = "Harvesting Resources";

				// Token: 0x0400D75A RID: 55130
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"One or more ",
					UI.PRE_KEYWORD,
					"Cargo Bay",
					UI.PST_KEYWORD,
					" modules are collecting resources from this rocket's current hex cell"
				});
			}

			// Token: 0x0200328D RID: 12941
			public class RAILGUNPAYLOADNEEDSEMPTYING
			{
				// Token: 0x0400D75B RID: 55131
				public static LocString NAME = "Ready To Unpack";

				// Token: 0x0400D75C RID: 55132
				public static LocString TOOLTIP = "This payload has reached its destination and is ready to be unloaded\n\nIt can be marked for unpacking manually, or automatically unpacked on arrival using a " + BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME;
			}

			// Token: 0x0200328E RID: 12942
			public class MISSIONCONTROLASSISTINGROCKET
			{
				// Token: 0x0400D75D RID: 55133
				public static LocString NAME = "Guidance Signal: {0}";

				// Token: 0x0400D75E RID: 55134
				public static LocString TOOLTIP = "Once transmission is complete, Mission Control will boost targeted rocket's speed";
			}

			// Token: 0x0200328F RID: 12943
			public class MISSIONCONTROLBOOSTED
			{
				// Token: 0x0400D75F RID: 55135
				public static LocString NAME = "Mission Control Speed Boost: {0}";

				// Token: 0x0400D760 RID: 55136
				public static LocString TOOLTIP = "Mission Control has given this rocket a {0} speed boost\n\n{1} remaining";
			}

			// Token: 0x02003290 RID: 12944
			public class TRANSITTUBEENTRANCEWAXREADY
			{
				// Token: 0x0400D761 RID: 55137
				public static LocString NAME = "Smooth Ride Ready";

				// Token: 0x0400D762 RID: 55138
				public static LocString TOOLTIP = "This building is stocked with speed-boosting " + ELEMENTS.MILKFAT.NAME + "\n\n{0} per use ({1} remaining)";
			}

			// Token: 0x02003291 RID: 12945
			public class NOROCKETSTOMISSIONCONTROLBOOST
			{
				// Token: 0x0400D763 RID: 55139
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400D764 RID: 55140
				public static LocString TOOLTIP = "Rockets must be mid-flight and not targeted by another Mission Control Station, or already boosted";
			}

			// Token: 0x02003292 RID: 12946
			public class NOROCKETSTOMISSIONCONTROLCLUSTERBOOST
			{
				// Token: 0x0400D765 RID: 55141
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400D766 RID: 55142
				public static LocString TOOLTIP = "Rockets must be mid-flight, within {0} tiles, and not targeted by another Mission Control Station or already boosted";
			}

			// Token: 0x02003293 RID: 12947
			public class AWAITINGEMPTYBUILDING
			{
				// Token: 0x0400D767 RID: 55143
				public static LocString NAME = "Empty Errand";

				// Token: 0x0400D768 RID: 55144
				public static LocString TOOLTIP = "Building will be emptied once a Duplicant is available";
			}

			// Token: 0x02003294 RID: 12948
			public class DUPLICANTACTIVATIONREQUIRED
			{
				// Token: 0x0400D769 RID: 55145
				public static LocString NAME = "Activation Required";

				// Token: 0x0400D76A RID: 55146
				public static LocString TOOLTIP = "A Duplicant is required to bring this building online";
			}

			// Token: 0x02003295 RID: 12949
			public class PILOTNEEDED
			{
				// Token: 0x0400D76B RID: 55147
				public static LocString NAME = "Switching to Autopilot";

				// Token: 0x0400D76C RID: 55148
				public static LocString TOOLTIP = "Autopilot will engage in {timeRemaining} if a Duplicant pilot does not assume control";
			}

			// Token: 0x02003296 RID: 12950
			public class AUTOPILOTACTIVE
			{
				// Token: 0x0400D76D RID: 55149
				public static LocString NAME = "Autopilot Engaged";

				// Token: 0x0400D76E RID: 55150
				public static LocString TOOLTIP = "This rocket has entered autopilot mode and will fly at reduced speed\n\nIt can resume full speed once a Duplicant pilot takes over";
			}

			// Token: 0x02003297 RID: 12951
			public class INFLIGHTPILOTED
			{
				// Token: 0x0400D76F RID: 55151
				public static LocString NAME = "Piloted";

				// Token: 0x0400D770 RID: 55152
				public static LocString DUPE_TOOLTIP = "Duplicant pilot's <b>Skill</b>: +{0} speed boost";

				// Token: 0x0400D771 RID: 55153
				public static LocString ROBO_TOOLTIP = "Piloted by a " + UI.PRE_KEYWORD + "Robo-Pilot" + UI.PST_KEYWORD;
			}

			// Token: 0x02003298 RID: 12952
			public class INFLIGHTUNPILOTED
			{
				// Token: 0x0400D772 RID: 55154
				public static LocString NAME = "Unpiloted";

				// Token: 0x0400D773 RID: 55155
				public static LocString TOOLTIP = "Inactive rocket module: -{penalty} speed {modules}";

				// Token: 0x0400D774 RID: 55156
				public static LocString ROBO_PILOT_ONLY_TOOLTIP = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Robo-Pilot",
					UI.PST_KEYWORD,
					" has run out of ",
					UI.PRE_KEYWORD,
					"Data Banks",
					UI.PST_KEYWORD,
					"\n\nThis rocket is stranded"
				});
			}

			// Token: 0x02003299 RID: 12953
			public class INFLIGHTAUTOPILOTED
			{
				// Token: 0x0400D775 RID: 55157
				public static LocString NAME = "Autopilot Engaged";

				// Token: 0x0400D776 RID: 55158
				public static LocString TOOLTIP = "This rocket's {modules} is inactive\n\nThis rocket has entered autopilot mode and will fly at reduced speed\n    •  -{penalty} speed";
			}

			// Token: 0x0200329A RID: 12954
			public class INFLIGHTSUPERPILOT
			{
				// Token: 0x0400D777 RID: 55159
				public static LocString NAME = "Multi-Piloted";

				// Token: 0x0400D778 RID: 55160
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This rocket is being piloted by a Duplicant and a ",
					UI.PRE_KEYWORD,
					"Robo-Pilot",
					UI.PST_KEYWORD,
					"\n    • Multi-Piloted: +{1} speed boost\n    • Duplicant pilot skill: +{0} speed boost"
				});
			}

			// Token: 0x0200329B RID: 12955
			public class ROCKETCHECKLISTINCOMPLETE
			{
				// Token: 0x0400D779 RID: 55161
				public static LocString NAME = "Launch Checklist Incomplete";

				// Token: 0x0400D77A RID: 55162
				public static LocString TOOLTIP = "Critical launch tasks uncompleted\n\nRefer to the Launch Checklist in the status panel";
			}

			// Token: 0x0200329C RID: 12956
			public class ROCKETCARGOEMPTYING
			{
				// Token: 0x0400D77B RID: 55163
				public static LocString NAME = "Unloading Cargo";

				// Token: 0x0400D77C RID: 55164
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rocket cargo is being unloaded into the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nLoading of new cargo will begin once unloading is complete"
				});
			}

			// Token: 0x0200329D RID: 12957
			public class ROCKETCARGOFILLING
			{
				// Token: 0x0400D77D RID: 55165
				public static LocString NAME = "Loading Cargo";

				// Token: 0x0400D77E RID: 55166
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Cargo is being loaded onto the rocket from the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nRocket cargo will be ready for launch once loading is complete"
				});
			}

			// Token: 0x0200329E RID: 12958
			public class ROCKETCARGOFULL
			{
				// Token: 0x0400D77F RID: 55167
				public static LocString NAME = "Platform Ready";

				// Token: 0x0400D780 RID: 55168
				public static LocString TOOLTIP = "All cargo operations are complete";
			}

			// Token: 0x0200329F RID: 12959
			public class FLIGHTALLCARGOFULL
			{
				// Token: 0x0400D781 RID: 55169
				public static LocString NAME = "All cargo bays are full";

				// Token: 0x0400D782 RID: 55170
				public static LocString TOOLTIP = "Rocket cannot store any more materials";
			}

			// Token: 0x020032A0 RID: 12960
			public class FLIGHTCARGOREMAINING
			{
				// Token: 0x0400D783 RID: 55171
				public static LocString NAME = "Cargo capacity remaining: {0}";

				// Token: 0x0400D784 RID: 55172
				public static LocString TOOLTIP = "Rocket can store up to {0} more materials";
			}

			// Token: 0x020032A1 RID: 12961
			public class ROCKET_PORT_IDLE
			{
				// Token: 0x0400D785 RID: 55173
				public static LocString NAME = "Idle";

				// Token: 0x0400D786 RID: 55174
				public static LocString TOOLTIP = "This port is idle because there is no rocket on the connected " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A2 RID: 12962
			public class ROCKET_PORT_UNLOADING
			{
				// Token: 0x0400D787 RID: 55175
				public static LocString NAME = "Unloading Rocket";

				// Token: 0x0400D788 RID: 55176
				public static LocString TOOLTIP = "Resources are being unloaded from the rocket into the local network";
			}

			// Token: 0x020032A3 RID: 12963
			public class ROCKET_PORT_LOADING
			{
				// Token: 0x0400D789 RID: 55177
				public static LocString NAME = "Loading Rocket";

				// Token: 0x0400D78A RID: 55178
				public static LocString TOOLTIP = "Resources are being loaded from the local network into the rocket's storage";
			}

			// Token: 0x020032A4 RID: 12964
			public class ROCKET_PORT_LOADED
			{
				// Token: 0x0400D78B RID: 55179
				public static LocString NAME = "Cargo Transfer Complete";

				// Token: 0x0400D78C RID: 55180
				public static LocString TOOLTIP = "The connected rocket has either reached max capacity for this resource type, or lacks appropriate storage modules";
			}

			// Token: 0x020032A5 RID: 12965
			public class CONNECTED_ROCKET_PORT
			{
				// Token: 0x0400D78D RID: 55181
				public static LocString NAME = "Port Network Attached";

				// Token: 0x0400D78E RID: 55182
				public static LocString TOOLTIP = "This module has been connected to a " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " and can now load and unload cargo";
			}

			// Token: 0x020032A6 RID: 12966
			public class CONNECTED_ROCKET_WRONG_PORT
			{
				// Token: 0x0400D78F RID: 55183
				public static LocString NAME = "Incorrect Port Network";

				// Token: 0x0400D790 RID: 55184
				public static LocString TOOLTIP = "The attached " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " is not the correct type for this cargo module";
			}

			// Token: 0x020032A7 RID: 12967
			public class CONNECTED_ROCKET_NO_PORT
			{
				// Token: 0x0400D791 RID: 55185
				public static LocString NAME = "No Rocket Ports";

				// Token: 0x0400D792 RID: 55186
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					" has no ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					" attached\n\n",
					UI.PRE_KEYWORD,
					"Solid",
					UI.PST_KEYWORD,
					", ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					", and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME_PLURAL,
					" can be attached to load and unload cargo from a landed rocket's modules"
				});
			}

			// Token: 0x020032A8 RID: 12968
			public class CLUSTERTELESCOPEALLWORKCOMPLETE
			{
				// Token: 0x0400D793 RID: 55187
				public static LocString NAME = "Area Complete";

				// Token: 0x0400D794 RID: 55188
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Telescope",
					UI.PST_KEYWORD,
					" has analyzed all the space visible from its current location"
				});
			}

			// Token: 0x020032A9 RID: 12969
			public class ROCKETPLATFORMCLOSETOCEILING
			{
				// Token: 0x0400D795 RID: 55189
				public static LocString NAME = "Low Clearance: {distance} Tiles";

				// Token: 0x0400D796 RID: 55190
				public static LocString TOOLTIP = "Tall rockets may not be able to land on this " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x020032AA RID: 12970
			public class MODULEGENERATORNOTPOWERED
			{
				// Token: 0x0400D797 RID: 55191
				public static LocString NAME = "In-Flight Generator: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400D798 RID: 55192
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine will generate ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" while traveling through space\n\nWhen thruster is idle, no ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" is generated"
				});
			}

			// Token: 0x020032AB RID: 12971
			public class MODULEGENERATORPOWERED
			{
				// Token: 0x0400D799 RID: 55193
				public static LocString NAME = "In-Flight Generator: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400D79A RID: 55194
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine is extracting ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from the thruster\n\nIt will continue generating power as long as it travels through space"
				});
			}

			// Token: 0x020032AC RID: 12972
			public class INORBITREQUIRED
			{
				// Token: 0x0400D79B RID: 55195
				public static LocString NAME = "Grounded";

				// Token: 0x0400D79C RID: 55196
				public static LocString TOOLTIP = "This building cannot operate from the surface of a " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " and must be in space to function";
			}

			// Token: 0x020032AD RID: 12973
			public class REACTORREFUELDISABLED
			{
				// Token: 0x0400D79D RID: 55197
				public static LocString NAME = "Refuel Disabled";

				// Token: 0x0400D79E RID: 55198
				public static LocString TOOLTIP = "This building will not be refueled once its active fuel has been consumed";
			}

			// Token: 0x020032AE RID: 12974
			public class RAILGUNCOOLDOWN
			{
				// Token: 0x0400D79F RID: 55199
				public static LocString NAME = "Cleaning Rails: {timeleft}";

				// Token: 0x0400D7A0 RID: 55200
				public static LocString TOOLTIP = "This building automatically performs routine maintenance every {x} launches";
			}

			// Token: 0x020032AF RID: 12975
			public class FRIDGECOOLING
			{
				// Token: 0x0400D7A1 RID: 55201
				public static LocString NAME = "Cooling Contents: {UsedPower}";

				// Token: 0x0400D7A2 RID: 55202
				public static LocString TOOLTIP = "{UsedPower} of {MaxPower} are being used to cool the contents of this food storage";
			}

			// Token: 0x020032B0 RID: 12976
			public class FRIDGESTEADY
			{
				// Token: 0x0400D7A3 RID: 55203
				public static LocString NAME = "Energy Saver: {UsedPower}";

				// Token: 0x0400D7A4 RID: 55204
				public static LocString TOOLTIP = "The contents of this food storage are at refrigeration temperatures\n\nEnergy Saver mode has been automatically activated using only {UsedPower} of {MaxPower}";
			}

			// Token: 0x020032B1 RID: 12977
			public class TELEPHONE
			{
				// Token: 0x02003CEC RID: 15596
				public class BABBLE
				{
					// Token: 0x0400F160 RID: 61792
					public static LocString NAME = "Babbling to no one.";

					// Token: 0x0400F161 RID: 61793
					public static LocString TOOLTIP = "{Duplicant} just needed to vent to into the void.";
				}

				// Token: 0x02003CED RID: 15597
				public class CONVERSATION
				{
					// Token: 0x0400F162 RID: 61794
					public static LocString TALKING_TO = "Talking to {Duplicant} on {Asteroid}";

					// Token: 0x0400F163 RID: 61795
					public static LocString TALKING_TO_NUM = "Talking to {0} friends.";
				}
			}

			// Token: 0x020032B2 RID: 12978
			public class CREATUREMANIPULATORPROGRESS
			{
				// Token: 0x0400D7A5 RID: 55205
				public static LocString NAME = "Collected Species Data {0}/{1}";

				// Token: 0x0400D7A6 RID: 55206
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires data from multiple ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" species to unlock its genetic manipulator\n\nSpecies scanned:"
				});

				// Token: 0x0400D7A7 RID: 55207
				public static LocString NO_DATA = "No species scanned";
			}

			// Token: 0x020032B3 RID: 12979
			public class CREATUREMANIPULATORMORPHMODELOCKED
			{
				// Token: 0x0400D7A8 RID: 55208
				public static LocString NAME = "Current Status: Offline";

				// Token: 0x0400D7A9 RID: 55209
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot operate until it collects more ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x020032B4 RID: 12980
			public class CREATUREMANIPULATORMORPHMODE
			{
				// Token: 0x0400D7AA RID: 55210
				public static LocString NAME = "Current Status: Online";

				// Token: 0x0400D7AB RID: 55211
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is ready to manipulate ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x020032B5 RID: 12981
			public class CREATUREMANIPULATORWAITING
			{
				// Token: 0x0400D7AC RID: 55212
				public static LocString NAME = "Waiting for a Critter";

				// Token: 0x0400D7AD RID: 55213
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is waiting for a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to get sucked into its scanning area"
				});
			}

			// Token: 0x020032B6 RID: 12982
			public class CREATUREMANIPULATORWORKING
			{
				// Token: 0x0400D7AE RID: 55214
				public static LocString NAME = "Poking and Prodding Critter";

				// Token: 0x0400D7AF RID: 55215
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is extracting genetic information from a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x020032B7 RID: 12983
			public class SPICEGRINDERNOSPICE
			{
				// Token: 0x0400D7B0 RID: 55216
				public static LocString NAME = "No Spice Selected";

				// Token: 0x0400D7B1 RID: 55217
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x020032B8 RID: 12984
			public class SPICEGRINDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400D7B2 RID: 55218
				public static LocString NAME = "Spice Grinder accepts mutant seeds";

				// Token: 0x0400D7B3 RID: 55219
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This spice grinder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x020032B9 RID: 12985
			public class MISSILELAUNCHER_NOSURFACESIGHT
			{
				// Token: 0x0400D7B4 RID: 55220
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400D7B5 RID: 55221
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x020032BA RID: 12986
			public class MISSILELAUNCHER_PARTIALLYBLOCKED
			{
				// Token: 0x0400D7B6 RID: 55222
				public static LocString NAME = "Limited Line of Sight";

				// Token: 0x0400D7B7 RID: 55223
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x020032BB RID: 12987
			public class MISSILELAUNCHER_LONGRANGECOOLDOWN
			{
				// Token: 0x0400D7B8 RID: 55224
				public static LocString NAME = "Reloading";

				// Token: 0x0400D7B9 RID: 55225
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building recently fired an ",
					UI.PRE_KEYWORD,
					"Intracosmic Blastshot",
					UI.PST_KEYWORD,
					" and needs a moment to reload"
				});
			}

			// Token: 0x020032BC RID: 12988
			public class COMPLEXFABRICATOR
			{
				// Token: 0x02003CEE RID: 15598
				public class COOKING
				{
					// Token: 0x0400F164 RID: 61796
					public static LocString NAME = "Cooking {Item}";

					// Token: 0x0400F165 RID: 61797
					public static LocString TOOLTIP = "This building is currently whipping up a batch of {Item}";
				}

				// Token: 0x02003CEF RID: 15599
				public class PRODUCING
				{
					// Token: 0x0400F166 RID: 61798
					public static LocString NAME = "Producing {Item}";

					// Token: 0x0400F167 RID: 61799
					public static LocString TOOLTIP = "This building is carrying out its current production orders";
				}

				// Token: 0x02003CF0 RID: 15600
				public class RESEARCHING
				{
					// Token: 0x0400F168 RID: 61800
					public static LocString NAME = "Researching {Item}";

					// Token: 0x0400F169 RID: 61801
					public static LocString TOOLTIP = "This building is currently conducting important research";
				}

				// Token: 0x02003CF1 RID: 15601
				public class ANALYZING
				{
					// Token: 0x0400F16A RID: 61802
					public static LocString NAME = "Analyzing {Item}";

					// Token: 0x0400F16B RID: 61803
					public static LocString TOOLTIP = "This building is currently analyzing a fascinating artifact";
				}

				// Token: 0x02003CF2 RID: 15602
				public class UNTRAINING
				{
					// Token: 0x0400F16C RID: 61804
					public static LocString NAME = "Untraining {Duplicant}";

					// Token: 0x0400F16D RID: 61805
					public static LocString TOOLTIP = "Restoring {Duplicant} to a blissfully ignorant state";
				}

				// Token: 0x02003CF3 RID: 15603
				public class TELESCOPE
				{
					// Token: 0x0400F16E RID: 61806
					public static LocString NAME = "Studying Space";

					// Token: 0x0400F16F RID: 61807
					public static LocString TOOLTIP = "This building is currently investigating the mysteries of space";
				}

				// Token: 0x02003CF4 RID: 15604
				public class CLUSTERTELESCOPEMETEOR
				{
					// Token: 0x0400F170 RID: 61808
					public static LocString NAME = "Studying Meteor";

					// Token: 0x0400F171 RID: 61809
					public static LocString TOOLTIP = "This building is currently studying a meteor";
				}
			}

			// Token: 0x020032BD RID: 12989
			public class REMOTEWORKERDEPOT
			{
				// Token: 0x02003CF5 RID: 15605
				public class MAKINGWORKER
				{
					// Token: 0x0400F172 RID: 61810
					public static LocString NAME = "Assembling Remote Worker";

					// Token: 0x0400F173 RID: 61811
					public static LocString TOOLTIP = "This building is currently assembling a remote worker drone";
				}
			}

			// Token: 0x020032BE RID: 12990
			public class REMOTEWORKTERMINAL
			{
				// Token: 0x02003CF6 RID: 15606
				public class NODOCK
				{
					// Token: 0x0400F174 RID: 61812
					public static LocString NAME = "No Dock Assigned";

					// Token: 0x0400F175 RID: 61813
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This building must be assigned a ",
						UI.PRE_KEYWORD,
						"Remote Worker Dock",
						UI.PST_KEYWORD,
						" in order to function"
					});
				}
			}

			// Token: 0x020032BF RID: 12991
			public class DATAMINER
			{
				// Token: 0x02003CF7 RID: 15607
				public class PRODUCTIONRATE
				{
					// Token: 0x0400F176 RID: 61814
					public static LocString NAME = "Production Rate: {RATE}";

					// Token: 0x0400F177 RID: 61815
					public static LocString TOOLTIP = "This building is operating at {RATE} of its maximum speed\n\nProduction rate decreases at higher temperatures\n\nCurrent ambient temperature: {TEMP}";
				}
			}
		}

		// Token: 0x02002564 RID: 9572
		public class DETAILS
		{
			// Token: 0x0400A8B9 RID: 43193
			public static LocString USE_COUNT = "Uses: {0}";

			// Token: 0x0400A8BA RID: 43194
			public static LocString USE_COUNT_TOOLTIP = "This building has been used {0} times";
		}
	}
}
