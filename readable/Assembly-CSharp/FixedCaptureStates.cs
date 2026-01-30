using System;
using STRINGS;

// Token: 0x020000F6 RID: 246
public class FixedCaptureStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>
{
	// Token: 0x0600048C RID: 1164 RVA: 0x00025428 File Offset: 0x00023628
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.capture;
		this.root.Exit("AbandonedCapturePoint", delegate(FixedCaptureStates.Instance smi)
		{
			smi.AbandonedCapturePoint();
		});
		this.capture.EventTransition(GameHashes.CapturePointNoLongerAvailable, null, null).DefaultState(this.capture.cheer);
		GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State state = this.capture.cheer.DefaultState(this.capture.cheer.pre);
		string name = CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.capture.cheer.pre.ScheduleGoTo(0.9f, this.capture.cheer.cheer);
		this.capture.cheer.cheer.Enter("FaceRancher", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetComponent<Facing>().Face(smi.GetCapturePoint().transform.GetPosition());
		}).PlayAnim("excited_loop").OnAnimQueueComplete(this.capture.cheer.pst);
		this.capture.cheer.pst.ScheduleGoTo(0.2f, this.capture.move);
		GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State state2 = this.capture.move.DefaultState(this.capture.move.movetoranch);
		string name2 = CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
		this.capture.move.movetoranch.Enter("Speedup", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed * 1.25f;
		}).MoveTo(new Func<FixedCaptureStates.Instance, int>(FixedCaptureStates.GetTargetCaptureCell), this.capture.move.waitforranchertobeready, null, false).Exit("RestoreSpeed", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed;
		});
		this.capture.move.waitforranchertobeready.Enter("SetCreatureAtRanchingStation", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetCapturePoint().Trigger(-1992722293, null);
		}).EventTransition(GameHashes.RancherReadyAtCapturePoint, this.capture.ranching, null);
		GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State ranching = this.capture.ranching;
		string name3 = CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
		string tooltip3 = CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
		string icon3 = "";
		StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
		NotificationType notification_type3 = NotificationType.Neutral;
		bool allow_multiples3 = false;
		main = Db.Get().StatusItemCategories.Main;
		ranching.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToGetCaptured, false);
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00025736 File Offset: 0x00023936
	private static FixedCapturePoint.Instance GetCapturePoint(FixedCaptureStates.Instance smi)
	{
		return smi.GetSMI<FixedCapturableMonitor.Instance>().targetCapturePoint;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00025744 File Offset: 0x00023944
	private static int GetTargetCaptureCell(FixedCaptureStates.Instance smi)
	{
		FixedCapturePoint.Instance capturePoint = FixedCaptureStates.GetCapturePoint(smi);
		return capturePoint.def.getTargetCapturePoint(capturePoint);
	}

	// Token: 0x04000360 RID: 864
	private FixedCaptureStates.CaptureStates capture;

	// Token: 0x04000361 RID: 865
	private GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State behaviourcomplete;

	// Token: 0x02001141 RID: 4417
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001142 RID: 4418
	public new class Instance : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.GameInstance
	{
		// Token: 0x0600841C RID: 33820 RVA: 0x003446A8 File Offset: 0x003428A8
		public Instance(Chore<FixedCaptureStates.Instance> chore, FixedCaptureStates.Def def) : base(chore, def)
		{
			this.originalSpeed = base.GetComponent<Navigator>().defaultSpeed;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToGetCaptured);
		}

		// Token: 0x0600841D RID: 33821 RVA: 0x003446E0 File Offset: 0x003428E0
		public FixedCapturePoint.Instance GetCapturePoint()
		{
			FixedCapturableMonitor.Instance smi = this.GetSMI<FixedCapturableMonitor.Instance>();
			if (smi == null)
			{
				return null;
			}
			return smi.targetCapturePoint;
		}

		// Token: 0x0600841E RID: 33822 RVA: 0x003446FF File Offset: 0x003428FF
		public void AbandonedCapturePoint()
		{
			if (this.GetCapturePoint() != null)
			{
				this.GetCapturePoint().Trigger(-1000356449, null);
			}
		}

		// Token: 0x04006450 RID: 25680
		public float originalSpeed;
	}

	// Token: 0x02001143 RID: 4419
	public class CaptureStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
	{
		// Token: 0x04006451 RID: 25681
		public FixedCaptureStates.CaptureStates.CheerStates cheer;

		// Token: 0x04006452 RID: 25682
		public FixedCaptureStates.CaptureStates.MoveStates move;

		// Token: 0x04006453 RID: 25683
		public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State ranching;

		// Token: 0x02002775 RID: 10101
		public class CheerStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
		{
			// Token: 0x0400AF41 RID: 44865
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pre;

			// Token: 0x0400AF42 RID: 44866
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State cheer;

			// Token: 0x0400AF43 RID: 44867
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pst;
		}

		// Token: 0x02002776 RID: 10102
		public class MoveStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
		{
			// Token: 0x0400AF44 RID: 44868
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State movetoranch;

			// Token: 0x0400AF45 RID: 44869
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State waitforranchertobeready;
		}
	}
}
