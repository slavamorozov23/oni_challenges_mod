using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x02000968 RID: 2408
public class GameplayEventManager : KMonoBehaviour
{
	// Token: 0x06004462 RID: 17506 RVA: 0x0018B001 File Offset: 0x00189201
	public static void DestroyInstance()
	{
		GameplayEventManager.Instance = null;
	}

	// Token: 0x06004463 RID: 17507 RVA: 0x0018B009 File Offset: 0x00189209
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameplayEventManager.Instance = this;
		this.notifier = base.GetComponent<Notifier>();
	}

	// Token: 0x06004464 RID: 17508 RVA: 0x0018B023 File Offset: 0x00189223
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreEvents();
	}

	// Token: 0x06004465 RID: 17509 RVA: 0x0018B031 File Offset: 0x00189231
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameplayEventManager.Instance = null;
	}

	// Token: 0x06004466 RID: 17510 RVA: 0x0018B040 File Offset: 0x00189240
	private void RestoreEvents()
	{
		this.activeEvents.RemoveAll((GameplayEventInstance x) => Db.Get().GameplayEvents.TryGet(x.eventID) == null);
		for (int i = this.activeEvents.Count - 1; i >= 0; i--)
		{
			GameplayEventInstance gameplayEventInstance = this.activeEvents[i];
			if (gameplayEventInstance.smi == null)
			{
				this.StartEventInstance(gameplayEventInstance, null);
			}
		}
	}

	// Token: 0x06004467 RID: 17511 RVA: 0x0018B0AD File Offset: 0x001892AD
	public void SetSleepTimerForEvent(GameplayEvent eventType, float time)
	{
		this.sleepTimers[eventType.IdHash] = time;
	}

	// Token: 0x06004468 RID: 17512 RVA: 0x0018B0C4 File Offset: 0x001892C4
	public float GetSleepTimer(GameplayEvent eventType)
	{
		float num = 0f;
		this.sleepTimers.TryGetValue(eventType.IdHash, out num);
		this.sleepTimers[eventType.IdHash] = num;
		return num;
	}

	// Token: 0x06004469 RID: 17513 RVA: 0x0018B100 File Offset: 0x00189300
	public bool IsGameplayEventActive(GameplayEvent eventType)
	{
		return this.activeEvents.Find((GameplayEventInstance e) => e.eventID == eventType.IdHash) != null;
	}

	// Token: 0x0600446A RID: 17514 RVA: 0x0018B134 File Offset: 0x00189334
	public bool IsGameplayEventRunningWithTag(Tag tag)
	{
		using (List<GameplayEventInstance>.Enumerator enumerator = this.activeEvents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.tags.Contains(tag))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600446B RID: 17515 RVA: 0x0018B194 File Offset: 0x00189394
	public void GetActiveEventsOfType<T>(int worldID, ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance gameplayEventInstance in this.activeEvents)
		{
			if (gameplayEventInstance.worldId == worldID && gameplayEventInstance.gameplayEvent is T)
			{
				results.Add(gameplayEventInstance);
			}
		}
	}

	// Token: 0x0600446C RID: 17516 RVA: 0x0018B208 File Offset: 0x00189408
	public void GetActiveEventsOfType<T>(ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance gameplayEventInstance in this.activeEvents)
		{
			if (gameplayEventInstance.gameplayEvent is T)
			{
				results.Add(gameplayEventInstance);
			}
		}
	}

	// Token: 0x0600446D RID: 17517 RVA: 0x0018B274 File Offset: 0x00189474
	private GameplayEventInstance CreateGameplayEvent(GameplayEvent gameplayEvent, int worldId)
	{
		return gameplayEvent.CreateInstance(worldId);
	}

	// Token: 0x0600446E RID: 17518 RVA: 0x0018B280 File Offset: 0x00189480
	public GameplayEventInstance GetGameplayEventInstance(HashedString eventID, int worldId = -1)
	{
		return this.activeEvents.Find((GameplayEventInstance e) => e.eventID == eventID && (worldId == -1 || e.worldId == worldId));
	}

	// Token: 0x0600446F RID: 17519 RVA: 0x0018B2B8 File Offset: 0x001894B8
	public GameplayEventInstance CreateOrGetEventInstance(GameplayEvent eventType, int worldId = -1)
	{
		GameplayEventInstance gameplayEventInstance = this.GetGameplayEventInstance(eventType.Id, worldId);
		if (gameplayEventInstance == null)
		{
			gameplayEventInstance = this.StartNewEvent(eventType, worldId, null);
		}
		return gameplayEventInstance;
	}

	// Token: 0x06004470 RID: 17520 RVA: 0x0018B2E8 File Offset: 0x001894E8
	public void RemoveActiveEvent(GameplayEventInstance eventInstance, string reason = "RemoveActiveEvent() called")
	{
		GameplayEventInstance gameplayEventInstance = this.activeEvents.Find((GameplayEventInstance x) => x == eventInstance);
		if (gameplayEventInstance != null)
		{
			if (gameplayEventInstance.smi != null)
			{
				gameplayEventInstance.smi.StopSM(reason);
				return;
			}
			this.activeEvents.Remove(gameplayEventInstance);
		}
	}

	// Token: 0x06004471 RID: 17521 RVA: 0x0018B340 File Offset: 0x00189540
	public GameplayEventInstance StartNewEvent(GameplayEvent eventType, int worldId = -1, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		GameplayEventInstance gameplayEventInstance = this.CreateGameplayEvent(eventType, worldId);
		this.StartEventInstance(gameplayEventInstance, setupActionsBeforeStart);
		this.activeEvents.Add(gameplayEventInstance);
		int num;
		this.pastEvents.TryGetValue(gameplayEventInstance.eventID, out num);
		this.pastEvents[gameplayEventInstance.eventID] = num + 1;
		return gameplayEventInstance;
	}

	// Token: 0x06004472 RID: 17522 RVA: 0x0018B394 File Offset: 0x00189594
	private void StartEventInstance(GameplayEventInstance gameplayEventInstance, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		StateMachine.Instance instance = gameplayEventInstance.PrepareEvent(this);
		StateMachine.Instance instance2 = instance;
		instance2.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(instance2.OnStop, new Action<string, StateMachine.Status>(delegate(string reason, StateMachine.Status status)
		{
			this.activeEvents.Remove(gameplayEventInstance);
		}));
		if (setupActionsBeforeStart != null)
		{
			setupActionsBeforeStart(instance);
		}
		gameplayEventInstance.StartEvent();
	}

	// Token: 0x06004473 RID: 17523 RVA: 0x0018B3FC File Offset: 0x001895FC
	public int NumberOfPastEvents(HashedString eventID)
	{
		int result;
		this.pastEvents.TryGetValue(eventID, out result);
		return result;
	}

	// Token: 0x06004474 RID: 17524 RVA: 0x0018B41C File Offset: 0x0018961C
	public static Notification CreateStandardCancelledNotification(EventInfoData eventInfoData)
	{
		if (eventInfoData == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"eventPopup is null in CreateStandardCancelledNotification"
			});
			return null;
		}
		eventInfoData.FinalizeText();
		return new Notification(string.Format(GAMEPLAY_EVENTS.CANCELED, eventInfoData.title), NotificationType.Event, (List<Notification> list, object data) => string.Format(GAMEPLAY_EVENTS.CANCELED_TOOLTIP, eventInfoData.title), null, true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x04002DFB RID: 11771
	public static GameplayEventManager Instance;

	// Token: 0x04002DFC RID: 11772
	public Notifier notifier;

	// Token: 0x04002DFD RID: 11773
	[Serialize]
	private List<GameplayEventInstance> activeEvents = new List<GameplayEventInstance>();

	// Token: 0x04002DFE RID: 11774
	[Serialize]
	private Dictionary<HashedString, int> pastEvents = new Dictionary<HashedString, int>();

	// Token: 0x04002DFF RID: 11775
	[Serialize]
	private Dictionary<HashedString, float> sleepTimers = new Dictionary<HashedString, float>();
}
