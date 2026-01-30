using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public class MingleChore : Chore<MingleChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001968 RID: 6504 RVA: 0x0008DDD8 File Offset: 0x0008BFD8
	public MingleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Relax, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new MingleChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(MingleChore.HasMingleCell, this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x0008DE7E File Offset: 0x0008C07E
	protected override StatusItem GetStatusItem()
	{
		return Db.Get().DuplicantStatusItems.Mingling;
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x0008DE8F File Offset: 0x0008C08F
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000ECC RID: 3788
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x04000ECD RID: 3789
	private static Chore.Precondition HasMingleCell = new Chore.Precondition
	{
		id = "HasMingleCell",
		description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_MINGLE_CELL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((MingleChore)data).smi.HasMingleCell();
		}
	};

	// Token: 0x020012ED RID: 4845
	public class States : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore>
	{
		// Token: 0x06008A23 RID: 35363 RVA: 0x00357988 File Offset: 0x00355B88
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.mingle;
			base.Target(this.mingler);
			this.root.EventTransition(GameHashes.ScheduleBlocksChanged, null, (MingleChore.StatesInstance smi) => !smi.IsRecTime());
			this.mingle.Transition(this.walk, (MingleChore.StatesInstance smi) => smi.IsSameRoom(), UpdateRate.SIM_200ms).Transition(this.move, (MingleChore.StatesInstance smi) => !smi.IsSameRoom(), UpdateRate.SIM_200ms);
			this.move.Transition(null, (MingleChore.StatesInstance smi) => !smi.HasMingleCell(), UpdateRate.SIM_200ms).MoveTo((MingleChore.StatesInstance smi) => smi.GetMingleCell(), this.onfloor, null, false);
			this.walk.Transition(null, (MingleChore.StatesInstance smi) => !smi.HasMingleCell(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_walk_kanim", 0f).MoveTo((MingleChore.StatesInstance smi) => smi.GetMingleCell(), this.onfloor, null, false);
			this.onfloor.ToggleAnims("anim_generic_convo_kanim", 0f).PlayAnim("idle", KAnim.PlayMode.Loop).ScheduleGoTo((MingleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 10), this.success).ToggleTag(GameTags.AlwaysConverse);
			this.success.ReturnSuccess();
		}

		// Token: 0x040069AC RID: 27052
		public StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.TargetParameter mingler;

		// Token: 0x040069AD RID: 27053
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State mingle;

		// Token: 0x040069AE RID: 27054
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State move;

		// Token: 0x040069AF RID: 27055
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State walk;

		// Token: 0x040069B0 RID: 27056
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State onfloor;

		// Token: 0x040069B1 RID: 27057
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State success;
	}

	// Token: 0x020012EE RID: 4846
	public class StatesInstance : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.GameInstance
	{
		// Token: 0x06008A25 RID: 35365 RVA: 0x00357B77 File Offset: 0x00355D77
		public StatesInstance(MingleChore master, GameObject mingler) : base(master)
		{
			this.mingler = mingler;
			base.sm.mingler.Set(mingler, base.smi, false);
			this.mingleCellSensor = base.GetComponent<Sensors>().GetSensor<MingleCellSensor>();
		}

		// Token: 0x06008A26 RID: 35366 RVA: 0x00357BB1 File Offset: 0x00355DB1
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06008A27 RID: 35367 RVA: 0x00357BD2 File Offset: 0x00355DD2
		public int GetMingleCell()
		{
			return this.mingleCellSensor.GetCell();
		}

		// Token: 0x06008A28 RID: 35368 RVA: 0x00357BDF File Offset: 0x00355DDF
		public bool HasMingleCell()
		{
			return this.mingleCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x06008A29 RID: 35369 RVA: 0x00357BF8 File Offset: 0x00355DF8
		public bool IsSameRoom()
		{
			int cell = Grid.PosToCell(this.mingler);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(this.GetMingleCell());
			return cavityForCell != null && cavityForCell2 != null && cavityForCell.handle == cavityForCell2.handle;
		}

		// Token: 0x040069B2 RID: 27058
		private MingleCellSensor mingleCellSensor;

		// Token: 0x040069B3 RID: 27059
		private GameObject mingler;
	}
}
