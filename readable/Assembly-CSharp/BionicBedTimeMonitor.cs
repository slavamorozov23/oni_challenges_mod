using System;
using UnityEngine;

// Token: 0x02000A09 RID: 2569
public class BionicBedTimeMonitor : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>
{
	// Token: 0x06004B49 RID: 19273 RVA: 0x001B5A08 File Offset: 0x001B3C08
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.notAllowed;
		this.notAllowed.ScheduleChange(this.bedTime, new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime)).EventTransition(GameHashes.BionicOnline, this.bedTime, new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime));
		this.bedTime.DefaultState(this.bedTime.runChore);
		this.bedTime.runChore.ToggleChore((BionicBedTimeMonitor.Instance smi) => new BionicBedTimeModeChore(smi.master), this.bedTime.choreEnded, this.bedTime.choreEnded).DefaultState(this.bedTime.runChore.notStarted);
		this.bedTime.runChore.notStarted.EventTransition(GameHashes.BeginChore, this.bedTime.runChore.running, new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.ChoreIsRunning)).ScheduleChange(this.notAllowed, GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Not(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime))).EventTransition(GameHashes.BionicOffline, this.notAllowed, null);
		this.bedTime.runChore.running.EventTransition(GameHashes.EndChore, this.bedTime.runChore.notStarted, GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Not(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.ChoreIsRunning))).DefaultState(this.bedTime.runChore.running.traveling);
		this.bedTime.runChore.running.traveling.TagTransition(GameTags.BionicBedTime, this.bedTime.runChore.running.defragmenting, false);
		this.bedTime.runChore.running.defragmenting.TagTransition(GameTags.BionicBedTime, this.bedTime.runChore.running.traveling, true).Enter(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State.Callback(BionicBedTimeMonitor.EnableLight)).Exit(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State.Callback(BionicBedTimeMonitor.DisableLight));
		this.bedTime.choreEnded.ScheduleChange(this.notAllowed, GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Not(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime))).EventTransition(GameHashes.BionicOffline, this.notAllowed, null).GoTo(this.bedTime.runChore);
	}

	// Token: 0x06004B4A RID: 19274 RVA: 0x001B5C61 File Offset: 0x001B3E61
	public static bool CanGoToBedTime(BionicBedTimeMonitor.Instance smi)
	{
		return BionicBedTimeMonitor.IsOnline(smi) && BionicBedTimeMonitor.ScheduleIsInBedTime(smi);
	}

	// Token: 0x06004B4B RID: 19275 RVA: 0x001B5C73 File Offset: 0x001B3E73
	private static void EnableLight(BionicBedTimeMonitor.Instance smi)
	{
		smi.EnableLight();
	}

	// Token: 0x06004B4C RID: 19276 RVA: 0x001B5C7B File Offset: 0x001B3E7B
	private static void DisableLight(BionicBedTimeMonitor.Instance smi)
	{
		smi.DisableLight();
	}

	// Token: 0x06004B4D RID: 19277 RVA: 0x001B5C83 File Offset: 0x001B3E83
	private static bool IsOnline(BionicBedTimeMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x06004B4E RID: 19278 RVA: 0x001B5C8B File Offset: 0x001B3E8B
	private static bool ScheduleIsInBedTime(BionicBedTimeMonitor.Instance smi)
	{
		return smi.IsScheduleInBedTime;
	}

	// Token: 0x06004B4F RID: 19279 RVA: 0x001B5C94 File Offset: 0x001B3E94
	public static bool ChoreIsRunning(BionicBedTimeMonitor.Instance smi)
	{
		ChoreDriver component = smi.GetComponent<ChoreDriver>();
		Chore chore = (component == null) ? null : component.GetCurrentChore();
		return chore != null && chore.choreType == Db.Get().ChoreTypes.BionicBedtimeMode;
	}

	// Token: 0x040031E5 RID: 12773
	private const float LIGHT_RADIUS = 3f;

	// Token: 0x040031E6 RID: 12774
	private const int LIGHT_LUX = 1800;

	// Token: 0x040031E7 RID: 12775
	public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State notAllowed;

	// Token: 0x040031E8 RID: 12776
	public BionicBedTimeMonitor.BedTimeStates bedTime;

	// Token: 0x02001A80 RID: 6784
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A81 RID: 6785
	public class DefragmentingStates : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State
	{
		// Token: 0x040081E2 RID: 33250
		public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State traveling;

		// Token: 0x040081E3 RID: 33251
		public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State defragmenting;
	}

	// Token: 0x02001A82 RID: 6786
	public class ChoreStates : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State
	{
		// Token: 0x040081E4 RID: 33252
		public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State notStarted;

		// Token: 0x040081E5 RID: 33253
		public BionicBedTimeMonitor.DefragmentingStates running;
	}

	// Token: 0x02001A83 RID: 6787
	public class BedTimeStates : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State
	{
		// Token: 0x040081E6 RID: 33254
		public BionicBedTimeMonitor.ChoreStates runChore;

		// Token: 0x040081E7 RID: 33255
		public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State choreEnded;
	}

	// Token: 0x02001A84 RID: 6788
	public new class Instance : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.GameInstance
	{
		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600A5D5 RID: 42453 RVA: 0x003B85A6 File Offset: 0x003B67A6
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600A5D6 RID: 42454 RVA: 0x003B85BD File Offset: 0x003B67BD
		public bool IsBedTimeChoreRunning
		{
			get
			{
				return this.prefabID.HasTag(GameTags.BionicBedTime);
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600A5D7 RID: 42455 RVA: 0x003B85CF File Offset: 0x003B67CF
		public bool IsScheduleInBedTime
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep);
			}
		}

		// Token: 0x0600A5D8 RID: 42456 RVA: 0x003B85EB File Offset: 0x003B67EB
		public Instance(IStateMachineTarget master, BionicBedTimeMonitor.Def def) : base(master, def)
		{
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.prefabID = base.GetComponent<KPrefabID>();
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x0600A5D9 RID: 42457 RVA: 0x003B8620 File Offset: 0x003B6820
		public void EnableLight()
		{
			this.lightSymbolTracker = base.gameObject.AddOrGet<LightSymbolTracker>();
			this.lightSymbolTracker.targetSymbol = "snapTo_mouth";
			this.lightSymbolTracker.enabled = true;
			this.light = base.gameObject.AddOrGet<Light2D>();
			this.light.Lux = 1800;
			this.light.Range = 3f;
			this.light.enabled = true;
			this.light.drawOverlay = true;
			this.light.Color = new Color(0f, 0.3137255f, 1f, 1f);
			this.light.overlayColour = new Color(1f, 1f, 1f, 1f);
			this.light.FullRefresh();
		}

		// Token: 0x0600A5DA RID: 42458 RVA: 0x003B86FB File Offset: 0x003B68FB
		public void DisableLight()
		{
			if (this.light != null)
			{
				this.light.enabled = false;
			}
			if (this.lightSymbolTracker != null)
			{
				this.lightSymbolTracker.enabled = false;
			}
		}

		// Token: 0x040081E8 RID: 33256
		private Light2D light;

		// Token: 0x040081E9 RID: 33257
		private LightSymbolTracker lightSymbolTracker;

		// Token: 0x040081EA RID: 33258
		private BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x040081EB RID: 33259
		private Schedulable schedulable;

		// Token: 0x040081EC RID: 33260
		private KPrefabID prefabID;
	}
}
