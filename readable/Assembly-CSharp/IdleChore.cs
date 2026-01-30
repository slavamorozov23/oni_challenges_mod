using System;
using UnityEngine;

// Token: 0x020004AB RID: 1195
public class IdleChore : Chore<IdleChore.StatesInstance>
{
	// Token: 0x06001967 RID: 6503 RVA: 0x0008DD88 File Offset: 0x0008BF88
	public IdleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Idle, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.IdleTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new IdleChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x020012EB RID: 4843
	public class StatesInstance : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.GameInstance
	{
		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06008A14 RID: 35348 RVA: 0x00357461 File Offset: 0x00355661
		// (set) Token: 0x06008A15 RID: 35349 RVA: 0x00357469 File Offset: 0x00355669
		public Navigator navigator { get; private set; }

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06008A16 RID: 35350 RVA: 0x00357472 File Offset: 0x00355672
		// (set) Token: 0x06008A17 RID: 35351 RVA: 0x0035747A File Offset: 0x0035567A
		public KBatchedAnimController animController { get; private set; }

		// Token: 0x06008A18 RID: 35352 RVA: 0x00357484 File Offset: 0x00355684
		public StatesInstance(IdleChore master, GameObject idler) : base(master)
		{
			base.sm.idler.Set(idler, base.smi, false);
			this.navigator = base.GetComponent<Navigator>();
			this.animController = base.GetComponent<KBatchedAnimController>();
			this.idleCellSensor = base.GetComponent<Sensors>().GetSensor<IdleCellSensor>();
		}

		// Token: 0x06008A19 RID: 35353 RVA: 0x003574DA File Offset: 0x003556DA
		public int GetIdleCell()
		{
			return this.idleCellSensor.GetCell();
		}

		// Token: 0x06008A1A RID: 35354 RVA: 0x003574E7 File Offset: 0x003556E7
		public bool HasIdleCell()
		{
			return this.idleCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x040069A3 RID: 27043
		private IdleCellSensor idleCellSensor;
	}

	// Token: 0x020012EC RID: 4844
	public class States : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore>
	{
		// Token: 0x06008A1B RID: 35355 RVA: 0x00357500 File Offset: 0x00355700
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.Target(this.idler);
			this.idle.DefaultState(this.idle.onfloor).Enter("UpdateNavType", delegate(IdleChore.StatesInstance smi)
			{
				IdleChore.States.UpdateNavType(smi);
			}).Update("UpdateNavType", delegate(IdleChore.StatesInstance smi, float dt)
			{
				IdleChore.States.UpdateNavType(smi);
			}, UpdateRate.SIM_200ms, false).ToggleStateMachine((IdleChore.StatesInstance smi) => new TaskAvailabilityMonitor.Instance(smi.master)).ToggleTag(GameTags.Idle);
			this.idle.onfloor.PlayAnim("idle_default", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isOnLadder, this.idle.onladder, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>(this.isOnTube, this.idle.ontube, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>(this.isOnSuitMarkerCell, this.idle.onsuitmarker, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>(this.isHovering, this.idle.hovering, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.onladder.PlayAnim("ladder_idle", KAnim.PlayMode.Loop).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.ontube.PlayAnim("tube_idle_loop", KAnim.PlayMode.Loop).Update("IdleMove", delegate(IdleChore.StatesInstance smi, float dt)
			{
				if (smi.HasIdleCell())
				{
					smi.GoTo(this.idle.move);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.idle.hovering.PlayAnim("hover_idle", KAnim.PlayMode.Loop).Update("IdleMove", delegate(IdleChore.StatesInstance smi, float dt)
			{
				if (smi.HasIdleCell())
				{
					smi.GoTo(this.idle.move);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.idle.onsuitmarker.PlayAnim("idle_default", KAnim.PlayMode.Loop).Enter(delegate(IdleChore.StatesInstance smi)
			{
				int cell = Grid.PosToCell(smi);
				Grid.SuitMarker.Flags flags;
				PathFinder.PotentialPath.Flags flags2;
				Grid.TryGetSuitMarkerFlags(cell, out flags, out flags2);
				IdleSuitMarkerCellQuery idleSuitMarkerCellQuery = new IdleSuitMarkerCellQuery((flags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0, Grid.CellToXY(cell).X);
				smi.navigator.RunQuery(idleSuitMarkerCellQuery);
				smi.navigator.GoTo(idleSuitMarkerCellQuery.GetResultCell(), null);
			}).EventTransition(GameHashes.DestinationReached, this.idle, null).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.move.Transition(this.idle, (IdleChore.StatesInstance smi) => !smi.HasIdleCell(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_walk_kanim", 0f).MoveTo((IdleChore.StatesInstance smi) => smi.GetIdleCell(), this.idle, this.idle, false).Exit("UpdateNavType", delegate(IdleChore.StatesInstance smi)
			{
				IdleChore.States.UpdateNavType(smi);
			}).Exit("ClearWalk", delegate(IdleChore.StatesInstance smi)
			{
				smi.animController.Play("idle_default", KAnim.PlayMode.Once, 1f, 0f);
			});
		}

		// Token: 0x06008A1C RID: 35356 RVA: 0x0035787C File Offset: 0x00355A7C
		public static void UpdateNavType(IdleChore.StatesInstance smi)
		{
			NavType currentNavType = smi.navigator.CurrentNavType;
			smi.sm.isOnLadder.Set(currentNavType == NavType.Ladder || currentNavType == NavType.Pole, smi, false);
			smi.sm.isOnTube.Set(currentNavType == NavType.Tube, smi, false);
			smi.sm.isHovering.Set(currentNavType == NavType.Hover, smi, false);
			int num = Grid.PosToCell(smi);
			smi.sm.isOnSuitMarkerCell.Set(Grid.IsValidCell(num) && Grid.HasSuitMarker[num], smi, false);
		}

		// Token: 0x040069A6 RID: 27046
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnLadder;

		// Token: 0x040069A7 RID: 27047
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnTube;

		// Token: 0x040069A8 RID: 27048
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnSuitMarkerCell;

		// Token: 0x040069A9 RID: 27049
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isHovering;

		// Token: 0x040069AA RID: 27050
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.TargetParameter idler;

		// Token: 0x040069AB RID: 27051
		public IdleChore.States.IdleState idle;

		// Token: 0x020027BE RID: 10174
		public class IdleState : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State
		{
			// Token: 0x0400B032 RID: 45106
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onfloor;

			// Token: 0x0400B033 RID: 45107
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onladder;

			// Token: 0x0400B034 RID: 45108
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State ontube;

			// Token: 0x0400B035 RID: 45109
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onsuitmarker;

			// Token: 0x0400B036 RID: 45110
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State hovering;

			// Token: 0x0400B037 RID: 45111
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State move;
		}
	}
}
