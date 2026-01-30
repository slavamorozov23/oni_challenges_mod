using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class BuzzStates : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>
{
	// Token: 0x0600040E RID: 1038 RVA: 0x00021ECC File Offset: 0x000200CC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.root.Exit("StopNavigator", new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(BuzzStates.StopNavigator)).ToggleMainStatusItem(IdleStates.IdleStatus, null).ToggleTag(GameTags.Idle);
		this.idle.Enter(new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(BuzzStates.PlayIdle)).ToggleScheduleCallback("DoBuzz", new Func<BuzzStates.Instance, float>(BuzzStates.GetIdleTime), new Action<BuzzStates.Instance>(BuzzStates.GoBuzz));
		this.buzz.ParamTransition<int>(this.numMoves, this.idle, GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.IsLTEZero_int);
		this.buzz.move.Enter(new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(BuzzStates.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.buzz.pause, null).EventTransition(GameHashes.NavigationFailed, this.buzz.pause, null);
		this.buzz.pause.Enter(new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(BuzzStates.BuzzPause));
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00021FD0 File Offset: 0x000201D0
	private static float GetIdleTime(BuzzStates.Instance smi)
	{
		return (float)UnityEngine.Random.Range(3, 10);
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00021FDB File Offset: 0x000201DB
	private static void GoBuzz(BuzzStates.Instance smi)
	{
		smi.sm.numMoves.Set(UnityEngine.Random.Range(4, 6), smi, false);
		smi.GoTo(smi.sm.buzz.move);
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0002200D File Offset: 0x0002020D
	private static void BuzzPause(BuzzStates.Instance smi)
	{
		smi.sm.numMoves.Set(smi.sm.numMoves.Get(smi) - 1, smi, false);
		smi.GoTo(smi.sm.buzz.move);
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0002204B File Offset: 0x0002024B
	private static void StopNavigator(BuzzStates.Instance smi)
	{
		smi.navigator.Stop(false, true);
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0002205C File Offset: 0x0002025C
	private static void MoveToNewCell(BuzzStates.Instance smi)
	{
		BuzzStates.MoveCellQuery.Instance.Reset(smi.navigator.CurrentNavType, smi.kpid.HasTag(GameTags.Amphibious));
		smi.navigator.RunQuery(BuzzStates.MoveCellQuery.Instance);
		smi.navigator.GoTo(BuzzStates.MoveCellQuery.Instance.GetResultCell(), null);
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x000220B8 File Offset: 0x000202B8
	private static void PlayIdle(BuzzStates.Instance smi)
	{
		NavType nav_type = smi.navigator.CurrentNavType;
		if (smi.facing.GetFacing())
		{
			nav_type = NavGrid.MirrorNavType(nav_type);
		}
		if (smi.def.customIdleAnim != null)
		{
			HashedString invalid = HashedString.Invalid;
			HashedString hashedString = smi.def.customIdleAnim(smi, ref invalid);
			if (hashedString != HashedString.Invalid)
			{
				if (invalid != HashedString.Invalid)
				{
					smi.kac.Play(invalid, KAnim.PlayMode.Once, 1f, 0f);
				}
				smi.kac.Queue(hashedString, KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		HashedString idleAnim = smi.navigator.NavGrid.GetIdleAnim(nav_type);
		smi.kac.Play(idleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0400030D RID: 781
	private StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.IntParameter numMoves;

	// Token: 0x0400030E RID: 782
	private BuzzStates.BuzzingStates buzz;

	// Token: 0x0400030F RID: 783
	public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State idle;

	// Token: 0x020010F5 RID: 4341
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040063A3 RID: 25507
		public BuzzStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x02002772 RID: 10098
		// (Invoke) Token: 0x0600C8D6 RID: 51414
		public delegate HashedString IdleAnimCallback(BuzzStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x020010F6 RID: 4342
	public new class Instance : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.GameInstance
	{
		// Token: 0x0600835D RID: 33629 RVA: 0x003434BB File Offset: 0x003416BB
		public Instance(Chore<BuzzStates.Instance> chore, BuzzStates.Def def) : base(chore, def)
		{
			this.navigator = base.GetComponent<Navigator>();
			this.kac = base.GetComponent<KBatchedAnimController>();
			this.kpid = base.GetComponent<KPrefabID>();
			this.facing = base.GetComponent<Facing>();
		}

		// Token: 0x040063A4 RID: 25508
		public Navigator navigator;

		// Token: 0x040063A5 RID: 25509
		public KBatchedAnimController kac;

		// Token: 0x040063A6 RID: 25510
		public KPrefabID kpid;

		// Token: 0x040063A7 RID: 25511
		public Facing facing;
	}

	// Token: 0x020010F7 RID: 4343
	public class BuzzingStates : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State
	{
		// Token: 0x040063A8 RID: 25512
		public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State move;

		// Token: 0x040063A9 RID: 25513
		public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State pause;
	}

	// Token: 0x020010F8 RID: 4344
	public class MoveCellQuery : PathFinderQuery
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x0600835F RID: 33631 RVA: 0x003434FD File Offset: 0x003416FD
		// (set) Token: 0x06008360 RID: 33632 RVA: 0x00343505 File Offset: 0x00341705
		public bool allowLiquid { get; set; }

		// Token: 0x06008361 RID: 33633 RVA: 0x0034350E File Offset: 0x0034170E
		public MoveCellQuery(NavType navType)
		{
			this.navType = navType;
			this.maxIterations = UnityEngine.Random.Range(5, 25);
		}

		// Token: 0x06008362 RID: 33634 RVA: 0x00343536 File Offset: 0x00341736
		public void Reset(NavType navType, bool allowLiquid)
		{
			this.navType = navType;
			this.maxIterations = UnityEngine.Random.Range(5, 25);
			this.targetCell = Grid.InvalidCell;
			this.allowLiquid = allowLiquid;
		}

		// Token: 0x06008363 RID: 33635 RVA: 0x00343560 File Offset: 0x00341760
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			bool flag = this.navType != NavType.Swim;
			bool flag2 = this.navType == NavType.Swim || this.allowLiquid;
			bool flag3 = Grid.IsSubstantialLiquid(cell, 0.35f);
			if (flag3 && !flag2)
			{
				return false;
			}
			if (!flag3 && !flag)
			{
				return false;
			}
			this.targetCell = cell;
			int num = this.maxIterations - 1;
			this.maxIterations = num;
			return num <= 0;
		}

		// Token: 0x06008364 RID: 33636 RVA: 0x003435D1 File Offset: 0x003417D1
		public override int GetResultCell()
		{
			return this.targetCell;
		}

		// Token: 0x040063AA RID: 25514
		private NavType navType;

		// Token: 0x040063AB RID: 25515
		private int targetCell = Grid.InvalidCell;

		// Token: 0x040063AC RID: 25516
		private int maxIterations;

		// Token: 0x040063AE RID: 25518
		public static BuzzStates.MoveCellQuery Instance = new BuzzStates.MoveCellQuery(NavType.Floor);
	}
}
