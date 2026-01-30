using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020004DE RID: 1246
[DebuggerDisplay("{base.Id}")]
public abstract class GameplayEvent : Resource, IComparable<GameplayEvent>, IHasDlcRestrictions
{
	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x00093CB4 File Offset: 0x00091EB4
	// (set) Token: 0x06001AD4 RID: 6868 RVA: 0x00093CBC File Offset: 0x00091EBC
	public int importance { get; private set; }

	// Token: 0x06001AD5 RID: 6869 RVA: 0x00093CC5 File Offset: 0x00091EC5
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x00093CCD File Offset: 0x00091ECD
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x00093CD8 File Offset: 0x00091ED8
	public virtual bool IsAllowed()
	{
		if (this.WillNeverRunAgain())
		{
			return false;
		}
		if (!this.allowMultipleEventInstances && GameplayEventManager.Instance.IsGameplayEventActive(this))
		{
			return false;
		}
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (gameplayEventPrecondition.required && !gameplayEventPrecondition.condition())
			{
				return false;
			}
		}
		float sleepTimer = GameplayEventManager.Instance.GetSleepTimer(this);
		return GameUtil.GetCurrentTimeInCycles() >= sleepTimer;
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x00093D78 File Offset: 0x00091F78
	public void SetSleepTimer(float timeToSleepUntil)
	{
		GameplayEventManager.Instance.SetSleepTimerForEvent(this, timeToSleepUntil);
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x00093D86 File Offset: 0x00091F86
	public virtual bool WillNeverRunAgain()
	{
		return !Game.IsCorrectDlcActiveForCurrentSave(this) || (this.numTimesAllowed != -1 && GameplayEventManager.Instance.NumberOfPastEvents(this.Id) >= this.numTimesAllowed);
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x00093DBD File Offset: 0x00091FBD
	public int GetCashedPriority()
	{
		return this.calculatedPriority;
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x00093DC5 File Offset: 0x00091FC5
	public virtual int CalculatePriority()
	{
		this.calculatedPriority = this.basePriority + this.CalculateBoost();
		return this.calculatedPriority;
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x00093DE0 File Offset: 0x00091FE0
	public int CalculateBoost()
	{
		int num = 0;
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (!gameplayEventPrecondition.required && gameplayEventPrecondition.condition())
			{
				num += gameplayEventPrecondition.priorityModifier;
			}
		}
		return num;
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x00093E50 File Offset: 0x00092050
	public GameplayEvent AddPrecondition(GameplayEventPrecondition precondition)
	{
		precondition.required = true;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x00093E66 File Offset: 0x00092066
	public GameplayEvent AddPriorityBoost(GameplayEventPrecondition precondition, int priorityBoost)
	{
		precondition.required = false;
		precondition.priorityModifier = priorityBoost;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x00093E83 File Offset: 0x00092083
	public GameplayEvent AddMinionFilter(GameplayEventMinionFilter filter)
	{
		this.minionFilters.Add(filter);
		return this;
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x00093E92 File Offset: 0x00092092
	public GameplayEvent TrySpawnEventOnSuccess(HashedString evt)
	{
		this.successEvents.Add(evt);
		return this;
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x00093EA1 File Offset: 0x000920A1
	public GameplayEvent TrySpawnEventOnFailure(HashedString evt)
	{
		this.failureEvents.Add(evt);
		return this;
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x00093EB0 File Offset: 0x000920B0
	public GameplayEvent SetVisuals(HashedString animFileName)
	{
		this.animFileName = animFileName;
		return this;
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x00093EBA File Offset: 0x000920BA
	public virtual Sprite GetDisplaySprite()
	{
		return null;
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x00093EBD File Offset: 0x000920BD
	public virtual string GetDisplayString()
	{
		return null;
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x00093EC0 File Offset: 0x000920C0
	public MinionIdentity GetRandomFilteredMinion()
	{
		List<MinionIdentity> list = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
		using (List<GameplayEventMinionFilter>.Enumerator enumerator = this.minionFilters.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameplayEventMinionFilter filter = enumerator.Current;
				list.RemoveAll((MinionIdentity x) => !filter.filter(x));
			}
		}
		if (list.Count != 0)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		return null;
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x00093F58 File Offset: 0x00092158
	public MinionIdentity GetRandomMinionPrioritizeFiltered()
	{
		MinionIdentity randomFilteredMinion = this.GetRandomFilteredMinion();
		if (!(randomFilteredMinion == null))
		{
			return randomFilteredMinion;
		}
		return Components.LiveMinionIdentities.Items[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Items.Count)];
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x00093F9C File Offset: 0x0009219C
	public int CompareTo(GameplayEvent other)
	{
		return -this.GetCashedPriority().CompareTo(other.GetCashedPriority());
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x00093FC0 File Offset: 0x000921C0
	public GameplayEvent(string id, int priority, int importance) : base(id, null, null)
	{
		this.tags = new List<Tag>();
		this.basePriority = priority;
		this.preconditions = new List<GameplayEventPrecondition>();
		this.minionFilters = new List<GameplayEventMinionFilter>();
		this.successEvents = new List<HashedString>();
		this.failureEvents = new List<HashedString>();
		this.importance = importance;
		this.animFileName = id;
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x0009402E File Offset: 0x0009222E
	public GameplayEvent(string id, int priority, int importance, string[] requiredDlcIds, string[] forbiddenDlcIds = null) : this(id, priority, importance)
	{
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06001AEA RID: 6890
	public abstract StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance);

	// Token: 0x06001AEB RID: 6891 RVA: 0x0009404C File Offset: 0x0009224C
	public GameplayEventInstance CreateInstance(int worldId)
	{
		GameplayEventInstance gameplayEventInstance = new GameplayEventInstance(this, worldId);
		if (this.tags != null)
		{
			gameplayEventInstance.tags.AddRange(this.tags);
		}
		return gameplayEventInstance;
	}

	// Token: 0x04000F73 RID: 3955
	public const int INFINITE = -1;

	// Token: 0x04000F74 RID: 3956
	public int numTimesAllowed = -1;

	// Token: 0x04000F75 RID: 3957
	public bool allowMultipleEventInstances;

	// Token: 0x04000F76 RID: 3958
	protected int basePriority;

	// Token: 0x04000F77 RID: 3959
	protected int calculatedPriority;

	// Token: 0x04000F79 RID: 3961
	public List<GameplayEventPrecondition> preconditions;

	// Token: 0x04000F7A RID: 3962
	public List<GameplayEventMinionFilter> minionFilters;

	// Token: 0x04000F7B RID: 3963
	public List<HashedString> successEvents;

	// Token: 0x04000F7C RID: 3964
	public List<HashedString> failureEvents;

	// Token: 0x04000F7D RID: 3965
	public string title;

	// Token: 0x04000F7E RID: 3966
	public string description;

	// Token: 0x04000F7F RID: 3967
	public HashedString animFileName;

	// Token: 0x04000F80 RID: 3968
	public List<Tag> tags;

	// Token: 0x04000F81 RID: 3969
	private string[] requiredDlcIds;

	// Token: 0x04000F82 RID: 3970
	private string[] forbiddenDlcIds;
}
