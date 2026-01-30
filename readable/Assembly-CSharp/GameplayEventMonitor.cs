using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000A27 RID: 2599
public class GameplayEventMonitor : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>
{
	// Token: 0x06004BFB RID: 19451 RVA: 0x001B9978 File Offset: 0x001B7B78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
		default_state = this.idle;
		this.root.EventHandler(GameHashes.GameplayEventMonitorStart, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnMonitorStart(data);
		}).EventHandler(GameHashes.GameplayEventMonitorEnd, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnMonitorEnd(data);
		}).EventHandler(GameHashes.GameplayEventMonitorChanged, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			this.UpdateFX(smi);
		});
		this.idle.EventTransition(GameHashes.GameplayEventMonitorStart, this.activeState, new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.HasEvents)).Enter(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State.Callback(this.UpdateEventDisplay));
		this.activeState.DefaultState(this.activeState.unseenEvents);
		this.activeState.unseenEvents.ToggleFX(new Func<GameplayEventMonitor.Instance, StateMachine.Instance>(this.CreateFX)).EventHandler(GameHashes.SelectObject, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnSelect(data);
		}).EventTransition(GameHashes.GameplayEventMonitorChanged, this.activeState.seenAllEvents, new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.SeenAll));
		this.activeState.seenAllEvents.EventTransition(GameHashes.GameplayEventMonitorStart, this.activeState.unseenEvents, GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Not(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.SeenAll))).Enter(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State.Callback(this.UpdateEventDisplay));
	}

	// Token: 0x06004BFC RID: 19452 RVA: 0x001B9AF8 File Offset: 0x001B7CF8
	private bool HasEvents(GameplayEventMonitor.Instance smi)
	{
		return smi.events.Count > 0;
	}

	// Token: 0x06004BFD RID: 19453 RVA: 0x001B9B08 File Offset: 0x001B7D08
	private bool SeenAll(GameplayEventMonitor.Instance smi)
	{
		return smi.UnseenCount() == 0;
	}

	// Token: 0x06004BFE RID: 19454 RVA: 0x001B9B13 File Offset: 0x001B7D13
	private void UpdateFX(GameplayEventMonitor.Instance smi)
	{
		if (smi.fx != null)
		{
			smi.fx.sm.notificationCount.Set(smi.UnseenCount(), smi.fx, false);
		}
	}

	// Token: 0x06004BFF RID: 19455 RVA: 0x001B9B40 File Offset: 0x001B7D40
	private GameplayEventFX.Instance CreateFX(GameplayEventMonitor.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			smi.fx = new GameplayEventFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f));
			return smi.fx;
		}
		return null;
	}

	// Token: 0x06004C00 RID: 19456 RVA: 0x001B9B78 File Offset: 0x001B7D78
	public void UpdateEventDisplay(GameplayEventMonitor.Instance smi)
	{
		if (smi.events.Count == 0 || smi.UnseenCount() > 0)
		{
			NameDisplayScreen.Instance.SetGameplayEventDisplay(smi.master.gameObject, false, null, null);
			return;
		}
		int num = -1;
		GameplayEvent gameplayEvent = null;
		foreach (GameplayEventInstance gameplayEventInstance in smi.events)
		{
			Sprite displaySprite = gameplayEventInstance.gameplayEvent.GetDisplaySprite();
			if (gameplayEventInstance.gameplayEvent.importance > num && displaySprite != null)
			{
				num = gameplayEventInstance.gameplayEvent.importance;
				gameplayEvent = gameplayEventInstance.gameplayEvent;
			}
		}
		if (gameplayEvent != null)
		{
			NameDisplayScreen.Instance.SetGameplayEventDisplay(smi.master.gameObject, true, gameplayEvent.GetDisplayString(), gameplayEvent.GetDisplaySprite());
		}
	}

	// Token: 0x04003269 RID: 12905
	public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State idle;

	// Token: 0x0400326A RID: 12906
	public GameplayEventMonitor.ActiveState activeState;

	// Token: 0x02001AE0 RID: 6880
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001AE1 RID: 6881
	public class ActiveState : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State
	{
		// Token: 0x04008307 RID: 33543
		public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State unseenEvents;

		// Token: 0x04008308 RID: 33544
		public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State seenAllEvents;
	}

	// Token: 0x02001AE2 RID: 6882
	public new class Instance : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.GameInstance
	{
		// Token: 0x0600A778 RID: 42872 RVA: 0x003BC5BB File Offset: 0x003BA7BB
		public Instance(IStateMachineTarget master, GameplayEventMonitor.Def def) : base(master, def)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x0600A779 RID: 42873 RVA: 0x003BC5E4 File Offset: 0x003BA7E4
		public void OnMonitorStart(object data)
		{
			GameplayEventInstance gameplayEventInstance = data as GameplayEventInstance;
			if (!this.events.Contains(gameplayEventInstance))
			{
				this.events.Add(gameplayEventInstance);
				gameplayEventInstance.RegisterMonitorCallback(base.gameObject);
			}
			base.smi.sm.UpdateFX(base.smi);
			base.smi.sm.UpdateEventDisplay(base.smi);
		}

		// Token: 0x0600A77A RID: 42874 RVA: 0x003BC64C File Offset: 0x003BA84C
		public void OnMonitorEnd(object data)
		{
			GameplayEventInstance gameplayEventInstance = data as GameplayEventInstance;
			if (this.events.Contains(gameplayEventInstance))
			{
				this.events.Remove(gameplayEventInstance);
				gameplayEventInstance.UnregisterMonitorCallback(base.gameObject);
			}
			base.smi.sm.UpdateFX(base.smi);
			base.smi.sm.UpdateEventDisplay(base.smi);
			if (this.events.Count == 0)
			{
				base.smi.GoTo(base.sm.idle);
			}
		}

		// Token: 0x0600A77B RID: 42875 RVA: 0x003BC6D8 File Offset: 0x003BA8D8
		public void OnSelect(object data)
		{
			if (!((Boxed<bool>)data).value)
			{
				return;
			}
			foreach (GameplayEventInstance gameplayEventInstance in this.events)
			{
				if (!gameplayEventInstance.seenNotification && gameplayEventInstance.GetEventPopupData != null)
				{
					gameplayEventInstance.seenNotification = true;
					EventInfoScreen.ShowPopup(gameplayEventInstance.GetEventPopupData());
					break;
				}
			}
			if (this.UnseenCount() == 0)
			{
				base.smi.GoTo(base.sm.activeState.seenAllEvents);
			}
		}

		// Token: 0x0600A77C RID: 42876 RVA: 0x003BC780 File Offset: 0x003BA980
		public int UnseenCount()
		{
			return this.events.Count((GameplayEventInstance evt) => !evt.seenNotification);
		}

		// Token: 0x04008309 RID: 33545
		public List<GameplayEventInstance> events = new List<GameplayEventInstance>();

		// Token: 0x0400830A RID: 33546
		public GameplayEventFX.Instance fx;
	}
}
