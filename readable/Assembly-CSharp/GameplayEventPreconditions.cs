using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;
using Klei.CustomSettings;
using ProcGen;

// Token: 0x020004E4 RID: 1252
public class GameplayEventPreconditions
{
	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06001B06 RID: 6918 RVA: 0x0009459F File Offset: 0x0009279F
	public static GameplayEventPreconditions Instance
	{
		get
		{
			if (GameplayEventPreconditions._instance == null)
			{
				GameplayEventPreconditions._instance = new GameplayEventPreconditions();
			}
			return GameplayEventPreconditions._instance;
		}
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x000945B8 File Offset: 0x000927B8
	public GameplayEventPrecondition LiveMinions(int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Components.LiveMinionIdentities.Count >= count),
			description = string.Format("At least {0} dupes alive", count)
		};
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x00094604 File Offset: 0x00092804
	public GameplayEventPrecondition BuildingExists(string buildingId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => BuildingInventory.Instance.BuildingCount(new Tag(buildingId)) >= count),
			description = string.Format("{0} {1} has been built", count, buildingId)
		};
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x00094660 File Offset: 0x00092860
	public GameplayEventPrecondition ResearchCompleted(string techName)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Research.Instance.Get(Db.Get().Techs.Get(techName)).IsComplete()),
			description = "Has researched " + techName + "."
		};
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x000946AC File Offset: 0x000928AC
	public GameplayEventPrecondition AchievementUnlocked(ColonyAchievement achievement)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => SaveGame.Instance.ColonyAchievementTracker.IsAchievementUnlocked(achievement)),
			description = "Unlocked the " + achievement.Id + " achievement"
		};
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x00094700 File Offset: 0x00092900
	public GameplayEventPrecondition RoomBuilt(RoomType roomType)
	{
		Predicate<global::Room> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				List<global::Room> rooms = Game.Instance.roomProber.rooms;
				Predicate<global::Room> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((global::Room match) => match.roomType == roomType));
				}
				return rooms.Exists(match2);
			},
			description = "Built a " + roomType.Id + " room"
		};
	}

	// Token: 0x06001B0C RID: 6924 RVA: 0x00094754 File Offset: 0x00092954
	public GameplayEventPrecondition CycleRestriction(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameUtil.GetCurrentTimeInCycles() >= min && GameUtil.GetCurrentTimeInCycles() <= max),
			description = string.Format("After cycle {0} and before cycle {1}", min, max)
		};
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x000947B4 File Offset: 0x000929B4
	public GameplayEventPrecondition MinionsWithEffect(string effectId, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<Effects>().Get(effectId) != null));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} effect applied", count, effectId)
		};
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x00094810 File Offset: 0x00092A10
	public GameplayEventPrecondition MinionsWithStatusItem(StatusItem statusItem, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<KSelectable>().HasStatusItem(statusItem)));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} status item", count, statusItem)
		};
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x0009486C File Offset: 0x00092A6C
	public GameplayEventPrecondition MinionsWithChoreGroupPriorityOrGreater(ChoreGroup choreGroup, int count, int priority)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(MinionIdentity minion)
					{
						ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
						return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
					});
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have their {1} set to {2} or higher.", count, choreGroup.Name, priority)
		};
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x000948DC File Offset: 0x00092ADC
	public GameplayEventPrecondition PastEventCount(string evtId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evtId) >= count),
			description = string.Format("The {0} event has triggered {1} times.", evtId, count)
		};
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x00094938 File Offset: 0x00092B38
	public GameplayEventPrecondition DifficultySetting(SettingConfig config, string levelId)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => CustomGameSettings.Instance.GetCurrentQualitySetting(config).id == levelId),
			description = string.Concat(new string[]
			{
				"The config ",
				config.id,
				" is level ",
				levelId,
				"."
			})
		};
	}

	// Token: 0x06001B12 RID: 6930 RVA: 0x000949B4 File Offset: 0x00092BB4
	public GameplayEventPrecondition ClusterHasTag(string tag)
	{
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
				return currentClusterLayout != null && currentClusterLayout.clusterTags.Contains(tag);
			},
			description = "The cluster is tagged with " + tag + "."
		};
	}

	// Token: 0x06001B13 RID: 6931 RVA: 0x00094A00 File Offset: 0x00092C00
	public GameplayEventPrecondition PastEventCountAndNotActive(GameplayEvent evt, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evt.IdHash) >= count && !GameplayEventManager.Instance.IsGameplayEventActive(evt)),
			description = string.Format("The {0} event has triggered {1} times and is not active.", evt.Id, count)
		};
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x00094A60 File Offset: 0x00092C60
	public GameplayEventPrecondition Not(GameplayEventPrecondition precondition)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => !precondition.condition()),
			description = "Not[" + precondition.description + "]"
		};
	}

	// Token: 0x06001B15 RID: 6933 RVA: 0x00094AB4 File Offset: 0x00092CB4
	public GameplayEventPrecondition Or(GameplayEventPrecondition precondition1, GameplayEventPrecondition precondition2)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => precondition1.condition() || precondition2.condition()),
			description = string.Concat(new string[]
			{
				"[",
				precondition1.description,
				"]-OR-[",
				precondition2.description,
				"]"
			})
		};
	}

	// Token: 0x04000F93 RID: 3987
	private static GameplayEventPreconditions _instance;
}
