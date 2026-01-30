using System;
using STRINGS;

// Token: 0x0200010E RID: 270
public class RanchedStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>
{
	// Token: 0x060004EF RID: 1263 RVA: 0x00027AD8 File Offset: 0x00025CD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.ranch;
		this.root.Exit("AbandonedRanchStation", delegate(RanchedStates.Instance smi)
		{
			if (smi.Monitor.TargetRanchStation != null)
			{
				if (smi.Monitor.TargetRanchStation.IsCritterInQueue(smi.Monitor))
				{
					Debug.LogWarning("Why are we exiting RanchedStates while in the queue?");
					smi.Monitor.TargetRanchStation.Abandon(smi.Monitor);
				}
				smi.Monitor.TargetRanchStation = null;
			}
			smi.sm.ranchTarget.Set(null, smi);
		});
		this.ranch.EnterTransition(this.ranch.Cheer, (RanchedStates.Instance smi) => RanchedStates.IsCrittersTurn(smi)).EventHandler(GameHashes.RanchStationNoLongerAvailable, delegate(RanchedStates.Instance smi)
		{
			smi.GoTo(null);
		}).BehaviourComplete(GameTags.Creatures.WantsToGetRanched, true).Update(delegate(RanchedStates.Instance smi, float deltaSeconds)
		{
			RanchStation.Instance ranchStation = smi.GetRanchStation();
			if (ranchStation.IsNullOrDestroyed())
			{
				smi.StopSM("No more target ranch station.");
				return;
			}
			Option<CavityInfo> option = Option.Maybe<CavityInfo>(Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi)));
			Option<CavityInfo> cavityInfo = ranchStation.GetCavityInfo();
			if (option.IsNone() || cavityInfo.IsNone())
			{
				smi.StopSM("No longer in any cavity.");
				return;
			}
			if (option.Unwrap() != cavityInfo.Unwrap())
			{
				smi.StopSM("Critter is in a different cavity");
				return;
			}
		}, UpdateRate.SIM_200ms, false).EventHandler(GameHashes.RancherReadyAtRanchStation, delegate(RanchedStates.Instance smi)
		{
			smi.UpdateWaitingState();
		}).Exit(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride));
		GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State cheer = this.ranch.Cheer;
		string name = CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		cheer.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter("FaceRancher", delegate(RanchedStates.Instance smi)
		{
			smi.GetComponent<Facing>().Face(smi.GetRanchStation().transform.GetPosition());
		}).PlayAnim("excited_loop").OnAnimQueueComplete(this.ranch.Cheer.Pst).ScheduleGoTo((RanchedStates.Instance smi) => smi.cheerAnimLength, this.ranch.Move);
		this.ranch.Cheer.Pst.ScheduleGoTo(0.2f, this.ranch.Move);
		GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state = this.ranch.Move.DefaultState(this.ranch.Move.MoveToRanch).Enter("Speedup", delegate(RanchedStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.OriginalSpeed * 1.25f;
		});
		string name2 = CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).Exit("RestoreSpeed", delegate(RanchedStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.OriginalSpeed;
		});
		this.ranch.Move.MoveToRanch.EnterTransition(this.ranch.Wait.WaitInLine, GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Not(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback(RanchedStates.IsCrittersTurn))).MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetRanchNavTarget), this.ranch.Wait.WaitInLine, null, false).Target(this.ranchTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, this.ranch.Wait.WaitInLine, (RanchedStates.Instance smi) => !RanchedStates.IsCrittersTurn(smi));
		this.ranch.Wait.WaitInLine.EnterTransition(this.ranch.Ranching, new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback(RanchedStates.IsCrittersTurn)).Enter(delegate(RanchedStates.Instance smi)
		{
			smi.EnterQueue();
		}).EventTransition(GameHashes.DestinationReached, this.ranch.Wait.Waiting, null);
		this.ranch.Wait.Waiting.Face(this.ranchTarget, 0f).PlayAnim((RanchedStates.Instance smi) => smi.def.StartWaitingAnim, KAnim.PlayMode.Once).QueueAnim((RanchedStates.Instance smi) => smi.def.WaitingAnim, true, null);
		this.ranch.Wait.DoneWaiting.PlayAnim((RanchedStates.Instance smi) => smi.def.EndWaitingAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.ranch.Move.MoveToRanch);
		this.ranch.Ranching.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.GetOnTable)).Enter("SetCreatureAtRanchingStation", delegate(RanchedStates.Instance smi)
		{
			smi.GetRanchStation().MessageCreatureArrived(smi);
			smi.AnimController.SetSceneLayer(Grid.SceneLayer.BuildingUse);
		}).EventTransition(GameHashes.RanchingComplete, this.ranch.Wavegoodbye, null).ToggleMainStatusItem(delegate(RanchedStates.Instance smi)
		{
			RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
			if (ranchStation != null)
			{
				return ranchStation.def.CreatureRanchingStatusItem;
			}
			return Db.Get().CreatureStatusItems.GettingRanched;
		}, null);
		GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state2 = this.ranch.Wavegoodbye.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride)).OnAnimQueueComplete(this.ranch.Runaway);
		string name3 = CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME;
		string tooltip3 = CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP;
		string icon3 = "";
		StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
		NotificationType notification_type3 = NotificationType.Neutral;
		bool allow_multiples3 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main);
		GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state3 = this.ranch.Runaway.MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetRunawayCell), null, null, false);
		string name4 = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip4 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon4 = "";
		StatusItem.IconType icon_type4 = StatusItem.IconType.Info;
		NotificationType notification_type4 = NotificationType.Neutral;
		bool allow_multiples4 = false;
		main = Db.Get().StatusItemCategories.Main;
		state3.ToggleStatusItem(name4, tooltip4, icon4, icon_type4, notification_type4, allow_multiples4, default(HashedString), 129022, null, null, main);
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0002809A File Offset: 0x0002629A
	private static void ClearLayerOverride(RanchedStates.Instance smi)
	{
		smi.AnimController.SetSceneLayer(Grid.SceneLayer.Creatures);
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x000280A9 File Offset: 0x000262A9
	private static RanchStation.Instance GetRanchStation(RanchedStates.Instance smi)
	{
		return smi.GetRanchStation();
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x000280B4 File Offset: 0x000262B4
	private static void GetOnTable(RanchedStates.Instance smi)
	{
		Navigator navigator = smi.Get<Navigator>();
		if (navigator.IsValidNavType(NavType.Floor))
		{
			navigator.SetCurrentNavType(NavType.Floor);
		}
		smi.Get<Facing>().SetFacing(false);
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x000280E4 File Offset: 0x000262E4
	private static bool IsCrittersTurn(RanchedStates.Instance smi)
	{
		RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
		return ranchStation != null && ranchStation.IsRancherReady && ranchStation.TryGetRanched(smi);
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00028110 File Offset: 0x00026310
	private static int GetRanchNavTarget(RanchedStates.Instance smi)
	{
		RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
		int num = smi.ModifyNavTargetForCritter(ranchStation.GetRanchNavTarget());
		if (smi.HasTag(GameTags.LargeCreature))
		{
			ref Vector2I ptr = Grid.PosToXY(smi.gameObject.transform.position);
			Vector2I vector2I = Grid.CellToXY(num);
			if (ptr.x > vector2I.x)
			{
				num = Grid.CellLeft(num);
			}
		}
		return num;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00028170 File Offset: 0x00026370
	private static int GetRunawayCell(RanchedStates.Instance smi)
	{
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		int num = Grid.OffsetCell(cell, 2, 0);
		if (Grid.Solid[num])
		{
			num = Grid.OffsetCell(cell, -2, 0);
		}
		return num;
	}

	// Token: 0x0400039B RID: 923
	private RanchedStates.RanchStates ranch;

	// Token: 0x0400039C RID: 924
	private StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.TargetParameter ranchTarget;

	// Token: 0x02001185 RID: 4485
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040064DA RID: 25818
		public string StartWaitingAnim = "queue_pre";

		// Token: 0x040064DB RID: 25819
		public string WaitingAnim = "queue_loop";

		// Token: 0x040064DC RID: 25820
		public string EndWaitingAnim = "queue_pst";

		// Token: 0x040064DD RID: 25821
		public int WaitCellOffset = 1;
	}

	// Token: 0x02001186 RID: 4486
	public new class Instance : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.GameInstance
	{
		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060084BC RID: 33980 RVA: 0x00345A74 File Offset: 0x00343C74
		public RanchableMonitor.Instance Monitor
		{
			get
			{
				if (this.ranchMonitor == null)
				{
					this.ranchMonitor = this.GetSMI<RanchableMonitor.Instance>();
				}
				return this.ranchMonitor;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060084BD RID: 33981 RVA: 0x00345A90 File Offset: 0x00343C90
		public KBatchedAnimController AnimController
		{
			get
			{
				return this.animController;
			}
		}

		// Token: 0x060084BE RID: 33982 RVA: 0x00345A98 File Offset: 0x00343C98
		public Instance(Chore<RanchedStates.Instance> chore, RanchedStates.Def def) : base(chore, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			this.OriginalSpeed = this.Monitor.NavComponent.defaultSpeed;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToGetRanched);
			KAnim.Anim anim = this.animController.GetAnim(new HashedString("excited_loop"));
			this.cheerAnimLength = ((anim != null) ? (anim.totalTime + 0.2f) : 1.2f);
		}

		// Token: 0x060084BF RID: 33983 RVA: 0x00345B1B File Offset: 0x00343D1B
		public RanchStation.Instance GetRanchStation()
		{
			if (this.Monitor != null)
			{
				return this.Monitor.TargetRanchStation;
			}
			return null;
		}

		// Token: 0x060084C0 RID: 33984 RVA: 0x00345B32 File Offset: 0x00343D32
		public void EnterQueue()
		{
			if (this.GetRanchStation() != null)
			{
				this.InitializeWaitCell();
				this.Monitor.NavComponent.GoTo(this.waitCell, null);
			}
		}

		// Token: 0x060084C1 RID: 33985 RVA: 0x00345B5A File Offset: 0x00343D5A
		public void AbandonRanchStation()
		{
			if (this.Monitor.TargetRanchStation == null || this.status == StateMachine.Status.Failed)
			{
				return;
			}
			this.StopSM("Abandoned Ranch");
		}

		// Token: 0x060084C2 RID: 33986 RVA: 0x00345B80 File Offset: 0x00343D80
		public void SetRanchStation(RanchStation.Instance ranch_station)
		{
			if (this.Monitor.TargetRanchStation != null && this.Monitor.TargetRanchStation != ranch_station)
			{
				this.Monitor.TargetRanchStation.Abandon(base.smi.Monitor);
			}
			base.smi.sm.ranchTarget.Set(ranch_station.gameObject, base.smi, false);
			this.Monitor.TargetRanchStation = ranch_station;
		}

		// Token: 0x060084C3 RID: 33987 RVA: 0x00345BF2 File Offset: 0x00343DF2
		public int ModifyNavTargetForCritter(int navCell)
		{
			if (base.smi.HasTag(GameTags.Creatures.Flyer))
			{
				return Grid.CellAbove(navCell);
			}
			return navCell;
		}

		// Token: 0x060084C4 RID: 33988 RVA: 0x00345C10 File Offset: 0x00343E10
		private void InitializeWaitCell()
		{
			if (this.GetRanchStation() == null)
			{
				return;
			}
			int cell = 0;
			Extents stationExtents = this.Monitor.TargetRanchStation.StationExtents;
			int cell2 = this.ModifyNavTargetForCritter(Grid.XYToCell(stationExtents.x, stationExtents.y));
			int num = 0;
			int num2;
			if (Grid.Raycast(cell2, new Vector2I(-1, 0), out num2, base.def.WaitCellOffset, ~(Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable)))
			{
				num = 1 + base.def.WaitCellOffset - num2;
				cell = this.ModifyNavTargetForCritter(Grid.XYToCell(stationExtents.x + 1, stationExtents.y));
			}
			int num3 = 0;
			int num4;
			if (num != 0 && Grid.Raycast(cell, new Vector2I(1, 0), out num4, base.def.WaitCellOffset, ~(Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable)))
			{
				num3 = base.def.WaitCellOffset - num4;
			}
			int x = (base.def.WaitCellOffset - num) * -1;
			if (num == base.def.WaitCellOffset)
			{
				x = 1 + base.def.WaitCellOffset - num3;
			}
			CellOffset offset = new CellOffset(x, 0);
			this.waitCell = Grid.OffsetCell(cell2, offset);
		}

		// Token: 0x060084C5 RID: 33989 RVA: 0x00345D20 File Offset: 0x00343F20
		public void UpdateWaitingState()
		{
			if (!RanchedStates.IsCrittersTurn(base.smi))
			{
				base.smi.GoTo(base.smi.sm.ranch.Wait.WaitInLine);
				return;
			}
			if (base.smi.IsInsideState(base.sm.ranch.Wait.Waiting))
			{
				base.smi.GoTo(base.smi.sm.ranch.Wait.DoneWaiting);
				return;
			}
			base.smi.GoTo(base.smi.sm.ranch.Cheer);
		}

		// Token: 0x040064DE RID: 25822
		public float OriginalSpeed;

		// Token: 0x040064DF RID: 25823
		private int waitCell;

		// Token: 0x040064E0 RID: 25824
		private KBatchedAnimController animController;

		// Token: 0x040064E1 RID: 25825
		private RanchableMonitor.Instance ranchMonitor;

		// Token: 0x040064E2 RID: 25826
		public float cheerAnimLength;
	}

	// Token: 0x02001187 RID: 4487
	public class RanchStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040064E3 RID: 25827
		public RanchedStates.CheerStates Cheer;

		// Token: 0x040064E4 RID: 25828
		public RanchedStates.MoveStates Move;

		// Token: 0x040064E5 RID: 25829
		public RanchedStates.WaitStates Wait;

		// Token: 0x040064E6 RID: 25830
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Ranching;

		// Token: 0x040064E7 RID: 25831
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Wavegoodbye;

		// Token: 0x040064E8 RID: 25832
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Runaway;
	}

	// Token: 0x02001188 RID: 4488
	public class CheerStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040064E9 RID: 25833
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Cheer;

		// Token: 0x040064EA RID: 25834
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Pst;
	}

	// Token: 0x02001189 RID: 4489
	public class MoveStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040064EB RID: 25835
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State MoveToRanch;
	}

	// Token: 0x0200118A RID: 4490
	public class WaitStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040064EC RID: 25836
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State WaitInLine;

		// Token: 0x040064ED RID: 25837
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Waiting;

		// Token: 0x040064EE RID: 25838
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State DoneWaiting;
	}
}
