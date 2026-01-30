using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class IdleStates : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>
{
	// Token: 0x060004C6 RID: 1222 RVA: 0x00026B6C File Offset: 0x00024D6C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.root.Exit("StopNavigator", new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(IdleStates.StopNavigator)).ToggleMainStatusItem(IdleStates.IdleStatus, null).ToggleTag(GameTags.Idle);
		this.loop.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(IdleStates.PlayIdle)).ToggleScheduleCallback("IdleMove", new Func<IdleStates.Instance, float>(IdleStates.GetIdleTime), new Action<IdleStates.Instance>(IdleStates.GoMove));
		this.move.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(IdleStates.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.loop, null).EventTransition(GameHashes.NavigationFailed, this.loop, null);
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00026C27 File Offset: 0x00024E27
	private static float GetIdleTime(IdleStates.Instance smi)
	{
		return (float)UnityEngine.Random.Range(3, 10);
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00026C32 File Offset: 0x00024E32
	private static void GoMove(IdleStates.Instance smi)
	{
		smi.GoTo(smi.sm.move);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00026C45 File Offset: 0x00024E45
	private static void StopNavigator(IdleStates.Instance smi)
	{
		smi.navigator.Stop(false, true);
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00026C58 File Offset: 0x00024E58
	private static void MoveToNewCell(IdleStates.Instance smi)
	{
		if (smi.kpid.HasTag(GameTags.StationaryIdling))
		{
			smi.GoTo(smi.sm.loop);
			return;
		}
		IdleStates.MoveCellQuery instance = IdleStates.MoveCellQuery.Instance;
		instance.Reset(smi.navigator.CurrentNavType);
		instance.allowLiquid = smi.kpid.HasTag(GameTags.Amphibious);
		instance.submerged = smi.kpid.HasTag(GameTags.Creatures.Submerged);
		int num = Grid.PosToCell(smi.navigator);
		if (smi.navigator.CurrentNavType == NavType.Hover && CellSelectionObject.IsExposedToSpace(num))
		{
			int num2 = 0;
			int cell = num;
			for (int i = 0; i < 10; i++)
			{
				cell = Grid.CellBelow(cell);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || !CellSelectionObject.IsExposedToSpace(cell))
				{
					break;
				}
				num2++;
			}
			instance.lowerCellBias = (num2 == 10);
		}
		smi.navigator.RunQuery(instance);
		if (smi.navigator.CanReach(instance.GetResultCell()))
		{
			smi.navigator.GoTo(instance.GetResultCell(), null);
			return;
		}
		smi.GoTo(smi.sm.loop);
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00026D78 File Offset: 0x00024F78
	private static void PlayIdle(IdleStates.Instance smi)
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

	// Token: 0x0400037E RID: 894
	private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State loop;

	// Token: 0x0400037F RID: 895
	private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State move;

	// Token: 0x04000380 RID: 896
	public static StatusItem IdleStatus = new StatusItem("IdleStatus", CREATURES.STATUSITEMS.IDLE.NAME, CREATURES.STATUSITEMS.IDLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Messages, false, OverlayModes.None.ID, 129022, true, null);

	// Token: 0x02001169 RID: 4457
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400649A RID: 25754
		public IdleStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x0400649B RID: 25755
		public PriorityScreen.PriorityClass priorityClass;

		// Token: 0x02002778 RID: 10104
		// (Invoke) Token: 0x0600C8E7 RID: 51431
		public delegate HashedString IdleAnimCallback(IdleStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x0200116A RID: 4458
	public new class Instance : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.GameInstance
	{
		// Token: 0x06008477 RID: 33911 RVA: 0x00345050 File Offset: 0x00343250
		public Instance(Chore<IdleStates.Instance> chore, IdleStates.Def def) : base(chore, def)
		{
			this.navigator = base.GetComponent<Navigator>();
			this.kpid = base.GetComponent<KPrefabID>();
			this.kac = base.GetComponent<KBatchedAnimController>();
			this.facing = base.GetComponent<Facing>();
			chore.masterPriority.priority_class = def.priorityClass;
		}

		// Token: 0x0400649C RID: 25756
		public Navigator navigator;

		// Token: 0x0400649D RID: 25757
		public KPrefabID kpid;

		// Token: 0x0400649E RID: 25758
		public KBatchedAnimController kac;

		// Token: 0x0400649F RID: 25759
		public Facing facing;
	}

	// Token: 0x0200116B RID: 4459
	public class MoveCellQuery : PathFinderQuery
	{
		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06008478 RID: 33912 RVA: 0x003450A6 File Offset: 0x003432A6
		// (set) Token: 0x06008479 RID: 33913 RVA: 0x003450AE File Offset: 0x003432AE
		public bool allowLiquid { get; set; }

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x0600847A RID: 33914 RVA: 0x003450B7 File Offset: 0x003432B7
		// (set) Token: 0x0600847B RID: 33915 RVA: 0x003450BF File Offset: 0x003432BF
		public bool submerged { get; set; }

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x0600847C RID: 33916 RVA: 0x003450C8 File Offset: 0x003432C8
		// (set) Token: 0x0600847D RID: 33917 RVA: 0x003450D0 File Offset: 0x003432D0
		public bool lowerCellBias { get; set; }

		// Token: 0x0600847E RID: 33918 RVA: 0x003450D9 File Offset: 0x003432D9
		public MoveCellQuery(NavType navType)
		{
			this.Reset(navType);
		}

		// Token: 0x0600847F RID: 33919 RVA: 0x003450F3 File Offset: 0x003432F3
		public void Reset(NavType navType)
		{
			this.navType = navType;
			this.maxIterations = UnityEngine.Random.Range(5, 25);
			this.targetCell = Grid.InvalidCell;
			this.allowLiquid = false;
			this.submerged = false;
			this.lowerCellBias = false;
		}

		// Token: 0x06008480 RID: 33920 RVA: 0x0034512C File Offset: 0x0034332C
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			if (Grid.ObjectLayers[9].ContainsKey(cell))
			{
				return false;
			}
			bool flag = this.submerged || Grid.IsNavigatableLiquid(cell);
			bool flag2 = this.navType != NavType.Swim;
			bool flag3 = this.navType == NavType.Swim || this.allowLiquid;
			if (flag && !flag3)
			{
				return false;
			}
			if (!flag && !flag2)
			{
				return false;
			}
			if (this.targetCell == Grid.InvalidCell || !this.lowerCellBias)
			{
				this.targetCell = cell;
			}
			else
			{
				int num = Grid.CellRow(this.targetCell);
				if (Grid.CellRow(cell) < num)
				{
					this.targetCell = cell;
				}
			}
			int num2 = this.maxIterations - 1;
			this.maxIterations = num2;
			return num2 <= 0;
		}

		// Token: 0x06008481 RID: 33921 RVA: 0x003451EB File Offset: 0x003433EB
		public override int GetResultCell()
		{
			return this.targetCell;
		}

		// Token: 0x040064A0 RID: 25760
		private NavType navType;

		// Token: 0x040064A1 RID: 25761
		private int targetCell = Grid.InvalidCell;

		// Token: 0x040064A2 RID: 25762
		private int maxIterations;

		// Token: 0x040064A6 RID: 25766
		public static IdleStates.MoveCellQuery Instance = new IdleStates.MoveCellQuery(NavType.Floor);
	}
}
