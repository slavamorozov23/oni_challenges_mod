using System;

namespace STRINGS
{
	// Token: 0x02000FED RID: 4077
	public class EQUIPMENT
	{
		// Token: 0x02002342 RID: 9026
		public class PREFABS
		{
			// Token: 0x02002E0E RID: 11790
			public class OXYGEN_MASK
			{
				// Token: 0x0400C6B0 RID: 50864
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400C6B1 RID: 50865
				public static LocString DESC = "Ensures my Duplicants can breathe easy... for a little while, anyways.";

				// Token: 0x0400C6B2 RID: 50866
				public static LocString EFFECT = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.\n\nMust be refilled with oxygen at an " + UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER") + " when depleted.";

				// Token: 0x0400C6B3 RID: 50867
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.";

				// Token: 0x0400C6B4 RID: 50868
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400C6B5 RID: 50869
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400C6B6 RID: 50870
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK"),
					".\n\nMasks can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});

				// Token: 0x0400C6B7 RID: 50871
				public static LocString REPAIR_WORN_RECIPE_NAME = "Repair " + EQUIPMENT.PREFABS.OXYGEN_MASK.NAME;

				// Token: 0x0400C6B8 RID: 50872
				public static LocString REPAIR_WORN_DESC = "Restore a " + UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK") + " to working order.";
			}

			// Token: 0x02002E0F RID: 11791
			public class ATMO_SUIT
			{
				// Token: 0x0400C6B9 RID: 50873
				public static LocString NAME = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400C6BA RID: 50874
				public static LocString DESC = "Ensures my Duplicants can breathe easy, anytime, anywhere.";

				// Token: 0x0400C6BB RID: 50875
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments, and protects against extreme temperatures.\n\nMust be refilled with oxygen at an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400C6BC RID: 50876
				public static LocString RECIPE_DESC = "Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " in toxic and low breathability environments.";

				// Token: 0x0400C6BD RID: 50877
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400C6BE RID: 50878
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400C6BF RID: 50879
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Atmo Suit", "ATMO_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});

				// Token: 0x0400C6C0 RID: 50880
				public static LocString REPAIR_WORN_RECIPE_NAME = "Repair " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400C6C1 RID: 50881
				public static LocString REPAIR_WORN_DESC = "Restore a " + UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT") + " to working order.";
			}

			// Token: 0x02002E10 RID: 11792
			public class ATMO_SUIT_SET
			{
				// Token: 0x02003B06 RID: 15110
				public class PUFT
				{
					// Token: 0x0400ED17 RID: 60695
					public static LocString NAME = "Puft Atmo Suit";

					// Token: 0x0400ED18 RID: 60696
					public static LocString DESC = "Critter-forward protective gear for the intrepid explorer!\nReleased for Klei Fest 2023.";
				}
			}

			// Token: 0x02002E11 RID: 11793
			public class HOLIDAY_2023_CRATE
			{
				// Token: 0x0400C6C2 RID: 50882
				public static LocString NAME = "Holiday Gift Crate";

				// Token: 0x0400C6C3 RID: 50883
				public static LocString DESC = "An unaddressed package has been discovered near the Printing Pod. It exudes seasonal cheer, and trace amounts of Neutronium have been detected.";
			}

			// Token: 0x02002E12 RID: 11794
			public class ATMO_SUIT_HELMET
			{
				// Token: 0x0400C6C4 RID: 50884
				public static LocString NAME = "Default Atmo Helmet";

				// Token: 0x0400C6C5 RID: 50885
				public static LocString DESC = "Default helmet for atmo suits.";

				// Token: 0x02003B07 RID: 15111
				public class FACADES
				{
					// Token: 0x02003F57 RID: 16215
					public class SPARKLE_RED
					{
						// Token: 0x0400F77B RID: 63355
						public static LocString NAME = "Red Glitter Atmo Helmet";

						// Token: 0x0400F77C RID: 63356
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02003F58 RID: 16216
					public class SPARKLE_GREEN
					{
						// Token: 0x0400F77D RID: 63357
						public static LocString NAME = "Green Glitter Atmo Helmet";

						// Token: 0x0400F77E RID: 63358
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02003F59 RID: 16217
					public class SPARKLE_BLUE
					{
						// Token: 0x0400F77F RID: 63359
						public static LocString NAME = "Blue Glitter Atmo Helmet";

						// Token: 0x0400F780 RID: 63360
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02003F5A RID: 16218
					public class SPARKLE_PURPLE
					{
						// Token: 0x0400F781 RID: 63361
						public static LocString NAME = "Violet Glitter Atmo Helmet";

						// Token: 0x0400F782 RID: 63362
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02003F5B RID: 16219
					public class LIMONE
					{
						// Token: 0x0400F783 RID: 63363
						public static LocString NAME = "Citrus Atmo Helmet";

						// Token: 0x0400F784 RID: 63364
						public static LocString DESC = "Fresh, fruity and full of breathable air.";
					}

					// Token: 0x02003F5C RID: 16220
					public class PUFT
					{
						// Token: 0x0400F785 RID: 63365
						public static LocString NAME = "Puft Atmo Helmet";

						// Token: 0x0400F786 RID: 63366
						public static LocString DESC = "Convincing enough to fool most Pufts and even a few Duplicants.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02003F5D RID: 16221
					public class CLUBSHIRT_PURPLE
					{
						// Token: 0x0400F787 RID: 63367
						public static LocString NAME = "Eggplant Atmo Helmet";

						// Token: 0x0400F788 RID: 63368
						public static LocString DESC = "It is neither an egg, nor a plant. But it <i>is</i> a functional helmet.";
					}

					// Token: 0x02003F5E RID: 16222
					public class TRIANGLES_TURQ
					{
						// Token: 0x0400F789 RID: 63369
						public static LocString NAME = "Confetti Atmo Helmet";

						// Token: 0x0400F78A RID: 63370
						public static LocString DESC = "Doubles as a party hat.";
					}

					// Token: 0x02003F5F RID: 16223
					public class CUMMERBUND_RED
					{
						// Token: 0x0400F78B RID: 63371
						public static LocString NAME = "Blastoff Atmo Helmet";

						// Token: 0x0400F78C RID: 63372
						public static LocString DESC = "Red means go!";
					}

					// Token: 0x02003F60 RID: 16224
					public class WORKOUT_LAVENDER
					{
						// Token: 0x0400F78D RID: 63373
						public static LocString NAME = "Pink Punch Atmo Helmet";

						// Token: 0x0400F78E RID: 63374
						public static LocString DESC = "Unapologetically ostentatious.";
					}

					// Token: 0x02003F61 RID: 16225
					public class CANTALOUPE
					{
						// Token: 0x0400F78F RID: 63375
						public static LocString NAME = "Rocketmelon Atmo Helmet";

						// Token: 0x0400F790 RID: 63376
						public static LocString DESC = "A melon for your melon.";
					}

					// Token: 0x02003F62 RID: 16226
					public class MONDRIAN_BLUE_RED_YELLOW
					{
						// Token: 0x0400F791 RID: 63377
						public static LocString NAME = "Cubist Atmo Helmet";

						// Token: 0x0400F792 RID: 63378
						public static LocString DESC = "Abstract geometrics are both hip <i>and</i> square.";
					}

					// Token: 0x02003F63 RID: 16227
					public class OVERALLS_RED
					{
						// Token: 0x0400F793 RID: 63379
						public static LocString NAME = "Spiffy Atmo Helmet";

						// Token: 0x0400F794 RID: 63380
						public static LocString DESC = "The twin antennae serve as an early warning system for low ceilings.";
					}
				}
			}

			// Token: 0x02002E13 RID: 11795
			public class ATMO_SUIT_BODY
			{
				// Token: 0x0400C6C6 RID: 50886
				public static LocString NAME = "Default Atmo Uniform";

				// Token: 0x0400C6C7 RID: 50887
				public static LocString DESC = "Default top and bottom of an atmo suit.";

				// Token: 0x02003B08 RID: 15112
				public class FACADES
				{
					// Token: 0x02003F64 RID: 16228
					public class SPARKLE_RED
					{
						// Token: 0x0400F795 RID: 63381
						public static LocString NAME = "Red Glitter Atmo Suit";

						// Token: 0x0400F796 RID: 63382
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02003F65 RID: 16229
					public class SPARKLE_GREEN
					{
						// Token: 0x0400F797 RID: 63383
						public static LocString NAME = "Green Glitter Atmo Suit";

						// Token: 0x0400F798 RID: 63384
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02003F66 RID: 16230
					public class SPARKLE_BLUE
					{
						// Token: 0x0400F799 RID: 63385
						public static LocString NAME = "Blue Glitter Atmo Suit";

						// Token: 0x0400F79A RID: 63386
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02003F67 RID: 16231
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400F79B RID: 63387
						public static LocString NAME = "Violet Glitter Atmo Suit";

						// Token: 0x0400F79C RID: 63388
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02003F68 RID: 16232
					public class LIMONE
					{
						// Token: 0x0400F79D RID: 63389
						public static LocString NAME = "Citrus Atmo Suit";

						// Token: 0x0400F79E RID: 63390
						public static LocString DESC = "Perfect for summery, atmospheric excursions.";
					}

					// Token: 0x02003F69 RID: 16233
					public class PUFT
					{
						// Token: 0x0400F79F RID: 63391
						public static LocString NAME = "Puft Atmo Suit";

						// Token: 0x0400F7A0 RID: 63392
						public static LocString DESC = "Warning: prolonged wear may result in feelings of Puft-up pride.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02003F6A RID: 16234
					public class BASIC_PURPLE
					{
						// Token: 0x0400F7A1 RID: 63393
						public static LocString NAME = "Crisp Eggplant Atmo Suit";

						// Token: 0x0400F7A2 RID: 63394
						public static LocString DESC = "It really emphasizes wide shoulders.";
					}

					// Token: 0x02003F6B RID: 16235
					public class PRINT_TRIANGLES_TURQ
					{
						// Token: 0x0400F7A3 RID: 63395
						public static LocString NAME = "Confetti Atmo Suit";

						// Token: 0x0400F7A4 RID: 63396
						public static LocString DESC = "It puts the \"fun\" in \"perfunctory nods to personnel individuality\"!";
					}

					// Token: 0x02003F6C RID: 16236
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400F7A5 RID: 63397
						public static LocString NAME = "Crisp Neon Pink Atmo Suit";

						// Token: 0x0400F7A6 RID: 63398
						public static LocString DESC = "The neck is a little snug.";
					}

					// Token: 0x02003F6D RID: 16237
					public class MULTI_RED_BLACK
					{
						// Token: 0x0400F7A7 RID: 63399
						public static LocString NAME = "Red-bellied Atmo Suit";

						// Token: 0x0400F7A8 RID: 63400
						public static LocString DESC = "It really highlights the midsection.";
					}

					// Token: 0x02003F6E RID: 16238
					public class CANTALOUPE
					{
						// Token: 0x0400F7A9 RID: 63401
						public static LocString NAME = "Rocketmelon Atmo Suit";

						// Token: 0x0400F7AA RID: 63402
						public static LocString DESC = "It starts to smell ripe pretty quickly.";
					}

					// Token: 0x02003F6F RID: 16239
					public class MULTI_BLUE_GREY_BLACK
					{
						// Token: 0x0400F7AB RID: 63403
						public static LocString NAME = "Swagger Atmo Suit";

						// Token: 0x0400F7AC RID: 63404
						public static LocString DESC = "Engineered to resemble stonewashed denim and black leather.";
					}

					// Token: 0x02003F70 RID: 16240
					public class MULTI_BLUE_YELLOW_RED
					{
						// Token: 0x0400F7AD RID: 63405
						public static LocString NAME = "Fundamental Stripe Atmo Suit";

						// Token: 0x0400F7AE RID: 63406
						public static LocString DESC = "Designed by the Primary Colors Appreciation Society.";
					}
				}
			}

			// Token: 0x02002E14 RID: 11796
			public class ATMO_SUIT_GLOVES
			{
				// Token: 0x0400C6C8 RID: 50888
				public static LocString NAME = "Default Atmo Gloves";

				// Token: 0x0400C6C9 RID: 50889
				public static LocString DESC = "Default atmo suit gloves.";

				// Token: 0x02003B09 RID: 15113
				public class FACADES
				{
					// Token: 0x02003F71 RID: 16241
					public class SPARKLE_RED
					{
						// Token: 0x0400F7AF RID: 63407
						public static LocString NAME = "Red Glitter Atmo Gloves";

						// Token: 0x0400F7B0 RID: 63408
						public static LocString DESC = "Sparkly red gloves for hostile environments.";
					}

					// Token: 0x02003F72 RID: 16242
					public class SPARKLE_GREEN
					{
						// Token: 0x0400F7B1 RID: 63409
						public static LocString NAME = "Green Glitter Atmo Gloves";

						// Token: 0x0400F7B2 RID: 63410
						public static LocString DESC = "Sparkly green gloves for hostile environments.";
					}

					// Token: 0x02003F73 RID: 16243
					public class SPARKLE_BLUE
					{
						// Token: 0x0400F7B3 RID: 63411
						public static LocString NAME = "Blue Glitter Atmo Gloves";

						// Token: 0x0400F7B4 RID: 63412
						public static LocString DESC = "Sparkly blue gloves for hostile environments.";
					}

					// Token: 0x02003F74 RID: 16244
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400F7B5 RID: 63413
						public static LocString NAME = "Violet Glitter Atmo Gloves";

						// Token: 0x0400F7B6 RID: 63414
						public static LocString DESC = "Sparkly violet gloves for hostile environments.";
					}

					// Token: 0x02003F75 RID: 16245
					public class LIMONE
					{
						// Token: 0x0400F7B7 RID: 63415
						public static LocString NAME = "Citrus Atmo Gloves";

						// Token: 0x0400F7B8 RID: 63416
						public static LocString DESC = "Lime-inspired gloves brighten up hostile environments.";
					}

					// Token: 0x02003F76 RID: 16246
					public class PUFT
					{
						// Token: 0x0400F7B9 RID: 63417
						public static LocString NAME = "Puft Atmo Gloves";

						// Token: 0x0400F7BA RID: 63418
						public static LocString DESC = "A little Puft-love for delicate extremities.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02003F77 RID: 16247
					public class GOLD
					{
						// Token: 0x0400F7BB RID: 63419
						public static LocString NAME = "Gold Atmo Gloves";

						// Token: 0x0400F7BC RID: 63420
						public static LocString DESC = "A golden touch! Without all the Midas-type baggage.";
					}

					// Token: 0x02003F78 RID: 16248
					public class PURPLE
					{
						// Token: 0x0400F7BD RID: 63421
						public static LocString NAME = "Eggplant Atmo Gloves";

						// Token: 0x0400F7BE RID: 63422
						public static LocString DESC = "Fab purple gloves for hostile environments.";
					}

					// Token: 0x02003F79 RID: 16249
					public class WHITE
					{
						// Token: 0x0400F7BF RID: 63423
						public static LocString NAME = "White Atmo Gloves";

						// Token: 0x0400F7C0 RID: 63424
						public static LocString DESC = "For the Duplicant who never gets their hands dirty.";
					}

					// Token: 0x02003F7A RID: 16250
					public class STRIPES_LAVENDER
					{
						// Token: 0x0400F7C1 RID: 63425
						public static LocString NAME = "Wildberry Atmo Gloves";

						// Token: 0x0400F7C2 RID: 63426
						public static LocString DESC = "Functional finger-protectors with fruity flair.";
					}

					// Token: 0x02003F7B RID: 16251
					public class CANTALOUPE
					{
						// Token: 0x0400F7C3 RID: 63427
						public static LocString NAME = "Rocketmelon Atmo Gloves";

						// Token: 0x0400F7C4 RID: 63428
						public static LocString DESC = "It takes eighteen melon rinds to make a single glove.";
					}

					// Token: 0x02003F7C RID: 16252
					public class BROWN
					{
						// Token: 0x0400F7C5 RID: 63429
						public static LocString NAME = "Leather Atmo Gloves";

						// Token: 0x0400F7C6 RID: 63430
						public static LocString DESC = "They creak rather loudly during the break-in period.";
					}
				}
			}

			// Token: 0x02002E15 RID: 11797
			public class ATMO_SUIT_BELT
			{
				// Token: 0x0400C6CA RID: 50890
				public static LocString NAME = "Default Atmo Belt";

				// Token: 0x0400C6CB RID: 50891
				public static LocString DESC = "Default belt for atmo suits.";

				// Token: 0x02003B0A RID: 15114
				public class FACADES
				{
					// Token: 0x02003F7D RID: 16253
					public class SPARKLE_RED
					{
						// Token: 0x0400F7C7 RID: 63431
						public static LocString NAME = "Red Glitter Atmo Belt";

						// Token: 0x0400F7C8 RID: 63432
						public static LocString DESC = "It's red! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x02003F7E RID: 16254
					public class SPARKLE_GREEN
					{
						// Token: 0x0400F7C9 RID: 63433
						public static LocString NAME = "Green Glitter Atmo Belt";

						// Token: 0x0400F7CA RID: 63434
						public static LocString DESC = "It's green! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x02003F7F RID: 16255
					public class SPARKLE_BLUE
					{
						// Token: 0x0400F7CB RID: 63435
						public static LocString NAME = "Blue Glitter Atmo Belt";

						// Token: 0x0400F7CC RID: 63436
						public static LocString DESC = "It's blue! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x02003F80 RID: 16256
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400F7CD RID: 63437
						public static LocString NAME = "Violet Glitter Atmo Belt";

						// Token: 0x0400F7CE RID: 63438
						public static LocString DESC = "It's violet! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x02003F81 RID: 16257
					public class LIMONE
					{
						// Token: 0x0400F7CF RID: 63439
						public static LocString NAME = "Citrus Atmo Belt";

						// Token: 0x0400F7D0 RID: 63440
						public static LocString DESC = "This lime-hued belt really pulls an atmo suit together.";
					}

					// Token: 0x02003F82 RID: 16258
					public class PUFT
					{
						// Token: 0x0400F7D1 RID: 63441
						public static LocString NAME = "Puft Atmo Belt";

						// Token: 0x0400F7D2 RID: 63442
						public static LocString DESC = "If critters wore belts...\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02003F83 RID: 16259
					public class TWOTONE_PURPLE
					{
						// Token: 0x0400F7D3 RID: 63443
						public static LocString NAME = "Eggplant Atmo Belt";

						// Token: 0x0400F7D4 RID: 63444
						public static LocString DESC = "In the more pretentious space-fashion circles, it's known as \"aubergine.\"";
					}

					// Token: 0x02003F84 RID: 16260
					public class BASIC_GOLD
					{
						// Token: 0x0400F7D5 RID: 63445
						public static LocString NAME = "Gold Atmo Belt";

						// Token: 0x0400F7D6 RID: 63446
						public static LocString DESC = "Better to be overdressed than underdressed.";
					}

					// Token: 0x02003F85 RID: 16261
					public class BASIC_GREY
					{
						// Token: 0x0400F7D7 RID: 63447
						public static LocString NAME = "Slate Atmo Belt";

						// Token: 0x0400F7D8 RID: 63448
						public static LocString DESC = "Slick and understated space style.";
					}

					// Token: 0x02003F86 RID: 16262
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400F7D9 RID: 63449
						public static LocString NAME = "Neon Pink Atmo Belt";

						// Token: 0x0400F7DA RID: 63450
						public static LocString DESC = "Visible from several planetoids away.";
					}

					// Token: 0x02003F87 RID: 16263
					public class CANTALOUPE
					{
						// Token: 0x0400F7DB RID: 63451
						public static LocString NAME = "Rocketmelon Atmo Belt";

						// Token: 0x0400F7DC RID: 63452
						public static LocString DESC = "A tribute to the <i>cucumis melo cantalupensis</i>.";
					}

					// Token: 0x02003F88 RID: 16264
					public class TWOTONE_BROWN
					{
						// Token: 0x0400F7DD RID: 63453
						public static LocString NAME = "Leather Atmo Belt";

						// Token: 0x0400F7DE RID: 63454
						public static LocString DESC = "Crafted from the tanned hide of a thick-skinned critter.";
					}
				}
			}

			// Token: 0x02002E16 RID: 11798
			public class ATMO_SUIT_SHOES
			{
				// Token: 0x0400C6CC RID: 50892
				public static LocString NAME = "Default Atmo Boots";

				// Token: 0x0400C6CD RID: 50893
				public static LocString DESC = "Default footwear for atmo suits.";

				// Token: 0x02003B0B RID: 15115
				public class FACADES
				{
					// Token: 0x02003F89 RID: 16265
					public class LIMONE
					{
						// Token: 0x0400F7DF RID: 63455
						public static LocString NAME = "Citrus Atmo Boots";

						// Token: 0x0400F7E0 RID: 63456
						public static LocString DESC = "Cheery boots for stomping around in hostile environments.";
					}

					// Token: 0x02003F8A RID: 16266
					public class PUFT
					{
						// Token: 0x0400F7E1 RID: 63457
						public static LocString NAME = "Puft Atmo Boots";

						// Token: 0x0400F7E2 RID: 63458
						public static LocString DESC = "These boots were made for puft-ing.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02003F8B RID: 16267
					public class SPARKLE_BLACK
					{
						// Token: 0x0400F7E3 RID: 63459
						public static LocString NAME = "Black Glitter Atmo Boots";

						// Token: 0x0400F7E4 RID: 63460
						public static LocString DESC = "A timeless color, with a little pizzazz.";
					}

					// Token: 0x02003F8C RID: 16268
					public class BASIC_BLACK
					{
						// Token: 0x0400F7E5 RID: 63461
						public static LocString NAME = "Stealth Atmo Boots";

						// Token: 0x0400F7E6 RID: 63462
						public static LocString DESC = "They attract no attention at all.";
					}

					// Token: 0x02003F8D RID: 16269
					public class BASIC_PURPLE
					{
						// Token: 0x0400F7E7 RID: 63463
						public static LocString NAME = "Eggplant Atmo Boots";

						// Token: 0x0400F7E8 RID: 63464
						public static LocString DESC = "Purple boots for stomping around in hostile environments.";
					}

					// Token: 0x02003F8E RID: 16270
					public class BASIC_LAVENDER
					{
						// Token: 0x0400F7E9 RID: 63465
						public static LocString NAME = "Lavender Atmo Boots";

						// Token: 0x0400F7EA RID: 63466
						public static LocString DESC = "Soothing space booties for tired feet.";
					}

					// Token: 0x02003F8F RID: 16271
					public class CANTALOUPE
					{
						// Token: 0x0400F7EB RID: 63467
						public static LocString NAME = "Rocketmelon Atmo Boots";

						// Token: 0x0400F7EC RID: 63468
						public static LocString DESC = "Keeps feet safe (and juicy) in hostile environments.";
					}
				}
			}

			// Token: 0x02002E17 RID: 11799
			public class JET_SUIT_SHOES
			{
				// Token: 0x0400C6CE RID: 50894
				public static LocString NAME = "Default Jet Boots";

				// Token: 0x0400C6CF RID: 50895
				public static LocString DESC = "Default footwear for jet suits.";
			}

			// Token: 0x02002E18 RID: 11800
			public class JET_SUIT_HELMET
			{
				// Token: 0x0400C6D0 RID: 50896
				public static LocString NAME = "Default Jet Helmet";

				// Token: 0x0400C6D1 RID: 50897
				public static LocString DESC = "Default helmet for jet suits.";
			}

			// Token: 0x02002E19 RID: 11801
			public class JET_SUIT_BODY
			{
				// Token: 0x0400C6D2 RID: 50898
				public static LocString NAME = "Default Jet Uniform";

				// Token: 0x0400C6D3 RID: 50899
				public static LocString DESC = "Default top and bottom of a jet suit.";
			}

			// Token: 0x02002E1A RID: 11802
			public class JET_SUIT_GLOVES
			{
				// Token: 0x0400C6D4 RID: 50900
				public static LocString NAME = "Default Jet Gloves";

				// Token: 0x0400C6D5 RID: 50901
				public static LocString DESC = "Default gloves for jet suits.";
			}

			// Token: 0x02002E1B RID: 11803
			public class AQUA_SUIT
			{
				// Token: 0x0400C6D6 RID: 50902
				public static LocString NAME = UI.FormatAsLink("Aqua Suit", "AQUA_SUIT");

				// Token: 0x0400C6D7 RID: 50903
				public static LocString DESC = "Because breathing underwater is better than... not.";

				// Token: 0x0400C6D8 RID: 50904
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400C6D9 RID: 50905
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.";

				// Token: 0x0400C6DA RID: 50906
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "AQUA_SUIT");

				// Token: 0x0400C6DB RID: 50907
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Aqua Suit", "AQUA_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02002E1C RID: 11804
			public class TEMPERATURE_SUIT
			{
				// Token: 0x0400C6DC RID: 50908
				public static LocString NAME = UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400C6DD RID: 50909
				public static LocString DESC = "Keeps my Duplicants cool in case things heat up.";

				// Token: 0x0400C6DE RID: 50910
				public static LocString EFFECT = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.\n\nMust be powered at a Thermo Suit Dock when depleted.";

				// Token: 0x0400C6DF RID: 50911
				public static LocString RECIPE_DESC = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.";

				// Token: 0x0400C6E0 RID: 50912
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400C6E1 RID: 50913
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02002E1D RID: 11805
			public class JET_SUIT
			{
				// Token: 0x0400C6E2 RID: 50914
				public static LocString NAME = UI.FormatAsLink("Jet Suit", "JET_SUIT");

				// Token: 0x0400C6E3 RID: 50915
				public static LocString DESC = "Allows my Duplicants to take to the skies, for a time.";

				// Token: 0x0400C6E4 RID: 50916
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					MISC.TAGS.COMBUSTIBLELIQUID,
					" at a ",
					UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400C6E5 RID: 50917
				public static LocString RECIPE_DESC = "Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " in toxic and low breathability environments.\n\nAllows Duplicant flight.";

				// Token: 0x0400C6E6 RID: 50918
				public static LocString GENERICNAME = "Jet Suit";

				// Token: 0x0400C6E7 RID: 50919
				public static LocString TANK_EFFECT_NAME = "Fuel Tank";

				// Token: 0x0400C6E8 RID: 50920
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Jet Suit", "JET_SUIT");

				// Token: 0x0400C6E9 RID: 50921
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Jet Suit", "JET_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});

				// Token: 0x0400C6EA RID: 50922
				public static LocString REPAIR_WORN_RECIPE_NAME = "Repair " + EQUIPMENT.PREFABS.JET_SUIT.NAME;

				// Token: 0x0400C6EB RID: 50923
				public static LocString REPAIR_WORN_DESC = "Restore a " + UI.FormatAsLink("Worn Jet Suit", "JET_SUIT") + " to working order.";
			}

			// Token: 0x02002E1E RID: 11806
			public class LEAD_SUIT
			{
				// Token: 0x0400C6EC RID: 50924
				public static LocString NAME = UI.FormatAsLink("Lead Suit", "LEAD_SUIT");

				// Token: 0x0400C6ED RID: 50925
				public static LocString DESC = "Because exposure to radiation doesn't grant Duplicants superpowers.";

				// Token: 0x0400C6EE RID: 50926
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and protection in areas with ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					".\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at a ",
					UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400C6EF RID: 50927
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nProtects Duplicants from ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					"."
				});

				// Token: 0x0400C6F0 RID: 50928
				public static LocString GENERICNAME = "Lead Suit";

				// Token: 0x0400C6F1 RID: 50929
				public static LocString BATTERY_EFFECT_NAME = "Suit Battery";

				// Token: 0x0400C6F2 RID: 50930
				public static LocString SUIT_OUT_OF_BATTERIES = "Suit Batteries Empty";

				// Token: 0x0400C6F3 RID: 50931
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "LEAD_SUIT");

				// Token: 0x0400C6F4 RID: 50932
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Lead Suit", "LEAD_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});

				// Token: 0x0400C6F5 RID: 50933
				public static LocString REPAIR_WORN_RECIPE_NAME = "Repair " + EQUIPMENT.PREFABS.LEAD_SUIT.NAME;

				// Token: 0x0400C6F6 RID: 50934
				public static LocString REPAIR_WORN_DESC = "Restore a " + UI.FormatAsLink("Worn Lead Suit", "LEAD_SUIT") + " to working order.";
			}

			// Token: 0x02002E1F RID: 11807
			public class COOL_VEST
			{
				// Token: 0x0400C6F7 RID: 50935
				public static LocString NAME = UI.FormatAsLink("Cool Vest", "COOL_VEST");

				// Token: 0x0400C6F8 RID: 50936
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400C6F9 RID: 50937
				public static LocString DESC = "Don't sweat it!";

				// Token: 0x0400C6FA RID: 50938
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation.";

				// Token: 0x0400C6FB RID: 50939
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation";
			}

			// Token: 0x02002E20 RID: 11808
			public class WARM_VEST
			{
				// Token: 0x0400C6FC RID: 50940
				public static LocString NAME = UI.FormatAsLink("Warm Coat", "WARM_VEST");

				// Token: 0x0400C6FD RID: 50941
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400C6FE RID: 50942
				public static LocString DESC = "Happiness is a warm Duplicant.";

				// Token: 0x0400C6FF RID: 50943
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation.";

				// Token: 0x0400C700 RID: 50944
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation";
			}

			// Token: 0x02002E21 RID: 11809
			public class FUNKY_VEST
			{
				// Token: 0x0400C701 RID: 50945
				public static LocString NAME = UI.FormatAsLink("Snazzy Suit", "FUNKY_VEST");

				// Token: 0x0400C702 RID: 50946
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400C703 RID: 50947
				public static LocString DESC = "This transforms my Duplicant into a walking beacon of charm and style.";

				// Token: 0x0400C704 RID: 50948
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases Decor in a small area effect around the wearer. Can be upgraded to ",
					UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING"),
					" at the ",
					UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION"),
					"."
				});

				// Token: 0x0400C705 RID: 50949
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer. Can be upgraded to " + UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING") + " at the " + UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION");
			}

			// Token: 0x02002E22 RID: 11810
			public class CUSTOMCLOTHING
			{
				// Token: 0x0400C706 RID: 50950
				public static LocString NAME = UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING");

				// Token: 0x0400C707 RID: 50951
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400C708 RID: 50952
				public static LocString DESC = "This transforms my Duplicant into a colony-inspiring fashion icon.";

				// Token: 0x0400C709 RID: 50953
				public static LocString EFFECT = "Increases Decor in a small area effect around the wearer.";

				// Token: 0x0400C70A RID: 50954
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer";

				// Token: 0x02003B0C RID: 15116
				public class FACADES
				{
					// Token: 0x0400ED19 RID: 60697
					public static LocString CLUBSHIRT = UI.FormatAsLink("Purple Polyester Suit", "CUSTOMCLOTHING");

					// Token: 0x0400ED1A RID: 60698
					public static LocString CUMMERBUND = UI.FormatAsLink("Classic Cummerbund", "CUSTOMCLOTHING");

					// Token: 0x0400ED1B RID: 60699
					public static LocString DECOR_02 = UI.FormatAsLink("Snazzier Red Suit", "CUSTOMCLOTHING");

					// Token: 0x0400ED1C RID: 60700
					public static LocString DECOR_03 = UI.FormatAsLink("Snazzier Blue Suit", "CUSTOMCLOTHING");

					// Token: 0x0400ED1D RID: 60701
					public static LocString DECOR_04 = UI.FormatAsLink("Snazzier Green Suit", "CUSTOMCLOTHING");

					// Token: 0x0400ED1E RID: 60702
					public static LocString DECOR_05 = UI.FormatAsLink("Snazzier Violet Suit", "CUSTOMCLOTHING");

					// Token: 0x0400ED1F RID: 60703
					public static LocString GAUDYSWEATER = UI.FormatAsLink("Pompom Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400ED20 RID: 60704
					public static LocString LIMONE = UI.FormatAsLink("Citrus Spandex Suit", "CUSTOMCLOTHING");

					// Token: 0x0400ED21 RID: 60705
					public static LocString MONDRIAN = UI.FormatAsLink("Cubist Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400ED22 RID: 60706
					public static LocString OVERALLS = UI.FormatAsLink("Spiffy Overalls", "CUSTOMCLOTHING");

					// Token: 0x0400ED23 RID: 60707
					public static LocString TRIANGLES = UI.FormatAsLink("Confetti Suit", "CUSTOMCLOTHING");

					// Token: 0x0400ED24 RID: 60708
					public static LocString WORKOUT = UI.FormatAsLink("Pink Unitard", "CUSTOMCLOTHING");
				}
			}

			// Token: 0x02002E23 RID: 11811
			public class CLOTHING_GLOVES
			{
				// Token: 0x0400C70B RID: 50955
				public static LocString NAME = "Default Gloves";

				// Token: 0x0400C70C RID: 50956
				public static LocString DESC = "The default gloves.";

				// Token: 0x02003B0D RID: 15117
				public class FACADES
				{
					// Token: 0x02003F90 RID: 16272
					public class STANDARD_GOLD
					{
						// Token: 0x0400F7ED RID: 63469
						public static LocString NAME = "Standard Gloves";

						// Token: 0x0400F7EE RID: 63470
						public static LocString DESC = "Standard-issue gloves for colony workers.";
					}

					// Token: 0x02003F91 RID: 16273
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400F7EF RID: 63471
						public static LocString NAME = "Basic Aqua Gloves";

						// Token: 0x0400F7F0 RID: 63472
						public static LocString DESC = "A good, solid pair of aqua-blue gloves that go with everything.";
					}

					// Token: 0x02003F92 RID: 16274
					public class BASIC_YELLOW
					{
						// Token: 0x0400F7F1 RID: 63473
						public static LocString NAME = "Basic Yellow Gloves";

						// Token: 0x0400F7F2 RID: 63474
						public static LocString DESC = "A good, solid pair of yellow gloves that go with everything.";
					}

					// Token: 0x02003F93 RID: 16275
					public class BASIC_BLACK
					{
						// Token: 0x0400F7F3 RID: 63475
						public static LocString NAME = "Basic Black Gloves";

						// Token: 0x0400F7F4 RID: 63476
						public static LocString DESC = "A good, solid pair of black gloves that go with everything.";
					}

					// Token: 0x02003F94 RID: 16276
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400F7F5 RID: 63477
						public static LocString NAME = "Basic Bubblegum Gloves";

						// Token: 0x0400F7F6 RID: 63478
						public static LocString DESC = "A good, solid pair of bubblegum-pink gloves that go with everything.";
					}

					// Token: 0x02003F95 RID: 16277
					public class BASIC_GREEN
					{
						// Token: 0x0400F7F7 RID: 63479
						public static LocString NAME = "Basic Green Gloves";

						// Token: 0x0400F7F8 RID: 63480
						public static LocString DESC = "A good, solid pair of green gloves that go with everything.";
					}

					// Token: 0x02003F96 RID: 16278
					public class BASIC_ORANGE
					{
						// Token: 0x0400F7F9 RID: 63481
						public static LocString NAME = "Basic Orange Gloves";

						// Token: 0x0400F7FA RID: 63482
						public static LocString DESC = "A good, solid pair of orange gloves that go with everything.";
					}

					// Token: 0x02003F97 RID: 16279
					public class BASIC_PURPLE
					{
						// Token: 0x0400F7FB RID: 63483
						public static LocString NAME = "Basic Purple Gloves";

						// Token: 0x0400F7FC RID: 63484
						public static LocString DESC = "A good, solid pair of purple gloves that go with everything.";
					}

					// Token: 0x02003F98 RID: 16280
					public class BASIC_RED
					{
						// Token: 0x0400F7FD RID: 63485
						public static LocString NAME = "Basic Red Gloves";

						// Token: 0x0400F7FE RID: 63486
						public static LocString DESC = "A good, solid pair of red gloves that go with everything.";
					}

					// Token: 0x02003F99 RID: 16281
					public class BASIC_WHITE
					{
						// Token: 0x0400F7FF RID: 63487
						public static LocString NAME = "Basic White Gloves";

						// Token: 0x0400F800 RID: 63488
						public static LocString DESC = "A good, solid pair of white gloves that go with everything.";
					}

					// Token: 0x02003F9A RID: 16282
					public class GLOVES_ATHLETIC_DEEPRED
					{
						// Token: 0x0400F801 RID: 63489
						public static LocString NAME = "Team Captain Sports Gloves";

						// Token: 0x0400F802 RID: 63490
						public static LocString DESC = "Red-striped gloves for winning at any activity.";
					}

					// Token: 0x02003F9B RID: 16283
					public class GLOVES_ATHLETIC_SATSUMA
					{
						// Token: 0x0400F803 RID: 63491
						public static LocString NAME = "Superfan Sports Gloves";

						// Token: 0x0400F804 RID: 63492
						public static LocString DESC = "Orange-striped gloves for enthusiastic athletes.";
					}

					// Token: 0x02003F9C RID: 16284
					public class GLOVES_ATHLETIC_LEMON
					{
						// Token: 0x0400F805 RID: 63493
						public static LocString NAME = "Hype Sports Gloves";

						// Token: 0x0400F806 RID: 63494
						public static LocString DESC = "Yellow-striped gloves for athletes who seek to raise the bar.";
					}

					// Token: 0x02003F9D RID: 16285
					public class GLOVES_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400F807 RID: 63495
						public static LocString NAME = "Go Team Sports Gloves";

						// Token: 0x0400F808 RID: 63496
						public static LocString DESC = "Green-striped gloves for the perenially good sport.";
					}

					// Token: 0x02003F9E RID: 16286
					public class GLOVES_ATHLETIC_COBALT
					{
						// Token: 0x0400F809 RID: 63497
						public static LocString NAME = "True Blue Sports Gloves";

						// Token: 0x0400F80A RID: 63498
						public static LocString DESC = "Blue-striped gloves perfect for shaking hands after the game.";
					}

					// Token: 0x02003F9F RID: 16287
					public class GLOVES_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400F80B RID: 63499
						public static LocString NAME = "Pep Rally Sports Gloves";

						// Token: 0x0400F80C RID: 63500
						public static LocString DESC = "Pink-striped glove designed to withstand countless high-fives.";
					}

					// Token: 0x02003FA0 RID: 16288
					public class GLOVES_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400F80D RID: 63501
						public static LocString NAME = "Underdog Sports Gloves";

						// Token: 0x0400F80E RID: 63502
						public static LocString DESC = "The muted stripe minimizes distractions so its wearer can focus on trying very, very hard.";
					}

					// Token: 0x02003FA1 RID: 16289
					public class CUFFLESS_BLUEBERRY
					{
						// Token: 0x0400F80F RID: 63503
						public static LocString NAME = "Blueberry Glovelets";

						// Token: 0x0400F810 RID: 63504
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003FA2 RID: 16290
					public class CUFFLESS_GRAPE
					{
						// Token: 0x0400F811 RID: 63505
						public static LocString NAME = "Grape Glovelets";

						// Token: 0x0400F812 RID: 63506
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003FA3 RID: 16291
					public class CUFFLESS_LEMON
					{
						// Token: 0x0400F813 RID: 63507
						public static LocString NAME = "Lemon Glovelets";

						// Token: 0x0400F814 RID: 63508
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003FA4 RID: 16292
					public class CUFFLESS_LIME
					{
						// Token: 0x0400F815 RID: 63509
						public static LocString NAME = "Lime Glovelets";

						// Token: 0x0400F816 RID: 63510
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003FA5 RID: 16293
					public class CUFFLESS_SATSUMA
					{
						// Token: 0x0400F817 RID: 63511
						public static LocString NAME = "Satsuma Glovelets";

						// Token: 0x0400F818 RID: 63512
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003FA6 RID: 16294
					public class CUFFLESS_STRAWBERRY
					{
						// Token: 0x0400F819 RID: 63513
						public static LocString NAME = "Strawberry Glovelets";

						// Token: 0x0400F81A RID: 63514
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003FA7 RID: 16295
					public class CUFFLESS_WATERMELON
					{
						// Token: 0x0400F81B RID: 63515
						public static LocString NAME = "Watermelon Glovelets";

						// Token: 0x0400F81C RID: 63516
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003FA8 RID: 16296
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400F81D RID: 63517
						public static LocString NAME = "LED Gloves";

						// Token: 0x0400F81E RID: 63518
						public static LocString DESC = "Great for gesticulating at parties.";
					}

					// Token: 0x02003FA9 RID: 16297
					public class ATHLETE
					{
						// Token: 0x0400F81F RID: 63519
						public static LocString NAME = "Racing Gloves";

						// Token: 0x0400F820 RID: 63520
						public static LocString DESC = "Crafted for high-speed handshakes.";
					}

					// Token: 0x02003FAA RID: 16298
					public class BASIC_BROWN_KHAKI
					{
						// Token: 0x0400F821 RID: 63521
						public static LocString NAME = "Basic Khaki Gloves";

						// Token: 0x0400F822 RID: 63522
						public static LocString DESC = "They don't show dirt.";
					}

					// Token: 0x02003FAB RID: 16299
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400F823 RID: 63523
						public static LocString NAME = "Basic Gunmetal Gloves";

						// Token: 0x0400F824 RID: 63524
						public static LocString DESC = "A tough name for soft gloves.";
					}

					// Token: 0x02003FAC RID: 16300
					public class CUFFLESS_BLACK
					{
						// Token: 0x0400F825 RID: 63525
						public static LocString NAME = "Stealth Glovelets";

						// Token: 0x0400F826 RID: 63526
						public static LocString DESC = "It's easy to forget they're even on.";
					}

					// Token: 0x02003FAD RID: 16301
					public class DENIM_BLUE
					{
						// Token: 0x0400F827 RID: 63527
						public static LocString NAME = "Denim Gloves";

						// Token: 0x0400F828 RID: 63528
						public static LocString DESC = "They're not great for dexterity.";
					}

					// Token: 0x02003FAE RID: 16302
					public class BASIC_GREY
					{
						// Token: 0x0400F829 RID: 63529
						public static LocString NAME = "Basic Gray Gloves";

						// Token: 0x0400F82A RID: 63530
						public static LocString DESC = "A good, solid pair of gray gloves that go with everything.";
					}

					// Token: 0x02003FAF RID: 16303
					public class BASIC_PINKSALMON
					{
						// Token: 0x0400F82B RID: 63531
						public static LocString NAME = "Basic Coral Gloves";

						// Token: 0x0400F82C RID: 63532
						public static LocString DESC = "A good, solid pair of bright pink gloves that go with everything.";
					}

					// Token: 0x02003FB0 RID: 16304
					public class BASIC_TAN
					{
						// Token: 0x0400F82D RID: 63533
						public static LocString NAME = "Basic Tan Gloves";

						// Token: 0x0400F82E RID: 63534
						public static LocString DESC = "A good, solid pair of tan gloves that go with everything.";
					}

					// Token: 0x02003FB1 RID: 16305
					public class BALLERINA_PINK
					{
						// Token: 0x0400F82F RID: 63535
						public static LocString NAME = "Ballet Gloves";

						// Token: 0x0400F830 RID: 63536
						public static LocString DESC = "Wrist ruffles highlight the poetic movements of the phalanges.";
					}

					// Token: 0x02003FB2 RID: 16306
					public class FORMAL_WHITE
					{
						// Token: 0x0400F831 RID: 63537
						public static LocString NAME = "White Silk Gloves";

						// Token: 0x0400F832 RID: 63538
						public static LocString DESC = "They're as soft as...well, silk.";
					}

					// Token: 0x02003FB3 RID: 16307
					public class LONG_WHITE
					{
						// Token: 0x0400F833 RID: 63539
						public static LocString NAME = "White Evening Gloves";

						// Token: 0x0400F834 RID: 63540
						public static LocString DESC = "Super-long gloves for super-formal occasions.";
					}

					// Token: 0x02003FB4 RID: 16308
					public class TWOTONE_CREAM_CHARCOAL
					{
						// Token: 0x0400F835 RID: 63541
						public static LocString NAME = "Contrast Cuff Gloves";

						// Token: 0x0400F836 RID: 63542
						public static LocString DESC = "For elegance so understated, it may go completely unnoticed.";
					}

					// Token: 0x02003FB5 RID: 16309
					public class SOCKSUIT_BEIGE
					{
						// Token: 0x0400F837 RID: 63543
						public static LocString NAME = "Vintage Handsock";

						// Token: 0x0400F838 RID: 63544
						public static LocString DESC = "Designed by someone with cold hands and an excess of old socks.";
					}

					// Token: 0x02003FB6 RID: 16310
					public class BASIC_SLATE
					{
						// Token: 0x0400F839 RID: 63545
						public static LocString NAME = "Basic Slate Gloves";

						// Token: 0x0400F83A RID: 63546
						public static LocString DESC = "A good, solid pair of slate gloves that go with everything.";
					}

					// Token: 0x02003FB7 RID: 16311
					public class KNIT_GOLD
					{
						// Token: 0x0400F83B RID: 63547
						public static LocString NAME = "Gold Knit Gloves";

						// Token: 0x0400F83C RID: 63548
						public static LocString DESC = "Produces a pleasantly muffled \"whump\" when high-fiving.";
					}

					// Token: 0x02003FB8 RID: 16312
					public class KNIT_MAGENTA
					{
						// Token: 0x0400F83D RID: 63549
						public static LocString NAME = "Magenta Knit Gloves";

						// Token: 0x0400F83E RID: 63550
						public static LocString DESC = "Produces a pleasantly muffled \"whump\" when high-fiving.";
					}

					// Token: 0x02003FB9 RID: 16313
					public class SPARKLE_WHITE
					{
						// Token: 0x0400F83F RID: 63551
						public static LocString NAME = "White Glitter Gloves";

						// Token: 0x0400F840 RID: 63552
						public static LocString DESC = "Each sequin was attached using sealant borrowed from the rocketry department.";
					}

					// Token: 0x02003FBA RID: 16314
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400F841 RID: 63553
						public static LocString NAME = "Frilly Saltrock Gloves";

						// Token: 0x0400F842 RID: 63554
						public static LocString DESC = "Thick, soft pink gloves with added flounce.";
					}

					// Token: 0x02003FBB RID: 16315
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400F843 RID: 63555
						public static LocString NAME = "Frilly Dusk Gloves";

						// Token: 0x0400F844 RID: 63556
						public static LocString DESC = "Thick, soft purple gloves with added flounce.";
					}

					// Token: 0x02003FBC RID: 16316
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400F845 RID: 63557
						public static LocString NAME = "Frilly Basin Gloves";

						// Token: 0x0400F846 RID: 63558
						public static LocString DESC = "Thick, soft blue gloves with added flounce.";
					}

					// Token: 0x02003FBD RID: 16317
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400F847 RID: 63559
						public static LocString NAME = "Frilly Balm Gloves";

						// Token: 0x0400F848 RID: 63560
						public static LocString DESC = "The soft teal fabric soothes hard-working hands.";
					}

					// Token: 0x02003FBE RID: 16318
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400F849 RID: 63561
						public static LocString NAME = "Frilly Leach Gloves";

						// Token: 0x0400F84A RID: 63562
						public static LocString DESC = "Thick, soft green gloves with added flounce.";
					}

					// Token: 0x02003FBF RID: 16319
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400F84B RID: 63563
						public static LocString NAME = "Frilly Yellowcake Gloves";

						// Token: 0x0400F84C RID: 63564
						public static LocString DESC = "Thick, soft yellow gloves with added flounce.";
					}

					// Token: 0x02003FC0 RID: 16320
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400F84D RID: 63565
						public static LocString NAME = "Frilly Atomic Gloves";

						// Token: 0x0400F84E RID: 63566
						public static LocString DESC = "Thick, bright orange gloves with added flounce.";
					}

					// Token: 0x02003FC1 RID: 16321
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400F84F RID: 63567
						public static LocString NAME = "Frilly Magma Gloves";

						// Token: 0x0400F850 RID: 63568
						public static LocString DESC = "Thick, soft red gloves with added flounce.";
					}

					// Token: 0x02003FC2 RID: 16322
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400F851 RID: 63569
						public static LocString NAME = "Frilly Slate Gloves";

						// Token: 0x0400F852 RID: 63570
						public static LocString DESC = "Thick, soft gray gloves with added flounce.";
					}

					// Token: 0x02003FC3 RID: 16323
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400F853 RID: 63571
						public static LocString NAME = "Frilly Charcoal Gloves";

						// Token: 0x0400F854 RID: 63572
						public static LocString DESC = "Thick, soft dark gray gloves with added flounce.";
					}
				}
			}

			// Token: 0x02002E24 RID: 11812
			public class CLOTHING_TOPS
			{
				// Token: 0x0400C70D RID: 50957
				public static LocString NAME = "Default Top";

				// Token: 0x0400C70E RID: 50958
				public static LocString DESC = "The default shirt.";

				// Token: 0x02003B0E RID: 15118
				public class FACADES
				{
					// Token: 0x02003FC4 RID: 16324
					public class STANDARD_YELLOW_TOP
					{
						// Token: 0x0400F855 RID: 63573
						public static LocString NAME = "Yellow Uniform Shirt";

						// Token: 0x0400F856 RID: 63574
						public static LocString DESC = "A standard-issue uniform shirt in flax yellow.";
					}

					// Token: 0x02003FC5 RID: 16325
					public class STANDARD_GREEN_TOP
					{
						// Token: 0x0400F857 RID: 63575
						public static LocString NAME = "Green Uniform Shirt";

						// Token: 0x0400F858 RID: 63576
						public static LocString DESC = "A standard-issue uniform shirt in swampy green.";
					}

					// Token: 0x02003FC6 RID: 16326
					public class STANDARD_RED_TOP
					{
						// Token: 0x0400F859 RID: 63577
						public static LocString NAME = "Red Uniform Shirt";

						// Token: 0x0400F85A RID: 63578
						public static LocString DESC = "A standard-issue uniform shirt in carmine red.";
					}

					// Token: 0x02003FC7 RID: 16327
					public class STANDARD_BLUE_TOP
					{
						// Token: 0x0400F85B RID: 63579
						public static LocString NAME = "Blue Uniform Shirt";

						// Token: 0x0400F85C RID: 63580
						public static LocString DESC = "A standard-issue uniform shirt in a standard-issue blue hue.";
					}

					// Token: 0x02003FC8 RID: 16328
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400F85D RID: 63581
						public static LocString NAME = "Basic Aqua Shirt";

						// Token: 0x0400F85E RID: 63582
						public static LocString DESC = "A nice aqua-blue shirt that goes with everything.";
					}

					// Token: 0x02003FC9 RID: 16329
					public class BASIC_BLACK
					{
						// Token: 0x0400F85F RID: 63583
						public static LocString NAME = "Basic Black Shirt";

						// Token: 0x0400F860 RID: 63584
						public static LocString DESC = "A nice black shirt that goes with everything.";
					}

					// Token: 0x02003FCA RID: 16330
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400F861 RID: 63585
						public static LocString NAME = "Basic Bubblegum Shirt";

						// Token: 0x0400F862 RID: 63586
						public static LocString DESC = "A nice bubblegum-pink shirt that goes with everything.";
					}

					// Token: 0x02003FCB RID: 16331
					public class BASIC_GREEN
					{
						// Token: 0x0400F863 RID: 63587
						public static LocString NAME = "Basic Green Shirt";

						// Token: 0x0400F864 RID: 63588
						public static LocString DESC = "A nice green shirt that goes with everything.";
					}

					// Token: 0x02003FCC RID: 16332
					public class BASIC_ORANGE
					{
						// Token: 0x0400F865 RID: 63589
						public static LocString NAME = "Basic Orange Shirt";

						// Token: 0x0400F866 RID: 63590
						public static LocString DESC = "A nice orange shirt that goes with everything.";
					}

					// Token: 0x02003FCD RID: 16333
					public class BASIC_PURPLE
					{
						// Token: 0x0400F867 RID: 63591
						public static LocString NAME = "Basic Purple Shirt";

						// Token: 0x0400F868 RID: 63592
						public static LocString DESC = "A nice purple shirt that goes with everything.";
					}

					// Token: 0x02003FCE RID: 16334
					public class BASIC_RED_BURNT
					{
						// Token: 0x0400F869 RID: 63593
						public static LocString NAME = "Basic Red Shirt";

						// Token: 0x0400F86A RID: 63594
						public static LocString DESC = "A nice red shirt that goes with everything.";
					}

					// Token: 0x02003FCF RID: 16335
					public class BASIC_WHITE
					{
						// Token: 0x0400F86B RID: 63595
						public static LocString NAME = "Basic White Shirt";

						// Token: 0x0400F86C RID: 63596
						public static LocString DESC = "A nice white shirt that goes with everything.";
					}

					// Token: 0x02003FD0 RID: 16336
					public class BASIC_YELLOW
					{
						// Token: 0x0400F86D RID: 63597
						public static LocString NAME = "Basic Yellow Shirt";

						// Token: 0x0400F86E RID: 63598
						public static LocString DESC = "A nice yellow shirt that goes with everything.";
					}

					// Token: 0x02003FD1 RID: 16337
					public class RAGLANTOP_DEEPRED
					{
						// Token: 0x0400F86F RID: 63599
						public static LocString NAME = "Team Captain T-shirt";

						// Token: 0x0400F870 RID: 63600
						public static LocString DESC = "A slightly sweat-stained tee for natural leaders.";
					}

					// Token: 0x02003FD2 RID: 16338
					public class RAGLANTOP_COBALT
					{
						// Token: 0x0400F871 RID: 63601
						public static LocString NAME = "True Blue T-shirt";

						// Token: 0x0400F872 RID: 63602
						public static LocString DESC = "A slightly sweat-stained tee for the real team players.";
					}

					// Token: 0x02003FD3 RID: 16339
					public class RAGLANTOP_FLAMINGO
					{
						// Token: 0x0400F873 RID: 63603
						public static LocString NAME = "Pep Rally T-shirt";

						// Token: 0x0400F874 RID: 63604
						public static LocString DESC = "A slightly sweat-stained tee to boost team spirits.";
					}

					// Token: 0x02003FD4 RID: 16340
					public class RAGLANTOP_KELLYGREEN
					{
						// Token: 0x0400F875 RID: 63605
						public static LocString NAME = "Go Team T-shirt";

						// Token: 0x0400F876 RID: 63606
						public static LocString DESC = "A slightly sweat-stained tee for cheering from the sidelines.";
					}

					// Token: 0x02003FD5 RID: 16341
					public class RAGLANTOP_CHARCOAL
					{
						// Token: 0x0400F877 RID: 63607
						public static LocString NAME = "Underdog T-shirt";

						// Token: 0x0400F878 RID: 63608
						public static LocString DESC = "For those who don't win a lot.";
					}

					// Token: 0x02003FD6 RID: 16342
					public class RAGLANTOP_LEMON
					{
						// Token: 0x0400F879 RID: 63609
						public static LocString NAME = "Hype T-shirt";

						// Token: 0x0400F87A RID: 63610
						public static LocString DESC = "A slightly sweat-stained tee to wear when talking a big game.";
					}

					// Token: 0x02003FD7 RID: 16343
					public class RAGLANTOP_SATSUMA
					{
						// Token: 0x0400F87B RID: 63611
						public static LocString NAME = "Superfan T-shirt";

						// Token: 0x0400F87C RID: 63612
						public static LocString DESC = "A slightly sweat-stained tee for the long-time supporter.";
					}

					// Token: 0x02003FD8 RID: 16344
					public class JELLYPUFFJACKET_BLUEBERRY
					{
						// Token: 0x0400F87D RID: 63613
						public static LocString NAME = "Blueberry Jelly Jacket";

						// Token: 0x0400F87E RID: 63614
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003FD9 RID: 16345
					public class JELLYPUFFJACKET_GRAPE
					{
						// Token: 0x0400F87F RID: 63615
						public static LocString NAME = "Grape Jelly Jacket";

						// Token: 0x0400F880 RID: 63616
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003FDA RID: 16346
					public class JELLYPUFFJACKET_LEMON
					{
						// Token: 0x0400F881 RID: 63617
						public static LocString NAME = "Lemon Jelly Jacket";

						// Token: 0x0400F882 RID: 63618
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003FDB RID: 16347
					public class JELLYPUFFJACKET_LIME
					{
						// Token: 0x0400F883 RID: 63619
						public static LocString NAME = "Lime Jelly Jacket";

						// Token: 0x0400F884 RID: 63620
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003FDC RID: 16348
					public class JELLYPUFFJACKET_SATSUMA
					{
						// Token: 0x0400F885 RID: 63621
						public static LocString NAME = "Satsuma Jelly Jacket";

						// Token: 0x0400F886 RID: 63622
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003FDD RID: 16349
					public class JELLYPUFFJACKET_STRAWBERRY
					{
						// Token: 0x0400F887 RID: 63623
						public static LocString NAME = "Strawberry Jelly Jacket";

						// Token: 0x0400F888 RID: 63624
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003FDE RID: 16350
					public class JELLYPUFFJACKET_WATERMELON
					{
						// Token: 0x0400F889 RID: 63625
						public static LocString NAME = "Watermelon Jelly Jacket";

						// Token: 0x0400F88A RID: 63626
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003FDF RID: 16351
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400F88B RID: 63627
						public static LocString NAME = "LED Jacket";

						// Token: 0x0400F88C RID: 63628
						public static LocString DESC = "For dancing in the dark.";
					}

					// Token: 0x02003FE0 RID: 16352
					public class TSHIRT_WHITE
					{
						// Token: 0x0400F88D RID: 63629
						public static LocString NAME = "Classic White Tee";

						// Token: 0x0400F88E RID: 63630
						public static LocString DESC = "It's practically begging for a big Bog Jelly stain down the front.";
					}

					// Token: 0x02003FE1 RID: 16353
					public class TSHIRT_MAGENTA
					{
						// Token: 0x0400F88F RID: 63631
						public static LocString NAME = "Classic Magenta Tee";

						// Token: 0x0400F890 RID: 63632
						public static LocString DESC = "It will never chafe against delicate inner-elbow skin.";
					}

					// Token: 0x02003FE2 RID: 16354
					public class ATHLETE
					{
						// Token: 0x0400F891 RID: 63633
						public static LocString NAME = "Racing Jacket";

						// Token: 0x0400F892 RID: 63634
						public static LocString DESC = "The epitome of fast fashion.";
					}

					// Token: 0x02003FE3 RID: 16355
					public class DENIM_BLUE
					{
						// Token: 0x0400F893 RID: 63635
						public static LocString NAME = "Denim Jacket";

						// Token: 0x0400F894 RID: 63636
						public static LocString DESC = "The top half of a Canadian tuxedo.";
					}

					// Token: 0x02003FE4 RID: 16356
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400F895 RID: 63637
						public static LocString NAME = "Executive Undershirt";

						// Token: 0x0400F896 RID: 63638
						public static LocString DESC = "The breathable base layer every power suit needs.";
					}

					// Token: 0x02003FE5 RID: 16357
					public class GONCH_SATSUMA
					{
						// Token: 0x0400F897 RID: 63639
						public static LocString NAME = "Underling Undershirt";

						// Token: 0x0400F898 RID: 63640
						public static LocString DESC = "Extra-absorbent fabric in the underarms to mop up nervous sweat.";
					}

					// Token: 0x02003FE6 RID: 16358
					public class GONCH_LEMON
					{
						// Token: 0x0400F899 RID: 63641
						public static LocString NAME = "Groupthink Undershirt";

						// Token: 0x0400F89A RID: 63642
						public static LocString DESC = "Because the most popular choice is always the right choice.";
					}

					// Token: 0x02003FE7 RID: 16359
					public class GONCH_LIME
					{
						// Token: 0x0400F89B RID: 63643
						public static LocString NAME = "Stakeholder Undershirt";

						// Token: 0x0400F89C RID: 63644
						public static LocString DESC = "Soft against the skin, for those who have skin in the game.";
					}

					// Token: 0x02003FE8 RID: 16360
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400F89D RID: 63645
						public static LocString NAME = "Admin Undershirt";

						// Token: 0x0400F89E RID: 63646
						public static LocString DESC = "Criminally underappreciated.";
					}

					// Token: 0x02003FE9 RID: 16361
					public class GONCH_GRAPE
					{
						// Token: 0x0400F89F RID: 63647
						public static LocString NAME = "Buzzword Undershirt";

						// Token: 0x0400F8A0 RID: 63648
						public static LocString DESC = "A value-added vest for touching base and thinking outside the box using best practices ASAP.";
					}

					// Token: 0x02003FEA RID: 16362
					public class GONCH_WATERMELON
					{
						// Token: 0x0400F8A1 RID: 63649
						public static LocString NAME = "Synergy Undershirt";

						// Token: 0x0400F8A2 RID: 63650
						public static LocString DESC = "Asking for it by name often triggers dramatic eye-rolls from bystanders.";
					}

					// Token: 0x02003FEB RID: 16363
					public class NERD_BROWN
					{
						// Token: 0x0400F8A3 RID: 63651
						public static LocString NAME = "Research Shirt";

						// Token: 0x0400F8A4 RID: 63652
						public static LocString DESC = "Comes with a thoughtfully chewed-up ballpoint pen.";
					}

					// Token: 0x02003FEC RID: 16364
					public class GI_WHITE
					{
						// Token: 0x0400F8A5 RID: 63653
						public static LocString NAME = "Rebel Gi Jacket";

						// Token: 0x0400F8A6 RID: 63654
						public static LocString DESC = "The contrasting trim hides stains from messy post-sparring snacks.";
					}

					// Token: 0x02003FED RID: 16365
					public class JACKET_SMOKING_BURGUNDY
					{
						// Token: 0x0400F8A7 RID: 63655
						public static LocString NAME = "Donor Jacket";

						// Token: 0x0400F8A8 RID: 63656
						public static LocString DESC = "Crafted from the softest, most philanthropic fibers.";
					}

					// Token: 0x02003FEE RID: 16366
					public class MECHANIC
					{
						// Token: 0x0400F8A9 RID: 63657
						public static LocString NAME = "Engineer Jacket";

						// Token: 0x0400F8AA RID: 63658
						public static LocString DESC = "Designed to withstand the rigors of applied science.";
					}

					// Token: 0x02003FEF RID: 16367
					public class VELOUR_BLACK
					{
						// Token: 0x0400F8AB RID: 63659
						public static LocString NAME = "PhD Velour Jacket";

						// Token: 0x0400F8AC RID: 63660
						public static LocString DESC = "A formal jacket for those who are \"not that kind of doctor.\"";
					}

					// Token: 0x02003FF0 RID: 16368
					public class VELOUR_BLUE
					{
						// Token: 0x0400F8AD RID: 63661
						public static LocString NAME = "Shortwave Velour Jacket";

						// Token: 0x0400F8AE RID: 63662
						public static LocString DESC = "A luxe, pettable jacket paired with a clip-on tie.";
					}

					// Token: 0x02003FF1 RID: 16369
					public class VELOUR_PINK
					{
						// Token: 0x0400F8AF RID: 63663
						public static LocString NAME = "Gamma Velour Jacket";

						// Token: 0x0400F8B0 RID: 63664
						public static LocString DESC = "Some scientists are less shy than others.";
					}

					// Token: 0x02003FF2 RID: 16370
					public class WAISTCOAT_PINSTRIPE_SLATE
					{
						// Token: 0x0400F8B1 RID: 63665
						public static LocString NAME = "Nobel Pinstripe Waistcoat";

						// Token: 0x0400F8B2 RID: 63666
						public static LocString DESC = "One must dress for the prize that one wishes to win.";
					}

					// Token: 0x02003FF3 RID: 16371
					public class WATER
					{
						// Token: 0x0400F8B3 RID: 63667
						public static LocString NAME = "HVAC Khaki Shirt";

						// Token: 0x0400F8B4 RID: 63668
						public static LocString DESC = "Designed to regulate temperature and humidity.";
					}

					// Token: 0x02003FF4 RID: 16372
					public class TWEED_PINK_ORCHID
					{
						// Token: 0x0400F8B5 RID: 63669
						public static LocString NAME = "Power Brunch Blazer";

						// Token: 0x0400F8B6 RID: 63670
						public static LocString DESC = "Winners never quit, quitters never win.";
					}

					// Token: 0x02003FF5 RID: 16373
					public class DRESS_SLEEVELESS_BOW_BW
					{
						// Token: 0x0400F8B7 RID: 63671
						public static LocString NAME = "PhD Dress";

						// Token: 0x0400F8B8 RID: 63672
						public static LocString DESC = "Ready for a post-thesis-defense party.";
					}

					// Token: 0x02003FF6 RID: 16374
					public class BODYSUIT_BALLERINA_PINK
					{
						// Token: 0x0400F8B9 RID: 63673
						public static LocString NAME = "Ballet Leotard";

						// Token: 0x0400F8BA RID: 63674
						public static LocString DESC = "Lab-crafted fabric with a level of stretchiness that defies the laws of physics.";
					}

					// Token: 0x02003FF7 RID: 16375
					public class SOCKSUIT_BEIGE
					{
						// Token: 0x0400F8BB RID: 63675
						public static LocString NAME = "Vintage Sockshirt";

						// Token: 0x0400F8BC RID: 63676
						public static LocString DESC = "Like a sock for the torso. With sleeves.";
					}

					// Token: 0x02003FF8 RID: 16376
					public class X_SPORCHID
					{
						// Token: 0x0400F8BD RID: 63677
						public static LocString NAME = "Sporefest Sweater";

						// Token: 0x0400F8BE RID: 63678
						public static LocString DESC = "This soft knit can be worn anytime, not just during Zombie Spore season.";
					}

					// Token: 0x02003FF9 RID: 16377
					public class X1_PINCHAPEPPERNUTBELLS
					{
						// Token: 0x0400F8BF RID: 63679
						public static LocString NAME = "Pinchabell Jacket";

						// Token: 0x0400F8C0 RID: 63680
						public static LocString DESC = "The peppernuts jingle just loudly enough to be distracting.";
					}

					// Token: 0x02003FFA RID: 16378
					public class POMPOM_SHINEBUGS_PINK_PEPPERNUT
					{
						// Token: 0x0400F8C1 RID: 63681
						public static LocString NAME = "Pom Bug Sweater";

						// Token: 0x0400F8C2 RID: 63682
						public static LocString DESC = "No Shine Bugs were harmed in the making of this sweater.";
					}

					// Token: 0x02003FFB RID: 16379
					public class SNOWFLAKE_BLUE
					{
						// Token: 0x0400F8C3 RID: 63683
						public static LocString NAME = "Crystal-Iced Sweater";

						// Token: 0x0400F8C4 RID: 63684
						public static LocString DESC = "Tiny imperfections in the front pattern ensure that no two are truly identical.";
					}

					// Token: 0x02003FFC RID: 16380
					public class PJ_CLOVERS_GLITCH_KELLY
					{
						// Token: 0x0400F8C5 RID: 63685
						public static LocString NAME = "Lucky Jammies";

						// Token: 0x0400F8C6 RID: 63686
						public static LocString DESC = "Even the most brilliant minds need a little extra luck sometimes.";
					}

					// Token: 0x02003FFD RID: 16381
					public class PJ_HEARTS_CHILLI_STRAWBERRY
					{
						// Token: 0x0400F8C7 RID: 63687
						public static LocString NAME = "Sweetheart Jammies";

						// Token: 0x0400F8C8 RID: 63688
						public static LocString DESC = "Plush chenille fabric and a drool-absorbent collar? This sleepsuit really <i>is</i> \"The One.\"";
					}

					// Token: 0x02003FFE RID: 16382
					public class BUILDER
					{
						// Token: 0x0400F8C9 RID: 63689
						public static LocString NAME = "Hi-Vis Jacket";

						// Token: 0x0400F8CA RID: 63690
						public static LocString DESC = "Unmissable style for the safety-minded.";
					}

					// Token: 0x02003FFF RID: 16383
					public class FLORAL_PINK
					{
						// Token: 0x0400F8CB RID: 63691
						public static LocString NAME = "Downtime Shirt";

						// Token: 0x0400F8CC RID: 63692
						public static LocString DESC = "For maxing and relaxing when errands are too taxing.";
					}

					// Token: 0x02004000 RID: 16384
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400F8CD RID: 63693
						public static LocString NAME = "Frilly Saltrock Undershirt";

						// Token: 0x0400F8CE RID: 63694
						public static LocString DESC = "A seamless pink undershirt with laser-cut ruffles.";
					}

					// Token: 0x02004001 RID: 16385
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400F8CF RID: 63695
						public static LocString NAME = "Frilly Dusk Undershirt";

						// Token: 0x0400F8D0 RID: 63696
						public static LocString DESC = "A seamless purple undershirt with laser-cut ruffles.";
					}

					// Token: 0x02004002 RID: 16386
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400F8D1 RID: 63697
						public static LocString NAME = "Frilly Basin Undershirt";

						// Token: 0x0400F8D2 RID: 63698
						public static LocString DESC = "A seamless blue undershirt with laser-cut ruffles.";
					}

					// Token: 0x02004003 RID: 16387
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400F8D3 RID: 63699
						public static LocString NAME = "Frilly Balm Undershirt";

						// Token: 0x0400F8D4 RID: 63700
						public static LocString DESC = "A seamless teal undershirt with laser-cut ruffles.";
					}

					// Token: 0x02004004 RID: 16388
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400F8D5 RID: 63701
						public static LocString NAME = "Frilly Leach Undershirt";

						// Token: 0x0400F8D6 RID: 63702
						public static LocString DESC = "A seamless green undershirt with laser-cut ruffles.";
					}

					// Token: 0x02004005 RID: 16389
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400F8D7 RID: 63703
						public static LocString NAME = "Frilly Yellowcake Undershirt";

						// Token: 0x0400F8D8 RID: 63704
						public static LocString DESC = "A seamless yellow undershirt with laser-cut ruffles.";
					}

					// Token: 0x02004006 RID: 16390
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400F8D9 RID: 63705
						public static LocString NAME = "Frilly Atomic Undershirt";

						// Token: 0x0400F8DA RID: 63706
						public static LocString DESC = "A seamless orange undershirt with laser-cut ruffles.";
					}

					// Token: 0x02004007 RID: 16391
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400F8DB RID: 63707
						public static LocString NAME = "Frilly Magma Undershirt";

						// Token: 0x0400F8DC RID: 63708
						public static LocString DESC = "A seamless red undershirt with laser-cut ruffles.";
					}

					// Token: 0x02004008 RID: 16392
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400F8DD RID: 63709
						public static LocString NAME = "Frilly Slate Undershirt";

						// Token: 0x0400F8DE RID: 63710
						public static LocString DESC = "A seamless grey undershirt with laser-cut ruffles.";
					}

					// Token: 0x02004009 RID: 16393
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400F8DF RID: 63711
						public static LocString NAME = "Frilly Charcoal Undershirt";

						// Token: 0x0400F8E0 RID: 63712
						public static LocString DESC = "A seamless dark gray undershirt with laser-cut ruffles.";
					}

					// Token: 0x0200400A RID: 16394
					public class KNIT_POLKADOT_TURQ
					{
						// Token: 0x0400F8E1 RID: 63713
						public static LocString NAME = "Polka Dot Track Jacket";

						// Token: 0x0400F8E2 RID: 63714
						public static LocString DESC = "The dots are infused with odor-neutralizing enzymes!";
					}

					// Token: 0x0200400B RID: 16395
					public class FLASHY
					{
						// Token: 0x0400F8E3 RID: 63715
						public static LocString NAME = "Superstar Jacket";

						// Token: 0x0400F8E4 RID: 63716
						public static LocString DESC = "Some of us were not made to be subtle.";
					}
				}
			}

			// Token: 0x02002E25 RID: 11813
			public class CLOTHING_BOTTOMS
			{
				// Token: 0x0400C70F RID: 50959
				public static LocString NAME = "Default Bottom";

				// Token: 0x0400C710 RID: 50960
				public static LocString DESC = "The default bottoms.";

				// Token: 0x02003B0F RID: 15119
				public class FACADES
				{
					// Token: 0x0200400C RID: 16396
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400F8E5 RID: 63717
						public static LocString NAME = "Basic Aqua Pants";

						// Token: 0x0400F8E6 RID: 63718
						public static LocString DESC = "A clean pair of aqua-blue pants that go with everything.";
					}

					// Token: 0x0200400D RID: 16397
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400F8E7 RID: 63719
						public static LocString NAME = "Basic Bubblegum Pants";

						// Token: 0x0400F8E8 RID: 63720
						public static LocString DESC = "A clean pair of bubblegum-pink pants that go with everything.";
					}

					// Token: 0x0200400E RID: 16398
					public class BASIC_GREEN
					{
						// Token: 0x0400F8E9 RID: 63721
						public static LocString NAME = "Basic Green Pants";

						// Token: 0x0400F8EA RID: 63722
						public static LocString DESC = "A clean pair of green pants that go with everything.";
					}

					// Token: 0x0200400F RID: 16399
					public class BASIC_ORANGE
					{
						// Token: 0x0400F8EB RID: 63723
						public static LocString NAME = "Basic Orange Pants";

						// Token: 0x0400F8EC RID: 63724
						public static LocString DESC = "A clean pair of orange pants that go with everything.";
					}

					// Token: 0x02004010 RID: 16400
					public class BASIC_PURPLE
					{
						// Token: 0x0400F8ED RID: 63725
						public static LocString NAME = "Basic Purple Pants";

						// Token: 0x0400F8EE RID: 63726
						public static LocString DESC = "A clean pair of purple pants that go with everything.";
					}

					// Token: 0x02004011 RID: 16401
					public class BASIC_RED
					{
						// Token: 0x0400F8EF RID: 63727
						public static LocString NAME = "Basic Red Pants";

						// Token: 0x0400F8F0 RID: 63728
						public static LocString DESC = "A clean pair of red pants that go with everything.";
					}

					// Token: 0x02004012 RID: 16402
					public class BASIC_WHITE
					{
						// Token: 0x0400F8F1 RID: 63729
						public static LocString NAME = "Basic White Pants";

						// Token: 0x0400F8F2 RID: 63730
						public static LocString DESC = "A clean pair of white pants that go with everything.";
					}

					// Token: 0x02004013 RID: 16403
					public class BASIC_YELLOW
					{
						// Token: 0x0400F8F3 RID: 63731
						public static LocString NAME = "Basic Yellow Pants";

						// Token: 0x0400F8F4 RID: 63732
						public static LocString DESC = "A clean pair of yellow pants that go with everything.";
					}

					// Token: 0x02004014 RID: 16404
					public class BASIC_BLACK
					{
						// Token: 0x0400F8F5 RID: 63733
						public static LocString NAME = "Basic Black Pants";

						// Token: 0x0400F8F6 RID: 63734
						public static LocString DESC = "A clean pair of black pants that go with everything.";
					}

					// Token: 0x02004015 RID: 16405
					public class SHORTS_BASIC_DEEPRED
					{
						// Token: 0x0400F8F7 RID: 63735
						public static LocString NAME = "Team Captain Shorts";

						// Token: 0x0400F8F8 RID: 63736
						public static LocString DESC = "A fresh pair of shorts for natural leaders.";
					}

					// Token: 0x02004016 RID: 16406
					public class SHORTS_BASIC_SATSUMA
					{
						// Token: 0x0400F8F9 RID: 63737
						public static LocString NAME = "Superfan Shorts";

						// Token: 0x0400F8FA RID: 63738
						public static LocString DESC = "A fresh pair of shorts for long-time supporters of...shorts.";
					}

					// Token: 0x02004017 RID: 16407
					public class SHORTS_BASIC_YELLOWCAKE
					{
						// Token: 0x0400F8FB RID: 63739
						public static LocString NAME = "Yellowcake Shorts";

						// Token: 0x0400F8FC RID: 63740
						public static LocString DESC = "A fresh pair of uranium-powder-colored shorts that are definitely not radioactive. Probably.";
					}

					// Token: 0x02004018 RID: 16408
					public class SHORTS_BASIC_KELLYGREEN
					{
						// Token: 0x0400F8FD RID: 63741
						public static LocString NAME = "Go Team Shorts";

						// Token: 0x0400F8FE RID: 63742
						public static LocString DESC = "A fresh pair of shorts for cheering from the sidelines.";
					}

					// Token: 0x02004019 RID: 16409
					public class SHORTS_BASIC_BLUE_COBALT
					{
						// Token: 0x0400F8FF RID: 63743
						public static LocString NAME = "True Blue Shorts";

						// Token: 0x0400F900 RID: 63744
						public static LocString DESC = "A fresh pair of shorts for the real team players.";
					}

					// Token: 0x0200401A RID: 16410
					public class SHORTS_BASIC_PINK_FLAMINGO
					{
						// Token: 0x0400F901 RID: 63745
						public static LocString NAME = "Pep Rally Shorts";

						// Token: 0x0400F902 RID: 63746
						public static LocString DESC = "The peppiest pair of shorts this side of the asteroid.";
					}

					// Token: 0x0200401B RID: 16411
					public class SHORTS_BASIC_CHARCOAL
					{
						// Token: 0x0400F903 RID: 63747
						public static LocString NAME = "Underdog Shorts";

						// Token: 0x0400F904 RID: 63748
						public static LocString DESC = "A fresh pair of shorts. They're cleaner than they look.";
					}

					// Token: 0x0200401C RID: 16412
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400F905 RID: 63749
						public static LocString NAME = "LED Pants";

						// Token: 0x0400F906 RID: 63750
						public static LocString DESC = "These legs are lit.";
					}

					// Token: 0x0200401D RID: 16413
					public class ATHLETE
					{
						// Token: 0x0400F907 RID: 63751
						public static LocString NAME = "Racing Pants";

						// Token: 0x0400F908 RID: 63752
						public static LocString DESC = "Fast, furious fashion.";
					}

					// Token: 0x0200401E RID: 16414
					public class BASIC_LIGHTBROWN
					{
						// Token: 0x0400F909 RID: 63753
						public static LocString NAME = "Basic Khaki Pants";

						// Token: 0x0400F90A RID: 63754
						public static LocString DESC = "Transition effortlessly from subterranean day to subterranean night.";
					}

					// Token: 0x0200401F RID: 16415
					public class BASIC_REDORANGE
					{
						// Token: 0x0400F90B RID: 63755
						public static LocString NAME = "Basic Crimson Pants";

						// Token: 0x0400F90C RID: 63756
						public static LocString DESC = "Like red pants, but slightly fancier-sounding.";
					}

					// Token: 0x02004020 RID: 16416
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400F90D RID: 63757
						public static LocString NAME = "Executive Briefs";

						// Token: 0x0400F90E RID: 63758
						public static LocString DESC = "Bossy (under)pants.";
					}

					// Token: 0x02004021 RID: 16417
					public class GONCH_SATSUMA
					{
						// Token: 0x0400F90F RID: 63759
						public static LocString NAME = "Underling Briefs";

						// Token: 0x0400F910 RID: 63760
						public static LocString DESC = "The seams are already unraveling.";
					}

					// Token: 0x02004022 RID: 16418
					public class GONCH_LEMON
					{
						// Token: 0x0400F911 RID: 63761
						public static LocString NAME = "Groupthink Briefs";

						// Token: 0x0400F912 RID: 63762
						public static LocString DESC = "All the cool people are wearing them.";
					}

					// Token: 0x02004023 RID: 16419
					public class GONCH_LIME
					{
						// Token: 0x0400F913 RID: 63763
						public static LocString NAME = "Stakeholder Briefs";

						// Token: 0x0400F914 RID: 63764
						public static LocString DESC = "They're really invested in keeping the wearer comfortable.";
					}

					// Token: 0x02004024 RID: 16420
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400F915 RID: 63765
						public static LocString NAME = "Admin Briefs";

						// Token: 0x0400F916 RID: 63766
						public static LocString DESC = "The workhorse of the underwear world.";
					}

					// Token: 0x02004025 RID: 16421
					public class GONCH_GRAPE
					{
						// Token: 0x0400F917 RID: 63767
						public static LocString NAME = "Buzzword Briefs";

						// Token: 0x0400F918 RID: 63768
						public static LocString DESC = "Underwear that works hard, plays hard, and gives 110% to maximize the \"bottom\" line.";
					}

					// Token: 0x02004026 RID: 16422
					public class GONCH_WATERMELON
					{
						// Token: 0x0400F919 RID: 63769
						public static LocString NAME = "Synergy Briefs";

						// Token: 0x0400F91A RID: 63770
						public static LocString DESC = "Teamwork makes the dream work.";
					}

					// Token: 0x02004027 RID: 16423
					public class DENIM_BLUE
					{
						// Token: 0x0400F91B RID: 63771
						public static LocString NAME = "Jeans";

						// Token: 0x0400F91C RID: 63772
						public static LocString DESC = "The bottom half of a Canadian tuxedo.";
					}

					// Token: 0x02004028 RID: 16424
					public class GI_WHITE
					{
						// Token: 0x0400F91D RID: 63773
						public static LocString NAME = "White Capris";

						// Token: 0x0400F91E RID: 63774
						public static LocString DESC = "The cropped length is ideal for wading through flooded hallways.";
					}

					// Token: 0x02004029 RID: 16425
					public class NERD_BROWN
					{
						// Token: 0x0400F91F RID: 63775
						public static LocString NAME = "Research Pants";

						// Token: 0x0400F920 RID: 63776
						public static LocString DESC = "The pockets are full of illegible notes that didn't quite survive the wash.";
					}

					// Token: 0x0200402A RID: 16426
					public class SKIRT_BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400F921 RID: 63777
						public static LocString NAME = "Aqua Rayon Skirt";

						// Token: 0x0400F922 RID: 63778
						public static LocString DESC = "The tag says \"Dry Clean Only.\" There are no dry cleaners in space.";
					}

					// Token: 0x0200402B RID: 16427
					public class SKIRT_BASIC_PURPLE
					{
						// Token: 0x0400F923 RID: 63779
						public static LocString NAME = "Purple Rayon Skirt";

						// Token: 0x0400F924 RID: 63780
						public static LocString DESC = "It's not the most breathable fabric, but it <i>is</i> a lovely shade of purple.";
					}

					// Token: 0x0200402C RID: 16428
					public class SKIRT_BASIC_GREEN
					{
						// Token: 0x0400F925 RID: 63781
						public static LocString NAME = "Olive Rayon Skirt";

						// Token: 0x0400F926 RID: 63782
						public static LocString DESC = "Designed not to get snagged on ladders.";
					}

					// Token: 0x0200402D RID: 16429
					public class SKIRT_BASIC_ORANGE
					{
						// Token: 0x0400F927 RID: 63783
						public static LocString NAME = "Apricot Rayon Skirt";

						// Token: 0x0400F928 RID: 63784
						public static LocString DESC = "Ready for spontaneous workplace twirling.";
					}

					// Token: 0x0200402E RID: 16430
					public class SKIRT_BASIC_PINK_ORCHID
					{
						// Token: 0x0400F929 RID: 63785
						public static LocString NAME = "Bubblegum Rayon Skirt";

						// Token: 0x0400F92A RID: 63786
						public static LocString DESC = "The bubblegum scent lasts 100 washes!";
					}

					// Token: 0x0200402F RID: 16431
					public class SKIRT_BASIC_RED
					{
						// Token: 0x0400F92B RID: 63787
						public static LocString NAME = "Garnet Rayon Skirt";

						// Token: 0x0400F92C RID: 63788
						public static LocString DESC = "It's business time.";
					}

					// Token: 0x02004030 RID: 16432
					public class SKIRT_BASIC_YELLOW
					{
						// Token: 0x0400F92D RID: 63789
						public static LocString NAME = "Yellow Rayon Skirt";

						// Token: 0x0400F92E RID: 63790
						public static LocString DESC = "A formerly white skirt that has not aged well.";
					}

					// Token: 0x02004031 RID: 16433
					public class SKIRT_BASIC_POLKADOT
					{
						// Token: 0x0400F92F RID: 63791
						public static LocString NAME = "Polka Dot Skirt";

						// Token: 0x0400F930 RID: 63792
						public static LocString DESC = "Polka dots are a way to infinity.";
					}

					// Token: 0x02004032 RID: 16434
					public class SKIRT_BASIC_WATERMELON
					{
						// Token: 0x0400F931 RID: 63793
						public static LocString NAME = "Picnic Skirt";

						// Token: 0x0400F932 RID: 63794
						public static LocString DESC = "The seeds are spittable, but will bear no fruit.";
					}

					// Token: 0x02004033 RID: 16435
					public class SKIRT_DENIM_BLUE
					{
						// Token: 0x0400F933 RID: 63795
						public static LocString NAME = "Denim Tux Skirt";

						// Token: 0x0400F934 RID: 63796
						public static LocString DESC = "Designed for the casual red carpet.";
					}

					// Token: 0x02004034 RID: 16436
					public class SKIRT_LEOPARD_PRINT_BLUE_PINK
					{
						// Token: 0x0400F935 RID: 63797
						public static LocString NAME = "Disco Leopard Skirt";

						// Token: 0x0400F936 RID: 63798
						public static LocString DESC = "A faux-fur party staple.";
					}

					// Token: 0x02004035 RID: 16437
					public class SKIRT_SPARKLE_BLUE
					{
						// Token: 0x0400F937 RID: 63799
						public static LocString NAME = "Blue Tinsel Skirt";

						// Token: 0x0400F938 RID: 63800
						public static LocString DESC = "The tinsel is scratchy, but look how shiny!";
					}

					// Token: 0x02004036 RID: 16438
					public class BASIC_ORANGE_SATSUMA
					{
						// Token: 0x0400F939 RID: 63801
						public static LocString NAME = "Hi-Vis Pants";

						// Token: 0x0400F93A RID: 63802
						public static LocString DESC = "They make the wearer feel truly seen.";
					}

					// Token: 0x02004037 RID: 16439
					public class PINSTRIPE_SLATE
					{
						// Token: 0x0400F93B RID: 63803
						public static LocString NAME = "Nobel Pinstripe Trousers";

						// Token: 0x0400F93C RID: 63804
						public static LocString DESC = "There's a waterproof pocket to keep acceptance speeches smudge-free.";
					}

					// Token: 0x02004038 RID: 16440
					public class VELOUR_BLACK
					{
						// Token: 0x0400F93D RID: 63805
						public static LocString NAME = "Black Velour Trousers";

						// Token: 0x0400F93E RID: 63806
						public static LocString DESC = "Fuzzy, formal and finely cut.";
					}

					// Token: 0x02004039 RID: 16441
					public class VELOUR_BLUE
					{
						// Token: 0x0400F93F RID: 63807
						public static LocString NAME = "Shortwave Velour Pants";

						// Token: 0x0400F940 RID: 63808
						public static LocString DESC = "Formal wear with a sensory side.";
					}

					// Token: 0x0200403A RID: 16442
					public class VELOUR_PINK
					{
						// Token: 0x0400F941 RID: 63809
						public static LocString NAME = "Gamma Velour Pants";

						// Token: 0x0400F942 RID: 63810
						public static LocString DESC = "They're stretchy <i>and</i> flame retardant.";
					}

					// Token: 0x0200403B RID: 16443
					public class SKIRT_BALLERINA_PINK
					{
						// Token: 0x0400F943 RID: 63811
						public static LocString NAME = "Ballet Tutu";

						// Token: 0x0400F944 RID: 63812
						public static LocString DESC = "A tulle skirt spun and assembled by an army of patent-pending nanobots.";
					}

					// Token: 0x0200403C RID: 16444
					public class SKIRT_TWEED_PINK_ORCHID
					{
						// Token: 0x0400F945 RID: 63813
						public static LocString NAME = "Power Brunch Skirt";

						// Token: 0x0400F946 RID: 63814
						public static LocString DESC = "It has pockets!";
					}

					// Token: 0x0200403D RID: 16445
					public class GINCH_PINK_GLUON
					{
						// Token: 0x0400F947 RID: 63815
						public static LocString NAME = "Gluon Shorties";

						// Token: 0x0400F948 RID: 63816
						public static LocString DESC = "Comfy pink short-shorts with a ruffled hem.";
					}

					// Token: 0x0200403E RID: 16446
					public class GINCH_PURPLE_CORTEX
					{
						// Token: 0x0400F949 RID: 63817
						public static LocString NAME = "Cortex Shorties";

						// Token: 0x0400F94A RID: 63818
						public static LocString DESC = "Comfy purple short-shorts with a ruffled hem.";
					}

					// Token: 0x0200403F RID: 16447
					public class GINCH_BLUE_FROSTY
					{
						// Token: 0x0400F94B RID: 63819
						public static LocString NAME = "Frosty Shorties";

						// Token: 0x0400F94C RID: 63820
						public static LocString DESC = "Icy blue short-shorts with a ruffled hem.";
					}

					// Token: 0x02004040 RID: 16448
					public class GINCH_TEAL_LOCUS
					{
						// Token: 0x0400F94D RID: 63821
						public static LocString NAME = "Locus Shorties";

						// Token: 0x0400F94E RID: 63822
						public static LocString DESC = "Comfy teal short-shorts with a ruffled hem.";
					}

					// Token: 0x02004041 RID: 16449
					public class GINCH_GREEN_GOOP
					{
						// Token: 0x0400F94F RID: 63823
						public static LocString NAME = "Goop Shorties";

						// Token: 0x0400F950 RID: 63824
						public static LocString DESC = "Short-shorts with a ruffled hem and one pocket full of melted snacks.";
					}

					// Token: 0x02004042 RID: 16450
					public class GINCH_YELLOW_BILE
					{
						// Token: 0x0400F951 RID: 63825
						public static LocString NAME = "Bile Shorties";

						// Token: 0x0400F952 RID: 63826
						public static LocString DESC = "Ruffled short-shorts in a stomach-turning shade of yellow.";
					}

					// Token: 0x02004043 RID: 16451
					public class GINCH_ORANGE_NYBBLE
					{
						// Token: 0x0400F953 RID: 63827
						public static LocString NAME = "Nybble Shorties";

						// Token: 0x0400F954 RID: 63828
						public static LocString DESC = "Comfy orange ruffled short-shorts for computer scientists.";
					}

					// Token: 0x02004044 RID: 16452
					public class GINCH_RED_IRONBOW
					{
						// Token: 0x0400F955 RID: 63829
						public static LocString NAME = "Ironbow Shorties";

						// Token: 0x0400F956 RID: 63830
						public static LocString DESC = "Comfy red short-shorts with a ruffled hem.";
					}

					// Token: 0x02004045 RID: 16453
					public class GINCH_GREY_PHLEGM
					{
						// Token: 0x0400F957 RID: 63831
						public static LocString NAME = "Phlegmy Shorties";

						// Token: 0x0400F958 RID: 63832
						public static LocString DESC = "Ruffled short-shorts in a rather sticky shade of light gray.";
					}

					// Token: 0x02004046 RID: 16454
					public class GINCH_GREY_OBELUS
					{
						// Token: 0x0400F959 RID: 63833
						public static LocString NAME = "Obelus Shorties";

						// Token: 0x0400F95A RID: 63834
						public static LocString DESC = "Comfy gray short-shorts with a ruffled hem.";
					}

					// Token: 0x02004047 RID: 16455
					public class KNIT_POLKADOT_TURQ
					{
						// Token: 0x0400F95B RID: 63835
						public static LocString NAME = "Polka Dot Track Pants";

						// Token: 0x0400F95C RID: 63836
						public static LocString DESC = "For clowning around during mandatory physical fitness week.";
					}

					// Token: 0x02004048 RID: 16456
					public class GI_BELT_WHITE_BLACK
					{
						// Token: 0x0400F95D RID: 63837
						public static LocString NAME = "Rebel Gi Pants";

						// Token: 0x0400F95E RID: 63838
						public static LocString DESC = "Relaxed-fit pants designed for roundhouse kicks.";
					}

					// Token: 0x02004049 RID: 16457
					public class BELT_KHAKI_TAN
					{
						// Token: 0x0400F95F RID: 63839
						public static LocString NAME = "HVAC Khaki Pants";

						// Token: 0x0400F960 RID: 63840
						public static LocString DESC = "Rip-resistant fabric makes crawling through ducts a breeze.";
					}
				}
			}

			// Token: 0x02002E26 RID: 11814
			public class CLOTHING_SHOES
			{
				// Token: 0x0400C711 RID: 50961
				public static LocString NAME = "Default Footwear";

				// Token: 0x0400C712 RID: 50962
				public static LocString DESC = "The default style of footwear.";

				// Token: 0x02003B10 RID: 15120
				public class FACADES
				{
					// Token: 0x0200404A RID: 16458
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400F961 RID: 63841
						public static LocString NAME = "Basic Aqua Shoes";

						// Token: 0x0400F962 RID: 63842
						public static LocString DESC = "A fresh pair of aqua-blue shoes that go with everything.";
					}

					// Token: 0x0200404B RID: 16459
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400F963 RID: 63843
						public static LocString NAME = "Basic Bubblegum Shoes";

						// Token: 0x0400F964 RID: 63844
						public static LocString DESC = "A fresh pair of bubblegum-pink shoes that go with everything.";
					}

					// Token: 0x0200404C RID: 16460
					public class BASIC_GREEN
					{
						// Token: 0x0400F965 RID: 63845
						public static LocString NAME = "Basic Green Shoes";

						// Token: 0x0400F966 RID: 63846
						public static LocString DESC = "A fresh pair of green shoes that go with everything.";
					}

					// Token: 0x0200404D RID: 16461
					public class BASIC_ORANGE
					{
						// Token: 0x0400F967 RID: 63847
						public static LocString NAME = "Basic Orange Shoes";

						// Token: 0x0400F968 RID: 63848
						public static LocString DESC = "A fresh pair of orange shoes that go with everything.";
					}

					// Token: 0x0200404E RID: 16462
					public class BASIC_PURPLE
					{
						// Token: 0x0400F969 RID: 63849
						public static LocString NAME = "Basic Purple Shoes";

						// Token: 0x0400F96A RID: 63850
						public static LocString DESC = "A fresh pair of purple shoes that go with everything.";
					}

					// Token: 0x0200404F RID: 16463
					public class BASIC_RED
					{
						// Token: 0x0400F96B RID: 63851
						public static LocString NAME = "Basic Red Shoes";

						// Token: 0x0400F96C RID: 63852
						public static LocString DESC = "A fresh pair of red shoes that go with everything.";
					}

					// Token: 0x02004050 RID: 16464
					public class BASIC_WHITE
					{
						// Token: 0x0400F96D RID: 63853
						public static LocString NAME = "Basic White Shoes";

						// Token: 0x0400F96E RID: 63854
						public static LocString DESC = "A fresh pair of white shoes that go with everything.";
					}

					// Token: 0x02004051 RID: 16465
					public class BASIC_YELLOW
					{
						// Token: 0x0400F96F RID: 63855
						public static LocString NAME = "Basic Yellow Shoes";

						// Token: 0x0400F970 RID: 63856
						public static LocString DESC = "A fresh pair of yellow shoes that go with everything.";
					}

					// Token: 0x02004052 RID: 16466
					public class BASIC_BLACK
					{
						// Token: 0x0400F971 RID: 63857
						public static LocString NAME = "Basic Black Shoes";

						// Token: 0x0400F972 RID: 63858
						public static LocString DESC = "A fresh pair of black shoes that go with everything.";
					}

					// Token: 0x02004053 RID: 16467
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400F973 RID: 63859
						public static LocString NAME = "Basic Gunmetal Shoes";

						// Token: 0x0400F974 RID: 63860
						public static LocString DESC = "A fresh pair of pastel shoes that go with everything.";
					}

					// Token: 0x02004054 RID: 16468
					public class BASIC_TAN
					{
						// Token: 0x0400F975 RID: 63861
						public static LocString NAME = "Basic Tan Shoes";

						// Token: 0x0400F976 RID: 63862
						public static LocString DESC = "They're remarkably unremarkable.";
					}

					// Token: 0x02004055 RID: 16469
					public class SOCKS_ATHLETIC_DEEPRED
					{
						// Token: 0x0400F977 RID: 63863
						public static LocString NAME = "Team Captain Gym Socks";

						// Token: 0x0400F978 RID: 63864
						public static LocString DESC = "Breathable socks with sporty red stripes.";
					}

					// Token: 0x02004056 RID: 16470
					public class SOCKS_ATHLETIC_SATSUMA
					{
						// Token: 0x0400F979 RID: 63865
						public static LocString NAME = "Superfan Gym Socks";

						// Token: 0x0400F97A RID: 63866
						public static LocString DESC = "Breathable socks with sporty orange stripes.";
					}

					// Token: 0x02004057 RID: 16471
					public class SOCKS_ATHLETIC_LEMON
					{
						// Token: 0x0400F97B RID: 63867
						public static LocString NAME = "Hype Gym Socks";

						// Token: 0x0400F97C RID: 63868
						public static LocString DESC = "Breathable socks with sporty yellow stripes.";
					}

					// Token: 0x02004058 RID: 16472
					public class SOCKS_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400F97D RID: 63869
						public static LocString NAME = "Go Team Gym Socks";

						// Token: 0x0400F97E RID: 63870
						public static LocString DESC = "Breathable socks with sporty green stripes.";
					}

					// Token: 0x02004059 RID: 16473
					public class SOCKS_ATHLETIC_COBALT
					{
						// Token: 0x0400F97F RID: 63871
						public static LocString NAME = "True Blue Gym Socks";

						// Token: 0x0400F980 RID: 63872
						public static LocString DESC = "Breathable socks with sporty blue stripes.";
					}

					// Token: 0x0200405A RID: 16474
					public class SOCKS_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400F981 RID: 63873
						public static LocString NAME = "Pep Rally Gym Socks";

						// Token: 0x0400F982 RID: 63874
						public static LocString DESC = "Breathable socks with sporty pink stripes.";
					}

					// Token: 0x0200405B RID: 16475
					public class SOCKS_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400F983 RID: 63875
						public static LocString NAME = "Underdog Gym Socks";

						// Token: 0x0400F984 RID: 63876
						public static LocString DESC = "Breathable socks that do nothing whatsoever to eliminate foot odor.";
					}

					// Token: 0x0200405C RID: 16476
					public class BASIC_GREY
					{
						// Token: 0x0400F985 RID: 63877
						public static LocString NAME = "Basic Gray Shoes";

						// Token: 0x0400F986 RID: 63878
						public static LocString DESC = "A fresh pair of gray shoes that go with everything.";
					}

					// Token: 0x0200405D RID: 16477
					public class DENIM_BLUE
					{
						// Token: 0x0400F987 RID: 63879
						public static LocString NAME = "Denim Shoes";

						// Token: 0x0400F988 RID: 63880
						public static LocString DESC = "Not technically essential for a Canadian tuxedo, but why not?";
					}

					// Token: 0x0200405E RID: 16478
					public class LEGWARMERS_STRAWBERRY
					{
						// Token: 0x0400F989 RID: 63881
						public static LocString NAME = "Slouchy Strawberry Socks";

						// Token: 0x0400F98A RID: 63882
						public static LocString DESC = "Freckly knitted socks that don't stay up.";
					}

					// Token: 0x0200405F RID: 16479
					public class LEGWARMERS_SATSUMA
					{
						// Token: 0x0400F98B RID: 63883
						public static LocString NAME = "Slouchy Satsuma Socks";

						// Token: 0x0400F98C RID: 63884
						public static LocString DESC = "Sweet knitted socks for spontaneous dance segments.";
					}

					// Token: 0x02004060 RID: 16480
					public class LEGWARMERS_LEMON
					{
						// Token: 0x0400F98D RID: 63885
						public static LocString NAME = "Slouchy Lemon Socks";

						// Token: 0x0400F98E RID: 63886
						public static LocString DESC = "Zesty knitted socks that don't stay up.";
					}

					// Token: 0x02004061 RID: 16481
					public class LEGWARMERS_LIME
					{
						// Token: 0x0400F98F RID: 63887
						public static LocString NAME = "Slouchy Lime Socks";

						// Token: 0x0400F990 RID: 63888
						public static LocString DESC = "Juicy knitted socks that don't stay up.";
					}

					// Token: 0x02004062 RID: 16482
					public class LEGWARMERS_BLUEBERRY
					{
						// Token: 0x0400F991 RID: 63889
						public static LocString NAME = "Slouchy Blueberry Socks";

						// Token: 0x0400F992 RID: 63890
						public static LocString DESC = "Knitted socks with a fun bobble-stitch texture.";
					}

					// Token: 0x02004063 RID: 16483
					public class LEGWARMERS_GRAPE
					{
						// Token: 0x0400F993 RID: 63891
						public static LocString NAME = "Slouchy Grape Socks";

						// Token: 0x0400F994 RID: 63892
						public static LocString DESC = "These fabulous knitted socks that don't stay up are really raisin the bar.";
					}

					// Token: 0x02004064 RID: 16484
					public class LEGWARMERS_WATERMELON
					{
						// Token: 0x0400F995 RID: 63893
						public static LocString NAME = "Slouchy Watermelon Socks";

						// Token: 0x0400F996 RID: 63894
						public static LocString DESC = "Summery knitted socks that don't stay up.";
					}

					// Token: 0x02004065 RID: 16485
					public class BALLERINA_PINK
					{
						// Token: 0x0400F997 RID: 63895
						public static LocString NAME = "Ballet Shoes";

						// Token: 0x0400F998 RID: 63896
						public static LocString DESC = "There's no \"pointe\" in aiming for anything less than perfection.";
					}

					// Token: 0x02004066 RID: 16486
					public class MARYJANE_SOCKS_BW
					{
						// Token: 0x0400F999 RID: 63897
						public static LocString NAME = "Frilly Sock Shoes";

						// Token: 0x0400F99A RID: 63898
						public static LocString DESC = "They add a little <i>je ne sais quoi</i> to everyday lab wear.";
					}

					// Token: 0x02004067 RID: 16487
					public class CLASSICFLATS_CREAM_CHARCOAL
					{
						// Token: 0x0400F99B RID: 63899
						public static LocString NAME = "Dressy Shoes";

						// Token: 0x0400F99C RID: 63900
						public static LocString DESC = "An enduring style, for enduring endless small talk.";
					}

					// Token: 0x02004068 RID: 16488
					public class VELOUR_BLUE
					{
						// Token: 0x0400F99D RID: 63901
						public static LocString NAME = "Shortwave Velour Shoes";

						// Token: 0x0400F99E RID: 63902
						public static LocString DESC = "Not the easiest to keep clean.";
					}

					// Token: 0x02004069 RID: 16489
					public class VELOUR_PINK
					{
						// Token: 0x0400F99F RID: 63903
						public static LocString NAME = "Gamma Velour Shoes";

						// Token: 0x0400F9A0 RID: 63904
						public static LocString DESC = "Finally, a pair of work-appropriate fuzzy shoes.";
					}

					// Token: 0x0200406A RID: 16490
					public class VELOUR_BLACK
					{
						// Token: 0x0400F9A1 RID: 63905
						public static LocString NAME = "Black Velour Shoes";

						// Token: 0x0400F9A2 RID: 63906
						public static LocString DESC = "Matching velour lining gently tickles feet with every step.";
					}

					// Token: 0x0200406B RID: 16491
					public class FLASHY
					{
						// Token: 0x0400F9A3 RID: 63907
						public static LocString NAME = "Superstar Shoes";

						// Token: 0x0400F9A4 RID: 63908
						public static LocString DESC = "Why walk when you can <i>moon</i>walk?";
					}

					// Token: 0x0200406C RID: 16492
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400F9A5 RID: 63909
						public static LocString NAME = "Frilly Saltrock Socks";

						// Token: 0x0400F9A6 RID: 63910
						public static LocString DESC = "Thick, soft pink socks with extra flounce.";
					}

					// Token: 0x0200406D RID: 16493
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400F9A7 RID: 63911
						public static LocString NAME = "Frilly Dusk Socks";

						// Token: 0x0400F9A8 RID: 63912
						public static LocString DESC = "Thick, soft purple socks with extra flounce.";
					}

					// Token: 0x0200406E RID: 16494
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400F9A9 RID: 63913
						public static LocString NAME = "Frilly Basin Socks";

						// Token: 0x0400F9AA RID: 63914
						public static LocString DESC = "Thick, soft blue socks with extra flounce.";
					}

					// Token: 0x0200406F RID: 16495
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400F9AB RID: 63915
						public static LocString NAME = "Frilly Balm Socks";

						// Token: 0x0400F9AC RID: 63916
						public static LocString DESC = "Thick, soothing teal socks with extra flounce.";
					}

					// Token: 0x02004070 RID: 16496
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400F9AD RID: 63917
						public static LocString NAME = "Frilly Leach Socks";

						// Token: 0x0400F9AE RID: 63918
						public static LocString DESC = "Thick, soft green socks with extra flounce.";
					}

					// Token: 0x02004071 RID: 16497
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400F9AF RID: 63919
						public static LocString NAME = "Frilly Yellowcake Socks";

						// Token: 0x0400F9B0 RID: 63920
						public static LocString DESC = "Dangerously soft yellow socks with extra flounce.";
					}

					// Token: 0x02004072 RID: 16498
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400F9B1 RID: 63921
						public static LocString NAME = "Frilly Atomic Socks";

						// Token: 0x0400F9B2 RID: 63922
						public static LocString DESC = "Thick, soft orange socks with extra flounce.";
					}

					// Token: 0x02004073 RID: 16499
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400F9B3 RID: 63923
						public static LocString NAME = "Frilly Magma Socks";

						// Token: 0x0400F9B4 RID: 63924
						public static LocString DESC = "Thick, toasty red socks with extra flounce.";
					}

					// Token: 0x02004074 RID: 16500
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400F9B5 RID: 63925
						public static LocString NAME = "Frilly Slate Socks";

						// Token: 0x0400F9B6 RID: 63926
						public static LocString DESC = "Thick, soft gray socks with extra flounce.";
					}

					// Token: 0x02004075 RID: 16501
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400F9B7 RID: 63927
						public static LocString NAME = "Frilly Charcoal Socks";

						// Token: 0x0400F9B8 RID: 63928
						public static LocString DESC = "Thick, soft dark gray socks with extra flounce.";
					}
				}
			}

			// Token: 0x02002E27 RID: 11815
			public class CLOTHING_HATS
			{
				// Token: 0x0400C713 RID: 50963
				public static LocString NAME = "Default Headgear";

				// Token: 0x0400C714 RID: 50964
				public static LocString DESC = "<DESC>";

				// Token: 0x02003B11 RID: 15121
				public class FACADES
				{
				}
			}

			// Token: 0x02002E28 RID: 11816
			public class CLOTHING_ACCESORIES
			{
				// Token: 0x0400C715 RID: 50965
				public static LocString NAME = "Default Accessory";

				// Token: 0x0400C716 RID: 50966
				public static LocString DESC = "<DESC>";

				// Token: 0x02003B12 RID: 15122
				public class FACADES
				{
				}
			}

			// Token: 0x02002E29 RID: 11817
			public class OXYGEN_TANK
			{
				// Token: 0x0400C717 RID: 50967
				public static LocString NAME = UI.FormatAsLink("Oxygen Tank", "OXYGEN_TANK");

				// Token: 0x0400C718 RID: 50968
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400C719 RID: 50969
				public static LocString DESC = "It's like a to-go bag for your lungs.";

				// Token: 0x0400C71A RID: 50970
				public static LocString EFFECT = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>.";

				// Token: 0x0400C71B RID: 50971
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>";
			}

			// Token: 0x02002E2A RID: 11818
			public class OXYGEN_TANK_UNDERWATER
			{
				// Token: 0x0400C71C RID: 50972
				public static LocString NAME = "Oxygen Rebreather";

				// Token: 0x0400C71D RID: 50973
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400C71E RID: 50974
				public static LocString DESC = "";

				// Token: 0x0400C71F RID: 50975
				public static LocString EFFECT = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid.";

				// Token: 0x0400C720 RID: 50976
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid";
			}

			// Token: 0x02002E2B RID: 11819
			public class EQUIPPABLEBALLOON
			{
				// Token: 0x0400C721 RID: 50977
				public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

				// Token: 0x0400C722 RID: 50978
				public static LocString DESC = "A floating friend to reassure my Duplicants they are so very, very clever.";

				// Token: 0x0400C723 RID: 50979
				public static LocString EFFECT = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response.";

				// Token: 0x0400C724 RID: 50980
				public static LocString RECIPE_DESC = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response";

				// Token: 0x0400C725 RID: 50981
				public static LocString GENERICNAME = "Balloon Friend";

				// Token: 0x02003B13 RID: 15123
				public class FACADES
				{
					// Token: 0x02004076 RID: 16502
					public class DEFAULT_BALLOON
					{
						// Token: 0x0400F9B9 RID: 63929
						public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9BA RID: 63930
						public static LocString DESC = "A floating friend to reassure my Duplicants that they are so very, very clever.";
					}

					// Token: 0x02004077 RID: 16503
					public class BALLOON_FIREENGINE_LONG_SPARKLES
					{
						// Token: 0x0400F9BB RID: 63931
						public static LocString NAME = UI.FormatAsLink("Magma Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9BC RID: 63932
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02004078 RID: 16504
					public class BALLOON_YELLOW_LONG_SPARKLES
					{
						// Token: 0x0400F9BD RID: 63933
						public static LocString NAME = UI.FormatAsLink("Lavatory Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9BE RID: 63934
						public static LocString DESC = "Sparkly balloons in an all-too-familiar hue.";
					}

					// Token: 0x02004079 RID: 16505
					public class BALLOON_BLUE_LONG_SPARKLES
					{
						// Token: 0x0400F9BF RID: 63935
						public static LocString NAME = UI.FormatAsLink("Wheezewort Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9C0 RID: 63936
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x0200407A RID: 16506
					public class BALLOON_GREEN_LONG_SPARKLES
					{
						// Token: 0x0400F9C1 RID: 63937
						public static LocString NAME = UI.FormatAsLink("Mush Bar Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9C2 RID: 63938
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x0200407B RID: 16507
					public class BALLOON_PINK_LONG_SPARKLES
					{
						// Token: 0x0400F9C3 RID: 63939
						public static LocString NAME = UI.FormatAsLink("Petal Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9C4 RID: 63940
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x0200407C RID: 16508
					public class BALLOON_PURPLE_LONG_SPARKLES
					{
						// Token: 0x0400F9C5 RID: 63941
						public static LocString NAME = UI.FormatAsLink("Dusky Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9C6 RID: 63942
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x0200407D RID: 16509
					public class BALLOON_BABY_PACU_EGG
					{
						// Token: 0x0400F9C7 RID: 63943
						public static LocString NAME = UI.FormatAsLink("Floatie Fish", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9C8 RID: 63944
						public static LocString DESC = "They do not taste as good as the real thing.";
					}

					// Token: 0x0200407E RID: 16510
					public class BALLOON_BABY_GLOSSY_DRECKO_EGG
					{
						// Token: 0x0400F9C9 RID: 63945
						public static LocString NAME = UI.FormatAsLink("Glossy Glee", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9CA RID: 63946
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x0200407F RID: 16511
					public class BALLOON_BABY_HATCH_EGG
					{
						// Token: 0x0400F9CB RID: 63947
						public static LocString NAME = UI.FormatAsLink("Helium Hatches", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9CC RID: 63948
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02004080 RID: 16512
					public class BALLOON_BABY_POKESHELL_EGG
					{
						// Token: 0x0400F9CD RID: 63949
						public static LocString NAME = UI.FormatAsLink("Peppy Pokeshells", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9CE RID: 63950
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02004081 RID: 16513
					public class BALLOON_BABY_PUFT_EGG
					{
						// Token: 0x0400F9CF RID: 63951
						public static LocString NAME = UI.FormatAsLink("Puffed-Up Pufts", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9D0 RID: 63952
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02004082 RID: 16514
					public class BALLOON_BABY_SHOVOLE_EGG
					{
						// Token: 0x0400F9D1 RID: 63953
						public static LocString NAME = UI.FormatAsLink("Voley Voley Voles", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9D2 RID: 63954
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02004083 RID: 16515
					public class BALLOON_BABY_PIP_EGG
					{
						// Token: 0x0400F9D3 RID: 63955
						public static LocString NAME = UI.FormatAsLink("Pip Pip Hooray", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9D4 RID: 63956
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02004084 RID: 16516
					public class CANDY_BLUEBERRY
					{
						// Token: 0x0400F9D5 RID: 63957
						public static LocString NAME = UI.FormatAsLink("Candied Blueberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9D6 RID: 63958
						public static LocString DESC = "A juicy bunch of blueberry-scented balloons.";
					}

					// Token: 0x02004085 RID: 16517
					public class CANDY_GRAPE
					{
						// Token: 0x0400F9D7 RID: 63959
						public static LocString NAME = UI.FormatAsLink("Candied Grape", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9D8 RID: 63960
						public static LocString DESC = "A juicy bunch of grape-scented balloons.";
					}

					// Token: 0x02004086 RID: 16518
					public class CANDY_LEMON
					{
						// Token: 0x0400F9D9 RID: 63961
						public static LocString NAME = UI.FormatAsLink("Candied Lemon", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9DA RID: 63962
						public static LocString DESC = "A juicy lemon-scented bunch of balloons.";
					}

					// Token: 0x02004087 RID: 16519
					public class CANDY_LIME
					{
						// Token: 0x0400F9DB RID: 63963
						public static LocString NAME = UI.FormatAsLink("Candied Lime", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9DC RID: 63964
						public static LocString DESC = "A juicy lime-scented bunch of balloons.";
					}

					// Token: 0x02004088 RID: 16520
					public class CANDY_ORANGE
					{
						// Token: 0x0400F9DD RID: 63965
						public static LocString NAME = UI.FormatAsLink("Candied Satsuma", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9DE RID: 63966
						public static LocString DESC = "A juicy satsuma-scented bunch of balloons.";
					}

					// Token: 0x02004089 RID: 16521
					public class CANDY_STRAWBERRY
					{
						// Token: 0x0400F9DF RID: 63967
						public static LocString NAME = UI.FormatAsLink("Candied Strawberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9E0 RID: 63968
						public static LocString DESC = "A juicy strawberry-scented bunch of balloons.";
					}

					// Token: 0x0200408A RID: 16522
					public class CANDY_WATERMELON
					{
						// Token: 0x0400F9E1 RID: 63969
						public static LocString NAME = UI.FormatAsLink("Candied Watermelon", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9E2 RID: 63970
						public static LocString DESC = "A juicy watermelon-scented bunch of balloons.";
					}

					// Token: 0x0200408B RID: 16523
					public class HAND_GOLD
					{
						// Token: 0x0400F9E3 RID: 63971
						public static LocString NAME = UI.FormatAsLink("Gold Fingers", "EQUIPPABLEBALLOON");

						// Token: 0x0400F9E4 RID: 63972
						public static LocString DESC = "Inflatable gestures of encouragement.";
					}
				}
			}

			// Token: 0x02002E2C RID: 11820
			public class SLEEPCLINICPAJAMAS
			{
				// Token: 0x0400C726 RID: 50982
				public static LocString NAME = UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");

				// Token: 0x0400C727 RID: 50983
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400C728 RID: 50984
				public static LocString DESC = "A soft, fleecy ticket to dreamland.";

				// Token: 0x0400C729 RID: 50985
				public static LocString EFFECT = "Helps Duplicants fall asleep by reducing " + UI.FormatAsLink("Stamina", "HEALTH") + ".\n\nEnables the wearer to dream and produce Dream Journals.";

				// Token: 0x0400C72A RID: 50986
				public static LocString DESTROY_TOAST = "Ripped Pajamas";
			}
		}
	}
}
