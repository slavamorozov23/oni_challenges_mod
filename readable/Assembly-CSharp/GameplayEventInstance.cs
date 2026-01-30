using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020004E0 RID: 1248
[SerializationConfig(MemberSerialization.OptIn)]
public class GameplayEventInstance : ISaveLoadable
{
	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06001AED RID: 6893 RVA: 0x0009408A File Offset: 0x0009228A
	// (set) Token: 0x06001AEE RID: 6894 RVA: 0x00094092 File Offset: 0x00092292
	public StateMachine.Instance smi { get; private set; }

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06001AEF RID: 6895 RVA: 0x0009409B File Offset: 0x0009229B
	// (set) Token: 0x06001AF0 RID: 6896 RVA: 0x000940A3 File Offset: 0x000922A3
	public bool seenNotification
	{
		get
		{
			return this._seenNotification;
		}
		set
		{
			this._seenNotification = value;
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(-1122598290, this);
			});
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x000940C3 File Offset: 0x000922C3
	public GameplayEvent gameplayEvent
	{
		get
		{
			if (this._gameplayEvent == null)
			{
				this._gameplayEvent = Db.Get().GameplayEvents.TryGet(this.eventID);
			}
			return this._gameplayEvent;
		}
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x000940EE File Offset: 0x000922EE
	public GameplayEventInstance(GameplayEvent gameplayEvent, int worldId)
	{
		this.eventID = gameplayEvent.Id;
		this.tags = new List<Tag>();
		this.eventStartTime = GameUtil.GetCurrentTimeInCycles();
		this.worldId = worldId;
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x00094124 File Offset: 0x00092324
	public StateMachine.Instance PrepareEvent(GameplayEventManager manager)
	{
		this.smi = this.gameplayEvent.GetSMI(manager, this);
		return this.smi;
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x00094140 File Offset: 0x00092340
	public void StartEvent()
	{
		StateMachine.Instance smi = this.smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStop));
		this.smi.StartSM();
		GameplayEventManager.Instance.Trigger(1491341646, this);
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x0009418F File Offset: 0x0009238F
	public void RegisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		if (!this.monitorCallbackObjects.Contains(go))
		{
			this.monitorCallbackObjects.Add(go);
		}
	}

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000941BE File Offset: 0x000923BE
	public void UnregisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		this.monitorCallbackObjects.Remove(go);
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x000941E0 File Offset: 0x000923E0
	public void OnStop(string reason, StateMachine.Status status)
	{
		GameplayEventManager.Instance.Trigger(1287635015, this);
		if (this.monitorCallbackObjects != null)
		{
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(1287635015, this);
			});
		}
		if (status == StateMachine.Status.Success)
		{
			using (List<HashedString>.Enumerator enumerator = this.gameplayEvent.successEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HashedString hashedString = enumerator.Current;
					GameplayEvent gameplayEvent = Db.Get().GameplayEvents.TryGet(hashedString);
					DebugUtil.DevAssert(gameplayEvent != null, string.Format("GameplayEvent {0} is null", hashedString), null);
					if (gameplayEvent != null && gameplayEvent.IsAllowed())
					{
						GameplayEventManager.Instance.StartNewEvent(gameplayEvent, -1, null);
					}
				}
				return;
			}
		}
		if (status == StateMachine.Status.Failed)
		{
			foreach (HashedString hashedString2 in this.gameplayEvent.failureEvents)
			{
				GameplayEvent gameplayEvent2 = Db.Get().GameplayEvents.TryGet(hashedString2);
				DebugUtil.DevAssert(gameplayEvent2 != null, string.Format("GameplayEvent {0} is null", hashedString2), null);
				if (gameplayEvent2 != null && gameplayEvent2.IsAllowed())
				{
					GameplayEventManager.Instance.StartNewEvent(gameplayEvent2, -1, null);
				}
			}
		}
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x00094338 File Offset: 0x00092538
	public float AgeInCycles()
	{
		return GameUtil.GetCurrentTimeInCycles() - this.eventStartTime;
	}

	// Token: 0x04000F84 RID: 3972
	[Serialize]
	public readonly HashedString eventID;

	// Token: 0x04000F85 RID: 3973
	[Serialize]
	public List<Tag> tags;

	// Token: 0x04000F86 RID: 3974
	[Serialize]
	public float eventStartTime;

	// Token: 0x04000F87 RID: 3975
	[Serialize]
	public readonly int worldId;

	// Token: 0x04000F88 RID: 3976
	[Serialize]
	private bool _seenNotification;

	// Token: 0x04000F89 RID: 3977
	public List<GameObject> monitorCallbackObjects;

	// Token: 0x04000F8A RID: 3978
	public GameplayEventInstance.GameplayEventPopupDataCallback GetEventPopupData;

	// Token: 0x04000F8B RID: 3979
	private GameplayEvent _gameplayEvent;

	// Token: 0x02001354 RID: 4948
	// (Invoke) Token: 0x06008B92 RID: 35730
	public delegate EventInfoData GameplayEventPopupDataCallback();
}
