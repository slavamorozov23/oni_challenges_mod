using System;

namespace STRINGS
{
	// Token: 0x02000FF8 RID: 4088
	public class RESEARCH
	{
		// Token: 0x02002577 RID: 9591
		public class MESSAGING
		{
			// Token: 0x0400A972 RID: 43378
			public static LocString NORESEARCHSELECTED = "No research selected";

			// Token: 0x0400A973 RID: 43379
			public static LocString RESEARCHTYPEREQUIRED = "{0} required";

			// Token: 0x0400A974 RID: 43380
			public static LocString RESEARCHTYPEALSOREQUIRED = "{0} also required";

			// Token: 0x0400A975 RID: 43381
			public static LocString NO_RESEARCHER_SKILL = "No Researchers assigned";

			// Token: 0x0400A976 RID: 43382
			public static LocString NO_RESEARCHER_SKILL_TOOLTIP = "The selected research focus requires {ResearchType} to complete\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " and teach a Duplicant the {ResearchType} Skill to use this building";

			// Token: 0x0400A977 RID: 43383
			public static LocString MISSING_RESEARCH_STATION = "Missing Research Station";

			// Token: 0x0400A978 RID: 43384
			public static LocString MISSING_RESEARCH_STATION_TOOLTIP = "The selected research focus requires a {0} to perform\n\nOpen the " + UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10) + " of the Build Menu to construct one";

			// Token: 0x0200342C RID: 13356
			public static class DLC
			{
				// Token: 0x0400DB6C RID: 56172
				public static LocString EXPANSION1 = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"\n\n<i>",
					UI.DLC1.NAME,
					"</i>",
					UI.PST_KEYWORD,
					" DLC Content"
				});

				// Token: 0x0400DB6D RID: 56173
				public static LocString DLC_CONTENT = "\n<i>{0}</i> DLC Content";
			}
		}

		// Token: 0x02002578 RID: 9592
		public class TYPES
		{
			// Token: 0x0400A979 RID: 43385
			public static LocString MISSINGRECIPEDESC = "Missing Recipe Description";

			// Token: 0x0200342D RID: 13357
			public class ALPHA
			{
				// Token: 0x0400DB6E RID: 56174
				public static LocString NAME = "Novice Research";

				// Token: 0x0400DB6F RID: 56175
				public static LocString DESC = UI.FormatAsLink("Novice Research", "RESEARCH") + " is required to unlock basic technologies.\nIt can be conducted at a " + UI.FormatAsLink("Research Station", "RESEARCHCENTER") + ".";

				// Token: 0x0400DB70 RID: 56176
				public static LocString RECIPEDESC = "Unlocks rudimentary technologies.";
			}

			// Token: 0x0200342E RID: 13358
			public class BETA
			{
				// Token: 0x0400DB71 RID: 56177
				public static LocString NAME = "Advanced Research";

				// Token: 0x0400DB72 RID: 56178
				public static LocString DESC = UI.FormatAsLink("Advanced Research", "RESEARCH") + " is required to unlock improved technologies.\nIt can be conducted at a " + UI.FormatAsLink("Super Computer", "ADVANCEDRESEARCHCENTER") + ".";

				// Token: 0x0400DB73 RID: 56179
				public static LocString RECIPEDESC = "Unlocks improved technologies.";
			}

			// Token: 0x0200342F RID: 13359
			public class GAMMA
			{
				// Token: 0x0400DB74 RID: 56180
				public static LocString NAME = "Interstellar Research";

				// Token: 0x0400DB75 RID: 56181
				public static LocString DESC = UI.FormatAsLink("Interstellar Research", "RESEARCH") + " is required to unlock space technologies.\nIt can be conducted at a " + UI.FormatAsLink("Virtual Planetarium", "COSMICRESEARCHCENTER") + ".";

				// Token: 0x0400DB76 RID: 56182
				public static LocString RECIPEDESC = "Unlocks cutting-edge technologies.";
			}

			// Token: 0x02003430 RID: 13360
			public class DELTA
			{
				// Token: 0x0400DB77 RID: 56183
				public static LocString NAME = "Applied Sciences Research";

				// Token: 0x0400DB78 RID: 56184
				public static LocString DESC = UI.FormatAsLink("Applied Sciences Research", "RESEARCH") + " is required to unlock materials science technologies.\nIt can be conducted at a " + UI.FormatAsLink("Materials Study Terminal", "NUCLEARRESEARCHCENTER") + ".";

				// Token: 0x0400DB79 RID: 56185
				public static LocString RECIPEDESC = "Unlocks next wave technologies.";
			}

			// Token: 0x02003431 RID: 13361
			public class ORBITAL
			{
				// Token: 0x0400DB7A RID: 56186
				public static LocString NAME = "Data Analysis Research";

				// Token: 0x0400DB7B RID: 56187
				public static LocString DESC = UI.FormatAsLink("Data Analysis Research", "RESEARCH") + " is required to unlock Data Analysis technologies.\nIt can be conducted at a " + UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER") + ".";

				// Token: 0x0400DB7C RID: 56188
				public static LocString RECIPEDESC = "Unlocks out-of-this-world technologies.";
			}
		}

		// Token: 0x02002579 RID: 9593
		public class OTHER_TECH_ITEMS
		{
			// Token: 0x02003432 RID: 13362
			public class AUTOMATION_OVERLAY
			{
				// Token: 0x0400DB7D RID: 56189
				public static LocString NAME = UI.FormatAsOverlay("Automation Overlay");

				// Token: 0x0400DB7E RID: 56190
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Automation Overlay") + ".";
			}

			// Token: 0x02003433 RID: 13363
			public class SUITS_OVERLAY
			{
				// Token: 0x0400DB7F RID: 56191
				public static LocString NAME = UI.FormatAsOverlay("Exosuit Overlay");

				// Token: 0x0400DB80 RID: 56192
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Exosuit Overlay") + ".";
			}

			// Token: 0x02003434 RID: 13364
			public class JET_SUIT
			{
				// Token: 0x0400DB81 RID: 56193
				public static LocString NAME = UI.PRE_KEYWORD + "Jet Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DB82 RID: 56194
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Jet Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02003435 RID: 13365
			public class OXYGEN_MASK
			{
				// Token: 0x0400DB83 RID: 56195
				public static LocString NAME = UI.PRE_KEYWORD + "Oxygen Mask" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DB84 RID: 56196
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Oxygen Masks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003436 RID: 13366
			public class LEAD_SUIT
			{
				// Token: 0x0400DB85 RID: 56197
				public static LocString NAME = UI.PRE_KEYWORD + "Lead Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DB86 RID: 56198
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Lead Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02003437 RID: 13367
			public class ATMO_SUIT
			{
				// Token: 0x0400DB87 RID: 56199
				public static LocString NAME = UI.PRE_KEYWORD + "Atmo Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DB88 RID: 56200
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Atmo Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02003438 RID: 13368
			public class BETA_RESEARCH_POINT
			{
				// Token: 0x0400DB89 RID: 56201
				public static LocString NAME = UI.PRE_KEYWORD + "Advanced Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400DB8A RID: 56202
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Advanced Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02003439 RID: 13369
			public class GAMMA_RESEARCH_POINT
			{
				// Token: 0x0400DB8B RID: 56203
				public static LocString NAME = UI.PRE_KEYWORD + "Interstellar Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400DB8C RID: 56204
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Interstellar Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x0200343A RID: 13370
			public class DELTA_RESEARCH_POINT
			{
				// Token: 0x0400DB8D RID: 56205
				public static LocString NAME = UI.PRE_KEYWORD + "Materials Science Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400DB8E RID: 56206
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Materials Science Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x0200343B RID: 13371
			public class ORBITAL_RESEARCH_POINT
			{
				// Token: 0x0400DB8F RID: 56207
				public static LocString NAME = UI.PRE_KEYWORD + "Data Analysis Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400DB90 RID: 56208
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Data Analysis Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x0200343C RID: 13372
			public class CONVEYOR_OVERLAY
			{
				// Token: 0x0400DB91 RID: 56209
				public static LocString NAME = UI.FormatAsOverlay("Conveyor Overlay");

				// Token: 0x0400DB92 RID: 56210
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Conveyor Overlay") + ".";
			}

			// Token: 0x0200343D RID: 13373
			public class SUPER_LIQUIDS
			{
				// Token: 0x0400DB93 RID: 56211
				public static LocString NAME = UI.PRE_KEYWORD + "Advanced Chemical Production" + UI.PST_KEYWORD;

				// Token: 0x0400DB94 RID: 56212
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables production of ",
					ELEMENTS.VISCOGEL.NAME,
					" and ",
					ELEMENTS.SUPERCOOLANT.NAME,
					" at the ",
					BUILDINGS.PREFABS.CHEMICALREFINERY.NAME,
					"."
				});
			}

			// Token: 0x0200343E RID: 13374
			public class LUBRICATION_STICK
			{
				// Token: 0x0400DB95 RID: 56213
				public static LocString NAME = UI.PRE_KEYWORD + "Gear Balm" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DB96 RID: 56214
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Gear Balm",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.APOTHECARY.NAME
				});
			}

			// Token: 0x0200343F RID: 13375
			public class DISPOSABLE_ELECTROBANK_METAL_ORE
			{
				// Token: 0x0400DB97 RID: 56215
				public static LocString NAME = UI.PRE_KEYWORD + "Metal Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DB98 RID: 56216
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Metal Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003440 RID: 13376
			public class DISPOSABLE_ELECTROBANK_URANIUM_ORE
			{
				// Token: 0x0400DB99 RID: 56217
				public static LocString NAME = UI.PRE_KEYWORD + "Uranium Ore Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DB9A RID: 56218
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Uranium Ore Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003441 RID: 13377
			public class ELECTROBANK
			{
				// Token: 0x0400DB9B RID: 56219
				public static LocString NAME = UI.PRE_KEYWORD + "Eco Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DB9C RID: 56220
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Eco Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003442 RID: 13378
			public class SELFCHARGINGELECTROBANK
			{
				// Token: 0x0400DB9D RID: 56221
				public static LocString NAME = UI.PRE_KEYWORD + "Atomic Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DB9E RID: 56222
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Atomic Power Bank",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUPERMATERIALREFINERY.NAME
				});
			}

			// Token: 0x02003443 RID: 13379
			public class FETCHDRONE
			{
				// Token: 0x0400DB9F RID: 56223
				public static LocString NAME = UI.PRE_KEYWORD + "Flydo" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBA0 RID: 56224
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Flydo",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003444 RID: 13380
			public class PILOTINGBOOSTER
			{
				// Token: 0x0400DBA1 RID: 56225
				public static LocString NAME = UI.PRE_KEYWORD + "Rocketry Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBA2 RID: 56226
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Rocketry Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003445 RID: 13381
			public class CONSTRUCTIONBOOSTER
			{
				// Token: 0x0400DBA3 RID: 56227
				public static LocString NAME = UI.PRE_KEYWORD + "Building Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBA4 RID: 56228
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Building Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003446 RID: 13382
			public class EXCAVATIONBOOSTER
			{
				// Token: 0x0400DBA5 RID: 56229
				public static LocString NAME = UI.PRE_KEYWORD + "Digging Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBA6 RID: 56230
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Digging Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003447 RID: 13383
			public class EXPLORERBOOSTER
			{
				// Token: 0x0400DBA7 RID: 56231
				public static LocString NAME = UI.PRE_KEYWORD + "Dowsing Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBA8 RID: 56232
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Dowsing Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003448 RID: 13384
			public class MACHINERYBOOSTER
			{
				// Token: 0x0400DBA9 RID: 56233
				public static LocString NAME = UI.PRE_KEYWORD + "Operating Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBAA RID: 56234
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Operating Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003449 RID: 13385
			public class ATHLETICSBOOSTER
			{
				// Token: 0x0400DBAB RID: 56235
				public static LocString NAME = UI.PRE_KEYWORD + "Athletics Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBAC RID: 56236
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Athletics Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200344A RID: 13386
			public class SCIENCEBOOSTER
			{
				// Token: 0x0400DBAD RID: 56237
				public static LocString NAME = UI.PRE_KEYWORD + "Researching Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBAE RID: 56238
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Researching Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200344B RID: 13387
			public class COOKINGBOOSTER
			{
				// Token: 0x0400DBAF RID: 56239
				public static LocString NAME = UI.PRE_KEYWORD + "Cooking Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBB0 RID: 56240
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Cooking Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200344C RID: 13388
			public class MEDICINEBOOSTER
			{
				// Token: 0x0400DBB1 RID: 56241
				public static LocString NAME = UI.PRE_KEYWORD + "Doctoring Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBB2 RID: 56242
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Doctoring Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200344D RID: 13389
			public class STRENGTHBOOSTER
			{
				// Token: 0x0400DBB3 RID: 56243
				public static LocString NAME = UI.PRE_KEYWORD + "Strength Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBB4 RID: 56244
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Strength Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200344E RID: 13390
			public class CREATIVITYBOOSTER
			{
				// Token: 0x0400DBB5 RID: 56245
				public static LocString NAME = UI.PRE_KEYWORD + "Decorating Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBB6 RID: 56246
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Decorating Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200344F RID: 13391
			public class AGRICULTUREBOOSTER
			{
				// Token: 0x0400DBB7 RID: 56247
				public static LocString NAME = UI.PRE_KEYWORD + "Farming Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBB8 RID: 56248
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Farming Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003450 RID: 13392
			public class HUSBANDRYBOOSTER
			{
				// Token: 0x0400DBB9 RID: 56249
				public static LocString NAME = UI.PRE_KEYWORD + "Ranching Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400DBBA RID: 56250
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Ranching Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}
		}

		// Token: 0x0200257A RID: 9594
		public class TREES
		{
			// Token: 0x0400A97A RID: 43386
			public static LocString TITLE_FOOD = "Food";

			// Token: 0x0400A97B RID: 43387
			public static LocString TITLE_POWER = "Power";

			// Token: 0x0400A97C RID: 43388
			public static LocString TITLE_SOLIDS = "Solid Material";

			// Token: 0x0400A97D RID: 43389
			public static LocString TITLE_COLONYDEVELOPMENT = "Colony Development";

			// Token: 0x0400A97E RID: 43390
			public static LocString TITLE_RADIATIONTECH = "Radiation Technologies";

			// Token: 0x0400A97F RID: 43391
			public static LocString TITLE_MEDICINE = "Medicine";

			// Token: 0x0400A980 RID: 43392
			public static LocString TITLE_LIQUIDS = "Liquids";

			// Token: 0x0400A981 RID: 43393
			public static LocString TITLE_GASES = "Gases";

			// Token: 0x0400A982 RID: 43394
			public static LocString TITLE_SUITS = "Exosuits";

			// Token: 0x0400A983 RID: 43395
			public static LocString TITLE_DECOR = "Decor";

			// Token: 0x0400A984 RID: 43396
			public static LocString TITLE_COMPUTERS = "Computers";

			// Token: 0x0400A985 RID: 43397
			public static LocString TITLE_ROCKETS = "Rocketry";
		}

		// Token: 0x0200257B RID: 9595
		public class TECHS
		{
			// Token: 0x02003451 RID: 13393
			public class JOBS
			{
				// Token: 0x0400DBBB RID: 56251
				public static LocString NAME = UI.FormatAsLink("Employment", "JOBS");

				// Token: 0x0400DBBC RID: 56252
				public static LocString DESC = "Exchange the skill points earned by Duplicants for new traits and abilities.";
			}

			// Token: 0x02003452 RID: 13394
			public class IMPROVEDOXYGEN
			{
				// Token: 0x0400DBBD RID: 56253
				public static LocString NAME = UI.FormatAsLink("Air Systems", "IMPROVEDOXYGEN");

				// Token: 0x0400DBBE RID: 56254
				public static LocString DESC = "Maintain clean, breathable air in the colony.";
			}

			// Token: 0x02003453 RID: 13395
			public class FARMINGTECH
			{
				// Token: 0x0400DBBF RID: 56255
				public static LocString NAME = UI.FormatAsLink("Basic Farming", "FARMINGTECH");

				// Token: 0x0400DBC0 RID: 56256
				public static LocString DESC = "Learn the introductory principles of " + UI.FormatAsLink("Plant", "PLANTS") + " domestication.";
			}

			// Token: 0x02003454 RID: 13396
			public class AGRICULTURE
			{
				// Token: 0x0400DBC1 RID: 56257
				public static LocString NAME = UI.FormatAsLink("Agriculture", "AGRICULTURE");

				// Token: 0x0400DBC2 RID: 56258
				public static LocString DESC = "Master the agricultural art of crop raising.";
			}

			// Token: 0x02003455 RID: 13397
			public class RANCHING
			{
				// Token: 0x0400DBC3 RID: 56259
				public static LocString NAME = UI.FormatAsLink("Ranching", "RANCHING");

				// Token: 0x0400DBC4 RID: 56260
				public static LocString DESC = "Tame and care for wild critters.";
			}

			// Token: 0x02003456 RID: 13398
			public class ANIMALCONTROL
			{
				// Token: 0x0400DBC5 RID: 56261
				public static LocString NAME = UI.FormatAsLink("Animal Control", "ANIMALCONTROL");

				// Token: 0x0400DBC6 RID: 56262
				public static LocString DESC = "Useful techniques to manage critter populations in the colony.";
			}

			// Token: 0x02003457 RID: 13399
			public class ANIMALCOMFORT
			{
				// Token: 0x0400DBC7 RID: 56263
				public static LocString NAME = UI.FormatAsLink("Creature Comforts", "ANIMALCOMFORT");

				// Token: 0x0400DBC8 RID: 56264
				public static LocString DESC = "Strategies for maximizing critters' quality of life.";
			}

			// Token: 0x02003458 RID: 13400
			public class DAIRYOPERATION
			{
				// Token: 0x0400DBC9 RID: 56265
				public static LocString NAME = UI.FormatAsLink("Brackene Flow", "DAIRYOPERATION");

				// Token: 0x0400DBCA RID: 56266
				public static LocString DESC = "Advanced production, processing and distribution of this fluid resource.";
			}

			// Token: 0x02003459 RID: 13401
			public class FOODREPURPOSING
			{
				// Token: 0x0400DBCB RID: 56267
				public static LocString NAME = UI.FormatAsLink("Food Repurposing", "FOODREPURPOSING");

				// Token: 0x0400DBCC RID: 56268
				public static LocString DESC = string.Concat(new string[]
				{
					"Blend that leftover ",
					UI.FormatAsLink("Food", "FOOD"),
					" into a ",
					UI.FormatAsLink("Morale", "MORALE"),
					"-boosting slurry."
				});
			}

			// Token: 0x0200345A RID: 13402
			public class FINEDINING
			{
				// Token: 0x0400DBCD RID: 56269
				public static LocString NAME = UI.FormatAsLink("Meal Preparation", "FINEDINING");

				// Token: 0x0400DBCE RID: 56270
				public static LocString DESC = "Prepare more nutritious " + UI.FormatAsLink("Food", "FOOD") + " and store it longer before spoiling.";
			}

			// Token: 0x0200345B RID: 13403
			public class FINERDINING
			{
				// Token: 0x0400DBCF RID: 56271
				public static LocString NAME = UI.FormatAsLink("Gourmet Meal Preparation", "FINERDINING");

				// Token: 0x0400DBD0 RID: 56272
				public static LocString DESC = "Raise colony Morale by cooking the most delicious, high-quality " + UI.FormatAsLink("Foods", "FOOD") + ".";
			}

			// Token: 0x0200345C RID: 13404
			public class GASPIPING
			{
				// Token: 0x0400DBD1 RID: 56273
				public static LocString NAME = UI.FormatAsLink("Ventilation", "GASPIPING");

				// Token: 0x0400DBD2 RID: 56274
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure.";
			}

			// Token: 0x0200345D RID: 13405
			public class IMPROVEDGASPIPING
			{
				// Token: 0x0400DBD3 RID: 56275
				public static LocString NAME = UI.FormatAsLink("Improved Ventilation", "IMPROVEDGASPIPING");

				// Token: 0x0400DBD4 RID: 56276
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x0200345E RID: 13406
			public class FLOWREDIRECTION
			{
				// Token: 0x0400DBD5 RID: 56277
				public static LocString NAME = UI.FormatAsLink("Flow Redirection", "FLOWREDIRECTION");

				// Token: 0x0400DBD6 RID: 56278
				public static LocString DESC = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " management for " + UI.FormatAsLink("Morale", "MORALE") + " and industry.";
			}

			// Token: 0x0200345F RID: 13407
			public class LIQUIDDISTRIBUTION
			{
				// Token: 0x0400DBD7 RID: 56279
				public static LocString NAME = UI.FormatAsLink("Liquid Distribution", "LIQUIDDISTRIBUTION");

				// Token: 0x0400DBD8 RID: 56280
				public static LocString DESC = "Advanced fittings ensure that " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources get where they need to go.";
			}

			// Token: 0x02003460 RID: 13408
			public class TEMPERATUREMODULATION
			{
				// Token: 0x0400DBD9 RID: 56281
				public static LocString NAME = UI.FormatAsLink("Temperature Modulation", "TEMPERATUREMODULATION");

				// Token: 0x0400DBDA RID: 56282
				public static LocString DESC = "Precise " + UI.FormatAsLink("Temperature", "HEAT") + " altering technologies to keep my colony at the perfect Kelvin.";
			}

			// Token: 0x02003461 RID: 13409
			public class HVAC
			{
				// Token: 0x0400DBDB RID: 56283
				public static LocString NAME = UI.FormatAsLink("HVAC", "HVAC");

				// Token: 0x0400DBDC RID: 56284
				public static LocString DESC = string.Concat(new string[]
				{
					"Regulate ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" in the colony for ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" cultivation and Duplicant comfort."
				});
			}

			// Token: 0x02003462 RID: 13410
			public class GASDISTRIBUTION
			{
				// Token: 0x0400DBDD RID: 56285
				public static LocString NAME = UI.FormatAsLink("Gas Distribution", "GASDISTRIBUTION");

				// Token: 0x0400DBDE RID: 56286
				public static LocString DESC = "Design building hookups to get " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources circulating properly.";
			}

			// Token: 0x02003463 RID: 13411
			public class LIQUIDTEMPERATURE
			{
				// Token: 0x0400DBDF RID: 56287
				public static LocString NAME = UI.FormatAsLink("Liquid Tuning", "LIQUIDTEMPERATURE");

				// Token: 0x0400DBE0 RID: 56288
				public static LocString DESC = string.Concat(new string[]
				{
					"Easily manipulate ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" ",
					UI.FormatAsLink("Heat", "Temperatures"),
					" with these temperature regulating technologies."
				});
			}

			// Token: 0x02003464 RID: 13412
			public class INSULATION
			{
				// Token: 0x0400DBE1 RID: 56289
				public static LocString NAME = UI.FormatAsLink("Insulation", "INSULATION");

				// Token: 0x0400DBE2 RID: 56290
				public static LocString DESC = "Improve " + UI.FormatAsLink("Heat", "Heat") + " distribution within the colony and guard buildings from extreme temperatures.";
			}

			// Token: 0x02003465 RID: 13413
			public class PRESSUREMANAGEMENT
			{
				// Token: 0x0400DBE3 RID: 56291
				public static LocString NAME = UI.FormatAsLink("Pressure Management", "PRESSUREMANAGEMENT");

				// Token: 0x0400DBE4 RID: 56292
				public static LocString DESC = "Unlock technologies to manage colony pressure and atmosphere.";
			}

			// Token: 0x02003466 RID: 13414
			public class PORTABLEGASSES
			{
				// Token: 0x0400DBE5 RID: 56293
				public static LocString NAME = UI.FormatAsLink("Portable Gases", "PORTABLEGASSES");

				// Token: 0x0400DBE6 RID: 56294
				public static LocString DESC = "Unlock technologies to easily move gases around your colony.";
			}

			// Token: 0x02003467 RID: 13415
			public class DIRECTEDAIRSTREAMS
			{
				// Token: 0x0400DBE7 RID: 56295
				public static LocString NAME = UI.FormatAsLink("Decontamination", "DIRECTEDAIRSTREAMS");

				// Token: 0x0400DBE8 RID: 56296
				public static LocString DESC = "Instruments to help reduce " + UI.FormatAsLink("Germ", "DISEASE") + " spread within the base.";
			}

			// Token: 0x02003468 RID: 13416
			public class LIQUIDFILTERING
			{
				// Token: 0x0400DBE9 RID: 56297
				public static LocString NAME = UI.FormatAsLink("Liquid-Based Refinement Processes", "LIQUIDFILTERING");

				// Token: 0x0400DBEA RID: 56298
				public static LocString DESC = "Use pumped " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " to filter out unwanted elements.";
			}

			// Token: 0x02003469 RID: 13417
			public class LIQUIDPIPING
			{
				// Token: 0x0400DBEB RID: 56299
				public static LocString NAME = UI.FormatAsLink("Plumbing", "LIQUIDPIPING");

				// Token: 0x0400DBEC RID: 56300
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure.";
			}

			// Token: 0x0200346A RID: 13418
			public class IMPROVEDLIQUIDPIPING
			{
				// Token: 0x0400DBED RID: 56301
				public static LocString NAME = UI.FormatAsLink("Improved Plumbing", "IMPROVEDLIQUIDPIPING");

				// Token: 0x0400DBEE RID: 56302
				public static LocString DESC = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x0200346B RID: 13419
			public class PRECISIONPLUMBING
			{
				// Token: 0x0400DBEF RID: 56303
				public static LocString NAME = UI.FormatAsLink("Advanced Caffeination", "PRECISIONPLUMBING");

				// Token: 0x0400DBF0 RID: 56304
				public static LocString DESC = "Let Duplicants relax after a long day of subterranean digging with a shot of warm beanjuice.";
			}

			// Token: 0x0200346C RID: 13420
			public class SANITATIONSCIENCES
			{
				// Token: 0x0400DBF1 RID: 56305
				public static LocString NAME = UI.FormatAsLink("Sanitation", "SANITATIONSCIENCES");

				// Token: 0x0400DBF2 RID: 56306
				public static LocString DESC = "Make daily ablutions less of a hassle.";
			}

			// Token: 0x0200346D RID: 13421
			public class ADVANCEDSANITATION
			{
				// Token: 0x0400DBF3 RID: 56307
				public static LocString NAME = UI.FormatAsLink("Advanced Sanitation", "ADVANCEDSANITATION");

				// Token: 0x0400DBF4 RID: 56308
				public static LocString DESC = "Clean up dirty Duplicants.";
			}

			// Token: 0x0200346E RID: 13422
			public class MEDICINEI
			{
				// Token: 0x0400DBF5 RID: 56309
				public static LocString NAME = UI.FormatAsLink("Pharmacology", "MEDICINEI");

				// Token: 0x0400DBF6 RID: 56310
				public static LocString DESC = "Compound natural cures to fight the most common " + UI.FormatAsLink("Sicknesses", "SICKNESSES") + " that plague Duplicants.";
			}

			// Token: 0x0200346F RID: 13423
			public class MEDICINEII
			{
				// Token: 0x0400DBF7 RID: 56311
				public static LocString NAME = UI.FormatAsLink("Medical Equipment", "MEDICINEII");

				// Token: 0x0400DBF8 RID: 56312
				public static LocString DESC = "The basic necessities doctors need to facilitate patient care.";
			}

			// Token: 0x02003470 RID: 13424
			public class MEDICINEIII
			{
				// Token: 0x0400DBF9 RID: 56313
				public static LocString NAME = UI.FormatAsLink("Pathogen Diagnostics", "MEDICINEIII");

				// Token: 0x0400DBFA RID: 56314
				public static LocString DESC = "Stop Germs at the source using special medical automation technology.";
			}

			// Token: 0x02003471 RID: 13425
			public class MEDICINEIV
			{
				// Token: 0x0400DBFB RID: 56315
				public static LocString NAME = UI.FormatAsLink("Micro-Targeted Medicine", "MEDICINEIV");

				// Token: 0x0400DBFC RID: 56316
				public static LocString DESC = "State of the art equipment to conquer the most stubborn of illnesses.";
			}

			// Token: 0x02003472 RID: 13426
			public class ADVANCEDFILTRATION
			{
				// Token: 0x0400DBFD RID: 56317
				public static LocString NAME = UI.FormatAsLink("Filtration", "ADVANCEDFILTRATION");

				// Token: 0x0400DBFE RID: 56318
				public static LocString DESC = string.Concat(new string[]
				{
					"Basic technologies for filtering ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02003473 RID: 13427
			public class POWERREGULATION
			{
				// Token: 0x0400DBFF RID: 56319
				public static LocString NAME = UI.FormatAsLink("Power Regulation", "POWERREGULATION");

				// Token: 0x0400DC00 RID: 56320
				public static LocString DESC = "Prevent wasted " + UI.FormatAsLink("Power", "POWER") + " with improved electrical tools.";
			}

			// Token: 0x02003474 RID: 13428
			public class COMBUSTION
			{
				// Token: 0x0400DC01 RID: 56321
				public static LocString NAME = UI.FormatAsLink("Internal Combustion", "COMBUSTION");

				// Token: 0x0400DC02 RID: 56322
				public static LocString DESC = "Fuel-powered generators for crude yet powerful " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x02003475 RID: 13429
			public class IMPROVEDCOMBUSTION
			{
				// Token: 0x0400DC03 RID: 56323
				public static LocString NAME = UI.FormatAsLink("Fossil Fuels", "IMPROVEDCOMBUSTION");

				// Token: 0x0400DC04 RID: 56324
				public static LocString DESC = "Burn dirty fuels for exceptional " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x02003476 RID: 13430
			public class INTERIORDECOR
			{
				// Token: 0x0400DC05 RID: 56325
				public static LocString NAME = UI.FormatAsLink("Interior Decor", "INTERIORDECOR");

				// Token: 0x0400DC06 RID: 56326
				public static LocString DESC = UI.FormatAsLink("Decor", "DECOR") + " boosting items to counteract the gloom of underground living.";
			}

			// Token: 0x02003477 RID: 13431
			public class ARTISTRY
			{
				// Token: 0x0400DC07 RID: 56327
				public static LocString NAME = UI.FormatAsLink("Artistic Expression", "ARTISTRY");

				// Token: 0x0400DC08 RID: 56328
				public static LocString DESC = "Majorly improve " + UI.FormatAsLink("Decor", "DECOR") + " by giving Duplicants the tools of artistic and emotional expression.";
			}

			// Token: 0x02003478 RID: 13432
			public class CLOTHING
			{
				// Token: 0x0400DC09 RID: 56329
				public static LocString NAME = UI.FormatAsLink("Textile Production", "CLOTHING");

				// Token: 0x0400DC0A RID: 56330
				public static LocString DESC = "Bring Duplicants the " + UI.FormatAsLink("Morale", "MORALE") + " boosting benefits of soft, cushy fabrics.";
			}

			// Token: 0x02003479 RID: 13433
			public class ACOUSTICS
			{
				// Token: 0x0400DC0B RID: 56331
				public static LocString NAME = UI.FormatAsLink("Sound Amplifiers", "ACOUSTICS");

				// Token: 0x0400DC0C RID: 56332
				public static LocString DESC = "Precise control of the audio spectrum allows Duplicants to get funky.";
			}

			// Token: 0x0200347A RID: 13434
			public class SPACEPOWER
			{
				// Token: 0x0400DC0D RID: 56333
				public static LocString NAME = UI.FormatAsLink("Space Power", "SPACEPOWER");

				// Token: 0x0400DC0E RID: 56334
				public static LocString DESC = "It's like power... in space!";
			}

			// Token: 0x0200347B RID: 13435
			public class AMPLIFIERS
			{
				// Token: 0x0400DC0F RID: 56335
				public static LocString NAME = UI.FormatAsLink("Power Amplifiers", "AMPLIFIERS");

				// Token: 0x0400DC10 RID: 56336
				public static LocString DESC = "Further increased efficacy of " + UI.FormatAsLink("Power", "POWER") + " management to prevent those wasted joules.";
			}

			// Token: 0x0200347C RID: 13436
			public class LUXURY
			{
				// Token: 0x0400DC11 RID: 56337
				public static LocString NAME = UI.FormatAsLink("Home Luxuries", "LUXURY");

				// Token: 0x0400DC12 RID: 56338
				public static LocString DESC = "Luxury amenities for advanced " + UI.FormatAsLink("Stress", "STRESS") + " reduction.";
			}

			// Token: 0x0200347D RID: 13437
			public class ENVIRONMENTALAPPRECIATION
			{
				// Token: 0x0400DC13 RID: 56339
				public static LocString NAME = UI.FormatAsLink("Environmental Appreciation", "ENVIRONMENTALAPPRECIATION");

				// Token: 0x0400DC14 RID: 56340
				public static LocString DESC = string.Concat(new string[]
				{
					"Improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" by lazing around in ",
					UI.FormatAsLink("Light", "LIGHT"),
					" with a high Lux value."
				});
			}

			// Token: 0x0200347E RID: 13438
			public class FINEART
			{
				// Token: 0x0400DC15 RID: 56341
				public static LocString NAME = UI.FormatAsLink("Fine Art", "FINEART");

				// Token: 0x0400DC16 RID: 56342
				public static LocString DESC = "Broader options for artistic " + UI.FormatAsLink("Decor", "DECOR") + " improvements.";
			}

			// Token: 0x0200347F RID: 13439
			public class REFRACTIVEDECOR
			{
				// Token: 0x0400DC17 RID: 56343
				public static LocString NAME = UI.FormatAsLink("High Culture", "REFRACTIVEDECOR");

				// Token: 0x0400DC18 RID: 56344
				public static LocString DESC = "New methods for working with extremely high quality art materials.";
			}

			// Token: 0x02003480 RID: 13440
			public class RENAISSANCEART
			{
				// Token: 0x0400DC19 RID: 56345
				public static LocString NAME = UI.FormatAsLink("Renaissance Art", "RENAISSANCEART");

				// Token: 0x0400DC1A RID: 56346
				public static LocString DESC = "The kind of art that culture legacies are made of.";
			}

			// Token: 0x02003481 RID: 13441
			public class GLASSFURNISHINGS
			{
				// Token: 0x0400DC1B RID: 56347
				public static LocString NAME = UI.FormatAsLink("Glass Blowing", "GLASSFURNISHINGS");

				// Token: 0x0400DC1C RID: 56348
				public static LocString DESC = "The decorative benefits of glass are both apparent and transparent.";
			}

			// Token: 0x02003482 RID: 13442
			public class SCREENS
			{
				// Token: 0x0400DC1D RID: 56349
				public static LocString NAME = UI.FormatAsLink("New Media", "SCREENS");

				// Token: 0x0400DC1E RID: 56350
				public static LocString DESC = "High tech displays with lots of pretty colors.";
			}

			// Token: 0x02003483 RID: 13443
			public class ADVANCEDPOWERREGULATION
			{
				// Token: 0x0400DC1F RID: 56351
				public static LocString NAME = UI.FormatAsLink("Advanced Power Regulation", "ADVANCEDPOWERREGULATION");

				// Token: 0x0400DC20 RID: 56352
				public static LocString DESC = "Circuit components required for large scale " + UI.FormatAsLink("Power", "POWER") + " management.";
			}

			// Token: 0x02003484 RID: 13444
			public class PLASTICS
			{
				// Token: 0x0400DC21 RID: 56353
				public static LocString NAME = UI.FormatAsLink("Plastic Manufacturing", "PLASTICS");

				// Token: 0x0400DC22 RID: 56354
				public static LocString DESC = "Stable, lightweight, durable. Plastics are useful for a wide array of applications.";
			}

			// Token: 0x02003485 RID: 13445
			public class SUITS
			{
				// Token: 0x0400DC23 RID: 56355
				public static LocString NAME = UI.FormatAsLink("Hazard Protection", "SUITS");

				// Token: 0x0400DC24 RID: 56356
				public static LocString DESC = "Vital gear for surviving in extreme conditions and environments.";
			}

			// Token: 0x02003486 RID: 13446
			public class DISTILLATION
			{
				// Token: 0x0400DC25 RID: 56357
				public static LocString NAME = UI.FormatAsLink("Distillation", "DISTILLATION");

				// Token: 0x0400DC26 RID: 56358
				public static LocString DESC = "Distill difficult mixtures down to their most useful parts.";
			}

			// Token: 0x02003487 RID: 13447
			public class ADVANCEDDISTILLATION
			{
				// Token: 0x0400DC27 RID: 56359
				public static LocString NAME = UI.FormatAsLink("Emulsification", "ADVANCEDDISTILLATION");

				// Token: 0x0400DC28 RID: 56360
				public static LocString DESC = "Specialized production of " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " compounds.";
			}

			// Token: 0x02003488 RID: 13448
			public class CATALYTICS
			{
				// Token: 0x0400DC29 RID: 56361
				public static LocString NAME = UI.FormatAsLink("Catalytics", "CATALYTICS");

				// Token: 0x0400DC2A RID: 56362
				public static LocString DESC = "Advanced gas manipulation using unique catalysts.";
			}

			// Token: 0x02003489 RID: 13449
			public class ADVANCEDRESEARCH
			{
				// Token: 0x0400DC2B RID: 56363
				public static LocString NAME = UI.FormatAsLink("Advanced Research", "ADVANCEDRESEARCH");

				// Token: 0x0400DC2C RID: 56364
				public static LocString DESC = "The tools my colony needs to conduct more advanced, in-depth research.";
			}

			// Token: 0x0200348A RID: 13450
			public class SPACEPROGRAM
			{
				// Token: 0x0400DC2D RID: 56365
				public static LocString NAME = UI.FormatAsLink("Space Program", "SPACEPROGRAM");

				// Token: 0x0400DC2E RID: 56366
				public static LocString DESC = "The first steps in getting a Duplicant to space.";
			}

			// Token: 0x0200348B RID: 13451
			public class CRASHPLAN
			{
				// Token: 0x0400DC2F RID: 56367
				public static LocString NAME = UI.FormatAsLink("Crash Plan", "CRASHPLAN");

				// Token: 0x0400DC30 RID: 56368
				public static LocString DESC = "What goes up, must come down.";
			}

			// Token: 0x0200348C RID: 13452
			public class DURABLELIFESUPPORT
			{
				// Token: 0x0400DC31 RID: 56369
				public static LocString NAME = UI.FormatAsLink("Durable Life Support", "DURABLELIFESUPPORT");

				// Token: 0x0400DC32 RID: 56370
				public static LocString DESC = "Improved devices for extended missions into space.";
			}

			// Token: 0x0200348D RID: 13453
			public class ARTIFICIALFRIENDS
			{
				// Token: 0x0400DC33 RID: 56371
				public static LocString NAME = UI.FormatAsLink("Artificial Friends", "ARTIFICIALFRIENDS");

				// Token: 0x0400DC34 RID: 56372
				public static LocString DESC = "Sweeping advances in companion technology.";
			}

			// Token: 0x0200348E RID: 13454
			public class ROBOTICTOOLS
			{
				// Token: 0x0400DC35 RID: 56373
				public static LocString NAME = UI.FormatAsLink("Robotic Tools", "ROBOTICTOOLS");

				// Token: 0x0400DC36 RID: 56374
				public static LocString DESC = "The goal of every great civilization is to one day make itself obsolete.";
			}

			// Token: 0x0200348F RID: 13455
			public class LOGICCONTROL
			{
				// Token: 0x0400DC37 RID: 56375
				public static LocString NAME = UI.FormatAsLink("Smart Home", "LOGICCONTROL");

				// Token: 0x0400DC38 RID: 56376
				public static LocString DESC = "Switches that grant full control of building operations within the colony.";
			}

			// Token: 0x02003490 RID: 13456
			public class LOGICCIRCUITS
			{
				// Token: 0x0400DC39 RID: 56377
				public static LocString NAME = UI.FormatAsLink("Advanced Automation", "LOGICCIRCUITS");

				// Token: 0x0400DC3A RID: 56378
				public static LocString DESC = "The only limit to colony automation is my own imagination.";
			}

			// Token: 0x02003491 RID: 13457
			public class PARALLELAUTOMATION
			{
				// Token: 0x0400DC3B RID: 56379
				public static LocString NAME = UI.FormatAsLink("Parallel Automation", "PARALLELAUTOMATION");

				// Token: 0x0400DC3C RID: 56380
				public static LocString DESC = "Multi-wire automation at a fraction of the space.";
			}

			// Token: 0x02003492 RID: 13458
			public class MULTIPLEXING
			{
				// Token: 0x0400DC3D RID: 56381
				public static LocString NAME = UI.FormatAsLink("Multiplexing", "MULTIPLEXING");

				// Token: 0x0400DC3E RID: 56382
				public static LocString DESC = "More choices for Automation signal distribution.";
			}

			// Token: 0x02003493 RID: 13459
			public class VALVEMINIATURIZATION
			{
				// Token: 0x0400DC3F RID: 56383
				public static LocString NAME = UI.FormatAsLink("Valve Miniaturization", "VALVEMINIATURIZATION");

				// Token: 0x0400DC40 RID: 56384
				public static LocString DESC = "Smaller, more efficient pumps for those low-throughput situations.";
			}

			// Token: 0x02003494 RID: 13460
			public class HYDROCARBONPROPULSION
			{
				// Token: 0x0400DC41 RID: 56385
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Propulsion", "HYDROCARBONPROPULSION");

				// Token: 0x0400DC42 RID: 56386
				public static LocString DESC = "Low-range rocket engines with lots of smoke.";
			}

			// Token: 0x02003495 RID: 13461
			public class BETTERHYDROCARBONPROPULSION
			{
				// Token: 0x0400DC43 RID: 56387
				public static LocString NAME = UI.FormatAsLink("Improved Hydrocarbon Propulsion", "BETTERHYDROCARBONPROPULSION");

				// Token: 0x0400DC44 RID: 56388
				public static LocString DESC = "Mid-range rocket engines with lots of smoke.";
			}

			// Token: 0x02003496 RID: 13462
			public class PRETTYGOODCONDUCTORS
			{
				// Token: 0x0400DC45 RID: 56389
				public static LocString NAME = UI.FormatAsLink("Low-Resistance Conductors", "PRETTYGOODCONDUCTORS");

				// Token: 0x0400DC46 RID: 56390
				public static LocString DESC = "Pure-core wires that can handle more " + UI.FormatAsLink("Electrical", "POWER") + " current without overloading.";
			}

			// Token: 0x02003497 RID: 13463
			public class RENEWABLEENERGY
			{
				// Token: 0x0400DC47 RID: 56391
				public static LocString NAME = UI.FormatAsLink("Renewable Energy", "RENEWABLEENERGY");

				// Token: 0x0400DC48 RID: 56392
				public static LocString DESC = "Clean, sustainable " + UI.FormatAsLink("Power", "POWER") + " production that produces little to no waste.";
			}

			// Token: 0x02003498 RID: 13464
			public class BASICREFINEMENT
			{
				// Token: 0x0400DC49 RID: 56393
				public static LocString NAME = UI.FormatAsLink("Brute-Force Refinement", "BASICREFINEMENT");

				// Token: 0x0400DC4A RID: 56394
				public static LocString DESC = "Low-tech refinement methods for producing clay and renewable sources of sand.";
			}

			// Token: 0x02003499 RID: 13465
			public class REFINEDOBJECTS
			{
				// Token: 0x0400DC4B RID: 56395
				public static LocString NAME = UI.FormatAsLink("Refined Renovations", "REFINEDOBJECTS");

				// Token: 0x0400DC4C RID: 56396
				public static LocString DESC = "Improve base infrastructure with new objects crafted from " + UI.FormatAsLink("Refined Metals", "REFINEDMETAL") + ".";
			}

			// Token: 0x0200349A RID: 13466
			public class GENERICSENSORS
			{
				// Token: 0x0400DC4D RID: 56397
				public static LocString NAME = UI.FormatAsLink("Generic Sensors", "GENERICSENSORS");

				// Token: 0x0400DC4E RID: 56398
				public static LocString DESC = "Drive automation in a variety of new, inventive ways.";
			}

			// Token: 0x0200349B RID: 13467
			public class DUPETRAFFICCONTROL
			{
				// Token: 0x0400DC4F RID: 56399
				public static LocString NAME = UI.FormatAsLink("Computing", "DUPETRAFFICCONTROL");

				// Token: 0x0400DC50 RID: 56400
				public static LocString DESC = "Virtually extend the boundaries of Duplicant imagination.";
			}

			// Token: 0x0200349C RID: 13468
			public class ADVANCEDSCANNERS
			{
				// Token: 0x0400DC51 RID: 56401
				public static LocString NAME = UI.FormatAsLink("Sensitive Microimaging", "ADVANCEDSCANNERS");

				// Token: 0x0400DC52 RID: 56402
				public static LocString DESC = "Computerized systems do the looking, so Duplicants don't have to.";
			}

			// Token: 0x0200349D RID: 13469
			public class SMELTING
			{
				// Token: 0x0400DC53 RID: 56403
				public static LocString NAME = UI.FormatAsLink("Smelting", "SMELTING");

				// Token: 0x0400DC54 RID: 56404
				public static LocString DESC = "High temperatures facilitate the production of purer, special use metal resources.";
			}

			// Token: 0x0200349E RID: 13470
			public class TRAVELTUBES
			{
				// Token: 0x0400DC55 RID: 56405
				public static LocString NAME = UI.FormatAsLink("Transit Tubes", "TRAVELTUBES");

				// Token: 0x0400DC56 RID: 56406
				public static LocString DESC = "A wholly futuristic way to move Duplicants around the base.";
			}

			// Token: 0x0200349F RID: 13471
			public class SMARTSTORAGE
			{
				// Token: 0x0400DC57 RID: 56407
				public static LocString NAME = UI.FormatAsLink("Smart Storage", "SMARTSTORAGE");

				// Token: 0x0400DC58 RID: 56408
				public static LocString DESC = "Completely automate the storage of solid resources.";
			}

			// Token: 0x020034A0 RID: 13472
			public class SOLIDTRANSPORT
			{
				// Token: 0x0400DC59 RID: 56409
				public static LocString NAME = UI.FormatAsLink("Solid Transport", "SOLIDTRANSPORT");

				// Token: 0x0400DC5A RID: 56410
				public static LocString DESC = "Free Duplicants from the drudgery of day-to-day material deliveries with new methods of automation.";
			}

			// Token: 0x020034A1 RID: 13473
			public class SOLIDMANAGEMENT
			{
				// Token: 0x0400DC5B RID: 56411
				public static LocString NAME = UI.FormatAsLink("Solid Management", "SOLIDMANAGEMENT");

				// Token: 0x0400DC5C RID: 56412
				public static LocString DESC = "Make solid decisions in " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " sorting.";
			}

			// Token: 0x020034A2 RID: 13474
			public class SOLIDDISTRIBUTION
			{
				// Token: 0x0400DC5D RID: 56413
				public static LocString NAME = UI.FormatAsLink("Solid Distribution", "SOLIDDISTRIBUTION");

				// Token: 0x0400DC5E RID: 56414
				public static LocString DESC = "Internal rocket hookups for " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x020034A3 RID: 13475
			public class HIGHTEMPFORGING
			{
				// Token: 0x0400DC5F RID: 56415
				public static LocString NAME = UI.FormatAsLink("Superheated Forging", "HIGHTEMPFORGING");

				// Token: 0x0400DC60 RID: 56416
				public static LocString DESC = "Craft entirely new materials by harnessing the most extreme temperatures.";
			}

			// Token: 0x020034A4 RID: 13476
			public class HIGHPRESSUREFORGING
			{
				// Token: 0x0400DC61 RID: 56417
				public static LocString NAME = UI.FormatAsLink("Pressurized Forging", "HIGHPRESSUREFORGING");

				// Token: 0x0400DC62 RID: 56418
				public static LocString DESC = "High pressure diamond forging.";
			}

			// Token: 0x020034A5 RID: 13477
			public class RADIATIONPROTECTION
			{
				// Token: 0x0400DC63 RID: 56419
				public static LocString NAME = UI.FormatAsLink("Radiation Protection", "RADIATIONPROTECTION");

				// Token: 0x0400DC64 RID: 56420
				public static LocString DESC = "Shield Duplicants from dangerous amounts of radiation.";
			}

			// Token: 0x020034A6 RID: 13478
			public class SKYDETECTORS
			{
				// Token: 0x0400DC65 RID: 56421
				public static LocString NAME = UI.FormatAsLink("Celestial Detection", "SKYDETECTORS");

				// Token: 0x0400DC66 RID: 56422
				public static LocString DESC = "Turn Duplicants' eyes to the skies and discover what undiscovered wonders await out there.";
			}

			// Token: 0x020034A7 RID: 13479
			public class MISSILES
			{
				// Token: 0x0400DC67 RID: 56423
				public static LocString NAME = UI.FormatAsLink("Missiles", "MISSILES");

				// Token: 0x0400DC68 RID: 56424
				public static LocString DESC = "Craft explosives and fire them into outer space.";
			}

			// Token: 0x020034A8 RID: 13480
			public class JETPACKS
			{
				// Token: 0x0400DC69 RID: 56425
				public static LocString NAME = UI.FormatAsLink("Personal Flight", "JETPACKS");

				// Token: 0x0400DC6A RID: 56426
				public static LocString DESC = "Give Duplicants the gift of a walk-free commute.";
			}

			// Token: 0x020034A9 RID: 13481
			public class BASICROCKETRY
			{
				// Token: 0x0400DC6B RID: 56427
				public static LocString NAME = UI.FormatAsLink("Introductory Rocketry", "BASICROCKETRY");

				// Token: 0x0400DC6C RID: 56428
				public static LocString DESC = "Everything required for launching the colony's very first space program.";
			}

			// Token: 0x020034AA RID: 13482
			public class ENGINESI
			{
				// Token: 0x0400DC6D RID: 56429
				public static LocString NAME = UI.FormatAsLink("Solid Fuel Combustion", "ENGINESI");

				// Token: 0x0400DC6E RID: 56430
				public static LocString DESC = "Rockets that fly further, longer.";
			}

			// Token: 0x020034AB RID: 13483
			public class ENGINESII
			{
				// Token: 0x0400DC6F RID: 56431
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Combustion", "ENGINESII");

				// Token: 0x0400DC70 RID: 56432
				public static LocString DESC = "Delve deeper into the vastness of space than ever before.";
			}

			// Token: 0x020034AC RID: 13484
			public class ENGINESIII
			{
				// Token: 0x0400DC71 RID: 56433
				public static LocString NAME = UI.FormatAsLink("Cryofuel Combustion", "ENGINESIII");

				// Token: 0x0400DC72 RID: 56434
				public static LocString DESC = "With this technology, the sky is your oyster. Go exploring!";
			}

			// Token: 0x020034AD RID: 13485
			public class CRYOFUELPROPULSION
			{
				// Token: 0x0400DC73 RID: 56435
				public static LocString NAME = UI.FormatAsLink("Cryofuel Propulsion", "CRYOFUELPROPULSION");

				// Token: 0x0400DC74 RID: 56436
				public static LocString DESC = "A semi-powerful engine to propel you further into the galaxy.";
			}

			// Token: 0x020034AE RID: 13486
			public class NUCLEARPROPULSION
			{
				// Token: 0x0400DC75 RID: 56437
				public static LocString NAME = UI.FormatAsLink("Radbolt Propulsion", "NUCLEARPROPULSION");

				// Token: 0x0400DC76 RID: 56438
				public static LocString DESC = "Radical technology to get you to the stars.";
			}

			// Token: 0x020034AF RID: 13487
			public class ADVANCEDRESOURCEEXTRACTION
			{
				// Token: 0x0400DC77 RID: 56439
				public static LocString NAME = UI.FormatAsLink("Advanced Resource Extraction", "ADVANCEDRESOURCEEXTRACTION");

				// Token: 0x0400DC78 RID: 56440
				public static LocString DESC = "Bring back souvieners from the stars.";
			}

			// Token: 0x020034B0 RID: 13488
			public class CARGOI
			{
				// Token: 0x0400DC79 RID: 56441
				public static LocString NAME = UI.FormatAsLink("Solid Cargo", "CARGOI");

				// Token: 0x0400DC7A RID: 56442
				public static LocString DESC = "Make extra use of journeys into space by mining and storing useful resources.";
			}

			// Token: 0x020034B1 RID: 13489
			public class CARGOII
			{
				// Token: 0x0400DC7B RID: 56443
				public static LocString NAME = UI.FormatAsLink("Liquid and Gas Cargo", "CARGOII");

				// Token: 0x0400DC7C RID: 56444
				public static LocString DESC = "Extract precious liquids and gases from the far reaches of space, and return with them to the colony.";
			}

			// Token: 0x020034B2 RID: 13490
			public class CARGOIII
			{
				// Token: 0x0400DC7D RID: 56445
				public static LocString NAME = UI.FormatAsLink("Unique Cargo", "CARGOIII");

				// Token: 0x0400DC7E RID: 56446
				public static LocString DESC = "Allow Duplicants to take their friends to see the stars... or simply bring souvenirs back from their travels.";
			}

			// Token: 0x020034B3 RID: 13491
			public class NOTIFICATIONSYSTEMS
			{
				// Token: 0x0400DC7F RID: 56447
				public static LocString NAME = UI.FormatAsLink("Notification Systems", "NOTIFICATIONSYSTEMS");

				// Token: 0x0400DC80 RID: 56448
				public static LocString DESC = "Get all the news you need to know about your complex colony.";
			}

			// Token: 0x020034B4 RID: 13492
			public class NUCLEARREFINEMENT
			{
				// Token: 0x0400DC81 RID: 56449
				public static LocString NAME = UI.FormatAsLink("Radiation Refinement", "NUCLEAR");

				// Token: 0x0400DC82 RID: 56450
				public static LocString DESC = "Refine uranium and generate radiation.";
			}

			// Token: 0x020034B5 RID: 13493
			public class NUCLEARRESEARCH
			{
				// Token: 0x0400DC83 RID: 56451
				public static LocString NAME = UI.FormatAsLink("Materials Science Research", "NUCLEARRESEARCH");

				// Token: 0x0400DC84 RID: 56452
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter.";
			}

			// Token: 0x020034B6 RID: 13494
			public class ADVANCEDNUCLEARRESEARCH
			{
				// Token: 0x0400DC85 RID: 56453
				public static LocString NAME = UI.FormatAsLink("More Materials Science Research", "ADVANCEDNUCLEARRESEARCH");

				// Token: 0x0400DC86 RID: 56454
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter even more.";
			}

			// Token: 0x020034B7 RID: 13495
			public class NUCLEARSTORAGE
			{
				// Token: 0x0400DC87 RID: 56455
				public static LocString NAME = UI.FormatAsLink("Radbolt Containment", "NUCLEARSTORAGE");

				// Token: 0x0400DC88 RID: 56456
				public static LocString DESC = "Build a quality cache of radbolts.";
			}

			// Token: 0x020034B8 RID: 13496
			public class SOLIDSPACE
			{
				// Token: 0x0400DC89 RID: 56457
				public static LocString NAME = UI.FormatAsLink("Solid Control", "SOLIDSPACE");

				// Token: 0x0400DC8A RID: 56458
				public static LocString DESC = "Transport and sort " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x020034B9 RID: 13497
			public class HIGHVELOCITYTRANSPORT
			{
				// Token: 0x0400DC8B RID: 56459
				public static LocString NAME = UI.FormatAsLink("High Velocity Transport", "HIGHVELOCITY");

				// Token: 0x0400DC8C RID: 56460
				public static LocString DESC = "Hurl things through space.";
			}

			// Token: 0x020034BA RID: 13498
			public class MONUMENTS
			{
				// Token: 0x0400DC8D RID: 56461
				public static LocString NAME = UI.FormatAsLink("Monuments", "MONUMENTS");

				// Token: 0x0400DC8E RID: 56462
				public static LocString DESC = "Monumental art projects.";
			}

			// Token: 0x020034BB RID: 13499
			public class BIOENGINEERING
			{
				// Token: 0x0400DC8F RID: 56463
				public static LocString NAME = UI.FormatAsLink("Bioengineering", "BIOENGINEERING");

				// Token: 0x0400DC90 RID: 56464
				public static LocString DESC = "Mutation station.";
			}

			// Token: 0x020034BC RID: 13500
			public class SPACECOMBUSTION
			{
				// Token: 0x0400DC91 RID: 56465
				public static LocString NAME = UI.FormatAsLink("Advanced Combustion", "SPACECOMBUSTION");

				// Token: 0x0400DC92 RID: 56466
				public static LocString DESC = "Sweet advancements in rocket engines.";
			}

			// Token: 0x020034BD RID: 13501
			public class HIGHVELOCITYDESTRUCTION
			{
				// Token: 0x0400DC93 RID: 56467
				public static LocString NAME = UI.FormatAsLink("High Velocity Destruction", "HIGHVELOCITYDESTRUCTION");

				// Token: 0x0400DC94 RID: 56468
				public static LocString DESC = "Mine the skies.";
			}

			// Token: 0x020034BE RID: 13502
			public class SPACEGAS
			{
				// Token: 0x0400DC95 RID: 56469
				public static LocString NAME = UI.FormatAsLink("Advanced Gas Flow", "SPACEGAS");

				// Token: 0x0400DC96 RID: 56470
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " engines and transportation for rockets.";
			}

			// Token: 0x020034BF RID: 13503
			public class DATASCIENCE
			{
				// Token: 0x0400DC97 RID: 56471
				public static LocString NAME = UI.FormatAsLink("Data Science", "DATASCIENCE");

				// Token: 0x0400DC98 RID: 56472
				public static LocString DESC = "The science of making the data work for my Duplicants, instead of the other way around.";
			}

			// Token: 0x020034C0 RID: 13504
			public class DATASCIENCEBASEGAME
			{
				// Token: 0x0400DC99 RID: 56473
				public static LocString NAME = UI.FormatAsLink("Data Science", "DATASCIENCEBASEGAME");

				// Token: 0x0400DC9A RID: 56474
				public static LocString DESC = "The science of making the data work for my Duplicants, instead of the other way around.";
			}
		}
	}
}
