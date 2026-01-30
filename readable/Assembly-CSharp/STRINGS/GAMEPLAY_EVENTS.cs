using System;

namespace STRINGS
{
	// Token: 0x02000FF2 RID: 4082
	public class GAMEPLAY_EVENTS
	{
		// Token: 0x04006031 RID: 24625
		public static LocString CANCELED = "{0} Canceled";

		// Token: 0x04006032 RID: 24626
		public static LocString CANCELED_TOOLTIP = "The {0} event was canceled";

		// Token: 0x04006033 RID: 24627
		public static LocString DEFAULT_OPTION_NAME = "OK";

		// Token: 0x04006034 RID: 24628
		public static LocString DEFAULT_OPTION_CONSIDER_NAME = "Let me think about it";

		// Token: 0x04006035 RID: 24629
		public static LocString CHAIN_EVENT_TOOLTIP = "This event is a chain event";

		// Token: 0x04006036 RID: 24630
		public static LocString BONUS_EVENT_DESCRIPTION = "{effects} for {duration}";

		// Token: 0x0200255A RID: 9562
		public class LOCATIONS
		{
			// Token: 0x0400A8A9 RID: 43177
			public static LocString NONE_AVAILABLE = "No location currently available";

			// Token: 0x0400A8AA RID: 43178
			public static LocString SUN = "The Sun";

			// Token: 0x0400A8AB RID: 43179
			public static LocString SURFACE = "Planetary Surface";

			// Token: 0x0400A8AC RID: 43180
			public static LocString PRINTING_POD = BUILDINGS.PREFABS.HEADQUARTERS.NAME;

			// Token: 0x0400A8AD RID: 43181
			public static LocString COLONY_WIDE = "Colonywide";
		}

		// Token: 0x0200255B RID: 9563
		public class TIMES
		{
			// Token: 0x0400A8AE RID: 43182
			public static LocString NOW = "Right now";

			// Token: 0x0400A8AF RID: 43183
			public static LocString IN_CYCLES = "In {0} cycles";

			// Token: 0x0400A8B0 RID: 43184
			public static LocString UNKNOWN = "Sometime";
		}

		// Token: 0x0200255C RID: 9564
		public class EVENT_TYPES
		{
			// Token: 0x020030D3 RID: 12499
			public class PARTY
			{
				// Token: 0x0400D34D RID: 54093
				public static LocString NAME = "Party";

				// Token: 0x0400D34E RID: 54094
				public static LocString DESCRIPTION = "THIS EVENT IS NOT WORKING\n{host} is throwing a birthday party for {dupe}. Make sure there is an available " + ROOMS.TYPES.REC_ROOM.NAME + " for the party.\n\nSocial events are good for Duplicant morale. Rejecting this party will hurt {host} and {dupe}'s fragile ego.";

				// Token: 0x0400D34F RID: 54095
				public static LocString CANCELED_NO_ROOM_TITLE = "Party Canceled";

				// Token: 0x0400D350 RID: 54096
				public static LocString CANCELED_NO_ROOM_DESCRIPTION = "The party was canceled because no " + ROOMS.TYPES.REC_ROOM.NAME + " was available.";

				// Token: 0x0400D351 RID: 54097
				public static LocString UNDERWAY = "Party Happening";

				// Token: 0x0400D352 RID: 54098
				public static LocString UNDERWAY_TOOLTIP = "There's a party going on";

				// Token: 0x0400D353 RID: 54099
				public static LocString ACCEPT_OPTION_NAME = "Allow the party to happen";

				// Token: 0x0400D354 RID: 54100
				public static LocString ACCEPT_OPTION_DESC = "Party goers will get {goodEffect}";

				// Token: 0x0400D355 RID: 54101
				public static LocString ACCEPT_OPTION_INVALID_TOOLTIP = "A cake must be built for this event to take place.";

				// Token: 0x0400D356 RID: 54102
				public static LocString REJECT_OPTION_NAME = "Cancel the party";

				// Token: 0x0400D357 RID: 54103
				public static LocString REJECT_OPTION_DESC = "{host} and {dupe} gain {badEffect}";
			}

			// Token: 0x020030D4 RID: 12500
			public class ECLIPSE
			{
				// Token: 0x0400D358 RID: 54104
				public static LocString NAME = "Eclipse";

				// Token: 0x0400D359 RID: 54105
				public static LocString DESCRIPTION = "A celestial object has obscured the sunlight";
			}

			// Token: 0x020030D5 RID: 12501
			public class SOLAR_FLARE
			{
				// Token: 0x0400D35A RID: 54106
				public static LocString NAME = "Solar Storm";

				// Token: 0x0400D35B RID: 54107
				public static LocString DESCRIPTION = "A solar flare is headed this way";
			}

			// Token: 0x020030D6 RID: 12502
			public class CREATURE_SPAWN
			{
				// Token: 0x0400D35C RID: 54108
				public static LocString NAME = "Critter Infestation";

				// Token: 0x0400D35D RID: 54109
				public static LocString DESCRIPTION = "There was a massive influx of destructive critters";
			}

			// Token: 0x020030D7 RID: 12503
			public class SATELLITE_CRASH
			{
				// Token: 0x0400D35E RID: 54110
				public static LocString NAME = "Satellite Crash";

				// Token: 0x0400D35F RID: 54111
				public static LocString DESCRIPTION = "Mysterious space junk has crashed into the surface.\n\nIt may contain useful resources or information, but it may also be dangerous. Approach with caution.";
			}

			// Token: 0x020030D8 RID: 12504
			public class FOOD_FIGHT
			{
				// Token: 0x0400D360 RID: 54112
				public static LocString NAME = "Food Fight";

				// Token: 0x0400D361 RID: 54113
				public static LocString DESCRIPTION = "Duplicants will throw food at each other for recreation\n\nIt may be wasteful, but everyone who participates will benefit from a major stress reduction.";

				// Token: 0x0400D362 RID: 54114
				public static LocString UNDERWAY = "Food Fight";

				// Token: 0x0400D363 RID: 54115
				public static LocString UNDERWAY_TOOLTIP = "There is a food fight happening now";

				// Token: 0x0400D364 RID: 54116
				public static LocString ACCEPT_OPTION_NAME = "Duplicants start preparing to fight.";

				// Token: 0x0400D365 RID: 54117
				public static LocString ACCEPT_OPTION_DETAILS = "(Plus morale)";

				// Token: 0x0400D366 RID: 54118
				public static LocString REJECT_OPTION_NAME = "No food fight today";

				// Token: 0x0400D367 RID: 54119
				public static LocString REJECT_OPTION_DETAILS = "Sadface";
			}

			// Token: 0x020030D9 RID: 12505
			public class PLANT_BLIGHT
			{
				// Token: 0x0400D368 RID: 54120
				public static LocString NAME = "Plant Blight: {plant}";

				// Token: 0x0400D369 RID: 54121
				public static LocString DESCRIPTION = "Our {plant} crops have been afflicted by a fungal sickness!\n\nI must get the Duplicants to uproot and compost the sick plants to save our farms.";

				// Token: 0x0400D36A RID: 54122
				public static LocString SUCCESS = "Blight Managed: {plant}";

				// Token: 0x0400D36B RID: 54123
				public static LocString SUCCESS_TOOLTIP = "All the blighted {plant} plants have been dealt with, halting the infection.";
			}

			// Token: 0x020030DA RID: 12506
			public class CRYOFRIEND
			{
				// Token: 0x0400D36C RID: 54124
				public static LocString NAME = "New Event: A Frozen Friend";

				// Token: 0x0400D36D RID: 54125
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} has made an amazing discovery! A barely working ",
					BUILDINGS.PREFABS.CRYOTANK.NAME,
					" has been uncovered containing a {friend} inside in a frozen state.\n\n{dupe} was successful in thawing {friend} and this encounter has filled both Duplicants with a sense of hope, something they will desperately need to keep their ",
					UI.FormatAsLink("Morale", "MORALE"),
					" up when facing the dangers ahead."
				});

				// Token: 0x0400D36E RID: 54126
				public static LocString BUTTON = "{friend} is thawed!";
			}

			// Token: 0x020030DB RID: 12507
			public class WARPWORLDREVEAL
			{
				// Token: 0x0400D36F RID: 54127
				public static LocString NAME = "New Event: Personnel Teleporter";

				// Token: 0x0400D370 RID: 54128
				public static LocString DESCRIPTION = "I've discovered a functioning teleportation device with a pre-programmed destination.\n\nIt appears to go to another " + UI.CLUSTERMAP.PLANETOID + ", and I'm fairly certain there's a return device on the other end.\n\nI could send a Duplicant through safely if I desired.";

				// Token: 0x0400D371 RID: 54129
				public static LocString BUTTON = "See Destination";
			}

			// Token: 0x020030DC RID: 12508
			public class ARTIFACT_REVEAL
			{
				// Token: 0x0400D372 RID: 54130
				public static LocString NAME = "New Event: Artifact Analyzed";

				// Token: 0x0400D373 RID: 54131
				public static LocString DESCRIPTION = "An artifact from a past civilization was analyzed.\n\n{desc}";

				// Token: 0x0400D374 RID: 54132
				public static LocString BUTTON = "Close";
			}
		}

		// Token: 0x0200255D RID: 9565
		public class BONUS
		{
			// Token: 0x020030DD RID: 12509
			public class BONUSDREAM1
			{
				// Token: 0x0400D375 RID: 54133
				public static LocString NAME = "Good Dream";

				// Token: 0x0400D376 RID: 54134
				public static LocString DESCRIPTION = "I've observed many improvements to {dupe}'s demeanor today. Analysis indicates unusually high amounts of dopamine in their system. There's a good chance this is due to an exceptionally good dream and analysis indicates that current sleeping conditions may have contributed to this occurrence.\n\nFurther improvements to sleeping conditions may have additional positive effects to the " + UI.FormatAsLink("Morale", "MORALE") + " of {dupe} and other Duplicants.";

				// Token: 0x0400D377 RID: 54135
				public static LocString CHAIN_TOOLTIP = "Improving the living conditions of {dupe} will lead to more good dreams.";
			}

			// Token: 0x020030DE RID: 12510
			public class BONUSDREAM2
			{
				// Token: 0x0400D378 RID: 54136
				public static LocString NAME = "Really Good Dream";

				// Token: 0x0400D379 RID: 54137
				public static LocString DESCRIPTION = "{dupe} had another really good dream and the resulting release of dopamine has made this Duplicant energetic and full of possibilities! This is an encouraging byproduct of improving the living conditions of the colony.\n\nBased on these observations, building a better sleeping area for my Duplicants will have a similar effect on their " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020030DF RID: 12511
			public class BONUSDREAM3
			{
				// Token: 0x0400D37A RID: 54138
				public static LocString NAME = "Great Dream";

				// Token: 0x0400D37B RID: 54139
				public static LocString DESCRIPTION = "I have detected a distinct spring in {dupe}'s step today. There is a good chance that this Duplicant had another great dream last night. Such incidents are further indications that working on the care and comfort of the colony is not a waste of time.\n\nI do wonder though: What do Duplicants dream of?";
			}

			// Token: 0x020030E0 RID: 12512
			public class BONUSDREAM4
			{
				// Token: 0x0400D37C RID: 54140
				public static LocString NAME = "Amazing Dream";

				// Token: 0x0400D37D RID: 54141
				public static LocString DESCRIPTION = "{dupe}'s dream last night must have been simply amazing! Their dopamine levels are at an all time high. Based on these results, it can be safely assumed that improving the living conditions of my Duplicants will reduce " + UI.FormatAsLink("Stress", "STRESS") + " and have similar positive effects on their well-being.\n\nObservations such as this are an integral and enjoyable part of science. When I see my Duplicants happy, I can't help but share in some of their joy.";
			}

			// Token: 0x020030E1 RID: 12513
			public class BONUSTOILET1
			{
				// Token: 0x0400D37E RID: 54142
				public static LocString NAME = "Small Comforts";

				// Token: 0x0400D37F RID: 54143
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} recently visited an Outhouse and appears to have appreciated the small comforts based on the marked increase to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nHigh ",
					UI.FormatAsLink("Morale", "MORALE"),
					" has been linked to a better work ethic and greater enthusiasm for complex jobs, which are essential in building a successful new colony."
				});
			}

			// Token: 0x020030E2 RID: 12514
			public class BONUSTOILET2
			{
				// Token: 0x0400D380 RID: 54144
				public static LocString NAME = "Greater Comforts";

				// Token: 0x0400D381 RID: 54145
				public static LocString DESCRIPTION = "{dupe} used a Lavatory and analysis shows a decided improvement to this Duplicant's " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nAs my colony grows and expands, it's important not to ignore the benefits of giving my Duplicants a pleasant place to relieve themselves.";
			}

			// Token: 0x020030E3 RID: 12515
			public class BONUSTOILET3
			{
				// Token: 0x0400D382 RID: 54146
				public static LocString NAME = "Small Luxury";

				// Token: 0x0400D383 RID: 54147
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} visited a ",
					ROOMS.TYPES.LATRINE.NAME,
					" and experienced luxury unlike they anything this Duplicant had previously experienced as analysis has revealed yet another boost to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nIt is unclear whether this development is a result of increased hygiene or whether there is something else inherently about working plumbing which would improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" in this way. Further analysis is needed."
				});
			}

			// Token: 0x020030E4 RID: 12516
			public class BONUSTOILET4
			{
				// Token: 0x0400D384 RID: 54148
				public static LocString NAME = "Greater Luxury";

				// Token: 0x0400D385 RID: 54149
				public static LocString DESCRIPTION = "{dupe} visited a Washroom and the experience has left this Duplicant with significantly improved " + UI.FormatAsLink("Morale", "MORALE") + ". Analysis indicates this improvement should continue for many cycles.\n\nThe relationship of my Duplicants and their surroundings is an interesting aspect of colony life. I should continue to watch future developments in this department closely.";
			}

			// Token: 0x020030E5 RID: 12517
			public class BONUSRESEARCH
			{
				// Token: 0x0400D386 RID: 54150
				public static LocString NAME = "Inspired Learner";

				// Token: 0x0400D387 RID: 54151
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Analysis indicates that the appearance of a ",
					UI.PRE_KEYWORD,
					"Research Station",
					UI.PST_KEYWORD,
					" has inspired {dupe} and heightened their brain activity on a cellular level.\n\nBrain stimulation is important if my Duplicants are going to adapt and innovate in their increasingly harsh environment."
				});
			}

			// Token: 0x020030E6 RID: 12518
			public class BONUSDIGGING1
			{
				// Token: 0x0400D388 RID: 54152
				public static LocString NAME = "Hot Diggity!";

				// Token: 0x0400D389 RID: 54153
				public static LocString DESCRIPTION = "Some interesting data has revealed that {dupe} has had a marked increase in physical abilities, an increase that cannot entirely be attributed to the usual improvements that occur after regular physical activity.\n\nBased on previous observations this Duplicant's positive associations with digging appear to account for this additional physical boost.\n\nThis would mean the personal preferences of my Duplicants are directly correlated to how hard they work. How interesting...";
			}

			// Token: 0x020030E7 RID: 12519
			public class BONUSSTORAGE
			{
				// Token: 0x0400D38A RID: 54154
				public static LocString NAME = "Something in Store";

				// Token: 0x0400D38B RID: 54155
				public static LocString DESCRIPTION = "Data indicates that {dupe}'s activity in storing something in a Storage Bin has led to an increase in this Duplicant's physical strength as well as an overall improvement to their general demeanor.\n\nThere have been many studies connecting organization with an increase in well-being. It is possible this explains {dupe}'s " + UI.FormatAsLink("Morale", "MORALE") + " improvements.";
			}

			// Token: 0x020030E8 RID: 12520
			public class BONUSBUILDER
			{
				// Token: 0x0400D38C RID: 54156
				public static LocString NAME = "Accomplished Builder";

				// Token: 0x0400D38D RID: 54157
				public static LocString DESCRIPTION = "{dupe} has been hard at work building many structures crucial to the future of the colony. It seems this activity has improved this Duplicant's budding construction and mechanical skills beyond what my models predicted.\n\nWhether this increase in ability is due to them learning new skills or simply gaining self-confidence I cannot say, but this unexpected development is a welcome surprise development.";
			}

			// Token: 0x020030E9 RID: 12521
			public class BONUSOXYGEN
			{
				// Token: 0x0400D38E RID: 54158
				public static LocString NAME = "Fresh Air";

				// Token: 0x0400D38F RID: 54159
				public static LocString DESCRIPTION = "{dupe} is experiencing a sudden unexpected improvement to their physical prowess which appears to be a result of exposure to elevated levels of oxygen from passing by an Oxygen Diffuser.\n\nObservations such as this are important in documenting just how beneficial having access to oxygen is to my colony.";
			}

			// Token: 0x020030EA RID: 12522
			public class BONUSALGAE
			{
				// Token: 0x0400D390 RID: 54160
				public static LocString NAME = "Fresh Algae Smell";

				// Token: 0x0400D391 RID: 54161
				public static LocString DESCRIPTION = "{dupe}'s recent proximity to an Algae Terrarium has left them feeling refreshed and exuberant and is correlated to an increase in their physical attributes. It is unclear whether these physical improvements came from the excess of oxygen or the invigorating smell of algae.\n\nIt's curious that I find myself nostalgic for the smell of algae growing in a lab. But how could this be...?";
			}

			// Token: 0x020030EB RID: 12523
			public class BONUSGENERATOR
			{
				// Token: 0x0400D392 RID: 54162
				public static LocString NAME = "Exercised";

				// Token: 0x0400D393 RID: 54163
				public static LocString DESCRIPTION = "{dupe} ran in a Manual Generator and the physical activity appears to have given this Duplicant increased strength and sense of well-being.\n\nWhile not the primary reason for building Manual Generators, I am very pleased to see my Duplicants reaping the " + UI.FormatAsLink("Stress", "STRESS") + " relieving benefits to physical activity.";
			}

			// Token: 0x020030EC RID: 12524
			public class BONUSDOOR
			{
				// Token: 0x0400D394 RID: 54164
				public static LocString NAME = "Open and Shut";

				// Token: 0x0400D395 RID: 54165
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"The act of closing a door has apparently lead to a decrease in the ",
					UI.FormatAsLink("Stress", "STRESS"),
					" levels of {dupe}, as well as decreased the exposure of this Duplicant to harmful ",
					UI.FormatAsLink("Germs", "GERMS"),
					".\n\nWhile it may be more efficient to group all my Duplicants together in common sleeping quarters, it's important to remember the mental benefits to privacy and space to express their individuality."
				});
			}

			// Token: 0x020030ED RID: 12525
			public class BONUSHITTHEBOOKS
			{
				// Token: 0x0400D396 RID: 54166
				public static LocString NAME = "Hit the Books";

				// Token: 0x0400D397 RID: 54167
				public static LocString DESCRIPTION = "{dupe}'s recent Research errand has resulted in a significant increase to this Duplicant's brain activity. The discovery of newly found knowledge has given {dupe} an invigorating jolt of excitement.\n\nI am all too familiar with this feeling.";
			}

			// Token: 0x020030EE RID: 12526
			public class BONUSLITWORKSPACE
			{
				// Token: 0x0400D398 RID: 54168
				public static LocString NAME = "Lit-erally Great";

				// Token: 0x0400D399 RID: 54169
				public static LocString DESCRIPTION = "{dupe}'s recent time in a well-lit area has greatly improved this Duplicant's ability to work with, and on, machinery.\n\nThis supports the prevailing theory that a well-lit workspace has many benefits beyond just improving my Duplicant's ability to see.";
			}

			// Token: 0x020030EF RID: 12527
			public class BONUSTALKER
			{
				// Token: 0x0400D39A RID: 54170
				public static LocString NAME = "Big Small Talker";

				// Token: 0x0400D39B RID: 54171
				public static LocString DESCRIPTION = "{dupe}'s recent conversation with another Duplicant shows a correlation to improved serotonin and " + UI.FormatAsLink("Morale", "MORALE") + " levels in this Duplicant. It is very possible that small talk with a co-worker, however short and seemingly insignificant, will make my Duplicant's feel connected to the colony as a whole.\n\nAs the colony gets bigger and more sophisticated, I must ensure that the opportunity for such connections continue, for the good of my Duplicants' mental well being.";
			}
		}
	}
}
