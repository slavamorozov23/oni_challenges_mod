using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B0D RID: 2829
public class ReusableTrap : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>
{
	// Token: 0x0600526B RID: 21099 RVA: 0x001DF9D0 File Offset: 0x001DDBD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.operational;
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).DefaultState(this.noOperational.idle);
		this.noOperational.idle.EnterTransition(this.noOperational.releasing, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ParamTransition<bool>(this.IsArmed, this.noOperational.disarming, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).PlayAnim("off");
		this.noOperational.releasing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.Release)).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.noOperational.idle);
		this.noOperational.disarming.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).PlayAnim("abort_armed").OnAnimQueueComplete(this.noOperational.idle);
		this.operational.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.unarmed);
		this.operational.unarmed.ParamTransition<bool>(this.IsArmed, this.operational.armed, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapNeedsArming, null).PlayAnim("unarmed").Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.StartArmTrapWorkChore)).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.CancelArmTrapWorkChore)).WorkableCompleteTransition(new Func<ReusableTrap.Instance, Workable>(ReusableTrap.GetWorkable), this.operational.armed);
		this.operational.armed.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsArmed)).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).PlayAnim("armed", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapArmed, null).Toggle("Enable/Disable Trap Trigger", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.EnableTrapTrigger), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Toggle("Enable/Disable Lure", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.ActivateLure), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableLure)).EventHandlerTransition(GameHashes.TrapTriggered, this.operational.capture.capturing, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.HasCritter_OnTrapTriggered));
		this.operational.capture.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).ToggleTag(GameTags.Trapped).DefaultState(this.operational.capture.capturing).EventHandlerTransition(GameHashes.OnStorageChange, this.operational.capture.release, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.OnStorageEmptied));
		this.operational.capture.capturing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.SetupCapturingAnimations)).Update(new Action<ReusableTrap.Instance, float>(ReusableTrap.OptionalCapturingAnimationUpdate), UpdateRate.RENDER_EVERY_TICK, false).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetCaptureAnimationName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.capture.idle).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.UnsetCapturingAnimations));
		this.operational.capture.idle.TriggerOnEnter(GameHashes.TrapCaptureCompleted, null).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapHasCritter, (ReusableTrap.Instance smi) => smi.CapturedCritter).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetIdleAnimationName), KAnim.PlayMode.Once);
		this.operational.capture.release.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).QueueAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName), false, null).OnAnimQueueComplete(this.operational.unarmed);
	}

	// Token: 0x0600526C RID: 21100 RVA: 0x001DFE47 File Offset: 0x001DE047
	public static void RefreshLogicOutput(ReusableTrap.Instance smi)
	{
		smi.RefreshLogicOutput();
	}

	// Token: 0x0600526D RID: 21101 RVA: 0x001DFE4F File Offset: 0x001DE04F
	public static void Release(ReusableTrap.Instance smi)
	{
		smi.Release();
	}

	// Token: 0x0600526E RID: 21102 RVA: 0x001DFE57 File Offset: 0x001DE057
	public static void StartArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x0600526F RID: 21103 RVA: 0x001DFE5F File Offset: 0x001DE05F
	public static void CancelArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06005270 RID: 21104 RVA: 0x001DFE67 File Offset: 0x001DE067
	public static string GetIdleAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture_idle";
		}
		return "capture_idle_large";
	}

	// Token: 0x06005271 RID: 21105 RVA: 0x001DFE7C File Offset: 0x001DE07C
	public static string GetCaptureAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture";
		}
		return "capture_large";
	}

	// Token: 0x06005272 RID: 21106 RVA: 0x001DFE91 File Offset: 0x001DE091
	public static string GetReleaseAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.WasLastCritterLarge)
		{
			return "release";
		}
		return "release_large";
	}

	// Token: 0x06005273 RID: 21107 RVA: 0x001DFEA6 File Offset: 0x001DE0A6
	public static bool OnStorageEmptied(ReusableTrap.Instance smi, object obj)
	{
		return !smi.HasCritter;
	}

	// Token: 0x06005274 RID: 21108 RVA: 0x001DFEB1 File Offset: 0x001DE0B1
	public static bool HasCritter_OnTrapTriggered(ReusableTrap.Instance smi, object capturedItem)
	{
		return smi.HasCritter;
	}

	// Token: 0x06005275 RID: 21109 RVA: 0x001DFEB9 File Offset: 0x001DE0B9
	public static bool StorageContainsCritter(ReusableTrap.Instance smi)
	{
		return smi.HasCritter;
	}

	// Token: 0x06005276 RID: 21110 RVA: 0x001DFEC1 File Offset: 0x001DE0C1
	public static bool StorageIsEmpty(ReusableTrap.Instance smi)
	{
		return !smi.HasCritter;
	}

	// Token: 0x06005277 RID: 21111 RVA: 0x001DFECC File Offset: 0x001DE0CC
	public static void EnableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(true);
	}

	// Token: 0x06005278 RID: 21112 RVA: 0x001DFED5 File Offset: 0x001DE0D5
	public static void DisableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(false);
	}

	// Token: 0x06005279 RID: 21113 RVA: 0x001DFEDE File Offset: 0x001DE0DE
	public static ArmTrapWorkable GetWorkable(ReusableTrap.Instance smi)
	{
		return smi.GetWorkable();
	}

	// Token: 0x0600527A RID: 21114 RVA: 0x001DFEE6 File Offset: 0x001DE0E6
	public static void ActivateLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(true);
	}

	// Token: 0x0600527B RID: 21115 RVA: 0x001DFEEF File Offset: 0x001DE0EF
	public static void DisableLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(false);
	}

	// Token: 0x0600527C RID: 21116 RVA: 0x001DFEF8 File Offset: 0x001DE0F8
	public static void SetupCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.SetupCapturingAnimations();
	}

	// Token: 0x0600527D RID: 21117 RVA: 0x001DFF00 File Offset: 0x001DE100
	public static void UnsetCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.UnsetCapturingAnimations();
	}

	// Token: 0x0600527E RID: 21118 RVA: 0x001DFF08 File Offset: 0x001DE108
	public static void OptionalCapturingAnimationUpdate(ReusableTrap.Instance smi, float dt)
	{
		if (smi.def.usingSymbolChaseCapturingAnimations && smi.lastCritterCapturedAnimController != null)
		{
			if (smi.lastCritterCapturedAnimController.currentAnim != smi.CAPTURING_CRITTER_ANIMATION_NAME)
			{
				smi.lastCritterCapturedAnimController.Play(smi.CAPTURING_CRITTER_ANIMATION_NAME, KAnim.PlayMode.Once, 1f, 0f);
			}
			bool flag;
			Vector3 position = smi.animController.GetSymbolTransform(smi.CAPTURING_SYMBOL_NAME, out flag).GetColumn(3);
			smi.lastCritterCapturedAnimController.transform.SetPosition(position);
		}
	}

	// Token: 0x0600527F RID: 21119 RVA: 0x001DFFAA File Offset: 0x001DE1AA
	public static void MarkAsArmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(true, smi, false);
		smi.gameObject.AddTag(GameTags.TrapArmed);
	}

	// Token: 0x06005280 RID: 21120 RVA: 0x001DFFD0 File Offset: 0x001DE1D0
	public static void MarkAsUnarmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(false, smi, false);
		smi.gameObject.RemoveTag(GameTags.TrapArmed);
	}

	// Token: 0x040037B1 RID: 14257
	public const string CAPTURE_ANIMATION_NAME = "capture";

	// Token: 0x040037B2 RID: 14258
	public const string CAPTURE_LARGE_ANIMATION_NAME = "capture_large";

	// Token: 0x040037B3 RID: 14259
	public const string CAPTURE_IDLE_ANIMATION_NAME = "capture_idle";

	// Token: 0x040037B4 RID: 14260
	public const string CAPTURE_IDLE_LARGE_ANIMATION_NAME = "capture_idle_large";

	// Token: 0x040037B5 RID: 14261
	public const string CAPTURE_RELEASE_ANIMATION_NAME = "release";

	// Token: 0x040037B6 RID: 14262
	public const string CAPTURE_RELEASE_LARGE_ANIMATION_NAME = "release_large";

	// Token: 0x040037B7 RID: 14263
	public const string UNARMED_ANIMATION_NAME = "unarmed";

	// Token: 0x040037B8 RID: 14264
	public const string ARMED_ANIMATION_NAME = "armed";

	// Token: 0x040037B9 RID: 14265
	public const string ABORT_ARMED_ANIMATION = "abort_armed";

	// Token: 0x040037BA RID: 14266
	public StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.BoolParameter IsArmed;

	// Token: 0x040037BB RID: 14267
	public ReusableTrap.NonOperationalStates noOperational;

	// Token: 0x040037BC RID: 14268
	public ReusableTrap.OperationalStates operational;

	// Token: 0x02001C41 RID: 7233
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x0600ACE5 RID: 44261 RVA: 0x003CE191 File Offset: 0x003CC391
		public bool usingLure
		{
			get
			{
				return this.lures != null && this.lures.Length != 0;
			}
		}

		// Token: 0x04008772 RID: 34674
		public string OUTPUT_LOGIC_PORT_ID;

		// Token: 0x04008773 RID: 34675
		public Tag[] lures;

		// Token: 0x04008774 RID: 34676
		public CellOffset releaseCellOffset = CellOffset.none;

		// Token: 0x04008775 RID: 34677
		public bool usingSymbolChaseCapturingAnimations;

		// Token: 0x04008776 RID: 34678
		public Func<string> getTrappedAnimationNameCallback;
	}

	// Token: 0x02001C42 RID: 7234
	public class CaptureStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04008777 RID: 34679
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State capturing;

		// Token: 0x04008778 RID: 34680
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x04008779 RID: 34681
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State release;
	}

	// Token: 0x02001C43 RID: 7235
	public class OperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x0400877A RID: 34682
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State unarmed;

		// Token: 0x0400877B RID: 34683
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State armed;

		// Token: 0x0400877C RID: 34684
		public ReusableTrap.CaptureStates capture;
	}

	// Token: 0x02001C44 RID: 7236
	public class NonOperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x0400877D RID: 34685
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x0400877E RID: 34686
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State releasing;

		// Token: 0x0400877F RID: 34687
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State disarming;
	}

	// Token: 0x02001C45 RID: 7237
	public new class Instance : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.GameInstance, TrappedStates.ITrapStateAnimationInstructions
	{
		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x0600ACEA RID: 44266 RVA: 0x003CE1D2 File Offset: 0x003CC3D2
		public bool IsCapturingLargeCritter
		{
			get
			{
				return this.HasCritter && this.CapturedCritter.HasTag(GameTags.LargeCreature);
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x0600ACEB RID: 44267 RVA: 0x003CE1EE File Offset: 0x003CC3EE
		public bool HasCritter
		{
			get
			{
				return !this.storage.IsEmpty();
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x0600ACEC RID: 44268 RVA: 0x003CE1FE File Offset: 0x003CC3FE
		public GameObject CapturedCritter
		{
			get
			{
				if (!this.HasCritter)
				{
					return null;
				}
				return this.storage.items[0];
			}
		}

		// Token: 0x0600ACED RID: 44269 RVA: 0x003CE21B File Offset: 0x003CC41B
		public ArmTrapWorkable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x0600ACEE RID: 44270 RVA: 0x003CE224 File Offset: 0x003CC424
		public void RefreshLogicOutput()
		{
			bool flag = base.IsInsideState(base.sm.operational) && this.HasCritter;
			this.logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag ? 1 : 0);
		}

		// Token: 0x0600ACEF RID: 44271 RVA: 0x003CE270 File Offset: 0x003CC470
		public Instance(IStateMachineTarget master, ReusableTrap.Def def) : base(master, def)
		{
		}

		// Token: 0x0600ACF0 RID: 44272 RVA: 0x003CE290 File Offset: 0x003CC490
		public override void StartSM()
		{
			base.StartSM();
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
			}
			ArmTrapWorkable armTrapWorkable = this.workable;
			armTrapWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(armTrapWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		}

		// Token: 0x0600ACF1 RID: 44273 RVA: 0x003CE2E0 File Offset: 0x003CC4E0
		private void OnWorkEvent(Workable workable, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped && workable.GetPercentComplete() < 1f && workable.GetPercentComplete() != 0f && base.IsInsideState(base.sm.operational.unarmed))
			{
				this.animController.Play("unarmed", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x0600ACF2 RID: 44274 RVA: 0x003CE343 File Offset: 0x003CC543
		public void SetTrapTriggerActiveState(bool active)
		{
			this.trapTrigger.enabled = active;
		}

		// Token: 0x0600ACF3 RID: 44275 RVA: 0x003CE354 File Offset: 0x003CC554
		public void SetLureActiveState(bool activate)
		{
			if (base.def.usingLure)
			{
				Lure.Instance smi = base.gameObject.GetSMI<Lure.Instance>();
				if (smi != null)
				{
					smi.SetActiveLures(activate ? base.def.lures : null);
				}
			}
		}

		// Token: 0x0600ACF4 RID: 44276 RVA: 0x003CE394 File Offset: 0x003CC594
		public void SetupCapturingAnimations()
		{
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
				this.lastCritterCapturedAnimController = this.CapturedCritter.GetComponent<KBatchedAnimController>();
			}
		}

		// Token: 0x0600ACF5 RID: 44277 RVA: 0x003CE3BC File Offset: 0x003CC5BC
		public void UnsetCapturingAnimations()
		{
			this.trapTrigger.SetStoredPosition(this.CapturedCritter);
			if (base.def.usingSymbolChaseCapturingAnimations && this.lastCritterCapturedAnimController != null)
			{
				this.lastCritterCapturedAnimController.Play("trapped", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.lastCritterCapturedAnimController = null;
		}

		// Token: 0x0600ACF6 RID: 44278 RVA: 0x003CE41C File Offset: 0x003CC61C
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<ArmTrapWorkable>(Db.Get().ChoreTypes.ArmTrap, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x0600ACF7 RID: 44279 RVA: 0x003CE462 File Offset: 0x003CC662
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("GroundTrap.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x0600ACF8 RID: 44280 RVA: 0x003CE484 File Offset: 0x003CC684
		public void Release()
		{
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
				Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.smi.transform.GetPosition()), base.def.releaseCellOffset), Grid.SceneLayer.Creatures);
				List<GameObject> list = new List<GameObject>();
				Storage storage = this.storage;
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list;
				storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				foreach (GameObject gameObject in list)
				{
					gameObject.transform.SetPosition(position);
					KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						component.SetSceneLayer(Grid.SceneLayer.Creatures);
					}
				}
			}
		}

		// Token: 0x0600ACF9 RID: 44281 RVA: 0x003CE558 File Offset: 0x003CC758
		public string GetTrappedAnimationName()
		{
			if (base.def.getTrappedAnimationNameCallback != null)
			{
				return base.def.getTrappedAnimationNameCallback();
			}
			return null;
		}

		// Token: 0x04008780 RID: 34688
		public string CAPTURING_CRITTER_ANIMATION_NAME = "caught_loop";

		// Token: 0x04008781 RID: 34689
		public string CAPTURING_SYMBOL_NAME = "creatureSymbol";

		// Token: 0x04008782 RID: 34690
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04008783 RID: 34691
		[MyCmpGet]
		private ArmTrapWorkable workable;

		// Token: 0x04008784 RID: 34692
		[MyCmpGet]
		private TrapTrigger trapTrigger;

		// Token: 0x04008785 RID: 34693
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x04008786 RID: 34694
		[MyCmpGet]
		public LogicPorts logicPorts;

		// Token: 0x04008787 RID: 34695
		public bool WasLastCritterLarge;

		// Token: 0x04008788 RID: 34696
		public KBatchedAnimController lastCritterCapturedAnimController;

		// Token: 0x04008789 RID: 34697
		private Chore chore;
	}
}
