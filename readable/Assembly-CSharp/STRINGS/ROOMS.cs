using System;

namespace STRINGS
{
	// Token: 0x02000FF3 RID: 4083
	public class ROOMS
	{
		// Token: 0x0200255E RID: 9566
		public class CATEGORY
		{
			// Token: 0x020030F0 RID: 12528
			public class NONE
			{
				// Token: 0x0400D39C RID: 54172
				public static LocString NAME = "None";
			}

			// Token: 0x020030F1 RID: 12529
			public class FOOD
			{
				// Token: 0x0400D39D RID: 54173
				public static LocString NAME = "Dining";
			}

			// Token: 0x020030F2 RID: 12530
			public class SLEEP
			{
				// Token: 0x0400D39E RID: 54174
				public static LocString NAME = "Sleep";
			}

			// Token: 0x020030F3 RID: 12531
			public class RECREATION
			{
				// Token: 0x0400D39F RID: 54175
				public static LocString NAME = "Recreation";
			}

			// Token: 0x020030F4 RID: 12532
			public class BATHROOM
			{
				// Token: 0x0400D3A0 RID: 54176
				public static LocString NAME = "Washroom";
			}

			// Token: 0x020030F5 RID: 12533
			public class BIONIC
			{
				// Token: 0x0400D3A1 RID: 54177
				public static LocString NAME = "";
			}

			// Token: 0x020030F6 RID: 12534
			public class HOSPITAL
			{
				// Token: 0x0400D3A2 RID: 54178
				public static LocString NAME = "Medical";
			}

			// Token: 0x020030F7 RID: 12535
			public class INDUSTRIAL
			{
				// Token: 0x0400D3A3 RID: 54179
				public static LocString NAME = "Industrial";
			}

			// Token: 0x020030F8 RID: 12536
			public class AGRICULTURAL
			{
				// Token: 0x0400D3A4 RID: 54180
				public static LocString NAME = "Agriculture";
			}

			// Token: 0x020030F9 RID: 12537
			public class PARK
			{
				// Token: 0x0400D3A5 RID: 54181
				public static LocString NAME = "Parks";
			}

			// Token: 0x020030FA RID: 12538
			public class SCIENCE
			{
				// Token: 0x0400D3A6 RID: 54182
				public static LocString NAME = "Science";
			}
		}

		// Token: 0x0200255F RID: 9567
		public class TYPES
		{
			// Token: 0x0400A8B1 RID: 43185
			public static LocString CONFLICTED = "Conflicted Room";

			// Token: 0x020030FB RID: 12539
			public class NEUTRAL
			{
				// Token: 0x0400D3A7 RID: 54183
				public static LocString NAME = "Miscellaneous Room";

				// Token: 0x0400D3A8 RID: 54184
				public static LocString DESCRIPTION = "An enclosed space with plenty of potential and no dedicated use.";

				// Token: 0x0400D3A9 RID: 54185
				public static LocString EFFECT = "- No effect";

				// Token: 0x0400D3AA RID: 54186
				public static LocString TOOLTIP = "This area has walls and doors but no dedicated use";
			}

			// Token: 0x020030FC RID: 12540
			public class LATRINE
			{
				// Token: 0x0400D3AB RID: 54187
				public static LocString NAME = "Latrine";

				// Token: 0x0400D3AC RID: 54188
				public static LocString DESCRIPTION = "It's a step up from doing one's business in full view of the rest of the colony.\n\nUsing a toilet in an enclosed room will improve Duplicants' Morale.";

				// Token: 0x0400D3AD RID: 54189
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3AE RID: 54190
				public static LocString TOOLTIP = "Using a toilet in an enclosed room will improve Duplicants' Morale";
			}

			// Token: 0x020030FD RID: 12541
			public class BIONICUPKEEP
			{
				// Token: 0x0400D3AF RID: 54191
				public static LocString NAME = "";

				// Token: 0x0400D3B0 RID: 54192
				public static LocString DESCRIPTION = "";

				// Token: 0x0400D3B1 RID: 54193
				public static LocString EFFECT = "";

				// Token: 0x0400D3B2 RID: 54194
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020030FE RID: 12542
			public class PLUMBEDBATHROOM
			{
				// Token: 0x0400D3B3 RID: 54195
				public static LocString NAME = "Washroom";

				// Token: 0x0400D3B4 RID: 54196
				public static LocString DESCRIPTION = "A sanctuary of personal hygiene.\n\nUsing a fully plumbed Washroom will improve Duplicants' Morale.";

				// Token: 0x0400D3B5 RID: 54197
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3B6 RID: 54198
				public static LocString TOOLTIP = "Using a fully plumbed Washroom will improve Duplicants' Morale";
			}

			// Token: 0x020030FF RID: 12543
			public class BARRACKS
			{
				// Token: 0x0400D3B7 RID: 54199
				public static LocString NAME = "Barracks";

				// Token: 0x0400D3B8 RID: 54200
				public static LocString DESCRIPTION = "A basic communal sleeping area for up-and-coming colonies.\n\nSleeping in Barracks will improve Duplicants' Morale.";

				// Token: 0x0400D3B9 RID: 54201
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3BA RID: 54202
				public static LocString TOOLTIP = "Sleeping in Barracks will improve Duplicants' Morale";
			}

			// Token: 0x02003100 RID: 12544
			public class BEDROOM
			{
				// Token: 0x0400D3BB RID: 54203
				public static LocString NAME = "Luxury Barracks";

				// Token: 0x0400D3BC RID: 54204
				public static LocString DESCRIPTION = "An upscale communal sleeping area full of things that greatly enhance quality of rest for occupants.\n\nSleeping in a Luxury Barracks will improve Duplicants' Morale.";

				// Token: 0x0400D3BD RID: 54205
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3BE RID: 54206
				public static LocString TOOLTIP = "Sleeping in a Luxury Barracks will improve Duplicants' Morale";
			}

			// Token: 0x02003101 RID: 12545
			public class PRIVATE_BEDROOM
			{
				// Token: 0x0400D3BF RID: 54207
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400D3C0 RID: 54208
				public static LocString DESCRIPTION = "A comfortable, roommate-free retreat where tired Duplicants can get uninterrupted rest.\n\nSleeping in a Private Bedroom will greatly improve Duplicants' Morale.";

				// Token: 0x0400D3C1 RID: 54209
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3C2 RID: 54210
				public static LocString TOOLTIP = "Sleeping in a Private Bedroom will greatly improve Duplicants' Morale";
			}

			// Token: 0x02003102 RID: 12546
			public class MESSHALL
			{
				// Token: 0x0400D3C3 RID: 54211
				public static LocString NAME = "Mess Hall";

				// Token: 0x0400D3C4 RID: 54212
				public static LocString DESCRIPTION = "A simple dining room setup that's easy to improve upon.\n\nEating at a mess table in a Mess Hall will increase Duplicants' Morale.";

				// Token: 0x0400D3C5 RID: 54213
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3C6 RID: 54214
				public static LocString TOOLTIP = "Eating at a Mess Table in a Mess Hall will improve Duplicants' Morale";
			}

			// Token: 0x02003103 RID: 12547
			public class KITCHEN
			{
				// Token: 0x0400D3C7 RID: 54215
				public static LocString NAME = "Kitchen";

				// Token: 0x0400D3C8 RID: 54216
				public static LocString DESCRIPTION = "A cooking area equipped to take meals to the next level.\n\nAdding ingredients from a Spice Grinder to foods cooked on an Electric Grill or Gas Range provides a variety of positive benefits.";

				// Token: 0x0400D3C9 RID: 54217
				public static LocString EFFECT = "- Enables Spice Grinder use";

				// Token: 0x0400D3CA RID: 54218
				public static LocString TOOLTIP = "Using a Spice Grinder in a Kitchen adds benefits to foods cooked on Electric Grill or Gas Range";
			}

			// Token: 0x02003104 RID: 12548
			public class GREATHALL
			{
				// Token: 0x0400D3CB RID: 54219
				public static LocString NAME = "Great Hall";

				// Token: 0x0400D3CC RID: 54220
				public static LocString DESCRIPTION = "A great place to eat, with great decor. Great!\n\nEating in a Great Hall will improve Duplicants' Morale.";

				// Token: 0x0400D3CD RID: 54221
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3CE RID: 54222
				public static LocString TOOLTIP = "Eating in a Great Hall will significantly improve Duplicants' Morale";
			}

			// Token: 0x02003105 RID: 12549
			public class BANQUETHALL
			{
				// Token: 0x0400D3CF RID: 54223
				public static LocString NAME = "Banquet Hall";

				// Token: 0x0400D3D0 RID: 54224
				public static LocString DESCRIPTION = "An exquisite place for communal dining.\n\nEating in a Banquet Hall will dramatically improve Duplicants' Morale.";

				// Token: 0x0400D3D1 RID: 54225
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3D2 RID: 54226
				public static LocString TOOLTIP = "Eating in a Banquet Hall will dramatically improve Duplicants' Morale";
			}

			// Token: 0x02003106 RID: 12550
			public class HOSPITAL
			{
				// Token: 0x0400D3D3 RID: 54227
				public static LocString NAME = "Hospital";

				// Token: 0x0400D3D4 RID: 54228
				public static LocString DESCRIPTION = "A dedicated medical facility that helps minimize recovery time.\n\nSick Duplicants assigned to medical buildings located within a Hospital are also less likely to spread Disease.";

				// Token: 0x0400D3D5 RID: 54229
				public static LocString EFFECT = "- Quarantine sick Duplicants";

				// Token: 0x0400D3D6 RID: 54230
				public static LocString TOOLTIP = "Sick Duplicants assigned to medical buildings located within a Hospital are less likely to spread Disease";
			}

			// Token: 0x02003107 RID: 12551
			public class MASSAGE_CLINIC
			{
				// Token: 0x0400D3D7 RID: 54231
				public static LocString NAME = "Massage Clinic";

				// Token: 0x0400D3D8 RID: 54232
				public static LocString DESCRIPTION = "A soothing space with a very relaxing ambience, especially when well-decorated.\n\nReceiving massages at a Massage Clinic will significantly improve Stress reduction.";

				// Token: 0x0400D3D9 RID: 54233
				public static LocString EFFECT = "- Massage stress relief bonus";

				// Token: 0x0400D3DA RID: 54234
				public static LocString TOOLTIP = "Receiving massages at a Massage Clinic will significantly improve Stress reduction";
			}

			// Token: 0x02003108 RID: 12552
			public class POWER_PLANT
			{
				// Token: 0x0400D3DB RID: 54235
				public static LocString NAME = "Power Plant";

				// Token: 0x0400D3DC RID: 54236
				public static LocString DESCRIPTION = "The perfect place for Duplicants to flex their Electrical Engineering skills.\n\nHeavy-duty generators built within a Power Plant can be tuned up using microchips from power control stations to improve their " + UI.FormatAsLink("Power", "POWER") + " production.";

				// Token: 0x0400D3DD RID: 54237
				public static LocString EFFECT = "- Enables " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " tune-ups on heavy-duty generators";

				// Token: 0x0400D3DE RID: 54238
				public static LocString TOOLTIP = "Heavy-duty generators built in a Power Plant can be tuned up using microchips from Power Control Stations to improve their Power production";
			}

			// Token: 0x02003109 RID: 12553
			public class MACHINE_SHOP
			{
				// Token: 0x0400D3DF RID: 54239
				public static LocString NAME = "Machine Shop";

				// Token: 0x0400D3E0 RID: 54240
				public static LocString DESCRIPTION = "It smells like elbow grease.\n\nDuplicants working in a Machine Shop can maintain buildings and increase their production speed.";

				// Token: 0x0400D3E1 RID: 54241
				public static LocString EFFECT = "- Increased fabrication efficiency";

				// Token: 0x0400D3E2 RID: 54242
				public static LocString TOOLTIP = "Duplicants working in a Machine Shop can maintain buildings and increase their production speed";
			}

			// Token: 0x0200310A RID: 12554
			public class FARM
			{
				// Token: 0x0400D3E3 RID: 54243
				public static LocString NAME = "Greenhouse";

				// Token: 0x0400D3E4 RID: 54244
				public static LocString DESCRIPTION = "An enclosed agricultural space best utilized by Duplicants with Crop Tending skills.\n\nCrops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed.";

				// Token: 0x0400D3E5 RID: 54245
				public static LocString EFFECT = "- Enables Farm Station use";

				// Token: 0x0400D3E6 RID: 54246
				public static LocString TOOLTIP = "Crops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed";
			}

			// Token: 0x0200310B RID: 12555
			public class CREATUREPEN
			{
				// Token: 0x0400D3E7 RID: 54247
				public static LocString NAME = "Stable";

				// Token: 0x0400D3E8 RID: 54248
				public static LocString DESCRIPTION = "Critters don't mind it here, as long as things don't get too crowded.\n\nStabled critters can be tended to in order to improve their happiness, hasten their domestication and increase their production.\n\nEnables the use of Grooming Stations, Shearing Stations, Critter Condos, Critter Fountains and Milking Stations.";

				// Token: 0x0400D3E9 RID: 54249
				public static LocString EFFECT = "- Critter taming and mood bonus";

				// Token: 0x0400D3EA RID: 54250
				public static LocString TOOLTIP = "A stable enables Grooming Station, Critter Condo, Critter Fountain, Shearing Station and Milking Station use";
			}

			// Token: 0x0200310C RID: 12556
			public class REC_ROOM
			{
				// Token: 0x0400D3EB RID: 54251
				public static LocString NAME = "Recreation Room";

				// Token: 0x0400D3EC RID: 54252
				public static LocString DESCRIPTION = "Where Duplicants go to mingle with off-duty peers and indulge in a little R&R.\n\nScheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room.";

				// Token: 0x0400D3ED RID: 54253
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3EE RID: 54254
				public static LocString TOOLTIP = "Scheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room";
			}

			// Token: 0x0200310D RID: 12557
			public class PARK
			{
				// Token: 0x0400D3EF RID: 54255
				public static LocString NAME = "Park";

				// Token: 0x0400D3F0 RID: 54256
				public static LocString DESCRIPTION = "A little greenery goes a long way.\n\nPassing through natural spaces throughout the day will raise the Morale of Duplicants.";

				// Token: 0x0400D3F1 RID: 54257
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3F2 RID: 54258
				public static LocString TOOLTIP = "Passing through natural spaces throughout the day will raise the Morale of Duplicants";
			}

			// Token: 0x0200310E RID: 12558
			public class NATURERESERVE
			{
				// Token: 0x0400D3F3 RID: 54259
				public static LocString NAME = "Nature Reserve";

				// Token: 0x0400D3F4 RID: 54260
				public static LocString DESCRIPTION = "A lot of greenery goes an even longer way.\n\nPassing through a Nature Reserve will grant higher Morale bonuses to Duplicants than a Park.";

				// Token: 0x0400D3F5 RID: 54261
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400D3F6 RID: 54262
				public static LocString TOOLTIP = "A Nature Reserve will grant higher Morale bonuses to Duplicants than a Park";
			}

			// Token: 0x0200310F RID: 12559
			public class LABORATORY
			{
				// Token: 0x0400D3F7 RID: 54263
				public static LocString NAME = "Laboratory";

				// Token: 0x0400D3F8 RID: 54264
				public static LocString DESCRIPTION = "Where wild hypotheses meet rigorous scientific experimentation.\n\nScience stations built in a Laboratory function more efficiently.\n\nA Laboratory enables the use of the Geotuner and the Mission Control Station.";

				// Token: 0x0400D3F9 RID: 54265
				public static LocString EFFECT = "- Efficiency bonus";

				// Token: 0x0400D3FA RID: 54266
				public static LocString TOOLTIP = "Science buildings built in a Laboratory function more efficiently\n\nA Laboratory enables Geotuner and Mission Control Station use";
			}

			// Token: 0x02003110 RID: 12560
			public class PRIVATE_BATHROOM
			{
				// Token: 0x0400D3FB RID: 54267
				public static LocString NAME = "Private Bathroom";

				// Token: 0x0400D3FC RID: 54268
				public static LocString DESCRIPTION = "Finally, a place to truly be alone with one's thoughts.\n\nDuplicants relieve even more Stress when using the toilet in a Private Bathroom than in a Latrine.";

				// Token: 0x0400D3FD RID: 54269
				public static LocString EFFECT = "- Stress relief bonus";

				// Token: 0x0400D3FE RID: 54270
				public static LocString TOOLTIP = "Duplicants relieve even more stress when using the toilet in a Private Bathroom than in a Latrine";
			}

			// Token: 0x02003111 RID: 12561
			public class BIONIC_UPKEEP
			{
				// Token: 0x0400D3FF RID: 54271
				public static LocString NAME = "";

				// Token: 0x0400D400 RID: 54272
				public static LocString DESCRIPTION = "";

				// Token: 0x0400D401 RID: 54273
				public static LocString EFFECT = "";

				// Token: 0x0400D402 RID: 54274
				public static LocString TOOLTIP = "";
			}
		}

		// Token: 0x02002560 RID: 9568
		public class CRITERIA
		{
			// Token: 0x0400A8B2 RID: 43186
			public static LocString HEADER = "<b>Requirements:</b>";

			// Token: 0x0400A8B3 RID: 43187
			public static LocString NEUTRAL_TYPE = "Enclosed by wall tile";

			// Token: 0x0400A8B4 RID: 43188
			public static LocString POSSIBLE_TYPES_HEADER = "Possible Room Types";

			// Token: 0x0400A8B5 RID: 43189
			public static LocString NO_TYPE_CONFLICTS = "Remove conflicting buildings";

			// Token: 0x0400A8B6 RID: 43190
			public static LocString IN_CODE_ERROR = "String Key Not Found: {0}";

			// Token: 0x02003112 RID: 12562
			public class CRITERIA_FAILED
			{
				// Token: 0x0400D403 RID: 54275
				public static LocString MISSING_BUILDING = "Missing {0}";

				// Token: 0x0400D404 RID: 54276
				public static LocString FAILED = "{0}";
			}

			// Token: 0x02003113 RID: 12563
			public static class DECORATION
			{
				// Token: 0x0400D405 RID: 54277
				public static LocString NAME = UI.FormatAsLink("Decor building", "REQUIREMENTCLASSDECORATION");

				// Token: 0x0400D406 RID: 54278
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECORATION.NAME;
			}

			// Token: 0x02003114 RID: 12564
			public class CEILING_HEIGHT
			{
				// Token: 0x0400D407 RID: 54279
				public static LocString NAME = "Minimum height: {0} tiles";

				// Token: 0x0400D408 RID: 54280
				public static LocString DESCRIPTION = "Must have a ceiling height of at least {0} tiles";

				// Token: 0x0400D409 RID: 54281
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CEILING_HEIGHT.NAME;
			}

			// Token: 0x02003115 RID: 12565
			public class MINIMUM_SIZE
			{
				// Token: 0x0400D40A RID: 54282
				public static LocString NAME = "Minimum size: {0} tiles";

				// Token: 0x0400D40B RID: 54283
				public static LocString DESCRIPTION = "Must have an area of at least {0} tiles";

				// Token: 0x0400D40C RID: 54284
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MINIMUM_SIZE.NAME;
			}

			// Token: 0x02003116 RID: 12566
			public class MAXIMUM_SIZE
			{
				// Token: 0x0400D40D RID: 54285
				public static LocString NAME = "Maximum size: {0} tiles";

				// Token: 0x0400D40E RID: 54286
				public static LocString DESCRIPTION = "Must have an area no larger than {0} tiles";

				// Token: 0x0400D40F RID: 54287
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MAXIMUM_SIZE.NAME;
			}

			// Token: 0x02003117 RID: 12567
			public class INDUSTRIALMACHINERY
			{
				// Token: 0x0400D410 RID: 54288
				public static LocString NAME = UI.FormatAsLink("Industrial machinery", "REQUIREMENTCLASSINDUSTRIALMACHINERY");

				// Token: 0x0400D411 RID: 54289
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.INDUSTRIALMACHINERY.NAME;
			}

			// Token: 0x02003118 RID: 12568
			public class HAS_BED
			{
				// Token: 0x0400D412 RID: 54290
				public static LocString NAME = "One or more " + UI.FormatAsLink("beds", "REQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400D413 RID: 54291
				public static LocString DESCRIPTION = "Requires at least one Cot or Comfy Bed";

				// Token: 0x0400D414 RID: 54292
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HAS_BED.NAME;
			}

			// Token: 0x02003119 RID: 12569
			public class HAS_LUXURY_BED
			{
				// Token: 0x0400D415 RID: 54293
				public static LocString NAME = "One or more " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				// Token: 0x0400D416 RID: 54294
				public static LocString DESCRIPTION = "Requires at least one Comfy Bed";

				// Token: 0x0400D417 RID: 54295
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HAS_LUXURY_BED.NAME;
			}

			// Token: 0x0200311A RID: 12570
			public class LUXURYBEDTYPE
			{
				// Token: 0x0400D418 RID: 54296
				public static LocString NAME = "Single " + UI.FormatAsLink("Comfy Bed", "LUXURYBED");

				// Token: 0x0400D419 RID: 54297
				public static LocString DESCRIPTION = "Must have no more than one Comfy Bed";

				// Token: 0x0400D41A RID: 54298
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LUXURYBEDTYPE.NAME;
			}

			// Token: 0x0200311B RID: 12571
			public class BED_SINGLE
			{
				// Token: 0x0400D41B RID: 54299
				public static LocString NAME = "Single " + UI.FormatAsLink("beds", "REQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400D41C RID: 54300
				public static LocString DESCRIPTION = "Must have no more than one Cot or Comfy Bed";

				// Token: 0x0400D41D RID: 54301
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BED_SINGLE.NAME;
			}

			// Token: 0x0200311C RID: 12572
			public class IS_BACKWALLED
			{
				// Token: 0x0400D41E RID: 54302
				public static LocString NAME = "Has backwall tiles";

				// Token: 0x0400D41F RID: 54303
				public static LocString DESCRIPTION = "Must be covered in backwall tiles";

				// Token: 0x0400D420 RID: 54304
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.IS_BACKWALLED.NAME;
			}

			// Token: 0x0200311D RID: 12573
			public class NO_COTS
			{
				// Token: 0x0400D421 RID: 54305
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED");

				// Token: 0x0400D422 RID: 54306
				public static LocString DESCRIPTION = "Room cannot contain a Cot";

				// Token: 0x0400D423 RID: 54307
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_COTS.NAME;
			}

			// Token: 0x0200311E RID: 12574
			public class NO_LUXURY_BEDS
			{
				// Token: 0x0400D424 RID: 54308
				public static LocString NAME = "No " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				// Token: 0x0400D425 RID: 54309
				public static LocString DESCRIPTION = "Room cannot contain a Comfy Bed";

				// Token: 0x0400D426 RID: 54310
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_LUXURY_BEDS.NAME;
			}

			// Token: 0x0200311F RID: 12575
			public class BEDTYPE
			{
				// Token: 0x0400D427 RID: 54311
				public static LocString NAME = UI.FormatAsLink("Beds", "REQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400D428 RID: 54312
				public static LocString DESCRIPTION = "Requires two or more Cots or Comfy Beds";

				// Token: 0x0400D429 RID: 54313
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BEDTYPE.NAME;
			}

			// Token: 0x02003120 RID: 12576
			public class BUILDING_DECOR_POSITIVE
			{
				// Token: 0x0400D42A RID: 54314
				public static LocString NAME = "Positive " + UI.FormatAsLink("decor", "REQUIREMENTCLASSDECORATION");

				// Token: 0x0400D42B RID: 54315
				public static LocString DESCRIPTION = "Requires at least one building with positive decor";

				// Token: 0x0400D42C RID: 54316
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME;
			}

			// Token: 0x02003121 RID: 12577
			public class DECORATIVE_ITEM
			{
				// Token: 0x0400D42D RID: 54317
				public static LocString NAME = UI.FormatAsLink("Decor building", "REQUIREMENTCLASSDECORATION") + " ({0})";

				// Token: 0x0400D42E RID: 54318
				public static LocString DESCRIPTION = "Requires {0} or more Decor buildings";

				// Token: 0x0400D42F RID: 54319
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECORATIVE_ITEM.NAME;
			}

			// Token: 0x02003122 RID: 12578
			public class ORNAMENT
			{
				// Token: 0x0400D430 RID: 54320
				public static LocString NAME = UI.FormatAsLink("Displayed Ornament", "REQUIREMENTCLASSORNAMENT");

				// Token: 0x0400D431 RID: 54321
				public static LocString DESCRIPTION = "Requires an ornament displayed on a " + BUILDINGS.PREFABS.ITEMPEDESTAL.NAME + " or " + BUILDINGS.PREFABS.SHELF.NAME;

				// Token: 0x0400D432 RID: 54322
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.ORNAMENT.NAME;
			}

			// Token: 0x02003123 RID: 12579
			public class CLINIC
			{
				// Token: 0x0400D433 RID: 54323
				public static LocString NAME = UI.FormatAsLink("Medical equipment", "REQUIREMENTCLASSCLINIC");

				// Token: 0x0400D434 RID: 54324
				public static LocString DESCRIPTION = "Requires one or more Sick Bays or Disease Clinics";

				// Token: 0x0400D435 RID: 54325
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CLINIC.NAME;
			}

			// Token: 0x02003124 RID: 12580
			public class POWERPLANT
			{
				// Token: 0x0400D436 RID: 54326
				public static LocString NAME = UI.FormatAsLink("Heavy-Duty Generator", "REQUIREMENTCLASSGENERATORTYPE") + "\n    • Two or more " + UI.FormatAsLink("Power Buildings", "REQUIREMENTCLASSPOWERBUILDING");

				// Token: 0x0400D437 RID: 54327
				public static LocString DESCRIPTION = "Requires a Heavy-Duty Generator and two or more Power Buildings";

				// Token: 0x0400D438 RID: 54328
				public static LocString CONFLICT_DESCRIPTION = "Heavy-Duty Generator and two or more Power buildings";
			}

			// Token: 0x02003125 RID: 12581
			public class FARMSTATIONTYPE
			{
				// Token: 0x0400D439 RID: 54329
				public static LocString NAME = UI.FormatAsLink("Farm Station", "FARMSTATION");

				// Token: 0x0400D43A RID: 54330
				public static LocString DESCRIPTION = "Requires a single Farm Station";

				// Token: 0x0400D43B RID: 54331
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FARMSTATIONTYPE.NAME;
			}

			// Token: 0x02003126 RID: 12582
			public class FARMBUILDING
			{
				// Token: 0x0400D43C RID: 54332
				public static LocString NAME = UI.FormatAsLink("Farm Building", "FARMBUILDING");

				// Token: 0x0400D43D RID: 54333
				public static LocString DESCRIPTION = "";

				// Token: 0x0400D43E RID: 54334
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FARMBUILDING.NAME;
			}

			// Token: 0x02003127 RID: 12583
			public class CREATURE_FEEDER
			{
				// Token: 0x0400D43F RID: 54335
				public static LocString NAME = UI.FormatAsLink("Critter Feeder", "CREATUREFEEDER");

				// Token: 0x0400D440 RID: 54336
				public static LocString DESCRIPTION = "Requires a single Critter Feeder";

				// Token: 0x0400D441 RID: 54337
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CREATURE_FEEDER.NAME;
			}

			// Token: 0x02003128 RID: 12584
			public class RANCHSTATIONTYPE
			{
				// Token: 0x0400D442 RID: 54338
				public static LocString NAME = UI.FormatAsLink("Ranching building", "REQUIREMENTCLASSRANCHSTATIONTYPE");

				// Token: 0x0400D443 RID: 54339
				public static LocString DESCRIPTION = "Requires a single Grooming Station, Critter Condo, Critter Fountain, Shearing Station or Milking Station";

				// Token: 0x0400D444 RID: 54340
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RANCHSTATIONTYPE.NAME;
			}

			// Token: 0x02003129 RID: 12585
			public class SPICESTATION
			{
				// Token: 0x0400D445 RID: 54341
				public static LocString NAME = UI.FormatAsLink("Spice Grinder", "SPICEGRINDER");

				// Token: 0x0400D446 RID: 54342
				public static LocString DESCRIPTION = "Requires a single Spice Grinder";

				// Token: 0x0400D447 RID: 54343
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SPICESTATION.NAME;
			}

			// Token: 0x0200312A RID: 12586
			public class COOKTOP
			{
				// Token: 0x0400D448 RID: 54344
				public static LocString NAME = UI.FormatAsLink("Cooking station", "REQUIREMENTCLASSCOOKTOP");

				// Token: 0x0400D449 RID: 54345
				public static LocString DESCRIPTION = "Requires a single Electric Grill or Gas Range";

				// Token: 0x0400D44A RID: 54346
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.COOKTOP.NAME;
			}

			// Token: 0x0200312B RID: 12587
			public class REFRIGERATOR
			{
				// Token: 0x0400D44B RID: 54347
				public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

				// Token: 0x0400D44C RID: 54348
				public static LocString DESCRIPTION = "Requires a single Refrigerator";

				// Token: 0x0400D44D RID: 54349
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.REFRIGERATOR.NAME;
			}

			// Token: 0x0200312C RID: 12588
			public class RECBUILDING
			{
				// Token: 0x0400D44E RID: 54350
				public static LocString NAME = UI.FormatAsLink("Recreational building", "REQUIREMENTCLASSRECBUILDING");

				// Token: 0x0400D44F RID: 54351
				public static LocString DESCRIPTION = "Requires one or more recreational buildings";

				// Token: 0x0400D450 RID: 54352
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RECBUILDING.NAME;
			}

			// Token: 0x0200312D RID: 12589
			public class PARK
			{
				// Token: 0x0400D451 RID: 54353
				public static LocString NAME = UI.FormatAsLink("Park Sign", "PARKSIGN");

				// Token: 0x0400D452 RID: 54354
				public static LocString DESCRIPTION = "Requires one or more Park Signs";

				// Token: 0x0400D453 RID: 54355
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.PARK.NAME;
			}

			// Token: 0x0200312E RID: 12590
			public class MACHINESHOPTYPE
			{
				// Token: 0x0400D454 RID: 54356
				public static LocString NAME = "Mechanics Station";

				// Token: 0x0400D455 RID: 54357
				public static LocString DESCRIPTION = "Requires requires one or more Mechanics Stations";

				// Token: 0x0400D456 RID: 54358
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MACHINESHOPTYPE.NAME;
			}

			// Token: 0x0200312F RID: 12591
			public class FOOD_BOX
			{
				// Token: 0x0400D457 RID: 54359
				public static LocString NAME = "Food storage";

				// Token: 0x0400D458 RID: 54360
				public static LocString DESCRIPTION = "Requires one or more Ration Boxes or Refrigerators";

				// Token: 0x0400D459 RID: 54361
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FOOD_BOX.NAME;
			}

			// Token: 0x02003130 RID: 12592
			[Obsolete("The light requirement constraint in rooms has been removed. This is retained solely to avoid breaking mods")]
			public class LIGHTSOURCE
			{
				// Token: 0x0400D45A RID: 54362
				public static LocString NAME = UI.FormatAsLink("Light source", "REQUIREMENTCLASSLIGHTSOURCE");

				// Token: 0x0400D45B RID: 54363
				public static LocString DESCRIPTION = "Requires one or more light sources";

				// Token: 0x0400D45C RID: 54364
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LIGHTSOURCE.NAME;
			}

			// Token: 0x02003131 RID: 12593
			public class DESTRESSINGBUILDING
			{
				// Token: 0x0400D45D RID: 54365
				public static LocString NAME = UI.FormatAsLink("De-Stressing Building", "MASSAGETABLE");

				// Token: 0x0400D45E RID: 54366
				public static LocString DESCRIPTION = "Requires one or more De-Stressing buildings";

				// Token: 0x0400D45F RID: 54367
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DESTRESSINGBUILDING.NAME;
			}

			// Token: 0x02003132 RID: 12594
			public class MASSAGE_TABLE
			{
				// Token: 0x0400D460 RID: 54368
				public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

				// Token: 0x0400D461 RID: 54369
				public static LocString DESCRIPTION = "Requires one or more Massage Tables";

				// Token: 0x0400D462 RID: 54370
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MASSAGE_TABLE.NAME;
			}

			// Token: 0x02003133 RID: 12595
			public class DININGTABLETYPE
			{
				// Token: 0x0400D463 RID: 54371
				public static LocString NAME = UI.FormatAsLink("Dining Table", "REQUIREMENTCLASSDININGTABLETYPE");

				// Token: 0x0400D464 RID: 54372
				public static LocString DESCRIPTION = "Requires a single Mess Table or Communal Table";

				// Token: 0x0400D465 RID: 54373
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DININGTABLETYPE.NAME;
			}

			// Token: 0x02003134 RID: 12596
			public class NO_BASIC_MESS_STATIONS
			{
				// Token: 0x0400D466 RID: 54374
				public static LocString NAME = "No " + UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x0400D467 RID: 54375
				public static LocString DESCRIPTION = "Cannot contain a Mess Table";

				// Token: 0x0400D468 RID: 54376
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_BASIC_MESS_STATIONS.NAME;
			}

			// Token: 0x02003135 RID: 12597
			public class NO_MESS_STATION
			{
				// Token: 0x0400D469 RID: 54377
				public static LocString NAME = "No " + UI.FormatAsLink("dining tables", "DININGTABLE");

				// Token: 0x0400D46A RID: 54378
				public static LocString DESCRIPTION = "Cannot contain a Mess Table or Communal Table";

				// Token: 0x0400D46B RID: 54379
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_MESS_STATION.NAME;
			}

			// Token: 0x02003136 RID: 12598
			public class MESS_STATION_MULTIPLE
			{
				// Token: 0x0400D46C RID: 54380
				public static LocString NAME = UI.FormatAsLink("Mess Tables", "DININGTABLE");

				// Token: 0x0400D46D RID: 54381
				public static LocString DESCRIPTION = "Requires two or more Mess Tables";

				// Token: 0x0400D46E RID: 54382
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MESS_STATION_MULTIPLE.NAME;
			}

			// Token: 0x02003137 RID: 12599
			public class MULTI_MINION_DINING_TABLE
			{
				// Token: 0x0400D46F RID: 54383
				public static LocString NAME = UI.FormatAsLink("Communal Table", "MULTIMINIONDININGTABLE");

				// Token: 0x0400D470 RID: 54384
				public static LocString DESCRIPTION = "Requires a Communal Table";

				// Token: 0x0400D471 RID: 54385
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MULTI_MINION_DINING_TABLE.NAME;
			}

			// Token: 0x02003138 RID: 12600
			public class RESEARCH_STATION
			{
				// Token: 0x0400D472 RID: 54386
				public static LocString NAME = UI.FormatAsLink("Research station", "REQUIREMENTCLASSRESEARCH_STATION");

				// Token: 0x0400D473 RID: 54387
				public static LocString DESCRIPTION = "Requires one or more Research Stations or Super Computers";

				// Token: 0x0400D474 RID: 54388
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RESEARCH_STATION.NAME;
			}

			// Token: 0x02003139 RID: 12601
			public class BIONICUPKEEP
			{
				// Token: 0x0400D475 RID: 54389
				public static LocString NAME = UI.FormatAsLink("Bionic service station", "GROUPBIONICUPKEEP");

				// Token: 0x0400D476 RID: 54390
				public static LocString DESCRIPTION = "Requires at least one Lubrication Station and one Gunk Extractor";

				// Token: 0x0400D477 RID: 54391
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONICUPKEEP.NAME;
			}

			// Token: 0x0200313A RID: 12602
			public class BIONIC_GUNKEMPTIER
			{
				// Token: 0x0400D478 RID: 54392
				public static LocString NAME = UI.FormatAsLink("Gunk Extractor", "REQUIREMENTCLASSBIONIC_GUNKEMPTIER");

				// Token: 0x0400D479 RID: 54393
				public static LocString DESCRIPTION = "Requires one or more Gunk Extractors";

				// Token: 0x0400D47A RID: 54394
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.NAME;
			}

			// Token: 0x0200313B RID: 12603
			public class BIONIC_LUBRICATION
			{
				// Token: 0x0400D47B RID: 54395
				public static LocString NAME = UI.FormatAsLink("Lubrication Station", "REQUIREMENTCLASSBIONIC_LUBRICATION");

				// Token: 0x0400D47C RID: 54396
				public static LocString DESCRIPTION = "Requires one or more Lubrication Stations";

				// Token: 0x0400D47D RID: 54397
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONIC_LUBRICATION.NAME;
			}

			// Token: 0x0200313C RID: 12604
			public class TOILETTYPE
			{
				// Token: 0x0400D47E RID: 54398
				public static LocString NAME = UI.FormatAsLink("Toilet", "REQUIREMENTCLASSTOILETTYPE");

				// Token: 0x0400D47F RID: 54399
				public static LocString DESCRIPTION = "Requires one or more Outhouses or Lavatories";

				// Token: 0x0400D480 RID: 54400
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.TOILETTYPE.NAME;
			}

			// Token: 0x0200313D RID: 12605
			public class FLUSHTOILETTYPE
			{
				// Token: 0x0400D481 RID: 54401
				public static LocString NAME = UI.FormatAsLink("Flush Toilet", "REQUIREMENTCLASSFLUSHTOILETTYPE");

				// Token: 0x0400D482 RID: 54402
				public static LocString DESCRIPTION = "Requires one or more Lavatories";

				// Token: 0x0400D483 RID: 54403
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FLUSHTOILETTYPE.NAME;
			}

			// Token: 0x0200313E RID: 12606
			public class NO_OUTHOUSES
			{
				// Token: 0x0400D484 RID: 54404
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouses", "OUTHOUSE");

				// Token: 0x0400D485 RID: 54405
				public static LocString DESCRIPTION = "Cannot contain basic Outhouses";

				// Token: 0x0400D486 RID: 54406
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_OUTHOUSES.NAME;
			}

			// Token: 0x0200313F RID: 12607
			public class WASHSTATION
			{
				// Token: 0x0400D487 RID: 54407
				public static LocString NAME = UI.FormatAsLink("Wash station", "REQUIREMENTCLASSWASHSTATION");

				// Token: 0x0400D488 RID: 54408
				public static LocString DESCRIPTION = "Requires one or more Wash Basins, Sinks, Hand Sanitizers, or Showers";

				// Token: 0x0400D489 RID: 54409
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WASHSTATION.NAME;
			}

			// Token: 0x02003140 RID: 12608
			public class ADVANCEDWASHSTATION
			{
				// Token: 0x0400D48A RID: 54410
				public static LocString NAME = UI.FormatAsLink("Plumbed wash station", "REQUIREMENTCLASSWASHSTATION");

				// Token: 0x0400D48B RID: 54411
				public static LocString DESCRIPTION = "Requires one or more Sinks, Hand Sanitizers, or Showers";

				// Token: 0x0400D48C RID: 54412
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.ADVANCEDWASHSTATION.NAME;
			}

			// Token: 0x02003141 RID: 12609
			public class NO_INDUSTRIAL_MACHINERY
			{
				// Token: 0x0400D48D RID: 54413
				public static LocString NAME = "No " + UI.FormatAsLink("industrial machinery", "REQUIREMENTCLASSINDUSTRIALMACHINERY");

				// Token: 0x0400D48E RID: 54414
				public static LocString DESCRIPTION = "Cannot contain any building labeled Industrial Machinery";

				// Token: 0x0400D48F RID: 54415
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME;
			}

			// Token: 0x02003142 RID: 12610
			public class WILDANIMAL
			{
				// Token: 0x0400D490 RID: 54416
				public static LocString NAME = "Wildlife";

				// Token: 0x0400D491 RID: 54417
				public static LocString DESCRIPTION = "Requires at least one wild critter";

				// Token: 0x0400D492 RID: 54418
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDANIMAL.NAME;
			}

			// Token: 0x02003143 RID: 12611
			public class WILDANIMALS
			{
				// Token: 0x0400D493 RID: 54419
				public static LocString NAME = "More wildlife";

				// Token: 0x0400D494 RID: 54420
				public static LocString DESCRIPTION = "Requires two or more wild critters";

				// Token: 0x0400D495 RID: 54421
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDANIMALS.NAME;
			}

			// Token: 0x02003144 RID: 12612
			public class WILDPLANT
			{
				// Token: 0x0400D496 RID: 54422
				public static LocString NAME = "Two wild plants";

				// Token: 0x0400D497 RID: 54423
				public static LocString DESCRIPTION = "Requires two or more wild plants";

				// Token: 0x0400D498 RID: 54424
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDPLANT.NAME;
			}

			// Token: 0x02003145 RID: 12613
			public class WILDPLANTS
			{
				// Token: 0x0400D499 RID: 54425
				public static LocString NAME = "Four wild plants";

				// Token: 0x0400D49A RID: 54426
				public static LocString DESCRIPTION = "Requires four or more wild plants";

				// Token: 0x0400D49B RID: 54427
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDPLANTS.NAME;
			}

			// Token: 0x02003146 RID: 12614
			public class SCIENCEBUILDING
			{
				// Token: 0x0400D49C RID: 54428
				public static LocString NAME = UI.FormatAsLink("Science building", "REQUIREMENTCLASSSCIENCEBUILDING");

				// Token: 0x0400D49D RID: 54429
				public static LocString DESCRIPTION = "Requires one or more science buildings";

				// Token: 0x0400D49E RID: 54430
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SCIENCEBUILDING.NAME;
			}

			// Token: 0x02003147 RID: 12615
			public class SCIENCE_BUILDINGS
			{
				// Token: 0x0400D49F RID: 54431
				public static LocString NAME = "Two " + UI.FormatAsLink("science buildings", "REQUIREMENTCLASSSCIENCEBUILDING");

				// Token: 0x0400D4A0 RID: 54432
				public static LocString DESCRIPTION = "Requires two or more science buildings";

				// Token: 0x0400D4A1 RID: 54433
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SCIENCE_BUILDINGS.NAME;
			}

			// Token: 0x02003148 RID: 12616
			public class ROCKETINTERIOR
			{
				// Token: 0x0400D4A2 RID: 54434
				public static LocString NAME = UI.FormatAsLink("Rocket interior", "REQUIREMENTCLASSROCKETINTERIOR");

				// Token: 0x0400D4A3 RID: 54435
				public static LocString DESCRIPTION = "Must be built inside a rocket";

				// Token: 0x0400D4A4 RID: 54436
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.ROCKETINTERIOR.NAME;
			}

			// Token: 0x02003149 RID: 12617
			public class WARMINGSTATION
			{
				// Token: 0x0400D4A5 RID: 54437
				public static LocString NAME = UI.FormatAsLink("Warming station", "REQUIREMENTCLASSWARMINGSTATION");

				// Token: 0x0400D4A6 RID: 54438
				public static LocString DESCRIPTION = "Raises the ambient temperature";

				// Token: 0x0400D4A7 RID: 54439
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WARMINGSTATION.NAME;
			}

			// Token: 0x0200314A RID: 12618
			public class GENERATORTYPE
			{
				// Token: 0x0400D4A8 RID: 54440
				public static LocString NAME = UI.FormatAsLink("Generator", "REQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400D4A9 RID: 54441
				public static LocString DESCRIPTION = "Generates electrical power";

				// Token: 0x0400D4AA RID: 54442
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.GENERATORTYPE.NAME;
			}

			// Token: 0x0200314B RID: 12619
			public class HEAVYDUTYGENERATORTYPE
			{
				// Token: 0x0400D4AB RID: 54443
				public static LocString NAME = UI.FormatAsLink("Heavy-duty generator", "REQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400D4AC RID: 54444
				public static LocString DESCRIPTION = "For big power needs";

				// Token: 0x0400D4AD RID: 54445
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HEAVYDUTYGENERATORTYPE.NAME;
			}

			// Token: 0x0200314C RID: 12620
			public class LIGHTDUTYGENERATORTYPE
			{
				// Token: 0x0400D4AE RID: 54446
				public static LocString NAME = UI.FormatAsLink("Basic generator", "REQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400D4AF RID: 54447
				public static LocString DESCRIPTION = "For basic power needs";

				// Token: 0x0400D4B0 RID: 54448
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LIGHTDUTYGENERATORTYPE.NAME;
			}

			// Token: 0x0200314D RID: 12621
			public class POWERBUILDING
			{
				// Token: 0x0400D4B1 RID: 54449
				public static LocString NAME = UI.FormatAsLink("Power building", "REQUIREMENTCLASSPOWERBUILDING");

				// Token: 0x0400D4B2 RID: 54450
				public static LocString DESCRIPTION = "Buildings that generate, store, or manage power";

				// Token: 0x0400D4B3 RID: 54451
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.POWERBUILDING.NAME;
			}
		}

		// Token: 0x02002561 RID: 9569
		public class DETAILS
		{
			// Token: 0x0400A8B7 RID: 43191
			public static LocString HEADER = "Room Details";

			// Token: 0x0200314E RID: 12622
			public class ASSIGNED_TO
			{
				// Token: 0x0400D4B4 RID: 54452
				public static LocString NAME = "<b>Assignments:</b>\n{0}";

				// Token: 0x0400D4B5 RID: 54453
				public static LocString UNASSIGNED = "Unassigned";
			}

			// Token: 0x0200314F RID: 12623
			public class AVERAGE_TEMPERATURE
			{
				// Token: 0x0400D4B6 RID: 54454
				public static LocString NAME = "Average temperature: {0}";
			}

			// Token: 0x02003150 RID: 12624
			public class AVERAGE_ATMO_MASS
			{
				// Token: 0x0400D4B7 RID: 54455
				public static LocString NAME = "Average air pressure: {0}";
			}

			// Token: 0x02003151 RID: 12625
			public class SIZE
			{
				// Token: 0x0400D4B8 RID: 54456
				public static LocString NAME = "Room size: {0} Tiles";
			}

			// Token: 0x02003152 RID: 12626
			public class BUILDING_COUNT
			{
				// Token: 0x0400D4B9 RID: 54457
				public static LocString NAME = "Buildings: {0}";
			}

			// Token: 0x02003153 RID: 12627
			public class CREATURE_COUNT
			{
				// Token: 0x0400D4BA RID: 54458
				public static LocString NAME = "Critters: {0}";
			}

			// Token: 0x02003154 RID: 12628
			public class PLANT_COUNT
			{
				// Token: 0x0400D4BB RID: 54459
				public static LocString NAME = "Plants: {0}";
			}

			// Token: 0x02003155 RID: 12629
			public class ORNAMENT_COUNT
			{
				// Token: 0x0400D4BC RID: 54460
				public static LocString NAME = "Displayed ornaments: {0}";
			}
		}

		// Token: 0x02002562 RID: 9570
		public class EFFECTS
		{
			// Token: 0x0400A8B8 RID: 43192
			public static LocString HEADER = "<b>Effects:</b>";
		}
	}
}
