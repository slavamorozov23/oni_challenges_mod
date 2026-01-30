using System;
using Database;

// Token: 0x020004E2 RID: 1250
public class GameplayEventMinionFilters
{
	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06001AFC RID: 6908 RVA: 0x0009436A File Offset: 0x0009256A
	public static GameplayEventMinionFilters Instance
	{
		get
		{
			if (GameplayEventMinionFilters._instance == null)
			{
				GameplayEventMinionFilters._instance = new GameplayEventMinionFilters();
			}
			return GameplayEventMinionFilters._instance;
		}
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x00094384 File Offset: 0x00092584
	public GameplayEventMinionFilter HasMasteredSkill(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasMasteredSkill(skill.Id)),
			id = "HasMasteredSkill"
		};
	}

	// Token: 0x06001AFE RID: 6910 RVA: 0x000943C0 File Offset: 0x000925C0
	public GameplayEventMinionFilter HasSkillAptitude(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasSkillAptitude(skill)),
			id = "HasSkillAptitude"
		};
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x000943FC File Offset: 0x000925FC
	public GameplayEventMinionFilter HasChoreGroupPriorityOrHigher(ChoreGroup choreGroup, int priority)
	{
		return new GameplayEventMinionFilter
		{
			filter = delegate(MinionIdentity minion)
			{
				ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
				return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
			},
			id = "HasChoreGroupPriorityOrHigher"
		};
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x00094440 File Offset: 0x00092640
	public GameplayEventMinionFilter AgeRange(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.arrivalTime >= min && minion.arrivalTime <= max),
			id = "AgeRange"
		};
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x00094483 File Offset: 0x00092683
	public GameplayEventMinionFilter PriorityIn()
	{
		GameplayEventMinionFilter gameplayEventMinionFilter = new GameplayEventMinionFilter();
		gameplayEventMinionFilter.filter = ((MinionIdentity minion) => true);
		gameplayEventMinionFilter.id = "PriorityIn";
		return gameplayEventMinionFilter;
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x000944BC File Offset: 0x000926BC
	public GameplayEventMinionFilter Not(GameplayEventMinionFilter filter)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => !filter.filter(minion)),
			id = "Not[" + filter.id + "]"
		};
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x00094510 File Offset: 0x00092710
	public GameplayEventMinionFilter Or(GameplayEventMinionFilter precondition1, GameplayEventMinionFilter precondition2)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => precondition1.filter(minion) || precondition2.filter(minion)),
			id = string.Concat(new string[]
			{
				"[",
				precondition1.id,
				"]-OR-[",
				precondition2.id,
				"]"
			})
		};
	}

	// Token: 0x04000F8E RID: 3982
	private static GameplayEventMinionFilters _instance;
}
