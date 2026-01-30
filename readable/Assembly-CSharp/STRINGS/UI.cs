using System;
using System.Collections.Generic;

namespace STRINGS
{
	// Token: 0x02000FF0 RID: 4080
	public class UI
	{
		// Token: 0x06007F49 RID: 32585 RVA: 0x00333DD2 File Offset: 0x00331FD2
		public static string FormatAsBuildMenuTab(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x06007F4A RID: 32586 RVA: 0x00333DE4 File Offset: 0x00331FE4
		public static string FormatAsBuildMenuTab(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06007F4B RID: 32587 RVA: 0x00333DFC File Offset: 0x00331FFC
		public static string FormatAsBuildMenuTab(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06007F4C RID: 32588 RVA: 0x00333E14 File Offset: 0x00332014
		public static string FormatAsOverlay(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x06007F4D RID: 32589 RVA: 0x00333E26 File Offset: 0x00332026
		public static string FormatAsOverlay(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06007F4E RID: 32590 RVA: 0x00333E3E File Offset: 0x0033203E
		public static string FormatAsOverlay(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06007F4F RID: 32591 RVA: 0x00333E56 File Offset: 0x00332056
		public static string FormatAsManagementMenu(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x06007F50 RID: 32592 RVA: 0x00333E68 File Offset: 0x00332068
		public static string FormatAsManagementMenu(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06007F51 RID: 32593 RVA: 0x00333E80 File Offset: 0x00332080
		public static string FormatAsManagementMenu(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06007F52 RID: 32594 RVA: 0x00333E98 File Offset: 0x00332098
		public static string FormatAsKeyWord(string text)
		{
			return UI.PRE_KEYWORD + text + UI.PST_KEYWORD;
		}

		// Token: 0x06007F53 RID: 32595 RVA: 0x00333EAA File Offset: 0x003320AA
		public static string FormatAsHotkey(string text)
		{
			return "<b><color=#F44A4A>" + text + "</b></color>";
		}

		// Token: 0x06007F54 RID: 32596 RVA: 0x00333EBC File Offset: 0x003320BC
		public static string FormatAsHotKey(global::Action a)
		{
			return "{Hotkey/" + a.ToString() + "}";
		}

		// Token: 0x06007F55 RID: 32597 RVA: 0x00333EDA File Offset: 0x003320DA
		public static string FormatAsTool(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06007F56 RID: 32598 RVA: 0x00333EF2 File Offset: 0x003320F2
		public static string FormatAsTool(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06007F57 RID: 32599 RVA: 0x00333F0A File Offset: 0x0033210A
		public static string FormatAsLink(string text, string linkID)
		{
			text = UI.StripLinkFormatting(text);
			linkID = CodexCache.FormatLinkID(linkID);
			return string.Concat(new string[]
			{
				"<link=\"",
				linkID,
				"\">",
				text,
				"</link>"
			});
		}

		// Token: 0x06007F58 RID: 32600 RVA: 0x00333F47 File Offset: 0x00332147
		public static string FormatAsPositiveModifier(string text)
		{
			return UI.PRE_POS_MODIFIER + text + UI.PST_POS_MODIFIER;
		}

		// Token: 0x06007F59 RID: 32601 RVA: 0x00333F59 File Offset: 0x00332159
		public static string FormatAsNegativeModifier(string text)
		{
			return UI.PRE_NEG_MODIFIER + text + UI.PST_NEG_MODIFIER;
		}

		// Token: 0x06007F5A RID: 32602 RVA: 0x00333F6B File Offset: 0x0033216B
		public static string FormatAsPositiveRate(string text)
		{
			return UI.PRE_RATE_POSITIVE + text + UI.PST_RATE;
		}

		// Token: 0x06007F5B RID: 32603 RVA: 0x00333F7D File Offset: 0x0033217D
		public static string FormatAsNegativeRate(string text)
		{
			return UI.PRE_RATE_NEGATIVE + text + UI.PST_RATE;
		}

		// Token: 0x06007F5C RID: 32604 RVA: 0x00333F8F File Offset: 0x0033218F
		public static string CLICK(UI.ClickType c)
		{
			return "(ClickType/" + c.ToString() + ")";
		}

		// Token: 0x06007F5D RID: 32605 RVA: 0x00333FAD File Offset: 0x003321AD
		public static string FormatAsAutomationState(string text, UI.AutomationState state)
		{
			if (state == UI.AutomationState.Active)
			{
				return UI.PRE_AUTOMATION_ACTIVE + text + UI.PST_AUTOMATION;
			}
			return UI.PRE_AUTOMATION_STANDBY + text + UI.PST_AUTOMATION;
		}

		// Token: 0x06007F5E RID: 32606 RVA: 0x00333FD3 File Offset: 0x003321D3
		public static string FormatAsCaps(string text)
		{
			return text.ToUpper();
		}

		// Token: 0x06007F5F RID: 32607 RVA: 0x00333FDC File Offset: 0x003321DC
		public static string ExtractLinkID(string text)
		{
			string text2 = text;
			int num = text2.IndexOf("<link=");
			if (num != -1)
			{
				int num2 = num + 7;
				int num3 = text2.IndexOf(">") - 1;
				text2 = text.Substring(num2, num3 - num2);
			}
			return text2;
		}

		// Token: 0x06007F60 RID: 32608 RVA: 0x0033401C File Offset: 0x0033221C
		public static string StripTagFormatting(string text, string tag)
		{
			string text2 = text;
			try
			{
				string text3 = string.Format("<{0}=", tag);
				string text4 = string.Format("</{0}>", tag);
				int length = text4.Length;
				while (text2.Contains(text3))
				{
					int num = text2.IndexOf(text4);
					if (num > -1)
					{
						text2 = text2.Remove(num, length);
					}
					else
					{
						Debug.LogWarningFormat("String has no closing {0} tag: {1}", new object[]
						{
							tag,
							text
						});
					}
					int num2 = text2.IndexOf(text3);
					if (num2 != -1)
					{
						int num3 = text2.IndexOf("\">", num2);
						if (num3 != -1)
						{
							text2 = text2.Remove(num2, num3 - num2 + 2);
						}
						else
						{
							text2 = text2.Remove(num2, text3.Length);
							Debug.LogWarningFormat("String has no open {0} closure: {1}", new object[]
							{
								tag,
								text
							});
						}
					}
					else
					{
						Debug.LogWarningFormat("String has no open {0} tag: {1}", new object[]
						{
							tag,
							text
						});
					}
				}
			}
			catch
			{
				Debug.LogFormat("STRIP TAG FORMATTING FOR {0} FAILED ON: {1}", new object[]
				{
					tag,
					text
				});
				text2 = text;
			}
			return text2;
		}

		// Token: 0x06007F61 RID: 32609 RVA: 0x00334134 File Offset: 0x00332334
		public static string StripLinkFormatting(string text)
		{
			return UI.StripTagFormatting(UI.StripTagFormatting(text, "link"), "LINK");
		}

		// Token: 0x06007F62 RID: 32610 RVA: 0x0033414B File Offset: 0x0033234B
		public static string StripStyleFormatting(string text)
		{
			return UI.StripTagFormatting(UI.StripTagFormatting(text, "style"), "STYLE");
		}

		// Token: 0x04005F88 RID: 24456
		public static string PRE_KEYWORD = "<style=\"KKeyword\">";

		// Token: 0x04005F89 RID: 24457
		public static string PST_KEYWORD = "</style>";

		// Token: 0x04005F8A RID: 24458
		public static string PRE_POS_MODIFIER = "<b>";

		// Token: 0x04005F8B RID: 24459
		public static string PST_POS_MODIFIER = "</b>";

		// Token: 0x04005F8C RID: 24460
		public static string PRE_NEG_MODIFIER = "<b>";

		// Token: 0x04005F8D RID: 24461
		public static string PST_NEG_MODIFIER = "</b>";

		// Token: 0x04005F8E RID: 24462
		public static string PRE_RATE_NEGATIVE = "<style=\"consumed\">";

		// Token: 0x04005F8F RID: 24463
		public static string PRE_RATE_POSITIVE = "<style=\"produced\">";

		// Token: 0x04005F90 RID: 24464
		public static string PST_RATE = "</style>";

		// Token: 0x04005F91 RID: 24465
		public static string CODEXLINK = "REQUIREMENTCLASS";

		// Token: 0x04005F92 RID: 24466
		public static string PRE_AUTOMATION_ACTIVE = "<b><style=\"logic_on\">";

		// Token: 0x04005F93 RID: 24467
		public static string PRE_AUTOMATION_STANDBY = "<b><style=\"logic_off\">";

		// Token: 0x04005F94 RID: 24468
		public static string PST_AUTOMATION = "</style></b>";

		// Token: 0x04005F95 RID: 24469
		public static string YELLOW_PREFIX = "<color=#ffff00ff>";

		// Token: 0x04005F96 RID: 24470
		public static string COLOR_SUFFIX = "</color>";

		// Token: 0x04005F97 RID: 24471
		public static string HORIZONTAL_RULE = "------------------";

		// Token: 0x04005F98 RID: 24472
		public static string HORIZONTAL_BR_RULE = "\n" + UI.HORIZONTAL_RULE + "\n";

		// Token: 0x04005F99 RID: 24473
		public static LocString POS_INFINITY = "Infinity";

		// Token: 0x04005F9A RID: 24474
		public static LocString NEG_INFINITY = "-Infinity";

		// Token: 0x04005F9B RID: 24475
		public static LocString PROCEED_BUTTON = "PROCEED";

		// Token: 0x04005F9C RID: 24476
		public static LocString COPY_BUILDING = "Copy";

		// Token: 0x04005F9D RID: 24477
		public static LocString COPY_BUILDING_TOOLTIP = "Create new build orders using the most recent building selection as a template. {Hotkey}";

		// Token: 0x04005F9E RID: 24478
		public static LocString NAME_WITH_UNITS = "{0} x {1}";

		// Token: 0x04005F9F RID: 24479
		public static LocString NA = "N/A";

		// Token: 0x04005FA0 RID: 24480
		public static LocString POSITIVE_FORMAT = "+{0}";

		// Token: 0x04005FA1 RID: 24481
		public static LocString NEGATIVE_FORMAT = "-{0}";

		// Token: 0x04005FA2 RID: 24482
		public static LocString FILTER = "Filter";

		// Token: 0x04005FA3 RID: 24483
		public static LocString SPEED_SLOW = "SLOW";

		// Token: 0x04005FA4 RID: 24484
		public static LocString SPEED_MEDIUM = "MEDIUM";

		// Token: 0x04005FA5 RID: 24485
		public static LocString SPEED_FAST = "FAST";

		// Token: 0x04005FA6 RID: 24486
		public static LocString RED_ALERT = "RED ALERT";

		// Token: 0x04005FA7 RID: 24487
		public static LocString JOBS = "PRIORITIES";

		// Token: 0x04005FA8 RID: 24488
		public static LocString CONSUMABLES = "CONSUMABLES";

		// Token: 0x04005FA9 RID: 24489
		public static LocString VITALS = "VITALS";

		// Token: 0x04005FAA RID: 24490
		public static LocString RESEARCH = "RESEARCH";

		// Token: 0x04005FAB RID: 24491
		public static LocString ROLES = "JOB ASSIGNMENTS";

		// Token: 0x04005FAC RID: 24492
		public static LocString RESEARCHPOINTS = "Research points";

		// Token: 0x04005FAD RID: 24493
		public static LocString SCHEDULE = "SCHEDULE";

		// Token: 0x04005FAE RID: 24494
		public static LocString REPORT = "REPORTS";

		// Token: 0x04005FAF RID: 24495
		public static LocString SKILLS = "SKILLS";

		// Token: 0x04005FB0 RID: 24496
		public static LocString OVERLAYSTITLE = "OVERLAYS";

		// Token: 0x04005FB1 RID: 24497
		public static LocString ALERTS = "ALERTS";

		// Token: 0x04005FB2 RID: 24498
		public static LocString MESSAGES = "MESSAGES";

		// Token: 0x04005FB3 RID: 24499
		public static LocString ACTIONS = "ACTIONS";

		// Token: 0x04005FB4 RID: 24500
		public static LocString QUEUE = "Queue";

		// Token: 0x04005FB5 RID: 24501
		public static LocString BASECOUNT = "Base {0}";

		// Token: 0x04005FB6 RID: 24502
		public static LocString CHARACTERCONTAINER_SKILLS_TITLE = "ATTRIBUTES";

		// Token: 0x04005FB7 RID: 24503
		public static LocString CHARACTERCONTAINER_TRAITS_TITLE = "TRAITS";

		// Token: 0x04005FB8 RID: 24504
		public static LocString CHARACTERCONTAINER_TRAITS_TITLE_BIONIC = "BIONIC SYSTEMS";

		// Token: 0x04005FB9 RID: 24505
		public static LocString CHARACTERCONTAINER_APTITUDES_TITLE = "INTERESTS";

		// Token: 0x04005FBA RID: 24506
		public static LocString CHARACTERCONTAINER_APTITUDES_TITLE_TOOLTIP = "A Duplicant's starting Attributes are determined by their Interests\n\nLearning Skills related to their Interests will give Duplicants a Morale boost";

		// Token: 0x04005FBB RID: 24507
		public static LocString CHARACTERCONTAINER_EXPECTATIONS_TITLE = "ADDITIONAL INFORMATION";

		// Token: 0x04005FBC RID: 24508
		public static LocString CHARACTERCONTAINER_SKILL_VALUE = " {0} {1}";

		// Token: 0x04005FBD RID: 24509
		public static LocString CHARACTERCONTAINER_NEED = "{0}: {1}";

		// Token: 0x04005FBE RID: 24510
		public static LocString CHARACTERCONTAINER_STRESSTRAIT = "Stress Reaction: {0}";

		// Token: 0x04005FBF RID: 24511
		public static LocString CHARACTERCONTAINER_JOYTRAIT = "Overjoyed Response: {0}";

		// Token: 0x04005FC0 RID: 24512
		public static LocString CHARACTERCONTAINER_CONGENITALTRAIT = "Genetic Trait: {0}";

		// Token: 0x04005FC1 RID: 24513
		public static LocString CHARACTERCONTAINER_NOARCHETYPESELECTED = "Random";

		// Token: 0x04005FC2 RID: 24514
		public static LocString CHARACTERCONTAINER_ARCHETYPESELECT_TOOLTIP = "Change the type of Duplicant the reroll button will produce";

		// Token: 0x04005FC3 RID: 24515
		public static LocString CAREPACKAGECONTAINER_INFORMATION_TITLE = "CARE PACKAGE";

		// Token: 0x04005FC4 RID: 24516
		public static LocString CHARACTERCONTAINER_CONFIRM_OUTFIT_SELECTION_TOOLTIP = "Click to confirm this outfit selection.";

		// Token: 0x04005FC5 RID: 24517
		public static LocString CHARACTERCONTAINER_ALL_MODELS = "Any";

		// Token: 0x04005FC6 RID: 24518
		public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED = "Increased <b>{0}</b>";

		// Token: 0x04005FC7 RID: 24519
		public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED = "Decreased <b>{0}</b>";

		// Token: 0x04005FC8 RID: 24520
		public static LocString CHARACTERCONTAINER_FILTER_STANDARD = "Check box to allow standard Duplicants";

		// Token: 0x04005FC9 RID: 24521
		public static LocString CHARACTERCONTAINER_FILTER_BIONIC = "Check box to allow Bionic Duplicants";

		// Token: 0x04005FCA RID: 24522
		public static LocString CHARACTERCONTAINER_NO_OUTFIT = "Default Outfit";

		// Token: 0x04005FCB RID: 24523
		public static LocString CHARACTERCONTAINER_NEXT_OUTFIT = "Next Outfit";

		// Token: 0x04005FCC RID: 24524
		public static LocString CHARACTERCONTAINER_PREV_OUTFIT = "Previous Outfit";

		// Token: 0x04005FCD RID: 24525
		public static LocString CHARACTERCONTAINER_EXPAND_OUTFIT_SELECTOR_BUTTON = string.Concat(new string[]
		{
			"<b>Current Outfit:</b> {0}\n\nClick to toggle between different outfits owned by this colony\n\nVisit the ",
			UI.PRE_KEYWORD,
			"Supply Closet",
			UI.PST_KEYWORD,
			" to create new outfits"
		});

		// Token: 0x04005FCE RID: 24526
		public static LocString PRODUCTINFO_SELECTMATERIAL = "Select {0}:";

		// Token: 0x04005FCF RID: 24527
		public static LocString PRODUCTINFO_RESEARCHREQUIRED = "Research required...";

		// Token: 0x04005FD0 RID: 24528
		public static LocString PRODUCTINFO_REQUIRESRESEARCHDESC = "Requires research: {0}";

		// Token: 0x04005FD1 RID: 24529
		public static LocString PRODUCTINFO_APPLICABLERESOURCES = "Required resources:";

		// Token: 0x04005FD2 RID: 24530
		public static LocString PRODUCTINFO_MISSINGRESOURCES_TITLE = "Requires {0}: {1}";

		// Token: 0x04005FD3 RID: 24531
		public static LocString PRODUCTINFO_MISSINGRESOURCES_HOVER = "Missing resources";

		// Token: 0x04005FD4 RID: 24532
		public static LocString PRODUCTINFO_MISSINGRESOURCES_DESC = "{0} has yet to be discovered";

		// Token: 0x04005FD5 RID: 24533
		public static LocString PRODUCTINFO_UNIQUE_PER_WORLD = "Limit one per " + UI.CLUSTERMAP.PLANETOID_KEYWORD;

		// Token: 0x04005FD6 RID: 24534
		public static LocString PRODUCTINFO_ROCKET_INTERIOR = "Rocket interior only";

		// Token: 0x04005FD7 RID: 24535
		public static LocString PRODUCTINFO_ROCKET_NOT_INTERIOR = "Cannot build inside rocket";

		// Token: 0x04005FD8 RID: 24536
		public static LocString BUILDTOOL_ROTATE = "Rotate this building";

		// Token: 0x04005FD9 RID: 24537
		public static LocString BUILDTOOL_ROTATE_CURRENT_DEGREES = "Currently rotated {Degrees} degrees";

		// Token: 0x04005FDA RID: 24538
		public static LocString BUILDTOOL_ROTATE_CURRENT_LEFT = "Currently facing left";

		// Token: 0x04005FDB RID: 24539
		public static LocString BUILDTOOL_ROTATE_CURRENT_RIGHT = "Currently facing right";

		// Token: 0x04005FDC RID: 24540
		public static LocString BUILDTOOL_ROTATE_CURRENT_UP = "Currently facing up";

		// Token: 0x04005FDD RID: 24541
		public static LocString BUILDTOOL_ROTATE_CURRENT_DOWN = "Currently facing down";

		// Token: 0x04005FDE RID: 24542
		public static LocString BUILDTOOL_ROTATE_CURRENT_UPRIGHT = "Currently upright";

		// Token: 0x04005FDF RID: 24543
		public static LocString BUILDTOOL_ROTATE_CURRENT_ON_SIDE = "Currently on its side";

		// Token: 0x04005FE0 RID: 24544
		public static LocString BUILDTOOL_CANT_ROTATE = "This building cannot be rotated";

		// Token: 0x04005FE1 RID: 24545
		public static LocString EQUIPMENTTAB_OWNED = "Owned Items";

		// Token: 0x04005FE2 RID: 24546
		public static LocString EQUIPMENTTAB_HELD = "Held Items";

		// Token: 0x04005FE3 RID: 24547
		public static LocString EQUIPMENTTAB_ROOM = "Assigned Rooms";

		// Token: 0x04005FE4 RID: 24548
		public static LocString JOBSCREEN_PRIORITY = "Priority";

		// Token: 0x04005FE5 RID: 24549
		public static LocString JOBSCREEN_HIGH = "High";

		// Token: 0x04005FE6 RID: 24550
		public static LocString JOBSCREEN_LOW = "Low";

		// Token: 0x04005FE7 RID: 24551
		public static LocString JOBSCREEN_EVERYONE = "Everyone";

		// Token: 0x04005FE8 RID: 24552
		public static LocString JOBSCREEN_DEFAULT = "New Duplicants";

		// Token: 0x04005FE9 RID: 24553
		public static LocString BUILD_REQUIRES_SKILL = "Skill: {Skill}";

		// Token: 0x04005FEA RID: 24554
		public static LocString BUILD_REQUIRES_SKILL_TOOLTIP = "At least one Duplicant must have the {Skill} Skill to construct this building";

		// Token: 0x04005FEB RID: 24555
		public static LocString OPERATION_REQUIRES_SKILL = "Skilled Operator: {Skill}";

		// Token: 0x04005FEC RID: 24556
		public static LocString OPERATION_REQUIRES_SKILL_TOOLTIP = "Only a Duplicant with the {Skill} Skill can operate this building";

		// Token: 0x04005FED RID: 24557
		public static LocString VITALSSCREEN_NAME = "Name";

		// Token: 0x04005FEE RID: 24558
		public static LocString VITALSSCREEN_STRESS = "Stress";

		// Token: 0x04005FEF RID: 24559
		public static LocString VITALSSCREEN_HEALTH = "Health";

		// Token: 0x04005FF0 RID: 24560
		public static LocString VITALSSCREEN_SICKNESS = "Disease";

		// Token: 0x04005FF1 RID: 24561
		public static LocString VITALSSCREEN_POWERBANKS = "Power";

		// Token: 0x04005FF2 RID: 24562
		public static LocString VITALSSCREEN_CALORIES = "Fullness";

		// Token: 0x04005FF3 RID: 24563
		public static LocString VITALSSCREEN_RATIONS = "Calories / Cycle";

		// Token: 0x04005FF4 RID: 24564
		public static LocString VITALSSCREEN_EATENTODAY = "Eaten Today";

		// Token: 0x04005FF5 RID: 24565
		public static LocString VITALSSCREEN_RATIONS_TOOLTIP = "Set how many calories this Duplicant may consume daily";

		// Token: 0x04005FF6 RID: 24566
		public static LocString VITALSSCREEN_EATENTODAY_TOOLTIP = "The amount of food this Duplicant has eaten this cycle";

		// Token: 0x04005FF7 RID: 24567
		public static LocString VITALSSCREEN_UNTIL_FULL = "Until Full";

		// Token: 0x04005FF8 RID: 24568
		public static LocString RESEARCHSCREEN_UNLOCKSTOOLTIP = "Unlocks: {0}";

		// Token: 0x04005FF9 RID: 24569
		public static LocString RESEARCHSCREEN_FILTER = "Search Tech";

		// Token: 0x04005FFA RID: 24570
		public static LocString ATTRIBUTELEVEL = "Expertise: Level {0} {1}";

		// Token: 0x04005FFB RID: 24571
		public static LocString ATTRIBUTELEVEL_SHORT = "Level {0} {1}";

		// Token: 0x04005FFC RID: 24572
		public static LocString NEUTRONIUMMASS = "Immeasurable";

		// Token: 0x04005FFD RID: 24573
		public static LocString CALCULATING = "Calculating...";

		// Token: 0x04005FFE RID: 24574
		public static LocString FORMATDAY = "{0:F1} cycles";

		// Token: 0x04005FFF RID: 24575
		public static LocString FORMATSECONDS = "{0}s";

		// Token: 0x04006000 RID: 24576
		public static LocString DELIVERED = "Delivered: {0} {1}";

		// Token: 0x04006001 RID: 24577
		public static LocString PICKEDUP = "Picked Up: {0} {1}";

		// Token: 0x04006002 RID: 24578
		public static LocString COPIED_SETTINGS = "Settings Applied";

		// Token: 0x04006003 RID: 24579
		public static LocString WELCOMEMESSAGETITLE = "- ALERT -";

		// Token: 0x04006004 RID: 24580
		public static LocString WELCOMEMESSAGEBODY = "I've awoken at the target location, but colonization efforts have already hit a hitch. I was supposed to land on the planet's surface, but became trapped many miles underground instead.\n\nAlthough the conditions are not ideal, it's imperative that I establish a colony here and begin mounting efforts to escape.";

		// Token: 0x04006005 RID: 24581
		public static LocString WELCOMEMESSAGEBODY_SPACEDOUT = "The asteroid we call home has collided with an anomalous planet, decimating our colony. Rebuilding it is of the utmost importance.\n\nI've detected a new cluster of material-rich planetoids in nearby space. If I can guide the Duplicants through the perils of space travel, we could build a colony even bigger and better than before.";

		// Token: 0x04006006 RID: 24582
		public static LocString WELCOMEMESSAGEBODY_KF23 = "This asteroid is oddly tilted, as though a powerful external force once knocked it off its axis.\n\nI'll need to recalibrate my approach to colony-building in order to make the most of this unusual distribution of resources.";

		// Token: 0x04006007 RID: 24583
		public static LocString WELCOMEMESSAGEBODY_DLC2_CERES = "The ambient temperatures of this planet are inhospitably low.\n\nI've detected the ruins of a scientifically advanced settlement buried deep beneath our landing site.\n\nIf my Duplicants can survive the journey into this frosty planet's core, we could use this newfound technology to build a colony like no other.";

		// Token: 0x04006008 RID: 24584
		public static LocString WELCOMEMESSAGEBODY_DLC4_PREHISTORIC = "My collision monitoring system has detected an imminent threat to our survival: a huge impactor asteroid is hurtling directly at this planet.\n\nWe must make our way to the surface and mount a defense system in time to destroy the incoming asteroid before it destroys us.";

		// Token: 0x04006009 RID: 24585
		public static LocString WELCOMEMESSAGEBODY_DLC4_PREHISTORIC_SHATTERED = "Impactor asteroid collision in 10 cycles!\n\nMy scans indicate that the impact will trigger the eruption of all geysers that surround our landing site. There are...so many.\n\nInitiate survival procedures immediately.";

		// Token: 0x0400600A RID: 24586
		public static LocString WELCOMEMESSAGEBEGIN = "BEGIN";

		// Token: 0x0400600B RID: 24587
		public static LocString VIEWDUPLICANTS = "Choose a Blueprint";

		// Token: 0x0400600C RID: 24588
		public static LocString DUPLICANTPRINTING = "Duplicant Printing";

		// Token: 0x0400600D RID: 24589
		public static LocString ASSIGNDUPLICANT = "Assign Duplicant";

		// Token: 0x0400600E RID: 24590
		public static LocString CRAFT = "ADD TO QUEUE";

		// Token: 0x0400600F RID: 24591
		public static LocString CLEAR_COMPLETED = "CLEAR COMPLETED ORDERS";

		// Token: 0x04006010 RID: 24592
		public static LocString CRAFT_CONTINUOUS = "CONTINUOUS";

		// Token: 0x04006011 RID: 24593
		public static LocString INCUBATE_CONTINUOUS_TOOLTIP = "When checked, this building will continuously incubate eggs of the selected type";

		// Token: 0x04006012 RID: 24594
		public static LocString PLACEINRECEPTACLE = "Plant";

		// Token: 0x04006013 RID: 24595
		public static LocString REMOVEFROMRECEPTACLE = "Uproot";

		// Token: 0x04006014 RID: 24596
		public static LocString CANCELPLACEINRECEPTACLE = "Cancel";

		// Token: 0x04006015 RID: 24597
		public static LocString CANCELREMOVALFROMRECEPTACLE = "Cancel";

		// Token: 0x04006016 RID: 24598
		public static LocString CHANGEPERSECOND = "Change per second: {0}";

		// Token: 0x04006017 RID: 24599
		public static LocString CHANGEPERCYCLE = "Total change per cycle: {0}";

		// Token: 0x04006018 RID: 24600
		public static LocString CHANGEPERCYCLE_FRESH = "Total change in freshness per cycle: {0}";

		// Token: 0x04006019 RID: 24601
		public static LocString MODIFIER_ITEM_TEMPLATE = "    • {0}: {1}";

		// Token: 0x0400601A RID: 24602
		public static LocString LISTENTRYSTRING = "     {0}\n";

		// Token: 0x0400601B RID: 24603
		public static LocString LISTENTRYSTRINGNOLINEBREAK = "     {0}";

		// Token: 0x02002503 RID: 9475
		public static class PLATFORMS
		{
			// Token: 0x0400A473 RID: 42099
			public static LocString UNKNOWN = "Your game client";

			// Token: 0x0400A474 RID: 42100
			public static LocString STEAM = "Steam";

			// Token: 0x0400A475 RID: 42101
			public static LocString EPIC = "Epic Games Store";

			// Token: 0x0400A476 RID: 42102
			public static LocString WEGAME = "Wegame";
		}

		// Token: 0x02002504 RID: 9476
		private enum KeywordType
		{
			// Token: 0x0400A478 RID: 42104
			Hotkey,
			// Token: 0x0400A479 RID: 42105
			BuildMenu,
			// Token: 0x0400A47A RID: 42106
			Attribute,
			// Token: 0x0400A47B RID: 42107
			Generic
		}

		// Token: 0x02002505 RID: 9477
		public enum ClickType
		{
			// Token: 0x0400A47D RID: 42109
			Click,
			// Token: 0x0400A47E RID: 42110
			Clicked,
			// Token: 0x0400A47F RID: 42111
			Clicking,
			// Token: 0x0400A480 RID: 42112
			Clickable,
			// Token: 0x0400A481 RID: 42113
			Clicks,
			// Token: 0x0400A482 RID: 42114
			click,
			// Token: 0x0400A483 RID: 42115
			clicked,
			// Token: 0x0400A484 RID: 42116
			clicking,
			// Token: 0x0400A485 RID: 42117
			clickable,
			// Token: 0x0400A486 RID: 42118
			clicks,
			// Token: 0x0400A487 RID: 42119
			CLICK,
			// Token: 0x0400A488 RID: 42120
			CLICKED,
			// Token: 0x0400A489 RID: 42121
			CLICKING,
			// Token: 0x0400A48A RID: 42122
			CLICKABLE,
			// Token: 0x0400A48B RID: 42123
			CLICKS
		}

		// Token: 0x02002506 RID: 9478
		public enum AutomationState
		{
			// Token: 0x0400A48D RID: 42125
			Active,
			// Token: 0x0400A48E RID: 42126
			Standby
		}

		// Token: 0x02002507 RID: 9479
		public class VANILLA
		{
			// Token: 0x0400A48F RID: 42127
			public static LocString NAME = "Base Game";

			// Token: 0x0400A490 RID: 42128
			public static LocString NAME_ITAL = "<i>" + UI.VANILLA.NAME + "</i>";
		}

		// Token: 0x02002508 RID: 9480
		public class DLC1
		{
			// Token: 0x0400A491 RID: 42129
			public static LocString NAME = "Spaced Out!";

			// Token: 0x0400A492 RID: 42130
			public static LocString NAME_ITAL = "<i>" + UI.DLC1.NAME + "</i>";
		}

		// Token: 0x02002509 RID: 9481
		public class DLC2
		{
			// Token: 0x0400A493 RID: 42131
			public static LocString NAME = "The Frosty Planet Pack";

			// Token: 0x0400A494 RID: 42132
			public static LocString NAME_ITAL = "<i>" + UI.DLC2.NAME + "</i>";

			// Token: 0x0400A495 RID: 42133
			public static LocString MIXING_TOOLTIP = "<b><i>The Frosty Planet Pack</i></b> features frozen biomes and elements useful in thermal regulation";
		}

		// Token: 0x0200250A RID: 9482
		public class DLC3
		{
			// Token: 0x0400A496 RID: 42134
			public static LocString NAME = "The Bionic Booster Pack";

			// Token: 0x0400A497 RID: 42135
			public static LocString NAME_ITAL = "<i>" + UI.DLC3.NAME + "</i>";

			// Token: 0x0400A498 RID: 42136
			public static LocString MIXING_TOOLTIP = UI.DLC3.NAME_ITAL + " features portable power storage, bionic Duplicants, and remote building operation";
		}

		// Token: 0x0200250B RID: 9483
		public class DLC4
		{
			// Token: 0x0400A499 RID: 42137
			public static LocString NAME = "The Prehistoric Planet Pack";

			// Token: 0x0400A49A RID: 42138
			public static LocString NAME_ITAL = "<i>" + UI.DLC4.NAME + "</i>";

			// Token: 0x0400A49B RID: 42139
			public static LocString MIXING_TOOLTIP = UI.DLC4.NAME_ITAL + " features carnivorous flora and fauna, biodiesel, and a focus on time-sensitive surface defense";
		}

		// Token: 0x0200250C RID: 9484
		public class COSMETIC1
		{
			// Token: 0x0400A49C RID: 42140
			public static LocString NAME = "Neutronium Cosmetics Pack";

			// Token: 0x0400A49D RID: 42141
			public static LocString NAME_ITAL = "<i>" + UI.COSMETIC1.NAME + "</i>";

			// Token: 0x0400A49E RID: 42142
			public static LocString MIXING_TOOLTIP = UI.COSMETIC1.NAME_ITAL + " features cosmetic blueprints fit for royalty";
		}

		// Token: 0x0200250D RID: 9485
		public class DIAGNOSTICS_SCREEN
		{
			// Token: 0x0400A49F RID: 42143
			public static LocString TITLE = "Diagnostics";

			// Token: 0x0400A4A0 RID: 42144
			public static LocString DIAGNOSTIC = "Diagnostic";

			// Token: 0x0400A4A1 RID: 42145
			public static LocString TOTAL = "Total";

			// Token: 0x0400A4A2 RID: 42146
			public static LocString RESERVED = "Reserved";

			// Token: 0x0400A4A3 RID: 42147
			public static LocString STATUS = "Status";

			// Token: 0x0400A4A4 RID: 42148
			public static LocString SEARCH = "Search";

			// Token: 0x0400A4A5 RID: 42149
			public static LocString CRITERIA_HEADER_TOOLTIP = "Expand or collapse diagnostic criteria panel";

			// Token: 0x0400A4A6 RID: 42150
			public static LocString SEE_ALL = "+ See All ({0})";

			// Token: 0x0400A4A7 RID: 42151
			public static LocString CRITERIA_TOOLTIP = "Toggle the <b>{0}</b> diagnostics evaluation of the <b>{1}</b> criteria";

			// Token: 0x0400A4A8 RID: 42152
			public static LocString CRITERIA_ENABLED_COUNT = "{0}/{1} criteria enabled";

			// Token: 0x02002E34 RID: 11828
			public class CLICK_TOGGLE_MESSAGE
			{
				// Token: 0x0400C739 RID: 51001
				public static LocString ALWAYS = UI.CLICK(UI.ClickType.Click) + " to pin this diagnostic to the sidebar - Current State: <b>Visible On Alert Only</b>";

				// Token: 0x0400C73A RID: 51002
				public static LocString ALERT_ONLY = UI.CLICK(UI.ClickType.Click) + " to subscribe to this diagnostic - Current State: <b>Never Visible</b>";

				// Token: 0x0400C73B RID: 51003
				public static LocString NEVER = UI.CLICK(UI.ClickType.Click) + " to mute this diagnostic on the sidebar - Current State: <b>Always Visible</b>";

				// Token: 0x0400C73C RID: 51004
				public static LocString TUTORIAL_DISABLED = UI.CLICK(UI.ClickType.Click) + " to enable this diagnostic -  Current State: <b>Temporarily disabled</b>";
			}
		}

		// Token: 0x0200250E RID: 9486
		public class TEMPORARY_ACTIONS
		{
			// Token: 0x02002E35 RID: 11829
			public class CAMERA_RETURN
			{
				// Token: 0x0400C73D RID: 51005
				public static LocString NAME = "Camera: Return";

				// Token: 0x0400C73E RID: 51006
				public static LocString TOOLTIP = "Return camera to its previous position";
			}
		}

		// Token: 0x0200250F RID: 9487
		public class WORLD_SELECTOR_SCREEN
		{
			// Token: 0x0400A4A9 RID: 42153
			public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;
		}

		// Token: 0x02002510 RID: 9488
		public class COLONY_DIAGNOSTICS
		{
			// Token: 0x0400A4AA RID: 42154
			public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants on this planetoid";

			// Token: 0x0400A4AB RID: 42155
			public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants aboard this rocket";

			// Token: 0x0400A4AC RID: 42156
			public static LocString ROCKET = "rocket";

			// Token: 0x0400A4AD RID: 42157
			public static LocString NO_MINIONS_REQUESTED = "    • Crew must be requested to update this diagnostic";

			// Token: 0x0400A4AE RID: 42158
			public static LocString NO_DATA = "    • Not enough data for evaluation";

			// Token: 0x0400A4AF RID: 42159
			public static LocString NO_DATA_SHORT = "    • No data";

			// Token: 0x0400A4B0 RID: 42160
			public static LocString MUTE_TUTORIAL = "Diagnostic can be muted in the <b><color=#E5B000>See All</color></b> panel";

			// Token: 0x0400A4B1 RID: 42161
			public static LocString GENERIC_STATUS_NORMAL = "All values normal";

			// Token: 0x0400A4B2 RID: 42162
			public static LocString PLACEHOLDER_CRITERIA_NAME = "";

			// Token: 0x0400A4B3 RID: 42163
			public static LocString GENERIC_CRITERIA_PASS = "Criteria met";

			// Token: 0x0400A4B4 RID: 42164
			public static LocString GENERIC_CRITERIA_FAIL = "Criteria not met";

			// Token: 0x02002E36 RID: 11830
			public class GENERIC_CRITERIA
			{
				// Token: 0x0400C73F RID: 51007
				public static LocString CHECKWORLDHASMINIONS = "Check world has Duplicants";
			}

			// Token: 0x02002E37 RID: 11831
			public class IDLEDIAGNOSTIC
			{
				// Token: 0x0400C740 RID: 51008
				public static LocString ALL_NAME = "Idleness";

				// Token: 0x0400C741 RID: 51009
				public static LocString TOOLTIP_NAME = "<b>Idleness</b>";

				// Token: 0x0400C742 RID: 51010
				public static LocString NORMAL = "    • All Duplicants currently have tasks";

				// Token: 0x0400C743 RID: 51011
				public static LocString IDLE = "    • One or more Duplicants are idle";

				// Token: 0x02003B15 RID: 15125
				public static class CRITERIA
				{
					// Token: 0x0400ED27 RID: 60711
					public static LocString CHECKIDLE = "Check idle";

					// Token: 0x0400ED28 RID: 60712
					public static LocString CHECKIDLESEVERE = "Use high severity idle warning";
				}
			}

			// Token: 0x02002E38 RID: 11832
			public class CHOREGROUPDIAGNOSTIC
			{
				// Token: 0x0400C744 RID: 51012
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

				// Token: 0x02003B16 RID: 15126
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002E39 RID: 11833
			public class ALLCHORESDIAGNOSTIC
			{
				// Token: 0x0400C745 RID: 51013
				public static LocString ALL_NAME = "Errands";

				// Token: 0x0400C746 RID: 51014
				public static LocString TOOLTIP_NAME = "<b>Errands</b>";

				// Token: 0x0400C747 RID: 51015
				public static LocString NORMAL = "    • {0} errands pending or in progress";

				// Token: 0x02003B17 RID: 15127
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002E3A RID: 11834
			public class WORKTIMEDIAGNOSTIC
			{
				// Token: 0x0400C748 RID: 51016
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

				// Token: 0x02003B18 RID: 15128
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002E3B RID: 11835
			public class ALLWORKTIMEDIAGNOSTIC
			{
				// Token: 0x0400C749 RID: 51017
				public static LocString ALL_NAME = "Work Time";

				// Token: 0x0400C74A RID: 51018
				public static LocString TOOLTIP_NAME = "<b>Work Time</b>";

				// Token: 0x0400C74B RID: 51019
				public static LocString NORMAL = "    • {0} of Duplicant time spent working";

				// Token: 0x02003B19 RID: 15129
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002E3C RID: 11836
			public class TRAVEL_TIME
			{
				// Token: 0x0400C74C RID: 51020
				public static LocString ALL_NAME = "Travel Time";

				// Token: 0x0400C74D RID: 51021
				public static LocString TOOLTIP_NAME = "<b>Travel Time</b>";

				// Token: 0x0400C74E RID: 51022
				public static LocString NORMAL = "    • {0} of Duplicant time spent traveling between errands";

				// Token: 0x02003B1A RID: 15130
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002E3D RID: 11837
			public class TRAPPEDDUPLICANTDIAGNOSTIC
			{
				// Token: 0x0400C74F RID: 51023
				public static LocString ALL_NAME = "Trapped";

				// Token: 0x0400C750 RID: 51024
				public static LocString TOOLTIP_NAME = "<b>Trapped</b>";

				// Token: 0x0400C751 RID: 51025
				public static LocString NORMAL = "    • No Duplicants are trapped";

				// Token: 0x0400C752 RID: 51026
				public static LocString STUCK = "    • One or more Duplicants are trapped";

				// Token: 0x02003B1B RID: 15131
				public static class CRITERIA
				{
					// Token: 0x0400ED29 RID: 60713
					public static LocString CHECKTRAPPED = "Check Trapped";
				}
			}

			// Token: 0x02002E3E RID: 11838
			public class FLOODEDDIAGNOSTIC
			{
				// Token: 0x0400C753 RID: 51027
				public static LocString ALL_NAME = "Flooded";

				// Token: 0x0400C754 RID: 51028
				public static LocString TOOLTIP_NAME = "<b>Flooded</b>";

				// Token: 0x0400C755 RID: 51029
				public static LocString NORMAL = "    • No buildings are flooded";

				// Token: 0x0400C756 RID: 51030
				public static LocString BUILDING_FLOODED = "    • One or more buildings are flooded";

				// Token: 0x02003B1C RID: 15132
				public static class CRITERIA
				{
					// Token: 0x0400ED2A RID: 60714
					public static LocString CHECKFLOODED = "Check Flooded";
				}
			}

			// Token: 0x02002E3F RID: 11839
			public class BREATHABILITYDIAGNOSTIC
			{
				// Token: 0x0400C757 RID: 51031
				public static LocString ALL_NAME = "Breathability";

				// Token: 0x0400C758 RID: 51032
				public static LocString TOOLTIP_NAME = "<b>Breathability</b>";

				// Token: 0x0400C759 RID: 51033
				public static LocString NORMAL = "    • Oxygen levels are satisfactory";

				// Token: 0x0400C75A RID: 51034
				public static LocString POOR = "    • Oxygen is becoming scarce or low pressure";

				// Token: 0x0400C75B RID: 51035
				public static LocString SUFFOCATING = "    • One or more Duplicants are suffocating";

				// Token: 0x0400C75C RID: 51036
				public static LocString POOR_BIONIC_TANKS = "    • Bionic oxygen tanks are low";

				// Token: 0x0400C75D RID: 51037
				public static LocString NEAR_OR_EMPTY_BIONIC_TANKS = "    • Bionic oxygen tanks are critically low";

				// Token: 0x02003B1D RID: 15133
				public static class CRITERIA
				{
					// Token: 0x0400ED2B RID: 60715
					public static LocString CHECKSUFFOCATION = "Check suffocation";

					// Token: 0x0400ED2C RID: 60716
					public static LocString CHECKLOWBREATHABILITY = "Check low breathability";

					// Token: 0x0400ED2D RID: 60717
					public static LocString CHECKLOWBIONICOXYGEN = "Check low Bionic Duplicant oxygen tanks";
				}
			}

			// Token: 0x02002E40 RID: 11840
			public class STRESSDIAGNOSTIC
			{
				// Token: 0x0400C75E RID: 51038
				public static LocString ALL_NAME = "Max Stress";

				// Token: 0x0400C75F RID: 51039
				public static LocString TOOLTIP_NAME = "<b>Max Stress</b>";

				// Token: 0x0400C760 RID: 51040
				public static LocString HIGH_STRESS = "    • One or more Duplicants is suffering high stress";

				// Token: 0x0400C761 RID: 51041
				public static LocString NORMAL = "    • Duplicants have acceptable stress levels";

				// Token: 0x02003B1E RID: 15134
				public static class CRITERIA
				{
					// Token: 0x0400ED2E RID: 60718
					public static LocString CHECKSTRESSED = "Check stressed";
				}
			}

			// Token: 0x02002E41 RID: 11841
			public class DECORDIAGNOSTIC
			{
				// Token: 0x0400C762 RID: 51042
				public static LocString ALL_NAME = "Decor";

				// Token: 0x0400C763 RID: 51043
				public static LocString TOOLTIP_NAME = "<b>Decor</b>";

				// Token: 0x0400C764 RID: 51044
				public static LocString LOW = "    • Decor levels are low";

				// Token: 0x0400C765 RID: 51045
				public static LocString NORMAL = "    • Decor levels are satisfactory";

				// Token: 0x02003B1F RID: 15135
				public static class CRITERIA
				{
					// Token: 0x0400ED2F RID: 60719
					public static LocString CHECKDECOR = "Check decor";
				}
			}

			// Token: 0x02002E42 RID: 11842
			public class TOILETDIAGNOSTIC
			{
				// Token: 0x0400C766 RID: 51046
				public static LocString ALL_NAME = "Toilets";

				// Token: 0x0400C767 RID: 51047
				public static LocString TOOLTIP_NAME = "<b>Toilets</b>";

				// Token: 0x0400C768 RID: 51048
				public static LocString NO_TOILETS = "    • Colony has no toilets";

				// Token: 0x0400C769 RID: 51049
				public static LocString NO_WORKING_TOILETS = "    • Colony has no working toilets";

				// Token: 0x0400C76A RID: 51050
				public static LocString TOILET_URGENT = "    • Duplicants urgently need to use a toilet";

				// Token: 0x0400C76B RID: 51051
				public static LocString FEW_TOILETS = "    • Toilet-to-Duplicant ratio is low";

				// Token: 0x0400C76C RID: 51052
				public static LocString INOPERATIONAL = "    • One or more toilets are out of order";

				// Token: 0x0400C76D RID: 51053
				public static LocString NORMAL = "    • Colony has adequate working toilets";

				// Token: 0x0400C76E RID: 51054
				public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants with a bladder on this planetoid";

				// Token: 0x0400C76F RID: 51055
				public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants with a bladder aboard this rocket";

				// Token: 0x02003B20 RID: 15136
				public static class CRITERIA
				{
					// Token: 0x0400ED30 RID: 60720
					public static LocString CHECKHASANYTOILETS = "Check has any toilets";

					// Token: 0x0400ED31 RID: 60721
					public static LocString CHECKENOUGHTOILETS = "Check enough toilets";

					// Token: 0x0400ED32 RID: 60722
					public static LocString CHECKBLADDERS = "Check Duplicants really need to use the toilet";
				}
			}

			// Token: 0x02002E43 RID: 11843
			public class BEDDIAGNOSTIC
			{
				// Token: 0x0400C770 RID: 51056
				public static LocString ALL_NAME = "Beds";

				// Token: 0x0400C771 RID: 51057
				public static LocString TOOLTIP_NAME = "<b>Beds</b>";

				// Token: 0x0400C772 RID: 51058
				public static LocString NORMAL = "    • Colony has adequate bedding";

				// Token: 0x0400C773 RID: 51059
				public static LocString NOT_ENOUGH_BEDS = "    • One or more Duplicants are missing a bed";

				// Token: 0x0400C774 RID: 51060
				public static LocString MISSING_ASSIGNMENT = "    • One or more Duplicants don't have an assigned bed";

				// Token: 0x0400C775 RID: 51061
				public static LocString CANT_REACH = "    • One or more Duplicants can't reach their bed";

				// Token: 0x0400C776 RID: 51062
				public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants on this planetoid who need sleep";

				// Token: 0x0400C777 RID: 51063
				public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants aboard this rocket who need sleep";

				// Token: 0x02003B21 RID: 15137
				public static class CRITERIA
				{
					// Token: 0x0400ED33 RID: 60723
					public static LocString CHECKENOUGHBEDS = "Check enough beds";

					// Token: 0x0400ED34 RID: 60724
					public static LocString CHECKREACHABILITY = "Check beds are reachable";
				}
			}

			// Token: 0x02002E44 RID: 11844
			public class FOODDIAGNOSTIC
			{
				// Token: 0x0400C778 RID: 51064
				public static LocString ALL_NAME = "Food";

				// Token: 0x0400C779 RID: 51065
				public static LocString TOOLTIP_NAME = "<b>Food</b>";

				// Token: 0x0400C77A RID: 51066
				public static LocString NORMAL = "    • Food supply is currently adequate";

				// Token: 0x0400C77B RID: 51067
				public static LocString LOW_CALORIES = "    • Food-to-Duplicant ratio is low";

				// Token: 0x0400C77C RID: 51068
				public static LocString HUNGRY = "    • One or more Duplicants are very hungry";

				// Token: 0x0400C77D RID: 51069
				public static LocString NO_FOOD = "    • Duplicants have no food";

				// Token: 0x02003B22 RID: 15138
				public class CRITERIA_HAS_FOOD
				{
					// Token: 0x0400ED35 RID: 60725
					public static LocString PASS = "    • Duplicants have food";

					// Token: 0x0400ED36 RID: 60726
					public static LocString FAIL = "    • Duplicants have no food";
				}

				// Token: 0x02003B23 RID: 15139
				public static class CRITERIA
				{
					// Token: 0x0400ED37 RID: 60727
					public static LocString CHECKENOUGHFOOD = "Check enough food";

					// Token: 0x0400ED38 RID: 60728
					public static LocString CHECKSTARVATION = "Check starvation";
				}
			}

			// Token: 0x02002E45 RID: 11845
			public class FARMDIAGNOSTIC
			{
				// Token: 0x0400C77E RID: 51070
				public static LocString ALL_NAME = "Crops";

				// Token: 0x0400C77F RID: 51071
				public static LocString TOOLTIP_NAME = "<b>Crops</b>";

				// Token: 0x0400C780 RID: 51072
				public static LocString NORMAL = "    • Crops are being grown in sufficient quantity";

				// Token: 0x0400C781 RID: 51073
				public static LocString NONE = "    • No farm plots";

				// Token: 0x0400C782 RID: 51074
				public static LocString NONE_PLANTED = "    • No crops planted";

				// Token: 0x0400C783 RID: 51075
				public static LocString WILTING = "    • One or more crops are wilting";

				// Token: 0x0400C784 RID: 51076
				public static LocString INOPERATIONAL = "    • One or more farm plots are inoperable";

				// Token: 0x02003B24 RID: 15140
				public static class CRITERIA
				{
					// Token: 0x0400ED39 RID: 60729
					public static LocString CHECKHASFARMS = "Check colony has farms";

					// Token: 0x0400ED3A RID: 60730
					public static LocString CHECKPLANTED = "Check farms are planted";

					// Token: 0x0400ED3B RID: 60731
					public static LocString CHECKWILTING = "Check crops wilting";

					// Token: 0x0400ED3C RID: 60732
					public static LocString CHECKOPERATIONAL = "Check farm plots operational";
				}
			}

			// Token: 0x02002E46 RID: 11846
			public class POWERUSEDIAGNOSTIC
			{
				// Token: 0x0400C785 RID: 51077
				public static LocString ALL_NAME = "Power use";

				// Token: 0x0400C786 RID: 51078
				public static LocString TOOLTIP_NAME = "<b>Power use</b>";

				// Token: 0x0400C787 RID: 51079
				public static LocString NORMAL = "    • Power supply is satisfactory";

				// Token: 0x0400C788 RID: 51080
				public static LocString OVERLOADED = "    • One or more power grids are damaged";

				// Token: 0x0400C789 RID: 51081
				public static LocString SIGNIFICANT_POWER_CHANGE_DETECTED = "Significant power use change detected. (Average:{0}, Current:{1})";

				// Token: 0x0400C78A RID: 51082
				public static LocString CIRCUIT_OVER_CAPACITY = "Circuit overloaded {0}/{1}";

				// Token: 0x02003B25 RID: 15141
				public static class CRITERIA
				{
					// Token: 0x0400ED3D RID: 60733
					public static LocString CHECKOVERWATTAGE = "Check circuit overloaded";

					// Token: 0x0400ED3E RID: 60734
					public static LocString CHECKPOWERUSECHANGE = "Check power use change";
				}
			}

			// Token: 0x02002E47 RID: 11847
			public class HEATDIAGNOSTIC
			{
				// Token: 0x0400C78B RID: 51083
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.ALL_NAME;

				// Token: 0x02003B26 RID: 15142
				public static class CRITERIA
				{
					// Token: 0x0400ED3F RID: 60735
					public static LocString CHECKHEAT = "Check heat";
				}
			}

			// Token: 0x02002E48 RID: 11848
			public class BATTERYDIAGNOSTIC
			{
				// Token: 0x0400C78C RID: 51084
				public static LocString ALL_NAME = "Battery";

				// Token: 0x0400C78D RID: 51085
				public static LocString TOOLTIP_NAME = "<b>Battery</b>";

				// Token: 0x0400C78E RID: 51086
				public static LocString NORMAL = "    • All batteries functional";

				// Token: 0x0400C78F RID: 51087
				public static LocString NONE = "    • No batteries are connected to a power grid";

				// Token: 0x0400C790 RID: 51088
				public static LocString DEAD_BATTERY = "    • One or more batteries have died";

				// Token: 0x0400C791 RID: 51089
				public static LocString LIMITED_CAPACITY = "    • Low battery capacity relative to power use";

				// Token: 0x02003B27 RID: 15143
				public class CRITERIA_CHECK_CAPACITY
				{
					// Token: 0x0400ED40 RID: 60736
					public static LocString PASS = "";

					// Token: 0x0400ED41 RID: 60737
					public static LocString FAIL = "";
				}

				// Token: 0x02003B28 RID: 15144
				public static class CRITERIA
				{
					// Token: 0x0400ED42 RID: 60738
					public static LocString CHECKCAPACITY = "Check capacity";

					// Token: 0x0400ED43 RID: 60739
					public static LocString CHECKDEAD = "Check dead";
				}
			}

			// Token: 0x02002E49 RID: 11849
			public class RADIATIONDIAGNOSTIC
			{
				// Token: 0x0400C792 RID: 51090
				public static LocString ALL_NAME = "Radiation";

				// Token: 0x0400C793 RID: 51091
				public static LocString TOOLTIP_NAME = "<b>Radiation</b>";

				// Token: 0x0400C794 RID: 51092
				public static LocString NORMAL = "    • No Radiation concerns";

				// Token: 0x0400C795 RID: 51093
				public static LocString AVERAGE_RADS = "Avg. {0}";

				// Token: 0x02003B29 RID: 15145
				public class CRITERIA_RADIATION_SICKNESS
				{
					// Token: 0x0400ED44 RID: 60740
					public static LocString PASS = "    • No current cases of radiation sickness";

					// Token: 0x0400ED45 RID: 60741
					public static LocString FAIL = "    • One or more Duplicants have radiation sickness";
				}

				// Token: 0x02003B2A RID: 15146
				public class CRITERIA_RADIATION_EXPOSURE
				{
					// Token: 0x0400ED46 RID: 60742
					public static LocString PASS = "    • Safe exposure levels";

					// Token: 0x0400ED47 RID: 60743
					public static LocString FAIL_CONCERN = "    • Exposure levels are above safe limits for one or more Duplicants";

					// Token: 0x0400ED48 RID: 60744
					public static LocString FAIL_WARNING = "    • One or more Duplicants are being exposed to extreme levels of radiation";
				}

				// Token: 0x02003B2B RID: 15147
				public static class CRITERIA
				{
					// Token: 0x0400ED49 RID: 60745
					public static LocString CHECKSICK = "Check sick";

					// Token: 0x0400ED4A RID: 60746
					public static LocString CHECKEXPOSED = "Check exposed";
				}
			}

			// Token: 0x02002E4A RID: 11850
			public class METEORDIAGNOSTIC
			{
				// Token: 0x0400C796 RID: 51094
				public static LocString ALL_NAME = "Meteor Showers";

				// Token: 0x0400C797 RID: 51095
				public static LocString TOOLTIP_NAME = "<b>Meteor Showers</b>";

				// Token: 0x0400C798 RID: 51096
				public static LocString NORMAL = "    • No meteor showers in progress";

				// Token: 0x0400C799 RID: 51097
				public static LocString SHOWER_UNDERWAY = "    • Meteor bombardment underway! {0} remaining";

				// Token: 0x02003B2C RID: 15148
				public static class CRITERIA
				{
					// Token: 0x0400ED4B RID: 60747
					public static LocString CHECKUNDERWAY = "Check meteor bombardment";
				}
			}

			// Token: 0x02002E4B RID: 11851
			public class ENTOMBEDDIAGNOSTIC
			{
				// Token: 0x0400C79A RID: 51098
				public static LocString ALL_NAME = "Entombed";

				// Token: 0x0400C79B RID: 51099
				public static LocString TOOLTIP_NAME = "<b>Entombed</b>";

				// Token: 0x0400C79C RID: 51100
				public static LocString NORMAL = "    • No buildings are entombed";

				// Token: 0x0400C79D RID: 51101
				public static LocString BUILDING_ENTOMBED = "    • One or more buildings are entombed";

				// Token: 0x02003B2D RID: 15149
				public static class CRITERIA
				{
					// Token: 0x0400ED4C RID: 60748
					public static LocString CHECKENTOMBED = "Check entombed";
				}
			}

			// Token: 0x02002E4C RID: 11852
			public class ROCKETFUELDIAGNOSTIC
			{
				// Token: 0x0400C79E RID: 51102
				public static LocString ALL_NAME = "Rocket Fuel";

				// Token: 0x0400C79F RID: 51103
				public static LocString TOOLTIP_NAME = "<b>Rocket Fuel</b>";

				// Token: 0x0400C7A0 RID: 51104
				public static LocString NORMAL = "    • This rocket has sufficient fuel";

				// Token: 0x0400C7A1 RID: 51105
				public static LocString WARNING = "    • This rocket has no fuel";

				// Token: 0x02003B2E RID: 15150
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002E4D RID: 11853
			public class ROCKETOXIDIZERDIAGNOSTIC
			{
				// Token: 0x0400C7A2 RID: 51106
				public static LocString ALL_NAME = "Rocket Oxidizer";

				// Token: 0x0400C7A3 RID: 51107
				public static LocString TOOLTIP_NAME = "<b>Rocket Oxidizer</b>";

				// Token: 0x0400C7A4 RID: 51108
				public static LocString NORMAL = "    • This rocket has sufficient oxidizer";

				// Token: 0x0400C7A5 RID: 51109
				public static LocString WARNING = "    • This rocket has insufficient oxidizer";

				// Token: 0x02003B2F RID: 15151
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002E4E RID: 11854
			public class REACTORDIAGNOSTIC
			{
				// Token: 0x0400C7A6 RID: 51110
				public static LocString ALL_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;

				// Token: 0x0400C7A7 RID: 51111
				public static LocString TOOLTIP_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;

				// Token: 0x0400C7A8 RID: 51112
				public static LocString NORMAL = "    • Safe";

				// Token: 0x0400C7A9 RID: 51113
				public static LocString CRITERIA_TEMPERATURE_WARNING = "    • Temperature dangerously high";

				// Token: 0x0400C7AA RID: 51114
				public static LocString CRITERIA_COOLANT_WARNING = "    • Coolant tank low";

				// Token: 0x02003B30 RID: 15152
				public static class CRITERIA
				{
					// Token: 0x0400ED4D RID: 60749
					public static LocString CHECKTEMPERATURE = "Check temperature";

					// Token: 0x0400ED4E RID: 60750
					public static LocString CHECKCOOLANT = "Check coolant";
				}
			}

			// Token: 0x02002E4F RID: 11855
			public class FLOATINGROCKETDIAGNOSTIC
			{
				// Token: 0x0400C7AB RID: 51115
				public static LocString ALL_NAME = "Flight Status";

				// Token: 0x0400C7AC RID: 51116
				public static LocString TOOLTIP_NAME = "<b>Flight Status</b>";

				// Token: 0x0400C7AD RID: 51117
				public static LocString NORMAL_FLIGHT = "    • This rocket is in flight towards its destination";

				// Token: 0x0400C7AE RID: 51118
				public static LocString NORMAL_UTILITY = "    • This rocket is performing a task at its destination";

				// Token: 0x0400C7AF RID: 51119
				public static LocString NORMAL_LANDED = "    • This rocket is currently landed on a " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;

				// Token: 0x0400C7B0 RID: 51120
				public static LocString WARNING_NO_DESTINATION = "    • This rocket is suspended in space with no set destination";

				// Token: 0x0400C7B1 RID: 51121
				public static LocString WARNING_NO_SPEED = "    • This rocket's flight has been halted";

				// Token: 0x02003B31 RID: 15153
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002E50 RID: 11856
			public class ROCKETINORBITDIAGNOSTIC
			{
				// Token: 0x0400C7B2 RID: 51122
				public static LocString ALL_NAME = "Rockets in Orbit";

				// Token: 0x0400C7B3 RID: 51123
				public static LocString TOOLTIP_NAME = "<b>Rockets in Orbit</b>";

				// Token: 0x0400C7B4 RID: 51124
				public static LocString NORMAL_ONE_IN_ORBIT = "    • {0} is in orbit waiting to land";

				// Token: 0x0400C7B5 RID: 51125
				public static LocString NORMAL_IN_ORBIT = "    • There are {0} rockets in orbit waiting to land";

				// Token: 0x0400C7B6 RID: 51126
				public static LocString WARNING_ONE_ROCKETS_STRANDED = "    • No " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " present. {0} stranded";

				// Token: 0x0400C7B7 RID: 51127
				public static LocString WARNING_ROCKETS_STRANDED = "    • No " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " present. {0} rockets stranded";

				// Token: 0x0400C7B8 RID: 51128
				public static LocString NORMAL_NO_ROCKETS = "    • No rockets waiting to land";

				// Token: 0x02003B32 RID: 15154
				public static class CRITERIA
				{
					// Token: 0x0400ED4F RID: 60751
					public static LocString CHECKORBIT = "Check Orbiting Rockets";
				}
			}

			// Token: 0x02002E51 RID: 11857
			public class BIONICBATTERYDIAGNOSTIC
			{
				// Token: 0x0400C7B9 RID: 51129
				public static LocString ALL_NAME = "Bionic Power";

				// Token: 0x0400C7BA RID: 51130
				public static LocString TOOLTIP_NAME = "<b>Bionic Power</b>";

				// Token: 0x02003B33 RID: 15155
				public class CRITERIA_BATTERIES
				{
					// Token: 0x0400ED50 RID: 60752
					public static LocString PASS = "    • " + UI.FormatAsLink("Power Bank", "ELECTROBANK") + " supply is currently adequate";

					// Token: 0x0400ED51 RID: 60753
					public static LocString NO_POWERBANKS = "    • Colony has no " + UI.FormatAsLink("Power Banks", "ELECTROBANK") + "\n\nBionic Duplicants are at risk of becoming powerless";

					// Token: 0x0400ED52 RID: 60754
					public static LocString LOW_POWERBANKS = "    • " + UI.FormatAsLink("Power Bank", "ELECTROBANK") + " reserves are low:\n\n<indent=20px>    • {0} are currently available</indent>\n<indent=20px>    • {1} are being consumed per cycle</indent>";
				}

				// Token: 0x02003B34 RID: 15156
				public class CRITERIA_POWERLEVEL
				{
					// Token: 0x0400ED53 RID: 60755
					public static LocString CRITICAL_MODE = "    • One or more Duplicants are in desperate need of " + UI.FormatAsLink("Power Banks", "ELECTROBANK");

					// Token: 0x0400ED54 RID: 60756
					public static LocString POWERLESS = "    • One or more Duplicants are incapacitated and in desperate need of " + UI.FormatAsLink("Power Banks", "ELECTROBANK");
				}

				// Token: 0x02003B35 RID: 15157
				public static class CRITERIA
				{
					// Token: 0x0400ED55 RID: 60757
					public static LocString CHECKENOUGHBATTERIES = "Check enough power banks";

					// Token: 0x0400ED56 RID: 60758
					public static LocString CHECKPOWERLEVEL = "Check critical power level";
				}
			}
		}

		// Token: 0x02002511 RID: 9489
		public class SELFCHARGINGBATTERYDIAGNOSTIC
		{
			// Token: 0x0400A4B5 RID: 42165
			public static LocString ALL_NAME = ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.NAME;

			// Token: 0x0400A4B6 RID: 42166
			public static LocString TOOLTIP_NAME = ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.NAME;

			// Token: 0x0400A4B7 RID: 42167
			public static LocString NORMAL = "    • Safe";

			// Token: 0x0400A4B8 RID: 42168
			public static LocString CRITERIA_BATTERYLIFE_WARNING = "    • End-of-lifespan explosion imminent";

			// Token: 0x02002E52 RID: 11858
			public static class CRITERIA
			{
				// Token: 0x0400C7BB RID: 51131
				public static LocString CHECKSELFCHARGINGBATTERYLIFE = "Check lifespan";
			}
		}

		// Token: 0x02002512 RID: 9490
		public class TRACKERS
		{
			// Token: 0x0400A4B9 RID: 42169
			public static LocString BREATHABILITY = "Breathability";

			// Token: 0x0400A4BA RID: 42170
			public static LocString FOOD = "Food";

			// Token: 0x0400A4BB RID: 42171
			public static LocString STRESS = "Max Stress";

			// Token: 0x0400A4BC RID: 42172
			public static LocString IDLE = "Idle Duplicants";
		}

		// Token: 0x02002513 RID: 9491
		public class CONTROLS
		{
			// Token: 0x0400A4BD RID: 42173
			public static LocString PRESS = "Press";

			// Token: 0x0400A4BE RID: 42174
			public static LocString PRESSLOWER = "press";

			// Token: 0x0400A4BF RID: 42175
			public static LocString PRESSUPPER = "PRESS";

			// Token: 0x0400A4C0 RID: 42176
			public static LocString PRESSING = "Pressing";

			// Token: 0x0400A4C1 RID: 42177
			public static LocString PRESSINGLOWER = "pressing";

			// Token: 0x0400A4C2 RID: 42178
			public static LocString PRESSINGUPPER = "PRESSING";

			// Token: 0x0400A4C3 RID: 42179
			public static LocString PRESSED = "Pressed";

			// Token: 0x0400A4C4 RID: 42180
			public static LocString PRESSEDLOWER = "pressed";

			// Token: 0x0400A4C5 RID: 42181
			public static LocString PRESSEDUPPER = "PRESSED";

			// Token: 0x0400A4C6 RID: 42182
			public static LocString PRESSES = "Presses";

			// Token: 0x0400A4C7 RID: 42183
			public static LocString PRESSESLOWER = "presses";

			// Token: 0x0400A4C8 RID: 42184
			public static LocString PRESSESUPPER = "PRESSES";

			// Token: 0x0400A4C9 RID: 42185
			public static LocString PRESSABLE = "Pressable";

			// Token: 0x0400A4CA RID: 42186
			public static LocString PRESSABLELOWER = "pressable";

			// Token: 0x0400A4CB RID: 42187
			public static LocString PRESSABLEUPPER = "PRESSABLE";

			// Token: 0x0400A4CC RID: 42188
			public static LocString CLICK = "Click";

			// Token: 0x0400A4CD RID: 42189
			public static LocString CLICKLOWER = "click";

			// Token: 0x0400A4CE RID: 42190
			public static LocString CLICKUPPER = "CLICK";

			// Token: 0x0400A4CF RID: 42191
			public static LocString CLICKING = "Clicking";

			// Token: 0x0400A4D0 RID: 42192
			public static LocString CLICKINGLOWER = "clicking";

			// Token: 0x0400A4D1 RID: 42193
			public static LocString CLICKINGUPPER = "CLICKING";

			// Token: 0x0400A4D2 RID: 42194
			public static LocString CLICKED = "Clicked";

			// Token: 0x0400A4D3 RID: 42195
			public static LocString CLICKEDLOWER = "clicked";

			// Token: 0x0400A4D4 RID: 42196
			public static LocString CLICKEDUPPER = "CLICKED";

			// Token: 0x0400A4D5 RID: 42197
			public static LocString CLICKS = "Clicks";

			// Token: 0x0400A4D6 RID: 42198
			public static LocString CLICKSLOWER = "clicks";

			// Token: 0x0400A4D7 RID: 42199
			public static LocString CLICKSUPPER = "CLICKS";

			// Token: 0x0400A4D8 RID: 42200
			public static LocString CLICKABLE = "Clickable";

			// Token: 0x0400A4D9 RID: 42201
			public static LocString CLICKABLELOWER = "clickable";

			// Token: 0x0400A4DA RID: 42202
			public static LocString CLICKABLEUPPER = "CLICKABLE";
		}

		// Token: 0x02002514 RID: 9492
		public class MATH_PICTURES
		{
			// Token: 0x02002E53 RID: 11859
			public class AXIS_LABELS
			{
				// Token: 0x0400C7BC RID: 51132
				public static LocString CYCLES = "Cycles";
			}
		}

		// Token: 0x02002515 RID: 9493
		public class SPACEDESTINATIONS
		{
			// Token: 0x02002E54 RID: 11860
			public class WORMHOLE
			{
				// Token: 0x0400C7BD RID: 51133
				public static LocString NAME = "Temporal Tear";

				// Token: 0x0400C7BE RID: 51134
				public static LocString DESCRIPTION = "The source of our misfortune, though it may also be our shot at freedom. Traces of Neutronium are detectable in my readings.";
			}

			// Token: 0x02002E55 RID: 11861
			public class RESEARCHDESTINATION
			{
				// Token: 0x0400C7BF RID: 51135
				public static LocString NAME = "Alluring Anomaly";

				// Token: 0x0400C7C0 RID: 51136
				public static LocString DESCRIPTION = "Our researchers would have a field day with this if they could only get close enough.";
			}

			// Token: 0x02002E56 RID: 11862
			public class DEBRIS
			{
				// Token: 0x02003B36 RID: 15158
				public class SATELLITE
				{
					// Token: 0x0400ED57 RID: 60759
					public static LocString NAME = "Satellite";

					// Token: 0x0400ED58 RID: 60760
					public static LocString DESCRIPTION = "An artificial construct that has escaped its orbit. It no longer appears to be monitored.";
				}
			}

			// Token: 0x02002E57 RID: 11863
			public class NONE
			{
				// Token: 0x0400C7C1 RID: 51137
				public static LocString NAME = "Unselected";
			}

			// Token: 0x02002E58 RID: 11864
			public class ORBIT
			{
				// Token: 0x0400C7C2 RID: 51138
				public static LocString NAME_FMT = "Orbiting {Name}";
			}

			// Token: 0x02002E59 RID: 11865
			public class EMPTY_SPACE
			{
				// Token: 0x0400C7C3 RID: 51139
				public static LocString NAME = "Empty Space";
			}

			// Token: 0x02002E5A RID: 11866
			public class FOG_OF_WAR_SPACE
			{
				// Token: 0x0400C7C4 RID: 51140
				public static LocString NAME = "Unexplored Space";
			}

			// Token: 0x02002E5B RID: 11867
			public class ARTIFACT_POI
			{
				// Token: 0x02003B37 RID: 15159
				public class GRAVITASSPACESTATION1
				{
					// Token: 0x0400ED59 RID: 60761
					public static LocString NAME = "Destroyed Satellite";

					// Token: 0x0400ED5A RID: 60762
					public static LocString DESC = string.Concat(new string[]
					{
						"The remnants of a bygone era, lost in time.\n\n",
						UI.FormatAsLink("Gathering", "EXOBASESDLC1"),
						" space junk requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}

				// Token: 0x02003B38 RID: 15160
				public class GRAVITASSPACESTATION2
				{
					// Token: 0x0400ED5B RID: 60763
					public static LocString NAME = "Demolished Rocket";

					// Token: 0x0400ED5C RID: 60764
					public static LocString DESC = string.Concat(new string[]
					{
						"A defunct rocket from a corporation that vanished long ago.\n\n",
						UI.FormatAsLink("Gathering", "EXOBASESDLC1"),
						" space junk requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}

				// Token: 0x02003B39 RID: 15161
				public class GRAVITASSPACESTATION3
				{
					// Token: 0x0400ED5D RID: 60765
					public static LocString NAME = "Ruined Rocket";

					// Token: 0x0400ED5E RID: 60766
					public static LocString DESC = string.Concat(new string[]
					{
						"The ruins of a rocket that stopped functioning ages ago.\n\n",
						UI.FormatAsLink("Gathering", "EXOBASESDLC1"),
						" space junk requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}

				// Token: 0x02003B3A RID: 15162
				public class GRAVITASSPACESTATION4
				{
					// Token: 0x0400ED5F RID: 60767
					public static LocString NAME = "Retired Planetary Excursion Module";

					// Token: 0x0400ED60 RID: 60768
					public static LocString DESC = string.Concat(new string[]
					{
						"A rocket part from a society that has been wiped out.\n\n",
						UI.FormatAsLink("Gathering", "EXOBASESDLC1"),
						" space junk requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}

				// Token: 0x02003B3B RID: 15163
				public class GRAVITASSPACESTATION5
				{
					// Token: 0x0400ED61 RID: 60769
					public static LocString NAME = "Destroyed Satellite";

					// Token: 0x0400ED62 RID: 60770
					public static LocString DESC = string.Concat(new string[]
					{
						"A destroyed Gravitas satellite.\n\n",
						UI.FormatAsLink("Gathering", "EXOBASESDLC1"),
						" space junk requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}

				// Token: 0x02003B3C RID: 15164
				public class GRAVITASSPACESTATION6
				{
					// Token: 0x0400ED63 RID: 60771
					public static LocString NAME = "Annihilated Satellite";

					// Token: 0x0400ED64 RID: 60772
					public static LocString DESC = string.Concat(new string[]
					{
						"The remains of a satellite made some time in the past.\n\n",
						UI.FormatAsLink("Gathering", "EXOBASESDLC1"),
						" space junk requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}

				// Token: 0x02003B3D RID: 15165
				public class GRAVITASSPACESTATION7
				{
					// Token: 0x0400ED65 RID: 60773
					public static LocString NAME = "Wrecked Space Shuttle";

					// Token: 0x0400ED66 RID: 60774
					public static LocString DESC = string.Concat(new string[]
					{
						"A defunct space shuttle that floats through space unattended.\n\n",
						UI.FormatAsLink("Gathering", "EXOBASESDLC1"),
						" space junk requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}

				// Token: 0x02003B3E RID: 15166
				public class GRAVITASSPACESTATION8
				{
					// Token: 0x0400ED67 RID: 60775
					public static LocString NAME = "Obsolete Space Station Module";

					// Token: 0x0400ED68 RID: 60776
					public static LocString DESC = string.Concat(new string[]
					{
						"The module from a space station that ceased to exist ages ago.\n\n",
						UI.FormatAsLink("Gathering", "EXOBASESDLC1"),
						" space junk requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}

				// Token: 0x02003B3F RID: 15167
				public class RUSSELLSTEAPOT
				{
					// Token: 0x0400ED69 RID: 60777
					public static LocString NAME = "Russell's Teapot";

					// Token: 0x0400ED6A RID: 60778
					public static LocString DESC = string.Concat(new string[]
					{
						"Has never been disproven to not exist.\n\n",
						UI.FormatAsLink("Gathering", "EXOBASESDLC1"),
						" space junk requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}
			}

			// Token: 0x02002E5C RID: 11868
			public class HARVESTABLE_POI
			{
				// Token: 0x0400C7C5 RID: 51141
				public static LocString POI_PRODUCTION = "{0}";

				// Token: 0x0400C7C6 RID: 51142
				public static LocString POI_PRODUCTION_TOOLTIP = "{0}";

				// Token: 0x02003B40 RID: 15168
				public class CARBONASTEROIDFIELD
				{
					// Token: 0x0400ED6B RID: 60779
					public static LocString NAME = "Carbon Asteroid Field";

					// Token: 0x0400ED6C RID: 60780
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid containing ",
						UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
						" and ",
						UI.FormatAsLink("Coal", "CARBON"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B41 RID: 15169
				public class METALLICASTEROIDFIELD
				{
					// Token: 0x0400ED6D RID: 60781
					public static LocString NAME = "Metallic Asteroid Field";

					// Token: 0x0400ED6E RID: 60782
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Iron", "IRON"),
						", ",
						UI.FormatAsLink("Copper", "COPPER"),
						", ",
						UI.FormatAsLink("Lead", "LEAD"),
						", and ",
						UI.FormatAsLink("Obsidian", "OBSIDIAN"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B42 RID: 15170
				public class SATELLITEFIELD
				{
					// Token: 0x0400ED6F RID: 60783
					public static LocString NAME = "Space Debris";

					// Token: 0x0400ED70 RID: 60784
					public static LocString DESC = string.Concat(new string[]
					{
						"Space junk from a forgotten age.\n\n",
						UI.FormatAsLink("Collecting artifacts", "EXOBASESDLC1"),
						" requires a rocket equipped with an ",
						BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME,
						"."
					});
				}

				// Token: 0x02003B43 RID: 15171
				public class ROCKYASTEROIDFIELD
				{
					// Token: 0x0400ED71 RID: 60785
					public static LocString NAME = "Rocky Asteroid Field";

					// Token: 0x0400ED72 RID: 60786
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Granite", "GRANITE"),
						", ",
						UI.FormatAsLink("Abyssalite", "KATAIRITE"),
						", ",
						UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK"),
						", and ",
						UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B44 RID: 15172
				public class INTERSTELLARICEFIELD
				{
					// Token: 0x0400ED73 RID: 60787
					public static LocString NAME = "Ice Asteroid Field";

					// Token: 0x0400ED74 RID: 60788
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Oxygen", "OXYGEN"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B45 RID: 15173
				public class ORGANICMASSFIELD
				{
					// Token: 0x0400ED75 RID: 60789
					public static LocString NAME = "Organic Mass Field";

					// Token: 0x0400ED76 RID: 60790
					public static LocString DESC = string.Concat(new string[]
					{
						"A mass of harvestable resources containing ",
						UI.FormatAsLink("Algae", "ALGAE"),
						", ",
						UI.FormatAsLink("Slime", "SLIMEMOLD"),
						", ",
						UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
						" and ",
						UI.FormatAsLink("Dirt", "DIRT"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B46 RID: 15174
				public class ICEASTEROIDFIELD
				{
					// Token: 0x0400ED77 RID: 60791
					public static LocString NAME = "Exploded Ice Giant";

					// Token: 0x0400ED78 RID: 60792
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of planetary remains containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						", ",
						UI.FormatAsLink("Oxygen", "OXYGEN"),
						" and ",
						UI.FormatAsLink("Natural Gas", "METHANE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B47 RID: 15175
				public class GASGIANTCLOUD
				{
					// Token: 0x0400ED79 RID: 60793
					public static LocString NAME = "Exploded Gas Giant";

					// Token: 0x0400ED7A RID: 60794
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable remains of a planet containing ",
						UI.FormatAsLink("Hydrogen", "HYDROGEN"),
						" in ",
						UI.FormatAsLink("gas", "ELEMENTS_GAS"),
						" form, and ",
						UI.FormatAsLink("Methane", "SOLIDMETHANE"),
						" in ",
						UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
						" and ",
						UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
						" form.\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B48 RID: 15176
				public class CHLORINECLOUD
				{
					// Token: 0x0400ED7B RID: 60795
					public static LocString NAME = "Chlorine Cloud";

					// Token: 0x0400ED7C RID: 60796
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of harvestable debris containing ",
						UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
						" and ",
						UI.FormatAsLink("Bleach Stone", "BLEACHSTONE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B49 RID: 15177
				public class GILDEDASTEROIDFIELD
				{
					// Token: 0x0400ED7D RID: 60797
					public static LocString NAME = "Gilded Asteroid Field";

					// Token: 0x0400ED7E RID: 60798
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Gold", "GOLD"),
						", ",
						UI.FormatAsLink("Fullerene", "FULLERENE"),
						", ",
						UI.FormatAsLink("Regolith", "REGOLITH"),
						" and more.\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B4A RID: 15178
				public class GLIMMERINGASTEROIDFIELD
				{
					// Token: 0x0400ED7F RID: 60799
					public static LocString NAME = "Glimmering Asteroid Field";

					// Token: 0x0400ED80 RID: 60800
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Tungsten", "TUNGSTEN"),
						", ",
						UI.FormatAsLink("Wolframite", "WOLFRAMITE"),
						" and more.\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B4B RID: 15179
				public class HELIUMCLOUD
				{
					// Token: 0x0400ED81 RID: 60801
					public static LocString NAME = "Helium Cloud";

					// Token: 0x0400ED82 RID: 60802
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of resources containing ",
						UI.FormatAsLink("Water", "WATER"),
						" and ",
						UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B4C RID: 15180
				public class OILYASTEROIDFIELD
				{
					// Token: 0x0400ED83 RID: 60803
					public static LocString NAME = "Oily Asteroid Field";

					// Token: 0x0400ED84 RID: 60804
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Solid Methane", "SOLIDMETHANE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B4D RID: 15181
				public class OXIDIZEDASTEROIDFIELD
				{
					// Token: 0x0400ED85 RID: 60805
					public static LocString NAME = "Oxidized Asteroid Field";

					// Token: 0x0400ED86 RID: 60806
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Rust", "RUST"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B4E RID: 15182
				public class SALTYASTEROIDFIELD
				{
					// Token: 0x0400ED87 RID: 60807
					public static LocString NAME = "Salty Asteroid Field";

					// Token: 0x0400ED88 RID: 60808
					public static LocString DESC = string.Concat(new string[]
					{
						"A field of harvestable resources containing ",
						UI.FormatAsLink("Salt Water", "SALTWATER"),
						",",
						UI.FormatAsLink("Brine", "BRINE"),
						" and ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B4F RID: 15183
				public class FROZENOREFIELD
				{
					// Token: 0x0400ED89 RID: 60809
					public static LocString NAME = "Frozen Ore Asteroid Field";

					// Token: 0x0400ED8A RID: 60810
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Polluted Ice", "DIRTYICE"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Snow", "SNOW"),
						" and ",
						UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B50 RID: 15184
				public class FORESTYOREFIELD
				{
					// Token: 0x0400ED8B RID: 60811
					public static LocString NAME = "Forested Ore Field";

					// Token: 0x0400ED8C RID: 60812
					public static LocString DESC = string.Concat(new string[]
					{
						"A field of harvestable resources containing ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						", ",
						UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
						" and ",
						UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B51 RID: 15185
				public class SWAMPYOREFIELD
				{
					// Token: 0x0400ED8D RID: 60813
					public static LocString NAME = "Swampy Ore Field";

					// Token: 0x0400ED8E RID: 60814
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Mud", "MUD"),
						", ",
						UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
						" and ",
						UI.FormatAsLink("Cobalt Ore", "COBALTITE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B52 RID: 15186
				public class SANDYOREFIELD
				{
					// Token: 0x0400ED8F RID: 60815
					public static LocString NAME = "Sandy Ore Field";

					// Token: 0x0400ED90 RID: 60816
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Sandstone", "SANDSTONE"),
						", ",
						UI.FormatAsLink("Algae", "ALGAE"),
						", ",
						UI.FormatAsLink("Copper Ore", "CUPRITE"),
						" and ",
						UI.FormatAsLink("Sand", "SAND"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B53 RID: 15187
				public class RADIOACTIVEGASCLOUD
				{
					// Token: 0x0400ED91 RID: 60817
					public static LocString NAME = "Radioactive Gas Cloud";

					// Token: 0x0400ED92 RID: 60818
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of resources containing ",
						UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
						", ",
						UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
						" and ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B54 RID: 15188
				public class RADIOACTIVEASTEROIDFIELD
				{
					// Token: 0x0400ED93 RID: 60819
					public static LocString NAME = "Radioactive Asteroid Field";

					// Token: 0x0400ED94 RID: 60820
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Bleach Stone", "BLEACHSTONE"),
						", ",
						UI.FormatAsLink("Rust", "RUST"),
						", ",
						UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
						" and ",
						UI.FormatAsLink("Sulfur", "SULFUR"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B55 RID: 15189
				public class OXYGENRICHASTEROIDFIELD
				{
					// Token: 0x0400ED95 RID: 60821
					public static LocString NAME = "Oxygen Rich Asteroid Field";

					// Token: 0x0400ED96 RID: 60822
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
						" and ",
						UI.FormatAsLink("Water", "WATER"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B56 RID: 15190
				public class INTERSTELLAROCEAN
				{
					// Token: 0x0400ED97 RID: 60823
					public static LocString NAME = "Interstellar Ocean";

					// Token: 0x0400ED98 RID: 60824
					public static LocString DESC = string.Concat(new string[]
					{
						"An interplanetary body that consists of ",
						UI.FormatAsLink("Salt Water", "SALTWATER"),
						", ",
						UI.FormatAsLink("Brine", "BRINE"),
						", ",
						UI.FormatAsLink("Salt", "SALT"),
						" and ",
						UI.FormatAsLink("Ice", "ICE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B57 RID: 15191
				public class DLC2CERESFIELD
				{
					// Token: 0x0400ED99 RID: 60825
					public static LocString NAME = "Frozen Cinnabar Asteroid Field";

					// Token: 0x0400ED9A RID: 60826
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable remains of a planet containing ",
						UI.FormatAsLink("Cinnabar", "Cinnabar"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						" and ",
						UI.FormatAsLink("Mercury", "MERCURY"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B58 RID: 15192
				public class DLC2CERESOREFIELD
				{
					// Token: 0x0400ED9B RID: 60827
					public static LocString NAME = "Frozen Mercury Asteroid Field";

					// Token: 0x0400ED9C RID: 60828
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Cinnabar", "Cinnabar"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						" and ",
						UI.FormatAsLink("Mercury", "MERCURY"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B59 RID: 15193
				public class DLC4PREHISTORICOREFIELD
				{
					// Token: 0x0400ED9D RID: 60829
					public static LocString NAME = "Amber Field";

					// Token: 0x0400ED9E RID: 60830
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable remains of a planet containing ",
						UI.FormatAsLink("Nickel Ore", "NICKELORE"),
						", ",
						UI.FormatAsLink("Peat", "PEAT"),
						", ",
						UI.FormatAsLink("Amber", "AMBER"),
						" and ",
						UI.FormatAsLink("Shale", "SHALE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B5A RID: 15194
				public class DLC4PREHISTORICMIXINGFIELD
				{
					// Token: 0x0400ED9F RID: 60831
					public static LocString NAME = "Conductive Ore Field";

					// Token: 0x0400EDA0 RID: 60832
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Nickel Ore", "NICKELORE"),
						", ",
						UI.FormatAsLink("Peat", "PEAT"),
						", ",
						UI.FormatAsLink("Amber", "AMBER"),
						" and ",
						UI.FormatAsLink("Shale", "SHALE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B5B RID: 15195
				public class DLC4IMPACTORDEBRISFIELD1
				{
					// Token: 0x0400EDA1 RID: 60833
					public static LocString NAME = "Demolior Debris";

					// Token: 0x0400EDA2 RID: 60834
					public static LocString DESC = string.Concat(new string[]
					{
						"The solid harvestable remains of Demolior, containing ",
						UI.FormatAsLink("Iridium", "IRIDIUM"),
						", ",
						UI.FormatAsLink("Mafic Rock", "MAFICROCK"),
						", ",
						UI.FormatAsLink("Gold", "GOLD"),
						", and ",
						UI.FormatAsLink("Granite", "GRANITE"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B5C RID: 15196
				public class DLC4IMPACTORDEBRISFIELD2
				{
					// Token: 0x0400EDA3 RID: 60835
					public static LocString NAME = "Liquid Demolior Debris";

					// Token: 0x0400EDA4 RID: 60836
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable liquid remains of Demolior, containing ",
						UI.FormatAsLink("Isosap", "ISORESIN"),
						", ",
						UI.FormatAsLink("Petroleum", "PETROLEUM"),
						", and ",
						UI.FormatAsLink("Liquid Sulfur", "LIQUIDSULFUR"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}

				// Token: 0x02003B5D RID: 15197
				public class DLC4IMPACTORDEBRISFIELD3
				{
					// Token: 0x0400EDA5 RID: 60837
					public static LocString NAME = "Molten Demolior Debris";

					// Token: 0x0400EDA6 RID: 60838
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable molten remains of Demolior, containing ",
						UI.FormatAsLink("Molten Iridium", "MOLTENIRIDIUM"),
						", ",
						UI.FormatAsLink("Magma", "MAGMA"),
						", ",
						UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN"),
						", and ",
						UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN"),
						".\n\n",
						UI.FormatAsLink("Space mining", "EXOBASESDLC1"),
						" requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						" and a cargo module."
					});
				}
			}

			// Token: 0x02002E5D RID: 11869
			public class GRAVITAS_SPACE_POI
			{
				// Token: 0x0400C7C7 RID: 51143
				public static LocString STATION = "Destroyed Gravitas Space Station";
			}

			// Token: 0x02002E5E RID: 11870
			public class TELESCOPE_TARGET
			{
				// Token: 0x0400C7C8 RID: 51144
				public static LocString NAME = "Telescope Target";
			}

			// Token: 0x02002E5F RID: 11871
			public class ASTEROIDS
			{
				// Token: 0x02003B5E RID: 15198
				public class ROCKYASTEROID
				{
					// Token: 0x0400EDA7 RID: 60839
					public static LocString NAME = "Rocky Asteroid";

					// Token: 0x0400EDA8 RID: 60840
					public static LocString DESCRIPTION = "A minor mineral planet. Unlike a comet, it does not possess a tail.";
				}

				// Token: 0x02003B5F RID: 15199
				public class METALLICASTEROID
				{
					// Token: 0x0400EDA9 RID: 60841
					public static LocString NAME = "Metallic Asteroid";

					// Token: 0x0400EDAA RID: 60842
					public static LocString DESCRIPTION = "A shimmering conglomerate of various metals.";
				}

				// Token: 0x02003B60 RID: 15200
				public class CARBONACEOUSASTEROID
				{
					// Token: 0x0400EDAB RID: 60843
					public static LocString NAME = "Carbon Asteroid";

					// Token: 0x0400EDAC RID: 60844
					public static LocString DESCRIPTION = "A common asteroid containing several useful resources.";
				}

				// Token: 0x02003B61 RID: 15201
				public class OILYASTEROID
				{
					// Token: 0x0400EDAD RID: 60845
					public static LocString NAME = "Oily Asteroid";

					// Token: 0x0400EDAE RID: 60846
					public static LocString DESCRIPTION = "A viscous asteroid that is only loosely held together. Contains fossil fuel resources.";
				}

				// Token: 0x02003B62 RID: 15202
				public class GOLDASTEROID
				{
					// Token: 0x0400EDAF RID: 60847
					public static LocString NAME = "Gilded Asteroid";

					// Token: 0x0400EDB0 RID: 60848
					public static LocString DESCRIPTION = "A rich asteroid with thin gold coating and veins of gold deposits throughout.";
				}
			}

			// Token: 0x02002E60 RID: 11872
			public class CLUSTERMAPMETEORSHOWERS
			{
				// Token: 0x02003B63 RID: 15203
				public class UNIDENTIFIED
				{
					// Token: 0x0400EDB1 RID: 60849
					public static LocString NAME = "Unidentified Object";

					// Token: 0x0400EDB2 RID: 60850
					public static LocString DESCRIPTION = "A cosmic anomaly is traveling through the galaxy.\n\nIts origins and purpose are currently unknown, though a " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " could change that.";
				}

				// Token: 0x02003B64 RID: 15204
				public class SLIME
				{
					// Token: 0x0400EDB3 RID: 60851
					public static LocString NAME = "Slimy Meteor Shower";

					// Token: 0x0400EDB4 RID: 60852
					public static LocString DESCRIPTION = "A shower of slimy, biodynamic meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B65 RID: 15205
				public class SNOW
				{
					// Token: 0x0400EDB5 RID: 60853
					public static LocString NAME = "Blizzard Meteor Shower";

					// Token: 0x0400EDB6 RID: 60854
					public static LocString DESCRIPTION = "A shower of cold, cold meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B66 RID: 15206
				public class ICE
				{
					// Token: 0x0400EDB7 RID: 60855
					public static LocString NAME = "Ice Meteor Shower";

					// Token: 0x0400EDB8 RID: 60856
					public static LocString DESCRIPTION = "A hailstorm of icy space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B67 RID: 15207
				public class ICEANDTREES
				{
					// Token: 0x0400EDB9 RID: 60857
					public static LocString NAME = "Icy Nectar Meteor Shower";

					// Token: 0x0400EDBA RID: 60858
					public static LocString DESCRIPTION = "A hailstorm of sweet, icy space rocks on a collision course with the surface of an asteroid";
				}

				// Token: 0x02003B68 RID: 15208
				public class COPPER
				{
					// Token: 0x0400EDBB RID: 60859
					public static LocString NAME = "Copper Meteor Shower";

					// Token: 0x0400EDBC RID: 60860
					public static LocString DESCRIPTION = "A shower of metallic meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B69 RID: 15209
				public class IRON
				{
					// Token: 0x0400EDBD RID: 60861
					public static LocString NAME = "Iron Meteor Shower";

					// Token: 0x0400EDBE RID: 60862
					public static LocString DESCRIPTION = "A shower of metallic space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B6A RID: 15210
				public class GOLD
				{
					// Token: 0x0400EDBF RID: 60863
					public static LocString NAME = "Gold Meteor Shower";

					// Token: 0x0400EDC0 RID: 60864
					public static LocString DESCRIPTION = "A shower of shiny metallic space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B6B RID: 15211
				public class URANIUM
				{
					// Token: 0x0400EDC1 RID: 60865
					public static LocString NAME = "Uranium Meteor Shower";

					// Token: 0x0400EDC2 RID: 60866
					public static LocString DESCRIPTION = "A toxic shower of radioactive meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B6C RID: 15212
				public class LIGHTDUST
				{
					// Token: 0x0400EDC3 RID: 60867
					public static LocString NAME = "Dust Fluff Meteor Shower";

					// Token: 0x0400EDC4 RID: 60868
					public static LocString DESCRIPTION = "A cloud-like shower of dust fluff meteors heading towards the surface of an asteroid.";
				}

				// Token: 0x02003B6D RID: 15213
				public class HEAVYDUST
				{
					// Token: 0x0400EDC5 RID: 60869
					public static LocString NAME = "Dense Dust Meteor Shower";

					// Token: 0x0400EDC6 RID: 60870
					public static LocString DESCRIPTION = "A dark cloud of heavy dust meteors heading towards the surface of an asteroid.";
				}

				// Token: 0x02003B6E RID: 15214
				public class REGOLITH
				{
					// Token: 0x0400EDC7 RID: 60871
					public static LocString NAME = "Regolith Meteor Shower";

					// Token: 0x0400EDC8 RID: 60872
					public static LocString DESCRIPTION = "A shower of rocky meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B6F RID: 15215
				public class OXYLITE
				{
					// Token: 0x0400EDC9 RID: 60873
					public static LocString NAME = "Oxylite Meteor Shower";

					// Token: 0x0400EDCA RID: 60874
					public static LocString DESCRIPTION = "A shower of rocky, oxygen-rich meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B70 RID: 15216
				public class BLEACHSTONE
				{
					// Token: 0x0400EDCB RID: 60875
					public static LocString NAME = "Bleach Stone Meteor Shower";

					// Token: 0x0400EDCC RID: 60876
					public static LocString DESCRIPTION = "A shower of bleach stone meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B71 RID: 15217
				public class MOO
				{
					// Token: 0x0400EDCD RID: 60877
					public static LocString NAME = "Gassy Mooteor Shower";

					// Token: 0x0400EDCE RID: 60878
					public static LocString DESCRIPTION = "A herd of methane-infused meteors that cause a bit of a stink, but do no actual damage.";
				}
			}

			// Token: 0x02002E61 RID: 11873
			public class CLUSTERMAPMETEORS
			{
				// Token: 0x02003B72 RID: 15218
				public class COPPER
				{
					// Token: 0x0400EDCF RID: 60879
					public static LocString NAME = "Copper Meteor";

					// Token: 0x0400EDD0 RID: 60880
					public static LocString DESCRIPTION = "A shower of metallic meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B73 RID: 15219
				public class IRON
				{
					// Token: 0x0400EDD1 RID: 60881
					public static LocString NAME = "Iron Meteor";

					// Token: 0x0400EDD2 RID: 60882
					public static LocString DESCRIPTION = "A shower of metallic space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003B74 RID: 15220
				public class LARGEIMACTOR
				{
					// Token: 0x0400EDD3 RID: 60883
					public static LocString NAME = "Demolior";

					// Token: 0x0400EDD4 RID: 60884
					public static LocString DESCRIPTION = "An ancient impactor asteroid on a collision course with the surface of this world.";
				}
			}

			// Token: 0x02002E62 RID: 11874
			public class COMETS
			{
				// Token: 0x02003B75 RID: 15221
				public class ROCKCOMET
				{
					// Token: 0x0400EDD5 RID: 60885
					public static LocString NAME = "Rock Meteor";
				}

				// Token: 0x02003B76 RID: 15222
				public class DUSTCOMET
				{
					// Token: 0x0400EDD6 RID: 60886
					public static LocString NAME = "Dust Meteor";
				}

				// Token: 0x02003B77 RID: 15223
				public class IRONCOMET
				{
					// Token: 0x0400EDD7 RID: 60887
					public static LocString NAME = "Iron Meteor";
				}

				// Token: 0x02003B78 RID: 15224
				public class COPPERCOMET
				{
					// Token: 0x0400EDD8 RID: 60888
					public static LocString NAME = "Copper Meteor";
				}

				// Token: 0x02003B79 RID: 15225
				public class GOLDCOMET
				{
					// Token: 0x0400EDD9 RID: 60889
					public static LocString NAME = "Gold Meteor";
				}

				// Token: 0x02003B7A RID: 15226
				public class FULLERENECOMET
				{
					// Token: 0x0400EDDA RID: 60890
					public static LocString NAME = "Fullerene Meteor";
				}

				// Token: 0x02003B7B RID: 15227
				public class URANIUMORECOMET
				{
					// Token: 0x0400EDDB RID: 60891
					public static LocString NAME = "Uranium Meteor";
				}

				// Token: 0x02003B7C RID: 15228
				public class NUCLEAR_WASTE
				{
					// Token: 0x0400EDDC RID: 60892
					public static LocString NAME = "Radioactive Meteor";
				}

				// Token: 0x02003B7D RID: 15229
				public class SATELLITE
				{
					// Token: 0x0400EDDD RID: 60893
					public static LocString NAME = "Defunct Satellite";
				}

				// Token: 0x02003B7E RID: 15230
				public class FOODCOMET
				{
					// Token: 0x0400EDDE RID: 60894
					public static LocString NAME = "Snack Bomb";
				}

				// Token: 0x02003B7F RID: 15231
				public class GASSYMOOCOMET
				{
					// Token: 0x0400EDDF RID: 60895
					public static LocString NAME = "Gassy Mooteor";
				}

				// Token: 0x02003B80 RID: 15232
				public class DIESELMOOCOMET
				{
					// Token: 0x0400EDE0 RID: 60896
					public static LocString NAME = "Husky Mooteor";
				}

				// Token: 0x02003B81 RID: 15233
				public class SLIMECOMET
				{
					// Token: 0x0400EDE1 RID: 60897
					public static LocString NAME = "Slime Meteor";
				}

				// Token: 0x02003B82 RID: 15234
				public class SNOWBALLCOMET
				{
					// Token: 0x0400EDE2 RID: 60898
					public static LocString NAME = "Snow Meteor";
				}

				// Token: 0x02003B83 RID: 15235
				public class SPACETREESEEDCOMET
				{
					// Token: 0x0400EDE3 RID: 60899
					public static LocString NAME = "Bonbon Meteor";
				}

				// Token: 0x02003B84 RID: 15236
				public class IRIDIUMCOMET
				{
					// Token: 0x0400EDE4 RID: 60900
					public static LocString NAME = "Iridium Meteor";
				}

				// Token: 0x02003B85 RID: 15237
				public class HARDICECOMET
				{
					// Token: 0x0400EDE5 RID: 60901
					public static LocString NAME = "Ice Meteor";
				}

				// Token: 0x02003B86 RID: 15238
				public class LIGHTDUSTCOMET
				{
					// Token: 0x0400EDE6 RID: 60902
					public static LocString NAME = "Dust Fluff Meteor";
				}

				// Token: 0x02003B87 RID: 15239
				public class ALGAECOMET
				{
					// Token: 0x0400EDE7 RID: 60903
					public static LocString NAME = "Algae Meteor";
				}

				// Token: 0x02003B88 RID: 15240
				public class PHOSPHORICCOMET
				{
					// Token: 0x0400EDE8 RID: 60904
					public static LocString NAME = "Phosphoric Meteor";
				}

				// Token: 0x02003B89 RID: 15241
				public class OXYLITECOMET
				{
					// Token: 0x0400EDE9 RID: 60905
					public static LocString NAME = "Oxylite Meteor";
				}

				// Token: 0x02003B8A RID: 15242
				public class BLEACHSTONECOMET
				{
					// Token: 0x0400EDEA RID: 60906
					public static LocString NAME = "Bleach Stone Meteor";
				}

				// Token: 0x02003B8B RID: 15243
				public class MINICOMET
				{
					// Token: 0x0400EDEB RID: 60907
					public static LocString NAME = "Debris Projectile";
				}
			}

			// Token: 0x02002E63 RID: 11875
			public class DWARFPLANETS
			{
				// Token: 0x02003B8C RID: 15244
				public class ICYDWARF
				{
					// Token: 0x0400EDEC RID: 60908
					public static LocString NAME = "Interstellar Ice";

					// Token: 0x0400EDED RID: 60909
					public static LocString DESCRIPTION = "A terrestrial destination, frozen completely solid.";
				}

				// Token: 0x02003B8D RID: 15245
				public class ORGANICDWARF
				{
					// Token: 0x0400EDEE RID: 60910
					public static LocString NAME = "Organic Mass";

					// Token: 0x0400EDEF RID: 60911
					public static LocString DESCRIPTION = "A mass of organic material similar to the ooze used to print Duplicants. This sample is heavily degraded.";
				}

				// Token: 0x02003B8E RID: 15246
				public class DUSTYDWARF
				{
					// Token: 0x0400EDF0 RID: 60912
					public static LocString NAME = "Dusty Dwarf";

					// Token: 0x0400EDF1 RID: 60913
					public static LocString DESCRIPTION = "A loosely held together composite of minerals.";
				}

				// Token: 0x02003B8F RID: 15247
				public class SALTDWARF
				{
					// Token: 0x0400EDF2 RID: 60914
					public static LocString NAME = "Salty Dwarf";

					// Token: 0x0400EDF3 RID: 60915
					public static LocString DESCRIPTION = "A dwarf planet with unusually high sodium concentrations.";
				}

				// Token: 0x02003B90 RID: 15248
				public class REDDWARF
				{
					// Token: 0x0400EDF4 RID: 60916
					public static LocString NAME = "Red Dwarf";

					// Token: 0x0400EDF5 RID: 60917
					public static LocString DESCRIPTION = "An M-class star orbited by clusters of extractable aluminum and methane.";
				}
			}

			// Token: 0x02002E64 RID: 11876
			public class PLANETS
			{
				// Token: 0x02003B91 RID: 15249
				public class TERRAPLANET
				{
					// Token: 0x0400EDF6 RID: 60918
					public static LocString NAME = "Terrestrial Planet";

					// Token: 0x0400EDF7 RID: 60919
					public static LocString DESCRIPTION = "A planet with a walkable surface, though it does not possess the resources to sustain long-term life.";
				}

				// Token: 0x02003B92 RID: 15250
				public class VOLCANOPLANET
				{
					// Token: 0x0400EDF8 RID: 60920
					public static LocString NAME = "Volcanic Planet";

					// Token: 0x0400EDF9 RID: 60921
					public static LocString DESCRIPTION = "A large terrestrial object composed mainly of molten rock.";
				}

				// Token: 0x02003B93 RID: 15251
				public class SHATTEREDPLANET
				{
					// Token: 0x0400EDFA RID: 60922
					public static LocString NAME = "Shattered Planet";

					// Token: 0x0400EDFB RID: 60923
					public static LocString DESCRIPTION = "A once-habitable planet that has sustained massive damage.\n\nA powerful containment field prevents our rockets from traveling to its surface.";
				}

				// Token: 0x02003B94 RID: 15252
				public class RUSTPLANET
				{
					// Token: 0x0400EDFC RID: 60924
					public static LocString NAME = "Oxidized Asteroid";

					// Token: 0x0400EDFD RID: 60925
					public static LocString DESCRIPTION = "A small planet covered in large swathes of brown rust.";
				}

				// Token: 0x02003B95 RID: 15253
				public class FORESTPLANET
				{
					// Token: 0x0400EDFE RID: 60926
					public static LocString NAME = "Living Planet";

					// Token: 0x0400EDFF RID: 60927
					public static LocString DESCRIPTION = "A small green planet displaying several markers of primitive life.";
				}

				// Token: 0x02003B96 RID: 15254
				public class SHINYPLANET
				{
					// Token: 0x0400EE00 RID: 60928
					public static LocString NAME = "Glimmering Planet";

					// Token: 0x0400EE01 RID: 60929
					public static LocString DESCRIPTION = "A planet composed of rare, shimmering minerals. From the distance, it looks like gem in the sky.";
				}

				// Token: 0x02003B97 RID: 15255
				public class CHLORINEPLANET
				{
					// Token: 0x0400EE02 RID: 60930
					public static LocString NAME = "Chlorine Planet";

					// Token: 0x0400EE03 RID: 60931
					public static LocString DESCRIPTION = "A noxious planet permeated by unbreathable chlorine.";
				}

				// Token: 0x02003B98 RID: 15256
				public class SALTDESERTPLANET
				{
					// Token: 0x0400EE04 RID: 60932
					public static LocString NAME = "Arid Planet";

					// Token: 0x0400EE05 RID: 60933
					public static LocString DESCRIPTION = "A sweltering, desert-like planet covered in surface salt deposits.";
				}

				// Token: 0x02003B99 RID: 15257
				public class DLC2CERESSPACEDESTINATION
				{
					// Token: 0x0400EE06 RID: 60934
					public static LocString NAME = "Ceres";

					// Token: 0x0400EE07 RID: 60935
					public static LocString DESCRIPTION = "A frozen planet peppered with cinnabar deposits.";
				}

				// Token: 0x02003B9A RID: 15258
				public class DLC4PREHISTORICSPACEDESTINATION
				{
					// Token: 0x0400EE08 RID: 60936
					public static LocString NAME = "Prehistoric Ore Field";

					// Token: 0x0400EE09 RID: 60937
					public static LocString DESCRIPTION = "A destination with extractable resources from another era.";
				}

				// Token: 0x02003B9B RID: 15259
				public class DLC4PREHISTORICDEMOLIORSPACEDESTINATION
				{
					// Token: 0x0400EE0A RID: 60938
					public static LocString NAME = "Demolior Debris";

					// Token: 0x0400EE0B RID: 60939
					public static LocString DESCRIPTION = "The remains of an obliterated asteroid containing a renewable source of iridium.";
				}

				// Token: 0x02003B9C RID: 15260
				public class DLC4PREHISTORICDEMOLIORSPACEDESTINATION2
				{
					// Token: 0x0400EE0C RID: 60940
					public static LocString NAME = "Liquid Demolior Debris";

					// Token: 0x0400EE0D RID: 60941
					public static LocString DESCRIPTION = "The liquid remains of an obliterated asteroid containing a renewable source of isosap.";
				}

				// Token: 0x02003B9D RID: 15261
				public class DLC4PREHISTORICDEMOLIORSPACEDESTINATION3
				{
					// Token: 0x0400EE0E RID: 60942
					public static LocString NAME = "Molten Demolior Debris";

					// Token: 0x0400EE0F RID: 60943
					public static LocString DESCRIPTION = "The hot metallic remains of an obliterated asteroid containing a renewable source of iridium.";
				}
			}

			// Token: 0x02002E65 RID: 11877
			public class GIANTS
			{
				// Token: 0x02003B9E RID: 15262
				public class GASGIANT
				{
					// Token: 0x0400EE10 RID: 60944
					public static LocString NAME = "Gas Giant";

					// Token: 0x0400EE11 RID: 60945
					public static LocString DESCRIPTION = "A massive volume of " + UI.FormatAsLink("Hydrogen Gas", "HYDROGEN") + " formed around a small solid center.";
				}

				// Token: 0x02003B9F RID: 15263
				public class ICEGIANT
				{
					// Token: 0x0400EE12 RID: 60946
					public static LocString NAME = "Ice Giant";

					// Token: 0x0400EE13 RID: 60947
					public static LocString DESCRIPTION = "A massive volume of frozen material, primarily composed of " + UI.FormatAsLink("Ice", "ICE") + ".";
				}

				// Token: 0x02003BA0 RID: 15264
				public class HYDROGENGIANT
				{
					// Token: 0x0400EE14 RID: 60948
					public static LocString NAME = "Helium Giant";

					// Token: 0x0400EE15 RID: 60949
					public static LocString DESCRIPTION = "A massive volume of " + UI.FormatAsLink("Helium", "HELIUM") + " formed around a small solid center.";
				}
			}
		}

		// Token: 0x02002516 RID: 9494
		public class SPACEARTIFACTS
		{
			// Token: 0x02002E66 RID: 11878
			public class ARTIFACTTIERS
			{
				// Token: 0x0400C7C9 RID: 51145
				public static LocString TIER_NONE = "Nothing";

				// Token: 0x0400C7CA RID: 51146
				public static LocString TIER0 = "Rarity 0";

				// Token: 0x0400C7CB RID: 51147
				public static LocString TIER1 = "Rarity 1";

				// Token: 0x0400C7CC RID: 51148
				public static LocString TIER2 = "Rarity 2";

				// Token: 0x0400C7CD RID: 51149
				public static LocString TIER3 = "Rarity 3";

				// Token: 0x0400C7CE RID: 51150
				public static LocString TIER4 = "Rarity 4";

				// Token: 0x0400C7CF RID: 51151
				public static LocString TIER5 = "Rarity 5";
			}

			// Token: 0x02002E67 RID: 11879
			public class PACUPERCOLATOR
			{
				// Token: 0x0400C7D0 RID: 51152
				public static LocString NAME = "Percolator";

				// Token: 0x0400C7D1 RID: 51153
				public static LocString DESCRIPTION = "Don't drink from it! There was a pacu... IN the percolator!";

				// Token: 0x0400C7D2 RID: 51154
				public static LocString ARTIFACT = "A coffee percolator with the remnants of a blend of coffee that was a personal favorite of Dr. Hassan Aydem.\n\nHe would specifically reserve the consumption of this particular blend for when he was reviewing research papers on Sunday afternoons.";
			}

			// Token: 0x02002E68 RID: 11880
			public class ROBOTARM
			{
				// Token: 0x0400C7D3 RID: 51155
				public static LocString NAME = "Robot Arm";

				// Token: 0x0400C7D4 RID: 51156
				public static LocString DESCRIPTION = "It's not functional. Just cool.";

				// Token: 0x0400C7D5 RID: 51157
				public static LocString ARTIFACT = "A commercially available robot arm that has had a significant amount of modifications made to it.\n\nThe initials B.A. appear on one of the fingers.";
			}

			// Token: 0x02002E69 RID: 11881
			public class HATCHFOSSIL
			{
				// Token: 0x0400C7D6 RID: 51158
				public static LocString NAME = "Pristine Fossil";

				// Token: 0x0400C7D7 RID: 51159
				public static LocString DESCRIPTION = "The preserved bones of an early species of Hatch.";

				// Token: 0x0400C7D8 RID: 51160
				public static LocString ARTIFACT = "The preservation of this skeleton occurred artificially using a technique called the \"The Ali Method\".\n\nIt should be noted that this fossilization technique was pioneered by one Dr. Ashkan Seyed Ali, an employee of Gravitas.";
			}

			// Token: 0x02002E6A RID: 11882
			public class MODERNART
			{
				// Token: 0x0400C7D9 RID: 51161
				public static LocString NAME = "Modern Art";

				// Token: 0x0400C7DA RID: 51162
				public static LocString DESCRIPTION = "I don't get it.";

				// Token: 0x0400C7DB RID: 51163
				public static LocString ARTIFACT = "A sculpture of the Neoplastism movement of Modern Art.\n\nGravitas records show that this piece was once used in a presentation called 'Form and Function in Corporate Aesthetic'.";
			}

			// Token: 0x02002E6B RID: 11883
			public class EGGROCK
			{
				// Token: 0x0400C7DC RID: 51164
				public static LocString NAME = "Egg-Shaped Rock";

				// Token: 0x0400C7DD RID: 51165
				public static LocString DESCRIPTION = "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.";

				// Token: 0x0400C7DE RID: 51166
				public static LocString ARTIFACT = "The words \"Happy Farters Day Dad. Love Macy\" appear on the bottom of this rock, written in a childlish scrawl.";
			}

			// Token: 0x02002E6C RID: 11884
			public class RAINBOWEGGROCK
			{
				// Token: 0x0400C7DF RID: 51167
				public static LocString NAME = "Egg-Shaped Rock";

				// Token: 0x0400C7E0 RID: 51168
				public static LocString DESCRIPTION = "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.\n\nThis one is rainbow colored.";

				// Token: 0x0400C7E1 RID: 51169
				public static LocString ARTIFACT = "The words \"Happy Father's Day, Dad. Love you!\" appear on the bottom of this rock, written in very neat handwriting. The words are surrounded by four hearts drawn in what appears to be a pink gel pen.";
			}

			// Token: 0x02002E6D RID: 11885
			public class OKAYXRAY
			{
				// Token: 0x0400C7E2 RID: 51170
				public static LocString NAME = "Old X-Ray";

				// Token: 0x0400C7E3 RID: 51171
				public static LocString DESCRIPTION = "Ew, weird. It has five fingers!";

				// Token: 0x0400C7E4 RID: 51172
				public static LocString ARTIFACT = "The description on this X-ray indicates that it was taken in the Gravitas Medical Facility.\n\nMost likely this X-ray was performed while investigating an injury that occurred within the facility.";
			}

			// Token: 0x02002E6E RID: 11886
			public class SHIELDGENERATOR
			{
				// Token: 0x0400C7E5 RID: 51173
				public static LocString NAME = "Shield Generator";

				// Token: 0x0400C7E6 RID: 51174
				public static LocString DESCRIPTION = "A mechanical prototype capable of producing a small section of shielding.";

				// Token: 0x0400C7E7 RID: 51175
				public static LocString ARTIFACT = "The energy field produced by this shield generator completely ignores those light behaviors which are wave-like and focuses instead on its particle behaviors.\n\nThis seemingly paradoxical state is possible when light is slowed down to the point at which it stops entirely.";
			}

			// Token: 0x02002E6F RID: 11887
			public class TEAPOT
			{
				// Token: 0x0400C7E8 RID: 51176
				public static LocString NAME = "Encrusted Teapot";

				// Token: 0x0400C7E9 RID: 51177
				public static LocString DESCRIPTION = "A teapot from the depths of space, coated in a thick layer of Neutronium.";

				// Token: 0x0400C7EA RID: 51178
				public static LocString ARTIFACT = "The amount of Neutronium present in this teapot suggests that it has crossed the threshold of the spacetime continuum on countless occasions, floating through many multiple universes over a plethora of times and spaces.\n\nThough there are, theoretically, an infinite amount of outcomes to any one event over many multi-verses, the homogeneity of the still relatively young multiverse suggests that this is then not the only teapot which has crossed into multiple universes. Despite the infinite possible outcomes of infinite multiverses it appears one high probability constant is that there is, or once was, a teapot floating somewhere in space within every universe.";
			}

			// Token: 0x02002E70 RID: 11888
			public class DNAMODEL
			{
				// Token: 0x0400C7EB RID: 51179
				public static LocString NAME = "Double Helix Model";

				// Token: 0x0400C7EC RID: 51180
				public static LocString DESCRIPTION = "An educational model of genetic information.";

				// Token: 0x0400C7ED RID: 51181
				public static LocString ARTIFACT = "A physical representation of the building blocks of life.\n\nThis one contains trace amounts of a Genetic Ooze prototype that was once used by Gravitas.";
			}

			// Token: 0x02002E71 RID: 11889
			public class SANDSTONE
			{
				// Token: 0x0400C7EE RID: 51182
				public static LocString NAME = "Sandstone";

				// Token: 0x0400C7EF RID: 51183
				public static LocString DESCRIPTION = "A beautiful rock composed of multiple layers of sediment.";

				// Token: 0x0400C7F0 RID: 51184
				public static LocString ARTIFACT = "This sample of sandstone appears to have been processed by the Gravitas Mining Gun that was made available to the general public.\n\nNote: The Gravitas public Mining Gun model is different than ones used by Duplicants in its larger size, and extra precautionary features added in order to be compliant with national safety standards.";
			}

			// Token: 0x02002E72 RID: 11890
			public class MAGMALAMP
			{
				// Token: 0x0400C7F1 RID: 51185
				public static LocString NAME = "Magma Lamp";

				// Token: 0x0400C7F2 RID: 51186
				public static LocString DESCRIPTION = "The sequel to \"Lava Lamp\".";

				// Token: 0x0400C7F3 RID: 51187
				public static LocString ARTIFACT = "Molten lava and obsidian combined in a way that allows the lava to maintain just enough heat to remain in liquid form.\n\nPlans of this lamp found in the Gravitas archives have been attributed to one Robin Nisbet, PhD.";
			}

			// Token: 0x02002E73 RID: 11891
			public class OBELISK
			{
				// Token: 0x0400C7F4 RID: 51188
				public static LocString NAME = "Small Obelisk";

				// Token: 0x0400C7F5 RID: 51189
				public static LocString DESCRIPTION = "A rectangular stone piece.\n\nIts function is unclear.";

				// Token: 0x0400C7F6 RID: 51190
				public static LocString ARTIFACT = "On close inspection this rectangle is actually a stone box built with a covert, almost seamless, lid, housing a tiny key.\n\nIt is still unclear what the key unlocks.";
			}

			// Token: 0x02002E74 RID: 11892
			public class RUBIKSCUBE
			{
				// Token: 0x0400C7F7 RID: 51191
				public static LocString NAME = "Rubik's Cube";

				// Token: 0x0400C7F8 RID: 51192
				public static LocString DESCRIPTION = "This mystery of the universe has already been solved.";

				// Token: 0x0400C7F9 RID: 51193
				public static LocString ARTIFACT = "A well-used, competition-compliant version of the popular puzzle cube.\n\nIt's worth noting that Dr. Dylan 'Nails' Winslow was once a regional Rubik's Cube champion.";
			}

			// Token: 0x02002E75 RID: 11893
			public class OFFICEMUG
			{
				// Token: 0x0400C7FA RID: 51194
				public static LocString NAME = "Office Mug";

				// Token: 0x0400C7FB RID: 51195
				public static LocString DESCRIPTION = "An intermediary place to store espresso before you move it to your mouth.";

				// Token: 0x0400C7FC RID: 51196
				public static LocString ARTIFACT = "An office mug with the Gravitas logo on it. Though their office mugs were all emblazoned with the same logo, Gravitas colored their mugs differently to distinguish between their various departments.\n\nThis one is from the AI department.";
			}

			// Token: 0x02002E76 RID: 11894
			public class AMELIASWATCH
			{
				// Token: 0x0400C7FD RID: 51197
				public static LocString NAME = "Wrist Watch";

				// Token: 0x0400C7FE RID: 51198
				public static LocString DESCRIPTION = "It was discovered in a package labeled \"To be entrusted to Dr. Walker\".";

				// Token: 0x0400C7FF RID: 51199
				public static LocString ARTIFACT = "This watch once belonged to pioneering aviator Amelia Earhart and travelled to space via astronaut Dr. Shannon Walker.\n\nHow it came to be floating in space is a matter of speculation, but perhaps the adventurous spirit of its original stewards became infused within the fabric of this timepiece and compelled the universe to launch it into the great unknown.";
			}

			// Token: 0x02002E77 RID: 11895
			public class MOONMOONMOON
			{
				// Token: 0x0400C800 RID: 51200
				public static LocString NAME = "Moonmoonmoon";

				// Token: 0x0400C801 RID: 51201
				public static LocString DESCRIPTION = "A moon's moon's moon. It's very small.";

				// Token: 0x0400C802 RID: 51202
				public static LocString ARTIFACT = "In contrast to most moons, this object's glowing properties do not come from reflecting an external source of light, but rather from an internal glow of mysterious origin.\n\nThe glow of this object also grants an extraordinary amount of Decor bonus to nearby Duplicants, almost as if it was designed that way.";
			}

			// Token: 0x02002E78 RID: 11896
			public class BIOLUMINESCENTROCK
			{
				// Token: 0x0400C803 RID: 51203
				public static LocString NAME = "Bioluminescent Rock";

				// Token: 0x0400C804 RID: 51204
				public static LocString DESCRIPTION = "A thriving colony of tiny, microscopic organisms is responsible for giving it its bluish glow.";

				// Token: 0x0400C805 RID: 51205
				public static LocString ARTIFACT = "The microscopic organisms within this rock are of a unique variety whose genetic code shows many tell-tale signs of being genetically engineered within a lab.\n\nFurther analysis reveals they share 99.999% of their genetic code with Shine Bugs.";
			}

			// Token: 0x02002E79 RID: 11897
			public class PLASMALAMP
			{
				// Token: 0x0400C806 RID: 51206
				public static LocString NAME = "Plasma Lamp";

				// Token: 0x0400C807 RID: 51207
				public static LocString DESCRIPTION = "No space colony is complete without one.";

				// Token: 0x0400C808 RID: 51208
				public static LocString ARTIFACT = "The bottom of this lamp contains the words 'Property of the Atmospheric Sciences Department'.\n\nIt's worth noting that the Gravitas Atmospheric Sciences Department once simulated an experiment testing the feasibility of survival in an environment filled with noble gasses, similar to the ones contained within this device.";
			}

			// Token: 0x02002E7A RID: 11898
			public class MOLDAVITE
			{
				// Token: 0x0400C809 RID: 51209
				public static LocString NAME = "Moldavite";

				// Token: 0x0400C80A RID: 51210
				public static LocString DESCRIPTION = "A unique green stone formed from the impact of a meteorite.";

				// Token: 0x0400C80B RID: 51211
				public static LocString ARTIFACT = "This extremely rare, museum grade moldavite once sat on the desk of Dr. Ren Sato, but it was stolen by some unknown person.\n\nDr. Sato suspected the perpetrator was none other than Director Stern, but was never able to confirm this theory.";
			}

			// Token: 0x02002E7B RID: 11899
			public class BRICKPHONE
			{
				// Token: 0x0400C80C RID: 51212
				public static LocString NAME = "Strange Brick";

				// Token: 0x0400C80D RID: 51213
				public static LocString DESCRIPTION = "It still works.";

				// Token: 0x0400C80E RID: 51214
				public static LocString ARTIFACT = "This cordless phone once held a direct line to an unknown location in which strange distant voices can be heard but not understood, nor interacted with.\n\nThough Gravitas spent a lot of money and years of study dedicated to discovering its secret, the mystery was never solved.";
			}

			// Token: 0x02002E7C RID: 11900
			public class SOLARSYSTEM
			{
				// Token: 0x0400C80F RID: 51215
				public static LocString NAME = "Self-Contained System";

				// Token: 0x0400C810 RID: 51216
				public static LocString DESCRIPTION = "A marvel of the cosmos, inside this display is an entirely self-contained solar system.";

				// Token: 0x0400C811 RID: 51217
				public static LocString ARTIFACT = "This marvel of a device was built using parts from an old Tornado-in-a-Box science fair project.\n\nVery faint, faded letters are still visible on the display bottom that read 'Camille P. Grade 5'.";
			}

			// Token: 0x02002E7D RID: 11901
			public class SINK
			{
				// Token: 0x0400C812 RID: 51218
				public static LocString NAME = "Sink";

				// Token: 0x0400C813 RID: 51219
				public static LocString DESCRIPTION = "No collection is complete without it.";

				// Token: 0x0400C814 RID: 51220
				public static LocString ARTIFACT = "A small trace of encrusted soap on this sink strongly suggests it was installed in a personal bathroom, rather than a public one which would have used a soap dispenser.\n\nThe soap sliver is light blue and contains a manufactured blueberry fragrance.";
			}

			// Token: 0x02002E7E RID: 11902
			public class ROCKTORNADO
			{
				// Token: 0x0400C815 RID: 51221
				public static LocString NAME = "Tornado Rock";

				// Token: 0x0400C816 RID: 51222
				public static LocString DESCRIPTION = "It's unclear how it formed, although I'm glad it did.";

				// Token: 0x0400C817 RID: 51223
				public static LocString ARTIFACT = "Speculations about the origin of this rock include a paper written by one Harold P. Moreson, Ph.D. in which he theorized it could be a rare form of hollow geode which failed to form any crystals inside.\n\nThis paper appears in the Gravitas archives, and in all probability, was one of the factors in the hiring of Moreson into the Geology department of the company.";
			}

			// Token: 0x02002E7F RID: 11903
			public class BLENDER
			{
				// Token: 0x0400C818 RID: 51224
				public static LocString NAME = "Blender";

				// Token: 0x0400C819 RID: 51225
				public static LocString DESCRIPTION = "Equipment used to conduct experiments answering the age-old question, \"Could that blend\"?";

				// Token: 0x0400C81A RID: 51226
				public static LocString ARTIFACT = "Trace amounts of edible foodstuffs present in this blender indicate that it was probably used to emulsify the ingredients of a mush bar.\n\nIt is also very likely that it was employed at least once in the production of a peanut butter and banana smoothie.";
			}

			// Token: 0x02002E80 RID: 11904
			public class SAXOPHONE
			{
				// Token: 0x0400C81B RID: 51227
				public static LocString NAME = "Mangled Saxophone";

				// Token: 0x0400C81C RID: 51228
				public static LocString DESCRIPTION = "The name \"Pesquet\" is barely legible on the inside.";

				// Token: 0x0400C81D RID: 51229
				public static LocString ARTIFACT = "Though it is often remarked that \"in space, no one can hear you scream\", Thomas Pesquet proved the same cannot be said for the smooth jazzy sounds of a saxophone.\n\nAlthough this instrument once belonged to the eminent French Astronaut its current bumped and bent shape suggests it has seen many adventures beyond that of just being used to perform an out-of-this-world saxophone solo.";
			}

			// Token: 0x02002E81 RID: 11905
			public class STETHOSCOPE
			{
				// Token: 0x0400C81E RID: 51230
				public static LocString NAME = "Stethoscope";

				// Token: 0x0400C81F RID: 51231
				public static LocString DESCRIPTION = "Listens to Duplicant heartbeats, or gurgly tummies.";

				// Token: 0x0400C820 RID: 51232
				public static LocString ARTIFACT = "The size and shape of this stethescope suggests it was not intended to be used by neither a human-sized nor a Duplicant-sized person but something half-way in between the two beings.";
			}

			// Token: 0x02002E82 RID: 11906
			public class VHS
			{
				// Token: 0x0400C821 RID: 51233
				public static LocString NAME = "Archaic Tech";

				// Token: 0x0400C822 RID: 51234
				public static LocString DESCRIPTION = "Be kind when you handle it. It's very fragile.";

				// Token: 0x0400C823 RID: 51235
				public static LocString ARTIFACT = "The label on this VHS tape reads \"Jackie and Olivia's House Warming Party\".\n\nUnfortunately, a device with which to play this recording no longer exists in this universe.";
			}

			// Token: 0x02002E83 RID: 11907
			public class REACTORMODEL
			{
				// Token: 0x0400C824 RID: 51236
				public static LocString NAME = "Model Nuclear Power Plant";

				// Token: 0x0400C825 RID: 51237
				public static LocString DESCRIPTION = "It's pronounced nu-clear.";

				// Token: 0x0400C826 RID: 51238
				public static LocString ARTIFACT = "Though this Nuclear Power Plant was never built, this model exists as an artifact to a time early in the life of Gravitas when it was researching all alternatives to solving the global energy problem.\n\nUltimately, the idea of building a Nuclear Power Plant was abandoned in favor of the \"much safer\" alternative of developing the Temporal Bow.";
			}

			// Token: 0x02002E84 RID: 11908
			public class MOODRING
			{
				// Token: 0x0400C827 RID: 51239
				public static LocString NAME = "Radiation Mood Ring";

				// Token: 0x0400C828 RID: 51240
				public static LocString DESCRIPTION = "How radioactive are you feeling?";

				// Token: 0x0400C829 RID: 51241
				public static LocString ARTIFACT = "A wholly unique ring not found anywhere outside of the Gravitas Laboratory.\n\nThough it can't be determined for sure who worked on this extraordinary curiousity it's worth noting that, for his Ph.D. thesis, Dr. Travaldo Farrington wrote a paper entitled \"Novelty Uses for Radiochromatic Dyes\".";
			}

			// Token: 0x02002E85 RID: 11909
			public class ORACLE
			{
				// Token: 0x0400C82A RID: 51242
				public static LocString NAME = "Useless Machine";

				// Token: 0x0400C82B RID: 51243
				public static LocString DESCRIPTION = "What does it do?";

				// Token: 0x0400C82C RID: 51244
				public static LocString ARTIFACT = "All of the parts for this contraption are recycled from projects abandoned by the Robotics department.\n\nThe design is very close to one published in an amateur DIY magazine that once sat in the lobby of the 'Employees Only' area of Gravitas' facilities.";
			}

			// Token: 0x02002E86 RID: 11910
			public class GRUBSTATUE
			{
				// Token: 0x0400C82D RID: 51245
				public static LocString NAME = "Grubgrub Statue";

				// Token: 0x0400C82E RID: 51246
				public static LocString DESCRIPTION = "A moving tribute to a tiny plant hugger.";

				// Token: 0x0400C82F RID: 51247
				public static LocString ARTIFACT = "It's very likely this statue was placed in a hidden, secluded place in the Gravitas laboratory since the creation of Grubgrubs was a closely held secret that the general public was not privy to.\n\nThis is a shame since the artistic quality of this statue is really quite accomplished.";
			}

			// Token: 0x02002E87 RID: 11911
			public class HONEYJAR
			{
				// Token: 0x0400C830 RID: 51248
				public static LocString NAME = "Honey Jar";

				// Token: 0x0400C831 RID: 51249
				public static LocString DESCRIPTION = "Sweet golden liquid with just a touch of uranium.";

				// Token: 0x0400C832 RID: 51250
				public static LocString ARTIFACT = "Records from the Genetics and Biology Lab of the Gravitas facility show that several early iterations of a radioactive Bee would continue to produce honey and that this honey was once accidentally stored in the employee kitchen which resulted in several incidents of minor radiation poisoning when it was erroneously labled as a sweetener for tea.\n\nEmployees who used this product reported that it was the \"sweetest honey they'd ever tasted\" and expressed no regret at the mix-up.";
			}

			// Token: 0x02002E88 RID: 11912
			public class PLASTICFLOWERS
			{
				// Token: 0x0400C833 RID: 51251
				public static LocString NAME = "Plastic Flowers";

				// Token: 0x0400C834 RID: 51252
				public static LocString DESCRIPTION = "Maintenance-free blooms that will outlast us all.";

				// Token: 0x0400C835 RID: 51253
				public static LocString ARTIFACT = "Manufactured and sold by a home staging company hired by Gravitas to \"make Space feel more like home.\"\n\nThis bouquet is designed to smell like freshly baked cookies.";
			}

			// Token: 0x02002E89 RID: 11913
			public class FOUNTAINPEN
			{
				// Token: 0x0400C836 RID: 51254
				public static LocString NAME = "Fountain Pen";

				// Token: 0x0400C837 RID: 51255
				public static LocString DESCRIPTION = "It cuts through red tape better than a sword ever could.";

				// Token: 0x0400C838 RID: 51256
				public static LocString ARTIFACT = "The handcrafted gold nib features a triangular logo with the letters V and I inside.\n\nIts owner was too proud to report it stolen, and would be shocked to learn of its whereabouts.";
			}
		}

		// Token: 0x02002517 RID: 9495
		public class KEEPSAKES
		{
			// Token: 0x02002E8A RID: 11914
			public class CRITTER_MANIPULATOR
			{
				// Token: 0x0400C839 RID: 51257
				public static LocString NAME = "Ceramic Morb";

				// Token: 0x0400C83A RID: 51258
				public static LocString DESCRIPTION = "A pottery project produced in an HR-mandated art therapy class.\n\nIt's glazed with a substance that once landed a curious lab technician in the ER.";
			}

			// Token: 0x02002E8B RID: 11915
			public class MEGA_BRAIN
			{
				// Token: 0x0400C83B RID: 51259
				public static LocString NAME = "Model Plane";

				// Token: 0x0400C83C RID: 51260
				public static LocString DESCRIPTION = "A treasured souvenir that was once a common accompaniment to children's meals during commercial flights. There's a hole in the bottom from when Dr. Holland had it mounted on a stand.";
			}

			// Token: 0x02002E8C RID: 11916
			public class LONELY_MINION
			{
				// Token: 0x0400C83D RID: 51261
				public static LocString NAME = "Rusty Toolbox";

				// Token: 0x0400C83E RID: 51262
				public static LocString DESCRIPTION = "On the inside of the lid, someone used a screwdriver to carve a drawing of a group of smiling Duplicants gathered around a massive crater.";
			}

			// Token: 0x02002E8D RID: 11917
			public class FOSSIL_HUNT
			{
				// Token: 0x0400C83F RID: 51263
				public static LocString NAME = "Critter Collar";

				// Token: 0x0400C840 RID: 51264
				public static LocString DESCRIPTION = "The tag reads \"Molly\".\n\nOn the reverse is \"Designed by B363\" stamped above what appears to be an unusually shaped pawprint.";
			}

			// Token: 0x02002E8E RID: 11918
			public class MORB_ROVER_MAKER
			{
				// Token: 0x0400C841 RID: 51265
				public static LocString NAME = "Toy Bot";

				// Token: 0x0400C842 RID: 51266
				public static LocString DESCRIPTION = "A custom-made robot programmed to deliver puns in a variety of celebrity voices.\n\nIt is also a paper shredder.";
			}

			// Token: 0x02002E8F RID: 11919
			public class GEOTHERMAL_PLANT
			{
				// Token: 0x0400C843 RID: 51267
				public static LocString NAME = "Shiny Coprolite";

				// Token: 0x0400C844 RID: 51268
				public static LocString DESCRIPTION = "A spectacular sample of organic material fossilized into lead.\n\nSome things really <i>do</i> get better with age.";
			}

			// Token: 0x02002E90 RID: 11920
			public class VIEWMASTER
			{
				// Token: 0x0400C845 RID: 51269
				public static LocString NAME = "Stereoscope";

				// Token: 0x0400C846 RID: 51270
				public static LocString DESCRIPTION = "A tool used to gaze into frozen moments of time.\n\nOne of the images is of a child standing in a field, waving a huge piece of blackened titanium.";
			}

			// Token: 0x02002E91 RID: 11921
			public class HIJACK_HEADQUARTERS
			{
				// Token: 0x0400C847 RID: 51271
				public static LocString NAME = "Hair Dryer";

				// Token: 0x0400C848 RID: 51272
				public static LocString DESCRIPTION = "A vintage follicle styling tool.\n\nThe handle bears a small engraving that reads: \"To N.T., from N.T.\"";
			}
		}

		// Token: 0x02002518 RID: 9496
		public class SANDBOXTOOLS
		{
			// Token: 0x02002E92 RID: 11922
			public class SETTINGS
			{
				// Token: 0x02003BA1 RID: 15265
				public class INSTANT_BUILD
				{
					// Token: 0x0400EE16 RID: 60950
					public static LocString NAME = "Instant build mode ON";

					// Token: 0x0400EE17 RID: 60951
					public static LocString TOOLTIP = "Toggle between placing construction plans and fully built buildings";
				}

				// Token: 0x02003BA2 RID: 15266
				public class BRUSH_SIZE
				{
					// Token: 0x0400EE18 RID: 60952
					public static LocString NAME = "Size";

					// Token: 0x0400EE19 RID: 60953
					public static LocString TOOLTIP = "Adjust brush size";
				}

				// Token: 0x02003BA3 RID: 15267
				public class BRUSH_NOISE_SCALE
				{
					// Token: 0x0400EE1A RID: 60954
					public static LocString NAME = "Noise A";

					// Token: 0x0400EE1B RID: 60955
					public static LocString TOOLTIP = "Adjust brush noisiness A";
				}

				// Token: 0x02003BA4 RID: 15268
				public class BRUSH_NOISE_DENSITY
				{
					// Token: 0x0400EE1C RID: 60956
					public static LocString NAME = "Noise B";

					// Token: 0x0400EE1D RID: 60957
					public static LocString TOOLTIP = "Adjust brush noisiness B";
				}

				// Token: 0x02003BA5 RID: 15269
				public class TEMPERATURE
				{
					// Token: 0x0400EE1E RID: 60958
					public static LocString NAME = "Temperature";

					// Token: 0x0400EE1F RID: 60959
					public static LocString TOOLTIP = "Adjust absolute temperature";
				}

				// Token: 0x02003BA6 RID: 15270
				public class TEMPERATURE_ADDITIVE
				{
					// Token: 0x0400EE20 RID: 60960
					public static LocString NAME = "Temperature";

					// Token: 0x0400EE21 RID: 60961
					public static LocString TOOLTIP = "Adjust additive temperature";
				}

				// Token: 0x02003BA7 RID: 15271
				public class RADIATION
				{
					// Token: 0x0400EE22 RID: 60962
					public static LocString NAME = "Absolute radiation";

					// Token: 0x0400EE23 RID: 60963
					public static LocString TOOLTIP = "Adjust absolute radiation";
				}

				// Token: 0x02003BA8 RID: 15272
				public class RADIATION_ADDITIVE
				{
					// Token: 0x0400EE24 RID: 60964
					public static LocString NAME = "Additive radiation";

					// Token: 0x0400EE25 RID: 60965
					public static LocString TOOLTIP = "Adjust additive radiation";
				}

				// Token: 0x02003BA9 RID: 15273
				public class STRESS_ADDITIVE
				{
					// Token: 0x0400EE26 RID: 60966
					public static LocString NAME = "Reduce Stress";

					// Token: 0x0400EE27 RID: 60967
					public static LocString TOOLTIP = "Adjust stress reduction";
				}

				// Token: 0x02003BAA RID: 15274
				public class MORALE
				{
					// Token: 0x0400EE28 RID: 60968
					public static LocString NAME = "Adjust Morale";

					// Token: 0x0400EE29 RID: 60969
					public static LocString TOOLTIP = "Bonus Morale adjustment";
				}

				// Token: 0x02003BAB RID: 15275
				public class MASS
				{
					// Token: 0x0400EE2A RID: 60970
					public static LocString NAME = "Mass";

					// Token: 0x0400EE2B RID: 60971
					public static LocString TOOLTIP = "Adjust mass";
				}

				// Token: 0x02003BAC RID: 15276
				public class DISEASE
				{
					// Token: 0x0400EE2C RID: 60972
					public static LocString NAME = "Germ";

					// Token: 0x0400EE2D RID: 60973
					public static LocString TOOLTIP = "Adjust type of germ";
				}

				// Token: 0x02003BAD RID: 15277
				public class DISEASE_COUNT
				{
					// Token: 0x0400EE2E RID: 60974
					public static LocString NAME = "Germs";

					// Token: 0x0400EE2F RID: 60975
					public static LocString TOOLTIP = "Adjust germ count";
				}

				// Token: 0x02003BAE RID: 15278
				public class BRUSH
				{
					// Token: 0x0400EE30 RID: 60976
					public static LocString NAME = "Brush";

					// Token: 0x0400EE31 RID: 60977
					public static LocString TOOLTIP = "Paint elements into the world simulation {Hotkey}";
				}

				// Token: 0x02003BAF RID: 15279
				public class ELEMENT
				{
					// Token: 0x0400EE32 RID: 60978
					public static LocString NAME = "Element";

					// Token: 0x0400EE33 RID: 60979
					public static LocString TOOLTIP = "Adjust type of element";
				}

				// Token: 0x02003BB0 RID: 15280
				public class SPRINKLE
				{
					// Token: 0x0400EE34 RID: 60980
					public static LocString NAME = "Sprinkle";

					// Token: 0x0400EE35 RID: 60981
					public static LocString TOOLTIP = "Paint elements into the simulation using noise {Hotkey}";
				}

				// Token: 0x02003BB1 RID: 15281
				public class FLOOD
				{
					// Token: 0x0400EE36 RID: 60982
					public static LocString NAME = "Fill";

					// Token: 0x0400EE37 RID: 60983
					public static LocString TOOLTIP = "Fill a section of the simulation with the chosen element {Hotkey}";
				}

				// Token: 0x02003BB2 RID: 15282
				public class SAMPLE
				{
					// Token: 0x0400EE38 RID: 60984
					public static LocString NAME = "Sample";

					// Token: 0x0400EE39 RID: 60985
					public static LocString TOOLTIP = "Copy the settings from a cell to use with brush tools {Hotkey}";
				}

				// Token: 0x02003BB3 RID: 15283
				public class HEATGUN
				{
					// Token: 0x0400EE3A RID: 60986
					public static LocString NAME = "Heat Gun";

					// Token: 0x0400EE3B RID: 60987
					public static LocString TOOLTIP = "Inject thermal energy into the simulation {Hotkey}";
				}

				// Token: 0x02003BB4 RID: 15284
				public class RADSTOOL
				{
					// Token: 0x0400EE3C RID: 60988
					public static LocString NAME = "Radiation Tool";

					// Token: 0x0400EE3D RID: 60989
					public static LocString TOOLTIP = "Inject or remove radiation from the simulation {Hotkey}";
				}

				// Token: 0x02003BB5 RID: 15285
				public class SPAWNER
				{
					// Token: 0x0400EE3E RID: 60990
					public static LocString NAME = "Spawner";

					// Token: 0x0400EE3F RID: 60991
					public static LocString TOOLTIP = "Spawn critters, food, equipment, and other entities {Hotkey}";
				}

				// Token: 0x02003BB6 RID: 15286
				public class STRESS
				{
					// Token: 0x0400EE40 RID: 60992
					public static LocString NAME = "Stress";

					// Token: 0x0400EE41 RID: 60993
					public static LocString TOOLTIP = "Manage Duplicants' stress levels {Hotkey}";
				}

				// Token: 0x02003BB7 RID: 15287
				public class CLEAR_FLOOR
				{
					// Token: 0x0400EE42 RID: 60994
					public static LocString NAME = "Clear Debris";

					// Token: 0x0400EE43 RID: 60995
					public static LocString TOOLTIP = "Delete loose items cluttering the floor {Hotkey}";
				}

				// Token: 0x02003BB8 RID: 15288
				public class DESTROY
				{
					// Token: 0x0400EE44 RID: 60996
					public static LocString NAME = "Destroy";

					// Token: 0x0400EE45 RID: 60997
					public static LocString TOOLTIP = "Delete everything in the selected cell(s) {Hotkey}";
				}

				// Token: 0x02003BB9 RID: 15289
				public class SPAWN_ENTITY
				{
					// Token: 0x0400EE46 RID: 60998
					public static LocString NAME = "Spawn";
				}

				// Token: 0x02003BBA RID: 15290
				public class FOW
				{
					// Token: 0x0400EE47 RID: 60999
					public static LocString NAME = "Reveal";

					// Token: 0x0400EE48 RID: 61000
					public static LocString TOOLTIP = "Dispel the Fog of War shrouding the map {Hotkey}";
				}

				// Token: 0x02003BBB RID: 15291
				public class CRITTER
				{
					// Token: 0x0400EE49 RID: 61001
					public static LocString NAME = "Critter Removal";

					// Token: 0x0400EE4A RID: 61002
					public static LocString TOOLTIP = "Remove critters! {Hotkey}";
				}

				// Token: 0x02003BBC RID: 15292
				public class SPAWN_STORY_TRAIT
				{
					// Token: 0x0400EE4B RID: 61003
					public static LocString NAME = "Story Traits";

					// Token: 0x0400EE4C RID: 61004
					public static LocString TOOLTIP = "Spawn story traits {Hotkey}";
				}
			}

			// Token: 0x02002E93 RID: 11923
			public class FILTERS
			{
				// Token: 0x0400C849 RID: 51273
				public static LocString BACK = "Back";

				// Token: 0x0400C84A RID: 51274
				public static LocString COMMON = "Common Substances";

				// Token: 0x0400C84B RID: 51275
				public static LocString SOLID = "Solids";

				// Token: 0x0400C84C RID: 51276
				public static LocString LIQUID = "Liquids";

				// Token: 0x0400C84D RID: 51277
				public static LocString GAS = "Gases";

				// Token: 0x02003BBD RID: 15293
				public class ENTITIES
				{
					// Token: 0x0400EE4D RID: 61005
					public static LocString BIONICUPGRADES = "Boosters";

					// Token: 0x0400EE4E RID: 61006
					public static LocString SPECIAL = "Special";

					// Token: 0x0400EE4F RID: 61007
					public static LocString GRAVITAS = "Gravitas";

					// Token: 0x0400EE50 RID: 61008
					public static LocString PLANTS = "Plants";

					// Token: 0x0400EE51 RID: 61009
					public static LocString SEEDS = "Seeds";

					// Token: 0x0400EE52 RID: 61010
					public static LocString CREATURE = "Critters";

					// Token: 0x0400EE53 RID: 61011
					public static LocString CREATURE_EGG = "Eggs";

					// Token: 0x0400EE54 RID: 61012
					public static LocString FOOD = "Foods";

					// Token: 0x0400EE55 RID: 61013
					public static LocString EQUIPMENT = "Equipment";

					// Token: 0x0400EE56 RID: 61014
					public static LocString GEYSERS = "Geysers";

					// Token: 0x0400EE57 RID: 61015
					public static LocString EXPERIMENTS = "Experimental";

					// Token: 0x0400EE58 RID: 61016
					public static LocString INDUSTRIAL_PRODUCTS = "Industrial";

					// Token: 0x0400EE59 RID: 61017
					public static LocString COMETS = "Meteors";

					// Token: 0x0400EE5A RID: 61018
					public static LocString ARTIFACTS = "Artifacts";

					// Token: 0x0400EE5B RID: 61019
					public static LocString STORYTRAITS = "Story Traits";

					// Token: 0x0400EE5C RID: 61020
					public static LocString ORE_CHUNKS = "Solid Materials";

					// Token: 0x0400EE5D RID: 61021
					public static LocString BOTTLES = "Bottled Liquids";

					// Token: 0x0400EE5E RID: 61022
					public static LocString CANISTERS = "Gas Canisters";
				}
			}

			// Token: 0x02002E94 RID: 11924
			public class CLEARFLOOR
			{
				// Token: 0x0400C84E RID: 51278
				public static LocString DELETED = "Deleted";
			}
		}

		// Token: 0x02002519 RID: 9497
		public class RETIRED_COLONY_INFO_SCREEN
		{
			// Token: 0x0400A4DB RID: 42203
			public static LocString SECONDS = "Seconds";

			// Token: 0x0400A4DC RID: 42204
			public static LocString CYCLES = "Cycles";

			// Token: 0x0400A4DD RID: 42205
			public static LocString CYCLE_COUNT = "Cycle Count: {0}";

			// Token: 0x0400A4DE RID: 42206
			public static LocString DUPLICANT_AGE = "Age: {0} cycles";

			// Token: 0x0400A4DF RID: 42207
			public static LocString SKILL_LEVEL = "Skill Level: {0}";

			// Token: 0x0400A4E0 RID: 42208
			public static LocString BUILDING_COUNT = "Count: {0}";

			// Token: 0x0400A4E1 RID: 42209
			public static LocString PREVIEW_UNAVAILABLE = "Preview\nUnavailable";

			// Token: 0x0400A4E2 RID: 42210
			public static LocString TIMELAPSE_UNAVAILABLE = "Timelapse\nUnavailable";

			// Token: 0x0400A4E3 RID: 42211
			public static LocString SEARCH = "SEARCH...";

			// Token: 0x02002E95 RID: 11925
			public class BUTTONS
			{
				// Token: 0x0400C84F RID: 51279
				public static LocString RETURN_TO_GAME = "RETURN TO GAME";

				// Token: 0x0400C850 RID: 51280
				public static LocString VIEW_OTHER_COLONIES = "BACK";

				// Token: 0x0400C851 RID: 51281
				public static LocString QUIT_TO_MENU = "QUIT TO MAIN MENU";

				// Token: 0x0400C852 RID: 51282
				public static LocString CLOSE = "CLOSE";
			}

			// Token: 0x02002E96 RID: 11926
			public class TITLES
			{
				// Token: 0x0400C853 RID: 51283
				public static LocString EXPLORER_HEADER = "COLONIES";

				// Token: 0x0400C854 RID: 51284
				public static LocString RETIRED_COLONIES = "Colony Summaries";

				// Token: 0x0400C855 RID: 51285
				public static LocString COLONY_STATISTICS = "Colony Statistics";

				// Token: 0x0400C856 RID: 51286
				public static LocString DUPLICANTS = "Duplicants";

				// Token: 0x0400C857 RID: 51287
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400C858 RID: 51288
				public static LocString CHEEVOS = "Colony Achievements";

				// Token: 0x0400C859 RID: 51289
				public static LocString ACHIEVEMENT_HEADER = "ACHIEVEMENTS";

				// Token: 0x0400C85A RID: 51290
				public static LocString TIMELAPSE = "Timelapse";
			}

			// Token: 0x02002E97 RID: 11927
			public class STATS
			{
				// Token: 0x0400C85B RID: 51291
				public static LocString OXYGEN_CREATED = "Total Oxygen Produced";

				// Token: 0x0400C85C RID: 51292
				public static LocString OXYGEN_CONSUMED = "Total Oxygen Consumed";

				// Token: 0x0400C85D RID: 51293
				public static LocString POWER_CREATED = "Average Power Produced";

				// Token: 0x0400C85E RID: 51294
				public static LocString POWER_WASTED = "Average Power Wasted";

				// Token: 0x0400C85F RID: 51295
				public static LocString TRAVEL_TIME = "Total Travel Time";

				// Token: 0x0400C860 RID: 51296
				public static LocString WORK_TIME = "Total Work Time";

				// Token: 0x0400C861 RID: 51297
				public static LocString AVERAGE_TRAVEL_TIME = "Average Travel Time";

				// Token: 0x0400C862 RID: 51298
				public static LocString AVERAGE_WORK_TIME = "Average Work Time";

				// Token: 0x0400C863 RID: 51299
				public static LocString CALORIES_CREATED = "Calorie Generation";

				// Token: 0x0400C864 RID: 51300
				public static LocString CALORIES_CONSUMED = "Calorie Consumption";

				// Token: 0x0400C865 RID: 51301
				public static LocString LIVE_DUPLICANTS = "Duplicants";

				// Token: 0x0400C866 RID: 51302
				public static LocString AVERAGE_STRESS_CREATED = "Average Stress Created";

				// Token: 0x0400C867 RID: 51303
				public static LocString AVERAGE_STRESS_REMOVED = "Average Stress Removed";

				// Token: 0x0400C868 RID: 51304
				public static LocString NUMBER_DOMESTICATED_CRITTERS = "Domesticated Critters";

				// Token: 0x0400C869 RID: 51305
				public static LocString NUMBER_WILD_CRITTERS = "Wild Critters";

				// Token: 0x0400C86A RID: 51306
				public static LocString AVERAGE_GERMS = "Average Germs";

				// Token: 0x0400C86B RID: 51307
				public static LocString ROCKET_MISSIONS = "Rocket Missions Underway";
			}
		}

		// Token: 0x0200251A RID: 9498
		public class DROPDOWN
		{
			// Token: 0x0400A4E4 RID: 42212
			public static LocString NONE = "Unassigned";
		}

		// Token: 0x0200251B RID: 9499
		public class FRONTEND
		{
			// Token: 0x0400A4E5 RID: 42213
			public static LocString GAME_VERSION = "Game Version: ";

			// Token: 0x0400A4E6 RID: 42214
			public static LocString LOADING = "Loading...";

			// Token: 0x0400A4E7 RID: 42215
			public static LocString DONE_BUTTON = "DONE";

			// Token: 0x02002E98 RID: 11928
			public class DEMO_OVER_SCREEN
			{
				// Token: 0x0400C86C RID: 51308
				public static LocString TITLE = "Thanks for playing!";

				// Token: 0x0400C86D RID: 51309
				public static LocString BODY = "Thank you for playing the demo for Oxygen Not Included!\n\nThis game is still in development.\n\nGo to kleigames.com/o2 or ask one of us if you'd like more information.";

				// Token: 0x0400C86E RID: 51310
				public static LocString BUTTON_EXIT_TO_MENU = "EXIT TO MENU";
			}

			// Token: 0x02002E99 RID: 11929
			public class CUSTOMGAMESETTINGSSCREEN
			{
				// Token: 0x02003BBE RID: 15294
				public class SETTINGS
				{
					// Token: 0x0200408C RID: 16524
					public class SANDBOXMODE
					{
						// Token: 0x0400F9E5 RID: 63973
						public static LocString NAME = "Sandbox Mode";

						// Token: 0x0400F9E6 RID: 63974
						public static LocString TOOLTIP = "Manipulate and customize the simulation with tools that ignore regular game constraints";

						// Token: 0x02004113 RID: 16659
						public static class LEVELS
						{
							// Token: 0x0200412E RID: 16686
							public static class DISABLED
							{
								// Token: 0x0400FB3D RID: 64317
								public static LocString NAME = "Disabled";

								// Token: 0x0400FB3E RID: 64318
								public static LocString TOOLTIP = "Unchecked: Sandbox Mode is turned off (Default)";
							}

							// Token: 0x0200412F RID: 16687
							public static class ENABLED
							{
								// Token: 0x0400FB3F RID: 64319
								public static LocString NAME = "Enabled";

								// Token: 0x0400FB40 RID: 64320
								public static LocString TOOLTIP = "Checked: Sandbox Mode is turned on";
							}
						}
					}

					// Token: 0x0200408D RID: 16525
					public class FASTWORKERSMODE
					{
						// Token: 0x0400F9E7 RID: 63975
						public static LocString NAME = "Fast Workers Mode";

						// Token: 0x0400F9E8 RID: 63976
						public static LocString TOOLTIP = "Duplicants will finish most work immediately and require little sleep";

						// Token: 0x02004114 RID: 16660
						public static class LEVELS
						{
							// Token: 0x02004130 RID: 16688
							public static class DISABLED
							{
								// Token: 0x0400FB41 RID: 64321
								public static LocString NAME = "Disabled";

								// Token: 0x0400FB42 RID: 64322
								public static LocString TOOLTIP = "Unchecked: Fast Workers Mode is turned off (Default)";
							}

							// Token: 0x02004131 RID: 16689
							public static class ENABLED
							{
								// Token: 0x0400FB43 RID: 64323
								public static LocString NAME = "Enabled";

								// Token: 0x0400FB44 RID: 64324
								public static LocString TOOLTIP = "Checked: Fast Workers Mode is turned on";
							}
						}
					}

					// Token: 0x0200408E RID: 16526
					public class EXPANSION1ACTIVE
					{
						// Token: 0x0400F9E9 RID: 63977
						public static LocString NAME = UI.DLC1.NAME_ITAL + " Content Enabled";

						// Token: 0x0400F9EA RID: 63978
						public static LocString TOOLTIP = "If checked, content from the " + UI.DLC1.NAME_ITAL + " Expansion will be available";

						// Token: 0x02004115 RID: 16661
						public static class LEVELS
						{
							// Token: 0x02004132 RID: 16690
							public static class DISABLED
							{
								// Token: 0x0400FB45 RID: 64325
								public static LocString NAME = "Disabled";

								// Token: 0x0400FB46 RID: 64326
								public static LocString TOOLTIP = "Unchecked: " + UI.DLC1.NAME_ITAL + " Content is turned off (Default)";
							}

							// Token: 0x02004133 RID: 16691
							public static class ENABLED
							{
								// Token: 0x0400FB47 RID: 64327
								public static LocString NAME = "Enabled";

								// Token: 0x0400FB48 RID: 64328
								public static LocString TOOLTIP = "Checked: " + UI.DLC1.NAME_ITAL + " Content is turned on";
							}
						}
					}

					// Token: 0x0200408F RID: 16527
					public class SAVETOCLOUD
					{
						// Token: 0x0400F9EB RID: 63979
						public static LocString NAME = "Save To Cloud";

						// Token: 0x0400F9EC RID: 63980
						public static LocString TOOLTIP = "This colony will be created in the cloud saves folder, and synced by the game platform.";

						// Token: 0x0400F9ED RID: 63981
						public static LocString TOOLTIP_LOCAL = "This colony will be created in the local saves folder. It will not be a cloud save and will not be synced by the game platform.";

						// Token: 0x0400F9EE RID: 63982
						public static LocString TOOLTIP_EXTRA = "This can be changed later with the colony management options in the load screen, from the main menu.";

						// Token: 0x02004116 RID: 16662
						public static class LEVELS
						{
							// Token: 0x02004134 RID: 16692
							public static class DISABLED
							{
								// Token: 0x0400FB49 RID: 64329
								public static LocString NAME = "Disabled";

								// Token: 0x0400FB4A RID: 64330
								public static LocString TOOLTIP = "Unchecked: This colony will be a local save";
							}

							// Token: 0x02004135 RID: 16693
							public static class ENABLED
							{
								// Token: 0x0400FB4B RID: 64331
								public static LocString NAME = "Enabled";

								// Token: 0x0400FB4C RID: 64332
								public static LocString TOOLTIP = "Checked: This colony will be a cloud save (Default)";
							}
						}
					}

					// Token: 0x02004090 RID: 16528
					public class CAREPACKAGES
					{
						// Token: 0x0400F9EF RID: 63983
						public static LocString NAME = "Care Packages";

						// Token: 0x0400F9F0 RID: 63984
						public static LocString TOOLTIP = "Affects what resources can be printed from the Printing Pod";

						// Token: 0x02004117 RID: 16663
						public static class LEVELS
						{
							// Token: 0x02004136 RID: 16694
							public static class NORMAL
							{
								// Token: 0x0400FB4D RID: 64333
								public static LocString NAME = "All";

								// Token: 0x0400FB4E RID: 64334
								public static LocString TOOLTIP = "Checked: The Printing Pod will offer both Duplicant blueprints and care packages (Default)";
							}

							// Token: 0x02004137 RID: 16695
							public static class DUPLICANTS_ONLY
							{
								// Token: 0x0400FB4F RID: 64335
								public static LocString NAME = "Duplicants Only";

								// Token: 0x0400FB50 RID: 64336
								public static LocString TOOLTIP = "Unchecked: The Printing Pod will only offer Duplicant blueprints";
							}
						}
					}

					// Token: 0x02004091 RID: 16529
					public class IMMUNESYSTEM
					{
						// Token: 0x0400F9F1 RID: 63985
						public static LocString NAME = "Disease";

						// Token: 0x0400F9F2 RID: 63986
						public static LocString TOOLTIP = "Affects Duplicants' chances of contracting a disease after germ exposure";

						// Token: 0x02004118 RID: 16664
						public static class LEVELS
						{
							// Token: 0x02004138 RID: 16696
							public static class COMPROMISED
							{
								// Token: 0x0400FB51 RID: 64337
								public static LocString NAME = "Outbreak Prone";

								// Token: 0x0400FB52 RID: 64338
								public static LocString TOOLTIP = "The whole colony will be ravaged by plague if a Duplicant so much as sneezes funny";

								// Token: 0x0400FB53 RID: 64339
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Outbreak Prone (Highest Difficulty)";
							}

							// Token: 0x02004139 RID: 16697
							public static class WEAK
							{
								// Token: 0x0400FB54 RID: 64340
								public static LocString NAME = "Germ Susceptible";

								// Token: 0x0400FB55 RID: 64341
								public static LocString TOOLTIP = "These Duplicants have an increased chance of contracting diseases from germ exposure";

								// Token: 0x0400FB56 RID: 64342
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Germ Susceptibility (Difficulty Up)";
							}

							// Token: 0x0200413A RID: 16698
							public static class DEFAULT
							{
								// Token: 0x0400FB57 RID: 64343
								public static LocString NAME = "Default";

								// Token: 0x0400FB58 RID: 64344
								public static LocString TOOLTIP = "Default disease chance";
							}

							// Token: 0x0200413B RID: 16699
							public static class STRONG
							{
								// Token: 0x0400FB59 RID: 64345
								public static LocString NAME = "Germ Resistant";

								// Token: 0x0400FB5A RID: 64346
								public static LocString TOOLTIP = "These Duplicants have a decreased chance of contracting diseases from germ exposure";

								// Token: 0x0400FB5B RID: 64347
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Germ Resistance (Difficulty Down)";
							}

							// Token: 0x0200413C RID: 16700
							public static class INVINCIBLE
							{
								// Token: 0x0400FB5C RID: 64348
								public static LocString NAME = "Total Immunity";

								// Token: 0x0400FB5D RID: 64349
								public static LocString TOOLTIP = "Like diplomatic immunity, but without the diplomacy. These Duplicants will never get sick";

								// Token: 0x0400FB5E RID: 64350
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Total Immunity (No Disease)";
							}
						}
					}

					// Token: 0x02004092 RID: 16530
					public class MORALE
					{
						// Token: 0x0400F9F3 RID: 63987
						public static LocString NAME = "Morale";

						// Token: 0x0400F9F4 RID: 63988
						public static LocString TOOLTIP = "Adjusts the minimum morale Duplicants must maintain to avoid gaining stress";

						// Token: 0x02004119 RID: 16665
						public static class LEVELS
						{
							// Token: 0x0200413D RID: 16701
							public static class VERYHARD
							{
								// Token: 0x0400FB5F RID: 64351
								public static LocString NAME = "Draconian";

								// Token: 0x0400FB60 RID: 64352
								public static LocString TOOLTIP = "The finest of the finest can barely keep up with these Duplicants' stringent demands";

								// Token: 0x0400FB61 RID: 64353
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Draconian (Highest Difficulty)";
							}

							// Token: 0x0200413E RID: 16702
							public static class HARD
							{
								// Token: 0x0400FB62 RID: 64354
								public static LocString NAME = "A Bit Persnickety";

								// Token: 0x0400FB63 RID: 64355
								public static LocString TOOLTIP = "Duplicants require higher morale than usual to fend off stress";

								// Token: 0x0400FB64 RID: 64356
								public static LocString ATTRIBUTE_MODIFIER_NAME = "A Bit Persnickety (Difficulty Up)";
							}

							// Token: 0x0200413F RID: 16703
							public static class DEFAULT
							{
								// Token: 0x0400FB65 RID: 64357
								public static LocString NAME = "Default";

								// Token: 0x0400FB66 RID: 64358
								public static LocString TOOLTIP = "Default morale needs";
							}

							// Token: 0x02004140 RID: 16704
							public static class EASY
							{
								// Token: 0x0400FB67 RID: 64359
								public static LocString NAME = "Chill";

								// Token: 0x0400FB68 RID: 64360
								public static LocString TOOLTIP = "Duplicants require lower morale than usual to fend off stress";

								// Token: 0x0400FB69 RID: 64361
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Chill (Difficulty Down)";
							}

							// Token: 0x02004141 RID: 16705
							public static class DISABLED
							{
								// Token: 0x0400FB6A RID: 64362
								public static LocString NAME = "Totally Blasé";

								// Token: 0x0400FB6B RID: 64363
								public static LocString TOOLTIP = "These Duplicants have zero standards and will never gain stress, regardless of their morale";

								// Token: 0x0400FB6C RID: 64364
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Totally Blasé (No Morale)";
							}
						}
					}

					// Token: 0x02004093 RID: 16531
					public class CALORIE_BURN
					{
						// Token: 0x0400F9F5 RID: 63989
						public static LocString NAME = "Hunger";

						// Token: 0x0400F9F6 RID: 63990
						public static LocString TOOLTIP = "Affects how quickly Duplicants burn calories and become hungry";

						// Token: 0x0200411A RID: 16666
						public static class LEVELS
						{
							// Token: 0x02004142 RID: 16706
							public static class VERYHARD
							{
								// Token: 0x0400FB6D RID: 64365
								public static LocString NAME = "Ravenous";

								// Token: 0x0400FB6E RID: 64366
								public static LocString TOOLTIP = "Your Duplicants are on a see-food diet... They see food and they eat it";

								// Token: 0x0400FB6F RID: 64367
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Ravenous (Highest Difficulty)";
							}

							// Token: 0x02004143 RID: 16707
							public static class HARD
							{
								// Token: 0x0400FB70 RID: 64368
								public static LocString NAME = "Rumbly Tummies";

								// Token: 0x0400FB71 RID: 64369
								public static LocString TOOLTIP = "Duplicants burn calories quickly and require more feeding than usual";

								// Token: 0x0400FB72 RID: 64370
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Rumbly Tummies (Difficulty Up)";
							}

							// Token: 0x02004144 RID: 16708
							public static class DEFAULT
							{
								// Token: 0x0400FB73 RID: 64371
								public static LocString NAME = "Default";

								// Token: 0x0400FB74 RID: 64372
								public static LocString TOOLTIP = "Default calorie burn rate";
							}

							// Token: 0x02004145 RID: 16709
							public static class EASY
							{
								// Token: 0x0400FB75 RID: 64373
								public static LocString NAME = "Fasting";

								// Token: 0x0400FB76 RID: 64374
								public static LocString TOOLTIP = "Duplicants burn calories slowly and get by with fewer meals";

								// Token: 0x0400FB77 RID: 64375
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Fasting (Difficulty Down)";
							}

							// Token: 0x02004146 RID: 16710
							public static class DISABLED
							{
								// Token: 0x0400FB78 RID: 64376
								public static LocString NAME = "Tummyless";

								// Token: 0x0400FB79 RID: 64377
								public static LocString TOOLTIP = "These Duplicants were printed without tummies and need no food at all";

								// Token: 0x0400FB7A RID: 64378
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Tummyless (No Hunger)";
							}
						}
					}

					// Token: 0x02004094 RID: 16532
					public class BIONICPOWERUSE
					{
						// Token: 0x0400F9F7 RID: 63991
						public static LocString NAME = "Bionic Wattage";

						// Token: 0x0400F9F8 RID: 63992
						public static LocString TOOLTIP = "Adjusts Bionic Duplicants' baseline power consumption";

						// Token: 0x0200411B RID: 16667
						public static class LEVELS
						{
							// Token: 0x02004147 RID: 16711
							public static class VERYHARD
							{
								// Token: 0x0400FB7B RID: 64379
								public static LocString NAME = "Energy Vampire";

								// Token: 0x0400FB7C RID: 64380
								public static LocString TOOLTIP = "These Bionic Duplicants drain batteries like it's their only job";
							}

							// Token: 0x02004148 RID: 16712
							public static class HARD
							{
								// Token: 0x0400FB7D RID: 64381
								public static LocString NAME = "Power Hungry";

								// Token: 0x0400FB7E RID: 64382
								public static LocString TOOLTIP = "These Duplicants have an increased appetite for power";
							}

							// Token: 0x02004149 RID: 16713
							public static class DEFAULT
							{
								// Token: 0x0400FB7F RID: 64383
								public static LocString NAME = "Default";

								// Token: 0x0400FB80 RID: 64384
								public static LocString TOOLTIP = "Default wattage";
							}

							// Token: 0x0200414A RID: 16714
							public static class EASY
							{
								// Token: 0x0400FB81 RID: 64385
								public static LocString NAME = "Energy Efficient";

								// Token: 0x0400FB82 RID: 64386
								public static LocString TOOLTIP = "These Duplicants consume less power than usual";
							}

							// Token: 0x0200414B RID: 16715
							public static class VERYEASY
							{
								// Token: 0x0400FB83 RID: 64387
								public static LocString NAME = "Analog";

								// Token: 0x0400FB84 RID: 64388
								public static LocString TOOLTIP = "These Bionic Duplicants run on old-school enthusiasm, and barely consume power at all";
							}
						}
					}

					// Token: 0x02004095 RID: 16533
					public class DEMOLIORDIFFICULTY
					{
						// Token: 0x0400F9F9 RID: 63993
						public static LocString NAME = "Demolior Impact";

						// Token: 0x0400F9FA RID: 63994
						public static LocString TOOLTIP = "Adjusts how soon the Demolior asteroid collides with <i>The Prehistoric Planet Pack</i> asteroid";

						// Token: 0x0200411C RID: 16668
						public static class LEVELS
						{
							// Token: 0x0200414C RID: 16716
							public static class VERYHARD
							{
								// Token: 0x0400FB85 RID: 64389
								public static LocString NAME = "Imminent Extinction";

								// Token: 0x0400FB86 RID: 64390
								public static LocString TOOLTIP = "It'll all be over soon\n\nOnly " + 100f.ToString() + " cycles until collision";
							}

							// Token: 0x0200414D RID: 16717
							public static class HARD
							{
								// Token: 0x0400FB87 RID: 64391
								public static LocString NAME = "Early Arrival";

								// Token: 0x0400FB88 RID: 64392
								public static LocString TOOLTIP = "Demolior impacts sooner than usual\n\n" + 150f.ToString() + " cycles until collision";
							}

							// Token: 0x0200414E RID: 16718
							public static class DEFAULT
							{
								// Token: 0x0400FB89 RID: 64393
								public static LocString NAME = "Default";

								// Token: 0x0400FB8A RID: 64394
								public static LocString TOOLTIP = "Demolior impacts in " + 200f.ToString() + " cycles";
							}

							// Token: 0x0200414F RID: 16719
							public static class EASY
							{
								// Token: 0x0400FB8B RID: 64395
								public static LocString NAME = "Slightly Delayed";

								// Token: 0x0400FB8C RID: 64396
								public static LocString TOOLTIP = "Demolior impacts later than usual\n\n" + 300f.ToString() + " cycles until collision";
							}

							// Token: 0x02004150 RID: 16720
							public static class VERYEASY
							{
								// Token: 0x0400FB8D RID: 64397
								public static LocString NAME = "Far-Off Forecast";

								// Token: 0x0400FB8E RID: 64398
								public static LocString TOOLTIP = "Duplicants could probably build a whole new asteroid by the time Demolior impacts this one\n\n500 cycles until collision";
							}

							// Token: 0x02004151 RID: 16721
							public static class OFF
							{
								// Token: 0x0400FB8F RID: 64399
								public static LocString NAME = "Disabled";

								// Token: 0x0400FB90 RID: 64400
								public static LocString TOOLTIP = "Demolior does not exist in this universe and the achievement cannot be earned";
							}
						}
					}

					// Token: 0x02004096 RID: 16534
					public class WORLD_CHOICE
					{
						// Token: 0x0400F9FB RID: 63995
						public static LocString NAME = "World";

						// Token: 0x0400F9FC RID: 63996
						public static LocString TOOLTIP = "New worlds added by mods can be selected here";
					}

					// Token: 0x02004097 RID: 16535
					public class CLUSTER_CHOICE
					{
						// Token: 0x0400F9FD RID: 63997
						public static LocString NAME = "Asteroid Belt";

						// Token: 0x0400F9FE RID: 63998
						public static LocString TOOLTIP = "New asteroid belts added by mods can be selected here";
					}

					// Token: 0x02004098 RID: 16536
					public class STORY_TRAIT_COUNT
					{
						// Token: 0x0400F9FF RID: 63999
						public static LocString NAME = "Story Traits";

						// Token: 0x0400FA00 RID: 64000
						public static LocString TOOLTIP = "Determines the number of story traits spawned";

						// Token: 0x0200411D RID: 16669
						public static class LEVELS
						{
							// Token: 0x02004152 RID: 16722
							public static class NONE
							{
								// Token: 0x0400FB91 RID: 64401
								public static LocString NAME = "Zilch";

								// Token: 0x0400FB92 RID: 64402
								public static LocString TOOLTIP = "Zero story traits. Zip. Nada. None";
							}

							// Token: 0x02004153 RID: 16723
							public static class FEW
							{
								// Token: 0x0400FB93 RID: 64403
								public static LocString NAME = "Stingy";

								// Token: 0x0400FB94 RID: 64404
								public static LocString TOOLTIP = "Not zero, but not a lot";
							}

							// Token: 0x02004154 RID: 16724
							public static class LOTS
							{
								// Token: 0x0400FB95 RID: 64405
								public static LocString NAME = "Oodles";

								// Token: 0x0400FB96 RID: 64406
								public static LocString TOOLTIP = "Plenty of story traits to go around";
							}
						}
					}

					// Token: 0x02004099 RID: 16537
					public class DURABILITY
					{
						// Token: 0x0400FA01 RID: 64001
						public static LocString NAME = "Durability";

						// Token: 0x0400FA02 RID: 64002
						public static LocString TOOLTIP = "Affects how quickly equippable suits wear out";

						// Token: 0x0200411E RID: 16670
						public static class LEVELS
						{
							// Token: 0x02004155 RID: 16725
							public static class INDESTRUCTIBLE
							{
								// Token: 0x0400FB97 RID: 64407
								public static LocString NAME = "Indestructible";

								// Token: 0x0400FB98 RID: 64408
								public static LocString TOOLTIP = "Duplicants have perfected clothes manufacturing and are able to make suits that last forever";

								// Token: 0x0400FB99 RID: 64409
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Indestructible Suits (No Durability)";
							}

							// Token: 0x02004156 RID: 16726
							public static class REINFORCED
							{
								// Token: 0x0400FB9A RID: 64410
								public static LocString NAME = "Reinforced";

								// Token: 0x0400FB9B RID: 64411
								public static LocString TOOLTIP = "Suits are more durable than usual";

								// Token: 0x0400FB9C RID: 64412
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Reinforced Suits (Difficulty Down)";
							}

							// Token: 0x02004157 RID: 16727
							public static class DEFAULT
							{
								// Token: 0x0400FB9D RID: 64413
								public static LocString NAME = "Default";

								// Token: 0x0400FB9E RID: 64414
								public static LocString TOOLTIP = "Default suit durability";
							}

							// Token: 0x02004158 RID: 16728
							public static class FLIMSY
							{
								// Token: 0x0400FB9F RID: 64415
								public static LocString NAME = "Flimsy";

								// Token: 0x0400FBA0 RID: 64416
								public static LocString TOOLTIP = "Suits wear out faster than usual";

								// Token: 0x0400FBA1 RID: 64417
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Flimsy Suits (Difficulty Up)";
							}

							// Token: 0x02004159 RID: 16729
							public static class THREADBARE
							{
								// Token: 0x0400FBA2 RID: 64418
								public static LocString NAME = "Threadbare";

								// Token: 0x0400FBA3 RID: 64419
								public static LocString TOOLTIP = "These Duplicants are no tailors - suits wear out much faster than usual";

								// Token: 0x0400FBA4 RID: 64420
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Threadbare Suits (Highest Difficulty)";
							}
						}
					}

					// Token: 0x0200409A RID: 16538
					public class RADIATION
					{
						// Token: 0x0400FA03 RID: 64003
						public static LocString NAME = "Radiation";

						// Token: 0x0400FA04 RID: 64004
						public static LocString TOOLTIP = "Affects how susceptible Duplicants are to radiation sickness";

						// Token: 0x0200411F RID: 16671
						public static class LEVELS
						{
							// Token: 0x0200415A RID: 16730
							public static class HARDEST
							{
								// Token: 0x0400FBA5 RID: 64421
								public static LocString NAME = "Critical Mass";

								// Token: 0x0400FBA6 RID: 64422
								public static LocString TOOLTIP = "Duplicants feel ill at the merest mention of radiation...and may never truly recover";

								// Token: 0x0400FBA7 RID: 64423
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Super Radiation (Highest Difficulty)";
							}

							// Token: 0x0200415B RID: 16731
							public static class HARDER
							{
								// Token: 0x0400FBA8 RID: 64424
								public static LocString NAME = "Toxic Positivity";

								// Token: 0x0400FBA9 RID: 64425
								public static LocString TOOLTIP = "Duplicants are more sensitive to radiation exposure than usual";

								// Token: 0x0400FBAA RID: 64426
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Vulnerable (Difficulty Up)";
							}

							// Token: 0x0200415C RID: 16732
							public static class DEFAULT
							{
								// Token: 0x0400FBAB RID: 64427
								public static LocString NAME = "Default";

								// Token: 0x0400FBAC RID: 64428
								public static LocString TOOLTIP = "Default radiation settings";
							}

							// Token: 0x0200415D RID: 16733
							public static class EASIER
							{
								// Token: 0x0400FBAD RID: 64429
								public static LocString NAME = "Healthy Glow";

								// Token: 0x0400FBAE RID: 64430
								public static LocString TOOLTIP = "Duplicants are more resistant to radiation exposure than usual";

								// Token: 0x0400FBAF RID: 64431
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Shielded (Difficulty Down)";
							}

							// Token: 0x0200415E RID: 16734
							public static class EASIEST
							{
								// Token: 0x0400FBB0 RID: 64432
								public static LocString NAME = "Nuke-Proof";

								// Token: 0x0400FBB1 RID: 64433
								public static LocString TOOLTIP = "Duplicants could bathe in radioactive waste and not even notice";

								// Token: 0x0400FBB2 RID: 64434
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Protection (Lowest Difficulty)";
							}
						}
					}

					// Token: 0x0200409B RID: 16539
					public class STRESS
					{
						// Token: 0x0400FA05 RID: 64005
						public static LocString NAME = "Stress";

						// Token: 0x0400FA06 RID: 64006
						public static LocString TOOLTIP = "Affects how quickly Duplicant stress rises";

						// Token: 0x02004120 RID: 16672
						public static class LEVELS
						{
							// Token: 0x0200415F RID: 16735
							public static class INDOMITABLE
							{
								// Token: 0x0400FBB3 RID: 64435
								public static LocString NAME = "Cloud Nine";

								// Token: 0x0400FBB4 RID: 64436
								public static LocString TOOLTIP = "A strong emotional support system makes these Duplicants impervious to all stress";

								// Token: 0x0400FBB5 RID: 64437
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Cloud Nine (No Stress)";
							}

							// Token: 0x02004160 RID: 16736
							public static class OPTIMISTIC
							{
								// Token: 0x0400FBB6 RID: 64438
								public static LocString NAME = "Chipper";

								// Token: 0x0400FBB7 RID: 64439
								public static LocString TOOLTIP = "Duplicants gain stress slower than usual";

								// Token: 0x0400FBB8 RID: 64440
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Chipper (Difficulty Down)";
							}

							// Token: 0x02004161 RID: 16737
							public static class DEFAULT
							{
								// Token: 0x0400FBB9 RID: 64441
								public static LocString NAME = "Default";

								// Token: 0x0400FBBA RID: 64442
								public static LocString TOOLTIP = "Default stress change rate";
							}

							// Token: 0x02004162 RID: 16738
							public static class PESSIMISTIC
							{
								// Token: 0x0400FBBB RID: 64443
								public static LocString NAME = "Glum";

								// Token: 0x0400FBBC RID: 64444
								public static LocString TOOLTIP = "Duplicants gain stress more quickly than usual";

								// Token: 0x0400FBBD RID: 64445
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Glum (Difficulty Up)";
							}

							// Token: 0x02004163 RID: 16739
							public static class DOOMED
							{
								// Token: 0x0400FBBE RID: 64446
								public static LocString NAME = "Frankly Depressing";

								// Token: 0x0400FBBF RID: 64447
								public static LocString TOOLTIP = "These Duplicants were never taught coping mechanisms... they're devastated by stress as a result";

								// Token: 0x0400FBC0 RID: 64448
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Frankly Depressing (Highest Difficulty)";
							}
						}
					}

					// Token: 0x0200409C RID: 16540
					public class STRESS_BREAKS
					{
						// Token: 0x0400FA07 RID: 64007
						public static LocString NAME = "Stress Reactions";

						// Token: 0x0400FA08 RID: 64008
						public static LocString TOOLTIP = "Determines whether Duplicants wreak havoc on the colony when they reach maximum stress";

						// Token: 0x02004121 RID: 16673
						public static class LEVELS
						{
							// Token: 0x02004164 RID: 16740
							public static class DEFAULT
							{
								// Token: 0x0400FBC1 RID: 64449
								public static LocString NAME = "Enabled";

								// Token: 0x0400FBC2 RID: 64450
								public static LocString TOOLTIP = "Checked: Duplicants will wreak havoc when they reach 100% stress (Default)";
							}

							// Token: 0x02004165 RID: 16741
							public static class DISABLED
							{
								// Token: 0x0400FBC3 RID: 64451
								public static LocString NAME = "Disabled";

								// Token: 0x0400FBC4 RID: 64452
								public static LocString TOOLTIP = "Unchecked: Duplicants will not wreak havoc at maximum stress";
							}
						}
					}

					// Token: 0x0200409D RID: 16541
					public class WORLDGEN_SEED
					{
						// Token: 0x0400FA09 RID: 64009
						public static LocString NAME = "Worldgen Seed";

						// Token: 0x0400FA0A RID: 64010
						public static LocString TOOLTIP = "This number chooses the procedural parameters that create your unique map\n\nWorldgen seeds can be copied and pasted so others can play a replica of your world configuration";

						// Token: 0x0400FA0B RID: 64011
						public static LocString FIXEDSEED = "This is a predetermined seed, and cannot be changed";
					}

					// Token: 0x0200409E RID: 16542
					public class TELEPORTERS
					{
						// Token: 0x0400FA0C RID: 64012
						public static LocString NAME = "Teleporters";

						// Token: 0x0400FA0D RID: 64013
						public static LocString TOOLTIP = "Determines whether teleporters will be spawned during Worldgen";

						// Token: 0x02004122 RID: 16674
						public static class LEVELS
						{
							// Token: 0x02004166 RID: 16742
							public static class ENABLED
							{
								// Token: 0x0400FBC5 RID: 64453
								public static LocString NAME = "Enabled";

								// Token: 0x0400FBC6 RID: 64454
								public static LocString TOOLTIP = "Checked: Teleporters will spawn during Worldgen (Default)";
							}

							// Token: 0x02004167 RID: 16743
							public static class DISABLED
							{
								// Token: 0x0400FBC7 RID: 64455
								public static LocString NAME = "Disabled";

								// Token: 0x0400FBC8 RID: 64456
								public static LocString TOOLTIP = "Unchecked: No Teleporters will spawn during Worldgen";
							}
						}
					}

					// Token: 0x0200409F RID: 16543
					public class METEORSHOWERS
					{
						// Token: 0x0400FA0E RID: 64014
						public static LocString NAME = "Meteor Showers";

						// Token: 0x0400FA0F RID: 64015
						public static LocString TOOLTIP = "Adjusts the intensity of incoming space rocks";

						// Token: 0x02004123 RID: 16675
						public static class LEVELS
						{
							// Token: 0x02004168 RID: 16744
							public static class CLEAR_SKIES
							{
								// Token: 0x0400FBC9 RID: 64457
								public static LocString NAME = "Clear Skies";

								// Token: 0x0400FBCA RID: 64458
								public static LocString TOOLTIP = "No meteor damage, no worries";
							}

							// Token: 0x02004169 RID: 16745
							public static class INFREQUENT
							{
								// Token: 0x0400FBCB RID: 64459
								public static LocString NAME = "Spring Showers";

								// Token: 0x0400FBCC RID: 64460
								public static LocString TOOLTIP = "Meteor showers are less frequent and less intense than usual";
							}

							// Token: 0x0200416A RID: 16746
							public static class DEFAULT
							{
								// Token: 0x0400FBCD RID: 64461
								public static LocString NAME = "Default";

								// Token: 0x0400FBCE RID: 64462
								public static LocString TOOLTIP = "Default meteor shower frequency and intensity";
							}

							// Token: 0x0200416B RID: 16747
							public static class INTENSE
							{
								// Token: 0x0400FBCF RID: 64463
								public static LocString NAME = "Cosmic Storm";

								// Token: 0x0400FBD0 RID: 64464
								public static LocString TOOLTIP = "Meteor showers are more frequent and more intense than usual";
							}

							// Token: 0x0200416C RID: 16748
							public static class DOOMED
							{
								// Token: 0x0400FBD1 RID: 64465
								public static LocString NAME = "Doomsday";

								// Token: 0x0400FBD2 RID: 64466
								public static LocString TOOLTIP = "An onslaught of apocalyptic hailstorms that feels almost personal";
							}
						}
					}

					// Token: 0x020040A0 RID: 16544
					public class DLC_MIXING
					{
						// Token: 0x02004124 RID: 16676
						public static class LEVELS
						{
							// Token: 0x0200416D RID: 16749
							public static class DISABLED
							{
								// Token: 0x0400FBD3 RID: 64467
								public static LocString NAME = "Disabled";

								// Token: 0x0400FBD4 RID: 64468
								public static LocString TOOLTIP = "Content from this DLC is currently <b>disabled</b>";
							}

							// Token: 0x0200416E RID: 16750
							public static class ENABLED
							{
								// Token: 0x0400FBD5 RID: 64469
								public static LocString NAME = "Enabled";

								// Token: 0x0400FBD6 RID: 64470
								public static LocString TOOLTIP = "Content from this DLC is currently <b>enabled</b>\n\nThis includes Care Packages, buildings, and space POIs";
							}
						}
					}

					// Token: 0x020040A1 RID: 16545
					public class SUBWORLD_MIXING
					{
						// Token: 0x02004125 RID: 16677
						public static class LEVELS
						{
							// Token: 0x0200416F RID: 16751
							public static class DISABLED
							{
								// Token: 0x0400FBD7 RID: 64471
								public static LocString NAME = "Disabled";

								// Token: 0x0400FBD8 RID: 64472
								public static LocString TOOLTIP = "This biome will not be mixed into any world";

								// Token: 0x0400FBD9 RID: 64473
								public static LocString TOOLTIP_BASEGAME = "This biome will not be mixed in";
							}

							// Token: 0x02004170 RID: 16752
							public static class TRY_MIXING
							{
								// Token: 0x0400FBDA RID: 64474
								public static LocString NAME = "Likely";

								// Token: 0x0400FBDB RID: 64475
								public static LocString TOOLTIP = "This biome is very likely to be mixed into a world";

								// Token: 0x0400FBDC RID: 64476
								public static LocString TOOLTIP_BASEGAME = "This biome is very likely to be mixed in";
							}

							// Token: 0x02004171 RID: 16753
							public static class GUARANTEE_MIXING
							{
								// Token: 0x0400FBDD RID: 64477
								public static LocString NAME = "Guaranteed";

								// Token: 0x0400FBDE RID: 64478
								public static LocString TOOLTIP = "This biome will be mixed into a world, even if it causes a worldgen failure";

								// Token: 0x0400FBDF RID: 64479
								public static LocString TOOLTIP_BASEGAME = "This biome will be mixed in, even if it causes a worldgen failure";
							}
						}
					}

					// Token: 0x020040A2 RID: 16546
					public class WORLD_MIXING
					{
						// Token: 0x02004126 RID: 16678
						public static class LEVELS
						{
							// Token: 0x02004172 RID: 16754
							public static class DISABLED
							{
								// Token: 0x0400FBE0 RID: 64480
								public static LocString NAME = "Disabled";

								// Token: 0x0400FBE1 RID: 64481
								public static LocString TOOLTIP = "This asteroid will not be mixed in";
							}

							// Token: 0x02004173 RID: 16755
							public static class TRY_MIXING
							{
								// Token: 0x0400FBE2 RID: 64482
								public static LocString NAME = "Likely";

								// Token: 0x0400FBE3 RID: 64483
								public static LocString TOOLTIP = "This asteroid is very likely to be mixed in";
							}

							// Token: 0x02004174 RID: 16756
							public static class GUARANTEE_MIXING
							{
								// Token: 0x0400FBE4 RID: 64484
								public static LocString NAME = "Guaranteed";

								// Token: 0x0400FBE5 RID: 64485
								public static LocString TOOLTIP = "This asteroid will be mixed in, even if it causes worldgen failure";
							}
						}
					}
				}
			}

			// Token: 0x02002E9A RID: 11930
			public class MAINMENU
			{
				// Token: 0x0400C86F RID: 51311
				public static LocString STARTDEMO = "START DEMO";

				// Token: 0x0400C870 RID: 51312
				public static LocString NEWGAME = "NEW GAME";

				// Token: 0x0400C871 RID: 51313
				public static LocString RESUMEGAME = "RESUME GAME";

				// Token: 0x0400C872 RID: 51314
				public static LocString LOADGAME = "LOAD GAME";

				// Token: 0x0400C873 RID: 51315
				public static LocString RETIREDCOLONIES = "COLONY SUMMARIES";

				// Token: 0x0400C874 RID: 51316
				public static LocString KLEIINVENTORY = "KLEI INVENTORY";

				// Token: 0x0400C875 RID: 51317
				public static LocString LOCKERMENU = "SUPPLY CLOSET";

				// Token: 0x0400C876 RID: 51318
				public static LocString SCENARIOS = "SCENARIOS";

				// Token: 0x0400C877 RID: 51319
				public static LocString TRANSLATIONS = "TRANSLATIONS";

				// Token: 0x0400C878 RID: 51320
				public static LocString OPTIONS = "OPTIONS";

				// Token: 0x0400C879 RID: 51321
				public static LocString QUITTODESKTOP = "QUIT";

				// Token: 0x0400C87A RID: 51322
				public static LocString RESTARTCONFIRM = "Should I really quit?\nAll unsaved progress will be lost.";

				// Token: 0x0400C87B RID: 51323
				public static LocString QUITCONFIRM = "Should I quit to the main menu?\nAll unsaved progress will be lost.";

				// Token: 0x0400C87C RID: 51324
				public static LocString RETIRECONFIRM = "Should I surrender under the soul-crushing weight of this universe's entropy and retire my colony?";

				// Token: 0x0400C87D RID: 51325
				public static LocString DESKTOPQUITCONFIRM = "Should I really quit?\nAll unsaved progress will be lost.";

				// Token: 0x0400C87E RID: 51326
				public static LocString RESUMEBUTTON_BASENAME = "{0}: Cycle {1}";

				// Token: 0x0400C87F RID: 51327
				public static LocString QUIT = "QUIT WITHOUT SAVING";

				// Token: 0x0400C880 RID: 51328
				public static LocString SAVEANDQUITTITLE = "SAVE AND QUIT";

				// Token: 0x0400C881 RID: 51329
				public static LocString SAVEANDQUITDESKTOP = "SAVE AND QUIT";

				// Token: 0x0400C882 RID: 51330
				public static LocString WISHLIST_AD = "Available now";

				// Token: 0x0400C883 RID: 51331
				public static LocString WISHLIST_AD_TOOLTIP = "<color=#ffff00ff><b>Click to view it in the store</b></color>";

				// Token: 0x02003BBF RID: 15295
				public class DLC
				{
					// Token: 0x0400EE5F RID: 61023
					public static LocString ACTIVATE_EXPANSION1 = "ENABLE DLC";

					// Token: 0x0400EE60 RID: 61024
					public static LocString ACTIVATE_EXPANSION1_TOOLTIP = "<b>This DLC is disabled</b>\n\n<color=#ffff00ff><b>Click to enable the <i>Spaced Out!</i> DLC</b></color>";

					// Token: 0x0400EE61 RID: 61025
					public static LocString ACTIVATE_EXPANSION1_DESC = "The game will need to restart in order to enable <i>Spaced Out!</i>";

					// Token: 0x0400EE62 RID: 61026
					public static LocString ACTIVATE_EXPANSION1_RAIL_DESC = "<i>Spaced Out!</i> will be enabled the next time you launch the game. The game will now close.";

					// Token: 0x0400EE63 RID: 61027
					public static LocString DEACTIVATE_EXPANSION1 = "DISABLE DLC";

					// Token: 0x0400EE64 RID: 61028
					public static LocString DEACTIVATE_EXPANSION1_TOOLTIP = "<b>This DLC is enabled</b>\n\n<color=#ffff00ff><b>Click to disable the <i>Spaced Out!</i> DLC</b></color>";

					// Token: 0x0400EE65 RID: 61029
					public static LocString DEACTIVATE_EXPANSION1_DESC = "The game will need to restart in order to enable the <i>Oxygen Not Included</i> base game.";

					// Token: 0x0400EE66 RID: 61030
					public static LocString DEACTIVATE_EXPANSION1_RAIL_DESC = "<i>Spaced Out!</i> will be disabled the next time you launch the game. The game will now close.";

					// Token: 0x0400EE67 RID: 61031
					public static LocString AD_DLC1 = "Spaced Out! DLC";

					// Token: 0x0400EE68 RID: 61032
					public static LocString CONTENT_INSTALLED_LABEL = "Installed";

					// Token: 0x0400EE69 RID: 61033
					public static LocString CONTENT_ACTIVE_TOOLTIP = "<b>This DLC is enabled</b>\n\nFind it in the destination selection screen when starting a new game, or in the Load Game screen for existing DLC-enabled saves";

					// Token: 0x0400EE6A RID: 61034
					public static LocString COSMETIC_CONTENT_ACTIVE_TOOLTIP = "<b>This DLC is enabled</b>\n\nFind it in the supply closet screen.";

					// Token: 0x0400EE6B RID: 61035
					public static LocString CONTENT_OWNED_NOTINSTALLED_LABEL = "";

					// Token: 0x0400EE6C RID: 61036
					public static LocString CONTENT_OWNED_NOTINSTALLED_TOOLTIP = "This DLC is owned but not currently installed";

					// Token: 0x0400EE6D RID: 61037
					public static LocString CONTENT_NOTOWNED_LABEL = "Available Now";

					// Token: 0x0400EE6E RID: 61038
					public static LocString CONTENT_NOTOWNED_TOOLTIP = "This DLC is available now!";
				}
			}

			// Token: 0x02002E9B RID: 11931
			public class DEVTOOLS
			{
				// Token: 0x0400C884 RID: 51332
				public static LocString TITLE = "About Dev Tools";

				// Token: 0x0400C885 RID: 51333
				public static LocString WARNING = "DANGER!!\n\nDev Tools are intended for developer use only. Using them may result in your save becoming unplayable, unstable, or severely damaged.\n\nThese tools are completely unsupported and may contain bugs. Are you sure you want to continue?";

				// Token: 0x0400C886 RID: 51334
				public static LocString DONTSHOW = "Do not show this message again";

				// Token: 0x0400C887 RID: 51335
				public static LocString BUTTON = "Show Dev Tools";
			}

			// Token: 0x02002E9C RID: 11932
			public class NEWGAMESETTINGS
			{
				// Token: 0x0400C888 RID: 51336
				public static LocString HEADER = "GAME SETTINGS";

				// Token: 0x02003BC0 RID: 15296
				public class BUTTONS
				{
					// Token: 0x0400EE6F RID: 61039
					public static LocString STANDARDGAME = "Standard Game";

					// Token: 0x0400EE70 RID: 61040
					public static LocString CUSTOMGAME = "Custom Game";

					// Token: 0x0400EE71 RID: 61041
					public static LocString CANCEL = "Cancel";

					// Token: 0x0400EE72 RID: 61042
					public static LocString STARTGAME = "Start Game";
				}
			}

			// Token: 0x02002E9D RID: 11933
			public class COLONYDESTINATIONSCREEN
			{
				// Token: 0x0400C889 RID: 51337
				public static LocString TITLE = "CHOOSE A DESTINATION";

				// Token: 0x0400C88A RID: 51338
				public static LocString GENTLE_ZONE = "Habitable Zone";

				// Token: 0x0400C88B RID: 51339
				public static LocString DETAILS = "Destination Details";

				// Token: 0x0400C88C RID: 51340
				public static LocString START_SITE = "Immediate Surroundings";

				// Token: 0x0400C88D RID: 51341
				public static LocString COORDINATE = "Coordinates:";

				// Token: 0x0400C88E RID: 51342
				public static LocString CANCEL = "Back";

				// Token: 0x0400C88F RID: 51343
				public static LocString CUSTOMIZE = "Game Settings";

				// Token: 0x0400C890 RID: 51344
				public static LocString START_GAME = "Start Game";

				// Token: 0x0400C891 RID: 51345
				public static LocString SHUFFLE = "Shuffle";

				// Token: 0x0400C892 RID: 51346
				public static LocString SHUFFLETOOLTIP = "Reroll World Seed\n\nThis will shuffle the layout of your world and the geographical traits listed below";

				// Token: 0x0400C893 RID: 51347
				public static LocString SHUFFLETOOLTIP_DISABLED = "This world's seed is predetermined. It cannot be changed";

				// Token: 0x0400C894 RID: 51348
				public static LocString HEADER_ASTEROID_STARTING = "Starting Asteroid";

				// Token: 0x0400C895 RID: 51349
				public static LocString HEADER_ASTEROID_NEARBY = "Nearby Asteroids";

				// Token: 0x0400C896 RID: 51350
				public static LocString HEADER_ASTEROID_DISTANT = "Distant Asteroids";

				// Token: 0x0400C897 RID: 51351
				public static LocString TRAITS_HEADER = "World Traits";

				// Token: 0x0400C898 RID: 51352
				public static LocString STORY_TRAITS_HEADER = "Story Traits";

				// Token: 0x0400C899 RID: 51353
				public static LocString MIXING_SETTINGS_HEADER = "Scramble DLCs";

				// Token: 0x0400C89A RID: 51354
				public static LocString MIXING_DLC_HEADER = "DLC Content";

				// Token: 0x0400C89B RID: 51355
				public static LocString MIXING_WORLDMIXING_HEADER = "Asteroid Remix";

				// Token: 0x0400C89C RID: 51356
				public static LocString MIXING_SUBWORLDMIXING_HEADER = "Biome Remix";

				// Token: 0x0400C89D RID: 51357
				public static LocString MIXING_NO_OPTIONS = "No additional content currently available for remixing. Don't worry, there's plenty already baked in.";

				// Token: 0x0400C89E RID: 51358
				public static LocString MIXING_WARNING = "Choose additional content to remix into the game. Scrambling realities may cause cosmic collapse.";

				// Token: 0x0400C89F RID: 51359
				public static LocString MIXING_TOOLTIP_DLC_MIXING = "DLC content includes buildings, Care Packages, space POIs, critters, etc\n\nEnabling DLC content allows asteroid and biome remixes from that DLC to be customized in the sections below";

				// Token: 0x0400C8A0 RID: 51360
				public static LocString MIXING_TOOLTIP_ASTEROID_MIXING = "Asteroid remixing modifies which asteroids appear on the Starmap\n\nRemixed asteroids will retain key features of the outer asteroids that they replace";

				// Token: 0x0400C8A1 RID: 51361
				public static LocString MIXING_TOOLTIP_BIOME_MIXING = "Biome remixing modifies which biomes will be included across multiple asteroids";

				// Token: 0x0400C8A2 RID: 51362
				public static LocString MIXING_TOOLTIP_TOO_MANY_GUARENTEED_ASTEROID_MIXINGS = UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_ASTEROID_MIXING + "\n\nMaximum of {1} guaranteed asteroid remixes allowed\n\nTotal currently selected: {0}";

				// Token: 0x0400C8A3 RID: 51363
				public static LocString MIXING_TOOLTIP_TOO_MANY_GUARENTEED_BIOME_MIXINGS = UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_BIOME_MIXING + "\n\nMaximum of {1} guaranteed biome remixes allowed\n\nTotal currently selected: {0}";

				// Token: 0x0400C8A4 RID: 51364
				public static LocString MIXING_TOOLTIP_LOCKED_START_NOT_SUPPORTED = "This destination does not support changing this setting";

				// Token: 0x0400C8A5 RID: 51365
				public static LocString MIXING_TOOLTIP_LOCKED_REQUIRE_DLC_NOT_ENABLED = "This setting requires the following content to be enabled:\n{0}";

				// Token: 0x0400C8A6 RID: 51366
				public static LocString MIXING_TOOLTIP_DLC_CONTENT = "This content is from {0}";

				// Token: 0x0400C8A7 RID: 51367
				public static LocString MIXING_TOOLTIP_MODDED_SETTING = "<i><color=#d6d6d6>This setting was added by a mod</color></i>";

				// Token: 0x0400C8A8 RID: 51368
				public static LocString MIXING_TOOLTIP_CANNOT_START = "Cannot start a new game with current asteroid and biome remix configuration";

				// Token: 0x0400C8A9 RID: 51369
				public static LocString NO_TRAITS = "No Traits";

				// Token: 0x0400C8AA RID: 51370
				public static LocString SINGLE_TRAIT = "1 Trait";

				// Token: 0x0400C8AB RID: 51371
				public static LocString TRAIT_COUNT = "{0} Traits";

				// Token: 0x0400C8AC RID: 51372
				public static LocString TRAIT_COUNT_TOOLTIP = "Customize selection in the Story Traits tab";

				// Token: 0x0400C8AD RID: 51373
				public static LocString TOO_MANY_TRAITS_WARNING = UI.YELLOW_PREFIX + "Unstable!" + UI.COLOR_SUFFIX;

				// Token: 0x0400C8AE RID: 51374
				public static LocString TOO_MANY_TRAITS_WARNING_TOOLTIP = UI.YELLOW_PREFIX + "Squeezing this many story traits into this asteroid may cause worldgen to fail\n\nConsider lowering the number of story traits or changing the selected asteroid" + UI.COLOR_SUFFIX;

				// Token: 0x0400C8AF RID: 51375
				public static LocString SHUFFLE_STORY_TRAITS_TOOLTIP = "Randomize Story Traits\n\nThis will select a comfortable number of story traits for the starting asteroid";

				// Token: 0x0400C8B0 RID: 51376
				public static LocString SELECTED_CLUSTER_TRAITS_HEADER = "Target Details";
			}

			// Token: 0x02002E9E RID: 11934
			public class MODESELECTSCREEN
			{
				// Token: 0x0400C8B1 RID: 51377
				public static LocString HEADER = "GAME MODE";

				// Token: 0x0400C8B2 RID: 51378
				public static LocString BLANK_DESC = "Select a playstyle...";

				// Token: 0x0400C8B3 RID: 51379
				public static LocString SURVIVAL_TITLE = "SURVIVAL";

				// Token: 0x0400C8B4 RID: 51380
				public static LocString SURVIVAL_DESC = "Stay on your toes and one step ahead of this unforgiving world. One slip up could bring your colony crashing down.";

				// Token: 0x0400C8B5 RID: 51381
				public static LocString NOSWEAT_TITLE = "NO SWEAT";

				// Token: 0x0400C8B6 RID: 51382
				public static LocString NOSWEAT_DESC = "When disaster strikes (and it inevitably will), take a deep breath and stay calm. You have ample time to find a solution.";

				// Token: 0x0400C8B7 RID: 51383
				public static LocString ACTIVE_CONTENT_HEADER = "ACTIVE CONTENT";
			}

			// Token: 0x02002E9F RID: 11935
			public class CLUSTERCATEGORYSELECTSCREEN
			{
				// Token: 0x0400C8B8 RID: 51384
				public static LocString HEADER = "ASTEROID STYLE";

				// Token: 0x0400C8B9 RID: 51385
				public static LocString BLANK_DESC = "Select an asteroid style...";

				// Token: 0x0400C8BA RID: 51386
				public static LocString VANILLA_TITLE = "Standard";

				// Token: 0x0400C8BB RID: 51387
				public static LocString VANILLA_DESC = "Scenarios designed for classic gameplay.";

				// Token: 0x0400C8BC RID: 51388
				public static LocString CLASSIC_TITLE = "Classic";

				// Token: 0x0400C8BD RID: 51389
				public static LocString CLASSIC_DESC = "Scenarios similar to the <b>classic Oxygen Not Included</b> experience. Large starting asteroids with many resources.\nLess emphasis on space travel.";

				// Token: 0x0400C8BE RID: 51390
				public static LocString SPACEDOUT_TITLE = "Spaced Out!";

				// Token: 0x0400C8BF RID: 51391
				public static LocString SPACEDOUT_DESC = "Scenarios designed for the <b>Spaced Out! DLC</b>.\nSmaller starting asteroids with resources distributed across the Starmap. More emphasis on space travel.";

				// Token: 0x0400C8C0 RID: 51392
				public static LocString EVENT_TITLE = "The Lab";

				// Token: 0x0400C8C1 RID: 51393
				public static LocString EVENT_DESC = "Alternative gameplay experiences, including experimental scenarios designed for special events.";
			}

			// Token: 0x02002EA0 RID: 11936
			public class PATCHNOTESSCREEN
			{
				// Token: 0x0400C8C2 RID: 51394
				public static LocString HEADER = "IMPORTANT UPDATE NOTES";

				// Token: 0x0400C8C3 RID: 51395
				public static LocString OK_BUTTON = "OK";

				// Token: 0x0400C8C4 RID: 51396
				public static LocString FULLPATCHNOTES_TOOLTIP = "View the full patch notes online";
			}

			// Token: 0x02002EA1 RID: 11937
			public class LOADSCREEN
			{
				// Token: 0x0400C8C5 RID: 51397
				public static LocString TITLE = "LOAD GAME";

				// Token: 0x0400C8C6 RID: 51398
				public static LocString TITLE_INSPECT = "LOAD GAME";

				// Token: 0x0400C8C7 RID: 51399
				public static LocString DELETEBUTTON = "DELETE";

				// Token: 0x0400C8C8 RID: 51400
				public static LocString BACKBUTTON = "< BACK";

				// Token: 0x0400C8C9 RID: 51401
				public static LocString CONFIRMDELETE = "Are you sure you want to delete {0}?\nYou cannot undo this action.";

				// Token: 0x0400C8CA RID: 51402
				public static LocString SAVEDETAILS = "<b>File:</b> {0}\n\n<b>Save Date:</b>\n{1}\n\n<b>Base Name:</b> {2}\n<b>Duplicants Alive:</b> {3}\n<b>Cycle(s) Survived:</b> {4}";

				// Token: 0x0400C8CB RID: 51403
				public static LocString AUTOSAVEWARNING = "<color=#F44A47FF>Autosave: This file will get deleted as new autosaves are created</color>";

				// Token: 0x0400C8CC RID: 51404
				public static LocString CORRUPTEDSAVE = "<b><color=#F44A47FF>Could not load file {0}. Its data may be corrupted.</color></b>";

				// Token: 0x0400C8CD RID: 51405
				public static LocString SAVE_TOO_NEW = "<b><color=#F44A47FF>Could not load file {0}. File is using build {1}, v{2}. This build is {3}, v{4}.</color></b>";

				// Token: 0x0400C8CE RID: 51406
				public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION = "This save file was created with a different DLC configuration\n\nTo load this file:";

				// Token: 0x0400C8CF RID: 51407
				public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_ENABLE = "    • Activate {0}";

				// Token: 0x0400C8D0 RID: 51408
				public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_DISABLE = "    • Deactivate {0}";

				// Token: 0x0400C8D1 RID: 51409
				public static LocString TOOLTIP_SAVE_USES_DLC = "{0} save";

				// Token: 0x0400C8D2 RID: 51410
				public static LocString UNSUPPORTED_SAVE_VERSION = "<b><color=#F44A47FF>This save file is from a previous version of the game and is no longer supported.</color></b>";

				// Token: 0x0400C8D3 RID: 51411
				public static LocString MORE_INFO = "More Info";

				// Token: 0x0400C8D4 RID: 51412
				public static LocString NEWEST_SAVE = "NEWEST";

				// Token: 0x0400C8D5 RID: 51413
				public static LocString BASE_NAME = "Base Name";

				// Token: 0x0400C8D6 RID: 51414
				public static LocString CYCLES_SURVIVED = "Cycles Survived";

				// Token: 0x0400C8D7 RID: 51415
				public static LocString DUPLICANTS_ALIVE = "Duplicants Alive";

				// Token: 0x0400C8D8 RID: 51416
				public static LocString WORLD_NAME = "Asteroid Type";

				// Token: 0x0400C8D9 RID: 51417
				public static LocString NO_FILE_SELECTED = "No file selected";

				// Token: 0x0400C8DA RID: 51418
				public static LocString COLONY_INFO_FMT = "{0}: {1}";

				// Token: 0x0400C8DB RID: 51419
				public static LocString LOAD_MORE_COLONIES_BUTTON = "Load more...";

				// Token: 0x0400C8DC RID: 51420
				public static LocString VANILLA_RESTART = "Loading this colony will require restarting the game with " + UI.DLC1.NAME_ITAL + " content disabled";

				// Token: 0x0400C8DD RID: 51421
				public static LocString EXPANSION1_RESTART = "Loading this colony will require restarting the game with " + UI.DLC1.NAME_ITAL + " content enabled";

				// Token: 0x0400C8DE RID: 51422
				public static LocString UNSUPPORTED_VANILLA_TEMP = "<b><color=#F44A47FF>This save file is from the base version of the game and currently cannot be loaded while " + UI.DLC1.NAME_ITAL + " is installed.</color></b>";

				// Token: 0x0400C8DF RID: 51423
				public static LocString CONTENT = "Content";

				// Token: 0x0400C8E0 RID: 51424
				public static LocString VANILLA_CONTENT = "Vanilla FIXME";

				// Token: 0x0400C8E1 RID: 51425
				public static LocString EXPANSION1_CONTENT = UI.DLC1.NAME_ITAL + " Expansion FIXME";

				// Token: 0x0400C8E2 RID: 51426
				public static LocString SAVE_INFO = "{0} saves  {1} autosaves  {2}";

				// Token: 0x0400C8E3 RID: 51427
				public static LocString COLONIES_TITLE = "Colony View";

				// Token: 0x0400C8E4 RID: 51428
				public static LocString COLONY_TITLE = "Viewing colony '{0}'";

				// Token: 0x0400C8E5 RID: 51429
				public static LocString COLONY_FILE_SIZE = "Size: {0}";

				// Token: 0x0400C8E6 RID: 51430
				public static LocString COLONY_FILE_NAME = "File: '{0}'";

				// Token: 0x0400C8E7 RID: 51431
				public static LocString NO_PREVIEW = "NO PREVIEW";

				// Token: 0x0400C8E8 RID: 51432
				public static LocString LOCAL_SAVE = "local";

				// Token: 0x0400C8E9 RID: 51433
				public static LocString CLOUD_SAVE = "cloud";

				// Token: 0x0400C8EA RID: 51434
				public static LocString CONVERT_COLONY = "CONVERT COLONY";

				// Token: 0x0400C8EB RID: 51435
				public static LocString CONVERT_ALL_COLONIES = "CONVERT ALL";

				// Token: 0x0400C8EC RID: 51436
				public static LocString CONVERT_ALL_WARNING = UI.PRE_KEYWORD + "\nWarning:" + UI.PST_KEYWORD + " Converting all colonies may take some time.";

				// Token: 0x0400C8ED RID: 51437
				public static LocString SAVE_INFO_DIALOG_TITLE = "SAVE INFORMATION";

				// Token: 0x0400C8EE RID: 51438
				public static LocString SAVE_INFO_DIALOG_TEXT = "Access your save files using the options below.";

				// Token: 0x0400C8EF RID: 51439
				public static LocString SAVE_INFO_DIALOG_TOOLTIP = "Access your save file locations from here.";

				// Token: 0x0400C8F0 RID: 51440
				public static LocString CONVERT_ERROR_TITLE = "SAVE CONVERSION UNSUCCESSFUL";

				// Token: 0x0400C8F1 RID: 51441
				public static LocString CONVERT_ERROR = string.Concat(new string[]
				{
					"Converting the colony ",
					UI.PRE_KEYWORD,
					"{Colony}",
					UI.PST_KEYWORD,
					" was unsuccessful!\nThe error was:\n\n<b>{Error}</b>\n\nPlease try again, or post a bug in the forums if this problem keeps happening."
				});

				// Token: 0x0400C8F2 RID: 51442
				public static LocString CONVERT_TO_CLOUD = "CONVERT TO CLOUD SAVES";

				// Token: 0x0400C8F3 RID: 51443
				public static LocString CONVERT_TO_LOCAL = "CONVERT TO LOCAL SAVES";

				// Token: 0x0400C8F4 RID: 51444
				public static LocString CONVERT_COLONY_TO_CLOUD = "Convert colony to use cloud saves";

				// Token: 0x0400C8F5 RID: 51445
				public static LocString CONVERT_COLONY_TO_LOCAL = "Convert to colony to use local saves";

				// Token: 0x0400C8F6 RID: 51446
				public static LocString CONVERT_ALL_TO_CLOUD = "Convert <b>all</b> colonies below to use cloud saves";

				// Token: 0x0400C8F7 RID: 51447
				public static LocString CONVERT_ALL_TO_LOCAL = "Convert <b>all</b> colonies below to use local saves";

				// Token: 0x0400C8F8 RID: 51448
				public static LocString CONVERT_ALL_TO_CLOUD_SUCCESS = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nAll existing colonies have been converted into ",
					UI.PRE_KEYWORD,
					"cloud",
					UI.PST_KEYWORD,
					" saves.\nNew colonies will use ",
					UI.PRE_KEYWORD,
					"cloud",
					UI.PST_KEYWORD,
					" saves by default.\n\n{Client} may take longer than usual to sync the next time you exit the game as a result of this change."
				});

				// Token: 0x0400C8F9 RID: 51449
				public static LocString CONVERT_ALL_TO_LOCAL_SUCCESS = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nAll existing colonies have been converted into ",
					UI.PRE_KEYWORD,
					"local",
					UI.PST_KEYWORD,
					" saves.\nNew colonies will use ",
					UI.PRE_KEYWORD,
					"local",
					UI.PST_KEYWORD,
					" saves by default.\n\n{Client} may take longer than usual to sync the next time you exit the game as a result of this change."
				});

				// Token: 0x0400C8FA RID: 51450
				public static LocString CONVERT_TO_CLOUD_DETAILS = "Converting a colony to use cloud saves will move all of the save files for that colony into the cloud saves folder.\n\nThis allows your game platform to sync this colony to the cloud for your account, so it can be played on multiple machines.";

				// Token: 0x0400C8FB RID: 51451
				public static LocString CONVERT_TO_LOCAL_DETAILS = "Converting a colony to NOT use cloud saves will move all of the save files for that colony into the local saves folder.\n\n" + UI.PRE_KEYWORD + "These save files will no longer be synced to the cloud." + UI.PST_KEYWORD;

				// Token: 0x0400C8FC RID: 51452
				public static LocString OPEN_SAVE_FOLDER = "LOCAL SAVES";

				// Token: 0x0400C8FD RID: 51453
				public static LocString OPEN_CLOUDSAVE_FOLDER = "CLOUD SAVES";

				// Token: 0x0400C8FE RID: 51454
				public static LocString MIGRATE_TITLE = "SAVE FILE MIGRATION";

				// Token: 0x0400C8FF RID: 51455
				public static LocString MIGRATE_SAVE_FILES = "MIGRATE SAVE FILES";

				// Token: 0x0400C900 RID: 51456
				public static LocString MIGRATE_COUNT = string.Concat(new string[]
				{
					"\nFound ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" autosaves that require migration."
				});

				// Token: 0x0400C901 RID: 51457
				public static LocString MIGRATE_RESULT = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nMigration moved ",
					UI.PRE_KEYWORD,
					"{0}/{1}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{2}/{3}",
					UI.PST_KEYWORD,
					" autosaves",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400C902 RID: 51458
				public static LocString MIGRATE_RESULT_FAILURES = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"<b>WARNING:</b> Not all saves could be migrated.",
					UI.PST_KEYWORD,
					"\nMigration moved ",
					UI.PRE_KEYWORD,
					"{0}/{1}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{2}/{3}",
					UI.PST_KEYWORD,
					" autosaves.\n\nThe file ",
					UI.PRE_KEYWORD,
					"{ErrorColony}",
					UI.PST_KEYWORD,
					" encountered this error:\n\n<b>{ErrorMessage}</b>"
				});

				// Token: 0x0400C903 RID: 51459
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE = "MIGRATION INCOMPLETE";

				// Token: 0x0400C904 RID: 51460
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_PRE = "<b>The game was unable to move all save files to their new location.\nTo fix this, please:</b>\n\n";

				// Token: 0x0400C905 RID: 51461
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1 = "    1. Try temporarily disabling virus scanners and malware\n         protection programs.";

				// Token: 0x0400C906 RID: 51462
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2 = "    2. Turn off file sync services such as OneDrive and DropBox.";

				// Token: 0x0400C907 RID: 51463
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3 = "    3. Restart the game to retry file migration.";

				// Token: 0x0400C908 RID: 51464
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_POST = "\n<b>If this still doesn't solve the problem, please post a bug in the forums and we will attempt to assist with your issue.</b>";

				// Token: 0x0400C909 RID: 51465
				public static LocString MIGRATE_INFO = "We've changed how save files are organized!\nPlease " + UI.CLICK(UI.ClickType.click) + " the button below to automatically update your save file storage.";

				// Token: 0x0400C90A RID: 51466
				public static LocString MIGRATE_DONE = "CONTINUE";

				// Token: 0x0400C90B RID: 51467
				public static LocString MIGRATE_FAILURES_FORUM_BUTTON = "VISIT FORUMS";

				// Token: 0x0400C90C RID: 51468
				public static LocString MIGRATE_FAILURES_DONE = "MORE INFO";

				// Token: 0x0400C90D RID: 51469
				public static LocString CLOUD_TUTORIAL_BOUNCER = "Upload Saves to Cloud";
			}

			// Token: 0x02002EA2 RID: 11938
			public class SAVESCREEN
			{
				// Token: 0x0400C90E RID: 51470
				public static LocString TITLE = "SAVE SLOTS";

				// Token: 0x0400C90F RID: 51471
				public static LocString NEWSAVEBUTTON = "New Save";

				// Token: 0x0400C910 RID: 51472
				public static LocString OVERWRITEMESSAGE = "Are you sure you want to overwrite {0}?";

				// Token: 0x0400C911 RID: 51473
				public static LocString SAVENAMETITLE = "SAVE NAME";

				// Token: 0x0400C912 RID: 51474
				public static LocString CONFIRMNAME = "Confirm";

				// Token: 0x0400C913 RID: 51475
				public static LocString CANCELNAME = "Cancel";

				// Token: 0x0400C914 RID: 51476
				public static LocString IO_ERROR = "An error occurred trying to save your game. Please ensure there is sufficient disk space.\n\n{0}";

				// Token: 0x0400C915 RID: 51477
				public static LocString REPORT_BUG = "Report Bug";

				// Token: 0x0400C916 RID: 51478
				public static LocString SAVE_COMPLETE_MESSAGE = "Save Complete";
			}

			// Token: 0x02002EA3 RID: 11939
			public class RAILFORCEQUIT
			{
				// Token: 0x0400C917 RID: 51479
				public static LocString SAVE_EXIT = "Play time has expired and the game is exiting. Would you like to overwrite {0}?";

				// Token: 0x0400C918 RID: 51480
				public static LocString WARN_EXIT = "Play time has expired and the game will now exit.";

				// Token: 0x0400C919 RID: 51481
				public static LocString DLC_NOT_PURCHASED = "The <i>Spaced Out!</i> DLC has not yet been purchased in the WeGame store. Purchase <i>Spaced Out!</i> to support <i>Oxygen Not Included</i> and enjoy the new content!";
			}

			// Token: 0x02002EA4 RID: 11940
			public class MOD_ERRORS
			{
				// Token: 0x0400C91A RID: 51482
				public static LocString TITLE = "MOD ERRORS";

				// Token: 0x0400C91B RID: 51483
				public static LocString DETAILS = "DETAILS";

				// Token: 0x0400C91C RID: 51484
				public static LocString CLOSE = "CLOSE";
			}

			// Token: 0x02002EA5 RID: 11941
			public class MODS
			{
				// Token: 0x0400C91D RID: 51485
				public static LocString TITLE = "MODS";

				// Token: 0x0400C91E RID: 51486
				public static LocString MANAGE = "Subscription";

				// Token: 0x0400C91F RID: 51487
				public static LocString MANAGE_LOCAL = "Browse";

				// Token: 0x0400C920 RID: 51488
				public static LocString WORKSHOP = "STEAM WORKSHOP";

				// Token: 0x0400C921 RID: 51489
				public static LocString ENABLE_ALL = "ENABLE ALL";

				// Token: 0x0400C922 RID: 51490
				public static LocString DISABLE_ALL = "DISABLE ALL";

				// Token: 0x0400C923 RID: 51491
				public static LocString DRAG_TO_REORDER = "Drag to reorder";

				// Token: 0x0400C924 RID: 51492
				public static LocString REQUIRES_RESTART = "Mod changes require restart";

				// Token: 0x0400C925 RID: 51493
				public static LocString FAILED_TO_LOAD = "A mod failed to load and is being disabled:\n\n{0}: {1}\n\n{2}";

				// Token: 0x0400C926 RID: 51494
				public static LocString DB_CORRUPT = "An error occurred trying to load the Mod Database.\n\n{0}";

				// Token: 0x0400C927 RID: 51495
				public static LocString NO_DESCRIPTION = "No description found in mod.yaml";

				// Token: 0x02003BC1 RID: 15297
				public class CONTENT_FAILURE
				{
					// Token: 0x0400EE73 RID: 61043
					public static LocString DISABLED_CONTENT = " - <b>Incompatible DLC configuration</b>";

					// Token: 0x0400EE74 RID: 61044
					public static LocString DISABLED_CONTENT_TOOLTIP = "The current configuration of enabled/disabled DLCs does not match this mod's specifications.";

					// Token: 0x0400EE75 RID: 61045
					public static LocString DISABLED_CONTENT_TOOLTIP_REQUIRED = "<b>Required DLCs:</b>";

					// Token: 0x0400EE76 RID: 61046
					public static LocString DISABLED_CONTENT_TOOLTIP_FORBIDDEN_DLC = "<b>Forbidden DLCs:</b>";

					// Token: 0x0400EE77 RID: 61047
					public static LocString NO_CONTENT = " - <b>No compatible mod found</b>";

					// Token: 0x0400EE78 RID: 61048
					public static LocString NO_CONTENT_TOOLTIP = "No content was found to load.";

					// Token: 0x0400EE79 RID: 61049
					public static LocString OLD_API = " - <b>Mod out-of-date</b>";

					// Token: 0x0400EE7A RID: 61050
					public static LocString OLD_API_TOOLTIP = "This mod is outdated.";
				}

				// Token: 0x02003BC2 RID: 15298
				public class TOOLTIPS
				{
					// Token: 0x0400EE7B RID: 61051
					public static LocString ENABLED = "Enabled";

					// Token: 0x0400EE7C RID: 61052
					public static LocString DISABLED = "Disabled";

					// Token: 0x0400EE7D RID: 61053
					public static LocString MANAGE_STEAM_SUBSCRIPTION = "Manage Steam Subscription";

					// Token: 0x0400EE7E RID: 61054
					public static LocString MANAGE_RAIL_SUBSCRIPTION = "Manage Subscription";

					// Token: 0x0400EE7F RID: 61055
					public static LocString MANAGE_LOCAL_MOD = "Manage Local Mod";
				}

				// Token: 0x02003BC3 RID: 15299
				public class RAILMODUPLOAD
				{
					// Token: 0x0400EE80 RID: 61056
					public static LocString TITLE = "Upload Mod";

					// Token: 0x0400EE81 RID: 61057
					public static LocString NAME = "Mod Name";

					// Token: 0x0400EE82 RID: 61058
					public static LocString DESCRIPTION = "Mod Description";

					// Token: 0x0400EE83 RID: 61059
					public static LocString VERSION = "Version Number";

					// Token: 0x0400EE84 RID: 61060
					public static LocString PREVIEW_IMAGE = "Preview Image Path";

					// Token: 0x0400EE85 RID: 61061
					public static LocString CONTENT_FOLDER = "Content Folder Path";

					// Token: 0x0400EE86 RID: 61062
					public static LocString SHARE_TYPE = "Share Type";

					// Token: 0x0400EE87 RID: 61063
					public static LocString SUBMIT = "Submit";

					// Token: 0x0400EE88 RID: 61064
					public static LocString SUBMIT_READY = "This mod is ready to submit";

					// Token: 0x0400EE89 RID: 61065
					public static LocString SUBMIT_NOT_READY = "The mod cannot be submitted. Check that all fields are properly entered and that the paths are valid.";

					// Token: 0x020040A3 RID: 16547
					public static class MOD_SHARE_TYPE
					{
						// Token: 0x0400FA10 RID: 64016
						public static LocString PRIVATE = "Private";

						// Token: 0x0400FA11 RID: 64017
						public static LocString TOOLTIP_PRIVATE = "This mod will only be visible to its creator";

						// Token: 0x0400FA12 RID: 64018
						public static LocString FRIEND = "Friend";

						// Token: 0x0400FA13 RID: 64019
						public static LocString TOOLTIP_FRIEND = "Friend";

						// Token: 0x0400FA14 RID: 64020
						public static LocString PUBLIC = "Public";

						// Token: 0x0400FA15 RID: 64021
						public static LocString TOOLTIP_PUBLIC = "This mod will be available to all players after publishing. It may be subject to review before being allowed to be published.";
					}

					// Token: 0x020040A4 RID: 16548
					public static class MOD_UPLOAD_RESULT
					{
						// Token: 0x0400FA16 RID: 64022
						public static LocString SUCCESS = "Mod upload succeeded.";

						// Token: 0x0400FA17 RID: 64023
						public static LocString FAILURE = "Mod upload failed.";
					}
				}
			}

			// Token: 0x02002EA6 RID: 11942
			public class MOD_EVENTS
			{
				// Token: 0x0400C928 RID: 51496
				public static LocString REQUIRED = "REQUIRED";

				// Token: 0x0400C929 RID: 51497
				public static LocString NOT_FOUND = "NOT FOUND";

				// Token: 0x0400C92A RID: 51498
				public static LocString INSTALL_INFO_INACCESSIBLE = "INACCESSIBLE";

				// Token: 0x0400C92B RID: 51499
				public static LocString OUT_OF_ORDER = "ORDERING CHANGED";

				// Token: 0x0400C92C RID: 51500
				public static LocString ACTIVE_DURING_CRASH = "ACTIVE DURING CRASH";

				// Token: 0x0400C92D RID: 51501
				public static LocString EXPECTED_ENABLED = "NEWLY DISABLED";

				// Token: 0x0400C92E RID: 51502
				public static LocString EXPECTED_DISABLED = "NEWLY ENABLED";

				// Token: 0x0400C92F RID: 51503
				public static LocString VERSION_UPDATE = "VERSION UPDATE";

				// Token: 0x0400C930 RID: 51504
				public static LocString AVAILABLE_CONTENT_CHANGED = "CONTENT CHANGED";

				// Token: 0x0400C931 RID: 51505
				public static LocString INSTALL_FAILED = "INSTALL FAILED";

				// Token: 0x0400C932 RID: 51506
				public static LocString DOWNLOAD_FAILED = "STEAM DOWNLOAD FAILED";

				// Token: 0x0400C933 RID: 51507
				public static LocString INSTALLED = "INSTALLED";

				// Token: 0x0400C934 RID: 51508
				public static LocString UNINSTALLED = "UNINSTALLED";

				// Token: 0x0400C935 RID: 51509
				public static LocString REQUIRES_RESTART = "RESTART REQUIRED";

				// Token: 0x0400C936 RID: 51510
				public static LocString BAD_WORLD_GEN = "LOAD FAILED";

				// Token: 0x0400C937 RID: 51511
				public static LocString DEACTIVATED = "DEACTIVATED";

				// Token: 0x0400C938 RID: 51512
				public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = "DEACTIVATED";

				// Token: 0x02003BC4 RID: 15300
				public class TOOLTIPS
				{
					// Token: 0x0400EE8A RID: 61066
					public static LocString REQUIRED = "The current save game couldn't load this mod. Unexpected things may happen!";

					// Token: 0x0400EE8B RID: 61067
					public static LocString NOT_FOUND = "This mod isn't installed";

					// Token: 0x0400EE8C RID: 61068
					public static LocString INSTALL_INFO_INACCESSIBLE = "Mod files are inaccessible";

					// Token: 0x0400EE8D RID: 61069
					public static LocString OUT_OF_ORDER = "Active mod has changed order with respect to some other active mod";

					// Token: 0x0400EE8E RID: 61070
					public static LocString ACTIVE_DURING_CRASH = "Mod was active during a crash and may be the cause";

					// Token: 0x0400EE8F RID: 61071
					public static LocString EXPECTED_ENABLED = "This mod needs to be enabled";

					// Token: 0x0400EE90 RID: 61072
					public static LocString EXPECTED_DISABLED = "This mod needs to be disabled";

					// Token: 0x0400EE91 RID: 61073
					public static LocString VERSION_UPDATE = "New version detected";

					// Token: 0x0400EE92 RID: 61074
					public static LocString AVAILABLE_CONTENT_CHANGED = "Content added or removed";

					// Token: 0x0400EE93 RID: 61075
					public static LocString INSTALL_FAILED = "Installation failed";

					// Token: 0x0400EE94 RID: 61076
					public static LocString DOWNLOAD_FAILED = "Steam failed to download the mod";

					// Token: 0x0400EE95 RID: 61077
					public static LocString INSTALLED = "Installation succeeded";

					// Token: 0x0400EE96 RID: 61078
					public static LocString UNINSTALLED = "Uninstalled";

					// Token: 0x0400EE97 RID: 61079
					public static LocString BAD_WORLD_GEN = "Encountered an error while loading file";

					// Token: 0x0400EE98 RID: 61080
					public static LocString DEACTIVATED = "Deactivated due to errors";

					// Token: 0x0400EE99 RID: 61081
					public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = "Deactivated due to Early Access for " + UI.DLC1.NAME_ITAL;
				}
			}

			// Token: 0x02002EA7 RID: 11943
			public class MOD_DIALOGS
			{
				// Token: 0x0400C939 RID: 51513
				public static LocString ADDITIONAL_MOD_EVENTS = "(...additional entries omitted)";

				// Token: 0x02003BC5 RID: 15301
				public class INSTALL_INFO_INACCESSIBLE
				{
					// Token: 0x0400EE9A RID: 61082
					public static LocString TITLE = "STEAM CONTENT ERROR";

					// Token: 0x0400EE9B RID: 61083
					public static LocString MESSAGE = "Failed to access local Steam files for mod {0}.\nTry restarting Oxygen not Included.\nIf that doesn't work, try re-subscribing to the mod via Steam.";
				}

				// Token: 0x02003BC6 RID: 15302
				public class STEAM_SUBSCRIBED
				{
					// Token: 0x0400EE9C RID: 61084
					public static LocString TITLE = "STEAM MOD SUBSCRIBED";

					// Token: 0x0400EE9D RID: 61085
					public static LocString MESSAGE = "Subscribed to Steam mod: {0}";
				}

				// Token: 0x02003BC7 RID: 15303
				public class STEAM_UPDATED
				{
					// Token: 0x0400EE9E RID: 61086
					public static LocString TITLE = "STEAM MOD UPDATE";

					// Token: 0x0400EE9F RID: 61087
					public static LocString MESSAGE = "Updating version of Steam mod: {0}";
				}

				// Token: 0x02003BC8 RID: 15304
				public class STEAM_UNSUBSCRIBED
				{
					// Token: 0x0400EEA0 RID: 61088
					public static LocString TITLE = "STEAM MOD UNSUBSCRIBED";

					// Token: 0x0400EEA1 RID: 61089
					public static LocString MESSAGE = "Unsubscribed from Steam mod: {0}";
				}

				// Token: 0x02003BC9 RID: 15305
				public class STEAM_REFRESH
				{
					// Token: 0x0400EEA2 RID: 61090
					public static LocString TITLE = "STEAM MODS REFRESHED";

					// Token: 0x0400EEA3 RID: 61091
					public static LocString MESSAGE = "Refreshed Steam mods:\n{0}";
				}

				// Token: 0x02003BCA RID: 15306
				public class ALL_MODS_DISABLED_EARLY_ACCESS
				{
					// Token: 0x0400EEA4 RID: 61092
					public static LocString TITLE = "ALL MODS DISABLED";

					// Token: 0x0400EEA5 RID: 61093
					public static LocString MESSAGE = "Mod support is temporarily suspended for the initial launch of " + UI.DLC1.NAME_ITAL + " into Early Access:\n{0}";
				}

				// Token: 0x02003BCB RID: 15307
				public class LOAD_FAILURE
				{
					// Token: 0x0400EEA6 RID: 61094
					public static LocString TITLE = "LOAD FAILURE";

					// Token: 0x0400EEA7 RID: 61095
					public static LocString MESSAGE = "Failed to load one or more mods:\n{0}\nThey will be re-installed when the game is restarted.\nGame may be unstable until restarted.";
				}

				// Token: 0x02003BCC RID: 15308
				public class SAVE_GAME_MODS_DIFFER
				{
					// Token: 0x0400EEA8 RID: 61096
					public static LocString TITLE = "MOD DIFFERENCES";

					// Token: 0x0400EEA9 RID: 61097
					public static LocString MESSAGE = "Save game mods differ from currently active mods:\n{0}";
				}

				// Token: 0x02003BCD RID: 15309
				public class MOD_ERRORS_ON_BOOT
				{
					// Token: 0x0400EEAA RID: 61098
					public static LocString TITLE = "MOD ERRORS";

					// Token: 0x0400EEAB RID: 61099
					public static LocString MESSAGE = "An error occurred during start-up with mods active.\nAll mods have been disabled to ensure a clean restart.\n{0}";

					// Token: 0x0400EEAC RID: 61100
					public static LocString DEV_MESSAGE = "An error occurred during start-up with mods active.\n{0}\nDisable all mods and restart, or continue in an unstable state?";
				}

				// Token: 0x02003BCE RID: 15310
				public class MODS_SCREEN_CHANGES
				{
					// Token: 0x0400EEAD RID: 61101
					public static LocString TITLE = "MODS CHANGED";

					// Token: 0x0400EEAE RID: 61102
					public static LocString MESSAGE = "{0}\nRestart required to reload mods.\nGame may be unstable until restarted.";
				}

				// Token: 0x02003BCF RID: 15311
				public class MOD_EVENTS
				{
					// Token: 0x0400EEAF RID: 61103
					public static LocString TITLE = "MOD EVENTS";

					// Token: 0x0400EEB0 RID: 61104
					public static LocString MESSAGE = "{0}";

					// Token: 0x0400EEB1 RID: 61105
					public static LocString DEV_MESSAGE = "{0}\nCheck Player.log for details.";
				}

				// Token: 0x02003BD0 RID: 15312
				public class RESTART
				{
					// Token: 0x0400EEB2 RID: 61106
					public static LocString OK = "RESTART";

					// Token: 0x0400EEB3 RID: 61107
					public static LocString CANCEL = "CONTINUE";

					// Token: 0x0400EEB4 RID: 61108
					public static LocString MESSAGE = "{0}\nRestart required.";

					// Token: 0x0400EEB5 RID: 61109
					public static LocString DEV_MESSAGE = "{0}\nRestart required.\nGame may be unstable until restarted.";
				}
			}

			// Token: 0x02002EA8 RID: 11944
			public class PAUSE_SCREEN
			{
				// Token: 0x0400C93A RID: 51514
				public static LocString TITLE = "PAUSED";

				// Token: 0x0400C93B RID: 51515
				public static LocString RESUME = "Resume";

				// Token: 0x0400C93C RID: 51516
				public static LocString LOGBOOK = "Logbook";

				// Token: 0x0400C93D RID: 51517
				public static LocString OPTIONS = "Options";

				// Token: 0x0400C93E RID: 51518
				public static LocString SAVE = "Save";

				// Token: 0x0400C93F RID: 51519
				public static LocString ALREADY_SAVED = "<i><color=#CAC8C8>Already Saved</color></i>";

				// Token: 0x0400C940 RID: 51520
				public static LocString SAVEAS = "Save As";

				// Token: 0x0400C941 RID: 51521
				public static LocString COLONY_SUMMARY = "Colony Summary";

				// Token: 0x0400C942 RID: 51522
				public static LocString LOCKERMENU = "Supply Closet";

				// Token: 0x0400C943 RID: 51523
				public static LocString LOAD = "Load";

				// Token: 0x0400C944 RID: 51524
				public static LocString QUIT = "Main Menu";

				// Token: 0x0400C945 RID: 51525
				public static LocString DESKTOPQUIT = "Quit to Desktop";

				// Token: 0x0400C946 RID: 51526
				public static LocString WORLD_SEED = "Coordinates: {0}";

				// Token: 0x0400C947 RID: 51527
				public static LocString WORLD_SEED_TOOLTIP = "Share coordinates with a friend and they can start a colony on an identical asteroid!\n\n{0} - The asteroid\n\n{1} - The world seed\n\n{2} - Difficulty and Custom settings\n\n{3} - Story Trait settings\n\n{4} - Scramble DLC settings";

				// Token: 0x0400C948 RID: 51528
				public static LocString WORLD_SEED_COPY_TOOLTIP = "Copy Coordinates to clipboard\n\nShare coordinates with a friend and they can start a colony on an identical asteroid!";

				// Token: 0x0400C949 RID: 51529
				public static LocString MANAGEMENT_BUTTON = "Pause Menu";

				// Token: 0x02003BD1 RID: 15313
				public class ADD_DLC_MENU
				{
					// Token: 0x0400EEB6 RID: 61110
					public static LocString ENABLE_QUESTION = "Enable DLC content on this save?\n\nThis will create a new copy of the save game. It will no longer be possible to load this copy without the DLC enabled.";

					// Token: 0x0400EEB7 RID: 61111
					public static LocString CONFIRM = "CONFIRM";

					// Token: 0x0400EEB8 RID: 61112
					public static LocString DLC_ENABLED_TOOLTIP = "This save has content from <b>{0}</b> DLC enabled";

					// Token: 0x0400EEB9 RID: 61113
					public static LocString DLC_DISABLED_TOOLTIP = "This save does not currently have content from <b>{0}</b> DLC enabled \n\n<b>Click to enable it</b>";

					// Token: 0x0400EEBA RID: 61114
					public static LocString DLC_DISABLED_NOT_EDITABLE_TOOLTIP = "This save does not have content from the <b>{0}</b> DLC enabled";
				}
			}

			// Token: 0x02002EA9 RID: 11945
			public class OPTIONS_SCREEN
			{
				// Token: 0x0400C94A RID: 51530
				public static LocString TITLE = "OPTIONS";

				// Token: 0x0400C94B RID: 51531
				public static LocString GRAPHICS = "Graphics";

				// Token: 0x0400C94C RID: 51532
				public static LocString AUDIO = "Audio";

				// Token: 0x0400C94D RID: 51533
				public static LocString GAME = "Game";

				// Token: 0x0400C94E RID: 51534
				public static LocString CONTROLS = "Controls";

				// Token: 0x0400C94F RID: 51535
				public static LocString UNITS = "Temperature Units";

				// Token: 0x0400C950 RID: 51536
				public static LocString METRICS = "Data Communication";

				// Token: 0x0400C951 RID: 51537
				public static LocString LANGUAGE = "Change Language";

				// Token: 0x0400C952 RID: 51538
				public static LocString WORLD_GEN = "World Generation Key";

				// Token: 0x0400C953 RID: 51539
				public static LocString RESET_TUTORIAL = "Reset Tutorial Messages";

				// Token: 0x0400C954 RID: 51540
				public static LocString RESET_TUTORIAL_WARNING = "All tutorial messages will be reset, and\nwill show up again the next time you play the game.";

				// Token: 0x0400C955 RID: 51541
				public static LocString FEEDBACK = "Feedback";

				// Token: 0x0400C956 RID: 51542
				public static LocString CREDITS = "Credits";

				// Token: 0x0400C957 RID: 51543
				public static LocString BACK = "Done";

				// Token: 0x0400C958 RID: 51544
				public static LocString UNLOCK_SANDBOX = "Unlock Sandbox Mode";

				// Token: 0x0400C959 RID: 51545
				public static LocString MODS = "MODS";

				// Token: 0x0400C95A RID: 51546
				public static LocString SAVE_OPTIONS = "Save Options";

				// Token: 0x02003BD2 RID: 15314
				public class TOGGLE_SANDBOX_SCREEN
				{
					// Token: 0x0400EEBB RID: 61115
					public static LocString UNLOCK_SANDBOX_WARNING = "Sandbox Mode will be enabled for this save file";

					// Token: 0x0400EEBC RID: 61116
					public static LocString CONFIRM = "Enable Sandbox Mode";

					// Token: 0x0400EEBD RID: 61117
					public static LocString CANCEL = "Cancel";

					// Token: 0x0400EEBE RID: 61118
					public static LocString CONFIRM_SAVE_BACKUP = "Enable Sandbox Mode, but save a backup first";

					// Token: 0x0400EEBF RID: 61119
					public static LocString BACKUP_SAVE_GAME_APPEND = " (BACKUP)";
				}
			}

			// Token: 0x02002EAA RID: 11946
			public class INPUT_BINDINGS_SCREEN
			{
				// Token: 0x0400C95B RID: 51547
				public static LocString TITLE = "CUSTOMIZE KEYS";

				// Token: 0x0400C95C RID: 51548
				public static LocString RESET = "Reset";

				// Token: 0x0400C95D RID: 51549
				public static LocString APPLY = "Done";

				// Token: 0x0400C95E RID: 51550
				public static LocString DUPLICATE = "{0} was already bound to {1} and is now unbound.";

				// Token: 0x0400C95F RID: 51551
				public static LocString UNBOUND_ACTION = "{0} is unbound. Are you sure you want to continue?";

				// Token: 0x0400C960 RID: 51552
				public static LocString MULTIPLE_UNBOUND_ACTIONS = "You have multiple unbound actions, this may result in difficulty playing the game. Are you sure you want to continue?";

				// Token: 0x0400C961 RID: 51553
				public static LocString WAITING_FOR_INPUT = "???";
			}

			// Token: 0x02002EAB RID: 11947
			public class TRANSLATIONS_SCREEN
			{
				// Token: 0x0400C962 RID: 51554
				public static LocString TITLE = "TRANSLATIONS";

				// Token: 0x0400C963 RID: 51555
				public static LocString UNINSTALL = "Uninstall";

				// Token: 0x0400C964 RID: 51556
				public static LocString PREINSTALLED_HEADER = "Preinstalled Language Packs";

				// Token: 0x0400C965 RID: 51557
				public static LocString UGC_HEADER = "Subscribed Workshop Language Packs";

				// Token: 0x0400C966 RID: 51558
				public static LocString UGC_MOD_TITLE_FORMAT = "{0} (workshop)";

				// Token: 0x0400C967 RID: 51559
				public static LocString ARE_YOU_SURE = "Are you sure you want to uninstall this language pack?";

				// Token: 0x0400C968 RID: 51560
				public static LocString PLEASE_REBOOT = "Please restart your game for these changes to take effect.";

				// Token: 0x0400C969 RID: 51561
				public static LocString NO_PACKS = "Steam Workshop";

				// Token: 0x0400C96A RID: 51562
				public static LocString DOWNLOAD = "Start Download";

				// Token: 0x0400C96B RID: 51563
				public static LocString INSTALL = "Install";

				// Token: 0x0400C96C RID: 51564
				public static LocString INSTALLED = "Installed";

				// Token: 0x0400C96D RID: 51565
				public static LocString NO_STEAM = "Unable to retrieve language list from Steam";

				// Token: 0x0400C96E RID: 51566
				public static LocString RESTART = "RESTART";

				// Token: 0x0400C96F RID: 51567
				public static LocString CANCEL = "CANCEL";

				// Token: 0x0400C970 RID: 51568
				public static LocString MISSING_LANGUAGE_PACK = "Selected language pack ({0}) not found.\nReverting to default language.";

				// Token: 0x0400C971 RID: 51569
				public static LocString UNKNOWN = "Unknown";

				// Token: 0x02003BD3 RID: 15315
				public class PREINSTALLED_LANGUAGES
				{
					// Token: 0x0400EEC0 RID: 61120
					public static LocString EN = "English (Klei)";

					// Token: 0x0400EEC1 RID: 61121
					public static LocString ZH_KLEI = "Chinese (Klei)";

					// Token: 0x0400EEC2 RID: 61122
					public static LocString KO_KLEI = "Korean (Klei)";

					// Token: 0x0400EEC3 RID: 61123
					public static LocString RU_KLEI = "Russian (Klei)";
				}
			}

			// Token: 0x02002EAC RID: 11948
			public class SCENARIOS_MENU
			{
				// Token: 0x0400C972 RID: 51570
				public static LocString TITLE = "Scenarios";

				// Token: 0x0400C973 RID: 51571
				public static LocString UNSUBSCRIBE = "Unsubscribe";

				// Token: 0x0400C974 RID: 51572
				public static LocString UNSUBSCRIBE_CONFIRM = "Are you sure you want to unsubscribe from this scenario?";

				// Token: 0x0400C975 RID: 51573
				public static LocString LOAD_SCENARIO_CONFIRM = "Load the \"{SCENARIO_NAME}\" scenario?";

				// Token: 0x0400C976 RID: 51574
				public static LocString LOAD_CONFIRM_TITLE = "LOAD";

				// Token: 0x0400C977 RID: 51575
				public static LocString SCENARIO_NAME = "Name:";

				// Token: 0x0400C978 RID: 51576
				public static LocString SCENARIO_DESCRIPTION = "Description";

				// Token: 0x0400C979 RID: 51577
				public static LocString BUTTON_DONE = "Done";

				// Token: 0x0400C97A RID: 51578
				public static LocString BUTTON_LOAD = "Load";

				// Token: 0x0400C97B RID: 51579
				public static LocString BUTTON_WORKSHOP = "Steam Workshop";

				// Token: 0x0400C97C RID: 51580
				public static LocString NO_SCENARIOS_AVAILABLE = "No scenarios available.\n\nSubscribe to some in the Steam Workshop.";
			}

			// Token: 0x02002EAD RID: 11949
			public class AUDIO_OPTIONS_SCREEN
			{
				// Token: 0x0400C97D RID: 51581
				public static LocString TITLE = "AUDIO OPTIONS";

				// Token: 0x0400C97E RID: 51582
				public static LocString HEADER_VOLUME = "VOLUME";

				// Token: 0x0400C97F RID: 51583
				public static LocString HEADER_SETTINGS = "SETTINGS";

				// Token: 0x0400C980 RID: 51584
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400C981 RID: 51585
				public static LocString MUSIC_EVERY_CYCLE = "Play background music each morning";

				// Token: 0x0400C982 RID: 51586
				public static LocString MUSIC_EVERY_CYCLE_TOOLTIP = "If enabled, background music will play every cycle instead of every few cycles";

				// Token: 0x0400C983 RID: 51587
				public static LocString AUTOMATION_SOUNDS_ALWAYS = "Always play automation sounds";

				// Token: 0x0400C984 RID: 51588
				public static LocString AUTOMATION_SOUNDS_ALWAYS_TOOLTIP = "If enabled, automation sound effects will play even when outside of the " + UI.FormatAsOverlay("Automation Overlay");

				// Token: 0x0400C985 RID: 51589
				public static LocString MUTE_ON_FOCUS_LOST = "Mute when unfocused";

				// Token: 0x0400C986 RID: 51590
				public static LocString MUTE_ON_FOCUS_LOST_TOOLTIP = "If enabled, the game will be muted while minimized or if the application loses focus";

				// Token: 0x0400C987 RID: 51591
				public static LocString AUDIO_BUS_MASTER = "Master";

				// Token: 0x0400C988 RID: 51592
				public static LocString AUDIO_BUS_SFX = "SFX";

				// Token: 0x0400C989 RID: 51593
				public static LocString AUDIO_BUS_MUSIC = "Music";

				// Token: 0x0400C98A RID: 51594
				public static LocString AUDIO_BUS_AMBIENCE = "Ambience";

				// Token: 0x0400C98B RID: 51595
				public static LocString AUDIO_BUS_UI = "UI";
			}

			// Token: 0x02002EAE RID: 11950
			public class GAME_OPTIONS_SCREEN
			{
				// Token: 0x0400C98C RID: 51596
				public static LocString TITLE = "GAME OPTIONS";

				// Token: 0x0400C98D RID: 51597
				public static LocString GENERAL_GAME_OPTIONS = "GENERAL";

				// Token: 0x0400C98E RID: 51598
				public static LocString DISABLED_WARNING = "More options available in-game";

				// Token: 0x0400C98F RID: 51599
				public static LocString DEFAULT_TO_CLOUD_SAVES = "Default to cloud saves";

				// Token: 0x0400C990 RID: 51600
				public static LocString DEFAULT_TO_CLOUD_SAVES_TOOLTIP = "When a new colony is created, this controls whether it will be saved into the cloud saves folder for syncing or not.";

				// Token: 0x0400C991 RID: 51601
				public static LocString RESET_TUTORIAL_DESCRIPTION = "Mark all tutorial messages \"unread\"";

				// Token: 0x0400C992 RID: 51602
				public static LocString SANDBOX_DESCRIPTION = "Enable sandbox tools";

				// Token: 0x0400C993 RID: 51603
				public static LocString CONTROLS_DESCRIPTION = "Change key bindings";

				// Token: 0x0400C994 RID: 51604
				public static LocString TEMPERATURE_UNITS = "TEMPERATURE UNITS";

				// Token: 0x0400C995 RID: 51605
				public static LocString SAVE_OPTIONS = "SAVE";

				// Token: 0x0400C996 RID: 51606
				public static LocString CAMERA_SPEED_LABEL = "Camera Pan Speed: {0}%";
			}

			// Token: 0x02002EAF RID: 11951
			public class METRIC_OPTIONS_SCREEN
			{
				// Token: 0x0400C997 RID: 51607
				public static LocString TITLE = "DATA COMMUNICATION";

				// Token: 0x0400C998 RID: 51608
				public static LocString HEADER_METRICS = "USER DATA";

				// Token: 0x0400C999 RID: 51609
				public static LocString USER_ACCOUNT_LINK = "View your Klei account online";
			}

			// Token: 0x02002EB0 RID: 11952
			public class COLONY_SAVE_OPTIONS_SCREEN
			{
				// Token: 0x0400C99A RID: 51610
				public static LocString TITLE = "COLONY SAVE OPTIONS";

				// Token: 0x0400C99B RID: 51611
				public static LocString DESCRIPTION = "Note: These values are configured per save file";

				// Token: 0x0400C99C RID: 51612
				public static LocString AUTOSAVE_FREQUENCY = "Autosave frequency:";

				// Token: 0x0400C99D RID: 51613
				public static LocString AUTOSAVE_FREQUENCY_DESCRIPTION = "Every: {0} cycle(s)";

				// Token: 0x0400C99E RID: 51614
				public static LocString AUTOSAVE_NEVER = "Never";

				// Token: 0x0400C99F RID: 51615
				public static LocString TIMELAPSE_RESOLUTION = "Timelapse resolution:";

				// Token: 0x0400C9A0 RID: 51616
				public static LocString TIMELAPSE_RESOLUTION_DESCRIPTION = "{0}x{1}";

				// Token: 0x0400C9A1 RID: 51617
				public static LocString TIMELAPSE_DISABLED_DESCRIPTION = "Disabled";
			}

			// Token: 0x02002EB1 RID: 11953
			public class FEEDBACK_SCREEN
			{
				// Token: 0x0400C9A2 RID: 51618
				public static LocString TITLE = "FEEDBACK";

				// Token: 0x0400C9A3 RID: 51619
				public static LocString HEADER = "We would love to hear from you!";

				// Token: 0x0400C9A4 RID: 51620
				public static LocString DESCRIPTION = "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file. The buttons to the right will help you find those files on your local drive.\n\nThank you for being part of the Oxygen Not Included community!";

				// Token: 0x0400C9A5 RID: 51621
				public static LocString ALT_DESCRIPTION = "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file.\n\nThank you for being part of the Oxygen Not Included community!";

				// Token: 0x0400C9A6 RID: 51622
				public static LocString BUG_FORUMS_BUTTON = "Report a Bug";

				// Token: 0x0400C9A7 RID: 51623
				public static LocString SUGGESTION_FORUMS_BUTTON = "Suggestions Forum";

				// Token: 0x0400C9A8 RID: 51624
				public static LocString LOGS_DIRECTORY_BUTTON = "Browse Log Files";

				// Token: 0x0400C9A9 RID: 51625
				public static LocString SAVE_FILES_DIRECTORY_BUTTON = "Browse Save Files";
			}

			// Token: 0x02002EB2 RID: 11954
			public class WORLD_GEN_OPTIONS_SCREEN
			{
				// Token: 0x0400C9AA RID: 51626
				public static LocString TITLE = "WORLD GENERATION OPTIONS";

				// Token: 0x0400C9AB RID: 51627
				public static LocString USE_SEED = "Set Worldgen Seed";

				// Token: 0x0400C9AC RID: 51628
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400C9AD RID: 51629
				public static LocString RANDOM_BUTTON = "Randomize";

				// Token: 0x0400C9AE RID: 51630
				public static LocString RANDOM_BUTTON_TOOLTIP = "Randomize a new worldgen seed";

				// Token: 0x0400C9AF RID: 51631
				public static LocString TOOLTIP = "This will override the current worldgen seed";
			}

			// Token: 0x02002EB3 RID: 11955
			public class METRICS_OPTIONS_SCREEN
			{
				// Token: 0x0400C9B0 RID: 51632
				public static LocString TITLE = "DATA COMMUNICATION OPTIONS";

				// Token: 0x0400C9B1 RID: 51633
				public static LocString ENABLE_BUTTON = "Enable Data Communication";

				// Token: 0x0400C9B2 RID: 51634
				public static LocString DESCRIPTION = "Collecting user data helps us improve the game.\n\nPlayers who opt out of data communication will no longer send us crash reports and play data.\n\nThey will also be unable to receive new item unlocks from our servers, though existing unlocked items will continue to function.\n\nFor more details on our privacy policy and how we use the data we collect, please visit our <color=#ECA6C9><u><b>privacy center</b></u></color>.";

				// Token: 0x0400C9B3 RID: 51635
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400C9B4 RID: 51636
				public static LocString RESTART_BUTTON = "Restart Game";

				// Token: 0x0400C9B5 RID: 51637
				public static LocString TOOLTIP = "Uncheck to disable data communication";

				// Token: 0x0400C9B6 RID: 51638
				public static LocString RESTART_WARNING = "A game restart is required to apply settings.";
			}

			// Token: 0x02002EB4 RID: 11956
			public class UNIT_OPTIONS_SCREEN
			{
				// Token: 0x0400C9B7 RID: 51639
				public static LocString TITLE = "TEMPERATURE UNITS";

				// Token: 0x0400C9B8 RID: 51640
				public static LocString CELSIUS = "Celsius";

				// Token: 0x0400C9B9 RID: 51641
				public static LocString CELSIUS_TOOLTIP = "Change temperature unit to Celsius (°C)";

				// Token: 0x0400C9BA RID: 51642
				public static LocString KELVIN = "Kelvin";

				// Token: 0x0400C9BB RID: 51643
				public static LocString KELVIN_TOOLTIP = "Change temperature unit to Kelvin (K)";

				// Token: 0x0400C9BC RID: 51644
				public static LocString FAHRENHEIT = "Fahrenheit";

				// Token: 0x0400C9BD RID: 51645
				public static LocString FAHRENHEIT_TOOLTIP = "Change temperature unit to Fahrenheit (°F)";
			}

			// Token: 0x02002EB5 RID: 11957
			public class GRAPHICS_OPTIONS_SCREEN
			{
				// Token: 0x0400C9BE RID: 51646
				public static LocString TITLE = "GRAPHICS OPTIONS";

				// Token: 0x0400C9BF RID: 51647
				public static LocString FULLSCREEN = "Fullscreen";

				// Token: 0x0400C9C0 RID: 51648
				public static LocString RESOLUTION = "Resolution:";

				// Token: 0x0400C9C1 RID: 51649
				public static LocString LOWRES = "Low Resolution Textures";

				// Token: 0x0400C9C2 RID: 51650
				public static LocString APPLYBUTTON = "Apply";

				// Token: 0x0400C9C3 RID: 51651
				public static LocString REVERTBUTTON = "Revert";

				// Token: 0x0400C9C4 RID: 51652
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400C9C5 RID: 51653
				public static LocString UI_SCALE = "UI Scale";

				// Token: 0x0400C9C6 RID: 51654
				public static LocString HEADER_DISPLAY = "DISPLAY";

				// Token: 0x0400C9C7 RID: 51655
				public static LocString HEADER_UI = "INTERFACE";

				// Token: 0x0400C9C8 RID: 51656
				public static LocString COLORMODE = "Color Mode:";

				// Token: 0x0400C9C9 RID: 51657
				public static LocString COLOR_MODE_DEFAULT = "Default";

				// Token: 0x0400C9CA RID: 51658
				public static LocString COLOR_MODE_PROTANOPIA = "Protanopia";

				// Token: 0x0400C9CB RID: 51659
				public static LocString COLOR_MODE_DEUTERANOPIA = "Deuteranopia";

				// Token: 0x0400C9CC RID: 51660
				public static LocString COLOR_MODE_TRITANOPIA = "Tritanopia";

				// Token: 0x0400C9CD RID: 51661
				public static LocString ACCEPT_CHANGES = "Accept Changes?";

				// Token: 0x0400C9CE RID: 51662
				public static LocString ACCEPT_CHANGES_STRING_COLOR = "Interface changes will be visible immediately, but applying color changes to in-game text will require a restart.\n\nAccept Changes?";

				// Token: 0x0400C9CF RID: 51663
				public static LocString COLORBLIND_FEEDBACK = "Color blindness options are currently in progress.\n\nIf you would benefit from an alternative color mode or have had difficulties with any of the default colors, please visit the forums and let us know about your experiences.\n\nYour feedback is extremely helpful to us!";

				// Token: 0x0400C9D0 RID: 51664
				public static LocString COLORBLIND_FEEDBACK_BUTTON = "Provide Feedback";
			}

			// Token: 0x02002EB6 RID: 11958
			public class WORLDGENSCREEN
			{
				// Token: 0x0400C9D1 RID: 51665
				public static LocString TITLE = "NEW GAME";

				// Token: 0x0400C9D2 RID: 51666
				public static LocString GENERATINGWORLD = "GENERATING WORLD";

				// Token: 0x0400C9D3 RID: 51667
				public static LocString SELECTSIZEPROMPT = "A new world is about to be created. Please select its size.";

				// Token: 0x0400C9D4 RID: 51668
				public static LocString LOADINGGAME = "LOADING WORLD...";

				// Token: 0x02003BD4 RID: 15316
				public class SIZES
				{
					// Token: 0x0400EEC4 RID: 61124
					public static LocString TINY = "Tiny";

					// Token: 0x0400EEC5 RID: 61125
					public static LocString SMALL = "Small";

					// Token: 0x0400EEC6 RID: 61126
					public static LocString STANDARD = "Standard";

					// Token: 0x0400EEC7 RID: 61127
					public static LocString LARGE = "Big";

					// Token: 0x0400EEC8 RID: 61128
					public static LocString HUGE = "Colossal";
				}
			}

			// Token: 0x02002EB7 RID: 11959
			public class MINSPECSCREEN
			{
				// Token: 0x0400C9D5 RID: 51669
				public static LocString TITLE = "WARNING!";

				// Token: 0x0400C9D6 RID: 51670
				public static LocString SIMFAILEDTOLOAD = "A problem occurred loading Oxygen Not Included. This is usually caused by the Visual Studio C++ 2015 runtime being improperly installed on the system. Please exit the game, run Windows Update, and try re-launching Oxygen Not Included.";

				// Token: 0x0400C9D7 RID: 51671
				public static LocString BODY = "We've detected that this computer does not meet the minimum requirements to run Oxygen Not Included. While you may continue with your current specs, the game might not run smoothly for you.\n\nPlease be aware that your experience may suffer as a result.";

				// Token: 0x0400C9D8 RID: 51672
				public static LocString OKBUTTON = "Okay, thanks!";

				// Token: 0x0400C9D9 RID: 51673
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002EB8 RID: 11960
			public class SUPPORTWARNINGS
			{
				// Token: 0x0400C9DA RID: 51674
				public static LocString AUDIO_DRIVERS = "A problem occurred initializing your audio device.\nSorry about that!\n\nThis is usually caused by outdated audio drivers.\n\nPlease visit your audio device manufacturer's website to download the latest drivers.";

				// Token: 0x0400C9DB RID: 51675
				public static LocString AUDIO_DRIVERS_MORE_INFO = "More Info";

				// Token: 0x0400C9DC RID: 51676
				public static LocString DUPLICATE_KEY_BINDINGS = "<b>Duplicate key bindings were detected.\nThis may be because your custom key bindings conflicted with a new feature's default key.\nPlease visit the controls screen to ensure your key bindings are set how you like them.</b>\n{0}";

				// Token: 0x0400C9DD RID: 51677
				public static LocString SAVE_DIRECTORY_READ_ONLY = "A problem occurred while accessing your save directory.\nThis may be because your directory is set to read-only.\n\nPlease ensure your save directory is readable as well as writable and re-launch the game.\n{0}";

				// Token: 0x0400C9DE RID: 51678
				public static LocString SAVE_DIRECTORY_INSUFFICIENT_SPACE = "There is insufficient disk space to write to your save directory.\n\nPlease free at least 15 MB to give your saves some room to breathe.\n{0}";

				// Token: 0x0400C9DF RID: 51679
				public static LocString WORLD_GEN_FILES = "A problem occurred while accessing certain game files that will prevent starting new games.\n\nPlease ensure that the directory and files are readable as well as writable and re-launch the game:\n\n{0}";

				// Token: 0x0400C9E0 RID: 51680
				public static LocString WORLD_GEN_FAILURE = "A problem occurred while generating a world from this seed:\n{0}.\n\nUnfortunately, not all seeds germinate. Please try again with a different seed.";

				// Token: 0x0400C9E1 RID: 51681
				public static LocString WORLD_GEN_FAILURE_MIXING = "A problem occurred while trying to mix a world from this seed:\n{0}.\n\nUnfortunately, not all seeds germinate. Please try again with different remix settings or a different seed.";

				// Token: 0x0400C9E2 RID: 51682
				public static LocString WORLD_GEN_FAILURE_STORY = "A problem occurred while generating a world from this seed:\n{0}.\n\nNot all story traits were able to be placed. Please try again with a different seed or fewer story traits.";

				// Token: 0x0400C9E3 RID: 51683
				public static LocString PLAYER_PREFS_CORRUPTED = "A problem occurred while loading your game options.\nThey have been reset to their default settings.\n\n";

				// Token: 0x0400C9E4 RID: 51684
				public static LocString IO_UNAUTHORIZED = "An Unauthorized Access Error occurred when trying to write to disk.\n\nPlease check that you have permissions to write to:\n{0}\n\nThis may prevent the game from saving.";

				// Token: 0x0400C9E5 RID: 51685
				public static LocString IO_UNAUTHORIZED_ONEDRIVE = "An Unauthorized Access Error occurred when trying to write to disk.\n\nOneDrive may be interfering with the game.\n\nPlease check that you have permissions to write to:\n{0}\n\nThis may prevent the game from saving.";

				// Token: 0x0400C9E6 RID: 51686
				public static LocString IO_SUFFICIENT_SPACE = "An Insufficient Space Error occurred when trying to write to disk. \n\nPlease free up some space.\n{0}";

				// Token: 0x0400C9E7 RID: 51687
				public static LocString IO_UNKNOWN = "An unknown error occurred when trying to write or access a file.\n{0}";

				// Token: 0x0400C9E8 RID: 51688
				public static LocString MORE_INFO_BUTTON = "More Info";
			}

			// Token: 0x02002EB9 RID: 11961
			public class SAVEUPGRADEWARNINGS
			{
				// Token: 0x0400C9E9 RID: 51689
				public static LocString SUDDENMORALEHELPER_TITLE = "MORALE CHANGES";

				// Token: 0x0400C9EA RID: 51690
				public static LocString SUDDENMORALEHELPER = "Welcome to the Expressive Upgrade! This update introduces a new Morale system that replaces Food and Decor Expectations that were found in previous versions of the game.\n\nThe game you are trying to load was created before this system was introduced, and will need to be updated. You may either:\n\n\n1) Enable the new Morale system in this save, removing Food and Decor Expectations. It's possible that when you load your save your old colony won't meet your Duplicants' new Morale needs, so they'll receive a 5 cycle Morale boost to give you time to adjust.\n\n2) Disable Morale in this save. The new Morale mechanics will still be visible, but won't affect your Duplicants' stress. Food and Decor expectations will no longer exist in this save.";

				// Token: 0x0400C9EB RID: 51691
				public static LocString SUDDENMORALEHELPER_BUFF = "1) Bring on Morale!";

				// Token: 0x0400C9EC RID: 51692
				public static LocString SUDDENMORALEHELPER_DISABLE = "2) Disable Morale";

				// Token: 0x0400C9ED RID: 51693
				public static LocString NEWAUTOMATIONWARNING_TITLE = "AUTOMATION CHANGES";

				// Token: 0x0400C9EE RID: 51694
				public static LocString NEWAUTOMATIONWARNING = "The following buildings have acquired new automation ports!\n\nTake a moment to check whether these buildings in your colony are now unintentionally connected to existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s.";

				// Token: 0x0400C9EF RID: 51695
				public static LocString MERGEDOWNCHANGES_TITLE = "BREATH OF FRESH AIR UPDATE CHANGES";

				// Token: 0x0400C9F0 RID: 51696
				public static LocString MERGEDOWNCHANGES = "Oxygen Not Included has had a <b>major update</b> since this save file was created! In addition to the <b>multitude of bug fixes and quality-of-life features</b>, please pay attention to these changes which may affect your existing colony:";

				// Token: 0x0400C9F1 RID: 51697
				public static LocString MERGEDOWNCHANGES_FOOD = "•<indent=20px>Fridges are more effective for early-game food storage</indent>\n•<indent=20px><b>Both</b> freezing temperatures and a sterile gas are needed for <b>total food preservation</b>.</indent>";

				// Token: 0x0400C9F2 RID: 51698
				public static LocString MERGEDOWNCHANGES_AIRFILTER = "•<indent=20px>" + BUILDINGS.PREFABS.AIRFILTER.NAME + " now requires <b>5w Power</b>.</indent>\n•<indent=20px>Duplicants will get <b>Stinging Eyes</b> from gasses such as chlorine and hydrogen.</indent>";

				// Token: 0x0400C9F3 RID: 51699
				public static LocString MERGEDOWNCHANGES_SIMULATION = "•<indent=20px>Many <b>simulation bugs</b> have been fixed.</indent>\n•<indent=20px>This may <b>change the effectiveness</b> of certain contraptions and " + BUILDINGS.PREFABS.STEAMTURBINE2.NAME + " setups.</indent>";

				// Token: 0x0400C9F4 RID: 51700
				public static LocString MERGEDOWNCHANGES_BUILDINGS = "•<indent=20px>The <b>" + BUILDINGS.PREFABS.OXYGENMASKSTATION.NAME + "</b> has been added to aid early-game exploration.</indent>\n•<indent=20px>Use the new <b>Meter Valves</b> for precise control of resources in pipes.</indent>";

				// Token: 0x0400C9F5 RID: 51701
				public static LocString SPACESCANNERANDTELESCOPECHANGES_TITLE = "JUNE 2023 QoL UPDATE CHANGES";

				// Token: 0x0400C9F6 RID: 51702
				public static LocString SPACESCANNERANDTELESCOPECHANGES_SUMMARY = "There have been significant changes to <b>Space Scanners</b> and <b>Telescopes</b> since this save file was created!\n\nMeteor showers have been disabled for 20 cycles to provide time to adapt.";

				// Token: 0x0400C9F7 RID: 51703
				public static LocString SPACESCANNERANDTELESCOPECHANGES_WARNING = "Please note these changes which may affect your existing colony:\n\n";

				// Token: 0x0400C9F8 RID: 51704
				public static LocString SPACESCANNERANDTELESCOPECHANGES_SPACESCANNERS = "•<indent=20px>Automation is synced between all Space Scanners targeting the same object.</indent>\n•<indent=20px>Network quality based on the total percentage of sky covered.</indent>\n•<indent=20px>Industrial machinery no longer impacts network quality.</indent>";

				// Token: 0x0400C9F9 RID: 51705
				public static LocString SPACESCANNERANDTELESCOPECHANGES_TELESCOPES = "•<indent=20px>Telescopes have a symmetrical scanning range.</indent>\n•<indent=20px>Obstructions block visibility from the blocked tile out toward the outer edge of scanning range.</indent>";

				// Token: 0x0400C9FA RID: 51706
				public static LocString U50_CHANGES_TITLE = "IMPORTANT CHANGES";

				// Token: 0x0400C9FB RID: 51707
				public static LocString U50_CHANGES_SUMMARY = "There have been significant changes to critters since this save file was created! Please check on your ranches.";

				// Token: 0x0400C9FC RID: 51708
				public static LocString U50_CHANGES_MOOD = "•<indent=20px>Critter moods have been expanded to include miserable and satisfied states: Miserable stops reproduction. Satisfied gives full metabolism and default reproduction.</indent>";

				// Token: 0x0400C9FD RID: 51709
				public static LocString U50_CHANGES_PACU = "•<indent=20px>Pacus have received a number of bug fixes and changes affecting their reproduction: Now correctly Confined when flopping or in less than 8 tiles of liquid. Easier to feed due to a rebalanced diet.</indent>";

				// Token: 0x0400C9FE RID: 51710
				public static LocString U50_CHANGES_SUITCHECKPOINTS = "•<indent=20px>Suit checkpoints now have an automation port to disable them so Duplicants can pass through. Some checkpoints may now be unintentionally connected to existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s.";

				// Token: 0x0400C9FF RID: 51711
				public static LocString U50_CHANGES_METER_VALVES = "•<indent=20px>Meter valves no longer continuously reset when receiving a green signal.</indent>";
			}
		}

		// Token: 0x0200251C RID: 9500
		public class SANDBOX_TOGGLE
		{
			// Token: 0x0400A4E8 RID: 42216
			public static LocString TOOLTIP_LOCKED = "<b>Sandbox Mode</b> must be unlocked in the options menu before it can be used. {Hotkey}";

			// Token: 0x0400A4E9 RID: 42217
			public static LocString TOOLTIP_UNLOCKED = "Toggle <b>Sandbox Mode</b> {Hotkey}";
		}

		// Token: 0x0200251D RID: 9501
		public class SKILLS_SCREEN
		{
			// Token: 0x0400A4EA RID: 42218
			public static LocString CURRENT_MORALE = "Current Morale: {0}\nMorale Need: {1}";

			// Token: 0x0400A4EB RID: 42219
			public static LocString SORT_BY_DUPLICANT = "Duplicants";

			// Token: 0x0400A4EC RID: 42220
			public static LocString SORT_BY_MORALE = "Morale";

			// Token: 0x0400A4ED RID: 42221
			public static LocString SORT_BY_EXPERIENCE = "Skill Points";

			// Token: 0x0400A4EE RID: 42222
			public static LocString SORT_BY_SKILL_AVAILABLE = "Skill Points";

			// Token: 0x0400A4EF RID: 42223
			public static LocString SORT_BY_HAT = "Hat";

			// Token: 0x0400A4F0 RID: 42224
			public static LocString SELECT_HAT = "<b>SELECT HAT</b>";

			// Token: 0x0400A4F1 RID: 42225
			public static LocString POINTS_AVAILABLE = "<b>SKILL POINTS AVAILABLE</b>";

			// Token: 0x0400A4F2 RID: 42226
			public static LocString MORALE = "<b>Morale</b>";

			// Token: 0x0400A4F3 RID: 42227
			public static LocString MORALE_EXPECTATION = "<b>Morale Need</b>";

			// Token: 0x0400A4F4 RID: 42228
			public static LocString EXPERIENCE = "EXPERIENCE TO NEXT LEVEL";

			// Token: 0x0400A4F5 RID: 42229
			public static LocString EXPERIENCE_TOOLTIP = "{0}exp to next Skill Point";

			// Token: 0x0400A4F6 RID: 42230
			public static LocString NOT_AVAILABLE = "Not available";

			// Token: 0x0400A4F7 RID: 42231
			public static LocString ASSIGNED_BOOSTERS_HEADER = "{0}'s Boosters";

			// Token: 0x0400A4F8 RID: 42232
			public static LocString ASSIGNED_BOOSTERS_COUNT_LABEL = "{0}/{1} boosters assigned";

			// Token: 0x0400A4F9 RID: 42233
			public static LocString AVAILABLE_BOOSTERS_LABEL = "{0} available";

			// Token: 0x0400A4FA RID: 42234
			public static LocString ASSIGNED_BOOSTERS_LABEL = "{0} assigned";

			// Token: 0x0400A4FB RID: 42235
			public static LocString BIONIC_UPGRADE_SLOT_LOCKED = "This booster slot is unavailable\n\nBooster slots can be unlocked using " + UI.PRE_KEYWORD + "Skill Points" + UI.PST_KEYWORD;

			// Token: 0x0400A4FC RID: 42236
			public static LocString BIONIC_UPGRADE_SLOT_AVAILABLE = "No booster installed\n\nAssign a booster from the menu below or craft new boosters at the " + UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE");

			// Token: 0x0400A4FD RID: 42237
			public static LocString BIONIC_UPGRADE_SLOT_UNASSIGN = UI.CLICK(UI.ClickType.click) + " to unassign";

			// Token: 0x02002EBA RID: 11962
			public class ASSIGNMENT_REQUIREMENTS
			{
				// Token: 0x0400CA00 RID: 51712
				public static LocString EXPECTATION_TARGET_SKILL = "Current Morale: {0}\nSkill Morale Needs: {1}";

				// Token: 0x0400CA01 RID: 51713
				public static LocString EXPECTATION_ALERT_TARGET_SKILL = "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";

				// Token: 0x0400CA02 RID: 51714
				public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

				// Token: 0x02003BD5 RID: 15317
				public class SKILLGROUP_ENABLED
				{
					// Token: 0x0400EEC9 RID: 61129
					public static LocString NAME = "Can perform {0}";

					// Token: 0x0400EECA RID: 61130
					public static LocString DESCRIPTION = "Capable of performing <b>{0}</b> skills";
				}

				// Token: 0x02003BD6 RID: 15318
				public class MASTERY
				{
					// Token: 0x0400EECB RID: 61131
					public static LocString CAN_MASTER = "{0} <b>can learn</b> {1}";

					// Token: 0x0400EECC RID: 61132
					public static LocString HAS_MASTERED = "{0} has <b>already learned</b> {1}";

					// Token: 0x0400EECD RID: 61133
					public static LocString CANNOT_MASTER = "{0} <b>cannot learn</b> {1}";

					// Token: 0x0400EECE RID: 61134
					public static LocString STRESS_WARNING_MESSAGE = string.Concat(new string[]
					{
						"Learning {0} will put {1} into a ",
						UI.PRE_KEYWORD,
						"Morale",
						UI.PST_KEYWORD,
						" deficit and cause unnecessary ",
						UI.PRE_KEYWORD,
						"Stress",
						UI.PST_KEYWORD,
						"!"
					});

					// Token: 0x0400EECF RID: 61135
					public static LocString REQUIRES_MORE_SKILL_POINTS = "    • Not enough " + UI.PRE_KEYWORD + "Skill Points" + UI.PST_KEYWORD;

					// Token: 0x0400EED0 RID: 61136
					public static LocString REQUIRES_PREVIOUS_SKILLS = "    • Missing prerequisite " + UI.PRE_KEYWORD + "Skill" + UI.PST_KEYWORD;

					// Token: 0x0400EED1 RID: 61137
					public static LocString PREVENTED_BY_TRAIT = string.Concat(new string[]
					{
						"    • This Duplicant possesses the ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" Trait and cannot learn this Skill"
					});

					// Token: 0x0400EED2 RID: 61138
					public static LocString SKILL_APTITUDE = string.Concat(new string[]
					{
						"{0} is interested in {1} and will receive a ",
						UI.PRE_KEYWORD,
						"Morale",
						UI.PST_KEYWORD,
						" bonus for learning it!"
					});

					// Token: 0x0400EED3 RID: 61139
					public static LocString SKILL_GRANTED = "{0} has been granted {1} by a Trait, but does not have increased " + UI.FormatAsKeyWord("Morale Requirements") + " from learning it";
				}
			}
		}

		// Token: 0x0200251E RID: 9502
		public class KLEI_INVENTORY_SCREEN
		{
			// Token: 0x0400A4FE RID: 42238
			public static LocString OPEN_INVENTORY_BUTTON = "Open Klei Inventory";

			// Token: 0x0400A4FF RID: 42239
			public static LocString ITEM_FACADE_FOR = "This blueprint works with any {ConfigProperName}.";

			// Token: 0x0400A500 RID: 42240
			public static LocString ARTABLE_ITEM_FACADE_FOR = "This blueprint works with any {ConfigProperName} of {ArtableQuality} quality.";

			// Token: 0x0400A501 RID: 42241
			public static LocString CLOTHING_ITEM_FACADE_FOR = "This blueprint can be worn by any Duplicant.";

			// Token: 0x0400A502 RID: 42242
			public static LocString BALLOON_ARTIST_FACADE_FOR = "This blueprint can be used by any Balloon Artist.";

			// Token: 0x0400A503 RID: 42243
			public static LocString MONUMENT_PART_FACADE_FOR = "This blueprint can be used in any Great Monument.";

			// Token: 0x0400A504 RID: 42244
			public static LocString COLLECTION = "Part of {Collection} collection.";

			// Token: 0x0400A505 RID: 42245
			public static LocString COLLECTION_THE = "Part of the {Collection} collection.";

			// Token: 0x0400A506 RID: 42246
			public static LocString COLLECTION_COMING_SOON = "Part of {Collection} collection. Coming soon!";

			// Token: 0x0400A507 RID: 42247
			public static LocString ITEM_RARITY_DETAILS = "{RarityName} quality.";

			// Token: 0x0400A508 RID: 42248
			public static LocString ITEM_PLAYER_OWNED_AMOUNT = "My colony has {OwnedCount} of these blueprints.";

			// Token: 0x0400A509 RID: 42249
			public static LocString ITEM_PLAYER_OWN_NONE = "My colony doesn't have any of these yet.";

			// Token: 0x0400A50A RID: 42250
			public static LocString ITEM_PLAYER_OWNED_AMOUNT_ICON = "x{OwnedCount}";

			// Token: 0x0400A50B RID: 42251
			public static LocString ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE = "This blueprint is part of my colony's permanent collection.";

			// Token: 0x0400A50C RID: 42252
			public static LocString ITEM_DLC_REQUIRED = "This blueprint is designed for the <i>Spaced Out!</i> DLC.";

			// Token: 0x0400A50D RID: 42253
			public static LocString ITEM_UNKNOWN_NAME = "Uh oh!";

			// Token: 0x0400A50E RID: 42254
			public static LocString ITEM_UNKNOWN_DESCRIPTION = "Hmm. Looks like this blueprint is missing from the supply closet. Perhaps due to a temporal anomaly...";

			// Token: 0x0400A50F RID: 42255
			public static LocString SEARCH_PLACEHOLDER = "Search";

			// Token: 0x0400A510 RID: 42256
			public static LocString CLEAR_SEARCH_BUTTON_TOOLTIP = "Clear search";

			// Token: 0x0400A511 RID: 42257
			public static LocString TOOLTIP_VIEW_ALL_ITEMS = "Filter: Showing all items\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x0400A512 RID: 42258
			public static LocString TOOLTIP_VIEW_OWNED_ONLY = "Filter: Showing owned items only\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x0400A513 RID: 42259
			public static LocString TOOLTIP_VIEW_DOUBLES_ONLY = "Filter: Showing multiples owned only\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x0400A514 RID: 42260
			public static LocString TOOLTIP_DLC_FILTER = "{0}\n\nClick to filter displayed content by DLC Pack";

			// Token: 0x0400A515 RID: 42261
			public static LocString TOOLTIP_DLC_FILTER_ALL = "<b>Showing all content</b>";

			// Token: 0x02002EBB RID: 11963
			public static class BARTERING
			{
				// Token: 0x0400CA03 RID: 51715
				public static LocString TOOLTIP_ACTION_INVALID_OFFLINE = "Currently unavailable";

				// Token: 0x0400CA04 RID: 51716
				public static LocString BUY = "PRINT";

				// Token: 0x0400CA05 RID: 51717
				public static LocString TOOLTIP_BUY_ACTIVE = "This item requires {0} spools of Filament to print";

				// Token: 0x0400CA06 RID: 51718
				public static LocString TOOLTIP_UNBUYABLE = "This item is unprintable";

				// Token: 0x0400CA07 RID: 51719
				public static LocString TOOLTIP_UNBUYABLE_BETA = "This item may be printable after the public testing period";

				// Token: 0x0400CA08 RID: 51720
				public static LocString TOOLTIP_UNBUYABLE_ALREADY_OWNED = "My colony already owns one of these blueprints";

				// Token: 0x0400CA09 RID: 51721
				public static LocString TOOLTIP_BUY_CANT_AFFORD = "Filament supply is too low";

				// Token: 0x0400CA0A RID: 51722
				public static LocString SELL = "RECYCLE";

				// Token: 0x0400CA0B RID: 51723
				public static LocString TOOLTIP_SELL_ACTIVE = "Recycle this blueprint for {0} spools of Filament";

				// Token: 0x0400CA0C RID: 51724
				public static LocString TOOLTIP_UNSELLABLE = "This item is non-recyclable";

				// Token: 0x0400CA0D RID: 51725
				public static LocString TOOLTIP_NONE_TO_SELL = "My colony does not own any of these blueprints";

				// Token: 0x0400CA0E RID: 51726
				public static LocString CANCEL = "CANCEL";

				// Token: 0x0400CA0F RID: 51727
				public static LocString CONFIRM_RECYCLE_HEADER = "RECYCLE INTO FILAMENT?";

				// Token: 0x0400CA10 RID: 51728
				public static LocString CONFIRM_PRINT_HEADER = "PRINT ITEM?";

				// Token: 0x0400CA11 RID: 51729
				public static LocString OFFLINE_LABEL = "Not connected to Klei server";

				// Token: 0x0400CA12 RID: 51730
				public static LocString LOADING = "Connecting to server...";

				// Token: 0x0400CA13 RID: 51731
				public static LocString TRANSACTION_ERROR = "Whoops! Something's gone wrong.";

				// Token: 0x0400CA14 RID: 51732
				public static LocString ACTION_DESCRIPTION_RECYCLE = "Recycling this blueprint will recover Filament that my colony can use to print other items.\n\nOne copy of this blueprint will be removed from my colony's supply closet.";

				// Token: 0x0400CA15 RID: 51733
				public static LocString ACTION_DESCRIPTION_PRINT = "Producing this blueprint requires Filament from my colony's supply.\n\nOne copy of this blueprint will be extruded at a time.";

				// Token: 0x0400CA16 RID: 51734
				public static LocString WALLET_TOOLTIP = "{0} spool of Filament available";

				// Token: 0x0400CA17 RID: 51735
				public static LocString WALLET_PLURAL_TOOLTIP = "{0} spools of Filament available";

				// Token: 0x0400CA18 RID: 51736
				public static LocString TRANSACTION_COMPLETE_HEADER = "SUCCESS!";

				// Token: 0x0400CA19 RID: 51737
				public static LocString TRANSACTION_INCOMPLETE_HEADER = "ERROR";

				// Token: 0x0400CA1A RID: 51738
				public static LocString PURCHASE_SUCCESS = "One copy of this blueprint has been added to my colony's supply closet.";

				// Token: 0x0400CA1B RID: 51739
				public static LocString SELL_SUCCESS = "The Filament recovered from recycling this item can now be used to print other items.";
			}

			// Token: 0x02002EBC RID: 11964
			public static class CATEGORIES
			{
				// Token: 0x0400CA1C RID: 51740
				public static LocString EQUIPMENT = "Equipment";

				// Token: 0x0400CA1D RID: 51741
				public static LocString DUPE_TOPS = "Tops & Onesies";

				// Token: 0x0400CA1E RID: 51742
				public static LocString DUPE_BOTTOMS = "Bottoms";

				// Token: 0x0400CA1F RID: 51743
				public static LocString DUPE_GLOVES = "Gloves";

				// Token: 0x0400CA20 RID: 51744
				public static LocString DUPE_SHOES = "Footwear";

				// Token: 0x0400CA21 RID: 51745
				public static LocString DUPE_HATS = "Headgear";

				// Token: 0x0400CA22 RID: 51746
				public static LocString DUPE_ACCESSORIES = "Accessories";

				// Token: 0x0400CA23 RID: 51747
				public static LocString ATMO_SUIT_HELMET = "Atmo Helmets";

				// Token: 0x0400CA24 RID: 51748
				public static LocString ATMO_SUIT_BODY = "Atmo Suits";

				// Token: 0x0400CA25 RID: 51749
				public static LocString ATMO_SUIT_GLOVES = "Atmo Gloves";

				// Token: 0x0400CA26 RID: 51750
				public static LocString ATMO_SUIT_BELT = "Atmo Belts";

				// Token: 0x0400CA27 RID: 51751
				public static LocString ATMO_SUIT_SHOES = "Atmo Boots";

				// Token: 0x0400CA28 RID: 51752
				public static LocString PRIMOGARB = "Primo Garb";

				// Token: 0x0400CA29 RID: 51753
				public static LocString ATMOSUITS = "Atmo Suits";

				// Token: 0x0400CA2A RID: 51754
				public static LocString JET_SUIT_BODY = "Jet Suits";

				// Token: 0x0400CA2B RID: 51755
				public static LocString JET_SUIT_HELMET = "Jet Helmets";

				// Token: 0x0400CA2C RID: 51756
				public static LocString JET_SUIT_GLOVES = "Jet Gloves";

				// Token: 0x0400CA2D RID: 51757
				public static LocString JET_SUIT_SHOES = "Jet Boots";

				// Token: 0x0400CA2E RID: 51758
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400CA2F RID: 51759
				public static LocString CRITTERS = "Critters";

				// Token: 0x0400CA30 RID: 51760
				public static LocString SWEEPYS = "Sweepys";

				// Token: 0x0400CA31 RID: 51761
				public static LocString DUPLICANTS = "Duplicants";

				// Token: 0x0400CA32 RID: 51762
				public static LocString ARTWORKS = "Artwork";

				// Token: 0x0400CA33 RID: 51763
				public static LocString JOY_RESPONSE = "Overjoyed Responses";

				// Token: 0x02003BD7 RID: 15319
				public static class JOY_RESPONSES
				{
					// Token: 0x0400EED4 RID: 61140
					public static LocString BALLOON_ARTIST = "Balloon Artist";
				}
			}

			// Token: 0x02002EBD RID: 11965
			public static class TOP_LEVEL_CATEGORIES
			{
				// Token: 0x0400CA34 RID: 51764
				public static LocString UNRELEASED = "DEBUG UNRELEASED";

				// Token: 0x0400CA35 RID: 51765
				public static LocString CLOTHING_TOPS = "Tops & Onesies";

				// Token: 0x0400CA36 RID: 51766
				public static LocString CLOTHING_BOTTOMS = "Bottoms";

				// Token: 0x0400CA37 RID: 51767
				public static LocString CLOTHING_GLOVES = "Gloves";

				// Token: 0x0400CA38 RID: 51768
				public static LocString CLOTHING_SHOES = "Footwear";

				// Token: 0x0400CA39 RID: 51769
				public static LocString ATMOSUITS = "Atmo Suits";

				// Token: 0x0400CA3A RID: 51770
				public static LocString JETSUITS = "Jet Suits";

				// Token: 0x0400CA3B RID: 51771
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400CA3C RID: 51772
				public static LocString WALLPAPERS = "Wallpapers";

				// Token: 0x0400CA3D RID: 51773
				public static LocString ARTWORK = "Artwork";

				// Token: 0x0400CA3E RID: 51774
				public static LocString JOY_RESPONSES = "Joy Responses";
			}

			// Token: 0x02002EBE RID: 11966
			public static class SUBCATEGORIES
			{
				// Token: 0x0400CA3F RID: 51775
				public static LocString UNRELEASED = "DEBUG UNRELEASED";

				// Token: 0x0400CA40 RID: 51776
				public static LocString UNCATEGORIZED = "BUG: UNCATEGORIZED";

				// Token: 0x0400CA41 RID: 51777
				public static LocString YAML = "YAML";

				// Token: 0x0400CA42 RID: 51778
				public static LocString DEFAULT = "Default";

				// Token: 0x0400CA43 RID: 51779
				public static LocString JOY_BALLOON = "Balloons";

				// Token: 0x0400CA44 RID: 51780
				public static LocString JOY_STICKER = "Stickers";

				// Token: 0x0400CA45 RID: 51781
				public static LocString PRIMO_GARB = "Primo Garb";

				// Token: 0x0400CA46 RID: 51782
				public static LocString CLOTHING_TOPS_BASIC = "Standard Shirts";

				// Token: 0x0400CA47 RID: 51783
				public static LocString CLOTHING_TOPS_TSHIRT = "Tees";

				// Token: 0x0400CA48 RID: 51784
				public static LocString CLOTHING_TOPS_FANCY = "Specialty Tops";

				// Token: 0x0400CA49 RID: 51785
				public static LocString CLOTHING_TOPS_JACKET = "Jackets";

				// Token: 0x0400CA4A RID: 51786
				public static LocString CLOTHING_TOPS_UNDERSHIRT = "Undershirts";

				// Token: 0x0400CA4B RID: 51787
				public static LocString CLOTHING_TOPS_DRESS = "Dresses and Onesies";

				// Token: 0x0400CA4C RID: 51788
				public static LocString CLOTHING_BOTTOMS_BASIC = "Standard Pants";

				// Token: 0x0400CA4D RID: 51789
				public static LocString CLOTHING_BOTTOMS_FANCY = "Fancy Pants";

				// Token: 0x0400CA4E RID: 51790
				public static LocString CLOTHING_BOTTOMS_SHORTS = "Shorts";

				// Token: 0x0400CA4F RID: 51791
				public static LocString CLOTHING_BOTTOMS_SKIRTS = "Skirts";

				// Token: 0x0400CA50 RID: 51792
				public static LocString CLOTHING_BOTTOMS_UNDERWEAR = "Underwear";

				// Token: 0x0400CA51 RID: 51793
				public static LocString CLOTHING_GLOVES_BASIC = "Standard Gloves";

				// Token: 0x0400CA52 RID: 51794
				public static LocString CLOTHING_GLOVES_FORMAL = "Fancy Gloves";

				// Token: 0x0400CA53 RID: 51795
				public static LocString CLOTHING_GLOVES_SHORT = "Short Gloves";

				// Token: 0x0400CA54 RID: 51796
				public static LocString CLOTHING_GLOVES_PRINTS = "Specialty Gloves";

				// Token: 0x0400CA55 RID: 51797
				public static LocString CLOTHING_SHOES_BASIC = "Standard Shoes";

				// Token: 0x0400CA56 RID: 51798
				public static LocString CLOTHING_SHOE_SOCKS = "Socks";

				// Token: 0x0400CA57 RID: 51799
				public static LocString CLOTHING_SHOES_FANCY = "Fancy Shoes";

				// Token: 0x0400CA58 RID: 51800
				public static LocString ATMOSUIT_HELMETS_BASIC = "Atmo Helmets";

				// Token: 0x0400CA59 RID: 51801
				public static LocString ATMOSUIT_HELMETS_FANCY = "Fancy Atmo Helmets";

				// Token: 0x0400CA5A RID: 51802
				public static LocString ATMOSUIT_BODIES_BASIC = "Atmo Suits";

				// Token: 0x0400CA5B RID: 51803
				public static LocString ATMOSUIT_BODIES_FANCY = "Fancy Atmo Suits";

				// Token: 0x0400CA5C RID: 51804
				public static LocString ATMOSUIT_GLOVES_BASIC = "Atmo Gloves";

				// Token: 0x0400CA5D RID: 51805
				public static LocString ATMOSUIT_GLOVES_FANCY = "Fancy Atmo Gloves";

				// Token: 0x0400CA5E RID: 51806
				public static LocString ATMOSUIT_BELTS_BASIC = "Atmo Belts";

				// Token: 0x0400CA5F RID: 51807
				public static LocString ATMOSUIT_BELTS_FANCY = "Fancy Atmo Belts";

				// Token: 0x0400CA60 RID: 51808
				public static LocString ATMOSUIT_SHOES_BASIC = "Atmo Boots";

				// Token: 0x0400CA61 RID: 51809
				public static LocString ATMOSUIT_SHOES_FANCY = "Fancy Atmo Boots";

				// Token: 0x0400CA62 RID: 51810
				public static LocString JETSUIT_HELMETS_BASIC = "Jet Helmets";

				// Token: 0x0400CA63 RID: 51811
				public static LocString JETSUIT_BODIES_BASIC = "Jet Suits";

				// Token: 0x0400CA64 RID: 51812
				public static LocString JETSUIT_GLOVES_BASIC = "Jet Gloves";

				// Token: 0x0400CA65 RID: 51813
				public static LocString JETSUIT_SHOES_BASIC = "Jet Boots";

				// Token: 0x0400CA66 RID: 51814
				public static LocString BUILDING_WALLPAPER_BASIC = "Solid Wallpapers";

				// Token: 0x0400CA67 RID: 51815
				public static LocString BUILDING_WALLPAPER_FANCY = "Geometric Wallpapers";

				// Token: 0x0400CA68 RID: 51816
				public static LocString BUILDING_WALLPAPER_PRINTS = "Patterned Wallpapers";

				// Token: 0x0400CA69 RID: 51817
				public static LocString BUILDING_CANVAS_STANDARD = "Standard Canvas";

				// Token: 0x0400CA6A RID: 51818
				public static LocString BUILDING_CANVAS_PORTRAIT = "Portrait Canvas";

				// Token: 0x0400CA6B RID: 51819
				public static LocString BUILDING_CANVAS_LANDSCAPE = "Landscape Canvas";

				// Token: 0x0400CA6C RID: 51820
				public static LocString BUILDING_SCULPTURE = "Sculptures";

				// Token: 0x0400CA6D RID: 51821
				public static LocString MONUMENT_BOTTOM = "Monument Base";

				// Token: 0x0400CA6E RID: 51822
				public static LocString MONUMENT_MIDDLE = "Monument Midsection";

				// Token: 0x0400CA6F RID: 51823
				public static LocString MONUMENT_TOP = "Monument Top";

				// Token: 0x0400CA70 RID: 51824
				public static LocString BUILDINGS_FLOWER_VASE = "Pots and Planters";

				// Token: 0x0400CA71 RID: 51825
				public static LocString BUILDINGS_BED_COT = "Cots";

				// Token: 0x0400CA72 RID: 51826
				public static LocString BUILDINGS_BED_LUXURY = "Comfy Beds";

				// Token: 0x0400CA73 RID: 51827
				public static LocString BUILDING_CEILING_LIGHT = "Lights";

				// Token: 0x0400CA74 RID: 51828
				public static LocString BUILDINGS_STORAGE = "Storage";

				// Token: 0x0400CA75 RID: 51829
				public static LocString BUILDINGS_INDUSTRIAL = "Industrial";

				// Token: 0x0400CA76 RID: 51830
				public static LocString BUILDINGS_FOOD = "Cooking";

				// Token: 0x0400CA77 RID: 51831
				public static LocString BUILDINGS_WASHROOM = "Sanitation";

				// Token: 0x0400CA78 RID: 51832
				public static LocString BUILDINGS_RANCHING = "Agricultural";

				// Token: 0x0400CA79 RID: 51833
				public static LocString BUILDINGS_RECREATION = "Recreation and Decor";

				// Token: 0x0400CA7A RID: 51834
				public static LocString BUILDINGS_PRINTING_POD = "Printing Pods";

				// Token: 0x0400CA7B RID: 51835
				public static LocString BUILDINGS_ELECTIC_WIRES = "Electrical";

				// Token: 0x0400CA7C RID: 51836
				public static LocString BUILDINGS_AUTOMATION = "Automation";

				// Token: 0x0400CA7D RID: 51837
				public static LocString BUILDINGS_RESEARCH = "Research";
			}

			// Token: 0x02002EBF RID: 11967
			public static class COLUMN_HEADERS
			{
				// Token: 0x0400CA7E RID: 51838
				public static LocString CATEGORY_HEADER = "BLUEPRINTS";

				// Token: 0x0400CA7F RID: 51839
				public static LocString ITEMS_HEADER = "Items";

				// Token: 0x0400CA80 RID: 51840
				public static LocString DETAILS_HEADER = "Details";
			}
		}

		// Token: 0x0200251F RID: 9503
		public class ITEM_DROP_SCREEN
		{
			// Token: 0x0400A516 RID: 42262
			public static LocString THANKS_FOR_PLAYING = "New blueprints unlocked!";

			// Token: 0x0400A517 RID: 42263
			public static LocString WEB_REWARDS_AVAILABLE = "Rewards available online!";

			// Token: 0x0400A518 RID: 42264
			public static LocString NOTHING_AVAILABLE = "All available blueprints claimed";

			// Token: 0x0400A519 RID: 42265
			public static LocString OPEN_URL_BUTTON = "CLAIM";

			// Token: 0x0400A51A RID: 42266
			public static LocString PRINT_ITEM_BUTTON = "PRINT";

			// Token: 0x0400A51B RID: 42267
			public static LocString DISMISS_BUTTON = "DISMISS";

			// Token: 0x0400A51C RID: 42268
			public static LocString ERROR_CANNOTLOADITEM = "Whoops! Something's gone wrong.";

			// Token: 0x0400A51D RID: 42269
			public static LocString UNOPENED_ITEM_COUNT = "{0} unclaimed blueprints";

			// Token: 0x0400A51E RID: 42270
			public static LocString UNOPENED_ITEM = "{0} unclaimed blueprint";

			// Token: 0x02002EC0 RID: 11968
			public static class IN_GAME_BUTTON
			{
				// Token: 0x0400CA81 RID: 51841
				public static LocString TOOLTIP_ITEMS_AVAILABLE = "Unlock new blueprints";

				// Token: 0x0400CA82 RID: 51842
				public static LocString TOOLTIP_ERROR_NO_ITEMS = "No new blueprints to unlock";
			}
		}

		// Token: 0x02002520 RID: 9504
		public class OUTFIT_BROWSER_SCREEN
		{
			// Token: 0x0400A51F RID: 42271
			public static LocString BUTTON_ADD_OUTFIT = "New Outfit";

			// Token: 0x0400A520 RID: 42272
			public static LocString BUTTON_PICK_OUTFIT = "Assign Outfit";

			// Token: 0x0400A521 RID: 42273
			public static LocString TOOLTIP_PICK_OUTFIT_ERROR_LOCKED = "Cannot assign this outfit to {MinionName} because my colony doesn't have all of these blueprints yet";

			// Token: 0x0400A522 RID: 42274
			public static LocString BUTTON_EDIT_OUTFIT = "Restyle Outfit";

			// Token: 0x0400A523 RID: 42275
			public static LocString BUTTON_COPY_OUTFIT = "Copy Outfit";

			// Token: 0x0400A524 RID: 42276
			public static LocString TOOLTIP_DELETE_OUTFIT = "Delete Outfit";

			// Token: 0x0400A525 RID: 42277
			public static LocString TOOLTIP_DELETE_OUTFIT_ERROR_READONLY = "This outfit cannot be deleted";

			// Token: 0x0400A526 RID: 42278
			public static LocString TOOLTIP_RENAME_OUTFIT = "Rename Outfit";

			// Token: 0x0400A527 RID: 42279
			public static LocString TOOLTIP_RENAME_OUTFIT_ERROR_READONLY = "This outfit cannot be renamed";

			// Token: 0x0400A528 RID: 42280
			public static LocString TOOLTIP_FILTER_BY_CLOTHING = "View my Clothing Outfits";

			// Token: 0x0400A529 RID: 42281
			public static LocString TOOLTIP_FILTER_BY_ATMO_SUITS = "View my Atmo Suit Outfits";

			// Token: 0x0400A52A RID: 42282
			public static LocString TOOLTIP_FILTER_BY_JET_SUITS = "View my Jet Suit Outfits";

			// Token: 0x02002EC1 RID: 11969
			public static class COLUMN_HEADERS
			{
				// Token: 0x0400CA83 RID: 51843
				public static LocString GALLERY_HEADER = "OUTFITS";

				// Token: 0x0400CA84 RID: 51844
				public static LocString MINION_GALLERY_HEADER = "WARDROBE";

				// Token: 0x0400CA85 RID: 51845
				public static LocString DETAILS_HEADER = "Preview";
			}

			// Token: 0x02002EC2 RID: 11970
			public class DELETE_WARNING_POPUP
			{
				// Token: 0x0400CA86 RID: 51846
				public static LocString HEADER = "Delete \"{OutfitName}\"?";

				// Token: 0x0400CA87 RID: 51847
				public static LocString BODY = "Are you sure you want to delete \"{OutfitName}\"?\n\nAny Duplicants assigned to wear this outfit on spawn will be printed wearing their default outfit instead. Existing Duplicants in saved games won't be affected.\n\nThis <b>cannot</b> be undone.";

				// Token: 0x0400CA88 RID: 51848
				public static LocString BUTTON_YES_DELETE = "Yes, delete outfit";

				// Token: 0x0400CA89 RID: 51849
				public static LocString BUTTON_DONT_DELETE = "Cancel";
			}

			// Token: 0x02002EC3 RID: 11971
			public class RENAME_POPUP
			{
				// Token: 0x0400CA8A RID: 51850
				public static LocString HEADER = "RENAME OUTFIT";
			}
		}

		// Token: 0x02002521 RID: 9505
		public class LOCKER_MENU
		{
			// Token: 0x0400A52B RID: 42283
			public static LocString TITLE = "SUPPLY CLOSET";

			// Token: 0x0400A52C RID: 42284
			public static LocString BUTTON_INVENTORY = "All";

			// Token: 0x0400A52D RID: 42285
			public static LocString BUTTON_INVENTORY_DESCRIPTION = "View all of my colony's blueprints";

			// Token: 0x0400A52E RID: 42286
			public static LocString BUTTON_DUPLICANTS = "Duplicants";

			// Token: 0x0400A52F RID: 42287
			public static LocString BUTTON_DUPLICANTS_DESCRIPTION = "Manage individual Duplicants' outfits";

			// Token: 0x0400A530 RID: 42288
			public static LocString BUTTON_OUTFITS = "Wardrobe";

			// Token: 0x0400A531 RID: 42289
			public static LocString BUTTON_OUTFITS_DESCRIPTION = "Manage my colony's collection of outfits";

			// Token: 0x0400A532 RID: 42290
			public static LocString DEFAULT_DESCRIPTION = "Select a screen";

			// Token: 0x0400A533 RID: 42291
			public static LocString BUTTON_CLAIM = "Claim Blueprints";

			// Token: 0x0400A534 RID: 42292
			public static LocString BUTTON_CLAIM_DESCRIPTION = "Claim any available blueprints";

			// Token: 0x0400A535 RID: 42293
			public static LocString BUTTON_CLAIM_NONE_DESCRIPTION = "All available blueprints claimed";

			// Token: 0x0400A536 RID: 42294
			public static LocString UNOPENED_ITEMS_TOOLTIP = "New blueprints available";

			// Token: 0x0400A537 RID: 42295
			public static LocString UNOPENED_ITEMS_NONE_TOOLTIP = "All available blueprints claimed";

			// Token: 0x0400A538 RID: 42296
			public static LocString OFFLINE_ICON_TOOLTIP = "Not connected to Klei server";

			// Token: 0x0400A539 RID: 42297
			public static LocString OFFLINE_ICON_TOOLTIP_DATA_COLLECTIONS = "Not connected to Klei server\n\nDisabled by privacy settings in Data Communication";
		}

		// Token: 0x02002522 RID: 9506
		public class LOCKER_NAVIGATOR
		{
			// Token: 0x0400A53A RID: 42298
			public static LocString BUTTON_BACK = "BACK";

			// Token: 0x0400A53B RID: 42299
			public static LocString BUTTON_CLOSE = "CLOSE";

			// Token: 0x02002EC4 RID: 11972
			public class DATA_COLLECTION_WARNING_POPUP
			{
				// Token: 0x0400CA8B RID: 51851
				public static LocString HEADER = "Data Communication is Disabled";

				// Token: 0x0400CA8C RID: 51852
				public static LocString BODY = "Data Communication must be enabled in order to access newly unlocked items. This setting can be found in the Options menu.\n\nExisting item unlocks can still be used while Data Communication is disabled.";

				// Token: 0x0400CA8D RID: 51853
				public static LocString BUTTON_OK = "Continue";

				// Token: 0x0400CA8E RID: 51854
				public static LocString BUTTON_OPEN_SETTINGS = "Options Menu";
			}
		}

		// Token: 0x02002523 RID: 9507
		public class JOY_RESPONSE_DESIGNER_SCREEN
		{
			// Token: 0x0400A53C RID: 42300
			public static LocString CATEGORY_HEADER = "OVERJOYED RESPONSES";

			// Token: 0x0400A53D RID: 42301
			public static LocString BUTTON_APPLY_TO_MINION = "Assign to {MinionName}";

			// Token: 0x0400A53E RID: 42302
			public static LocString TOOLTIP_NO_FACADES_FOR_JOY_TRAIT = "There aren't any blueprints for {JoyResponseType} Duplicants yet";

			// Token: 0x0400A53F RID: 42303
			public static LocString TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED = "This Overjoyed Response blueprint cannot be assigned because my colony doesn't own it yet";

			// Token: 0x02002EC5 RID: 11973
			public class CHANGES_NOT_SAVED_WARNING_POPUP
			{
				// Token: 0x0400CA8F RID: 51855
				public static LocString HEADER = "Discard changes to {MinionName}'s Overjoyed Response?";
			}
		}

		// Token: 0x02002524 RID: 9508
		public class OUTFIT_DESIGNER_SCREEN
		{
			// Token: 0x0400A540 RID: 42304
			public static LocString CATEGORY_HEADER = "CLOTHING";

			// Token: 0x02002EC6 RID: 11974
			public class MINION_INSTANCE
			{
				// Token: 0x0400CA90 RID: 51856
				public static LocString BUTTON_APPLY_TO_MINION = "Assign to {MinionName}";

				// Token: 0x0400CA91 RID: 51857
				public static LocString BUTTON_APPLY_TO_TEMPLATE = "Apply to Template";

				// Token: 0x02003BD8 RID: 15320
				public class APPLY_TEMPLATE_POPUP
				{
					// Token: 0x0400EED5 RID: 61141
					public static LocString HEADER = "SAVE AS TEMPLATE";

					// Token: 0x0400EED6 RID: 61142
					public static LocString DESC_SAVE_EXISTING = "\"{OutfitName}\" will be updated and applied to {MinionName} on save.";

					// Token: 0x0400EED7 RID: 61143
					public static LocString DESC_SAVE_NEW = "A new outfit named \"{OutfitName}\" will be created and assigned to {MinionName} on save.";

					// Token: 0x0400EED8 RID: 61144
					public static LocString BUTTON_SAVE_EXISTING = "Update Outfit";

					// Token: 0x0400EED9 RID: 61145
					public static LocString BUTTON_SAVE_NEW = "Save New Outfit";
				}
			}

			// Token: 0x02002EC7 RID: 11975
			public class OUTFIT_TEMPLATE
			{
				// Token: 0x0400CA92 RID: 51858
				public static LocString BUTTON_SAVE = "Save Template";

				// Token: 0x0400CA93 RID: 51859
				public static LocString BUTTON_COPY = "Save a Copy";

				// Token: 0x0400CA94 RID: 51860
				public static LocString TOOLTIP_SAVE_ERROR_LOCKED = "Cannot save this outfit because my colony doesn't own all of its blueprints yet";

				// Token: 0x0400CA95 RID: 51861
				public static LocString TOOLTIP_SAVE_ERROR_READONLY = "This wardrobe staple cannot be altered\n\nMake a copy to save your changes";
			}

			// Token: 0x02002EC8 RID: 11976
			public class CHANGES_NOT_SAVED_WARNING_POPUP
			{
				// Token: 0x0400CA96 RID: 51862
				public static LocString HEADER = "Discard changes to \"{OutfitName}\"?";

				// Token: 0x0400CA97 RID: 51863
				public static LocString BODY = "There are unsaved changes which will be lost if you exit now.\n\nAre you sure you want to discard your changes?";

				// Token: 0x0400CA98 RID: 51864
				public static LocString BUTTON_DISCARD = "Yes, discard changes";

				// Token: 0x0400CA99 RID: 51865
				public static LocString BUTTON_RETURN = "Cancel";
			}

			// Token: 0x02002EC9 RID: 11977
			public class COPY_POPUP
			{
				// Token: 0x0400CA9A RID: 51866
				public static LocString HEADER = "RENAME COPY";
			}
		}

		// Token: 0x02002525 RID: 9509
		public class OUTFIT_NAME
		{
			// Token: 0x0400A541 RID: 42305
			public static LocString NEW = "Custom Outfit";

			// Token: 0x0400A542 RID: 42306
			public static LocString COPY_OF = "Copy of {OutfitName}";

			// Token: 0x0400A543 RID: 42307
			public static LocString RESOLVE_CONFLICT = "{OutfitName} ({ConflictNumber})";

			// Token: 0x0400A544 RID: 42308
			public static LocString ERROR_NAME_EXISTS = "There's already an outfit named \"{OutfitName}\"";

			// Token: 0x0400A545 RID: 42309
			public static LocString MINIONS_OUTFIT = "{MinionName}'s Current Outfit";

			// Token: 0x0400A546 RID: 42310
			public static LocString NONE = "Default Outfit";

			// Token: 0x0400A547 RID: 42311
			public static LocString NONE_JOY_RESPONSE = "Default Overjoyed Response";

			// Token: 0x0400A548 RID: 42312
			public static LocString NONE_ATMO_SUIT = "Default Atmo Suit";

			// Token: 0x0400A549 RID: 42313
			public static LocString NONE_JET_SUIT = "Default Jet Suit";
		}

		// Token: 0x02002526 RID: 9510
		public class OUTFIT_DESCRIPTION
		{
			// Token: 0x0400A54A RID: 42314
			public static LocString CONTAINS_NON_OWNED_ITEMS = "This outfit can only be worn once my colony has access to all of its blueprints.";

			// Token: 0x0400A54B RID: 42315
			public static LocString NO_JOY_RESPONSE_NAME = "Default Overjoyed Response";

			// Token: 0x0400A54C RID: 42316
			public static LocString NO_JOY_RESPONSE_DESC = "Default response to an overjoyed state.";
		}

		// Token: 0x02002527 RID: 9511
		public class MINION_BROWSER_SCREEN
		{
			// Token: 0x0400A54D RID: 42317
			public static LocString CATEGORY_HEADER = "DUPLICANTS";

			// Token: 0x0400A54E RID: 42318
			public static LocString BUTTON_CHANGE_OUTFIT = "Open Wardrobe";

			// Token: 0x0400A54F RID: 42319
			public static LocString BUTTON_EDIT_OUTFIT_ITEMS = "Restyle Outfit";

			// Token: 0x0400A550 RID: 42320
			public static LocString BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS = "Restyle Atmo Suit";

			// Token: 0x0400A551 RID: 42321
			public static LocString BUTTON_EDIT_JET_SUIT_OUTFIT_ITEMS = "Restyle Jet Suit";

			// Token: 0x0400A552 RID: 42322
			public static LocString BUTTON_EDIT_JOY_RESPONSE = "Restyle Overjoyed Response";

			// Token: 0x0400A553 RID: 42323
			public static LocString OUTFIT_TYPE_CLOTHING = "CLOTHING";

			// Token: 0x0400A554 RID: 42324
			public static LocString OUTFIT_TYPE_JOY_RESPONSE = "OVERJOYED RESPONSE";

			// Token: 0x0400A555 RID: 42325
			public static LocString OUTFIT_TYPE_ATMOSUIT = "ATMO SUIT";

			// Token: 0x0400A556 RID: 42326
			public static LocString OUTFIT_TYPE_JETSUIT = "JET SUIT";

			// Token: 0x0400A557 RID: 42327
			public static LocString TOOLTIP_FROM_DLC = "This Duplicant is part of {0} DLC";

			// Token: 0x0400A558 RID: 42328
			public static LocString TOOLTIP_CYCLE_PREVIOUS_OUTFIT_TYPE = "Previous Restylable\n\nClick to restyle this Duplicant's other outfit types and Overjoyed response";

			// Token: 0x0400A559 RID: 42329
			public static LocString TOOLTIP_CYCLE_NEXT_OUTFIT_TYPE = "Next Restylable\n\nClick to restyle this Duplicant's other outfit types and Overjoyed response";
		}

		// Token: 0x02002528 RID: 9512
		public class PERMIT_RARITY
		{
			// Token: 0x0400A55A RID: 42330
			public static readonly LocString UNKNOWN = "Unknown";

			// Token: 0x0400A55B RID: 42331
			public static readonly LocString UNIVERSAL = "Universal";

			// Token: 0x0400A55C RID: 42332
			public static readonly LocString LOYALTY = "<color=#FFB037>Loyalty</color>";

			// Token: 0x0400A55D RID: 42333
			public static readonly LocString COMMON = "<color=#97B2B9>Common</color>";

			// Token: 0x0400A55E RID: 42334
			public static readonly LocString DECENT = "<color=#81EBDE>Decent</color>";

			// Token: 0x0400A55F RID: 42335
			public static readonly LocString NIFTY = "<color=#71E379>Nifty</color>";

			// Token: 0x0400A560 RID: 42336
			public static readonly LocString SPLENDID = "<color=#FF6DE7>Splendid</color>";
		}

		// Token: 0x02002529 RID: 9513
		public class OUTFITS
		{
			// Token: 0x02002ECA RID: 11978
			public class STANDARD_YELLOW
			{
				// Token: 0x0400CA9B RID: 51867
				public static LocString NAME = "Standard Yellow Uniform";
			}

			// Token: 0x02002ECB RID: 11979
			public class STANDARD_RED
			{
				// Token: 0x0400CA9C RID: 51868
				public static LocString NAME = "Standard Red Uniform";
			}

			// Token: 0x02002ECC RID: 11980
			public class STANDARD_GREEN
			{
				// Token: 0x0400CA9D RID: 51869
				public static LocString NAME = "Standard Green Uniform";
			}

			// Token: 0x02002ECD RID: 11981
			public class STANDARD_BLUE
			{
				// Token: 0x0400CA9E RID: 51870
				public static LocString NAME = "Standard Blue Uniform";
			}

			// Token: 0x02002ECE RID: 11982
			public class BASIC_BLACK
			{
				// Token: 0x0400CA9F RID: 51871
				public static LocString NAME = "Basic Black Outfit";
			}

			// Token: 0x02002ECF RID: 11983
			public class BASIC_WHITE
			{
				// Token: 0x0400CAA0 RID: 51872
				public static LocString NAME = "Basic White Outfit";
			}

			// Token: 0x02002ED0 RID: 11984
			public class BASIC_RED
			{
				// Token: 0x0400CAA1 RID: 51873
				public static LocString NAME = "Basic Red Outfit";
			}

			// Token: 0x02002ED1 RID: 11985
			public class BASIC_ORANGE
			{
				// Token: 0x0400CAA2 RID: 51874
				public static LocString NAME = "Basic Orange Outfit";
			}

			// Token: 0x02002ED2 RID: 11986
			public class BASIC_YELLOW
			{
				// Token: 0x0400CAA3 RID: 51875
				public static LocString NAME = "Basic Yellow Outfit";
			}

			// Token: 0x02002ED3 RID: 11987
			public class BASIC_GREEN
			{
				// Token: 0x0400CAA4 RID: 51876
				public static LocString NAME = "Basic Green Outfit";
			}

			// Token: 0x02002ED4 RID: 11988
			public class BASIC_AQUA
			{
				// Token: 0x0400CAA5 RID: 51877
				public static LocString NAME = "Basic Aqua Outfit";
			}

			// Token: 0x02002ED5 RID: 11989
			public class BASIC_PURPLE
			{
				// Token: 0x0400CAA6 RID: 51878
				public static LocString NAME = "Basic Purple Outfit";
			}

			// Token: 0x02002ED6 RID: 11990
			public class BASIC_PINK_ORCHID
			{
				// Token: 0x0400CAA7 RID: 51879
				public static LocString NAME = "Basic Bubblegum Outfit";
			}

			// Token: 0x02002ED7 RID: 11991
			public class BASIC_DEEPRED
			{
				// Token: 0x0400CAA8 RID: 51880
				public static LocString NAME = "Team Captain Outfit";
			}

			// Token: 0x02002ED8 RID: 11992
			public class BASIC_BLUE_COBALT
			{
				// Token: 0x0400CAA9 RID: 51881
				public static LocString NAME = "True Blue Outfit";
			}

			// Token: 0x02002ED9 RID: 11993
			public class BASIC_PINK_FLAMINGO
			{
				// Token: 0x0400CAAA RID: 51882
				public static LocString NAME = "Pep Rally Outfit";
			}

			// Token: 0x02002EDA RID: 11994
			public class BASIC_GREEN_KELLY
			{
				// Token: 0x0400CAAB RID: 51883
				public static LocString NAME = "Go Team! Outfit";
			}

			// Token: 0x02002EDB RID: 11995
			public class BASIC_GREY_CHARCOAL
			{
				// Token: 0x0400CAAC RID: 51884
				public static LocString NAME = "Underdog Outfit";
			}

			// Token: 0x02002EDC RID: 11996
			public class BASIC_LEMON
			{
				// Token: 0x0400CAAD RID: 51885
				public static LocString NAME = "Team Hype Outfit";
			}

			// Token: 0x02002EDD RID: 11997
			public class BASIC_SATSUMA
			{
				// Token: 0x0400CAAE RID: 51886
				public static LocString NAME = "Superfan Outfit";
			}

			// Token: 0x02002EDE RID: 11998
			public class JELLYPUFF_BLUEBERRY
			{
				// Token: 0x0400CAAF RID: 51887
				public static LocString NAME = "Blueberry Jelly Outfit";
			}

			// Token: 0x02002EDF RID: 11999
			public class JELLYPUFF_GRAPE
			{
				// Token: 0x0400CAB0 RID: 51888
				public static LocString NAME = "Grape Jelly Outfit";
			}

			// Token: 0x02002EE0 RID: 12000
			public class JELLYPUFF_LEMON
			{
				// Token: 0x0400CAB1 RID: 51889
				public static LocString NAME = "Lemon Jelly Outfit";
			}

			// Token: 0x02002EE1 RID: 12001
			public class JELLYPUFF_LIME
			{
				// Token: 0x0400CAB2 RID: 51890
				public static LocString NAME = "Lime Jelly Outfit";
			}

			// Token: 0x02002EE2 RID: 12002
			public class JELLYPUFF_SATSUMA
			{
				// Token: 0x0400CAB3 RID: 51891
				public static LocString NAME = "Satsuma Jelly Outfit";
			}

			// Token: 0x02002EE3 RID: 12003
			public class JELLYPUFF_STRAWBERRY
			{
				// Token: 0x0400CAB4 RID: 51892
				public static LocString NAME = "Strawberry Jelly Outfit";
			}

			// Token: 0x02002EE4 RID: 12004
			public class JELLYPUFF_WATERMELON
			{
				// Token: 0x0400CAB5 RID: 51893
				public static LocString NAME = "Watermelon Jelly Outfit";
			}

			// Token: 0x02002EE5 RID: 12005
			public class ATHLETE
			{
				// Token: 0x0400CAB6 RID: 51894
				public static LocString NAME = "Racing Outfit";
			}

			// Token: 0x02002EE6 RID: 12006
			public class CIRCUIT
			{
				// Token: 0x0400CAB7 RID: 51895
				public static LocString NAME = "LED Party Outfit";
			}

			// Token: 0x02002EE7 RID: 12007
			public class ATMOSUIT_LIMONE
			{
				// Token: 0x0400CAB8 RID: 51896
				public static LocString NAME = "Citrus Atmo Outfit";
			}

			// Token: 0x02002EE8 RID: 12008
			public class ATMOSUIT_SPARKLE_RED
			{
				// Token: 0x0400CAB9 RID: 51897
				public static LocString NAME = "Red Glitter Atmo Outfit";
			}

			// Token: 0x02002EE9 RID: 12009
			public class ATMOSUIT_SPARKLE_BLUE
			{
				// Token: 0x0400CABA RID: 51898
				public static LocString NAME = "Blue Glitter Atmo Outfit";
			}

			// Token: 0x02002EEA RID: 12010
			public class ATMOSUIT_SPARKLE_GREEN
			{
				// Token: 0x0400CABB RID: 51899
				public static LocString NAME = "Green Glitter Atmo Outfit";
			}

			// Token: 0x02002EEB RID: 12011
			public class ATMOSUIT_SPARKLE_LAVENDER
			{
				// Token: 0x0400CABC RID: 51900
				public static LocString NAME = "Violet Glitter Atmo Outfit";
			}

			// Token: 0x02002EEC RID: 12012
			public class ATMOSUIT_PUFT
			{
				// Token: 0x0400CABD RID: 51901
				public static LocString NAME = "Puft Atmo Outfit";
			}

			// Token: 0x02002EED RID: 12013
			public class ATMOSUIT_CONFETTI
			{
				// Token: 0x0400CABE RID: 51902
				public static LocString NAME = "Confetti Atmo Outfit";
			}

			// Token: 0x02002EEE RID: 12014
			public class ATMOSUIT_BASIC_PURPLE
			{
				// Token: 0x0400CABF RID: 51903
				public static LocString NAME = "Eggplant Atmo Outfit";
			}

			// Token: 0x02002EEF RID: 12015
			public class ATMOSUIT_PINK_PURPLE
			{
				// Token: 0x0400CAC0 RID: 51904
				public static LocString NAME = "Pink Punch Atmo Outfit";
			}

			// Token: 0x02002EF0 RID: 12016
			public class ATMOSUIT_RED_GREY
			{
				// Token: 0x0400CAC1 RID: 51905
				public static LocString NAME = "Blastoff Atmo Outfit";
			}

			// Token: 0x02002EF1 RID: 12017
			public class CANUXTUX
			{
				// Token: 0x0400CAC2 RID: 51906
				public static LocString NAME = "Canadian Tuxedo Outfit";
			}

			// Token: 0x02002EF2 RID: 12018
			public class GONCHIES_STRAWBERRY
			{
				// Token: 0x0400CAC3 RID: 51907
				public static LocString NAME = "Executive Undies Outfit";
			}

			// Token: 0x02002EF3 RID: 12019
			public class GONCHIES_SATSUMA
			{
				// Token: 0x0400CAC4 RID: 51908
				public static LocString NAME = "Underling Undies Outfit";
			}

			// Token: 0x02002EF4 RID: 12020
			public class GONCHIES_LEMON
			{
				// Token: 0x0400CAC5 RID: 51909
				public static LocString NAME = "Groupthink Undies Outfit";
			}

			// Token: 0x02002EF5 RID: 12021
			public class GONCHIES_LIME
			{
				// Token: 0x0400CAC6 RID: 51910
				public static LocString NAME = "Stakeholder Undies Outfit";
			}

			// Token: 0x02002EF6 RID: 12022
			public class GONCHIES_BLUEBERRY
			{
				// Token: 0x0400CAC7 RID: 51911
				public static LocString NAME = "Admin Undies Outfit";
			}

			// Token: 0x02002EF7 RID: 12023
			public class GONCHIES_GRAPE
			{
				// Token: 0x0400CAC8 RID: 51912
				public static LocString NAME = "Buzzword Undies Outfit";
			}

			// Token: 0x02002EF8 RID: 12024
			public class GONCHIES_WATERMELON
			{
				// Token: 0x0400CAC9 RID: 51913
				public static LocString NAME = "Synergy Undies Outfit";
			}

			// Token: 0x02002EF9 RID: 12025
			public class NERD
			{
				// Token: 0x0400CACA RID: 51914
				public static LocString NAME = "Research Outfit";
			}

			// Token: 0x02002EFA RID: 12026
			public class REBELGI
			{
				// Token: 0x0400CACB RID: 51915
				public static LocString NAME = "Rebel Gi Outfit";
			}

			// Token: 0x02002EFB RID: 12027
			public class DONOR
			{
				// Token: 0x0400CACC RID: 51916
				public static LocString NAME = "Donor Outfit";
			}

			// Token: 0x02002EFC RID: 12028
			public class MECHANIC
			{
				// Token: 0x0400CACD RID: 51917
				public static LocString NAME = "Engineer Coveralls";
			}

			// Token: 0x02002EFD RID: 12029
			public class VELOUR_BLACK
			{
				// Token: 0x0400CACE RID: 51918
				public static LocString NAME = "PhD Velour Outfit";
			}

			// Token: 0x02002EFE RID: 12030
			public class SLEEVELESS_BOW_BW
			{
				// Token: 0x0400CACF RID: 51919
				public static LocString NAME = "PhD Dress Outfit";
			}

			// Token: 0x02002EFF RID: 12031
			public class VELOUR_BLUE
			{
				// Token: 0x0400CAD0 RID: 51920
				public static LocString NAME = "Shortwave Velour Outfit";
			}

			// Token: 0x02002F00 RID: 12032
			public class VELOUR_PINK
			{
				// Token: 0x0400CAD1 RID: 51921
				public static LocString NAME = "Gamma Velour Outfit";
			}

			// Token: 0x02002F01 RID: 12033
			public class WATER
			{
				// Token: 0x0400CAD2 RID: 51922
				public static LocString NAME = "HVAC Coveralls";
			}

			// Token: 0x02002F02 RID: 12034
			public class WAISTCOAT_PINSTRIPE_SLATE
			{
				// Token: 0x0400CAD3 RID: 51923
				public static LocString NAME = "Nobel Pinstripe Outfit";
			}

			// Token: 0x02002F03 RID: 12035
			public class TWEED_PINK_ORCHID
			{
				// Token: 0x0400CAD4 RID: 51924
				public static LocString NAME = "Power Brunch Outfit";
			}

			// Token: 0x02002F04 RID: 12036
			public class BALLET
			{
				// Token: 0x0400CAD5 RID: 51925
				public static LocString NAME = "Ballet Outfit";
			}

			// Token: 0x02002F05 RID: 12037
			public class ATMOSUIT_CANTALOUPE
			{
				// Token: 0x0400CAD6 RID: 51926
				public static LocString NAME = "Rocketmelon Atmo Outfit";
			}

			// Token: 0x02002F06 RID: 12038
			public class PAJAMAS_SNOW
			{
				// Token: 0x0400CAD7 RID: 51927
				public static LocString NAME = "Crystal-Iced Jammies";
			}

			// Token: 0x02002F07 RID: 12039
			public class X_SPORCHID
			{
				// Token: 0x0400CAD8 RID: 51928
				public static LocString NAME = "Sporefest Outfit";
			}

			// Token: 0x02002F08 RID: 12040
			public class X1_PINCHAPEPPERNUTBELLS
			{
				// Token: 0x0400CAD9 RID: 51929
				public static LocString NAME = "Pinchabell Outfit";
			}

			// Token: 0x02002F09 RID: 12041
			public class POMPOM_SHINEBUGS_PINK_PEPPERNUT
			{
				// Token: 0x0400CADA RID: 51930
				public static LocString NAME = "Pom Bug Outfit";
			}

			// Token: 0x02002F0A RID: 12042
			public class SNOWFLAKE_BLUE
			{
				// Token: 0x0400CADB RID: 51931
				public static LocString NAME = "Crystal-Iced Outfit";
			}

			// Token: 0x02002F0B RID: 12043
			public class POLKADOT_TRACKSUIT
			{
				// Token: 0x0400CADC RID: 51932
				public static LocString NAME = "Polka Dot Tracksuit";
			}

			// Token: 0x02002F0C RID: 12044
			public class SUPERSTAR
			{
				// Token: 0x0400CADD RID: 51933
				public static LocString NAME = "Superstar Outfit";
			}

			// Token: 0x02002F0D RID: 12045
			public class ATMOSUIT_SPIFFY
			{
				// Token: 0x0400CADE RID: 51934
				public static LocString NAME = "Spiffy Atmo Outfit";
			}

			// Token: 0x02002F0E RID: 12046
			public class ATMOSUIT_CUBIST
			{
				// Token: 0x0400CADF RID: 51935
				public static LocString NAME = "Cubist Atmo Outfit";
			}

			// Token: 0x02002F0F RID: 12047
			public class LUCKY
			{
				// Token: 0x0400CAE0 RID: 51936
				public static LocString NAME = "Lucky Jammies Outfit";
			}

			// Token: 0x02002F10 RID: 12048
			public class SWEETHEART
			{
				// Token: 0x0400CAE1 RID: 51937
				public static LocString NAME = "Sweetheart Jammies Outfit";
			}

			// Token: 0x02002F11 RID: 12049
			public class GINCH_GLUON
			{
				// Token: 0x0400CAE2 RID: 51938
				public static LocString NAME = "Frilly Saltrock Outfit";
			}

			// Token: 0x02002F12 RID: 12050
			public class GINCH_CORTEX
			{
				// Token: 0x0400CAE3 RID: 51939
				public static LocString NAME = "Dusk Undies Outfit";
			}

			// Token: 0x02002F13 RID: 12051
			public class GINCH_FROSTY
			{
				// Token: 0x0400CAE4 RID: 51940
				public static LocString NAME = "Frostbasin Undies Outfit";
			}

			// Token: 0x02002F14 RID: 12052
			public class GINCH_LOCUS
			{
				// Token: 0x0400CAE5 RID: 51941
				public static LocString NAME = "Balmy Undies Outfit";
			}

			// Token: 0x02002F15 RID: 12053
			public class GINCH_GOOP
			{
				// Token: 0x0400CAE6 RID: 51942
				public static LocString NAME = "Leachy Undies Outfit";
			}

			// Token: 0x02002F16 RID: 12054
			public class GINCH_BILE
			{
				// Token: 0x0400CAE7 RID: 51943
				public static LocString NAME = "Yellowcake Undies Outfit";
			}

			// Token: 0x02002F17 RID: 12055
			public class GINCH_NYBBLE
			{
				// Token: 0x0400CAE8 RID: 51944
				public static LocString NAME = "Atomic Undies Outfit";
			}

			// Token: 0x02002F18 RID: 12056
			public class GINCH_IRONBOW
			{
				// Token: 0x0400CAE9 RID: 51945
				public static LocString NAME = "Magma Undies Outfit";
			}

			// Token: 0x02002F19 RID: 12057
			public class GINCH_PHLEGM
			{
				// Token: 0x0400CAEA RID: 51946
				public static LocString NAME = "Slate Undies Outfit";
			}

			// Token: 0x02002F1A RID: 12058
			public class GINCH_OBELUS
			{
				// Token: 0x0400CAEB RID: 51947
				public static LocString NAME = "Charcoal Undies Outfit";
			}

			// Token: 0x02002F1B RID: 12059
			public class HIVIS
			{
				// Token: 0x0400CAEC RID: 51948
				public static LocString NAME = "Hi-Vis Outfit";
			}

			// Token: 0x02002F1C RID: 12060
			public class DOWNTIME
			{
				// Token: 0x0400CAED RID: 51949
				public static LocString NAME = "Downtime Outfit";
			}

			// Token: 0x02002F1D RID: 12061
			public class FLANNEL_RED
			{
				// Token: 0x0400CAEE RID: 51950
				public static LocString NAME = "Classic Flannel Outfit";
			}

			// Token: 0x02002F1E RID: 12062
			public class FLANNEL_ORANGE
			{
				// Token: 0x0400CAEF RID: 51951
				public static LocString NAME = "Cadmium Flannel Outfit";
			}

			// Token: 0x02002F1F RID: 12063
			public class FLANNEL_YELLOW
			{
				// Token: 0x0400CAF0 RID: 51952
				public static LocString NAME = "Flax Flannel Outfit";
			}

			// Token: 0x02002F20 RID: 12064
			public class FLANNEL_GREEN
			{
				// Token: 0x0400CAF1 RID: 51953
				public static LocString NAME = "Swampy Flannel Outfit";
			}

			// Token: 0x02002F21 RID: 12065
			public class FLANNEL_BLUE_MIDDLE
			{
				// Token: 0x0400CAF2 RID: 51954
				public static LocString NAME = "Scrub Flannel Outfit";
			}

			// Token: 0x02002F22 RID: 12066
			public class FLANNEL_PURPLE
			{
				// Token: 0x0400CAF3 RID: 51955
				public static LocString NAME = "Fusion Flannel Outfit";
			}

			// Token: 0x02002F23 RID: 12067
			public class FLANNEL_PINK_ORCHID
			{
				// Token: 0x0400CAF4 RID: 51956
				public static LocString NAME = "Flare Flannel Outfit";
			}

			// Token: 0x02002F24 RID: 12068
			public class FLANNEL_WHITE
			{
				// Token: 0x0400CAF5 RID: 51957
				public static LocString NAME = "White Flannel Outfit";
			}

			// Token: 0x02002F25 RID: 12069
			public class FLANNEL_BLACK
			{
				// Token: 0x0400CAF6 RID: 51958
				public static LocString NAME = "Monochrome Flannel Outfit";
			}
		}

		// Token: 0x0200252A RID: 9514
		public class ROLES_SCREEN
		{
			// Token: 0x0400A561 RID: 42337
			public static LocString MANAGEMENT_BUTTON = "JOBS";

			// Token: 0x0400A562 RID: 42338
			public static LocString ROLE_PROGRESS = "<b>Job Experience: {0}/{1}</b>\nDuplicants can become eligible for specialized jobs by maxing their current job experience";

			// Token: 0x0400A563 RID: 42339
			public static LocString NO_JOB_STATION_WARNING = string.Concat(new string[]
			{
				"Build a ",
				UI.PRE_KEYWORD,
				"Printing Pod",
				UI.PST_KEYWORD,
				" to unlock this menu\n\nThe ",
				UI.PRE_KEYWORD,
				"Printing Pod",
				UI.PST_KEYWORD,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
				" of the Build Menu"
			});

			// Token: 0x0400A564 RID: 42340
			public static LocString AUTO_PRIORITIZE = "Auto-Prioritize:";

			// Token: 0x0400A565 RID: 42341
			public static LocString AUTO_PRIORITIZE_ENABLED = "Duplicant priorities are automatically reconfigured when they are assigned a new job";

			// Token: 0x0400A566 RID: 42342
			public static LocString AUTO_PRIORITIZE_DISABLED = "Duplicant priorities can only be changed manually";

			// Token: 0x0400A567 RID: 42343
			public static LocString EXPECTATION_ALERT_EXPECTATION = "Current Morale: {0}\nJob Morale Needs: {1}";

			// Token: 0x0400A568 RID: 42344
			public static LocString EXPECTATION_ALERT_JOB = "Current Morale: {0}\n{2} Minimum Morale: {1}";

			// Token: 0x0400A569 RID: 42345
			public static LocString EXPECTATION_ALERT_TARGET_JOB = "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";

			// Token: 0x0400A56A RID: 42346
			public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

			// Token: 0x0400A56B RID: 42347
			public static LocString EXPECTATION_ALERT_DESC_JOB = "This Duplicant's Morale is too low to handle the assigned job, which will cause them Stress over time.";

			// Token: 0x0400A56C RID: 42348
			public static LocString EXPECTATION_ALERT_DESC_TARGET_JOB = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

			// Token: 0x0400A56D RID: 42349
			public static LocString HIGHEST_EXPECTATIONS_TIER = "<b>Highest Expectations</b>";

			// Token: 0x0400A56E RID: 42350
			public static LocString ADDED_EXPECTATIONS_AMOUNT = " (+{0} Expectation)";

			// Token: 0x02002F26 RID: 12070
			public class WIDGET
			{
				// Token: 0x0400CAF7 RID: 51959
				public static LocString NUMBER_OF_MASTERS_TOOLTIP = "<b>Duplicants who have mastered this job:</b>{0}";

				// Token: 0x0400CAF8 RID: 51960
				public static LocString NO_MASTERS_TOOLTIP = "<b>No Duplicants have mastered this job</b>";
			}

			// Token: 0x02002F27 RID: 12071
			public class TIER_NAMES
			{
				// Token: 0x0400CAF9 RID: 51961
				public static LocString ZERO = "Tier 0";

				// Token: 0x0400CAFA RID: 51962
				public static LocString ONE = "Tier 1";

				// Token: 0x0400CAFB RID: 51963
				public static LocString TWO = "Tier 2";

				// Token: 0x0400CAFC RID: 51964
				public static LocString THREE = "Tier 3";

				// Token: 0x0400CAFD RID: 51965
				public static LocString FOUR = "Tier 4";

				// Token: 0x0400CAFE RID: 51966
				public static LocString FIVE = "Tier 5";

				// Token: 0x0400CAFF RID: 51967
				public static LocString SIX = "Tier 6";

				// Token: 0x0400CB00 RID: 51968
				public static LocString SEVEN = "Tier 7";

				// Token: 0x0400CB01 RID: 51969
				public static LocString EIGHT = "Tier 8";

				// Token: 0x0400CB02 RID: 51970
				public static LocString NINE = "Tier 9";
			}

			// Token: 0x02002F28 RID: 12072
			public class SLOTS
			{
				// Token: 0x0400CB03 RID: 51971
				public static LocString UNASSIGNED = "Vacant Position";

				// Token: 0x0400CB04 RID: 51972
				public static LocString UNASSIGNED_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to assign a Duplicant to this job opening";

				// Token: 0x0400CB05 RID: 51973
				public static LocString NOSLOTS = "No slots available";

				// Token: 0x0400CB06 RID: 51974
				public static LocString NO_ELIGIBLE_DUPLICANTS = "No Duplicants meet the requirements for this job";

				// Token: 0x0400CB07 RID: 51975
				public static LocString ASSIGNMENT_PENDING = "(Pending)";

				// Token: 0x0400CB08 RID: 51976
				public static LocString PICK_JOB = "No Job";

				// Token: 0x0400CB09 RID: 51977
				public static LocString PICK_DUPLICANT = "None";
			}

			// Token: 0x02002F29 RID: 12073
			public class DROPDOWN
			{
				// Token: 0x0400CB0A RID: 51978
				public static LocString NAME_AND_ROLE = "{0} <color=#F44A47FF>({1})</color>";

				// Token: 0x0400CB0B RID: 51979
				public static LocString ALREADY_ROLE = "(Currently {0})";
			}

			// Token: 0x02002F2A RID: 12074
			public class SIDEBAR
			{
				// Token: 0x0400CB0C RID: 51980
				public static LocString ASSIGNED_DUPLICANTS = "Assigned Duplicants";

				// Token: 0x0400CB0D RID: 51981
				public static LocString UNASSIGNED_DUPLICANTS = "Unassigned Duplicants";

				// Token: 0x0400CB0E RID: 51982
				public static LocString UNASSIGN = "Unassign job";
			}

			// Token: 0x02002F2B RID: 12075
			public class PRIORITY
			{
				// Token: 0x0400CB0F RID: 51983
				public static LocString TITLE = "Job Priorities";

				// Token: 0x0400CB10 RID: 51984
				public static LocString DESCRIPTION = "{0}s prioritize these work errands: ";

				// Token: 0x0400CB11 RID: 51985
				public static LocString NO_EFFECT = "This job does not affect errand prioritization";
			}

			// Token: 0x02002F2C RID: 12076
			public class RESUME
			{
				// Token: 0x0400CB12 RID: 51986
				public static LocString TITLE = "Qualifications";

				// Token: 0x0400CB13 RID: 51987
				public static LocString PREVIOUS_ROLES = "PREVIOUS DUTIES";

				// Token: 0x0400CB14 RID: 51988
				public static LocString UNASSIGNED = "Unassigned";

				// Token: 0x0400CB15 RID: 51989
				public static LocString NO_SELECTION = "No Duplicant selected";
			}

			// Token: 0x02002F2D RID: 12077
			public class PERKS
			{
				// Token: 0x0400CB16 RID: 51990
				public static LocString TITLE_BASICTRAINING = "Basic Job Training";

				// Token: 0x0400CB17 RID: 51991
				public static LocString TITLE_MORETRAINING = "Additional Job Training";

				// Token: 0x0400CB18 RID: 51992
				public static LocString NO_PERKS = "This job comes with no training";

				// Token: 0x0400CB19 RID: 51993
				public static LocString ATTRIBUTE_EFFECT_FMT = "<b>{0}</b> " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

				// Token: 0x0400CB1A RID: 51994
				public static LocString IMMUNITY = "Immunity to <b>{0}</b>";

				// Token: 0x02003BD9 RID: 15321
				public class CAN_DIG_VERY_FIRM
				{
					// Token: 0x0400EEDA RID: 61146
					public static LocString DESCRIPTION = UI.FormatAsLink(ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + " Material", "HARDNESS") + " Mining";
				}

				// Token: 0x02003BDA RID: 15322
				public class CAN_DIG_NEARLY_IMPENETRABLE
				{
					// Token: 0x0400EEDB RID: 61147
					public static LocString DESCRIPTION = UI.FormatAsLink("Abyssalite", "KATAIRITE") + " Mining";
				}

				// Token: 0x02003BDB RID: 15323
				public class CAN_DIG_SUPER_SUPER_HARD
				{
					// Token: 0x0400EEDC RID: 61148
					public static LocString DESCRIPTION = UI.FormatAsLink("Diamond", "DIAMOND") + " and " + UI.FormatAsLink("Obsidian", "OBSIDIAN") + " Mining";
				}

				// Token: 0x02003BDC RID: 15324
				public class CAN_DIG_RADIOACTIVE_MATERIALS
				{
					// Token: 0x0400EEDD RID: 61149
					public static LocString DESCRIPTION = UI.FormatAsLink("Corium", "CORIUM") + " Mining";
				}

				// Token: 0x02003BDD RID: 15325
				public class CAN_DIG_UNOBTANIUM
				{
					// Token: 0x0400EEDE RID: 61150
					public static LocString DESCRIPTION = UI.FormatAsLink("Neutronium", "UNOBTANIUM") + " Mining";
				}

				// Token: 0x02003BDE RID: 15326
				public class CAN_ART
				{
					// Token: 0x0400EEDF RID: 61151
					public static LocString DESCRIPTION = string.Concat(new string[]
					{
						"Can produce artwork using:\n<indent=30px>• ",
						BUILDINGS.PREFABS.CANVAS.NAME,
						"\n• ",
						BUILDINGS.PREFABS.SMALLSCULPTURE.NAME,
						"\n• ",
						BUILDINGS.PREFABS.SCULPTURE.NAME,
						"\n• ",
						BUILDINGS.PREFABS.ICESCULPTURE.NAME,
						"\n• ",
						BUILDINGS.PREFABS.METALSCULPTURE.NAME,
						"\n• ",
						BUILDINGS.PREFABS.WOODSCULPTURE.NAME,
						"</indent>"
					});
				}

				// Token: 0x02003BDF RID: 15327
				public class CAN_ART_UGLY
				{
					// Token: 0x0400EEE0 RID: 61152
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Crude" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x02003BE0 RID: 15328
				public class CAN_ART_OKAY
				{
					// Token: 0x0400EEE1 RID: 61153
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Mediocre" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x02003BE1 RID: 15329
				public class CAN_ART_GREAT
				{
					// Token: 0x0400EEE2 RID: 61154
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Master" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x02003BE2 RID: 15330
				public class CAN_FARM_TINKER
				{
					// Token: 0x0400EEE3 RID: 61155
					public static LocString DESCRIPTION = UI.FormatAsLink("Crop Tending", "PLANTS");
				}

				// Token: 0x02003BE3 RID: 15331
				public class CAN_IDENTIFY_MUTANT_SEEDS
				{
					// Token: 0x0400EEE4 RID: 61156
					public static LocString DESCRIPTION = string.Concat(new string[]
					{
						"Can identify ",
						UI.PRE_KEYWORD,
						"Mutant Seeds",
						UI.PST_KEYWORD,
						" at the ",
						BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME
					});
				}

				// Token: 0x02003BE4 RID: 15332
				public class CAN_FARM_STATION
				{
					// Token: 0x0400EEE5 RID: 61157
					public static LocString DESCRIPTION = string.Concat(new string[]
					{
						"Can craft ",
						UI.PRE_KEYWORD,
						"Micronutrient Fertilizer",
						UI.PST_KEYWORD,
						" at the ",
						BUILDINGS.PREFABS.FARMSTATION.NAME
					});
				}

				// Token: 0x02003BE5 RID: 15333
				public class CAN_SALVAGE_PLANT_FIBER
				{
					// Token: 0x0400EEE6 RID: 61158
					public static LocString DESCRIPTION = "Can salvage " + ITEMS.INDUSTRIAL_PRODUCTS.PLANT_FIBER.NAME;
				}

				// Token: 0x02003BE6 RID: 15334
				public class CAN_WRANGLE_CREATURES
				{
					// Token: 0x0400EEE7 RID: 61159
					public static LocString DESCRIPTION = "Critter Wrangling";
				}

				// Token: 0x02003BE7 RID: 15335
				public class CAN_USE_BUILDING
				{
					// Token: 0x0400EEE8 RID: 61160
					public static LocString DESCRIPTION = "{0} Usage";
				}

				// Token: 0x02003BE8 RID: 15336
				public class CAN_USE_RANCH_STATION
				{
					// Token: 0x0400EEE9 RID: 61161
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.RANCHSTATION.NAME + " Usage";
				}

				// Token: 0x02003BE9 RID: 15337
				public class CAN_USE_MILKING_STATION
				{
					// Token: 0x0400EEEA RID: 61162
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MILKINGSTATION.NAME + " Usage";
				}

				// Token: 0x02003BEA RID: 15338
				public class CAN_POWER_TINKER
				{
					// Token: 0x0400EEEB RID: 61163
					public static LocString DESCRIPTION = UI.FormatAsLink("Generator", "POWER") + " Tuning and " + UI.FormatAsLink("Microchip", "POWER_STATION_TOOLS") + " Crafting";
				}

				// Token: 0x02003BEB RID: 15339
				public class CAN_ELECTRIC_GRILL
				{
					// Token: 0x0400EEEC RID: 61164
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COOKINGSTATION.NAME + " Usage";
				}

				// Token: 0x02003BEC RID: 15340
				public class CAN_GAS_RANGE
				{
					// Token: 0x0400EEED RID: 61165
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.GOURMETCOOKINGSTATION.NAME + " Usage";
				}

				// Token: 0x02003BED RID: 15341
				public class CAN_DEEP_FRYER
				{
					// Token: 0x0400EEEE RID: 61166
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.DEEPFRYER.NAME + " Usage";
				}

				// Token: 0x02003BEE RID: 15342
				public class CAN_SPICE_GRINDER
				{
					// Token: 0x0400EEEF RID: 61167
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.SPICEGRINDER.NAME + " Usage";
				}

				// Token: 0x02003BEF RID: 15343
				public class CAN_MAKE_MISSILES
				{
					// Token: 0x0400EEF0 RID: 61168
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MISSILEFABRICATOR.NAME + " Usage";
				}

				// Token: 0x02003BF0 RID: 15344
				public class CAN_CRAFT_ELECTRONICS
				{
					// Token: 0x0400EEF1 RID: 61169
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME + " Usage";
				}

				// Token: 0x02003BF1 RID: 15345
				public class ADVANCED_RESEARCH
				{
					// Token: 0x0400EEF2 RID: 61170
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x02003BF2 RID: 15346
				public class INTERSTELLAR_RESEARCH
				{
					// Token: 0x0400EEF3 RID: 61171
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x02003BF3 RID: 15347
				public class NUCLEAR_RESEARCH
				{
					// Token: 0x0400EEF4 RID: 61172
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x02003BF4 RID: 15348
				public class ORBITAL_RESEARCH
				{
					// Token: 0x0400EEF5 RID: 61173
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.DLC1COSMICRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x02003BF5 RID: 15349
				public class GEYSER_TUNING
				{
					// Token: 0x0400EEF6 RID: 61174
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.GEOTUNER.NAME + " Usage";
				}

				// Token: 0x02003BF6 RID: 15350
				public class CHEMISTRY
				{
					// Token: 0x0400EEF7 RID: 61175
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CHEMICALREFINERY.NAME + " Usage";
				}

				// Token: 0x02003BF7 RID: 15351
				public class CAN_CLOTHING_ALTERATION
				{
					// Token: 0x0400EEF8 RID: 61176
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CLOTHINGALTERATIONSTATION.NAME + " Usage";
				}

				// Token: 0x02003BF8 RID: 15352
				public class CAN_STUDY_WORLD_OBJECTS
				{
					// Token: 0x0400EEF9 RID: 61177
					public static LocString DESCRIPTION = "Geographical Analysis";
				}

				// Token: 0x02003BF9 RID: 15353
				public class CAN_STUDY_ARTIFACTS
				{
					// Token: 0x0400EEFA RID: 61178
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME + " Usage";
				}

				// Token: 0x02003BFA RID: 15354
				public class CAN_USE_CLUSTER_TELESCOPE
				{
					// Token: 0x0400EEFB RID: 61179
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " Usage";
				}

				// Token: 0x02003BFB RID: 15355
				public class CAN_CLUSTERTELESCOPEENCLOSED
				{
					// Token: 0x0400EEFC RID: 61180
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME + " Usage";
				}

				// Token: 0x02003BFC RID: 15356
				public class EXOSUIT_EXPERTISE
				{
					// Token: 0x0400EEFD RID: 61181
					public static LocString DESCRIPTION = UI.FormatAsLink("Exosuit", "EQUIPMENT") + " Penalty Reduction";
				}

				// Token: 0x02003BFD RID: 15357
				public class EXOSUIT_DURABILITY
				{
					// Token: 0x0400EEFE RID: 61182
					public static LocString DESCRIPTION = "Slows " + UI.FormatAsLink("Exosuit", "EQUIPMENT") + " Durability Damage";
				}

				// Token: 0x02003BFE RID: 15358
				public class CONVEYOR_BUILD
				{
					// Token: 0x0400EEFF RID: 61183
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.SOLIDCONDUIT.NAME + " Construction";
				}

				// Token: 0x02003BFF RID: 15359
				public class CAN_DO_PLUMBING
				{
					// Token: 0x0400EF00 RID: 61184
					public static LocString DESCRIPTION = "Pipe Emptying";
				}

				// Token: 0x02003C00 RID: 15360
				public class CAN_USE_ROCKETS
				{
					// Token: 0x0400EF01 RID: 61185
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COMMANDMODULE.NAME + " Usage";
				}

				// Token: 0x02003C01 RID: 15361
				public class CAN_DO_ASTRONAUT_TRAINING
				{
					// Token: 0x0400EF02 RID: 61186
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ASTRONAUTTRAININGCENTER.NAME + " Usage";
				}

				// Token: 0x02003C02 RID: 15362
				public class CAN_MISSION_CONTROL
				{
					// Token: 0x0400EF03 RID: 61187
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MISSIONCONTROL.NAME + " Usage";
				}

				// Token: 0x02003C03 RID: 15363
				public class CAN_PILOT_ROCKET
				{
					// Token: 0x0400EF04 RID: 61188
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME + " Usage";
				}

				// Token: 0x02003C04 RID: 15364
				public class CAN_COMPOUND
				{
					// Token: 0x0400EF05 RID: 61189
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.APOTHECARY.NAME + " Usage";
				}

				// Token: 0x02003C05 RID: 15365
				public class CAN_DOCTOR
				{
					// Token: 0x0400EF06 RID: 61190
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.DOCTORSTATION.NAME + " Usage";
				}

				// Token: 0x02003C06 RID: 15366
				public class CAN_ADVANCED_MEDICINE
				{
					// Token: 0x0400EF07 RID: 61191
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME + " Usage";
				}

				// Token: 0x02003C07 RID: 15367
				public class CAN_DEMOLISH
				{
					// Token: 0x0400EF08 RID: 61192
					public static LocString DESCRIPTION = "Demolish Gravitas Buildings";
				}

				// Token: 0x02003C08 RID: 15368
				public class EXTRA_BIONIC_BATTERIES
				{
					// Token: 0x0400EF09 RID: 61193
					public static LocString DESCRIPTION = "Extra " + UI.FormatAsLink("Power Banks", "ELECTROBANK");
				}

				// Token: 0x02003C09 RID: 15369
				public class REDUCED_GUNK_PRODUCTION
				{
					// Token: 0x0400EF0A RID: 61194
					public static LocString DESCRIPTION = "10% Slower " + UI.FormatAsLink("Gunk", "LIQUIDGUNK") + " Buildup";
				}

				// Token: 0x02003C0A RID: 15370
				public class EFFICIENT_BIONIC_GEARS
				{
					// Token: 0x0400EF0B RID: 61195
					public static LocString DESCRIPTION = "50% Grinding Gears penalty reduction";
				}
			}

			// Token: 0x02002F2E RID: 12078
			public class ASSIGNMENT_REQUIREMENTS
			{
				// Token: 0x0400CB1B RID: 51995
				public static LocString TITLE = "Qualifications";

				// Token: 0x0400CB1C RID: 51996
				public static LocString NONE = "This position has no qualification requirements";

				// Token: 0x0400CB1D RID: 51997
				public static LocString ALREADY_IS_ROLE = "{0} <b>is already</b> assigned to the {1} position";

				// Token: 0x0400CB1E RID: 51998
				public static LocString ALREADY_IS_JOBLESS = "{0} <b>is already</b> unemployed";

				// Token: 0x0400CB1F RID: 51999
				public static LocString MASTERED = "{0} has mastered the {1} position";

				// Token: 0x0400CB20 RID: 52000
				public static LocString WILL_BE_UNASSIGNED = "Note: Assigning {0} to {1} will <color=#F44A47FF>unassign</color> them from {2}";

				// Token: 0x0400CB21 RID: 52001
				public static LocString RELEVANT_ATTRIBUTES = "Relevant skills:";

				// Token: 0x0400CB22 RID: 52002
				public static LocString APTITUDES = "Interests";

				// Token: 0x0400CB23 RID: 52003
				public static LocString RELEVANT_APTITUDES = "Relevant Interests:";

				// Token: 0x0400CB24 RID: 52004
				public static LocString NO_APTITUDE = "None";

				// Token: 0x02003C0B RID: 15371
				public class ELIGIBILITY
				{
					// Token: 0x0400EF0C RID: 61196
					public static LocString ELIGIBLE = "{0} is qualified for the {1} position";

					// Token: 0x0400EF0D RID: 61197
					public static LocString INELIGIBLE = "{0} is <color=#F44A47FF>not qualified</color> for the {1} position";
				}

				// Token: 0x02003C0C RID: 15372
				public class UNEMPLOYED
				{
					// Token: 0x0400EF0E RID: 61198
					public static LocString NAME = "Unassigned";

					// Token: 0x0400EF0F RID: 61199
					public static LocString DESCRIPTION = "Duplicant must not already have a job assignment";
				}

				// Token: 0x02003C0D RID: 15373
				public class HAS_COLONY_LEADER
				{
					// Token: 0x0400EF10 RID: 61200
					public static LocString NAME = "Has colony leader";

					// Token: 0x0400EF11 RID: 61201
					public static LocString DESCRIPTION = "A colony leader must be assigned";
				}

				// Token: 0x02003C0E RID: 15374
				public class HAS_ATTRIBUTE_DIGGING_BASIC
				{
					// Token: 0x0400EF12 RID: 61202
					public static LocString NAME = "Basic Digging";

					// Token: 0x0400EF13 RID: 61203
					public static LocString DESCRIPTION = "Must have at least {0} digging skill";
				}

				// Token: 0x02003C0F RID: 15375
				public class HAS_ATTRIBUTE_COOKING_BASIC
				{
					// Token: 0x0400EF14 RID: 61204
					public static LocString NAME = "Basic Cooking";

					// Token: 0x0400EF15 RID: 61205
					public static LocString DESCRIPTION = "Must have at least {0} cooking skill";
				}

				// Token: 0x02003C10 RID: 15376
				public class HAS_ATTRIBUTE_LEARNING_BASIC
				{
					// Token: 0x0400EF16 RID: 61206
					public static LocString NAME = "Basic Learning";

					// Token: 0x0400EF17 RID: 61207
					public static LocString DESCRIPTION = "Must have at least {0} learning skill";
				}

				// Token: 0x02003C11 RID: 15377
				public class HAS_ATTRIBUTE_LEARNING_MEDIUM
				{
					// Token: 0x0400EF18 RID: 61208
					public static LocString NAME = "Medium Learning";

					// Token: 0x0400EF19 RID: 61209
					public static LocString DESCRIPTION = "Must have at least {0} learning skill";
				}

				// Token: 0x02003C12 RID: 15378
				public class HAS_EXPERIENCE
				{
					// Token: 0x0400EF1A RID: 61210
					public static LocString NAME = "{0} Experience";

					// Token: 0x0400EF1B RID: 61211
					public static LocString DESCRIPTION = "Mastery of the <b>{0}</b> job";
				}

				// Token: 0x02003C13 RID: 15379
				public class HAS_COMPLETED_ANY_OTHER_ROLE
				{
					// Token: 0x0400EF1C RID: 61212
					public static LocString NAME = "General Experience";

					// Token: 0x0400EF1D RID: 61213
					public static LocString DESCRIPTION = "Mastery of <b>at least one</b> job";
				}

				// Token: 0x02003C14 RID: 15380
				public class CHOREGROUP_ENABLED
				{
					// Token: 0x0400EF1E RID: 61214
					public static LocString NAME = "Can perform {0}";

					// Token: 0x0400EF1F RID: 61215
					public static LocString DESCRIPTION = "Capable of performing <b>{0}</b> jobs";
				}
			}

			// Token: 0x02002F2F RID: 12079
			public class EXPECTATIONS
			{
				// Token: 0x0400CB25 RID: 52005
				public static LocString TITLE = "Special Provisions Request";

				// Token: 0x0400CB26 RID: 52006
				public static LocString NO_EXPECTATIONS = "No additional provisions are required to perform this job";

				// Token: 0x02003C15 RID: 15381
				public class PRIVATE_ROOM
				{
					// Token: 0x0400EF20 RID: 61216
					public static LocString NAME = "Private Bedroom";

					// Token: 0x0400EF21 RID: 61217
					public static LocString DESCRIPTION = "Duplicants in this job would appreciate their own place to unwind";
				}

				// Token: 0x02003C16 RID: 15382
				public class FOOD_QUALITY
				{
					// Token: 0x020040A5 RID: 16549
					public class MINOR
					{
						// Token: 0x0400FA18 RID: 64024
						public static LocString NAME = "Standard Food";

						// Token: 0x0400FA19 RID: 64025
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire food that meets basic living standards";
					}

					// Token: 0x020040A6 RID: 16550
					public class MEDIUM
					{
						// Token: 0x0400FA1A RID: 64026
						public static LocString NAME = "Good Food";

						// Token: 0x0400FA1B RID: 64027
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire decent food for their efforts";
					}

					// Token: 0x020040A7 RID: 16551
					public class HIGH
					{
						// Token: 0x0400FA1C RID: 64028
						public static LocString NAME = "Great Food";

						// Token: 0x0400FA1D RID: 64029
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire better than average food";
					}

					// Token: 0x020040A8 RID: 16552
					public class VERY_HIGH
					{
						// Token: 0x0400FA1E RID: 64030
						public static LocString NAME = "Superb Food";

						// Token: 0x0400FA1F RID: 64031
						public static LocString DESCRIPTION = "Duplicants employed in this Tier have a refined taste for food";
					}

					// Token: 0x020040A9 RID: 16553
					public class EXCEPTIONAL
					{
						// Token: 0x0400FA20 RID: 64032
						public static LocString NAME = "Ambrosial Food";

						// Token: 0x0400FA21 RID: 64033
						public static LocString DESCRIPTION = "Duplicants employed in this Tier expect only the best cuisine";
					}
				}

				// Token: 0x02003C17 RID: 15383
				public class DECOR
				{
					// Token: 0x020040AA RID: 16554
					public class MINOR
					{
						// Token: 0x0400FA22 RID: 64034
						public static LocString NAME = "Minor Decor";

						// Token: 0x0400FA23 RID: 64035
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire slightly improved colony decor";
					}

					// Token: 0x020040AB RID: 16555
					public class MEDIUM
					{
						// Token: 0x0400FA24 RID: 64036
						public static LocString NAME = "Medium Decor";

						// Token: 0x0400FA25 RID: 64037
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire reasonably improved colony decor";
					}

					// Token: 0x020040AC RID: 16556
					public class HIGH
					{
						// Token: 0x0400FA26 RID: 64038
						public static LocString NAME = "High Decor";

						// Token: 0x0400FA27 RID: 64039
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire a decent increase in colony decor";
					}

					// Token: 0x020040AD RID: 16557
					public class VERY_HIGH
					{
						// Token: 0x0400FA28 RID: 64040
						public static LocString NAME = "Superb Decor";

						// Token: 0x0400FA29 RID: 64041
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire majorly improved colony decor";
					}

					// Token: 0x020040AE RID: 16558
					public class UNREASONABLE
					{
						// Token: 0x0400FA2A RID: 64042
						public static LocString NAME = "Decadent Decor";

						// Token: 0x0400FA2B RID: 64043
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire unrealistically luxurious improvements to decor";
					}
				}

				// Token: 0x02003C18 RID: 15384
				public class QUALITYOFLIFE
				{
					// Token: 0x020040AF RID: 16559
					public class TIER0
					{
						// Token: 0x0400FA2C RID: 64044
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400FA2D RID: 64045
						public static LocString DESCRIPTION = "Tier 0";
					}

					// Token: 0x020040B0 RID: 16560
					public class TIER1
					{
						// Token: 0x0400FA2E RID: 64046
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400FA2F RID: 64047
						public static LocString DESCRIPTION = "Tier 1";
					}

					// Token: 0x020040B1 RID: 16561
					public class TIER2
					{
						// Token: 0x0400FA30 RID: 64048
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400FA31 RID: 64049
						public static LocString DESCRIPTION = "Tier 2";
					}

					// Token: 0x020040B2 RID: 16562
					public class TIER3
					{
						// Token: 0x0400FA32 RID: 64050
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400FA33 RID: 64051
						public static LocString DESCRIPTION = "Tier 3";
					}

					// Token: 0x020040B3 RID: 16563
					public class TIER4
					{
						// Token: 0x0400FA34 RID: 64052
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400FA35 RID: 64053
						public static LocString DESCRIPTION = "Tier 4";
					}

					// Token: 0x020040B4 RID: 16564
					public class TIER5
					{
						// Token: 0x0400FA36 RID: 64054
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400FA37 RID: 64055
						public static LocString DESCRIPTION = "Tier 5";
					}

					// Token: 0x020040B5 RID: 16565
					public class TIER6
					{
						// Token: 0x0400FA38 RID: 64056
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400FA39 RID: 64057
						public static LocString DESCRIPTION = "Tier 6";
					}

					// Token: 0x020040B6 RID: 16566
					public class TIER7
					{
						// Token: 0x0400FA3A RID: 64058
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400FA3B RID: 64059
						public static LocString DESCRIPTION = "Tier 7";
					}

					// Token: 0x020040B7 RID: 16567
					public class TIER8
					{
						// Token: 0x0400FA3C RID: 64060
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400FA3D RID: 64061
						public static LocString DESCRIPTION = "Tier 8";
					}
				}
			}
		}

		// Token: 0x0200252B RID: 9515
		public class GAMEPLAY_EVENT_INFO_SCREEN
		{
			// Token: 0x0400A56F RID: 42351
			public static LocString WHERE = "WHERE: {0}";

			// Token: 0x0400A570 RID: 42352
			public static LocString WHEN = "WHEN: {0}";
		}

		// Token: 0x0200252C RID: 9516
		public class DEBUG_TOOLS
		{
			// Token: 0x0400A571 RID: 42353
			public static LocString ENTER_TEXT = "";

			// Token: 0x0400A572 RID: 42354
			public static LocString DEBUG_ACTIVE = "Debug tools active";

			// Token: 0x0400A573 RID: 42355
			public static LocString INVALID_LOCATION = "Invalid Location";

			// Token: 0x02002F30 RID: 12080
			public class PAINT_ELEMENTS_SCREEN
			{
				// Token: 0x0400CB27 RID: 52007
				public static LocString TITLE = "CELL PAINTER";

				// Token: 0x0400CB28 RID: 52008
				public static LocString ELEMENT = "Element";

				// Token: 0x0400CB29 RID: 52009
				public static LocString MASS_KG = "Mass (kg)";

				// Token: 0x0400CB2A RID: 52010
				public static LocString TEMPERATURE_KELVIN = "Temperature (K)";

				// Token: 0x0400CB2B RID: 52011
				public static LocString DISEASE = "Disease";

				// Token: 0x0400CB2C RID: 52012
				public static LocString DISEASE_COUNT = "Disease Count";

				// Token: 0x0400CB2D RID: 52013
				public static LocString BUILDINGS = "Buildings:";

				// Token: 0x0400CB2E RID: 52014
				public static LocString CELLS = "Cells:";

				// Token: 0x0400CB2F RID: 52015
				public static LocString ADD_FOW_MASK = "Prevent FoW Reveal";

				// Token: 0x0400CB30 RID: 52016
				public static LocString REMOVE_FOW_MASK = "Allow FoW Reveal";

				// Token: 0x0400CB31 RID: 52017
				public static LocString PAINT = "Paint";

				// Token: 0x0400CB32 RID: 52018
				public static LocString SAMPLE = "Sample";

				// Token: 0x0400CB33 RID: 52019
				public static LocString STORE = "Store";

				// Token: 0x0400CB34 RID: 52020
				public static LocString FILL = "Fill";

				// Token: 0x0400CB35 RID: 52021
				public static LocString SPAWN_ALL = "Spawn All (Slow)";
			}

			// Token: 0x02002F31 RID: 12081
			public class SAVE_BASE_TEMPLATE
			{
				// Token: 0x0400CB36 RID: 52022
				public static LocString TITLE = "Base and World Tools";

				// Token: 0x0400CB37 RID: 52023
				public static LocString SAVE_TITLE = "Save Selection";

				// Token: 0x0400CB38 RID: 52024
				public static LocString CLEAR_BUTTON = "Clear Floor";

				// Token: 0x0400CB39 RID: 52025
				public static LocString DESTROY_BUTTON = "Destroy";

				// Token: 0x0400CB3A RID: 52026
				public static LocString DECONSTRUCT_BUTTON = "Deconstruct";

				// Token: 0x0400CB3B RID: 52027
				public static LocString CLEAR_SELECTION_BUTTON = "Clear Selection";

				// Token: 0x0400CB3C RID: 52028
				public static LocString DEFAULT_SAVE_NAME = "TemplateSaveName";

				// Token: 0x0400CB3D RID: 52029
				public static LocString MORE = "More";

				// Token: 0x0400CB3E RID: 52030
				public static LocString BASE_GAME_FOLDER_NAME = "Base Game";

				// Token: 0x02003C19 RID: 15385
				public class SELECTION_INFO_PANEL
				{
					// Token: 0x0400EF22 RID: 61218
					public static LocString TOTAL_MASS = "Total mass: {0}";

					// Token: 0x0400EF23 RID: 61219
					public static LocString AVERAGE_MASS = "Average cell mass: {0}";

					// Token: 0x0400EF24 RID: 61220
					public static LocString AVERAGE_TEMPERATURE = "Average temperature: {0}";

					// Token: 0x0400EF25 RID: 61221
					public static LocString TOTAL_JOULES = "Total joules: {0}";

					// Token: 0x0400EF26 RID: 61222
					public static LocString JOULES_PER_KILOGRAM = "Joules per kilogram: {0}";

					// Token: 0x0400EF27 RID: 61223
					public static LocString TOTAL_RADS = "Total rads: {0}";

					// Token: 0x0400EF28 RID: 61224
					public static LocString AVERAGE_RADS = "Average rads: {0}";
				}
			}
		}

		// Token: 0x0200252D RID: 9517
		public class WORLDGEN
		{
			// Token: 0x0400A574 RID: 42356
			public static LocString NOHEADERS = "";

			// Token: 0x0400A575 RID: 42357
			public static LocString COMPLETE = "Success! Space adventure awaits.";

			// Token: 0x0400A576 RID: 42358
			public static LocString FAILED = "Goodness, has this ever gone terribly wrong!";

			// Token: 0x0400A577 RID: 42359
			public static LocString RESTARTING = "Rebooting...";

			// Token: 0x0400A578 RID: 42360
			public static LocString LOADING = "Loading world...";

			// Token: 0x0400A579 RID: 42361
			public static LocString GENERATINGWORLD = "The Galaxy Synthesizer";

			// Token: 0x0400A57A RID: 42362
			public static LocString CHOOSEWORLDSIZE = "Select the magnitude of your new galaxy.";

			// Token: 0x0400A57B RID: 42363
			public static LocString USING_PLAYER_SEED = "Using selected worldgen seed: {0}";

			// Token: 0x0400A57C RID: 42364
			public static LocString CLEARINGLEVEL = "Staring into the void...";

			// Token: 0x0400A57D RID: 42365
			public static LocString GENERATESOLARSYSTEM = "Catalyzing Big Bang...";

			// Token: 0x0400A57E RID: 42366
			public static LocString GENERATESOLARSYSTEM1 = "Catalyzing Big Bang...";

			// Token: 0x0400A57F RID: 42367
			public static LocString GENERATESOLARSYSTEM2 = "Catalyzing Big Bang...";

			// Token: 0x0400A580 RID: 42368
			public static LocString GENERATESOLARSYSTEM3 = "Catalyzing Big Bang...";

			// Token: 0x0400A581 RID: 42369
			public static LocString GENERATESOLARSYSTEM4 = "Catalyzing Big Bang...";

			// Token: 0x0400A582 RID: 42370
			public static LocString GENERATESOLARSYSTEM5 = "Catalyzing Big Bang...";

			// Token: 0x0400A583 RID: 42371
			public static LocString GENERATESOLARSYSTEM6 = "Approaching event horizon...";

			// Token: 0x0400A584 RID: 42372
			public static LocString GENERATESOLARSYSTEM7 = "Approaching event horizon...";

			// Token: 0x0400A585 RID: 42373
			public static LocString GENERATESOLARSYSTEM8 = "Approaching event horizon...";

			// Token: 0x0400A586 RID: 42374
			public static LocString GENERATESOLARSYSTEM9 = "Approaching event horizon...";

			// Token: 0x0400A587 RID: 42375
			public static LocString SETUPNOISE = "BANG!";

			// Token: 0x0400A588 RID: 42376
			public static LocString BUILDNOISESOURCE = "Sorting quadrillions of atoms...";

			// Token: 0x0400A589 RID: 42377
			public static LocString BUILDNOISESOURCE1 = "Sorting quadrillions of atoms...";

			// Token: 0x0400A58A RID: 42378
			public static LocString BUILDNOISESOURCE2 = "Sorting quadrillions of atoms...";

			// Token: 0x0400A58B RID: 42379
			public static LocString BUILDNOISESOURCE3 = "Ironing the fabric of creation...";

			// Token: 0x0400A58C RID: 42380
			public static LocString BUILDNOISESOURCE4 = "Ironing the fabric of creation...";

			// Token: 0x0400A58D RID: 42381
			public static LocString BUILDNOISESOURCE5 = "Ironing the fabric of creation...";

			// Token: 0x0400A58E RID: 42382
			public static LocString BUILDNOISESOURCE6 = "Taking hot meteor shower...";

			// Token: 0x0400A58F RID: 42383
			public static LocString BUILDNOISESOURCE7 = "Tightening asteroid belts...";

			// Token: 0x0400A590 RID: 42384
			public static LocString BUILDNOISESOURCE8 = "Tightening asteroid belts...";

			// Token: 0x0400A591 RID: 42385
			public static LocString BUILDNOISESOURCE9 = "Tightening asteroid belts...";

			// Token: 0x0400A592 RID: 42386
			public static LocString GENERATENOISE = "Baking igneous rock...";

			// Token: 0x0400A593 RID: 42387
			public static LocString GENERATENOISE1 = "Multilayering sediment...";

			// Token: 0x0400A594 RID: 42388
			public static LocString GENERATENOISE2 = "Multilayering sediment...";

			// Token: 0x0400A595 RID: 42389
			public static LocString GENERATENOISE3 = "Multilayering sediment...";

			// Token: 0x0400A596 RID: 42390
			public static LocString GENERATENOISE4 = "Superheating gases...";

			// Token: 0x0400A597 RID: 42391
			public static LocString GENERATENOISE5 = "Superheating gases...";

			// Token: 0x0400A598 RID: 42392
			public static LocString GENERATENOISE6 = "Superheating gases...";

			// Token: 0x0400A599 RID: 42393
			public static LocString GENERATENOISE7 = "Vacuuming out vacuums...";

			// Token: 0x0400A59A RID: 42394
			public static LocString GENERATENOISE8 = "Vacuuming out vacuums...";

			// Token: 0x0400A59B RID: 42395
			public static LocString GENERATENOISE9 = "Vacuuming out vacuums...";

			// Token: 0x0400A59C RID: 42396
			public static LocString NORMALISENOISE = "Interpolating suffocating gas...";

			// Token: 0x0400A59D RID: 42397
			public static LocString WORLDLAYOUT = "Freezing ice formations...";

			// Token: 0x0400A59E RID: 42398
			public static LocString WORLDLAYOUT1 = "Freezing ice formations...";

			// Token: 0x0400A59F RID: 42399
			public static LocString WORLDLAYOUT2 = "Freezing ice formations...";

			// Token: 0x0400A5A0 RID: 42400
			public static LocString WORLDLAYOUT3 = "Freezing ice formations...";

			// Token: 0x0400A5A1 RID: 42401
			public static LocString WORLDLAYOUT4 = "Melting magma...";

			// Token: 0x0400A5A2 RID: 42402
			public static LocString WORLDLAYOUT5 = "Melting magma...";

			// Token: 0x0400A5A3 RID: 42403
			public static LocString WORLDLAYOUT6 = "Melting magma...";

			// Token: 0x0400A5A4 RID: 42404
			public static LocString WORLDLAYOUT7 = "Sprinkling sand...";

			// Token: 0x0400A5A5 RID: 42405
			public static LocString WORLDLAYOUT8 = "Sprinkling sand...";

			// Token: 0x0400A5A6 RID: 42406
			public static LocString WORLDLAYOUT9 = "Sprinkling sand...";

			// Token: 0x0400A5A7 RID: 42407
			public static LocString WORLDLAYOUT10 = "Sprinkling sand...";

			// Token: 0x0400A5A8 RID: 42408
			public static LocString COMPLETELAYOUT = "Cooling glass...";

			// Token: 0x0400A5A9 RID: 42409
			public static LocString COMPLETELAYOUT1 = "Cooling glass...";

			// Token: 0x0400A5AA RID: 42410
			public static LocString COMPLETELAYOUT2 = "Cooling glass...";

			// Token: 0x0400A5AB RID: 42411
			public static LocString COMPLETELAYOUT3 = "Cooling glass...";

			// Token: 0x0400A5AC RID: 42412
			public static LocString COMPLETELAYOUT4 = "Digging holes...";

			// Token: 0x0400A5AD RID: 42413
			public static LocString COMPLETELAYOUT5 = "Digging holes...";

			// Token: 0x0400A5AE RID: 42414
			public static LocString COMPLETELAYOUT6 = "Digging holes...";

			// Token: 0x0400A5AF RID: 42415
			public static LocString COMPLETELAYOUT7 = "Adding buckets of dirt...";

			// Token: 0x0400A5B0 RID: 42416
			public static LocString COMPLETELAYOUT8 = "Adding buckets of dirt...";

			// Token: 0x0400A5B1 RID: 42417
			public static LocString COMPLETELAYOUT9 = "Adding buckets of dirt...";

			// Token: 0x0400A5B2 RID: 42418
			public static LocString COMPLETELAYOUT10 = "Adding buckets of dirt...";

			// Token: 0x0400A5B3 RID: 42419
			public static LocString PROCESSRIVERS = "Pouring rivers...";

			// Token: 0x0400A5B4 RID: 42420
			public static LocString CONVERTTERRAINCELLSTOEDGES = "Hardening diamonds...";

			// Token: 0x0400A5B5 RID: 42421
			public static LocString PROCESSING = "Embedding metals...";

			// Token: 0x0400A5B6 RID: 42422
			public static LocString PROCESSING1 = "Embedding metals...";

			// Token: 0x0400A5B7 RID: 42423
			public static LocString PROCESSING2 = "Embedding metals...";

			// Token: 0x0400A5B8 RID: 42424
			public static LocString PROCESSING3 = "Burying precious ore...";

			// Token: 0x0400A5B9 RID: 42425
			public static LocString PROCESSING4 = "Burying precious ore...";

			// Token: 0x0400A5BA RID: 42426
			public static LocString PROCESSING5 = "Burying precious ore...";

			// Token: 0x0400A5BB RID: 42427
			public static LocString PROCESSING6 = "Burying precious ore...";

			// Token: 0x0400A5BC RID: 42428
			public static LocString PROCESSING7 = "Excavating tunnels...";

			// Token: 0x0400A5BD RID: 42429
			public static LocString PROCESSING8 = "Excavating tunnels...";

			// Token: 0x0400A5BE RID: 42430
			public static LocString PROCESSING9 = "Excavating tunnels...";

			// Token: 0x0400A5BF RID: 42431
			public static LocString BORDERS = "Just adding water...";

			// Token: 0x0400A5C0 RID: 42432
			public static LocString BORDERS1 = "Just adding water...";

			// Token: 0x0400A5C1 RID: 42433
			public static LocString BORDERS2 = "Staring at the void...";

			// Token: 0x0400A5C2 RID: 42434
			public static LocString BORDERS3 = "Staring at the void...";

			// Token: 0x0400A5C3 RID: 42435
			public static LocString BORDERS4 = "Staring at the void...";

			// Token: 0x0400A5C4 RID: 42436
			public static LocString BORDERS5 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400A5C5 RID: 42437
			public static LocString BORDERS6 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400A5C6 RID: 42438
			public static LocString BORDERS7 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400A5C7 RID: 42439
			public static LocString BORDERS8 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400A5C8 RID: 42440
			public static LocString BORDERS9 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400A5C9 RID: 42441
			public static LocString DRAWWORLDBORDER = "Establishing personal boundaries...";

			// Token: 0x0400A5CA RID: 42442
			public static LocString PLACINGTEMPLATES = "Generating interest...";

			// Token: 0x0400A5CB RID: 42443
			public static LocString SETTLESIM = "Infusing oxygen...";

			// Token: 0x0400A5CC RID: 42444
			public static LocString SETTLESIM1 = "Infusing oxygen...";

			// Token: 0x0400A5CD RID: 42445
			public static LocString SETTLESIM2 = "Too much oxygen. Removing...";

			// Token: 0x0400A5CE RID: 42446
			public static LocString SETTLESIM3 = "Too much oxygen. Removing...";

			// Token: 0x0400A5CF RID: 42447
			public static LocString SETTLESIM4 = "Ideal oxygen levels achieved...";

			// Token: 0x0400A5D0 RID: 42448
			public static LocString SETTLESIM5 = "Ideal oxygen levels achieved...";

			// Token: 0x0400A5D1 RID: 42449
			public static LocString SETTLESIM6 = "Planting space flora...";

			// Token: 0x0400A5D2 RID: 42450
			public static LocString SETTLESIM7 = "Planting space flora...";

			// Token: 0x0400A5D3 RID: 42451
			public static LocString SETTLESIM8 = "Releasing wildlife...";

			// Token: 0x0400A5D4 RID: 42452
			public static LocString SETTLESIM9 = "Releasing wildlife...";

			// Token: 0x0400A5D5 RID: 42453
			public static LocString ANALYZINGWORLD = "Shuffling DNA Blueprints...";

			// Token: 0x0400A5D6 RID: 42454
			public static LocString ANALYZINGWORLDCOMPLETE = "Tidying up for the Duplicants...";

			// Token: 0x0400A5D7 RID: 42455
			public static LocString PLACINGCREATURES = "Building the suspense...";
		}

		// Token: 0x0200252E RID: 9518
		public class TOOLTIPS
		{
			// Token: 0x0400A5D8 RID: 42456
			public static LocString MANAGEMENTMENU_JOBS = string.Concat(new string[]
			{
				"Manage my Duplicant Priorities {Hotkey}\n\n",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				" are calculated <i>before</i> the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by the ",
				UI.FormatAsTool("Priority Tool", global::Action.Prioritize)
			});

			// Token: 0x0400A5D9 RID: 42457
			public static LocString MANAGEMENTMENU_CONSUMABLES = "Manage my Duplicants' diets and medications {Hotkey}";

			// Token: 0x0400A5DA RID: 42458
			public static LocString MANAGEMENTMENU_VITALS = "View my Duplicants' vitals {Hotkey}";

			// Token: 0x0400A5DB RID: 42459
			public static LocString MANAGEMENTMENU_RESEARCH = "View the Research Tree {Hotkey}";

			// Token: 0x0400A5DC RID: 42460
			public static LocString MANAGEMENTMENU_RESEARCH_NO_RESEARCH = "No active research projects";

			// Token: 0x0400A5DD RID: 42461
			public static LocString MANAGEMENTMENU_RESEARCH_CARD_NAME = "Currently researching: {0}";

			// Token: 0x0400A5DE RID: 42462
			public static LocString MANAGEMENTMENU_RESEARCH_ITEM_LINE = "• {0}";

			// Token: 0x0400A5DF RID: 42463
			public static LocString MANAGEMENTMENU_REQUIRES_RESEARCH = string.Concat(new string[]
			{
				"Build a Research Station to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.RESEARCHCENTER.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
				" of the Build Menu"
			});

			// Token: 0x0400A5E0 RID: 42464
			public static LocString MANAGEMENTMENU_DAILYREPORT = "View each cycle's Colony Report {Hotkey}";

			// Token: 0x0400A5E1 RID: 42465
			public static LocString MANAGEMENTMENU_CODEX = "Browse entries in my Database {Hotkey}";

			// Token: 0x0400A5E2 RID: 42466
			public static LocString MANAGEMENTMENU_SCHEDULE = "Adjust the colony's time usage {Hotkey}";

			// Token: 0x0400A5E3 RID: 42467
			public static LocString MANAGEMENTMENU_STARMAP = "Manage astronaut rocket missions {Hotkey}";

			// Token: 0x0400A5E4 RID: 42468
			public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE = string.Concat(new string[]
			{
				"Build a Telescope to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.TELESCOPE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
				" of the Build Menu"
			});

			// Token: 0x0400A5E5 RID: 42469
			public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE_CLUSTER = string.Concat(new string[]
			{
				"Build a Telescope to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.TELESCOPE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Rocketry Tab", global::Action.Plan14),
				" of the Build Menu"
			});

			// Token: 0x0400A5E6 RID: 42470
			public static LocString MANAGEMENTMENU_SKILLS = "Manage Duplicants' Skill assignments {Hotkey}";

			// Token: 0x0400A5E7 RID: 42471
			public static LocString MANAGEMENTMENU_REQUIRES_SKILL_STATION = string.Concat(new string[]
			{
				"Build a Printing Pod to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
				" of the Build Menu"
			});

			// Token: 0x0400A5E8 RID: 42472
			public static LocString MANAGEMENTMENU_PAUSEMENU = "Open the game menu {Hotkey}";

			// Token: 0x0400A5E9 RID: 42473
			public static LocString MANAGEMENTMENU_RESOURCES = "Open the resource management screen {Hotkey}";

			// Token: 0x0400A5EA RID: 42474
			public static LocString OPEN_CODEX_ENTRY = "View full entry in database";

			// Token: 0x0400A5EB RID: 42475
			public static LocString NO_CODEX_ENTRY = "No database entry available";

			// Token: 0x0400A5EC RID: 42476
			public static LocString OPEN_RESOURCE_INFO = "{0} of {1} available for the Duplicants on this asteroid to use\n\nClick to open Resources menu";

			// Token: 0x0400A5ED RID: 42477
			public static LocString CHANGE_OUTFIT = "Change this Duplicant's outfit";

			// Token: 0x0400A5EE RID: 42478
			public static LocString CHANGE_MATERIAL = "Change this building's construction material";

			// Token: 0x0400A5EF RID: 42479
			public static LocString METERSCREEN_AVGSTRESS = "Highest Stress: {0}";

			// Token: 0x0400A5F0 RID: 42480
			public static LocString METERSCREEN_MEALHISTORY = "Calories Available: {0}\n\nDuplicants consume a minimum of {1} calories each per cycle";

			// Token: 0x0400A5F1 RID: 42481
			public static LocString METERSCREEN_ELECTROBANK_JOULES = "Joules Available: {0}\n\nBionic Duplicants use a minimum of {1} each per cycle\n\nPower Banks Available: {2}\n";

			// Token: 0x0400A5F2 RID: 42482
			public static LocString METERSCREEN_POPULATION = "Population: {0}";

			// Token: 0x0400A5F3 RID: 42483
			public static LocString METERSCREEN_POPULATION_CLUSTER = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " Population: {1}\nTotal Population: {2}";

			// Token: 0x0400A5F4 RID: 42484
			public static LocString METERSCREEN_SICK_DUPES = "Sick Duplicants: {0}";

			// Token: 0x0400A5F5 RID: 42485
			public static LocString METERSCREEN_INVALID_FOOD_TYPE = "Invalid Food Type: {0}";

			// Token: 0x0400A5F6 RID: 42486
			public static LocString METERSCREEN_INVALID_ELECTROBANK_TYPE = "Invalid Power Bank Type: {0}";

			// Token: 0x0400A5F7 RID: 42487
			public static LocString PLAYBUTTON = "Start";

			// Token: 0x0400A5F8 RID: 42488
			public static LocString PAUSEBUTTON = "Pause";

			// Token: 0x0400A5F9 RID: 42489
			public static LocString PAUSE = "Pause {Hotkey}";

			// Token: 0x0400A5FA RID: 42490
			public static LocString UNPAUSE = "Unpause {Hotkey}";

			// Token: 0x0400A5FB RID: 42491
			public static LocString SPEEDBUTTON_SLOW = "Slow speed {Hotkey}";

			// Token: 0x0400A5FC RID: 42492
			public static LocString SPEEDBUTTON_MEDIUM = "Medium speed {Hotkey}";

			// Token: 0x0400A5FD RID: 42493
			public static LocString SPEEDBUTTON_FAST = "Fast speed {Hotkey}";

			// Token: 0x0400A5FE RID: 42494
			public static LocString RED_ALERT_TITLE = "Toggle Red Alert";

			// Token: 0x0400A5FF RID: 42495
			public static LocString RED_ALERT_CONTENT = "Duplicants will work, ignoring schedules and their basic needs\n\nUse in case of emergency";

			// Token: 0x0400A600 RID: 42496
			public static LocString DISINFECTBUTTON = "Disinfect buildings {Hotkey}";

			// Token: 0x0400A601 RID: 42497
			public static LocString MOPBUTTON = "Mop liquid spills {Hotkey}";

			// Token: 0x0400A602 RID: 42498
			public static LocString DIGBUTTON = "Set dig errands {Hotkey}";

			// Token: 0x0400A603 RID: 42499
			public static LocString CANCELBUTTON = "Cancel errands {Hotkey}";

			// Token: 0x0400A604 RID: 42500
			public static LocString DECONSTRUCTBUTTON = "Demolish buildings {Hotkey}";

			// Token: 0x0400A605 RID: 42501
			public static LocString ATTACKBUTTON = "Attack poor, wild critters {Hotkey}";

			// Token: 0x0400A606 RID: 42502
			public static LocString CAPTUREBUTTON = "Capture critters {Hotkey}";

			// Token: 0x0400A607 RID: 42503
			public static LocString CLEARBUTTON = "Move debris into storage {Hotkey}";

			// Token: 0x0400A608 RID: 42504
			public static LocString HARVESTBUTTON = "Harvest plants {Hotkey}";

			// Token: 0x0400A609 RID: 42505
			public static LocString PRIORITIZEMAINBUTTON = "";

			// Token: 0x0400A60A RID: 42506
			public static LocString PRIORITIZEBUTTON = string.Concat(new string[]
			{
				"Set Building Priority {Hotkey}\n\nDuplicant Priorities",
				UI.PST_KEYWORD,
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities),
				" are calculated <i>before</i> the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by this tool"
			});

			// Token: 0x0400A60B RID: 42507
			public static LocString CLEANUPMAINBUTTON = "Mop and sweep messy floors {Hotkey}";

			// Token: 0x0400A60C RID: 42508
			public static LocString CANCELDECONSTRUCTIONBUTTON = "Cancel queued orders or deconstruct existing buildings {Hotkey}";

			// Token: 0x0400A60D RID: 42509
			public static LocString HELP_ROTATE_KEY = "Press " + UI.FormatAsHotKey(global::Action.RotateBuilding) + " to Rotate";

			// Token: 0x0400A60E RID: 42510
			public static LocString HELP_BUILDLOCATION_INVALID_CELL = "Invalid Cell";

			// Token: 0x0400A60F RID: 42511
			public static LocString HELP_BUILDLOCATION_MISSING_TELEPAD = "World has no " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " or " + BUILDINGS.PREFABS.EXOBASEHEADQUARTERS.NAME;

			// Token: 0x0400A610 RID: 42512
			public static LocString HELP_BUILDLOCATION_FLOOR = "Must be built on solid ground";

			// Token: 0x0400A611 RID: 42513
			public static LocString HELP_BUILDLOCATION_WALL = "Must be built against a wall";

			// Token: 0x0400A612 RID: 42514
			public static LocString HELP_BUILDLOCATION_BACK_WALL_REQUIRED = "Must be built against a back wall";

			// Token: 0x0400A613 RID: 42515
			public static LocString HELP_BUILDLOCATION_FLOOR_OR_ATTACHPOINT = "Must be built on solid ground or overlapping an {0}";

			// Token: 0x0400A614 RID: 42516
			public static LocString HELP_BUILDLOCATION_OCCUPIED = "Must be built in unoccupied space";

			// Token: 0x0400A615 RID: 42517
			public static LocString HELP_BUILDLOCATION_BURIEDOBJECT = "Cannot be built over buried objects";

			// Token: 0x0400A616 RID: 42518
			public static LocString HELP_BUILDLOCATION_CEILING = "Must be built on the ceiling";

			// Token: 0x0400A617 RID: 42519
			public static LocString HELP_BUILDLOCATION_INSIDEGROUND = "Must be built in the ground";

			// Token: 0x0400A618 RID: 42520
			public static LocString HELP_BUILDLOCATION_ATTACHPOINT = "Must be built overlapping a {0}";

			// Token: 0x0400A619 RID: 42521
			public static LocString HELP_BUILDLOCATION_SPACE = "Must be built on the surface in space";

			// Token: 0x0400A61A RID: 42522
			public static LocString HELP_BUILDLOCATION_CORNER = "Must be built in a corner";

			// Token: 0x0400A61B RID: 42523
			public static LocString HELP_BUILDLOCATION_CORNER_FLOOR = "Must be built in a corner on the ground";

			// Token: 0x0400A61C RID: 42524
			public static LocString HELP_BUILDLOCATION_BELOWROCKETCEILING = "Must be placed further from the edge of space";

			// Token: 0x0400A61D RID: 42525
			public static LocString HELP_BUILDLOCATION_ONROCKETENVELOPE = "Must be built on the interior wall of a rocket";

			// Token: 0x0400A61E RID: 42526
			public static LocString HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN = "Obstructed by a building";

			// Token: 0x0400A61F RID: 42527
			public static LocString HELP_BUILDLOCATION_NOT_IN_TILES = "Cannot be built inside tile";

			// Token: 0x0400A620 RID: 42528
			public static LocString HELP_BUILDLOCATION_GASPORTS_OVERLAP = "Gas ports cannot overlap";

			// Token: 0x0400A621 RID: 42529
			public static LocString HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP = "Liquid ports cannot overlap";

			// Token: 0x0400A622 RID: 42530
			public static LocString HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP = "Solid ports cannot overlap";

			// Token: 0x0400A623 RID: 42531
			public static LocString HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED = "Automation ports cannot overlap";

			// Token: 0x0400A624 RID: 42532
			public static LocString HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP = "Power connectors cannot overlap";

			// Token: 0x0400A625 RID: 42533
			public static LocString HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE = "Heavi-Watt connectors cannot be built inside tile";

			// Token: 0x0400A626 RID: 42534
			public static LocString HELP_BUILDLOCATION_WIRE_OBSTRUCTION = "Obstructed by Heavi-Watt Wire";

			// Token: 0x0400A627 RID: 42535
			public static LocString HELP_BUILDLOCATION_BACK_WALL = "Obstructed by back wall";

			// Token: 0x0400A628 RID: 42536
			public static LocString HELP_TUBELOCATION_NO_UTURNS = "Can't U-Turn";

			// Token: 0x0400A629 RID: 42537
			public static LocString HELP_TUBELOCATION_STRAIGHT_BRIDGES = "Can't Turn Here";

			// Token: 0x0400A62A RID: 42538
			public static LocString HELP_REQUIRES_ROOM = "Must be in a " + UI.PRE_KEYWORD + "Room" + UI.PST_KEYWORD;

			// Token: 0x0400A62B RID: 42539
			public static LocString OXYGENOVERLAYSTRING = "Displays ambient oxygen density {Hotkey}";

			// Token: 0x0400A62C RID: 42540
			public static LocString POWEROVERLAYSTRING = "Displays power grid components {Hotkey}";

			// Token: 0x0400A62D RID: 42541
			public static LocString TEMPERATUREOVERLAYSTRING = "Displays ambient temperature {Hotkey}";

			// Token: 0x0400A62E RID: 42542
			public static LocString HEATFLOWOVERLAYSTRING = "Displays comfortable temperatures for Duplicants {Hotkey}";

			// Token: 0x0400A62F RID: 42543
			public static LocString SUITOVERLAYSTRING = "Displays Exosuits and related buildings {Hotkey}";

			// Token: 0x0400A630 RID: 42544
			public static LocString LOGICOVERLAYSTRING = "Displays automation grid components {Hotkey}";

			// Token: 0x0400A631 RID: 42545
			public static LocString ROOMSOVERLAYSTRING = "Displays special purpose rooms and bonuses {Hotkey}";

			// Token: 0x0400A632 RID: 42546
			public static LocString JOULESOVERLAYSTRING = "Displays the thermal energy in each cell";

			// Token: 0x0400A633 RID: 42547
			public static LocString LIGHTSOVERLAYSTRING = "Displays the visibility radius of light sources {Hotkey}";

			// Token: 0x0400A634 RID: 42548
			public static LocString LIQUIDVENTOVERLAYSTRING = "Displays liquid pipe system components {Hotkey}";

			// Token: 0x0400A635 RID: 42549
			public static LocString GASVENTOVERLAYSTRING = "Displays gas pipe system components {Hotkey}";

			// Token: 0x0400A636 RID: 42550
			public static LocString DECOROVERLAYSTRING = "Displays areas with Morale-boosting decor values {Hotkey}";

			// Token: 0x0400A637 RID: 42551
			public static LocString PRIORITIESOVERLAYSTRING = "Displays work priority values {Hotkey}";

			// Token: 0x0400A638 RID: 42552
			public static LocString DISEASEOVERLAYSTRING = "Displays areas of disease risk {Hotkey}";

			// Token: 0x0400A639 RID: 42553
			public static LocString NOISE_POLLUTION_OVERLAY_STRING = "Displays ambient noise levels {Hotkey}";

			// Token: 0x0400A63A RID: 42554
			public static LocString CROPS_OVERLAY_STRING = "Displays plant growth progress {Hotkey}";

			// Token: 0x0400A63B RID: 42555
			public static LocString CONVEYOR_OVERLAY_STRING = "Displays conveyor transport components {Hotkey}";

			// Token: 0x0400A63C RID: 42556
			public static LocString TILEMODE_OVERLAY_STRING = "Displays material information {Hotkey}";

			// Token: 0x0400A63D RID: 42557
			public static LocString REACHABILITYOVERLAYSTRING = "Displays areas accessible by Duplicants";

			// Token: 0x0400A63E RID: 42558
			public static LocString RADIATIONOVERLAYSTRING = "Displays radiation levels {Hotkey}";

			// Token: 0x0400A63F RID: 42559
			public static LocString ENERGYREQUIRED = UI.FormatAsLink("Power", "POWER") + " Required";

			// Token: 0x0400A640 RID: 42560
			public static LocString ENERGYGENERATED = UI.FormatAsLink("Power", "POWER") + " Produced";

			// Token: 0x0400A641 RID: 42561
			public static LocString INFOPANEL = "The Info Panel contains an overview of the basic information about my Duplicant";

			// Token: 0x0400A642 RID: 42562
			public static LocString VITALSPANEL = "The Vitals Panel monitors the status and well being of my Duplicant";

			// Token: 0x0400A643 RID: 42563
			public static LocString STRESSPANEL = "The Stress Panel offers a detailed look at what is affecting my Duplicant psychologically";

			// Token: 0x0400A644 RID: 42564
			public static LocString STATSPANEL = "The Stats Panel gives me an overview of my Duplicant's individual stats";

			// Token: 0x0400A645 RID: 42565
			public static LocString ITEMSPANEL = "The Items Panel displays everything this Duplicant is in possession of";

			// Token: 0x0400A646 RID: 42566
			public static LocString STRESSDESCRIPTION = string.Concat(new string[]
			{
				"Accommodate my Duplicant's needs to manage their ",
				UI.FormatAsLink("Stress", "STRESS"),
				".\n\nLow ",
				UI.FormatAsLink("Stress", "STRESS"),
				" can provide a productivity boost, while high ",
				UI.FormatAsLink("Stress", "STRESS"),
				" can impair production or even lead to a nervous breakdown."
			});

			// Token: 0x0400A647 RID: 42567
			public static LocString ALERTSTOOLTIP = "Alerts provide important information about what's happening in the colony right now";

			// Token: 0x0400A648 RID: 42568
			public static LocString MESSAGESTOOLTIP = "Messages are events that have happened and tips to help me manage my colony";

			// Token: 0x0400A649 RID: 42569
			public static LocString NEXTMESSAGESTOOLTIP = "Next message";

			// Token: 0x0400A64A RID: 42570
			public static LocString CLOSETOOLTIP = "Close";

			// Token: 0x0400A64B RID: 42571
			public static LocString DISMISSMESSAGE = "Dismiss message";

			// Token: 0x0400A64C RID: 42572
			public static LocString RECIPE_QUEUE = "Queue {0} for continuous fabrication";

			// Token: 0x0400A64D RID: 42573
			public static LocString RED_ALERT_BUTTON_ON = "Enable Red Alert";

			// Token: 0x0400A64E RID: 42574
			public static LocString RED_ALERT_BUTTON_OFF = "Disable Red Alert";

			// Token: 0x0400A64F RID: 42575
			public static LocString JOBSSCREEN_PRIORITY = "High priority tasks are always performed before low priority tasks.\n\nHowever, a busy Duplicant will continue to work on their current work errand until it's complete, even if a more important errand becomes available.";

			// Token: 0x0400A650 RID: 42576
			public static LocString JOBSSCREEN_ATTRIBUTES = "The following attributes affect a Duplicant's efficiency at this errand:";

			// Token: 0x0400A651 RID: 42577
			public static LocString JOBSSCREEN_CANNOTPERFORMTASK = "{0} cannot perform this errand.";

			// Token: 0x0400A652 RID: 42578
			public static LocString JOBSSCREEN_RELEVANT_ATTRIBUTES = "Relevant Attributes:";

			// Token: 0x0400A653 RID: 42579
			public static LocString SORTCOLUMN = UI.CLICK(UI.ClickType.Click) + " to sort";

			// Token: 0x0400A654 RID: 42580
			public static LocString NOMATERIAL = "Not enough materials";

			// Token: 0x0400A655 RID: 42581
			public static LocString SELECTAMATERIAL = "There are insufficient materials to construct this building";

			// Token: 0x0400A656 RID: 42582
			public static LocString EDITNAME = "Give this Duplicant a new name";

			// Token: 0x0400A657 RID: 42583
			public static LocString RANDOMIZENAME = "Randomize this Duplicant's name";

			// Token: 0x0400A658 RID: 42584
			public static LocString EDITNAMEGENERIC = "Rename {0}";

			// Token: 0x0400A659 RID: 42585
			public static LocString EDITNAMEROCKET = "Rename this rocket";

			// Token: 0x0400A65A RID: 42586
			public static LocString BASE_VALUE = "Base Value";

			// Token: 0x0400A65B RID: 42587
			public static LocString MATIERIAL_MOD = "Made out of {0}";

			// Token: 0x0400A65C RID: 42588
			public static LocString VITALS_CHECKBOX_TEMPERATURE = string.Concat(new string[]
			{
				"This plant's internal ",
				UI.PRE_KEYWORD,
				"Temperature",
				UI.PST_KEYWORD,
				" is <b>{temperature}</b>"
			});

			// Token: 0x0400A65D RID: 42589
			public static LocString VITALS_CHECKBOX_PRESSURE = string.Concat(new string[]
			{
				"The current ",
				UI.PRE_KEYWORD,
				"Gas",
				UI.PST_KEYWORD,
				" pressure is <b>{pressure}</b>"
			});

			// Token: 0x0400A65E RID: 42590
			public static LocString VITALS_CHECKBOX_ATMOSPHERE = "This plant is immersed in {element}";

			// Token: 0x0400A65F RID: 42591
			public static LocString VITALS_CHECKBOX_ILLUMINATION_DARK = "This plant is currently in the dark";

			// Token: 0x0400A660 RID: 42592
			public static LocString VITALS_CHECKBOX_ILLUMINATION_LIGHT = "This plant is currently lit";

			// Token: 0x0400A661 RID: 42593
			public static LocString VITALS_CHECKBOX_SPACETREE_ILLUMINATION_DARK = "This plant must be lit in order to produce " + UI.PRE_KEYWORD + "Nectar" + UI.PST_KEYWORD;

			// Token: 0x0400A662 RID: 42594
			public static LocString VITALS_CHECKBOX_SPACETREE_ILLUMINATION_LIGHT = string.Concat(new string[]
			{
				"This plant is currently lit, and will produce ",
				UI.PRE_KEYWORD,
				"Nectar",
				UI.PST_KEYWORD,
				" when fully grown"
			});

			// Token: 0x0400A663 RID: 42595
			public static LocString VITALS_CHECKBOX_FERTILIZER = string.Concat(new string[]
			{
				"<b>{mass}</b> of ",
				UI.PRE_KEYWORD,
				"Fertilizer",
				UI.PST_KEYWORD,
				" is currently available"
			});

			// Token: 0x0400A664 RID: 42596
			public static LocString VITALS_CHECKBOX_IRRIGATION = string.Concat(new string[]
			{
				"<b>{mass}</b> of ",
				UI.PRE_KEYWORD,
				"Liquid",
				UI.PST_KEYWORD,
				" is currently available"
			});

			// Token: 0x0400A665 RID: 42597
			public static LocString VITALS_CHECKBOX_SUBMERGED_TRUE = "This plant is fully submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PRE_KEYWORD;

			// Token: 0x0400A666 RID: 42598
			public static LocString VITALS_CHECKBOX_SUBMERGED_FALSE = "This plant must be submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

			// Token: 0x0400A667 RID: 42599
			public static LocString VITALS_CHECKBOX_DROWNING_TRUE = "This plant is not drowning";

			// Token: 0x0400A668 RID: 42600
			public static LocString VITALS_CHECKBOX_DROWNING_FALSE = "This plant is drowning in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

			// Token: 0x0400A669 RID: 42601
			public static LocString VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL = "This plant is housed in an operational farm plot";

			// Token: 0x0400A66A RID: 42602
			public static LocString VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL = "This plant is not housed in an operational farm plot";

			// Token: 0x0400A66B RID: 42603
			public static LocString VITALS_CHECKBOX_RADIATION = string.Concat(new string[]
			{
				"This plant is sitting in <b>{rads}</b> of ambient ",
				UI.PRE_KEYWORD,
				"Radiation",
				UI.PST_KEYWORD,
				". It needs between {minRads} and {maxRads} to grow"
			});

			// Token: 0x0400A66C RID: 42604
			public static LocString VITALS_CHECKBOX_RADIATION_NO_MIN = string.Concat(new string[]
			{
				"This plant is sitting in <b>{rads}</b> of ambient ",
				UI.PRE_KEYWORD,
				"Radiation",
				UI.PST_KEYWORD,
				". It needs less than {maxRads} to grow"
			});

			// Token: 0x0400A66D RID: 42605
			public static LocString VITALS_CHECKBOX_ENTITY_CONSUMER_REQUIREMENTS = string.Concat(new string[]
			{
				"This plant must consume ",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				" in order to grow"
			});

			// Token: 0x0400A66E RID: 42606
			public static LocString VITALS_CHECKBOX_ENTITY_CONSUMER_SATISFIED = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " consumed";

			// Token: 0x0400A66F RID: 42607
			public static LocString VITALS_CHECKBOX_ENTITY_CONSUMER_UNSATISFIED = "Awaiting prey";

			// Token: 0x0400A670 RID: 42608
			public static LocString VITALS_CHECKBOX_POLLINATED = string.Concat(new string[]
			{
				"This plant was recently pollinated by a ",
				UI.PRE_KEYWORD,
				"Critter",
				UI.PST_KEYWORD,
				" "
			});

			// Token: 0x0400A671 RID: 42609
			public static LocString VITALS_CHECKBOX_UNPOLLINATED = string.Concat(new string[]
			{
				"This plant must be pollinated by a ",
				UI.PRE_KEYWORD,
				"Critter",
				UI.PST_KEYWORD,
				"{0}"
			});
		}

		// Token: 0x0200252F RID: 9519
		public class CLUSTERMAP
		{
			// Token: 0x0400A672 RID: 42610
			public static LocString PLANETOID = "Planetoid";

			// Token: 0x0400A673 RID: 42611
			public static LocString PLANETOID_KEYWORD = UI.PRE_KEYWORD + UI.CLUSTERMAP.PLANETOID + UI.PST_KEYWORD;

			// Token: 0x0400A674 RID: 42612
			public static LocString TITLE = "STARMAP";

			// Token: 0x0400A675 RID: 42613
			public static LocString LANDING_SITES = "LANDING SITES";

			// Token: 0x0400A676 RID: 42614
			public static LocString DESTINATION = "DESTINATION";

			// Token: 0x0400A677 RID: 42615
			public static LocString OCCUPANTS = "CREW";

			// Token: 0x0400A678 RID: 42616
			public static LocString ELEMENTS = "ELEMENTS";

			// Token: 0x0400A679 RID: 42617
			public static LocString UNKNOWN_DESTINATION = "Unknown";

			// Token: 0x0400A67A RID: 42618
			public static LocString TILES = "Starmap Hexes";

			// Token: 0x0400A67B RID: 42619
			public static LocString TILES_PER_CYCLE = "Starmap hexes per cycle";

			// Token: 0x0400A67C RID: 42620
			public static LocString CHANGE_DESTINATION = UI.CLICK(UI.ClickType.Click) + " to change destination";

			// Token: 0x0400A67D RID: 42621
			public static LocString SELECT_DESTINATION = "Select a new destination on the map";

			// Token: 0x0400A67E RID: 42622
			public static LocString TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR = "Cannot travel to this hex until it has been analyzed\n\nSpace can be analyzed with a " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " or " + BUILDINGS.PREFABS.SCANNERMODULE.NAME;

			// Token: 0x0400A67F RID: 42623
			public static LocString TOOLTIP_INVALID_DESTINATION_NO_PATH = string.Concat(new string[]
			{
				"There is no navigable rocket path to this ",
				UI.CLUSTERMAP.PLANETOID_KEYWORD,
				"\n\nSpace can be analyzed with a ",
				BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME,
				" or ",
				BUILDINGS.PREFABS.SCANNERMODULE.NAME,
				" to clear the way"
			});

			// Token: 0x0400A680 RID: 42624
			public static LocString TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD = string.Concat(new string[]
			{
				"There is no ",
				BUILDINGS.PREFABS.LAUNCHPAD.NAME,
				" on this ",
				UI.CLUSTERMAP.PLANETOID_KEYWORD,
				" for a rocket to land on\n\nSend a ",
				BUILDINGS.PREFABS.PIONEERMODULE.NAME,
				" or ",
				BUILDINGS.PREFABS.SCOUTMODULE.NAME,
				" to its orbit to deploy a scout and make first contact"
			});

			// Token: 0x0400A681 RID: 42625
			public static LocString TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID = "Must select a " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " destination";

			// Token: 0x0400A682 RID: 42626
			public static LocString TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE = "This destination is further away than this rocket is currently equipped to go\n\n    • Current Range: {0} Hexes\n    • Potential " + DUPLICANTS.ATTRIBUTES.FUELRANGEPERKILOGRAM.NAME + ": {1} Hexes";

			// Token: 0x0400A683 RID: 42627
			public static LocString TOOLTIP_INVALID_METEOR_TARGET = "This destination does not have an impactor asteroid to target";

			// Token: 0x0400A684 RID: 42628
			public static LocString TOOLTIP_HIDDEN_HEX = "???";

			// Token: 0x0400A685 RID: 42629
			public static LocString TOOLTIP_PEEKED_HEX_WITH_OBJECT = "UNKNOWN OBJECT DETECTED!";

			// Token: 0x0400A686 RID: 42630
			public static LocString TOOLTIP_EMPTY_HEX = "EMPTY SPACE";

			// Token: 0x0400A687 RID: 42631
			public static LocString TOOLTIP_PATH_LENGTH = "Trip Distance: {0}/{1}";

			// Token: 0x0400A688 RID: 42632
			public static LocString TOOLTIP_PATH_LENGTH_RETURN = "Trip Distance: {0}/{1} (Return Trip)";

			// Token: 0x02002F32 RID: 12082
			public class STATUS
			{
				// Token: 0x0400CB3F RID: 52031
				public static LocString NORMAL = "Normal";

				// Token: 0x02003C1A RID: 15386
				public class ROCKET
				{
					// Token: 0x0400EF29 RID: 61225
					public static LocString GROUNDED = "Normal";

					// Token: 0x0400EF2A RID: 61226
					public static LocString TRAVELING = "Traveling";

					// Token: 0x0400EF2B RID: 61227
					public static LocString STRANDED = "Stranded";

					// Token: 0x0400EF2C RID: 61228
					public static LocString IDLE = "Idle";
				}
			}

			// Token: 0x02002F33 RID: 12083
			public class ASTEROIDS
			{
				// Token: 0x02003C1B RID: 15387
				public class ELEMENT_AMOUNTS
				{
					// Token: 0x0400EF2D RID: 61229
					public static LocString LOTS = "Plentiful";

					// Token: 0x0400EF2E RID: 61230
					public static LocString SOME = "Significant amount";

					// Token: 0x0400EF2F RID: 61231
					public static LocString LITTLE = "Small amount";

					// Token: 0x0400EF30 RID: 61232
					public static LocString VERY_LITTLE = "Trace amount";
				}

				// Token: 0x02003C1C RID: 15388
				public class SURFACE_CONDITIONS
				{
					// Token: 0x0400EF31 RID: 61233
					public static LocString LIGHT = "Peak Light";

					// Token: 0x0400EF32 RID: 61234
					public static LocString RADIATION = "Cosmic Radiation";
				}
			}

			// Token: 0x02002F34 RID: 12084
			public class POI
			{
				// Token: 0x0400CB40 RID: 52032
				public static LocString TITLE = "POINT OF INTEREST";

				// Token: 0x0400CB41 RID: 52033
				public static LocString MASS_REMAINING = "<b>Total Mass Remaining</b>";

				// Token: 0x0400CB42 RID: 52034
				public static LocString ROCKETS_AT_THIS_LOCATION = "<b>Rockets at this location</b>";

				// Token: 0x0400CB43 RID: 52035
				public static LocString ARTIFACTS = "Artifact";

				// Token: 0x0400CB44 RID: 52036
				public static LocString ARTIFACTS_AVAILABLE = "Available";

				// Token: 0x0400CB45 RID: 52037
				public static LocString ARTIFACTS_DEPLETED = "Collected\nRecharge: {0}";
			}

			// Token: 0x02002F35 RID: 12085
			public class HEXCELL_INVENTORY
			{
				// Token: 0x0400CB46 RID: 52038
				public static LocString NAME = "Harvestable Resources";

				// Token: 0x0400CB47 RID: 52039
				public static LocString DESC = "Free-floating resources that can be gathered by a rocket equipped with a cargo module.\n\nAdditional resources may become available by " + UI.FormatAsLink("mining", "EXOBASESDLC1") + " other space POIs on the same hex cell.";

				// Token: 0x02003C1D RID: 15389
				public class UI_PANEL
				{
					// Token: 0x0400EF33 RID: 61235
					public static LocString TITLE = "CONTENTS";
				}
			}

			// Token: 0x02002F36 RID: 12086
			public class ROCKETS
			{
				// Token: 0x02003C1E RID: 15390
				public class SPEED
				{
					// Token: 0x0400EF34 RID: 61236
					public static LocString NAME = "Rocket Speed: ";

					// Token: 0x0400EF35 RID: 61237
					public static LocString TOOLTIP = "<b>Rocket speed</b> is calculated by dividing <b>engine power</b> by <b>burden</b>";

					// Token: 0x0400EF36 RID: 61238
					public static LocString PILOT_SPEED_MODIFIER = "\n\nRockets operating on autopilot will have a reduced speed\n\n<b>Rocket speed</b> can be increased by a Duplicant pilot's <b>Skill</b> and by a " + UI.PRE_KEYWORD + "Robo-Pilot" + UI.PST_KEYWORD;

					// Token: 0x0400EF37 RID: 61239
					public static LocString UNPILOTED_SPEED_TOOLTIP = "Rocket is operating on autopilot: -{speed_boost} speed";

					// Token: 0x0400EF38 RID: 61240
					public static LocString SUPERPILOTED_SPEED_TOOLTIP = "Multi-Piloted: +{speed_boost} speed boost";

					// Token: 0x0400EF39 RID: 61241
					public static LocString DUPEPILOT_SPEED_TOOLTIP = "Current Duplicant pilot <b>Skill</b>: +{speed_boost} speed boost";

					// Token: 0x0400EF3A RID: 61242
					public static LocString ROBO_PILOT_ONLY_SPEED_TOOLTIP = string.Concat(new string[]
					{
						"Piloted by a ",
						UI.PRE_KEYWORD,
						"Robo-Pilot",
						UI.PST_KEYWORD,
						": +0% speed boost"
					});

					// Token: 0x0400EF3B RID: 61243
					public static LocString DEAD_ROBO_PILOT_ONLY_SPEED_TOOLTIP = string.Concat(new string[]
					{
						UI.PRE_KEYWORD,
						"Robo-Pilot",
						UI.PST_KEYWORD,
						" has no ",
						UI.PRE_KEYWORD,
						"Data Banks",
						UI.PST_KEYWORD,
						". This rocket is stranded"
					});
				}

				// Token: 0x02003C1F RID: 15391
				public class FUEL_REMAINING
				{
					// Token: 0x0400EF3C RID: 61244
					public static LocString NAME = "Fuel Remaining: ";

					// Token: 0x0400EF3D RID: 61245
					public static LocString TOOLTIP = "This rocket has {0} fuel in its tank";
				}

				// Token: 0x02003C20 RID: 15392
				public class OXIDIZER_REMAINING
				{
					// Token: 0x0400EF3E RID: 61246
					public static LocString NAME = "Oxidizer Power Remaining: ";

					// Token: 0x0400EF3F RID: 61247
					public static LocString TOOLTIP = "This rocket has enough oxidizer in its tank for {0} of fuel";
				}

				// Token: 0x02003C21 RID: 15393
				public class RANGE
				{
					// Token: 0x0400EF40 RID: 61248
					public static LocString NAME = "Range Remaining: ";

					// Token: 0x0400EF41 RID: 61249
					public static LocString TOOLTIP = "<b>Range remaining</b> is calculated by dividing the lesser of <b>fuel remaining</b> and <b>oxidizer power remaining</b> by <b>fuel consumed per hex</b>";

					// Token: 0x0400EF42 RID: 61250
					public static LocString ROBO_PILOTED_TOOLTIP = string.Concat(new string[]
					{
						"\nRockets piloted by a ",
						UI.PRE_KEYWORD,
						"Robo-Pilot",
						UI.PST_KEYWORD,
						" can travel one hex per {0} ",
						UI.PRE_KEYWORD,
						"Data Banks",
						UI.PST_KEYWORD,
						"\n    • ",
						UI.PRE_KEYWORD,
						"Data Banks",
						UI.PST_KEYWORD,
						" Remaining: {1}"
					});
				}

				// Token: 0x02003C22 RID: 15394
				public class FUEL_PER_HEX
				{
					// Token: 0x0400EF43 RID: 61251
					public static LocString NAME = "Fuel consumed per Hex: {0}";

					// Token: 0x0400EF44 RID: 61252
					public static LocString TOOLTIP = "This rocket can travel one hex per {0} of fuel";
				}

				// Token: 0x02003C23 RID: 15395
				public class BURDEN_TOTAL
				{
					// Token: 0x0400EF45 RID: 61253
					public static LocString NAME = "Rocket burden: ";

					// Token: 0x0400EF46 RID: 61254
					public static LocString TOOLTIP = "The combined burden of all the modules in this rocket";
				}

				// Token: 0x02003C24 RID: 15396
				public class BURDEN_MODULE
				{
					// Token: 0x0400EF47 RID: 61255
					public static LocString NAME = "Module Burden: ";

					// Token: 0x0400EF48 RID: 61256
					public static LocString TOOLTIP = "The selected module adds {0} to the rocket's total " + DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME;
				}

				// Token: 0x02003C25 RID: 15397
				public class POWER_TOTAL
				{
					// Token: 0x0400EF49 RID: 61257
					public static LocString NAME = "Rocket engine power: ";

					// Token: 0x0400EF4A RID: 61258
					public static LocString TOOLTIP = "The total engine power added by all the modules in this rocket";
				}

				// Token: 0x02003C26 RID: 15398
				public class POWER_MODULE
				{
					// Token: 0x0400EF4B RID: 61259
					public static LocString NAME = "Module Engine Power: ";

					// Token: 0x0400EF4C RID: 61260
					public static LocString TOOLTIP = "The selected module adds {0} to the rocket's total " + DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME;
				}

				// Token: 0x02003C27 RID: 15399
				public class MODULE_STATS
				{
					// Token: 0x0400EF4D RID: 61261
					public static LocString NAME = "Module Stats: ";

					// Token: 0x0400EF4E RID: 61262
					public static LocString NAME_HEADER = "Module Stats";

					// Token: 0x0400EF4F RID: 61263
					public static LocString TOOLTIP = "Properties of the selected module";
				}

				// Token: 0x02003C28 RID: 15400
				public class MAX_MODULES
				{
					// Token: 0x0400EF50 RID: 61264
					public static LocString NAME = "Max Modules: ";

					// Token: 0x0400EF51 RID: 61265
					public static LocString TOOLTIP = "The {0} can support {1} rocket modules, plus itself";
				}

				// Token: 0x02003C29 RID: 15401
				public class MAX_HEIGHT
				{
					// Token: 0x0400EF52 RID: 61266
					public static LocString NAME = "Height: {0}/{1}";

					// Token: 0x0400EF53 RID: 61267
					public static LocString NAME_RAW = "Height: ";

					// Token: 0x0400EF54 RID: 61268
					public static LocString NAME_MAX_SUPPORTED = "Maximum supported rocket height: ";

					// Token: 0x0400EF55 RID: 61269
					public static LocString TOOLTIP = "The {0} can support a total rocket height {1}";
				}

				// Token: 0x02003C2A RID: 15402
				public class ARTIFACT_MODULE
				{
					// Token: 0x0400EF56 RID: 61270
					public static LocString EMPTY = "Empty";
				}
			}
		}

		// Token: 0x02002530 RID: 9520
		public class STARMAP
		{
			// Token: 0x0400A689 RID: 42633
			public static LocString TITLE = "STARMAP";

			// Token: 0x0400A68A RID: 42634
			public static LocString MANAGEMENT_BUTTON = "STARMAP";

			// Token: 0x0400A68B RID: 42635
			public static LocString SUBROW = "•  {0}";

			// Token: 0x0400A68C RID: 42636
			public static LocString UNKNOWN_DESTINATION = "Destination Unknown";

			// Token: 0x0400A68D RID: 42637
			public static LocString ANALYSIS_AMOUNT = "Analysis {0} Complete";

			// Token: 0x0400A68E RID: 42638
			public static LocString ANALYSIS_COMPLETE = "ANALYSIS COMPLETE";

			// Token: 0x0400A68F RID: 42639
			public static LocString NO_ANALYZABLE_DESTINATION_SELECTED = "No destination selected";

			// Token: 0x0400A690 RID: 42640
			public static LocString UNKNOWN_TYPE = "Type Unknown";

			// Token: 0x0400A691 RID: 42641
			public static LocString DISTANCE = "{0} km";

			// Token: 0x0400A692 RID: 42642
			public static LocString MODULE_MASS = "+ {0} t";

			// Token: 0x0400A693 RID: 42643
			public static LocString MODULE_STORAGE = "{0} / {1}";

			// Token: 0x0400A694 RID: 42644
			public static LocString ANALYSIS_DESCRIPTION = "Use a Telescope to analyze space destinations.\n\nCompleting analysis on an object will unlock rocket missions to that destination.";

			// Token: 0x0400A695 RID: 42645
			public static LocString RESEARCH_DESCRIPTION = "Gather Interstellar Research Data using Research Modules.";

			// Token: 0x0400A696 RID: 42646
			public static LocString ROCKET_RENAME_BUTTON_TOOLTIP = "Rename this rocket";

			// Token: 0x0400A697 RID: 42647
			public static LocString NO_ROCKETS_HELP_TEXT = "Rockets allow you to visit nearby celestial bodies.\n\nEach rocket must have a Command Module, an Engine, and Fuel.\n\nYou can also carry other modules that allow you to gather specific resources from the places you visit.\n\nRemember the more weight a rocket has, the more limited it'll be on the distance it can travel. You can add more fuel to fix that, but fuel will add weight as well.";

			// Token: 0x0400A698 RID: 42648
			public static LocString CONTAINER_REQUIRED = "{0} installation required to retrieve material";

			// Token: 0x0400A699 RID: 42649
			public static LocString CAN_CARRY_ELEMENT = "Gathered by: {1}";

			// Token: 0x0400A69A RID: 42650
			public static LocString CANT_CARRY_ELEMENT = "{0} installation required to retrieve material";

			// Token: 0x0400A69B RID: 42651
			public static LocString STATUS = "SELECTED";

			// Token: 0x0400A69C RID: 42652
			public static LocString DISTANCE_OVERLAY = "TOO FAR FOR THIS ROCKET";

			// Token: 0x0400A69D RID: 42653
			public static LocString COMPOSITION_UNDISCOVERED = "?????????";

			// Token: 0x0400A69E RID: 42654
			public static LocString COMPOSITION_UNDISCOVERED_TOOLTIP = "Further research required to identify resource\n\nSend a Research Module to this destination for more information";

			// Token: 0x0400A69F RID: 42655
			public static LocString COMPOSITION_UNDISCOVERED_AMOUNT = "???";

			// Token: 0x0400A6A0 RID: 42656
			public static LocString COMPOSITION_SMALL_AMOUNT = "Trace Amount";

			// Token: 0x0400A6A1 RID: 42657
			public static LocString CURRENT_MASS = "Current Mass";

			// Token: 0x0400A6A2 RID: 42658
			public static LocString CURRENT_MASS_TOOLTIP = "Warning: Missions to this destination will not return a full cargo load to avoid depleting the destination for future explorations\n\nDestination: {0} Resources Available\nRocket Capacity: {1}";

			// Token: 0x0400A6A3 RID: 42659
			public static LocString MAXIMUM_MASS = "Maximum Mass";

			// Token: 0x0400A6A4 RID: 42660
			public static LocString MINIMUM_MASS = "Minimum Mass";

			// Token: 0x0400A6A5 RID: 42661
			public static LocString MINIMUM_MASS_TOOLTIP = "This destination must retain at least this much mass in order to prevent depletion and allow the future regeneration of resources.\n\nDuplicants will always maintain a destination's minimum mass requirements, potentially returning with less cargo than their rocket can hold";

			// Token: 0x0400A6A6 RID: 42662
			public static LocString REPLENISH_RATE = "Replenished/Cycle:";

			// Token: 0x0400A6A7 RID: 42663
			public static LocString REPLENISH_RATE_TOOLTIP = "The rate at which this destination regenerates resources";

			// Token: 0x0400A6A8 RID: 42664
			public static LocString ROCKETLIST = "Rocket Hangar";

			// Token: 0x0400A6A9 RID: 42665
			public static LocString NO_ROCKETS_TITLE = "NO ROCKETS";

			// Token: 0x0400A6AA RID: 42666
			public static LocString ROCKET_COUNT = "ROCKETS: {0}";

			// Token: 0x0400A6AB RID: 42667
			public static LocString LAUNCH_MISSION = "LAUNCH MISSION";

			// Token: 0x0400A6AC RID: 42668
			public static LocString CANT_LAUNCH_MISSION = "CANNOT LAUNCH";

			// Token: 0x0400A6AD RID: 42669
			public static LocString LAUNCH_ROCKET = "Launch Rocket";

			// Token: 0x0400A6AE RID: 42670
			public static LocString LAND_ROCKET = "Land Rocket";

			// Token: 0x0400A6AF RID: 42671
			public static LocString SEE_ROCKETS_LIST = "See Rockets List";

			// Token: 0x0400A6B0 RID: 42672
			public static LocString DEFAULT_NAME = "Rocket";

			// Token: 0x0400A6B1 RID: 42673
			public static LocString ANALYZE_DESTINATION = "ANALYZE OBJECT";

			// Token: 0x0400A6B2 RID: 42674
			public static LocString SUSPEND_DESTINATION_ANALYSIS = "PAUSE ANALYSIS";

			// Token: 0x0400A6B3 RID: 42675
			public static LocString DESTINATIONTITLE = "Destination Status";

			// Token: 0x02002F37 RID: 12087
			public class DESTINATIONSTUDY
			{
				// Token: 0x0400CB48 RID: 52040
				public static LocString UPPERATMO = "Study upper atmosphere";

				// Token: 0x0400CB49 RID: 52041
				public static LocString LOWERATMO = "Study lower atmosphere";

				// Token: 0x0400CB4A RID: 52042
				public static LocString MAGNETICFIELD = "Study magnetic field";

				// Token: 0x0400CB4B RID: 52043
				public static LocString SURFACE = "Study surface";

				// Token: 0x0400CB4C RID: 52044
				public static LocString SUBSURFACE = "Study subsurface";
			}

			// Token: 0x02002F38 RID: 12088
			public class COMPONENT
			{
				// Token: 0x0400CB4D RID: 52045
				public static LocString FUEL_TANK = "Fuel Tank";

				// Token: 0x0400CB4E RID: 52046
				public static LocString ROCKET_ENGINE = "Rocket Engine";

				// Token: 0x0400CB4F RID: 52047
				public static LocString CARGO_BAY = "Cargo Bay";

				// Token: 0x0400CB50 RID: 52048
				public static LocString OXIDIZER_TANK = "Oxidizer Tank";
			}

			// Token: 0x02002F39 RID: 12089
			public class MISSION_STATUS
			{
				// Token: 0x0400CB51 RID: 52049
				public static LocString GROUNDED = "Grounded";

				// Token: 0x0400CB52 RID: 52050
				public static LocString LAUNCHING = "Launching";

				// Token: 0x0400CB53 RID: 52051
				public static LocString WAITING_TO_LAND = "Waiting To Land";

				// Token: 0x0400CB54 RID: 52052
				public static LocString LANDING = "Landing";

				// Token: 0x0400CB55 RID: 52053
				public static LocString UNDERWAY = "Underway";

				// Token: 0x0400CB56 RID: 52054
				public static LocString UNDERWAY_BOOSTED = "Underway <color=#5FDB37FF>(Boosted)</color>";

				// Token: 0x0400CB57 RID: 52055
				public static LocString DESTROYED = "Destroyed";

				// Token: 0x0400CB58 RID: 52056
				public static LocString GO = "ALL SYSTEMS GO";
			}

			// Token: 0x02002F3A RID: 12090
			public class LISTTITLES
			{
				// Token: 0x0400CB59 RID: 52057
				public static LocString MISSIONSTATUS = "Mission Status";

				// Token: 0x0400CB5A RID: 52058
				public static LocString LAUNCHCHECKLIST = "Launch Checklist";

				// Token: 0x0400CB5B RID: 52059
				public static LocString MAXRANGE = "Max Range";

				// Token: 0x0400CB5C RID: 52060
				public static LocString MASS = "Mass";

				// Token: 0x0400CB5D RID: 52061
				public static LocString STORAGE = "Storage";

				// Token: 0x0400CB5E RID: 52062
				public static LocString FUEL = "Fuel";

				// Token: 0x0400CB5F RID: 52063
				public static LocString OXIDIZER = "Oxidizer";

				// Token: 0x0400CB60 RID: 52064
				public static LocString PASSENGERS = "Passengers";

				// Token: 0x0400CB61 RID: 52065
				public static LocString RESEARCH = "Research";

				// Token: 0x0400CB62 RID: 52066
				public static LocString ARTIFACTS = "Artifacts";

				// Token: 0x0400CB63 RID: 52067
				public static LocString ANALYSIS = "Analysis";

				// Token: 0x0400CB64 RID: 52068
				public static LocString WORLDCOMPOSITION = "World Composition";

				// Token: 0x0400CB65 RID: 52069
				public static LocString RESOURCES = "Resources";

				// Token: 0x0400CB66 RID: 52070
				public static LocString MODULES = "Modules";

				// Token: 0x0400CB67 RID: 52071
				public static LocString TYPE = "Type";

				// Token: 0x0400CB68 RID: 52072
				public static LocString DISTANCE = "Distance";

				// Token: 0x0400CB69 RID: 52073
				public static LocString DESTINATION_MASS = "World Mass Available";

				// Token: 0x0400CB6A RID: 52074
				public static LocString STORAGECAPACITY = "Storage Capacity";
			}

			// Token: 0x02002F3B RID: 12091
			public class ROCKETWEIGHT
			{
				// Token: 0x0400CB6B RID: 52075
				public static LocString MASS = "Mass: ";

				// Token: 0x0400CB6C RID: 52076
				public static LocString MASSPENALTY = "Mass Penalty: ";

				// Token: 0x0400CB6D RID: 52077
				public static LocString CURRENTMASS = "Current Rocket Mass: ";

				// Token: 0x0400CB6E RID: 52078
				public static LocString CURRENTMASSPENALTY = "Current Weight Penalty: ";
			}

			// Token: 0x02002F3C RID: 12092
			public class DESTINATIONSELECTION
			{
				// Token: 0x0400CB6F RID: 52079
				public static LocString REACHABLE = "Destination set";

				// Token: 0x0400CB70 RID: 52080
				public static LocString UNREACHABLE = "Destination set";

				// Token: 0x0400CB71 RID: 52081
				public static LocString NOTSELECTED = "Destination set";
			}

			// Token: 0x02002F3D RID: 12093
			public class DESTINATIONSELECTION_TOOLTIP
			{
				// Token: 0x0400CB72 RID: 52082
				public static LocString REACHABLE = "Viable destination selected, ready for launch";

				// Token: 0x0400CB73 RID: 52083
				public static LocString UNREACHABLE = "The selected destination is beyond rocket reach";

				// Token: 0x0400CB74 RID: 52084
				public static LocString NOTSELECTED = "Select the rocket's Command Module to set a destination";
			}

			// Token: 0x02002F3E RID: 12094
			public class HASFOOD
			{
				// Token: 0x0400CB75 RID: 52085
				public static LocString NAME = "Food Loaded";

				// Token: 0x0400CB76 RID: 52086
				public static LocString TOOLTIP = "Sufficient food stores have been loaded, ready for launch";
			}

			// Token: 0x02002F3F RID: 12095
			public class HASSUIT
			{
				// Token: 0x0400CB77 RID: 52087
				public static LocString NAME = "Has " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400CB78 RID: 52088
				public static LocString TOOLTIP = "An " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME + " has been loaded";
			}

			// Token: 0x02002F40 RID: 12096
			public class NOSUIT
			{
				// Token: 0x0400CB79 RID: 52089
				public static LocString NAME = "Missing " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400CB7A RID: 52090
				public static LocString TOOLTIP = "Rocket cannot launch without an " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME + " loaded";
			}

			// Token: 0x02002F41 RID: 12097
			public class NOFOOD
			{
				// Token: 0x0400CB7B RID: 52091
				public static LocString NAME = "Insufficient Food";

				// Token: 0x0400CB7C RID: 52092
				public static LocString TOOLTIP = "Rocket cannot launch without adequate food stores for passengers";
			}

			// Token: 0x02002F42 RID: 12098
			public class CARGOEMPTY
			{
				// Token: 0x0400CB7D RID: 52093
				public static LocString NAME = "Emptied Cargo Bay";

				// Token: 0x0400CB7E RID: 52094
				public static LocString TOOLTIP = "Cargo Bays must be emptied of all materials before launch";
			}

			// Token: 0x02002F43 RID: 12099
			public class LAUNCHCHECKLIST
			{
				// Token: 0x0400CB7F RID: 52095
				public static LocString ASTRONAUT_TITLE = "Astronaut";

				// Token: 0x0400CB80 RID: 52096
				public static LocString HASASTRONAUT = "Astronaut ready for liftoff";

				// Token: 0x0400CB81 RID: 52097
				public static LocString ASTRONAUGHT = "No Astronaut assigned";

				// Token: 0x0400CB82 RID: 52098
				public static LocString INSTALLED = "Installed";

				// Token: 0x0400CB83 RID: 52099
				public static LocString INSTALLED_TOOLTIP = "A suitable {0} has been installed";

				// Token: 0x0400CB84 RID: 52100
				public static LocString REQUIRED = "Required";

				// Token: 0x0400CB85 RID: 52101
				public static LocString REQUIRED_TOOLTIP = "A {0} must be installed before launch";

				// Token: 0x0400CB86 RID: 52102
				public static LocString MISSING_TOOLTIP = "No {0} installed\n\nThis rocket cannot launch without a completed {0}";

				// Token: 0x0400CB87 RID: 52103
				public static LocString NO_DESTINATION = "No destination selected";

				// Token: 0x0400CB88 RID: 52104
				public static LocString MINIMUM_MASS = "Resources available {0}";

				// Token: 0x0400CB89 RID: 52105
				public static LocString RESOURCE_MASS_TOOLTIP = "{0} has {1} resources available\nThis rocket has capacity for {2}";

				// Token: 0x0400CB8A RID: 52106
				public static LocString INSUFFICENT_MASS_TOOLTIP = "Launching to this destination will not return a full cargo load";

				// Token: 0x02003C2B RID: 15403
				public class CONSTRUCTION_COMPLETE
				{
					// Token: 0x020040B8 RID: 16568
					public class STATUS
					{
						// Token: 0x0400FA3E RID: 64062
						public static LocString READY = "No active construction";

						// Token: 0x0400FA3F RID: 64063
						public static LocString FAILURE = "No active construction";

						// Token: 0x0400FA40 RID: 64064
						public static LocString WARNING = "No active construction";
					}

					// Token: 0x020040B9 RID: 16569
					public class TOOLTIP
					{
						// Token: 0x0400FA41 RID: 64065
						public static LocString READY = "Construction of all modules is complete";

						// Token: 0x0400FA42 RID: 64066
						public static LocString FAILURE = "In-progress module construction is preventing takeoff";

						// Token: 0x0400FA43 RID: 64067
						public static LocString WARNING = "Construction warning";
					}
				}

				// Token: 0x02003C2C RID: 15404
				public class PILOT_BOARDED
				{
					// Token: 0x0400EF57 RID: 61271
					public static LocString READY = "Pilot boarded";

					// Token: 0x0400EF58 RID: 61272
					public static LocString FAILURE = "Pilot boarded";

					// Token: 0x0400EF59 RID: 61273
					public static LocString WARNING = "Pilot boarded";

					// Token: 0x0400EF5A RID: 61274
					public static LocString ROBO_PILOT_WARNING = "Copilot boarded";

					// Token: 0x020040BA RID: 16570
					public class TOOLTIP
					{
						// Token: 0x0400FA44 RID: 64068
						public static LocString READY = "A Duplicant with the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill is currently onboard";

						// Token: 0x0400FA45 RID: 64069
						public static LocString FAILURE = "At least one crew member aboard the rocket must possess the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill to launch\n\nQualified Duplicants must be assigned to the rocket crew, and have access to the module's hatch";

						// Token: 0x0400FA46 RID: 64070
						public static LocString WARNING = "Pilot warning";

						// Token: 0x0400FA47 RID: 64071
						public static LocString ROBO_PILOT_WARNING = string.Concat(new string[]
						{
							"This rocket is being piloted by a ",
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							"\n\nThere are no Duplicants with the ",
							DUPLICANTS.ROLES.ROCKETPILOT.NAME,
							" skill currently onboard\n\nQualified Duplicants must be assigned to the rocket crew, and have access to the module's hatch"
						});
					}
				}

				// Token: 0x02003C2D RID: 15405
				public class CREW_BOARDED
				{
					// Token: 0x0400EF5B RID: 61275
					public static LocString READY = "All crew boarded";

					// Token: 0x0400EF5C RID: 61276
					public static LocString FAILURE = "All crew boarded";

					// Token: 0x0400EF5D RID: 61277
					public static LocString WARNING = "All crew boarded";

					// Token: 0x020040BB RID: 16571
					public class TOOLTIP
					{
						// Token: 0x0400FA48 RID: 64072
						public static LocString READY = "All Duplicants assigned to the rocket crew are boarded and ready for launch\n\n    • {0}/{1} Boarded";

						// Token: 0x0400FA49 RID: 64073
						public static LocString FAILURE = "No crew members have boarded this rocket\n\nDuplicants must be assigned to the rocket crew and have access to the module's hatch to board\n\n    • {0}/{1} Boarded";

						// Token: 0x0400FA4A RID: 64074
						public static LocString WARNING = "Some Duplicants assigned to this rocket crew have not yet boarded\n    • {0}/{1} Boarded";

						// Token: 0x0400FA4B RID: 64075
						public static LocString NONE = "There are no Duplicants assigned to this rocket crew\n    • {0}/{1} Boarded";
					}
				}

				// Token: 0x02003C2E RID: 15406
				public class NO_EXTRA_PASSENGERS
				{
					// Token: 0x0400EF5E RID: 61278
					public static LocString READY = "Non-crew exited";

					// Token: 0x0400EF5F RID: 61279
					public static LocString FAILURE = "Non-crew exited";

					// Token: 0x0400EF60 RID: 61280
					public static LocString WARNING = "Non-crew exited";

					// Token: 0x020040BC RID: 16572
					public class TOOLTIP
					{
						// Token: 0x0400FA4C RID: 64076
						public static LocString READY = "All non-crew Duplicants have disembarked";

						// Token: 0x0400FA4D RID: 64077
						public static LocString FAILURE = "Non-crew Duplicants must exit the rocket before launch";

						// Token: 0x0400FA4E RID: 64078
						public static LocString WARNING = "Non-crew warning";
					}
				}

				// Token: 0x02003C2F RID: 15407
				public class FLIGHT_PATH_CLEAR
				{
					// Token: 0x020040BD RID: 16573
					public class STATUS
					{
						// Token: 0x0400FA4F RID: 64079
						public static LocString READY = "Clear launch path";

						// Token: 0x0400FA50 RID: 64080
						public static LocString FAILURE = "Clear launch path";

						// Token: 0x0400FA51 RID: 64081
						public static LocString WARNING = "Clear launch path";
					}

					// Token: 0x020040BE RID: 16574
					public class TOOLTIP
					{
						// Token: 0x0400FA52 RID: 64082
						public static LocString READY = "The rocket's launch path is clear for takeoff";

						// Token: 0x0400FA53 RID: 64083
						public static LocString FAILURE = "This rocket does not have a clear line of sight to space, preventing launch\n\nThe rocket's launch path can be cleared by excavating undug tiles and deconstructing any buildings above the rocket";

						// Token: 0x0400FA54 RID: 64084
						public static LocString WARNING = "";
					}
				}

				// Token: 0x02003C30 RID: 15408
				public class HAS_FUEL_TANK
				{
					// Token: 0x020040BF RID: 16575
					public class STATUS
					{
						// Token: 0x0400FA55 RID: 64085
						public static LocString READY = "Fuel Tank";

						// Token: 0x0400FA56 RID: 64086
						public static LocString FAILURE = "Fuel Tank";

						// Token: 0x0400FA57 RID: 64087
						public static LocString WARNING = "Fuel Tank";
					}

					// Token: 0x020040C0 RID: 16576
					public class TOOLTIP
					{
						// Token: 0x0400FA58 RID: 64088
						public static LocString READY = "A fuel tank has been installed";

						// Token: 0x0400FA59 RID: 64089
						public static LocString FAILURE = "No fuel tank installed\n\nThis rocket cannot launch without a completed fuel tank";

						// Token: 0x0400FA5A RID: 64090
						public static LocString WARNING = "Fuel tank warning";
					}
				}

				// Token: 0x02003C31 RID: 15409
				public class HAS_ENGINE
				{
					// Token: 0x020040C1 RID: 16577
					public class STATUS
					{
						// Token: 0x0400FA5B RID: 64091
						public static LocString READY = "Engine";

						// Token: 0x0400FA5C RID: 64092
						public static LocString FAILURE = "Engine";

						// Token: 0x0400FA5D RID: 64093
						public static LocString WARNING = "Engine";
					}

					// Token: 0x020040C2 RID: 16578
					public class TOOLTIP
					{
						// Token: 0x0400FA5E RID: 64094
						public static LocString READY = "A suitable engine has been installed";

						// Token: 0x0400FA5F RID: 64095
						public static LocString FAILURE = "No engine installed\n\nThis rocket cannot launch without a completed engine";

						// Token: 0x0400FA60 RID: 64096
						public static LocString WARNING = "Engine warning";
					}
				}

				// Token: 0x02003C32 RID: 15410
				public class HAS_NOSECONE
				{
					// Token: 0x020040C3 RID: 16579
					public class STATUS
					{
						// Token: 0x0400FA61 RID: 64097
						public static LocString READY = "Nosecone";

						// Token: 0x0400FA62 RID: 64098
						public static LocString FAILURE = "Nosecone";

						// Token: 0x0400FA63 RID: 64099
						public static LocString WARNING = "Nosecone";
					}

					// Token: 0x020040C4 RID: 16580
					public class TOOLTIP
					{
						// Token: 0x0400FA64 RID: 64100
						public static LocString READY = "A suitable nosecone has been installed";

						// Token: 0x0400FA65 RID: 64101
						public static LocString FAILURE = "No nosecone installed\n\nThis rocket cannot launch without a completed nosecone";

						// Token: 0x0400FA66 RID: 64102
						public static LocString WARNING = "Nosecone warning";
					}
				}

				// Token: 0x02003C33 RID: 15411
				public class HAS_CARGO_BAY_FOR_NOSECONE_HARVEST
				{
					// Token: 0x020040C5 RID: 16581
					public class STATUS
					{
						// Token: 0x0400FA67 RID: 64103
						public static LocString READY = "Drillcone Cargo Bay";

						// Token: 0x0400FA68 RID: 64104
						public static LocString FAILURE = "Drillcone Cargo Bay";

						// Token: 0x0400FA69 RID: 64105
						public static LocString WARNING = "Drillcone Cargo Bay";
					}

					// Token: 0x020040C6 RID: 16582
					public class TOOLTIP
					{
						// Token: 0x0400FA6A RID: 64106
						public static LocString READY = "A suitable cargo bay has been installed";

						// Token: 0x0400FA6B RID: 64107
						public static LocString FAILURE = "No cargo bay installed\n\nThis rocket has a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + " installed but nowhere to store the materials";

						// Token: 0x0400FA6C RID: 64108
						public static LocString WARNING = "No cargo bay installed\n\nThis rocket has a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + " installed but nowhere to store the materials";
					}
				}

				// Token: 0x02003C34 RID: 15412
				public class HAS_CONTROLSTATION
				{
					// Token: 0x020040C7 RID: 16583
					public class STATUS
					{
						// Token: 0x0400FA6D RID: 64109
						public static LocString READY = "Control Station";

						// Token: 0x0400FA6E RID: 64110
						public static LocString FAILURE = "Control Station";

						// Token: 0x0400FA6F RID: 64111
						public static LocString WARNING = "Control Station";
					}

					// Token: 0x020040C8 RID: 16584
					public class TOOLTIP
					{
						// Token: 0x0400FA70 RID: 64112
						public static LocString READY = "The control station is installed and waiting for the pilot";

						// Token: 0x0400FA71 RID: 64113
						public static LocString FAILURE = "No control station\n\nA new Rocket Control Station must be installed inside the rocket";

						// Token: 0x0400FA72 RID: 64114
						public static LocString WARNING = "Control Station warning";

						// Token: 0x0400FA73 RID: 64115
						public static LocString WARNING_ROBO_PILOT = "No control station\n\nThis rocket is being piloted by a Robo-Pilot Module";
					}
				}

				// Token: 0x02003C35 RID: 15413
				public class LOADING_COMPLETE
				{
					// Token: 0x020040C9 RID: 16585
					public class STATUS
					{
						// Token: 0x0400FA74 RID: 64116
						public static LocString READY = "Cargo Loading Complete";

						// Token: 0x0400FA75 RID: 64117
						public static LocString FAILURE = "";

						// Token: 0x0400FA76 RID: 64118
						public static LocString WARNING = "Cargo Loading Complete";
					}

					// Token: 0x020040CA RID: 16586
					public class TOOLTIP
					{
						// Token: 0x0400FA77 RID: 64119
						public static LocString READY = "All possible loading and unloading has been completed";

						// Token: 0x0400FA78 RID: 64120
						public static LocString FAILURE = "";

						// Token: 0x0400FA79 RID: 64121
						public static LocString WARNING = "The " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " could still transfer cargo to or from this rocket";
					}
				}

				// Token: 0x02003C36 RID: 15414
				public class CARGO_TRANSFER_COMPLETE
				{
					// Token: 0x020040CB RID: 16587
					public class STATUS
					{
						// Token: 0x0400FA7A RID: 64122
						public static LocString READY = "Cargo Transfer Complete";

						// Token: 0x0400FA7B RID: 64123
						public static LocString FAILURE = "";

						// Token: 0x0400FA7C RID: 64124
						public static LocString WARNING = "Cargo Transfer Complete";
					}

					// Token: 0x020040CC RID: 16588
					public class TOOLTIP
					{
						// Token: 0x0400FA7D RID: 64125
						public static LocString READY = "All possible loading and unloading has been completed";

						// Token: 0x0400FA7E RID: 64126
						public static LocString FAILURE = "";

						// Token: 0x0400FA7F RID: 64127
						public static LocString WARNING = "The " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " could still transfer cargo to or from this rocket";
					}
				}

				// Token: 0x02003C37 RID: 15415
				public class INTERNAL_CONSTRUCTION_COMPLETE
				{
					// Token: 0x020040CD RID: 16589
					public class STATUS
					{
						// Token: 0x0400FA80 RID: 64128
						public static LocString READY = "Landers Ready";

						// Token: 0x0400FA81 RID: 64129
						public static LocString FAILURE = "Landers Ready";

						// Token: 0x0400FA82 RID: 64130
						public static LocString WARNING = "";
					}

					// Token: 0x020040CE RID: 16590
					public class TOOLTIP
					{
						// Token: 0x0400FA83 RID: 64131
						public static LocString READY = "All requested landers have been built and are ready for deployment";

						// Token: 0x0400FA84 RID: 64132
						public static LocString FAILURE = "Additional landers must be constructed to fulfill the lander requests of this rocket";

						// Token: 0x0400FA85 RID: 64133
						public static LocString WARNING = "";
					}
				}

				// Token: 0x02003C38 RID: 15416
				public class MAX_MODULES
				{
					// Token: 0x020040CF RID: 16591
					public class STATUS
					{
						// Token: 0x0400FA86 RID: 64134
						public static LocString READY = "Module limit";

						// Token: 0x0400FA87 RID: 64135
						public static LocString FAILURE = "Module limit";

						// Token: 0x0400FA88 RID: 64136
						public static LocString WARNING = "Module limit";
					}

					// Token: 0x020040D0 RID: 16592
					public class TOOLTIP
					{
						// Token: 0x0400FA89 RID: 64137
						public static LocString READY = "The rocket's engine can support the number of installed rocket modules";

						// Token: 0x0400FA8A RID: 64138
						public static LocString FAILURE = "The number of installed modules exceeds the engine's module limit\n\nExcess modules must be removed";

						// Token: 0x0400FA8B RID: 64139
						public static LocString WARNING = "Module limit warning";
					}
				}

				// Token: 0x02003C39 RID: 15417
				public class HAS_RESOURCE
				{
					// Token: 0x020040D1 RID: 16593
					public class STATUS
					{
						// Token: 0x0400FA8C RID: 64140
						public static LocString READY = "{0} {1} supplied";

						// Token: 0x0400FA8D RID: 64141
						public static LocString FAILURE = "{0} missing {1}";

						// Token: 0x0400FA8E RID: 64142
						public static LocString WARNING = "{0} missing {1}";
					}

					// Token: 0x020040D2 RID: 16594
					public class TOOLTIP
					{
						// Token: 0x0400FA8F RID: 64143
						public static LocString READY = "{0} {1} supplied";

						// Token: 0x0400FA90 RID: 64144
						public static LocString FAILURE = "{0} has less than {1} {2}";

						// Token: 0x0400FA91 RID: 64145
						public static LocString WARNING = "{0} has less than {1} {2}";
					}
				}

				// Token: 0x02003C3A RID: 15418
				public class MAX_HEIGHT
				{
					// Token: 0x020040D3 RID: 16595
					public class STATUS
					{
						// Token: 0x0400FA92 RID: 64146
						public static LocString READY = "Height limit";

						// Token: 0x0400FA93 RID: 64147
						public static LocString FAILURE = "Height limit";

						// Token: 0x0400FA94 RID: 64148
						public static LocString WARNING = "Height limit";
					}

					// Token: 0x020040D4 RID: 16596
					public class TOOLTIP
					{
						// Token: 0x0400FA95 RID: 64149
						public static LocString READY = "The rocket's engine can support the height of the rocket";

						// Token: 0x0400FA96 RID: 64150
						public static LocString FAILURE = "The height of the rocket exceeds the engine's limit\n\nExcess modules must be removed";

						// Token: 0x0400FA97 RID: 64151
						public static LocString WARNING = "Height limit warning";
					}
				}

				// Token: 0x02003C3B RID: 15419
				public class PROPERLY_FUELED
				{
					// Token: 0x020040D5 RID: 16597
					public class STATUS
					{
						// Token: 0x0400FA98 RID: 64152
						public static LocString READY = "Fueled";

						// Token: 0x0400FA99 RID: 64153
						public static LocString FAILURE = "Fueled";

						// Token: 0x0400FA9A RID: 64154
						public static LocString WARNING = "Fueled";
					}

					// Token: 0x020040D6 RID: 16598
					public class TOOLTIP
					{
						// Token: 0x0400FA9B RID: 64155
						public static LocString READY = "The rocket is sufficiently fueled for a roundtrip to its destination and back";

						// Token: 0x0400FA9C RID: 64156
						public static LocString READY_NO_DESTINATION = "This rocket's fuel tanks have been filled to capacity, but it has no destination";

						// Token: 0x0400FA9D RID: 64157
						public static LocString FAILURE = "This rocket does not have enough fuel to reach its destination\n\nIf the tanks are full, a different Fuel Tank Module may be required";

						// Token: 0x0400FA9E RID: 64158
						public static LocString WARNING = "The rocket has enough fuel for a one-way trip to its destination, but will not be able to make it back";
					}
				}

				// Token: 0x02003C3C RID: 15420
				public class SUFFICIENT_OXIDIZER
				{
					// Token: 0x020040D7 RID: 16599
					public class STATUS
					{
						// Token: 0x0400FA9F RID: 64159
						public static LocString READY = "Sufficient Oxidizer";

						// Token: 0x0400FAA0 RID: 64160
						public static LocString FAILURE = "Sufficient Oxidizer";

						// Token: 0x0400FAA1 RID: 64161
						public static LocString WARNING = "Warning: Limited oxidizer";
					}

					// Token: 0x020040D8 RID: 16600
					public class TOOLTIP
					{
						// Token: 0x0400FAA2 RID: 64162
						public static LocString READY = "This rocket has sufficient oxidizer for a roundtrip to its destination and back";

						// Token: 0x0400FAA3 RID: 64163
						public static LocString FAILURE = "This rocket does not have enough oxidizer to reach its destination\n\nIf the oxidizer tanks are full, a different Oxidizer Tank Module may be required";

						// Token: 0x0400FAA4 RID: 64164
						public static LocString WARNING = "The rocket has enough oxidizer for a one-way trip to its destination, but will not be able to make it back";
					}
				}

				// Token: 0x02003C3D RID: 15421
				public class ON_LAUNCHPAD
				{
					// Token: 0x020040D9 RID: 16601
					public class STATUS
					{
						// Token: 0x0400FAA5 RID: 64165
						public static LocString READY = "On a launch pad";

						// Token: 0x0400FAA6 RID: 64166
						public static LocString FAILURE = "Not on a launch pad";

						// Token: 0x0400FAA7 RID: 64167
						public static LocString WARNING = "No launch pad";
					}

					// Token: 0x020040DA RID: 16602
					public class TOOLTIP
					{
						// Token: 0x0400FAA8 RID: 64168
						public static LocString READY = "On a launch pad";

						// Token: 0x0400FAA9 RID: 64169
						public static LocString FAILURE = "Not on a launch pad";

						// Token: 0x0400FAAA RID: 64170
						public static LocString WARNING = "No launch pad";
					}
				}

				// Token: 0x02003C3E RID: 15422
				public class ROBOT_PILOT_DATA_REQUIREMENTS
				{
					// Token: 0x020040DB RID: 16603
					public class STATUS
					{
						// Token: 0x0400FAAB RID: 64171
						public static LocString WARNING_NO_DATA_BANKS_HUMAN_PILOT = "Robo-Pilot programmed";

						// Token: 0x0400FAAC RID: 64172
						public static LocString READY = "Robo-Pilot programmed";

						// Token: 0x0400FAAD RID: 64173
						public static LocString FAILURE = "Robo-Pilot programmed";

						// Token: 0x0400FAAE RID: 64174
						public static LocString WARNING = "Robo-Pilot programmed";
					}

					// Token: 0x020040DC RID: 16604
					public class TOOLTIP
					{
						// Token: 0x0400FAAF RID: 64175
						public static LocString READY = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has sufficient ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" for a roundtrip to its destination and back\n    • ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" stored: {0}/{1}"
						});

						// Token: 0x0400FAB0 RID: 64176
						public static LocString READY_NO_DESTINATION = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has sufficient ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							", but no destination has been set\n    • ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" stored: {0}"
						});

						// Token: 0x0400FAB1 RID: 64177
						public static LocString FAILURE_NO_DESTINATION = "No destination has been set";

						// Token: 0x0400FAB2 RID: 64178
						public static LocString FAILURE = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" requires at least {0} ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" to reach its destination\n    • ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" stored: {1}"
						});

						// Token: 0x0400FAB3 RID: 64179
						public static LocString WARNING = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has insufficient ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" for a roundtrip to its destination and back\n    • ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							"stored: {0}/{1}"
						});

						// Token: 0x0400FAB4 RID: 64180
						public static LocString WARNING_NO_DATA_BANKS_HUMAN_PILOT = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" cannot function without ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							"\n\nThis rocket is currently being operated by a Duplicant who possesses the ",
							DUPLICANTS.ROLES.ROCKETPILOT.NAME,
							" skill"
						});
					}
				}

				// Token: 0x02003C3F RID: 15423
				public class ROBOT_PILOT_POWER_SOUCRE
				{
					// Token: 0x020040DD RID: 16605
					public class STATUS
					{
						// Token: 0x0400FAB5 RID: 64181
						public static LocString READY = "Robo-Pilot has power";

						// Token: 0x0400FAB6 RID: 64182
						public static LocString WARNING = "Robo-Pilot has power";

						// Token: 0x0400FAB7 RID: 64183
						public static LocString FAILURE = "Robo-Pilot has power";
					}

					// Token: 0x020040DE RID: 16606
					public class TOOLTIP
					{
						// Token: 0x0400FAB8 RID: 64184
						public static LocString READY = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has a ",
							UI.PRE_KEYWORD,
							"Power",
							UI.PST_KEYWORD,
							" source"
						});

						// Token: 0x0400FAB9 RID: 64185
						public static LocString WARNING = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has insufficient  ",
							UI.PRE_KEYWORD,
							"Power",
							UI.PST_KEYWORD,
							" for a round-trip to its destination"
						});

						// Token: 0x0400FABA RID: 64186
						public static LocString FAILURE = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" requires a ",
							UI.PRE_KEYWORD,
							"Power",
							UI.PST_KEYWORD,
							" source for launch"
						});
					}
				}
			}

			// Token: 0x02002F44 RID: 12100
			public class FULLTANK
			{
				// Token: 0x0400CB8B RID: 52107
				public static LocString NAME = "Fuel Tank full";

				// Token: 0x0400CB8C RID: 52108
				public static LocString TOOLTIP = "Tank is full, ready for launch";
			}

			// Token: 0x02002F45 RID: 12101
			public class EMPTYTANK
			{
				// Token: 0x0400CB8D RID: 52109
				public static LocString NAME = "Fuel Tank not full";

				// Token: 0x0400CB8E RID: 52110
				public static LocString TOOLTIP = "Fuel tank must be filled before launch";
			}

			// Token: 0x02002F46 RID: 12102
			public class FULLOXIDIZERTANK
			{
				// Token: 0x0400CB8F RID: 52111
				public static LocString NAME = "Oxidizer Tank full";

				// Token: 0x0400CB90 RID: 52112
				public static LocString TOOLTIP = "Tank is full, ready for launch";
			}

			// Token: 0x02002F47 RID: 12103
			public class EMPTYOXIDIZERTANK
			{
				// Token: 0x0400CB91 RID: 52113
				public static LocString NAME = "Oxidizer Tank not full";

				// Token: 0x0400CB92 RID: 52114
				public static LocString TOOLTIP = "Oxidizer tank must be filled before launch";
			}

			// Token: 0x02002F48 RID: 12104
			public class ROCKETSTATUS
			{
				// Token: 0x0400CB93 RID: 52115
				public static LocString STATUS_TITLE = "Rocket Status";

				// Token: 0x0400CB94 RID: 52116
				public static LocString NONE = "NONE";

				// Token: 0x0400CB95 RID: 52117
				public static LocString SELECTED = "SELECTED";

				// Token: 0x0400CB96 RID: 52118
				public static LocString LOCKEDIN = "LOCKED IN";

				// Token: 0x0400CB97 RID: 52119
				public static LocString NODESTINATION = "No destination selected";

				// Token: 0x0400CB98 RID: 52120
				public static LocString DESTINATIONVALUE = "None";

				// Token: 0x0400CB99 RID: 52121
				public static LocString NOPASSENGERS = "No passengers";

				// Token: 0x0400CB9A RID: 52122
				public static LocString STATUS = "Status";

				// Token: 0x0400CB9B RID: 52123
				public static LocString TOTAL = "Total";

				// Token: 0x0400CB9C RID: 52124
				public static LocString WEIGHTPENALTY = "Weight Penalty";

				// Token: 0x0400CB9D RID: 52125
				public static LocString TIMEREMAINING = "Time Remaining";

				// Token: 0x0400CB9E RID: 52126
				public static LocString BOOSTED_TIME_MODIFIER = "Less Than ";
			}

			// Token: 0x02002F49 RID: 12105
			public class ROCKETSTATS
			{
				// Token: 0x0400CB9F RID: 52127
				public static LocString TOTAL_OXIDIZABLE_FUEL = "Total oxidizable fuel";

				// Token: 0x0400CBA0 RID: 52128
				public static LocString TOTAL_OXIDIZER = "Total oxidizer";

				// Token: 0x0400CBA1 RID: 52129
				public static LocString TOTAL_FUEL = "Total fuel";

				// Token: 0x0400CBA2 RID: 52130
				public static LocString NO_ENGINE = "NO ENGINE";

				// Token: 0x0400CBA3 RID: 52131
				public static LocString ENGINE_EFFICIENCY = "Main engine efficiency";

				// Token: 0x0400CBA4 RID: 52132
				public static LocString OXIDIZER_EFFICIENCY = "Average oxidizer efficiency";

				// Token: 0x0400CBA5 RID: 52133
				public static LocString SOLID_BOOSTER = "Solid boosters";

				// Token: 0x0400CBA6 RID: 52134
				public static LocString ROBO_PILOT_RANGE = "Robo-Pilot Range";

				// Token: 0x0400CBA7 RID: 52135
				public static LocString ROBO_PILOT_EFFICIENCY = "Robo-Pilot can travel {0} per " + UI.PRE_KEYWORD + "Data Bank" + UI.PST_KEYWORD;

				// Token: 0x0400CBA8 RID: 52136
				public static LocString TOTAL_THRUST = "Total thrust";

				// Token: 0x0400CBA9 RID: 52137
				public static LocString TOTAL_RANGE = "Total range";

				// Token: 0x0400CBAA RID: 52138
				public static LocString DRY_MASS = "Dry mass";

				// Token: 0x0400CBAB RID: 52139
				public static LocString WET_MASS = "Wet mass";
			}

			// Token: 0x02002F4A RID: 12106
			public class STORAGESTATS
			{
				// Token: 0x0400CBAC RID: 52140
				public static LocString STORAGECAPACITY = "{0} / {1}";
			}
		}

		// Token: 0x02002531 RID: 9521
		public class RESEARCHSCREEN
		{
			// Token: 0x0400A6B4 RID: 42676
			public static LocString SEARCH_RESULTS_CATEGORY = "Search Results";

			// Token: 0x02002F4B RID: 12107
			public class FILTER_BUTTONS
			{
				// Token: 0x0400CBAD RID: 52141
				public static LocString HEADER = "Preset Filters";

				// Token: 0x0400CBAE RID: 52142
				public static LocString ALL = "All";

				// Token: 0x0400CBAF RID: 52143
				public static LocString AVAILABLE = "Next";

				// Token: 0x0400CBB0 RID: 52144
				public static LocString COMPLETED = "Completed";

				// Token: 0x0400CBB1 RID: 52145
				public static LocString OXYGEN = "Oxygen";

				// Token: 0x0400CBB2 RID: 52146
				public static LocString FOOD = "Food";

				// Token: 0x0400CBB3 RID: 52147
				public static LocString WATER = "Water";

				// Token: 0x0400CBB4 RID: 52148
				public static LocString POWER = "Power";

				// Token: 0x0400CBB5 RID: 52149
				public static LocString MORALE = "Morale";

				// Token: 0x0400CBB6 RID: 52150
				public static LocString RANCHING = "Ranching";

				// Token: 0x0400CBB7 RID: 52151
				public static LocString FILTER = "Filter";

				// Token: 0x0400CBB8 RID: 52152
				public static LocString TILE = "Tile";

				// Token: 0x0400CBB9 RID: 52153
				public static LocString TRANSPORT = "Transport";

				// Token: 0x0400CBBA RID: 52154
				public static LocString AUTOMATION = "Automation";

				// Token: 0x0400CBBB RID: 52155
				public static LocString MEDICINE = "Medicine";

				// Token: 0x0400CBBC RID: 52156
				public static LocString ROCKET = "Rocket";

				// Token: 0x0400CBBD RID: 52157
				public static LocString RADIATION = "Radiation";
			}
		}

		// Token: 0x02002532 RID: 9522
		public class CODEX
		{
			// Token: 0x0400A6B5 RID: 42677
			public static LocString SEARCH_HEADER = "Search Database";

			// Token: 0x0400A6B6 RID: 42678
			public static LocString BACK_BUTTON = "Back ({0})";

			// Token: 0x0400A6B7 RID: 42679
			public static LocString TIPS = "Tips";

			// Token: 0x0400A6B8 RID: 42680
			public static LocString GAME_SYSTEMS = "Systems";

			// Token: 0x0400A6B9 RID: 42681
			public static LocString DETAILS = "Details";

			// Token: 0x0400A6BA RID: 42682
			public static LocString RECIPE_ITEM = "{0} x {1}{2}";

			// Token: 0x0400A6BB RID: 42683
			public static LocString RECIPE_FABRICATOR = "{1} ({0} seconds)";

			// Token: 0x0400A6BC RID: 42684
			public static LocString RECIPE_FABRICATOR_HEADER = "Produced by";

			// Token: 0x0400A6BD RID: 42685
			public static LocString BACK_BUTTON_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go back:\n{0}";

			// Token: 0x0400A6BE RID: 42686
			public static LocString BACK_BUTTON_NO_HISTORY_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go back:\nN/A";

			// Token: 0x0400A6BF RID: 42687
			public static LocString FORWARD_BUTTON_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go forward:\n{0}";

			// Token: 0x0400A6C0 RID: 42688
			public static LocString FORWARD_BUTTON_NO_HISTORY_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go forward:\nN/A";

			// Token: 0x0400A6C1 RID: 42689
			public static LocString TITLE = "DATABASE";

			// Token: 0x0400A6C2 RID: 42690
			public static LocString MANAGEMENT_BUTTON = "DATABASE";

			// Token: 0x02002F4C RID: 12108
			public class CODEX_DISCOVERED_MESSAGE
			{
				// Token: 0x0400CBBE RID: 52158
				public static LocString TITLE = "New Log Entry";

				// Token: 0x0400CBBF RID: 52159
				public static LocString BODY = "I've added a new entry to my log: {codex}\n";
			}

			// Token: 0x02002F4D RID: 12109
			public class SUBWORLDS
			{
				// Token: 0x0400CBC0 RID: 52160
				public static LocString ELEMENTS = "Elements";

				// Token: 0x0400CBC1 RID: 52161
				public static LocString PLANTS = "Plants";

				// Token: 0x0400CBC2 RID: 52162
				public static LocString CRITTERS = "Critters";

				// Token: 0x0400CBC3 RID: 52163
				public static LocString NONE = "None";
			}

			// Token: 0x02002F4E RID: 12110
			public class GEYSERS
			{
				// Token: 0x0400CBC4 RID: 52164
				public static LocString DESC = "Geysers and Fumaroles emit elements at variable intervals. They provide a sustainable source of material, albeit in typically low volumes.\n\nThe variable factors of a geyser are:\n\n    • Emission element \n    • Emission temperature \n    • Emission mass \n    • Cycle length \n    • Dormancy duration \n    • Disease emitted";
			}

			// Token: 0x02002F4F RID: 12111
			public class EQUIPMENT
			{
				// Token: 0x0400CBC5 RID: 52165
				public static LocString DESC = "Equipment description";
			}

			// Token: 0x02002F50 RID: 12112
			public class FOOD
			{
				// Token: 0x0400CBC6 RID: 52166
				public static LocString QUALITY = "Quality: {0}";

				// Token: 0x0400CBC7 RID: 52167
				public static LocString CALORIES = "Calories: {0}";

				// Token: 0x0400CBC8 RID: 52168
				public static LocString SPOILPROPERTIES = "Refrigeration temperature: {0}\nDeep Freeze temperature: {1}\nSpoil time: {2}";

				// Token: 0x0400CBC9 RID: 52169
				public static LocString NON_PERISHABLE = "Spoil time: Never";
			}

			// Token: 0x02002F51 RID: 12113
			public class CATEGORYNAMES
			{
				// Token: 0x0400CBCA RID: 52170
				public static LocString ROOT = UI.FormatAsLink("Index", "HOME");

				// Token: 0x0400CBCB RID: 52171
				public static LocString PLANTS = UI.FormatAsLink("Plants", "PLANTS");

				// Token: 0x0400CBCC RID: 52172
				public static LocString CREATURES = UI.FormatAsLink("Critters", "CREATURES");

				// Token: 0x0400CBCD RID: 52173
				public static LocString DUPLICANTS = UI.FormatAsLink("Duplicants", "DUPLICANTS");

				// Token: 0x0400CBCE RID: 52174
				public static LocString EMAILS = UI.FormatAsLink("E-mail", "EMAILS");

				// Token: 0x0400CBCF RID: 52175
				public static LocString JOURNALS = UI.FormatAsLink("Journals", "JOURNALS");

				// Token: 0x0400CBD0 RID: 52176
				public static LocString MYLOG = UI.FormatAsLink("My Log", "MYLOG");

				// Token: 0x0400CBD1 RID: 52177
				public static LocString INVESTIGATIONS = UI.FormatAsLink("Investigations", "Investigations");

				// Token: 0x0400CBD2 RID: 52178
				public static LocString RESEARCHNOTES = UI.FormatAsLink("Research Notes", "RESEARCHNOTES");

				// Token: 0x0400CBD3 RID: 52179
				public static LocString NOTICES = UI.FormatAsLink("Notices", "NOTICES");

				// Token: 0x0400CBD4 RID: 52180
				public static LocString FOOD = UI.FormatAsLink("Food", "FOOD");

				// Token: 0x0400CBD5 RID: 52181
				public static LocString MINION_MODIFIERS = UI.FormatAsLink("Duplicant Effects (EDITOR ONLY)", "MINION_MODIFIERS");

				// Token: 0x0400CBD6 RID: 52182
				public static LocString BUILDINGS = UI.FormatAsLink("Buildings", "BUILDINGS");

				// Token: 0x0400CBD7 RID: 52183
				public static LocString ROOMS = UI.FormatAsLink("Rooms", "ROOMS");

				// Token: 0x0400CBD8 RID: 52184
				public static LocString TECH = UI.FormatAsLink("Research", "TECH");

				// Token: 0x0400CBD9 RID: 52185
				public static LocString TIPS = UI.FormatAsLink("Tutorials", "LESSONS");

				// Token: 0x0400CBDA RID: 52186
				public static LocString TUTORIALS = UI.FormatAsLink("Tutorials", "LESSONS");

				// Token: 0x0400CBDB RID: 52187
				public static LocString VIDEOTUTORIALS = UI.FormatAsLink("Videos", "VIDEOTUTORIALS");

				// Token: 0x0400CBDC RID: 52188
				public static LocString SYSTEMSTUTORIALS = UI.FormatAsLink("Systems", "SYSTEMSTUTORIALS");

				// Token: 0x0400CBDD RID: 52189
				public static LocString EQUIPMENT = UI.FormatAsLink("Equipment", "EQUIPMENT");

				// Token: 0x0400CBDE RID: 52190
				public static LocString BIOMES = UI.FormatAsLink("Biomes", "BIOMES");

				// Token: 0x0400CBDF RID: 52191
				public static LocString STORYTRAITS = UI.FormatAsLink("Story Traits", "STORYTRAITS");

				// Token: 0x0400CBE0 RID: 52192
				public static LocString MISCELLANEOUSTIPS = UI.FormatAsLink("Tips", "MISCELLANEOUSTIPS");

				// Token: 0x0400CBE1 RID: 52193
				public static LocString MISCELLANEOUSITEMS = UI.FormatAsLink("Items", "MISCELLANEOUSITEMS");

				// Token: 0x0400CBE2 RID: 52194
				public static LocString ORNAMENTS = UI.FormatAsLink("Ornaments", "ORNAMENTS");

				// Token: 0x0400CBE3 RID: 52195
				public static LocString ELEMENTS = UI.FormatAsLink("Elements", "ELEMENTS");

				// Token: 0x0400CBE4 RID: 52196
				public static LocString ELEMENTSSOLID = UI.FormatAsLink("Solids", "ELEMENTS_SOLID");

				// Token: 0x0400CBE5 RID: 52197
				public static LocString ELEMENTSGAS = UI.FormatAsLink("Gases", "ELEMENTS_GAS");

				// Token: 0x0400CBE6 RID: 52198
				public static LocString ELEMENTSLIQUID = UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID");

				// Token: 0x0400CBE7 RID: 52199
				public static LocString ELEMENTSOTHER = UI.FormatAsLink("Other", "ELEMENTS_OTHER");

				// Token: 0x0400CBE8 RID: 52200
				public static LocString ELEMENTTYPES = UI.FormatAsLink("Element Properties", "ELEMENT_TYPES");

				// Token: 0x0400CBE9 RID: 52201
				public static LocString BUILDINGMATERIALCLASSES = UI.FormatAsLink("Building Materials", "BUILDING_MATERIAL_CLASSES");

				// Token: 0x0400CBEA RID: 52202
				public static LocString INDUSTRIALINGREDIENTS = UI.FormatAsLink("Industrial Ingredients", "INDUSTRIALINGREDIENTS");

				// Token: 0x0400CBEB RID: 52203
				public static LocString TECHCOMPONENTS = UI.FormatAsLink("Tech Components", "TECHCOMPONENTS");

				// Token: 0x0400CBEC RID: 52204
				public static LocString DUPLICANTSCATEGORY = UI.FormatAsLink("Duplicants", "DUPLICANTS");

				// Token: 0x0400CBED RID: 52205
				public static LocString MEDICINES = UI.FormatAsLink("Medicines", "MEDICINES");

				// Token: 0x0400CBEE RID: 52206
				public static LocString GEYSERS = UI.FormatAsLink("Geysers", "GEYSERS");

				// Token: 0x0400CBEF RID: 52207
				public static LocString SYSTEMS = UI.FormatAsLink("Systems", "SYSTEMS");

				// Token: 0x0400CBF0 RID: 52208
				public static LocString ROLES = UI.FormatAsLink("Duplicant Skills", "ROLES");

				// Token: 0x0400CBF1 RID: 52209
				public static LocString DISEASE = UI.FormatAsLink("Disease", "DISEASE");

				// Token: 0x0400CBF2 RID: 52210
				public static LocString SICKNESS = UI.FormatAsLink("Sickness", "SICKNESS");

				// Token: 0x0400CBF3 RID: 52211
				public static LocString MEDIA = UI.FormatAsLink("Media", "MEDIA");
			}
		}

		// Token: 0x02002533 RID: 9523
		public class DEVELOPMENTBUILDS
		{
			// Token: 0x0400A6C3 RID: 42691
			public static LocString WATERMARK = "BUILD: {0}";

			// Token: 0x0400A6C4 RID: 42692
			public static LocString TESTING_WATERMARK = "TESTING BUILD: {0}";

			// Token: 0x0400A6C5 RID: 42693
			public static LocString TESTING_TOOLTIP = "This game is currently running a Test version.\n\n" + UI.CLICK(UI.ClickType.Click) + " for more info.";

			// Token: 0x0400A6C6 RID: 42694
			public static LocString TESTING_MESSAGE_TITLE = "TESTING BUILD";

			// Token: 0x0400A6C7 RID: 42695
			public static LocString TESTING_MESSAGE = "This game is running a Test version of Oxygen Not Included. This means that some features may be in development or buggier than normal, and require more testing before they can be moved into the Release build.\n\nIf you encounter any bugs or strange behavior, please add a report to the bug forums. We appreciate it!";

			// Token: 0x0400A6C8 RID: 42696
			public static LocString TESTING_MORE_INFO = "BUG FORUMS";

			// Token: 0x0400A6C9 RID: 42697
			public static LocString FULL_PATCH_NOTES = "Full Patch Notes";

			// Token: 0x0400A6CA RID: 42698
			public static LocString PREVIOUS_VERSION = "Previous Version";

			// Token: 0x02002F52 RID: 12114
			public class ALPHA
			{
				// Token: 0x02003C40 RID: 15424
				public class MESSAGES
				{
					// Token: 0x0400EF61 RID: 61281
					public static LocString FORUMBUTTON = "FORUMS";

					// Token: 0x0400EF62 RID: 61282
					public static LocString MAILINGLIST = "MAILING LIST";

					// Token: 0x0400EF63 RID: 61283
					public static LocString PATCHNOTES = "PATCH NOTES";

					// Token: 0x0400EF64 RID: 61284
					public static LocString FEEDBACK = "FEEDBACK";
				}

				// Token: 0x02003C41 RID: 15425
				public class LOADING
				{
					// Token: 0x0400EF65 RID: 61285
					public static LocString TITLE = "<b>Welcome to Oxygen Not Included!</b>";

					// Token: 0x0400EF66 RID: 61286
					public static LocString BODY = "This game is in the early stages of development which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\nDuring this time Oxygen Not Included will be receiving regular updates to fix bugs, add features, and introduce additional content, so if you encounter issues or just have suggestions to share, please let us know on our forums: <u>http://forums.kleientertainment.com</u>\n\nA special thanks to those who joined us during our time in Alpha. We value your feedback and thank you for joining us in the development process. We couldn't do this without you.\n\nEnjoy your time in deep space!\n\n- Klei";

					// Token: 0x0400EF67 RID: 61287
					public static LocString BODY_NOLINKS = "This DLC is currently in active development, which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\n During this time Spaced Out! will be receiving regular updates to fix bugs, add features, and introduce additional content.\n\n We've got lots of content old and new to add to this DLC before it's ready, and we're happy to have you along with us. Enjoy your time in deep space!\n\n - The Team at Klei";

					// Token: 0x0400EF68 RID: 61288
					public static LocString FORUMBUTTON = "Visit Forums";
				}

				// Token: 0x02003C42 RID: 15426
				public class HEALTHY_MESSAGE
				{
					// Token: 0x0400EF69 RID: 61289
					public static LocString CONTINUEBUTTON = "Thanks!";
				}
			}

			// Token: 0x02002F53 RID: 12115
			public class PREVIOUS_UPDATE
			{
				// Token: 0x0400CBF4 RID: 52212
				public static LocString TITLE = "<b>Welcome to Oxygen Not Included</b>";

				// Token: 0x0400CBF5 RID: 52213
				public static LocString BODY = "Whoops!\n\nYou're about to opt in to the <b>Previous Update branch</b>. That means opting out of all new features, fixes and content from the live branch.\n\nThis branch is temporary. It will be replaced when the next update is released. It's also completely unsupported: please don't report bugs or issues you find here.\n\nAre you sure you want to opt in?";

				// Token: 0x0400CBF6 RID: 52214
				public static LocString CONTINUEBUTTON = "Play Old Version";

				// Token: 0x0400CBF7 RID: 52215
				public static LocString FORUMBUTTON = "More Information";

				// Token: 0x0400CBF8 RID: 52216
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002F54 RID: 12116
			public class DLC_BETA
			{
				// Token: 0x0400CBF9 RID: 52217
				public static LocString TITLE = "<b>Welcome to Oxygen Not Included</b>";

				// Token: 0x0400CBFA RID: 52218
				public static LocString BODY = "You're about to opt in to the beta for <b>The Bionic Booster Pack</b> DLC.\nThis free beta is a work in progress, and will be discontinued before the paid DLC is released. \n\nAre you sure you want to opt in?";

				// Token: 0x0400CBFB RID: 52219
				public static LocString CONTINUEBUTTON = "Play Beta";

				// Token: 0x0400CBFC RID: 52220
				public static LocString FORUMBUTTON = "More Information";

				// Token: 0x0400CBFD RID: 52221
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002F55 RID: 12117
			public class UPDATES
			{
				// Token: 0x0400CBFE RID: 52222
				public static LocString UPDATES_HEADER = "NEXT UPGRADE LIVE IN";

				// Token: 0x0400CBFF RID: 52223
				public static LocString NOW = "Less than a day";

				// Token: 0x0400CC00 RID: 52224
				public static LocString TWENTY_FOUR_HOURS = "Less than a day";

				// Token: 0x0400CC01 RID: 52225
				public static LocString FINAL_WEEK = "{0} days";

				// Token: 0x0400CC02 RID: 52226
				public static LocString BIGGER_TIMES = "{1} weeks {0} days";
			}
		}

		// Token: 0x02002534 RID: 9524
		public class UNITSUFFIXES
		{
			// Token: 0x0400A6CB RID: 42699
			public static LocString SECOND = " s";

			// Token: 0x0400A6CC RID: 42700
			public static LocString PERSECOND = "/s";

			// Token: 0x0400A6CD RID: 42701
			public static LocString PERCYCLE = "/cycle";

			// Token: 0x0400A6CE RID: 42702
			public static LocString UNIT = " unit";

			// Token: 0x0400A6CF RID: 42703
			public static LocString UNITS = " units";

			// Token: 0x0400A6D0 RID: 42704
			public static LocString PERCENT = "%";

			// Token: 0x0400A6D1 RID: 42705
			public static LocString DEGREES = " degrees";

			// Token: 0x0400A6D2 RID: 42706
			public static LocString CRITTERS = " critters";

			// Token: 0x0400A6D3 RID: 42707
			public static LocString GROWTH = "growth";

			// Token: 0x0400A6D4 RID: 42708
			public static LocString SECONDS = "Seconds";

			// Token: 0x0400A6D5 RID: 42709
			public static LocString DUPLICANTS = "Duplicants";

			// Token: 0x0400A6D6 RID: 42710
			public static LocString GERMS = "Germs";

			// Token: 0x0400A6D7 RID: 42711
			public static LocString ROCKET_MISSIONS = "Missions";

			// Token: 0x0400A6D8 RID: 42712
			public static LocString TILES = "Tiles";

			// Token: 0x0400A6D9 RID: 42713
			public static LocString TILES_STARMAP = "Hexes";

			// Token: 0x02002F56 RID: 12118
			public class MASS
			{
				// Token: 0x0400CC03 RID: 52227
				public static LocString TONNE = " t";

				// Token: 0x0400CC04 RID: 52228
				public static LocString KILOGRAM = " kg";

				// Token: 0x0400CC05 RID: 52229
				public static LocString GRAM = " g";

				// Token: 0x0400CC06 RID: 52230
				public static LocString MILLIGRAM = " mg";

				// Token: 0x0400CC07 RID: 52231
				public static LocString MICROGRAM = " mcg";

				// Token: 0x0400CC08 RID: 52232
				public static LocString POUND = " lb";

				// Token: 0x0400CC09 RID: 52233
				public static LocString DRACHMA = " dr";

				// Token: 0x0400CC0A RID: 52234
				public static LocString GRAIN = " gr";
			}

			// Token: 0x02002F57 RID: 12119
			public class TEMPERATURE
			{
				// Token: 0x0400CC0B RID: 52235
				public static LocString CELSIUS = " " + 'º'.ToString() + "C";

				// Token: 0x0400CC0C RID: 52236
				public static LocString FAHRENHEIT = " " + 'º'.ToString() + "F";

				// Token: 0x0400CC0D RID: 52237
				public static LocString KELVIN = " K";
			}

			// Token: 0x02002F58 RID: 12120
			public class CALORIES
			{
				// Token: 0x0400CC0E RID: 52238
				public static LocString CALORIE = " cal";

				// Token: 0x0400CC0F RID: 52239
				public static LocString KILOCALORIE = " kcal";
			}

			// Token: 0x02002F59 RID: 12121
			public class ELECTRICAL
			{
				// Token: 0x0400CC10 RID: 52240
				public static LocString JOULE = " J";

				// Token: 0x0400CC11 RID: 52241
				public static LocString KILOJOULE = " kJ";

				// Token: 0x0400CC12 RID: 52242
				public static LocString MEGAJOULE = " MJ";

				// Token: 0x0400CC13 RID: 52243
				public static LocString WATT = " W";

				// Token: 0x0400CC14 RID: 52244
				public static LocString KILOWATT = " kW";
			}

			// Token: 0x02002F5A RID: 12122
			public class HEAT
			{
				// Token: 0x0400CC15 RID: 52245
				public static LocString DTU = " DTU";

				// Token: 0x0400CC16 RID: 52246
				public static LocString KDTU = " kDTU";

				// Token: 0x0400CC17 RID: 52247
				public static LocString DTU_S = " DTU/s";

				// Token: 0x0400CC18 RID: 52248
				public static LocString KDTU_S = " kDTU/s";
			}

			// Token: 0x02002F5B RID: 12123
			public class DISTANCE
			{
				// Token: 0x0400CC19 RID: 52249
				public static LocString METER = " m";

				// Token: 0x0400CC1A RID: 52250
				public static LocString KILOMETER = " km";
			}

			// Token: 0x02002F5C RID: 12124
			public class DISEASE
			{
				// Token: 0x0400CC1B RID: 52251
				public static LocString UNITS = " germs";
			}

			// Token: 0x02002F5D RID: 12125
			public class NOISE
			{
				// Token: 0x0400CC1C RID: 52252
				public static LocString UNITS = " dB";
			}

			// Token: 0x02002F5E RID: 12126
			public class INFORMATION
			{
				// Token: 0x0400CC1D RID: 52253
				public static LocString BYTE = "B";

				// Token: 0x0400CC1E RID: 52254
				public static LocString KILOBYTE = "kB";

				// Token: 0x0400CC1F RID: 52255
				public static LocString MEGABYTE = "MB";

				// Token: 0x0400CC20 RID: 52256
				public static LocString GIGABYTE = "GB";

				// Token: 0x0400CC21 RID: 52257
				public static LocString TERABYTE = "TB";
			}

			// Token: 0x02002F5F RID: 12127
			public class LIGHT
			{
				// Token: 0x0400CC22 RID: 52258
				public static LocString LUX = " lux";
			}

			// Token: 0x02002F60 RID: 12128
			public class RADIATION
			{
				// Token: 0x0400CC23 RID: 52259
				public static LocString RADS = " rads";
			}

			// Token: 0x02002F61 RID: 12129
			public class HIGHENERGYPARTICLES
			{
				// Token: 0x0400CC24 RID: 52260
				public static LocString PARTRICLE = " Radbolt";

				// Token: 0x0400CC25 RID: 52261
				public static LocString PARTRICLES = " Radbolts";
			}
		}

		// Token: 0x02002535 RID: 9525
		public class OVERLAYS
		{
			// Token: 0x02002F62 RID: 12130
			public class TILEMODE
			{
				// Token: 0x0400CC26 RID: 52262
				public static LocString NAME = "MATERIALS OVERLAY";

				// Token: 0x0400CC27 RID: 52263
				public static LocString BUTTON = "Materials Overlay";
			}

			// Token: 0x02002F63 RID: 12131
			public class OXYGEN
			{
				// Token: 0x0400CC28 RID: 52264
				public static LocString NAME = "OXYGEN OVERLAY";

				// Token: 0x0400CC29 RID: 52265
				public static LocString BUTTON = "Oxygen Overlay";

				// Token: 0x0400CC2A RID: 52266
				public static LocString LEGEND1 = "Very Breathable";

				// Token: 0x0400CC2B RID: 52267
				public static LocString LEGEND2 = "Breathable";

				// Token: 0x0400CC2C RID: 52268
				public static LocString LEGEND3 = "Barely Breathable";

				// Token: 0x0400CC2D RID: 52269
				public static LocString LEGEND4 = "Unbreathable";

				// Token: 0x0400CC2E RID: 52270
				public static LocString LEGEND5 = "Barely Breathable";

				// Token: 0x0400CC2F RID: 52271
				public static LocString LEGEND6 = "Unbreathable";

				// Token: 0x02003C43 RID: 15427
				public class TOOLTIPS
				{
					// Token: 0x0400EF6A RID: 61290
					public static LocString LEGEND1 = string.Concat(new string[]
					{
						"<b>Very Breathable</b>\nHigh ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400EF6B RID: 61291
					public static LocString LEGEND2 = string.Concat(new string[]
					{
						"<b>Breathable</b>\nSufficient ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400EF6C RID: 61292
					public static LocString LEGEND3 = string.Concat(new string[]
					{
						"<b>Barely Breathable</b>\nLow ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400EF6D RID: 61293
					public static LocString LEGEND4 = string.Concat(new string[]
					{
						"<b>Unbreathable</b>\nExtremely low or absent ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations\n\nDuplicants will suffocate if trapped in these areas"
					});

					// Token: 0x0400EF6E RID: 61294
					public static LocString LEGEND5 = "<b>Slightly Toxic</b>\nHarmful gas concentration";

					// Token: 0x0400EF6F RID: 61295
					public static LocString LEGEND6 = "<b>Very Toxic</b>\nLethal gas concentration";
				}
			}

			// Token: 0x02002F64 RID: 12132
			public class ELECTRICAL
			{
				// Token: 0x0400CC30 RID: 52272
				public static LocString NAME = "POWER OVERLAY";

				// Token: 0x0400CC31 RID: 52273
				public static LocString BUTTON = "Power Overlay";

				// Token: 0x0400CC32 RID: 52274
				public static LocString LEGEND1 = "<b>BUILDING POWER</b>";

				// Token: 0x0400CC33 RID: 52275
				public static LocString LEGEND2 = "Consumer";

				// Token: 0x0400CC34 RID: 52276
				public static LocString LEGEND3 = "Producer";

				// Token: 0x0400CC35 RID: 52277
				public static LocString LEGEND4 = "<b>CIRCUIT POWER HEALTH</b>";

				// Token: 0x0400CC36 RID: 52278
				public static LocString LEGEND5 = "Inactive";

				// Token: 0x0400CC37 RID: 52279
				public static LocString LEGEND6 = "Safe";

				// Token: 0x0400CC38 RID: 52280
				public static LocString LEGEND7 = "Strained";

				// Token: 0x0400CC39 RID: 52281
				public static LocString LEGEND8 = "Overloaded";

				// Token: 0x0400CC3A RID: 52282
				public static LocString DIAGRAM_HEADER = "Energy from the <b>Left Outlet</b> is used by the <b>Right Outlet</b>";

				// Token: 0x0400CC3B RID: 52283
				public static LocString LEGEND_SWITCH = "Switch";

				// Token: 0x02003C44 RID: 15428
				public class TOOLTIPS
				{
					// Token: 0x0400EF70 RID: 61296
					public static LocString LEGEND1 = "Displays whether buildings use or generate " + UI.FormatAsLink("Power", "POWER");

					// Token: 0x0400EF71 RID: 61297
					public static LocString LEGEND2 = "<b>Consumer</b>\nThese buildings draw power from a circuit";

					// Token: 0x0400EF72 RID: 61298
					public static LocString LEGEND3 = "<b>Producer</b>\nThese buildings generate power for a circuit";

					// Token: 0x0400EF73 RID: 61299
					public static LocString LEGEND4 = "Displays the health of wire systems";

					// Token: 0x0400EF74 RID: 61300
					public static LocString LEGEND5 = "<b>Inactive</b>\nThere is no power activity on these circuits";

					// Token: 0x0400EF75 RID: 61301
					public static LocString LEGEND6 = "<b>Safe</b>\nThese circuits are not in danger of overloading";

					// Token: 0x0400EF76 RID: 61302
					public static LocString LEGEND7 = "<b>Strained</b>\nThese circuits are close to consuming more power than their wires support";

					// Token: 0x0400EF77 RID: 61303
					public static LocString LEGEND8 = "<b>Overloaded</b>\nThese circuits are consuming more power than their wires support";

					// Token: 0x0400EF78 RID: 61304
					public static LocString LEGEND_SWITCH = "<b>Switch</b>\nActivates or deactivates connected circuits";
				}
			}

			// Token: 0x02002F65 RID: 12133
			public class TEMPERATURE
			{
				// Token: 0x0400CC3C RID: 52284
				public static LocString NAME = "TEMPERATURE OVERLAY";

				// Token: 0x0400CC3D RID: 52285
				public static LocString BUTTON = "Temperature Overlay";

				// Token: 0x0400CC3E RID: 52286
				public static LocString EXTREMECOLD = "Absolute Zero";

				// Token: 0x0400CC3F RID: 52287
				public static LocString VERYCOLD = "Cold";

				// Token: 0x0400CC40 RID: 52288
				public static LocString COLD = "Chilled";

				// Token: 0x0400CC41 RID: 52289
				public static LocString TEMPERATE = "Temperate";

				// Token: 0x0400CC42 RID: 52290
				public static LocString HOT = "Warm";

				// Token: 0x0400CC43 RID: 52291
				public static LocString VERYHOT = "Hot";

				// Token: 0x0400CC44 RID: 52292
				public static LocString EXTREMEHOT = "Scorching";

				// Token: 0x0400CC45 RID: 52293
				public static LocString MAXHOT = "Molten";

				// Token: 0x0400CC46 RID: 52294
				public static LocString HEATSOURCES = "Heat Source";

				// Token: 0x0400CC47 RID: 52295
				public static LocString HEATSINK = "Heat Sink";

				// Token: 0x0400CC48 RID: 52296
				public static LocString DEFAULT_TEMPERATURE_BUTTON = "Default";

				// Token: 0x02003C45 RID: 15429
				public class TOOLTIPS
				{
					// Token: 0x0400EF79 RID: 61305
					public static LocString TEMPERATURE = "Temperatures reaching {0}";

					// Token: 0x0400EF7A RID: 61306
					public static LocString HEATSOURCES = "Elements displaying this symbol can produce heat";

					// Token: 0x0400EF7B RID: 61307
					public static LocString HEATSINK = "Elements displaying this symbol can absorb heat";
				}
			}

			// Token: 0x02002F66 RID: 12134
			public class STATECHANGE
			{
				// Token: 0x0400CC49 RID: 52297
				public static LocString LOWPOINT = "Low energy state change";

				// Token: 0x0400CC4A RID: 52298
				public static LocString STABLE = "Stable";

				// Token: 0x0400CC4B RID: 52299
				public static LocString HIGHPOINT = "High energy state change";

				// Token: 0x02003C46 RID: 15430
				public class TOOLTIPS
				{
					// Token: 0x0400EF7C RID: 61308
					public static LocString LOWPOINT = "Nearing a low energy state change";

					// Token: 0x0400EF7D RID: 61309
					public static LocString STABLE = "Not near any state changes";

					// Token: 0x0400EF7E RID: 61310
					public static LocString HIGHPOINT = "Nearing high energy state change";
				}
			}

			// Token: 0x02002F67 RID: 12135
			public class HEATFLOW
			{
				// Token: 0x0400CC4C RID: 52300
				public static LocString NAME = "THERMAL TOLERANCE OVERLAY";

				// Token: 0x0400CC4D RID: 52301
				public static LocString HOVERTITLE = "THERMAL TOLERANCE";

				// Token: 0x0400CC4E RID: 52302
				public static LocString BUTTON = "Thermal Tolerance Overlay";

				// Token: 0x0400CC4F RID: 52303
				public static LocString COOLING = "Body Heat Loss";

				// Token: 0x0400CC50 RID: 52304
				public static LocString NEUTRAL = "Comfort Zone";

				// Token: 0x0400CC51 RID: 52305
				public static LocString HEATING = "Body Heat Retention";

				// Token: 0x0400CC52 RID: 52306
				public static LocString COOLING_DUPE = "Body Heat Loss {0}\n\nUncomfortably chilly surroundings";

				// Token: 0x0400CC53 RID: 52307
				public static LocString NEUTRAL_DUPE = "Comfort Zone {0}";

				// Token: 0x0400CC54 RID: 52308
				public static LocString HEATING_DUPE = "Body Heat Loss {0}\n\nUncomfortably toasty surroundings";

				// Token: 0x02003C47 RID: 15431
				public class TOOLTIPS
				{
					// Token: 0x0400EF7F RID: 61311
					public static LocString COOLING = "<b>Body Heat Loss</b>\nUncomfortably cold\n\nDuplicants lose more heat in chilly surroundings than they can absorb\n    • Warm Coats help Duplicants retain body heat";

					// Token: 0x0400EF80 RID: 61312
					public static LocString NEUTRAL = "<b>Comfort Zone</b>\nComfortable area\n\nDuplicants can regulate their internal temperatures in these areas";

					// Token: 0x0400EF81 RID: 61313
					public static LocString HEATING = "<b>Body Heat Retention</b>\nUncomfortably warm\n\nDuplicants absorb more heat in toasty surroundings than they can release";
				}
			}

			// Token: 0x02002F68 RID: 12136
			public class RELATIVETEMPERATURE
			{
				// Token: 0x0400CC55 RID: 52309
				public static LocString NAME = "RELATIVE TEMPERATURE";

				// Token: 0x0400CC56 RID: 52310
				public static LocString HOVERTITLE = "RELATIVE TEMPERATURE";

				// Token: 0x0400CC57 RID: 52311
				public static LocString BUTTON = "Relative Temperature Overlay";
			}

			// Token: 0x02002F69 RID: 12137
			public class ROOMS
			{
				// Token: 0x0400CC58 RID: 52312
				public static LocString NAME = "ROOM OVERLAY";

				// Token: 0x0400CC59 RID: 52313
				public static LocString BUTTON = "Room Overlay";

				// Token: 0x0400CC5A RID: 52314
				public static LocString ROOM = "Room {0}";

				// Token: 0x0400CC5B RID: 52315
				public static LocString HOVERTITLE = "ROOMS";

				// Token: 0x02003C48 RID: 15432
				public static class NOROOM
				{
					// Token: 0x0400EF82 RID: 61314
					public static LocString HEADER = "No Room";

					// Token: 0x0400EF83 RID: 61315
					public static LocString DESC = "Enclose this space with walls and doors to make a room";

					// Token: 0x0400EF84 RID: 61316
					public static LocString TOO_BIG = "<color=#F44A47FF>    • Size: {0} Tiles\n    • Maximum room size: {1} Tiles</color>";
				}

				// Token: 0x02003C49 RID: 15433
				public class TOOLTIPS
				{
					// Token: 0x0400EF85 RID: 61317
					public static LocString ROOM = "Completed Duplicant bedrooms";

					// Token: 0x0400EF86 RID: 61318
					public static LocString NOROOMS = "Duplicants have nowhere to sleep";
				}
			}

			// Token: 0x02002F6A RID: 12138
			public class JOULES
			{
				// Token: 0x0400CC5C RID: 52316
				public static LocString NAME = "JOULES";

				// Token: 0x0400CC5D RID: 52317
				public static LocString HOVERTITLE = "JOULES";

				// Token: 0x0400CC5E RID: 52318
				public static LocString BUTTON = "Joules Overlay";
			}

			// Token: 0x02002F6B RID: 12139
			public class LIGHTING
			{
				// Token: 0x0400CC5F RID: 52319
				public static LocString NAME = "LIGHT OVERLAY";

				// Token: 0x0400CC60 RID: 52320
				public static LocString BUTTON = "Light Overlay";

				// Token: 0x0400CC61 RID: 52321
				public static LocString LITAREA = "Lit Area";

				// Token: 0x0400CC62 RID: 52322
				public static LocString DARK = "Unlit Area";

				// Token: 0x0400CC63 RID: 52323
				public static LocString HOVERTITLE = "LIGHT";

				// Token: 0x0400CC64 RID: 52324
				public static LocString DESC = "{0} Lux";

				// Token: 0x02003C4A RID: 15434
				public class RANGES
				{
					// Token: 0x0400EF87 RID: 61319
					public static LocString NO_LIGHT = "Pitch Black";

					// Token: 0x0400EF88 RID: 61320
					public static LocString VERY_LOW_LIGHT = "Very Dim";

					// Token: 0x0400EF89 RID: 61321
					public static LocString LOW_LIGHT = "Dim";

					// Token: 0x0400EF8A RID: 61322
					public static LocString MEDIUM_LIGHT = "Well Lit";

					// Token: 0x0400EF8B RID: 61323
					public static LocString HIGH_LIGHT = "Bright";

					// Token: 0x0400EF8C RID: 61324
					public static LocString VERY_HIGH_LIGHT = "Brilliant";

					// Token: 0x0400EF8D RID: 61325
					public static LocString MAX_LIGHT = "Blinding";
				}

				// Token: 0x02003C4B RID: 15435
				public class TOOLTIPS
				{
					// Token: 0x0400EF8E RID: 61326
					public static LocString NAME = "LIGHT OVERLAY";

					// Token: 0x0400EF8F RID: 61327
					public static LocString LITAREA = "<b>Lit Area</b>\nWorking in well-lit areas improves Duplicant " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;

					// Token: 0x0400EF90 RID: 61328
					public static LocString DARK = "<b>Unlit Area</b>\nWorking in the dark has no effect on Duplicants";
				}
			}

			// Token: 0x02002F6C RID: 12140
			public class CROP
			{
				// Token: 0x0400CC65 RID: 52325
				public static LocString NAME = "FARMING OVERLAY";

				// Token: 0x0400CC66 RID: 52326
				public static LocString BUTTON = "Farming Overlay";

				// Token: 0x0400CC67 RID: 52327
				public static LocString GROWTH_HALTED = "Halted Growth";

				// Token: 0x0400CC68 RID: 52328
				public static LocString GROWING = "Growing";

				// Token: 0x0400CC69 RID: 52329
				public static LocString FULLY_GROWN = "Fully Grown";

				// Token: 0x02003C4C RID: 15436
				public class TOOLTIPS
				{
					// Token: 0x0400EF91 RID: 61329
					public static LocString GROWTH_HALTED = "<b>Halted Growth</b>\nSubstandard conditions prevent these plants from growing";

					// Token: 0x0400EF92 RID: 61330
					public static LocString GROWING = "<b>Growing</b>\nThese plants are thriving in their current conditions";

					// Token: 0x0400EF93 RID: 61331
					public static LocString FULLY_GROWN = "<b>Fully Grown</b>\nThese plants have reached maturation\n\nSelect the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to batch harvest";
				}
			}

			// Token: 0x02002F6D RID: 12141
			public class LIQUIDPLUMBING
			{
				// Token: 0x0400CC6A RID: 52330
				public static LocString NAME = "PLUMBING OVERLAY";

				// Token: 0x0400CC6B RID: 52331
				public static LocString BUTTON = "Plumbing Overlay";

				// Token: 0x0400CC6C RID: 52332
				public static LocString CONSUMER = "Output Pipe";

				// Token: 0x0400CC6D RID: 52333
				public static LocString FILTERED = "Filtered Output Pipe";

				// Token: 0x0400CC6E RID: 52334
				public static LocString PRODUCER = "Building Intake";

				// Token: 0x0400CC6F RID: 52335
				public static LocString CONNECTED = "Connected";

				// Token: 0x0400CC70 RID: 52336
				public static LocString DISCONNECTED = "Disconnected";

				// Token: 0x0400CC71 RID: 52337
				public static LocString NETWORK = "Liquid Network {0}";

				// Token: 0x0400CC72 RID: 52338
				public static LocString DIAGRAM_BEFORE_ARROW = "Liquid flows from <b>Output Pipe</b>";

				// Token: 0x0400CC73 RID: 52339
				public static LocString DIAGRAM_AFTER_ARROW = "<b>Building Intake</b>";

				// Token: 0x02003C4D RID: 15437
				public class TOOLTIPS
				{
					// Token: 0x0400EF94 RID: 61332
					public static LocString CONNECTED = "Connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

					// Token: 0x0400EF95 RID: 61333
					public static LocString DISCONNECTED = "Not connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

					// Token: 0x0400EF96 RID: 61334
					public static LocString CONSUMER = "<b>Output Pipe</b>\nOutputs send liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING");

					// Token: 0x0400EF97 RID: 61335
					public static LocString FILTERED = "<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING");

					// Token: 0x0400EF98 RID: 61336
					public static LocString PRODUCER = "<b>Building Intake</b>\nIntakes send liquid into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "LIQUIDPIPING");

					// Token: 0x0400EF99 RID: 61337
					public static LocString NETWORK = "Liquid network {0}";
				}
			}

			// Token: 0x02002F6E RID: 12142
			public class GASPLUMBING
			{
				// Token: 0x0400CC74 RID: 52340
				public static LocString NAME = "VENTILATION OVERLAY";

				// Token: 0x0400CC75 RID: 52341
				public static LocString BUTTON = "Ventilation Overlay";

				// Token: 0x0400CC76 RID: 52342
				public static LocString CONSUMER = "Output Pipe";

				// Token: 0x0400CC77 RID: 52343
				public static LocString FILTERED = "Filtered Output Pipe";

				// Token: 0x0400CC78 RID: 52344
				public static LocString PRODUCER = "Building Intake";

				// Token: 0x0400CC79 RID: 52345
				public static LocString CONNECTED = "Connected";

				// Token: 0x0400CC7A RID: 52346
				public static LocString DISCONNECTED = "Disconnected";

				// Token: 0x0400CC7B RID: 52347
				public static LocString NETWORK = "Gas Network {0}";

				// Token: 0x0400CC7C RID: 52348
				public static LocString DIAGRAM_BEFORE_ARROW = "Gas flows from <b>Output Pipe</b>";

				// Token: 0x0400CC7D RID: 52349
				public static LocString DIAGRAM_AFTER_ARROW = "<b>Building Intake</b>";

				// Token: 0x02003C4E RID: 15438
				public class TOOLTIPS
				{
					// Token: 0x0400EF9A RID: 61338
					public static LocString CONNECTED = "Connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING");

					// Token: 0x0400EF9B RID: 61339
					public static LocString DISCONNECTED = "Not connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING");

					// Token: 0x0400EF9C RID: 61340
					public static LocString CONSUMER = string.Concat(new string[]
					{
						"<b>Output Pipe</b>\nOutputs send ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" into ",
						UI.PRE_KEYWORD,
						"Pipes",
						UI.PST_KEYWORD,
						"\n\nMust be on the same network as at least one ",
						UI.FormatAsLink("Intake", "GASPIPING")
					});

					// Token: 0x0400EF9D RID: 61341
					public static LocString FILTERED = string.Concat(new string[]
					{
						"<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" into ",
						UI.PRE_KEYWORD,
						"Pipes",
						UI.PST_KEYWORD,
						"\n\nMust be on the same network as at least one ",
						UI.FormatAsLink("Intake", "GASPIPING")
					});

					// Token: 0x0400EF9E RID: 61342
					public static LocString PRODUCER = "<b>Building Intake</b>\nIntakes send gas into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "GASPIPING");

					// Token: 0x0400EF9F RID: 61343
					public static LocString NETWORK = "Gas network {0}";
				}
			}

			// Token: 0x02002F6F RID: 12143
			public class SUIT
			{
				// Token: 0x0400CC7E RID: 52350
				public static LocString NAME = "EXOSUIT OVERLAY";

				// Token: 0x0400CC7F RID: 52351
				public static LocString BUTTON = "Exosuit Overlay";

				// Token: 0x0400CC80 RID: 52352
				public static LocString SUIT_ICON = "Exosuit";

				// Token: 0x0400CC81 RID: 52353
				public static LocString SUIT_ICON_TOOLTIP = "<b>Exosuit</b>\nHighlights the current location of equippable exosuits";
			}

			// Token: 0x02002F70 RID: 12144
			public class LOGIC
			{
				// Token: 0x0400CC82 RID: 52354
				public static LocString NAME = "AUTOMATION OVERLAY";

				// Token: 0x0400CC83 RID: 52355
				public static LocString BUTTON = "Automation Overlay";

				// Token: 0x0400CC84 RID: 52356
				public static LocString INPUT = "Input Port";

				// Token: 0x0400CC85 RID: 52357
				public static LocString OUTPUT = "Output Port";

				// Token: 0x0400CC86 RID: 52358
				public static LocString RIBBON_INPUT = "Ribbon Input Port";

				// Token: 0x0400CC87 RID: 52359
				public static LocString RIBBON_OUTPUT = "Ribbon Output Port";

				// Token: 0x0400CC88 RID: 52360
				public static LocString RESET_UPDATE = "Reset Port";

				// Token: 0x0400CC89 RID: 52361
				public static LocString CONTROL_INPUT = "Control Port";

				// Token: 0x0400CC8A RID: 52362
				public static LocString CIRCUIT_STATUS_HEADER = "GRID STATUS";

				// Token: 0x0400CC8B RID: 52363
				public static LocString ONE = "Green";

				// Token: 0x0400CC8C RID: 52364
				public static LocString ZERO = "Red";

				// Token: 0x0400CC8D RID: 52365
				public static LocString DISCONNECTED = "DISCONNECTED";

				// Token: 0x02003C4F RID: 15439
				public abstract class TOOLTIPS
				{
					// Token: 0x0400EFA0 RID: 61344
					public static LocString INPUT = "<b>Input Port</b>\nReceives a signal from an automation grid";

					// Token: 0x0400EFA1 RID: 61345
					public static LocString OUTPUT = "<b>Output Port</b>\nSends a signal out to an automation grid";

					// Token: 0x0400EFA2 RID: 61346
					public static LocString RIBBON_INPUT = "<b>Ribbon Input Port</b>\nReceives a 4-bit signal from an automation grid";

					// Token: 0x0400EFA3 RID: 61347
					public static LocString RIBBON_OUTPUT = "<b>Ribbon Output Port</b>\nSends a 4-bit signal out to an automation grid";

					// Token: 0x0400EFA4 RID: 61348
					public static LocString RESET_UPDATE = "<b>Reset Port</b>\nReset a " + BUILDINGS.PREFABS.LOGICMEMORY.NAME + "'s internal Memory to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

					// Token: 0x0400EFA5 RID: 61349
					public static LocString CONTROL_INPUT = "<b>Control Port</b>\nControl the signal selection of a " + BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.NAME + " or " + BUILDINGS.PREFABS.LOGICGATEDEMULTIPLEXER.NAME;

					// Token: 0x0400EFA6 RID: 61350
					public static LocString ONE = "<b>Green</b>\nThis port is currently " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

					// Token: 0x0400EFA7 RID: 61351
					public static LocString ZERO = "<b>Red</b>\nThis port is currently " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

					// Token: 0x0400EFA8 RID: 61352
					public static LocString DISCONNECTED = "<b>Disconnected</b>\nThis port is not connected to an automation grid";
				}
			}

			// Token: 0x02002F71 RID: 12145
			public class CONVEYOR
			{
				// Token: 0x0400CC8E RID: 52366
				public static LocString NAME = "CONVEYOR OVERLAY";

				// Token: 0x0400CC8F RID: 52367
				public static LocString BUTTON = "Conveyor Overlay";

				// Token: 0x0400CC90 RID: 52368
				public static LocString OUTPUT = "Loader";

				// Token: 0x0400CC91 RID: 52369
				public static LocString INPUT = "Receptacle";

				// Token: 0x02003C50 RID: 15440
				public abstract class TOOLTIPS
				{
					// Token: 0x0400EFA9 RID: 61353
					public static LocString OUTPUT = string.Concat(new string[]
					{
						"<b>Loader</b>\nLoads material onto a ",
						UI.PRE_KEYWORD,
						"Conveyor Rail",
						UI.PST_KEYWORD,
						" for transport to Receptacles"
					});

					// Token: 0x0400EFAA RID: 61354
					public static LocString INPUT = string.Concat(new string[]
					{
						"<b>Receptacle</b>\nReceives material from a ",
						UI.PRE_KEYWORD,
						"Conveyor Rail",
						UI.PST_KEYWORD,
						" and stores it for Duplicant use"
					});
				}
			}

			// Token: 0x02002F72 RID: 12146
			public class DECOR
			{
				// Token: 0x0400CC92 RID: 52370
				public static LocString NAME = "DECOR OVERLAY";

				// Token: 0x0400CC93 RID: 52371
				public static LocString BUTTON = "Decor Overlay";

				// Token: 0x0400CC94 RID: 52372
				public static LocString TOTAL = "Total Decor: ";

				// Token: 0x0400CC95 RID: 52373
				public static LocString ENTRY = "{0} {1} {2}";

				// Token: 0x0400CC96 RID: 52374
				public static LocString COUNT = "({0})";

				// Token: 0x0400CC97 RID: 52375
				public static LocString VALUE = "{0}{1}";

				// Token: 0x0400CC98 RID: 52376
				public static LocString VALUE_ZERO = "{0}{1}";

				// Token: 0x0400CC99 RID: 52377
				public static LocString HEADER_POSITIVE = "Positive Value:";

				// Token: 0x0400CC9A RID: 52378
				public static LocString HEADER_NEGATIVE = "Negative Value:";

				// Token: 0x0400CC9B RID: 52379
				public static LocString LOWDECOR = "Negative Decor";

				// Token: 0x0400CC9C RID: 52380
				public static LocString HIGHDECOR = "Positive Decor";

				// Token: 0x0400CC9D RID: 52381
				public static LocString CLUTTER = "Debris";

				// Token: 0x0400CC9E RID: 52382
				public static LocString LIGHTING = "Lighting";

				// Token: 0x0400CC9F RID: 52383
				public static LocString CLOTHING = "{0}'s Outfit";

				// Token: 0x0400CCA0 RID: 52384
				public static LocString CLOTHING_TRAIT_DECORUP = "{0}'s Outfit (Innately Stylish)";

				// Token: 0x0400CCA1 RID: 52385
				public static LocString CLOTHING_TRAIT_DECORDOWN = "{0}'s Outfit (Shabby Dresser)";

				// Token: 0x0400CCA2 RID: 52386
				public static LocString HOVERTITLE = "DECOR";

				// Token: 0x0400CCA3 RID: 52387
				public static LocString MAXIMUM_DECOR = "{0}{1} (Maximum Decor)";

				// Token: 0x02003C51 RID: 15441
				public class TOOLTIPS
				{
					// Token: 0x0400EFAB RID: 61355
					public static LocString LOWDECOR = string.Concat(new string[]
					{
						"<b>Negative Decor</b>\nArea with insufficient ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" values\n* Resources on the floor are considered \"debris\" and will decrease decor"
					});

					// Token: 0x0400EFAC RID: 61356
					public static LocString HIGHDECOR = string.Concat(new string[]
					{
						"<b>Positive Decor</b>\nArea with sufficient ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" values\n* Lighting and aesthetically pleasing buildings increase decor"
					});
				}
			}

			// Token: 0x02002F73 RID: 12147
			public class PRIORITIES
			{
				// Token: 0x0400CCA4 RID: 52388
				public static LocString NAME = "PRIORITY OVERLAY";

				// Token: 0x0400CCA5 RID: 52389
				public static LocString BUTTON = "Priority Overlay";

				// Token: 0x0400CCA6 RID: 52390
				public static LocString ONE = "1 (Low Urgency)";

				// Token: 0x0400CCA7 RID: 52391
				public static LocString ONE_TOOLTIP = "Priority 1";

				// Token: 0x0400CCA8 RID: 52392
				public static LocString TWO = "2";

				// Token: 0x0400CCA9 RID: 52393
				public static LocString TWO_TOOLTIP = "Priority 2";

				// Token: 0x0400CCAA RID: 52394
				public static LocString THREE = "3";

				// Token: 0x0400CCAB RID: 52395
				public static LocString THREE_TOOLTIP = "Priority 3";

				// Token: 0x0400CCAC RID: 52396
				public static LocString FOUR = "4";

				// Token: 0x0400CCAD RID: 52397
				public static LocString FOUR_TOOLTIP = "Priority 4";

				// Token: 0x0400CCAE RID: 52398
				public static LocString FIVE = "5";

				// Token: 0x0400CCAF RID: 52399
				public static LocString FIVE_TOOLTIP = "Priority 5";

				// Token: 0x0400CCB0 RID: 52400
				public static LocString SIX = "6";

				// Token: 0x0400CCB1 RID: 52401
				public static LocString SIX_TOOLTIP = "Priority 6";

				// Token: 0x0400CCB2 RID: 52402
				public static LocString SEVEN = "7";

				// Token: 0x0400CCB3 RID: 52403
				public static LocString SEVEN_TOOLTIP = "Priority 7";

				// Token: 0x0400CCB4 RID: 52404
				public static LocString EIGHT = "8";

				// Token: 0x0400CCB5 RID: 52405
				public static LocString EIGHT_TOOLTIP = "Priority 8";

				// Token: 0x0400CCB6 RID: 52406
				public static LocString NINE = "9 (High Urgency)";

				// Token: 0x0400CCB7 RID: 52407
				public static LocString NINE_TOOLTIP = "Priority 9";
			}

			// Token: 0x02002F74 RID: 12148
			public class DISEASE
			{
				// Token: 0x0400CCB8 RID: 52408
				public static LocString NAME = "GERM OVERLAY";

				// Token: 0x0400CCB9 RID: 52409
				public static LocString BUTTON = "Germ Overlay";

				// Token: 0x0400CCBA RID: 52410
				public static LocString HOVERTITLE = "Germ";

				// Token: 0x0400CCBB RID: 52411
				public static LocString INFECTION_SOURCE = "Germ Source";

				// Token: 0x0400CCBC RID: 52412
				public static LocString INFECTION_SOURCE_TOOLTIP = "<b>Germ Source</b>\nAreas where germs are produced\n•  Placing Wash Basins or Hand Sanitizers near these areas may prevent disease spread";

				// Token: 0x0400CCBD RID: 52413
				public static LocString NO_DISEASE = "Zero surface germs";

				// Token: 0x0400CCBE RID: 52414
				public static LocString DISEASE_NAME_FORMAT = "{0}<color=#{1}></color>";

				// Token: 0x0400CCBF RID: 52415
				public static LocString DISEASE_NAME_FORMAT_NO_COLOR = "{0}";

				// Token: 0x0400CCC0 RID: 52416
				public static LocString DISEASE_FORMAT = "{1} [{0}]<color=#{2}></color>";

				// Token: 0x0400CCC1 RID: 52417
				public static LocString DISEASE_FORMAT_NO_COLOR = "{1} [{0}]";

				// Token: 0x0400CCC2 RID: 52418
				public static LocString CONTAINER_FORMAT = "\n    {0}: {1}";

				// Token: 0x02003C52 RID: 15442
				public class DISINFECT_THRESHOLD_DIAGRAM
				{
					// Token: 0x0400EFAD RID: 61357
					public static LocString UNITS = "Germs";

					// Token: 0x0400EFAE RID: 61358
					public static LocString MIN_LABEL = "0";

					// Token: 0x0400EFAF RID: 61359
					public static LocString MAX_LABEL = "1m";

					// Token: 0x0400EFB0 RID: 61360
					public static LocString THRESHOLD_PREFIX = "Disinfect At:";

					// Token: 0x0400EFB1 RID: 61361
					public static LocString TOOLTIP = "Automatically disinfect any building with more than {NumberOfGerms} germs.";

					// Token: 0x0400EFB2 RID: 61362
					public static LocString TOOLTIP_DISABLED = "Automatic building disinfection disabled.";
				}
			}

			// Token: 0x02002F75 RID: 12149
			public class CROPS
			{
				// Token: 0x0400CCC3 RID: 52419
				public static LocString NAME = "FARMING OVERLAY";

				// Token: 0x0400CCC4 RID: 52420
				public static LocString BUTTON = "Farming Overlay";
			}

			// Token: 0x02002F76 RID: 12150
			public class POWER
			{
				// Token: 0x0400CCC5 RID: 52421
				public static LocString WATTS_GENERATED = "Watts Generated";

				// Token: 0x0400CCC6 RID: 52422
				public static LocString WATTS_CONSUMED = "Watts Consumed";
			}

			// Token: 0x02002F77 RID: 12151
			public class RADIATION
			{
				// Token: 0x0400CCC7 RID: 52423
				public static LocString NAME = "RADIATION";

				// Token: 0x0400CCC8 RID: 52424
				public static LocString BUTTON = "Radiation Overlay";

				// Token: 0x0400CCC9 RID: 52425
				public static LocString DESC = "{rads} per cycle ({description})";

				// Token: 0x0400CCCA RID: 52426
				public static LocString SHIELDING_DESC = "Radiation Blocking: {radiationAbsorptionFactor}";

				// Token: 0x0400CCCB RID: 52427
				public static LocString HOVERTITLE = "RADIATION";

				// Token: 0x02003C53 RID: 15443
				public class RANGES
				{
					// Token: 0x0400EFB3 RID: 61363
					public static LocString NONE = "Completely Safe";

					// Token: 0x0400EFB4 RID: 61364
					public static LocString VERY_LOW = "Mostly Safe";

					// Token: 0x0400EFB5 RID: 61365
					public static LocString LOW = "Barely Safe";

					// Token: 0x0400EFB6 RID: 61366
					public static LocString MEDIUM = "Slight Hazard";

					// Token: 0x0400EFB7 RID: 61367
					public static LocString HIGH = "Significant Hazard";

					// Token: 0x0400EFB8 RID: 61368
					public static LocString VERY_HIGH = "Extreme Hazard";

					// Token: 0x0400EFB9 RID: 61369
					public static LocString MAX = "Maximum Hazard";

					// Token: 0x0400EFBA RID: 61370
					public static LocString INPUTPORT = "Radbolt Input Port";

					// Token: 0x0400EFBB RID: 61371
					public static LocString OUTPUTPORT = "Radbolt Output Port";
				}

				// Token: 0x02003C54 RID: 15444
				public class TOOLTIPS
				{
					// Token: 0x0400EFBC RID: 61372
					public static LocString NONE = "Completely Safe";

					// Token: 0x0400EFBD RID: 61373
					public static LocString VERY_LOW = "Mostly Safe";

					// Token: 0x0400EFBE RID: 61374
					public static LocString LOW = "Barely Safe";

					// Token: 0x0400EFBF RID: 61375
					public static LocString MEDIUM = "Slight Hazard";

					// Token: 0x0400EFC0 RID: 61376
					public static LocString HIGH = "Significant Hazard";

					// Token: 0x0400EFC1 RID: 61377
					public static LocString VERY_HIGH = "Extreme Hazard";

					// Token: 0x0400EFC2 RID: 61378
					public static LocString MAX = "Maximum Hazard";

					// Token: 0x0400EFC3 RID: 61379
					public static LocString INPUTPORT = "Radbolt Input Port";

					// Token: 0x0400EFC4 RID: 61380
					public static LocString OUTPUTPORT = "Radbolt Output Port";
				}
			}
		}

		// Token: 0x02002536 RID: 9526
		public class TABLESCREENS
		{
			// Token: 0x0400A6DA RID: 42714
			public static LocString DUPLICANT_PROPERNAME = "<b>{0}</b>";

			// Token: 0x0400A6DB RID: 42715
			public static LocString SELECT_DUPLICANT_BUTTON = UI.CLICK(UI.ClickType.Click) + " to select <b>{0}</b>";

			// Token: 0x0400A6DC RID: 42716
			public static LocString GOTO_DUPLICANT_BUTTON = "Double-" + UI.CLICK(UI.ClickType.click) + " to go to <b>{0}</b>";

			// Token: 0x0400A6DD RID: 42717
			public static LocString COLUMN_SORT_BY_NAME = "Sort by <b>Name</b>";

			// Token: 0x0400A6DE RID: 42718
			public static LocString COLUMN_SORT_BY_STRESS = "Sort by <b>Stress</b>";

			// Token: 0x0400A6DF RID: 42719
			public static LocString COLUMN_SORT_BY_HITPOINTS = "Sort by <b>Health</b>";

			// Token: 0x0400A6E0 RID: 42720
			public static LocString COLUMN_SORT_BY_SICKNESSES = "Sort by <b>Disease</b>";

			// Token: 0x0400A6E1 RID: 42721
			public static LocString COLUMN_SORT_BY_FULLNESS = "Sort by <b>Fullness</b>";

			// Token: 0x0400A6E2 RID: 42722
			public static LocString COLUMN_SORT_BY_EATEN_TODAY = "Sort by number of <b>Calories</b> consumed today";

			// Token: 0x0400A6E3 RID: 42723
			public static LocString COLUMN_SORT_BY_EXPECTATIONS = "Sort by <b>Morale</b>";

			// Token: 0x0400A6E4 RID: 42724
			public static LocString COLUMN_SORT_BY_POWERBANKS = "Sort by <b>Power Banks</b>";

			// Token: 0x0400A6E5 RID: 42725
			public static LocString NA = "N/A";

			// Token: 0x0400A6E6 RID: 42726
			public static LocString INFORMATION_NOT_AVAILABLE_TOOLTIP = "Information is not available because {1} is in {0}";

			// Token: 0x0400A6E7 RID: 42727
			public static LocString NOBODY_HERE = "Nobody here...";
		}

		// Token: 0x02002537 RID: 9527
		public class CONSUMABLESSCREEN
		{
			// Token: 0x0400A6E8 RID: 42728
			public static LocString TITLE = "CONSUMABLES";

			// Token: 0x0400A6E9 RID: 42729
			public static LocString TOOLTIP_TOGGLE_ALL = "Toggle <b>all</b> food permissions <b>colonywide</b>";

			// Token: 0x0400A6EA RID: 42730
			public static LocString TOOLTIP_TOGGLE_COLUMN = "Toggle colonywide <b>{0}</b> permission";

			// Token: 0x0400A6EB RID: 42731
			public static LocString TOOLTIP_TOGGLE_ROW = "Toggle <b>all consumable permissions</b> for <b>{0}</b>";

			// Token: 0x0400A6EC RID: 42732
			public static LocString NEW_MINIONS_TOOLTIP_TOGGLE_ROW = "Toggle <b>all consumable permissions</b> for <b>New Duplicants</b>";

			// Token: 0x0400A6ED RID: 42733
			public static LocString NEW_MINIONS_FOOD_PERMISSION_ON = string.Concat(new string[]
			{
				"<b>New Duplicants</b> are <b>allowed</b> to eat \n",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				"</b> by default"
			});

			// Token: 0x0400A6EE RID: 42734
			public static LocString NEW_MINIONS_FOOD_PERMISSION_OFF = string.Concat(new string[]
			{
				"<b>New Duplicants</b> are <b>not allowed</b> to eat \n",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				" by default"
			});

			// Token: 0x0400A6EF RID: 42735
			public static LocString FOOD_PERMISSION_ON = "<b>{0}</b> is <b>allowed</b> to eat " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x0400A6F0 RID: 42736
			public static LocString FOOD_PERMISSION_OFF = "<b>{0}</b> is <b>not allowed</b> to eat " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x0400A6F1 RID: 42737
			public static LocString FOOD_CANT_CONSUME = "<b>{0}</b> <b>physically cannot</b> eat\n" + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x0400A6F2 RID: 42738
			public static LocString FOOD_REFUSE = "<b>{0}</b> <b>refuses</b> to eat\n" + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x0400A6F3 RID: 42739
			public static LocString FOOD_AVAILABLE = "Available: {0}";

			// Token: 0x0400A6F4 RID: 42740
			public static LocString FOOD_MORALE = UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD + ": {0}";

			// Token: 0x0400A6F5 RID: 42741
			public static LocString FOOD_QUALITY = UI.PRE_KEYWORD + "Quality" + UI.PST_KEYWORD + ": {0}";

			// Token: 0x0400A6F6 RID: 42742
			public static LocString FOOD_QUALITY_VS_EXPECTATION = string.Concat(new string[]
			{
				"\nThis food will give ",
				UI.PRE_KEYWORD,
				"Morale",
				UI.PST_KEYWORD,
				" <b>{0}</b> if {1} eats it"
			});

			// Token: 0x0400A6F7 RID: 42743
			public static LocString CANNOT_ADJUST_PERMISSIONS = "Cannot adjust consumable permissions because they're in {0}";
		}

		// Token: 0x02002538 RID: 9528
		public class JOBSSCREEN
		{
			// Token: 0x0400A6F8 RID: 42744
			public static LocString TITLE = "MANAGE DUPLICANT PRIORITIES";

			// Token: 0x0400A6F9 RID: 42745
			public static LocString TOOLTIP_TOGGLE_ALL = "Set priority of all Errand Types colonywide";

			// Token: 0x0400A6FA RID: 42746
			public static LocString HEADER_TOOLTIP = string.Concat(new string[]
			{
				"<size=16>{Job} Errand Type</size>\n\n{Details}\n\nDuplicants will first choose what ",
				UI.PRE_KEYWORD,
				"Errand Type",
				UI.PST_KEYWORD,
				" to perform based on ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				",\nthen they will choose individual tasks within that type using ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by the ",
				UI.FormatAsLink("Priority Tool", "PRIORITIES"),
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities)
			});

			// Token: 0x0400A6FB RID: 42747
			public static LocString HEADER_DETAILS_TOOLTIP = "{Description}\n\nAffected errands: {ChoreList}";

			// Token: 0x0400A6FC RID: 42748
			public static LocString HEADER_CHANGE_TOOLTIP = string.Concat(new string[]
			{
				"Set the priority for the ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type colonywide\n"
			});

			// Token: 0x0400A6FD RID: 42749
			public static LocString NEW_MINION_ITEM_TOOLTIP = string.Concat(new string[]
			{
				"The ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type is automatically a {Priority} ",
				UI.PRE_KEYWORD,
				"Priority",
				UI.PST_KEYWORD,
				" for <b>Arriving Duplicants</b>"
			});

			// Token: 0x0400A6FE RID: 42750
			public static LocString ITEM_TOOLTIP = UI.PRE_KEYWORD + "{Job}" + UI.PST_KEYWORD + " Priority for {Name}:\n<b>{Priority} Priority ({PriorityValue})</b>";

			// Token: 0x0400A6FF RID: 42751
			public static LocString MINION_SKILL_TOOLTIP = string.Concat(new string[]
			{
				"{Name}'s ",
				UI.PRE_KEYWORD,
				"{Attribute}",
				UI.PST_KEYWORD,
				" Skill: "
			});

			// Token: 0x0400A700 RID: 42752
			public static LocString TRAIT_DISABLED = string.Concat(new string[]
			{
				"{Name} possesses the ",
				UI.PRE_KEYWORD,
				"{Trait}",
				UI.PST_KEYWORD,
				" trait and <b>cannot</b> do ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errands"
			});

			// Token: 0x0400A701 RID: 42753
			public static LocString INCREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Prioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>New Duplicants</b>"
			});

			// Token: 0x0400A702 RID: 42754
			public static LocString DECREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Deprioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>New Duplicants</b>"
			});

			// Token: 0x0400A703 RID: 42755
			public static LocString INCREASE_ROW_PRIORITY_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Prioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>{Name}</b>"
			});

			// Token: 0x0400A704 RID: 42756
			public static LocString DECREASE_ROW_PRIORITY_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Deprioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>{Name}</b>"
			});

			// Token: 0x0400A705 RID: 42757
			public static LocString INCREASE_PRIORITY_TUTORIAL = "{Hotkey} Increase Priority";

			// Token: 0x0400A706 RID: 42758
			public static LocString DECREASE_PRIORITY_TUTORIAL = "{Hotkey} Decrease Priority";

			// Token: 0x0400A707 RID: 42759
			public static LocString CANNOT_ADJUST_PRIORITY = string.Concat(new string[]
			{
				"Priorities for ",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				" cannot be adjusted currently because they're in {1}"
			});

			// Token: 0x0400A708 RID: 42760
			public static LocString SORT_TOOLTIP = string.Concat(new string[]
			{
				"Sort by the ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type"
			});

			// Token: 0x0400A709 RID: 42761
			public static LocString DISABLED_TOOLTIP = string.Concat(new string[]
			{
				"{Name} may not perform ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errands"
			});

			// Token: 0x0400A70A RID: 42762
			public static LocString OPTIONS = "Options";

			// Token: 0x0400A70B RID: 42763
			public static LocString TOGGLE_ADVANCED_MODE = "Enable Proximity";

			// Token: 0x0400A70C RID: 42764
			public static LocString TOGGLE_ADVANCED_MODE_TOOLTIP = "<b>Errand Proximity Settings</b>\n\nEnabling Proximity settings tells my Duplicants to always choose the closest, most urgent errand to perform.\n\nWhen disabled, Duplicants will choose between two high priority errands based on a hidden priority hierarchy instead.\n\nEnabling Proximity helps cut down on travel time in areas with lots of high priority errands, and is useful for large colonies.";

			// Token: 0x0400A70D RID: 42765
			public static LocString RESET_SETTINGS = "Reset Priorities";

			// Token: 0x0400A70E RID: 42766
			public static LocString RESET_SETTINGS_TOOLTIP = "<b>Reset Priorities</b>\n\nReturns all priorities to their default values.\n\nProximity Enabled: Priorities will be adjusted high-to-low.\n\nProximity Disabled: All priorities will be reset to neutral.";

			// Token: 0x02002F78 RID: 12152
			public class PRIORITY
			{
				// Token: 0x0400CCCC RID: 52428
				public static LocString VERYHIGH = "Very High";

				// Token: 0x0400CCCD RID: 52429
				public static LocString HIGH = "High";

				// Token: 0x0400CCCE RID: 52430
				public static LocString STANDARD = "Standard";

				// Token: 0x0400CCCF RID: 52431
				public static LocString LOW = "Low";

				// Token: 0x0400CCD0 RID: 52432
				public static LocString VERYLOW = "Very Low";

				// Token: 0x0400CCD1 RID: 52433
				public static LocString DISABLED = "Disallowed";
			}

			// Token: 0x02002F79 RID: 12153
			public class PRIORITY_CLASS
			{
				// Token: 0x0400CCD2 RID: 52434
				public static LocString IDLE = "Idle";

				// Token: 0x0400CCD3 RID: 52435
				public static LocString BASIC = "Normal";

				// Token: 0x0400CCD4 RID: 52436
				public static LocString HIGH = "Urgent";

				// Token: 0x0400CCD5 RID: 52437
				public static LocString PERSONAL_NEEDS = "Personal Needs";

				// Token: 0x0400CCD6 RID: 52438
				public static LocString EMERGENCY = "Emergency";

				// Token: 0x0400CCD7 RID: 52439
				public static LocString COMPULSORY = "Involuntary";
			}
		}

		// Token: 0x02002539 RID: 9529
		public class VITALSSCREEN
		{
			// Token: 0x0400A70F RID: 42767
			public static LocString HEALTH = "Health";

			// Token: 0x0400A710 RID: 42768
			public static LocString SICKNESS = "Diseases";

			// Token: 0x0400A711 RID: 42769
			public static LocString NO_SICKNESSES = "No diseases";

			// Token: 0x0400A712 RID: 42770
			public static LocString MULTIPLE_SICKNESSES = "Multiple diseases ({0})";

			// Token: 0x0400A713 RID: 42771
			public static LocString SICKNESS_REMAINING = "{0}\n({1})";

			// Token: 0x0400A714 RID: 42772
			public static LocString STRESS = "Stress";

			// Token: 0x0400A715 RID: 42773
			public static LocString EXPECTATIONS = "Expectations";

			// Token: 0x0400A716 RID: 42774
			public static LocString CALORIES = "Fullness";

			// Token: 0x0400A717 RID: 42775
			public static LocString EATEN_TODAY = "Eaten Today";

			// Token: 0x0400A718 RID: 42776
			public static LocString EATEN_TODAY_TOOLTIP = "Consumed {0} of food this cycle";

			// Token: 0x0400A719 RID: 42777
			public static LocString ATMOSPHERE_CONDITION = "Atmosphere:";

			// Token: 0x0400A71A RID: 42778
			public static LocString SUBMERSION = "Liquid Level";

			// Token: 0x0400A71B RID: 42779
			public static LocString NOT_DROWNING = "Liquid Level";

			// Token: 0x0400A71C RID: 42780
			public static LocString FOOD_EXPECTATIONS = "Food Expectation";

			// Token: 0x0400A71D RID: 42781
			public static LocString FOOD_EXPECTATIONS_TOOLTIP = "This Duplicant desires food that is {0} quality or better";

			// Token: 0x0400A71E RID: 42782
			public static LocString DECOR_EXPECTATIONS = "Decor Expectation";

			// Token: 0x0400A71F RID: 42783
			public static LocString DECOR_EXPECTATIONS_TOOLTIP = "This Duplicant desires decor that is {0} or higher";

			// Token: 0x0400A720 RID: 42784
			public static LocString QUALITYOFLIFE_EXPECTATIONS = "Morale";

			// Token: 0x0400A721 RID: 42785
			public static LocString QUALITYOFLIFE_EXPECTATIONS_TOOLTIP = "This Duplicant requires " + UI.FormatAsLink("{0} Morale", "MORALE") + ".\n\nCurrent Morale:";

			// Token: 0x0400A722 RID: 42786
			public static LocString POLLINATION = "Pollination";

			// Token: 0x02002F7A RID: 12154
			public class CONDITIONS_GROWING
			{
				// Token: 0x02003C55 RID: 15445
				public class WILD
				{
					// Token: 0x0400EFC5 RID: 61381
					public static LocString BASE = "<b>Wild Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400EFC6 RID: 61382
					public static LocString TOOLTIP = "This plant will take {0} to grow in the wild";
				}

				// Token: 0x02003C56 RID: 15446
				public class DOMESTIC
				{
					// Token: 0x0400EFC7 RID: 61383
					public static LocString BASE = "<b>Domestic Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400EFC8 RID: 61384
					public static LocString TOOLTIP = "This plant will take {0} to grow domestically";
				}

				// Token: 0x02003C57 RID: 15447
				public class ADDITIONAL_DOMESTIC
				{
					// Token: 0x0400EFC9 RID: 61385
					public static LocString BASE = "<b>Additional Domestic Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400EFCA RID: 61386
					public static LocString TOOLTIP = "This plant will take {0} to grow domestically";
				}

				// Token: 0x02003C58 RID: 15448
				public class WILD_DECOR
				{
					// Token: 0x0400EFCB RID: 61387
					public static LocString BASE = "<b>Wild Growth</b>";

					// Token: 0x0400EFCC RID: 61388
					public static LocString TOOLTIP = "This plant must have these requirements met to grow in the wild";
				}

				// Token: 0x02003C59 RID: 15449
				public class WILD_INSTANT
				{
					// Token: 0x0400EFCD RID: 61389
					public static LocString BASE = "<b>Wild Growth\n[{0}% Throughput]</b>";

					// Token: 0x0400EFCE RID: 61390
					public static LocString TOOLTIP = "This plant must have these requirements met to grow in the wild";
				}

				// Token: 0x02003C5A RID: 15450
				public class ADDITIONAL_DOMESTIC_INSTANT
				{
					// Token: 0x0400EFCF RID: 61391
					public static LocString BASE = "<b>Domestic Growth\n[{0}% Throughput]</b>";

					// Token: 0x0400EFD0 RID: 61392
					public static LocString TOOLTIP = "This plant must have these requirements met to grow domestically";
				}
			}
		}

		// Token: 0x0200253A RID: 9530
		public class SCHEDULESCREEN
		{
			// Token: 0x0400A723 RID: 42787
			public static LocString SCHEDULE_EDITOR = "SCHEDULE EDITOR";

			// Token: 0x0400A724 RID: 42788
			public static LocString SCHEDULE_NAME_DEFAULT = "Default Standard Schedule";

			// Token: 0x0400A725 RID: 42789
			public static LocString SCHEDULE_NAME_NEW = "New Schedule";

			// Token: 0x0400A726 RID: 42790
			public static LocString SCHEDULE_NAME_FORMAT = "Schedule {0}";

			// Token: 0x0400A727 RID: 42791
			public static LocString SCHEDULE_NAME_DEFAULT_BIONIC = "Default Bionic Schedule";

			// Token: 0x0400A728 RID: 42792
			public static LocString SCHEDULE_DROPDOWN_ASSIGNED = "{0} (Assigned)";

			// Token: 0x0400A729 RID: 42793
			public static LocString SCHEDULE_DROPDOWN_BLANK = "<i>Move Duplicant...</i>";

			// Token: 0x0400A72A RID: 42794
			public static LocString SCHEDULE_DOWNTIME_MORALE = "Duplicants will receive {0} Morale from the scheduled Downtime shifts";

			// Token: 0x0400A72B RID: 42795
			public static LocString RENAME_BUTTON_TOOLTIP = "Rename custom schedule";

			// Token: 0x0400A72C RID: 42796
			public static LocString ALARM_BUTTON_ON_TOOLTIP = "Toggle Notifications\n\nSounds and notifications will play when shifts change for this schedule.\n\nENABLED\n" + UI.CLICK(UI.ClickType.Click) + " to disable";

			// Token: 0x0400A72D RID: 42797
			public static LocString ALARM_BUTTON_OFF_TOOLTIP = "Toggle Notifications\n\nNo sounds or notifications will play for this schedule.\n\nDISABLED\n" + UI.CLICK(UI.ClickType.Click) + " to enable";

			// Token: 0x0400A72E RID: 42798
			public static LocString DELETE_BUTTON_TOOLTIP = "Delete Schedule";

			// Token: 0x0400A72F RID: 42799
			public static LocString PAINT_TOOLS = "Paint Tools:";

			// Token: 0x0400A730 RID: 42800
			public static LocString ADD_SCHEDULE = "Add New Schedule";

			// Token: 0x0400A731 RID: 42801
			public static LocString POO = "dar";

			// Token: 0x0400A732 RID: 42802
			public static LocString DOWNTIME_MORALE = "Downtime Morale: {0}";

			// Token: 0x0400A733 RID: 42803
			public static LocString ALARM_TITLE_ENABLED = "Alarm On";

			// Token: 0x0400A734 RID: 42804
			public static LocString ALARM_TITLE_DISABLED = "Alarm Off";

			// Token: 0x0400A735 RID: 42805
			public static LocString SETTINGS = "Settings";

			// Token: 0x0400A736 RID: 42806
			public static LocString ALARM_BUTTON = "Shift Alarms";

			// Token: 0x0400A737 RID: 42807
			public static LocString RESET_SETTINGS = "Reset Shifts";

			// Token: 0x0400A738 RID: 42808
			public static LocString RESET_SETTINGS_TOOLTIP = "Restore this schedule to default shifts";

			// Token: 0x0400A739 RID: 42809
			public static LocString DELETE_SCHEDULE = "Delete Schedule";

			// Token: 0x0400A73A RID: 42810
			public static LocString DELETE_SCHEDULE_TOOLTIP = "Remove this schedule and unassign all Duplicants from it";

			// Token: 0x0400A73B RID: 42811
			public static LocString DUPLICANT_NIGHTOWL_TOOLTIP = string.Concat(new string[]
			{
				DUPLICANTS.TRAITS.NIGHTOWL.NAME,
				"\n• All ",
				UI.PRE_KEYWORD,
				"Attributes",
				UI.PST_KEYWORD,
				" <b>+3</b> at night"
			});

			// Token: 0x0400A73C RID: 42812
			public static LocString DUPLICANT_EARLYBIRD_TOOLTIP = string.Concat(new string[]
			{
				DUPLICANTS.TRAITS.EARLYBIRD.NAME,
				"\n• All ",
				UI.PRE_KEYWORD,
				"Attributes",
				UI.PST_KEYWORD,
				" <b>+2</b> in the morning"
			});

			// Token: 0x0400A73D RID: 42813
			public static LocString SHIFT_SCHEDULE_LEFT_TOOLTIP = "Shift all schedule blocks left";

			// Token: 0x0400A73E RID: 42814
			public static LocString SHIFT_SCHEDULE_RIGHT_TOOLTIP = "Shift all schedule blocks right";

			// Token: 0x0400A73F RID: 42815
			public static LocString SHIFT_SCHEDULE_UP_TOOLTIP = "Swap this row with the one above it";

			// Token: 0x0400A740 RID: 42816
			public static LocString SHIFT_SCHEDULE_DOWN_TOOLTIP = "Swap this row with the one below it";

			// Token: 0x0400A741 RID: 42817
			public static LocString DUPLICATE_SCHEDULE_TIMETABLE = "Duplicate this row";

			// Token: 0x0400A742 RID: 42818
			public static LocString DELETE_SCHEDULE_TIMETABLE = "Delete this row\n\nSchedules must have two or more rows in order for one row to be deleted";

			// Token: 0x0400A743 RID: 42819
			public static LocString DUPLICATE_SCHEDULE = "Duplicate this schedule";
		}

		// Token: 0x0200253B RID: 9531
		public class COLONYLOSTSCREEN
		{
			// Token: 0x0400A744 RID: 42820
			public static LocString COLONYLOST = "COLONY LOST";

			// Token: 0x0400A745 RID: 42821
			public static LocString COLONYLOSTDESCRIPTION = "All Duplicants are dead or incapacitated.";

			// Token: 0x0400A746 RID: 42822
			public static LocString RESTARTPROMPT = "Press <color=#F44A47><b>[ESC]</b></color> to return to a previous colony, or begin a new one.";

			// Token: 0x0400A747 RID: 42823
			public static LocString DISMISSBUTTON = "DISMISS";

			// Token: 0x0400A748 RID: 42824
			public static LocString QUITBUTTON = "MAIN MENU";
		}

		// Token: 0x0200253C RID: 9532
		public class VICTORYSCREEN
		{
			// Token: 0x0400A749 RID: 42825
			public static LocString HEADER = "SUCCESS: IMPERATIVE ACHIEVED!";

			// Token: 0x0400A74A RID: 42826
			public static LocString DESCRIPTION = "I have fulfilled the conditions of one of my Hardwired Imperatives";

			// Token: 0x0400A74B RID: 42827
			public static LocString RESTARTPROMPT = "Press <color=#F44A47><b>[ESC]</b></color> to retire the colony and begin anew.";

			// Token: 0x0400A74C RID: 42828
			public static LocString DISMISSBUTTON = "DISMISS";

			// Token: 0x0400A74D RID: 42829
			public static LocString RETIREBUTTON = "RETIRE COLONY";
		}

		// Token: 0x0200253D RID: 9533
		public class GENESHUFFLERMESSAGE
		{
			// Token: 0x0400A74E RID: 42830
			public static LocString HEADER = "NEURAL VACILLATION COMPLETE";

			// Token: 0x0400A74F RID: 42831
			public static LocString BODY_SUCCESS = "Whew! <b>{0}'s</b> brain is still vibrating, but they've never felt better!\n\n<b>{0}</b> acquired the <b>{1}</b> trait.\n\n<b>{1}:</b>\n{2}";

			// Token: 0x0400A750 RID: 42832
			public static LocString BODY_FAILURE = "The machine attempted to alter this Duplicant, but there's no improving on perfection.\n\n<b>{0}</b> already has all positive traits!";

			// Token: 0x0400A751 RID: 42833
			public static LocString DISMISSBUTTON = "DISMISS";
		}

		// Token: 0x0200253E RID: 9534
		public class PRINTERCEPTORSCREEN
		{
			// Token: 0x0400A752 RID: 42834
			public static LocString HEADER = "PRINTABLES MENU";

			// Token: 0x0400A753 RID: 42835
			public static LocString SELECT_ENTITY = "View";

			// Token: 0x0400A754 RID: 42836
			public static LocString SELECT_ENTITY_TOOLTIP = "Click for more information about this printable";

			// Token: 0x0400A755 RID: 42837
			public static LocString PRINT = "Print";

			// Token: 0x0400A756 RID: 42838
			public static LocString PRINT_TOOLTIP = string.Concat(new string[]
			{
				"Click to print selected item\n\nThe number of ",
				UI.PRE_KEYWORD,
				"Data Banks",
				UI.PST_KEYWORD,
				" required to print additional copies of this item will increase by {0} each time"
			});

			// Token: 0x0400A757 RID: 42839
			public static LocString PRINT_TOOLTIP_DISABLED = "Not enough stored materials to complete this printing job\n\nCheck building inventory";

			// Token: 0x0400A758 RID: 42840
			public static LocString DATABANKS_AVAILABLE = "Data Banks Available: {0}";

			// Token: 0x0400A759 RID: 42841
			public static LocString DATABANKS_COST = "Data Banks Required: {0}";
		}

		// Token: 0x0200253F RID: 9535
		public class CRASHSCREEN
		{
			// Token: 0x0400A75A RID: 42842
			public static LocString TITLE = "\"Whoops! We're sorry, but it seems your game has encountered an error. It's okay though - these errors are how we find and fix problems to make our game more fun for everyone. If you use the box below to submit a crash report to us, we can use this information to get the issue sorted out.\"";

			// Token: 0x0400A75B RID: 42843
			public static LocString TITLE_MODS = "\"Oops-a-daisy! We're sorry, but it seems your game has encountered an error. If you uncheck all of the mods below, we will be able to help the next time this happens. Any mods that could be related to this error have already been unchecked.\"";

			// Token: 0x0400A75C RID: 42844
			public static LocString HEADER = "OPTIONAL CRASH DESCRIPTION";

			// Token: 0x0400A75D RID: 42845
			public static LocString HEADER_MODS = "ACTIVE MODS";

			// Token: 0x0400A75E RID: 42846
			public static LocString BODY = "Help! A black hole ate my game!";

			// Token: 0x0400A75F RID: 42847
			public static LocString THANKYOU = "Thank you!\n\nYou're making our game better, one crash at a time.";

			// Token: 0x0400A760 RID: 42848
			public static LocString UPLOAD_FAILED = "There was an issue in reporting this crash.\n\nPlease submit a bug report at:\n<u>https://forums.kleientertainment.com/klei-bug-tracker/oni/</u>";

			// Token: 0x0400A761 RID: 42849
			public static LocString UPLOADINFO = "UPLOAD ADDITIONAL INFO ({0})";

			// Token: 0x0400A762 RID: 42850
			public static LocString REPORTBUTTON = "REPORT CRASH";

			// Token: 0x0400A763 RID: 42851
			public static LocString REPORTING = "REPORTING, PLEASE WAIT...";

			// Token: 0x0400A764 RID: 42852
			public static LocString CONTINUEBUTTON = "CONTINUE GAME";

			// Token: 0x0400A765 RID: 42853
			public static LocString MOREINFOBUTTON = "MORE INFO";

			// Token: 0x0400A766 RID: 42854
			public static LocString COPYTOCLIPBOARDBUTTON = "COPY TO CLIPBOARD";

			// Token: 0x0400A767 RID: 42855
			public static LocString QUITBUTTON = "QUIT TO DESKTOP";

			// Token: 0x0400A768 RID: 42856
			public static LocString SAVEFAILED = "Save Failed: {0}";

			// Token: 0x0400A769 RID: 42857
			public static LocString LOADFAILED = "Load Failed: {0}\nSave Version: {1}\nExpected: {2}";

			// Token: 0x0400A76A RID: 42858
			public static LocString REPORTEDERROR_SUCCESS = "Thank you for reporting this error.";

			// Token: 0x0400A76B RID: 42859
			public static LocString REPORTEDERROR_FAILURE_TOO_LARGE = "Unable to report error. Save file is too large. Please contact us using the bug tracker.";

			// Token: 0x0400A76C RID: 42860
			public static LocString REPORTEDERROR_FAILURE = "Unable to report error. Please contact us using the bug tracker.";

			// Token: 0x0400A76D RID: 42861
			public static LocString UPLOADINPROGRESS = "Submitting {0}";
		}

		// Token: 0x02002540 RID: 9536
		public class DEMOOVERSCREEN
		{
			// Token: 0x0400A76E RID: 42862
			public static LocString TIMEREMAINING = "Demo time remaining:";

			// Token: 0x0400A76F RID: 42863
			public static LocString TIMERTOOLTIP = "Demo time remaining";

			// Token: 0x0400A770 RID: 42864
			public static LocString TIMERINACTIVE = "Timer inactive";

			// Token: 0x0400A771 RID: 42865
			public static LocString DEMOOVER = "END OF DEMO";

			// Token: 0x0400A772 RID: 42866
			public static LocString DESCRIPTION = "Thank you for playing <color=#F44A47>Oxygen Not Included</color>!";

			// Token: 0x0400A773 RID: 42867
			public static LocString DESCRIPTION_2 = "";

			// Token: 0x0400A774 RID: 42868
			public static LocString QUITBUTTON = "RESET";
		}

		// Token: 0x02002541 RID: 9537
		public class CREDITSSCREEN
		{
			// Token: 0x0400A775 RID: 42869
			public static LocString TITLE = "CREDITS";

			// Token: 0x0400A776 RID: 42870
			public static LocString CLOSEBUTTON = "CLOSE";

			// Token: 0x02002F7B RID: 12155
			public class THIRD_PARTY
			{
				// Token: 0x0400CCD8 RID: 52440
				public static LocString FMOD = "FMOD Sound System\nCopyright Firelight Technologies";

				// Token: 0x0400CCD9 RID: 52441
				public static LocString HARMONY = "Harmony by Andreas Pardeike";
			}
		}

		// Token: 0x02002542 RID: 9538
		public class ALLRESOURCESSCREEN
		{
			// Token: 0x0400A777 RID: 42871
			public static LocString RESOURCES_TITLE = "RESOURCES";

			// Token: 0x0400A778 RID: 42872
			public static LocString RESOURCES = "Resources";

			// Token: 0x0400A779 RID: 42873
			public static LocString SEARCH = "Search";

			// Token: 0x0400A77A RID: 42874
			public static LocString NAME = "Resource";

			// Token: 0x0400A77B RID: 42875
			public static LocString TOTAL = "Total";

			// Token: 0x0400A77C RID: 42876
			public static LocString AVAILABLE = "Available";

			// Token: 0x0400A77D RID: 42877
			public static LocString RESERVED = "Reserved";

			// Token: 0x0400A77E RID: 42878
			public static LocString SEARCH_PLACEHODLER = "Enter text...";

			// Token: 0x0400A77F RID: 42879
			public static LocString FIRST_FRAME_NO_DATA = "...";

			// Token: 0x0400A780 RID: 42880
			public static LocString PIN_TOOLTIP = "Check to pin resource to side panel";

			// Token: 0x0400A781 RID: 42881
			public static LocString UNPIN_TOOLTIP = "Unpin resource";
		}

		// Token: 0x02002543 RID: 9539
		public class PRIORITYSCREEN
		{
			// Token: 0x0400A782 RID: 42882
			public static LocString BASIC = "Set the order in which specific pending errands should be done\n\n1: Least Urgent\n9: Most Urgent";

			// Token: 0x0400A783 RID: 42883
			public static LocString HIGH = "";

			// Token: 0x0400A784 RID: 42884
			public static LocString TOP_PRIORITY = "Top Priority\n\nThis priority will override all other priorities and set the colony on Yellow Alert until the errand is completed";

			// Token: 0x0400A785 RID: 42885
			public static LocString HIGH_TOGGLE = "";

			// Token: 0x0400A786 RID: 42886
			public static LocString OPEN_JOBS_SCREEN = string.Concat(new string[]
			{
				UI.CLICK(UI.ClickType.Click),
				" to open the Priorities Screen\n\nDuplicants will first decide what to work on based on their ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				", and then decide where to work based on ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD
			});

			// Token: 0x0400A787 RID: 42887
			public static LocString DIAGRAM = string.Concat(new string[]
			{
				"Duplicants will first choose what ",
				UI.PRE_KEYWORD,
				"Errand Type",
				UI.PST_KEYWORD,
				" to perform using their ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities),
				"\n\nThey will then choose one ",
				UI.PRE_KEYWORD,
				"Errand",
				UI.PST_KEYWORD,
				" from within that type using the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by this tool"
			});

			// Token: 0x0400A788 RID: 42888
			public static LocString DIAGRAM_TITLE = "BUILDING PRIORITY";
		}

		// Token: 0x02002544 RID: 9540
		public class RESOURCESCREEN
		{
			// Token: 0x0400A789 RID: 42889
			public static LocString HEADER = "RESOURCES";

			// Token: 0x0400A78A RID: 42890
			public static LocString CATEGORY_TOOLTIP = "Counts all unallocated resources within reach\n\n" + UI.CLICK(UI.ClickType.Click) + " to expand";

			// Token: 0x0400A78B RID: 42891
			public static LocString AVAILABLE_TOOLTIP = "Available: <b>{0}</b>\n({1} of {2} allocated to pending errands)";

			// Token: 0x0400A78C RID: 42892
			public static LocString TREND_TOOLTIP = "The available amount of this resource has {0} {1} in the last cycle";

			// Token: 0x0400A78D RID: 42893
			public static LocString TREND_TOOLTIP_NO_CHANGE = "The available amount of this resource has NOT CHANGED in the last cycle";

			// Token: 0x0400A78E RID: 42894
			public static LocString FLAT_STR = "<b>NOT CHANGED</b>";

			// Token: 0x0400A78F RID: 42895
			public static LocString INCREASING_STR = "<color=" + Constants.POSITIVE_COLOR_STR + ">INCREASED</color>";

			// Token: 0x0400A790 RID: 42896
			public static LocString DECREASING_STR = "<color=" + Constants.NEGATIVE_COLOR_STR + ">DECREASED</color>";

			// Token: 0x0400A791 RID: 42897
			public static LocString CLEAR_NEW_RESOURCES = "Clear New";

			// Token: 0x0400A792 RID: 42898
			public static LocString CLEAR_ALL = "Unpin all resources";

			// Token: 0x0400A793 RID: 42899
			public static LocString SEE_ALL = "+ See All ({0})";

			// Token: 0x0400A794 RID: 42900
			public static LocString NEW_TAG = "NEW";
		}

		// Token: 0x02002545 RID: 9541
		public class CONFIRMDIALOG
		{
			// Token: 0x0400A795 RID: 42901
			public static LocString OK = "OK";

			// Token: 0x0400A796 RID: 42902
			public static LocString CANCEL = "CANCEL";

			// Token: 0x0400A797 RID: 42903
			public static LocString DIALOG_HEADER = "MESSAGE";
		}

		// Token: 0x02002546 RID: 9542
		public class FACADE_SELECTION_PANEL
		{
			// Token: 0x0400A798 RID: 42904
			public static LocString HEADER = "Select Blueprint";

			// Token: 0x0400A799 RID: 42905
			public static LocString STORE_BUTTON_TOOLTIP = "See more Blueprints in the Supply Closet";
		}

		// Token: 0x02002547 RID: 9543
		public class FILE_NAME_DIALOG
		{
			// Token: 0x0400A79A RID: 42906
			public static LocString ENTER_TEXT = "Enter Text...";
		}

		// Token: 0x02002548 RID: 9544
		public class MINION_IDENTITY_SORT
		{
			// Token: 0x0400A79B RID: 42907
			public static LocString TITLE = "Sort By";

			// Token: 0x0400A79C RID: 42908
			public static LocString NAME = "Duplicant";

			// Token: 0x0400A79D RID: 42909
			public static LocString ROLE = "Role";

			// Token: 0x0400A79E RID: 42910
			public static LocString PERMISSION = "Permission";
		}

		// Token: 0x02002549 RID: 9545
		public class UISIDESCREENS
		{
			// Token: 0x02002F7C RID: 12156
			public class TABS
			{
				// Token: 0x0400CCDA RID: 52442
				public static LocString HEADER = "Options";

				// Token: 0x0400CCDB RID: 52443
				public static LocString CONFIGURATION = "Config";

				// Token: 0x0400CCDC RID: 52444
				public static LocString MATERIAL = "Material";

				// Token: 0x0400CCDD RID: 52445
				public static LocString SKIN = "Blueprint";
			}

			// Token: 0x02002F7D RID: 12157
			public class BLUEPRINT_TAB
			{
				// Token: 0x0400CCDE RID: 52446
				public static LocString EDIT_OUTFIT_BUTTON = "Restyle";

				// Token: 0x0400CCDF RID: 52447
				public static LocString SUBCATEGORY_OUTFIT = "Clothing";

				// Token: 0x0400CCE0 RID: 52448
				public static LocString SUBCATEGORY_ATMOSUIT = "Atmo Suit";

				// Token: 0x0400CCE1 RID: 52449
				public static LocString SUBCATEGORY_JOYRESPONSE = "Overjoyed";

				// Token: 0x0400CCE2 RID: 52450
				public static LocString SUBCATEGORY_JETSUIT = "Jet Suit";
			}

			// Token: 0x02002F7E RID: 12158
			public class NOCONFIG
			{
				// Token: 0x0400CCE3 RID: 52451
				public static LocString TITLE = "No configuration";

				// Token: 0x0400CCE4 RID: 52452
				public static LocString LABEL = "There is no configuration available for this object.";
			}

			// Token: 0x02002F7F RID: 12159
			public class ARTABLESELECTIONSIDESCREEN
			{
				// Token: 0x0400CCE5 RID: 52453
				public static LocString TITLE = "Style Selection";

				// Token: 0x0400CCE6 RID: 52454
				public static LocString BUTTON = "Redecorate";

				// Token: 0x0400CCE7 RID: 52455
				public static LocString BUTTON_TOOLTIP = "Clears current artwork\n\nCreates errand for a skilled Duplicant to create selected style";

				// Token: 0x0400CCE8 RID: 52456
				public static LocString CLEAR_BUTTON_TOOLTIP = "Clears current artwork\n\nAllows a skilled Duplicant to create artwork of their choice";
			}

			// Token: 0x02002F80 RID: 12160
			public class ARTIFACTANALYSISSIDESCREEN
			{
				// Token: 0x0400CCE9 RID: 52457
				public static LocString NO_ARTIFACTS_DISCOVERED = "No artifacts analyzed";

				// Token: 0x0400CCEA RID: 52458
				public static LocString NO_ARTIFACTS_DISCOVERED_TOOLTIP = "Analyzing artifacts requires a Duplicant with the Masterworks skill";
			}

			// Token: 0x02002F81 RID: 12161
			public class BUTTONMENUSIDESCREEN
			{
				// Token: 0x0400CCEB RID: 52459
				public static LocString TITLE = "Building Menu";

				// Token: 0x0400CCEC RID: 52460
				public static LocString ALLOW_INTERNAL_CONSTRUCTOR = "Enable Auto-Delivery";

				// Token: 0x0400CCED RID: 52461
				public static LocString ALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = "Order Duplicants to deliver {0}" + UI.FormatAsLink("s", "NONE") + " to this building automatically when they need replacing";

				// Token: 0x0400CCEE RID: 52462
				public static LocString DISALLOW_INTERNAL_CONSTRUCTOR = "Cancel Auto-Delivery";

				// Token: 0x0400CCEF RID: 52463
				public static LocString DISALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = "Cancel automatic {0} deliveries to this building";
			}

			// Token: 0x02002F82 RID: 12162
			public class LOREBEARERSIDESCREEN
			{
				// Token: 0x0400CCF0 RID: 52464
				public static LocString TITLE = "Read Files";
			}

			// Token: 0x02002F83 RID: 12163
			public class CONFIGURECONSUMERSIDESCREEN
			{
				// Token: 0x0400CCF1 RID: 52465
				public static LocString TITLE = "Configure Building";

				// Token: 0x0400CCF2 RID: 52466
				public static LocString SELECTION_DESCRIPTION_HEADER = "Description";
			}

			// Token: 0x02002F84 RID: 12164
			public class TREEFILTERABLESIDESCREEN
			{
				// Token: 0x0400CCF3 RID: 52467
				public static LocString TITLE = "Element Filter";

				// Token: 0x0400CCF4 RID: 52468
				public static LocString TITLE_CRITTER = "Critter Filter";

				// Token: 0x0400CCF5 RID: 52469
				public static LocString ALLBUTTON = "All Standard";

				// Token: 0x0400CCF6 RID: 52470
				public static LocString ALLBUTTONTOOLTIP = "Allow storage of all standard resources preferred by this building\n\nNon-standard resources must be selected manually\n\nNon-standard resources include:\n    • Clothing\n    • Critter Eggs\n    • Sublimators";

				// Token: 0x0400CCF7 RID: 52471
				public static LocString ALLBUTTON_EDIBLES = "All Edibles";

				// Token: 0x0400CCF8 RID: 52472
				public static LocString ALLBUTTON_EDIBLES_TOOLTIP = "Allow storage of all edible resources";

				// Token: 0x0400CCF9 RID: 52473
				public static LocString ALLBUTTON_CRITTERS = "All Critters";

				// Token: 0x0400CCFA RID: 52474
				public static LocString ALLBUTTON_CRITTERS_TOOLTIP = "Allow storage of all eligible " + UI.PRE_KEYWORD + "Critters" + UI.PST_KEYWORD;

				// Token: 0x0400CCFB RID: 52475
				public static LocString SPECIAL_RESOURCES = "Non-Standard";

				// Token: 0x0400CCFC RID: 52476
				public static LocString SPECIAL_RESOURCES_TOOLTIP = "These objects may not be ideally suited to storage";

				// Token: 0x0400CCFD RID: 52477
				public static LocString CATEGORYBUTTONTOOLTIP = "Allow storage of anything in the {0} resource category";

				// Token: 0x0400CCFE RID: 52478
				public static LocString MATERIALBUTTONTOOLTIP = "Add or remove this material from storage";

				// Token: 0x0400CCFF RID: 52479
				public static LocString ONLYALLOWTRANSPORTITEMSBUTTON = "Sweep Only";

				// Token: 0x0400CD00 RID: 52480
				public static LocString ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP = "Only store objects marked Sweep <color=#F44A47><b>[K]</b></color> in this container";

				// Token: 0x0400CD01 RID: 52481
				public static LocString ONLYALLOWSPICEDITEMSBUTTON = "Seasoned Food Only";

				// Token: 0x0400CD02 RID: 52482
				public static LocString ONLYALLOWSPICEDITEMSBUTTONTOOLTIP = "Only store foods that have been seasoned at the " + UI.PRE_KEYWORD + "Spice Grinder" + UI.PST_KEYWORD;

				// Token: 0x0400CD03 RID: 52483
				public static LocString SEARCH_PLACEHOLDER = "Search";
			}

			// Token: 0x02002F85 RID: 12165
			public class TELESCOPESIDESCREEN
			{
				// Token: 0x0400CD04 RID: 52484
				public static LocString TITLE = "Telescope Configuration";

				// Token: 0x0400CD05 RID: 52485
				public static LocString NO_SELECTED_ANALYSIS_TARGET = "No analysis focus selected\nOpen the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to select a focus";

				// Token: 0x0400CD06 RID: 52486
				public static LocString ANALYSIS_TARGET_SELECTED = "Object focus selected\nAnalysis underway";

				// Token: 0x0400CD07 RID: 52487
				public static LocString OPENSTARMAPBUTTON = "OPEN STARMAP";

				// Token: 0x0400CD08 RID: 52488
				public static LocString ANALYSIS_TARGET_HEADER = "Object Analysis";
			}

			// Token: 0x02002F86 RID: 12166
			public class CLUSTERTELESCOPESIDESCREEN
			{
				// Token: 0x0400CD09 RID: 52489
				public static LocString TITLE = "Telescope Configuration";

				// Token: 0x0400CD0A RID: 52490
				public static LocString CHECKBOX_METEORS = "Allow meteor shower identification";

				// Token: 0x0400CD0B RID: 52491
				public static LocString CHECKBOX_TOOLTIP_METEORS = string.Concat(new string[]
				{
					"Prioritizes unidentified meteors that come within range in a previously revealed location\n\nWill interrupt a Duplicant working on revealing a new ",
					UI.PRE_KEYWORD,
					"Starmap",
					UI.PST_KEYWORD,
					" location"
				});
			}

			// Token: 0x02002F87 RID: 12167
			public class TEMPORALTEARSIDESCREEN
			{
				// Token: 0x0400CD0C RID: 52492
				public static LocString TITLE = "Temporal Tear";

				// Token: 0x0400CD0D RID: 52493
				public static LocString BUTTON_OPEN = "Enter Tear";

				// Token: 0x0400CD0E RID: 52494
				public static LocString BUTTON_CLOSED = "Tear Closed";

				// Token: 0x0400CD0F RID: 52495
				public static LocString BUTTON_LABEL = "Enter Temporal Tear";

				// Token: 0x0400CD10 RID: 52496
				public static LocString CONFIRM_POPUP_MESSAGE = "Are you sure you want to fire this?";

				// Token: 0x0400CD11 RID: 52497
				public static LocString CONFIRM_POPUP_CONFIRM = "Yes, I'm ready for a meteor shower.";

				// Token: 0x0400CD12 RID: 52498
				public static LocString CONFIRM_POPUP_CANCEL = "No, I need more time to prepare.";

				// Token: 0x0400CD13 RID: 52499
				public static LocString CONFIRM_POPUP_TITLE = "Temporal Tear Opener";
			}

			// Token: 0x02002F88 RID: 12168
			public class RAILGUNSIDESCREEN
			{
				// Token: 0x0400CD14 RID: 52500
				public static LocString TITLE = "Launcher Configuration";

				// Token: 0x0400CD15 RID: 52501
				public static LocString NO_SELECTED_LAUNCH_TARGET = "No destination selected\nOpen the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to set a course";

				// Token: 0x0400CD16 RID: 52502
				public static LocString LAUNCH_TARGET_SELECTED = "Launcher destination {0} set";

				// Token: 0x0400CD17 RID: 52503
				public static LocString OPENSTARMAPBUTTON = "OPEN STARMAP";

				// Token: 0x0400CD18 RID: 52504
				public static LocString LAUNCH_RESOURCES_HEADER = "Launch Resources:";

				// Token: 0x0400CD19 RID: 52505
				public static LocString MINIMUM_PAYLOAD_MASS = "Minimum launch mass:";
			}

			// Token: 0x02002F89 RID: 12169
			public class CLUSTERWORLDSIDESCREEN
			{
				// Token: 0x0400CD1A RID: 52506
				public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400CD1B RID: 52507
				public static LocString VIEW_WORLD = "Oversee " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400CD1C RID: 52508
				public static LocString VIEW_WORLD_DISABLE_TOOLTIP = "Cannot view " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400CD1D RID: 52509
				public static LocString VIEW_WORLD_TOOLTIP = "View this " + UI.CLUSTERMAP.PLANETOID + "'s surface";
			}

			// Token: 0x02002F8A RID: 12170
			public class ROCKETVIEWINTERIORSECTION
			{
				// Token: 0x02003C5B RID: 15451
				public class BUTTONVIEWINTERIOR
				{
					// Token: 0x0400EFD1 RID: 61393
					public static LocString TITLE = "Rocket Interior";

					// Token: 0x0400EFD2 RID: 61394
					public static LocString LABEL = "View Interior";

					// Token: 0x0400EFD3 RID: 61395
					public static LocString DESC = "What's goin' on in there?";

					// Token: 0x0400EFD4 RID: 61396
					public static LocString INVALID = string.Concat(new string[]
					{
						"This rocket does not have a ",
						UI.PRE_KEYWORD,
						"Spacefarer",
						UI.PST_KEYWORD,
						" module"
					});
				}

				// Token: 0x02003C5C RID: 15452
				public class BUTTONVIEWEXTERIOR
				{
					// Token: 0x0400EFD5 RID: 61397
					public static LocString LABEL = "View Exterior";

					// Token: 0x0400EFD6 RID: 61398
					public static LocString DESC = "Switch to external world view";

					// Token: 0x0400EFD7 RID: 61399
					public static LocString INVALID = "Not available while rocket is in flight";
				}
			}

			// Token: 0x02002F8B RID: 12171
			public class ROCKETMODULESIDESCREEN
			{
				// Token: 0x0400CD1E RID: 52510
				public static LocString TITLE = "Module";

				// Token: 0x0400CD1F RID: 52511
				public static LocString CHANGEMODULEPANEL = "Add or Change Module";

				// Token: 0x0400CD20 RID: 52512
				public static LocString ENGINE_MAX_HEIGHT = "This engine allows a <b>Maximum Rocket Height</b> of {0}";

				// Token: 0x02003C5D RID: 15453
				public class MODULESTATCHANGE
				{
					// Token: 0x0400EFD8 RID: 61400
					public static LocString TITLE = "Rocket stats on construction:";

					// Token: 0x0400EFD9 RID: 61401
					public static LocString BURDEN = "    • " + DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME + ": {0} ({1})";

					// Token: 0x0400EFDA RID: 61402
					public static LocString RANGE = string.Concat(new string[]
					{
						"    • Potential ",
						DUPLICANTS.ATTRIBUTES.FUELRANGEPERKILOGRAM.NAME,
						": {0}/1",
						UI.UNITSUFFIXES.MASS.KILOGRAM,
						" Fuel ({1})"
					});

					// Token: 0x0400EFDB RID: 61403
					public static LocString SPEED = "    • Speed: {0} ({1})";

					// Token: 0x0400EFDC RID: 61404
					public static LocString ENGINEPOWER = "    • " + DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME + ": {0} ({1})";

					// Token: 0x0400EFDD RID: 61405
					public static LocString HEIGHT = "    • " + DUPLICANTS.ATTRIBUTES.HEIGHT.NAME + ": {0}/{2} ({1})";

					// Token: 0x0400EFDE RID: 61406
					public static LocString HEIGHT_NOMAX = "    • " + DUPLICANTS.ATTRIBUTES.HEIGHT.NAME + ": {0} ({1})";

					// Token: 0x0400EFDF RID: 61407
					public static LocString POSITIVEDELTA = UI.FormatAsPositiveModifier("{0}");

					// Token: 0x0400EFE0 RID: 61408
					public static LocString NEGATIVEDELTA = UI.FormatAsNegativeModifier("{0}");
				}

				// Token: 0x02003C5E RID: 15454
				public class BUTTONSWAPMODULEUP
				{
					// Token: 0x0400EFE1 RID: 61409
					public static LocString DESC = "Swap this rocket module with the one above";

					// Token: 0x0400EFE2 RID: 61410
					public static LocString INVALID = "No module above may be swapped.\n\n    • A module above may be unable to have modules placed above it.\n    • A module above may be unable to fit into the space below it.\n    • This module may be unable to fit into the space above it.";
				}

				// Token: 0x02003C5F RID: 15455
				public class BUTTONSWAPMODULEDOWN
				{
					// Token: 0x0400EFE3 RID: 61411
					public static LocString DESC = "Swap this rocket module with the one below";

					// Token: 0x0400EFE4 RID: 61412
					public static LocString INVALID = "No module below may be swapped.\n\n    • A module below may be unable to have modules placed below it.\n    • A module below may be unable to fit into the space above it.\n    • This module may be unable to fit into the space below it.";
				}

				// Token: 0x02003C60 RID: 15456
				public class BUTTONCHANGEMODULE
				{
					// Token: 0x0400EFE5 RID: 61413
					public static LocString LABEL = "Change Module";

					// Token: 0x0400EFE6 RID: 61414
					public static LocString DESC = "Swap this module for a different module";

					// Token: 0x0400EFE7 RID: 61415
					public static LocString INVALID = "This module cannot be changed to a different type";
				}

				// Token: 0x02003C61 RID: 15457
				public class BUTTONREMOVEMODULE
				{
					// Token: 0x0400EFE8 RID: 61416
					public static LocString LABEL = "Deconstruct";

					// Token: 0x0400EFE9 RID: 61417
					public static LocString LABEL_CANCEL = "Cancel Deconstruct";

					// Token: 0x0400EFEA RID: 61418
					public static LocString DESC = "Remove this module";

					// Token: 0x0400EFEB RID: 61419
					public static LocString DESC_CANCEL = "Cancel the order for deconstructing this module";

					// Token: 0x0400EFEC RID: 61420
					public static LocString INVALID = "This module cannot be removed";
				}

				// Token: 0x02003C62 RID: 15458
				public class ADDMODULE
				{
					// Token: 0x0400EFED RID: 61421
					public static LocString LABEL = "Add Module";

					// Token: 0x0400EFEE RID: 61422
					public static LocString DESC = "Add a new module above this one";

					// Token: 0x0400EFEF RID: 61423
					public static LocString INVALID = "Modules cannot be added above this module, or there is no room above to add a module";
				}
			}

			// Token: 0x02002F8C RID: 12172
			public class CLUSTERLOCATIONFILTERSIDESCREEN
			{
				// Token: 0x0400CD21 RID: 52513
				public static LocString TITLE = "Location Filter";

				// Token: 0x0400CD22 RID: 52514
				public static LocString HEADER = "Send Green signal at locations";

				// Token: 0x0400CD23 RID: 52515
				public static LocString EMPTY_SPACE_ROW = "In Space";
			}

			// Token: 0x02002F8D RID: 12173
			public class DISPENSERSIDESCREEN
			{
				// Token: 0x0400CD24 RID: 52516
				public static LocString TITLE = "Dispenser";

				// Token: 0x0400CD25 RID: 52517
				public static LocString BUTTON_CANCEL = "Cancel order";

				// Token: 0x0400CD26 RID: 52518
				public static LocString BUTTON_DISPENSE = "Dispense item";
			}

			// Token: 0x02002F8E RID: 12174
			public class ROCKETRESTRICTIONSIDESCREEN
			{
				// Token: 0x0400CD27 RID: 52519
				public static LocString TITLE = "Rocket Restrictions";

				// Token: 0x0400CD28 RID: 52520
				public static LocString BUILDING_RESTRICTIONS_LABEL = "Interior Building Restrictions";

				// Token: 0x0400CD29 RID: 52521
				public static LocString NONE_RESTRICTION_BUTTON = "None";

				// Token: 0x0400CD2A RID: 52522
				public static LocString NONE_RESTRICTION_BUTTON_TOOLTIP = "There are no restrictions on buildings inside this rocket";

				// Token: 0x0400CD2B RID: 52523
				public static LocString GROUNDED_RESTRICTION_BUTTON = "Grounded";

				// Token: 0x0400CD2C RID: 52524
				public static LocString GROUNDED_RESTRICTION_BUTTON_TOOLTIP = "Buildings with their access restricted cannot be operated while grounded, though they can still be filled";

				// Token: 0x0400CD2D RID: 52525
				public static LocString AUTOMATION = "Automation Controlled";

				// Token: 0x0400CD2E RID: 52526
				public static LocString AUTOMATION_TOOLTIP = "Building restrictions are managed by automation\n\nBuildings with their access restricted cannot be operated, though they can still be filled";
			}

			// Token: 0x02002F8F RID: 12175
			public class HABITATMODULESIDESCREEN
			{
				// Token: 0x0400CD2F RID: 52527
				public static LocString TITLE = "Spacefarer Module";

				// Token: 0x0400CD30 RID: 52528
				public static LocString VIEW_BUTTON = "View Interior";

				// Token: 0x0400CD31 RID: 52529
				public static LocString VIEW_BUTTON_TOOLTIP = "What's goin' on in there?";
			}

			// Token: 0x02002F90 RID: 12176
			public class HARVESTMODULESIDESCREEN
			{
				// Token: 0x0400CD32 RID: 52530
				public static LocString TITLE = "Mining Resources";

				// Token: 0x0400CD33 RID: 52531
				public static LocString MINING_IN_PROGRESS = "Drilling...";

				// Token: 0x0400CD34 RID: 52532
				public static LocString MINING_STOPPED = "Not drilling";

				// Token: 0x0400CD35 RID: 52533
				public static LocString ENABLE = "Enable Drill";

				// Token: 0x0400CD36 RID: 52534
				public static LocString DISABLE = "Disable Drill";
			}

			// Token: 0x02002F91 RID: 12177
			public class CARGOMODULESIDESCREEN
			{
				// Token: 0x0400CD37 RID: 52535
				public static LocString TITLE = "Harvesting Resources";

				// Token: 0x0400CD38 RID: 52536
				public static LocString GATHERING_IN_PROGRESS = "Gathering...";

				// Token: 0x0400CD39 RID: 52537
				public static LocString GATHERING_FULL = "Storage Full";

				// Token: 0x0400CD3A RID: 52538
				public static LocString GATHERING_STOPPED = "Not gathering";

				// Token: 0x0400CD3B RID: 52539
				public static LocString ENABLE = "Enable Gather";

				// Token: 0x0400CD3C RID: 52540
				public static LocString DISABLE = "Disable Gather";
			}

			// Token: 0x02002F92 RID: 12178
			public class SELECTMODULESIDESCREEN
			{
				// Token: 0x0400CD3D RID: 52541
				public static LocString TITLE = "Select Module";

				// Token: 0x0400CD3E RID: 52542
				public static LocString BUILDBUTTON = "Build";

				// Token: 0x02003C63 RID: 15459
				public class CONSTRAINTS
				{
					// Token: 0x020040DF RID: 16607
					public class RESEARCHED
					{
						// Token: 0x0400FABB RID: 64187
						public static LocString COMPLETE = "Research Completed";

						// Token: 0x0400FABC RID: 64188
						public static LocString FAILED = "Research Incomplete";
					}

					// Token: 0x020040E0 RID: 16608
					public class MATERIALS_AVAILABLE
					{
						// Token: 0x0400FABD RID: 64189
						public static LocString COMPLETE = "Materials available";

						// Token: 0x0400FABE RID: 64190
						public static LocString FAILED = "• Materials unavailable";
					}

					// Token: 0x020040E1 RID: 16609
					public class ONE_COMMAND_PER_ROCKET
					{
						// Token: 0x0400FABF RID: 64191
						public static LocString COMPLETE = "";

						// Token: 0x0400FAC0 RID: 64192
						public static LocString FAILED = "• Command module already installed";
					}

					// Token: 0x020040E2 RID: 16610
					public class ONE_ENGINE_PER_ROCKET
					{
						// Token: 0x0400FAC1 RID: 64193
						public static LocString COMPLETE = "";

						// Token: 0x0400FAC2 RID: 64194
						public static LocString FAILED = "• Engine module already installed";
					}

					// Token: 0x020040E3 RID: 16611
					public class ENGINE_AT_BOTTOM
					{
						// Token: 0x0400FAC3 RID: 64195
						public static LocString COMPLETE = "";

						// Token: 0x0400FAC4 RID: 64196
						public static LocString FAILED = "• Must install at bottom of rocket";
					}

					// Token: 0x020040E4 RID: 16612
					public class TOP_ONLY
					{
						// Token: 0x0400FAC5 RID: 64197
						public static LocString COMPLETE = "";

						// Token: 0x0400FAC6 RID: 64198
						public static LocString FAILED = "• Must install at top of rocket";
					}

					// Token: 0x020040E5 RID: 16613
					public class SPACE_AVAILABLE
					{
						// Token: 0x0400FAC7 RID: 64199
						public static LocString COMPLETE = "";

						// Token: 0x0400FAC8 RID: 64200
						public static LocString FAILED = "• Space above rocket blocked";
					}

					// Token: 0x020040E6 RID: 16614
					public class PASSENGER_MODULE_AVAILABLE
					{
						// Token: 0x0400FAC9 RID: 64201
						public static LocString COMPLETE = "";

						// Token: 0x0400FACA RID: 64202
						public static LocString FAILED = "• Max number of passenger modules installed";
					}

					// Token: 0x020040E7 RID: 16615
					public class MAX_MODULES
					{
						// Token: 0x0400FACB RID: 64203
						public static LocString COMPLETE = "";

						// Token: 0x0400FACC RID: 64204
						public static LocString FAILED = "• Max module limit of engine reached";
					}

					// Token: 0x020040E8 RID: 16616
					public class MAX_HEIGHT
					{
						// Token: 0x0400FACD RID: 64205
						public static LocString COMPLETE = "";

						// Token: 0x0400FACE RID: 64206
						public static LocString FAILED = "• Engine's height limit reached or exceeded";

						// Token: 0x0400FACF RID: 64207
						public static LocString FAILED_NO_ENGINE = "• Rocket requires space for an engine";
					}

					// Token: 0x020040E9 RID: 16617
					public class ONE_ROBOPILOT_PER_ROCKET
					{
						// Token: 0x0400FAD0 RID: 64208
						public static LocString COMPLETE = "";

						// Token: 0x0400FAD1 RID: 64209
						public static LocString FAILED = "• Robo-Pilot module already installed";
					}
				}
			}

			// Token: 0x02002F93 RID: 12179
			public class FILTERSIDESCREEN
			{
				// Token: 0x0400CD3F RID: 52543
				public static LocString TITLE = "Filter Outputs";

				// Token: 0x0400CD40 RID: 52544
				public static LocString NO_SELECTION = "None";

				// Token: 0x0400CD41 RID: 52545
				public static LocString OUTPUTELEMENTHEADER = "Output 1";

				// Token: 0x0400CD42 RID: 52546
				public static LocString SELECTELEMENTHEADER = "Output 2";

				// Token: 0x0400CD43 RID: 52547
				public static LocString OUTPUTRED = "Output Red";

				// Token: 0x0400CD44 RID: 52548
				public static LocString OUTPUTGREEN = "Output Green";

				// Token: 0x0400CD45 RID: 52549
				public static LocString NOELEMENTSELECTED = "No element selected";

				// Token: 0x0400CD46 RID: 52550
				public static LocString DRIEDFOOD = "Dried Food";

				// Token: 0x02003C64 RID: 15460
				public static class UNFILTEREDELEMENTS
				{
					// Token: 0x0400EFF0 RID: 61424
					public static LocString GAS = "Gas Output:\nAll";

					// Token: 0x0400EFF1 RID: 61425
					public static LocString LIQUID = "Liquid Output:\nAll";

					// Token: 0x0400EFF2 RID: 61426
					public static LocString SOLID = "Solid Output:\nAll";
				}

				// Token: 0x02003C65 RID: 15461
				public static class FILTEREDELEMENT
				{
					// Token: 0x0400EFF3 RID: 61427
					public static LocString GAS = "Filtered Gas Output:\n{0}";

					// Token: 0x0400EFF4 RID: 61428
					public static LocString LIQUID = "Filtered Liquid Output:\n{0}";

					// Token: 0x0400EFF5 RID: 61429
					public static LocString SOLID = "Filtered Solid Output:\n{0}";
				}
			}

			// Token: 0x02002F94 RID: 12180
			public class SINGLEITEMSELECTIONSIDESCREEN
			{
				// Token: 0x0400CD47 RID: 52551
				public static LocString TITLE = "Element Filter";

				// Token: 0x0400CD48 RID: 52552
				public static LocString LIST_TITLE = "Options";

				// Token: 0x0400CD49 RID: 52553
				public static LocString NO_SELECTION = "None";

				// Token: 0x02003C66 RID: 15462
				public class CURRENT_ITEM_SELECTED_SECTION
				{
					// Token: 0x0400EFF6 RID: 61430
					public static LocString TITLE = "Current Selection";

					// Token: 0x0400EFF7 RID: 61431
					public static LocString NO_ITEM_TITLE = "No Item Selected";

					// Token: 0x0400EFF8 RID: 61432
					public static LocString NO_ITEM_MESSAGE = "Select an item for storage below.";
				}
			}

			// Token: 0x02002F95 RID: 12181
			public class FEWOPTIONSELECTIONSIDESCREEN
			{
				// Token: 0x0400CD4A RID: 52554
				public static LocString TITLE = "Options";
			}

			// Token: 0x02002F96 RID: 12182
			public class MISSILESELECTIONSIDESCREEN
			{
				// Token: 0x0400CD4B RID: 52555
				public static LocString TITLE = BUILDINGS.PREFABS.MISSILELAUNCHER.NAME;

				// Token: 0x0400CD4C RID: 52556
				public static LocString HEADER = "Projectile Selection";

				// Token: 0x02003C67 RID: 15463
				public class VANILLALARGEIMPACTOR
				{
					// Token: 0x0400EFF9 RID: 61433
					public static LocString HEALTH_BAR_TITLE = "Health";

					// Token: 0x0400EFFA RID: 61434
					public static LocString HEALTH_BAR_TOOLTIP = "Demolior health: {0} / {1}";

					// Token: 0x0400EFFB RID: 61435
					public static LocString TIME_UNTIL_COLLISION_TITLE = "Time Until Impact";

					// Token: 0x0400EFFC RID: 61436
					public static LocString TIME_UNTIL_COLLISION_TOOLTIP = "{0} cycles remaining until impact";
				}
			}

			// Token: 0x02002F97 RID: 12183
			public class LOGICBROADCASTCHANNELSIDESCREEN
			{
				// Token: 0x0400CD4D RID: 52557
				public static LocString TITLE = "Channel Selector";

				// Token: 0x0400CD4E RID: 52558
				public static LocString HEADER = "Channel Selector";

				// Token: 0x0400CD4F RID: 52559
				public static LocString IN_RANGE = "In Range";

				// Token: 0x0400CD50 RID: 52560
				public static LocString OUT_OF_RANGE = "Out of Range";

				// Token: 0x0400CD51 RID: 52561
				public static LocString NO_SENDERS = "No Channels Available";

				// Token: 0x0400CD52 RID: 52562
				public static LocString NO_SENDERS_DESC = "Build a " + BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.NAME + " to transmit a signal.";
			}

			// Token: 0x02002F98 RID: 12184
			public class CONDITIONLISTSIDESCREEN
			{
				// Token: 0x0400CD53 RID: 52563
				public static LocString TITLE = "Condition List";
			}

			// Token: 0x02002F99 RID: 12185
			public class FABRICATORSIDESCREEN
			{
				// Token: 0x0400CD54 RID: 52564
				public static LocString TITLE = "Production Orders";

				// Token: 0x0400CD55 RID: 52565
				public static LocString SUBTITLE = "Recipes";

				// Token: 0x0400CD56 RID: 52566
				public static LocString NORECIPEDISCOVERED = "No discovered recipes";

				// Token: 0x0400CD57 RID: 52567
				public static LocString NORECIPEDISCOVERED_BODY = "Discover new ingredients or research new technology to unlock some recipes.";

				// Token: 0x0400CD58 RID: 52568
				public static LocString UNDISCOVERED_RECIPES = "The following recipes are not yet discovered.\nI must discover new ingredients or research new technology to unlock them:";

				// Token: 0x0400CD59 RID: 52569
				public static LocString NORECIPESELECTED = "No recipe selected";

				// Token: 0x0400CD5A RID: 52570
				public static LocString SELECTRECIPE = "Select a recipe to fabricate.";

				// Token: 0x0400CD5B RID: 52571
				public static LocString COST = "<b>Ingredients:</b>\n";

				// Token: 0x0400CD5C RID: 52572
				public static LocString RESULTREQUIREMENTS = "<b>Requirements:</b>";

				// Token: 0x0400CD5D RID: 52573
				public static LocString RESULTEFFECTS = "<b>Effects:</b>";

				// Token: 0x0400CD5E RID: 52574
				public static LocString KG = "- {0}: {1}\n";

				// Token: 0x0400CD5F RID: 52575
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400CD60 RID: 52576
				public static LocString CANCEL = "Cancel";

				// Token: 0x0400CD61 RID: 52577
				public static LocString RECIPE_REQUIREMENT = "{0}: {1}";

				// Token: 0x0400CD62 RID: 52578
				public static LocString RECIPE_AVAILABLE = "Available: {0}";

				// Token: 0x0400CD63 RID: 52579
				public static LocString RECIPEPRODUCT = "{0}: {1}";

				// Token: 0x0400CD64 RID: 52580
				public static LocString UNITS_AND_CALS = "{0} [{1}]";

				// Token: 0x0400CD65 RID: 52581
				public static LocString CALS = "{0}";

				// Token: 0x0400CD66 RID: 52582
				public static LocString QUEUED_MISSING_INGREDIENTS_TOOLTIP = "Missing {0} of {1}\n";

				// Token: 0x0400CD67 RID: 52583
				public static LocString CURRENT_ORDER = "Current order: {0}";

				// Token: 0x0400CD68 RID: 52584
				public static LocString NEXT_ORDER = "Next order: {0}";

				// Token: 0x0400CD69 RID: 52585
				public static LocString NO_WORKABLE_ORDER = "No workable order";

				// Token: 0x0400CD6A RID: 52586
				public static LocString RECIPE_DETAILS = "Recipe Details";

				// Token: 0x0400CD6B RID: 52587
				public static LocString RECIPE_QUEUE = "Order Production Quantity: ";

				// Token: 0x0400CD6C RID: 52588
				public static LocString RECIPE_QUEUE_CLICK_DESCRIPTION = "<b>" + UI.CLICK(UI.ClickType.Click) + " to select the next queued variant of this recipe</b>";

				// Token: 0x0400CD6D RID: 52589
				public static LocString RECIPE_FOREVER = "Forever";

				// Token: 0x0400CD6E RID: 52590
				public static LocString RECIPE_NONE = "No Orders Queued";

				// Token: 0x0400CD6F RID: 52591
				public static LocString CHANGE_RECIPE_ARROW_LABEL = "Change recipe";

				// Token: 0x0400CD70 RID: 52592
				public static LocString RECIPE_RESEARCH_REQUIRED = "Research Required";

				// Token: 0x0400CD71 RID: 52593
				public static LocString RECIPE_UNDISCOVERED_INGREDIENTS = "Undiscovered ingredients";

				// Token: 0x0400CD72 RID: 52594
				public static LocString INGREDIENT_CATEGORY = "Ingredient #{0}";

				// Token: 0x0400CD73 RID: 52595
				public static LocString ADDITIONAL_REQUIREMENTS = "Additional Requirements";

				// Token: 0x0400CD74 RID: 52596
				public static LocString ADDITIONAL_REQUIREMENTS_TOOLTIP = string.Concat(new string[]
				{
					"This recipe requires ",
					ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME,
					" to be collected by the building's input port\n\nOpen the ",
					UI.FormatAsBuildMenuTab("Automation Overlay", global::Action.Overlay13),
					" to view building ports"
				});

				// Token: 0x0400CD75 RID: 52597
				public static LocString NO_DISCOVERED_INGREDIENTS = "No ingredients discovered";

				// Token: 0x0400CD76 RID: 52598
				public static LocString UNDISCOVERED_INGREDIENTS_IN_CATEGORY = "Some ingredient options have not been discovered yet:\n\n{0}";

				// Token: 0x0400CD77 RID: 52599
				public static LocString ALL_INGREDIENTS_IN_CATEGORY_DISOVERED = "All ingredient options in this category have been discovered.";

				// Token: 0x0400CD78 RID: 52600
				public static LocString INGREDIENTS = "<b>Ingredients:</b>";

				// Token: 0x0400CD79 RID: 52601
				public static LocString RECIPE_EFFECTS = "<b>Effects:</b>";

				// Token: 0x0400CD7A RID: 52602
				public static LocString RECIPE_EFFECTS_HEADER = "Effects";

				// Token: 0x0400CD7B RID: 52603
				public static LocString ALLOW_MUTANT_SEED_INGREDIENTS = "Building accepts mutant seeds";

				// Token: 0x0400CD7C RID: 52604
				public static LocString ALLOW_MUTANT_SEED_INGREDIENTS_TOOLTIP = "Toggle whether Duplicants will deliver mutant seed species to this building as recipe ingredients";

				// Token: 0x0400CD7D RID: 52605
				public static LocString RECIPE_RADBOLTS_REQUIRED = ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME + ": {0}";

				// Token: 0x02003C68 RID: 15464
				public class TOOLTIPS
				{
					// Token: 0x0400EFFD RID: 61437
					public static LocString RECIPE_WORKTIME = "This recipe takes {0} to complete";

					// Token: 0x0400EFFE RID: 61438
					public static LocString RECIPERQUIREMENT_SUFFICIENT = "This recipe consumes {1} of an available {2} of {0}";

					// Token: 0x0400EFFF RID: 61439
					public static LocString RECIPERQUIREMENT_INSUFFICIENT = "This recipe requires {1} {0}\nAvailable: {2}";

					// Token: 0x0400F000 RID: 61440
					public static LocString RECIPEPRODUCT = "This recipe produces {1} {0}";

					// Token: 0x0400F001 RID: 61441
					public static LocString ADDITIONAL_INGREDIENT_OPTIONS_MESSAGE = UIConstants.ColorPrefixYellow + "Alternative ingredient options are available." + UIConstants.ColorSuffix;
				}

				// Token: 0x02003C69 RID: 15465
				public class EFFECTS
				{
					// Token: 0x0400F002 RID: 61442
					public static LocString OXYGEN_TANK = STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK.NAME + " ({0})";

					// Token: 0x0400F003 RID: 61443
					public static LocString OXYGEN_TANK_UNDERWATER = STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK_UNDERWATER.NAME + " ({0})";

					// Token: 0x0400F004 RID: 61444
					public static LocString JETSUIT_TANK = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.TANK_EFFECT_NAME + " ({0})";

					// Token: 0x0400F005 RID: 61445
					public static LocString LEADSUIT_BATTERY = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.BATTERY_EFFECT_NAME + " ({0})";

					// Token: 0x0400F006 RID: 61446
					public static LocString COOL_VEST = STRINGS.EQUIPMENT.PREFABS.COOL_VEST.NAME + " ({0})";

					// Token: 0x0400F007 RID: 61447
					public static LocString WARM_VEST = STRINGS.EQUIPMENT.PREFABS.WARM_VEST.NAME + " ({0})";

					// Token: 0x0400F008 RID: 61448
					public static LocString FUNKY_VEST = STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.NAME + " ({0})";

					// Token: 0x0400F009 RID: 61449
					public static LocString RESEARCHPOINT = "{0}: +1";
				}

				// Token: 0x02003C6A RID: 15466
				public class RECIPE_CATEGORIES
				{
					// Token: 0x0400F00A RID: 61450
					public static LocString ATMO_SUIT_FACADES = "Atmo Suit Styles";

					// Token: 0x0400F00B RID: 61451
					public static LocString JET_SUIT_FACADES = "Jet Suit Styles";

					// Token: 0x0400F00C RID: 61452
					public static LocString LEAD_SUIT_FACADES = "Lead Suit Styles";

					// Token: 0x0400F00D RID: 61453
					public static LocString PRIMO_GARB_FACADES = "Primo Garb Styles";
				}
			}

			// Token: 0x02002F9A RID: 12186
			public class ASSIGNMENTGROUPCONTROLLER
			{
				// Token: 0x0400CD7E RID: 52606
				public static LocString TITLE = "Duplicant Assignment";

				// Token: 0x0400CD7F RID: 52607
				public static LocString PILOT = "Pilot";

				// Token: 0x0400CD80 RID: 52608
				public static LocString OFFWORLD = "Offworld";

				// Token: 0x02003C6B RID: 15467
				public class TOOLTIPS
				{
					// Token: 0x0400F00E RID: 61454
					public static LocString DIFFERENT_WORLD = "This Duplicant is on a different " + UI.CLUSTERMAP.PLANETOID;

					// Token: 0x0400F00F RID: 61455
					public static LocString ASSIGN = "<b>Add</b> this Duplicant to rocket crew";

					// Token: 0x0400F010 RID: 61456
					public static LocString UNASSIGN = "<b>Remove</b> this Duplicant from rocket crew";
				}
			}

			// Token: 0x02002F9B RID: 12187
			public class LAUNCHPADSIDESCREEN
			{
				// Token: 0x0400CD81 RID: 52609
				public static LocString TITLE = "Rocket Platform";

				// Token: 0x0400CD82 RID: 52610
				public static LocString WAITING_TO_LAND_PANEL = "Waiting to land";

				// Token: 0x0400CD83 RID: 52611
				public static LocString NO_ROCKETS_WAITING = "No rockets in orbit";

				// Token: 0x0400CD84 RID: 52612
				public static LocString IN_ORBIT_ABOVE_PANEL = "Rockets in orbit";

				// Token: 0x0400CD85 RID: 52613
				public static LocString NEW_ROCKET_BUTTON = "NEW ROCKET";

				// Token: 0x0400CD86 RID: 52614
				public static LocString LAND_BUTTON = "LAND HERE";

				// Token: 0x0400CD87 RID: 52615
				public static LocString CANCEL_LAND_BUTTON = "CANCEL";

				// Token: 0x0400CD88 RID: 52616
				public static LocString LAUNCH_BUTTON = "BEGIN LAUNCH SEQUENCE";

				// Token: 0x0400CD89 RID: 52617
				public static LocString LAUNCH_BUTTON_DEBUG = "BEGIN LAUNCH SEQUENCE (DEBUG ENABLED)";

				// Token: 0x0400CD8A RID: 52618
				public static LocString LAUNCH_BUTTON_TOOLTIP = "Blast off!";

				// Token: 0x0400CD8B RID: 52619
				public static LocString LAUNCH_BUTTON_NOT_READY_TOOLTIP = "This rocket is <b>not</b> ready to launch\n\n<b>Review the Launch Checklist in the status panel for more detail</b>";

				// Token: 0x0400CD8C RID: 52620
				public static LocString LAUNCH_WARNINGS_BUTTON = "ACKNOWLEDGE WARNINGS";

				// Token: 0x0400CD8D RID: 52621
				public static LocString LAUNCH_WARNINGS_BUTTON_TOOLTIP = "Some items in the Launch Checklist require attention\n\n<b>" + UI.CLICK(UI.ClickType.Click) + " to ignore warnings and proceed with launch</b>";

				// Token: 0x0400CD8E RID: 52622
				public static LocString LAUNCH_REQUESTED_BUTTON = "CANCEL LAUNCH";

				// Token: 0x0400CD8F RID: 52623
				public static LocString LAUNCH_REQUESTED_BUTTON_TOOLTIP = "This rocket will take off as soon as a Duplicant takes the controls\n\n<b>" + UI.CLICK(UI.ClickType.Click) + " to cancel launch</b>";

				// Token: 0x0400CD90 RID: 52624
				public static LocString LAUNCH_AUTOMATION_CONTROLLED = "AUTOMATION CONTROLLED";

				// Token: 0x0400CD91 RID: 52625
				public static LocString LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP = "This " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + "'s launch operation is controlled by automation signals";

				// Token: 0x02003C6C RID: 15468
				public class STATUS
				{
					// Token: 0x0400F011 RID: 61457
					public static LocString STILL_PREPPING = "Launch Checklist Incomplete";

					// Token: 0x0400F012 RID: 61458
					public static LocString READY_FOR_LAUNCH = "Ready to Launch";

					// Token: 0x0400F013 RID: 61459
					public static LocString LOADING_CREW = "Loading crew...";

					// Token: 0x0400F014 RID: 61460
					public static LocString UNLOADING_PASSENGERS = "Unloading non-crew...";

					// Token: 0x0400F015 RID: 61461
					public static LocString WAITING_FOR_PILOT = "Pilot requested at control station...";

					// Token: 0x0400F016 RID: 61462
					public static LocString COUNTING_DOWN = "5... 4... 3... 2... 1...";

					// Token: 0x0400F017 RID: 61463
					public static LocString TAKING_OFF = "Liftoff!!";
				}
			}

			// Token: 0x02002F9C RID: 12188
			public class AUTOPLUMBERSIDESCREEN
			{
				// Token: 0x0400CD92 RID: 52626
				public static LocString TITLE = "Automatic Building Configuration";

				// Token: 0x02003C6D RID: 15469
				public class BUTTONS
				{
					// Token: 0x020040EA RID: 16618
					public class POWER
					{
						// Token: 0x0400FAD2 RID: 64210
						public static LocString TOOLTIP = "Add Dev Generator and Electrical Wires";
					}

					// Token: 0x020040EB RID: 16619
					public class PIPES
					{
						// Token: 0x0400FAD3 RID: 64211
						public static LocString TOOLTIP = "Add Dev Pumps and Pipes";
					}

					// Token: 0x020040EC RID: 16620
					public class SOLIDS
					{
						// Token: 0x0400FAD4 RID: 64212
						public static LocString TOOLTIP = "Spawn solid resources for a relevant recipe or conversions";
					}

					// Token: 0x020040ED RID: 16621
					public class MINION
					{
						// Token: 0x0400FAD5 RID: 64213
						public static LocString TOOLTIP = "Spawn a Duplicant in front of the building";
					}

					// Token: 0x020040EE RID: 16622
					public class FACADE
					{
						// Token: 0x0400FAD6 RID: 64214
						public static LocString TOOLTIP = "Toggle the building blueprint";
					}
				}
			}

			// Token: 0x02002F9D RID: 12189
			public class SELFDESTRUCTSIDESCREEN
			{
				// Token: 0x0400CD93 RID: 52627
				public static LocString TITLE = "Self Destruct";

				// Token: 0x0400CD94 RID: 52628
				public static LocString MESSAGE_TEXT = "EMERGENCY PROCEDURES";

				// Token: 0x0400CD95 RID: 52629
				public static LocString BUTTON_TEXT = "ABANDON SHIP!";

				// Token: 0x0400CD96 RID: 52630
				public static LocString BUTTON_TEXT_CONFIRM = "CONFIRM ABANDON SHIP";

				// Token: 0x0400CD97 RID: 52631
				public static LocString BUTTON_TOOLTIP = "This rocket is equipped with an emergency escape system.\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";

				// Token: 0x0400CD98 RID: 52632
				public static LocString BUTTON_TOOLTIP_CONFIRM = "<b>This will eject any passengers and destroy the rocket.<b>\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";
			}

			// Token: 0x02002F9E RID: 12190
			public class GENESHUFFLERSIDESREEN
			{
				// Token: 0x0400CD99 RID: 52633
				public static LocString TITLE = "Neural Vacillator";

				// Token: 0x0400CD9A RID: 52634
				public static LocString COMPLETE = "Something feels different.";

				// Token: 0x0400CD9B RID: 52635
				public static LocString UNDERWAY = "Neural Vacillation in progress.";

				// Token: 0x0400CD9C RID: 52636
				public static LocString CONSUMED = "There are no charges left in this Vacillator.";

				// Token: 0x0400CD9D RID: 52637
				public static LocString CONSUMED_WAITING = "Recharge requested, awaiting delivery by Duplicant.";

				// Token: 0x0400CD9E RID: 52638
				public static LocString BUTTON = "Complete Neural Process";

				// Token: 0x0400CD9F RID: 52639
				public static LocString BUTTON_RECHARGE = "Recharge";

				// Token: 0x0400CDA0 RID: 52640
				public static LocString BUTTON_RECHARGE_CANCEL = "Cancel Recharge";
			}

			// Token: 0x02002F9F RID: 12191
			public class MINIONTODOSIDESCREEN
			{
				// Token: 0x0400CDA1 RID: 52641
				public static LocString NAME = "Errands";

				// Token: 0x0400CDA2 RID: 52642
				public static LocString TOOLTIP = "<b>Errands</b>\nView current and upcoming errands";

				// Token: 0x0400CDA3 RID: 52643
				public static LocString CURRENT_TITLE = "Current Errand";

				// Token: 0x0400CDA4 RID: 52644
				public static LocString LIST_TITLE = "Upcoming Errands";

				// Token: 0x0400CDA5 RID: 52645
				public static LocString CURRENT_SCHEDULE_BLOCK = "CURRENT SHIFT: {0}";

				// Token: 0x0400CDA6 RID: 52646
				public static LocString CHORE_TARGET = "{Target}";

				// Token: 0x0400CDA7 RID: 52647
				public static LocString CHORE_TARGET_AND_GROUP = "{Target} -- {Groups}";

				// Token: 0x0400CDA8 RID: 52648
				public static LocString SELF_LABEL = "Self";

				// Token: 0x0400CDA9 RID: 52649
				public static LocString TRUNCATED_CHORES = "{0} more";

				// Token: 0x0400CDAA RID: 52650
				public static LocString TOOLTIP_IDLE = string.Concat(new string[]
				{
					"{IdleDescription}\n\nDuplicants will only <b>{Errand}</b> when there is nothing else for them to do\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.IDLE,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400CDAB RID: 52651
				public static LocString TOOLTIP_NORMAL = string.Concat(new string[]
				{
					"{Description}\n\nErrand Type: {Groups}\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • {Name}'s {BestGroup} Priority: {PersonalPriorityValue} ({PersonalPriority})\n    • This {Building}'s Priority: {BuildingPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400CDAC RID: 52652
				public static LocString TOOLTIP_PERSONAL = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is a ",
					UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS,
					" errand and so will be performed before all Regular errands\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400CDAD RID: 52653
				public static LocString TOOLTIP_EMERGENCY = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is an ",
					UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY,
					" errand and so will be performed before all Regular and Personal errands\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY,
					" : {ClassPriority}\n    • This {Building}'s Priority: {BuildingPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400CDAE RID: 52654
				public static LocString TOOLTIP_COMPULSORY = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is a ",
					UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY,
					" action and so will occur immediately\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400CDAF RID: 52655
				public static LocString TOOLTIP_DESC_ACTIVE = "{Name}'s Current Errand: <b>{Errand}</b>";

				// Token: 0x0400CDB0 RID: 52656
				public static LocString TOOLTIP_DESC_INACTIVE = "{Name} could work on <b>{Errand}</b>, but it's not their top priority right now";

				// Token: 0x0400CDB1 RID: 52657
				public static LocString TOOLTIP_IDLEDESC_ACTIVE = "{Name} is currently <b>Idle</b>";

				// Token: 0x0400CDB2 RID: 52658
				public static LocString TOOLTIP_IDLEDESC_INACTIVE = "{Name} could become <b>Idle</b> when all other errands are canceled or completed";

				// Token: 0x0400CDB3 RID: 52659
				public static LocString TOOLTIP_NA = "--";

				// Token: 0x0400CDB4 RID: 52660
				public static LocString CHORE_GROUP_SEPARATOR = " or ";
			}

			// Token: 0x02002FA0 RID: 12192
			public class MODULEFLIGHTUTILITYSIDESCREEN
			{
				// Token: 0x0400CDB5 RID: 52661
				public static LocString TITLE = "Deployables";

				// Token: 0x0400CDB6 RID: 52662
				public static LocString SELECT_TARGET_BUTTON = "Select Target";

				// Token: 0x0400CDB7 RID: 52663
				public static LocString SELECT_TARGET_BUTTON_TOOLTIP = "Select this module's target on the starmap";

				// Token: 0x0400CDB8 RID: 52664
				public static LocString DEPLOY_BUTTON = "Deploy";

				// Token: 0x0400CDB9 RID: 52665
				public static LocString DEPLOY_BUTTON_TOOLTIP = "Send this module's contents to the surface of the currently orbited " + UI.CLUSTERMAP.PLANETOID_KEYWORD + "\n\nA specific deploy location may need to be chosen for certain modules";

				// Token: 0x0400CDBA RID: 52666
				public static LocString REPEAT_BUTTON_TOOLTIP = "Automatically deploy this module's contents when a destination orbit is reached";

				// Token: 0x0400CDBB RID: 52667
				public static LocString SELECT_DUPLICANT = "Select Duplicant";

				// Token: 0x0400CDBC RID: 52668
				public static LocString PILOT_FMT = "{0} - Pilot";

				// Token: 0x0400CDBD RID: 52669
				public static LocString FIRE_BUTTON = "Fire";

				// Token: 0x0400CDBE RID: 52670
				public static LocString FIRE_BUTTON_TOOLTIP = "Fire this module's contents at the selected target";

				// Token: 0x0400CDBF RID: 52671
				public static LocString CLEAR_TARGET_BUTTON_TOOLTIP = "Cancel target selection";
			}

			// Token: 0x02002FA1 RID: 12193
			public class HIGHENERGYPARTICLEDIRECTIONSIDESCREEN
			{
				// Token: 0x0400CDC0 RID: 52672
				public static LocString TITLE = "Emitting Particle Direction";

				// Token: 0x0400CDC1 RID: 52673
				public static LocString SELECTED_DIRECTION = "Selected direction: {0}";

				// Token: 0x0400CDC2 RID: 52674
				public static LocString DIRECTION_N = "N";

				// Token: 0x0400CDC3 RID: 52675
				public static LocString DIRECTION_NE = "NE";

				// Token: 0x0400CDC4 RID: 52676
				public static LocString DIRECTION_E = "E";

				// Token: 0x0400CDC5 RID: 52677
				public static LocString DIRECTION_SE = "SE";

				// Token: 0x0400CDC6 RID: 52678
				public static LocString DIRECTION_S = "S";

				// Token: 0x0400CDC7 RID: 52679
				public static LocString DIRECTION_SW = "SW";

				// Token: 0x0400CDC8 RID: 52680
				public static LocString DIRECTION_W = "W";

				// Token: 0x0400CDC9 RID: 52681
				public static LocString DIRECTION_NW = "NW";
			}

			// Token: 0x02002FA2 RID: 12194
			public class MONUMENTSIDESCREEN
			{
				// Token: 0x0400CDCA RID: 52682
				public static LocString TITLE = "Great Monument";

				// Token: 0x0400CDCB RID: 52683
				public static LocString FLIP_FACING_BUTTON = UI.CLICK(UI.ClickType.CLICK) + " TO ROTATE";
			}

			// Token: 0x02002FA3 RID: 12195
			public class PLANTERSIDESCREEN
			{
				// Token: 0x0400CDCC RID: 52684
				public static LocString TITLE = "{0} Seeds";

				// Token: 0x0400CDCD RID: 52685
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400CDCE RID: 52686
				public static LocString AWAITINGREQUEST = "PLANT: {0}";

				// Token: 0x0400CDCF RID: 52687
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400CDD0 RID: 52688
				public static LocString AWAITINGREMOVAL = "AWAITING DIGGING UP: {0}";

				// Token: 0x0400CDD1 RID: 52689
				public static LocString ENTITYDEPOSITED = "PLANTED: {0}";

				// Token: 0x0400CDD2 RID: 52690
				public static LocString MUTATIONS_HEADER = "Mutations";

				// Token: 0x0400CDD3 RID: 52691
				public static LocString DEPOSIT = "Plant";

				// Token: 0x0400CDD4 RID: 52692
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400CDD5 RID: 52693
				public static LocString REMOVE = "Uproot";

				// Token: 0x0400CDD6 RID: 52694
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400CDD7 RID: 52695
				public static LocString SELECT_TITLE = "SELECT";

				// Token: 0x0400CDD8 RID: 52696
				public static LocString SELECT_DESC = "Select a seed to plant.";

				// Token: 0x0400CDD9 RID: 52697
				public static LocString LIFECYCLE = "<b>Life Cycle</b>:";

				// Token: 0x0400CDDA RID: 52698
				public static LocString PLANTREQUIREMENTS = "<b>Growth Requirements</b>:";

				// Token: 0x0400CDDB RID: 52699
				public static LocString PLANTEFFECTS = "<b>Effects</b>:";

				// Token: 0x0400CDDC RID: 52700
				public static LocString NUMBEROFHARVESTS = "Harvests: {0}";

				// Token: 0x0400CDDD RID: 52701
				public static LocString YIELD = "{0}: {1} ";

				// Token: 0x0400CDDE RID: 52702
				public static LocString YIELD_NONFOOD = "{0}: {1} ";

				// Token: 0x0400CDDF RID: 52703
				public static LocString YIELD_SINGLE = "{0}";

				// Token: 0x0400CDE0 RID: 52704
				public static LocString YIELDPERHARVEST = "{0} {1} per harvest";

				// Token: 0x0400CDE1 RID: 52705
				public static LocString TOTALHARVESTCALORIESWITHPERUNIT = "{0} [{1} / unit]";

				// Token: 0x0400CDE2 RID: 52706
				public static LocString TOTALHARVESTCALORIES = "{0}";

				// Token: 0x0400CDE3 RID: 52707
				public static LocString BONUS_SEEDS = "Base " + UI.FormatAsLink("Seed", "PLANTS") + " Harvest Chance: {0}";

				// Token: 0x0400CDE4 RID: 52708
				public static LocString YIELD_SEED = "{1} {0}";

				// Token: 0x0400CDE5 RID: 52709
				public static LocString YIELD_SEED_SINGLE = "{0}";

				// Token: 0x0400CDE6 RID: 52710
				public static LocString YIELD_SEED_FINAL_HARVEST = "{1} {0} - Final harvest only";

				// Token: 0x0400CDE7 RID: 52711
				public static LocString YIELD_SEED_SINGLE_FINAL_HARVEST = "{0} - Final harvest only";

				// Token: 0x0400CDE8 RID: 52712
				public static LocString ROTATION_NEED_FLOOR = "<b>Requires upward plot orientation.</b>";

				// Token: 0x0400CDE9 RID: 52713
				public static LocString ROTATION_NEED_WALL = "<b>Requires sideways plot orientation.</b>";

				// Token: 0x0400CDEA RID: 52714
				public static LocString ROTATION_NEED_CEILING = "<b>Requires downward plot orientation.</b>";

				// Token: 0x0400CDEB RID: 52715
				public static LocString NO_SPECIES_SELECTED = "Select a seed species above...";

				// Token: 0x0400CDEC RID: 52716
				public static LocString DISEASE_DROPPER_BURST = "{Disease} Burst: {DiseaseAmount}";

				// Token: 0x0400CDED RID: 52717
				public static LocString DISEASE_DROPPER_CONSTANT = "{Disease}: {DiseaseAmount}";

				// Token: 0x0400CDEE RID: 52718
				public static LocString DISEASE_ON_HARVEST = "{Disease} on crop: {DiseaseAmount}";

				// Token: 0x0400CDEF RID: 52719
				public static LocString AUTO_SELF_HARVEST = "Self-Harvest On Grown";

				// Token: 0x02003C6E RID: 15470
				public class TOOLTIPS
				{
					// Token: 0x0400F018 RID: 61464
					public static LocString PLANTLIFECYCLE = "Duration and number of harvests produced by this plant in a lifetime";

					// Token: 0x0400F019 RID: 61465
					public static LocString PLANTREQUIREMENTS = "Minimum conditions for basic plant growth";

					// Token: 0x0400F01A RID: 61466
					public static LocString PLANTEFFECTS = "Additional attributes of this plant";

					// Token: 0x0400F01B RID: 61467
					public static LocString YIELD = UI.FormatAsLink("{2}", "KCAL") + " produced [" + UI.FormatAsLink("{1}", "KCAL") + " / unit]";

					// Token: 0x0400F01C RID: 61468
					public static LocString YIELD_NONFOOD = "{0} produced per harvest";

					// Token: 0x0400F01D RID: 61469
					public static LocString NUMBEROFHARVESTS = "This plant can mature {0} times before the end of its life cycle";

					// Token: 0x0400F01E RID: 61470
					public static LocString YIELD_SEED = "Sow to grow more of this plant";

					// Token: 0x0400F01F RID: 61471
					public static LocString YIELD_SEED_FINAL_HARVEST = "{0}\n\nProduced in the final harvest of the plant's life cycle";

					// Token: 0x0400F020 RID: 61472
					public static LocString BONUS_SEEDS = "This plant has a {0} chance to produce new seeds when harvested";

					// Token: 0x0400F021 RID: 61473
					public static LocString DISEASE_DROPPER_BURST = "At certain points in this plant's lifecycle, it will emit a burst of {DiseaseAmount} {Disease}.";

					// Token: 0x0400F022 RID: 61474
					public static LocString DISEASE_DROPPER_CONSTANT = "This plant emits {DiseaseAmount} {Disease} while it is alive.";

					// Token: 0x0400F023 RID: 61475
					public static LocString DISEASE_ON_HARVEST = "The {Crop} produced by this plant will have {DiseaseAmount} {Disease} on it.";

					// Token: 0x0400F024 RID: 61476
					public static LocString AUTO_SELF_HARVEST = "This plant will instantly drop its crop and begin regrowing when it is matured.";

					// Token: 0x0400F025 RID: 61477
					public static LocString PLANT_TOGGLE_TOOLTIP = "{0}\n\n{1}\n\n<b>{2}</b> seeds available";
				}
			}

			// Token: 0x02002FA4 RID: 12196
			public class EGGINCUBATOR
			{
				// Token: 0x0400CDF0 RID: 52720
				public static LocString TITLE = "Critter Eggs";

				// Token: 0x0400CDF1 RID: 52721
				public static LocString AWAITINGREQUEST = "INCUBATE: {0}";

				// Token: 0x0400CDF2 RID: 52722
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400CDF3 RID: 52723
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400CDF4 RID: 52724
				public static LocString ENTITYDEPOSITED = "INCUBATING: {0}";

				// Token: 0x0400CDF5 RID: 52725
				public static LocString DEPOSIT = "Incubate";

				// Token: 0x0400CDF6 RID: 52726
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400CDF7 RID: 52727
				public static LocString REMOVE = "Remove";

				// Token: 0x0400CDF8 RID: 52728
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400CDF9 RID: 52729
				public static LocString SELECT_TITLE = "SELECT";

				// Token: 0x0400CDFA RID: 52730
				public static LocString SELECT_DESC = "Select an egg to incubate.";
			}

			// Token: 0x02002FA5 RID: 12197
			public class BASICRECEPTACLE
			{
				// Token: 0x0400CDFB RID: 52731
				public static LocString TITLE = "Displayed Object";

				// Token: 0x0400CDFC RID: 52732
				public static LocString AWAITINGREQUEST = "SELECT: {0}";

				// Token: 0x0400CDFD RID: 52733
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400CDFE RID: 52734
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400CDFF RID: 52735
				public static LocString ENTITYDEPOSITED = "DISPLAYING: {0}";

				// Token: 0x0400CE00 RID: 52736
				public static LocString DEPOSIT = "Select";

				// Token: 0x0400CE01 RID: 52737
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400CE02 RID: 52738
				public static LocString REMOVE = "Remove";

				// Token: 0x0400CE03 RID: 52739
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400CE04 RID: 52740
				public static LocString SELECT_TITLE = "SELECT OBJECT";

				// Token: 0x0400CE05 RID: 52741
				public static LocString SELECT_DESC = "Select an object to display here.";
			}

			// Token: 0x02002FA6 RID: 12198
			public class SPECIALCARGOBAYCLUSTER
			{
				// Token: 0x0400CE06 RID: 52742
				public static LocString TITLE = "Target Critter";

				// Token: 0x0400CE07 RID: 52743
				public static LocString AWAITINGREQUEST = "SELECT: {0}";

				// Token: 0x0400CE08 RID: 52744
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400CE09 RID: 52745
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400CE0A RID: 52746
				public static LocString ENTITYDEPOSITED = "CONTENTS: {0}";

				// Token: 0x0400CE0B RID: 52747
				public static LocString DEPOSIT = "Select";

				// Token: 0x0400CE0C RID: 52748
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400CE0D RID: 52749
				public static LocString REMOVE = "Remove";

				// Token: 0x0400CE0E RID: 52750
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400CE0F RID: 52751
				public static LocString SELECT_TITLE = "SELECT CRITTER";

				// Token: 0x0400CE10 RID: 52752
				public static LocString SELECT_DESC = "Select a critter to store in this module.";
			}

			// Token: 0x02002FA7 RID: 12199
			public class LURE
			{
				// Token: 0x0400CE11 RID: 52753
				public static LocString TITLE = "Select Bait";

				// Token: 0x0400CE12 RID: 52754
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400CE13 RID: 52755
				public static LocString AWAITINGREQUEST = "PLANT: {0}";

				// Token: 0x0400CE14 RID: 52756
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400CE15 RID: 52757
				public static LocString AWAITINGREMOVAL = "AWAITING DIGGING UP: {0}";

				// Token: 0x0400CE16 RID: 52758
				public static LocString ENTITYDEPOSITED = "PLANTED: {0}";

				// Token: 0x0400CE17 RID: 52759
				public static LocString ATTRACTS = "Attract {1}s";
			}

			// Token: 0x02002FA8 RID: 12200
			public class ROLESTATION
			{
				// Token: 0x0400CE18 RID: 52760
				public static LocString TITLE = "Duplicant Skills";

				// Token: 0x0400CE19 RID: 52761
				public static LocString OPENROLESBUTTON = "SKILLS";
			}

			// Token: 0x02002FA9 RID: 12201
			public class RESEARCHSIDESCREEN
			{
				// Token: 0x0400CE1A RID: 52762
				public static LocString TITLE = "Select Research";

				// Token: 0x0400CE1B RID: 52763
				public static LocString CURRENTLYRESEARCHING = "Currently Researching";

				// Token: 0x0400CE1C RID: 52764
				public static LocString NOSELECTEDRESEARCH = "No Research selected";

				// Token: 0x0400CE1D RID: 52765
				public static LocString OPENRESEARCHBUTTON = "RESEARCH";
			}

			// Token: 0x02002FAA RID: 12202
			public class REFINERYSIDESCREEN
			{
				// Token: 0x0400CE1E RID: 52766
				public static LocString RECIPE_FROM_TO = "{0} to {1}";

				// Token: 0x0400CE1F RID: 52767
				public static LocString RECIPE_WITH = "{1} ({0})";

				// Token: 0x0400CE20 RID: 52768
				public static LocString RECIPE_FROM_TO_WITH_NEWLINES = "{0}\nto\n{1}";

				// Token: 0x0400CE21 RID: 52769
				public static LocString RECIPE_FROM_TO_COMPOSITE = "{0} to {1} and {2}";

				// Token: 0x0400CE22 RID: 52770
				public static LocString RECIPE_FROM_TO_HEP = "{0} to " + UI.FormatAsLink("Radbolts", "RADIATION") + " and {1}";

				// Token: 0x0400CE23 RID: 52771
				public static LocString RECIPE_SIMPLE_INCLUDE_AMOUNTS = "{0} {1}";

				// Token: 0x0400CE24 RID: 52772
				public static LocString RECIPE_FROM_TO_INCLUDE_AMOUNTS = "{2} {0} to {3} {1}";

				// Token: 0x0400CE25 RID: 52773
				public static LocString RECIPE_WITH_INCLUDE_AMOUNTS = "{3} {1} ({2} {0})";

				// Token: 0x0400CE26 RID: 52774
				public static LocString RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS = "{3} {0} to {4} {1} and {5} {2}";

				// Token: 0x0400CE27 RID: 52775
				public static LocString RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS = "{2} {0} to {3} " + UI.FormatAsLink("Radbolts", "RADIATION") + " and {4} {1}";
			}

			// Token: 0x02002FAB RID: 12203
			public class SEALEDDOORSIDESCREEN
			{
				// Token: 0x0400CE28 RID: 52776
				public static LocString TITLE = "Sealed Door";

				// Token: 0x0400CE29 RID: 52777
				public static LocString LABEL = "This door requires a sample to unlock.";

				// Token: 0x0400CE2A RID: 52778
				public static LocString BUTTON = "SUBMIT BIOSCAN";

				// Token: 0x0400CE2B RID: 52779
				public static LocString AWAITINGBUTTON = "AWAITING BIOSCAN";
			}

			// Token: 0x02002FAC RID: 12204
			public class ENCRYPTEDLORESIDESCREEN
			{
				// Token: 0x0400CE2C RID: 52780
				public static LocString TITLE = "Encrypted File";

				// Token: 0x0400CE2D RID: 52781
				public static LocString LABEL = "This computer contains encrypted files.";

				// Token: 0x0400CE2E RID: 52782
				public static LocString BUTTON = "ATTEMPT DECRYPTION";

				// Token: 0x0400CE2F RID: 52783
				public static LocString AWAITINGBUTTON = "AWAITING DECRYPTION";
			}

			// Token: 0x02002FAD RID: 12205
			public class ACCESS_CONTROL_SIDE_SCREEN
			{
				// Token: 0x0400CE30 RID: 52784
				public static LocString TITLE = "Door Access Control";

				// Token: 0x0400CE31 RID: 52785
				public static LocString DOOR_DEFAULT = "Default";

				// Token: 0x0400CE32 RID: 52786
				public static LocString MINION_ACCESS = "Access Permissions";

				// Token: 0x0400CE33 RID: 52787
				public static LocString MINION_SELECT_TOOLTIP = string.Concat(new string[]
				{
					UI.CLICK(UI.ClickType.Click),
					" to toggle between default and custom ",
					UI.PRE_KEYWORD,
					"Access Permissions",
					UI.PST_KEYWORD,
					" for this Duplicant\n\n",
					UI.PRE_KEYWORD,
					"Access Permissions",
					UI.PST_KEYWORD,
					" apply only when door is set to auto"
				});

				// Token: 0x0400CE34 RID: 52788
				public static LocString GO_LEFT_ENABLED = "Passing Left through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400CE35 RID: 52789
				public static LocString GO_LEFT_DISABLED = "Passing Left through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400CE36 RID: 52790
				public static LocString GO_RIGHT_ENABLED = "Passing Right through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400CE37 RID: 52791
				public static LocString GO_RIGHT_DISABLED = "Passing Right through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400CE38 RID: 52792
				public static LocString GO_UP_ENABLED = "Passing Up through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400CE39 RID: 52793
				public static LocString GO_UP_DISABLED = "Passing Up through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400CE3A RID: 52794
				public static LocString GO_DOWN_ENABLED = "Passing Down through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400CE3B RID: 52795
				public static LocString GO_DOWN_DISABLED = "Passing Down through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400CE3C RID: 52796
				public static LocString SET_TO_DEFAULT = UI.CLICK(UI.ClickType.Click) + " to clear custom permissions";

				// Token: 0x0400CE3D RID: 52797
				public static LocString SET_TO_CUSTOM = UI.CLICK(UI.ClickType.Click) + " to assign custom permissions";

				// Token: 0x0400CE3E RID: 52798
				public static LocString USING_DEFAULT = "Default Access";

				// Token: 0x0400CE3F RID: 52799
				public static LocString USING_CUSTOM = "Custom Access";

				// Token: 0x0400CE40 RID: 52800
				public static LocString EMPTY_CATEGORY = "None available";

				// Token: 0x0400CE41 RID: 52801
				public static LocString CATEGORY_HEADER_TOOLTIP = string.Concat(new string[]
				{
					"Use the arrows to set default ",
					UI.PRE_KEYWORD,
					"Access Permissions",
					UI.PST_KEYWORD,
					" for all entities in this category\n\n",
					UI.CLICK(UI.ClickType.Click),
					" to collapse or expand this category"
				});
			}

			// Token: 0x02002FAE RID: 12206
			public class OWNABLESSIDESCREEN
			{
				// Token: 0x0400CE42 RID: 52802
				public static LocString TITLE = "Equipment and Amenities";

				// Token: 0x0400CE43 RID: 52803
				public static LocString NO_ITEM_ASSIGNED = "Assign";

				// Token: 0x0400CE44 RID: 52804
				public static LocString NO_ITEM_FOUND = "None found";

				// Token: 0x0400CE45 RID: 52805
				public static LocString NO_APPLICABLE = "{0}: Ineligible";

				// Token: 0x02003C6F RID: 15471
				public static class TOOLTIPS
				{
					// Token: 0x0400F026 RID: 61478
					public static LocString NO_APPLICABLE = "This Duplicant cannot be assigned " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

					// Token: 0x0400F027 RID: 61479
					public static LocString NO_ITEM_ASSIGNED = string.Concat(new string[]
					{
						"Click to view and assign existing ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" to this Duplicant"
					});

					// Token: 0x0400F028 RID: 61480
					public static LocString ITEM_ASSIGNED_GENERIC = "This Duplicant has {0} assigned to them";

					// Token: 0x0400F029 RID: 61481
					public static LocString ITEM_ASSIGNED = "{0}\n\n{1}";
				}

				// Token: 0x02003C70 RID: 15472
				public class CATEGORIES
				{
					// Token: 0x0400F02A RID: 61482
					public static LocString SUITS = "Suits";

					// Token: 0x0400F02B RID: 61483
					public static LocString AMENITIES = "Amenities";
				}
			}

			// Token: 0x02002FAF RID: 12207
			public class OWNABLESSECONDSIDESCREEN
			{
				// Token: 0x0400CE46 RID: 52806
				public static LocString TITLE = "{0}";

				// Token: 0x0400CE47 RID: 52807
				public static LocString NONE_ROW_LABEL = "Clear";

				// Token: 0x0400CE48 RID: 52808
				public static LocString NONE_ROW_TOOLTIP = "Click to remove any item currently assigned to the selected slot";

				// Token: 0x0400CE49 RID: 52809
				public static LocString ASSIGNED_TO_OTHER_STATUS = "Assigned to: {0}";

				// Token: 0x0400CE4A RID: 52810
				public static LocString ASSIGNED_TO_SELF_STATUS = "Assigned";

				// Token: 0x0400CE4B RID: 52811
				public static LocString NOT_ASSIGNED = "Unassigned";
			}

			// Token: 0x02002FB0 RID: 12208
			public class ASSIGNABLESIDESCREEN
			{
				// Token: 0x0400CE4C RID: 52812
				public static LocString TITLE = "Assign {0}";

				// Token: 0x0400CE4D RID: 52813
				public static LocString ASSIGNED = "Assigned";

				// Token: 0x0400CE4E RID: 52814
				public static LocString UNASSIGNED = "-";

				// Token: 0x0400CE4F RID: 52815
				public static LocString DISABLED = "Ineligible";

				// Token: 0x0400CE50 RID: 52816
				public static LocString SORT_BY_DUPLICANT = "Duplicant";

				// Token: 0x0400CE51 RID: 52817
				public static LocString SORT_BY_ASSIGNMENT = "Assignment";

				// Token: 0x0400CE52 RID: 52818
				public static LocString ASSIGN_TO_TOOLTIP = "Assign to {0}";

				// Token: 0x0400CE53 RID: 52819
				public static LocString UNASSIGN_TOOLTIP = "Assigned to {0}";

				// Token: 0x0400CE54 RID: 52820
				public static LocString DISABLED_TOOLTIP = "{0} is ineligible for this skill assignment";

				// Token: 0x0400CE55 RID: 52821
				public static LocString PUBLIC = "Public";
			}

			// Token: 0x02002FB1 RID: 12209
			public class COMETDETECTORSIDESCREEN
			{
				// Token: 0x0400CE56 RID: 52822
				public static LocString TITLE = "Space Scanner";

				// Token: 0x0400CE57 RID: 52823
				public static LocString HEADER = "Sends automation signal when selected object is detected";

				// Token: 0x0400CE58 RID: 52824
				public static LocString ASSIGNED = "Assigned";

				// Token: 0x0400CE59 RID: 52825
				public static LocString UNASSIGNED = "-";

				// Token: 0x0400CE5A RID: 52826
				public static LocString DISABLED = "Ineligible";

				// Token: 0x0400CE5B RID: 52827
				public static LocString SORT_BY_DUPLICANT = "Duplicant";

				// Token: 0x0400CE5C RID: 52828
				public static LocString SORT_BY_ASSIGNMENT = "Assignment";

				// Token: 0x0400CE5D RID: 52829
				public static LocString ASSIGN_TO_TOOLTIP = "Scanning for {0}";

				// Token: 0x0400CE5E RID: 52830
				public static LocString UNASSIGN_TOOLTIP = "Scanning for {0}";

				// Token: 0x0400CE5F RID: 52831
				public static LocString NOTHING = "Nothing";

				// Token: 0x0400CE60 RID: 52832
				public static LocString COMETS = "Meteor Showers";

				// Token: 0x0400CE61 RID: 52833
				public static LocString ROCKETS = "Rocket Landing Ping";

				// Token: 0x0400CE62 RID: 52834
				public static LocString DUPEMADE = "Interplanetary Payloads";
			}

			// Token: 0x02002FB2 RID: 12210
			public class GEOTUNERSIDESCREEN
			{
				// Token: 0x0400CE63 RID: 52835
				public static LocString TITLE = "Select Geyser";

				// Token: 0x0400CE64 RID: 52836
				public static LocString DESCRIPTION = "Select an analyzed geyser to transmit amplification data to.";

				// Token: 0x0400CE65 RID: 52837
				public static LocString NOTHING = "No geyser selected";

				// Token: 0x0400CE66 RID: 52838
				public static LocString UNSTUDIED_TOOLTIP = "This geyser must be analyzed before it can be selected\n\nDouble-click to view this geyser";

				// Token: 0x0400CE67 RID: 52839
				public static LocString STUDIED_TOOLTIP = string.Concat(new string[]
				{
					"Increase this geyser's ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" and output"
				});

				// Token: 0x0400CE68 RID: 52840
				public static LocString GEOTUNER_LIMIT_TOOLTIP = "This geyser cannot be targeted by more " + UI.PRE_KEYWORD + "Geotuners" + UI.PST_KEYWORD;

				// Token: 0x0400CE69 RID: 52841
				public static LocString STUDIED_TOOLTIP_MATERIAL = "Required resource: {MATERIAL}";

				// Token: 0x0400CE6A RID: 52842
				public static LocString STUDIED_TOOLTIP_POTENTIAL_OUTPUT = "Potential Output {POTENTIAL_OUTPUT}";

				// Token: 0x0400CE6B RID: 52843
				public static LocString STUDIED_TOOLTIP_BASE_TEMP = "Base {BASE}";

				// Token: 0x0400CE6C RID: 52844
				public static LocString STUDIED_TOOLTIP_VISIT_GEYSER = "Double-click to view this geyser";

				// Token: 0x0400CE6D RID: 52845
				public static LocString STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE = "Geotuned ";

				// Token: 0x0400CE6E RID: 52846
				public static LocString STUDIED_TOOLTIP_NUMBER_HOVERED = "This geyser is targeted by {0} Geotuners";
			}

			// Token: 0x02002FB3 RID: 12211
			public class REMOTE_WORK_TERMINAL_SIDE_SCREEN
			{
				// Token: 0x0400CE6F RID: 52847
				public static LocString TITLE = "Dock Assignment";

				// Token: 0x0400CE70 RID: 52848
				public static LocString DESCRIPTION = "Select a remote worker dock for this controller to target.";

				// Token: 0x0400CE71 RID: 52849
				public static LocString NOTHING_SELECTED = "None";

				// Token: 0x0400CE72 RID: 52850
				public static LocString DOCK_TOOLTIP = "Click to assign this dock to this controller\n\nDouble-click to view this dock";
			}

			// Token: 0x02002FB4 RID: 12212
			public class COMMAND_MODULE_SIDE_SCREEN
			{
				// Token: 0x0400CE73 RID: 52851
				public static LocString TITLE = "Launch Conditions";

				// Token: 0x0400CE74 RID: 52852
				public static LocString DESTINATION_BUTTON = "Show Starmap";

				// Token: 0x0400CE75 RID: 52853
				public static LocString DESTINATION_BUTTON_EXPANSION = "Show Starmap";
			}

			// Token: 0x02002FB5 RID: 12213
			public class CLUSTERDESTINATIONSIDESCREEN
			{
				// Token: 0x0400CE76 RID: 52854
				public static LocString TITLE = "Destination";

				// Token: 0x0400CE77 RID: 52855
				public static LocString DESTINATION_LABEL = "Destination: {0}";

				// Token: 0x0400CE78 RID: 52856
				public static LocString DESTINATION_LABEL_SELECTING = "Selecting new destination...";

				// Token: 0x0400CE79 RID: 52857
				public static LocString DESTINATION_LABEL_INVALID = "None selected";

				// Token: 0x0400CE7A RID: 52858
				public static LocString CHANGE_DESTINATION_BUTTON = "Select Destination";

				// Token: 0x0400CE7B RID: 52859
				public static LocString CHANGE_DESTINATION_BUTTON_TOOLTIP = "Select a new destination for this rocket";

				// Token: 0x0400CE7C RID: 52860
				public static LocString CHANGE_DESTINATION_BUTTON_SELECTING_TOOLTIP = "Select a destination by clicking on the desired " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " hex";

				// Token: 0x0400CE7D RID: 52861
				public static LocString CHANGE_DESTINATION_BUTTON_TOOLTIP_MISSILE = "Select a new target for this projectile launcher";

				// Token: 0x0400CE7E RID: 52862
				public static LocString CHANGE_DESTINATION_BUTTON_TOOLTIP_RAILGUN = "Select a new target for this payload launcher";

				// Token: 0x0400CE7F RID: 52863
				public static LocString CLEAR_DESTINATION_BUTTON = "Clear";

				// Token: 0x0400CE80 RID: 52864
				public static LocString CLEAR_DESTINATION_BUTTON_TOOLTIP = "Clear this rocket's selected destination";

				// Token: 0x0400CE81 RID: 52865
				public static LocString CLEAR_DESTINATION_BUTTON_TOOLTIP_MISSILE = "Clear this projectile launcher's selected target";

				// Token: 0x0400CE82 RID: 52866
				public static LocString CLEAR_DESTINATION_BUTTON_TOOLTIP_RAILGUN = "Clear this payload launcher's selected target";

				// Token: 0x0400CE83 RID: 52867
				public static LocString LANDING_PLATFORM_LABEL = "Landing Site: {0}";

				// Token: 0x0400CE84 RID: 52868
				public static LocString FIRSTAVAILABLE = "Any " + BUILDINGS.PREFABS.LAUNCHPAD.NAME;

				// Token: 0x0400CE85 RID: 52869
				public static LocString DROPDOWN_TOOLTIP_VALID_SITE = "Land at {0} when the site is clear";

				// Token: 0x0400CE86 RID: 52870
				public static LocString DROPDOWN_TOOLTIP_FIRST_AVAILABLE = "Select the first available landing site";

				// Token: 0x0400CE87 RID: 52871
				public static LocString DROPDOWN_TOOLTIP_TOO_SHORT = "This rocket's height exceeds the space available in this landing site";

				// Token: 0x0400CE88 RID: 52872
				public static LocString DROPDOWN_TOOLTIP_PATH_OBSTRUCTED = "Landing path obstructed";

				// Token: 0x0400CE89 RID: 52873
				public static LocString DROPDOWN_TOOLTIP_SITE_OBSTRUCTED = "Landing position on the platform is obstructed";

				// Token: 0x0400CE8A RID: 52874
				public static LocString DROPDOWN_TOOLTIP_PAD_DISABLED = BUILDINGS.PREFABS.LAUNCHPAD.NAME + " is disabled";

				// Token: 0x0400CE8B RID: 52875
				public static LocString ROUNDTRIP_LABEL_ONE_WAY = "Flight Plan: One Way";

				// Token: 0x0400CE8C RID: 52876
				public static LocString ROUNDTRIP_LABEL_ROUNDTRIP = "Flight Plan: Round Trip";

				// Token: 0x0400CE8D RID: 52877
				public static LocString ROUNDTRIP_BUTTON_ROUNDTRIP = "Set Round Trip";

				// Token: 0x0400CE8E RID: 52878
				public static LocString ROUNDTRIP_BUTTON_ONE_WAY = "Set One Way Trip";

				// Token: 0x0400CE8F RID: 52879
				public static LocString ROUNDTRIP_BUTTON_TOOLTIP_ROUNDTRIP = "Set rocket to travel to its destination and back";

				// Token: 0x0400CE90 RID: 52880
				public static LocString ROUNDTRIP_BUTTON_TOOLTIP_ONE_WAY = "Set rocket to travel to its destination and remain there";

				// Token: 0x0400CE91 RID: 52881
				public static LocString TITLE_MISSILE_TARGET = "Long Range Target";

				// Token: 0x0400CE92 RID: 52882
				public static LocString NONEAVAILABLE = "No landing site";

				// Token: 0x0400CE93 RID: 52883
				public static LocString NO_TALL_SITES_AVAILABLE = "No landing sites fit the height of this rocket";

				// Token: 0x02003C71 RID: 15473
				public class ASSIGNMENTSTATUS
				{
					// Token: 0x0400F02C RID: 61484
					public static LocString LOCAL = "Current";

					// Token: 0x0400F02D RID: 61485
					public static LocString DESTINATION = "Destination";
				}
			}

			// Token: 0x02002FB6 RID: 12214
			public class EQUIPPABLESIDESCREEN
			{
				// Token: 0x0400CE94 RID: 52884
				public static LocString TITLE = "Equip {0}";

				// Token: 0x0400CE95 RID: 52885
				public static LocString ASSIGNEDTO = "Assigned to: {Assignee}";

				// Token: 0x0400CE96 RID: 52886
				public static LocString UNASSIGNED = "Unassigned";

				// Token: 0x0400CE97 RID: 52887
				public static LocString GENERAL_CURRENTASSIGNED = "(Owner)";
			}

			// Token: 0x02002FB7 RID: 12215
			public class EQUIPPABLE_SIDE_SCREEN
			{
				// Token: 0x0400CE98 RID: 52888
				public static LocString TITLE = "Assign To Duplicant";

				// Token: 0x0400CE99 RID: 52889
				public static LocString CURRENTLY_EQUIPPED = "Currently Equipped:\n{0}";

				// Token: 0x0400CE9A RID: 52890
				public static LocString NONE_EQUIPPED = "None";

				// Token: 0x0400CE9B RID: 52891
				public static LocString EQUIP_BUTTON = "Equip";

				// Token: 0x0400CE9C RID: 52892
				public static LocString DROP_BUTTON = "Drop";

				// Token: 0x0400CE9D RID: 52893
				public static LocString SWAP_BUTTON = "Swap";
			}

			// Token: 0x02002FB8 RID: 12216
			public class TELEPADSIDESCREEN
			{
				// Token: 0x0400CE9E RID: 52894
				public static LocString TITLE = "Printables";

				// Token: 0x0400CE9F RID: 52895
				public static LocString NEXTPRODUCTION = "Next Production: {0}";

				// Token: 0x0400CEA0 RID: 52896
				public static LocString GAMEOVER = "Colony Lost";

				// Token: 0x0400CEA1 RID: 52897
				public static LocString VICTORY_CONDITIONS = "Hardwired Imperatives";

				// Token: 0x0400CEA2 RID: 52898
				public static LocString SUMMARY_TITLE = "Colony Summary";

				// Token: 0x0400CEA3 RID: 52899
				public static LocString SKILLS_BUTTON = "Duplicant Skills";
			}

			// Token: 0x02002FB9 RID: 12217
			public class VALVESIDESCREEN
			{
				// Token: 0x0400CEA4 RID: 52900
				public static LocString TITLE = "Flow Control";
			}

			// Token: 0x02002FBA RID: 12218
			public class BIONIC_SIDE_SCREEN
			{
				// Token: 0x0400CEA5 RID: 52901
				public static LocString TITLE = "Boosters";

				// Token: 0x0400CEA6 RID: 52902
				public static LocString UPGRADE_SLOT_LOCKED = "N/A";

				// Token: 0x0400CEA7 RID: 52903
				public static LocString UPGRADE_SLOT_EMPTY = "Empty";

				// Token: 0x0400CEA8 RID: 52904
				public static LocString UPGRADE_SLOT_ASSIGNED = "Assigned";

				// Token: 0x0400CEA9 RID: 52905
				public static LocString UPGRADE_SLOT_INSTALLED = "Installed";

				// Token: 0x0400CEAA RID: 52906
				public static LocString CURRENT_WATTAGE_LABEL = "Current Wattage: <b>{0}</b>";

				// Token: 0x0400CEAB RID: 52907
				public static LocString CURRENT_WATTAGE_LABEL_BATTERY_SAVE_MODE = "Current Wattage: <color=#0303fc><b>{0}</b> {1}</color>";

				// Token: 0x0400CEAC RID: 52908
				public static LocString CURRENT_WATTAGE_LABEL_OFFLINE = "Current Wattage: <color=#GG2222>Offline {0}</color>";

				// Token: 0x0400CEAD RID: 52909
				public const string OFFLINE_MODE_COLOR = "<color=#GG2222>";

				// Token: 0x0400CEAE RID: 52910
				public const string BATTERY_SAVE_MODE_COLOR = "<color=#0303fc>";

				// Token: 0x0400CEAF RID: 52911
				public const string COLOR_END = "</color>";

				// Token: 0x02003C72 RID: 15474
				public class TOOLTIP
				{
					// Token: 0x0400F02E RID: 61486
					public static LocString CURRENT_WATTAGE = "Wattage is the amount of energy that this Duplicant's bionic parts consume per second\n\nInstalled boosters consume wattage while active";

					// Token: 0x0400F02F RID: 61487
					public static LocString SLOT_LOCKED = "This booster slot is unavailable\n\nBooster slots can be unlocked using " + UI.PRE_KEYWORD + "Skill Points" + UI.PST_KEYWORD;

					// Token: 0x0400F030 RID: 61488
					public static LocString SLOT_EMPTY = "No booster installed\n\nClick to view available boosters";

					// Token: 0x0400F031 RID: 61489
					public static LocString SLOT_ASSIGNED = string.Concat(new string[]
					{
						"This ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" will be installed when it is within this Duplicant's reach"
					});

					// Token: 0x0400F032 RID: 61490
					public static LocString SLOT_INSTALLED = "{0}";
				}

				// Token: 0x02003C73 RID: 15475
				public class BOOSTER_ASSIGNMENT
				{
					// Token: 0x0400F033 RID: 61491
					public static LocString NOT_ALREADY_ASSIGNED = "{0} does not currently have this type of booster assigned";

					// Token: 0x0400F034 RID: 61492
					public static LocString ALREADY_ASSIGNED = "{0} currently has <b>{1} of this type</b> of booster assigned";

					// Token: 0x0400F035 RID: 61493
					public static LocString AVAILABLE_SLOTS = "{0} has <b>{1}/{2}</b> booster slots assigned";

					// Token: 0x0400F036 RID: 61494
					public static LocString NO_AVAILABLE_SLOTS = UI.YELLOW_PREFIX + "All of {0}'s booster slots are currently assigned: <b>{1}/{2}</b>" + UI.COLOR_SUFFIX;

					// Token: 0x0400F037 RID: 61495
					public static LocString HEADER_PERKS = "<b>Enables:</b>";

					// Token: 0x0400F038 RID: 61496
					public static LocString HEADER_ATTRIBUTES = "<b>Boosts:</b>";
				}
			}

			// Token: 0x02002FBB RID: 12219
			public class LIMIT_VALVE_SIDE_SCREEN
			{
				// Token: 0x0400CEB0 RID: 52912
				public static LocString TITLE = "Meter Control";

				// Token: 0x0400CEB1 RID: 52913
				public static LocString AMOUNT = "Amount: {0}";

				// Token: 0x0400CEB2 RID: 52914
				public static LocString LIMIT = "Limit:";

				// Token: 0x0400CEB3 RID: 52915
				public static LocString RESET_BUTTON = "Reset Amount";

				// Token: 0x0400CEB4 RID: 52916
				public static LocString SLIDER_TOOLTIP_UNITS = "The amount of Units or Mass passing through the sensor.";
			}

			// Token: 0x02002FBC RID: 12220
			public class NUCLEAR_REACTOR_SIDE_SCREEN
			{
				// Token: 0x0400CEB5 RID: 52917
				public static LocString TITLE = "Reaction Mass Target";

				// Token: 0x0400CEB6 RID: 52918
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will attempt to keep the reactor supplied with ",
					UI.PRE_KEYWORD,
					"{0}{1}",
					UI.PST_KEYWORD,
					" of ",
					UI.PRE_KEYWORD,
					"{2}",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002FBD RID: 12221
			public class MANUALGENERATORSIDESCREEN
			{
				// Token: 0x0400CEB7 RID: 52919
				public static LocString TITLE = "Battery Recharge Threshold";

				// Token: 0x0400CEB8 RID: 52920
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400CEB9 RID: 52921
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will be requested to operate this generator when the total charge of the connected ",
					UI.PRE_KEYWORD,
					"Batteries",
					UI.PST_KEYWORD,
					" falls below <b>{0}%</b>"
				});
			}

			// Token: 0x02002FBE RID: 12222
			public class PRINTERCEPTORSIDESCREEN
			{
				// Token: 0x0400CEBA RID: 52922
				public static LocString TITLE = "Inventory";

				// Token: 0x0400CEBB RID: 52923
				public static LocString BUTTON_PRINT = "Choose a Blueprint";

				// Token: 0x0400CEBC RID: 52924
				public static LocString BUTTON_INTERCEPTOR = "Intercept Charge";

				// Token: 0x0400CEBD RID: 52925
				public static LocString INTERCEPT_METER = "{0}/{1} Stored Charges";

				// Token: 0x0400CEBE RID: 52926
				public static LocString INTERCEPT_TOOLTIP = "Click to intercept a charge from the " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME;

				// Token: 0x0400CEBF RID: 52927
				public static LocString INTERCEPT_TOOLTIP_DISABLED = "There is no " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " charge to intercept";

				// Token: 0x0400CEC0 RID: 52928
				public static LocString INTERCEPT_TOOLTIP_DISABLED_TOO_FULL = "This building is fully charged\n\nStored charges must be consumed before additional charges can be intercepted";

				// Token: 0x0400CEC1 RID: 52929
				public static LocString PRINT_TOOLTIP = string.Concat(new string[]
				{
					"Click to view printable ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Seeds",
					UI.PST_KEYWORD
				});

				// Token: 0x0400CEC2 RID: 52930
				public static LocString PRINT_TOOLTIP_DISABLED = "Insufficient stored materials";

				// Token: 0x0400CEC3 RID: 52931
				public static LocString DATABANK_COUNT = "{0} Stored Data Banks";

				// Token: 0x0400CEC4 RID: 52932
				public static LocString LOCKED_LABEL = "Access Code Required";

				// Token: 0x0400CEC5 RID: 52933
				public static LocString ACTIVATE_TOILET_BUTTON = "Unclog";

				// Token: 0x0400CEC6 RID: 52934
				public static LocString ACTIVATE_TOILET_BUTTON_TOOLTIP = "Send a Duplicant to plunge the depths of this building";

				// Token: 0x0400CEC7 RID: 52935
				public static LocString ACTIVATE_TOILET_BUTTON_CANCEL = "Cancel Unclog";

				// Token: 0x0400CEC8 RID: 52936
				public static LocString ACTIVATE_TOILET_BUTTON_CANCEL_TOOLTIP = "Cancel plunging this toilet";
			}

			// Token: 0x02002FBF RID: 12223
			public class SPACEHEATERSIDESCREEN
			{
				// Token: 0x0400CEC9 RID: 52937
				public static LocString TITLE = "Power Consumption";

				// Token: 0x0400CECA RID: 52938
				public static LocString CURRENT_THRESHOLD = "Current Power Consumption: {0}";

				// Token: 0x0400CECB RID: 52939
				public static LocString TOOLTIP = "Adjust power consumption to determine how much heat is produced\n\nCurrent heat production: <b>{0}</b>";
			}

			// Token: 0x02002FC0 RID: 12224
			public class MANUALDELIVERYGENERATORSIDESCREEN
			{
				// Token: 0x0400CECC RID: 52940
				public static LocString TITLE = "Fuel Request Threshold";

				// Token: 0x0400CECD RID: 52941
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400CECE RID: 52942
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will be requested to deliver ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when the total charge of the connected ",
					UI.PRE_KEYWORD,
					"Batteries",
					UI.PST_KEYWORD,
					" falls below <b>{1}%</b>"
				});
			}

			// Token: 0x02002FC1 RID: 12225
			public class TIME_OF_DAY_SIDE_SCREEN
			{
				// Token: 0x0400CECF RID: 52943
				public static LocString TITLE = "Time-of-Day Sensor";

				// Token: 0x0400CED0 RID: 52944
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" after the selected Turn On time, and a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" after the selected Turn Off time"
				});

				// Token: 0x0400CED1 RID: 52945
				public static LocString START = "Turn On";

				// Token: 0x0400CED2 RID: 52946
				public static LocString STOP = "Turn Off";
			}

			// Token: 0x02002FC2 RID: 12226
			public class CRITTER_COUNT_SIDE_SCREEN
			{
				// Token: 0x0400CED3 RID: 52947
				public static LocString TITLE = "Critter Count Sensor";

				// Token: 0x0400CED4 RID: 52948
				public static LocString TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if there are more than <b>{0}</b> ",
					UI.PRE_KEYWORD,
					"Critters",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" in the room"
				});

				// Token: 0x0400CED5 RID: 52949
				public static LocString TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if there are fewer than <b>{0}</b> ",
					UI.PRE_KEYWORD,
					"Critters",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" in the room"
				});

				// Token: 0x0400CED6 RID: 52950
				public static LocString START = "Turn On";

				// Token: 0x0400CED7 RID: 52951
				public static LocString STOP = "Turn Off";

				// Token: 0x0400CED8 RID: 52952
				public static LocString VALUE_NAME = "Count";
			}

			// Token: 0x02002FC3 RID: 12227
			public class OIL_WELL_CAP_SIDE_SCREEN
			{
				// Token: 0x0400CED9 RID: 52953
				public static LocString TITLE = "Backpressure Release Threshold";

				// Token: 0x0400CEDA RID: 52954
				public static LocString TOOLTIP = "Duplicants will be requested to release backpressure buildup when it exceeds <b>{0}%</b>";
			}

			// Token: 0x02002FC4 RID: 12228
			public class MODULAR_CONDUIT_PORT_SIDE_SCREEN
			{
				// Token: 0x0400CEDB RID: 52955
				public static LocString TITLE = "Pump Control";

				// Token: 0x0400CEDC RID: 52956
				public static LocString LABEL_UNLOAD = "Unload Only";

				// Token: 0x0400CEDD RID: 52957
				public static LocString LABEL_BOTH = "Load/Unload";

				// Token: 0x0400CEDE RID: 52958
				public static LocString LABEL_LOAD = "Load Only";

				// Token: 0x0400CEDF RID: 52959
				public static readonly List<LocString> LABELS = new List<LocString>
				{
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_UNLOAD,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_BOTH,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_LOAD
				};

				// Token: 0x0400CEE0 RID: 52960
				public static LocString TOOLTIP_UNLOAD = "This pump will attempt to <b>Unload</b> cargo from the landed rocket, but not attempt to load new cargo";

				// Token: 0x0400CEE1 RID: 52961
				public static LocString TOOLTIP_BOTH = "This pump will both <b>Load</b> and <b>Unload</b> cargo from the landed rocket";

				// Token: 0x0400CEE2 RID: 52962
				public static LocString TOOLTIP_LOAD = "This pump will attempt to <b>Load</b> cargo onto the landed rocket, but will not unload it";

				// Token: 0x0400CEE3 RID: 52963
				public static readonly List<LocString> TOOLTIPS = new List<LocString>
				{
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_UNLOAD,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_BOTH,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_LOAD
				};

				// Token: 0x0400CEE4 RID: 52964
				public static LocString DESCRIPTION = "";
			}

			// Token: 0x02002FC5 RID: 12229
			public class LOGIC_BUFFER_SIDE_SCREEN
			{
				// Token: 0x0400CEE5 RID: 52965
				public static LocString TITLE = "Buffer Time";

				// Token: 0x0400CEE6 RID: 52966
				public static LocString TOOLTIP = "Will continue to send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for <b>{0} seconds</b> after receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002FC6 RID: 12230
			public class LOGIC_FILTER_SIDE_SCREEN
			{
				// Token: 0x0400CEE7 RID: 52967
				public static LocString TITLE = "Filter Time";

				// Token: 0x0400CEE8 RID: 52968
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Will only send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if it receives ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" for longer than <b>{0} seconds</b>"
				});
			}

			// Token: 0x02002FC7 RID: 12231
			public class TIME_RANGE_SIDE_SCREEN
			{
				// Token: 0x0400CEE9 RID: 52969
				public static LocString TITLE = "Time Schedule";

				// Token: 0x0400CEEA RID: 52970
				public static LocString ON = "Activation Time";

				// Token: 0x0400CEEB RID: 52971
				public static LocString ON_TOOLTIP = string.Concat(new string[]
				{
					"Activation time determines the time of day this sensor should begin sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" {0} through the day"
				});

				// Token: 0x0400CEEC RID: 52972
				public static LocString DURATION = "Active Duration";

				// Token: 0x0400CEED RID: 52973
				public static LocString DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Active duration determines what percentage of the day this sensor will spend sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for {0} of the day"
				});
			}

			// Token: 0x02002FC8 RID: 12232
			public class TIMER_SIDE_SCREEN
			{
				// Token: 0x0400CEEE RID: 52974
				public static LocString TITLE = "Timer";

				// Token: 0x0400CEEF RID: 52975
				public static LocString ON = "Green Duration";

				// Token: 0x0400CEF0 RID: 52976
				public static LocString GREEN_DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Green duration determines the amount of time this sensor should send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for {0}"
				});

				// Token: 0x0400CEF1 RID: 52977
				public static LocString OFF = "Red Duration";

				// Token: 0x0400CEF2 RID: 52978
				public static LocString RED_DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Red duration determines the amount of time this sensor should send a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"\n\nThis sensor will send a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" for {0}"
				});

				// Token: 0x0400CEF3 RID: 52979
				public static LocString CURRENT_TIME = "{0}/{1}";

				// Token: 0x0400CEF4 RID: 52980
				public static LocString MODE_LABEL_SECONDS = "Mode: Seconds";

				// Token: 0x0400CEF5 RID: 52981
				public static LocString MODE_LABEL_CYCLES = "Mode: Cycles";

				// Token: 0x0400CEF6 RID: 52982
				public static LocString RESET_BUTTON = "Reset Timer";

				// Token: 0x0400CEF7 RID: 52983
				public static LocString DISABLED = "Timer Disabled";
			}

			// Token: 0x02002FC9 RID: 12233
			public class COUNTER_SIDE_SCREEN
			{
				// Token: 0x0400CEF8 RID: 52984
				public static LocString TITLE = "Counter";

				// Token: 0x0400CEF9 RID: 52985
				public static LocString RESET_BUTTON = "Reset Counter";

				// Token: 0x0400CEFA RID: 52986
				public static LocString DESCRIPTION = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when count is reached:";

				// Token: 0x0400CEFB RID: 52987
				public static LocString INCREMENT_MODE = "Mode: Increment";

				// Token: 0x0400CEFC RID: 52988
				public static LocString DECREMENT_MODE = "Mode: Decrement";

				// Token: 0x0400CEFD RID: 52989
				public static LocString ADVANCED_MODE = "Advanced Mode";

				// Token: 0x0400CEFE RID: 52990
				public static LocString CURRENT_COUNT_SIMPLE = "{0} of ";

				// Token: 0x0400CEFF RID: 52991
				public static LocString CURRENT_COUNT_ADVANCED = "{0} % ";

				// Token: 0x02003C74 RID: 15476
				public class TOOLTIPS
				{
					// Token: 0x0400F039 RID: 61497
					public static LocString ADVANCED_MODE = string.Concat(new string[]
					{
						"In Advanced Mode, the ",
						BUILDINGS.PREFABS.LOGICCOUNTER.NAME,
						" will count from <b>0</b> rather than <b>1</b>. It will reset when the max is reached, and send a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						" as a brief pulse rather than continuously."
					});
				}
			}

			// Token: 0x02002FCA RID: 12234
			public class PILOT_AND_CREW_SIDESCREEN
			{
				// Token: 0x0400CF00 RID: 52992
				public static LocString TITLE = "Crew";

				// Token: 0x0400CF01 RID: 52993
				public static LocString INFO_LABEL = "Assigned Pilot: {0}";

				// Token: 0x0400CF02 RID: 52994
				public static LocString INFO_LABEL_ROBOT_ONLY = "Assigned Pilot: Robo-Pilot";

				// Token: 0x0400CF03 RID: 52995
				public static LocString NO_ASSIGNED_NAME = "None";

				// Token: 0x0400CF04 RID: 52996
				public static LocString EDIT_CREW_BUTTON_LABEL = "Select Crew";

				// Token: 0x0400CF05 RID: 52997
				public static LocString EDIT_CREW_BUTTON_TOOLTIP = "Assign Duplicants to crew this rocket";

				// Token: 0x0400CF06 RID: 52998
				public static LocString EDIT_CREW_BUTTON_DISABLED_TOOLTIP = string.Concat(new string[]
				{
					"Rockets must include a ",
					UI.PRE_KEYWORD,
					"Spacefarer",
					UI.PST_KEYWORD,
					" module in order for Duplicant crew to be assigned\n\nRockets with a ",
					UI.PRE_KEYWORD,
					"Robo-Pilot",
					UI.PST_KEYWORD,
					" module can launch without a Duplicant crew"
				});
			}

			// Token: 0x02002FCB RID: 12235
			public class SUMMON_CREW_SIDESCREEN
			{
				// Token: 0x0400CF07 RID: 52999
				public static LocString TITLE = "Access Permissions";

				// Token: 0x0400CF08 RID: 53000
				public static LocString INFO_LABEL_NO_CREW_NEEDED = "No crew required";

				// Token: 0x0400CF09 RID: 53001
				public static LocString INFO_LABEL_NO_CREW_FOUND = "No crew assigned";

				// Token: 0x0400CF0A RID: 53002
				public static LocString INFO_LABEL_PUBLIC_ACCESS = "Access: Public";

				// Token: 0x0400CF0B RID: 53003
				public static LocString INFO_LABEL_AWAITING_CREW = "Crew Boarded: {0} of {1}";

				// Token: 0x0400CF0C RID: 53004
				public static LocString INFO_LABEL_CREW_READY = "All crew members have boarded";

				// Token: 0x0400CF0D RID: 53005
				public static LocString INFO_LABEL_TOOLTIP_NO_CREW_NEEDED = string.Concat(new string[]
				{
					"This rocket is piloted by a ",
					UI.PRE_KEYWORD,
					"Robo-Pilot",
					UI.PST_KEYWORD,
					" module and does not require a Duplicant crew"
				});

				// Token: 0x0400CF0E RID: 53006
				public static LocString INFO_LABEL_TOOLTIP_NO_CREW_FOUND = string.Concat(new string[]
				{
					"This rocket must be piloted by a Duplicant with the ",
					UI.PRE_KEYWORD,
					"Rocket Piloting",
					UI.PST_KEYWORD,
					" skill\n\nRockets with a ",
					UI.PRE_KEYWORD,
					"Robo-Pilot",
					UI.PST_KEYWORD,
					" module do not require a Duplicant pilot"
				});

				// Token: 0x0400CF0F RID: 53007
				public static LocString INFO_LABEL_TOOLTIP_PUBLIC_ACCESS = "All Duplicants can access this rocket regardless of crew assignment\n\nRockets cannot launch in this mode";

				// Token: 0x0400CF10 RID: 53008
				public static LocString INFO_LABEL_TOOLTIP_AWAITING_CREW = "Crew boarding in progress\n\nNon-crew will disembark and can no longer board this rocket\n\nRockets cannot launch until all crew members have boarded";

				// Token: 0x0400CF11 RID: 53009
				public static LocString INFO_LABEL_TOOLTIP_CREW_READY = string.Concat(new string[]
				{
					"All crew members have boarded and are ready for this rocket to launch\n\nPress the ",
					UI.PRE_KEYWORD,
					"Begin Launch Sequence",
					UI.PST_KEYWORD,
					" button to launch this rocket"
				});

				// Token: 0x0400CF12 RID: 53010
				public static LocString SUMMON_CREW_BUTTON_LABEL = "Summon Crew";

				// Token: 0x0400CF13 RID: 53011
				public static LocString SUMMON_CREW_BUTTON_TOOLTIP = "Summon crew members to board this rocket in preparation for launch\n\nNon-crew will disembark";

				// Token: 0x0400CF14 RID: 53012
				public static LocString CANCEL_BUTTON_LABEL = "Set to Public Access";

				// Token: 0x0400CF15 RID: 53013
				public static LocString CANCEL_BUTTON_TOOLTIP = "Enable all Duplicants to access this rocket regardless of crew assignment\n\nRockets cannot launch in this mode";
			}

			// Token: 0x02002FCC RID: 12236
			public class TIMEDSWITCHSIDESCREEN
			{
				// Token: 0x0400CF16 RID: 53014
				public static LocString TITLE = "Time Schedule";

				// Token: 0x0400CF17 RID: 53015
				public static LocString ONTIME = "On Time:";

				// Token: 0x0400CF18 RID: 53016
				public static LocString OFFTIME = "Off Time:";

				// Token: 0x0400CF19 RID: 53017
				public static LocString TIMETODEACTIVATE = "Time until deactivation: {0}";

				// Token: 0x0400CF1A RID: 53018
				public static LocString TIMETOACTIVATE = "Time until activation: {0}";

				// Token: 0x0400CF1B RID: 53019
				public static LocString WARNING = "Switch must be connected to a " + UI.FormatAsLink("Power", "POWER") + " grid";

				// Token: 0x0400CF1C RID: 53020
				public static LocString CURRENTSTATE = "Current State:";

				// Token: 0x0400CF1D RID: 53021
				public static LocString ON = "On";

				// Token: 0x0400CF1E RID: 53022
				public static LocString OFF = "Off";
			}

			// Token: 0x02002FCD RID: 12237
			public class CAPTURE_POINT_SIDE_SCREEN
			{
				// Token: 0x0400CF1F RID: 53023
				public static LocString TITLE = "Stable Management";

				// Token: 0x0400CF20 RID: 53024
				public static LocString AUTOWRANGLE = "Auto-Wrangle Surplus";

				// Token: 0x0400CF21 RID: 53025
				public static LocString AUTOWRANGLE_TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant will automatically wrangle any critters that exceed the population limit or that do not belong in this stable\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill in order to wrangle critters"
				});

				// Token: 0x0400CF22 RID: 53026
				public static LocString LIMIT_TOOLTIP = "Critters exceeding this population limit will automatically be wrangled:";

				// Token: 0x0400CF23 RID: 53027
				public static LocString UNITS_SUFFIX = " Critters";
			}

			// Token: 0x02002FCE RID: 12238
			public class TEMPERATURESWITCHSIDESCREEN
			{
				// Token: 0x0400CF24 RID: 53028
				public static LocString TITLE = "Temperature Threshold";

				// Token: 0x0400CF25 RID: 53029
				public static LocString CURRENT_TEMPERATURE = "Current Temperature:\n{0}";

				// Token: 0x0400CF26 RID: 53030
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400CF27 RID: 53031
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400CF28 RID: 53032
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002FCF RID: 12239
			public class BRIGHTNESSSWITCHSIDESCREEN
			{
				// Token: 0x0400CF29 RID: 53033
				public static LocString TITLE = "Brightness Threshold";

				// Token: 0x0400CF2A RID: 53034
				public static LocString CURRENT_TEMPERATURE = "Current Brightness:\n{0}";

				// Token: 0x0400CF2B RID: 53035
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400CF2C RID: 53036
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400CF2D RID: 53037
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002FD0 RID: 12240
			public class RADIATIONSWITCHSIDESCREEN
			{
				// Token: 0x0400CF2E RID: 53038
				public static LocString TITLE = "Radiation Threshold";

				// Token: 0x0400CF2F RID: 53039
				public static LocString CURRENT_TEMPERATURE = "Current Radiation:\n{0}/cycle";

				// Token: 0x0400CF30 RID: 53040
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400CF31 RID: 53041
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400CF32 RID: 53042
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002FD1 RID: 12241
			public class WATTAGESWITCHSIDESCREEN
			{
				// Token: 0x0400CF33 RID: 53043
				public static LocString TITLE = "Wattage Threshold";

				// Token: 0x0400CF34 RID: 53044
				public static LocString CURRENT_TEMPERATURE = "Current Wattage:\n{0}";

				// Token: 0x0400CF35 RID: 53045
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400CF36 RID: 53046
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400CF37 RID: 53047
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002FD2 RID: 12242
			public class HEPSWITCHSIDESCREEN
			{
				// Token: 0x0400CF38 RID: 53048
				public static LocString TITLE = "Radbolt Threshold";
			}

			// Token: 0x02002FD3 RID: 12243
			public class THRESHOLD_SWITCH_SIDESCREEN
			{
				// Token: 0x0400CF39 RID: 53049
				public static LocString TITLE = "Pressure";

				// Token: 0x0400CF3A RID: 53050
				public static LocString THRESHOLD_SUBTITLE = "Threshold:";

				// Token: 0x0400CF3B RID: 53051
				public static LocString CURRENT_VALUE = "Current {0}:\n{1}";

				// Token: 0x0400CF3C RID: 53052
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400CF3D RID: 53053
				public static LocString ABOVE_BUTTON = "Above";

				// Token: 0x0400CF3E RID: 53054
				public static LocString BELOW_BUTTON = "Below";

				// Token: 0x0400CF3F RID: 53055
				public static LocString STATUS_ACTIVE = "Switch Active";

				// Token: 0x0400CF40 RID: 53056
				public static LocString STATUS_INACTIVE = "Switch Inactive";

				// Token: 0x0400CF41 RID: 53057
				public static LocString PRESSURE = "Ambient Pressure";

				// Token: 0x0400CF42 RID: 53058
				public static LocString PRESSURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400CF43 RID: 53059
				public static LocString PRESSURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400CF44 RID: 53060
				public static LocString TEMPERATURE = "Ambient Temperature";

				// Token: 0x0400CF45 RID: 53061
				public static LocString TEMPERATURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400CF46 RID: 53062
				public static LocString TEMPERATURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400CF47 RID: 53063
				public static LocString CONTENT_TEMPERATURE = "Internal Temperature";

				// Token: 0x0400CF48 RID: 53064
				public static LocString CONTENT_TEMPERATURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of its contents is above <b>{0}</b>"
				});

				// Token: 0x0400CF49 RID: 53065
				public static LocString CONTENT_TEMPERATURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of its contents is below <b>{0}</b>"
				});

				// Token: 0x0400CF4A RID: 53066
				public static LocString BRIGHTNESS = "Ambient Brightness";

				// Token: 0x0400CF4B RID: 53067
				public static LocString BRIGHTNESS_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Brightness",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400CF4C RID: 53068
				public static LocString BRIGHTNESS_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Brightness",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400CF4D RID: 53069
				public static LocString WATTAGE = "Wattage Reading";

				// Token: 0x0400CF4E RID: 53070
				public static LocString WATTAGE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Wattage",
					UI.PST_KEYWORD,
					" consumed is above <b>{0}</b>"
				});

				// Token: 0x0400CF4F RID: 53071
				public static LocString WATTAGE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Wattage",
					UI.PST_KEYWORD,
					" consumed is below <b>{0}</b>"
				});

				// Token: 0x0400CF50 RID: 53072
				public static LocString DISEASE_TITLE = "Germ Threshold";

				// Token: 0x0400CF51 RID: 53073
				public static LocString DISEASE = "Ambient Germs";

				// Token: 0x0400CF52 RID: 53074
				public static LocString DISEASE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the number of ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400CF53 RID: 53075
				public static LocString DISEASE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the number of ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400CF54 RID: 53076
				public static LocString DISEASE_UNITS = "";

				// Token: 0x0400CF55 RID: 53077
				public static LocString CONTENT_DISEASE = "Germ Count";

				// Token: 0x0400CF56 RID: 53078
				public static LocString RADIATION = "Ambient Radiation";

				// Token: 0x0400CF57 RID: 53079
				public static LocString RADIATION_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400CF58 RID: 53080
				public static LocString RADIATION_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400CF59 RID: 53081
				public static LocString HEPS = "Radbolt Reading";

				// Token: 0x0400CF5A RID: 53082
				public static LocString HEPS_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400CF5B RID: 53083
				public static LocString HEPS_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});
			}

			// Token: 0x02002FD4 RID: 12244
			public class CAPACITY_CONTROL_SIDE_SCREEN
			{
				// Token: 0x0400CF5C RID: 53084
				public static LocString TITLE = "Storage Capacity Control";

				// Token: 0x0400CF5D RID: 53085
				public static LocString MAX_LABEL = "Max:";
			}

			// Token: 0x02002FD5 RID: 12245
			public class DOOR_TOGGLE_SIDE_SCREEN
			{
				// Token: 0x0400CF5E RID: 53086
				public static LocString TITLE = "DOOR SETTINGS";

				// Token: 0x0400CF5F RID: 53087
				public static LocString OPEN = "Door is open to all permitted entities, as well as liquids and gases.\n";

				// Token: 0x0400CF60 RID: 53088
				public static LocString AUTO = "Door will auto-open according to access permissions.\n";

				// Token: 0x0400CF61 RID: 53089
				public static LocString CLOSE = "Door is locked. It must be unlocked before use.\n";

				// Token: 0x0400CF62 RID: 53090
				public static LocString PENDING_FORMAT = "{0} {1}";

				// Token: 0x0400CF63 RID: 53091
				public static LocString OPEN_PENDING = "Awaiting Duplicant to open door.";

				// Token: 0x0400CF64 RID: 53092
				public static LocString AUTO_PENDING = "Awaiting Duplicant to automate door.";

				// Token: 0x0400CF65 RID: 53093
				public static LocString CLOSE_PENDING = "Awaiting Duplicant to lock door.";

				// Token: 0x0400CF66 RID: 53094
				public static LocString ACCESS_FORMAT = "{0}\n\n{1}";

				// Token: 0x0400CF67 RID: 53095
				public static LocString ACCESS_OFFLINE = "Emergency Access Permissions:\nAll Duplicants are permitted to use this door until " + UI.FormatAsLink("Power", "POWER") + " is restored.";

				// Token: 0x0400CF68 RID: 53096
				public static LocString POI_INTERNAL = "This door cannot be manually controlled.";
			}

			// Token: 0x02002FD6 RID: 12246
			public class ACTIVATION_RANGE_SIDE_SCREEN
			{
				// Token: 0x0400CF69 RID: 53097
				public static LocString NAME = "Breaktime Policy";

				// Token: 0x0400CF6A RID: 53098
				public static LocString ACTIVATE = "Break starts at:";

				// Token: 0x0400CF6B RID: 53099
				public static LocString DEACTIVATE = "Break ends at:";
			}

			// Token: 0x02002FD7 RID: 12247
			public class CAPACITY_SIDE_SCREEN
			{
				// Token: 0x0400CF6C RID: 53100
				public static LocString TOOLTIP = "Adjust the maximum amount that can be stored here";
			}

			// Token: 0x02002FD8 RID: 12248
			public class SUIT_SIDE_SCREEN
			{
				// Token: 0x0400CF6D RID: 53101
				public static LocString TITLE = "Dock Inventory";

				// Token: 0x0400CF6E RID: 53102
				public static LocString CONFIGURATION_REQUIRED = "Configuration Required:";

				// Token: 0x0400CF6F RID: 53103
				public static LocString CONFIG_REQUEST_SUIT = "Deliver Suit";

				// Token: 0x0400CF70 RID: 53104
				public static LocString CONFIG_REQUEST_SUIT_TOOLTIP = "Duplicants will immediately deliver and dock the nearest unequipped suit";

				// Token: 0x0400CF71 RID: 53105
				public static LocString CONFIG_NO_SUIT = "Leave Empty";

				// Token: 0x0400CF72 RID: 53106
				public static LocString CONFIG_NO_SUIT_TOOLTIP = "The next suited Duplicant to pass by will unequip their suit and dock it here";

				// Token: 0x0400CF73 RID: 53107
				public static LocString CONFIG_CANCEL_REQUEST = "Cancel Request";

				// Token: 0x0400CF74 RID: 53108
				public static LocString CONFIG_CANCEL_REQUEST_TOOLTIP = "Cancel this suit delivery";

				// Token: 0x0400CF75 RID: 53109
				public static LocString CONFIG_DROP_SUIT = "Undock Suit";

				// Token: 0x0400CF76 RID: 53110
				public static LocString CONFIG_DROP_SUIT_TOOLTIP = "Disconnect this suit, dropping it on the ground";

				// Token: 0x0400CF77 RID: 53111
				public static LocString CONFIG_DROP_SUIT_NO_SUIT_TOOLTIP = "There is no suit in this building to undock";
			}

			// Token: 0x02002FD9 RID: 12249
			public class AUTOMATABLE_SIDE_SCREEN
			{
				// Token: 0x0400CF78 RID: 53112
				public static LocString TITLE = "Automatable Storage";

				// Token: 0x0400CF79 RID: 53113
				public static LocString ALLOWMANUALBUTTON = "Allow Manual Use";

				// Token: 0x0400CF7A RID: 53114
				public static LocString ALLOWMANUALBUTTONTOOLTIP = "Allow Duplicants to manually manage these storage materials";
			}

			// Token: 0x02002FDA RID: 12250
			public class STUDYABLE_SIDE_SCREEN
			{
				// Token: 0x0400CF7B RID: 53115
				public static LocString TITLE = "Analyze Natural Feature";

				// Token: 0x0400CF7C RID: 53116
				public static LocString STUDIED_STATUS = "Researchers have completed their analysis and compiled their findings.";

				// Token: 0x0400CF7D RID: 53117
				public static LocString STUDIED_BUTTON = "ANALYSIS COMPLETE";

				// Token: 0x0400CF7E RID: 53118
				public static LocString SEND_STATUS = "Send a researcher to gather data here.\n\nAnalyzing a feature takes time, but yields useful results.";

				// Token: 0x0400CF7F RID: 53119
				public static LocString SEND_BUTTON = "ANALYZE";

				// Token: 0x0400CF80 RID: 53120
				public static LocString PENDING_STATUS = "A researcher is in the process of studying this feature.";

				// Token: 0x0400CF81 RID: 53121
				public static LocString PENDING_BUTTON = "CANCEL ANALYSIS";
			}

			// Token: 0x02002FDB RID: 12251
			public class MEDICALCOTSIDESCREEN
			{
				// Token: 0x0400CF82 RID: 53122
				public static LocString TITLE = "Severity Requirement";

				// Token: 0x0400CF83 RID: 53123
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant may not use this cot until their ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" falls below <b>{0}%</b>"
				});
			}

			// Token: 0x02002FDC RID: 12252
			public class WARPPORTALSIDESCREEN
			{
				// Token: 0x0400CF84 RID: 53124
				public static LocString TITLE = "Teleporter";

				// Token: 0x0400CF85 RID: 53125
				public static LocString IDLE = "Teleporter online.\nPlease select a passenger:";

				// Token: 0x0400CF86 RID: 53126
				public static LocString WAITING = "Ready to transmit passenger.";

				// Token: 0x0400CF87 RID: 53127
				public static LocString COMPLETE = "Passenger transmitted!";

				// Token: 0x0400CF88 RID: 53128
				public static LocString UNDERWAY = "Transmitting passenger...";

				// Token: 0x0400CF89 RID: 53129
				public static LocString CONSUMED = "Teleporter recharging:";

				// Token: 0x0400CF8A RID: 53130
				public static LocString BUTTON = "Teleport!";

				// Token: 0x0400CF8B RID: 53131
				public static LocString CANCELBUTTON = "Cancel";
			}

			// Token: 0x02002FDD RID: 12253
			public class RADBOLTTHRESHOLDSIDESCREEN
			{
				// Token: 0x0400CF8C RID: 53132
				public static LocString TITLE = "Radbolt Threshold";

				// Token: 0x0400CF8D RID: 53133
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400CF8E RID: 53134
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Releases a ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" when stored Radbolts exceed <b>{0}</b>"
				});

				// Token: 0x0400CF8F RID: 53135
				public static LocString PROGRESS_BAR_LABEL = "Radbolt Generation";

				// Token: 0x0400CF90 RID: 53136
				public static LocString PROGRESS_BAR_TOOLTIP = string.Concat(new string[]
				{
					"The building will emit a ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" in the chosen direction when fully charged"
				});
			}

			// Token: 0x02002FDE RID: 12254
			public class LOGICBITSELECTORSIDESCREEN
			{
				// Token: 0x0400CF91 RID: 53137
				public static LocString RIBBON_READER_TITLE = "Ribbon Reader";

				// Token: 0x0400CF92 RID: 53138
				public static LocString RIBBON_READER_DESCRIPTION = "Selected <b>Bit's Signal</b> will be read by the <b>Output Port</b>";

				// Token: 0x0400CF93 RID: 53139
				public static LocString RIBBON_WRITER_TITLE = "Ribbon Writer";

				// Token: 0x0400CF94 RID: 53140
				public static LocString RIBBON_WRITER_DESCRIPTION = "Received <b>Signal</b> will be written to selected <b>Bit</b>";

				// Token: 0x0400CF95 RID: 53141
				public static LocString BIT = "Bit {0}";

				// Token: 0x0400CF96 RID: 53142
				public static LocString STATE_ACTIVE = "Green";

				// Token: 0x0400CF97 RID: 53143
				public static LocString STATE_INACTIVE = "Red";
			}

			// Token: 0x02002FDF RID: 12255
			public class LOGICALARMSIDESCREEN
			{
				// Token: 0x0400CF98 RID: 53144
				public static LocString TITLE = "Notification Designer";

				// Token: 0x0400CF99 RID: 53145
				public static LocString DESCRIPTION = "Notification will be sent upon receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + "\n\nMaking modifications will clear any existing notifications being sent by this building.";

				// Token: 0x0400CF9A RID: 53146
				public static LocString NAME = "<b>Name:</b>";

				// Token: 0x0400CF9B RID: 53147
				public static LocString NAME_DEFAULT = "Notification";

				// Token: 0x0400CF9C RID: 53148
				public static LocString TOOLTIP = "<b>Tooltip:</b>";

				// Token: 0x0400CF9D RID: 53149
				public static LocString TOOLTIP_DEFAULT = "Tooltip";

				// Token: 0x0400CF9E RID: 53150
				public static LocString TYPE = "<b>Type:</b>";

				// Token: 0x0400CF9F RID: 53151
				public static LocString PAUSE = "<b>Pause:</b>";

				// Token: 0x0400CFA0 RID: 53152
				public static LocString ZOOM = "<b>Zoom:</b>";

				// Token: 0x02003C75 RID: 15477
				public class TOOLTIPS
				{
					// Token: 0x0400F03A RID: 61498
					public static LocString NAME = "Select notification text";

					// Token: 0x0400F03B RID: 61499
					public static LocString TOOLTIP = "Select notification hover text";

					// Token: 0x0400F03C RID: 61500
					public static LocString TYPE = "Select the visual and aural style of the notification";

					// Token: 0x0400F03D RID: 61501
					public static LocString PAUSE = "Time will pause upon notification when checked";

					// Token: 0x0400F03E RID: 61502
					public static LocString ZOOM = "The view will zoom to this building upon notification when checked";

					// Token: 0x0400F03F RID: 61503
					public static LocString BAD = "\"Boing boing!\"";

					// Token: 0x0400F040 RID: 61504
					public static LocString NEUTRAL = "\"Pop!\"";

					// Token: 0x0400F041 RID: 61505
					public static LocString DUPLICANT_THREATENING = "AHH!";
				}
			}

			// Token: 0x02002FE0 RID: 12256
			public class GENETICANALYSISSIDESCREEN
			{
				// Token: 0x0400CFA1 RID: 53153
				public static LocString TITLE = "Genetic Analysis";

				// Token: 0x0400CFA2 RID: 53154
				public static LocString NONE_DISCOVERED = "No mutant seeds have been found.";

				// Token: 0x0400CFA3 RID: 53155
				public static LocString SELECT_SEEDS = "Select which seed types to analyze:";

				// Token: 0x0400CFA4 RID: 53156
				public static LocString SEED_NO_MUTANTS = "</i>No mutants found</i>";

				// Token: 0x0400CFA5 RID: 53157
				public static LocString SEED_FORBIDDEN = "</i>Won't analyze</i>";

				// Token: 0x0400CFA6 RID: 53158
				public static LocString SEED_ALLOWED = "</i>Will analyze</i>";
			}

			// Token: 0x02002FE1 RID: 12257
			public class RELATEDENTITIESSIDESCREEN
			{
				// Token: 0x0400CFA7 RID: 53159
				public static LocString TITLE = "Related Objects";
			}
		}

		// Token: 0x0200254A RID: 9546
		public class USERMENUACTIONS
		{
			// Token: 0x02002FE2 RID: 12258
			public class TINKER
			{
				// Token: 0x0400CFA8 RID: 53160
				public static LocString ALLOW = "Allow Tinker";

				// Token: 0x0400CFA9 RID: 53161
				public static LocString DISALLOW = "Disallow Tinker";

				// Token: 0x0400CFAA RID: 53162
				public static LocString TOOLTIP_DISALLOW = "Disallow {0} on this {1}";

				// Token: 0x0400CFAB RID: 53163
				public static LocString TOOLTIP_ALLOW = "Allow  {0} on this {1}";
			}

			// Token: 0x02002FE3 RID: 12259
			public class TRANSITTUBEWAX
			{
				// Token: 0x0400CFAC RID: 53164
				public static LocString NAME = "Enable Smooth Ride";

				// Token: 0x0400CFAD RID: 53165
				public static LocString TOOLTIP = "Enables the use of " + ELEMENTS.MILKFAT.NAME + " to boost travel speed";
			}

			// Token: 0x02002FE4 RID: 12260
			public class CANCELTRANSITTUBEWAX
			{
				// Token: 0x0400CFAE RID: 53166
				public static LocString NAME = "Disable Smooth Ride";

				// Token: 0x0400CFAF RID: 53167
				public static LocString TOOLTIP = "Disables travel speed boost and refunds stored " + ELEMENTS.MILKFAT.NAME;
			}

			// Token: 0x02002FE5 RID: 12261
			public class CLEANTOILET
			{
				// Token: 0x0400CFB0 RID: 53168
				public static LocString NAME = "Clean Toilet";

				// Token: 0x0400CFB1 RID: 53169
				public static LocString TOOLTIP = "Empty waste from this toilet";
			}

			// Token: 0x02002FE6 RID: 12262
			public class CANCELCLEANTOILET
			{
				// Token: 0x0400CFB2 RID: 53170
				public static LocString NAME = "Cancel Clean";

				// Token: 0x0400CFB3 RID: 53171
				public static LocString TOOLTIP = "Cancel this cleaning order";
			}

			// Token: 0x02002FE7 RID: 12263
			public class EMPTYBEEHIVE
			{
				// Token: 0x0400CFB4 RID: 53172
				public static LocString NAME = "Enable Autoharvest";

				// Token: 0x0400CFB5 RID: 53173
				public static LocString TOOLTIP = "Automatically harvest this hive when full";
			}

			// Token: 0x02002FE8 RID: 12264
			public class CANCELEMPTYBEEHIVE
			{
				// Token: 0x0400CFB6 RID: 53174
				public static LocString NAME = "Disable Autoharvest";

				// Token: 0x0400CFB7 RID: 53175
				public static LocString TOOLTIP = "Do not automatically harvest this hive";
			}

			// Token: 0x02002FE9 RID: 12265
			public class EMPTYDESALINATOR
			{
				// Token: 0x0400CFB8 RID: 53176
				public static LocString NAME = "Empty Desalinator";

				// Token: 0x0400CFB9 RID: 53177
				public static LocString TOOLTIP = "Empty salt from this desalinator";
			}

			// Token: 0x02002FEA RID: 12266
			public class CHANGE_ROOM
			{
				// Token: 0x0400CFBA RID: 53178
				public static LocString REQUEST_OUTFIT = "Request Outfit";

				// Token: 0x0400CFBB RID: 53179
				public static LocString REQUEST_OUTFIT_TOOLTIP = "Request outfit to be delivered to this change room";

				// Token: 0x0400CFBC RID: 53180
				public static LocString CANCEL_REQUEST = "Cancel Request";

				// Token: 0x0400CFBD RID: 53181
				public static LocString CANCEL_REQUEST_TOOLTIP = "Cancel outfit request";

				// Token: 0x0400CFBE RID: 53182
				public static LocString DROP_OUTFIT = "Drop Outfit";

				// Token: 0x0400CFBF RID: 53183
				public static LocString DROP_OUTFIT_TOOLTIP = "Drop outfit on floor";
			}

			// Token: 0x02002FEB RID: 12267
			public class DUMP
			{
				// Token: 0x0400CFC0 RID: 53184
				public static LocString NAME = "Empty";

				// Token: 0x0400CFC1 RID: 53185
				public static LocString TOOLTIP = "Dump bottle contents onto the floor";

				// Token: 0x0400CFC2 RID: 53186
				public static LocString NAME_OFF = "Cancel Empty";

				// Token: 0x0400CFC3 RID: 53187
				public static LocString TOOLTIP_OFF = "Cancel this empty order";
			}

			// Token: 0x02002FEC RID: 12268
			public class TAGFILTER
			{
				// Token: 0x0400CFC4 RID: 53188
				public static LocString NAME = "Filter Settings";

				// Token: 0x0400CFC5 RID: 53189
				public static LocString TOOLTIP = "Assign materials to storage";
			}

			// Token: 0x02002FED RID: 12269
			public class CANCELCONSTRUCTION
			{
				// Token: 0x0400CFC6 RID: 53190
				public static LocString NAME = "Cancel Build";

				// Token: 0x0400CFC7 RID: 53191
				public static LocString TOOLTIP = "Cancel this build order";
			}

			// Token: 0x02002FEE RID: 12270
			public class DIG
			{
				// Token: 0x0400CFC8 RID: 53192
				public static LocString NAME = "Dig";

				// Token: 0x0400CFC9 RID: 53193
				public static LocString TOOLTIP = "Dig out this cell";

				// Token: 0x0400CFCA RID: 53194
				public static LocString TOOLTIP_OFF = "Cancel this dig order";
			}

			// Token: 0x02002FEF RID: 12271
			public class CANCELMOP
			{
				// Token: 0x0400CFCB RID: 53195
				public static LocString NAME = "Cancel Mop";

				// Token: 0x0400CFCC RID: 53196
				public static LocString TOOLTIP = "Cancel this mop order";
			}

			// Token: 0x02002FF0 RID: 12272
			public class CANCELDIG
			{
				// Token: 0x0400CFCD RID: 53197
				public static LocString NAME = "Cancel Dig";

				// Token: 0x0400CFCE RID: 53198
				public static LocString TOOLTIP = "Cancel this dig order";
			}

			// Token: 0x02002FF1 RID: 12273
			public class UPROOT
			{
				// Token: 0x0400CFCF RID: 53199
				public static LocString NAME = "Uproot";

				// Token: 0x0400CFD0 RID: 53200
				public static LocString TOOLTIP = "Convert this plant into a seed";
			}

			// Token: 0x02002FF2 RID: 12274
			public class CANCELUPROOT
			{
				// Token: 0x0400CFD1 RID: 53201
				public static LocString NAME = "Cancel Uproot";

				// Token: 0x0400CFD2 RID: 53202
				public static LocString TOOLTIP = "Cancel this uproot order";
			}

			// Token: 0x02002FF3 RID: 12275
			public class HARVEST_WHEN_READY
			{
				// Token: 0x0400CFD3 RID: 53203
				public static LocString NAME = "Enable Autoharvest";

				// Token: 0x0400CFD4 RID: 53204
				public static LocString TOOLTIP = "Automatically harvest this plant when it matures";
			}

			// Token: 0x02002FF4 RID: 12276
			public class CANCEL_HARVEST_WHEN_READY
			{
				// Token: 0x0400CFD5 RID: 53205
				public static LocString NAME = "Disable Autoharvest";

				// Token: 0x0400CFD6 RID: 53206
				public static LocString TOOLTIP = "Do not automatically harvest this plant";
			}

			// Token: 0x02002FF5 RID: 12277
			public class HARVEST
			{
				// Token: 0x0400CFD7 RID: 53207
				public static LocString NAME = "Harvest";

				// Token: 0x0400CFD8 RID: 53208
				public static LocString TOOLTIP = "Harvest materials from this plant";

				// Token: 0x0400CFD9 RID: 53209
				public static LocString TOOLTIP_DISABLED = "This plant has nothing to harvest";
			}

			// Token: 0x02002FF6 RID: 12278
			public class CANCELHARVEST
			{
				// Token: 0x0400CFDA RID: 53210
				public static LocString NAME = "Cancel Harvest";

				// Token: 0x0400CFDB RID: 53211
				public static LocString TOOLTIP = "Cancel this harvest order";
			}

			// Token: 0x02002FF7 RID: 12279
			public class ATTACK
			{
				// Token: 0x0400CFDC RID: 53212
				public static LocString NAME = "Attack";

				// Token: 0x0400CFDD RID: 53213
				public static LocString TOOLTIP = "Attack this critter";
			}

			// Token: 0x02002FF8 RID: 12280
			public class CANCELATTACK
			{
				// Token: 0x0400CFDE RID: 53214
				public static LocString NAME = "Cancel Attack";

				// Token: 0x0400CFDF RID: 53215
				public static LocString TOOLTIP = "Cancel this attack order";
			}

			// Token: 0x02002FF9 RID: 12281
			public class CAPTURE
			{
				// Token: 0x0400CFE0 RID: 53216
				public static LocString NAME = "Wrangle";

				// Token: 0x0400CFE1 RID: 53217
				public static LocString TOOLTIP = "Capture this critter alive";
			}

			// Token: 0x02002FFA RID: 12282
			public class CANCELCAPTURE
			{
				// Token: 0x0400CFE2 RID: 53218
				public static LocString NAME = "Cancel Wrangle";

				// Token: 0x0400CFE3 RID: 53219
				public static LocString TOOLTIP = "Cancel this wrangle order";
			}

			// Token: 0x02002FFB RID: 12283
			public class RELEASEELEMENT
			{
				// Token: 0x0400CFE4 RID: 53220
				public static LocString NAME = "Empty Building";

				// Token: 0x0400CFE5 RID: 53221
				public static LocString TOOLTIP = "Refund all resources currently in use by this building";
			}

			// Token: 0x02002FFC RID: 12284
			public class DECONSTRUCT
			{
				// Token: 0x0400CFE6 RID: 53222
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400CFE7 RID: 53223
				public static LocString TOOLTIP = "Deconstruct this building and refund all resources";

				// Token: 0x0400CFE8 RID: 53224
				public static LocString NAME_OFF = "Cancel Deconstruct";

				// Token: 0x0400CFE9 RID: 53225
				public static LocString TOOLTIP_OFF = "Cancel this deconstruct order";
			}

			// Token: 0x02002FFD RID: 12285
			public class DEMOLISH
			{
				// Token: 0x0400CFEA RID: 53226
				public static LocString NAME = "Demolish";

				// Token: 0x0400CFEB RID: 53227
				public static LocString TOOLTIP = "Demolish this building";

				// Token: 0x0400CFEC RID: 53228
				public static LocString NAME_OFF = "Cancel Demolition";

				// Token: 0x0400CFED RID: 53229
				public static LocString TOOLTIP_OFF = "Cancel this demolition order";
			}

			// Token: 0x02002FFE RID: 12286
			public class ROCKETUSAGERESTRICTION
			{
				// Token: 0x0400CFEE RID: 53230
				public static LocString NAME_UNCONTROLLED = "Uncontrolled";

				// Token: 0x0400CFEF RID: 53231
				public static LocString TOOLTIP_UNCONTROLLED = "Do not allow this building to be controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;

				// Token: 0x0400CFF0 RID: 53232
				public static LocString NAME_CONTROLLED = "Controlled";

				// Token: 0x0400CFF1 RID: 53233
				public static LocString TOOLTIP_CONTROLLED = "Allow this building's operation to be controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02002FFF RID: 12287
			public class MANUAL_DELIVERY
			{
				// Token: 0x0400CFF2 RID: 53234
				public static LocString NAME = "Disable Delivery";

				// Token: 0x0400CFF3 RID: 53235
				public static LocString TOOLTIP = "Do not deliver materials to this building";

				// Token: 0x0400CFF4 RID: 53236
				public static LocString NAME_OFF = "Enable Delivery";

				// Token: 0x0400CFF5 RID: 53237
				public static LocString TOOLTIP_OFF = "Deliver materials to this building";
			}

			// Token: 0x02003000 RID: 12288
			public class SELECTRESEARCH
			{
				// Token: 0x0400CFF6 RID: 53238
				public static LocString NAME = "Select Research";

				// Token: 0x0400CFF7 RID: 53239
				public static LocString TOOLTIP = "Choose a technology from the " + UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch);
			}

			// Token: 0x02003001 RID: 12289
			public class RECONSTRUCT
			{
				// Token: 0x0400CFF8 RID: 53240
				public static LocString REQUEST_RECONSTRUCT = "Order Rebuild";

				// Token: 0x0400CFF9 RID: 53241
				public static LocString REQUEST_RECONSTRUCT_TOOLTIP = "Deconstruct this building and rebuild it using the selected material";

				// Token: 0x0400CFFA RID: 53242
				public static LocString CANCEL_RECONSTRUCT = "Cancel Rebuild Order";

				// Token: 0x0400CFFB RID: 53243
				public static LocString CANCEL_RECONSTRUCT_TOOLTIP = "Cancel deconstruction and rebuilding of this building";
			}

			// Token: 0x02003002 RID: 12290
			public class RELOCATE
			{
				// Token: 0x0400CFFC RID: 53244
				public static LocString NAME = "Relocate";

				// Token: 0x0400CFFD RID: 53245
				public static LocString TOOLTIP = "Move this building to a new location\n\nCosts no additional resources";

				// Token: 0x0400CFFE RID: 53246
				public static LocString NAME_OFF = "Cancel Relocation";

				// Token: 0x0400CFFF RID: 53247
				public static LocString TOOLTIP_OFF = "Cancel this relocation order";
			}

			// Token: 0x02003003 RID: 12291
			public class ENABLEBUILDING
			{
				// Token: 0x0400D000 RID: 53248
				public static LocString NAME = "Disable Building";

				// Token: 0x0400D001 RID: 53249
				public static LocString TOOLTIP = "Halt the use of this building {Hotkey}\n\nDisabled buildings consume no energy or resources";

				// Token: 0x0400D002 RID: 53250
				public static LocString NAME_OFF = "Enable Building";

				// Token: 0x0400D003 RID: 53251
				public static LocString TOOLTIP_OFF = "Resume the use of this building {Hotkey}";
			}

			// Token: 0x02003004 RID: 12292
			public class READLORE
			{
				// Token: 0x0400D004 RID: 53252
				public static LocString NAME = "Inspect";

				// Token: 0x0400D005 RID: 53253
				public static LocString ALREADYINSPECTED = "Already inspected";

				// Token: 0x0400D006 RID: 53254
				public static LocString TOOLTIP = "Recover files from this structure";

				// Token: 0x0400D007 RID: 53255
				public static LocString TOOLTIP_ALREADYINSPECTED = "This structure has already been inspected";

				// Token: 0x0400D008 RID: 53256
				public static LocString GOTODATABASE = "View Entry";

				// Token: 0x0400D009 RID: 53257
				public static LocString SEARCH_DISPLAY = "The display is still functional. I copy its message into my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400D00A RID: 53258
				public static LocString SEARCH_ELLIESDESK = "All I find on the machine is a curt e-mail from a disgruntled employee.\n\nNew Database Entry discovered.";

				// Token: 0x0400D00B RID: 53259
				public static LocString SEARCH_POD = "I search my incoming message history and find a single entry. I move the odd message into my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400D00C RID: 53260
				public static LocString ALREADY_SEARCHED = "I already took everything of interest from this. I can check the Database to re-read what I found.";

				// Token: 0x0400D00D RID: 53261
				public static LocString SEARCH_CABINET = "One intact document remains - an old yellowing newspaper clipping. It won't be of much use, but I add it to my database nonetheless.\n\nNew Database Entry discovered.";

				// Token: 0x0400D00E RID: 53262
				public static LocString SEARCH_STERNSDESK = "There's an old magazine article from a publication called the \"Nucleoid\" tucked in the top drawer. I add it to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400D00F RID: 53263
				public static LocString ALREADY_SEARCHED_STERNSDESK = "The desk is eerily empty inside.";

				// Token: 0x0400D010 RID: 53264
				public static LocString SEARCH_TELEPORTER_SENDER = "While scanning the antiquated computer code of this machine I uncovered some research notes. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400D011 RID: 53265
				public static LocString SEARCH_TELEPORTER_RECEIVER = "Incongruously placed research notes are hidden within the operating instructions of this device. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400D012 RID: 53266
				public static LocString SEARCH_CRYO_TANK = "There are some safety instructions included in the operating instructions of this Cryotank. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400D013 RID: 53267
				public static LocString SEARCH_PROPGRAVITASCREATUREPOSTER = "There's a handwritten note taped to the back of this poster. I add it to my database.\n\nNew Database Entry discovered.";

				// Token: 0x02003C76 RID: 15478
				public class SEARCH_COMPUTER_PODIUM
				{
					// Token: 0x0400F042 RID: 61506
					public static LocString SEARCH1 = "I search through the computer's database and find an unredacted e-mail.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02003C77 RID: 15479
				public class SEARCH_COMPUTER_SUCCESS
				{
					// Token: 0x0400F043 RID: 61507
					public static LocString SEARCH1 = "After searching through the computer's database, I managed to piece together some files that piqued my interest.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F044 RID: 61508
					public static LocString SEARCH2 = "Searching through the computer, I find some recoverable files that are still readable.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F045 RID: 61509
					public static LocString SEARCH3 = "The computer looks pristine on the outside, but is corrupted internally. Still, I managed to find one uncorrupted file, and have added it to my database.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F046 RID: 61510
					public static LocString SEARCH4 = "The computer was wiped almost completely clean, except for one file hidden in the recycle bin.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F047 RID: 61511
					public static LocString SEARCH5 = "I search the computer, storing what useful data I can find in my own memory.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F048 RID: 61512
					public static LocString SEARCH6 = "This computer is broken and requires some finessing to get working. Still, I recover a handful of interesting files.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02003C78 RID: 15480
				public class SEARCH_COMPUTER_FAIL
				{
					// Token: 0x0400F049 RID: 61513
					public static LocString SEARCH1 = "Unfortunately, the computer's hard drive is irreparably corrupted.";

					// Token: 0x0400F04A RID: 61514
					public static LocString SEARCH2 = "The computer was wiped clean before I got here. There is nothing to recover.";

					// Token: 0x0400F04B RID: 61515
					public static LocString SEARCH3 = "Some intact files are available on the computer, but nothing I haven't already discovered elsewhere. I find nothing else.";

					// Token: 0x0400F04C RID: 61516
					public static LocString SEARCH4 = "The computer has nothing of import.";

					// Token: 0x0400F04D RID: 61517
					public static LocString SEARCH5 = "Someone's left a solitaire game up. There's nothing else of interest on the computer.\n\nAlso, it looks as though they were about to lose.";

					// Token: 0x0400F04E RID: 61518
					public static LocString SEARCH6 = "The background on this computer depicts two kittens hugging in a field of daisies. There is nothing else of import to be found.";

					// Token: 0x0400F04F RID: 61519
					public static LocString SEARCH7 = "The user alphabetized the shortcuts on their desktop. There is nothing else of import to be found.";

					// Token: 0x0400F050 RID: 61520
					public static LocString SEARCH8 = "The background is a picture of a golden retriever in a science lab. It looks very confused. There is nothing else of import to be found.";

					// Token: 0x0400F051 RID: 61521
					public static LocString SEARCH9 = "This user never changed their default background. There is nothing else of import to be found. How dull.";
				}

				// Token: 0x02003C79 RID: 15481
				public class SEARCH_TECHNOLOGY_SUCCESS
				{
					// Token: 0x0400F052 RID: 61522
					public static LocString SEARCH1 = "I scour the internal systems and find something of interest.\n\nNew Database Entry discovered.";

					// Token: 0x0400F053 RID: 61523
					public static LocString SEARCH2 = "I see if I can salvage anything from the electronics. I add what I find to my database.\n\nNew Database Entry discovered.";

					// Token: 0x0400F054 RID: 61524
					public static LocString SEARCH3 = "I look for anything of interest within the abandoned machinery and add what I find to my database.\n\nNew Database Entry discovered.";
				}

				// Token: 0x02003C7A RID: 15482
				public class SEARCH_OBJECT_SUCCESS
				{
					// Token: 0x0400F055 RID: 61525
					public static LocString SEARCH1 = "I look around and recover an old file.\n\nNew Database Entry discovered.";

					// Token: 0x0400F056 RID: 61526
					public static LocString SEARCH2 = "There's a three-ringed binder inside. I scan the surviving documents.\n\nNew Database Entry discovered.";

					// Token: 0x0400F057 RID: 61527
					public static LocString SEARCH3 = "A discarded journal inside remains mostly intact. I scan the pages of use.\n\nNew Database Entry discovered.";

					// Token: 0x0400F058 RID: 61528
					public static LocString SEARCH4 = "A single page of a long printout remains legible. I scan it and add it to my database.\n\nNew Database Entry discovered.";

					// Token: 0x0400F059 RID: 61529
					public static LocString SEARCH5 = "A few loose papers can be found inside. I scan the ones that look interesting.\n\nNew Database Entry discovered.";

					// Token: 0x0400F05A RID: 61530
					public static LocString SEARCH6 = "I find a memory stick inside and copy its data into my database.\n\nNew Database Entry discovered.";
				}

				// Token: 0x02003C7B RID: 15483
				public class SEARCH_OBJECT_FAIL
				{
					// Token: 0x0400F05B RID: 61531
					public static LocString SEARCH1 = "I look around but find nothing of interest.";
				}

				// Token: 0x02003C7C RID: 15484
				public class SEARCH_SPACEPOI_SUCCESS
				{
					// Token: 0x0400F05C RID: 61532
					public static LocString SEARCH1 = "A quick analysis of the hardware of this debris has uncovered some searchable files within.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F05D RID: 61533
					public static LocString SEARCH2 = "There's an archaic interface I can interact with on this device.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F05E RID: 61534
					public static LocString SEARCH3 = "While investigating the software of this wreckage, a compelling file comes to my attention.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F05F RID: 61535
					public static LocString SEARCH4 = "Not much remains of the software that once ran this spacecraft except for one file that piques my interest.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F060 RID: 61536
					public static LocString SEARCH5 = "I find some noteworthy data hidden amongst the system files of this space junk.\n\nNew Database Entry unlocked.";

					// Token: 0x0400F061 RID: 61537
					public static LocString SEARCH6 = "Despite being subjected to years of degradation, there are still recoverable files in this machinery.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02003C7D RID: 15485
				public class SEARCH_SPACEPOI_FAIL
				{
					// Token: 0x0400F062 RID: 61538
					public static LocString SEARCH1 = "There's nothing of interest left in this old space junk.";

					// Token: 0x0400F063 RID: 61539
					public static LocString SEARCH2 = "I've salvaged everything I can from this vehicle.";

					// Token: 0x0400F064 RID: 61540
					public static LocString SEARCH3 = "Years of neglect and radioactive decay have destroyed all the useful data from this derelict spacecraft.";
				}

				// Token: 0x02003C7E RID: 15486
				public class SEARCH_DISPLAY_FAIL
				{
					// Token: 0x0400F065 RID: 61541
					public static LocString SEARCH1 = "The display is frozen. Whatever information it once contained is long gone.";
				}

				// Token: 0x02003C7F RID: 15487
				public class SEARCH_MIRROR_SUCCESS
				{
					// Token: 0x0400F066 RID: 61542
					public static LocString SEARCH1 = "I look behind the mirror and find a recording device taped to the back.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02003C80 RID: 15488
				public class SEARCH_MIRROR_FAIL
				{
					// Token: 0x0400F067 RID: 61543
					public static LocString SEARCH1 = "There's nothing to see here but the streaks left by a distracted cleaner.";
				}
			}

			// Token: 0x02003005 RID: 12293
			public class OPENPOI
			{
				// Token: 0x0400D014 RID: 53268
				public static LocString NAME = "Rummage";

				// Token: 0x0400D015 RID: 53269
				public static LocString TOOLTIP = "Scrounge for usable materials";

				// Token: 0x0400D016 RID: 53270
				public static LocString NAME_OFF = "Cancel Rummage";

				// Token: 0x0400D017 RID: 53271
				public static LocString TOOLTIP_OFF = "Cancel this rummage order";

				// Token: 0x0400D018 RID: 53272
				public static LocString ALREADY_RUMMAGED = "Already Rummaged";

				// Token: 0x0400D019 RID: 53273
				public static LocString TOOLTIP_ALREADYRUMMAGED = "There are no usable materials left to find";
			}

			// Token: 0x02003006 RID: 12294
			public class OPEN_TECHUNLOCKS
			{
				// Token: 0x0400D01A RID: 53274
				public static LocString NAME = "Unlock Portal";

				// Token: 0x0400D01B RID: 53275
				public static LocString TOOLTIP = "Retrieve data stored in this building";

				// Token: 0x0400D01C RID: 53276
				public static LocString NAME_OFF = "Cancel Unlock Portal";

				// Token: 0x0400D01D RID: 53277
				public static LocString TOOLTIP_OFF = "Cancel this portal access order";

				// Token: 0x0400D01E RID: 53278
				public static LocString ALREADY_RUMMAGED = "Already Unlocked";

				// Token: 0x0400D01F RID: 53279
				public static LocString TOOLTIP_ALREADYRUMMAGED = "All data has been accessed and recorded";
			}

			// Token: 0x02003007 RID: 12295
			public class EMPTYSTORAGE
			{
				// Token: 0x0400D020 RID: 53280
				public static LocString NAME = "Empty Storage";

				// Token: 0x0400D021 RID: 53281
				public static LocString TOOLTIP = "Eject all resources from this container";

				// Token: 0x0400D022 RID: 53282
				public static LocString NAME_OFF = "Cancel Empty";

				// Token: 0x0400D023 RID: 53283
				public static LocString TOOLTIP_OFF = "Cancel this empty order";
			}

			// Token: 0x02003008 RID: 12296
			public class CLOSESTORAGE
			{
				// Token: 0x0400D024 RID: 53284
				public static LocString NAME = "Close Storage";

				// Token: 0x0400D025 RID: 53285
				public static LocString TOOLTIP = "Prevent this container from receiving resources for storage";

				// Token: 0x0400D026 RID: 53286
				public static LocString NAME_OFF = "Cancel Close";

				// Token: 0x0400D027 RID: 53287
				public static LocString TOOLTIP_OFF = "Cancel this close order";
			}

			// Token: 0x02003009 RID: 12297
			public class COPY_BUILDING_SETTINGS
			{
				// Token: 0x0400D028 RID: 53288
				public static LocString NAME = "Copy Settings";

				// Token: 0x0400D029 RID: 53289
				public static LocString TOOLTIP = "Apply the settings and priorities of this building to other buildings of the same type {Hotkey}";
			}

			// Token: 0x0200300A RID: 12298
			public class CLEAR
			{
				// Token: 0x0400D02A RID: 53290
				public static LocString NAME = "Sweep";

				// Token: 0x0400D02B RID: 53291
				public static LocString TOOLTIP = "Put this object away in the nearest storage container";

				// Token: 0x0400D02C RID: 53292
				public static LocString NAME_OFF = "Cancel Sweeping";

				// Token: 0x0400D02D RID: 53293
				public static LocString TOOLTIP_OFF = "Cancel this sweep order";
			}

			// Token: 0x0200300B RID: 12299
			public class COMPOST
			{
				// Token: 0x0400D02E RID: 53294
				public static LocString NAME = "Compost";

				// Token: 0x0400D02F RID: 53295
				public static LocString TOOLTIP = "Mark this object for compost";

				// Token: 0x0400D030 RID: 53296
				public static LocString NAME_OFF = "Cancel Compost";

				// Token: 0x0400D031 RID: 53297
				public static LocString TOOLTIP_OFF = "Cancel this compost order";
			}

			// Token: 0x0200300C RID: 12300
			public class PICKUPABLEMOVE
			{
				// Token: 0x0400D032 RID: 53298
				public static LocString NAME = "Relocate To";

				// Token: 0x0400D033 RID: 53299
				public static LocString TOOLTIP = "Relocate this object to a specific location";

				// Token: 0x0400D034 RID: 53300
				public static LocString NAME_OFF = "Cancel Relocate";

				// Token: 0x0400D035 RID: 53301
				public static LocString TOOLTIP_OFF = "Cancel order to relocate this object";
			}

			// Token: 0x0200300D RID: 12301
			public class UNEQUIP
			{
				// Token: 0x0400D036 RID: 53302
				public static LocString NAME = "Unequip {0}";

				// Token: 0x0400D037 RID: 53303
				public static LocString TOOLTIP = "Take off and drop this equipment";
			}

			// Token: 0x0200300E RID: 12302
			public class QUARANTINE
			{
				// Token: 0x0400D038 RID: 53304
				public static LocString NAME = "Quarantine";

				// Token: 0x0400D039 RID: 53305
				public static LocString TOOLTIP = "Isolate this Duplicant\nThe Duplicant will return to their assigned Cot";

				// Token: 0x0400D03A RID: 53306
				public static LocString TOOLTIP_DISABLED = "No quarantine zone assigned";

				// Token: 0x0400D03B RID: 53307
				public static LocString NAME_OFF = "Cancel Quarantine";

				// Token: 0x0400D03C RID: 53308
				public static LocString TOOLTIP_OFF = "Cancel this quarantine order";
			}

			// Token: 0x0200300F RID: 12303
			public class DRAWPATHS
			{
				// Token: 0x0400D03D RID: 53309
				public static LocString NAME = "Show Navigation";

				// Token: 0x0400D03E RID: 53310
				public static LocString TOOLTIP = "Show all areas within this Duplicant's reach";

				// Token: 0x0400D03F RID: 53311
				public static LocString NAME_OFF = "Hide Navigation";

				// Token: 0x0400D040 RID: 53312
				public static LocString TOOLTIP_OFF = "Hide areas within this Duplicant's reach";
			}

			// Token: 0x02003010 RID: 12304
			public class MOVETOLOCATION
			{
				// Token: 0x0400D041 RID: 53313
				public static LocString NAME = "Move To";

				// Token: 0x0400D042 RID: 53314
				public static LocString TOOLTIP = "Move this Duplicant to a specific location";
			}

			// Token: 0x02003011 RID: 12305
			public class FOLLOWCAM
			{
				// Token: 0x0400D043 RID: 53315
				public static LocString NAME = "Follow Cam";

				// Token: 0x0400D044 RID: 53316
				public static LocString TOOLTIP = "Track this Duplicant with the camera";
			}

			// Token: 0x02003012 RID: 12306
			public class WORKABLE_DIRECTION_BOTH
			{
				// Token: 0x0400D045 RID: 53317
				public static LocString NAME = "Set Direction: Both";

				// Token: 0x0400D046 RID: 53318
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by in either direction";
			}

			// Token: 0x02003013 RID: 12307
			public class WORKABLE_DIRECTION_LEFT
			{
				// Token: 0x0400D047 RID: 53319
				public static LocString NAME = "Set Direction: Left";

				// Token: 0x0400D048 RID: 53320
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by from right to left";
			}

			// Token: 0x02003014 RID: 12308
			public class WORKABLE_DIRECTION_RIGHT
			{
				// Token: 0x0400D049 RID: 53321
				public static LocString NAME = "Set Direction: Right";

				// Token: 0x0400D04A RID: 53322
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by from left to right";
			}

			// Token: 0x02003015 RID: 12309
			public class MANUAL_PUMP_DELIVERY
			{
				// Token: 0x02003C81 RID: 15489
				public static class ALLOWED
				{
					// Token: 0x0400F068 RID: 61544
					public static LocString NAME = "Enable Auto-Bottle";

					// Token: 0x0400F069 RID: 61545
					public static LocString TOOLTIP = "If enabled, Duplicants will deliver bottled liquids to this building directly from these sources:\n";

					// Token: 0x0400F06A RID: 61546
					public static LocString ITEM = "\n{0}";
				}

				// Token: 0x02003C82 RID: 15490
				public static class DENIED
				{
					// Token: 0x0400F06B RID: 61547
					public static LocString NAME = "Disable Auto-Bottle";

					// Token: 0x0400F06C RID: 61548
					public static LocString TOOLTIP = "If disabled, Duplicants will no longer deliver bottled liquids directly from Pitcher Pumps";
				}

				// Token: 0x02003C83 RID: 15491
				public static class ALLOWED_GAS
				{
					// Token: 0x0400F06D RID: 61549
					public static LocString NAME = "Enable Auto-Bottle";

					// Token: 0x0400F06E RID: 61550
					public static LocString TOOLTIP = "If enabled, Duplicants will deliver gas canisters to this building directly from Canister Fillers";
				}

				// Token: 0x02003C84 RID: 15492
				public static class DENIED_GAS
				{
					// Token: 0x0400F06F RID: 61551
					public static LocString NAME = "Disable Auto-Bottle";

					// Token: 0x0400F070 RID: 61552
					public static LocString TOOLTIP = "If disabled, Duplicants will no longer deliver gas canisters directly from Canister Fillers";
				}
			}

			// Token: 0x02003016 RID: 12310
			public class SUIT_MARKER_TRAVERSAL
			{
				// Token: 0x02003C85 RID: 15493
				public static class ONLY_WHEN_ROOM_AVAILABLE
				{
					// Token: 0x0400F071 RID: 61553
					public static LocString NAME = "Clearance: Vacancy";

					// Token: 0x0400F072 RID: 61554
					public static LocString TOOLTIP = "Suited Duplicants may only pass if there is an available dock to store their suit";
				}

				// Token: 0x02003C86 RID: 15494
				public static class ALWAYS
				{
					// Token: 0x0400F073 RID: 61555
					public static LocString NAME = "Clearance: Always";

					// Token: 0x0400F074 RID: 61556
					public static LocString TOOLTIP = "Suited Duplicants may pass even if there is no room to store their suits\n\nWhen all available docks are full, Duplicants will unequip their suits and drop them on the floor";
				}
			}

			// Token: 0x02003017 RID: 12311
			public class ACTIVATEBUILDING
			{
				// Token: 0x0400D04B RID: 53323
				public static LocString ACTIVATE = "Activate";

				// Token: 0x0400D04C RID: 53324
				public static LocString TOOLTIP_ACTIVATE = "Request a Duplicant to activate this building";

				// Token: 0x0400D04D RID: 53325
				public static LocString TOOLTIP_ACTIVATED = "This building has already been activated";

				// Token: 0x0400D04E RID: 53326
				public static LocString ACTIVATE_CANCEL = "Cancel Activation";

				// Token: 0x0400D04F RID: 53327
				public static LocString ACTIVATED = "Activated";

				// Token: 0x0400D050 RID: 53328
				public static LocString TOOLTIP_CANCEL = "Cancel activation of this building";
			}

			// Token: 0x02003018 RID: 12312
			public class ACCEPT_MUTANT_SEEDS
			{
				// Token: 0x0400D051 RID: 53329
				public static LocString ACCEPT = "Allow Mutants";

				// Token: 0x0400D052 RID: 53330
				public static LocString REJECT = "Forbid Mutants";

				// Token: 0x0400D053 RID: 53331
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Toggle whether or not this building will accept ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" for recipes that could use them"
				});

				// Token: 0x0400D054 RID: 53332
				public static LocString FISH_FEEDER_TOOLTIP = string.Concat(new string[]
				{
					"Toggle whether or not this feeder will accept ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" for critters who eat them"
				});
			}

			// Token: 0x02003019 RID: 12313
			public class CARVE
			{
				// Token: 0x0400D055 RID: 53333
				public static LocString NAME = "Carve";

				// Token: 0x0400D056 RID: 53334
				public static LocString TOOLTIP = "Carve this rock to enhance its positive effects";
			}

			// Token: 0x0200301A RID: 12314
			public class CANCELCARVE
			{
				// Token: 0x0400D057 RID: 53335
				public static LocString NAME = "Cancel Carve";

				// Token: 0x0400D058 RID: 53336
				public static LocString TOOLTIP = "Cancel order to carve this rock";
			}
		}

		// Token: 0x0200254B RID: 9547
		public class BUILDCATEGORIES
		{
			// Token: 0x0200301B RID: 12315
			public static class BASE
			{
				// Token: 0x0400D059 RID: 53337
				public static LocString NAME = UI.FormatAsLink("Base", "BUILDCATEGORYBASE");

				// Token: 0x0400D05A RID: 53338
				public static LocString TOOLTIP = "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
			}

			// Token: 0x0200301C RID: 12316
			public static class CONVEYANCE
			{
				// Token: 0x0400D05B RID: 53339
				public static LocString NAME = UI.FormatAsLink("Shipping", "BUILDCATEGORYCONVEYANCE");

				// Token: 0x0400D05C RID: 53340
				public static LocString TOOLTIP = "Transport ore and solid materials around my base. {Hotkey}";
			}

			// Token: 0x0200301D RID: 12317
			public static class OXYGEN
			{
				// Token: 0x0400D05D RID: 53341
				public static LocString NAME = UI.FormatAsLink("Oxygen", "BUILDCATEGORYOXYGEN");

				// Token: 0x0400D05E RID: 53342
				public static LocString TOOLTIP = "Everything I need to keep the colony breathing. {Hotkey}";
			}

			// Token: 0x0200301E RID: 12318
			public static class POWER
			{
				// Token: 0x0400D05F RID: 53343
				public static LocString NAME = UI.FormatAsLink("Power", "BUILDCATEGORYPOWER");

				// Token: 0x0400D060 RID: 53344
				public static LocString TOOLTIP = "Need to power the colony? Here's how to do it! {Hotkey}";
			}

			// Token: 0x0200301F RID: 12319
			public static class FOOD
			{
				// Token: 0x0400D061 RID: 53345
				public static LocString NAME = UI.FormatAsLink("Food", "BUILDCATEGORYFOOD");

				// Token: 0x0400D062 RID: 53346
				public static LocString TOOLTIP = "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
			}

			// Token: 0x02003020 RID: 12320
			public static class UTILITIES
			{
				// Token: 0x0400D063 RID: 53347
				public static LocString NAME = UI.FormatAsLink("Utilities", "BUILDCATEGORYUTILITIES");

				// Token: 0x0400D064 RID: 53348
				public static LocString TOOLTIP = "Heat up and cool down. {Hotkey}";
			}

			// Token: 0x02003021 RID: 12321
			public static class PLUMBING
			{
				// Token: 0x0400D065 RID: 53349
				public static LocString NAME = UI.FormatAsLink("Plumbing", "BUILDCATEGORYPLUMBING");

				// Token: 0x0400D066 RID: 53350
				public static LocString TOOLTIP = "Get the colony's water running and its sewage flowing. {Hotkey}";
			}

			// Token: 0x02003022 RID: 12322
			public static class HVAC
			{
				// Token: 0x0400D067 RID: 53351
				public static LocString NAME = UI.FormatAsLink("Ventilation", "BUILDCATEGORYHVAC");

				// Token: 0x0400D068 RID: 53352
				public static LocString TOOLTIP = "Control the flow of gas in the base. {Hotkey}";
			}

			// Token: 0x02003023 RID: 12323
			public static class REFINING
			{
				// Token: 0x0400D069 RID: 53353
				public static LocString NAME = UI.FormatAsLink("Refinement", "BUILDCATEGORYREFINING");

				// Token: 0x0400D06A RID: 53354
				public static LocString TOOLTIP = "Use the resources I want, filter the ones I don't. {Hotkey}";
			}

			// Token: 0x02003024 RID: 12324
			public static class ROCKETRY
			{
				// Token: 0x0400D06B RID: 53355
				public static LocString NAME = UI.FormatAsLink("Rocketry", "BUILDCATEGORYROCKETRY");

				// Token: 0x0400D06C RID: 53356
				public static LocString TOOLTIP = "With rockets, the sky's no longer the limit! {Hotkey}";
			}

			// Token: 0x02003025 RID: 12325
			public static class MEDICAL
			{
				// Token: 0x0400D06D RID: 53357
				public static LocString NAME = UI.FormatAsLink("Medicine", "BUILDCATEGORYMEDICAL");

				// Token: 0x0400D06E RID: 53358
				public static LocString TOOLTIP = "A cure for everything but the common cold. {Hotkey}";
			}

			// Token: 0x02003026 RID: 12326
			public static class FURNITURE
			{
				// Token: 0x0400D06F RID: 53359
				public static LocString NAME = UI.FormatAsLink("Furniture", "BUILDCATEGORYFURNITURE");

				// Token: 0x0400D070 RID: 53360
				public static LocString TOOLTIP = "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
			}

			// Token: 0x02003027 RID: 12327
			public static class EQUIPMENT
			{
				// Token: 0x0400D071 RID: 53361
				public static LocString NAME = UI.FormatAsLink("Stations", "BUILDCATEGORYEQUIPMENT");

				// Token: 0x0400D072 RID: 53362
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x02003028 RID: 12328
			public static class MISC
			{
				// Token: 0x0400D073 RID: 53363
				public static LocString NAME = UI.FormatAsLink("Decor", "BUILDCATEGORYMISC");

				// Token: 0x0400D074 RID: 53364
				public static LocString TOOLTIP = "Spruce up my colony with some lovely interior decorating. {Hotkey}";
			}

			// Token: 0x02003029 RID: 12329
			public static class AUTOMATION
			{
				// Token: 0x0400D075 RID: 53365
				public static LocString NAME = UI.FormatAsLink("Automation", "BUILDCATEGORYAUTOMATION");

				// Token: 0x0400D076 RID: 53366
				public static LocString TOOLTIP = "Automate my base with a wide range of sensors. {Hotkey}";
			}

			// Token: 0x0200302A RID: 12330
			public static class HEP
			{
				// Token: 0x0400D077 RID: 53367
				public static LocString NAME = UI.FormatAsLink("Radiation", "BUILDCATEGORYHEP");

				// Token: 0x0400D078 RID: 53368
				public static LocString TOOLTIP = "Here's where things get rad. {Hotkey}";
			}
		}

		// Token: 0x0200254C RID: 9548
		public class NEWBUILDCATEGORIES
		{
			// Token: 0x0200302B RID: 12331
			public static class BASE
			{
				// Token: 0x0400D079 RID: 53369
				public static LocString NAME = UI.FormatAsLink("Base", "BUILD_CATEGORY_BASE");

				// Token: 0x0400D07A RID: 53370
				public static LocString TOOLTIP = "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
			}

			// Token: 0x0200302C RID: 12332
			public static class INFRASTRUCTURE
			{
				// Token: 0x0400D07B RID: 53371
				public static LocString NAME = UI.FormatAsLink("Utilities", "BUILD_CATEGORY_INFRASTRUCTURE");

				// Token: 0x0400D07C RID: 53372
				public static LocString TOOLTIP = "Power, plumbing, and ventilation can all be found here. {Hotkey}";
			}

			// Token: 0x0200302D RID: 12333
			public static class FOODANDAGRICULTURE
			{
				// Token: 0x0400D07D RID: 53373
				public static LocString NAME = UI.FormatAsLink("Food", "BUILD_CATEGORY_FOODANDAGRICULTURE");

				// Token: 0x0400D07E RID: 53374
				public static LocString TOOLTIP = "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
			}

			// Token: 0x0200302E RID: 12334
			public static class LOGISTICS
			{
				// Token: 0x0400D07F RID: 53375
				public static LocString NAME = UI.FormatAsLink("Logistics", "BUILD_CATEGORY_LOGISTICS");

				// Token: 0x0400D080 RID: 53376
				public static LocString TOOLTIP = "Devices for base automation and material transport. {Hotkey}";
			}

			// Token: 0x0200302F RID: 12335
			public static class HEALTHANDHAPPINESS
			{
				// Token: 0x0400D081 RID: 53377
				public static LocString NAME = UI.FormatAsLink("Accommodation", "BUILD_CATEGORY_HEALTHANDHAPPINESS");

				// Token: 0x0400D082 RID: 53378
				public static LocString TOOLTIP = "Everything a Duplicant needs to stay happy, healthy, and fulfilled. {Hotkey}";
			}

			// Token: 0x02003030 RID: 12336
			public static class INDUSTRIAL
			{
				// Token: 0x0400D083 RID: 53379
				public static LocString NAME = UI.FormatAsLink("Industrials", "BUILD_CATEGORY_INDUSTRIAL");

				// Token: 0x0400D084 RID: 53380
				public static LocString TOOLTIP = "Machinery for oxygen production, heat management, and material refinement. {Hotkey}";
			}

			// Token: 0x02003031 RID: 12337
			public static class LADDERS
			{
				// Token: 0x0400D085 RID: 53381
				public static LocString NAME = "Ladders";

				// Token: 0x0400D086 RID: 53382
				public static LocString BUILDMENUTITLE = "Ladders";

				// Token: 0x0400D087 RID: 53383
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003032 RID: 12338
			public static class TILES
			{
				// Token: 0x0400D088 RID: 53384
				public static LocString NAME = "Tiles and Drywall";

				// Token: 0x0400D089 RID: 53385
				public static LocString BUILDMENUTITLE = "Tiles and Drywall";

				// Token: 0x0400D08A RID: 53386
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003033 RID: 12339
			public static class PRINTINGPODS
			{
				// Token: 0x0400D08B RID: 53387
				public static LocString NAME = "Printing Pods";

				// Token: 0x0400D08C RID: 53388
				public static LocString BUILDMENUTITLE = "Printing Pods";

				// Token: 0x0400D08D RID: 53389
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003034 RID: 12340
			public static class DOORS
			{
				// Token: 0x0400D08E RID: 53390
				public static LocString NAME = "Doors";

				// Token: 0x0400D08F RID: 53391
				public static LocString BUILDMENUTITLE = "Doors";

				// Token: 0x0400D090 RID: 53392
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003035 RID: 12341
			public static class STORAGE
			{
				// Token: 0x0400D091 RID: 53393
				public static LocString NAME = "Storage";

				// Token: 0x0400D092 RID: 53394
				public static LocString BUILDMENUTITLE = "Storage";

				// Token: 0x0400D093 RID: 53395
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003036 RID: 12342
			public static class TRANSPORT
			{
				// Token: 0x0400D094 RID: 53396
				public static LocString NAME = "Transit Tubes";

				// Token: 0x0400D095 RID: 53397
				public static LocString BUILDMENUTITLE = "Transit Tubes";

				// Token: 0x0400D096 RID: 53398
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003037 RID: 12343
			public static class OPERATIONS
			{
				// Token: 0x0400D097 RID: 53399
				public static LocString NAME = "Operations";

				// Token: 0x0400D098 RID: 53400
				public static LocString BUILDMENUTITLE = "Operations";

				// Token: 0x0400D099 RID: 53401
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003038 RID: 12344
			public static class PRODUCERS
			{
				// Token: 0x0400D09A RID: 53402
				public static LocString NAME = "Production";

				// Token: 0x0400D09B RID: 53403
				public static LocString BUILDMENUTITLE = "Production";

				// Token: 0x0400D09C RID: 53404
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003039 RID: 12345
			public static class SCRUBBERS
			{
				// Token: 0x0400D09D RID: 53405
				public static LocString NAME = "Purification";

				// Token: 0x0400D09E RID: 53406
				public static LocString BUILDMENUTITLE = "Purification";

				// Token: 0x0400D09F RID: 53407
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200303A RID: 12346
			public static class BATTERIES
			{
				// Token: 0x0400D0A0 RID: 53408
				public static LocString NAME = "Batteries";

				// Token: 0x0400D0A1 RID: 53409
				public static LocString BUILDMENUTITLE = "Batteries";

				// Token: 0x0400D0A2 RID: 53410
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200303B RID: 12347
			public static class SWITCHES
			{
				// Token: 0x0400D0A3 RID: 53411
				public static LocString NAME = "Switches";

				// Token: 0x0400D0A4 RID: 53412
				public static LocString BUILDMENUTITLE = "Switches";

				// Token: 0x0400D0A5 RID: 53413
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200303C RID: 12348
			public static class COOKING
			{
				// Token: 0x0400D0A6 RID: 53414
				public static LocString NAME = "Cooking";

				// Token: 0x0400D0A7 RID: 53415
				public static LocString BUILDMENUTITLE = "Cooking";

				// Token: 0x0400D0A8 RID: 53416
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200303D RID: 12349
			public static class FARMING
			{
				// Token: 0x0400D0A9 RID: 53417
				public static LocString NAME = "Farming";

				// Token: 0x0400D0AA RID: 53418
				public static LocString BUILDMENUTITLE = "Farming";

				// Token: 0x0400D0AB RID: 53419
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200303E RID: 12350
			public static class RANCHING
			{
				// Token: 0x0400D0AC RID: 53420
				public static LocString NAME = "Ranching";

				// Token: 0x0400D0AD RID: 53421
				public static LocString BUILDMENUTITLE = "Ranching";

				// Token: 0x0400D0AE RID: 53422
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200303F RID: 12351
			public static class WASHROOM
			{
				// Token: 0x0400D0AF RID: 53423
				public static LocString NAME = "Washroom";

				// Token: 0x0400D0B0 RID: 53424
				public static LocString BUILDMENUTITLE = "Washroom";

				// Token: 0x0400D0B1 RID: 53425
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003040 RID: 12352
			public static class VALVES
			{
				// Token: 0x0400D0B2 RID: 53426
				public static LocString NAME = "Valves";

				// Token: 0x0400D0B3 RID: 53427
				public static LocString BUILDMENUTITLE = "Valves";

				// Token: 0x0400D0B4 RID: 53428
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003041 RID: 12353
			public static class PUMPS
			{
				// Token: 0x0400D0B5 RID: 53429
				public static LocString NAME = "Pumps";

				// Token: 0x0400D0B6 RID: 53430
				public static LocString BUILDMENUTITLE = "Pumps";

				// Token: 0x0400D0B7 RID: 53431
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003042 RID: 12354
			public static class SENSORS
			{
				// Token: 0x0400D0B8 RID: 53432
				public static LocString NAME = "Sensors";

				// Token: 0x0400D0B9 RID: 53433
				public static LocString BUILDMENUTITLE = "Sensors";

				// Token: 0x0400D0BA RID: 53434
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003043 RID: 12355
			public static class PORTS
			{
				// Token: 0x0400D0BB RID: 53435
				public static LocString NAME = "Ports";

				// Token: 0x0400D0BC RID: 53436
				public static LocString BUILDMENUTITLE = "Ports";

				// Token: 0x0400D0BD RID: 53437
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003044 RID: 12356
			public static class MATERIALS
			{
				// Token: 0x0400D0BE RID: 53438
				public static LocString NAME = "Materials";

				// Token: 0x0400D0BF RID: 53439
				public static LocString BUILDMENUTITLE = "Materials";

				// Token: 0x0400D0C0 RID: 53440
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003045 RID: 12357
			public static class OIL
			{
				// Token: 0x0400D0C1 RID: 53441
				public static LocString NAME = "Oil";

				// Token: 0x0400D0C2 RID: 53442
				public static LocString BUILDMENUTITLE = "Oil";

				// Token: 0x0400D0C3 RID: 53443
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003046 RID: 12358
			public static class ADVANCED
			{
				// Token: 0x0400D0C4 RID: 53444
				public static LocString NAME = "Advanced";

				// Token: 0x0400D0C5 RID: 53445
				public static LocString BUILDMENUTITLE = "Advanced";

				// Token: 0x0400D0C6 RID: 53446
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003047 RID: 12359
			public static class ORGANIC
			{
				// Token: 0x0400D0C7 RID: 53447
				public static LocString NAME = "Organic";

				// Token: 0x0400D0C8 RID: 53448
				public static LocString BUILDMENUTITLE = "Organic";

				// Token: 0x0400D0C9 RID: 53449
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003048 RID: 12360
			public static class BEDS
			{
				// Token: 0x0400D0CA RID: 53450
				public static LocString NAME = "Beds";

				// Token: 0x0400D0CB RID: 53451
				public static LocString BUILDMENUTITLE = "Beds";

				// Token: 0x0400D0CC RID: 53452
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003049 RID: 12361
			public static class LIGHTS
			{
				// Token: 0x0400D0CD RID: 53453
				public static LocString NAME = "Lights";

				// Token: 0x0400D0CE RID: 53454
				public static LocString BUILDMENUTITLE = "Lights";

				// Token: 0x0400D0CF RID: 53455
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200304A RID: 12362
			public static class DINING
			{
				// Token: 0x0400D0D0 RID: 53456
				public static LocString NAME = "Dining";

				// Token: 0x0400D0D1 RID: 53457
				public static LocString BUILDMENUTITLE = "Dining";

				// Token: 0x0400D0D2 RID: 53458
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200304B RID: 12363
			public static class MANUFACTURING
			{
				// Token: 0x0400D0D3 RID: 53459
				public static LocString NAME = "Manufacturing";

				// Token: 0x0400D0D4 RID: 53460
				public static LocString BUILDMENUTITLE = "Manufacturing";

				// Token: 0x0400D0D5 RID: 53461
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200304C RID: 12364
			public static class TEMPERATURE
			{
				// Token: 0x0400D0D6 RID: 53462
				public static LocString NAME = "Temperature";

				// Token: 0x0400D0D7 RID: 53463
				public static LocString BUILDMENUTITLE = "Temperature";

				// Token: 0x0400D0D8 RID: 53464
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200304D RID: 12365
			public static class RESEARCH
			{
				// Token: 0x0400D0D9 RID: 53465
				public static LocString NAME = "Research";

				// Token: 0x0400D0DA RID: 53466
				public static LocString BUILDMENUTITLE = "Research";

				// Token: 0x0400D0DB RID: 53467
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200304E RID: 12366
			public static class GENERATORS
			{
				// Token: 0x0400D0DC RID: 53468
				public static LocString NAME = "Generators";

				// Token: 0x0400D0DD RID: 53469
				public static LocString BUILDMENUTITLE = "Generators";

				// Token: 0x0400D0DE RID: 53470
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200304F RID: 12367
			public static class WIRES
			{
				// Token: 0x0400D0DF RID: 53471
				public static LocString NAME = "Wires";

				// Token: 0x0400D0E0 RID: 53472
				public static LocString BUILDMENUTITLE = "Wires";

				// Token: 0x0400D0E1 RID: 53473
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003050 RID: 12368
			public static class ELECTROBANKBUILDINGS
			{
				// Token: 0x0400D0E2 RID: 53474
				public static LocString NAME = "Converters";

				// Token: 0x0400D0E3 RID: 53475
				public static LocString BUILDMENUTITLE = "Converters";

				// Token: 0x0400D0E4 RID: 53476
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003051 RID: 12369
			public static class LOGICGATES
			{
				// Token: 0x0400D0E5 RID: 53477
				public static LocString NAME = "Gates";

				// Token: 0x0400D0E6 RID: 53478
				public static LocString BUILDMENUTITLE = "Gates";

				// Token: 0x0400D0E7 RID: 53479
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003052 RID: 12370
			public static class TRANSMISSIONS
			{
				// Token: 0x0400D0E8 RID: 53480
				public static LocString NAME = "Transmissions";

				// Token: 0x0400D0E9 RID: 53481
				public static LocString BUILDMENUTITLE = "Transmissions";

				// Token: 0x0400D0EA RID: 53482
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003053 RID: 12371
			public static class LOGICMANAGER
			{
				// Token: 0x0400D0EB RID: 53483
				public static LocString NAME = "Monitoring";

				// Token: 0x0400D0EC RID: 53484
				public static LocString BUILDMENUTITLE = "Monitoring";

				// Token: 0x0400D0ED RID: 53485
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003054 RID: 12372
			public static class LOGICAUDIO
			{
				// Token: 0x0400D0EE RID: 53486
				public static LocString NAME = "Ambience";

				// Token: 0x0400D0EF RID: 53487
				public static LocString BUILDMENUTITLE = "Ambience";

				// Token: 0x0400D0F0 RID: 53488
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003055 RID: 12373
			public static class CONVEYANCESTRUCTURES
			{
				// Token: 0x0400D0F1 RID: 53489
				public static LocString NAME = "Structural";

				// Token: 0x0400D0F2 RID: 53490
				public static LocString BUILDMENUTITLE = "Structural";

				// Token: 0x0400D0F3 RID: 53491
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003056 RID: 12374
			public static class BUILDMENUPORTS
			{
				// Token: 0x0400D0F4 RID: 53492
				public static LocString NAME = "Ports";

				// Token: 0x0400D0F5 RID: 53493
				public static LocString BUILDMENUTITLE = "Ports";

				// Token: 0x0400D0F6 RID: 53494
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003057 RID: 12375
			public static class POWERCONTROL
			{
				// Token: 0x0400D0F7 RID: 53495
				public static LocString NAME = "Power\nRegulation";

				// Token: 0x0400D0F8 RID: 53496
				public static LocString BUILDMENUTITLE = "Power Regulation";

				// Token: 0x0400D0F9 RID: 53497
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003058 RID: 12376
			public static class PLUMBINGSTRUCTURES
			{
				// Token: 0x0400D0FA RID: 53498
				public static LocString NAME = "Plumbing";

				// Token: 0x0400D0FB RID: 53499
				public static LocString BUILDMENUTITLE = "Plumbing";

				// Token: 0x0400D0FC RID: 53500
				public static LocString TOOLTIP = "Get the colony's water running and its sewage flowing. {Hotkey}";
			}

			// Token: 0x02003059 RID: 12377
			public static class PIPES
			{
				// Token: 0x0400D0FD RID: 53501
				public static LocString NAME = "Pipes";

				// Token: 0x0400D0FE RID: 53502
				public static LocString BUILDMENUTITLE = "Pipes";

				// Token: 0x0400D0FF RID: 53503
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200305A RID: 12378
			public static class VENTILATIONSTRUCTURES
			{
				// Token: 0x0400D100 RID: 53504
				public static LocString NAME = "Ventilation";

				// Token: 0x0400D101 RID: 53505
				public static LocString BUILDMENUTITLE = "Ventilation";

				// Token: 0x0400D102 RID: 53506
				public static LocString TOOLTIP = "Control the flow of gas in your base. {Hotkey}";
			}

			// Token: 0x0200305B RID: 12379
			public static class CONVEYANCE
			{
				// Token: 0x0400D103 RID: 53507
				public static LocString NAME = "Ore\nTransport";

				// Token: 0x0400D104 RID: 53508
				public static LocString BUILDMENUTITLE = "Ore Transport";

				// Token: 0x0400D105 RID: 53509
				public static LocString TOOLTIP = "Transport ore and solid materials around my base. {Hotkey}";
			}

			// Token: 0x0200305C RID: 12380
			public static class HYGIENE
			{
				// Token: 0x0400D106 RID: 53510
				public static LocString NAME = "Hygiene";

				// Token: 0x0400D107 RID: 53511
				public static LocString BUILDMENUTITLE = "Hygiene";

				// Token: 0x0400D108 RID: 53512
				public static LocString TOOLTIP = "Keeps my Duplicants clean.";
			}

			// Token: 0x0200305D RID: 12381
			public static class MEDICAL
			{
				// Token: 0x0400D109 RID: 53513
				public static LocString NAME = "Medical";

				// Token: 0x0400D10A RID: 53514
				public static LocString BUILDMENUTITLE = "Medical";

				// Token: 0x0400D10B RID: 53515
				public static LocString TOOLTIP = "A cure for everything but the common cold. {Hotkey}";
			}

			// Token: 0x0200305E RID: 12382
			public static class WELLNESS
			{
				// Token: 0x0400D10C RID: 53516
				public static LocString NAME = "Wellness";

				// Token: 0x0400D10D RID: 53517
				public static LocString BUILDMENUTITLE = "Wellness";

				// Token: 0x0400D10E RID: 53518
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200305F RID: 12383
			public static class RECREATION
			{
				// Token: 0x0400D10F RID: 53519
				public static LocString NAME = "Recreation";

				// Token: 0x0400D110 RID: 53520
				public static LocString BUILDMENUTITLE = "Recreation";

				// Token: 0x0400D111 RID: 53521
				public static LocString TOOLTIP = "Everything needed to reduce stress and increase fun.";
			}

			// Token: 0x02003060 RID: 12384
			public static class FURNITURE
			{
				// Token: 0x0400D112 RID: 53522
				public static LocString NAME = "Furniture";

				// Token: 0x0400D113 RID: 53523
				public static LocString BUILDMENUTITLE = "Furniture";

				// Token: 0x0400D114 RID: 53524
				public static LocString TOOLTIP = "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
			}

			// Token: 0x02003061 RID: 12385
			public static class DECOR
			{
				// Token: 0x0400D115 RID: 53525
				public static LocString NAME = "Decor";

				// Token: 0x0400D116 RID: 53526
				public static LocString BUILDMENUTITLE = "Decor";

				// Token: 0x0400D117 RID: 53527
				public static LocString TOOLTIP = "Spruce up your colony with some lovely interior decorating. {Hotkey}";
			}

			// Token: 0x02003062 RID: 12386
			public static class OXYGEN
			{
				// Token: 0x0400D118 RID: 53528
				public static LocString NAME = "Oxygen";

				// Token: 0x0400D119 RID: 53529
				public static LocString BUILDMENUTITLE = "Oxygen";

				// Token: 0x0400D11A RID: 53530
				public static LocString TOOLTIP = "Everything I need to keep my colony breathing. {Hotkey}";
			}

			// Token: 0x02003063 RID: 12387
			public static class UTILITIES
			{
				// Token: 0x0400D11B RID: 53531
				public static LocString NAME = "Temperature";

				// Token: 0x0400D11C RID: 53532
				public static LocString BUILDMENUTITLE = "Temperature";

				// Token: 0x0400D11D RID: 53533
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003064 RID: 12388
			public static class REFINING
			{
				// Token: 0x0400D11E RID: 53534
				public static LocString NAME = "Refinement";

				// Token: 0x0400D11F RID: 53535
				public static LocString BUILDMENUTITLE = "Refinement";

				// Token: 0x0400D120 RID: 53536
				public static LocString TOOLTIP = "Use the resources you want, filter the ones you don't. {Hotkey}";
			}

			// Token: 0x02003065 RID: 12389
			public static class EQUIPMENT
			{
				// Token: 0x0400D121 RID: 53537
				public static LocString NAME = "Equipment";

				// Token: 0x0400D122 RID: 53538
				public static LocString BUILDMENUTITLE = "Equipment";

				// Token: 0x0400D123 RID: 53539
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x02003066 RID: 12390
			public static class ARCHAEOLOGY
			{
				// Token: 0x0400D124 RID: 53540
				public static LocString NAME = "Archaeology";

				// Token: 0x0400D125 RID: 53541
				public static LocString BUILDMENUTITLE = "Archaeology";

				// Token: 0x0400D126 RID: 53542
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003067 RID: 12391
			public static class METEORDEFENSE
			{
				// Token: 0x0400D127 RID: 53543
				public static LocString NAME = "Meteor Defense";

				// Token: 0x0400D128 RID: 53544
				public static LocString BUILDMENUTITLE = "Meteor Defense";

				// Token: 0x0400D129 RID: 53545
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003068 RID: 12392
			public static class INDUSTRIALSTATION
			{
				// Token: 0x0400D12A RID: 53546
				public static LocString NAME = "Industrial";

				// Token: 0x0400D12B RID: 53547
				public static LocString BUILDMENUTITLE = "Industrial";

				// Token: 0x0400D12C RID: 53548
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003069 RID: 12393
			public static class TELESCOPES
			{
				// Token: 0x0400D12D RID: 53549
				public static LocString NAME = "Telescopes";

				// Token: 0x0400D12E RID: 53550
				public static LocString BUILDMENUTITLE = "Telescopes";

				// Token: 0x0400D12F RID: 53551
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x0200306A RID: 12394
			public static class MISSILES
			{
				// Token: 0x0400D130 RID: 53552
				public static LocString NAME = "Meteor Defense";

				// Token: 0x0400D131 RID: 53553
				public static LocString BUILDMENUTITLE = "Meteor Defense";

				// Token: 0x0400D132 RID: 53554
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200306B RID: 12395
			public static class FITTINGS
			{
				// Token: 0x0400D133 RID: 53555
				public static LocString NAME = "Fittings";

				// Token: 0x0400D134 RID: 53556
				public static LocString BUILDMENUTITLE = "Fittings";

				// Token: 0x0400D135 RID: 53557
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200306C RID: 12396
			public static class SANITATION
			{
				// Token: 0x0400D136 RID: 53558
				public static LocString NAME = "Sanitation";

				// Token: 0x0400D137 RID: 53559
				public static LocString BUILDMENUTITLE = "Sanitation";

				// Token: 0x0400D138 RID: 53560
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200306D RID: 12397
			public static class AUTOMATED
			{
				// Token: 0x0400D139 RID: 53561
				public static LocString NAME = "Automated";

				// Token: 0x0400D13A RID: 53562
				public static LocString BUILDMENUTITLE = "Automated";

				// Token: 0x0400D13B RID: 53563
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200306E RID: 12398
			public static class ROCKETSTRUCTURES
			{
				// Token: 0x0400D13C RID: 53564
				public static LocString NAME = "Structural";

				// Token: 0x0400D13D RID: 53565
				public static LocString BUILDMENUTITLE = "Structural";

				// Token: 0x0400D13E RID: 53566
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200306F RID: 12399
			public static class ROCKETNAV
			{
				// Token: 0x0400D13F RID: 53567
				public static LocString NAME = "Navigation";

				// Token: 0x0400D140 RID: 53568
				public static LocString BUILDMENUTITLE = "Navigation";

				// Token: 0x0400D141 RID: 53569
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003070 RID: 12400
			public static class CONDUITSENSORS
			{
				// Token: 0x0400D142 RID: 53570
				public static LocString NAME = "Pipe Sensors";

				// Token: 0x0400D143 RID: 53571
				public static LocString BUILDMENUTITLE = "Pipe Sensors";

				// Token: 0x0400D144 RID: 53572
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003071 RID: 12401
			public static class ROCKETRY
			{
				// Token: 0x0400D145 RID: 53573
				public static LocString NAME = "Rocketry";

				// Token: 0x0400D146 RID: 53574
				public static LocString BUILDMENUTITLE = "Rocketry";

				// Token: 0x0400D147 RID: 53575
				public static LocString TOOLTIP = "Rocketry {Hotkey}";
			}

			// Token: 0x02003072 RID: 12402
			public static class ENGINES
			{
				// Token: 0x0400D148 RID: 53576
				public static LocString NAME = "Engines";

				// Token: 0x0400D149 RID: 53577
				public static LocString BUILDMENUTITLE = "Engines";

				// Token: 0x0400D14A RID: 53578
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003073 RID: 12403
			public static class TANKS
			{
				// Token: 0x0400D14B RID: 53579
				public static LocString NAME = "Tanks";

				// Token: 0x0400D14C RID: 53580
				public static LocString BUILDMENUTITLE = "Tanks";

				// Token: 0x0400D14D RID: 53581
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003074 RID: 12404
			public static class CARGO
			{
				// Token: 0x0400D14E RID: 53582
				public static LocString NAME = "Cargo";

				// Token: 0x0400D14F RID: 53583
				public static LocString BUILDMENUTITLE = "Cargo";

				// Token: 0x0400D150 RID: 53584
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003075 RID: 12405
			public static class MODULE
			{
				// Token: 0x0400D151 RID: 53585
				public static LocString NAME = "Modules";

				// Token: 0x0400D152 RID: 53586
				public static LocString BUILDMENUTITLE = "Modules";

				// Token: 0x0400D153 RID: 53587
				public static LocString TOOLTIP = "";
			}
		}

		// Token: 0x0200254D RID: 9549
		public class TOOLS
		{
			// Token: 0x0400A79F RID: 42911
			public static LocString TOOL_AREA_FMT = "{0} x {1}\n{2} tiles";

			// Token: 0x0400A7A0 RID: 42912
			public static LocString TOOL_LENGTH_FMT = "{0}";

			// Token: 0x0400A7A1 RID: 42913
			public static LocString FILTER_HOVERCARD_HEADER = "   <style=\"hovercard_element\">({0})</style>";

			// Token: 0x0400A7A2 RID: 42914
			public static LocString CAPITALS = "<uppercase>{0}</uppercase>";

			// Token: 0x02003076 RID: 12406
			public class SANDBOX
			{
				// Token: 0x02003C87 RID: 15495
				public class SANDBOX_TOGGLE
				{
					// Token: 0x0400F075 RID: 61557
					public static LocString NAME = "SANDBOX";
				}

				// Token: 0x02003C88 RID: 15496
				public class BRUSH
				{
					// Token: 0x0400F076 RID: 61558
					public static LocString NAME = "Brush";

					// Token: 0x0400F077 RID: 61559
					public static LocString HOVERACTION = "PAINT SIM";
				}

				// Token: 0x02003C89 RID: 15497
				public class SPRINKLE
				{
					// Token: 0x0400F078 RID: 61560
					public static LocString NAME = "Sprinkle";

					// Token: 0x0400F079 RID: 61561
					public static LocString HOVERACTION = "SPRINKLE SIM";
				}

				// Token: 0x02003C8A RID: 15498
				public class FLOOD
				{
					// Token: 0x0400F07A RID: 61562
					public static LocString NAME = "Fill";

					// Token: 0x0400F07B RID: 61563
					public static LocString HOVERACTION = "PAINT SECTION";
				}

				// Token: 0x02003C8B RID: 15499
				public class MARQUEE
				{
					// Token: 0x0400F07C RID: 61564
					public static LocString NAME = "Marquee";
				}

				// Token: 0x02003C8C RID: 15500
				public class SAMPLE
				{
					// Token: 0x0400F07D RID: 61565
					public static LocString NAME = "Sample";

					// Token: 0x0400F07E RID: 61566
					public static LocString HOVERACTION = "COPY SELECTION";
				}

				// Token: 0x02003C8D RID: 15501
				public class HEATGUN
				{
					// Token: 0x0400F07F RID: 61567
					public static LocString NAME = "Heat Gun";

					// Token: 0x0400F080 RID: 61568
					public static LocString HOVERACTION = "PAINT HEAT";
				}

				// Token: 0x02003C8E RID: 15502
				public class RADSTOOL
				{
					// Token: 0x0400F081 RID: 61569
					public static LocString NAME = "Radiation Tool";

					// Token: 0x0400F082 RID: 61570
					public static LocString HOVERACTION = "PAINT RADS";
				}

				// Token: 0x02003C8F RID: 15503
				public class STRESSTOOL
				{
					// Token: 0x0400F083 RID: 61571
					public static LocString NAME = "Happy Tool";

					// Token: 0x0400F084 RID: 61572
					public static LocString HOVERACTION = "PAINT CALM";
				}

				// Token: 0x02003C90 RID: 15504
				public class SPAWNER
				{
					// Token: 0x0400F085 RID: 61573
					public static LocString NAME = "Spawner";

					// Token: 0x0400F086 RID: 61574
					public static LocString HOVERACTION = "SPAWN";
				}

				// Token: 0x02003C91 RID: 15505
				public class CLEAR_FLOOR
				{
					// Token: 0x0400F087 RID: 61575
					public static LocString NAME = "Clear Floor";

					// Token: 0x0400F088 RID: 61576
					public static LocString HOVERACTION = "DELETE DEBRIS";
				}

				// Token: 0x02003C92 RID: 15506
				public class DESTROY
				{
					// Token: 0x0400F089 RID: 61577
					public static LocString NAME = "Destroy";

					// Token: 0x0400F08A RID: 61578
					public static LocString HOVERACTION = "DELETE";
				}

				// Token: 0x02003C93 RID: 15507
				public class SPAWN_ENTITY
				{
					// Token: 0x0400F08B RID: 61579
					public static LocString NAME = "Spawn";
				}

				// Token: 0x02003C94 RID: 15508
				public class FOW
				{
					// Token: 0x0400F08C RID: 61580
					public static LocString NAME = "Reveal";

					// Token: 0x0400F08D RID: 61581
					public static LocString HOVERACTION = "DE-FOG";
				}

				// Token: 0x02003C95 RID: 15509
				public class CRITTER
				{
					// Token: 0x0400F08E RID: 61582
					public static LocString NAME = "Critter Removal";

					// Token: 0x0400F08F RID: 61583
					public static LocString HOVERACTION = "DELETE CRITTERS";
				}

				// Token: 0x02003C96 RID: 15510
				public class SPAWN_STORY_TRAIT
				{
					// Token: 0x0400F090 RID: 61584
					public static LocString NAME = "Story Trait";

					// Token: 0x0400F091 RID: 61585
					public static LocString HOVERACTION = "PLACE";

					// Token: 0x0400F092 RID: 61586
					public static LocString ERROR_ALREADY_EXISTS = "{StoryName} already exists in this save";

					// Token: 0x0400F093 RID: 61587
					public static LocString ERROR_INVALID_LOCATION = "Invalid location";

					// Token: 0x0400F094 RID: 61588
					public static LocString ERROR_DUPE_HAZARD = "One or more Duplicants are in the way";

					// Token: 0x0400F095 RID: 61589
					public static LocString ERROR_ROBOT_HAZARD = "One or more robots are in the way";

					// Token: 0x0400F096 RID: 61590
					public static LocString ERROR_CREATURE_HAZARD = "One or more critters are in the way";

					// Token: 0x0400F097 RID: 61591
					public static LocString ERROR_BUILDING_HAZARD = "One or more buildings are in the way";
				}
			}

			// Token: 0x02003077 RID: 12407
			public class GENERIC
			{
				// Token: 0x0400D154 RID: 53588
				public static LocString BACK = "Back";

				// Token: 0x0400D155 RID: 53589
				public static LocString UNKNOWN = "UNKNOWN";

				// Token: 0x0400D156 RID: 53590
				public static LocString BUILDING_HOVER_NAME_FMT = "{Name}    <style=\"hovercard_element\">({Element})</style>";

				// Token: 0x0400D157 RID: 53591
				public static LocString LOGIC_INPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400D158 RID: 53592
				public static LocString LOGIC_OUTPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400D159 RID: 53593
				public static LocString LOGIC_MULTI_INPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400D15A RID: 53594
				public static LocString LOGIC_MULTI_OUTPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";
			}

			// Token: 0x02003078 RID: 12408
			public class ATTACK
			{
				// Token: 0x0400D15B RID: 53595
				public static LocString NAME = "Attack";

				// Token: 0x0400D15C RID: 53596
				public static LocString TOOLNAME = "Attack tool";

				// Token: 0x0400D15D RID: 53597
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02003079 RID: 12409
			public class CAPTURE
			{
				// Token: 0x0400D15E RID: 53598
				public static LocString NAME = "Wrangle";

				// Token: 0x0400D15F RID: 53599
				public static LocString TOOLNAME = "Wrangle tool";

				// Token: 0x0400D160 RID: 53600
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400D161 RID: 53601
				public static LocString NOT_CAPTURABLE = "Cannot Wrangle";
			}

			// Token: 0x0200307A RID: 12410
			public class BUILD
			{
				// Token: 0x0400D162 RID: 53602
				public static LocString NAME = "Build {0}";

				// Token: 0x0400D163 RID: 53603
				public static LocString TOOLNAME = "Build tool";

				// Token: 0x0400D164 RID: 53604
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) + " TO BUILD";

				// Token: 0x0400D165 RID: 53605
				public static LocString TOOLACTION_DRAG = "DRAG";
			}

			// Token: 0x0200307B RID: 12411
			public class PLACE
			{
				// Token: 0x0400D166 RID: 53606
				public static LocString NAME = "Place {0}";

				// Token: 0x0400D167 RID: 53607
				public static LocString TOOLNAME = "Place tool";

				// Token: 0x0400D168 RID: 53608
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) + " TO PLACE";

				// Token: 0x02003C97 RID: 15511
				public class REASONS
				{
					// Token: 0x0400F098 RID: 61592
					public static LocString CAN_OCCUPY_AREA = "Location blocked";

					// Token: 0x0400F099 RID: 61593
					public static LocString ON_FOUNDATION = "Must place on the ground";

					// Token: 0x0400F09A RID: 61594
					public static LocString VISIBLE_TO_SPACE = "Must have a clear path to space";

					// Token: 0x0400F09B RID: 61595
					public static LocString RESTRICT_TO_WORLD = "Incorrect " + UI.CLUSTERMAP.PLANETOID;
				}
			}

			// Token: 0x0200307C RID: 12412
			public class MOVETOLOCATION
			{
				// Token: 0x0400D169 RID: 53609
				public static LocString NAME = "Relocate";

				// Token: 0x0400D16A RID: 53610
				public static LocString TOOLNAME = "Relocate Tool";

				// Token: 0x0400D16B RID: 53611
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) ?? "";

				// Token: 0x0400D16C RID: 53612
				public static LocString UNREACHABLE = "UNREACHABLE";
			}

			// Token: 0x0200307D RID: 12413
			public class COPYSETTINGS
			{
				// Token: 0x0400D16D RID: 53613
				public static LocString NAME = "Paste Settings";

				// Token: 0x0400D16E RID: 53614
				public static LocString TOOLNAME = "Paste Settings Tool";

				// Token: 0x0400D16F RID: 53615
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x0200307E RID: 12414
			public class DIG
			{
				// Token: 0x0400D170 RID: 53616
				public static LocString NAME = "Dig";

				// Token: 0x0400D171 RID: 53617
				public static LocString TOOLNAME = "Dig tool";

				// Token: 0x0400D172 RID: 53618
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x0200307F RID: 12415
			public class DISINFECT
			{
				// Token: 0x0400D173 RID: 53619
				public static LocString NAME = "Disinfect";

				// Token: 0x0400D174 RID: 53620
				public static LocString TOOLNAME = "Disinfect tool";

				// Token: 0x0400D175 RID: 53621
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02003080 RID: 12416
			public class DISCONNECT
			{
				// Token: 0x0400D176 RID: 53622
				public static LocString NAME = "Disconnect";

				// Token: 0x0400D177 RID: 53623
				public static LocString TOOLTIP = "Sever conduits and connectors {Hotkey}";

				// Token: 0x0400D178 RID: 53624
				public static LocString TOOLNAME = "Disconnect tool";

				// Token: 0x0400D179 RID: 53625
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02003081 RID: 12417
			public class CANCEL
			{
				// Token: 0x0400D17A RID: 53626
				public static LocString NAME = "Cancel";

				// Token: 0x0400D17B RID: 53627
				public static LocString TOOLNAME = "Cancel tool";

				// Token: 0x0400D17C RID: 53628
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02003082 RID: 12418
			public class DECONSTRUCT
			{
				// Token: 0x0400D17D RID: 53629
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400D17E RID: 53630
				public static LocString TOOLNAME = "Deconstruct tool";

				// Token: 0x0400D17F RID: 53631
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02003083 RID: 12419
			public class CLEANUPCATEGORY
			{
				// Token: 0x0400D180 RID: 53632
				public static LocString NAME = "Clean";

				// Token: 0x0400D181 RID: 53633
				public static LocString TOOLNAME = "Clean Up tools";
			}

			// Token: 0x02003084 RID: 12420
			public class PRIORITIESCATEGORY
			{
				// Token: 0x0400D182 RID: 53634
				public static LocString NAME = "Priority";
			}

			// Token: 0x02003085 RID: 12421
			public class MARKFORSTORAGE
			{
				// Token: 0x0400D183 RID: 53635
				public static LocString NAME = "Sweep";

				// Token: 0x0400D184 RID: 53636
				public static LocString TOOLNAME = "Sweep tool";

				// Token: 0x0400D185 RID: 53637
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02003086 RID: 12422
			public class MOP
			{
				// Token: 0x0400D186 RID: 53638
				public static LocString NAME = "Mop";

				// Token: 0x0400D187 RID: 53639
				public static LocString TOOLNAME = "Mop tool";

				// Token: 0x0400D188 RID: 53640
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400D189 RID: 53641
				public static LocString TOO_MUCH_LIQUID = "Too Much Liquid";

				// Token: 0x0400D18A RID: 53642
				public static LocString NOT_ON_FLOOR = "Not On Floor";
			}

			// Token: 0x02003087 RID: 12423
			public class HARVEST
			{
				// Token: 0x0400D18B RID: 53643
				public static LocString NAME = "Harvest";

				// Token: 0x0400D18C RID: 53644
				public static LocString TOOLNAME = "Harvest tool";

				// Token: 0x0400D18D RID: 53645
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02003088 RID: 12424
			public class PRIORITIZE
			{
				// Token: 0x0400D18E RID: 53646
				public static LocString NAME = "Priority";

				// Token: 0x0400D18F RID: 53647
				public static LocString TOOLNAME = "Priority tool";

				// Token: 0x0400D190 RID: 53648
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400D191 RID: 53649
				public static LocString SPECIFIC_PRIORITY = "Set Priority: {0}";
			}

			// Token: 0x02003089 RID: 12425
			public class EMPTY_PIPE
			{
				// Token: 0x0400D192 RID: 53650
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400D193 RID: 53651
				public static LocString TOOLTIP = "Extract pipe contents {Hotkey}";

				// Token: 0x0400D194 RID: 53652
				public static LocString TOOLNAME = "Empty Pipe tool";

				// Token: 0x0400D195 RID: 53653
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x0200308A RID: 12426
			public class FILTERSCREEN
			{
				// Token: 0x0400D196 RID: 53654
				public static LocString OPTIONS = "Tool Filter";
			}

			// Token: 0x0200308B RID: 12427
			public class FILTERLAYERS
			{
				// Token: 0x02003C98 RID: 15512
				public class BUILDINGS
				{
					// Token: 0x0400F09C RID: 61596
					public static LocString NAME = "Buildings";

					// Token: 0x0400F09D RID: 61597
					public static LocString TOOLTIP = "All buildings";
				}

				// Token: 0x02003C99 RID: 15513
				public class TILES
				{
					// Token: 0x0400F09E RID: 61598
					public static LocString NAME = "Tiles";

					// Token: 0x0400F09F RID: 61599
					public static LocString TOOLTIP = "Tiles only";
				}

				// Token: 0x02003C9A RID: 15514
				public class WIRES
				{
					// Token: 0x0400F0A0 RID: 61600
					public static LocString NAME = "Power Wires";

					// Token: 0x0400F0A1 RID: 61601
					public static LocString TOOLTIP = "Power wires only";
				}

				// Token: 0x02003C9B RID: 15515
				public class SOLIDCONDUITS
				{
					// Token: 0x0400F0A2 RID: 61602
					public static LocString NAME = "Conveyor Rails";

					// Token: 0x0400F0A3 RID: 61603
					public static LocString TOOLTIP = "Conveyor rails only";
				}

				// Token: 0x02003C9C RID: 15516
				public class DIGPLACER
				{
					// Token: 0x0400F0A4 RID: 61604
					public static LocString NAME = "Dig Orders";

					// Token: 0x0400F0A5 RID: 61605
					public static LocString TOOLTIP = "Dig orders only";
				}

				// Token: 0x02003C9D RID: 15517
				public class CLEANANDCLEAR
				{
					// Token: 0x0400F0A6 RID: 61606
					public static LocString NAME = "Sweep & Mop Orders";

					// Token: 0x0400F0A7 RID: 61607
					public static LocString TOOLTIP = "Sweep and mop orders only";
				}

				// Token: 0x02003C9E RID: 15518
				public class HARVEST_WHEN_READY
				{
					// Token: 0x0400F0A8 RID: 61608
					public static LocString NAME = "Enable Harvest";

					// Token: 0x0400F0A9 RID: 61609
					public static LocString TOOLTIP = "Enable harvest on selected plants";
				}

				// Token: 0x02003C9F RID: 15519
				public class DO_NOT_HARVEST
				{
					// Token: 0x0400F0AA RID: 61610
					public static LocString NAME = "Disable Harvest";

					// Token: 0x0400F0AB RID: 61611
					public static LocString TOOLTIP = "Disable harvest on selected plants";
				}

				// Token: 0x02003CA0 RID: 15520
				public class ATTACK
				{
					// Token: 0x0400F0AC RID: 61612
					public static LocString NAME = "Attack";

					// Token: 0x0400F0AD RID: 61613
					public static LocString TOOLTIP = "";
				}

				// Token: 0x02003CA1 RID: 15521
				public class LOGIC
				{
					// Token: 0x0400F0AE RID: 61614
					public static LocString NAME = "Automation";

					// Token: 0x0400F0AF RID: 61615
					public static LocString TOOLTIP = "Automation buildings only";
				}

				// Token: 0x02003CA2 RID: 15522
				public class BACKWALL
				{
					// Token: 0x0400F0B0 RID: 61616
					public static LocString NAME = "Background Buildings";

					// Token: 0x0400F0B1 RID: 61617
					public static LocString TOOLTIP = "Background buildings only";
				}

				// Token: 0x02003CA3 RID: 15523
				public class LIQUIDPIPES
				{
					// Token: 0x0400F0B2 RID: 61618
					public static LocString NAME = "Liquid Pipes";

					// Token: 0x0400F0B3 RID: 61619
					public static LocString TOOLTIP = "Liquid pipes only";
				}

				// Token: 0x02003CA4 RID: 15524
				public class GASPIPES
				{
					// Token: 0x0400F0B4 RID: 61620
					public static LocString NAME = "Gas Pipes";

					// Token: 0x0400F0B5 RID: 61621
					public static LocString TOOLTIP = "Gas pipes only";
				}

				// Token: 0x02003CA5 RID: 15525
				public class ALL
				{
					// Token: 0x0400F0B6 RID: 61622
					public static LocString NAME = "All";

					// Token: 0x0400F0B7 RID: 61623
					public static LocString TOOLTIP = "Target all";
				}

				// Token: 0x02003CA6 RID: 15526
				public class ALL_OVERLAY
				{
					// Token: 0x0400F0B8 RID: 61624
					public static LocString NAME = "All";

					// Token: 0x0400F0B9 RID: 61625
					public static LocString TOOLTIP = "Show all";
				}

				// Token: 0x02003CA7 RID: 15527
				public class METAL
				{
					// Token: 0x0400F0BA RID: 61626
					public static LocString NAME = "Metal";

					// Token: 0x0400F0BB RID: 61627
					public static LocString TOOLTIP = "Show only metals";
				}

				// Token: 0x02003CA8 RID: 15528
				public class BUILDABLE
				{
					// Token: 0x0400F0BC RID: 61628
					public static LocString NAME = "Mineral";

					// Token: 0x0400F0BD RID: 61629
					public static LocString TOOLTIP = "Show only minerals";
				}

				// Token: 0x02003CA9 RID: 15529
				public class FILTER
				{
					// Token: 0x0400F0BE RID: 61630
					public static LocString NAME = "Filtration Medium";

					// Token: 0x0400F0BF RID: 61631
					public static LocString TOOLTIP = "Show only filtration mediums";
				}

				// Token: 0x02003CAA RID: 15530
				public class CONSUMABLEORE
				{
					// Token: 0x0400F0C0 RID: 61632
					public static LocString NAME = "Consumable Ore";

					// Token: 0x0400F0C1 RID: 61633
					public static LocString TOOLTIP = "Show only consumable ore";
				}

				// Token: 0x02003CAB RID: 15531
				public class ORGANICS
				{
					// Token: 0x0400F0C2 RID: 61634
					public static LocString NAME = "Organic";

					// Token: 0x0400F0C3 RID: 61635
					public static LocString TOOLTIP = "Show only organic materials";
				}

				// Token: 0x02003CAC RID: 15532
				public class FARMABLE
				{
					// Token: 0x0400F0C4 RID: 61636
					public static LocString NAME = "Cultivable Soil";

					// Token: 0x0400F0C5 RID: 61637
					public static LocString TOOLTIP = "Show only cultivable soil";
				}

				// Token: 0x02003CAD RID: 15533
				public class LIQUIFIABLE
				{
					// Token: 0x0400F0C6 RID: 61638
					public static LocString NAME = "Liquefiable";

					// Token: 0x0400F0C7 RID: 61639
					public static LocString TOOLTIP = "Show only liquefiable elements";
				}

				// Token: 0x02003CAE RID: 15534
				public class GAS
				{
					// Token: 0x0400F0C8 RID: 61640
					public static LocString NAME = "Gas";

					// Token: 0x0400F0C9 RID: 61641
					public static LocString TOOLTIP = "Show only gases";
				}

				// Token: 0x02003CAF RID: 15535
				public class LIQUID
				{
					// Token: 0x0400F0CA RID: 61642
					public static LocString NAME = "Liquid";

					// Token: 0x0400F0CB RID: 61643
					public static LocString TOOLTIP = "Show only liquids";
				}

				// Token: 0x02003CB0 RID: 15536
				public class MISC
				{
					// Token: 0x0400F0CC RID: 61644
					public static LocString NAME = "Miscellaneous";

					// Token: 0x0400F0CD RID: 61645
					public static LocString TOOLTIP = "Show only miscellaneous elements";
				}

				// Token: 0x02003CB1 RID: 15537
				public class ABSOLUTETEMPERATURE
				{
					// Token: 0x0400F0CE RID: 61646
					public static LocString NAME = "Absolute Temperature";

					// Token: 0x0400F0CF RID: 61647
					public static LocString TOOLTIP = "<b>Absolute Temperature</b>\nView the default temperature ranges and categories relative to absolute zero";
				}

				// Token: 0x02003CB2 RID: 15538
				public class RELATIVETEMPERATURE
				{
					// Token: 0x0400F0D0 RID: 61648
					public static LocString NAME = "Relative Temperature";

					// Token: 0x0400F0D1 RID: 61649
					public static LocString TOOLTIP = "<b>Relative Temperature</b>\nCustomize visual map to identify temperatures relative to a selected midpoint\n\nDrag the slider to adjust the relative temperature range";
				}

				// Token: 0x02003CB3 RID: 15539
				public class HEATFLOW
				{
					// Token: 0x0400F0D2 RID: 61650
					public static LocString NAME = "Thermal Tolerance";

					// Token: 0x0400F0D3 RID: 61651
					public static LocString TOOLTIP = "<b>Thermal Tolerance</b>\nView the impact of ambient temperatures on living beings";
				}

				// Token: 0x02003CB4 RID: 15540
				public class STATECHANGE
				{
					// Token: 0x0400F0D4 RID: 61652
					public static LocString NAME = "State Change";

					// Token: 0x0400F0D5 RID: 61653
					public static LocString TOOLTIP = "<b>State Change</b>\nView the impact of ambient temperatures on element states";
				}

				// Token: 0x02003CB5 RID: 15541
				public class BREATHABLE
				{
					// Token: 0x0400F0D6 RID: 61654
					public static LocString NAME = "Breathable Gas";

					// Token: 0x0400F0D7 RID: 61655
					public static LocString TOOLTIP = "Show only breathable gases";
				}

				// Token: 0x02003CB6 RID: 15542
				public class UNBREATHABLE
				{
					// Token: 0x0400F0D8 RID: 61656
					public static LocString NAME = "Unbreathable Gas";

					// Token: 0x0400F0D9 RID: 61657
					public static LocString TOOLTIP = "Show only unbreathable gases";
				}

				// Token: 0x02003CB7 RID: 15543
				public class AGRICULTURE
				{
					// Token: 0x0400F0DA RID: 61658
					public static LocString NAME = "Agriculture";

					// Token: 0x0400F0DB RID: 61659
					public static LocString TOOLTIP = "";
				}

				// Token: 0x02003CB8 RID: 15544
				public class ADAPTIVETEMPERATURE
				{
					// Token: 0x0400F0DC RID: 61660
					public static LocString NAME = "Adapt. Temperature";

					// Token: 0x0400F0DD RID: 61661
					public static LocString TOOLTIP = "";
				}

				// Token: 0x02003CB9 RID: 15545
				public class CONSTRUCTION
				{
					// Token: 0x0400F0DE RID: 61662
					public static LocString NAME = "Construction";

					// Token: 0x0400F0DF RID: 61663
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Target ",
						UI.PRE_KEYWORD,
						"Construction",
						UI.PST_KEYWORD,
						" errands only"
					});
				}

				// Token: 0x02003CBA RID: 15546
				public class DIG
				{
					// Token: 0x0400F0E0 RID: 61664
					public static LocString NAME = "Digging";

					// Token: 0x0400F0E1 RID: 61665
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Target ",
						UI.PRE_KEYWORD,
						"Digging",
						UI.PST_KEYWORD,
						" errands only"
					});
				}

				// Token: 0x02003CBB RID: 15547
				public class CLEAN
				{
					// Token: 0x0400F0E2 RID: 61666
					public static LocString NAME = "Cleaning";

					// Token: 0x0400F0E3 RID: 61667
					public static LocString TOOLTIP = "Target cleaning errands only";
				}

				// Token: 0x02003CBC RID: 15548
				public class OPERATE
				{
					// Token: 0x0400F0E4 RID: 61668
					public static LocString NAME = "Duties";

					// Token: 0x0400F0E5 RID: 61669
					public static LocString TOOLTIP = "Target general duties only";
				}
			}
		}

		// Token: 0x0200254E RID: 9550
		public class DETAILTABS
		{
			// Token: 0x0200308C RID: 12428
			public class TEXTICONDATA
			{
				// Token: 0x02003CBD RID: 15549
				public class REFRIGERATED
				{
					// Token: 0x0400F0E6 RID: 61670
					public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_refrigerated\">";

					// Token: 0x0400F0E7 RID: 61671
					public static LocString NAME = "Refrigerated";

					// Token: 0x0400F0E8 RID: 61672
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Ideal ",
						UI.PRE_KEYWORD,
						"Temperature",
						UI.PST_KEYWORD,
						" storage is slowing this food's ",
						UI.PRE_KEYWORD,
						"Decay Rate",
						UI.PST_KEYWORD,
						"\n\nCold environments can also mimic refrigeration"
					});
				}

				// Token: 0x02003CBE RID: 15550
				public class DEEPFROZEN
				{
					// Token: 0x0400F0E9 RID: 61673
					public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_frozen\">";

					// Token: 0x0400F0EA RID: 61674
					public static LocString NAME = "Deep Freeze";

					// Token: 0x0400F0EB RID: 61675
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Extremely low ",
						UI.PRE_KEYWORD,
						"Temperatures",
						UI.PST_KEYWORD,
						" are greatly prolonging the shelf-life of this food\n\nCold environments may also result in frozen food"
					});
				}

				// Token: 0x02003CBF RID: 15551
				public class FRESH
				{
					// Token: 0x0400F0EC RID: 61676
					public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_fresh\">";

					// Token: 0x0400F0ED RID: 61677
					public static LocString NAME = "Fresh";

					// Token: 0x0400F0EE RID: 61678
					public static LocString TOOLTIP = "This food is fresh";
				}

				// Token: 0x02003CC0 RID: 15552
				public class STALE
				{
					// Token: 0x0400F0EF RID: 61679
					public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_stale\">";

					// Token: 0x0400F0F0 RID: 61680
					public static LocString NAME = "Stale";

					// Token: 0x0400F0F1 RID: 61681
					public static LocString TOOLTIP = "This food is still edible, but will soon expire";
				}

				// Token: 0x02003CC1 RID: 15553
				public class SPICEDFOOD
				{
					// Token: 0x0400F0F2 RID: 61682
					public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_spiced\">";

					// Token: 0x0400F0F3 RID: 61683
					public static LocString NAME = "Seasoned";

					// Token: 0x0400F0F4 RID: 61684
					public static LocString TOOLTIP = "This food has been improved with spice from the " + STRINGS.BUILDINGS.PREFABS.SPICEGRINDER.NAME;
				}

				// Token: 0x02003CC2 RID: 15554
				public class GERMS
				{
					// Token: 0x0400F0F5 RID: 61685
					public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_germs\">";

					// Token: 0x0400F0F6 RID: 61686
					public static LocString NAME = "{0} Germs";

					// Token: 0x0400F0F7 RID: 61687
					public static LocString TOOLTIP = "Contaminated with {0} germs";
				}
			}

			// Token: 0x0200308D RID: 12429
			public class STATS
			{
				// Token: 0x0400D197 RID: 53655
				public static LocString NAME = "Skills";

				// Token: 0x0400D198 RID: 53656
				public static LocString TOOLTIP = "<b>Skills</b>\nView this Duplicant's resume and attributes";

				// Token: 0x0400D199 RID: 53657
				public static LocString GROUPNAME_ATTRIBUTES = "ATTRIBUTES";

				// Token: 0x0400D19A RID: 53658
				public static LocString GROUPNAME_STRESS = "TODAY'S STRESS";

				// Token: 0x0400D19B RID: 53659
				public static LocString GROUPNAME_EXPECTATIONS = "EXPECTATIONS";

				// Token: 0x0400D19C RID: 53660
				public static LocString GROUPNAME_TRAITS = "TRAITS";
			}

			// Token: 0x0200308E RID: 12430
			public class SIMPLEINFO
			{
				// Token: 0x0400D19D RID: 53661
				public static LocString NAME = "Status";

				// Token: 0x0400D19E RID: 53662
				public static LocString TOOLTIP = "<b>Status</b>\nView current status";

				// Token: 0x0400D19F RID: 53663
				public static LocString GROUPNAME_STATUS = "STATUS";

				// Token: 0x0400D1A0 RID: 53664
				public static LocString GROUPNAME_DESCRIPTION = "INFORMATION";

				// Token: 0x0400D1A1 RID: 53665
				public static LocString GROUPNAME_CONDITION = "CONDITION";

				// Token: 0x0400D1A2 RID: 53666
				public static LocString GROUPNAME_REQUIREMENTS = "REQUIREMENTS";

				// Token: 0x0400D1A3 RID: 53667
				public static LocString GROUPNAME_EFFECTS = "EFFECTS";

				// Token: 0x0400D1A4 RID: 53668
				public static LocString GROUPNAME_RESEARCH = "RESEARCH";

				// Token: 0x0400D1A5 RID: 53669
				public static LocString GROUPNAME_LORE = "RECOVERED FILES";

				// Token: 0x0400D1A6 RID: 53670
				public static LocString GROUPNAME_FERTILITY = "EGG CHANCES";

				// Token: 0x0400D1A7 RID: 53671
				public static LocString GROUPNAME_MOO_FERTILITY = "SONG CHANCES";

				// Token: 0x0400D1A8 RID: 53672
				public static LocString GROUPNAME_ROCKET = "ROCKETRY";

				// Token: 0x0400D1A9 RID: 53673
				public static LocString GROUPNAME_CARGOBAY = "CARGO BAYS";

				// Token: 0x0400D1AA RID: 53674
				public static LocString GROUPNAME_ELEMENTS = "RESOURCES";

				// Token: 0x0400D1AB RID: 53675
				public static LocString GROUPNAME_LIFE = "LIFEFORMS";

				// Token: 0x0400D1AC RID: 53676
				public static LocString GROUPNAME_BIOMES = "BIOMES";

				// Token: 0x0400D1AD RID: 53677
				public static LocString GROUPNAME_GEYSERS = "GEYSERS";

				// Token: 0x0400D1AE RID: 53678
				public static LocString GROUPNAME_METEORSHOWERS = "METEOR SHOWERS";

				// Token: 0x0400D1AF RID: 53679
				public static LocString GROUPNAME_WORLDTRAITS = "WORLD TRAITS";

				// Token: 0x0400D1B0 RID: 53680
				public static LocString GROUPNAME_CLUSTER_POI = "POINT OF INTEREST";

				// Token: 0x0400D1B1 RID: 53681
				public static LocString GROUPNAME_MOVABLE = "MOVING";

				// Token: 0x0400D1B2 RID: 53682
				public static LocString NO_METEORSHOWERS = "No meteor showers forecasted";

				// Token: 0x0400D1B3 RID: 53683
				public static LocString NO_GEYSERS = "No geysers detected";

				// Token: 0x0400D1B4 RID: 53684
				public static LocString UNKNOWN_GEYSERS = "Unknown Geysers ({num})";
			}

			// Token: 0x0200308F RID: 12431
			public class DETAILS
			{
				// Token: 0x0400D1B5 RID: 53685
				public static LocString NAME = "Properties";

				// Token: 0x0400D1B6 RID: 53686
				public static LocString MINION_NAME = "About";

				// Token: 0x0400D1B7 RID: 53687
				public static LocString TOOLTIP = "<b>Properties</b>\nView elements, temperature, germs and more";

				// Token: 0x0400D1B8 RID: 53688
				public static LocString MINION_TOOLTIP = "More information";

				// Token: 0x0400D1B9 RID: 53689
				public static LocString GROUPNAME_DETAILS = "DETAILS";

				// Token: 0x0400D1BA RID: 53690
				public static LocString GROUPNAME_CONTENTS = "CONTENTS";

				// Token: 0x0400D1BB RID: 53691
				public static LocString GROUPNAME_MINION_CONTENTS = "CARRIED ITEMS";

				// Token: 0x0400D1BC RID: 53692
				public static LocString STORAGE_EMPTY = "None";

				// Token: 0x0400D1BD RID: 53693
				public static LocString CONTENTS_MASS = "{0}: {1}";

				// Token: 0x0400D1BE RID: 53694
				public static LocString CONTENTS_TEMPERATURE = "{0} at {1}";

				// Token: 0x0400D1BF RID: 53695
				public static LocString CONTENTS_ROTTABLE = "\n • {0}";

				// Token: 0x0400D1C0 RID: 53696
				public static LocString CONTENTS_DISEASED = "\n • {0}";

				// Token: 0x0400D1C1 RID: 53697
				public static LocString NET_STRESS = "<b>Today's Net Stress: {0}%</b>";

				// Token: 0x02003CC3 RID: 15555
				public class RADIATIONABSORPTIONFACTOR
				{
					// Token: 0x0400F0F8 RID: 61688
					public static LocString NAME = "Radiation Blocking: {0}";

					// Token: 0x0400F0F9 RID: 61689
					public static LocString TOOLTIP = "This object will block approximately {0} of radiation.";
				}
			}

			// Token: 0x02003090 RID: 12432
			public class PERSONALITY
			{
				// Token: 0x0400D1C2 RID: 53698
				public static LocString NAME = "Bio";

				// Token: 0x0400D1C3 RID: 53699
				public static LocString TOOLTIP = "<b>Bio</b>\nView this Duplicant's personality, skills, traits and amenities";

				// Token: 0x0400D1C4 RID: 53700
				public static LocString GROUPNAME_BIO = "ABOUT";

				// Token: 0x0400D1C5 RID: 53701
				public static LocString GROUPNAME_RESUME = "{0}'S RESUME";

				// Token: 0x02003CC4 RID: 15556
				public class RESUME
				{
					// Token: 0x0400F0FA RID: 61690
					public static LocString MASTERED_SKILLS = "<b><size=13>Learned Skills:</size></b>";

					// Token: 0x0400F0FB RID: 61691
					public static LocString MASTERED_SKILLS_TOOLTIP = string.Concat(new string[]
					{
						"All ",
						UI.PRE_KEYWORD,
						"Traits",
						UI.PST_KEYWORD,
						" and ",
						UI.PRE_KEYWORD,
						"Morale Needs",
						UI.PST_KEYWORD,
						" become permanent once a Duplicant has learned a new ",
						UI.PRE_KEYWORD,
						"Skill",
						UI.PST_KEYWORD,
						"\n\n",
						STRINGS.BUILDINGS.PREFABS.RESETSKILLSSTATION.NAME,
						"s can be built from the ",
						UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
						" to completely reset a Duplicant's learned ",
						UI.PRE_KEYWORD,
						"Skills",
						UI.PST_KEYWORD,
						", refunding all ",
						UI.PRE_KEYWORD,
						"Skill Points",
						UI.PST_KEYWORD
					});

					// Token: 0x0400F0FC RID: 61692
					public static LocString JOBTRAINING_TOOLTIP = string.Concat(new string[]
					{
						"{0} learned this ",
						UI.PRE_KEYWORD,
						"Skill",
						UI.PST_KEYWORD,
						" while working as a {1}"
					});

					// Token: 0x020040EF RID: 16623
					public class APTITUDES
					{
						// Token: 0x0400FAD7 RID: 64215
						public static LocString NAME = "<b><size=13>Personal Interests:</size></b>";

						// Token: 0x0400FAD8 RID: 64216
						public static LocString TOOLTIP = "{0} enjoys these types of work";
					}

					// Token: 0x020040F0 RID: 16624
					public class PERKS
					{
						// Token: 0x0400FAD9 RID: 64217
						public static LocString NAME = "<b><size=13>Skill Training:</size></b>";

						// Token: 0x0400FADA RID: 64218
						public static LocString TOOLTIP = "These are permanent skills {0} gained from learned skills";
					}

					// Token: 0x020040F1 RID: 16625
					public class CURRENT_ROLE
					{
						// Token: 0x0400FADB RID: 64219
						public static LocString NAME = "<size=13><b>Current Job:</b> {0}</size>";

						// Token: 0x0400FADC RID: 64220
						public static LocString TOOLTIP = "{0} is currently working as a {1}";

						// Token: 0x0400FADD RID: 64221
						public static LocString NOJOB_TOOLTIP = "This {0} is... \"between jobs\" at present";
					}

					// Token: 0x020040F2 RID: 16626
					public class NO_MASTERED_SKILLS
					{
						// Token: 0x0400FADE RID: 64222
						public static LocString NAME = "None";

						// Token: 0x0400FADF RID: 64223
						public static LocString TOOLTIP = string.Concat(new string[]
						{
							"{0} has not learned any ",
							UI.PRE_KEYWORD,
							"Skills",
							UI.PST_KEYWORD,
							" yet"
						});
					}
				}

				// Token: 0x02003CC5 RID: 15557
				public class EQUIPMENT
				{
					// Token: 0x0400F0FD RID: 61693
					public static LocString GROUPNAME_ROOMS = "AMENITIES";

					// Token: 0x0400F0FE RID: 61694
					public static LocString GROUPNAME_OWNABLE = "EQUIPMENT";

					// Token: 0x0400F0FF RID: 61695
					public static LocString NO_ASSIGNABLES = "None";

					// Token: 0x0400F100 RID: 61696
					public static LocString NO_ASSIGNABLES_TOOLTIP = "{0} has not been assigned any buildings of their own";

					// Token: 0x0400F101 RID: 61697
					public static LocString UNASSIGNED = "Unassigned";

					// Token: 0x0400F102 RID: 61698
					public static LocString UNASSIGNED_TOOLTIP = "This Duplicant has not been assigned a {0}";

					// Token: 0x0400F103 RID: 61699
					public static LocString ASSIGNED_TOOLTIP = "{2} has been assigned a {0}\n\nEffects: {1}";

					// Token: 0x0400F104 RID: 61700
					public static LocString NOEQUIPMENT = "None";

					// Token: 0x0400F105 RID: 61701
					public static LocString NOEQUIPMENT_TOOLTIP = "{0}'s wearing their Printday Suit and nothing more";
				}
			}

			// Token: 0x02003091 RID: 12433
			public class ENERGYCONSUMER
			{
				// Token: 0x0400D1C6 RID: 53702
				public static LocString NAME = "Energy";

				// Token: 0x0400D1C7 RID: 53703
				public static LocString TOOLTIP = "View how much power this building consumes";
			}

			// Token: 0x02003092 RID: 12434
			public class ENERGYWIRE
			{
				// Token: 0x0400D1C8 RID: 53704
				public static LocString NAME = "Energy";

				// Token: 0x0400D1C9 RID: 53705
				public static LocString TOOLTIP = "View this wire's network";
			}

			// Token: 0x02003093 RID: 12435
			public class ENERGYGENERATOR
			{
				// Token: 0x0400D1CA RID: 53706
				public static LocString NAME = "Energy";

				// Token: 0x0400D1CB RID: 53707
				public static LocString TOOLTIP = "<b>Energy</b>\nMonitor the power this building is generating";

				// Token: 0x0400D1CC RID: 53708
				public static LocString CIRCUITOVERVIEW = "CIRCUIT OVERVIEW";

				// Token: 0x0400D1CD RID: 53709
				public static LocString GENERATORS = "POWER GENERATORS";

				// Token: 0x0400D1CE RID: 53710
				public static LocString CONSUMERS = "POWER CONSUMERS";

				// Token: 0x0400D1CF RID: 53711
				public static LocString BATTERIES = "BATTERIES";

				// Token: 0x0400D1D0 RID: 53712
				public static LocString DISCONNECTED = "Not connected to an electrical circuit";

				// Token: 0x0400D1D1 RID: 53713
				public static LocString NOGENERATORS = "No generators on this circuit";

				// Token: 0x0400D1D2 RID: 53714
				public static LocString NOCONSUMERS = "No consumers on this circuit";

				// Token: 0x0400D1D3 RID: 53715
				public static LocString NOBATTERIES = "No batteries on this circuit";

				// Token: 0x0400D1D4 RID: 53716
				public static LocString AVAILABLE_JOULES = UI.FormatAsLink("Power", "POWER") + " stored: {0}";

				// Token: 0x0400D1D5 RID: 53717
				public static LocString AVAILABLE_JOULES_TOOLTIP = "Amount of power stored in batteries";

				// Token: 0x0400D1D6 RID: 53718
				public static LocString WATTAGE_GENERATED = UI.FormatAsLink("Power", "POWER") + " produced: {0}";

				// Token: 0x0400D1D7 RID: 53719
				public static LocString WATTAGE_GENERATED_TOOLTIP = "The total amount of power generated by this circuit";

				// Token: 0x0400D1D8 RID: 53720
				public static LocString WATTAGE_CONSUMED = UI.FormatAsLink("Power", "POWER") + " consumed: {0}";

				// Token: 0x0400D1D9 RID: 53721
				public static LocString WATTAGE_CONSUMED_TOOLTIP = "The total amount of power used by this circuit";

				// Token: 0x0400D1DA RID: 53722
				public static LocString POTENTIAL_WATTAGE_CONSUMED = "Potential power consumed: {0}";

				// Token: 0x0400D1DB RID: 53723
				public static LocString POTENTIAL_WATTAGE_CONSUMED_TOOLTIP = "The total amount of power that can be used by this circuit if all connected buildings are active";

				// Token: 0x0400D1DC RID: 53724
				public static LocString MAX_SAFE_WATTAGE = "Maximum safe wattage: {0}";

				// Token: 0x0400D1DD RID: 53725
				public static LocString MAX_SAFE_WATTAGE_TOOLTIP = "Exceeding this value will overload the circuit and can result in damage to wiring and buildings";
			}

			// Token: 0x02003094 RID: 12436
			public class DISEASE
			{
				// Token: 0x0400D1DE RID: 53726
				public static LocString NAME = "Germs";

				// Token: 0x0400D1DF RID: 53727
				public static LocString TOOLTIP = "<b>Germs</b>\nView germ resistance and risk of contagion";

				// Token: 0x0400D1E0 RID: 53728
				public static LocString DISEASE_SOURCE = "DISEASE SOURCE";

				// Token: 0x0400D1E1 RID: 53729
				public static LocString IMMUNE_SYSTEM = "GERM HOST";

				// Token: 0x0400D1E2 RID: 53730
				public static LocString CONTRACTION_RATES = "CONTRACTION RATES";

				// Token: 0x0400D1E3 RID: 53731
				public static LocString CURRENT_GERMS = "SURFACE GERMS";

				// Token: 0x0400D1E4 RID: 53732
				public static LocString NO_CURRENT_GERMS = "SURFACE GERMS";

				// Token: 0x0400D1E5 RID: 53733
				public static LocString GERMS_INFO = "GERM LIFE CYCLE";

				// Token: 0x0400D1E6 RID: 53734
				public static LocString INFECTION_INFO = "INFECTION DETAILS";

				// Token: 0x0400D1E7 RID: 53735
				public static LocString DISEASE_INFO_POPUP_HEADER = "DISEASE INFO: {0}";

				// Token: 0x0400D1E8 RID: 53736
				public static LocString DISEASE_INFO_POPUP_BUTTON = "FULL INFO";

				// Token: 0x0400D1E9 RID: 53737
				public static LocString DISEASE_INFO_POPUP_TOOLTIP = "View detailed germ and infection info for {0}";

				// Token: 0x02003CC6 RID: 15558
				public class DETAILS
				{
					// Token: 0x0400F106 RID: 61702
					public static LocString NODISEASE = "No surface germs";

					// Token: 0x0400F107 RID: 61703
					public static LocString NODISEASE_TOOLTIP = "There are no germs present on this object";

					// Token: 0x0400F108 RID: 61704
					public static LocString DISEASE_AMOUNT = "{0}: {1}";

					// Token: 0x0400F109 RID: 61705
					public static LocString DISEASE_AMOUNT_TOOLTIP = "{0} are present on the surface of the selected object";

					// Token: 0x0400F10A RID: 61706
					public static LocString DEATH_FORMAT = "{0} dead/cycle";

					// Token: 0x0400F10B RID: 61707
					public static LocString DEATH_FORMAT_TOOLTIP = "Germ count is being reduced by {0}/cycle";

					// Token: 0x0400F10C RID: 61708
					public static LocString GROWTH_FORMAT = "{0} spawned/cycle";

					// Token: 0x0400F10D RID: 61709
					public static LocString GROWTH_FORMAT_TOOLTIP = "Germ count is being increased by {0}/cycle";

					// Token: 0x0400F10E RID: 61710
					public static LocString NEUTRAL_FORMAT = "No change";

					// Token: 0x0400F10F RID: 61711
					public static LocString NEUTRAL_FORMAT_TOOLTIP = "Germ count is static";

					// Token: 0x020040F3 RID: 16627
					public class GROWTH_FACTORS
					{
						// Token: 0x0400FAE0 RID: 64224
						public static LocString TITLE = "\nGrowth factors:";

						// Token: 0x0400FAE1 RID: 64225
						public static LocString TOOLTIP = "These conditions are contributing to the multiplication of germs";

						// Token: 0x0400FAE2 RID: 64226
						public static LocString RATE_OF_CHANGE = "Change rate: {0}";

						// Token: 0x0400FAE3 RID: 64227
						public static LocString RATE_OF_CHANGE_TOOLTIP = "Germ count is fluctuating at a rate of {0}";

						// Token: 0x0400FAE4 RID: 64228
						public static LocString HALF_LIFE_NEG = "Half life: {0}";

						// Token: 0x0400FAE5 RID: 64229
						public static LocString HALF_LIFE_NEG_TOOLTIP = "In {0} the germ count on this object will be halved";

						// Token: 0x0400FAE6 RID: 64230
						public static LocString HALF_LIFE_POS = "Doubling time: {0}";

						// Token: 0x0400FAE7 RID: 64231
						public static LocString HALF_LIFE_POS_TOOLTIP = "In {0} the germ count on this object will be doubled";

						// Token: 0x0400FAE8 RID: 64232
						public static LocString HALF_LIFE_NEUTRAL = "Static";

						// Token: 0x0400FAE9 RID: 64233
						public static LocString HALF_LIFE_NEUTRAL_TOOLTIP = "The germ count is neither increasing nor decreasing";

						// Token: 0x02004127 RID: 16679
						public class SUBSTRATE
						{
							// Token: 0x0400FB28 RID: 64296
							public static LocString GROW = "    • Growing on {0}: {1}";

							// Token: 0x0400FB29 RID: 64297
							public static LocString GROW_TOOLTIP = "Contact with this substance is causing germs to multiply";

							// Token: 0x0400FB2A RID: 64298
							public static LocString NEUTRAL = "    • No change on {0}";

							// Token: 0x0400FB2B RID: 64299
							public static LocString NEUTRAL_TOOLTIP = "Contact with this substance has no effect on germ count";

							// Token: 0x0400FB2C RID: 64300
							public static LocString DIE = "    • Dying on {0}: {1}";

							// Token: 0x0400FB2D RID: 64301
							public static LocString DIE_TOOLTIP = "Contact with this substance is causing germs to die off";
						}

						// Token: 0x02004128 RID: 16680
						public class ENVIRONMENT
						{
							// Token: 0x0400FB2E RID: 64302
							public static LocString TITLE = "    • Surrounded by {0}: {1}";

							// Token: 0x0400FB2F RID: 64303
							public static LocString GROW_TOOLTIP = "This atmosphere is causing germs to multiply";

							// Token: 0x0400FB30 RID: 64304
							public static LocString DIE_TOOLTIP = "This atmosphere is causing germs to die off";
						}

						// Token: 0x02004129 RID: 16681
						public class TEMPERATURE
						{
							// Token: 0x0400FB31 RID: 64305
							public static LocString TITLE = "    • Current temperature {0}: {1}";

							// Token: 0x0400FB32 RID: 64306
							public static LocString GROW_TOOLTIP = "This temperature is allowing germs to multiply";

							// Token: 0x0400FB33 RID: 64307
							public static LocString DIE_TOOLTIP = "This temperature is causing germs to die off";
						}

						// Token: 0x0200412A RID: 16682
						public class PRESSURE
						{
							// Token: 0x0400FB34 RID: 64308
							public static LocString TITLE = "    • Current pressure {0}: {1}";

							// Token: 0x0400FB35 RID: 64309
							public static LocString GROW_TOOLTIP = "Atmospheric pressure is causing germs to multiply";

							// Token: 0x0400FB36 RID: 64310
							public static LocString DIE_TOOLTIP = "Atmospheric pressure is causing germs to die off";
						}

						// Token: 0x0200412B RID: 16683
						public class RADIATION
						{
							// Token: 0x0400FB37 RID: 64311
							public static LocString TITLE = "    • Exposed to {0} Rads: {1}";

							// Token: 0x0400FB38 RID: 64312
							public static LocString DIE_TOOLTIP = "Radiation exposure is causing germs to die off";
						}

						// Token: 0x0200412C RID: 16684
						public class DYING_OFF
						{
							// Token: 0x0400FB39 RID: 64313
							public static LocString TITLE = "    • <b>Dying off: {0}</b>";

							// Token: 0x0400FB3A RID: 64314
							public static LocString TOOLTIP = "Low germ count in this area is causing germs to die rapidly\n\nFewer than {0} are on this {1} of material.\n({2} germs/" + UI.UNITSUFFIXES.MASS.KILOGRAM + ")";
						}

						// Token: 0x0200412D RID: 16685
						public class OVERPOPULATED
						{
							// Token: 0x0400FB3B RID: 64315
							public static LocString TITLE = "    • <b>Overpopulated: {0}</b>";

							// Token: 0x0400FB3C RID: 64316
							public static LocString TOOLTIP = "Too many germs are present in this area, resulting in rapid die-off until the population stabilizes\n\nA maximum of {0} can be on this {1} of material.\n({2} germs/" + UI.UNITSUFFIXES.MASS.KILOGRAM + ")";
						}
					}
				}
			}

			// Token: 0x02003095 RID: 12437
			public class NEEDS
			{
				// Token: 0x0400D1EA RID: 53738
				public static LocString NAME = "Stress";

				// Token: 0x0400D1EB RID: 53739
				public static LocString TOOLTIP = "View this Duplicant's psychological status";

				// Token: 0x0400D1EC RID: 53740
				public static LocString CURRENT_STRESS_LEVEL = "Current " + UI.FormatAsLink("Stress", "STRESS") + " Level: {0}";

				// Token: 0x0400D1ED RID: 53741
				public static LocString OVERVIEW = "Overview";

				// Token: 0x0400D1EE RID: 53742
				public static LocString STRESS_CREATORS = UI.FormatAsLink("Stress", "STRESS") + " Creators";

				// Token: 0x0400D1EF RID: 53743
				public static LocString STRESS_RELIEVERS = UI.FormatAsLink("Stress", "STRESS") + " Relievers";

				// Token: 0x0400D1F0 RID: 53744
				public static LocString CURRENT_NEED_LEVEL = "Current Level: {0}";

				// Token: 0x0400D1F1 RID: 53745
				public static LocString NEXT_NEED_LEVEL = "Next Level: {0}";
			}

			// Token: 0x02003096 RID: 12438
			public class EGG_CHANCES
			{
				// Token: 0x0400D1F2 RID: 53746
				public static LocString CHANCE_FORMAT = "{0}: {1}";

				// Token: 0x0400D1F3 RID: 53747
				public static LocString CHANCE_FORMAT_TOOLTIP = "This critter has a {1} chance of laying {0}s.\n\nThis probability increases when the creature:\n{2}";

				// Token: 0x0400D1F4 RID: 53748
				public static LocString CHANCE_MOD_FORMAT = "    • {0}\n";

				// Token: 0x0400D1F5 RID: 53749
				public static LocString CHANCE_FORMAT_TOOLTIP_NOMOD = "This critter has a {1} chance of laying {0}s.";
			}

			// Token: 0x02003097 RID: 12439
			public class MOO_SONG_CHANCES
			{
				// Token: 0x0400D1F6 RID: 53750
				public static LocString CHANCE_FORMAT = "{0}: {1}";

				// Token: 0x0400D1F7 RID: 53751
				public static LocString CHANCE_FORMAT_TOOLTIP = "This critter has a {1} chance of calling {0}s\n\nThis probability increases when the creature:\n{2}";

				// Token: 0x0400D1F8 RID: 53752
				public static LocString CHANCE_MOD_FORMAT = "    • {0}\n";

				// Token: 0x0400D1F9 RID: 53753
				public static LocString CHANCE_FORMAT_TOOLTIP_NOMOD = "This critter has a {1} chance of calling {0}s";
			}

			// Token: 0x02003098 RID: 12440
			public class BUILDING_CHORES
			{
				// Token: 0x0400D1FA RID: 53754
				public static LocString NAME = "Errands";

				// Token: 0x0400D1FB RID: 53755
				public static LocString TOOLTIP = "<b>Errands</b>\nView available errands and current queue";

				// Token: 0x0400D1FC RID: 53756
				public static LocString CHORE_TYPE_TOOLTIP = "Errand Type: {0}";

				// Token: 0x0400D1FD RID: 53757
				public static LocString AVAILABLE_CHORES = "AVAILABLE ERRANDS";

				// Token: 0x0400D1FE RID: 53758
				public static LocString DUPE_TOOLTIP_FAILED = "{Name} cannot currently {Errand}\n\nReason:\n{FailedPrecondition}";

				// Token: 0x0400D1FF RID: 53759
				public static LocString DUPE_TOOLTIP_SUCCEEDED = "{Description}\n\n{Errand}'s Type: {Groups}\n\n{Name}'s {BestGroup} Priority: {PersonalPriorityValue} ({PersonalPriority})\n{Building} Priority: {BuildingPriority}\nAll {BestGroup} Errands: {TypePriority}\n\nTotal Priority: {TotalPriority}";

				// Token: 0x0400D200 RID: 53760
				public static LocString DUPE_TOOLTIP_DESC_ACTIVE = "{Name} is currently busy: \"{Errand}\"";

				// Token: 0x0400D201 RID: 53761
				public static LocString DUPE_TOOLTIP_DESC_INACTIVE = "\"{Errand}\" is #{Rank} on {Name}'s To Do list, after they finish their current errand";
			}

			// Token: 0x02003099 RID: 12441
			public class PROCESS_CONDITIONS
			{
				// Token: 0x0400D202 RID: 53762
				public static LocString NAME = "LAUNCH CHECKLIST";

				// Token: 0x0400D203 RID: 53763
				public static LocString ROCKETPREP = "Rocket Construction";

				// Token: 0x0400D204 RID: 53764
				public static LocString ROCKETPREP_TOOLTIP = "It is recommended that all boxes on the Rocket Construction checklist be ticked before launching";

				// Token: 0x0400D205 RID: 53765
				public static LocString ROCKETSTORAGE = "Cargo Manifest";

				// Token: 0x0400D206 RID: 53766
				public static LocString ROCKETSTORAGE_TOOLTIP = "It is recommended that all boxes on the Cargo Manifest checklist be ticked before launching";

				// Token: 0x0400D207 RID: 53767
				public static LocString ROCKETFLIGHT = "Flight Route";

				// Token: 0x0400D208 RID: 53768
				public static LocString ROCKETFLIGHT_TOOLTIP = "A rocket requires a clear path to a set destination to conduct a mission";

				// Token: 0x0400D209 RID: 53769
				public static LocString ROCKETBOARD = "Crew Manifest";

				// Token: 0x0400D20A RID: 53770
				public static LocString ROCKETBOARD_TOOLTIP = "It is recommended that all boxes on the Crew Manifest checklist be ticked before launching";

				// Token: 0x0400D20B RID: 53771
				public static LocString ALL = "Requirements";

				// Token: 0x0400D20C RID: 53772
				public static LocString ALL_TOOLTIP = "These conditions must be fulfilled in order to launch a rocket mission";
			}

			// Token: 0x0200309A RID: 12442
			public class COSMETICS
			{
				// Token: 0x0400D20D RID: 53773
				public static LocString NAME = "Blueprint";

				// Token: 0x0400D20E RID: 53774
				public static LocString TOOLTIP = "<b>Blueprint</b>\nView and change assigned blueprints";
			}

			// Token: 0x0200309B RID: 12443
			public class MATERIAL
			{
				// Token: 0x0400D20F RID: 53775
				public static LocString NAME = "Material";

				// Token: 0x0400D210 RID: 53776
				public static LocString TOOLTIP = "<b>Material</b>\nView and change this building's construction material";

				// Token: 0x0400D211 RID: 53777
				public static LocString SUB_HEADER_CURRENT_MATERIAL = "CURRENT MATERIAL";

				// Token: 0x0400D212 RID: 53778
				public static LocString BUTTON_CHANGE_MATERIAL = "Change Material";
			}

			// Token: 0x0200309C RID: 12444
			public class CONFIGURATION
			{
				// Token: 0x0400D213 RID: 53779
				public static LocString NAME = "Config";

				// Token: 0x0400D214 RID: 53780
				public static LocString TOOLTIP = "<b>Config</b>\nView and change filters, recipes, production orders and more";

				// Token: 0x0400D215 RID: 53781
				public static LocString TOOLTIP_DUPLICANT = "<b>Config</b>\nView and change assigned equipment and amenities";
			}
		}

		// Token: 0x0200254F RID: 9551
		public class BUILDMENU
		{
			// Token: 0x0400A7A3 RID: 42915
			public static LocString GRID_VIEW_TOGGLE_TOOLTIP = "Toggle Grid View";

			// Token: 0x0400A7A4 RID: 42916
			public static LocString LIST_VIEW_TOGGLE_TOOLTIP = "Toggle List View";

			// Token: 0x0400A7A5 RID: 42917
			public static LocString NO_SEARCH_RESULTS = "NO RESULTS FOUND";

			// Token: 0x0400A7A6 RID: 42918
			public static LocString SEARCH_RESULTS_HEADER = "SEARCH RESULTS";

			// Token: 0x0400A7A7 RID: 42919
			public static LocString SEARCH_TEXT_PLACEHOLDER = "Search all buildings...";

			// Token: 0x0400A7A8 RID: 42920
			public static LocString SEARCH_TOOLTIP = "Search all buildings {Hotkey}";

			// Token: 0x0400A7A9 RID: 42921
			public static LocString CLEAR_SEARCH_TOOLTIP = "Clear search";
		}

		// Token: 0x02002550 RID: 9552
		public class BUILDINGEFFECTS
		{
			// Token: 0x0400A7AA RID: 42922
			public static LocString OPERATIONREQUIREMENTS = "<b>Requirements:</b>";

			// Token: 0x0400A7AB RID: 42923
			public static LocString REQUIRESPOWER = UI.FormatAsLink("Power", "POWER") + ": {0}";

			// Token: 0x0400A7AC RID: 42924
			public static LocString REQUIRESELEMENT = "Supply of {0}";

			// Token: 0x0400A7AD RID: 42925
			public static LocString REQUIRESLIQUIDINPUT = UI.FormatAsLink("Liquid Intake Pipe", "LIQUIDPIPING");

			// Token: 0x0400A7AE RID: 42926
			public static LocString REQUIRESLIQUIDOUTPUT = UI.FormatAsLink("Liquid Output Pipe", "LIQUIDPIPING");

			// Token: 0x0400A7AF RID: 42927
			public static LocString REQUIRESLIQUIDOUTPUTS = "Two " + UI.FormatAsLink("Liquid Output Pipes", "LIQUIDPIPING");

			// Token: 0x0400A7B0 RID: 42928
			public static LocString REQUIRESGASINPUT = UI.FormatAsLink("Gas Intake Pipe", "GASPIPING");

			// Token: 0x0400A7B1 RID: 42929
			public static LocString REQUIRESGASOUTPUT = UI.FormatAsLink("Gas Output Pipe", "GASPIPING");

			// Token: 0x0400A7B2 RID: 42930
			public static LocString REQUIRESGASOUTPUTS = "Two " + UI.FormatAsLink("Gas Output Pipes", "GASPIPING");

			// Token: 0x0400A7B3 RID: 42931
			public static LocString REQUIRESMANUALOPERATION = "Duplicant operation";

			// Token: 0x0400A7B4 RID: 42932
			public static LocString REQUIRESSKILLEDOPERATION = "Skilled Duplicant operation";

			// Token: 0x0400A7B5 RID: 42933
			public static LocString REQUIRESSKILLEDOPERATION_DLC3 = "Skilled Duplicant operation";

			// Token: 0x0400A7B6 RID: 42934
			public static LocString REQUIRESCREATIVITY = "Duplicant " + UI.FormatAsLink("Creativity", "ARTING1");

			// Token: 0x0400A7B7 RID: 42935
			public static LocString REQUIRESPOWERGENERATOR = UI.FormatAsLink("Power", "POWER") + " generator";

			// Token: 0x0400A7B8 RID: 42936
			public static LocString REQUIRESSEED = "1 Unplanted " + UI.FormatAsLink("Seed", "PLANTS");

			// Token: 0x0400A7B9 RID: 42937
			public static LocString PREFERS_ROOM = "Preferred Room: {0}";

			// Token: 0x0400A7BA RID: 42938
			public static LocString REQUIRESROOM = "Dedicated Room: {0}";

			// Token: 0x0400A7BB RID: 42939
			public static LocString ALLOWS_FERTILIZER = "Plant " + UI.FormatAsLink("Fertilization", "WILTCONDITIONS");

			// Token: 0x0400A7BC RID: 42940
			public static LocString ALLOWS_IRRIGATION = "Plant " + UI.FormatAsLink("Liquid", "WILTCONDITIONS");

			// Token: 0x0400A7BD RID: 42941
			public static LocString ASSIGNEDDUPLICANT = "Duplicant assignment";

			// Token: 0x0400A7BE RID: 42942
			public static LocString CONSUMESANYELEMENT = "Any Element";

			// Token: 0x0400A7BF RID: 42943
			public static LocString ENABLESDOMESTICGROWTH = "Enables " + UI.FormatAsLink("Plant Domestication", "PLANTS");

			// Token: 0x0400A7C0 RID: 42944
			public static LocString TRANSFORMER_INPUT_WIRE = "Input " + UI.FormatAsLink("Power Wire", "WIRE");

			// Token: 0x0400A7C1 RID: 42945
			public static LocString TRANSFORMER_OUTPUT_WIRE = "Output " + UI.FormatAsLink("Power Wire", "WIRE") + " (Limited to {0})";

			// Token: 0x0400A7C2 RID: 42946
			public static LocString OPERATIONEFFECTS = "<b>Effects:</b>";

			// Token: 0x0400A7C3 RID: 42947
			public static LocString BATTERYCAPACITY = UI.FormatAsLink("Power", "POWER") + " capacity: {0}";

			// Token: 0x0400A7C4 RID: 42948
			public static LocString BATTERYLEAK = UI.FormatAsLink("Power", "POWER") + " leak: {0}";

			// Token: 0x0400A7C5 RID: 42949
			public static LocString STORAGECAPACITY = "Storage capacity: {0}";

			// Token: 0x0400A7C6 RID: 42950
			public static LocString ELEMENTEMITTED_INPUTTEMP = "{0}: {1}";

			// Token: 0x0400A7C7 RID: 42951
			public static LocString ELEMENTEMITTED_ENTITYTEMP = "{0}: {1}";

			// Token: 0x0400A7C8 RID: 42952
			public static LocString ELEMENTEMITTED_MINORENTITYTEMP = "{0}: {1}";

			// Token: 0x0400A7C9 RID: 42953
			public static LocString ELEMENTEMITTED_MINTEMP = "{0}: {1}";

			// Token: 0x0400A7CA RID: 42954
			public static LocString ELEMENTEMITTED_FIXEDTEMP = "{0}: {1}";

			// Token: 0x0400A7CB RID: 42955
			public static LocString ELEMENTCONSUMED = "{0}: {1}";

			// Token: 0x0400A7CC RID: 42956
			public static LocString ELEMENTEMITTED_TOILET = "{0}: {1} per use";

			// Token: 0x0400A7CD RID: 42957
			public static LocString ELEMENTEMITTEDPERUSE = "{0}: {1} per use";

			// Token: 0x0400A7CE RID: 42958
			public static LocString DISEASEEMITTEDPERUSE = "{0}: {1} per use";

			// Token: 0x0400A7CF RID: 42959
			public static LocString DISEASECONSUMEDPERUSE = "All Diseases: -{0} per use";

			// Token: 0x0400A7D0 RID: 42960
			public static LocString ELEMENTCONSUMEDPERUSE = "{0}: {1} per use";

			// Token: 0x0400A7D1 RID: 42961
			public static LocString ENERGYCONSUMED = UI.FormatAsLink("Power", "POWER") + " consumed: {0}";

			// Token: 0x0400A7D2 RID: 42962
			public static LocString ENERGYGENERATED = UI.FormatAsLink("Power", "POWER") + ": +{0}";

			// Token: 0x0400A7D3 RID: 42963
			public static LocString HEATGENERATED = UI.FormatAsLink("Heat", "HEAT") + ": +{0}/s";

			// Token: 0x0400A7D4 RID: 42964
			public static LocString HEATCONSUMED = UI.FormatAsLink("Heat", "HEAT") + ": -{0}/s";

			// Token: 0x0400A7D5 RID: 42965
			public static LocString HEATER_TARGETTEMPERATURE = "Target " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400A7D6 RID: 42966
			public static LocString HEATGENERATED_AIRCONDITIONER = UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)";

			// Token: 0x0400A7D7 RID: 42967
			public static LocString HEATGENERATED_LIQUIDCONDITIONER = UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)";

			// Token: 0x0400A7D8 RID: 42968
			public static LocString FABRICATES = "Fabricates";

			// Token: 0x0400A7D9 RID: 42969
			public static LocString FABRICATEDITEM = "{1}";

			// Token: 0x0400A7DA RID: 42970
			public static LocString PROCESSES = "Refines:";

			// Token: 0x0400A7DB RID: 42971
			public static LocString PROCESSEDITEM = "{1} {0}";

			// Token: 0x0400A7DC RID: 42972
			public static LocString PLANTERBOX_PENTALTY = "Planter box penalty";

			// Token: 0x0400A7DD RID: 42973
			public static LocString DECORPROVIDED = UI.FormatAsLink("Decor", "DECOR") + ": {1} (Radius: {2} tiles)";

			// Token: 0x0400A7DE RID: 42974
			public static LocString OVERHEAT_TEMP = "Overheat " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400A7DF RID: 42975
			public static LocString MINIMUM_TEMP = "Freeze " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400A7E0 RID: 42976
			public static LocString OVER_PRESSURE_MASS = "Overpressure: {0}";

			// Token: 0x0400A7E1 RID: 42977
			public static LocString REFILLOXYGENTANK = "Refills Exosuit " + STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK.NAME;

			// Token: 0x0400A7E2 RID: 42978
			public static LocString DUPLICANTMOVEMENTBOOST = "Runspeed: {0}";

			// Token: 0x0400A7E3 RID: 42979
			public static LocString ELECTROBANKS = UI.FormatAsLink("Charge", "POWER") + ": {0}";

			// Token: 0x0400A7E4 RID: 42980
			public static LocString STRESSREDUCEDPERMINUTE = UI.FormatAsLink("Stress", "STRESS") + ": {0} per minute";

			// Token: 0x0400A7E5 RID: 42981
			public static LocString REMOVESEFFECTSUBTITLE = "Cures";

			// Token: 0x0400A7E6 RID: 42982
			public static LocString REMOVEDEFFECT = "{0}";

			// Token: 0x0400A7E7 RID: 42983
			public static LocString ADDED_EFFECT = "Added Effect: {0}";

			// Token: 0x0400A7E8 RID: 42984
			public static LocString GASCOOLING = UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}";

			// Token: 0x0400A7E9 RID: 42985
			public static LocString LIQUIDCOOLING = UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}";

			// Token: 0x0400A7EA RID: 42986
			public static LocString MAX_WATTAGE = "Max " + UI.FormatAsLink("Power", "POWER") + ": {0}";

			// Token: 0x0400A7EB RID: 42987
			public static LocString MAX_BITS = UI.FormatAsLink("Bit", "LOGIC") + " Depth: {0}";

			// Token: 0x0400A7EC RID: 42988
			public static LocString RESEARCH_MATERIALS = "{0}: {1} per " + UI.FormatAsLink("Research", "RESEARCH") + " point";

			// Token: 0x0400A7ED RID: 42989
			public static LocString PRODUCES_RESEARCH_POINTS = "{0}";

			// Token: 0x0400A7EE RID: 42990
			public static LocString HIT_POINTS_PER_CYCLE = UI.FormatAsLink("Health", "Health") + " per cycle: {0}";

			// Token: 0x0400A7EF RID: 42991
			public static LocString KCAL_PER_CYCLE = UI.FormatAsLink("KCal", "FOOD") + " per cycle: {0}";

			// Token: 0x0400A7F0 RID: 42992
			public static LocString REMOVES_DISEASE = "Kills germs";

			// Token: 0x0400A7F1 RID: 42993
			public static LocString DOCTORING = "Doctoring";

			// Token: 0x0400A7F2 RID: 42994
			public static LocString RECREATION = "Recreation";

			// Token: 0x0400A7F3 RID: 42995
			public static LocString COOLANT = "Coolant: {1} {0}";

			// Token: 0x0400A7F4 RID: 42996
			public static LocString REFINEMENT_ENERGY = "Heat: {0}";

			// Token: 0x0400A7F5 RID: 42997
			public static LocString IMPROVED_BUILDINGS = "Improved Buildings";

			// Token: 0x0400A7F6 RID: 42998
			public static LocString IMPROVED_PLANTS = "Improved Plants";

			// Token: 0x0400A7F7 RID: 42999
			public static LocString IMPROVED_BUILDINGS_ITEM = "{0}";

			// Token: 0x0400A7F8 RID: 43000
			public static LocString IMPROVED_PLANTS_ITEM = "{0}";

			// Token: 0x0400A7F9 RID: 43001
			public static LocString GEYSER_PRODUCTION = "{0}: {1} at {2}";

			// Token: 0x0400A7FA RID: 43002
			public static LocString GEYSER_DISEASE = "Germs: {0}";

			// Token: 0x0400A7FB RID: 43003
			public static LocString GEYSER_PERIOD = "Eruption Period: {0} every {1}";

			// Token: 0x0400A7FC RID: 43004
			public static LocString GEYSER_YEAR_UNSTUDIED = "Active Period: (Requires Analysis)";

			// Token: 0x0400A7FD RID: 43005
			public static LocString GEYSER_YEAR_PERIOD = "Active Period: {0} every {1}";

			// Token: 0x0400A7FE RID: 43006
			public static LocString GEYSER_YEAR_NEXT_ACTIVE = "Next Activity: {0}";

			// Token: 0x0400A7FF RID: 43007
			public static LocString GEYSER_YEAR_NEXT_DORMANT = "Next Dormancy: {0}";

			// Token: 0x0400A800 RID: 43008
			public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = "Average Output: (Requires Analysis)";

			// Token: 0x0400A801 RID: 43009
			public static LocString GEYSER_YEAR_AVR_OUTPUT = "Average Output: {0}";

			// Token: 0x0400A802 RID: 43010
			public static LocString CAPTURE_METHOD_WRANGLE = "Capture Method: Wrangling";

			// Token: 0x0400A803 RID: 43011
			public static LocString CAPTURE_METHOD_FLYING_TRAP = "Capture Method: Airborne Critter Trap";

			// Token: 0x0400A804 RID: 43012
			public static LocString CAPTURE_METHOD_LAND_TRAP = "Capture Method: Critter Trap";

			// Token: 0x0400A805 RID: 43013
			public static LocString CAPTURE_METHOD_FISH_TRAP = "Capture Method: Fish Trap";

			// Token: 0x0400A806 RID: 43014
			public static LocString DIET_HEADER = "Digestion:";

			// Token: 0x0400A807 RID: 43015
			public static LocString DIET_CONSUMED = "    • Diet: {Foodlist}";

			// Token: 0x0400A808 RID: 43016
			public static LocString DIET_STORED = "    • Stores: {Foodlist}";

			// Token: 0x0400A809 RID: 43017
			public static LocString DIET_CONSUMED_ITEM = "{Food}: {Amount}";

			// Token: 0x0400A80A RID: 43018
			public static LocString DIET_PRODUCED = "    • Excretion: {Items}";

			// Token: 0x0400A80B RID: 43019
			public static LocString DIET_PRODUCED_ITEM = "{Item}: {Percent} of consumed mass";

			// Token: 0x0400A80C RID: 43020
			public static LocString DIET_PRODUCED_ITEM_FROM_PLANT = "{Item}: {Amount} when properly fed";

			// Token: 0x0400A80D RID: 43021
			public static LocString DIET_ADDITIONAL_PRODUCED = "Secondary Excretion: {Items}";

			// Token: 0x0400A80E RID: 43022
			public static LocString SCALE_GROWTH = "Shearable {Item}: {Amount} per {Time}";

			// Token: 0x0400A80F RID: 43023
			public static LocString SCALE_GROWTH_ATMO = "Shearable {Item}: {Amount} per {Time} ({Atmosphere})";

			// Token: 0x0400A810 RID: 43024
			public static LocString SCALE_GROWTH_TEMP = "Shearable {Item}: {Amount} per {Time} ({TempMin} - {TempMax})";

			// Token: 0x0400A811 RID: 43025
			public static LocString ACCESS_CONTROL = "Duplicant Access Permissions";

			// Token: 0x0400A812 RID: 43026
			public static LocString ROCKETRESTRICTION_HEADER = "Restriction Control:";

			// Token: 0x0400A813 RID: 43027
			public static LocString ROCKETRESTRICTION_BUILDINGS = "    • Buildings: {buildinglist}";

			// Token: 0x0400A814 RID: 43028
			public static LocString UNSTABLEENTOMBDEFENSEREADY = "Entomb Defense: Ready";

			// Token: 0x0400A815 RID: 43029
			public static LocString UNSTABLEENTOMBDEFENSETHREATENED = "Entomb Defense: Threatened";

			// Token: 0x0400A816 RID: 43030
			public static LocString UNSTABLEENTOMBDEFENSEREACTING = "Entomb Defense: Reacting";

			// Token: 0x0400A817 RID: 43031
			public static LocString UNSTABLEENTOMBDEFENSEOFF = "Entomb Defense: Off";

			// Token: 0x0400A818 RID: 43032
			public static LocString ITEM_TEMPERATURE_ADJUST = "Stored " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400A819 RID: 43033
			public static LocString NOISE_CREATED = UI.FormatAsLink("Noise", "SOUND") + ": {0} dB (Radius: {1} tiles)";

			// Token: 0x0400A81A RID: 43034
			public static LocString MESS_TABLE_SALT = "Table Salt: +{0}";

			// Token: 0x0400A81B RID: 43035
			public static LocString COMMUNAL_DINING = "Communal Dining: +{0}";

			// Token: 0x0400A81C RID: 43036
			public static LocString ACTIVE_PARTICLE_CONSUMPTION = "Radbolts: {Rate}";

			// Token: 0x0400A81D RID: 43037
			public static LocString PARTICLE_PORT_INPUT = "Radbolt Input Port";

			// Token: 0x0400A81E RID: 43038
			public static LocString PARTICLE_PORT_OUTPUT = "Radbolt Output Port";

			// Token: 0x0400A81F RID: 43039
			public static LocString IN_ORBIT_REQUIRED = "Active In Space";

			// Token: 0x0400A820 RID: 43040
			public static LocString KETTLE_MELT_RATE = "Melting Rate: {0}";

			// Token: 0x0400A821 RID: 43041
			public static LocString FOOD_DEHYDRATOR_WATER_OUTPUT = "Wet Floor";

			// Token: 0x0200309D RID: 12445
			public class TOOLTIPS
			{
				// Token: 0x0400D216 RID: 53782
				public static LocString OPERATIONREQUIREMENTS = "All requirements must be met in order for this building to operate";

				// Token: 0x0400D217 RID: 53783
				public static LocString REQUIRESPOWER = string.Concat(new string[]
				{
					"Must be connected to a power grid with at least ",
					UI.FormatAsNegativeRate("{0}"),
					" of available ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D218 RID: 53784
				public static LocString REQUIRESELEMENT = string.Concat(new string[]
				{
					"Must receive deliveries of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" to function"
				});

				// Token: 0x0400D219 RID: 53785
				public static LocString REQUIRESLIQUIDINPUT = string.Concat(new string[]
				{
					"Must receive ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" from a ",
					STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400D21A RID: 53786
				public static LocString REQUIRESLIQUIDOUTPUT = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400D21B RID: 53787
				public static LocString REQUIRESLIQUIDOUTPUTS = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400D21C RID: 53788
				public static LocString REQUIRESGASINPUT = string.Concat(new string[]
				{
					"Must receive ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" from a ",
					STRINGS.BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400D21D RID: 53789
				public static LocString REQUIRESGASOUTPUT = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400D21E RID: 53790
				public static LocString REQUIRESGASOUTPUTS = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400D21F RID: 53791
				public static LocString REQUIRESMANUALOPERATION = "A Duplicant must be present to run this building";

				// Token: 0x0400D220 RID: 53792
				public static LocString REQUIRESSKILLEDOPERATION = "Only a Duplicant with the {Skill} skill can use this building";

				// Token: 0x0400D221 RID: 53793
				public static LocString REQUIRESSKILLEDOPERATION_DLC3 = "Only a Duplicant with the {Skill} skill or {Booster} can use this building";

				// Token: 0x0400D222 RID: 53794
				public static LocString REQUIRESCREATIVITY = "An expressive Duplicant must work on this object to create " + UI.PRE_KEYWORD + "Art" + UI.PST_KEYWORD;

				// Token: 0x0400D223 RID: 53795
				public static LocString REQUIRESPOWERGENERATOR = string.Concat(new string[]
				{
					"Must be connected to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" producing generator to function"
				});

				// Token: 0x0400D224 RID: 53796
				public static LocString REQUIRESSEED = "Must receive a plant " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;

				// Token: 0x0400D225 RID: 53797
				public static LocString PREFERS_ROOM = "This building gains additional effects or functionality when built inside a " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

				// Token: 0x0400D226 RID: 53798
				public static LocString REQUIRESROOM = string.Concat(new string[]
				{
					"Must be built within a dedicated ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					"\n\n",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" will become a ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" after construction"
				});

				// Token: 0x0400D227 RID: 53799
				public static LocString ALLOWS_FERTILIZER = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Fertilizer",
					UI.PST_KEYWORD,
					" to be delivered to plants"
				});

				// Token: 0x0400D228 RID: 53800
				public static LocString ALLOWS_IRRIGATION = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to be delivered to plants"
				});

				// Token: 0x0400D229 RID: 53801
				public static LocString ALLOWS_IRRIGATION_PIPE = string.Concat(new string[]
				{
					"Allows irrigation ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" connection"
				});

				// Token: 0x0400D22A RID: 53802
				public static LocString ASSIGNEDDUPLICANT = "This amenity may only be used by the Duplicant it is assigned to";

				// Token: 0x0400D22B RID: 53803
				public static LocString BUILDINGROOMREQUIREMENTCLASS = "This category of building may be required or prohibited in certain " + UI.PRE_KEYWORD + "Rooms" + UI.PST_KEYWORD;

				// Token: 0x0400D22C RID: 53804
				public static LocString OPERATIONEFFECTS = "The building will produce these effects when its requirements are met";

				// Token: 0x0400D22D RID: 53805
				public static LocString BATTERYCAPACITY = string.Concat(new string[]
				{
					"Can hold <b>{0}</b> of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" when connected to a ",
					UI.PRE_KEYWORD,
					"Generator",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D22E RID: 53806
				public static LocString BATTERYLEAK = string.Concat(new string[]
				{
					UI.FormatAsNegativeRate("{0}"),
					" of this battery's charge will be lost as ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D22F RID: 53807
				public static LocString STORAGECAPACITY = "Holds up to <b>{0}</b> of material";

				// Token: 0x0400D230 RID: 53808
				public static LocString ELEMENTEMITTED_INPUTTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be the combined ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the input materials."
				});

				// Token: 0x0400D231 RID: 53809
				public static LocString ELEMENTEMITTED_ENTITYTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the building at the time of production"
				});

				// Token: 0x0400D232 RID: 53810
				public static LocString ELEMENTEMITTED_MINORENTITYTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be at least <b>{2}</b>, or hotter if the building is hotter."
				});

				// Token: 0x0400D233 RID: 53811
				public static LocString ELEMENTEMITTED_MINTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be at least <b>{2}</b>, or hotter if the input materials are hotter."
				});

				// Token: 0x0400D234 RID: 53812
				public static LocString ELEMENTEMITTED_FIXEDTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be produced at <b>{2}</b>."
				});

				// Token: 0x0400D235 RID: 53813
				public static LocString ELEMENTCONSUMED = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use"
				});

				// Token: 0x0400D236 RID: 53814
				public static LocString ELEMENTEMITTED_TOILET = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use\n\nDuplicant waste is emitted at <b>{2}</b>."
				});

				// Token: 0x0400D237 RID: 53815
				public static LocString ELEMENTEMITTEDPERUSE = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use\n\nIt will be the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the input materials."
				});

				// Token: 0x0400D238 RID: 53816
				public static LocString DISEASEEMITTEDPERUSE = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use"
				});

				// Token: 0x0400D239 RID: 53817
				public static LocString DISEASECONSUMEDPERUSE = "Removes " + UI.FormatAsNegativeRate("{0}") + " per use";

				// Token: 0x0400D23A RID: 53818
				public static LocString ELEMENTCONSUMEDPERUSE = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use"
				});

				// Token: 0x0400D23B RID: 53819
				public static LocString ENERGYCONSUMED = string.Concat(new string[]
				{
					"Draws ",
					UI.FormatAsNegativeRate("{0}"),
					" from the ",
					UI.PRE_KEYWORD,
					"Power Grid",
					UI.PST_KEYWORD,
					" it's connected to"
				});

				// Token: 0x0400D23C RID: 53820
				public static LocString ENERGYGENERATED = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{0}"),
					" for the ",
					UI.PRE_KEYWORD,
					"Power Grid",
					UI.PST_KEYWORD,
					" it's connected to"
				});

				// Token: 0x0400D23D RID: 53821
				public static LocString ENABLESDOMESTICGROWTH = string.Concat(new string[]
				{
					"Accelerates ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					" growth and maturation"
				});

				// Token: 0x0400D23E RID: 53822
				public static LocString HEATGENERATED = string.Concat(new string[]
				{
					"Generates ",
					UI.FormatAsPositiveRate("{0}"),
					" per second\n\nSum ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" change is affected by the material attributes of the heated substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity"
				});

				// Token: 0x0400D23F RID: 53823
				public static LocString HEATCONSUMED = string.Concat(new string[]
				{
					"Dissipates ",
					UI.FormatAsNegativeRate("{0}"),
					" per second\n\nSum ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" change can be affected by the material attributes of the cooled substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity"
				});

				// Token: 0x0400D240 RID: 53824
				public static LocString HEATER_TARGETTEMPERATURE = string.Concat(new string[]
				{
					"Stops heating when the surrounding average ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400D241 RID: 53825
				public static LocString FABRICATES = "Fabrication is the production of items and equipment";

				// Token: 0x0400D242 RID: 53826
				public static LocString PROCESSES = "Processes raw materials into refined materials";

				// Token: 0x0400D243 RID: 53827
				public static LocString PROCESSEDITEM = "Refining this material produces " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

				// Token: 0x0400D244 RID: 53828
				public static LocString PLANTERBOX_PENTALTY = "Plants grow more slowly when contained within boxes";

				// Token: 0x0400D245 RID: 53829
				public static LocString DECORPROVIDED = string.Concat(new string[]
				{
					"Improves ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values by ",
					UI.FormatAsPositiveModifier("<b>{0}</b>"),
					" in a <b>{1}</b> tile radius"
				});

				// Token: 0x0400D246 RID: 53830
				public static LocString DECORDECREASED = string.Concat(new string[]
				{
					"Decreases ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values by ",
					UI.FormatAsNegativeModifier("<b>{0}</b>"),
					" in a <b>{1}</b> tile radius"
				});

				// Token: 0x0400D247 RID: 53831
				public static LocString OVERHEAT_TEMP = "Begins overheating at <b>{0}</b>";

				// Token: 0x0400D248 RID: 53832
				public static LocString MINIMUM_TEMP = "Ceases to function when temperatures fall below <b>{0}</b>";

				// Token: 0x0400D249 RID: 53833
				public static LocString OVER_PRESSURE_MASS = "Ceases to function when the surrounding mass is above <b>{0}</b>";

				// Token: 0x0400D24A RID: 53834
				public static LocString REFILLOXYGENTANK = string.Concat(new string[]
				{
					"Refills ",
					UI.PRE_KEYWORD,
					"Exosuit",
					UI.PST_KEYWORD,
					" Oxygen tanks with ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" for reuse"
				});

				// Token: 0x0400D24B RID: 53835
				public static LocString DUPLICANTMOVEMENTBOOST = "Duplicants walk <b>{0}</b> faster on this tile";

				// Token: 0x0400D24C RID: 53836
				public static LocString ELECTROBANKS = string.Concat(new string[]
				{
					"Power Banks store {0} of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nThey can be discharged by circuits, buildings and Bionic Duplicants"
				});

				// Token: 0x0400D24D RID: 53837
				public static LocString STRESSREDUCEDPERMINUTE = string.Concat(new string[]
				{
					"Removes <b>{0}</b> of Duplicants' ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" for every uninterrupted minute of use"
				});

				// Token: 0x0400D24E RID: 53838
				public static LocString REMOVESEFFECTSUBTITLE = "Use of this building will remove the listed effects";

				// Token: 0x0400D24F RID: 53839
				public static LocString REMOVEDEFFECT = "{0}";

				// Token: 0x0400D250 RID: 53840
				public static LocString ADDED_EFFECT = "Effect being applied:\n\n{0}\n{1}";

				// Token: 0x0400D251 RID: 53841
				public static LocString GASCOOLING = string.Concat(new string[]
				{
					"Reduces the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of piped ",
					UI.PRE_KEYWORD,
					"Gases",
					UI.PST_KEYWORD,
					" by <b>{0}</b>"
				});

				// Token: 0x0400D252 RID: 53842
				public static LocString LIQUIDCOOLING = string.Concat(new string[]
				{
					"Reduces the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of piped ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" by <b>{0}</b>"
				});

				// Token: 0x0400D253 RID: 53843
				public static LocString MAX_WATTAGE = string.Concat(new string[]
				{
					"Drawing more than the maximum allowed ",
					UI.PRE_KEYWORD,
					"Watts",
					UI.PST_KEYWORD,
					" can result in damage to the circuit"
				});

				// Token: 0x0400D254 RID: 53844
				public static LocString MAX_BITS = string.Concat(new string[]
				{
					"Sending an ",
					UI.PRE_KEYWORD,
					"Automation Signal",
					UI.PST_KEYWORD,
					" with a higher ",
					UI.PRE_KEYWORD,
					"Bit Depth",
					UI.PST_KEYWORD,
					" than the connected ",
					UI.PRE_KEYWORD,
					"Logic Wire",
					UI.PST_KEYWORD,
					" can result in damage to the circuit"
				});

				// Token: 0x0400D255 RID: 53845
				public static LocString RESEARCH_MATERIALS = string.Concat(new string[]
				{
					"This research station consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for each ",
					UI.PRE_KEYWORD,
					"Research Point",
					UI.PST_KEYWORD,
					" produced"
				});

				// Token: 0x0400D256 RID: 53846
				public static LocString PRODUCES_RESEARCH_POINTS = string.Concat(new string[]
				{
					"Produces ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" research"
				});

				// Token: 0x0400D257 RID: 53847
				public static LocString REMOVES_DISEASE = string.Concat(new string[]
				{
					"The cooking process kills all ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" present in the ingredients, removing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" risk when eating the product"
				});

				// Token: 0x0400D258 RID: 53848
				public static LocString DOCTORING = "Doctoring increases existing health benefits and can allow the treatment of otherwise stubborn " + UI.PRE_KEYWORD + "Diseases" + UI.PST_KEYWORD;

				// Token: 0x0400D259 RID: 53849
				public static LocString RECREATION = string.Concat(new string[]
				{
					"Improves Duplicant ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" during scheduled ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D25A RID: 53850
				public static LocString HEATGENERATED_AIRCONDITIONER = string.Concat(new string[]
				{
					"Generates ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" based on the ",
					UI.PRE_KEYWORD,
					"Volume",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Specific Heat Capacity",
					UI.PST_KEYWORD,
					" of the pumped ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					"\n\nCooling 1",
					UI.UNITSUFFIXES.MASS.KILOGRAM,
					" of ",
					ELEMENTS.OXYGEN.NAME,
					" the entire <b>{1}</b> will output <b>{0}</b>"
				});

				// Token: 0x0400D25B RID: 53851
				public static LocString HEATGENERATED_LIQUIDCONDITIONER = string.Concat(new string[]
				{
					"Generates ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" based on the ",
					UI.PRE_KEYWORD,
					"Volume",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Specific Heat Capacity",
					UI.PST_KEYWORD,
					" of the pumped ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					"\n\nCooling 10",
					UI.UNITSUFFIXES.MASS.KILOGRAM,
					" of ",
					ELEMENTS.WATER.NAME,
					" the entire <b>{1}</b> will output <b>{0}</b>"
				});

				// Token: 0x0400D25C RID: 53852
				public static LocString MOVEMENT_BONUS = "Increases the Runspeed of Duplicants";

				// Token: 0x0400D25D RID: 53853
				public static LocString COOLANT = string.Concat(new string[]
				{
					"<b>{1}</b> of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" coolant is required to cool off an item produced by this building\n\nCoolant ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" increase is variable and dictated by the amount of energy needed to cool the produced item"
				});

				// Token: 0x0400D25E RID: 53854
				public static LocString REFINEMENT_ENERGY_HAS_COOLANT = string.Concat(new string[]
				{
					UI.FormatAsPositiveRate("{0}"),
					" of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" will be produced to cool off the fabricated item\n\nThis will raise the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the contained ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" by ",
					UI.FormatAsPositiveModifier("{2}"),
					", and heat the containing building"
				});

				// Token: 0x0400D25F RID: 53855
				public static LocString REFINEMENT_ENERGY_NO_COOLANT = string.Concat(new string[]
				{
					UI.FormatAsPositiveRate("{0}"),
					" of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" will be produced to cool off the fabricated item\n\nIf ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" is used for coolant, its ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will be raised by ",
					UI.FormatAsPositiveModifier("{2}"),
					", and will heat the containing building"
				});

				// Token: 0x0400D260 RID: 53856
				public static LocString IMPROVED_BUILDINGS = UI.PRE_KEYWORD + "Tune Ups" + UI.PST_KEYWORD + " will improve these buildings:";

				// Token: 0x0400D261 RID: 53857
				public static LocString IMPROVED_BUILDINGS_ITEM = "{0}";

				// Token: 0x0400D262 RID: 53858
				public static LocString IMPROVED_PLANTS = UI.PRE_KEYWORD + "Crop Tending" + UI.PST_KEYWORD + " will improve growth times for these plants:";

				// Token: 0x0400D263 RID: 53859
				public static LocString IMPROVED_PLANTS_ITEM = "{0}";

				// Token: 0x0400D264 RID: 53860
				public static LocString GEYSER_PRODUCTION = string.Concat(new string[]
				{
					"While erupting, this geyser will produce ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" at a rate of ",
					UI.FormatAsPositiveRate("{1}"),
					", and at a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{2}</b>"
				});

				// Token: 0x0400D265 RID: 53861
				public static LocString GEYSER_PRODUCTION_GEOTUNED = string.Concat(new string[]
				{
					"While erupting, this geyser will produce ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" at a rate of ",
					UI.FormatAsPositiveRate("{1}"),
					", and at a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{2}</b>"
				});

				// Token: 0x0400D266 RID: 53862
				public static LocString GEYSER_PRODUCTION_GEOTUNED_COUNT = "<b>{0}</b> of <b>{1}</b> Geotuners targeting this geyser are amplifying it";

				// Token: 0x0400D267 RID: 53863
				public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL = "Total geotuning: {0} {1}";

				// Token: 0x0400D268 RID: 53864
				public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL_ROW_TITLE = "Geotuned ";

				// Token: 0x0400D269 RID: 53865
				public static LocString GEYSER_DISEASE = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " germs are present in the output of this geyser";

				// Token: 0x0400D26A RID: 53866
				public static LocString GEYSER_PERIOD = "This geyser will produce for <b>{0}</b> of every <b>{1}</b>";

				// Token: 0x0400D26B RID: 53867
				public static LocString GEYSER_YEAR_UNSTUDIED = "A researcher must analyze this geyser to determine its geoactive period";

				// Token: 0x0400D26C RID: 53868
				public static LocString GEYSER_YEAR_PERIOD = "This geyser will be active for <b>{0}</b> out of every <b>{1}</b>\n\nIt will be dormant the rest of the time";

				// Token: 0x0400D26D RID: 53869
				public static LocString GEYSER_YEAR_NEXT_ACTIVE = "This geyser will become active in <b>{0}</b>";

				// Token: 0x0400D26E RID: 53870
				public static LocString GEYSER_YEAR_NEXT_DORMANT = "This geyser will become dormant in <b>{0}</b>";

				// Token: 0x0400D26F RID: 53871
				public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = "A researcher must analyze this geyser to determine its average output rate";

				// Token: 0x0400D270 RID: 53872
				public static LocString GEYSER_YEAR_AVR_OUTPUT = "This geyser emits an average of {average} of {element} during its lifetime\n\nThis includes its dormant period";

				// Token: 0x0400D271 RID: 53873
				public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE = "Total Geotuning ";

				// Token: 0x0400D272 RID: 53874
				public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW = "Geotuned ";

				// Token: 0x0400D273 RID: 53875
				public static LocString CAPTURE_METHOD_WRANGLE = string.Concat(new string[]
				{
					"This critter can be captured\n\nMark critters for capture using the ",
					UI.FormatAsTool("Wrangle Tool", global::Action.Capture),
					"\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill in order to wrangle critters"
				});

				// Token: 0x0400D274 RID: 53876
				public static LocString CAPTURE_METHOD_FLYING_TRAP = "This critter can be captured and moved using an " + STRINGS.BUILDINGS.PREFABS.CREATUREAIRTRAP.NAME;

				// Token: 0x0400D275 RID: 53877
				public static LocString CAPTURE_METHOD_TRAP = "This critter can be captured and moved using a " + STRINGS.BUILDINGS.PREFABS.CREATURETRAP.NAME;

				// Token: 0x0400D276 RID: 53878
				public static LocString CAPTURE_METHOD_FISH_TRAP = "This critter can be captured and moved using a " + STRINGS.BUILDINGS.PREFABS.FISHTRAP.NAME;

				// Token: 0x0400D277 RID: 53879
				public static LocString NOISE_POLLUTION_INCREASE = "Produces noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";

				// Token: 0x0400D278 RID: 53880
				public static LocString NOISE_POLLUTION_DECREASE = "Dampens noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";

				// Token: 0x0400D279 RID: 53881
				public static LocString ITEM_TEMPERATURE_ADJUST = string.Concat(new string[]
				{
					"Stored items will reach a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{0}</b> over time"
				});

				// Token: 0x0400D27A RID: 53882
				public static LocString DIET_HEADER = "Creatures will eat and digest only specific materials";

				// Token: 0x0400D27B RID: 53883
				public static LocString DIET_CONSUMED = "This critter can typically consume these materials at the following rates:\n\n{Foodlist}";

				// Token: 0x0400D27C RID: 53884
				public static LocString DIET_PRODUCED = "This critter will \"produce\" the following materials:\n\n{Items}";

				// Token: 0x0400D27D RID: 53885
				public static LocString DIET_ADDITIONAL_PRODUCED = "This critter gets bloated after eating and will produce {Items}";

				// Token: 0x0400D27E RID: 53886
				public static LocString ROCKETRESTRICTION_HEADER = "Controls whether a building is operational within a rocket interior";

				// Token: 0x0400D27F RID: 53887
				public static LocString ROCKETRESTRICTION_BUILDINGS = "This station controls the operational status of the following buildings:\n\n{buildinglist}";

				// Token: 0x0400D280 RID: 53888
				public static LocString UNSTABLEENTOMBDEFENSEREADY = string.Concat(new string[]
				{
					"This plant is ready to shake off ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements that threaten to entomb it"
				});

				// Token: 0x0400D281 RID: 53889
				public static LocString UNSTABLEENTOMBDEFENSETHREATENED = string.Concat(new string[]
				{
					"This plant is preparing to shake off ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements that are entombing it"
				});

				// Token: 0x0400D282 RID: 53890
				public static LocString UNSTABLEENTOMBDEFENSEREACTING = string.Concat(new string[]
				{
					"This plant is currently unentombing itself from ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements"
				});

				// Token: 0x0400D283 RID: 53891
				public static LocString UNSTABLEENTOMBDEFENSEOFF = string.Concat(new string[]
				{
					"This plant's ability to unentomb itself from ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements is currently disabled"
				});

				// Token: 0x0400D284 RID: 53892
				public static LocString BRANCH_GROWER_PLANT_POTENTIAL_OUTPUT = "{0} to {1}";

				// Token: 0x0400D285 RID: 53893
				public static LocString EDIBLE_PLANT_INTERNAL_STORAGE = "{0} of stored {1}";

				// Token: 0x0400D286 RID: 53894
				public static LocString SCALE_GROWTH = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveModifier("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D287 RID: 53895
				public static LocString SCALE_GROWTH_ATMO = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveRate("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must be kept in ",
					UI.PRE_KEYWORD,
					"{Atmosphere}",
					UI.PST_KEYWORD,
					"-rich environments to regrow sheared ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D288 RID: 53896
				public static LocString SCALE_GROWTH_TEMP = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveRate("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must eat food between {TempMin} - {TempMax} to regrow sheared ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D289 RID: 53897
				public static LocString SCALE_GROWTH_FED = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveModifier("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must be well fed to grow shearable ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D28A RID: 53898
				public static LocString MESS_TABLE_SALT = string.Concat(new string[]
				{
					"Duplicants gain ",
					UI.FormatAsPositiveModifier("+{0}"),
					" ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when using ",
					UI.PRE_KEYWORD,
					"Table Salt",
					UI.PST_KEYWORD,
					" with their food at a ",
					STRINGS.ROOMS.CRITERIA.DININGTABLETYPE.NAME
				});

				// Token: 0x0400D28B RID: 53899
				public static LocString COMMUNAL_DINING = string.Concat(new string[]
				{
					"Duplicants gain ",
					UI.FormatAsPositiveModifier("+{0}"),
					" ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when dining with friends at a ",
					STRINGS.BUILDINGS.PREFABS.MULTIMINIONDININGTABLE.NAME
				});

				// Token: 0x0400D28C RID: 53900
				public static LocString ACCESS_CONTROL = "Settings to allow or restrict Duplicants from passing through the door.";

				// Token: 0x0400D28D RID: 53901
				public static LocString TRANSFORMER_INPUT_WIRE = string.Concat(new string[]
				{
					"Connect a ",
					UI.PRE_KEYWORD,
					"Wire",
					UI.PST_KEYWORD,
					" to the large ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD,
					" with any amount of ",
					UI.PRE_KEYWORD,
					"Watts",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D28E RID: 53902
				public static LocString TRANSFORMER_OUTPUT_WIRE = string.Concat(new string[]
				{
					"The ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" provided by the the small ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" will be limited to {0}."
				});

				// Token: 0x0400D28F RID: 53903
				public static LocString FABRICATOR_INGREDIENTS = "Ingredients:\n{0}";

				// Token: 0x0400D290 RID: 53904
				public static LocString ACTIVE_PARTICLE_CONSUMPTION = string.Concat(new string[]
				{
					"This building requires ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" to function, consuming them at a rate of {Rate} while in use"
				});

				// Token: 0x0400D291 RID: 53905
				public static LocString PARTICLE_PORT_INPUT = "A Radbolt Port on this building allows it to receive " + UI.PRE_KEYWORD + "Radbolts" + UI.PST_KEYWORD;

				// Token: 0x0400D292 RID: 53906
				public static LocString PARTICLE_PORT_OUTPUT = string.Concat(new string[]
				{
					"This building has a configurable Radbolt Port for ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" emission"
				});

				// Token: 0x0400D293 RID: 53907
				public static LocString IN_ORBIT_REQUIRED = "This building is only operational while its parent rocket is in flight";

				// Token: 0x0400D294 RID: 53908
				public static LocString FOOD_DEHYDRATOR_WATER_OUTPUT = string.Concat(new string[]
				{
					"This building dumps ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" on the floor while in use"
				});

				// Token: 0x0400D295 RID: 53909
				public static LocString KETTLE_MELT_RATE = string.Concat(new string[]
				{
					"This building melts {0} of ",
					UI.PRE_KEYWORD,
					"Ice",
					UI.PST_KEYWORD,
					" into {0} of cold ({1}) ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					"\n\n",
					UI.PRE_KEYWORD,
					"Wood",
					UI.PST_KEYWORD,
					" consumption varies depending on the initial temperature of the ",
					UI.PRE_KEYWORD,
					"Ice",
					UI.PST_KEYWORD
				});
			}
		}

		// Token: 0x02002551 RID: 9553
		public class LOGIC_PORTS
		{
			// Token: 0x0400A822 RID: 43042
			public static LocString INPUT_PORTS = UI.FormatAsLink("Auto Inputs", "LOGIC");

			// Token: 0x0400A823 RID: 43043
			public static LocString INPUT_PORTS_TOOLTIP = "Input ports change a state on this building when a signal is received";

			// Token: 0x0400A824 RID: 43044
			public static LocString OUTPUT_PORTS = UI.FormatAsLink("Auto Outputs", "LOGIC");

			// Token: 0x0400A825 RID: 43045
			public static LocString OUTPUT_PORTS_TOOLTIP = "Output ports send a signal when this building changes state";

			// Token: 0x0400A826 RID: 43046
			public static LocString INPUT_PORT_TOOLTIP = "Input Behavior:\n• {0}\n• {1}";

			// Token: 0x0400A827 RID: 43047
			public static LocString OUTPUT_PORT_TOOLTIP = "Output Behavior:\n• {0}\n• {1}";

			// Token: 0x0400A828 RID: 43048
			public static LocString CONTROL_OPERATIONAL = "Enable/Disable";

			// Token: 0x0400A829 RID: 43049
			public static LocString CONTROL_OPERATIONAL_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable building";

			// Token: 0x0400A82A RID: 43050
			public static LocString CONTROL_OPERATIONAL_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable building";

			// Token: 0x0400A82B RID: 43051
			public static LocString PORT_INPUT_DEFAULT_NAME = "INPUT";

			// Token: 0x0400A82C RID: 43052
			public static LocString PORT_OUTPUT_DEFAULT_NAME = "OUTPUT";

			// Token: 0x0400A82D RID: 43053
			public static LocString GATE_MULTI_INPUT_ONE_NAME = "INPUT A";

			// Token: 0x0400A82E RID: 43054
			public static LocString GATE_MULTI_INPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400A82F RID: 43055
			public static LocString GATE_MULTI_INPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400A830 RID: 43056
			public static LocString GATE_MULTI_INPUT_TWO_NAME = "INPUT B";

			// Token: 0x0400A831 RID: 43057
			public static LocString GATE_MULTI_INPUT_TWO_ACTIVE = "Green Signal";

			// Token: 0x0400A832 RID: 43058
			public static LocString GATE_MULTI_INPUT_TWO_INACTIVE = "Red Signal";

			// Token: 0x0400A833 RID: 43059
			public static LocString GATE_MULTI_INPUT_THREE_NAME = "INPUT C";

			// Token: 0x0400A834 RID: 43060
			public static LocString GATE_MULTI_INPUT_THREE_ACTIVE = "Green Signal";

			// Token: 0x0400A835 RID: 43061
			public static LocString GATE_MULTI_INPUT_THREE_INACTIVE = "Red Signal";

			// Token: 0x0400A836 RID: 43062
			public static LocString GATE_MULTI_INPUT_FOUR_NAME = "INPUT D";

			// Token: 0x0400A837 RID: 43063
			public static LocString GATE_MULTI_INPUT_FOUR_ACTIVE = "Green Signal";

			// Token: 0x0400A838 RID: 43064
			public static LocString GATE_MULTI_INPUT_FOUR_INACTIVE = "Red Signal";

			// Token: 0x0400A839 RID: 43065
			public static LocString GATE_SINGLE_INPUT_ONE_NAME = "INPUT";

			// Token: 0x0400A83A RID: 43066
			public static LocString GATE_SINGLE_INPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400A83B RID: 43067
			public static LocString GATE_SINGLE_INPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400A83C RID: 43068
			public static LocString GATE_MULTI_OUTPUT_ONE_NAME = "OUTPUT A";

			// Token: 0x0400A83D RID: 43069
			public static LocString GATE_MULTI_OUTPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400A83E RID: 43070
			public static LocString GATE_MULTI_OUTPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400A83F RID: 43071
			public static LocString GATE_MULTI_OUTPUT_TWO_NAME = "OUTPUT B";

			// Token: 0x0400A840 RID: 43072
			public static LocString GATE_MULTI_OUTPUT_TWO_ACTIVE = "Green Signal";

			// Token: 0x0400A841 RID: 43073
			public static LocString GATE_MULTI_OUTPUT_TWO_INACTIVE = "Red Signal";

			// Token: 0x0400A842 RID: 43074
			public static LocString GATE_MULTI_OUTPUT_THREE_NAME = "OUTPUT C";

			// Token: 0x0400A843 RID: 43075
			public static LocString GATE_MULTI_OUTPUT_THREE_ACTIVE = "Green Signal";

			// Token: 0x0400A844 RID: 43076
			public static LocString GATE_MULTI_OUTPUT_THREE_INACTIVE = "Red Signal";

			// Token: 0x0400A845 RID: 43077
			public static LocString GATE_MULTI_OUTPUT_FOUR_NAME = "OUTPUT D";

			// Token: 0x0400A846 RID: 43078
			public static LocString GATE_MULTI_OUTPUT_FOUR_ACTIVE = "Green Signal";

			// Token: 0x0400A847 RID: 43079
			public static LocString GATE_MULTI_OUTPUT_FOUR_INACTIVE = "Red Signal";

			// Token: 0x0400A848 RID: 43080
			public static LocString GATE_SINGLE_OUTPUT_ONE_NAME = "OUTPUT";

			// Token: 0x0400A849 RID: 43081
			public static LocString GATE_SINGLE_OUTPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400A84A RID: 43082
			public static LocString GATE_SINGLE_OUTPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400A84B RID: 43083
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_NAME = "CONTROL A";

			// Token: 0x0400A84C RID: 43084
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position";

			// Token: 0x0400A84D RID: 43085
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position";

			// Token: 0x0400A84E RID: 43086
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_NAME = "CONTROL B";

			// Token: 0x0400A84F RID: 43087
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position";

			// Token: 0x0400A850 RID: 43088
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position";
		}

		// Token: 0x02002552 RID: 9554
		public class GAMEOBJECTEFFECTS
		{
			// Token: 0x0400A851 RID: 43089
			public static LocString CALORIES = "+{0}";

			// Token: 0x0400A852 RID: 43090
			public static LocString FOOD_QUALITY = "Quality: {0}";

			// Token: 0x0400A853 RID: 43091
			public static LocString FOOD_MORALE = "Morale: {0}";

			// Token: 0x0400A854 RID: 43092
			public static LocString FORGAVEATTACKER = "Forgiveness";

			// Token: 0x0400A855 RID: 43093
			public static LocString COLDBREATHER = UI.FormatAsLink("Cooling Effect", "HEAT");

			// Token: 0x0400A856 RID: 43094
			public static LocString LIFECYCLETITLE = "Growth:";

			// Token: 0x0400A857 RID: 43095
			public static LocString GROWTHTIME_SIMPLE = "Life Cycle: {0}";

			// Token: 0x0400A858 RID: 43096
			public static LocString GROWTHTIME_REGROWTH = "Domestic growth: {0} / {1}";

			// Token: 0x0400A859 RID: 43097
			public static LocString GROWTHTIME = "Growth: {0}";

			// Token: 0x0400A85A RID: 43098
			public static LocString INITIALGROWTHTIME = "Initial Growth: {0}";

			// Token: 0x0400A85B RID: 43099
			public static LocString REGROWTHTIME = "Regrowth: {0}";

			// Token: 0x0400A85C RID: 43100
			public static LocString REQUIRES_LIGHT = UI.FormatAsLink("Light", "LIGHT") + ": {Lux}";

			// Token: 0x0400A85D RID: 43101
			public static LocString REQUIRES_DARKNESS = UI.FormatAsLink("Darkness", "LIGHT");

			// Token: 0x0400A85E RID: 43102
			public static LocString REQUIRESFERTILIZER = "{0}: {1}";

			// Token: 0x0400A85F RID: 43103
			public static LocString IDEAL_FERTILIZER = "{0}: {1}";

			// Token: 0x0400A860 RID: 43104
			public static LocString EQUIPMENT_MODS = "{Attribute} {Value}";

			// Token: 0x0400A861 RID: 43105
			public static LocString ROTTEN = "Rotten";

			// Token: 0x0400A862 RID: 43106
			public static LocString REQUIRES_ATMOSPHERE = UI.FormatAsLink("Atmosphere", "ATMOSPHERE") + ": {0}";

			// Token: 0x0400A863 RID: 43107
			public static LocString REQUIRES_PRESSURE = UI.FormatAsLink("Air", "ATMOSPHERE") + " Pressure: {0} minimum";

			// Token: 0x0400A864 RID: 43108
			public static LocString IDEAL_PRESSURE = UI.FormatAsLink("Air", "ATMOSPHERE") + " Pressure: {0}";

			// Token: 0x0400A865 RID: 43109
			public static LocString REQUIRES_TEMPERATURE = UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}";

			// Token: 0x0400A866 RID: 43110
			public static LocString IDEAL_TEMPERATURE = UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}";

			// Token: 0x0400A867 RID: 43111
			public static LocString REQUIRES_SUBMERSION = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Submersion";

			// Token: 0x0400A868 RID: 43112
			public static LocString FOOD_EFFECTS = "Effects:";

			// Token: 0x0400A869 RID: 43113
			public static LocString EMITS_LIGHT = UI.FormatAsLink("Light Range", "LIGHT") + ": {0} tiles";

			// Token: 0x0400A86A RID: 43114
			public static LocString EMITS_LIGHT_LUX = UI.FormatAsLink("Brightness", "LIGHT") + ": {0} Lux";

			// Token: 0x0400A86B RID: 43115
			public static LocString AMBIENT_RADIATION = "Ambient Radiation";

			// Token: 0x0400A86C RID: 43116
			public static LocString AMBIENT_RADIATION_FMT = "{minRads} - {maxRads}";

			// Token: 0x0400A86D RID: 43117
			public static LocString AMBIENT_NO_MIN_RADIATION_FMT = "Less than {maxRads}";

			// Token: 0x0400A86E RID: 43118
			public static LocString REQUIRES_NO_MIN_RADIATION = "Maximum " + UI.FormatAsLink("Radiation", "RADIATION") + ": {MaxRads}";

			// Token: 0x0400A86F RID: 43119
			public static LocString REQUIRES_RADIATION = UI.FormatAsLink("Radiation", "RADIATION") + ": {MinRads} to {MaxRads}";

			// Token: 0x0400A870 RID: 43120
			public static LocString MUTANT_STERILE = "Doesn't Drop " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x0400A871 RID: 43121
			public static LocString DARKNESS = "Darkness";

			// Token: 0x0400A872 RID: 43122
			public static LocString LIGHT = "Light";

			// Token: 0x0400A873 RID: 43123
			public static LocString SEED_PRODUCTION_DIG_ONLY = "Consumes 1 " + UI.FormatAsLink("Seed", "PLANTS");

			// Token: 0x0400A874 RID: 43124
			public static LocString SEED_PRODUCTION_HARVEST = "Harvest yields " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x0400A875 RID: 43125
			public static LocString SEED_PRODUCTION_FINAL_HARVEST = "Final harvest yields " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x0400A876 RID: 43126
			public static LocString SEED_PRODUCTION_FRUIT = "Fruit produces " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x0400A877 RID: 43127
			public static LocString SEED_REQUIREMENT_CEILING = "Plot Orientation: Downward";

			// Token: 0x0400A878 RID: 43128
			public static LocString SEED_REQUIREMENT_WALL = "Plot Orientation: Sideways";

			// Token: 0x0400A879 RID: 43129
			public static LocString REQUIRES_RECEPTACLE = "Farm Plot";

			// Token: 0x0400A87A RID: 43130
			public static LocString PLANT_MARK_FOR_HARVEST = "Autoharvest Enabled";

			// Token: 0x0400A87B RID: 43131
			public static LocString PLANT_DO_NOT_HARVEST = "Autoharvest Disabled";

			// Token: 0x0400A87C RID: 43132
			public static LocString REQUIRES_POLLINATION = "Pollination";

			// Token: 0x0200309E RID: 12446
			public class INSULATED
			{
				// Token: 0x0400D296 RID: 53910
				public static LocString NAME = "Insulated";

				// Token: 0x0400D297 RID: 53911
				public static LocString TOOLTIP = "Proper insulation drastically reduces thermal conductivity";
			}

			// Token: 0x0200309F RID: 12447
			public class TOOLTIPS
			{
				// Token: 0x0400D298 RID: 53912
				public static LocString CALORIES = "+{0}";

				// Token: 0x0400D299 RID: 53913
				public static LocString FOOD_QUALITY = "Quality: {0}";

				// Token: 0x0400D29A RID: 53914
				public static LocString FOOD_MORALE = "Morale: {0}";

				// Token: 0x0400D29B RID: 53915
				public static LocString COLDBREATHER = "Lowers ambient air temperature";

				// Token: 0x0400D29C RID: 53916
				public static LocString GROWTHTIME_SIMPLE = "This plant takes <b>{0}</b> to grow";

				// Token: 0x0400D29D RID: 53917
				public static LocString GROWTHTIME_REGROWTH = "This plant initially takes <b>{0}</b> to grow, but only <b>{1}</b> to mature after first harvest";

				// Token: 0x0400D29E RID: 53918
				public static LocString GROWTHTIME = "This plant takes <b>{0}</b> to grow";

				// Token: 0x0400D29F RID: 53919
				public static LocString INITIALGROWTHTIME = "This plant takes <b>{0}</b> to mature again once replanted";

				// Token: 0x0400D2A0 RID: 53920
				public static LocString REGROWTHTIME = "This plant takes <b>{0}</b> to mature again once harvested";

				// Token: 0x0400D2A1 RID: 53921
				public static LocString EQUIPMENT_MODS = "{Attribute} {Value}";

				// Token: 0x0400D2A2 RID: 53922
				public static LocString REQUIRESFERTILIZER = string.Concat(new string[]
				{
					"This plant requires <b>{1}</b> ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400D2A3 RID: 53923
				public static LocString IDEAL_FERTILIZER = string.Concat(new string[]
				{
					"This plant requires <b>{1}</b> of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400D2A4 RID: 53924
				public static LocString REQUIRES_LIGHT = string.Concat(new string[]
				{
					"This plant requires a ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" source bathing it in at least {Lux}"
				});

				// Token: 0x0400D2A5 RID: 53925
				public static LocString REQUIRES_DARKNESS = "This plant requires complete darkness";

				// Token: 0x0400D2A6 RID: 53926
				public static LocString REQUIRES_ATMOSPHERE = "This plant must be submerged in one of the following gases: {0}";

				// Token: 0x0400D2A7 RID: 53927
				public static LocString REQUIRES_ATMOSPHERE_LIQUID = "This plant must be submerged in one of the following liquids: {0}";

				// Token: 0x0400D2A8 RID: 53928
				public static LocString REQUIRES_ATMOSPHERE_MIXED = "This plant must be submerged in one of the following gases or liquids: {0}";

				// Token: 0x0400D2A9 RID: 53929
				public static LocString REQUIRES_PRESSURE = string.Concat(new string[]
				{
					"Ambient ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" pressure must be at least <b>{0}</b> for basic growth"
				});

				// Token: 0x0400D2AA RID: 53930
				public static LocString IDEAL_PRESSURE = string.Concat(new string[]
				{
					"This plant requires ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" pressures above <b>{0}</b> for basic growth"
				});

				// Token: 0x0400D2AB RID: 53931
				public static LocString REQUIRES_TEMPERATURE = string.Concat(new string[]
				{
					"Internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" must be between <b>{0}</b> and <b>{1}</b> for basic growth"
				});

				// Token: 0x0400D2AC RID: 53932
				public static LocString IDEAL_TEMPERATURE = string.Concat(new string[]
				{
					"This plant requires internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" between <b>{0}</b> and <b>{1}</b> for basic growth"
				});

				// Token: 0x0400D2AD RID: 53933
				public static LocString REQUIRES_SUBMERSION = string.Concat(new string[]
				{
					"This plant must be fully submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400D2AE RID: 53934
				public static LocString FOOD_EFFECTS = "Duplicants will gain the following effects from eating this food: {0}";

				// Token: 0x0400D2AF RID: 53935
				public static LocString REQUIRES_RECEPTACLE = string.Concat(new string[]
				{
					"This plant must be housed in a ",
					UI.FormatAsLink("Planter Box", "PLANTERBOX"),
					", ",
					UI.FormatAsLink("Farm Tile", "FARMTILE"),
					", or ",
					UI.FormatAsLink("Hydroponic Farm", "HYDROPONICFARM"),
					" farm to grow domestically"
				});

				// Token: 0x0400D2B0 RID: 53936
				public static LocString EMITS_LIGHT = string.Concat(new string[]
				{
					"Emits ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					"\n\nDuplicants can operate buildings more quickly when they're well lit"
				});

				// Token: 0x0400D2B1 RID: 53937
				public static LocString EMITS_LIGHT_LUX = string.Concat(new string[]
				{
					"Emits ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					"\n\nDuplicants can operate buildings more quickly when they're well lit"
				});

				// Token: 0x0400D2B2 RID: 53938
				public static LocString METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP = "Distribution of meteor types in this shower";

				// Token: 0x0400D2B3 RID: 53939
				public static LocString SEED_PRODUCTION_DIG_ONLY = "May be replanted, but will produce no further " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400D2B4 RID: 53940
				public static LocString SEED_PRODUCTION_HARVEST = "Harvesting this plant will yield new " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400D2B5 RID: 53941
				public static LocString SEED_PRODUCTION_FINAL_HARVEST = string.Concat(new string[]
				{
					"Yields new ",
					UI.PRE_KEYWORD,
					"Seeds",
					UI.PST_KEYWORD,
					" on the final harvest of its life cycle"
				});

				// Token: 0x0400D2B6 RID: 53942
				public static LocString SEED_PRODUCTION_FRUIT = "Consuming this plant's fruit will yield new " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400D2B7 RID: 53943
				public static LocString SEED_REQUIREMENT_CEILING = "This seed must be planted in a downward facing plot\n\nPress " + UI.FormatAsKeyWord("[O]") + " while building farm plots to rotate them";

				// Token: 0x0400D2B8 RID: 53944
				public static LocString SEED_REQUIREMENT_WALL = "This seed must be planted in a side facing plot\n\nPress " + UI.FormatAsKeyWord("[O]") + " while building farm plots to rotate them";

				// Token: 0x0400D2B9 RID: 53945
				public static LocString REQUIRES_NO_MIN_RADIATION = "This plant will stop growing if exposed to more than {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400D2BA RID: 53946
				public static LocString REQUIRES_RADIATION = "This plant will only grow if it has between {MinRads} and {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400D2BB RID: 53947
				public static LocString MUTANT_SEED_TOOLTIP = "\n\nGrowing near its maximum radiation increases the chance of mutant seeds being produced";

				// Token: 0x0400D2BC RID: 53948
				public static LocString MUTANT_STERILE = "This plant will not produce seeds of its own due to changes to its DNA";

				// Token: 0x0400D2BD RID: 53949
				public static LocString REQUIRES_POLLINATION = string.Concat(new string[]
				{
					"This plant must be tended by a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to grow"
				});
			}

			// Token: 0x020030A0 RID: 12448
			public class DAMAGE_POPS
			{
				// Token: 0x0400D2BE RID: 53950
				public static LocString OVERHEAT = "Overheat Damage";

				// Token: 0x0400D2BF RID: 53951
				public static LocString CORROSIVE_ELEMENT = "Corrosive Element Damage";

				// Token: 0x0400D2C0 RID: 53952
				public static LocString WRONG_ELEMENT = "Wrong Element Damage";

				// Token: 0x0400D2C1 RID: 53953
				public static LocString CIRCUIT_OVERLOADED = "Overload Damage";

				// Token: 0x0400D2C2 RID: 53954
				public static LocString LOGIC_CIRCUIT_OVERLOADED = "Signal Overload Damage";

				// Token: 0x0400D2C3 RID: 53955
				public static LocString LIQUID_PRESSURE = "Pressure Damage";

				// Token: 0x0400D2C4 RID: 53956
				public static LocString MINION_DESTRUCTION = "Tantrum Damage";

				// Token: 0x0400D2C5 RID: 53957
				public static LocString CONDUIT_CONTENTS_FROZE = "Cold Damage";

				// Token: 0x0400D2C6 RID: 53958
				public static LocString CONDUIT_CONTENTS_BOILED = "Heat Damage";

				// Token: 0x0400D2C7 RID: 53959
				public static LocString MICROMETEORITE = "Micrometeorite Damage";

				// Token: 0x0400D2C8 RID: 53960
				public static LocString COMET = "Meteor Damage";

				// Token: 0x0400D2C9 RID: 53961
				public static LocString ROCKET = "Rocket Thruster Damage";

				// Token: 0x0400D2CA RID: 53962
				public static LocString POWER_BANK_WATER_DAMAGE = "Water Damage";
			}
		}

		// Token: 0x02002553 RID: 9555
		public class ASTEROIDCLOCK
		{
			// Token: 0x0400A87D RID: 43133
			public static LocString CYCLE = "Cycle";

			// Token: 0x0400A87E RID: 43134
			public static LocString CYCLES_OLD = "This Colony is {0} Cycle(s) Old";

			// Token: 0x0400A87F RID: 43135
			public static LocString TIME_PLAYED = "Time Played: {0} hours";

			// Token: 0x0400A880 RID: 43136
			public static LocString SCHEDULE_BUTTON_TOOLTIP = "Manage Schedule";

			// Token: 0x0400A881 RID: 43137
			public static LocString MILESTONE_TITLE = "Approaching Milestone";

			// Token: 0x0400A882 RID: 43138
			public static LocString MILESTONE_DESCRIPTION = "This colony is about to hit Cycle {0}!";
		}

		// Token: 0x02002554 RID: 9556
		public class ENDOFDAYREPORT
		{
			// Token: 0x0400A883 RID: 43139
			public static LocString REPORT_TITLE = "DAILY REPORTS";

			// Token: 0x0400A884 RID: 43140
			public static LocString DAY_TITLE = "Cycle {0}";

			// Token: 0x0400A885 RID: 43141
			public static LocString DAY_TITLE_TODAY = "Cycle {0} - Today";

			// Token: 0x0400A886 RID: 43142
			public static LocString DAY_TITLE_YESTERDAY = "Cycle {0} - Yesterday";

			// Token: 0x0400A887 RID: 43143
			public static LocString NOTIFICATION_TITLE = "Cycle {0} report ready";

			// Token: 0x0400A888 RID: 43144
			public static LocString NOTIFICATION_TOOLTIP = "The daily report for Cycle {0} is ready to view";

			// Token: 0x0400A889 RID: 43145
			public static LocString NEXT = "Next";

			// Token: 0x0400A88A RID: 43146
			public static LocString PREV = "Prev";

			// Token: 0x0400A88B RID: 43147
			public static LocString ADDED = "Added";

			// Token: 0x0400A88C RID: 43148
			public static LocString REMOVED = "Removed";

			// Token: 0x0400A88D RID: 43149
			public static LocString NET = "Net";

			// Token: 0x0400A88E RID: 43150
			public static LocString DUPLICANT_DETAILS_HEADER = "Duplicant Details:";

			// Token: 0x0400A88F RID: 43151
			public static LocString TIME_DETAILS_HEADER = "Total Time Details:";

			// Token: 0x0400A890 RID: 43152
			public static LocString BASE_DETAILS_HEADER = "Base Details:";

			// Token: 0x0400A891 RID: 43153
			public static LocString AVERAGE_TIME_DETAILS_HEADER = "Average Time Details:";

			// Token: 0x0400A892 RID: 43154
			public static LocString MY_COLONY = "my colony";

			// Token: 0x0400A893 RID: 43155
			public static LocString NONE = "None";

			// Token: 0x020030A1 RID: 12449
			public class OXYGEN_CREATED
			{
				// Token: 0x0400D2CB RID: 53963
				public static LocString NAME = UI.FormatAsLink("Oxygen", "OXYGEN") + " Generation:";

				// Token: 0x0400D2CC RID: 53964
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Oxygen", "OXYGEN") + " was produced by {1} over the course of the day";

				// Token: 0x0400D2CD RID: 53965
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Oxygen", "OXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x020030A2 RID: 12450
			public class CALORIES_CREATED
			{
				// Token: 0x0400D2CE RID: 53966
				public static LocString NAME = "Calorie Generation:";

				// Token: 0x0400D2CF RID: 53967
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Food", "FOOD") + " was produced by {1} over the course of the day";

				// Token: 0x0400D2D0 RID: 53968
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Food", "FOOD") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x020030A3 RID: 12451
			public class NUMBER_OF_DOMESTICATED_CRITTERS
			{
				// Token: 0x0400D2D1 RID: 53969
				public static LocString NAME = "Domesticated Critters:";

				// Token: 0x0400D2D2 RID: 53970
				public static LocString POSITIVE_TOOLTIP = "{0} domestic critters live in {1}";

				// Token: 0x0400D2D3 RID: 53971
				public static LocString NEGATIVE_TOOLTIP = "{0} domestic critters live in {1}";
			}

			// Token: 0x020030A4 RID: 12452
			public class NUMBER_OF_WILD_CRITTERS
			{
				// Token: 0x0400D2D4 RID: 53972
				public static LocString NAME = "Wild Critters:";

				// Token: 0x0400D2D5 RID: 53973
				public static LocString POSITIVE_TOOLTIP = "{0} wild critters live in {1}";

				// Token: 0x0400D2D6 RID: 53974
				public static LocString NEGATIVE_TOOLTIP = "{0} wild critters live in {1}";
			}

			// Token: 0x020030A5 RID: 12453
			public class ROCKETS_IN_FLIGHT
			{
				// Token: 0x0400D2D7 RID: 53975
				public static LocString NAME = "Rocket Missions Underway:";

				// Token: 0x0400D2D8 RID: 53976
				public static LocString POSITIVE_TOOLTIP = "{0} rockets are currently flying missions for {1}";

				// Token: 0x0400D2D9 RID: 53977
				public static LocString NEGATIVE_TOOLTIP = "{0} rockets are currently flying missions for {1}";
			}

			// Token: 0x020030A6 RID: 12454
			public class STRESS_DELTA
			{
				// Token: 0x0400D2DA RID: 53978
				public static LocString NAME = UI.FormatAsLink("Stress", "STRESS") + " Change:";

				// Token: 0x0400D2DB RID: 53979
				public static LocString POSITIVE_TOOLTIP = UI.FormatAsLink("Stress", "STRESS") + " increased by a total of {0} for {1}";

				// Token: 0x0400D2DC RID: 53980
				public static LocString NEGATIVE_TOOLTIP = UI.FormatAsLink("Stress", "STRESS") + " decreased by a total of {0} for {1}";
			}

			// Token: 0x020030A7 RID: 12455
			public class TRAVELTIMEWARNING
			{
				// Token: 0x0400D2DD RID: 53981
				public static LocString WARNING_TITLE = "Long Commutes";

				// Token: 0x0400D2DE RID: 53982
				public static LocString WARNING_MESSAGE = "My Duplicants are spending a significant amount of time traveling between their errands (> {0})";
			}

			// Token: 0x020030A8 RID: 12456
			public class TRAVEL_TIME
			{
				// Token: 0x0400D2DF RID: 53983
				public static LocString NAME = "Travel Time:";

				// Token: 0x0400D2E0 RID: 53984
				public static LocString POSITIVE_TOOLTIP = "On average, {1} spent {0} of their time traveling between tasks";
			}

			// Token: 0x020030A9 RID: 12457
			public class WORK_TIME
			{
				// Token: 0x0400D2E1 RID: 53985
				public static LocString NAME = "Work Time:";

				// Token: 0x0400D2E2 RID: 53986
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent working";
			}

			// Token: 0x020030AA RID: 12458
			public class IDLE_TIME
			{
				// Token: 0x0400D2E3 RID: 53987
				public static LocString NAME = "Idle Time:";

				// Token: 0x0400D2E4 RID: 53988
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent idling";
			}

			// Token: 0x020030AB RID: 12459
			public class PERSONAL_TIME
			{
				// Token: 0x0400D2E5 RID: 53989
				public static LocString NAME = "Personal Time:";

				// Token: 0x0400D2E6 RID: 53990
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent tending to personal needs";
			}

			// Token: 0x020030AC RID: 12460
			public class ENERGY_USAGE
			{
				// Token: 0x0400D2E7 RID: 53991
				public static LocString NAME = UI.FormatAsLink("Power", "POWER") + " Usage:";

				// Token: 0x0400D2E8 RID: 53992
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was created by {1} over the course of the day";

				// Token: 0x0400D2E9 RID: 53993
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x020030AD RID: 12461
			public class ENERGY_WASTED
			{
				// Token: 0x0400D2EA RID: 53994
				public static LocString NAME = UI.FormatAsLink("Power", "POWER") + " Wasted:";

				// Token: 0x0400D2EB RID: 53995
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was lost today due to battery runoff and overproduction in {1}";
			}

			// Token: 0x020030AE RID: 12462
			public class LEVEL_UP
			{
				// Token: 0x0400D2EC RID: 53996
				public static LocString NAME = "Attribute Increases:";

				// Token: 0x0400D2ED RID: 53997
				public static LocString TOOLTIP = "Today {1} gained a total of {0} attribute levels";
			}

			// Token: 0x020030AF RID: 12463
			public class TOILET_INCIDENT
			{
				// Token: 0x0400D2EE RID: 53998
				public static LocString NAME = "Restroom Accidents:";

				// Token: 0x0400D2EF RID: 53999
				public static LocString TOOLTIP = "{0} Duplicants couldn't quite reach the toilet in time today";
			}

			// Token: 0x020030B0 RID: 12464
			public class DISEASE_ADDED
			{
				// Token: 0x0400D2F0 RID: 54000
				public static LocString NAME = UI.FormatAsLink("Diseases", "DISEASE") + " Contracted:";

				// Token: 0x0400D2F1 RID: 54001
				public static LocString POSITIVE_TOOLTIP = "{0} " + UI.FormatAsLink("Disease", "DISEASE") + " were contracted by {1}";

				// Token: 0x0400D2F2 RID: 54002
				public static LocString NEGATIVE_TOOLTIP = "{0} " + UI.FormatAsLink("Disease", "DISEASE") + " were cured by {1}";
			}

			// Token: 0x020030B1 RID: 12465
			public class CONTAMINATED_OXYGEN_FLATULENCE
			{
				// Token: 0x0400D2F3 RID: 54003
				public static LocString NAME = UI.FormatAsLink("Flatulence", "CONTAMINATEDOXYGEN") + " Generation:";

				// Token: 0x0400D2F4 RID: 54004
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400D2F5 RID: 54005
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x020030B2 RID: 12466
			public class CONTAMINATED_OXYGEN_TOILET
			{
				// Token: 0x0400D2F6 RID: 54006
				public static LocString NAME = UI.FormatAsLink("Toilet Emissions: ", "CONTAMINATEDOXYGEN");

				// Token: 0x0400D2F7 RID: 54007
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400D2F8 RID: 54008
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x020030B3 RID: 12467
			public class CONTAMINATED_OXYGEN_SUBLIMATION
			{
				// Token: 0x0400D2F9 RID: 54009
				public static LocString NAME = UI.FormatAsLink("Sublimation", "CONTAMINATEDOXYGEN") + ":";

				// Token: 0x0400D2FA RID: 54010
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400D2FB RID: 54011
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x020030B4 RID: 12468
			public class DISEASE_STATUS
			{
				// Token: 0x0400D2FC RID: 54012
				public static LocString NAME = "Disease Status:";

				// Token: 0x0400D2FD RID: 54013
				public static LocString TOOLTIP = "There are {0} covering {1}";
			}

			// Token: 0x020030B5 RID: 12469
			public class CHORE_STATUS
			{
				// Token: 0x0400D2FE RID: 54014
				public static LocString NAME = "Errands:";

				// Token: 0x0400D2FF RID: 54015
				public static LocString POSITIVE_TOOLTIP = "{0} errands are queued for {1}";

				// Token: 0x0400D300 RID: 54016
				public static LocString NEGATIVE_TOOLTIP = "{0} errands were completed over the course of the day by {1}";
			}

			// Token: 0x020030B6 RID: 12470
			public class NOTES
			{
				// Token: 0x0400D301 RID: 54017
				public static LocString NOTE_ENTRY_LINE_ITEM = "{0}\n{1}: {2}";

				// Token: 0x0400D302 RID: 54018
				public static LocString BUTCHERED = "Butchered for {0}";

				// Token: 0x0400D303 RID: 54019
				public static LocString BUTCHERED_CONTEXT = "Butchered";

				// Token: 0x0400D304 RID: 54020
				public static LocString CRAFTED = "Crafted a {0}";

				// Token: 0x0400D305 RID: 54021
				public static LocString CRAFTED_USED = "{0} used as ingredient";

				// Token: 0x0400D306 RID: 54022
				public static LocString CRAFTED_CONTEXT = "Crafted";

				// Token: 0x0400D307 RID: 54023
				public static LocString HARVESTED = "Harvested {0}";

				// Token: 0x0400D308 RID: 54024
				public static LocString HARVESTED_CONTEXT = "Harvested";

				// Token: 0x0400D309 RID: 54025
				public static LocString EATEN = "{0} eaten";

				// Token: 0x0400D30A RID: 54026
				public static LocString ROTTED = "Rotten {0}";

				// Token: 0x0400D30B RID: 54027
				public static LocString ROTTED_CONTEXT = "Rotted";

				// Token: 0x0400D30C RID: 54028
				public static LocString GERMS = "On {0}";

				// Token: 0x0400D30D RID: 54029
				public static LocString TIME_SPENT = "{0}";

				// Token: 0x0400D30E RID: 54030
				public static LocString WORK_TIME = "{0}";

				// Token: 0x0400D30F RID: 54031
				public static LocString PERSONAL_TIME = "{0}";

				// Token: 0x0400D310 RID: 54032
				public static LocString FOODFIGHT_CONTEXT = "{0} ingested in food fight";
			}
		}

		// Token: 0x02002555 RID: 9557
		public static class SCHEDULEBLOCKTYPES
		{
			// Token: 0x020030B7 RID: 12471
			public static class EAT
			{
				// Token: 0x0400D311 RID: 54033
				public static LocString NAME = "Mealtime";

				// Token: 0x0400D312 RID: 54034
				public static LocString DESCRIPTION = "EAT:\nDuring Mealtime Duplicants will head to their assigned mess halls and eat.";
			}

			// Token: 0x020030B8 RID: 12472
			public static class SLEEP
			{
				// Token: 0x0400D313 RID: 54035
				public static LocString NAME = "Sleep";

				// Token: 0x0400D314 RID: 54036
				public static LocString DESCRIPTION = "SLEEP:\nWhen it's time to sleep, Duplicants will head to their assigned rooms and rest.";
			}

			// Token: 0x020030B9 RID: 12473
			public static class WORK
			{
				// Token: 0x0400D315 RID: 54037
				public static LocString NAME = "Work";

				// Token: 0x0400D316 RID: 54038
				public static LocString DESCRIPTION = "WORK:\nDuring Work hours Duplicants will perform any pending errands in the colony.";
			}

			// Token: 0x020030BA RID: 12474
			public static class RECREATION
			{
				// Token: 0x0400D317 RID: 54039
				public static LocString NAME = "Recreation";

				// Token: 0x0400D318 RID: 54040
				public static LocString DESCRIPTION = "HAMMER TIME:\nDuring Hammer Time, Duplicants will relieve their " + UI.FormatAsLink("Stress", "STRESS") + " through dance. Please be aware that no matter how hard my Duplicants try, they will absolutely not be able to touch this.";
			}

			// Token: 0x020030BB RID: 12475
			public static class HYGIENE
			{
				// Token: 0x0400D319 RID: 54041
				public static LocString NAME = "Hygiene";

				// Token: 0x0400D31A RID: 54042
				public static LocString DESCRIPTION = "HYGIENE:\nDuring " + UI.FormatAsLink("Hygiene", "HYGIENE") + " hours Duplicants will head to their assigned washrooms to get cleaned up.";
			}
		}

		// Token: 0x02002556 RID: 9558
		public static class SCHEDULEGROUPS
		{
			// Token: 0x0400A894 RID: 43156
			public static LocString TOOLTIP_FORMAT = "{0}\n\n{1}";

			// Token: 0x0400A895 RID: 43157
			public static LocString MISSINGBLOCKS = "Warning: Scheduling Issues ({0})";

			// Token: 0x0400A896 RID: 43158
			public static LocString NOTIME = "No {0} shifts allotted";

			// Token: 0x020030BC RID: 12476
			public static class HYGENE
			{
				// Token: 0x0400D31B RID: 54043
				public static LocString NAME = "Bathtime";

				// Token: 0x0400D31C RID: 54044
				public static LocString DESCRIPTION = "During Bathtime shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands.\n\nOnce they're all caught up on personal hygiene, Duplicants will head back to work.";

				// Token: 0x0400D31D RID: 54045
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Bathtime",
					UI.PST_KEYWORD,
					" shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands."
				});
			}

			// Token: 0x020030BD RID: 12477
			public static class WORKTIME
			{
				// Token: 0x0400D31E RID: 54046
				public static LocString NAME = "Work";

				// Token: 0x0400D31F RID: 54047
				public static LocString DESCRIPTION = "During Work shifts my Duplicants must perform the errands I have placed for them throughout the colony.\n\nIt's important when scheduling to maintain a good work-life balance for my Duplicants to maintain their health and prevent Morale loss.";

				// Token: 0x0400D320 RID: 54048
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Work",
					UI.PST_KEYWORD,
					" shifts my Duplicants must perform the errands I've placed for them throughout the colony."
				});
			}

			// Token: 0x020030BE RID: 12478
			public static class RECREATION
			{
				// Token: 0x0400D321 RID: 54049
				public static LocString NAME = "Downtime";

				// Token: 0x0400D322 RID: 54050
				public static LocString DESCRIPTION = "During Downtime my Duplicants they may do as they please.\n\nThis may include personal matters like bathroom visits or snacking, or they may choose to engage in leisure activities like socializing with friends.\n\nDowntime increases Duplicant Morale.";

				// Token: 0x0400D323 RID: 54051
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts my Duplicants they may do as they please."
				});
			}

			// Token: 0x020030BF RID: 12479
			public static class SLEEP
			{
				// Token: 0x0400D324 RID: 54052
				public static LocString NAME = "Bedtime";

				// Token: 0x0400D325 RID: 54053
				public static LocString DESCRIPTION = "My Duplicants use Bedtime shifts to rest up after a hard day's work.\n\nScheduling too few bedtime shifts may prevent my Duplicants from regaining enough Stamina to make it through the following day.";

				// Token: 0x0400D326 RID: 54054
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"My Duplicants use ",
					UI.PRE_KEYWORD,
					"Bedtime",
					UI.PST_KEYWORD,
					" shifts to rest up after a hard day's work."
				});
			}
		}

		// Token: 0x02002557 RID: 9559
		public class ELEMENTAL
		{
			// Token: 0x020030C0 RID: 12480
			public class AGE
			{
				// Token: 0x0400D327 RID: 54055
				public static LocString NAME = "Age: {0}";

				// Token: 0x0400D328 RID: 54056
				public static LocString TOOLTIP = "The selected object is {0} cycles old";

				// Token: 0x0400D329 RID: 54057
				public static LocString UNKNOWN = "Unknown";

				// Token: 0x0400D32A RID: 54058
				public static LocString UNKNOWN_TOOLTIP = "The age of the selected object is unknown";
			}

			// Token: 0x020030C1 RID: 12481
			public class UPTIME
			{
				// Token: 0x0400D32B RID: 54059
				public static LocString NAME = "Uptime:\n{0}{1}: {2}\n{0}{3}: {4}\n{0}{5}: {6}";

				// Token: 0x0400D32C RID: 54060
				public static LocString THIS_CYCLE = "This Cycle";

				// Token: 0x0400D32D RID: 54061
				public static LocString LAST_CYCLE = "Last Cycle";

				// Token: 0x0400D32E RID: 54062
				public static LocString LAST_X_CYCLES = "Last {0} Cycles";
			}

			// Token: 0x020030C2 RID: 12482
			public class PRIMARYELEMENT
			{
				// Token: 0x0400D32F RID: 54063
				public static LocString NAME = "Primary Element: {0}";

				// Token: 0x0400D330 RID: 54064
				public static LocString TOOLTIP = "The selected object is primarily composed of {0}";
			}

			// Token: 0x020030C3 RID: 12483
			public class UNITS
			{
				// Token: 0x0400D331 RID: 54065
				public static LocString NAME = "Stack Units: {0}";

				// Token: 0x0400D332 RID: 54066
				public static LocString TOOLTIP = "This stack contains {0} units of {1}";
			}

			// Token: 0x020030C4 RID: 12484
			public class MASS
			{
				// Token: 0x0400D333 RID: 54067
				public static LocString NAME = "Mass: {0}";

				// Token: 0x0400D334 RID: 54068
				public static LocString TOOLTIP = "The selected object has a mass of {0}";
			}

			// Token: 0x020030C5 RID: 12485
			public class TEMPERATURE
			{
				// Token: 0x0400D335 RID: 54069
				public static LocString NAME = "Temperature: {0}";

				// Token: 0x0400D336 RID: 54070
				public static LocString TOOLTIP = "The selected object's current temperature is {0}";
			}

			// Token: 0x020030C6 RID: 12486
			public class DISEASE
			{
				// Token: 0x0400D337 RID: 54071
				public static LocString NAME = "Disease: {0}";

				// Token: 0x0400D338 RID: 54072
				public static LocString TOOLTIP = "There are {0} on the selected object";
			}

			// Token: 0x020030C7 RID: 12487
			public class SHC
			{
				// Token: 0x0400D339 RID: 54073
				public static LocString NAME = "Specific Heat Capacity: {0}";

				// Token: 0x0400D33A RID: 54074
				public static LocString TOOLTIP = "{SPECIFIC_HEAT_CAPACITY} is required to heat 1 g of the selected object by 1 {TEMPERATURE_UNIT}";
			}

			// Token: 0x020030C8 RID: 12488
			public class THERMALCONDUCTIVITY
			{
				// Token: 0x0400D33B RID: 54075
				public static LocString NAME = "Thermal Conductivity: {0}";

				// Token: 0x0400D33C RID: 54076
				public static LocString TOOLTIP = "This object can conduct heat to other materials at a rate of {THERMAL_CONDUCTIVITY} W for each degree {TEMPERATURE_UNIT} difference\n\nBetween two objects, the rate of heat transfer will be determined by the object with the lowest Thermal Conductivity";

				// Token: 0x02003CC7 RID: 15559
				public class ADJECTIVES
				{
					// Token: 0x0400F110 RID: 61712
					public static LocString VALUE_WITH_ADJECTIVE = "{0} ({1})";

					// Token: 0x0400F111 RID: 61713
					public static LocString VERY_LOW_CONDUCTIVITY = "Highly Insulating";

					// Token: 0x0400F112 RID: 61714
					public static LocString LOW_CONDUCTIVITY = "Insulating";

					// Token: 0x0400F113 RID: 61715
					public static LocString MEDIUM_CONDUCTIVITY = "Conductive";

					// Token: 0x0400F114 RID: 61716
					public static LocString HIGH_CONDUCTIVITY = "Highly Conductive";

					// Token: 0x0400F115 RID: 61717
					public static LocString VERY_HIGH_CONDUCTIVITY = "Extremely Conductive";
				}
			}

			// Token: 0x020030C9 RID: 12489
			public class CONDUCTIVITYBARRIER
			{
				// Token: 0x0400D33D RID: 54077
				public static LocString NAME = "Insulation Thickness: {0}";

				// Token: 0x0400D33E RID: 54078
				public static LocString TOOLTIP = "Thick insulation reduces an object's Thermal Conductivity";
			}

			// Token: 0x020030CA RID: 12490
			public class VAPOURIZATIONPOINT
			{
				// Token: 0x0400D33F RID: 54079
				public static LocString NAME = "Vaporization Point: {0}";

				// Token: 0x0400D340 RID: 54080
				public static LocString TOOLTIP = "The selected object will evaporate into a gas at {0}";
			}

			// Token: 0x020030CB RID: 12491
			public class MELTINGPOINT
			{
				// Token: 0x0400D341 RID: 54081
				public static LocString NAME = "Melting Point: {0}";

				// Token: 0x0400D342 RID: 54082
				public static LocString TOOLTIP = "The selected object will melt into a liquid at {0}";
			}

			// Token: 0x020030CC RID: 12492
			public class OVERHEATPOINT
			{
				// Token: 0x0400D343 RID: 54083
				public static LocString NAME = "Overheat Modifier: {0}";

				// Token: 0x0400D344 RID: 54084
				public static LocString TOOLTIP = "This building will overheat and take damage if its temperature reaches {0}\n\nBuilding with better building materials can increase overheat temperature";
			}

			// Token: 0x020030CD RID: 12493
			public class FREEZEPOINT
			{
				// Token: 0x0400D345 RID: 54085
				public static LocString NAME = "Freeze Point: {0}";

				// Token: 0x0400D346 RID: 54086
				public static LocString TOOLTIP = "The selected object will cool into a solid at {0}";
			}

			// Token: 0x020030CE RID: 12494
			public class DEWPOINT
			{
				// Token: 0x0400D347 RID: 54087
				public static LocString NAME = "Condensation Point: {0}";

				// Token: 0x0400D348 RID: 54088
				public static LocString TOOLTIP = "The selected object will condense into a liquid at {0}";
			}
		}

		// Token: 0x02002558 RID: 9560
		public class IMMIGRANTSCREEN
		{
			// Token: 0x0400A897 RID: 43159
			public static LocString IMMIGRANTSCREENTITLE = "Select a Blueprint";

			// Token: 0x0400A898 RID: 43160
			public static LocString PROCEEDBUTTON = "Print";

			// Token: 0x0400A899 RID: 43161
			public static LocString CANCELBUTTON = "Cancel";

			// Token: 0x0400A89A RID: 43162
			public static LocString REJECTALL = "Reject All";

			// Token: 0x0400A89B RID: 43163
			public static LocString EMBARK = "EMBARK";

			// Token: 0x0400A89C RID: 43164
			public static LocString SELECTDUPLICANTS = "Select {0} Duplicants";

			// Token: 0x0400A89D RID: 43165
			public static LocString SELECTYOURCREW = "CHOOSE THREE DUPLICANTS TO BEGIN";

			// Token: 0x0400A89E RID: 43166
			public static LocString SHUFFLE = "REROLL";

			// Token: 0x0400A89F RID: 43167
			public static LocString SHUFFLETOOLTIP = "Reroll for a different Duplicant";

			// Token: 0x0400A8A0 RID: 43168
			public static LocString BACK = "BACK";

			// Token: 0x0400A8A1 RID: 43169
			public static LocString CONFIRMATIONTITLE = "Reject All Printables?";

			// Token: 0x0400A8A2 RID: 43170
			public static LocString CONFIRMATIONBODY = "The Printing Pod will need time to recharge if I reject these Printables.";

			// Token: 0x0400A8A3 RID: 43171
			public static LocString NAME_YOUR_COLONY = "NAME THE COLONY";

			// Token: 0x0400A8A4 RID: 43172
			public static LocString CARE_PACKAGE_ELEMENT_QUANTITY = "{0} of {1}";

			// Token: 0x0400A8A5 RID: 43173
			public static LocString CARE_PACKAGE_ELEMENT_COUNT = "{0} x {1}";

			// Token: 0x0400A8A6 RID: 43174
			public static LocString CARE_PACKAGE_ELEMENT_COUNT_ONLY = "x {0}";

			// Token: 0x0400A8A7 RID: 43175
			public static LocString CARE_PACKAGE_CURRENT_AMOUNT = "Available: {0}";

			// Token: 0x0400A8A8 RID: 43176
			public static LocString DUPLICATE_COLONY_NAME = "A colony named \"{0}\" already exists";
		}

		// Token: 0x02002559 RID: 9561
		public class METERS
		{
			// Token: 0x020030CF RID: 12495
			public class HEALTH
			{
				// Token: 0x0400D349 RID: 54089
				public static LocString TOOLTIP = "Health";
			}

			// Token: 0x020030D0 RID: 12496
			public class BREATH
			{
				// Token: 0x0400D34A RID: 54090
				public static LocString TOOLTIP = "Oxygen";
			}

			// Token: 0x020030D1 RID: 12497
			public class FUEL
			{
				// Token: 0x0400D34B RID: 54091
				public static LocString TOOLTIP = "Fuel";
			}

			// Token: 0x020030D2 RID: 12498
			public class BATTERY
			{
				// Token: 0x0400D34C RID: 54092
				public static LocString TOOLTIP = "Battery Charge";
			}
		}
	}
}
