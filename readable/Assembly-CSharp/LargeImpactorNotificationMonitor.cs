using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B85 RID: 2949
public class LargeImpactorNotificationMonitor : GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>
{
	// Token: 0x060057F3 RID: 22515 RVA: 0x001FFC18 File Offset: 0x001FDE18
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.undiscovered;
		this.undiscovered.ParamTransition<bool>(this.HasBeenDiscovered, this.discovered, GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.IsTrue).EventHandler(GameHashes.DiscoveredSpace, (LargeImpactorNotificationMonitor.Instance smi) => Game.Instance, new GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.GameEvent.Callback(LargeImpactorNotificationMonitor.OnDuplicantReachedSpace)).EventHandler(GameHashes.DLCPOICompleted, (LargeImpactorNotificationMonitor.Instance smi) => Game.Instance, new GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.GameEvent.Callback(LargeImpactorNotificationMonitor.OnPOIActivated));
		this.discovered.DefaultState(this.discovered.sequence);
		this.discovered.sequence.ParamTransition<bool>(this.SequenceCompleted, this.discovered.notification, GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.IsTrue).Enter(new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.RevealSurface)).Enter(new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.PlaySequence)).EventHandler(GameHashes.SequenceCompleted, new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.CompleteSequence));
		this.discovered.notification.DefaultState(this.discovered.notification.delayEntry);
		this.discovered.notification.delayEntry.ScheduleGoTo(3f, this.discovered.notification.running);
		this.discovered.notification.running.Enter(new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.PlayNotificationEnterSound)).Enter(new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.SetLandingZoneVisualizationToActive)).ScheduleAction("Toggle off the visualization after a delay", 2f, new Action<LargeImpactorNotificationMonitor.Instance>(LargeImpactorNotificationMonitor.FoldTheVisualization)).ToggleNotification((LargeImpactorNotificationMonitor.Instance smi) => smi.notification);
	}

	// Token: 0x060057F4 RID: 22516 RVA: 0x001FFDEF File Offset: 0x001FDFEF
	public static void CompleteSequence(LargeImpactorNotificationMonitor.Instance smi)
	{
		smi.sm.SequenceCompleted.Set(true, smi, false);
	}

	// Token: 0x060057F5 RID: 22517 RVA: 0x001FFE05 File Offset: 0x001FE005
	public static void Discover(LargeImpactorNotificationMonitor.Instance smi)
	{
		smi.sm.HasBeenDiscovered.Set(true, smi, false);
	}

	// Token: 0x060057F6 RID: 22518 RVA: 0x001FFE1B File Offset: 0x001FE01B
	public static void RevealSurface(LargeImpactorNotificationMonitor.Instance smi)
	{
		smi.RevealSurface();
	}

	// Token: 0x060057F7 RID: 22519 RVA: 0x001FFE23 File Offset: 0x001FE023
	public static void PlayNotificationEnterSound(LargeImpactorNotificationMonitor.Instance smi)
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("Notification_Imperative", false));
	}

	// Token: 0x060057F8 RID: 22520 RVA: 0x001FFE35 File Offset: 0x001FE035
	public static void SetLandingZoneVisualizationToActive(LargeImpactorNotificationMonitor.Instance smi)
	{
		smi.GetComponent<LargeImpactorVisualizer>().Active = true;
	}

	// Token: 0x060057F9 RID: 22521 RVA: 0x001FFE44 File Offset: 0x001FE044
	public static void FoldTheVisualization(LargeImpactorNotificationMonitor.Instance smi)
	{
		LargeImpactorVisualizer component = smi.GetComponent<LargeImpactorVisualizer>();
		if (!component.Folded)
		{
			component.SetFoldedState(true);
		}
	}

	// Token: 0x060057FA RID: 22522 RVA: 0x001FFE67 File Offset: 0x001FE067
	public static void OnPOIActivated(LargeImpactorNotificationMonitor.Instance smi, object obj)
	{
		if (((GameObject)obj).PrefabID() == "POIDlc4TechUnlock")
		{
			LargeImpactorNotificationMonitor.Discover(smi);
		}
	}

	// Token: 0x060057FB RID: 22523 RVA: 0x001FFE8C File Offset: 0x001FE08C
	public static void OnDuplicantReachedSpace(LargeImpactorNotificationMonitor.Instance smi, object obj)
	{
		int myWorldId = ((GameObject)obj).GetMyWorldId();
		int myWorldId2 = smi.gameObject.GetMyWorldId();
		if (myWorldId == myWorldId2)
		{
			LargeImpactorNotificationMonitor.Discover(smi);
		}
	}

	// Token: 0x060057FC RID: 22524 RVA: 0x001FFEB9 File Offset: 0x001FE0B9
	public static void PlaySequence(LargeImpactorNotificationMonitor.Instance smi)
	{
		smi.PlaySequence();
	}

	// Token: 0x04003AE8 RID: 15080
	public const string NOTIFICATION_PREFAB_ID = "LargeImpactNotification";

	// Token: 0x04003AE9 RID: 15081
	public GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State undiscovered;

	// Token: 0x04003AEA RID: 15082
	public LargeImpactorNotificationMonitor.DiscoveredStates discovered;

	// Token: 0x04003AEB RID: 15083
	public StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.BoolParameter HasBeenDiscovered;

	// Token: 0x04003AEC RID: 15084
	public StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.BoolParameter SequenceCompleted;

	// Token: 0x02001D08 RID: 7432
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001D09 RID: 7433
	public class NotificationStates : GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State
	{
		// Token: 0x04008A1B RID: 35355
		public GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State delayEntry;

		// Token: 0x04008A1C RID: 35356
		public GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State running;
	}

	// Token: 0x02001D0A RID: 7434
	public class DiscoveredStates : GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State
	{
		// Token: 0x04008A1D RID: 35357
		public GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State sequence;

		// Token: 0x04008A1E RID: 35358
		public LargeImpactorNotificationMonitor.NotificationStates notification;
	}

	// Token: 0x02001D0B RID: 7435
	public new class Instance : GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.GameInstance
	{
		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x0600AFB3 RID: 44979 RVA: 0x003D6F88 File Offset: 0x003D5188
		public bool HasRevealSequencePlayed
		{
			get
			{
				return base.sm.SequenceCompleted.Get(this);
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x0600AFB5 RID: 44981 RVA: 0x003D6FA4 File Offset: 0x003D51A4
		// (set) Token: 0x0600AFB4 RID: 44980 RVA: 0x003D6F9B File Offset: 0x003D519B
		public Notification notification { get; private set; }

		// Token: 0x0600AFB6 RID: 44982 RVA: 0x003D6FAC File Offset: 0x003D51AC
		public Instance(IStateMachineTarget master, LargeImpactorNotificationMonitor.Def def) : base(master, def)
		{
			this.notifier = base.gameObject.AddOrGet<Notifier>();
			LargeImpactorStatus.Instance smi = base.smi.GetSMI<LargeImpactorStatus.Instance>();
			string title = MISC.NOTIFICATIONS.INCOMINGPREHISTORICASTEROIDNOTIFICATION.NAME;
			NotificationType type = NotificationType.Custom;
			object tooltip_data = smi;
			this.notification = new Notification(title, type, new Func<List<Notification>, object, string>(this.ResolveNotificationTooltip), tooltip_data, false, 0f, null, null, null, true, false, false);
			this.notification.customNotificationID = "LargeImpactNotification";
		}

		// Token: 0x0600AFB7 RID: 44983 RVA: 0x003D7020 File Offset: 0x003D5220
		private string ResolveNotificationTooltip(List<Notification> not, object data)
		{
			LargeImpactorStatus.Instance instance = (LargeImpactorStatus.Instance)data;
			return GameUtil.SafeStringFormat(MISC.NOTIFICATIONS.INCOMINGPREHISTORICASTEROIDNOTIFICATION.TOOLTIP, new object[]
			{
				GameUtil.GetFormattedInt((float)instance.Health, GameUtil.TimeSlice.None),
				GameUtil.GetFormattedInt((float)instance.def.MAX_HEALTH, GameUtil.TimeSlice.None),
				GameUtil.GetFormattedCycles(instance.TimeRemainingBeforeCollision, "F1", false)
			});
		}

		// Token: 0x0600AFB8 RID: 44984 RVA: 0x003D7084 File Offset: 0x003D5284
		public void RevealSurface()
		{
			GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.Id, -1);
			if (gameplayEventInstance != null)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(gameplayEventInstance.worldId);
				if (world != null && !world.IsSurfaceRevealed)
				{
					world.RevealSurface();
				}
			}
		}

		// Token: 0x0600AFB9 RID: 44985 RVA: 0x003D70E1 File Offset: 0x003D52E1
		public void SetNotificationVisibility(bool visible)
		{
			if (visible)
			{
				this.notifier.Add(this.notification, "");
				return;
			}
			this.notifier.Remove(this.notification);
		}

		// Token: 0x0600AFBA RID: 44986 RVA: 0x003D7110 File Offset: 0x003D5310
		public void PlaySequence()
		{
			this.AbortSequenceCoroutine();
			this.CreateReticleForSequence();
			GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.Id, -1);
			if (gameplayEventInstance != null)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(gameplayEventInstance.worldId);
				this.sequenceCoroutine = LargeImpactorRevealSequence.Start(this.notifier, this.sequenceReticle, world);
			}
		}

		// Token: 0x0600AFBB RID: 44987 RVA: 0x003D717C File Offset: 0x003D537C
		private void CreateReticleForSequence()
		{
			this.DeleteReticleObject();
			this.sequenceReticle = Util.KInstantiateUI<LargeImpactorSequenceUIReticle>(ScreenPrefabs.Instance.largeImpactorSequenceReticlePrefab.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
			LargeImpactorStatus.Instance smi = base.gameObject.GetSMI<LargeImpactorStatus.Instance>();
			this.sequenceReticle.SetTarget(smi);
		}

		// Token: 0x0600AFBC RID: 44988 RVA: 0x003D71D1 File Offset: 0x003D53D1
		private void DeleteReticleObject()
		{
			if (this.sequenceReticle != null)
			{
				this.sequenceReticle.gameObject.DeleteObject();
			}
		}

		// Token: 0x0600AFBD RID: 44989 RVA: 0x003D71F1 File Offset: 0x003D53F1
		private void AbortSequenceCoroutine()
		{
			if (this.sequenceCoroutine != null)
			{
				this.notifier.StopCoroutine(this.sequenceCoroutine);
				this.sequenceCoroutine = null;
			}
		}

		// Token: 0x0600AFBE RID: 44990 RVA: 0x003D7213 File Offset: 0x003D5413
		protected override void OnCleanUp()
		{
			this.AbortSequenceCoroutine();
			this.DeleteReticleObject();
			base.OnCleanUp();
		}

		// Token: 0x04008A1F RID: 35359
		private Notifier notifier;

		// Token: 0x04008A21 RID: 35361
		private Coroutine sequenceCoroutine;

		// Token: 0x04008A22 RID: 35362
		private LargeImpactorSequenceUIReticle sequenceReticle;
	}
}
