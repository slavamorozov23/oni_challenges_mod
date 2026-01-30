using System;
using STRINGS;
using UnityEngine;

// Token: 0x020005CE RID: 1486
public class StompStates : GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>
{
	// Token: 0x06002214 RID: 8724 RVA: 0x000C5EAC File Offset: 0x000C40AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Never;
		default_state = this.approach;
		this.root.Enter(new StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State.Callback(StompStates.RefreshTarget));
		this.approach.InitializeStates(this.stomper, this.target, (StompStates.Instance smi) => smi.TargetOffsets, this.stomp, this.failed, null).ToggleMainStatusItem(new Func<StompStates.Instance, StatusItem>(StompStates.GetGoingToStompStatusItem), null).OnTargetLost(this.target, this.failed).Target(this.target).EventTransition(GameHashes.Harvest, this.failed, null).EventTransition(GameHashes.Uprooted, this.failed, null).EventTransition(GameHashes.QueueDestroyObject, this.failed, null);
		this.stomp.DefaultState(this.stomp.pre).ToggleMainStatusItem(new Func<StompStates.Instance, StatusItem>(StompStates.GetStompingStatusItem), null);
		this.stomp.pre.Enter(new StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State.Callback(StompStates.ResetStompLoopTimer)).PlayAnim("stomping_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.stomp.loop);
		this.stomp.loop.ParamTransition<float>(this.stompingLoopTimer, this.stomp.pst, GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.IsLTZero).PlayAnim("stomping_loop", KAnim.PlayMode.Loop).Update(new Action<StompStates.Instance, float>(StompStates.StompUpdate), UpdateRate.SIM_200ms, false);
		this.stomp.pst.PlayAnim("stomping_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
		this.complete.BehaviourComplete(GameTags.Creatures.WantsToStomp, false);
		this.failed.Enter(new StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State.Callback(StompStates.ReportFailure)).EnterGoTo(null);
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x000C607D File Offset: 0x000C427D
	private static StatusItem GetGoingToStompStatusItem(StompStates.Instance smi)
	{
		return StompStates.GetStatusItem(smi, CREATURES.STATUSITEMS.GOING_TO_STOMP.NAME, CREATURES.STATUSITEMS.GOING_TO_STOMP.TOOLTIP);
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x000C6099 File Offset: 0x000C4299
	private static StatusItem GetStompingStatusItem(StompStates.Instance smi)
	{
		return StompStates.GetStatusItem(smi, CREATURES.STATUSITEMS.STOMPING.NAME, CREATURES.STATUSITEMS.STOMPING.TOOLTIP);
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x000C60B8 File Offset: 0x000C42B8
	private static StatusItem GetStatusItem(StompStates.Instance smi, string name, string tooltip)
	{
		return new StatusItem(smi.GetCurrentState().longName, name, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, true, null);
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000C60EF File Offset: 0x000C42EF
	private static void ResetStompLoopTimer(StompStates.Instance smi)
	{
		smi.sm.stompingLoopTimer.Set(0f, smi, false);
	}

	// Token: 0x06002219 RID: 8729 RVA: 0x000C610C File Offset: 0x000C430C
	private static void StompUpdate(StompStates.Instance smi, float dt)
	{
		if (smi.StompLoopTimer <= 1.8333334f)
		{
			smi.sm.stompingLoopTimer.Set(smi.StompLoopTimer + dt, smi, false);
			return;
		}
		if (smi.HarvestAnyOneIntersectingPlant())
		{
			StompStates.ResetStompLoopTimer(smi);
			return;
		}
		smi.sm.stompingLoopTimer.Set(-1f, smi, false);
	}

	// Token: 0x0600221A RID: 8730 RVA: 0x000C616C File Offset: 0x000C436C
	private static void RefreshTarget(StompStates.Instance smi)
	{
		StompMonitor.Instance smi2 = smi.GetSMI<StompMonitor.Instance>();
		smi.SetTarget(smi2.Target);
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x000C618C File Offset: 0x000C438C
	private static void ReportFailure(StompStates.Instance smi)
	{
		StompMonitor.Instance smi2 = smi.GetSMI<StompMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.sm.StompStateFailed.Trigger(smi2);
		}
	}

	// Token: 0x040013E1 RID: 5089
	public const string PRE_STOMP_ANIM_NAME = "stomping_pre";

	// Token: 0x040013E2 RID: 5090
	public const string LOOP_STOMP_ANIM_NAME = "stomping_loop";

	// Token: 0x040013E3 RID: 5091
	public const string PST_STOMP_ANIM_NAME = "stomping_pst";

	// Token: 0x040013E4 RID: 5092
	private const int STOMP_LOOP_ANIM_FRAME_COUNT = 55;

	// Token: 0x040013E5 RID: 5093
	private const float STOMP_LOOP_ANIM_DURATION = 1.8333334f;

	// Token: 0x040013E6 RID: 5094
	public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.ApproachSubState<IApproachable> approach;

	// Token: 0x040013E7 RID: 5095
	public StompStates.StompState stomp;

	// Token: 0x040013E8 RID: 5096
	public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State complete;

	// Token: 0x040013E9 RID: 5097
	public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State failed;

	// Token: 0x040013EA RID: 5098
	public StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.FloatParameter stompingLoopTimer;

	// Token: 0x040013EB RID: 5099
	public StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.TargetParameter stomper;

	// Token: 0x040013EC RID: 5100
	public StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.TargetParameter target;

	// Token: 0x0200149E RID: 5278
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200149F RID: 5279
	public class StompState : GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State
	{
		// Token: 0x04006F11 RID: 28433
		public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State pre;

		// Token: 0x04006F12 RID: 28434
		public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State loop;

		// Token: 0x04006F13 RID: 28435
		public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State pst;
	}

	// Token: 0x020014A0 RID: 5280
	public new class Instance : GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.GameInstance
	{
		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x0600906D RID: 36973 RVA: 0x0036E9CC File Offset: 0x0036CBCC
		public float StompLoopTimer
		{
			get
			{
				return base.sm.stompingLoopTimer.Get(this);
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x0600906E RID: 36974 RVA: 0x0036E9DF File Offset: 0x0036CBDF
		public GameObject CurrentTarget
		{
			get
			{
				return base.sm.target.Get(this);
			}
		}

		// Token: 0x0600906F RID: 36975 RVA: 0x0036E9F4 File Offset: 0x0036CBF4
		public Instance(Chore<StompStates.Instance> chore, StompStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToStomp);
			this.occupyArea = base.GetComponent<OccupyArea>();
			base.sm.stomper.Set(base.smi.gameObject, base.smi, false);
		}

		// Token: 0x06009070 RID: 36976 RVA: 0x0036EA54 File Offset: 0x0036CC54
		public void SetTarget(GameObject target)
		{
			base.smi.sm.target.Set(target, base.smi, false);
			if (this.CurrentTarget == null)
			{
				this.TargetOffsets = new CellOffset[]
				{
					new CellOffset(0, 0)
				};
				return;
			}
			ListPool<CellOffset, StompStates.Instance>.PooledList pooledList = ListPool<CellOffset, StompStates.Instance>.Allocate();
			StompMonitor.Def.GetObjectCellsOffsetsWithExtraBottomPadding(this.CurrentTarget, pooledList);
			this.TargetOffsets = pooledList.ToArray();
			pooledList.Recycle();
		}

		// Token: 0x06009071 RID: 36977 RVA: 0x0036EACC File Offset: 0x0036CCCC
		public bool HarvestAnyOneIntersectingPlant()
		{
			int cell = Grid.PosToCell(base.gameObject);
			bool result = false;
			for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, this.occupyArea.OccupiedCellsOffsets[i]);
				if (Grid.IsValidCell(cell2))
				{
					GameObject gameObject = Grid.Objects[cell2, 5];
					gameObject = ((gameObject != null) ? gameObject : Grid.Objects[cell2, 1]);
					if (!(gameObject == null))
					{
						Harvestable component = gameObject.GetComponent<Harvestable>();
						if (!(component == null) && component.CanBeHarvested)
						{
							component.Trigger(2127324410, BoxedBools.True);
							component.Harvest();
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04006F14 RID: 28436
		public CellOffset[] TargetOffsets;

		// Token: 0x04006F15 RID: 28437
		private OccupyArea occupyArea;
	}
}
